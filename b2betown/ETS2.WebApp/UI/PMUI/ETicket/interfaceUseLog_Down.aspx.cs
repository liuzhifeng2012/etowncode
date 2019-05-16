using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using ETS.Framework;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Common.Business;
using ETS2.PM.Service.PMService.Modle.Enum;


namespace ETS2.WebApp.UI.PMUI.ETicket
{
    public partial class interfaceUseLog_Down : System.Web.UI.Page
    {

        public int comid = 0;
        public int agentid = 0;
        public int orderid = 0;
        public string md5info = "";
        public string err = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            var comid = Request["comid"];
            var startime = Request["startime"].ConvertTo<string>("");
            var endtime = Request["endtime"].ConvertTo<string>("");


            //如果没有日期，只读取当天数据
            if (startime == "") {
                startime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (endtime == "")
            {
                endtime = DateTime.Now.ToString("yyyy-MM-dd");
            }

            string endtime_temp = endtime;

            var condition = " a.comid=" + comid;
            
            if (startime != "")
            {
                condition += " and a.usetime>='" + startime + "'";
            }
            if (endtime != "")
            {
                endtime = DateTime.Parse(endtime).AddDays(1).ToString();
                condition += " and a.usetime<'" + endtime + "'";
            }

            ExcelRender.RenderToExcel(
            ExcelSqlHelper.ExecuteDataTable(CommandType.Text, "select a.orderid [编号],b.Pro_name [产品名称],c.U_name [姓名],c.U_phone [手机],d.company [出票单位],a.wlorderid [WL订单号],c.Pay_price [结算价],a.usedQuantity [使用数量],b.Pro_end[有效期],a.usetime[使用时间] from wl_uselog as a  left join b2b_com_pro as b on a.proid=b.id left join b2b_order as c on a.orderid=c.id left join agent_company as d on c.agentid=d.id where  " + condition + " order by a.id desc"),
            Context, "WLinterfaceuselog" + startime + "--" + endtime_temp + ".xls");



        }
    }
}