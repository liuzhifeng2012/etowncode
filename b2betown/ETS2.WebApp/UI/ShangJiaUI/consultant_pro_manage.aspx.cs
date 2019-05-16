﻿using System;
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
    public partial class consultant_pro_manage : System.Web.UI.Page
    {
        protected string headPortraitImgSrc = "/images/defaultThumb.png";
        protected string smallheadPortraitImgSrc = "";
        public int typeid = 0;
        public int id = 0;
        public string Name = "";
        public string linkurl = "";
        public int Modelid = 0;
        public int Daohangimg = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            var comid = UserHelper.CurrentCompany.ID;
            id = Request["id"].ConvertTo<int>(0);

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
            //var comid = Context.Request["comid"].ConvertTo<int>(0);
            var comid = UserHelper.CurrentCompany.ID;
            if (id != 0)
            {
                B2b_company_menu menumodel = B2bCompanyMenuData.GetConsultantByComid(comid, id);
                if (menumodel != null)
                {
                    Name = menumodel.Name;
                    linkurl = menumodel.Linkurl;
                    typeid = menumodel.Linktype;



                    FileUploadModel identityFileUpload = new FileUploadData().GetFileById(menumodel.Imgurl);
                    if (identityFileUpload != null)
                    {
                        headPortraitImgSrc = identityFileUpload.Relativepath;
                    }

                }
            }
        }


    }
}