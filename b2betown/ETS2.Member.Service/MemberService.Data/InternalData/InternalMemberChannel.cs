using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.Member.Service.MemberService.Model;
using System.Data;

namespace ETS2.Member.Service.MemberService.Data.InternalData
{
    public class InternalMemberChannel
    {
        private SqlHelper sqlHelper;
        public InternalMemberChannel(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        //通过渠道ID 获得
        internal Member_Channel GetChannelDetail(int channelid, int comid)
        {
            const string sqltxt = @"SELECT  [id]
      ,[Com_id]
      ,[Issuetype]
      ,[companyid]
      ,[name]
      ,[mobile]
      ,[cardcode]
      ,[Chaddress]
      ,[ChObjects]
      ,[RebateOpen]
      ,[RebateConsume]
      ,[RebateConsume2]
      ,[rebatelevel]
      ,[opencardnum]
      ,[firstdealnum]
      ,[summoney]
      ,whetherdefaultchannel
      ,runstate
  FROM [EtownDB].[dbo].[Member_Channel] where id=@id and com_id=@comid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@id", channelid);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Member_Channel()
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Issuetype = reader.GetValue<int>("Issuetype"),
                        Companyid = reader.GetValue<int>("companyid"),
                        Name = reader.GetValue<string>("name"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Cardcode = reader.GetValue<decimal>("cardcode"),
                        Chaddress = reader.GetValue<string>("Chaddress"),
                        ChObjects = reader.GetValue<string>("ChObjects"),
                        RebateOpen = reader.GetValue<decimal>("RebateOpen"),
                        RebateConsume = reader.GetValue<decimal>("RebateConsume"),
                        RebateConsume2 = reader.GetValue<decimal>("RebateConsume2"),
                        RebateLevel = reader.GetValue<int>("rebatelevel"),
                        Opencardnum = reader.GetValue<int>("opencardnum"),
                        Firstdealnum = reader.GetValue<int>("firstdealnum"),
                        Summoney = reader.GetValue<decimal>("summoney"),
                        Whetherdefaultchannel = Convert.IsDBNull(reader.GetValue(16)) == true ? 0 : reader.GetValue<int>("whetherdefaultchannel"),
                        Runstate = Convert.IsDBNull(reader.GetValue(17)) == true ? 1 : reader.GetValue<int>("runstate")
                    };
                }
            }
            return null;

        }


        //通过渠道ID 获得
        internal int GetChannelidbymanageuserid(int manageuserid, int comid)
        {
            const string sqltxt = @"SELECT  [id]
      ,[Com_id]
      ,[Issuetype]
      ,[companyid]
      ,[name]
      ,[mobile]
      ,[cardcode]
      ,[Chaddress]
      ,[ChObjects]
      ,[RebateOpen]
      ,[RebateConsume]
      ,[RebateConsume2]
      ,[rebatelevel]
      ,[opencardnum]
      ,[firstdealnum]
      ,[summoney]
      ,whetherdefaultchannel
      ,runstate
  FROM [EtownDB].[dbo].[Member_Channel] where com_id=@comid and mobile in (select tel from b2b_company_manageuser where com_id=@comid and id=@id)";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@id", manageuserid);
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



        //通过渠道ID 获得
        internal Member_Channel GetChannelDetail(int channelid)
        {
            const string sqltxt = @"SELECT  [id]
      ,[Com_id]
      ,[Issuetype]
      ,[companyid]
      ,[name]
      ,[mobile]
      ,[cardcode]
      ,[Chaddress]
      ,[ChObjects]
      ,[RebateOpen]
      ,[RebateConsume]
      ,[RebateConsume2]
      ,[rebatelevel]
      ,[opencardnum]
      ,[firstdealnum]
      ,[summoney]
      ,[whetherdefaultchannel]
      ,[runstate]
,Lockuserweixin
,Lockusertime
,LockUser
,rebatemoney
  FROM [EtownDB].[dbo].[Member_Channel] where id=@id ";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@id", channelid);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Member_Channel()
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Issuetype = reader.GetValue<int>("Issuetype"),
                        Companyid = reader.GetValue<int>("companyid"),
                        Name = reader.GetValue<string>("name"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Cardcode = reader.GetValue<decimal>("cardcode"),
                        Chaddress = reader.GetValue<string>("Chaddress"),
                        ChObjects = reader.GetValue<string>("ChObjects"),
                        RebateOpen = reader.GetValue<decimal>("RebateOpen"),
                        RebateConsume = reader.GetValue<decimal>("RebateConsume"),
                        RebateConsume2 = reader.GetValue<decimal>("RebateConsume2"),
                        RebateLevel = reader.GetValue<int>("rebatelevel"),
                        Opencardnum = reader.GetValue<int>("opencardnum"),
                        Firstdealnum = reader.GetValue<int>("firstdealnum"),
                        Summoney = reader.GetValue<decimal>("summoney"),
                        Whetherdefaultchannel = reader.GetValue<int>("whetherdefaultchannel"),
                        Runstate = reader.GetValue<int>("runstate"),
                        Lockuserweixin = reader.GetValue<string>("Lockuserweixin"),
                        Lockusertime = reader.GetValue<DateTime>("Lockusertime"),
                        Lockuser = reader.GetValue<int>("LockUser"),
                        Rebatemoney = reader.GetValue<decimal>("rebatemoney"),

                    };
                }
            }
            return null;

        }


        //通过渠道ID 获得 员工id
        internal int GetmanageuseridbyChannelid(int channelid, int comid)
        {
            const string sqltxt = @"select * from b2b_company_manageuser where com_id=@comid and tel in (SELECT  mobile  FROM [Member_Channel] where com_id=@comid and id=@id )";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@id", channelid);
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



        #region 判断手机是否存在
        private static readonly string SQL_SearchPhone = @"SELECT * FROM dbo.Member_Channel a 
                                                    WHERE a.mobile = @mobile and a.com_id=@comid";
        public bool Ishasphone(string moblie, int comid, out int cid)
        {
            var command = sqlHelper.PrepareTextSqlCommand(SQL_SearchPhone);
            command.AddParam("@mobile", moblie);
            command.AddParam("@comid", comid);
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    cid = reader.GetValue<int>("id");
                    return false;
                }
                else
                {
                    cid = 0;
                    return true;
                }
            }
        }
        #endregion




        //插入或修改渠道信息
        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateMemberChannel";

        internal int EditChannel(Member_Channel model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);

            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Com_id", model.Com_id);
            cmd.AddParam("@Issuetype", model.Issuetype);
            cmd.AddParam("@Name", model.Name);
            cmd.AddParam("@Mobile", model.Mobile);
            cmd.AddParam("@Cardcode", model.Cardcode);
            cmd.AddParam("@Chaddress", model.Chaddress);
            cmd.AddParam("@ChObjects", model.ChObjects);
            cmd.AddParam("@RebateOpen", model.RebateOpen);
            cmd.AddParam("@RebateConsume2", model.RebateConsume2);
            cmd.AddParam("@RebateLevel", model.RebateLevel);
            cmd.AddParam("@Companyid", model.Companyid);
            cmd.AddParam("@RebateConsume", model.RebateConsume);

            cmd.AddParam("@Opencardnum", model.Opencardnum);
            cmd.AddParam("@Firstdealnum", model.Firstdealnum);
            cmd.AddParam("@Summoney", model.Summoney);
            cmd.AddParam("@whetherdefaultchannel", model.Whetherdefaultchannel);
            cmd.AddParam("@runstate", model.Runstate);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }

        //渠道列表
        internal List<Member_Channel> PageList(string companyid, int pageindex, int pagesize, string issuetype, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "Member_Channel";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";

            var condition = "";

            condition += " com_id=" + companyid;

            if (issuetype != "all")
            {
                condition += " and issuetype=" + issuetype;
            }

            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<Member_Channel> list = new List<Member_Channel>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_Channel
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Name = reader.GetValue<string>("name"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Issuetype = reader.GetValue<int>("issuetype"),
                        Companyid = reader.GetValue<int>("companyid"),
                        Chaddress = reader.GetValue<string>("chaddress"),
                        Cardcode = reader.GetValue<decimal>("Cardcode"),
                        ChObjects = reader.GetValue<string>("ChObjects"),
                        RebateConsume = reader.GetValue<decimal>("RebateConsume"),
                        RebateConsume2 = reader.GetValue<decimal>("RebateConsume2"),
                        RebateLevel = reader.GetValue<int>("RebateLevel"),
                        RebateOpen = reader.GetValue<decimal>("RebateOpen"),
                        Opencardnum = reader.GetValue<int>("opencardnum"),
                        Firstdealnum = reader.GetValue<int>("firstdealnum"),
                        Summoney = reader.GetValue<decimal>("summoney")

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }
        //渠道列表
        internal List<Member_Channel> PageList(string companyid, int pageindex, int pagesize, string issuetype, int channelcompanyid, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "Member_Channel";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";

            var condition = "";

            condition += " companyid=" + channelcompanyid;

            if (issuetype != "all")
            {
                condition += " and issuetype=" + issuetype;
            }

            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<Member_Channel> list = new List<Member_Channel>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_Channel
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Name = reader.GetValue<string>("name"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Issuetype = reader.GetValue<int>("issuetype"),
                        Companyid = reader.GetValue<int>("companyid"),
                        Chaddress = reader.GetValue<string>("chaddress"),
                        Cardcode = reader.GetValue<decimal>("Cardcode"),
                        ChObjects = reader.GetValue<string>("ChObjects"),
                        RebateConsume = reader.GetValue<decimal>("RebateConsume"),
                        RebateConsume2 = reader.GetValue<decimal>("RebateConsume2"),
                        RebateLevel = reader.GetValue<int>("RebateLevel"),
                        RebateOpen = reader.GetValue<decimal>("RebateOpen"),
                        Opencardnum = reader.GetValue<int>("opencardnum"),
                        Firstdealnum = reader.GetValue<int>("firstdealnum"),
                        Summoney = reader.GetValue<decimal>("summoney")

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }
        //渠道查询
        internal List<Member_Channel> SeachList(string companyid, int pageindex, int pagesize, string key, int select, bool isNum, out int totalcount)
        {
            string condition = "";//条件

            string channelcompanylimit = "";//公司限制条件
            channelcompanylimit += "   com_id in (" + companyid + ") ";


            string iss = "";
            if (select != 9)
            {
                iss = " and Issuetype= " + select;
            }


            if (key == "" || key == null)
            {
                condition += channelcompanylimit + iss;
            }
            else
            {
                if (isNum)
                {

                    condition += channelcompanylimit + " and (cardcode=" + key + "  or companyid=" + key + " or mobile='" + key + "')";
                }
                else
                {
                    condition += channelcompanylimit + " and ( mobile='" + key + "' or name='" + key + "') ";
                }
            }

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "Member_Channel";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";

            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);



            List<Member_Channel> list = new List<Member_Channel>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_Channel
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Name = reader.GetValue<string>("name"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Issuetype = reader.GetValue<int>("issuetype"),
                        Companyid = reader.GetValue<int>("companyid"),
                        Chaddress = reader.GetValue<string>("chaddress"),
                        Cardcode = reader.GetValue<decimal>("Cardcode"),
                        ChObjects = reader.GetValue<string>("ChObjects"),
                        RebateConsume = reader.GetValue<decimal>("RebateConsume"),
                        RebateConsume2 = reader.GetValue<decimal>("RebateConsume2"),
                        RebateLevel = reader.GetValue<int>("RebateLevel"),
                        RebateOpen = reader.GetValue<decimal>("RebateOpen"),
                        Opencardnum = reader.GetValue<int>("opencardnum"),
                        Firstdealnum = reader.GetValue<int>("firstdealnum"),
                        Summoney = reader.GetValue<decimal>("summoney")

                    });

                }
            }

            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());


            return list;

        }
        //渠道查询
        internal List<Member_Channel> SeachList(string companyid, int pageindex, int pagesize, string key, int select, bool isNum, int channelcompanyid, out int totalcount)
        {
            string condition = "";//条件

            string channelcompanylimit = "";//公司限制条件
            channelcompanylimit += "   companyid in (" + channelcompanyid + ") ";


            string iss = "";
            if (select != 9)
            {
                iss = " and Issuetype= " + select;
            }


            if (key == "" || key == null)
            {
                condition += channelcompanylimit + iss;
            }
            else
            {
                if (isNum)
                {

                    condition += channelcompanylimit + " and (cardcode=" + key + "  or companyid=" + key + " or mobile='" + key + "')";
                }
                else
                {
                    condition += channelcompanylimit + " and ( mobile='" + key + "' or name='" + key + "') ";
                }
            }

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "Member_Channel";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";

            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);



            List<Member_Channel> list = new List<Member_Channel>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_Channel
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Name = reader.GetValue<string>("name"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Issuetype = reader.GetValue<int>("issuetype"),
                        Companyid = reader.GetValue<int>("companyid"),
                        Chaddress = reader.GetValue<string>("chaddress"),
                        Cardcode = reader.GetValue<decimal>("Cardcode"),
                        ChObjects = reader.GetValue<string>("ChObjects"),
                        RebateConsume = reader.GetValue<decimal>("RebateConsume"),
                        RebateConsume2 = reader.GetValue<decimal>("RebateConsume2"),
                        RebateLevel = reader.GetValue<int>("RebateLevel"),
                        RebateOpen = reader.GetValue<decimal>("RebateOpen"),
                        Opencardnum = reader.GetValue<int>("opencardnum"),
                        Firstdealnum = reader.GetValue<int>("firstdealnum"),
                        Summoney = reader.GetValue<decimal>("summoney")

                    });

                }
            }

            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());


            return list;

        }
        //通过公司ID获得渠道列表
        internal List<Member_Channel> GetChannelListByCompanyid(string companyid)
        {
            const string sqltet = @"SELECT [id]
      ,[Com_id]
      ,[Issuetype]
      ,[companyid]
      ,[name]
      ,[mobile]
      ,[cardcode]
      ,[Chaddress]
      ,[ChObjects]
      ,[RebateOpen]
      ,[RebateConsume]
      ,[RebateConsume2]
      ,[rebatelevel]
      ,[opencardnum]
      ,[firstdealnum]
      ,[summoney]
  FROM [EtownDB].[dbo].[Member_Channel] where companyid=@companyid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqltet);
            cmd.AddParam("@companyid", companyid);

            List<Member_Channel> list = new List<Member_Channel>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Member_Channel()
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Issuetype = reader.GetValue<int>("Issuetype"),
                        Companyid = reader.GetValue<int>("companyid"),
                        Name = reader.GetValue<string>("name"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Cardcode = reader.GetValue<decimal>("cardcode"),
                        Chaddress = reader.GetValue<string>("Chaddress"),
                        ChObjects = reader.GetValue<string>("ChObjects"),
                        RebateOpen = reader.GetValue<decimal>("RebateOpen"),
                        RebateConsume = reader.GetValue<decimal>("RebateConsume"),
                        RebateConsume2 = reader.GetValue<decimal>("RebateConsume2"),
                        RebateLevel = reader.GetValue<int>("rebatelevel"),
                        Opencardnum = reader.GetValue<int>("opencardnum"),
                        Firstdealnum = reader.GetValue<int>("firstdealnum"),
                        Summoney = reader.GetValue<decimal>("summoney")
                    });
                }
                return list;
            }
        }

        //通过卡号关联的渠道号查询渠道信息
        internal Member_Channel GetChannelDetailByCardNo(string Cardcode)
        {
            const string sqltxt = @"SELECT [id]
      ,[Com_id]
      ,[Issuetype]
      ,[companyid]
      ,[name]
      ,[mobile]
      ,[cardcode]
      ,[Chaddress]
      ,[ChObjects]
      ,[RebateOpen]
      ,[RebateConsume]
      ,[RebateConsume2]
      ,[rebatelevel]
      ,[opencardnum]
      ,[firstdealnum]
      ,[summoney]
  FROM [EtownDB].[dbo].[Member_Channel] where id in (SELECT  
       [IssueCard]
      
  FROM [EtownDB].[dbo].[Member_Card] where Cardcode=@cardcode )";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@cardcode", Cardcode);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Member_Channel()
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Issuetype = reader.GetValue<int>("Issuetype"),
                        Companyid = reader.GetValue<int>("companyid"),
                        Name = reader.GetValue<string>("name"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Cardcode = reader.GetValue<decimal>("cardcode"),
                        Chaddress = reader.GetValue<string>("Chaddress"),
                        ChObjects = reader.GetValue<string>("ChObjects"),
                        RebateOpen = reader.GetValue<decimal>("RebateOpen"),
                        RebateConsume = reader.GetValue<decimal>("RebateConsume"),
                        RebateConsume2 = reader.GetValue<decimal>("RebateConsume2"),
                        RebateLevel = reader.GetValue<int>("rebatelevel"),
                        Opencardnum = reader.GetValue<int>("opencardnum"),
                        Firstdealnum = reader.GetValue<int>("firstdealnum"),
                        Summoney = reader.GetValue<decimal>("summoney"),
                    };
                }
            }

            return null;
        }

        //通过卡号关联的渠道号查询渠道信息
        internal int RegCardChannel_Server_Auto(string Cardcode, int ChannelCard, int ServerCard, int comid)
        {
            string sqltxt = "";
            if (ChannelCard != 0)
            {
                sqltxt = @"update member_card set servercard=@ServerCard,issuecard=@ChannelCard where cardcode=@cardcode and com_id=@comid";
            }
            else
            {
                sqltxt = @"update member_card set servercard=@ServerCard where cardcode=@cardcode and com_id=@comid";
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@cardcode", Cardcode);
            cmd.AddParam("@ChannelCard", ChannelCard);
            cmd.AddParam("@ServerCard", ServerCard);
            cmd.AddParam("@comid", comid);
            cmd.ExecuteNonQuery();
            return 0;
        }



        //通过渠道卡号获得渠道信息
        internal Member_Channel GetSelfChannelDetailByCardNo(string Cardcode)
        {
            const string sqltxt = @"SELECT [id]
      ,[Com_id]
      ,[Issuetype]
      ,[companyid]
      ,[name]
      ,[mobile]
      ,[cardcode]
      ,[Chaddress]
      ,[ChObjects]
      ,[RebateOpen]
      ,[RebateConsume]
      ,[RebateConsume2]
      ,[rebatelevel]
      ,[opencardnum]
      ,[firstdealnum]
      ,[summoney]
  FROM [EtownDB].[dbo].[Member_Channel] where cardcode=@cardcode ";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@cardcode", Cardcode);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Member_Channel()
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Issuetype = reader.GetValue<int>("Issuetype"),
                        Companyid = reader.GetValue<int>("companyid"),
                        Name = reader.GetValue<string>("name"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Cardcode = reader.GetValue<decimal>("cardcode"),
                        Chaddress = reader.GetValue<string>("Chaddress"),
                        ChObjects = reader.GetValue<string>("ChObjects"),
                        RebateOpen = reader.GetValue<decimal>("RebateOpen"),
                        RebateConsume = reader.GetValue<decimal>("RebateConsume"),
                        RebateConsume2 = reader.GetValue<decimal>("RebateConsume2"),
                        RebateLevel = reader.GetValue<int>("rebatelevel"),
                        Opencardnum = reader.GetValue<int>("opencardnum"),
                        Firstdealnum = reader.GetValue<int>("firstdealnum"),
                        Summoney = reader.GetValue<decimal>("summoney"),
                    };
                }
            }
            return null;
        }



        //通过手机号，COMID获得渠道信息
        internal Member_Channel GetPhoneComIdChannelDetail(string phone, int comid)
        {
            const string sqltxt = @"SELECT [id]
      ,[Com_id]
      ,[Issuetype]
      ,[companyid]
      ,[name]
      ,[mobile]
      ,[cardcode]
      ,[Chaddress]
      ,[ChObjects]
      ,[RebateOpen]
      ,[RebateConsume]
      ,[RebateConsume2]
      ,[rebatelevel]
      ,[opencardnum]
      ,[firstdealnum]
      ,[summoney]
,[LockUserTime]
,[LockUserWeixin]
  FROM [EtownDB].[dbo].[Member_Channel] where mobile=@phone and com_id=@comid ";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@phone", phone);
            cmd.AddParam("@comid", comid);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Member_Channel()
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Issuetype = reader.GetValue<int>("Issuetype"),
                        Companyid = reader.GetValue<int>("companyid"),
                        Name = reader.GetValue<string>("name"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Cardcode = reader.GetValue<decimal>("cardcode"),
                        Chaddress = reader.GetValue<string>("Chaddress"),
                        ChObjects = reader.GetValue<string>("ChObjects"),
                        RebateOpen = reader.GetValue<decimal>("RebateOpen"),
                        RebateConsume = reader.GetValue<decimal>("RebateConsume"),
                        RebateConsume2 = reader.GetValue<decimal>("RebateConsume2"),
                        RebateLevel = reader.GetValue<int>("rebatelevel"),
                        Opencardnum = reader.GetValue<int>("opencardnum"),
                        Firstdealnum = reader.GetValue<int>("firstdealnum"),
                        Summoney = reader.GetValue<decimal>("summoney"),
                        Lockuserweixin = reader.GetValue<string>("Lockuserweixin"),
                        Lockusertime = reader.GetValue<DateTime>("Lockusertime"),
                    };
                }
            }
            return null;
        }

        //得到WEB或微信渠道号
        internal Member_Channel GetChannelWebWeixin(string issuetype, int comid)
        {
            string sqltxt = "";

            if (issuetype == "web")
            {
                sqltxt = @"SELECT [id]
      ,[Com_id]
      ,[Issuetype]
      ,[companyid]
      ,[name]
      ,[mobile]
      ,[cardcode]
      ,[Chaddress]
      ,[ChObjects]
      ,[RebateOpen]
      ,[RebateConsume]
      ,[RebateConsume2]
      ,[rebatelevel]
      ,[opencardnum]
      ,[firstdealnum]
      ,[summoney]
     FROM [EtownDB].[dbo].[Member_Channel] where issuetype=3 and com_id=@comid";

            }
            else
            {
                sqltxt = @"SELECT [id]
      ,[Com_id]
      ,[Issuetype]
      ,[companyid]
      ,[name]
      ,[mobile]
      ,[cardcode]
      ,[Chaddress]
      ,[ChObjects]
      ,[RebateOpen]
      ,[RebateConsume]
      ,[RebateConsume2]
      ,[rebatelevel]
      ,[opencardnum]
      ,[firstdealnum]
      ,[summoney]
     FROM [EtownDB].[dbo].[Member_Channel] where issuetype=4 and com_id=@comid";

            }

            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Member_Channel()
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Issuetype = reader.GetValue<int>("Issuetype"),
                        Companyid = reader.GetValue<int>("companyid"),
                        Name = reader.GetValue<string>("name"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Cardcode = reader.GetValue<decimal>("cardcode"),
                        Chaddress = reader.GetValue<string>("Chaddress"),
                        ChObjects = reader.GetValue<string>("ChObjects"),
                        RebateOpen = reader.GetValue<decimal>("RebateOpen"),
                        RebateConsume = reader.GetValue<decimal>("RebateConsume"),
                        RebateConsume2 = reader.GetValue<decimal>("RebateConsume2"),
                        RebateLevel = reader.GetValue<int>("rebatelevel"),
                        Opencardnum = reader.GetValue<int>("opencardnum"),
                        Firstdealnum = reader.GetValue<int>("firstdealnum"),
                        Summoney = reader.GetValue<decimal>("summoney"),
                    };
                }
            }
            return null;
        }

        internal Member_Channel GetCardByCardName(string name)
        {
            const string sqltxt = @"SELECT [id]
      ,[Com_id]
      ,[Issuetype]
      ,[companyid]
      ,[name]
      ,[mobile]
      ,[cardcode]
      ,[Chaddress]
      ,[ChObjects]
      ,[RebateOpen]
      ,[RebateConsume]
      ,[RebateConsume2]
      ,[rebatelevel]
      ,[opencardnum]
      ,[firstdealnum]
      ,[summoney]
  FROM [EtownDB].[dbo].[Member_Channel] where name=@name";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@name", name);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Member_Channel()
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Issuetype = reader.GetValue<int>("Issuetype"),
                        Companyid = reader.GetValue<int>("companyid"),
                        Name = reader.GetValue<string>("name"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Cardcode = reader.GetValue<decimal>("cardcode"),
                        Chaddress = reader.GetValue<string>("Chaddress"),
                        ChObjects = reader.GetValue<string>("ChObjects"),
                        RebateOpen = reader.GetValue<decimal>("RebateOpen"),
                        RebateConsume = reader.GetValue<decimal>("RebateConsume"),
                        RebateConsume2 = reader.GetValue<decimal>("RebateConsume2"),
                        RebateLevel = reader.GetValue<int>("rebatelevel"),
                        Opencardnum = reader.GetValue<int>("opencardnum"),
                        Firstdealnum = reader.GetValue<int>("firstdealnum"),
                        Summoney = reader.GetValue<decimal>("summoney"),
                    };
                }
            }
            return null;
        }

        //服务号和推荐人
        internal string UpchannlT(int userid, int channlid, int uptype)
        {
            string sqltxt = "";
            var iss = "";
            var server = "";
            if (uptype != 0)
            {
                if (uptype == 1)
                {
                    iss = "[IssueCard] = @channlid";
                }
                else if (uptype == 2)
                {
                    server = " [ServerCard] = @channlid";
                }
                else
                {
                    iss = "[IssueCard] = @channlid";
                    server = ",[ServerCard] = @channlid";
                }
            } sqltxt = @"UPDATE [EtownDB].[dbo].[Member_Card]
   SET " + iss + "  " + server + " WHERE  [id] = @userid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@userid", userid);
            cmd.AddParam("@channlid", channlid);
            cmd.ExecuteNonQuery();

            return "OK";
        }

        //统计渠道信息
        internal List<Member_Channel> Channelstatistics(string comid, int pageindex, int pagesize, string issuetype, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

            string tblname = "View_outchannelstatistics";
            if (issuetype == "inner")
            {
                tblname = "View_innerchannelstatistics";
            }

            var condition = "";
            if (comid != "0")
            {
                condition += "com_id=" + comid;
            }

            cmd.PagingCommand1(tblname, "*", "companyname", "", pagesize, pageindex, "", condition);


            List<Member_Channel> list = new List<Member_Channel>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_Channel
                    {
                        //Companyid = reader.GetValue<int>("companyid"),
                        Opencardnum = reader.GetValue<int>("opencardnum"),
                        Firstdealnum = reader.GetValue<int>("firstdealnum"),
                        Summoney = reader.GetValue<decimal>("summoney"),
                        Channeltypename = reader.GetValue<string>("companyname"),
                        Companyid = reader.GetValue<int>("id"),
                        Companynum = reader.GetValue<int>("companynum")
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }
        //统计渠道信息
        internal List<Member_Channel> Channelstatistics2(string comid, int pageindex, int pagesize, string issuetype, string companystate, out int totalcount, int channelcompanyid = 0)
        {
            if (channelcompanyid > 0)//如果有渠道单位编号，则只是显示当前渠道单位的统计信息,不用分页
            {
                string sqlq = @"SELECT     a.companyname, a.id,a.companystate, COUNT(c.id) AS Expr1, SUM(isnull(c.opencardnum,0))  AS opencardnum, SUM(isnull(c.firstdealnum,0)) AS firstdealnum, SUM(isnull(c.summoney,0)) 
                                AS summoney, a.Com_id, a.id AS Expr2,COUNT(d.ID) AS companynum
                                FROM         dbo.Member_Channel_company AS a LEFT OUTER JOIN
                               dbo.Member_Channel AS c ON a.id = c.companyid left join 
                               dbo.Member_Activity_Log AS d ON d.sales_admin = c.name
                               where a.id=@channelcompanyid                  
                              GROUP BY a.companyname, a.Com_id, a.id,a.companystate";
                var cmd = this.sqlHelper.PrepareTextSqlCommand(sqlq);
                cmd.AddParam("@channelcompanyid", channelcompanyid);
                List<Member_Channel> list = new List<Member_Channel>();
                using (var reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        list.Add(new Member_Channel
                        {
                            //Companyid = reader.GetValue<int>("companyid"),
                            Opencardnum = reader.GetValue<int>("opencardnum"),
                            Firstdealnum = reader.GetValue<int>("firstdealnum"),
                            Summoney = reader.GetValue<decimal>("summoney"),
                            Channeltypename = reader.GetValue<string>("companyname"),
                            Companyid = reader.GetValue<int>("id"),
                            Companynum = reader.GetValue<int>("companynum"),
                            Companystate = reader.GetValue<int>("companystate")
                        });
                        totalcount = 1;
                    }
                    else
                    {
                        list = null;
                        totalcount = 0;
                    }
                }
                return list;

            }
            else
            {
                //用视图实现分页显示
                var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

                string tblname = "View_outchannelstatistics";


                if (issuetype == "inner")
                {
                    tblname = "View_innerchannelstatistics";
                }


                var condition = "companystate in ('" + companystate + "')";
                if (comid != "0")
                {
                    condition = "com_id=" + comid + " and companystate in (" + companystate + ")";
                }

                cmd.PagingCommand1(tblname, "*", "companyname", "", pagesize, pageindex, "", condition);


                List<Member_Channel> list = new List<Member_Channel>();
                using (var reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        list.Add(new Member_Channel
                        {
                            //Companyid = reader.GetValue<int>("companyid"),
                            Opencardnum = reader.GetValue<int>("opencardnum"),
                            Firstdealnum = reader.GetValue<int>("firstdealnum"),
                            Summoney = reader.GetValue<decimal>("summoney"),
                            Channeltypename = reader.GetValue<string>("companyname"),
                            Companyid = reader.GetValue<int>("id"),
                            Companynum = reader.GetValue<int>("companynum"),
                            Companystate = reader.GetValue<int>("companystate")
                        });

                    }
                }
                totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

                return list;
            }
        }
        //统计渠道 验卡信息
        internal List<Member_Channel> ChannelYk(string comid, int pageindex, int pagesize, string issuetype, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "View_member_channle_YK";
            var strGetFields = "*";
            var sortKey = "companyname";
            var sortMode = "1";

            var condition = "";
            if (comid != "0")
            {
                condition += "com_id=" + comid;
            }

            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<Member_Channel> list = new List<Member_Channel>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_Channel
                    {

                        Com_id = reader.GetValue<int>("companynum"),
                        Channeltypename = reader.GetValue<string>("companyname")
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }

        internal List<Member_Channel> GetAllInnerChannels(out int totalcount)
        {


            string sqld = @"SELECT   [id]
      ,[Com_id]
      ,[Issuetype]
      ,[companyid]
      ,[name]
      ,[mobile]
      ,[cardcode]
      ,[Chaddress]
      ,[ChObjects]
      ,[RebateOpen]
      ,[RebateConsume]
      ,[RebateConsume2]
      ,[rebatelevel]
      ,[opencardnum]
      ,[firstdealnum]
      ,[summoney]
  FROM [EtownDB].[dbo].[Member_Channel] where Issuetype=@Issuetype";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqld);
            cmd.AddParam("@Issuetype", 0);


            List<Member_Channel> list = new List<Member_Channel>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_Channel
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Issuetype = reader.GetValue<int>("Issuetype"),
                        Channeltypename = reader.GetValue<int>("Issuetype") == 1 ? "外部渠道" : "内部渠道",
                        Companyid = reader.GetValue<int>("companyid"),
                        Name = reader.GetValue<string>("name"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Cardcode = reader.GetValue<decimal>("cardcode"),
                        Chaddress = reader.GetValue<string>("Chaddress"),
                        ChObjects = reader.GetValue<string>("ChObjects"),
                        RebateOpen = reader.GetValue<decimal>("RebateOpen"),
                        RebateConsume = reader.GetValue<decimal>("RebateConsume"),
                        RebateConsume2 = reader.GetValue<decimal>("RebateConsume2"),
                        RebateLevel = reader.GetValue<int>("rebatelevel"),
                        Opencardnum = reader.GetValue<int>("opencardnum"),
                        Firstdealnum = reader.GetValue<int>("firstdealnum"),
                        Summoney = reader.GetValue<decimal>("summoney"),
                    });

                }
            }
            totalcount = list.Count;

            return list;
        }

        internal List<Member_Channel> SearchChannelByChannelUnit(string comid, int pageindex, int pagesize, int channelcompanyid, out int totalcount)
        {
            string condition = "  companyid in (" + channelcompanyid + ") ";

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var tblName = "Member_Channel";
            var strGetFields = "*";

            cmd.PagingCommand1(tblName, strGetFields, "id desc", "", pagesize, pageindex, "", condition);



            List<Member_Channel> list = new List<Member_Channel>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_Channel
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Name = reader.GetValue<string>("name"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Issuetype = reader.GetValue<int>("issuetype"),
                        Companyid = reader.GetValue<int>("companyid"),
                        Chaddress = reader.GetValue<string>("chaddress"),
                        Cardcode = reader.GetValue<decimal>("Cardcode"),
                        ChObjects = reader.GetValue<string>("ChObjects"),
                        RebateConsume = reader.GetValue<decimal>("RebateConsume"),
                        RebateConsume2 = reader.GetValue<decimal>("RebateConsume2"),
                        RebateLevel = reader.GetValue<int>("RebateLevel"),
                        RebateOpen = reader.GetValue<decimal>("RebateOpen"),
                        Opencardnum = reader.GetValue<int>("opencardnum"),
                        Firstdealnum = reader.GetValue<int>("firstdealnum"),
                        Summoney = reader.GetValue<decimal>("summoney"),
                        Whetherdefaultchannel = Convert.IsDBNull(reader.GetValue(16)) == true ? 1 : reader.GetValue<int>("whetherdefaultchannel"),
                        Runstate = Convert.IsDBNull(reader.GetValue(17)) == true ? 1 : reader.GetValue<int>("runstate")

                    });

                }
            }

            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());


            return list;

        }

        internal List<Member_Channel> Channellistbycomid(string comid, int pageindex, int pagesize, out int totalcount)
        {
            string condition = "";

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var tblName = "Member_Channel";
            var strGetFields = "*";

            cmd.PagingCommand1(tblName, strGetFields, "id desc", "", pagesize, pageindex, "", condition);



            List<Member_Channel> list = new List<Member_Channel>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_Channel
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Name = reader.GetValue<string>("name"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Issuetype = reader.GetValue<int>("issuetype"),
                        Companyid = reader.GetValue<int>("companyid"),
                        Chaddress = reader.GetValue<string>("chaddress"),
                        Cardcode = reader.GetValue<decimal>("Cardcode"),
                        ChObjects = reader.GetValue<string>("ChObjects"),
                        RebateConsume = reader.GetValue<decimal>("RebateConsume"),
                        RebateConsume2 = reader.GetValue<decimal>("RebateConsume2"),
                        RebateLevel = reader.GetValue<int>("RebateLevel"),
                        RebateOpen = reader.GetValue<decimal>("RebateOpen"),
                        Opencardnum = reader.GetValue<int>("opencardnum"),
                        Firstdealnum = reader.GetValue<int>("firstdealnum"),
                        Summoney = reader.GetValue<decimal>("summoney")

                    });

                }
            }

            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());


            return list;
        }

        internal Member_Channel GetChannelByOpenId(string openid)
        {
            string sql = @"select * from Member_Channel where id  in 
                            (
                              select issuecard from Member_Card where cardcode in 
                              (
                                select  IDcard from b2b_crm  where weixin=@openid
                              )
                            ) ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@openid", openid);

            using (var reader = cmd.ExecuteReader())
            {
                Member_Channel u = null;
                if (reader.Read())
                {
                    u = new Member_Channel()
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Issuetype = reader.GetValue<int>("Issuetype"),
                        Companyid = reader.GetValue<int>("companyid"),
                        Name = reader.GetValue<string>("name"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Cardcode = reader.GetValue<decimal>("cardcode"),
                        Chaddress = reader.GetValue<string>("Chaddress"),
                        ChObjects = reader.GetValue<string>("ChObjects"),
                        RebateOpen = reader.GetValue<decimal>("RebateOpen"),
                        RebateConsume = reader.GetValue<decimal>("RebateConsume"),
                        RebateConsume2 = reader.GetValue<decimal>("RebateConsume2"),
                        RebateLevel = reader.GetValue<int>("rebatelevel"),
                        Opencardnum = reader.GetValue<int>("opencardnum"),
                        Firstdealnum = reader.GetValue<int>("firstdealnum"),
                        Summoney = reader.GetValue<decimal>("summoney"),
                        Whetherdefaultchannel = Convert.IsDBNull(reader.GetValue(16)) == true ? 0 : reader.GetValue<int>("whetherdefaultchannel"),
                        Runstate = reader.GetValue<int>("runstate")
                    };
                }
                return u;
            }

        }

        internal int GetDefaultChannelNum(int companyid)
        {
            string sql = "select count(1) from Member_Channel where companyid=" + companyid + " and whetherdefaultchannel=1";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            return int.Parse(o.ToString());
        }

        internal Member_Channel GetDefaultChannel(int channelcompanyid)
        {
            const string sqltxt = @"SELECT  [id]
      ,[Com_id]
      ,[Issuetype]
      ,[companyid]
      ,[name]
      ,[mobile]
      ,[cardcode]
      ,[Chaddress]
      ,[ChObjects]
      ,[RebateOpen]
      ,[RebateConsume]
      ,[RebateConsume2]
      ,[rebatelevel]
      ,[opencardnum]
      ,[firstdealnum]
      ,[summoney]
      ,whetherdefaultchannel
  FROM [EtownDB].[dbo].[Member_Channel] where companyid=@companyid and whetherdefaultchannel=1";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@companyid", channelcompanyid);


            Member_Channel u = null;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    u = new Member_Channel()
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Issuetype = reader.GetValue<int>("Issuetype"),
                        Companyid = reader.GetValue<int>("companyid"),
                        Name = reader.GetValue<string>("name"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Cardcode = reader.GetValue<decimal>("cardcode"),
                        Chaddress = reader.GetValue<string>("Chaddress"),
                        ChObjects = reader.GetValue<string>("ChObjects"),
                        RebateOpen = reader.GetValue<decimal>("RebateOpen"),
                        RebateConsume = reader.GetValue<decimal>("RebateConsume"),
                        RebateConsume2 = reader.GetValue<decimal>("RebateConsume2"),
                        RebateLevel = reader.GetValue<int>("rebatelevel"),
                        Opencardnum = reader.GetValue<int>("opencardnum"),
                        Firstdealnum = reader.GetValue<int>("firstdealnum"),
                        Summoney = reader.GetValue<decimal>("summoney"),
                        Whetherdefaultchannel = Convert.IsDBNull(reader.GetValue(16)) == true ? 0 : reader.GetValue<int>("whetherdefaultchannel")
                    };
                }
            }
            return u;

        }
        internal int GetChannelImgbyopenid(string openid)
        {
            const string sqltxt = @"select * from b2b_company_manageuser where tel in ( select mobile from member_channel where id in (select issuecard from member_card where cardcode in (select idcard from b2b_crm where weixin=@openid)))";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@openid", openid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("headimg");
                }
            }
            return 0;
        }


        internal List<Member_Channel> GetChannelList(int channelcomid, string runstate, string whetherdefaultchannel, out int totalcount)
        {

            string sqld = @"SELECT   [id]
      ,[Com_id]
      ,[Issuetype]
      ,[companyid]
      ,[name]
      ,[mobile]
      ,[cardcode]
      ,[Chaddress]
      ,[ChObjects]
      ,[RebateOpen]
      ,[RebateConsume]
      ,[RebateConsume2]
      ,[rebatelevel]
      ,[opencardnum]
      ,[firstdealnum]
      ,[summoney]
      ,whetherdefaultchannel
      ,runstate
  FROM [EtownDB].[dbo].[Member_Channel] where companyid in (" + channelcomid + ") and runstate in (" + runstate + ") and whetherdefaultchannel in (" + whetherdefaultchannel + ")";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqld);



            List<Member_Channel> list = new List<Member_Channel>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_Channel
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Issuetype = reader.GetValue<int>("Issuetype"),
                        Channeltypename = reader.GetValue<int>("Issuetype") == 1 ? "外部渠道" : "内部渠道",
                        Companyid = reader.GetValue<int>("companyid"),
                        Name = reader.GetValue<string>("name"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Cardcode = reader.GetValue<decimal>("cardcode"),
                        Chaddress = reader.GetValue<string>("Chaddress"),
                        ChObjects = reader.GetValue<string>("ChObjects"),
                        RebateOpen = reader.GetValue<decimal>("RebateOpen"),
                        RebateConsume = reader.GetValue<decimal>("RebateConsume"),
                        RebateConsume2 = reader.GetValue<decimal>("RebateConsume2"),
                        RebateLevel = reader.GetValue<int>("rebatelevel"),
                        Opencardnum = reader.GetValue<int>("opencardnum"),
                        Firstdealnum = reader.GetValue<int>("firstdealnum"),
                        Summoney = reader.GetValue<decimal>("summoney"),
                        Runstate = reader.GetValue<int>("runstate"),
                        Whetherdefaultchannel = reader.GetValue<int>("whetherdefaultchannel")
                    });

                }
            }
            totalcount = list.Count;

            return list;
        }
        internal Member_Channel GetChannel(int comid, string name)
        {
            const string sqltxt = @"SELECT  [id]
      ,[Com_id]
      ,[Issuetype]
      ,[companyid]
      ,[name]
      ,[mobile]
      ,[cardcode]
      ,[Chaddress]
      ,[ChObjects]
      ,[RebateOpen]
      ,[RebateConsume]
      ,[RebateConsume2]
      ,[rebatelevel]
      ,[opencardnum]
      ,[firstdealnum]
      ,[summoney]
      ,whetherdefaultchannel
  FROM [EtownDB].[dbo].[Member_Channel] where Com_id=@comid  and name=@name and whetherdefaultchannel=0 ";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@name", name);

            Member_Channel u = null;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    u = new Member_Channel()
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Issuetype = reader.GetValue<int>("Issuetype"),
                        Companyid = reader.GetValue<int>("companyid"),
                        Name = reader.GetValue<string>("name"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Cardcode = reader.GetValue<decimal>("cardcode"),
                        Chaddress = reader.GetValue<string>("Chaddress"),
                        ChObjects = reader.GetValue<string>("ChObjects"),
                        RebateOpen = reader.GetValue<decimal>("RebateOpen"),
                        RebateConsume = reader.GetValue<decimal>("RebateConsume"),
                        RebateConsume2 = reader.GetValue<decimal>("RebateConsume2"),
                        RebateLevel = reader.GetValue<int>("rebatelevel"),
                        Opencardnum = reader.GetValue<int>("opencardnum"),
                        Firstdealnum = reader.GetValue<int>("firstdealnum"),
                        Summoney = reader.GetValue<decimal>("summoney"),
                        Whetherdefaultchannel = Convert.IsDBNull(reader.GetValue(16)) == true ? 0 : reader.GetValue<int>("whetherdefaultchannel")
                    };
                }
            }
            return u;
        }

        internal int EditSimplyChannel(int id, int Com_id, string Employeename, string Channelcompanyid, string tel = "", int channelsource = 0, int channelstate = 1)
        {
            try
            {
                if (id == 0)
                {
                    string sql = @"INSERT INTO  [Member_Channel]
                       ([Com_id]
                       ,[Issuetype]
                       ,[companyid]
                       ,[name]
                       ,[mobile]
                       ,[cardcode]
                       ,[Chaddress]
                       ,[ChObjects]
                       ,[RebateOpen]
                       ,[RebateConsume]
                       ,[RebateConsume2]
                       ,[rebatelevel]
                       ,[opencardnum]
                       ,[firstdealnum]
                       ,[summoney]
                       ,[whetherdefaultchannel]
                       ,[runstate])
                 VALUES
                       (" + Com_id + "," + channelsource + "," + Channelcompanyid + ",'" + Employeename + "','" + tel + "',0,'','',0,0,0,0,0,0,0,0," + channelstate + ");select @@identity;";
                    var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                    object o = cmd.ExecuteScalar();
                    return int.Parse(o.ToString());
                }
                else
                {
                    string sql = "update Member_Channel set Issuetype=" + channelsource + ", com_id=" + Com_id + " ,mobile='" + tel + "',companyid=" + Channelcompanyid + ",name='" + Employeename + "',runstate=" + channelstate + "  where id=" + id;
                    var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                    cmd.ExecuteNonQuery();
                    return id;
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        internal int WxMessageLockUser(int channleid, string openid)
        {
            //先清除绑定此会员的顾问的账户
            string sql2 = "update Member_Channel set lockuserweixin='' where lockuserweixin=@openid";
            var cmd2 = sqlHelper.PrepareTextSqlCommand(sql2);
            cmd2.AddParam("@openid", openid);
            cmd2.ExecuteNonQuery();

            //对顾问账户绑定此会员微信号
            string sql = "update Member_Channel set lockuserweixin=@openid,lockusertime=getdate() where id=@channleid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@channleid", channleid);
            cmd.AddParam("@openid", openid);
            cmd.AddParam("@lockusertime", DateTime.Now);
            return cmd.ExecuteNonQuery();
        }

        //对顾问账户绑定此会员微信号进行清空，超时清空暂时不用
        internal int WxMessageLockUserTime(int channleid, string openid)
        {
            //string sql = "update Member_Channel set lockusertime=getdate() where id=@channleid";
            //var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            //cmd.AddParam("@channleid", channleid);
            //cmd.AddParam("@lockusertime", DateTime.Now);
            //return cmd.ExecuteNonQuery();

            return 0;
        }

        internal int WxMessageUnLockUser(int channleid)
        {
            string sql = "update Member_Channel set lockuserweixin='' where id=@channleid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@channleid", channleid);
            cmd.AddParam("@lockusertime", DateTime.Now);
            return cmd.ExecuteNonQuery();
        }

        internal int WxMessageDeadLock(int channleid, int locktype)
        {
            //对顾问账户绑定此会员微信号
            string sql = "update Member_Channel set lockuser=@locktype where id=@channleid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@locktype", locktype);
            cmd.AddParam("@channleid", channleid);
            return cmd.ExecuteNonQuery();
        }
        internal int UserUnlockChannel(decimal cardid, int comid)
        {
            //对顾问账户绑定此会员微信号
            string sql = "update member_card set issuecard=0 where cardcode=@cardid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@cardid", cardid);
            return cmd.ExecuteNonQuery();
        }


        //获取商户或门市渠道列表，只显示微信绑定的在线列表
        internal List<Member_Channel> GetChannelListByComid(int comid, int companyid, int channeltype, out int totalcount, int channelid = 0)
        {
            int weeki = Convert.ToInt32(DateTime.Today.DayOfWeek);

            string sqld = @"SELECT   *
  FROM [EtownDB].[dbo].[Member_Channel] where com_id=@comid and runstate=1  and mobile in (select phone from b2b_crm where com_id=@comid and phone in (select mobile from Member_Channel where mobile !='' and com_id=@comid and runstate=1 and mobile in (select tel from b2b_company_manageuser where com_id=@comid and peoplelistview=1 and employeestate=1 and( workdays like '%" + weeki + "%' or workdays is null) )) and weixin !='' )";

            if (companyid != 0)
            {
                sqld += " and companyid =" + companyid;
            }
            if (channeltype == 1)
            {
                sqld += " and Lockuserweixin=''";
            }

            if (channelid != 0)
            {
                sqld += " and id=" + channelid;
            }


            var cmd = sqlHelper.PrepareTextSqlCommand(sqld);
            cmd.AddParam("@comid", comid);


            List<Member_Channel> list = new List<Member_Channel>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_Channel
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Issuetype = reader.GetValue<int>("Issuetype"),
                        Channeltypename = reader.GetValue<int>("Issuetype") == 1 ? "外部渠道" : "内部渠道",
                        Companyid = reader.GetValue<int>("companyid"),
                        Name = reader.GetValue<string>("name"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Cardcode = reader.GetValue<decimal>("cardcode"),
                        Chaddress = reader.GetValue<string>("Chaddress"),
                        ChObjects = reader.GetValue<string>("ChObjects"),
                        RebateOpen = reader.GetValue<decimal>("RebateOpen"),
                        RebateConsume = reader.GetValue<decimal>("RebateConsume"),
                        RebateConsume2 = reader.GetValue<decimal>("RebateConsume2"),
                        RebateLevel = reader.GetValue<int>("rebatelevel"),
                        Opencardnum = reader.GetValue<int>("opencardnum"),
                        Firstdealnum = reader.GetValue<int>("firstdealnum"),
                        Summoney = reader.GetValue<decimal>("summoney"),
                        Lockuserweixin = reader.GetValue<string>("Lockuserweixin"),
                        Lockusertime = reader.GetValue<DateTime>("Lockusertime"),
                    });

                }
            }
            totalcount = list.Count;

            return list;
        }

        internal int WxMessageUnLockUserTimeout()
        {
            string sql = "update Member_Channel set lockuserweixin='' where LockUserTime>@lockusertime";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@lockusertime", DateTime.Now.AddHours(12));
            return cmd.ExecuteNonQuery();
        }

        //获取商户或门市渠道列表，只显示微信绑定的在线列表
        internal int GetChannelListByComidState(int comid, int channelid)
        {
            int weeki = Convert.ToInt32(DateTime.Today.DayOfWeek);

            string sqld = @"SELECT   *
  FROM [b2b_crm] where com_id=@comid and weixin !='' and phone in (select tel  from b2b_company_manageuser where com_id=@comid and id=" + channelid + "   )";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqld);
            cmd.AddParam("@comid", comid);
            using (var reader = cmd.ExecuteReader())
            {

                if (reader.Read())
                {
                    return 1;
                }
            }
            return 0;
        }



        internal Member_Channel GetChannelByMasterId(int masterid)
        {
            try
            {
                string sql = @"select * from Member_Channel  where com_id=(select com_id from b2b_company_manageuser where id=@masterid)  and mobile = (select tel from b2b_company_manageuser where id=@masterid)";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@masterid", masterid);

                using (var reader = cmd.ExecuteReader())
                {
                    Member_Channel u = null;
                    if (reader.Read())
                    {
                        u = new Member_Channel()
                        {
                            Id = reader.GetValue<int>("Id"),
                            Com_id = reader.GetValue<int>("Com_id"),
                            Issuetype = reader.GetValue<int>("Issuetype"),
                            Companyid = reader.GetValue<int>("companyid"),
                            Name = reader.GetValue<string>("name"),
                            Mobile = reader.GetValue<string>("mobile"),
                            Cardcode = reader.GetValue<decimal>("cardcode"),
                            Chaddress = reader.GetValue<string>("Chaddress"),
                            ChObjects = reader.GetValue<string>("ChObjects"),
                            RebateOpen = reader.GetValue<decimal>("RebateOpen"),
                            RebateConsume = reader.GetValue<decimal>("RebateConsume"),
                            RebateConsume2 = reader.GetValue<decimal>("RebateConsume2"),
                            RebateLevel = reader.GetValue<int>("rebatelevel"),
                            Opencardnum = reader.GetValue<int>("opencardnum"),
                            Firstdealnum = reader.GetValue<int>("firstdealnum"),
                            Summoney = reader.GetValue<decimal>("summoney"),
                            Whetherdefaultchannel = Convert.IsDBNull(reader.GetValue(16)) == true ? 0 : reader.GetValue<int>("whetherdefaultchannel"),
                            Runstate = reader.GetValue<int>("runstate"),
                            Rebatemoney = reader.GetValue<decimal>("rebatemoney"),
                        };
                    }
                    return u;
                }
            }
            catch
            {
                return null;
            }
        }

        internal int IsGuwen(string openid, int comid)
        {
            string sql = "select count(1) from member_channel where  mobile=(select phone from b2b_crm where phone!='' and  weixin='" + openid + "' and com_id=" + comid + ") and com_id=" + comid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            return o == null ? 0 : int.Parse(o.ToString());
        }

        internal decimal Getrestrebate(int comid, string phone)
        {
            try
            {
                string sql = "select  rebatemoney from member_channel where com_id=" + comid + " and  mobile='" + phone + "'";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    decimal m = 0;
                    if (reader.Read())
                    {
                        m = reader.GetValue<decimal>("rebatemoney");
                    }
                    return m;
                }
            }
            catch 
            {
                return 0;
            }
        }

        internal int GetChannelid(int comid, string phone)
        {
            try
            {
                string sql = "select id from member_channel where com_id=" + comid + " and mobile='" + phone + "'";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    int m = 0;
                    if (reader.Read())
                    {
                        m = reader.GetValue<int>("id");
                    }
                    return m;
                }
            }
            catch 
            {
                return 0;
            }
        }
    }
}
