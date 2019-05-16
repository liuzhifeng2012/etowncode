using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using System.Web.Script.Serialization;
using ETS.Framework;
using System.Xml;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using Newtonsoft.Json;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS2.WeiXin.Service.WeiXinService.Model.Enum;

namespace ETS2.WeiXin.Service.WinXinService.BLL
{
    public class Weixin_tmplmsgManage
    {
        private static object lockobject = new object();
        /// <summary>
        /// 微信发送模板消息--新订单生成通知
        /// </summary>
        /// <param name="token"></param>
        /// <param name="comid"></param>
        /// <returns></returns>
        public void WxTmplMsg_OrderNewCreate(int orderid,string weixin="")
        {
            lock (lockobject)
            {
                string infotype = ((int)WxTmplType.OrderNewCreate).ToString();//模板类型:新订单生成

                B2b_order m_order = new B2bOrderData().GetOrderById(orderid);
                if (m_order == null)
                {
                    return;
                }
                if (weixin == "")
                {
                    weixin = new B2bCrmData().GetWeiXinByCrmid(m_order.U_id);
                }
                if (weixin == "")
                {
                    return;
                }

                Weixin_template m_tmpl = new Weixin_templateData().GetWeixinTmpl(m_order.Comid, infotype);
                if (m_tmpl == null)
                {
                    return;
                }
                if (m_tmpl.Template_id == "")
                {
                    return;
                }
                m_tmpl.First_DATA = "新订单生成通知";


                string json_param = "{" +
                                "\"touser\":\"{{touser.DATA}}\"," +
                                "\"template_id\":\"{{template_id.DATA}}\"," +
                                "\"url\":\"{{url.DATA}}\"," +
                                "\"topcolor\":\"#FF0000\"," +
                                "\"data\":{" +
                                "\"first\": {" +
                                "\"value\":\"{{first.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"OrderId\":{" +
                                "\"value\":\"{{OrderId.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"ProductId\":{" +
                                "\"value\":\"{{ProductId.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"ProductName\":{" +
                                "\"value\":\"{{ProductName.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"remark\":{" +
                                "\"value\":\"{{remark.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}" +
                                "}" +
                                "}";


                Weixin_templatemsg_sendlog log = new Weixin_templatemsg_sendlog
                {
                    Id = 0,
                    Msg_send_content = json_param,
                    Touser = "",
                    Template_id = "",
                    Url = "",
                    Msg_send_createtime = DateTime.Now,
                    Msg_call_content = "",
                    Msg_call_errcode = "",
                    Msg_call_errmsg = "",
                    Msgid = "",
                    Msg_push_content = "",
                    Msg_push_CreateTime = "",
                    Msg_push_CreateTime_format = DateTime.Parse("1970-01-01"),
                    Msg_push_status = "",
                    Orderid = orderid,
                    Public_account = "",
                    Comid = 0,
                    Remark = "",
                    Infotype = infotype
                };
                int logid = new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                if (logid == 0)
                {
                    return;
                }
                else
                {
                    log.Id = logid;
                }
                try
                {
                    log.Comid = m_order.Comid;
                    int comid = m_order.Comid;
                    WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(comid);
                    if (basicc == null)
                    {
                        log.Remark = "公司获取失败";
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                        return;
                    }
                    WXAccessToken token = GetAccessToken(basicc.Comid, basicc.AppId, basicc.AppSecret);

                    log.Template_id = m_tmpl.Template_id;
                    log.Touser = weixin;

                    //对同一订单 模板消息一旦发送成功，不在重复发送
                    bool issendsuc = new Weixin_templatemsg_sendlogData().Issendsuc(orderid, m_tmpl.Template_id);
                    if (issendsuc)
                    {
                        log.Remark = "对同一订单 模板消息一旦发送成功，不在重复发送";
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                        return;
                    }

                    B2b_com_pro m_pro = new B2bComProData().GetProById(m_order.Pro_id.ToString(),m_order.Speciid);
                    if (m_pro == null)
                    {
                        log.Remark = "产品获取失败";
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                        return;
                    }

                    string requesturl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + token.ACCESS_TOKEN;

                    string linkurl = "http://shop" + comid + ".etown.cn/h5/pay.aspx?orderid=" + orderid;
                    log.Url = linkurl;

                    json_param = json_param.Replace("{{touser.DATA}}", weixin);
                    json_param = json_param.Replace("{{template_id.DATA}}", m_tmpl.Template_id);
                    json_param = json_param.Replace("{{url.DATA}}", linkurl);
                    json_param = json_param.Replace("{{first.DATA}}", m_tmpl.First_DATA);
                    json_param = json_param.Replace("{{OrderId.DATA}}", orderid.ToString());
                    json_param = json_param.Replace("{{ProductId.DATA}}", m_pro.Id.ToString());
                    json_param = json_param.Replace("{{ProductName.DATA}}", m_pro.Pro_name);
                    json_param = json_param.Replace("{{remark.DATA}}", m_tmpl.Remark_DATA);

                    log.Msg_send_content = json_param;
                    new Weixin_templatemsg_sendlogData().EditTmplLog(log);

                    string ret = new GetUrlData().HttpPost(requesturl, json_param);
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    OuterClass foo = ser.Deserialize<OuterClass>(ret);
                    if (foo.errcode != null)
                    {
                        log.Msg_call_content = ret;
                        log.Msg_call_errcode = foo.errcode;
                        log.Msg_call_errmsg = foo.errmsg;
                        if (foo.errcode == "0")
                        {
                            log.Msgid = foo.msgid;
                        }
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                    }
                    else
                    {
                        log.Remark = "调用模板消息后返回失败";
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                    }
                }
                catch (Exception e)
                {
                    log.Remark = "意外错误";
                    new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                }
            }
        }

        /// <summary>
        /// 微信发送模板消息--订单状态变更通知(暂时只是在订单支付成功 和 酒店订单取消时调用)
        /// </summary>
        /// <param name="token"></param>
        /// <param name="comid"></param>
        /// <returns></returns>
        public void WxTmplMsg_OrderStatusChange(int orderid)
        {
            lock (lockobject)
            {
                string infotype = ((int)WxTmplType.OrderStatusChange).ToString();//模板类型:订单状态变更通知

                B2b_order m_order = new B2bOrderData().GetOrderById(orderid);
                if (m_order == null)
                {
                    return;
                }
                Weixin_template m_tmpl = new Weixin_templateData().GetWeixinTmpl(m_order.Comid, infotype);
                if (m_tmpl == null)
                {
                    return;
                }
                if (m_tmpl.Template_id == "")
                {
                    return;
                }

                //m_tmpl.First_DATA = "订单状态变更通知";
                if (m_order.Order_state == (int)OrderStatus.HasPay)
                {
                    m_tmpl.First_DATA = "订单支付成功";
                }
                if (m_order.Order_state == (int)OrderStatus.InvalidOrder)
                {
                    m_tmpl.First_DATA = "订单已取消";
                }

                string weixin = new B2bCrmData().GetWeiXinByCrmid(m_order.U_id);
                if (weixin == "")
                {
                    return;
                }

                string json_param = "{" +
                                 "\"touser\":\"{{touser.DATA}}\"," +
                                 "\"template_id\":\"{{template_id.DATA}}\"," +
                                 "\"url\":\"{{url.DATA}}\"," +
                                 "\"topcolor\":\"#FF0000\"," +
                                 "\"data\":{" +
                                 "\"first\": {" +
                                 "\"value\":\"{{first.DATA}}\"," +
                                 "\"color\":\"#173177\"" +
                                 "}," +
                                 "\"orderId\":{" +
                                 "\"value\":\"{{orderId.DATA}}\"," +
                                 "\"color\":\"#173177\"" +
                                 "}," +
                                 "\"orderPrice\":{" +
                                 "\"value\":\"{{orderPrice.DATA}}\"," +
                                 "\"color\":\"#173177\"" +
                                 "}," +
                                 "\"orderStatus\":{" +
                                 "\"value\":\"{{orderStatus.DATA}}\"," +
                                 "\"color\":\"#173177\"" +
                                 "}," +
                                  "\"productName\":{" +
                                 "\"value\":\"{{productName.DATA}}\"," +
                                 "\"color\":\"#173177\"" +
                                 "}," +
                                 "\"remark\":{" +
                                 "\"value\":\"{{remark.DATA}}\"," +
                                 "\"color\":\"#173177\"" +
                                 "}" +
                                 "}" +
                                 "}";


                Weixin_templatemsg_sendlog log = new Weixin_templatemsg_sendlog
                {
                    Id = 0,
                    Msg_send_content = json_param,
                    Touser = "",
                    Template_id = "",
                    Url = "",
                    Msg_send_createtime = DateTime.Now,
                    Msg_call_content = "",
                    Msg_call_errcode = "",
                    Msg_call_errmsg = "",
                    Msgid = "",
                    Msg_push_content = "",
                    Msg_push_CreateTime = "",
                    Msg_push_CreateTime_format = DateTime.Parse("1970-01-01"),
                    Msg_push_status = "",
                    Orderid = orderid,
                    Public_account = "",
                    Comid = 0,
                    Remark = "",
                    Infotype = infotype
                };
                int logid = new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                if (logid == 0)
                {
                    return;
                }
                else
                {
                    log.Id = logid;
                }
                try
                {

                    log.Comid = m_order.Comid;
                    int comid = m_order.Comid;
                    WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(comid);
                    if (basicc == null)
                    {
                        log.Remark = "公司获取失败";
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                        return;
                    }

                    WXAccessToken token = GetAccessToken(basicc.Comid, basicc.AppId, basicc.AppSecret);

                    log.Template_id = m_tmpl.Template_id;
                    log.Touser = weixin;

                    //对同一订单 模板消息一旦发送成功，不在重复发送
                    bool issendsuc = new Weixin_templatemsg_sendlogData().Issendsuc(orderid, m_tmpl.Template_id);
                    if (issendsuc)
                    {
                        log.Remark = "对同一订单 模板消息一旦发送成功，不在重复发送";
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                        return;
                    }

                    B2b_com_pro m_pro = new B2bComProData().GetProById(m_order.Pro_id.ToString());
                    if (m_pro == null)
                    {
                        log.Remark = "产品获取失败";
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                        return;
                    }

                    //暂时只是在订单支付成功 和 酒店订单取消时调用
                    int iscansend = 0;
                    if (m_order.Order_state == (int)OrderStatus.HasPay)
                    {
                        iscansend = 1;
                    }
                    else
                    {
                        if (m_order.Order_state == (int)OrderStatus.Hotecannel && m_pro.Server_type == 9)
                        {
                            iscansend = 1;
                        }
                    }

                    if (iscansend == 0)
                    {
                        if (m_pro.Server_type == 1)
                        {
                            string server_typename = "票务";
                            log.Remark = "暂时只是在订单支付成功 和 酒店订单取消时调用：产品服务类型(" + server_typename + "),订单类型(" + EnumUtils.GetName((OrderStatus)m_order.Order_state) + ")";
                        }
                        if (m_pro.Server_type == 2)
                        {
                            string server_typename = "跟团游";
                            log.Remark = "暂时只是在订单支付成功 和 酒店订单取消时调用：产品服务类型(" + server_typename + "),订单类型(" + EnumUtils.GetName((OrderStatus)m_order.Order_state) + ")";
                        }
                        if (m_pro.Server_type == 8)
                        {
                            string server_typename = "当地游";
                            log.Remark = "暂时只是在订单支付成功 和 酒店订单取消时调用：产品服务类型(" + server_typename + "),订单类型(" + EnumUtils.GetName((OrderStatus)m_order.Order_state) + ")";
                        }
                        if (m_pro.Server_type == 9)
                        {
                            string server_typename = "酒店客房";
                            log.Remark = "暂时只是在订单支付成功 和 酒店订单取消时调用：产品服务类型(" + server_typename + "),订单类型(" + EnumUtils.GetName((OrderStatus)m_order.Order_state) + ")";
                        }


                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                        return;
                    }


                    string requesturl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + token.ACCESS_TOKEN;

                    string linkurl = "";
                    log.Url = linkurl;

                    decimal totalpricec = m_order.Pay_price * m_order.U_num;
                    json_param = json_param.Replace("{{touser.DATA}}", weixin);
                    json_param = json_param.Replace("{{template_id.DATA}}", m_tmpl.Template_id);
                    json_param = json_param.Replace("{{url.DATA}}", linkurl);
                    json_param = json_param.Replace("{{first.DATA}}", m_tmpl.First_DATA);
                    json_param = json_param.Replace("{{orderId.DATA}}", orderid.ToString());
                    json_param = json_param.Replace("{{orderPrice.DATA}}", totalpricec.ToString("f2"));
                    json_param = json_param.Replace("{{orderStatus.DATA}}", EnumUtils.GetName((OrderStatus)m_order.Order_state));
                    json_param = json_param.Replace("{{productName.DATA}}", m_pro.Pro_name);
                    json_param = json_param.Replace("{{remark.DATA}}", m_tmpl.Remark_DATA);

                    log.Msg_send_content = json_param;

                    new Weixin_templatemsg_sendlogData().EditTmplLog(log);

                    string ret = new GetUrlData().HttpPost(requesturl, json_param);
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    OuterClass foo = ser.Deserialize<OuterClass>(ret);
                    if (foo.errcode != null)
                    {
                        log.Msg_call_content = ret;
                        log.Msg_call_errcode = foo.errcode;
                        log.Msg_call_errmsg = foo.errmsg;
                        log.Msgid = foo.msgid;

                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                    }
                    else
                    {
                        log.Remark = "调用模板消息后返回失败";
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                    }
                }
                catch (Exception e)
                {
                    log.Remark = "意外错误";
                    new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                }
            }
        }

        /// <summary>
        /// 微信发送模板消息--门票订单预订成功通知(发码成功情况下发送)
        /// </summary>
        /// <param name="token"></param>
        /// <param name="comid"></param>
        /// <returns></returns>
        public void WxTmplMsg_OrderSendCodeSuc(int orderid)
        {
            lock (lockobject)
            {
                string infotype = ((int)WxTmplType.OrderSendCodeSuc).ToString();//模板类型:订单状态变更通知
                B2b_order m_order = new B2bOrderData().GetOrderById(orderid);
                if (m_order == null)
                {
                    return;
                }

                Weixin_template m_tmpl = new Weixin_templateData().GetWeixinTmpl(m_order.Comid, infotype);
                if (m_tmpl == null)
                {
                    return;
                }
                if (m_tmpl.Template_id == "")
                {
                    return;
                }
                m_tmpl.First_DATA = "发码成功通知";

                string weixin = new B2bCrmData().GetWeiXinByCrmid(m_order.U_id);
                if (weixin == "")
                {
                    return;
                }

                string json_param = "{" +
                                "\"touser\":\"{{touser.DATA}}\"," +
                                "\"template_id\":\"{{template_id.DATA}}\"," +
                                "\"url\":\"{{url.DATA}}\"," +
                                "\"topcolor\":\"#FF0000\"," +
                                "\"data\":{" +
                                "\"first\": {" +
                                "\"value\":\"{{first.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"OrderID\":{" +
                                "\"value\":\"{{OrderID.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"PkgName\":{" +
                                "\"value\":\"{{PkgName.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"TakeOffDate\":{" +
                                "\"value\":\"{{TakeOffDate.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"remark\":{" +
                                "\"value\":\"{{remark.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}" +
                                "}" +
                                "}";


                Weixin_templatemsg_sendlog log = new Weixin_templatemsg_sendlog
                {
                    Id = 0,
                    Msg_send_content = json_param,
                    Touser = "",
                    Template_id = "",
                    Url = "",
                    Msg_send_createtime = DateTime.Now,
                    Msg_call_content = "",
                    Msg_call_errcode = "",
                    Msg_call_errmsg = "",
                    Msgid = "",
                    Msg_push_content = "",
                    Msg_push_CreateTime = "",
                    Msg_push_CreateTime_format = DateTime.Parse("1970-01-01"),
                    Msg_push_status = "",
                    Orderid = orderid,
                    Public_account = "",
                    Comid = 0,
                    Remark = "",
                    Infotype = infotype
                };
                int logid = new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                if (logid == 0)
                {
                    return;
                }
                else
                {
                    log.Id = logid;
                }
                try
                {
                    log.Comid = m_order.Comid;

                    int comid = m_order.Comid;
                    WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(comid);
                    if (basicc == null)
                    {
                        log.Remark = "公司获取失败";
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                        return;
                    }

                    WXAccessToken token = GetAccessToken(basicc.Comid, basicc.AppId, basicc.AppSecret);


                    log.Template_id = m_tmpl.Template_id;
                    log.Touser = weixin;

                    //对同一订单 模板消息一旦发送成功，不在重复发送
                    bool issendsuc = new Weixin_templatemsg_sendlogData().Issendsuc(orderid, m_tmpl.Template_id);
                    if (issendsuc)
                    {
                        log.Remark = "对同一订单 模板消息一旦发送成功，不在重复发送";
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                        return;
                    }

                    B2b_com_pro m_pro = new B2bComProData().GetProById(m_order.Pro_id.ToString());
                    if (m_pro == null)
                    {
                        log.Remark = "产品获取失败";
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                        return;
                    }

                    //暂时只是在订单发码成功时调用
                    int iscansend = 0;
                    if (m_order.Order_state == (int)OrderStatus.HasSendCode)
                    {
                        iscansend = 1;
                    }


                    if (iscansend == 0)
                    {
                        log.Remark = "暂时只是在订单发码成功时调用：订单类型(" + EnumUtils.GetName((OrderStatus)m_order.Order_state) + ")";
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                        return;
                    }


                    string requesturl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + token.ACCESS_TOKEN;

                    B2b_com_pro modelcompro = m_pro;
                    #region 产品有效期判定(微信模板--门票订单预订成功通知 中也有用到)
                    string provalidatemethod = modelcompro.ProValidateMethod;
                    int appointdate = modelcompro.Appointdata;
                    int iscanuseonsameday = modelcompro.Iscanuseonsameday;

                    DateTime pro_end = modelcompro.Pro_end;
                    if (provalidatemethod == "2")//按指定有效期
                    {
                        if (appointdate == (int)ProAppointdata.NotAppoint)
                        {
                        }
                        else if (appointdate == (int)ProAppointdata.OneWeek)
                        {
                            pro_end = DateTime.Now.AddDays(7);
                        }
                        else if (appointdate == (int)ProAppointdata.OneMonth)
                        {
                            pro_end = DateTime.Now.AddMonths(1);
                        }
                        else if (appointdate == (int)ProAppointdata.ThreeMonth)
                        {
                            pro_end = DateTime.Now.AddMonths(3);
                        }
                        else if (appointdate == (int)ProAppointdata.SixMonth)
                        {
                            pro_end = DateTime.Now.AddMonths(6);
                        }
                        else if (appointdate == (int)ProAppointdata.OneYear)
                        {
                            pro_end = DateTime.Now.AddYears(1);
                        }

                        //如果指定有效期大于产品有效期，则按产品有效期
                        if (pro_end > modelcompro.Pro_end)
                        {
                            pro_end = modelcompro.Pro_end;
                        }
                    }
                    else //按产品有效期
                    {
                        pro_end = modelcompro.Pro_end;
                    }

                    DateTime pro_start = modelcompro.Pro_start;
                    DateTime nowday = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                    if (iscanuseonsameday == 1)//当天可用  
                    {
                        if (nowday < pro_start)//当天日期小于产品起始日期
                        {
                            pro_start = modelcompro.Pro_start;
                        }
                        else
                        {
                            pro_start = nowday;
                        }
                    }
                    else //当天不可用
                    {
                        if (nowday < pro_start)//当天日期小于产品起始日期
                        {
                            pro_start = modelcompro.Pro_start;
                        }
                        else
                        {
                            pro_start = nowday.AddDays(1);
                        }
                    }
                    #endregion


                    string linkurl = "";
                    log.Url = linkurl;
                    json_param = json_param.Replace("{{touser.DATA}}", weixin);
                    json_param = json_param.Replace("{{template_id.DATA}}", m_tmpl.Template_id);
                    json_param = json_param.Replace("{{url.DATA}}", linkurl);
                    json_param = json_param.Replace("{{first.DATA}}", m_tmpl.First_DATA);
                    json_param = json_param.Replace("{{OrderID.DATA}}", orderid.ToString());
                    json_param = json_param.Replace("{{PkgName.DATA}}", m_pro.Pro_name);
                    json_param = json_param.Replace("{{TakeOffDate.DATA}}", pro_start.ToString("yyyy.MM.dd") + "-" + pro_end.ToString("yyyy.MM.dd"));
                    json_param = json_param.Replace("{{remark.DATA}}", "辅码:" + m_order.Pno + m_tmpl.Remark_DATA);

                    log.Msg_send_content = json_param;

                    new Weixin_templatemsg_sendlogData().EditTmplLog(log);

                    string ret = new GetUrlData().HttpPost(requesturl, json_param);
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    OuterClass foo = ser.Deserialize<OuterClass>(ret);
                    if (foo.errcode != null)
                    {
                        log.Msg_call_content = ret;
                        log.Msg_call_errcode = foo.errcode;
                        log.Msg_call_errmsg = foo.errmsg;
                        log.Msgid = foo.msgid;

                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                    }
                    else
                    {
                        log.Remark = "调用模板消息后返回失败";
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                    }
                }
                catch (Exception e)
                {
                    log.Remark = "意外错误";
                    new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                }
            }
        }


        /// <summary>
        /// 微信发送模板消息--酒店预订确认通知(酒店方确认订单情况下发送)
        /// </summary>
        /// <param name="token"></param>
        /// <param name="comid"></param>
        /// <returns></returns>
        public void WxTmplMsg_OrderHotelConfirm(int orderid)
        {
            lock (lockobject)
            {
                string infotype = ((int)WxTmplType.OrderHotelConfirm).ToString();//模板类型:酒店预订确认通知
                B2b_order m_order = new B2bOrderData().GetOrderById(orderid);
                if (m_order == null)
                {
                    return;
                }
                Weixin_template m_tmpl = new Weixin_templateData().GetWeixinTmpl(m_order.Comid, infotype);
                if (m_tmpl == null)
                {
                    return;
                }
                if (m_tmpl.Template_id == "")
                {
                    return;
                }
                m_tmpl.First_DATA = "酒店预订确认通知";

                string weixin = new B2bCrmData().GetWeiXinByCrmid(m_order.U_id);
                if (weixin == "")
                {
                    return;
                }

                string json_param = "{" +
                                "\"touser\":\"{{touser.DATA}}\"," +
                                "\"template_id\":\"{{template_id.DATA}}\"," +
                                "\"url\":\"{{url.DATA}}\"," +
                                "\"topcolor\":\"#FF0000\"," +
                                "\"data\":{" +
                                "\"first\": {" +
                                "\"value\":\"{{first.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"order\":{" +
                                "\"value\":\"{{order.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"Name\":{" +
                                "\"value\":\"{{Name.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"datein\":{" +
                                "\"value\":\"{{datein.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                 "\"dateout\":{" +
                                "\"value\":\"{{dateout.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                 "\"number\":{" +
                                "\"value\":\"{{number.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                 "\"room type\":{" +
                                "\"value\":\"{{room type.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                 "\"pay\":{" +
                                "\"value\":\"{{pay.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"remark\":{" +
                                "\"value\":\"{{remark.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}" +
                                "}" +
                                "}";

                Weixin_templatemsg_sendlog log = new Weixin_templatemsg_sendlog
                {
                    Id = 0,
                    Msg_send_content = json_param,
                    Touser = "",
                    Template_id = "",
                    Url = "",
                    Msg_send_createtime = DateTime.Now,
                    Msg_call_content = "",
                    Msg_call_errcode = "",
                    Msg_call_errmsg = "",
                    Msgid = "",
                    Msg_push_content = "",
                    Msg_push_CreateTime = "",
                    Msg_push_CreateTime_format = DateTime.Parse("1970-01-01"),
                    Msg_push_status = "",
                    Orderid = orderid,
                    Public_account = "",
                    Comid = 0,
                    Remark = "",
                    Infotype = infotype
                };
                int logid = new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                if (logid == 0)
                {
                    return;
                }
                else
                {
                    log.Id = logid;
                }
                try
                {

                    log.Comid = m_order.Comid;
                    int comid = m_order.Comid;
                    WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(comid);
                    if (basicc == null)
                    {
                        log.Remark = "公司获取失败";
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                        return;
                    }

                    WXAccessToken token = GetAccessToken(basicc.Comid, basicc.AppId, basicc.AppSecret);



                    log.Template_id = m_tmpl.Template_id;
                    log.Touser = weixin;

                    //对同一订单 模板消息一旦发送成功，不在重复发送
                    bool issendsuc = new Weixin_templatemsg_sendlogData().Issendsuc(orderid, m_tmpl.Template_id);
                    if (issendsuc)
                    {
                        log.Remark = "对同一订单 模板消息一旦发送成功，不在重复发送";
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                        return;
                    }

                    B2b_com_pro m_pro = new B2bComProData().GetProById(m_order.Pro_id.ToString());
                    if (m_pro == null)
                    {
                        log.Remark = "产品获取失败";
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                        return;
                    }

                    B2b_order_hotel m_orderhotel = new B2b_order_hotelData().GetHotelOrderByOrderId(orderid);
                    if (m_orderhotel == null)
                    {
                        log.Remark = "酒店订单获取失败";
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                        return;
                    }

                    //必须为酒店订单
                    int iscansend = 0;
                    if (m_pro.Server_type == 9)
                    {
                        iscansend = 1;
                    }


                    if (iscansend == 0)
                    {
                        if (m_pro.Server_type == 1)
                        {
                            string server_typename = "票务";
                            log.Remark = "暂时只是在订单支付成功 和 酒店订单取消时调用：产品服务类型(" + server_typename + "),订单类型(" + EnumUtils.GetName((OrderStatus)m_order.Order_state) + ")";
                        }
                        if (m_pro.Server_type == 2)
                        {
                            string server_typename = "跟团游";
                            log.Remark = "暂时只是在订单支付成功 和 酒店订单取消时调用：产品服务类型(" + server_typename + "),订单类型(" + EnumUtils.GetName((OrderStatus)m_order.Order_state) + ")";
                        }
                        if (m_pro.Server_type == 8)
                        {
                            string server_typename = "当地游";
                            log.Remark = "暂时只是在订单支付成功 和 酒店订单取消时调用：产品服务类型(" + server_typename + "),订单类型(" + EnumUtils.GetName((OrderStatus)m_order.Order_state) + ")";
                        }
                        if (m_pro.Server_type == 9)
                        {
                            string server_typename = "酒店客房";
                            log.Remark = "暂时只是在订单支付成功 和 酒店订单取消时调用：产品服务类型(" + server_typename + "),订单类型(" + EnumUtils.GetName((OrderStatus)m_order.Order_state) + ")";
                        }


                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                        return;
                    }


                    string requesturl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + token.ACCESS_TOKEN;

                    string linkurl = "";
                    log.Url = linkurl;
                    json_param = json_param.Replace("{{touser.DATA}}", weixin);
                    json_param = json_param.Replace("{{template_id.DATA}}", m_tmpl.Template_id);
                    json_param = json_param.Replace("{{url.DATA}}", linkurl);
                    json_param = json_param.Replace("{{first.DATA}}", m_tmpl.First_DATA);
                    json_param = json_param.Replace("{{order.DATA}}", orderid.ToString());
                    json_param = json_param.Replace("{{Name.DATA}}", m_order.U_name);
                    json_param = json_param.Replace("{{datein.DATA}}", m_orderhotel.Start_date.ToString("yyyy-MM-dd"));
                    json_param = json_param.Replace("{{dateout.DATA}}", m_orderhotel.End_date.ToString("yyyy-MM-dd"));
                    json_param = json_param.Replace("{{number.DATA}}", m_order.U_num.ToString());
                    json_param = json_param.Replace("{{room type.DATA}}", m_pro.Pro_name);
                    json_param = json_param.Replace("{{pay.DATA}}", m_order.Pay_price.ToString("f2"));
                    json_param = json_param.Replace("{{remark.DATA}}", m_tmpl.Remark_DATA);

                    log.Msg_send_content = json_param;

                    new Weixin_templatemsg_sendlogData().EditTmplLog(log);

                    string ret = new GetUrlData().HttpPost(requesturl, json_param);
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    OuterClass foo = ser.Deserialize<OuterClass>(ret);
                    if (foo.errcode != null)
                    {
                        log.Msg_call_content = ret;
                        log.Msg_call_errcode = foo.errcode;
                        log.Msg_call_errmsg = foo.errmsg;
                        log.Msgid = foo.msgid;

                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                    }
                    else
                    {
                        log.Remark = "调用模板消息后返回失败";
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                    }
                }
                catch (Exception e)
                {
                    log.Remark = "意外错误";
                    new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                }
            }
        }


        /// <summary>
        /// 微信发送模板消息--会员充值通知
        /// </summary>
        /// <param name="comid">公司id</param>
        /// <param name="weixin">微信</param>
        /// <param name="first">标题</param>
        /// <param name="accountType">账户类型(如：会员卡号..)</param>
        /// <param name="account">账户(如：卡号..)</param>
        /// <param name="amount">充值金额</param>
        /// <param name="result">充值状态(如:充值成功..)</param>
        /// <param name="remark">备注</param>
        public void WxTmplMsg_CrmRecharge(int comid, string weixin, string first, string accountType, string account, string amount, string result, string remark)
        {
            lock (lockobject)
            {
                string infotype = ((int)WxTmplType.CrmRecharge).ToString();//模板类型:会员充值通知 

                if (comid <= 0)
                {
                    return;
                }
                if (weixin == "")
                {
                    return;
                }

                Weixin_template m_tmpl = new Weixin_templateData().GetWeixinTmpl(comid, infotype);
                if (m_tmpl == null)
                {
                    return;
                }
                if (m_tmpl.Template_id == "")
                {
                    return;
                }
                m_tmpl.First_DATA = first;


                string json_param = "{" +
                                "\"touser\":\"{{touser.DATA}}\"," +
                                "\"template_id\":\"{{template_id.DATA}}\"," +
                                "\"url\":\"{{url.DATA}}\"," +
                                "\"topcolor\":\"#FF0000\"," +
                                "\"data\":{" +
                                "\"first\": {" +
                                "\"value\":\"{{first.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"accountType\":{" +
                                "\"value\":\"{{accountType.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"account\":{" +
                                "\"value\":\"{{account.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"amount\":{" +
                                "\"value\":\"{{amount.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                 "\"result\":{" +
                                "\"value\":\"{{result.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"remark\":{" +
                                "\"value\":\"{{remark.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}" +
                                "}" +
                                "}";


                Weixin_templatemsg_sendlog log = new Weixin_templatemsg_sendlog
                {
                    Id = 0,
                    Msg_send_content = json_param,
                    Touser = "",
                    Template_id = "",
                    Url = "",
                    Msg_send_createtime = DateTime.Now,
                    Msg_call_content = "",
                    Msg_call_errcode = "",
                    Msg_call_errmsg = "",
                    Msgid = "",
                    Msg_push_content = "",
                    Msg_push_CreateTime = "",
                    Msg_push_CreateTime_format = DateTime.Parse("1970-01-01"),
                    Msg_push_status = "",
                    Orderid = 0,
                    Public_account = "",
                    Comid = 0,
                    Remark = "",
                    Infotype = infotype
                };
                int logid = new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                if (logid == 0)
                {
                    return;
                }
                else
                {
                    log.Id = logid;
                }
                try
                {
                    log.Comid = comid;

                    WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(comid);
                    if (basicc == null)
                    {
                        log.Remark = "公司获取失败";
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                        return;
                    }
                    WXAccessToken token = GetAccessToken(basicc.Comid, basicc.AppId, basicc.AppSecret);

                    log.Template_id = m_tmpl.Template_id;
                    log.Touser = weixin;

                    string requesturl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + token.ACCESS_TOKEN;

                    string linkurl = "";
                    log.Url = linkurl;

                    json_param = json_param.Replace("{{touser.DATA}}", weixin);
                    json_param = json_param.Replace("{{template_id.DATA}}", m_tmpl.Template_id);
                    json_param = json_param.Replace("{{url.DATA}}", linkurl);
                    json_param = json_param.Replace("{{first.DATA}}", m_tmpl.First_DATA);
                    json_param = json_param.Replace("{{accountType.DATA}}", accountType);
                    json_param = json_param.Replace("{{account.DATA}}", account);
                    json_param = json_param.Replace("{{amount.DATA}}", amount);
                    json_param = json_param.Replace("{{result.DATA}}", result);
                    json_param = json_param.Replace("{{remark.DATA}}", remark);

                    log.Msg_send_content = json_param;
                    new Weixin_templatemsg_sendlogData().EditTmplLog(log);

                    string ret = new GetUrlData().HttpPost(requesturl, json_param);
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    OuterClass foo = ser.Deserialize<OuterClass>(ret);
                    if (foo.errcode != null)
                    {
                        log.Msg_call_content = ret;
                        log.Msg_call_errcode = foo.errcode;
                        log.Msg_call_errmsg = foo.errmsg;
                        if (foo.errcode == "0")
                        {
                            log.Msgid = foo.msgid;
                        }
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                    }
                    else
                    {
                        log.Remark = "调用模板消息后返回失败";
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                    }
                }
                catch (Exception e)
                {
                    log.Remark = "意外错误";
                    new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                }
            }
        }


        /// <summary>
        /// 微信发送模板消息--会员消费通知
        /// </summary>
        /// <param name="comid">公司id</param>
        /// <param name="weixin">微信</param>
        /// <param name="productType">产品类型(例如:商品名..)</param>
        /// <param name="name">产品名称(例如:微信某某店某商品..)</param>
        /// <param name="accountType">账户类型(如：会员卡号..)</param>
        /// <param name="account">账户(如：卡号..)</param>
        /// <param name="time">消费时间</param>
        /// <param name="remark">备注</param>
        public void WxTmplMsg_CrmConsume(int comid, string weixin, string productType, string name, string accountType, string account, string time, string remark)
        {
            lock (lockobject)
            {
                string infotype = ((int)WxTmplType.CrmConsume).ToString();//模板类型:会员消费通知 

                if (comid <= 0)
                {
                    return;
                }
                if (weixin == "")
                {
                    return;
                }

                Weixin_template m_tmpl = new Weixin_templateData().GetWeixinTmpl(comid, infotype);
                if (m_tmpl == null)
                {
                    return;
                }
                if (m_tmpl.Template_id == "")
                {
                    return;
                }



                string json_param = "{" +
                                "\"touser\":\"{{touser.DATA}}\"," +
                                "\"template_id\":\"{{template_id.DATA}}\"," +
                                "\"url\":\"{{url.DATA}}\"," +
                                "\"topcolor\":\"#FF0000\"," +
                                "\"data\":{" +
                                "\"productType\": {" +
                                "\"value\":\"{{productType.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                  "\"name\": {" +
                                "\"value\":\"{{name.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"accountType\":{" +
                                "\"value\":\"{{accountType.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"account\":{" +
                                "\"value\":\"{{account.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"time\":{" +
                                "\"value\":\"{{time.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"remark\":{" +
                                "\"value\":\"{{remark.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}" +
                                "}" +
                                "}";


                Weixin_templatemsg_sendlog log = new Weixin_templatemsg_sendlog
                {
                    Id = 0,
                    Msg_send_content = json_param,
                    Touser = "",
                    Template_id = "",
                    Url = "",
                    Msg_send_createtime = DateTime.Now,
                    Msg_call_content = "",
                    Msg_call_errcode = "",
                    Msg_call_errmsg = "",
                    Msgid = "",
                    Msg_push_content = "",
                    Msg_push_CreateTime = "",
                    Msg_push_CreateTime_format = DateTime.Parse("1970-01-01"),
                    Msg_push_status = "",
                    Orderid = 0,
                    Public_account = "",
                    Comid = 0,
                    Remark = "",
                    Infotype = infotype
                };
                int logid = new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                if (logid == 0)
                {
                    return;
                }
                else
                {
                    log.Id = logid;
                }
                try
                {
                    log.Comid = comid;

                    WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(comid);
                    if (basicc == null)
                    {
                        log.Remark = "公司获取失败";
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                        return;
                    }
                    WXAccessToken token = GetAccessToken(basicc.Comid, basicc.AppId, basicc.AppSecret);

                    log.Template_id = m_tmpl.Template_id;
                    log.Touser = weixin;

                    string requesturl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + token.ACCESS_TOKEN;

                    string linkurl = "";
                    log.Url = linkurl;

                    json_param = json_param.Replace("{{touser.DATA}}", weixin);
                    json_param = json_param.Replace("{{template_id.DATA}}", m_tmpl.Template_id);
                    json_param = json_param.Replace("{{url.DATA}}", linkurl);
                    json_param = json_param.Replace("{{productType.DATA}}", productType);
                    json_param = json_param.Replace("{{name.DATA}}", name);
                    json_param = json_param.Replace("{{accountType.DATA}}", accountType);
                    json_param = json_param.Replace("{{account.DATA}}", account);
                    json_param = json_param.Replace("{{time.DATA}}", time);
                    json_param = json_param.Replace("{{remark.DATA}}", remark);

                    log.Msg_send_content = json_param;
                    new Weixin_templatemsg_sendlogData().EditTmplLog(log);

                    string ret = new GetUrlData().HttpPost(requesturl, json_param);
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    OuterClass foo = ser.Deserialize<OuterClass>(ret);
                    if (foo.errcode != null)
                    {
                        log.Msg_call_content = ret;
                        log.Msg_call_errcode = foo.errcode;
                        log.Msg_call_errmsg = foo.errmsg;
                        if (foo.errcode == "0")
                        {
                            log.Msgid = foo.msgid;
                        }
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                    }
                    else
                    {
                        log.Remark = "调用模板消息后返回失败";
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                    }
                }
                catch (Exception e)
                {
                    log.Remark = "意外错误";
                    new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                }
            }
        }


        /// <summary>
        ///  微信发送模板消息--积分奖励提醒
        /// </summary>
        /// <param name="comid">公司id</param>
        /// <param name="weixin">微信</param>
        /// <param name="first">标题</param>
        /// <param name="keyword1">账户</param>
        /// <param name="keyword2">时间</param>
        /// <param name="keyword3">类型</param>
        /// <param name="keyword4">奖励积分</param>
        /// <param name="keyword5">总积分</param>
        /// <param name="remark">备注</param>
        public void WxTmplMsg_CrmIntegralReward(int comid, string weixin, string first, string keyword1, string keyword2, string keyword3, string keyword4, string keyword5,string remark)
        {
            lock (lockobject)
            {
                string infotype = ((int)WxTmplType.CrmIntegralReward).ToString();//模板类型:积分奖励提醒 

                if (comid <= 0)
                {
                    return;
                }
                if (weixin == "")
                {
                    return;
                }

                Weixin_template m_tmpl = new Weixin_templateData().GetWeixinTmpl(comid, infotype);
                if (m_tmpl == null)
                {
                    return;
                }
                if (m_tmpl.Template_id == "")
                {
                    return;
                }



                string json_param = "{" +
                                "\"touser\":\"{{touser.DATA}}\"," +
                                "\"template_id\":\"{{template_id.DATA}}\"," +
                                "\"url\":\"{{url.DATA}}\"," +
                                "\"topcolor\":\"#FF0000\"," +
                                "\"data\":{" +
                                "\"first\": {" +
                                "\"value\":\"{{first.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                  "\"keyword1\": {" +
                                "\"value\":\"{{keyword1.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"keyword2\":{" +
                                "\"value\":\"{{keyword2.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"keyword3\":{" +
                                "\"value\":\"{{keyword3.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"keyword4\":{" +
                                "\"value\":\"{{keyword4.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"keyword5\":{" +
                                "\"value\":\"{{keyword5.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"remark\":{" +
                                "\"value\":\"{{remark.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}" +
                                "}" +
                                "}";


                Weixin_templatemsg_sendlog log = new Weixin_templatemsg_sendlog
                {
                    Id = 0,
                    Msg_send_content = json_param,
                    Touser = "",
                    Template_id = "",
                    Url = "",
                    Msg_send_createtime = DateTime.Now,
                    Msg_call_content = "",
                    Msg_call_errcode = "",
                    Msg_call_errmsg = "",
                    Msgid = "",
                    Msg_push_content = "",
                    Msg_push_CreateTime = "",
                    Msg_push_CreateTime_format = DateTime.Parse("1970-01-01"),
                    Msg_push_status = "",
                    Orderid = 0,
                    Public_account = "",
                    Comid = 0,
                    Remark = "",
                    Infotype = infotype
                };
                int logid = new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                if (logid == 0)
                {
                    return;
                }
                else
                {
                    log.Id = logid;
                }
                try
                {
                    log.Comid = comid;

                    WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(comid);
                    if (basicc == null)
                    {
                        log.Remark = "公司获取失败";
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                        return;
                    }
                    WXAccessToken token = GetAccessToken(basicc.Comid, basicc.AppId, basicc.AppSecret);

                    log.Template_id = m_tmpl.Template_id;
                    log.Touser = weixin;

                    string requesturl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + token.ACCESS_TOKEN;

                    string linkurl = "";
                    log.Url = linkurl;

                    json_param = json_param.Replace("{{touser.DATA}}", weixin);
                    json_param = json_param.Replace("{{template_id.DATA}}", m_tmpl.Template_id);
                    json_param = json_param.Replace("{{url.DATA}}", linkurl);
                    json_param = json_param.Replace("{{first.DATA}}", first);
                    json_param = json_param.Replace("{{keyword1.DATA}}", keyword1);
                    json_param = json_param.Replace("{{keyword2.DATA}}", keyword2);
                    json_param = json_param.Replace("{{keyword3.DATA}}", keyword3);
                    json_param = json_param.Replace("{{keyword4.DATA}}", keyword4);
                    json_param = json_param.Replace("{{keyword5.DATA}}", keyword5);
                    json_param = json_param.Replace("{{remark.DATA}}", remark);

                    log.Msg_send_content = json_param;
                    new Weixin_templatemsg_sendlogData().EditTmplLog(log);

                    string ret = new GetUrlData().HttpPost(requesturl, json_param);
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    OuterClass foo = ser.Deserialize<OuterClass>(ret);
                    if (foo.errcode != null)
                    {
                        log.Msg_call_content = ret;
                        log.Msg_call_errcode = foo.errcode;
                        log.Msg_call_errmsg = foo.errmsg;
                        if (foo.errcode == "0")
                        {
                            log.Msgid = foo.msgid;
                        }
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                    }
                    else
                    {
                        log.Remark = "调用模板消息后返回失败";
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                    }
                }
                catch (Exception e)
                {
                    log.Remark = "意外错误";
                    new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                }
            }
        }

        /// <summary>
        ///  微信发送模板消息--订阅活动开始提醒
        /// </summary>
        /// <param name="comid">公司id</param>
        /// <param name="weixin">微信</param>
        /// <param name="first">标题</param>
        /// <param name="keyword1">活动名称</param>
        /// <param name="keyword2">开始时间</param>
        /// <param name="remark">备注</param>
        public void WxTmplMsg_SubscribeActReward(int comid, string weixin, string first, string keyword1, string keyword2, string remark)
        {
            lock (lockobject)
            {
                string infotype = ((int)WxTmplType.SubscribeActReward).ToString();//模板类型:订阅活动开始提醒 

                if (comid <= 0)
                {
                    return;
                }
                if (weixin == "")
                {
                    return;
                }

                Weixin_template m_tmpl = new Weixin_templateData().GetWeixinTmpl(comid, infotype);
                if (m_tmpl == null)
                {
                    return;
                }
                if (m_tmpl.Template_id == "")
                {
                    return;
                }



                string json_param = "{" +
                                "\"touser\":\"{{touser.DATA}}\"," +
                                "\"template_id\":\"{{template_id.DATA}}\"," +
                                "\"url\":\"{{url.DATA}}\"," +
                                "\"topcolor\":\"#FF0000\"," +
                                "\"data\":{" +
                                "\"first\": {" +
                                "\"value\":\"{{first.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                  "\"keyword1\": {" +
                                "\"value\":\"{{keyword1.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"keyword2\":{" +
                                "\"value\":\"{{keyword2.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}," +
                                "\"remark\":{" +
                                "\"value\":\"{{remark.DATA}}\"," +
                                "\"color\":\"#173177\"" +
                                "}" +
                                "}" +
                                "}";


                Weixin_templatemsg_sendlog log = new Weixin_templatemsg_sendlog
                {
                    Id = 0,
                    Msg_send_content = json_param,
                    Touser = "",
                    Template_id = "",
                    Url = "",
                    Msg_send_createtime = DateTime.Now,
                    Msg_call_content = "",
                    Msg_call_errcode = "",
                    Msg_call_errmsg = "",
                    Msgid = "",
                    Msg_push_content = "",
                    Msg_push_CreateTime = "",
                    Msg_push_CreateTime_format = DateTime.Parse("1970-01-01"),
                    Msg_push_status = "",
                    Orderid = 0,
                    Public_account = "",
                    Comid = 0,
                    Remark = "",
                    Infotype = infotype
                };
                int logid = new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                if (logid == 0)
                {
                    return;
                }
                else
                {
                    log.Id = logid;
                }
                try
                {
                    log.Comid = comid;

                    WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(comid);
                    if (basicc == null)
                    {
                        log.Remark = "公司获取失败";
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                        return;
                    }
                    WXAccessToken token = GetAccessToken(basicc.Comid, basicc.AppId, basicc.AppSecret);

                    log.Template_id = m_tmpl.Template_id;
                    log.Touser = weixin;

                    string requesturl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + token.ACCESS_TOKEN;

                    string linkurl = "";
                    log.Url = linkurl;

                    json_param = json_param.Replace("{{touser.DATA}}", weixin);
                    json_param = json_param.Replace("{{template_id.DATA}}", m_tmpl.Template_id);
                    json_param = json_param.Replace("{{url.DATA}}", linkurl);
                    json_param = json_param.Replace("{{first.DATA}}", first);
                    json_param = json_param.Replace("{{keyword1.DATA}}", keyword1);
                    json_param = json_param.Replace("{{keyword2.DATA}}", keyword2);
                    json_param = json_param.Replace("{{remark.DATA}}", remark);

                    log.Msg_send_content = json_param;
                    new Weixin_templatemsg_sendlogData().EditTmplLog(log);

                    string ret = new GetUrlData().HttpPost(requesturl, json_param);
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    OuterClass foo = ser.Deserialize<OuterClass>(ret);
                    if (foo.errcode != null)
                    {
                        log.Msg_call_content = ret;
                        log.Msg_call_errcode = foo.errcode;
                        log.Msg_call_errmsg = foo.errmsg;
                        if (foo.errcode == "0")
                        {
                            log.Msgid = foo.msgid;
                        }
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                    }
                    else
                    {
                        log.Remark = "调用模板消息后返回失败";
                        new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                    }
                }
                catch (Exception e)
                {
                    log.Remark = "意外错误";
                    new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                }
            }
        }
 


        /// <summary>
        /// 获取凭证（首先判断数据库是否存在凭证以及是否过期，如过期，重新获取）
        /// </summary>
        /// <returns></returns>
        public WXAccessToken GetAccessToken(int comid, string AppId, string AppSecret)
        {
            DateTime fitcreatetime = DateTime.Now.AddHours(-2);
            WXAccessToken token = new WXAccessTokenData().GetLaststWXAccessToken(fitcreatetime, comid);
            if (token == null)
            {
                string geturl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppId + "&secret=" + AppSecret;
                string jsonText = new GetUrlData().HttpGet(geturl);

                try
                {
                    XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + jsonText + "}");

                    XmlElement rootElement = doc.DocumentElement;
                    string access_token = rootElement.SelectSingleNode("access_token").InnerText;

                    //把获取到的凭证录入数据库中
                    token = new WXAccessToken()
                    {
                        Id = 0,
                        ACCESS_TOKEN = access_token,
                        CreateDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        Comid = comid

                    };
                    int edittoken = new WXAccessTokenData().EditAccessToken(token);
                }
                catch 
                {
                    return null;
                }
            }
            return token;
        }
    }
    public class OuterClass
    {
        public string errcode;
        public string errmsg;
        public string msgid;
    }
}
