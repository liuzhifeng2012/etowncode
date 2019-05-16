using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI.MemberUI
{
    public partial class ChannelCompanyList : System.Web.UI.Page
    {
        public string channeltype = "";//渠道类型
        protected void Page_Load(object sender, EventArgs e)
        {
            channeltype = Request["channeltype"].ConvertTo<string>("");
        }
    }
}