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
using ETS2.Common.Business;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.WebApp.Meituan_jk
{
    public partial class priceget : System.Web.UI.Page
    {
        private Agent_company agentinfo;
        readonly string _requestParam;

        public priceget()
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
                var response = new PriceResponse();
                response.partnerId = int.Parse(PartnerId);
                response.code = 300;
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
                var response = new PriceResponse();
                response.partnerId = int.Parse(PartnerId);
                response.code = 300;
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

            string actionResult = GetPriceList(mlog);

            Response.Write(actionResult);

        }

        ///    拉取库存价格日历
        public string GetPriceList(Meituan_reqlog mlog)
        {
            var response = new PriceResponse();
            response.partnerId = int.Parse(agentinfo.mt_partnerId);
            try
            {
                var data = JsonConvert.DeserializeObject<PriceRequest>(_requestParam);
                if (data.body == null)
                {
                    response.code = 300;
                    response.describe = "Body数据解析失败";
                }
                else
                {
                    PriceRequestBody body = data.body;

                    //todo 根据请求参数查询产品返回结果
                    string partnerDealId = body.partnerDealId;  
                    string startTime = body.startTime;
                    string endTime = body.endTime;

                    B2b_com_pro pro = new B2bComProData().GetProById(partnerDealId);
                    if (pro != null)
                    { 
                        List<PriceResponseBody> list = new List<PriceResponseBody>();
                        int days = (DateTime.Parse(endTime) - DateTime.Parse(startTime)).Days;
                        //当天不可用   
                        if (pro.Iscanuseonsameday == 0)
                        {
                            //当天出票不可用，并且拉取日期是当天，则不返回当天
                            if (startTime == DateTime.Now.ToString("yyyy-MM-dd"))
                            {
                                for (int i = 1; i <= days; i++)
                                {
                                    list.Add(new PriceResponseBody
                                    {
                                        partnerDealId = partnerDealId,
                                        date = DateTime.Parse(startTime).AddDays(i).ToString("yyyy-MM-dd"),
                                        //暂时美团接口只买票务，其他类型产品先不考虑
                                        marketPrice = pro.Face_price,
                                        mtPrice = pro.Advise_price,
                                        settlementPrice = GetSettlementPrice(agentinfo.Id,partnerDealId),
                                        stock = pro.Ispanicbuy == 0 ? 100000000 : pro.Limitbuytotalnum//库存	无限制 stock = 100000000
                                        
                                    });
                                }
                            }
                            else 
                            {
                                for (int i = 0; i <= days; i++)
                                {
                                    list.Add(new PriceResponseBody
                                    {
                                        partnerDealId = partnerDealId,
                                        date = DateTime.Parse(startTime).AddDays(i).ToString("yyyy-MM-dd"),
                                        //暂时美团接口只买票务，其他类型产品先不考虑
                                        marketPrice = pro.Face_price,
                                        mtPrice = pro.Advise_price,
                                        settlementPrice = GetSettlementPrice(agentinfo.Id, partnerDealId),
                                        stock = pro.Ispanicbuy == 0 ? 100000000 : pro.Limitbuytotalnum//库存	无限制 stock = 100000000
                                        
                                    });
                                }
                            } 
                        }
                        else
                        {
                            for (int i = 0; i <= days; i++)
                            {

                                list.Add(new PriceResponseBody
                                {
                                    partnerDealId = partnerDealId,
                                    date = DateTime.Parse(startTime).AddDays(i).ToString("yyyy-MM-dd"),
                                    //暂时美团接口只买票务，其他类型产品先不考虑
                                    marketPrice = pro.Face_price,
                                    mtPrice = pro.Advise_price,
                                    settlementPrice = GetSettlementPrice(agentinfo.Id, partnerDealId),
                                    stock = pro.Ispanicbuy == 0 ? 100000000 : pro.Limitbuytotalnum//库存	无限制 stock = 100000000
                                        
                                });
                            }
                        }


                        response.code = 200;
                        response.describe = "success";
                        response.partnerId = int.Parse(agentinfo.mt_partnerId); 
                        //response.body=new List<PriceResponseBody>

                        response.body = list;
                    }
                    else
                    {
                        response.code = 300;
                        response.describe = "查询库存价格日历异常.";
                    }

                }
            }
            catch (Exception ex)
            {
                response.code = 300;
                response.describe = "异常错误";
            }
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

        private decimal GetSettlementPrice(int agentid, string partnerDealId)
        {
            decimal settlementPrice = 0;
            //结算价
            Agent_warrant warrant = new Agent_companyData().GetAgentWarrantInfo(agentinfo.Id,  partnerDealId.ConvertTo<int>(0));
            if (warrant != null)
            {
                settlementPrice = new B2bComProData().GetAgentPrice(partnerDealId.ConvertTo<int>(0), warrant.Warrant_level);
            }
            return settlementPrice;
        }
    }
}