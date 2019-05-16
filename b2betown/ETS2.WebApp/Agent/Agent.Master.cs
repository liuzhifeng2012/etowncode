using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS2.Common.Business;
using FileUpload.FileUpload.Entities;
using FileUpload.FileUpload.Data;

namespace ETS2.WebApp.Agent
{
    public partial class Agent : System.Web.UI.MasterPage
    {

        public string fileUrl = AppSettings.CommonSetting.GetValue("FileUpload/FileUrl").ConvertTo<string>();//文件访问地址

        public int comid = 0;//公司id
        public string proid = "";//产品id
        public string ordertype = "1";//订单类型 默认为1（直销订单）
        public int orderid = 0;//
        public string proname = "";



        //页面显示信息
        public string Logourl = "";//LOGO
        public string Com_name = "";//公司名称
        public string Scenic_name = "";//景点名称
        public string Scenic_intro = "";//景点简介
        public string Service_Phone = "";//服务电话
        public string Copyright = "";//服务电话
        public string RequestUrl = "";//访问网址通过访问网址判断商家，如果访问网址为空则跳转到登陆页

        //用户信息
        public string AccountId = "";
        public string AccountName = "";
        public string AccountCard = "";
        public string Today = "";

        protected void Page_Load(object sender, EventArgs e)
        {



            if (Session["AccountId"] != null)
            {
                AccountId = Session["AccountId"].ToString();
                AccountName = Session["AccountName"].ToString();
                AccountCard = Session["AccountCard"].ToString();
                Today = DateTime.Now.ToString("yyyy-MM-dd");

            }

            comid = Request["comid"].ConvertTo<int>(0);
            proid = Request["proid"].ConvertTo<string>("");
            orderid = Request["orderid"].ConvertTo<int>(0);
            ordertype = Request["ordertype"].ConvertTo<string>("1");
            RequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();

            //根据域名读取商户ID,如果没有绑定域名直接跳转后台
            B2b_company_info companyinfo = B2bCompanyData.GetComId(RequestUrl);
            if (companyinfo != null)
            {
                comid = companyinfo.Com_id;
            }

            if (RequestUrl == "agent.maikexing.com")
            {
                comid = 1305;
            }

            //如果COMID=0判断 是否为绑定的管理后台域名
            if (comid == 0) {
                B2b_company_info companyinfo1 = B2bCompanyData.GetComIdByAdmindomain(RequestUrl);
                if (companyinfo1 != null)
                {
                    comid = companyinfo1.Com_id;
                }
            }

            



            //判断商户ID（域名未绑定）
            if (comid != 0)
            {
                B2b_company com = B2bCompanyData.GetAllComMsg(comid);
                B2b_company_saleset comsetinfo = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());
                if (com != null)
                {
                    Com_name = com.Com_name;
                    Scenic_name = com.Scenic_name;
                    Scenic_intro = com.B2bcompanyinfo.Scenic_intro;

                    if (comsetinfo != null)
                    {
                        Logourl = comsetinfo.Logo;
                        if (Logourl != "")
                        {
                             
                            Logourl = "<img src=\"" + FileSerivce.GetImgUrl(Logourl.ConvertTo<int>(0)) + "\" alt=\"" + Scenic_name + "\" height=\"30\" />";
                        }

                        Service_Phone = comsetinfo.Service_Phone;
                        if (Service_Phone != "")
                        {
                            Service_Phone = "客服电话:" + Service_Phone;
                        }

                        Copyright = comsetinfo.Copyright;
                    }
                }
            }
            else
            {
                //如果没有商户ID（域名未绑定） 则跳转管理登陆页面。
                // Response.Redirect("/Manage/index1.html");
            }



        }
    }
}