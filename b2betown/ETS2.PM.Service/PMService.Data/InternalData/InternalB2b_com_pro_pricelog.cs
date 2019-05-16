using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalB2b_com_pro_pricelog
    {
        public SqlHelper sqlHelper;
        public InternalB2b_com_pro_pricelog(SqlHelper sqlHelper) 
        {
            this.sqlHelper = sqlHelper;
        }
        internal B2b_com_pro_pricelog  GetlastPriceLog(int proid)
        {
            string selsql2 = @"SELECT top 1 [id]
      ,[comid]
      ,[proid]
      ,[face_price]
      ,[advise_price]
      ,[agentsettle_price]
      ,[Agent1_price]
      ,[Agent2_price]
      ,[Agent3_price]
      ,[opertime]
      ,[operor]
  FROM [EtownDB].[dbo].[b2b_com_pro_pricelog] where proid=@proid order by id desc";
            var cmdd = sqlHelper.PrepareTextSqlCommand(selsql2);
            cmdd.AddParam("@proid", proid);


            using (var reader = cmdd.ExecuteReader())
            {
                B2b_com_pro_pricelog m = null;
                if (reader.Read())
                {
                    m = new B2b_com_pro_pricelog
                    {
                        id = reader.GetValue<int>("id"),
                        proid = reader.GetValue<int>("proid"),
                        advise_price = reader.GetValue<decimal>("advise_price"),
                        face_price = reader.GetValue<decimal>("face_price"),
                        agentsettle_price = reader.GetValue<decimal>("agentsettle_price"),
                        Agent1_price = reader.GetValue<decimal>("Agent1_price"),
                        Agent2_price = reader.GetValue<decimal>("Agent2_price"),
                        Agent3_price = reader.GetValue<decimal>("Agent3_price"),
                        comid = reader.GetValue<int>("comid"),
                        operor = reader.GetValue<int>("operor"),
                        opertime = reader.GetValue<DateTime>("opertime"),

                    };
                }
                return m;
            }
        }
        internal int InsertPriceLog(int proid, B2b_com_pro product)
        {
            string inssql = @"INSERT INTO [EtownDB].[dbo].[b2b_com_pro_pricelog]
           ([comid]
           ,[proid]
           ,[face_price]
           ,[advise_price]
           ,[agentsettle_price]
           ,[Agent1_price]
           ,[Agent2_price]
           ,[Agent3_price]
           ,[opertime]
           ,[operor])
     VALUES
           (@comid
           ,@proid
           ,@face_price 
           ,@advise_price 
           ,@agentsettle_price 
           ,@Agent1_price 
           ,@Agent2_price 
           ,@Agent3_price 
           ,@opertime 
           ,@operor )";
            var cmd = sqlHelper.PrepareTextSqlCommand(inssql);
            cmd.AddParam("@comid", product.Com_id);
            cmd.AddParam("@proid", proid);
            cmd.AddParam("@face_price", product.Face_price);
            cmd.AddParam("@advise_price", product.Advise_price);
            cmd.AddParam("@agentsettle_price", product.Agentsettle_price);
            cmd.AddParam("@Agent1_price", product.Agent1_price);
            cmd.AddParam("@Agent2_price", product.Agent2_price);
            cmd.AddParam("@Agent3_price", product.Agent3_price);
            cmd.AddParam("@opertime", DateTime.Now);
            cmd.AddParam("@operor", product.Createuserid);

            return cmd.ExecuteNonQuery();
        }
        internal int EditPriceLog(int proid, B2b_com_pro product)
        {
            if (product.Id == 0)//新增产品
            {
               return  InsertPriceLog(  proid,   product);
            }
            else //编辑产品
            {
                string selsql = "select count(1) from b2b_com_pro_pricelog where proid=" + proid;
                var cmd = sqlHelper.PrepareTextSqlCommand(selsql);
                object o = cmd.ExecuteScalar();
                if (o.ToString() == "0")
                {
                    return InsertPriceLog(proid, product);
                }
                else
                {
                    B2b_com_pro_pricelog log = GetlastPriceLog(proid);
                    if (log == null)
                    {
                        return 0;
                    }
                    else 
                    {
                        //判断产品价格是否进行了更改
                        if (log.face_price != product.Face_price || log.advise_price != product.Advise_price || log.agentsettle_price != product.Agentsettle_price || log.Agent1_price != product.Agent1_price || log.Agent2_price != product.Agent2_price || log.Agent3_price != product.Agent3_price)
                        {
                            return InsertPriceLog(proid, product);
                        }
                        else 
                        {
                            return 1;
                        }
                    }
                }
            }
        }
    }
}
