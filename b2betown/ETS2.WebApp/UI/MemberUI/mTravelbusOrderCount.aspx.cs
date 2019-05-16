using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ETS2.WebApp.UI.MemberUI
{
    public partial class mTravelbusOrderCount : System.Web.UI.Page
    {
        public string nowdate = "";
        public string nextdate = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            nowdate = DateTime.Now.ToString("yyyy-MM-dd");
          
            nextdate = DateTime.Parse(nowdate).AddDays(6).ToString("yyyy-MM-dd");
        }
    }
}