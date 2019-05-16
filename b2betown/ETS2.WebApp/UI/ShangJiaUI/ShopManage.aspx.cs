using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.Common.Business;
using FileUpload.FileUpload.Entities.Enum;

namespace ETS2.WebApp.UI.ShangJiaUI
{
    public partial class ShopManage : System.Web.UI.Page
    {
        public int menuindex = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            menuindex = Request["menuindex"].ConvertTo<int>(0);

              BindHeadPortrait();

        }

        private void BindHeadPortrait()
        {
            headPortrait.uploadFileInfo.ObjType = (int)FileObjType.Photo;
            headPortrait.displayImgId = "headPortraitImg";

            headPortrait2.uploadFileInfo.ObjType = (int)FileObjType.Photo;
            headPortrait2.displayImgId = "headPortraitImg2";
        }
    }
}