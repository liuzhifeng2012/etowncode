using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.WebApp.M
{
    public partial class Login : System.Web.UI.Page
    {
        public string pass = ""; 
        protected void Page_Load(object sender, EventArgs e)
        {
            string openid="asdasdfasdfasdfas1223aad88";
            B2bCrmData userdate= new B2bCrmData();
            //var setpass = userdate.WeixinSetPass(openid, 101);



            pass = userdate.WeixinGetPass(openid, 101);

            //userdate.WeixinConPass(openid, 101);


        }
    }
}