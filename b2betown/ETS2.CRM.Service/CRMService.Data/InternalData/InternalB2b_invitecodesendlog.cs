using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.CRM.Service.CRMService.Data.InternalData
{

    public class InternalB2b_invitecodesendlog
    {
        private SqlHelper sqlHelper;
        public InternalB2b_invitecodesendlog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int Inslog(B2b_invitecodesendlog log)
        {
            string sql = @"INSERT INTO [EtownDB].[dbo].[b2b_invitecodesendlog]
           ([phone]
           ,[smscontent]
           ,[invitecode]
           ,[senduserid]
           ,[sendtime]
           ,[issendsuc]
           ,[isqunfa]
           ,[remark]
           ,[comid])
     VALUES
           (@phone
           ,@smscontent
           ,@invitecode
           ,@senduserid
           ,@sendtime
           ,@issendsuc
           ,@isqunfa
           ,@remark
           ,@comid)";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@phone", log.Phone);
            cmd.AddParam("@smscontent", log.Smscontent);
            cmd.AddParam("@invitecode", log.Invitecode);
            cmd.AddParam("@senduserid", log.Senduserid);
            cmd.AddParam("@sendtime", log.Sendtime);
            cmd.AddParam("@issendsuc", log.Issendsuc);
            cmd.AddParam("@isqunfa", log.Isqunfa);
            cmd.AddParam("@remark", log.Remark);
            cmd.AddParam("@comid", log.Comid);

            return cmd.ExecuteNonQuery();
        }

        internal List<B2b_invitecodesendlog> Getinvitecodesendlog(int comid, int userid, int pageindex, int pagesize, out int totalcount)
        {
            var condition = "";

            //得到员工全部信息 
            B2b_company_manageuser model = new B2bCompanyManagerUserData().GetCompanyUser(userid);
            if (model != null)
            {
                if (model.Channelcompanyid == 0)//公司员工，显示公司群发记录
                {
                    condition = " senduserid in (select id from b2b_company_manageuser where com_id=" + comid + " and  channelcompanyid=0)";
                }
                else //门店员工，显示门店群发记录
                {
                    condition = " senduserid in (select id from b2b_company_manageuser where com_id=" + comid + " and  channelcompanyid=" + model.Channelcompanyid + ")";
                }
            }
            else
            {
                totalcount = 0;
                return null;
            }
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            cmd.PagingCommand1("b2b_invitecodesendlog", "*", "id desc", "", pagesize, pageindex, "", condition);


            List<B2b_invitecodesendlog> list = new List<B2b_invitecodesendlog>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_invitecodesendlog()
                    {
                        Id = reader.GetValue<int>("id"),
                        Phone = reader.GetValue<string>("Phone"),
                        Smscontent = reader.GetValue<string>("Smscontent"),
                        Invitecode = reader.GetValue<string>("Invitecode"),
                        Senduserid = reader.GetValue<int>("Senduserid"),
                        Sendtime = reader.GetValue<DateTime>("Sendtime"),
                        Issendsuc = reader.GetValue<int>("Issendsuc"),
                        Isqunfa = reader.GetValue<int>("Isqunfa"),
                        Remark = reader.GetValue<string>("Remark"),

                        Comid = reader.GetValue<int>("comid")

                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }

        internal B2b_invitecodesendlog GetNoteRecord(string invitecode, int comid)
        {
            string sql = @"SELECT [id]
      ,[phone]
      ,[smscontent]
      ,[invitecode]
      ,[senduserid]
      ,[sendtime]
      ,[issendsuc]
      ,[isqunfa]
      ,[remark]
      ,[comid]
  FROM [EtownDB].[dbo].[b2b_invitecodesendlog] where invitecode=@invitecode and comid=@comid";
            try
            {
                var cmd = this.sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@invitecode", invitecode);
                cmd.AddParam("@comid", comid);

                B2b_invitecodesendlog u = null;
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        u = new B2b_invitecodesendlog()
                        {
                            Id = reader.GetValue<int>("id"),
                            Phone = reader.GetValue<string>("Phone"),
                            Smscontent = reader.GetValue<string>("Smscontent"),
                            Invitecode = reader.GetValue<string>("Invitecode"),
                            Senduserid = reader.GetValue<int>("Senduserid"),
                            Sendtime = reader.GetValue<DateTime>("Sendtime"),
                            Issendsuc = reader.GetValue<int>("Issendsuc"),
                            Isqunfa = reader.GetValue<int>("Isqunfa"),
                            Remark = reader.GetValue<string>("Remark"),

                            Comid = reader.GetValue<int>("comid")

                        };
                    }
                }
                return u;
            }
            catch
            {
                return null;
            }
        }
    }
}
