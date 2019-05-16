using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.CRM.Service.CRMService.Modle;
using System.IO;
using ETS.Framework;
using ETS2.PM.Service.Meituan.Model;
using ETS2.PM.Service.Meituan.Data;
using ETS2.CRM.Service.CRMService.Data;
using Newtonsoft.Json;


namespace ETS2.WebApp.Meituan_jk
{
    public partial class balanceget : System.Web.UI.Page
    {

        /// <summary>
        /// 美团账户余额查询
        /// </summary>
        private Agent_company agentinfo;
        readonly string _requestParam;

        public balanceget()
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
                var response = new MtpBalanceResponse();
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
                var response = new MtpBalanceResponse();
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

            string actionResult = GetBalance(mlog);

            Response.Write(actionResult);
        }

        private string GetBalance(Meituan_reqlog mlog)
        {
            var response = new MtpBalanceResponse();
            response.partnerId = agentinfo.mt_partnerId.ConvertTo<int>(0);
            var data = JsonConvert.DeserializeObject<MtpBalanceRequest>(_requestParam);
            if (data.partnerId == 0)
            {
                response.code = 300;
                response.describe = "Body数据解析失败";
            }
            else
            {
                MtpBalanceRequestBody body = data.body;
                //因为我们系统没有给单个产品授权过额度,所以接收的请求body中产品id不用处理

                //得到分销的授权信息
                List<Agent_warrant> warrantinfolist = new AgentCompanyData().GetAgentWarrantList(agentinfo.Id, "1");
                if (warrantinfolist.Count == 1)
                {
                    //预付款账户余额	单位为分, 非预付款商家, 此值返回-1不要传0
                    int prepaidAccountBalance = -1;
                    //授信账户余额	    单位为分, 无授信账户, 此值返回-1不要传0
                    int creditAccountBalance = -1;
                    foreach (Agent_warrant rinfo in warrantinfolist)
                    { 
                        prepaidAccountBalance = int.Parse(rinfo.Imprest.ToString("f0")) == 0 ? -1 : int.Parse(rinfo.Imprest.ToString("f0"));
                         
                        creditAccountBalance = int.Parse(rinfo.Credit.ToString("f0")) == 0 ? -1 : int.Parse(rinfo.Credit.ToString("f0"));

                    }


                    List<MtpBalanceResponseBody> blist = new List<MtpBalanceResponseBody>();
                    if (data.body.partnerDealIds.Length > 0)
                    {
                        foreach (string dealid in data.body.partnerDealIds)
                        {
                            if (dealid != "")
                            {
                                blist.Add(new MtpBalanceResponseBody
                                {
                                    prepaidAccountBalance = prepaidAccountBalance,
                                    creditAccountBalance = creditAccountBalance,
                                    partnerDealId = dealid
                                });
                            }
                        }
                    }
                    else
                    {
                        blist.Add(new MtpBalanceResponseBody
                        {
                            prepaidAccountBalance = prepaidAccountBalance,
                            creditAccountBalance = creditAccountBalance,
                            partnerDealId = ""
                        });
                    }


                    response.code = 200;
                    response.describe = "successful";
                    response.partnerId = int.Parse(agentinfo.mt_partnerId);
                    response.body = blist;
                }
                else
                {
                    response.code = 300;
                    response.describe = "分销授权信息有误";
                }
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
    }
}