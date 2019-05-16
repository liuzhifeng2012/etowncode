using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.Member.Service.MemberService.Model;
using ETS.Data.SqlHelper;
using ETS2.Member.Service.MemberService.Data;
using Newtonsoft.Json;
using System.Collections;
using ETS2.Member.Service.MemberService.Model.Enum;

namespace ETS.JsonFactory
{
    public class IssueJsonDate
    {
        public static string EditIssue(Member_Issue issuemodel, string actchecked)
        {

            try
            {
                var issueid = new MemberIssueData().InsertOrUpdate(issuemodel);

                var delissueact = new MemberIssueActivityData().DelIssueIdByIssueId(issueid);

                string[] actgroup = actchecked.Split(',');
                for (int i = 0; i < actgroup.Length; i++)
                {
                    Member_Issue_Activity act = new Member_Issue_Activity()
                    {
                        Id = 0,
                        ISid = issueid,
                        Acid = int.Parse(actgroup[i])
                    };
                    var issueactid = new MemberIssueActivityData().InsertOrUpdate(act);
                }


                return JsonConvert.SerializeObject(new { type = 100, msg = issueid });
            }
            catch
            {
                new SqlHelper().Dispose();
                throw;
            }

        }

        public static string GetIssueDetailById(int issueid)
        {
            try
            {

                var pro = new MemberIssueData().GetIssueDetailById(issueid);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string GetIssueDetail2(int issueid)
        {
            try
            {

                var pro = new MemberIssueData().GetIssueDetailById(issueid);
                List<Member_Issue> list = new List<Member_Issue>();
                list.Add(pro);
                IEnumerable result = "";
                if (list != null)

                    result = from prop in list
                             select new
                             {
                                 Id = prop.Id,
                                 Com_id = prop.Com_id,
                                 Chid = prop.Chid,
                                 Crid = prop.Crid,
                                 Title = prop.Title,
                                 Num = prop.Num,
                                 Openyes = prop.Openyes,
                                 Openaddress = prop.Openaddress,
                                 ActName = new MemberIssueActivityData().GetIssueActStr(prop.Id),
                                 IssueType = new MemberChannelData().GetChannelDetail(prop.Chid).Issuetype == 0 ? "内部渠道" : "外部渠道",
                                 ChannelUnit = new MemberChannelcompanyData().GetCompanyById(new MemberChannelData().GetChannelDetail(prop.Chid).Companyid).Companyname,
                                 Name = new MemberChannelData().GetChannelDetail(prop.Chid).Name,
                                 EnteredNumber = new MemberCardData().GetEnteredNumber(issueid),//得到此次发行已经录入的卡号数量
                                 CName = new MemberCardData().GetCardCreateByCrid(prop.Crid).Cname,
                                 OpenCardNum = new MemberCardData().GetOpenCardNum(issueid),//得到此次发行已经开卡的数量
                             };

                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string GetIssuePromot(int issueid)
        {
            try
            {

                var pro = new MemberIssueActivityData().GetIssuePromot(issueid);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        public static string PageList(string comid, int pageindex, int pagesize)
        {
            var totalcount = 0;
            try
            {

                var prodata = new MemberIssueData();
                var list = prodata.PageList(comid, pageindex, pagesize, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Com_id = pro.Com_id,
                                 Crid = pro.Crid,
                                 Chid = pro.Chid,
                                 Title = pro.Title,
                                 Num = pro.Num,
                                 Openyes = pro.Openyes,
                                 Openaddress = pro.Openaddress,
                                 IsSueType = new MemberChannelData().GetChannelDetail(pro.Chid).Issuetype == 0 ? "内部渠道" : "外部渠道",
                                 UnitName = new MemberChannelcompanyData().GetCompanyById(new MemberChannelData().GetChannelDetail(pro.Chid).Companyid).Companyname,
                                 ISueName = new MemberChannelData().GetChannelDetail(pro.Chid).Name,
                                 //根据issueid 得到录入量和开卡量；
                                 EnteredNum = new MemberCardData().GetEnteredNumber(pro.Id),
                                 OpenCardNum = new MemberCardData().GetOpenCardNum(pro.Id)

                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string EnterCardNumber(int issueid, decimal cardnumber, int comid)
        {
            string result = "";
            Member_Issue issue = new MemberIssueData().GetIssueDetailById(issueid);

            Member_Card cardinfo = new MemberCardData().GetCardByCardNumber(cardnumber);
            if (cardinfo == null)
            {

                result = "卡号不存在";
                return JsonConvert.SerializeObject(new { type = 1, msg = result });
            }
            else
            {
                //判断卡号是否已经录入
                bool hasentered = new MemberCardData().IsHasEntered(cardnumber);
                if (hasentered)
                {
                    result = "卡号已经录入";
                    return JsonConvert.SerializeObject(new { type = 1, msg = result });
                }
            }
            bool isenter = CommonEnterCardNumber(issueid, cardnumber, comid, out   result, issue);
            if (isenter)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = result });
            }
        }

        public static string BatchEnterCardNumber(int issueid, decimal numberbegin, decimal numberend, int comid, bool ignoreentered)
        {

            //判断卡号区间内的卡号数量 是否大于此次剩余的发行数量；大于返回错误
            Member_Issue issue = new MemberIssueData().GetIssueDetailById(issueid);
            decimal enterrcount = numberend - numberbegin;
            decimal enterrnum = decimal.Parse(issue.Num.ToString());
            decimal enterednum = decimal.Parse(new MemberCardData().GetEnteredNumber(issueid).ToString());//这次发行已经录入的数目
            if (enterrcount > enterrnum - enterednum)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "区间范围超过了剩余的发行数量" + (enterrnum - enterednum) });
            }


            string result1 = "";  //判断区间内的卡号是否全部符合录入要求；符合，录入；不符合弹出确认框
            string result2 = "";//判断卡片区间内的卡号是否都存在于 卡号生成表(Member_Card)
            for (var i = numberbegin; i <= numberend; i++)
            {
                Member_Card cardinfo = new MemberCardData().GetCardByCardNumber(i);

                if (cardinfo == null)
                {

                    result2 += i + ",";
                }
                else
                {
                    if (cardinfo.IssueCard > 0)
                    {
                        result1 += i + ",";
                    }
                }
            }
            if (result1.Trim() != "")
            {
                if (!ignoreentered)//不忽视已经录入的
                {
                    return JsonConvert.SerializeObject(new { type = 10, msg = result1.Substring(0, result1.Length - 1) + "已经录入到数据库" });
                }

            }
            if (result2.Trim() != "")
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = result2.Substring(0, result2.Length - 1) + "卡号表中不存在，请管理员生成" });
            }



            //循环向卡号生成表中录入 当前卡号所属渠道的卡号；并且把卡号和所属促销活动 关联起来
            string result4 = "";
            for (var i = numberbegin; i <= numberend; i++)
            {
                string result3 = "";//单卡号处理时返回的内容
                bool isenter = CommonEnterCardNumber(issueid, i, comid, out   result1, issue);
                if (isenter == false)
                {
                    result4 += i + result3 + ",";
                }
            }
            if (result4 != "")
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = result4.Substring(0, result4.Length - 1) });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = numberbegin + "-" + numberend + "卡号录入成功" });
            }
        }
        /// <summary>
        /// 公用录入单卡号方法 result:录入成功返回cardid，错误返回错误原因
        /// </summary>
        /// <returns></returns>
        public static bool CommonEnterCardNumber(int issueid, decimal cardnumber, int comid, out string result, Member_Issue issue)
        {
            Member_Card cardinfo = new MemberCardData().GetCardByCardNumber(cardnumber);


            //判断卡号是否存在属于此次发行的卡片类型
            int cardtype = cardinfo.Crid;
            int issuecardtype = issue.Crid;
            if (cardtype != issuecardtype)
            {
                result = "输入的卡号所属类型和发行需要的卡号类型不相符";
                return false;
            }

            Member_Channel channel = new MemberChannelData().GetChannelDetail(issue.Chid);
            List<Member_Issue_Activity> actlist = new MemberIssueActivityData().GetIssuePromot(issueid);

            try
            {
                var upMemberCardd = new MemberCardData().UPChannelCardCode(channel.Id, cardnumber, issueid);//更改生成卡号表中卡号所属渠道的id和发行id

                foreach (Member_Issue_Activity issueact in actlist)
                {
                    Member_Card_Activity cardactinfo = new MemberCardActivityData().GetCardActInfo(cardinfo.Id, issueact.Acid);
                    //防止一张卡重复录入同一次活动
                    if (cardactinfo == null)
                    {
                        Member_Card_Activity cardact = new Member_Card_Activity()
                        {
                            Id = 0,
                            ACTID = issueact.Acid,
                            Actnum = 1,
                            CardID = cardinfo.Id,
                            USEstate = (int)MemberCardUserState.NotUse,
                            USEsubdate = DateTime.Parse("1900-01-01")
                        };
                        var insertcardact = new MemberCardActivityData().EditMemberCardActivity(cardact);
                    }
                }
                result = upMemberCardd.ToString();
                return true;
            }
            catch (Exception ex)
            {
                new SqlHelper().Dispose();
                result = "意外错误";
                return false;
                throw;
            }
        }
    }
}
