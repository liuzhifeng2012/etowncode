using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.CRM.Service.CRMService.Data;
using ETS.Framework;
using ETS2.Common.Business;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.WebApp.wxpay
{
    public partial class wxpaysuc : System.Web.UI.Page
    {
        public string phone = "";
        public string comid = "";
        public string comname = "";
        public string order_type = "";//订单类型:1正常订单；2充值订单
        public int orderid = 0;
        public string md5 = "";
        public int servertype = 0;

        public bool bo = false;//是否是手机端
        protected void Page_Load(object sender, EventArgs e)
        {
            phone = Request["phone"].ConvertTo<string>("");
            comid = Request["comid"].ConvertTo<string>("");
            comname = Request["comname"].ConvertTo<string>("");
            order_type = Request["order_type"].ConvertTo<string>("1");
            orderid = Request["orderid"].ConvertTo<int>(0);
            md5 = Request["md5"].ConvertTo<string>("");

            servertype = Request["servertype"].ConvertTo<int>(0);
            string Returnmd5 = EncryptionHelper.ToMD5(orderid.ToString() + "lixh1210", "UTF-8");



            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            bo = detectmobilebrowser.HttpUserAgent(u);
            //System.Threading.Thread.Sleep(10000); //延迟执行
            //if (bo)
            //{
            //    Response.Redirect("http://shop.etown.cn/agent/m");
            //}
            //else 
            //{
            //    Response.Redirect("http://shop.etown.cn/agent");
            //}

        }
    }
}