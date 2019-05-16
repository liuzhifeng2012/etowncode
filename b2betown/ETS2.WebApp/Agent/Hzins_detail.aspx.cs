using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.Agent
{
    public partial class Hzins_detail : System.Web.UI.Page
    {
        public int orderid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            orderid = Request["orderid"].ConvertTo<int>(0);
        }
    }
}