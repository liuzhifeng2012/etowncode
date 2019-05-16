using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Framework;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using System.Xml;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS.JsonFactory;
using ETS2.VAS.Service.VASService.Modle.Enum;
using System.Text;
using System.IO;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle.Enum;
using Newtonsoft.Json;
using System.Runtime.Remoting.Messaging;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.VAS.Service.VASService.Data;

namespace ETS2.WebApp.TicketService
{
    /// <summary>
    /// Handler1 的摘要说明
    /// </summary>
    public class Handler1 : IHttpHandler
    {
        delegate void AsyncsendEventHandler(string updateurl, string pno, int confirmnum, string confirmtime, int agentcomid, int comid, int validateticketlogid, int aorderid);//发送验证同步发送请求委托
       
        /// <summary>
        /// 阳光验证通知接口
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string xml = context.Request["xml"].ConvertTo<string>("");
            if (xml == "")
            {
                context.Response.Write("fail 接收参数为空");//参数为空
                return;
            }

            if (xml != "")
            {
                //录入交互日志
                ApiLog mapilog = new ApiLog
                {
                    Id = 0,
                    request_type = "sync_order",
                    Serviceid = 1,
                    Str = xml.Trim(),
                    Subdate = DateTime.Now,
                    ReturnStr = "",
                    ReturnSubdate = DateTime.Now,
                    Errmsg = "",
                };
                int ins = new ApiLogData().EditLog(mapilog);
                mapilog.Id = ins;

                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);
                    XmlElement root = doc.DocumentElement;
                    string request_type = root.SelectSingleNode("request_type").InnerText;
                    string req_seq = root.SelectSingleNode("req_seq").InnerText;//原订单流水号
                    string platform_req_seq = root.SelectSingleNode("platform_req_seq").InnerText;//平台请求流水号
                    string order_num = root.SelectSingleNode("order/order_num").InnerText;//订单号
                    string num = root.SelectSingleNode("order/num").InnerText;//使用张数
                    string use_time = root.SelectSingleNode("order/use_time").InnerText;//使用时间

                    //根据平台订单号得到易城订单号
                    int orderid = new B2bOrderData().GetOrderidByServiceOrderNum(order_num);
                    if (orderid == 0)
                    {
                        //"老系统订单，服务商订单信息获取不到"===只有阳光订单才有这种可能，其他接口则直接返回错误 
                        //把验票记录 记入 验证同步日志表
                        Api_yg_Syncordernotice notice = new Api_yg_Syncordernotice
                        {
                            id = 0,
                            req_seq = req_seq,
                            platform_req_seq = platform_req_seq,
                            order_num = order_num,
                            num = num.ConvertTo<int>(0),
                            use_time = use_time.ConvertTo<DateTime>(),
                            rcontent = "老系统订单，服务商订单信息获取不到",
                            orderid = 0
                        };
                        int insnotice = new Api_yg_SyncordernoticeData().EditNotice(notice);
                        notice.id = insnotice;

                        mapilog.ReturnStr = "success";
                        mapilog.ReturnSubdate = DateTime.Now;
                        mapilog.Errmsg = notice.rcontent;
                        new ApiLogData().EditLog(mapilog);

                        context.Response.Write("success");
                        return;
                    }

                    //根据平台流水号 得到正确处理的验证通知记录
                    Api_yg_Syncordernotice sucnotice = new Api_yg_SyncordernoticeData().GetSucNotice(platform_req_seq);
                    if (sucnotice == null)
                    {
                        //把验票记录 记入 验证同步日志表
                        Api_yg_Syncordernotice notice = new Api_yg_Syncordernotice
                        {
                            id = 0,
                            req_seq = req_seq,
                            platform_req_seq = platform_req_seq,
                            order_num = order_num,
                            num = num.ConvertTo<int>(0),
                            use_time = use_time.ConvertTo<DateTime>(),
                            rcontent = "",
                            orderid = orderid
                        };
                        int insnotice = new Api_yg_SyncordernoticeData().EditNotice(notice);
                        notice.id = insnotice;




                        //易城系统订单 处理
                        B2b_order morder = new B2bOrderData().GetOrderById(orderid);
                        if (morder != null)
                        {
                            int incount = morder.U_num;//总数量
                            int usecount = morder.service_usecount + num.ConvertTo<int>(0);//验证数量
                            if (incount == usecount)//全部验证 
                            {
                                morder.service_usecount = usecount;
                                morder.service_lastcount = incount - usecount - morder.Cancelnum;
                                morder.Servicepro_v_state = ((int)Product_V_State.AllValidate).ToString();//验证全部
                                morder.Order_state = (int)OrderStatus.HasUsed;//已消费

                                new B2bOrderData().InsertOrUpdate(morder);
                            }
                            else //部分验证
                            {
                                morder.service_usecount = usecount;
                                morder.service_lastcount = incount - usecount - morder.Cancelnum;
                                morder.Servicepro_v_state = ((int)Product_V_State.PartValidate).ToString();//验证部分
                                morder.Order_state = (int)OrderStatus.HasUsed;//已消费

                                new B2bOrderData().InsertOrUpdate(morder);
                            }

                            //根据b单判断是否有a单存在
                            B2b_order aorder = new B2bOrderData().GetOldorderBybindingId(morder.Id);
                            if (aorder != null)
                            {
                                aorder.Order_state = morder.Order_state;

                                new B2bOrderData().InsertOrUpdate(aorder);
                            }

                            notice.rcontent = "success";
                            new Api_yg_SyncordernoticeData().EditNotice(notice);


                            //如果是分销订单，则需要给分销发送验证通知
                            //判断b订单 是否 属于某个a订单  
                            if (aorder != null)
                            {
                                //得到a订单的分销信息
                                if (aorder.Agentid > 0)
                                {
                                    Agent_company acompany = new AgentCompanyData().GetAgentCompany(aorder.Agentid);
                                    if (acompany != null)
                                    {
                                        //异步发送验证同步请求
                                        AsyncsendEventHandler mydelegate = new AsyncsendEventHandler(AsyncSend);
                                        mydelegate.BeginInvoke(acompany.Agent_updateurl.Trim(), morder.Pno, morder.service_usecount, use_time, aorder.Agentid, aorder.Comid, 0, aorder.Id, new AsyncCallback(Completed), null);
                                    }
                                }
                            }
                            else
                            {
                                if (morder.Agentid > 0)
                                {
                                    Agent_company acompany = new AgentCompanyData().GetAgentCompany(morder.Agentid);
                                    if (acompany != null)
                                    {
                                        //异步发送验证同步请求
                                        AsyncsendEventHandler mydelegate = new AsyncsendEventHandler(AsyncSend);
                                        mydelegate.BeginInvoke(acompany.Agent_updateurl.Trim(), morder.Pno, morder.service_usecount, use_time, morder.Agentid, morder.Comid, 0, morder.Id, new AsyncCallback(Completed), null);
                                    }
                                }
                            }

                            mapilog.ReturnStr = "success";
                            mapilog.ReturnSubdate = DateTime.Now;
                            mapilog.Errmsg = "成功";
                            new ApiLogData().EditLog(mapilog);

                            context.Response.Write("success");
                            return;

                        }
                        else
                        {

                            mapilog.ReturnStr = "fail 获取订单失败";
                            mapilog.ReturnSubdate = DateTime.Now;
                            mapilog.Errmsg = "fail 获取订单失败";
                            new ApiLogData().EditLog(mapilog);

                            notice.rcontent = "fail 获取订单失败";
                            new Api_yg_SyncordernoticeData().EditNotice(notice);

                            context.Response.Write("fail 获取订单失败");
                            return;
                        }
                    }
                    else
                    {

                        mapilog.ReturnStr = "success";
                        mapilog.ReturnSubdate = DateTime.Now;
                        mapilog.Errmsg = "已经成功处理过，不在处理";
                        new ApiLogData().EditLog(mapilog);

                        context.Response.Write("success");
                        return;
                    }
                }
                catch
                {
                    context.Response.Write("fail 意外错误");//意外错误
                    return;  
                }
            }
        }
        #region 发送验证同步请求:回调函数和验证同步方法
        public static void Completed(IAsyncResult result)
        {
            //获取委托对象，调用EndInvoke方法获取运行结果
            AsyncResult _result = (AsyncResult)result;
            AsyncsendEventHandler myDelegate = (AsyncsendEventHandler)_result.AsyncDelegate;
            myDelegate.EndInvoke(_result);
        }
        public static void AsyncSend(string updateurl, string pno, int confirmnum, string confirmtime, int agentcomid, int comidd1, int validateticketlogid, int aorderid)
        {
            Agent_asyncsendlog log = new Agent_asyncsendlog
            {
                Id = 0,
                Pno = pno,
                Num = confirmnum,
                Sendtime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                Confirmtime = DateTime.Parse(confirmtime),
                Issendsuc = 0,//0失败；1成功
                Agentupdatestatus = (int)AgentUpdateStatus.Fail,
                Agentcomid = agentcomid,
                Comid = comidd1,
                Remark = "",
                Issecondsend = 0,
                Platform_req_seq = (1000000000 + agentcomid).ToString() + DateTime.Now.ToString("yyyyMMddhhssmm") + CommonFunc.CreateNum(6),//请求流水号
                Request_content = "",
                Response_content = "",
                b2b_etcket_logid = validateticketlogid
            };
            int inslog = new Agent_asyncsendlogData().EditLog(log);
            log.Id = inslog;
            try
            {


                //获得分销商信息
                Agent_company agentinfo = new AgentCompanyData().GetAgentCompany(agentcomid);
                if (agentinfo != null)
                {
                    string agent_updateurl = agentinfo.Agent_updateurl;

                    #region 糯米分销
                    if (agentinfo.Agent_type == (int)AgentCompanyType.NuoMi)//糯米分销
                    {

                        string re = new GetUrlData().HttpPost(updateurl, "");

                        XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + re + "}");
                        XmlElement root = doc.DocumentElement;
                        string info = root.SelectSingleNode("info").InnerText;

                        //只要返回了数据，则是发送成功
                        log.Id = inslog;
                        log.Issendsuc = 1;
                        log.Request_content = updateurl;
                        log.Response_content = re;

                        if (info == "success")
                        {

                            string errCode = root.SelectSingleNode("res/errCode").InnerText;//分销商更新结果
                            if (errCode == "0")
                            {
                                log.Agentupdatestatus = (int)AgentUpdateStatus.Suc;
                                new Agent_asyncsendlogData().EditLog(log);
                            }
                            else
                            {
                                log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                                new Agent_asyncsendlogData().EditLog(log);
                            }
                        }
                        else
                        {
                            log.Remark = info;
                            log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                            new Agent_asyncsendlogData().EditLog(log);
                        }
                    }
                    #endregion
                    #region 一般分销
                    else //一般分销
                    {

                        /*根据订单号得到分销商订单请求记录(当前需要得到原订单请求流水号)*/
                        List<Agent_requestlog> listagent_rlog = new Agent_requestlogData().GetAgent_requestlogByOrdernum(aorderid.ToString(), "add_order", "1");
                        if (listagent_rlog == null)
                        {
                            log.Remark = "根据订单号得到分销商订单请求记录失败";//单引号替换为'',双引号不用处理;
                            log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                            new Agent_asyncsendlogData().EditLog(log);
                            return;
                        }
                        if (listagent_rlog.Count == 0)
                        {
                            log.Remark = "根据订单号得到分销商订单请求记录失败.";//单引号替换为'',双引号不用处理;
                            log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                            new Agent_asyncsendlogData().EditLog(log);
                            return;
                        }


                        /*验证通知发送内容*/
                        string sbuilder = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                        "<business_trans>" +
                        "<request_type>sync_order</request_type>" +//<!--验证同步-->
                        "<req_seq>{0}</req_seq>" +//<!--原订单请求流水号-->
                        "<platform_req_seq>{1}</platform_req_seq>" +//<!--平台请求流水号-->
                        "<order>" +//<!--订单信息-->
                        "<code>{5}</code>" +//<!-- 验证电子码 y-->
                        "<order_num>{2}</order_num>" +//<!-- 订单号 y-->
                        "<num>{3}</num>" +//<!-- 使用张数 -->
                        "<use_time>{4}</use_time>" +//<!--  使用时间  -->
                        "</order>" +
                        "</business_trans>", listagent_rlog[0].Req_seq, log.Platform_req_seq, aorderid, confirmnum, CommonFunc.ConvertDateTimeInt(DateTime.Parse(confirmtime)), pno);

                        #region 分销通知发送方式post
                        if (agentinfo.inter_sendmethod.ToLower() == "post")
                        {
                            string re = "";
                            if (agentcomid == 6490)
                            {
                                re = new GetUrlData().HttpPost(updateurl, sbuilder);

                                updateurl += "?xml=" + sbuilder;//只为记录
                            }
                            else
                            {
                                updateurl += "?xml=" + sbuilder;
                                re = new GetUrlData().HttpPost(updateurl, "");
                            }


                            //只要返回了数据，则是发送成功
                            log.Id = inslog;
                            log.Issendsuc = 1;
                            log.Request_content = updateurl;
                            log.Response_content = re;

                            log.Remark = re.Replace("'", "''");//单引号替换为'',双引号不用处理;
                            if (re.Length >= 7)
                            {
                                re = re.Substring(0, 7);
                            }
                            if (re == "success")
                            {
                                log.Agentupdatestatus = (int)AgentUpdateStatus.Suc;
                                new Agent_asyncsendlogData().EditLog(log);
                            }
                            else
                            {

                                log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                                new Agent_asyncsendlogData().EditLog(log);
                            }
                        }
                        #endregion
                        #region 分销通知发送方式get
                        else
                        {
                            if (updateurl.IndexOf('?') > -1)
                            {
                                updateurl += "&xml=" + sbuilder;
                            }
                            else
                            {
                                updateurl += "?xml=" + sbuilder;
                            }

                            string re = new GetUrlData().HttpGet(updateurl);
                            //只要返回了数据，则是发送成功
                            log.Id = inslog;
                            log.Issendsuc = 1;
                            log.Request_content = updateurl;
                            log.Response_content = re;

                            log.Remark = re.Replace("'", "''");//单引号替换为'',双引号不用处理;
                            if (re.Length >= 7)
                            {
                                re = re.Substring(0, 7);
                            }
                            if (re == "success")
                            {
                                log.Agentupdatestatus = (int)AgentUpdateStatus.Suc;
                                new Agent_asyncsendlogData().EditLog(log);

                            }
                            else
                            {

                                log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                                new Agent_asyncsendlogData().EditLog(log);
                            }
                        }
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    log.Remark = "分销商获取失败";
                    log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                    new Agent_asyncsendlogData().EditLog(log);
                }



            }
            catch (Exception e)
            {
                log.Id = inslog;
                log.Remark = e.Message.Replace("'", "''");//单引号替换为'',双引号不用处理
                new Agent_asyncsendlogData().EditLog(log);
            }
        }
        #endregion
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}