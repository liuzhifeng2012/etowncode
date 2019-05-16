using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;
using System.Data;

namespace ETS2.PM.Service.PMService.Data
{
    public class B2bEticketData
    {
        #region 编辑电子票信息
        public int InsertOrUpdate(B2b_eticket eticket)
        {

            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticket(helper).InsertOrUpdate(eticket);


            }
        }
        #endregion

        public B2b_eticket GetEticketDetail(string pno, string posid = "")
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticket(helper).GetEticketDetail(pno, posid);
            }
        }

        public List<B2b_eticket> GetEticketListbyidcard(string idcard, string posid = "")
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticket(helper).GetEticketListbyidcard(idcard, posid);
            }
        }

        public string GetEticketOrderPno(int order_no,int agentid)
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticket(helper).GetEticketOrderPno(order_no, agentid);
            }
        }


        public B2b_eticket GetEticketByID(string id)
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticket(helper).GetEticketByID(id);
            }
        }


        public int BackAgentEticket(int id, string pno)
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticket(helper).BackAgentEticket(id, pno);
            }
        }


        public B2b_eticket GetPnoDetail(string pno)
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticket(helper).GetPnoDetail(pno);
            }
        }

        public B2b_eticket GetEticketSearch(string pno,int comid,int agentid)
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticket(helper).GetEticketSearch(pno, comid, agentid);
            }
        }
        public B2b_eticket GetPnoEticketinfo(string pno)
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticket(helper).GetPnoEticketinfo(pno);
            }
        }

        public List<B2b_eticket> GetBackEticketlist(int comid, int agentid, int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticket(helper).GetBackEticketlist(comid, agentid, pageindex, pagesize, out totalcount);
            }
        }

        public B2b_eticket GetEticketDetail(int orderid, string openid)
        {
             using(var helper=new SqlHelper())
             {
                 return new InternalB2bEticket(helper).GetEticketDetail(orderid,openid);
             }
        }

        public int GetEticketVCount(int comid, int agentid,int proid,string startime="",string endtime="")
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticket(helper).GetEticketVCount(comid, agentid, proid,startime,endtime);
            }
        }

        public int GetEticketOrderVCount(int comid, int agentid, int orderid)
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticket(helper).GetEticketOrderVCount(comid, agentid, orderid);
            }
        }

        //未使用数量
        public int GetEticketOrderVoidCount(int comid, int agentid, int orderid)
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticket(helper).GetEticketOrderVoidCount(comid, agentid, orderid);
            }
        }
        

        //作废订单
        public int EticketOrderVoid(int comid, int agentid, int orderid)
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticket(helper).EticketOrderVoid(comid, agentid, orderid);
            }
        }


        public B2b_eticket SelectOrderid(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                var list= new InternalB2bEticket(helper).SelectOrderid(orderid);
                return list;
            }
        }
        public List<B2b_eticket> SelectOrderid_list(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                var list= new InternalB2bEticket(helper).SelectOrderid_list(orderid);
                return list;
            }
        }

        

        #region 根据订单查询电子票剩余可使用数量
        public int SelectEticketUnUsebyOrderid(int orderid)
        {

            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticket(helper).SelectEticketUnUsebyOrderid(orderid);


            }
        }
        #endregion

        #region 退票，电子票可使用数量清0
        public int Backticket_use_num(int orderid)
        {

            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticket(helper).Backticket_use_num(orderid);


            }
        }
        #endregion


        #region 对预约码回滚
        public int Backticket_yuyuepno_num(string pno,int num)
        {

            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticket(helper).Backticket_yuyuepno_num(pno, num);


            }
        }
        #endregion


        public DataSet CreateDaoma_Dataset(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                var num = new InternalB2bOrder(helper).CreateDaoma_Dataset(orderid);
                return num;
            }
        }

        public int GetEticketbyid(int eticketid)
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticket(helper).GetEticketbyid(eticketid);
            }
        }

        public string GetOutcompanybyid(int eticketid)
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticket(helper).GetOutcompanybyid(eticketid);
            }
        }



        public string GetOutcompanybyorderid(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticket(helper).GetOutcompanybyorderid(orderid);
            }
        }

        public List<B2b_eticket> PrintTicketbyOrderid(int orderid,int size)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2bEticket(helper).PrintTicketbyOrderid(orderid,size);
                return list;
            }
        }

        public List<B2b_eticket> AlreadyPrintbyOrderid(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2bEticket(helper).AlreadyPrintbyOrderid(orderid);
                return list;
            }
        }

        public int AlreadyPrintNumbyOrderid(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2bEticket(helper).AlreadyPrintNumbyOrderid(orderid);
                return list;
            }
        }

        public int PrintStateUp(int eticketid)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2bEticket(helper).PrintStateUp(eticketid);
                return list;
            }
        }

        public int PrintStateUpPagecode(int eticketid,string Pagecode)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2bEticket(helper).PrintStateUpPagecode(eticketid, Pagecode);
                return list;
            }
        }


        public B2b_eticket GetEticketByOrderid(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                B2b_eticket r = new InternalB2bEticket(helper).GetEticketByOrderid(orderid);
                return r;
            }
        }

        //public B2b_eticket GetEticketByTaobaoOrderid(string taobaoorderid)
        //{
        //    using (var helper = new SqlHelper())
        //    {
        //        B2b_eticket r = new InternalB2bEticket(helper).GetEticketByTaobaoOrderid(taobaoorderid);
        //        return r;
        //    }
        //}

        public int UpPnoNumZero(int orderid,int backnum,int libnum)
        {
            using(var helper=new SqlHelper())
            {
                int r = new InternalB2bEticket(helper).UpPnoNumZero(orderid,backnum,libnum);
                return r;
            }
        }
        /// <summary>
        /// 根据订单号得到电子票信息
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public List<B2b_eticket> GetEticketListByOrderid(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_eticket> r = new InternalB2bEticket(helper).GetEticketListByOrderid(orderid);
                return r;
            }
        }

        #region 编辑增加电子票 绑定使用人
        public int bindingpnoUpdatepeople(B2b_eticket eticket)
        {

            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticket(helper).bindingpnoUpdatepeople(eticket);


            }
        }
        #endregion



        #region 编辑电子票服务
        public int InsertOrUpdateB2b_eticket_Deposit(b2b_eticket_Deposit eticketDeposit)
        {

            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticket(helper).InsertOrUpdateB2b_eticket_Deposit(eticketDeposit);


            }
        }
        #endregion

        /// <summary>
        /// 根据电子票查询绑定的押金
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public List<b2b_eticket_Deposit> GetB2b_eticket_DepositBypno(int eid)
        {
            using (var helper = new SqlHelper())
            {
                List<b2b_eticket_Deposit> r = new InternalB2bEticket(helper).GetB2b_eticket_DepositBypno(eid);
                return r;
            }
        }

        /// <summary>
        /// 根据id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public b2b_eticket_Deposit GetB2b_eticket_DepositByid(int id)
        {
            using (var helper = new SqlHelper())
            {
                b2b_eticket_Deposit r = new InternalB2bEticket(helper).GetB2b_eticket_DepositByid(id);
                return r;
            }
        }

        public int UpbacketicketDepositstate(int id)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bEticket(helper).UpbacketicketDepositstate(id);
                return r;
            }
        }
        public int Upeticketsencardstate(int eticketid,int state)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bEticket(helper).Upeticketsencardstate(eticketid,state);
                return r;
            }
        }

        /// <summary>
        /// 清空电子票数量
        /// </summary>
        /// <param name="pno"></param>
        /// <returns></returns>
        public int ClearPnoNum(string pno)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bEticket(helper).ClearPnoNum(pno);
                return r;
            }
        }
    }
}
