using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.WebApp.V
{
    public partial class LoginF : System.Web.UI.Page
    {
        public string RequestUrl = "";//访问网址通过访问网址判断商家，如果访问网址为空则跳转到登陆页
        public int comid = 0;//公司id

        public string comeurl = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            RequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();
            //根据域名读取商户ID,如果没有绑定域名直接跳转后台
            B2b_company_info companyinfo = B2bCompanyData.GetComId(RequestUrl);
            if (companyinfo != null)
            {
                comid = companyinfo.Com_id;
            }
            string url = HttpContext.Current.Request.Url.Query;
            if (url != "")
            {
                comeurl = Request["come"].ToString();
            }
        }
    }
}