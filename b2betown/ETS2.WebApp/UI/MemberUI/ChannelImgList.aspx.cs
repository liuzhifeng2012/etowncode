using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;


namespace ETS2.WebApp.UI.MemberUI
{
    public partial class ChannelImgList : System.Web.UI.Page
    {
        public string channelcompanyid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            channelcompanyid = Request["channelcompanyid"].ConvertTo<string>("0");
        }
    }
}