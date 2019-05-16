using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.byts
{
    public partial class CardSuccess : System.Web.UI.Page
    {
        public string IDCard = "";
        public string Name_temp = "";
        public string Phone = "";
        public string Email = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Name_temp = Request["Name"].ConvertTo<string>("");
            IDCard = Request["Cardcode"].ConvertTo<string>("");
            Email = Request["Email"].ConvertTo<string>("");
            Phone = Request["Phone"].ConvertTo<string>("");
          
        }
    }
}