using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ETS2.WebApp.UI.PMUI.ETicket
{
    public partial class Eticket_safety : System.Web.UI.Page
    {
        public string today = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            today=DateTime.Now.ToString("yyyy-MM-dd");
        }
    }
}