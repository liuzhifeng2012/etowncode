using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internalapi_hzins_orderCancel
    {
        public SqlHelper sqlHelper;
        public Internalapi_hzins_orderCancel(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int EditApi_hzins_orderCancel(Api_hzins_orderCancel m)
        {
            //暂时阶段只是有录入操作，没有编辑操作
            if (m.id == 0)
            {
                string sql = @"INSERT INTO  [api_hzins_orderCancel]
                                    ([orderid]
                                    ,[insureNo]
                                    ,[respCode]
                                    ,[respMsg])
                                VALUES
                                    (@orderid 
                                    ,@insureNo 
                                    ,@respCode 
                                    ,@respMsg);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@orderid",m.orderid);
                cmd.AddParam("@insureNo",m.insureNo);
                cmd.AddParam("@respCode", m.respCode);
                cmd.AddParam("@respMsg", m.respMsg);

                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
            else 
            {
                return 0;
            }
            
        }
    }
}
