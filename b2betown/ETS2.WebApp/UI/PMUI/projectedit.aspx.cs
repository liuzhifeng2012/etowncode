using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using FileUpload.FileUpload.Entities.Enum;
using ETS2.Common.Business;

namespace ETS2.WebApp.UI.PMUI
{
    public partial class projectedit : System.Web.UI.Page
    {
        public int projectid = 0;//项目id

        public string imgurl = "/images/defaultThumb.png";
        protected void Page_Load(object sender, EventArgs e)
        {
            projectid = Request["projectid"].ConvertTo<int>(0);
            BindHeadPortrait();

             
        }
        private void BindHeadPortrait()
        {
            headPortrait.uploadFileInfo.ObjType = (int)FileObjType.Photo;
            headPortrait.displayImgId = "headPortraitImg";

        }
    }
}