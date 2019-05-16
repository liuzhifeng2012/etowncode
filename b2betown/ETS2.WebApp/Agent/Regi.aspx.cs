using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.JsonFactory;

namespace ETS2.WebApp.Agent
{
    public partial class Regi : System.Web.UI.Page
    {
        public int comid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            string RequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();
            comid = GeneralFunc.GetComid(RequestUrl);
        }
    }
}