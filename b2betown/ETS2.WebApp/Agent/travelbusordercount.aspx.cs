using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.PM.Service.PMService.Data;

namespace ETS2.WebApp.Agent
{
    public partial class travelbusordercount : System.Web.UI.Page
    {
        public string nowdate = "";
        public string monthdate = "";

        public int servertype = 0;//服务类型
        public int orderstate_paysuc = (int)OrderStatus.HasFin;//订单类型：处理成功
        public int paystate_haspay = (int)PayStatus.HasPay;//支付类型:已经支付

        public int comid_temp = 0;
        public int Agentid = 0;
        public string Account = "";
        public string company = "";
        public string yufukuan = "";
        public int Warrant_type = 0;
        public string Warrant_type_str = "";
        public int ishaslvyoubusproorder = 0;//是否含有 旅游大巴产品订单
        public string contact_phone = "";//联系人电话
        protected void Page_Load(object sender, EventArgs e)
        { 
            DateTime gooutdate = Request["gooutdate"].ConvertTo<DateTime>(DateTime.Now);
            nowdate = gooutdate.ToString("yyyy-MM-dd");
            monthdate = DateTime.Parse(nowdate).AddDays(6).ToString("yyyy-MM-dd");

            servertype = (int)ProductServer_Type.LvyouBus;

            comid_temp = Request["comid"].ConvertTo<int>(0);
            if (comid_temp == 0)
            {
                Response.Redirect("/Agent/Default.aspx");
            }
            ishaslvyoubusproorder = new B2bOrderData().IsHasLvyoubusProOrder(comid_temp, (int)ProductServer_Type.LvyouBus);   
      
            B2b_company companyinfo = B2bCompanyData.GetAllComMsg(comid_temp);
            if (companyinfo != null)
            {
                company = companyinfo.Com_name;
                contact_phone = companyinfo.B2bcompanyinfo == null ? "" : companyinfo.B2bcompanyinfo.Tel;
    
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
            
        }
    }
}