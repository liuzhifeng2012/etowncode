using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;
using System.Data;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Data
{
    public class B2bOrderData
    {

        #region 编辑订单信息
        public int InsertOrUpdate(B2b_order order)
        {
            using (var helper = new SqlHelper())
            {

                var orderid = new InternalB2bOrder(helper).InsertOrUpdate(order);

                return orderid;
            }
        }
        #endregion


        #region 创建购物车订单
        public int CartInsertOrUpdate(B2b_order order)
        {
            using (var helper = new SqlHelper())
            {

                var orderid = new InternalB2bOrder(helper).CartInsertOrUpdate(order);

                return orderid;
            }
        }
        #endregion

        #region 获得订单分页列表
        public List<B2b_order> OrderPageList(string comid, int pageindex, int pagesize, string key, int order_state, int ordertype, out int totalcount, int userid = 0, int crmid = 0, int orderIsAccurateToPerson = 0, int channelcompanyid = 0, string begindate = "", string enddate = "", int servertype = 0, int datetype=0)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bOrder(helper).OrderPageList(comid, pageindex, pagesize, key, order_state, ordertype, out totalcount, userid, crmid, orderIsAccurateToPerson, channelcompanyid, begindate, enddate, servertype, datetype);

                return list;
            }
        }
        #endregion


        #region 根据教练ID查询
        public List<B2b_order> channelcoachOrderPageList(string comid, int channelcoachid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bOrder(helper).channelcoachOrderPageList(comid, channelcoachid);

                return list;
            }
        }
        #endregion


        #region 获得订单分页列表
        public List<B2b_order> OrderCartPageList(string comid, int cartid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bOrder(helper).OrderCartPageList(comid, cartid, out totalcount);

                return list;
            }
        }
        #endregion

        #region 获得订单分页列表
        public List<B2b_order> OrderCountList(string comid, string startime, string endtime, int searchtype, out int totalcount, int userid = 0)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bOrder(helper).OrderCountList(comid, startime, endtime, searchtype, out totalcount, userid);

                return list;
            }
        }
        #endregion

        #region 订单财务确认
        public List<B2b_order> Orderfinset(string comid, string startime, string endtime, int mangefinset,string key, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bOrder(helper).Orderfinset(comid, startime, endtime, mangefinset, key, out totalcount);

                return list;
            }
        }
        #endregion

        #region 订单财务确认
        public List<B2b_order> Orderfinset_pay_price(string comid, string startime, string endtime, int mangefinset, string key, string submanagename)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bOrder(helper).Orderfinset_pay_price(comid, startime, endtime, mangefinset, key, submanagename);

                return list;
            }
        }
        #endregion

        #region 订单财务确认
        public List<B2b_order> Orderfinset_pay_price_list(string comid, string startime, string endtime, int mangefinset, string key, string submanagename, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bOrder(helper).Orderfinset_pay_price_list(comid, startime, endtime, mangefinset, key, submanagename, out totalcount);

                return list;
            }
        }
        #endregion

         #region 订单财务确认
        public int orderfinset_quren(string comid, string startime, string endtime, int mangefinset, string key, string submanagename)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bOrder(helper).orderfinset_quren(comid, startime, endtime, mangefinset, key, submanagename);

                return list;
            }
        }
        #endregion
        


        #region 合作商获得订单分页列表
        public List<B2b_order> CoopOrderPageList(string comid, int pageindex, int pagesize, string ordercome, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bOrder(helper).CoopOrderPageList(comid, pageindex, pagesize, ordercome, out totalcount);

                return list;
            }
        }
        #endregion

        #region 合作商获得订单分页列表
        public List<B2b_order> CoopVerOrderPageList(string comid, int pageindex, int pagesize, string ordercome, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bOrder(helper).CoopVerOrderPageList(comid, pageindex, pagesize, ordercome, out totalcount);

                return list;
            }
        }
        #endregion


        #region 获得订单分页列表
        public List<B2b_order> AgentOrderPageList(string comid, int agentid, int pageindex, int pagesize, string key, int order_state, out int totalcount, string begindate = "", string enddate = "", int servertype = 0)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bOrder(helper).AgentOrderPageList(comid, agentid, pageindex, pagesize, key, order_state, out totalcount, begindate, enddate, servertype);

                return list;
            }
        }
        #endregion

        #region 获得分销验票统计
        public List<B2b_order> AgentOrderCount(string comid, int agentid, int pageindex, int pagesize, out int totalcount, string key = "")
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bOrder(helper).AgentOrderCount(comid, agentid, pageindex, pagesize, out totalcount, key);

                return list;
            }
        }
        #endregion

        #region 获得退票列表
        public List<B2b_order> TicketPageList(int pageindex, int pagesize, string key, int order_state, int endstate, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bOrder(helper).TicketPageList(pageindex, pagesize, key, order_state, endstate, out totalcount);

                return list;
            }
        }
        #endregion

        #region 根据id获得订单信息
        public B2b_order GetOrderById(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                var order = new InternalB2bOrder(helper).GetOrderById(orderid);
                return order;
            }
        }
        #endregion

        #region 根据bindingorderid返回元订单号
        public int GetIdOrderBybindingId(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                var order = new InternalB2bOrder(helper).GetIdOrderBybindingId(orderid);
                return order;
            }
        }
        #endregion

        #region 根据id获得订单信息
        public string GetPnoByOrderId(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                var order = new InternalB2bOrder(helper).GetPnoByOrderId(orderid);
                return order;
            }
        }
        #endregion


        #region 获取销售统计列表
        public List<B2b_com_pro> SaleCountPageList(string comid, int pageindex, int pagesize, string startdate, string enddate, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bOrder(helper).SaleCountPageList(comid, pageindex, pagesize, startdate, enddate, out totalcount);

                return list;
            }
        }
        #endregion

        public DataSet GetTotaldate(string comid, string startdate, string enddate)
        {
            using (var helper = new SqlHelper())
            {

                var totaldate = new InternalB2bOrder(helper).GetTotaldate(comid, startdate, enddate);

                return totaldate;
            }
        }

        //获取成功订购数笔数
        public B2b_com_pro CountOrder(int Id, string startdate, string enddate)
        {
            using (var helper = new SqlHelper())
            {

                var totaldate = new InternalB2bOrder(helper).CountOrder(Id, startdate, enddate);

                return totaldate;
            }
        }

        //获取成功订购数笔数
        public B2b_com_pro Daoma_CountOrder(int Id, string startdate, string enddate)
        {
            using (var helper = new SqlHelper())
            {

                var totaldate = new InternalB2bOrder(helper).Daoma_CountOrder(Id, startdate, enddate);

                return totaldate;
            }
        }

        public B2b_order GetOrderByEticketid(int eticketid)
        {
            using (var helper = new SqlHelper())
            {
                var order = new InternalB2bOrder(helper).GetOrderByEticketId(eticketid);
                return order;
            }
        }

        public List<B2b_order> ConsumerOrderPageList(string openid, int pageindex, int pagesize, int accountid, out int totalcount, int servertype = 0, int channelid = 0, string startime = "", string endtime = "")
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bOrder(helper).ConsumerOrderPageList(openid, pageindex, pagesize, accountid, out totalcount, servertype, channelid, startime, endtime);

                return list;
            }
        }

        public List<B2b_order> ClientOrderquerenPageList(string openid, int pageindex, int pagesize, int accountid, out int totalcount, int servertype = 0)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bOrder(helper).ClientOrderquerenPageList(openid, pageindex, pagesize, accountid, out totalcount, servertype);

                return list;
            }
        }

        public int CountOrderServertype12(string openid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bOrder(helper).CountOrderServertype12(openid);

                return list;
            }
        }

        public List<B2b_order> ComOrderPageList(int comid, int pageindex, int pagesize, int accountid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bOrder(helper).ComOrderPageList(comid, pageindex, pagesize, accountid, out totalcount);

                return list;
            }
        }
        public B2b_order GetProductByOrderId(int orderid, string openid)
        {
            using (var helper = new SqlHelper())
            {
                B2b_order order = new InternalB2bOrder(helper).GetProductByOrderId(orderid, openid);
                return order;
            }
        }

        public int GetOrderIdByWeixin(string openid, int comid, int proid)
        {
            using (var helper = new SqlHelper())
            {
                var order = new InternalB2bOrder(helper).GetOrderIdByWeixin(openid, comid, proid);
                return order;
            }
        }

        public int ConfirExpress(int comid, int id, string expresscom, string expresscode, int order_state, string expresstext)
        {
            using (var helper = new SqlHelper())
            {
                var order = new InternalB2bOrder(helper).ConfirExpress(comid, id, expresscom, expresscode, order_state, expresstext);
                return order;
            }
        }

        public int guoqi_biaozhu(string comid, int id)
        {
            using (var helper = new SqlHelper())
            {
                var order = new InternalB2bOrder(helper).guoqi_biaozhu(comid, id);
                return order;
            }
        }
        


        public int ModifyUidByWeiXin(string weixin, int uid, int comid)
        {
            using (var helper = new SqlHelper())
            {
                var num = new InternalB2bOrder(helper).ModifyUidByWeiXin(weixin, uid, comid);
                return num;
            }
        }



        //public int ModifyUidByPhone(decimal phone, int uid, int comid)
        //{
        //    using (var helper = new SqlHelper())
        //    {
        //        var num = new InternalB2bOrder(helper).ModifyUidByPhone(phone, uid, comid);
        //        return num;
        //    }
        //}


        public int CoopOrderCount(int comid, string ordercome, out int Allnum, out int Todaynum, out int Yesterdaynum, out int Transactionnum)
        {
            using (var helper = new SqlHelper())
            {

                var totaldate = new InternalB2bOrder(helper).CoopOrderCount(comid, ordercome, out Allnum, out  Todaynum, out  Yesterdaynum, out  Transactionnum);

                return totaldate;
            }
        }

        /// <summary>
        /// 根据服务商id和服务商订单号获得外部订单信息
        /// </summary>
        /// <param name="order_num"></param>
        /// <param name="serviceid"></param>
        /// <returns></returns>
        public B2b_order GetOrderByOutPro_OrderNum(string order_num, int serviceid)
        {
            using (var helper = new SqlHelper())
            {
                B2b_order order = new InternalB2bOrder(helper).GetOrderByOutPro_OrderNum(order_num, serviceid);
                return order;
            }
        }
        /*同一用户对于同一产品是否有未完成订单*/
        public int Ishasnotpayorder(int uid, int proid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).Ishasnotpayorder(uid, proid);
                return r;
            }
        }
        /// <summary>
        /// 获得旅游大巴订单详情
        /// </summary>
        /// <param name="traveldate"></param>
        /// <param name="proid"></param>
        /// <param name="order_state"></param>
        /// <returns></returns>
        public List<B2b_order> Travelbusorderdetail(string traveldate, int proid, int order_state)
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_order> list = new InternalB2bOrder(helper).Travelbusorderdetail(traveldate, proid, order_state);
                return list;
            }
        }

        /// <summary>
        /// 根据产品编号得到支付成功人数
        /// </summary>
        /// <param name="daydate"></param>
        /// <param name="servertype"></param>
        /// <param name="comid"></param>
        /// <param name="orderstate"></param>
        /// <returns></returns>
        public int GetPaySucNumByProid(int proid, int comid, int paystate, DateTime daydate, int agentid = 0, string orderstate = "")
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).GetPaySucNumByProid(proid, comid, paystate, daydate, agentid, orderstate);
                return r;
            }
        }

        /// <summary>
        /// 根据产品编号得到结团人数
        /// </summary>
        /// <param name="daydate"></param>
        /// <param name="servertype"></param>
        /// <param name="comid"></param>
        /// <param name="orderstate"></param>
        /// <returns></returns>
        public int GetCloseTeamNumByProid(int proid, int comid, int orderstate, DateTime daydate)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).GetCloseTeamNumByProid(proid, comid, orderstate, daydate);
                return r;
            }
        }

        /// <summary>
        /// 根据服务类型得到当天的支付成功人数
        /// </summary>
        /// <param name="daydate"></param>
        /// <param name="servertype"></param>
        /// <param name="comid"></param>
        /// <param name="orderstate"></param>
        /// <returns></returns>
        public int GetPaysucNumByServertype(DateTime daydate, int servertype, int comid, int paystate, int agentid = 0, string orderstate = "")
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).GetPaysucNumByServertype(daydate, servertype, comid, paystate, agentid, orderstate);
                return r;
            }
        }

        /// <summary>
        /// 根据服务类型得到当天的结团人数
        /// </summary>
        /// <param name="daydate"></param>
        /// <param name="servertype"></param>
        /// <param name="comid"></param>
        /// <param name="orderstate"></param>
        /// <returns></returns>
        public int GetCloseTeamNumByServertype(DateTime daydate, int servertype, int comid, int orderstate, int agentid = 0)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).GetCloseTeamNumByServertype(daydate, servertype, comid, orderstate, agentid);
                return r;
            }
        }


        public IList<B2b_order> travelbusorderdetailByiscloseteam(string gooutdate, int proid, int iscloseteam)
        {
            using (var helper = new SqlHelper())
            {
                IList<B2b_order> r = new InternalB2bOrder(helper).travelbusorderdetailByiscloseteam(gooutdate, proid, iscloseteam);
                return r;
            }
        }

        public IList<B2b_order> travelbusorderdetailBypaystate(string gooutdate, int proid, int paystate)
        {
            using (var helper = new SqlHelper())
            {
                IList<B2b_order> r = new InternalB2bOrder(helper).travelbusorderdetailBypaystate(gooutdate, proid, paystate);
                return r;
            }
        }

        public int Upsendstate(int orderid, int sendstate)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).Upsendstate(orderid, sendstate);
                return r;
            }
        }

        public int Uporderstatesendstate(int orderid, int sendstate,int orderstate)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).Uporderstatesendstate(orderid, sendstate,orderstate);
                return r;
            }
        }
        /// <summary>
        /// 录入订单乘车人信息表
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="travelname"></param>
        /// <param name="travelidcard"></param>
        /// <param name="travelnation"></param>
        /// <param name="u_name"></param>
        /// <param name="u_phone"></param>
        /// <param name="dateTime"></param>
        /// <param name="u_num"></param>
        /// <param name="u_traveldate"></param>
        /// <param name="comid"></param>
        /// <param name="agentid"></param>
        /// <param name="proid"></param>
        /// <returns></returns>
        public int Insertb2b_order_busNamelist(int orderid, string travelname, string travelidcard, string travelnation, string u_name, string u_phone, DateTime dateTime, string u_num, string u_traveldate, int comid, int agentid, int proid, string pickuppoint, string dropoffpoint, string travelphone = "", string travelremark = "", string travelpinyin = "", string traveladdress = "", string travelpostcode = "", string travelemail = "", string travelcredentialsType = "")
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).Insertb2b_order_busNamelist(orderid, travelname, travelidcard, travelnation, u_name, u_phone, dateTime, u_num, u_traveldate, comid, agentid, proid, pickuppoint, dropoffpoint, travelphone, travelremark, travelpinyin, traveladdress, travelpostcode, travelemail, travelcredentialsType);
                return r;
            }
        }
        /// <summary>
        /// 根据订单状态查询 旅游大巴订单子表 --乘客表
        /// </summary>
        /// <param name="gooutdate"></param>
        /// <param name="proid"></param>
        /// <param name="paystate"></param>
        /// <returns></returns>
        public IList<b2b_order_busNamelist> travelbustravelerlistBypaystate(string gooutdate, int proid, int paystate, int agentid = 0, string orderstate = "")
        {
            using (var helper = new SqlHelper())
            {
                IList<b2b_order_busNamelist> r = new InternalB2bOrder(helper).travelbustravelerlistBypaystate(gooutdate, proid, paystate, agentid, orderstate);
                return r;
            }
        }

        public List<b2b_order_busNamelist> travelbusordertravalerdetail(string gooutdate, int proid, int order_state)
        {
            using (var helper = new SqlHelper())
            {
                List<b2b_order_busNamelist> list = new InternalB2bOrder(helper).travelbusordertravalerdetail(gooutdate, proid, order_state);
                return list;
            }
        }

        #region 已消费
        public static decimal Xiaofei_price(int proid, int comid, string startdate, string enddate)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bOrder(sql);
                    var result = internalData.Xiaofei_price(proid, comid, startdate, enddate);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 倒码已消费
        public static decimal Daoma_Xiaofei_price(int proid, int comid, string startdate, string enddate)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bOrder(sql);
                    var result = internalData.Daoma_Xiaofei_price(proid, comid, startdate, enddate);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 沉淀
        public static decimal Chendian_price(int proid, int comid, string startdate, string enddate)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bOrder(sql);
                    var result = internalData.Chendian_price(proid, comid, startdate, enddate);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 退票
        public static decimal Tuipiao_price(int proid, int comid, string startdate, string enddate)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bOrder(sql);
                    var result = internalData.Tuipiao_price(proid, comid, startdate, enddate);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 直销订单- 退票
        public static decimal BackPrice(string comid, string startdate, string enddate)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bOrder(sql);
                    var result = internalData.BackPrice(comid, startdate, enddate);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 直销订单- 微信
        public static decimal WeixinSale(string comid, string startdate, string enddate)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bOrder(sql);
                    var result = internalData.WeixinSale(comid, startdate, enddate);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 直销订单- 微信
        public static decimal WebSale(string comid, string startdate, string enddate)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bOrder(sql);
                    var result = internalData.WebSale(comid, startdate, enddate);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 直销订单- 已消费
        public static decimal UseState(string comid, string startdate, string enddate)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bOrder(sql);
                    var result = internalData.UseState(comid, startdate, enddate);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 直销订单- 未消费
        public static decimal UnUseState(string comid, string startdate, string enddate)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bOrder(sql);
                    var result = internalData.UnUseState(comid, startdate, enddate);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        /// <summary>
        /// 根据订单号得到客户上车地点
        /// </summary>
        /// <param name="order_no"></param>
        /// <returns></returns>
        public string GetPickuppointByorderid(int order_no)
        {
            using (var helper = new SqlHelper())
            {
                string d = new InternalB2bOrder(helper).GetPickuppointByorderid(order_no);
                return d;
            }
        }
        /// <summary>
        /// 查询是否含有旅游大巴订单
        /// </summary>
        /// <param name="comid"></param>
        /// <param name="servertype"></param>
        /// <returns></returns>
        public int IsHasLvyoubusProOrder(int comid, int servertype)
        {
            using (var helper = new SqlHelper())
            {

                var result = new InternalB2bOrder(helper).IsHasLvyoubusProOrder(comid, servertype);

                return result;
            }
        }
        /// <summary>
        /// 把退票张数记录到订单表中
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public int InsertCancleTicketNum(int orderid, int num)
        {
            using (var helper = new SqlHelper())
            {

                var result = new InternalB2bOrder(helper).InsertCancleTicketNum(orderid, num);

                return result;
            }
        }

        /// <summary>
        /// 根据分销对外接口请求流水号查询得到订单信息
        /// </summary>
        /// <param name="分销对外接口请求流水号"></param>
        /// <returns></returns>
        public B2b_order GetOrderByAgentRequestReqSeq(string req_seq)
        {
            using (var helper = new SqlHelper())
            {

                B2b_order result = new InternalB2bOrder(helper).GetOrderByAgentRequestReqSeq(req_seq);

                return result;
            }
        }
        /// <summary>
        /// 根据订单编号得到分销授权类型
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public int GetWarrant_typeByOrderid(int orderid)
        {
            using (var helper = new SqlHelper())
            {

                int result = new InternalB2bOrder(helper).GetWarrant_typeByOrderid(orderid);

                return result;
            }

        }

        #region 保存常用地址
        public int SaveAddress(B2b_order order)
        {
            using (var helper = new SqlHelper())
            {

                var orderid = new InternalB2bOrder(helper).SaveAddress(order);

                return orderid;
            }
        }
        #endregion
        #region 新建实体类b2b_address后添加的方法 by xiaoliu
        public int SaveAddress(B2b_address m)
        {
            using (var helper = new SqlHelper())
            {

                var orderid = new InternalB2bOrder(helper).SaveAddress(m);

                return orderid;
            }
        }
        #endregion

        #region 修改常用地址
        public int UpSaveAddress(B2b_order order)
        {
            using (var helper = new SqlHelper())
            {

                var orderid = new InternalB2bOrder(helper).UpSaveAddress(order);

                return orderid;
            }
        }
        #endregion
        #region 删除常用地址
        public int DelSaveAddress(int id, int agentid)
        {
            using (var helper = new SqlHelper())
            {

                var orderid = new InternalB2bOrder(helper).DelSaveAddress(id, agentid);

                return orderid;
            }
        }
        #endregion


        #region 查询购物车有此产品吗
        public int SearchCart(int comid, int agentid, int proid, int Speciid)
        {
            using (var helper = new SqlHelper())
            {

                var orderid = new InternalB2bOrder(helper).SearchCart(comid, agentid, proid, Speciid);

                return orderid;
            }
        }
        #endregion
        #region 查询购物车产品数量
        public int SearchCartCount(int comid, int agentid)
        {
            using (var helper = new SqlHelper())
            {

                var orderid = new InternalB2bOrder(helper).SearchCartCount(comid, agentid);

                return orderid;
            }
        }
        #endregion

        #region 查询购物车有此产品列表
        public List<B2b_com_pro> SearchCartList(int comid, int agentid, string proid, string cartid = "")
        {
            using (var helper = new SqlHelper())
            {


                var data = new InternalB2bOrder(helper).SearchCartList(comid, agentid, proid, cartid);

                return data;
            }
        }
        #endregion

        #region 插入购物车
        public int InsertCart(B2b_order order)
        {
            using (var helper = new SqlHelper())
            {

                var orderid = new InternalB2bOrder(helper).InsertCart(order);

                return orderid;
            }
        }
        #endregion

        #region 修改购物车
        public int UpCartNum(B2b_order order)
        {
            using (var helper = new SqlHelper())
            {

                var orderid = new InternalB2bOrder(helper).UpCartNum(order);

                return orderid;
            }
        }
        #endregion

        #region 删除购物车
        public int DelCart(B2b_order order, string cartid)
        {
            using (var helper = new SqlHelper())
            {

                var orderid = new InternalB2bOrder(helper).DelCart(order, cartid);

                return orderid;
            }
        }
        #endregion




        #region 直销用户查询购物车有此产品吗
        public int SearchUserCart(int comid, string userid, int proid, int Speciid)
        {
            using (var helper = new SqlHelper())
            {

                var orderid = new InternalB2bOrder(helper).SearchUserCart(comid, userid, proid, Speciid);

                return orderid;
            }
        }
        #endregion

        #region 直销用户查询购物车有此产品吗
        public List<B2b_com_pro> SearchUserCartList(int comid, string userid, string proid, string speciid = "")
        {
            using (var helper = new SqlHelper())
            {


                var data = new InternalB2bOrder(helper).SearchUserCartList(comid, userid, proid, speciid);

                return data;
            }
        }
        #endregion


        #region 查询购物车产品by cartid
        public List<B2b_com_pro> SearchCartListBycartid(string cartid)
        {
            using (var helper = new SqlHelper())
            {


                var data = new InternalB2bOrder(helper).SearchCartListBycartid(cartid);

                return data;
            }
        }
        #endregion

        #region 返回购物车规格产品
        public string SearchUserCartSpeciNameList(int comid, int cartid, int speciid, string viewtype)
        {
            using (var helper = new SqlHelper())
            {


                var data = new InternalB2bOrder(helper).SearchUserCartSpeciNameList(comid, cartid, speciid, viewtype);

                return data;
            }
        }
        #endregion


        #region 返回购物车规格产品
        public decimal SearchUserCartSpeciPrcieList(int comid, int cartid, int speciid, string viewtype)
        {
            using (var helper = new SqlHelper())
            {


                var data = new InternalB2bOrder(helper).SearchUserCartSpeciPrcieList(comid, cartid, speciid, viewtype);

                return data;
            }
        }
        #endregion


        #region 直销用户查询订单产品
        public List<B2b_com_pro> SearchUserOrderList(int comid, int cartid, string proid)
        {
            using (var helper = new SqlHelper())
            {


                var data = new InternalB2bOrder(helper).SearchUserOrderList(comid, cartid, proid);

                return data;
            }
        }
        #endregion


        #region 直销用户查询购物车有此产品吗
        public decimal SumCartPrice(int comid, string userid, string cartid)
        {
            using (var helper = new SqlHelper())
            {
                var data = new InternalB2bOrder(helper).SumCartPrice(comid, userid, cartid);

                return data;
            }
        }
        #endregion

        #region 直销用户查询购物车有此产品吗
        public decimal SumCartSpeciPrice(int comid, string userid, string Speciid)
        {
            using (var helper = new SqlHelper())
            {
                var data = new InternalB2bOrder(helper).SumCartSpeciPrice(comid, userid, Speciid);

                return data;
            }
        }
        #endregion


        #region 插入购物车
        public int InsertUserCart(B2b_order order)
        {
            using (var helper = new SqlHelper())
            {

                var orderid = new InternalB2bOrder(helper).InsertUserCart(order);

                return orderid;
            }
        }
        #endregion

        #region 修改购物车
        public int UpUserCartNum(B2b_order order)
        {
            using (var helper = new SqlHelper())
            {

                var orderid = new InternalB2bOrder(helper).UpUserCartNum(order);

                return orderid;
            }
        }
        #endregion

        #region 删除购物车
        public int DelUserCart(B2b_order order, string cartid)
        {
            using (var helper = new SqlHelper())
            {

                var orderid = new InternalB2bOrder(helper).DelUserCart(order, cartid);

                return orderid;
            }
        }
        #endregion



        public List<B2b_order> SaveAddressPageList(int agentid, int pageindex, int pagesize, string key, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bOrder(helper).SaveAddressPageList(agentid, pageindex, pagesize, key, out totalcount);

                return list;
            }
        }


        public B2b_address Getagentaddrbyid(int addrid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bOrder(helper).Getagentaddrbyid(addrid);

                return list;
            }
        }

        public int Deladdr(int addrid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bOrder(helper).Deladdr(addrid);

                return list;
            }

        }

        public decimal GetAgentCartTotalPrice(int comid, int agentid, string proid, int Agentlevel)
        {
            using (var helper = new SqlHelper())
            {


                var data = new InternalB2bOrder(helper).GetAgentCartTotalPrice(comid, agentid, proid, Agentlevel);

                return data;
            }
        }


        #region 根据id获得购物车合计金额
        public decimal GetCartOrderMoneyById(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                var order = new InternalB2bOrder(helper).GetCartOrderMoneyById(orderid);
                return order;
            }
        }
        #endregion

        #region 根据id获得购物车快递费
        public decimal GetCartOrderExpressMoneyById(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                var order = new InternalB2bOrder(helper).GetCartOrderExpressMoneyById(orderid);
                return order;
            }
        }
        #endregion


        #region 根据id获得购物车产品列表名称
        public string GetCartOrderProById(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                var order = new InternalB2bOrder(helper).GetCartOrderProById(orderid);
                return order;
            }
        }
        #endregion

        #region 根据id获得购物车产品列表
        public IList<B2b_order> shopcartorder(int shoporderid)
        {
            using (var helper = new SqlHelper())
            {
                IList<B2b_order> r = new InternalB2bOrder(helper).shopcartorder(shoporderid);
                return r;
            }
        }
        #endregion


        public int Uporderstate(int orderid, int orderstate)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).Uporderstate(orderid, orderstate);
                return r;
            }
        }

        public int UpB2borderPhone(int orderid, string phone)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).UpB2borderPhone(orderid, phone);
                return r;
            }
        }
        /// <summary>
        /// 根据订单号 得到绑定订单号
        /// </summary>
        /// <returns></returns>
        public int GetBindOrderIdByOrderid(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).GetBindOrderIdByOrderid(orderid);
                return r;
            }
        }

        public int UpHandlid(int id, int Handlid)
        {
            using (var helper = new SqlHelper())
            {
                var order = new InternalB2bOrder(helper).UpHandlid(id, Handlid);
                return order;
            }
        }


        public bool Ishas_qunarorder(string qunar_orderId)
        {
            using (var helper = new SqlHelper())
            {
                bool r = new InternalB2bOrder(helper).Ishas_qunarorder(qunar_orderId);
                return r;
            }
        }

        public int InsertQunar_Orderid(int parterorderid, string qunar_orderid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).InsertQunar_Orderid(parterorderid, qunar_orderid);
                return r;
            }
        }

        public int GetParterOrderId(string qunar_orderId)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).GetParterOrderId(qunar_orderId);
                return r;
            }
        }
        /// <summary>
        /// 根据电子码得到 已经消费的数量
        /// </summary>
        /// <param name="pno"></param>
        /// <returns></returns>
        public int GetHasConsumeNumByPno(string pno)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).GetHasConsumeNumByPno(pno);
                return r;
            }
        }
        /// <summary>
        /// 得到订单状态
        /// </summary>
        /// <param name="partnerorderId"></param>
        /// <returns></returns>
        public int GetOrderState(string partnerorderId)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).GetOrderState(partnerorderId);
                return r;
            }
        }
        /// <summary>
        /// 修改订单人预订人信息
        /// </summary>
        /// <param name="partnerorderId"></param>
        /// <param name="c_name"></param>
        /// <param name="c_mobile"></param>
        /// <returns></returns>
        public int UpdateOrder_contactperson(string partnerorderId, string c_name, string c_mobile)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).UpdateOrder_contactperson(partnerorderId, c_name, c_mobile);
                return r;
            }
        }
        /// <summary>
        /// 根据订单号得到已经消费的电子票数
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public int GetHasConsumeNumByOrderId(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).GetHasConsumeNumByOrderId(orderid);
                return r;
            }
        }
        /// <summary>
        /// 取消订单（标注为超时订单）
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool CancelOrder(string orderid)
        {
            using (var helper = new SqlHelper())
            {
                bool r = new InternalB2bOrder(helper).CancelOrder(orderid);
                return r;
            }
        }
        /// <summary>
        /// 得到直销订单可用数量
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public int GetSurplusNum(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                //判断订单是否生成了电子码：没有 则直接读取订单中的张数；有 则按正常流程读电子票表中的可用张数
                int ishaseticket = new InternalB2bOrder(helper).Getishaseticket(orderid);
                if (ishaseticket == 0)
                {
                    int num = new InternalB2bOrder(helper).GetOrderBookNum(orderid);
                    return num;
                }
                else
                {
                    int num = new InternalB2bOrder(helper).GetSurplusNum(orderid);
                    return num;
                }

            }
        }

        public int Insquitnum(int orderid, int refundNum)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).Insquitnum(orderid, refundNum);
                return r;
            }
        }

        public int Upqunarcashback(string partnerorderId, string orderCashBackMoney)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).Upqunarcashback(partnerorderId, orderCashBackMoney);
                return r;
            }
        }
        /// <summary>
        /// 录入糯米交易号
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="nuomidealid"></param>
        /// <returns></returns>
        public int InsNuomiDealid(int orderid, int nuomidealid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).InsNuomiDealid(orderid, nuomidealid);
                return r;
            }

        }
        /// <summary>
        /// 得到订单 的糯米交易号
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public string GetNuomi_dealid(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                string r = new InternalB2bOrder(helper).GetNuomi_dealid(orderid);
                return r;
            }
        }
        /// <summary>
        /// 根据导入产品的分销订单 得到 原始订单
        /// </summary>
        /// <param name="importorderid"></param>
        /// <returns></returns>
        public int Getinitorderid(int importorderid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).Getinitorderid(importorderid);
                return r;
            }
        }

        public int GetConsumeNum(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).GetConsumeNum(orderid);
                return r;
            }
        }



        public B2b_order GetOldorderBybindingId(int bindingorderid)
        {
            using (var helper = new SqlHelper())
            {
                B2b_order m = new InternalB2bOrder(helper).GetOldorderBybindingId(bindingorderid);
                return m;
            }
        }

        public int GetOrderidbypno(string pno)
        {
            using (var helper = new SqlHelper())
            {
                int m = new InternalB2bOrder(helper).GetOrderidbypno(pno);
                return m;
            }
        }

        public IList<B2b_order> Getclientlistbyagentid(int Pageindex, int Pagesize, string Key, int Agentid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                IList<B2b_order> list = new InternalB2bOrder(helper).Getclientlistbyagentid(Pageindex, Pagesize, Key, Agentid, out totalcount);
                return list;
            }
        }

        public int GetShopcartidbyoid(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).GetShopcartidbyoid(orderid);
                return r;
            }
        }

        public int InsAskquitfee(int orderid, int quit_num, decimal quit_fee, string quit_Reason, string quit_info)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).InsAskquitfee(orderid, quit_num, quit_fee, quit_Reason, quit_info);

                return r;
            }
        }

        public int UporderRecommendMannelid(int crmid, int orderid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).UporderRecommendMannelid(crmid, orderid);
                return r;
            }
        }


        public int GetevaluateidByoid(int oid, int evatype)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).GetevaluateidByoid(oid, evatype);
                return r;
            }
        }

        public int Insertevaluateid(int comid, int uid, int oid, int channelid, int annonymous, int star, int evatype, string area)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).Insertevaluateid(comid, uid, oid, channelid, annonymous, star, evatype, area);
                return r;
            }
        }

        public int updateevaluateid(int id, int annonymous, int star, string area)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).updateevaluateid(id, annonymous, star, area);
                return r;
            }
        }


        public List<B2b_order_evaluate> EvaluatePageList(int comid, int oid, int uid, int channelid, int evatype, int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bOrder(helper).EvaluatePageList(comid, oid, uid, channelid, evatype, pageindex, pagesize, out totalcount);

                return list;
            }
        }

        public List<B2b_order_evaluate> EvaluatePageinfo(int id, int evatype)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bOrder(helper).EvaluatePageinfo(id, evatype);

                return list;
            }
        }

        public int Evaluateyesno(int oid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).Evaluateyesno(oid);
                return r;
            }
        }

        public int InsertSmsback(int type, string mobile, string state, string con, string time)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).InsertSmsback(type, mobile, state, con, time);
                return r;
            }

        }


        internal int GetProIdbyorderid(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).GetProIdbyorderid(orderid);
                return r;
            }
        }

        public int UpOrderRemark(int orderid, string orderremark)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).UpOrderRemark(orderid, orderremark);
                return r;
            }
        }

        public int UpOrderStateAndRemark(int orderid, int orderstate, string orderremark)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).UpOrderStateAndRemark(orderid, orderstate, orderremark);
                return r;
            }
        }

        public int GetOrderidByServiceOrderNum(string serviceorder_num)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).GetOrderidByServiceOrderNum(serviceorder_num);
                return r;
            }
        }

        public int GetServiceLastcount(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).GetServiceLastcount(orderid);
                return r;
            }
        }

        public string GetService_code(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                string r = new InternalB2bOrder(helper).GetService_code(orderid);
                return r;
            }
        }


        #region 获取电子票兑换数量
        public int GetOrderyuyuepnoshulaingBypno(string pno, int yuyueweek)
        {
            using (var helper = new SqlHelper())
            {
                var order = new InternalB2bOrder(helper).GetOrderyuyuepnoshulaingBypno(pno, yuyueweek);
                return order;
            }
        }
        #endregion


        #region 根据id获得订单信息
        public int GedangriyuyueBypno(string pno, int proid, DateTime traveldate)
        {
            using (var helper = new SqlHelper())
            {
                var order = new InternalB2bOrder(helper).GedangriyuyueBypno(pno, proid, traveldate);
                return order;
            }
        }
        #endregion

        public int UpGivebxorderid(int orderid, int bxorderid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).UpGivebxorderid(orderid, bxorderid);
                return r;
            }

        }

        public int Upyuyueweekid(int orderid, int yuyueweek)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).Upyuyueweekid(orderid, yuyueweek);
                return r;
            }

        }

        /// <summary>
        /// 赠送保险订单 和 订单 关联
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="givebaoxianorderid"></param>
        /// <returns></returns>
        public int GuanlianBxorder(int orderid, int givebaoxianorderid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).GuanlianBxorder(orderid, givebaoxianorderid);
                return r;
            }
        }
        /// <summary>
        /// 被保险人 出游当天 上过保险的数量
        /// </summary>
        /// <param name="idcard"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public int GetSamedayBaoxianOrderNum(string idcard, DateTime dateTime)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).GetSamedayBaoxianOrderNum(idcard, dateTime);
                return r;
            }
        }

        public IList<b2b_order_busNamelist> GetTravelBusNamelist(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                IList<b2b_order_busNamelist> r = new InternalB2bOrder(helper).GetTravelBusNamelist(orderid);
                return r;
            }
        }

        public int Changetraveldate(int orderid, string testpro, string traveldate, string oldtraveldate, out string message)
        {
            using (var helper = new SqlHelper())
            {
                //判断是否有a订单
                int aorderid = new B2bOrderData().Getinitorderid(orderid);
                if (aorderid > 0)
                {
                    orderid = aorderid;
                }

                int r = new InternalB2bOrder(helper).Changetraveldate(orderid, testpro, traveldate, oldtraveldate, out message);
                return r;
            }
        }

        public int TagBusride(int orderbusrideid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).TagBusride(orderbusrideid);
                return r;
            }
        }

        public int Getgivebaoxinorderid(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).Getgivebaoxinorderid(orderid);
                return r;
            }
        }

        public List<B2b_Rentserver_RefundLog> GetyajinrefundLoglist(int pageindex, int pagesize, string key, int refundstate, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_Rentserver_RefundLog> r = new InternalB2b_Rentserver_RefundLog(helper).GetyajinrefundLoglist(pageindex, pagesize, key, refundstate, out totalcount);
                return r;
            }
        }

        public List<B2b_order> GetAgentOrderList(int comid, int agentid, string key, int order_state, string begindate, string enddate, int servertype)
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_order> r = new InternalB2bOrder(helper).GetAgentOrderList(comid, agentid, key, order_state, begindate, enddate, servertype);
                return r;
            }
        }

        public string GetPhoneByOrderid(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                string r = new InternalB2bOrder(helper).GetPhoneByOrderid(orderid);
                return r;
            }
        }

        public int upprobindinfo(int orderid, string bindname, string bindcompany, string bindphone)
        {
            using (var helper = new SqlHelper())
            {
                var r = new InternalB2bOrder(helper).upprobindinfo(orderid, bindname, bindcompany, bindphone);
                return r;
            }
        }

        public List<B2b_order> GetOrderList(int comid, int order_state, int servertype, string begindate, string enddate, string key, int userid, int orderIsAccurateToPerson, int channelcompanyid, int ordertype)
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_order> r = new InternalB2bOrder(helper).GetOrderList(comid, order_state, servertype, begindate, enddate, key, userid, orderIsAccurateToPerson, channelcompanyid, ordertype);
                return r;
            }
        }

        public DataTable HotelOrderlist(int comid, string begindate, string enddate, int productid, string orderstate, int isdownexcel = 0)
        {
            var condition = " where  a.order_state in (2,4,8,22)  and a.pro_id in (select id from b2b_com_pro where server_type=9 and comid=" + comid + ") and a.comid=" + comid;
            if (begindate != "" && enddate != "")
            {
                begindate = DateTime.Parse(begindate).ToString("yyyy-MM-dd 00:00:00");
                enddate = DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59");
                condition = condition + " and b.start_date between '" + begindate + "' and '" + enddate + "'";
            }

            if (productid != 0)
            {
                condition = condition + " and a.pro_id =" + productid;
            }


            string sql = "select a.bookpro_bindcompany,a.bookpro_bindconfirmtime,a.bookpro_bindname,a.bookpro_bindphone,a.pro_id ,c.pro_name,a.id as ordernum,a.u_subdate,a.u_name,a.u_phone,a.u_num,b.start_date,b.end_date,b.bookdaynum from b2b_order as a left join b2b_order_hotel as b on a.id=b.orderid left join b2b_com_pro as c on a.pro_id=c.id " + condition + "  order by a.u_subdate";
            DataTable dt = ExcelSqlHelper.ExecuteDataTable(sql);
            if (dt != null)
            {
                DataTable tblDatas = new DataTable("Datas");
                DataColumn dc = null;
                dc = tblDatas.Columns.Add("ID", Type.GetType("System.Int32"));
                dc.AutoIncrement = true;//自动增加
                dc.AutoIncrementSeed = 1;//起始为1
                dc.AutoIncrementStep = 1;//步长为1
                dc.AllowDBNull = false;

                //如果打印到excel，则列明需要改变一下
                if (isdownexcel == 1)
                {
                    dc = tblDatas.Columns.Add("产品编号", Type.GetType("System.Int32"));
                    dc = tblDatas.Columns.Add("产品名称", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("订单号", Type.GetType("System.Int32"));
                    dc = tblDatas.Columns.Add("提单时间", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("提单人", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("项目名称", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("提单电话", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("预定间数", Type.GetType("System.Int32"));
                    dc = tblDatas.Columns.Add("入住日期", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("离店日期", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("入住天数", Type.GetType("System.Int32"));

                    dc = tblDatas.Columns.Add("绑定人公司", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("绑定人确认时间", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("绑定人姓名", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("绑定人电话", Type.GetType("System.String"));


                    DataRow newRow;
                    foreach (DataRow row in dt.Rows)
                    {
                        newRow = tblDatas.NewRow();
                        newRow["产品编号"] = int.Parse(row["pro_id"].ToString());
                        newRow["产品名称"] = row["pro_name"].ToString();
                        newRow["订单号"] = int.Parse(row["ordernum"].ToString());
                        newRow["提单时间"] = row["u_subdate"].ToString();
                        newRow["项目名称"] = new B2bComProData().GetProjectNameByProid(row["pro_id"].ToString());
                        newRow["提单人"] = row["u_name"].ToString();
                        newRow["提单电话"] = row["u_phone"].ToString();
                        newRow["预定间数"] = int.Parse(row["u_num"].ToString());
                        newRow["入住日期"] = row["start_date"].ToString();
                        newRow["离店日期"] = row["end_date"].ToString();
                        newRow["入住天数"] = row["bookdaynum"].ToString().ConvertTo<int>(9999);

                        newRow["绑定人公司"] = row["bookpro_bindcompany"].ToString();
                        newRow["绑定人确认时间"] = row["bookpro_bindconfirmtime"].ToString();
                        newRow["绑定人姓名"] = row["bookpro_bindname"].ToString();
                        newRow["绑定人电话"] = row["bookpro_bindphone"].ToString();
                        tblDatas.Rows.Add(newRow);
                    }
                }
                else
                {
                    dc = tblDatas.Columns.Add("pro_id", Type.GetType("System.Int32"));
                    dc = tblDatas.Columns.Add("pro_name", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("ordernum", Type.GetType("System.Int32"));
                    dc = tblDatas.Columns.Add("u_subdate", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("u_name", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("projectname", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("u_phone", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("u_num", Type.GetType("System.Int32"));
                    dc = tblDatas.Columns.Add("start_date", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("end_date", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("bookdaynum", Type.GetType("System.Int32"));

                    dc = tblDatas.Columns.Add("bookpro_bindcompany", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("bookpro_bindconfirmtime", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("bookpro_bindname", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("bookpro_bindphone", Type.GetType("System.String"));

                    DataRow newRow;
                    foreach (DataRow row in dt.Rows)
                    {
                        newRow = tblDatas.NewRow();
                        newRow["pro_id"] = int.Parse(row["pro_id"].ToString());
                        newRow["pro_name"] = row["pro_name"].ToString();
                        newRow["ordernum"] = int.Parse(row["ordernum"].ToString());
                        newRow["u_subdate"] = row["u_subdate"].ToString();
                        newRow["projectname"] = new B2bComProData().GetProjectNameByProid(row["pro_id"].ToString());
                        newRow["u_name"] = row["u_name"].ToString();
                        newRow["u_phone"] = row["u_phone"].ToString();
                        newRow["u_num"] = int.Parse(row["u_num"].ToString());
                        newRow["start_date"] = row["start_date"].ToString();
                        newRow["end_date"] = row["end_date"].ToString();
                        newRow["bookdaynum"] = row["bookdaynum"].ToString().ConvertTo<int>(9999);

                        newRow["bookpro_bindcompany"] = row["bookpro_bindcompany"].ToString();
                        newRow["bookpro_bindconfirmtime"] = row["bookpro_bindconfirmtime"].ToString();
                        newRow["bookpro_bindname"] = row["bookpro_bindname"].ToString();
                        newRow["bookpro_bindphone"] = row["bookpro_bindphone"].ToString();
                        tblDatas.Rows.Add(newRow);
                    }
                }


                return tblDatas;
            }
            else
            {
                return null;
            }
        }
        /// <summary> 
        /// 得到超时订单的产品列表 
        /// !!!!!注：考虑到是定时任务用到的，运行速度需要保证快，所以只是返回了需要的几个值(id,server_type,ispanicbuy,Manyspeci,Bindingid)，并没有返回产品的全部信息
        /// </summary>
        /// <returns></returns>
        public List<B2b_com_pro> GetTimeoutOrderProlist()
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_com_pro> list = new InternalB2bComPro(helper).GetTimeoutOrderProlist();
                return list;
            }
        }

        /// <summary>
        /// 订单总金额
        /// </summary>
        /// <param name="modelcompro"></param>
        /// <param name="modelb2border"></param>
        /// <param name="price">单价</param>
        /// <param name="pricedetail">价格描述</param>
        /// <returns></returns>
        public decimal GetOrderTotalPrice(B2b_com_pro modelcompro, B2b_order modelb2border, out string price, out string pricedetail)
        {
            decimal p_totalprice1 = 0;

            int orderid = modelb2border.Id;
            int servertype = modelcompro.Server_type;
            //如果服务类型是“酒店客房”，则根据酒店扩展订单表中房态信息，获取支付金额
            if (modelcompro.Server_type == 9)
            {
                B2b_order_hotel hotelorder = new B2b_order_hotelData().GetHotelOrderByOrderId(orderid);
                if (hotelorder != null)
                {
                    string fangtai = hotelorder.Fangtai;
                    DateTime start_data = hotelorder.Start_date;
                    DateTime end_data = hotelorder.End_date;
                    int bookdaynum = hotelorder.Bookdaynum;

                    decimal everyroomprice = 0;
                    string[] ftstr = fangtai.Split(',');
                    for (int i = 0; i < ftstr.Length; i++)
                    {
                        if (ftstr[i].ConvertTo<decimal>(0) > 0)
                        {
                            everyroomprice += ftstr[i].ConvertTo<decimal>(0);
                        }
                    }
                    pricedetail = "";
                    price = everyroomprice.ToString();

                    p_totalprice1 = (modelb2border.U_num * everyroomprice - modelb2border.Integral1 - modelb2border.Imprest1);

                }
                else
                {
                    pricedetail = "";
                    price = "0";
                }
            }
            else if (servertype == 2 || servertype == 8)//当地游；跟团游
            {
                string outdate = modelb2border.U_traveldate.ToString("yyyy-MM-dd");

                //读取団期价格,根据实际选择的団期报价
                B2b_com_LineGroupDate linemode = new B2b_com_LineGroupDateData().GetLineDayGroupDate(DateTime.Parse(outdate), modelcompro.Id);
                if (linemode != null)
                {
                    price = linemode.Menprice.ToString();
                    decimal childreduce = modelcompro.Childreduce;
                    decimal childprice = decimal.Parse(price) - childreduce;
                    if (childprice < 0)
                    {
                        childprice = 0;
                    }
                    pricedetail = modelb2border.U_num + "成人," + modelb2border.Child_u_num + "儿童(成人" + price + "元/人，儿童" + childprice + "元/人)";

                    p_totalprice1 = (modelb2border.U_num * (linemode.Menprice) + (modelb2border.Child_u_num) * childprice - modelb2border.Integral1 - modelb2border.Imprest1);
                }
                else
                {
                    pricedetail = "";
                    price = "0";
                }
            }
            else //票务;实物产品
            {
                p_totalprice1 = (modelb2border.U_num * modelb2border.Pay_price + modelb2border.Express - modelb2border.Integral1 - modelb2border.Imprest1);
                price = modelb2border.Pay_price.ToString(); //modelb2border.Pay_price.ToString();
                if (price == "0.00" || price == "0")
                {
                    price = "0";
                }
                else
                {
                    price = CommonFunc.OperTwoDecimal(price);
                }

                //只有实物订单有购物车，并且重新计算购物车订单金额
                if (modelb2border.Shopcartid > 0)
                {
                    p_totalprice1 = new B2bOrderData().GetCartOrderMoneyById(orderid);
                }

                pricedetail = "";
            }
            return p_totalprice1;
        }

        public int GetPaystateByOrderid(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bComPro(helper).GetPaystateByOrderid(orderid);
                return r;
            }
        }

        public List<B2b_order> prosaleordercount(int comid, string begindate, string enddate, int projectid, int productid, string key, string orderstate, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                var order = new InternalB2bOrder(helper).prosaleordercount(comid, begindate, enddate, projectid, productid, key, orderstate, out totalcount);
                return order;
            }
        }

        public int proyanzhengordercount(int comid, string begindate, string enddate, int projectid, int productid,int agentid)
        {
            using (var helper = new SqlHelper())
            {
                var order = new InternalB2bOrder(helper).proyanzhengordercount(comid, begindate, enddate, projectid, productid, agentid);
                return order;
            }
        }
        public int GetNeedVisitDateOrderPaysucNum(DateTime daydate, int servertype, int comid, int paystate, int agentid = 0, string orderstate = "")
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).GetNeedVisitDateOrderPaysucNum(daydate, servertype, comid, paystate, agentid, orderstate);
                return r;
            }
        }

        public List<B2b_order> getNeedvisitdatePaysucorderlist(DateTime gooutdate, int proid, int paystate, string orderstate)
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_order> r = new InternalB2bOrder(helper).getNeedvisitdatePaysucorderlist(gooutdate, proid, paystate, orderstate);
                return r;
            }
        }

        public int HasFinOrder(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bOrder(helper).HasFinOrder(orderid);
                return r;
            }
        }

    }
}
