using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;
using System.Data;
using System.Data.SqlClient;
using ETS.Framework;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS2.Common.Business;
using System.Collections;


namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalB2bComPro
    {
        private SqlHelper sqlHelper;
        public InternalB2bComPro(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        #region 添加或者编辑产品信息

        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateB2bComPro";

        public int InsertOrUpdate(B2b_com_pro model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);

            cmd.AddParam("@id", model.Id);
            cmd.AddParam("@merchant_code", model.merchant_code);
            cmd.AddParam("@com_id", model.Com_id);
            cmd.AddParam("@pro_name", model.Pro_name);
            cmd.AddParam("@pro_state", model.Pro_state);
            cmd.AddParam("@server_type", model.Server_type);
            cmd.AddParam("@pro_type", model.Pro_type);
            cmd.AddParam("@source_type", model.Source_type);
            cmd.AddParam("@pro_Remark", model.Pro_Remark);
            cmd.AddParam("@pro_start", model.Pro_start);
            cmd.AddParam("@pro_end", model.Pro_end);
            cmd.AddParam("@face_price", model.Face_price);
            cmd.AddParam("@advise_price", model.Advise_price);
            cmd.AddParam("@Agent1_price", model.Agent1_price);
            cmd.AddParam("@Agent2_price", model.Agent2_price);
            cmd.AddParam("@Agent3_price", model.Agent3_price);
            cmd.AddParam("@agentsettle_price", model.Agentsettle_price);
            cmd.AddParam("@ThatDay_can", model.ThatDay_can);
            cmd.AddParam("@Thatday_can_day", model.Thatday_can_day);
            cmd.AddParam("@service_Contain", model.Service_Contain);
            cmd.AddParam("@service_NotContain", model.Service_NotContain);
            cmd.AddParam("@Precautions", model.Precautions);
            cmd.AddParam("@tuan_pro", model.Tuan_pro);
            cmd.AddParam("@zhixiao", model.Zhixiao);
            cmd.AddParam("@agentsale", model.Agentsale);
            cmd.AddParam("@createuserid", model.Createuserid);
            cmd.AddParam("@imgurl", model.Imgurl);
            cmd.AddParam("@pro_Number", model.Pro_number);
            cmd.AddParam("@pro_Explain", model.Pro_explain);
            cmd.AddParam("@pro_Integral", model.Pro_Integral);
            cmd.AddParam("@Sms", model.Sms);
            cmd.AddParam("@Tuipiao", model.Tuipiao);
            cmd.AddParam("@Tuipiao_guoqi", model.Tuipiao_guoqi);
            cmd.AddParam("@Tuipiao_endday", model.Tuipiao_endday);

            cmd.AddParam("@serviceid", model.Serviceid);
            cmd.AddParam("@service_proid", model.Service_proid);
            cmd.AddParam("@realnametype", model.Realnametype);

            cmd.AddParam("@projectid", model.Projectid);
            cmd.AddParam("@trvalproductid", model.Travelproductid);
            cmd.AddParam("@traveltype", model.Traveltype);
            cmd.AddParam("@trvalstarting", model.Travelstarting);

            cmd.AddParam("@ispanicbuy", model.Ispanicbuy);
            cmd.AddParam("@panic_begintime", model.Panic_begintime);
            cmd.AddParam("@panicbuy_endtime", model.Panicbuy_endtime);
            cmd.AddParam("@limitbuytotalnum", model.Limitbuytotalnum);

            cmd.AddParam("@Linepro_booktype", model.Linepro_booktype);

            cmd.AddParam("@ProValidateMethod", model.ProValidateMethod);
            cmd.AddParam("@appointdata", model.Appointdata);
            cmd.AddParam("@iscanuseonsameday", model.Iscanuseonsameday);
            cmd.AddParam("@viewmethod", model.Viewmethod);

            cmd.AddParam("@childreduce", model.Childreduce);

            cmd.AddParam("@pickuppoint", model.pickuppoint);
            cmd.AddParam("@dropoffpoint", model.dropoffpoint);
            cmd.AddParam("@pro_note", model.pro_note);

            cmd.AddParam("@QuitTicketMechanism", model.QuitTicketMechanism);

            cmd.AddParam("@isneedbespeak", model.isneedbespeak);
            cmd.AddParam("@daybespeaknum", model.daybespeaknum);
            cmd.AddParam("@bespeaksucmsg", model.bespeaksucmsg);
            cmd.AddParam("@bespeakfailmsg", model.bespeakfailmsg);
            cmd.AddParam("@customservicephone", model.customservicephone);
            cmd.AddParam("@isblackoutdate", model.isblackoutdate);
            cmd.AddParam("@etickettype", model.etickettype);

            cmd.AddParam("@ishasdeliveryfee", model.ishasdeliveryfee);
            cmd.AddParam("@deliverytmp", model.deliverytmp);
            cmd.AddParam("@pro_weight", model.pro_weight);

            cmd.AddParam("@bookpro_bindname", model.bookpro_bindname);
            cmd.AddParam("@bookpro_bindphone", model.bookpro_bindphone);
            cmd.AddParam("@bookpro_bindcompany", model.bookpro_bindcompany);
            cmd.AddParam("@bookpro_ispay", model.bookpro_ispay);
            cmd.AddParam("@isduoguige", model.Manyspeci);
            cmd.AddParam("@isrebate", model.isrebate);
            cmd.AddParam("@unsure", model.unsure);
            cmd.AddParam("@unyuyueyanzheng", model.unyuyueyanzheng);
            cmd.AddParam("@selbindbx", model.selbindbx);

            cmd.AddParam("@Wrentserver", model.Wrentserver);
            cmd.AddParam("@WDeposit", model.WDeposit);
            cmd.AddParam("@Depositprice", model.Depositprice);
            cmd.AddParam("@pro_yanzheng_method", model.pro_yanzheng_method);
            cmd.AddParam("@firststationtime", model.firststationtime);

            cmd.AddParam("@pnonumperticket", model.pnonumperticket);
            cmd.AddParam("@worktimehour", model.worktimehour);
            cmd.AddParam("@worktimeid", model.worktimeid);
            cmd.AddParam("@SpecifyPosid", model.SpecifyPosid);
            cmd.AddParam("@isSetVisitDate", model.isSetVisitDate);

            cmd.AddParam("@progroupid", model.progroupid);
            cmd.AddParam("@issetidcard", model.issetidcard);
            cmd.AddParam("@zhaji_usenum", model.zhaji_usenum);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;

        }
        #endregion
        #region 修改产品服务信息
        internal int ModifyProExt(string proid, string service_Contain, string service_NotContain, string Precautions, string Sms)
        {
            var sqlTxt = @"UPDATE b2b_com_pro
		   SET 			   			   
			     service_Contain  =   @service_Contain,
			     service_NotContain=    @service_NotContain,
			     Precautions =   @Precautions,
                Sms =   @Sms		   
		   WHERE Id = @Id;";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@Id", proid);
            cmd.AddParam("@service_Contain", service_Contain);
            cmd.AddParam("@service_NotContain", service_NotContain);
            cmd.AddParam("@Precautions", Precautions);
            cmd.AddParam("@Sms", Sms);


            object obj = cmd.ExecuteScalar();

            return obj != null ? int.Parse(obj.ToString()) : 0;
        }
        #endregion
        #region 获取特定公司的产品列表
        //internal List<B2b_com_pro> ProPageList(string comid, int pageindex, int pagesize, out int totalcount)
        //{
        //    var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
        //    var tblName = "b2b_com_pro";
        //    var strGetFields = "*";
        //    var sortKey = "sortid";
        //    var sortMode = "1";
        //    var condition = "com_id=" + comid;
        //    cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


        //    List<B2b_com_pro> list = new List<B2b_com_pro>();
        //    using (var reader = cmd.ExecuteReader())
        //    {

        //        while (reader.Read())
        //        {
        //            list.Add(new B2b_com_pro
        //            {
        //                Id = reader.GetValue<int>("id"),
        //                Com_id = reader.GetValue<int>("com_id"),
        //                Pro_name = reader.GetValue<string>("pro_name"),
        //                Pro_state = reader.GetValue<int>("pro_state"),
        //                Server_type = reader.GetValue<int>("server_type"),
        //                Pro_type = reader.GetValue<int>("pro_type"),
        //                Source_type = reader.GetValue<int>("Source_type"),
        //                Pro_Remark = reader.GetValue<string>("pro_Remark"),
        //                Pro_start = reader.GetValue<DateTime>("pro_start"),
        //                Pro_end = reader.GetValue<DateTime>("pro_end"),
        //                Face_price = reader.GetValue<decimal>("face_price"),
        //                Advise_price = reader.GetValue<decimal>("advise_price"),
        //                Agentsettle_price = reader.GetValue<decimal>("agentsettle_price"),
        //                ThatDay_can = reader.GetValue<int>("ThatDay_can"),
        //                Thatday_can_day = reader.GetValue<int>("Thatday_can_day"),
        //                Service_Contain = reader.GetValue<string>("service_Contain"),
        //                Service_NotContain = reader.GetValue<string>("service_NotContain"),
        //                Precautions = reader.GetValue<string>("Precautions"),
        //                Tuan_pro = reader.GetValue<int>("tuan_pro"),
        //                Zhixiao = reader.GetValue<int>("zhixiao"),
        //                Agentsale = reader.GetValue<int>("agentsale"),
        //                Createtime = reader.GetValue<DateTime>("createtime"),
        //                Createuserid = reader.GetValue<int>("createuserid"),

        //                Imgurl = reader.GetValue<int>("imgurl")
        //            });

        //        }
        //    }
        //    totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

        //    return list;

        //}
        #endregion


        #region 产品排序列表
        internal List<B2b_com_pro> sortlist(int comid, out int totalcount)
        {

            var sqlTxt = @"select * from b2b_com_pro where com_id=@comid and pro_state=1 order by sortid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@comid", comid);

            List<B2b_com_pro> list = new List<B2b_com_pro>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_com_pro
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Pro_name = reader.GetValue<string>("pro_name"),
                        Pro_state = reader.GetValue<int>("pro_state"),
                        Server_type = reader.GetValue<int>("server_type"),
                        Pro_type = reader.GetValue<int>("pro_type"),
                        Source_type = reader.GetValue<int>("Source_type"),
                        Pro_Remark = reader.GetValue<string>("pro_Remark"),
                        Pro_start = reader.GetValue<DateTime>("pro_start"),
                        Pro_end = reader.GetValue<DateTime>("pro_end"),
                        Face_price = reader.GetValue<decimal>("face_price"),
                        Advise_price = reader.GetValue<decimal>("advise_price"),
                        Agentsettle_price = reader.GetValue<decimal>("agentsettle_price"),
                        ThatDay_can = reader.GetValue<int>("ThatDay_can"),
                        Thatday_can_day = reader.GetValue<int>("Thatday_can_day"),
                        Service_Contain = reader.GetValue<string>("service_Contain"),
                        Service_NotContain = reader.GetValue<string>("service_NotContain"),
                        Precautions = reader.GetValue<string>("Precautions"),
                        Tuan_pro = reader.GetValue<int>("tuan_pro"),
                        Zhixiao = reader.GetValue<int>("zhixiao"),
                        Agentsale = reader.GetValue<int>("agentsale"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Createuserid = reader.GetValue<int>("createuserid"),

                        Imgurl = reader.GetValue<int>("imgurl"),
                        Sortid = reader.GetValue<int>("sortid")
                    });

                }
            }
            totalcount = list.Count;
            return list;

        }
        #endregion

        internal int SortMenu(string menuid, int sortid)
        {
            string sql = "update b2b_com_pro set sortid=@sortid where id =@id ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", menuid);
            cmd.AddParam("@sortid", sortid);

            return cmd.ExecuteNonQuery();
        }
        #region 更新产品时同时更新帮 导入 的产品信息
        internal int UpBindingProInsertOrUpdate(B2b_com_pro model)
        {

            //agentsettle_price
            if (model != null)
            {
                if (model.Id != 0)
                {
                    string sql = "update b2b_com_pro set progroupid=@progroupid ,isSetVisitDate=@isSetVisitDate,SpecifyPosid=@SpecifyPosid, pnonumperticket=@pnonumperticket, pro_yanzheng_method=@pro_yanzheng_method, firststationtime=@firststationtime, ProValidateMethod=@ProValidateMethod,face_price=@face_price,Sms=@Sms,pro_start=@pro_start,pro_end=@pro_end,server_type=@server_type,pro_type=@pro_type,iscanuseonsameday=@iscanuseonsameday,pro_name=@pro_name, [serviceid]=@serviceid  ,[service_proid]=@service_proid  ,[realnametype]=@realnametype  ,[trval_productid] =@trval_productid ,[travel_type] =@travel_type  ,[trval_starting]=@trval_starting  ,[ispanicbuy]=@ispanicbuy  ,[panicbuybegin_time]=@panicbuybegin_time  ,[panicbuyenddate_time]=@panicbuyenddate_time  ,[linepro_booktype]=@linepro_booktype   ,[appointdata]=@appointdata   ,[childreduce]=@childreduce   ,[pickuppoint]=@pickuppoint  ,[dropoffpoint]=@dropoffpoint   ,[pro_note]=@pro_note   ,[QuitTicketMechanism] =@QuitTicketMechanism  ,[daybespeaknum]=@daybespeaknum   ,[isneedbespeak]=@isneedbespeak  ,[bespeaksucmsg]=@bespeaksucmsg  ,[bespeakfailmsg]=@bespeakfailmsg  ,[customservicephone]=@customservicephone ,[isblackoutdate]=@isblackoutdate  ,[etickettype]=@etickettype,unyuyueyanzheng=@unyuyueyanzheng,unsure=@unsure,selbindbx=@selbindbx,Manyspeci=@Manyspeci,Advise_price=@advise_price,imgurl=@imgurl,service_Contain=@service_Contain,service_NotContain=@service_NotContain,Precautions=@Precautions  where bindingid =@proid ";
                    var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                    cmd.AddParam("@progroupid", model.progroupid);
                    cmd.AddParam("@isSetVisitDate", model.isSetVisitDate);
                    cmd.AddParam("@proid", model.Id);
                    cmd.AddParam("@ProValidateMethod", model.ProValidateMethod);
                    cmd.AddParam("@face_price", model.Face_price);
                    cmd.AddParam("@Sms", model.Sms);
                    cmd.AddParam("@pro_start", model.Pro_start);
                    cmd.AddParam("@pro_end", model.Pro_end);
                    cmd.AddParam("@server_type", model.Server_type);
                    cmd.AddParam("@pro_type", model.Pro_type);
                    cmd.AddParam("@iscanuseonsameday", model.Iscanuseonsameday);

                    cmd.AddParam("@pro_name", model.Pro_name);
                    cmd.AddParam("@serviceid", model.Serviceid);
                    cmd.AddParam("@service_proid", model.Service_proid);
                    cmd.AddParam("@realnametype", model.Realnametype);
                    cmd.AddParam("@trval_productid", model.Travelproductid);
                    cmd.AddParam("@travel_type", model.Traveltype);
                    cmd.AddParam("@trval_starting", model.Travelstarting);
                    cmd.AddParam("@ispanicbuy", model.Ispanicbuy);
                    cmd.AddParam("@panicbuybegin_time", model.Panic_begintime);
                    cmd.AddParam("@panicbuyenddate_time", model.Panicbuy_endtime);
                    cmd.AddParam("@linepro_booktype", model.Linepro_booktype);
                    cmd.AddParam("@appointdata", model.Appointdata);

                    cmd.AddParam("@viewmethod", model.Viewmethod);
                    cmd.AddParam("@childreduce", model.Childreduce);
                    cmd.AddParam("@pickuppoint", model.pickuppoint);
                    cmd.AddParam("@dropoffpoint", model.dropoffpoint);
                    cmd.AddParam("@pro_note", model.pro_note);
                    cmd.AddParam("@QuitTicketMechanism", model.QuitTicketMechanism);
                    cmd.AddParam("@daybespeaknum", model.daybespeaknum);
                    cmd.AddParam("@isneedbespeak", model.isneedbespeak);
                    cmd.AddParam("@bespeaksucmsg", model.bespeaksucmsg);
                    cmd.AddParam("@bespeakfailmsg", model.bespeakfailmsg);
                    cmd.AddParam("@customservicephone", model.customservicephone);

                    cmd.AddParam("@isblackoutdate", model.isblackoutdate);
                    cmd.AddParam("@etickettype", model.etickettype);
                    cmd.AddParam("@unsure", model.unsure);
                    cmd.AddParam("@unyuyueyanzheng", model.unyuyueyanzheng);
                    cmd.AddParam("@selbindbx", model.selbindbx);
                    cmd.AddParam("@firststationtime", model.firststationtime);
                    cmd.AddParam("@pro_yanzheng_method", model.pro_yanzheng_method);
                    cmd.AddParam("@pnonumperticket", model.pnonumperticket);
                    cmd.AddParam("@SpecifyPosid", model.SpecifyPosid);
                    cmd.AddParam("@Manyspeci", model.Manyspeci);


                    cmd.AddParam("@advise_price", model.Advise_price);
                    cmd.AddParam("@imgurl", model.Imgurl);
                    cmd.AddParam("@service_Contain", model.Service_Contain);
                    cmd.AddParam("@service_NotContain", model.Service_NotContain);
                    cmd.AddParam("@Precautions", model.Precautions);

                    return cmd.ExecuteNonQuery();
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        #endregion



        #region 更新 导入 产品的上下线
        internal int UpBindingProState(int proid, int state)
        {

            //agentsettle_price

            if (proid != 0)
            {
                string sql = "update b2b_com_pro set Pro_state=@state  where bindingid =@proid ";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@proid", proid);
                cmd.AddParam("@state", state);

                return cmd.ExecuteNonQuery();
            }
            else
            {
                return 0;
            }

        }
        #endregion

        #region 更新产品时同时更新帮 导入 的产品 分销价（成本价）
        internal int UpBindingProUpdatePrice(int comid, int bindingid, decimal price)
        {

            //agentsettle_price

            string sql = "update b2b_com_pro set agentsettle_price=@price where bindingid =@bindingid and com_id=@comid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@bindingid", bindingid);
            cmd.AddParam("@price", price);
            cmd.AddParam("@comid", comid);
            return cmd.ExecuteNonQuery();
        }
        #endregion

        #region 获取票号
        internal List<B2b_eticket> SearchPnoPageList(string comid, int pageindex, int pagesize, int pro_id, int statetype, out int totalcount)
        {
            string sqltxt = @"select a.pro_name,b.id,b.pno,b.pnum,b.runstate,b.sendstate,b.subtime,b.sendtime,b.oid from b2b_com_pro as a left join b2b_stock_eticket as b on a.id=b.pro_id where a.com_id=@comid and a.source_type=2 and a.id=@proid ";
            if (statetype != 0)
            {
                sqltxt = sqltxt + " and runstate=" + statetype;
            }
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@proid", pro_id);
            cmd.AddParam("@comid", comid);
            int i = 0;
            List<B2b_eticket> list = new List<B2b_eticket>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_eticket
                    {
                        Id = reader.GetValue<int>("id"),
                        Pno = reader.GetValue<string>("Pno"),
                        Pnum = reader.GetValue<int>("pnum"),
                        E_proname = reader.GetValue<string>("pro_name"),
                        Runstate = reader.GetValue<int>("runstate"),
                        Oid = reader.GetValue<int>("oid"),
                        Send_state = reader.GetValue<int>("Sendstate"),
                        Subdate = reader.GetValue<DateTime>("subtime"),
                        Sendtime = reader.GetValue<DateTime>("sendtime")

                    });
                    i = i + 1;
                }
            }
            totalcount = i;

            return list;

        }
        #endregion

        #region 获取库存电子票数量
        internal int ProSEPageCount(int proid)
        {
            var sqlTxt = @"select count(pnum) as pnum from b2b_stock_eticket where pro_id=@Id;";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@Id", proid);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("pnum");
                }
                return 0;
            }
        }
        #endregion

        #region 获取验证电子票数量
        internal int ProYanzhengCount(int proid, DateTime startime, DateTime endtime, int all = 0, int agentid = 0)
        {
            endtime = endtime.AddDays(1);//小于明天0点

            var sqlTxt = @"select SUM(use_pnum) as use_pnum from b2b_etcket_log where action=1 and a_state=1 ";

            //有分销值，是导入产品的统计返回
            if (agentid != 0)
            {
                sqlTxt += " and eticket_id in (select id from b2b_eticket where pro_id=@Id and agent_id=@agentid)";
            }
            else
            {
                sqlTxt += " and eticket_id in (select id from b2b_eticket where pro_id=@Id)";
            }

            if (all == 0)
            {
                sqlTxt += " and actiondate>=@startime and actiondate<@endtime";
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@Id", proid);
            cmd.AddParam("@startime", startime);
            cmd.AddParam("@endtime", endtime);
            cmd.AddParam("@agentid", agentid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("use_pnum");
                }
                return 0;
            }
        }
        #endregion

        #region 获取酒店订购数量
        internal int ProHotelYanzhengCount(int proid, DateTime startime, DateTime endtime, int all = 0, int agentid = 0)
        {
            endtime = endtime.AddDays(1);//小于明天0点

            var sqlTxt = @"select SUM(a.u_num*b.bookdaynum) as u_num from b2b_order as a left join  b2b_order_hotel as b on a.id=b.orderid where a.Order_state in (4,8,22)";

            //有分销值，是导入产品的统计返回
            if (agentid != 0)
            {
                sqlTxt += " and  a.Pro_id=@Id and a.Agentid=@agentid";
            }
            else
            {
                sqlTxt += " and  a.pro_id=@Id";
            }

            if (all == 0)
            {
                sqlTxt += " and b.start_date>=@startime and b.start_date<@endtime";
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@Id", proid);
            cmd.AddParam("@startime", startime);
            cmd.AddParam("@endtime", endtime);
            cmd.AddParam("@agentid", agentid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("u_num");
                }
                return 0;
            }
        }
        #endregion


        #region 获取验证电子票数量
        internal int ProYanzhengCountbyProjectid(int comid, int projectid, DateTime startime, DateTime endtime, int all = 0, int agentpro = 0, int agentid = 0)
        {

            var sqlTxt = @"select SUM(use_pnum) as use_pnum from b2b_etcket_log where eticket_id in (select id from b2b_eticket where pro_id in (select id from b2b_com_pro where projectid=@Id)) and action=1 and a_state=1";

            //如果是导入产品，查询方式，更改已绑定方式查询
            if (agentpro == 1)
            {
                sqlTxt = @"select SUM(use_pnum) as use_pnum from b2b_etcket_log where eticket_id in (select id from b2b_eticket where agent_id=@agentid and pro_id in (select id from b2b_com_pro where projectid in (select bindingprojectid from b2b_com_project  where id=@Id and comid=@comid))) and action=1 and a_state=1";
            }


            if (all == 0)
            {
                sqlTxt += "  and actiondate>=@startime and actiondate<@endtime";
            }


            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@Id", projectid);
            cmd.AddParam("@startime", startime);
            cmd.AddParam("@endtime", endtime);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@agentid", agentid);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("use_pnum");
                }
                return 0;
            }
        }
        #endregion


        #region 获取酒店订购数量
        internal int ProHotelYanzhengCountbyProjectid(int comid, int projectid, DateTime startime, DateTime endtime, int all = 0, int agentpro = 0, int agentid = 0)
        {

            var sqlTxt = @"select SUM(a.u_num*b.bookdaynum) as u_num from b2b_order as a left join  b2b_order_hotel as b on a.id=b.orderid where  a.Order_state in (4,8,22)";

            //有分销值，是导入产品的统计返回
            if (agentid != 0)
            {
                sqlTxt += " and  a.Pro_id  in (select id from b2b_com_pro where projectid=@Id and server_type=9) and a.Agentid=@agentid";
            }
            else
            {
                sqlTxt += " and  a.pro_id in (select id from b2b_com_pro where projectid=@Id and server_type=9)";
            }

            if (all == 0)
            {
                sqlTxt += " and b.start_date>=@startime and b.start_date<@endtime ";
            }


            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@Id", projectid);
            cmd.AddParam("@startime", startime);
            cmd.AddParam("@endtime", endtime);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@agentid", agentid);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("u_num");
                }
                return 0;
            }
        }
        #endregion


        #region 获取库存电子票数量,未发送
        internal int ProSEPageCount_UNUse(int proid)
        {
            var sqlTxt = @"select count(pnum) as pnum from b2b_stock_eticket where pro_id=@Id and Runstate=1;";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@Id", proid);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("pnum");
                }
                return 0;
            }
        }
        #endregion

        #region 获取库存电子票数量,已发送
        internal int ProSEPageCount_Use(int proid)
        {
            var sqlTxt = @"select count(pnum) as pnum from b2b_stock_eticket where pro_id=@Id and Runstate=2;";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@Id", proid);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("pnum");
                }
                return 0;
            }
        }
        #endregion

        #region 获取库存电子票数量,作废
        internal int ProSEPageCount_Con(int proid)
        {
            var sqlTxt = @"select count(pnum) as pnum from b2b_stock_eticket where pro_id=@Id and Runstate=3;";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@Id", proid);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("pnum");
                }
                return 0;
            }
        }
        #endregion




        #region 查询产品列表 menuid 是商城首页栏目id，如果不为0则按此目录查询
        internal List<B2b_com_pro> Selectpagelist(string comid, int pageindex, int pagesize, string key, out int totalcount, int projectid = 0, int proclass = 0, int menuid = 0, int consultantid = 0, int channelid = 0, string channelphone = "", int allview = 0, string pno = "", int Servertype = 0)
        {

            int i = 0;
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = "com_id=" + comid + " and pro_state=1 and Server_type !=3 and pro_end>='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";

            if (Servertype != 0)
            {
                condition += " and Server_type=" + Servertype;
            }

            if (key == "抢购")
            {
                condition += " and ispanicbuy=1";
            }
            else if (key != "")
            {
                condition += "  and  pro_name like '%" + key + "%'";
            }

            if (projectid != 0)
            {
                condition += " and projectid=" + projectid;
            }

            if (proclass != 0)
            {
                condition += " and id in (select proid from b2b_com_pro_class where classid=" + proclass + " and comid=" + comid + ")";
            }

            if (menuid != 0)
            {
                condition += " and id in (select proid from [H5Menu_pro] where menuid=" + menuid + ")";
            }

            if (consultantid != 0)
            {
                condition += " and (projectid in (select linktype from [consultant_pro] where id=" + consultantid + ")  or bookpro_bindphone ='" + channelphone + "')";
            }
            if (channelid != 0)
            {
                condition += " and (id in (select proid from [consultant_pro_prolist] where menuid in (select id from consultant_pro where channelid=" + channelid + ")) or bookpro_bindphone ='" + channelphone + "') ";
            }

            string orderby = "sortid";

            if (channelphone != "")
            {
                orderby = "CASE bookpro_bindphone WHEN '" + channelphone + "' THEN 1  ELSE  2 END,sortid";
            }


            //产品新增显示设置 1.全部2分销3微信4.官网5.微信和官网

            if (allview == 1) { }
            else
            {
                condition += " and viewmethod in (1,3,5)";
            }


            if (pno != "")
            {//根据 预约电子码 查询有效产品

                string pno1 = EncryptionHelper.EticketPnoDES(pno, 1);//解密


                condition = "com_id=" + comid + " and pro_state=1 and Server_type !=3 and pro_end>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and id in(select proid from Bus_Feeticket_pro where busid in (select busid from bus_feeticket_pno where proid in (select pro_id from b2b_eticket where pno='" + pno1 + "')))";
            }



            cmd.PagingCommand1("b2b_com_pro", "*", orderby, "", pagesize, pageindex, "", condition);

            List<B2b_com_pro> list = new List<B2b_com_pro>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    i = i + 1;

                    list.Add(new B2b_com_pro
                    {
                        xuhao = i,
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Pro_name = reader.GetValue<string>("pro_name"),
                        Pro_state = reader.GetValue<int>("pro_state"),
                        Server_type = reader.GetValue<int>("server_type"),
                        Pro_type = reader.GetValue<int>("pro_type"),
                        Source_type = reader.GetValue<int>("Source_type"),
                        Pro_Remark = reader.GetValue<string>("pro_Remark"),
                        Pro_start = reader.GetValue<DateTime>("pro_start"),
                        Pro_end = reader.GetValue<DateTime>("pro_end"),
                        Face_price = reader.GetValue<decimal>("face_price"),
                        Advise_price = reader.GetValue<decimal>("advise_price"),
                        Agentsettle_price = reader.GetValue<decimal>("agentsettle_price"),
                        ThatDay_can = reader.GetValue<int>("ThatDay_can"),
                        Thatday_can_day = reader.GetValue<int>("Thatday_can_day"),
                        Service_Contain = reader.GetValue<string>("service_Contain"),
                        Service_NotContain = reader.GetValue<string>("service_NotContain"),
                        Precautions = reader.GetValue<string>("Precautions"),
                        Tuan_pro = reader.GetValue<int>("tuan_pro"),
                        Zhixiao = reader.GetValue<int>("zhixiao"),
                        Agentsale = reader.GetValue<int>("agentsale"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Createuserid = reader.GetValue<int>("createuserid"),
                        Pro_explain = reader.GetValue<string>("pro_explain"),
                        Ispanicbuy = reader.GetValue<int>("Ispanicbuy"),
                        Limitbuytotalnum = reader.GetValue<int>("Limitbuytotalnum"),
                        ishasdeliveryfee = reader.GetValue<int>("ishasdeliveryfee"),
                        Imgurl = reader.GetValue<int>("imgurl"),
                        Pro_Integral = reader.GetValue<decimal>("Pro_Integral"),
                        Projectid = reader.GetValue<int>("projectid"),
                        Bindingid = reader.GetValue<int>("Bindingid"),
                        Imgaddress = FileSerivce.GetImgUrl(reader.GetValue<int>("imgurl")),
                        Manyspeci = reader.GetValue<int>("Manyspeci"),
                        unsure = reader.GetValue<int>("unsure"),
                        unyuyueyanzheng = reader.GetValue<int>("unyuyueyanzheng"),
                        firststationtime = reader.GetValue<string>("firststationtime"),

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;

        }
        #endregion

        #region 查询产品列表 menuid 是商城首页栏目id，如果不为0则按此目录查询
        internal List<B2b_com_pro> TopPropagelist(string comid, int pageindex, int pagesize, out int totalcount)
        {


            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = "com_id=" + comid + " and pro_state=1 and Server_type !=3 and pro_end>='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";


            //产品新增显示设置 1.全部2分销3微信4.官网5.微信和官网
            condition += " and viewmethod in (1,3,5)";

            condition += " and id in (select top " + pagesize + " pro_id from b2b_order where comid=" + comid + "  group by pro_id order by sum(u_num) desc )";


            cmd.PagingCommand1("b2b_com_pro", "*", "sortid", "", pagesize, pageindex, "", condition);

            List<B2b_com_pro> list = new List<B2b_com_pro>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_com_pro
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Pro_name = reader.GetValue<string>("pro_name"),
                        Pro_state = reader.GetValue<int>("pro_state"),
                        Server_type = reader.GetValue<int>("server_type"),
                        Pro_type = reader.GetValue<int>("pro_type"),
                        Source_type = reader.GetValue<int>("Source_type"),
                        Pro_Remark = reader.GetValue<string>("pro_Remark"),
                        Pro_start = reader.GetValue<DateTime>("pro_start"),
                        Pro_end = reader.GetValue<DateTime>("pro_end"),
                        Face_price = reader.GetValue<decimal>("face_price"),
                        Advise_price = reader.GetValue<decimal>("advise_price"),
                        Agentsettle_price = reader.GetValue<decimal>("agentsettle_price"),
                        ThatDay_can = reader.GetValue<int>("ThatDay_can"),
                        Thatday_can_day = reader.GetValue<int>("Thatday_can_day"),
                        Service_Contain = reader.GetValue<string>("service_Contain"),
                        Service_NotContain = reader.GetValue<string>("service_NotContain"),
                        Precautions = reader.GetValue<string>("Precautions"),
                        Tuan_pro = reader.GetValue<int>("tuan_pro"),
                        Zhixiao = reader.GetValue<int>("zhixiao"),
                        Agentsale = reader.GetValue<int>("agentsale"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Createuserid = reader.GetValue<int>("createuserid"),
                        Pro_explain = reader.GetValue<string>("pro_explain"),
                        Ispanicbuy = reader.GetValue<int>("Ispanicbuy"),
                        Limitbuytotalnum = reader.GetValue<int>("Limitbuytotalnum"),
                        ishasdeliveryfee = reader.GetValue<int>("ishasdeliveryfee"),
                        Imgurl = reader.GetValue<int>("imgurl"),
                        Pro_Integral = reader.GetValue<decimal>("Pro_Integral"),
                        Projectid = reader.GetValue<int>("projectid"),
                        Bindingid = reader.GetValue<int>("Bindingid"),
                        Imgaddress = FileSerivce.GetImgUrl(reader.GetValue<int>("imgurl")),
                        Manyspeci = reader.GetValue<int>("Manyspeci"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;

        }
        #endregion


        //查询此项目下如果含有非酒店产品则返回0，如果不含则返回1
        internal int Selectpro_hotel(int comid, int projectid)
        {
            const string sqltxt = @"SELECT  *  FROM b2b_com_pro where projectid=@projectid and not server_type=9 and com_id=@comid";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@projectid", projectid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return 0;
                }
                return 1;
            }
        }



        internal B2b_com_pro GetProById(string proid, int Speciid = 0, int channelcoachid = 0)
        {
            const string sqltxt = @"SELECT   *
 FROM b2b_com_pro where [Id]=@proid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@proid", proid);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_com_pro
                    {
                        issetidcard = reader.GetValue<int>("issetidcard"),
                        progroupid = reader.GetValue<int>("progroupid"),
                        isSetVisitDate = reader.GetValue<int>("isSetVisitDate"),
                        SpecifyPosid = reader.GetValue<string>("SpecifyPosid"),
                        pnonumperticket = reader.GetValue<int>("pnonumperticket"),
                        firststationtime = reader.GetValue<string>("firststationtime"),
                        pro_yanzheng_method = reader.GetValue<int>("pro_yanzheng_method"),
                        ishasdeliveryfee = reader.GetValue<int>("ishasdeliveryfee"),
                        deliverytmp = reader.GetValue<int>("deliverytmp"),
                        Id = reader.GetValue<int>("id"),
                        merchant_code = reader.GetValue<string>("merchant_code"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Pro_name = reader.GetValue<string>("pro_name") + new B2b_com_pro_SpeciData().Getspecinamebyid(Speciid) + new B2bComProData().Getcoachnamebyid(channelcoachid),
                        Pro_state = reader.GetValue<int>("pro_state"),
                        Server_type = reader.GetValue<int>("server_type"),
                        Pro_type = reader.GetValue<int>("pro_type"),
                        Source_type = reader.GetValue<int>("source_type"),
                        Pro_Remark = reader.GetValue<string>("pro_Remark"),
                        Pro_start = reader.GetValue<DateTime>("pro_start"),
                        Pro_end = reader.GetValue<DateTime>("pro_end"),
                        Face_price = reader.GetValue<decimal>("face_price"),
                        Advise_price = reader.GetValue<decimal>("advise_price"),
                        Agent1_price = reader.GetValue<decimal>("Agent1_price"),
                        Agent2_price = reader.GetValue<decimal>("Agent2_price"),
                        Agent3_price = reader.GetValue<decimal>("Agent3_price"),
                        Agentsettle_price = reader.GetValue<decimal>("agentsettle_price"),
                        ThatDay_can = reader.GetValue<int>("ThatDay_can"),
                        Thatday_can_day = reader.GetValue<int>("Thatday_can_day"),
                        Service_Contain = reader.GetValue<string>("service_Contain"),
                        Service_NotContain = reader.GetValue<string>("service_NotContain"),
                        Precautions = reader.GetValue<string>("Precautions"),
                        Tuan_pro = reader.GetValue<int>("tuan_pro"),
                        Zhixiao = reader.GetValue<int>("zhixiao"),
                        Agentsale = reader.GetValue<int>("agentsale"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Sms = reader.GetValue<string>("Sms"),
                        Createuserid = reader.GetValue<int>("createuserid"),
                        Imgurl = reader.GetValue<int>("imgurl"),
                        Pro_number = reader.GetValue<int>("pro_number"),
                        Pro_explain = reader.GetValue<string>("pro_explain"),
                        Pro_Integral = reader.GetValue<decimal>("pro_Integral"),
                        Tuipiao = reader.GetValue<int>("TuiPiao"),
                        Tuipiao_guoqi = reader.GetValue<int>("TuiPiao_Guoqi"),
                        Tuipiao_endday = reader.GetValue<int>("TuiPiao_Endday"),
                        Serviceid = reader.GetValue<int>("serviceid"),
                        Service_proid = reader.GetValue<string>("service_proid"),
                        Realnametype = reader.GetValue<int>("realnametype"),
                        Projectid = reader.GetValue<int>("projectid"),

                        Travelproductid = reader.GetValue<int>("trval_productid"),
                        Traveltype = reader.GetValue<string>("travel_type").ConvertTo<int>(0),
                        Travelstarting = reader.GetValue<string>("trval_starting"),

                        Ispanicbuy = reader.GetValue<int>("ispanicbuy"),
                        Panic_begintime = reader.GetValue<DateTime>("panicbuybegin_time"),
                        Panicbuy_endtime = reader.GetValue<DateTime>("panicbuyenddate_time"),
                        Limitbuytotalnum = reader.GetValue<int>("limitbuytotalnum"),
                        Linepro_booktype = reader.GetValue<int>("linepro_booktype"),
                        ProValidateMethod = reader.GetValue<string>("ProValidateMethod"),
                        Appointdata = reader.GetValue<int>("appointdata"),
                        Iscanuseonsameday = reader.GetValue<int>("iscanuseonsameday"),
                        Viewmethod = reader.GetValue<int>("viewmethod"),
                        Childreduce = reader.GetValue<decimal>("childreduce"),
                        Bindingid = reader.GetValue<int>("Bindingid"),
                        pickuppoint = reader.GetValue<string>("pickuppoint"),
                        dropoffpoint = reader.GetValue<string>("dropoffpoint"),
                        pro_note = reader.GetValue<string>("pro_note"),
                        QuitTicketMechanism = reader.GetValue<int>("QuitTicketMechanism"),

                        daybespeaknum = reader.GetValue<int>("daybespeaknum"),
                        isneedbespeak = reader.GetValue<int>("isneedbespeak"),
                        bespeaksucmsg = reader.GetValue<string>("bespeaksucmsg"),
                        bespeakfailmsg = reader.GetValue<string>("bespeakfailmsg"),
                        customservicephone = reader.GetValue<string>("customservicephone"),
                        isblackoutdate = reader.GetValue<int>("isblackoutdate"),
                        etickettype = reader.GetValue<int>("etickettype"),
                        buynum = reader.GetValue<int>("buynum"),
                        pro_weight = reader.GetValue<decimal>("pro_weight"),
                        nuomi_dealid = reader.GetValue<string>("nuomi_dealid"),
                        bookpro_bindphone = reader.GetValue<string>("bookpro_bindphone"),
                        bookpro_ispay = reader.GetValue<int>("bookpro_ispay"),
                        bookpro_bindname = reader.GetValue<string>("bookpro_bindname"),
                        bookpro_bindcompany = reader.GetValue<string>("bookpro_bindcompany"),
                        Manyspeci = reader.GetValue<int>("Manyspeci"),
                        isrebate = reader.GetValue<int>("isrebate"),
                        unsure = reader.GetValue<int>("unsure"),
                        unyuyueyanzheng = reader.GetValue<int>("unyuyueyanzheng"),
                        selbindbx = reader.GetValue<int>("selbindbx"),
                        Wrentserver = reader.GetValue<int>("Wrentserver"),
                        WDeposit = reader.GetValue<int>("WDeposit"),
                        Depositprice = reader.GetValue<decimal>("Depositprice"),
                        worktimehour = reader.GetValue<int>("worktimehour"),
                        worktimeid = reader.GetValue<int>("worktimeid"),
                        zhaji_usenum = reader.GetValue<int>("zhaji_usenum"),
                    };
                }
                return null;
            }
        }


        //查询产品是否过期 ，只针对 106商户 如果过期的弹出已过期 按钮
        internal int GetProyouxiaoqiById(int proid)
        {
            const string sqltxt = @"SELECT   *
 FROM b2b_com_pro where [Id]=@proid and com_id=106";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@proid", proid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    if (DateTime.Compare(DateTime.Now, reader.GetValue<DateTime>("pro_end")) > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                return 0;
            }
        }

        internal B2b_com_pro GetProspeciidById(string proid, int Speciid)
        {
            const string sqltxt = @"SELECT * FROM b2b_com_pro_Speci where [proid]=@proid and id=@Speciid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@proid", proid);
            cmd.AddParam("@Speciid", Speciid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_com_pro
                    {

                        Id = reader.GetValue<int>("id"),
                        Pro_name = reader.GetValue<string>("speci_name"),

                        Face_price = reader.GetValue<decimal>("speci_face_price"),
                        Advise_price = reader.GetValue<decimal>("speci_advise_price"),
                        Agent1_price = reader.GetValue<decimal>("speci_agent1_price"),
                        Agent2_price = reader.GetValue<decimal>("speci_agent2_price"),
                        Agent3_price = reader.GetValue<decimal>("speci_agent3_price"),
                        Agentsettle_price = reader.GetValue<decimal>("speci_agentsettle_price"),
                        Limitbuytotalnum = reader.GetValue<int>("speci_totalnum"),//库存数量 

                    };
                }
                return null;
            }
        }


        internal int GetTopProImageById(int comid)
        {
            const string sqltxt = @"SELECT  top 1 * FROM b2b_com_pro where [Com_id]=@comid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@comid", comid);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("imgurl");
                }
                return 0;
            }
        }



        internal int GetProSource_typeById(string proid)
        {
            const string sqltxt = @"SELECT   *
 FROM b2b_com_pro where [Id]=@proid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@proid", proid);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("Source_type");
                }
                return 0;
            }
        }


        internal int GetProServer_typeById(string proid)
        {
            const string sqltxt = @"SELECT   *
 FROM b2b_com_pro where [Id]=@proid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@proid", proid);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("Server_type");
                }
                return 0;
            }
        }

        internal string GetProRecerceSMSpeopleById(string proid)
        {
            const string sqltxt = @"SELECT   *
 FROM b2b_com_pro where [Id]=@proid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@proid", proid);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<string>("bookpro_bindcompany") + ":" + reader.GetValue<string>("bookpro_bindname") + "(" + reader.GetValue<string>("bookpro_bindphone") + ")";
                }
                return "";
            }
        }


        internal string GetTravelEndingByLineid(int lineid)
        {
            string sql = "select id,ending,lineid from b2b_com_trvalending where lineid=" + lineid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            try
            {

                using (var reader = cmd.ExecuteReader())
                {
                    string endingstr = "";
                    while (reader.Read())
                    {
                        endingstr += reader.GetValue<string>("ending") + ",";
                    }
                    if (endingstr.IndexOf(",") > -1)
                    {
                        endingstr = endingstr.Substring(0, endingstr.Length - 1);
                    }
                    return endingstr;
                }

            }
            catch (Exception e)
            {
                return "";
            }
        }

        //客户看到产品详情，只能看到本商户的产品
        internal B2b_com_pro ClientGetProById(string proid, int comid)
        {
            const string sqltxt = @"SELECT  *
  FROM [b2b_com_pro] where [Id]=@proid  and com_id=@comid";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@proid", proid);
            cmd.AddParam("@comid", comid);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_com_pro
                    {
                        isSetVisitDate = reader.GetValue<int>("isSetVisitDate"),
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Pro_name = reader.GetValue<string>("pro_name"),
                        Pro_state = reader.GetValue<int>("pro_state"),
                        Server_type = reader.GetValue<int>("server_type"),
                        Pro_type = reader.GetValue<int>("pro_type"),
                        Source_type = reader.GetValue<int>("source_type"),
                        Pro_Remark = reader.GetValue<string>("pro_Remark"),
                        Pro_start = reader.GetValue<DateTime>("pro_start"),
                        Pro_end = reader.GetValue<DateTime>("pro_end"),
                        Face_price = reader.GetValue<decimal>("face_price"),
                        Advise_price = reader.GetValue<decimal>("advise_price"),
                        Agentsettle_price = reader.GetValue<decimal>("agentsettle_price"),
                        ThatDay_can = reader.GetValue<int>("ThatDay_can"),
                        Thatday_can_day = reader.GetValue<int>("Thatday_can_day"),
                        Service_Contain = reader.GetValue<string>("service_Contain"),
                        Service_NotContain = reader.GetValue<string>("service_NotContain"),
                        Precautions = reader.GetValue<string>("Precautions"),
                        Tuan_pro = reader.GetValue<int>("tuan_pro"),
                        Zhixiao = reader.GetValue<int>("zhixiao"),
                        Agentsale = reader.GetValue<int>("agentsale"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Sms = reader.GetValue<string>("Sms"),
                        Createuserid = reader.GetValue<int>("createuserid"),
                        Imgurl = reader.GetValue<int>("imgurl"),
                        Pro_number = reader.GetValue<int>("pro_number"),
                        Pro_explain = reader.GetValue<string>("pro_explain"),
                        Pro_Integral = reader.GetValue<decimal>("pro_Integral"),
                        Tuipiao = reader.GetValue<int>("TuiPiao"),
                        Tuipiao_guoqi = reader.GetValue<int>("TuiPiao_Guoqi"),
                        Tuipiao_endday = reader.GetValue<int>("TuiPiao_Endday"),
                        Serviceid = reader.GetValue<int>("serviceid"),
                        Service_proid = reader.GetValue<string>("service_proid"),
                        Realnametype = reader.GetValue<int>("realnametype"),
                        Projectid = reader.GetValue<int>("projectid"),

                        Travelproductid = reader.GetValue<int>("trval_productid"),
                        Traveltype = reader.GetValue<string>("travel_type").ConvertTo<int>(0),
                        Travelstarting = reader.GetValue<string>("trval_starting"),
                        Housetype = new B2b_com_housetypeData().GetHouseType(reader.GetValue<int>("id"), comid),
                        ProValidateMethod = reader.GetValue<string>("ProValidateMethod"),
                        Appointdata = reader.GetValue<int>("Appointdata"),
                        Iscanuseonsameday = reader.GetValue<int>("Iscanuseonsameday"),
                        Ispanicbuy = reader.GetValue<int>("ispanicbuy"),
                        Limitbuytotalnum = reader.GetValue<int>("limitbuytotalnum"),

                        Pro_youxiaoqi = GetPro_Youxiaoqi(reader.GetValue<DateTime>("pro_start"), reader.GetValue<DateTime>("pro_end"), reader.GetValue<string>("ProValidateMethod"), reader.GetValue<int>("Appointdata"), reader.GetValue<int>("Iscanuseonsameday")),
                        unsure = reader.GetValue<int>("unsure"),
                        unyuyueyanzheng = reader.GetValue<int>("unyuyueyanzheng"),
                        Bindingid = reader.GetValue<int>("Bindingid"),
                    };
                }
                return null;
            }
        }


        internal int ModifyProState(int proid, int prostate)
        {
            var sqlTxt = @"UPDATE b2b_com_pro SET pro_state=@prostate WHERE Id = @Id;";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@Id", proid);
            cmd.AddParam("@prostate", prostate);
            return cmd.ExecuteNonQuery();

        }


        internal int LuruEticket(int proid, string key, int comid, int pnum)
        {
            var sqlTxt = @"insert b2b_Stock_Eticket (com_id,pro_id,pno,pnum) values(@comid,@proid,@key,@pnum);";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@proid", proid);
            cmd.AddParam("@key", key);
            cmd.AddParam("@pnum", pnum);
            return cmd.ExecuteNonQuery();
        }

        internal int ZuofeiEticket(int proid, string key, int comid)
        {
            var sqlTxt = @"update b2b_Stock_Eticket set runstate=3 where com_id=@comid and pro_id=@proid and runstate=1 and pno=@key;";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@proid", proid);
            cmd.AddParam("@key", key);
            return cmd.ExecuteNonQuery();
        }


        internal string SearchEticket(int comid, string key)
        {
            var sqlTxt = @"select * from b2b_Stock_Eticket where com_id=@comid and pno in(" + key + ")";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<string>("pno");
                }
                return "";
            }

        }

        internal string SearchWeishiyongEticket(int comid, string key, out int countnum)
        {
            var sqlTxt = @"select * from b2b_Stock_Eticket where com_id=@comid and pno in(" + key + ")";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@comid", comid);
            int i = 0;
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    if (reader.GetValue<int>("runstate") == 2)
                    {
                        countnum = 0;
                        return reader.GetValue<string>("pno") + "已使用";
                    }

                    if (reader.GetValue<int>("runstate") == 3)
                    {
                        countnum = 0;
                        return reader.GetValue<string>("pno") + "已作废";
                    }
                    i = i + 1;
                }

                countnum = i;
                if (i > 0)
                {
                    return "";
                }
                else
                {
                    return "没有此电子票！";
                }


            }

        }

        internal string ReaderTop1Eticket(int proid, int comid, int num, int order_no, out string pnostr)
        {
            string pno = "";
            string pid = "";
            pnostr = "";
            int i = 0;
            var sqlTxt = @"select top " + num + " * from b2b_Stock_Eticket where com_id=@comid and pro_id=@proid and runstate=1";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@proid", proid);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    pno = pno + "码:" + reader.GetValue<string>("pno");
                    pnostr = pnostr + reader.GetValue<string>("pno") + ",";
                    pid = pid + reader.GetValue<int>("id").ToString() + ",";
                    i = i + 1;
                }

                //如果读取出的码，不等于要得到的码数量，则代表码不够，则返回无码，不发送
                if (num != i)
                {
                    pnostr = "";
                    return "";
                }


                reader.Close();

                if (pid != "")
                {
                    pid = pid.Substring(0, pid.Length - 1);
                    sqlTxt = @"update b2b_Stock_Eticket set runstate=2,sendstate=2,oid=" + order_no + ",sendtime='" + DateTime.Now + "' where id in (" + pid + ")";
                    var cmd1 = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                    cmd1.ExecuteReader();
                }
                pnostr = pnostr.Substring(0, pnostr.Length - 1);
                return pno;
            }

        }

        //先判断是否有此订单的电子码已经读取了，防止重复读取，如果已读取直接读取此电子码
        internal string SearchTop1Eticket(int proid, int comid, int num, int order_no, out string pnostr)
        {
            string pno = "";
            string pid = "";
            pnostr = "";
            int i = 0;
            var sqlTxt = @"select * from b2b_Stock_Eticket where com_id=@comid and pro_id=@proid and oid=@order_no";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@proid", proid);
            cmd.AddParam("@order_no", order_no);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    pno = pno + "码:" + reader.GetValue<string>("pno");
                    pnostr = pnostr + reader.GetValue<string>("pno") + ",";
                    pid = pid + reader.GetValue<int>("id").ToString() + ",";
                    i = i + 1;
                }
                if (pnostr != "")
                {
                    pnostr = pnostr.Substring(0, pnostr.Length - 1);
                }
                return pno;
            }

        }

        internal List<B2b_com_pro> ProPageList(string comid, int pageindex, int pagesize, int prostate, out int totalcount, int projectid = 0, string key = "", string viewmethod = "", int canviewpro = 0, int userid = 0, int servertype = 0)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var tblName = "b2b_com_pro";
            var strGetFields = "*";
            var sortKey = "sortid,id desc";
            //var sortMode = "0";
            var condition = "com_id=" + comid;
            if (projectid > 0)
            {
                condition += " and projectid=" + projectid;
            }

            if (prostate == 1)//分销或其他地方用到读取上线并且在有效期内产品
            {
                condition += " and pro_state = 1" + " and pro_end>='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            }
            if (prostate == 2)//读取上线产品
            {
                condition += " and pro_state = 1";
            }
            if (prostate == 3)//读取下线产品
            {
                condition += " and pro_state = 0";
            }



            if (key != "")
            {
                if (key.ConvertTo<int>(0) == 0)
                {
                    condition += " and pro_name like '%" + key + "%'";
                }
                else
                {
                    condition += " and  id=" + key;
                }
            }
            //产品新增加显示设置  1.全部2分销3微信4.官网5.微信和官网;商家后台显示全部的
            if (viewmethod != "")
            {
                condition += " and viewmethod in (" + viewmethod + ")";
            }

            //可以看到产品的范围
            if (canviewpro == 1)
            {
                //限制只是可以看到自己发布的产品
                if (userid > 0)
                {
                    condition += " and createuserid =" + userid;
                }
            }
            if (servertype != 0)
            {
                condition += " and  server_type=" + servertype;
            }

            //cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);
            cmd.PagingCommand1(tblName, strGetFields, sortKey, "", pagesize, pageindex, "0", condition);

            List<B2b_com_pro> list = new List<B2b_com_pro>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_com_pro
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Pro_name = reader.GetValue<string>("pro_name"),
                        Pro_state = reader.GetValue<int>("pro_state"),
                        Server_type = reader.GetValue<int>("server_type"),
                        Pro_type = reader.GetValue<int>("pro_type"),
                        Source_type = reader.GetValue<int>("Source_type"),
                        Pro_Remark = reader.GetValue<string>("pro_Remark"),
                        Pro_start = reader.GetValue<DateTime>("pro_start"),
                        Pro_end = reader.GetValue<DateTime>("pro_end"),
                        Face_price = reader.GetValue<decimal>("face_price"),
                        Advise_price = reader.GetValue<decimal>("advise_price"),
                        Agent1_price = reader.GetValue<decimal>("Agent1_price"),
                        Agent2_price = reader.GetValue<decimal>("Agent2_price"),
                        Agent3_price = reader.GetValue<decimal>("Agent3_price"),
                        Agentsettle_price = reader.GetValue<decimal>("agentsettle_price"),
                        ThatDay_can = reader.GetValue<int>("ThatDay_can"),
                        Thatday_can_day = reader.GetValue<int>("Thatday_can_day"),
                        Service_Contain = reader.GetValue<string>("service_Contain"),
                        Service_NotContain = reader.GetValue<string>("service_NotContain"),
                        Precautions = reader.GetValue<string>("Precautions"),
                        Tuan_pro = reader.GetValue<int>("tuan_pro"),
                        Zhixiao = reader.GetValue<int>("zhixiao"),
                        Agentsale = reader.GetValue<int>("agentsale"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Createuserid = reader.GetValue<int>("createuserid"),

                        Imgurl = reader.GetValue<int>("imgurl"),
                        Tuipiao = reader.GetValue<int>("TuiPiao"),
                        Tuipiao_guoqi = reader.GetValue<int>("TuiPiao_Guoqi"),
                        Tuipiao_endday = reader.GetValue<int>("TuiPiao_Endday"),
                        Projectid = reader.GetValue<int>("projectid"),
                        Bindingid = reader.GetValue<int>("Bindingid"),
                        Viewmethod = reader.GetValue<int>("Viewmethod"),
                        Ispanicbuy = reader.GetValue<int>("ispanicbuy"),
                        Manyspeci = reader.GetValue<int>("manyspeci"),
                        unsure = reader.GetValue<int>("unsure"),
                        unyuyueyanzheng = reader.GetValue<int>("unyuyueyanzheng"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }

        #region 最大最小价格

        internal string Pro_max(int comid)
        {
            var sqltxt = @"select max(advise_price) as maxprice,MIN(advise_price) as minprice  from b2b_com_pro where com_id=@comid ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@comid", comid);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<decimal>("maxprice").ToString() + "," + reader.GetValue<decimal>("minprice").ToString();
                }
                return "0,0";
            }

        }

        #endregion

        internal List<B2b_com_pro> statepagelist(int comid, int pageindex, int pagesize, int state, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "b2b_com_pro";
            var strGetFields = "*";
            var sortKey = "sortid";
            var sortMode = "1";
            var condition = "com_id=" + comid + " and pro_state=" + state;
            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<B2b_com_pro> list = new List<B2b_com_pro>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_com_pro
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Pro_name = reader.GetValue<string>("pro_name"),
                        Pro_state = reader.GetValue<int>("pro_state"),
                        Server_type = reader.GetValue<int>("server_type"),
                        Pro_type = reader.GetValue<int>("pro_type"),
                        Source_type = reader.GetValue<int>("Source_type"),
                        Pro_Remark = reader.GetValue<string>("pro_Remark"),
                        Pro_explain = reader.GetValue<string>("Pro_explain"),
                        Pro_start = reader.GetValue<DateTime>("pro_start"),
                        Pro_end = reader.GetValue<DateTime>("pro_end"),
                        Face_price = reader.GetValue<decimal>("face_price"),
                        Advise_price = reader.GetValue<decimal>("advise_price"),
                        Agentsettle_price = reader.GetValue<decimal>("agentsettle_price"),
                        ThatDay_can = reader.GetValue<int>("ThatDay_can"),
                        Thatday_can_day = reader.GetValue<int>("Thatday_can_day"),
                        Service_Contain = reader.GetValue<string>("service_Contain"),
                        Service_NotContain = reader.GetValue<string>("service_NotContain"),
                        Precautions = reader.GetValue<string>("Precautions"),
                        Tuan_pro = reader.GetValue<int>("tuan_pro"),
                        Zhixiao = reader.GetValue<int>("zhixiao"),
                        Agentsale = reader.GetValue<int>("agentsale"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Createuserid = reader.GetValue<int>("createuserid"),

                        Imgurl = reader.GetValue<int>("imgurl"),
                        Tuipiao = reader.GetValue<int>("TuiPiao"),
                        Tuipiao_guoqi = reader.GetValue<int>("TuiPiao_Guoqi"),
                        Tuipiao_endday = reader.GetValue<int>("TuiPiao_Endday")
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;

        }


        internal B2b_com_pro GetProByOrderID(int orderid)
        {
            const string sqltxt = @"SELECT   [id]
      ,[com_id]
      ,[pro_name]
      ,[pro_state]
      ,[server_type]
      ,[pro_type]
      ,[source_type]
      ,[pro_Remark]
      ,[pro_start]
      ,[pro_end]
      ,[face_price]
      ,[advise_price]
      ,[agentsettle_price]
      ,[ThatDay_can]
      ,[Thatday_can_day]
      ,[service_Contain]
      ,[service_NotContain]
      ,[Precautions]
      ,[tuan_pro]
      ,[zhixiao]
      ,[agentsale]
      ,[createuserid]
      ,[createtime]
        ,[sms]
        ,[imgurl]
        ,[pro_number]
        ,[pro_explain]
        ,[pro_Integral]
          FROM [b2b_com_pro] where [Id] in (select pro_id from b2b_order where id=@orderid) ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@orderid", orderid);



            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_com_pro
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Pro_name = reader.GetValue<string>("pro_name"),
                        Pro_state = reader.GetValue<int>("pro_state"),
                        Server_type = reader.GetValue<int>("server_type"),
                        Pro_type = reader.GetValue<int>("pro_type"),
                        Source_type = reader.GetValue<int>("source_type"),
                        Pro_Remark = reader.GetValue<string>("pro_Remark"),
                        Pro_start = reader.GetValue<DateTime>("pro_start"),
                        Pro_end = reader.GetValue<DateTime>("pro_end"),
                        Face_price = reader.GetValue<decimal>("face_price"),
                        Advise_price = reader.GetValue<decimal>("advise_price"),
                        Agentsettle_price = reader.GetValue<decimal>("agentsettle_price"),
                        ThatDay_can = reader.GetValue<int>("ThatDay_can"),
                        Thatday_can_day = reader.GetValue<int>("Thatday_can_day"),
                        Service_Contain = reader.GetValue<string>("service_Contain"),
                        Service_NotContain = reader.GetValue<string>("service_NotContain"),
                        Precautions = reader.GetValue<string>("Precautions"),
                        Tuan_pro = reader.GetValue<int>("tuan_pro"),
                        Zhixiao = reader.GetValue<int>("zhixiao"),
                        Agentsale = reader.GetValue<int>("agentsale"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Sms = reader.GetValue<string>("Sms"),
                        Createuserid = reader.GetValue<int>("createuserid"),
                        Imgurl = reader.GetValue<int>("imgurl"),
                        Pro_number = reader.GetValue<int>("pro_number"),
                        Pro_explain = reader.GetValue<string>("pro_explain"),
                        Pro_Integral = reader.GetValue<decimal>("pro_Integral")
                    };
                }
                return null;
            }
        }



        //分类列表
        internal List<B2b_com_class> Proclasslist(int pageindex, int pagesize, out int totalcount, int industryid = 0)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "b2b_com_class";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "0";
            var condition = "";
            if (industryid != 0)
            {
                condition = condition + " industryid=" + industryid;
            }

            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<B2b_com_class> list = new List<B2b_com_class>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_com_class
                    {
                        Id = reader.GetValue<int>("id"),
                        Classid = reader.GetValue<int>("id"),
                        Classname = reader.GetValue<string>("Classname"),
                        Industryid = reader.GetValue<int>("industryid")
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;

        }



        internal B2b_com_class Proclassbyid(int classid)
        {
            const string sqltxt = @"SELECT   *
          FROM [dbo].[b2b_com_class] where [Id]=@classid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@classid", classid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_com_class
                    {
                        Id = reader.GetValue<int>("id"),
                        Classid = reader.GetValue<int>("id"),
                        Classname = reader.GetValue<string>("Classname"),
                        Industryid = reader.GetValue<int>("industryid")
                    };
                }
                return null;
            }
        }

        internal int Searchproclassbyid(int proid)
        {
            const string sqltxt = @"SELECT a.classid,b.classname
          FROM b2b_com_pro_class as a left join [b2b_com_class] as b on a.classid=b.id where a.[proId]=@proid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@proid", proid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("classid");
                }
                return 0;
            }
        }

        internal string Searchproclassnamebyid(int proid)
        {
            const string sqltxt = @"SELECT a.classid,b.classname
          FROM b2b_com_pro_class as a left join [b2b_com_class] as b on a.classid=b.id where a.[proId]=@proid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@proid", proid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<string>("classname");
                }
                return "";
            }
        }
        internal int upproclass(int proid, int proclass, int comid)
        {   //先删除
            const string sqltxt2 = @"delete [b2b_com_pro_class] where proid= @proid and comid=@comid";
            var cmd2 = this.sqlHelper.PrepareTextSqlCommand(sqltxt2);
            cmd2.AddParam("@proid", proid);
            cmd2.AddParam("@comid", comid);
            cmd2.ExecuteNonQuery();

            //再插入
            const string sqltxt = @"insert [b2b_com_pro_class] (classid,proid,comid) values(@classid,@proid,@comid)";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@proid", proid);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@classid", proclass);

            return cmd.ExecuteNonQuery();

        }


        internal int Proclassmanage(int classid, string classname, int industryid)
        {
            var sqlTxt = "";
            if (classid == 0)
            {
                sqlTxt = @"insert b2b_com_class (classname,industryid) values(@classname,@industryid) ";
            }
            else
            {
                sqlTxt = @"update b2b_com_class set classname=@classname,industryid=@industryid where id=@classid ";
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@classid", classid);
            cmd.AddParam("@classname", classname);
            cmd.AddParam("@industryid", industryid);

            return cmd.ExecuteNonQuery();
        }


        internal int Proclassdel(int classid)
        {
            var sqlTxt = @"delete b2b_com_class where id=@classid ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@classid", classid);
            return cmd.ExecuteNonQuery();
        }



        internal string GetLowerPriceByProjectId(int projectid)
        {
            var sqltxt = @"select  MIN(advise_price) as minprice  from b2b_com_pro where projectid=" + projectid + " and pro_state=1 and advise_price !=0 and viewmethod !=2 and pro_end>'" + DateTime.Now + "'";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                string minprice = o == null ? "0" : o.ToString();
                if (minprice.IndexOf('.') > -1)
                {
                    minprice = minprice.Substring(0, o.ToString().IndexOf('.', 0));
                }
                return minprice == "" ? "0" : minprice;
            }
            catch
            {
                sqlHelper.Dispose();
                return "0";
            }
        }
        /// <summary>
        /// 修改公司下产品的项目id
        /// </summary>
        /// <param name="comid"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        internal int UpProjectId(string comid, int projectid)
        {
            string sql = "update b2b_com_pro set projectid=" + projectid + " where com_id=" + comid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();

        }

        internal List<B2b_com_pro> GetHouseTypePageList(int pageindex, int pagesize, int comid, out int totalcount, int proid = 0, int projectid = 0, int paichuproid = 0)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            string condition = "server_type=9 and com_id=" + comid + " and pro_state=1";

            if (projectid > 0)
            {
                condition += " and projectid=" + projectid;
            }
            if (paichuproid > 0)
            {
                condition += " and id !=" + paichuproid;
            }

            if (pagesize == 0)//显示全部
            {
                pagesize = 100000;
            }
            else
            {
                if (proid > 0)
                {
                    condition += " and id=" + proid;
                }
            }

            cmd.PagingCommand1("b2b_com_pro", "*", "id", "", pagesize, pageindex, "", condition);

            List<B2b_com_pro> list = new List<B2b_com_pro>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_com_pro
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Pro_name = reader.GetValue<string>("pro_name"),
                        Pro_state = reader.GetValue<int>("pro_state"),
                        Server_type = reader.GetValue<int>("server_type"),
                        Pro_type = reader.GetValue<int>("pro_type"),
                        Source_type = reader.GetValue<int>("Source_type"),

                        Projectid = reader.GetValue<int>("Projectid"),
                        Imgurl = reader.GetValue<int>("imgurl"),

                        Housetype = new B2b_com_housetypeData().GetHouseType(reader.GetValue<int>("id"), comid)

                    });

                }

            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }

        internal int ReduceLimittotalnum(int proid, int ordernum)
        {
            string sql = "update b2b_com_pro set limitbuytotalnum=limitbuytotalnum-" + ordernum + ",buynum=buynum+" + ordernum + "  where id=" + proid;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }
        //扣减库存
        internal int ReduceLimittotalSpeciidnum(int proid, int Speciid, int ordernum)
        {
            string sql = "update b2b_com_pro_Speci set speci_totalnum=speci_totalnum-" + ordernum + "  where id=" + Speciid + " and proid=" + proid;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal bool Ispanicbuypro(int proid)
        {
            string sql = "select ispanicbuy from b2b_com_pro  where id=" + proid;

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                if (o == null)
                {
                    return false;
                }
                else
                {
                    if (o.ToString() == "")
                    {
                        return false;
                    }
                    else
                    {
                        if (o.ToString() == "1")
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
            catch (Exception e)
            {
                sqlHelper.Dispose();
                return false;
            }

        }
        /// <summary>
        ///   作废当前产品超时未支付订单，并且完成回滚操作
        /// </summary>
        /// <param name="proid"></param>
        /// <param name="servertype"></param>
        /// <returns></returns>
        internal int CancelOvertimeOrder(B2b_com_pro pro)
        {
            try
            {
                sqlHelper.BeginTrancation();
                int result = 0;

                #region  票务产品 实物产品,超时订单，回滚操作(可销售数量增加;已销售数量减少)
                if (pro.Server_type == 1 || pro.Server_type == 11)
                {
                    #region  抢购/限购产品超时订单 回滚库存
                    if (pro.Ispanicbuy == 1 || pro.Ispanicbuy == 2)
                    {
                        string sql1 = "select sum(u_num) from b2b_order where order_state=1 and pay_state=1 and u_subdate<'" + DateTime.Now.AddMinutes(-30) + "' and pro_id=" + pro.Id;
                        var cmd = sqlHelper.PrepareTextSqlCommand(sql1);
                        object o = cmd.ExecuteScalar();
                        int total = int.Parse(o.ToString());

                        string sql2 = "update b2b_com_pro set limitbuytotalnum=limitbuytotalnum+" + total + ",buynum=buynum-" + total + " where id=" + pro.Id;
                        cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                        cmd.ExecuteNonQuery();

                        string sql3 = "update b2b_order set order_state=" + (int)OrderStatus.Timeout + " where order_state=1 and pay_state=1 and u_subdate<'" + DateTime.Now.AddMinutes(-30) + "' and pro_id=" + pro.Id;
                        cmd = sqlHelper.PrepareTextSqlCommand(sql3);
                        cmd.ExecuteNonQuery();
                    }
                    #endregion
                    #region 多规格产品超时订单 回滚库存
                    if (pro.Manyspeci == 1)
                    {
                        string sql1 = "select sum(u_num) as sumnum,Speciid from b2b_order where order_state=1 and pay_state=1 and u_subdate<'" + DateTime.Now.AddDays(-1) + "' and pro_id=" + pro.Id + " group by Speciid";
                        var cmd = sqlHelper.PrepareTextSqlCommand(sql1);
                        Hashtable ht = new Hashtable();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // 添加keyvalue键值对
                                ht.Add(reader.GetValue<int>("sumnum"), reader.GetValue<int>("Speciid"));
                            }
                        }

                        foreach (DictionaryEntry de in ht)
                        {
                            //Console.WriteLine("Key -- {0}; Value --{1}.", de.Key, de.Value);   
                            string sql2 = "update b2b_com_pro_Speci set speci_totalnum=speci_totalnum+" + de.Key + "  where id=" + de.Value + " and proid=" + pro.Id;
                            cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                            return cmd.ExecuteNonQuery();
                        }

                        string sql3 = "update b2b_order set order_state=" + (int)OrderStatus.Timeout + " where order_state=1 and pay_state=1 and u_subdate<'" + DateTime.Now.AddDays(-1) + "' and pro_id=" + pro.Id;
                        cmd = sqlHelper.PrepareTextSqlCommand(sql3);
                        cmd.ExecuteNonQuery();
                    }
                    #endregion
                    result = 1;
                }
                #endregion
                #region  旅游大巴 超时订单 班车座位数回滚
                else if (pro.Server_type == 10)
                {
                    //得到超时订单列表
                    string sql = "select  id,u_traveldate,u_num from b2b_order where order_state=1 and pay_state=1 and u_subdate<'" + DateTime.Now.AddMinutes(-60) + "' and pro_id=" + pro.Id;
                    var cmd = sqlHelper.PrepareTextSqlCommand(sql);

                    List<int> list_orderid = new List<int>();
                    List<DateTime> list_traveldate = new List<DateTime>();
                    List<int> list_booknum = new List<int>();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list_orderid.Add(reader.GetValue<int>("id"));
                            list_traveldate.Add(reader.GetValue<DateTime>("u_traveldate"));
                            list_booknum.Add(reader.GetValue<int>("u_num"));
                        }
                    }

                    if (list_orderid.Count > 0)
                    {
                        //超时订单(并且没有截团) 班车座位数回滚 
                        for (int i = 0; i < list_orderid.Count; i++)
                        {
                            //现在截团操作已经不存在，所以sql1不用执行
                            //string sql1 = "select count(1) from travelbusorder_operlog where proid=" + pro.Id + " and traveldate='" + list_traveldate[i] + "'";
                            //cmd = sqlHelper.PrepareTextSqlCommand(sql1);
                            //object o = cmd.ExecuteScalar(); 

                            //if (int.Parse(o.ToString()) == 0)//没有截团，进行回滚
                            //{  
                            string sql2 = "update B2b_com_LineGroupDate set emptynum=emptynum+" + list_booknum[i] + "  where lineid=" + pro.Id + " and daydate='" + list_traveldate[i] + "'";
                            cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                            cmd.ExecuteNonQuery();
                            //}

                            //订单状态改为超时订单
                            string sql3 = "update b2b_order set order_state=" + (int)OrderStatus.Timeout + " where      id=" + list_orderid[i];
                            cmd = sqlHelper.PrepareTextSqlCommand(sql3);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    result = 1;
                }
                #endregion
                #region  旅游产品 超时订单  团期回滚
                else if (pro.Server_type == 2 || pro.Server_type == 8)
                {
                    //得到超时订单列表
                    string sql = "select  id,u_traveldate,u_num from b2b_order where order_state=1 and pay_state=1 and u_subdate<'" + DateTime.Now.AddDays(-1) + "' and pro_id=" + pro.Id;
                    var cmd = sqlHelper.PrepareTextSqlCommand(sql);

                    List<int> list_orderid = new List<int>();
                    List<DateTime> list_traveldate = new List<DateTime>();
                    List<int> list_booknum = new List<int>();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list_orderid.Add(reader.GetValue<int>("id"));
                            list_traveldate.Add(reader.GetValue<DateTime>("u_traveldate"));
                            list_booknum.Add(reader.GetValue<int>("u_num"));
                        }
                    }

                    if (list_orderid.Count > 0)
                    {
                        //超时订单 座位数回滚 
                        for (int i = 0; i < list_orderid.Count; i++)
                        {
                            string sql2 = "update B2b_com_LineGroupDate set emptynum=emptynum+" + list_booknum[i] + "  where lineid=" + pro.Id + " and daydate='" + list_traveldate[i] + "'";
                            cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                            cmd.ExecuteNonQuery();


                            //订单状态改为超时订单
                            string sql3 = "update b2b_order set order_state=" + (int)OrderStatus.Timeout + " where      id=" + list_orderid[i];
                            cmd = sqlHelper.PrepareTextSqlCommand(sql3);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    result = 1;
                }
                #endregion
                #region 其他产品 对超过24小时的所有订单 订单状态都改为 超时订单
                else
                {
                    //对所有订单超过24小时的都做 超时订单处理
                    string sqlall = "update b2b_order set order_state=" + (int)OrderStatus.Timeout + " where order_state=1 and pay_state=1 and u_subdate<'" + DateTime.Now.AddDays(-1) + "'";
                    var cmdall = sqlHelper.PrepareTextSqlCommand(sqlall);
                    cmdall.ExecuteNonQuery();

                    result = 0;
                }
                #endregion

                sqlHelper.Commit();
                return result;
            }
            catch (Exception e)
            {
                sqlHelper.Rollback();
                return 0;
            }
            finally
            {
                sqlHelper.Dispose();
            }

        }

        /// <summary>
        ///  对单独订单进行库存回滚
        /// </summary>
        /// <param name="proid"></param>
        /// <param name="servertype"></param>
        /// <returns></returns>
        internal int BackOrdernum(int proid, int num)
        {
            try
            {
                // 预订的产品数量
                string sql2 = "update b2b_com_pro set limitbuytotalnum=limitbuytotalnum+" + num + ",buynum=buynum-" + num + " where id=" + proid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                return cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                sqlHelper.Rollback();
                sqlHelper.Dispose();
                return 0;
            }

        }


        /// <summary>
        ///  对规格库存回滚
        /// </summary>
        /// <param name="proid"></param>
        /// <param name="servertype"></param>
        /// <returns></returns>
        internal int BackSpeciOrdernum(int proid, int Speciid, int num)
        {
            try
            {
                // 预订的产品数量
                string sql2 = "update b2b_com_pro_Speci set speci_totalnum=speci_totalnum+" + num + " where id=" + Speciid + " and proid=" + proid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                return cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                sqlHelper.Rollback();
                sqlHelper.Dispose();
                return 0;
            }

        }

        /// <summary>
        /// 判断产品是否有效：1.票务，直接判断有效期 和产品上线状态2.酒店，跟团游，当地游，旅游大巴  则判断是否含有有效的房态/团期 以及产品上下线状态
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        /// <param name="dateTime"></param>
        /// <param name="dateTime_2"></param>
        /// <returns></returns>
        internal int IsYouxiao(int proid, int Server_type, DateTime Pro_start, DateTime Pro_end, int pro_state, string outdate = "", int ordernum = 0)
        {
            if (pro_state == 0)
            {
                return 0;
            }
            else
            {

                if (Server_type == 1 || Server_type == 11 || Server_type == 14)
                {
                    string sql = "select count(1) from b2b_com_pro where id=" + proid + " and '" + Pro_end.ToString("yyyy-MM-dd") + "'>= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";

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
                else if (Server_type == 2 || Server_type == 8 || Server_type == 9 || Server_type == 10)
                {
                    string sql = "select count(1) from B2b_com_LineGroupDate  where  lineid =" + proid + " and emptynum>0  and     daydate  >='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                    if (outdate != "")//出行日期
                    {
                        try
                        {
                            DateTime outtime = DateTime.Parse(outdate);
                            sql += " and Convert(varchar(10), daydate,120)='" + outtime.ToString("yyyy-MM-dd") + "'";
                        }
                        catch
                        { }
                    }
                    if (ordernum != 0)
                    {
                        sql += " and  emptynum>=" + ordernum;

                    }
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
                else
                {
                    return 0;
                }
            }
        }

        internal B2b_com_class GetB2bcomclass(int id)
        {
            string sql = "select * from b2b_com_class where id=" + id;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                B2b_com_class m = null;
                if (reader.Read())
                {
                    m = new B2b_com_class
                    {
                        Id = reader.GetValue<int>("id"),
                        Classname = reader.GetValue<string>("classname"),
                        Industryid = reader.GetValue<int>("industryid")
                    };
                }
                return m;
            }
        }


        internal string GetPro_Youxiaoqi(DateTime prostart, DateTime proend, string provalidatemethod, int appointdate, int iscanuseonsameday)
        {

            DateTime pro_end;
            DateTime pro_start;
            string pro_youxiaoqi = "";


            pro_end = proend;
            if (provalidatemethod == "2")//按指定有效期
            {
                if (appointdate == (int)ProAppointdata.NotAppoint)
                {
                }
                else if (appointdate == (int)ProAppointdata.OneWeek)
                {
                    pro_end = DateTime.Now.AddDays(7);
                    if (pro_end < proend)
                    {
                        //pro_youxiaoqi = "出票后一周有效";
                        pro_youxiaoqi = pro_end.ToString("yyyy.MM.dd");
                    }
                    else
                    {
                        pro_youxiaoqi = proend.ToString("yyyy.MM.dd");
                    }


                }
                else if (appointdate == (int)ProAppointdata.OneMonth)
                {
                    pro_end = DateTime.Now.AddMonths(1);

                    if (pro_end < proend)
                    {
                        pro_youxiaoqi = "出票后一月有效";
                        pro_youxiaoqi = pro_end.ToString("yyyy.MM.dd");
                    }
                    else
                    {
                        pro_youxiaoqi = proend.ToString("yyyy.MM.dd");
                    }
                }
                else if (appointdate == (int)ProAppointdata.ThreeMonth)
                {
                    pro_end = DateTime.Now.AddMonths(3);

                    if (pro_end < proend)
                    {
                        pro_youxiaoqi = "出票后三月有效";
                        pro_youxiaoqi = pro_end.ToString("yyyy.MM.dd");
                    }
                    else
                    {
                        pro_youxiaoqi = proend.ToString("yyyy.MM.dd");
                    }
                }
                else if (appointdate == (int)ProAppointdata.SixMonth)
                {
                    pro_end = DateTime.Now.AddMonths(6);

                    if (pro_end < proend)
                    {
                        pro_youxiaoqi = "出票后半年有效";
                        pro_youxiaoqi = pro_end.ToString("yyyy.MM.dd");
                    }
                    else
                    {
                        pro_youxiaoqi = proend.ToString("yyyy.MM.dd");
                    }
                }
                else if (appointdate == (int)ProAppointdata.OneYear)
                {
                    pro_end = DateTime.Now.AddYears(1);

                    if (pro_end < proend)
                    {
                        pro_youxiaoqi = "出票后一年有效";
                        pro_youxiaoqi = pro_end.ToString("yyyy.MM.dd");
                    }
                    else
                    {
                        pro_youxiaoqi = proend.ToString("yyyy.MM.dd");
                    }
                }
            }
            else //按产品有效期
            {
                pro_end = proend;
                pro_youxiaoqi = proend.ToString("yyyy.MM.dd");
            }

            pro_start = prostart;
            DateTime nowday = DateTime.Parse(DateTime.Now.ToString("yyyy.MM.dd"));
            if (iscanuseonsameday == 1)//当天可用  
            {
                if (nowday < pro_start)//当天日期小于产品起始日期
                {
                    pro_start = prostart;
                    pro_youxiaoqi = prostart.ToString("yyyy.MM.dd") + " - " + pro_youxiaoqi;
                }

            }
            else //当天不可用
            {
                if (nowday < pro_start)//当天日期小于产品起始日期
                {
                    pro_start = prostart;
                    pro_youxiaoqi = prostart.ToString("yyyy.MM.dd") + " - " + pro_youxiaoqi;
                }
                else
                {
                    pro_start = nowday.AddDays(1);
                    pro_youxiaoqi = pro_start.ToString("yyyy.MM.dd") + " - " + pro_youxiaoqi;

                }
            }

            return pro_youxiaoqi;

        }




        internal IList<B2b_com_pro> Getb2bcomprobytraveldate(DateTime daydate, int servertype, int comid, string isSetVisitDate = "0,1")
        {
            if (isSetVisitDate != "0,1")
            {
                if (isSetVisitDate == "1")
                {
                    string sql = " select id,pro_name from b2b_com_pro where isSetVisitDate=" + isSetVisitDate + " and id in (select pro_id from b2b_order where u_traveldate ='" + daydate + "')  and server_type=" + servertype + "   and com_id=" + comid;

                    var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                    using (var reader = cmd.ExecuteReader())
                    {
                        List<B2b_com_pro> list = new List<B2b_com_pro>();
                        while (reader.Read())
                        {
                            list.Add(new B2b_com_pro
                            {
                                Id = reader.GetValue<int>("id"),
                                Pro_name = reader.GetValue<string>("pro_name")
                            });
                        }
                        return list;
                    }
                }
                else
                {
                    string sql = " select id,pro_name from b2b_com_pro where isSetVisitDate=" + isSetVisitDate + "   and server_type=" + servertype + "   and com_id=" + comid;

                    var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                    using (var reader = cmd.ExecuteReader())
                    {
                        List<B2b_com_pro> list = new List<B2b_com_pro>();
                        while (reader.Read())
                        {
                            list.Add(new B2b_com_pro
                            {
                                Id = reader.GetValue<int>("id"),
                                Pro_name = reader.GetValue<string>("pro_name")
                            });
                        }
                        return list;
                    }
                }

            }
            else
            {
                string sql = " select id,pro_name from b2b_com_pro where id in (select lineid from B2b_com_LineGroupDate where daydate='" + daydate + "') and server_type=" + servertype + "   and com_id=" + comid;

                sql += " order by firststationtime";


                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    List<B2b_com_pro> list = new List<B2b_com_pro>();
                    while (reader.Read())
                    {
                        list.Add(new B2b_com_pro
                        {
                            Id = reader.GetValue<int>("id"),
                            Pro_name = reader.GetValue<string>("pro_name")
                        });
                    }
                    return list;
                }
            }
        }
        internal int IsHasLvyoubusPro(int comid, int servertype)
        {
            string sql = "select count(1) from b2b_com_pro where server_type=" + servertype + "  and com_id=" + comid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            int r = o == null ? 0 : int.Parse(o.ToString());
            if (r > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        internal int GetOldproidById(string product_num)
        {
            try
            {
                string sql = "select bindingid from b2b_com_pro where id=" + product_num;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null ? 0 : int.Parse(o.ToString());
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }

        internal int ProAutoDownLine()
        {
            string sql = "update b2b_com_pro set pro_state=0 where pro_state=1 and  pro_end<'" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal B2b_com_pro Getprobypno(string pno)
        {
            const string sqltxt = @"SELECT   [id]
      ,[com_id]
      ,[pro_name]
      ,[pro_state]
      ,[server_type]
      ,[pro_type]
      ,[source_type]
      ,[pro_Remark]
      ,[pro_start]
      ,[pro_end]
      ,[face_price]
      ,[advise_price]
      ,[Agent1_price]
,[Agent2_price]
,[Agent3_price]
      ,[agentsettle_price]
      ,[ThatDay_can]
      ,[Thatday_can_day]
      ,[service_Contain]
      ,[service_NotContain]
      ,[Precautions]
      ,[tuan_pro]
      ,[zhixiao]
      ,[agentsale]
      ,[createuserid]
      ,[createtime]
,[sms]
,[imgurl]
,[pro_number]
,[pro_explain]
,[pro_Integral]
,[TuiPiao]
,[TuiPiao_Guoqi]
,[TuiPiao_Endday]
,serviceid
,service_proid
,realnametype
,projectid
,trval_productid
,travel_type
,trval_starting
,ispanicbuy 
,panicbuybegin_time
,panicbuyenddate_time
,limitbuytotalnum
,linepro_booktype
,ProValidateMethod
,appointdata
,iscanuseonsameday
,viewmethod
,childreduce
,Bindingid
,pickuppoint
,dropoffpoint
,pro_note
,QuitTicketMechanism
,daybespeaknum,
 isneedbespeak,
 bespeaksucmsg,
 bespeakfailmsg,
 customservicephone,
 isblackoutdate,
 etickettype,
 buynum 
  FROM [EtownDB].[dbo].[b2b_com_pro] where [Id] in (select pro_id from b2b_order where pno=@pno and pno!='') ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@pno", pno);



            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_com_pro
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Pro_name = reader.GetValue<string>("pro_name"),
                        Pro_state = reader.GetValue<int>("pro_state"),
                        Server_type = reader.GetValue<int>("server_type"),
                        Pro_type = reader.GetValue<int>("pro_type"),
                        Source_type = reader.GetValue<int>("source_type"),
                        Pro_Remark = reader.GetValue<string>("pro_Remark"),
                        Pro_start = reader.GetValue<DateTime>("pro_start"),
                        Pro_end = reader.GetValue<DateTime>("pro_end"),
                        Face_price = reader.GetValue<decimal>("face_price"),
                        Advise_price = reader.GetValue<decimal>("advise_price"),
                        Agent1_price = reader.GetValue<decimal>("Agent1_price"),
                        Agent2_price = reader.GetValue<decimal>("Agent2_price"),
                        Agent3_price = reader.GetValue<decimal>("Agent3_price"),
                        Agentsettle_price = reader.GetValue<decimal>("agentsettle_price"),
                        ThatDay_can = reader.GetValue<int>("ThatDay_can"),
                        Thatday_can_day = reader.GetValue<int>("Thatday_can_day"),
                        Service_Contain = reader.GetValue<string>("service_Contain"),
                        Service_NotContain = reader.GetValue<string>("service_NotContain"),
                        Precautions = reader.GetValue<string>("Precautions"),
                        Tuan_pro = reader.GetValue<int>("tuan_pro"),
                        Zhixiao = reader.GetValue<int>("zhixiao"),
                        Agentsale = reader.GetValue<int>("agentsale"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Sms = reader.GetValue<string>("Sms"),
                        Createuserid = reader.GetValue<int>("createuserid"),
                        Imgurl = reader.GetValue<int>("imgurl"),
                        Pro_number = reader.GetValue<int>("pro_number"),
                        Pro_explain = reader.GetValue<string>("pro_explain"),
                        Pro_Integral = reader.GetValue<decimal>("pro_Integral"),
                        Tuipiao = reader.GetValue<int>("TuiPiao"),
                        Tuipiao_guoqi = reader.GetValue<int>("TuiPiao_Guoqi"),
                        Tuipiao_endday = reader.GetValue<int>("TuiPiao_Endday"),
                        Serviceid = reader.GetValue<int>("serviceid"),
                        Service_proid = reader.GetValue<string>("service_proid"),
                        Realnametype = reader.GetValue<int>("realnametype"),
                        Projectid = reader.GetValue<int>("projectid"),

                        Travelproductid = reader.GetValue<int>("trval_productid"),
                        Traveltype = reader.GetValue<string>("travel_type").ConvertTo<int>(0),
                        Travelstarting = reader.GetValue<string>("trval_starting"),

                        Ispanicbuy = reader.GetValue<int>("ispanicbuy"),
                        Panic_begintime = reader.GetValue<DateTime>("panicbuybegin_time"),
                        Panicbuy_endtime = reader.GetValue<DateTime>("panicbuyenddate_time"),
                        Limitbuytotalnum = reader.GetValue<int>("limitbuytotalnum"),
                        Linepro_booktype = reader.GetValue<int>("linepro_booktype"),
                        ProValidateMethod = reader.GetValue<string>("ProValidateMethod"),
                        Appointdata = reader.GetValue<int>("appointdata"),
                        Iscanuseonsameday = reader.GetValue<int>("iscanuseonsameday"),
                        Viewmethod = reader.GetValue<int>("viewmethod"),
                        Childreduce = reader.GetValue<decimal>("childreduce"),
                        Bindingid = reader.GetValue<int>("Bindingid"),
                        pickuppoint = reader.GetValue<string>("pickuppoint"),
                        dropoffpoint = reader.GetValue<string>("dropoffpoint"),
                        pro_note = reader.GetValue<string>("pro_note"),
                        QuitTicketMechanism = reader.GetValue<int>("QuitTicketMechanism"),

                        daybespeaknum = reader.GetValue<int>("daybespeaknum"),
                        isneedbespeak = reader.GetValue<int>("isneedbespeak"),
                        bespeaksucmsg = reader.GetValue<string>("bespeaksucmsg"),
                        bespeakfailmsg = reader.GetValue<string>("bespeakfailmsg"),
                        customservicephone = reader.GetValue<string>("customservicephone"),
                        isblackoutdate = reader.GetValue<int>("isblackoutdate"),
                        etickettype = reader.GetValue<int>("etickettype"),
                        buynum = reader.GetValue<int>("buynum"),
                    };
                }
                return null;
            }
        }

        internal int Getordercanbooknumbypno(string pno)
        {
            try
            {
                //查询退票数量
                string sql = "select cancel_ticketnum from b2b_order where pno='" + pno + "' and pno!=''";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                int cancelnum = int.Parse(o.ToString());

                if (cancelnum > 0)
                {
                    return 0;
                }
                else
                {
                    //查询电子票可用数量
                    string sql2 = "select  use_pnum from b2b_eticket  where pno='" + pno + "' and pno!=''";
                    cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                    object oo = cmd.ExecuteScalar();
                    sqlHelper.Dispose();
                    int canbooknum = int.Parse(oo.ToString());
                    return canbooknum;
                }
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }

        internal int GetOrderIdByPno(string pno)
        {
            try
            {
                string sql = "select id from b2b_order where pno='" + pno + "'";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return int.Parse(o.ToString());
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }

        internal int AddLimittotalnum(int proid, int ordernum)
        {
            string sql = "update b2b_com_pro set limitbuytotalnum=limitbuytotalnum+" + ordernum + ",buynum=buynum-" + ordernum + "  where id=" + proid;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int GetbindingidbyProid(int proid)
        {
            string sql = "select bindingid from b2b_com_pro   where id=" + proid;
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return int.Parse(o.ToString());
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }

        internal void GetbindingidbyProid(int proid, out int limitbuytotalnum, out int buynum)
        {
            string sql = "select limitbuytotalnum ,buynum from b2b_com_pro where id=" + proid;
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        limitbuytotalnum = reader.GetValue<int>("limitbuytotalnum");
                        buynum = reader.GetValue<int>("buynum");
                    }
                    else
                    {
                        limitbuytotalnum = 0;
                        buynum = 0;
                    }
                }
            }
            catch
            {
                sqlHelper.Dispose();
                limitbuytotalnum = 0;
                buynum = 0;
            }
        }

        internal int Uplimitbuytotalnum(int proid, int upnum)
        {
            string sql = "update b2b_com_pro set limitbuytotalnum=limitbuytotalnum+" + upnum + "  where id=" + proid;
            if (upnum < 0)
            {
                sql = "update b2b_com_pro set limitbuytotalnum=limitbuytotalnum-abs(" + upnum + ")  where id=" + proid;

            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int EditProChildImg(int proid, string MultiImgUpIds)
        {
            try
            {
                sqlHelper.BeginTrancation();

                string sql1 = "delete b2b_com_pro_childimg where proid=" + proid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql1);
                cmd.ExecuteNonQuery();

                string[] fileuploadidarr = MultiImgUpIds.Split(',');
                if (fileuploadidarr.Length > 0)
                {
                    for (int i = 0; i < fileuploadidarr.Length; i++)
                    {
                        if (fileuploadidarr[i] != "")
                        {
                            string sql2 = "insert into b2b_com_pro_childimg (proid,fileuploadid) values(" + proid + "," + fileuploadidarr[i] + ")";
                            var cmd2 = sqlHelper.PrepareTextSqlCommand(sql2);
                            cmd2.ExecuteNonQuery();
                        }
                    }
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

        internal int DelProChildImg(int fileUploadId)
        {
            string sql = "delete b2b_com_pro_childimg where fileuploadid =" + fileUploadId;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }


        internal List<B2b_com_pro> WebProPageList(string comid, int pageindex, int pagesize, int prostate, out int totalcount, int projectid = 0, string key = "", string viewmethod = "")
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var tblName = "b2b_com_pro";
            var strGetFields = "*";
            var sortKey = "sortid,id desc";
            //var sortMode = "0";
            var condition = "com_id=" + comid + " and server_type in (1,9,10)";
            if (projectid > 0)
            {
                condition += " and projectid=" + projectid;
            }

            if (prostate == 1)//分销或其他地方用到读取上线并且在有效期内产品
            {
                condition += " and pro_state = 1" + " and pro_end>='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            }
            if (prostate == 2)//读取上线产品
            {
                condition += " and pro_state = 1";
            }
            if (prostate == 3)//读取下线产品
            {
                condition += " and pro_state = 0";
            }



            if (key != "")
            {
                condition += " and pro_name like '%" + key + "%'";
            }
            //产品新增加显示设置  1.全部2分销3微信4.官网5.微信和官网;商家后台显示全部的
            if (viewmethod != "")
            {
                condition += " and viewmethod in (" + viewmethod + ")";
            }

            //cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);
            cmd.PagingCommand1(tblName, strGetFields, sortKey, "", pagesize, pageindex, "0", condition);

            List<B2b_com_pro> list = new List<B2b_com_pro>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_com_pro
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Pro_name = reader.GetValue<string>("pro_name"),
                        Pro_state = reader.GetValue<int>("pro_state"),
                        Server_type = reader.GetValue<int>("server_type"),
                        Pro_type = reader.GetValue<int>("pro_type"),
                        Source_type = reader.GetValue<int>("Source_type"),
                        Pro_Remark = reader.GetValue<string>("pro_Remark"),
                        Pro_start = reader.GetValue<DateTime>("pro_start"),
                        Pro_end = reader.GetValue<DateTime>("pro_end"),
                        Face_price = reader.GetValue<decimal>("face_price"),
                        Advise_price = reader.GetValue<decimal>("advise_price"),
                        Agent1_price = reader.GetValue<decimal>("Agent1_price"),
                        Agent2_price = reader.GetValue<decimal>("Agent2_price"),
                        Agent3_price = reader.GetValue<decimal>("Agent3_price"),
                        Agentsettle_price = reader.GetValue<decimal>("agentsettle_price"),
                        ThatDay_can = reader.GetValue<int>("ThatDay_can"),
                        Thatday_can_day = reader.GetValue<int>("Thatday_can_day"),
                        Service_Contain = reader.GetValue<string>("service_Contain"),
                        Service_NotContain = reader.GetValue<string>("service_NotContain"),
                        Precautions = reader.GetValue<string>("Precautions"),
                        Tuan_pro = reader.GetValue<int>("tuan_pro"),
                        Zhixiao = reader.GetValue<int>("zhixiao"),
                        Agentsale = reader.GetValue<int>("agentsale"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Createuserid = reader.GetValue<int>("createuserid"),

                        Imgurl = reader.GetValue<int>("imgurl"),
                        Tuipiao = reader.GetValue<int>("TuiPiao"),
                        Tuipiao_guoqi = reader.GetValue<int>("TuiPiao_Guoqi"),
                        Tuipiao_endday = reader.GetValue<int>("TuiPiao_Endday"),
                        Projectid = reader.GetValue<int>("projectid"),
                        Bindingid = reader.GetValue<int>("Bindingid"),
                        Viewmethod = reader.GetValue<int>("Viewmethod"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }

        internal int GetImgUrl(int proid)
        {
            string sql = "select imgurl from b2b_com_pro where id=" + proid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            int imgurl = 0;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    imgurl = reader.GetValue<int>("imgurl");
                }
            }
            return imgurl;
        }

        internal string GetLinePro_BookType(int proid)
        {
            string sql = "select linepro_booktype from b2b_com_pro where id=" + proid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            int r = 1;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    r = reader.GetValue<int>("linepro_booktype");
                }
            }
            return r.ToString();
        }

        internal int GetFirstProImgInProjectId(int projectid)
        {
            string sql = "select top 1 imgurl from b2b_com_pro where projectid=" + projectid + " and pro_state=1 and imgurl>0 order by id desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                int imgurl = 0;
                if (reader.Read())
                {
                    imgurl = reader.GetValue<int>("imgurl");
                }
                return imgurl;
            }
        }

        internal string GetProName(int proid)
        {
            string sql = "select pro_name from b2b_com_pro where id=" + proid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                string proname = "";
                if (reader.Read())
                {
                    proname = reader.GetValue<string>("pro_name");
                }
                return proname;
            }
        }
        //通过产品ID获取原始商户名称
        internal string GetComNamebyproid(int proid)
        {
            string sql = "select * from b2b_company where id in(select com_id from b2b_com_pro where id=" + proid + ")";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                string proname = "";
                if (reader.Read())
                {
                    proname = reader.GetValue<string>("com_name");
                }
                return proname;
            }
        }

        //通过产品ID获取产品类型
        internal int GetSourcetypebyproid(int proid)
        {
            string sql = "select * from b2b_com_pro where id=" + proid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                int Sourcetype = 0;
                if (reader.Read())
                {
                    Sourcetype = reader.GetValue<int>("Source_type");
                }
                return Sourcetype;
            }
        }

        internal IList<B2b_com_pro> GetProlistbyprojectid(int projectid, string servertypes, int topnums = 10)
        {
            string sql = "select top " + topnums + " * from b2b_com_pro where projectid=" + projectid + " and server_type in (" + servertypes + ") and pro_state=1";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);


            using (var reader = cmd.ExecuteReader())
            {
                IList<B2b_com_pro> list = new List<B2b_com_pro>();
                while (reader.Read())
                {
                    list.Add(new B2b_com_pro
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Pro_name = reader.GetValue<string>("pro_name"),
                        Pro_state = reader.GetValue<int>("pro_state"),
                        Server_type = reader.GetValue<int>("server_type"),
                        Pro_type = reader.GetValue<int>("pro_type"),
                        Source_type = reader.GetValue<int>("Source_type"),
                        Pro_Remark = reader.GetValue<string>("pro_Remark"),
                        Pro_start = reader.GetValue<DateTime>("pro_start"),
                        Pro_end = reader.GetValue<DateTime>("pro_end"),
                        Face_price = reader.GetValue<decimal>("face_price"),
                        Advise_price = reader.GetValue<decimal>("advise_price"),
                        Agentsettle_price = reader.GetValue<decimal>("agentsettle_price"),
                        ThatDay_can = reader.GetValue<int>("ThatDay_can"),
                        Thatday_can_day = reader.GetValue<int>("Thatday_can_day"),
                        Service_Contain = reader.GetValue<string>("service_Contain"),
                        Service_NotContain = reader.GetValue<string>("service_NotContain"),
                        Precautions = reader.GetValue<string>("Precautions"),
                        Tuan_pro = reader.GetValue<int>("tuan_pro"),
                        Zhixiao = reader.GetValue<int>("zhixiao"),
                        Agentsale = reader.GetValue<int>("agentsale"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Createuserid = reader.GetValue<int>("createuserid"),
                        Pro_explain = reader.GetValue<string>("pro_explain"),
                        Ispanicbuy = reader.GetValue<int>("Ispanicbuy"),
                        Limitbuytotalnum = reader.GetValue<int>("Limitbuytotalnum"),
                        ishasdeliveryfee = reader.GetValue<int>("ishasdeliveryfee"),
                        Imgurl = reader.GetValue<int>("imgurl"),
                        Pro_Integral = reader.GetValue<decimal>("Pro_Integral"),
                        Projectid = reader.GetValue<int>("projectid"),
                        Bindingid = reader.GetValue<int>("Bindingid"),
                        Imgaddress = FileSerivce.GetImgUrl(reader.GetValue<int>("imgurl")),
                    });
                }
                return list;
            }
        }


        internal int GetServiceidbyproid(int proid)
        {
            string sql = "select serviceid from b2b_com_pro where id=" + proid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("serviceid");
                }
                return 0;
            }
        }



        internal string Getcoachnamebyid(int channelcoachid)
        {
            string guigename = "";
            if (channelcoachid == 0)
            {
                return "";
            }

            string sql = "select  *  from member_channel where id=" + channelcoachid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    guigename = " 教练:" + reader.GetValue<string>("name");
                }
            }
            return guigename;
        }

        internal int GetIsHzinsPro(int proid)
        {
            string sql = "select serviceid from b2b_com_pro where id=" + proid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("serviceid");
                }
                return 0;
            }
        }

        internal int GetSourcetypeByOrderid(int orderid)
        {
            string sql = "select source_type from b2b_com_pro where id=(select pro_id from b2b_order where id=" + orderid + ")";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                int r = 0;
                if (reader.Read())
                {
                    r = reader.GetValue<int>("source_type");
                }
                return r;
            }
        }

        internal IList<B2b_com_pro> Getbaoxianlist(int comid)
        {
            string sql = "select id,pro_name from b2b_com_pro where com_id=" + comid + " and pro_state=1 and server_type=14";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                IList<B2b_com_pro> list = new List<B2b_com_pro>();
                while (reader.Read())
                {
                    list.Add(new B2b_com_pro
                    {
                        Id = reader.GetValue<int>("id"),
                        Pro_name = reader.GetValue<string>("pro_name")
                    });
                }
                return list;
            }
        }

        internal int GetSelbindbx(int proid)
        {
            string sql = "select selbindbx from b2b_com_pro where id=" + proid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                int r = 0;
                if (reader.Read())
                {
                    r = reader.GetValue<int>("selbindbx");
                }
                return r;
            }
        }

        internal int GetMeituanMaxPoiid()
        {
            try
            {
                var sql = "select   max(id) from b2b_com_project where   id in (select id from b2b_com_project where  onlinestate=1 and comid in (select comid  from agent_warrant where  agentid=(select id from agent_company where company='美团旅游')  and warrant_state=1))";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    int r = 0;
                    if (reader.Read())
                    {
                        r = reader.GetValue<int>("id");
                    }
                    return r;
                }
            }
            catch
            {
                return 0;
            }
        }

        internal int GetMeituanPoiNextIncrementId(int projectid)
        {
            try
            {
                var sql = "select top 1 id from b2b_com_project where id>" + projectid + " and id in (select id from b2b_com_project where  onlinestate=1 and comid in (select comid  from agent_warrant where  agentid=(select id from agent_company where company='美团旅游')  and warrant_state=1))";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    int r = 0;
                    if (reader.Read())
                    {
                        r = reader.GetValue<int>("id");
                    }
                    return r;
                }
            }
            catch
            {
                return 0;
            }
        }

        internal int GetMeituanMaxProid()
        {
            try
            {
                var sql = "select   max(id) from b2b_com_pro where   id in (select id from b2b_com_pro where  pro_state=1 and com_id in (select comid  from agent_warrant where  agentid=(select id from agent_company where company='美团旅游')  and warrant_state=1))";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    int r = 0;
                    if (reader.Read())
                    {
                        r = reader.GetValue<int>("id");
                    }
                    return r;
                }
            }
            catch
            {
                return 0;
            }
        }

        internal int GetMeituanProNextIncrementId(int proid)
        {
            try
            {
                var sql = "select top 1 id from b2b_com_pro where id>" + proid + " and id in (select id from b2b_com_pro where  pro_state=1 and com_id in (select comid  from agent_warrant where  agentid=(select id from agent_company where company='美团旅游')  and warrant_state=1))";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    int r = 0;
                    if (reader.Read())
                    {
                        r = reader.GetValue<int>("id");
                    }
                    return r;
                }
            }
            catch
            {
                return 0;
            }
        }

        internal List<B2b_com_pro> GetAgentProList(out int totalcount, int Agentid, string method, string productids, int pageindex, int pagesize)
        {
            #region 多点拉取
            if (method.Trim() == "multi")
            {
                if (productids == "")
                {
                    totalcount = 0;
                    return new List<B2b_com_pro>();
                }
                var proidarr = productids.Split(',');

                string proidstr = "";
                for (int i = 0; i < proidarr.Length; i++)
                {
                    if (proidarr[i] != "")
                    {
                        proidstr = proidarr[i] + ",";
                    }
                }
                proidstr = proidstr.Substring(0, proidstr.Length - 1);



                string sql = "select * from b2b_com_pro ";

                var condition = " where  server_type in (1,10) and ManySpeci=0 and  id in (" + proidstr + ") and pro_state=1 and convert(varchar(10),pro_end,120)>='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                //授权给分销的商户
                condition += " and  com_id in (select comid from agent_warrant where agentid =" + Agentid + " and warrant_state=1)";
                //产品不包含 已经授权给分销的商户下的没有设置分销价格的产品
                condition += " and not id in (" +
    " select id from b2b_com_pro where server_type not in (2,8) and agent1_price =0 and com_id in (select comid from agent_warrant where agentid =" + Agentid + " and warrant_level=1)" +
    " )" +
    " and not id in (" +
    " select id from b2b_com_pro where  server_type not in (2,8) and  agent2_price =0 and com_id in (select comid from agent_warrant where agentid =" + Agentid + " and warrant_level=2)" +
    " )" +
    " and not id in (" +
    " select id from b2b_com_pro where  server_type not in (2,8) and  agent3_price =0 and com_id in (select comid from agent_warrant where agentid =" + Agentid + " and warrant_level=3)" +
    " )";
                //产品不包含 授权给分销是验证扣款的商户下的外部接口产品(现在只有阳光)
                condition += " and not id in (select id from b2b_com_pro where source_type=3 and com_id in (select comid from agent_warrant where  agentid =" + Agentid + " and Warrant_type = 2))";

                sql += condition;

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                List<B2b_com_pro> list = new List<B2b_com_pro>();
                using (var reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        list.Add(new B2b_com_pro
                        {
                            ishasdeliveryfee = reader.GetValue<int>("ishasdeliveryfee"),
                            deliverytmp = reader.GetValue<int>("deliverytmp"),
                            Id = reader.GetValue<int>("id"),
                            Com_id = reader.GetValue<int>("com_id"),
                            Pro_name = reader.GetValue<string>("pro_name"),
                            Pro_state = reader.GetValue<int>("pro_state"),
                            Server_type = reader.GetValue<int>("server_type"),
                            Pro_type = reader.GetValue<int>("pro_type"),
                            Source_type = reader.GetValue<int>("source_type"),
                            Pro_Remark = reader.GetValue<string>("pro_Remark"),
                            Pro_start = reader.GetValue<DateTime>("pro_start"),
                            Pro_end = reader.GetValue<DateTime>("pro_end"),
                            Face_price = reader.GetValue<decimal>("face_price"),
                            Advise_price = reader.GetValue<decimal>("advise_price"),
                            Agent1_price = reader.GetValue<decimal>("Agent1_price"),
                            Agent2_price = reader.GetValue<decimal>("Agent2_price"),
                            Agent3_price = reader.GetValue<decimal>("Agent3_price"),
                            Agentsettle_price = reader.GetValue<decimal>("agentsettle_price"),
                            ThatDay_can = reader.GetValue<int>("ThatDay_can"),
                            Thatday_can_day = reader.GetValue<int>("Thatday_can_day"),
                            Service_Contain = reader.GetValue<string>("service_Contain"),
                            Service_NotContain = reader.GetValue<string>("service_NotContain"),
                            Precautions = reader.GetValue<string>("Precautions"),
                            Tuan_pro = reader.GetValue<int>("tuan_pro"),
                            Zhixiao = reader.GetValue<int>("zhixiao"),
                            Agentsale = reader.GetValue<int>("agentsale"),
                            Createtime = reader.GetValue<DateTime>("createtime"),
                            Sms = reader.GetValue<string>("Sms"),
                            Createuserid = reader.GetValue<int>("createuserid"),
                            Imgurl = reader.GetValue<int>("imgurl"),
                            Pro_number = reader.GetValue<int>("pro_number"),
                            Pro_explain = reader.GetValue<string>("pro_explain"),
                            Pro_Integral = reader.GetValue<decimal>("pro_Integral"),
                            Tuipiao = reader.GetValue<int>("TuiPiao"),
                            Tuipiao_guoqi = reader.GetValue<int>("TuiPiao_Guoqi"),
                            Tuipiao_endday = reader.GetValue<int>("TuiPiao_Endday"),
                            Serviceid = reader.GetValue<int>("serviceid"),
                            Service_proid = reader.GetValue<string>("service_proid"),
                            Realnametype = reader.GetValue<int>("realnametype"),
                            Projectid = reader.GetValue<int>("projectid"),

                            Travelproductid = reader.GetValue<int>("trval_productid"),
                            Traveltype = reader.GetValue<string>("travel_type").ConvertTo<int>(0),
                            Travelstarting = reader.GetValue<string>("trval_starting"),

                            Ispanicbuy = reader.GetValue<int>("ispanicbuy"),
                            Panic_begintime = reader.GetValue<DateTime>("panicbuybegin_time"),
                            Panicbuy_endtime = reader.GetValue<DateTime>("panicbuyenddate_time"),
                            Limitbuytotalnum = reader.GetValue<int>("limitbuytotalnum"),
                            Linepro_booktype = reader.GetValue<int>("linepro_booktype"),
                            ProValidateMethod = reader.GetValue<string>("ProValidateMethod"),
                            Appointdata = reader.GetValue<int>("appointdata"),
                            Iscanuseonsameday = reader.GetValue<int>("iscanuseonsameday"),
                            Viewmethod = reader.GetValue<int>("viewmethod"),
                            Childreduce = reader.GetValue<decimal>("childreduce"),
                            Bindingid = reader.GetValue<int>("Bindingid"),
                            pickuppoint = reader.GetValue<string>("pickuppoint"),
                            dropoffpoint = reader.GetValue<string>("dropoffpoint"),
                            pro_note = reader.GetValue<string>("pro_note"),
                            QuitTicketMechanism = reader.GetValue<int>("QuitTicketMechanism"),

                            daybespeaknum = reader.GetValue<int>("daybespeaknum"),
                            isneedbespeak = reader.GetValue<int>("isneedbespeak"),
                            bespeaksucmsg = reader.GetValue<string>("bespeaksucmsg"),
                            bespeakfailmsg = reader.GetValue<string>("bespeakfailmsg"),
                            customservicephone = reader.GetValue<string>("customservicephone"),
                            isblackoutdate = reader.GetValue<int>("isblackoutdate"),
                            etickettype = reader.GetValue<int>("etickettype"),
                            buynum = reader.GetValue<int>("buynum"),
                            pro_weight = reader.GetValue<decimal>("pro_weight"),
                            nuomi_dealid = reader.GetValue<string>("nuomi_dealid"),
                            bookpro_bindphone = reader.GetValue<string>("bookpro_bindphone"),
                            bookpro_ispay = reader.GetValue<int>("bookpro_ispay"),
                            bookpro_bindname = reader.GetValue<string>("bookpro_bindname"),
                            Manyspeci = reader.GetValue<int>("Manyspeci"),
                            isrebate = reader.GetValue<int>("isrebate"),
                            unsure = reader.GetValue<int>("unsure"),
                            unyuyueyanzheng = reader.GetValue<int>("unyuyueyanzheng"),
                            selbindbx = reader.GetValue<int>("selbindbx"),
                        });
                    }

                }
                totalcount = list.Count;
                return list;
            }
            #endregion
            #region  批量拉取 page
            else if (method.Trim() == "page")
            {
                var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

                var condition = " server_type in (1,10) and ManySpeci=0  and pro_state=1 and convert(varchar(10),pro_end,120)>='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                //授权给分销的商户
                condition += " and  com_id in (select comid from agent_warrant where agentid =" + Agentid + " and warrant_state=1)";
                //产品不包含 已经授权给分销的商户下的没有设置分销价格的产品
                condition += " and not id in (" +
    " select id from b2b_com_pro where server_type not in (2,8) and agent1_price =0 and com_id in (select comid from agent_warrant where agentid =" + Agentid + " and warrant_level=1)" +
    " )" +
    " and not id in (" +
    " select id from b2b_com_pro where  server_type not in (2,8) and  agent2_price =0 and com_id in (select comid from agent_warrant where agentid =" + Agentid + " and warrant_level=2)" +
    " )" +
    " and not id in (" +
    " select id from b2b_com_pro where  server_type not in (2,8) and  agent3_price =0 and com_id in (select comid from agent_warrant where agentid =" + Agentid + " and warrant_level=3)" +
    " )";
                //产品不包含 授权给分销是验证扣款的商户下的外部接口产品(现在只有阳光)
                condition += " and not id in (select id from b2b_com_pro where source_type=3 and com_id in (select comid from agent_warrant where  agentid =" + Agentid + " and Warrant_type = 2))";


                cmd.PagingCommand1("b2b_com_pro", "*", "id", "", pagesize, pageindex, "", condition);

                List<B2b_com_pro> list = new List<B2b_com_pro>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new B2b_com_pro
                        {
                            ishasdeliveryfee = reader.GetValue<int>("ishasdeliveryfee"),
                            deliverytmp = reader.GetValue<int>("deliverytmp"),
                            Id = reader.GetValue<int>("id"),
                            Com_id = reader.GetValue<int>("com_id"),
                            Pro_name = reader.GetValue<string>("pro_name"),
                            Pro_state = reader.GetValue<int>("pro_state"),
                            Server_type = reader.GetValue<int>("server_type"),
                            Pro_type = reader.GetValue<int>("pro_type"),
                            Source_type = reader.GetValue<int>("source_type"),
                            Pro_Remark = reader.GetValue<string>("pro_Remark"),
                            Pro_start = reader.GetValue<DateTime>("pro_start"),
                            Pro_end = reader.GetValue<DateTime>("pro_end"),
                            Face_price = reader.GetValue<decimal>("face_price"),
                            Advise_price = reader.GetValue<decimal>("advise_price"),
                            Agent1_price = reader.GetValue<decimal>("Agent1_price"),
                            Agent2_price = reader.GetValue<decimal>("Agent2_price"),
                            Agent3_price = reader.GetValue<decimal>("Agent3_price"),
                            Agentsettle_price = reader.GetValue<decimal>("agentsettle_price"),
                            ThatDay_can = reader.GetValue<int>("ThatDay_can"),
                            Thatday_can_day = reader.GetValue<int>("Thatday_can_day"),
                            Service_Contain = reader.GetValue<string>("service_Contain"),
                            Service_NotContain = reader.GetValue<string>("service_NotContain"),
                            Precautions = reader.GetValue<string>("Precautions"),
                            Tuan_pro = reader.GetValue<int>("tuan_pro"),
                            Zhixiao = reader.GetValue<int>("zhixiao"),
                            Agentsale = reader.GetValue<int>("agentsale"),
                            Createtime = reader.GetValue<DateTime>("createtime"),
                            Sms = reader.GetValue<string>("Sms"),
                            Createuserid = reader.GetValue<int>("createuserid"),
                            Imgurl = reader.GetValue<int>("imgurl"),
                            Pro_number = reader.GetValue<int>("pro_number"),
                            Pro_explain = reader.GetValue<string>("pro_explain"),
                            Pro_Integral = reader.GetValue<decimal>("pro_Integral"),
                            Tuipiao = reader.GetValue<int>("TuiPiao"),
                            Tuipiao_guoqi = reader.GetValue<int>("TuiPiao_Guoqi"),
                            Tuipiao_endday = reader.GetValue<int>("TuiPiao_Endday"),
                            Serviceid = reader.GetValue<int>("serviceid"),
                            Service_proid = reader.GetValue<string>("service_proid"),
                            Realnametype = reader.GetValue<int>("realnametype"),
                            Projectid = reader.GetValue<int>("projectid"),

                            Travelproductid = reader.GetValue<int>("trval_productid"),
                            Traveltype = reader.GetValue<string>("travel_type").ConvertTo<int>(0),
                            Travelstarting = reader.GetValue<string>("trval_starting"),

                            Ispanicbuy = reader.GetValue<int>("ispanicbuy"),
                            Panic_begintime = reader.GetValue<DateTime>("panicbuybegin_time"),
                            Panicbuy_endtime = reader.GetValue<DateTime>("panicbuyenddate_time"),
                            Limitbuytotalnum = reader.GetValue<int>("limitbuytotalnum"),
                            Linepro_booktype = reader.GetValue<int>("linepro_booktype"),
                            ProValidateMethod = reader.GetValue<string>("ProValidateMethod"),
                            Appointdata = reader.GetValue<int>("appointdata"),
                            Iscanuseonsameday = reader.GetValue<int>("iscanuseonsameday"),
                            Viewmethod = reader.GetValue<int>("viewmethod"),
                            Childreduce = reader.GetValue<decimal>("childreduce"),
                            Bindingid = reader.GetValue<int>("Bindingid"),
                            pickuppoint = reader.GetValue<string>("pickuppoint"),
                            dropoffpoint = reader.GetValue<string>("dropoffpoint"),
                            pro_note = reader.GetValue<string>("pro_note"),
                            QuitTicketMechanism = reader.GetValue<int>("QuitTicketMechanism"),

                            daybespeaknum = reader.GetValue<int>("daybespeaknum"),
                            isneedbespeak = reader.GetValue<int>("isneedbespeak"),
                            bespeaksucmsg = reader.GetValue<string>("bespeaksucmsg"),
                            bespeakfailmsg = reader.GetValue<string>("bespeakfailmsg"),
                            customservicephone = reader.GetValue<string>("customservicephone"),
                            isblackoutdate = reader.GetValue<int>("isblackoutdate"),
                            etickettype = reader.GetValue<int>("etickettype"),
                            buynum = reader.GetValue<int>("buynum"),
                            pro_weight = reader.GetValue<decimal>("pro_weight"),
                            nuomi_dealid = reader.GetValue<string>("nuomi_dealid"),
                            bookpro_bindphone = reader.GetValue<string>("bookpro_bindphone"),
                            bookpro_ispay = reader.GetValue<int>("bookpro_ispay"),
                            bookpro_bindname = reader.GetValue<string>("bookpro_bindname"),
                            Manyspeci = reader.GetValue<int>("Manyspeci"),
                            isrebate = reader.GetValue<int>("isrebate"),
                            unsure = reader.GetValue<int>("unsure"),
                            unyuyueyanzheng = reader.GetValue<int>("unyuyueyanzheng"),
                            selbindbx = reader.GetValue<int>("selbindbx"),
                        });
                    }

                }
                totalcount = list.Count;
                return list;

            }
            #endregion
            else
            {
                totalcount = 0;
                return new List<B2b_com_pro>();
            }
        }

        internal IList<B2b_com_pro> GetWxProlistbyprojectid(int projectid, string servertypes, int topnums)
        {
            string sql = "select top " + topnums + " * from b2b_com_pro where projectid=" + projectid + " and server_type in (" + servertypes + ") and pro_state=1 and viewmethod in (1,3,5)";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);


            using (var reader = cmd.ExecuteReader())
            {
                IList<B2b_com_pro> list = new List<B2b_com_pro>();
                while (reader.Read())
                {
                    list.Add(new B2b_com_pro
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Pro_name = reader.GetValue<string>("pro_name"),
                        Pro_state = reader.GetValue<int>("pro_state"),
                        Server_type = reader.GetValue<int>("server_type"),
                        Pro_type = reader.GetValue<int>("pro_type"),
                        Source_type = reader.GetValue<int>("Source_type"),
                        Pro_Remark = reader.GetValue<string>("pro_Remark"),
                        Pro_start = reader.GetValue<DateTime>("pro_start"),
                        Pro_end = reader.GetValue<DateTime>("pro_end"),
                        Face_price = reader.GetValue<decimal>("face_price"),
                        Advise_price = reader.GetValue<decimal>("advise_price"),
                        Agentsettle_price = reader.GetValue<decimal>("agentsettle_price"),
                        ThatDay_can = reader.GetValue<int>("ThatDay_can"),
                        Thatday_can_day = reader.GetValue<int>("Thatday_can_day"),
                        Service_Contain = reader.GetValue<string>("service_Contain"),
                        Service_NotContain = reader.GetValue<string>("service_NotContain"),
                        Precautions = reader.GetValue<string>("Precautions"),
                        Tuan_pro = reader.GetValue<int>("tuan_pro"),
                        Zhixiao = reader.GetValue<int>("zhixiao"),
                        Agentsale = reader.GetValue<int>("agentsale"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Createuserid = reader.GetValue<int>("createuserid"),
                        Pro_explain = reader.GetValue<string>("pro_explain"),
                        Ispanicbuy = reader.GetValue<int>("Ispanicbuy"),
                        Limitbuytotalnum = reader.GetValue<int>("Limitbuytotalnum"),
                        ishasdeliveryfee = reader.GetValue<int>("ishasdeliveryfee"),
                        Imgurl = reader.GetValue<int>("imgurl"),
                        Pro_Integral = reader.GetValue<decimal>("Pro_Integral"),
                        Projectid = reader.GetValue<int>("projectid"),
                        Bindingid = reader.GetValue<int>("Bindingid"),
                        Imgaddress = FileSerivce.GetImgUrl(reader.GetValue<int>("imgurl")),
                    });
                }
                return list;
            }
        }

        internal int GetServertypeByPno(string pno)
        {
            string sql = "select server_type  from b2b_com_pro where id =(select pro_id  from b2b_eticket where pno='" + pno + "')";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("server_type");
                }
                return 0;
            }
        }

        internal string GetProBindPosid(int proid)
        {
            string sql = "select SpecifyPosid from b2b_com_pro where id =" + proid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                string bindposid = "";
                if (reader.Read())
                {
                    bindposid = reader.GetValue<string>("SpecifyPosid");
                }
                return bindposid;
            }
        }

        internal string GetProjectNameByProid(string proid)
        {
            string sql = "select projectname from b2b_com_project where id =(select projectid from b2b_com_pro where id=" + proid + ")";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                string r = "";
                if (reader.Read())
                {
                    r = reader.GetValue<string>("projectname");
                }
                return r;
            }
        }

        internal List<B2b_com_pro> Selhotelproductlist(int comid, int projectid)
        {
            string sql = "select id ,pro_name from b2b_com_pro where com_id=" + comid;
            if (projectid > 0)
            {
                sql += " and projectid=" + projectid;
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            List<B2b_com_pro> list = new List<B2b_com_pro>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_com_pro
                    {
                        Id = reader.GetValue<int>("id"),
                        Pro_name = reader.GetValue<string>("pro_name")
                    });
                }
            }
            return list;

        }

        internal int RollbackProKucun(int proid, int num, string traveldate = "", string enddate = "")
        {
            try
            {
                #region  查询得到原产品id
                //判断产品是否是导入产品
                string sql1 = "select Bindingid,server_type from b2b_com_pro where id=" + proid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql1);


                int primaryProid = 0;//原产品id
                int server_type = 0;//服务类型

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        primaryProid = reader.GetValue<int>("Bindingid");
                        server_type = reader.GetValue<int>("server_type");

                    }
                }


                if (primaryProid == 0)
                {
                    primaryProid = proid;
                }
                #endregion

                #region 服务类型是票务 或者 实物，需要把可销售数量和 已销售数量回滚
                if (server_type == 1 || server_type == 11)
                {
                    string sql2 = "update b2b_com_pro set limitbuytotalnum=limitbuytotalnum+" + num + ",buynum=buynum-" + num + " where id=" + primaryProid + " or Bindingid=" + primaryProid;
                    cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                    cmd.ExecuteNonQuery();
                }
                #endregion
                #region 服务类型是旅游大巴 , 需要把空位数量 回滚
                else if (server_type == 10)
                {
                    string sql = "update B2b_com_LineGroupDate set emptynum=emptynum+" + num + "  where lineid=" + primaryProid + " and CONVERT(varchar(10),daydate,120)='" + traveldate + "'";
                    cmd = sqlHelper.PrepareTextSqlCommand(sql);
                    return cmd.ExecuteNonQuery();
                }
                #endregion
                #region 服务类型是  酒店, 需要把空位数量 回滚
                else if (server_type == 9)
                {
                    string sql = "update B2b_com_LineGroupDate set emptynum=emptynum+" + num + "  where lineid=" + primaryProid + " and CONVERT(varchar(10),daydate,120)>='" + traveldate + "'  and CONVERT(varchar(10),daydate,120)<'" + enddate + "'";
                    cmd = sqlHelper.PrepareTextSqlCommand(sql);
                    return cmd.ExecuteNonQuery();
                }
                #endregion
                #region 其他服务类型暂不处理
                else
                { }
                #endregion


                return 1;
            }
            catch
            {
                return 0;
            }
        }



        internal int GetServertypeByProid(int proid)
        {
            string sql = "select server_type from b2b_com_pro where id=" + proid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                int r = 0;
                if (reader.Read())
                {
                    r = reader.GetValue<int>("server_type");
                }
                return r;
            }
        }

        internal int GetLimitbuytotalnum(int proid)
        {
            string sql = "select limitbuytotalnum from b2b_com_pro where id=" + proid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                int r = 0;
                if (reader.Read())
                {
                    r = reader.GetValue<int>("limitbuytotalnum");
                }
                return r;
            }
        }

        internal List<B2b_com_pro> GetTimeoutOrderProlist()
        {
            string sql = "select id,server_type,ispanicbuy,Manyspeci,Bindingid from b2b_com_pro where id in (select pro_id from b2b_order where order_state=1 and pay_state=1 and u_subdate<'" + DateTime.Now.AddMinutes(-30) + "')";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            List<B2b_com_pro> list = new List<B2b_com_pro>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_com_pro
                    {
                        Id = reader.GetValue<int>("id"),
                        Server_type = reader.GetValue<int>("server_type"),
                        Ispanicbuy = reader.GetValue<int>("ispanicbuy"),
                        Manyspeci = reader.GetValue<int>("Manyspeci"),
                        Bindingid = reader.GetValue<int>("Bindingid"),
                    });
                }
            }
            return list;
        }

        internal int GetPaystateByOrderid(int orderid)
        {
            string sql = "select pay_state from b2b_order where id=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                int paystate = 1;
                if (reader.Read())
                {
                    paystate = reader.GetValue<int>("pay_state");
                }
                return paystate;
            }
        }

        internal List<B2b_com_pro_group> GetProgrouplistByComid(int comid, string runstate = "0,1")
        {
            string sql = "select * from B2b_com_pro_group where comid=" + comid;
            if (runstate != "0,1")
            {
                sql += " and runstate=" + runstate;
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            List<B2b_com_pro_group> list = new List<B2b_com_pro_group>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_com_pro_group
                    {
                        id = reader.GetValue<int>("id"),
                        groupname = reader.GetValue<string>("groupname"),
                        runstate = reader.GetValue<int>("runstate"),
                        comid = reader.GetValue<int>("comid")
                    });
                }
                return list;
            }
        }

        internal List<B2b_com_pro_group> GetProgroupPagelistByComid(int comid, int pageindex, int pagesize, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            string condition = " comid=" + comid;
            cmd.PagingCommand1("B2b_com_pro_group", "*", "id desc", "", pagesize, pageindex, "", condition);

            List<B2b_com_pro_group> list = new List<B2b_com_pro_group>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_com_pro_group
                    {
                        id = reader.GetValue<int>("id"),
                        groupname = reader.GetValue<string>("groupname"),
                        runstate = reader.GetValue<int>("runstate"),
                        comid = reader.GetValue<int>("comid")
                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }

        internal int Editprogroup(B2b_com_pro_group m)
        {
            if (m.id > 0)
            {
                string sql = "update B2b_com_pro_group set groupname='" + m.groupname + "',runstate=" + m.runstate + " where id=" + m.id;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.ExecuteNonQuery();

                return m.id;
            }
            else
            {
                //判断商户下是否有同名的组，防止添加重复
                string sq = "select count(1) from  B2b_com_pro_group where groupname ='" + m.groupname + "'";
                var cm = sqlHelper.PrepareTextSqlCommand(sq);
                object co = cm.ExecuteScalar();
                if (int.Parse(co.ToString()) > 0)
                {
                    return 0;
                }
                else
                {
                    string sql = "insert  into B2b_com_pro_group (groupname,runstate,comid) values('" + m.groupname + "'," + m.runstate + "," + m.comid + ");select @@identity;";
                    var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                    object o = cmd.ExecuteScalar();

                    return int.Parse(o.ToString());
                }


            }
        }



        internal List<B2b_com_pro_Package> GetProPackagPagelistByid(int pid, int pageindex, int pagesize, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            string condition = " fid=" + pid;
            cmd.PagingCommand1("B2b_com_pro_Package", "*", "id desc", "", pagesize, pageindex, "", condition);

            List<B2b_com_pro_Package> list = new List<B2b_com_pro_Package>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_com_pro_Package
                    {
                        Id = reader.GetValue<int>("id"),
                        Fid = reader.GetValue<int>("Fid"),
                        Sid = reader.GetValue<int>("Sid"),
                        Snum = reader.GetValue<int>("Snum")
                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }


        internal B2b_com_pro_Package GetProPackagbyid(int id)
        {
            string sql = "select * from B2b_com_pro_Package where id=" + id;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                B2b_com_pro_Package m = null;
                if (reader.Read())
                {
                    m = new B2b_com_pro_Package
                    {
                        Id = reader.GetValue<int>("id"),
                        Fid = reader.GetValue<int>("Fid"),
                        Sid = reader.GetValue<int>("Sid"),
                        Snum = reader.GetValue<int>("Snum"),
                    };
                }
                return m;
            }
        }




        #region 添加或修改套票绑定产品
        internal int ProPackageInsertOrUpdate(B2b_com_pro_Package product)
        {
            var sqlTxt = @"UPDATE B2b_com_pro_Package
		   SET 			   			   
			     fid  =   @Fid,
			     Snum=    @Snum,
			     Sid =   @Sid,   
		   WHERE Id = @Id;";


            if (product.Id == 0)
            {

                sqlTxt = @"insert B2b_com_pro_Package (fid,sid,snum) values(@Fid,@Sid,@Snum);";
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@Id", product.Id);
            cmd.AddParam("@Fid", product.Fid);
            cmd.AddParam("@Sid", product.Sid);
            cmd.AddParam("@Snum", product.Snum);

            object obj = cmd.ExecuteScalar();

            return obj != null ? int.Parse(obj.ToString()) : 0;
        }
        #endregion

        #region 删除套票产品绑定
        internal int DelProPackagbyid(int id)
        {
            var sqlTxt = @"delete B2b_com_pro_Package  
		   WHERE Id = @Id;";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@Id", id);
            object obj = cmd.ExecuteScalar();

            return obj != null ? int.Parse(obj.ToString()) : 0;
        }
        #endregion

        #region 计算此酒店房间入住的结算价
        internal int jisuan_hotel_jiesuanjia(int comid, int proid, string stardate, string enddate)
        {
            var sqlTxt = @"select B2b_com_pro_Package  
		   WHERE Id = @Id;";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@Id", proid);
            object obj = cmd.ExecuteScalar();

            return obj != null ? int.Parse(obj.ToString()) : 0;
        }
        #endregion


        internal List<B2b_com_pro> GetNotStockProPagelist(int comid, int agentid, int agentlevel, int pageindex, int pagesize, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var tblName = "b2b_com_pro";
            var strGetFields = "id,pro_name";
            var sortKey = "id desc";
            //暂时只是支持 商家自己家 的 单规格产品：多规格产品美团接口不支持；倒码产品、分销导入产品、外来接口产品 无法保证验证通知及时发送给美团，可能会造成票已经验证但是美团仍然可以退票的情况，给商家带来损失。
            var condition = "server_type=1 and  source_type=1 and ManySpeci=0 and pro_state=1 and com_id=" + comid + " and id not in (select proid from b2b_com_pro_groupbuystocklog where comid=" + comid + " and stockagentcompanyid=" + agentid + "  and isstock=1)  and convert(varchar(10),pro_end,120)>='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            if (agentlevel == 1)
            {
                condition += " and Agent1_price>0";
            }
            if (agentlevel == 2)
            {
                condition += " and Agent2_price>0";
            }
            if (agentlevel == 3)
            {
                condition += " and Agent3_price>0";
            }



            //cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);
            cmd.PagingCommand1(tblName, strGetFields, sortKey, "", pagesize, pageindex, "0", condition);

            List<B2b_com_pro> list = new List<B2b_com_pro>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_com_pro
                    {
                        Id = reader.GetValue<int>("id"),
                        Pro_name = reader.GetValue<string>("pro_name"),

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }

        internal List<int> Getbindingproidlist(int proid)
        {
            string sql = "select id from b2b_com_pro where bindingid=" + proid + " and pro_state=1";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            List<int> list = new List<int>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(reader.GetValue<int>("id"));
                }
            }


            return list;
        }

        internal List<int> GetAutoDownlineProlist()
        {
            string sql = "select id from b2b_com_pro where pro_state=1 and  pro_end<'" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            List<int> list = new List<int>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(reader.GetValue<int>("id"));
                }
            }


            return list;
        }

        internal decimal GetAgentPrice(int proid, int agentlevel)
        {
            string sql = "";
            if (agentlevel == 1)
            {
                sql = "select Agent1_price from b2b_com_pro where id=" + proid;
            }
            else if (agentlevel == 2)
            {
                sql = "select Agent2_price from b2b_com_pro where id=" + proid;
            }
            else
            {
                sql = "select Agent3_price from b2b_com_pro where id=" + proid;
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                decimal price = 0;
                if (reader.Read())
                {
                    if (agentlevel == 1)
                    {
                        price = reader.GetValue<decimal>("Agent1_price");
                    }
                    else if (agentlevel == 2)
                    {
                        price = reader.GetValue<decimal>("Agent2_price");
                    }
                    else
                    {
                        price = reader.GetValue<decimal>("Agent3_price");
                    }
                }
                return price;
            }
        }

        internal List<int> GetProChildImgArr(int proid, int topnum)
        {
            string sql = "select top " + topnum + " fileuploadid from b2b_com_pro_childimg where proid=" + proid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                List<int> list = new List<int>();
                while (reader.Read())
                {
                    list.Add(reader.GetValue<int>("fileuploadid"));
                }
                return list;
            }
        }


        internal List<B2b_com_pro_bandingzhajipos> GetProbandingzhajilistByproid(int comid, int proid, out int totalcount)
        {
            string sql = "select * from b2b_com_pro_bandingzhajipos where comid=" + comid + " and proid=" + proid ;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            List<B2b_com_pro_bandingzhajipos> list = new List<B2b_com_pro_bandingzhajipos>();
            using (var reader = cmd.ExecuteReader())
            {
                int i = 0;
                while (reader.Read())
                {
                    list.Add(new B2b_com_pro_bandingzhajipos
                    {
                        id = reader.GetValue<int>("id"),
                        comid = reader.GetValue<int>("comid"),
                        pos_id = reader.GetValue<string>("pos_id"),
                        proid = reader.GetValue<int>("proid"),

                    });
                    i = i + 1;
                }
                totalcount = i;
            }
            

            return list;
        }



        internal List<B2b_com_pro_bandingzhajipos> GetProbandingzhajilistByproidposid(int comid, int proid, string pos_id, out int totalcount)
        {
            string sql = "select * from b2b_com_pro_bandingzhajipos where comid=" + comid + " and proid=" + proid + " and pos_id ='" + pos_id + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            List<B2b_com_pro_bandingzhajipos> list = new List<B2b_com_pro_bandingzhajipos>();
            using (var reader = cmd.ExecuteReader())
            {
                int i = 0;
                while (reader.Read())
                {
                    list.Add(new B2b_com_pro_bandingzhajipos
                    {
                        id = reader.GetValue<int>("id"),
                        comid = reader.GetValue<int>("comid"),
                        pos_id = reader.GetValue<string>("pos_id"),
                        proid = reader.GetValue<int>("proid"),

                    });
                    i = i + 1;
                }
                totalcount = i;
            }


            return list;
        }

        #region 添加或修改绑定闸机
        internal int ProbandingzhajiposInsertOrUpdate(B2b_com_pro_bandingzhajipos prozhaji)
        {
            var sqlTxt = "";


            if (prozhaji.id == 0)
            {

                sqlTxt = @"insert B2b_com_pro_bandingzhajipos (comid,proid,pos_id) values(@comid,@proid,@pos_id);";
            }
            else
            {
                sqlTxt = @"UPDATE B2b_com_pro_bandingzhajipos
		               SET 			   			   
			                 proid  =   @proid,
			                 comid=    @comid,
			                 pos_id =   @posid,   
		               WHERE id = @id;";

            }

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@id", prozhaji.id);
            cmd.AddParam("@comid", prozhaji.comid);
            cmd.AddParam("@proid", prozhaji.proid);
            cmd.AddParam("@pos_id", prozhaji.pos_id);

            object obj = cmd.ExecuteScalar();

            return obj != null ? int.Parse(obj.ToString()) : 0;
        }
        #endregion


        #region 删除绑定闸机
        internal int Probandingzhajiposdel(int comid,int proid)
        {
            var sqlTxt = @"delete B2b_com_pro_bandingzhajipos
		   WHERE 			   			   
			     proid  =  @proid and 
			     comid=    @comid ;";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@proid", proid);

            object obj = cmd.ExecuteScalar();

            return obj != null ? int.Parse(obj.ToString()) : 0;
        }
        #endregion





        internal List<Rentserver_User_zhajilog> GetRentserver_User_zhajilogByuid(int comid, int Rentserver_Userid, out int totalcount)
        {
            string sql = "select * from Rentserver_User_zhajilog where comid=" + comid + " and Rentserver_Userid=" + Rentserver_Userid +" order by id desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            List<Rentserver_User_zhajilog> list = new List<Rentserver_User_zhajilog>();
            using (var reader = cmd.ExecuteReader())
            {
                int i = 0;
                while (reader.Read())
                {
                    list.Add(new Rentserver_User_zhajilog
                    {
                        id = reader.GetValue<int>("id"),
                        comid = reader.GetValue<int>("comid"),
                        pos_id = reader.GetValue<string>("pos_id"),
                        proid = reader.GetValue<int>("proid"),
                        subtime = reader.GetValue<DateTime>("subtime"),
                        clearchipid = reader.GetValue<string>("clearchipid"),
                        oid = reader.GetValue<int>("oid"),
                        Rentserver_Userid =  reader.GetValue<int>("Rentserver_Userid"),

                    });
                    i = i + 1;
                }
                totalcount = i;
            }


            return list;
        }


        #region 添加或修改绑定闸机
        internal int Rentserver_User_zhajilogInsertOrUpdate(Rentserver_User_zhajilog prozhajilog)
        {
            var sqlTxt = "";


            if (prozhajilog.id == 0)
            {

                sqlTxt = @"insert Rentserver_User_zhajilog (comid,proid,pos_id,oid,subtime,clearchipid,Rentserver_Userid) values(@comid,@proid,@pos_id,@oid,@subtime,@clearchipid,@Rentserver_Userid);";
            }
            else {
                sqlTxt = @"UPDATE Rentserver_User_zhajilog
		   SET 			   			   
			     proid  =   @proid,
			     comid=    @comid,
			     pos_id =   @posid,
                 oid=@oid,
                 subtime=@subtime,
                 clearchipid =@clearchipid,
                 Rentserver_Userid= @Rentserver_Userid
		   WHERE id = @id;";
            
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@id", prozhajilog.id);
            cmd.AddParam("@comid", prozhajilog.comid);
            cmd.AddParam("@proid", prozhajilog.proid);
            cmd.AddParam("@pos_id", prozhajilog.pos_id);
            cmd.AddParam("@oid", prozhajilog.oid);
            cmd.AddParam("@subtime", prozhajilog.subtime);
            cmd.AddParam("@clearchipid", prozhajilog.clearchipid);
            cmd.AddParam("@Rentserver_Userid", prozhajilog.Rentserver_Userid);

            object obj = cmd.ExecuteScalar();

            return obj != null ? int.Parse(obj.ToString()) : 0;
        }
        #endregion

    }
}
