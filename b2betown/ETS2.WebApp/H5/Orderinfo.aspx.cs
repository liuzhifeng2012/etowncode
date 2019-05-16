using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.Common.Business;
using FileUpload.FileUpload.Data;
using ETS.Framework;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;

namespace ETS2.WebApp.H5
{
    public partial class Orderinfo : System.Web.UI.Page
    {
        //public string fileUrl = AppSettings.CommonSetting.GetValue("FileUpload/FileUrl").ConvertTo<string>();//文件访问地址
        public string headPortraitImgSrc = "../Images/weilxlogo.gif";

        public int comid = 0;
        public string title = "";
        public string price = "";
        public string advise_price = "";

        public string summary = "";
        public int id=0;
        public string nowdate = "";//现在日期

        public string author = "";

        public string remark = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            id = Request["id"].ConvertTo<int>(0);
            if (id != 0)
            {
                B2b_com_pro pro = new B2bComProData().GetProById(id.ToString());

                comid = pro.Com_id;
                title = pro.Pro_name;
                if (title.Length > 7) {
                    title = title.Substring(0, 7);
                }
                price = pro.Face_price.ToString();
                if (price == "0.00" || price == "0")
                {
                    price = "";
                }
                else
                {
                    price = price.IndexOf(".") != -1 ? price.Substring(0, price.IndexOf(".")) : price;
                    price = "￥" + price;
                }
                advise_price = "￥" + pro.Advise_price.ToString();
                nowdate = DateTime.Now.ToString("yyyy-MM-dd");
                summary = pro.Pro_Remark;
                if (summary.Length > 50)
                {
                    summary = summary.Insert(50, "<br/>");
                }
                if (summary.Length > 100)
                {
                    summary = summary.Substring(0, 100) + "...";
                }
                author = pro.Precautions;
                remark = pro.Pro_Remark;
            }

            //materialid = Request["materialid"].ConvertTo<int>(0);

            //WxMaterial wxmaterial = new WxMaterialData().GetWxMaterial(materialid);
            //nowdate = DateTime.Now.ToString("yyyy-MM-dd");

            //if (wxmaterial != null)
            //{
            //    id = wxmaterial.MaterialId;
            //    title = wxmaterial.Title;
            //    thisday = DateTime.Now.ToString("yyyy-MM-dd");
            //    article = wxmaterial.Article;

            //    phone_tel = wxmaterial.Phone;
            //    phone = "电话预定：";
            //    price = wxmaterial.Price.ToString();
            //    datetime = wxmaterial.Operatime.ToString("yyyy-MM-dd");

            //    if (price == "0.00" || price == "0")
            //    {
            //        price = "";
            //    }
            //    else
            //    {
            //        price = price.IndexOf(".") != -1 ? price.Substring(0, price.IndexOf(".")) : price;
            //        price = "￥" + price;
            //    }

            //   //标签
            //    author = wxmaterial.Author;

            //    summary = wxmaterial.Summary;
            //    if (summary.Length > 50)
            //    {
            //        summary = summary.Insert(50, "<br/>");
            //    }
            //    if (summary.Length > 100)
            //    {
            //        summary = summary.Substring(0,100)+"...";
            //    }
            //    var identityFileUpload = new FileUploadData().GetFileById(wxmaterial.Imgpath.ToString().ConvertTo<int>(0));

            //    if (identityFileUpload.Relativepath != "")
            //    {

            //        headPortraitImgSrc = fileUrl + identityFileUpload.Relativepath;

            //    }
            //    else
            //    {
            //        headPortraitImgSrc = "";
            //    }
            //}
        }
    }
}