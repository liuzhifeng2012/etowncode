using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Model;

namespace ETS2.WeiXin.Service.WeiXinService.Data.InternalData
{
    public class Internalwxqunfa_news_addrecord
    {
        private SqlHelper sqlHelper;
        public Internalwxqunfa_news_addrecord(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal List<Wxqunfa_news_addrecord> Wxqunfa_news_addrecordpagelist(int userid, int comid, int pageindex, int pagesize, string key, out int totalcount)
        {

            string condition = " comid=" + comid;
            //公司员工显示公司的；门市员工显示门市的；
            if (userid > 0)
            {
                try
                {
                    string sql2 = "select channelcompanyid from b2b_company_manageuser where id=" + userid;
                    var cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                    object o = cmd.ExecuteScalar();
                    
                    if (int.Parse(o.ToString()) > 0)
                    {
                        condition += " and createuserid in (select id from b2b_company_manageuser  where channelcompanyid=" + int.Parse(o.ToString()) + " and com_id=" + comid + ")";
                    }
                    else
                    {
                        condition += " and createuserid in (select id from b2b_company_manageuser  where channelcompanyid=0 and com_id=" + comid + ")";
                    }
                }
                catch 
                {
                    sqlHelper.Dispose();
                    condition += " and createuserid in (select id from b2b_company_manageuser  where channelcompanyid=0 and com_id=" + comid + ")";
               
                }
            }
            condition += " and DATEADD(day,-3,GETDATE())<createtime";

            var cmdd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            cmdd.PagingCommand1("wxqunfa_news_addrecord", "*", "id desc", "", pagesize, pageindex, "", condition);

            List<Wxqunfa_news_addrecord> list = new List<Wxqunfa_news_addrecord>();
            using (var reader = cmdd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Wxqunfa_news_addrecord
                    {
                        id = reader.GetValue<int>("id"),
                        is_singlenews = reader.GetValue<int>("is_singlenews"),
                        type = reader.GetValue<string>("type"),
                        media_id = reader.GetValue<string>("media_id"),
                        created_at = reader.GetValue<string>("created_at"),
                        createtime = reader.GetValue<DateTime>("createtime"),
                        createuserid = reader.GetValue<int>("createuserid"),
                        comid = reader.GetValue<int>("comid")
                    });
                }
            }

            totalcount = int.Parse(cmdd.Parameters[0].Value.ToString());
            return list;

        }

        internal Wxqunfa_news_addrecord Wxqunfa_news_addrecord(int userid, int comid, int tuwen_recordid)
        {

            string condition = " comid=" + comid;
            //公司员工显示公司的；门市员工显示门市的；
            if (userid > 0)
            {
                try
                {
                    string sql2 = "select channelcompanyid from b2b_company_manageuser where id=" + userid;
                    var cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                    object o = cmd.ExecuteScalar();
                    sqlHelper.Dispose();
                    if (int.Parse(o.ToString()) > 0)
                    {
                        condition += " and createuserid in (select id from b2b_company_manageuser  where channelcompanyid=" + int.Parse(o.ToString()) + " and com_id=" + comid + ")";
                    }
                    else
                    {
                        condition += " and createuserid in (select id from b2b_company_manageuser  where channelcompanyid=0 and com_id=" + comid + ")";
                    }
                }
                catch 
                {
                    sqlHelper.Dispose();
                    condition += " and createuserid in (select id from b2b_company_manageuser  where channelcompanyid=0 and com_id=" + comid + ")";
                
                }
            }
            condition += " and id=" + tuwen_recordid;
            string sql = "select * from  wxqunfa_news_addrecord where " + condition;
            var cmdd = sqlHelper.PrepareTextSqlCommand(sql);

            using (var reader = cmdd.ExecuteReader())
            {
                Wxqunfa_news_addrecord r = null;
                if (reader.Read())
                {
                    r = new Wxqunfa_news_addrecord
                   {
                       id = reader.GetValue<int>("id"),
                       is_singlenews = reader.GetValue<int>("is_singlenews"),
                       type = reader.GetValue<string>("type"),
                       media_id = reader.GetValue<string>("media_id"),
                       created_at = reader.GetValue<string>("created_at"),
                       createtime = reader.GetValue<DateTime>("createtime"),
                       createuserid = reader.GetValue<int>("createuserid"),
                       comid = reader.GetValue<int>("comid")
                   };
                }
                return r;
            }
        }
    }
}
