using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internalb2b_com_pro_Specitype
    {
        private SqlHelper sqlHelper;
        public Internalb2b_com_pro_Specitype(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int Editguigetype(B2b_com_pro_Specitype mguigetype)
        {
            string sql = "select  id  from b2b_com_pro_Specitype where proid=" + mguigetype.proid + " and type_name='" + mguigetype.type_name + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            var typeid = 0;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    typeid = reader.GetValue<int>("id");
                }
            }


            #region 编辑
            if (typeid > 0)
            {
                string sql1 = "update b2b_com_pro_Specitype set type_name='" + mguigetype.type_name + "' where id=" + typeid;
                var cmd1 = sqlHelper.PrepareTextSqlCommand(sql1);
                cmd1.ExecuteNonQuery();
                return typeid;
            }
            #endregion
            #region 添加
            else
            {
                string sql2 = "INSERT INTO  [b2b_com_pro_Specitype] ([comid]  ,[proid]  ,[type_name])  VALUES  (" + mguigetype.comid + "," + mguigetype.proid + ",'" + mguigetype.type_name + "');select @@identity;";
                var cmd2 = sqlHelper.PrepareTextSqlCommand(sql2);
                object o = cmd2.ExecuteScalar();
                return int.Parse(o.ToString());
            }
            #endregion
        }

        internal List<B2b_com_pro_Specitype> Getggtypelist(int proid)
        {
            string sql = "select * from b2b_com_pro_Specitype where proid=" + proid + " and id in (select typeid from b2b_com_pro_Specitypevalue where  proid=" + proid + " and isonline=1)";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                List<B2b_com_pro_Specitype> list = new List<B2b_com_pro_Specitype>();
                while (reader.Read())
                {
                    list.Add(new B2b_com_pro_Specitype
                    {
                        id = reader.GetValue<int>("id"),
                        comid = reader.GetValue<int>("comid"),
                        proid = reader.GetValue<int>("proid"),
                        type_name = reader.GetValue<string>("type_name")

                    });

                }
                return list;
            }
        }

        internal int Getggtypemaxid(int proid)
        {
            string sql = "select max(id) as  maxid from B2b_com_pro_Specitype where proid=" + proid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("maxid");
                }
                return -1;
            }
        }
    }
}
