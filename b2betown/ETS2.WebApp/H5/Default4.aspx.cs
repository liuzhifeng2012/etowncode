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
namespace ETS2.WebApp.H5
{
    public partial class Default4 : System.Web.UI.Page
    {
        #region 辨别商家参数
        public string RequestUrl = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Trim().ToLower();
        public int comid = 0;
        #endregion

        public string title_arr = "";
        public string img_arr = "";
        public string url_arr = "";
        public string companyname = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //string u = Request.ServerVariables["HTTP_USER_AGENT"];
            //bool bo = detectmobilebrowser.HttpUserAgent(u);
            bool bo = true;

            string RequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();
            if (Domain_def.Domain_yanzheng(RequestUrl))//如果符合shop101.etown.cn的格式，则从多微信商户基本信息表中获取comid
            {

                comid = Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl).ToString());

            }
            else
            {
                B2b_company_info companyinfo = B2bCompanyData.GetComId(RequestUrl);
                if (companyinfo != null)
                {
                    comid = companyinfo.Com_id;
                  }
            }


            if (comid != 0) { 
                 B2b_company company = B2bCompanyData.GetCompany(comid);
                 if (company != null)
                 {
                     companyname = company.Com_name;
                 }
            }


            //Banner
            int totalcount = 0;
            B2bCompanyImageData imgdata = new B2bCompanyImageData();
            List<B2b_company_image> imglist = imgdata.PageGetimageList(comid, 0, out totalcount);
            if (imglist != null)
            {
                for (int i = 0; i < totalcount; i++)
                {
                    if (title_arr == "")
                    {
                        title_arr = "\\\"" + imglist[i].Title + "\\\"";
                        img_arr = "\\\"" + FileSerivce.GetImgUrl(imglist[i].Imgurl).Replace("/", "\\\\\\/") + "\\\"";
                        url_arr = "\\\"" + imglist[i].Linkurl.Replace("/", "\\\\\\/") + "\\\""; ;
                    }
                    else
                    {
                        title_arr += "," + "\\\"" + imglist[i].Title + "\\\"";
                        img_arr += "," + "\\\"" + FileSerivce.GetImgUrl(imglist[i].Imgurl).Replace("/", "\\\\\\/") + "\\\"";
                        url_arr += "," + "\\\"" + imglist[i].Linkurl.Replace("/", "\\\\\\/") + "\\\""; ;
                    }
                }


            }



        }
    }
}