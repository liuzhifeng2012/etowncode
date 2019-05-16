using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;
using System.Data.SqlClient;

namespace ETS2.CRM.Service.CRMService.Data.InternalData
{
    public class InternalB2b_interesttag
    {
        private SqlHelper sqlHelper;
        public InternalB2b_interesttag(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal IList<B2b_interesttag> GetTagListByTypeid(out int total, int typeid, int comid, string issystemadd)
        {
            string sql = "select id,tagname,tagtypeid from b2b_interesttag where 1=1 ";
            if (typeid > 0)
            {
                sql += "  and tagtypeid=" + typeid;
            }
            if (issystemadd == "0,1")
            {
                if (comid > 0)//查询系统添加的和特定公司添加的
                {
                    sql += " and (issystemadd=1 or (issystemadd=0 and comid="+comid+"))";
                }
                else
                { //查询全部(系统添加的和所有公司添加的)

                }
            }
            else
            {
                if (issystemadd == "0")//查询公司添加的
                {
                    sql += " and issystemadd=0 and comid="+comid;
                }
                if (issystemadd == "1")//查询系统添加的
                {
                    sql += " and issystemadd=1 ";
                }
            }


            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    IList<B2b_interesttag> list = new List<B2b_interesttag>();

                    while (reader.Read())
                    {
                        list.Add(new B2b_interesttag()
                        {
                            Id = reader.GetValue<int>("id"),
                            TagName = reader.GetValue<string>("tagname"),
                            Tagtypeid = reader.GetValue<int>("tagtypeid")
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

        internal B2b_interesttag GetTagById(int id)
        {
            string sql = "select id,tagname,tagtypeid from b2b_interesttag where id=" + id;


            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    B2b_interesttag u = null;

                    if (reader.Read())
                    {
                        u = new B2b_interesttag()
                        {
                            Id = reader.GetValue<int>("id"),
                            TagName = reader.GetValue<string>("tagname"),
                            Tagtypeid = reader.GetValue<int>("tagtypeid")
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

        internal int EditTag(int id, string name, int tagtypeid, int issystemadd = 0, int comid = 0)
        {
            if (id == 0)
            {
                string sql = "insert into b2b_interesttag (tagname,tagtypeid,issystemadd,comid) values('" + name + "'," + tagtypeid + "," + issystemadd + "," + comid + ");select @@identity;";

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                return o == null ? 0 : int.Parse(o.ToString());
            }
            else
            {
                string sql = "update b2b_interesttag set tagname='" + name + "' where id=" + id;

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.ExecuteNonQuery();
                return id;

            }
        }

        internal int Deltag(int tagid)
        {
            sqlHelper.BeginTrancation();
            try
            {
                SqlCommand cmd = new SqlCommand();

                cmd = sqlHelper.PrepareTextSqlCommand("delete b2b_crm_interesttag where tagid =" + tagid);//操作会员兴趣关联表
                cmd.ExecuteNonQuery();
                cmd = sqlHelper.PrepareTextSqlCommand("delete b2b_interesttag where id=" + tagid);//操作兴趣标签表
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
