using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.CRM.Service.CRMService.Data.InternalData
{
    public class InternalPosversionmodifylog
    {
        private SqlHelper sqlHelper;
        public InternalPosversionmodifylog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        public Posversionmodifylog GetLasterPosversionmodifylogByPosid(string pos_id)
        {
            const string sqlTxt = @"SELECT top 1 [id]
      ,[posid]
      ,[versionNo]
      ,[updatefileurl]
      ,[updatetype]
      ,[updatetime]
  FROM [EtownDB].[dbo].[posversionmodifylog] where posid=@posid order by id desc";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@posid", pos_id);

            using (var reader = cmd.ExecuteReader())
            {
                Posversionmodifylog u = null;

                while (reader.Read())
                {
                    u = new Posversionmodifylog
                    {
                        Id = reader.GetValue<int>("id"),
                        Posid = reader.GetValue<int>("posid"),
                        VersionNo = reader.GetValue<decimal>("versionNo"),
                        Updatefileurl = reader.GetValue<string>("updatefileurl"),
                        Updatetime = reader.GetValue<DateTime>("updatetime"),
                        Updatetype = reader.GetValue<int>("updatetype"),
                    };

                }
                return u;
            }
        }

        public IList<Posversionmodifylog> GetPosVersionPageList(int pageindex, int pagesize, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "posversionmodifylog";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "0";
            var condition = "id in (select max(id) from posversionmodifylog group by posid having count(posid)>0)";
            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<Posversionmodifylog> list = new List<Posversionmodifylog>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Posversionmodifylog
                    {
                        Id = reader.GetValue<int>("id"),
                        VersionNo = reader.GetValue<decimal>("VersionNo"),
                        Posid = reader.GetValue<int>("Posid"),
                        Updatefileurl = reader.GetValue<string>("Updatefileurl"),
                        Updatetime = reader.GetValue<DateTime>("Updatetime"),
                        Updatetype = reader.GetValue<int>("Updatetype"),


                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }
    }
}
