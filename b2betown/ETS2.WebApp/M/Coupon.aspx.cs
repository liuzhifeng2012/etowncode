using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.Member.Service.MemberService.Data;
using ETS2.Member.Service.MemberService.Model;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.Common.Business;
using ETS2.WeiXin.Service.WeiXinService.Data;
using FileUpload.FileUpload.Data;
using FileUpload.FileUpload.Entities;

namespace ETS2.WebApp.M
{
    public partial class Coupon : System.Web.UI.Page
    {
        public string openid = "";//微信传递过来字符串
        public decimal cardcode = 0;
        public string aid = "";
        public string act = "";
        public int comid = 0;//公司id
        public string RequestUrl = "";
        public int totalcount = 0;

        public string Actend = "";
        public string Title = "";
        public string Atitle = "";
        public string Remark = "";
        public string Useremark = "";
        public string Usetitle = "";




        public int usestate = 0;//使用状态



        //获得公司logo地址和公司名称
        public string comname = "";//公司名称
        public string comlogo = "";//公司logo地址

        protected void Page_Load(object sender, EventArgs e)
        {
            openid = Request["openid"];
            act = Request["act"];
            aid = Request["aid"].ToString();

            RequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();


            //如果SESSION有值，进行赋值
            if (openid == "" && Session["Openid"] != null)
            {
                openid = Session["Openid"].ToString();
            }

            //根据域名读取商户ID,如果没有绑定域名直接跳转后台
            if (Domain_def.Domain_yanzheng(RequestUrl))//如果符合shop101.etown.cn的格式，则从多微信商户基本信息表中获取comid
            {
                //先通过正则表达式获取COMid
                comid = Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl).ToString());
                if (comid == 0)
                {
                    comid = new WeiXinBasicData().GetWeiXinBasicByDomain(RequestUrl).Comid;
                }
            }
            else
            {
                B2b_company_info companyinfo = B2bCompanyData.GetComId(RequestUrl);
                if (companyinfo != null)
                {
                    comid = companyinfo.Com_id;
                }
            }

            if (comid != 0)
            {
                //根据公司id得到公司logo地址和公司名称
                comname = B2bCompanyData.GetCompany(comid).Com_name;
                B2b_company_saleset pro = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());

                if (pro != null)
                {
                    comlogo=FileSerivce.GetImgUrl(pro.Logo.ConvertTo<int>(0));
                }
            }
            //如果账户登陆状态则读取用户信息
            if (Session["AccountId"] != null)
            {
                //获取用户卡号
                B2bCrmData crmdate = new B2bCrmData();
                B2b_crm crmmodle = crmdate.Readuser(Int32.Parse(Session["AccountId"].ToString()), comid);

                if (crmmodle != null)
                {
                    cardcode = crmmodle.Idcard;
                }

                //获取优惠劵信息 对已领的优惠劵和未领的优惠劵
                if (aid != "")
                {
                    MemberActivityData actdata = new MemberActivityData();
                    if (act == "A")//已领的优惠劵
                    {
                        Member_Activity actmodel = actdata.AccountActInfo(Int32.Parse(aid), Int32.Parse(Session["AccountId"].ToString()), comid, out totalcount);
                        if (actmodel != null)
                        {
                            Actend = actmodel.Actend.ToString("yyyy-MM-dd");
                            Title = actmodel.Title;
                            usestate = actmodel.Usestate;
                            Atitle = actmodel.Atitle;
                            Remark = actmodel.Remark.Replace(((char)10).ToString(), "<br />");
                            //Useremark = actmodel.Useremark;
                            Usetitle = actmodel.Usetitle.Replace(((char)10).ToString(), "<br />");
                        }
                    }
                    else
                    {//未领取的
                        Member_Activity actmodel = actdata.UnAccountActInfo(Int32.Parse(aid), 0, comid, out totalcount);
                        if (actmodel != null)
                        {
                            Actend = actmodel.Actend.ToString("yyyy-MM-dd");
                            Title = actmodel.Title;
                            Atitle = actmodel.Atitle;
                            Remark = actmodel.Remark.Replace(((char)10).ToString(), "<br />");
                            //Useremark = actmodel.Useremark.Replace(((char)10).ToString(), "<br />");
                            Usetitle = actmodel.Usetitle;
                            usestate = 6;

                        }

                    }


                    //活动使用门市
                    MemberChannelcompanyData chandata = new MemberChannelcompanyData();
                    var list = chandata.GetUnitListselected(int.Parse(aid));
                    if (list != null) {
                        for (int i=0 ;i<list.Count();i++){
                            Useremark += list[i].Companyname + " <br>";
                        }
                    }

                }
                else
                {

                    Response.Redirect("/M/Default.aspx");
                }
            }
            else
            {//非会员看未领取的
                if (aid != "")
                {
                    MemberActivityData actdata = new MemberActivityData();
                    Member_Activity actmodel = actdata.UnAccountActInfo(Int32.Parse(aid), 0, comid, out totalcount);
                    if (actmodel != null)
                    {
                        Actend = actmodel.Actend.ToString("yyyy-MM-dd");
                        Title = actmodel.Title;
                        Atitle = actmodel.Atitle;
                        Remark = actmodel.Remark.Replace(((char)10).ToString(), "<br />");
                        Useremark = actmodel.Useremark.Replace(((char)10).ToString(), "<br />");
                        Usetitle = actmodel.Usetitle.Length > 14 ? actmodel.Usetitle.Substring(0, 13) + "." : actmodel.Useremark;
                        usestate = 5;

                    }
                }

            }

        }
    }
}