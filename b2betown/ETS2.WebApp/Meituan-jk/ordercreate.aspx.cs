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
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS.JsonFactory;
using System.Xml;
using ETS2.PM.Service.WL.Data;
using ETS2.PM.Service.WL.Model;

namespace ETS2.WebApp.Meituan_jk
{
    public partial class ordercreate : System.Web.UI.Page
    {
        private static object lockobj = new object();

        private Agent_company agentinfo;
        readonly string _requestParam;

        public ordercreate()
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
            #region 记入日志表Meituan_reqlog
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
                var response = new OrderCreateResponse();
                response.partnerId = int.Parse(PartnerId);
                response.code = 499;
                response.describe = "分销尚未配置美团合作商信息(PartnerId:" + PartnerId + ")";

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
                var response = new OrderCreateResponse();
                response.partnerId = int.Parse(PartnerId);
                response.code = 499;
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

            string actionResult = Getordercreate(mlog);

            Response.Write(actionResult);

        }

        ///     产品订单创建
        public string Getordercreate(Meituan_reqlog mlog)
        {
            lock (lockobj)
            {
                var response = new OrderCreateResponse();
                response.partnerId = int.Parse(agentinfo.mt_partnerId);
                try
                {
                    var data = JsonConvert.DeserializeObject<OrderCreateRequest>(_requestParam);
                    if (data.body == null)
                    {
                        response.code = 499;
                        response.describe = "Body数据解析失败";
                        return EditMTlog_Order(response, mlog);
                    }
                    else
                    {
                        OrderCreateRequestBody body = data.body;

                        string product_num = body.partnerDealId;
                        string num = body.quantity.ToString();
                        mlog.mtorderid=body.orderId.ToString(); 

                        //todo 根据请求参数查询产品返回结果
                        B2b_com_pro pro = new B2bComProData().GetProById(product_num);
                        #region 产品信息
                        if (pro != null)
                        {

                            #region 分销授权信息判断
                            Agent_company agentwarrantinfo = AgentCompanyData.GetAgentWarrant(agentinfo.Id, pro.Com_id);

                            if (agentwarrantinfo != null)
                            {
                                int warrantid = agentwarrantinfo.Warrantid;
                                int Warrant_type = agentwarrantinfo.Warrant_type;//支付类型分销 1出票扣款 2验码扣款 
                                int Warrant_level = agentwarrantinfo.Warrant_level;
                                if (agentwarrantinfo.Warrant_state == 0)
                                {
                                    response.code = 499;
                                    response.describe = "商户尚未授权此分销";
                                    return EditMTlog_Order(response, mlog);
                                }
                            }
                            else
                            {
                                response.code = 499;
                                response.describe = "分销还没有得到商户授权";
                                return EditMTlog_Order(response, mlog);
                            }
                            #endregion

                            #region 暂时对外接口只支持票务产品
                            if (pro.Server_type != 1)
                            {
                                response.code = 499;
                                response.describe = "暂时对外接口只支持票务产品，其他产品请到分销后台提单";
                                return EditMTlog_Order(response, mlog);
                            }
                            #endregion

                            #region 价格(建议价)效验，保证美团抓到的是最新价格
                            string advice_price = body.unitPrice.ToString("f0");
                            if (pro.Advise_price.ToString("f0") != advice_price)
                            {
                                response.code = 410;
                                response.describe = "价格(建议价)效验失败，请重新拉取价格库存日历";
                                return EditMTlog_Order(response, mlog);
                            }
                            #endregion

                            #region 价格(分销价)效验，保证美团抓到的是最新的结算价
                            if (agentwarrantinfo.Warrant_level == 1)
                            {
                                if (body.buyPrice.ToString("f0") != pro.Agent1_price.ToString("f0"))
                                {
                                    response.code = 410;
                                    response.describe = "价格(分销价)效验失败，请重新拉取价格库存日历";
                                    return EditMTlog_Order(response, mlog);
                                }
                            }
                            if (agentwarrantinfo.Warrant_level == 2)
                            {
                                if (body.buyPrice.ToString("f0") != pro.Agent2_price.ToString("f0"))
                                {
                                    response.code = 410;
                                    response.describe = "价格(分销价)效验失败，请重新拉取价格库存日历";
                                    return EditMTlog_Order(response, mlog);
                                }
                            }
                            if (agentwarrantinfo.Warrant_level == 3)
                            {
                                if (body.buyPrice.ToString("f0") != pro.Agent3_price.ToString("f0"))
                                {
                                    response.code = 410;
                                    response.describe = "价格(分销价)效验失败，请重新拉取价格库存日历";
                                    return EditMTlog_Order(response, mlog);
                                }
                            }
                            #endregion

                            #region 多规格产品编码格式验证
                            int speciid = 0;
                            //判断产品编号是否符合多规格产品特点：例如 2503-1
                            if (product_num.IndexOf("-") > -1)
                            {
                                speciid = product_num.Substring(product_num.IndexOf("-") + 1).ConvertTo<int>(0);
                                if (speciid == 0)
                                {
                                    response.code = 410;
                                    response.describe = "多规格产品编码格式有误";
                                    return EditMTlog_Order(response, mlog);
                                }
                                product_num = product_num.Substring(0, product_num.IndexOf("-"));
                            }
                            #endregion

                            #region 产品编码格式有误
                            if (product_num.ConvertTo<int>(0) == 0)
                            {
                                response.code = 410;
                                response.describe = "产品编码格式有误";
                                return EditMTlog_Order(response, mlog);
                            }
                            #endregion

                            #region  购买数量格式有误
                            if (num.ConvertTo<int>(0) == 0)
                            {
                                response.code = 410;
                                response.describe = "购买数量格式有误";
                                return EditMTlog_Order(response, mlog);
                            }
                            #endregion

                            #region  产品限购则需要判断 限购数量 是否足够
                            if (pro.Ispanicbuy != 0)
                            {
                                //最多可购买数量
                                int zuiduo_canbuynum = pro.Limitbuytotalnum;
                                if (int.Parse(num) > zuiduo_canbuynum)
                                {
                                    response.code = 420;
                                    response.describe = "产品库存不足";
                                    return EditMTlog_Order(response, mlog);
                                }
                            }
                            #endregion

                            #region 产品已暂停
                            if (pro.Pro_state == 0)
                            {
                                response.code = 421;
                                response.describe = "产品已暂停";
                                return EditMTlog_Order(response, mlog);
                            }
                            #endregion

                            #region 产品已过期
                            if (pro.Pro_end < DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
                            {
                                response.code = 421;
                                response.describe = "产品已过期";
                                return EditMTlog_Order(response, mlog);
                            }
                            #endregion

                            #region   产品是否需要预约:需要预约则最晚预约时间是 游玩前一天的18点
                            if (pro.isneedbespeak == 1)
                            {
                                if (body.travelDate != "")
                                {
                                    DateTime visitdate = DateTime.Parse(body.travelDate);
                                    DateTime bookdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

                                    //必须提前一天预约
                                    if (bookdate >= visitdate)
                                    {
                                        response.code = 422;
                                        response.describe = "产品需要提前一天预约";
                                        return EditMTlog_Order(response, mlog);
                                    }
                                }
                            }
                            #endregion

                            #region  是否有使用限制
                            if (pro.Iscanuseonsameday == 0)//1:当天出票可用 ;2:2小时内出票不可用;0:当天出票不可用
                            {
                                if (body.travelDate != "")
                                {
                                    DateTime visitdate = DateTime.Parse(body.travelDate);//游玩日期:2012-12-12 格式要求：yyyy-MM-dd

                                    DateTime bookdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                                    if (bookdate >= visitdate)
                                    {
                                        response.code = 422;
                                        response.describe = "预定日期至少在游玩日期之前一天";
                                        return EditMTlog_Order(response, mlog);
                                    }
                                }

                            }
                            #endregion


                            
                           
                             Meituan_reqlog mtOrderCrateSucLog = new Meituan_reqlogData().GetMtOrderCrateLogByMtorder(body.orderId.ToString(),"200");
                             #region 美团订单成功创建过
                             if (mtOrderCrateSucLog!=null)
                            {
                                response.code = 200;
                                response.describe = "订单创建成功";
                                response.partnerId = int.Parse(agentinfo.mt_partnerId);
                                response.body = new OrderCreateResponseBody
                                {
                                    partnerOrderId = mtOrderCrateSucLog.ordernum
                                };

                                mlog.ordernum = mtOrderCrateSucLog.ordernum;
                                return EditMTlog_Order(response, mlog);
                            }
                            #endregion
                            #region 美团订单没有成功创建过
                            else 
                            {
                                int isInterfaceSub = 1;//是否是电子票接口提交的订单:0.否;1.是
                                string ordertype = "1";//1.出票2.充值
                                int orderid = 0;

                                string real_name = "";
                                string mobile = "";
                                string use_date = body.travelDate;
                                List<BaseVisitor> visitorlist = body.visitors;
                                if (visitorlist.Count > 0)
                                {
                                    foreach (BaseVisitor info in visitorlist)
                                    {
                                        if (info != null)
                                        {
                                            real_name = info.name;
                                            mobile = info.mobile;
                                            break;
                                        }
                                    }
                                }
                                //创建一笔未支付订单
                                string rdata = OrderJsonData.CreateNopayOrder(agentinfo.Id, product_num, ordertype, num, real_name, mobile, use_date, isInterfaceSub, out orderid, speciid);
                                XmlDocument retdoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + rdata + "}");
                                XmlElement retroot = retdoc.DocumentElement;
                                string rtype = retroot.SelectSingleNode("type").InnerText;
                                string rmsg = retroot.SelectSingleNode("msg").InnerText;
                                if (rtype == "100")//创建订单成功
                                {

                                    //针对美团因为创建未支付订单 是单写的，所以同时创建一笔wl订单
                                    #region 万龙接口订单
                                    if (pro.Source_type == 3 && pro.Serviceid == 4)
                                    {
                                        try
                                        {
                                            B2b_company commanage = B2bCompanyData.GetAllComMsg(pro.Com_id);
                                            WlGetProInfoDealRequestData wldata = new WlGetProInfoDealRequestData(commanage.B2bcompanyinfo.wl_PartnerId, commanage.B2bcompanyinfo.wl_userkey);

                                            WlDealResponseBody WlDealinfo = wldata.SelectonegetWlProDealData(pro.Service_proid, pro.Com_id);
                                            if (WlDealinfo == null)
                                            {
                                                response.code = 499;
                                                response.describe = "订单创建失败1";
                                                return EditMTlog_Order(response, mlog);
                                            }
                                            double toal = WlDealinfo.marketPrice * int.Parse(num);
                                            string tavedate ="";


                                            var createwlorder = wldata.wlOrderCreateRequest_json(int.Parse(commanage.B2bcompanyinfo.wl_PartnerId), real_name, mobile, orderid.ToString(), product_num.ToString(), WlDealinfo.proID, WlDealinfo.settlementPrice, WlDealinfo.marketPrice, toal, int.Parse(num), tavedate);//

                                            var wlcreate = wldata.wlOrderCreateRequest_data(createwlorder, pro.Com_id);
                                            if (wlcreate.IsSuccess == true)
                                            {
                                                //wl订单创建成功
                                            }
                                            else
                                            {
                                                //return JsonConvert.SerializeObject(new { type = 1, msg = "wl接口创建订单失败1" });
                                                response.code = 499;
                                                response.describe = "订单创建失败1";
                                                return EditMTlog_Order(response, mlog);
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            response.code = 499;
                                            response.describe = "订单创建失败" + ex.Message;
                                            return EditMTlog_Order(response, mlog);
                                        }
                                    }
                                    #endregion


                                    response.code = 200;
                                    response.describe = "订单创建成功";
                                    response.partnerId = int.Parse(agentinfo.mt_partnerId);
                                    response.body = new OrderCreateResponseBody
                                    {
                                        partnerOrderId = orderid.ToString()
                                    };

                                    mlog.ordernum = orderid.ToString();
                                    return EditMTlog_Order(response, mlog);
                                }
                                else
                                {
                                    response.code = 499;
                                    response.describe = "订单创建失败";
                                    return EditMTlog_Order(response, mlog);
                                }
                            }
                            #endregion 

                        }
                        else
                        {
                            response.code = 421;
                            response.describe = "产品不存在";
                            return EditMTlog_Order(response, mlog);
                        }
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    response.code = 499;
                    response.describe = "异常错误" + ex;

                    return EditMTlog_Order(response, mlog);
                }
            }
        }
        //编辑日志
        public string EditMTlog_Order(OrderCreateResponse response, Meituan_reqlog mlog)
        {
            string json = JsonConvert.SerializeObject(response);

            #region 把处理结果录入数据库
            mlog.respstr = json;
            mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            mlog.code = response.code.ToString();
            mlog.describe = response.describe;
            new Meituan_reqlogData().EditReqlog(mlog);
            #endregion

            return json;
        }
    }
}