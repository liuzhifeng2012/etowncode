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
using ETS2.PM.Service.PMService.Data;


namespace ETS2.WebApp.UI.ShangJiaUI
{
    public partial class H5Setting : System.Web.UI.Page
    {
        protected string headPortraitImgSrc = "/images/defaultThumb.png";
        protected string smallheadPortraitImgSrc = "/images/defaultThumb.png";
        public int typeid = 0;
        public int id = 0;
        public string Title = "";
        public string linkurl = "#";
        public int Modelid = 0;
        public int bjimage_w=0;
        public int bjimage_h = 0;
        public string bgtishi = "请对图片进行处理,上传过大图片会影响浏览";


        protected void Page_Load(object sender, EventArgs e)
        {
            var comid = UserHelper.CurrentCompany.ID;
            typeid = Request["typeid"].ConvertTo<int>(0);
            id = Request["id"].ConvertTo<int>(0);

            BindHeadPortrait();
            ShowImgBind();
            B2bModelData mdate = new B2bModelData();
            //判断模板，进行跳转
            var h5model = mdate.SelectModelSearchComid(comid);
            if (h5model != null)
            {
                Modelid = h5model.Modelid;
            }

            if (Modelid != 0) {
                var modeldata =new B2bModelData();
                var modelinfo = modeldata.GetModelById(Modelid);
                if (modelinfo != null) { 
                    bjimage_w= modelinfo.Bgimage_w;
                    bjimage_h = modelinfo.Bgimage_h;
                }
            }

            if(bjimage_h != 0 ){
                bgtishi = "图片大小 高:" + bjimage_h + "px 宽:" + bjimage_w + "px .请对图片进行处理,上传过大图片会影响浏览！";
            
            }


        }
        private void BindHeadPortrait()
        {
            headPortrait.uploadFileInfo.ObjType = (int)FileObjType.Photo;
            headPortrait.displayImgId = "headPortraitImg";
        }
        private void ShowImgBind()
        {
            //var comid = Context.Request["comid"].ConvertTo<int>(0);
            var comid = UserHelper.CurrentCompany.ID;
            if (id != 0)
            {
                B2b_company_image imagemodel = B2bCompanyImageData.GetimageByComid(comid, id);
                if (imagemodel != null)
                {
                    Title = imagemodel.Title;
                    linkurl=imagemodel.Linkurl;



                    FileUploadModel identityFileUpload = new FileUploadData().GetFileById(imagemodel.Imgurl);
                    if (identityFileUpload != null)
                    {
                        headPortraitImgSrc = identityFileUpload.Relativepath;
                    }

                }
            }
        }
    }
}