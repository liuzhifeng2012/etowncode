using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Common.Business;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.CRM.Service.CRMService.Data;
using ETS.JsonFactory;
using ETS2.WeiXin.Service.WeiXinService.Model;
using System.Xml;
using Newtonsoft.Json;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Member.Service.MemberService.Data;
namespace ETS2.WebApp.H5
{
    public partial class evaluate : System.Web.UI.Page
    {
        public string openid = "";//微信号
        public int AccountId = 0;//会员id
        public B2b_crm userinfo = new B2b_crm();
        public int comid = 0;
        public string title = "";
        public string RequestUrl = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Trim().ToLower();
        public string userid = "0";//用户临时 Uid 或 实际Uid 
        public int evatype = 0;
        public int channelid = 0;
        public string logoimg = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            evatype = Request["evatype"].ConvertTo<int>(0);
            channelid = Request["channelid"].ConvertTo<int>(0);

            RequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();
            //根据域名读取商户ID,如果没有绑定域名直接跳转后台
            if (Domain_def.Domain_yanzheng(RequestUrl))//如果符合shop101.etown.cn的格式，则从多微信商户基本信息表中获取comid
            {
                //先通过正则表达式获取COMid
                comid = Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl).ToString());
                if (comid == 0)
                {
                    comid = new WeiXinBasicData().GetWeiXinBasicByDomain(RequestUrl).Comid;
                }
            }
            else
            {
                B2b_company_info companyinfo = B2bCompanyData.GetComId(RequestUrl);
                if (companyinfo != null)
                {
                    comid = companyinfo.Com_id;
                }

            }

            if (comid != 0)
            {
                title = B2bCompanyData.GetCompany(comid).Com_name;
                B2b_company_saleset pro = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());

                if (pro != null)
                {
                    logoimg = FileSerivce.GetImgUrl(pro.Logo.ConvertTo<int>(0));
                }
            }
        }
    }
}