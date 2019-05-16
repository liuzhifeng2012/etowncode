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
    public partial class PrintTicket : System.Web.UI.Page
    {
        public int comid_temp = 0;
        public int Agentid = 0;
        public string Account = "";
        public int id = 0;
        public string company = "";
        public string yufukuan = "";
        public int Warrant_type = 0;
        public string Warrant_type_str = "";

        public string proname = "";//产品名称
        public int ordernum = 0;//订购数量
        public int unprintnum = 0;//未打印数量
        public int printnum = 0;//已打印数量

        public decimal Face_price = 0;
        public string pro_end ;
        public string pro_start;
        public string address = "";
        public string Pro_Remark = "";
        public string mobile = "";
        public DateTime subtime;
        public string agentcompany = "";

        //str4=电话，str7=预订热线

        protected void Page_Load(object sender, EventArgs e)
        {
            id = Request["orderid"].ConvertTo<int>(0);

            B2bOrderData orderdata = new B2bOrderData();
            var orderinfo = orderdata.GetOrderById(id);
            if (orderinfo == null) {
                Response.Redirect("Order.aspx");//未查询到订单
                return;
            }
            //订单数量
            ordernum = orderinfo.U_num;
            subtime = orderinfo.U_subdate;

            B2bEticketData eticketdata=new B2bEticketData();
            //已打印数量
            printnum = eticketdata.AlreadyPrintNumbyOrderid(id);
            unprintnum = ordernum - printnum;


            B2bComProData prodate = new B2bComProData();
            var proinfo = prodate.GetProById(orderinfo.Pro_id.ToString());
            if (proinfo != null)
            {
                comid_temp = proinfo.Com_id;
                proname = proinfo.Pro_name;
                Face_price=proinfo.Face_price;
                pro_end = proinfo.Pro_end.ToString("yyyy年MM月dd日");
                pro_start = proinfo.Pro_start.ToString("yyyy年MM月dd日");
                Pro_Remark = "备注: "+proinfo.pro_note;

                B2b_com_projectData projectdate = new B2b_com_projectData();
                var projectinfo = projectdate.GetProject(proinfo.Projectid,proinfo.Com_id);
                if (projectinfo != null) {
                    address = "地址:"+projectinfo.Address;
                    mobile = projectinfo.Mobile;
                    //proname = projectinfo.Projectname + proname;//不增加项目信息
                }

                if (orderinfo.Agentid != 0) {
                    var agentinfo = AgentCompanyData.GetAgentByid(orderinfo.Agentid);
                    if (agentinfo != null) {
                        mobile = agentinfo.Mobile;//读取分销电话
                        agentcompany = agentinfo.Company;
                    }
                }


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

                if (Agentid != orderinfo.Agentid)
                {
                    //订单与分销不匹配
                    Response.Redirect("order.aspx");
                }


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