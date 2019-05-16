using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.JsonFactory;
using ETS2.Common.Business;
using ETS.Framework;

namespace ETS2.WebApp.WeiXin
{
    public partial class WxTemplateDetail : System.Web.UI.Page
    {
        public int id = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            id = Request["id"].ConvertTo<int>(0);
        }
    }
}