using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.WL.Model;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.Framework;
namespace ETS2.PM.Service.WL.Data.Internal
{
    public class InternalWlGetProInfoDealRequest
    {
        public SqlHelper sqlHelper;
        public InternalWlGetProInfoDealRequest(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        //插入拉取的产品
        internal int InsertorUpdateWlProDeal(WlDealResponseBody m)
        {
            if (m.id == 0)
            {
                string sql = @"INSERT INTO  [WL_pro]
                           (comid,
                            subtime,
                            useDateMode,
                            visitorInfoType,
                            proID,
                            scheduleOnlineTime,
                            scheduleOfflineTime,
                            needTicket,
                            orderCancelTime,
                            stock,
                            marketPrice,
                            wlPrice,
                            settlementPrice,
                            title,
                            subTitle,
                            include,
                            exclude,
                            partnerId,
                            voucherDateBegin,
                            voucherDateEnd,
                            stockMode
                            )
                     VALUES
                           (
                             @comid
                            ,@subtime
                            ,@useDateMode
                            ,@visitorInfoType
                            ,@proID
                            ,@scheduleOnlineTime
                            ,@scheduleOfflineTime
                            ,@needTicket
                            ,@orderCancelTime
                            ,@stock
                            ,@marketPrice
                            ,@wlPrice
                            ,@settlementPrice
                            ,@title
                            ,@subTitle
                            ,@include
                            ,@exclude
                            ,@partnerId
                            ,@voucherDateBegin
                            ,@voucherDateEnd
                            ,@stockMode
                            );select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@comid", m.comid);
                cmd.AddParam("@subtime", DateTime.Now);
                cmd.AddParam("@useDateMode", m.useDateMode);
                cmd.AddParam("@visitorInfoType", m.visitorInfoType);
                cmd.AddParam("@proID", m.proID);
                cmd.AddParam("@scheduleOnlineTime", m.scheduleOnlineTime);
                cmd.AddParam("@scheduleOfflineTime", m.scheduleOfflineTime);
                cmd.AddParam("@needTicket", m.needTicket);
                cmd.AddParam("@orderCancelTime", m.orderCancelTime);
                cmd.AddParam("@stock", m.stock);
                cmd.AddParam("@marketPrice", m.marketPrice);
                cmd.AddParam("@wlPrice", m.wlPrice);
                cmd.AddParam("@settlementPrice", m.settlementPrice);
                cmd.AddParam("@title", m.title);
                cmd.AddParam("@subTitle", m.subTitle);

                if (m.include == null) {
                    m.include = "";
                }

                cmd.AddParam("@include", m.include);

                if (m.exclude == null)
                {
                    m.exclude = "";
                }

                cmd.AddParam("@exclude", m.exclude);

                cmd.AddParam("@partnerId", m.partnerId);
                cmd.AddParam("@voucherDateBegin", m.voucherDateBegin);
                cmd.AddParam("@voucherDateEnd", m.voucherDateEnd);
                cmd.AddParam("@stockMode", m.stockMode);

                object o = cmd.ExecuteScalar();
                return o == null ? 0 : int.Parse(o.ToString());
            }
            else
            {
                string sql = @"UPDATE  [WL_pro]
                               SET comid=@comid,
                            subtime=@subtime,
                            useDateMode=@useDateMode,
                            visitorInfoType=@visitorInfoType,
                            proID=@proID,
                            scheduleOnlineTime=@scheduleOnlineTime,
                            scheduleOfflineTime=@scheduleOfflineTime,
                            needTicket=@needTicket,
                            orderCancelTime=@orderCancelTime,
                            stock=@stock,
                            marketPrice=@marketPrice,
                            wlPrice=@wlPrice,
                            settlementPrice=@settlementPrice,
                            title=@title,
                            subTitle=@subTitle,
                            include=@include,
                            exclude=@exclude,
                            partnerId=@partnerId,
                            voucherDateBegin=@voucherDateBegin,
                            voucherDateEnd=@voucherDateEnd,
                            stockMode=@stockMode,
                            state=@state
                             WHERE id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id", m.id);
                cmd.AddParam("@comid", m.comid);
                cmd.AddParam("@subtime", DateTime.Now);
                cmd.AddParam("@useDateMode", m.useDateMode);
                cmd.AddParam("@visitorInfoType", m.visitorInfoType);
                cmd.AddParam("@proID", m.proID);
                cmd.AddParam("@scheduleOnlineTime", m.scheduleOnlineTime);
                cmd.AddParam("@scheduleOfflineTime", m.scheduleOfflineTime);
                cmd.AddParam("@needTicket", m.needTicket);
                cmd.AddParam("@orderCancelTime", m.orderCancelTime);
                cmd.AddParam("@stock", m.stock);
                cmd.AddParam("@marketPrice", m.marketPrice);
                cmd.AddParam("@wlPrice", m.wlPrice);
                cmd.AddParam("@settlementPrice", m.settlementPrice);
                cmd.AddParam("@title", m.title);
                cmd.AddParam("@subTitle", m.subTitle);
                cmd.AddParam("@include", m.include);
                if (m.exclude == null)
                {
                    m.exclude = "";
                }

                cmd.AddParam("@exclude", m.exclude);
                cmd.AddParam("@partnerId", m.partnerId);
                cmd.AddParam("@voucherDateBegin", m.voucherDateBegin);
                cmd.AddParam("@voucherDateEnd", m.voucherDateEnd);
                cmd.AddParam("@stockMode", m.stockMode);
                cmd.AddParam("@state", m.state);

                cmd.ExecuteNonQuery();
                return m.id;
            }
        }


        //更新前关闭所有以前拉取的产品，更新后自动开通，没开通的就是已经下线产品
        internal int UPCloseWlProDeal(int comid)
        {
                string sql = @"UPDATE  [WL_pro]
                               SET
                            state=@state
                             WHERE  comid=@comid";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@state",0);
                cmd.AddParam("@comid", comid);
                cmd.ExecuteNonQuery();
                return comid;
            
        }



        //查询一条产品逐一核对
        internal WlDealResponseBody SelectproidgetWlProDeal(string proID, int comid)
        {
            string sql = @"SELECT top 1  *
  FROM  WL_pro where   proID=@proID and  comid =@comid order by id desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@proID", proID);
            cmd.AddParam("@comid", comid);
            using (var reader = cmd.ExecuteReader())
            {
                WlDealResponseBody m = null;
                if (reader.Read())
                {
                    m = new WlDealResponseBody
                    {
                        id = reader.GetValue<int>("id"),
                        comid = reader.GetValue<int>("comid"),
                        subtime = reader.GetValue<DateTime>("subtime"),
                        //useDateMode = reader.GetValue<int>("useDateMode"),
                        //visitorInfoType = reader.GetValue<int>("visitorInfoType"),
                        proID = reader.GetValue<string>("proID"),
                        scheduleOnlineTime = reader.GetValue<string>("scheduleOnlineTime"),
                        scheduleOfflineTime = reader.GetValue<string>("scheduleOfflineTime"),
                        //needTicket = reader.GetValue<bool>("needTicket"),
                        //orderCancelTime = reader.GetValue<int>("orderCancelTime"),
                        //stock = reader.GetValue<int>("stock"),
                        //marketPrice = reader.GetValue<double>("marketPrice"),
                        //wlPrice = reader.GetValue<double>("wlPrice"),
                        //settlementPrice = reader.GetValue<double>("settlementPrice"),
                        //title = reader.GetValue<string>("title"),
                        //subTitle = reader.GetValue<string>("subTitle"),
                        //include = reader.GetValue<string>("include"),
                        //exclude = reader.GetValue<string>("exclude"),
                        //partnerId = reader.GetValue<string>("partnerId"),
                        //voucherDateBegin = reader.GetValue<string>("voucherDateBegin"),
                        //voucherDateEnd = reader.GetValue<string>("voucherDateEnd"),
                        //stockMode = reader.GetValue<int>("stockMode")
                    };
                }
                return m;
            }
        }


        //第一次插入订单，没有万龙订单id
        internal int InsertorUpdateWlOrderCreate(wlOrderCreateRequest m, wlOrderCreateResponse p)
        {
            if (m.body.id == 0)
            {
                string sql = @"INSERT INTO  [wl_OrderCreate]
                           (comid,
                            orderid,
                            name,
                            mobile,
                            partnerdealid,
                            partnerId,
                            wldealid,
                            buyprice,
                            unitprice,
                            totalprice,
                            quantity,
                            traveldate,
                            wlorderid,
                            code,
                            describe,
                            status
                            )
                     VALUES
                           (
                             @comid,
                             @partnerOrderId,
                             @name,
                             @mobile,
                             @partnerdealid,
                             @partnerId,
                             @wldealid,
                             @buyprice,
                             @unitprice,
                             @totalprice,
                             @quantity,
                             @traveldate,
                             @wlorderid,
                             @code,
                             @describe,
                             @status
                            );select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@comid", m.body.comid);
                cmd.AddParam("@subtime", DateTime.Now);
                cmd.AddParam("@name", m.body.contactPerson.name);
                cmd.AddParam("@mobile", m.body.contactPerson.mobile);
                cmd.AddParam("@partnerdealid", m.body.partnerDealId);
                cmd.AddParam("@partnerId", m.partnerId);
                cmd.AddParam("@wldealid", m.body.wlDealId);
                cmd.AddParam("@buyprice", m.body.buyPrice);
                cmd.AddParam("@unitprice", m.body.unitPrice);
                cmd.AddParam("@totalprice", m.body.totalPrice);
                cmd.AddParam("@quantity", m.body.quantity);
                cmd.AddParam("@traveldate", m.body.travelDate);
                cmd.AddParam("@partnerOrderId", int.Parse(m.body.partnerOrderId));
                cmd.AddParam("@wlorderid", p.body.wlOrderId);
                cmd.AddParam("@code", p.code);
                cmd.AddParam("@describe", p.describe);
                cmd.AddParam("@status", 1);//第一次插入时候默认返回值为-1

                object o = cmd.ExecuteScalar();
                return o == null ? 0 : int.Parse(o.ToString());
            }
            else
            {
                string sql = @"UPDATE  [wl_OrderCreate]
                               SET comid=@comid,
                             orderid=@partnerOrderId,
                             name=@name,
                             mobile=@mobile,
                             partnerdealid=@partnerdealid,
                             partnerId=@partnerId,
                             wldealid=@wldealid,
                             buyprice=@buyprice,
                             unitprice=@unitprice,
                             totalprice=@totalprice,
                             quantity=@quantity,
                             traveldate=@traveldate,
                             wlorderid=@wlorderid,
                             code=@code,
                             describe=@describe,
                             status=@status
                             WHERE id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id", m.body.id);
                cmd.AddParam("@comid", m.body.comid);
                cmd.AddParam("@subtime", DateTime.Now);
                cmd.AddParam("@name", m.body.contactPerson.name);
                cmd.AddParam("@mobile", m.body.contactPerson.mobile);
                cmd.AddParam("@partnerdealid", m.body.partnerDealId);
                cmd.AddParam("@partnerId", m.partnerId);
                cmd.AddParam("@wldealid", m.body.wlDealId);
                cmd.AddParam("@buyprice", m.body.buyPrice);
                cmd.AddParam("@unitprice", m.body.unitPrice);
                cmd.AddParam("@totalprice", m.body.totalPrice);
                cmd.AddParam("@quantity", m.body.quantity);
                cmd.AddParam("@traveldate", m.body.travelDate);
                cmd.AddParam("@partnerOrderId", int.Parse(m.body.partnerOrderId));
                cmd.AddParam("@wlorderid", p.body.wlOrderId);
                cmd.AddParam("@code", p.code);
                cmd.AddParam("@describe", p.describe);
              
                cmd.ExecuteNonQuery();
                return m.body.id;
            }
        }



        //核销后修改订单使用数量
        internal int UpdateWlOrderPaySC(wl_order_model m)
        {
            if (m != null)
            {
                string sql = @"UPDATE  [wl_OrderCreate]
                               SET 
                             usedQuantity=@usedQuantity,
                             refundedQuantity=@refundedQuantity 
                             WHERE id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);

                cmd.AddParam("@id", m.id);
                cmd.AddParam("@usedQuantity ", m.usedQuantity);
                cmd.AddParam("@refundedQuantity ", m.refundedQuantity);
                cmd.ExecuteNonQuery();
                return m.id;
            }
            else
            {
                return 0;
            }
        }


        //核销日志
        internal int InsertWlUseLog(int comid, string wlorderid, int usedQuantity, int partnerId, int quantity,int orderid,int proid)
        {
            if (comid != 0)
            {
                string sql = @"INSERT INTO  [wl_uselog]
                           (comid,
                            partnerId,
                            quantity,
                            usedQuantity,
                            wlorderid,
                            orderid,
                            proid
                            )
                     VALUES
                           (
                             @comid,
                             @partnerId,
                             @quantity,
                             @usedQuantity,
                             @wlorderid,
                             @orderid,
                             @proid

                            );select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@comid", comid);
                cmd.AddParam("@partnerId", partnerId);
                cmd.AddParam("@quantity", quantity);
                cmd.AddParam("@usedQuantity", usedQuantity);
                cmd.AddParam("@wlorderid", wlorderid);
                cmd.AddParam("@orderid", orderid);
                cmd.AddParam("@proid", proid);

                object o = cmd.ExecuteScalar();
                return o == null ? 0 : int.Parse(o.ToString());
            }
            else
            {
                return 0;
            }
        }


        //万龙接口日志
        internal List<wl_use_log_model> InterfaceUsePageList(int comid, int pageindex, int pagesize, out int totalcount, string key, string startime, string endtime)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "wl_uselog";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";
            var condition = "comid=" + comid ;
            

            if (startime != "")
            {
                condition += " and usetime>='" + startime + "'";
            }
            if (endtime != "")
            {
                endtime = DateTime.Parse(endtime).AddDays(1).ToString();
                condition += " and usetime<'" + endtime + "'";
            }

            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);

            List<wl_use_log_model> list = new List<wl_use_log_model>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new wl_use_log_model
                    {
                        id = reader.GetValue<int>("Id"),
                        wlorderid = reader.GetValue<string>("wlorderid"),
                        usedQuantity = reader.GetValue<int>("usedQuantity"),
                        comid = reader.GetValue<int>("comid"),
                        quantity = reader.GetValue<int>("quantity"),
                        usetime = reader.GetValue<DateTime>("usetime"),
                        orderid =  reader.GetValue<int>("orderid"),
                        partnerdealid= reader.GetValue<int>("proid")
                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }




        //订单支付成功后
        internal int UpdateWlOrderPaySC(wlOrderPayResponse m)
        {
            if (m.body.wlOrderId != "")
            {
                string sql = @"UPDATE  [wl_OrderCreate]
                               SET 
                             pay_code=@pay_code,
                             pay_describe=@pay_describe,
                             voucherType=@voucherType,
                             vouchers=@vouchers,
                             voucherPics=@voucherPics,
                             status=@status
                             WHERE wlorderid=@wlorderid";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);

                cmd.AddParam("@wlorderid", m.body.wlOrderId);
                cmd.AddParam("@pay_code", m.code);
                cmd.AddParam("@pay_describe", m.describe);
                cmd.AddParam("@voucherType", m.body.voucherType);


                string vouchers = "";

                if (m.body.vouchers != null) {
                    for (int i = 0; i < m.body.vouchers.Count(); i++) {
                        if (vouchers == "")
                        {
                            vouchers = m.body.vouchers[i];
                        }
                        else
                        {
                            vouchers = vouchers + "," + m.body.vouchers[i];
                        }
                    }
                }

                cmd.AddParam("@vouchers", vouchers);

                string voucherPics = "";

                if (m.body.voucherPics != null)
                {
                    for (int i = 0; i < m.body.voucherPics.Count(); i++)
                    {
                        if (voucherPics == "")
                        {
                            voucherPics = m.body.voucherPics[i];
                        }
                        else
                        {
                            voucherPics = voucherPics + "," + m.body.voucherPics[i];
                        }
                    }
                }

                cmd.AddParam("@voucherPics", voucherPics);

                cmd.AddParam("@status",4);
                cmd.ExecuteNonQuery();
                return 1;
            }
            else
            {
                return 0;
            }
        }


        //退款或关闭订单
        internal int UpdateWlOrderBack(wlOrderRefundResponse m, int orderStatus,wlOrderRefundRequest p)
        {
            if (m.body.partnerOrderId != "")
            {
                string sql = @"UPDATE  [wl_OrderCreate]
                               SET 
                             status=@status,
                             partnerRefundId=@partnerRefundId,
                             requestTime=@requestTime,
                             responeTime=@responeTime,
                             refundQuantity =@refundQuantity
                             WHERE orderid=@partnerOrderId and wlorderid=@wlorderid";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@partnerOrderId", int.Parse(p.body.partnerOrderId));
                cmd.AddParam("@wlorderid", m.body.wlOrderId);
                cmd.AddParam("@status", orderStatus);
                cmd.AddParam("@refundQuantity", p.body.refundQuantity);
                cmd.AddParam("@partnerRefundId", p.body.partnerRefundId);
                cmd.AddParam("@requestTime", m.body.requestTime);
                cmd.AddParam("@responeTime", m.body.responeTime);
                cmd.ExecuteNonQuery();
                return int.Parse(m.body.partnerOrderId);
            }
            else
            {
                return 0;
            }
        }


        //查询单笔订单，合作商订单号或者万龙订单号或者id
        internal wl_order_model SearchWlOrder(int comid,int id,string wlorderid,int orderid)
        {
            string sql = @"SELECT * FROM  wl_OrderCreate where comid =@comid ";
            if (id != 0) {
                sql += " and id=@id";
            }
            if (wlorderid != "" )
            {
                sql += " and wlorderid=@wlorderid";
            }
            if (orderid != 0)
            {
                sql += " and orderid=@orderid";
            }


            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@wlorderid", wlorderid);
            cmd.AddParam("@orderid", orderid);

            using (var reader = cmd.ExecuteReader())
            {
                wl_order_model m = null;
                if (reader.Read())
                {
                    m = new wl_order_model
                    {
                        id = reader.GetValue<int>("id"),
                        comid = reader.GetValue<int>("comid"),
                        subtime = reader.GetValue<DateTime>("subtime"),
                        orderid = reader.GetValue<int>("orderid"),
                        //partnerOrderId = reader.GetValue<int>("partnerOrderId"),
                        partnerId = reader.GetValue<string>("partnerId"),
                        name = reader.GetValue<string>("name"),
                        mobile = reader.GetValue<string>("mobile"),
                        wldealid = reader.GetValue<string>("wldealid"),
                        buyprice =Convert.ToDouble(reader.GetValue<decimal>("buyprice")),
                        unitprice =Convert.ToDouble(reader.GetValue<decimal>("unitprice")),
                        totalprice = Convert.ToDouble(reader.GetValue<decimal>("totalprice")),
                        quantity = reader.GetValue<int>("quantity"),
                        usedQuantity = reader.GetValue<int>("usedQuantity"),
                        refundedQuantity = reader.GetValue<int>("refundedQuantity"),
                        traveldate = reader.GetValue<string>("traveldate"),
                        wlorderid = reader.GetValue<string>("wlorderid"),
                        code = reader.GetValue<int>("code"),
                        describe = reader.GetValue<string>("describe"),
                        pay_code = reader.GetValue<int>("pay_code"),
                        pay_describe = reader.GetValue<string>("pay_describe"),
                        voucherType = reader.GetValue<int>("voucherType"),
                        vouchers = reader.GetValue<string>("vouchers"),
                        voucherPics = reader.GetValue<string>("voucherPics"),
                        status = reader.GetValue<int>("status"),
                        partnerRefundId = reader.GetValue<string>("partnerRefundId"),
                        refundId = reader.GetValue<string>("refundId"),
                        requestTime = reader.GetValue<string>("requestTime"),
                        responeTime = reader.GetValue<string>("responeTime")
                    };
                }
                return m;
            }
        }

        
            //查询单笔订单，合作商订单号或者万龙订单号或者id
        internal wl_order_model getWlOrderidData(string wlorderid)
        {
            string sql = @"SELECT * FROM  wl_OrderCreate where wlorderid=@wlorderid";


            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@wlorderid", wlorderid);


            using (var reader = cmd.ExecuteReader())
            {
                wl_order_model m = null;
                if (reader.Read())
                {
                    m = new wl_order_model
                    {
                        id = reader.GetValue<int>("id"),
                        comid = reader.GetValue<int>("comid"),
                        subtime = reader.GetValue<DateTime>("subtime"),
                        orderid = reader.GetValue<int>("orderid"),
                        //partnerOrderId = reader.GetValue<int>("partnerOrderId"),
                        partnerId = reader.GetValue<string>("partnerId"),
                        partnerdealid = reader.GetValue<int>("partnerdealid"),
                        name = reader.GetValue<string>("name"),
                        mobile = reader.GetValue<string>("mobile"),
                        wldealid = reader.GetValue<string>("wldealid"),
                        buyprice =Convert.ToDouble(reader.GetValue<decimal>("buyprice")),
                        unitprice =Convert.ToDouble(reader.GetValue<decimal>("unitprice")),
                        totalprice = Convert.ToDouble(reader.GetValue<decimal>("totalprice")),
                        quantity = reader.GetValue<int>("quantity"),
                        usedQuantity = reader.GetValue<int>("usedQuantity"),
                        refundedQuantity = reader.GetValue<int>("refundedQuantity"),
                        traveldate = reader.GetValue<string>("traveldate"),
                        wlorderid = reader.GetValue<string>("wlorderid"),
                        code = reader.GetValue<int>("code"),
                        describe = reader.GetValue<string>("describe"),
                        pay_code = reader.GetValue<int>("pay_code"),
                        pay_describe = reader.GetValue<string>("pay_describe"),
                        voucherType = reader.GetValue<int>("voucherType"),
                        vouchers = reader.GetValue<string>("vouchers"),
                        voucherPics = reader.GetValue<string>("voucherPics"),
                        status = reader.GetValue<int>("status"),
                        partnerRefundId = reader.GetValue<string>("partnerRefundId"),
                        refundId = reader.GetValue<string>("refundId"),
                        requestTime = reader.GetValue<string>("requestTime"),
                        responeTime = reader.GetValue<string>("responeTime")
                    };
                }
                return m;
            }
        }

        //查询所有库里产品
        internal List<WlDealResponseBody> SelectallgetWlProDeal(string proID, int comid, out int totalcount)
        {
            string sql = @"SELECT  *  FROM  WL_pro where    comid =@comid ";

            if (proID != "") {
                sql += " and  proID=@proID ";
            }
            sql +=" order by id desc";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@proID", proID);
            cmd.AddParam("@comid", comid);
            List<WlDealResponseBody> list = new List<WlDealResponseBody>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new WlDealResponseBody
                    {
                        id = reader.GetValue<int>("id"),
                        comid = reader.GetValue<int>("comid"),
                        subtime = reader.GetValue<DateTime>("subtime"),
                        useDateMode = reader.GetValue<int>("useDateMode"),
                        visitorInfoType = reader.GetValue<int>("visitorInfoType"),
                        proID = reader.GetValue<string>("proID"),
                        scheduleOnlineTime = reader.GetValue<string>("scheduleOnlineTime"),
                        scheduleOfflineTime = reader.GetValue<string>("scheduleOfflineTime"),
                        needTicket = reader.GetValue<bool>("needTicket"),
                        orderCancelTime = reader.GetValue<int>("orderCancelTime"),
                        stock = Decimal.ToInt64(reader.GetValue<decimal>("stock")),
                        marketPrice = Convert.ToDouble(reader.GetValue<decimal>("marketPrice")),
                        wlPrice = Convert.ToDouble(reader.GetValue<decimal>("wlPrice")),
                        settlementPrice = Convert.ToDouble(reader.GetValue<decimal>("settlementPrice")),
                        title = reader.GetValue<string>("title"),
                        subTitle = reader.GetValue<string>("subTitle"),
                        include = reader.GetValue<string>("include"),
                        exclude = reader.GetValue<string>("exclude"),
                        partnerId = reader.GetValue<int>("partnerId").ToString(),
                        voucherDateBegin = reader.GetValue<string>("voucherDateBegin"),
                        voucherDateEnd = reader.GetValue<string>("voucherDateEnd"),
                        stockMode = reader.GetValue<int>("stockMode"),
                        state = reader.GetValue<int>("state")
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }


        //查询单个万龙库里产品
        internal WlDealResponseBody SelectonegetWlProDeal(string wlDealId,int comid)
        {
            string sql = @"SELECT  *  FROM  WL_pro where comid =@comid ";
            sql += " and  proid=@wlDealId ";
            sql +=" order by id desc";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@wlDealId", wlDealId);
            cmd.AddParam("@comid", comid);
           WlDealResponseBody wlpro = new WlDealResponseBody();
            using (var reader = cmd.ExecuteReader())
            {

                if (reader.Read())
                {
                    wlpro = new WlDealResponseBody
                    {
                        id = reader.GetValue<int>("id"),
                        comid = reader.GetValue<int>("comid"),
                        subtime = reader.GetValue<DateTime>("subtime"),
                        useDateMode = reader.GetValue<int>("useDateMode"),
                        visitorInfoType = reader.GetValue<int>("visitorInfoType"),
                        proID = reader.GetValue<string>("proID"),
                        scheduleOnlineTime = reader.GetValue<string>("scheduleOnlineTime"),
                        scheduleOfflineTime = reader.GetValue<string>("scheduleOfflineTime"),
                        needTicket = reader.GetValue<bool>("needTicket"),
                        orderCancelTime = reader.GetValue<int>("orderCancelTime"),
                        stock = Decimal.ToInt64(reader.GetValue<decimal>("stock")),
                        marketPrice = Convert.ToDouble(reader.GetValue<decimal>("marketPrice")),
                        wlPrice = Convert.ToDouble(reader.GetValue<decimal>("wlPrice")),
                        settlementPrice = Convert.ToDouble(reader.GetValue<decimal>("settlementPrice")),
                        title = reader.GetValue<string>("title"),
                        subTitle = reader.GetValue<string>("subTitle"),
                        include = reader.GetValue<string>("include"),
                        exclude = reader.GetValue<string>("exclude"),
                        partnerId = reader.GetValue<int>("partnerId").ToString(),
                        voucherDateBegin = reader.GetValue<string>("voucherDateBegin"),
                        voucherDateEnd = reader.GetValue<string>("voucherDateEnd"),
                        stockMode = reader.GetValue<int>("stockMode"),
                        state = reader.GetValue<int>("state")
                    };

                }
            }
            
            return wlpro;
        }




        internal List<WlDealResponseBody> WLProPageList(string comid, int pageindex, int pagesize, int prostate, out int totalcount, string key = "")
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var tblName = "WL_pro";
            var strGetFields = "*";
            var sortKey = "id desc";
            //var sortMode = "0";
            var condition = "comid=" + comid;


            if (prostate == 2)//读取上线产品
            {
                condition += " and state = 1";
            }
            if (prostate == 3)//读取下线产品
            {
                condition += " and state = 0";
            }


            if (key != "")
            {
                    condition += " and title like '%" + key + "%'";
            }
           

            //cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);
            cmd.PagingCommand1(tblName, strGetFields, sortKey, "", pagesize, pageindex, "0", condition);

            List<WlDealResponseBody> list = new List<WlDealResponseBody>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new WlDealResponseBody
                    {
                        id = reader.GetValue<int>("id"),
                        comid = reader.GetValue<int>("comid"),
                        subtime = reader.GetValue<DateTime>("subtime"),
                        useDateMode = reader.GetValue<int>("useDateMode"),
                        visitorInfoType = reader.GetValue<int>("visitorInfoType"),
                        proID = reader.GetValue<string>("proID"),
                        scheduleOnlineTime = reader.GetValue<string>("scheduleOnlineTime"),
                        scheduleOfflineTime = reader.GetValue<string>("scheduleOfflineTime"),
                        needTicket = reader.GetValue<bool>("needTicket"),
                        orderCancelTime = reader.GetValue<int>("orderCancelTime"),
                        stock = Decimal.ToInt64(reader.GetValue<decimal>("stock")),
                        marketPrice = Convert.ToDouble(reader.GetValue<decimal>("marketPrice")),
                        wlPrice = Convert.ToDouble(reader.GetValue<decimal>("wlPrice")),
                        settlementPrice = Convert.ToDouble(reader.GetValue<decimal>("settlementPrice")),
                        title = reader.GetValue<string>("title"),
                        subTitle = reader.GetValue<string>("subTitle"),
                        include = reader.GetValue<string>("include"),
                        exclude = reader.GetValue<string>("exclude"),
                        partnerId = reader.GetValue<int>("partnerId").ToString(),
                        voucherDateBegin = reader.GetValue<string>("voucherDateBegin"),
                        voucherDateEnd = reader.GetValue<string>("voucherDateEnd"),
                        stockMode = reader.GetValue<int>("stockMode"),
                        state = reader.GetValue<int>("state")
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }

    }
}
