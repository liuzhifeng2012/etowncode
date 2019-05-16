using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using FileUpload.FileUpload.Data;
using FileUpload.FileUpload.Entities.Enum;
using ETS2.Common.Business;

namespace ETS2.WebApp.MemberShipCard
{
    public partial class MemberShipCardDetail : System.Web.UI.Page
    {
        public string nowdate = "";//现在日期

        protected string headPortraitImgSrc = "";

        public int materialid = 0;//素材id

        protected void Page_Load(object sender, EventArgs e)
        {

            nowdate = DateTime.Now.ToString("yyyy-MM-dd");

            materialid = Request["materialid"].ConvertTo<int>(0);

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

            if (UserHelper.ValidateLogin())
            {


                //根据素材id得到素材信息
                MemberShipCardMaterial material = new MemberShipCardMaterialData().GetMembershipcardMaterial(UserHelper.CurrentCompany.ID, materialid);
                if (material != null)
                {
                    var identityFileUpload = new FileUploadData().GetFileById(material.Imgpath.ToString().ConvertTo<int>(0));
                    if(identityFileUpload!=null){
                      headPortraitImgSrc = identityFileUpload.Relativepath;
                    }
                }
            }

        }
    }
}