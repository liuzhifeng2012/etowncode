using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.Data.SqlHelper;
using System.Data.SqlClient;

namespace ETS2.CRM.Service.CRMService.Data.InternalData
{
    public class InternalB2b_group
    {
        public SqlHelper sqlHelper;
        public InternalB2b_group(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal IList<B2b_group> GetCrmGroupList(out int total, int comid, int pageindex, int pagesize)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

            cmd.PagingCommand1("b2b_group", "*", "id", "", pagesize, pageindex, "", "comid=" + comid);


            List<B2b_group> list = new List<B2b_group>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_group
                    {
                        Id = reader.GetValue<int>("id"),
                        Groupname = reader.GetValue<string>("groupname"),
                        Remark = reader.GetValue<string>("remark"),
                        Comid = reader.GetValue<int>("comid"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Createuserid = reader.GetValue<int>("createuserid")
                    });

                }
            }
            total = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }

        internal B2b_group GetB2bgroup(int groupid)
        {
            string sql = "select id,groupname,remark,comid,createtime,createuserid from b2b_group  where id=" + groupid;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);


            using (var reader = cmd.ExecuteReader())
            {
                B2b_group u = null;
                if (reader.Read())
                {
                    u = new B2b_group
                    {
                        Id = reader.GetValue<int>("id"),
                        Groupname = reader.GetValue<string>("groupname"),
                        Remark = reader.GetValue<string>("remark"),
                        Comid = reader.GetValue<int>("comid"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Createuserid = reader.GetValue<int>("createuserid")
                    };

                }
                return u;
            }


        }

        internal int EditB2bGroup(int id, string name, int comid, int userid, string remark, DateTime createtime)
        {
            if (id > 0)
            {
                string sql = "update b2b_group set groupname='" + name + "' where id=" + id;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.ExecuteNonQuery();
                return id;
            }
            else
            {
                string sql = "insert into b2b_group (groupname,remark,comid,createtime,createuserid) values('" + name + "','" + remark + "'," + comid + ",'" + createtime + "'," + userid + ");select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                return o == null ? 0 : int.Parse(o.ToString());
            }
        }

        internal int Delb2bgroup(int groupid)
        {
            sqlHelper.BeginTrancation();
            try
            {
                SqlCommand cmd = new SqlCommand();


                cmd = sqlHelper.PrepareTextSqlCommand("update b2b_crm set groupid=0 where groupid=" + groupid);
                cmd.ExecuteNonQuery();
                cmd = sqlHelper.PrepareTextSqlCommand("delete b2b_group where id=" + groupid);
                cmd.ExecuteNonQuery();

                sqlHelper.Commit();
                sqlHelper.Dispose();
                return 1;

            }
            catch (Exception e)
            {
                sqlHelper.Rollback();
                sqlHelper.Dispose();
                return 0;
            }
 

        }

        internal IList<B2b_group> GetCompanyB2bgroup(out int total, int comid)
        {
            string sql = "SELECT [id]      ,[groupname]      ,[remark]      ,[comid]      ,[createtime]      ,[createuserid]  FROM [EtownDB].[dbo].[B2b_Group] where comid=" + comid;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);


            List<B2b_group> list = new List<B2b_group>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_group
                    {
                        Id = reader.GetValue<int>("id"),
                        Groupname = reader.GetValue<string>("groupname"),
                        Remark = reader.GetValue<string>("remark"),
                        Comid = reader.GetValue<int>("comid"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Createuserid = reader.GetValue<int>("createuserid")
                    });

                }
            }
            total = list.Count;

            return list;
        }

        internal int Changefenzu(int crmid, int groupid)
        {
            string sql = "update b2b_crm set groupid='" + groupid + "' where id=" + crmid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();

        }
    }
}
