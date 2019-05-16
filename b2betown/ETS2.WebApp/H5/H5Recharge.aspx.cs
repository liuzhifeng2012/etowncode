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
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.WebApp.H5
{
    public partial class H5Recharge : System.Web.UI.Page
    {
        public int comid = 0;
        public string openid = "";//微信号
        protected void Page_Load(object sender, EventArgs e)
        {
            //从cookie中得到微信号
            if (Request.Cookies["openid"] != null)
            {
                openid = Request.Cookies["openid"].Value;
            }

            B2bCrmData b2b_crm = new B2bCrmData();
            if (openid != "")
            {
                B2b_crm b2bmodle = b2b_crm.b2b_crmH5(openid, comid);
                if (b2bmodle != null)
                {
                    comid = b2bmodle.Com_id;
                }

            }
           
        }
    }
}