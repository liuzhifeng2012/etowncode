using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.VAS.Service.VASService.Data;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.VAS.Service.VASService.Modle.Enum;
using ETS2.PM.Service.PMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle.Enum;
using ETS.Framework;
using System.Xml;
using Newtonsoft.Json;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.WebApp.UI.BusinessesUI
{
    public partial class ResendVerifyNotice : System.Web.UI.Page
    {
        private static object lockobj = new object();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            //string key = txtkey.Text.Trim();
            //if (key == "")
            //{
            //    Label222.InnerText = "请输入订单号/电子票号";
            //}

            //string pno = "";

            ////根据输入的关键词是否是电子码
            //B2b_eticket meticket = new B2bEticketData().GetEticketDetail(key);
            //if (meticket != null)
            //{
            //    pno = key;
            //}
            //else 
            //{
            //    //判断输入的关键词是否是订单号
            //    int orderid = key.ConvertTo<int>(0);
            //    if (orderid == 0)
            //    {
            //        Label222.InnerText = "订单号/电子票号输入有误";
            //        return;
            //    }
            //    else
            //    {
            //        B2b_order morder = new B2bOrderData().GetOrderById(orderid);
            //        if (morder == null)
            //        {
            //            Label222.InnerText = "订单查询有误";
            //            return;
            //        }
            //        else 
            //        {
            //            if (morder.Bindingagentorderid == 0)
            //            {
            //                B2b_order morder2 = new B2bOrderData().GetOrderById(morder.Bindingagentorderid);
            //                if (morder2 == null)
            //                {
            //                    Label222.InnerText = "绑定订单查询有误";
            //                    return;
            //                }
            //                else 
            //                {
            //                    pno = morder2.Pno;
            //                }
            //            }
            //            else
            //            {
            //                pno = morder.Pno;
            //            }
            //        }
            //    }
            //}


            ////判断关键词 在验证通知日志中是否有记录
            //int count = new Agent_asyncsendlogData().Getnoticelognum(pno);
            //if (count == 0)
            //{
            //    Label222.InnerText = "订单号/电子票号没有验证通知记录";
            //    return;
            //}


            ////判断关键词 在验证通知日志表中的通知错误记录
            //List<Agent_asyncsendlog> list = new Agent_asyncsendlogData().GetSendNoticeErrList(pno);
            //if (list.Count == 0)
            //{
            //    Label222.InnerText = "验证通知已经发送成功";
            //    return;
            //}
            //else
            //{
            //    string result = "";
            //    foreach (Agent_asyncsendlog log in list)
            //    {
            //        result += AsyncSend(log) + "<br>";
            //    }
            //    Label222.InnerText = result;
            //    return;
            //}
        }

        public string AsyncSend(Agent_asyncsendlog log1)
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

                                        return "糯米成功";
                                    }
                                    else
                                    {
                                        log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                                        new Agent_asyncsendlogData().EditLog(log);

                                        return "糯米失败:" + log.Agentupdatestatus;
                                    }


                                }
                                else
                                {
                                    log.Remark = info;
                                    log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                                    new Agent_asyncsendlogData().EditLog(log);

                                    return "糯米失败:" + log.Remark;
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
                                    return log.Remark;
                                }

                                if (eticketinfo.Oid == 0)
                                {
                                    log.Remark = "电子票对应的订单号为0";//单引号替换为'',双引号不用处理;
                                    log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                                    new Agent_asyncsendlogData().EditLog(log);
                                    return log.Remark;
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
                                    return log.Remark;
                                }

                                if (listagent_rlog.Count == 0)
                                {
                                    log.Remark = "根据订单号得到分销商订单请求记录失败.";//单引号替换为'',双引号不用处理;
                                    log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                                    new Agent_asyncsendlogData().EditLog(log);
                                    return log.Remark;
                                }


                                /*验证通知发送内容*/
                                string sbuilder = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<business_trans>" +
                                "<request_type>sync_order</request_type>" +//<!--验证同步-->
                                "<req_seq>{0}</req_seq>" +//<!--原订单请求流水号-->
                                "<platform_req_seq>{1}</platform_req_seq>" +//<!--平台请求流水号-->
                                "<order>" +//<!--订单信息-->
                                "<order_num>{2}</order_num>" +//<!-- 订单号 y-->
                                "<num>{3}</num>" +//<!-- 使用张数 -->
                                "<use_time>{4}</use_time>" +//<!--  使用时间  -->
                                "</order>" +
                                "</business_trans>", listagent_rlog[0].Req_seq, log.Platform_req_seq, eticketinfo.Oid, log.Num, ConvertDateTimeInt(log.Confirmtime));

                                string updateurl = agent_updateurl + "?xml=" + sbuilder;


                                string re = new GetUrlData().HttpPost(updateurl, "");

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

                                    return "成功";
                                }
                                else
                                {
                                    log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                                    new Agent_asyncsendlogData().EditLog(log);

                                    return re;
                                }

                            }
                            #endregion
                        }
                        else 
                        {
                            return "分销商信息获取失败";
                        }
                    }
                    else 
                    {
                        return "直销订单无需发送验证通知";
                    }

                }
                catch (Exception e)
                {
                    log.Id = inslog;
                    log.Remark = e.Message.Replace("'", "''");//单引号替换为'',双引号不用处理
                    new Agent_asyncsendlogData().EditLog(log);

                    return log.Remark;
                }
            }

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
    }
}