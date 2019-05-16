using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class B2bEticketLogData
    {
        /// <summary>
        /// 编辑电子码验证日志信息
        /// </summary>
        /// <param name="elog"></param>
        public int InsertOrUpdateLog(B2b_eticket_log elog)
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticketLog(helper).InsertOrUpdate(elog);
            }
        }
        public List<B2b_eticket_log> EPageList(string comid, int pageindex, int pagesize, int eclass, int proid, int jsid, out int totalcount, int posid=0, string key = "", string startime = "", string endtime = "",int agentid=0,int projectid=0,int saleagentid=0)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bEticketLog(helper).EPageList(comid, pageindex, pagesize, eclass, proid, jsid, out totalcount, posid, key, startime, endtime, agentid, projectid, saleagentid);

                return list;
            }
        }
        #region 修改未结算的电子票的结算id
        internal int ModifyJsid(int JSid, int comid)
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticketLog(helper).ModifyJsid(JSid, comid);


            }
        }
        #endregion

        #region 得到未开始结算的第一条电子票信息
        internal B2b_eticket_log GetFrontNotJS()
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticketLog(helper).GetFrontNotJS();
            }
        }
        #endregion

        public B2b_eticket_log GetFinalPrintEticketLogByPosid(string pos_id)
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticketLog(helper).GetFinalPrintEticketLogByPosid(pos_id);
            }
        }

        internal int ModifyJsidByPosId(int JSid, string pos_id)
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticketLog(helper).ModifyJsidByPosId(JSid, pos_id);


            }
        }

        public B2b_eticket_log GetTicketLogById(string validateticketlogid)
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticketLog(helper).GetTicketLogById(validateticketlogid);
            }
        }
        #region 根据电子票号得到电子票验码日志列表
        public List<B2b_eticket_log> GetPnoConsumeLogList(string pno)
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticketLog(helper).GetPnoConsumeLogList(pno);
            }
        }
        #endregion


        //#region 根据随机码id得到验票日志
        //public List<B2b_eticket_log> GetElogByRandomid(string randomid)
        //{
        //    using (var helper = new SqlHelper())
        //    {
        //        return new InternalB2bEticketLog(helper).GetElogByRandomid(randomid);
        //    }
        //}
        //#endregion


        #region 分销验证日志
        public List<B2b_eticket_log> AgentEticketlog(int comid, int agentid, int pageindex, int pagesize, out int totalcount, string key = "", string startime = "", string endtime = "")
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticketLog(helper).AgentEticketlog(comid, agentid, pageindex, pagesize, out totalcount,key,startime,endtime);
            }
        }
        #endregion


        #region 按订单查询分销验证日志
        public List<B2b_eticket_log> VAgentEticketlog(int comid, int agentid,int orderid, int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticketLog(helper).VAgentEticketlog(comid, agentid,orderid, pageindex, pagesize, out totalcount);
            }
        }
        #endregion

        public bool GetRandomWhetherSame( int action, string randomid)
        {

            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticketLog(helper).GetRandomWhetherSame(action,randomid);
            }
        }

        public B2b_eticket_log GetElogByRandomid(string randomid, int action)
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticketLog(helper).GetElogByRandomid(randomid,action);
            }
        }
        /// <summary>
        /// 根据电子票号得到验证数量
        /// </summary>
        /// <param name="pno"></param>
        /// <returns></returns>
        public int GetVerifyNum(string pno)
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticketLog(helper).GetVerifyNum(pno);

            }
        }
        /// <summary>
        /// 根据订单号得到电子票验证成功日志
        /// </summary>
        /// <param name="bindorderid"></param>
        /// <returns></returns>
        public List<B2b_eticket_log> GeteticketloglistByorderid(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticketLog(helper).GeteticketloglistByorderid( orderid);

            }
        }
        /// <summary>
        /// 判断是否有冲正成功的记录 
        /// </summary>
        /// <param name="randomid"></param>
        /// <param name="action"></param>
        /// <param name="a_state"></param>
        /// <returns></returns>
        public bool Ishasreversesuclog(string randomid )
        {
             using(var helper=new SqlHelper())
             {
                 bool b = new InternalB2bEticketLog(helper).Ishasreversesuclog(randomid);
                 return b;
             }
        }

        public int GetYanzhenglogCountByPno(string pno)
        {
            using (var helper = new SqlHelper())
            {
                int b = new InternalB2bEticketLog(helper).GetYanzhenglogCountByPno(pno);
                return b;
            }
        }

        public B2b_eticket_log GetLasterYueyupnoElog(string pno, int usenum)
        {
            using (var helper = new SqlHelper())
            {
                B2b_eticket_log b = new InternalB2bEticketLog(helper).GetLasterYueyupnoElog(pno,usenum);
                return b;
            }
        }

        public B2b_eticket_log GetlastyanzhengsuclogByPno(string pno)
        {
            using (var helper = new SqlHelper())
            {
                B2b_eticket_log b = new InternalB2bEticketLog(helper).GetlastyanzhengsuclogByPno(pno);
                return b;
            }
        }
    }
}
