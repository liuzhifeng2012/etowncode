using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.PM.Service.PMService.Data;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.WebApp.Agent
{
    public partial class ShopCartSales : System.Web.UI.Page
    {

        public int comid_temp = 0;
        public int Agentid = 0;
        public string Account = "";

        public string company = "";
        public string yufukuan = "";
        public int Warrant_type = 0;
        public string Warrant_type_str = "";
        public string pickuppoint = "";//上车地点
        public string dropoffpoint = "";//下车地点
        public string proid = "";
        public string num = "";
        public string cartid="";

        public string id = "";
        public string id_speciid = "";
        public string temp_id = "";
        public string temp_id_speciid = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            cartid = Request["cartid"].ConvertTo<string>("");


            //去除最右侧，号
            if (cartid != "")
            {
                if (cartid.Substring(cartid.Length - 1, 1) == ",")
                {
                    cartid = cartid.Substring(0, cartid.Length - 1);
                }
            }

            var cartid_arr = cartid.Split(',');

            //如果包含规格产品
            var prospeciid = new B2bOrderData().SearchCartListBycartid(cartid);
            if (prospeciid != null)
            {

                for (int i = 0; i < prospeciid.Count; i++)
                {
                    id += prospeciid[i].Id + ",";
                    id_speciid += prospeciid[i].Speciid + ",";

                    temp_id = prospeciid[i].Id.ToString();
                    temp_id_speciid = prospeciid[i].Speciid.ToString();

                    num += prospeciid[i].U_num + ",";
                }
            }

            if (num != "")
            {
                if (num.Substring(num.Length - 1, 1) == ",")
                {
                    num = num.Substring(0, num.Length - 1);
                }
            }
            if (id != "")
            {
                if (id.Substring(id.Length - 1, 1) == ",")
                {
                    id = id.Substring(0, id.Length - 1);
                }
            }
            if (id_speciid != "")
            {
                if (id_speciid.Substring(id_speciid.Length - 1, 1) == ",")
                {
                    id_speciid = id_speciid.Substring(0, id_speciid.Length - 1);
                }
            }


            B2bComProData prodate = new B2bComProData();
            var proinfo = prodate.GetProById(temp_id);
            if (proinfo != null)
            {
                comid_temp = proinfo.Com_id;
                pickuppoint = proinfo.pickuppoint;
                dropoffpoint = proinfo.dropoffpoint;

                //作废超时未支付订单，完成回滚操作
                int rs = new B2bComProData().CancelOvertimeOrder(proinfo);
            }

           


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

        }
    }
}