using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ETS.Data.SqlHelper;
using System.Collections;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Member.Service.MemberService.Data;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.Member.Service.MemberService.Model.Enum;

namespace ETS.JsonFactory
{
    public class CardJsonDate
    {

        #region 根据id得到卡号管理
        public static string GetCardById(int id)
        {

            try
            {

                var pro = MemberCardData.GetCardById(id);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion
        #region 根据id得到卡号管理
        public static string GetCardById(int comid, int id)
        {

            try
            {

                var pro = MemberCardData.GetCardById(comid, id);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion
        #region 得到First段卡号
        public static string GetCardFirst(int id)
        {

            try
            {

                var pro = MemberCardData.GetCardFirst(id);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 添加或修改促销活动信息
        public static string EditCardInfo(Member_Card cardinfo)
        {

            try
            {

                var pro = MemberCardData.EditCardInfo(cardinfo);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 促销活动列表
        public static string CardPageList(string comid, int pageindex, int pagesize)
        {

            var totalcount = 0;
            try
            {

                var actdata = new MemberCardData();
                var list = actdata.CardPageList(comid, pageindex, pagesize, out totalcount);
                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Com_id = pro.Com_id,
                                 Cname = pro.Cname,
                                 Ctype = pro.Ctype == 1 ? "实体卡" : "非实体卡",
                                 Printnum = pro.Printnum,
                                 Qrcode = pro.Qrcode,
                                 Zhuanzeng = pro.Zhuanzeng,
                                 Remark = pro.Remark,
                                 Exchange = pro.Exchange,
                                 CardRule = pro.CardRule,
                                 CardRule_First = pro.CardRule_First,
                                 CardRule_starnum = pro.CardRule_starnum,
                                 Outstate = pro.Outstate == false ? "立即导出" : "已导出",
                                 Subdate = pro.Subdate,
                                 Createstate = pro.Createstate == false ? "立即生成卡号" : "已生成",

                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 根据发行id得到卡片类型信息
        public static string GetCardByIssueId(int issueid)
        {
            try
            {

                var pro = MemberCardData.GetCardByIssueId(issueid);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion
        #region 得到不同渠道，活动，发行 下的卡号列表
        public static string GetMemberCardList(int comid, decimal cardcode, int pageindex, int pagesize, int issueid, int channelid, int actid, int isopencard)
        {
            var totalcount = 0;
            try
            {
                var prodata = new MemberCardData();
                if (cardcode.ToString().Trim().Length == 11)//会员电话号码
                {
                    var carddata = new B2bCrmData().GetB2bCrmByPhone(comid,cardcode.ToString());
                    if (carddata != null) {
                        cardcode = carddata.Idcard;
                    }
                }


                var list = prodata.GetMemberCardList(comid, cardcode, pageindex, pagesize, issueid, channelid, actid, isopencard, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 CardCode = pro.Cardcode,
                                 Name = pro.Openstate == 0 ? "--" : new B2bCrmData().GetB2bCrmByCardcode(pro.Cardcode) == null ? "" : new B2bCrmData().GetB2bCrmByCardcode(pro.Cardcode).Name,
                                 Phone = pro.Openstate == 0 ? "--" : new B2bCrmData().GetB2bCrmByCardcode(pro.Cardcode) == null ? "" : new B2bCrmData().GetB2bCrmByCardcode(pro.Cardcode).Phone,
                                 Imprest = pro.Openstate == 0 ? 0 : new B2bCrmData().GetB2bCrmByCardcode(pro.Cardcode) == null ? 0 : new B2bCrmData().GetB2bCrmByCardcode(pro.Cardcode).Imprest,
                                 Integral = pro.Openstate == 0 ? 0 : new B2bCrmData().GetB2bCrmByCardcode(pro.Cardcode) == null ? 0 : new B2bCrmData().GetB2bCrmByCardcode(pro.Cardcode).Integral,
                                 OpenSubDate = pro.Openstate == 0 ? "--" : pro.Opensubdate.ToString("yyyy-MM-dd") == "1900-01-01" ? "--" : pro.Opensubdate.ToString("yyyy-MM-dd"),
                                 ChannelName = pro.IssueCard == 0 ? "--" : new MemberChannelData().GetChannelDetail(int.Parse(pro.IssueCard.ToString())).Name,
                                 ActStr = pro.IssueCard == 0 ? "--" : new MemberIssueActivityData().GetIssueActStr(pro.IssueId),
                                 IssueTitle = pro.IssueCard == 0 ? "--" : new MemberIssueData().GetIssueDetailById(pro.IssueId) == null ? "" : new MemberIssueData().GetIssueDetailById(pro.IssueId).Title,
                                 EnteredState = pro.IssueCard == 0 ? "未录入" : "已录入",
                                 OpenState = pro.Openstate == 0 ? "未开卡" : "已开卡"
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 查询卡号管理是否已开卡，用于验证卡
        public static string SearchCard(string pno, int comid, out int userinfo)
        {
            try
            {
                MemberCardData carddate = new MemberCardData();
                var pro = carddate.SearchCard(pno, comid, out userinfo);
                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                userinfo = 0;
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 确认使用活动卡号管理
        public static string EconfirmCard(int aid, int actid, int cardid, int comid, out Member_Activity actinfo, out string phone, out string name, out decimal idcard, out decimal agcardcode)
        {

            try
            {
                MemberCardData carddate = new MemberCardData();
                var pro = carddate.EconfirmCard(aid, actid, cardid, comid, out actinfo, out phone, out name, out idcard, out agcardcode);


                Member_Channel channel = new MemberChannelData().GetChannelDetailByCardNo(idcard.ToString());//根据卡号得到所属渠道的详细信息


                if (channel != null)
                {
                    //给渠道表中 开卡数量和总金额赋值
                    channel.Firstdealnum = channel.Firstdealnum + 1;
                    channel.Summoney = channel.Summoney + channel.RebateConsume;
                    var channeldata = new MemberChannelData().EditChannel(channel);

                    //把返佣日志录入渠道返佣日志表
                    ChannelRebateLog channelrebatelog = new ChannelRebateLog()
                    {
                        Id = 0,
                        Channelid = channel.Id,
                        Execdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        Rebatemoney = channel.RebateConsume,
                        Type = (int)ChannelRebateType.FirstDeal,
                        Summoney = channel.Summoney,
                        Remark = "消费返佣" + channel.RebateConsume + "元"
                    };
                    var channelrebatelogret = new ChannelRebateLogData().EditChannelRebateLog(channelrebatelog);

                }

                return JsonConvert.SerializeObject(new { type = 100, actinfo = actinfo, phone = phone, name = name, idcard = idcard, agcardcode = agcardcode, msg = pro });
            }
            catch (Exception ex)
            {
                actinfo = null;
                phone = "";
                name = "";
                idcard = 0;
                agcardcode = 0;
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 获得活动卡号管理
        public static string GetCardInfo(int logid, int pno, int comid)
        {
            try
            {
                MemberCardData carddate = new MemberCardData();
                var pro = carddate.GetCardInfo(logid, pno, comid);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 电脑验卡
        public static string GetCarValidate(int actid, int cardid, int orderid, string servername, int num_people, int per_capita_money, int menber_return_money, string sales_admin, int comid, string AccountName)
        {
            try
            {
                MemberCardData carddate = new MemberCardData();
                var pro = carddate.GetCarValidate(actid, cardid, orderid, servername, num_people, per_capita_money, menber_return_money, sales_admin, comid, AccountName);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion
    }
}
