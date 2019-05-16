using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using FileUpload.FileUpload.Entities.Enum;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.WebApp.WeiXin
{
    public partial class WxSingleSendMsgPage : System.Web.UI.Page
    {
        public int comid = 0;
        public string fromusername = "";

        public bool IsParentCompanyUser = true; //判断操作账户类型(总公司账户;门市账户)
        protected void Page_Load(object sender, EventArgs e)
        {
            comid = Request["comid"].ConvertTo<int>(0);
            fromusername = Request["fromusername"].ConvertTo<string>("");

            UploadFile1.uploadFileInfo.ObjType = (int)FileObjType.Photo;
            UploadFile1.displayImgId = "Img1";

            int userid = UserHelper.CurrentUserId();
            IsParentCompanyUser = new B2bCompanyManagerUserData().IsParentCompanyUser(userid);
    
        }
    }
}