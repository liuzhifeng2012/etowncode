using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.WeiXin.Service.WinXinService.BLL.WeiXinUploadDown;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using System.Data;
using ETS2.CRM.Service.CRMService.Data;
using ETS.Framework;
using ETS2.PM.Service.Qunar_Ms.Data;
using ETS2.PM.Service.Qunar_Ms.Model;
using ETS2.VAS.Service.VASService.Data;
using ETS2.VAS.Service.VASService.Modle;
using System.Xml;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;

namespace ETS2.WebApp.H5
{
    public partial class htest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            ////TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\weixinlogs\\aalog.txt", "uuu");
            ////TxtHelper.CheckLog("D:\\site\\b2betown\\ETS2.WebApp\\weixinlogs\\aalog.txt", "uuu");
            string lengthurl = TextBox1.Text.Trim();
            string shorturl = new GetUrlData().ToShortUrl(lengthurl);
            Label1.Text = shorturl;
        }

        #region 20160121去哪儿由于录入产品号错误 引起的问题 回滚,已经注释掉
        //protected void Button2_Click(object sender, EventArgs e)
        //{
        //    string orderid = TextBox1.Text.Trim();
        //    //查询出b订单信息
        //    B2b_order modelb2border = new B2bOrderData().GetOrderById(int.Parse(orderid));


        //    //查询出商户 "去哪儿直销" 2574 ,导入产品的产品编号
        //    string sqld1 = "select id  from b2b_com_pro where com_id=2574 and bindingid=" + modelb2border.Pro_id;
        //    object o1 = ExcelSqlHelper.ExecuteScalar(sqld1);
        //    int bindingproid = int.Parse(o1.ToString());

        //    //查询出b订单产品信息
        //    B2b_com_pro bindingpro = new B2bComProData().GetProById(bindingproid.ToString());

        //    //录入原始直销订单
        //    B2b_order order = new B2b_order()
        //                    {
        //                        Id = 0,
        //                        Pro_id = bindingpro.Id,
        //                        Speciid = 0,
        //                        Order_type = 1,
        //                        U_name = modelb2border.U_name,
        //                        U_id = modelb2border.U_id,
        //                        U_phone = modelb2border.U_phone,
        //                        U_num = modelb2border.U_num,
        //                        U_subdate = modelb2border.U_subdate,
        //                        Payid = 0,
        //                        Pay_price = bindingpro.Advise_price,
        //                        Cost = bindingpro.Agentsettle_price,
        //                        Profit = bindingpro.Advise_price - bindingpro.Agentsettle_price,
        //                        Order_state = modelb2border.Order_state,//
        //                        Pay_state = modelb2border.Pay_state,//支付模块未做出来前先设置为已支付
        //                        Send_state = modelb2border.Send_state,
        //                        Ticketcode = modelb2border.Ticketcode,//电子码未创建，支付后产生码赋值
        //                        Rebate = 0,//  利润返佣金额暂时定为0
        //                        Ordercome = "",//订购来源 暂时定为空
        //                        U_traveldate = modelb2border.U_traveldate,
        //                        Dealer = "自动",
        //                        Comid = 2574,
        //                        Pno = modelb2border.Pno,
        //                        Openid = "",
        //                        Ticketinfo = modelb2border.Ticketinfo,

        //                        Integral1 = 0,//积分
        //                        Imprest1 = 0,//预付款
        //                        Agentid = 0,     //分销ID
        //                        Warrantid = 0,   //授权ID
        //                        Warrant_type = 1,  //支付类型分销 1出票扣款 2验码扣款

        //                        pickuppoint = modelb2border.pickuppoint,
        //                        dropoffpoint = modelb2border.dropoffpoint,

        //                        Order_remark = modelb2border.Order_remark,
        //                        baoxiannames = modelb2border.baoxiannames,
        //                        baoxianpinyinnames = modelb2border.baoxianpinyinnames,
        //                        baoxianidcards = modelb2border.baoxianidcards,
        //                        travelnames = modelb2border.travelnames,
        //                        travelphones = modelb2border.travelphones,
        //                        serverid = "",
        //                        payserverpno = "",

        //                        travelidcards = modelb2border.travelidcards,
        //                        travelnations = modelb2border.travelnations,

        //                        travelremarks = modelb2border.travelremarks,
        //                        Backtickettime = modelb2border.Backtickettime,
        //                        Bindingagentorderid = modelb2border.Id

        //                    };
        //    if (modelb2border.Id != 148889)
        //    {
        //        int directorderid = new B2bOrderData().InsertOrUpdate(order);
        //        //把去哪儿订单号录入订单表
        //        int rd = new B2bOrderData().InsertQunar_Orderid(directorderid, modelb2border.qunar_orderid);
        //    }

        //    //得到验证的次数
        //    int yanzhenglogcount = new B2bEticketLogData().GetYanzhenglogCountByPno(modelb2border.Pno);
        //    if (yanzhenglogcount > 0)
        //    {
        //        #region 如果验证了，发送验证通知
        //        string orderQuantity = modelb2border.U_num.ToString();
        //        int hasConsumeNum = new B2bOrderData().GetHasConsumeNumByOrderId(modelb2border.Id);//累计消费数量(包含退票数量)
        //        string useQuantity = (hasConsumeNum - modelb2border.Cancelnum).ToString();//累计消费数量(不包含退票数量)
        //        string consumeInfo = "";

        //        string partnerorderId = modelb2border.Id.ToString();
        //        //需要判断订单 是否为 导入产品的订单
        //        int initorderid = new B2bOrderData().Getinitorderid(modelb2border.Id);
        //        if (initorderid > 0)
        //        {
        //            //获得原始直销订单信息
        //            partnerorderId = initorderid.ToString();

        //            //判断b订单是否在20160121前由于去哪儿录入产品号错误 产生的错误订单中(147613,147849,148717,148813,148815,148819,148840,148846,148863,148889,149251,149779,149906,150088,150116,150207,150391,150412,150467,150482,150501,150515,150526,150529,150530,150533,150535,150587,150599,150661,150837,150866,151408,151538,151777,151870,152052,154026,154751,155122,155858,156112,156419,160826,162089,163317)，
        //            //如果在的话用b订单号，否则用a订单号，这个判断需要保留,直到狼牙山上面的46个订单都处理完成后才可删除，谨记!!! by xiaoliu
        //            int[] intqud = { 147613, 147849, 148717, 148813, 148815, 148819, 148840, 148846, 148863, 148889, 149251, 149779, 149906, 150088, 150116, 150207, 150391, 150412, 150467, 150482, 150501, 150515, 150526, 150529, 150530, 150533, 150535, 150587, 150599, 150661, 150837, 150866, 151408, 151538, 151777, 151870, 152052, 154026, 154751, 155122, 155858, 156112, 156419, 160826, 162089, 163317 };
        //            //B2b_pay mpayyy = new B2bPayData().GetYYYYB2bPay(modelb2border.Id, 1145); 
        //            if (intqud.Contains(modelb2border.Id))
        //            {
        //                partnerorderId = modelb2border.Id.ToString();
        //            }
        //            modelb2border = new B2bOrderData().GetOrderById(initorderid);
        //        }


        //        #region 如果是去哪订单，向去哪发送消费通知
        //        if (modelb2border.qunar_orderid != "")
        //        {
        //            int rlogid = 0;
        //            int qunar_requestid = 0;
        //            string requestParam = GetQunarRequestParam(partnerorderId, orderQuantity, useQuantity, consumeInfo, modelb2border.Comid, out qunar_requestid, out rlogid);
        //            if (requestParam != "" && qunar_requestid > 0)
        //            {
        //                string qunar_ret = new GetUrlData().HttpPost("http://agent.piao.qunar.com/api/external/supplierServiceV2.qunar?method=noticeOrderConsumed&requestParam=" + requestParam, "");//正式环境地址

        //                //把得到的数据解析出来
        //                string requestParamdd = "{\"root\":" + qunar_ret + "}";
        //                XmlDocument doc1 = JsonConvert.DeserializeXmlNode(requestParamdd);
        //                XmlElement root = doc1.DocumentElement;
        //                string data = root.SelectSingleNode("data").InnerText;

        //                //base64解密
        //                data = Encoding.UTF8.GetString(EncryptionExtention.FromBase64(data));
        //                TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "--data向去哪发送消费通知(orderid:" + partnerorderId + ")," + data);

        //                XmlDocument xr = new XmlDocument();
        //                xr.LoadXml(data);
        //                XmlNamespaceManager nsMgr = new XmlNamespaceManager(xr.NameTable);
        //                nsMgr.AddNamespace("ns", "http://piao.qunar.com/2013/QMenpiaoRequestSchema");

        //                string message = "";
        //                if (xr.SelectSingleNode("/ns:response/ns:body/ns:message", nsMgr) != null)
        //                {
        //                    message = xr.SelectSingleNode("/ns:response/ns:body/ns:message", nsMgr).InnerText;
        //                }
        //                //把返回数据记入数据库
        //                int insresponse = new Qunar_noticeOrderConsumedData().InsResponse(message, data, qunar_requestid);
        //                if (insresponse == 0)
        //                {
        //                    TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "向去哪发送消费通知(orderid:" + partnerorderId + "),把返回数据录入数据库(insresponse)出错");
        //                }
        //                if (rlogid > 0)
        //                {
        //                    Qunar_ms_requestlog rlog = new Qunar_ms_requestlogData().GetLog(rlogid);
        //                    if (rlog == null)
        //                    {
        //                        TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "向去哪发送消费通知(orderid:" + partnerorderId + "),GetLog=null");
        //                    }
        //                    else
        //                    {
        //                        rlog.msg = "suc";
        //                        new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
        //                    }

        //                }
        //                else
        //                {
        //                    TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "向去哪发送消费通知(orderid:" + partnerorderId + "),rlogid=0");
        //                }
        //            }
        //            else
        //            {
        //                TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "向去哪发送消费通知(orderid:" + partnerorderId + "),生成请求数据出错");
        //            }

        //        }
        //        else
        //        {
        //            TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "向去哪发送消费通知(orderid:" + partnerorderId + "),去哪订单号为空");
        //        }
        //        #endregion

        //        #endregion
        //    }
        //    Label1.Text = "订单 " + orderid + " 操作成功";
        //}

        //private static string GetQunarRequestParam(string partnerorderId, string orderQuantity, string useQuantity, string consumeInfo, int comid, out int qunar_requestid, out int rlogid)
        //{


        //    //根据公司id得到对应的去哪信息
        //    B2b_company company_qunar = new B2bCompanyData().Getqunarbycomid(comid);
        //    if (company_qunar != null)
        //    {
        //        if (company_qunar.isqunar != 0 && company_qunar.qunar_pass != "" && company_qunar.qunar_username != "")
        //        {
        //            string frombase64requestxml = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
        //                                                   "<request xsi:schemaLocation=\"http://piao.qunar.com/2013/QMenpiaoRequestSchema QMRequestDataSchema-2.0.1.xsd\" xmlns=\"http://piao.qunar.com/2013/QMenpiaoRequestSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
        //                                                       "<header>" +
        //                                                           "<application>Qunar.Menpiao.Agent</application>" +
        //                                                           "<processor>SupplierDataExchangeProcessor</processor>" +
        //                                                           "<version>v2.0.1</version>" +
        //                                                           "<bodyType>NoticeOrderConsumedRequestBody</bodyType>" +
        //                                                           "<createUser>SupplierSystemName</createUser>" +
        //                                                           "<createTime>{0}</createTime>" +
        //                                                           "<supplierIdentity>{1}</supplierIdentity>" +
        //                                                       "</header>" +
        //                                                       "<body xsi:type=\"NoticeOrderConsumedRequestBody\">" +
        //                                                           "<orderInfo>" +
        //                                                               "<partnerorderId>{2}</partnerorderId>" +
        //                                                               "<orderQuantity>{3}</orderQuantity>" +
        //                                                               "<useQuantity>{4}</useQuantity>" +
        //                                                               "<consumeInfo>{5}</consumeInfo>" +
        //                                                           "</orderInfo>" +
        //                                                       "</body>" +
        //                                                   "</request>", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), company_qunar.qunar_username, partnerorderId, orderQuantity, useQuantity, consumeInfo);
        //            string requestxml = EncryptionExtention.ToBase64(frombase64requestxml);
        //            string signkey = company_qunar.qunar_pass;

        //            string requestsecurityType = "MD5";
        //            string requestsigned = EncryptionHelper.ToMD5(signkey + requestxml, "utf-8");

        //            string requestParam = "{\"data\":\"" + requestxml + "\",\"signed\":\"" + requestsigned + "\",\"securityType\":\"" + requestsecurityType + "\"}";

        //            //获得去哪订单
        //            string qunar_orderid = new Qunar_CreateOrderForBeforePaySyncData().GetQunarOrderId(partnerorderId);

        //            Qunar_ms_requestlog rlog = new Qunar_ms_requestlog
        //            {
        //                id = 0,
        //                method = "noticeOrderConsumed",
        //                requestParam = requestParam,
        //                base64data = requestxml,
        //                securityType = requestsecurityType,
        //                signed = requestsigned,
        //                frombase64data = frombase64requestxml,
        //                bodyType = "NoticeOrderConsumedRequestBody",
        //                createUser = "SupplierSystemName",
        //                supplierIdentity = "supplierIdentity",
        //                createTime = DateTime.Now,
        //                qunar_orderId = qunar_orderid,
        //                msg = ""
        //            };
        //            rlogid = new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
        //            if (rlogid == 0)
        //            {
        //                TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "向去哪发送消费通知(orderid:" + partnerorderId + "),录入请求记录(rlogid)出错");
        //            }
        //            //录入请求数据
        //            int requestid = new Qunar_noticeOrderConsumedData().InsRequest(partnerorderId, orderQuantity, useQuantity, consumeInfo);
        //            qunar_requestid = requestid;
        //            if (requestid == 0)
        //            {
        //                TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "向去哪发送消费通知(orderid:" + partnerorderId + "),录入请求数据(requestid)出错");
        //            }

        //            return requestParam;
        //        }
        //        else
        //        {
        //            TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "向去哪发送消费通知(orderid:" + partnerorderId + "),去哪配置信息不完整");
        //        }
        //    }
        //    else
        //    {
        //        TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "向去哪发送消费通知(orderid:" + partnerorderId + "),根据comid得到去哪信息出错");
        //    }


        //    qunar_requestid = 0;
        //    rlogid = 0;
        //    return "";

        //}

        #endregion
    }
}