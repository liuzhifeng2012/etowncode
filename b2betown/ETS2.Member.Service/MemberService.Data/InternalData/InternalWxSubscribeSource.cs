using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.Member.Service.MemberService.Model;


namespace ETS2.Member.Service.MemberService.Data.InternalData
{
    public class InternalWxSubscribeSource
    {
        private SqlHelper sqlHelper;
        public InternalWxSubscribeSource(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal List<WxSubscribeSource> GetWXSourcelist(int comid, int wxsourcetype, int pageindex, int pagesize, out int totalcount)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

            string condition = "comid=" + comid;
            if (wxsourcetype == 0)
            {
                condition += "and (sourcetype=1 or sourcetype=2)";
            }
            else
            {
                condition += "and  sourcetype=" + wxsourcetype;
            }
            cmd.PagingCommand1("WxSubscribeSource", "*", "id", "", pagesize, pageindex, "", condition);

            List<WxSubscribeSource> list = new List<WxSubscribeSource>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new WxSubscribeSource
                    {
                        Id = reader.GetValue<int>("id"),
                        Sourcetype = reader.GetValue<int>("sourcetype"),
                        Activityid = reader.GetValue<int>("activityid"),
                        Channelcompanyid = reader.GetValue<int>("channelcompanyid"),
                        Comid = reader.GetValue<int>("comid"),

                    });
                }

            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }

        internal List<WxSubscribeSource> SeledWXSourcelist(int comid, int wxsourcetype, int pageindex, int pagesize, out int totalcount, string onlinestatus = "1")
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

            string condition = "comid=" + comid + " and onlinestatus in (" + onlinestatus + ")";
            if (wxsourcetype == 1)//发布的活动
            {
                condition = "sourcetype=" + wxsourcetype + "  and onlinestatus in (" + onlinestatus + ") and comid=" + comid;
            }
            if (wxsourcetype == 2)//渠道公司
            {
                condition = "sourcetype=" + wxsourcetype + "  and onlinestatus in  (" + onlinestatus + ")  and comid=" + comid;
            }
            if (wxsourcetype == 3)//渠道公司 活动 综合的
            {
                condition = "sourcetype=" + wxsourcetype + "  and onlinestatus in  (" + onlinestatus + ")  and  comid=" + comid;
            }
            cmd.PagingCommand1("WxSubscribeSource", "*", "id", "", pagesize, pageindex, "", condition);

            List<WxSubscribeSource> list = new List<WxSubscribeSource>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new WxSubscribeSource
                    {
                        Id = reader.GetValue<int>("id"),
                        Sourcetype = reader.GetValue<int>("sourcetype"),
                        Activityid = reader.GetValue<int>("activityid"),
                        Channelcompanyid = reader.GetValue<int>("channelcompanyid"),
                        Comid = reader.GetValue<int>("comid"),
                        Ticket = reader.GetValue<string>("ticket"),
                        Title = reader.GetValue<string>("title"),
                        Qrcodeurl = reader.GetValue<string>("qrcodeurl"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Onlinestatus = Convert.IsDBNull(reader.GetValue(9)) == true ? true : reader.GetValue<bool>("onlinestatus")
                    });
                }

            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }

        internal int EditSubscribeSource(WxSubscribeSource source)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand("usp_InsertOrUpdateWxSubscribeSource");

            cmd.AddParam("@id", source.Id);
            cmd.AddParam("@sourcetype", source.Sourcetype);
            cmd.AddParam("@channelcompanyid", source.Channelcompanyid);
            cmd.AddParam("@activityid", source.Activityid);
            cmd.AddParam("@comid", source.Comid);
            cmd.AddParam("@ticket", source.Ticket);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;

        }

        internal int WheherCreated(int comid, int sourcetype, int promoteact, int promotechannelcompany)
        {
            string sql = "";
            if (sourcetype == 1)
            {
                sql = "select count(1) from WxSubscribeSource where sourcetype=1 and  activityid=" + promoteact + " and comid=" + comid;
            }
            else if (sourcetype == 2)
            {
                sql = "select count(1) from WxSubscribeSource where sourcetype=2 and  channelcompanyid=" + promotechannelcompany + " and comid=" + comid;
            }
            else
            {
                sql = "select count(1) from WxSubscribeSource where sourcetype=3 and  activityid=" + promoteact + " and channelcompanyid=" + promotechannelcompany + " and comid=" + comid;
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            return int.Parse(o.ToString());
        }

        internal int WhetherSameExplain(string qrcodename, int comid)
        {
            string sql = "select count(1) from WxSubscribeSource where comid=" + comid + " and title='" + qrcodename + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            return int.Parse(o.ToString());
        }

        internal WxSubscribeSource Getwxqrcode(int qrcodeid)
        {
            string sql = @"SELECT [id]
      ,[sourcetype]
      ,[channelcompanyid]
      ,[activityid]
      ,[comid]
      ,[ticket]
      ,[title]
      ,[qrcodeurl]
      ,[createtime]
      ,Onlinestatus
      ,channelid
      ,productid
  FROM [EtownDB].[dbo].[WxSubscribeSource] where id=@qrcodeid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@qrcodeid", qrcodeid);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new WxSubscribeSource
                    {
                        Id = reader.GetValue<int>("id"),
                        Sourcetype = reader.GetValue<int>("sourcetype"),
                        Activityid = reader.GetValue<int>("activityid"),
                        Channelcompanyid = reader.GetValue<int>("channelcompanyid"),
                        Comid = reader.GetValue<int>("comid"),
                        Ticket = reader.GetValue<string>("ticket"),
                        Title = reader.GetValue<string>("title"),
                        Qrcodeurl = reader.GetValue<string>("qrcodeurl"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Onlinestatus = Convert.IsDBNull(reader.GetValue(9)) == true ? true : reader.GetValue<bool>("onlinestatus"),
                        Channelid = Convert.IsDBNull(reader.GetValue(10)) == true ? 0 : reader.GetValue<int>("channelid"),
                        Productid = reader.GetValue<int>("productid")
                    };
                }

            }
            return null;
        }

        internal List<WxSubscribeSource> GetWXSourcelist2(int comid, string wxsourcetype, int pageindex, int pagesize, string onlinestatus, out int totalcount)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

            string condition = "sourcetype in (" + wxsourcetype + ") and  comid=" + comid + " and onlinestatus in (" + onlinestatus + ")";

            cmd.PagingCommand1("WxSubscribeSource", "*", "id", "", pagesize, pageindex, "", condition);

            List<WxSubscribeSource> list = new List<WxSubscribeSource>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new WxSubscribeSource
                    {
                        Id = reader.GetValue<int>("id"),
                        Sourcetype = reader.GetValue<int>("sourcetype"),
                        Activityid = reader.GetValue<int>("activityid"),
                        Channelcompanyid = reader.GetValue<int>("channelcompanyid"),
                        Comid = reader.GetValue<int>("comid"),
                        Ticket = reader.GetValue<string>("ticket"),
                        Title = reader.GetValue<string>("title"),
                        Qrcodeurl = reader.GetValue<string>("qrcodeurl"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Onlinestatus = Convert.IsDBNull(reader.GetValue(9)) == true ? true : reader.GetValue<bool>("onlinestatus")
                    });
                }

            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }

        internal WxSubscribeDetail GetWXSourceByOpenId(string openid)
        {
            string sql = @"select top 1 * from WxSubscribeDetail     where openid=@openid order by id desc";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@openid", openid);

            WxSubscribeDetail u = null;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    u = new WxSubscribeDetail
                    {
                        Id = reader.GetValue<int>("id"),
                        Openid = reader.GetValue<string>("Openid"),
                        Subscribesourceid = reader.GetValue<int>("Subscribesourceid"),
                        Subscribetime = reader.GetValue<DateTime>("subscribetime"),
                        Comid = reader.GetValue<int>("comid"),
                        Eevent = reader.GetValue<string>("event"),
                        Eventkey = reader.GetValue<string>("eventkey")
                    };
                }

            }
            return u;
        }

        internal bool Ishascreate_paramqrcode(int channelid)
        {
            string sql = "select count(1) from WxSubscribeSource where channelid=" + channelid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            if (int.Parse(o.ToString()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal WxSubscribeSource GetWXSourceById(int subscribesourceid)
        {
            string sql = "select * from WxSubscribeSource where id=" + subscribesourceid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            using (var reader = cmd.ExecuteReader())
            {
                WxSubscribeSource m = null;
                if (reader.Read())
                {
                    m = new WxSubscribeSource
                    {
                        Id = reader.GetValue<int>("id"),
                        Sourcetype = reader.GetValue<int>("sourcetype"),
                        Activityid = reader.GetValue<int>("activityid"),
                        Channelcompanyid = reader.GetValue<int>("channelcompanyid"),
                        viewchannelcompanyid = reader.GetValue<int>("viewchannelcompanyid"),
                        Comid = reader.GetValue<int>("comid"),
                        Ticket = reader.GetValue<string>("ticket"),
                        Title = reader.GetValue<string>("title"),
                        Qrcodeurl = reader.GetValue<string>("qrcodeurl"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Onlinestatus = Convert.IsDBNull(reader.GetValue(9)) == true ? true : reader.GetValue<bool>("onlinestatus"),
                        Channelid = reader.GetValue<int>("channelid"),
                        Productid = reader.GetValue<int>("productid"),

                        qrcodeviewtype = reader.GetValue<int>("qrcodeviewtype"),
                        projectid = reader.GetValue<int>("projectid"),
                        wxmaterialtypeid = reader.GetValue<int>("wxmaterialtypeid"),
                        Wxmaterialid = reader.GetValue<int>("Wxmaterialid"),
                        choujiangactid = reader.GetValue<int>("choujiangactid"),
                    };
                }
                return m;
            }

        }

        internal WxSubscribeSource GetWXSourceByChannelcompanyid(int channelcompanyid)
        {
            string sql = "select * from WxSubscribeSource where   sourcetype=2 and channelcompanyid=" + channelcompanyid + " and channelid=0 and onlinestatus=1  ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            using (var reader = cmd.ExecuteReader())
            {
                WxSubscribeSource m = null;
                if (reader.Read())
                {
                    m = new WxSubscribeSource
                    {
                        Id = reader.GetValue<int>("id"),
                        Sourcetype = reader.GetValue<int>("sourcetype"),
                        Activityid = reader.GetValue<int>("activityid"),
                        Channelcompanyid = reader.GetValue<int>("channelcompanyid"),
                        Comid = reader.GetValue<int>("comid"),
                        Ticket = reader.GetValue<string>("ticket"),
                        Title = reader.GetValue<string>("title"),
                        Qrcodeurl = reader.GetValue<string>("qrcodeurl"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Onlinestatus = Convert.IsDBNull(reader.GetValue(9)) == true ? true : reader.GetValue<bool>("onlinestatus"),
                        Channelid = reader.GetValue<int>("channelid"),
                        Productid = reader.GetValue<int>("productid")
                    };
                }
                return m;
            }
        }

        internal WxSubscribeSource Getchannelwxqrcodebychannelid(int channelid)
        {
            string sql = "select * from WxSubscribeSource where    channelid=" + channelid;

            //判断渠道是否有二维码
            int num = Getqrcodenumbychannelid(channelid);
            if (num == 0)
            {
                int isdefaultchannel = 0;
                int channelcompanyid = 0;
                //如果渠道是默认渠道，则查询门店二维码
                Member_Channel mcompany = new MemberChannelData().GetChannelDetail(channelid);
                if (mcompany != null)
                {
                    if (mcompany.Whetherdefaultchannel == 1)
                    {
                        isdefaultchannel = 1;
                        channelcompanyid = mcompany.Companyid;
                    }
                }

                if (isdefaultchannel == 1)
                {
                    sql = "select * from WxSubscribeSource where channelcompanyid=" + channelcompanyid + " and channelid=0 and channelcompanyid>0";
                }
            }


            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            using (var reader = cmd.ExecuteReader())
            {
                WxSubscribeSource m = null;
                if (reader.Read())
                {
                    m = new WxSubscribeSource
                    {
                        Id = reader.GetValue<int>("id"),
                        Sourcetype = reader.GetValue<int>("sourcetype"),
                        Activityid = reader.GetValue<int>("activityid"),
                        Channelcompanyid = reader.GetValue<int>("channelcompanyid"),
                        viewchannelcompanyid = reader.GetValue<int>("viewchannelcompanyid"),
                        Comid = reader.GetValue<int>("comid"),
                        Ticket = reader.GetValue<string>("ticket"),
                        Title = reader.GetValue<string>("title"),
                        Qrcodeurl = reader.GetValue<string>("qrcodeurl"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Onlinestatus = Convert.IsDBNull(reader.GetValue(9)) == true ? true : reader.GetValue<bool>("onlinestatus"),
                        Channelid = reader.GetValue<int>("channelid"),
                        Productid = reader.GetValue<int>("productid"),

                        qrcodeviewtype = reader.GetValue<int>("qrcodeviewtype"),
                        projectid = reader.GetValue<int>("projectid"),
                        wxmaterialtypeid = reader.GetValue<int>("wxmaterialtypeid"),
                        Wxmaterialid = reader.GetValue<int>("Wxmaterialid"),

                        choujiangactid = reader.GetValue<int>("choujiangactid"),
                    };
                }
                return m;
            }
        }

        private int Getqrcodenumbychannelid(int channelid)
        {
            string sql = "select count(1) from WxSubscribeSource where channelid=" + channelid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            return int.Parse(o.ToString());
        }

        internal WxSubscribeSource GetReserveproVerifywxqrcode(int comid, int qrcodeviewtype)
        {
            string sql = "select * from WxSubscribeSource where    comid=" + comid + " and qrcodeviewtype=" + qrcodeviewtype + " and comid>0 and qrcodeviewtype>0";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            using (var reader = cmd.ExecuteReader())
            {
                WxSubscribeSource m = null;
                if (reader.Read())
                {
                    m = new WxSubscribeSource
                    {
                        Id = reader.GetValue<int>("id"),
                        Sourcetype = reader.GetValue<int>("sourcetype"),
                        Activityid = reader.GetValue<int>("activityid"),
                        Channelcompanyid = reader.GetValue<int>("channelcompanyid"),
                        viewchannelcompanyid = reader.GetValue<int>("viewchannelcompanyid"),
                        Comid = reader.GetValue<int>("comid"),
                        Ticket = reader.GetValue<string>("ticket"),
                        Title = reader.GetValue<string>("title"),
                        Qrcodeurl = reader.GetValue<string>("qrcodeurl"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Onlinestatus = Convert.IsDBNull(reader.GetValue(9)) == true ? true : reader.GetValue<bool>("onlinestatus"),
                        Channelid = reader.GetValue<int>("channelid"),
                        Productid = reader.GetValue<int>("productid"),

                        qrcodeviewtype = reader.GetValue<int>("qrcodeviewtype"),
                        projectid = reader.GetValue<int>("projectid"),
                        wxmaterialtypeid = reader.GetValue<int>("wxmaterialtypeid"),
                        Wxmaterialid = reader.GetValue<int>("Wxmaterialid"),
                    };
                }
                return m;
            }
        }
    }
}
