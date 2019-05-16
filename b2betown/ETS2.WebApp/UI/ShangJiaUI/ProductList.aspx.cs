using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.Common.Business;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Modle.Enum;

namespace ETS2.WebApp.UI.ShangJiaUI
{
    public partial class ProList : System.Web.UI.Page
    {
        public int AccountId = 0;
        public string AccountName = "";
        public string AccountCard = "";
        public string AccountEmail = "";
        public string AccountWeixin = "";
        public string Accountphone = "";
        public decimal Imprest = 0;
        public decimal Integral = 0;
        public string AccountPass = "";

        public string txtServiceInfo = "";//商家介绍
        public int comid = 0;//公司id
        public string Scenic_name = "";
        public string scenic_address = "";
        public string coordinate = "";
        public int coordinatesize = 8;

        public string weixin = "";
        public string Com_name = "";
        public string RequestUrl = "";//访问网址通过访问网址判断商家，如果访问网址为空则跳转到登陆页
        //微信
        public string weixinimg = "";
        public string weixinname = "";
        public string Qq = "";
        public string Tel = "";
        public string comlogo = "";
        public IList<B2b_com_project> projectlist;
        public List<B2b_company_menu> menulist;
        public int menutotalcount = 0;
        public int porjectcount = 0;
        public string Copyright = "";
        public int menuindex = 0;

        public string userid = "0";//用户临时 Uid 或 实际Uid 
        public int viewtop = 1; //头部及左侧相关显示控制
        protected void Page_Load(object sender, EventArgs e)
        {
            //如果判断手机浏览器 跳转手机版
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            bool bo = detectmobilebrowser.HttpUserAgent(u);
            if (bo) {
                Response.Redirect("/h5/order/");
            }


            RequestUrl = Request.ServerVariables["SERVER_NAME"].ToString();
            comid = Request["comid"].ConvertTo<int>(0);
            menuindex = Request["menuindex"].ConvertTo<int>(0);


            B2b_company_info companyinfo = B2bCompanyData.GetComId(RequestUrl);
            if (companyinfo != null)
            {
                comid = companyinfo.Com_id;
            }
            else
            { //判定是否为自助域名规则安 shop1.etown.cn
                if (Domain_def.Domain_yanzheng(RequestUrl))
                {
                    comid = Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl));
                }
            }


            //绿野 不显示头部
            if (comid == 2553)
            {
                viewtop = 0;
            }

            //判断商户ID（域名未绑定）
            if (comid != 0)
            {
                B2b_company modlecom = B2bCompanyData.GetAllComMsg(comid);
                if (modlecom != null)
                {
                    Com_name = modlecom.Com_name;
                    txtServiceInfo = modlecom.B2bcompanyinfo.Serviceinfo;
                    Scenic_name = modlecom.Scenic_name;
                    scenic_address = modlecom.B2bcompanyinfo.Scenic_address;
                    coordinate = modlecom.B2bcompanyinfo.Coordinate;
                    coordinatesize = modlecom.B2bcompanyinfo.Coordinatesize;
                    weixinimg = modlecom.B2bcompanyinfo.Weixinimg;
                    weixinname = modlecom.B2bcompanyinfo.Weixinname;
                    Qq=modlecom.B2bcompanyinfo.Qq;
                    

                }

                B2b_company_saleset pro = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());
                if (pro != null)
                {
                    if (pro.Smalllogo != null && pro.Smalllogo != "")
                    {
                        comlogo = FileSerivce.GetImgUrl(pro.Smalllogo.ConvertTo<int>(0));
                    }
                    Copyright = pro.Copyright;
                    Tel = pro.Service_Phone;
                }



                //读取首页栏目,只读取前10个栏目，太多影响打开
                var shopmenudata = new B2bCompanyMenuData();
                menulist = shopmenudata.GetMenuList(comid, 1, 10, out menutotalcount, 1,menuindex);
                if (menulist != null)
                {
                    for (int i = 0; i < menulist.Count; i++)
                    {
                        menulist[i].Imgurl_address = FileSerivce.GetImgUrl(menulist[i].Imgurl);
                    }
                }

                //如果没有栏目读取项目
                if (menutotalcount == 0)
                {
                    var prodata = new B2b_com_projectData();
                    projectlist = prodata.Projectpagelist(comid.ToString(), 1, 10, "1", out porjectcount, "");
                }

            }


            int totalcount = 0;
            if (Session["AccountId"] != null)
            {
                //账户信息
                AccountId = Int32.Parse(Session["AccountId"].ToString());
                AccountName = Session["AccountName"].ToString();
                AccountCard = Session["AccountCard"].ToString();
                RequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();

                //会员信息
                B2bCrmData dateuser = new B2bCrmData();
                B2b_crm modeluser = dateuser.Readuser(AccountId, comid);

                if (modeluser != null)
                {
                    AccountWeixin = modeluser.Weixin;
                    AccountEmail = modeluser.Email;
                    Accountphone = modeluser.Phone;
                    Imprest = modeluser.Imprest;
                    Integral = modeluser.Integral;
                    //密码
                    AccountPass = modeluser.Password1;
                    //微信关注
                    weixin = modeluser.Weixin == "" ? "未关注" : "已关注";
                }

                //用户订单
                //List<B2b_order> orderlist = new B2bOrderData().ComOrderPageList(comid,1, 100, AccountId, out totalcount);
                //Rporder.DataSource = orderlist;
                //Rporder.DataBind();

            }


            

        }

        public string proname(string proid){

            proid = new B2bComProData().GetProById(proid).Pro_name;

            return proid;
        }

        public string prodate(string proid)
        {

            proid = new B2bComProData().GetProById(proid).Pro_end.ToString("yyyy-MM-dd");

            return proid;
        }
        public string enterdate(DateTime date) {
            string time = date.ToString("yyyy-MM-dd");
            return time;
        }

        public string pnocode(int id) {
            string pno = "";
            if (id != 0)
            {
                //根据订单信息得到产品详情
                B2b_com_pro proo = new B2bComProData().GetProByOrderID(id);
                if (proo != null)
                {
                   int sourcetype = proo.Source_type;
                    if (sourcetype == 1)
                    {
                        //根据订单号得到电子码信息
                        B2b_eticket eticket = new B2bEticketData().SelectOrderid(id);
                        if (eticket != null)
                        {
                            pno = eticket.Pno;
                        }
                    }
                    if (sourcetype == 2)
                    {
                        B2b_order order = new B2bOrderData().GetOrderById(id);
                        pno = order.Pno;
                    }
                }
            }
            return pno;
        }

        public string orderstate_str(int str,int orderid) {
            string order = "";
            if (str != 0)
            {
                order = EnumUtils.GetName((OrderStatus)str);
                if (str == 1)
                {
                    order = " <a class=\"a\" href=\"/ui/vasui/pay.aspx?orderid=" + orderid + "\">未付款</a>";
                }
            }
            return order;
        }


    }
}