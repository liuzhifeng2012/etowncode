using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.PM.Service.LMM.Data;
using ETS2.PM.Service.LMM.Model;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;
using System.IO;
using ETS2.CRM.Service.CRMService.Data;
using Newtonsoft.Json;
using ETS.JsonFactory;
using System.Xml;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS2.VAS.Service.VASService.Data;
using ETS2.PM.Service.WL.Data;
using System.Runtime.Remoting.Messaging;
using System.Net;
using System.Text;
using ETS2.PM.Service.Meituan.Model;

namespace ETS2.WebApp.lvmama
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        //产品变化通知
        protected void Button1_Click(object sender, EventArgs e)
        {

            string proid = "12323";
            //string json = "{" +
            //                "\"num\": \"5\", " +
            //                "\"serialNo\": \"1006\", " +
            //                "\"settlePrice\": \"0.01\", " +
            //                "\"supplierGoodsId\": \"12323\", " +
            //                "\"sign\": \"42f2fb7fefd33ea7d6e073ecf2f9daac\", " +
            //                "\"timestamp\": \"20181219142615\", " +
            //                "\"visitTime\": \"20181220000000\", " +
            //                "\"uid\": \"lvmama1234\", " +
            //                "\"password\": \"85a61c849\"," +
            //                "\"contacts\": { " +
            //                        "\"idNum\": \"\", " +
            //                        "\"idType\": \"\", " +
            //                        "\"mobile\": \"13488761102\", " +
            //                        "\"name\": \"购票人\"" +
            //                   " }, " +
            //                    "\"travellerList\": [ " +
            //                     "   { " +
            //                      "      \"idNum\": \"\", " +
            //                       "     \"idType\": \"\", " +
            //                       "     \"mobile\": \"13488761102\", " +
            //                       "     \"name\": \"游玩人1\"" +
            //                        "}" +
            //                   " ]" +
            //                "}";

            //var data = JsonConvert.DeserializeObject<apply_codemodel>(json);

            //string json = "{" +
            //                "\"uid\": \"lvmama1234\"," +
            //                "\"password\": \"85a61c849\"," +
            //                "\"extId\": \"302390\", " +
            //                "\"sign\": \"0e0e607ea59e636e5c618984dbd71180\"," +
            //                "\"timestamp\": \"20181219142615\"" +
            //     "}";
            //var data = JsonConvert.DeserializeObject<discard_codemodel>(json);

            //string json = "{" +
            //                "\"uid\": \"lvmama1234\"," +
            //                "\"password\": \"85a61c849\"," +
            //                "\"extId\": \"302390\", " +
            //                "\"sign\": \"0e0e607ea59e636e5c618984dbd71180\"," +
            //                "\"timestamp\": \"20181219142615\"" +
            //     "}";
           // var data = JsonConvert.DeserializeObject<sms_resendmodel>(json);


            string json = "{\"contacts\":{\"idNum\":\"441400198002244166\",\"idType\":\"ID_CARD\",\"mobile\":\"13488761102\",\"name\":\"测试下单\"},\"num\":\"1\",\"password\":\"a6b50913\",\"serialNo\":\"2018122592819013\",\"settlePrice\":\"0.01\",\"sign\":\"3fe30eee43a6f04d62be36dc69d7a28b\",\"supplierGoodsId\":\"12811\",\"timestamp\":\"20181225135505\",\"travellerList\":[{\"idNum\":\"441400198002244166\",\"idType\":\"ID_CARD\",\"mobile\":\"13488761102\",\"name\":\"测试下单\"}],\"uid\":\"lvmama8527\",\"visitTime\":\"20181226000000\"}";
            var data = JsonConvert.DeserializeObject<apply_codemodel>(json);
            var send = testsend(data);
            //Label1.Text = r.orderid.ToString();
            //OrderJsonData.agentorder_shoudongchuli(303220);
        }



        public backRefund testsend(apply_codemodel requestData)
        {

            //string url = "http://localhost:14395/Meituan-jk/ordercreate.aspx";

            string url = "http://localhost:14395/lvmama/apply_code.aspx";

            backRefund result = new backRefund
            {
                IsSuccess = false,
            };
            try
            {
                var responseStr = DoRequest(url, JsonConvert.SerializeObject(requestData));
                Label1.Text = responseStr;
                if (string.IsNullOrEmpty(responseStr))
                {
                    result.msg = "返回数据为空";
                }
                else
                {
                    backRefund responseBody = (backRefund)JsonConvert.DeserializeObject(responseStr, typeof(backRefund));
                    if (responseBody.status == "0")
                    {
                        result.IsSuccess = true;
                        result.msg = JsonConvert.SerializeObject(responseBody);
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.msg = responseBody.msg;
                    }

                }
            }
            catch (Exception ex)
            {
                result.msg = "异常" + ex.Message;
            }

           
            return result;
        }


        public string DoRequest(string url, string data)
        {
            DateTime date = DateTime.Now;
            return PostResponse(url, data, date, 200000);
        }

        public static string GetCurrentDateStr()
        {
            return DateTime.Now.GetDateTimeFormats('r')[0];
        }
        public string PostResponse(string url, string postData, DateTime date, int Timeout)
        {

            //HttpRequestBase
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Date = date;
            var indexOf = url.IndexOf("/", 10, StringComparison.Ordinal);

            request.Method = "POST";
            request.ContentType = "application/json;charset=utf-8 ";
            request.Timeout = Timeout;
            request.ServicePoint.Expect100Continue = false;
            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = bytes.Length;
            HttpWebResponse response = null;
            try
            {
                request.GetRequestStream().Write(bytes, 0, bytes.Length);
                response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                {
                    return ex.Message;
                }
                WebResponse wr = ex.Response;
                Stream st = wr.GetResponseStream();
                StreamReader sr = new StreamReader(st, System.Text.Encoding.UTF8);
                string sError = sr.ReadToEnd();
                sr.Close();
                st.Close();
                return sError;
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
        }


    }
}