using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.Member.Service.MemberService.Model;


namespace ETS2.Member.Service.MemberService.Data.InternalData
{
    public class InternalWxSubscribeDetail
    {
        private SqlHelper sqlHelper;
        public InternalWxSubscribeDetail(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int EditSubscribeDetail(WxSubscribeDetail model)
        {
            string sql = @"INSERT INTO [EtownDB].[dbo].[WxSubscribeDetail]
                               ([openid]
                               ,[subscribetime]
                               ,[subscribesourceid]
                               ,[event]
                               ,[eventkey]
                               ,[comid]
                               ,createtime)
                         VALUES
                               (@openid
                               ,@subscribetime
                               ,@subscribesourceid
                               ,@event
                               ,@eventkey
                               ,@comid
                               ,@createtime)";
            if (model.Id > 0)
            {
                sql = @"UPDATE [EtownDB].[dbo].[WxSubscribeDetail]
                           SET [openid] = @openid 
                              ,[subscribetime] = @subscribetime 
                              ,[subscribesourceid] = @subscribesourceid 
                              ,[event] = @event 
                              ,[eventkey] = @eventkey 
                              ,[comid] = @comid 
                              ,createtime=@createtime
                         WHERE id=@id";
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@openid", model.Openid);
            cmd.AddParam("@subscribetime", model.Subscribetime);
            cmd.AddParam("@subscribesourceid", model.Subscribesourceid);
            cmd.AddParam("@event", model.Eevent);
            cmd.AddParam("@eventkey", model.Eventkey);
            cmd.AddParam("@comid", model.Comid);
            cmd.AddParam("@id", model.Id);
            cmd.AddParam("@createtime", model.Createtime);

            return cmd.ExecuteNonQuery();
        }

        internal List<WxSubscribeDetail> GetWxSubscribeList(int comid, int subscribesourceid, int pageindex, int pagesize, out int totalcount)
        {
            try
            {
                var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
                string condition = "";
                if (subscribesourceid == 0)//全部
                {
                    condition = "a.comid=" + comid + " and a.subscribesourceid!=0 and a.event!='unsubscribe'";
                }
                else //扫描带参二维码过来的
                {
                    condition = "a.subscribesourceid=" + subscribesourceid + " and a.event!='unsubscribe'";
                }
                

                cmd.PagingCommand1("WxSubscribeDetail a left join WxSubscribeSource b on a.subscribesourceid=b.id left join WxMemberBasic c on a.openid=c.openid", "a.*,b.channelcompanyid,b.activityid,c.sex,c.city,c.nickname", "a.id desc", "", pagesize, pageindex, "", condition);

                List<WxSubscribeDetail> list = new List<WxSubscribeDetail>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new WxSubscribeDetail
                        {
                            Id = reader.GetValue<int>("id"),
                            Openid = reader.GetValue<string>("Openid"),
                            Subscribesourceid = reader.GetValue<int>("Subscribesourceid"),
                            Subscribetime = reader.GetValue<DateTime>("subscribetime"),
                            Comid = reader.GetValue<int>("comid"),
                            Eevent = reader.GetValue<string>("event"),
                            Eventkey = reader.GetValue<string>("eventkey"),
                            Channelcompanyid = reader.GetValue<int>("channelcompanyid"),
                            Activityid = reader.GetValue<int>("activityid"),
                            Sex = reader.GetValue<int>("sex"),
                            City = reader.GetValue<string>("city"),
                            Nickname = reader.GetValue<string>("nickname")
                        });
                    }

                }
                totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
                return list;
            }
            catch (Exception ex)
            {
                totalcount = 0;
                return null;
            }
        }

        internal WxSubscribeDetail GetWxSubscribeByOpenId(string openid, int comid)
        {
            string sql = @"SELECT top 1  [id]
      ,[openid]
      ,[subscribetime]
      ,[subscribesourceid]
      ,[event]
      ,[eventkey]
      ,[comid]
  FROM [EtownDB].[dbo].[WxSubscribeDetail] where openid=@openid and comid=@comid order by id desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@openid", openid);
            cmd.AddParam("@comid", comid);

            try
            {
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new WxSubscribeDetail
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
                return null;
            }
            catch
            {
                return null;
            }

        }

        internal int GetSubScribeTotalCountBySourceidd(int subscribesourceid)
        {
            string sql = @"select count(1)  from WxSubscribeDetail where subscribesourceid=@id  and event='subscribe' and eventkey!=''";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", subscribesourceid);
            object o = cmd.ExecuteScalar();

            return int.Parse(o.ToString());
        }

        internal int GetScanTotalCount(int subscribesourceid)
        {
            string sql = "select count(1)  from WxSubscribeDetail where subscribesourceid=" + subscribesourceid + " and event in ('subscribe','SCAN')  and eventkey!=''";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();

            return int.Parse(o.ToString());
        }

        internal string GetLasterScanTime(int subscribesourceid)
        {
            try
            {
                string sql = "select top 1  subscribetime  from WxSubscribeDetail where subscribesourceid=" + subscribesourceid + "  order by subscribetime desc";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null ? "---" : DateTime.Parse(o.ToString()).ToString("yyyy-MM-dd");
            }
            catch 
            {
                sqlHelper.Dispose();
               return  "---";
            }
        }
        /// <summary>
        /// 获得当前 公司/活动 下所有二维码关注微信总数
        /// </summary>
        /// <param name="wxsourceid"></param>
        /// <returns></returns>
        internal int GetWxTotal2(string id, string isqudao, string issuetype = "0,1")
        {
            //默认关注公司
            string sql = @"select count(1) from b2b_crm where weixin!='' and idcard in 
                            (
                              select cardcode from member_card where issuecard in
                              (select id from member_channel where companyid=@id)
                            )
                            and whetherwxfocus=1";
            if (issuetype == "1")
            {
                sql = @"select count(1) from WxSubscribeDetail 
                        where subscribesourceid in(select id from WxSubscribeSource where channelcompanyid= @id)
                        and event='subscribe' and eventkey!='' ";
            }
            if (isqudao == "0")//关注活动
            {
                sql = @"select count(1)  from WxSubscribeDetail 
                    where subscribesourceid in (select id from WxSubscribeSource where activityid=@id ) 
                    and event='subscribe' and eventkey!=''";
            }
            if(id=="0")//总部员工
            {
                sql = @"select count(1) from WxSubscribeDetail 
                        where subscribesourceid in(select id from WxSubscribeSource where channelcompanyid=0 and channelid>0)
                        and event='subscribe' and eventkey!='' ";
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);
            object o = cmd.ExecuteScalar();

            return o == null ? 0 : int.Parse(o.ToString());
        }
        /// <summary>
        /// 获得当前 公司/活动 下所有二维码扫码总数
        /// </summary>
        /// <param name="wxsourceid"></param>
        /// <returns></returns>
        internal int GetScanTotal2(string id, string isqudao)
        {
            string sql = @"select count(1) from WxSubscribeDetail 
                        where subscribesourceid in(select id from WxSubscribeSource where channelcompanyid= @id)
                        and event in('subscribe','SCAN') and eventkey!=''";
            if(id=="0")//总公司员工关注数量
            {
                sql = @"select count(1) from WxSubscribeDetail 
                        where subscribesourceid in(select id from WxSubscribeSource where channelcompanyid=0 and channelid>0)
                        and event in('subscribe','SCAN') and eventkey!=''";
            }
            if (isqudao == "0")//关注活动
            {
                sql = @"select count(1) from WxSubscribeDetail 
                        where subscribesourceid in(select id from WxSubscribeSource where activityid= @id)
                        and event in('subscribe','SCAN') and eventkey!=''";
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);
            object o = cmd.ExecuteScalar();

            return o == null ? 0 : int.Parse(o.ToString());
        }
        /// <summary>
        /// 获得当前 公司/活动 下所有二维码取消关注总数
        /// </summary>
        /// <param name="wxsourceid"></param>
        /// <returns></returns>
        internal int GetQxWxTotal2(string id, string isqudao)
        {
            //默认关注公司
            string sql = @"select count(1) from WxSubscribeDetail 
                        where subscribesourceid in(select id from WxSubscribeSource where channelcompanyid= @id)
                        and event='unsubscribe'";
            if (isqudao == "0")//关注活动
            {
                sql = @"select count(1)  from WxSubscribeDetail 
                    where subscribesourceid in (select id from WxSubscribeSource where activityid=@id ) 
                    and event='unsubscribe' ";
            }
            if(id=="0")//公司员工
            {
                sql = @"select count(1) from WxSubscribeDetail 
                        where subscribesourceid in(select id from WxSubscribeSource where channelcompanyid=0 and channelid>0)
                        and event='unsubscribe'";
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);
            object o = cmd.ExecuteScalar();

            return o == null ? 0 : int.Parse(o.ToString());
        }

        internal int GetQxWxTotal(int wxsourceid)
        {
            string sql = @"select count(1)  from WxSubscribeDetail where subscribesourceid=@id  and event='unsubscribe' ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", wxsourceid);
            object o = cmd.ExecuteScalar();

            return int.Parse(o.ToString());
        }
        /*得到门店关注总数*/
        internal int GetMd_Subscribenum(int comid)
        {
            string sql = @"select count(1) from b2b_crm where weixin!='' and idcard in 
                            (
                              select cardcode from member_card where issuecard in
                              (select id from member_channel where   companyid in (select id from Member_Channel_company where  Issuetype=0 and com_id=@comid   ))
                            )
                            and whetherwxfocus=1";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            cmd.AddParam("@comid", comid);
            object o = cmd.ExecuteScalar();

            return int.Parse(o.ToString());
        }

        internal int GetReqnum(string weixin, string cretime, string eventname)
        {
            string sql = "select count(1) from WxSubscribeDetail    where event='" + eventname + "' and  openid='" + weixin + "' and createtime='" + cretime + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            return o == null ? 0 : int.Parse(o.ToString());

        }
    }
}
