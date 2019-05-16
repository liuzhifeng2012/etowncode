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


namespace ETS2.WebApp.UI.ShangJiaUI
{
    public partial class DirectSellSetting : System.Web.UI.Page
    {
        protected string headPortraitImgSrc = "/images/defaultThumb.png";
        protected string smallheadPortraitImgSrc = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            BindHeadPortrait();
            ShowImgBind();
        }
        private void BindHeadPortrait()
        {
            headPortrait.uploadFileInfo.ObjType = (int)FileObjType.Photo;
            headPortrait.displayImgId = "headPortraitImg";

            SmallHeadPortrait.uploadFileInfo.ObjType = (int)FileObjType.Photo;
            SmallHeadPortrait.displayImgId = "SmallHeadPortraitImg";
        }
        private void ShowImgBind()
        {
            //var comid = Context.Request["comid"].ConvertTo<int>(0);
            var comid = UserHelper.CurrentCompany.ID;

            //根据公司id得到 直销设置
            B2b_company_saleset saleset = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());

            if (saleset != null)
            {
                FileUploadModel identityFileUpload = new FileUploadData().GetFileById(saleset.Logo.ConvertTo<int>(0));
                if (identityFileUpload != null)
                {
                    headPortraitImgSrc = identityFileUpload.Relativepath;
                }


                if (saleset.Smalllogo == null || saleset.Smalllogo == "" || saleset.Smalllogo == "0")
                {

                }
                else
                {
                    FileUploadModel smallidentityFileUpload = new FileUploadData().GetFileById(saleset.Smalllogo.ConvertTo<int>(0));
                    if (smallidentityFileUpload != null )
                    {
                        smallheadPortraitImgSrc = smallidentityFileUpload.Relativepath;
                    }
                }


            }
        }
    }
}