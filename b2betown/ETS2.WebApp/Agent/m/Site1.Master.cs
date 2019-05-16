using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.WebApp.Agent.m
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        public int Agentid = 0; 
        public string account = "";
        public int Agentsort = 1;//0为票务分销，1为微站渠道,2为项目商户
        public int AccountLevel = 1;

        public int comid_temp = 0;
        public string company = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comid_temp = Request["comid"].ConvertTo<int>(0);
            if (Session["Agentid"] != null)
            {
                //账户信息
                Agentid = (Session["Agentid"].ToString()).ConvertTo<int>(0);

              
                B2b_company companyinfo = B2bCompanyData.GetAllComMsg(comid_temp);
                if (companyinfo != null)
                {
                    company = companyinfo.Com_name;

                }


                if (Session["Account"] == null)
                {
                    Response.Redirect("login.aspx");
                }


                account = Session["Account"].ToString();

                if (Agentid != 0)
                {
                    var agentdata = AgentCompanyData.GetAgentByid(Agentid);
                    if (agentdata != null)
                    {
                        Agentsort = agentdata.Agentsort;
                        if (Agentsort == 2)
                        {
                            Response.Redirect("Project.aspx");
                        }
                    }
                }

                var agentdate = AgentCompanyData.GetAgentAccountByUid(account, Agentid);
                if (agentdate == null)//没有分销账户直接退出
                {
                    Response.Redirect("login.aspx");
                }

                AccountLevel = agentdate.AccountLevel;

            }
            else
            {

                if (Request.Cookies["Agentid"] != null)
                {
                    string accountmd5 = "";
                    string Account = "";
                    int Agentid_temp =  (Request.Cookies["Agentid"].Value).ConvertTo<int>(0);
                    if (Request.Cookies["AgentKey"] != null)
                    {
                        accountmd5 = Request.Cookies["AgentKey"].Value;
                    }
                    if (Request.Cookies["Account"] != null)
                    {
                        Account = Request.Cookies["Account"].Value;
                    }

                    var agentdata = AgentCompanyData.GetAgentByid(Agentid_temp);
                    if (agentdata != null)
                    {
                        var returnmd5 = EncryptionHelper.ToMD5(Account + "lixh1210" + Agentid_temp, "UTF-8");
                        if (returnmd5 == accountmd5)
                        {
                            Session["Agentid"] = Agentid_temp;
                            Session["Account"] = Account;
                            Response.Redirect(Request.Url.ToString());
                        }
                        else
                        {
                            Response.Redirect("login.aspx");
                        }

                    }
                    else
                    {
                        Response.Redirect("login.aspx");
                    }


                }
                else
                {

                    Response.Redirect("login.aspx");
                }

            }
        }
    }
}