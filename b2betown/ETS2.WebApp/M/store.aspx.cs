using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using FileUpload.FileUpload.Data;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.Common.Business;
using ETS.JsonFactory;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Member.Service.MemberService.Data;

namespace ETS2.WebApp.M
{
    public partial class store : System.Web.UI.Page
    {
        protected int materialid = 0;
        public string phone = "";
        public string address = "";
        public string name = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            bool bo = detectmobilebrowser.HttpUserAgent(u);

            materialid = Request["materialid"].ConvertTo<int>(0);

            if (materialid != 0)
            {
                //if (bo == false)
                //{
                //    if (materialid == 101)
                //    {
                //        Response.Redirect("http://vctrip.etown.cn/");
                //    }
                //    Response.Redirect("http://shop" + materialid + ".etown.cn");
                //}
                B2b_company_info info = new B2bCompanyInfoData().GetCompanyInfo(materialid);
                if (info != null)
                {
                    phone = info.Tel;
                    address = info.Scenic_address;

                }
                B2b_company com = B2bCompanyData.GetCompany(materialid);
                if (com != null)
                {
                    name = com.Com_name;
                }

               
                if (Request.Cookies["AccountId"] != null)//判断是否有登陆COOKI
                    {
                        string accountmd5 = "";
                        int AccountId = int.Parse(Request.Cookies["AccountId"].Value);
                        if (Request.Cookies["AccountKey"] != null)
                        {
                            accountmd5 = Request.Cookies["AccountKey"].Value;
                        }
                        B2b_crm userinfo;
                        var data = CrmMemberJsonData.WeixinCookieLogin(AccountId.ToString(), accountmd5, materialid, out userinfo);
                        if (data == "OK")
                        {
                            if (userinfo != null)
                            {
                                string openid_temp = userinfo.Weixin;
                                GetMenshiDetail(openid_temp, materialid);
                            }
                        }
                 }


            }
        }


        //判断是否跳转
        private void GetMenshiDetail(string openid, int comid)
        {
            Member_Channel channel = new MemberChannelData().GetChannelByOpenId(openid);
            if (channel == null)
            {
                //errlog = "获得渠道为空";
            }
            else
            {
                //渠道单位类型:0:内部渠道；1：外部渠道;3:网站注册;4:微信注册
                if (channel.Issuetype == 0 || channel.Issuetype == 1)//会员属于门店
                {

                    //根据微信号得到会员所在的门市信息
                    Member_Channel_company menshi = new MemberChannelcompanyData().GetMenShiByJumpId(openid, comid);
                    if (menshi != null)
                    {
                        if (menshi.Whetherdepartment == 0)
                        {
                            //如果不是内部部门，显示门店列表
                             Response.Redirect("/H5/storedetail.aspx?menshiid=" + menshi.Id);
                        }
                    }
                }
                else//会员属于公司 
                {
                   // channelcompanyid = 0;
                }
            }
        }
    }
}