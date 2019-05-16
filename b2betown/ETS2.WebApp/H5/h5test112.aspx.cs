using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using System.Text;
using Newtonsoft.Json;
using ETS.JsonFactory;

namespace ETS2.WebApp.H5
{
    public partial class h5test112 : System.Web.UI.Page
    {
        private string partnerId = "23386";//渠道商身份标识，由慧择指定
        private string signkey = "23386^*#%";//签名秘钥，由慧择指定


        protected void Page_Load(object sender, EventArgs e)
        {
        }

        //投保
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            t_caozuo.Text = "orderApply";

            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string transNo = CommonFunc.ConvertDateTimeInt(DateTime.Parse(nowtime)).ToString();//(以后需要修改为渠道商订单号)交易流水号，每次请求不能相同
            string caseCode = "0000057083200808";//(以后需要更改为正式产品)方案代码，每一款保险公司产品的方案代码都不相同，由慧择分配

            //签名，预签名数据：密钥+渠道商身份标识+交易流水号+方案代码
            string sign = EncryptionHelper.ToMD5(signkey + partnerId + transNo + caseCode, "UTF-8");
            sign = sign.ToLower();

            string s = "{" +
                        "\"partnerId\":" + partnerId + "," +
                        "\"transNo\":\"" + transNo + "\"," +
                        "\"caseCode\":\"" + caseCode + "\"," +
                        "\"sign\":\"" + sign + "\"," +
                        "\"applicationData\":{" +
                                "\"applicationDate\":\"" + nowtime + "\"," +
                                "\"startDate\":\"2015-10-01\"," +
                                "\"endDate\":\"2015-10-03\"" +
                        "}," +
                        "\"applicantInfo\":{" +
                                 "\"cName\":\"贝书文\"," +
                                  "\"eName\":\"seven\"," +
                                  "\"cardType\":\"1\"," +
                                  "\"cardCode\":\"460108198404070653\"," +
                                  "\"sex\":1," +
                                   "\"birthday\":\"1984-04-07\"," +
                                   "\"mobile\":\"13800000000\"," +
                                   "\"email\":\"379639880@qq.com\"," +
                                   "\"jobInfo\":null" +
                         "}," +
                        "\"insurantInfos\":[" +
                            "{" +
                                 "\"insurantId\":1001," +
                                 "\"cName\":\"贝书文\"," +
                                 "\"eName\":\"seven\"," +
                                 "\"sex\":1," +
                                 "\"cardType\":3," +
                                 "\"cardCode\":\"4601\"," +
                                 "\"birthday\":\"1984-04-07\"," +
                                 "\"relationId\":1," +
                                 "\"count\":1," +
                                 "\"singlePrice\":40.00," +
                                 "\"fltNo\":null," +
                                 "\"fltDate\":null," +
                                 "\"city\":\"深圳\"," +
                                 "\"tripPurposeId\":0," +
                                  "\"destination\":null," +
                                  "\"visaCity\":null," +
                                  "\"jobInfo\":null," +
                                  "\"mobile\":null" +
                            "}" +
                        "]," +
                        "\"extendInfo\":{" +
                             "\"userId\":null," +
                             "\"email\":null," +
                             "\"userName\":null," +
                             "\"phone\":null" +
                         "}" +
            "}";
            t_xml.Text = s;
        }

        //退保
        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            t_caozuo.Text = "orderCancel";

            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string transNo = CommonFunc.ConvertDateTimeInt(DateTime.Parse(nowtime)).ToString();//(以后需要修改为渠道商订单号)易流水号，每次请求不能相同
           
            string insureNo="15091019569042";//投保单号
            //签名，预签名数据：密钥+渠道商身份标识+交易流水号+方案代码
            string sign = EncryptionHelper.ToMD5(signkey + partnerId + transNo + insureNo, "UTF-8");
            sign = sign.ToLower();

            string s = "{" +
                    "\"transNo\":\""+transNo+"\"," +
                    "\"partnerId\":"+partnerId+"," +
                    "\"insureNo\":\"" + insureNo + "\"," +
                    "\"sign\":\""+sign+"\"," +
                    "\"extendInfo\":{" +
                        "\"userId\":null," +
                        "\"email\":null," +
                        "\"userName\":null," +
                        "\"phone\":null" +
                    "}" +
                "}";

            t_xml.Text = s;
        }

        //保单下载
        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            t_caozuo.Text = "download";

            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string transNo = CommonFunc.ConvertDateTimeInt(DateTime.Parse(nowtime)).ToString();//(以后需要修改为渠道商订单号)易流水号，每次请求不能相同

            string insureNo = "15091019569042";//投保单号
            //签名，预签名数据：密钥+渠道商身份标识+交易流水号+方案代码
            string sign = EncryptionHelper.ToMD5(signkey + partnerId + transNo + insureNo, "UTF-8");
            sign = sign.ToLower();

 
            string s = "{"+
                            "\"transNo\":\""+transNo+"\","+
                            "\"partnerId\":"+partnerId+","+
                            "\"insureNum\":\""+insureNo+"\","+
                            "\"sign\":\""+sign+"\""+
                       "}";

            t_xml.Text = s;
        }
        //投保单查询
        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            t_caozuo.Text = "orderDetail";

            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string transNo = CommonFunc.ConvertDateTimeInt(DateTime.Parse(nowtime)).ToString();//(以后需要修改为渠道商订单号)易流水号，每次请求不能相同

            string insureNo = "15091019569042";//投保单号
            //签名，预签名数据：密钥+渠道商身份标识+交易流水号+方案代码
            string sign = EncryptionHelper.ToMD5(signkey + partnerId + transNo + insureNo, "UTF-8");
            sign = sign.ToLower();

            string s = "{"+
                            "\"transNo\":\""+transNo+"\","+
                            "\"partnerId\":"+partnerId+","+
                            "\"insureNum\":\""+insureNo+"\","+
                            "\"sign\":\""+sign+"\","+
                            "\"idCard\":null,"+
                            "\"email\":null,"+
                            "\"userId\":null,"+
                            "\"pageNum\":1,"+
                            "\"pageSize\":5"+
                        "}";

            t_xml.Text = s;
        }
        //批量查询保单
        protected void LinkButton5_Click(object sender, EventArgs e)
        {
            t_caozuo.Text = "orderSearch";

            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string transNo = CommonFunc.ConvertDateTimeInt(DateTime.Parse(nowtime)).ToString();//(以后需要修改为渠道商订单号)易流水号，每次请求不能相同
             
            //签名，预签名数据：密钥+渠道商身份标识+交易流水号+方案代码
            string sign = EncryptionHelper.ToMD5(signkey + partnerId + transNo , "UTF-8");
            sign = sign.ToLower();

            string s = "{"+
                            "\"transNo\":\""+transNo+"\","+
                            "\"partnerId\":"+partnerId+","+
                            "\"sign\":\""+sign+"\","+
                            //"\"insureNums\":[\"15080546794635\",\"15080544341441\"],"+
                            "\"insureNums\":[]," +
                            "\"applicant\":null,"+
                            "\"insurant\":null,"+
                            "\"idCard\":null,"+
                            "\"userId\":null,"+
                            "\"startTime\":null,"+
                            "\"endTime\":null,"+
                            "\"pageNum\":1,"+
                            "\"pageSize\":5"+
                        "}";

            t_xml.Text = s;
        }
        //投保单详情
        protected void LinkButton6_Click(object sender, EventArgs e)
        {
            t_caozuo.Text = "insureDetail";

            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string transNo = CommonFunc.ConvertDateTimeInt(DateTime.Parse(nowtime)).ToString();//(以后需要修改为渠道商订单号)易流水号，每次请求不能相同

            string insureNo = "15091019569042";//投保单号
            //签名，预签名数据：密钥+渠道商身份标识+交易流水号+方案代码
            string sign = EncryptionHelper.ToMD5(signkey + partnerId + transNo+insureNo, "UTF-8");
            sign = sign.ToLower();

            string s = "{"+
                            "\"transNo\":\""+transNo+"\","+
                            "\"partnerId\":"+partnerId+","+
                            "\"insureNum\":\""+insureNo+"\","+
                            "\"sign\":\""+sign+"\""+
                        "}";

            t_xml.Text = s;
        }
        //确定
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (t_caozuo.Text.Trim() == "")
            {
                Label1.Text = "请填写操作类型";
                return;
            }
            if (t_xml.Text.Trim() == "")
            {
                Label1.Text = "请填写发送内容";
                return;
            }

            string str = new GetUrlData().HttpPostJson("http://channel.hzins.com/api/" + t_caozuo.Text.Trim(), t_xml.Text.Trim());
            Label1.Text = str;
           
        }

       
    }
}