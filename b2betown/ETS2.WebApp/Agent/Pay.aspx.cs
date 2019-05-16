using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.WebApp.Agent
{
    public partial class Pay : System.Web.UI.Page
    {
        public string proname = "";
        public string u_name = "";
        public string u_mobile = "";
        public string travel_date = "";
        public int buy_num = 0;
        public string u_youxiaoqi = "";
        public int p_totalprice = 0;
        public int orderid = 0;
        public int agentorderid = 0;
        public int ordertype = 0;

        public int comid_temp = 0;
        public int Agentid = 0;
        public string Account = "";
        public int id = 0;
        public string company = "";
        public string yufukuan = "";
        public int Warrant_type = 0;
        public string act = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //返回订单号
            orderid = Request["orderid"].ConvertTo<int>(0);
            agentorderid = Request["agentorderid"].ConvertTo<int>(0);
            act = Request["act"].ConvertTo<string>("");
            

            if (orderid != 0)
            {
                //根据订单id得到订单信息
                B2bOrderData dataorder = new B2bOrderData();
                B2b_order modelb2border = dataorder.GetOrderById(orderid);

                //根据产品id得到产品信息
                B2bComProData datapro = new B2bComProData();
                B2b_com_pro modelcompro = datapro.GetProById(modelb2border.Pro_id.ToString());

                //如果订单“未付款”现实支付及订单信息
                if ((int)modelb2border.Order_state == (int)OrderStatus.WaitPay)
                {

                    u_name = modelb2border.U_name.Substring(0, 1) + "**";
                    u_mobile = modelb2border.U_phone.Substring(0, 4) + "****" + modelb2border.U_phone.Substring(modelb2border.U_phone.Length - 3, 3);

                    ;
                    travel_date = modelb2border.U_traveldate.ToString();
                    buy_num = modelb2border.U_num;
                    p_totalprice = (int)(modelb2border.U_num * modelb2border.Pay_price);
                    ordertype = modelb2border.Order_type;
                    if (ordertype == 2)
                    {
                        proname = "预付款充值";
                        u_youxiaoqi = "";
                    }
                    else
                    {
                        proname = modelcompro.Pro_name;
                        u_youxiaoqi = modelcompro.Pro_start.ToString() + " - " + modelcompro.Pro_end.ToString();
                    }

                }

                comid_temp = Request["comid"].ConvertTo<int>(0);

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
                        yufukuan = "您预付款:" + agenginfo.Imprest + " 元";
                        Warrant_type = agenginfo.Warrant_type;
                    }

                }


            }
        }
    }
}