using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.Member.Service.MemberService.Model;
using ETS.Data.SqlHelper;
using ETS2.Member.Service.MemberService.Data.InternalData;
using System.Data;

namespace ETS2.Member.Service.MemberService.Data
{
    public class MemberERNIEData
    {

        #region 促销活动列表
        public List<Member_ERNIE> ERNIEActPageList( int comid, int pageindex, int pagesize, out int totalcount,string runstate="0,1")
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalMemberERNIED(helper).ERNIEActPageList(comid, pageindex, pagesize, out totalcount,runstate);
                return list;
            }

        }
        #endregion

        #region 根据id得到活动信息
        public static Member_ERNIE ERNIEGetActById(int actid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberERNIED(sql);
                    Member_ERNIE result = internalData.ERNIEGetActById(actid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion



        #region 得到最新摇奖活动
        public static int ERNIETOPgetid(int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberERNIED(sql);
                    var result = internalData.ERNIETOPgetid(comid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion

        #region 编辑摇奖活动信息
        public static int ERNIEEditActInfo(Member_ERNIE actinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberERNIED(sql);
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



        #region 促销活动奖品列表
        public List<Member_ERNIE_Award> ERNIEAwardPageList(int actid, int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalMemberERNIED(helper).ERNIEAwardPageList(actid, pageindex, pagesize, out totalcount);
                return list;
            }
        }
        #endregion

        #region 根据id得到活动奖品信息
        public static Member_ERNIE_Award ERNIEGetAwardById(int actid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberERNIED(sql);
                    Member_ERNIE_Award result = internalData.ERNIEGetAwardById(actid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion


        #region 根据几等奖得到活动奖品信息
        public static Member_ERNIE_Award ERNIEAwardget(int actid, int topclass)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberERNIED(sql);
                    Member_ERNIE_Award result = internalData.ERNIEAwardget(actid,topclass);

                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion

        #region 编辑摇奖奖品活动信息
        public static int ERNIEEditAwardInfo(Member_ERNIE_Award actinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberERNIED(sql);
                    int result = internalData.AwardInsertOrUpdate(actinfo);

                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion

        #region 删除已有奖项
        public static int ERNIEDelAwardInfo(int actid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberERNIED(sql);
                    int result = internalData.ERNIEDelAwardInfo(actid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion


        #region 抽奖插入
        public static int InsertChoujiang(ERNIE_Record Recordinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberERNIED(sql);
                    int result = internalData.InsertChoujiang(Recordinfo);

                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion


        #region 抽奖查询 按用户信息及活动限制信息
        public static int SearchChoujiang(ERNIE_Record Recordinfo, int ERNIE_Limit)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberERNIED(sql);
                    int result = internalData.SearchChoujiang(Recordinfo, ERNIE_Limit);

                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion

        #region 抽奖活动确认上线
        public static int ERNIEeditActOnline(int actid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberERNIED(sql);
                    int result = internalData.ERNIEeditActOnline(actid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion

        #region 抽奖活动确认上线
        public static int ERNIERecordedit(int actid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberERNIED(sql);
                    int result = internalData.ERNIERecordedit(actid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion

        #region 插入抽奖奖品随机码
        public static int InsertAward(ERNIE_Awardcode Awardinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberERNIED(sql);
                    int result = internalData.InsertAward(Awardinfo);

                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion

        #region 插入抽奖奖品随机码
        public static int ChoujiangSearchAwardcode(int Recordid,int actid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberERNIED(sql);
                    int result = internalData.ChoujiangSearchAwardcode(Recordid, actid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion

        #region 处理抽奖奖品随机码 抽奖ID,奖品随机号ID，用户ID
        public static int ZhongjiangAwardcode(int Recordid, int Awardcodeid,int uid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberERNIED(sql);
                    int result = internalData.ZhongjiangAwardcode(Recordid, Awardcodeid, uid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion

        #region 中奖修改
        public static int ERNIEZhongjiang(ERNIE_Record Recordinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberERNIED(sql);
                    int result = internalData.ERNIEZhongjiang(Recordinfo);

                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion

        #region 获得中奖信息
        public ERNIE_Record ERNIERecordInfo(int Recordid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberERNIED(sql);
                    var result = internalData.ERNIERecordInfo(Recordid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion

        #region 中奖修改
        public static int ERNIEZhongjiangChuli(int id)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberERNIED(sql);
                    int result = internalData.ERNIEZhongjiangChuli(id);

                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion

        #region 奖品列表
        public List<ERNIE_Record> ERNIERecordpagelist(string comid, int pageindex, int pagesize, int actid, int etype, string key, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalMemberERNIED(helper).ERNIERecordpagelist(comid, pageindex, pagesize, actid, etype, key, out totalcount);
                return list;
            }

        }
        #endregion


        #region 获奖名单
        public List<ERNIE_Record> ERNIEHuojiangmingdan(int rid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberERNIED(sql);
                    var result = internalData.ERNIEHuojiangmingdan(rid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion

        #region 根据几等奖得到活动奖品信息
        public static int ERNIEAwardgetID(int EAwardid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberERNIED(sql);
                    int result = internalData.ERNIEAwardgetID(EAwardid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion
    }
}
