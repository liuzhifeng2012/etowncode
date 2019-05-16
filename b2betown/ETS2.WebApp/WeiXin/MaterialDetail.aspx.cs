using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FileUpload.FileUpload.Entities.Enum;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using FileUpload.FileUpload.Data;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS.JsonFactory;
using ETS2.Common.Business;

namespace ETS2.WebApp.WeiXin
{
    public partial class MaterialDetail : System.Web.UI.Page
    {
        public string nowdate = "";//现在日期
        public string monthdate = "";//现在日期加一个月

        protected int materialid = 0;
        protected string headPortraitImgSrc = "";

        protected int promotetypeid = 0;//促销类型

        //public int comid=0;
        //public int percal=0;
        //public int peryear=0;
        //public int percalid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //comid = UserHelper.CurrentCompany.ID ;//固定comid

            nowdate = DateTime.Now.ToString("yyyy-MM-dd");
            monthdate = DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd");

            materialid = Request["materialid"].ConvertTo<int>(0);
            promotetypeid = Request["promotetypeid"].ConvertTo<int>(0);

            //if (materialid != 0)
            //{
            //    WxMaterial wxmaterial = new WxMaterialData().GetWxMaterial(materialid);
            //    if (wxmaterial != null)
            //    {
            //        percalid = wxmaterial.Periodicalid;
            //    }
            //}

            //if (promotetypeid != 0)
            //{
            //    periodical period = new WxMaterialData().selectWxsaletype(promotetypeid, comid);
            //    if (period != null)
            //    {
            //        percal = period.Percal;
            //        peryear = period.Peryear;
            //        percalid = period.Id;
            //    }
            //}

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

            //根据素材id得到素材信息
            WxMaterial wxmaterial = new WxMaterialData().GetWxMaterial(materialid);
            if (wxmaterial != null)
            {
                var identityFileUpload = new FileUploadData().GetFileById(wxmaterial.Imgpath.ToString().ConvertTo<int>(0));
                if (identityFileUpload != null)
                {
                    headPortraitImgSrc = identityFileUpload.Relativepath;
                }

            }
        }
    }
}