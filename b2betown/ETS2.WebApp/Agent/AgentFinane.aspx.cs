using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.WebApp.Agent
{
    public partial class AgentFinane : System.Web.UI.Page
    {
        public int comid_temp = 0;
        public int Agentid = 0;
        public string Account = "";
        public string company = "";
        public string yufukuan = "";
        public int Warrant_type = 0;
        public string Warrant_type_str = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comid_temp = 106;


            B2b_company companyinfo = B2bCompanyData.GetAllComMsg(comid_temp);
            if (companyinfo != null)
            {
                company = companyinfo.Com_name;
            }
            if (Session["Agentid"] != null)
            {
                //账户信息
                Agentid = Int32.Parse(Session["Agentid"].ToString());
                Account = Session["Account"].ToString();
                Agent_company agenginfo = AgentCompanyData.GetAgentWarrant(Agentid, comid_temp);
                if (agenginfo != null)
                {
                    yufukuan = "您预付款:" + agenginfo.Imprest.ToString("0.00") + " 元";
                    Warrant_type = agenginfo.Warrant_type;
                    if (Warrant_type == 1)
                    {
                        Warrant_type_str = "销售扣款";
                    }
                    if (Warrant_type == 2)
                    {
                        Warrant_type_str = "验证扣款";
                    }

                }
                else
                {
                    Response.Redirect("/Agent/Default.aspx");
                }

            }
            else
            {
                Response.Redirect("/Agent/login.aspx");

            }
        }
    }
}