using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using System.Data;
using ETS2.Member.Service.MemberService.Data.InternalData;
using ETS2.Member.Service.MemberService.Model;

namespace ETS2.Member.Service.MemberService.Data
{
    public class MemberCardData
    {

        #region 根据id得到卡号管理
        public static Member_Card GetCardById(int actid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberCard(sql);
                    Member_Card result = internalData.GetCardById(actid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion
        #region 根据id得到卡号管理
        public static Member_Card GetCardById(int comid, int actid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberCard(sql);
                    Member_Card result = internalData.GetCardById(comid, actid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion
        public static Member_Card GetCardId(int cardid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberCard(sql);
                    Member_Card result = internalData.GetCardId(cardid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #region 根据id得到卡号管理
        public static Member_Card GetCardFirst(int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberCard(sql);
                    Member_Card result = internalData.GetCardFirst(comid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion

        #region 编辑活动信息
        public static int EditCardInfo(Member_Card info)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberCard(sql);
                    int result = internalData.InsertOrUpdate(info);

                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion


        #region 创建随机卡号,1为网站2002，2为微信2001
        public static string CreateECard(int carttype, int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberCard(sql);
                    string result = internalData.CreateECard(carttype, comid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion

        #region 查询积分，网站注册，和微信注册。活动自动执行 faceobjects：类型3=网站，4=微信,
        public static decimal AutoInputMoeny(int uid, int faceobjects, int comid, out int jifen_range)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberCard(sql);
                    var result = internalData.AutoInputMoeny(uid, faceobjects, comid, out jifen_range);

                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion

        #region 促销活动列表
        public List<Member_Card> CardPageList(string comid, int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalMemberCard(helper).CardPageList(comid, pageindex, pagesize, out totalcount);
                return list;
            }

        }
        #endregion


        #region 得到随机编号码
        public static int GetRandomCode()
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberCard(sql);
                    int result = internalData.GetRandomCode();
                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion



        #region 编辑卡号 by:xiaoliu
        public int EditMemberCard(Member_Card membercard)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    int result = new InternalMemberCard(sql).EditMemberCard(membercard);
                    return result;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

        }
        #endregion
        #region 根据卡号的卡片信息
        public Member_Card GetCardByCardNumber(decimal cardnumber)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var cardinfo = new InternalMemberCard(sql).GetCardByCardNumber(cardnumber);
                    return cardinfo;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 根据卡号的卡片信息
        public static Member_Card GetCardNumber(decimal cardnumber)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var cardinfo = new InternalMemberCard(sql).GetCardByCardNumber(cardnumber);
                    return cardinfo;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region  更改生成卡号表中卡号所属渠道的卡号
        public int UPChannelCardCode(decimal channelcardcode, decimal cardnumber, int issueid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberCard(sql);
                    int result = internalData.UPChannelCardCode(channelcardcode, cardnumber, issueid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        public int GetEnteredNumber(int issueid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberCard(sql);
                    int result = internalData.GetEnteredNumber(issueid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public bool IsHasEntered(decimal cardnumber)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberCard(sql);
                    int result = internalData.IsHasEntered(cardnumber);
                    if (result > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        public static Member_Card GetCardByIssueId(int issueid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberCard(sql);
                    Member_Card result = internalData.GetCardByIssueId(issueid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #region 根据卡类型id得到卡类型实体
        public Member_Card GetCardCreateByCrid(int crid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberCard(sql);
                    Member_Card result = internalData.GetCardCreateByCrid(crid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion
        #region 根据发行id得到开卡数量
        public int GetOpenCardNum(int issueid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberCard(sql);
                    int result = internalData.GetOpenCardNum(issueid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        public int GetEnteredNumberByChannelId(int channelid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberCard(sql);
                    int result = internalData.GetEnteredNumberByChannelId(channelid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }


        #region 根据卡号、手机判断有效性
        public string SearchCard(string pno, int comid, out int userinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var cardinfo = new InternalMemberCard(sql).SearchCard(pno, comid, out userinfo);
                    return cardinfo;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion



        #region 确认卡号活动
        public string EconfirmCard(int aid, int actid, int cardid, int comid, out Member_Activity actinfo, out string phone, out string name, out decimal idcard, out decimal agcardcode)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var cardinfo = new InternalMemberCard(sql).EconfirmCard(aid, actid, cardid, comid, out actinfo, out phone, out name, out idcard, out agcardcode);
                    return cardinfo;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 获得卡号活动
        public string GetCardInfo(int logid, int pno, int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var cardinfo = new InternalMemberCard(sql).GetCardInfo(logid, pno, comid);
                    return cardinfo;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 电脑验卡
        public string GetCarValidate(int actid, int cardid, int orderid, string servername, int num_people, int per_capita_money, int menber_return_money, string sales_admin, int comid, string AccountName)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var cardinfo = new InternalMemberCard(sql).GetCarValidate(actid, cardid, orderid, servername, num_people, per_capita_money, menber_return_money, sales_admin, comid, AccountName);
                    return cardinfo;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        public List<Member_Card> GetMemberCardList(int comid, decimal cardcode, int pageindex, int pagesize, int issueid, int channelid, int actid, int isopencard, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalMemberCard(helper).GetMemberCardList(comid, cardcode, pageindex, pagesize, issueid, channelid, actid, isopencard, out totalcount);
                return list;
            }
        }

        #region membercard 卡号和渠道id联系起来
        public int upCardcodeChannel(string cardcode, int channelid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberCard(sql);
                    int result = internalData.upCardcodeChannel(cardcode, channelid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        public Member_Card GetMemberCardByOpenId(string openid)
        {
            using (var helper = new SqlHelper())
            {
                Member_Card list = new InternalMemberCard(helper).GetMemberCardList(openid);
                return list;
            }
        }

        public int UpMemberChennel(string openid, int channelid)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalMemberCard(helper).UpMemberChennel(openid, channelid);
                return result;
            }
        }

        public int IsHasOutCardcode(int comid, string outcardcode)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalMemberCard(helper).IsHasOutCardcode(comid, outcardcode);
                return result;
            }
        }

        public int InsOutMemberCardcode(string outcardcode, int isused, int comid, int imlogid)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalMemberCard(helper).InsOutMemberCardcode(outcardcode, isused, comid, imlogid);
                return result;
            }
        }

        public int Insoutcardcodeimlog(string initfilename, string relativepath, int imtor, int comid, string imtime)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalMemberCard(helper).Insoutcardcodeimlog(initfilename, relativepath, imtor, comid, imtime);
                return result;
            }
        }
        /// <summary>
        /// 得到不可用的 外部导入卡号
        /// </summary>
        /// <param name="comid"></param>
        /// <returns></returns>
        internal string GetUnusedOutCardcode(int comid)
        {
            using (var helper = new SqlHelper())
            {
                string result = new InternalMemberCard(helper).GetUnusedOutCardcode(comid);
                return result;
            }
        }
    }
}
