using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.Data.SqlHelper;
using System.Data.SqlClient;

namespace ETS2.CRM.Service.CRMService.Data.InternalData
{
    class InternalB2b_interesttagtype
    {
        private SqlHelper sqlHelper;
        public InternalB2b_interesttagtype(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal IList<B2b_interesttagtype> GetTagTypeList(int industryid, out int total)
        {
            string sql = "select id,typename,remark,createtime,industryid,isselfdefined  from B2b_interesttagtype where industryid=" + industryid + " order by isselfdefined";

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    IList<B2b_interesttagtype> list = new List<B2b_interesttagtype>();

                    while (reader.Read())
                    {
                        list.Add(new B2b_interesttagtype()
                        {
                            Id = reader.GetValue<int>("id"),
                            Typename = reader.GetValue<string>("typename"),
                            Remark = reader.GetValue<string>("remark"),
                            Createtime = reader.GetValue<DateTime>("createtime"),
                            Industryid = reader.GetValue<int>("industryid"),
                            Isselfdefined = reader.GetValue<int>("isselfdefined")

                        });
                    }
                    total = list.Count;
                    return list;

                }
            }
            catch
            {
                total = 0;
                return null;

            }
        }

        internal B2b_interesttagtype GetTagTypeById(int id)
        {
            string sql = "select id,typename,remark,createtime,industryid,isselfdefined  from B2b_interesttagtype where id=" + id;

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    B2b_interesttagtype u = null;

                    if (reader.Read())
                    {
                        u = new B2b_interesttagtype()
                        {
                            Id = reader.GetValue<int>("id"),
                            Typename = reader.GetValue<string>("typename"),
                            Remark = reader.GetValue<string>("remark"),
                            Createtime = reader.GetValue<DateTime>("createtime"),
                            Industryid = reader.GetValue<int>("industryid"),
                            Isselfdefined = reader.GetValue<int>("isselfdefined")
                        };
                    }


                    return u;
                }
            }
            catch
            {

                return null;

            }
        }

        internal int EditComTagType(int id, string name, string remark, int industryid)
        {
            if (id > 0)//编辑
            {
                string sql = "update B2b_interesttagtype set typename='" + name + "' , remark ='" + remark + "' where id=" + id;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                return cmd.ExecuteNonQuery();

            }
            else//添加
            {
                string sql = "insert into B2b_interesttagtype (typename,remark,createtime,industryid,isselfdefined) values('" + name + "','" + remark + "',getdate()," + industryid + ",'0');select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);

                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null ? 0 : int.Parse(o.ToString());
            }

        }

        internal int Deltagtype(int id)
        {

            sqlHelper.BeginTrancation();
            try
            {
                SqlCommand cmd = new SqlCommand();

                cmd = sqlHelper.PrepareTextSqlCommand("delete b2b_crm_interesttag where tagid in (select id from b2b_interesttag where tagtypeid in (select id from B2b_interesttagtype where id=" + id + "))");//操作会员兴趣关联表
                cmd.ExecuteNonQuery();
                cmd = sqlHelper.PrepareTextSqlCommand("delete b2b_interesttag where tagtypeid in (select id from B2b_interesttagtype where id=" + id + ")");//操作兴趣标签表
                cmd.ExecuteNonQuery();
                cmd = sqlHelper.PrepareTextSqlCommand("delete B2b_interesttagtype where id=" + id);//操作兴趣类型表
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
    }
}
