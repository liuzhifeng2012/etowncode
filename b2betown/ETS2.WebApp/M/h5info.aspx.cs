using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FileUpload.FileUpload.Entities.Enum;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using FileUpload.FileUpload.Data;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.Common.Business;

namespace ETS2.WebApp.M
{
    public partial class h5info : System.Web.UI.Page
    {
        protected int materialid = 0;

        public string phone_tel = "";

        public string fileUrl = AppSettings.CommonSetting.GetValue("FileUpload/FileUrl").ConvertTo<string>();//文件访问地址
        public string headPortraitImgSrc = "";

        public string title = "";
        public string thisday = "";
        public string article = "";
        public string phone = "";
        public string price = "";

        public string summary = "";
        public int id;

        protected void Page_Load(object sender, EventArgs e)
        {

            materialid = Request["materialid"].ConvertTo<int>(0);

            MemberShipCardMaterial material = new MemberShipCardMaterialData().GetMembershipcardMaterial(materialid);

            if (material != null)
            {
                id = material.MaterialId;
                title = material.Title;
                thisday = DateTime.Now.ToString("yyyy-MM-dd");
                article = material.Article;
                phone_tel = material.Phone;
                phone = "客服电话：";
                price = material.Price.ToString();

                if (price == "0.00" || price == "0")
                {
                    price = "";
                }
                else
                {
                    price = price.IndexOf(".") != -1 ? price.Substring(0, price.IndexOf(".")) : price;
                    price = "￥" + price + "元-";
                }



                summary = material.Summary;


                headPortraitImgSrc = FileSerivce.GetImgUrl(material.Imgpath.ToString().ConvertTo<int>(0));


            }



        }
    }
}