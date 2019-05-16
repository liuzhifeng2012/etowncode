using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle.Enum;

namespace ETS2.WebApp.Agent
{
    public partial class Manage_sales : System.Web.UI.Page
    {
        public int comid_temp = 0;
        public int Agentid = 0;
        public string Account = "";
        public string company = "";
        public string yufukuan = "";
        public int Warrant_type = 0;
        public string Warrant_type_str = "";
        public int projectid = 0;
        public string projectname = "";

        public int ishaslvyoubusproorder = 0;//是否含有 旅游大巴产品订单
        public string contact_phone = "";//联系人电话
        protected void Page_Load(object sender, EventArgs e)
        {
            comid_temp = Request["comid"].ConvertTo<int>(0);
            projectid = Request["projectid"].ConvertTo<int>(0);

            B2b_company companyinfo=  B2bCompanyData.GetAllComMsg(comid_temp);
            if (companyinfo != null){
                company = companyinfo.Com_name;
                contact_phone = companyinfo.B2bcompanyinfo == null ? "" : companyinfo.B2bcompanyinfo.Tel; 
            }

            if (projectid != 0)
            {
                B2b_com_projectData projectdata = new B2b_com_projectData();
                var projectinfo = projectdata.GetProject(projectid, comid_temp);
                if (projectinfo != null)
                {
                    projectname = projectinfo.Projectname + ": ";
                }
            }

           // ishaslvyoubusproorder = new B2bOrderData().IsHasLvyoubusProOrder(comid_temp, (int)ProductServer_Type.LvyouBus);   
      


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
                else {
                    Response.Redirect("/Agent/Default.aspx");
                }
            }
        }
    }
}