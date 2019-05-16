using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FileUpload.FileUpload.Entities.Enum;
using ETS2.CRM.Service.CRMService.Data;
using FileUpload.FileUpload.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.Framework;
using ETS2.Common.Business;
using FileUpload.FileUpload.Entities;
using ETS2.PM.Service.PMService.Data;
using ETS2.WeiXin.Service.WeiXinService.Data;

namespace ETS2.WebApp.UI.ShangJiaUI
{
    public partial class Ad_image_up : System.Web.UI.Page
    {
        protected string headPortraitImgSrc = "/images/defaultThumb.png";
        protected string smallheadPortraitImgSrc = "";
        public int adid = 0;
        public int id = 0;
        public string Title = "";
        public string Link = "";
        

        protected void Page_Load(object sender, EventArgs e)
        {
            var comid = UserHelper.CurrentCompany.ID;
            id = Request["id"].ConvertTo<int>(0);
            adid = Request["adid"].ConvertTo<int>(0);
            BindHeadPortrait();
            ShowImgBind();
        }


        private void BindHeadPortrait()
        {
            headPortrait.uploadFileInfo.ObjType = (int)FileObjType.Photo;
            headPortrait.displayImgId = "headPortraitImg";

        }

        private void ShowImgBind()
        {
            //var comid = Context.Request["comid"].ConvertTo<int>(0);
            var comid = UserHelper.CurrentCompany.ID;
            if (adid != 0)
            {
                var actdata = new WxAdData();
                var pro = actdata.Getwxad(adid, comid);

                if (pro != null)
                {

                    Title = pro.Title;
                    Link = pro.Link;
                }
            }

            if (id != 0)
            {
                var actdata = new WxAdImagesData();
                var imgpro = actdata.Getwxadimages(id, adid);

                if (imgpro != null)
                {

                    FileUploadModel identityFileUpload = new FileUploadData().GetFileById(imgpro.Imageid);
                    if (identityFileUpload != null)
                    {
                        headPortraitImgSrc = identityFileUpload.Relativepath;
                    }
                }
            }
        }
    }
}