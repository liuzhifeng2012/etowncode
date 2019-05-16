using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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

namespace ETS2.WebApp.mjld_jk
{
    public partial class notice : System.Web.UI.Page
    {
        delegate void AsyncsendEventHandler(string updateurl, string pno, int confirmnum, string confirmtime, int agentcomid, int comid, int validateticketlogid, int aorderid);//发送验证同步发送请求委托
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string xml = Request["xml"].ToString();
                //string xml = "rzChHB2sU9ld7U8YJm+ehw3aukqIGDM4BtqU0aUKqsh0Wlv7lE7QY0h+6TyATqOKSH7pPIBOo4pIfuk8gE6jikh+6TyATqOK88GTsYxHp+jxFjUWFoOLabGkQCK4I/78+O3AIZim06QMwlqxUloGSwUMhENjECcrzbztXnARK88eCV0nzDbYTSXGBvHHQSpCtYsGXg/lwRreR77JNU04PrnA/lhi3vGqrkNT1h37aYFq1/tYQ1Xz/ZZH12+3kc7eN5HVgtJCsazTnwU3IXzBtgTaJ2ESK0/X0ncZdJ1sFl4cdnO/KompxrjYGhqFw7FIjsFUOFy3n1dNbNhZIFe8Y78Lir83nuy1W7A/bsAjwDoqCCTzSMhu4i1fpj0wiwQw2BdtiUN3SUQvOPw+mKMgINlGNcH6G0/KSglfJdF6P3DNfmxtL8FfbNt6uk7GCtoKHZD21Lc7H95at7dJ0msz8Y+YwVrIaCGc";
                //string xml = "<?xml version='1.0' encoding='UTF-8'?><Body><type>use_info</type><orderId>22221</orderId><outOrderId>126079</outOrderId><credence>991553361205</credence><useCount>1</useCount><lastCount>0</lastCount><useTime>2015-10-21 17:31:42</useTime><exchangeId>16504001</exchangeId><postTime>2015-10-21 17:33:00</postTime></Body>";
                if (xml == "")
                {
                    TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\mjldlog.txt", "xml空");

                    Response.Write("fail 接收参数为空");

                    return;
                }
                if (xml != "")
                {
                    TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\mjldlog.txt", xml);

                    ApiService mapiservice = new ApiServiceData().GetApiservice(3);
                    if (mapiservice == null)
                    {
                        TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\mjldlog.txt", "获取服务商信息失败-" + xml);

                        Response.Write("fail 获取服务商信息失败");

                        return;
                    }
                    xml = Mjld_TCodeServiceCrypt.Decrypt3DESFromBase64(xml, mapiservice.Deskey);
                    try
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(xml.Trim());
                        XmlElement root = doc.DocumentElement;

                        string type = root.SelectSingleNode("type").InnerText;

                        //验证推送
                        if (type == "use_info")
                        {
                            TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\mjldlog.txt", "use_info: " + xml);


                            //录入交互日志
                            ApiLog mapilog = new ApiLog
                            {
                                Id = 0,
                                request_type = "use_info",
                                Serviceid = 3,
                                Str = xml.Trim(),
                                Subdate = DateTime.Now,
                                ReturnStr = "",
                                ReturnSubdate = DateTime.Now,
                                Errmsg = "",
                            };
                            int ins = new ApiLogData().EditLog(mapilog);

                            //平台订单号
                            string MjldorderId = root.SelectSingleNode("orderId").InnerText;
                            //合作伙伴订单号
                            string orderid = root.SelectSingleNode("outOrderId").InnerText;
                            //验证码
                            string credence = root.SelectSingleNode("credence").InnerText;
                            //使用数量
                            string useCount = root.SelectSingleNode("useCount").InnerText;
                            //剩余数量
                            string lastCount = root.SelectSingleNode("lastCount").InnerText;
                            //验证使用时间
                            string useTime = root.SelectSingleNode("useTime").InnerText;
                            //验证ID
                            string exchangeId = root.SelectSingleNode("exchangeId").InnerText;
                            //景区ID
                            //string ScenicId = root.SelectSingleNode("ScenicId").InnerText;
                            string ScenicId = "";
                            //推送时间
                            string postTime = root.SelectSingleNode("postTime").InnerText;

                            //根据验证id判断是否已经成功处理过该验证推送
                            Api_mjld_AsyncUsenotice sucnotice = new Api_mjld_AsyncUsenoticeData().GetSucUseNoticeByExchangeId(exchangeId);
                            if (sucnotice == null)
                            {
                                Api_mjld_AsyncUsenotice usenotice = new Api_mjld_AsyncUsenotice
                                {
                                    id = 0,
                                    type = type,
                                    mjldOrderId = MjldorderId,
                                    credence = credence,
                                    useCount = useCount.ConvertTo<int>(0),
                                    lastCount = lastCount.ConvertTo<int>(0),
                                    useTime = useTime,
                                    exchangeId = exchangeId,
                                    ScenicId = ScenicId,
                                    postTime = postTime,
                                    rcontent = "",
                                    orderId = orderid.ConvertTo<int>(0),
                                };
                                int insUsenotice = new Api_mjld_AsyncUsenoticeData().EditUsenotice(usenotice);
                                usenotice.id = insUsenotice;

                                //易城系统订单 处理
                                B2b_order morder = new B2bOrderData().GetOrderById(orderid.ConvertTo<int>(0));
                                if (morder != null)
                                {
                                    if (lastCount.ConvertTo<int>(0) > 0)
                                    {
                                        int usecount = morder.service_usecount + usenotice.useCount;
                                        morder.service_usecount = usecount;
                                        morder.service_lastcount = usenotice.lastCount;
                                        morder.Servicepro_v_state = ((int)Product_V_State.PartValidate).ToString();//验证部分

                                        morder.Order_state = (int)OrderStatus.HasUsed;//已消费

                                        new B2bOrderData().InsertOrUpdate(morder);
                                    }
                                    else
                                    {
                                        int usecount = morder.service_usecount + usenotice.useCount;
                                        morder.service_usecount = usecount;
                                        morder.service_lastcount = usenotice.lastCount;
                                        morder.Servicepro_v_state = ((int)Product_V_State.AllValidate).ToString();//验证全部

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

                                    usenotice.rcontent = "1";
                                    new Api_mjld_AsyncUsenoticeData().EditUsenotice(usenotice);

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
                                                mydelegate.BeginInvoke(acompany.Agent_updateurl.Trim(), credence, int.Parse(useCount), useTime, aorder.Agentid, aorder.Comid, 0, aorder.Id, new AsyncCallback(Completed), null);
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
                                                mydelegate.BeginInvoke(acompany.Agent_updateurl.Trim(), credence, int.Parse(useCount), useTime, morder.Agentid, morder.Comid, 0, morder.Id, new AsyncCallback(Completed), null);
                                            }
                                        }
                                    }



                                    Response.Write("1");


                                    return;
                                }
                                else
                                {
                                    usenotice.rcontent = "fail 获取订单失败";
                                    new Api_mjld_AsyncUsenoticeData().EditUsenotice(usenotice);

                                    Response.Write("fail");

                                    return;
                                }


                            }
                            else
                            {
                                Response.Write("1");

                                return;
                            }

                        }
                        //退票推送
                        if (type == "back_order")
                        {
                            TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\mjldlog.txt", "back_order: " + xml);


                            //录入交互日志
                            ApiLog mapilog = new ApiLog
                            {
                                Id = 0,
                                request_type = "back_order",
                                Serviceid = 3,
                                Str = xml.Trim(),
                                Subdate = DateTime.Now,
                                ReturnStr = "",
                                ReturnSubdate = DateTime.Now,
                                Errmsg = "",
                            };
                            int ins = new ApiLogData().EditLog(mapilog);


                            //2为退单成功 3为拒绝
                            string backStatus = root.SelectSingleNode("backStatus").InnerText;
                            //外部退单id
                            string outBackId = root.SelectSingleNode("outBackId").InnerText;
                            //成功退货数量
                            string backCount = root.SelectSingleNode("backCount").InnerText;


                            //本平台退单id
                            string backId = root.SelectSingleNode("backId").InnerText;
                            //推送时间
                            string postTime = root.SelectSingleNode("postTime").InnerText;

                            //根据mjldorderid判断是否成功处理过
                            Api_mjld_AsyncBacknotice sucnotice = new Api_mjld_AsyncBacknoticeData().GetSucApi_mjld_AsyncBacknotice(backId);
                            if (sucnotice == null)
                            {
                                Api_mjld_AsyncBacknotice backnotice = new Api_mjld_AsyncBacknotice
                                {
                                    id = 0,
                                    mjldorderid = backId,
                                    orderid = outBackId.ConvertTo<int>(0),
                                    backCount = backCount.ConvertTo<int>(0),
                                    backStatus = backStatus.ConvertTo<int>(0),
                                    postTime = postTime,
                                    rcontent = "",
                                    type = type
                                };
                                int insBacknotice = new Api_mjld_AsyncBacknoticeData().EditBacknotice(backnotice);
                                backnotice.id = insBacknotice;

                                //易城系统订单 
                                B2b_order morder = new B2bOrderData().GetOrderById(outBackId.ConvertTo<int>(0));
                                if (morder != null)
                                {
                                    //只有订单状态 为"退票处理中",才会进行退款操作
                                    if (morder.Order_state == (int)OrderStatus.WaitQuitOrder)
                                    {
                                        if (backStatus == "2")
                                        {
                                            OrderJsonData.QuitOrder(0, outBackId.ConvertTo<int>(0), morder.Pro_id, backCount.ConvertTo<int>(0), "");
                                        }
                                        //拒绝退款 则还原原来订单状态；订单备注中 提示“拒绝退款”
                                        if (backStatus == "3")
                                        {
                                            //判断是否有订单 绑定传入的订单
                                            B2b_order d_loldorder = new B2bOrderData().GetOldorderBybindingId(outBackId.ConvertTo<int>(0));
                                            if (d_loldorder != null)
                                            {
                                                d_loldorder.Order_state = (morder.Order_remark).ConvertTo<int>(0);
                                                d_loldorder.Order_remark = "美景联动拒绝退票";

                                                new B2bOrderData().UpOrderStateAndRemark(d_loldorder.Id, d_loldorder.Order_state, d_loldorder.Order_remark);
                                            }

                                            morder.Order_state = (morder.Order_remark).ConvertTo<int>(0); ;
                                            morder.Order_remark = "美景联动拒绝退票";
                                            new B2bOrderData().UpOrderStateAndRemark(morder.Id, morder.Order_state, morder.Order_remark);

                                        }

                                        backnotice.rcontent = "1";
                                        new Api_mjld_AsyncBacknoticeData().EditBacknotice(backnotice);

                                        Response.Write("1");

                                        return;
                                    }
                                    else
                                    {
                                        backnotice.rcontent = "fail 订单状态应该为(退票处理中),可是现在为:" + EnumUtils.GetName((OrderStatus)morder.Order_state);
                                        new Api_mjld_AsyncBacknoticeData().EditBacknotice(backnotice);

                                        Response.Write("fail");

                                        return;
                                    }
                                }
                                else
                                {
                                    backnotice.rcontent = "fail 获取订单失败";
                                    new Api_mjld_AsyncBacknoticeData().EditBacknotice(backnotice);

                                    Response.Write("fail");

                                    return;
                                }


                            }
                            else
                            {
                                Response.Write("1");


                                return;
                            }
                        }


                    }
                    catch
                    {
                        Response.Write("fail");

                        return;
                    }

                }
            }
            catch
            {
                Response.Write("fail 接收参数为空");

                return;
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
    }
}