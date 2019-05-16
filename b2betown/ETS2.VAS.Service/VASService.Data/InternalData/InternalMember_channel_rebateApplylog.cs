using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.VAS.Service.VASService.Modle;

namespace ETS2.VAS.Service.VASService.Data.InternalData
{
    public class InternalMember_channel_rebateApplylog
    {
        public SqlHelper sqlHelper;
        public InternalMember_channel_rebateApplylog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal int Insrebateapplylog(Member_channel_rebateApplylog m)
        {
            string sql = @"INSERT INTO  [Member_channel_rebateApplylog]
                               ( [applytime]
                               ,[applytype]
                               ,[applydetail]
                               ,[applymoney]
                               ,[channelid]
                               ,[operstatus],comid)
                         VALUES
                               ( 
                                @applytime 
                               ,@applytype 
                               ,@applydetail 
                               ,@applymoney 
                               ,@channelid 
                               ,@operstatus,@comid)";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@applytime", m.applytime);
            cmd.AddParam("@applytype", m.applytype);
            cmd.AddParam("@applydetail", m.applydetail);
            cmd.AddParam("@applymoney", m.applymoney);
            cmd.AddParam("@channelid", m.channelid);
            cmd.AddParam("@operstatus", m.operstatus);
            cmd.AddParam("@comid", m.comid);

            return cmd.ExecuteNonQuery();
        }

        internal IList<Member_channel_rebateApplylog> Channelrebateapplylist(int pageindex, int pagesize, int channelid, string operstatus, out int totalcount)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            cmd.PagingCommand1("Member_channel_rebateApplylog", "*", "id desc", "", pagesize, pageindex, "", "channelid=" + channelid + " and operstatus  in (" + operstatus + ")");

            List<Member_channel_rebateApplylog> list = new List<Member_channel_rebateApplylog>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_channel_rebateApplylog
                    {
                        id = reader.GetValue<int>("id"),
                        channelid = reader.GetValue<int>("channelid"),
                        applytime = reader.GetValue<DateTime>("applytime"),
                        applytype = reader.GetValue<string>("applytype"),
                        applydetail = reader.GetValue<string>("applydetail"),
                        applymoney = reader.GetValue<decimal>("applymoney"),
                        operstatus = reader.GetValue<int>("operstatus"),
                        comid = reader.GetValue<int>("comid"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }

        internal IList<Member_channel_rebateApplylog> Channelrebateapplyalllist(int pageindex, int pagesize, int comid, string operstatus, out int totalcount)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            cmd.PagingCommand1("Member_channel_rebateApplylog", "*", "id desc", "", pagesize, pageindex, "", "comid=" + comid + " and operstatus  in (" + operstatus + ")");

            List<Member_channel_rebateApplylog> list = new List<Member_channel_rebateApplylog>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_channel_rebateApplylog
                    {
                        id = reader.GetValue<int>("id"),
                        channelid = reader.GetValue<int>("channelid"),
                        applytime = reader.GetValue<DateTime>("applytime"),
                        applytype = reader.GetValue<string>("applytype"),
                        applydetail = reader.GetValue<string>("applydetail"),
                        applymoney = reader.GetValue<decimal>("applymoney"),
                        operstatus = reader.GetValue<int>("operstatus"),
                        comid = reader.GetValue<int>("comid"),

                        opertime = reader.GetValue<DateTime>("opertime"),
                        opertor = reader.GetValue<int>("opertor"),
                        operremark = reader.GetValue<string>("operremark"),
                        zhuanzhangsucimg = reader.GetValue<int>("zhuanzhangsucimg"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }

        internal int Confirmcompletedakuan(int id, int operstatus, int opertor, string operremark, int zhuanzhangsucimg)
        {
            string sql = "update Member_channel_rebateApplylog set operstatus=" + operstatus + ",opertor=" + opertor + ",operremark='" + operremark + "',zhuanzhangsucimg=" + zhuanzhangsucimg + " where id=" + id;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal decimal Getrebateapplytotal(int comid, string staffphone)
        {
            try
            {
                string sql = "select sum(applymoney) as totalapplymoney from Member_channel_rebateApplylog where channelid=(select id from Member_Channel where com_id=" + comid + " and mobile='" + staffphone + "')";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    decimal r = 0;
                    if (reader.Read())
                    {
                        r = reader.GetValue<decimal>("totalapplymoney");
                    }
                    return r;
                }
            }
            catch 
            {
                return 0;
            }
        }

        internal decimal Getrebatehastixian(int comid, string staffphone)
        {
            try
            {
                string sql = "select sum(applymoney) as totalapplymoney from Member_channel_rebateApplylog where operstatus=1 and  channelid=(select id from Member_Channel where com_id=" + comid + " and mobile='" + staffphone + "')";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    decimal r = 0;
                    if (reader.Read())
                    {
                        r = reader.GetValue<decimal>("totalapplymoney");
                    }
                    return r;
                }
            }
            catch 
            {
                return 0;
            }
        }

        internal decimal Getrebatenottixian(int comid, string staffphone)
        {
            try
            {
                string sql = "select sum(applymoney) as totalapplymoney from Member_channel_rebateApplylog where operstatus=0 and  channelid=(select id from Member_Channel where com_id=" + comid + " and mobile='" + staffphone + "')";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    decimal r = 0;
                    if (reader.Read())
                    {
                        r = reader.GetValue<decimal>("totalapplymoney");
                    }
                    return r;
                }
            }
            catch
            {
                return 0;
            }
        }
    }
}
