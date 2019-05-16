using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS.Framework;

namespace ETS2.WebApp.UI.MemberUI
{
    public partial class BespeakDetail : System.Web.UI.Page
    {
        public int proid = 0;//产品id
        public string proname = "";//产品名称
        public string gooutdate = "";//出行日期
        public int paysucnum = 0;//统计总人数
        public int closeteamnum = 0;//已截团人数
        public int orderstatus_hasfin = (int)OrderStatus.HasFin;//订单状态:已结团


        public string operremark = "";//操作备注
        public int operlogid = 0;//操作日志id

        protected void Page_Load(object sender, EventArgs e)
        {
            proid = Context.Request["proid"].ConvertTo<int>(0);
            gooutdate = Context.Request["gooutdate"].ConvertTo<string>("");
            if (proid == 0 || gooutdate == "")
            {
                Response.Redirect("/default.aspx");
            }

            if (proid != 0 && gooutdate != "")
            {
                //获取 产品名称
                object o = ExcelSqlHelper.ExecuteScalar("select pro_name from b2b_com_pro where id=" + proid);
                proname = o == null ? "" : o.ToString();

                try
                {
                    //获取 支付成功订单的统计人数
                    object q = ExcelSqlHelper.ExecuteScalar("select sum(u_num) from b2b_order where pro_id=" + proid + " and u_traveldate='" + gooutdate + "' and pay_state=" + (int)PayStatus.HasPay);
                    paysucnum = q == null ? 0 : int.Parse(q.ToString());

                    //获取 结团人数
                    q = ExcelSqlHelper.ExecuteScalar("select sum(u_num) from b2b_order where pro_id=" + proid + " and u_traveldate='" + gooutdate + "' and order_state=" + orderstatus_hasfin);
                    closeteamnum = q == null ? 0 : int.Parse(q.ToString());
                }
                catch
                {
                    paysucnum = 0;
                }
                //判断是否有操作日志
                object qq = ExcelSqlHelper.ExecuteScalar("select count(1) from travelbusorder_operlog where proid=" + proid + " and traveldate='" + gooutdate + "'");
                int logcount = qq == null ? 0 : int.Parse(qq.ToString());
                if (logcount > 0)//如果含有操作日志
                {
                    //操作日志备注
                    qq = ExcelSqlHelper.ExecuteScalar("select  operremark from travelbusorder_operlog where proid=" + proid + " and traveldate='" + gooutdate + "'");
                    operremark = qq == null ? "" : qq.ToString();
                    //操作日志id
                    qq = ExcelSqlHelper.ExecuteScalar("select id from travelbusorder_operlog where proid=" + proid + " and traveldate='" + gooutdate + "'");
                    operlogid = qq == null ? 0 : int.Parse(qq.ToString());
                }
            }
            else
            {
                Response.Redirect("/default.aspx");
            }
        }
    }
}