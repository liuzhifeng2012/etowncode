using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.Common.Business;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS.Framework;

namespace ETS2.WebApp.H5.manage
{
    public partial class Default5 : System.Web.UI.Page
    {

        #region 辨别商家参数
        public string RequestUrl = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Trim().ToLower();
        public int comid = 0;
        #endregion

        public string title_arr = "";
        public string img_arr = "";
        public string url_arr = "";
        public string companyname = "";
        public string tel = "";
        public string bannerstr = "<ol></ol>";
        public string bannerolstr = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            comid = Request["comid"].ConvertTo<int>(0);
              
            if (comid != 0)
            {
                B2b_company company = B2bCompanyData.GetCompany(comid);
                if (company != null)
                {
                    companyname = company.Com_name;
                    tel = company.B2bcompanyinfo.Tel;
                }
            }


            //Banner
            int totalcount = 0;
            B2bCompanyImageData imgdata = new B2bCompanyImageData();
            List<B2b_company_image> imglist = imgdata.PageGetimageList(comid, 0, out totalcount);
            if (imglist != null)
            {

                bannerstr = "<ul> ";
                bannerolstr = " <ol>";


                for (int i = 0; i < totalcount; i++)
                {
                    bannerstr += "<li><a onclick=\"return false;\">	<img src=\"" + FileSerivce.GetImgUrl(imglist[i].Imgurl) + "\" alt=\"" + imglist[i].Title + "\" style=\"width:100%;\" /></a></li>";
                    if (i == 0)
                    {
                        bannerolstr += "<li class=\"on\"></li>";
                    }
                    else
                    {
                        bannerolstr += "<li ></li>";
                    }
                }

                bannerstr += "</ul>" + bannerolstr + "</ol>";
            }



        }
    }
}