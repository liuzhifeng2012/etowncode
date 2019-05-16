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
using ETS2.VAS.Service.VASService.Modle;
using ETS2.VAS.Service.VASService.Data;


namespace ETS2.WebApp.UI.ShangJiaUI
{
    public partial class Pro : System.Web.UI.Page
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

        public string weixinpass = "";//微信一次性密码

        public int shijiacha = 0;

        public string provalidatemethod;//判断 1按产品有效期，2指定有效期
        public int appointdate;//1=一星期，，2=1个月，3=3个月，4=6个月，5=一年
        public int iscanuseonsameday;//1当天可用，0当天不可用


        public DateTime pro_end;
        public DateTime pro_start;
        public string pro_youxiaoqi = "";


        public int projectid = 0;//项目id
        public string pro_name = "";

        public int iscanbook = 1;//产品是否可以预订

        public decimal childreduce = 0;//儿童减免费用
        public int pro_servertype = 1;//产品服务类型1.票务10.旅游大巴

        public string pickuppoint = "";//上车地点
        public string dropoffpoint = "";//下车地点
        public string imgurl = "";
        public string logoimg = "image/shop.png!60x60.jpg";
        public string Scenic_intro = "";

        public int Ispanicbuy = 0;
        public int Limitbuytotalnum = 0;


        public string comName = "";
        public string title = "";
        public string key = "";
        public int proclass = 0;
        public Decimal price = 0;

        public string Wxfocus_url = "";
        public string Wxfocus_author = "";

        public B2b_crm userinfo = null;

        public string openid = "";//微信号

        public int uid = 0;
        public string uip = "";

        public int buyuid = 0; //购买用户ID
        public int tocomid = 0;//来访商户COMID
        public string biaoti = "在线预订";




        public string summary = "";
        public string sumaryend = "";
        public string nowdate = "";//现在日期
        public DateTime nowtoday;

        public DateTime panic_begintime;
        public DateTime panicbuy_endtime;


        public string ordertype = "1";//订单类型 默认为1订单；2充值
        public string costprice = "";//成本价
        public string lirun_price = "";//利润

        public string remark = "";

        public int pro_num = 0;
        public int pro_max = 0;
        public int pro_min = 0;
        public string pro_explain = "";
        public string phone = "";
        public string Copyright = "";
        public int viewtop_pro = 1;

        public decimal Face_price=0;

        public string userid = "0";//用户临时 Uid 或 实际Uid 
        public int id = 0;
        public int manyspeci = 0;
        public List<B2b_com_pro_Speci> gglist = null;


        protected void Page_Load(object sender, EventArgs e)
        {
            RequestUrl = Request.ServerVariables["SERVER_NAME"].ToString();
            comid = Request["comid"].ConvertTo<int>(0);
            id = Request["id"].ConvertTo<int>(0);

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

            if (id != 0)
            {
                B2b_com_pro pro = new B2bComProData().GetProById(id.ToString());
                if (pro == null) {

                    return ;
                }
                //绿野 不显示头部
                if (comid == 2553)
                {
                    viewtop_pro = 0;
                }


                if (pro != null)
                {
                    if (pro.Com_id != comid) {
                        return ;
                    }

                    Ispanicbuy = pro.Ispanicbuy;//是否抢购或限购
                    Limitbuytotalnum = pro.Limitbuytotalnum;//限购数量

                    manyspeci = pro.Manyspeci;//
                    //票务产品，判断 是否抢购/限购，是的话 作废超时未支付订单，完成回滚操作
                    if (pro_servertype == 1)
                    {
                        if (pro.Ispanicbuy == 1 || pro.Ispanicbuy == 2)
                        {
                            int rs = new B2bComProData().CancelOvertimeOrder(pro);
                        }
                    }

                    iscanbook = new B2bComProData().IsYouxiao(pro.Id, pro.Server_type, pro.Pro_start, pro.Pro_end, pro.Pro_state);//判断产品是否有效：1.票务，直接判断有效期 和产品上线状态2.酒店，跟团游，当地游 则判断是否含有有效的房态/团期 以及产品上下线状态
                    pro_servertype = pro.Server_type;
                    pickuppoint = pro.pickuppoint;
                    dropoffpoint = pro.dropoffpoint;


                    childreduce = pro.Childreduce;
                }
                if (pro.Ispanicbuy == 1)
                {
                    panic_begintime = pro.Panic_begintime;
                    panicbuy_endtime = pro.Panicbuy_endtime;

                    nowtoday = DateTime.Now;
                    TimeSpan tss = pro.Panic_begintime - nowtoday;
                    var day = tss.Days * 24 * 3600; ;      //这是相差的天数
                    var h = tss.Hours * 3600;              //这是相差的小时数，
                    var m = tss.Minutes * 60;
                    var s = tss.Seconds;
                    shijiacha = day + h + m + s;
                }
                projectid = pro.Projectid;
                comid = pro.Com_id;
                pro_name = pro.Pro_name;
                price = pro.Advise_price;
                Face_price = pro.Face_price;

                //如果含有规格读取规格价格中最低价
                if (manyspeci == 1)
                {

                   gglist = new B2b_com_pro_SpeciData().Getgglist(pro.Id);

                    if (gglist != null)
                    {
                        price = 0;
                        Face_price = 0;
                        for (int i = 0; i < gglist.Count(); i++)
                        {

                            if (price == 0 || price > gglist[i].speci_advise_price)
                            {
                                price = gglist[i].speci_advise_price;
                                Face_price = gglist[i].speci_face_price;
                            }
                        }
                    }
                }


                imgurl = FileSerivce.GetImgUrl(pro.Imgurl);
                if (price == 0)
                {
                    price = 0;
                }
                else
                {
                    CommonFunc.OperTwoDecimal(price.ToString());
                    //price = price.IndexOf(".") != -1 ? price.Substring(0, price.IndexOf(".")) : price;                    
                }

                nowdate = DateTime.Now.ToString("yyyy-MM-dd");


                if (pro.Service_Contain != "")
                {
                    sumaryend = pro.Service_Contain;
                }

                if (pro.Service_NotContain != "")
                {
                    sumaryend = sumaryend + "</br> " + pro.Service_NotContain;
                }

                if (pro.Precautions != "")
                {
                    sumaryend = sumaryend + "</br> " + pro.Precautions;
                }


                //如果服务类型是 票务；  则备注信息中 显示 电子码使用限制
                if (pro.Server_type == 1)
                {
                    if (pro.Iscanuseonsameday == 0)//电子码当天不可用
                    {
                        sumaryend = "此产品当天预订不可用<br>" + sumaryend;
                    }
                    if (pro.Iscanuseonsameday == 1)//电子码当天可用
                    {
                        sumaryend = "此产品当天预订可用<br>" + sumaryend;
                    }
                    if (pro.Iscanuseonsameday == 2)//电子码出票2小时内不可用
                    {
                        sumaryend = "此产品出票2小时内不可用<br>" + sumaryend;
                    }
                }

                remark = pro.Pro_Remark;
                pro_num = pro.Pro_number;
                if (pro_num == 0)
                {
                    pro_max = 100;
                    pro_min = 1;
                }
                else
                {
                    pro_min = 1;
                    pro_max = pro_num;
                }
                pro_explain = pro.Pro_explain;



                #region 产品有效期判定(微信模板--门票订单预订成功通知 中也有用到)
                provalidatemethod = pro.ProValidateMethod;//判断 1按产品有效期，2指定有效期
                appointdate = pro.Appointdata;//1=一星期，，2=1个月，3=3个月，4=6个月，5=一年
                iscanuseonsameday = pro.Iscanuseonsameday;//1当天可用，0当天不可用

                DateTime pro_end = pro.Pro_end;
                //返回有效期
                pro_youxiaoqi = new B2bComProData().GetPro_Youxiaoqi(pro.Pro_start, pro.Pro_end, provalidatemethod, appointdate, iscanuseonsameday);

                #endregion





                var commodel = B2bCompanyData.GetCompany(comid);
                if (commodel != null)
                {
                    if (commodel.B2bcompanyinfo != null)
                    {
                        Wxfocus_url = commodel.B2bcompanyinfo.Wxfocus_url;
                        Wxfocus_author = commodel.B2bcompanyinfo.Wxfocus_author; ;
                    }
                }




                //查询项目电话,如果有项目电话调取项目电话
                var projectdata = new B2b_com_projectData();
                var projectmodel = projectdata.GetProject(projectid, comid);
                if (projectmodel != null)
                {
                    if (projectmodel.Mobile != "")
                    {
                        phone = projectmodel.Mobile;
                    }
                }


                //从cookie中得到微信号
                if (Request.Cookies["openid"] != null)
                {
                    openid = Request.Cookies["openid"].Value;
                }
                B2bCrmData b2b_crm = new B2bCrmData();
                if (openid != "")
                {
                    B2b_crm b2bmodle = b2b_crm.b2b_crmH5(openid, comid);
                    if (b2bmodle != null)
                    {
                        Imprest = b2bmodle.Imprest;
                        Integral = b2bmodle.Integral;
                    }
                }
            }





            //判断商户ID（域名未绑定）
            if (comid != 0)
            {
                B2b_company modlecom = B2bCompanyData.GetAllComMsg(comid);
                if (modlecom != null)
                {
                    Com_name = modlecom.Com_name;
                    title = modlecom.Com_name;
                    
                    txtServiceInfo = modlecom.B2bcompanyinfo.Serviceinfo;
                    Scenic_name = modlecom.Scenic_name;
                    scenic_address = modlecom.B2bcompanyinfo.Scenic_address;
                    coordinate = modlecom.B2bcompanyinfo.Coordinate;
                    coordinatesize = modlecom.B2bcompanyinfo.Coordinatesize;
                    weixinimg = modlecom.B2bcompanyinfo.Weixinimg;
                    weixinname = modlecom.B2bcompanyinfo.Weixinname;
                    Qq = modlecom.B2bcompanyinfo.Qq;

                }

                B2b_company_saleset pro = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());
                if (pro != null)
                {
                    if (pro.Smalllogo != null && pro.Smalllogo != "")
                    {
                        comlogo = FileSerivce.GetImgUrl(pro.Smalllogo.ConvertTo<int>(0));
                    }
                    phone = pro.Service_Phone;
                    Copyright = pro.Copyright;
                    Tel = pro.Service_Phone;
                }



                //读取首页栏目,只读取前10个栏目，太多影响打开
                var shopmenudata = new B2bCompanyMenuData();
                menulist = shopmenudata.GetMenuList(comid, 1, 10, out menutotalcount, 1);
                if (menulist != null)
                {
                    for (int i = 0; i < menutotalcount; i++)
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


            }




        }

        public string proname(string proid)
        {

            proid = new B2bComProData().GetProById(proid).Pro_name;

            return proid;
        }

        public string prodate(string proid)
        {

            proid = new B2bComProData().GetProById(proid).Pro_end.ToString("yyyy-MM-dd");

            return proid;
        }
        public string enterdate(DateTime date)
        {
            string time = date.ToString("yyyy-MM-dd");
            return time;
        }

        public string pnocode(int id)
        {
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

        public string orderstate_str(int str, int orderid)
        {
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