using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Model;

namespace ETS2.WeiXin.Service.WeiXinService.Data.InternalData
{
    public class InternalWxOperationType
    {
        private SqlHelper sqlHelper;
        public InternalWxOperationType(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }


        internal WxOperationType GetOprationType(int typeid)
        {
            string sql = @"SELECT   [id]
      ,[typename]
  FROM [WxOperationType] where id=@typeid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@typeid", typeid);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    WxOperationType oprtype = new WxOperationType();
                    oprtype.Id = reader.GetValue<int>("id");
                    oprtype.Typename = reader.GetValue<string>("typename");

                    reader.Close();

                    return oprtype;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
