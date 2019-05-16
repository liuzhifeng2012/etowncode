using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
 
using System.ComponentModel;

namespace ETS.Framework
{
    public static class ExtendFunc
    {
        #region ToMD5 By:ChfooZhang
        public static string ToMD5(this string encypStr)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            try
            {
                inputBye = Encoding.GetEncoding("GBK").GetBytes(encypStr);
            }
            catch (Exception ex)
            {
                inputBye = Encoding.GetEncoding("GBK").GetBytes(encypStr);
            }
            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }
        #endregion

        #region 全角半角转换
        /// <summary>
        /// 转全角的函数(SBC case)
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>全角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ToSBC(this string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }


        /// <summary> 转半角的函数(DBC case) </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>半角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ToDBC(this string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
        #endregion

        #region 截取字符串...................
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="str">要截取的字符串</param>
        /// <param name="len">截取的长度</param>
        /// <returns></returns>
        public static string CuteString(this string str, int len)
        {
            if (String.IsNullOrEmpty(str))
            {
                return null;
            }
            string temp = str;
            int j = 0;
            int k = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                if (Regex.IsMatch(temp.Substring(i, 1), @"[\u4e00-\u9fa5]+"))
                {
                    j += 2;
                }
                else
                {
                    j += 1;
                }
                if (j <= len)
                {
                    k += 1;
                }
                if (j >= len)
                {
                    return temp.Substring(0, k) + "...";
                }
            }
            return temp;

        }
        #endregion

        #region 绑定Listcontrol By:Xiaoxiong
        /// <summary>
        /// 绑定Listcontrol By:Xiaoxiong
        /// </summary>
        /// <param name="listCtrl"></param>
        /// <param name="source"></param>
        /// <param name="textKey"></param>
        /// <param name="valueKey"></param>
        public static void BindSource(this ListControl listCtrl, IEnumerable source, string textKey, string valueKey)
        {
            listCtrl.DataSource = source;
            listCtrl.DataTextField = textKey;
            listCtrl.DataValueField = valueKey;
            listCtrl.DataBind();
        }
        #endregion


        public static string TrimTxt(this TextBox txtBox)
        {
            string txt = txtBox.Text;
            if (String.IsNullOrEmpty(txt))
            {
                return string.Empty;
            }
            return txt.Trim();
        }

        public static string CombinRelativePath(this string basePath, string pathPart)
        {
            if (String.IsNullOrEmpty(pathPart))
            {
                return null;
            }
            return Path.Combine(basePath, pathPart).Replace("\\", "/");
        }

        public static string ToUnicode(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in str)
            {
                int i = (int)item;
                sb.AppendFormat("\\u{0}", i.ToString("X").PadLeft(4, '0'));
            }
            return sb.ToString();
        }

        public static string GetRegexString(this string str, string pattern)
        {
            Regex regex = new Regex(pattern);
            var matchCollection = regex.Matches(str);
            string strReg = string.Empty;
            foreach (var item in matchCollection)
            {
                strReg += item.ToString();
            }
            return strReg;
        }

        #region  把字符串转换为指定的类型,如果转换不成功,返回指定类型的默认值...
        /// <summary>
        /// 把字符串转换为指定的类型,如果转换不成功,返回指定类型的默认值.
        /// </summary>
        /// <typeparam name="T">要转换的类型</typeparam>
        /// <param name="str">要转换的字符串</param>
        /// <returns></returns>
        public static T ConvertTo<T>(this string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return default(T);
            }
            str = str.Trim();
            try
            {
                return (T)Convert.ChangeType(str, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 把字符串转换为指定的类型,如果转换不成功,返回指定类型的默认值.
        /// </summary>
        /// <typeparam name="T">要转换的类型</typeparam>
        /// <param name="str">要转换的字符串</param>
        /// <returns></returns>
        public static T ConvertTo<T>(this string str, T defaultValue)
        {
            if (String.IsNullOrEmpty(str))
            {
                return defaultValue;
            }
            str = str.Trim();
            try
            {
                return (T)Convert.ChangeType(str, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }
        #endregion


        #region  把列表控件的选定项的值转换为指定的类型,如果转换不成功返回类型的默认值..
        /// <summary>
        /// 把列表控件的选定项的值转换为指定的类型,如果转换不成功返回类型的默认值..
        /// </summary>
        /// <typeparam name="T">要转换的类型</typeparam>
        /// <param name="ctl">列表控件</param>
        /// <returns></returns>
        public static T GetValue<T>(this ListControl ctl)
        {
            var v = ctl.SelectedValue;
            return v.ConvertTo<T>();
        }
        #endregion


        #region 设置列表控件中ListItem对像的Value值
        /// <summary>
        /// 设置列表控件中ListItem对像的Value值
        /// </summary>
        /// <param name="ctl">列表控件</param>
        /// <param name="value">要显示的值</param>
        public static void SetValue(this ListControl ctl, object value)
        {
            if (value == null)
            {
                return;
            }
            if (ctl.Items.FindByValue(value.ToString()) != null)
            {
                ctl.SelectedValue = value.ToString();
            }
        }
        #endregion


        #region 把字符串以指定的字符进行分割,并返回指定的数组类型..........
        /// <summary>
        /// 把字符串以指定的字符进行分割,并返回指定的数组类型
        /// </summary>
        /// <typeparam name="T">要转换的数组类型</typeparam>
        /// <param name="str">要转换的字符串</param>
        /// <param name="splitCharacter">分割符</param>
        /// <returns></returns>
        public static List<T> ConvertTo<T>(this string str, char splitCharacter)
        {
            if (String.IsNullOrEmpty(str))
            {
                return null;
            }
            var query = from c in str.Split(splitCharacter)
                        let trimed = c.Trim()
                        where !String.IsNullOrEmpty(trimed)
                        select trimed.ConvertTo<T>();
            return query.ToList();

        }
        #endregion


        #region Md5 加密方法.......
        /// <summary>
        /// Md5 加密方法
        /// </summary>
        /// <param name="inputStr">要加密的字符串</param>
        /// <returns></returns>
        public static string MakeMD5(this string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                throw new ArgumentNullException("inputStr");
            }
            string strReturn = "";
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytInput = System.Text.UTF8Encoding.Default.GetBytes(inputStr);
                byte[] bytArray = md5.ComputeHash(bytInput);
                for (int i = 0; i < bytArray.Length; i++)
                {
                    strReturn += bytArray[i].ToString("X");
                }
                bytArray = null;
                bytInput = null;
                return strReturn;
            }
        }
        #endregion

        public static string Hex(this string inputStr)
        {
            byte[] ByteFoo = System.Text.Encoding.Default.GetBytes(inputStr);

            StringBuilder builder = new StringBuilder();

            foreach (byte b in ByteFoo)
            {
                builder.Append(b.ToString("X")); //X表示十六进制显示
            }
            return builder.ToString();
        }

        public static string SmsMD5(this string source)
        {
            StringBuilder builder = new StringBuilder();

            using (MD5 md5 = MD5.Create())
            {
                byte[] bytInput = System.Text.UTF8Encoding.Default.GetBytes(source);
                byte[] bytArray = md5.ComputeHash(bytInput);
                for (int i = 0; i < bytArray.Length; i++)
                {
                    if (bytArray[i].ToString("x").Length == 1)
                    {
                        builder.Append("0");
                    }
                    builder.Append(bytArray[i].ToString("x"));
                }
                bytArray = null;
                bytInput = null;
            }

            return builder.ToString();
        }





        public static object GetPropertyValue(this object obj, string property)
        {
            System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
            return propertyInfo.GetValue(obj, null);
        }


        public static string ToSiteHtmlString(this string source)
        {
            if (source == null)
                return "";
            else
                return source.Replace(@"&lt;", "<").Replace(@"&gt;", ">").Replace(@"&amp;", "&").Replace(@"&quot;", "\"").Replace("\r\n", "<br/>").Replace("\n", " ").Replace("\\r\\n", "<br/>").Replace("\\n", " ");//
        }

        public static string StringJoin<T>(this IEnumerable<T> source, char sepearator, Func<T, string> func)
        {
            if (source == null || source.Count() == 0)
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();


            IEnumerator<T> enumerator = source.GetEnumerator();
            //enumerator.Reset();
            // sb.Append(func(enumerator.));

            int ops = 0;

            while (enumerator.MoveNext())
            {
                if (ops > 0)
                {
                    sb.Append(sepearator);
                }
                sb.Append(func(enumerator.Current));
                ops++;
            }
            //int len = source.Count();
            //if (len > 1)
            //{
            //    for (int i = 1; i < len; i++)
            //    {

            //    }
            //}
            return sb.ToString();
        }

        /// <summary>
        /// 将英文的星期汉化
        /// </summary>
        /// <param name="week"></param>
        /// <returns></returns>
        public static string EnglishToChineseWee(this string week)
        {
            switch (week)
            {
                case "Monday":
                    return "星期一";
                case "Tuesday":
                    return "星期二";
                case "Wednesday":
                    return "星期三";
                case "Thursday":
                    return "星期四";
                case "Friday":
                    return "星期五";
                case "Saturday":
                    return "星期六";
                case "Sunday":
                    return "星期日";
                default:
                    return week;
            }
        }

        private static readonly int PAGE_DefaultSize = 20;

        public static IQueryable<T> Page<T, TKey>(this IQueryable<T> query,
                                            int pageIndex,
                                            int pageSize,
                                            Expression<Func<T, TKey>> orderBy,
                                            Expression<Func<T, bool>> condition,
                                            bool isAscending,
                                            out int itemCount)
        {
            itemCount = query.Count(condition);
            if (itemCount == 0)
            {
                return null;
            }
            if (pageSize <= 0)
            {
                pageSize = PAGE_DefaultSize;
            }
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            int rowStart = (pageIndex - 1) * pageSize;

            if (itemCount == 0)
            {
                return null;
            }
            if (isAscending)
            {
                query = query.OrderBy(orderBy);
            }
            else
            {
                query = query.OrderByDescending(orderBy);
            }
            return query.Where(condition).Skip(rowStart).Take(pageSize);
        }

        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

    }


    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;
            if (map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }
            return base.VisitParameter(p);
        }
    }
}
