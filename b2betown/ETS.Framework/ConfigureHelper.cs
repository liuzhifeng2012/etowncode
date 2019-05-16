using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Runtime.Caching;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;

namespace ETS.Framework
{




    public class ConfigureHelper
    {
        private string settingPath;
        private string appSectionName;
        private static readonly string configCachingKey = "Caching_CustomConfig_Attriubte_{0}_{1}_{2}";
        private static readonly string configCachingValueKey = "Caching_CustomConfig_Value_{0}_{1}";
        private static readonly string configCachingCombinAttr = "Caching_CustomConfig_CombinAttr_{0}_{1}_{2}_{3}";


        private XmlDocument xDoc;

        public ConfigureHelper(string appSectionName)
        {
            this.appSectionName = appSectionName;
            this.settingPath = ConfigurationManager.AppSettings[appSectionName];
            xDoc = new XmlDocument();
        }

        public void Save()
        {
            xDoc.Save(settingPath);
        }

        public XPathNavigator GetSection(string sectionName)
        {
            //XmlDocument xDoc = new XmlDocument();
            xDoc.Load(settingPath);
            XPathNavigator navigator = xDoc.CreateNavigator();
            return navigator.SelectSingleNode(String.Concat("/configuration/", sectionName));
        }

        public IEnumerable<string> GetChildren(string sectionName, string nodeName)
        {
            var navigator = GetSection(sectionName);
            if (navigator.MoveToFirstChild())
            {
                yield return GetOuterXml(navigator, nodeName);
                while (navigator.MoveToNext())
                {
                    yield return GetOuterXml(navigator, nodeName);
                }
            }
            //yield return null;
        }

        /// <summary>
        /// 使用此方法必须把实现父节点ArrayOf<paramref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public List<T> ToEntity<T>(string sectionName)
        {
            var navitagor = GetSection(sectionName);
            if (navitagor == null)
            {
                return null;
            }

            string key = String.Format("Caching_7F30C82D-14B9-41A5-9D04-EC42B88D4415_{0}_{1}", typeof(T), appSectionName);
            return MemoryCacheHelper.Get<List<T>>(key, () =>
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                XElement xElm = XElement.Parse(navitagor.OuterXml);
                using (var reader = xElm.CreateReader())
                {
                    return (List<T>)serializer.Deserialize(reader);
                }
            }, MemoryCacheHelper.FilePolicy(settingPath));
        }

        private string GetOuterXml(XPathNavigator navigator, string localName)
        {
            if (navigator.LocalName.Equals(localName, StringComparison.OrdinalIgnoreCase))
            {
                return navigator.OuterXml;
            }
            return null;
        }

        public T GetAttribute<T>(string sectionName, string attrName)
        {
            var naviagator = GetSection(sectionName);

            return naviagator == null ? default(T) : naviagator.GetAttribute(attrName, string.Empty).ConvertTo<T>();

            //return MemoryCacheHelper.Get<T>(String.Format(configCachingKey, sectionName, attrName, appSectionName), () =>
            //{
            //    var naviagator = GetSection(sectionName);

            //    return naviagator == null ? default(T) : naviagator.GetAttribute(attrName, string.Empty).ConvertTo<T>();

            //}, MemoryCacheHelper.FilePolicy(settingPath));
        }

        public string GetValue(string sectionName)
        {

            return MemoryCacheHelper.Get<string>(String.Format(configCachingValueKey, sectionName, appSectionName), () =>
            {
                XPathNavigator navigator = GetSection(sectionName);
                return navigator == null ? null : navigator.Value;
                //return GetSection(sectionName).Value;
            },
            MemoryCacheHelper.FilePolicy(settingPath));
        }

        private string KEY_GetChildrenAttr = "Caching_4256F636-6A87-4473-9360-325E25D19DB8_{0}_{1}_{2}_{3}";

        public string GetChildAttr(string selectionName, string childName, string attrName)
        {
            string key = String.Format(KEY_GetChildrenAttr, selectionName, childName, attrName, appSectionName);
            return MemoryCacheHelper.Get<string>(key.GetHashCode().ToString(),
                () =>
                {
                    XPathNavigator navigator = GetSection(selectionName);
                    XPathNodeIterator iterator = navigator.SelectChildren(childName, string.Empty);
                    if (iterator.MoveNext())
                    {
                        return iterator.Current.GetAttribute(attrName, string.Empty);
                    }
                    return null;
                },
                MemoryCacheHelper.FilePolicy(settingPath));
        }

        public string CombineChildAttr(string selectionName, string childName, string attrName)
        {

            return MemoryCacheHelper.Get<string>(String.Format(configCachingCombinAttr, selectionName, childName, attrName, appSectionName),
                () =>
                {
                    XPathNavigator navigator = GetSection(selectionName);
                    string attr = navigator.GetAttribute(attrName, string.Empty);
                    XPathNodeIterator iterator = navigator.SelectChildren(childName, string.Empty);
                    //iterator.Current.att
                    while (iterator.MoveNext())
                    {
                        attr = String.Concat(attr, iterator.Current.GetAttribute(attrName, string.Empty));
                    }
                    return attr;
                },
                MemoryCacheHelper.FilePolicy(settingPath));
        }

    }
}
