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
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS2.PM.Service.WL.Data;

namespace ETS2.WebApp.Meituan_jk
{
    public partial class orderquery : System.Web.UI.Page
    {
        private Agent_company agentinfo;
        readonly string _requestParam;

        public orderquery()
        {
            _requestParam = GetRequestStreamString();
        }

        public string GetRequestStreamString()
        {
            StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Request.InputStream);
            string retstr = reader.ReadToEnd();
            return retstr;
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
                var response = new OrderQueryResponse();
                response.partnerId = int.Parse(PartnerId);
                response.code = 399;
                response.describe = "分销尚未配置美团合作商信息(合作商PartnerId:" + PartnerId + ")";

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
                var response = new OrderQueryResponse();
                response.partnerId = int.Parse(PartnerId);
                response.code = 399;
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

            string actionResult = Getorderquery(mlog);

            Response.Write(actionResult);

        }

        ///     产品查询
        public string Getorderquery(Meituan_reqlog mlog)
        {
            var response = new OrderQueryResponse();
            response.partnerId = int.Parse(agentinfo.mt_partnerId);
            try
            {
                var data = JsonConvert.DeserializeObject<OrderQueryRequest>(_requestParam);
                if (data.body == null)
                {
                    response.code = 399;
                    response.describe = "Body数据解析失败";
                    return EditMTlog_Err(response, mlog);
                }
                else
                {
                    OrderQueryRequestBody body = data.body;

                    string mtorderid = body.orderId.ToString();
                    //如果合作商订单Id不存在，则查询美团订单创建日志
                    if (body.partnerOrderId == "")
                    {
                        Meituan_reqlog mtOrderCrateSucLog = new Meituan_reqlogData().GetMtOrderCrateLogByMtorder(body.orderId.ToString(), "200");
                        body.partnerOrderId = mtOrderCrateSucLog.ordernum;
                    }
                    string ordernum = body.partnerOrderId; 

                    mlog.mtorderid = mtorderid;
                    mlog.ordernum = ordernum;

                     
                    int organization = agentinfo.Id;

                    //判断分销商查询订单是否是 自己发送的接口订单
                    Agent_requestlog mrequestlogg = new Agent_requestlogData().GetAgent_addorderlogByReq_seq(organization.ToString(), mtorderid);
                    if (mrequestlogg == null)
                    {
                        response.code = 301;
                        response.describe = "当前查询的订单不存在";
                        return EditMTlog_Err(response, mlog);
                    }
                    if (mrequestlogg.Is_dealsuc == 0)
                    {
                        response.code = 301;
                        response.describe = "当前查询的订单不存在";
                        return EditMTlog_Err(response, mlog);
                    }
                    if (mtorderid.Trim() == "")
                    {
                        response.code = 301;
                        response.describe = "美团订单号 不可为空";
                        return EditMTlog_Err(response, mlog);
                    }

                    B2b_order morder = new B2bOrderData().GetOrderByAgentRequestReqSeq(mtorderid);
                    if (morder != null)
                    {
                        if (morder.Pro_id > 0)
                        {
                            B2b_com_pro pro = new B2bComProData().GetProById(morder.Pro_id.ToString());
                            if (pro == null)
                            {
                                response.code = 399;
                                response.describe = "产品不存在";
                                return EditMTlog_Err(response, mlog);
                            }
                            else
                            {

                                #region 主要用途 判断是否是商家自己产品，如果为外来接口产品，暂时不售卖
                                //判断产品码来源 (4分销倒过来的产品 1系统自动生成产品  2倒码产品 判断分销是否是 自己发码；3外来接口产品暂时不售卖)
                                int prosourtype = pro.Source_type;
                                //if (prosourtype == 3)//外来接口产品,暂时只有阳光接口产品(需要手机号)
                                //{
                                //    //暂时只售卖商家自己产品,主要是产品有效期 需要另外通过外部接口获取，过于麻烦
                                //    response.code = 301;
                                //    response.describe = "暂时只可查询商家自己产品";
                                //    return EditMTlog_Err(response, mlog);

                                //}
                                if (prosourtype == 4)//分销导入产品; 
                                {
                                    int old_proid = new B2bComProData().GetOldproidById(morder.Pro_id.ToString());//绑定产品的原始编号
                                    if (old_proid == 0)
                                    {
                                        response.code = 399;
                                        response.describe = "分销导入产品的原始产品编号没有查到";
                                        return EditMTlog_Err(response, mlog);


                                    }
                                    else
                                    {
                                        prosourtype = new B2bComProData().GetProSource_typeById(old_proid.ToString());
                                        //if (prosourtype == 3)
                                        //{
                                        //    //暂时只售卖商家自己产品,主要是产品有效期 需要另外通过外部接口获取，过于麻烦
                                        //    response.code = 300;
                                        //    response.describe = "暂时只可查询商家自己产品";
                                        //    return EditMTlog_Err(response, mlog);

                                        //}
                                    }
                                }
                                #endregion

                                #region 产品有效期
                                //经过以上赋值prosourtype，只可能2个值:1系统自动生成码产品;2倒码产品
                                DateTime pro_start = pro.Pro_start;
                                DateTime pro_end = pro.Pro_end;
                                if (prosourtype == 2) //倒码产品
                                { }
                                if (prosourtype == 1) //系统自动生成码产品
                                {
                                    #region 产品有效期判定(微信模板--门票订单预订成功通知 中也有用到)
                                    string provalidatemethod = pro.ProValidateMethod;
                                    int appointdate = pro.Appointdata;
                                    int iscanuseonsameday = pro.Iscanuseonsameday;

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
                                        if (pro_end > pro.Pro_end)
                                        {
                                            pro_end = pro.Pro_end;
                                        }
                                    }
                                    else //按产品有效期
                                    {
                                        pro_end = pro.Pro_end;
                                    }

                                    //DateTime pro_start = modelcompro.Pro_start;
                                    DateTime nowday = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                                    if (iscanuseonsameday == 1)//当天可用  
                                    {
                                        if (nowday < pro_start)//当天日期小于产品起始日期
                                        {
                                            pro_start = pro.Pro_start;
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
                                            pro_start = pro.Pro_start;
                                        }
                                        else
                                        {
                                            pro_start = nowday.AddDays(1);
                                        }
                                    }
                                    #endregion
                                }
                                #endregion

                                #region  购买数量   可使用数量  使用数量 退票数量 出票时间 电子票号(列表)

                                string all_pno = "";//全部电子码
                                string keyong_pno = "";//可用电子码
                                string add_time = morder.U_subdate.ToString("yyyy-MM-dd HH:mm:ss");
                                int buy_num = 0;
                                int keyong_num = 0;
                                int consume_num = 0;
                                int tuipiao_num = morder.Cancelnum;

                                if (prosourtype == 1)//系统自动生成码产品
                                {
                                    int noworderid = morder.Id;
                                    //判断是否含有绑定订单
                                    if (morder.Bindingagentorderid > 0)
                                    {
                                        noworderid = morder.Bindingagentorderid;
                                    }

                                    //根据订单号得到电子票信息
                                    List<B2b_eticket> meticketlist = new B2bEticketData().GetEticketListByOrderid(noworderid);
                                    if (meticketlist == null)
                                    {
                                        response.code = 302;
                                        response.describe = "根据订单号查询电子票信息失败";
                                        return EditMTlog_Err(response, mlog);

                                    }
                                    else
                                    {
                                        if (meticketlist.Count == 0)
                                        {
                                            response.code = 302;
                                            response.describe = "根据订单号查询电子票信息失败";
                                            return EditMTlog_Err(response, mlog);

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
                                }
                                else if (prosourtype==3)
                                {
                                    if (pro.Serviceid == 4)
                                    { //如果是接口产品
                                        B2b_company commanage = B2bCompanyData.GetAllComMsg(pro.Com_id);
                                        WlGetProInfoDealRequestData wldata = new WlGetProInfoDealRequestData(commanage.B2bcompanyinfo.wl_PartnerId, commanage.B2bcompanyinfo.wl_userkey);
                                        var wlorderinfo = wldata.SearchWlOrderData(pro.Com_id, 0, "", morder.Id);
                                        if (wlorderinfo != null)
                                        {
                                            all_pno = wlorderinfo.vouchers;
                                            buy_num = wlorderinfo.quantity;
                                            keyong_num = wlorderinfo.quantity-wlorderinfo.usedQuantity;
                                            consume_num = wlorderinfo.usedQuantity; ;

                                        }
                                    }
                                
                                
                                }


                                else //倒码产品
                                { }
                                #endregion

                                #region 实名制类型 真是姓名 状态
                                string real_name_type = pro.Realnametype.ToString();
                                string real_name = morder.U_name;
                                string statusdesc = EnumUtils.GetName((OrderStatus)morder.Order_state);
                                #endregion


                                #region 手机号 根据订单号得到 分销商发送接口请求记录
                                string mobile = "";
                                Agent_requestlog mrequestlog = new Agent_requestlogData().GetAgent_addorderlogByOrderId(morder.Id.ToString(), 1);
                                if (mrequestlog == null)
                                {
                                    response.code = 399;
                                    response.describe = "根据订单号获得分销商接口发送请求记录失败";
                                    return EditMTlog_Err(response, mlog);

                                }
                                #endregion


                                int orderstate = morder.Order_state;
                                //                        
                                int mt_orderstatus = (int)Meituan_orderStatus.CreateFail;
                                if (orderstate == (int)OrderStatus.HasSendCode)
                                {
                                    mt_orderstatus = (int)Meituan_orderStatus.CreateSuc;
                                }
                                else if (orderstate == (int)OrderStatus.HasUsed)
                                {
                                    mt_orderstatus = (int)Meituan_orderStatus.PaySuc;
                                }
                                else 
                                {
                                   if(orderstate==(int)PayStatus.HasPay)
                                   {
                                       mt_orderstatus = (int)Meituan_orderStatus.PaySuc;
                                   }
                                   if (orderstate == (int)PayStatus.NotPay)
                                   {
                                       mt_orderstatus = (int)Meituan_orderStatus.PayFailed;
                                   }
                                   if (orderstate == (int)PayStatus.WaitPay)
                                   {
                                       mt_orderstatus = (int)Meituan_orderStatus.Paying;
                                   }
                                }
                                

                                response.code = 200;
                                response.describe = "success";

                            
                                /************现在只有一单一码的情况************
                               * required	凭证状态	见映射表<凭证码状态>
                               *  0	未使用 
                                  1	已使用 
                                  2	已退款  
                                  3	已废弃 对应的门票还未消费，但是此凭证码作废了 
                               * ************************/
                                List<Voucher> voucherlist = new List<Voucher>();

                                //电子票未使用
                                if (keyong_num == buy_num)
                                {
                                    Voucher vou = new Voucher
                                    {
                                        voucher = all_pno,
                                        voucherPics = "",
                                        voucherInvalidTime = morder.U_subdate.ToString("yyyy-MM-dd HH:mm:ss"),
                                        quantity = keyong_num,
                                        status = 0
                                    };
                                    voucherlist.Add(vou);
                                }
                                else 
                                {
                                    //电子票已使用
                                    if (tuipiao_num == 0)
                                    {
                                        string yanzhengtime = morder.U_subdate.ToString("yyyy-MM-dd HH:mm:ss");
                                        //得到电子票最近的一条验证成功日志
                                        B2b_eticket_log lastyanzhengsuclog = new B2bEticketLogData().GetlastyanzhengsuclogByPno(all_pno);
                                        if (lastyanzhengsuclog!=null)
                                        {
                                            yanzhengtime = lastyanzhengsuclog.Actiondate.ToString("yyyy-MM-dd HH:mm:ss");
                                        }
                                        Voucher vou = new Voucher
                                        {
                                            voucher = all_pno,
                                            voucherPics = "",
                                            voucherInvalidTime = morder.U_subdate.ToString("yyyy-MM-dd HH:mm:ss"),
                                            quantity = consume_num,
                                            status = 1
                                        };
                                        voucherlist.Add(vou);
                                    }
                                    else 
                                    {
                                        //电子票退票
                                        if (tuipiao_num != buy_num)
                                        {
                                            Voucher vou = new Voucher
                                            {
                                                voucher = all_pno,
                                                voucherPics = "",
                                                voucherInvalidTime = morder.Backtickettime.ToString("yyyy-MM-dd HH:mm:ss"),
                                                quantity = keyong_num,
                                                status = 2
                                            };
                                            voucherlist.Add(vou);
                                        }
                                        //电子票废弃
                                        else 
                                        {
                                            Voucher vou = new Voucher
                                            {
                                                voucher = all_pno,
                                                voucherPics = "",
                                                voucherInvalidTime = morder.Backtickettime.ToString("yyyy-MM-dd HH:mm:ss"),
                                                quantity = keyong_num,
                                                status = 3
                                            };
                                            voucherlist.Add(vou);
                                        }
                                    }
                                }
                               
                              

                              

                                response.body = new OrderQueryResponseBody
                                {
                                    orderId = body.orderId,
                                    partnerOrderId = body.partnerOrderId,
                                    orderStatus = mt_orderstatus,
                                    orderQuantity = morder.U_num,
                                    usedQuantity = (consume_num - morder.Cancelnum),
                                    refundedQuantity = morder.Cancelnum,
                                    voucherType = 2,
                                    voucherList = voucherlist
                                };

                                return EditMTlog_Err(response, mlog);
                            }

                        }
                        else
                        {
                            response.code = 301;
                            response.describe = "订单中产品不存在";

                            return EditMTlog_Err(response, mlog);


                        }
                    }
                    else
                    {
                        response.code = 301;
                        response.describe = "订单不存在";

                        return EditMTlog_Err(response, mlog);

                    }

                }
            }
            catch (Exception ex)
            {
                response.code = 399;
                response.describe = "异常错误";

                return EditMTlog_Err(response, mlog);
            }

        }
        //编辑日志
        public string EditMTlog_Err(OrderQueryResponse response, Meituan_reqlog mlog)
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