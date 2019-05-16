using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using ETS.Framework;
using ETS2.PM.Service.Meituan.Model;
using ETS2.PM.Service.Meituan.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle.Enum;
using Newtonsoft.Json;
using System.IO;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS.JsonFactory;

namespace ETS2.WebApp.Meituan_jk
{
    public partial class dealget : System.Web.UI.Page
    {
        private Agent_company agentinfo;
        readonly string _requestParam;

        public dealget()
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
                var response = new DealResponse();
                response.partnerId = int.Parse(PartnerId);
                response.code = 300;
                response.describe = "分销尚未配置美团合作商信息(合作商PartnerId:" + PartnerId + ")";

                string json = JsonConvert.SerializeObject(response);

                //把处理结果录入日志
                mlog.respstr = mlog.respstr+json;
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
                var response = new DealResponse();
                response.partnerId = int.Parse(PartnerId);
                response.code = 300;
                response.describe = "签名验证失败";

                string json = JsonConvert.SerializeObject(response);

                //把处理结果录入日志
                mlog.respstr = mlog.respstr+json;
                mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                mlog.code = response.code.ToString();
                mlog.describe = response.describe;
                new Meituan_reqlogData().EditReqlog(mlog);

                LogHelper.RecordSelfLog("Error", "meituan", "拉取产品 签名错误 mtSign-" + mtSign + "  meSign-" + afterSign);

                Response.Write(json);
                return;
            }
            #endregion

            string actionResult = GetProductList(mlog);

            Response.Write(actionResult);

        }



        ///     抓产品
        public string GetProductList(Meituan_reqlog mlog)
        {
            var response = new DealResponse();
            response.partnerId = agentinfo.mt_partnerId.ConvertTo<int>(0);
            try
            {
                var data = JsonConvert.DeserializeObject<DealRequest>(_requestParam);
                if (data.body == null)
                {
                    response.code = 300;
                    response.describe = "Body数据解析失败";
                }
                else
                {
                    DealRequestBody body = data.body;

                    //todo 根据请求参数查询产品返回结果
                    if (body.method.Equals("multi", StringComparison.OrdinalIgnoreCase))
                    {
                        #region 获取产品列表
                        int totalcount = 0;
                        List<DealResponseBody> list = new DealResponseData().GetDealResponseBody(out totalcount, agentinfo, body.method, body.partnerDealIds);
                        foreach (DealResponseBody rbody in list)
                        {
                            B2b_com_project pro_project = new B2b_com_projectData().GetProject(rbody.partnerPoiId.ConvertTo<int>(0));
                            if (pro_project != null)
                            {
                                List<ServicePhone> servicePhonelist = new List<ServicePhone>();
                                servicePhonelist.Add(new ServicePhone
                                {
                                    phone = pro_project.Mobile.Split(' ')[0],
                                    startHour = "08",
                                    startMin = "00",
                                    endHour = "17",
                                    endMin = "00"
                                });
                                //服务电话
                                rbody.servicePhones = servicePhonelist;

                                //入园地址
                                rbody.getInAddresses = new string[] { pro_project.Address };
                            }
                            //结算价
                            Agent_warrant warrant = new Agent_companyData().GetAgentWarrantInfo(agentinfo.Id, rbody.partnerDealId.ConvertTo<int>(0));
                            if (warrant != null)
                            {
                                rbody.settlementPrice = new B2bComProData().GetAgentPrice(rbody.partnerDealId.ConvertTo<int>(0), warrant.Warrant_level);
                            }
                            /*******************************
                             * 产品图片(最多5张)=产品主图片+产品子图片（前4张）
                             * ******************************/
                            List<DealImageInfo> imgs = rbody.dealImageInfos;
                            int topnum=4;
                            List<int> childimgarr = new B2bComProData().GetProChildImgArr(rbody.partnerDealId.ConvertTo<int>(0), topnum);
                            if (childimgarr.Count > 0)
                            {
                                foreach (int iimg in childimgarr)
                                {
                                    imgs.Add(new DealImageInfo
                                    {
                                        imageName = "",
                                        imageUrl = iimg.ToString(),
                                        frontImage = false
                                    });
                                } 
                            }

                            foreach(DealImageInfo iinfo in imgs)
                            {
                              var iimgurl = FileSerivce.GetImgUrl(iinfo.imageUrl.ConvertTo<int>(0));
                              iinfo.imageUrl = iimgurl;
                            }
                            rbody.dealImageInfos = imgs;
                        }

                        if (list.Count > 0)
                        {
                            response.code = 200;
                            response.describe = "success";
                            response.partnerId = int.Parse(agentinfo.mt_partnerId);
                            response.totalSize = totalcount;
                            response.body = list;
                        }
                        else
                        {
                            response.code = 300;
                            response.describe = "获取deal失败";
                        }

                        #endregion
                    }
                    else if (body.method.Equals("page", StringComparison.OrdinalIgnoreCase))
                    {
                        int pageindex = body.currentPage, pagesize = body.pageSize;
                        if (pageindex <= 0)
                        {
                            pageindex = 1;
                        }
                        if (pagesize <= 0 || pagesize > 20)
                        {
                            pagesize = 20;
                        }
                        #region 获取产品列表
                        int totalcount = 0;
                        List<DealResponseBody> list = new DealResponseData().GetDealResponseBody(out totalcount, agentinfo, body.method, body.partnerDealIds, pageindex, pagesize);
                        foreach (DealResponseBody rbody in list)
                        {
                            B2b_com_project pro_project = new B2b_com_projectData().GetProject(rbody.partnerPoiId.ConvertTo<int>(0));
                            if (pro_project != null)
                            {
                                List<ServicePhone> servicePhonelist = new List<ServicePhone>();
                                servicePhonelist.Add(new ServicePhone
                                {
                                    phone = pro_project.Mobile.Split(' ')[0],
                                    startHour = "08",
                                    startMin = "00",
                                    endHour = "17",
                                    endMin = "00"
                                });
                                //服务电话
                                rbody.servicePhones = servicePhonelist;

                                //入园地址
                                rbody.getInAddresses = new string[] { pro_project.Address };
                            }
                            //结算价
                            Agent_warrant warrant = new Agent_companyData().GetAgentWarrantInfo(agentinfo.Id, rbody.partnerDealId.ConvertTo<int>(0));
                            if (warrant != null)
                            {
                                rbody.settlementPrice = new B2bComProData().GetAgentPrice(rbody.partnerDealId.ConvertTo<int>(0), warrant.Warrant_level);
                            }
                            /*******************************
                             * 产品图片(最多5张)=产品主图片+产品子图片（前4张）
                             * ******************************/
                            List<DealImageInfo> imgs = rbody.dealImageInfos;
                            int topnum = 4;
                            List<int> childimgarr = new B2bComProData().GetProChildImgArr(rbody.partnerDealId.ConvertTo<int>(0), topnum);
                            if (childimgarr.Count > 0)
                            {
                                foreach (int iimg in childimgarr)
                                {
                                    imgs.Add(new DealImageInfo
                                    {
                                        imageName = "",
                                        imageUrl = iimg.ToString(),
                                        frontImage = false
                                    });
                                }
                            }

                            foreach (DealImageInfo iinfo in imgs)
                            {
                                var iimgurl = FileSerivce.GetImgUrl(iinfo.imageUrl.ConvertTo<int>(0));
                                iinfo.imageUrl = iimgurl;
                            }
                            rbody.dealImageInfos = imgs;
                        }

                        if (list.Count > 0)
                        {
                            response.code = 200;
                            response.describe = "success";
                            response.partnerId = int.Parse(agentinfo.mt_partnerId);
                            response.totalSize = totalcount;
                            //response.body = new List<DealResponseBody>();
                            response.body = list;
                        }
                        else
                        {
                            response.code = 300;
                            response.describe = "获取deal为空";
                        }

                        #endregion

                    }
                    else
                    {
                        response.code = 300;
                        response.describe = "拉取方式(method:" + body.method + ")出错";
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
    }
}