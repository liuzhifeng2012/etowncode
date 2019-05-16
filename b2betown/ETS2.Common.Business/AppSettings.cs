using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.Common.Business
{
    public class AppSettings
    {
        /// <summary>
        /// 通过Settings配制
        /// </summary>
        public static ConfigureHelper CommonSetting = new ConfigureHelper("CommonScetion");
        /// <summary>
        /// 缓存文件设置
        /// </summary>
        public static ConfigureHelper CachingSetting = new ConfigureHelper("CachingSecion");
        /// <summary>
        /// 虚拟帐户流程配置
        /// </summary>
        public static ConfigureHelper VASFlowSetting = new ConfigureHelper("VASFlowSecion");
        /// <summary>
        /// 短信模板配置文件
        /// </summary>
        public static ConfigureHelper SMSTemplateSetting = new ConfigureHelper("SMSScetion");
    }
}
