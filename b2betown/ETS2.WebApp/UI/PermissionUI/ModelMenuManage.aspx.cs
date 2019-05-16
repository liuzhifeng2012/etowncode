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


namespace ETS2.WebApp.UI.PermissionUI
{
    public partial class ModelMenuManage : System.Web.UI.Page
    {
        public int modelid = 0;//模板id
        public int id = 0;
        protected string headPortraitImgSrc = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            modelid = Request["modelid"].ConvertTo<int>(0);
            id = Request["id"].ConvertTo<int>(0);

            BindHeadPortrait();
            ShowImgBind(id);
        }

        private void BindHeadPortrait()
        {
            headPortrait.uploadFileInfo.ObjType = (int)FileObjType.Photo;
            headPortrait.displayImgId = "headPortraitImg";

        }
        private void ShowImgBind(int id)
        {


            //根据产品id得到 产品信息
            B2bModelData modeldate = new B2bModelData();
            var proo = modeldate.GetModelMenuById(id);

            if (proo != null)
            {
                var identityFileUpload = new FileUploadData().GetFileById(proo.Imgurl.ToString().ConvertTo<int>(0));
                if (identityFileUpload != null)
                {
                    headPortraitImgSrc = identityFileUpload.Relativepath;
                }
            }
        }
    }
}