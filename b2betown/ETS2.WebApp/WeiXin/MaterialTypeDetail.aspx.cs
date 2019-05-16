using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.WeiXin
{
    public partial class MaterialTypeDetail : System.Web.UI.Page
    {
        public string id = "0";//父菜单id
        protected void Page_Load(object sender, EventArgs e)
        {
            id = Request["id"].ConvertTo<string>("0");
        }
    }
}