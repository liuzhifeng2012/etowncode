using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.Tenpay.Tenpay;
using ETS2.VAS.Service.VASService.Data;
using ETS.Framework;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS.JsonFactory;

namespace ETS2.WebApp.tenpay
{
    public partial class payNotifyUrl : System.Web.UI.Page
    {
        public string RequestUrl = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Trim().ToLower();
        public int comid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //获取访问的商户COMID
            if (Domain_def.Domain_yanzheng(RequestUrl))//如果符合shop101.etown.cn的格式，则从多微信商户基本信息表中获取comid
            {
                comid = Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl));
            }
            else
            {

                B2b_company_info companyinfo = B2bCompanyData.GetComId(RequestUrl);
                if (companyinfo != null)
                {
                    comid = companyinfo.Com_id;
                }
            }
            #region 获得财付通支付参数 tenpay_id,tenpay_key
            //根据产品判断商家是否含有自己的财付通支付:
            //a.含有的话支付到商家；
            //b.没有的话支付到平台财付通账户(易城账户，公司id=106)
            string tenpay_id = "";
            string tenpay_key = "";
            B2b_finance_paytype model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(comid);
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
           

            //创建ResponseHandler实例
            ResponseHandler resHandler = new ResponseHandler(Context);
            resHandler.setKey(tenpay_key);

            //判断签名
            if (resHandler.isTenpaySign())
            {
                ///通知id
                string notify_id = resHandler.getParameter("notify_id");
                //通过通知ID查询，确保通知来至财付通
                //创建查询请求
                RequestHandler queryReq = new RequestHandler(Context);
                queryReq.init();
                queryReq.setKey(tenpay_key);
                queryReq.setGateUrl("https://gw.tenpay.com/gateway/simpleverifynotifyid.xml");
                queryReq.setParameter("partner", tenpay_id);
                queryReq.setParameter("notify_id", notify_id);

                //通信对象
                TenpayHttpClient httpClient = new TenpayHttpClient();
                httpClient.setTimeOut(5);
                //设置请求内容
                httpClient.setReqContent(queryReq.getRequestURL());
                //后台调用
                if (httpClient.call())
                {
                    //设置结果参数
                    ClientResponseHandler queryRes = new ClientResponseHandler();
                    queryRes.setContent(httpClient.getResContent());
                    queryRes.setKey(tenpay_key);
                    //判断签名及结果
                    //只有签名正确,retcode为0，trade_state为0才是支付成功
                    if (queryRes.isTenpaySign())
                    {
                        //取结果参数做业务处理
                        string out_trade_no = queryRes.getParameter("out_trade_no");
                        //财付通订单号
                        string transaction_id = queryRes.getParameter("transaction_id");
                        //金额,以分为单位
                        string total_fee = queryRes.getParameter("total_fee");
                        //如果有使用折扣券，discount有值，total_fee+discount=原请求的total_fee
                        string discount = queryRes.getParameter("discount");
                        //支付结果
                        string trade_state = resHandler.getParameter("trade_state");
                        //交易模式，1即时到帐 2中介担保
                        string trade_mode = resHandler.getParameter("trade_mode");
                        #region
                        //判断签名及结果
                        if ("0".Equals(queryRes.getParameter("retcode")))
                        {
                            //Response.Write("id验证成功");

                            if ("1".Equals(trade_mode))
                            {       //即时到账 
                                if ("0".Equals(trade_state))
                                {
                                    //------------------------------
                                    //即时到账处理业务开始
                                    //------------------------------

                                    //处理数据库逻辑
                                    //注意交易单不要重复处理
                                    //注意判断返回金额

                                    //------------------------------
                                    //即时到账处理业务完毕
                                    //------------------------------
                                    string retunstr = new PayReturnSendEticketData().PayReturnSendEticket(notify_id, Int32.Parse(out_trade_no), total_fee.ConvertTo<decimal>(), "TRADE_SUCCESS");

                                    //对分销充值订单支付返回，对绑定订单进行处理确认
                                    var handdata = OrderJsonData.agentorderpay_Hand(Int32.Parse(out_trade_no));


                                    //给财付通系统发送成功信息，财付通系统收到此结果后不再进行后续通知
                                    Response.Write("success");
                                }
                                else
                                {
                                    Response.Write("即时到账支付失败");
                                }
                            }
                            else if ("2".Equals(trade_mode))
                            { //中介担保
                                //------------------------------
                                //中介担保处理业务开始
                                //------------------------------
                                //处理数据库逻辑
                                //注意交易单不要重复处理
                                //注意判断返回金额

                                int iStatus = Convert.ToInt32(trade_state);
                                switch (iStatus)
                                {
                                    case 0:		//付款成功

                                        break;
                                    case 1:		//交易创建

                                        break;
                                    case 2:		//收获地址填写完毕

                                        break;
                                    case 4:		//卖家发货成功

                                        break;
                                    case 5:		//买家收货确认，交易成功

                                        break;
                                    case 6:		//交易关闭，未完成超时关闭

                                        break;
                                    case 7:		//修改交易价格成功

                                        break;
                                    case 8:		//买家发起退款

                                        break;
                                    case 9:		//退款成功

                                        break;
                                    case 10:	//退款关闭

                                        break;

                                }


                                //------------------------------
                                //中介担保处理业务开始
                                //------------------------------


                                //给财付通系统发送成功信息，财付通系统收到此结果后不再进行后续通知
                                Response.Write("success");
                            }
                        }
                        else
                        {
                            //错误时，返回结果可能没有签名，写日志trade_state、retcode、retmsg看失败详情。
                            //通知财付通处理失败，需要重新通知
                            Response.Write("查询验证签名失败或id验证失败");
                            Response.Write("retcode:" + queryRes.getParameter("retcode"));
                        }
                        #endregion
                    }
                    else
                    {
                        Response.Write("通知ID查询签名验证失败");
                    }
                }
                else
                {
                    //通知财付通处理失败，需要重新通知
                    Response.Write("后台调用通信失败");
                    //写错误日志
                    Response.Write("call err:" + httpClient.getErrInfo() + "<br>" + httpClient.getResponseCode() + "<br>");

                }
            }
            else
            {
                Response.Write("签名验证失败");
            }
            Response.End();
        }
    }
}