using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using ETS.Framework;
using System.Xml;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS.JsonFactory;
using Newtonsoft.Json;
using ETS2.VAS.Service.VASService.Data;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS2.Common.Business;
using ETS2.VAS.Service.VASService.Data.Common;


namespace ETS2.WebApp.TicketService
{
    /// <summary>
    /// HttpService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class HttpService : System.Web.Services.WebService
    {
        private static object lockobj = new object();
        [WebMethod]
        public string getEleInterface(string organization, string xml)
        {
            lock (lockobj)
            {
                //获得真实机构号(提供给分销商的机构号是 原机构号+1000000000)
                int trueorganization = int.Parse(organization) - 1000000000;
                organization = trueorganization.ToString();


                //把分销商发送过来的请求记入数据库
                Agent_requestlog reqlog = new Agent_requestlog()
                {
                    Id = 0,
                    Organization = int.Parse(organization),
                    Encode_requeststr = xml,
                    Decode_requeststr = "",
                    Request_time = DateTime.Now,
                    Encode_returnstr = "",
                    Decode_returnstr = "",
                    Return_time = DateTime.Parse("1970-01-01 00:00:00"),
                    Errmsg = "",
                    Request_type = "",
                    Req_seq = "",
                    Ordernum = "",
                    Is_dealsuc = 0,
                    Is_second_receivereq = 0,
                    Request_ip = CommonFunc.GetRealIP()
                };

                string skey = "";//des秘钥
                try
                {
                    //根据机构号获得机构(分销商)信息
                    Agent_company agentcompany = new AgentCompanyData().GetAgentCompany(int.Parse(organization));

                    int reqlogid = new Agent_requestlogData().Editagent_reqlog(reqlog);
                    if (reqlogid < 0 || reqlogid == 0)//录入分销发送过来的请求失败
                    {
                        return GetErrmsg("1111", "请检查发送内容格式是否正确", agentcompany == null ? "" : agentcompany.Inter_deskey, null);
                    }
                    if (reqlogid > 0)
                    {
                        reqlog.Id = reqlogid;
                    }

                    if (agentcompany == null)
                    {
                        reqlog.Errmsg = "获取分销公司信息失败";
                        new Agent_requestlogData().Editagent_reqlog(reqlog);

                        return GetErrmsg("1111", "分销公司信息不存在", "", reqlog);
                    }
                    else
                    {


                        skey = agentcompany.Inter_deskey;//des秘钥
                        if (skey == "")
                        {
                            reqlog.Errmsg = "请联系服务商获取密钥";
                            new Agent_requestlogData().Editagent_reqlog(reqlog);

                            return GetErrmsg("1111", "请联系服务商获取密钥", "", reqlog);
                        }



                        //解密发送过来的请求
                        xml = EncryptionHelper.DESDeCode(xml, skey);

                        //把解密的xml记入数据库
                        reqlog.Decode_requeststr = xml;
                        new Agent_requestlogData().Editagent_reqlog(reqlog);

                        //判断发起请求的ip是否和分销绑定的ip匹配
                        string Requestip = CommonFunc.GetRealIP();
                        if (organization == "1038" || organization == "2124")//机构号为1038，是测试账户，不用限制ip
                        { }
                        else
                        {
                            bool ismatch_ip = new Agent_requestlogData().Ismatch_ip(organization, Requestip);
                            if (ismatch_ip == false)
                            {
                                return GetErrmsg("1111", "请求ip(" + Requestip + ")还未绑定", skey, reqlog);
                            }
                        }


                        XmlDocument xdoc = new XmlDocument();
                        xdoc.LoadXml(xml);
                        XmlElement root = xdoc.DocumentElement;//根节点

                        string request_type = root.SelectSingleNode("request_type").InnerText.ToLower();
                        string req_seq = root.SelectSingleNode("req_seq").InnerText.Trim();
                        string order_num = "";
                        if (request_type == "cancel_order" || request_type == "repeat_order")
                        {
                            order_num = root.SelectSingleNode("order/order_num").InnerText;
                        }


                        //判断是否是二次发送相同的请求
                        int is_secondreq = new Agent_requestlogData().Is_secondreq(organization, req_seq, request_type);


                        //把发送的请求类型，请求流水号，订单号,是否是二次发送相同的请求录入数据库
                        reqlog.Request_type = request_type;
                        reqlog.Req_seq = req_seq;
                        reqlog.Ordernum = order_num;
                        reqlog.Is_second_receivereq = is_secondreq;

                        new Agent_requestlogData().Editagent_reqlog(reqlog);

                        //判断请求流水号是否是30位
                        if (req_seq == "")
                        {
                            return GetErrmsg("1111", "流水号不可为空", skey, reqlog);
                        }
                        else
                        {
                            ////除了发送电子票接口，其他接口流水号必须为30位
                            //if (request_type != "add_order") 
                            //{ 
                            //    if (req_seq.Length != 30)
                            //    {
                            //        return GetErrmsg("1111", "流水号必须为30位", skey, reqlog);
                            //    }
                            //}
                        }


                        if (request_type == "add_order")//发送电子票
                        {


                            //如果第一次发送失败，第二次重新提交订单
                            if (is_secondreq == 1)
                            {
                                //return GetErrmsg("1111", "重复订单", skey, reqlog);

                                GetErrmsg("1111", "重复订单", skey, reqlog);

                                //获取处理成功的请求信息：如果没有则重新提单
                                Agent_requestlog suclog = new Agent_requestlogData().GetAgent_addorderlogByReq_seq(organization, req_seq, 1);
                                if (suclog != null)
                                {
                                    return suclog.Encode_returnstr;
                                }
                                else
                                {
                                    return Add_order(reqlog, xml, organization, skey);
                                }
                            }
                            else
                            {
                                return Add_order(reqlog, xml, organization, skey);
                            }
                        }
                        else if (request_type == "repeat_order")//重发电子票
                        {
                            return Repeat_order(reqlog, xml, organization, skey);
                        }
                        else if (request_type == "cancel_order")//撤销电子票
                        {
                            return Cancel_order(reqlog, xml, organization, skey);
                        }
                        else if (request_type == "query_order")//查询电子票 
                        {
                            string add_order_req_seq = root.SelectSingleNode("order/add_order_req_seq").InnerText;

                            //判断分销商查询订单是否是 自己发送的接口订单
                            Agent_requestlog mrequestlogg = new Agent_requestlogData().GetAgent_addorderlogByReq_seq(organization, add_order_req_seq);
                            if (mrequestlogg == null)
                            {
                                return GetErrmsg("1111", "当前查询的订单不存在", skey, reqlog);
                            }
                            if (mrequestlogg.Is_dealsuc == 0)
                            {
                                return GetErrmsg("1111", "当前查询的订单不存在..", skey, reqlog);
                            }

                            return Query_order(reqlog, xml, organization, skey);
                        }
                        else if (request_type == "query_product")//查询产品基本信息 
                        {
                            return Query_product(reqlog, xml, organization, skey);
                        }
                        else if (request_type == "query_price")//查询产品价格
                        {
                            return Query_price(reqlog, xml, organization, skey);
                        }
                        //else if (request_type == "sendto_order")//转发电子票(暂时搁后)
                        //{
                        //    return Sendto_order(reqlog, xml, organization, skey);
                        //}
                        else//请求类型不存在
                        {
                            return GetErrmsg("1111", "请求类型不存在", skey, reqlog);
                        }
                    }
                }
                catch (Exception e)
                {
                    //return GetErrmsg("1111", e.Message, skey, reqlog);
                    return GetErrmsg("1111", "传递参数错误", skey, reqlog);
                }
            }
        }

        private string Query_price(Agent_requestlog reqlog, string xml, string organization, string skey)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(xml);
            XmlElement root = xdoc.DocumentElement;

            string partnerDealId = root.SelectSingleNode("body/productid").InnerText.Trim();
            string startTime = root.SelectSingleNode("body/startTime").InnerText.Trim();
            string endTime = root.SelectSingleNode("body/endTime").InnerText.Trim();


            //得到分销商信息
            Agent_company agentcompany = AgentCompanyData.GetAgentByid(organization.ConvertTo<int>(0));
            if (agentcompany == null)
            {
                return GetErrmsg("1111", "分销信息有误", skey, reqlog);
            }
            B2b_com_pro pro = new B2bComProData().GetProById(partnerDealId);
            if (pro != null)
            {
                if (pro.Server_type == 1 || pro.Server_type == 10)
                {
                    int proid_temp = pro.Id;
                    //如果产品是导入产品，则查询原始产品的库存价格
                    if (pro.Source_type == 4)
                    {
                        proid_temp = pro.Bindingid;
                    }

                    //判断是否需要提前预约
                    int isneedbespeak = pro.isneedbespeak;

                    //产品提前预定时间
                    int aheadHour = 0;
                    if (pro.Iscanuseonsameday == 0)
                    {
                        aheadHour = 25;
                    }
                    else if (pro.Iscanuseonsameday == 2)
                    {
                        aheadHour = 2;
                    }
                    else
                    {
                        aheadHour = 0;
                    }

                    string pricestr = "";//价格库存 字符串

                    #region 票务 价格库存
                    if (pro.Server_type == 1)
                    {

                        int days = (DateTime.Parse(endTime) - DateTime.Parse(startTime)).Days;
                        //当天不可用，则当天库存不返回
                        if (pro.Iscanuseonsameday == 0)
                        {
                            for (int i = 1; i <= days; i++)
                            {
                                //库存
                                int stock = pro.Ispanicbuy == 0 ? 100000000 : pro.Limitbuytotalnum;

                                pricestr += "<price>" +
                                                 "<productid>" + pro.Id + "</productid>" +
                                                 "<priceDate>" + DateTime.Parse(startTime).AddDays(i).ToString("yyyy-MM-dd") + "</priceDate>" +
                                                 "<facePrice>" + pro.Face_price + "</facePrice>" +
                                                 "<advicePrice>" + pro.Advise_price + "</advicePrice>" +
                                                 "<stock>" + stock + "</stock>" +  //库存	无限制 stock = 100000000
                                                 "<aheadHour>" + aheadHour + "</aheadHour>" +
                                             "</price>";
                            }
                        }
                        else
                        {
                            for (int i = 0; i <= days; i++)
                            {
                                //库存
                                int stock = pro.Ispanicbuy == 0 ? 100000000 : pro.Limitbuytotalnum;

                                pricestr += "<price>" +
                                                "<productid>" + pro.Id + "</productid>" +
                                                "<priceDate>" + DateTime.Parse(startTime).AddDays(i).ToString("yyyy-MM-dd") + "</priceDate>" +
                                                "<facePrice>" + pro.Face_price + "</facePrice>" +
                                                "<advicePrice>" + pro.Advise_price + "</advicePrice>" +
                                                "<stock>" + stock + "</stock>" +  //库存	无限制 stock = 100000000
                                                "<aheadHour>" + aheadHour + "</aheadHour>" +
                                            "</price>";
                            }
                        }
                    }
                    #endregion
                    #region 旅游大巴 价格库存
                    else
                    {
                        int days = (DateTime.Parse(endTime) - DateTime.Parse(startTime)).Days;
                        if (days < 0)
                        {
                            return GetErrmsg("1111", "开始日期需要小于结束日期", skey, reqlog);
                        }

                        List<B2b_com_LineGroupDate> list = new B2b_com_LineGroupDateData().GetLineGroupDate(proid_temp, DateTime.Parse(startTime), DateTime.Parse(endTime));

                        #region 返回班车团期不为空 不存在团期部分库存设为0
                        if (list.Count > 0)
                        {
                            var date = from r in list
                                       select r.Daydate.ToString("yyyy-MM-dd");
                            List<string> youxiaoDate = date.ToList();

                            #region 当天不可用，则 不返回 当天库存
                            if (pro.Iscanuseonsameday == 0)
                            {
                                for (int i = 1; i <= days; i++)
                                {
                                    int stock = 0;
                                    string ndate = DateTime.Parse(startTime).AddDays(i).ToString("yyyy-MM-dd");
                                    if (youxiaoDate.Contains(ndate))
                                    {
                                        var ndateLineGroupDate = list.Where(u => u.Daydate.ToString("yyyy-MM-dd") == ndate);
                                        foreach (B2b_com_LineGroupDate nn in ndateLineGroupDate)
                                        {
                                            stock = nn.Emptynum;
                                        }
                                    }

                                    pricestr += "<price>" +
                                                     "<productid>" + pro.Id + "</productid>" +
                                                     "<priceDate>" + DateTime.Parse(startTime).AddDays(i).ToString("yyyy-MM-dd") + "</priceDate>" +
                                                     "<facePrice>" + pro.Face_price + "</facePrice>" +
                                                     "<advicePrice>" + pro.Advise_price + "</advicePrice>" +
                                                     "<stock>" + stock + "</stock>" +  //库存	无限制 stock = 100000000
                                                     "<aheadHour>" + aheadHour + "</aheadHour>" +
                                                 "</price>";
                                }
                            }
                            #endregion
                            #region 当天可用，则 返回 当天库存
                            else
                            {
                                for (int i = 0; i <= days; i++)
                                {
                                    int stock = 0;
                                    string ndate = DateTime.Parse(startTime).AddDays(i).ToString("yyyy-MM-dd");
                                    if (youxiaoDate.Contains(ndate))
                                    {
                                        var ndateLineGroupDate = list.Where(u => u.Daydate.ToString("yyyy-MM-dd") == ndate);
                                        foreach (B2b_com_LineGroupDate nn in ndateLineGroupDate)
                                        {
                                            stock = nn.Emptynum;
                                        }
                                    }

                                    pricestr += "<price>" +
                                                    "<productid>" + pro.Id + "</productid>" +
                                                    "<priceDate>" + DateTime.Parse(startTime).AddDays(i).ToString("yyyy-MM-dd") + "</priceDate>" +
                                                    "<facePrice>" + pro.Face_price + "</facePrice>" +
                                                    "<advicePrice>" + pro.Advise_price + "</advicePrice>" +
                                                    "<stock>" + stock + "</stock>" +  //库存	无限制 stock = 100000000
                                                    "<aheadHour>" + aheadHour + "</aheadHour>" +
                                                "</price>";
                                }
                            }
                            #endregion

                        }
                        #endregion
                        #region 返回班车团期为空 则库存都设为0
                        else
                        {
                            #region 当天不可用，则 不返回 当天库存
                            if (pro.Iscanuseonsameday == 0)
                            {
                                for (int i = 1; i <= days; i++)
                                {
                                    pricestr += "<price>" +
                                                     "<productid>" + pro.Id + "</productid>" +
                                                     "<priceDate>" + DateTime.Parse(startTime).AddDays(i).ToString("yyyy-MM-dd") + "</priceDate>" +
                                                     "<facePrice>" + pro.Face_price + "</facePrice>" +
                                                     "<advicePrice>" + pro.Advise_price + "</advicePrice>" +
                                                     "<stock>0</stock>" +  //库存	无限制 stock = 100000000
                                                     "<aheadHour>" + aheadHour + "</aheadHour>" +
                                                 "</price>";
                                }
                            }
                            #endregion
                            #region 当天可用，则 返回 当天库存
                            else
                            {
                                for (int i = 0; i <= days; i++)
                                {
                                    pricestr += "<price>" +
                                                    "<productid>" + pro.Id + "</productid>" +
                                                    "<priceDate>" + DateTime.Parse(startTime).AddDays(i).ToString("yyyy-MM-dd") + "</priceDate>" +
                                                    "<facePrice>" + pro.Face_price + "</facePrice>" +
                                                    "<advicePrice>" + pro.Advise_price + "</advicePrice>" +
                                                    "<stock>0</stock>" +  //库存	无限制 stock = 100000000
                                                    "<aheadHour>" + aheadHour + "</aheadHour>" +
                                                "</price>";
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                    string ret = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                              "<business_trans version=\"1.0\">" +
                                  "<response_type>query_price</response_type>" +
                                  "<req_seq>" + reqlog.Req_seq + "</req_seq>" +
                                  "<result>" +
                                      "<id>0000</id>" +
                                      "<comment>成功</comment>" +
                                  "</result>" +
                                  "<body>" +
                                      "<prices>" + pricestr + "</prices>" +
                                  "</body>" +
                              "</business_trans>";


                    string encode_ret = EncryptionHelper.DESEnCode(ret, skey);


                    reqlog.Decode_returnstr = ret;
                    reqlog.Encode_returnstr = encode_ret;
                    reqlog.Return_time = DateTime.Now;
                    reqlog.Is_dealsuc = 1;
                    new Agent_requestlogData().Editagent_reqlog(reqlog);

                    return encode_ret;

                }
                else
                {
                    return GetErrmsg("1111", "暂时对外接口只是售卖票务、旅游大巴产品", skey, reqlog);
                }
            }
            else
            {
                return GetErrmsg("1111", "查询库存价格日历产品异常", skey, reqlog);
            }
        }

        private string Query_product(Agent_requestlog reqlog, string xml, string organization, string skey)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(xml);
            XmlElement root = xdoc.DocumentElement;

            string method = root.SelectSingleNode("body/method").InnerText.Trim();
            int pageindex = root.SelectSingleNode("body/currentPage").InnerText.Trim().ConvertTo<int>(1);
            int pagesize = root.SelectSingleNode("body/pageSize").InnerText.Trim().ConvertTo<int>(20);
            string productids = root.SelectSingleNode("body/productids").InnerText.Trim();

            //得到分销商信息
            Agent_company agentcompany = AgentCompanyData.GetAgentByid(organization.ConvertTo<int>(0));
            if (agentcompany == null)
            {
                return GetErrmsg("1111", "分销信息有误", skey, reqlog);
            }

            if (method.Equals("multi", StringComparison.OrdinalIgnoreCase) || method.Equals("page", StringComparison.OrdinalIgnoreCase))
            {
                #region 获取产品列表
                int totalcount = 0;
                List<B2b_com_pro> list = new B2bComProData().GetAgentProList(out totalcount, agentcompany.Id, method, productids, pageindex, pagesize);

                if (list.Count > 0)
                {
                    #region 产品基本信息
                    string prodetailstr = "";
                    foreach (B2b_com_pro pro in list)
                    {
                        var imgurl = FileSerivce.GetImgUrl(pro.Imgurl);
                        if (pro != null)
                        {
                            decimal buyPrice = AgentJosnData.GetAgentPrice(agentcompany.Id, pro.Com_id, pro.Id, pro.Server_type, pro.Agent1_price, pro.Agent2_price, pro.Agent3_price);
                            int maximum = pro.Pro_number == 0 ? 10000 : pro.Pro_number;
                            int stock = pro.Ispanicbuy == 0 ? 100000000 : pro.Limitbuytotalnum;
                            int aheadHour = pro.Iscanuseonsameday == 1 ? 0 : pro.Iscanuseonsameday == 2 ? 2 : 25;//产品提前预定时间 
                            int canRefund = pro.QuitTicketMechanism == 2 ? 0 : 1;
                            string refundnote = pro.QuitTicketMechanism == 0 ? "有效期内可退款" : pro.QuitTicketMechanism == 1 ? "有效期内/外 均可退票" : "不可退票";

                            prodetailstr += "<product>" +
                                                "<id>" + pro.Id + "</id>" +
                                                "<name>" + pro.Pro_name + "</name>" +
                                                "<pics>" +
                                                  "<url>" + imgurl + "</url>" +
                                                "</pics>" +
                                                "<startTime>" + pro.Pro_start + "</startTime>" +
                                                "<endTime>" + pro.Pro_end + "</endTime>" +
                                                "<facePrice>" + pro.Face_price + "</facePrice>" +
                                                "<advicePrice>" + pro.Advise_price + "</advicePrice>" +
                                                 "<settlementPrice>" + buyPrice + "</settlementPrice>" +
                                                "<minimum>1</minimum>" +
                                                "<maximum>" + maximum + "<maximum>" +
                                                "<stock>" + stock + "</stock>" +
                                                "<needBook>" + pro.isneedbespeak + "</needBook>" +
                                                "<aheadHour>" + aheadHour + "</aheadHour>" +
                                                "<canRefund>" + canRefund + "</canRefund>";
                            #region 票务  上下车地点列表
                            if (pro.Server_type == 1)
                            {
                                prodetailstr += "<pickuppointlist></pickuppointlist><dropoffpointlist></dropoffpointlist>";
                            }
                            #endregion
                            #region 旅游大巴 上下车地点列表
                            else if (pro.Server_type == 10)
                            {
                                #region 上车地点组合字符串
                                string pickupliststr = "";
                                if (pro.pickuppoint != "")
                                {
                                    var pickuppointlist = pro.pickuppoint.Split('，');
                                    for (int i = 0; i < pickuppointlist.Length; i++)
                                    {
                                        if (pickuppointlist[i] != "")
                                        {
                                            pickupliststr += "<point>" + pickuppointlist[i] + "</point>";
                                        }
                                    }
                                }
                                #endregion
                                prodetailstr += "<pickuppointlist>" + pickupliststr + "</pickuppointlist>";
                                #region 下车地点组合字符串
                                string dropliststr = "";
                                if (pro.dropoffpoint != "")
                                {
                                    var dropoffpointlist = pro.dropoffpoint.Split('，');
                                    for (int i = 0; i < dropoffpointlist.Length; i++)
                                    {
                                        if (dropoffpointlist[i] != "")
                                        {
                                            dropliststr += "<point>" + dropoffpointlist[i] + "</point>";
                                        }
                                    }
                                }
                                #endregion
                                prodetailstr += "<dropoffpointlist>" + dropliststr + "</dropoffpointlist>";
                            }
                            #endregion
                            #region 其他类型 上下车地点列表
                            else
                            {
                                return GetErrmsg("1111", "暂时对外接口只是售卖票务、旅游大巴产品", skey, reqlog);
                            }
                            #endregion
                            prodetailstr += "<purchaseNote>" +
                                                    "<chargeIncludeNote></chargeIncludeNote>" +
                                                    "<refundNote>" + refundnote + "</refundNote>" +
                                                    "<useNote>凭短信中的电子码直接消费</useNote>" +
                                                    "<importantNote>" + CommonFunc.ClearHtml(pro.Precautions) + "</importantNote>" +
                                                    "<imageTextNote>" + pro.Service_Contain + "</imageTextNote>" +
                                                "</purchaseNote>" +
                                                "<visitorRequire>";
                            #region 票务  游玩人信息要求
                            if (pro.Server_type == 1)
                            {
                                prodetailstr += "<firstVisitor>" +
                                         "<name>true</name>" +
                                         "<pinyin>false</pinyin>" +
                                         "<mobile>true</mobile>" +
                                         "<address>false</address>" +
                                         "<postcode>false</postcode>" +
                                         "<email>false</email>" +
                                         "<credentials>false</credentials>" +
                                         "<credentialsType>false</credentialsType>" +
                                      "</firstVisitor>" +
                                      "<otherVisitor>" +
                                         "<name>false</name>" +
                                         "<pinyin>false</pinyin>" +
                                         "<mobile>false</mobile>" +
                                         "<address>false</address>" +
                                         "<postcode>false</postcode>" +
                                         "<email>false</email>" +
                                         "<credentials>false</credentials>" +
                                         "<credentialsType></credentialsType>" +
                                     "<otherVisitor>";
                            }
                            #endregion
                            #region 旅游大巴 游玩人信息要求
                            else if (pro.Server_type == 10)
                            {
                                prodetailstr += "<firstVisitor>" +
                                     "<name>true</name>" +
                                     "<pinyin>false</pinyin>" +
                                     "<mobile>true</mobile>" +
                                     "<address>false</address>" +
                                     "<postcode>false</postcode>" +
                                     "<email>false</email>" +
                                     "<credentials>false</credentials>" +
                                     "<credentialsType></credentialsType>" +
                                  "</firstVisitor>" +
                                  "<otherVisitor>" +
                                     "<name>true</name>" +
                                     "<pinyin>false</pinyin>" +
                                     "<mobile>true</mobile>" +
                                     "<address>false</address>" +
                                     "<postcode>false</postcode>" +
                                     "<email>false</email>" +
                                     "<credentials>false</credentials>" +
                                     "<credentialsType></credentialsType>" +
                                 "<otherVisitor>";
                            }
                            #endregion
                            #region 其他类型 游玩人信息要求
                            else
                            {
                                return GetErrmsg("1111", "暂时对外接口只是售卖票务、旅游大巴产品", skey, reqlog);
                            }
                            #endregion

                            prodetailstr += "</visitorRequire>" +
                             "</product>";
                        }
                    }
                    #endregion
                    string ret = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<business_trans version=\"1.0\">" +
                                    "<response_type>query_product</response_type>" +
                                    "<req_seq>" + reqlog.Req_seq + "</req_seq>" +
                                    "<result>" +
                                        "<id>0000</id>" +
                                        "<comment>成功</comment>" +
                                    "</result>" +
                                    "<body>" +
                                        "<totalSize>" + totalcount + "</totalSize>" +
                                        "<products>" + prodetailstr + "</products>" +
                                    "</body>" +
                                "</business_trans>";


                    string encode_ret = EncryptionHelper.DESEnCode(ret, skey);


                    reqlog.Decode_returnstr = ret;
                    reqlog.Encode_returnstr = encode_ret;
                    reqlog.Return_time = DateTime.Now;
                    reqlog.Is_dealsuc = 1;
                    new Agent_requestlogData().Editagent_reqlog(reqlog);

                    return encode_ret;
                }
                else
                {
                    return GetErrmsg("1111", "查询产品为空", skey, reqlog);
                }
                #endregion
            }
            else
            {
                return GetErrmsg("1111", "查询方式只支持page、multi;", skey, reqlog);
            }
        }

        private string Query_order(Agent_requestlog reqlog, string xml, string organization, string skey)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(xml);
            XmlElement root = xdoc.DocumentElement;

            string add_order_req_seq = root.SelectSingleNode("order/add_order_req_seq").InnerText.Trim();

            if (add_order_req_seq.Trim() == "")
            {
                return GetErrmsg("1111", "流水号不可为空", skey, reqlog);
            }

            //判断订单是否是当前分销的订单
            bool isselforder = new Agent_requestlogData().Getisselforderbyreq_sql(organization, add_order_req_seq.ConvertTo<string>(""));
            if (isselforder == false)
            {
                return GetErrmsg("1111", "订单并非此分销的订单", skey, reqlog);
            }

            B2b_order morder = new B2bOrderData().GetOrderByAgentRequestReqSeq(add_order_req_seq.ConvertTo<string>(""));
            if (morder != null)
            {
                if (morder.Pro_id > 0)
                {
                    B2b_com_pro pro = new B2bComProData().GetProById(morder.Pro_id.ToString());
                    if (pro == null)
                    {
                        return GetErrmsg("1111", "产品不存在", skey, reqlog);
                    }
                    else
                    {
                        #region 主要用途 判断是否是商家自己产品
                        //判断产品码来源 (4分销倒过来的产品 1系统自动生成产品  2倒码产品 判断分销是否是 自己发码；3外来接口产品暂时不售卖)
                        int prosourtype = pro.Source_type;
                        if (prosourtype == 3)//外来接口产品,暂时只有阳光接口产品(需要手机号)
                        {
                            ////暂时只售卖商家自己产品,主要是产品有效期 需要另外通过外部接口获取，过于麻烦
                            //return GetErrmsg("1111", "暂时只可查询商家自己产品", skey, reqlog);
                        }
                        if (prosourtype == 4)//分销导入产品; 
                        {
                            int old_proid = new B2bComProData().GetOldproidById(morder.Pro_id.ToString());//绑定产品的原始编号
                            if (old_proid == 0)
                            {
                                return GetErrmsg("1111", "分销导入产品的原始产品编号没有查到", skey, reqlog);
                            }
                            else
                            {
                                prosourtype = new B2bComProData().GetProSource_typeById(old_proid.ToString());
                                if (prosourtype == 3)
                                {
                                    ////暂时只售卖商家自己产品,主要是产品有效期 需要另外通过外部接口获取，过于麻烦
                                    //return GetErrmsg("1111", "暂时只可查询商家自己产品", skey, reqlog);
                                }
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
                            if (pro.Server_type == 10)//旅游大巴产品有效期 =出游日期
                            {
                                pro_start = morder.U_traveldate;
                                pro_end = morder.U_traveldate;
                            }
                            else //票务
                            {
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
                                return GetErrmsg("1111", "根据订单号查询电子票信息失败", skey, reqlog);
                            }
                            else
                            {
                                if (meticketlist.Count == 0)
                                {
                                    return GetErrmsg("1111", "根据订单号查询电子票信息失败", skey, reqlog);
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
                            return GetErrmsg("1111", "根据订单号获得分销商接口发送请求记录失败", skey, reqlog);
                        }
                        else
                        {
                            try
                            {
                                XmlDocument xdocc = new XmlDocument();
                                xdocc.LoadXml(mrequestlog.Decode_requeststr);
                                XmlElement roott = xdocc.DocumentElement;
                                mobile = roott.SelectSingleNode("order/mobile").InnerText;
                            }
                            catch
                            {
                                mobile = "";
                            }
                        }
                        #endregion


                        #region 游玩人信息
                        string visitorstr = "";//游玩人信息 

                        IList<b2b_order_busNamelist> busNamelist = new List<b2b_order_busNamelist>();
                        if (pro.Bindingid > 0)
                        {
                            busNamelist = new B2bOrderData().GetTravelBusNamelist(morder.Bindingagentorderid);
                        }
                        else
                        {
                            busNamelist = new B2bOrderData().GetTravelBusNamelist(morder.Id);
                        }

                        if (busNamelist.Count > 0)
                        {
                            foreach (b2b_order_busNamelist mbusname in busNamelist)
                            {
                                if (mbusname != null)
                                {
                                    visitorstr += "<visitor>" +
                                                     "<name>" + mbusname.name + "</name>" +
                                                     "<pinyin>" + mbusname.pinyin + "</pinyin>" +
                                                     "<mobile>" + mbusname.contactphone + "</mobile>" +
                                                     "<address>" + mbusname.address + "</address>" +
                                                     "<postcode>" + mbusname.postcode + "</postcode>" +
                                                     "<email>" + mbusname.email + "</email>" +
                                                     "<credentials>" + mbusname.IdCard + "</credentials>" +
                                                     "<credentialsType>" + mbusname.credentialsType + "</credentialsType>" +
                                                 "</visitor>";
                                }
                            }
                        }
                        #endregion


                        #region 主要用途 如果是外来接口产品，则查询外来接口订单详情
                        //判断产品码来源 (4分销倒过来的产品 1系统自动生成产品  2倒码产品 判断分销是否是 自己发码；3外来接口产品暂时不售卖)
                        prosourtype = pro.Source_type;
                        if (prosourtype == 3)//外来接口产品 
                        {
                            ////暂时只售卖商家自己产品,主要是产品有效期 需要另外通过外部接口获取，过于麻烦
                            //return GetErrmsg("1111", "暂时只可查询商家自己产品", skey, reqlog);
                            if (pro.Serviceid == 1)//阳光
                            {
                                #region 阳光接口查询
                                try
                                {
                                    string yg_result = new SunShineInter().query_order(morder.Service_order_num);

                                    XmlDocument doc = new XmlDocument();
                                    doc.LoadXml(yg_result);
                                    XmlElement ygroot = doc.DocumentElement;

                                    string req_seq = ygroot.SelectSingleNode("req_seq").InnerText;//请求流水号
                                    string id = ygroot.SelectSingleNode("result/id").InnerText;//结果id 
                                    string comment = ygroot.SelectSingleNode("result/comment").InnerText;// 结果描述   
                                    //-----------------新增2 begin-------------------------//
                                    if (id == "0000")//发码成功
                                    {
                                        string ygbuy_num = ygroot.SelectSingleNode("order/buy_num").InnerText;//购买数量
                                        string ygspare_num = ygroot.SelectSingleNode("order/spare_num").InnerText;//剩余可使用数量   
                                        string yguse_num = ygroot.SelectSingleNode("order/use_num").InnerText;//交易数量
                                        string ygstart_validity_date = ygroot.SelectSingleNode("order/start_validity_date").InnerText;//开始有效期
                                        string ygend_validity_date = ygroot.SelectSingleNode("order/end_validity_date").InnerText;//结束有效期

                                        buy_num = ygbuy_num.ConvertTo<int>(0);
                                        keyong_num = ygspare_num.ConvertTo<int>(0);
                                        consume_num = yguse_num.ConvertTo<int>(0) + morder.Cancelnum;
                                        pro_start = ygstart_validity_date.ConvertTo<DateTime>();
                                        pro_end = ygend_validity_date.ConvertTo<DateTime>();
                                    }
                                    else
                                    {
                                        return GetErrmsg("1111", "查询订单失败:" + comment, skey, reqlog);
                                    }
                                }
                                catch (Exception e)
                                {
                                    return GetErrmsg("1111", "查询订单失败:" + e.Message, skey, reqlog);
                                }
                                #endregion
                            }
                            else if (pro.Serviceid == 3)//美景联动
                            {
                                #region 美景联动查询接口
                                ApiService mapiservice = new ApiServiceData().GetApiservice(3);
                                string MjldinsureNo = new Api_mjld_SubmitOrder_outputData().GetMjldinsureNo(morder.Id);
                                if (MjldinsureNo != "")
                                {
                                    
                                    try
                                    {
                                        //2.6、	订单浏览 
                                        string mjld_GetCodeInforesult = new MjldInter().GetOrderDetail(mapiservice, MjldinsureNo);
                                        if (mjld_GetCodeInforesult != "")
                                        {
                                            XmlDocument doc = new XmlDocument();
                                            doc.LoadXml(mjld_GetCodeInforesult);
                                            XmlElement mjroot = doc.DocumentElement;

                                            string endTime = mjroot.SelectSingleNode("endTime").InnerText;//有效期
                                            string inCount = mjroot.SelectSingleNode("inCount").InnerText;//总人数
                                            string usedCount = mjroot.SelectSingleNode("usedCount").InnerText;//已使用人数
                                            string backCount = mjroot.SelectSingleNode("backCount").InnerText;//退票人数
                                            string status = mjroot.SelectSingleNode("status").InnerText;//状态
                                            int keyongNum = inCount.ConvertTo<int>(0) - usedCount.ConvertTo<int>(0) - backCount.ConvertTo<int>(0);

                                            buy_num = inCount.ConvertTo<int>(0);
                                            keyong_num = keyongNum;
                                            consume_num = usedCount.ConvertTo<int>(0) + backCount.ConvertTo<int>(0);
                                            tuipiao_num = backCount.ConvertTo<int>(0);
                                            //pro_start = ;//开始日期用录入产品的开始日期
                                            pro_end = endTime.ConvertTo<DateTime>();
                                        } 
                                    }
                                    catch(Exception e) 
                                    {
                                        return GetErrmsg("1111", "查询订单失败:" + e.Message, skey, reqlog);
                                    }

                                }
                                #endregion
                            }
                            else { }
                        }
                        if (prosourtype == 4)//分销导入产品; 
                        {
                            int old_proid = new B2bComProData().GetOldproidById(morder.Pro_id.ToString());//绑定产品的原始编号
                            if (old_proid == 0)
                            {
                                return GetErrmsg("1111", "分销导入产品的原始产品编号没有查到", skey, reqlog);
                            }
                            else
                            {
                                prosourtype = new B2bComProData().GetProSource_typeById(old_proid.ToString());
                                if (prosourtype == 3)
                                {
                                    ////暂时只售卖商家自己产品,主要是产品有效期 需要另外通过外部接口获取，过于麻烦
                                    //return GetErrmsg("1111", "暂时只可查询商家自己产品", skey, reqlog);

                                    pro = new B2bComProData().GetProById(old_proid.ToString());
                                    if(pro==null)
                                    {
                                        return GetErrmsg("1111", "分销导入产品的原始产品没有查到", skey, reqlog);
                                    }
                                    morder = new B2bOrderData().GetOrderById(morder.Bindingagentorderid);
                                    if(morder==null)
                                    {
                                        return GetErrmsg("1111", "分销导入产品的原始产品订单没有查到", skey, reqlog);
                                    }
                                    if (pro.Serviceid == 1)//阳光
                                    {
                                        #region 阳光接口查询
                                        try
                                        {
                                            string yg_result = new SunShineInter().query_order(morder.Service_order_num);

                                            XmlDocument doc = new XmlDocument();
                                            doc.LoadXml(yg_result);
                                            XmlElement ygroot = doc.DocumentElement;

                                            string req_seq = ygroot.SelectSingleNode("req_seq").InnerText;//请求流水号
                                            string id = ygroot.SelectSingleNode("result/id").InnerText;//结果id 
                                            string comment = ygroot.SelectSingleNode("result/comment").InnerText;// 结果描述   
                                            //-----------------新增2 begin-------------------------//
                                            if (id == "0000")//发码成功
                                            {
                                                string ygbuy_num = ygroot.SelectSingleNode("order/buy_num").InnerText;//购买数量
                                                string ygspare_num = ygroot.SelectSingleNode("order/spare_num").InnerText;//剩余可使用数量   
                                                string yguse_num = ygroot.SelectSingleNode("order/use_num").InnerText;//交易数量
                                                string ygstart_validity_date = ygroot.SelectSingleNode("order/start_validity_date").InnerText;//开始有效期
                                                string ygend_validity_date = ygroot.SelectSingleNode("order/end_validity_date").InnerText;//结束有效期

                                                buy_num = ygbuy_num.ConvertTo<int>(0);
                                                keyong_num = ygspare_num.ConvertTo<int>(0);
                                                consume_num = yguse_num.ConvertTo<int>(0) + morder.Cancelnum;
                                                pro_start = ygstart_validity_date.ConvertTo<DateTime>();
                                                pro_end = ygend_validity_date.ConvertTo<DateTime>();
                                            }
                                            else
                                            {
                                                return GetErrmsg("1111", "查询订单失败:" + comment, skey, reqlog);
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            return GetErrmsg("1111", "查询订单失败:" + e.Message, skey, reqlog);
                                        }
                                        #endregion
                                    }
                                    else if (pro.Serviceid == 3)//美景联动
                                    {
                                        #region 美景联动查询接口
                                        ApiService mapiservice = new ApiServiceData().GetApiservice(3);
                                        string MjldinsureNo = new Api_mjld_SubmitOrder_outputData().GetMjldinsureNo(morder.Id);
                                        if (MjldinsureNo != "")
                                        {

                                            try
                                            {
                                                //2.6、	订单浏览 
                                                string mjld_GetCodeInforesult = new MjldInter().GetOrderDetail(mapiservice, MjldinsureNo);
                                                if (mjld_GetCodeInforesult != "")
                                                {
                                                    XmlDocument doc = new XmlDocument();
                                                    doc.LoadXml(mjld_GetCodeInforesult);
                                                    XmlElement mjroot = doc.DocumentElement;

                                                    string endTime = mjroot.SelectSingleNode("endTime").InnerText;//有效期
                                                    string inCount = mjroot.SelectSingleNode("inCount").InnerText;//总人数
                                                    string usedCount = mjroot.SelectSingleNode("usedCount").InnerText;//已使用人数
                                                    string backCount = mjroot.SelectSingleNode("backCount").InnerText;//退票人数
                                                    string status = mjroot.SelectSingleNode("status").InnerText;//状态
                                                    int keyongNum = inCount.ConvertTo<int>(0) - usedCount.ConvertTo<int>(0) - backCount.ConvertTo<int>(0);

                                                    buy_num = inCount.ConvertTo<int>(0);
                                                    keyong_num = keyongNum;
                                                    consume_num = usedCount.ConvertTo<int>(0) + backCount.ConvertTo<int>(0);
                                                    tuipiao_num = backCount.ConvertTo<int>(0);
                                                    //pro_start = ;//开始日期用录入产品的开始日期
                                                    pro_end = endTime.ConvertTo<DateTime>();
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                return GetErrmsg("1111", "查询订单失败:" + e.Message, skey, reqlog);
                                            }

                                        }
                                        #endregion
                                    }
                                    else { }
                                }
                            }
                        }
                        #endregion



                        string ret = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                    "<business_trans version=\"1.0\">" +
                                        "<response_type>query_order</response_type>" +
                                        "<req_seq>" + reqlog.Req_seq + "</req_seq>" +
                                        "<result>" +
                                            "<id>0000</id>" +
                                            "<comment>成功</comment>" +
                                        "</result>" +
                                        "<order>" +
                                            "<order_num>" + morder.Id + "</order_num>" +
                                            "<product_name><![CDATA[" + pro.Pro_name + "]]></product_name>" +
                                            "<product_id>" + pro.Id + "</product_id>" +
                                            "<phone_rev>" + mobile + "</phone_rev>" +
                                            "<buy_num>" + buy_num + "</buy_num>" +
                                            "<spare_num>" + keyong_num + "</spare_num>" +
                                            "<use_num>" + (consume_num - morder.Cancelnum) + "</use_num>" +
                                            "<cancel_ticketnum>" + morder.Cancelnum + "</cancel_ticketnum>" +
                                            "<start_validity_date>" + pro_start.ToString("yyyy-MM-dd") + "</start_validity_date>" +
                                            "<end_validity_date>" + pro_end.ToString("yyyy-MM-dd") + "</end_validity_date>" +
                                            "<add_time>" + add_time + "</add_time>" +
                                            "<real_name_type>" + real_name_type + "</real_name_type>" +
                                            "<real_name>" + real_name + "</real_name>" +
                                            "<status><![CDATA[" + morder.Order_state + ":" + statusdesc + "]]></status>" +
                                            "<pno>" + all_pno + "</pno>" +
                                            "<pickuppoint>" + morder.pickuppoint + "</pickuppoint>" +
                                            "<dropoffpoint>" + morder.dropoffpoint + "</dropoffpoint>" +
                                            "<visitors>" + visitorstr + "</visitors>" +
                                        "</order>" +
                                    "</business_trans>";

                        string encode_ret = EncryptionHelper.DESEnCode(ret, skey);


                        reqlog.Decode_returnstr = ret;
                        reqlog.Encode_returnstr = encode_ret;
                        reqlog.Return_time = DateTime.Now;
                        reqlog.Is_dealsuc = 1;
                        reqlog.Ordernum = morder.Id.ToString();
                        new Agent_requestlogData().Editagent_reqlog(reqlog);

                        return encode_ret;


                    }

                }
                else
                {
                    return GetErrmsg("1111", "订单中产品不存在", skey, reqlog);
                }
            }
            else
            {
                return GetErrmsg("1111", "订单不存在", skey, reqlog);
            }

        }

        private string Cancel_order(Agent_requestlog reqlog, string xml, string organization, string skey)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(xml);
            XmlElement root = xdoc.DocumentElement;

            string order_num = root.SelectSingleNode("order/order_num").InnerText;

            if (order_num.ConvertTo<int>(0) == 0)
            {
                return GetErrmsg("1111", "订单号格式有误", skey, reqlog);
            }
            string num = root.SelectSingleNode("order/num").InnerText;

            if (num.ConvertTo<int>(0) == 0)
            {
                return GetErrmsg("1111", "退票张数格式有误", skey, reqlog);
            }
            //判断订单是否是当前分销的订单
            bool isselforder = new Agent_requestlogData().Getisselforder(organization, order_num);
            if (isselforder == false)
            {
                return GetErrmsg("1111", "订单并非此分销的订单", skey, reqlog);
            }

            B2b_order morder = new B2bOrderData().GetOrderById(order_num.ConvertTo<int>(0));
            if (morder != null)
            {
                if (morder.Pro_id > 0)
                {
                    B2b_com_pro pro = new B2bComProData().GetProById(morder.Pro_id.ToString());
                    if (pro == null)
                    {
                        return GetErrmsg("1111", "产品不存在", skey, reqlog);
                    }
                    else
                    {
                        if (pro.Source_type == 2 )//产品来源:1.系统自动生成2.倒码产品3.外部接口产品4.导入产品
                        { 
                            return GetErrmsg("1111", "倒码产品不支持接口退票", skey, reqlog);
                        } 
                        else
                        {
                            //得到订单的验证方式：0(一码多验);1(一码一验)， 
                            #region 一码多验 退票 按订单号退票
                            if (morder.yanzheng_method == 0)
                            {
                                string data = OrderJsonData.QuitOrder(pro.Com_id, order_num.ConvertTo<int>(0), pro.Id, num.ConvertTo<int>(0), "分销外部接口退票");
                                XmlDocument retdoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + data + "}");
                                XmlElement retroot = retdoc.DocumentElement;
                                string type = retroot.SelectSingleNode("type").InnerText;
                                string msg = retroot.SelectSingleNode("msg").InnerText;

                                if (type == "100")//取消订单成功
                                {
                                    string ret = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                                            "<business_trans version=\"1.0\">" +
                                                                                "<response_type>cancel_order</response_type>" +
                                                                                "<req_seq>" + reqlog.Req_seq + "</req_seq>" +
                                                                                "<result>" +
                                                                                    "<id>0000</id>" +
                                                                                    "<comment>成功</comment>" +
                                                                                "</result>" +
                                                                                "<order>" +
                                                                                    "<order_num>" + order_num + "</order_num>" +
                                                                                    "<num>" + num + "</num>" +
                                                                                    "<cancel_pno></cancel_pno>" +
                                                                                "</order>" +
                                                                            "</business_trans>";
                                    string encode_ret = EncryptionHelper.DESEnCode(ret, skey);


                                    reqlog.Decode_returnstr = ret;
                                    reqlog.Encode_returnstr = encode_ret;
                                    reqlog.Return_time = DateTime.Now;
                                    reqlog.Is_dealsuc = 1;
                                    new Agent_requestlogData().Editagent_reqlog(reqlog);

                                    return encode_ret;
                                }
                                else//取消订单失败
                                {
                                    return GetErrmsg("1111", msg, skey, reqlog);
                                }
                            }
                            #endregion
                            #region 一码一验 退票 按电子码退票
                            else if (morder.yanzheng_method == 1)
                            {
                                //string data = EticketJsonData.BackAgentEticket(pro.Com_id, order_num.ConvertTo<int>(0), pro.Id, num.ConvertTo<int>(0), "分销外部接口退票");    
                                if (root.SelectSingleNode("order/cancel_pno") == null)
                                {
                                    return GetErrmsg("1111", "请输入退票的电子码", skey, reqlog);
                                }
                                string cancel_pno = root.SelectSingleNode("order/cancel_pno").InnerText;
                                if (cancel_pno == "")
                                {
                                    return GetErrmsg("1111", "请输入退票电子码", skey, reqlog);
                                }
                                B2b_eticket meticket = new B2bEticketData().GetEticketDetail(cancel_pno);

                                if (meticket == null)
                                {
                                    return GetErrmsg("1111", "退票电子码不存在", skey, reqlog);
                                }
                                if (meticket.Use_pnum != num.ConvertTo<int>(0))
                                {
                                    return GetErrmsg("1111", "退票数量不符", skey, reqlog);
                                }

                                string data = EticketJsonData.BackAgentEticket_interface(order_num.ConvertTo<int>(0), cancel_pno, "分销外部接口退票[一单多码]");

                                XmlDocument retdoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + data + "}");
                                XmlElement retroot = retdoc.DocumentElement;
                                string type = retroot.SelectSingleNode("type").InnerText;
                                string msg = retroot.SelectSingleNode("msg").InnerText;

                                if (type == "100")//取消订单成功
                                {
                                    string ret = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                                            "<business_trans version=\"1.0\">" +
                                                                                "<response_type>cancel_order</response_type>" +
                                                                                "<req_seq>" + reqlog.Req_seq + "</req_seq>" +
                                                                                "<result>" +
                                                                                    "<id>0000</id>" +
                                                                                    "<comment>成功</comment>" +
                                                                                "</result>" +
                                                                                "<order>" +
                                                                                    "<order_num>" + order_num + "</order_num>" +
                                                                                    "<num>" + num + "</num>" +
                                                                                    "<cancel_pno>" + cancel_pno + "</cancel_pno>" +
                                                                                "</order>" +
                                                                            "</business_trans>";
                                    string encode_ret = EncryptionHelper.DESEnCode(ret, skey);


                                    reqlog.Decode_returnstr = ret;
                                    reqlog.Encode_returnstr = encode_ret;
                                    reqlog.Return_time = DateTime.Now;
                                    reqlog.Is_dealsuc = 1;
                                    new Agent_requestlogData().Editagent_reqlog(reqlog);

                                    return encode_ret;
                                }
                                else//取消订单失败
                                {
                                    return GetErrmsg("1111", msg, skey, reqlog);
                                }
                            }
                            #endregion
                            else
                            {
                                return GetErrmsg("1111", "订单验码方式不支持", skey, reqlog);
                            }
                        }
                    }

                }
                else
                {
                    return GetErrmsg("1111", "订单中产品不存在", skey, reqlog);
                }
            }
            else
            {
                return GetErrmsg("1111", "订单不存在", skey, reqlog);
            }

        }

        private string Repeat_order(Agent_requestlog reqlog, string xml, string organization, string skey)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(xml);
            XmlElement root = xdoc.DocumentElement;

            string order_num = root.SelectSingleNode("order/order_num").InnerText;
            if (order_num.ConvertTo<int>(0) == 0)
            {
                return GetErrmsg("1111", "订单号格式有误", skey, reqlog);
            }

            var vasmodel = new SendEticketData().SendEticket(order_num.ConvertTo<int>(0), 2);//重发电子码
            if (vasmodel == "OK")
            {
                string ret = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                    "<business_trans version=\"1.0\">" +
                                                        "<response_type>repeat_order</response_type>" +
                                                        "<req_seq>" + reqlog.Req_seq + "</req_seq>" +
                                                        "<result>" +
                                                            "<id>0000</id>" +
                                                            "<comment>成功</comment>" +
                                                        "</result>" +
                                                        "<order>" +
                                                            "<order_num>" + order_num + "</order_num>" +
                                                        "</order>" +
                                                    "</business_trans>";
                string encode_ret = EncryptionHelper.DESEnCode(ret, skey);


                reqlog.Decode_returnstr = ret;
                reqlog.Encode_returnstr = encode_ret;
                reqlog.Return_time = DateTime.Now;
                reqlog.Is_dealsuc = 1;
                new Agent_requestlogData().Editagent_reqlog(reqlog);

                return encode_ret;
            }
            else
            {
                return GetErrmsg("1111", vasmodel, skey, reqlog);
            }

        }

        private string Add_order(Agent_requestlog reqlog, string xml, string organization, string skey)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(xml);
            XmlElement root = xdoc.DocumentElement;

            string product_num = root.SelectSingleNode("order/product_num").InnerText;

            int speciid = 0;
            //判断产品编号是否符合多规格产品特点：例如 2503-1
            if (product_num.IndexOf("-") > -1)
            {
                speciid = product_num.Substring(product_num.IndexOf("-") + 1).ConvertTo<int>(0);
                if (speciid == 0)
                {
                    return GetErrmsg("1111", "产品编码格式有误", skey, reqlog);
                }
                product_num = product_num.Substring(0, product_num.IndexOf("-"));
            }
            if (product_num.ConvertTo<int>(0) == 0)
            {
                return GetErrmsg("1111", "产品编码格式有误", skey, reqlog);
            }
            string num = root.SelectSingleNode("order/num").InnerText;
            if (num.ConvertTo<int>(0) == 0)
            {
                return GetErrmsg("1111", "购买数量格式有误", skey, reqlog);
            }
            string mobile = root.SelectSingleNode("order/mobile").InnerText;

            //得到分销商信息
            Agent_company agentcompany = AgentCompanyData.GetAgentByid(organization.ConvertTo<int>(0));
            if (agentcompany == null)
            {
                return GetErrmsg("1111", "分销信息有误", skey, reqlog);
            }

            #region 主要用途 判断是否是商家自己产品，如果为外来接口产品，暂时不售卖
            //判断产品来源 (分销倒过来的产品 系统自动生成产品  倒码产品 判断分销是否是 自己发码；外来接口产品暂时不售卖)
            int prosourtype = new B2bComProData().GetProSource_typeById(product_num);
            if (prosourtype == 3)//外来接口产品,暂时只有阳光接口产品(需要手机号)
            {
                ////暂时只售卖商家自己产品,主要是产品有效期 需要另外通过外部接口获取，过于麻烦
                //return GetErrmsg("1111", "暂时只售卖商家自己产品", skey, reqlog);

                //外来接口产品 则必须有下单电话
                if (mobile.Trim() == "")
                {
                    return GetErrmsg("1111", "当前产品必须有下单电话", skey, reqlog);
                }
            }
            if (prosourtype == 4)//分销导入产品; 
            {
                int old_proid = new B2bComProData().GetOldproidById(product_num);//绑定产品的原始编号
                if (old_proid == 0)
                {
                    return GetErrmsg("1111", "分销导入产品的原始产品编号没有查到", skey, reqlog);
                }
                else
                {
                    prosourtype = new B2bComProData().GetProSource_typeById(old_proid.ToString());
                    if (prosourtype == 3)
                    {
                        ////暂时只售卖商家自己产品,主要是产品有效期 需要另外通过外部接口获取，过于麻烦
                        //return GetErrmsg("1111", "暂时只售卖商家自己产品", skey, reqlog);

                        //外来接口产品 则必须有下单电话
                        if (mobile.Trim() == "")
                        {
                            return GetErrmsg("1111", "当前产品必须有下单电话", skey, reqlog);
                        }
                    }
                }
            }
            #endregion


            if (agentcompany.Agent_messagesetting == 1)//分销商自己发送短信
            {
                //如果分销商没有传递过来手机号，为了订单发送短信验证通过，设置手机为分销商 手机
                if (mobile == "")
                {
                    mobile = agentcompany.Mobile;
                }

            }
            else
            {
                if (CommonFunc.IsMobile(mobile.Trim()) == false)
                {
                    return GetErrmsg("1111", "手机号码格式有误", skey, reqlog);
                }
            }


            //获取产品服务类型，暂时只是支持票务 、 大巴
            int proservertype = new B2bComProData().GetProServer_typeById(product_num);
            if (proservertype == 1 || proservertype == 10)
            { }
            else
            {
                return GetErrmsg("1111", "暂时对外接口只支持票务、大巴，其他产品请到分销后台提单", skey, reqlog);
            }

            string use_date = root.SelectSingleNode("order/use_date").InnerText;
            //旅游大巴产品 对游玩日期格式要求
            if (proservertype == 10)
            {
                try
                {
                    use_date = DateTime.Parse(use_date).ToString("yyyy-MM-dd");
                }
                catch
                {
                    return GetErrmsg("1111", "游玩日期[" + use_date + "] 格式要求：yyyy-MM-dd", skey, reqlog);
                }
            }

            string real_name_type = root.SelectSingleNode("order/real_name_type").InnerText;
            if (real_name_type == "")
            {
                real_name_type = "0";
            }
            if (real_name_type != "0")
            {
                return GetErrmsg("1111", "实名制类型，暂时都为0", skey, reqlog);
            }
            string real_name = root.SelectSingleNode("order/real_name").InnerText;


            #region 新增字段 验码方式(0：一码多验，一单生成一个码，此种为默认方式；1一码一验，一单生成多个码)
            int yanzheng_method = 0;
            if (root.SelectSingleNode("order/yanzheng_method") != null)
            {
                yanzheng_method = (root.SelectSingleNode("order/yanzheng_method").InnerText).ConvertTo<int>(0);
                if (yanzheng_method != 0 && yanzheng_method != 1)
                {
                    return GetErrmsg("1111", "验码方式只能赋值0 或者 1", skey, reqlog);
                }
                if (yanzheng_method == 1)
                {
                    if (prosourtype == 3)
                    {
                        return GetErrmsg("1111", "当前产品不支持一码一验方式出票", skey, reqlog);
                    }
                }
            }
            #endregion


            #region 新增字段 身份证号
            string idcard = "";
            if (root.SelectSingleNode("order/cardCode") != null)
            {
                idcard = (root.SelectSingleNode("order/cardCode").InnerText).ConvertTo<string>("");
            }
            #endregion

            B2b_com_pro modelcompro = new B2bComProData().GetProById(product_num);
            if (modelcompro == null)
            {
                return GetErrmsg("1111", "根据产品编号查询产品信息失败", skey, reqlog);
            }


            //新增乘车人信息
            var travelnames = "";
            var travelidcards = "";
            var travelnations = "";
            var travel_pickuppoints = "";
            var travel_dropoffpoints = "";
            var travelphones = "";
            var travelremarks = "";

            var travelpinyins = "";
            var traveladdresss = "";
            var travelpostcodes = "";
            var travelemails = "";
            var travelcredentialsTypes = "";
            //保险人信息
            var baoxiannames = ""; ;
            var baoxianpinyinnames = "";
            var baoxianidcards = ""; ;

            #region 旅游大巴产品 验证录入订单信息是否正确
            if (modelcompro.Server_type == 10)
            {
                try
                {
                    //游玩日期
                    if (use_date == "")
                    {
                        return GetErrmsg("1111", "旅游大巴产品需要传入游玩日期", skey, reqlog);
                    }

                    //价格(建议价)效验，保证分销抓到的是最新价格
                    string advice_price = root.SelectSingleNode("order/advice_price").InnerText;
                    if (modelcompro.Advise_price.ToString("f0") != decimal.Parse(advice_price).ToString("f0"))
                    {
                        return GetErrmsg("1111", "价格效验失败，请重新拉取价格库存日历", skey, reqlog);
                    }
                    string pickuppoint = root.SelectSingleNode("order/pickuppoint").InnerText.Trim();
                    string dropoffpoint = root.SelectSingleNode("order/dropoffpoint").InnerText.Trim();
                    if (pickuppoint == "" || dropoffpoint == "")
                    {
                        return GetErrmsg("1111", "乘车人 上车地点、下车地点 信息必须完善", skey, reqlog);
                    }
                    travel_pickuppoints = pickuppoint;
                    travel_dropoffpoints = dropoffpoint;

                    //第一游玩人信息 必填信息是否填写
                    if (root.SelectSingleNode("order/firstVisitor") != null)
                    {
                        XmlNode firstvisitor = root.SelectSingleNode("order/firstVisitor/visitor");
                        string name = firstvisitor.SelectSingleNode("name").InnerText.Trim();
                        string pinyin = firstvisitor.SelectSingleNode("pinyin").InnerText.Trim();
                        string first_mobile = firstvisitor.SelectSingleNode("mobile").InnerText.Trim();
                        string address = firstvisitor.SelectSingleNode("address").InnerText.Trim();
                        string postcode = firstvisitor.SelectSingleNode("postcode").InnerText.Trim();
                        string email = firstvisitor.SelectSingleNode("email").InnerText.Trim();
                        string credentials = firstvisitor.SelectSingleNode("credentials").InnerText.Trim();
                        string credentialsType = firstvisitor.SelectSingleNode("credentialsType").InnerText.Trim();


                        //判断必填信息是否填写
                        if (name == "" || first_mobile == "")
                        {
                            return GetErrmsg("1111", "第一游玩人 姓名、电话 信息必须完善", skey, reqlog);
                        }
                        //如果身份证填写了，则需要判断一下身份证格式
                        if (credentials != "")
                        {
                            if (CommonFunc.IsIDcard(credentials) == false)
                            {
                                return GetErrmsg("1111", "第一游玩人 身份证 信息格式有误", skey, reqlog);
                            }
                        }

                        //大巴信息
                        travelnames += name + ",";
                        travelidcards += credentials + ",";
                        travelnations += "汉族,";
                        travelphones += first_mobile + ",";
                        travelremarks += ",";
                        travelpinyins += pinyin + ",";
                        traveladdresss += address + ",";
                        travelpostcodes += postcode + ",";
                        travelemails += email + ",";
                        travelcredentialsTypes += credentialsType + ",";
                        //保险信息
                        baoxiannames += name + ",";
                        baoxianpinyinnames += pinyin + ",";
                        baoxianidcards += credentials + ",";

                    }
                    //其他游玩人信息  必填信息是否填写
                    if (root.SelectSingleNode("order/otherVisitor") != null)
                    {
                        if (root.SelectSingleNode("order/otherVisitor").HasChildNodes)
                        {
                            string othervisitorerr = "";

                            XmlNodeList othervisitorlist = root.SelectSingleNode("order/otherVisitor").ChildNodes;
                            foreach (XmlNode node in othervisitorlist)
                            {
                                string name = node.SelectSingleNode("name").InnerText.Trim();
                                string pinyin = node.SelectSingleNode("pinyin").InnerText.Trim();
                                string other_mobile = node.SelectSingleNode("mobile").InnerText.Trim();
                                string address = node.SelectSingleNode("address").InnerText.Trim();
                                string postcode = node.SelectSingleNode("postcode").InnerText.Trim();
                                string email = node.SelectSingleNode("email").InnerText.Trim();
                                string credentials = node.SelectSingleNode("credentials").InnerText.Trim();
                                string credentialsType = node.SelectSingleNode("credentialsType").InnerText.Trim();


                                //判断必填信息是否填写
                                if (name == "" || other_mobile == "")
                                {
                                    othervisitorerr += "其他游玩人(" + name + ") 姓名、电话 信息必须完善;";
                                }
                                //如果身份证填写了，则需要判断一下身份证格式
                                if (credentials != "")
                                {
                                    if (CommonFunc.IsIDcard(credentials) == false)
                                    {
                                        othervisitorerr += "其他游玩人(" + name + ") 身份证 信息格式有误;";
                                    }
                                }
                                //大巴信息
                                travelnames += name + ",";
                                travelidcards += credentials + ",";
                                travelnations += "汉族,";
                                travelphones += other_mobile + ",";
                                travelremarks += ",";

                                travelpinyins += pinyin + ",";
                                traveladdresss += address + ",";
                                travelpostcodes += postcode + ",";
                                travelemails += email + ",";
                                travelcredentialsTypes += credentialsType + ",";
                                //保险信息
                                baoxiannames += name + ",";
                                baoxianpinyinnames += pinyin + ",";
                                baoxianidcards += credentials + ",";
                            }
                            if (othervisitorerr != "")
                            {
                                return GetErrmsg("1111", othervisitorerr, skey, reqlog);
                            }
                        }
                    }

                }
                catch
                {
                    return GetErrmsg("1111", "价格效验失败/意外错误", skey, reqlog);
                }
            }
            #endregion


            int isInterfaceSub = 1;//是否是电子票接口提交的订单:0.否;1.是
            string ordertype = "1";//1.出票2.充值
            string agentaccount = "";//分销商登录账户：如果为空的话则是通过接口提交的订单；否则是通过分销后台提交的订单，需要考虑员工账户额度限定；
            int orderid = 0;
            //string data = OrderJsonData.AgentOrder(int.Parse(organization), product_num, ordertype, num, real_name, mobile, use_date, agentaccount, isInterfaceSub, out orderid, int.Parse(real_name_type), 1, "", "", "", 0, "", "", "", "", 0, 0, 0, yanzheng_method, speciid);
            string data = OrderJsonData.AgentOrder(int.Parse(organization), product_num, ordertype, num, real_name, mobile, use_date, agentaccount, isInterfaceSub, out orderid, int.Parse(real_name_type), 1, travel_pickuppoints, travel_dropoffpoints, travelremarks, 0, "", "", "", "", 0, 0, 0, yanzheng_method, speciid, baoxiannames, baoxianpinyinnames, baoxianidcards, 0, null, 0, "", "", "", "", "", idcard);
            XmlDocument retdoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + data + "}");
            XmlElement retroot = retdoc.DocumentElement;
            string type = retroot.SelectSingleNode("type").InnerText;
            string msg = retroot.SelectSingleNode("msg").InnerText;
            if (type == "100")//创建订单成功
            {
                string pno = retroot.SelectSingleNode("pno").InnerText;
                string dikou = retroot.SelectSingleNode("dikou").InnerText;


                #region 产品有效期
                //经过以上赋值prosourtype，只可能2个值:1系统自动生成码产品;2倒码产品
                DateTime pro_start = modelcompro.Pro_start;
                DateTime pro_end = modelcompro.Pro_end;
                if (prosourtype == 2) //倒码产品
                { }
                if (prosourtype == 1) //系统自动生成码产品
                {
                    if (modelcompro.Server_type == 10)//旅游大巴
                    {
                        pro_start = DateTime.Parse(use_date);
                        pro_end = DateTime.Parse(use_date);
                    }
                    else //票务
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
                }
                #endregion

                if (modelcompro.Server_type == 10)
                {
                    #region  录入订单子表(订单乘车人信息表)
                    if (travelnames != "")
                    {
                        int comid = modelcompro.Com_id;
                        int proid = modelcompro.Id;
                        //对订单查询如果导入产品产生订单，插入乘车人插入b订单
                        var b2borderinfo = new B2bOrderData().GetOrderById(orderid);
                        if (b2borderinfo != null)
                        {
                            if (b2borderinfo.Bindingagentorderid != 0)
                            {
                                orderid = b2borderinfo.Bindingagentorderid;
                                var b2borderinfo_B = new B2bOrderData().GetOrderById(orderid);
                                if (b2borderinfo_B != null)
                                {
                                    comid = b2borderinfo_B.Comid;
                                    proid = b2borderinfo_B.Pro_id;
                                }
                            }
                        }
                        for (int i = 1; i <= num.ConvertTo<int>(0); i++)
                        {
                            string travelname = travelnames.Split(',')[i - 1];
                            string travelidcard = travelidcards.Split(',')[i - 1];
                            string travelnation = travelnations.Split(',')[i - 1];
                            string travelphone = travelphones.Split(',')[i - 1];
                            string travelremark = travelremarks.Split(',')[i - 1];
                            string travel_pickuppoint = travel_pickuppoints;
                            string travel_dropoffpoint = travel_dropoffpoints;

                            string travelpinyin = travelpinyins.Split(',')[i - 1];
                            string traveladdress = traveladdresss.Split(',')[i - 1];
                            string travelpostcode = travelpostcodes.Split(',')[i - 1];
                            string travelemail = travelemails.Split(',')[i - 1];
                            string travelcredentialsType = travelcredentialsTypes.Split(',')[i - 1];

                            int rt = new B2bOrderData().Insertb2b_order_busNamelist(orderid, travelname, travelidcard, travelnation, real_name, mobile, DateTime.Now, num, use_date, comid, agentcompany.Id, proid, travel_pickuppoint, travel_dropoffpoint, travelphone, travelremark, travelpinyin, traveladdress, travelpostcode, travelemail, travelcredentialsType);
                        }
                    }
                    #endregion
                    #region 赠送保险
                    OrderJsonData.ZengsongBaoxian(orderid);
                    #endregion
                }

                string ret = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                         "<business_trans version=\"1.0\">" +
                                                             "<response_type>add_order</response_type>" +
                                                             "<req_seq>" + reqlog.Req_seq + "</req_seq>" +
                                                             "<result>" +
                                                                 "<id>0000</id>" +
                                                                 "<comment>成功</comment>" +
                                                             "</result>" +
                                                             "<order>" +
                                                                 "<order_num>" + msg + "</order_num>" +
                                                                 "<code>" + pno + "</code>" +
                                                                 "<start_validity_date>" + pro_start.ToString("yyyy-MM-dd") + "</start_validity_date>" +
                                                                 "<end_validity_date>" + pro_end.ToString("yyyy-MM-dd") + "</end_validity_date>" +
                                                             "</order>" +
                                                         "</business_trans>";
                string encode_ret = EncryptionHelper.DESEnCode(ret, skey);


                reqlog.Decode_returnstr = ret;
                reqlog.Encode_returnstr = encode_ret;
                reqlog.Return_time = DateTime.Now;
                reqlog.Is_dealsuc = 1;
                reqlog.Ordernum = msg;
                new Agent_requestlogData().Editagent_reqlog(reqlog);


                return encode_ret;
            }
            else//创建订单失败
            {
                return GetErrmsg("1111", msg, skey, reqlog);
            }
        }



        private string GetErrmsg(string errcode, string errmsg, string inter_key, Agent_requestlog reqlog)
        {
            try
            {
                string req_sql = "";
                if (reqlog != null)
                {
                    req_sql = reqlog.Req_seq;
                }

                string ret = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                         "<business_trans  version=\"1.0\">" +
                             "<response_type>ERROR</response_type>" +
                             "<req_seq>" + req_sql + "</req_seq>" +
                                 "<result>" +
                                     "<id>" + errcode + "</id>" +
                                     "<comment><![CDATA[" + errmsg + "]]></comment>" +
                                 "</result>" +
                         "</business_trans>";
                string encode_ret = EncryptionHelper.DESEnCode(ret, inter_key);


                if (reqlog != null)
                {
                    reqlog.Errmsg = errmsg;
                    reqlog.Decode_returnstr = ret;
                    reqlog.Encode_returnstr = encode_ret;
                    reqlog.Return_time = DateTime.Now;

                    new Agent_requestlogData().Editagent_reqlog(reqlog);
                }
                if (encode_ret == "")
                {
                    encode_ret = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                      "<business_trans  version=\"1.0\">" +
                          "<response_type>ERROR</response_type>" +
                          "<req_seq></req_seq>" +
                              "<result>" +
                                  "<id>1111</id>" +
                                  "<comment><![CDATA[请求参数传入格式有误]]></comment>" +
                              "</result>" +
                      "</business_trans>";

                    return encode_ret;
                }
                else
                {
                    return encode_ret;
                }

            }
            catch
            {
                string ret = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                       "<business_trans  version=\"1.0\">" +
                           "<response_type>ERROR</response_type>" +
                           "<req_seq></req_seq>" +
                               "<result>" +
                                   "<id>1111</id>" +
                                   "<comment><![CDATA[请求参数传入格式有误]]></comment>" +
                               "</result>" +
                       "</business_trans>";

                return ret;
            }
        }

    }
}
