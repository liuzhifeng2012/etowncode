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

namespace ETS2.WebApp.V
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        public string price;
        public string titile;
        public string imgurl = AppSettings.CommonSetting.GetValue("FileUpload/FileUrl").ConvertTo<string>();
        public string contxt;
        public string phone;
        public string type;
        public string daohang;
        protected void Page_Load(object sender, EventArgs e)
        {
            int id = int.Parse(Request.QueryString["id"].ToString());
            var info = new WxMaterialData().Getidinfo(id);
            if (info != null)
            {
                price = info.Price.ToString() == null ? "" : info.Price.ToString();
                titile = info.Title.ToString() == null ? "" : info.Title.ToString();
                contxt = info.Article.ToString() == null ? "" : info.Article.ToString();
                phone = info.Price.ToString() == null ? "" : info.Phone.ToString();
                //图片链接
                var identityFileUpload = new FileUploadData().GetFileById(info.Imgpath.ToString().ConvertTo<int>(0));
                if (identityFileUpload != null)
                {
                    imgurl = imgurl + identityFileUpload.Relativepath;
                }
                var wspt = new WxSalePromoteTypeData().GetWxMenu(int.Parse(info.SalePromoteTypeid.ToString()));

                if (wspt.Typename.ToString() == "精选推荐")
                {
                    type = "<a href=\"top10.aspx\">精选推荐</a> > ￥" + price + "起 -- " + titile + "";
                }
                else
                {
                    type = wspt.Typename.ToString();
                }
            }
            else
            {
                type = "不存在文章详细，<input type=\"button\" onclick=\"javascript:history.go(-1)\" value=\"返回上一页\" />";
            }
        }
    }
}