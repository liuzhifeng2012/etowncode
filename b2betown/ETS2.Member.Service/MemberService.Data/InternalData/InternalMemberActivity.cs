using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.Member.Service.MemberService.Model;
using ETS.Data.SqlHelper;

namespace ETS2.Member.Service.MemberService.Data.InternalData
{
    public class InternalMemberActivity
    {
        private SqlHelper sqlHelper;
        public InternalMemberActivity(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal Member_Activity GetActById(int actid)
        {
            const string sqltxt = @"SELECT   [id]
      ,[Com_id]
      ,[title]
      ,[Atitle]
      ,[Acttype]
      ,[Money]
      ,[Discount]
      ,[CashFull]
      ,[Cashback]
      ,[UseOnce]
      ,[RepeatIssue]
      ,[Actstar]
      ,[Actend]
      ,[FaceObjects]
      ,[ReturnAct]
      ,[Runstate]
      ,[remark]
      ,[useremark]
      ,[usetitle]
      ,createuserid
      ,createtime
  FROM [EtownDB].[dbo].[Member_Activity] where [Id]=@actid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@actid", actid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Member_Activity
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Title = reader.GetValue<string>("title"),
                        Acttype = reader.GetValue<int>("Acttype"),
                        Money = reader.GetValue<decimal>("Money"),
                        Discount = reader.GetValue<double>("Discount"),
                        CashFull = reader.GetValue<decimal>("CashFull"),
                        Cashback = reader.GetValue<decimal>("Cashback"),
                        UseOnce = reader.GetValue<bool>("UseOnce"),
                        RepeatIssue = reader.GetValue<int>("RepeatIssue"),
                        Actstar = reader.GetValue<DateTime>("Actstar"),
                        Actend = reader.GetValue<DateTime>("Actend"),
                        FaceObjects = reader.GetValue<int>("FaceObjects"),
                        ReturnAct = reader.GetValue<int>("ReturnAct"),
                        Runstate = reader.GetValue<bool>("Runstate"),
                        Atitle = reader.GetValue<string>("Atitle"),
                        Remark = reader.GetValue<string>("Remark"),
                        Useremark = reader.GetValue<string>("Useremark"),
                        Usetitle = reader.GetValue<string>("Usetitle"),
                        CreateUserId = reader.GetValue<int>("createuserid"),
                        CreateTime = reader.GetValue<DateTime>("createtime")
                    };
                }
                return null;
            }
        }



        #region 添加或者编辑促销活动信息
        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateMemberActInfo";
        public int InsertOrUpdate(Member_Activity model)
        {

            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);
            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Com_id", model.Com_id);
            cmd.AddParam("@title", model.Title);
            cmd.AddParam("@Acttype", model.Acttype);
            cmd.AddParam("@Money", model.Money);
            cmd.AddParam("@Discount", model.Discount);
            cmd.AddParam("@CashFull", model.CashFull);
            cmd.AddParam("@Cashback", model.Cashback);
            cmd.AddParam("@UseOnce", model.UseOnce);
            cmd.AddParam("@RepeatIssue", model.RepeatIssue);
            cmd.AddParam("@Actstar", model.Actstar);
            cmd.AddParam("@Actend", model.Actend);
            cmd.AddParam("@FaceObjects", model.FaceObjects);
            cmd.AddParam("@ReturnAct", model.ReturnAct);
            cmd.AddParam("@Runstate", model.Runstate);
            cmd.AddParam("@Atitle", model.Atitle);
            cmd.AddParam("@Remark", model.Remark);
            cmd.AddParam("@Useremark", model.Useremark);
            cmd.AddParam("@Usetitle", model.Usetitle);
            cmd.AddParam("@Createuserid", model.CreateUserId);
            cmd.AddParam("@Createtime", model.CreateTime);


            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();

            string insertsqltxt = "";

            //先删除原数据， 使用门店
            insertsqltxt = "delete Member_Act_Ch_Company where actid = " + (int)parm.Value;
            cmd = this.sqlHelper.PrepareTextSqlCommand(insertsqltxt);
            cmd.ExecuteNonQuery();

            if (model.Usechannel == "0")
            {
                //重新插入， 使用门店
                insertsqltxt = "insert Member_Act_Ch_Company (actid,companyid) values (" + (int)parm.Value + ",0)";
                cmd = this.sqlHelper.PrepareTextSqlCommand(insertsqltxt);
                cmd.ExecuteNonQuery();

            }
            else
            {
                //分割后重新插入， 使用门店
                string[] s = model.Usechannel.Split(new char[] { ',' });
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i].Trim() != "" && s[i].Trim() != null)
                    {
                        insertsqltxt = "insert Member_Act_Ch_Company (actid,companyid) values (" + (int)parm.Value + "," + s[i].Trim() + ")";
                        cmd = this.sqlHelper.PrepareTextSqlCommand(insertsqltxt);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            return (int)parm.Value;
        }
        #endregion

        #region 促销活动列表
        internal List<Member_Activity> ActPageList(string comid, int pageindex, int pagesize, out int totalcount,string state="0,1")
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "Member_Activity";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";
            var condition = "com_id=" + comid+" and runstate in ("+state+")";
            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<Member_Activity> list = new List<Member_Activity>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_Activity
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Title = reader.GetValue<string>("title"),
                        Acttype = reader.GetValue<int>("Acttype"),
                        Discount = reader.GetValue<double>("Discount"),
                        Money = reader.GetValue<decimal>("Money"),
                        CashFull = reader.GetValue<decimal>("CashFull"),
                        Cashback = reader.GetValue<decimal>("Cashback"),
                        FaceObjects = reader.GetValue<int>("FaceObjects"),
                        RepeatIssue = reader.GetValue<int>("RepeatIssue"),
                        ReturnAct = reader.GetValue<int>("ReturnAct"),
                        UseOnce = reader.GetValue<bool>("UseOnce"),
                        Actstar = reader.GetValue<DateTime>("Actstar"),
                        Actend = reader.GetValue<DateTime>("Actend"),
                        Runstate = reader.GetValue<bool>("Runstate"),
                        CreateUserId = reader.GetValue<int>("createuserid"),
                        CreateTime = reader.GetValue<DateTime>("createtime")
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }
        #endregion



        #region 微信网站促销活动列表 3=网页，4=微信
        internal List<Member_Activity> ActWeixinPageList(string comid, int Face)
        {
            int pageindex = 1;
            int pagesize = 10;
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "Member_Activity";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";
            var condition = "com_id=" + comid + " and FaceObjects=" + Face;
            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);
            List<Member_Activity> list = new List<Member_Activity>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_Activity
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Title = reader.GetValue<string>("title"),
                        Acttype = reader.GetValue<int>("Acttype"),
                        Discount = reader.GetValue<double>("Discount"),
                        Money = reader.GetValue<decimal>("Money"),
                        CashFull = reader.GetValue<decimal>("CashFull"),
                        Cashback = reader.GetValue<decimal>("Cashback"),
                        FaceObjects = reader.GetValue<int>("FaceObjects"),
                        RepeatIssue = reader.GetValue<int>("RepeatIssue"),
                        ReturnAct = reader.GetValue<int>("ReturnAct"),
                        UseOnce = reader.GetValue<bool>("UseOnce"),
                        Actstar = reader.GetValue<DateTime>("Actstar"),
                        Actend = reader.GetValue<DateTime>("Actend"),
                        Runstate = reader.GetValue<bool>("Runstate"),
                    });

                }
            }
            //totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }
        #endregion


        #region 根据卡号查询促销活动列表，channelcompanyid=查询用户的所属渠道，作用：当客户看到时根据用户查询到所属门市及总社的所有活动 。channelcomid=商户登陆账户的所属渠道，作用：商户验证会员卡 时，查只能查询到此商户所兑换的服务，也就是说当总社推出的一个活动此门市未参与则不能验证此活动
        internal List<Member_Activity> AccountActPageList(int accountid, int comid, int channelcompanyid, int pageindex, int pagesize, out int totalcount,int channelcomid=0)
        {
            //var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            //var tblName = "Member_Activity as a left join Member_Card_Activity as b on a.id=b.actid";
            //var strGetFields = "a.id,a.com_id,a.title,a.Acttype,a.discount,a.money,a.cashfull,a.cashback,a.faceobjects,a.repeatissue,a.returnact,a.useonce,a.actstar,a.actend,a.runstate,b.USEstate,b.Actnum";
            //var sortKey = "a.id";
            //var sortMode = "1";
            //var condition = "a.com_id=" + comid + "and a.id in (select actid from Member_Card_Activity where CardID=" + accountid + " and USEstate=1)";
            //cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            string sqltxt = @"select a.id,b.id as aid,b.cardid,a.com_id,a.title,a.Acttype,a.discount,a.money,a.cashfull,a.cashback,a.faceobjects,a.repeatissue,a.returnact,a.useonce,a.actstar,a.actend,a.runstate,b.USEstate,b.Actnum,a.remark,a.usetitle,a.useremark from Member_Activity as a left join Member_Card_Activity as b on a.id=b.actid where a.com_id=@comid and b.CardID=@accountid and a.actend>=getdate()";


            if (channelcompanyid != 0)//客户看到才传递此参数
            {
                sqltxt = sqltxt + " and a.id in( select actid from member_act_ch_company where companyid = @channelcompanyid or companyid=0)";
            }
            else
            {//否则 可能为验证传递过来的
                if (channelcomid == 0)
                {
                    //总社会员只显示总社活动，如果现实所有渠道活动则可能出现当关注一家门市时，活动都看不到了
                    //sqltxt = sqltxt + " and a.id in( select actid from member_act_ch_company where companyid=0)";
                }
                else { 
                    //验证查询
                    sqltxt = sqltxt + " and a.id in( select actid from member_act_ch_company where companyid=@channelcomid  )";
                }
            }

            sqltxt = sqltxt + "  and a.Runstate=1 order by b.USEstate,b.id desc";


            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@accountid", accountid);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@channelcompanyid", channelcompanyid);
            cmd.AddParam("@channelcomid", channelcomid);
            

            int inum = 0;
            List<Member_Activity> list = new List<Member_Activity>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    inum++;
                    list.Add(new Member_Activity
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Title = reader.GetValue<string>("title"),
                        Acttype = reader.GetValue<int>("Acttype"),
                        Discount = reader.GetValue<double>("Discount"),
                        Money = reader.GetValue<decimal>("Money"),
                        CashFull = reader.GetValue<decimal>("CashFull"),
                        Cashback = reader.GetValue<decimal>("Cashback"),
                        FaceObjects = reader.GetValue<int>("FaceObjects"),
                        RepeatIssue = reader.GetValue<int>("RepeatIssue"),
                        ReturnAct = reader.GetValue<int>("ReturnAct"),
                        UseOnce = reader.GetValue<bool>("UseOnce"),
                        Actstar = reader.GetValue<DateTime>("Actstar"),
                        Actend = reader.GetValue<DateTime>("Actend"),
                        Runstate = reader.GetValue<bool>("Runstate"),
                        Usestate = reader.GetValue<int>("Usestate"),
                        Actnum = reader.GetValue<int>("Actnum"),
                        Cardid = reader.GetValue<int>("Cardid"),
                        Aid = reader.GetValue<int>("aid"),
                        Remark = reader.GetValue<string>("Remark"),
                        Usetitle = reader.GetValue<string>("Usetitle"),
                        Useremark = reader.GetValue<string>("Useremark"),


                    });

                }
            }
            //totalcount = int.Parse(cmd.Parameters[7].Value.ToString());
            totalcount = inum;
            return list;
        }
        #endregion

        #region 根据卡号查询促销活动列表
        internal List<Member_Activity> AccountUnActPageList(int accountid, int comid, int channelcompanyid, out int totalcount)
        {
            string sqltxt = "";

            if (accountid == 0)
            {
                sqltxt = @"select a.id,a.com_id,a.title,a.Acttype,a.discount,a.money,a.cashfull,a.cashback,a.faceobjects,a.repeatissue,a.returnact,a.useonce,a.actstar,a.actend,a.runstate,a.remark,a.usetitle,a.useremark  from Member_Activity as a  where a.com_id=@comid  and a.faceobjects=1  and a.actstar<=@today and a.actend>=@today";
            }
            else
            {
                sqltxt = @"select a.id,a.com_id,a.title,a.Acttype,a.discount,a.money,a.cashfull,a.cashback,a.faceobjects,a.repeatissue,a.returnact,a.useonce,a.actstar,a.actend,a.runstate,a.remark,a.usetitle,a.useremark  from Member_Activity as a  where not a.id in (select actid from Member_Card_Activity as b where  b.CardID=@accountid) and a.com_id=@comid  and a.faceobjects=1  and a.actstar<=@today and a.actend>=@today";
            }

            if (channelcompanyid != 0)
            {
                sqltxt = sqltxt + " and a.id in( select actid from member_act_ch_company where companyid = @channelcompanyid or companyid=0)";
            }
            else {
                //总社会员只显示总社活动，如果现实所有渠道活动则可能出现当关注一家门市时，活动都看不到了
                sqltxt = sqltxt + " and a.id in( select actid from member_act_ch_company where companyid=0)";
            }


            sqltxt = sqltxt + " and a.Runstate=1  order by a.id desc";


            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@accountid", accountid);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@channelcompanyid", channelcompanyid);
            cmd.AddParam("@today", DateTime.Now);


            int inum = 0;
            List<Member_Activity> list = new List<Member_Activity>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    inum++;
                    list.Add(new Member_Activity
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Title = reader.GetValue<string>("title"),
                        Acttype = reader.GetValue<int>("Acttype"),
                        Discount = reader.GetValue<double>("Discount"),
                        Money = reader.GetValue<decimal>("Money"),
                        CashFull = reader.GetValue<decimal>("CashFull"),
                        Cashback = reader.GetValue<decimal>("Cashback"),
                        FaceObjects = reader.GetValue<int>("FaceObjects"),
                        RepeatIssue = reader.GetValue<int>("RepeatIssue"),
                        ReturnAct = reader.GetValue<int>("ReturnAct"),
                        UseOnce = reader.GetValue<bool>("UseOnce"),
                        Actstar = reader.GetValue<DateTime>("Actstar"),
                        Actend = reader.GetValue<DateTime>("Actend"),
                        Runstate = reader.GetValue<bool>("Runstate"),
                        Remark = reader.GetValue<string>("Remark"),
                        Usetitle = reader.GetValue<string>("Usetitle"),
                        Useremark = reader.GetValue<string>("Useremark"),
                        //Usestate = reader.GetValue<int>("Usestate"),
                        // Actnum = reader.GetValue<int>("Actnum"),
                        // Cardid = reader.GetValue<int>("Cardid"),
                        // Aid = reader.GetValue<int>("aid"),

                    });

                }
            }
            //totalcount = int.Parse(cmd.Parameters[7].Value.ToString());
            totalcount = inum;
            return list;
        }
        #endregion


        #region 查询促销活动信息
        internal Member_Activity AccountActInfo(int aid, int accountid, int comid, out int totalcount)
        {
            const string sqltxt = @"select a.id,a.Atitle,a.Remark,a.Useremark,a.Usetitle,b.id as aid,b.cardid,a.com_id,a.title,a.Acttype,a.discount,a.money,a.cashfull,a.cashback,a.faceobjects,a.repeatissue,a.returnact,a.useonce,a.actstar,a.actend,a.runstate,b.USEstate,b.Actnum from Member_Activity as a left join Member_Card_Activity as b on a.id=b.actid where a.com_id=@comid and a.id=@aid and b.CardID in (select id  from member_card where Cardcode in  (select IDcard from b2b_crm where id =@accountid ))";

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@accountid", accountid);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@aid", aid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    totalcount = 1;
                    return new Member_Activity
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Title = reader.GetValue<string>("title"),
                        Acttype = reader.GetValue<int>("Acttype"),
                        Discount = reader.GetValue<double>("Discount"),
                        Money = reader.GetValue<decimal>("Money"),
                        CashFull = reader.GetValue<decimal>("CashFull"),
                        Cashback = reader.GetValue<decimal>("Cashback"),
                        FaceObjects = reader.GetValue<int>("FaceObjects"),
                        RepeatIssue = reader.GetValue<int>("RepeatIssue"),
                        ReturnAct = reader.GetValue<int>("ReturnAct"),
                        UseOnce = reader.GetValue<bool>("UseOnce"),
                        Actstar = reader.GetValue<DateTime>("Actstar"),
                        Actend = reader.GetValue<DateTime>("Actend"),
                        Runstate = reader.GetValue<bool>("Runstate"),
                        Usestate = reader.GetValue<int>("Usestate"),
                        Actnum = reader.GetValue<int>("Actnum"),
                        Cardid = reader.GetValue<int>("Cardid"),
                        Aid = reader.GetValue<int>("aid"),
                        Atitle = reader.GetValue<string>("Atitle"),
                        Remark = reader.GetValue<string>("Remark"),
                        Useremark = reader.GetValue<string>("Useremark"),
                        Usetitle = reader.GetValue<string>("Usetitle"),
                    };
                }
                totalcount = 0;
                return null;
            }

        }
        #endregion

        #region 未绑定的查询促销活动
        internal Member_Activity UnAccountActInfo(int aid, int accountid, int comid, out int totalcount)
        {
            const string sqltxt = @"select a.id,a.Atitle,a.Remark,a.Useremark,a.Usetitle,a.com_id,a.title,a.Acttype,a.discount,a.money,a.cashfull,a.cashback,a.faceobjects,a.repeatissue,a.returnact,a.useonce,a.actstar,a.actend,a.runstate from Member_Activity as a where a.com_id=@comid and a.id=@aid and FaceObjects=1";

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@accountid", accountid);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@aid", aid);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    totalcount = 1;
                    return new Member_Activity
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Title = reader.GetValue<string>("title"),
                        Acttype = reader.GetValue<int>("Acttype"),
                        Discount = reader.GetValue<double>("Discount"),
                        Money = reader.GetValue<decimal>("Money"),
                        CashFull = reader.GetValue<decimal>("CashFull"),
                        Cashback = reader.GetValue<decimal>("Cashback"),
                        FaceObjects = reader.GetValue<int>("FaceObjects"),
                        RepeatIssue = reader.GetValue<int>("RepeatIssue"),
                        ReturnAct = reader.GetValue<int>("ReturnAct"),
                        UseOnce = reader.GetValue<bool>("UseOnce"),
                        Actstar = reader.GetValue<DateTime>("Actstar"),
                        Actend = reader.GetValue<DateTime>("Actend"),
                        Runstate = reader.GetValue<bool>("Runstate"),
                        Atitle = reader.GetValue<string>("Atitle"),
                        Remark = reader.GetValue<string>("Remark"),
                        Useremark = reader.GetValue<string>("Useremark"),
                        Usetitle = reader.GetValue<string>("Usetitle"),
                        //Usestate = reader.GetValue<int>("Usestate"),
                        //Actnum = reader.GetValue<int>("Actnum"),
                        //Cardid = reader.GetValue<int>("Cardid"),
                        //Aid = reader.GetValue<int>("aid"),
                    };
                }
                totalcount = 0;
                return null;
            }

        }
        #endregion

        #region 领取促销活动
        internal int AccountClaimActPageList(int aid, int cardid, int comid)
        {
            const string sqltxt = @"insert Member_Card_Activity (cardid,actid) values(@cardid,@aid);SELECT @@IDENTITY;";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@cardid", cardid);
            cmd.AddParam("@aid", aid);
            object o = cmd.ExecuteScalar();
            int newId = o == null ? 0 : int.Parse(o.ToString());
            return newId;
        }
        #endregion

        #region 自动领取促销活动cid = 3:web，4:微信
        internal string WebWeixinActIns(int uid, int cid, int comid)
        {
            const string sqltxt = @"insert Member_Card_Activity (cardid,actid) select @uid,id from Member_Activity where faceObjects=@cid";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@uid", uid);
            cmd.AddParam("@cid", cid);
            cmd.ExecuteNonQuery();
            return "OK";
        }
        #endregion


        internal Member_Activity GetMemberActivityById(int id)
        {
            string sql = @"SELECT   [id]
      ,[Com_id]
      ,[title]
      ,[Acttype]
      ,[Money]
      ,[Discount]
      ,[CashFull]
      ,[Cashback]
      ,[UseOnce]
      ,[Actstar]
      ,[actend]
      ,[FaceObjects]
      ,[RepeatIssue]
      ,[ReturnAct]
      ,[runstate]
      ,[atitle]
      ,[usetitle]
      ,[remark]
      ,[useremark]
      ,[whethercreateqrcode]
      ,createuserid
     ,createtime
  FROM [EtownDB].[dbo].[Member_Activity] where id=@id ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Member_Activity
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Title = reader.GetValue<string>("title"),
                        Acttype = reader.GetValue<int>("Acttype"),
                        Discount = reader.GetValue<double>("Discount"),
                        Money = reader.GetValue<decimal>("Money"),
                        CashFull = reader.GetValue<decimal>("CashFull"),
                        Cashback = reader.GetValue<decimal>("Cashback"),
                        FaceObjects = reader.GetValue<int>("FaceObjects"),
                        RepeatIssue = reader.GetValue<int>("RepeatIssue"),
                        ReturnAct = reader.GetValue<int>("ReturnAct"),
                        UseOnce = reader.GetValue<bool>("UseOnce"),
                        Actstar = reader.GetValue<DateTime>("Actstar"),
                        Actend = reader.GetValue<DateTime>("Actend"),
                        Runstate = reader.GetValue<bool>("Runstate"),
                        Atitle = reader.GetValue<string>("Atitle"),
                        Remark = reader.GetValue<string>("Remark"),
                        Useremark = reader.GetValue<string>("Useremark"),
                        Usetitle = reader.GetValue<string>("Usetitle"),
                        Whethercreateqrcode = reader.GetValue<bool>("whethercreateqrcode"),
                        CreateUserId = reader.GetValue<int>("createuserid"),
                        CreateTime = reader.GetValue<DateTime>("createtime")
                    };
                }
            }
            return null;
        }

        internal int HandleQrCodeCreateStatus(int activityid, string checkstatus)
        {
            string sql = "update Member_Activity set whethercreateqrcode=@checkstatus where id=@id";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@checkstatus", checkstatus);
            cmd.AddParam("@id", activityid);
            return cmd.ExecuteNonQuery();
        }

        internal List<Member_Activity> GetPromotionActList(int comid)
        {
            string sql = @"SELECT [id]
      ,[Com_id]
      ,[title]
      ,[Acttype]
      ,[Money]
      ,[Discount]
      ,[CashFull]
      ,[Cashback]
      ,[UseOnce]
      ,[Actstar]
      ,[actend]
      ,[FaceObjects]
      ,[RepeatIssue]
      ,[ReturnAct]
      ,[runstate]
      ,[atitle]
      ,[usetitle]
      ,[remark]
      ,[useremark]
      ,[whethercreateqrcode]
  FROM [EtownDB].[dbo].[Member_Activity] where com_id=@comid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);
            try
            {
                List<Member_Activity> list = new List<Member_Activity>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Member_Activity
                        {
                            Id = reader.GetValue<int>("id"),
                            Com_id = reader.GetValue<int>("com_id"),
                            Title = reader.GetValue<string>("title"),
                            Acttype = reader.GetValue<int>("Acttype"),
                            Discount = reader.GetValue<double>("Discount"),
                            Money = reader.GetValue<decimal>("Money"),
                            CashFull = reader.GetValue<decimal>("CashFull"),
                            Cashback = reader.GetValue<decimal>("Cashback"),
                            FaceObjects = reader.GetValue<int>("FaceObjects"),
                            RepeatIssue = reader.GetValue<int>("RepeatIssue"),
                            ReturnAct = reader.GetValue<int>("ReturnAct"),
                            UseOnce = reader.GetValue<bool>("UseOnce"),
                            Actstar = reader.GetValue<DateTime>("Actstar"),
                            Actend = reader.GetValue<DateTime>("Actend"),
                            Runstate = reader.GetValue<bool>("Runstate"),
                            Atitle = reader.GetValue<string>("Atitle"),
                            Remark = reader.GetValue<string>("Remark"),
                            Useremark = reader.GetValue<string>("Useremark"),
                            Usetitle = reader.GetValue<string>("Usetitle"),
                            Whethercreateqrcode = reader.GetValue<bool>("whethercreateqrcode")
                        });
                    }
                }
                return list;
            }
            catch
            {
                return null;
            }

        }

        internal List<Member_Activity> GetActivityList(int comid, string runstate, string whetherexpired)
        {
            string sql = @"SELECT [id]
      ,[Com_id]
      ,[title]
      ,[Acttype]
      ,[Money]
      ,[Discount]
      ,[CashFull]
      ,[Cashback]
      ,[UseOnce]
      ,[Actstar]
      ,[actend]
      ,[FaceObjects]
      ,[RepeatIssue]
      ,[ReturnAct]
      ,[runstate]
      ,[atitle]
      ,[usetitle]
      ,[remark]
      ,[useremark]
      ,[whethercreateqrcode]
  FROM [EtownDB].[dbo].[Member_Activity] where com_id=" + comid + " and runstate in (" + runstate + ")";
            if (whetherexpired == "1")//过期
            {
                sql += " and actend<getdate()";
            }
            else if (whetherexpired == "0")//不过期
            {
                sql += " and actend>=getdate()";
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            try
            {
                List<Member_Activity> list = new List<Member_Activity>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Member_Activity
                        {
                            Id = reader.GetValue<int>("id"),
                            Com_id = reader.GetValue<int>("com_id"),
                            Title = reader.GetValue<string>("title"),
                            Acttype = reader.GetValue<int>("Acttype"),
                            Discount = reader.GetValue<double>("Discount"),
                            Money = reader.GetValue<decimal>("Money"),
                            CashFull = reader.GetValue<decimal>("CashFull"),
                            Cashback = reader.GetValue<decimal>("Cashback"),
                            FaceObjects = reader.GetValue<int>("FaceObjects"),
                            RepeatIssue = reader.GetValue<int>("RepeatIssue"),
                            ReturnAct = reader.GetValue<int>("ReturnAct"),
                            UseOnce = reader.GetValue<bool>("UseOnce"),
                            Actstar = reader.GetValue<DateTime>("Actstar"),
                            Actend = reader.GetValue<DateTime>("Actend"),
                            Runstate = reader.GetValue<bool>("Runstate"),
                            Atitle = reader.GetValue<string>("Atitle"),
                            Remark = reader.GetValue<string>("Remark"),
                            Useremark = reader.GetValue<string>("Useremark"),
                            Usetitle = reader.GetValue<string>("Usetitle"),
                            Whethercreateqrcode = reader.GetValue<bool>("whethercreateqrcode")
                        });
                    }
                }
                return list;
            }
            catch
            {
                return null;
            }
        }

        internal bool WhetherSameunit(int createuserid, int? operchannelcompanyid)
        {
            string sql = "select count(1) from b2b_company_manageuser where id=" + createuserid + " and channelcompanyid  =" + operchannelcompanyid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            int d = o == null ? 0 : int.Parse(o.ToString());
            if (d > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
