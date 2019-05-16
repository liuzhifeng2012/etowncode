using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using FileUpload.FileUpload.Entities.Enum;
using ETS2.Common.Business;
using FileUpload.FileUpload.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using FileUpload.FileUpload.Entities;


namespace ETS2.WebApp.UI.PermissionUI
{
    public partial class ModelManage : System.Web.UI.Page
    {
        public int modelid = 0;//模板id
        protected string headPortraitImgSrc = "";
        protected string headSmalltraitImgSrc = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            modelid = Request["modelid"].ConvertTo<int>(0);
            BindHeadPortrait();
            ShowImgBind(modelid);
        }
        private void BindHeadPortrait()
        {
            headPortrait.uploadFileInfo.ObjType = (int)FileObjType.Photo;
            headPortrait.displayImgId = "headPortraitImg";

            SmallHeadPortrait.uploadFileInfo.ObjType = (int)FileObjType.Photo;
            SmallHeadPortrait.displayImgId = "SmallHeadPortraitImg";

        }
        private void ShowImgBind(int id)
        {


            //根据产品id得到 产品信息
            B2bModelData modeldate = new B2bModelData();
            var proo = modeldate.GetModelById(id);

            if (proo != null)
            {
                var identityFileUpload = new FileUploadData().GetFileById(proo.Bgimage.ToString().ConvertTo<int>(0));
                if (identityFileUpload != null)
                {
                    headPortraitImgSrc = identityFileUpload.Relativepath;
                }
                if (proo.Smallimg == null || proo.Smallimg == 0)
                {

                }
                else
                {
                    FileUploadModel smallidentityFileUpload = new FileUploadData().GetFileById(proo.Smallimg.ToString().ConvertTo<int>(0));
                    if (smallidentityFileUpload != null)
                    {
                        if (smallidentityFileUpload.Id != 0)
                        {

                            headSmalltraitImgSrc = smallidentityFileUpload.Relativepath;
                        }
                    }
                }
            }

        }

    }
}