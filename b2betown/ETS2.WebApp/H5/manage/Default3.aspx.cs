using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.Common.Business;
using ETS.JsonFactory;
using ETS.Framework;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.WebApp.H5.manage
{
    public partial class Default3 : System.Web.UI.Page
    {
        #region 辨别商家参数
        public string RequestUrl = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Trim().ToLower();
        public int comid = 0;
        #endregion

        public string title_arr = "";
        public string img_arr = "";
        public string url_arr = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            comid = Request["comid"].ConvertTo<int>(0);
            


            //Banner
            int totalcount = 0;
            var Linkurl = "/ui/shangjiaui/H5Setlist.aspx";
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
                        url_arr = "\\\"" + Linkurl.Replace("/", "\\\\\\/") + "\\\""; ;
                    }
                    else
                    {
                        title_arr += "," + "\\\"" + imglist[i].Title + "\\\"";
                        img_arr += "," + "\\\"" + FileSerivce.GetImgUrl(imglist[i].Imgurl).Replace("/", "\\\\\\/") + "\\\"";
                        url_arr += "," + "\\\"" + Linkurl.Replace("/", "\\\\\\/") + "\\\""; ;
                    }
                }


            }



        }
    }
}