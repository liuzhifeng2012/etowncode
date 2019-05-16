using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Framework;
using ETS.JsonFactory;
using ETS2.Member.Service.MemberService.Model;


namespace ETS2.WebApp.JsonFactory
{
    /// <summary>
    /// PromotionHandler 的摘要说明
    /// </summary>
    public class PromotionHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string oper = context.Request["oper"].ConvertTo<string>("");

            if (oper != "")
            {
                if (oper == "getActById")
                {
                    int actid = context.Request["actid"].ConvertTo<int>(0);
                    var data = PromotionJsonDate.GetActById(actid);

                    context.Response.Write(data);
                }
                if (oper == "WhetherEditById")
                {
                    int actid = context.Request["actid"].ConvertTo<int>(0);
                    int operuserid = context.Request["operuserid"].ConvertTo<int>(0);

                    var data = PromotionJsonDate.WhetherEditByIdJson(actid,operuserid);

                    context.Response.Write(data);
                }
                if (oper == "editActinfo")
                {
                    var title = context.Request["title"].ConvertTo<string>("");
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var Acttype = context.Request["Acttype"].ConvertTo<int>(0);
                    var Money = context.Request["Money"].ConvertTo<int>(0);
                    var Discount = context.Request["Discount"].ConvertTo<int>(0);
                    var CashFull = context.Request["CashFull"].ConvertTo<int>(0);
                    var Cashback = context.Request["Cashback"].ConvertTo<int>(0);
                    var UseOnce = context.Request["UseOnce"].ConvertTo<bool>();
                    var RepeatIssue = context.Request["RepeatIssue"].ConvertTo<int>(1);
                    var Actstar = context.Request["Actstar"].ConvertTo<DateTime>();
                    var Actend = context.Request["Actend"].ConvertTo<DateTime>();
                    var FaceObjects = context.Request["FaceObjects"].ConvertTo<int>(1);
                    var ReturnAct = context.Request["ReturnAct"].ConvertTo<int>(0);
                    var Id = context.Request["actid"].ConvertTo<int>(0);
                    var Runstate = context.Request["runstate"].ConvertTo<bool>();
                    var Atitle = context.Request["atitle"].ConvertTo<string>("");
                    var Remark = context.Request["remark"].ConvertTo<string>("");
                    var Useremark = context.Request["useremark"].ConvertTo<string>("");
                    var Usetitle = context.Request["usetitle"].ConvertTo<string>("");
                    var UseChannel = context.Request["UseChannel"].ConvertTo<string>("0");

                    int createuserid = context.Request["createuserid"].ConvertTo<int>(0);


                    Member_Activity manageuser = new Member_Activity()
                    {
                        Id = Id,
                        Com_id = comid,
                        Title = title,
                        Acttype = Acttype,
                        Money = Money,
                        Discount = Discount,
                        CashFull = CashFull,
                        Cashback = Cashback,
                        UseOnce = UseOnce,
                        RepeatIssue = RepeatIssue,
                        Actstar = Actstar,
                        Actend = Actend,
                        FaceObjects = FaceObjects,
                        ReturnAct = ReturnAct,
                        Runstate = Runstate,
                        Atitle = Atitle,
                        Remark = Remark,
                        Useremark = Useremark,
                        Usetitle = Usetitle,
                        Usechannel = UseChannel,
                        CreateUserId = createuserid,
                        CreateTime = DateTime.Now

                    };
                    var data = PromotionJsonDate.EditActInfo(manageuser);
                    context.Response.Write(data);

                }

                if (oper == "pagelist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);

                    int userid=context.Request["userid"].ConvertTo<int>(0);
                    string state = context.Request["state"].ConvertTo<string>("0,1");
                    string data = PromotionJsonDate.ActPageList(comid, pageindex, pagesize,userid,state);

                    context.Response.Write(data);

                }

                if (oper == "ERNIEgetActById")
                {
                    int actid = context.Request["actid"].ConvertTo<int>(0);
                    var data = PromotionJsonDate.ERNIEGetActById(actid);

                    context.Response.Write(data);
                }

                if (oper == "ERNIEeditActinfo")
                {
                    var title = context.Request["title"].ConvertTo<string>("");
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var ERNIE_type = context.Request["ERNIE_type"].ConvertTo<int>(1);

                    var ERNIE_RateNum = context.Request["ERNIE_RateNum"].ConvertTo<int>(10000);
                    var ERNIE_Limit = context.Request["ERNIE_Limit"].ConvertTo<int>(0);
                    var Limit_Num = context.Request["Limit_Num"].ConvertTo<int>(0);
                    var runstate = context.Request["Runstate"].ConvertTo<int>(0);
                    var ERNIE_star = context.Request["ERNIE_star"].ConvertTo<DateTime>();
                    var ERNIE_end = context.Request["ERNIE_end"].ConvertTo<DateTime>();
                    var Id = context.Request["actid"].ConvertTo<int>(0);
                    var Remark = context.Request["Remark"].ConvertTo<string>("");

                    var Award_title = context.Request["Award_title"].ConvertTo<string>("");
                    var Award_num = context.Request["Award_num"].ConvertTo<int>(0);
                    var Award_type = context.Request["Award_type"].ConvertTo<int>(0);
                    var Award_Get_Num = context.Request["Award_Get_Num"].ConvertTo<int>(0);

                    List<Member_ERNIE_Award> Awardlist = new List<Member_ERNIE_Award>();
                    for (int i = 0; i < 6; i++)
                    {
                        Awardlist.Add(new Member_ERNIE_Award()
                        {
                            Award_title = context.Request["Award_title" + i].ConvertTo<string>(""),
                            Award_num = context.Request["Award_num" + i].ConvertTo<int>(1),
                            Award_type = context.Request["Award_type" + i].ConvertTo<int>(0),
                            Award_Get_Num = context.Request["Award_Get_Num" + i].ConvertTo<int>(0),

                        });
                    }


                    Member_ERNIE manageuser = new Member_ERNIE()
                    {
                        Id = Id,
                        Com_id = comid,
                        Title = title,
                        ERNIE_type = ERNIE_type,
                        ERNIE_RateNum = ERNIE_RateNum,
                        ERNIE_Limit = ERNIE_Limit,
                        Limit_Num = Limit_Num,
                        Runstate = runstate,
                        ERNIE_star = ERNIE_star,
                        ERNIE_end = ERNIE_end,
                        Remark = Remark

                    };
                    var data = PromotionJsonDate.ERNIEEditActInfo(manageuser, Awardlist);
                    context.Response.Write(data);
                }
                if (oper == "ERNIEActpagelist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var runstate = context.Request["runstate"].ConvertTo<string>("0,1");
                    string data = PromotionJsonDate.ERNIEActPageList(comid, pageindex, pagesize,runstate);
                    context.Response.Write(data);
                }



                if (oper == "ERNIEgetAwardById")
                {
                    int actid = context.Request["actid"].ConvertTo<int>(0);
                    var data = PromotionJsonDate.ERNIEGetAwardById(actid);

                    context.Response.Write(data);
                }

                if (oper == "ERNIERecordpagelist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var actid = context.Request["actid"].ConvertTo<int>(0);
                    var ERNIE_type = context.Request["ERNIE_type"].ConvertTo<int>(9);
                    var key = context.Request["key"].ConvertTo<string>("");
                    string data = PromotionJsonDate.ERNIERecordpagelist(comid, pageindex, pagesize, actid, ERNIE_type, key);
                    context.Response.Write(data);
                }



                if (oper == "ERNIERecordedit")
                {
                    var actid = context.Request["actid"].ConvertTo<int>(0);
                    var data = PromotionJsonDate.ERNIERecordedit(actid);
                    context.Response.Write(data);
                }


                if (oper == "ERNIEAwardget")
                {
                    int actid = context.Request["actid"].ConvertTo<int>(0);
                    int topclass = context.Request["topclass"].ConvertTo<int>(0);
                    var data = PromotionJsonDate.ERNIEAwardget(actid, topclass);

                    context.Response.Write(data);
                }



                if (oper == "ERNIEeditAwardinfo")
                {
                    var Id = context.Request["Id"].ConvertTo<int>(0);
                    var ERNIE_id = context.Request["ERNIE_id"].ConvertTo<int>(0);
                    var Award_class = context.Request["Award_class"].ConvertTo<int>(1);
                    var Award_num = context.Request["Award_num"].ConvertTo<int>(10000);
                    var Award_type = context.Request["Award_type"].ConvertTo<int>(0);
                    var Award_Get_Num = context.Request["Award_Get_Num"].ConvertTo<int>(0);
                    var Award_title = context.Request["Award_title"].ConvertTo<string>("");


                    Member_ERNIE_Award manageuser = new Member_ERNIE_Award()
                    {
                        Id = Id,
                        ERNIE_id = ERNIE_id,
                        Award_class = Award_class,
                        Award_num = Award_num,
                        Award_type = Award_type,
                        Award_Get_Num = Award_Get_Num,
                        Award_title = Award_title
                    };
                    var data = PromotionJsonDate.ERNIEEditAwardInfo(manageuser);
                    context.Response.Write(data);
                }
                if (oper == "ERNIEAwardpagelist")
                {
                    var actid = context.Request["actid"].ConvertTo<int>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    string data = PromotionJsonDate.ERNIEAwardPageList(actid, pageindex, pagesize);
                    context.Response.Write(data);
                }


                if (oper == "ERNIEeditActOnline")
                {
                    var actid = context.Request["actid"].ConvertTo<int>(0);
                    var data = PromotionJsonDate.ERNIEeditActOnline(actid);
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