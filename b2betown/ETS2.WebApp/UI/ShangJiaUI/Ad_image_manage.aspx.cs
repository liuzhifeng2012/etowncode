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
    public partial class Ad_image_manage : System.Web.UI.Page
    {
        public int adid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            var comid = UserHelper.CurrentCompany.ID;
            adid = Request["adid"].ConvertTo<int>(0);
            ShowImgBind();

        }


        private void ShowImgBind()
        {
            //var comid = Context.Request["comid"].ConvertTo<int>(0);
            var comid = UserHelper.CurrentCompany.ID;
            if (adid != 0)
            {
                var actdata = new WxAdData();
                var pro = actdata.Getwxad(adid, comid);

                if (pro != null)
                {
                    Title = pro.Title;
                }
            }
        }
    }
}