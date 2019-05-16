using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.CRM.Service.CRMService.Data.InternalData
{
    public class InternalB2bCompanyPos
    {
        private SqlHelper sqlHelper;
        public InternalB2bCompanyPos(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal B2b_company_pos GetPosByPosId(string pos_id)
        {
            const string sqlTxt = @"SELECT [id]
      ,[com_id]
      ,[poscompany]
      ,[posid]
      ,[BindingTime]
      ,[admin]
      ,[Remark]
  FROM [EtownDB].[dbo].[b2b_company_pos] where  posid=@posid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@posid", pos_id);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_company_pos u = null;

                while (reader.Read())
                {
                    u = new B2b_company_pos
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Poscompany = reader.GetValue<string>("poscompany"),
                        Posid = reader.GetValue<int>("posid"),
                        BindingTime = reader.GetValue<DateTime>("BindingTime"),
                        Admin = reader.GetValue<string>("admin"),
                        Remark = reader.GetValue<string>("Remark")
                    };

                }
                return u;
            }
        }

        internal List<B2b_company_pos> GetPosByComId(int comid,out  int secondtotalcount)
        {
            const string sqlTxt = @"SELECT [id]
      ,[com_id]
      ,[poscompany]
      ,[posid]
      ,[BindingTime]
      ,[admin]
      ,[Remark]
  FROM [EtownDB].[dbo].[b2b_company_pos] where  com_id=@comid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@comid", comid);

            try
            {
                using (var reader = cmd.ExecuteReader())
                {
                    List<B2b_company_pos> list = new List<B2b_company_pos>();

                    while (reader.Read())
                    {
                        list.Add(new B2b_company_pos
                        {
                            Id = reader.GetValue<int>("id"),
                            Com_id = reader.GetValue<int>("com_id"),
                            Poscompany = reader.GetValue<string>("poscompany"),
                            Posid = reader.GetValue<int>("posid"),
                            BindingTime = reader.GetValue<DateTime>("BindingTime"),
                            Admin = reader.GetValue<string>("admin"),
                            Remark = reader.GetValue<string>("Remark")
                        });

                    }
                    secondtotalcount = list.Count;
                    return list;
                }
            }
            catch (Exception e)
            {
                secondtotalcount = 0;
                return null;
            }
        }
    }
}
