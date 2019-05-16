using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS.Framework
{
    /// <summary>
    /// 名    称：描述枚举的属性<br/>
    /// 版    本：1.0<br/>
    /// 作    者：***<br/>
    /// 创始时间：2009-04-21 10:43:20<br/>
    /// 描    述：<br/>
    /// </summary>
    /// <remarks>
    /// ----------修改记录------------
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]

    public class EnumAttribute : Attribute
    {

        private string _name;
        private string _description;

        /// <summary>
        /// 枚举名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 枚举描述
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">枚举名称</param>
        public EnumAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">枚举名称</param>
        /// <param name="description">枚举描述</param>
        public EnumAttribute(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }
    }
}
