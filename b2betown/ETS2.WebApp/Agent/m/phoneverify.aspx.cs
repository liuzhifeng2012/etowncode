using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS.JsonFactory;

namespace ETS2.WebApp.Agent.m
{
    public partial class phoneverify : System.Web.UI.Page
    {
        public int comid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
           string  RequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();

            //根据域名读取商户ID,如果没有绑定域名直接跳转后台
            comid = GeneralFunc.GetComid(RequestUrl);
        }
    }
}