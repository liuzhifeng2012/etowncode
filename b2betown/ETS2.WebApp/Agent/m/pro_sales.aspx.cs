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
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.WebApp.Agent.m
{
    public partial class pro_sales : System.Web.UI.Page
    {
        public int comid_temp = 0;
        public int Agentid = 0;
        public string Account = "";
        public int id = 0; 
        public string pickuppoint = "";//上车地点
        public string dropoffpoint = "";//下车地点

        public int iscanbook = 0;//产品是否有效

        public int addrid = 0;//常用地址id,只有实物产品用到
        public int unum = 1;//订购产品数量,只有实物产品用到

        public int servertype = 1;//产品服务类型
        public int manyspeci = 0;
        public List<B2b_com_pro_Speci> gglist = null;

        public Decimal price = 0;
        public decimal face_price = 0;

        public string nextdaydate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
        public int issetidcard = 0;//是否需要填写身份证
        //public decimal hotel_agent = 0;
        public int Agentlevel = 0;//分销级别
        protected void Page_Load(object sender, EventArgs e)
        {
            unum = Request["unum"].ConvertTo<int>(1);
            addrid = Request["addrid"].ConvertTo<int>(0);
            id = Request["id"].ConvertTo<int>(0);
            B2bComProData prodate = new B2bComProData();
            var pro = prodate.GetProById(id.ToString());
            if (pro != null)
            {
                servertype = pro.Server_type;
                comid_temp = pro.Com_id;
                pickuppoint = pro.pickuppoint;
                dropoffpoint = pro.dropoffpoint;
                price = pro.Advise_price;
                face_price = pro.Face_price;
                manyspeci = pro.Manyspeci;
                issetidcard = pro.issetidcard;
                //如果多规格读取规格
                if (manyspeci == 1)
                {
                    gglist = new B2b_com_pro_SpeciData().Getgglist(pro.Id);

                }


                if (Session["Agentid"] != null)
                {
                    //账户信息
                    Agentid = Int32.Parse(Session["Agentid"].ToString());
                     Account = Session["Account"].ToString();
                    var Warrant_type_str = "";
                    Agent_company agenginfo = AgentCompanyData.GetAgentWarrant(Agentid, comid_temp);
                    if (agenginfo != null)
                    {
                        var yufukuan = "您预付款:" + agenginfo.Imprest.ToString("0.00") + " 元";
                        var Warrant_type = agenginfo.Warrant_type;
                        if (Warrant_type == 1)
                        {
                            Warrant_type_str = "销售扣款";
                        }
                        if (Warrant_type == 2)
                        {
                            Warrant_type_str = "验证扣款";
                        }
                        Agentlevel = agenginfo.Warrant_level; ;
                        //if (pro != null)
                        //{
                        //    if (Agentlevel == 1)
                        //    {
                        //        hotel_agent = pro.Agent1_price;
                        //    }
                        //    if (Agentlevel == 2)
                        //    {
                        //        hotel_agent = pro.Agent2_price;
                        //    }
                        //    if (Agentlevel == 3)
                        //    {
                        //        hotel_agent = pro.Agent3_price;
                        //    }
                        //}

                        //重新给分销按 级别默认标注，
                        if (gglist != null)
                        {
                            for (int i = 0; i < gglist.Count(); i++)
                            {

                                if (servertype == 9)//订房 
                                {
                                    if (Agentlevel == 1)
                                    {
                                        gglist[i].speci_advise_price = gglist[i].speci_agent1_price;
                                    }
                                    if (Agentlevel == 2)
                                    {
                                        gglist[i].speci_advise_price = gglist[i].speci_agent2_price;
                                    }
                                    if (Agentlevel == 3)
                                    {
                                        gglist[i].speci_advise_price = gglist[i].speci_agent3_price;
                                    }

                                    if (price == 0 || price > gglist[i].speci_advise_price)
                                    {
                                        price = gglist[i].speci_advise_price;
                                        face_price = gglist[i].speci_face_price;
                                    }

                                }
                                else if (servertype == 10)//大巴
                                {
                                    if (Agentlevel == 1)
                                    {
                                        gglist[i].speci_advise_price = gglist[i].speci_agent1_price;
                                    }
                                    if (Agentlevel == 2)
                                    {
                                        gglist[i].speci_advise_price = gglist[i].speci_agent2_price;
                                    }
                                    if (Agentlevel == 3)
                                    {
                                        gglist[i].speci_advise_price = gglist[i].speci_agent3_price;
                                    }

                                    if (price == 0 || price > gglist[i].speci_advise_price)
                                    {
                                        price = gglist[i].speci_advise_price;
                                        face_price = gglist[i].speci_face_price;
                                    }
                                }
                                else
                                {


                                    if (Agentlevel == 1)
                                    {
                                        gglist[i].speci_advise_price = gglist[i].speci_agent1_price;
                                    }
                                    if (Agentlevel == 2)
                                    {
                                        gglist[i].speci_advise_price = gglist[i].speci_agent2_price;
                                    }
                                    if (Agentlevel == 3)
                                    {
                                        gglist[i].speci_advise_price = gglist[i].speci_agent3_price;
                                    }

                                    if (price == 0 || price > gglist[i].speci_advise_price)
                                    {
                                        price = gglist[i].speci_advise_price;
                                        face_price = gglist[i].speci_face_price;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Response.Redirect("/Agent/Default.aspx");
                    }

                }

               


                ////如果含有规格读取规格价格中最低价
                //if (manyspeci == 1)
                //{
                //    if (gglist != null)
                //    {
                //        price = 0;
                //        face_price = 0;
                //        for (int i = 0; i < gglist.Count(); i++)
                //        {

                //            if (price == 0 || price > gglist[i].speci_advise_price)
                //            {
                //                price = gglist[i].speci_advise_price;
                //                face_price = gglist[i].speci_face_price;
                //            }
                //        }
                //    }
                //}

                //作废超时未支付订单，完成回滚操作
                int rs = new B2bComProData().CancelOvertimeOrder(pro);
                if (pro.Source_type == 4)
                {
                    iscanbook = 1;
                }
                else
                {
                    iscanbook = new B2bComProData().IsYouxiao(pro.Id, pro.Server_type, pro.Pro_start, pro.Pro_end, pro.Pro_state);//判断产品是否有效：1.票务，直接判断有效期 和产品上线状态2.酒店，跟团游，当地游 则判断是否含有有效的房态/团期 以及产品上下线状态
                }
            }

        }
    }
}