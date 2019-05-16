using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using ETS2.WeiXin.Service;
using ETS2.WeiXin.Service.WinXinService.BLL;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using System.Xml;
using ETS2.Member.Service.MemberService.Model;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.Common.Business;
using ETS2.Member.Service.MemberService.Data;
using ETS.Framework;
using ETS.JsonFactory;

namespace ETS2.WebApp.WeiXin
{
    public partial class Index : System.Web.UI.Page
    {
        private static object lockobj = new object();

        public string httphead = "http://";//由于苹果手机微信链接 没有http:// 不识别，所以需要添加

        protected void Page_Load(object sender, EventArgs e)
        {
            WeiXinManage _wx = new WeiXinManage();

            //string postStr = "<xml><ToUserName><![CDATA[toUser]]></ToUserName>" +
            //                "<FromUserName><![CDATA[FromUser]]></FromUserName>" +
            //                "<CreateTime>123456789</CreateTime>" +
            //                "<MsgType><![CDATA[event]]></MsgType>" +
            //                "<Event><![CDATA[CLICK]]></Event>" +
            //                "<EventKey><![CDATA[V_OPENCARD]]></EventKey>" +
            //                "</xml>";
            //_wx.Handle(postStr);

            //获取访问的域名   
            string RequestDomin = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToLower();
            string Requestfile = System.Web.HttpContext.Current.Request.ServerVariables["Url"].ToLower();
            //根据访问的域名获得公司信息
            WeiXinBasic basicc = new WeiXinBasicData().GetWeiXinBasicByDomain(RequestDomin);


            string postStr = "";

            if (Request.HttpMethod.ToLower() == "post")
            {

                Stream s = System.Web.HttpContext.Current.Request.InputStream;

                byte[] b = new byte[s.Length];

                s.Read(b, 0, (int)s.Length);

                postStr = Encoding.UTF8.GetString(b);



                if (!string.IsNullOrEmpty(postStr)) //请求处理
                {

                    _wx.Handle(postStr, basicc);
                    Handle(postStr,basicc);

                }

            }

            else
            {

                _wx.Auth(basicc);

            }

        }

        public void Handle(string postStr, WeiXinBasic basic)
        {
            try
            {
                //封装请求类

                XmlDocument doc = new XmlDocument();

                doc.LoadXml(postStr);

                XmlElement rootElement = doc.DocumentElement;



                XmlNode MsgType = rootElement.SelectSingleNode("MsgType");



                RequestXML requestXML = new RequestXML();

                requestXML.PostStr = postStr;

                requestXML.ToUserName = rootElement.SelectSingleNode("ToUserName").InnerText;

                requestXML.FromUserName = rootElement.SelectSingleNode("FromUserName").InnerText;

                requestXML.CreateTime = rootElement.SelectSingleNode("CreateTime").InnerText;

                requestXML.MsgType = MsgType.InnerText;

                if (requestXML.MsgType == "event" || requestXML.MsgType == "text")
                {
                    if (requestXML.MsgType == "event")
                    {
                        requestXML.Eevent = rootElement.SelectSingleNode("Event").InnerText;
                        requestXML.EventKey = rootElement.SelectSingleNode("EventKey") == null ? "" : rootElement.SelectSingleNode("EventKey").InnerText;
                    }

                    ResponseMsg(requestXML, basic);
                }
            }
            catch (Exception ex)
            {
                string emptystr = "";
                System.Web.HttpContext.Current.Response.Write(emptystr);
                //加txt文档记录
                TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\weixinerrLog.txt", ex.Message);
            }
        }

        private void ResponseMsg(RequestXML requestXML, WeiXinBasic basic)
        { 
            try
            {
                string resxml = "";
                
                #region 接收事件推送
                if (requestXML.MsgType == "event")
                { 
                    #region 关注事件 或者  已关注微信用户 扫描带参数二维码事件
                    if (requestXML.Eevent == "SCAN" || requestXML.Eevent == "subscribe")
                    {
                        lock (lockobj)
                        {
                            //判断是否是并发请求
                            string cretime = requestXML.CreateTime;
                            //获得请求发送的次数
                            int reqnum = new WxSubscribeDetailData().GetReqnum(requestXML.FromUserName, cretime, requestXML.Eevent);
                            if (reqnum == 1)
                            { 
                                //如果用户二次扫描的合作单位信息，则显示 当前扫描的合作单位信息
                                int wxsourceid = requestXML.EventKey.Substring(requestXML.EventKey.IndexOf("_") + 1).ConvertTo<int>(0);
                                if (wxsourceid > 0)
                                {
                                    WxSubscribeSource sourcer = new WxSubscribeSourceData().GetWXSourceById(wxsourceid);
                                    if (sourcer != null)
                                    {
                                        if (sourcer.Productid > 0)//扫描产品二维码
                                        {
                                            //根据产品id得到产品信息
                                            B2b_com_pro pro = new B2bComProData().GetProById(sourcer.Productid.ToString());
                                            if (pro != null)
                                            {
                                                //如果是优惠劵产品，生成优惠劵订单
                                                if (pro.Server_type == 3)
                                                {
                                                    var orderdata = OrderJsonData.insertyuohuiquan(sourcer.Productid, requestXML.FromUserName);
                                                }
                                            }
                                        } 
                                    } 
                                }
                                
                            }
                        }
                    }
                    #endregion 
                    #region 接收事件推送
                    if (requestXML.MsgType == "text")
                    {
                        //判断是否是顾问，1是；0不是
                        int isguwen = new MemberChannelData().IsGuwen(requestXML.FromUserName, basic.Comid);
                        if (isguwen == 1)
                        {
                            if (requestXML.Content != "")
                            {
                                if (requestXML.Content.ToLower().Substring(0, 2) == "qx" || requestXML.Content.ToLower().Substring(0, 2) == "qr")
                                {
                                    string str = "";
                                    //截取前两个字qx，进入确认流程
                                    if (requestXML.Content.ToLower().Substring(2, requestXML.Content.Length - 2) != "")
                                    {
                                        try
                                        {
                                            int orderid = int.Parse(requestXML.Content.ToLower().Substring(2, requestXML.Content.Length - 2));
                                            var snsback = OrderJsonData.UporderPaystate(orderid, requestXML.Content.ToLower().Substring(0, 2), requestXML.Content);//处理订单
                                        }
                                        catch { }

                                        if (requestXML.Content.ToLower().Substring(0, 2) == "qr")
                                        {
                                            str = "订单已确认";
                                        }
                                        if (requestXML.Content.ToLower().Substring(0, 2) == "qx")
                                        {
                                            str = "订单已取消";
                                        }
                                    }

                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + CommonFunc.ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + str + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                    System.Web.HttpContext.Current.Response.Write(resxml);
                                }
                            }
                        }
                    }
                    #endregion



                }
                #endregion 
            

            }
            catch (Exception ex)
            {
                ////WriteTxt("异常" + ex.Message + "Struck:" + ex.StackTrace.ToString());
                //wx_logs.MyInsert("异常" + ex.Message + "Struck:" + ex.StackTrace.ToString());
                //string emptystr = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[]]></Content></xml>";
                string emptystr = "";
                System.Web.HttpContext.Current.Response.Write(emptystr);
            }
        }

    }
}