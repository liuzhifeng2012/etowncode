using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;
using System.Data.SqlClient;

namespace ETS2.CRM.Service.CRMService.Data.InternalData
{
    public class InternalB2b_crm_interesttag
    {
        private SqlHelper sqlHelper;
        public InternalB2b_crm_interesttag(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal IList<B2b_crm_interesttag> GetCrmInterest(out int total, int crmid)
        {
            string sql = "select id,crmid,tagid from b2b_crm_interesttag where crmid=" + crmid;


            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    IList<B2b_crm_interesttag> list = new List<B2b_crm_interesttag>();

                    while (reader.Read())
                    {
                        list.Add(new B2b_crm_interesttag()
                        {
                            Id = reader.GetValue<int>("id"),
                            Crmid = reader.GetValue<int>("crmid"),
                            Tagid = reader.GetValue<int>("tagid")
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

        internal int EditCrmInterest(int crmid, string checkedstr)
        {

            sqlHelper.BeginTrancation();
            try
            {
                SqlCommand cmd = new SqlCommand();

                cmd = sqlHelper.PrepareTextSqlCommand("delete b2b_crm_interesttag where crmid=" + crmid);
                cmd.ExecuteNonQuery();

                string[] str = checkedstr.Split(',');
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] != "")
                    {
                        cmd = sqlHelper.PrepareTextSqlCommand("insert into b2b_crm_interesttag (crmid,tagid) values(" + crmid + "," + str[i] + ")");
                        cmd.ExecuteNonQuery();
                    }
                }
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
