using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.Alipay;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS.Framework;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS2.Common.Business;
using ETS2.VAS.Service.VASService.Data;
using ETS2.VAS.Service.VASService.Modle;

namespace ETS2.WebApp.H5
{
    public partial class Payment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int orderid = Request["out_trade_no"].ConvertTo<int>(0);
            //根据订单id得到订单信息
            B2bOrderData dataorder = new B2bOrderData();
            B2b_order modelb2border = dataorder.GetOrderById(orderid);

            //根据产品id得到产品信息
            B2bComProData datapro = new B2bComProData();
            B2b_com_pro modelcompro = datapro.GetProById(modelb2border.Pro_id.ToString());

            ////////////////////////////////////////////请求参数////////////////////////////////////////////

            //必填参数//

            //请与贵网站订单系统中的唯一订单号匹配
            string out_trade_no = orderid.ToString();
            //订单名称，显示在支付宝收银台里的“商品名称”里，显示在支付宝的交易管理的“商品名称”的列表里。
            string subject = modelcompro.Pro_name;
            //订单描述、订单详细、订单备注，显示在支付宝收银台里的“商品描述”里
            string body = "pay";
            //订单总金额，显示在支付宝收银台里的“应付总额”里
            string total_fee = String.Format("{0:N2}", (modelb2border.U_num * modelb2border.Pay_price));//



            //扩展功能参数——默认支付方式//

            //默认支付方式，代码见“即时到帐接口”技术文档
            string paymethod = Request["paymethod"];
            //默认网银代号，代号列表见“即时到帐接口”技术文档“附录”→“银行列表”
            string defaultbank = Request["defaultbank"];

            //扩展功能参数——防钓鱼//

            //防钓鱼时间戳
            string anti_phishing_key = "";
            //获取客户端的IP地址，建议：编写获取客户端IP地址的程序
            string exter_invoke_ip = "";
            //注意：
            //请慎重选择是否开启防钓鱼功能
            //exter_invoke_ip、anti_phishing_key一旦被设置过，那么它们就会成为必填参数
            //建议使用POST方式请求数据
            //示例：
            //exter_invoke_ip = "";
            //Service aliQuery_timestamp = new Service();
            //anti_phishing_key = aliQuery_timestamp.Query_timestamp();               //获取防钓鱼时间戳函数

            //扩展功能参数——其他//

            //商品展示地址，要用http:// 格式的完整路径，不允许加?id=123这类自定义参数
            string show_url = "";
            //自定义参数，可存放任何内容（除=、&等特殊字符外），不会显示在页面上
            string extra_common_param = "";
            //默认买家支付宝账号
            string buyer_email = "";

            //扩展功能参数——分润(若要使用，请按照注释要求的格式赋值)//

            //提成类型，该值为固定值：10，不需要修改
            string royalty_type = "";
            //提成信息集
            string royalty_parameters = "";
            //注意：
            //与需要结合商户网站自身情况动态获取每笔交易的各分润收款账号、各分润金额、各分润说明。最多只能设置10条
            //各分润金额的总和须小于等于total_fee
            //提成信息集格式为：收款方Email_1^金额1^备注1|收款方Email_2^金额2^备注2
            //示例：
            //royalty_type = "10";
            //royalty_parameters = "111@126.com^0.01^分润备注一|222@126.com^0.01^分润备注二";

            ////////////////////////////////////////////////////////////////////////////////////////////////

            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("payment_type", "1");
            sParaTemp.Add("show_url", show_url);
            sParaTemp.Add("out_trade_no", out_trade_no);
            sParaTemp.Add("subject", subject);
            sParaTemp.Add("body", body);
            sParaTemp.Add("total_fee", total_fee);
            sParaTemp.Add("paymethod", paymethod);
            sParaTemp.Add("defaultbank", defaultbank);
            sParaTemp.Add("anti_phishing_key", anti_phishing_key);
            sParaTemp.Add("exter_invoke_ip", exter_invoke_ip);
            sParaTemp.Add("extra_common_param", extra_common_param);
            sParaTemp.Add("buyer_email", buyer_email);
            sParaTemp.Add("royalty_type", royalty_type);
            sParaTemp.Add("royalty_parameters", royalty_parameters);
            sParaTemp.Add("default_login", "Y");


            //写入支付数据库,先判定是否有此订单支付
            //根据订单id得到订单信息
            B2bPayData datapay = new B2bPayData();
            B2b_pay modelb2pay = datapay.GetPayByoId(orderid);


            if (modelb2pay == null)
            {
                B2b_pay eticket = new B2b_pay()
                {
                    Id = 0,
                    Oid = orderid,
                    Pay_com = "alipay",
                    Pay_name = modelb2border.U_name,
                    Pay_phone = modelb2border.U_phone,
                    Total_fee = (decimal)modelb2border.Pay_price * modelb2border.U_num,
                    Trade_no = "",
                    Trade_status = "trade_pendpay",
                    Uip = ""
                };
                int payid = datapay.InsertOrUpdate(eticket);
            }
            else
            {
                //对已完成支付的，再次提交支付，跳转到订单也或显示此订单已支付
                if (modelb2pay.Trade_status != "TRADE_SUCCESS")
                {
                    //防止金额有所改动
                    modelb2pay.Total_fee = (decimal)modelb2border.Pay_price * modelb2border.U_num;
                    datapay.InsertOrUpdate(modelb2pay);
                }
                else
                {

                }



            }


            //构造即时到帐接口表单提交HTML数据，无需修改
            Service ali = new Service();
            string sHtmlText = ali.Create_direct_pay_by_user(sParaTemp);
            Response.Write(sHtmlText);
        }
    }
}