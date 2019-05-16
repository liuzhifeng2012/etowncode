using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Framework;
using ETS.JsonFactory;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Member.Service.MemberService.Data;
using ETS2.CRM.Service.CRMService.Data;
using Newtonsoft.Json;


namespace ETS2.WebApp.JsonFactory
{
    /// <summary>
    /// CardHandler 的摘要说明
    /// </summary>
    public class CardHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string oper = context.Request["oper"].ConvertTo<string>("");

            if (oper != "")
            {
                if (oper == "getCardById")
                {
                    int id = context.Request["cardid"].ConvertTo<int>(0);
                    var data = CardJsonDate.GetCardById(id);

                    context.Response.Write(data);
                }
                if (oper == "getCardByIdNew")//新添加了公司限制
                {
                    int id = context.Request["cardid"].ConvertTo<int>(0);
                    int comid=context.Request["comid"].ConvertTo<int>(0);
                    var data = CardJsonDate.GetCardById(comid,id);

                    context.Response.Write(data);
                }
                if (oper == "getCardByIssueId")
                {
                    int issueid = context.Request["issueid"].ConvertTo<int>(0);
                    var data = CardJsonDate.GetCardByIssueId(issueid);

                    context.Response.Write(data);
                }
                if (oper == "getCardFirst")
                {
                    int id = context.Request["comid"].ConvertTo<int>(0);
                    var data = CardJsonDate.GetCardFirst(id);

                    context.Response.Write(data);
                }


                if (oper == "editCardinfo")
                {

                    var cname = context.Request["cname"].ConvertTo<string>("");
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var ctype = context.Request["ctype"].ConvertTo<int>(0);
                    var printnum = context.Request["printnum"].ConvertTo<int>(0);
                    var zhuanzeng = context.Request["zhuanzeng"].ConvertTo<int>(0);
                    var qrcode = context.Request["qrcode"].ConvertTo<int>(0);
                    var exchange = context.Request["exchange"].ConvertTo<string>("");
                    var remark = context.Request["remark"].ConvertTo<string>("");
                    var cardRule = context.Request["cardRule"].ConvertTo<int>(1);
                    var cardRule_starnum = context.Request["cardRule_starnum"].ConvertTo<int>(0);
                    var cardRule_First = context.Request["cardRule_First"].ConvertTo<int>(0);
                    var CardRule_Second = context.Request["CardRule_Second"].ConvertTo<int>(0);
                    var Id = context.Request["cardid"].ConvertTo<int>(0);


                    Member_Card manageuser = new Member_Card()
                    {
                        Id = Id,
                        Com_id = comid,
                        Cname = cname,
                        Ctype = ctype,
                        Printnum = printnum,
                        Zhuanzeng = zhuanzeng,
                        Qrcode = qrcode,
                        Exchange = exchange,
                        Remark = remark,
                        CardRule = cardRule,
                        CardRule_starnum = cardRule_starnum,
                        CardRule_First = cardRule_First,
                        CardRule_Second = CardRule_Second
                    };
                    var data = CardJsonDate.EditCardInfo(manageuser);
                    context.Response.Write(data);

                }

                if (oper == "pagelist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    string data = CardJsonDate.CardPageList(comid, pageindex, pagesize);


                    context.Response.Write(data);

                }
                //查询卡号或手机是有账户
                if (oper == "SearchCard")
                {
                    string pno = context.Request["pno"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int userinfo = 0;
                    var data = CardJsonDate.SearchCard(pno, comid, out userinfo);
                    context.Response.Write(data);
                }

                //确认使用
                if (oper == "econfirmCard")
                {
                    int aid = context.Request["aid"].ConvertTo<int>(0);
                    int actid = context.Request["actid"].ConvertTo<int>(0);
                    int cardid = context.Request["cardid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    Member_Activity actinfo;
                    string phone = "";
                    string name = "";
                    decimal idcard = 0;
                    decimal agcardcode = 0;

                    var data = CardJsonDate.EconfirmCard(aid, actid, cardid, comid, out actinfo, out phone, out name, out idcard, out agcardcode);
                    context.Response.Write(data);
                }

                //根据使用日志查询记录
                if (oper == "getCardinfo")
                {
                    int actid = context.Request["actid"].ConvertTo<int>(0);
                    int pno = context.Request["pno"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    var data = CardJsonDate.GetCardInfo(actid, pno, comid);
                    context.Response.Write(data);
                }

                //得到不同渠道，活动，发行 下的卡号列表
                if (oper == "MemberCardPageList")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var cardcode = context.Request["cardcode"].ConvertTo<decimal>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(0);
                    var issueid = context.Request["issueid"].ConvertTo<int>(0);
                    var channelid = context.Request["channelid"].ConvertTo<int>(0);
                    var actid = context.Request["actid"].ConvertTo<int>(0);
                    var isopencard = context.Request["isopencard"].ConvertTo<int>(1);

                    var datastr = CardJsonDate.GetMemberCardList(comid, cardcode, pageindex, pagesize, issueid, channelid, actid, isopencard);
                    context.Response.Write(datastr);
                }
                //电脑验卡
                if (oper == "readuser")
                {
                    var MALogactid = context.Request["Actid"].ConvertTo<int>(0);
                    var MALogcardid = context.Request["Cardid"].ConvertTo<int>(0);
                    var MALogOrderId = context.Request["OrderId"].ConvertTo<int>(0);
                    var MALogServerName = context.Request["ServerName"].ConvertTo<string>("");
                    var MALogNum_people = context.Request["Num_people"].ConvertTo<int>(0);
                    var MALogPer_capita_money = context.Request["Per_capita_money"].ConvertTo<int>(0);
                    var MALogMember_return_money = context.Request["Member_return_money"].ConvertTo<int>(0);
                    var MALogsales_admin = context.Request["sales_admin"].ConvertTo<string>("");
                    var MALogcomid = context.Request["comid"].ConvertTo<int>(0);
                    var AccountName = context.Request["AccountName"].ConvertTo<string>("");

                    var data = CardJsonDate.GetCarValidate(MALogactid, MALogcardid, MALogOrderId, MALogServerName, MALogNum_people, MALogPer_capita_money, MALogMember_return_money, MALogsales_admin, MALogcomid, AccountName);
                    context.Response.Write(data);
                }
                //使用预付款、积分
                if (oper == "CashCoupon")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var KrderId = context.Request["KrderId"].ConvertTo<decimal>();
                    var KrderM = context.Request["KrderM"].ConvertTo<string>();
                    var Kales_admin = context.Request["Kales_admin"].ConvertTo<string>("");
                    var money = context.Request["money"].ConvertTo<int>();
                    var Imprest = context.Request["Imprest"].ConvertTo<int>();
                    var Integral = context.Request["Integral"].ConvertTo<int>();
                    var card = context.Request["card"].ConvertTo<decimal>();
                    var mid = new B2bCrmData().GetB2bCrmByCardcode(card).Id;
                    string acttype = "";
                    string data = "";
                    if (money == 0)
                    {
                        acttype = "reduce_imprest";
                        if (int.Parse(KrderM) > Imprest)
                        {
                            data = JsonConvert.SerializeObject(new { type = 1, msg = "预付款" });
                        }
                        else
                        {
                            data = BusinessCustomersJsonData.WriteMoney(mid, comid, acttype, KrderM, KrderId, Kales_admin);
                        }
                    }
                    if (money == 1)
                    {
                        acttype = "reduce_integral";
                        if (int.Parse(KrderM) > Integral)
                        {
                            data = JsonConvert.SerializeObject(new { type = 1, msg = "积分" });
                        }
                        else
                        {
                            data = BusinessCustomersJsonData.WriteMoney(mid, comid, acttype, KrderM, KrderId, Kales_admin);
                        }
                    }

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