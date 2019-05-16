using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.Agent
{
    public partial class Project_Count : System.Web.UI.Page
    {
        public int Id = 0;
        public int Agentid=0;
        public string Projectname = "";
        public string today = "";
        public string Yesterday = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Id = Request["Id"].ConvertTo<int>(0);
            if (Session["Agentid"] != null)
            {
                //账户信息
                Agentid = Int32.Parse(Session["Agentid"].ToString());
            }

            today = DateTime.Now.ToString("yyyy-MM-dd");
            Yesterday = DateTime.Now.ToString("yyyy") + "-" + DateTime.Now.ToString("MM") + "-01";

        }
    }
}