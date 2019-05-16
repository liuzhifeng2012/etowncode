using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.Member.Service.MemberService.Model;
using ETS.Data.SqlHelper;
using ETS2.Member.Service.MemberService.Data.InternalData;

namespace ETS2.Member.Service.MemberService.Data
{
    public class MemberChannelData
    {
        public Member_Channel GetChannelDetail(int channelid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalMemberChannel(helper).GetChannelDetail(channelid);

                return list;
            }
        }
        public Member_Channel GetChannelDetail(int channelid, int comid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalMemberChannel(helper).GetChannelDetail(channelid, comid);

                return list;
            }
        }

        public int GetChannelidbymanageuserid(int manageuserid, int comid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalMemberChannel(helper).GetChannelidbymanageuserid(manageuserid, comid);

                return list;
            }
        }
        public int GetanageuseridbymChannelid(int channelid, int comid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalMemberChannel(helper).GetmanageuseridbyChannelid(channelid, comid);

                return list;
            }
        }

        #region 判断手机是否存在
        public bool Ishasphone(string mobile, int comid, out int cid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberChannel(sql);
                    var user = internalData.Ishasphone(mobile, comid, out cid);
                    return user;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        public int EditChannel(Member_Channel channel)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalMemberChannel(helper).EditChannel(channel);
                return id;
            }
        }

        public List<Member_Channel> PageList(string companyid, int pageindex, int pagesize, string issuetype, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalMemberChannel(helper).PageList(companyid, pageindex, pagesize, issuetype, out totalcount);

                return list;
            }
        }
        public List<Member_Channel> PageList(string companyid, int pageindex, int pagesize, string issuetype, int channelcompanyid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalMemberChannel(helper).PageList(companyid, pageindex, pagesize, issuetype, channelcompanyid, out totalcount);

                return list;
            }
        }
        public List<Member_Channel> Channelstatistics(string comid, int pageindex, int pagesize, string issuetype, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalMemberChannel(helper).Channelstatistics(comid, pageindex, pagesize, issuetype, out totalcount);

                return list;
            }
        }
        public List<Member_Channel> Channelstatistics2(string comid, int pageindex, int pagesize, string issuetype, string companystate, out int totalcount, int channelcompanyid = 0)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalMemberChannel(helper).Channelstatistics2(comid, pageindex, pagesize, issuetype, companystate, out totalcount, channelcompanyid);

                return list;
            }
        }
        public List<Member_Channel> ChannelYk(string comid, int pageindex, int pagesize, string issuetype, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalMemberChannel(helper).ChannelYk(comid, pageindex, pagesize, issuetype, out totalcount);

                return list;
            }
        }

        public List<Member_Channel> SeachList(string comid, int pageindex, int pagesize, string key, int select, bool isNum, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalMemberChannel(helper).SeachList(comid, pageindex, pagesize, key, select, isNum, out totalcount);

                return list;
            }
        }
        public List<Member_Channel> SeachList(string comid, int pageindex, int pagesize, string key, int select, bool isNum, int channelcompanyid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalMemberChannel(helper).SeachList(comid, pageindex, pagesize, key, select, isNum, channelcompanyid, out totalcount);

                return list;
            }
        }
        public List<Member_Channel> GetChannelByCompanyid(string companyid)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalMemberChannel(helper).GetChannelListByCompanyid(companyid);
                return list;
            }
        }


        /// <summary>
        /// 根据卡号得到所属渠道的详细信息
        /// </summary>
        /// <param name="Cardcode"></param>
        /// <returns></returns>
        public Member_Channel GetChannelDetailByCardNo(string Cardcode)
        {
            using (var helper = new SqlHelper())
            {

                var channel = new InternalMemberChannel(helper).GetChannelDetailByCardNo(Cardcode);

                return channel;
            }
        }



        #region 未发行的卡，门市第一次绑卡，自动分配渠道及服务专员
        public string RegCardChannel_Server_Auto(string card, int ChannelCard, int ServerCard, int comid)
        {
            using (var helper = new SqlHelper())
            {

                var crmid = new InternalMemberChannel(helper).RegCardChannel_Server_Auto(card, ChannelCard, ServerCard, comid);

                return crmid.ToString();
            }
        }
        #endregion


        /// <summary>
        /// 根据卡号得到自己的渠道详细信息
        /// </summary>
        /// <param name="Cardcode"></param>
        /// <returns></returns>
        public Member_Channel GetSelfChannelDetailByCardNo(string Cardcode)
        {
            using (var helper = new SqlHelper())
            {

                var channel = new InternalMemberChannel(helper).GetSelfChannelDetailByCardNo(Cardcode);

                return channel;
            }
        }

        /// <summary>
        ///通过手机号，COMID获得渠道信息
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="comid"></param>
        /// <returns></returns>
        public Member_Channel GetPhoneComIdChannelDetail(string phone, int comid)
        {
            using (var helper = new SqlHelper())
            {

                var channel = new InternalMemberChannel(helper).GetPhoneComIdChannelDetail(phone, comid);

                return channel;
            }
        }


        /// <summary>
        /// 得到WEB或微信渠道号
        /// </summary>
        /// <param name="Cometype"></param>
        /// <returns></returns>
        public Member_Channel GetChannelWebWeixin(string Cometype, int comid)
        {
            using (var helper = new SqlHelper())
            {

                var channel = new InternalMemberChannel(helper).GetChannelWebWeixin(Cometype, comid);

                return channel;
            }
        }
        /// <summary>
        ///  渠道id获得详细
        /// </summary>
        /// <param name="channelid"></param>
        /// <returns></returns>
        public static Member_Channel GetChannelinfo(int channelid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalMemberChannel(helper).GetChannelDetail(channelid);

                return list;
            }
        }

        #region 根据名称得到卡片信息
        public Member_Channel GetCardByCardName(string name)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var cardinfo = new InternalMemberChannel(sql).GetCardByCardName(name);
                    return cardinfo;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        //修改渠道和推荐人
        public string UpchannlT(int uid, int channlid, int uptype)
        {
            using (var helper = new SqlHelper())
            {
                var pro = new InternalMemberChannel(helper).UpchannlT(uid, channlid, uptype);
                return pro;
            }
        }


        //通过id查询类型
        public static Member_Channel Upstring(string cardchl)
        {
            try
            {
                Member_Channel list = null;
                var channelid = MemberCardData.GetCardNumber(decimal.Parse(cardchl));
                using (var helper = new SqlHelper())
                {
                    if (channelid != null)
                    {
                        if (int.Parse(channelid.IssueCard.ToString()) != 0)
                            list = new InternalMemberChannel(helper).GetChannelDetail(int.Parse(channelid.IssueCard.ToString()));
                    }
                    return list;
                }
            }
            catch 
            {
                return null;
            }
        }


        //通过id查询类型
        public static string SearchNamestring(string cardchl)
        {
            try
            {
                Member_Channel list = null;
                var channelid = MemberCardData.GetCardNumber(decimal.Parse(cardchl));
                using (var helper = new SqlHelper())
                {
                    if (channelid != null)
                    {
                        if (int.Parse(channelid.IssueCard.ToString()) != 0)
                            list = new InternalMemberChannel(helper).GetChannelDetail(int.Parse(channelid.IssueCard.ToString()));

                        if (list != null)
                        {
                            return list.Name;
                        }
                        else
                        {
                            return "";
                        }

                    }
                    return "";
                }
            }
            catch 
            {
                return "";
            }
        }

        public List<Member_Channel> GetAllInnerChannels(out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                var pro = new InternalMemberChannel(helper).GetAllInnerChannels(out totalcount);
                return pro;
            }
        }

        public Member_Channel_company GetChannelCompanyById(int channelcompanyid)
        {
            using (var helper = new SqlHelper())
            {
                var model = new InternalMemberChannelcompany(helper).GetChannelCompanyById(channelcompanyid);
                return model;
            }
        }

        public int HandleQrCodeCreateStatus(int channelcompanyid, string checkstatus)
        {
            using (var helper = new SqlHelper())
            {
                var model = new InternalMemberChannelcompany(helper).HandleQrCodeCreateStatus(channelcompanyid, checkstatus);
                return model;
            }
        }

        public List<Member_Channel> SearchChannelByChannelUnit(string comid, int pageindex, int pagesize, int channelcompanyid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalMemberChannel(helper).SearchChannelByChannelUnit(comid, pageindex, pagesize, channelcompanyid, out totalcount);

                return list;
            }
        }

        public List<Member_Channel> Channellistbycomid(string comid, int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                List<Member_Channel> list = new InternalMemberChannel(helper).Channellistbycomid(comid, pageindex, pagesize, out totalcount);

                return list;
            }
        }

        public Member_Channel GetChannelByOpenId(string openid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalMemberChannel(helper).GetChannelByOpenId(openid);

                return list;
            }
        }

        public int GetDefaultChannelNum(int companyid)
        {
            using (var helper = new SqlHelper())
            {

                int result = new InternalMemberChannel(helper).GetDefaultChannelNum(companyid);

                return result;
            }
        }
        /// <summary>
        /// 根据渠道单位编号获得默认渠道人
        /// </summary>
        /// <param name="channelcompanyid"></param>
        /// <returns></returns>
        public Member_Channel GetDefaultChannel(int channelcompanyid)
        {
            using (var helper = new SqlHelper())
            {

                Member_Channel result = new InternalMemberChannel(helper).GetDefaultChannel(channelcompanyid);

                return result;
            }
        }

        /// <summary>
        /// 根据渠道单位编号获得默认渠道人
        /// </summary>
        /// <param name="channelcompanyid"></param>
        /// <returns></returns>
        public int GetChannelImgbyopenid(string openid)
        {
            using (var helper = new SqlHelper())
            {

                var result = new InternalMemberChannel(helper).GetChannelImgbyopenid(openid);

                return result;
            }
        }

        public List<Member_Channel> GetChannelList(int channelcomid, string runstate, string whetherdefaultchannel, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                List<Member_Channel> List = new InternalMemberChannel(helper).GetChannelList(channelcomid, runstate, whetherdefaultchannel, out totalcount);
                return List;
            }
        }
        /// <summary>
        /// 根据公司id和渠道发行人查询
        /// </summary>
        /// <param name="comid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Member_Channel GetChannel(int comid, string name)
        {
            using (var helper = new SqlHelper())
            {

                Member_Channel result = new InternalMemberChannel(helper).GetChannel(comid, name);

                return result;
            }
        }

        public int EditSimplyChannel(int id, int Com_id, string Employeename, string Channelcompanyid, string tel = "",int channelsource=0,int channelstate=1)
        {
            using (var helper = new SqlHelper())
            {

                int result = new InternalMemberChannel(helper).EditSimplyChannel(id, Com_id, Employeename, Channelcompanyid, tel, channelsource, channelstate);

                return result;
            }
        }

        public bool IsRz_fuwuno(int comid)
        {
            try
            {
                string sql = "select weixintype from weixinbasic where comid=" + comid;
                var cmd = new SqlHelper().PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                new SqlHelper().Dispose();
                if (o.ToString() == "4")
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
                new SqlHelper().Dispose();
                return false;
            }
        }

        /// <summary>
        /// 顾问锁定客户
        /// </summary>
        /// <param name="channleid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public int WxMessageLockUser(int channleid, string openid)
        {
            using (var helper = new SqlHelper())
            {

                var result = new InternalMemberChannel(helper).WxMessageLockUser(channleid, openid);

                return result;
            }
        }

        /// <summary>
        /// 锁定客户最后通话时间
        /// </summary>
        /// <param name="channleid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public int WxMessageLockUserTime(int channleid, string openid)
        {
            using (var helper = new SqlHelper())
            {

                var result = new InternalMemberChannel(helper).WxMessageLockUserTime(channleid, openid);

                return result;
            }
        }


        /// <summary>
        /// 解除锁定客户
        /// </summary>
        /// <param name="channleid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public int WxMessageUnLockUser(int channleid)
        {
            using (var helper = new SqlHelper())
            {

                var result = new InternalMemberChannel(helper).WxMessageUnLockUser(channleid);

                return result;
            }
        }
        /// <summary>
        /// 锁死用户
        /// </summary>
        /// <param name="channleid"></param>
        /// <param name="locktype">1=锁死，0=解除锁死</param>
        /// <returns></returns>
        public int WxMessageDeadLock(int channleid, int locktype)
        {
            using (var helper = new SqlHelper())
            {

                var result = new InternalMemberChannel(helper).WxMessageDeadLock(channleid, locktype);

                return result;
            }
        }

        /// <summary>
        /// 客户解除绑定顾问
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="comid"></param>
        /// <returns></returns>
        public int UserUnlockChannel(decimal cardid, int comid)
        {
            using (var helper = new SqlHelper())
            {

                var result = new InternalMemberChannel(helper).UserUnlockChannel(cardid, comid);

                return result;
            }
        }


        //获取商户或门市渠道列表，只显示微信绑定的在线列表
        public List<Member_Channel> GetChannelListByComid(int comid, int companyid, int channeltype, out int totalcount, int channelid = 0)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalMemberChannel(helper).GetChannelListByComid(comid, companyid, channeltype, out totalcount, channelid);
                return list;
            }
        }


        //获取员工账户是否绑定
        public int GetChannelListByComidState(int comid, int channelid)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalMemberChannel(helper).GetChannelListByComidState(comid, channelid);
                return list;
            }
        }



        /// <summary>
        /// 超时，解除锁定客户
        /// </summary>
        /// <param name="channleid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public int WxMessageUnLockUserTimeout()
        {
            using (var helper = new SqlHelper())
            {

                var result = new InternalMemberChannel(helper).WxMessageUnLockUserTimeout();

                return result;
            }
        }


        public Member_Channel GetChannelByMasterId(int masterid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalMemberChannel(helper).GetChannelByMasterId(masterid);

                return list;
            }
        }
        /// <summary>
        /// 根据微信号判断是否是顾问
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="comid"></param>
        /// <returns></returns>
        public int IsGuwen(string openid, int comid)
        {
            using (var helper = new SqlHelper())
            {

                int r = new InternalMemberChannel(helper).IsGuwen(openid, comid);

                return r;
            }
        }

        public decimal Getrestrebate(int comid, string phone)
        {
            using (var helper = new SqlHelper())
            {

                decimal r = new InternalMemberChannel(helper).Getrestrebate(comid,phone);

                return r;
            }
        }

        public int GetChannelid(int comid, string phone)
        {
            using (var helper = new SqlHelper())
            {

                int r = new InternalMemberChannel(helper).GetChannelid(comid, phone);

                return r;
            }
        }
    }
}
