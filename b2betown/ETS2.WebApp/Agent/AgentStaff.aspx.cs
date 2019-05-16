using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.WebApp.Agent
{
    public partial class AgentStaff : System.Web.UI.Page
    {
        public int Agentid = 0;
        public string Account = "";

        //public int accountlevel = 0;//账户级别:0开户账户；1添加账户
        protected void Page_Load(object sender, EventArgs e)
        {
            //int accountid = Request["accountid"].ConvertTo<int>(0);
            //Agent_regiinfo m_account = AgentCompanyData.GetAgentAccountByid(accountid);
            //if (m_account == null)//没有分销账户直接退出
            //{
            //    Response.Redirect("login.aspx");
            //} 
            //accountlevel = m_account.AccountLevel;

            if (Session["Agentid"] != null)
            {
                //账户信息
                Agentid = Int32.Parse(Session["Agentid"].ToString());
                Account = Session["Account"].ToString();


                //查询此分销是否为开户账户如果不是则返回登录页，此页面只有开户账户才能管理
                var agentdate = AgentCompanyData.GetAgentByid(Agentid);
                if (agentdate == null)
                {
                    Response.Redirect("/Agent/login.aspx");
                }
                if (agentdate.AccountLevel != 0)
                {
                    Response.Redirect("/Agent/login.aspx");
                }


            }
        }
    }
}