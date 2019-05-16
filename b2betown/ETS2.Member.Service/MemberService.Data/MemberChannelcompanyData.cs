using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.Member.Service.MemberService.Data.InternalData;
using ETS.Data.SqlHelper;
using ETS2.Member.Service.MemberService.Model;

namespace ETS2.Member.Service.MemberService.Data
{
    public class MemberChannelcompanyData
    {
        public List<Member_Channel_company> GetUnitList(int unittype = 2)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalMemberChannelcompany(helper).GetUnitList(unittype);

                return list;
            }
        }
        public List<Member_Channel_company> GetUnitList(int comid, string unittype)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalMemberChannelcompany(helper).GetUnitList(comid, unittype);

                return list;
            }
        }
        public List<Member_Channel_company> GetUnitList(int comid, int unittype = 2,int channelcompanyid=0)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalMemberChannelcompany(helper).GetUnitList(comid, unittype, channelcompanyid);

                return list;
            }
        }
        public List<Member_Channel_company> GetUnitListselected(int actid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalMemberChannelcompany(helper).GetUnitListselected(actid);

                return list;
            }
        }

        public int GetchannelUnitListselected(int actid,int channelcomid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalMemberChannelcompany(helper).GetchannelUnitListselected(actid, channelcomid);

                return list;
            }
        }

        public Member_Channel_company GetCompanyById(int companyid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalMemberChannelcompany(helper).GetCompanyById(companyid);

                return list;
            }
        }

        public string GetCompanyNameById(int companyid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalMemberChannelcompany(helper).GetCompanyNameById(companyid);

                return list;
            }
        }
        //微信和网站注册渠道
        public string UpCompanyById(string cardchl)
        {
            //var list = "";

            //int channelid = int.Parse(MemberCardData.GetCardNumber(decimal.Parse(cardchl)).IssueCard.ToString());
            var companyid = MemberChannelData.Upstring(cardchl);
            using (var helper = new SqlHelper())
            {
                if (companyid != null)
                {
                    var pro = new InternalMemberChannelcompany(helper).GetCompanyById(companyid.Companyid);
                    if (companyid.Issuetype == 3)
                    {
                        return "网站";
                    }
                    else if (companyid.Issuetype == 4)
                    {
                        return "微信";
                    }
                    else
                    {
                        if (pro != null)
                        {
                            return pro.Companyname;
                        }
                        else
                        {
                            return "--";
                        }
                    }
                }
                else
                {
                    return "--";
                }
            }
        }
        public int EditChannelCompany(Member_Channel_company company)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalMemberChannelcompany(helper).EditChannelCompany(company);
                return id;
            }
        }

        public int Upchannelcompanproject(int channelcompanyid, string companyproject)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalMemberChannelcompany(helper).Upchannelcompanproject(channelcompanyid, companyproject);
                return id;
            }
        }


        public List<Member_Channel_company> GetPromoteChannelCompany(int comid)
        {
            using (var helper = new SqlHelper())
            {
                List<Member_Channel_company> List = new InternalMemberChannelcompany(helper).GetPromoteChannelCompany(comid);
                return List;
            }
        }

        public List<Member_Channel_company> GetChannelCompanyList(int comid, string channeltype, int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                List<Member_Channel_company> List = new InternalMemberChannelcompany(helper).GetChannelCompanyList(comid, channeltype, pageindex, pagesize, out totalcount);
                return List;
            }
        }

        public Member_Channel_company GetChannelCompany(string channelcompanyid)
        {
            using (var helper = new SqlHelper())
            {
                Member_Channel_company List = new InternalMemberChannelcompany(helper).GetChannelCompany(channelcompanyid);
                return List;
            }
        }
        /// <summary>
        /// 得到渠道单位列表
        /// </summary>
        /// <param name="comid"></param>
        /// <param name="channeltype"></param>
        /// <returns></returns>
        public List<Member_Channel_company> GetChannelList(int comid, int channeltype = 100)
        {
            using (var helper = new SqlHelper())
            {
                List<Member_Channel_company> List = new InternalMemberChannelcompany(helper).GetChannelList(comid, channeltype);
                return List;
            }
        }

        public Member_Channel_company GetMenShiByOpenId(string openid, int comid)
        {
            using (var helper = new SqlHelper())
            {
                Member_Channel_company List = new InternalMemberChannelcompany(helper).GetChannelCompany(openid, comid);
                return List;
            }
        }

        public Member_Channel_company GetMenShiByJumpId(string openid, int comid)
        {
            using (var helper = new SqlHelper())
            {
                Member_Channel_company List = new InternalMemberChannelcompany(helper).GetMenShiByJumpId(openid, comid);
                return List;
            }
        }

        public List<Member_Channel_company> Channelcompanypagelist(string comid, int pageindex, int pagesize, string key, out int totalcount, int channelcompanyid = 0, string channelcompanytype = "0,1,3,4")
        {
            using (var helper = new SqlHelper())
            {
                List<Member_Channel_company> List = new InternalMemberChannelcompany(helper).Channelcompanypagelist(comid, pageindex, pagesize, key, out totalcount, channelcompanyid, channelcompanytype);
                return List;
            }
        }

        public List<Member_Channel_company> ChannelcompanyOrderlocation(string comid, int pageindex, int pagesize, string key, out int totalcount, int channelcompanyid = 0, string channelcompanytype = "0,1,3,4",string n1="",string e1="")
        {
            using (var helper = new SqlHelper())
            {
                List<Member_Channel_company> List = new InternalMemberChannelcompany(helper).ChannelcompanyOrderlocation(comid, pageindex, pagesize, key, out totalcount, channelcompanyid, channelcompanytype, n1, e1);
                return List;
            }
        }

        public int Adjustchannelcompanystatus(int companyid, int status)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalMemberChannelcompany(helper).Adjustchannelcompanystatus(companyid, status);
                return result;
            }
        }


        public int jianchaguwenbyweixin(int comid, string weixin)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalMemberChannelcompany(helper).jianchaguwenbyweixin(comid, weixin);
                return result;
            }
        }
        public int getchannelidbyweixin(int comid, string weixin,int uid=0)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalMemberChannelcompany(helper).getchannelidbyweixin(comid, weixin, uid);
                return result;
            }
        }

        public List<Member_Channel_company> GetChannelCompanyList(int comid, string issuetype, string companystate, string whetherdepartment, int channelcompanyid=0)
        {
            using (var helper = new SqlHelper())
            {
                List<Member_Channel_company> List = new InternalMemberChannelcompany(helper).GetChannelCompanyList(comid, issuetype, companystate, whetherdepartment,channelcompanyid);
                return List;
            }
        }



        public Member_Channel_company GetChannelCompanyByUserId(int userid)
        {
            using (var helper = new SqlHelper())
            {
                Member_Channel_company model = new InternalMemberChannelcompany(helper).GetChannelCompanyByUserId(userid);
                return model;
            }
        }

        public Member_Channel_company GetChannelCompanyByCrmId(int crmid)
        {
            using (var helper = new SqlHelper())
            {
                Member_Channel_company model = new InternalMemberChannelcompany(helper).GetChannelCompanyByCrmId(crmid);
                return model;
            }
        }

        public Member_Channel_company GetChannelCompanyByWxsourceId(int wxsourceid)
        {
            using (var helper = new SqlHelper())
            {
                Member_Channel_company model = new InternalMemberChannelcompany(helper).GetChannelCompanyByWxsourceId(wxsourceid);
                return model;
            }
        }

        public List<Member_Channel_company> GetMenshiByComid(int comid)
        {
            using(var helper=new SqlHelper())
            {
                List<Member_Channel_company> list = new InternalMemberChannelcompany(helper).GetMenshisByComid(comid);
                return list;
            }
        }




        public  string GetChannelCompanyNameById(int? Channelcompanyid)
        {
            if (Channelcompanyid == 0)
            {
                return "";
            }
            else
            {
                object o = ExcelSqlHelper.ExecuteScalar("select companyname from Member_Channel_company where id=" + Channelcompanyid);
                return o == null ? "" : o.ToString();
            }
        }

        public string GetChannelCompanyName(int crmid)
        {
            if (crmid == 0)
            {
                return "";
            }
            else 
            {
                Member_Channel_company m = GetChannelCompanyByCrmId(crmid);
                if (m == null)
                {
                    return "";
                }
                else 
                {
                    return m.Companyname;
                }
            }
        }

        public Member_Channel_company GetMenshiByPhone(string phone,int comid)
        {
            using (var helper = new SqlHelper())
            {
                Member_Channel_company list = new InternalMemberChannelcompany(helper).GetMenshiByPhone(phone,comid);
                return list;
            }
        }
        /// <summary>
        /// 根据会员微信得到顾问所在渠道单位
        /// </summary>
        /// <returns></returns>
        public Member_Channel_company GetGuWenChannelCompanyByCrmWeixin(string weixin,int comid)
        {
            using (var helper = new SqlHelper())
            {
                Member_Channel_company list = new InternalMemberChannelcompany(helper).GetGuWenChannelCompanyByCrmWeixin(weixin,comid);
                return list;
            }
        }

        public Member_Channel_company GetMemberChanelCompanyByUserid(int userid)
        {
            using (var helper = new SqlHelper())
            {
                Member_Channel_company r = new InternalMemberChannelcompany(helper).GetMemberChanelCompanyByUserid(userid);
                return r;
            }
        }

        public List<Member_Channel_company> Getchannelcompanylist(int comid, string Issuetype, string companystate, string key)
        {
            using(var helper=new SqlHelper())
            {
                List<Member_Channel_company> r = new InternalMemberChannelcompany(helper).Getchannelcompanylist(comid,Issuetype,companystate,key);

                return r;
            }
        }
    }
}
