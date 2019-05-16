using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS.Framework;

namespace ETS2.WebApp.UI.PMUI
{
    public partial class BindingAgentList : System.Web.UI.Page
    {
        public int comid = 0;//公司编号

        public int bindingagent = 0;//绑定的分销账户
        protected void Page_Load(object sender, EventArgs e)
        {


            if (UserHelper.ValidateLogin())
            {
                comid = UserHelper.CurrentCompany.ID;
            }

            B2b_company company = UserHelper.CurrentCompany;
            var comdata = B2bCompanyData.GetCompany(company.ID);
            if (comdata != null)
            {
                bindingagent = comdata.Bindingagent;

                //如果未绑定分销则 返回 绑定分销页面
                if (bindingagent == 0)
                {
                    Response.Redirect("BindingAgent.aspx");
                }

            }



        }
    }
}