using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Qunar_Ms.QMRequestDataSchema;

namespace ETS2.PM.Service.Qunar_Ms.Data.Internal
{
    public class InternalQunar_CreateOrderForBeforePaySync
    {
        public SqlHelper sqlHelper;
        public InternalQunar_CreateOrderForBeforePaySync(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int InsQunar_CreateOrderForBeforePaySync(CreateOrderForBeforePaySyncRequestBody createOrderForBeforePaySyncRequestBody)
        {
            CreateOrderForBeforePaySyncRequestBodyorderInfo createOrderForBeforePaySyncRequestBodyorderInfo = createOrderForBeforePaySyncRequestBody.orderInfo;
            string qunar_orderId = createOrderForBeforePaySyncRequestBodyorderInfo.orderId;
            string orderQuantity = createOrderForBeforePaySyncRequestBodyorderInfo.orderQuantity;
            string orderPrice = createOrderForBeforePaySyncRequestBodyorderInfo.orderPrice;//总价
            string orderCashBackMoney = createOrderForBeforePaySyncRequestBodyorderInfo.orderCashBackMoney;
            string orderStatus = createOrderForBeforePaySyncRequestBodyorderInfo.orderStatus;
            string orderRemark = createOrderForBeforePaySyncRequestBodyorderInfo.orderRemark;
            string orderSource = createOrderForBeforePaySyncRequestBodyorderInfo.orderSource;
            string eticketNo = createOrderForBeforePaySyncRequestBodyorderInfo.eticketNo;
            if (eticketNo == null)
            {
                eticketNo = "";
            }

            CreateOrderForBeforePaySyncRequestBodyorderInfoproduct mproduct = createOrderForBeforePaySyncRequestBodyorderInfo.product;
            string resourceId = mproduct.resourceId;
            string productName = mproduct.productName;
            string visitDate = mproduct.visitDate;
            string sellPrice = mproduct.sellPrice;//单价
            string cashBackMoney = mproduct.cashBackMoney;

            CreateOrderForBeforePaySyncRequestBodyorderInfocontactPerson mcontactPerson = createOrderForBeforePaySyncRequestBodyorderInfo.contactPerson;
            string name = mcontactPerson.name;
            string namePinyin = mcontactPerson.namePinyin;
            string mobile = mcontactPerson.mobile;
            string email = mcontactPerson.email;
            string address = mcontactPerson.address;
            string zipCode = mcontactPerson.zipCode;
            try
            {
                string sql = @"INSERT INTO  [qunar_CreateOrderForBeforePaySync]
           ([qunar_orderId]
           ,[resourceId]
           ,[productName]
           ,[visitDate]
           ,[sellPrice]
           ,[cashBackMoney]
           ,[name]
           ,[namePinyin]
           ,[mobile]
           ,[email]
           ,[address]
           ,[zipCode]
           ,[orderQuantity]
           ,[orderPrice]
           ,[orderCashBackMoney]
           ,[orderStatus]
           ,[orderRemark]
           ,[orderSource]
           ,[eticketNo]
           ,[parterorderid]
           ,[orderStatus_ret]
           ,[eticketNo_ret]
           ,qunar_ret)
     VALUES
           (@qunar_orderId 
           ,@resourceId 
           ,@productName 
           ,@visitDate 
           ,@sellPrice 
           ,@cashBackMoney 
           ,@name 
           ,@namePinyin 
           ,@mobile 
           ,@email 
           ,@address 
           ,@zipCode 
           ,@orderQuantity 
           ,@orderPrice 
           ,@orderCashBackMoney 
           ,@orderStatus 
           ,@orderRemark 
           ,@orderSource 
           ,@eticketNo 
           ,@parterorderid 
           ,@orderStatus_ret 
           ,@eticketNo_ret,@qunar_ret);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@qunar_orderId", qunar_orderId);
                cmd.AddParam("@resourceId", resourceId);
                cmd.AddParam("@productName", productName);
                cmd.AddParam("@visitDate", visitDate);
                cmd.AddParam("@sellPrice", sellPrice);
                cmd.AddParam("@cashBackMoney", cashBackMoney);
                cmd.AddParam("@name", name);
                cmd.AddParam("@namePinyin", namePinyin);
                cmd.AddParam("@mobile", mobile);
                cmd.AddParam("@email", email);
                cmd.AddParam("@address", address);
                cmd.AddParam("@zipCode", zipCode);
                cmd.AddParam("@orderQuantity", orderQuantity);
                cmd.AddParam("@orderPrice", orderPrice);
                cmd.AddParam("@orderCashBackMoney", orderCashBackMoney);
                cmd.AddParam("@orderStatus", orderStatus);
                cmd.AddParam("@orderRemark", orderRemark);
                cmd.AddParam("@orderSource", orderSource);
                cmd.AddParam("@eticketNo", eticketNo);
                cmd.AddParam("@parterorderid", 0);
                cmd.AddParam("@orderStatus_ret", "");
                cmd.AddParam("@eticketNo_ret", "");
                cmd.AddParam("@qunar_ret", "");

                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
            catch
            {
                return 0;
            }
        }

        internal int InsQunar_CreateOrderForBeforePaySync_ret(string qunar_orderId, int parterorderid, string orderStatus, string eticketNo, string qunar_ret)
        {
            try
            {
                string sql = "update qunar_CreateOrderForBeforePaySync set parterorderid=@parterorderid,orderStatus_ret=@orderStatus_ret,eticketNo_ret=@eticketNo_ret,qunar_ret=@qunar_ret where qunar_orderId=@qunar_orderId";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@qunar_orderId", qunar_orderId);
                cmd.AddParam("@parterorderid", parterorderid);
                cmd.AddParam("@orderStatus_ret", orderStatus);
                cmd.AddParam("@eticketNo_ret", eticketNo);
                cmd.AddParam("@qunar_ret", qunar_ret);

                return cmd.ExecuteNonQuery();
            }
            catch 
            {
                return 0;
            }
        }

        internal bool Ishasrequest(string qunar_orderId)
        {
            string sql = "select count(1) from qunar_CreateOrderForBeforePaySync where qunar_orderId='" + qunar_orderId + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            if (int.Parse(o.ToString()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal string GetQunarOrderId(string partnerOrderId)
        {
            string sql = "select  qunar_orderId from  qunar_CreateOrderForBeforePaySync where parterorderid=" + partnerOrderId;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                string qunar_orderid = "";
                if (reader.Read())
                {
                    qunar_orderid = reader.GetValue<string>("qunar_orderId");
                }
                return qunar_orderid;
            }

        }
    }
}
