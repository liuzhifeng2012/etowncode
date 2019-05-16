using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.Common.Business;
using ETS2.VAS.Service.VASService.Data.Common;
using System.Xml;
using ETS.Data.SqlHelper;
using System.Runtime.Remoting.Messaging;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.WeiXin.Service.WinXinService.BLL;
using System.Text.RegularExpressions;
using ETS2.VAS.Service.VASService.Data.InternalData;
using Newtonsoft.Json;
using ETS2.Member.Service.MemberService.Data;
using ETS.Framework;
using ETS2.PM.Service.WL.Data;
using ETS2.PM.Service.WL.Model;


namespace ETS2.VAS.Service.VASService.Data
{
    public class SendEticketData
    {
        private static object lockobj = new object();

        delegate void AsyncsendsmsEventHandler(string phone, string smscontent, int comid, int orderid, string pno, int insertsendEticketid, int pro_sourcetype);//发送电子票委托

        #region 支付异步返回后，完成订单并发送电子码 注：淘宝码商修改手机通知，需要向新的手机号发送短信，而不应该读取短信发送记录里的手机
        public string SendEticket(int order_no, int sentype, string mobile = "", int aorderid = 0)
        {
            //发码或重发码

            //根据订单id得到订单信息
            B2bOrderData dataorder = new B2bOrderData();
            B2b_order modelb2border = dataorder.GetOrderById(order_no);

            //根据产品id得到产品信息
            B2bComProData datapro = new B2bComProData();

            if (modelb2border == null)
            {

                return "订单不存在";
            }
            B2b_com_pro modelcompro = datapro.GetProById(modelb2border.Pro_id.ToString(), modelb2border.Speciid, modelb2border.channelcoachid);
            if (modelcompro == null)
            {
                return "订单提交产品不存在";
            }
            #region 新发送电子票
            if (sentype == 1)//新发送电子码
            {
                //已付款
                if ((int)modelb2border.Order_state == (int)OrderStatus.HasPay)
                {
                    #region 系统自动生成
                    if (modelcompro.Source_type == 1)
                    {
                        //旅游大巴产品 增加乘车人信息
                        if (modelcompro.Server_type == 10)
                        {
                            #region  录入订单子表(订单乘车人信息表)
                            if (modelb2border.travelnames != "")
                            {
                                for (int i = 1; i <= modelb2border.U_num; i++)
                                {
                                    string travelname = modelb2border.travelnames.Split(',')[i - 1];
                                    string travelidcard = modelb2border.travelidcards.Split(',')[i - 1];
                                    string travelnation = modelb2border.travelnations.Split(',')[i - 1];
                                    string travelphone = modelb2border.travelphones.Split(',')[i - 1];
                                     
                                    string travelremark = "";

                                    if (modelb2border.travelremarks !="")
                                    {
                                        travelremark= modelb2border.travelremarks.Split(',')[i - 1];
                                    }
                                    string travel_pickuppoint = modelb2border.pickuppoint;
                                    string travel_dropoffpoint = modelb2border.dropoffpoint;


                                    int rt = new B2bOrderData().Insertb2b_order_busNamelist(modelb2border.Id, travelname, travelidcard, travelnation, modelb2border.U_name, modelb2border.U_phone, DateTime.Now, modelb2border.U_num.ToString(), modelb2border.U_traveldate.ToString(), modelb2border.Comid, modelb2border.Agentid, modelb2border.Pro_id, travel_pickuppoint, travel_dropoffpoint, travelphone, travelremark);
                                }
                            }
                            #endregion

                        }
                        return SendXitongSms(modelcompro, modelb2border, order_no, aorderid);
                    }
                    #endregion
                    #region 外来接口产品
                    else if (modelcompro.Source_type == 3)
                    {
                        return SendJiekouSms(modelcompro, modelb2border, order_no);
                    }
                    #endregion
                    #region 倒码的则读取电子码
                    else if (modelcompro.Source_type == 2)
                    {
                        return SendDaomaSms(modelcompro, modelb2border, order_no);
                    }
                    #endregion
                    #region 本系统商户导入产品
                    else if (modelcompro.Source_type == 4)
                    {
                        var agentorderstate = 0;
                        //因为是导入产品，已经提交分销订单，并在分销订单下完成了，这里不需要处理发送 并且发送电子码了
                        var agentorder = dataorder.GetOrderById(modelb2border.Bindingagentorderid);
                        if (agentorder != null)
                        {
                            agentorderstate = agentorder.Order_state;

                            //----------------- 按绑定的分销订单修改状态-------------------------//
                            modelb2border.Send_state = agentorder.Send_state;
                            modelb2border.Order_state = agentorder.Order_state;
                            modelb2border.Ticketcode = agentorder.Ticketcode;
                            //修改订单中发码状态为“已发码”，订单状态为"已发码"，电子码id输入
                            modelb2border.Backtickettime = DateTime.Now;
                            dataorder.InsertOrUpdate(modelb2border);
                        }

                        return "OK";
                    }
                    #endregion
                    else
                    {
                        return "产品类型出错";
                    }
                }
                else
                {
                    return "付款状态错误，或已经发码请刷新页面！";
                }
            }
            #endregion
            #region 重发电子票
            else
            { //重发
                //如果是系统导入产品，找到原始分销订单产品,分销订单产品，并初始化订单号
                if (modelcompro.Source_type == 4)
                {
                    order_no = modelb2border.Bindingagentorderid;
                    modelb2border = dataorder.GetOrderById(modelb2border.Bindingagentorderid);
                    modelcompro = datapro.GetProById(modelb2border.Pro_id.ToString());

                }

                if (modelcompro.Source_type == 3)//外来接口产品 
                {
                    #region 阳光绿洲
                    if (modelcompro.Serviceid == 1)//阳光绿洲
                    {
                        ApiService mapiservice = new ApiServiceData().GetApiservice(modelcompro.Serviceid);
                        if (mapiservice == null)
                        {
                            return "fail 产品服务商信息查询失败";
                        }
                        Api_yg_addorder_output moutput = new Api_yg_addorder_outputData().Getapi_yg_addorder_output(modelb2border.Id);
                        if (moutput == null)
                        {
                            return "fail 阳光订单信息查询失败";
                        }
                        

                        string service_ordernum = modelb2border.Service_order_num;
                        string sendresult = new SunShineInter().repeat_order(mapiservice, moutput);

                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(sendresult);
                        XmlElement root = doc.DocumentElement;

                        string req_seq = root.SelectSingleNode("req_seq").InnerText;//请求流水号
                        string id = root.SelectSingleNode("result/id").InnerText;//结果id 
                        string comment = root.SelectSingleNode("result/comment").InnerText;// 结果描述  


                        //-----------------新增2 begin-------------------------//
                        if (id == "0000")//发码成功
                        {
                            return "OK";
                        }
                        else//发码出错
                        {
                            //修改订单中发码状态为“未发码”，订单状态为"发码出错" 
                            modelb2border.Send_state = (int)SendCodeStatus.NotSend;
                            modelb2border.Order_state = (int)OrderStatus.RepeatSendCodeErr;
                            modelb2border.Ticketcode = 0;


                            modelb2border.Service_req_seq = req_seq;
                            modelb2border.Order_remark = comment;

                            dataorder.InsertOrUpdate(modelb2border);

                            return comment;
                        }
                    }
                    #endregion
                    #region  万龙接口自行发送短信
                    else if (modelcompro.Serviceid == 4) 
                    {//wl短信重发
                        //记录短信日志表
                        B2bSmsMobileSendDate smsmobilelog = new B2bSmsMobileSendDate();
                        B2b_smsmobilesend smsinfo = smsmobilelog.Searchsmslog(order_no);

                        string msg = "";
                        if (smsinfo != null)
                        {
                            if (mobile != "")
                            {
                                smsinfo.Mobile = mobile;
                            }

                            int sendback = -1;
                            //判断是否是淘宝订单，并且手机号形式如:135****1212:是的话直接返回发送成功；否则进入正式的发送短信操作
                            bool is_tborder = IsTbOrder(smsinfo.Mobile);
                            if (is_tborder)
                            {
                                sendback = 1;
                            }
                            else
                            {
                                sendback = SendSmsHelper.SendSms(modelb2border.U_phone, "R" + smsinfo.Content, modelcompro.Com_id, out msg);
                            }

                            if (sendback > 0)
                            {
                                return "OK";
                            }
                            else
                            {
                                return "电子码重发失败！";
                            }
                        }
                        else
                        {
                            return "电子码重发失败！";
                        }
                    }
                        #endregion
                    #region  慧择网保险产品
                    else if (modelcompro.Serviceid == 2)//慧择网保险产品
                    {
                        return "OK";
                    }
                    #endregion
                    #region 美景联动
                    else if (modelcompro.Serviceid == 3)//美景联动
                    {
                        ApiService mapiservice = new ApiServiceData().GetApiservice(modelcompro.Serviceid);
                        if (mapiservice == null)
                        {
                            return "fail 产品服务商信息查询失败";
                        }
                        Api_Mjld_SubmitOrder_output moutput = new Api_mjld_SubmitOrder_outputData().GetApi_Mjld_SubmitOrder_output(modelb2border.Id);
                        if (moutput == null)
                        {
                            return "fail 美景联动订单信息查询失败";
                        }

                        string sendresult = new MjldInter().ReSendSms(mapiservice, moutput);


                        if (sendresult.Trim() != "")
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(sendresult);
                            XmlElement root = doc.DocumentElement;

                            string status = root.SelectSingleNode("status").InnerText;

                            //-----------------新增2 begin-------------------------//
                            if (status == "0")//发码成功
                            {
                                return "OK";
                            }
                            else//发码出错
                            {
                                dataorder.InsertOrUpdate(modelb2border);

                                return "fail 重新发码失败";
                            }
                        }

                        return "fail";
                    }
                    #endregion
                    else
                    {
                        return "OK";
                    }
                }
                else
                {
                    if (modelb2border.Agentid != 0)//分销订单
                    {
                        Agent_company magentcompany = new AgentCompanyData().GetAgentCompany(modelb2border.Agentid);
                        if (magentcompany == null)
                        {
                            return "分销公司信息不存在";
                        }
                        else
                        {
                            //if (magentcompany.Agent_messagesetting == 1) //分销自己发送短信
                            //{
                            //    //记录短信日志表
                            //    B2bSmsMobileSendDate smsmobilelog = new B2bSmsMobileSendDate();
                            //    B2b_smsmobilesend smsinfo = smsmobilelog.Searchsmslog(order_no);
                            //    if (smsinfo != null)
                            //    {
                            //        return "分销商自己发送短信，" + smsinfo.Pno;
                            //    }
                            //    else
                            //    {
                            //        return "电子码重发失败！";
                            //    }
                            //}
                            //else//易城发送短信 
                            //{
                            //记录短信日志表
                            B2bSmsMobileSendDate smsmobilelog = new B2bSmsMobileSendDate();
                            B2b_smsmobilesend smsinfo = smsmobilelog.Searchsmslog(order_no);

                            string msg = "";
                            if (smsinfo != null)
                            {
                                if (mobile != "")
                                {
                                    smsinfo.Mobile = mobile;
                                }

                                int sendback = -1;
                                //判断是否是淘宝订单，并且手机号形式如:135****1212:是的话直接返回发送成功；否则进入正式的发送短信操作
                                bool is_tborder = IsTbOrder(smsinfo.Mobile);
                                if (is_tborder)
                                {
                                    sendback = 1;
                                }
                                else
                                {
                                    sendback = SendSmsHelper.SendSms(modelb2border.U_phone, "R" + smsinfo.Content, modelcompro.Com_id, out msg);
                                }
                                //int sendback = SendSmsHelper.SendSms(smsinfo.Mobile, "R" + smsinfo.Content, modelcompro.Com_id, out msg);

                                if (sendback > 0)
                                {
                                    return "OK";
                                }
                                else
                                {
                                    return "电子码重发失败！";
                                }
                            }
                            else
                            {
                                return "电子码重发失败！";
                            }
                            //}
                        }
                    }
                    else//直销订单 
                    {
                        //记录短信日志表
                        B2bSmsMobileSendDate smsmobilelog = new B2bSmsMobileSendDate();
                        B2b_smsmobilesend smsinfo = smsmobilelog.Searchsmslog(order_no);
                        string msg = "";
                        if (smsinfo != null)
                        {
                            int sendback = SendSmsHelper.SendSms(smsinfo.Mobile, "R" + smsinfo.Content, modelcompro.Com_id, out msg);
                            if (sendback > 0)
                            {
                                return "OK";
                            }
                            else
                            {
                                return "电子码重发失败！";
                            }
                        }
                        else
                        {
                            return "电子码重发失败！";
                        }
                    }

                }
            }
            #endregion
        }
        #endregion

        #region 只生成电子码 createtype=1 产生一张 createtype=产生多张，每张电子票一人次
        public string CreateEticket(int order_no, int createtype)
        {
            //根据订单id得到订单信息
            B2bOrderData dataorder = new B2bOrderData();
            B2b_order modelb2border = dataorder.GetOrderById(order_no);

            //根据产品id得到产品信息
            B2bComProData datapro = new B2bComProData();
            B2b_com_pro modelcompro = datapro.GetProById(modelb2border.Pro_id.ToString());

            int u_num = modelb2border.U_num;
            int comid = modelcompro.Com_id;

            if (createtype == 1)//生成一个码
            {
                RandomCode modelrandomcode = GetRandomCode();

                string eticketcode = "9" + comid.ToString() + modelrandomcode.Code.ToString();

                //电子码写入订单表里
                modelb2border.Pno = eticketcode;
                modelb2border.Backtickettime = DateTime.Now;
                dataorder.InsertOrUpdate(modelb2border);


                //录入电子票列表
                B2bEticketData eticketdata = new B2bEticketData();
                B2b_eticket eticket = new B2b_eticket()
                {

                    Id = 0,
                    Com_id = comid,
                    Pro_id = modelcompro.Id,
                    Agent_id = modelb2border.Agentid,
                    Oid = order_no,
                    Pno = eticketcode,
                    E_type = (int)EticketCodeType.ShuZiMa,
                    Pnum = modelb2border.U_num,
                    Use_pnum = modelb2border.U_num,
                    E_proname = modelcompro.Pro_name,
                    E_face_price = modelcompro.Face_price,
                    E_sale_price = modelb2border.Pay_price,
                    E_cost_price = modelb2border.Cost,
                    V_state = (int)EticketCodeStatus.NotValidate,
                    Send_state = (int)EticketCodeSendStatus.NotSend,
                    Subdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                };
                int eticketid = eticketdata.InsertOrUpdate(eticket);

                return eticketcode;
            }
            else
            {//生成多个电子票。，每张个电子码一人次
                string eticketcode = "";

                int eticketid = 0;
                for (int i = 0; i < u_num; i++)
                {
                    //生成电子码，获取随机码

                    RandomCode modelrandomcode = GetRandomCode();


                    eticketcode = "9" + comid.ToString() + modelrandomcode.Code.ToString();

                    ////电子码写入订单表里
                    //modelb2border.Pno = eticketcode;
                    //modelb2border.Backtickettime = DateTime.Now;
                    //dataorder.InsertOrUpdate(modelb2border);

                    //录入电子票列表
                    B2bEticketData eticketdata = new B2bEticketData();
                    B2b_eticket eticket = new B2b_eticket()
                    {
                        Id = 0,
                        Com_id = comid,
                        Pro_id = modelcompro.Id,
                        Agent_id = modelb2border.Agentid,
                        Oid = order_no,
                        Pno = eticketcode,
                        E_type = (int)EticketCodeType.ShuZiMa,
                        Pnum = 1,
                        Use_pnum = 1,
                        E_proname = modelcompro.Pro_name,
                        E_face_price = modelcompro.Face_price,
                        E_sale_price = modelb2border.Pay_price,
                        E_cost_price = modelb2border.Cost,
                        V_state = (int)EticketCodeStatus.NotValidate,
                        Send_state = (int)EticketCodeSendStatus.NotSend,
                        Subdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    };
                    eticketid = eticketdata.InsertOrUpdate(eticket);

                    //对电子票赋值，纸质票规则码
                    if (eticketid != 0)
                    {

                        string pagecode = "";//前4位商户号+8位订单号+4位数量号
                        if (comid < 1000)
                        {
                            pagecode = comid.ToString() + "0";
                        }
                        else
                        {
                            pagecode = comid.ToString();
                        }

                        //后面8位是 订单号，订单号不足 10000000
                        pagecode += (10000000 + order_no).ToString();

                        //最后4位是 第几张
                        var i_temp = i + 1;
                        if (i_temp >= 1000)
                        {
                            pagecode += i_temp.ToString();
                        }
                        else if (i_temp >= 100)
                        {
                            pagecode += "0" + i_temp.ToString();
                        }
                        else if (i_temp >= 10)
                        {
                            pagecode += "00" + i_temp.ToString();
                        }
                        else
                        {
                            pagecode += "000" + i_temp.ToString();
                        }

                        var uppagecod = eticketdata.PrintStateUpPagecode(eticketid, pagecode);

                    }

                }
                return eticketid.ToString();
            }

        }
        #endregion


        #region 获取订单电子码短信
        public string HuoQuEticket(int order_no)
        {
            //查询订单
            B2bOrderData orderdata = new B2bOrderData();
            var orderinfo = orderdata.GetOrderById(order_no);
            if (orderinfo != null)
            {
                //判断是否为导入产品的绑定订单
                if (orderinfo.Bindingagentorderid != 0)
                {
                    //记录短信日志表
                    B2bSmsMobileSendDate smsmobilelog = new B2bSmsMobileSendDate();
                    B2b_smsmobilesend smsdate = smsmobilelog.Searchsmslog(orderinfo.Bindingagentorderid);

                    if (smsdate != null)
                    {
                        return smsdate.Content.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");

                    }
                }
                else
                {//普通订单
                    //记录短信日志表
                    B2bSmsMobileSendDate smsmobilelog = new B2bSmsMobileSendDate();
                    B2b_smsmobilesend smsdate = smsmobilelog.Searchsmslog(order_no);

                    if (smsdate != null)
                    {
                        return smsdate.Content.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");

                    }
                }
            }
            return "";
        }
        #endregion

        #region 获取订单电子码短信
        public string HuoQuEticketPno(int order_no)
        {
            //查询订单
            B2bOrderData orderdata = new B2bOrderData();
            var orderinfo = orderdata.GetOrderById(order_no);
            if (orderinfo != null)
            {
                //判断是否为导入产品的绑定订单
                if (orderinfo.Bindingagentorderid != 0)
                {
                    //读取原始订单电子码
                    var orderinfo_old = orderdata.GetOrderById(orderinfo.Bindingagentorderid);
                    if (orderinfo_old != null)
                    {
                        return orderinfo_old.Pno;
                    }
                }
                else
                {//普通订单
                    return orderinfo.Pno;
                }
            }
            return "";
        }
        #endregion



         #region 发送wl电子码，以后其他商家也可以用，先都叫万龙方式，就是提取别人的码，自己发送
        public string WLSendSms(B2b_com_pro modelcompro, B2b_order modelb2border, int order_no)
        {
            B2bOrderData dataorder = new B2bOrderData();



            string eticketcode = "";
            string eticketcodeimg = "";

            B2b_company commanage = B2bCompanyData.GetAllComMsg(modelb2border.Comid);
            WlGetProInfoDealRequestData wldata = new WlGetProInfoDealRequestData(commanage.B2bcompanyinfo.wl_PartnerId, commanage.B2bcompanyinfo.wl_userkey);

            var Wlorderinfo = wldata.SearchWlOrderData(modelb2border.Comid, 0, "", modelb2border.Id);
            if (Wlorderinfo == null)
            {
                return "wl订单出错";
            }
            else {
                eticketcode = Wlorderinfo.vouchers;
                eticketcodeimg = Wlorderinfo.voucherPics;
            
            }
            //得到短信内容
            string sendstr = modelcompro.Sms;
            if (sendstr == "")
            {
                sendstr = "感谢您订购" + modelcompro.Pro_name + modelb2border.U_num + "张" + ",电子码:" + eticketcode + " 有效期:" + modelcompro.Pro_start.ToString("yyyy-MM-dd") + "-" + modelcompro.Pro_end.ToString("yyyy-MM-dd") + "";
            }
            else
            {
                //$票号$ $姓名$ $数量$ $有效期$ $产品名称$ 进行替换
                sendstr = sendstr.Replace("$票号$", eticketcode);
                
                sendstr = sendstr.Replace("$数量$", modelb2border.U_num.ToString());
                if (modelcompro.Server_type == 10)
                {
                    sendstr = sendstr.Replace("$有效期$", modelb2border.U_traveldate.ToString("yyyy.MM.dd"));
                    sendstr = sendstr.Replace("$上车地点$", modelb2border.pickuppoint);
                }
                else if (modelcompro.Server_type == 12 || modelcompro.Server_type == 13)
                {
                    sendstr = sendstr.Replace("$有效期$", modelb2border.U_traveldate.ToString("yyyy.MM.dd hh:mm:ss"));
                }
                else
                {
                    sendstr = sendstr.Replace("$有效期$", modelcompro.Pro_start.ToString("yyyy-MM-dd") + "-" + modelcompro.Pro_end.ToString("yyyy-MM-dd"));
                }



                sendstr = sendstr.Replace("$使用日期$", modelb2border.U_traveldate.ToString("yyyy.MM.dd hh:mm:ss"));
                sendstr = sendstr.Replace("$预订电话$", modelb2border.U_phone);

                sendstr = sendstr.Replace("$产品名称$", modelcompro.Pro_name);
                sendstr = sendstr.Replace("$姓名$", modelb2border.U_name);
                sendstr = sendstr.Replace("$评价链接$", "http://shop" + modelb2border.Comid + ".etown.cn/h5/subevaluate.aspx?oid=" + modelb2border.Id);
                sendstr = sendstr.Replace("$二维码地址$", eticketcodeimg);
                //sendstr = sendstr.Replace("$二维码地址$", new GetUrlData().ToShortUrl("http://shop" + modelb2border.Comid + ".etown.cn/qrcode?pno=" + EncryptionHelper.Encrypt(eticketcode, "lixh1210")));
            }


            #region 发送操作，除了酒店客房中预订类型为2(预付成功发送短信)外，其他操作相同
            //电子票发送日志表，创建发送记录
            B2bEticketSendLogData eticketsnedlog = new B2bEticketSendLogData();
            B2b_eticket_send_log eticketlog = new B2b_eticket_send_log()
            {
                Id = 0,
                Eticket_id = 0,
                Pnotext = sendstr,
                Phone = modelb2border.U_phone,
                Sendstate = 0,
                Sendtype = 1,
                Senddate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            };
            int insertsendEticketid = eticketsnedlog.InsertOrUpdate(eticketlog);

            //修改电子票发送日志表的发码状态为"发送中"
            B2b_eticket_send_log eticketlogup = new B2b_eticket_send_log()
            {
                Id = insertsendEticketid,
                Sendstate = (int)SendCodeStatus.SendLoading,
                Senddate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            };
            #endregion
            //异步发送电子码
            AsyncsendsmsEventHandler mydelegate = new AsyncsendsmsEventHandler(AsyncSendSms);
            mydelegate.BeginInvoke(modelb2border.U_phone, sendstr, modelcompro.Com_id, order_no, eticketcode, insertsendEticketid, 3, new AsyncCallback(CompletedSendSms), null);


            return "OK";
        }
         #endregion

        #region 发送系统自动生成电子码
        public string SendXitongSms(B2b_com_pro modelcompro, B2b_order modelb2border, int order_no, int aorderid = 0)
        {
            B2bOrderData dataorder = new B2bOrderData();

            int eticketid = 0;//电子码 ID号

            //生成电子码
            int u_num = modelb2border.U_num;
            int comid = modelcompro.Com_id;
            int yanzheng_method = modelb2border.yanzheng_method;
            int pro_yanzheng_method = modelcompro.pro_yanzheng_method;
            //由于产品添加了绑定服务，所以增加了产品核销方式(pro_yanzheng_method):现在接口核销方式 和 产品核销方式只要有一个是一码一验，则生成多个码
            if (yanzheng_method == 1 || pro_yanzheng_method == 1)
            {
                yanzheng_method = 1;
            }

            string eticketcode = "";
            string sendstr = "";

            if (yanzheng_method == 0)
            {


                RandomCode modelrandomcode = GetRandomCode();

                eticketcode = "9" + comid.ToString() + modelrandomcode.Code.ToString();
                if (comid == 2607) { //换成8位码
                    eticketcode =modelrandomcode.Code.ToString();
                }



                sendstr = "";

                #region 如果是未发送的生成插入,如果是已生成过的直接读取电子码 1=未发送，2已发送，3发送中
                if (modelb2border.Send_state == 1)
                {
                    //电子码写入订单表里
                    modelb2border.Pno = eticketcode;
                    modelb2border.Backtickettime = DateTime.Now;
                    dataorder.InsertOrUpdate(modelb2border);

                    //录入电子票列表
                    B2bEticketData eticketdata = new B2bEticketData();
                    B2b_eticket eticket = new B2b_eticket()
                    {

                        Id = 0,
                        Com_id = comid,
                        Pro_id = modelcompro.Id,
                        Agent_id = modelb2border.Agentid,//直销
                        Oid = order_no,
                        Pno = eticketcode,
                        E_type = (int)EticketCodeType.ShuZiMa,
                        Pnum = modelb2border.U_num * modelcompro.pnonumperticket,
                        Use_pnum = modelb2border.U_num * modelcompro.pnonumperticket,
                        E_proname = modelcompro.Pro_name,
                        E_face_price = modelcompro.Face_price,
                        E_sale_price = modelb2border.Pay_price,//电子票中的价格按订单的走
                        E_cost_price = modelcompro.Agentsettle_price,
                        V_state = (int)EticketCodeStatus.NotValidate,
                        Send_state = (int)EticketCodeSendStatus.NotSend,
                        Subdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    };
                    eticketid = eticketdata.InsertOrUpdate(eticket);

                }
                else
                {
                    //查询电子票列表
                    B2bEticketData eticketdata = new B2bEticketData();
                    var eticketmodel = eticketdata.SelectOrderid(order_no);
                    if (eticketmodel != null)
                    {
                        eticketid = eticketmodel.Id;
                        eticketcode = eticketmodel.Pno;
                    }
                }
                #endregion
            }
            else
            {
                //生成多个码，每个码1次
                #region 如果是未发送的生成插入,如果是已生成过的直接读取电子码 1=未发送，2已发送，3发送中
                if (modelb2border.Send_state == 1)
                {
                    for (int i = 0; i < u_num * modelcompro.pnonumperticket; i++)
                    {

                        RandomCode modelrandomcode = GetRandomCode();//得到未用随机码对象


                        string eticketcode_temp = "9" + comid.ToString() + modelrandomcode.Code.ToString();
                        if (comid == 2607)
                        { //换成8位码
                            eticketcode_temp = modelrandomcode.Code.ToString();
                        }


                        eticketcode += eticketcode_temp + ",";//循环记录

                        //录入电子票列表
                        B2bEticketData eticketdata = new B2bEticketData();
                        B2b_eticket eticket = new B2b_eticket()
                        {
                            Id = 0,
                            Com_id = comid,
                            Pro_id = modelcompro.Id,
                            Agent_id = modelb2border.Agentid,
                            Oid = order_no,
                            Pno = eticketcode_temp,
                            E_type = (int)EticketCodeType.ShuZiMa,
                            Pnum = 1,
                            Use_pnum = 1,
                            E_proname = modelcompro.Pro_name,
                            E_face_price = modelcompro.Face_price,
                            E_sale_price = modelb2border.Pay_price,
                            E_cost_price = modelb2border.Cost,
                            V_state = (int)EticketCodeStatus.NotValidate,
                            Send_state = (int)EticketCodeSendStatus.NotSend,
                            Subdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                        };
                        eticketid = eticketdata.InsertOrUpdate(eticket);
                    }

                    //对码最后一位逗号进行处理，上面字符串连接的肯定有 “，”所以直接去除
                    eticketcode = eticketcode.Substring(0, eticketcode.Length - 1);

                    //电子码写入订单表里
                    modelb2border.Pno = eticketcode;
                    modelb2border.Backtickettime = DateTime.Now;
                    dataorder.InsertOrUpdate(modelb2border);

                }
                else
                {
                    //查询电子票列表
                    B2bEticketData eticketdata = new B2bEticketData();
                    var eticketmodel = eticketdata.SelectOrderid_list(order_no);
                    if (eticketmodel != null)
                    {
                        for (int i = 0; i < eticketmodel.Count; i++)
                        {

                            eticketcode += eticketmodel[i].Pno + ",";
                            eticketid = eticketmodel[i].Id;
                        }

                    }
                }
                #endregion
            }

            if (eticketid > 0)
            {
                //当获取电子码后，先更改发送中状态
                modelb2border.Send_state = (int)SendCodeStatus.SendLoading;
                modelb2border.Backtickettime = DateTime.Now;
                //修改订单中发码状态为“发送中”
                dataorder.InsertOrUpdate(modelb2border);


                //对押金服务表，只针对 自己产生电子票的。
                var inserver = InsertEticetServerDepositbyorder(modelb2border, eticketcode);


                #region 得到产品服务类型:如果为9(酒店客房)，则查询产品扩展表-酒店产品表 中的酒店预订类型reservetype(1.不用支付，直接发送短信；2预付，发送预订短信；3预付，发送预订二维码)
                if (modelcompro.Server_type == 9)
                {
                    B2b_com_housetype modelhousetype = new B2b_com_housetypeData().GetHouseType(modelcompro.Id, modelcompro.Com_id);
                    if (modelhousetype != null)
                    {
                        //获取酒店订单
                        B2b_order_hotel modelhotelorder = new B2b_order_hotelData().GetHotelOrderByOrderId(modelb2border.Id);
                        if (modelhotelorder == null)
                        {
                            return "获取酒店订单失败";
                        }
                        //获取酒店所在项目的名称
                        string projectname = new B2b_com_projectData().GetProjectNameByid(modelcompro.Projectid);

                        if (modelhousetype.ReserveType == 2)
                        {
                            //得到预付成功后发送的短信内容
                            //string smskey = "酒店订房预付发送短信";
                            //sendstr = new SendSmsHelper(new SqlHelper()).GetSmsContent(smskey);
                            sendstr = modelcompro.Sms;


                            if (sendstr == "")
                            {
                                sendstr = "你的订房提交成功，" + modelcompro.Pro_name + " 数量" + modelb2border.U_num.ToString() + "入住日期" + modelhotelorder.Start_date.ToString("yyyy-MM-dd") + " 离店日期" + modelhotelorder.End_date.ToString("yyyy-MM-dd") + ",请与6点前入住，如到晚到请提前电话确认。";
                            }
                            else
                            {
                                //$title$,$num,$starttime$,$endtime$,$num1$ 进行替换
                                sendstr = sendstr.Replace("$产品名称$", projectname + modelcompro.Pro_name);
                                sendstr = sendstr.Replace("$数量$", modelb2border.U_num.ToString());
                                sendstr = sendstr.Replace("$入住日期$", modelhotelorder.Start_date.ToString("yyyy-MM-dd"));
                                sendstr = sendstr.Replace("$离店日期$", modelhotelorder.End_date.ToString("yyyy-MM-dd"));
                                sendstr = sendstr.Replace("$几天$", modelhotelorder.Bookdaynum.ToString());
                                sendstr = sendstr.Replace("$姓名$", modelb2border.U_name.ToString());

                            }

                            #region 发送操作，除了酒店客房中预订类型为2(预付成功发送短信)外，其他操作相同
                            #region////电子票发送日志表，创建发送记录
                            //B2bEticketSendLogData eticketsnedlog = new B2bEticketSendLogData();
                            //B2b_eticket_send_log eticketlog = new B2b_eticket_send_log()
                            //{
                            //    Id = 0,
                            //    Eticket_id = eticketid,
                            //    Pnotext = sendstr,
                            //    Phone = modelb2border.U_phone,
                            //    Sendstate = 0,
                            //    Sendtype = 1,
                            //    Senddate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                            //};
                            //int insertsendEticketid = eticketsnedlog.InsertOrUpdate(eticketlog);
                            #endregion
                            //发送电子码
                            string msg = "";
                            int sendstate = 0;//发送状态 1=成功，2=失败 0=未发送

                            int sendback = -1;
                            //判断是否是淘宝订单，并且手机号形式如:135****1212:是的话直接返回发送成功；否则进入正式的发送短信操作
                            bool is_tborder = IsTbOrder(modelb2border.U_phone);
                            if (is_tborder)
                            {
                                sendback = 1;
                            }
                            else
                            {
                                sendback = SendSmsHelper.SendSms(modelb2border.U_phone, sendstr, comid, out msg);
                            }


                            //Smsmodel smodel = new Smsmodel()//微信酒店预订服务商通知短信
                            //{
                            //    RecerceSMSPhone = modelhousetype.RecerceSMSPhone,
                            //    Phone = modelb2border.U_phone,
                            //    Name = modelb2border.U_name,
                            //    Title = projectname + modelcompro.Pro_name,
                            //    //Money = order.Pay_price * order.U_num,
                            //    Key = "微信酒店预订服务商通知短信",
                            //    Comid = comid,
                            //    Num = modelb2border.U_num,//间数
                            //    Num1 = modelhotelorder.Bookdaynum,//天数
                            //    Starttime = modelhotelorder.Start_date,
                            //    Endtime = modelhotelorder.End_date,

                            //};
                            ////向酒店负责人发送客人的预定信息
                            //SendSmsHelper.Member_smsBal(smodel);

                            if (sendback > 0)
                            {
                                //labelmsg.InnerText = "发送成功" + sendback;
                                sendstate = 1;
                                #region ////修改电子票发送日志表的发码状态为成功
                                //B2b_eticket_send_log eticketlogup = new B2b_eticket_send_log()
                                //{
                                //    Id = insertsendEticketid,
                                //    Sendstate = 1,
                                //    Senddate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                                //};
                                //int upsendEticket = eticketsnedlog.InsertOrUpdate(eticketlogup);
                                ////-----------------新增2 begin-------------------------//
                                //modelb2border.Send_state = (int)SendCodeStatus.HasSend;
                                //modelb2border.Order_state = (int)OrderStatus.HasSendCode;
                                //modelb2border.Ticketcode = eticketid;
                                //modelb2border.Backtickettime = DateTime.Now;
                                ////修改订单中发码状态为“已发码”，订单状态为"已发码"，电子码id输入
                                //dataorder.InsertOrUpdate(modelb2border);
                                ////------------------新增2 end---------------------------//
                                #endregion
                            }
                            else
                            {
                                //labelmsg.InnerText = "发送错误" + msg;
                                sendstate = 2;
                            }

                            //记录短信日志表
                            B2bSmsMobileSendDate smsmobilelog = new B2bSmsMobileSendDate();
                            B2b_smsmobilesend smslog = new B2b_smsmobilesend()
                            {
                                Mobile = modelb2border.U_phone,
                                Content = sendstr,
                                Flag = sendstate,
                                Text = msg,
                                Delaysendtime = "",
                                Oid = order_no,
                                Pno = eticketcode,
                                Realsendtime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                                Smsid = sendback,
                                Sendeticketid = 0
                            };
                            int insertsendmobileid = smsmobilelog.InsertOrUpdate(smslog);
                            return "OK";
                            #endregion
                        }
                        else if (modelhousetype.ReserveType == 3)
                        {
                            //得到预付成功后发送的短信内容
                            string smskey = "酒店订房预付发送二维码";
                            sendstr = new SendSmsHelper(new SqlHelper()).GetSmsContent(smskey);

                            if (sendstr == "")
                            {
                                sendstr = "你的订单已提交成功，我们会尽快为你处理";
                            }
                            else
                            {
                                //$title$,$num,$starttime$,$endtime$,$num1$,$ecode$ 进行替换
                                sendstr = sendstr.Replace("$title$", projectname + modelcompro.Pro_name);
                                sendstr = sendstr.Replace("$num$", modelb2border.U_num.ToString());
                                sendstr = sendstr.Replace("$starttime$", modelhotelorder.Start_date.ToString("yyyy-MM-dd"));
                                sendstr = sendstr.Replace("$endtime$", modelhotelorder.End_date.ToString("yyyy-MM-dd"));
                                sendstr = sendstr.Replace("$num1$", modelhotelorder.Bookdaynum.ToString());
                                sendstr = sendstr.Replace("$ecode$", eticketcode);
                            }

                            #region 发送操作，除了酒店客房中预订类型为2(预付成功发送短信)外，其他操作相同
                            //电子票发送日志表，创建发送记录
                            B2bEticketSendLogData eticketsnedlog = new B2bEticketSendLogData();
                            B2b_eticket_send_log eticketlog = new B2b_eticket_send_log()
                            {
                                Id = 0,
                                Eticket_id = eticketid,
                                Pnotext = sendstr,
                                Phone = modelb2border.U_phone,
                                Sendstate = 0,
                                Sendtype = 1,
                                Senddate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                            };
                            int insertsendEticketid = eticketsnedlog.InsertOrUpdate(eticketlog);


                            //发送电子码
                            string msg = "";
                            int sendstate = 0;//发送状态 1=成功，2=失败 0=未发送

                            int sendback = -1;
                            //判断是否是淘宝订单，并且手机号形式如:135****1212:是的话直接返回发送成功；否则进入正式的发送短信操作
                            bool is_tborder = IsTbOrder(modelb2border.U_phone);
                            if (is_tborder)
                            {
                                sendback = 1;
                            }
                            else
                            {
                                sendback = SendSmsHelper.SendSms(modelb2border.U_phone, sendstr, comid, out msg);
                            }


                            //Smsmodel smodel = new Smsmodel()//微信酒店预订服务商通知短信
                            //{
                            //    RecerceSMSPhone = modelhousetype.RecerceSMSPhone,
                            //    Phone = modelb2border.U_phone,
                            //    Name = modelb2border.U_name,
                            //    Title = projectname + modelcompro.Pro_name,
                            //    //Money = order.Pay_price * order.U_num,
                            //    Key = "微信酒店预订服务商通知短信",
                            //    Comid = comid,
                            //    Num = modelb2border.U_num,//间数
                            //    Num1 = modelhotelorder.Bookdaynum,//天数
                            //    Starttime = modelhotelorder.Start_date,
                            //    Endtime = modelhotelorder.End_date,

                            //};
                            ////向酒店负责人发送客人的预定信息
                            //SendSmsHelper.Member_smsBal(smodel);

                            if (sendback > 0)
                            {
                                //labelmsg.InnerText = "发送成功" + sendback;
                                sendstate = 1;
                                //修改电子票发送日志表的发码状态为成功
                                B2b_eticket_send_log eticketlogup = new B2b_eticket_send_log()
                                {
                                    Id = insertsendEticketid,
                                    Sendstate = 1,
                                    Senddate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                                };
                                int upsendEticket = eticketsnedlog.InsertOrUpdate(eticketlogup);
                                //-----------------新增2 begin-------------------------//
                                modelb2border.Send_state = (int)SendCodeStatus.HasSend;
                                modelb2border.Order_state = (int)OrderStatus.HasSendCode;
                                modelb2border.Ticketcode = eticketid;
                                modelb2border.Backtickettime = DateTime.Now;
                                //修改订单中发码状态为“已发码”，订单状态为"已发码"，电子码id输入
                                dataorder.InsertOrUpdate(modelb2border);
                                //------------------新增2 end---------------------------//
                            }
                            else
                            {
                                //labelmsg.InnerText = "发送错误" + msg;

                                sendstate = 2;
                            }

                            //记录短信日志表
                            B2bSmsMobileSendDate smsmobilelog = new B2bSmsMobileSendDate();
                            B2b_smsmobilesend smslog = new B2b_smsmobilesend()
                            {
                                Mobile = modelb2border.U_phone,
                                Content = sendstr,
                                Flag = sendstate,
                                Text = msg,
                                Delaysendtime = "",
                                Oid = order_no,
                                Pno = eticketcode,
                                Realsendtime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                                Smsid = sendback,
                                Sendeticketid = insertsendEticketid
                            };
                            int insertsendmobileid = smsmobilelog.InsertOrUpdate(smslog);
                            return "OK";
                            #endregion
                        }
                        else
                        {
                            //前台现付
                            return "OK";
                        }
                    }
                    else
                    {
                        return "获取房型类型失败";
                    }
                }
                #endregion
                else
                {
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
                    if (iscanuseonsameday == 1 || iscanuseonsameday == 2)//当天可用  
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

                    //得到短信
                    sendstr = modelcompro.Sms;
                    if (sendstr == "")
                    {
                        sendstr = "感谢您订购" + modelcompro.Pro_name + modelb2border.U_num + "张" + ",电子码:" + eticketcode + " 有效期:" + pro_start.ToString("yyyy/MM/dd") + "-" + pro_end.ToString("yyyy/MM/dd") + "";
                    }
                    else
                    {
                        //$票号$ $姓名$ $数量$ $有效期$ $产品名称$ 进行替换
                        sendstr = sendstr.Replace("$票号$", eticketcode);
                        sendstr = sendstr.Replace("$数量$", modelb2border.U_num.ToString());
                        if (modelcompro.Server_type == 10)
                        {
                            sendstr = sendstr.Replace("$有效期$", modelb2border.U_traveldate.ToString("yyyy.MM.dd"));

                            sendstr = sendstr.Replace("$上车地点$", modelb2border.pickuppoint);
                        }
                        else if (modelcompro.Server_type == 12 || modelcompro.Server_type == 13)
                        {
                            sendstr = sendstr.Replace("$有效期$", modelb2border.U_traveldate.ToString("yyyy.MM.dd hh:mm:ss"));
                        }
                        else
                        {
                            sendstr = sendstr.Replace("$有效期$", pro_start.ToString("yyyy.MM.dd") + "-" + pro_end.ToString("yyyy.MM.dd"));
                        }



                        sendstr = sendstr.Replace("$使用日期$", modelb2border.U_traveldate.ToString("yyyy.MM.dd hh:mm:ss"));
                        sendstr = sendstr.Replace("$预订电话$", modelb2border.U_phone);

                        sendstr = sendstr.Replace("$产品名称$", modelcompro.Pro_name);
                        sendstr = sendstr.Replace("$姓名$", modelb2border.U_name);
                        sendstr = sendstr.Replace("$评价链接$", "http://shop" + modelb2border.Comid + ".etown.cn/h5/subevaluate.aspx?oid=" + modelb2border.Id);
                        sendstr = sendstr.Replace("$二维码地址$", new GetUrlData().ToShortUrl("http://shop" + modelb2border.Comid + ".etown.cn/qrcode?pno=" + EncryptionHelper.Encrypt(eticketcode, "lixh1210")));
                    }

                    #region 发送操作，除了酒店客房中预订类型为2(预付成功发送短信)外，其他操作相同
                    //电子票发送日志表，创建发送记录
                    B2bEticketSendLogData eticketsnedlog = new B2bEticketSendLogData();
                    B2b_eticket_send_log eticketlog = new B2b_eticket_send_log()
                    {
                        Id = 0,
                        Eticket_id = eticketid,
                        Pnotext = sendstr,
                        Phone = modelb2border.U_phone,
                        Sendstate = 0,
                        Sendtype = 1,
                        Senddate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    };
                    int insertsendEticketid = eticketsnedlog.InsertOrUpdate(eticketlog);

                    //只针对 教练产品发送 
                    if (modelcompro.Server_type == 13)
                    {
                        //得到短信
                        string sendstr_wx = modelcompro.Sms;
                        if (sendstr_wx == "")
                        {
                            sendstr = "感谢您订购" + modelcompro.Pro_name + modelb2border.U_num + "张" + ",电子码:" + eticketcode + " 有效期:" + pro_start.ToString("yyyy/MM/dd") + "-" + pro_end.ToString("yyyy/MM/dd") + "";
                        }
                        else
                        {
                            //$票号$ $姓名$ $数量$ $有效期$ $产品名称$ 进行替换
                            sendstr_wx = sendstr_wx.Replace("$票号$", eticketcode);
                            sendstr_wx = sendstr_wx.Replace("$数量$", modelb2border.U_num.ToString());
                            if (modelcompro.Server_type == 10)
                            {
                                sendstr_wx = sendstr_wx.Replace("$有效期$", modelb2border.U_traveldate.ToString("yyyy.MM.dd"));

                                sendstr_wx = sendstr_wx.Replace("$上车地点$", modelb2border.pickuppoint);
                            }
                            else if (modelcompro.Server_type == 12 || modelcompro.Server_type == 13)
                            {
                                sendstr_wx = sendstr_wx.Replace("$有效期$", "\n\n" + modelb2border.U_traveldate.ToString("yyyy.MM.dd hh:mm:ss"));
                            }
                            else
                            {
                                sendstr_wx = sendstr_wx.Replace("$有效期$", "\n\n" + pro_start.ToString("yyyy.MM.dd") + "-" + pro_end.ToString("yyyy.MM.dd"));
                            }
                            sendstr_wx = sendstr_wx.Replace("$产品名称$", "\n\n" + modelcompro.Pro_name);
                            sendstr_wx = sendstr_wx.Replace("$姓名$", modelb2border.U_name);
                            sendstr_wx = sendstr_wx.Replace("$评价链接$", "\n\nhttp://shop" + modelb2border.Comid + ".etown.cn/h5/subevaluate.aspx?oid=" + modelb2border.Id);
                        }



                        //直接微信通道发送
                        CustomerMsg_Send.SendWxkefumsg(modelb2border.Id, 2, sendstr_wx, modelb2border.Comid);//给绑定顾问发送
                    }
                    //修改电子票发送日志表的发码状态为"发送中"
                    B2b_eticket_send_log eticketlogup = new B2b_eticket_send_log()
                    {
                        Id = insertsendEticketid,
                        Sendstate = (int)SendCodeStatus.SendLoading,
                        Senddate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    };

                    int upsendEticket = eticketsnedlog.InsertOrUpdate(eticketlogup);
                    //-----------------新增2 begin-------------------------//

                    if (modelcompro.Server_type == 11 && modelb2border.Deliverytype != 4)//实物订单，配送
                    {
                        //对实物订单，配送的 不做处理，不对订单状态处理，只对发送状态标注为已发码
                        modelb2border.Send_state = (int)SendCodeStatus.HasSend;
                    }
                    else
                    {
                        modelb2border.Send_state = (int)SendCodeStatus.SendLoading;
                        modelb2border.Order_state = (int)OrderStatus.HasSendCode;
                    }
                    modelb2border.Ticketcode = eticketid;
                    modelb2border.Backtickettime = DateTime.Now;
                    //修改订单中发码状态为“发送中”，订单状态为"已发码"，电子码id输入
                    dataorder.InsertOrUpdate(modelb2border);
                    //------------------新增2 end---------------------------//


                    if (modelcompro.Server_type == 11 && modelb2border.Deliverytype != 4)//实物订单，配送，只产生码，不发送
                    {

                        //修改电子票发送日志表的发码状态为"发送成功"
                        B2b_eticket_send_log eticketlogup2 = new B2b_eticket_send_log()
                        {
                            Id = insertsendEticketid,
                            Sendstate = (int)SendCodeStatus.HasSend,
                            Senddate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                        };

                        int upsendEticket2 = new B2bEticketSendLogData().InsertOrUpdate(eticketlogup2);

                        //记录短信日志表
                        B2bSmsMobileSendDate smsmobilelog = new B2bSmsMobileSendDate();
                        B2b_smsmobilesend smslog = new B2b_smsmobilesend()
                        {
                            Mobile = modelb2border.U_phone,
                            Content = sendstr,
                            Flag = (int)SendCodeStatus.HasSend,
                            Text = "配送订单不发送短信",
                            Delaysendtime = "",
                            Oid = order_no,
                            Pno = eticketcode,
                            Realsendtime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                            Smsid = 99999999,//只做记录，不发送
                            Sendeticketid = insertsendEticketid
                        };
                        int insertsendmobileid = smsmobilelog.InsertOrUpdate(smslog);
                    }
                    else
                    {

                        //对优惠券产品 不发送短信 ，直接跳出发送客服通道操作
                        if (modelcompro.Server_type == 3 && modelb2border.U_phone.Trim() == "")
                        {//优惠券产品,无手机号的，不发送短信，发送客服通道

                            var openid_temp = "";

                            if (modelb2border.Openid != "")
                            {
                                openid_temp = modelb2border.Openid;
                            }

                            if (openid_temp == "")
                            {
                                var crminfo = new B2bCrmData().GetB2bCrmById(modelb2border.U_id);
                                if (crminfo != null)
                                {
                                    openid_temp = crminfo.Weixin;
                                }
                            }


                            if (openid_temp != "")
                            {
                                //根据访问获得公司信息
                                WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(modelcompro.Com_id);

                                //短信内容发送微信客服通道
                                var data = CustomerMsg_Send.SendWxMsg(comid, modelb2border.Openid, 1, "", sendstr, "", basicc.Weixinno);

                            }
                            //修改订单中发码状态为“已发码”
                            int upsendstate = new B2bOrderData().Upsendstate(order_no, (int)SendCodeStatus.HasSend);
                            return "OK";
                        }




                        int agentid = modelb2border.Agentid;
                        if (agentid != 0)
                        {
                            ////根据b订单得到a订单 
                            B2b_order m_aorder = new B2bOrderData().GetOrderById(aorderid);
                            #region 存在a订单 查询a订单中分销短信发送方式
                            if (m_aorder != null)
                            {
                                #region 分销接口提单，则根据分销短信设置方式发送短信
                                if (m_aorder.isInterfaceSub == 1)
                                {
                                    //判断a订单分销 短信发送方式
                                    Agent_company agentcompany = AgentCompanyData.GetAgentByid(m_aorder.Agentid);
                                    #region a订单是分销订单
                                    if (agentcompany != null)
                                    {
                                        if (agentcompany.Agent_messagesetting == 1)//分销商自己发送
                                        {
                                            //修改电子票发送日志表的发码状态为"发送成功"
                                            B2b_eticket_send_log eticketlogup2 = new B2b_eticket_send_log()
                                            {
                                                Id = insertsendEticketid,
                                                Sendstate = (int)SendCodeStatus.HasSend,
                                                Senddate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                                            };

                                            int upsendEticket2 = new B2bEticketSendLogData().InsertOrUpdate(eticketlogup2);
                                            //修改订单中发码状态为“已发码”
                                            int upsendstate = new B2bOrderData().Upsendstate(order_no, (int)SendCodeStatus.HasSend);

                                            //记录短信日志表
                                            B2bSmsMobileSendDate smsmobilelog = new B2bSmsMobileSendDate();
                                            B2b_smsmobilesend smslog = new B2b_smsmobilesend()
                                            {
                                                Mobile = agentcompany.Mobile,
                                                Content = sendstr,
                                                Flag = (int)SendCodeStatus.HasSend,
                                                Text = agentcompany.Company + "-分销商自己发码",
                                                Delaysendtime = "",
                                                Oid = order_no,
                                                Pno = eticketcode,
                                                Realsendtime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                                                Smsid = 99999999,//分销商自己发码
                                                Sendeticketid = insertsendEticketid
                                            };
                                            int insertsendmobileid = smsmobilelog.InsertOrUpdate(smslog);
                                        }
                                        else //易城系统发送
                                        {
                                            //异步发送电子码
                                            AsyncsendsmsEventHandler mydelegate = new AsyncsendsmsEventHandler(AsyncSendSms);
                                            mydelegate.BeginInvoke(modelb2border.U_phone, sendstr, modelcompro.Com_id, order_no, eticketcode, insertsendEticketid, modelcompro.Source_type, new AsyncCallback(CompletedSendSms), null);

                                        }
                                    }
                                    #endregion
                                    #region a 订单是直销订单
                                    else
                                    {
                                        //agentcompany = AgentCompanyData.GetAgentByid(agentid);
                                        //if (agentcompany.Agent_messagesetting == 1)//分销商自己发送
                                        //{
                                        //    //修改电子票发送日志表的发码状态为"发送成功"
                                        //    B2b_eticket_send_log eticketlogup2 = new B2b_eticket_send_log()
                                        //    {
                                        //        Id = insertsendEticketid,
                                        //        Sendstate = (int)SendCodeStatus.HasSend,
                                        //        Senddate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                                        //    };

                                        //    int upsendEticket2 = new B2bEticketSendLogData().InsertOrUpdate(eticketlogup2);
                                        //    //修改订单中发码状态为“已发码”
                                        //    int upsendstate = new B2bOrderData().Upsendstate(order_no, (int)SendCodeStatus.HasSend);

                                        //    //记录短信日志表
                                        //    B2bSmsMobileSendDate smsmobilelog = new B2bSmsMobileSendDate();
                                        //    B2b_smsmobilesend smslog = new B2b_smsmobilesend()
                                        //    {
                                        //        Mobile = agentcompany.Mobile,
                                        //        Content = sendstr,
                                        //        Flag = (int)SendCodeStatus.HasSend,
                                        //        Text = agentcompany.Company + "-分销商自己发码",
                                        //        Delaysendtime = "",
                                        //        Oid = order_no,
                                        //        Pno = eticketcode,
                                        //        Realsendtime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                                        //        Smsid = 99999999,//分销商自己发码
                                        //        Sendeticketid = insertsendEticketid
                                        //    };
                                        //    int insertsendmobileid = smsmobilelog.InsertOrUpdate(smslog);
                                        //}
                                        //else //易城系统发送
                                        //{
                                        //异步发送电子码
                                        AsyncsendsmsEventHandler mydelegate = new AsyncsendsmsEventHandler(AsyncSendSms);
                                        mydelegate.BeginInvoke(modelb2border.U_phone, sendstr, modelcompro.Com_id, order_no, eticketcode, insertsendEticketid, modelcompro.Source_type, new AsyncCallback(CompletedSendSms), null);
                                        //}
                                    }
                                    #endregion
                                }
                                #endregion
                                #region 分销后台提单，则都发送短信
                                else
                                {
                                    //异步发送电子码
                                    AsyncsendsmsEventHandler mydelegate = new AsyncsendsmsEventHandler(AsyncSendSms);
                                    mydelegate.BeginInvoke(modelb2border.U_phone, sendstr, modelcompro.Com_id, order_no, eticketcode, insertsendEticketid, modelcompro.Source_type, new AsyncCallback(CompletedSendSms), null);
                                }
                                #endregion
                            }
                            #endregion
                            #region 不存在a订单 正常执行
                            else
                            {
                                #region  分销接口提单，则根据分销短信设置方式发送短信
                                if (modelb2border.isInterfaceSub == 1)
                                {
                                    Agent_company agentcompany = AgentCompanyData.GetAgentByid(agentid);

                                    if (agentcompany.Agent_messagesetting == 1)//分销商自己发送
                                    {
                                        //修改电子票发送日志表的发码状态为"发送成功"
                                        B2b_eticket_send_log eticketlogup2 = new B2b_eticket_send_log()
                                        {
                                            Id = insertsendEticketid,
                                            Sendstate = (int)SendCodeStatus.HasSend,
                                            Senddate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                                        };

                                        int upsendEticket2 = new B2bEticketSendLogData().InsertOrUpdate(eticketlogup2);
                                        //修改订单中发码状态为“已发码”
                                        int upsendstate = new B2bOrderData().Upsendstate(order_no, (int)SendCodeStatus.HasSend);

                                        //记录短信日志表
                                        B2bSmsMobileSendDate smsmobilelog = new B2bSmsMobileSendDate();
                                        B2b_smsmobilesend smslog = new B2b_smsmobilesend()
                                        {
                                            Mobile = agentcompany.Mobile,
                                            Content = sendstr,
                                            Flag = (int)SendCodeStatus.HasSend,
                                            Text = agentcompany.Company + "-分销商自己发码",
                                            Delaysendtime = "",
                                            Oid = order_no,
                                            Pno = eticketcode,
                                            Realsendtime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                                            Smsid = 99999999,//分销商自己发码
                                            Sendeticketid = insertsendEticketid
                                        };
                                        int insertsendmobileid = smsmobilelog.InsertOrUpdate(smslog);
                                    }
                                    else //易城系统发送
                                    {
                                        //异步发送电子码
                                        AsyncsendsmsEventHandler mydelegate = new AsyncsendsmsEventHandler(AsyncSendSms);
                                        mydelegate.BeginInvoke(modelb2border.U_phone, sendstr, modelcompro.Com_id, order_no, eticketcode, insertsendEticketid, modelcompro.Source_type, new AsyncCallback(CompletedSendSms), null);

                                    }
                                }
                                #endregion
                                #region 分销后台提单，则都发送短信
                                else
                                {
                                    //异步发送电子码
                                    AsyncsendsmsEventHandler mydelegate = new AsyncsendsmsEventHandler(AsyncSendSms);
                                    mydelegate.BeginInvoke(modelb2border.U_phone, sendstr, modelcompro.Com_id, order_no, eticketcode, insertsendEticketid, modelcompro.Source_type, new AsyncCallback(CompletedSendSms), null);
                                }
                                #endregion
                            }
                            #endregion


                        }
                        else //易城系统发送
                        {
                            //异步发送电子码
                            AsyncsendsmsEventHandler mydelegate = new AsyncsendsmsEventHandler(AsyncSendSms);
                            mydelegate.BeginInvoke(modelb2border.U_phone, sendstr, modelcompro.Com_id, order_no, eticketcode, insertsendEticketid, modelcompro.Source_type, new AsyncCallback(CompletedSendSms), null);

                        }
                    }


                    //如果是教练产品，发送电子码后发送一个评价连接
                    if (modelcompro.Server_type == 13)
                    {
                        //对教练时间进行控制
                        var channeldata = new MemberChannelData();
                        var workmodel = new B2b_company_manageuser_useworktime();
                        var MasterId_temp = channeldata.GetanageuseridbymChannelid(modelb2border.channelcoachid, modelb2border.Comid);
                        var Hournum = int.Parse(modelb2border.U_traveldate.ToString("HH"));

                        var yuyueshijian = JiaolianTqBaoxinmsg(modelb2border.Speciid);//获取 预约 几小时

                        if (yuyueshijian > 0)
                        {
                            //获取到多个时间是，每个小时 没个小时一条记录
                            for (int i = Hournum; i < Hournum + yuyueshijian; i++)
                            {
                                workmodel.MasterId = MasterId_temp;
                                workmodel.oid = modelb2border.Id;
                                workmodel.useDate = DateTime.Parse(modelb2border.U_traveldate.ToString("yyyy-MM-dd"));
                                workmodel.Hournum = i;
                                workmodel.comid = modelb2border.Comid;
                                workmodel.text = "订单锁定时间";
                                var insertuseworktime = B2bCompanyManagerUserData.UseworktimeInsertOrUpdate(workmodel);
                            }

                        }
                        else
                        {
                            //如果未获取到时间，不操作
                        }


                        //短信通知 给客户
                        var querenduanxin = " 您预约教练产品：" + modelcompro.Pro_name + " 预约时间:" + modelb2border.U_traveldate.ToString("yyyy-MM-dd hh:mm") + ",请持电子码，准时到达。服务完成后您可以点击此连接进行对服务进行评价。 http://shop" + modelb2border.Comid + ".etown.cn/h5/subevaluate.aspx?oid=" + modelb2border.Id;
                        var msg = "";
                        var sendback = SendSmsHelper.SendSms(modelb2border.U_phone, querenduanxin, modelb2border.Comid, out msg);


                        //给客户发送微信客服通道通知
                        //var querenduanxin_wx = " 您预约教练产品：\n\n" + modelcompro.Pro_name + " \n\n预约时间:" + modelb2border.U_traveldate.ToString("yyyy-MM-dd hh:mm") + ",\n\n请持电子码，准时到达。\n\n服务完成后您可以点击此连接进行对服务进行评价。\n\n http://shop" + modelb2border.Comid + ".etown.cn/h5/subevaluate.aspx?oid=" + modelb2border.Id;
                        //CustomerMsg_Send.SendWxkefumsg(modelb2border.Id, 2, querenduanxin_wx, modelb2border.Comid);//给客户发送


                        //给教练通知
                        var channelinfo = MemberChannelData.GetChannelinfo(modelb2border.channelcoachid);
                        if (channelinfo != null)
                        {
                            if (channelinfo.Mobile != "")
                            {
                                //短信通知 给预约教练
                                var querenduanxin_channel = channelinfo.Name + "教练您好, 客户" + modelb2border.U_name + ":" + modelb2border.U_phone + " 预约了 " + modelcompro.Pro_name + " 时间:" + modelb2border.U_traveldate.ToString("yyyy-MM-dd hh:mm") + ",请接待，教学完毕后您点击链接为客户进行评价。 http://shop" + modelb2border.Comid + ".etown.cn/h5/subevaluate.aspx?evatype=1&oid=" + modelb2border.Id;
                                var msg_channel = "";
                                var sendback_channel = SendSmsHelper.SendSms(channelinfo.Mobile, querenduanxin_channel, modelb2border.Comid, out msg_channel);


                                var querenduanxin_channel_wx = channelinfo.Name + "教练您好, \n\n客户" + modelb2border.U_name + ":" + modelb2border.U_phone + " \n\n预约了 " + modelcompro.Pro_name + " \n\n时间:" + modelb2border.U_traveldate.ToString("yyyy-MM-dd hh:mm") + ",\n\n请接待，教学完毕后您点击链接为客户进行评价。 \n\n http://shop" + modelb2border.Comid + ".etown.cn/h5/subevaluate.aspx?evatype=1&oid=" + modelb2border.Id;
                                //给预约教练发送微信客服通道通知
                                CustomerMsg_Send.SendWxkefumsg(modelb2border.Id, 3, querenduanxin_channel_wx, modelb2border.Comid);//给预约教练发送
                            }
                        }
                    }

                    return "OK";
                    #endregion



                }

            }
            else
            {
                return "创建电子码失败";
            }

        }
        //判断手机号形式如:135****1212,则为淘宝订单，无需发送短信，只需要返回给淘宝码，淘宝负责发送
        private static bool IsTbOrder(string uphone)
        {

            string fphone = @"^1\d{2}\*\*\*\*\d{4}$";
            Regex dReg = new Regex(fphone);
            if (dReg.IsMatch(uphone))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        #endregion

        #region 发送外来接口电子票
        public string SendJiekouSms(B2b_com_pro modelcompro, B2b_order modelb2border, int order_no)
        {
            if (modelcompro.Serviceid == 1)//阳光
            {
                #region  阳光接口产品
                ApiService mapiservice = new ApiServiceData().GetApiservice(modelcompro.Serviceid);
                if (mapiservice == null)
                {
                    return "fail 产品服务商信息查询失败";
                }
                Api_yg_addorder_input minput = new Api_yg_addorder_inputData().Getapi_yg_addorder_input(modelb2border.Id);
                if (minput == null)
                {
                    return "fail 阳光提单录入信息查询失败";
                }


                string sendresult = new SunShineInter().Add_Order(mapiservice,minput);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sendresult);
                XmlElement root = doc.DocumentElement;

                string req_seq = root.SelectSingleNode("req_seq").InnerText;//请求流水号
                string id = root.SelectSingleNode("result/id").InnerText;//结果id 
                string comment = root.SelectSingleNode("result/comment").InnerText;// 结果描述  


                Api_yg_addorder_output mout = new Api_yg_addorder_output
                {
                    id = 0,
                    req_seq = req_seq,
                    resultid = id,
                    resultcomment = comment,
                    yg_ordernum = "",
                    code = "",
                    orderId = modelb2border.Id
                };
                int r = new Api_yg_addorder_outputData().EditApi_yg_addorder_output(mout);
                mout.id = r;

                if (id == "0000")//发码成功
                {
                    string order_num = root.SelectSingleNode("order/order_num").InnerText;//订单号
                    string code = root.SelectSingleNode("order/code").InnerText;

                    mout.yg_ordernum = order_num;
                    mout.code = code;
                    new Api_yg_addorder_outputData().EditApi_yg_addorder_output(mout);

                    //修改订单中发码状态为“已发码”，订单状态为"已发码" 
                    modelb2border.Send_state = (int)SendCodeStatus.HasSend;
                    modelb2border.Order_state = (int)OrderStatus.HasSendCode;
                    modelb2border.Ticketcode = 0;
                    modelb2border.Backtickettime = DateTime.Now;

                    modelb2border.Service_order_num = order_num;
                    modelb2border.Service_req_seq = req_seq;
                    modelb2border.Servicepro_v_state = "0";
                    modelb2border.service_code = code;
                    modelb2border.service_usecount = 0;
                    modelb2border.service_lastcount = modelb2border.U_num;
                    modelb2border.Pno = code;

                    new B2bOrderData().InsertOrUpdate(modelb2border);

                    return "OK";
                }
                else//发码出错
                {
                    //修改订单中发码状态为“未发码”，订单状态为"发码出错" 
                    modelb2border.Send_state = (int)SendCodeStatus.NotSend;
                    modelb2border.Order_state = (int)OrderStatus.SendCodeErr;
                    modelb2border.Ticketcode = 0;


                    modelb2border.Service_req_seq = req_seq;
                    modelb2border.Order_remark = comment;

                    new B2bOrderData().InsertOrUpdate(modelb2border);

                    return comment;
                }
                #endregion
            }
            else if (modelcompro.Serviceid == 2)  //慧择网保险 
            {
                #region 慧择网保险产品
                string applyresult = new HzinsInter().Hzins_OrderApply(modelcompro, modelb2border);
                if (applyresult != "fail")
                {
                    Hzins_OrderApplyResp mresp = (Hzins_OrderApplyResp)JsonConvert.DeserializeObject(applyresult, typeof(Hzins_OrderApplyResp));
                    if (mresp != null)
                    {
                        if (mresp.respCode == 0)//易城系统订单状态：已支付；被保人出单状态：待出单
                        {
                            modelb2border.Order_state = (int)OrderStatus.HasPay;
                            modelb2border.Order_remark = mresp.respMsg;

                            modelb2border.service_order_num = mresp.data.insureNum;
                            modelb2border.Service_req_seq = mresp.data.transNo;
                            modelb2border.Servicepro_v_state = "0";
                            modelb2border.service_code = "";
                            modelb2border.service_usecount = 0;
                            modelb2border.service_lastcount = modelb2border.U_num;


                            new B2bOrderData().InsertOrUpdate(modelb2border);


                            foreach (OrderApplyResp_orderExts mexts in mresp.data.orderExts)
                            {
                                string insureNum = mexts.insureNum;
                                List<String> insurantIds = mexts.insurantIds;
                                string insurantIdsstr = "";
                                foreach (string ids in insurantIds)
                                {
                                    if (ids != "")
                                    {
                                        insurantIdsstr += ids + ",";
                                    }
                                }
                                insurantIdsstr = insurantIdsstr.Substring(0, insurantIdsstr.Length - 1);

                                decimal priceTotal = mexts.priceTotal;
                                int insurantCount = mexts.insurantCount;


                                Api_hzins_OrderApplyResp_OrderExt m1 = new Api_hzins_OrderApplyResp_OrderExt
                                {
                                    id = 0,
                                    orderid = modelb2border.Id,
                                    insureNum = insureNum,
                                    insurantIds = insurantIdsstr,
                                    priceTotal = priceTotal,
                                    insurantCount = insurantCount
                                };
                                int ins1 = new Api_hzins_OrderApplyResp_OrderExtData().EditOrderApplyResp_OrderExt(m1);

                            }

                            if (mresp.data.orderInfos != null)//易城系统订单状态：已发码；有/没有 保单则被保人出单状态：已出单/待出单，
                            {
                                foreach (OrderApplyResp_orderInfos infos in mresp.data.orderInfos)
                                {
                                    string insureNum = infos.insureNum;
                                    string policyNum = infos.policyNum;
                                    string cName = infos.cName;
                                    string cardCode = infos.cardCode;
                                    int issueState = (int)Hzins_issueState.WaitOrder;
                                    if (policyNum != "")
                                    {
                                        issueState = (int)Hzins_issueState.HasOrder;
                                    }

                                    Api_hzins_OrderApplyResp_OrderInfo m1 = new Api_hzins_OrderApplyResp_OrderInfo
                                    {
                                        id = 0,
                                        orderid = modelb2border.Id,
                                        insureNum = insureNum,
                                        policyNum = policyNum,
                                        cName = cName,
                                        cardCode = cardCode,
                                        issueState = issueState
                                    };
                                    int ins1 = new Api_hzins_OrderApplyResp_OrderInfoData().EditOrderApplyResp_OrderInfo(m1);

                                }


                                modelb2border.Order_state = (int)OrderStatus.HasSendCode;
                                new B2bOrderData().InsertOrUpdate(modelb2border);
                            }
                            return "OK";
                        }
                        else
                        {
                            modelb2border.Order_remark = mresp.respCode.ToString() + "(" + mresp.respMsg + ")";
                            new B2bOrderData().InsertOrUpdate(modelb2border);
                        }
                    }
                }


                return "Fail";
                #endregion
            }
            else if (modelcompro.Serviceid == 3)//美景联动
            {
                #region 美景联动产品
                ApiService mapiservice = new ApiServiceData().GetApiservice(modelcompro.Serviceid);
                if (mapiservice == null)
                {
                    return "fail 产品服务商信息查询失败";
                }
                Api_Mjld_SubmitOrder_input minput = new Api_mjld_SubmitOrder_inputData().GetApi_Mjld_SubmitOrder_input(modelb2border.Id);
                if (minput == null)
                {
                    return "fail 美景联动提单录入信息查询失败";
                }

                string sendresult = new MjldInter().SubmitOrder(mapiservice, minput);

                if (sendresult != "")
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(sendresult);
                    XmlElement root = doc.DocumentElement;

                    string timeStamp = root.SelectSingleNode("timeStamp").InnerText;//时间戳
                    string mjldorderId = root.SelectSingleNode("orderId").InnerText;//美景联动订单id
                    string return_orderId = root.SelectSingleNode("outOrderId").InnerText;//系统订单id
                    string endTime = root.SelectSingleNode("endTime").InnerText;//有效期
                    string credence = root.SelectSingleNode("credence").InnerText;//验证码
                    string inCount = root.SelectSingleNode("inCount").InnerText;//人数
                    string status = root.SelectSingleNode("status").InnerText;//状态

                    Api_Mjld_SubmitOrder_output mout = new Api_Mjld_SubmitOrder_output
                    {
                        id = 0,
                        timeStamp = timeStamp,
                        mjldOrderId = mjldorderId,
                        endTime = endTime,
                        credence = credence,
                        inCount = inCount,
                        status = int.Parse(status),
                        orderId = int.Parse(return_orderId)
                    };
                    int r = new Api_mjld_SubmitOrder_outputData().EditApi_Mjld_SubmitOrder_output(mout);


                    if (status == "0")//发码成功
                    {
                        //修改订单中发码状态为“已发码”，订单状态为"已发码" 
                        modelb2border.Send_state = (int)SendCodeStatus.HasSend;
                        modelb2border.Order_state = (int)OrderStatus.HasSendCode;
                        modelb2border.Ticketcode = 0;
                        modelb2border.Backtickettime = DateTime.Now;

                        modelb2border.service_order_num = mjldorderId;
                        modelb2border.Service_req_seq = timeStamp;
                        modelb2border.Servicepro_v_state = "0";
                        modelb2border.service_code = credence;
                        modelb2border.service_usecount = 0;
                        modelb2border.service_lastcount = inCount.ConvertTo<int>(0);
                        modelb2border.Pno = credence;


                        new B2bOrderData().InsertOrUpdate(modelb2border);

                        return "OK";
                    }
                    else//发码出错
                    {
                        //修改订单中发码状态为“未发码”，订单状态为"发码出错" 
                        modelb2border.Send_state = (int)SendCodeStatus.NotSend;
                        modelb2border.Order_state = (int)OrderStatus.SendCodeErr;
                        modelb2border.Ticketcode = 0;


                        new B2bOrderData().InsertOrUpdate(modelb2border);

                        return "fail 发码出错";
                    }

                }


                return "fail";
                #endregion
            }

            else if (modelcompro.Serviceid == 4)//万龙
            {
                #region 万龙发送
                bool paystate = false;

                try
                {
                    B2b_company commanage = B2bCompanyData.GetAllComMsg(modelcompro.Com_id);
                    WlGetProInfoDealRequestData wldata = new WlGetProInfoDealRequestData(commanage.B2bcompanyinfo.wl_PartnerId, commanage.B2bcompanyinfo.wl_userkey);

                    var wlorder = wldata.SearchWlOrderData(modelcompro.Com_id, 0, "", modelb2border.Id);

                    if (wlorder != null)
                    {
                        if (wlorder.status == 1)
                        {
                            var payorder = wldata.wlOrderPayRequest_json(int.Parse(commanage.B2bcompanyinfo.wl_PartnerId), modelb2border.Id.ToString(), wlorder.wlorderid);//
                            var wlcreate = wldata.wlOrderPayRequest_data(payorder, modelcompro.Com_id);
                            if (wlcreate.IsSuccess == true)
                            {
                                paystate = true;
                                return WLSendSms(modelcompro, modelb2border, order_no);
                            }
                            else
                            {
                                paystate = false;
                            }
                        }
                        else { 
                        //如果
                        
                        }
                    }
                    else {
                        paystate = false;
                    }
                }
                catch
                {
                    paystate = false;
                }





                if (paystate == true)//发码成功
                {
                    //修改订单中发码状态为“已发码”，订单状态为"已发码" 
                    modelb2border.Send_state = (int)SendCodeStatus.HasSend;
                    modelb2border.Order_state = (int)OrderStatus.HasSendCode;
                    modelb2border.Ticketcode = 0;
                    modelb2border.Backtickettime = DateTime.Now;

                    //modelb2border.service_order_num = mjldorderId;
                    //modelb2border.Service_req_seq = timeStamp;
                    //modelb2border.Servicepro_v_state = "0";
                    //modelb2border.service_code = credence;
                    //modelb2border.service_usecount = 0;
                    //modelb2border.service_lastcount = inCount.ConvertTo<int>(0);
                    //modelb2border.Pno = credence;

                    new B2bOrderData().InsertOrUpdate(modelb2border);
                    return "OK";
                }
                else//发码出错
                {
                    //修改订单中发码状态为“未发码”，订单状态为"发码出错" 
                    modelb2border.Send_state = (int)SendCodeStatus.NotSend;
                    modelb2border.Order_state = (int)OrderStatus.SendCodeErr;
                    modelb2border.Ticketcode = 0;

                    new B2bOrderData().InsertOrUpdate(modelb2border);

                    return "fail 发码出错";
                }
                return "fail 发码出错";
                #endregion
            }
            else
            {
                return "Fail 产品服务商信息有误";
            }

        }
        #endregion

        #region 发送倒码电子票
        public string SendDaomaSms(B2b_com_pro modelcompro, B2b_order modelb2border, int order_no)
        {
            //根据产品id得到产品信息
            B2bComProData datapro = new B2bComProData();
            B2bOrderData dataorder = new B2bOrderData();
            //倒码的则读取电子码

            //得到短信
            string sendstr = modelcompro.Sms;

            string pnostr = "";//实际电子码,通过获得电子码返回

            string pno = "";//电子码

            //先判断此订单是否有此订单及产品的电子码了吗，防止重复读取
            pno = datapro.SearchTop1Eticket(modelb2border.Pro_id, modelcompro.Com_id, modelb2border.U_num, order_no, out pnostr);
            if (pno == "")
            {
                //获取电子码
                pno = datapro.ReaderTop1Eticket(modelb2border.Pro_id, modelcompro.Com_id, modelb2border.U_num, order_no, out pnostr);

            }


            if (pno == "")
            {
                return "取得电子码错误！";
            }

            //电子码写入订单表里
            modelb2border.Pno = pnostr;
            modelb2border.Backtickettime = DateTime.Now;
            dataorder.InsertOrUpdate(modelb2border);


            //$票号$ $姓名$ $数量$ $有效期$ $产品名称$ 进行替换
            sendstr = sendstr.Replace("$票号$", pno);
            sendstr = sendstr.Replace("$数量$", modelb2border.U_num.ToString());
            sendstr = sendstr.Replace("$有效期$", modelcompro.Pro_end.ToString("yyyy-MM-dd"));
            sendstr = sendstr.Replace("$产品名称$", modelcompro.Pro_name);
            sendstr = sendstr.Replace("$姓名$", modelb2border.U_name);


            //-----------------新增2 begin-------------------------//
            modelb2border.Send_state = (int)SendCodeStatus.SendLoading;
            modelb2border.Order_state = (int)OrderStatus.HasSendCode;
            modelb2border.Ticketcode = 0;
            //修改订单中发码状态为“发送中”，订单状态为"已发码"，电子码id输入
            modelb2border.Backtickettime = DateTime.Now;
            dataorder.InsertOrUpdate(modelb2border);



            //对优惠券产品 不发送短信 ，直接跳出发送客服通道操作
            if (modelcompro.Server_type == 3 && modelb2border.U_phone.Trim() == "")
            {//优惠券产品,无手机号的，不发送短信，发送客服通道

                var openid_temp = "";

                if (modelb2border.Openid != "")
                {
                    openid_temp = modelb2border.Openid;
                }

                if (openid_temp == "")
                {
                    var crminfo = new B2bCrmData().GetB2bCrmById(modelb2border.U_id);
                    if (crminfo != null)
                    {
                        openid_temp = crminfo.Weixin;
                    }
                }


                if (openid_temp != "")
                {
                    //根据访问获得公司信息
                    WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(modelcompro.Com_id);

                    //短信内容发送微信客服通道
                    var data = CustomerMsg_Send.SendWxMsg(modelcompro.Com_id, modelb2border.Openid, 1, "", sendstr, "", basicc.Weixinno);

                }
                //修改订单中发码状态为“已发码”
                int upsendstate = new B2bOrderData().Upsendstate(order_no, (int)SendCodeStatus.HasSend);
                return "OK";
            }



            int agentid = modelb2border.Agentid;
            if (agentid != 0)
            {
                Agent_company agentcompany = AgentCompanyData.GetAgentByid(agentid);
                if (agentcompany.Agent_messagesetting == 1)//分销商自己发送
                {
                    //修改订单中发码状态为“已发码”
                    int upsendstate = new B2bOrderData().Upsendstate(order_no, (int)SendCodeStatus.HasSend);

                    //记录短信日志表
                    B2bSmsMobileSendDate smsmobilelog = new B2bSmsMobileSendDate();
                    B2b_smsmobilesend smslog = new B2b_smsmobilesend()
                    {
                        Mobile = agentcompany.Mobile,
                        Content = sendstr,
                        Flag = (int)SendCodeStatus.HasSend,
                        Text = agentcompany.Company + "-分销商自己发码",
                        Delaysendtime = "",
                        Oid = order_no,
                        Pno = pnostr,
                        Realsendtime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        Smsid = 99999999,//分销商自己发码
                        Sendeticketid = 0
                    };
                    int insertsendmobileid = smsmobilelog.InsertOrUpdate(smslog);

                }
                else//易城系统发送
                {
                    //异步发送电子码
                    AsyncsendsmsEventHandler mydelegate = new AsyncsendsmsEventHandler(AsyncSendSms);
                    mydelegate.BeginInvoke(modelb2border.U_phone, sendstr, modelcompro.Com_id, order_no, pnostr, 0, modelcompro.Source_type, new AsyncCallback(CompletedSendSms), null);
                }
            }
            else
            {
                //异步发送电子码
                AsyncsendsmsEventHandler mydelegate = new AsyncsendsmsEventHandler(AsyncSendSms);
                mydelegate.BeginInvoke(modelb2border.U_phone, sendstr, modelcompro.Com_id, order_no, pnostr, 0, modelcompro.Source_type, new AsyncCallback(CompletedSendSms), null);
            }
            return "OK";
        }
        #endregion

        #region 异步发送二维码短信

        public static void CompletedSendSms(IAsyncResult result)
        {
            //获取委托对象，调用EndInvoke方法获取运行结果
            AsyncResult _result = (AsyncResult)result;
            AsyncsendsmsEventHandler myDelegate = (AsyncsendsmsEventHandler)_result.AsyncDelegate;

            myDelegate.EndInvoke(_result);
        }


        public static void AsyncSendSms(string phone, string smscontent, int comid, int orderid, string pno, int insertsendEticketid, int pro_sourcetype)
        {
            string msg = "";

            int sendback = -1;
            //判断是否是淘宝订单，并且手机号形式如:135****1212:是的话直接返回发送成功；否则进入正式的发送短信操作
            bool is_tborder = IsTbOrder(phone);
            if (is_tborder)
            {
                sendback = 1;
            }
            else
            {
                sendback = SendSmsHelper.SendSms(phone, smscontent, comid, out msg);
            }


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

                    sendstate = (int)SendCodeStatus.HasSend;
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
            if (pro_sourcetype == 3)//wl修改发送状态
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
                    //单位万龙订单修改的，发送状态与订单状态同时修改为已发送
                    int upsendstate = new B2bOrderData().Uporderstatesendstate(orderid, (int)SendCodeStatus.HasSend,4);



                    sendstate = (int)SendCodeStatus.HasSend;
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
                    sendstate = (int)SendCodeStatus.HasSend;
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


        public RandomCode GetRandomCode()
        {
            lock (lockobj)
            {
                //生成电子码，获取随机码
                RandomCodeData datarandomcode = new RandomCodeData();
                RandomCode modelrandomcode = datarandomcode.GetRandomCode();//得到未用随机码对象

                //设置取出的电子码状态为1（已使用）
                modelrandomcode.State = 1;
                datarandomcode.InsertOrUpdate(modelrandomcode);
                return modelrandomcode;
            }
        }


        //同步对客服发送新订单通知
        public static void SendWeixinKfMsg(int orderid)
        {
            try
            {
                //查询订单
                B2b_order m_order = new B2bOrderData().GetOrderById(orderid);
                if (m_order == null)
                {
                    return;
                }


                var prodata = new B2bComProData();
                var proinfo = prodata.GetProById(m_order.Pro_id.ToString(), m_order.Speciid);
                if (proinfo == null)
                {
                    return;
                }


                //只有支付成功后才发送短信
                if (m_order.Pay_state != 2)
                {
                    if (proinfo.Server_type == 12 || proinfo.Server_type == 13 || proinfo.Server_type == 9)
                    {
                        //如果是 预约产品或教练产品并不需要一定支付
                    }
                    else
                    {
                        return;
                    }
                }

                //查询客服
                var SendEData = new SendEticketData();
                var crmlist = SendEData.SearchIsDefaultKfList(m_order.Comid);

                //针对订房给默认客服发送短信
                if (proinfo.Server_type == 9)
                {
                    if (crmlist.Count > 0)
                    {
                       
                            m_order.M_b2b_order_hotel = new B2b_order_hotelData().GetHotelOrderByOrderId(m_order.Id);
                            if (m_order.Pay_state == 2)
                            {//只对已支付的订房进行发送短信通知
                                for (int i = 0; i < crmlist.Count; i++)
                                {
                                    string msg = "";
                                    string duanxinmsg = "客户预订：" + proinfo.Pro_name + " 姓名:" + m_order.U_name + " (" + m_order.U_phone + ") ，" + m_order.U_num + "间，入住时间:" + m_order.M_b2b_order_hotel.Start_date.ToString("yyyy-MM-dd") + " 离店时间： " + m_order.M_b2b_order_hotel.End_date.ToString("yyyy-MM-dd") + " ,订单号:" + m_order.Id + ",请及时处理。 ";
                                    var sendback = SendSmsHelper.SendSms(crmlist[i].Phone, duanxinmsg, m_order.Comid, out msg);
                                }
                            }
                    }
                }


                //查询微信设置
                WeiXinBasic wxbasic = new WeiXinBasicData().GetWxBasicByComId(m_order.Comid);
                if (wxbasic == null)
                {
                    return;
                }

                var Weixintype = wxbasic.Weixintype;//微信类型
                if (Weixintype != 4)
                {//如果不是 微信认证服务号直接退出，只有微信认证服务号才能继续发送消息模板
                    return;
                }

                WeiXinBasic basic = new WeiXinBasicData().GetWxBasicByComId(m_order.Comid);




                if (crmlist.Count > 0)
                {

                    for (int i = 0; i < crmlist.Count; i++)
                    {
                        //new Weixin_tmplmsgManage().WxTmplMsg_OrderNewCreate(orderid, crmlist[i].Weixin);//对客服发送
                        string sms = "";
                        //给客服发送通知 ，通过客服通道
                        if (proinfo.Server_type == 12)
                        {
                            //支付后通知
                            if (proinfo.bookpro_ispay == 1)
                            {
                                if (m_order.Pay_state == 2)
                                {
                                    string data_suodingtongzhi = CustomerMsg_Send.SendWxMsg(m_order.Comid, crmlist[i].Weixin, 1, "", "新预约订单提交(已付款)：\n\n客户:" + m_order.U_name + " (" + m_order.U_phone + ")提交一笔新预约订单。\n\n" + proinfo.Pro_name + ", \n\n订单号：" + orderid + " \n\n预约时间：" + m_order.U_traveldate.ToString("yyyy-MM-dd hh:mm:ss") + ",\n\n请及时联系处理。", "", basic.Weixinno);
                                }
                                else {
                                    string data_suodingtongzhi = CustomerMsg_Send.SendWxMsg(m_order.Comid, crmlist[i].Weixin, 1, "", "新预约订单提交（等待客户付款）：\n\n客户:" + m_order.U_name + " (" + m_order.U_phone + ")提交一笔新预约订单。\n\n" + proinfo.Pro_name + ", \n\n订单号：" + orderid + " \n\n预约时间：" + m_order.U_traveldate.ToString("yyyy-MM-dd hh:mm:ss") + ",\n\n请及时联系处理。", "", basic.Weixinno);
                                
                                }
                            }
                            else {
                                string data_suodingtongzhi = CustomerMsg_Send.SendWxMsg(m_order.Comid, crmlist[i].Weixin, 1, "", "新预约订单提交（免支付预约）：\n\n客户:" + m_order.U_name + " (" + m_order.U_phone + ")提交一笔新预约订单。\n\n" + proinfo.Pro_name + ", \n\n订单号：" + orderid + " \n\n预约时间：" + m_order.U_traveldate.ToString("yyyy-MM-dd hh:mm:ss") + ",\n\n请及时联系处理。", "", basic.Weixinno);
                            }
                        
                        
                        }
                        else if (proinfo.Server_type == 13)
                        {//对教练产品

                            //教练产品先 不发送 通知客服的信息。不发此信息
                            string data_suodingtongzhi = CustomerMsg_Send.SendWxMsg(m_order.Comid, crmlist[i].Weixin, 1, "", "新教练预约订单提交：\n\n客户:" + m_order.U_name + " (" + m_order.U_phone + ")提交一笔新订单。\n\n" + proinfo.Pro_name + ",\n\n 订单号：" + orderid + " \n\n预约时间：" + m_order.U_traveldate.ToString("yyyy-MM-dd hh:mm:ss") + ",请及时联系处理。", "", basic.Weixinno);
                        }
                        else if (proinfo.Server_type == 9)
                        {//订房产品
                            if (m_order.Pay_state == 2)
                            {//只对已支付的订房进行发送短信通知
                                string Returnmd5_temp = EncryptionHelper.ToMD5(orderid.ToString() + "lixh1210", "UTF-8");
                                var querenduanxin_weixin = "有客户预订房间：\n\n " + proinfo.Pro_name + " \n\n客户:" + m_order.U_name + " (" + m_order.U_phone + ") ，" + m_order.U_num + "间,入住时间:" + m_order.U_traveldate.ToString("yyyy-MM-dd hh:mm") + " 离店时间：" + m_order.M_b2b_order_hotel.End_date.ToString("yyyy-MM-dd") + " \n\n点击下面链接进行确认。\n http://shop" + proinfo.Com_id + ".etown.cn/h5/Confirmyuyue.aspx?id=" + orderid + "&md5=" + Returnmd5_temp;
                                string data_suodingtongzhi = CustomerMsg_Send.SendWxMsg(m_order.Comid, crmlist[i].Weixin, 1, "", querenduanxin_weixin, "", basic.Weixinno);
                            }
                             
                        }
                        else
                        {
                            string data_suodingtongzhi = CustomerMsg_Send.SendWxMsg(m_order.Comid, crmlist[i].Weixin, 1, "", "新订单提交：\n\n客户:" + m_order.U_name + " (" + m_order.U_phone + ")提交一笔新订单。\n\n" + proinfo.Pro_name + ", \n\n订单号：" + orderid + ",\n\n请及时联系处理。", "", basic.Weixinno);

                        }
                    }

                }

                //给绑定顾问发送通知
                if (proinfo.bookpro_bindphone != "")
                {

                    string bindinggwweixin = "";

                    var crminfo = SendEData.SearchBindingGw(proinfo.bookpro_bindphone, m_order.Comid);
                    if (crminfo != null)
                    {
                        for (int i = 0; i < crminfo.Count; i++)
                        {
                            bindinggwweixin = crminfo[i].Weixin;
                            if (bindinggwweixin != "")
                            {

                                if (proinfo.Server_type != 13 && proinfo.Server_type != 9)//预约教练，酒店不发送，其他 只要绑定了手机就发送
                                {

                                    //支付后通知
                                    if (proinfo.bookpro_ispay == 1)
                                    {
                                        if (m_order.Pay_state == 2)
                                        {
                                            //给客服发送通知 ，通过客服通道
                                            string data_suodingtongzhi = CustomerMsg_Send.SendWxMsg(m_order.Comid, bindinggwweixin, 1, "", proinfo.bookpro_bindname + "您好（支付成功），\n\n客户:" + m_order.U_name + " (" + m_order.U_phone + ")提交一笔新预约订单。" + proinfo.Pro_name + ",\n\n 订单号：" + orderid + " \n\n预约时间：" + m_order.U_traveldate.ToString("yyyy-MM-dd hh:mm:ss") + ",\n\n请及时联系处理。", "", basic.Weixinno);
                                            string msg = "";
                                            var sendback = SendSmsHelper.SendSms(proinfo.bookpro_bindphone, proinfo.bookpro_bindname + "您好（支付成功），客户:" + m_order.U_name + " (" + m_order.U_phone + ")提交一笔新预约订单。" + proinfo.Pro_name + ", 订单号：" + orderid + " 预约时间：" + m_order.U_traveldate.ToString("yyyy-MM-dd hh:mm:ss") + ",请及时联系处理。", m_order.Comid, out msg);
                                   
                                        }
                                        else
                                        {
                                            //给客服发送通知 ，通过客服通道
                                            string data_suodingtongzhi = CustomerMsg_Send.SendWxMsg(m_order.Comid, bindinggwweixin, 1, "", proinfo.bookpro_bindname + "您好（等待客户支付），\n\n客户:" + m_order.U_name + " (" + m_order.U_phone + ")提交一笔新预约订单。" + proinfo.Pro_name + ",\n\n 订单号：" + orderid + " \n\n预约时间：" + m_order.U_traveldate.ToString("yyyy-MM-dd hh:mm:ss") + ",\n\n请及时联系处理。", "", basic.Weixinno);
                                            //string msg = "";
                                            //var sendback = SendSmsHelper.SendSms(proinfo.bookpro_bindphone, proinfo.bookpro_bindname + "您好（等待客户支付），客户:" + m_order.U_name + " (" + m_order.U_phone + ")提交一笔新预约订单。" + proinfo.Pro_name + ", 订单号：" + orderid + " 预约时间：" + m_order.U_traveldate.ToString("yyyy-MM-dd hh:mm:ss") + ",请及时联系处理。", m_order.Comid, out msg);
                                   
                                        }
                                    }
                                    else
                                    {

                                        //给客服发送通知 ，通过客服通道
                                        string data_suodingtongzhi = CustomerMsg_Send.SendWxMsg(m_order.Comid, bindinggwweixin, 1, "", proinfo.bookpro_bindname + "您好(免支付预约)，\n\n客户:" + m_order.U_name + " (" + m_order.U_phone + ")提交一笔新预约订单。" + proinfo.Pro_name + ",\n\n 订单号：" + orderid + " \n\n预约时间：" + m_order.U_traveldate.ToString("yyyy-MM-dd hh:mm:ss") + ",\n\n请及时联系处理。", "", basic.Weixinno);
                                        string msg = "";
                                        var sendback = SendSmsHelper.SendSms(proinfo.bookpro_bindphone, proinfo.bookpro_bindname + "您好(免支付预约)，客户:" + m_order.U_name + " (" + m_order.U_phone + ")提交一笔新预约订单。" + proinfo.Pro_name + ", 订单号：" + orderid + " 预约时间：" + m_order.U_traveldate.ToString("yyyy-MM-dd hh:mm:ss") + ",请及时联系处理。", m_order.Comid, out msg);
                                    }
                                }
                            }
                        }
                    }
                }



            }
            catch
            {
                return;
            }
        }



        //同步对客服发送验证通知
        public static void SendWeixinYanzhengMsg(int orderid, int num)
        {
            try
            {
                //查询订单
                B2b_order m_order = new B2bOrderData().GetOrderById(orderid);
                if (m_order == null)
                {
                    return;
                }
                //只有支付成功后才发送短信
                if (m_order.Pay_state != 2)
                {
                    return;
                }

                var prodata = new B2bComProData();
                var proinfo = prodata.GetProById(m_order.Pro_id.ToString());
                if (proinfo == null)
                {
                    return;
                }
                if (proinfo.Server_type != 12)
                {//只针对预约产品验证发送验证通知
                    return;
                }

                //查询微信设置
                WeiXinBasic wxbasic = new WeiXinBasicData().GetWxBasicByComId(m_order.Comid);
                if (wxbasic == null)
                {
                    return;
                }

                var Weixintype = wxbasic.Weixintype;//微信类型
                if (Weixintype != 4)
                {//如果不是 微信认证服务号直接退出，只有微信认证服务号才能继续发送消息模板
                    return;
                }

                WeiXinBasic basic = new WeiXinBasicData().GetWxBasicByComId(m_order.Comid);


                //查询客服
                var SendEData = new SendEticketData();
                var crmlist = SendEData.SearchIsDefaultKfList(m_order.Comid);

                if (crmlist.Count > 0)
                {

                    for (int i = 0; i < crmlist.Count; i++)
                    {
                        //给客服发送通知 ，通过客服通道
                        string data_suodingtongzhi = CustomerMsg_Send.SendWxMsg(m_order.Comid, crmlist[i].Weixin, 1, "", "预约产品验证：客户:" + m_order.U_name + " (" + m_order.U_phone + ") 验证 " + proinfo.Pro_name + "。 验证数量：" + num + " 验证时间:" + DateTime.Now, "", basic.Weixinno);
                    }

                }

                //给客户发送验证通知
                var crmdata = new B2bCrmData();
                var uinfo = crmdata.GetB2bCrmById(m_order.U_id);
                if (uinfo != null)
                {
                    if (uinfo.Weixin != "")
                    {

                        //给客户发送通知 ，通过客服通道
                        string data_suodingtongzhi = CustomerMsg_Send.SendWxMsg(m_order.Comid, uinfo.Weixin, 1, "", "尊敬的 " + m_order.U_name + " 你好！ 您的预约产品：" + proinfo.Pro_name + "已验证，验证数量：" + num + " 验证时间:" + DateTime.Now, "", basic.Weixinno);
                    }
                }




                //给绑定顾问发送通知
                if (proinfo.bookpro_bindphone != "")
                {
                    string bindinggwweixin = "";

                    var crminfo = SendEData.SearchBindingGw(proinfo.bookpro_bindphone, m_order.Comid);
                    if (crminfo != null)
                    {
                        for (int i = 0; i < crminfo.Count; i++)
                        {
                            bindinggwweixin = crminfo[i].Weixin;
                            if (bindinggwweixin != "")
                            {
                                //给客服发送通知 ，通过客服通道
                                string data_suodingtongzhi = CustomerMsg_Send.SendWxMsg(m_order.Comid, crmlist[i].Weixin, 1, "", proinfo.bookpro_bindname + "您好，您绑定的预约产品 ：" + proinfo.Pro_name + " 客户:" + m_order.U_name + " (" + m_order.U_phone + ") 已经验证 。 验证数量：" + num + " 验证时间:" + DateTime.Now, "", basic.Weixinno);
                                var msg = "";
                                var sendback = SendSmsHelper.SendSms(proinfo.bookpro_bindphone, proinfo.bookpro_bindname + "您好，您绑定的预约产品 ：" + proinfo.Pro_name + " 客户:" + m_order.U_name + " (" + m_order.U_phone + ") 已经验证 。 验证数量：" + num + " 验证时间:" + DateTime.Now, m_order.Comid, out msg);

                            }
                        }
                    }
                }



            }
            catch
            {
                return;
            }
        }



        //获取绑定微信的 默认客服
        public List<B2b_crm> SearchIsDefaultKfList(int comid)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalSendEticketData(helper).SearchIsDefaultKfList(comid);
                return list;
            }
        }
        //获取绑定微信的 默认客服
        public List<B2b_crm> SearchBindingGw(string phone, int comid)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalSendEticketData(helper).SearchBindingGw(phone, comid);
                return list;
            }
        }



        public static int JiaolianTqBaoxinmsg(int speciid)
        {

            int dxbuytimenum = 0;

            string dxspeciname = new B2b_com_pro_SpeciData().Getspecinamebyid(speciid);
            if (dxspeciname != "")
            {
                string[] dxstr = dxspeciname.Split(';');
                foreach (string dxr in dxstr)
                {
                    if (dxr != "")
                    {
                        if (dxr.IndexOf("时间") > -1)
                        {
                            int dxqixian = CommonFunc.GetNumber(dxr);
                            if (dxqixian == 0)
                            {
                                return 0;
                            }
                            //得到期限单位:天；月；年
                            if (dxr.IndexOf("小时") > -1)
                            {
                                return dxqixian;
                            }
                            //if (dxr.IndexOf("h") > -1)
                            //{
                            //    return dxqixian;
                            //}
                            //if (dxr.IndexOf("天") > -1)
                            //{
                            //    return dxqixian*24;//如果
                            //}
                        }

                    }
                }
                return dxbuytimenum;
            }
            else
            {
                return dxbuytimenum;
            }
        }

        //插入电子票押金服务表 ，此电子票关联的服务及押金金额及相关的订单号
        public static int InsertEticetServerDepositbyorder(B2b_order orderinfo, string pno)
        {
            B2bEticketData eticketdata = new B2bEticketData();
            if (orderinfo.serverid != "")
            {
                if (pno != "")
                {
                    var pno_arr = pno.Split(',');
                    for (int i = 0; i < pno_arr.Count(); i++)
                    {
                        if (pno_arr[i].Trim() != "")
                        {
                            var eticketinfo = new B2bEticketData().GetPnoEticketinfo(pno_arr[i].Trim());
                            if (eticketinfo != null)
                            {
                                eticketinfo.ishasdeposit = 1;//已支付押金
                                var eticketup = eticketdata.InsertOrUpdate(eticketinfo);//修改电子票有押金

                                //查询是否已经验证发卡

                                var inb2b_Rentserver_temp = new RentserverData().SearchRentserver_bypno(pno_arr[i].Trim());

                                //查询是否有多个服务
                                var orderinfo_arr = orderinfo.serverid.Split(',');//分解

                                for (int j = 0; j < orderinfo_arr.Count(); j++)
                                {
                                    if (orderinfo_arr[j].Trim() != "")
                                    {
                                        var sid_temp = int.Parse(orderinfo_arr[j]);
                                        var tempid = InsertEticetServerDepositbyorder_2(sid_temp, eticketinfo, orderinfo.Id);//最后插入

                                        //当插入电子票表后，检查是否已经验证，如果验证要再次插入服务验证表
                                        if (tempid != 0)
                                        {
                                            if (inb2b_Rentserver_temp != null)
                                            {
                                                var Rentserver_User_info_temp = new B2b_Rentserver_User_info();

                                                var rentserverdata = new RentserverData().GetRentServerById(sid_temp);
                                                Rentserver_User_info_temp.num = rentserverdata.num;
                                                Rentserver_User_info_temp.Rentserverid = tempid;
                                                Rentserver_User_info_temp.Userid = inb2b_Rentserver_temp.id;
                                                Rentserver_User_info_temp.Verstate = 0;

                                                //对每一种服务进行插入
                                                var inb2b_Rentserver_User_info_temp = new RentserverData().inb2b_Rentserver_User_info(Rentserver_User_info_temp);
                                            }

                                        }


                                        //插入子服务
                                        var rentserver_id_temp = 0;
                                        var rentserver_f_temp = new RentserverData().RentserverbyFid(sid_temp, orderinfo.Comid);
                                        if (rentserver_f_temp != null)
                                        {
                                            rentserver_id_temp = rentserver_f_temp.id;//获取子服务id

                                            var tempfid = InsertEticetServerDepositbyorder_2(rentserver_id_temp, eticketinfo, orderinfo.Id);//插入电子票服务关联表带押金

                                            //当插入电子票表后，检查是否已经验证，如果验证要再次插入服务验证表
                                            if (tempfid != 0)
                                            {
                                                if (inb2b_Rentserver_temp != null)
                                                {
                                                    var Rentserver_User_info_temp = new B2b_Rentserver_User_info();

                                                    var rentserverdata = new RentserverData().GetRentServerById(rentserver_id_temp);
                                                    Rentserver_User_info_temp.num = rentserverdata.num;
                                                    Rentserver_User_info_temp.Rentserverid = tempfid;
                                                    Rentserver_User_info_temp.Userid = inb2b_Rentserver_temp.id;
                                                    Rentserver_User_info_temp.Verstate = 0;

                                                    //对每一种服务进行插入
                                                    var inb2b_Rentserver_User_info_temp = new RentserverData().inb2b_Rentserver_User_info(Rentserver_User_info_temp);
                                                }

                                            }


                                        }





                                    }
                                }

                            }
                        }
                    }
                    return 1;//涉及到循环插入，如何防止 重复插入后漏插入，
                }
            }

            return 0;
        }

        public static int InsertEticetServerDepositbyorder_2(int sid, B2b_eticket eticketinfo, int orderid)
        {
            var Rentserverinfo = new RentserverData().Rentserverbyid(sid, eticketinfo.Com_id);

            if (Rentserverinfo != null)
            {

                //插入电子票所带服务及价格，押金
                b2b_eticket_Deposit eticketdeposit = new b2b_eticket_Deposit();
                eticketdeposit.eticketid = eticketinfo.Id;
                eticketdeposit.sid = Rentserverinfo.id;
                eticketdeposit.Depositorder = orderid;
                eticketdeposit.saleprice = Rentserverinfo.saleprice;
                eticketdeposit.Depositprice = Rentserverinfo.serverDepositprice;
                eticketdeposit.Backdepositstate = 0;
                eticketdeposit.id = 0;

                var tempid = new B2bEticketData().InsertOrUpdateB2b_eticket_Deposit(eticketdeposit);

                return tempid;
            }
            return 0;
        }


        public bool IsMobile(string uphone)
        {

            string fphone = @"^1\d{10}$";
            Regex dReg = new Regex(fphone);
            if (dReg.IsMatch(uphone))
            {
                return true;
            }
            else
            {
                return false;
            }

        }


    }
}
