using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.Common.Business;
using ETS.Framework;

namespace ETS2.WebApp.WeiXin
{
    public partial class WxSet : System.Web.UI.Page
    {
        public string domain = "";//公司域名
        public string url = "";//公司url "http://shop"+comid+".etown.cn/weixin/index.aspx"
        public string token = "";//公司token 16个随机字母+数字
        protected void Page_Load(object sender, EventArgs e)
        {
            int comid = UserHelper.CurrentCompany.ID;
            url = "http://shop" + comid + ".etown.cn/weixin/index.aspx";

            domain = "shop" + comid + ".etown.cn";

            token = RandomHelper.RandCode(20);
        }
    }
}