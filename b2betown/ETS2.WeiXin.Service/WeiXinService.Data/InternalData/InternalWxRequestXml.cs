using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Member.Service.MemberService.Data;

namespace ETS2.WeiXin.Service.WeiXinService.Data.InternalData
{
    public class InternalWxRequestXml
    {
        private SqlHelper sqlHelper;
        public InternalWxRequestXml(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateWxRequestXml";
        internal int InsertOrUpdate(RequestXML model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);

            cmd.AddParam("@id", model.Id);
            cmd.AddParam("@ToUserName", model.ToUserName);
            cmd.AddParam("@FromUserName", model.FromUserName);
            cmd.AddParam("@CreateTime", model.CreateTime);
            cmd.AddParam("@MsgType", model.MsgType);
            cmd.AddParam("@Content", model.Content);
            cmd.AddParam("@Location_X", model.Location_X);
            cmd.AddParam("@Location_Y", model.Location_Y);
            cmd.AddParam("@Scale", model.Scale);
            cmd.AddParam("@Label", model.Label);
            cmd.AddParam("@PicUrl", model.PicUrl);
            cmd.AddParam("@PostStr", model.PostStr);
            cmd.AddParam("@CreateTimeFormat", model.CreateTimeFormat);
            cmd.AddParam("@Sendstate", model.Sendstate);

            //新增加的参数
            cmd.AddParam("@MediaId", model.MediaId);
            cmd.AddParam("@Format", model.Format);
            cmd.AddParam("@Recognition", model.Recognition);
            cmd.AddParam("@ThumbMediaId", model.ThumbMediaId);
            cmd.AddParam("@Title", model.Title);
            cmd.AddParam("@Description", model.Description);
            cmd.AddParam("@Url", model.Url);
            cmd.AddParam("@MsgId", model.MsgId);
            cmd.AddParam("@ContentType", model.ContentType);
            cmd.AddParam("@Comid", model.Comid);

            if (model.Manageuserid==null)
            {
                model.Manageuserid = 0;
            }
            if(model.Manageusername==null)
            {
                model.Manageusername = "";
            }
            cmd.AddParam("@ManageUserId", model.Manageuserid);
            cmd.AddParam("@ManageUserName", model.Manageusername);


            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }




        internal List<RequestXML> GetWxErrSendMsgList(int comid, string FromUserName, out int totalcount)
        {
            int count = 0;
            DateTime CreateTimeFormat = DateTime.Now.AddDays(-7);//一周以内的 条数
            //最多20条
            string sql = @"select top 20 * from WxRequestXml where sendstate=0 and createtimeformat >@CreateTimeFormat and comid=@comid and tousername=@fromusername order by id desc";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@CreateTimeFormat", CreateTimeFormat);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@fromusername", FromUserName);


            List<RequestXML> list = new List<RequestXML>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {

                    RequestXML model = new RequestXML();
                    model.Id = reader.GetValue<int>("Id");
                    model.ToUserName = reader.GetValue<string>("ToUserName");
                    model.FromUserName = reader.GetValue<string>("FromUserName");
                    model.CreateTime = reader.GetValue<string>("CreateTime");
                    model.MsgType = reader.GetValue<string>("MsgType");
                    model.Content = reader.GetValue<string>("Content");
                    model.Location_X = reader.GetValue<string>("Location_X");
                    model.Location_Y = reader.GetValue<string>("Location_Y");
                    model.Scale = reader.GetValue<string>("Scale");
                    model.Label = reader.GetValue<string>("Label");
                    model.PicUrl = reader.GetValue<string>("PicUrl");
                    model.PostStr = reader.GetValue<string>("PostStr");
                    model.CreateTimeFormat = reader.GetValue<DateTime>("CreateTimeFormat").ToString().ConvertTo<DateTime>(DateTime.Parse("1900-01-01 00:00:00"));

                    list.Add(model);
                    count++;
                }
            }
            totalcount = count;
            return list;
        }

        internal int UpWxErrSendMsgList(int id)
        {

            string sql = @"update WxRequestXml set sendstate=1 where id=@id";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);

            return cmd.ExecuteNonQuery();
          
        }

        
        internal List<RequestXML> GetWxSendMsgListByComid(int userid, int comid, int pageindex, int pagesize, out int totalcount)
        {
            var Table = "WxMemberBasic c right join   b2b_crm a " +
                          " right join " +
                          " ( select * from WxRequestXml where id in " +
                              " (select idd from " +
                                 " ( select max(a.id) as idd,fromusername from WxRequestXml a " +
                                "  where  a.comid=" + comid + " and a.contenttype=1 and a.createtimeformat between CONCAT(Convert(varchar(10), dateadd(dd,-1,getdate()),121),' 00:00:00')   and CONCAT(Convert(varchar(10),getdate(),121),' 23:59:59') group by fromusername " +
                              "    ) as c " +
                            "    ) " +
                          " ) as b " +
                        " on a.weixin=b.fromusername on c.openid=a.weixin ";
            var Column = "a.id as aid,a.name as aname,a.phone as aphone,a.weixin as aweixin,b.*,c.headimgurl,c.province,c.city,c.nickname,c.sex";
            var OrderColumn = " b.createtimeformat desc";
            var GroupColumn = "";
            var PageSize = pagesize;
            var CurrentPage = pageindex;
            var Group = "";
            var Condition = "";

            Member_Channel_company channelcom = new MemberChannelcompanyData().GetChannelCompanyByUserId(userid);
            if (channelcom == null)//总公司账户
            {
                Condition = "";
            }
            else
            {
                if (channelcom.Issuetype == 0)//内部门市
                {
                    Condition = "a.idcard in (select cardcode from member_card where issuecard in (select id from member_channel where companyid  =" + channelcom.Id + "))";
                }
                else//合作公司
                {
                    Condition = "a.weixin in (select openid from WxSubscribeDetail where subscribesourceid in (select id from WxSubscribeSource where channelcompanyid =" + channelcom.Id + "))";
                }
            }



            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");



            cmd.PagingCommand1(Table, Column, OrderColumn, GroupColumn, PageSize, CurrentPage, Group, Condition);


            List<RequestXML> list = new List<RequestXML>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    RequestXML model = new RequestXML();
                    model.Id = reader.GetValue<int>("Id");
                    model.ToUserName = reader.GetValue<string>("ToUserName");
                    model.FromUserName = reader.GetValue<string>("FromUserName");
                    model.CreateTime = reader.GetValue<string>("CreateTime");
                    model.MsgType = reader.GetValue<string>("MsgType");
                    model.Content = reader.GetValue<string>("Content");
                    model.Location_X = reader.GetValue<string>("Location_X");
                    model.Location_Y = reader.GetValue<string>("Location_Y");
                    model.Scale = reader.GetValue<string>("Scale");
                    model.Label = reader.GetValue<string>("Label");
                    model.PicUrl = reader.GetValue<string>("PicUrl");
                    model.PostStr = reader.GetValue<string>("PostStr");
                    model.CreateTimeFormat = reader.GetValue<DateTime>("CreateTimeFormat").ToString().ConvertTo<DateTime>(DateTime.Parse("1900-01-01 00:00:00"));

                    model.MediaId = reader.GetValue<string>("MediaId");
                    model.Format = reader.GetValue<string>("Format");
                    model.Recognition = reader.GetValue<string>("Recognition");
                    model.ThumbMediaId = reader.GetValue<string>("ThumbMediaId");
                    model.Title = reader.GetValue<string>("Title");
                    model.Description = reader.GetValue<string>("Description");
                    model.Url = reader.GetValue<string>("Url");
                    model.MsgId = reader.GetValue<string>("MsgId");
                    model.ContentType = reader.GetValue<bool>("ContentType");
                    model.Comid = reader.GetValue<int>("Comid");


                    model.Crmid = reader.GetValue<int>("aid");
                    model.Crmname = reader.GetValue<string>("aname");
                    model.Crmphone = reader.GetValue<string>("aphone");
                    model.Manageuserid = reader.GetValue<int>("manageuserid").ToString().ConvertTo<int>(0);
                    model.Manageusername = reader.GetValue<string>("manageusername").ConvertTo<string>("");


                    model.Headimgurl = reader.GetValue<string>("headimgurl").ConvertTo<string>("/images/defaultThumb.png");
                    model.City = reader.GetValue<string>("city").ConvertTo<string>("");
                    model.Province = reader.GetValue<string>("province").ConvertTo<string>("");
                    model.Nickname = reader.GetValue<string>("nickname").ConvertTo<string>("");
                    model.Sex = reader.GetValue<int>("sex").ToString().ConvertTo<int>(0);

                    list.Add(model);
                }
            }
            //totalcount = list.Count;
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }

        internal bool JudgeWhetherReply(DateTime CreateTimeFormat, string FromUserName, int comid)
        {
            string sql = @"select count(1) from WxRequestXml where manageuserid>0 and createtimeformat >@CreateTimeFormat and comid=@comid and tousername=@fromusername";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@CreateTimeFormat", CreateTimeFormat);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@fromusername", FromUserName);


            object o = cmd.ExecuteScalar();
            if (o.ToString().ConvertTo<int>(0) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        internal List<RequestXML> GetWxSendMsgListByFromUser(int comid, string fromusername, int pageindex, int pagesize, out int totalcount)
        {
            //            string sql = @" select a.id as aid, a.name as aname,a.phone as aphone,a.weixin as aweixin,b.* from b2b_crm a 
            //                            right join 
            //                            ( select * from WxRequestXml where ((fromusername=@fromusername and contenttype=1 )
            //                                 or (tousername=@fromusername and contenttype=0)) and createtimeformat between CONCAT(Convert(varchar(10), dateadd(dd,-4,getdate()),121),' 00:00:00')   and CONCAT(Convert(varchar(10),getdate(),121),' 23:59:59')  
            //                            ) as b
            //                            on a.weixin=b.fromusername
            //                            order by  b.createtimeformat desc ";
            //            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            //            cmd.AddParam("@comid", comid);
            //            cmd.AddParam("@fromusername", fromusername);

            var Table = "WxMemberBasic c right join b2b_crm a " +
                           "  right join  " +
                           "  ( select * from WxRequestXml where ((fromusername='" + fromusername + "' ) " +
                              "    or (tousername='" + fromusername + "' )) and createtimeformat between CONCAT(Convert(varchar(10), dateadd(dd,-4,getdate()),121),' 00:00:00')   and CONCAT(Convert(varchar(10),getdate(),121),' 23:59:59')   " +
                            " ) as b " +
                          " on a.weixin=b.fromusername on c.openid=a.weixin";
            var Column = "a.id as aid, a.name as aname,a.phone as aphone,a.weixin as aweixin,b.*,c.headimgurl,c.province,c.city,c.nickname,c.sex ";
            var OrderColumn = "b.createtimeformat desc";
            var GroupColumn = "";
            var PageSize = pagesize;
            var CurrentPage = pageindex;
            var Group = "";
            var Condition = "";

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");



            cmd.PagingCommand1(Table, Column, OrderColumn, GroupColumn, PageSize, CurrentPage, Group, Condition);
            List<RequestXML> list = new List<RequestXML>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    RequestXML model = new RequestXML();
                    model.Id = reader.GetValue<int>("Id");
                    model.ToUserName = reader.GetValue<string>("ToUserName");
                    model.FromUserName = reader.GetValue<string>("FromUserName");
                    model.CreateTime = reader.GetValue<string>("CreateTime");
                    model.MsgType = reader.GetValue<string>("MsgType");
                    model.Content = reader.GetValue<string>("Content");
                    model.Location_X = reader.GetValue<string>("Location_X");
                    model.Location_Y = reader.GetValue<string>("Location_Y");
                    model.Scale = reader.GetValue<string>("Scale");
                    model.Label = reader.GetValue<string>("Label");
                    model.PicUrl = reader.GetValue<string>("PicUrl");
                    model.PostStr = reader.GetValue<string>("PostStr");
                    model.CreateTimeFormat = reader.GetValue<DateTime>("CreateTimeFormat").ToString().ConvertTo<DateTime>(DateTime.Parse("1900-01-01 00:00:00"));

                    model.MediaId = reader.GetValue<string>("MediaId");
                    model.Format = reader.GetValue<string>("Format");
                    model.Recognition = reader.GetValue<string>("Recognition");
                    model.ThumbMediaId = reader.GetValue<string>("ThumbMediaId");
                    model.Title = reader.GetValue<string>("Title");
                    model.Description = reader.GetValue<string>("Description");
                    model.Url = reader.GetValue<string>("Url");
                    model.MsgId = reader.GetValue<string>("MsgId");
                    model.ContentType = reader.GetValue<bool>("ContentType");
                    model.Comid = reader.GetValue<int>("Comid");

                    model.Crmid = reader.GetValue<int>("aid");
                    model.Crmname = reader.GetValue<string>("aname");
                    model.Crmphone = reader.GetValue<string>("aphone");
                    model.Manageuserid = reader.GetValue<int>("manageuserid").ToString().ConvertTo<int>(0);
                    model.Manageusername = reader.GetValue<string>("manageusername").ConvertTo<string>("");

                    model.Headimgurl = reader.GetValue<string>("headimgurl").ConvertTo<string>("/images/defaultThumb.png");
                    model.City = reader.GetValue<string>("city").ConvertTo<string>("");
                    model.Province = reader.GetValue<string>("province").ConvertTo<string>("");
                    model.Nickname = reader.GetValue<string>("nickname").ConvertTo<string>("");
                    model.Sex = reader.GetValue<int>("sex").ToString().ConvertTo<int>(0);

                    list.Add(model);
                }
            }
            //totalcount = list.Count;
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }

        internal string GetLasterReplyTime(DateTime CreateTimeFormat, string FromUserName, int comid)
        {
            string sql = @"select top 1 * from WxRequestXml where createtimeformat >@CreateTimeFormat and comid=@comid and tousername=@fromusername order by createtimeformat desc";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@CreateTimeFormat", CreateTimeFormat);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@fromusername", FromUserName);

            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return reader.GetValue<DateTime>("createtimeformat").ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                return "";
            }

            //object o = cmd.ExecuteScalar();
            //if (o.ToString().ConvertTo<int>(0) == 0)
            //{
            //    return false;
            //}
            //else
            //{
            //    return true;
            //}
        }

        internal bool JudgeWhetherRenZheng(int comid)
        {

            try
            {
                string sql = "select weixintype from weixinbasic where comid=" + comid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                if (o.ToString() == "4")//认证服务号含有高级接口更能
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                sqlHelper.Dispose();
                return false;
            }
            //if (comid == 101 || comid == 1125)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

        internal bool GetWhetherSendAutoReply(string FromUserName, string defaultret)
        {
            string sql = "select count(1) from WxRequestXml where tousername='" + FromUserName + "' and content='" + defaultret + "' and createtimeformat>dateadd(hh,-48,getdate())";
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

        internal int AdjustQrcodeStatus(int sourceid, int onlinestatus)
        {
            string sql = "update WxSubscribeSource set onlinestatus=" + onlinestatus + " where id=" + sourceid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            return cmd.ExecuteNonQuery();

        }

        internal int JadgeMsg_isReceived(string fromusername, string createtime)
        {
            string sql = "select count(1) from  WxRequestXml where  FromUserName='" + fromusername + "' and createtime ='" + createtime + "' ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            if (int.Parse(o.ToString()) > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }


        //客户端看最新咨询信息
        internal List<RequestXML> Getzixunlog(int comid, int pageindex, int pagesize, string userweixin, string guwenweixin,string key, out int totalcount)
        {

            var Table = "WxMemberBasic c right join b2b_crm a " +
                           "  right join  " +
                           "  ( select * from WxRequestXml where msgtype in ('voice','text') and not fromusername like 'gh_%' " +
                            " ) as b " +
                          " on a.weixin=b.fromusername on c.openid=a.weixin";
            var Column = "a.id as aid, a.name as aname,a.phone as aphone,a.weixin as aweixin,b.*,c.headimgurl as cheadimgurl,c.province,c.city,c.nickname,c.sex ";
            var OrderColumn = "b.createtimeformat desc";
            var GroupColumn = "";
            var PageSize = pagesize;
            var CurrentPage = pageindex;
            var Group = "";
            var Condition = "c.comid=" + comid + " and  ((tousername='" + userweixin + "' and fromusername='" + guwenweixin + "') or (tousername='" + guwenweixin + "' and fromusername='" + userweixin + "'))";//

            if (key != "") {
                Condition += " and b.id>"+key;
            }


            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

            cmd.PagingCommand1(Table, Column, OrderColumn, GroupColumn, PageSize, CurrentPage, Group, Condition);
            List<RequestXML> list = new List<RequestXML>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    RequestXML model = new RequestXML();
                    model.Id = reader.GetValue<int>("Id");
                    model.ToUserName = reader.GetValue<string>("ToUserName");
                    model.FromUserName = reader.GetValue<string>("FromUserName");
                    model.CreateTime = reader.GetValue<string>("CreateTime");
                    model.MsgType = reader.GetValue<string>("MsgType");
                    model.Content = reader.GetValue<string>("Content");
                    model.Location_X = reader.GetValue<string>("Location_X");
                    model.Location_Y = reader.GetValue<string>("Location_Y");
                    model.Scale = reader.GetValue<string>("Scale");
                    model.Label = reader.GetValue<string>("Label");
                    model.PicUrl = reader.GetValue<string>("PicUrl");
                    model.PostStr = reader.GetValue<string>("PostStr");
                    model.CreateTimeFormat = reader.GetValue<DateTime>("CreateTimeFormat").ToString().ConvertTo<DateTime>(DateTime.Now);

                    model.MediaId = reader.GetValue<string>("MediaId");
                    model.Format = reader.GetValue<string>("Format");
                    model.Recognition = reader.GetValue<string>("Recognition");
                    model.ThumbMediaId = reader.GetValue<string>("ThumbMediaId");
                    model.Title = reader.GetValue<string>("Title");
                    model.Description = reader.GetValue<string>("Description");
                    model.Url = reader.GetValue<string>("Url");
                    model.MsgId = reader.GetValue<string>("MsgId");
                    model.ContentType = reader.GetValue<bool>("ContentType");
                    model.Comid = reader.GetValue<int>("Comid");

                    model.Crmid = reader.GetValue<int>("aid");
                    model.Crmname = reader.GetValue<string>("aname");
                    model.Crmphone = reader.GetValue<string>("aphone");
                    model.Manageuserid = reader.GetValue<int>("manageuserid").ToString().ConvertTo<int>(0);
                    model.Manageusername = reader.GetValue<string>("manageusername").ConvertTo<string>("");
                    model.FromUserName = reader.GetValue<string>("FromUserName").ConvertTo<string>("");

                    model.Headimgurl = reader.GetValue<string>("cheadimgurl").ConvertTo<string>("/images/defaultThumb.png");
                    model.City = reader.GetValue<string>("city").ConvertTo<string>("");
                    model.Province = reader.GetValue<string>("province").ConvertTo<string>("");
                    model.Nickname = reader.GetValue<string>("nickname").ConvertTo<string>("");
                    model.Sex = reader.GetValue<int>("sex").ToString().ConvertTo<int>(0);

                    list.Add(model);
                }
            }
            //totalcount = list.Count;
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }



        //客户端看最新咨询信息
        internal List<RequestXML> GetWxSendMsgListByTop5(int comid, int pageindex, int pagesize, out int totalcount)
        {

            var Table = "WxMemberBasic c right join b2b_crm a " +
                           "  right join  " +
                           "  ( select * from WxRequestXml where msgtype in ('voice','text') and not fromusername like 'gh_%' and createtimeformat between CONCAT(Convert(varchar(10), dateadd(dd,-4,getdate()),121),' 00:00:00')   and CONCAT(Convert(varchar(10),getdate(),121),' 23:59:59')   " +
                            " ) as b " +
                          " on a.weixin=b.fromusername on c.openid=a.weixin";
            var Column = "a.id as aid, a.name as aname,a.phone as aphone,a.weixin as aweixin,b.*,c.headimgurl,c.province,c.city,c.nickname,c.sex ";
            var OrderColumn = "b.createtimeformat desc";
            var GroupColumn = "";
            var PageSize = pagesize;
            var CurrentPage = pageindex;
            var Group = "";
            var Condition = "c.comid=" + comid + " and fromusername in (select weixin from b2b_crm where idcard in (select cardcode from member_card where issuecard in (select id from member_channel where issuetype=0)))";//对非顾问的排除

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");



            cmd.PagingCommand1(Table, Column, OrderColumn, GroupColumn, PageSize, CurrentPage, Group, Condition);
            List<RequestXML> list = new List<RequestXML>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    RequestXML model = new RequestXML();
                    model.Id = reader.GetValue<int>("Id");
                    model.ToUserName = reader.GetValue<string>("ToUserName");
                    model.FromUserName = reader.GetValue<string>("FromUserName");
                    model.CreateTime = reader.GetValue<string>("CreateTime");
                    model.MsgType = reader.GetValue<string>("MsgType");
                    model.Content = reader.GetValue<string>("Content");
                    model.Location_X = reader.GetValue<string>("Location_X");
                    model.Location_Y = reader.GetValue<string>("Location_Y");
                    model.Scale = reader.GetValue<string>("Scale");
                    model.Label = reader.GetValue<string>("Label");
                    model.PicUrl = reader.GetValue<string>("PicUrl");
                    model.PostStr = reader.GetValue<string>("PostStr");
                    model.CreateTimeFormat = reader.GetValue<DateTime>("CreateTimeFormat").ToString().ConvertTo<DateTime>(DateTime.Now);

                    model.MediaId = reader.GetValue<string>("MediaId");
                    model.Format = reader.GetValue<string>("Format");
                    model.Recognition = reader.GetValue<string>("Recognition");
                    model.ThumbMediaId = reader.GetValue<string>("ThumbMediaId");
                    model.Title = reader.GetValue<string>("Title");
                    model.Description = reader.GetValue<string>("Description");
                    model.Url = reader.GetValue<string>("Url");
                    model.MsgId = reader.GetValue<string>("MsgId");
                    model.ContentType = reader.GetValue<bool>("ContentType");
                    model.Comid = reader.GetValue<int>("Comid");

                    model.Crmid = reader.GetValue<int>("aid");
                    model.Crmname = reader.GetValue<string>("aname");
                    model.Crmphone = reader.GetValue<string>("aphone");
                    model.Manageuserid = reader.GetValue<int>("manageuserid").ToString().ConvertTo<int>(0);
                    model.Manageusername = reader.GetValue<string>("manageusername").ConvertTo<string>("");
                    model.FromUserName = reader.GetValue<string>("FromUserName").ConvertTo<string>("");

                    model.Headimgurl = reader.GetValue<string>("headimgurl").ConvertTo<string>("/images/defaultThumb.png");
                    model.City = reader.GetValue<string>("city").ConvertTo<string>("");
                    model.Province = reader.GetValue<string>("province").ConvertTo<string>("");
                    model.Nickname = reader.GetValue<string>("nickname").ConvertTo<string>("");
                    model.Sex = reader.GetValue<int>("sex").ToString().ConvertTo<int>(0);

                    list.Add(model);
                }
            }
            //totalcount = list.Count;
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }


        internal List<RequestXML> GetWxrequestxmlByopenid(int pageindex, int pagesize,out  int totalcount, string openid, string sysweixin)
        {
            var Table = "WxRequestXml";
            var Column = "*";
            var OrderColumn = "id desc";
            var GroupColumn = "";
            var PageSize = pagesize;
            var CurrentPage = pageindex;
            var Group = "";
            var Condition = " (fromusername='"+openid+"' or tousername='"+openid+"') and  msgtype in ('voice','text') and fromusername not  like 'gh_%' and tousername not  like 'gh_%' and createtimeformat between CONCAT(Convert(varchar(10), dateadd(dd,-4,getdate()),121),' 00:00:00')   and CONCAT(Convert(varchar(10),getdate(),121),' 23:59:59')  ";

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");



            cmd.PagingCommand1(Table, Column, OrderColumn, GroupColumn, PageSize, CurrentPage, Group, Condition);
            List<RequestXML> list = new List<RequestXML>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    RequestXML model = new RequestXML();
                    model.Id = reader.GetValue<int>("Id");
                    model.ToUserName = reader.GetValue<string>("ToUserName");
                    model.FromUserName = reader.GetValue<string>("FromUserName");
                    model.CreateTime = reader.GetValue<string>("CreateTime");
                    model.MsgType = reader.GetValue<string>("MsgType");
                    model.Content = reader.GetValue<string>("Content");
                    model.Location_X = reader.GetValue<string>("Location_X");
                    model.Location_Y = reader.GetValue<string>("Location_Y");
                    model.Scale = reader.GetValue<string>("Scale");
                    model.Label = reader.GetValue<string>("Label");
                    model.PicUrl = reader.GetValue<string>("PicUrl");
                    model.PostStr = reader.GetValue<string>("PostStr");
                    model.CreateTimeFormat = reader.GetValue<DateTime>("CreateTimeFormat").ToString().ConvertTo<DateTime>(DateTime.Now);

                    model.MediaId = reader.GetValue<string>("MediaId");
                    model.Format = reader.GetValue<string>("Format");
                    model.Recognition = reader.GetValue<string>("Recognition");
                    model.ThumbMediaId = reader.GetValue<string>("ThumbMediaId");
                    model.Title = reader.GetValue<string>("Title");
                    model.Description = reader.GetValue<string>("Description");
                    model.Url = reader.GetValue<string>("Url");
                    model.MsgId = reader.GetValue<string>("MsgId");
                    model.ContentType = reader.GetValue<bool>("ContentType");
                    model.Comid = reader.GetValue<int>("Comid");

                
                    list.Add(model);
                }
            }
             
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }




        internal int GetWxErr_sms_SendMsgList(int comid, string mobile)
        {
            //最多20条
            string sql = @"select id from WxRequestXml_sms where comid=@comid and mobile=@mobile and subdate=@subdate";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@mobile", mobile);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@subdate", DateTime.Now.ToString("yyyy-MM-dd"));
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("id");
                }
            }
            return 0;
        }

        internal int InsertWxErr_sms_SendMsgList(int comid, string moble)
        {

            string sql = @"insert WxRequestXml_sms(comid,mobile,subdate) values(" + comid + ",'" + moble + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            return cmd.ExecuteNonQuery();

        }

    }
}
