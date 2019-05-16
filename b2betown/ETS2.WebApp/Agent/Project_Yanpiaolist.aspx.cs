using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.Agent
{
    public partial class Project_Yanpiaolist : System.Web.UI.Page
    {
        public int Id = 0;
        public int Agentid = 0;
        public string Projectname = "";
        public string startime = "";
        public string endtime = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Id = Request["Id"].ConvertTo<int>(0);
            startime = Request["startime"].ConvertTo<string>(""); ;
            endtime = Request["endtime"].ConvertTo<string>(""); ;

            if (Session["Agentid"] != null)
            {
                //账户信息
                Agentid = Int32.Parse(Session["Agentid"].ToString());
            }


        }
    }
}