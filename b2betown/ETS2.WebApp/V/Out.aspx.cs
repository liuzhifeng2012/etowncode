﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.WebApp.V
{
    public partial class Out : System.Web.UI.Page
    {
        public string RequestUrl = "";//访问网址通过访问网址判断商家，如果访问网址为空则跳转到登陆页
        public int comid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["AccountId"] != null)
            {
                RequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();
                B2b_company_info companyinfo = B2bCompanyData.GetComId(RequestUrl);
                if (companyinfo != null)
                {
                    comid = companyinfo.Com_id;
                }
            }

            if (comid == 101)
            {
                Session["AccountId"] = null;
                Session["AccountName"] = null;
                Session["AccountCard"] = null;
                Response.Redirect("http://vctrip.etown.cn");
            }
            else
            {
                Session["AccountId"] = null;
                Session["AccountName"] = null;
                Session["AccountCard"] = null;
                Response.Redirect("/ui/shangjiaui/ProductList.aspx");
            }

        }
    }
}