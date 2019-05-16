using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.Member.Service.MemberService.Model;
using System.Data;

namespace ETS2.Member.Service.MemberService.Data.InternalData
{
    public class InternalMemberForwarding
    {
       private SqlHelper sqlHelper;
        public InternalMemberForwarding(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        //通过渠道ID 获得
        internal int Forwardingcount_search(int uid, int wxmaid, string uip, int comid)
        {
            const string sqltxt = @"SELECT  *
  FROM [dbo].[Member_forwardingcount_log] where wxmaid=@wxmaid and comid=@comid and uip=@uip";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@uid", uid);
            cmd.AddParam("@wxmaid", wxmaid);
            cmd.AddParam("@uip", uip);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                       return reader.GetValue<int>("Id");
                }
            }
            return 0;
        }

        //记录访问日志
        internal int Forwardingcountlog_add(int uid, int wxmaid, string uip, int comid)
        {

            string sqltxt = "";
            sqltxt = @"insert Member_forwardingcount_log (comid,wxmaid,uid,uip) values(@comid,@wxmaid,0,@uip)";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@wxmaid", wxmaid);
            cmd.AddParam("@uip", uip);
            cmd.AddParam("@comid", comid);
            return  cmd.ExecuteNonQuery();
        }

        //通过渠道ID 获得
        internal int Forwardingcount_add(int uid, int wxmaid, int comid)
        {
            int weixinjilu = 0;
            string sqltxt = @"SELECT  *
  FROM [dbo].[Member_forwardingcount] where wxmaid=@wxmaid and comid=@comid and uid=@uid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@wxmaid", wxmaid);
            cmd.AddParam("@uid", uid);

            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    weixinjilu = reader.GetValue<int>("Id");
                }
            }
            return weixinjilu;
        }

        //插入数据库或+1
        internal int ForwardingcountInsert(int uid, int wxmaid, int comid, int weixinjilu)
        {
            string sqltxt = "";

            //如果有weixinjilu=0，
            if (weixinjilu == 0)
            {
                sqltxt = @"insert Member_forwardingcount (comid,wxmaid,uid) values(@comid,@wxmaid,@uid)";
                var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
                cmd.AddParam("@wxmaid", wxmaid);
                cmd.AddParam("@uid", uid);
                cmd.AddParam("@comid", comid);
                return cmd.ExecuteNonQuery();
            }
            else
            {
                sqltxt = @"update Member_forwardingcount set fornum=fornum+1 where id=@weixinjilu";
                var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
                cmd.AddParam("@weixinjilu", weixinjilu);
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
