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
using ETS2.Common.Business;
using ETS2.PM.Service.PMService.Modle;


namespace ETS2.WebApp.Agent
{
    public partial class Pro_sales : System.Web.UI.Page
    {
        public int comid_temp = 0;
        public int Agentid = 0;
        public string Account = "";
        public int id = 0;
        public string company = "";
        public string yufukuan = "";
        public int Warrant_type = 0;
        public string Warrant_type_str = "";
        public string pickuppoint = "";//上车地点
        public string dropoffpoint = "";//下车地点
        public string contact_phone = "";//联系人电话

        public int iscanbook = 0;//产品是否有效
        public string comlogo="";

        public int manyspeci = 0;
        public List<B2b_com_pro_Speci> gglist = null;//规格列表
        public decimal face_price = 0;
        public decimal price = 0;
        public int Server_type = 0;
        //public decimal hotel_agent = 0;//团期中加了分销返还后不再适用
        public int  Agentlevel=0;//分销级别
        public int issetidcard = 0;//是否需要填写身份证
        protected void Page_Load(object sender, EventArgs e)
        {
            id = Request["id"].ConvertTo<int>(0);
            B2bComProData prodate = new B2bComProData();
            var proinfo = prodate.GetProById(id.ToString());
            if (proinfo != null)
            {
                comid_temp = proinfo.Com_id;
                pickuppoint = proinfo.pickuppoint;
                dropoffpoint = proinfo.dropoffpoint;
                Server_type = proinfo.Server_type;
                manyspeci = proinfo.Manyspeci;
                face_price = proinfo.Face_price;
                price = proinfo.Advise_price;
                issetidcard=proinfo.issetidcard;
                //如果多规格读取规格
                if (manyspeci == 1)
                {
                    gglist = new B2b_com_pro_SpeciData().Getgglist(proinfo.Id);
                }


                //作废超时未支付订单，完成回滚操作
                int rs = new B2bComProData().CancelOvertimeOrder(proinfo);
                if (proinfo.Source_type == 4)
                {
                    iscanbook = 1;
                }
                else
                {
                    iscanbook = new B2bComProData().IsYouxiao(proinfo.Id, proinfo.Server_type, proinfo.Pro_start, proinfo.Pro_end, proinfo.Pro_state);//判断产品是否有效：1.票务，直接判断有效期 和产品上线状态2.酒店，跟团游，当地游 则判断是否含有有效的房态/团期 以及产品上下线状态
                }
            }
            B2b_company companyinfo = B2bCompanyData.GetAllComMsg(comid_temp);
            if (companyinfo != null)
            {
                company = companyinfo.Com_name;
                contact_phone = companyinfo.B2bcompanyinfo == null ? "" : companyinfo.B2bcompanyinfo.Tel; 
            }

            B2b_company_saleset pro = B2bCompanySaleSetData.GetDirectSellByComid(comid_temp.ToString());
            if (pro != null)
            {
                if (pro.Smalllogo != null && pro.Smalllogo != "")
                {
                    comlogo = FileSerivce.GetImgUrl(pro.Smalllogo.ConvertTo<int>(0));
                }
            }


            if (Session["Agentid"] != null)
            {
                //账户信息
                Agentid = Int32.Parse(Session["Agentid"].ToString());
                Account = Session["Account"].ToString();

                Agent_company agenginfo = AgentCompanyData.GetAgentWarrant(Agentid, comid_temp);
                if (agenginfo != null) {
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
                   Agentlevel = agenginfo.Warrant_level; ;

                    //if (proinfo != null)
                    //{
                    //    if (Agentlevel == 1)
                    //    {
                    //        hotel_agent = proinfo.Agent1_price;
                    //    }
                    //    if (Agentlevel == 2)
                    //    {
                    //        hotel_agent = proinfo.Agent2_price;
                    //    }
                    //    if (Agentlevel == 3)
                    //    {
                    //        hotel_agent = proinfo.Agent3_price;
                    //    }
                    //}


                    //重新给分销按 级别默认标注，
                    if (gglist != null)
                    {


                        price = 0;
                        face_price = 0;
                        for (int i = 0; i < gglist.Count(); i++)
                        {

                            if (Server_type == 9)//订房 
                            {
                                if (Agentlevel == 1)
                                {
                                    gglist[i].speci_advise_price = gglist[i].speci_agent1_price;
                                }
                                if (Agentlevel == 2)
                                {
                                
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
                            else if (Server_type == 10)//大巴
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
                            else { 
                            
                            
                                if (Agentlevel == 1) {
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
        }
    }
}