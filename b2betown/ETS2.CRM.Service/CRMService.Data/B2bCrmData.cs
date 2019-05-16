using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Data.InternalData;
using ETS2.Member.Service.MemberService.Model;
using System.Data;

namespace ETS2.CRM.Service.CRMService.Data
{
    public class B2bCrmData
    {
        #region 编辑易城系统会员信息
        public int InsertOrUpdate(B2b_crm crm)
        {
            using (var helper = new SqlHelper())
            {

                var crmid = new InternalB2bCrm(helper).InsertOrUpdate(crm);

                return crmid;
            }
        }
        #endregion


        #region 根据卡号判断其有效性
        public string GetCard(string card, int comid)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2bCrm(helper).GetCard(card, comid);

                return list;
            }
        }
        #endregion


        #region 根据渠道卡号判断其有效性
        public string GetChannelCard(string card, int comid)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2bCrm(helper).GetChannelCard(card, comid);

                return list;
            }
        }
        #endregion

        #region 根据卡号判断其有效性
        public string GetEmail(string email, int comid)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2bCrm(helper).GetEmail(email, comid);

                return list;
            }
        }
        #endregion

        #region 根据手机号判断其有效性
        public string GetPhone(string phone, int comid)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2bCrm(helper).GetPhone(phone, comid);

                return list;
            }
        }
        #endregion

        #region 判断key有效性
        public string Getkey(string key)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2bCrm(helper).Getkey(key);

                return list;
            }
        }
        #endregion

        #region 判断微信号是否有用过
        public bool GetWeixin(string openid, int comid)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2bCrm(helper).GetWeixin(openid, comid);

                return list;
            }
        }
        #endregion

        #region 登陆
        public string Login(string account, int accounttype, string pwd, int comid, out B2b_crm userinfo)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2bCrm(helper).Login(account, accounttype, pwd, comid, out userinfo);

                return list;
            }
        }
        #endregion

        #region 修改密码、手机
        public string UpMemberpass(int com_id, int id, string pass)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2bCrm(helper).UpMemberpass(com_id, id, pass);

                return list;
            }
        }

        public string UpMemberphone(int com_id, int id, string phone)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2bCrm(helper).UpMemberphone(com_id, id, phone);

                return list;
            }
        }

        public string UpMembermail(int com_id, int id, string mail)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2bCrm(helper).UpMembermail(com_id, id, mail);

                return list;
            }
        }

        public string UpMembername(int com_id, int id, string name)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2bCrm(helper).UpMembername(com_id, id, name);

                return list;
            }
        }

        public string UpMembercard(int com_id, string phone, decimal card)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2bCrm(helper).UpMembercard(com_id, phone, card);

                return list;
            }
        }

        #endregion

        #region 微信通过密码登陆，并同时绑定
        public string WeixinPassLogin(string phone, string weixin, string pwd, int comid, out B2b_crm userinfo)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2bCrm(helper).WeixinPassLogin(phone, weixin, pwd, comid, out userinfo);

                return list;
            }
        }
        #endregion

        #region 微信通过Cookie登陆，并同时绑定
        public string WeixinCookieLogin(string accountid, string accountmd5, int comid, out B2b_crm userinfo)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2bCrm(helper).WeixinCookieLogin(accountid, accountmd5, comid, out userinfo);

                return list;
            }
        }
        #endregion


        #region 登陆
        public string WeixinLogin(string openid, string weixinpass, int comid, out B2b_crm userinfo)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2bCrm(helper).WeixinLogin(openid, weixinpass, comid, out userinfo);

                return list;
            }
        }
        #endregion


        #region 读取用户信息
        public B2b_crm Readuser(int accountid, int comid)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2bCrm(helper).Readuser(accountid, comid);

                return list;
            }
        }
        #endregion

        #region h5 通过 openid查询用户信息
        public B2b_crm b2b_crmH5(string openid, int comid)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2bCrm(helper).b2b_crmH5(openid, comid);

                return list;
            }
        }
        #endregion

        //初始会员密码
        public string initialuser(B2b_crm b2binfo)
        {
            using (var helper = new SqlHelper())
            {

                var crmid = new InternalB2bCrm(helper).initialuser(b2binfo);

                return crmid.ToString();
            }
        }

        #region crm活动日志详细页面
        public Member_Activity_Log Logdetails(int id, int comid)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2bCrm(helper).Logdetails(id, comid);

                return list;
            }
        }
        #endregion

        #region 根据Card和ACT得到日志
        public List<Member_Activity_Log> Logcardact(int actid, int cardid, int comid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2bCrm(helper).Logcardact(actid, cardid, comid, out totalcount);

                return list;
            }
        }
        #endregion

        #region 卡号绑定与注册
        public string RegCard(string card, string email, string password1, string name, string phone, int comid, string sex)
        {
            using (var helper = new SqlHelper())
            {

                var crmid = new InternalB2bCrm(helper).RegCard(card, email, password1, name, phone, comid, sex);

                return crmid.ToString();
            }
        }
        #endregion

        #region 用户注册
        public int UsereRegCard(string cardid, string email, string password1, string name, string phone, int comid, string sex, int channelid)
        {
            using (var helper = new SqlHelper())
            {

                var crmid = new InternalB2bCrm(helper).UsereRegCard(cardid, email, password1, name, phone, comid, sex, channelid);

                return crmid;
            }
        }
        #endregion

        #region 修改会员信息
        public string UpMember(B2b_crm b2binfo)
        {
            using (var helper = new SqlHelper())
            {

                var crmid = new InternalB2bCrm(helper).UpMember(b2binfo);

                return crmid.ToString();
            }
        }
        #endregion

        #region 微信编辑会员信息
        public string weiUpMember(B2b_crm b2binfo)
        {
            using (var helper = new SqlHelper())
            {

                var crmid = new InternalB2bCrm(helper).weiUpMember(b2binfo);

                return crmid.ToString();
            }
        }
        #endregion

        #region 解绑手机
        public string UserUnlockPhone(string openid, int comid)
        {
            using (var helper = new SqlHelper())
            {

                var crmid = new InternalB2bCrm(helper).UserUnlockPhone(openid, comid);

                return crmid.ToString();
            }
        }
        #endregion

        #region 微信绑卡
        public string WeixinRegCard(string card, string openid, string password1, string name, string phone, int comid, int channelid)
        {
            using (var helper = new SqlHelper())
            {

                var crmid = new InternalB2bCrm(helper).WeixinRegCard(card, openid, password1, name, phone, comid, channelid);

                return crmid.ToString();
            }
        }
        #endregion

        #region 设定微信的随机密码
        public string WeixinSetPass(string openid, int comid)
        {
            string password1 = "123456";
            Random ra = new Random();
            int num = ra.Next(100000, 1000000);
            int num2 = ra.Next(100000, 1000000);
            password1 = Convert.ToDouble(num.ToString() + num2.ToString()).ToString();//获取12位随机微信密码

            using (var helper = new SqlHelper())
            {
                var crmid = new InternalB2bCrm(helper).WeixinSetPass(openid, password1, comid);
                return crmid.ToString();
            }
        }
        #endregion


        #region 设定并获得微信密码
        public string WeixinGetPass(string openid, int comid)
        {
            string password1 = "123456";
            Random ra = new Random();
            int num = ra.Next(100000, 1000000);
            int num2 = ra.Next(100000, 1000000);
            password1 = Convert.ToDouble(num.ToString() + num2.ToString()).ToString();//获取12位随机微信密码

            using (var helper = new SqlHelper())
            {
                var crmid2 = new InternalB2bCrm(helper).WeixinSetPass(openid, password1, comid);
            }

            using (var helper = new SqlHelper())
            {
                var crmid = new InternalB2bCrm(helper).WeixinGetPass(openid, comid);
                return crmid.ToString();
            }
        }
        #endregion

        #region 注销微信密码
        public string WeixinConPass(string openid, int comid)
        {
            using (var helper = new SqlHelper())
            {
                var crmid = new InternalB2bCrm(helper).WeixinConPass(openid, comid);
                return crmid.ToString();
            }
        }
        #endregion


        #region 手机注册账户
        public string RegAccount(string card, string phone, string name, string password1, int comid, int channelid)
        {
            using (var helper = new SqlHelper())
            {

                var crmid = new InternalB2bCrm(helper).RegAccount(card, phone, name, password1, comid, channelid);

                return crmid.ToString();
            }
        }
        #endregion


        public List<B2b_crm> SJKeHuPageList(string comid, int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bCrm(helper).SJKeHuPageList(comid, pageindex, pagesize, out totalcount);

                return list;
            }
        }
        public List<B2b_crm> SJKeHuPageList(string comid, int pageindex, int pagesize, B2b_company_manageuser user, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bCrm(helper).SJKeHuPageList(comid, pageindex, pagesize, user, out totalcount);

                return list;
            }
        }
        public List<B2b_crm> fuwuPageList(string comid, int pageindex, int pagesize, int user, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bCrm(helper).fuwuPageList(comid, pageindex, pagesize, user, out totalcount);

                return list;
            }
        }

        #region 活动加载明细列表
        public List<Member_Activity_Log> LoadingList(string comid, int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bCrm(helper).LoadingList(comid, pageindex, pagesize, out totalcount);

                return list;
            }
        }
        #endregion
        public List<Member_Activity_Log> LoadingList(string comid, int pageindex, int pagesize, int channelcompanyid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bCrm(helper).LoadingList(comid, pageindex, pagesize, channelcompanyid, out totalcount);

                return list;
            }
        }
        public List<B2b_crm> SearchPageList(string comid, int pageindex, int pagesize, string key, bool isNum, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bCrm(helper).SearchPageList(comid, pageindex, pagesize, key, isNum, out totalcount);

                return list;
            }
        }
        public List<B2b_crm> SearchPageList(string comid, int pageindex, int pagesize, string key, out int totalcount, string isactivate = "1", string iswxfocus = "0,1", string ishasweixin = "0,1", string ishasphone = "0,1")
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bCrm(helper).SearchPageList(comid, pageindex, pagesize, key, out totalcount, isactivate, iswxfocus, ishasweixin, ishasphone);

                return list;
            }
        }

        public string Searchb2bcrmbyopenid(string openid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bCrm(helper).Searchb2bcrmbyopenid(openid);

                return list;
            }
        }

        public List<B2b_crm> SearchPageList(string comid, int pageindex, int pagesize, string key, bool isNum, B2b_company_manageuser userr, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bCrm(helper).SearchPageList(comid, pageindex, pagesize, key, isNum, userr, out totalcount);

                return list;
            }
        }
        public List<B2b_crm> SearchPageList(string comid, int pageindex, int pagesize, string key, B2b_company_manageuser userr, out int totalcount, string isactivate = "1", string iswxfocus = "0,1", string ishasweixin = "0,1")
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bCrm(helper).SearchPageList(comid, pageindex, pagesize, key, userr, out totalcount, isactivate, iswxfocus, ishasweixin);

                return list;
            }
        }

        public List<B2b_crm> SearchPageList1(string comid, int pageindex, int pagesize, string key, int channelcompanyid, out int totalcount, string isactivate = "1", string iswxfocus = "0,1", string ishasweixin = "0,1", string channelcompanytype = "0", bool crmIsAccurateToPerson = false, int userid = 0, string ishasphone = "0,1")
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bCrm(helper).SearchPageList1(comid, pageindex, pagesize, key, channelcompanyid, out totalcount, isactivate, iswxfocus, ishasweixin, channelcompanytype, crmIsAccurateToPerson, userid, ishasphone);

                return list;
            }
        }


        #region  活动使用日志
        public List<Member_Activity_Log> SearchActivityList(string comid, int pageindex, int pagesize, string key, string ServerName, bool isNum, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bCrm(helper).SearchActivityList(comid, pageindex, pagesize, key, ServerName, isNum, out totalcount);

                return list;
            }
        }
        #endregion
        public List<Member_Activity_Log> SearchActivityList(string comid, int pageindex, int pagesize, string key, string ServerName, bool isNum, int channelcompanyid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bCrm(helper).SearchActivityList(comid, pageindex, pagesize, key, ServerName, isNum, channelcompanyid, out totalcount);

                return list;
            }
        }
        public B2b_crm GetSjKeHu(string phone, int comid)
        {

            using (var helper = new SqlHelper())
            {

                var pro = new InternalB2bCrm(helper).GetSjKeHu(phone, comid);

                return pro;
            }
        }

        #region 根据卡号 得到会员的详细信息
        public B2b_crm GetB2bCrmByCardcode(decimal cardcode)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalB2bCrm(helper).GetB2bCrmByCardcode(cardcode);

                return pro;
            }
        }
        #endregion

        #region 根据卡号 得到会员的详细信息
        public static B2b_crm GetCrmCardcode(decimal cardcode)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalB2bCrm(helper).GetB2bCrmByCardcode(cardcode);

                return pro;
            }
        }
        #endregion
        #region 根据电话号码得到会员的详细信息
        public B2b_crm GetB2bCrmByPhone(string phone)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalB2bCrm(helper).GetB2bCrmByPhone(phone);

                return pro;
            }
        }
        #endregion
        #region 根据微信号得到会员的详细信息
        public B2b_crm GetB2bCrmByWeiXin(string weixinno)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalB2bCrm(helper).GetB2bCrmByWeiXin(weixinno);

                return pro;
            }
        }
        #endregion

        public int InsB2bCrm(int comid, string cardcode, string weixinNo, string weixinpass, string registertime, int whetherwxfocus = 0, string crmlevel = "0")
        {
            using (var helper = new SqlHelper())
            {

                var crmid = new InternalB2bCrm(helper).InsB2bCrm(comid, cardcode, weixinNo, weixinpass, registertime, whetherwxfocus, crmlevel);

                return crmid;
            }
        }
        #region 发展会员统计
        public DataTable GetCrmStatistics(int comid, int channelcompanyid)
        {
            using (var helper = new SqlHelper())
            {
                DataTable dt = new InternalB2bCrm(helper).GetCrmStatistics(comid, channelcompanyid);
                return dt;
            }
        }
        #endregion

        public string GetB2bCrm(string openid, int comid, out B2b_crm userinfo)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2bCrm(helper).GetB2bCrm(openid, comid, out userinfo);

                return list;
            }
        }

        public int GetMemberNums(int comid)
        {
            using (var helper = new SqlHelper())
            {
                var data = new InternalB2bCrm(helper).GetMemberNums(comid);

                return data;
            }
        }

        public int GetWeiXinAttentionNum(int comid)
        {
            using (var helper = new SqlHelper())
            {
                int num = new InternalB2bCrm(helper).GetWeiXinAttentionNum(comid);
                return num;
            }
        }

        public int ModifyIDCard(string cardcode, int crmid)
        {
            using (var helper = new SqlHelper())
            {

                int id = new InternalB2bCrm(helper).ModifyIDCard(cardcode, crmid);

                return id;
            }
        }


        public B2b_crm GetB2bCrmByWeiXin(string weixinno, int comid)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalB2bCrm(helper).GetB2bCrmByWeiXin(weixinno, comid);

                return pro;
            }
        }

        public B2b_crm GetB2bCrmByPhone(string phone, int comid)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalB2bCrm(helper).GetB2bCrmByPhone(phone, comid);

                return pro;
            }
        }

        public B2b_crm GetB2bCrm(string openid, int comid)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalB2bCrm(helper).GetB2bCrm(openid, comid);

                return pro;
            }
        }
        /// <summary>
        /// excel导入其他商家的会员
        /// </summary>
        /// <param name="comid"></param>
        /// <param name="cardcode"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="weixin"></param>
        /// <param name="whetherwxfocus"></param>
        /// <param name="whetheractivate"></param>
        /// <param name="registertime"></param>
        /// <returns></returns>

        public int ExcelInsB2bCrm(int comid, string cardcode, string name, string phone, string weixin, int whetherwxfocus, int whetheractivate, string registertime, string email, decimal imprest, decimal integral, string country, string province, string city, string address, string agegroup, string crmlevel)
        {
            using (var helper = new SqlHelper())
            {

                int id = new InternalB2bCrm(helper).ExcelInsB2bCrm(comid, cardcode, name, phone, weixin, whetherwxfocus, whetheractivate, registertime, email, imprest, integral, country, province, city, address, agegroup, crmlevel);

                return id;
            }
        }

        /// <summary>
        /// 修改会员的激活状态
        /// </summary>
        /// <param name="weixin"></param>
        /// <param name="comid"></param>
        /// <param name="activatestate"></param>
        /// <returns></returns>
        public int ModifyCrmActivate(string weixin, int comid, int activatestate)
        {
            using (var helper = new SqlHelper())
            {

                int id = new InternalB2bCrm(helper).ModifyCrmActivate(weixin, comid, activatestate);

                return id;
            }
        }



        public B2b_crm GetB2bCrmByPhone(int comid, string Phone)
        {
            using (var helper = new SqlHelper())
            {
                B2b_crm crm = new InternalB2bCrm(helper).GetB2bCrmByPhone(comid, Phone);
                return crm;
            }
        }

        public bool IsHasCrmWeiXin(int comid, string weixin)
        {
            using (var helper = new SqlHelper())
            {
                bool result = new InternalB2bCrm(helper).IsHasCrmWeiXin(comid, weixin);
                return result;
            }
        }

        public bool IsHasCrmPhone(int comid, string phone)
        {
            using (var helper = new SqlHelper())
            {
                bool result = new InternalB2bCrm(helper).IsHasCrmPhone(comid, phone);
                return result;
            }
        }

        public B2b_crm GetB2bCrmById(int crmid)
        {
            using (var helper = new SqlHelper())
            {
                B2b_crm crm = new InternalB2bCrm(helper).GetB2bCrmById(crmid);
                return crm;
            }
        }

        public List<B2b_crm> GetB2bCrmWeixinByComid(int comid, string country = "", string province = "", string city = "", string sex = "", int tagtype = 0, int tag = 0, string groupid = "全部")
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_crm> list = new InternalB2bCrm(helper).GetB2bCrmWeixinByComid(comid, country, province, city, sex, tagtype, tag, groupid);
                return list;
            }
        }

        public List<B2b_crm> GetB2bCrmWeixinByMenshi(int menshiid, string country = "", string province = "", string city = "", string sex = "", int tagtype = 0, int tag = 0)
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_crm> list = new InternalB2bCrm(helper).GetB2bCrmWeixinByMenshi(menshiid, country, province, city, sex, tagtype, tag);
                return list;
            }
        }



        public int CoverRegion(string openid, string Country, string Province, string City)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalB2bCrm(helper).CoverRegion(openid, Country, Province, City);
                return result;
            }
        }

        public List<string> Getqunfaphone(int comid, out  int total, string qunfanum = "-1")
        {
            using (var helper = new SqlHelper())
            {
                List<string> result = new InternalB2bCrm(helper).Getqunfaphone(comid, out total, qunfanum);
                return result;
            }
        }

        public bool IsCompanyCrm(string openid, int comid)
        {
            using (var helper = new SqlHelper())
            {
                bool result = new InternalB2bCrm(helper).IsCompanyCrm(openid, comid);
                return result;
            }
        }


        public string GetMemberChannelcompanyByB2bCrmId(int crmid)
        {
            using (var helper = new SqlHelper())
            {
                var crm = new InternalB2bCrm(helper).GetMemberChannelcompanyByB2bCrmId(crmid);
                return crm;
            }
        }

        public int GetMemberChannelcompanyIdByB2bCrmId(int crmid)
        {
            using (var helper = new SqlHelper())
            {
                var crm = new InternalB2bCrm(helper).GetMemberChannelcompanyIdByB2bCrmId(crmid);
                return crm;
            }
        }

        //门市坐标
        public string GetB2bchannelDistanceByid(int channelcomid)
        {
            using (var helper = new SqlHelper())
            {
                var crm = new InternalB2bCrm(helper).GetB2bchannelDistanceByid(channelcomid);
                return crm;
            }
        }

        //用户坐标坐标
        public string GetB2bCrmDistanceByid(string openid)
        {
            using (var helper = new SqlHelper())
            {
                var crm = new InternalB2bCrm(helper).GetB2bCrmDistanceByid(openid);
                return crm;
            }
        }


        //项目坐标
        public string GetProjDistanceByid(int projid)
        {
            using (var helper = new SqlHelper())
            {
                var crm = new InternalB2bCrm(helper).GetProjDistanceByid(projid);
                return crm;
            }
        }

        //门市坐标
        public string GetChannelDistanceByid(int channelid)
        {
            using (var helper = new SqlHelper())
            {
                var crm = new InternalB2bCrm(helper).GetChannelDistanceByid(channelid);
                return crm;
            }
        }


        //计算渠道坐标
        public double CalculateTheCoordinates(string openid, int channelcomid)
        {
            var channeldistance = GetB2bchannelDistanceByid(channelcomid);
            var crmdistance = GetB2bCrmDistanceByid(openid);
            var n1 = "";
            var e1 = "";
            var n2 = "";
            var e2 = "";

            if (channeldistance != "" && crmdistance != "")
            {
                var locatearr = channeldistance.Split(',');
                if (locatearr.Length >= 2)
                {
                    n1 = locatearr[1];
                    e1 = locatearr[0];
                }

                var crmlocatearr = crmdistance.Split(',');
                if (crmlocatearr.Length >= 2)
                {
                    n2 = crmlocatearr[1];
                    e2 = crmlocatearr[0];
                }

                if (n1 == "" || n2 == "" || e1 == "" || e2 == "")
                {
                    return 99999999;
                }

                using (var helper = new SqlHelper())
                {
                    var crm = new InternalB2bCrm(helper).CalculateTheCoordinates(n1, e1, n2, e2);
                    return crm;
                }

            }

            return 99999999;
        }


        //计算项目坐标
        public double ProjCoordinates(string openid, int projid)
        {
            var projdistance = GetProjDistanceByid(projid);
            var crmdistance = GetB2bCrmDistanceByid(openid);
            var n1 = "";
            var e1 = "";
            var n2 = "";
            var e2 = "";

            if (projdistance != "" && crmdistance != "")
            {
                var locatearr = projdistance.Split(',');
                if (locatearr.Length >= 2)
                {
                    n1 = locatearr[1];
                    e1 = locatearr[0];
                }

                var crmlocatearr = crmdistance.Split(',');
                if (crmlocatearr.Length >= 2)
                {
                    n2 = crmlocatearr[1];
                    e2 = crmlocatearr[0];
                }

                if (n1 == "" || n2 == "" || e1 == "" || e2 == "")
                {
                    return 99999999;
                }

                using (var helper = new SqlHelper())
                {
                    var crm = new InternalB2bCrm(helper).CalculateTheCoordinates(n1, e1, n2, e2);
                    return crm;
                }

            }

            return 99999999;
        }


        //计算渠道人/渠道人单位/渠道人公司  距离会员的距离
        public double PeopleCoordinates(string openid, int channelcompanyid, int comid = 0, string usern = "", string usere = "")
        {
            var projdistance = "";


            //根据会员微信获取顾问坐标；没有计算渠道单位/渠道公司的坐标
            B2b_crm_location crmlocation = new B2bCrmData().GetGuwenLocationByVipweixin(openid,comid);
            if (crmlocation != null)
            {
                //纬度+经度
                projdistance = crmlocation.Longitude + "," + crmlocation.Latitude;
            }
            else
            {

                if (channelcompanyid != 0)
                {
                    projdistance = GetChannelDistanceByid(channelcompanyid);
                }
                else
                {
                    if (comid != 0)
                    {
                        B2bCompanyData comdata = new B2bCompanyData();
                        var datainfo = B2bCompanyData.GetAllComMsg(comid);
                        if (datainfo != null)
                        {
                            projdistance = datainfo.B2bcompanyinfo.Coordinate;
                        }
                    }
                }
            }

            if (projdistance == "") {
                return 0;
            }



            var crmdistance = GetB2bCrmDistanceByid(openid);
            var n1 = "";
            var e1 = "";
            var n2 = "";
            var e2 = "";

            if (projdistance != "")
            {
                var locatearr = projdistance.Split(',');
                if (locatearr.Length >= 2)
                {
                    n1 = locatearr[1];
                    e1 = locatearr[0];
                }

                if (usern != "" && usere != "" && usern != "0" && usere != "0")
                {
                    n2 = usern;
                    e2 = usere;
                }
                else
                {
                    var crmlocatearr = crmdistance.Split(',');
                    if (crmlocatearr.Length >= 2)
                    {
                        n2 = crmlocatearr[1];
                        e2 = crmlocatearr[0];
                    }
                }


                if (n1 == "" || n2 == "" || e1 == "" || e2 == "")
                {
                    return 0;
                }

                using (var helper = new SqlHelper())
                {
                    var crm = new InternalB2bCrm(helper).CalculateTheCoordinates(n1, e1, n2, e2);
                    return crm;
                }

            }

            return 0;
        }

        private B2b_crm_location GetGuwenLocationByVipweixin(string openid, int comid)
        {
            using (var helper = new SqlHelper())
            {
                B2b_crm_location r = new InternalB2bCrm(helper).GetGuwenLocationByVipweixin(openid,comid);
                return r;
            }
        }


        public string GetWeiXinByCrmid(int crmid)
        {
            using (var helper = new SqlHelper())
            {
                string r = new InternalB2bCrm(helper).Getweixinbycrmid(crmid);
                return r;
            }
        }
        /// <summary>
        /// 记录微信用户最后一次互动时间
        /// </summary>
        /// <param name="comid"></param>
        /// <param name="weixin"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public int RecordInteracttime(int comid, string weixin, DateTime dateTime)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bCrm(helper).RecordInteracttime(comid, weixin, dateTime);
                return r;
            }
        }
        /// <summary>
        /// 根据微信号 修改用户的微信关注状态和 激活状态
        /// </summary>
        /// <param name="comid"></param>
        /// <param name="weixin"></param>
        /// <param name="whetherwxfocus"></param>
        /// <param name="whetheractivate"></param>
        /// <returns></returns>
        public int UpWxsubstate(int comid, string weixin, int whetherwxfocus, int whetheractivate)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bCrm(helper).UpWxsubstate(comid, weixin, whetherwxfocus, whetheractivate);
                return r;
            }
        }
        /// <summary>
        /// 调整用户等积分;记录等积分调整日志；同时根据等积分变动调整会员级别（会员级别只增不降）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dengjifen"></param>
        /// <returns></returns>
        public int Adjust_dengjifen(B2bcrm_dengjifenlog djflog, int id, int comid, decimal dengjifen)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bCrm(helper).Adjust_dengjifen(djflog, id, comid, dengjifen);
                return r;
            }
        }




        //计算渠道坐标
        public int WorkDay(string Workdays)
        {
            int week = 0;
            var dt = DateTime.Today.DayOfWeek.ToString();
            switch (dt)
            {
                case "Monday":
                    week = 2;
                    break;
                case "Tuesday":
                    week = 3;
                    break;
                case "Wednesday":
                    week = 4;
                    break;
                case "Thursday":
                    week = 5;
                    break;
                case "Friday":
                    week = 6;
                    break;
                case "Saturday":
                    week = 7;
                    break;
                case "Sunday":
                    week = 1;
                    break;
            }

            if (Workdays == null)
            {
                return 1;//未添加则都为工作日
            }
            else
            {
                string[] str = Workdays.Split(',');
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i].Contains(week.ToString()))
                    {
                        return 1;//在工作日内
                    }
                }
            }
            return 0;
        }




        /// <summary>
        ///根据用户微信号判断 其顾问是不是可以咨询
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="p"></param>
        /// <param name="comid"></param>
        /// <returns></returns>
        public int IsCanZixun(string openid, int comid)
        {
            B2b_company_manageuser manageruser = new B2bCompanyManagerUserData().GetGuwenByVipweixin(openid,comid);
            if (manageruser == null)
            {
                return 0;
            }
            else
            {
                string Workdays = manageruser.Workdays;
                //判断是否在工作日内，1为在，0为不在
                int WorkdaysView = new B2bCrmData().WorkDay(Workdays);

                //判断顾问是否绑定了微信 0未绑定；1已绑定
                int isbindweixin = 0;
                var crmmodel = new B2bCrmData().GetB2bCrmByPhone(manageruser.Com_id, manageruser.Tel);
                if (crmmodel != null)
                {
                    if (crmmodel.Weixin != "")
                    {
                        isbindweixin = 1;
                    }
                    else
                    {
                        isbindweixin = 0;
                    }
                }

                //顾问已经绑定微信并且在工作日内，则可以咨询
                if (WorkdaysView == 1 && isbindweixin == 1)
                {
                    return 1;
                }
                else 
                {
                    return 0;
                }

            }
        }

        public B2b_crm_location GetB2bcrmlocationByopenid(string openid)
        {  
            using(var helper=new SqlHelper())
            {
                B2b_crm_location r = new InternalB2bCrm(helper).GetB2bcrmlocationByopenid(openid);
                return r;
            }
        }

        /// <summary>
        /// 进入分销免登陆账户
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="freelandingaccount"></param>
        public int UpFreelandingAccount(string openid, string freelandingaccount)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bCrm(helper).UpFreelandingAccount(openid, freelandingaccount);
                return r;
            }
        }

        public int GetComidByOpenid(string openid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bCrm(helper).GetComidByOpenid(openid);
                return r;
            }
        }


        public string GetNameorImgByid(int id,int type)
        {
            using (var helper = new SqlHelper())
            {
                var r = new InternalB2bCrm(helper).GetNameorImgByid(id,type);
                return r;
            }
        }

       
        

    }
}
