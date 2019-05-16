using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Common.Business;


namespace ETS2.WebApp.H5
{
    public partial class Hotelinfo : System.Web.UI.Page
    {
        public int comid = 0;
        public string title = "";
        public string article = "";
        public string merchant_intro = "";//商家介绍
        public string serviceinfo = "";
        public string Scenic_Takebus = "";
        public string Scenic_Drivingcar = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            bool bo = detectmobilebrowser.HttpUserAgent(u);

            comid = Request["id"].ConvertTo<int>(0);
            if (comid != 0)
            {
                //if (bo == false)
                //{
                //    if (comid == 101)
                //    {
                //        Response.Redirect("http://vctrip.etown.cn/");
                //    }
                //    Response.Redirect("http://shop" + comid + ".etown.cn");
                //}
                B2b_company_info info = new B2bCompanyInfoData().GetCompanyInfo(comid);

                B2b_company com = B2bCompanyData.GetCompany(comid);
                if (com != null)
                {
                    title = com.Com_name;
                }
                if (info != null)
                {
                    article = info.Scenic_intro;
                    merchant_intro = info.Merchant_intro;
                    if (merchant_intro == "")//如果商家介绍为空的话，赋值景区介绍
                    {
                        merchant_intro = info.Scenic_intro;
                    }
                    serviceinfo = info.Serviceinfo;
                    Scenic_Takebus = info.Scenic_Takebus;
                    Scenic_Drivingcar = info.Scenic_Drivingcar;
                }
            }
        }
    }
}