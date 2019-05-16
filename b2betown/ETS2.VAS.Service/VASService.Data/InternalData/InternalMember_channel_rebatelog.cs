using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.VAS.Service.VASService.Modle;

namespace ETS2.VAS.Service.VASService.Data.InternalData
{
    public class InternalMember_channel_rebatelog
    {
        public SqlHelper sqlHelper;

        public InternalMember_channel_rebatelog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int Editrebatelog(Member_channel_rebatelog m)
        {
            if (m.id == 0)
            {
                string sql = @"INSERT INTO  [Member_channel_rebatelog]
                               ([subdatetime]
                               ,[orderid]
                               ,[proid]
                               ,[proname]
                               ,[ordermoney]
                               ,[rebatemoney]
                               ,[over_money]
                               ,[channelid]
                              
                               ,[payment]
                               ,[payment_type]
                               ,comid)
                         VALUES
                               (@subdatetime 
                               ,@orderid 
                               ,@proid
                               ,@proname 
                               ,@ordermoney 
                               ,@rebatemoney 
                               ,@over_money 
                               ,@channelid  
                               ,@payment 
                               ,@payment_type
                               ,@comid);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@subdatetime", m.subdatetime);
                cmd.AddParam("@orderid", m.orderid);
                cmd.AddParam("@proid", m.proid);
                cmd.AddParam("@proname", m.proname);
                cmd.AddParam("@ordermoney", m.ordermoney);
                cmd.AddParam("@rebatemoney", m.rebatemoney);
                cmd.AddParam("@over_money", m.over_money);
                cmd.AddParam("@channelid", m.channelid);
                cmd.AddParam("@payment", m.payment);
                cmd.AddParam("@payment_type", m.payment_type);
                cmd.AddParam("@comid", m.comid);

                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
            else
            {
                string sql = @"UPDATE  [Member_channel_rebatelog]
                           SET [subdatetime] = @subdatetime 
                              ,[orderid] = @orderid 
                              ,[proid] = @proid 
                              ,[proname] = @proname 
                              ,[ordermoney] = @ordermoney 
                              ,[rebatemoney] = @rebatemoney 
                              ,[over_money] = @over_money 
                              ,[channelid] = @channelid 
                              ,[payment] = @payment 
                              ,[payment_type] = @payment_type 
                              ,comid=@comid
                         WHERE id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id", m.id);
                cmd.AddParam("@subdatetime", m.subdatetime);
                cmd.AddParam("@orderid", m.orderid);
                cmd.AddParam("@proid", m.proid);
                cmd.AddParam("@proname", m.proname);
                cmd.AddParam("@ordermoney", m.ordermoney);
                cmd.AddParam("@rebatemoney", m.rebatemoney);
                cmd.AddParam("@over_money", m.over_money);
                cmd.AddParam("@channelid", m.channelid);
                cmd.AddParam("@payment", m.payment);
                cmd.AddParam("@payment_type", m.payment_type);
                cmd.AddParam("@comid", m.comid);

                cmd.ExecuteNonQuery();
                return m.id;
            }
        }

        internal decimal Getrebatemoney(int channelid)
        {
            string sql = "select rebatemoney from Member_Channel where id=" + channelid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<decimal>("rebatemoney");
                }
                return 0;
            }
        }

        internal int Editchannelrebate(int channelid, decimal overmoney)
        {
            string sql = "update Member_Channel set rebatemoney='" + overmoney + "' where id=" + channelid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal IList<Member_channel_rebatelog> Channelrebatelist(int pageindex, int pagesize, int channelid, string payment, out int totalcount)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            cmd.PagingCommand1("Member_channel_rebatelog", "*", "id desc", "", pagesize, pageindex, "", "channelid=" + channelid + " and payment  in (" + payment + ")");

            List<Member_channel_rebatelog> list = new List<Member_channel_rebatelog>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_channel_rebatelog
                    {
                        id = reader.GetValue<int>("id"),
                        channelid = reader.GetValue<int>("channelid"),
                        orderid = reader.GetValue<int>("orderid"),
                        ordermoney = reader.GetValue<decimal>("ordermoney"),
                        over_money = reader.GetValue<decimal>("over_money"),
                        rebatemoney = reader.GetValue<decimal>("rebatemoney"),
                        payment = reader.GetValue<int>("payment"),
                        payment_type = reader.GetValue<string>("payment_type"),
                        proid = reader.GetValue<string>("proid"),
                        proname = reader.GetValue<string>("proname"),
                        subdatetime = reader.GetValue<DateTime>("subdatetime"),
                        comid = reader.GetValue<int>("comid"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }

        internal Member_channel_rebatelog GetRebateIncomelog(int proid)
        {
            string sql = "select * from Member_channel_rebatelog where proid=" + proid + " and payment=1";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                Member_channel_rebatelog m = null;
                if (reader.Read())
                {
                    m = new Member_channel_rebatelog
                    {
                        id = reader.GetValue<int>("id"),
                        channelid = reader.GetValue<int>("channelid"),
                        orderid = reader.GetValue<int>("orderid"),
                        ordermoney = reader.GetValue<decimal>("ordermoney"),
                        over_money = reader.GetValue<decimal>("over_money"),
                        rebatemoney = reader.GetValue<decimal>("rebatemoney"),
                        payment = reader.GetValue<int>("payment"),
                        payment_type = reader.GetValue<string>("payment_type"),
                        proid = reader.GetValue<string>("proid"),
                        proname = reader.GetValue<string>("proname"),
                        subdatetime = reader.GetValue<DateTime>("subdatetime"),
                        comid = reader.GetValue<int>("comid"),
                    };
                }
                return m;
            }
        }

        internal int Getrebatenum(int comid, string staffphone)
        {
            try
            {
                string sql = "select count(1) as num from Member_channel_rebatelog  where  channelid=(select id from member_channel where com_id=" + comid + " and mobile='" + staffphone + "')";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    int num = 0;
                    if (reader.Read())
                    {
                        num = reader.GetValue<int>("num");
                    }
                    return num;
                }
            }
            catch 
            {
                return 0;
            }
        }
    }
}
