using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internalapi_hzins_OrderApplyReq_applicantInfo
    {
        public SqlHelper sqlHelper;
        public Internalapi_hzins_OrderApplyReq_applicantInfo(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int EditOrderApplyReq_applicantInfo(Api_hzins_OrderApplyReq_applicantInfo m)
        {
            //暂时保险请求只有录入操作，没有编辑操作
            if (m.id > 0)
            {
                return 0;
            }
            else
            {
                string sql = @"INSERT INTO  [api_hzins_OrderApplyReq_applicantInfo]
                                   ([cName]
                                   ,[eName]
                                   ,[cardType]
                                   ,[cardCode]
                                   ,[sex]
                                   ,[birthday]
                                   ,[mobile]
                                   ,[email]
                                   ,[jobInfo]
                                   ,[orderid])
                             VALUES
                                   (@cName 
                                   ,@eName 
                                   ,@cardType 
                                   ,@cardCode 
                                   ,@sex 
                                   ,@birthday 
                                   ,@mobile 
                                   ,@email 
                                   ,@jobInfo 
                                   ,@orderid);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@cName", m.cName);
                cmd.AddParam("@eName", m.eName);
                cmd.AddParam("@cardType", m.cardType);
                cmd.AddParam("@cardCode", m.cardCode);
                cmd.AddParam("@sex", m.sex);
                cmd.AddParam("@birthday", m.birthday);
                cmd.AddParam("@mobile", m.mobile);
                cmd.AddParam("@email", m.email);
                cmd.AddParam("@jobInfo", m.jobInfo);
                cmd.AddParam("@orderid", m.orderid);

                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
        }

        internal Api_hzins_OrderApplyReq_applicantInfo GetOrderApplyReq_applicantInfo(int orderid)
        {
            string sql = "select * from api_hzins_OrderApplyReq_applicantInfo where orderid=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                Api_hzins_OrderApplyReq_applicantInfo m = null;
                if (reader.Read())
                {
                    m = new Api_hzins_OrderApplyReq_applicantInfo
                    {
                        id = reader.GetValue<int>("id"),
                        cName = reader.GetValue<string>("cName"),
                        eName = reader.GetValue<string>("eName"),
                        cardType = reader.GetValue<int>("cardType"),
                        cardCode = reader.GetValue<string>("cardCode"),
                        sex = reader.GetValue<int>("sex"),
                        birthday = reader.GetValue<string>("birthday"),
                        mobile = reader.GetValue<string>("mobile"),
                        email = reader.GetValue<string>("email"),
                        jobInfo = reader.GetValue<string>("jobInfo"),
                        orderid = reader.GetValue<int>("orderid"),
                    };
                }
                return m;
            }
        }
    }
}
