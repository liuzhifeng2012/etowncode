using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using System.Text;
using System.Net;
using System.IO;

namespace ETS2.WebApp.mjld_jk
{
    public partial class _default : System.Web.UI.Page
    {
        private string user = "jkcsdl01";//代理商用户登陆名
        private string password = "123456";//代理商用户登陆密码
        private string businessid = "6e88e52077df4d20bd6d7cf7a90f5a80";//商户ID
        private string des_key = "71debbbf55f64f8b8c604994";//加密Key

        private string interurl = "http://preview1.mjld.com.cn:8022/Outer/Interface/";//接口url

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //2.5 提交订单
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            t_caozuo.Text = "SubmitOrder";

            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string timeStamp = CommonFunc.ConvertDateTimeInt(DateTime.Parse(nowtime)).ToString();//(以后需要修改为渠道商订单号)交易流水号，每次请求不能相同



            string s = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                        "<Body>" +
                            "<timeStamp>" + timeStamp + "</timeStamp>" +
                            "<user>" + user + "</user>" +
                            "<password>" + password + "</password>" +
                            "<goodsId>-商品id-</goodsId>" +
                            "<num>-张数-</num>" + //<!—可以填多个，默认为1 -->  
                            "<phone>-客户电话-</phone>" +
                            "<batch>0</batch>" +//<!-值填1时一码一票，值填0或不填该字段是一码多票>
                            "<guest_name>客户姓名</guest_name>" +
                            "<identityno>客户身份证</identityno>" +
                            "<order_note>订单备注</order_note>" +
                            "<forecasttime></forecasttime>" +//【产品详情里IsReserve=True时，需传递该时间；IsReserve=False时，必须保留该值为空】
                            "<outOrderId>-代理商订单编号-</outOrderId>" +
                            "<orderpost>" +//快递信息
                            "<consignee>收货地址(收货人)</consignee>" +
                            "<address>收货地址(具体地址)</address>" +
                            "<zipcode>收货地址(区号)</zipcode>" +
                            "</orderpost>" +
                        "</Body>";
            t_xml.Text = s;
        }

        //2.6 订单浏览
        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            t_caozuo.Text = "GetOrderDetail";

            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string timeStamp = CommonFunc.ConvertDateTimeInt(DateTime.Parse(nowtime)).ToString();//(以后需要修改为渠道商订单号)易流水号，每次请求不能相同



            string s = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                        "<Body>" +
                          "<timeStamp>" + timeStamp + "</timeStamp>" +
                          "<user>" + user + "</user>" +
                          "<password>" + password + "</password>" +
                          "<orderId>-订单编号-</orderId>" +
                          "<outOrderId>-外部代理商订单号-</outOrderId >" +
                        "</Body>";

            t_xml.Text = s;
        }

        //2.7、	短信重发
        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            t_caozuo.Text = "ReSendSms";

            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string timeStamp = CommonFunc.ConvertDateTimeInt(DateTime.Parse(nowtime)).ToString();//(以后需要修改为渠道商订单号)易流水号，每次请求不能相同


            string s = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                        "<Body>" +
                          "<timeStamp>" + timeStamp + "</timeStamp>" +
                          "<user>" + user + "</user>" +
                          "<password>" + password + "</password>" +
                          "<orderId>订单号</orderId>" +
                          "<credenceno>票码</credenceno>" +
                        "</Body>";

            t_xml.Text = s;
        }
        //2.8、	查询验证码信息
        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            t_caozuo.Text = "GetCodeInfo";

            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string timeStamp = CommonFunc.ConvertDateTimeInt(DateTime.Parse(nowtime)).ToString();//(以后需要修改为渠道商订单号)易流水号，每次请求不能相同

            string s = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                        "<Body>" +
                          "<timeStamp>" + timeStamp + "</timeStamp>" +
                          "<user>" + user + "</user>" +
                          "<password>" + password + "</password>" +
                          "<credenceno>票码</credenceno>" +
                        "</Body>";

            t_xml.Text = s;
        }
        //2.9、	退单
        protected void LinkButton5_Click(object sender, EventArgs e)
        {
            t_caozuo.Text = "BackOrder";

            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string timeStamp = CommonFunc.ConvertDateTimeInt(DateTime.Parse(nowtime)).ToString();//(以后需要修改为渠道商订单号)易流水号，每次请求不能相同



            string s = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                        "<Body>" +
                          "<timeStamp>" + timeStamp + "</timeStamp>" +
                          "<user>" + user + "</user>" +
                          "<password>" + password + "</password>" +
                          "<credenceno>票码</credenceno>" +
                          "<backNum>退单数量</backNum>" +
                          "<outBackId>代理商退单号</outBackId>" +
                        "</Body>";

            t_xml.Text = s;
        }
        //2.10、	订单整单快速退单
        protected void LinkButton6_Click(object sender, EventArgs e)
        {
            t_caozuo.Text = "RefundByOrderID";

            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string timeStamp = CommonFunc.ConvertDateTimeInt(DateTime.Parse(nowtime)).ToString();//(以后需要修改为渠道商订单号)易流水号，每次请求不能相同


            string s = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                        "<Body>" +
                          "<timeStamp>" + timeStamp + "</timeStamp>" +
                          "<user>" + user + "</user>" +
                          "<password>" + password + "</password>" +
                          "<RefundPart>false</RefundPart>" +//true：允许部分退票，false：不允许部分退票
                          "<outBackId>代理商退单号</outBackId>" +
                          "<orderid>订单编号</orderid>" +
                        "</Body>";

            t_xml.Text = s;
        }
        //2.1、	查询地区
        protected void LinkButton7_Click(object sender, EventArgs e)
        {
            t_caozuo.Text = "SelectAreaList";

            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string timeStamp = CommonFunc.ConvertDateTimeInt(DateTime.Parse(nowtime)).ToString();//(以后需要修改为渠道商订单号)易流水号，每次请求不能相同


            string s = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"+
                        "<Body>"+
                           "<timeStamp>"+timeStamp+"</timeStamp>"+
                          //格式<timeStamp>20150402095532</timeStamp>（年月日时分秒）
                          "<user>"+user+"</user>"+
                          "<password>"+password+"</password>"+
                          "<ParentId>上级地区编号(默认0)</ParentId>" +//<!--默认0-->
                         "</Body>";

            t_xml.Text = s;
        }
        //2.2、	查询主题（SelectThemeList）
        protected void LinkButton8_Click(object sender, EventArgs e)
        {
            t_caozuo.Text = "SelectThemeList";

            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string timeStamp = CommonFunc.ConvertDateTimeInt(DateTime.Parse(nowtime)).ToString();//(以后需要修改为渠道商订单号)易流水号，每次请求不能相同


            string s = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"+
                        "<Body>"+
                          "<timeStamp>"+timeStamp+"</timeStamp>"+
                          //格式<timeStamp>20150402095532</timeStamp>（年月日时分秒）
                          "<user>"+user+"</user>"+
                          "<password>"+password+"</password>"+
                        "</Body>";

            t_xml.Text = s;
        }
        //2.3、	查询产品列表（SelectProductList）
        protected void LinkButton9_Click(object sender, EventArgs e)
        {
            t_caozuo.Text = "SelectProductList";

            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string timeStamp = CommonFunc.ConvertDateTimeInt(DateTime.Parse(nowtime)).ToString();//(以后需要修改为渠道商订单号)易流水号，每次请求不能相同


            string s = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"+
                        "<Body>"+
                            "<timeStamp>"+timeStamp+"</timeStamp>"+
                        //格式<timeStamp>20150402095532</timeStamp>（年月日时分秒）
                           "<user>"+user+"</user>"+
                            "<password>"+password+"</password>"+
                            "<PageIndex>当前页</PageIndex>"+
                            "<PageSize>每页数据记录数</PageSize>"+
                            "<goodsName>搜索商品的名称</goodsName>"+
                            "<AreaId>地区Id</AreaId>"+
                            "<AreaName>地区名称</AreaName>"+
                            "<ThemeId>主题Id</ThemeId>"+ 
                        "<SaleType>出售类型(1电子票或2机打票，默认1电子票；0返回所有类型的产品)</SaleType>"+//(1电子票或2机打票，默认1电子票；0返回所有类型的产品)
                        "</Body>";

            t_xml.Text = s;
        }
        //2.4、	查询产品详情（SelectProductInfo）
        protected void LinkButton10_Click(object sender, EventArgs e)
        {
            t_caozuo.Text = "SelectProductInfo";

            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string timeStamp = CommonFunc.ConvertDateTimeInt(DateTime.Parse(nowtime)).ToString();//(以后需要修改为渠道商订单号)易流水号，每次请求不能相同


            string s = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"+
                        "<Body>"+
                          "<timeStamp>"+timeStamp+"</timeStamp>"+
                          "<user>"+user+"</user>"+
                          "<password>"+password+"</password>"+
                          "<goodsCode>商品编号</goodsCode>"+
                        "</Body>";

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

            string data = HttpUtility.UrlEncode(Mjld_TCodeServiceCrypt.Encrypt3DESToBase64(t_xml.Text.Trim(), des_key));
            string postData = string.Format("businessid={1}&content={0}", HttpUtility.UrlEncode(data), businessid);

            string str = POST(interurl + t_caozuo.Text.Trim(), postData);

            try
            {
                string bstr = Mjld_TCodeServiceCrypt.Decrypt3DESFromBase64(str, des_key);

                Label1.Text = bstr;
            }
            catch
            {
                Label1.Text = str;
            } 
        }
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

    }
}