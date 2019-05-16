using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;
using System.Net;
using System.IO;
using System.Web;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;

namespace ETS2.VAS.Service.VASService.Data.Common
{
    public class MjldInter
    {
        //private string user = "jkcsdl01";//代理商用户登陆名
        //private string password = "123456";//代理商用户登陆密码
        //private string businessid = "6e88e52077df4d20bd6d7cf7a90f5a80";//商户ID
        //private string des_key = "71debbbf55f64f8b8c604994";//加密Key

        //private string interurl = "http://preview1.mjld.com.cn:8022/Outer/Interface/";//接口url
        private string interurl = "http://outer.mjld.com.cn/Outer/Interface/";//正式接口url

        #region 2.5、	提交订单（SubmitOrder）
        public string SubmitOrder(ApiService mapiservice, Api_Mjld_SubmitOrder_input minput)
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<Body>" +
                    "<timeStamp>" + minput.timeStamp + "</timeStamp>" +
                    "<user>" + minput.user + "</user>" +
                    "<password>" + minput.password + "</password>" +
                    "<goodsId>" + minput.goodsId + "</goodsId>" +
                    "<num>" + minput.num + "</num>" + //<!—可以填多个，默认为1 -->  
                    "<phone>" + minput.phone + "</phone>" +
                    "<batch>" + minput.batch + "</batch>" +//<!-值填1时一码一票，值填0或不填该字段是一码多票>
                    "<guest_name>" + minput.guest_name + "</guest_name>" +
                    "<identityno>" + minput.identityno + "</identityno>" +
                    "<order_note>" + minput.order_note + "</order_note>" +
                    "<forecasttime>" + minput.forecasttime + "</forecasttime>" +//【产品详情里IsReserve=True时，需传递该时间；IsReserve=False时，必须保留该值为空】
                    "<outOrderId>" + minput.orderId + "</outOrderId>" +
                    "<orderpost>" +//快递信息
                    "<consignee>" + minput.consignee + "</consignee>" +
                    "<address>" + minput.address + "</address>" +
                    "<zipcode>" + minput.zipcode + "</zipcode>" +
                    "</orderpost>" +
                "</Body>";

            string data = HttpUtility.UrlEncode(Mjld_TCodeServiceCrypt.Encrypt3DESToBase64(xml, mapiservice.Deskey));
            string postData = string.Format("businessid={1}&content={0}", data, mapiservice.Organization);

            string str = POST(interurl + "SubmitOrder", postData);

            string bstr = "fail";
            try
            {
                bstr = Mjld_TCodeServiceCrypt.Decrypt3DESFromBase64(str, mapiservice.Deskey);
            }
            catch
            {
                bstr += " " + str;
            }

            //录入交互日志
            ApiLog mapilog = new ApiLog
            {
                Id = 0,
                request_type = "SubmitOrder",
                Serviceid = 3,
                Str = xml.Trim(),
                Subdate = DateTime.Now,
                ReturnStr = bstr,
                ReturnSubdate = DateTime.Now,
                Errmsg = "",
            };
            int ins = new ApiLogData().EditLog(mapilog);

            return bstr;
        }
        #endregion
        #region 2.7、	短信重发（ReSendSms）
        public string ReSendSms(ApiService mapiservice, Api_Mjld_SubmitOrder_output moutput)
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                            "<Body>" +
                            "<timeStamp>" + CommonFunc.ConvertDateTimeInt(DateTime.Now).ToString() + "</timeStamp>" +
                            "<user>" + mapiservice.Servicername + "</user>" +
                            "<password>" + mapiservice.Password + "</password>" +
                            "<orderId>" + moutput.mjldOrderId + "</orderId>" +
                            "<credenceno>" + moutput.credence + "</credenceno>" +
                        "</Body>";

            string data = HttpUtility.UrlEncode(Mjld_TCodeServiceCrypt.Encrypt3DESToBase64(xml, mapiservice.Deskey));
            string postData = string.Format("businessid={1}&content={0}", data, mapiservice.Organization);

            string str = POST(interurl + "ReSendSms", postData);

            string bstr = "fail";
            try
            {
                bstr = Mjld_TCodeServiceCrypt.Decrypt3DESFromBase64(str, mapiservice.Deskey);
            }
            catch
            {
                bstr += " " + str;
            }

            //录入交互日志
            ApiLog mapilog = new ApiLog
            {
                Id = 0,
                request_type = "ReSendSms",
                Serviceid = 3,
                Str = xml.Trim(),
                Subdate = DateTime.Now,
                ReturnStr = bstr,
                ReturnSubdate = DateTime.Now,
                Errmsg = "",
            };
            int ins = new ApiLogData().EditLog(mapilog);

            return bstr;
        }
        #endregion
        #region 2.10、	订单整单快速退单（RefundByOrderID）
        public string RefundByOrderID(ApiService mapiservice, Api_mjld_RefundByOrderID mrefund)
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                           "<Body>" +
                              "<timeStamp>" + CommonFunc.ConvertDateTimeInt(DateTime.Now).ToString() + "</timeStamp>" +
                              "<user>" + mrefund.user + "</user>" +
                              "<password>" + mrefund.password + "</password>" +
                              "<RefundPart>" + mrefund.RefundPart + "</RefundPart>" + //true：允许部分退票，false：不允许部分退票
                              "<outBackId>" + mrefund.orderid + "</outBackId>" +
                              "<orderid>" + mrefund.mjldorderId + "</orderid>" +
                           "</Body>";

            string data = HttpUtility.UrlEncode(Mjld_TCodeServiceCrypt.Encrypt3DESToBase64(xml, mapiservice.Deskey));
            string postData = string.Format("businessid={1}&content={0}", data, mapiservice.Organization);

            string str = POST(interurl + "RefundByOrderID", postData);

            string bstr = "";
            try
            {
                bstr = Mjld_TCodeServiceCrypt.Decrypt3DESFromBase64(str, mapiservice.Deskey);
            }
            catch
            {
                bstr = "";
            }

            //录入交互日志
            ApiLog mapilog = new ApiLog
            {
                Id = 0,
                request_type = "RefundByOrderID",
                Serviceid = 3,
                Str = xml.Trim(),
                Subdate = DateTime.Now,
                ReturnStr = bstr,
                ReturnSubdate = DateTime.Now,
                Errmsg = "",
            };
            int ins = new ApiLogData().EditLog(mapilog);

            return bstr;
        }
        #endregion
        #region 2.6、	订单浏览（GetOrderDetail）
        public string GetOrderDetail(ApiService mapiservice, string MjldinsureNo)
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                            "<Body>" +
                              "<timeStamp>" + CommonFunc.ConvertDateTimeInt(DateTime.Now).ToString() + "</timeStamp>" +
                            "<user>" + mapiservice.Servicername + "</user>" +
                              "<password>" + mapiservice.Password + "</password>" +
                              "<orderId>" + MjldinsureNo + "</orderId>" +
                              "<outOrderId ></outOrderId>" +
                            "</Body>";

            string data = HttpUtility.UrlEncode(Mjld_TCodeServiceCrypt.Encrypt3DESToBase64(xml, mapiservice.Deskey));
            string postData = string.Format("businessid={1}&content={0}", data, mapiservice.Organization);

            string str = POST(interurl + "GetOrderDetail", postData);

            string bstr = "";
            try
            {
                bstr = Mjld_TCodeServiceCrypt.Decrypt3DESFromBase64(str, mapiservice.Deskey);
            }
            catch
            {
                bstr = "";
            }

            //录入交互日志
            ApiLog mapilog = new ApiLog
            {
                Id = 0,
                request_type = "GetOrderDetail",
                Serviceid = 3,
                Str = xml.Trim(),
                Subdate = DateTime.Now,
                ReturnStr = bstr,
                ReturnSubdate = DateTime.Now,
                Errmsg = "",
            };
            int ins = new ApiLogData().EditLog(mapilog);

            return bstr;
        }
        #endregion
        #region 发送post请求
        private string POST(string url, string postdata)
        {
            var content = "";
            var data = Encoding.UTF8.GetBytes(postdata);
            // 准备请求...
            try
            {
                // 设置参数
                var request = WebRequest.Create(url) as HttpWebRequest;
                var cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                var outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据
                var response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                var instream = response.GetResponseStream();


                //返回结果网页（html）代码
                var myStreamReader = new StreamReader(instream, Encoding.GetEncoding("utf-8"));
                content = myStreamReader.ReadToEnd();

                return content;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        #endregion


    }
}
