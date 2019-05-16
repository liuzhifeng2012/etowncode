using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.Agent
{
    public partial class Agent_OpenTaobao_Manage : System.Web.UI.Page
    {
        public int comid_temp = 0;
        public int Agentid = 0;
        public string Account = "";
        public int id = 0;
        public string company = "";

        public int  serialnum = 0;//需要编辑的淘宝分销关联表 的序列号
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["Agentid"] != null)
            {
                //账户信息
                Agentid = Int32.Parse(Session["Agentid"].ToString());
                Account = Session["Account"].ToString();
            }
            serialnum = Request["serialnum"].ConvertTo<int>(0);
        }
    }
}