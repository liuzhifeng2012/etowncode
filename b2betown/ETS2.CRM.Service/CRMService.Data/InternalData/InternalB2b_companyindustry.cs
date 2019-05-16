using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.Data.SqlHelper;
using System.Data.SqlClient;

namespace ETS2.CRM.Service.CRMService.Data.InternalData
{
    public class InternalB2b_companyindustry
    {
        private SqlHelper sqlHelper;
        public InternalB2b_companyindustry(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal IList<B2b_companyindustry> GetIndustryList(out int total, int industryid = 0)
        {
            string sql = "select id,industryname,remark from b2b_companyindustry";
            if (industryid > 0)
            {
                sql += "  where id=" + industryid;
            }

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    IList<B2b_companyindustry> list = new List<B2b_companyindustry>();

                    while (reader.Read())
                    {
                        list.Add(new B2b_companyindustry()
                        {
                            Id = reader.GetValue<int>("id"),
                            Industryname = reader.GetValue<string>("industryname"),
                            Remark = reader.GetValue<string>("remark")
                        });
                    }
                    sqlHelper.Dispose();
                    total = list.Count;
                    return list;

                }
            }
            catch
            {
                sqlHelper.Dispose();
                total = 0;
                return null;

            }
        }

        internal B2b_companyindustry GetIndustryById(int id)
        {
            string sql = "select id,industryname,remark from b2b_companyindustry where id=" + id;

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    B2b_companyindustry u = null;

                    if (reader.Read())
                    {
                        u = new B2b_companyindustry()
                       {
                           Id = reader.GetValue<int>("id"),
                           Industryname = reader.GetValue<string>("industryname"),
                           Remark = reader.GetValue<string>("remark")
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

        internal int EditComIndustry(int id, string name, string remark)
        {

            if (id > 0)
            {
                string sql = "update b2b_companyindustry set industryname='" + name + "',remark='" + remark + "' where id=" + id;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);

                cmd.ExecuteNonQuery();
                return id;
            }
            else
            {
                sqlHelper.BeginTrancation();
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd = sqlHelper.PrepareTextSqlCommand("insert into b2b_companyindustry(industryname,remark) values('" + name + "','" + remark + "');select @@identity;");//操作公司行业表
                    object o = cmd.ExecuteScalar();

                    //添加公司行业后默认添加自定义兴趣类型
                    cmd = sqlHelper.PrepareTextSqlCommand("insert into B2b_interesttagtype(typename,remark,createtime,industryid,isselfdefined) values('自定义兴趣类型','',getdate(),'" + o.ToString() + "','1')");//操作兴趣类型表
                    cmd.ExecuteNonQuery();

                    sqlHelper.Commit();
                    sqlHelper.Dispose();
                    return o == null ? 0 : int.Parse(o.ToString());

                }
                catch (Exception e)
                {
                    sqlHelper.Rollback();
                    sqlHelper.Dispose();
                    return 0;
                }
              
            }
        }

        internal int Delindustry(int id)
        {

            sqlHelper.BeginTrancation();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd = sqlHelper.PrepareTextSqlCommand("delete b2b_crm_interesttag where tagid in (select id from b2b_interesttag where tagtypeid in (select id from B2b_interesttagtype where industryid=" + id + "))");//操作会员兴趣关联表
                cmd.ExecuteNonQuery();
                cmd = sqlHelper.PrepareTextSqlCommand("delete b2b_interesttag where tagtypeid in (select id from B2b_interesttagtype where industryid=" + id + ")");//操作兴趣标签表
                cmd.ExecuteNonQuery();
                cmd = sqlHelper.PrepareTextSqlCommand("delete B2b_interesttagtype where industryid=" + id);//操作兴趣类型表
                cmd.ExecuteNonQuery();
                cmd = sqlHelper.PrepareTextSqlCommand("delete b2b_companyindustry where id=" + id);//操作公司行业表
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

        internal B2b_companyindustry GetIndustryByComid(int comid)
        {
            string sql = "SELECT [id] ,[industryname] ,[remark] FROM [EtownDB].[dbo].[b2b_companyindustry] where id =(select com_type from b2b_company where id=" + comid + ")";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    B2b_companyindustry u = null;

                    if (reader.Read())
                    {
                        u = new B2b_companyindustry()
                        {
                            Id = reader.GetValue<int>("id"),
                            Industryname = reader.GetValue<string>("industryname"),
                            Remark = reader.GetValue<string>("remark")
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
    }
}
