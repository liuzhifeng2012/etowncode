using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.VAS.Service.VASService.Modle;


namespace ETS2.VAS.Service.VASService.Data.InternalData
{
    public class InternalMemberIntegral
    {

         private SqlHelper sqlHelper;
         public InternalMemberIntegral(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }


         #region 积分插入数据库
         private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateIntegral";
         internal int InsertOrUpdate(Member_Integral model)
         {
             var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);
             cmd.AddParam("@Id", model.Id);
             cmd.AddParam("@Comid", model.Comid);
             cmd.AddParam("@Money", model.Money);
             cmd.AddParam("@Admin", model.Admin);
             cmd.AddParam("@Ip", model.Ip);
             cmd.AddParam("@Ptype", model.Ptype);
             cmd.AddParam("@Oid", model.Oid);
             cmd.AddParam("@Remark", model.Remark);
             cmd.AddParam("@orderid",model.OrderId);
             cmd.AddParam("@ordername",model.OrderName);

             var parm = cmd.AddReturnValueParameter("ReturnValue");
             cmd.ExecuteNonQuery();
             return (int)parm.Value;
         }
         #endregion

         #region 获得积分日志
         /// <summary>
         ///  获得积分日志
         /// </summary>
         /// <param name="comid"></param>
         /// <returns></returns>
         internal List<Member_Integral> ReadIntegral(int id, int comid, out int totalcount)
         {
             int readi = 0;
             const string sqlTxt = @"SELECT TOP 10 [id]
      ,[com_id]
      ,[mid]
      ,[oid]
      ,[money]
      ,[ptype]
      ,[remark]
      ,[subdate]
      ,[admin]
      ,[ip]
      ,orderid
      ,orderName
        FROM [EtownDB].[dbo].[Member_Integral_Log] as a where a.com_ID=@Comid and a.mid=@Id order by id desc";

             var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
             cmd.AddParam("@Comid", comid);
             cmd.AddParam("@Id", id);

             List<Member_Integral> list = new List<Member_Integral>();
             using (var reader = cmd.ExecuteReader())
             {
                 while (reader.Read())
                 {
                     list.Add(new Member_Integral
                     {
                         Id = reader.GetValue<int>("id"),
                         Comid = reader.GetValue<int>("com_id"),
                         Admin = reader.GetValue<string>("Admin"),
                         Ip = reader.GetValue<string>("Ip"),
                         Money = reader.GetValue<decimal>("Money"),
                         Mid = reader.GetValue<int>("Mid"),
                         Ptype = reader.GetValue<int>("Ptype"),
                         Remark = reader.GetValue<string>("Remark"),
                         Subdate = reader.GetValue<DateTime>("Subdate"),
                         OrderId = reader.GetValue<decimal>("OrderId"),
                         OrderName = reader.GetValue<string>("OrderName")
                         
                     });
                     readi = readi + 1;
                 }
             }
             totalcount = readi;
             return list;

         }
         #endregion


    }
}
