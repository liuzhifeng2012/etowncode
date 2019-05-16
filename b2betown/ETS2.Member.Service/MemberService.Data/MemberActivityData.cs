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
    public class MemberActivityData
    {
        #region 根据id得到活动信息
        public static Member_Activity GetActById(int actid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberActivity(sql);
                    Member_Activity result = internalData.GetActById(actid);

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
        public static int EditActInfo(Member_Activity actinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberActivity(sql);
                    int result = internalData.InsertOrUpdate(actinfo);

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
        public List<Member_Activity> ActPageList(string comid, int pageindex, int pagesize, out int totalcount,string state="0,1")
        {
             using (var helper = new SqlHelper())
            {
                    var list = new InternalMemberActivity(helper).ActPageList(comid, pageindex, pagesize, out totalcount,state);
                    return list;
            }

        }
        #endregion



        #region 促销活动列表
        public List<Member_Activity> AccountActPageList(int accountid, int comid, int channelcompanyid, int pageindex, int pagesize, out int totalcount,int channelcomid=0)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalMemberActivity(helper).AccountActPageList(accountid, comid, channelcompanyid, pageindex, pagesize, out totalcount, channelcomid);
                return list;
            }

        }
        #endregion

        #region 促销活动列表
        public List<Member_Activity> AccountUnActPageList(int accountid, int comid, int channelcompanyid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalMemberActivity(helper).AccountUnActPageList(accountid, comid, channelcompanyid, out totalcount);
                return list;
            }

        }
        #endregion

        #region 领取促销活动列表
        public int AccountClaimActPageList(int aid, int cardid, int comid)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalMemberActivity(helper).AccountClaimActPageList(aid, cardid, comid);
                return list;
            }

        }
        #endregion


        #region 促销活动详情
        public Member_Activity AccountActInfo(int aid, int accountid, int comid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {try
                {
                     Member_Activity result = new InternalMemberActivity(helper).AccountActInfo(aid, accountid, comid, out totalcount);
                     return result;

                 }
                catch{
                    throw;
                }
            }

        }
        #endregion

        #region 未领取促销活动详情
        public Member_Activity UnAccountActInfo(int aid, int accountid, int comid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                try
                {
                    Member_Activity result = new InternalMemberActivity(helper).UnAccountActInfo(aid, accountid, comid, out totalcount);
                    return result;

                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion


        #region 根据渠道ID和会员ID插入默认活动
        public static string WebWeixinActIns(int uid,int cid,int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberActivity(sql);
                    var result = internalData.WebWeixinActIns(uid, cid, comid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion



        public Member_Activity GetMemberActivityById(int id)
        { 
            using(var helper=new SqlHelper())
            {
                Member_Activity data = new InternalMemberActivity(helper).GetMemberActivityById(id);
                return data;
            }
        }

        public int HandleQrCodeCreateStatus(int activityid, string checkstatus)
        {
            using (var helper = new SqlHelper())
            {
                int data = new InternalMemberActivity(helper).HandleQrCodeCreateStatus(activityid, checkstatus);
                return data;
            }
        }

        public List<Member_Activity> GetPromotionActList(int comid)
        {
             using(var helper=new SqlHelper())
             {
                 List<Member_Activity> data = new InternalMemberActivity(helper).GetPromotionActList(comid);
                 return data;
             }
        }

        public List<Member_Activity> GetActivityList(int comid, string runstate, string whetherexpired)
        {
            using (var helper = new SqlHelper())
            {
                List<Member_Activity> data = new InternalMemberActivity(helper).GetActivityList(comid,runstate,whetherexpired);
                return data;
            }
        }

        public bool WhetherSameunit(int createuserid, int? operchannelcompanyid)
        {
            using (var helper = new SqlHelper())
            {
                bool data = new InternalMemberActivity(helper).WhetherSameunit(createuserid, operchannelcompanyid);
                return data;
            }
        }
    }
}
