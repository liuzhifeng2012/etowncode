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
    public partial class eticketlist_Down2 : System.Web.UI.Page
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
            var projectid = Request["projectid"].ConvertTo<int>(0);
            var proid = Request["proid"].ConvertTo<int>(0);
            var key = Request["key"].ConvertTo<string>("");//电子码

            object o1 = ExcelSqlHelper.ExecuteScalar("select com_name from b2b_company where id=" + comid);

            var condition = " a.action=1 and a.com_id=" + comid + " and a.a_state=" + (int)ECodeOperStatus.OperSuc + " and a.pno in (select pno from b2b_eticket where 1=1 ";
            if (projectid != 0)
            {
                condition += " and pro_id in (select id from b2b_com_pro where projectid=" + projectid + ")";
            }
            if (proid != 0)
            {
                condition += " and pro_id=" + proid;
            }
            condition += ")";

            if (key != "")
            {
                condition += " and a.pno='" + key + "' ";
            }
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
            Context, o1.ToString() + "验证电子票列表" + ".xls");



        }
    }
}