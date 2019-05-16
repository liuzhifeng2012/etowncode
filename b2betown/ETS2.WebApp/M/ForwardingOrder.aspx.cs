using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.Common.Business;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS.Framework;
using ETS2.Member.Service.MemberService.Data;
using ETS2.Member.Service.MemberService.Model;
using ETS2.WeiXin.Service.WeiXinService.Model;

namespace ETS2.WebApp.M
{
    public partial class ForwardingOrder : System.Web.UI.Page
    {
        public string RequestUrl = "";
        public string comname = "";
        public string comlogo = "";
        public int comid = 0;//公司id
        public int aid = 0;
        public string wxTitle = "";
        public string wxsummary = "";

        protected void Page_Load(object sender, EventArgs e)
        {

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


            WxMaterialData wxdate = new WxMaterialData();
            aid = wxdate.FrowardingSetList(comid);
            if (aid != 0)
            {

                WxMaterial wxinfo = wxdate.GetWxMaterial(aid);
                if (wxinfo != null) {
                     wxTitle = wxinfo.Title;
                     wxsummary = wxinfo.Summary;

                }

            }


           


        }
    }
}