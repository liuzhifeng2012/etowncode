using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FileUpload.FileUpload.Entities.Enum;
using ETS2.Common.Business;
using ETS.Framework;

namespace ETS2.WebApp.UI.PermissionUI
{
    public partial class Withdraw_handle : System.Web.UI.Page
    {
        public string fileurl = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            BindHeadPortrait();

            fileurl = AppSettings.CommonSetting.GetValue("FileUpload/FileUrl").ConvertTo<string>();
        }

        private void BindHeadPortrait()
        {
            headPortrait.uploadFileInfo.ObjType = (int)FileObjType.Photo;
            headPortrait.displayImgId = "headPortraitImg";

        }
    }
}