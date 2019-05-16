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
    public partial class orderrefund : System.Web.UI.Page
    {
        private Agent_company agentinfo;
        readonly string _requestParam;

        public orderrefund()
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
                var response = new OrderCancelResponse();
                response.partnerId = int.Parse(PartnerId);
                response.code = 699;
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
                var response = new OrderCancelResponse();
                response.partnerId = int.Parse(PartnerId);
                response.code = 669;
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

            string actionResult = Getorderrefund(mlog);
            Response.Write(actionResult);

        }

        ///     订单退票
        public string Getorderrefund(Meituan_reqlog mlog)
        {
            var response = new OrderCancelResponse();
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
                    Request_type = "cancel_order",
                    Req_seq = "",
                    Ordernum = "",
                    Is_dealsuc = 0,
                    Is_second_receivereq = 0,
                    Request_ip = CommonFunc.GetRealIP()
                };
                int reqlogid = new Agent_requestlogData().Editagent_reqlog(reqlog);
                reqlog.Id = reqlogid;
                #endregion


                var data = JsonConvert.DeserializeObject<OrderCancelRequest>(_requestParam);
                if (data.body == null)
                {
                    response.code = 699;
                    response.describe = "数据解析失败";
                    return EditMTlog_Err(response, mlog, reqlog);
                }
                else
                {
                    OrderCancelRequestBody body = data.body;

                    string mtorderid = body.orderId.ToString();
                    string order_num = body.partnerOrderId;
                    int num = body.refundQuantity;


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
                        //获取处理成功的请求信息：如果没有则重新操作
                        Agent_requestlog suclog = new Agent_requestlogData().GetAgent_cancelorderlogByReq_seq(organization.ToString(), mtorderid, 1);
                        if (suclog != null)
                        {
                            //return suclog.Decode_returnstr;
                            response = (OrderCancelResponse)JsonConvert.DeserializeObject(suclog.Decode_returnstr, typeof(OrderCancelResponse));

                            reqlog.Is_dealsuc = 1;
                            //reqlog.Ordernum = suclog.Ordernum;

                            mlog.ordernum = suclog.Ordernum;

                            return EditMTlog_Err(response, mlog, reqlog);
                        }
                        else
                        {
                            return Cancel_order(reqlog, body, organization.ToString(), mlog);
                        }
                    }
                    else
                    {
                        return Cancel_order(reqlog, body, organization.ToString(), mlog);
                    }

                }
            }
            catch (Exception ex)
            {
                response.code = 699;
                response.describe = "异常错误";

                return EditMTlog_Err(response, mlog, null);
            }

        }
        //编辑美团日志
        public string EditMTlog_Err(OrderCancelResponse response, Meituan_reqlog mlog, Agent_requestlog reqlog)
        {

            //对历史订单如果有返回300的，美团没有300值进行修改
            if (response.code == 300)
            {
                response.code = 699;
            }
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


        private string Cancel_order(Agent_requestlog reqlog, OrderCancelRequestBody body, string organization, Meituan_reqlog mlog)
        {
            var response = new OrderCancelResponse();
            response.partnerId = int.Parse(agentinfo.mt_partnerId);

            string mtorderid = body.orderId.ToString();
            string order_num = body.partnerOrderId;
            string num = body.refundQuantity.ToString();

            mlog.mtorderid = mtorderid;
            mlog.ordernum = order_num;

            #region 条件判断
            if (order_num.ConvertTo<int>(0) == 0)
            {
                response.code = 699;
                response.describe = "订单号格式有误";
                return EditMTlog_Err(response, mlog, reqlog);
            }

            if (num.ConvertTo<int>(0) == 0)
            {
                response.code = 606;
                response.describe = "退票张数格式有误";
                return EditMTlog_Err(response, mlog, reqlog);
            }
            //判断订单是否是当前分销的订单
            bool isselforder = new Agent_requestlogData().Getisselforder(organization, order_num);
            if (isselforder == false)
            {
                response.code = 699;
                response.describe = "订单并非此分销的订单";
                return EditMTlog_Err(response, mlog, reqlog);

            }
            #endregion
            B2b_order morder = new B2bOrderData().GetOrderById(order_num.ConvertTo<int>(0));
            if (morder != null)
            {
                if (morder.Pro_id > 0)
                {
                    B2b_com_pro pro = new B2bComProData().GetProById(morder.Pro_id.ToString());
                    if (pro == null)
                    {
                        response.code = 607;
                        response.describe = "产品不存在";
                        return EditMTlog_Err(response, mlog, reqlog);
                    }
                    else
                    {
                        if (pro.Source_type == 2)//产品来源:1.系统自动生成2.倒码产品
                        {
                            response.code = 607;
                            response.describe = "倒码产品不支持接口退票";
                            return EditMTlog_Err(response, mlog, reqlog);
                        }
                        else
                        {
                            //得到订单的验证方式：0(一码多验);1(一码一验)， 
                            #region 一码多验 退票 按订单号退票，或者外部接口订单
                            if (morder.yanzheng_method == 0 || pro.Source_type == 3)
                            {
                                string data = OrderJsonData.QuitOrder(pro.Com_id, order_num.ConvertTo<int>(0), pro.Id, num.ConvertTo<int>(0), "分销外部接口退票");
                                XmlDocument retdoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + data + "}");
                                XmlElement retroot = retdoc.DocumentElement;
                                string type = retroot.SelectSingleNode("type").InnerText;
                                string msg = retroot.SelectSingleNode("msg").InnerText;

                                if (type == "100")//取消订单成功
                                {

                                    response.code = 200;
                                    response.describe = "success";
                                    response.body = new OrderCancelResponseBody
                                    {
                                        orderId = body.orderId,
                                        refundId=body.refundId,
                                        partnerOrderId = body.partnerOrderId,
                                        requestTime=reqlog.Request_time.ToString("yyyy-MM-dd HH:mm:ss"),
                                        responseTime = reqlog.Return_time.ToString("yyyy-MM-dd HH:mm:ss")
                                    };


                                    reqlog.Ordernum = order_num;
                                    reqlog.Is_dealsuc = 1;


                                    return EditMTlog_Err(response, mlog, reqlog); 
                                }
                                else//取消订单失败
                                {
                                    response.code = 606;
                                    response.describe = msg;
                                    return EditMTlog_Err(response, mlog, reqlog);
                                }
                            }
                            #endregion
                            #region 一码一验 ,或非接口产品
                            else
                            {
                                response.code = 604;
                                response.describe = "订单验码方式不支持";
                                return EditMTlog_Err(response, mlog, reqlog);
                            }
                            #endregion

                        }
                    }

                }
                else
                {
                    response.code = 699;
                    response.describe = "订单中产品不存在";
                    return EditMTlog_Err(response, mlog, reqlog);

                }
            }
            else
            {
                response.code = 699;
                response.describe = "订单不存在";
                return EditMTlog_Err(response, mlog, reqlog);

            }
           


        }
    }
}