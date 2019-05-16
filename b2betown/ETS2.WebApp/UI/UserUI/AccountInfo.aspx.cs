using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FileUpload.FileUpload.Entities.Enum;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Data.InternalData;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using FileUpload.FileUpload.Entities;
using FileUpload.FileUpload.Data;
using ETS.Framework;

namespace ETS2.WebApp.UI.UserUI
{
    public partial class AccountInfo : System.Web.UI.Page
    {
        public string weixinqrcodeurl = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            BindHeadPortrait();
            ShowImgBind();
        }
        private void BindHeadPortrait()
        {
            UploadFile1.uploadFileInfo.ObjType = (int)FileObjType.Photo;
            UploadFile1.displayImgId = "Img1";
        }
        private void ShowImgBind()
        {

            var comid = UserHelper.CurrentCompany.ID;
            //根据公司id得到公司附加信息
            B2b_company_info B2bcompanyinfo = new B2bCompanyInfoData().GetCompanyInfo(comid);



            if (B2bcompanyinfo != null)
            {
                FileUploadModel identityFileUpload = new FileUploadData().GetFileById(B2bcompanyinfo.Weixinimg.ConvertTo<int>(0));
                if (identityFileUpload != null)
                {
                    weixinqrcodeurl = identityFileUpload.Relativepath;
                }
            }
        }
    }
}