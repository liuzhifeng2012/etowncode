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

namespace ETS2.WebApp.UI.PermissionUI
{
    public partial class ImglibraryManage : System.Web.UI.Page
    {
        protected string headPortraitImgSrc = "";
        protected string smallheadPortraitImgSrc = "";
        public int usetype = 0;
        public int modelid = 0;
        public int id = 0;
        public string Title = "";
        public string linkurl = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            usetype = Request["usetype"].ConvertTo<int>(0);
            modelid = Request["modelid"].ConvertTo<int>(0);
            id = Request["id"].ConvertTo<int>(0);

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
            var comid = UserHelper.CurrentCompany.ID;
            if (id != 0)
            {
                var imagemodel = B2bCompanyImageData.GetimageLibraryByid(id);
                if (imagemodel != null)
                {
                    usetype = imagemodel.Usetype;
                    modelid = imagemodel.Modelid;

                    FileUploadModel identityFileUpload = new FileUploadData().GetFileById(imagemodel.Imgurl);
                    if (identityFileUpload != null)
                    {
                        headPortraitImgSrc = identityFileUpload.Relativepath;
                    }

                }
            }
        }
    }
}