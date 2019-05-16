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
    public partial class ShangJiaSet3 : System.Web.UI.Page
    {
        public string domain = "";//公司域名

        protected void Page_Load(object sender, EventArgs e)
        {
            int comid = UserHelper.CurrentCompany.ID;

            domain = "shop" + comid + ".etown.cn";


        }
    }
}