using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ETS2.WebApp.UI.PMUI
{
    public partial class ServerFakaStat : System.Web.UI.Page
    {
        public string startime = "";
        public string endtime = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            startime = DateTime.Now.ToString("yyyy-MM-dd");
            endtime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
        }
    }
}