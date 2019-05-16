using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.Common.Business;
using FileUpload.FileUpload.Data;
using ETS.JsonFactory;
using ETS2.Member.Service.MemberService.Data;
using ETS2.Member.Service.MemberService.Model;
using System.Xml;
using Newtonsoft.Json;
using FileUpload.FileUpload.Entities;

namespace ETS2.WebApp.H5.manage
{
    public partial class Default : System.Web.UI.Page
    {
        #region 辨别商家参数
        public string RequestUrl = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Trim().ToLower();
        public int comid = 0;
        #endregion

        //获得公司logo地址和公司名称
        public string comname = "";//公司名称
        public string comlogo = "";//公司logo地址

        public B2b_crm userinfo = new B2b_crm();
        protected void Page_Load(object sender, EventArgs e)
        {

            comid = Request["comid"].ConvertTo<int>(0);

            if (comid != 0)
            {
                B2bModelData mdate = new B2bModelData();
                //判断模板，进行跳转
                var h5model = mdate.SelectModelSearchComid(comid);
                if (h5model != null)
                {
                    if (h5model.Modelid != 1)
                    {
                        Response.Redirect("/h5/manage/Default" + h5model.Modelid + ".aspx?comid="+comid);
                    }
                }

            }

        }

    }
}