using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.Common.Business;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.WebApp.H5
{

    public partial class Order : System.Web.UI.MasterPage
    {
        

        #region 辨别商家参数
        public string RequestUrl = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Trim().ToLower();
        public int comid = 0;
        #endregion
        public string comName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Request.ValidateInput(); 

            if (Domain_def.Domain_yanzheng(RequestUrl))//如果符合shop101.etown.cn的格式，则从多微信商户基本信息表中获取comid
            {
                //先通过正则表达式获取COMid
                comid = Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl).ToString());
                if (comid == 0)
                {
                    comid = new WeiXinBasicData().GetWeiXinBasicByDomain(RequestUrl).Comid;
                }
                if (comid != 0)
                {
                    comName = B2bCompanyData.GetCompany(comid).Com_name ;

                    B2b_company_saleset pro = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());

                }

            }
            else
            {
                B2b_company_info companyinfo = B2bCompanyData.GetComId(RequestUrl);
                if (companyinfo != null)
                {
                    comid = companyinfo.Com_id;
                    if (comid != 0)
                    {
                        //comName = B2bCompanyData.GetCompany(comid).Com_name;
                    }
                }

            }

            
            

        }
    }
}