using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FileUpload.FileUpload.Entities.Enum;
using ETS.Framework;

namespace ETS2.WebApp.UI.VASUI
{
    public partial class ChanelrebateApplyDeal : System.Web.UI.Page
    {
        public int Channelid = 0;//渠道id
        protected void Page_Load(object sender, EventArgs e)
        {
            Channelid = Request["Channelid"].ConvertTo<int>(0);

            BindHeadPortrait();
        }
        private void BindHeadPortrait()
        {
            headPortrait.uploadFileInfo.ObjType = (int)FileObjType.Photo;
            headPortrait.displayImgId = "headPortraitImg";

        }
    }
}