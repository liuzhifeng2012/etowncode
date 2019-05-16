using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using ETS.Framework;
using ETS2.PM.Service.Meituan.Model;
using ETS2.PM.Service.Meituan.Data;
using Newtonsoft.Json;
using ETS2.PM.Service.PMService.Modle;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.PM.Service.PMService.Data;
using ETS.JsonFactory;
using System.Xml;
using ETS2.PM.Service.PMService.Modle.Enum;

namespace ETS2.WebApp.Meituan_jk
{
    public partial class orderpay : System.Web.UI.Page
    {
        private static object lockobj = new object();

        private Agent_company agentinfo;
        readonly string _requestParam;

        public orderpay()
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

            #region 记入日志表1 Meituan_reqlog
            string reqip = CommonFunc.GetRealIP();
            Meituan_reqlog mlog = new Meituan_reqlog
            {
                id = 0,
                reqstr = _requestParam,
                subtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                respstr = "",
                resptime = "",
                code = "",
                describe = "",
                req_type = "",
                sendip = reqip,
                stockagentcompanyid = 0
            };
            int logid = new Meituan_reqlogData().EditReqlog(mlog);
            mlog.id = logid;

            #endregion

            string date = System.Web.HttpContext.Current.Request.Headers.Get("Date");
            string PartnerId = System.Web.HttpContext.Current.Request.Headers.Get("PartnerId");
            string Authorization = System.Web.HttpContext.Current.Request.Headers.Get("Authorization");
            string requestMethod = System.Web.HttpContext.Current.Request.HttpMethod;
            string URI = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath;
            //authorization 形式: "MWS" + " " + client + ":" + sign;
            string mtSign = Authorization.Substring(Authorization.IndexOf(":") + 1);

            mlog.req_type = URI;

            agentinfo = new AgentCompanyData().GetAgentCompanyByMeituanPartnerId(PartnerId);
            #region 验证是否已经配置美团合作商信息
            if (agentinfo == null)
            {
                var response = new OrderPayResponse();
                response.partnerId = int.Parse(PartnerId);
                response.code = 599;
                response.describe = "分销尚未配置美团合作商信息(合作商PartnerId:" + PartnerId + ")";

                string json = JsonConvert.SerializeObject(response);

                //把处理结果录入日志
                mlog.respstr = json;
                mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                mlog.code = response.code.ToString();
                mlog.describe = response.describe;
                new Meituan_reqlogData().EditReqlog(mlog);

                LogHelper.RecordSelfLog("Error", "meituan", response.describe);

                Response.Write(json);
                return;
            }
            #endregion

            mlog.stockagentcompanyid = agentinfo.Id;

            #region 签名验证
            string beforeSign = requestMethod + " " + URI + "\n" + date;
            string afterSign = new MeiTuanInter(agentinfo.mt_partnerId, agentinfo.mt_secret, agentinfo.mt_client).GetSign(beforeSign);
            //判断签名是否正确
            if (afterSign != mtSign)
            {
                var response = new OrderPayResponse();
                response.partnerId = int.Parse(PartnerId);
                response.code = 599;
                response.describe = "签名验证失败";

                string json = JsonConvert.SerializeObject(response);

                //把处理结果录入日志
                mlog.respstr = json;
                mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                mlog.code = response.code.ToString();
                mlog.describe = response.describe;
                new Meituan_reqlogData().EditReqlog(mlog);

                LogHelper.RecordSelfLog("Error", "meituan", "拉取产品 签名错误 mtSign-" + mtSign + "  meSign-" + afterSign);

                Response.Write(json);
                return;
            }
            #endregion

            string actionResult = Getorderpay(mlog);

            Response.Write(actionResult);


        }

        ///     订单支付
        public string Getorderpay(Meituan_reqlog mlog)
        {
            lock (lockobj)
            {
                var response = new OrderPayResponse();
                response.partnerId = int.Parse(agentinfo.mt_partnerId);
                try
                {
                    int organization = agentinfo.Id;
                    #region 把分销商发送过来的请求记入数据库日志表2  agent_requestlog
                    Agent_requestlog reqlog = new Agent_requestlog()
                    {
                        Id = 0,
                        Organization = organization,
                        Encode_requeststr = "",
                        Decode_requeststr = _requestParam,
                        Request_time = DateTime.Now,
                        Encode_returnstr = "",
                        Decode_returnstr = "",
                        Return_time = DateTime.Parse("1970-01-01 00:00:00"),
                        Errmsg = "",
                        Request_type = "add_order",
                        Req_seq = "",
                        Ordernum = "",
                        Is_dealsuc = 0,
                        Is_second_receivereq = 0,
                        Request_ip = CommonFunc.GetRealIP()
                    };
                    int reqlogid = new Agent_requestlogData().Editagent_reqlog(reqlog);
                    reqlog.Id = reqlogid;
                    #endregion


                    var data = JsonConvert.DeserializeObject<OrderPayRequest>(_requestParam);
                    if (data.body == null)
                    {
                        response.code = 599;
                        response.describe = "Body数据解析失败";
                        return EditMTlog_Err(response, mlog, reqlog);
                    }
                    else
                    {
                        OrderPayRequestBody body = data.body;

                        string mtorderid = body.orderId.ToString();
                        //如果合作商订单Id不存在，则查询美团订单创建日志
                        if (body.partnerOrderId == "")
                        {
                            Meituan_reqlog mtOrderCrateSucLog = new Meituan_reqlogData().GetMtOrderCrateLogByMtorder(body.orderId.ToString(), "200");
                            body.partnerOrderId = mtOrderCrateSucLog.ordernum;
                        }
                        string order_num = body.partnerOrderId; 
                        mlog.mtorderid = body.orderId.ToString();


                        //日志表2 判断是否是二次发送相同的请求
                        int is_secondreq = new Agent_requestlogData().Is_secondreq(organization.ToString(), mtorderid, reqlog.Request_type);

                        #region 把发送的请求类型，请求流水号，订单号,是否是二次发送相同的请求录入数据库 日志表2
                        reqlog.Req_seq = mtorderid;
                        reqlog.Ordernum = order_num;
                        reqlog.Is_second_receivereq = is_secondreq;
                        new Agent_requestlogData().Editagent_reqlog(reqlog);
                        #endregion

                        #region 编辑日志表1
                        mlog.mtorderid = mtorderid;
                        mlog.ordernum = order_num;
                        mlog.issecond_req = is_secondreq;
                        new Meituan_reqlogData().EditReqlog(mlog);
                        #endregion

                        if (is_secondreq == 1)
                        {
                            //获取处理成功的请求信息：如果没有则重新提单
                            Agent_requestlog suclog = new Agent_requestlogData().GetAgent_addorderlogByReq_seq(organization.ToString(), mtorderid, 1);
                            if (suclog != null)
                            {
                                //return suclog.Decode_returnstr;
                                Meituan_reqlog mtlogg = new Meituan_reqlogData().GetMeituan_Orderpayreqlog(mtorderid, "200");
                                if (mtlogg != null)
                                {
                                    response = (OrderPayResponse)JsonConvert.DeserializeObject(mtlogg.respstr, typeof(OrderPayResponse));

                                    reqlog.Is_dealsuc = 1;
                                    //reqlog.Ordernum = suclog.Ordernum;

                                    mlog.ordernum = suclog.Ordernum;

                                    return EditMTlog_Err(response, mlog, reqlog);
                                }
                                else
                                {
                                    response.code = 502;
                                    response.describe = "查询美团支付成功日志失败";
                                    return EditMTlog_Err(response, mlog, reqlog);
                                }
                            }
                            else
                            {
                                return pay_order(reqlog, body, organization.ToString(), mlog);
                            }
                        }
                        else
                        {
                            return pay_order(reqlog, body, organization.ToString(), mlog);
                        }

                    }
                }
                catch (Exception ex)
                {
                    response.code = 599;
                    response.describe = "异常错误";

                    return EditMTlog_Err(response, mlog, null);
                }
            }
        }
        //编辑美团日志
        public string EditMTlog_Err(OrderPayResponse response, Meituan_reqlog mlog, Agent_requestlog reqlog)
        {
            string json = JsonConvert.SerializeObject(response);

            #region 把处理结果录入数据库
            mlog.respstr = json;
            mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            mlog.code = response.code.ToString();
            mlog.describe = response.describe;
            new Meituan_reqlogData().EditReqlog(mlog);
            #endregion

            #region  把处理结果录入分销接口通用日志表
            if (reqlog != null)
            {
                reqlog.Errmsg = json;
                reqlog.Decode_returnstr = json;
                reqlog.Encode_returnstr = "";
                reqlog.Return_time = DateTime.Now;

                new Agent_requestlogData().Editagent_reqlog(reqlog);
            }
            #endregion

            return json;
        }


        private string pay_order(Agent_requestlog reqlog, OrderPayRequestBody body, string organization, Meituan_reqlog mlog)
        {
            var response = new OrderPayResponse();
            response.partnerId = int.Parse(agentinfo.mt_partnerId);

            string mtorderid = body.orderId.ToString();
            string partnerOrderId = body.partnerOrderId;
            ////如果合作商订单Id不存在，则查询美团订单创建日志
            //if(partnerOrderId=="")
            //{
            //    Meituan_reqlog mtOrderCrateSucLog = new Meituan_reqlogData().GetMtOrderCrateLogByMtorder(body.orderId.ToString(), "200");
            //    partnerOrderId = mtOrderCrateSucLog.ordernum;   
            //}

            string data = OrderJsonData.agentorder_shoudongchuli(partnerOrderId.ConvertTo<int>(0));
            XmlDocument retdoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + data + "}");
            XmlElement retroot = retdoc.DocumentElement;
            string type = retroot.SelectSingleNode("type").InnerText;
            string msg = retroot.SelectSingleNode("msg").InnerText;
            if (type == "100")//创建订单成功
            {
                string pno = retroot.SelectSingleNode("pno").InnerText; 

                response.code = 200;
                response.describe = "success";
                response.body = new OrderPayResponseBody
                {
                    orderId=body.orderId,
                    partnerOrderId = partnerOrderId, 
                    voucherType = (int)Meituan_voucherType.multiuse,
                    asyReturnVoucher = false,
                    vouchers = new string[]{pno},
                    voucherPics=new string[]{}
                };


                reqlog.Is_dealsuc = 1;
                reqlog.Ordernum = partnerOrderId;

                mlog.ordernum = partnerOrderId;

                return EditMTlog_Err(response, mlog, reqlog);
            }
            else//创建订单失败
            {

                if (msg == "预付款不足")
                {
                    response.code = 501;
                    response.describe =  msg;
                }
                else
                {
                    response.code = 599;
                    response.describe = "支付订单失败:" + msg;
                }
                return EditMTlog_Err(response, mlog, reqlog);
            }
        }

    }
}