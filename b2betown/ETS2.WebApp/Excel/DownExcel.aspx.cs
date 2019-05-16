using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Threading;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using System.Reflection;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS2.Permision.Service.PermisionService.Model;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Member.Service.MemberService.Data;
using ETS2.Permision.Service.PermisionService.Data;
using ETS2.VAS.Service.VASService.Data;
using ETS2.VAS.Service.VASService.Modle;

namespace ETS2.WebApp.Excel
{
    public partial class DownExcel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string oper = Request["oper"].ConvertTo<string>("");
            #region 导出卡号
            if (oper == "outcardno")//导出卡号
            {
                int crid = Request["crid"].ConvertTo<int>(0);
                int comid = Request["comid"].ConvertTo<int>(0);
                if (crid > 0 && comid > 0)
                {
                    DataTable table = ExcelSqlHelper.ExecuteDataTable("select cardcode from member_card where crid=" + crid + " and com_id=" + comid);

                    ExcelRender.RenderToExcel(table, Context, "卡号列表.xls");
                }
            }
            #endregion
            #region 导出旅游大巴产品结单名单
            if (oper == "outlvyoubusorderlistbyorderstate")//导出旅游大巴产品结单名单
            {
                int proid = Request["proid"].ConvertTo<int>(0);
                string gooutdate = Request["gooutdate"].ConvertTo<string>("");
                int orderstate = Request["orderstate"].ConvertTo<int>(0);

                object o = ExcelSqlHelper.ExecuteScalar("select pro_name from b2b_com_pro where id=" + proid);

                //DataTable table = ExcelSqlHelper.ExecuteDataTable("select id as 订单号,u_name as 预订人,u_num as 预订人数,CONVERT(varchar(10),u_traveldate,120) as 出行时间,u_subdate as 预订时间 FROM b2b_order where  u_traveldate='" + gooutdate + "' and pro_id=" + proid + " and order_state=" + orderstate);
                //DataTable table = ExcelSqlHelper.ExecuteDataTable("SELECT  [orderid] as 订单号,[name] as 乘车人姓名,[IdCard] as 乘车人身份证,[Nation] as 乘车人民族,CONVERT(varchar(10),[travel_time],120) as 乘车日期,[yuding_name] as 预订人姓名,[yuding_phone] as 预订人电话,[yuding_time] as 预订时间,pickuppoint as 上车地点,dropoffpoint as 下车地点  FROM  [b2b_order_busNamelist] where orderid in (select id  FROM b2b_order where  u_traveldate='" + gooutdate + "' and pro_id=" + proid + " and order_state=" + orderstate + ")");

                DataTable table = ExcelSqlHelper.ExecuteDataTable("SELECT   a.[orderid] as 订单号,a.[name] as 乘车人姓名,a.[contactphone] as 乘车人电话,a.[IdCard] as 乘车人身份证,a.[Nation] as 乘车人民族,CONVERT(varchar(10),a.[travel_time],120) as 乘车日期,a.[yuding_name] as 预订人姓名,a.[yuding_phone] as 预订人电话,a.[yuding_time] as 预订时间,a.pickuppoint as 上车地点,a.dropoffpoint as 下车地点,b.company as 购票地点,a.contactremark  as 备注  FROM  agent_company as b right join [b2b_order_busNamelist] as a on a.agentid=b.id left join b2b_order as c on a.orderid=c.id where a.orderid in (select id  FROM b2b_order where  u_traveldate='" + gooutdate + "' and pro_id=" + proid + " and order_state=" + orderstate + ")");

                ExcelRender.RenderToExcel(table, Context, gooutdate + o.ToString() + "结单名单.xls");
            }
            #endregion
            #region 分销订单导入到excel
            if (oper == "agentordertoexcel")
            {
                var comid = Request["comid"].ConvertTo<int>(0);
                int agentid = Request["agentid"].ConvertTo<int>(0);
                var key = Request["key"].ConvertTo<string>("");
                var account = Request["account"].ConvertTo<string>("");
                var order_state = Request["order_state"].ConvertTo<int>(0);
                var begindate = Request["beginDate"].ConvertTo<string>("");
                var enddate = Request["endDate"].ConvertTo<string>("");
                var servertype = Request["servertype"].ConvertTo<int>(0);

                object o1 = ExcelSqlHelper.ExecuteScalar("select company from agent_company where id=" + agentid);
                object o2 = ExcelSqlHelper.ExecuteScalar("select com_name from b2b_company where id=" + comid);

                List<B2b_order> agentorderlist = new B2bOrderData().GetAgentOrderList(comid, agentid, key, order_state, begindate, enddate, servertype);
                if (agentorderlist.Count > 0)
                {
                    DataTable tblDatas = new DataTable("Datas");
                    DataColumn dc = null;
                    dc = tblDatas.Columns.Add("ID", Type.GetType("System.Int32"));
                    dc.AutoIncrement = true;//自动增加
                    dc.AutoIncrementSeed = 1;//起始为1
                    dc.AutoIncrementStep = 1;//步长为1
                    dc.AllowDBNull = false;

                    dc = tblDatas.Columns.Add("订单号", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("提单时间", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("产品名称", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("提单人", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("单价", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("数量", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("实收", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("状态", Type.GetType("System.String"));

                    DataRow newRow;
                    foreach (B2b_order morder in agentorderlist)
                    {
                        if (morder != null)
                        {
                            newRow = tblDatas.NewRow();
                            newRow["订单号"] = morder.Id;
                            newRow["提单时间"] = morder.U_subdate;
                            newRow["产品名称"] = morder.Order_type == 2 ? "预付款充值" : new B2bComProData().GetProById(morder.Pro_id.ToString(), morder.Speciid).Pro_name;
                            newRow["提单人"] = morder.U_name + "(" + morder.U_phone + ")";
                            newRow["单价"] = morder.Pay_price;
                            newRow["数量"] = morder.U_num;
                            newRow["实收"] = (morder.Pay_price) * (morder.U_num) + morder.Express - morder.Integral1 - morder.Imprest1;
                            newRow["状态"] = EnumUtils.GetName((OrderStatus)morder.Order_state);
                            tblDatas.Rows.Add(newRow);
                        }
                    }
                    ExcelRender.RenderToExcel(tblDatas, Context, o1.ToString() + "-" + o2.ToString() + "-订单列表.xls");
                }

            }
            #endregion

            #region  商户产品分销销售订单导入到excel
            if (oper == "agentsalescodetoexcel")
            {
                var comid = Request["comid"].ConvertTo<int>(0);
                var order_state = Request["order_state"].ConvertTo<int>(0);
                var servertype = Request["servertype"].ConvertTo<int>(0);
                var begindate = Request["beginDate"].ConvertTo<string>("");
                var enddate = Request["endDate"].ConvertTo<string>("");
                var key = Request["key"].ConvertTo<string>("");
                var ordertype = Request["ordertype"].ConvertTo<int>(0);
                var userid = Request["userid"].ConvertTo<int>(0);


                int orderIsAccurateToPerson = 0;
                int channelcompanyid = 0;
                if (userid > 0)
                {
                    Sys_Group group = new Sys_GroupData().GetGroupByUserId(userid);
                    if (group != null)
                    {
                        //判断订单是否要求精确到渠道人 
                        orderIsAccurateToPerson = group.OrderIsAccurateToPerson;
                        Member_Channel_company channelcom = new MemberChannelcompanyData().GetChannelCompanyByUserId(userid);
                        if (channelcom != null)
                        {
                            channelcompanyid = channelcom.Id;
                        }
                    }
                }

                object o2 = ExcelSqlHelper.ExecuteScalar("select com_name from b2b_company where id=" + comid);
                 
                List<B2b_order> directorderlist = new B2bOrderData().GetOrderList(comid, order_state, servertype, begindate, enddate, key, userid, orderIsAccurateToPerson, channelcompanyid, ordertype);
                if (directorderlist.Count > 0)
                {
                    DataTable tblDatas = new DataTable("Datas");
                    DataColumn dc = null;
                    dc = tblDatas.Columns.Add("ID", Type.GetType("System.Int32"));
                    dc.AutoIncrement = true;//自动增加
                    dc.AutoIncrementSeed = 1;//起始为1
                    dc.AutoIncrementStep = 1;//步长为1
                    dc.AllowDBNull = false;

                    dc = tblDatas.Columns.Add("订单号", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("提单时间", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("产品名称", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("提单人", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("单价", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("数量", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("实收", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("收货地址", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("状态", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("分销商", Type.GetType("System.String"));

                    DataRow newRow;
                    foreach (B2b_order morder in directorderlist)
                    {
                        if (morder != null)
                        {
                            newRow = tblDatas.NewRow();
                            newRow["订单号"] = morder.Id;
                            newRow["提单时间"] = morder.U_subdate;
                            newRow["产品名称"] = morder.Order_type == 2 ? "预付款充值" : new B2bComProData().GetProById(morder.Pro_id.ToString(), morder.Speciid).Pro_name;
                            newRow["提单人"] = morder.U_name + "(" + morder.U_phone + ")";
                            newRow["单价"] = morder.Pay_price;
                            newRow["数量"] = morder.U_num;
                            newRow["实收"] = (morder.Pay_price) * (morder.U_num) + morder.Express - morder.Integral1 - morder.Imprest1;
                            newRow["收货地址"] = morder.Province + "-" + morder.City + "-" + morder.Address;
                            newRow["状态"] = EnumUtils.GetName((OrderStatus)morder.Order_state);
                            newRow["分销商"] = new Agent_companyData().GetAgentName(morder.Agentid);
                            tblDatas.Rows.Add(newRow);
                        }
                    }
                    ExcelRender.RenderToExcel(tblDatas, Context, o2.ToString() + "-后台销售订单列表.xls");
                }
            }
            #endregion
            #region  商户产品分销倒码订单导入到excel
            if (oper == "agentbackcodetoexcel")
            {
                var comid = Request["comid"].ConvertTo<int>(0);
                var order_state = Request["order_state"].ConvertTo<int>(0);
                var servertype = Request["servertype"].ConvertTo<int>(0);
                var begindate = Request["beginDate"].ConvertTo<string>("");
                var enddate = Request["endDate"].ConvertTo<string>("");
                var key = Request["key"].ConvertTo<string>("");
                var ordertype = Request["ordertype"].ConvertTo<int>(0);
                var userid = Request["userid"].ConvertTo<int>(0);


                int orderIsAccurateToPerson = 0;
                int channelcompanyid = 0;
                if (userid > 0)
                {
                    Sys_Group group = new Sys_GroupData().GetGroupByUserId(userid);
                    if (group != null)
                    {
                        //判断订单是否要求精确到渠道人 
                        orderIsAccurateToPerson = group.OrderIsAccurateToPerson;
                        Member_Channel_company channelcom = new MemberChannelcompanyData().GetChannelCompanyByUserId(userid);
                        if (channelcom != null)
                        {
                            channelcompanyid = channelcom.Id;
                        }
                    }
                }

                object o2 = ExcelSqlHelper.ExecuteScalar("select com_name from b2b_company where id=" + comid);

                List<B2b_order> directorderlist = new B2bOrderData().GetOrderList(comid, order_state, servertype, begindate, enddate, key, userid, orderIsAccurateToPerson, channelcompanyid, ordertype);
                if (directorderlist.Count > 0)
                {
                    DataTable tblDatas = new DataTable("Datas");
                    DataColumn dc = null;
                    dc = tblDatas.Columns.Add("ID", Type.GetType("System.Int32"));
                    dc.AutoIncrement = true;//自动增加
                    dc.AutoIncrementSeed = 1;//起始为1
                    dc.AutoIncrementStep = 1;//步长为1
                    dc.AllowDBNull = false;

                    dc = tblDatas.Columns.Add("订单号", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("提单时间", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("产品名称", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("提单人", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("单价", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("数量", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("实收", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("收货地址", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("状态", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("分销商", Type.GetType("System.String"));
                    DataRow newRow;
                    foreach (B2b_order morder in directorderlist)
                    {
                        if (morder != null)
                        {
                            newRow = tblDatas.NewRow();
                            newRow["订单号"] = morder.Id;
                            newRow["提单时间"] = morder.U_subdate;
                            newRow["产品名称"] = morder.Order_type == 2 ? "预付款充值" : new B2bComProData().GetProById(morder.Pro_id.ToString(), morder.Speciid).Pro_name;
                            newRow["提单人"] = morder.U_name + "(" + morder.U_phone + ")";
                            newRow["单价"] = morder.Pay_price;
                            newRow["数量"] = morder.U_num;
                            newRow["实收"] = (morder.Pay_price) * (morder.U_num) + morder.Express - morder.Integral1 - morder.Imprest1;
                            newRow["收货地址"] = morder.Province + "-" + morder.City + "-" + morder.Address;
                            newRow["状态"] = EnumUtils.GetName((OrderStatus)morder.Order_state);
                            newRow["分销商"] = new Agent_companyData().GetAgentName(morder.Agentid);
                            tblDatas.Rows.Add(newRow);
                        }
                    }
                    ExcelRender.RenderToExcel(tblDatas, Context, o2.ToString() + "-倒码订单列表.xls");
                }
            }
            #endregion

            #region 直销订单导入到excel
            if (oper == "directordertoexcel")
            {
                var comid = Request["comid"].ConvertTo<int>(0);
                var order_state = Request["order_state"].ConvertTo<int>(0);
                var servertype = Request["servertype"].ConvertTo<int>(0);
                var begindate = Request["beginDate"].ConvertTo<string>("");
                var enddate = Request["endDate"].ConvertTo<string>("");
                var key = Request["key"].ConvertTo<string>("");
                var ordertype = Request["ordertype"].ConvertTo<int>(0);
                var userid = Request["userid"].ConvertTo<int>(0);


                int orderIsAccurateToPerson = 0;
                int channelcompanyid = 0;
                if (userid > 0)
                {
                    Sys_Group group = new Sys_GroupData().GetGroupByUserId(userid);
                    if (group != null)
                    {
                        //判断订单是否要求精确到渠道人 
                        orderIsAccurateToPerson = group.OrderIsAccurateToPerson;
                        Member_Channel_company channelcom = new MemberChannelcompanyData().GetChannelCompanyByUserId(userid);
                        if (channelcom != null)
                        {
                            channelcompanyid = channelcom.Id;
                        }
                    }
                }

                object o2 = ExcelSqlHelper.ExecuteScalar("select com_name from b2b_company where id=" + comid);

                List<B2b_order> directorderlist = new B2bOrderData().GetOrderList(comid, order_state, servertype, begindate, enddate, key, userid, orderIsAccurateToPerson, channelcompanyid, ordertype);
                if (directorderlist.Count > 0)
                {
                    DataTable tblDatas = new DataTable("Datas");
                    DataColumn dc = null;
                    dc = tblDatas.Columns.Add("ID", Type.GetType("System.Int32"));
                    dc.AutoIncrement = true;//自动增加
                    dc.AutoIncrementSeed = 1;//起始为1
                    dc.AutoIncrementStep = 1;//步长为1
                    dc.AllowDBNull = false;

                    dc = tblDatas.Columns.Add("订单号", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("提单时间", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("产品名称", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("提单人", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("身份证", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("手机", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("单价", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("数量", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("实收", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("收货地址", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("状态", Type.GetType("System.String"));

                    DataRow newRow;
                    foreach (B2b_order morder in directorderlist)
                    {
                        if (morder != null)
                        {
                            newRow = tblDatas.NewRow();
                            newRow["订单号"] = morder.Id;
                            newRow["提单时间"] = morder.U_subdate;
                            newRow["产品名称"] = morder.Order_type == 2 ? "预付款充值" : new B2bComProData().GetProById(morder.Pro_id.ToString(), morder.Speciid).Pro_name;
                            newRow["提单人"] = morder.U_name ;
                            newRow["身份证"] = morder.U_idcard;
                            newRow["手机"] =  morder.U_phone ;
                            newRow["单价"] = morder.Pay_price;
                            newRow["数量"] = morder.U_num;
                            newRow["实收"] = (morder.Pay_price) * (morder.U_num) + morder.Express - morder.Integral1 - morder.Imprest1;
                            newRow["收货地址"] = morder.Province + "-" + morder.City + "-" + morder.Address;
                            newRow["状态"] = EnumUtils.GetName((OrderStatus)morder.Order_state);
                            tblDatas.Rows.Add(newRow);
                        }
                    }
                    ExcelRender.RenderToExcel(tblDatas, Context, o2.ToString() + "-直销订单列表.xls");
                }
            }
            #endregion
            #region 订房信息导入到excel
            if (oper == "hotelordertoexcel")
            {
                int comid = Request["comid"].ConvertTo<int>(0);
                string begindate = Request["begindate"].ConvertTo<string>("");
                string enddate = Request["enddate"].ConvertTo<string>("");
                int productid = Request["productid"].ConvertTo<int>(0);
                string orderstate = Request["orderstate"].ConvertTo<string>("");
                try
                {
                    object o1 = ExcelSqlHelper.ExecuteScalar("select com_name from b2b_company where id=" + comid);
                    object o2 = ExcelSqlHelper.ExecuteScalar("select pro_name from b2b_com_pro where id=" + productid);
                    object o3 = ExcelSqlHelper.ExecuteScalar("select projectname from b2b_com_project where id=(select projectid from b2b_com_pro where id=" + productid + ")");

                    DataTable tblDatas = new B2bOrderData().HotelOrderlist(comid, begindate, enddate, productid, orderstate, 1);

                    if (begindate == "" || enddate == "")
                    {
                        ExcelRender.RenderToExcel(tblDatas, Context, o1.ToString() + "-" + o2.ToString() + "-" + o3.ToString() + "-订房订单列表.xls");
                    }
                    else
                    {
                        ExcelRender.RenderToExcel(tblDatas, Context, o1.ToString() + "-" + o2.ToString() + "-" + o3.ToString() + "(" + DateTime.Parse(begindate).ToString("yyyy-MM-dd") + "到" + DateTime.Parse(enddate).ToString("yyyy-MM-dd") + ")" + "-订房订单列表.xls");
                    }

                }
                catch { }
            }
            #endregion
            #region 下载分销特定项目下的验票记录
            if (oper == "downagentpro_yplist")
            {
                int proid = Request["proid"].ConvertTo<int>(0);
                string startime = Request["startime"].ConvertTo<string>("");
                string endtime = Request["endtime"].ConvertTo<string>("");
                int agentid = 0;
                if (Session["Agentid"] != null)
                {
                    agentid = Int32.Parse(Session["Agentid"].ToString());
                }

                object o1 = ExcelSqlHelper.ExecuteScalar("select company from agent_company where id=" + agentid);
                object o2 = ExcelSqlHelper.ExecuteScalar("select pro_name from b2b_com_pro where id=" + proid);


                int comid = new B2bComProData().GetComidByProid(proid);

                var condition = " a.action=1 and a.com_id=" + comid + " and a.a_state=" + (int)ECodeOperStatus.OperSuc + " and a.pno in (select pno from b2b_eticket where 1=1 ";

                if (proid != 0)
                {
                    condition += " and pro_id=" + proid;
                }
                if (agentid != 0)
                {
                    condition += " and pro_id in (select id from b2b_com_pro where projectid in (select projectid from b2b_project_agent where agentid=" + agentid + ") )";
                }
                condition += ")";


                if (startime != "")
                {
                    condition += " and a.actiondate>='" + startime + "'";
                }
                if (endtime != "")
                {
                    endtime = DateTime.Parse(endtime).AddDays(1).ToString();
                    condition += " and a.actiondate<'" + endtime + "'";
                }

                ExcelRender.RenderToExcel(
                ExcelSqlHelper.ExecuteDataTable(CommandType.Text, "select b.e_proname [产品名称],a.pno [电子码],b.pnum [订购数量],a.use_pnum [本次使用数量],a.actiondate [验证时间],a.posid [验证pos],b.e_face_price [门市价],b.e_sale_price [结算价],c.company[出票方] from b2b_etcket_log as a  left join b2b_eticket as b on a.com_id=b.com_id and a.pno=b.pno and a.eticket_id=b.id left join agent_company as c on b.agent_id=c.id where  " + condition + " order by a.id desc"),
                Context, "分销:" + o1.ToString() + "-" + "产品:" + o2.ToString() + "-验证电子票列表(" + startime + "--" + endtime + ")" + ".xls");
            }
            #endregion

            #region 财务导出到excel
            if (oper == "Financelisttoexcel")
            {
                var comid = Request["comid"].ConvertTo<int>(0);
                var begindate = Request["beginDate"].ConvertTo<string>("");
                var enddate = Request["endDate"].ConvertTo<string>("");

                var financedata = new B2bFinanceData();
               

                var directorderlist = financedata.FinanceallList(comid, begindate, enddate);
                if (directorderlist.Count > 0)
                {
                    DataTable tblDatas = new DataTable("Datas");
                    DataColumn dc = null;
                    dc = tblDatas.Columns.Add("ID", Type.GetType("System.Int32"));
                    dc.AutoIncrement = true;//自动增加
                    dc.AutoIncrementSeed = 1;//起始为1
                    dc.AutoIncrementStep = 1;//步长为1
                    dc.AllowDBNull = false;

                    dc = tblDatas.Columns.Add("订单号", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("时间", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("产品名称", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("类型", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("金额", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("余额", Type.GetType("System.String"));
                    dc = tblDatas.Columns.Add("渠道", Type.GetType("System.String"));


                    DataRow newRow;
                    foreach (B2b_Finance morder in directorderlist)
                    {
                        if (morder != null)
                        {
                            newRow = tblDatas.NewRow();
                            newRow["订单号"] = morder.Order_id;
                            newRow["时间"] = morder.Subdate;
                            newRow["产品名称"] = morder.Servicesname;
                            newRow["类型"] = morder.Payment_type ;
                            newRow["金额"] = morder.Money;
                            newRow["余额"] = morder.Over_money;
                            newRow["渠道"] = morder.Money_come=="wx"?"微信支付":morder.Money_come;
                           
                            tblDatas.Rows.Add(newRow);
                        }
                    }
                    ExcelRender.RenderToExcel(tblDatas, Context, comid.ToString()+ DateTime.Now.ToString("yyyyMMddHHmmss") + "-财务明细列表.xls");
                }
            }
            #endregion

            #region 注释内容
            //int splitid = Request["splitid"].ConvertTo<int>(0);//微信拆分表id
            //if (splitid == 0)//下载excel导入样例操作,暂时没有用到
            //{
            //    DataTable table = new DataTable();
            //    table.Columns.Add("姓名", typeof(string));
            //    table.Columns.Add("手机", typeof(string));
            //    table.Columns.Add("邮箱", typeof(string));
            //    table.Columns.Add("微信号", typeof(string));

            //    table.Columns.Add("预付款", typeof(string));
            //    table.Columns.Add("积分", typeof(string));
            //    table.Columns.Add("目的地", typeof(string));
            //    table.Columns.Add("年龄段", typeof(string));
            //    table.Columns.Add("会员级别", typeof(string));

            //    for (int i = 0; i < 10; i++)
            //    {
            //        string name = "张三" + i.ToString();
            //        Thread.Sleep(1);
            //        string phone = "手机号" + i.ToString();
            //        Thread.Sleep(1);
            //        string email = "邮箱地址" + i.ToString();
            //        Thread.Sleep(1);
            //        string weixin = "微信" + i.ToString();
            //        Thread.Sleep(1);
            //        string imprest = i.ToString();
            //        Thread.Sleep(1);
            //        string integral = i.ToString();
            //        Thread.Sleep(1);
            //        string destination = "目的地" + i.ToString();
            //        Thread.Sleep(1);
            //        string agegroup = "年龄段" + i.ToString();
            //        Thread.Sleep(1);
            //        string crmlevel = "1";
            //        Thread.Sleep(1);

            //        table.Rows.Add(name, phone, email, weixin, imprest, integral, destination, agegroup, crmlevel);
            //    }

            //    ExcelRender.RenderToExcel(table, Context, "Excel导入样例.xls");
            //}
            //else//下载获取的微信关注用户表 的拆分用户列表,暂时没有到
            //{
            //    ObtainGzListSplit model = new ObtainGzListSplitData().GetObtainGzListSplit(splitid);
            //    if (model != null)
            //    {
            //        string openidstr = model.Splitopenid;
            //        string[] str = openidstr.Split(',');

            //        DataTable table = new DataTable();
            //        table.Columns.Add("姓名", typeof(string));
            //        table.Columns.Add("手机", typeof(string));
            //        table.Columns.Add("邮箱", typeof(string));
            //        table.Columns.Add("微信号", typeof(string));


            //        for (int i = 0; i < str.Length; i++)
            //        {
            //            string name = "";
            //            Thread.Sleep(1);
            //            string phone = "";
            //            Thread.Sleep(1);
            //            string email = "";
            //            Thread.Sleep(1);
            //            string weixin = str[i];
            //            Thread.Sleep(1);

            //            table.Rows.Add(name, phone, email, weixin);
            //        }

            //        ExcelRender.RenderToExcel(table, Context, model.Comid + "-" + splitid + ".xls");
            //    }
            //    else
            //    {
            //        Label1.Text = "获取拆分用户列表为空";
            //    }
            //}
            #endregion

        }
        /// <summary>
        /// 将泛类型集合List类转换成DataTable
        /// </summary>
        /// <param name="list">泛类型集合</param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(List<T> entitys)
        {
            //检查实体集合不能为空
            if (entitys == null || entitys.Count < 1)
            {
                throw new Exception("需转换的集合为空");
            }
            //取出第一个实体的所有Propertie
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            //生成DataTable的structure
            //生产代码中，应将生成的DataTable结构Cache起来，此处略
            DataTable dt = new DataTable();
            for (int i = 0; i < entityProperties.Length; i++)
            {
                //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
                dt.Columns.Add(entityProperties[i].Name);
            }
            //将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                //检查所有的的实体都为同一类型
                if (entity.GetType() != entityType)
                {
                    throw new Exception("要转换的集合元素类型不一致");
                }
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }
    }
}