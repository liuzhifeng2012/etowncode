using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Data;
using Newtonsoft.Json;
using ETS.Data.SqlHelper;
using System.Web;
using System.Web.SessionState;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS.Framework;
using System.Collections;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle.Enum;
using System.Xml;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.VAS.Service.VASService.Modle.Enum;
using ETS2.VAS.Service.VASService.Data;
using ETS2.VAS.Service.VASService.Data.Common;
using ETS2.PM.Service.Taobao_Ms.Model;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using ETS2.PM.Service.Taobao_Ms.Data;
using Top.Api.Util;
using ETS2.PM.Service.Qunar_Ms.Model;
using ETS2.PM.Service.Qunar_Ms.Data;
using ETS2.PM.Service.Meituan.Model;
using ETS2.PM.Service.Meituan.Data;
using ETS2.Common.Business;
using ETS2.PM.Service.LMM.Data;
using ETS2.PM.Service.LMM.Model;
using ETS2.PM.Service.WL.Data;


namespace ETS.JsonFactory
{
    public class EticketJsonData
    {
        private static object lockobjj = new object();

        private static string tb_returl = "http://gw.api.taobao.com/router/rest";
        //以下5个参数 是码商接口参数
        private static string tb_CodemerchantId = "727429491";//码商ID
        private static string tb_appkey = "23139679";//开放平台证书权限管理App Key
        private static string tb_appsecret = "adde2a4100288166bbee8df66c127d42";//开放平台证书权限管理App Secret
        //开放平台应用通过授权（参考：用户授权介绍）得到的Access Token值（原老的TOP协议对应为SessionKey，现Oauth2.0协议对应为Access Token）。
        private static string tb_session = "61017227c9b25cd5e74e3daf09f1471cfaa3f87cd1d5a16727429491";
        private static string tb_refresh_token = "61021225b4d99ef699e27391422c86188ccd989e2d45766727429491";


        public static string GetEticketDetail(string pno, string comid)
        {

            try
            {
                var prodata = new B2bEticketData();
                var eticketinfo = prodata.GetEticketDetail(pno);



                string reason = "";
                //录入电子码验证日志
                B2bEticketLogData elogdata = new B2bEticketLogData();
                B2b_eticket_log elog = new B2b_eticket_log()
                   {
                       Id = 0,
                       Eticket_id = 0,
                       Pno = pno,
                       Action = (int)ECodeOper.ValidateECode,
                       A_state = (int)ECodeOperStatus.OperFail,
                       A_remark = reason,
                       Use_pnum = 0,
                       Actiondate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                       Com_id = int.Parse(comid),
                       PosId = 0
                   };

                if (eticketinfo == null)
                {
                    reason = "查询操作不成功，不存在此电子码，请核查是否输入错误";
                    elog.A_remark = reason;
                    elogdata.InsertOrUpdateLog(elog);
                    return JsonConvert.SerializeObject(new { type = 1, msg = reason });
                }

                //是否是此商家的产品
                int com_id = eticketinfo.Com_id;
                if (com_id.ToString() != comid)
                {
                    reason = "电子码无法使用,不是此商家产品";
                    elog.A_remark = reason;
                    elog.Eticket_id = new B2bEticketData().GetEticketDetail(pno).Id;
                    elogdata.InsertOrUpdateLog(elog);
                    return JsonConvert.SerializeObject(new { type = 1, msg = reason });
                }
                //电子码可使用票数
                int usenum = eticketinfo.Use_pnum;
                if (usenum == 0)
                {
                    reason = "电子码无法使用,电子码可使用数量为0";
                    elog.A_remark = reason;
                    elog.Eticket_id = new B2bEticketData().GetEticketDetail(pno).Id;
                    elogdata.InsertOrUpdateLog(elog);
                    return JsonConvert.SerializeObject(new { type = 1, msg = reason });

                }
                //电子码状态（作废，正常）
                int vstate = eticketinfo.V_state;
                if (vstate == (int)EticketCodeStatus.HasValidate || vstate == (int)EticketCodeStatus.HasZuoFei)
                {
                    reason = "电子码无法使用,电子码状态：" + EnumUtils.GetName((EticketCodeStatus)vstate);
                    elog.A_remark = reason;
                    elog.Eticket_id = new B2bEticketData().GetEticketDetail(pno).Id;
                    elogdata.InsertOrUpdateLog(elog);
                    return JsonConvert.SerializeObject(new { type = 1, msg = reason });
                }



                return JsonConvert.SerializeObject(new { type = 100, msg = eticketinfo });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        delegate void AsyncsendEventHandler(string updateurl, string pno, int confirmnum, string confirmtime, int agentcomid, int comid, int validateticketlogid);//发送验证同步发送请求委托

        delegate void AsyncsendEventHandler_qunar(B2b_order ordermodel);//发送验证同步发送请求委托-去哪

        delegate void AsyncsendEventHandler_weixinyanzheng(int orderid, int usenum);


        public static string EConfirm(string pno, string usenum, string comid = "", int posid = 0, string randomid = "", string pcaccount = "")
        {
            lock (lockobjj)
            {

                using (var sql = new SqlHelper())
                {
                    try
                    {
                        //进行一次查询
                        var prodata = new B2bEticketData();
                        var eticketinfo = prodata.GetEticketDetail(pno);
                        if (eticketinfo == null)
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "没有此电子票" });
                        }


                        #region 对电子票情况进行检查：判断是否可以验证
                        string reason = "";
                        //录入电子码验证日志
                        B2bEticketLogData elogdata = new B2bEticketLogData();
                        B2b_eticket_log elog = new B2b_eticket_log()
                        {
                            Id = 0,
                            Eticket_id = eticketinfo.Id,
                            Pno = pno,
                            Action = (int)ECodeOper.ValidateECode,
                            A_state = (int)ECodeOperStatus.OperFail,
                            A_remark = reason,
                            Use_pnum = 0,
                            Actiondate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                            Com_id = eticketinfo.Com_id,
                            PosId = posid,
                            RandomId = randomid,
                            Pcaccount = pcaccount
                        };



                        //产品有效期，产品状态
                        var pro = new B2bComProData().GetProById(eticketinfo.Pro_id.ToString());
                        if (pro == null)
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "获取产品失败" });
                        }

                        #region  限定产品使用日期
                        int isblackoutdate = pro.isblackoutdate;//判断产品是否限定产品使用日期
                        if (isblackoutdate == 1)
                        {
                            //得到当前日期类型
                            B2b_com_blackoutdates m_blackoutdate = new B2b_com_blackoutdatesData().Getblackoutdate(DateTime.Now.ToString("yyyy-MM-dd"), pro.Com_id);
                            int nowdatetype = 0;//0平日；1周末；2节假日
                            if (m_blackoutdate == null)
                            {
                                //按系统默认规则:周六日为周末，其他为平日;无节假日
                                if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    nowdatetype = 1;
                                }
                                else
                                {
                                    nowdatetype = 0;
                                }
                            }
                            else
                            {
                                //按系统设定
                                nowdatetype = m_blackoutdate.datetype;
                            }

                            //得到当日日期类型下可以使用的电子票类型
                            List<B2b_eticket_useset> list_B2b_eticket_useset = new B2b_eticket_usesetData().Geteticket_usesetlist(pro.Com_id, nowdatetype.ToString());
                            if (list_B2b_eticket_useset == null)
                            {
                                #region 按系统默认规则
                                if (nowdatetype == 0) { }//平日：可以验证 平日，周末，节假日票；
                                if (nowdatetype == 1)//周末：可以验证 周末，节假日票；
                                {
                                    if (pro.etickettype == 0)
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = "平日票在周末不可使用" });
                                    }
                                }
                                if (nowdatetype == 2)//节假日：只可以验证节假日票 ；
                                {
                                    if (pro.etickettype == 0)
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = "平日票在节假日不可使用" });
                                    }
                                    if (pro.etickettype == 1)
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = "周末票在节假日不可使用" });
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                if (list_B2b_eticket_useset.Count == 0)
                                {
                                    #region 按系统默认规则
                                    if (nowdatetype == 0) { }//平日：可以验证 平日，周末，节假日票；
                                    if (nowdatetype == 1)//周末：可以验证 周末，节假日票；
                                    {
                                        if (pro.etickettype == 0)
                                        {
                                            return JsonConvert.SerializeObject(new { type = 1, msg = "平日票在周末不可使用" });
                                        }
                                    }
                                    if (nowdatetype == 2)//节假日：只可以验证节假日票 ；
                                    {
                                        if (pro.etickettype == 0)
                                        {
                                            return JsonConvert.SerializeObject(new { type = 1, msg = "平日票在节假日不可使用" });
                                        }
                                        if (pro.etickettype == 1)
                                        {
                                            return JsonConvert.SerializeObject(new { type = 1, msg = "周末票在节假日不可使用" });
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region 按系统设定规则
                                    List<int> canuse_etickettypeint = new List<int>();
                                    foreach (B2b_eticket_useset md in list_B2b_eticket_useset)
                                    {
                                        canuse_etickettypeint.Add(md.etickettype);
                                    }

                                    if (canuse_etickettypeint.Contains(pro.etickettype) == false)
                                    {
                                        string datatypename = "";
                                        if (nowdatetype == 0)
                                        {
                                            datatypename = "平日";
                                        }
                                        if (nowdatetype == 1)
                                        {
                                            datatypename = "周末";
                                        }
                                        if (nowdatetype == 2)
                                        {
                                            datatypename = "节假日";
                                        }

                                        string etickettypename = "";
                                        if (pro.etickettype == 0)
                                        {
                                            etickettypename = "平日票";
                                        }
                                        if (pro.etickettype == 1)
                                        {
                                            etickettypename = "周末票";
                                        }
                                        if (pro.etickettype == 2)
                                        {
                                            etickettypename = "节假日票";
                                        }

                                        return JsonConvert.SerializeObject(new { type = 1, msg = etickettypename + "在" + datatypename + "不可使用" });
                                    }
                                    #endregion
                                }
                            }



                        }

                        #endregion

                        #region 判断有效期
                        string provalidatemethod = pro.ProValidateMethod;//判断 1按产品有效期，2指定有效期
                        int appointdate = pro.Appointdata;//1=一星期，，2=1个月，3=3个月，4=6个月，5=一年
                        int iscanuseonsameday = pro.Iscanuseonsameday;//1当天可用，0当天不可用



                        DateTime sendtime = DateTime.Parse(eticketinfo.Subdate.ToString("yyyy-MM-dd"));//电子票生成发送日期

                        DateTime pro_end = pro.Pro_end; //有效期结束时间
                        DateTime pro_start = pro.Pro_start;//有效期开始时间
                        DateTime nowday = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));//现在时间
                        ////限制当天不可用
                        //if (iscanuseonsameday == 0)
                        //{
                        //    if (nowday == sendtime)
                        //    {//验票时间等于电子票生成日期
                        //        return JsonConvert.SerializeObject(new { type = 1, msg = "此票订购当天不可使用" });
                        //    }
                        //}
                        //使用限制(1.当天出票可用；0.当天出票不可用；2.2小时内出票不可用；)
                        if (iscanuseonsameday == 0)
                        {
                            if (nowday == sendtime)
                            {//验票时间等于电子票生成日期
                                return JsonConvert.SerializeObject(new { type = 1, msg = "此票订购当天不可使用" });
                            }
                        }
                        if (iscanuseonsameday == 2)
                        {
                            if (eticketinfo.Subdate.AddMinutes(30) >= DateTime.Now)
                            {
                                //验票时间在 出票时间2个小时内--*20150206 经理建议：实际验证限制半小时
                                return JsonConvert.SerializeObject(new { type = 1, msg = "此电子票出票2小时内不可使用" });
                            }
                        }

                        if (provalidatemethod == "2")//按指定有效期
                        {
                            //结束时间
                            if (appointdate == (int)ProAppointdata.NotAppoint)
                            {
                            }
                            else if (appointdate == (int)ProAppointdata.OneWeek)
                            {
                                pro_end = sendtime.AddDays(7);
                            }
                            else if (appointdate == (int)ProAppointdata.OneMonth)
                            {
                                pro_end = sendtime.AddMonths(1);
                            }
                            else if (appointdate == (int)ProAppointdata.ThreeMonth)
                            {
                                pro_end = sendtime.AddMonths(3);
                            }
                            else if (appointdate == (int)ProAppointdata.SixMonth)
                            {
                                pro_end = sendtime.AddMonths(6);
                            }
                            else if (appointdate == (int)ProAppointdata.OneYear)
                            {
                                pro_end = sendtime.AddYears(1);
                            }

                            //如果指定有效期大于产品有效期，则按产品有效期
                            if (pro_end > pro.Pro_end)
                            {
                                pro_end = pro.Pro_end;
                            }

                        }
                        //起始时间，不用处理

                        //预约和教练产品 限定预约当天使用其他时间不可验证
                        if (pro.Server_type == 12 || pro.Server_type == 13 )
                        {
                            if (pro.unyuyueyanzheng == 1)
                            {
                                var orderinfo_temp = new B2bOrderData().GetOrderById(eticketinfo.Oid);
                                if (orderinfo_temp != null)
                                {
                                    if (orderinfo_temp.U_traveldate.ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd"))
                                    {

                                        return JsonConvert.SerializeObject(new { type = 1, msg = "您预约的时间为" + orderinfo_temp.U_traveldate.ToString("yyyy-MM-dd") + " 只能当天验证使用,如果已过期将不能使用。" });
                                    }

                                }
                            }
                        }

                        //预约，班车 都只能限定当天使用
                        if (pro.Server_type == 10)
                        {
                            if (posid != 0)// && posid != 999999999
                            {//验票机验票 只能指定当天验证，而服务器后台 不限制
                                var orderinfo_temp = new B2bOrderData().GetOrderById(eticketinfo.Oid);
                                if (orderinfo_temp != null)
                                {
                                    if (orderinfo_temp.U_traveldate.ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd"))
                                    {

                                        return JsonConvert.SerializeObject(new { type = 1, msg = "您预约的时间为" + orderinfo_temp.U_traveldate.ToString("yyyy-MM-dd") + " 只能当天验证使用。" });
                                    }
                                }
                            }
                        }


                        #endregion

                        if (pro_start <= DateTime.Now && pro_end >= DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
                        {
                        }
                        else
                        {
                            reason = "产品已过有效期，请联系商家：" + pro_start + "--" + pro_end;
                            elog.A_remark = reason;
                            elogdata.InsertOrUpdateLog(elog);
                            return JsonConvert.SerializeObject(new { type = 1, msg = reason });
                        }





                        var prostate = pro.Pro_state;
                        //if (prostate == (int)ProductStatus.Stop)
                        //{
                        //    reason = "电子码无法使用,产品已暂停";
                        //    elog.A_remark = reason;
                        //    elogdata.InsertOrUpdateLog(elog);
                        //    return JsonConvert.SerializeObject(new { type = 1, msg = reason });

                        //}

                        //使用数量
                        var use_num = eticketinfo.Use_pnum;//可用数量
                        if (int.Parse(usenum) > use_num)
                        {
                            reason = "电子码无法使用,可用数量为：" + use_num;
                            elog.A_remark = reason;
                            elogdata.InsertOrUpdateLog(elog);
                            return JsonConvert.SerializeObject(new { type = 1, msg = reason });
                        }


                        //确认使用
                        var Edata = new B2bEticketData();

                        var surplusnum = use_num - int.Parse(usenum);//剩余可使用数量
                        eticketinfo.Use_pnum = surplusnum;
                        if (surplusnum > 0)
                        {
                            eticketinfo.V_state = (int)EticketCodeStatus.PartValidate;
                        }
                        else
                        {
                            eticketinfo.V_state = (int)EticketCodeStatus.HasValidate;
                        }

                        #endregion

                        #region 电子码如果是 淘宝码商发送的电子码，需要进行 核销(验证)申请回调
                        Taobao_send_noticeretlog sendretlog = new Taobao_send_noticeretlogData().GetSendRetLogByQrcode(pno);

                        Taobao_send_noticelog sendnoticelog = new Taobao_send_noticelog();

                        if (sendretlog != null)
                        {
                            string tb_xml = string.Empty;
                            bool bl = false;//给制定状态 
                            string selfdefine_serial_num = "000" + DateTime.Now.ToString("yyyyMMddHHmmssfff"); //自定义核销流水号 

                            bool isconsumesuc = false;// 核销回调申请 结果
                            string tb_resultdesc = "";// 核销回调申请 结果 描述
                            try
                            {
                                //得到淘宝发码通知
                                sendnoticelog = new Taobao_send_noticelogData().GetSendNoticeLogByTbOid(sendretlog.order_id);
                                if (sendnoticelog != null)
                                {

                                    #region 对核销回调进行记录
                                    Taobao_consume_retlog mretlog = new Taobao_consume_retlog
                                    {
                                        id = 0,
                                        order_id = sendretlog.order_id,
                                        verify_code = sendretlog.verify_codes.Substring(0, sendretlog.verify_codes.IndexOf(":")),
                                        token = sendnoticelog.token,
                                        consume_num = int.Parse(usenum),
                                        codemerchant_id = tb_CodemerchantId,
                                        posid = posid.ToString(),
                                        mobile = "",
                                        new_code = "",
                                        serial_num = selfdefine_serial_num,
                                        qr_images = "",
                                        ret_code = "",
                                        item_title = sendnoticelog.item_title,
                                        left_num = 0,
                                        sms_tpl = "",
                                        print_tpl = "",
                                        consume_secial_num = selfdefine_serial_num,
                                        code_left_num = 0,
                                        ret_time = DateTime.Now,
                                        ycSystemRandomid = randomid

                                    };
                                    int logid = new Taobao_consume_retlogData().Editcomsumeretlog(mretlog);
                                    mretlog.id = logid;
                                    #endregion

                                    #region 调用淘宝核销申请接口
                                    string url = tb_returl;
                                    ITopClient client = new DefaultTopClient(url, tb_appkey, tb_appsecret);
                                    VmarketEticketConsumeRequest req = new VmarketEticketConsumeRequest();
                                    req.OrderId = long.Parse(sendretlog.order_id);
                                    req.VerifyCode = sendretlog.verify_codes.Substring(0, sendretlog.verify_codes.IndexOf(":"));
                                    req.ConsumeNum = long.Parse(usenum.ToString());
                                    req.Token = sendnoticelog.token;
                                    req.CodemerchantId = long.Parse(tb_CodemerchantId);
                                    req.Posid = posid.ToString();
                                    //req.Mobile ="";
                                    //req.NewCode = "";
                                    req.SerialNum = selfdefine_serial_num;
                                    //req.QrImages = "";

                                    //设置请求和响应时间为10s
                                    WebUtils w = new WebUtils();
                                    w.Timeout = 10000;

                                    //核销回调操作
                                    VmarketEticketConsumeResponse response = client.Execute(req, tb_session);

                                    mretlog.ret_code = response.Body;
                                    tb_resultdesc = response.Body;// 核销回调申请 结果 描述
                                    new Taobao_consume_retlogData().Editcomsumeretlog(mretlog);

                                    #region 解析回调结果
                                    tb_xml = response.Body;
                                    XmlDocument doc = new XmlDocument();
                                    doc.LoadXml(response.Body);
                                    XmlElement root = doc.DocumentElement;

                                    if (root.SelectSingleNode("ret_code") != null)
                                    {
                                        string ret_code = root.SelectSingleNode("ret_code").InnerText;


                                        if (ret_code == "1")//回调成功，继续向下进入验证流程
                                        {
                                            mretlog.ret_code = ret_code;
                                            if (root.SelectSingleNode("ret_code") != null)
                                            {
                                                mretlog.item_title = root.SelectSingleNode("ret_code").InnerText;
                                            }
                                            if (root.SelectSingleNode("left_num") != null)
                                            {
                                                mretlog.left_num = int.Parse(root.SelectSingleNode("left_num").InnerText);
                                            }
                                            if (root.SelectSingleNode("sms_tpl") != null)
                                            {
                                                mretlog.sms_tpl = root.SelectSingleNode("sms_tpl").InnerText;
                                            }
                                            if (root.SelectSingleNode("print_tpl") != null)
                                            {
                                                mretlog.print_tpl = root.SelectSingleNode("print_tpl").InnerText;
                                            }
                                            if (root.SelectSingleNode("consume_secial_num") != null)
                                            {
                                                mretlog.consume_secial_num = root.SelectSingleNode("consume_secial_num").InnerText;
                                            }
                                            if (root.SelectSingleNode("code_left_num") != null)
                                            {
                                                mretlog.code_left_num = int.Parse(root.SelectSingleNode("code_left_num").InnerText);
                                            }
                                            new Taobao_consume_retlogData().Editcomsumeretlog(mretlog);
                                            isconsumesuc = true;
                                        }
                                        else
                                        {
                                            tb_xml = delayTime(1, sendretlog.order_id, sendretlog.verify_codes.Substring(0, sendretlog.verify_codes.IndexOf(":")), usenum, selfdefine_serial_num, sendnoticelog.token, posid.ToString());


                                            //var backstr_temp = ParamErr("请求错误，请重新验证");
                                            //return GetBackStr(backstr_temp, poslogid);
                                        }

                                    }
                                    else
                                    {

                                        string sub_code = root.SelectSingleNode("sub_code").InnerText;
                                        if (sub_code == "isp.top-remote-connection-timeout")
                                        {
                                            tb_xml = delayTime(1, sendretlog.order_id, sendretlog.verify_codes.Substring(0, sendretlog.verify_codes.IndexOf(":")), usenum, selfdefine_serial_num, sendnoticelog.token, posid.ToString());

                                        }


                                        //var backstr_temp = ParamErr("请求错误，请重新验证");
                                        //return GetBackStr(backstr_temp, poslogid);
                                    }
                                    #endregion

                                    #endregion
                                }
                                else
                                {
                                    //var backstr_temp = ParamErr("请求超时，请重新验证");
                                    //return GetBackStr(backstr_temp, poslogid);
                                }
                            }
                            catch (Exception ex)
                            {
                                //冲正
                                tb_xml = delayTime(1, sendretlog.order_id, sendretlog.verify_codes.Substring(0, sendretlog.verify_codes.IndexOf(":")), usenum, selfdefine_serial_num, sendnoticelog.token, posid.ToString());
                                bl = true;

                                //var backstr_temp = ParamErr("请求超时，请重新验证.");
                                //return GetBackStr(backstr_temp, poslogid);
                            }
                            //风险处理
                            if (string.IsNullOrEmpty(tb_xml) && !bl)
                            {
                                tb_xml = delayTime(1, sendretlog.order_id, sendretlog.verify_codes.Substring(0, sendretlog.verify_codes.IndexOf(":")), usenum, selfdefine_serial_num, sendnoticelog.token, posid.ToString());


                                //var backstr_temp = ParamErr("请求超时，请重新验证..");
                                //return GetBackStr(backstr_temp, poslogid);
                            }
                            if (isconsumesuc == false)
                            {
                                //var backstr_temp = ParamErr("请求超时，请重新验证.");
                                //return GetBackStr(backstr_temp, poslogid);

                                string tb_sub_code = "";//返回错误码
                                try
                                {
                                    XmlDocument tb_xdoc = new XmlDocument();
                                    tb_xdoc.LoadXml(tb_resultdesc);
                                    XmlElement tb_root = tb_xdoc.DocumentElement;
                                    tb_sub_code = tb_root.SelectSingleNode("sub_code").InnerText;
                                    string tb_sub_msg = tb_root.SelectSingleNode("sub_msg").InnerText;
                                    tb_resultdesc = tb_sub_code + ":" + tb_sub_msg;
                                }
                                catch
                                {
                                    tb_resultdesc = "";
                                }

                                //和星海商量，如果出现isv.eticket-order-not-found:invalid-orderid:找不到对应的电子凭证订单 ，放过
                                if (tb_sub_code.ToLower().Trim() == "isv.eticket-order-not-found:invalid-orderid")
                                {
                                }
                                else
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "淘宝验证接口有误(" + tb_resultdesc + ")" });
                                }


                            }
                        }
                        #endregion

                        #region 分销电子票
                        if (eticketinfo.Agent_id > 0)//分销商电子票,需要发送验证同步请求
                        {
                            //获得分销商信息
                            Agent_company agentinfo = new AgentCompanyData().GetAgentCompany(eticketinfo.Agent_id);
                            if (agentinfo != null)
                            {
                                #region  如果是分销出票，判断分销商为验证扣款时，分销商预付款+信用额度 不足时，不给验票，提示额度不足

                                //根据订单编号得到 分销授权类型
                                int Warrant_type = new B2bOrderData().GetWarrant_typeByOrderid(eticketinfo.Oid);
                                if (Warrant_type == 2)
                                {
                                    //获得分销商授权信息
                                    Agent_warrant model_agent_warrant = new AgentCompanyData().GetAgent_Warrant(eticketinfo.Agent_id, eticketinfo.Com_id);
                                    if (model_agent_warrant == null)
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = "分销商授权信息没有查到" });
                                    }
                                    else
                                    {
                                        #region 分销商预付款+信用额度 不足1000、500、0 时，给分销商短信通知
                                        decimal newestBalance = model_agent_warrant.Imprest + model_agent_warrant.Credit;
                                        if (newestBalance > agentinfo.maxremindmoney)
                                        { }
                                        else if (newestBalance > 500 && newestBalance <= agentinfo.maxremindmoney)
                                        {
                                            //分销额度不足 提醒通知短信
                                            Agent_RechargeRemindSms remindsms = new Agent_RechargeRemindSmsData().GetAgent_RechargeRemindSms(eticketinfo.Agent_id, eticketinfo.Com_id, agentinfo.maxremindmoney, 0);
                                            //如果额度不足通知短信已经发送过，并且没有充值处理，则不再发送；如果已经充值处理了，提单后额度仍然不足，则继续发送短信
                                            if (remindsms == null)
                                            {
                                                //额度不足提醒短信当天已经发送过则不再发送
                                                int remindnum = new Agent_RechargeRemindSmsData().GetAgent_NowdayRechargeRemindSmsNum(eticketinfo.Agent_id, eticketinfo.Com_id, agentinfo.maxremindmoney);
                                                if (remindnum == 0)
                                                {
                                                    string companyname = new B2bCompanyData().GetCompanyNameById(eticketinfo.Com_id);
                                                    string remindsmsstr = "额度提醒:你在商户 " + companyname + " 下的信用额度/预付款已经不足" + agentinfo.maxremindmoney + "元，请及时充值；";
                                                    string msg = "";
                                                    int sendback = SendSmsHelper.SendSms(agentinfo.Mobile, remindsmsstr, eticketinfo.Com_id, out msg);

                                                    Agent_RechargeRemindSms mremindsms = new Agent_RechargeRemindSms
                                                    {
                                                        id = 0,
                                                        agentid = eticketinfo.Agent_id,
                                                        comid = eticketinfo.Com_id,
                                                        smscontent = remindsmsstr,
                                                        tishi_edu = agentinfo.maxremindmoney.ToString(),
                                                        tishi_phone = agentinfo.Mobile,
                                                        tishi_time = DateTime.Now,
                                                        isdeal = 0
                                                    };
                                                    int insremindsms = new Agent_RechargeRemindSmsData().InsRemindsms(mremindsms);
                                                }
                                            }
                                        }
                                        else if (newestBalance > 0 && newestBalance <= 500)
                                        {
                                            //分销额度不足 提醒通知短信
                                            Agent_RechargeRemindSms remindsms = new Agent_RechargeRemindSmsData().GetAgent_RechargeRemindSms(eticketinfo.Agent_id, eticketinfo.Com_id, 500, 0);
                                            //如果额度不足通知短信已经发送过，并且没有充值处理，则不再发送；如果已经充值处理了，提单后额度仍然不足，则继续发送短信
                                            if (remindsms == null)
                                            {
                                                //额度不足提醒短信当天已经发送过则不再发送
                                                int remindnum = new Agent_RechargeRemindSmsData().GetAgent_NowdayRechargeRemindSmsNum(eticketinfo.Agent_id, eticketinfo.Com_id, 500);
                                                if (remindnum == 0)
                                                {
                                                    string companyname = new B2bCompanyData().GetCompanyNameById(eticketinfo.Com_id);
                                                    string remindsmsstr = "额度提醒:你在商户 " + companyname + " 下的信用额度/预付款已经不足500元，请及时充值；";
                                                    string msg = "";
                                                    int sendback = SendSmsHelper.SendSms(agentinfo.Mobile, remindsmsstr, eticketinfo.Com_id, out msg);

                                                    Agent_RechargeRemindSms mremindsms = new Agent_RechargeRemindSms
                                                    {
                                                        id = 0,
                                                        agentid = eticketinfo.Agent_id,
                                                        comid = eticketinfo.Com_id,
                                                        smscontent = remindsmsstr,
                                                        tishi_edu = "500",
                                                        tishi_phone = agentinfo.Mobile,
                                                        tishi_time = DateTime.Now,
                                                        isdeal = 0
                                                    };
                                                    int insremindsms = new Agent_RechargeRemindSmsData().InsRemindsms(mremindsms);
                                                }
                                            }
                                        }
                                        else //newestBalance < 0
                                        {
                                            //分销额度不足 提醒通知短信
                                            Agent_RechargeRemindSms remindsms = new Agent_RechargeRemindSmsData().GetAgent_RechargeRemindSms(eticketinfo.Agent_id, eticketinfo.Com_id, 0, 0);
                                            //如果额度不足通知短信已经发送过，并且没有充值处理，则不再发送；如果已经充值处理了，提单后额度仍然不足，则继续发送短信
                                            if (remindsms == null)
                                            {
                                                //额度不足提醒短信当天已经发送过则不再发送
                                                int remindnum = new Agent_RechargeRemindSmsData().GetAgent_NowdayRechargeRemindSmsNum(eticketinfo.Agent_id, eticketinfo.Com_id, 0);
                                                if (remindnum == 0)
                                                {
                                                    string companyname = new B2bCompanyData().GetCompanyNameById(eticketinfo.Com_id);
                                                    string remindsmsstr = "额度提醒:你在商户 " + companyname + " 下的信用额度/预付款已经不足0元，请及时充值；";
                                                    string msg = "";
                                                    int sendback = SendSmsHelper.SendSms(agentinfo.Mobile, remindsmsstr, eticketinfo.Com_id, out msg);

                                                    Agent_RechargeRemindSms mremindsms = new Agent_RechargeRemindSms
                                                    {
                                                        id = 0,
                                                        agentid = eticketinfo.Agent_id,
                                                        comid = eticketinfo.Com_id,
                                                        smscontent = remindsmsstr,
                                                        tishi_edu = "0",
                                                        tishi_phone = agentinfo.Mobile,
                                                        tishi_time = DateTime.Now,
                                                        isdeal = 0
                                                    };
                                                    int insremindsms = new Agent_RechargeRemindSmsData().InsRemindsms(mremindsms);
                                                }
                                            }
                                        }
                                        #endregion
                                        if (model_agent_warrant.Imprest + model_agent_warrant.Credit < 0)
                                        {
                                            return JsonConvert.SerializeObject(new { type = 1, msg = "分销商预付款不足，无法继续验票" });
                                        }
                                    }
                                }
                                #endregion

                                #region  主要作用 查询出 订单信息，分销信息， 产品分销商价格(如果分销商为 验证扣款，分销商价格为0时，则不可验证)
                                decimal now_agentproprice = 0;//产品 当前给分销的价格

                                if (eticketinfo.Oid == 0)
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "电子票表中 订单号 不存在" });
                                }

                                B2bOrderData orderdata = new B2bOrderData();
                                B2b_order ordermodel = orderdata.GetOrderById(eticketinfo.Oid);

                                Agent_company agentmodel = null;//在分销商为验证扣款 时，会改动分销的财务数据，才会需要求得分销商信息
                                if (ordermodel != null)
                                {
                                    //判断授权类型为 验证扣款 = 2
                                    if (ordermodel.Warrant_type == 2)
                                    {
                                        agentmodel = AgentCompanyData.GetAgentCompanyByUid(ordermodel.Warrantid);// 根据授权id得到分销商基本信息
                                        if (agentmodel != null)
                                        {
                                            #region  20141216经星海和李经理电话交流，最终李经理要求只要 产品设置 分销商销售价格为0，则导出的码 不可以再验证




                                            if (ordermodel.Speciid == 0)
                                            {
                                                if (agentmodel.Warrant_level == 1)//1级分销
                                                {
                                                    now_agentproprice = pro.Agent1_price;
                                                }
                                                if (agentmodel.Warrant_level == 2)//2级分销
                                                {
                                                    now_agentproprice = pro.Agent2_price;
                                                }
                                                if (agentmodel.Warrant_level == 3)//3级分销
                                                {
                                                    now_agentproprice = pro.Agent3_price;
                                                }
                                            }
                                            else
                                            {
                                                //如果含规格 ，按规格读取价格
                                                B2b_com_pro prospeciid = new B2bComProData().GetProspeciidById(ordermodel.Pro_id.ToString(), ordermodel.Speciid);
                                                if (prospeciid != null)
                                                {
                                                    if (agentmodel.Warrant_level == 1)//1级分销
                                                    {
                                                        now_agentproprice = prospeciid.Agent1_price;
                                                    }
                                                    if (agentmodel.Warrant_level == 2)//2级分销
                                                    {
                                                        now_agentproprice = prospeciid.Agent2_price;
                                                    }
                                                    if (agentmodel.Warrant_level == 3)//3级分销
                                                    {
                                                        now_agentproprice = prospeciid.Agent3_price;
                                                    }
                                                }
                                                else
                                                {
                                                    now_agentproprice = ordermodel.Pay_price;//如果未查询出规格价格，按订单的价格扣款
                                                }
                                            }

                                            if (now_agentproprice == 0)
                                            {
                                                return JsonConvert.SerializeObject(new { type = 1, msg = "当前产品的分销价格为0，分销商验证扣款电子票不可用" });
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            return JsonConvert.SerializeObject(new { type = 1, msg = " 根据授权id得到分销商基本信息 失败" });
                                        }
                                    }
                                }
                                else
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "根据电子票表中 订单号 获得 订单信息 失败" });
                                }
                                #endregion


                                //agent_queryurl,agent_updateurl 糯米接口中会用到;agent_queryurl agent_Inter_deskey 一般分销接口中会用到
                                string agent_queryurl = agentinfo.Agent_queryurl;
                                string agent_updateurl = agentinfo.Agent_updateurl;
                                string agent_Inter_deskey = agentinfo.Inter_deskey;

                                int Agent_type = agentinfo.Agent_type;//分销类型
                                string Agent_auth_username = agentinfo.Agent_auth_username;//百度糯米用户名
                                string Agent_auth_token = agentinfo.Agent_auth_token;//百度糯米token

                                int a_comid = eticketinfo.Com_id;//a订单的公司id
                                int a_orderid = eticketinfo.Oid;//a订单的订单id
                                int a_agentid = eticketinfo.Agent_id;//a订单的分销id

                                //a订单:原分销向指定商户提交的订单;b订单:指定商户下的绑定分销向拥有产品的商户提交的订单
                                //电子票表中记录的Oid是b订单
                                //判断b订单 是否 属于某个a订单 
                                B2b_order loldorder = orderdata.GetOldorderBybindingId(eticketinfo.Oid);
                                if (loldorder != null)
                                {
                                    //得到a订单的分销信息
                                    if (loldorder.Agentid > 0)
                                    {
                                        Agent_company acompany = new AgentCompanyData().GetAgentCompany(loldorder.Agentid);
                                        if (acompany != null)
                                        {
                                            agent_queryurl = acompany.Agent_queryurl;
                                            agent_updateurl = acompany.Agent_updateurl;
                                            agent_Inter_deskey = acompany.Inter_deskey;

                                            Agent_type = agentinfo.Agent_type;//分销类型
                                            Agent_auth_username = agentinfo.Agent_auth_username;//百度糯米用户名
                                            Agent_auth_token = agentinfo.Agent_auth_token;//百度糯米token

                                            a_comid = loldorder.Comid;
                                            a_orderid = loldorder.Id;
                                            a_agentid = loldorder.Agentid;
                                        }
                                        else
                                        {
                                            return JsonConvert.SerializeObject(new { type = 1, msg = "分销信息查询出错" });
                                        }
                                    }
                                    else
                                    {
                                        agent_queryurl = "";
                                        agent_updateurl = "";
                                        agent_Inter_deskey = "";
                                        Agent_type = 0;//分销类型
                                        Agent_auth_username = "";//百度糯米用户名
                                        Agent_auth_token = "";//百度糯米token

                                        a_comid = loldorder.Comid;
                                        a_orderid = loldorder.Id;
                                        a_agentid = 0;
                                    }

                                }


                                #region 糯米分销产品
                                if (Agent_type == (int)AgentCompanyType.NuoMi)//糯米分销
                                {
                                    //查询站外码状态
                                    string username = Agent_auth_username;//百度糯米用户名
                                    string token = Agent_auth_token;//百度糯米token
                                    string code = eticketinfo.Pno;//券码
                                    //供应商名称:每个供应商分别对应一个供应商名称(平台总账户设置)
                                    B2b_company supplier = new B2bCompanyData().GetCompanyBasicById(a_comid);
                                    if (supplier == null)
                                    {

                                        reason = "获取供应商信息失败";
                                        elog.A_remark = reason;
                                        elogdata.InsertOrUpdateLog(elog);
                                        return JsonConvert.SerializeObject(new { type = 1, msg = reason });
                                    }
                                    if (supplier.Agent_nuomi_bindcomname == "")
                                    {
                                        reason = "供应商没有绑定糯米数据";
                                        elog.A_remark = reason;
                                        elogdata.InsertOrUpdateLog(elog);
                                        return JsonConvert.SerializeObject(new { type = 1, msg = reason });
                                    }



                                    string bindcomname = supplier.Agent_nuomi_bindcomname;


                                    string nuomi_dealid = new B2bOrderData().GetNuomi_dealid(a_orderid);

                                    //if(nuomi_dealid=="")
                                    //{
                                    //    nuomi_dealid = pro.nuomi_dealid;
                                    //}
                                    string queryurl = agent_queryurl + "?auth={\"userName\":\"" + username + "\",\"token\":\"" + token + "\"}&code=" + code + "&userName=" + bindcomname + "&dealId=" + nuomi_dealid + "&phone=";
                                    string updateurl = agent_updateurl + "?auth={\"userName\":\"" + username + "\",\"token\":\"" + token + "\"}&code=" + code + "&userName=" + bindcomname + "&dealId=" + nuomi_dealid + "&phone=";

                                    string re1 = new GetUrlData().HttpPost(queryurl, "");
                                    if (re1 != "")
                                    {
                                        //返回格式:{"info":"success","res":{"status":1,"dealId":738736,"useTime":1400221668,"expireTime":253402271999,"code":"00289a5f7be6"},"code":0}
                                        XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + re1 + "}");
                                        //格式:<root>
                                        //       <info>success</info>
                                        //       <res>
                                        //         <status>1</status>   //status:(0:未消费,1:已消费,-2:已退款,-3:不存在)
                                        //         <dealId>738736</dealId>
                                        //         <useTime>1400221668</useTime>
                                        //         <expireTime>253402271999</expireTime>
                                        //         <code>00289a5f7be6</code>
                                        //       </res>
                                        //       <code>0</code>
                                        //    </root>
                                        XmlElement root = doc.DocumentElement;
                                        string info = root.SelectSingleNode("info").InnerText;
                                        if (info == "success")
                                        {
                                            string status = root.SelectSingleNode("res/status").InnerText;//劵码状态
                                            if (status == "0")
                                            {
                                                //电子票验证
                                                var returnval = Edata.InsertOrUpdate(eticketinfo);

                                                var validateticketlogid = 0;//电子票验票日志id
                                                if (returnval > 0)
                                                {

                                                    reason = "电子码验证成功";
                                                    if (surplusnum > 0)
                                                    {
                                                        reason += " 验证部分";
                                                    }
                                                    else
                                                    {
                                                        reason += " 验证全部";
                                                    }
                                                    elog.A_remark = reason;
                                                    elog.A_state = (int)ECodeOperStatus.OperSuc;
                                                    elog.Use_pnum = int.Parse(usenum);
                                                    validateticketlogid = elogdata.InsertOrUpdateLog(elog);

                                                    //异步发送验证同步请求
                                                    AsyncsendEventHandler mydelegate = new AsyncsendEventHandler(AsyncSend);
                                                    mydelegate.BeginInvoke(updateurl, code, int.Parse(usenum), elog.Actiondate.ToString(), a_agentid, a_comid, validateticketlogid, new AsyncCallback(Completed), null);


                                                    //如果是验码扣款，进行分销扣款,先判断是否有订单号
                                                    if (eticketinfo.Oid != 0)
                                                    {
                                                        if (ordermodel != null)
                                                        {
                                                            //对分销的销售扣款，订单更改状态
                                                            if (ordermodel.Warrant_type == 1)
                                                            {
                                                                ordermodel.Order_state = (int)OrderStatus.HasUsed;
                                                                new B2bOrderData().InsertOrUpdate(ordermodel);
                                                            }


                                                            //判断授权类型为 验证扣款 = 2
                                                            if (ordermodel.Warrant_type == 2)
                                                            {

                                                                decimal overmoney = 0;
                                                                if (agentmodel != null)
                                                                {

                                                                    overmoney = agentmodel.Imprest - now_agentproprice * int.Parse(usenum);
                                                                    //分销商财务扣款
                                                                    Agent_Financial Financialinfo = new Agent_Financial
                                                                    {
                                                                        Id = 0,
                                                                        Com_id = eticketinfo.Com_id,
                                                                        Agentid = ordermodel.Agentid,
                                                                        Warrantid = ordermodel.Warrantid,
                                                                        Order_id = ordermodel.Id,
                                                                        Servicesname = pro.Pro_name + "[" + eticketinfo.Oid + "]",
                                                                        SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                                                                        Money = 0 - now_agentproprice * int.Parse(usenum),
                                                                        Payment = 1,            //收支(0=收款,1=支出)
                                                                        Payment_type = "验票扣款",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点,分销扣款）
                                                                        Money_come = "分销账户",         //资金来源（网上支付,银行收款，分销账户等）
                                                                        Over_money = overmoney
                                                                    };
                                                                    var fin = AgentCompanyData.AgentFinancial(Financialinfo);
                                                                }
                                                                //处理如果是导入产品，原订单的也是分销订单并且是验证扣款，财务处理
                                                                var oldor = AgentImprestPro(ordermodel.Id, int.Parse(usenum));
                                                            }
                                                        }
                                                    }


                                                }
                                                return JsonConvert.SerializeObject(new { type = 100, msg = validateticketlogid });


                                            }
                                            else if (status == "1")
                                            {

                                                reason = "糯米电子码已消费";
                                                return JsonConvert.SerializeObject(new { type = 1, msg = reason });
                                            }
                                            else if (status == "-2")
                                            {
                                                reason = "电子码已退款";
                                                return JsonConvert.SerializeObject(new { type = 1, msg = reason });
                                            }
                                            else if (status == "-3")
                                            {
                                                reason = "电子码不存在";
                                                return JsonConvert.SerializeObject(new { type = 1, msg = reason });
                                            }
                                            else
                                            {
                                                reason = "电子码状态有误";
                                                return JsonConvert.SerializeObject(new { type = 1, msg = reason });
                                            }
                                        }
                                        else//查询站外码状态出错
                                        {
                                            reason = info;
                                            return JsonConvert.SerializeObject(new { type = 1, msg = reason });
                                        }
                                    }
                                    else //查询站外码状态 返回内容为空
                                    {
                                        reason = "查询站外码状态有误";
                                        return JsonConvert.SerializeObject(new { type = 1, msg = reason });
                                    }


                                }
                                #endregion
                                #region 一般分销
                                else
                                {
                                    var returnval = Edata.InsertOrUpdate(eticketinfo);

                                    var validateticketlogid = 0;//电子票验票日志id
                                    if (returnval > 0)
                                    {



                                        reason = "电子码验证成功";
                                        if (surplusnum > 0)
                                        {
                                            reason += " 验证部分";
                                        }
                                        else
                                        {
                                            reason += " 验证全部";
                                        }
                                        elog.A_remark = reason;
                                        elog.A_state = (int)ECodeOperStatus.OperSuc;
                                        elog.Use_pnum = int.Parse(usenum);
                                        validateticketlogid = elogdata.InsertOrUpdateLog(elog);


                                        if (agent_updateurl.Trim() != "" && agent_Inter_deskey != "")
                                        {
                                            //异步发送验证同步请求
                                            AsyncsendEventHandler mydelegate = new AsyncsendEventHandler(AsyncSend);
                                            mydelegate.BeginInvoke(agent_updateurl.Trim(), pno, int.Parse(usenum), elog.Actiondate.ToString(), a_agentid, a_comid, validateticketlogid, new AsyncCallback(Completed), null);
                                        }

                                        #region 如果是美团分销，则向美团发送验证通知
                                        Agent_company mtagentcompany = new AgentCompanyData().GetAgentCompany(a_agentid);
                                        if (mtagentcompany != null)
                                        {
                                            if (mtagentcompany.mt_partnerId != "")
                                            {

                                                Meituan_reqlog meituanlogg = new Meituan_reqlogData().GetMt_OrderpayreqlogBySelforderid(a_orderid, "200");
                                                if (meituanlogg == null)
                                                {
                                                    //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\MTlog.txt", "美团验证通知：查询美团支付成功订单失败");
                                                    LogHelper.RecordSelfLog("Erro", "meituan", "美团验证通知发送失败：查询美团支付成功订单失败");
                                                }
                                                else
                                                {
                                                    #region  订单电子票使用情况
                                                    string all_pno = "";//全部电子码
                                                    string keyong_pno = "";//可用电子码 
                                                    int buy_num = 0;
                                                    int keyong_num = 0;
                                                    int consume_num = 0;
                                                    int tuipiao_num = ordermodel.Cancelnum;
                                                    if (loldorder != null)
                                                    {
                                                        tuipiao_num = loldorder.Cancelnum;
                                                    }
                                                    //根据订单号得到电子票信息
                                                    List<B2b_eticket> meticketlist = new B2bEticketData().GetEticketListByOrderid(eticketinfo.Oid);
                                                    if (meticketlist == null)
                                                    {
                                                        LogHelper.RecordSelfLog("Error", "meituan", "根据订单号查询电子票信息失败");
                                                    }
                                                    else
                                                    {
                                                        if (meticketlist.Count == 0)
                                                        {
                                                            LogHelper.RecordSelfLog("Error", "meituan", "根据订单号查询电子票信息失败");
                                                        }
                                                        else
                                                        {
                                                            foreach (B2b_eticket meticket in meticketlist)
                                                            {
                                                                if (meticket != null)
                                                                {
                                                                    buy_num += meticket.Pnum;
                                                                    keyong_num += meticket.Use_pnum;
                                                                    consume_num += meticket.Pnum - meticket.Use_pnum;
                                                                    all_pno += meticket.Pno + ",";
                                                                    if (meticket.Use_pnum > 0)
                                                                    {
                                                                        keyong_pno += meticket.Pno + ",";
                                                                    }

                                                                }
                                                            }
                                                            if (all_pno.Length > 0)
                                                            {
                                                                all_pno = all_pno.Substring(0, all_pno.Length - 1);
                                                            }
                                                            if (keyong_pno.Length > 0)
                                                            {
                                                                keyong_pno = keyong_pno.Substring(0, keyong_pno.Length - 1);
                                                            }
                                                        }
                                                    }
                                                    #endregion

                                                    //int meituan_orderstate = (int)Meituan_orderStatus.AllUsed;
                                                    //if (keyong_num > 0)
                                                    //{
                                                    //    meituan_orderstate = (int)Meituan_orderStatus.PartUsed;
                                                    //}
                                                    //根据订单编号得到美团订单编号 
                                                    string meituancontent = "{" +
                                                    "\"partnerId\": " + mtagentcompany.mt_partnerId + "," +
                                                    "\"body\": " +
                                                    "{" +
                                                        //"\"bookOrderId\": \"" + meituanlogg.mtorderid + "\"," +
                                                        //"\"partnerOrderId\": \"" + a_orderid + "\"," +
                                                        //"\"orderStatus\": " + meituan_orderstate + "," +
                                                        //"\"orderQuantity\": " + buy_num + "," +
                                                        //"\"usedQuantity\": " + (consume_num - ordermodel.Cancelnum) + "," +
                                                        //"\"refundQuantity\": " + tuipiao_num + "," +
                                                        //"\"isConsumed\": true," +
                                                        //"\"voucher\": \"" + pno + "\"" +

                                                        "\"orderId\": " + meituanlogg.mtorderid + "," +
                                                        "\"partnerOrderId\": \"" + a_orderid + "\"," +
                                                        "\"quantity\": " + buy_num + "," +
                                                        "\"usedQuantity\": " + (consume_num - ordermodel.Cancelnum) + "," +
                                                        "\"refundedQuantity\": " + tuipiao_num + "," +
                                                        "\"voucherList\": " +
                                                                         "[{" +
                                                                             "\"voucher\":\"" + pno + "\"," +
                                                                             "\"voucherPics\":\"\"," +
                                                                             "\"voucherInvalidTime\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\"," +
                                                                             "\"quantity\":" + usenum + "," +
                                                                             "\"status\":1" +//4.10 凭证码状态:0	未使用;1	已使用;2	已退款;3	已废弃 对应的门票还未消费，但是此凭证码作废了
                                                                         "}]" +
                                                    "}" +
                                                "}";


                                                    OrderConsumeNotice mrequest = (OrderConsumeNotice)JsonConvert.DeserializeObject(meituancontent, typeof(OrderConsumeNotice));

                                                    ReturnResult r = new MeiTuanInter(mtagentcompany.mt_partnerId, mtagentcompany.mt_secret, mtagentcompany.mt_client).ConsumeNotify(mrequest, mtagentcompany.Id);
                                                }

                                            }
                                        } 
                                        #endregion


                                        #region 如果是驴妈妈分销，则向驴妈妈发送验证通知

                                        if (agentinfo.Agent_type == (int)AgentCompanyType.Lvmama)
                                        {

                                            
                                            Lvmama_reqlog LvmamaOrderCrateSucLog = new lvmama_reqlogData().GetLvmama_OrderpayreqlogBySelforderid(a_orderid, "0");
                                            
                                            if (LvmamaOrderCrateSucLog != null)
                                            {

                                                B2b_order morder = new B2bOrderData().GetOrderById(a_orderid);
                                                if (morder != null)
                                                {


                                                    string state = "1";
                                                    if (morder.U_num > int.Parse(usenum))
                                                    {
                                                        state = "2";
                                                    }
                                                    else if (int.Parse(usenum) == morder.U_num)
                                                    {
                                                        state = "3";
                                                    }
                                                    else
                                                    {
                                                        state = "1";
                                                    }

                                                    var lvmamadata = new LVMAMA_Data(agentinfo.Lvmama_uid, agentinfo.Lvmama_password, agentinfo.Lvmama_Apikey);
                                                    //初始的时候没有sign值，等组合后下面生成加密文件
                                                    var hexiaojson = lvmamadata.usedticketscallback_json(LvmamaOrderCrateSucLog.mtorderid, agentinfo.Lvmama_uid, agentinfo.Lvmama_password, state, "", DateTime.Now.ToString("yyyyMMddHHmmss"), usenum);

                                                    #region 签名验证
                                                    string Md5Sign = lvmamadata.usedticketscallbackmd5(hexiaojson);
                                                    string afterSign = lvmamadata.lumamasign(Md5Sign, agentinfo.Lvmama_Apikey);
                                                    #endregion
                                                    hexiaojson.sign = afterSign;
                                                    try
                                                    {
                                                        var asynnoticecall = lvmamadata.useConsumeNotify(hexiaojson, agentinfo.Id);
                                                    }catch (Exception ex)
                                                    {
                                                        //暂时不做任何处理，当系统自己的票，给美团和驴妈妈发同步验证申请，接受错误返回只会增加时长
                                                    }
                                                }
                                            }
                                        }
                                        #endregion


                                        if (eticketinfo.Oid != 0)
                                        {
                                            if (ordermodel != null)
                                            {
                                                //对订单更改状态

                                                ordermodel.Order_state = (int)OrderStatus.HasUsed;
                                                new B2bOrderData().InsertOrUpdate(ordermodel);

                                                ////判断是否有订单 绑定传入的订单
                                                //B2b_order d_loldorder = new B2bOrderData().GetOldorderBybindingId(eticketinfo.Oid);
                                                if (loldorder != null)
                                                {
                                                    if (loldorder.qunar_orderid != "")
                                                    {
                                                        //异步发送验证同步请求--去哪 
                                                        AsyncsendEventHandler_qunar mydelegate = new AsyncsendEventHandler_qunar(AsyncSend_qunar);
                                                        mydelegate.BeginInvoke(ordermodel, new AsyncCallback(Completed_qunar), null);
                                                    }
                                                }

                                                //如果是验码扣款，进行分销扣款,先判断是否有订单号
                                                //判断授权类型为 验证扣款 = 2
                                                if (ordermodel.Warrant_type == 2)
                                                {
                                                    decimal overmoney = 0;
                                                    if (agentmodel != null)
                                                    {

                                                        overmoney = agentmodel.Imprest - now_agentproprice * int.Parse(usenum);
                                                        //分销商财务扣款
                                                        Agent_Financial Financialinfo = new Agent_Financial
                                                        {
                                                            Id = 0,
                                                            Com_id = eticketinfo.Com_id,
                                                            Agentid = ordermodel.Agentid,
                                                            Warrantid = ordermodel.Warrantid,
                                                            Order_id = ordermodel.Id,
                                                            Servicesname = pro.Pro_name + "[" + eticketinfo.Oid + "]",
                                                            SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                                                            Money = 0 - now_agentproprice * int.Parse(usenum),
                                                            Payment = 1,            //收支(0=收款,1=支出)
                                                            Payment_type = "验票扣款",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点,分销扣款）
                                                            Money_come = "分销账户",         //资金来源（网上支付,银行收款，分销账户等）
                                                            Over_money = overmoney
                                                        };
                                                        var fin = AgentCompanyData.AgentFinancial(Financialinfo);

                                                        //扣除商户分销订单手续费,验证扣款订单，每次 扣分销款的时候对商户 收取手续费
                                                        var KouchuShouxufei_temp = OrderJsonData.KouchuShouxufei(ordermodel);

                                                    }
                                                    //处理如果是导入产品，原订单的也是分销订单并且是验证扣款，财务处理
                                                    var oldor = AgentImprestPro(ordermodel.Id, int.Parse(usenum));
                                                }

                                            }
                                        }

                                    }
                                    return JsonConvert.SerializeObject(new { type = 100, msg = validateticketlogid });
                                }
                                #endregion

                            }
                            else
                            {
                                reason = "分销商信息为空";
                                return JsonConvert.SerializeObject(new { type = 1, msg = reason });
                            }
                        }
                        #endregion
                        #region 非分销商电子票，无需发送验证同步请求
                        else
                        {
                            var returnval = Edata.InsertOrUpdate(eticketinfo);

                            var validateticketlogid = 0;//电子票验票日志id
                            if (returnval > 0)
                            {

                                reason = "电子码验证成功";
                                if (surplusnum > 0)
                                {
                                    reason += " 验证部分";
                                }
                                else
                                {
                                    reason += " 验证全部";
                                }
                                elog.A_remark = reason;
                                elog.A_state = (int)ECodeOperStatus.OperSuc;
                                elog.Use_pnum = int.Parse(usenum);
                                validateticketlogid = elogdata.InsertOrUpdateLog(elog);

                                #region 直销不用分销付款
                                //如果是验码扣款，进行分销扣款,先判断是否有订单号
                                if (eticketinfo.Oid != 0)
                                {
                                    //查询订单
                                    B2bOrderData orderdata = new B2bOrderData();
                                    B2b_order ordermodel = orderdata.GetOrderById(eticketinfo.Oid);
                                    if (ordermodel != null)
                                    {
                                        //对直销订单，订单更改状态

                                        ordermodel.Order_state = (int)OrderStatus.HasUsed;
                                        new B2bOrderData().InsertOrUpdate(ordermodel);

                                        //异步发送验证同步请求--去哪
                                        if (ordermodel.qunar_orderid != "")
                                        {
                                            AsyncsendEventHandler_qunar mydelegate = new AsyncsendEventHandler_qunar(AsyncSend_qunar);
                                            mydelegate.BeginInvoke(ordermodel, new AsyncCallback(Completed_qunar), null);
                                        }

                                        #region 注释部分
                                        //        //判断授权类型为 验证扣款 = 2
                                        //        if (ordermodel.Warrant_type == 2)
                                        //        {

                                        //            decimal overmoney = 0;
                                        //            Agent_company agentmodel = AgentCompanyData.GetAgentCompanyByUid(ordermodel.Warrantid);
                                        //            if (agentmodel != null)
                                        //            {

                                        //                overmoney = agentmodel.Imprest - ordermodel.Pay_price * int.Parse(usenum);
                                        //                //分销商财务扣款
                                        //                Agent_Financial Financialinfo = new Agent_Financial
                                        //                {
                                        //                    Id = 0,
                                        //                    Com_id = eticketinfo.Com_id,
                                        //                    Agentid = ordermodel.Agentid,
                                        //                    Warrantid = ordermodel.Warrantid,
                                        //                    Order_id = ordermodel.Id,
                                        //                    Servicesname = pro.Pro_name + "[" + eticketinfo.Oid + "]",
                                        //                    SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                                        //                    Money = 0 - ordermodel.Pay_price * int.Parse(usenum),
                                        //                    Payment = 1,            //收支(0=收款,1=支出)
                                        //                    Payment_type = "验票扣款",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点,分销扣款）
                                        //                    Money_come = "分销账户",         //资金来源（网上支付,银行收款，分销账户等）
                                        //                    Over_money = overmoney
                                        //                };
                                        //                var fin = AgentCompanyData.AgentFinancial(Financialinfo);
                                        //            }
                                        //        }
                                        #endregion
                                    }
                                }
                                #endregion
                            }

                            //异步发送验证同步请求--微信验证通知
                            AsyncsendEventHandler_weixinyanzheng wxmydelegate = new AsyncsendEventHandler_weixinyanzheng(SendEticketData.SendWeixinYanzhengMsg);
                            wxmydelegate.BeginInvoke(eticketinfo.Oid, int.Parse(usenum), new AsyncCallback(Completed_weixinyanzheng), null);

                            ////发送通知
                            // SendEticketData.SendWeixinYanzhengMsg(eticketinfo.Oid, int.Parse(usenum));


                            return JsonConvert.SerializeObject(new { type = 100, msg = validateticketlogid });
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "验卡意外错误" });
                        throw;
                    }
                }
            }
        }
        public static void Completed_weixinyanzheng(IAsyncResult result)
        {
            //获取委托对象，调用EndInvoke方法获取运行结果
            AsyncResult _result = (AsyncResult)result;
            AsyncsendEventHandler_weixinyanzheng myDelegate = (AsyncsendEventHandler_weixinyanzheng)_result.AsyncDelegate;
            myDelegate.EndInvoke(_result);
        }

        private static string GetQunarRequestParam(string partnerorderId, string orderQuantity, string useQuantity, string consumeInfo, int comid, out int qunar_requestid, out int rlogid)
        {


            //根据公司id得到对应的去哪信息
            B2b_company company_qunar = new B2bCompanyData().Getqunarbycomid(comid);
            if (company_qunar != null)
            {
                if (company_qunar.isqunar != 0 && company_qunar.qunar_pass != "" && company_qunar.qunar_username != "")
                {
                    string frombase64requestxml = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                                           "<request xsi:schemaLocation=\"http://piao.qunar.com/2013/QMenpiaoRequestSchema QMRequestDataSchema-2.0.1.xsd\" xmlns=\"http://piao.qunar.com/2013/QMenpiaoRequestSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                                                               "<header>" +
                                                                   "<application>Qunar.Menpiao.Agent</application>" +
                                                                   "<processor>SupplierDataExchangeProcessor</processor>" +
                                                                   "<version>v2.0.1</version>" +
                                                                   "<bodyType>NoticeOrderConsumedRequestBody</bodyType>" +
                                                                   "<createUser>SupplierSystemName</createUser>" +
                                                                   "<createTime>{0}</createTime>" +
                                                                   "<supplierIdentity>{1}</supplierIdentity>" +
                                                               "</header>" +
                                                               "<body xsi:type=\"NoticeOrderConsumedRequestBody\">" +
                                                                   "<orderInfo>" +
                                                                       "<partnerorderId>{2}</partnerorderId>" +
                                                                       "<orderQuantity>{3}</orderQuantity>" +
                                                                       "<useQuantity>{4}</useQuantity>" +
                                                                       "<consumeInfo>{5}</consumeInfo>" +
                                                                   "</orderInfo>" +
                                                               "</body>" +
                                                           "</request>", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), company_qunar.qunar_username, partnerorderId, orderQuantity, useQuantity, consumeInfo);
                    string requestxml = EncryptionExtention.ToBase64(frombase64requestxml);
                    string signkey = company_qunar.qunar_pass;

                    string requestsecurityType = "MD5";
                    string requestsigned = EncryptionHelper.ToMD5(signkey + requestxml, "utf-8");

                    string requestParam = "{\"data\":\"" + requestxml + "\",\"signed\":\"" + requestsigned + "\",\"securityType\":\"" + requestsecurityType + "\"}";

                    //获得去哪订单
                    string qunar_orderid = new Qunar_CreateOrderForBeforePaySyncData().GetQunarOrderId(partnerorderId);

                    Qunar_ms_requestlog rlog = new Qunar_ms_requestlog
                    {
                        id = 0,
                        method = "noticeOrderConsumed",
                        requestParam = requestParam,
                        base64data = requestxml,
                        securityType = requestsecurityType,
                        signed = requestsigned,
                        frombase64data = frombase64requestxml,
                        bodyType = "NoticeOrderConsumedRequestBody",
                        createUser = "SupplierSystemName",
                        supplierIdentity = "supplierIdentity",
                        createTime = DateTime.Now,
                        qunar_orderId = qunar_orderid,
                        msg = ""
                    };
                    rlogid = new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                    if (rlogid == 0)
                    {
                        TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "向去哪发送消费通知(orderid:" + partnerorderId + "),录入请求记录(rlogid)出错");
                    }
                    //录入请求数据
                    int requestid = new Qunar_noticeOrderConsumedData().InsRequest(partnerorderId, orderQuantity, useQuantity, consumeInfo);
                    qunar_requestid = requestid;
                    if (requestid == 0)
                    {
                        TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "向去哪发送消费通知(orderid:" + partnerorderId + "),录入请求数据(requestid)出错");
                    }

                    return requestParam;
                }
                else
                {
                    TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "向去哪发送消费通知(orderid:" + partnerorderId + "),去哪配置信息不完整");
                }
            }
            else
            {
                TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "向去哪发送消费通知(orderid:" + partnerorderId + "),根据comid得到去哪信息出错");
            }


            qunar_requestid = 0;
            rlogid = 0;
            return "";

        }


        #region 淘宝冲正申请回调
        public static string delayTime(double secend, string order_id, string verify_code, string num, string selfdefine_serial_num, string token, string pos_id)
        {
            DateTime tempTime = DateTime.Now;
            string xml = string.Empty;
            //强制延时操作
            while (tempTime.AddSeconds(secend).CompareTo(DateTime.Now) > 0)
            {
                xml = string.Empty;
            }
            try
            {
                #region 对淘宝冲正申请回调 进行记录
                Taobao_reverse_retlog reverseretlog = new Taobao_reverse_retlog
                {
                    id = 0,
                    order_id = order_id,
                    reverse_code = verify_code,
                    reverse_num = int.Parse(num),
                    consume_secial_num = selfdefine_serial_num,
                    verify_codes = "",
                    qr_images = "",
                    token = token,
                    codemerchant_id = tb_CodemerchantId,
                    posid = pos_id,
                    ret_code = "",
                    item_title = "",
                    left_num = 0,
                    ret_time = DateTime.Now
                };
                int logid = new Taobao_reverse_retlogData().Editretlog(reverseretlog);
                reverseretlog.id = logid;
                #endregion

                #region 淘宝冲正申请回调
                string url = tb_returl;
                ITopClient client = new DefaultTopClient(url, tb_appkey, tb_appsecret);
                VmarketEticketReverseRequest req = new VmarketEticketReverseRequest();
                req.OrderId = long.Parse(order_id);
                req.ReverseCode = verify_code;
                req.ReverseNum = long.Parse(num);
                req.ConsumeSecialNum = selfdefine_serial_num;
                req.VerifyCodes = "";
                req.QrImages = "";
                req.Token = token;
                req.CodemerchantId = long.Parse(tb_CodemerchantId);
                req.Posid = pos_id;

                WebUtils w = new WebUtils();
                w.Timeout = 10000;

                VmarketEticketReverseResponse response = client.Execute(req, tb_session);

                xml = response.Body;

                reverseretlog.ret_code = response.Body;
                new Taobao_reverse_retlogData().Editretlog(reverseretlog);


                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response.Body);
                XmlElement root = doc.DocumentElement;
                if (root.SelectSingleNode("ret_code") != null)
                {
                    string ret_code = root.SelectSingleNode("ret_code").InnerText;

                    if (ret_code == "1")
                    {
                        reverseretlog.ret_code = ret_code;
                        if (root.SelectSingleNode("item_title") != null)
                        {
                            reverseretlog.item_title = root.SelectSingleNode("item_title").InnerText;
                        }
                        if (root.SelectSingleNode("left_num") != null)
                        {
                            reverseretlog.left_num = int.Parse(root.SelectSingleNode("left_num").InnerText);
                        }
                        new Taobao_reverse_retlogData().Editretlog(reverseretlog);
                    }
                    else
                    {
                        //???(暂时没有做)如果冲正接口调用异常，需要重试，直到冲正成功为止
                        xml = null;
                    }
                }
                else
                {

                    string sub_code = root.SelectSingleNode("sub_code").InnerText;
                    if (sub_code == "isv.eticket-reverse-error:consume-serial-num-not-found")
                    {//不在操作
                    }
                    else
                    {
                        //???(暂时没有做)如果冲正接口调用异常，需要重试，直到冲正成功为止
                        xml = null;
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                return null;
            }
            return xml;
        }
        #endregion



        #region 判断是否为分销导入产品，对原订单分销（如果是也是验证扣款则进行扣款）
        public static int AgentImprestPro(int orderid, int usenum)
        {
            try
            {
                //查询电子票
                var orderdata = new B2bOrderData();

                var orderid_old = orderdata.GetIdOrderBybindingId(orderid);

                //确认获取到了原订单ID,否则则无原订单
                if (orderid_old != orderid)
                {
                    var orderinfo = orderdata.GetOrderById(orderid_old);
                    if (orderinfo != null)
                    {
                        //只有分销订单才需要财务扣减
                        if (orderinfo.Agentid != 0)
                        {
                            //判断只有 验证扣款 才需要扣减，非验证扣款不用处理
                            if (orderinfo.Warrant_type == 2)
                            {
                                var prodata = new B2bComProData();
                                var pro = prodata.GetProById(orderinfo.Pro_id.ToString());
                                if (pro == null)
                                {
                                    return 0;
                                }

                                var agentmodel = AgentCompanyData.GetAgentByid(orderinfo.Agentid);
                                if (agentmodel == null)
                                {
                                    return 0;
                                }

                                decimal overmoney = 0;
                                if (agentmodel != null)
                                {

                                    overmoney = agentmodel.Imprest - orderinfo.Pay_price * usenum;
                                    //分销商财务扣款
                                    Agent_Financial Financialinfo = new Agent_Financial
                                    {
                                        Id = 0,
                                        Com_id = orderinfo.Comid,
                                        Agentid = orderinfo.Agentid,
                                        Warrantid = orderinfo.Warrantid,
                                        Order_id = orderid_old,
                                        Servicesname = pro.Pro_name + "[" + orderid_old + "]",
                                        SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                                        Money = 0 - orderinfo.Pay_price * usenum,
                                        Payment = 1,            //收支(0=收款,1=支出)
                                        Payment_type = "验票扣款",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点,分销扣款）
                                        Money_come = "分销账户",         //资金来源（网上支付,银行收款，分销账户等）
                                        Over_money = overmoney
                                    };
                                    var fin = AgentCompanyData.AgentFinancial(Financialinfo);

                                    var KouchuShouxufei_temp = OrderJsonData.KouchuShouxufei(orderinfo);

                                }



                            }
                        }


                    }
                }

                return 1;
            }
            catch (Exception ex)
            {

                return 0;
                throw;
            }
        }
        #endregion




        public static string GetEticketDetailNotValidate(string pno, string comid, string validateticketlogid)
        {
            try
            {
                var eticketloginfo = new B2bEticketLogData().GetTicketLogById(validateticketlogid);

                var prodata = new B2bEticketData();
                var eticketinfo = prodata.GetEticketDetail(pno);

                List<B2b_eticket> list = new List<B2b_eticket>();
                list.Add(eticketinfo);
                IEnumerable result = "";
                if (list != null)
                {
                    result = from eticket in list
                             select new
                             {
                                 ProductName = eticketinfo.E_proname,
                                 ThisUseNum = eticketloginfo.Use_pnum,
                                 OnePrice = eticket.E_face_price,
                                 Pno = eticket.Pno,
                                 Pro_Start = new B2bComProData().GetProById(eticket.Pro_id.ToString()).Pro_start,
                                 Pro_end = new B2bComProData().GetProById(eticket.Pro_id.ToString()).Pro_end,
                                 Company = AgentCompanyData.GetAgentByid(eticket.Agent_id) == null ? "" : AgentCompanyData.GetAgentByid(eticket.Agent_id).Company,//出票单位
                             };
                }


                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        #region 根据公司id获取验票明细列表
        public static string EPageList(string comid, int pageindex, int pagesize, int eclass, int proid, int jsid, int posid = 0, string key = "", string startime = "", string endtime = "", int agentid = 0, int projectid = 0,int saleagentid=0)
        {
            var totalcount = 0;
            try
            {

                var edata = new B2bEticketLogData();
                var list = edata.EPageList(comid, pageindex, pagesize, eclass, proid, jsid, out totalcount, posid, key, startime, endtime, agentid, projectid, saleagentid);
                IEnumerable result = "";
                if (list != null)

                    result = from e in list
                             select new
                             {
                                 id = e.Id,
                                 //orderid = new B2bOrderData().GetOrderByEticketid(e.Eticket_id).Id,
                                 ProName = new B2bEticketData().GetEticketDetail(e.Pno).E_proname,
                                 FacePrice = new B2bEticketData().GetEticketDetail(e.Pno).E_face_price,
                                 UseNum = e.Use_pnum,
                                 ProEnd = new B2bComProData().GetProById(new B2bEticketData().GetEticketDetail(e.Pno).Pro_id.ToString()).Pro_end,
                                 Pno = e.Pno,
                                 ConfirmDate = e.Actiondate.ToString("yyyy-MM-dd HH:mm:ss"),
                                 Proid = new B2bEticketData().GetEticketDetail(e.Pno).Pro_id,
                                 Orderid = new B2bEticketData().GetEticketbyid(e.Eticket_id),
                                 Outcompany = new B2bEticketData().GetOutcompanybyid(e.Eticket_id),
                                 Pcaccount = e.Pcaccount,
                                 PosId = e.PosId
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion
        #region 每日结算列表
        public static string DayJSList(string comid)
        {

            try
            {

                var DayJSdata = new DayJieSuanData();
                var dsdayjs = DayJSdata.DayJSList(comid);


                if (dsdayjs == null)
                {
                    return JsonConvert.SerializeObject(new { type = 100, totalCount = 0, msg = dsdayjs });
                }
                return JsonConvert.SerializeObject(new { type = 100, msg = dsdayjs });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion
        #region 得到特定用户每日结算统计
        public static string DayJSResult(string comid, string jsid)
        {
            try
            {

                var DayJSdata = new DayJieSuanData();
                var dsdayjs = DayJSdata.DayJSResult(comid, jsid);



                return JsonConvert.SerializeObject(new { type = 100, msg = dsdayjs });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion
        #region 获取电子票详情 输出是否成功

        public static string GetEticketDetail(string pno, string comid, out int isget)
        {
            isget = 0;
            try
            {

                var prodata = new B2bEticketData();
                var eticketinfo = prodata.GetEticketDetail(pno);



                string reason = "";
                //录入电子码验证日志
                B2bEticketLogData elogdata = new B2bEticketLogData();
                B2b_eticket_log elog = new B2b_eticket_log()
                {
                    Id = 0,
                    Eticket_id = 0,
                    Pno = pno,
                    Action = (int)ECodeOper.ValidateECode,
                    A_state = (int)ECodeOperStatus.OperFail,
                    A_remark = reason,
                    Use_pnum = 0,
                    Actiondate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    Com_id = int.Parse(comid),
                    PosId = 0
                };

                if (eticketinfo == null)
                {
                    reason = "查询操作不成功，不存在此电子码，请核查是否输入错误";
                    elog.A_remark = reason;
                    elogdata.InsertOrUpdateLog(elog);

                    return JsonConvert.SerializeObject(new { type = 1, msg = reason });
                }

                //是否是此商家的产品
                int com_id = eticketinfo.Com_id;
                if (com_id.ToString() != comid)
                {
                    reason = "电子码无法使用,不是此商家产品";
                    elog.A_remark = reason;
                    elog.Eticket_id = new B2bEticketData().GetEticketDetail(pno).Id;
                    elogdata.InsertOrUpdateLog(elog);
                    return JsonConvert.SerializeObject(new { type = 1, msg = reason });
                }
                //电子码可使用票数
                int usenum = eticketinfo.Use_pnum;
                if (usenum == 0)
                {
                    reason = "电子码无法使用,电子码可使用数量为0";
                    elog.A_remark = reason;
                    elog.Eticket_id = new B2bEticketData().GetEticketDetail(pno).Id;
                    elogdata.InsertOrUpdateLog(elog);
                    return JsonConvert.SerializeObject(new { type = 1, msg = reason });

                }
                //电子码状态（作废，正常）
                int vstate = eticketinfo.V_state;
                if (vstate == (int)EticketCodeStatus.HasValidate || vstate == (int)EticketCodeStatus.HasZuoFei)
                {
                    reason = "电子码无法使用,电子码状态：" + EnumUtils.GetName((EticketCodeStatus)vstate);
                    elog.A_remark = reason;
                    elog.Eticket_id = new B2bEticketData().GetEticketDetail(pno).Id;
                    elogdata.InsertOrUpdateLog(elog);
                    return JsonConvert.SerializeObject(new { type = 1, msg = reason });
                }


                isget = 1;
                return JsonConvert.SerializeObject(new { type = 100, msg = eticketinfo });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion
        #region 得到电子票详细信息（包括厂家信息，产品信息，电子票信息）
        public static string GetPnoDetail(string pno)
        {
            try
            {
                var prodata = new B2bEticketData();
                var eticketinfo = prodata.GetPnoDetail(pno);

                List<B2b_eticket> list = new List<B2b_eticket>();
                list.Add(eticketinfo);
                IEnumerable result = "";
                if (list != null)
                {
                    result = from eticket in list
                             select new
                             {
                                 ProductName = eticket.E_proname,
                                 UserNum = eticket.Use_pnum,
                                 Pro_end = eticket.Compro.Pro_end,
                                 ServiceContain = eticket.Compro.Service_Contain,
                                 ServiceNotContain = eticket.Compro.Service_NotContain,
                                 Precautions = eticket.Compro.Precautions,
                                 ComAddress = eticket.Companyinfo.Com_add,
                                 ScenicName = eticket.Company.Scenic_name,
                             };
                }


                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 得到电子票详细信息（包括厂家信息，产品信息，电子票信息）
        public static string GetEticketSearch(string pno, int comid, int agentid)
        {
            try
            {
                var prodata = new B2bEticketData();
                var orderdate = new B2bOrderData();

                //先查询电子码是否有效
                var eticket_info = prodata.GetPnoEticketinfo(pno);
                if (eticket_info == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "没有此电子票", totalCount = 0 });
                }

                var oid = eticket_info.Oid;//原始订单号
                //查询此订单是否为 导入产品订单
                var order_id = orderdate.Getinitorderid(oid);
                B2b_order order_info = null;

                if (order_id != 0)
                { //如果有 A订单也就是 分销导入订单
                    order_info = orderdate.GetOrderById(order_id);
                    if (order_info == null)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "订单查询错误", totalCount = 0 });
                    }

                    //如果传入COMID都不符则退出
                    if (comid != order_info.Comid && comid != eticket_info.Com_id)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "订单错误，权限不足", totalCount = 0 });
                    }

                    //如果传入agentid都不符则退出
                    if (agentid != order_info.Agentid && agentid != eticket_info.Agent_id)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "订单错误，超出权限.", totalCount = 0 });
                    }

                    comid = eticket_info.Com_id;
                    agentid = eticket_info.Agent_id;

                }








                var eticketinfo = prodata.GetEticketSearch(pno, comid, agentid);

                if (eticketinfo == null)
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = "", totalCount = 0 });
                }
                B2b_eticket eticketmodel = new B2b_eticket
                {
                    E_face_price = eticketinfo.E_face_price,
                    Agent_id = eticketinfo.Agent_id,
                    Com_id = eticketinfo.Com_id,
                    E_sale_price = eticketinfo.E_sale_price,
                    E_proname = eticketinfo.E_proname,
                    Id = eticketinfo.Id,
                    Oid = eticketinfo.Oid,
                    Warrant_type = orderdate.GetOrderById(eticketinfo.Oid).Warrant_type,
                    Pnum = eticketinfo.Pnum,
                    Use_pnum = eticketinfo.Use_pnum,
                    Subdate = eticketinfo.Subdate,
                    Pno = eticketinfo.Pno,
                    V_state = eticketinfo.V_state,
                    Cancelnum = GetCancelNum(eticketinfo.Oid, eticketinfo.Pno),
                    VerifyNum = GetVerifyNum(eticketinfo.Pno)
                };

                return JsonConvert.SerializeObject(new { type = 100, msg = eticketmodel, totalCount = 1 });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        private static int GetVerifyNum(string pno)
        {
            int verifynum = new B2bEticketLogData().GetVerifyNum(pno);
            return verifynum;
        }
        #endregion


        #region 退票电子票列表
        public static string GetBackEticketlist(int comid, int agentid, int pageindex, int pagesize)
        {
            int totalCount = 0;
            try
            {
                var prodata = new B2bEticketData();
                var orderdate = new B2bOrderData();
                var eticketinfo = prodata.GetBackEticketlist(comid, agentid, pageindex, pagesize, out totalCount);

                if (eticketinfo == null)
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = "", totalCount = 0 });
                }

                IEnumerable result = "";
                if (eticketinfo != null)

                    result = from e in eticketinfo
                             select new
                             {
                                 E_face_price = e.E_face_price,
                                 Agent_id = e.Agent_id,
                                 Com_id = e.Com_id,
                                 E_sale_price = e.E_sale_price,
                                 E_proname = e.E_proname,
                                 Id = e.Id,
                                 Oid = e.Oid,
                                 Warrant_type = orderdate.GetOrderById(e.Oid).Warrant_type,
                                 Pnum = e.Pnum,
                                 Use_pnum = e.Use_pnum,
                                 Subdate = e.Subdate,
                                 Pno = e.Pno,
                                 V_state = e.V_state,
                                 Cancelnum = GetCancelNum(e.Oid, e.Pno),
                                 VerifyNum = GetVerifyNum(e.Pno)
                             };

                return JsonConvert.SerializeObject(new { type = 100, msg = result, totalCount = totalCount });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        private static string GetCancelNum(int orderid, string pno = "")
        {
            if (pno != "")
            {
                B2b_eticket m_eticket = new B2bEticketData().GetEticketDetail(pno);
                if (m_eticket != null)
                {
                    int totalnum = m_eticket.Pnum;//总数目
                    int surplusnum = m_eticket.Use_pnum;//剩余数目
                    int verifynum = GetVerifyNum(pno);//验证数目
                    int quitnum = totalnum - surplusnum - verifynum;//退票数目

                    return quitnum.ToString();
                }
                else
                { //此种情况不可能出现
                    return "--";
                }
            }
            else
            {
                //判断 当前订单 是否是某个订单的绑定订单
                B2b_order loldorder = new B2bOrderData().GetOldorderBybindingId(orderid);
                if (loldorder != null)
                {
                    orderid = loldorder.Id;
                }

                B2b_order m = new B2bOrderData().GetOrderById(orderid);
                if (m == null)
                {
                    return "--";
                }
                else
                {
                    return m.Cancelnum.ToString();
                    #region 注释部分
                    ////如果倒码订单，每个电子码 1张票，也只能退一张
                    //if (m.Warrant_type == 2)
                    //{
                    //    return "1";
                    //}

                    ////如果出票订单，一个订单多个码，则每个电子码也是一张，所以返回1
                    //if (m.yanzheng_method == 1)
                    //{
                    //    return "1";
                    //}
                    //if (m.Cancelnum == 0)
                    //{
                    //    return "--";
                    //}
                    //else
                    //{
                    //    return m.Cancelnum.ToString();
                    //}
                    #endregion
                }


            }

        }
        #endregion


        #region 分销退电子票--未考虑导入产品的退票，暂时弃用
        public static string BackAgentEticket(int id, string pno, int comid, int agentid)
        {
            try
            {
                //查询电子票
                var prodata = new B2bEticketData();
                B2b_eticket eticketinfo = prodata.GetEticketByID(id.ToString());


                if (eticketinfo == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "电子码错误" });
                }
                //查询订单
                var orderdate = new B2bOrderData();
                var oldorder = orderdate.GetOrderById(eticketinfo.Oid);

                if (oldorder == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "订单错误" });
                }

                //商家是否匹配
                if (comid != eticketinfo.Com_id)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "商户匹配错误" });
                }

                //分销是否匹配
                if (agentid != oldorder.Agentid)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "分销匹配错误" });
                }

                //查询分销商授权ID
                var agentinfo = AgentCompanyData.GetAgentWarrant(oldorder.Agentid, oldorder.Comid);
                if (agentinfo == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "分销信息错误" });
                }

                B2b_com_pro pro = new B2bComProData().GetProById(oldorder.Pro_id.ToString());
                if (pro == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "订单中产品不存在" });
                }
                #region 退票条件验证
                if (pro.Server_type == 10)
                {
                    int isjietuan = new Travelbusorder_operlogData().Ishasplanbus(pro.Id, oldorder.U_traveldate);
                    if (isjietuan > 0)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "旅游大巴产品截团后不可退票" });
                    }
                    //if (DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) >= oldorder.U_traveldate)
                    //{
                    //    return JsonConvert.SerializeObject(new { type = 1, msg = "旅游大巴产品 乘车日期[不包括乘车日期]前退票" });
                    //}
                }
                //得到产品退票机制:0有效期内退票；1有效期内/外 均可退票；2不可退票
                int QuitTicketMechanism = pro.QuitTicketMechanism;
                if (QuitTicketMechanism == 0)
                {
                    B2b_com_pro modelcompro = pro;
                    int prosourtype = modelcompro.Source_type;
                    if (prosourtype == 4)//分销导入产品; 
                    {
                        int old_proid = new B2bComProData().GetOldproidById(pro.Id.ToString());//绑定产品的原始编号
                        if (old_proid == 0)
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "获取产品原始编号失败" });
                        }
                        else
                        {
                            prosourtype = new B2bComProData().GetProSource_typeById(old_proid.ToString());
                        }
                    }
                    #region 产品有效期
                    //经过以上赋值prosourtype，只可能2个值:1系统自动生成码产品;2倒码产品
                    DateTime pro_start = modelcompro.Pro_start;
                    DateTime pro_end = modelcompro.Pro_end;
                    if (prosourtype == 2 || prosourtype == 3) //倒码产品 或者 外来接口导入产品 
                    { }
                    if (prosourtype == 1) //系统自动生成码产品
                    {
                        #region 产品有效期判定(微信模板--门票订单预订成功通知 中也有用到)
                        string provalidatemethod = modelcompro.ProValidateMethod;
                        int appointdate = modelcompro.Appointdata;
                        int iscanuseonsameday = modelcompro.Iscanuseonsameday;

                        //DateTime pro_end = modelcompro.Pro_end;
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

                        //DateTime pro_start = modelcompro.Pro_start;
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
                    }
                    #endregion

                    if (pro_end >= DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
                    {
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "产品已过有效期,不可退票" });
                    }
                }
                if (QuitTicketMechanism == 2)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "当前产品不可退票" });
                }
                #endregion

                decimal overmoney = agentinfo.Imprest;
                int Use_pnum = eticketinfo.Use_pnum;//可使用数量

                //作废电子码(分销退票，使用数量清零)
                var backeticketinfo = prodata.BackAgentEticket(id, pno);

                if (backeticketinfo > 0)
                {
                    if (oldorder.Warrant_type == 1)//订单授权出票扣款的 退票退款，验证扣款的直接跳过
                    {
                        #region 分销商财务退票返款
                        //合计预付款
                        overmoney = overmoney + eticketinfo.E_sale_price * Use_pnum;

                        Agent_Financial Financialinfo = new Agent_Financial
                        {
                            Id = 0,
                            Com_id = oldorder.Comid,
                            Agentid = oldorder.Agentid,
                            Warrantid = oldorder.Warrantid,
                            Order_id = eticketinfo.Oid,
                            Servicesname = eticketinfo.E_proname + "[" + pno + "]",
                            SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                            Money = eticketinfo.E_sale_price * Use_pnum,
                            Payment = 0,            //收支(0=收款,1=支出)
                            Payment_type = "退票款",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点,分销扣款）
                            Money_come = "分销账户",         //资金来源（网上支付,银行收款，分销账户等）
                            Over_money = overmoney
                        };
                        var fin = AgentCompanyData.AgentFinancial(Financialinfo);
                        #endregion

                        #region 系统自动生成电子票退票(只针对出票扣款)，对订单进行处理； 其他方式电子码不用考虑，在查询中查询不到
                        if (oldorder.Ticket != 0)//已经发生过退票，需要把新退票信息加入
                        {
                            oldorder.Ticket = oldorder.Ticket + Use_pnum * oldorder.Pay_price;
                            oldorder.Ticketinfo = oldorder.Ticketinfo + "[" + pno + "]";
                        }
                        else
                        {
                            oldorder.Ticket = Use_pnum * oldorder.Pay_price;
                            oldorder.Ticketinfo = "根据电子票号退票[" + pno + "]";
                        }
                        oldorder.Backtickettime = DateTime.Now;
                        OrderJsonData.InsertRecharge(oldorder);

                        if (oldorder.Order_state == 4 || oldorder.Order_state == 22 || oldorder.Order_state == 8)//只对已发码或已处理，产品进行退票处理，未发码（只是支付了不处理），部分使用退票
                        {
                            #region 产品来源,系统自动生成,直接完成退票
                            if (pro.Source_type == 1)
                            {
                                #region 作废票处理
                                //订单状态为:订单退款
                                if (oldorder.Agentid == 0)
                                {//直销订单，标记为处理中做财务处理
                                    oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                                }
                                else
                                {//分销订单直接退款
                                    oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                                }

                                OrderJsonData.InsertRecharge(oldorder);
                                #endregion
                            }
                            #endregion
                            else
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "产品来源不对" });
                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "退票错误" });
                }

                //把退票张数记录到数据库订单表中
                if (oldorder.Id > 0 && Use_pnum > 0)
                {
                    if (oldorder.Cancelnum > 0)//已经发生过退票,需要把新退票张数加入
                    {
                        oldorder.Cancelnum = oldorder.Cancelnum + Use_pnum;
                        int inscanclenum = new B2bOrderData().InsertCancleTicketNum(oldorder.Id, oldorder.Cancelnum);
                    }
                    else
                    {
                        int inscanclenum = new B2bOrderData().InsertCancleTicketNum(oldorder.Id, Use_pnum);
                    }

                    //服务类型是票务，需要把可销售数量和 已销售数量回滚
                    if (pro.Server_type == 1)
                    {
                        if (pro.Ispanicbuy == 1 || pro.Ispanicbuy == 2)
                        {
                            //把票务的可销售数量增加，已销售数量减少
                            int Rollbackticket = new B2bComProData().AddLimittotalnum(pro.Id, Use_pnum);
                        }
                    }

                }
                return JsonConvert.SerializeObject(new { type = 100, msg = backeticketinfo });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 分销退电子票-对外接口
        public static string BackAgentEticket_interface(int orderid, string pno, string dremark = "")
        {
            try
            {
                //判断是否有订单 绑定传入的订单
                B2b_order d_loldorder = new B2bOrderData().GetOldorderBybindingId(orderid);
                if (d_loldorder != null)
                {
                    orderid = d_loldorder.Id;
                }

                //查询电子票
                var prodata = new B2bEticketData();
                B2b_eticket eticketinfo = prodata.GetEticketDetail(pno);
                if (eticketinfo == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "电子码错误" });
                }

                //查询订单
                var orderdate = new B2bOrderData();
                var oldorder = orderdate.GetOrderById(orderid);//a订单

                if (oldorder == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "查询订单错误" });
                }
                //查询分销商授权ID
                var agentinfo = AgentCompanyData.GetAgentWarrant(oldorder.Agentid, oldorder.Comid);
                if (agentinfo == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "分销信息错误" });
                }

                B2b_com_pro pro = new B2bComProData().GetProById(oldorder.Pro_id.ToString());
                if (pro == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "订单中产品不存在" });
                }
                #region 退票条件验证
                if (pro.Server_type == 10)
                {
                    int isjietuan = new Travelbusorder_operlogData().Ishasplanbus(pro.Id, oldorder.U_traveldate);
                    if (isjietuan > 0)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "旅游大巴产品截团后不可退票" });
                    }
                    //if(DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"))>=oldorder.U_traveldate)
                    //{
                    //    return JsonConvert.SerializeObject(new { type = 1, msg = "旅游大巴产品 乘车日期[不包括乘车日期]前退票" });
                    //}
                }
                //得到产品退票机制:0有效期内退票；1有效期内/外 均可退票；2不可退票
                int QuitTicketMechanism = pro.QuitTicketMechanism;
                if (QuitTicketMechanism == 0)
                {
                    B2b_com_pro modelcompro = pro;
                    int prosourtype = modelcompro.Source_type;
                    if (prosourtype == 4)//分销导入产品; 
                    {
                        int old_proid = new B2bComProData().GetOldproidById(pro.Id.ToString());//绑定产品的原始编号
                        if (old_proid == 0)
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "获取产品原始编号失败" });
                        }
                        else
                        {
                            prosourtype = new B2bComProData().GetProSource_typeById(old_proid.ToString());
                        }
                    }
                    #region 产品有效期
                    //经过以上赋值prosourtype，只可能2个值:1系统自动生成码产品;2倒码产品
                    DateTime pro_start = modelcompro.Pro_start;
                    DateTime pro_end = modelcompro.Pro_end;
                    if (prosourtype == 2 || prosourtype == 3) //倒码产品 或者 外来接口导入产品 
                    { }
                    if (prosourtype == 1) //系统自动生成码产品
                    {
                        #region 产品有效期判定(微信模板--门票订单预订成功通知 中也有用到)
                        string provalidatemethod = modelcompro.ProValidateMethod;
                        int appointdate = modelcompro.Appointdata;
                        int iscanuseonsameday = modelcompro.Iscanuseonsameday;

                        //DateTime pro_end = modelcompro.Pro_end;
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

                        //DateTime pro_start = modelcompro.Pro_start;
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
                    }
                    #endregion

                    if (pro_end >= DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
                    {
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "产品已过有效期,不可退票" });
                    }
                }
                if (QuitTicketMechanism == 2)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "当前产品不可退票" });
                }
                #endregion

                #region 产品来源,系统自动生成,直接完成退票
                if (pro.Source_type == 1)
                {
                    #region 作废票处理:订单状态、电子码可用数量 改变
                    //订单状态为:订单退款
                    if (oldorder.Agentid == 0)
                    {//直销订单，标记为处理中做财务处理
                        oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                    }
                    else
                    {//分销订单直接退款
                        oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                    }

                    OrderJsonData.InsertRecharge(oldorder);

                    //电子码作废
                    var eticketback = prodata.BackAgentEticket(eticketinfo.Id, pno);

                    #endregion
                }
                #endregion
                #region 产品来源,分销系统导入产品
                else if (pro.Source_type == 4)
                {
                    #region 作废票处理
                    //订单状态为:订单退款
                    if (oldorder.Agentid == 0)
                    {//直销订单，标记为处理中做财务处理
                        oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                    }
                    else
                    {//分销订单直接退款
                        oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                    }

                    OrderJsonData.InsertRecharge(oldorder);

                    //电子码作废
                    var eticketback = prodata.BackAgentEticket(eticketinfo.Id, pno);
                    #endregion

                    #region 对分销导入产品，对绑定分销账户的订单进行处理并退款
                    var agentorder = new B2bOrderData().GetOrderById(oldorder.Bindingagentorderid);
                    if (agentorder != null)
                    {
                        agentorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                        var agentorderup = OrderJsonData.InsertRecharge(agentorder);

                        //读取分销商信息
                        Agent_company bind_agentinfo = AgentCompanyData.GetAgentWarrant(agentorder.Agentid, agentorder.Comid);
                        if (bind_agentinfo == null)
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "绑定的分销账户授权信息错误，退票操作部分失败，请手工完成后续操作" });
                        }
                        else
                        {
                            if (agentorder.Warrant_type == 1)
                            {
                                //计算分销余额
                                decimal overmoney = bind_agentinfo.Imprest + eticketinfo.Use_pnum * eticketinfo.E_sale_price;

                                //分销商财务扣款
                                Agent_Financial Financialinfo = new Agent_Financial
                                {
                                    Id = 0,
                                    Com_id = agentorder.Comid,
                                    Agentid = agentorder.Agentid,
                                    Warrantid = agentorder.Warrantid,
                                    Order_id = agentorder.Id,
                                    Servicesname = pro.Pro_name + "[" + oldorder.Bindingagentorderid + "]",
                                    SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                                    Money = eticketinfo.Use_pnum * eticketinfo.E_sale_price,
                                    Payment = 0,            //收支(0=收款,1=支出)
                                    Payment_type = "退票退款",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点,分销扣款）
                                    Money_come = "分销账户",         //资金来源（网上支付,银行收款，分销账户等）
                                    Over_money = overmoney
                                };
                                var fin = AgentCompanyData.AgentFinancial(Financialinfo);
                            }

                        }
                    }
                    #endregion

                }
                #endregion
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "产品来源不对" });
                }

                #region 必须分销订单 Agentid，必须是后台销售订单Warrant_type(倒码订单不需要财务处理)

                if (oldorder.Agentid != 0 && oldorder.Warrant_type == 1)
                {
                    //合计预付款
                    decimal overmoney = agentinfo.Imprest + oldorder.Pay_price * eticketinfo.Use_pnum;

                    Agent_Financial Financialinfo = new Agent_Financial
                    {
                        Id = 0,
                        Com_id = oldorder.Comid,
                        Agentid = oldorder.Agentid,
                        Warrantid = oldorder.Warrantid,
                        Order_id = oldorder.Id,
                        Servicesname = eticketinfo.E_proname + "[" + pno + "]",
                        SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                        Money = oldorder.Pay_price * eticketinfo.Use_pnum,
                        Payment = 0,            //收支(0=收款,1=支出)
                        Payment_type = "退票款",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点,分销扣款）
                        Money_come = "分销账户",         //资金来源（网上支付,银行收款，分销账户等）
                        Over_money = overmoney
                    };
                    var fin = AgentCompanyData.AgentFinancial(Financialinfo);
                }
                #endregion

                #region 把退票张数记录到数据库订单表中;并且回滚产品数量
                if (oldorder.Id > 0 && eticketinfo.Use_pnum > 0)
                {
                    if (oldorder.Cancelnum > 0)//已经发生过退票,需要把新退票张数加入
                    {
                        oldorder.Cancelnum = oldorder.Cancelnum + eticketinfo.Use_pnum;
                        int inscanclenum = new B2bOrderData().InsertCancleTicketNum(oldorder.Id, oldorder.Cancelnum);

                        //绑定订单退票 退票数量记录入数据库
                        if (pro.Source_type == 4)
                        {
                            int inscanclenum2 = new B2bOrderData().InsertCancleTicketNum(oldorder.Bindingagentorderid, oldorder.Cancelnum);
                        }
                    }
                    else
                    {
                        int inscanclenum = new B2bOrderData().InsertCancleTicketNum(oldorder.Id, eticketinfo.Use_pnum);
                        //绑定订单退票 退票数量记录入数据库
                        if (pro.Source_type == 4)
                        {
                            int inscanclenum2 = new B2bOrderData().InsertCancleTicketNum(oldorder.Bindingagentorderid, eticketinfo.Use_pnum);
                        }
                    }

                    //服务类型是票务，需要把可销售数量和 已销售数量回滚
                    if (pro.Server_type == 1)
                    {
                        if (pro.Ispanicbuy == 1 || pro.Ispanicbuy == 2)
                        {
                            //把票务的可销售数量增加，已销售数量减少
                            int Rollbackticket = new B2bComProData().AddLimittotalnum(pro.Id, eticketinfo.Use_pnum);
                        }
                    }

                }
                #endregion

                return JsonConvert.SerializeObject(new { type = 100, msg = "退票成功" });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 得到电子票验码日志
        public static string GetPnoConsumeLogList(string pno)
        {
            try
            {
                List<B2b_eticket_log> log = new B2bEticketLogData().GetPnoConsumeLogList(pno);
                if (log != null)
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = log, totalcount = log.Count });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "", totalcount = 0 });
                }

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }
        #endregion

        #region 分销验证日志
        public static string AgentEticketlog(int comid, int agentid, int pageindex, int pagesize, string key = "", string startime = "", string endtime = "")
        {
            try
            {
                int totalCount = 0;
                B2bOrderData orderdata = new B2bOrderData();


                List<B2b_eticket_log> log = new B2bEticketLogData().AgentEticketlog(comid, agentid, pageindex, pagesize, out totalCount, key, startime, endtime);
                IEnumerable result = "";
                if (log != null)

                    result = from e in log
                             select new
                             {
                                 Id = e.Id,
                                 Pno = e.Pno,
                                 PosId = e.PosId,
                                 A_state = e.A_state,
                                 Action = e.Action,
                                 Actiondate = e.Actiondate,
                                 Eticket_id = e.Eticket_id,
                                 Use_pnum = e.Use_pnum,
                                 Agent_id = e.Agent_id,
                                 E_proname = e.E_proname,
                                 Com_id = e.Com_id,
                                 Pnum = e.Pnum,
                                 E_sale_price = e.E_sale_price,
                                 Oid = orderdata.GetIdOrderBybindingId(e.Oid),
                                 E_type = e.E_type,
                             };





                if (log != null)
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = result, totalCount = totalCount });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "", totalCount = 0 });
                }

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }
        #endregion

        #region 按订单分销验证日志
        public static string VAgentEticketlog(int comid, int agentid, int orderid, int pageindex, int pagesize)
        {
            try
            {
                int totalCount = 0;
                List<B2b_eticket_log> log = new B2bEticketLogData().VAgentEticketlog(comid, agentid, orderid, pageindex, pagesize, out totalCount);
                if (log != null)
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = log, totalCount = totalCount });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "", totalCount = 0 });
                }

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }
        #endregion



        #region 接口核销日志
        public static string InterfaceUsePageList(string comid, int pageindex, int pagesize, string key = "", string startime = "", string endtime = "")
        {
            var totalcount = 0;
            try
            {

                B2b_company commanage = B2bCompanyData.GetAllComMsg(int.Parse(comid));
                WlGetProInfoDealRequestData wldata = new WlGetProInfoDealRequestData(commanage.B2bcompanyinfo.wl_PartnerId, commanage.B2bcompanyinfo.wl_userkey);


                var edata = new B2bEticketLogData();
                var list = wldata.InterfaceUsePageList(int.Parse(comid), pageindex, pagesize, out totalcount, key, startime, endtime);
                IEnumerable result = "";
                if (list != null)

                    result = from e in list
                             select new
                             {
                                 id = e.id,
                                 wlorder=e.wlorderid,
                                 usedQuantity=e.usedQuantity,
                                 usetime=e.usetime,
                                 comid=e.comid,
                                 orderid=e.orderid,
                                 wldatainfo = wldata.getWlOrderidData(e.wlorderid),
                                 orderdatainfo = new B2bOrderData().GetOrderById(e.orderid),
                                 prodatainfo = new B2bComProData().GetProById(e.partnerdealid.ToString()),
                                 Outcompany = new B2bEticketData().GetOutcompanybyorderid(e.orderid),
                                 
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion



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

        #region 发送验证同步请求:回调函数和验证同步方法
        public static void Completed(IAsyncResult result)
        {
            //获取委托对象，调用EndInvoke方法获取运行结果
            AsyncResult _result = (AsyncResult)result;
            AsyncsendEventHandler myDelegate = (AsyncsendEventHandler)_result.AsyncDelegate;
            myDelegate.EndInvoke(_result);
        }
        public static void AsyncSend(string updateurl, string pno, int confirmnum, string confirmtime, int agentcomid, int comidd1, int validateticketlogid)
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
                        var eticketinfo = new B2bEticketData().GetEticketDetail(pno);
                        if (eticketinfo == null)
                        {
                            log.Remark = "获得电子票信息失败";//单引号替换为'',双引号不用处理;
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

                        if (eticketinfo.Oid == 0)
                        {
                            log.Remark = "电子票对应的订单号为0";//单引号替换为'',双引号不用处理;
                            log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                            new Agent_asyncsendlogData().EditLog(log);
                            return;
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

                        string re = "";
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
                        "</business_trans>", listagent_rlog[0].Req_seq, log.Platform_req_seq, eticketinfo.Oid, confirmnum, ConvertDateTimeInt(DateTime.Parse(confirmtime)), pno);
                        #region 分销通知发送方式post
                        if (agentinfo.inter_sendmethod.ToLower() == "post")
                        {
                            if (agentcomid == 6490)
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
                                    "</business_trans>", listagent_rlog[0].Req_seq, log.Platform_req_seq, eticketinfo.Oid, eticketinfo.Pnum, ConvertDateTimeInt(DateTime.Parse(confirmtime)), pno);

                                    //re = GetUrlData.ResponseString(sbuilder, updateurl);
                                    re = new GetUrlData().HttpPost(updateurl, sbuilder);
                                }
                                //string encode_ret = EncryptionHelper.DESEnCode(sbuilder, agentinfo.Inter_deskey);//加密
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

        #region 发送验证同步请求:回调函数和验证同步方法--去哪
        public static void Completed_qunar(IAsyncResult result)
        {
            //获取委托对象，调用EndInvoke方法获取运行结果
            AsyncResult _result = (AsyncResult)result;
            AsyncsendEventHandler_qunar myDelegate = (AsyncsendEventHandler_qunar)_result.AsyncDelegate;
            myDelegate.EndInvoke(_result);
        }
        public static void AsyncSend_qunar(B2b_order ordermodel)
        {
            string orderQuantity = ordermodel.U_num.ToString();
            int hasConsumeNum = new B2bOrderData().GetHasConsumeNumByOrderId(ordermodel.Id);//累计消费数量(包含退票数量)
            string useQuantity = (hasConsumeNum - ordermodel.Cancelnum).ToString();//累计消费数量(不包含退票数量)
            string consumeInfo = "";

            string partnerorderId = ordermodel.Id.ToString();
            //需要判断订单 是否为 导入产品的订单
            int initorderid = new B2bOrderData().Getinitorderid(ordermodel.Id);
            if (initorderid > 0)
            {
                //获得原始直销订单信息
                partnerorderId = initorderid.ToString();

                //判断b订单是否在20160121前由于去哪儿录入产品号错误 产生的错误订单中(147613,147849,148717,148813,148815,148819,148840,148846,148863,148889,149251,149779,149906,150088,150116,150207,150391,150412,150467,150482,150501,150515,150526,150529,150530,150533,150535,150587,150599,150661,150837,150866,151408,151538,151777,151870,152052,154026,154751,155122,155858,156112,156419,160826,162089,163317)，
                //如果在的话用b订单号，否则用a订单号，这个判断需要保留,直到狼牙山上面的46个订单都处理完成后才可删除，谨记!!! by xiaoliu
                int[] intqud = { 147613, 147849, 148717, 148813, 148815, 148819, 148840, 148846, 148863, 148889, 149251, 149779, 149906, 150088, 150116, 150207, 150391, 150412, 150467, 150482, 150501, 150515, 150526, 150529, 150530, 150533, 150535, 150587, 150599, 150661, 150837, 150866, 151408, 151538, 151777, 151870, 152052, 154026, 154751, 155122, 155858, 156112, 156419, 160826, 162089, 163317 };
                if (intqud.Contains(ordermodel.Id))
                {
                    partnerorderId = ordermodel.Id.ToString();
                }
                ordermodel = new B2bOrderData().GetOrderById(initorderid);
            }


            #region 如果是去哪订单，向去哪发送消费通知
            if (ordermodel.qunar_orderid != "")
            {
                int rlogid = 0;
                int qunar_requestid = 0;
                string requestParam = GetQunarRequestParam(partnerorderId, orderQuantity, useQuantity, consumeInfo, ordermodel.Comid, out qunar_requestid, out rlogid);
                if (requestParam != "" && qunar_requestid > 0)
                {
                    //string requestjson = JsonConvert.SerializeObject(new { method = "noticeOrderConsumed", requestParam = requestParam });
                    //string qunar_ret = new GetUrlData().HttpPost("http://agentat.piao.qunar.com/singleApiDebugData?method=noticeOrderConsumed&requestParam=" + requestParam, "");
                    //string qunar_ret = new GetUrlData().HttpPost("http://agent.beta.qunar.com/api/external/supplierServiceV2.qunar?method=noticeOrderConsumed&requestParam=" + requestParam, "");
                    //string qunar_ret = new GetUrlData().HttpPost("http://piaoagentdev.qunar.com/api/external/supplierServiceV2.qunar?method=noticeOrderConsumed&requestParam=" + requestParam, "");//联调环境地址

                    string qunar_ret = new GetUrlData().HttpPost("http://agent.piao.qunar.com/api/external/supplierServiceV2.qunar?method=noticeOrderConsumed&requestParam=" + requestParam, "");//正式环境地址

                    //把得到的数据解析出来
                    string requestParamdd = "{\"root\":" + qunar_ret + "}";
                    XmlDocument doc1 = JsonConvert.DeserializeXmlNode(requestParamdd);
                    XmlElement root = doc1.DocumentElement;
                    string data = root.SelectSingleNode("data").InnerText;

                    //base64解密
                    data = Encoding.UTF8.GetString(EncryptionExtention.FromBase64(data));
                    TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "--data向去哪发送消费通知(orderid:" + partnerorderId + ")," + data);

                    XmlDocument xr = new XmlDocument();
                    xr.LoadXml(data);
                    XmlNamespaceManager nsMgr = new XmlNamespaceManager(xr.NameTable);
                    nsMgr.AddNamespace("ns", "http://piao.qunar.com/2013/QMenpiaoRequestSchema");

                    string message = "";
                    if (xr.SelectSingleNode("/ns:response/ns:body/ns:message", nsMgr) != null)
                    {
                        message = xr.SelectSingleNode("/ns:response/ns:body/ns:message", nsMgr).InnerText;
                    }
                    //把返回数据记入数据库
                    int insresponse = new Qunar_noticeOrderConsumedData().InsResponse(message, data, qunar_requestid);
                    if (insresponse == 0)
                    {
                        TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "向去哪发送消费通知(orderid:" + partnerorderId + "),把返回数据录入数据库(insresponse)出错");
                    }
                    if (rlogid > 0)
                    {
                        Qunar_ms_requestlog rlog = new Qunar_ms_requestlogData().GetLog(rlogid);
                        if (rlog == null)
                        {
                            TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "向去哪发送消费通知(orderid:" + partnerorderId + "),GetLog=null");
                        }
                        else
                        {
                            rlog.msg = "suc";
                            new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                        }

                    }
                    else
                    {
                        TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "向去哪发送消费通知(orderid:" + partnerorderId + "),rlogid=0");
                    }
                }
                else
                {
                    TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "向去哪发送消费通知(orderid:" + partnerorderId + "),生成请求数据出错");
                }

            }
            else
            {
                TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "向去哪发送消费通知(orderid:" + partnerorderId + "),去哪订单号为空");
            }
            #endregion
        }
        #endregion


        #region 测试异步执行
        delegate void AsyncsendEventHandler1(string pno);//发送验证同步发送请求委托

        public static void TestAsync()
        {
            string pno = "900001";//电子码

            AsyncsendEventHandler1 mydelegate = new AsyncsendEventHandler1(AsyncSend1);
            mydelegate.BeginInvoke(pno, new AsyncCallback(Completed1), null);



        }

        public static void Completed1(IAsyncResult result)
        {
            //获取委托对象，调用EndInvoke方法获取运行结果
            AsyncResult _result = (AsyncResult)result;
            AsyncsendEventHandler1 myDelegate = (AsyncsendEventHandler1)_result.AsyncDelegate;
            myDelegate.EndInvoke(_result);
        }


        public static void AsyncSend1(string pno)
        {
            try
            {
                string url = "";
                string re = new GetUrlData().HttpPost(url, "");

                string sql = "insert into test(retstr,pno) values('" + re + "','" + pno + "')";
                ExcelSqlHelper.ExecuteNonQuery(sql);
            }
            catch (Exception e)
            {
                string sql = "insert into test(retstr,pno) values('返回数据错误','" + pno + "')";
                ExcelSqlHelper.ExecuteNonQuery(sql);
            }
        }
        #endregion


        #region 安全码
        public static string GetComDayRandomlist(int comid, DateTime searchdate)
        {
            try
            {
                var log = new B2bCompanyData().GetComDayRandomlist(comid, searchdate);
                if (log != null)
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = log, totalCount = log.Count });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "", totalCount = 0 });
                }

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }
        #endregion



        #region 安全码
        public static string CreateComDayRandom(int comid, DateTime searchdate)
        {
            try
            {
                var log = new B2bCompanyData().CreateComDayRandom(comid, searchdate);

                return JsonConvert.SerializeObject(new { type = 100, msg = log });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }
        #endregion


       


    }

}
