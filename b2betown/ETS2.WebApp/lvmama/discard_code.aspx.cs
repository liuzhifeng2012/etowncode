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
    public partial class  discard_code : System.Web.UI.Page
    {
        delegate void AsyncsendsmsEventHandler(discardcodecallbackmodel discardcodecalljson, Agent_company agentinfo);//异步发送审核通知


        private Agent_company agentinfo;
        string uid = "";
        string password = "";
        readonly string _requestParam;

        public discard_code()
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
                req_type = "discard_code",
                sendip = reqip,
                stockagentcompanyid = 0
            };
            int logid = new lvmama_reqlogData().EditReqlog(mlog);
            mlog.id = logid;
            #endregion



            var data = JsonConvert.DeserializeObject<discard_codemodel>(_requestParam);

            if (data == null)
            {
                return;
            }

            try
            {
                uid = data.uid;
                password = data.password;

            

            string timestamp = data.timestamp;
            string sign = data.sign;
            string extId = data.extId;
            

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
            mlog.ordernum = extId;


            #region 签名验证
            string Md5Sign = lvmamadata.discard_codemd5(data);
            string afterSign = lvmamadata.lumamasign(Md5Sign, agentinfo.Lvmama_Apikey);

            //判断签名是否正确
            if (afterSign != sign)
            {
                var response = new apply_codeRefund();
                response.uid = uid;
                response.orderId = extId;
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

            string actionResult = Getorderrefund(mlog,  agentinfo);

            Response.Write(actionResult);

            }
            catch (Exception ex)
            {
                // Response.Write(ex);
            }
        }

        //     订单退票
        public string Getorderrefund(Lvmama_reqlog mlog, Agent_company agentinfo)
        {
            var response = new backRefund();
            response.uid = agentinfo.Lvmama_uid;
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
                    Request_type = "discard_code",
                    Req_seq = "",
                    Ordernum = "",
                    Is_dealsuc = 0,
                    Is_second_receivereq = 0,
                    Request_ip = CommonFunc.GetRealIP()
                };
                int reqlogid = new Agent_requestlogData().Editagent_reqlog(reqlog);
                reqlog.Id = reqlogid;
                #endregion


                var data = JsonConvert.DeserializeObject<discard_codemodel>(_requestParam);
                if (data.uid == "")
                {
                    response.uid = agentinfo.Lvmama_uid;
                    response.status = "2";
                    response.msg = "数据解析失败";
                    return EditLvmamalog_Order(response, mlog,reqlog);

                }
                else
                {

                     string uid = data.uid;
                     string password = data.password;
                     string sign = data.sign;
                     string timestamp = data.timestamp;
                     string extId = data.extId;
                    

                     Lvmama_reqlog LvmamaOrderCrateSucLog = new lvmama_reqlogData().GetLvmama_OrderpayreqlogBySelforderid(int.Parse(extId), "0");
                     if (LvmamaOrderCrateSucLog == null) {
                         response.status = "4";
                         response.msg = "未查找到相对应订单";

                         return EditLvmamalog_Order(response, mlog, null);
                     }
                     string lvmamaorderid = LvmamaOrderCrateSucLog.mtorderid;
                     data.lvmmamaorderid = lvmamaorderid;


                     B2bOrderData dataorder = new B2bOrderData();
                     B2b_order modelb2border = dataorder.GetOrderById(int.Parse(extId));
                     if (modelb2border == null) {
                         response.status = "4";
                         response.msg = "未查找到相对应订单";

                         return EditLvmamalog_Order(response, mlog, null);
                     }

                     int num = modelb2border.U_num;
                     data.num = num;



                    //日志表2 判断是否是二次发送相同的请求
                     int is_secondreq = new Agent_requestlogData().Is_secondreq(organization.ToString(), lvmamaorderid, reqlog.Request_type);

                    #region 把发送的请求类型，请求流水号，订单号,是否是二次发送相同的请求录入数据库 日志表2
                    reqlog.Req_seq = lvmamaorderid;
                    reqlog.Ordernum = extId;
                    reqlog.Is_second_receivereq = is_secondreq;
                    new Agent_requestlogData().Editagent_reqlog(reqlog);
                    #endregion

                    #region 编辑日志表1
                    mlog.mtorderid = lvmamaorderid;
                    mlog.ordernum = extId;
                    mlog.issecond_req = is_secondreq;
                    new lvmama_reqlogData().EditReqlog(mlog);
                    #endregion

                    //因为是作废，重复的也可以操作，但返回不一定正确
                    if (is_secondreq == 1)
                    {
                        return Cancel_order(reqlog, data, organization.ToString(), mlog);
                    }
                    else
                    {
                        return Cancel_order(reqlog, data, organization.ToString(), mlog);
                    }

                }
            }
            catch (Exception ex)
            {
                response.status = "2";
                response.msg = "异常错误";

                return EditLvmamalog_Order(response, mlog, null);
            }

        }



        //编辑日志
        public string EditLvmamalog_Order(backRefund response, Lvmama_reqlog mlog, Agent_requestlog reqlog)
        {
            string json = JsonConvert.SerializeObject(response);

            #region 把处理结果录入数据库
            mlog.respstr = json;
            mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            mlog.code = response.status;
            mlog.describe = response.msg;
            new lvmama_reqlogData().EditReqlog(mlog);
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


            if (agentinfo != null)
            {
                var lvmamadata = new LVMAMA_Data(agentinfo.Lvmama_uid, agentinfo.Lvmama_password, agentinfo.Lvmama_Apikey);


                //初始的时候没有sign值，等组合后下面生成加密文件
                var discardcodecalljson = lvmamadata.discardcodecall_json(reqlog.Req_seq, agentinfo.Lvmama_uid, agentinfo.Lvmama_password, response.status,"", response.msg);


                #region 签名验证
                string Md5Sign = lvmamadata.discardcall_codemd5(discardcodecalljson);
                string afterSign = lvmamadata.lumamasign(Md5Sign, agentinfo.Lvmama_Apikey);
                #endregion

                discardcodecalljson.sign = afterSign;

                //异步通知
                AsyncsendsmsEventHandler mydelegate = new AsyncsendsmsEventHandler(AsyncSendSms);
                mydelegate.BeginInvoke(discardcodecalljson, agentinfo, new AsyncCallback(CompletedSendSms), null);
            }


            return json;
        }


        #region 异步发送废票通知
        public static void CompletedSendSms(IAsyncResult result)
        {
            //获取委托对象，调用EndInvoke方法获取运行结果
            AsyncResult _result = (AsyncResult)result;
            AsyncsendsmsEventHandler myDelegate = (AsyncsendsmsEventHandler)_result.AsyncDelegate;
            myDelegate.EndInvoke(_result);
        }
        #endregion

        public static void AsyncSendSms(discardcodecallbackmodel discardcodecalljson, Agent_company agentinfo)
        {

            var lvmamadata = new LVMAMA_Data(agentinfo.Lvmama_uid, agentinfo.Lvmama_password, agentinfo.Lvmama_Apikey);

            var asynnoticecall = lvmamadata.discardcodecallbacksend(discardcodecalljson, agentinfo.Id);

        }


        private string Cancel_order(Agent_requestlog reqlog, discard_codemodel body, string organization, Lvmama_reqlog mlog)
        {
            var response = new backRefund();
           // response.partnerId = int.Parse(agentinfo.mt_partnerId);

            string mtorderid = body.lvmmamaorderid;
            string order_num = body.extId;
            int num = body.num;

            mlog.mtorderid = mtorderid;
            mlog.ordernum = order_num;

            #region 条件判断
            if (order_num.ConvertTo<int>(0) == 0)
            {
                response.status = "4";
                response.msg = "订单号格式有误";
                return EditLvmamalog_Order(response, mlog, reqlog);
            }

            if (num == 0)
            {
                response.status = "4";
                response.msg = "退票张数格式有误";
                return EditLvmamalog_Order(response, mlog, reqlog);
            }
            //判断订单是否是当前分销的订单
            //bool isselforder = new Agent_requestlogData().lvmamaGetisselforder(organization, order_num);
            //if (isselforder == false)
            //{
            //    response.status = "4";
            //    response.msg = "订单信息错误";
            //    return EditLvmamalog_Order(response, mlog, reqlog);

            //}
            #endregion
            B2b_order morder = new B2bOrderData().GetOrderById(order_num.ConvertTo<int>(0));
            if (morder != null)
            {
                if (morder.Pro_id > 0)
                {
                    //判断订单出票单位是否正确
                    if (morder.Agentid != int.Parse(organization))
                    {
                        response.status = "4";
                        response.msg = "订单信息错误";
                        return EditLvmamalog_Order(response, mlog, reqlog);
                    }


                    B2b_com_pro pro = new B2bComProData().GetProById(morder.Pro_id.ToString());
                    if (pro == null)
                    {
                        response.status = "4";
                        response.msg = "此订单对应产品错误";
                        return EditLvmamalog_Order(response, mlog, reqlog);
                    }
                    else
                    {
                        if (pro.Source_type == 2 )//产品来源:1.系统自动生成2.倒码产品3.外部接口产品
                        {
                            response.status = "4";
                            response.msg = "倒码产品不支持接口退票";
                            return EditLvmamalog_Order(response, mlog, reqlog);
                        }
                        else
                        {
                            //得到订单的验证方式：0(一码多验);1(一码一验)， 
                            #region 一码多验 退票 按订单号退票
                            if (morder.yanzheng_method == 0)
                            {
                                string data = OrderJsonData.QuitOrder(pro.Com_id, order_num.ConvertTo<int>(0), pro.Id, num, "分销外部接口退票");
                                XmlDocument retdoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + data + "}");
                                XmlElement retroot = retdoc.DocumentElement;
                                string type = retroot.SelectSingleNode("type").InnerText;
                                string msg = retroot.SelectSingleNode("msg").InnerText;

                                if (type == "100")//取消订单成功
                                {
                                    response.status = "0";
                                    response.msg = "success";
                                    reqlog.Ordernum = order_num;
                                    reqlog.Is_dealsuc = 1;
                                    return EditLvmamalog_Order(response, mlog, reqlog);
                                }
                                else//取消订单失败
                                {
                                    response.status = "4";
                                    response.msg = msg;
                                    return EditLvmamalog_Order(response, mlog, reqlog);
                                }
                            }
                            #endregion
                            #region 一码一验 退票 按电子码退票
                            else
                            {
                                response.status = "2";
                                response.msg = "订单验码方式不支持";
                                return EditLvmamalog_Order(response, mlog, reqlog);
                            }
                            #endregion
                        }
                    }

                }
                else
                {
                    response.status = "4";
                    response.msg = "订单中产品不存在";
                    return EditLvmamalog_Order(response, mlog, reqlog);
                }
            }
            else
            {
                response.status = "4";
                response.msg = "订单不存在";
                return EditLvmamalog_Order(response, mlog, reqlog);
            }



        }








    }
}