using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.Tenpay.Tenpay;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS.Framework;
using ETS2.VAS.Service.VASService.Data;
using ETS2.VAS.Service.VASService.Modle;


namespace ETS2.WebApp.tenpay
{
    public partial class payRequest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sp_billno = Request["order_no"];
            string product_name = Request["product_name"];
            string order_price = Request["order_price"]; ;
            string remarkexplain = Request["remarkexplain"];
            string bank_type_value = Request["bank_type_value"].ConvertTo<string>("DEFAULT");


            double money = 0;
           
            if (null == Request["order_price"])
            {
                Response.Write("支付金额错误！");
                Response.End();
                return;
            }
            try
            {
                money = Convert.ToDouble(order_price);
            }
            catch
            {
                Response.Write("支付金额格式错误！");
                Response.End();
                return;
            }
            if (null == sp_billno)
            {
                ////生成订单10位序列号，此处用时间和随机数生成，商户根据自己调整，保证唯一
                //sp_billno = DateTime.Now.ToString("HHmmss") + TenpayUtil.BuildRandomStr(4);
                Response.Write("订单编号错误！");
                Response.End();
                return;
            }
            else
            {
                sp_billno =  Request["order_no"].ToString() ;
            }

            //根据订单id得到订单信息
            B2bOrderData dataorder = new B2bOrderData();
            B2b_order modelb2border = dataorder.GetOrderById(sp_billno.ConvertTo<int>(0));
            if (modelb2border == null)
            {
                Response.Write("订单信息获取错误！");
                Response.End();
                return;
            }
            //根据产品id得到产品信息
            B2bComProData datapro = new B2bComProData();
            B2b_com_pro modelcompro = datapro.GetProById(modelb2border.Pro_id.ToString());
            if (modelcompro == null)
            {
                Response.Write("产品信息错误！");
                Response.End();
                return;
            }

            if (modelb2border != null && modelcompro != null)
            {
                //写入支付数据库,先判定是否有此订单支付
                //根据订单id得到订单信息
                B2bPayData datapay = new B2bPayData();
                B2b_pay modelb2pay = datapay.GetPayByoId(sp_billno.ConvertTo<int>(0));

                if (modelb2pay != null)
                {
                    //对已完成支付的，再次提交支付，跳转到订单也或显示此订单已支付
                    if (modelb2pay.Trade_status == "TRADE_SUCCESS")
                    { 
                        Response.Write("订单已经支付过！");
                        Response.End();
                        return;
                    }
                }


              

                #region 获得财付通支付参数 tenpay_id,tenpay_key
                //根据产品判断商家是否含有自己的财付通支付:
                //a.含有的话支付到商家；
                //b.没有的话支付到平台财付通账户(易城账户，公司id=106)


                string tenpay_id = "";
                string tenpay_key = "";
                B2b_finance_paytype model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(modelcompro.Com_id);
                if (model != null)
                {
                    //商家财付通支付的所有参数都存在
                    if (model.Tenpay_id != "" && model.Tenpay_key != "")
                    {
                        tenpay_id = model.Tenpay_id;
                        tenpay_key = model.Tenpay_key;
                    }
                    else
                    {
                        model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(106);
                        if (model == null)
                        {
                            Response.Write("商户财付通信息设置不完全！");
                            Response.End();
                            return;
                        }
                        else
                        {
                            //商家财付通支付的所有参数都存在
                            if (model.Tenpay_id != "" && model.Tenpay_key != "")
                            {
                                tenpay_id = model.Tenpay_id;
                                tenpay_key = model.Tenpay_key;
                            }
                            else
                            {
                                Response.Write("商户财付通信息设置不完全！！");
                                Response.End();
                                return;
                            }
                        }
                    }
                }
                else
                {
                    model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(106);
                    if (model == null)
                    {
                        Response.Write("商户财付通信息设置不完全！");
                        Response.End();
                        return;
                    }
                    else
                    {
                        //商家财付通支付的所有参数都存在
                        if (model.Tenpay_id != "" && model.Tenpay_key != "")
                        {
                            tenpay_id = model.Tenpay_id;
                            tenpay_key = model.Tenpay_key;
                        }
                        else
                        {
                            Response.Write("商户财付通信息设置不完全！！");
                            Response.End();
                            return;
                        }
                    }
                }

                #endregion


                if (modelb2pay == null)
                {
                    B2b_pay eticket = new B2b_pay()
                    {
                        Id = 0,
                        Oid = sp_billno.ConvertTo<int>(0),
                        Pay_com = "mtenpay",
                        Pay_name = modelb2border.U_name,
                        Pay_phone = modelb2border.U_phone,
                        Total_fee = (decimal)modelb2border.Pay_price * modelb2border.U_num + modelb2border.Express - modelb2border.Integral1 - modelb2border.Imprest1,
                        Trade_no = "",
                        Trade_status = "trade_pendpay",
                        Uip = "",
                        comid = model.Com_id
                    };
                    int payid = datapay.InsertOrUpdate(eticket);
                }
                else
                {
                    //对已完成支付的，再次提交支付，跳转到订单也或显示此订单已支付
                    if (modelb2pay.Trade_status != "TRADE_SUCCESS")
                    {
                        //防止金额有所改动
                        modelb2pay.comid = model.Com_id;
                        modelb2pay.Pay_com = "mtenpay";
                        modelb2pay.Total_fee = (decimal)modelb2border.Pay_price * modelb2border.U_num + modelb2border.Express - modelb2border.Integral1 - modelb2border.Imprest1;
                        datapay.InsertOrUpdate(modelb2pay);
                    }
                    else
                    {
                        Response.Write("订单已经支付过！");
                        Response.End();
                        return;
                    }
                }
            

                //创建RequestHandler实例
                RequestHandler reqHandler = new RequestHandler(Context);
                //初始化
                reqHandler.init();
                //设置密钥
                reqHandler.setKey(tenpay_key);
                reqHandler.setGateUrl("https://gw.tenpay.com/gateway/pay.htm");


                //-----------------------------
                //设置支付参数
                //-----------------------------
                reqHandler.setParameter("partner", tenpay_id);		        //商户号
                reqHandler.setParameter("out_trade_no", sp_billno);		//商家订单号
                reqHandler.setParameter("total_fee", (money * 100).ToString());			        //商品金额,以分为单位
                reqHandler.setParameter("return_url", "http://shop" + modelcompro.Com_id.ToString() + ".etown.cn/tenpay/payReturnUrl.aspx");		    //交易完成后跳转的URL
                reqHandler.setParameter("notify_url", "http://shop" + modelcompro.Com_id.ToString() + ".etown.cn/tenpay/payNotifyUrl.aspx");		    //接收财付通通知的URL
                reqHandler.setParameter("body", remarkexplain);	                    //商品描述
                reqHandler.setParameter("bank_type", bank_type_value);		    //银行类型(中介担保时此参数无效)
                reqHandler.setParameter("spbill_create_ip", Page.Request.UserHostAddress);   //用户的公网ip，不是商户服务器IP
                reqHandler.setParameter("fee_type", "1");                    //币种，1人民币
                reqHandler.setParameter("subject", product_name);              //商品名称(中介交易时必填)
                

                //系统可选参数
                reqHandler.setParameter("sign_type", "MD5");
                reqHandler.setParameter("service_version", "1.0");
                reqHandler.setParameter("input_charset", "UTF-8");
                reqHandler.setParameter("sign_key_index", "1");

                //业务可选参数

                reqHandler.setParameter("attach", "");                      //附加数据，原样返回
                reqHandler.setParameter("product_fee", "0");                 //商品费用，必须保证transport_fee + product_fee=total_fee
                reqHandler.setParameter("transport_fee", "0");               //物流费用，必须保证transport_fee + product_fee=total_fee
                reqHandler.setParameter("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));            //订单生成时间，格式为yyyymmddhhmmss
                reqHandler.setParameter("time_expire", "");                 //订单失效时间，格式为yyyymmddhhmmss
                reqHandler.setParameter("buyer_id", "");                    //买方财付通账号
                reqHandler.setParameter("goods_tag", "");                   //商品标记
                reqHandler.setParameter("trade_mode", "1");                 //交易模式，1即时到账(默认)，2中介担保，3后台选择（买家进支付中心列表选择）
                reqHandler.setParameter("transport_desc", "");              //物流说明
                reqHandler.setParameter("trans_type", "1");                  //交易类型，1实物交易，2虚拟交易
                reqHandler.setParameter("agentid", "");                     //平台ID
                reqHandler.setParameter("agent_type", "");                  //代理模式，0无代理(默认)，1表示卡易售模式，2表示网店模式
                reqHandler.setParameter("seller_id", "");                   //卖家商户号，为空则等同于partner

              

                //获取请求带参数的url
                string requestUrl = reqHandler.getRequestURL();

                //获取debug信息,建议把请求和debug信息写入日志，方便定位问题
                string debuginfo = reqHandler.getDebugInfo();
                //Response.Write("<br/>requestUrl:" + requestUrl + "<br/>");
                //Response.Write("<br/>debuginfo:" + debuginfo + "<br/>");

                //Get的实现方式
                string a_link = "<a target=\"_blank\" href=\"" + requestUrl + "\">" + "财付通支付" + "</a>";
                //Response.Write(a_link);
                Response.Redirect(requestUrl);

                //post实现方式

                /*  Response.Write("<form method=\"post\" action=\"" + reqHandler.getGateUrl() + "\" >\n");
                 Hashtable ht = reqHandler.getAllParameters();
                 foreach (DictionaryEntry de in ht)
                 {
                     Response.Write("<input type=\"hidden\" name=\"" + de.Key + "\" value=\"" + de.Value + "\" >\n");
                 }
                 Response.Write("<input type=\"submit\" value=\"财付通支付\" >\n</form>\n");*/
            }
        }
    }
}