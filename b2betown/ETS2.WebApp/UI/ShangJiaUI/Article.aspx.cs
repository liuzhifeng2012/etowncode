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
    public partial class Article : System.Web.UI.Page
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


        public decimal Face_price = 0;

        public string userid = "0";//用户临时 Uid 或 实际Uid 
        public int id = 0;
        public int promotetypeid = 0;//文章分类


        protected void Page_Load(object sender, EventArgs e)
        {
            RequestUrl = Request.ServerVariables["SERVER_NAME"].ToString();
            comid = Request["comid"].ConvertTo<int>(0);
            id = Request["id"].ConvertTo<int>(0);
            promotetypeid = Request["promotetypeid"].ConvertTo<int>(0);

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

            if (comid != 0)
            {

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

                    Wxfocus_url = modlecom.B2bcompanyinfo.Wxfocus_url;
                    Wxfocus_author = modlecom.B2bcompanyinfo.Wxfocus_author; ;

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


            }




        }

    }
}