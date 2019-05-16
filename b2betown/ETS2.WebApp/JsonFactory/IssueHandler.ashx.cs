using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Framework;
using ETS.JsonFactory;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Member.Service.MemberService.Data;

namespace ETS2.WebApp.JsonFactory
{
    /// <summary>
    /// IssueHandler 的摘要说明
    /// </summary>
    public class IssueHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string oper = context.Request["oper"].ConvertTo<string>("");
            if (oper != "")
            {

                if (oper == "editissue")
                {
                    int issueid = context.Request["issueid"].ConvertTo<int>(0);
                    string pubtitle = context.Request["pubtitle"];
                    int channelid = context.Request["channelid"].ConvertTo<int>(0);
                    int cardid = context.Request["cardid"].ConvertTo<int>(0);
                    int num = context.Request["num"].ConvertTo<int>(0);
                    string actchecked = context.Request["actchecked"];
                    var isopen1 = context.Request["isopen"];
                    bool isopen = context.Request["isopen"].ConvertTo<string>("true") == "true" ? true : false;
                    int openaddress = context.Request["openaddress"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    Member_Issue issuemodel = new Member_Issue()
                    {
                        Id = issueid,
                        Chid = channelid,
                        Crid = cardid,
                        Num = num,
                        Com_id = comid,
                        Openyes = isopen,
                        Openaddress = openaddress,
                        Title = pubtitle
                    };

                    string data = IssueJsonDate.EditIssue(issuemodel, actchecked);
                    context.Response.Write(data);
                }
                if (oper == "GetIssueDetailById")
                {
                    int issueid = context.Request["issueid"].ConvertTo<int>(0);
                    string data = IssueJsonDate.GetIssueDetailById(issueid);
                    context.Response.Write(data);
                }
                if (oper == "GetIssueDetail2")
                {
                    int issueid = context.Request["issueid"].ConvertTo<int>(0);
                    string data = IssueJsonDate.GetIssueDetail2(issueid);
                    context.Response.Write(data);
                }


                if (oper == "GetIssuePromot")
                {
                    int issueid = context.Request["issueid"].ConvertTo<int>(0);
                    string data = IssueJsonDate.GetIssuePromot(issueid);
                    context.Response.Write(data);
                }
                if (oper == "pagelist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    string data = IssueJsonDate.PageList(comid, pageindex, pagesize);


                    context.Response.Write(data);

                }
                if (oper == "entercardnumber")
                {
                    int issueid = context.Request["issueid"].ConvertTo<int>(0);
                    //var carddd = context.Request["cardnumber"];
                    decimal cardnumber = context.Request["cardnumber"].ConvertTo<decimal>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = IssueJsonDate.EnterCardNumber(issueid, cardnumber, comid);
                    context.Response.Write(data);
                }
                if (oper == "batchentercardnumber")
                {
                    int issueid = context.Request["issueid"].ConvertTo<int>(0);
                    decimal numberbegin=context.Request["numberbegin"].ConvertTo<decimal>(0);
                    decimal numberend = context.Request["numberend"].ConvertTo<decimal>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    bool ignoreentered=context.Request["ignoreentered"].ConvertTo<bool>(false);

                    string data = IssueJsonDate.BatchEnterCardNumber(issueid, numberbegin,numberend, comid,ignoreentered);
                    //string data = IssueJsonDate.EnterCardNumber(issueid, numberbegin, comid);
                    context.Response.Write(data);
                }
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