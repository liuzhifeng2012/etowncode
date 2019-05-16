using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.JsonFactory;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Member.Service.MemberService.Data;

namespace ETS2.WebApp.M
{
    public partial class Weixin : System.Web.UI.Page
    {
        public string openid = "";
        public string weixinpass = "";//微信随机密码（只有一次有效）
        public string errinfo = ""; //返回微信登陆错误信息
        public string RequestUrl = "";//访问网址通过访问网址判断商家，如果访问网址为空则跳转到登陆页
        public int comid = 0;
        public int AccountId = 0;
        public decimal Imprest = 0;
        public decimal Integral = 0;

        public string AccountWeixin = "";
        public string AccountEmail = "";
        public string Accountphone = "";
        public string AccountCard = "";
        public string AccountName = "";

        public decimal Servercard = 0;
        public string Servername = "";//服务专员姓名
        public string Servermobile = "";//服务专员手机


        protected void Page_Load(object sender, EventArgs e)
        {
            openid = Request["openid"];
            weixinpass = Request["weixinpass"];

            //如果SESSION有值，进行赋值
            if (openid != "" && Session["Openid"] != null) {
                openid=Session["Openid"].ToString();
            }



            //获得商户ID
            RequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();
            B2b_company_info companyinfo = B2bCompanyData.GetComId(RequestUrl);
            if (companyinfo != null)
            {
                comid = companyinfo.Com_id;
                Session["Com_id"] = comid;
            }


            //已登录直接跳转
            if (Session["AccountId"] != null)
            {
                AccountId = Int32.Parse(Session["AccountId"].ToString());
                B2bCrmData dateuser1 = new B2bCrmData();
                B2b_crm modeluser = dateuser1.Readuser(AccountId, comid);

                if (modeluser != null)
                {
                    AccountWeixin = modeluser.Weixin;
                    AccountEmail = modeluser.Email;
                    Accountphone = modeluser.Phone;
                    AccountName = modeluser.Name;
                    AccountCard = modeluser.Idcard.ToString();
                    Servercard = modeluser.Servercard;
                    Integral = modeluser.Integral;
                    Imprest = modeluser.Imprest;

                }

                //服务专员信息,服务专员ID
                if (Servercard != 0)
                {
                    MemberChannelData channeldate = new MemberChannelData();
                    Member_Channel channelmode2 = channeldate.GetChannelDetail(Int32.Parse(Servercard.ToString()));
                    if (channelmode2 != null)
                    {
                        Servername = channelmode2.Name;
                        Servermobile = channelmode2.Mobile;
                    }
                }
            }
           


        }
    }
}