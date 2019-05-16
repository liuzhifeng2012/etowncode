using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internalb2b_com_pro_Specitypevalue
    {
        private SqlHelper sqlHelper;
        public Internalb2b_com_pro_Specitypevalue(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int UpLinestatus(int proid, int linestatus)
        {
            string sql = "update b2b_com_pro_Specitypevalue set isonline=" + linestatus + " where proid=" + proid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int Editguigetypevalue(B2b_com_pro_Specitypevalue m)
        {
            string sql = "select  id  from b2b_com_pro_Specitypevalue where proid=" + m.proid + " and typeid='" + m.typeid + "' and val_name='" + m.val_name + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            var valid = 0;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    valid = reader.GetValue<int>("id");
                }
            }


            #region 编辑
            if (valid > 0)
            {
                string sql1 = "update b2b_com_pro_Specitypevalue set val_name='" + m.val_name + "',isonline=1 where id=" + valid;
                var cmd1 = sqlHelper.PrepareTextSqlCommand(sql1);
                cmd1.ExecuteNonQuery();
                return valid;
            }
            #endregion
            #region 添加
            else
            {
                string sql2 = "INSERT INTO  [b2b_com_pro_Specitypevalue] ([comid] ,typeid ,[val_name] ,[proid] ,isonline)  VALUES  (" + m.comid + "," + m.typeid + ",'" + m.val_name + "'," + m.proid + ",1);select @@identity;";
                var cmd2 = sqlHelper.PrepareTextSqlCommand(sql2);
                object o = cmd2.ExecuteScalar();
                return int.Parse(o.ToString());
            }
            #endregion
        }

        internal int Getguigevalueid(int proid, string ggtype, string ggvalue)
        {
            string sql = "select id from b2b_com_pro_Specitypevalue where val_name ='" + ggvalue + "' and proid=" + proid + " and typeid in (select id from b2b_com_pro_Specitype where type_name='" + ggtype + "' and proid=" + proid + ")";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                int valid = 0;
                if (reader.Read())
                {
                    valid = reader.GetValue<int>("id");
                }
                return valid;
            }
        }

        internal List<B2b_com_pro_Specitypevalue> Getggvallist(int ggtypeid)
        {
            string sql = "select * from b2b_com_pro_Specitypevalue where typeid=" + ggtypeid+" and isonline=1";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                List<B2b_com_pro_Specitypevalue> list = new List<B2b_com_pro_Specitypevalue>();
                while (reader.Read())
                {
                    list.Add(new B2b_com_pro_Specitypevalue
                    {
                        id = reader.GetValue<int>("id"),
                        comid = reader.GetValue<int>("comid"),
                        isonline = reader.GetValue<int>("isonline"),
                        proid = reader.GetValue<int>("proid"),
                        typeid = reader.GetValue<int>("typeid"),
                        val_name = reader.GetValue<string>("val_name"),
                        Name = reader.GetValue<string>("val_name"),
                    });
                }
                return list;
            }
        }

        internal List<B2b_com_pro_Specitypevalue> Getexpiredggvallist(int proid)
        {
            string sql = "select * from b2b_com_pro_Specitypevalue where     isonline=0";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                List<B2b_com_pro_Specitypevalue> list = new List<B2b_com_pro_Specitypevalue>();
                while (reader.Read())
                {
                    list.Add(new B2b_com_pro_Specitypevalue
                    {
                        id = reader.GetValue<int>("id"),
                        comid = reader.GetValue<int>("comid"),
                        isonline = reader.GetValue<int>("isonline"),
                        proid = reader.GetValue<int>("proid"),
                        typeid = reader.GetValue<int>("typeid"),
                        val_name = reader.GetValue<string>("val_name"),
                        Name = reader.GetValue<string>("val_name"),
                    });
                }
                return list;
            }
        }
    }
}
