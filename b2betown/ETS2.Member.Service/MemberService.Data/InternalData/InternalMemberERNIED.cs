using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.Member.Service.MemberService.Model;
using ETS.Data.SqlHelper;

namespace ETS2.Member.Service.MemberService.Data.InternalData
{
    public class InternalMemberERNIED
    {         
        private SqlHelper sqlHelper;
        public InternalMemberERNIED(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal Member_ERNIE ERNIEGetActById(int actid)
        {
            const string sqltxt = @"SELECT   *
  FROM [Member_Act_ERNIE_Manage] where [Id]=@actid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@actid", actid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {

                    return new Member_ERNIE
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Title = reader.GetValue<string>("title"),
                        ERNIE_type = reader.GetValue<int>("ERNIE_type"),
                        ERNIE_RateNum = reader.GetValue<int>("ERNIE_RateNum"),
                        ERNIE_Limit = reader.GetValue<int>("ERNIE_Limit"),
                        Limit_Num = reader.GetValue<int>("Limit_Num"),
                        Runstate = reader.GetValue<int>("Runstate"),
                        ERNIE_star = reader.GetValue<DateTime>("ERNIE_star"),
                        ERNIE_end = reader.GetValue<DateTime>("ERNIE_end"),
                        Remark = reader.GetValue<string>("Remark"),
                        Online = reader.GetValue<int>("Online"),
                    };
                }
                return null;
            }
        }

        //得到最新摇奖活动
        internal int ERNIETOPgetid(int comid)
        {
            const string sqltxt = @"SELECT  top 1 *
  FROM [Member_Act_ERNIE_Manage] where [com_Id]=@comid and runstate=1 and online=1 order by id desc";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("Id");
                }
                return 0;
            }
        }

        #region 促销活动列表
        internal List<Member_ERNIE> ERNIEActPageList(int comid, int pageindex, int pagesize, out int totalcount,string runstate="0,1")
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "Member_Act_ERNIE_Manage";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";
            var condition = "com_id=" + comid + " and Runstate in ("+runstate+")";
            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<Member_ERNIE> list = new List<Member_ERNIE>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_ERNIE
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Title = reader.GetValue<string>("title"),
                        ERNIE_type = reader.GetValue<int>("ERNIE_type"),
                        ERNIE_RateNum = reader.GetValue<int>("ERNIE_RateNum"),
                        ERNIE_Limit = reader.GetValue<int>("ERNIE_Limit"),
                        Limit_Num = reader.GetValue<int>("Limit_Num"),
                        Runstate = reader.GetValue<int>("Runstate"),
                        ERNIE_star = reader.GetValue<DateTime>("ERNIE_star"),
                        ERNIE_end = reader.GetValue<DateTime>("ERNIE_end"),
                        Online = reader.GetValue<int>("Online"),
                        Remark = reader.GetValue<string>("Remark"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());
            
            return list;
        }
        #endregion

        #region 添加或者编辑促销活动信息
        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateMemberERNIEActInfo";
        public int InsertOrUpdate(Member_ERNIE model)
        {

            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);
            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Com_id", model.Com_id);
            cmd.AddParam("@title", model.Title);
            cmd.AddParam("@ERNIE_type", model.ERNIE_type);
            cmd.AddParam("@ERNIE_star", model.ERNIE_star);
            cmd.AddParam("@ERNIE_end", model.ERNIE_end);
            cmd.AddParam("@ERNIE_RateNum", model.ERNIE_RateNum);
            cmd.AddParam("@ERNIE_Limit", model.ERNIE_Limit);
            cmd.AddParam("@Limit_Num", model.Limit_Num);
            cmd.AddParam("@Runstate", model.Runstate);
            cmd.AddParam("@Remark", model.Remark);
  
            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion


        
        internal Member_ERNIE_Award ERNIEGetAwardById(int actid)
        {
            const string sqltxt = @"SELECT   *
  FROM [Member_Act_ERNIE_Award] where [id]=@actid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@actid", actid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Member_ERNIE_Award
                    {
                        Id = reader.GetValue<int>("Id"),
                        ERNIE_id = reader.GetValue<int>("ERNIE_id"),
                        Award_class = reader.GetValue<int>("Award_class"),
                        Award_num = reader.GetValue<int>("Award_num"),
                        Award_type = reader.GetValue<int>("Award_type"),
                        Award_Get_Num = reader.GetValue<int>("Award_Get_Num"),
                        Award_title = reader.GetValue<string>("Award_title"),
                    };
                }
                return null;
            }
        }

        internal Member_ERNIE_Award ERNIEAwardget(int actid,int topclass)
        {
            const string sqltxt = @"SELECT   *
  FROM [Member_Act_ERNIE_Award] where [ERNIE_id]=@actid and Award_class=@topclass ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@actid", actid);
            cmd.AddParam("@topclass", topclass);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Member_ERNIE_Award
                    {
                        Id = reader.GetValue<int>("Id"),
                        ERNIE_id = reader.GetValue<int>("ERNIE_id"),
                        Award_class = reader.GetValue<int>("Award_class"),
                        Award_num = reader.GetValue<int>("Award_num"),
                        Award_type = reader.GetValue<int>("Award_type"),
                        Award_Get_Num = reader.GetValue<int>("Award_Get_Num"),
                        Award_title = reader.GetValue<string>("Award_title"),
                    };
                }
                return null;
            }
        }
        #region 促销活动列表
        internal List<Member_ERNIE_Award> ERNIEAwardPageList(int actid, int pageindex, int pagesize, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "Member_Act_ERNIE_Award";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";
            var condition = "ERNIE_id=" + actid;
            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<Member_ERNIE_Award> list = new List<Member_ERNIE_Award>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_ERNIE_Award
                    {
                        Id = reader.GetValue<int>("Id"),
                        ERNIE_id = reader.GetValue<int>("ERNIE_id"),
                        Award_title = reader.GetValue<string>("Award_title"),
                        Award_class = reader.GetValue<int>("Award_class"),
                        Award_num = reader.GetValue<int>("Award_num"),
                        Award_type = reader.GetValue<int>("Award_type"),
                        Award_Get_Num = reader.GetValue<int>("Award_Get_Num"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }
        #endregion

        #region 添加或者编辑促销活动信息
        private static readonly string AwardSQLInsertOrUpdate = "usp_InsertOrUpdateMemberERNIEAwardInfo";
        public int AwardInsertOrUpdate(Member_ERNIE_Award model)
        {

            var cmd = sqlHelper.PrepareStoredSqlCommand(AwardSQLInsertOrUpdate);
            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@ERNIE_id", model.ERNIE_id);
            cmd.AddParam("@Award_class", model.Award_class);
            cmd.AddParam("@Award_title", model.Award_title);
            cmd.AddParam("@Award_num", model.Award_num);
            cmd.AddParam("@Award_type", model.Award_type);
            cmd.AddParam("@Award_Get_Num", model.Award_Get_Num);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion


        #region 添加或者编辑促销活动信息
        public int ERNIEDelAwardInfo(int actid)
        {
            const string sqltxt = @"delete [Member_Act_ERNIE_Award] where [ERNIE_id]=@actid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@actid", actid);
            return cmd.ExecuteNonQuery();
         }
        #endregion


        #region 插入抽奖
        public int InsertChoujiang(ERNIE_Record Recordinfo){

            const string sqltxt = @"insert [Member_Act_ERNIE_Record] (ERNIE_id,ERNIE_uid,ERNIE_openid,ERNIE_code,ERNIE_time,ip)values(@ERNIE_id,@ERNIE_uid,@ERNIE_openid,@ERNIE_code,@ERNIE_time,@Ip) ;select @@identity;";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@ERNIE_id", Recordinfo.ERNIE_id);
            cmd.AddParam("@ERNIE_uid", Recordinfo.ERNIE_uid);
            cmd.AddParam("@ERNIE_openid", Recordinfo.ERNIE_openid);
            cmd.AddParam("@ERNIE_code", Recordinfo.ERNIE_code);
            cmd.AddParam("@ERNIE_time", Recordinfo.ERNIE_time);
            cmd.AddParam("@Ip", Recordinfo.Ip);
            object o = cmd.ExecuteScalar();
            int newId = o == null ? 0 : int.Parse(o.ToString());
            return newId;
        }
        #endregion

        #region 插入中奖号码
        public int InsertAward(ERNIE_Awardcode Awardinfo)
        {
            const string sqltxt = @"insert [Member_Act_ERNIE_Awardcode] (Award_id,ERNIE_id,Award_code)values(@Award_id,@ERNIE_id,@Award_code) ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@Award_id", Awardinfo.Award_id);
            cmd.AddParam("@ERNIE_id", Awardinfo.ERNIE_id);
            cmd.AddParam("@Award_code", Awardinfo.Award_code);
            return cmd.ExecuteNonQuery();
        }
        #endregion


        #region 查询抽奖，指定活动，指定用户，指定OPENid ，返回抽奖次数,判断是否已参加过抽奖
        public int SearchChoujiang(ERNIE_Record Recordinfo, int ERNIE_Limit)
        {

            string sqltxt = @"select count(id) as num from [Member_Act_ERNIE_Record] where (ERNIE_uid= " + Recordinfo.ERNIE_uid + "  or ERNIE_openid='" + Recordinfo.ERNIE_openid + "') and ERNIE_id=" + Recordinfo.ERNIE_id + "";
            if (ERNIE_Limit == 1) {
                sqltxt = sqltxt + " and ERNIE_time>='"+DateTime.Today+"'" ;
            }
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                     int num=reader.GetValue<int>("num");

                     return num;
                }
            }
            return 0;
        }
        #endregion

        #region 抽奖上线
        public int ERNIEeditActOnline(int actid)
        {
            const string sqltxt = @"update [Member_Act_ERNIE_Manage] set Online=1  where id=@actid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@actid", actid);
            return cmd.ExecuteNonQuery();

        }
        #endregion

        #region 确认已发奖
        public int ERNIERecordedit(int actid)
        {
            const string sqltxt = @"update [Member_Act_ERNIE_Record] set process_state=1 where id=@actid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@actid", actid);
            return cmd.ExecuteNonQuery();

        }
        #endregion
        #region 查询抽奖，指定活动，指定用户，指定OPENid ，返回抽奖次数,判断是否已参加过抽奖
        public int ChoujiangSearchAwardcode(int rid, int actid)
        {
            string sqltxt = @"select top 1 * from Member_Act_ERNIE_Awardcode where ERNIE_id =@actid and Award_code in (select ERNIE_code from Member_Act_ERNIE_Record where id=@rid and ERNIE_id =@actid ) and usestate=0 order by id";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@rid", rid);
            cmd.AddParam("@actid", actid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("id");
                }
            }
            return 0;
        }
        #endregion


        #region 插入抽奖
        public int ZhongjiangAwardcode(int Recordid, int Awardcodeid, int uid)
        {
            int ernieclass = 0;
            int id = 0;

            string sqltxt = @"update [Member_Act_ERNIE_Awardcode] set uid=@uid  where id=@Awardcodeid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@uid", uid);
            cmd.AddParam("@Awardcodeid", Awardcodeid);
            cmd.ExecuteNonQuery();

            sqltxt = @"select top 1 * from Member_Act_ERNIE_Award where id in (select Award_id from Member_Act_ERNIE_Awardcode where id=@Awardcodeid )";
            cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@Awardcodeid", Awardcodeid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    ernieclass=reader.GetValue<int>("award_class");
                    id = reader.GetValue<int>("id");
                }
            }

            sqltxt = @"update [Member_Act_ERNIE_Record] set Winning_state=1,Awardid=@id  where id=@Recordid ";
            cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@Recordid", Recordid);
            cmd.AddParam("@id",id);
            cmd.ExecuteNonQuery();

            return ernieclass;
        }
        #endregion



        #region 中奖信息
        public int ERNIEZhongjiang(ERNIE_Record Recordinfo)
        {

            const string sqltxt = @"update [Member_Act_ERNIE_Record] set Name=@Name,Phone=@Phone where ERNIE_id = @ERNIE_id and id=@Id and ERNIE_openid= @ERNIE_openid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@Id", Recordinfo.Id);
            cmd.AddParam("@ERNIE_openid", Recordinfo.ERNIE_openid);
            cmd.AddParam("@ERNIE_id", Recordinfo.ERNIE_id);
            cmd.AddParam("@Name", Recordinfo.Name);
            cmd.AddParam("@Phone", Recordinfo.Phone);
            return cmd.ExecuteNonQuery();
        }
        #endregion



        #region 中奖状态处理
        public int ERNIEZhongjiangChuli(int id)
        {
            const string sqltxt = @"update [Member_Act_ERNIE_Record] set process_state=1 where id = @Id";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@Id", id);
            return cmd.ExecuteNonQuery();
        }
        #endregion

        //获得中奖信息
        internal ERNIE_Record ERNIERecordInfo(int rid)
        {
            const string sqltxt = @"SELECT   *
  FROM [Member_Act_ERNIE_Record] where [id]=@rid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@rid", rid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new ERNIE_Record
                    {
                        Id = reader.GetValue<int>("Id"),
                        ERNIE_id = reader.GetValue<int>("ERNIE_id"),
                        Name = reader.GetValue<string>("Name"),
                        Phone = reader.GetValue<string>("Phone"),
                        ERNIE_code = reader.GetValue<decimal>("ERNIE_code"),
                        ERNIE_openid = reader.GetValue<string>("ERNIE_openid"),
                        ERNIE_uid = reader.GetValue<int>("ERNIE_uid"),
                        Address = reader.GetValue<string>("Address"),
                        Winning_state = reader.GetValue<int>("Winning_state"),
                        Ip = reader.GetValue<string>("Ip"),
                        ERNIE_time = reader.GetValue<DateTime>("ERNIE_time"),
                        Awardid = reader.GetValue<int>("Awardid"),
                        Process_state = reader.GetValue<int>("Process_state"),
                    };
                }
                return null;
            }
        }

        #region 奖品列表
        internal List<ERNIE_Record> ERNIERecordpagelist(string comid, int pageindex, int pagesize, int actid, int etype, string key, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "Member_Act_ERNIE_Record";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";
            var condition = " Winning_state=1 and  ERNIE_id in (select id from Member_Act_ERNIE_Manage where com_id=" + comid + ")";

            if (actid != 0)
            {
                condition = condition + " and ERNIE_id in (select id from Member_Act_ERNIE_Manage where com_id=" + comid + " and id=" + actid + ")";
            }
            if (key != "")
            {
                condition = condition + " and (name like '%" + key + "%' or phone like '%" + key + "%')";
            }
            if (etype != 9)
            {
                condition = condition + " and Awardid in (select id from Member_Act_ERNIE_Award where award_type=" + etype + ")";
            }


            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<ERNIE_Record> list = new List<ERNIE_Record>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new ERNIE_Record
                    {
                        Id = reader.GetValue<int>("id"),
                        Address = reader.GetValue<string>("Address"),
                        ERNIE_code = reader.GetValue<decimal>("ERNIE_code"),
                        Awardid = reader.GetValue<int>("Awardid"),
                        ERNIE_id = reader.GetValue<int>("ERNIE_id"),
                        ERNIE_openid = reader.GetValue<string>("ERNIE_openid"),
                        Winning_state = reader.GetValue<int>("Winning_state"),
                        ERNIE_uid = reader.GetValue<int>("ERNIE_uid"),
                        Ip = reader.GetValue<string>("Ip"),
                        Name = reader.GetValue<string>("Name"),
                        Phone = reader.GetValue<string>("Phone"),
                        Process_state = reader.GetValue<int>("Process_state"),
                        ERNIE_time = reader.GetValue<DateTime>("ERNIE_time"),

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }
        #endregion



        #region 中奖信息
        public List<ERNIE_Record> ERNIEHuojiangmingdan(int actid)
        {

            const string sqltxt = @"SELECT   *
  FROM [Member_Act_ERNIE_Record] where [ERNIE_id]=@actid and winning_state=1 and phone !='' ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@actid", actid);
              List<ERNIE_Record> list = new List<ERNIE_Record>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                   list.Add(new ERNIE_Record
                    {
                        Id = reader.GetValue<int>("Id"),
                        ERNIE_id = reader.GetValue<int>("ERNIE_id"),
                        Name = reader.GetValue<string>("Name"),
                        Phone = reader.GetValue<string>("Phone"),
                        ERNIE_code = reader.GetValue<decimal>("ERNIE_code"),
                        ERNIE_openid = reader.GetValue<string>("ERNIE_openid"),
                        ERNIE_uid = reader.GetValue<int>("ERNIE_uid"),
                        Address = reader.GetValue<string>("Address"),
                        Winning_state = reader.GetValue<int>("Winning_state"),
                        Ip = reader.GetValue<string>("Ip"),
                        ERNIE_time = reader.GetValue<DateTime>("ERNIE_time"),
                        Awardid = reader.GetValue<int>("Awardid"),
                        Process_state = reader.GetValue<int>("Process_state"),
                    });

                }
                return list;
            }
        }
        #endregion

        //通过中奖AwardID查询奖项
        internal int ERNIEAwardgetID(int Awardid)
        {
            const string sqltxt = @"SELECT   *
  FROM [Member_Act_ERNIE_Award] where [id]=@Awardid";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@Awardid", Awardid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("Award_class");
                }
                return 0;
            }
        }

    }
}
