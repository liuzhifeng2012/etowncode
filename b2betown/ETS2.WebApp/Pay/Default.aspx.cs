using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.WebApp.Pay
{
    public partial class Default : System.Web.UI.Page
    {

        #region 辨别商家参数
        public string RequestUrl = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Trim().ToLower();
        public int comid = 0;
        #endregion

        public string company = "";
        public string tel = "";
        public string comlogo = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comid = Request["comid"].ConvertTo<int>(0);


            if (comid == 0)//shop101标准格式获取comid
            {
                if (Domain_def.Domain_yanzheng(RequestUrl))//如果符合shop101.etown.cn的格式，则从多微信商户基本信息表中获取comid
                {
                    //先通过正则表达式获取COMid
                    comid = Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl).ToString());
                }
            }

            if (comid == 0)//如果非标准格式，查询 是否有绑定的域名
            {
                var domaincomid = B2bCompanyData.GetComId(RequestUrl);
                if (domaincomid != null)
                {
                    comid = domaincomid.Com_id;
                }
            }


            if (comid != 0)
            {
                var commodel = B2bCompanyData.GetCompany(comid);

                if (commodel != null)
                {

                    company = commodel.Com_name;
                    if (commodel.B2bcompanyinfo != null)
                    {
                        tel = commodel.B2bcompanyinfo.Tel;
                    }

                }


                var comtDirect = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());
                if (comtDirect != null)
                {
                    if (comtDirect.Smalllogo != null && comtDirect.Smalllogo != "")
                    {
                        comlogo = FileSerivce.GetImgUrl(comtDirect.Smalllogo.ConvertTo<int>(0));
                    }
                }
            
            
            
            
            }



        }
    }
}