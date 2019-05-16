using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Framework;
using ETS.JsonFactory;

namespace ETS2.WebApp.JsonFactory
{
    /// <summary>
    /// UserHandle 的摘要说明
    /// </summary>
    public class UserHandle : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string oper = context.Request["oper"].ConvertTo<string>("");
            if (oper != "")
            {
                if (oper == "changepwd")
                {
                    int userid = context.Request["userid"].ConvertTo<int>(0);
                    string oldpwd = context.Request["oldpwd"];
                    string pwd1 = context.Request["pwd1"];


                    string changepwddata = UserHandleJsonData.ChangePwd(userid, oldpwd, pwd1);

                    context.Response.Write(changepwddata);
                }
            }
            if (oper == "GetManageUserInfo")
            {
                int userid = context.Request["id"].ConvertTo<int>(0);
                string data = UserHandleJsonData.GetManageUserInfo(userid);
                context.Response.Write(data);
            }
            if (oper == "GetManageUserByAccount")
            {
                string account = context.Request["account"].ConvertTo<string>("");
                string data = UserHandleJsonData.GetManageUserByAccount(account);
                context.Response.Write(data);
            }
            if (oper == "GetAccountInfo")
            {
                int userid = context.Request["id"].ConvertTo<int>(0);
                string data = UserHandleJsonData.GetAccountInfo(userid);
                context.Response.Write(data);
            }
            if (oper == "GetB2bCompanyByCompanyName")
            {
                string companyname = context.Request["companyname"].ConvertTo<string>("");
                string data = UserHandleJsonData.GetB2bCompanyByCompanyName(companyname);
                context.Response.Write(data);
            }
            if (oper == "GetB2bCompanyInfoByDomainname")
            {
                string Domainname = context.Request["Domainname"].ConvertTo<string>("");
                string data = UserHandleJsonData.GetB2bCompanyInfoByDomainname(Domainname);
                context.Response.Write(data);
            }

            if (oper == "Getjiaolianworktime")
            {
                int userid = context.Request["id"].ConvertTo<int>(0);
                var date = context.Request["date"].ConvertTo<DateTime>(DateTime.Now);





                string data = UserHandleJsonData.GetWorktimeInfo(userid,date);
                context.Response.Write(data);
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}