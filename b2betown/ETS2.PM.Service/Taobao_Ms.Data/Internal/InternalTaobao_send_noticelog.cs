using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Taobao_Ms.Model;

namespace ETS2.PM.Service.Taobao_Ms.Data.Internal
{
    public class InternalTaobao_send_noticelog
    {
        public SqlHelper sqlHelper;
        public InternalTaobao_send_noticelog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int Editsendnoticelog(Taobao_send_noticelog log)
        {
            if (log.id == 0)
            {
                string sql = @"INSERT INTO  [taobao_send_noticelog]
           ([timestamp]
           ,[sign]
           ,[order_id]
           ,[mobile]
           ,[num]
           ,[method]
           ,[taobao_sid]
           ,[seller_nick]
           ,[item_title]
           ,[send_type]
           ,[consume_type]
           ,[sms_template]
           ,[valid_start]
           ,[valid_ends]
           ,[num_iid]
           ,[outer_iid]
           ,[sub_outer_iid]
           ,[sku_properties]
           ,[token]
           ,[total_fee]
           ,[weeks]
           ,[subtime]
           ,[responsecode]
           ,[responsetime]
           ,[self_order_id]
           ,[agentid]
           ,[errmsg]
           ,type
           ,encrypt_mobile
           ,md5_mobile)
     VALUES
           (@timestamp 
           ,@sign 
           ,@order_id 
           ,@mobile 
           ,@num
           ,@method 
           ,@taobao_sid 
           ,@seller_nick 
           ,@item_title 
           ,@send_type 
           ,@consume_type 
           ,@sms_template 
           ,@valid_start 
           ,@valid_ends 
           ,@num_iid 
           ,@outer_iid 
           ,@sub_outer_iid 
           ,@sku_properties 
           ,@token 
           ,@total_fee 
           ,@weeks
           ,@subtime 
           ,@responsecode 
           ,@responsetime 
           ,@self_order_id 
           ,@agentid 
           ,@errmsg
           ,@type
           ,@encrypt_mobile
           ,@md5_mobile
          );select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@timestamp", log.timestamp);
                cmd.AddParam("@sign", log.sign);
                cmd.AddParam("@order_id", log.order_id);
                cmd.AddParam("@mobile", log.mobile);
                cmd.AddParam("@num", log.num);
                cmd.AddParam("@method", log.method);
                cmd.AddParam("@taobao_sid", log.taobao_sid);
                cmd.AddParam("@seller_nick", log.seller_nick);
                cmd.AddParam("@item_title", log.item_title);
                cmd.AddParam("@send_type", log.send_type);
                cmd.AddParam("@consume_type", log.consume_type);
                cmd.AddParam("@sms_template", log.sms_template);
                cmd.AddParam("@valid_start", log.valid_start);
                cmd.AddParam("@valid_ends", log.valid_ends);
                cmd.AddParam("@num_iid", log.num_iid);
                cmd.AddParam("@outer_iid", log.outer_iid);
                cmd.AddParam("@sub_outer_iid", log.sub_outer_iid);
                cmd.AddParam("@sku_properties", log.sku_properties);
                cmd.AddParam("@token", log.token);
                cmd.AddParam("@total_fee", log.total_fee);
                cmd.AddParam("@weeks", log.weeks);
                cmd.AddParam("@subtime", log.subtime);
                cmd.AddParam("@responsecode", log.responsecode);
                cmd.AddParam("@responsetime", log.responsetime);
                cmd.AddParam("@self_order_id", log.self_order_id);
                cmd.AddParam("@agentid", log.agentid);
                cmd.AddParam("@errmsg", log.errmsg);

                cmd.AddParam("@type", log.type);
                cmd.AddParam("@encrypt_mobile", log.encrypt_mobile);
                cmd.AddParam("@md5_mobile", log.md5_mobile);
                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());

            }
            else
            {
                string sql = @"UPDATE  [taobao_send_noticelog]
   SET [timestamp] = @timestamp 
      ,[sign] = @sign 
      ,[order_id] = @order_id 
      ,[mobile] = @mobile 
      ,[num] = @num 
      ,[method] = @method 
      ,[taobao_sid] = @taobao_sid 
      ,[seller_nick] = @seller_nick 
      ,[item_title] = @item_title 
      ,[send_type] = @send_type 
      ,[consume_type] = @consume_type 
      ,[sms_template] = @sms_template 
      ,[valid_start] = @valid_start 
      ,[valid_ends] = @valid_ends 
      ,[num_iid] = @num_iid 
      ,[outer_iid] = @outer_iid 
      ,[sub_outer_iid] = @sub_outer_iid 
      ,[sku_properties] = @sku_properties 
      ,[token] = @token 
      ,[total_fee] = @total_fee 
      ,[weeks] = @weeks 
      ,[subtime] = @subtime 
      ,[responsecode] = @responsecode 
      ,[responsetime] = @responsetime 
      ,[self_order_id] = @self_order_id 
      ,[agentid] = @agentid 
      ,[errmsg] = @errmsg
      ,type=@type
      ,encrypt_mobile=@encrypt_mobile
      ,md5_mobile=@md5_mobile
 WHERE id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id", log.id);
                cmd.AddParam("@timestamp", log.timestamp);
                cmd.AddParam("@sign", log.sign);
                cmd.AddParam("@order_id", log.order_id);
                cmd.AddParam("@mobile", log.mobile);
                cmd.AddParam("@num", log.num);
                cmd.AddParam("@method", log.method);
                cmd.AddParam("@taobao_sid", log.taobao_sid);
                cmd.AddParam("@seller_nick", log.seller_nick);
                cmd.AddParam("@item_title", log.item_title);
                cmd.AddParam("@send_type", log.send_type);
                cmd.AddParam("@consume_type", log.consume_type);
                cmd.AddParam("@sms_template", log.sms_template);
                cmd.AddParam("@valid_start", log.valid_start);
                cmd.AddParam("@valid_ends", log.valid_ends);
                cmd.AddParam("@num_iid", log.num_iid);
                cmd.AddParam("@outer_iid", log.outer_iid);
                cmd.AddParam("@sub_outer_iid", log.sub_outer_iid);
                cmd.AddParam("@sku_properties", log.sku_properties);
                cmd.AddParam("@token", log.token);
                cmd.AddParam("@total_fee", log.total_fee);
                cmd.AddParam("@weeks", log.weeks);
                cmd.AddParam("@subtime", log.subtime);
                cmd.AddParam("@responsecode", log.responsecode);
                cmd.AddParam("@responsetime", log.responsetime);
                cmd.AddParam("@self_order_id", log.self_order_id);
                cmd.AddParam("@agentid", log.agentid);
                cmd.AddParam("@errmsg", log.errmsg);
                cmd.AddParam("@type", log.type);
                cmd.AddParam("@encrypt_mobile", log.encrypt_mobile);
                cmd.AddParam("@md5_mobile", log.md5_mobile);
                cmd.ExecuteNonQuery();
                return log.id;
            }
        }

        //internal int GetSendNoticeNum(string taobao_orderid, string method)
        //{
        //    string sql = "select count(1) from taobao_send_noticelog where order_id='" + taobao_orderid + "' and method='" + method + "'";
        //    var cmd = sqlHelper.PrepareTextSqlCommand(sql);
        //    object o = cmd.ExecuteScalar();
        //    return int.Parse(o.ToString());
        //}
        internal int GetSendNoticeNum(string token)
        {
            string sql = "select count(1) from taobao_send_noticelog where token='" + token + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            return int.Parse(o.ToString());
        }

        internal string GetSysOidByTaobaoOid(string tboid)
        {
            string sql = "select top 1  self_order_id from taobao_send_noticelog where order_id='" + tboid + "' and errmsg='' order by id desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                string sysoid = "";
                if (reader.Read())
                {
                    sysoid = reader.GetValue<int>("self_order_id").ToString();
                }
                return sysoid;
            }
        }

        internal Taobao_send_noticelog GetSendNoticeLogByTbOid(string orderid)
        {
            string sql = @"SELECT [id]
      ,[timestamp]
      ,[sign]
      ,[order_id]
      ,[mobile]
      ,[num]
      ,[method]
      ,[taobao_sid]
      ,[seller_nick]
      ,[item_title]
      ,[send_type]
      ,[consume_type]
      ,[sms_template]
      ,[valid_start]
      ,[valid_ends]
      ,[num_iid]
      ,[outer_iid]
      ,[sub_outer_iid]
      ,[sku_properties]
      ,[token]
      ,[total_fee]
      ,[weeks]
      ,[subtime]
      ,[responsecode]
      ,[responsetime]
      ,[self_order_id]
      ,[agentid]
      ,[errmsg]
      ,type
      ,encrypt_mobile
      ,md5_mobile
  FROM [EtownDB].[dbo].[taobao_send_noticelog] where order_id=@order_id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@order_id", orderid);
            using (var reader = cmd.ExecuteReader())
            {
                Taobao_send_noticelog log = null;
                if (reader.Read())
                {
                    log = new Taobao_send_noticelog
                    {
                        id = reader.GetValue<int>("id")
      ,
                        timestamp = reader.GetValue<string>("timestamp")
      ,
                        sign = reader.GetValue<string>("sign")
      ,
                        order_id = reader.GetValue<string>("order_id")
      ,
                        mobile = reader.GetValue<string>("mobile")
      ,
                        num = reader.GetValue<int>("num")
      ,
                        method = reader.GetValue<string>("method")
      ,
                        taobao_sid = reader.GetValue<string>("taobao_sid")
      ,
                        seller_nick = reader.GetValue<string>("seller_nick")
      ,
                        item_title = reader.GetValue<string>("item_title")
      ,
                        send_type = reader.GetValue<int>("send_type")
      ,
                        consume_type = reader.GetValue<int>("consume_type")
      ,
                        sms_template = reader.GetValue<string>("sms_template")
      ,
                        valid_start = reader.GetValue<DateTime>("valid_start")
      ,
                        valid_ends = reader.GetValue<DateTime>("valid_ends")
      ,
                        num_iid = reader.GetValue<string>("num_iid")
      ,
                        outer_iid = reader.GetValue<string>("outer_iid")
      ,
                        sub_outer_iid = reader.GetValue<string>("sub_outer_iid")
      ,
                        sku_properties = reader.GetValue<string>("sku_properties")
      ,
                        token = reader.GetValue<string>("token")
      ,
                        total_fee = reader.GetValue<decimal>("total_fee")
      ,
                        weeks = reader.GetValue<string>("weeks")
      ,
                        subtime = reader.GetValue<DateTime>("subtime")
      ,
                        responsecode = reader.GetValue<string>("responsecode")
      ,
                        responsetime = reader.GetValue<DateTime>("responsetime")
      ,
                        self_order_id = reader.GetValue<int>("self_order_id")
      ,
                        agentid = reader.GetValue<int>("agentid")
      ,
                        errmsg = reader.GetValue<string>("errmsg")
                         ,
                        type = reader.GetValue<int>("type")
                         ,
                        encrypt_mobile = reader.GetValue<string>("encrypt_mobile")
                         ,
                        md5_mobile = reader.GetValue<string>("md5_mobile")
                    };
                }
                return log;
            }
        }

        internal Taobao_send_noticelog GetSendNoticeBySelfOrderid(string selforderid)
        {
            string sql = @"SELECT [id]
      ,[timestamp]
      ,[sign]
      ,[order_id]
      ,[mobile]
      ,[num]
      ,[method]
      ,[taobao_sid]
      ,[seller_nick]
      ,[item_title]
      ,[send_type]
      ,[consume_type]
      ,[sms_template]
      ,[valid_start]
      ,[valid_ends]
      ,[num_iid]
      ,[outer_iid]
      ,[sub_outer_iid]
      ,[sku_properties]
      ,[token]
      ,[total_fee]
      ,[weeks]
      ,[subtime]
      ,[responsecode]
      ,[responsetime]
      ,[self_order_id]
      ,[agentid]
      ,[errmsg]
      ,type
      ,encrypt_mobile
      ,md5_mobile
  FROM  [taobao_send_noticelog] where self_order_id=@self_order_id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@self_order_id", selforderid);
            using (var reader = cmd.ExecuteReader())
            {
                Taobao_send_noticelog log = null;
                if (reader.Read())
                {
                    log = new Taobao_send_noticelog
                    {
                        id = reader.GetValue<int>("id")  ,
                        timestamp = reader.GetValue<string>("timestamp"),
                        sign = reader.GetValue<string>("sign"),
                        order_id = reader.GetValue<string>("order_id"),
                        mobile = reader.GetValue<string>("mobile"),
                        num = reader.GetValue<int>("num"),
                        method = reader.GetValue<string>("method") ,
                        taobao_sid = reader.GetValue<string>("taobao_sid"),
                        seller_nick = reader.GetValue<string>("seller_nick"),
                        item_title = reader.GetValue<string>("item_title"),
                        send_type = reader.GetValue<int>("send_type"),
                        consume_type = reader.GetValue<int>("consume_type"),
                        sms_template = reader.GetValue<string>("sms_template"),
                        valid_start = reader.GetValue<DateTime>("valid_start"),
                        valid_ends = reader.GetValue<DateTime>("valid_ends"),
                        num_iid = reader.GetValue<string>("num_iid"),
                        outer_iid = reader.GetValue<string>("outer_iid"),
                        sub_outer_iid = reader.GetValue<string>("sub_outer_iid"),
                        sku_properties = reader.GetValue<string>("sku_properties"),
                        token = reader.GetValue<string>("token"),
                        total_fee = reader.GetValue<decimal>("total_fee"),
                        weeks = reader.GetValue<string>("weeks"),
                        subtime = reader.GetValue<DateTime>("subtime"),
                        responsecode = reader.GetValue<string>("responsecode"),
                        responsetime = reader.GetValue<DateTime>("responsetime"),
                        self_order_id = reader.GetValue<int>("self_order_id"),
                        agentid = reader.GetValue<int>("agentid"),
                        errmsg = reader.GetValue<string>("errmsg"),
                        type = reader.GetValue<int>("type"),
                        encrypt_mobile = reader.GetValue<string>("encrypt_mobile"),
                        md5_mobile = reader.GetValue<string>("md5_mobile")
                    };
                }
                return log;
            }
        }

        internal Taobao_send_noticelog GetSendNoticeLogByQrcode(string qrcode)
        {
            string sql = @"select top 1 token from taobao_send_noticelog where order_id=(select top 1 order_id  from taobao_send_noticeretlog where verify_codes like '%"+qrcode+"%' order by id desc) order by id desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql); 
            using (var reader = cmd.ExecuteReader())
            {
                Taobao_send_noticelog log = null;
                if (reader.Read())
                {
                    log = new Taobao_send_noticelog
                    {
                        id = reader.GetValue<int>("id"),
                        timestamp = reader.GetValue<string>("timestamp"),
                        sign = reader.GetValue<string>("sign"),
                        order_id = reader.GetValue<string>("order_id"),
                        mobile = reader.GetValue<string>("mobile"),
                        num = reader.GetValue<int>("num"),
                        method = reader.GetValue<string>("method"),
                        taobao_sid = reader.GetValue<string>("taobao_sid"),
                        seller_nick = reader.GetValue<string>("seller_nick"),
                        item_title = reader.GetValue<string>("item_title"),
                        send_type = reader.GetValue<int>("send_type"),
                        consume_type = reader.GetValue<int>("consume_type"),
                        sms_template = reader.GetValue<string>("sms_template"),
                        valid_start = reader.GetValue<DateTime>("valid_start"),
                        valid_ends = reader.GetValue<DateTime>("valid_ends"),
                        num_iid = reader.GetValue<string>("num_iid"),
                        outer_iid = reader.GetValue<string>("outer_iid"),
                        sub_outer_iid = reader.GetValue<string>("sub_outer_iid"),
                        sku_properties = reader.GetValue<string>("sku_properties"),
                        token = reader.GetValue<string>("token"),
                        total_fee = reader.GetValue<decimal>("total_fee"),
                        weeks = reader.GetValue<string>("weeks"),
                        subtime = reader.GetValue<DateTime>("subtime"),
                        responsecode = reader.GetValue<string>("responsecode"),
                        responsetime = reader.GetValue<DateTime>("responsetime"),
                        self_order_id = reader.GetValue<int>("self_order_id"),
                        agentid = reader.GetValue<int>("agentid"),
                        errmsg = reader.GetValue<string>("errmsg"),
                        type = reader.GetValue<int>("type"),
                        encrypt_mobile = reader.GetValue<string>("encrypt_mobile"),
                        md5_mobile = reader.GetValue<string>("md5_mobile")
                    };
                }
                return log;
            }
        }
    }
}
