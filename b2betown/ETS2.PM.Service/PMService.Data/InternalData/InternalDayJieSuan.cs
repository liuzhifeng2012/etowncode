using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;
using ETS.Framework;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Modle;
using System.Data.SqlClient;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalDayJieSuan
    {
        public SqlHelper sqlHelper;
        public InternalDayJieSuan(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal DataSet DayJSList(string comid)
        {
            //判断验码日志表中是否含有公司未验证的记录（有，向当日结算表中录入新纪录，同时更改验码日志表中jsid；没有，返回null）
            var verifylogcount = DayJieSuanData.GetVerifyLogCountByComId(comid);
            if (verifylogcount > 0)
            {
                B2b_dayjiesuan dayjs = new B2b_dayjiesuan
                {
                    Id = 0,
                    Jstime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    Jsstartdate = new B2bEticketLogData().GetFrontNotJS() == null ? DateTime.Parse("1900-01-01") : new B2bEticketLogData().GetFrontNotJS().Actiondate,//得到未开始结算的记录
                    Jsenddate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")),
                    Com_id = int.Parse(comid),
                    PosId = 0
                };
                var JSid = new DayJieSuanData().InsertOrUpdate(dayjs);


                //更改此次结算的电子票发码日志表中的关联jsid（当前公司 验码 状态成功 js=0即未结算过的）
                new B2bEticketLogData().ModifyJsid(JSid, int.Parse(comid));

                const string sqltxt = @"SELECT   
      [eticket_id] 
      ,COUNT(1) as TotalVerifyNum
      ,sum(use_pnum) as TotalConsumedNum
  FROM [EtownDB].[dbo].[b2b_etcket_log] where jsid=@jsid group by eticket_id";
                var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
                cmd.AddParam("@jsid", JSid);

                using (var reader = cmd.ExecuteReader())
                {

                    DataTable dt = new DataTable("dayjs");
                    DataColumn dc1 = dt.Columns.Add("proname");
                    DataColumn dc2 = dt.Columns.Add("TotalConsumedNum");
                    DataColumn dc3 = dt.Columns.Add("e_face_price");
                    DataColumn dc4 = dt.Columns.Add("DirectsaleConsumedNum");
                    DataColumn dc5 = dt.Columns.Add("DistributesaleConsumedNum");
                    DataColumn dc6 = dt.Columns.Add("jsstartdate");
                    DataColumn dc7 = dt.Columns.Add("jsenddate");
                    DataColumn dc8 = dt.Columns.Add("pro_id");
                    DataColumn dc9 = dt.Columns.Add("jsid");
                    DataRow drr;

                    while (reader.Read())
                    {
                        drr = dt.NewRow();
                        drr["proname"] = new B2bEticketData().GetEticketByID(reader.GetValue<int>("eticket_id").ToString()).E_proname;
                        drr["TotalConsumedNum"] = reader.GetValue<int>("TotalConsumedNum");
                        drr["e_face_price"] = new B2bEticketData().GetEticketByID(reader.GetValue<int>("eticket_id").ToString()).E_face_price.ToString("f2");
                        drr["DirectsaleConsumedNum"] = reader.GetValue<int>("TotalConsumedNum");
                        drr["DistributesaleConsumedNum"] = 0;
                        drr["jsstartdate"] = dayjs.Jsstartdate.ToString("yyyy-MM-dd");
                        drr["jsenddate"] = dayjs.Jsenddate.ToString("yyyy-MM-dd");
                        drr["jsid"] = JSid;
                        drr["pro_id"] = new B2bEticketData().GetEticketByID(reader.GetValue<int>("eticket_id").ToString()).Pro_id;
                        dt.Rows.Add(drr);
                    }

                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);
                    return ds;

                }
            }
            else
            {
                return null;
            }
        }
//        #region 获得当天的结算信息对象
//        internal B2b_dayjiesuan GetDateDayJS(string comid)
//        {
//            const string sqltxt = @"SELECT   [id]
//      ,[jsstartdate]
//      ,[jsenddate]
//      ,[jstime]
//,[com_id]
//  FROM [EtownDB].[dbo].[b2b_dayjiesuan] where jsenddate=@jsenddate and com_id=@comid";
//            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
//            cmd.AddParam("@jsenddate", DateTime.Now.ToString("yyyy-MM-dd"));
//            cmd.AddParam("@comid", comid);

//            using (var reader = cmd.ExecuteReader())
//            {
//                if (reader.Read())
//                {
//                    return new B2b_dayjiesuan
//                    {
//                        Id = reader.GetValue<int>("id"),
//                        Jsstartdate = reader.GetValue<DateTime>("jsstartdate"),
//                        Jsenddate = reader.GetValue<DateTime>("jsenddate"),
//                        Jstime = reader.GetValue<DateTime>("jstime"),
//                        Com_id = reader.GetValue<int>("com_id")
//                    };
//                }
//                return null;
//            }
//        }
//        #endregion
        #region 编辑当日结算信息
        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateDayJieSuan";
        internal int InsertOrUpdate(B2b_dayjiesuan model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);

            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Jstime", model.Jstime);
            cmd.AddParam("@Jsstartdate", model.Jsstartdate);
            cmd.AddParam("@Jsenddate", model.Jsenddate);
            cmd.AddParam("@Com_id", model.Com_id);
            cmd.AddParam("@PosId", model.PosId);



            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion

        internal B2b_dayjiesuan GetDayJSByID(int JSid)
        {
            const string sqltxt = @"SELECT   [id]
      ,[jsstartdate]
      ,[jsenddate]
      ,[jstime]
,[com_id]
  FROM [EtownDB].[dbo].[b2b_dayjiesuan] where id=@id";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@id", JSid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_dayjiesuan
                    {
                        Id = reader.GetValue<int>("id"),
                        Jsstartdate = reader.GetValue<DateTime>("jsstartdate"),
                        Jsenddate = reader.GetValue<DateTime>("jsenddate"),
                        Jstime = reader.GetValue<DateTime>("jstime"),
                        Com_id = reader.GetValue<int>("com_id")
                    };
                }
                return null;
            }
        }
        #region 得到特定商家每日结算统计
        internal DataSet DayJSResult(string comid,string jsid)
        {
            ////得到此商家当日结算表中信息
            B2b_dayjiesuan dayjs =new DayJieSuanData().GetDayJSByID(int.Parse(jsid));

            const string sqltxt = @"select   
      sum(use_pnum) as totalnum
      
  FROM [EtownDB].[dbo].[b2b_etcket_log] where jsid=@jsid";

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);

            cmd.AddParam("@jsid", jsid);

            using (var reader = cmd.ExecuteReader())
            {

                DataTable dt = new DataTable("dayjsresult");
                DataColumn dc1 = dt.Columns.Add("TotalConsumedNum");
                DataColumn dc2 = dt.Columns.Add("jsstartdate");
                DataColumn dc3 = dt.Columns.Add("jsenddate");
                DataColumn dc4 = dt.Columns.Add("jstime");
                DataColumn dc5 = dt.Columns.Add("Accounts");

                DataRow drr;

                while (reader.Read())
                {

                    drr = dt.NewRow();
                    drr["jsstartdate"] = dayjs.Jsstartdate.ToString("yyyy-MM-dd");
                    drr["TotalConsumedNum"] = reader.GetValue<int>("totalnum");
                    drr["jsenddate"] = dayjs.Jsenddate.ToString("yyyy-MM-dd");
                    drr["jstime"] = dayjs.Jstime.ToString("yyyy-MM-dd HH:mm:ss");
                    drr["Accounts"] = ((B2b_company_manageuser)UserHelper.CurrentUser()).Accounts;


                    dt.Rows.Add(drr);

                }

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                return ds;

            }
        }
        #endregion

        internal DataSet DayJSListByPosId(string pos_id)
        {
            //判断验码日志表中是否含有pos未验证的记录（有，向当日结算表中录入新纪录，同时更改验码日志表中jsid；没有，返回null）
            var verifylogcount = DayJieSuanData.GetVerifyLogCount(pos_id);
            if (verifylogcount > 0)
            {
                B2b_dayjiesuan newdayjs = new B2b_dayjiesuan
                {
                    Id = 0,
                    Jstime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    Jsstartdate = new B2bEticketLogData().GetFrontNotJS() == null ? DateTime.Parse("1900-01-01") : new B2bEticketLogData().GetFrontNotJS().Actiondate,//得到未开始结算的记录
                    Jsenddate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")),
                    Com_id = new B2bCompanyPosData().GetPosByPosId(pos_id).Id,
                    PosId = int.Parse(pos_id)
                };
                var JSid = new DayJieSuanData().InsertOrUpdate(newdayjs);

                //更改此次结算的电子票发码日志表中的关联jsid（当前pos 验码 状态成功 js=0即未结算过的）
                new B2bEticketLogData().ModifyJsidByPosId(JSid, pos_id);

                const string sqltxt = @"SELECT   
      [eticket_id] 
      ,COUNT(1) as TotalVerifyNum
      ,sum(use_pnum) as TotalConsumedNum
  FROM [EtownDB].[dbo].[b2b_etcket_log] where jsid=@jsid group by eticket_id";
                var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
                cmd.AddParam("@jsid", JSid);

                using (var reader = cmd.ExecuteReader())
                {

                    DataTable dt = new DataTable("dayjs");
                    DataColumn dc1 = dt.Columns.Add("proname");
                    DataColumn dc2 = dt.Columns.Add("TotalConsumedNum");
                    DataColumn dc3 = dt.Columns.Add("e_face_price");
                    DataColumn dc4 = dt.Columns.Add("TotalVerifyNum");//验票笔数
                    DataRow drr;

                    while (reader.Read())
                    {
                        drr = dt.NewRow();
                        drr["proname"] = new B2bEticketData().GetEticketByID(reader.GetValue<int>("eticket_id").ToString()).E_proname;
                        drr["TotalConsumedNum"] = reader.GetValue<int>("TotalConsumedNum");
                        drr["TotalVerifyNum"] = reader.GetValue<int>("TotalVerifyNum");
                        drr["e_face_price"] = new B2bEticketData().GetEticketByID(reader.GetValue<int>("eticket_id").ToString()).E_face_price.ToString("f2");

                        dt.Rows.Add(drr);
                    }

                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);
                    return ds;

                }
            }
            else
            {
                return null;
            }

        }

        internal B2b_dayjiesuan GetDateDayJSByPosId(string pos_id)
        {
            const string sqltxt = @"SELECT [id]
      ,[jsstartdate]
      ,[jsenddate]
      ,[jstime]
      ,[com_id]
      ,[posid]
  FROM [EtownDB].[dbo].[b2b_dayjiesuan] where posid=@posid and  jsenddate=@jsenddate";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@posid", pos_id);
            cmd.AddParam("@jsenddate", DateTime.Now.ToString("yyyy-MM-dd"));

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_dayjiesuan
                    {
                        Id = reader.GetValue<int>("id"),
                        Jsstartdate = reader.GetValue<DateTime>("jsstartdate"),
                        Jsenddate = reader.GetValue<DateTime>("jsenddate"),
                        Jstime = reader.GetValue<DateTime>("jstime"),
                        Com_id = reader.GetValue<int>("com_id"),
                        PosId = reader.GetValue<int>("posid")
                    };
                }
                return null;
            }
        }

        internal int GetVerifyLogCount(string pos_id)
        {
            string sqltxt = "select count(1) from b2b_etcket_log where action=1 and a_state=1 and jsid=0 and posid=" + pos_id;
            return (int)MSSqlHelper.ExecuteScalar(sqltxt);

        }

        internal int GetVerifyLogCountByComId(string comid)
        {
            string sqltxt = "select count(1) from b2b_etcket_log where action=1 and a_state=1 and jsid=0 and com_id=" + comid;
            return (int)MSSqlHelper.ExecuteScalar(sqltxt);

        }

//        internal B2b_dayjiesuan GetDayJsById(string jsid)
//        {
//            const string sqltxt = @"SELECT [id]
//      ,[jsstartdate]
//      ,[jsenddate]
//      ,[jstime]
//      ,[com_id]
//      ,[posid]
//  FROM [EtownDB].[dbo].[b2b_dayjiesuan] where id=@jsid";
//            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
//            cmd.AddParam("@jsid", jsid);
           

//            using (var reader = cmd.ExecuteReader())
//            {
//                if (reader.Read())
//                {
//                    return new B2b_dayjiesuan
//                    {
//                        Id = reader.GetValue<int>("id"),
//                        Jsstartdate = reader.GetValue<DateTime>("jsstartdate"),
//                        Jsenddate = reader.GetValue<DateTime>("jsenddate"),
//                        Jstime = reader.GetValue<DateTime>("jstime"),
//                        Com_id = reader.GetValue<int>("com_id"),
//                        PosId = reader.GetValue<int>("posid")
//                    };
//                }
//                return null;
//            }
//        }
    }
}
