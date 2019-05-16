using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data
{
    public class DayJieSuanData
    {
        /// <summary>
        /// 后台验证  验票列表
        /// </summary>
        /// <param name="comid"></param>
        /// <returns></returns>
        public DataSet DayJSList(string comid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalDayJieSuan(helper).DayJSList(comid);

                return list;
            }
        }


        //#region 获得当天的结算信息对象
        //internal static B2b_dayjiesuan GetDateDayJS(string comid)
        //{
        //    using (var helper = new SqlHelper())
        //    {
        //        var dayjs = new InternalDayJieSuan(helper).GetDateDayJS(comid);
        //        return dayjs;
        //    }
        //}
        //#endregion

        #region 编辑当日结算信息
        internal int InsertOrUpdate(B2b_dayjiesuan newdayjs)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    int result = new InternalDayJieSuan(sql).InsertOrUpdate(newdayjs);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        internal B2b_dayjiesuan GetDayJSByID(int JSid) 
        {
            using (var helper = new SqlHelper())
            {
                var dayjs = new InternalDayJieSuan(helper).GetDayJSByID(JSid);
                return dayjs;
            }
        }
        /// <summary>
        /// 后台验证 报表统计
        /// </summary>
        /// <param name="comid"></param>
        /// <returns></returns>
        public DataSet DayJSResult(string comid,string jsid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalDayJieSuan(helper).DayJSResult(comid,jsid);

                return list;
            }
        }
        /// <summary>
        /// pos验证 验票报表
        /// </summary>
        /// <param name="pos_id"></param>
        /// <returns></returns>
        public DataSet DayJSListByPosId(string pos_id)
        {
            using (var helper = new SqlHelper())
            {
                var dayjs = new InternalDayJieSuan(helper).DayJSListByPosId(pos_id);
                return dayjs;
            }
        }

        //internal static B2b_dayjiesuan GetDateDayJSByPosId(string pos_id)
        //{
        //    using (var helper = new SqlHelper())
        //    {
        //        var dayjs = new InternalDayJieSuan(helper).GetDateDayJSByPosId(pos_id);
        //        return dayjs;
        //    }
        //}
        /// <summary>
        /// 判断验码日志表中是否含有pos未验证的记录
        /// </summary>
        /// <param name="pos_id"></param>
        /// <returns></returns>
        internal static int GetVerifyLogCount(string pos_id)
        {
            using (var helper = new SqlHelper())
            {
                var count = new InternalDayJieSuan(helper).GetVerifyLogCount(pos_id);
                return count;
            }
        }
        /// <summary>
        ///   判断验码日志表中是否含有公司未验证的记录
        /// </summary>
        /// <param name="pos_id"></param>
        /// <returns></returns>
        internal static int GetVerifyLogCountByComId(string comid)
        {
            using (var helper = new SqlHelper())
            {
                var count = new InternalDayJieSuan(helper).GetVerifyLogCountByComId(comid);
                return count;
            }
        }
        ///// <summary>
        ///// 根据id得到结算信息
        ///// </summary>
        ///// <param name="jsid"></param>
        ///// <returns></returns>
        //internal static B2b_dayjiesuan GetDayJsById(string jsid)
        //{
        //    using (var helper = new SqlHelper())
        //    {
        //        var dayjs = new InternalDayJieSuan(helper).GetDayJsById(jsid);
        //        return dayjs;
        //    }
        //}

      
    }

}
