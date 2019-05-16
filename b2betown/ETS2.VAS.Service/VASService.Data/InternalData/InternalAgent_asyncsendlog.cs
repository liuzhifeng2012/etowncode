using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.VAS.Service.VASService.Modle;
using ETS.Data.SqlHelper;

namespace ETS2.VAS.Service.VASService.Data.InternalData
{
    public class InternalAgent_asyncsendlog
    {
        private SqlHelper sqlHelper;
        public InternalAgent_asyncsendlog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal int EditLog(Agent_asyncsendlog log)
        {
            string sql = @"INSERT INTO [EtownDB].[dbo].[agent_asyncsendlog]
           ([pno]
           ,[num]
           ,[confirmtime]
           ,[sendtime]
           ,[issendsuc]
           ,[agentupdatestatus]
           ,[remark]
           ,[agentcomid]
           ,[comid]
           ,issecondsend,platform_req_seq,request_content,response_content,b2b_etcket_logid,isreissue)
     VALUES
           (@pno
           ,@num
           ,@confirmtime
           ,@sendtime
           ,@issendsuc
           ,@agentupdatestatus
           ,@remark
           ,@agentcomid
           ,@comid
           ,@issecondsend,@platform_req_seq,@request_content,@response_content,@b2b_etcket_logid,@isreissue);select @@IDENTITY;";

            if (log.Id > 0)
            {
                sql = @"UPDATE [EtownDB].[dbo].[agent_asyncsendlog]
                       SET [pno] = @pno
                          ,[num] = @num
                          ,[confirmtime] = @confirmtime
                          ,[sendtime] = @sendtime
                          ,[issendsuc] = @issendsuc
                          ,[agentupdatestatus] = @agentupdatestatus
                          ,[remark] = @remark
                          ,[agentcomid] = @agentcomid
                          ,[comid] = @comid
                          ,issecondsend=@issecondsend
                          ,platform_req_seq=@platform_req_seq
                          ,request_content=@request_content  
                          ,response_content=@response_content
                          ,b2b_etcket_logid=@b2b_etcket_logid
                          ,isreissue=@isreissue
                     WHERE id=@id";
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            if (log.Id > 0)
            {
                cmd.AddParam("@id", log.Id);
            }
            cmd.AddParam("@pno", log.Pno);
            cmd.AddParam("@num", log.Num);
            cmd.AddParam("@confirmtime", log.Confirmtime);
            cmd.AddParam("@sendtime", log.Sendtime);
            cmd.AddParam("@issendsuc", log.Issendsuc);
            cmd.AddParam("@agentupdatestatus", log.Agentupdatestatus);
            cmd.AddParam("@remark", log.Remark);
            cmd.AddParam("@agentcomid", log.Agentcomid);
            cmd.AddParam("@comid", log.Comid);
            cmd.AddParam("@issecondsend", log.Issecondsend);
            cmd.AddParam("@platform_req_seq", log.Platform_req_seq);
            cmd.AddParam("@request_content", log.Request_content == null ? "" : log.Request_content);
            cmd.AddParam("@response_content", log.Response_content == null ? "" : log.Response_content);
            cmd.AddParam("@b2b_etcket_logid", log.b2b_etcket_logid);
            cmd.AddParam("@isreissue", log.isreissue);
            if (log.Id > 0)
            {
                cmd.ExecuteNonQuery();
                return log.Id;
            }
            else
            {
                object o = cmd.ExecuteScalar();
                return o == null ? 0 : int.Parse(o.ToString());
            }

        }

        internal Agent_asyncsendlog GetTop1SendFail()
        {
            string sql = @"SELECT top 1 [id]
      ,[pno]
      ,[num]
      ,[confirmtime]
      ,[sendtime]
      ,[issendsuc]
      ,[agentupdatestatus]
      ,[remark]
      ,[agentcomid]
      ,[comid]
      ,issecondsend
     , b2b_etcket_logid
  FROM [EtownDB].[dbo].[agent_asyncsendlog]  where  issecondsend=0 and  issendsuc=0  and b2b_etcket_logid in ( select b2b_etcket_logid from agent_asyncsendlog group by b2b_etcket_logid having COUNT(1)<3)
  and b2b_etcket_logid not in (select b2b_etcket_logid from [agent_asyncsendlog] where agentupdatestatus=0)";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                Agent_asyncsendlog u = null;
                if (reader.Read())
                {
                    u = new Agent_asyncsendlog
                    {
                        Id = reader.GetValue<int>("id"),
                        Pno = reader.GetValue<string>("pno"),
                        Num = reader.GetValue<int>("num"),
                        Confirmtime = reader.GetValue<DateTime>("confirmtime"),
                        Sendtime = reader.GetValue<DateTime>("sendtime"),
                        Issendsuc = reader.GetValue<int>("issendsuc"),
                        Agentupdatestatus = reader.GetValue<int>("agentupdatestatus"),
                        Remark = reader.GetValue<string>("remark"),
                        Agentcomid = reader.GetValue<int>("agentcomid"),
                        Comid = reader.GetValue<int>("comid"),
                        Issecondsend = reader.GetValue<int>("issecondsend"),
                        b2b_etcket_logid = reader.GetValue<int>("b2b_etcket_logid"),
                    };
                }
                sqlHelper.Dispose();
                return u;
            }
        }

        internal List<Agent_asyncsendlog> GetTop10SendFail()
        {
            string sql = @"SELECT top 10 [id]
      ,[pno]
      ,[num]
      ,[confirmtime]
      ,[sendtime]
      ,[issendsuc]
      ,[agentupdatestatus]
      ,[remark]
      ,[agentcomid]
      ,[comid]
  ,issecondsend
,platform_req_seq
,request_content
,response_content
,b2b_etcket_logid 
  FROM [EtownDB].[dbo].[agent_asyncsendlog]
  where issecondsend=0 
  and agentupdatestatus=1
  and b2b_etcket_logid in ( select b2b_etcket_logid from agent_asyncsendlog group by b2b_etcket_logid having COUNT(1)<3)
  and b2b_etcket_logid not in (select b2b_etcket_logid from [agent_asyncsendlog] where agentupdatestatus=0)";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                List<Agent_asyncsendlog> list = new List<Agent_asyncsendlog>();
                while (reader.Read())
                {
                    list.Add(new Agent_asyncsendlog
                    {
                        Id = reader.GetValue<int>("id"),
                        Pno = reader.GetValue<string>("pno"),
                        Num = reader.GetValue<int>("num"),
                        Confirmtime = reader.GetValue<DateTime>("confirmtime"),
                        Sendtime = reader.GetValue<DateTime>("sendtime"),
                        Issendsuc = reader.GetValue<int>("issendsuc"),
                        Agentupdatestatus = reader.GetValue<int>("agentupdatestatus"),
                        Remark = reader.GetValue<string>("remark"),
                        Agentcomid = reader.GetValue<int>("agentcomid"),
                        Comid = reader.GetValue<int>("comid"),
                        Issecondsend = reader.GetValue<int>("issecondsend"),
                        Platform_req_seq = reader.GetValue<string>("platform_req_seq"),
                        Request_content = reader.GetValue<string>("request_content"),
                        Response_content = reader.GetValue<string>("response_content"),
                        b2b_etcket_logid = reader.GetValue<int>("b2b_etcket_logid"),
                    });
                }
                return list;
            }
        }

        internal int Getnoticelognum(string key)
        {
            string sql = "select count(1) from agent_asyncsendlog where pno='" + key + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            return int.Parse(o.ToString());
        }

        internal List<Agent_asyncsendlog> GetSendNoticeErrList(string pno)
        {
            string sql = "select  * from agent_asyncsendlog where pno='" + pno + "' and platform_req_seq not in (select platform_req_seq from agent_asyncsendlog where response_content='success' and pno='" + pno + "')";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            List<Agent_asyncsendlog> list = new List<Agent_asyncsendlog>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Agent_asyncsendlog()
                    {
                        Id = reader.GetValue<int>("id"),
                        Pno = reader.GetValue<string>("pno"),
                        Num = reader.GetValue<int>("num"),
                        Confirmtime = reader.GetValue<DateTime>("confirmtime"),
                        Sendtime = reader.GetValue<DateTime>("sendtime"),
                        Issendsuc = reader.GetValue<int>("issendsuc"),
                        Agentupdatestatus = reader.GetValue<int>("agentupdatestatus"),
                        Remark = reader.GetValue<string>("remark"),
                        Agentcomid = reader.GetValue<int>("agentcomid"),
                        Comid = reader.GetValue<int>("comid"),
                        Issecondsend = reader.GetValue<int>("issecondsend"),
                        b2b_etcket_logid = reader.GetValue<int>("b2b_etcket_logid"),
                    });
                }
            }
            return list;
        }

        internal List<Agent_asyncsendlog> Getyzlogs(int agentcomid, string yzdate, string pno, int comid, int pageindex, int pagesize, out int totalcount, int id = 0)
        { 
            string sql = "select min(id) from agent_asyncsendlog where 1=1 ";

            #region 查询条件
            if (agentcomid > 0)
            {
                sql += " and agentcomid=" + agentcomid;
            }
            if (yzdate != "")
            {
                sql += " and convert(varchar(10), confirmtime,120) ='" + yzdate + "'";
            }
            if (pno != "")
            {
                sql += " and pno='" + pno + "'";
            }
            if (comid > 0)
            {
                sql += " and comid=" + comid;
            }
            if (id > 0)
            {
                sql += " and id=" + id;
            }
            //sql += " and pno not in (select pno from agent_asyncsendlog where  isreissue='1' or remark='success')";
            //sql += " and remark!='根据订单号得到分销商订单请求记录失败.'";
            sql += " group by b2b_etcket_logid";
            #endregion
             

            var cmd2 = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            cmd2.PagingCommand1("agent_asyncsendlog", "*", "id desc", "", pagesize, pageindex, "", " id in ("+sql+")");

            List<Agent_asyncsendlog> list = new List<Agent_asyncsendlog>();
            using (var reader2 = cmd2.ExecuteReader())
            {
                while (reader2.Read())
                {
                    list.Add(new Agent_asyncsendlog()
                    {
                        Id = reader2.GetValue<int>("id"),
                        Pno = reader2.GetValue<string>("pno"),
                        Num = reader2.GetValue<int>("num"),
                        Confirmtime = reader2.GetValue<DateTime>("confirmtime"),
                        Sendtime = reader2.GetValue<DateTime>("sendtime"),
                        Issendsuc = reader2.GetValue<int>("issendsuc"),
                        Agentupdatestatus = reader2.GetValue<int>("agentupdatestatus"),
                        Remark = reader2.GetValue<string>("remark"),
                        Agentcomid = reader2.GetValue<int>("agentcomid"),
                        Comid = reader2.GetValue<int>("comid"),
                        Issecondsend = reader2.GetValue<int>("issecondsend"),
                        Platform_req_seq = reader2.GetValue<string>("platform_req_seq"),
                        Request_content = reader2.GetValue<string>("request_content"),
                        Response_content = reader2.GetValue<string>("response_content"),
                        b2b_etcket_logid = reader2.GetValue<int>("b2b_etcket_logid"),
                        isreissue = reader2.GetValue<int>("isreissue"),
                    });
                }
            }
            totalcount = int.Parse(cmd2.Parameters[0].Value.ToString());
            return list;
        }

        internal int IsneedreissueById(int id)
        {
            string sql = "select count(1) from agent_asyncsendlog where  b2b_etcket_logid not in (select b2b_etcket_logid from agent_asyncsendlog where  isreissue='1' or remark like '%success%' or remark like '%根据订单号得到分销商订单请求记录失败%') and id=" + id;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();

            int result = int.Parse(o.ToString());
            if (result == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        internal List<Agent_asyncsendlog> getNoticesByYzlogid(int b2b_etcket_logid, out int totalcount)
        {
            string sql2 = "select * from Agent_asyncsendlog where b2b_etcket_logid=" + b2b_etcket_logid;
            var cmd2 = sqlHelper.PrepareTextSqlCommand(sql2);

            List<Agent_asyncsendlog> list = new List<Agent_asyncsendlog>();
            using (var reader2 = cmd2.ExecuteReader())
            {
                while (reader2.Read())
                {
                    list.Add(new Agent_asyncsendlog()
                    {
                        Id = reader2.GetValue<int>("id"),
                        Pno = reader2.GetValue<string>("pno"),
                        Num = reader2.GetValue<int>("num"),
                        Confirmtime = reader2.GetValue<DateTime>("confirmtime"),
                        Sendtime = reader2.GetValue<DateTime>("sendtime"),
                        Issendsuc = reader2.GetValue<int>("issendsuc"),
                        Agentupdatestatus = reader2.GetValue<int>("agentupdatestatus"),
                        Remark = reader2.GetValue<string>("remark"),
                        Agentcomid = reader2.GetValue<int>("agentcomid"),
                        Comid = reader2.GetValue<int>("comid"),
                        Issecondsend = reader2.GetValue<int>("issecondsend"),
                        Platform_req_seq = reader2.GetValue<string>("platform_req_seq"),
                        Request_content = reader2.GetValue<string>("request_content"),
                        Response_content = reader2.GetValue<string>("response_content"),
                        b2b_etcket_logid = reader2.GetValue<int>("b2b_etcket_logid"),
                        isreissue = reader2.GetValue<int>("isreissue"),
                    });
                }
            }
            totalcount = list.Count;
            return list;
        }

        internal Agent_asyncsendlog getNoticeLog(int noticelogid)
        {
            string sql2 = "select * from Agent_asyncsendlog where id=" + noticelogid;
            var cmd2 = sqlHelper.PrepareTextSqlCommand(sql2);


            using (var reader2 = cmd2.ExecuteReader())
            {
                Agent_asyncsendlog r = null;
                if (reader2.Read())
                {
                    r = new Agent_asyncsendlog()
                    {
                        Id = reader2.GetValue<int>("id"),
                        Pno = reader2.GetValue<string>("pno"),
                        Num = reader2.GetValue<int>("num"),
                        Confirmtime = reader2.GetValue<DateTime>("confirmtime"),
                        Sendtime = reader2.GetValue<DateTime>("sendtime"),
                        Issendsuc = reader2.GetValue<int>("issendsuc"),
                        Agentupdatestatus = reader2.GetValue<int>("agentupdatestatus"),
                        Remark = reader2.GetValue<string>("remark"),
                        Agentcomid = reader2.GetValue<int>("agentcomid"),
                        Comid = reader2.GetValue<int>("comid"),
                        Issecondsend = reader2.GetValue<int>("issecondsend"),
                        Platform_req_seq = reader2.GetValue<string>("platform_req_seq"),
                        Request_content = reader2.GetValue<string>("request_content"),
                        Response_content = reader2.GetValue<string>("response_content"),
                        b2b_etcket_logid = reader2.GetValue<int>("b2b_etcket_logid"),
                        isreissue = reader2.GetValue<int>("isreissue"),
                    };
                }
                return r;
            }


        }
    }
}
