using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Taobao_Ms.Model;

namespace ETS2.PM.Service.Taobao_Ms.Data.Internal
{
    public class InternalTaobao_order_modify_noticelog
    {
        public SqlHelper sqlHelper;
        public InternalTaobao_order_modify_noticelog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }


        internal int Editnoticelog(Taobao_order_modify_noticelog log)
        {
            if (log.id == 0)
            {
                string sql = @"INSERT INTO  [taobao_order_modify_noticelog]
           ([timestamp]
           ,[sign]
           ,[order_id]
      
           ,[method]
           ,[taobao_sid]
           ,[seller_nick]
           ,sub_method
,data
          
           ,[subtime]
           ,[responsecode]
           ,[responsetime]
           ,[self_order_id]
           ,[agentid]
           ,[errmsg])
     VALUES
           (@timestamp 
           ,@sign 
           ,@order_id 
           
           ,@method 
           ,@taobao_sid 
           ,@seller_nick 
            ,@sub_method
,@data
            
           ,@subtime 
           ,@responsecode 
           ,@responsetime 
           ,@self_order_id 
           ,@agentid 
           ,@errmsg );select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@timestamp", log.timestamp);
                cmd.AddParam("@sign", log.sign);
                cmd.AddParam("@order_id", log.order_id);

                cmd.AddParam("@method", log.method);
                cmd.AddParam("@taobao_sid", log.taobao_sid);
                cmd.AddParam("@seller_nick", log.seller_nick);
                cmd.AddParam("@sub_method", log.sub_method);
                cmd.AddParam("@data", log.data);


                cmd.AddParam("@subtime", log.subtime);
                cmd.AddParam("@responsecode", log.responsecode);
                cmd.AddParam("@responsetime", log.responsetime);
                cmd.AddParam("@self_order_id", log.self_order_id);
                cmd.AddParam("@agentid", log.agentid);
                cmd.AddParam("@errmsg", log.errmsg);
                try
                {
                    object o = cmd.ExecuteScalar();
                    return int.Parse(o.ToString());
                }
                catch
                {
                    sqlHelper.Dispose();
                    return 0;
                }
            }
            else
            {
                string sql = @"UPDATE  [taobao_order_modify_noticelog]
   SET [timestamp] = @timestamp 
      ,[sign] = @sign 
      ,[order_id] = @order_id 
  
      ,[method] = @method 
      ,[taobao_sid] = @taobao_sid 
      ,[seller_nick] = @seller_nick 
      
      ,[data] = @data 
      ,[sub_method] = @sub_method 
 
      ,[subtime] = @subtime 
      ,[responsecode] = @responsecode 
      ,[responsetime] = @responsetime 
      ,[self_order_id] = @self_order_id 
      ,[agentid] = @agentid 
      ,[errmsg] = @errmsg 
 WHERE id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id", log.id);
                cmd.AddParam("@timestamp", log.timestamp);
                cmd.AddParam("@sign", log.sign);
                cmd.AddParam("@order_id", log.order_id);

                cmd.AddParam("@method", log.method);
                cmd.AddParam("@taobao_sid", log.taobao_sid);
                cmd.AddParam("@seller_nick", log.seller_nick);


                cmd.AddParam("@sub_method", log.sub_method);
                cmd.AddParam("@data", log.data);

                cmd.AddParam("@subtime", log.subtime);
                cmd.AddParam("@responsecode", log.responsecode);
                cmd.AddParam("@responsetime", log.responsetime);
                cmd.AddParam("@self_order_id", log.self_order_id);
                cmd.AddParam("@agentid", log.agentid);
                cmd.AddParam("@errmsg", log.errmsg);
                cmd.ExecuteNonQuery();
                return log.id;
            }
        }

        internal int  GetNoticeNum(string taobao_orderid, string method)
        {
            string sql = "select count(1) from taobao_order_modify_noticelog where order_id='" + taobao_orderid + "' and method='" + method + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            return int.Parse(o.ToString());
        }
    }
}
