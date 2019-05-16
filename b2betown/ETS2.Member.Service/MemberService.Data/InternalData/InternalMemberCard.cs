using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.Member.Service.MemberService.Model;
using ETS.Data.SqlHelper;
using ETS2.Member.Service.MemberService.Data.InternalData;
using System.Data;

namespace ETS2.Member.Service.MemberService.Data.InternalData
{
    public class InternalMemberCard
    {
        private SqlHelper sqlHelper;
        public InternalMemberCard(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal Member_Card GetCardById(int cardid)
        {
            const string sqltxt = @"SELECT   [id]
      ,[Com_id]
      ,[cname]
      ,[ctype]
      ,[printnum]
      ,[zhuanzeng]
      ,[qrcode]
      ,[exchange]
      ,[remark]
      ,[cardRule]
      ,[cardRule_starnum]
      ,[cardRule_First] 
  FROM [EtownDB].[dbo].[Member_Card_Create] where [Id]=@cardid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@cardid", cardid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Member_Card
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Cname = reader.GetValue<string>("cname"),
                        Ctype = reader.GetValue<int>("ctype"),
                        Printnum = reader.GetValue<int>("Printnum"),
                        Zhuanzeng = reader.GetValue<int>("Zhuanzeng"),
                        Qrcode = reader.GetValue<int>("Qrcode"),
                        Exchange = reader.GetValue<string>("Exchange"),
                        Remark = reader.GetValue<string>("Remark"),
                        CardRule = reader.GetValue<int>("CardRule"),
                        CardRule_starnum = reader.GetValue<int>("CardRule_starnum"),
                        CardRule_First = reader.GetValue<int>("CardRule_First"),

                    };
                }
                return null;
            }
        }
        internal Member_Card GetCardById(int comid, int cardid)
        {
            const string sqltxt = @"SELECT   [id]
      ,[Com_id]
      ,[cname]
      ,[ctype]
      ,[printnum]
      ,[zhuanzeng]
      ,[qrcode]
      ,[exchange]
      ,[remark]
      ,[cardRule]
      ,[cardRule_starnum]
      ,[cardRule_First] 
  FROM [EtownDB].[dbo].[Member_Card_Create] where [Id]=@cardid  and com_id=@comid";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@cardid", cardid);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Member_Card
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Cname = reader.GetValue<string>("cname"),
                        Ctype = reader.GetValue<int>("ctype"),
                        Printnum = reader.GetValue<int>("Printnum"),
                        Zhuanzeng = reader.GetValue<int>("Zhuanzeng"),
                        Qrcode = reader.GetValue<int>("Qrcode"),
                        Exchange = reader.GetValue<string>("Exchange"),
                        Remark = reader.GetValue<string>("Remark"),
                        CardRule = reader.GetValue<int>("CardRule"),
                        CardRule_starnum = reader.GetValue<int>("CardRule_starnum"),
                        CardRule_First = reader.GetValue<int>("CardRule_First"),

                    };
                }
                return null;
            }
        }
        #region  根据卡号id获得卡号等Member_Card
        internal Member_Card GetCardId(int cardid)
        {
            string sqltxt = @"SELECT [id]
      ,[Com_id]
      ,[Crid]
      ,[Cardcode]
      ,[OPENstate]
      ,[OPENsubdate]
      ,[IssueCard]
      ,[issueid]
      ,[ServerCard]
  FROM [EtownDB].[dbo].[Member_Card] where [id]=@cardid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@cardid", cardid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Member_Card
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Crid = reader.GetValue<int>("Crid"),
                        Cardcode = reader.GetValue<decimal>("Cardcode"),
                        Openstate = reader.GetValue<int>("Openstate"),
                        Opensubdate = reader.GetValue<DateTime>("Opensubdate") == null ? DateTime.Parse("1900-01-01") : reader.GetValue<DateTime>("Opensubdate"),
                        IssueCard = reader.GetValue<decimal>("IssueCard"),
                        IssueId = reader.GetValue<int>("IssueId"),

                    };
                }
                return null;
            }
        }
        #endregion

        #region 卡号第一位

        internal Member_Card GetCardFirst(int comid)
        {
            const string sqltxt = @"SELECT  top 1 [id]
      ,[cardRule_First]
  FROM [EtownDB].[dbo].[Member_Card_Create] where [Com_id]=@comid order by cardRule_First desc";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Member_Card
                    {
                        Id = reader.GetValue<int>("Id"),
                        CardRule_First = reader.GetValue<int>("CardRule_First") + 1,
                    };
                }
                return null;
            }
        }
        #endregion

        #region 添加或编辑卡号管理

        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateMemberCardInfo";
        public int InsertOrUpdate(Member_Card model)
        {

            string insertsqltxt = "";//卡号插入SQL
            int cardid = 0;//卡管理ID
            string cardcode = "";//卡号
            int RandomCode = 0;


            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);
            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Com_id", model.Com_id);
            cmd.AddParam("@cname", model.Cname);
            cmd.AddParam("@ctype", model.Ctype);
            cmd.AddParam("@printnum", model.Printnum);
            cmd.AddParam("@zhuanzeng", model.Zhuanzeng);
            cmd.AddParam("@qrcode", model.Qrcode);
            cmd.AddParam("@Exchange", model.Exchange);
            cmd.AddParam("@remark", model.Remark);
            cmd.AddParam("@cardRule", model.CardRule);
            cmd.AddParam("@cardRule_starnum", model.CardRule_starnum);
            cmd.AddParam("@cardRule_First", model.CardRule_First);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();

            cardid = (int)parm.Value;

            //是修改操作将不产生卡号
            if (model.Id == 0)
            {

                //后8位如果是顺序编码2 则执行
                if (model.CardRule == 2)
                {
                    for (int i = model.CardRule_starnum; i < model.CardRule_starnum + model.Printnum; i++)
                    {
                        cardcode = model.CardRule_First + model.CardRule_Second.ToString() + i;
                        insertsqltxt = "insert Member_Card (com_id,crid,cardcode)values(" + model.Com_id + "," + cardid + "," + cardcode + ")";
                        cmd = this.sqlHelper.PrepareTextSqlCommand(insertsqltxt);
                        cmd.ExecuteNonQuery();
                    }
                }
                //后8位采用随机编码1
                else
                {
                    for (int i = 1; i < model.Printnum; i++)
                    {
                        //获得随机码
                        RandomCode = MemberCardData.GetRandomCode();
                        //使用随机码时，标记为已使用防止重复码
                        insertsqltxt = "update RandomCode set state = 1 where code = " + RandomCode;
                        cmd = this.sqlHelper.PrepareTextSqlCommand(insertsqltxt);
                        cmd.ExecuteNonQuery();

                        //插入卡库Member_Card
                        cardcode = model.CardRule_First + model.CardRule_Second.ToString() + RandomCode;
                        insertsqltxt = "insert Member_Card (com_id,crid,cardcode)values(" + model.Com_id + "," + cardid + "," + cardcode + ")";
                        cmd = this.sqlHelper.PrepareTextSqlCommand(insertsqltxt);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            return (int)parm.Value;
        }
        #endregion

        #region 创建电子码，兵插入活动 cardtype=1为网站，cardtype=2为微信
        public string CreateECard(int cardtype, int comid)
        {
            string insertsqltxt = "";//卡号插入SQL
            string cardcode = "";//卡号 
            int Face = 1;//活动面向对象 Face=3网站 Face=4微信


            //从公司自己导入的卡号中获取卡号，如果没有的话则创建
            cardcode = new MemberCardData().GetUnusedOutCardcode(comid);
            if (cardcode == "")
            {
                int RandomCode = 0;
                int Firstcard = 0;
                int Scountcard = Int16.Parse(DateTime.Now.ToString("yyMM"));

                if (cardtype == 1)
                {
                    Firstcard = 2002;
                    Face = 3;
                }
                else
                {
                    Firstcard = 2001;
                    Face = 4;
                }

                //获得随机码
                RandomCode = MemberCardData.GetRandomCode();
                //使用随机码时，标记为已使用防止重复码
                insertsqltxt = "update RandomCode set state = 1 where code = " + RandomCode;
                var cmda = this.sqlHelper.PrepareTextSqlCommand(insertsqltxt);
                cmda.ExecuteNonQuery();


                //插入卡库Member_Card
                cardcode = Firstcard.ToString() + Scountcard.ToString() + RandomCode;
            }
            else
            {
                //把得到的外部导入会员卡号标注为已使用，防止重复
                insertsqltxt = "update Out_MemberCardCode set isused = 1 where outcardcode = '" + cardcode + "'";
                var cmda = this.sqlHelper.PrepareTextSqlCommand(insertsqltxt);
                cmda.ExecuteNonQuery();
            }



            insertsqltxt = "insert Member_Card (com_id,crid,cardcode,openstate)values(" + comid + ",0," + cardcode + ",1);select @@identity;";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(insertsqltxt);
            cmd = this.sqlHelper.PrepareTextSqlCommand(insertsqltxt);
            string cardid = cmd.ExecuteScalar().ToString();


            //直接插入活动指定活动3=网站活动，4=微信活动
            insertsqltxt = "insert into Member_Card_Activity (CardID,ACTID ) select " + cardid + ",id from Member_Activity where  com_id=" + comid + " and FaceObjects=" + Face;
            cmd = this.sqlHelper.PrepareTextSqlCommand(insertsqltxt);
            cmd.ExecuteNonQuery();

            DateTime today = DateTime.Now;

            //当时微信注册或网站注册的 积分活动，自动已赠送，活动变成已使用
            string updatesqltxt = "update member_card_activity set actnum=0,usestate=2,usesubdate='" + today + "' where cardid=" + cardid + " and actid in (select id from Member_Activity where acttype=4 and faceobjects in (4,3))";
            cmd = this.sqlHelper.PrepareTextSqlCommand(updatesqltxt);
            cmd.ExecuteNonQuery();

            return cardcode;
        }
        #endregion




        #region  自动打入积分，网站注册，和微信注册。如果活动自动执行 carttype：类型3=网站，4=微信
        public int AutoInputMoeny(int uid, int faceobjects, int comid, out int jifen_range)
        {
            //获取活动赠送 积分/积分

            decimal money = 0;
            string sqltxt = @"SELECT *
  FROM  [Member_Activity] where faceobjects=@faceobjects and com_id=@comid and runstate=1 and Acttype=4 and actstar<=getdate() and actend>=getdate()";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@faceobjects", faceobjects);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    money = money + reader.GetValue<decimal>("money");
                }
            }

            if (money != 0)
            {
                string tmoney = money.ToString("0");
                int tjifen_range = int.Parse(tmoney);
                jifen_range = tjifen_range;
                //jifen_range = int.Parse(money.ToString());
                string insertsqltxt = " INSERT Member_Integral_Log (com_id,mid,money,ptype,admin,ip,orderid,orderName)values(@Comid,@Id,@Money,@Ptype,@Admin,@Ip,@orderid,@ordername)";
                cmd = this.sqlHelper.PrepareTextSqlCommand(insertsqltxt);
                cmd.AddParam("@Comid", comid);
                cmd.AddParam("@Id", uid);
                cmd.AddParam("@Money", money);
                cmd.AddParam("@Ptype", 1);
                cmd.AddParam("@Admin", "活动领取");
                cmd.AddParam("@Ip", "");
                cmd.AddParam("@orderid", 0);
                cmd.AddParam("@ordername", "活动领取");
                cmd.ExecuteNonQuery();


                //录入积分/积分
                insertsqltxt = "update b2b_crm set integral = integral+" + money + " where id = " + uid + " and com_id=" + comid;
                cmd = this.sqlHelper.PrepareTextSqlCommand(insertsqltxt);
                return cmd.ExecuteNonQuery();
            }

            jifen_range = 0;
            return 0;

        }
        #endregion


        #region 卡号管理列表
        internal List<Member_Card> CardPageList(string comid, int pageindex, int pagesize, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "Member_Card_Create";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";
            var condition = "com_id=" + comid;
            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<Member_Card> list = new List<Member_Card>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_Card
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Cname = reader.GetValue<string>("cname"),
                        Ctype = reader.GetValue<int>("Ctype"),
                        Printnum = reader.GetValue<int>("Printnum"),
                        Zhuanzeng = reader.GetValue<int>("Zhuanzeng"),
                        Qrcode = reader.GetValue<int>("Qrcode"),
                        Exchange = reader.GetValue<string>("Exchange"),
                        Remark = reader.GetValue<string>("Remark"),
                        CardRule = reader.GetValue<int>("CardRule"),
                        CardRule_starnum = reader.GetValue<int>("CardRule_starnum"),
                        CardRule_First = reader.GetValue<int>("CardRule_First"),
                        Subdate = reader.GetValue<DateTime>("subdate"),
                        Outstate = reader.GetValue<bool>("outstate"),
                        Createstate = reader.GetValue<bool>("Createstate"),

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }
        #endregion


        #region 得到随机编号吗
        public int GetRandomCode()
        {

            const string sqltxt = @"SELECT  top 1 [id]
      ,[code]
  FROM [EtownDB].[dbo].[RandomCode] where [state]=0 ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("code");

                }
                return 0;
            }
        }
        #endregion

        #region 编辑卡片管理 by:xiaoliu
        private static readonly string SQLInsertOrUpdate2 = "usp_InsertOrUpdateMemberCard2";
        internal int EditMemberCard(Member_Card model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate2);

            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Comid", model.Com_id);
            cmd.AddParam("@Crid", model.Crid);
            cmd.AddParam("@Cardcode", model.Cardcode);
            cmd.AddParam("@OPENstate", model.Openstate);
            cmd.AddParam("@OPENsubdate", model.Opensubdate);
            cmd.AddParam("@IssueCard", model.IssueCard);



            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;


        }
        #endregion

        internal Member_Card GetCardByCardNumber(decimal cardnumber)
        {
            const string sqltxt = @"SELECT [id]
      ,[Com_id]
      ,[Crid]
      ,[Cardcode]
      ,[OPENstate]
      ,[OPENsubdate]
      ,[IssueCard]
      ,ServerCard
  FROM [EtownDB].[dbo].[Member_Card] where cardcode=@cardcode";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@cardcode", cardnumber);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Member_Card
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Crid = reader.GetValue<int>("Crid"),
                        Cardcode = reader.GetValue<decimal>("Cardcode"),
                        Openstate = reader.GetValue<int>("OPENstate"),
                        Opensubdate = reader.GetValue<DateTime>("OPENsubdate"),
                        IssueCard = reader.GetValue<decimal>("IssueCard"),
                        ServerCard = reader.GetValue<decimal>("ServerCard")
                    };
                }
                return null;
            }
        }

        #region 更改生成卡号表中卡号所属渠道的卡号
        internal int UPChannelCardCode(decimal channelcardcode, decimal cardnumber, int issueid)
        {
            string sqltext = @"update Member_Card set IssueCard=@channelcardcode,issueid=@issueid where cardcode=@cardcode";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqltext);
            cmd.AddParam("@cardcode", cardnumber);
            cmd.AddParam("@channelcardcode", channelcardcode);
            cmd.AddParam("@issueid", issueid);

            return cmd.ExecuteNonQuery();



        }
        #endregion
        #region 获得此次发行已经录入的卡号数量
        internal int GetEnteredNumber(int issueid)
        {
            string sqltext = @"select count(1) from Member_Card where issueid=@issueid  and IssueCard>0";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqltext);
            cmd.AddParam("@issueid", issueid);

            return (int)cmd.ExecuteScalar();
        }
        #endregion
        #region 判断卡号是否已经录入
        internal int IsHasEntered(decimal cardnumber)
        {
            string sqltext = @"select count(1) from Member_Card where cardcode=@cardnumber and issuecard>0 ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqltext);
            cmd.AddParam("@cardnumber", cardnumber);

            return (int)cmd.ExecuteScalar();
        }
        #endregion

        internal Member_Card GetCardByIssueId(int issueid)
        {
            const string sqltxt = @"SELECT   [id]
      ,[Com_id]
      ,[cname]
      ,[ctype]
      ,[printnum]
      ,[zhuanzeng]
      ,[qrcode]
      ,[exchange]
      ,[remark]
      ,[cardRule]
      ,[cardRule_starnum]
      ,[cardRule_First]
  FROM [EtownDB].[dbo].[Member_Card_Create] where [Id] in (select Crid from Member_Issue where id=@issueid) ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@issueid", issueid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Member_Card
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Cname = reader.GetValue<string>("cname"),
                        Ctype = reader.GetValue<int>("ctype"),
                        Printnum = reader.GetValue<int>("Printnum"),
                        Zhuanzeng = reader.GetValue<int>("Zhuanzeng"),
                        Qrcode = reader.GetValue<int>("Qrcode"),
                        Exchange = reader.GetValue<string>("Exchange"),
                        Remark = reader.GetValue<string>("Remark"),
                        CardRule = reader.GetValue<int>("CardRule"),
                        CardRule_starnum = reader.GetValue<int>("CardRule_starnum"),
                        CardRule_First = reader.GetValue<int>("CardRule_First"),

                    };
                }
                return null;
            }
        }

        internal Member_Card GetCardCreateByCrid(int crid)
        {
            const string sqltxt = @"SELECT   [id]
      ,[Com_id]
      ,[cname]
      ,[ctype]
      ,[printnum]
      ,[zhuanzeng]
      ,[qrcode]
      ,[exchange]
      ,[remark]
      ,[cardRule]
      ,[cardRule_starnum]
      ,[cardRule_First]
  FROM [EtownDB].[dbo].[Member_Card_Create] where [Id] =@id ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@id", crid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Member_Card
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Cname = reader.GetValue<string>("cname"),
                        Ctype = reader.GetValue<int>("ctype"),
                        Printnum = reader.GetValue<int>("Printnum"),
                        Zhuanzeng = reader.GetValue<int>("Zhuanzeng"),
                        Qrcode = reader.GetValue<int>("Qrcode"),
                        Exchange = reader.GetValue<string>("Exchange"),
                        Remark = reader.GetValue<string>("Remark"),
                        CardRule = reader.GetValue<int>("CardRule"),
                        CardRule_starnum = reader.GetValue<int>("CardRule_starnum"),
                        CardRule_First = reader.GetValue<int>("CardRule_First"),

                    };
                }
                return null;
            }
        }

        internal int GetOpenCardNum(int issueid)
        {
            string sqltext = @"select count(1) from Member_Card where issueid=@issueid  and [OPENstate]=1";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqltext);
            cmd.AddParam("@issueid", issueid);

            return (int)cmd.ExecuteScalar();
        }

        internal int GetEnteredNumberByChannelId(int channelid)
        {
            string sqltext = @"select count(1) from Member_Card where [IssueCard]=@channelid ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqltext);
            cmd.AddParam("@channelid", channelid);

            return (int)cmd.ExecuteScalar();
        }



        #region 判断卡号是否已经录入
        internal string SearchCard(string pno, int comid, out int userinfo)
        {
            string sqltxt = "";

            //如果大于11为卡号则查询卡号右查询，否则为手机号（手机号为11位，卡号16位）
            if (pno.ToString().Length > 11)
            {
                sqltxt = @"select  a.id,b.com_id,b.cardcode,b.openstate from b2b_crm as a  right  join  member_card as b 
on a.idcard=b.cardcode and a.com_id=b.com_id
where  b.cardcode=" + pno + " and b.com_id=" + comid + "";
            }
            else
            {
                sqltxt = @"select  a.id,b.com_id,b.cardcode,b.openstate from b2b_crm as a left join  member_card as b 
on a.idcard=b.cardcode and a.com_id=b.com_id
where  a.phone='" + pno + "' and b.com_id=" + comid + "";

            }
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            //cmd.AddParam("@cardcode", pno.ToString());
            //cmd.AddParam("@comid", comid);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    if (reader.GetValue<int>("openstate") == 0)
                    {
                        userinfo = 0;
                        return "您输入的卡号尚未开卡，请登陆v.vctrip.com进行开卡";
                    }
                    else
                    {
                        userinfo = reader.GetValue<int>("id");
                        return "OK";
                    }
                }
                else
                {
                    userinfo = 0;
                    return "没有查询到账户！";
                }
            }
        }
        #endregion


        #region 确认使用活动
        internal string EconfirmCard(int aid, int actid, int cardid, int comid, out Member_Activity actinfo, out string phone, out string name, out decimal idcard, out decimal agcardcode)
        {
            string sqltxt = "";
            sqltxt = @"select a.id,b.id as aid,a.ReturnAct,b.cardid,a.com_id,a.title,a.Acttype,a.discount,a.money,a.cashfull,a.cashback,a.faceobjects,a.repeatissue,a.returnact,a.useonce,a.actstar,a.actend,a.runstate,b.USEstate,b.Actnum,d.name,d.idcard,d.phone,e.cardcode from Member_Activity as a left join Member_Card_Activity as b on a.id=b.actid  left join member_card as c on b.cardid=c.id left join b2b_crm as d on c.cardcode=d.idcard and c.com_id=d.com_id left join member_channel as e on c.issuecard=e.id and e.com_id=e.com_id where a.com_id=@comid and b.id=@aid and b.cardid=@cardid and b.USEstate=1 order by a.id desc";

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@aid", aid);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@cardid", cardid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {

                    actinfo = new Member_Activity
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Title = reader.GetValue<string>("title"),
                        Money = reader.GetValue<decimal>("money"),
                        Acttype = reader.GetValue<int>("Acttype"),
                        //Cashback = reader.GetValue<int>("Cashback"),
                        //CashFull = reader.GetValue<int>("CashFull"),
                        //Discount = reader.GetValue<double>("Discount"),
                        Actnum = reader.GetValue<int>("Actnum"),
                        Runstate = reader.GetValue<bool>("Runstate"),
                    };

                    phone = reader.GetValue<string>("phone");
                    name = reader.GetValue<string>("name");
                    idcard = reader.GetValue<decimal>("idcard");
                    agcardcode = reader.GetValue<decimal>("cardcode");
                    int ReturnAct = reader.GetValue<int>("ReturnAct");

                    reader.Close();
                    //修改活动状态
                    sqltxt = @"update member_card_activity set Actnum=0,USEstate=2,USEsubdate=@subdate where id=@aid";
                    var cmd1 = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
                    cmd1.AddParam("@aid", aid);
                    cmd1.AddParam("@subdate", DateTime.Now);
                    cmd1.ExecuteNonQuery();

                    //赠送活动
                    if (ReturnAct != 0)
                    {
                        sqltxt = @"insert member_card_activity (cardid,actid) values (@cardid,@ReturnAct)";
                        var cmd2 = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
                        cmd2.AddParam("@cardid", cardid);
                        cmd2.AddParam("@ReturnAct", ReturnAct);
                        cmd2.ExecuteNonQuery();
                    }

                    return "OK";
                }
                else
                {
                    actinfo = null;
                    phone = "";
                    name = "";
                    idcard = 0;
                    agcardcode = 0;
                    return "此账户使用优惠活动错误，请重新操作！";
                }
            }



        }
        #endregion



        #region 确认使用活动
        internal string GetCardInfo(int actid, int pno, int comid)
        {
            return actid.ToString();
        }
        #endregion

        internal List<Member_Card> GetMemberCardList(int comid, decimal cardcode, int pageindex, int pagesize, int issueid, int channelid, int actid, int isopencard, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "Member_Card";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";
            var condition = "com_id=" + comid;
            if (cardcode != 0)
            {
                condition += " and cardcode=" + cardcode;
            }
            if (issueid != 0)
            {
                condition += " and issueid=" + issueid;
            }
            if (channelid != 0)
            {
                condition += " and issuecard=" + channelid;
            }
            if (actid != 0)
            {
                condition += " and issueid in (select isid from Member_Issue_Activity where actid=" + actid + ")";
            }
            if (isopencard != 0)
            {
                if (isopencard == 1)//已经开卡1
                {
                    condition += " and openstate=1";
                }
                else//未开卡2
                {
                    condition += " and openstate=0";
                }

            }
            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<Member_Card> list = new List<Member_Card>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_Card
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Crid = reader.GetValue<int>("Crid"),
                        Cardcode = reader.GetValue<decimal>("Cardcode"),
                        Openstate = reader.GetValue<int>("Openstate"),
                        Opensubdate = reader.GetValue<DateTime?>("Opensubdate") == null ? DateTime.Parse("1900-01-01") : reader.GetValue<DateTime>("Opensubdate"),
                        IssueCard = reader.GetValue<decimal>("IssueCard"),
                        IssueId = reader.GetValue<int>("IssueId"),


                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }


        #region 电脑验卡
        public string GetCarValidate(int actid, int cardid, int orderid, string servername, int num_people, int per_capita_money, int menber_return_money, string sales_admin, int comid, string AccountName)
        {
            string sqltxt = "";
            sqltxt = @"INSERT INTO Member_Activity_Log(ACTID,CardID,OrderId,ServerName,Num_people,Per_capita_money,sales_admin,Member_return_money,Usesubdate,comid,logname ) VALUES (@ACTID,@CardID,@OrderId,@ServerName,@Num_people,@Per_capita_money,@sales_admin,@Member_return_money,@Usesubdate,@comid,@logname)";
            var com = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            com.AddParam("@ACTID", actid);
            com.AddParam("@CardID", cardid);
            com.AddParam("@OrderId", orderid);
            com.AddParam("@ServerName", servername);
            com.AddParam("@Num_people", num_people);
            com.AddParam("@Per_capita_money", per_capita_money);
            com.AddParam("@sales_admin", sales_admin);
            com.AddParam("@Member_return_money", menber_return_money);
            com.AddParam("@Usesubdate", DateTime.Now);
            com.AddParam("@comid", comid);
            com.AddParam("@logname", AccountName);
            com.ExecuteNonQuery();
            return "OK";
        }
        #endregion
        #region membercard 卡号和渠道id联系起来
        internal int upCardcodeChannel(string cardcode, int channelid)
        {
            const string sqlTxt = @"update [Member_Card] set [IssueCard]=@channelid where [Cardcode]=@cardcode";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@channelid", channelid);
            cmd.AddParam("@cardcode", cardcode);

            return cmd.ExecuteNonQuery();

        }
        #endregion

        internal Member_Card GetMemberCardList(string openid)
        {
            const string sqltxt = @"select * from member_card where cardcode in (select idcard from b2b_crm where weixin=@openid)";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@openid", openid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Member_Card
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Crid = reader.GetValue<int>("Crid"),
                        Cardcode = reader.GetValue<decimal>("Cardcode"),
                        Openstate = reader.GetValue<int>("Openstate"),
                        Opensubdate = reader.GetValue<DateTime?>("Opensubdate") == null ? DateTime.Parse("1900-01-01") : reader.GetValue<DateTime>("Opensubdate"),
                        IssueCard = reader.GetValue<decimal>("IssueCard"),
                        IssueId = reader.GetValue<int>("IssueId"),

                    };
                }
                return null;
            }
        }

        internal int UpMemberChennel(string openid, int channelid)
        {
            string sql = "update Member_Card set IssueCard=" + channelid + " where cardcode in (select idcard from b2b_crm where weixin='" + openid + "')";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();

        }

        internal int IsHasOutCardcode(int comid, string outcardcode)
        {
            string sql = "select count(1) from out_membercardcode where comid=" + comid + " and outcardcode='" + outcardcode + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            return int.Parse(o.ToString());
        }

        internal int InsOutMemberCardcode(string outcardcode, int isused, int comid, int imlogid)
        {
            string sql = @"INSERT  [Out_MemberCardCode]
                               ([outcardcode]
                               ,[isused]
                               ,[comid]
                               ,[imlogid])
                         VALUES
                               (@outcardcode 
                               ,@isused 
                               ,@comid 
                               ,@imlogid);select @@identity;";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@outcardcode", outcardcode);
            cmd.AddParam("@isused", isused);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@imlogid", imlogid);
            object o = cmd.ExecuteScalar();
            return int.Parse(o.ToString());
        }

        internal int Insoutcardcodeimlog(string initfilename, string relativepath, int imtor, int comid, string imtime)
        {
            string sql = @"INSERT [Out_MemberCardCodeImLog]
                               ([relativepath]
                               ,[importor]
                               ,[comid]
                               ,[improttime]
                               ,initfilename)
                         VALUES
                               (@relativepath 
                               ,@importor 
                               ,@comid 
                               ,@improttime,@initfilename);select @@identity;";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@relativepath", relativepath);
            cmd.AddParam("@importor", imtor);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@improttime", imtime);
            cmd.AddParam("@initfilename", initfilename);
            object o = cmd.ExecuteScalar();
            return int.Parse(o.ToString());
        }

        internal string GetUnusedOutCardcode(int comid)
        {
            string sql = "select top 1 outcardcode  from Out_MemberCardCode where comid=" + comid + " and  isused=0";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                string outcardcode = "";
                if (reader.Read())
                {
                    outcardcode = reader.GetValue<string>("outcardcode");
                }
                return outcardcode;
            }
        }
    }
}
