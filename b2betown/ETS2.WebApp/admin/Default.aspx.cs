using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.Common.Business;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using FileUpload.FileUpload.Entities;
using FileUpload.FileUpload.Data;

namespace ETS2.WebApp.admin
{
    public partial class Default : System.Web.UI.Page
    {
        public string fileUrl = AppSettings.CommonSetting.GetValue("FileUpload/FileUrl").ConvertTo<string>();//文件访问地址
        public string RequestDomin = "";//访问域名
        public string Requestfile = "";//访问文件
        public string Logourl = "";
        public int comid = 0;
        public string Com_name ="";
        public string Scenic_name = "";
        public string Scenic_intro = "";
        public string Service_Phone = "";
        public string Copyright = "";





        protected void Page_Load(object sender, EventArgs e)
        {  //获取访问的域名   
            RequestDomin = Request.ServerVariables["SERVER_NAME"].ToLower();
            Requestfile = Request.ServerVariables["Url"].ToLower();

            //通过域名查询商户号
            B2b_company_info companyinfo = B2bCompanyData.AdminDomainGetComId(RequestDomin);
            if (companyinfo != null)
            {
                comid = companyinfo.Com_id;
            }


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

                        Logourl = "<img src=\"" + FileSerivce.GetImgUrl(Logourl.ConvertTo<int>(0)) + "\" alt=\"" + Scenic_name + "\" height=\"80\" />";
                        
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
    }
}