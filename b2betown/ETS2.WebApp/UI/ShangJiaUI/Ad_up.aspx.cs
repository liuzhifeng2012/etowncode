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
using ETS2.WeiXin.Service.WeiXinService.Data;
namespace ETS2.WebApp.UI.ShangJiaUI
{
    public partial class Ad_up : System.Web.UI.Page
    {
        protected string headPortraitImgSrc = "/images/defaultThumb.png";
        protected string smallheadPortraitImgSrc = "";
        public int Adtype = 0;
        public int id = 0;
        public string Title = "";
        public string Link = "";
        public string Author = "";
        public string Keyword = "";

        public int Applystate = 0;
        public int Votecount = 0;
        public int Lookcount = 0;
        public int Musicid = 0;
        public string Musicscr = "";
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
                var actdata = new WxAdData();
                var pro = actdata.Getwxad(id, comid);

                if (pro != null)
                {
                    Adtype = pro.Adtype;
                    id = pro.Id;
                    Title = pro.Title;
                    Link = pro.Link;
                    Author = pro.Author;
                    Keyword = pro.Keyword;

                    Applystate = pro.Applystate;
                    Votecount = pro.Votecount;
                    Lookcount = pro.Lookcount;

                    Musicid = pro.Musicid;
                    if (Musicid != 0)
                    {
                        Musicscr = FileSerivce.GetImgUrl(Musicid);
                    }

                    FileUploadModel identityFileUpload = new FileUploadData().GetFileById(pro.Musicid);
                    if (identityFileUpload != null)
                    {
                        headPortraitImgSrc = identityFileUpload.Relativepath;
                    }

                }
            }
        }

    }
}