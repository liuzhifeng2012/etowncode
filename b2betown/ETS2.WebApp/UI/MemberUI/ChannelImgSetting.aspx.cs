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


namespace ETS2.WebApp.UI.MemberUI
{
    public partial class ChannelImgSetting : System.Web.UI.Page
    {
        protected string headPortraitImgSrc = "";
        protected string smallheadPortraitImgSrc = "";
        public int typeid = 0;
        public int id = 0;
        public string Title = "";
        public string linkurl = "";
        public int channelcompanyid = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            typeid = Request["typeid"].ConvertTo<int>(0);
            id = Request["id"].ConvertTo<int>(0);
            channelcompanyid = Request["channelcompanyid"].ConvertTo<int>(0);

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
            if (id != 0)
            {
                B2b_company_image imagemodel = B2bCompanyImageData.GetimageByComid(comid, id);
                if (imagemodel != null)
                {
                    Title = imagemodel.Title;
                    linkurl = imagemodel.Linkurl;



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