using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.PM.Service.Meituan.Data;
using ETS.Framework;

namespace ETS2.WebApp.M
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            #region 签名验证
            string date = System.Web.HttpContext.Current.Request.Headers.Get("Date");
            string PartnerId = System.Web.HttpContext.Current.Request.Headers.Get("PartnerId");
            string Authorization = System.Web.HttpContext.Current.Request.Headers.Get("Authorization");
            string requestMethod = System.Web.HttpContext.Current.Request.HttpMethod;
            string URI = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath;
            //authorization 形式: "MWS" + " " + client + ":" + sign;
            //string mtSign = Authorization.Substring(Authorization.IndexOf(":") + 1);

            //string beforeSign = requestMethod + " " + URI + "\n" + "Wed, 06 May 2015 10:34:20 GMT";
            string beforeSign = "POST /rhone/mtp/api/order/test/ba" + "\n" + "Wed, 06 May 2015 10:34:20 GMT";
            string afterSign = new MeiTuanInter().GetSign(beforeSign);
            //判断签名是否正确
            if (afterSign == "")
            {
                //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\MTlog.txt", "签名错误:mtSign-" + mtSign + "  meSign-" + afterSign);
                Response.Write("签名正确：" + afterSign);
                return;
            }
            else {

                Response.Write("签名错误：" + afterSign + "::::" + afterSign);
                return;
            }
            #endregion

           // mlog.req_type = URI;

          //  string actionResult = GetProductList(mlog);

            
        }


    }
}