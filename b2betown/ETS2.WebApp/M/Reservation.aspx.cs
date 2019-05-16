using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.WeiXin.Service.WeiXinService.Data;
using FileUpload.FileUpload.Data;
using ETS2.Common.Business;
using ETS.Framework;
using ETS2.WeiXin.Service.WeiXinService.Model;

namespace ETS2.WebApp.M
{
    public partial class Reservation : System.Web.UI.Page
    {
        protected int materialid = 0;

        public string fileUrl = AppSettings.CommonSetting.GetValue("FileUpload/FileUrl").ConvertTo<string>();//文件访问地址
        public string headPortraitImgSrc = "";

        public string title = "";
        public string thisday = "";
        public string article = "";
        public string phone = "";
        public string price = "";

        public string summary = "";

        public int id;
        public string nowdate = "";//现在日期

        public int comid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            materialid = int.Parse( Request.QueryString["materialid"].ToString());

            WxMaterial wxmaterial = new WxMaterialData().GetWxMaterial(materialid);

            nowdate = DateTime.Now.ToString("yyyy-MM-dd");

            if (wxmaterial != null)
            {
                id = wxmaterial.MaterialId;
                title = wxmaterial.Title;
                thisday = DateTime.Now.ToString("yyyy-MM-dd");
                article = wxmaterial.Article;

                phone = "电话预定：" + wxmaterial.Phone;
                price = wxmaterial.Price.ToString();

                if (price == "0.00" || price == "0")
                {
                    price = "";
                }
                else
                {
                    price = price.IndexOf(".") != -1 ? price.Substring(0, price.IndexOf(".")) : price;
                    price = "￥" + price + "元-";
                }

                comid = wxmaterial.Comid;

                summary = wxmaterial.Summary;
                

                headPortraitImgSrc = FileSerivce.GetImgUrl(wxmaterial.Imgpath.ToString().ConvertTo<int>(0));

               
            }



        }
    }
}