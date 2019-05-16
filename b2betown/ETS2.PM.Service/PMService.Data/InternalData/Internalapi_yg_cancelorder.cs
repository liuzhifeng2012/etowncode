using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internalapi_yg_cancelorder
    {
        public SqlHelper sqlHelper;
        public Internalapi_yg_cancelorder(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int EditApi_yg_cancelorder(Api_yg_cancelorder m)
        {
            if (m.id > 0)
            {
                string sql = @"UPDATE [api_yg_cancelorder]
                               SET [organization] = @organization
                                  ,[password] = @password
                                  ,[req_seq] = @req_seq
                                  ,[ygorder_num] = @ygorder_num
                                  ,[num] = @num
                                  ,[rResultid] = @rResultid
                                  ,[rResultComment] = @rResultComment
                                  ,[rygorder_num] = @rygorder_num
                                  ,[rnum] = @rnum
                                  ,[orderId] = @orderId
                                  ,[opertime] = @opertime
                             WHERE  id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id", m.id);
                cmd.AddParam("@organization", m.organization);
                cmd.AddParam("@password", m.password);
                cmd.AddParam("@req_seq", m.req_seq);
                cmd.AddParam("@ygorder_num", m.ygorder_num);
                cmd.AddParam("@num", m.num);
                cmd.AddParam("@rResultid", m.rResultid);
                cmd.AddParam("@rResultComment", m.rResultComment);
                cmd.AddParam("@rygorder_num", m.rygorder_num);
                cmd.AddParam("@rnum", m.rnum);
                cmd.AddParam("@orderId", m.orderId);
                cmd.AddParam("@opertime", m.opertime);

                cmd.ExecuteNonQuery();
                return m.id;
            }
            else
            {
                string sql = @"INSERT  [api_yg_cancelorder]
                                   ([organization]
                                   ,[password]
                                   ,[req_seq]
                                   ,[ygorder_num]
                                   ,[num]
                                   ,[rResultid]
                                   ,[rResultComment]
                                   ,[rygorder_num]
                                   ,[rnum]
                                   ,[orderId]
                                   ,[opertime])
                             VALUES
                                   (@organization 
                                   ,@password 
                                   ,@req_seq 
                                   ,@ygorder_num 
                                   ,@num 
                                   ,@rResultid 
                                   ,@rResultComment 
                                   ,@rygorder_num 
                                   ,@rnum 
                                   ,@orderId 
                                   ,@opertime);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@organization", m.organization);
                cmd.AddParam("@password", m.password);
                cmd.AddParam("@req_seq", m.req_seq);
                cmd.AddParam("@ygorder_num", m.ygorder_num);
                cmd.AddParam("@num", m.num);
                cmd.AddParam("@rResultid", m.rResultid);
                cmd.AddParam("@rResultComment", m.rResultComment);
                cmd.AddParam("@rygorder_num", m.rygorder_num);
                cmd.AddParam("@rnum", m.rnum);
                cmd.AddParam("@orderId", m.orderId);
                cmd.AddParam("@opertime", m.opertime);

                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }

        }
    }
}
