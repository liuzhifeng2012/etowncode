using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS.Data.SqlHelper;
using ETS.Framework;

namespace ETS2.WeiXin.Service.WeiXinService.Data.InternalData
{
    public class InternalWeiXinBasic
    {
        private SqlHelper sqlHelper;
        public InternalWeiXinBasic(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal WeiXinBasic GetWxBasicByComId(int comid)
        {
            string sql = @"SELECT [id]
      ,[comid]
      ,[domain]
      ,[url]
      ,[token]
      ,[AppId]
      ,[AppSecret]
      ,[attentionautoreply]
      ,[leavemsgautoreply]
      ,weixinno
      ,whethervertify
      ,weixintype
  FROM [EtownDB].[dbo].[WeiXinBasic] where comid=@comid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    WeiXinBasic basic = new WeiXinBasic();
                    basic.Id = reader.GetValue<int>("id");
                    basic.Comid = reader.GetValue<int>("comid");
                    basic.Domain = reader.GetValue<string>("domain");
                    basic.Url = reader.GetValue<string>("url");
                    basic.Token = reader.GetValue<string>("token");
                    basic.AppId = reader.GetValue<string>("AppId");
                    basic.AppSecret = reader.GetValue<string>("AppSecret");
                    basic.Attentionautoreply = reader.GetValue<string>("attentionautoreply");
                    basic.Leavemsgautoreply = reader.GetValue<string>("leavemsgautoreply");
                    basic.Weixinno = reader.GetValue<string>("weixinno");
                    basic.Whethervertify = reader.GetValue<bool>("whethervertify").ToString().ConvertTo<bool>(false);
                    basic.Weixintype = reader.GetValue<int>("weixintype");


                    reader.Close();

                    return basic;
                }
                else
                {
                    return null;
                }
            }
        }
        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateWxBasic";
        internal int Editwxbasic(WeiXinBasic model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);

            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Comid", model.Comid);
            cmd.AddParam("@Domain", model.Domain);
            cmd.AddParam("@Url", model.Url);
            cmd.AddParam("@Token", model.Token);
            cmd.AddParam("@AppId", model.AppId);
            cmd.AddParam("@AppSecret", model.AppSecret);
            cmd.AddParam("@Attentionautoreply", model.Attentionautoreply);
            cmd.AddParam("@Leavemsgautoreply", model.Leavemsgautoreply);
            cmd.AddParam("@whethervertify", model.Whethervertify);


            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }

        internal WeiXinBasic GetWeiXinBasicByDomain(string RequestDomin)
        {
            string sql = @"SELECT [id]
      ,[comid]
      ,[domain]
      ,[url]
      ,[token]
      ,[AppId]
      ,[AppSecret]
      ,[attentionautoreply]
      ,[leavemsgautoreply]
      ,weixinno 
,Weixintype
  FROM  [WeiXinBasic] where domain=@domain";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@domain", RequestDomin);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    WeiXinBasic basic = new WeiXinBasic();
                    basic.Id = reader.GetValue<int>("id");
                    basic.Comid = reader.GetValue<int>("comid");
                    basic.Domain = reader.GetValue<string>("domain");
                    basic.Url = reader.GetValue<string>("url");
                    basic.Token = reader.GetValue<string>("token");
                    basic.AppId = reader.GetValue<string>("AppId");
                    basic.AppSecret = reader.GetValue<string>("AppSecret");
                    basic.Attentionautoreply = reader.GetValue<string>("attentionautoreply");
                    basic.Leavemsgautoreply = reader.GetValue<string>("leavemsgautoreply");
                    basic.Weixinno = reader.GetValue<string>("weixinno");
                    basic.Weixintype = reader.GetValue<int>("Weixintype");
                    reader.Close();

                    return basic;
                }
                else
                {
                    return null;
                }
            }
        }



        internal int ModifyWeiXinID(int id, string weixinoriginalid)
        {
            string sql = "update weixinbasic set weixinno=@weixinno where id=@id ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);
            cmd.AddParam("@weixinno", weixinoriginalid);

            return cmd.ExecuteNonQuery();
        }

        internal int Editwxbasicstep1(int wxbasicid, int weixintype, int comid, string domain, string url, string token)
        {
            if (wxbasicid == 0)
            {
                string sql = "insert into weixinbasic ([comid],[url],[token],[domain],[weixintype],appid,appsecret,attentionautoreply,leavemsgautoreply,weixinno,whethervertify) values " +
                    "(" + comid + ",'" + url + "','" + token + "','" + domain + "'," + weixintype + ",'','','','','',0)";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                return cmd.ExecuteNonQuery();
            }
            else
            {
                string sql = "update weixinbasic set weixintype=" + weixintype + " where id=" + wxbasicid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                return cmd.ExecuteNonQuery();
            }
        }

        internal int Editwxbasicstep2(int wxbasicid, string appid, string appsecret)
        {
            if (wxbasicid == 0)//第二步不可能进行插入操作
            {
                return 0;
            }
            else
            {
                string sql = "update weixinbasic set appid='" + appid + "',AppSecret='" + appsecret + "' where id=" + wxbasicid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                return cmd.ExecuteNonQuery();
            }
        }

        internal int Editwxbasicreply(int wxbasicid, string leavemsgreply, string attentionreply)
        {
            if (wxbasicid == 0)//不可能进行插入操作
            {
                return 0;
            }
            else
            {
                string sql = "update weixinbasic set   [attentionautoreply]='" + attentionreply + "' ,[leavemsgautoreply]='" + leavemsgreply + "' where id=" + wxbasicid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                return cmd.ExecuteNonQuery();
            }
        }

        internal int Editwxbasicstep(int wxbasicid, string appid, string appsecret, int weixintype)
        {
            if (wxbasicid == 0)// 不可能进行插入操作
            {
                return 0;
            }
            else
            {
                try
                {
                    sqlHelper.BeginTrancation();
                    string sql1 = "update weixinbasic set appid='" + appid + "',AppSecret='" + appsecret + "',weixintype=" + weixintype + " where id=" + wxbasicid;
                    var cmd = sqlHelper.PrepareTextSqlCommand(sql1);
                    cmd.ExecuteNonQuery();

                    //判断微信关键词、关注留言 自动回复是否为空，为空这是为默认内容"感谢您的留言，我们将尽快回复..."
                    string sql2 = "select attentionautoreply,leavemsgautoreply from weixinbasic where id=" + wxbasicid;
                    cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                    
                     string leavemsgautoreply = ""; 
                     string attentionautoreply = "";

                    using(var reader=cmd.ExecuteReader())
                    {
                       if(reader.Read())
                       {
                           leavemsgautoreply  =reader.GetValue<string>("leavemsgautoreply");
                           attentionautoreply = reader.GetValue<string>("attentionautoreply");
                       }
                    }
                     
                    if (leavemsgautoreply == "")
                    {
                        string sql3 = "update weixinbasic set leavemsgautoreply='感谢您的留言，我们将尽快回复...'   where id=" + wxbasicid;
                        cmd = sqlHelper.PrepareTextSqlCommand(sql3);
                        cmd.ExecuteNonQuery();
                    }
                    if (attentionautoreply == "")
                    {
                        string sql3 = "update weixinbasic set attentionautoreply='感谢您关注我们！'   where id=" + wxbasicid;
                        cmd = sqlHelper.PrepareTextSqlCommand(sql3);
                        cmd.ExecuteNonQuery();
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
}
