using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Timers;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.VAS.Service.VASService.Modle.Enum;
using ETS2.VAS.Service.VASService.Data;
using System.Xml;
using ETS.Framework;
using Newtonsoft.Json;
using ETS2.PM.Service.PMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle.Enum;
using System.Runtime.Remoting.Messaging;
using ETS2.PM.Service.PMService.Modle;
using ETS2.Member.Service.MemberService.Data;
using ETS2.Common.Business;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS.JsonFactory;

namespace ETS2.WebApp
{
    public class Global : System.Web.HttpApplication
    {

        void Application_BeginRequest(object sender, EventArgs e)
        {
            ////根据需要进行全局性(即针对整个应用程序)Sql注入检查 
            //SqlChecker SqlChecker = new SqlChecker(this.Request, this.Response);
            ////或 SqlChecker SqlChecker = new SqlChecker(this.Request,this.Response,safeUrl); 
            //SqlChecker.Check(); 
        }

        delegate void AsyncSendEventHandler(Agent_asyncsendlog log);

        delegate void Asyncsendsms2EventHandler(B2b_smsmobilesend smslog);

        private static object lockobj = new object();
        void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码,每隔10分钟执行一次
            System.Timers.Timer objTimer = new Timer();
            objTimer.Interval = 600000; //这个时间单位毫秒,比如10秒，就写10000 
            //objTimer.Interval = 20000; //这个时间单位毫秒,比如10秒，就写10000 
            objTimer.Enabled = true;
            objTimer.Elapsed += new ElapsedEventHandler(objTimer_Elapsed);

            RegisterRoutes(System.Web.Routing.RouteTable.Routes);
        }

        void objTimer_Elapsed(object sender, ElapsedEventArgs e)  //这个方法内实现你想做的事情。 
        {
            #region 查询分销商验证同步日志表，  发送失败(失败次数小于3次)记录中的前10条(正序)，重新发送验证请求
            List<Agent_asyncsendlog> loglist = new Agent_asyncsendlogData().GetTop10SendFail();
            if (loglist != null)
            {
                if (loglist.Count > 0)
                {
                    foreach (Agent_asyncsendlog log in loglist)
                    {
                        AsyncSendEventHandler mydelegate = new AsyncSendEventHandler(AsyncSend);
                        mydelegate.BeginInvoke(log, new AsyncCallback(Completed), null);
                    }
                }
            }
            #endregion

            #region 超时12小时自动解除 顾问锁定用户

            var channlelock = new MemberChannelData().WxMessageUnLockUserTimeout();
            #endregion

            #region 定时执行:1.凌晨1点0分-1点10分 之间执行一次(过期产品应该自动下线状态。); 2.商户 每天生成一个随机码
            int intHour = DateTime.Now.Hour;
            int intMinute = DateTime.Now.Minute;
            if (intHour == 1 && intMinute < 10)
            {
                #region 过期产品应该自动下线状态
                ProductJsonData.ProAutoDownLine();
                #endregion
                #region  商户 每天生成一个随机码
                int onlineCompanyCount = 0;
                List<B2b_company> b2bcompanylist = new B2bCompanyData().GetAllCompanys("1", out onlineCompanyCount);
                foreach (B2b_company cominfo in b2bcompanylist)
                {
                    if (cominfo != null)
                    {
                        //得到商户当天的 随机码
                        new B2bCompanyData().GetComDayRandomstr(cominfo.ID, "999999999");
                    }
                }
                #endregion
            }

            #endregion

            #region 超时(时限:30分钟)订单自动作废
            try
            {
                //查询出含有超时订单的产品
                List<B2b_com_pro> timeoutorderprolist = new B2bOrderData().GetTimeoutOrderProlist();
                if (timeoutorderprolist.Count > 0)
                {
                    foreach (B2b_com_pro mpro in timeoutorderprolist)
                    {
                        //作废超时未支付订单，完成回滚操作
                        int rs = new B2bComProData().CancelOvertimeOrder(mpro);
                    }
                }
            }
            catch { }
            #endregion



            #region (暂时未应用)联合查询短信日志表和订单表，查出订单状态成功(4发码成功；22已处理)，但是发码失败(1未发送；3发送中)的前5条订单，重新发送二维码短信
            //List<B2b_smsmobilesend> smsloglist = new B2bSmsMobileSendDate().GetTop5SendFail();
            //if (loglist != null)
            //{
            //    if (loglist.Count > 0)
            //    {
            //        foreach (B2b_smsmobilesend msmslog in smsloglist)
            //        {
            //            Asyncsendsms2EventHandler mydelegate2 = new Asyncsendsms2EventHandler(AsyncSendSms);
            //            mydelegate2.BeginInvoke(msmslog, new AsyncCallback(CompletedSendSms), null);
            //        }
            //    }
            //}


            ////ExcelSqlHelper.ExecuteNonQuery("insert into test (retstr,pno) values('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','')");
            #endregion
        }
        #region 异步发送验证同步请求
        public static void Completed(IAsyncResult result)
        {
            //获取委托对象，调用EndInvoke方法获取运行结果
            AsyncResult _result = (AsyncResult)result;
            AsyncSendEventHandler myDelegate = (AsyncSendEventHandler)_result.AsyncDelegate;
            myDelegate.EndInvoke(_result);
        }
        void AsyncSend(Agent_asyncsendlog log1)
        {
            lock (lockobj)
            {
                Agent_asyncsendlog log = new Agent_asyncsendlog
                {
                    Id = 0,
                    Pno = log1.Pno,
                    Num = log1.Num,
                    Sendtime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    Confirmtime = log1.Confirmtime,
                    Issendsuc = 0,//0失败；1成功
                    Agentupdatestatus = (int)AgentUpdateStatus.Fail,
                    Agentcomid = log1.Agentcomid,
                    Comid = log1.Comid,
                    Remark = "",
                    Issecondsend = 1,
                    Platform_req_seq = log1.Platform_req_seq,//平台流水号，同一次验证的平台流水号相同，分销商根据平台流水号判断是否是同一次验证
                    Request_content = "",
                    Response_content = "",
                    b2b_etcket_logid = log1.b2b_etcket_logid
                };
                int inslog = new Agent_asyncsendlogData().EditLog(log);
                log.Id = inslog;
                try
                {
                    var eticketinfo = new B2bEticketData().GetEticketDetail(log1.Pno);
                    #region 外部接口产品(电子票表b2b_eticket中没有电子票信息) 直接用日志中的地址
                    if (eticketinfo == null)
                    {
                        //获得分销商信息
                        Agent_company agentinfo = new AgentCompanyData().GetAgentCompany(log1.Agentcomid);
                        if (agentinfo != null)
                        {

                            #region 糯米分销
                            if (agentinfo.Agent_type == (int)AgentCompanyType.NuoMi)//糯米分销
                            {
                                //查询站外码状态
                                string username = agentinfo.Agent_auth_username;//百度糯米用户名
                                string token = agentinfo.Agent_auth_token;//百度糯米token
                                string code = log1.Pno;//券码
                                string bindcomname = agentinfo.Agent_bindcomname;//供应商名称

                                string updateurl = log1.Request_content;


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
                            #region 一般分销推送验证同步请求
                            else
                            {
                                string re = "";
                                string updateurl = "";
                                #region 分销通知发送方式post
                                if (agentinfo.inter_sendmethod.ToLower() == "post")
                                {
                                    updateurl = log1.Request_content;//只为记录
                                    if (log1.Agentcomid == 6490)
                                    {
                                        //截取 xxx?xml=xx 
                                        re = new GetUrlData().HttpPost(updateurl.Substring(0, updateurl.IndexOf('?')), updateurl.Substring(updateurl.IndexOf('?') + 5));
                                    }
                                    else
                                    {
                                        updateurl = log1.Request_content;
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
                                    updateurl = log1.Request_content;

                                    re = new GetUrlData().HttpGet(updateurl);
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
                    }
                    #endregion
                    #region 系统自动生成的电子码
                    else
                    {
                        if (eticketinfo.Agent_id > 0)//分销商电子票,需要发送验证同步请求
                        {
                            //获得分销商信息
                            Agent_company agentinfo = new AgentCompanyData().GetAgentCompany(log1.Agentcomid);
                            if (agentinfo != null)
                            {
                                string agent_updateurl = agentinfo.Agent_updateurl;
                                #region 糯米分销
                                if (agentinfo.Agent_type == (int)AgentCompanyType.NuoMi)//糯米分销
                                {
                                    //查询站外码状态
                                    string username = agentinfo.Agent_auth_username;//百度糯米用户名
                                    string token = agentinfo.Agent_auth_token;//百度糯米token
                                    string code = eticketinfo.Pno;//券码
                                    string bindcomname = agentinfo.Agent_bindcomname;//供应商名称

                                    string updateurl = agent_updateurl + "?auth={\"userName\":\"" + username + "\",\"token\":\"" + token + "\"}&code=" + code + "&userName=" + bindcomname + "&dealId=&phone="; ;


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
                                #region 一般分销推送验证同步请求
                                else
                                {
                                    //一般分销推送验证同步请求

                                    if (agent_updateurl.Trim() == "" || agentinfo.Inter_deskey == "")
                                    {
                                        log.Remark = "分销商验证同步通知或秘钥没提供";//单引号替换为'',双引号不用处理;
                                        log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                                        new Agent_asyncsendlogData().EditLog(log);
                                        return;
                                    }

                                    if (eticketinfo.Oid == 0)
                                    {
                                        log.Remark = "电子票对应的订单号为0";//单引号替换为'',双引号不用处理;
                                        log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                                        new Agent_asyncsendlogData().EditLog(log);
                                        return;
                                    }

                                    //a订单:原分销向指定商户提交的订单;b订单:指定商户下的绑定分销向拥有产品的商户提交的订单
                                    //电子票表中记录的Oid是b订单
                                    //判断b订单 是否 属于某个a订单 
                                    B2b_order loldorder = new B2bOrderData().GetOldorderBybindingId(eticketinfo.Oid);
                                    if (loldorder != null)
                                    {
                                        eticketinfo.Oid = loldorder.Id;
                                    }
                                    /*根据订单号得到分销商订单请求记录(当前需要得到原订单请求流水号)*/
                                    List<Agent_requestlog> listagent_rlog = new Agent_requestlogData().GetAgent_requestlogByOrdernum(eticketinfo.Oid.ToString(), "add_order", "1");
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
                                    "</business_trans>", listagent_rlog[0].Req_seq, log.Platform_req_seq, eticketinfo.Oid, log.Num, ConvertDateTimeInt(log.Confirmtime), log.Pno);

                                    string re = "";
                                    string updateurl = "";
                                    #region 分销通知发送方式post
                                    if (agentinfo.inter_sendmethod.ToLower() == "post")
                                    {
                                        if (eticketinfo.Agent_id == 6490)
                                        {
                                            //当都用完了，才发送通知请求
                                            if (eticketinfo.Use_pnum == 0)
                                            {
                                                sbuilder = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
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
                                                "</business_trans>", listagent_rlog[0].Req_seq, log.Platform_req_seq, eticketinfo.Oid, eticketinfo.Pnum, ConvertDateTimeInt(log.Confirmtime), log.Pno);


                                                re = new GetUrlData().HttpPost(agent_updateurl, sbuilder);
                                            }
                                            updateurl = agent_updateurl + "?xml=" + sbuilder;//只为记录
                                        }
                                        else
                                        {
                                            updateurl = agent_updateurl + "?xml=" + sbuilder;
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
                                        if (agent_updateurl.IndexOf('?') > -1)
                                        {
                                            updateurl = agent_updateurl + "&xml=" + sbuilder;
                                        }
                                        else
                                        {
                                            updateurl = agent_updateurl + "?xml=" + sbuilder;
                                        }
                                        re = new GetUrlData().HttpGet(updateurl);
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
                        }
                    }
                    #endregion

                }
                catch (Exception e)
                {
                    log.Id = inslog;
                    log.Remark = e.Message.Replace("'", "''");//单引号替换为'',双引号不用处理
                    new Agent_asyncsendlogData().EditLog(log);
                }
            }

        }
        #endregion

        #region 异步发送二维码短信
        public static void CompletedSendSms(IAsyncResult result)
        {
            //获取委托对象，调用EndInvoke方法获取运行结果
            AsyncResult _result = (AsyncResult)result;
            Asyncsendsms2EventHandler myDelegate = (Asyncsendsms2EventHandler)_result.AsyncDelegate;

            myDelegate.EndInvoke(_result);
        }


        public static void AsyncSendSms(B2b_smsmobilesend msmslog)
        {
            //根据订单号得到产品信息
            B2b_com_pro mpro = new B2bComProData().GetProByOrderID(msmslog.Oid);
            if (mpro == null)
            {
                return;
            }

            string phone = msmslog.Mobile;
            string smscontent = msmslog.Content;
            int comid = mpro.Com_id;
            int orderid = msmslog.Oid;
            string pno = msmslog.Pno;
            int insertsendEticketid = msmslog.Sendeticketid;
            int pro_sourcetype = mpro.Source_type;

            string msg = "";

            int sendback = SendSmsHelper.SendSms(phone, smscontent, comid, out msg);

            int sendstate = 1;//1未发码；2已发码;3发送中 
            if (pro_sourcetype == 1)//系统自动生成
            {

                if (sendback > 0)
                {
                    //修改电子票发送日志表的发码状态为"发送成功"
                    B2b_eticket_send_log eticketlogup = new B2b_eticket_send_log()
                    {
                        Id = insertsendEticketid,
                        Sendstate = (int)SendCodeStatus.HasSend,
                        Senddate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    };

                    int upsendEticket = new B2bEticketSendLogData().InsertOrUpdate(eticketlogup);
                    //修改订单中发码状态为“已发码”
                    int upsendstate = new B2bOrderData().Upsendstate(orderid, (int)SendCodeStatus.HasSend);
                }
                else
                {
                    //修改电子票发送日志表的发码状态为"未发码"
                    B2b_eticket_send_log eticketlogup = new B2b_eticket_send_log()
                    {
                        Id = insertsendEticketid,
                        Sendstate = (int)SendCodeStatus.NotSend,
                        Senddate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    };

                    int upsendEticket = new B2bEticketSendLogData().InsertOrUpdate(eticketlogup);
                    //修改订单中发码状态为“未发码”
                    int upsendstate = new B2bOrderData().Upsendstate(orderid, (int)SendCodeStatus.NotSend);
                }
            }
            if (pro_sourcetype == 2)//倒码产品
            {
                if (sendback > 0)
                {
                    //修改订单中发码状态为“已发码”
                    int upsendstate = new B2bOrderData().Upsendstate(orderid, (int)SendCodeStatus.HasSend);
                }
                else
                {
                    //修改订单中发码状态为“未发码”
                    int upsendstate = new B2bOrderData().Upsendstate(orderid, (int)SendCodeStatus.NotSend);
                }
            }


            //记录短信日志表
            B2bSmsMobileSendDate smsmobilelog = new B2bSmsMobileSendDate();
            B2b_smsmobilesend smslog = new B2b_smsmobilesend()
            {
                Mobile = phone,
                Content = smscontent,
                Flag = sendstate,
                Text = msg,
                Delaysendtime = "",
                Oid = orderid,
                Pno = pno,
                Realsendtime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                Smsid = sendback,
                Sendeticketid = insertsendEticketid
            };
            int insertsendmobileid = smsmobilelog.InsertOrUpdate(smslog);
        }


        #endregion
        void Application_End(object sender, EventArgs e)
        {
            //  在应用程序关闭时运行的代码

        }

        void Application_Error(object sender, EventArgs e)
        {
            // 在出现未处理的错误时运行的代码

        }

        void Session_Start(object sender, EventArgs e)
        {
            // 在新会话启动时运行的代码

        }

        void Session_End(object sender, EventArgs e)
        {
            // 在会话结束时运行的代码。 
            // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
            // InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer 
            // 或 SQLServer，则不会引发该事件。

        }
        /// <summary>

        /// datetime转换为aunixtime

        /// </summary>

        /// <param name="time"></param>

        /// <returns></returns>

        private static int ConvertDateTimeInt(System.DateTime time)
        {

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

            return (int)(time - startTime).TotalSeconds;

        }


        static void RegisterRoutes(System.Web.Routing.RouteCollection routes)
        {
            routes.Ignore("lvmama");
            routes.MapPageRoute("apply_code", "lvmama/apply_code", "~/lvmama/apply_code.aspx", false);
            routes.MapPageRoute("discard_code", "lvmama/discard_code", "~/lvmama/discard_code.aspx", false);
            routes.MapPageRoute("sms_resend", "lvmama/sms_resend", "~/lvmama/sms_resend.aspx", false);
        }

    }
}
