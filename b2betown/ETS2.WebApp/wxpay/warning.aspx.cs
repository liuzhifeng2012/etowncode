using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.Tenpay.TenpayApp;
using ETS2.Common.Business;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.VAS.Service.VASService.Data;
using ETS2.WeiXin.Service.WeiXinService.Model;


namespace ETS2.WebApp.wxpay
{
    public partial class warning : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //创建支付应答对象
            ResponseHandler resHandler = new ResponseHandler(Context);
            resHandler.init();

            int comid = 0;
            string RequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            if (Domain_def.Domain_yanzheng(RequestUrl))//如果符合shop101.etown.cn的格式
            {
                //先通过正则表达式获取COMid
                comid = Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl).ToString());
                B2b_finance_paytype modelfinance = new B2b_finance_paytypeData().GetFinancePayTypeByComid(comid);
                resHandler.setKey(modelfinance.Wx_partnerkey, modelfinance.Wx_paysignkey);
            }
            else
            {
                Response.Write("获取商家支付参数失败");
                return;
            }

            ////判断签名
            //if (resHandler.isTenpaySign())
            //{
            //    if (resHandler.isWXsign())
            //    {
            if (Request.HttpMethod.ToLower() == "post")
            {
                byte[] ar;
                ar = new byte[this.Request.InputStream.Length];

                this.Request.InputStream.Read(ar, 0, ar.Length);

                string sXML = this.Request.ContentEncoding.GetString(ar);

                //回复服务器处理成功
                string sql = "insert into wxwarning(responsexml,comid) values('" + sXML + "'," + comid + ")";
                ExcelSqlHelper.ExecuteNonQuery(sql);

                Response.Write("success");
                Response.Write("success:" + resHandler.getDebugInfo());
                //    }
                //}
                //else
                //{
                //    //sha1签名失败
                //    Response.Write("fail");
                //    Response.Write("fail:" + resHandler.getDebugInfo());
                //}
                Response.End();
            }
        }
    }
}