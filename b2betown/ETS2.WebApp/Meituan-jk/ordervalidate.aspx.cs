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


namespace ETS2.WebApp.Meituan_jk
{
    /// <summary>
    /// 产品预约-新接口已经弃用
    /// </summary>
    public partial class ordervalidate : System.Web.UI.Page
    {
        readonly string _requestParam;

        public ordervalidate()
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
            //#region 记入日志表Meituan_reqlog
            //string reqip = CommonFunc.GetRealIP();
            //Meituan_reqlog mlog = new Meituan_reqlog
            //{
            //    id = 0,
            //    reqstr = _requestParam,
            //    subtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            //    respstr = "",
            //    resptime = "",
            //    code = "",
            //    describe = "",
            //    req_type = "",
            //    sendip = reqip
            //};
            //int logid = new Meituan_reqlogData().EditReqlog(mlog);
            //mlog.id = logid;
            //#endregion

            //#region 签名验证
            //string date = System.Web.HttpContext.Current.Request.Headers.Get("Date");
            //string PartnerId = System.Web.HttpContext.Current.Request.Headers.Get("PartnerId");
            //string Authorization = System.Web.HttpContext.Current.Request.Headers.Get("Authorization");
            //string requestMethod = System.Web.HttpContext.Current.Request.HttpMethod;
            //string URI = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath;
            ////authorization 形式: "MWS" + " " + client + ":" + sign;
            //string mtSign = Authorization.Substring(Authorization.IndexOf(":") + 1);

            //string beforeSign = requestMethod + " " + URI + "\n" + date;
            //string afterSign = new MeiTuanInter().GetSign(beforeSign);
            ////判断签名是否正确
            //if (afterSign != mtSign)
            //{
            //    TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\MTlog.txt", "签名错误:mtSign-" + mtSign + "  meSign-" + afterSign);
            //    return;
            //}
            //#endregion

            //mlog.req_type = URI;

            //string actionResult = Getordervalidate(mlog);

            //Response.Write(actionResult);


        }

        /////     产品预约
        //public string Getordervalidate(Meituan_reqlog mlog)
        //{
        //    var response = new OrderValidateResponse();
        //    response.partnerId = int.Parse(new MeiTuanInter().mt_partnerId);
        //    try
        //    {
        //        var data = JsonConvert.DeserializeObject<OrderValidateRequest>(_requestParam);
        //        if (data.body == null)
        //        {
        //            response.code = 300;
        //            response.describe = "Body数据解析失败";
        //            return EditMTlog_Order(response, mlog);
        //        }
        //        else
        //        {
        //            OrderValidateRequestBody body = data.body;

        //            string product_num = body.partnerDealId;
        //            string num = body.quantity.ToString();
                 

        //            //todo 根据请求参数查询产品返回结果
        //            B2b_com_pro pro = new B2bComProData().GetProById(product_num);
        //            #region 产品信息
        //            if (pro != null)
        //            {
        //                //根据机构号获得机构(分销商)信息
        //                Agent_company agentcompany = new AgentCompanyData().GetAgentCompanyByName("美团旅游");
        //                if (agentcompany == null)
        //                {
        //                    response.code = 300;
        //                    response.describe = "不存在 美团旅游 分销账户";
        //                    return EditMTlog_Order(response, mlog);
        //                }

        //                Agent_company agentwarrantinfo = AgentCompanyData.GetAgentWarrant(agentcompany.Id, pro.Com_id);

        //                if (agentwarrantinfo != null)
        //                {
        //                    int  warrantid = agentwarrantinfo.Warrantid;
        //                    int  Warrant_type = agentwarrantinfo.Warrant_type;//支付类型分销 1出票扣款 2验码扣款 
        //                    int Warrant_level = agentwarrantinfo.Warrant_level;
        //                    if (agentwarrantinfo.Warrant_state == 0)
        //                    {
        //                        response.code = 300;
        //                        response.describe = "产品还没有得到商户授权";
        //                        return EditMTlog_Order(response, mlog); 
        //                    }
        //                }
        //                else
        //                {
        //                    response.code = 300;
        //                    response.describe = "产品还没有得到商户授权";
        //                    return EditMTlog_Order(response, mlog); 
        //                }





        //                #region 暂时对外接口只支持票务产品
        //                if (pro.Server_type != 1)
        //                {
        //                    response.code = 300;
        //                    response.describe = "暂时对外接口只支持票务产品，其他产品请到分销后台提单";
        //                    return EditMTlog_Order(response, mlog);
        //                }
        //                #endregion
        //                #region 价格(建议价)效验，保证美团抓到的是最新价格
        //                string advice_price = body.sellPrice.ToString("f0");
        //                if(pro.Advise_price.ToString("f0")!=advice_price)
        //                {
        //                    response.code = 300;
        //                    response.describe = "价格效验失败，请重新拉取价格库存日历";
        //                    return EditMTlog_Order(response, mlog);
        //                }
        //                #endregion

        //                #region 多规格产品编码格式验证
        //                int speciid = 0;
        //                //判断产品编号是否符合多规格产品特点：例如 2503-1
        //                if (product_num.IndexOf("-") > -1)
        //                {
        //                    speciid = product_num.Substring(product_num.IndexOf("-") + 1).ConvertTo<int>(0);
        //                    if (speciid == 0)
        //                    {
        //                        response.code = 300;
        //                        response.describe = "多规格产品编码格式有误";
        //                        return EditMTlog_Order(response, mlog);
        //                    }
        //                    product_num = product_num.Substring(0, product_num.IndexOf("-"));
        //                }
        //                #endregion

        //                #region 产品编码格式有误
        //                if (product_num.ConvertTo<int>(0) == 0)
        //                {
        //                    response.code = 300;
        //                    response.describe = "产品编码格式有误";
        //                    return EditMTlog_Order(response, mlog);
        //                }
        //                #endregion

        //                #region  购买数量格式有误
        //                if (num.ConvertTo<int>(0) == 0)
        //                {
        //                    response.code = 300;
        //                    response.describe = "购买数量格式有误";
        //                    return EditMTlog_Order(response, mlog);
        //                }
        //                #endregion

        //                #region  产品限购则需要判断 限购数量 是否足够
        //                if (pro.Ispanicbuy != 0)
        //                {
        //                    //最多可购买数量
        //                    int zuiduo_canbuynum = pro.Limitbuytotalnum;
        //                    if (int.Parse(num) > zuiduo_canbuynum)
        //                    {
        //                        response.code = 300;
        //                        response.describe = "产品库存不足";
        //                        return EditMTlog_Order(response, mlog);
        //                    }
        //                }
        //                #endregion

        //                #region 产品已暂停
        //                if (pro.Pro_state == 0)
        //                {
        //                    response.code = 300;
        //                    response.describe = "产品已暂停";
        //                    return EditMTlog_Order(response, mlog);
        //                }
        //                #endregion
        //                #region 产品已过期
        //                if (pro.Pro_end < DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
        //                {
        //                    response.code = 300;
        //                    response.describe = "产品已过期";
        //                    return EditMTlog_Order(response, mlog);
        //                }
        //                #endregion

        //                #region   产品是否需要预约:需要预约则最晚预约时间是 游玩前一天的18点
        //                if (pro.isneedbespeak == 1)
        //                {
        //                    DateTime visitdate = DateTime.Parse(body.visitDate);
        //                    DateTime nowdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

        //                    //必须提前一天预约
        //                    if (nowdate > visitdate)
        //                    {
        //                        //当天余位验证，票务暂时不用加，不过旅游大巴时需要验证
        //                    }
        //                    else
        //                    {
        //                        response.code = 300;
        //                        response.describe = "产品需要提前一天预约";
        //                        return EditMTlog_Order(response, mlog);
        //                    }
        //                    #region 注释内容
        //                    ////最晚预约时间
        //                    //DateTime zuiwan_bespeaktime = DateTime.Parse(visitdate.AddDays(-1).ToString("yyyy-MM-dd 18:00:00"));
        //                    ////当前时间晚于最晚预约时间，则预约失败
        //                    //if (nowdate > zuiwan_bespeaktime)
        //                    //{
        //                    //    response.code = 300;
        //                    //    response.describe = "当前时间已经超过最晚预约时间(" + zuiwan_bespeaktime + ")";
        //                    //    return EditMTlog_Order(response, mlog);
        //                    //}
        //                    #endregion
        //                }
        //                #endregion
        //                #region  是否有使用限制
        //                if (pro.Iscanuseonsameday == 0)//1:当天出票可用 ;2:2小时内出票不可用;0:当天出票不可用
        //                {
        //                    DateTime visitdate = DateTime.Parse(body.visitDate);//游玩日期:2012-12-12 格式要求：yyyy-MM-dd
        //                    //DateTime nowdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        //                    if (DateTime.Parse(visitdate.ToString("yyyy-MM-dd")) > DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
        //                    {
        //                    }
        //                    else
        //                    {
        //                        response.code = 300;
        //                        response.describe = "预定日期至少在游玩日期之前一天";
        //                        return EditMTlog_Order(response, mlog);
        //                    }
        //                }
        //                #endregion


        //                response.code = 200;
        //                response.describe = "预约成功";
        //                response.partnerId = int.Parse(new MeiTuanInter().mt_partnerId);
        //                response.body = new OrderValidateResponseBody
        //                {
        //                    orderStatus = (int)Meituan_orderStatus.ValidateSuc
        //                };

        //                return EditMTlog_Order(response, mlog);
        //            }
        //            else
        //            {
        //                response.code = 300;
        //                response.describe = "产品不存在";
        //                return EditMTlog_Order(response, mlog);
        //            }
        //            #endregion
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.code = 300;
        //        response.describe = "查询产品异常";

        //        return EditMTlog_Order(response, mlog);
        //    }

        //}
        ////编辑日志
        //public string EditMTlog_Order(OrderValidateResponse response, Meituan_reqlog mlog)
        //{
        //    string json = JsonConvert.SerializeObject(response);

        //    #region 把处理结果录入数据库
        //    mlog.respstr = json;
        //    mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //    mlog.code = response.code.ToString();
        //    mlog.describe = response.describe;
        //    new Meituan_reqlogData().EditReqlog(mlog);
        //    #endregion

        //    return json;
        //}
    }
}