using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.Member.Service.MemberService.Data;

namespace ETS2.WeiXin.Service.WeiXinService.Data.InternalData
{
    public class InternalWxqunfa_log
    {
        private SqlHelper sqlHelper;
        public InternalWxqunfa_log(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int EditLog(Wxqunfa_log log)
        {
            string sql = @"INSERT INTO [EtownDB].[dbo].[wxqunfa_log]
           ([msgtype],[media_id] ,[content],[sendtime],[errcode] ,[errmsg],[msg_id] ,[userid],[channelcompanyid] ,[comid] ,[yearmonth],[yearmonthday],[weixins])
     VALUES
           (@msgtype
           ,@media_id
           ,@content
           ,@sendtime
           ,@errcode
           ,@errmsg
           ,@msg_id
           ,@userid
           ,@channelcompanyid
           ,@comid
           ,@yearmonth
           ,@yearmonthday
           ,@weixins);select @@IDENTITY;";
            if (log.Id > 0)
            {
                sql = @" UPDATE [EtownDB].[dbo].[wxqunfa_log]
                       SET [msgtype] = @msgtype
                          ,[media_id] = @media_id
                          ,[content] = @content
                          ,[sendtime] = @sendtime
                          ,[errcode] = @errcode
                          ,[errmsg] = @errmsg
                          ,[msg_id] = @msg_id
                          ,[userid] = @userid
                          ,[channelcompanyid] = @channelcompanyid
                          ,[comid] = @comid
                          ,[yearmonth] = @yearmonth
                          ,[yearmonthday] = @yearmonthday
                          ,[weixins] = @weixins
                     WHERE id=@id";
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@msgtype", log.Msgtype);
            cmd.AddParam("@media_id", log.Media_id);
            cmd.AddParam("@content", log.Content);
            cmd.AddParam("@sendtime", log.Sendtime);
            cmd.AddParam("@errcode", log.Errcode);
            cmd.AddParam("@errmsg", log.Errmsg);
            cmd.AddParam("@msg_id", log.Msg_id);
            cmd.AddParam("@userid", log.Userid);
            cmd.AddParam("@channelcompanyid", log.Channelcompanyid);
            cmd.AddParam("@comid", log.Comid);
            cmd.AddParam("@yearmonth", log.Yearmonth);
            cmd.AddParam("@yearmonthday", log.Yearmonthday);
            cmd.AddParam("@weixins", log.Weixins);
            if (log.Id > 0)
            {
                cmd.AddParam("@id", log.Id);
            }

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

        internal int GetSendNum(int comid, int channelcompanyid, string yearmonth)
        {
            string sql = "";
            if (channelcompanyid > 0)
            {
                sql = "select count(1) from wxqunfa_log where userid in (select id from b2b_company_manageuser where com_id=" + comid + " and  channelcompanyid=" + channelcompanyid + ") and yearmonth='" + yearmonth + "' and errcode=0";
            }
            else
            {
                sql = "select count(1) from wxqunfa_log where userid in (select id from b2b_company_manageuser where com_id=" + comid + " and  channelcompanyid=0) and yearmonth='" + yearmonth + "' and errcode=0";
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            object o = cmd.ExecuteScalar();
            return o == null ? 0 : int.Parse(o.ToString());
        }

        internal List<Wxqunfa_log> GetQunfalist(int comid, int userid, int pageindex, int pagesize, out int totalcount)
        {
            var condition = "";

            //得到员工全部信息 
            B2b_company_manageuser model = new B2bCompanyManagerUserData().GetCompanyUser(userid);
            if (model != null)
            {
                if (model.Channelcompanyid == 0)//公司员工，显示公司群发记录
                {
                    condition = " userid in (select id from b2b_company_manageuser where com_id=" + comid + " and  channelcompanyid=0)";
                }
                else //门店员工，显示门店群发记录
                {
                    condition = " userid in (select id from b2b_company_manageuser where com_id=" + comid + " and  channelcompanyid=" + model.Channelcompanyid + ")";
                }
            }
            else
            {
                totalcount = 0;
                return null;
            }
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            cmd.PagingCommand1("wxqunfa_log", "*", "id desc", "", pagesize, pageindex, "", condition);


            List<Wxqunfa_log> list = new List<Wxqunfa_log>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Wxqunfa_log()
                    {
                        Id = reader.GetValue<int>("id"),
                        Msgtype = reader.GetValue<string>("msgtype"),
                        Media_id = reader.GetValue<string>("media_id"),
                        Content = reader.GetValue<string>("content"),
                        Sendtime = reader.GetValue<DateTime>("sendtime"),
                        Errcode = reader.GetValue<int>("errcode"),
                        Errmsg = reader.GetValue<string>("errmsg"),
                        Msg_id = reader.GetValue<string>("msg_id"),
                        Userid = reader.GetValue<int>("userid"),
                        Channelcompanyid = reader.GetValue<int>("channelcompanyid"),
                        Comid = reader.GetValue<int>("comid"),
                        Yearmonth = reader.GetValue<string>("yearmonth"),
                        Yearmonthday = reader.GetValue<string>("yearmonthday"),
                        Weixins = reader.GetValue<string>("weixins")
                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }
    }
}
