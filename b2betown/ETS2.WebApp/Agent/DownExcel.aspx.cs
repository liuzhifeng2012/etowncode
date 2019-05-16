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

namespace ETS2.WebApp.Agent
{
    public partial class DownExcel : System.Web.UI.Page
    {
        public int comid = 0;
        public int agentid = 0;
        public int orderid = 0;
        public string md5info = "";
        public string err = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            comid = Request["comid"].ConvertTo<int>(0);
            agentid = Request["agentid"].ConvertTo<int>(0);
            orderid = Request["orderid"].ConvertTo<int>(0);
            md5info = Request["md5info"].ConvertTo<string>("");

            string Returnmd5 = EncryptionHelper.ToMD5(orderid.ToString() + comid.ToString() + agentid.ToString() + "lixh1210", "UTF-8");

            if (Returnmd5 == md5info)//验证MD5
            {
                B2bOrderData orderdate = new B2bOrderData();
                B2b_order ordermodel = orderdate.GetOrderById(orderid);

                if (ordermodel != null)
                {
                    B2bComProData prodata = new B2bComProData();
                    B2b_com_pro promodel = prodata.GetProById(ordermodel.Pro_id.ToString());

                    if (promodel != null)
                    {
                        if (ordermodel.Agentid == agentid && ordermodel.Comid == comid && ordermodel.Warrant_type == 2)
                        {
                            ExcelRender.RenderToExcel(
                            ExcelSqlHelper.ExecuteDataTable(CommandType.Text, "select e_proname [产品名称],pno [票号],pnum [数量] from b2b_eticket where oid=" + orderid),
                            Context, promodel.Pro_name + ordermodel.U_subdate.Date.ToString("yyyyMMdd") + "-" + ordermodel.Id.ToString() + ".xls");

                        }
                        else {
                            err = "订单参数错误";
                        }
                    }
                    else {
                        err = "产品错误";
                    }
                }
                else {
                    err = "订单错误";
                }
            }
            else {
                err = "验证错误";
            }
           
        }
    }
}