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

namespace ETS2.WebApp.lvmama
{
    public partial class apply_code : System.Web.UI.Page
    {delegate void AsyncsendsmsEventHandler(asynnoticecallback asynnoticecalljson, Agent_company agentinfo);//异步发送审核通知

             private static object lockobj = new object();

        private Agent_company agentinfo;
        string uid = "";
        string password = "";
        readonly string _requestParam;

        public apply_code()
        {
            _requestParam = GetRequestStreamString();
        }

        public string GetRequestStreamString()
        {
            StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Request.InputStream);
            return reader.ReadToEnd();
        }


        protected void Page_Load(object sender, EventArgs e)
        {


            #region 记入日志表LVmama_reqlog
            string reqip = CommonFunc.GetRealIP();
            Lvmama_reqlog mlog = new Lvmama_reqlog
            {
                id = 0,
                reqstr = _requestParam,
                subtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                respstr = "",
                resptime = "",
                code = "",
                describe = "",
                req_type = "apply_code",
                sendip = reqip,
                stockagentcompanyid = 0
            };
            int logid = new lvmama_reqlogData().EditReqlog(mlog);
            mlog.id = logid;
            #endregion


            
            var data = JsonConvert.DeserializeObject<apply_codemodel>(_requestParam);

            if (data == null) {
                return;
            }
            try
            {
                uid = data.uid;
                password = data.password;
                string serialNo = data.serialNo;
                string sign = data.sign;


                agentinfo = new AgentCompanyData().GetAgentCompanyByLvmamaPartnerId(uid);

                LVMAMA_Data lvmamadata = new LVMAMA_Data(agentinfo.Lvmama_uid, agentinfo.Lvmama_password, agentinfo.Lvmama_Apikey);


                #region 验证是否已经配置驴妈妈合作商信息
                if (agentinfo == null)
                {
                    var response = new apply_codeRefund();
                    response.uid = uid;
                    response.orderId = "0";
                    response.status = "2";
                    response.msg = "分销尚未配置驴妈妈商信息(uid:" + uid + ")";

                    string json = JsonConvert.SerializeObject(response);

                    //把处理结果录入日志
                    mlog.respstr = "";
                    mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    mlog.code = response.status;
                    mlog.describe = response.msg;
                    new lvmama_reqlogData().EditReqlog(mlog);
                    LogHelper.RecordSelfLog("Error", "lvmama", response.msg);
                    Response.Write(json);
                    return;
                }
                #endregion

                mlog.stockagentcompanyid = agentinfo.Id;
                mlog.mtorderid = serialNo;


                #region 签名验证
                string Md5Sign = lvmamadata.lumamamd5(data);
                string afterSign = lvmamadata.lumamasign(Md5Sign, agentinfo.Lvmama_Apikey);

                //判断签名是否正确
                if (afterSign != sign)
                {
                    var response = new apply_codeRefund();
                    response.uid = uid;
                    response.orderId = "0";
                    response.status = "2";
                    response.msg = "签名认证失败";

                    string json = JsonConvert.SerializeObject(response);

                    //把处理结果录入日志
                    mlog.respstr = "";
                    mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    mlog.code = response.status;
                    mlog.describe = response.msg;

                    new lvmama_reqlogData().EditReqlog(mlog);

                    LogHelper.RecordSelfLog("Error", "lvmama", "签名错误 lvmamamSign-" + sign + "  meSign-" + afterSign);

                    Response.Write(json);
                    return;
                }
                #endregion

                string actionResult = Getordercreate(mlog, agentinfo);

                Response.Write(actionResult);
            }
            catch (Exception ex)
            {
                // Response.Write(ex);
            }
        }

        #region 产品订单创建
        public string Getordercreate(Lvmama_reqlog mlog, Agent_company agentinfo)
        {
            lock (lockobj)
            {
                var response = new apply_codeRefund();
                try
                {
                    var data = JsonConvert.DeserializeObject<apply_codemodel>(_requestParam);
                    if (data.contacts == null)
                    {
                        response.uid = agentinfo.Lvmama_uid;
                        response.status = "2";
                        response.msg = "数据解析失败";
                        return EditLvmamalog_Order(response, mlog);
                    }
                    else
                    {
                        contacts body = data.contacts;

                        string num = data.num;
                        string timestamp = data.timestamp;
                        string visitTime = data.visitTime;
                        string supplierGoodsId = data.supplierGoodsId;
                        string settlePrice = data.settlePrice;
                        string serialNo = data.serialNo;
                        string idNum = body.idNum;
                        string name = body.name;
                        string mobile = body.mobile;
                        string idType = body.idType;

                        //结算价核对
                        decimal jiesuanjia=0;

                        ////todo 根据请求参数查询产品返回结果
                        B2b_com_pro pro = new B2bComProData().GetProById(supplierGoodsId);
                        #region 产品信息
                        if (pro != null)
                        {

                            #region 分销授权信息判断
                            Agent_company agentwarrantinfo = AgentCompanyData.GetAgentWarrant(agentinfo.Id, pro.Com_id);

                            if (agentwarrantinfo != null)
                            {
                                int warrantid = agentwarrantinfo.Warrantid;
                                int Warrant_type = agentwarrantinfo.Warrant_type;//支付类型分销 1出票扣款 2验码扣款 
                                int Warrant_level = agentwarrantinfo.Warrant_level;

                                if(Warrant_level==1){
                                    jiesuanjia=pro.Agent1_price;
                                }
                                if(Warrant_level==2){
                                    jiesuanjia=pro.Agent2_price;
                                }

                                if(Warrant_level==3){
                                    jiesuanjia=pro.Agent3_price;
                                }



                                if (agentwarrantinfo.Warrant_state == 0)
                                {
                                    response.uid = agentinfo.Lvmama_uid;
                                    response.orderId = "0";
                                    response.status = "3";
                                    response.msg = "商户关闭授权";
                                    return EditLvmamalog_Order(response, mlog);
                                }
                            }
                            else
                            {
                                 response.uid = agentinfo.Lvmama_uid;
                                 response.orderId = "0";
                                 response.status = "3";
                                 response.msg = "商户未授权开通分销";
                                return EditLvmamalog_Order(response, mlog);
                            }
                            #endregion

                            #region 暂时对外接口只支持票务产品
                            //if (pro.Server_type != 1)
                            //{
                            //    response.status = 300;
                            //    response.msg = "暂时对外接口只支持票务产品，其他产品请到分销后台提单";
                            //    return EditLvmamalog_Order(response, mlog);
                            //}
                            #endregion

                            #region 结算价，保证价格是最新价格
                            decimal advice_price =  Convert.ToDecimal(settlePrice);
                            if (jiesuanjia != advice_price)
                            {
                                response.uid = agentinfo.Lvmama_uid;
                                response.orderId = "0";
                                response.status = "3";
                                response.msg = "结算价格不同，请联系供应商要去新的价格。驴妈妈价："+advice_price+" 系统价："+jiesuanjia;
                                return EditLvmamalog_Order(response, mlog);
                            }
                            #endregion

 

                            #region 产品编码格式有误
                            if (supplierGoodsId.ConvertTo<int>(0) == 0)
                            {
                                response.uid = agentinfo.Lvmama_uid;
                                response.orderId = "0";
                                response.status = "3";
                                response.msg = "产品编码格式有误";
                                return EditLvmamalog_Order(response, mlog);
                            }
                            #endregion

                            #region  购买数量格式有误
                            if (num.ConvertTo<int>(0) == 0)
                            {
                                response.uid = agentinfo.Lvmama_uid;
                                response.orderId = "0";
                                response.status = "3";
                                response.msg = "购买数量格式有误";
                                return EditLvmamalog_Order(response, mlog);
                            }
                            #endregion

                            #region  产品限购则需要判断 限购数量 是否足够
                            if (pro.Ispanicbuy != 0)
                            {
                                //最多可购买数量
                                int zuiduo_canbuynum = pro.Limitbuytotalnum;
                                if (int.Parse(num) > zuiduo_canbuynum)
                                {
                                    response.uid = agentinfo.Lvmama_uid;
                                    response.orderId = "0";
                                    response.status = "3";
                                    response.msg = "产品库存不足";
                                    return EditLvmamalog_Order(response, mlog);
                                }
                            }
                            #endregion

                            #region 产品已暂停
                            if (pro.Pro_state == 0)
                            {
                                response.uid = agentinfo.Lvmama_uid;
                                response.orderId = "0";
                                response.status = "3";
                                response.msg = "产品已下线，请联系商家";
                                return EditLvmamalog_Order(response, mlog);
                            }
                            #endregion

                            #region 产品已过期
                            if (pro.Pro_end < DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
                            {
                                response.uid = agentinfo.Lvmama_uid;
                                response.orderId = "0";
                                response.status = "3";
                                response.msg = "产品已过期";
                                return EditLvmamalog_Order(response, mlog);
                            }
                            #endregion

                            #region   产品是否需要预约:需要预约则最晚预约时间是 游玩前一天的18点
                            //if (pro.isneedbespeak == 1)
                            //{
                            //    if (timestamp != "")
                            //    {
                            //        DateTime visitdate = DateTime.Parse(timestamp);
                            //        DateTime bookdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

                            //        //必须提前一天预约
                            //        if (bookdate >= visitdate)
                            //        {
                            //             response.orderId = "0";
                                         //response.status = "300";
                                         //response.msg = "产品未预约产品";
                            //            return EditLvmamalog_Order(response, mlog);
                            //        }
                            //    }
                            //}
                            #endregion

                            #region  是否有使用限制
                            //if (pro.Iscanuseonsameday == 0)//1:当天出票可用 ;2:2小时内出票不可用;0:当天出票不可用
                            //{
                            //    if (body.travelDate != "")
                            //    {
                            //        DateTime visitdate = DateTime.Parse(body.travelDate);//游玩日期:2012-12-12 格式要求：yyyy-MM-dd

                            //        DateTime bookdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                            //        if (bookdate >= visitdate)
                            //        {
                            //            response.code = 300;
                            //            response.describe = "预定日期至少在游玩日期之前一天";
                            //            return EditMTlog_Order(response, mlog);
                            //        }
                            //    }

                            //}
                            #endregion


                            
                             
                             Lvmama_reqlog mtOrderCrateSucLog = new lvmama_reqlogData().GetLvmamaOrderCreateLogByLvmamaorder(serialNo,"0");
                             #region 查询驴妈妈吗 是否 创建订单日志
                             if (mtOrderCrateSucLog!=null)
                            {


                                string pno="";
                                if (pro.Source_type == 3)//如果是接口产品，按接口方式读码selservice，接口产品只读取了wl的码
                                {
                                    if (pro.Serviceid == 4)
                                    { //如果是接口产品
                                        B2b_company commanage = B2bCompanyData.GetAllComMsg(pro.Com_id);
                                        WlGetProInfoDealRequestData wldata = new WlGetProInfoDealRequestData(commanage.B2bcompanyinfo.wl_PartnerId, commanage.B2bcompanyinfo.wl_userkey);
                                        var wlorderinfo = wldata.SearchWlOrderData(pro.Com_id, 0, "", int.Parse(mtOrderCrateSucLog.ordernum));
                                        if (wlorderinfo != null)
                                        {
                                            pno = wlorderinfo.vouchers;
                                        }
                                    }
                                }
                                else
                                {//如果不是借口，则按自己规则读码
                                    SendEticketData sendate = new SendEticketData();
                                    pno = sendate.HuoQuEticketPno(int.Parse(mtOrderCrateSucLog.ordernum));
                                }

                                response.uid = agentinfo.Lvmama_uid;
                                response.orderId = mtOrderCrateSucLog.ordernum;
                                response.status = "0";
                                response.authCode = pno;
                                response.serialNo = serialNo;
                                response.msg = "订单创建成功";
                                mlog.ordernum = mtOrderCrateSucLog.ordernum;
                                return EditLvmamalog_Order(response, mlog);
                            }
                            #endregion
                            #region 此驴妈妈订单号 ，没有成功创建过
                            else 
                            {
                                int isInterfaceSub = 1;//是否是电子票接口提交的订单:0.否;1.是
                                string ordertype = "1";//1.出票2.充值
                                int orderid = 0;
                                int speciid=0;
                                //string real_name = "";
                                //string mobile = "";
                                 
                                string use_date=timestamp.Substring(0,4)+"-"+timestamp.Substring(4,2)+"-"+timestamp.Substring(6,2);

                                //real_name = name;
                                //mobile = mobile;

                                //创建一笔未支付订单
                                string rdata = OrderJsonData.AgentOrder(agentinfo.Id, supplierGoodsId, ordertype, num, name, mobile, use_date,"", isInterfaceSub, out orderid);
                                XmlDocument retdoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + rdata + "}");
                                XmlElement retroot = retdoc.DocumentElement;
                                string rtype = retroot.SelectSingleNode("type").InnerText;
                                string rmsg = retroot.SelectSingleNode("msg").InnerText;
                                if (rtype == "100")//创建订单成功
                                {
                                    response.uid = agentinfo.Lvmama_uid;
                                    response.status = "0";
                                    response.msg = "订单创建成功";
                                    response.orderId = orderid.ToString();
                                    response.serialNo = serialNo;
                                    response.authCode = retroot.SelectSingleNode("pno").InnerText; //只有成功的订单才有pno项;
                                    mlog.ordernum = orderid.ToString();
                                    return EditLvmamalog_Order(response, mlog);
                                }
                                else
                                {
                                    response.uid = agentinfo.Lvmama_uid;
                                    response.status = "3";
                                    response.msg = "创建订单失败" + rmsg;
                                    return EditLvmamalog_Order(response, mlog);
                                }
                            }
                            #endregion 

                        }
                        else
                        {
                            response.uid = agentinfo.Lvmama_uid;
                            response.uid = "0";
                            response.status = "4";
                            response.msg = "产品不存在";
                            return EditLvmamalog_Order(response, mlog);
                        }
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    response.uid = agentinfo.Lvmama_uid;
                    response.status = "8";
                    response.msg = "异常错误" + ex;

                    return EditLvmamalog_Order(response, mlog);
                }
            }

        }
         #endregion

        //编辑日志
        public string EditLvmamalog_Order(apply_codeRefund response, Lvmama_reqlog mlog)
        {
            response.codeURL = "";

            string json = JsonConvert.SerializeObject(response);

            #region 把处理结果录入数据库
            mlog.respstr = json;
            mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            mlog.code = response.status;
            mlog.ordernum = response.orderId;
            mlog.describe = response.msg;
            mlog.mtorderid=response.serialNo;
            mlog.stockagentcompanyid = mlog.stockagentcompanyid;
            new lvmama_reqlogData().EditReqlog(mlog);
            #endregion


            //驴妈妈，对供应商单独审核订单，才需要异步提交
            //Agent_company agentinfo = new AgentCompanyData().GetAgentCompanyByLvmamaPartnerId(response.uid);
            //if (agentinfo != null)
            //{
            //    var lvmamadata = new LVMAMA_Data(agentinfo.Lvmama_uid, agentinfo.Lvmama_password, agentinfo.Lvmama_Apikey);

               

            //    //初始的时候没有sign值，等组合后下面生成加密文件
            //    var asynnoticecalljson = lvmamadata.asyConsumeNotify_json(response.serialNo, agentinfo.Lvmama_uid, agentinfo.Lvmama_password, response.status, "", response.authCode, "", mlog.ordernum);
               
                
            //    #region 签名验证
            //    string Md5Sign = lvmamadata.asynnoticecallbackmd5(asynnoticecalljson);
            //    string afterSign = lvmamadata.lumamasign(Md5Sign, agentinfo.Lvmama_Apikey);
            //    #endregion

            //    asynnoticecalljson.sign = afterSign;

            //    //异步通知
            //    AsyncsendsmsEventHandler mydelegate = new AsyncsendsmsEventHandler(AsyncSendSms);
            //    mydelegate.BeginInvoke(asynnoticecalljson, agentinfo, new AsyncCallback(CompletedSendSms), null);
            //}
            return json;
        }


        #region 异步发送审核

        public static void CompletedSendSms(IAsyncResult result)
        {
            //获取委托对象，调用EndInvoke方法获取运行结果
            AsyncResult _result = (AsyncResult)result;
            AsyncsendsmsEventHandler myDelegate = (AsyncsendsmsEventHandler)_result.AsyncDelegate;

            myDelegate.EndInvoke(_result);
        }


        public static void AsyncSendSms(asynnoticecallback asynnoticecalljson, Agent_company agentinfo)
        {


            var lvmamadata = new LVMAMA_Data(agentinfo.Lvmama_uid, agentinfo.Lvmama_password, agentinfo.Lvmama_Apikey);

            var asynnoticecall = lvmamadata.asyConsumeNotify(asynnoticecalljson, agentinfo.Id);

        }

        #endregion

    
    }
    
}