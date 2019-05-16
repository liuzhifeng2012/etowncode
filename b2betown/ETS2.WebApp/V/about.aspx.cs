using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.Common.Business;
using FileUpload.FileUpload.Data;
using ETS2.WeiXin.Service.WeiXinService.Model;

namespace ETS2.WebApp.V
{
    public partial class about : System.Web.UI.Page
    {
        public string titile;
        public string imgurl;
        public string contxt;
        public string phone;
        public string wxtype = "";
        public string phone_tel = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = int.Parse(Request.QueryString["id"].ToString());
            var info = new WxMaterialData().Getidinfo(id);



            if (info != null)
            {
                titile = info.Title.ToString() == null ? "" : info.Title.ToString();
                contxt = info.Article.ToString() == null ? "" : info.Article.ToString();
                phone_tel = info.Phone.ToString() == null ? "" : info.Phone.ToString();
                phone = "客服电话：";

                //所属栏目
                var wxdata = new WxSalePromoteTypeData().GetWxMenu(info.SalePromoteTypeid);
                if (wxdata != null)
                {
                    wxtype = wxdata.Typename;
                }

                //contxt.Replace("../UploadFile", "http://image.etown.cn/UploadFile");



                imgurl = "<img src=\"" + FileSerivce.GetImgUrl(info.Imgpath.ToString().ConvertTo<int>(0)) + "\">";


            }
        }
    }
}