using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using System.Net;
using System.IO;
using System.Xml;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Member.Service.MemberService.Data;
using System.Collections;
using ETS.Framework;
using FileUpload.FileUpload.Data;
using ETS2.Common.Business;
using ETS2.PM.Service.PMService.Data;
using Newtonsoft.Json;
using ETS.Data.SqlHelper;
using System.Data.SqlClient;
using System.Data;
using ETS2.CRM.Service.CRMService.Modle.Enum;
using ETS2.PM.Service.PMService.Modle;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using ETS2.WeiXin.Service.WeiXinService.Model.Enum;
using ETS2.WeiXin.Service.WinXinService.BLL.WeiXinUploadDown;



namespace ETS2.WeiXin.Service.WinXinService.BLL
{
    public class WeiXinManage : System.Web.UI.Page
    {

        private static object lockobj = new object();

        public string httphead = "http://";//由于苹果手机微信链接 没有http:// 不识别，所以需要添加

        //private string materialdetailurl = "/weixin/ProDetail.aspx?materialid=";//精选推荐 素材详情链接(带预订)
        private string aboutmaterialdetailurl = "/weixin/wxmaterialdetail.aspx?materialid=";//关于微旅行 详情链接(不带预订)

        #region 第一次判断接口是否联通
        public void Auth(WeiXinBasic basicc)
        {

            string echoStr = System.Web.HttpContext.Current.Request.QueryString["echostr"];

            if (CheckSignature(basicc))
            {

                if (!string.IsNullOrEmpty(echoStr))
                {

                    System.Web.HttpContext.Current.Response.Write(echoStr);

                    System.Web.HttpContext.Current.Response.End();

                }

            }

        }
        #endregion


        #region 微信请求操作类
        public void Handle(string postStr, WeiXinBasic basic)
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


            //记录微信公众号ID
            if (basic.Weixinno == "")
            {
                int upweixinid = new WeiXinBasicData().ModifyWeiXinID(basic.Id, rootElement.SelectSingleNode("ToUserName").InnerText);
            }

            //获得微信用户基本信息
            GetWeiXinCrmBasic(basic, rootElement.SelectSingleNode("FromUserName").InnerText);



            lock (lockobj)
            {
                if (requestXML.MsgType == "text")
                {
                    lock (lockobj)
                    {
                        requestXML.Content = rootElement.SelectSingleNode("Content").InnerText;
                        requestXML.MsgId = rootElement.SelectSingleNode("MsgId").InnerText;
                        requestXML.ContentType = true;
                        requestXML.Comid = basic.Comid;

                        int inswxexchangemsg = new WxRequestXmlData().EditWxRequestXmlLog(requestXML);
                    }
                }
                else if (requestXML.MsgType == "voice")
                {
                    lock (lockobj)
                    {
                        requestXML.MediaId = rootElement.SelectSingleNode("MediaId").InnerText;
                        requestXML.Format = rootElement.SelectSingleNode("Format").InnerText;
                        requestXML.MsgId = rootElement.SelectSingleNode("MsgId").InnerText;
                        if (rootElement.SelectSingleNode("Recognition") != null)
                        {
                            requestXML.Recognition = rootElement.SelectSingleNode("Recognition").InnerText;
                        }
                        else
                        {
                            requestXML.Recognition = "";
                        }
                        requestXML.ContentType = true;
                        requestXML.Comid = basic.Comid;


                        int inswxexchangemsg = new WxRequestXmlData().EditWxRequestXmlLog(requestXML);
                    }
                }
                else if (requestXML.MsgType == "location")
                {
                    lock (lockobj)
                    {
                        requestXML.Location_X = rootElement.SelectSingleNode("Location_X").InnerText;

                        requestXML.Location_Y = rootElement.SelectSingleNode("Location_Y").InnerText;

                        requestXML.Scale = rootElement.SelectSingleNode("Scale").InnerText;

                        requestXML.Label = rootElement.SelectSingleNode("Label").InnerText;

                        requestXML.MsgId = rootElement.SelectSingleNode("MsgId").InnerText;

                        requestXML.Content = "地理位置消息推送";

                        requestXML.ContentType = true;
                        requestXML.Comid = basic.Comid;


                        int inswxexchangemsg = new WxRequestXmlData().EditWxRequestXmlLog(requestXML);
                    }

                }
                else if (requestXML.MsgType == "image")
                {
                    lock (lockobj)
                    {
                        requestXML.MediaId = rootElement.SelectSingleNode("MediaId").InnerText;
                        requestXML.PicUrl = rootElement.SelectSingleNode("PicUrl").InnerText;
                        requestXML.MsgId = rootElement.SelectSingleNode("MsgId").InnerText;
                        requestXML.Content = "图片消息推送";
                        requestXML.ContentType = true;
                        requestXML.Comid = basic.Comid;

                        int inswxexchangemsg = new WxRequestXmlData().EditWxRequestXmlLog(requestXML);
                    }
                }
                if (requestXML.MsgType == "link")
                {
                    lock (lockobj)
                    {
                        requestXML.Title = rootElement.SelectSingleNode("MediaId").InnerText;
                        requestXML.Description = rootElement.SelectSingleNode("PicUrl").InnerText;
                        requestXML.Url = rootElement.SelectSingleNode("Url").InnerText;
                        requestXML.MsgId = rootElement.SelectSingleNode("MsgId").InnerText;
                        requestXML.ContentType = true;
                        requestXML.Comid = basic.Comid;

                        int inswxexchangemsg = new WxRequestXmlData().EditWxRequestXmlLog(requestXML);
                    }
                }
                else if (requestXML.MsgType == "event")
                {
                    lock (lockobj)
                    {
                        #region (以下事件并没有记入数据库，所以感觉此方法没有，暂时先注释掉)根据用户(FromUserName) 和 消息创建时间(CreateTime) 判断事件是否接收过，接收过不在接收处理
                        //int Msg_isReceived = new WxRequestXmlData().JadgeMsg_isReceived(requestXML.FromUserName, requestXML.CreateTime);
                        //if (Msg_isReceived == 1)
                        //{
                        //    string emptystr = "";
                        //    System.Web.HttpContext.Current.Response.Write(emptystr);
                        //    return;
                        //}
                        #endregion

                        requestXML.Eevent = rootElement.SelectSingleNode("Event").InnerText;
                        requestXML.EventKey = rootElement.SelectSingleNode("EventKey") == null ? "" : rootElement.SelectSingleNode("EventKey").InnerText;

                        if (requestXML.Eevent == "scancode_push")
                        {
                            requestXML.ScanResult = rootElement.SelectSingleNode("ScanCodeInfo/ScanResult").InnerText;
                        }
                        if (requestXML.Eevent == "scancode_waitmsg")
                        {
                            requestXML.ScanResult = rootElement.SelectSingleNode("ScanCodeInfo/ScanResult").InnerText;
                        }
                        if (requestXML.Eevent == "LOCATION")
                        {
                            requestXML.Latitude = rootElement.SelectSingleNode("Latitude").InnerText;

                            requestXML.Longitude = rootElement.SelectSingleNode("Longitude").InnerText;

                            requestXML.Precision = rootElement.SelectSingleNode("Precision").InnerText;
                        }
                        if (requestXML.Eevent == "TEMPLATESENDJOBFINISH")//微信模板消息推送
                        {
                            requestXML.Tmplmsg_push_status = rootElement.SelectSingleNode("Status").InnerText;
                            requestXML.MsgId = rootElement.SelectSingleNode("MsgID").InnerText;
                            Weixin_templatemsg_sendlog log = new Weixin_templatemsg_sendlogData().GetTmplLogByMsgId(requestXML.MsgId);
                            if (log != null)
                            {
                                log.Msg_push_content = postStr;
                                log.Msg_push_CreateTime = requestXML.CreateTime;
                                log.Msg_push_CreateTime_format = requestXML.CreateTimeFormat;
                                log.Msg_push_status = requestXML.Tmplmsg_push_status;
                                log.Public_account = requestXML.ToUserName;

                                new Weixin_templatemsg_sendlogData().EditTmplLog(log);
                            }
                        }
                    }
                }



                #region (多客服暂时没有用到，已经注释掉) 将消息转发到多客服
                int rr = 0;
                string sql = "select istransfer_customer_service from b2b_company_info where com_id=" + basic.Comid;
                object o = ExcelSqlHelper.ExecuteScalar(sql);
                if (o != null)
                {
                    if (o.ToString() != "")
                    {
                        if (o.ToString() == "1" && requestXML.MsgType != "event")
                        {
                            rr = 1;
                            //转发到多客服
                            //System.Web.HttpContext.Current.Response.Write("<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + requestXML.CreateTime + "</CreateTime><MsgType><![CDATA[transfer_customer_service]]></MsgType></xml>");
                            //转发到特定多客服
                            //System.Web.HttpContext.Current.Response.Write("<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + requestXML.CreateTime + "</CreateTime><MsgType><![CDATA[transfer_customer_service]]></MsgType><TransInfo><KfAccount>xbh001@ETown_CN</KfAccount></TransInfo></xml>");
                            //return;

                            #region 将消息转发到特定多客服
                            lock (lockobj)
                            {
                                #region 判断当前商户5秒钟内是否获取过在线客服列表
                                bool is_huoqu = new Wx_onlinekf_huoqurecordData().Ishuoqued(basic.Comid);
                                //bool is_huoqu = false;
                                if (is_huoqu == false)//获取过，不在获取;没有获取过，重新获取在线客服列表,并且修改客服列表内客服的状态
                                {
                                    //根据公司id得到开发者凭据
                                    WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(basic.Comid);

                                    //获取凭证（首先判断数据库是否存在凭证以及是否过期，如过期，重新获取）
                                    WXAccessToken token = GetAccessToken(basicc.Comid, basicc.AppId, basicc.AppSecret);

                                    string url = "https://api.weixin.qq.com/cgi-bin/customservice/getonlinekflist?access_token=" + token.ACCESS_TOKEN;

                                    string ret = new GetUrlData().HttpGet(url);

                                    //把获取的在线客服记录 记入记录表
                                    Wx_onlinekf_huoqurecord record = new Wx_onlinekf_huoqurecord
                                    {
                                        Id = 0,
                                        Comid = basic.Comid,
                                        Huoqu_issuc = true,
                                        Huoqu_time = DateTime.Now,
                                        Huoqu_content = ret
                                    };
                                    int rrr = new Wx_onlinekf_huoqurecordData().InsertRecord(record);

                                    JavaScriptSerializer ser = new JavaScriptSerializer();
                                    OuterClass foo = ser.Deserialize<OuterClass>(ret);
                                    if (foo.kf_online_list != null)
                                    {
                                        List<InternalClass> data = foo.kf_online_list;

                                        if (data.Count > 0)
                                        {
                                            int h = new WxkfData().UpWxkf_downline(basic.Comid);//设置微信客服下线
                                            foreach (InternalClass ic in data)
                                            {
                                                //设置客服为在线状态
                                                int r = new WxkfData().Upwxkf_online(ic.kf_id, basic.Comid, ic.status, ic.auto_accept, ic.accepted_case);
                                            }
                                        }
                                    }
                                }
                                #endregion

                                //获取在线的全部客服列表
                                string isonline = "1";//在线状态：在线
                                string isrun = "1";//运行状态:运行
                                List<Wxkf> kf_list = new WxkfData().Getwxkflist(basic.Comid, isonline, isrun);
                                if (kf_list == null)
                                {
                                    ////客服都不在线/没有运行 转发到多客服
                                    //System.Web.HttpContext.Current.Response.Write("<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + requestXML.CreateTime + "</CreateTime><MsgType><![CDATA[transfer_customer_service]]></MsgType></xml>");
                                    //return;

                                    //客服都不在线，则不在转发给多客服
                                    rr = 0;
                                }
                                else
                                {
                                    if (kf_list.Count == 0)
                                    {
                                        ////客服都不在线/没有运行 转发到多客服，而不是到特定多客服
                                        //System.Web.HttpContext.Current.Response.Write("<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + requestXML.CreateTime + "</CreateTime><MsgType><![CDATA[transfer_customer_service]]></MsgType></xml>");
                                        //return;

                                        //客服都不在线，则不在转发给多客服
                                        rr = 0;
                                    }
                                    else
                                    {
                                        //客服有人在线 转发到多客服，而不是到特定多客服
                                        System.Web.HttpContext.Current.Response.Write("<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + requestXML.CreateTime + "</CreateTime><MsgType><![CDATA[transfer_customer_service]]></MsgType></xml>");
                                        return;

                                        #region 把消息转发到在线客服，客服接入顺序a.转给渠道人客服b.转给上次接入客服C.转给门市客服(主客服优先，然后随机分配给其他在线客服)D.转给公司客服(总客服) */

                                        //////在线微信客服工号列表
                                        ////List<string> online_wxkfaccountlist = new List<string>();
                                        ////foreach (Wxkf mmm in kf_list)
                                        ////{
                                        ////    online_wxkfaccountlist.Add(mmm.Kf_account);
                                        ////}

                                        ////得到会员所在渠道
                                        //Member_Channel m_channel = new MemberChannelData().GetChannelByOpenId(requestXML.FromUserName);

                                        ////获得当前用户最近一次的接入记录
                                        //Wx_kfinsert_record lastrecord = new Wx_kfinsert_recordData().GetLastRecord(requestXML.FromUserName, basic.Comid);

                                        //#region a.转发给渠道人客服
                                        //if (m_channel != null)
                                        //{
                                        //    //渠道为默认渠道；渠道类型为网站注册/微信注册；渠道状态为暂停 则不会得到转发消息
                                        //    if (m_channel.Whetherdefaultchannel == 1 || m_channel.Issuetype == 3 || m_channel.Issuetype == 4 || m_channel.Runstate == 0)
                                        //    {
                                        //    }
                                        //    else
                                        //    {
                                        //        //获得渠道人对应在线客服
                                        //        Wxkf mm_wxkf = new WxkfData().GetChannel_Wxkf(m_channel.Id, basic.Comid, isonline, isrun);
                                        //        if (mm_wxkf != null)
                                        //        {
                                        //            //判断对应客服是否在 在线客服列表中
                                        //            //if (online_wxkfaccountlist.Contains(mm_wxkf.Kf_account))
                                        //            //{
                                        //            if (lastrecord == null)
                                        //            {
                                        //                //向客服接入记录表中录入数据
                                        //                Wx_kfinsert_record kfinsert_record = new Wx_kfinsert_record
                                        //                {
                                        //                    Id = 0,
                                        //                    Comid = basic.Comid,
                                        //                    Kf_id = mm_wxkf.Kf_id,
                                        //                    Kf_account = mm_wxkf.Kf_account,
                                        //                    Openid = requestXML.FromUserName,
                                        //                    Lastinserttime = DateTime.Now
                                        //                };
                                        //                int ins = new Wx_kfinsert_recordData().Insertkfinsert_record(kfinsert_record);

                                        //            }
                                        //            else
                                        //            {
                                        //                if (lastrecord.Kf_id != mm_wxkf.Kf_id)
                                        //                {
                                        //                    //向客服接入记录表中录入数据
                                        //                    Wx_kfinsert_record kfinsert_record = new Wx_kfinsert_record
                                        //                    {
                                        //                        Id = 0,
                                        //                        Comid = basic.Comid,
                                        //                        Kf_id = mm_wxkf.Kf_id,
                                        //                        Kf_account = mm_wxkf.Kf_account,
                                        //                        Openid = requestXML.FromUserName,
                                        //                        Lastinserttime = DateTime.Now
                                        //                    };
                                        //                    int ins = new Wx_kfinsert_recordData().Insertkfinsert_record(kfinsert_record);

                                        //                }
                                        //            }
                                        //            //转发到特定多客服
                                        //            System.Web.HttpContext.Current.Response.Write("<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + requestXML.CreateTime + "</CreateTime><MsgType><![CDATA[transfer_customer_service]]></MsgType><TransInfo><KfAccount>" + mm_wxkf.Kf_account + "</KfAccount></TransInfo></xml>");
                                        //            return;
                                        //            //}
                                        //        }
                                        //    }
                                        //}
                                        //#endregion

                                        //#region b.转给上次接入客服
                                        //if (lastrecord != null)
                                        //{
                                        //    ////判断对应客服是否在 在线客服列表中
                                        //    //if (online_wxkfaccountlist.Contains(m_kfins_record.Kf_account))
                                        //    //{
                                        //    Wxkf kff = new WxkfData().Getwxkf(lastrecord.Kf_id, basic.Comid);
                                        //    if (kff != null)
                                        //    {
                                        //        //判断上次接入的客服是否在线以及是否运行
                                        //        if (kff.Isonline > 0 && kff.Isrun > 0)
                                        //        {
                                        //            //不在向客服接入记录表中录入数据
                                        //            //转发到特定多客服
                                        //            System.Web.HttpContext.Current.Response.Write("<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + requestXML.CreateTime + "</CreateTime><MsgType><![CDATA[transfer_customer_service]]></MsgType><TransInfo><KfAccount>" + kff.Kf_account + "</KfAccount></TransInfo></xml>");
                                        //            return;
                                        //        }
                                        //    }
                                        //    //}
                                        //}
                                        //#endregion

                                        //#region 转给门市客服(主客服优先，然后随机分配给其他在线客服)
                                        //if (m_channel != null)
                                        //{
                                        //    if (m_channel.Companyid > 0)
                                        //    {
                                        //        //获得渠道门市的在线客服列表
                                        //        List<Wxkf> ms_wxkflist = new WxkfData().GetMs_wxkflist(m_channel.Companyid, isonline, isrun);
                                        //        if (ms_wxkflist != null)
                                        //        {
                                        //            List<int> ms_wxkfidlist = new List<int>();
                                        //            if (ms_wxkflist.Count > 0)
                                        //            {
                                        //                string zkf_account = "";//总客服工号
                                        //                foreach (Wxkf ff in ms_wxkflist)
                                        //                {
                                        //                    if (ff.Iszongkf == 1)
                                        //                    {
                                        //                        zkf_account = ff.Kf_account;
                                        //                        //向客服接入记录表中录入数据
                                        //                        Wx_kfinsert_record kfinsert_record = new Wx_kfinsert_record
                                        //                        {
                                        //                            Id = 0,
                                        //                            Comid = basic.Comid,
                                        //                            Kf_id = ff.Kf_id,
                                        //                            Kf_account = ff.Kf_account,
                                        //                            Openid = requestXML.FromUserName,
                                        //                            Lastinserttime = DateTime.Now
                                        //                        };
                                        //                        int ins = new Wx_kfinsert_recordData().Insertkfinsert_record(kfinsert_record);
                                        //                    }
                                        //                    ms_wxkfidlist.Add(ff.Kf_id);
                                        //                }
                                        //                if (zkf_account != "")
                                        //                {
                                        //                    //转发到特定多客服
                                        //                    System.Web.HttpContext.Current.Response.Write("<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + requestXML.CreateTime + "</CreateTime><MsgType><![CDATA[transfer_customer_service]]></MsgType><TransInfo><KfAccount>" + zkf_account + "</KfAccount></TransInfo></xml>");
                                        //                    return;
                                        //                }
                                        //                //随机取出一个客服id
                                        //                Random r = new Random();
                                        //                int j = r.Next(0, ms_wxkfidlist.Count);

                                        //                //向客服接入记录表中录入数据
                                        //                Wx_kfinsert_record kfinsert_record2 = new Wx_kfinsert_record
                                        //                {
                                        //                    Id = 0,
                                        //                    Comid = basic.Comid,
                                        //                    Kf_id = ms_wxkflist[j].Kf_id,
                                        //                    Kf_account = ms_wxkflist[j].Kf_account,
                                        //                    Openid = requestXML.FromUserName,
                                        //                    Lastinserttime = DateTime.Now
                                        //                };
                                        //                int ins2 = new Wx_kfinsert_recordData().Insertkfinsert_record(kfinsert_record2);
                                        //                //转发到特定多客服
                                        //                System.Web.HttpContext.Current.Response.Write("<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + requestXML.CreateTime + "</CreateTime><MsgType><![CDATA[transfer_customer_service]]></MsgType><TransInfo><KfAccount>" + ms_wxkflist[j].Kf_account + "</KfAccount></TransInfo></xml>");
                                        //                return;
                                        //            }
                                        //        }
                                        //    }

                                        //}
                                        //#endregion

                                        //#region 转给公司客服(总客服)
                                        //if (m_channel != null)
                                        //{
                                        //    //得到总公司在线客服列表
                                        //    List<Wxkf> ms_wxkflist = new WxkfData().GetGs_wxkflist(basic.Comid, isonline, isrun);
                                        //    if (ms_wxkflist != null)
                                        //    {
                                        //        List<int> ms_wxkfidlist = new List<int>();
                                        //        if (ms_wxkflist.Count > 0)
                                        //        {
                                        //            string zkf_account = "";//总客服工号
                                        //            foreach (Wxkf ff in ms_wxkflist)
                                        //            {
                                        //                if (ff.Iszongkf == 1)
                                        //                {
                                        //                    zkf_account = ff.Kf_account;
                                        //                    //向客服接入记录表中录入数据
                                        //                    Wx_kfinsert_record kfinsert_record = new Wx_kfinsert_record
                                        //                    {
                                        //                        Id = 0,
                                        //                        Comid = basic.Comid,
                                        //                        Kf_id = ff.Kf_id,
                                        //                        Kf_account = ff.Kf_account,
                                        //                        Openid = requestXML.FromUserName,
                                        //                        Lastinserttime = DateTime.Now
                                        //                    };
                                        //                    int ins = new Wx_kfinsert_recordData().Insertkfinsert_record(kfinsert_record);
                                        //                }
                                        //                ms_wxkfidlist.Add(ff.Kf_id);
                                        //            }
                                        //            if (zkf_account != "")
                                        //            {
                                        //                //转发到特定多客服
                                        //                System.Web.HttpContext.Current.Response.Write("<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + requestXML.CreateTime + "</CreateTime><MsgType><![CDATA[transfer_customer_service]]></MsgType><TransInfo><KfAccount>" + zkf_account + "</KfAccount></TransInfo></xml>");
                                        //                return;
                                        //            }

                                        //            //随机取出一个客服id
                                        //            Random r = new Random();
                                        //            int j = r.Next(0, ms_wxkfidlist.Count);

                                        //            //向客服接入记录表中录入数据
                                        //            Wx_kfinsert_record kfinsert_record2 = new Wx_kfinsert_record
                                        //            {
                                        //                Id = 0,
                                        //                Comid = basic.Comid,
                                        //                Kf_id = ms_wxkflist[j].Kf_id,
                                        //                Kf_account = ms_wxkflist[j].Kf_account,
                                        //                Openid = requestXML.FromUserName,
                                        //                Lastinserttime = DateTime.Now
                                        //            };
                                        //            int ins2 = new Wx_kfinsert_recordData().Insertkfinsert_record(kfinsert_record2);
                                        //            //转发到特定多客服
                                        //            System.Web.HttpContext.Current.Response.Write("<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + requestXML.CreateTime + "</CreateTime><MsgType><![CDATA[transfer_customer_service]]></MsgType><TransInfo><KfAccount>" + ms_wxkflist[j].Kf_account + "</KfAccount></TransInfo></xml>");
                                        //            return;
                                        //        }
                                        //    }
                                        //}
                                        //#endregion

                                        #endregion
                                    }
                                }

                            }
                            #endregion

                        }
                    }
                }
                #endregion
                //回复消息
                if (rr == 0)
                {
                    try
                    {
                        ResponseMsg(requestXML, basic);
                        if (requestXML.FromUserName != "")
                        {
                            //如果微信取消关注的话，则用户表 微信关注状态调为:未关注(0)； 微信激活状态为：激活(1); 
                            if (requestXML.MsgType == "event")
                            {
                                if (requestXML.Eevent == "unsubscribe")
                                { }
                                else
                                {
                                    int ddd = new B2bCrmData().UpWxsubstate(basic.Comid, requestXML.FromUserName, 1, 1);
                                }
                            }
                            else //只有进入微信请求操作类：则 用户表 微信关注状态调为：关注(1) 微信激活状态为：激活(1);
                            {
                                int ddd = new B2bCrmData().UpWxsubstate(basic.Comid, requestXML.FromUserName, 1, 1);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        string emptystr = "";
                        System.Web.HttpContext.Current.Response.Write(emptystr);
                    }
                }
            }
        }



        #region 判断商家是否为认证商家：认证商家的话获得微信用户基本信息(每天获取一次)
        private void GetWeiXinCrmBasic(WeiXinBasic basic, string fromusername)
        {
            try
            {
                bool whetherrz = new WxRequestXmlData().JudgeWhetherRenZheng(basic.Comid);
                if (whetherrz)
                {
                    //判断是否含有此微信用户的微信基本信息
                    WxMemberBasic mWxmemberbasic = new WxMemberBasicData().Getwxmemberbasic(fromusername);
                    if (mWxmemberbasic != null)
                    {
                        if (mWxmemberbasic.renewtime.ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd"))
                        {
                            //获取凭证（首先判断数据库是否存在凭证以及是否过期，如过期，重新获取）
                            WXAccessToken token = GetAccessToken(basic.Comid, basic.AppId, basic.AppSecret);
                            string createmenuurl = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + token.ACCESS_TOKEN + "&openid=" + fromusername + "&lang=zh_CN";

                            string createmenuutret = new GetUrlData().HttpGet(createmenuurl);
                            XmlDocument createselfmenudoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + createmenuutret + "}");
                            XmlElement createselfmenurootElement = createselfmenudoc.DocumentElement;
                            System.Xml.XmlNode createerrcode = createselfmenurootElement.SelectSingleNode("errcode");
                            if (createerrcode == null)
                            {
                                WxMemberBasic memberbasic = new WxMemberBasic();
                                memberbasic.Id = mWxmemberbasic.Id;
                                memberbasic.Subscribe = createselfmenurootElement.SelectSingleNode("subscribe").InnerText.ConvertTo<int>(0);
                                memberbasic.Openid = createselfmenurootElement.SelectSingleNode("openid").InnerText;
                                memberbasic.Nickname = createselfmenurootElement.SelectSingleNode("nickname").InnerText;
                                memberbasic.Sex = createselfmenurootElement.SelectSingleNode("sex").InnerText.ConvertTo<int>(0);
                                memberbasic.Language = createselfmenurootElement.SelectSingleNode("language").InnerText;
                                memberbasic.City = createselfmenurootElement.SelectSingleNode("city").InnerText;
                                memberbasic.Province = createselfmenurootElement.SelectSingleNode("province").InnerText;
                                memberbasic.Country = createselfmenurootElement.SelectSingleNode("country").InnerText;
                                memberbasic.Headimgurl = createselfmenurootElement.SelectSingleNode("headimgurl").InnerText.Replace("\\", "");
                                memberbasic.Subscribe_time = UnixTimeToTime(createselfmenurootElement.SelectSingleNode("subscribe_time").InnerText);
                                memberbasic.Comid = basic.Comid;
                                memberbasic.renewtime = DateTime.Now;

                                int editwxmemeberbasic = new WxMemberBasicData().EditWxMemeberBasic(memberbasic);

                                //覆盖会员表中国家，省市，地区 
                                int coverregion = new B2bCrmData().CoverRegion(memberbasic.Openid, memberbasic.Country, memberbasic.Province, memberbasic.City);

                            }
                        }
                    }
                    else
                    {
                        //获取凭证（首先判断数据库是否存在凭证以及是否过期，如过期，重新获取）
                        WXAccessToken token = GetAccessToken(basic.Comid, basic.AppId, basic.AppSecret);
                        string createmenuurl = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + token.ACCESS_TOKEN + "&openid=" + fromusername + "&lang=zh_CN";

                        string createmenuutret = new GetUrlData().HttpGet(createmenuurl);
                        XmlDocument createselfmenudoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + createmenuutret + "}");
                        XmlElement createselfmenurootElement = createselfmenudoc.DocumentElement;
                        System.Xml.XmlNode createerrcode = createselfmenurootElement.SelectSingleNode("errcode");
                        if (createerrcode == null)
                        {
                            WxMemberBasic memberbasic = new WxMemberBasic();
                            memberbasic.Id = 0;
                            memberbasic.Subscribe = createselfmenurootElement.SelectSingleNode("subscribe").InnerText.ConvertTo<int>(0);
                            memberbasic.Openid = createselfmenurootElement.SelectSingleNode("openid").InnerText;
                            memberbasic.Nickname = createselfmenurootElement.SelectSingleNode("nickname").InnerText;
                            memberbasic.Sex = createselfmenurootElement.SelectSingleNode("sex").InnerText.ConvertTo<int>(0);
                            memberbasic.Language = createselfmenurootElement.SelectSingleNode("language").InnerText;
                            memberbasic.City = createselfmenurootElement.SelectSingleNode("city").InnerText;
                            memberbasic.Province = createselfmenurootElement.SelectSingleNode("province").InnerText;
                            memberbasic.Country = createselfmenurootElement.SelectSingleNode("country").InnerText;
                            memberbasic.Headimgurl = createselfmenurootElement.SelectSingleNode("headimgurl").InnerText.Replace("\\", "");
                            memberbasic.Subscribe_time = UnixTimeToTime(createselfmenurootElement.SelectSingleNode("subscribe_time").InnerText);
                            memberbasic.Comid = basic.Comid;
                            memberbasic.renewtime = DateTime.Now;

                            int editwxmemeberbasic = new WxMemberBasicData().EditWxMemeberBasic(memberbasic);

                            //覆盖会员表中国家，省市，地区 
                            int coverregion = new B2bCrmData().CoverRegion(memberbasic.Openid, memberbasic.Country, memberbasic.Province, memberbasic.City);

                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }

        }
        #endregion
        #endregion


        #region 验证微信签名
        /// <summary>

        /// 验证微信签名

        /// </summary>

        /// * 将signature  timestamp nonce 三个参数进行字典排序

        /// * 将三个参数连接成一个字符串进行加密

        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信

        /// <returns></returns>

        private bool CheckSignature(WeiXinBasic basicc)
        {

            if (basicc == null)
            {
                return false;
            }
            else
            {
                string Token = basicc.Token;

                string signature = System.Web.HttpContext.Current.Request.QueryString["signature"];

                string timestamp = System.Web.HttpContext.Current.Request.QueryString["timestamp"];

                string nonce = System.Web.HttpContext.Current.Request.QueryString["nonce"];

                string[] ArrTmp = { Token, timestamp, nonce };

                Array.Sort(ArrTmp);     //字典排序

                string tmpStr = string.Join("", ArrTmp);

                tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");

                tmpStr = tmpStr.ToLower();

                if (tmpStr == signature)
                {

                    return true;

                }

                else
                {

                    return false;

                }

            }
        }

        #endregion

        /// <summary>
        /// 获取凭证（首先判断数据库是否存在凭证以及是否过期，如过期，重新获取）
        /// </summary>
        /// <returns></returns>
        public static WXAccessToken GetAccessToken(int comid, string AppId, string AppSecret)
        {
            DateTime fitcreatetime = DateTime.Now.AddHours(-2);
            WXAccessToken token = new WXAccessTokenData().GetLaststWXAccessToken(fitcreatetime, comid);
            if (token == null)
            {
                string geturl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppId + "&secret=" + AppSecret;
                string jsonText = new GetUrlData().HttpGet(geturl);

                XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + jsonText + "}");

                XmlElement rootElement = doc.DocumentElement;
                string access_token = rootElement.SelectSingleNode("access_token").InnerText;

                //把获取到的凭证录入数据库中
                token = new WXAccessToken()
                {
                    Id = 0,
                    ACCESS_TOKEN = access_token,
                    CreateDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    Comid = comid

                };
                int edittoken = new WXAccessTokenData().EditAccessToken(token);
            }
            return token;
        }

        /// <summary>

        /// 回复消息（微信消息返回）

        /// </summary>

        /// <param name="weixinXML"></param>

        private void ResponseMsg(RequestXML requestXML, WeiXinBasic basic)
        {

            try
            {
                string resxml = "";
                int channelid = 0;
                //获取会员用户信息
                B2b_crm crmdata_temp = new B2bCrmData().GetB2bCrmByWeiXin(requestXML.FromUserName, basic.Comid);
                if (crmdata_temp != null)
                {
                    MemberCardData carddata = new MemberCardData();
                    var cardinfo = carddata.GetCardByCardNumber(crmdata_temp.Idcard);
                    if (cardinfo != null)
                    {
                        channelid = Int32.Parse(cardinfo.IssueCard.ToString());
                    }
                }


                #region 除了关注事件外，其他情况都会判断用户是否为会员，不是会员的话录入数据库
                if (requestXML.MsgType == "event")
                {
                    if (requestXML.Eevent == "subscribe")
                    { }
                    else
                    {
                        B2b_crm weixincrm = new B2bCrmData().GetB2bCrmByWeiXin(requestXML.FromUserName, basic.Comid);
                        //如果此公司没有此微信的会员信息，则录入
                        if (weixincrm == null)
                        {
                            resxml = InsB2bCrm(basic, requestXML, "");

                            MemberCardData carddata = new MemberCardData();
                            var cardinfo = carddata.GetCardByCardNumber(weixincrm.Idcard);
                            if (cardinfo != null)
                            {
                                channelid = Int32.Parse(cardinfo.IssueCard.ToString());
                            }

                        }
                    }
                }
                else
                {
                    B2b_crm weixincrm = new B2bCrmData().GetB2bCrmByWeiXin(requestXML.FromUserName, basic.Comid);
                    //如果此公司没有此微信的会员信息，则录入
                    if (weixincrm == null)
                    {
                        resxml = InsB2bCrm(basic, requestXML, "");

                        MemberCardData carddata = new MemberCardData();
                        var cardinfo = carddata.GetCardByCardNumber(weixincrm.Idcard);
                        if (cardinfo != null)
                        {
                            channelid = Int32.Parse(cardinfo.IssueCard.ToString());
                        }

                    }
                }
                #endregion

                //记录微信用户最后一次互动时间
                int recordinteracttime = new B2bCrmData().RecordInteracttime(basic.Comid, requestXML.FromUserName, DateTime.Now);

                //判断是否是顾问，1是；0不是
                int isguwen = new MemberChannelData().IsGuwen(requestXML.FromUserName, basic.Comid);

                #region 默认回复消息
                string defaultret = basic.Leavemsgautoreply;
                defaultret = defaultret.Replace("<p>", "").Replace("</p>", "\r\n").Replace("<br />", "\r\n").Replace("&ldquo;", "\"").Replace("&rdquo;", "\"").Replace("&nbsp;", " ");
                #endregion
                #region 接收普通消息:文本消息
                if (requestXML.MsgType == "text")
                {
                    #region 微信输入文本信息
                    //发送验证码成功后返回内容
                    string sendcorrect = "已经发送验证码短信到该手机号，请在30分钟内输入您收到的验证码数字。60秒钟内仍没有收到短信可输入手机号重发验证码";
                    //验证码发送超出次数限制后返回内容
                    string sendmorelimitenum = "你重发验证码次数超限，请在30分钟后，重新申请发送验证码";
                    //验证码重发频率过高（1分钟）后返回内容
                    string sendmorefastnum = "重发请求频率过快，请1分钟后重新请求";
                    //验证码过期提示
                    string verifycodeexpired = "你输入的验证码已经过期，请输入手机号重发新验证码短信";
                    //验证码输入错误
                    string verifycodeerror = "你输入的验证码有误，请重新核实验证码信息，或输入手机号重发验证码短信";
                    //验证码输入正确
                    string verifycodecorrect = "您已经成功关联手机号码，请点击 会员卡服务专区 查看";
                    //如果微信已经有手机号
                    string hadphone = "本微信帐号已经关联了手机号码，请在 会员卡服务专区 中查看，如果该手机号不是您的，或需要更换，请选择《解除关联手机号》操作后，重新关联手机号";
                    //如果手机号已经有微信
                    string hadweixin = "本手机号码已经关联了微信帐号，请在 会员卡服务专区 中查看，如果该手机号不是您的，或需要更换，请选择《解除关联手机号》操作后，重新关联手机号";

                    //没有邀请码记录
                    string noinvitecoderecord = "没有找到邀请码记录";

                    //是否是顾问文字留言 
                    bool Ismarkedandnotdeal = false;
                    if (isguwen == 1)
                    {
                        //if (requestXML.Content == "录制我的语音" || requestXML.Content.ToLower() == "wyly" || requestXML.Content == "我要留言")
                        //{
                        //    //设定并获取微信随机码                              
                        //    var pass = new B2bCrmData().WeixinGetPass(requestXML.FromUserName, basic.Comid);

                        //    //根据微信号获得会员信息
                        //    B2b_crm crminfo = new B2bCrmData().GetB2bCrmByWeiXin(requestXML.FromUserName, basic.Comid);
                        //    if (crminfo == null)//会员信息为空时，添加此会员信息
                        //    {
                        //        resxml = InsB2bCrm(basic, requestXML, "");
                        //    }


                        //    string linkurl = "http://shop" + basic.Comid + ".etown.cn/h5/peoplevoiceup.aspx?comid=" + basic.Comid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass + "&clientuptypemark=" + (int)Clientuptypemark.DownGreetVoice;


                        //    string str = "点击 <a href='" + linkurl + "'>录制我的语音</a>\n";


                        //    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + str + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                        //}

                        if (requestXML.Content == "我的消息" || requestXML.Content.ToLower() == "wdxx")
                        {
                            int count = 0;
                            var wxxmldata = new WxRequestXmlData();
                            var wxxmlinfo = wxxmldata.GetWxErrSendMsgList(requestXML.Comid, requestXML.FromUserName, out count);

                            if (wxxmlinfo != null)
                            {

                                //循环发送
                                for (int i = 0; i < count; i++)
                                {
                                    var upstate = wxxmldata.UpWxErrSendMsgList(wxxmlinfo[i].Id);//更改为 已发送
                                    int type = 0;
                                    if (wxxmlinfo[i].MsgType == "text")
                                    {
                                        type = 1;
                                    }
                                    if (wxxmlinfo[i].MsgType == "voice")
                                    {
                                        type = 2;
                                    }
                                    if (wxxmlinfo[i].MsgType == "image")
                                    {
                                        type = 3;
                                    }
                                    var sendmsg = CustomerMsg_Send.SendWxMsg(requestXML.Comid, wxxmlinfo[i].ToUserName, type, wxxmlinfo[i].PicUrl, wxxmlinfo[i].Content, wxxmlinfo[i].MediaId, wxxmlinfo[i].FromUserName);
                                }
                            }

                            //最后返回个空值
                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[]]></Content><FuncFlag>1</FuncFlag></xml>";

                        }


                        if (requestXML.Content.ToLower().Substring(0, 2) == "qx" || requestXML.Content.ToLower().Substring(0, 2) == "qr")
                        {

                            //if (requestXML.Content.ToLower().Substring(0, 2) == "qr")
                            //{
                            //    //截取前两个字qr，进入确认流程
                            //    int orderid = int.Parse(requestXML.Content.ToLower().Substring(2, requestXML.Content.Length - 2));
                            //    //var snsback = ETS.OrderJsonData.UporderPaystate(orderid, "qr", requestXML.Content);//印在 WeiXinManage下无法调用josn 
                            //}
                            //string str = "";


                            //resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + str + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                        }

                        //是否是顾问文字留言 
                        Ismarkedandnotdeal = new Wxmedia_updownlogData().Ismarkedandnotdeal(requestXML.FromUserName, (int)Clientuptypemark.DownTxt);
                    }
                    #region 顾问文字留言
                    if (Ismarkedandnotdeal)
                    {
                        //得到含有已经标记过"上传文字留言" 但是没有完成上传 的记录
                        Wxmedia_updownlog udlog = new Wxmedia_updownlogData().GetMarkedAndNotdeallog(requestXML.FromUserName, (int)Clientuptypemark.DownTxt);
                        if (udlog == null)
                        {
                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[上传文字留言失败.]]></Content><FuncFlag>1</FuncFlag></xml>";
                        }
                        else
                        {
                            udlog.mediaid = "";
                            udlog.mediatype = "txt";
                            udlog.savepath = "";
                            udlog.created_at = ConvertDateTimeInt(DateTime.Now).ToString();
                            udlog.createtime = DateTime.Now;
                            udlog.opertype = "down";//下载顾问文字留言
                            udlog.isfinish = 1;
                            udlog.txtcontent = requestXML.Content;


                            int udlogresult = new Wxmedia_updownlogData().Edituploadlog(udlog);
                            if (udlogresult > 0)
                            {
                                resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[上传文字留言成功.]]></Content><FuncFlag>1</FuncFlag></xml>";
                            }
                            else
                            {
                                resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[上传文字留言失败..]]></Content><FuncFlag>1</FuncFlag></xml>";
                            }
                        }
                    }
                    #endregion
                    #region 非顾问文字留言
                    else
                    {

                        //if (requestXML.Content == "我的消息" || requestXML.Content.ToLower() == "wdxx")
                        //{
                        //    int count = 0;
                        //    var wxxmldata = new WxRequestXmlData();
                        //    var wxxmlinfo = wxxmldata.GetWxErrSendMsgList(requestXML.Comid, requestXML.FromUserName, out count);

                        //    if (wxxmlinfo != null)
                        //    {

                        //        //循环发送
                        //        for (int i = 0; i < count; i++)
                        //        {
                        //            var upstate = wxxmldata.UpWxErrSendMsgList(wxxmlinfo[i].Id);//更改为 已发送
                        //            int type = 0;
                        //            if (wxxmlinfo[i].MsgType == "text")
                        //            {
                        //                type = 1;
                        //            }
                        //            if (wxxmlinfo[i].MsgType == "voice")
                        //            {
                        //                type = 2;
                        //            }
                        //            if (wxxmlinfo[i].MsgType == "image")
                        //            {
                        //                type = 3;
                        //            }
                        //            var sendmsg = CustomerMsg_Send.SendWxMsg(requestXML.Comid, wxxmlinfo[i].ToUserName, type, wxxmlinfo[i].PicUrl, wxxmlinfo[i].Content, wxxmlinfo[i].MediaId, wxxmlinfo[i].FromUserName);
                        //        }
                        //    }

                        //    //最后返回个空值
                        //    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[]]></Content><FuncFlag>1</FuncFlag></xml>";

                        //}


                        #region 输入手机号
                        if (System.Text.RegularExpressions.Regex.IsMatch(requestXML.Content, @"^[1][3-8]\d{9}"))//输入手机号：向手机号发送短信验证码；
                        {
                            bool isbind = false;
                            //1,此微信是否绑定过手机：a.绑定过给出提示;b.未绑定（2.判断会员表中是否有绑定此手机号的会员：a.不含有：直接录入手机号b.含有（3.判断手机是否绑定过微信:a.绑定过给出提示b.未绑定则合并微信手机账户））

                            //此次实现 1.验证微信是否绑定过手机2.验证手机绑定过微信
                            B2b_crm weixincrm = new B2bCrmData().GetB2bCrmByWeiXin(requestXML.FromUserName, basic.Comid);
                            //如果此公司没有此微信的会员信息，则录入
                            if (weixincrm == null)
                            {
                                resxml = InsB2bCrm(basic, requestXML, "");
                            }

                            if (weixincrm.Phone.ConvertTo<decimal>(0) == 0)
                            {
                                B2b_crm phonecrm = new B2bCrmData().GetB2bCrmByPhone(requestXML.Content, basic.Comid);
                                if (phonecrm != null)
                                {
                                    if (phonecrm.Weixin.ConvertTo<string>("") != "") //此手机绑定过微信
                                    {
                                        isbind = true;
                                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + hadweixin + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                    }
                                }
                            }
                            else //此微信已绑定手机号
                            {
                                isbind = true;
                                resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + hadphone + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                            }


                            if (isbind == false)//如果满足条件，发送验证码
                            {
                                //创建随机码
                                Random ra = new Random();
                                decimal code = ra.Next(100000, 999999);
                                //向手机发送验证码
                                string phone = requestXML.Content;

                                int sendnum = 0;//向手机发送短信的次数（半小时内）
                                //半小时内向此手机发送短信地记录
                                Phone_codemodel sendrecord = Phone_code.code_info(decimal.Parse(phone), basic.Comid, requestXML.FromUserName);

                                if (sendrecord != null)
                                {
                                    sendnum = sendrecord.Codenum + 1;

                                    //此次发送短信时间与最后一次发送短信地时间之差
                                    int numtime = DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 60 * 60 - sendrecord.Codetime.Second - sendrecord.Codetime.Minute * 60 - sendrecord.Codetime.Hour * 60 * 60;
                                    if (numtime < 60)//重复点击
                                    {
                                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + sendmorefastnum + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                    }
                                    else
                                    {
                                        if (sendrecord.Codenum > 2)//防止大量重发
                                        {
                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + sendmorelimitenum + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                        }
                                        else
                                        {
                                            //修改状态为1，可以发送
                                            string data = Phone_code.upcode(decimal.Parse(phone), sendrecord.Code, basic.Comid, sendnum, 1, requestXML.FromUserName);

                                            SendSmsHelper.GetMember_sms(phone.ToString(), "", "", sendrecord.Code.ToString(), 0, "随机码", basic.Comid, sendnum);
                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + sendcorrect + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                        }
                                    }
                                }
                                else
                                {
                                    sendnum = 1;
                                    string data = Phone_code.insertcode(decimal.Parse(phone), code, basic.Comid, sendnum, requestXML.FromUserName);

                                    var list = Phone_code.code_info(decimal.Parse(phone), basic.Comid, requestXML.FromUserName);
                                    if (list != null)
                                    {
                                        SendSmsHelper.GetMember_sms(phone.ToString(), "", "", code.ToString(), 0, "随机码", basic.Comid, sendnum);
                                    }
                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + sendcorrect + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                }
                            }


                        }
                        #endregion
                        #region 输入验证码
                        else if (System.Text.RegularExpressions.Regex.IsMatch(requestXML.Content, @"\d{6}") && requestXML.Content.Length == 6)//输入验证码:1，绑定手机；2，合并手机和微信
                        {
                            if (int.Parse(requestXML.Content) >= 100000 && int.Parse(requestXML.Content) <= 999999)
                            {
                                //根据微信号得到30分钟内短信发码记录，判断验证码是否匹配
                                Phone_codemodel sendrecord = Phone_code.GetNoteRecord(requestXML.FromUserName, basic.Comid);
                                if (sendrecord != null)
                                {
                                    if (sendrecord.Code.ToString() == requestXML.Content)
                                    {

                                        //绑定手机，微信：会员表中当前微信号变为空改写到含有绑定手机的记录中;同时预付款，服务费也需要变动
                                        //resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[验证码验证成功]]></Content><FuncFlag>1</FuncFlag></xml>";


                                        //1,此微信是否绑定过手机：a.绑定过给出提示;b.未绑定（2.判断会员表中是否有绑定此手机号的会员：a.不含有：直接录入手机号b.含有（3.判断手机是否绑定过微信:a.绑定过给出提示b.未绑定则合并微信手机账户））
                                        B2b_crm weixincrm = new B2bCrmData().GetB2bCrmByWeiXin(requestXML.FromUserName, basic.Comid);
                                        //如果此公司没有此微信的会员信息，则录入
                                        if (weixincrm == null)
                                        {
                                            resxml = InsB2bCrm(basic, requestXML, "");
                                            weixincrm = new B2bCrmData().GetB2bCrmByWeiXin(requestXML.FromUserName, basic.Comid);
                                        }
                                        if (weixincrm.Phone.ConvertTo<decimal>(0) == 0)
                                        {

                                            B2b_crm phonecrm = new B2bCrmData().GetB2bCrmByPhone(sendrecord.Phone.ToString(), basic.Comid);
                                            if (phonecrm != null)
                                            {
                                                if (phonecrm.Weixin.ConvertTo<string>("") == "")
                                                {
                                                    //合并手机微信账户：a.会员表变动-原微信账户的手机(phone)，预付款(imprest),积分或者叫积分(integral)合并到手机号对应的会员信息中
                                                    //-----------------b.订单表变动-原微信号的订单中uid变动：变为手机微信合并后的手机号对应的会员号uid
                                                    //-----------------2014.04.14 c.原微信账户对应渠道修改为微信注册渠道，而合并后的手机账户对应渠道修改为原微信对应的渠道


                                                    #region 事务执行，出错回滚
                                                    string err_msg = "";
                                                    SqlHelper sqlhelper = new SqlHelper();
                                                    sqlhelper.BeginTrancation();
                                                    try
                                                    {
                                                        SqlCommand cmd = new SqlCommand();

                                                        //把订单表中公司此微信号的订单uid赋值
                                                        string sql3 = "update b2b_order set u_id='" + phonecrm.Id + "'  where openid='" + requestXML.FromUserName + "' and comid=" + basic.Comid;
                                                        cmd = sqlhelper.PrepareTextSqlCommand(sql3);
                                                        cmd.ExecuteNonQuery();
                                                        //修改原手机账户对应渠道
                                                        string OriginalWxChannelId = "select issuecard from member_card where cardcode =(select idcard from b2b_crm where com_id=" + basic.Comid + " and weixin='" + requestXML.FromUserName + "')";//得到原微信账户对应的渠道编号
                                                        string originalphonecardid = "select id from member_card where cardcode =(select idcard from b2b_crm where com_id=" + basic.Comid + " and phone='" + phonecrm.Phone + "')";//得到原手机账户对应的卡编号
                                                        string sql4 = "update member_card  set issuecard =(" + OriginalWxChannelId + ")  where id=(" + originalphonecardid + ")";
                                                        cmd = sqlhelper.PrepareTextSqlCommand(sql4);
                                                        cmd.ExecuteNonQuery();

                                                        //修改原微信账户对应渠道
                                                        string companywxchannelid = "select id from member_channel where com_id=" + basic.Comid + " and issuetype=4";//公司对应微信渠道
                                                        string originalwxcardid = "select id from member_card where cardcode =(select idcard from b2b_crm where com_id=" + basic.Comid + " and weixin='" + weixincrm.Weixin + "')";//原始微信对应卡编号
                                                        string sql5 = "update member_card set issuecard=(" + companywxchannelid + ") where id=(" + originalwxcardid + ")";
                                                        cmd = sqlhelper.PrepareTextSqlCommand(sql5);
                                                        cmd.ExecuteNonQuery();

                                                        //修改手机账户(微信，预付款，积分 )
                                                        decimal Imprest = phonecrm.Imprest.ToString().ConvertTo<decimal>(0) + weixincrm.Imprest.ToString().ConvertTo<decimal>(0);
                                                        decimal Integral = phonecrm.Integral.ToString().ConvertTo<decimal>(0) + weixincrm.Integral.ToString().ConvertTo<decimal>(0);
                                                        string sql1 = "update b2b_crm set   weixin='" + weixincrm.Weixin + "',imprest='" + Imprest + "',Integral='" + Integral + "' where id=" + phonecrm.Id;
                                                        cmd = sqlhelper.PrepareTextSqlCommand(sql1);
                                                        cmd.ExecuteNonQuery();


                                                        //修改微信账户(微信，预付款，积分  )
                                                        string sql2 = "update b2b_crm set   weixin='',imprest='0',Integral='0'    where id=" + weixincrm.Id;
                                                        cmd = sqlhelper.PrepareTextSqlCommand(sql2);
                                                        cmd.ExecuteNonQuery();



                                                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[手机绑定成功]]></Content><FuncFlag>1</FuncFlag></xml>";

                                                        sqlhelper.Commit();
                                                    }
                                                    catch (Exception e)
                                                    {
                                                        err_msg = e.Message;
                                                        //resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + err_msg + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                        resxml = "";
                                                        sqlhelper.Rollback();
                                                    }
                                                    finally
                                                    {
                                                        //if (err_msg == "")
                                                        //{
                                                        //    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[手机绑定成功]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                        //}
                                                        //else
                                                        //{
                                                        //    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + err_msg + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                        //}
                                                        sqlhelper.Dispose();
                                                        if (resxml != "")
                                                        {
                                                            //修改手机账户等级分和会员级别
                                                            B2bcrm_dengjifenlog djflog = new B2bcrm_dengjifenlog
                                                            {
                                                                id = 0,
                                                                crmid = phonecrm.Id,
                                                                dengjifen = weixincrm.Dengjifen,
                                                                ptype = 1,
                                                                opertor = "系统",
                                                                opertime = DateTime.Now,
                                                                orderid = 0,
                                                                ordername = "微信绑定手机",
                                                                remark = "微信绑定手机"
                                                            };
                                                            int ajd_djf = new B2bCrmData().Adjust_dengjifen(djflog, phonecrm.Id, basic.Comid, weixincrm.Dengjifen);

                                                            //修改微信会员等积分和会员级别
                                                            string sql2 = "update b2b_crm set   weixin='',imprest='0',Integral='0',crmlevel='0',dengjifen='0'    where id=" + weixincrm.Id;
                                                            ExcelSqlHelper.ExecuteNonQuery(sql2);

                                                        }

                                                    }
                                                    #endregion
                                                }
                                                else //此手机绑定过微信
                                                {
                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + hadweixin + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                                }
                                            }
                                            else //会员表中 没有此电话号的会员：直接给含有此微信号的会员添加手机号
                                            {

                                                weixincrm.Phone = sendrecord.Phone.ToString();

                                                int u_id = new B2bCrmData().InsertOrUpdate(weixincrm);

                                                //把订单表中公司此微信号的订单uid赋值
                                                int orderchuli1 = new B2bOrderData().ModifyUidByWeiXin(requestXML.FromUserName, weixincrm.Id, basic.Comid);

                                                //判断手机号对应渠道 是否绑定了卡号：1.没有绑定，则直接向渠道表录入会员的卡号2.已经绑定，则不再处理
                                                //int isbindcardbyphone = new MemberChannelData().Isbindcardbyphone(weixincrm.Phone, basic.Comid);
                                                //if (isbindcardbyphone == 0)
                                                //{
                                                //    int inscrmcard = new MemberChannelData().InsCrmCardToChannel(weixincrm.Idcard, weixincrm.Phone, basic.Comid);
                                                //}

                                                //判断手机号对应渠道 是否绑定了卡号：1.没有绑定，则直接向渠道表录入会员的卡号2.已经绑定，则不再处理
                                                string err_msg = "";
                                                SqlHelper sqlhelper = new SqlHelper();
                                                sqlhelper.BeginTrancation();
                                                try
                                                {
                                                    SqlCommand cmd = new SqlCommand();

                                                    string sql6 = "select cardcode from member_channel where com_id=" + basic.Comid + " and mobile='" + weixincrm.Phone + "'";
                                                    cmd = sqlhelper.PrepareTextSqlCommand(sql6);
                                                    object o = cmd.ExecuteScalar();
                                                    if (o.ToString() == "0")
                                                    {
                                                        string sql7 = "update member_channel set cardcode='" + weixincrm.Idcard + "' where com_id=" + basic.Comid + " and mobile='" + weixincrm.Phone + "'";
                                                        cmd = sqlhelper.PrepareTextSqlCommand(sql7);
                                                        cmd.ExecuteNonQuery();
                                                    }

                                                    sqlhelper.Commit();
                                                }
                                                catch (Exception e)
                                                {
                                                    err_msg = e.Message;
                                                    sqlhelper.Rollback();
                                                }
                                                finally
                                                {
                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + verifycodecorrect + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                    sqlhelper.Dispose();
                                                }

                                            }
                                        }
                                        else //此微信已绑定手机号
                                        {
                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + hadphone + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                        }




                                    }
                                    else
                                    {
                                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + verifycodeerror + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                    }
                                }
                                else
                                {
                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + verifycodeexpired + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                }

                            }
                        }
                        #endregion
                        #region 输入邀请码
                        else if (System.Text.RegularExpressions.Regex.IsMatch(requestXML.Content, @"\d{8}") && requestXML.Content.Length == 8)//输入邀请码:1，绑定手机；2，合并手机和微信
                        {
                            if (Int32.Parse(requestXML.Content) >= 10000000 && Int32.Parse(requestXML.Content) <= 99999999)
                            {
                                //根据邀请码得到短信发码记录 
                                B2b_invitecodesendlog sendrecord = new B2b_invitecodesendlogData().GetNoteRecord(requestXML.Content, basic.Comid);
                                if (sendrecord != null)
                                {
                                    //if (sendrecord.Invitecode.ToString() == requestXML.Content)
                                    //{
                                    //绑定手机，微信：会员表中当前微信号变为空改写到含有绑定手机的记录中;同时预付款，服务费也需要变动
                                    //resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[验证码验证成功]]></Content><FuncFlag>1</FuncFlag></xml>";


                                    //1,此微信是否绑定过手机：a.绑定过给出提示;b.未绑定（2.判断会员表中是否有绑定此手机号的会员：a.不含有：直接录入手机号b.含有（3.判断手机是否绑定过微信:a.绑定过给出提示b.未绑定则合并微信手机账户））
                                    B2b_crm weixincrm = new B2bCrmData().GetB2bCrmByWeiXin(requestXML.FromUserName, basic.Comid);
                                    //如果此公司没有此微信的会员信息，则录入
                                    if (weixincrm == null)
                                    {
                                        resxml = InsB2bCrm(basic, requestXML, "");
                                        weixincrm = new B2bCrmData().GetB2bCrmByWeiXin(requestXML.FromUserName, basic.Comid);
                                    }
                                    if (weixincrm.Phone.ConvertTo<decimal>(0) == 0)
                                    {

                                        B2b_crm phonecrm = new B2bCrmData().GetB2bCrmByPhone(sendrecord.Phone.ToString(), basic.Comid);
                                        if (phonecrm != null)
                                        {
                                            if (phonecrm.Weixin.ConvertTo<string>("") == "")
                                            {
                                                //合并手机微信账户：a.会员表变动-原微信账户的手机(phone)，预付款(imprest),积分或者叫积分(integral)合并到手机号对应的会员信息中
                                                //-----------------b.订单表变动-原微信号的订单中uid变动：变为手机微信合并后的手机号对应的会员号uid
                                                //-----------------2014.04.14 c.原微信账户对应渠道修改为微信注册渠道，而合并后的手机账户对应渠道修改为原微信对应的渠道
                                                #region 事务执行，出错回滚
                                                SqlHelper sqlhelper = new SqlHelper();
                                                sqlhelper.BeginTrancation();
                                                try
                                                {
                                                    SqlCommand cmd = new SqlCommand();

                                                    //把订单表中公司此微信号的订单uid赋值
                                                    string sql3 = "update b2b_order set u_id='" + phonecrm.Id + "'  where openid='" + requestXML.FromUserName + "' and comid=" + basic.Comid;
                                                    cmd = sqlhelper.PrepareTextSqlCommand(sql3);
                                                    cmd.ExecuteNonQuery();
                                                    //修改原手机账户对应渠道
                                                    string OriginalWxChannelId = "select issuecard from member_card where cardcode =(select idcard from b2b_crm where com_id=" + basic.Comid + " and weixin='" + requestXML.FromUserName + "')";//得到原微信账户对应的渠道编号
                                                    string originalphonecardid = "select id from member_card where cardcode =(select idcard from b2b_crm where com_id=" + basic.Comid + " and phone='" + phonecrm.Phone + "')";//得到原手机账户对应的卡编号
                                                    string sql4 = "update member_card  set issuecard =(" + OriginalWxChannelId + ")  where id=(" + originalphonecardid + ")";
                                                    cmd = sqlhelper.PrepareTextSqlCommand(sql4);
                                                    cmd.ExecuteNonQuery();

                                                    //修改原微信账户对应渠道
                                                    string companywxchannelid = "select id from member_channel where com_id=" + basic.Comid + " and issuetype=4";//公司对应微信渠道
                                                    string originalwxcardid = "select id from member_card where cardcode =(select idcard from b2b_crm where com_id=" + basic.Comid + " and weixin='" + weixincrm.Weixin + "')";//原始微信对应卡编号
                                                    string sql5 = "update member_card set issuecard=(" + companywxchannelid + ") where id=(" + originalwxcardid + ")";
                                                    cmd = sqlhelper.PrepareTextSqlCommand(sql5);
                                                    cmd.ExecuteNonQuery();

                                                    //修改手机账户(微信，预付款，积分)
                                                    decimal Imprest = phonecrm.Imprest.ToString().ConvertTo<decimal>(0) + weixincrm.Imprest.ToString().ConvertTo<decimal>(0);
                                                    decimal Integral = phonecrm.Integral.ToString().ConvertTo<decimal>(0) + weixincrm.Integral.ToString().ConvertTo<decimal>(0);
                                                    string sql1 = "update b2b_crm set    weixin='" + weixincrm.Weixin + "',imprest='" + Imprest + "',Integral='" + Integral + "' where id=" + phonecrm.Id;
                                                    cmd = sqlhelper.PrepareTextSqlCommand(sql1);
                                                    cmd.ExecuteNonQuery();
                                                    //修改微信账户(微信，预付款，积分)
                                                    string sql2 = "update b2b_crm set   weixin='',imprest='0',Integral='0' where id=" + weixincrm.Id;
                                                    cmd = sqlhelper.PrepareTextSqlCommand(sql2);
                                                    cmd.ExecuteNonQuery();
                                                    sqlhelper.Commit();
                                                }
                                                catch (Exception e)
                                                {
                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + e.Message + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                                    sqlhelper.Rollback();
                                                }
                                                finally
                                                {
                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[手机绑定成功]]></Content><FuncFlag>1</FuncFlag></xml>";

                                                    sqlhelper.Dispose();
                                                }
                                                #endregion
                                            }
                                            else //此手机绑定过微信
                                            {
                                                resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + hadweixin + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                            }
                                        }
                                        else //会员表中 没有此电话号的会员：直接给含有此微信号的会员添加手机号
                                        {

                                            weixincrm.Phone = sendrecord.Phone.ToString();

                                            int u_id = new B2bCrmData().InsertOrUpdate(weixincrm);

                                            //把订单表中公司此微信号的订单uid赋值
                                            int orderchuli1 = new B2bOrderData().ModifyUidByWeiXin(requestXML.FromUserName, weixincrm.Id, basic.Comid);


                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + verifycodecorrect + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                        }
                                    }
                                    else //此微信已绑定手机号
                                    {
                                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + hadphone + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                    }




                                    //}
                                    //else
                                    //{
                                    //    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + verifycodeerror + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                    //}
                                }
                                else
                                {
                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + noinvitecoderecord + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                }

                            }
                        }
                        #endregion
                        #region 重发验证短信
                        else if (requestXML.Content.Trim() == "重发验证短信" || requestXML.Content.Trim().ToLower() == "cfyzdx")
                        {
                            int sendnum = 0;//向手机发送短信的次数（半小时内）
                            //根据微信号得到30分钟内短信发码记录，判断验证码是否匹配
                            Phone_codemodel sendrecord = Phone_code.GetNoteRecord(requestXML.FromUserName, basic.Comid);
                            if (sendrecord != null)
                            {
                                sendnum = sendrecord.Codenum + 1;

                                //此次发送短信时间与最后一次发送短信地时间之差
                                int numtime = DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 60 * 60 - sendrecord.Codetime.Second - sendrecord.Codetime.Minute * 60 - sendrecord.Codetime.Hour * 60 * 60;
                                if (numtime < 60)//重复点击
                                {
                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + sendmorefastnum + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                }
                                else
                                {
                                    if (sendrecord.Codenum > 2)//防止大量重发
                                    {
                                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + sendmorelimitenum + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                    }
                                    else
                                    {
                                        //修改状态为1，可以发送
                                        string data = Phone_code.upcode(sendrecord.Phone, sendrecord.Code, basic.Comid, sendnum, 1, requestXML.FromUserName);

                                        SendSmsHelper.GetMember_sms(sendrecord.Phone.ToString(), "", "", sendrecord.Code.ToString(), 0, "随机码", basic.Comid, sendnum);
                                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + sendcorrect + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                    }
                                }
                            }
                            else
                            {
                                resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + verifycodeexpired + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                            }
                        }
                        #endregion
                        #region 关键词查询
                        else
                        {
                            //先判断微信用户是否为渠道，userchannleid非0则是渠道
                            var crmdata = new B2bCrmData();
                            var channledata = new MemberChannelData();//读取渠道信息
                            int userchannleid = 0;
                            var userinfo = crmdata.b2b_crmH5(requestXML.FromUserName, basic.Comid);
                            if (userinfo != null)
                            {
                                //判断用户类型（会员，还是渠道）
                                if (userinfo.Phone != "")
                                {
                                    var userchannleinfo = channledata.GetPhoneComIdChannelDetail(userinfo.Phone, basic.Comid);
                                    if (userchannleinfo != null)
                                    {
                                        userchannleid = userchannleinfo.Id;//用户为渠道，获取渠道ID
                                    }
                                }
                            }

                            //锁定用户规则
                            Regex reg = new Regex(@"(R|r)(\d+)");


                            if (requestXML.Content == "我的顾问" || requestXML.Content == "我的旅游顾问" || requestXML.Content == "旅游顾问" || requestXML.Recognition == "我的服务顾问")//查询会员所在的公司或者门市信息
                            {
                                resxml = SelectMenShiByOpenId(requestXML, basic);

                            }
                            else if (requestXML.Content == "解绑" || requestXML.Content.ToLower() == "jb" || requestXML.Content.ToLower() == "jiebang" || requestXML.Content == "解锁" || requestXML.Content.ToLower() == "js" || requestXML.Content.ToLower() == "jiesuo" || requestXML.Content.ToLower() == "quit")
                            {

                                WeixinMessageUnLock(requestXML.FromUserName, basic.Comid, basic);
                            }
                            else if (requestXML.Content == "锁定" || requestXML.Content.ToLower() == "sd" || requestXML.Content.ToLower() == "suoding")
                            {
                                //锁定，只能手动锁定用户
                                WeixinMessageDeadLock(requestXML.FromUserName, 1, basic.Comid, basic);

                            }
                            else if (requestXML.Content == "解除锁定" || requestXML.Content.ToLower() == "jcsd" || requestXML.Content.ToLower() == "jiechusuoding")
                            {
                                //恢复为客户咨询自动锁定最后用户
                                WeixinMessageDeadLock(requestXML.FromUserName, 0, basic.Comid, basic);
                            }
                            else if (requestXML.Content == "解除顾问" || requestXML.Content.ToLower() == "jcgw" || requestXML.Content.ToLower() == "jiechuguwen")
                            {
                                //客户解除 与 顾问的 绑定
                                UseUnLockChannel(requestXML.FromUserName, basic.Comid, basic);
                            }
                            else if (requestXML.Content == "解绑手机" || requestXML.Content.ToLower() == "jbsj" || requestXML.Content.ToLower() == "jiebangshouji")
                            {
                                //客户解除 与 顾问的 绑定
                                UserUnLockPhne(requestXML.FromUserName, basic.Comid, basic);
                            }
                            else if (reg.IsMatch(requestXML.Content))
                            {

                                WeixinMessageLock(requestXML.FromUserName, requestXML.Content, basic.Comid, basic);
                            }
                            #region 提前标记下载上传的问候语音
                            else if (requestXML.Content == "上传问候语音")
                            {
                                if (isguwen == 0)
                                {
                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[只有顾问才可上传问候语音]]></Content><FuncFlag>1</FuncFlag></xml>";

                                }
                                else
                                {
                                    //删除当前顾问 的没有完成操作的标记 
                                    int r_delmark = new Wxmedia_updownlogData().DelGuwenNotSucMediaMark(requestXML.FromUserName);

                                    ////判断是否含有已经标记过"下载用户上传的问候语音" 但是没有完成操作 的记录
                                    //bool Ismarkedandnotdeal = new Wxmedia_updownlogData().Ismarkedandnotdeal(requestXML.FromUserName, (int)Clientuptypemark.DownGreetVoice);
                                    //if (Ismarkedandnotdeal == false)
                                    //{
                                    Wxmedia_updownlog udlog = new Wxmedia_updownlog
                                    {
                                        id = 0,
                                        mediaid = "",
                                        mediatype = "",
                                        savepath = "",
                                        created_at = "",
                                        createtime = DateTime.Now,
                                        opertype = "down",
                                        operweixin = requestXML.FromUserName,
                                        clientuptypemark = (int)Clientuptypemark.DownGreetVoice,
                                        comid = basic.Comid,
                                        relativepath = "",
                                        txtcontent = "",
                                        isfinish = 0
                                    };
                                    int udlogresult = new Wxmedia_updownlogData().Edituploadlog(udlog);
                                    if (udlogresult == 0)
                                    {
                                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[暂时不可上传语音]]></Content><FuncFlag>1</FuncFlag></xml>";
                                    }
                                    else
                                    {
                                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[请上传你的语音,语音会在用户绑定你为顾问时作为你的个人问候语展示.]]></Content><FuncFlag>1</FuncFlag></xml>";
                                    }
                                    //}
                                    //else
                                    //{
                                    //    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[请上传你的语音,语音会在用户绑定你为顾问时作为你的个人问候语展示.]]></Content><FuncFlag>1</FuncFlag></xml>";
                                    //}
                                }
                            }
                            #endregion
                            #region 提前标记下载上传的语音(注意“非问候语音”)
                            else if (requestXML.Content.ToLower() == "shangchuanyuyin")
                            {
                                if (isguwen == 0)
                                {
                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[只有顾问才可上传语音]]></Content><FuncFlag>1</FuncFlag></xml>";

                                }
                                else
                                {
                                    //删除当前顾问 的没有完成操作的标记 
                                    int r_delmark = new Wxmedia_updownlogData().DelGuwenNotSucMediaMark(requestXML.FromUserName);

                                    ////判断是否含有已经标记过"下载用户上传的语音" 但是没有完成操作 的记录
                                    //bool Ismarkedandnotdeal = new Wxmedia_updownlogData().Ismarkedandnotdeal(requestXML.FromUserName, (int)Clientuptypemark.DownVoice);
                                    //if (Ismarkedandnotdeal == false)
                                    //{
                                    Wxmedia_updownlog udlog = new Wxmedia_updownlog
                                    {
                                        id = 0,
                                        mediaid = "",
                                        mediatype = "",
                                        savepath = "",
                                        created_at = "",
                                        createtime = DateTime.Now,
                                        opertype = "down",
                                        operweixin = requestXML.FromUserName,
                                        clientuptypemark = (int)Clientuptypemark.DownVoice,
                                        comid = basic.Comid,
                                        relativepath = "",
                                        txtcontent = "",
                                        isfinish = 0
                                    };
                                    int udlogresult = new Wxmedia_updownlogData().Edituploadlog(udlog);
                                    if (udlogresult == 0)
                                    {
                                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[暂时不可上传语音]]></Content><FuncFlag>1</FuncFlag></xml>";
                                    }
                                    else
                                    {
                                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[请上传你的语音.]]></Content><FuncFlag>1</FuncFlag></xml>";
                                    }
                                    //}
                                    //else
                                    //{
                                    //    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[请上传你的语音.]]></Content><FuncFlag>1</FuncFlag></xml>";
                                    //}
                                }
                            }
                            #endregion
                            #region 提前标记上传文字留言
                            else if (requestXML.Content.ToLower() == "wenziliuyan")
                            {
                                if (isguwen == 0)
                                {
                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[只有顾问才可上传文字留言]]></Content><FuncFlag>1</FuncFlag></xml>";
                                }
                                else
                                {
                                    //删除当前顾问 的没有完成操作的标记 
                                    int r_delmark = new Wxmedia_updownlogData().DelGuwenNotSucMediaMark(requestXML.FromUserName);

                                    Wxmedia_updownlog udlog = new Wxmedia_updownlog
                                    {
                                        id = 0,
                                        mediaid = "",
                                        mediatype = "",
                                        savepath = "",
                                        created_at = "",
                                        createtime = DateTime.Now,
                                        opertype = "down",
                                        operweixin = requestXML.FromUserName,
                                        clientuptypemark = (int)Clientuptypemark.DownTxt,
                                        comid = basic.Comid,
                                        relativepath = "",
                                        txtcontent = "",
                                        isfinish = 0
                                    };
                                    int udlogresult = new Wxmedia_updownlogData().Edituploadlog(udlog);
                                    if (udlogresult == 0)
                                    {
                                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[暂时不可输入文字留言]]></Content><FuncFlag>1</FuncFlag></xml>";
                                    }
                                    else
                                    {
                                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[请输入你的文字留言.]]></Content><FuncFlag>1</FuncFlag></xml>";
                                    }

                                }
                            }
                            #endregion
                            #region 提前标记上传图片
                            else if (requestXML.Content.ToLower() == "shangchuantupian")
                            {
                                if (isguwen == 0)
                                {
                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[只有顾问才可上传图片]]></Content><FuncFlag>1</FuncFlag></xml>";
                                }
                                else
                                {
                                    //删除当前顾问 的没有完成操作的标记 
                                    int r_delmark = new Wxmedia_updownlogData().DelGuwenNotSucMediaMark(requestXML.FromUserName);

                                    Wxmedia_updownlog udlog = new Wxmedia_updownlog
                                    {
                                        id = 0,
                                        mediaid = "",
                                        mediatype = "",
                                        savepath = "",
                                        created_at = "",
                                        createtime = DateTime.Now,
                                        opertype = "down",
                                        operweixin = requestXML.FromUserName,
                                        clientuptypemark = (int)Clientuptypemark.DownImg,
                                        comid = basic.Comid,
                                        relativepath = "",
                                        txtcontent = "",
                                        isfinish = 0
                                    };
                                    int udlogresult = new Wxmedia_updownlogData().Edituploadlog(udlog);
                                    if (udlogresult == 0)
                                    {
                                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[暂时不可上传顾问图片]]></Content><FuncFlag>1</FuncFlag></xml>";
                                    }
                                    else
                                    {
                                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[你已进入上传顾问图片状态，请上传顾问图片.]]></Content><FuncFlag>1</FuncFlag></xml>";
                                    }

                                }
                            }
                            #endregion
                            #region 标记退出上传图片
                            else if (requestXML.Content.ToLower() == "tcsctp")
                            {
                                //删除当前顾问 的没有完成操作的标记 
                                int r_delmark = new Wxmedia_updownlogData().DelGuwenNotSucMediaMark(requestXML.FromUserName);
                                resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[退出上传图片状态]]></Content><FuncFlag>1</FuncFlag></xml>";

                            }
                            #endregion
                            #region 把下载到服务器的语音上传到 微信服务器，并且返回给用户
                            else if (requestXML.Content == "测试返回问候语音")
                            {
                                WXAccessToken m_accesstoken = GetAccessToken(basic.Comid, basic.AppId, basic.AppSecret);
                                //根据用户微信得到其顾问微信，然后根据微信和标记得到最新的一条保存路径(注：已经上传过语音的即mediaid!="")
                                Wxmedia_updownlog udlog = new Wxmedia_updownlogData().GetWxmedia_updownlog(requestXML.FromUserName, (int)Clientuptypemark.DownGreetVoice, basic.Comid);
                                if (udlog == null)
                                {
                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[返回问候语音出错]]></Content><FuncFlag>1</FuncFlag></xml>";
                                }
                                else
                                {
                                    string media_id = new WxUploadDownManage().UploadMultimedia(m_accesstoken.ACCESS_TOKEN, "voice", udlog.savepath);
                                    if (media_id != "")
                                    {
                                        Wxmedia_updownlog uplog = new Wxmedia_updownlog
                                        {
                                            id = 0,
                                            mediaid = media_id,
                                            mediatype = "voice",
                                            savepath = udlog.savepath,
                                            created_at = ConvertDateTimeInt(DateTime.Now).ToString(),
                                            createtime = DateTime.Now,
                                            opertype = "up",
                                            operweixin = requestXML.FromUserName,
                                            clientuptypemark = (int)Clientuptypemark.UpMedia,//上传多媒体信息
                                            comid = basic.Comid
                                        };
                                        int uplogresult = new Wxmedia_updownlogData().Edituploadlog(uplog);
                                        if (uplogresult == 0)
                                        {
                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[记入多媒体日志出错]]></Content><FuncFlag>1</FuncFlag></xml>";
                                        }
                                        else
                                        {
                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[voice]]></MsgType><Voice><MediaId><![CDATA[" + media_id + "]]></MediaId></Voice></xml>";
                                        }
                                    }
                                    else
                                    {
                                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[返回问候语音出错.]]></Content><FuncFlag>1</FuncFlag></xml>";
                                    }
                                }

                            }
                            #endregion
                            else
                            {
                                IList<WxMaterial> materiallist = new WxMaterialData().GetWxMaterialByKeyword(requestXML.Content, basic.Comid);
                                if (materiallist != null)
                                {
                                    if (materiallist.Count > 0)
                                    {
                                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>" + materiallist.Count + "</ArticleCount><Articles>";
                                        foreach (WxMaterial wxmaterial in materiallist)
                                        {
                                            resxml += "<item><Title><![CDATA[" + wxmaterial.Title + "]]></Title><Description><![CDATA[" + wxmaterial.Summary + "]]></Description><PicUrl><![CDATA[" + FileSerivce.GetImgUrl(wxmaterial.Imgpath.ConvertTo<int>(0)) + "]]></PicUrl><Url><![CDATA[" + httphead + basic.Domain + aboutmaterialdetailurl + wxmaterial.MaterialId + "]]></Url></item>";

                                        }
                                        resxml += "</Articles></xml>";
                                    }
                                    else
                                    {   //只有客户收到此消息，所有顾问都是回复客户消息
                                        if (userchannleid == 0)
                                        {
                                            resxml = SendAutoReply(requestXML.FromUserName, requestXML.ToUserName, defaultret);
                                        }

                                    }
                                }
                                else
                                {   //只有客户收到此消息，所有顾问都是回复客户消息
                                    if (userchannleid == 0)
                                    {
                                        resxml = SendAutoReply(requestXML.FromUserName, requestXML.ToUserName, defaultret);
                                    }
                                    //resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + defaultret + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                }


                                //对渠道发送消息模板
                                var retmessage = WeixinMessageChannel(channelid, basic.Comid, requestXML.FromUserName, requestXML.Content, 1, "", basic);

                            }
                        }
                        #endregion
                    #endregion
                    }
                    #endregion
                }
                #endregion
                #region 接收普通消息:语音信息
                if (requestXML.MsgType == "voice")
                {
                    if (requestXML.Recognition == "我的顾问" || requestXML.Recognition == "我的旅游顾问" || requestXML.Recognition == "旅游顾问" || requestXML.Recognition == "我的服务顾问")//查询会员所在的公司或者门市信息
                    {
                        resxml = SelectMenShiByOpenId(requestXML, basic);

                    }
                    else if (requestXML.Recognition == "解锁" || requestXML.Recognition.ToLower() == "js" || requestXML.Recognition.ToLower() == "jiesuo" || requestXML.Recognition.ToLower() == "quit")
                    {

                        WeixinMessageUnLock(requestXML.FromUserName, basic.Comid, basic);
                    }
                    else if (requestXML.Recognition == "锁定" || requestXML.Recognition.ToLower() == "sd" || requestXML.Recognition.ToLower() == "suoding")
                    {
                        //锁定，只能手动锁定用户
                        WeixinMessageDeadLock(requestXML.FromUserName, 1, basic.Comid, basic);

                    }
                    else if (requestXML.Recognition == "解除锁定" || requestXML.Recognition.ToLower() == "jcsd" || requestXML.Recognition.ToLower() == "jiechusuoding")
                    {
                        //恢复为客户咨询自动锁定最后用户
                        WeixinMessageDeadLock(requestXML.FromUserName, 0, basic.Comid, basic);
                    }
                    else if (requestXML.Recognition == "解除顾问" || requestXML.Recognition.ToLower() == "jcgw" || requestXML.Recognition.ToLower() == "jiechuguwen")
                    {
                        //客户解除 与 顾问的 绑定
                        UseUnLockChannel(requestXML.FromUserName, basic.Comid, basic);
                    }
                    else
                    {
                        //对渠道发送消息模板
                        var retmessage = WeixinMessageChannel(channelid, basic.Comid, requestXML.FromUserName, requestXML.Recognition, 2, requestXML.MediaId, basic);
                    }

                    #region 判断是否含有已经标记过"下载上传的问候语音" 但是没有完成操作 的记录
                    bool Ismarkedandnotdeal = new Wxmedia_updownlogData().Ismarkedandnotdeal(requestXML.FromUserName, (int)Clientuptypemark.DownGreetVoice);
                    if (Ismarkedandnotdeal)
                    {
                        //获取凭证（首先判断数据库是否存在凭证以及是否过期，如过期，重新获取）
                        WXAccessToken token = GetAccessToken(basic.Comid, basic.AppId, basic.AppSecret);

                        string Filepath = System.Web.HttpContext.Current.Server.MapPath("/WxClientVoiceUploadFile/");
                        if (!Directory.Exists(Filepath))//判断路径是否存在
                        {
                            Directory.CreateDirectory(Filepath);//如果不存在创建文件夹           
                        }
                        string medianame = DateTime.Now.ToString("yyyyMMddHHmmssfff") + (new Random()).Next().ToString().Substring(0, 4) + ".amr";
                        string relativepath = "/WxClientVoiceUploadFile/" + medianame;
                        string savepath = System.Web.HttpContext.Current.Server.MapPath("/WxClientVoiceUploadFile/") + medianame;

                        // 下载用户上传的语音到本地
                        string udresult = new WxUploadDownManage().GetMultimedia(token.ACCESS_TOKEN, requestXML.MediaId, savepath);
                        if (udresult == "1")//下载用户上传的语音 到本地成功
                        {
                            //得到含有已经标记过"上传问候语音" 但是没有完成上传 的记录
                            Wxmedia_updownlog udlog = new Wxmedia_updownlogData().GetMarkedAndNotdeallog(requestXML.FromUserName, (int)Clientuptypemark.DownGreetVoice);
                            if (udlog == null)
                            {
                                resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[上传语音失败.]]></Content><FuncFlag>1</FuncFlag></xml>";
                            }
                            else
                            {
                                udlog.mediaid = requestXML.MediaId;
                                udlog.mediatype = "voice";
                                udlog.savepath = savepath;
                                udlog.created_at = ConvertDateTimeInt(DateTime.Now).ToString();
                                udlog.createtime = DateTime.Now;
                                udlog.opertype = "down";//下载用户上传的语音
                                udlog.isfinish = 1;
                                udlog.relativepath = relativepath;


                                int udlogresult = new Wxmedia_updownlogData().Edituploadlog(udlog);
                                if (udlogresult > 0)
                                {
                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[上传语音成功,语音会在用户绑定你为顾问时作为你的个人问候语展示.]]></Content><FuncFlag>1</FuncFlag></xml>";
                                }
                                else
                                {
                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[上传语音失败..]]></Content><FuncFlag>1</FuncFlag></xml>";
                                }
                            }
                        }
                        else //下载用户上传的语音 到本地失败
                        {
                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[.上传语音失败]]></Content><FuncFlag>1</FuncFlag></xml>";
                        }
                    }
                    #endregion
                    #region 判断是否含有已经标记过"下载上传的语音" 但是没有完成操作 的记录
                    bool Ismarkedandnotdeal2 = new Wxmedia_updownlogData().Ismarkedandnotdeal(requestXML.FromUserName, (int)Clientuptypemark.DownVoice);
                    if (Ismarkedandnotdeal2)
                    {
                        //获取凭证（首先判断数据库是否存在凭证以及是否过期，如过期，重新获取）
                        WXAccessToken token = GetAccessToken(basic.Comid, basic.AppId, basic.AppSecret);


                        string Filepath = System.Web.HttpContext.Current.Server.MapPath("/WxClientVoiceUploadFile/");
                        if (!Directory.Exists(Filepath))//判断路径是否存在
                        {
                            Directory.CreateDirectory(Filepath);//如果不存在创建文件夹           
                        }
                        string medianame = DateTime.Now.ToString("yyyyMMddHHmmssfff") + (new Random()).Next().ToString().Substring(0, 4) + ".amr";
                        string relativepath = "/WxClientVoiceUploadFile/" + medianame;
                        string savepath = System.Web.HttpContext.Current.Server.MapPath("/WxClientVoiceUploadFile/") + medianame;

                        // 下载用户上传的语音到本地
                        string udresult = new WxUploadDownManage().GetMultimedia(token.ACCESS_TOKEN, requestXML.MediaId, savepath);
                        if (udresult == "1")//下载用户上传的语音 到本地成功
                        {
                            //得到含有已经标记过"上传语音" 但是没有完成上传 的记录
                            Wxmedia_updownlog udlog = new Wxmedia_updownlogData().GetMarkedAndNotdeallog(requestXML.FromUserName, (int)Clientuptypemark.DownVoice);
                            if (udlog == null)
                            {
                                resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[上传语音失败.]]></Content><FuncFlag>1</FuncFlag></xml>";
                            }
                            else
                            {
                                udlog.mediaid = requestXML.MediaId;
                                udlog.mediatype = "voice";
                                udlog.savepath = savepath;
                                udlog.created_at = ConvertDateTimeInt(DateTime.Now).ToString();
                                udlog.createtime = DateTime.Now;
                                udlog.opertype = "down";//下载用户上传的语音
                                udlog.isfinish = 1;
                                udlog.relativepath = relativepath;

                                int udlogresult = new Wxmedia_updownlogData().Edituploadlog(udlog);
                                if (udlogresult > 0)
                                {
                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[上传语音成功.]]></Content><FuncFlag>1</FuncFlag></xml>";
                                }
                                else
                                {
                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[上传语音失败..]]></Content><FuncFlag>1</FuncFlag></xml>";
                                }
                            }
                        }
                        else //下载用户上传的语音 到本地失败
                        {
                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[.上传语音失败]]></Content><FuncFlag>1</FuncFlag></xml>";
                        }
                    }
                    #endregion
                }
                #endregion
                #region 接收普通消息:地址坐标
                else if (requestXML.MsgType == "location")
                {
                    //    string city = GetMapInfo(requestXML.Location_X, requestXML.Location_Y);

                    //    if (city == "0")
                    //    {

                    //        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[好啦，我们知道你的位置了，你可以?:" + GetDefault() + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                    //    }

                    //    else
                    //    {

                    //        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[好啦，我们知道你的位置了，你可以?:" + GetDefault() + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                    //    }

                }
                #endregion
                #region 接收普通消息:图片信息
                else if (requestXML.MsgType == "image")
                {
                    #region //注释内容
                    //resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[亲，我没看懂你的意思，你可以:" + GetDefault() + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                    ////返回10条以内

                    //int size = 10;

                    //resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><Content><![CDATA[]]></Content><ArticleCount>" + size + "</ArticleCount><Articles>";

                    //List<string> list = new List<string>();

                    ////假如有20条查询返回的结果

                    //for (int i = 0; i < 20; i++)
                    //{

                    //    list.Add("1");

                    //}

                    //string[] piclist = new string[] { "/Abstract_Pencil_Scribble_Background_Vector_main.jpg", "/balloon_tree.jpg", "/bloom.jpg", "/colorful_flowers.jpg", "/colorful_summer_flower.jpg", "/fall.jpg", "/fall_tree.jpg", "/growing_flowers.jpg", "/shoes_illustration.jpg", "/splashed_tree.jpg" };



                    //for (int i = 0; i < size && i < list.Count; i++)
                    //{

                    //    resxml += "<item><Title><![CDATA[沈阳,黑龙江]]></Title><Description><![CDATA[元旦特价：300，市场价：400]]></Description><PicUrl><![CDATA[" + "http://www.hougelou.com" + piclist[i] + "]]></PicUrl><Url><![CDATA[http://www.hougelou.com]]></Url></item>";

                    //}

                    //resxml += "</Articles><FuncFlag>1</FuncFlag></xml>";
                    #endregion

                    bool Ismarkedandnotdeal = false;
                    if (isguwen == 1)
                    {
                        //是否是顾问上传图片
                        Ismarkedandnotdeal = new Wxmedia_updownlogData().Ismarkedandnotdeal(requestXML.FromUserName, (int)Clientuptypemark.DownImg);
                        if (Ismarkedandnotdeal)
                        {
                            lock (lockobj)
                            {
                                //获取凭证（首先判断数据库是否存在凭证以及是否过期，如过期，重新获取）
                                WXAccessToken token = GetAccessToken(basic.Comid, basic.AppId, basic.AppSecret);

                                string Filepath = System.Web.HttpContext.Current.Server.MapPath("/WxClientImgUploadFile/");
                                if (!Directory.Exists(Filepath))//判断路径是否存在
                                {
                                    Directory.CreateDirectory(Filepath);//如果不存在创建文件夹           
                                }

                                string imgname = DateTime.Now.ToString("yyyyMMddHHmmssfff") + (new Random()).Next().ToString().Substring(0, 4) + ".jpg";
                                string savepath = System.Web.HttpContext.Current.Server.MapPath("/WxClientImgUploadFile/") + imgname;
                                string relativepath = "/WxClientImgUploadFile/" + imgname;
                                // 下载用户上传的图片到本地
                                string udresult = new WxUploadDownManage().GetMultimedia(token.ACCESS_TOKEN, requestXML.MediaId, savepath);
                                if (udresult == "1")//下载用户上传的图片 到本地成功
                                {
                                    //得到含有已经标记过"上传图片" 但是没有完成上传 的记录
                                    Wxmedia_updownlog udlog = new Wxmedia_updownlogData().GetMarkedAndNotdeallog(requestXML.FromUserName, (int)Clientuptypemark.DownImg);
                                    if (udlog == null)
                                    {
                                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[上传图片失败.]]></Content><FuncFlag>1</FuncFlag></xml>";
                                    }
                                    else
                                    {
                                        udlog.mediaid = requestXML.MediaId;
                                        udlog.mediatype = "image";
                                        udlog.savepath = savepath;
                                        udlog.created_at = ConvertDateTimeInt(DateTime.Now).ToString();
                                        udlog.createtime = DateTime.Now;
                                        udlog.opertype = "down";//下载用户上传的图片
                                        udlog.isfinish = 1;
                                        udlog.relativepath = relativepath;


                                        int udlogresult = new Wxmedia_updownlogData().Edituploadlog(udlog);
                                        if (udlogresult > 0)
                                        {
                                            //删除当前顾问 的没有完成操作的标记 
                                            int r_delmark = new Wxmedia_updownlogData().DelGuwenNotSucMediaMark(requestXML.FromUserName);

                                            Wxmedia_updownlog udlog2 = new Wxmedia_updownlog
                                            {
                                                id = 0,
                                                mediaid = "",
                                                mediatype = "",
                                                savepath = "",
                                                created_at = "",
                                                createtime = DateTime.Now,
                                                opertype = "down",
                                                operweixin = requestXML.FromUserName,
                                                clientuptypemark = (int)Clientuptypemark.DownImg,
                                                comid = basic.Comid,
                                                relativepath = "",
                                                txtcontent = "",
                                                isfinish = 0
                                            };
                                            int udlogresult2 = new Wxmedia_updownlogData().Edituploadlog(udlog2);

                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[上传图片成功，如退出上传顾问图片状态，请输入'退出上传图片'首字母]]></Content><FuncFlag>1</FuncFlag></xml>";
                                        }
                                        else
                                        {
                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[上传图片失败..]]></Content><FuncFlag>1</FuncFlag></xml>";
                                        }
                                    }
                                }
                                else //下载用户上传的图片 到本地失败
                                {
                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[.上传图片失败]]></Content><FuncFlag>1</FuncFlag></xml>";
                                }
                            }
                        }
                    }

                    if (Ismarkedandnotdeal == false)
                    {
                        //对渠道发送消息模板
                        var retmessage = WeixinMessageChannel(channelid, basic.Comid, requestXML.FromUserName, requestXML.PicUrl, 3, requestXML.MediaId, basic);
                    }
                }
                #endregion
                #region 接收事件推送
                else if (requestXML.MsgType == "event")
                {
                    #region 关注事件
                    if (requestXML.Eevent == "subscribe")
                    {
                        lock (lockobj)
                        {
                            //判断是否是并发请求
                            string cretime = requestXML.CreateTime;
                            //获得请求发送的次数
                            int reqnum = new WxSubscribeDetailData().GetReqnum(requestXML.FromUserName, cretime, "subscribe");
                            if (reqnum == 0)
                            {
                                #region 记录用户关注事件
                                requestXML.Content = "用户关注事件";
                                requestXML.ContentType = true;
                                requestXML.Comid = basic.Comid;
                                int inswxexchangemsg = new WxRequestXmlData().EditWxRequestXmlLog(requestXML);
                                #endregion
                                #region 未关注微信用户 扫描带参数二维码关注事件
                                if (requestXML.EventKey != "")
                                {
                                    int subscribesourceid = requestXML.EventKey.Substring(requestXML.EventKey.IndexOf("_") + 1).ConvertTo<int>(0);

                                    //添加扫码记录
                                    WxSubscribeDetail subscribemodel = new WxSubscribeDetail();
                                    subscribemodel.Id = 0;
                                    subscribemodel.Openid = requestXML.FromUserName;
                                    subscribemodel.Subscribetime = DateTime.Now;
                                    subscribemodel.Subscribesourceid = subscribesourceid;
                                    subscribemodel.Eevent = requestXML.Eevent;
                                    subscribemodel.Eventkey = requestXML.EventKey;
                                    subscribemodel.Comid = basic.Comid;
                                    subscribemodel.Createtime = requestXML.CreateTime;

                                    int editsubscribedetail = new WxSubscribeDetailData().EditSubscribeDetail(subscribemodel);
                                    //新用户关注
                                    resxml = SubscribeWeiXin(requestXML, basic, subscribesourceid);
                                    #region 下面代码重复累赘，注释掉 换为上面的代码
                                    ////判断会员表中是否含有此会员
                                    //B2b_crm crmmm = new B2bCrmData().GetB2bCrm(requestXML.FromUserName, basic.Comid);
                                    //#region 取消关注后二次扫描带参二维码重新关注
                                    //if (crmmm != null)
                                    //{
                                    //    int subscribesourceid = requestXML.EventKey.Substring(requestXML.EventKey.IndexOf("_") + 1).ConvertTo<int>(0);

                                    //    //添加扫码记录
                                    //    WxSubscribeDetail subscribemodel = new WxSubscribeDetail();
                                    //    subscribemodel.Id = 0;
                                    //    subscribemodel.Openid = requestXML.FromUserName;
                                    //    subscribemodel.Subscribetime = DateTime.Now;
                                    //    subscribemodel.Subscribesourceid = subscribesourceid;
                                    //    subscribemodel.Eevent = requestXML.Eevent;
                                    //    subscribemodel.Eventkey = requestXML.EventKey;
                                    //    subscribemodel.Comid = basic.Comid;
                                    //    subscribemodel.Createtime = requestXML.CreateTime;

                                    //    int editsubscribedetail = new WxSubscribeDetailData().EditSubscribeDetail(subscribemodel);
                                    //    //新用户关注
                                    //    resxml = SubscribeWeiXin(requestXML, basic, subscribesourceid);
                                    //}
                                    //#endregion
                                    //#region  扫描带参二维码 新关注用户，记录
                                    //else
                                    //{
                                    //    int subscribesourceid = requestXML.EventKey.Substring(requestXML.EventKey.IndexOf("_") + 1).ConvertTo<int>(0);

                                    //    //添加扫码记录
                                    //    WxSubscribeDetail subscribemodel = new WxSubscribeDetail();
                                    //    subscribemodel.Id = 0;
                                    //    subscribemodel.Openid = requestXML.FromUserName;
                                    //    subscribemodel.Subscribetime = DateTime.Now;
                                    //    subscribemodel.Subscribesourceid = subscribesourceid;
                                    //    subscribemodel.Eevent = requestXML.Eevent;
                                    //    subscribemodel.Eventkey = requestXML.EventKey;
                                    //    subscribemodel.Comid = basic.Comid;
                                    //    subscribemodel.Createtime = requestXML.CreateTime;

                                    //    int editsubscribedetail = new WxSubscribeDetailData().EditSubscribeDetail(subscribemodel);
                                    //    //新用户关注
                                    //    resxml = SubscribeWeiXin(requestXML, basic, subscribesourceid);
                                    //}

                                    //#region 判断扫描参数如果是 渠道，或门市二维码则执行给客户发送 顾问信息
                                    //int sourceid = requestXML.EventKey.Substring(requestXML.EventKey.IndexOf("_") + 1).ConvertTo<int>(0);
                                    //WxSubscribeSource msource = new WxSubscribeSourceData().GetWXSourceById(sourceid);
                                    //if (msource != null)
                                    //{
                                    //    if (msource.Channelcompanyid != 0 || msource.Channelid != 0)
                                    //    {
                                    //        //自动重新分配渠道，或者读取现有渠道并发送客户通道
                                    //        channelid = CustomerMsg_Send.AutoFenpeiChannel(basic.Comid, requestXML.FromUserName, 1, msource.Channelcompanyid, msource.Channelid);
                                    //    }
                                    //}
                                    //#endregion

                                    //#endregion
                                    #endregion
                                }
                                #endregion
                                #region 未关注微信用户 搜索公众号(非扫描带参数二维码)关注事件 或者扫描无参二维码关注事件
                                else
                                {
                                    WxSubscribeDetail subscribemodel = new WxSubscribeDetail();
                                    subscribemodel.Id = 0;
                                    subscribemodel.Openid = requestXML.FromUserName;
                                    subscribemodel.Subscribetime = DateTime.Now;
                                    subscribemodel.Eevent = requestXML.Eevent;
                                    subscribemodel.Comid = basic.Comid;
                                    subscribemodel.Createtime = requestXML.CreateTime;

                                    //添加扫码记录
                                    int editsubscribedetail = new WxSubscribeDetailData().EditSubscribeDetail(subscribemodel);
                                    //新用户关注
                                    resxml = SubscribeWeiXin(requestXML, basic);
                                }
                                #endregion

                                #region 用户关注则录入位置坐标0，0
                                SqlHelper sqlhelper = new SqlHelper();
                                sqlhelper.BeginTrancation();
                                try
                                {
                                    SqlCommand cmd = new SqlCommand();

                                    //会员位置表：判断是否保存过位置信息，a.保存过修改b.没有保存过添加
                                    string sql1 = "select count(1) from b2b_crm_location  where weixin='" + requestXML.FromUserName + "' and comid=" + basic.Comid;
                                    cmd = sqlhelper.PrepareTextSqlCommand(sql1);
                                    object o = cmd.ExecuteScalar();
                                    if (int.Parse(o.ToString()) > 0)
                                    {
                                        string sql2 = "update b2b_crm_location set Latitude='0' ,Longitude='0' ,Precision='0',createtime='" + requestXML.CreateTime + "',createtimeformat='" + requestXML.CreateTimeFormat + "'  where weixin='" + requestXML.FromUserName + "' and comid=" + basic.Comid;
                                        cmd = sqlhelper.PrepareTextSqlCommand(sql2);
                                        cmd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        if (basic.Comid > 0)
                                        {
                                            string sql3 = "insert into b2b_crm_location (weixin,createtime,Latitude,Longitude,Precision,comid,createtimeformat) values('" + requestXML.FromUserName + "','" + requestXML.CreateTime + "','0','0','0','" + basic.Comid + "','" + requestXML.CreateTimeFormat + "')";
                                            cmd = sqlhelper.PrepareTextSqlCommand(sql3);
                                            cmd.ExecuteNonQuery();
                                        }
                                    }

                                    sqlhelper.Commit();
                                }
                                catch (Exception e)
                                {
                                    sqlhelper.Rollback();
                                }
                                finally
                                {
                                    sqlhelper.Dispose();
                                }
                                #endregion


                                #region 用户扫描带参二维码，被动回复消息
                                int wxsourceid = requestXML.EventKey.Substring(requestXML.EventKey.IndexOf("_") + 1).ConvertTo<int>(0);
                                if (wxsourceid > 0)
                                {
                                    WxSubscribeSource sourcer = new WxSubscribeSourceData().GetWXSourceById(wxsourceid);
                                    if (sourcer != null)
                                    {
                                        //设定并获取微信随机码                              
                                        var pass = new B2bCrmData().WeixinGetPass(requestXML.FromUserName, basic.Comid);

                                        //根据微信号获得会员信息
                                        B2b_crm crminfo = new B2bCrmData().GetB2bCrmByWeiXin(requestXML.FromUserName, basic.Comid);
                                        if (crminfo == null)//会员信息为空时，添加此会员信息
                                        {
                                            resxml = InsB2bCrm(basic, requestXML, "");
                                        }

                                        #region  绑定有渠道的二维码
                                        if (sourcer.qrcodeviewtype > 0)//绑定有渠道的二维码
                                        {
                                            #region 1营销活动
                                            if (sourcer.qrcodeviewtype == 1)
                                            {
                                                Member_Activity m = MemberActivityData.GetActById(sourcer.Activityid);
                                                if (m != null)
                                                {
                                                    string content = m.Title + "\n" + m.Actstar.ToString("yyyy-MM-dd") + "-" + m.Actend.ToString("yyyy-MM-dd");

                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                }
                                            }
                                            #endregion
                                            #region 2产品图文
                                            else if (sourcer.qrcodeviewtype == 2)
                                            {
                                                #region 显示项目下产品
                                                if (sourcer.Productid == 0)//显示项目下产品
                                                {
                                                    int projectid = sourcer.projectid;
                                                    if (projectid > 0)
                                                    {
                                                        string servertypes = "1,2,8,9,10,11,12";//显示的服务类型:电子票；跟团游；当地游;酒店；实物
                                                        int topnums = 10;//显示数量
                                                        IList<B2b_com_pro> prolist = new B2bComProData().GetWxProlistbyprojectid(projectid, servertypes, topnums);
                                                        if (prolist.Count > 0)
                                                        {
                                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>" + prolist.Count + "</ArticleCount><Articles>";
                                                            foreach (B2b_com_pro pro in prolist)
                                                            {
                                                                string proname = pro.Pro_name;
                                                                string proimgurl = FileSerivce.GetImgUrl(pro.Imgurl);
                                                                string linkurl = "";

                                                                if (pro.Server_type == 1 || pro.Server_type == 12)//票务 或者 预约产品
                                                                {
                                                                    linkurl = httphead + basic.Domain + "/h5/order/pro.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                                }
                                                                else if (pro.Server_type == 2 || pro.Server_type == 8)//旅游
                                                                {
                                                                    linkurl = httphead + basic.Domain + "/h5/linedetail.aspx?lineid=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                                }
                                                                else if (pro.Server_type == 9)//酒店客房
                                                                {
                                                                    linkurl = httphead + basic.Domain + "/h5/hotel/hotelshow.aspx?proid=" + pro.Id + "&id=" + basic.Comid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                                }
                                                                else if (pro.Server_type == 11)//实物
                                                                {
                                                                    linkurl = httphead + basic.Domain + "/h5/order/pro.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                                }
                                                                else if (pro.Server_type == 10)//旅游大巴
                                                                {
                                                                    linkurl = httphead + basic.Domain + "/h5/OrderEnter.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                                }
                                                                else
                                                                {
                                                                    linkurl = "";
                                                                }
                                                                string adviseprice = "￥" + pro.Advise_price.ToString("F2");
                                                                resxml += "<item><Title><![CDATA[" + pro.Pro_name + "]]></Title><Description><![CDATA[" + adviseprice + "]]></Description><PicUrl><![CDATA[" + proimgurl + "]]></PicUrl><Url><![CDATA[" + linkurl + "]]></Url></item>";
                                                            }
                                                            resxml += "</Articles></xml>";
                                                        }
                                                    }
                                                }
                                                #endregion
                                                #region 显示特定产品
                                                else //显示特定产品
                                                {
                                                    //根据产品id得到产品信息
                                                    B2b_com_pro pro = new B2bComProData().GetProById(sourcer.Productid.ToString());
                                                    if (pro != null)
                                                    {
                                                        if (pro.Server_type != 3)
                                                        {
                                                            string proname = pro.Pro_name;
                                                            string proimgurl = FileSerivce.GetImgUrl(pro.Imgurl);
                                                            string linkurl = "";
                                                            if (pro.Server_type == 1 || pro.Server_type == 12)//票务 或者 预约产品
                                                            {
                                                                linkurl = httphead + basic.Domain + "/h5/order/pro.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            }
                                                            else if (pro.Server_type == 2 || pro.Server_type == 8)//旅游
                                                            {
                                                                linkurl = httphead + basic.Domain + "/h5/linedetail.aspx?lineid=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            }
                                                            else if (pro.Server_type == 9)//酒店客房
                                                            {
                                                                linkurl = httphead + basic.Domain + "/h5/hotel/hotelshow.aspx?proid=" + pro.Id + "&id=" + basic.Comid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            }
                                                            else if (pro.Server_type == 11)//实物
                                                            {
                                                                linkurl = httphead + basic.Domain + "/h5/order/pro.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            }
                                                            else if (pro.Server_type == 14)//保险产品
                                                            {
                                                                linkurl = httphead + basic.Domain + "/h5/OrderEnter.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            }
                                                            else if (pro.Server_type == 10)//旅游大巴
                                                            {
                                                                linkurl = httphead + basic.Domain + "/h5/OrderEnter.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            }
                                                            else
                                                            {
                                                                linkurl = "";
                                                            }

                                                            string adviseprice = "￥" + pro.Advise_price.ToString("F2");
                                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount>" +
                                                            "<Articles><item><Title><![CDATA[" + proname + "]]></Title><Description><![CDATA[" + adviseprice + "]]></Description><PicUrl><![CDATA[" + proimgurl + "]]></PicUrl><Url><![CDATA[" + linkurl + "]]></Url></item></Articles></xml>";
                                                        }
                                                        else
                                                        {
                                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                            #endregion
                                            #region 3分销商，暂时 没东西
                                            else if (sourcer.qrcodeviewtype == 3)
                                            {

                                            }
                                            #endregion
                                            #region  4文章图文
                                            else if (sourcer.qrcodeviewtype == 4)
                                            {
                                                #region 文章类型下文章列表
                                                if (sourcer.Wxmaterialid == 0)
                                                {
                                                    if (sourcer.wxmaterialtypeid > 0)
                                                    {
                                                        int topnums = 10;//显示数量
                                                        IList<WxMaterial> wxMateriallist = new WxMaterialData().GetWxMateriallistbytypeid(sourcer.wxmaterialtypeid, topnums);
                                                        if (wxMateriallist.Count > 0)
                                                        {
                                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>" + wxMateriallist.Count + "</ArticleCount><Articles>";
                                                            foreach (WxMaterial m_wxmaterial in wxMateriallist)
                                                            {
                                                                string materialname = m_wxmaterial.Title;
                                                                string materialimgurl = FileSerivce.GetImgUrl(m_wxmaterial.Imgpath.ConvertTo<int>(0));
                                                                string linkurl = httphead + basic.Domain + aboutmaterialdetailurl + m_wxmaterial.MaterialId + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;


                                                                resxml += "<item><Title><![CDATA[" + materialname + "]]></Title><Description><![CDATA[]]></Description><PicUrl><![CDATA[" + materialimgurl + "]]></PicUrl><Url><![CDATA[" + linkurl + "]]></Url></item>";
                                                            }
                                                            resxml += "</Articles></xml>";
                                                        }
                                                    }
                                                }
                                                #endregion
                                                #region 单个文章
                                                else
                                                {
                                                    //根据素材id得到素材信息
                                                    WxMaterial m_wxmaterial = new WxMaterialData().GetWxMaterial(sourcer.Wxmaterialid);
                                                    if (m_wxmaterial != null)
                                                    {
                                                        string materialname = m_wxmaterial.Title;
                                                        string materialimgurl = FileSerivce.GetImgUrl(m_wxmaterial.Imgpath.ConvertTo<int>(0));
                                                        string linkurl = httphead + basic.Domain + aboutmaterialdetailurl + m_wxmaterial.MaterialId + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;

                                                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount>" +
                                                        "<Articles><item><Title><![CDATA[" + materialname + "]]></Title><Description><![CDATA[]]></Description><PicUrl><![CDATA[" + materialimgurl + "]]></PicUrl><Url><![CDATA[" + linkurl + "]]></Url></item></Articles></xml>";
                                                    }
                                                }
                                                #endregion
                                            }
                                            #endregion
                                            #region 5微商城
                                            else if (sourcer.qrcodeviewtype == 5)
                                            {
                                                B2b_company mcompany = new B2bCompanyData().GetMicromallByComid(basic.Comid);
                                                if (mcompany != null)
                                                {
                                                    //string linkurl = httphead + basic.Domain + "/h5/order/Default.aspx?uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                    //string content = "欢迎您访问我们的 <a href='" + linkurl + "'>微信小店</a>\n，优惠预定或查看推荐文章";

                                                    //resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                    string proimgurl = "";
                                                    B2b_company_saleset saleset = B2bCompanySaleSetData.GetDirectSellByComid(basic.Comid.ToString());
                                                    if (saleset != null)
                                                    {
                                                        proimgurl = FileSerivce.GetImgUrl(saleset.Smalllogo.ConvertTo<int>(0));
                                                    }
                                                    string linkurl = httphead + basic.Domain + "/h5/order/Default.aspx?uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                   
                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount>" +
                                                          "<Articles><item><Title><![CDATA[" + mcompany.Com_name + "]]></Title><Description><![CDATA[]]></Description><PicUrl><![CDATA[" + proimgurl + "]]></PicUrl><Url><![CDATA[" + linkurl + "]]></Url></item></Articles></xml>";
                                             
                                                }
                                            }
                                            #endregion
                                            #region 6门店
                                            else if (sourcer.qrcodeviewtype == 6)
                                            {
                                                Member_Channel_company c_company = new MemberChannelcompanyData().GetChannelCompany(sourcer.viewchannelcompanyid.ToString());
                                                if (c_company != null)
                                                {
                                                    string linkurl = httphead + basic.Domain + "/h5/StoreDefault.aspx?menshiid=" + c_company.Id + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                    string content = "欢迎您访问我们的门店：<a href='" + linkurl + "'>" + c_company.Companyname + "</a>\n";
                                                    //string companyimgurl = FileSerivce.GetImgUrl(c_company.Companyimg);

                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                }
                                                if (sourcer.viewchannelcompanyid == 0)
                                                {
                                                    string linkurl = httphead + basic.Domain + "/h5/Storelist.aspx?uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                    string content = "欢迎您访问<a href='" + linkurl + "'>我们的门店</a>\n";
                                                    //string companyimgurl = FileSerivce.GetImgUrl(c_company.Companyimg);

                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                }
                                            }
                                            #endregion
                                            #region 7合作单位
                                            else if (sourcer.qrcodeviewtype == 7)
                                            {
                                                Member_Channel_company c_company = new MemberChannelcompanyData().GetChannelCompany(sourcer.viewchannelcompanyid.ToString());
                                                if (c_company != null)
                                                {
                                                    string linkurl = httphead + basic.Domain + "/h5/StoreDefault.aspx?menshiid=" + c_company.Id + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                    string content = "欢迎您访问我们的门店：<a href='" + linkurl + "'>" + c_company.Companyname + "</a>\n";
                                                    //string companyimgurl = FileSerivce.GetImgUrl(c_company.Companyimg);

                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                }
                                                if (sourcer.viewchannelcompanyid == 0)
                                                {
                                                    string linkurl = httphead + basic.Domain + "/h5/Storelist.aspx?uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                    string content = "欢迎您访问<a href='" + linkurl + "'>我们的门店</a>\n";
                                                    //string companyimgurl = FileSerivce.GetImgUrl(c_company.Companyimg);

                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                }
                                            }
                                            #endregion
                                            #region 8顾问
                                            else if (sourcer.qrcodeviewtype == 8)
                                            {
                                                Member_Channel m_Channel = new MemberChannelData().GetChannelDetail(sourcer.Channelid);
                                                if (m_Channel.Whetherdefaultchannel == 1)
                                                {
                                                    string linkurl = "http://shop" + basic.Comid + ".etown.cn/h5/Peoplelist.aspx?uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;

                                                    string str = "点击查看 <a href='" + linkurl + "'>服务顾问列表</a>\n";

                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + str + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                                }
                                                else
                                                {
                                                    string name = m_Channel.Name;

                                                    //根据顾问信息中的电话得到 员工信息
                                                    B2b_company_manageuser u = new B2bCompanyManagerUserData().GetCompanyUserByPhone(m_Channel.Mobile, m_Channel.Com_id);

                                                    string linkurl = "";
                                                    if (u != null)
                                                    {
                                                        linkurl = "http://shop" + basic.Comid + ".etown.cn/h5/People.aspx?MasterId=" + u.Id + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                    }

                                                    string str = "点击联系 服务顾问：<a href='" + linkurl + "'>" + name + "</a>\n";

                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + str + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                                }

                                            }
                                            #endregion
                                            #region 9公司预订产品验证二维码
                                            else if (sourcer.qrcodeviewtype == 9)
                                            {
                                                string linkurl = "http://shop" + basic.Comid + ".etown.cn/h5/order/yuding_orderlist.aspx?uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;


                                                string str = "点击查看你的 <a href='" + linkurl + "'>预订产品订单</a>\n";
                                                int ordercount = new B2bOrderData().CountOrderServertype12(requestXML.FromUserName);

                                                if (ordercount == 0)
                                                {
                                                    str = "您没有预约产品订单\n";
                                                }

                                                resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + str + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                            }
                                            #endregion
                                            #region 10抽奖活动二维码
                                            else if (sourcer.qrcodeviewtype == 10)
                                            {
                                                string ntime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                string md5ntime = EncryptionHelper.ToMD5(ntime + "lixh1210", "UTF-8");

                                                string linkurl = "http://shop" + basic.Comid + ".etown.cn/m/Choujiang?uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass + "&ntime=" + ntime + "&md5ntime=" + md5ntime + "&actid=" + sourcer.choujiangactid;

                                                string str = "<a href='" + linkurl + "'>请点击进入抽奖活动页面</a>\n";

                                                resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + str + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                            }
                                            #endregion
                                        }
                                        #endregion
                                        #region 没有绑定渠道的二维码
                                        else //没有绑定渠道的二维码
                                        {
                                            #region 扫描素材二维码
                                            if (sourcer.Wxmaterialid > 0)//扫描素材二维码
                                            {
                                                //根据素材id得到素材信息
                                                WxMaterial m_wxmaterial = new WxMaterialData().GetWxMaterial(sourcer.Wxmaterialid);
                                                if (m_wxmaterial != null)
                                                {
                                                    string materialname = m_wxmaterial.Title;
                                                    string materialimgurl = FileSerivce.GetImgUrl(m_wxmaterial.Imgpath.ConvertTo<int>(0));
                                                    string linkurl = httphead + basic.Domain + aboutmaterialdetailurl + m_wxmaterial.MaterialId + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;

                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount>" +
                                                    "<Articles><item><Title><![CDATA[" + materialname + "]]></Title><Description><![CDATA[]]></Description><PicUrl><![CDATA[" + materialimgurl + "]]></PicUrl><Url><![CDATA[" + linkurl + "]]></Url></item></Articles></xml>";
                                                }
                                            }
                                            #endregion
                                            #region 扫描产品二维码
                                            if (sourcer.Productid > 0)//扫描产品二维码
                                            {
                                                //根据产品id得到产品信息
                                                B2b_com_pro pro = new B2bComProData().GetProById(sourcer.Productid.ToString());
                                                if (pro != null)
                                                {
                                                    if (pro.Server_type != 3)
                                                    {
                                                        string proname = pro.Pro_name;
                                                        string proimgurl = FileSerivce.GetImgUrl(pro.Imgurl);
                                                        string linkurl = "";
                                                        if (pro.Server_type == 1 || pro.Server_type == 12)//票务 或者 预约产品
                                                        {
                                                            linkurl = httphead + basic.Domain + "/h5/order/pro.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                        }
                                                        else if (pro.Server_type == 2 || pro.Server_type == 8)//旅游
                                                        {
                                                            linkurl = httphead + basic.Domain + "/h5/linedetail.aspx?lineid=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                        }
                                                        else if (pro.Server_type == 9)//酒店客房
                                                        {
                                                            linkurl = httphead + basic.Domain + "/h5/hotel/hotelshow.aspx?proid=" + pro.Id + "&id=" + basic.Comid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                        }
                                                        else if (pro.Server_type == 11)//实物
                                                        {
                                                            linkurl = httphead + basic.Domain + "/h5/order/pro.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                        }
                                                        else if (pro.Server_type == 14)//保险产品
                                                        {
                                                            linkurl = httphead + basic.Domain + "/h5/OrderEnter.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                        }
                                                        else if (pro.Server_type == 10)//旅游大巴
                                                        {
                                                            linkurl = httphead + basic.Domain + "/h5/OrderEnter.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                        }
                                                        else
                                                        {
                                                            linkurl = "";
                                                        }

                                                        string adviseprice = "￥" + pro.Advise_price.ToString("F2");
                                                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount>" +
                                                        "<Articles><item><Title><![CDATA[" + proname + "]]></Title><Description><![CDATA[" + adviseprice + "]]></Description><PicUrl><![CDATA[" + proimgurl + "]]></PicUrl><Url><![CDATA[" + linkurl + "]]></Url></item></Articles></xml>";
                                                    }
                                                    else
                                                    {
                                                        resxml = "";
                                                        //resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                    }
                                                }
                                            }
                                            #endregion
                                            #region 扫描门市二维码
                                            if (sourcer.Channelcompanyid > 0)//扫描门市二维码
                                            {
                                                Member_Channel_company company = new MemberChannelcompanyData().GetChannelCompanyByWxsourceId(wxsourceid);
                                                if (company != null)
                                                {
                                                    if (company.Issuetype == 1)//如果用户二次扫描的是合作单位，则显示合作单位信息
                                                    {
                                                        #region  已经注释掉，原来的显示内容
                                                        //  B2b_company gongsi = B2bCompanyData.GetCompany(basic.Comid);

                                                        //  string outstr = "感谢您关注 " + gongsi.Com_name + "特约合作商户:\n\n" +
                                                        //"<a href='" + httphead + basic.Domain + "/h5/StoreDefault.aspx?menshiid=" + company.Id + "'>" + company.Companyname + "</a>\n";
                                                        //  if (company.Companyphone.Trim() != "")
                                                        //  {
                                                        //      outstr += "电话 " + company.Companyphone + "\n";
                                                        //  }
                                                        //  if (company.Companyaddress.Trim() != "")
                                                        //  {
                                                        //      outstr += "地址 " + company.Companyaddress + "\n";
                                                        //  }
                                                        //  if (company.Bookurl.Trim() != "")
                                                        //  {
                                                        //      outstr += "<a href='" + company.Bookurl.Trim() + "'>点击立即预定 </a>\n ";
                                                        //  }

                                                        //  resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + outstr + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                        #endregion
                                                        Member_Channel_company c_company = new MemberChannelcompanyData().GetChannelCompany(sourcer.viewchannelcompanyid.ToString());
                                                        if (c_company != null)
                                                        {
                                                            string linkurl = httphead + basic.Domain + "/h5/StoreDefault.aspx?menshiid=" + c_company.Id + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            string content = "欢迎您访问我们的门店：<a href='" + linkurl + "'>" + c_company.Companyname + "</a>\n";
                                                            //string companyimgurl = FileSerivce.GetImgUrl(c_company.Companyimg);

                                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                        }
                                                        if (sourcer.viewchannelcompanyid == 0)
                                                        {
                                                            string linkurl = httphead + basic.Domain + "/h5/Storelist.aspx?uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            string content = "欢迎您访问<a href='" + linkurl + "'>我们的门店</a>\n";
                                                            //string companyimgurl = FileSerivce.GetImgUrl(c_company.Companyimg);

                                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                        }
                                                    }
                                                    if (company.Issuetype == 0)//如果扫描的是内部门店二维码
                                                    {
                                                        #region 已经注释掉，原来的显示内容
                                                        ////根据微信号获判断会员渠道信息(如果会员是公司会员 即渠道为微信注册渠道，则渠道修改为门市默认渠道)
                                                        //bool iscompanycrm = new B2bCrmData().IsCompanyCrm(requestXML.FromUserName, basic.Comid);
                                                        //if (iscompanycrm)
                                                        //{
                                                        //    //int channelid = 0;//默认渠道id
                                                        //    //根据渠道单位id得到 默认渠道人
                                                        //    Member_Channel defaultchannel = new MemberChannelData().GetDefaultChannel(company.Id);

                                                        //    if (defaultchannel == null)//没有默认渠道人，添加渠道单位的默认渠道人
                                                        //    {

                                                        //        Member_Channel channel = new Member_Channel()
                                                        //        {
                                                        //            Id = 0,
                                                        //            Com_id = company.Com_id,
                                                        //            Issuetype = company.Issuetype,
                                                        //            Companyid = company.Id,
                                                        //            Name = "默认渠道",
                                                        //            Mobile = "",
                                                        //            Cardcode = 0,
                                                        //            Chaddress = "",
                                                        //            ChObjects = "",
                                                        //            RebateOpen = 0,
                                                        //            RebateConsume = 0,
                                                        //            RebateConsume2 = 0,
                                                        //            RebateLevel = 0,
                                                        //            Opencardnum = 0,
                                                        //            Firstdealnum = 0,
                                                        //            Summoney = 0,
                                                        //            Whetherdefaultchannel = 1,
                                                        //            Runstate = 1
                                                        //        };
                                                        //        channelid = new MemberChannelData().EditChannel(channel);
                                                        //    }
                                                        //    else
                                                        //    {
                                                        //        channelid = defaultchannel.Id;
                                                        //    }
                                                        //    if (channelid > 0)
                                                        //    {
                                                        //        int resultt = new MemberCardData().UpMemberChennel(requestXML.FromUserName, channelid);
                                                        //    }

                                                        //    string innerstr = "感谢您关注 " + company.Companyname + "\n" +

                                                        //      "电话 " + company.Companyphone + "\n" +
                                                        //      "地址 " + company.Companyaddress + "\n" +

                                                        //       "<a href='" + httphead + basic.Domain + "/h5/StoreDefault.aspx?menshiid=" + company.Id + "'> 请点击链接查看更多...</a>\n";

                                                        //    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + innerstr + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                                        //}
                                                        #endregion
                                                        Member_Channel_company c_company = new MemberChannelcompanyData().GetChannelCompany(sourcer.viewchannelcompanyid.ToString());
                                                        if (c_company != null)
                                                        {
                                                            string linkurl = httphead + basic.Domain + "/h5/StoreDefault.aspx?menshiid=" + c_company.Id + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            string content = "欢迎您访问我们的门店：<a href='" + linkurl + "'>" + c_company.Companyname + "</a>\n";
                                                            //string companyimgurl = FileSerivce.GetImgUrl(c_company.Companyimg);

                                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                        }
                                                        if (sourcer.viewchannelcompanyid == 0)
                                                        {
                                                            string linkurl = httphead + basic.Domain + "/h5/Storelist.aspx?uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            string content = "欢迎您访问<a href='" + linkurl + "'>我们的门店</a>\n";
                                                            //string companyimgurl = FileSerivce.GetImgUrl(c_company.Companyimg);

                                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                        #endregion
                                    }

                                }
                                #endregion
                                ////获得微信用户基本信息
                                //GetWeiXinCrmBasic(basic, requestXML.FromUserName);
                            }
                        }
                    }
                    #endregion
                    #region 取消关注事件
                    if (requestXML.Eevent == "unsubscribe")
                    {
                        lock (lockobj)
                        {
                            //判断是否是并发请求
                            string cretime = requestXML.CreateTime;
                            //获得请求发送的次数
                            int reqnum = new WxSubscribeDetailData().GetReqnum(requestXML.FromUserName, cretime, "unsubscribe");
                            if (reqnum == 0)
                            {
                                #region 记录用户取消关注事件
                                requestXML.Content = "用户取消关注事件";
                                requestXML.ContentType = true;
                                requestXML.Comid = basic.Comid;
                                int inswxexchangemsg = new WxRequestXmlData().EditWxRequestXmlLog(requestXML);
                                #endregion

                                //根据微信号得到二维码所带参数
                                int eventkey = 0;
                                WxSubscribeDetail subscribedetail = new WxSubscribeSourceData().GetWXSourceByOpenId(requestXML.FromUserName);
                                if (subscribedetail != null)
                                {
                                    eventkey = subscribedetail.Subscribesourceid;
                                }

                                //用户取消关注:(1)会员表中标注状态:未关注(0);微信激活状态为：激活(1);(2)卡号表中对应的渠道修改为微信注册渠道
                                //得到会员信息
                                B2b_crm crmmm = new B2bCrmData().GetB2bCrm(requestXML.FromUserName, basic.Comid);
                                if (crmmm != null)
                                {
                                    SqlHelper sqlhelper = new SqlHelper();
                                    sqlhelper.BeginTrancation();
                                    try
                                    {

                                        SqlCommand cmd = new SqlCommand();
                                        cmd = sqlhelper.PrepareTextSqlCommand("update b2b_crm set whetherwxfocus=0,whetheractivate =1 where weixin='" + requestXML.FromUserName + "' and com_id=" + basic.Comid);//会员表中标注状态:未关注
                                        cmd.ExecuteNonQuery();
                                        cmd = sqlhelper.PrepareTextSqlCommand("update Member_Card set IssueCard=(select id from Member_Channel where Com_id=" + basic.Comid + " and Issuetype=4 ) where Cardcode =(select IDcard from b2b_crm where weixin='" + requestXML.FromUserName + "' and com_id=" + basic.Comid + ")");//卡号表中对应的渠道修改为微信注册渠道
                                        cmd.ExecuteNonQuery();
                                        //微信取消记录录入扫码记录表
                                        cmd = sqlhelper.PrepareTextSqlCommand("INSERT INTO [EtownDB].[dbo].[WxSubscribeDetail] ([openid] ,[subscribetime],[subscribesourceid] ,[event],[eventkey],[comid] ,createtime)VALUES" +
                                       "('" + requestXML.FromUserName + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + eventkey + " ,'" + requestXML.Eevent + "','" + eventkey + "'," + basic.Comid + " ,'" + requestXML.CreateTime + "')");
                                        cmd.ExecuteNonQuery();
                                        sqlhelper.Commit();
                                    }
                                    catch (Exception e)
                                    {
                                        sqlhelper.Rollback();
                                    }
                                    finally
                                    {
                                        sqlhelper.Dispose();
                                    }

                                }
                            }
                        }
                    }
                    #endregion

                    #region   已关注微信用户 扫描带参数二维码事件
                    if (requestXML.Eevent == "SCAN")
                    {
                        lock (lockobj)
                        {
                            //判断是否是并发请求
                            string cretime = requestXML.CreateTime;
                            //获得请求发送的次数
                            int reqnum = new WxSubscribeDetailData().GetReqnum(requestXML.FromUserName, cretime, "SCAN");
                            if (reqnum == 0)
                            {
                                #region 记录已关注微信用户二次扫码事件
                                requestXML.Content = "已关注微信用户二次扫码事件";
                                requestXML.ContentType = true;
                                requestXML.Comid = basic.Comid;
                                int inswxexchangemsg = new WxRequestXmlData().EditWxRequestXmlLog(requestXML);
                                #endregion
                                WxSubscribeDetail subscribemodel = new WxSubscribeDetail();
                                subscribemodel.Id = 0;
                                subscribemodel.Openid = requestXML.FromUserName;
                                subscribemodel.Subscribetime = DateTime.Now;
                                subscribemodel.Subscribesourceid = requestXML.EventKey.Substring(requestXML.EventKey.IndexOf("_") + 1).ConvertTo<int>(0);
                                subscribemodel.Eevent = requestXML.Eevent;
                                subscribemodel.Eventkey = requestXML.EventKey;
                                subscribemodel.Comid = basic.Comid;
                                subscribemodel.Createtime = requestXML.CreateTime;

                                int editsubscribedetail = new WxSubscribeDetailData().EditSubscribeDetail(subscribemodel);


                                //用户扫描带参二维码，被动回复消息
                                int wxsourceid = requestXML.EventKey.Substring(requestXML.EventKey.IndexOf("_") + 1).ConvertTo<int>(0);
                                if (wxsourceid > 0)
                                {
                                    WxSubscribeSource sourcer = new WxSubscribeSourceData().GetWXSourceById(wxsourceid);

                                    //设定并获取微信随机码                              
                                    var pass = new B2bCrmData().WeixinGetPass(requestXML.FromUserName, basic.Comid);

                                    //根据微信号获得会员信息
                                    B2b_crm crminfo = new B2bCrmData().GetB2bCrmByWeiXin(requestXML.FromUserName, basic.Comid);
                                    if (crminfo == null)//会员信息为空时，添加此会员信息
                                    {
                                        resxml = InsB2bCrm(basic, requestXML, "");
                                    }
                                    #region 永久二维码
                                    if (sourcer != null)
                                    {
                                        #region  绑定有渠道的二维码
                                        if (sourcer.qrcodeviewtype > 0)//绑定有渠道的二维码
                                        {
                                            #region 1营销活动
                                            if (sourcer.qrcodeviewtype == 1)
                                            {
                                                Member_Activity m = MemberActivityData.GetActById(sourcer.Activityid);
                                                if (m != null)
                                                {
                                                    string content = m.Title + "\n" + m.Actstar.ToString("yyyy-MM-dd") + "-" + m.Actend.ToString("yyyy-MM-dd");

                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                }
                                            }
                                            #endregion
                                            #region 2产品图文
                                            else if (sourcer.qrcodeviewtype == 2)
                                            {
                                                #region 显示项目下产品
                                                if (sourcer.Productid == 0)//显示项目下产品
                                                {
                                                    int projectid = sourcer.projectid;
                                                    if (projectid > 0)
                                                    {
                                                        string servertypes = "1,2,8,9,10,11,12";//显示的服务类型:电子票；跟团游；当地游;酒店；实物
                                                        int topnums = 10;//显示数量
                                                        IList<B2b_com_pro> prolist = new B2bComProData().GetWxProlistbyprojectid(projectid, servertypes, topnums);
                                                        if (prolist.Count > 0)
                                                        {
                                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>" + prolist.Count + "</ArticleCount><Articles>";
                                                            foreach (B2b_com_pro pro in prolist)
                                                            {
                                                                string proname = pro.Pro_name;
                                                                string proimgurl = FileSerivce.GetImgUrl(pro.Imgurl);
                                                                string linkurl = "";

                                                                if (pro.Server_type == 1 || pro.Server_type == 12)//票务 或者 预约产品
                                                                {
                                                                    linkurl = httphead + basic.Domain + "/h5/order/pro.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                                }
                                                                else if (pro.Server_type == 2 || pro.Server_type == 8)//旅游
                                                                {
                                                                    linkurl = httphead + basic.Domain + "/h5/linedetail.aspx?lineid=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                                }
                                                                else if (pro.Server_type == 9)//酒店客房
                                                                {
                                                                    linkurl = httphead + basic.Domain + "/h5/hotel/hotelshow.aspx?proid=" + pro.Id + "&id=" + basic.Comid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                                }
                                                                else if (pro.Server_type == 11)//实物
                                                                {
                                                                    linkurl = httphead + basic.Domain + "/h5/order/pro.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                                }
                                                                else if (pro.Server_type == 10)//旅游大巴
                                                                {
                                                                    linkurl = httphead + basic.Domain + "/h5/OrderEnter.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                                }
                                                                else
                                                                {
                                                                    linkurl = "";
                                                                }

                                                                string adviseprice = "￥" + pro.Advise_price.ToString("F2");

                                                                resxml += "<item><Title><![CDATA[" + pro.Pro_name + "]]></Title><Description><![CDATA[" + adviseprice + "]]></Description><PicUrl><![CDATA[" + proimgurl + "]]></PicUrl><Url><![CDATA[" + linkurl + "]]></Url></item>";
                                                            }
                                                            resxml += "</Articles></xml>";
                                                        }
                                                    }
                                                }
                                                #endregion
                                                #region 显示特定产品
                                                else //显示特定产品
                                                {
                                                    //根据产品id得到产品信息
                                                    B2b_com_pro pro = new B2bComProData().GetProById(sourcer.Productid.ToString());
                                                    if (pro != null)
                                                    {
                                                        if (pro.Server_type != 3)
                                                        {
                                                            string proname = pro.Pro_name;
                                                            string proimgurl = FileSerivce.GetImgUrl(pro.Imgurl);
                                                            string linkurl = "";
                                                            if (pro.Server_type == 1 || pro.Server_type == 12)//票务 或者 预约产品
                                                            {
                                                                linkurl = httphead + basic.Domain + "/h5/order/pro.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            }
                                                            else if (pro.Server_type == 2 || pro.Server_type == 8)//旅游
                                                            {
                                                                linkurl = httphead + basic.Domain + "/h5/linedetail.aspx?lineid=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            }
                                                            else if (pro.Server_type == 9)//酒店客房
                                                            {
                                                                linkurl = httphead + basic.Domain + "/h5/hotel/hotelshow.aspx?proid=" + pro.Id + "&id=" + basic.Comid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            }
                                                            else if (pro.Server_type == 11)//实物
                                                            {
                                                                linkurl = httphead + basic.Domain + "/h5/order/pro.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            }
                                                            else if (pro.Server_type == 14)//保险产品
                                                            {
                                                                linkurl = httphead + basic.Domain + "/h5/OrderEnter.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            }
                                                            else if (pro.Server_type == 10)//旅游大巴
                                                            {
                                                                linkurl = httphead + basic.Domain + "/h5/OrderEnter.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            }
                                                            else
                                                            {
                                                                linkurl = "";
                                                            }

                                                            string adviseprice = "￥" + pro.Advise_price.ToString("F2");
                                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount>" +
                                                            "<Articles><item><Title><![CDATA[" + proname + "]]></Title><Description><![CDATA[" + adviseprice + "]]></Description><PicUrl><![CDATA[" + proimgurl + "]]></PicUrl><Url><![CDATA[" + linkurl + "]]></Url></item></Articles></xml>";
                                                        }
                                                        else
                                                        {
                                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                            #endregion
                                            #region 3分销商，暂时 没东西
                                            else if (sourcer.qrcodeviewtype == 3)
                                            {

                                            }
                                            #endregion
                                            #region  4文章图文
                                            else if (sourcer.qrcodeviewtype == 4)
                                            {
                                                #region 文章类型下文章列表
                                                if (sourcer.Wxmaterialid == 0)
                                                {
                                                    if (sourcer.wxmaterialtypeid > 0)
                                                    {
                                                        int topnums = 10;//显示数量
                                                        IList<WxMaterial> wxMateriallist = new WxMaterialData().GetWxMateriallistbytypeid(sourcer.wxmaterialtypeid, topnums);
                                                        if (wxMateriallist.Count > 0)
                                                        {
                                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>" + wxMateriallist.Count + "</ArticleCount><Articles>";
                                                            foreach (WxMaterial m_wxmaterial in wxMateriallist)
                                                            {
                                                                string materialname = m_wxmaterial.Title;
                                                                string materialimgurl = FileSerivce.GetImgUrl(m_wxmaterial.Imgpath.ConvertTo<int>(0));
                                                                string linkurl = httphead + basic.Domain + aboutmaterialdetailurl + m_wxmaterial.MaterialId + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;


                                                                resxml += "<item><Title><![CDATA[" + materialname + "]]></Title><Description><![CDATA[]]></Description><PicUrl><![CDATA[" + materialimgurl + "]]></PicUrl><Url><![CDATA[" + linkurl + "]]></Url></item>";
                                                            }
                                                            resxml += "</Articles></xml>";
                                                        }
                                                    }
                                                }
                                                #endregion
                                                #region 单个文章
                                                else
                                                {
                                                    //根据素材id得到素材信息
                                                    WxMaterial m_wxmaterial = new WxMaterialData().GetWxMaterial(sourcer.Wxmaterialid);
                                                    if (m_wxmaterial != null)
                                                    {
                                                        string materialname = m_wxmaterial.Title;
                                                        string materialimgurl = FileSerivce.GetImgUrl(m_wxmaterial.Imgpath.ConvertTo<int>(0));
                                                        string linkurl = httphead + basic.Domain + aboutmaterialdetailurl + m_wxmaterial.MaterialId + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;

                                                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount>" +
                                                        "<Articles><item><Title><![CDATA[" + materialname + "]]></Title><Description><![CDATA[]]></Description><PicUrl><![CDATA[" + materialimgurl + "]]></PicUrl><Url><![CDATA[" + linkurl + "]]></Url></item></Articles></xml>";
                                                    }
                                                }
                                                #endregion
                                            }
                                            #endregion
                                            #region 5微商城
                                            else if (sourcer.qrcodeviewtype == 5)
                                            {
                                                B2b_company mcompany = new B2bCompanyData().GetMicromallByComid(basic.Comid);
                                                if (mcompany != null)
                                                {
                                                    //string linkurl = httphead + basic.Domain + "/h5/order/Default.aspx?uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                    //string content = "欢迎您访问我们的 <a href='" + linkurl + "'>微信小店</a>\n，优惠预定或查看推荐文章";

                                                    //resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                    string proimgurl = "";
                                                    B2b_company_saleset saleset = B2bCompanySaleSetData.GetDirectSellByComid(basic.Comid.ToString());
                                                    if (saleset != null)
                                                    {
                                                        proimgurl = FileSerivce.GetImgUrl(saleset.Smalllogo.ConvertTo<int>(0));
                                                    }
                                                    string linkurl = httphead + basic.Domain + "/h5/order/Default.aspx?uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                    
                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount>" +
                                                          "<Articles><item><Title><![CDATA[" + mcompany.Com_name + "]]></Title><Description><![CDATA[]]></Description><PicUrl><![CDATA[" + proimgurl + "]]></PicUrl><Url><![CDATA[" + linkurl + "]]></Url></item></Articles></xml>";
                                             
                                                }
                                            }
                                            #endregion
                                            #region 6门店
                                            else if (sourcer.qrcodeviewtype == 6)
                                            {
                                                Member_Channel_company c_company = new MemberChannelcompanyData().GetChannelCompany(sourcer.viewchannelcompanyid.ToString());
                                                if (c_company != null)
                                                {
                                                    string linkurl = httphead + basic.Domain + "/h5/StoreDefault.aspx?menshiid=" + c_company.Id + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                    string content = "欢迎您访问我们的门店：<a href='" + linkurl + "'>" + c_company.Companyname + "</a>\n";
                                                    //string companyimgurl = FileSerivce.GetImgUrl(c_company.Companyimg);

                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                }
                                                if (sourcer.viewchannelcompanyid == 0)
                                                {
                                                    string linkurl = httphead + basic.Domain + "/h5/Storelist.aspx?uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                    string content = "欢迎您访问<a href='" + linkurl + "'>我们的门店</a>\n";
                                                    //string companyimgurl = FileSerivce.GetImgUrl(c_company.Companyimg);

                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                }
                                            }
                                            #endregion
                                            #region 7合作单位
                                            else if (sourcer.qrcodeviewtype == 7)
                                            {
                                                Member_Channel_company c_company = new MemberChannelcompanyData().GetChannelCompany(sourcer.viewchannelcompanyid.ToString());
                                                if (c_company != null)
                                                {
                                                    string linkurl = httphead + basic.Domain + "/h5/StoreDefault.aspx?menshiid=" + c_company.Id + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                    string content = "欢迎您访问我们的门店：<a href='" + linkurl + "'>" + c_company.Companyname + "</a>\n";
                                                    //string companyimgurl = FileSerivce.GetImgUrl(c_company.Companyimg);

                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                }
                                                if (sourcer.viewchannelcompanyid == 0)
                                                {
                                                    string linkurl = httphead + basic.Domain + "/h5/Storelist.aspx?uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                    string content = "欢迎您访问<a href='" + linkurl + "'>我们的门店</a>\n";
                                                    //string companyimgurl = FileSerivce.GetImgUrl(c_company.Companyimg);

                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                }
                                            }
                                            #endregion
                                            #region 8顾问
                                            else if (sourcer.qrcodeviewtype == 8)
                                            {
                                                Member_Channel m_Channel = new MemberChannelData().GetChannelDetail(sourcer.Channelid);
                                                if (m_Channel.Whetherdefaultchannel == 1)
                                                {
                                                    string linkurl = "http://shop" + basic.Comid + ".etown.cn/h5/Peoplelist.aspx?uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;

                                                    string str = "点击查看 <a href='" + linkurl + "'>服务顾问列表</a>\n";

                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + str + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                                }
                                                else
                                                {
                                                    string name = m_Channel.Name;

                                                    //根据顾问信息中的电话得到 员工信息
                                                    B2b_company_manageuser u = new B2bCompanyManagerUserData().GetCompanyUserByPhone(m_Channel.Mobile, m_Channel.Com_id);

                                                    string linkurl = "";
                                                    if (u != null)
                                                    {
                                                        linkurl = "http://shop" + basic.Comid + ".etown.cn/h5/People.aspx?MasterId=" + u.Id + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                    }

                                                    string str = "点击联系 服务顾问：<a href='" + linkurl + "'>" + name + "</a>\n";

                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + str + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                                }
                                            }
                                            #endregion
                                            #region 9公司预订产品验证二维码
                                            else if (sourcer.qrcodeviewtype == 9)
                                            {
                                                string ntime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                string md5ntime = EncryptionHelper.ToMD5(ntime + "lixh1210", "UTF-8");

                                                string linkurl = "http://shop" + basic.Comid + ".etown.cn/h5/order/yuding_orderlist.aspx?uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass + "&ntime=" + ntime + "&md5ntime=" + md5ntime;


                                                string str = "点击查看你的 <a href='" + linkurl + "'>预订产品订单</a>\n";
                                                int ordercount = new B2bOrderData().CountOrderServertype12(requestXML.FromUserName);

                                                if (ordercount == 0)
                                                {
                                                    str = "您没有预约产品订单\n";
                                                }

                                                resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + str + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                            }
                                            #endregion
                                            #region 10抽奖活动二维码
                                            else if (sourcer.qrcodeviewtype == 10)
                                            {
                                                string ntime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                string md5ntime = EncryptionHelper.ToMD5(ntime + "lixh1210", "UTF-8");

                                                string linkurl = "http://shop" + basic.Comid + ".etown.cn/m/Choujiang?uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass + "&ntime=" + ntime + "&md5ntime=" + md5ntime + "&actid=" + sourcer.choujiangactid;

                                                string str = "<a href='" + linkurl + "'>请点击进入抽奖活动页面</a>\n";
                                              
                                                resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + str + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                            }
                                            #endregion

                                            #region 判断顾问（渠道人）是否变更了，变更的话切换顾问（渠道人）；并且显示变更后的顾问（渠道人）信息-如果变更后为默认渠道人不显示
                                            int nowchannelid = sourcer.Channelid;
                                            if (nowchannelid > 0 && nowchannelid != channelid)
                                            {
                                                var crmmodel = new B2bCrmData().GetB2bCrm(requestXML.FromUserName, basic.Comid);
                                                if (crmmodel != null)
                                                {
                                                    //变更顾问(渠道人) 
                                                    int upchannel = new MemberCardData().upCardcodeChannel(crmmodel.Idcard.ToString(), nowchannelid);
                                                    if (upchannel > 0)
                                                    {
                                                        //如果顾问(渠道人)不是默认渠道人,则发送顾问信息     
                                                        Member_Channel nowchannel = new MemberChannelData().GetChannelDetail(nowchannelid);
                                                        if (nowchannel.Whetherdefaultchannel != 1)
                                                        {
                                                            //发送顾问信息
                                                            CustomerMsg_Send.AutoFenpeiChannel(basic.Comid, requestXML.FromUserName, 1, sourcer.Channelcompanyid, sourcer.Channelid);
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                        #endregion
                                        #region 没有绑定渠道的二维码
                                        else //没有绑定渠道的二维码
                                        {
                                            #region 扫描素材二维码
                                            if (sourcer.Wxmaterialid > 0)//扫描素材二维码
                                            {
                                                //根据素材id得到素材信息
                                                WxMaterial m_wxmaterial = new WxMaterialData().GetWxMaterial(sourcer.Wxmaterialid);
                                                if (m_wxmaterial != null)
                                                {
                                                    string materialname = m_wxmaterial.Title;
                                                    string materialimgurl = FileSerivce.GetImgUrl(m_wxmaterial.Imgpath.ConvertTo<int>(0));
                                                    string linkurl = httphead + basic.Domain + aboutmaterialdetailurl + m_wxmaterial.MaterialId + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;


                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount>" +
                                                    "<Articles><item><Title><![CDATA[" + materialname + "]]></Title><Description><![CDATA[]]></Description><PicUrl><![CDATA[" + materialimgurl + "]]></PicUrl><Url><![CDATA[" + linkurl + "]]></Url></item></Articles></xml>";
                                                }
                                            }
                                            #endregion
                                            #region 扫描产品二维码
                                            if (sourcer.Productid > 0)//扫描产品二维码
                                            {
                                                //根据产品id得到产品信息
                                                B2b_com_pro pro = new B2bComProData().GetProById(sourcer.Productid.ToString());
                                                if (pro != null)
                                                {
                                                    if (pro.Server_type != 3)
                                                    {

                                                        string proname = pro.Pro_name;
                                                        string proimgurl = FileSerivce.GetImgUrl(pro.Imgurl);
                                                        string linkurl = "";
                                                        if (pro.Server_type == 1 || pro.Server_type == 12)//票务 或者 预约产品
                                                        {
                                                            linkurl = httphead + basic.Domain + "/h5/order/pro.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                        }
                                                        else if (pro.Server_type == 2 || pro.Server_type == 8)//旅游
                                                        {
                                                            linkurl = httphead + basic.Domain + "/h5/linedetail.aspx?lineid=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                        }
                                                        else if (pro.Server_type == 9)//酒店客房
                                                        {
                                                            linkurl = httphead + basic.Domain + "/h5/hotel/hotelshow.aspx?proid=" + pro.Id + "&id=" + basic.Comid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                        }
                                                        else if (pro.Server_type == 11)//实物
                                                        {
                                                            linkurl = httphead + basic.Domain + "/h5/order/pro.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                        }
                                                        else if (pro.Server_type == 14)//保险产品
                                                        {
                                                            linkurl = httphead + basic.Domain + "/h5/order/pro.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                        }
                                                        else if (pro.Server_type == 10)//旅游大巴
                                                        {
                                                            linkurl = httphead + basic.Domain + "/h5/order/pro.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                        }

                                                        else
                                                        {
                                                            linkurl = httphead + basic.Domain + "/h5/order/pro.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                        }

                                                        string adviseprice = "￥" + pro.Advise_price.ToString("F2");
                                                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount>" +
                                                        "<Articles><item><Title><![CDATA[" + proname + "]]></Title><Description><![CDATA[" + adviseprice + "]]></Description><PicUrl><![CDATA[" + proimgurl + "]]></PicUrl><Url><![CDATA[" + linkurl + "]]></Url></item></Articles></xml>";
                                                    }
                                                    else
                                                    {
                                                        resxml = "";
                                                        //resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                    }
                                                }
                                            }
                                            #endregion
                                            #region 扫描门市二维码
                                            if (sourcer.Channelcompanyid > 0)//扫描门市二维码
                                            {
                                                Member_Channel_company company = new MemberChannelcompanyData().GetChannelCompanyByWxsourceId(wxsourceid);
                                                if (company != null)
                                                {
                                                    if (company.Issuetype == 1)//如果用户二次扫描的是合作单位，则显示合作单位信息
                                                    {
                                                        #region  已经注释掉，原来的显示内容
                                                        //  B2b_company gongsi = B2bCompanyData.GetCompany(basic.Comid);

                                                        //  string outstr = "感谢您关注 " + gongsi.Com_name + "特约合作商户:\n\n" +
                                                        //"<a href='" + httphead + basic.Domain + "/h5/StoreDefault.aspx?menshiid=" + company.Id + "'>" + company.Companyname + "</a>\n";
                                                        //  if (company.Companyphone.Trim() != "")
                                                        //  {
                                                        //      outstr += "电话 " + company.Companyphone + "\n";
                                                        //  }
                                                        //  if (company.Companyaddress.Trim() != "")
                                                        //  {
                                                        //      outstr += "地址 " + company.Companyaddress + "\n";
                                                        //  }
                                                        //  if (company.Bookurl.Trim() != "")
                                                        //  {
                                                        //      outstr += "<a href='" + company.Bookurl.Trim() + "'>点击立即预定 </a>\n ";
                                                        //  }

                                                        //  resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + outstr + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                        #endregion
                                                        Member_Channel_company c_company = new MemberChannelcompanyData().GetChannelCompany(sourcer.Channelcompanyid.ToString());
                                                        if (c_company != null)
                                                        {
                                                            string linkurl = httphead + basic.Domain + "/h5/StoreDefault.aspx?menshiid=" + c_company.Id + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            string content = "欢迎您访问我们的门店：<a href='" + linkurl + "'>" + c_company.Companyname + "</a>\n";
                                                            //string companyimgurl = FileSerivce.GetImgUrl(c_company.Companyimg);

                                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                        }
                                                        else
                                                        {
                                                            string linkurl = httphead + basic.Domain + "/h5/Storelist.aspx?uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            string content = "欢迎您访问<a href='" + linkurl + "'>我们的门店</a>\n";
                                                            //string companyimgurl = FileSerivce.GetImgUrl(c_company.Companyimg);

                                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                        }
                                                    }
                                                    if (company.Issuetype == 0)//如果扫描的是内部门店二维码
                                                    {
                                                        #region 已经注释掉，原来的显示内容
                                                        ////根据微信号获判断会员渠道信息(如果会员是公司会员 即渠道为微信注册渠道，则渠道修改为门市默认渠道)
                                                        //bool iscompanycrm = new B2bCrmData().IsCompanyCrm(requestXML.FromUserName, basic.Comid);
                                                        //if (iscompanycrm)
                                                        //{
                                                        //    //int channelid = 0;//默认渠道id
                                                        //    //根据渠道单位id得到 默认渠道人
                                                        //    Member_Channel defaultchannel = new MemberChannelData().GetDefaultChannel(company.Id);

                                                        //    if (defaultchannel == null)//没有默认渠道人，添加渠道单位的默认渠道人
                                                        //    {

                                                        //        Member_Channel channel = new Member_Channel()
                                                        //        {
                                                        //            Id = 0,
                                                        //            Com_id = company.Com_id,
                                                        //            Issuetype = company.Issuetype,
                                                        //            Companyid = company.Id,
                                                        //            Name = "默认渠道",
                                                        //            Mobile = "",
                                                        //            Cardcode = 0,
                                                        //            Chaddress = "",
                                                        //            ChObjects = "",
                                                        //            RebateOpen = 0,
                                                        //            RebateConsume = 0,
                                                        //            RebateConsume2 = 0,
                                                        //            RebateLevel = 0,
                                                        //            Opencardnum = 0,
                                                        //            Firstdealnum = 0,
                                                        //            Summoney = 0,
                                                        //            Whetherdefaultchannel = 1,
                                                        //            Runstate = 1
                                                        //        };
                                                        //        channelid = new MemberChannelData().EditChannel(channel);
                                                        //    }
                                                        //    else
                                                        //    {
                                                        //        channelid = defaultchannel.Id;
                                                        //    }
                                                        //    if (channelid > 0)
                                                        //    {
                                                        //        int resultt = new MemberCardData().UpMemberChennel(requestXML.FromUserName, channelid);
                                                        //    }

                                                        //    string innerstr = "感谢您关注 " + company.Companyname + "\n" +

                                                        //      "电话 " + company.Companyphone + "\n" +
                                                        //      "地址 " + company.Companyaddress + "\n" +

                                                        //       "<a href='" + httphead + basic.Domain + "/h5/StoreDefault.aspx?menshiid=" + company.Id + "'> 请点击链接查看更多...</a>\n";

                                                        //    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + innerstr + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                                        //}
                                                        #endregion
                                                        Member_Channel_company c_company = new MemberChannelcompanyData().GetChannelCompany(sourcer.Channelcompanyid.ToString());
                                                        if (c_company != null)
                                                        {
                                                            string linkurl = httphead + basic.Domain + "/h5/StoreDefault.aspx?menshiid=" + c_company.Id + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            string content = "欢迎您访问我们的门店：<a href='" + linkurl + "'>" + c_company.Companyname + "</a>\n";
                                                            //string companyimgurl = FileSerivce.GetImgUrl(c_company.Companyimg);

                                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                        }
                                                        else
                                                        {
                                                            string linkurl = httphead + basic.Domain + "/h5/Storelist.aspx?uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            string content = "欢迎您访问<a href='" + linkurl + "'>我们的门店</a>\n";
                                                            //string companyimgurl = FileSerivce.GetImgUrl(c_company.Companyimg);

                                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                        #endregion


                                        ////判断扫描参数如果是 渠道，或门市二维码则执行给客户发送 顾问信息
                                        //if (sourcer.Channelcompanyid != 0 || sourcer.Channelid != 0)
                                        //{
                                        //    //自动重新分配渠道，或者读取现有渠道并发送客户通道
                                        //    channelid = CustomerMsg_Send.AutoFenpeiChannel(basic.Comid, requestXML.FromUserName, 1, sourcer.Channelcompanyid, sourcer.Channelid);
                                        //} 
                                    }
                                    #endregion
                                    #region 临时二维码
                                    else
                                    {
                                        //临时二维码生成规则(1000000+素材id)，所以素材id=临时二维码id-1000000
                                        int tempqrcodeid = wxsourceid;
                                        int materialid = tempqrcodeid - 1000000;

                                        string materialname = "";
                                        //根据materialid 得到文章信息
                                        if (materialid > 0)
                                        {
                                            WxMaterial msucai = new WxMaterialData().GetWxMaterial(materialid);
                                            if (msucai != null)
                                            {
                                                materialname = msucai.Title;
                                            }
                                        }

                                        string linkurl = "http://shop" + basic.Comid + ".etown.cn/h5/peoplevoiceup.aspx?comid=" + basic.Comid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass + "&clientuptypemark=" + (int)Clientuptypemark.DownMaterialVoice + "&materialid=" + materialid;


                                        string str = "点击 <a href='" + linkurl + "'>录制文章:" + materialname + " 介绍语音</a>\n";


                                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + str + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                    }
                                    #endregion
                                }
                                ////获得微信用户基本信息
                                //GetWeiXinCrmBasic(basic, requestXML.FromUserName);
                            }
                        }
                    }
                    #endregion
                    #region  上报地理位置事件
                    if (requestXML.Eevent == "LOCATION")
                    {
                        //#region 记录上报地理位置事件
                        //requestXML.Content = "上报地理位置事件";
                        //requestXML.ContentType = true;
                        //requestXML.Comid = basic.Comid;
                        //int inswxexchangemsg = new WxRequestXmlData().EditWxRequestXmlLog(requestXML);
                        //#endregion

                        if (requestXML.Latitude != "" && requestXML.Longitude != "")
                        {
                            SqlHelper sqlhelper = new SqlHelper();
                            sqlhelper.BeginTrancation();
                            try
                            {
                                SqlCommand cmd = new SqlCommand();

                                //会员位置表：判断是否保存过位置信息，a.保存过修改b.没有保存过添加
                                string sql1 = "select count(1) from b2b_crm_location  where weixin='" + requestXML.FromUserName + "' and comid=" + basic.Comid;
                                cmd = sqlhelper.PrepareTextSqlCommand(sql1);
                                object o = cmd.ExecuteScalar();
                                if (int.Parse(o.ToString()) > 0)
                                {
                                    string sql2 = "update b2b_crm_location set Latitude='" + requestXML.Latitude + "' ,Longitude='" + requestXML.Longitude + "' ,Precision='" + requestXML.Precision + "',createtime='" + requestXML.CreateTime + "',createtimeformat='" + requestXML.CreateTimeFormat + "'  where weixin='" + requestXML.FromUserName + "' and comid=" + basic.Comid;
                                    cmd = sqlhelper.PrepareTextSqlCommand(sql2);
                                    cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    if (basic.Comid > 0)
                                    {
                                        string sql3 = "insert into b2b_crm_location (weixin,createtime,Latitude,Longitude,Precision,comid,createtimeformat) values('" + requestXML.FromUserName + "','" + requestXML.CreateTime + "','" + requestXML.Latitude + "','" + requestXML.Longitude + "','" + requestXML.Precision + "','" + basic.Comid + "','" + requestXML.CreateTimeFormat + "')";
                                        cmd = sqlhelper.PrepareTextSqlCommand(sql3);
                                        cmd.ExecuteNonQuery();
                                    }
                                }

                                sqlhelper.Commit();
                            }
                            catch (Exception e)
                            {
                                sqlhelper.Rollback();
                            }
                            finally
                            {
                                sqlhelper.Dispose();
                            }
                        }
                    }
                    #endregion
                    #region 自定义菜单事件
                    if (requestXML.Eevent == "CLICK")
                    {

                        B2bCrmData userdate = new B2bCrmData();
                        //设定并获取微信随机码                              
                        var pass = userdate.WeixinGetPass(requestXML.FromUserName, basic.Comid);

                        //根据微信服务器返回内容判断是何种操作
                        int eventkey = (requestXML.EventKey).ConvertTo<int>(0);
                        if (eventkey != 0)
                        {
                            WxMenu menu = new WxMenuData().GetWxMenu(eventkey, basic.Comid);
                            if (menu != null)
                            {
                                #region 记录用户菜单查询
                                WxOperationType opertype = new WxOperationTypeData().GetOprationType(menu.Operationtypeid);
                                if (opertype != null)
                                {
                                    requestXML.Content = opertype.Typename;
                                    requestXML.ContentType = true;
                                    requestXML.Comid = basic.Comid;
                                    int inswxexchangemsg = new WxRequestXmlData().EditWxRequestXmlLog(requestXML);
                                }
                                #endregion

                                #region 回复文本信息
                                if (menu.Operationtypeid == 3)
                                {
                                    string anwsertext = menu.Wxanswertext.Replace("<p>", "").Replace("</p>", "\r\n").Replace("<br />", "\r\n").Replace("&ldquo;", "\"").Replace("&rdquo;", "\"").Replace("&nbsp;", " ");

                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + anwsertext + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                }
                                #endregion
                                #region 回复图文信息
                                if (menu.Operationtypeid == 4)
                                {
                                    //根据微信号获得会员信息
                                    B2b_crm crminfo = new B2bCrmData().GetB2bCrmByWeiXin(requestXML.FromUserName, basic.Comid);
                                    if (crminfo == null)//会员信息为空时，添加此会员信息
                                    {

                                        //resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[未查询到会员信息]]></Content><FuncFlag>1</FuncFlag></xml>";

                                        resxml = InsB2bCrm(basic, requestXML, "");
                                    }


                                    if (menu.pictexttype == 1)//素材图文信息
                                    {
                                        int totalcount = 0;
                                        IList<WxMaterial> materiallist = new List<WxMaterial>();
                                        //获得促销类型的最大期
                                        periodical periodicald = new WxMaterialData().GetPeriodicalBySaleType(basic.Comid, menu.SalePromoteTypeid);
                                        if (periodicald == null)
                                        {
                                            materiallist = new WxMaterialData().WxMaterialPageList(basic.Comid, 1, 9, 10, menu.SalePromoteTypeid, out totalcount);//暂时先显示前10条
                                        }
                                        else
                                        {
                                            materiallist = new WxMaterialData().GetWxMaterialByNewestPeriodical(1, 9, 10, menu.SalePromoteTypeid, periodicald.Id, basic.Comid, out totalcount);//暂时先显示前10条,加上期的限制

                                        }

                                        if (totalcount > 0)
                                        {
                                            //判断素材类型是否需要显示往期推荐
                                            int isshowpast = 0;//是否显示往期推荐
                                            WxSalePromoteType wxsaletype1 = new WxSalePromoteTypeData().GetMaterialType(menu.SalePromoteTypeid, basic.Comid);
                                            if (wxsaletype1 != null)
                                            {
                                                if (wxsaletype1.Isshowpast == true)
                                                {
                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>" + (materiallist.Count + 1) + "</ArticleCount><Articles>";
                                                    isshowpast = 1;
                                                }
                                                else
                                                {
                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>" + materiallist.Count + "</ArticleCount><Articles>";

                                                }
                                            }

                                            WxSalePromoteType protypemodel = new WxSalePromoteTypeData().GetMaterialType(menu.SalePromoteTypeid);
                                            for (int ii = 0; ii < materiallist.Count; ii++)
                                            {

                                                if (ii == 0)
                                                {
                                                    //if (protypemodel.Typeclass == "book")//book 显示预订页面
                                                    //{
                                                    //    resxml += "<item><Title><![CDATA[" + protypemodel.Typename + " " + periodicald.Peryear + "年第" + periodicald.Percal + "期: " + materiallist[ii].Title + "]]></Title><Description><![CDATA[" + materiallist[ii].Summary + "]]></Description><PicUrl><![CDATA[" + httphead + basic.Domain + AppSettings.CommonSetting.GetValue("FileUpload/FileUrl").ConvertTo<string>() + new FileUploadData().GetFileById(materiallist[ii].Imgpath.ConvertTo<int>(0)).Relativepath + "]]></PicUrl><Url><![CDATA[" + httphead + basic.Domain + materialdetailurl + materiallist[ii].MaterialId + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass + "]]></Url></item>";

                                                    //}
                                                    //else//detail 只是显示详情
                                                    //{
                                                    if (isshowpast == 1)
                                                    {
                                                        resxml += "<item><Title><![CDATA[" + protypemodel.Typename + " " + periodicald.Peryear + "年第" + periodicald.Percal + "期: " + materiallist[ii].Title + "]]></Title><Description><![CDATA[" + materiallist[ii].Summary + "]]></Description><PicUrl><![CDATA[" + FileSerivce.GetImgUrl(materiallist[ii].Imgpath.ConvertTo<int>(0)) + "]]></PicUrl><Url><![CDATA[" + httphead + basic.Domain + aboutmaterialdetailurl + materiallist[ii].MaterialId + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass + "]]></Url></item>";
                                                    }
                                                    else
                                                    {
                                                        resxml += "<item><Title><![CDATA[" + materiallist[ii].Title + "]]></Title><Description><![CDATA[" + materiallist[ii].Summary + "]]></Description><PicUrl><![CDATA[" + FileSerivce.GetImgUrl(materiallist[ii].Imgpath.ConvertTo<int>(0)) + "]]></PicUrl><Url><![CDATA[" + httphead + basic.Domain + aboutmaterialdetailurl + materiallist[ii].MaterialId + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass + "]]></Url></item>";
                                                    }
                                                    //}
                                                }
                                                else
                                                {
                                                    //if (protypemodel.Typeclass == "book")//book 显示预订页面
                                                    //{
                                                    //    resxml += "<item><Title><![CDATA[" + materiallist[ii].Title + "]]></Title><Description><![CDATA[" + materiallist[ii].Summary + "]]></Description><PicUrl><![CDATA[" + httphead + basic.Domain + AppSettings.CommonSetting.GetValue("FileUpload/FileUrl").ConvertTo<string>() + new FileUploadData().GetFileById(materiallist[ii].Imgpath.ConvertTo<int>(0)).Relativepath + "]]></PicUrl><Url><![CDATA[" + httphead + basic.Domain + materialdetailurl + materiallist[ii].MaterialId + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass + "]]></Url></item>";

                                                    //}
                                                    //else//detail 只是显示详情
                                                    //{

                                                    resxml += "<item><Title><![CDATA[" + materiallist[ii].Title + "]]></Title><Description><![CDATA[" + materiallist[ii].Summary + "]]></Description><PicUrl><![CDATA[" + FileSerivce.GetImgUrl(materiallist[ii].Imgpath.ConvertTo<int>(0)) + "]]></PicUrl><Url><![CDATA[" + httphead + basic.Domain + aboutmaterialdetailurl + materiallist[ii].MaterialId + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass + "]]></Url></item>";

                                                    //}
                                                }

                                            }
                                            if (wxsaletype1 != null)
                                            {
                                                if (wxsaletype1.Isshowpast == true)
                                                {
                                                    resxml += "<item><Title><![CDATA[" + protypemodel.Typename + " >> 往期产品推荐]]></Title><Description><![CDATA[]]></Description><PicUrl><![CDATA[]]></PicUrl><Url><![CDATA[" + httphead + basic.Domain + "/M/period.aspx?id=" + periodicald.Id + "&type=" + protypemodel.Id + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass + "]]></Url></item>";
                                                }
                                            }

                                            resxml += "</Articles></xml>";
                                        }
                                    }
                                    if (menu.pictexttype == 2)//单类产品图文信息
                                    {

                                        int totalcount = 0;
                                        List<B2b_com_pro> list = new B2bComProData().Selectpagelist(basic.Comid.ToString(), 1, 9, "", out totalcount, 0, menu.Product_class);
                                        if (list != null)
                                        {
                                            if (totalcount > 0)
                                            {

                                                int youxiaonum = 0;
                                                string youxiaoxml = "";
                                                for (int ii = 0; ii < list.Count; ii++)
                                                {

                                                    ////放弃判断产品是否有效，因为需要用户进入产品页面进入过期订单的回滚：1.票务，直接判断有效期 和产品上线状态2.酒店，跟团游，当地游 则判断是否含有有效的房态/团期 以及产品上下线状态
                                                    //int IsYouXiao = new B2bComProData().IsYouxiao(list[ii].Id, list[ii].Server_type, list[ii].Pro_start, list[ii].Pro_end, list[ii].Pro_state);
                                                    int IsYouXiao = 1;
                                                    if (IsYouXiao == 1)
                                                    {
                                                        youxiaonum++;

                                                        string linkurl = "";//链接地址
                                                        //根据服务类型判断链接地址
                                                        if (list[ii].Server_type == 1)//电子凭证(票务)
                                                        {
                                                            linkurl = httphead + basic.Domain + "/h5/orderenter.aspx?id=" + list[ii].Id + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                        }
                                                        else if (list[ii].Server_type == 2 || list[ii].Server_type == 8)//2跟团游;8当地游
                                                        {
                                                            linkurl = httphead + basic.Domain + "/h5/linedetail.aspx?lineid=" + list[ii].Id + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;

                                                        }
                                                        else if (list[ii].Server_type == 9)//酒店客房
                                                        {
                                                            linkurl = httphead + basic.Domain + "/h5/hotel/hotelshow.aspx?projectid=" + list[ii].Projectid + "&id=" + basic.Comid + "&proid=" + list[ii].Id + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                        }
                                                        else if (list[ii].Server_type == 10)//旅游大巴
                                                        {
                                                            linkurl = httphead + basic.Domain + "/h5/orderenter.aspx?id=" + list[ii].Id + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                        }
                                                        else
                                                        {
                                                            linkurl = "";
                                                        }
                                                        youxiaoxml += "<item><Title><![CDATA[" + list[ii].Pro_name + "]]></Title><Description><![CDATA[]]></Description><PicUrl><![CDATA[" + FileSerivce.GetImgUrl(list[ii].Imgurl) + "]]></PicUrl><Url><![CDATA[" + linkurl + "]]></Url></item>";
                                                    }
                                                }
                                                if (youxiaonum > 0)
                                                {
                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>" + (list.Count + 1) + "</ArticleCount><Articles>";
                                                    resxml += youxiaoxml;
                                                    resxml += "<item><Title><![CDATA[更多产品]]></Title><Description><![CDATA[]]></Description><PicUrl><![CDATA[]]></PicUrl><Url><![CDATA[" + httphead + basic.Domain + "/h5/Orderlist.aspx?proclass=" + menu.Product_class + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass + "]]></Url></item>";
                                                    resxml += "</Articles></xml>";
                                                }
                                                else
                                                {
                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[暂无活动，敬请期待]]></Content><FuncFlag>1</FuncFlag></xml>";

                                                }
                                            }
                                            else
                                            {
                                                resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[暂无活动，敬请期待]]></Content><FuncFlag>1</FuncFlag></xml>";

                                            }
                                        }
                                        else
                                        {
                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[暂无活动，敬请期待]]></Content><FuncFlag>1</FuncFlag></xml>";
                                        }
                                    }
                                }
                                #endregion
                                #region 回复会员信息
                                if (menu.Operationtypeid == 5)//回复会员信息
                                {

                                    //根据微信号获得会员信息
                                    B2b_crm crminfo = new B2bCrmData().GetB2bCrmByWeiXin(requestXML.FromUserName);



                                    string binded = "尊敬的会员：" + crminfo.Name + "您好！\n\n" +

                                                    "您的会员卡卡号：" + crminfo.Idcard + "\n";
                                    if (crminfo.Phone.ConvertTo<string>("") == "")
                                    {
                                        binded += "您的绑定手机号:\n(请在微信留言中回复你的手机号，接收验证码后即可将该手机绑定到本会员账户)\n";
                                    }
                                    else
                                    {
                                        binded += "您的绑定手机号：" + crminfo.Phone + "\n";
                                    }
                                    binded += "您的姓名：" + crminfo.Name + "\n\n";
                                    if (crminfo.Imprest.ToString().ConvertTo<int>(0) > 0)
                                    {
                                        binded += "你的预付款:" + crminfo.Imprest;
                                    }
                                    if (crminfo.Integral.ToString().ConvertTo<int>(0) > 0)
                                    {
                                        binded += "你的积分:" + crminfo.Integral;
                                    }

                                    // "如果您需要修改上述信息，请登录微旅行网页版操作；\n\n" +

                                    binded += "点击查看  <a href='" + httphead + basic.Domain + "/m/indexcard.aspx?openid=" + requestXML.FromUserName + "&weixinpass=" + pass + "'>会员卡服务专区</a> \n\n" +
                                     "以上信息为您个人的会员信息请不要转发；\n\n";
                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + binded + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                }
                                #endregion
                                #region 回复优惠劵信息
                                if (menu.Operationtypeid == 6)//回复优惠劵信息
                                {
                                    string def = "你的会员账户中的优惠券信息，<a href='" + httphead + basic.Domain + "/m/Default.aspx?openid=" + requestXML.FromUserName + "&weixinpass=" + pass + "'>请点击查看</a>";
                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + def + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                }
                                #endregion
                                #region 在线票务预订,进入项目列表页面
                                if (menu.Operationtypeid == 8)
                                {
                                    //根据微信号获得会员信息
                                    B2b_crm crminfo = new B2bCrmData().GetB2bCrmByWeiXin(requestXML.FromUserName);


                                    string linkurl1 = "";
                                    //微旅行和易城 进入商家列表页面；其他商家进入商家产品列表页面
                                    //if (basic.Comid == 101 || basic.Comid == 106)
                                    //{
                                    //    linkurl1 = httphead + basic.Domain + "/h5/list.aspx?openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                    //}
                                    //else
                                    //{
                                    linkurl1 = httphead + basic.Domain + "/h5/list.aspx?openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                    //}

                                    string binded = "\n\n<a href='" + linkurl1 + "'>感谢您使用 在线预订 功能       请点击进入...</a> \n\n" +

                                                    "当您完成购买后将获得包含电子凭证数字码的短信。\n\n" +

                                                    "同时，您可在 微信菜单 会员卡 》我的电子凭证 中查看购买成功的产品。 \n\n";

                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + binded + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                }
                                #endregion
                                #region 在线酒店预订，微旅行和易城 进入商家列表页面；其他商家进入商家产品列表页面
                                if (menu.Operationtypeid == 11)
                                {
                                    //根据微信号获得会员信息
                                    B2b_crm crminfo = new B2bCrmData().GetB2bCrmByWeiXin(requestXML.FromUserName);

                                    string linkurl1 = "";
                                    //微旅行和易城 进入商家列表页面；其他商家进入商家产品列表页面
                                    if (basic.Comid == 101 || basic.Comid == 106)
                                    {
                                        linkurl1 = httphead + basic.Domain + "/h5/hotel/hotellist.aspx?openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                    }
                                    else
                                    {
                                        linkurl1 = httphead + basic.Domain + "/h5/hotel/Hotelshow.aspx?openid=" + requestXML.FromUserName + "&weixinpass=" + pass + "&id=" + basic.Comid;
                                    }

                                    string binded = "\n\n<a href='" + linkurl1 + "'>感谢您使用 微信酒店预订 功能       请点击进入...</a> \n\n";



                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + binded + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                }
                                #endregion
                                #region 关联手机
                                if (menu.Operationtypeid == 9)//关联手机
                                {
                                    string binded = "尊敬的会员，请在微信留言中 输入您的手机号码，将微信与你手机关联。\n\n" +

                                                    "微信关联手机号的优势：\n\n" +

                                                    "1、在微信上查看会员专享优惠及消费积分情况等；\n" +
                                                    "2、在微信上查看您的电子凭证信息（二维码及使用状态等）；\n" +
                                                    "3、可获得更及时有效的正式会员尊享服务。\n\n";

                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + binded + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                }
                                #endregion
                                #region 回复电子凭证信息
                                if (menu.Operationtypeid == 10)//回复电子凭证信息
                                {


                                    string def = "你的会员账户中的电子凭证信息，<a href='" + httphead + basic.Domain + "/M/eticketlist.aspx?openid=" + requestXML.FromUserName + "&weixinpass=" + pass + "'>请点击查看</a>";
                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + def + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                }
                                #endregion
                                #region 回复大抽奖
                                if (menu.Operationtypeid == 12)// 回复大抽奖
                                {

                                    int actid = MemberERNIEData.ERNIETOPgetid(basic.Comid);
                                    string def = "";
                                    if (actid != 0)
                                    {
                                        def = "请点击进入 <a href='" + httphead + basic.Domain + "/m/Choujiang/?id=" + actid + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass + "'>会员抽奖活动</a>";
                                    }
                                    else
                                    {
                                        def = "还没有抽奖活动，敬请关注！";

                                    }

                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + def + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                }
                                #endregion
                                #region 我要咨询(回复渠道单位信息)
                                if (menu.Operationtypeid == 13)
                                {
                                    resxml = SelectMenShiByOpenId(requestXML, basic);
                                }
                                #endregion
                                #region 所属门店
                                if (menu.Operationtypeid == 14)
                                {
                                    string linkurl1 = httphead + basic.Domain + "/h5/storelist.aspx?issuetype=0&openid=" + requestXML.FromUserName;
                                    string binded = "\n\n<a href='" + linkurl1 + "'>请点击查看所属门店信息</a>";

                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + binded + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                }
                                #endregion
                                #region 合作公司列表
                                if (menu.Operationtypeid == 15)
                                {
                                    string linkurl1 = httphead + basic.Domain + "/h5/storelist.aspx?issuetype=1&openid=" + requestXML.FromUserName;
                                    string binded = "\n\n<a href='" + linkurl1 + "'>请点击查看合作公司信息</a>";

                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + binded + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                }
                                #endregion
                                #region 微网站
                                if (menu.Operationtypeid == 16)
                                {
                                    //根据微信号获得会员信息
                                    B2b_crm crminfo = new B2bCrmData().GetB2bCrmByWeiXin(requestXML.FromUserName);

                                    string linkurl1 = httphead + basic.Domain + "/h5/default.aspx?openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                    string binded = "\n\n<a href='" + linkurl1 + "'>请点击查看微网站</a>\n";

                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + binded + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                }
                                #endregion
                                #region 门市位置坐标
                                if (menu.Operationtypeid == 17)
                                {
                                    //根据微信号获得会员信息
                                    B2b_crm crminfo = new B2bCrmData().GetB2bCrmByWeiXin(requestXML.FromUserName);

                                    string linkurl1 = httphead + basic.Domain + "/h5/mapinfo.aspx?openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                    string binded = "\n\n<a href='" + linkurl1 + "'>请点击查看位置导航</a>\n";

                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + binded + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                }
                                #endregion
                                #region 产品分类(回复图文)
                                if (menu.Operationtypeid == 18)
                                {
                                    //根据微信号获得会员信息
                                    B2b_crm crminfo = new B2bCrmData().GetB2bCrmByWeiXin(requestXML.FromUserName, basic.Comid);
                                    if (crminfo == null)//会员信息为空时，添加此会员信息
                                    {
                                        resxml = InsB2bCrm(basic, requestXML, "");
                                    }

                                    int totalcount = 0;
                                    List<B2b_com_pro> list = new B2bComProData().Selectpagelist(basic.Comid.ToString(), 1, 9, "", out totalcount, 0, menu.Product_class);
                                    if (list != null)
                                    {
                                        if (totalcount > 0)
                                        {
                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>" + (list.Count + 1) + "</ArticleCount><Articles>";
                                            for (int ii = 0; ii < list.Count; ii++)
                                            {
                                                string linkurl = "";//链接地址
                                                //根据服务类型判断链接地址
                                                if (list[ii].Server_type == 1)//电子凭证(票务)
                                                {
                                                    linkurl = httphead + basic.Domain + "/h5/orderenter.aspx?id=" + list[ii].Id + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                }
                                                else if (list[ii].Server_type == 2 || list[ii].Server_type == 8)//2跟团游;8当地游
                                                {
                                                    linkurl = httphead + basic.Domain + "/h5/linedetail.aspx?lineid=" + list[ii].Id + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;

                                                }
                                                else if (list[ii].Server_type == 9)//酒店客房
                                                {
                                                    linkurl = httphead + basic.Domain + "/h5/hotel/hotelshow.aspx?projectid=" + list[ii].Projectid + "&id=" + basic.Comid + "&proid=" + list[ii].Id + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                }
                                                else
                                                {
                                                    linkurl = "";
                                                }
                                                resxml += "<item><Title><![CDATA[" + list[ii].Pro_name + "]]></Title><Description><![CDATA[]]></Description><PicUrl><![CDATA[" + FileSerivce.GetImgUrl(list[ii].Imgurl) + "]]></PicUrl><Url><![CDATA[" + linkurl + "]]></Url></item>";
                                            }

                                            resxml += "<item><Title><![CDATA[更多产品]]></Title><Description><![CDATA[]]></Description><PicUrl><![CDATA[]]></PicUrl><Url><![CDATA[" + httphead + basic.Domain + "/h5/Orderlist.aspx?proclass=" + menu.Product_class + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass + "]]></Url></item>";
                                            resxml += "</Articles></xml>";
                                        }
                                        else
                                        {
                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[暂无活动，敬请期待]]></Content><FuncFlag>1</FuncFlag></xml>";

                                        }
                                    }
                                    else
                                    {
                                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[暂无活动，敬请期待]]></Content><FuncFlag>1</FuncFlag></xml>";
                                    }


                                }
                                #endregion
                                #region 景区门票
                                if (menu.Operationtypeid == 19)
                                {
                                    //根据微信号获得会员信息
                                    B2b_crm crminfo = new B2bCrmData().GetB2bCrmByWeiXin(requestXML.FromUserName);

                                    string linkurl1 = httphead + basic.Domain + "/h5/prodefault.aspx?openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                    string binded = "\n\n<a href='" + linkurl1 + "'>请点击查看景区门票</a>\n";

                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + binded + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                }
                                #endregion
                                #region 抢购
                                if (menu.Operationtypeid == 20)
                                {
                                    //根据微信号获得会员信息
                                    B2b_crm crminfo = new B2bCrmData().GetB2bCrmByWeiXin(requestXML.FromUserName);

                                    string linkurl1 = httphead + basic.Domain + "/h5/qianggou.aspx?openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                    string binded = "\n\n<a href='" + linkurl1 + "'>请点击查看抢购产品</a>\n";

                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + binded + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                }
                                #endregion
                                #region   产品分类(链接页面)
                                if (menu.Operationtypeid == 21)
                                {
                                    //根据微信号获得会员信息
                                    B2b_crm crminfo = new B2bCrmData().GetB2bCrmByWeiXin(requestXML.FromUserName);

                                    B2b_com_class model_class = new B2bComProData().GetB2bcomclass(menu.Product_class);
                                    if (model_class != null)
                                    {
                                        string linkurl1 = menu.Linkurl;
                                        string binded = "\n\n<a href='" + linkurl1 + "'>请点击查看" + model_class.Classname + "产品</a>\n";

                                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + binded + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                    }

                                }
                                #endregion
                                #region   关键词(链接页面)
                                if (menu.Operationtypeid == 22)
                                {
                                    //根据微信号获得会员信息
                                    B2b_crm crminfo = new B2bCrmData().GetB2bCrmByWeiXin(requestXML.FromUserName);


                                    string linkurl1 = menu.Linkurl;
                                    string binded = "\n\n<a href='" + linkurl1 + "'>请点击查看" + menu.Keyy + "产品</a>\n";

                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + binded + "]]></Content><FuncFlag>1</FuncFlag></xml>";



                                }
                                #endregion
                                #region   我的服务顾问(回复链接)
                                if (menu.Operationtypeid == 27)
                                {
                                    //服务顾问列表 title 文字用 我的服务顾问 子菜单的名称文字。包括客服通道中的 “服务顾问”文字替换。
                                    WxMenu menur = new WxMenuData().GetWxMenuByOperType(27, basic.Comid);
                                    string title = "服务顾问";
                                    if (menur != null)
                                    {
                                        title = menur.Name;
                                    }

                                    string binded = "请点击进入 <a href='http://shop" + basic.Comid + ".etown.cn/weixin/skippage.aspx?openid=" + requestXML.FromUserName + "&weixinpass=" + pass + "'>" + title + "</a> 页面\n";

                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + binded + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                }
                                #endregion
                                #region   我的服务顾问(回复文本)
                                if (menu.Operationtypeid == 28)
                                {
                                    //页面请求时发送给客户顾问信息
                                    CustomerMsg_Send.AutoFenpeiChannel(basic.Comid, requestXML.FromUserName);
                                }
                                #endregion
                                #region   微商城
                                if (menu.Operationtypeid == 29)
                                {
                                    string binded = "请点击进入 <a href='http://shop" + basic.Comid + ".etown.cn/h5/order/default.aspx?openid=" + requestXML.FromUserName + "&weixinpass=" + pass + "'>微商城</a> 页面\n";

                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + binded + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                }
                                #endregion
                                #region   微分销
                                if (menu.Operationtypeid == 30)
                                {
                                    string binded = "请点击进入 <a href='http://shop" + basic.Comid + ".etown.cn/agent/m/login.aspx?openid=" + requestXML.FromUserName + "&weixinpass=" + pass + "'>微分销</a> 页面\n";

                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + binded + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                }
                                #endregion
                                #region   电子凭证 和以上的重复了，去掉
                                if (menu.Operationtypeid == 31)
                                {
                                    string binded = "请点击进入 <a href='http://shop" + basic.Comid + ".etown.cn/agent/m/EticketList.aspx?openid=" + requestXML.FromUserName + "&weixinpass=" + pass + "'>电子凭证</a> 页面\n";

                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + binded + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                }
                                #endregion
                            }
                        }
                    }
                    #endregion
                    #region 扫码推事件的事件推送
                    if (requestXML.Eevent == "scancode_push")
                    {
                        lock (lockobj)
                        {
                            //判断是否是并发请求
                            string cretime = requestXML.CreateTime;
                            //获得请求发送的次数
                            int reqnum = new WxSubscribeDetailData().GetReqnum(requestXML.FromUserName, cretime, "scancode_push");
                            if (reqnum == 0)
                            {
                                #region 记录扫码推事件的事件推送
                                requestXML.Content = "扫码内容:" + requestXML.ScanResult;
                                requestXML.ContentType = true;
                                requestXML.Comid = basic.Comid;
                                int inswxexchangemsg = new WxRequestXmlData().EditWxRequestXmlLog(requestXML);
                                #endregion

                                //用户扫描带参二维码，被动回复消息
                                int wxsourceid = requestXML.EventKey.Substring(requestXML.EventKey.IndexOf("_") + 1).ConvertTo<int>(0);
                                if (wxsourceid > 0)
                                {
                                    WxSubscribeSource sourcer = new WxSubscribeSourceData().GetWXSourceById(wxsourceid);
                                    if (sourcer != null)
                                    {
                                        //设定并获取微信随机码                              
                                        var pass = new B2bCrmData().WeixinGetPass(requestXML.FromUserName, basic.Comid);

                                        //根据微信号获得会员信息
                                        B2b_crm crminfo = new B2bCrmData().GetB2bCrmByWeiXin(requestXML.FromUserName, basic.Comid);
                                        if (crminfo == null)//会员信息为空时，添加此会员信息
                                        {
                                            resxml = InsB2bCrm(basic, requestXML, "");
                                        }

                                        #region  绑定有渠道的二维码
                                        if (sourcer.qrcodeviewtype > 0)//绑定有渠道的二维码
                                        {
                                            #region 1营销活动
                                            if (sourcer.qrcodeviewtype == 1)
                                            {
                                                Member_Activity m = MemberActivityData.GetActById(sourcer.Activityid);
                                                if (m != null)
                                                {
                                                    string content = m.Title + "\n" + m.Actstar.ToString("yyyy-MM-dd") + "-" + m.Actend.ToString("yyyy-MM-dd");

                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                }
                                            }
                                            #endregion
                                            #region 2产品图文
                                            else if (sourcer.qrcodeviewtype == 2)
                                            {
                                                #region 显示项目下产品
                                                if (sourcer.Productid == 0)//显示项目下产品
                                                {
                                                    int projectid = sourcer.projectid;
                                                    if (projectid > 0)
                                                    {
                                                        string servertypes = "1,2,8,9,11,12";//显示的服务类型:电子票；跟团游；当地游;酒店；实物
                                                        int topnums = 10;//显示数量
                                                        IList<B2b_com_pro> prolist = new B2bComProData().GetProlistbyprojectid(projectid, servertypes, topnums);
                                                        if (prolist.Count > 0)
                                                        {
                                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>" + prolist.Count + "</ArticleCount><Articles>";
                                                            foreach (B2b_com_pro pro in prolist)
                                                            {
                                                                string proname = pro.Pro_name;
                                                                string proimgurl = FileSerivce.GetImgUrl(pro.Imgurl);
                                                                string linkurl = "";

                                                                if (pro.Server_type == 1 || pro.Server_type == 12)//票务 或者 预约产品
                                                                {
                                                                    linkurl = httphead + basic.Domain + "/h5/order/pro.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                                }
                                                                else if (pro.Server_type == 2 || pro.Server_type == 8)//旅游
                                                                {
                                                                    linkurl = httphead + basic.Domain + "/h5/linedetail.aspx?lineid=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                                }
                                                                else if (pro.Server_type == 9)//酒店客房
                                                                {
                                                                    linkurl = httphead + basic.Domain + "/h5/hotel/hotelshow.aspx?proid=" + pro.Id + "&id=" + basic.Comid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                                }
                                                                else if (pro.Server_type == 11)//实物
                                                                {
                                                                    linkurl = httphead + basic.Domain + "/h5/order/pro.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                                }
                                                                else
                                                                {
                                                                    linkurl = "";
                                                                }

                                                                string adviseprice = "￥" + pro.Advise_price.ToString("F2");

                                                                resxml += "<item><Title><![CDATA[" + pro.Pro_name + "]]></Title><Description><![CDATA[" + adviseprice + "]]></Description><PicUrl><![CDATA[" + proimgurl + "]]></PicUrl><Url><![CDATA[" + linkurl + "]]></Url></item>";
                                                            }
                                                            resxml += "</Articles></xml>";
                                                        }
                                                    }
                                                }
                                                #endregion
                                                #region 显示特定产品
                                                else //显示特定产品
                                                {
                                                    //根据产品id得到产品信息
                                                    B2b_com_pro pro = new B2bComProData().GetProById(sourcer.Productid.ToString());
                                                    if (pro != null)
                                                    {
                                                        if (pro.Server_type != 3)
                                                        {
                                                            string proname = pro.Pro_name;
                                                            string proimgurl = FileSerivce.GetImgUrl(pro.Imgurl);
                                                            string linkurl = "";
                                                            if (pro.Server_type == 1 || pro.Server_type == 12)//票务 或者 预约产品
                                                            {
                                                                linkurl = httphead + basic.Domain + "/h5/order/pro.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            }
                                                            else if (pro.Server_type == 2 || pro.Server_type == 8)//旅游
                                                            {
                                                                linkurl = httphead + basic.Domain + "/h5/linedetail.aspx?lineid=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            }
                                                            else if (pro.Server_type == 9)//酒店客房
                                                            {
                                                                linkurl = httphead + basic.Domain + "/h5/hotel/hotelshow.aspx?proid=" + pro.Id + "&id=" + basic.Comid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            }
                                                            else if (pro.Server_type == 11)//实物
                                                            {
                                                                linkurl = httphead + basic.Domain + "/h5/order/pro.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            }
                                                            else
                                                            {
                                                                linkurl = "";
                                                            }

                                                            string adviseprice = "￥" + pro.Advise_price.ToString("F2");
                                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount>" +
                                                            "<Articles><item><Title><![CDATA[" + proname + "]]></Title><Description><![CDATA[" + adviseprice + "]]></Description><PicUrl><![CDATA[" + proimgurl + "]]></PicUrl><Url><![CDATA[" + linkurl + "]]></Url></item></Articles></xml>";
                                                        }
                                                        else
                                                        {
                                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                            #endregion
                                            #region 3分销商，暂时 没东西
                                            else if (sourcer.qrcodeviewtype == 3)
                                            {

                                            }
                                            #endregion
                                            #region  4文章图文
                                            else if (sourcer.qrcodeviewtype == 4)
                                            {
                                                #region 文章类型下文章列表
                                                if (sourcer.Wxmaterialid == 0)
                                                {
                                                    if (sourcer.wxmaterialtypeid > 0)
                                                    {
                                                        int topnums = 10;//显示数量
                                                        IList<WxMaterial> wxMateriallist = new WxMaterialData().GetWxMateriallistbytypeid(sourcer.wxmaterialtypeid, topnums);
                                                        if (wxMateriallist.Count > 0)
                                                        {
                                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>" + wxMateriallist.Count + "</ArticleCount><Articles>";
                                                            foreach (WxMaterial m_wxmaterial in wxMateriallist)
                                                            {
                                                                string materialname = m_wxmaterial.Title;
                                                                string materialimgurl = FileSerivce.GetImgUrl(m_wxmaterial.Imgpath.ConvertTo<int>(0));
                                                                string linkurl = httphead + basic.Domain + aboutmaterialdetailurl + m_wxmaterial.MaterialId + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;


                                                                resxml += "<item><Title><![CDATA[" + materialname + "]]></Title><Description><![CDATA[]]></Description><PicUrl><![CDATA[" + materialimgurl + "]]></PicUrl><Url><![CDATA[" + linkurl + "]]></Url></item>";
                                                            }
                                                            resxml += "</Articles></xml>";
                                                        }
                                                    }
                                                }
                                                #endregion
                                                #region 单个文章
                                                else
                                                {
                                                    //根据素材id得到素材信息
                                                    WxMaterial m_wxmaterial = new WxMaterialData().GetWxMaterial(sourcer.Wxmaterialid);
                                                    if (m_wxmaterial != null)
                                                    {
                                                        string materialname = m_wxmaterial.Title;
                                                        string materialimgurl = FileSerivce.GetImgUrl(m_wxmaterial.Imgpath.ConvertTo<int>(0));
                                                        string linkurl = httphead + basic.Domain + aboutmaterialdetailurl + m_wxmaterial.MaterialId + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;

                                                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount>" +
                                                        "<Articles><item><Title><![CDATA[" + materialname + "]]></Title><Description><![CDATA[]]></Description><PicUrl><![CDATA[" + materialimgurl + "]]></PicUrl><Url><![CDATA[" + linkurl + "]]></Url></item></Articles></xml>";
                                                    }
                                                }
                                                #endregion
                                            }
                                            #endregion
                                            #region 5微商城
                                            else if (sourcer.qrcodeviewtype == 5)
                                            {
                                                B2b_company mcompany = new B2bCompanyData().GetMicromallByComid(basic.Comid);
                                                if (mcompany != null)
                                                {
                                                    string proimgurl = "";
                                                    B2b_company_saleset saleset = B2bCompanySaleSetData.GetDirectSellByComid(basic.Comid.ToString());
                                                    if(saleset!=null)
                                                    {
                                                        proimgurl = FileSerivce.GetImgUrl(saleset.Smalllogo.ConvertTo<int>(0));
                                                    }
                                                    string linkurl = httphead + basic.Domain + "/h5/order/Default.aspx?uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                    //string content = "欢迎您访问我们的 <a href='" + linkurl + "'>微信小店</a>\n，优惠预定或查看推荐文章"; 
                                                    //resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount>" +
                                                          "<Articles><item><Title><![CDATA[" + mcompany.Com_name + "]]></Title><Description><![CDATA[]]></Description><PicUrl><![CDATA[" + proimgurl + "]]></PicUrl><Url><![CDATA[" + linkurl + "]]></Url></item></Articles></xml>";
                                             
                                                }
                                            }
                                            #endregion
                                            #region 6门店
                                            else if (sourcer.qrcodeviewtype == 6)
                                            {
                                                Member_Channel_company c_company = new MemberChannelcompanyData().GetChannelCompany(sourcer.viewchannelcompanyid.ToString());
                                                if (c_company != null)
                                                {
                                                    string linkurl = httphead + basic.Domain + "/h5/StoreDefault.aspx?menshiid=" + c_company.Id + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                    string content = "欢迎您访问我们的门店：<a href='" + linkurl + "'>" + c_company.Companyname + "</a>\n";
                                                    //string companyimgurl = FileSerivce.GetImgUrl(c_company.Companyimg);

                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                }
                                                if (sourcer.viewchannelcompanyid == 0)
                                                {
                                                    string linkurl = httphead + basic.Domain + "/h5/Storelist.aspx?uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                    string content = "欢迎您访问<a href='" + linkurl + "'>我们的门店</a>\n";
                                                    //string companyimgurl = FileSerivce.GetImgUrl(c_company.Companyimg);

                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                }
                                            }
                                            #endregion
                                            #region 7合作单位
                                            else if (sourcer.qrcodeviewtype == 7)
                                            {
                                                Member_Channel_company c_company = new MemberChannelcompanyData().GetChannelCompany(sourcer.viewchannelcompanyid.ToString());
                                                if (c_company != null)
                                                {
                                                    string linkurl = httphead + basic.Domain + "/h5/StoreDefault.aspx?menshiid=" + c_company.Id + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                    string content = "欢迎您访问我们的门店：<a href='" + linkurl + "'>" + c_company.Companyname + "</a>\n";
                                                    //string companyimgurl = FileSerivce.GetImgUrl(c_company.Companyimg);

                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                }
                                                if (sourcer.viewchannelcompanyid == 0)
                                                {
                                                    string linkurl = httphead + basic.Domain + "/h5/Storelist.aspx?uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                    string content = "欢迎您访问<a href='" + linkurl + "'>我们的门店</a>\n";
                                                    //string companyimgurl = FileSerivce.GetImgUrl(c_company.Companyimg);

                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                }
                                            }
                                            #endregion
                                            #region 8顾问
                                            else if (sourcer.qrcodeviewtype == 8)
                                            {
                                                Member_Channel m_Channel = new MemberChannelData().GetChannelDetail(sourcer.Channelid);
                                                if (m_Channel.Whetherdefaultchannel == 1)
                                                {
                                                    string linkurl = "http://shop" + basic.Comid + ".etown.cn/h5/Peoplelist.aspx?uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;

                                                    string str = "点击查看 <a href='" + linkurl + "'>服务顾问列表</a>\n";

                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + str + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                                }
                                                else
                                                {
                                                    string name = m_Channel.Name;

                                                    //根据顾问信息中的电话得到 员工信息
                                                    B2b_company_manageuser u = new B2bCompanyManagerUserData().GetCompanyUserByPhone(m_Channel.Mobile, m_Channel.Com_id);

                                                    string linkurl = "";
                                                    if (u != null)
                                                    {
                                                        linkurl = "http://shop" + basic.Comid + ".etown.cn/h5/People.aspx?MasterId=" + u.Id + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                    }

                                                    string str = "点击联系 服务顾问：<a href='" + linkurl + "'>" + name + "</a>\n";

                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + str + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                                }
                                            }
                                            #endregion
                                            #region 9公司预订产品验证二维码
                                            else if (sourcer.qrcodeviewtype == 9)
                                            {
                                                string linkurl = "http://shop" + basic.Comid + ".etown.cn/h5/order/yuding_orderlist.aspx?uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;


                                                string str = "查看您的 <a href='" + linkurl + "'>预约产品订单信息</a>\n";

                                                int ordercount = new B2bOrderData().CountOrderServertype12(requestXML.FromUserName);

                                                if (ordercount == 0)
                                                {
                                                    str = "您没有预约产品订单\n";
                                                }

                                                resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + str + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                            }
                                            #endregion

                                            #region 判断顾问（渠道人）是否变更了，变更的话切换顾问（渠道人）；并且显示变更后的顾问（渠道人）信息-如果变更后为默认渠道人不显示
                                            int nowchannelid = sourcer.Channelid;
                                            if (nowchannelid > 0 && nowchannelid != channelid)
                                            {
                                                var crmmodel = new B2bCrmData().GetB2bCrm(requestXML.FromUserName, basic.Comid);
                                                if (crmmodel != null)
                                                {
                                                    //变更顾问(渠道人) 
                                                    int upchannel = new MemberCardData().upCardcodeChannel(crmmodel.Idcard.ToString(), nowchannelid);
                                                    if (upchannel > 0)
                                                    {
                                                        //如果顾问(渠道人)不是默认渠道人,则发送顾问信息     
                                                        Member_Channel nowchannel = new MemberChannelData().GetChannelDetail(nowchannelid);
                                                        if (nowchannel.Whetherdefaultchannel != 1)
                                                        {
                                                            //发送顾问信息
                                                            CustomerMsg_Send.AutoFenpeiChannel(basic.Comid, requestXML.FromUserName, 1, sourcer.Channelcompanyid, sourcer.Channelid);
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                        #endregion
                                        #region 没有绑定渠道的二维码
                                        else //没有绑定渠道的二维码
                                        {
                                            #region 扫描素材二维码
                                            if (sourcer.Wxmaterialid > 0)//扫描素材二维码
                                            {
                                                //根据素材id得到素材信息
                                                WxMaterial m_wxmaterial = new WxMaterialData().GetWxMaterial(sourcer.Wxmaterialid);
                                                if (m_wxmaterial != null)
                                                {
                                                    string materialname = m_wxmaterial.Title;
                                                    string materialimgurl = FileSerivce.GetImgUrl(m_wxmaterial.Imgpath.ConvertTo<int>(0));
                                                    string linkurl = httphead + basic.Domain + aboutmaterialdetailurl + m_wxmaterial.MaterialId + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;


                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount>" +
                                                    "<Articles><item><Title><![CDATA[" + materialname + "]]></Title><Description><![CDATA[]]></Description><PicUrl><![CDATA[" + materialimgurl + "]]></PicUrl><Url><![CDATA[" + linkurl + "]]></Url></item></Articles></xml>";
                                                }
                                            }
                                            #endregion
                                            #region 扫描产品二维码
                                            if (sourcer.Productid > 0)//扫描产品二维码
                                            {
                                                //根据产品id得到产品信息
                                                B2b_com_pro pro = new B2bComProData().GetProById(sourcer.Productid.ToString());
                                                if (pro != null)
                                                {
                                                    string proname = pro.Pro_name;
                                                    string proimgurl = FileSerivce.GetImgUrl(pro.Imgurl);
                                                    string linkurl = "";
                                                    if (pro.Server_type == 1 || pro.Server_type == 12)//票务 或者 预约产品
                                                    {
                                                        linkurl = httphead + basic.Domain + "/h5/order/pro.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                    }
                                                    else if (pro.Server_type == 2 || pro.Server_type == 8)//旅游
                                                    {
                                                        linkurl = httphead + basic.Domain + "/h5/linedetail.aspx?lineid=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                    }
                                                    else if (pro.Server_type == 9)//酒店客房
                                                    {
                                                        linkurl = httphead + basic.Domain + "/h5/hotel/hotelshow.aspx?proid=" + pro.Id + "&id=" + basic.Comid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                    }
                                                    else if (pro.Server_type == 11)//实物
                                                    {
                                                        linkurl = httphead + basic.Domain + "/h5/order/pro.aspx?id=" + pro.Id + "&projectid=" + pro.Projectid + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                    }
                                                    else
                                                    {
                                                        linkurl = "";
                                                    }

                                                    string adviseprice = "￥" + pro.Advise_price.ToString("F2");
                                                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>1</ArticleCount>" +
                                                    "<Articles><item><Title><![CDATA[" + proname + "]]></Title><Description><![CDATA[" + adviseprice + "]]></Description><PicUrl><![CDATA[" + proimgurl + "]]></PicUrl><Url><![CDATA[" + linkurl + "]]></Url></item></Articles></xml>";

                                                }
                                            }
                                            #endregion
                                            #region 扫描门市二维码
                                            if (sourcer.Channelcompanyid > 0)//扫描门市二维码
                                            {
                                                Member_Channel_company company = new MemberChannelcompanyData().GetChannelCompanyByWxsourceId(wxsourceid);
                                                if (company != null)
                                                {
                                                    if (company.Issuetype == 1)//如果用户二次扫描的是合作单位，则显示合作单位信息
                                                    {
                                                        #region  已经注释掉，原来的显示内容
                                                        //  B2b_company gongsi = B2bCompanyData.GetCompany(basic.Comid);

                                                        //  string outstr = "感谢您关注 " + gongsi.Com_name + "特约合作商户:\n\n" +
                                                        //"<a href='" + httphead + basic.Domain + "/h5/StoreDefault.aspx?menshiid=" + company.Id + "'>" + company.Companyname + "</a>\n";
                                                        //  if (company.Companyphone.Trim() != "")
                                                        //  {
                                                        //      outstr += "电话 " + company.Companyphone + "\n";
                                                        //  }
                                                        //  if (company.Companyaddress.Trim() != "")
                                                        //  {
                                                        //      outstr += "地址 " + company.Companyaddress + "\n";
                                                        //  }
                                                        //  if (company.Bookurl.Trim() != "")
                                                        //  {
                                                        //      outstr += "<a href='" + company.Bookurl.Trim() + "'>点击立即预定 </a>\n ";
                                                        //  }

                                                        //  resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + outstr + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                        #endregion
                                                        Member_Channel_company c_company = new MemberChannelcompanyData().GetChannelCompany(sourcer.Channelcompanyid.ToString());
                                                        if (c_company != null)
                                                        {
                                                            string linkurl = httphead + basic.Domain + "/h5/StoreDefault.aspx?menshiid=" + c_company.Id + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            string content = "欢迎您访问我们的门店：<a href='" + linkurl + "'>" + c_company.Companyname + "</a>\n";
                                                            //string companyimgurl = FileSerivce.GetImgUrl(c_company.Companyimg);

                                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                        }
                                                        else
                                                        {
                                                            string linkurl = httphead + basic.Domain + "/h5/Storelist.aspx?uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            string content = "欢迎您访问<a href='" + linkurl + "'>我们的门店</a>\n";
                                                            //string companyimgurl = FileSerivce.GetImgUrl(c_company.Companyimg);

                                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                        }
                                                    }
                                                    if (company.Issuetype == 0)//如果扫描的是内部门店二维码
                                                    {
                                                        #region 已经注释掉，原来的显示内容
                                                        ////根据微信号获判断会员渠道信息(如果会员是公司会员 即渠道为微信注册渠道，则渠道修改为门市默认渠道)
                                                        //bool iscompanycrm = new B2bCrmData().IsCompanyCrm(requestXML.FromUserName, basic.Comid);
                                                        //if (iscompanycrm)
                                                        //{
                                                        //    //int channelid = 0;//默认渠道id
                                                        //    //根据渠道单位id得到 默认渠道人
                                                        //    Member_Channel defaultchannel = new MemberChannelData().GetDefaultChannel(company.Id);

                                                        //    if (defaultchannel == null)//没有默认渠道人，添加渠道单位的默认渠道人
                                                        //    {

                                                        //        Member_Channel channel = new Member_Channel()
                                                        //        {
                                                        //            Id = 0,
                                                        //            Com_id = company.Com_id,
                                                        //            Issuetype = company.Issuetype,
                                                        //            Companyid = company.Id,
                                                        //            Name = "默认渠道",
                                                        //            Mobile = "",
                                                        //            Cardcode = 0,
                                                        //            Chaddress = "",
                                                        //            ChObjects = "",
                                                        //            RebateOpen = 0,
                                                        //            RebateConsume = 0,
                                                        //            RebateConsume2 = 0,
                                                        //            RebateLevel = 0,
                                                        //            Opencardnum = 0,
                                                        //            Firstdealnum = 0,
                                                        //            Summoney = 0,
                                                        //            Whetherdefaultchannel = 1,
                                                        //            Runstate = 1
                                                        //        };
                                                        //        channelid = new MemberChannelData().EditChannel(channel);
                                                        //    }
                                                        //    else
                                                        //    {
                                                        //        channelid = defaultchannel.Id;
                                                        //    }
                                                        //    if (channelid > 0)
                                                        //    {
                                                        //        int resultt = new MemberCardData().UpMemberChennel(requestXML.FromUserName, channelid);
                                                        //    }

                                                        //    string innerstr = "感谢您关注 " + company.Companyname + "\n" +

                                                        //      "电话 " + company.Companyphone + "\n" +
                                                        //      "地址 " + company.Companyaddress + "\n" +

                                                        //       "<a href='" + httphead + basic.Domain + "/h5/StoreDefault.aspx?menshiid=" + company.Id + "'> 请点击链接查看更多...</a>\n";

                                                        //    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + innerstr + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                                                        //}
                                                        #endregion
                                                        Member_Channel_company c_company = new MemberChannelcompanyData().GetChannelCompany(sourcer.Channelcompanyid.ToString());
                                                        if (c_company != null)
                                                        {
                                                            string linkurl = httphead + basic.Domain + "/h5/StoreDefault.aspx?menshiid=" + c_company.Id + "&uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            string content = "欢迎您访问我们的门店：<a href='" + linkurl + "'>" + c_company.Companyname + "</a>\n";
                                                            //string companyimgurl = FileSerivce.GetImgUrl(c_company.Companyimg);

                                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                        }
                                                        else
                                                        {
                                                            string linkurl = httphead + basic.Domain + "/h5/Storelist.aspx?uid=" + crminfo.Id + "&openid=" + requestXML.FromUserName + "&weixinpass=" + pass;
                                                            string content = "欢迎您访问<a href='" + linkurl + "'>我们的门店</a>\n";
                                                            //string companyimgurl = FileSerivce.GetImgUrl(c_company.Companyimg);

                                                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + content + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                        #endregion
                                    }

                                }

                            }
                        }

                    }
                    #endregion
                    #region 扫码推事件且弹出“消息接收中”提示框的事件推送
                    if (requestXML.Eevent == "scancode_waitmsg")
                    {

                        requestXML.Content = "扫码内容:" + requestXML.ScanResult;
                        requestXML.ContentType = true;
                        requestXML.Comid = basic.Comid;
                        int inswxexchangemsg = new WxRequestXmlData().EditWxRequestXmlLog(requestXML);

                    }
                    #endregion
                }
                #endregion


                #region 发送被动响应信息，信息内容录入数据库
                if (resxml != "")
                {
                    //发送被动响应信息，信息内容录入数据库
                    XmlDocument docc = new XmlDocument();
                    docc.LoadXml(resxml);
                    XmlElement rootElement = docc.DocumentElement;
                    XmlNode MsgType = rootElement.SelectSingleNode("MsgType");
                    if (requestXML.MsgType == "text" || requestXML.MsgType == "voice")//用户发送微信信息是文本或者语音时，对返回内容进行记录
                    {
                        if (MsgType.InnerText == "text" && rootElement.SelectSingleNode("Content").InnerText != "")
                        {
                            RequestXML retRequestXML = new RequestXML();
                            retRequestXML.PostStr = resxml;
                            retRequestXML.ToUserName = rootElement.SelectSingleNode("ToUserName").InnerText;
                            retRequestXML.FromUserName = rootElement.SelectSingleNode("FromUserName").InnerText;
                            retRequestXML.CreateTime = rootElement.SelectSingleNode("CreateTime").InnerText;
                            retRequestXML.MsgType = rootElement.SelectSingleNode("MsgType").InnerText;
                            retRequestXML.Content = rootElement.SelectSingleNode("Content").InnerText;
                            retRequestXML.contentType = false;
                            retRequestXML.Comid = basic.Comid;
                            int inswxexchangemsg = new WxRequestXmlData().EditWxRequestXmlLog(retRequestXML);
                        }
                    }
                }
                else
                {
                    //resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[ ]]></Content></xml>";
                    resxml = "";
                }
                #endregion

                System.Web.HttpContext.Current.Response.Write(resxml);

            }
            catch (Exception ex)
            {
                TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\weixinerrlog.txt", "err1:" + ex.Message);

                ////WriteTxt("异常" + ex.Message + "Struck:" + ex.StackTrace.ToString());
                //wx_logs.MyInsert("异常" + ex.Message + "Struck:" + ex.StackTrace.ToString());
                //string emptystr = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[]]></Content></xml>";
                string emptystr = "";
                System.Web.HttpContext.Current.Response.Write(emptystr);
            }
        }

        private string SelectMenShiByOpenId(RequestXML requestXML, WeiXinBasic basic)
        {

            string resxml = "";
            //根据微信号得到会员所在渠道信息
            Member_Channel channel = new MemberChannelData().GetChannelByOpenId(requestXML.FromUserName);

            B2bCrmData userdate = new B2bCrmData();
            //设定并获取微信随机码                              
            var pass = userdate.WeixinGetPass(requestXML.FromUserName, basic.Comid);

            //返回的默认内容
            string anwsertext = "<a  href='http://shop" + basic.Comid + ".etown.cn/weixin/skippage.aspx?openid=" + requestXML.FromUserName + "&weixinpass=" + pass + "'>请点击查看 服务顾问信息 </a>\n";

            if (channel == null)
            { }
            else
            {
                //渠道单位类型:0:内部渠道；1：外部渠道;3:网站注册;4:微信注册
                //(1)会员属于公司:显示公司信息(名称，电话，地址)
                //(2)会员属于门店:显示门店信息(名称，电话，地址)+服务人员（姓名，电话）
                if (channel.Issuetype == 0 || channel.Issuetype == 1)//会员属于门店
                {
                    //根据微信号得到会员所在的门市信息
                    Member_Channel_company menshi = new MemberChannelcompanyData().GetMenShiByOpenId(requestXML.FromUserName, basic.Comid);
                    #region //我的顾问，已经注释掉
                    //string anwsertext = "亲，请语音或文字留言给我们吧。\n" +
                    //    "或联系您的服务顾问 \n" +
                    //    menshi.Companyname + "\n";
                    ////如果是默认渠道的话，不显示服务专员信息
                    //if (channel.Whetherdefaultchannel == 1)
                    //{ }
                    //else
                    //{
                    //    string guwen = "";
                    //    if (channel.Name != "")
                    //    {
                    //        guwen += channel.Name;
                    //    }
                    //    if (channel.Mobile != "")
                    //    {
                    //        guwen += channel.Mobile;
                    //    }
                    //    if (guwen.Trim() != "")
                    //    {
                    //        anwsertext += "" + guwen + "\r\n";
                    //    }

                    //}
                    //anwsertext += "电话 " + menshi.Companyphone + "\r\n";
                    //anwsertext += "地址 " + menshi.Companyaddress + "\r\n";

                    //anwsertext += "请点击 <a href='http://shop" + basic.Comid + ".etown.cn/weixin/skippage.aspx?openid=" + requestXML.FromUserName + "&weixinpass=" + pass + "'>服务顾问</a> 页面\n";
                    #endregion
                    #region 我的顾问, new
                    //如果是默认渠道的话，不显示服务专员信息
                    if (channel.Whetherdefaultchannel == 1)
                    { }
                    else
                    {
                        if (channel.Name != "")
                        {
                            anwsertext = "亲，我是您的服务顾问：" + channel.Name + " \n\n" +

                                            "<a  href='http://shop" + basic.Comid + ".etown.cn/weixin/skippage.aspx?openid=" + requestXML.FromUserName + "&weixinpass=" + pass + "'>请点击查看 服务顾问信息 </a>\n\n" +

                                            "或在微信中直接留言，我会很快为你回复...\n";
                        }
                    }
                    #endregion
                }
                else //会员属于公司
                { }
            }

            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + anwsertext + "]]></Content><FuncFlag>1</FuncFlag></xml>";
            return resxml;


        }

        private string SendAutoReply(string FromUserName, string ToUserName, string defaultret)
        {
            string resxml = "";
            //第一次留言时，系统返回微信接口自动回复，当客人48小时之内第二次留言时，自动留言回复就不返回了；
            //也就是说， 我们后台设置的留言自动回复，48小时内，只返回一次。
            bool whethersendautoreply = new WxRequestXmlData().GetWhetherSendAutoReply(FromUserName, defaultret);//判断48小时内是否返回过自动回复
            if (whethersendautoreply)
            {
                //resxml = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[]]></Content><FuncFlag>1</FuncFlag></xml>";
                resxml = "";
            }
            else
            {
                resxml = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + defaultret + "]]></Content><FuncFlag>1</FuncFlag></xml>";

            }
            return resxml;
        }
        /// <summary>
        /// 微信关注事件
        /// </summary>
        /// <param name="requestXML"></param>
        /// <param name="basic"></param>
        /// <returns></returns>
        private string SubscribeWeiXin(RequestXML requestXML, WeiXinBasic basic, int subscribesourceid = 0)
        {
            try
            {
                string resxml = "";

                // 第一次关注微信时显示
                string d = basic.Attentionautoreply.Replace("<p>", "").Replace("</p>", "\r\n").Replace("<br />", "\r\n").Replace("&ldquo;", "\"").Replace("&rdquo;", "\"").Replace("&nbsp;", " ").Replace("<span>", "").Replace("</span>", "");

                //首次关注，直接注册成为账户
                string weixinNo = requestXML.FromUserName;//用户微信号
                #region 搜索公众号关注
                if (subscribesourceid == 0)//搜索公众号关注
                {
                    //判断是否已经是注册账户
                    B2b_crm b2bcrm = new B2bCrmData().GetB2bCrmByWeiXin(weixinNo, basic.Comid);
                    #region 取消关注后又搜索公众号重新关注:(1)会员表中标注状态:关注(2)卡号表中对应的渠道修改为微信注册渠道(此处不用修改，原来取消关注时对应渠道就已经设置微信注册渠道)
                    if (b2bcrm != null)//取消关注后又搜索公众号重新关注:(1)会员表中标注状态:关注(2)卡号表中对应的渠道修改为微信注册渠道(此处不用修改，原来取消关注时对应渠道就已经设置微信注册渠道)
                    {
                        SqlHelper sqlhelper = new SqlHelper();
                        sqlhelper.BeginTrancation();
                        try
                        {
                            SqlCommand cmd = new SqlCommand();
                            cmd = sqlhelper.PrepareTextSqlCommand("update b2b_crm set whetherwxfocus=1 where weixin='" + requestXML.FromUserName + "' and com_id=" + basic.Comid);//会员表中标注状态:未关注
                            cmd.ExecuteNonQuery();

                            sqlhelper.Commit();
                        }
                        catch (Exception e)
                        {

                            sqlhelper.Rollback();
                        }
                        finally
                        {
                            sqlhelper.Dispose();
                            resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + d + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                        }

                    }
                    #endregion

                    #region 新用户搜索微信号关注的
                    else
                    {
                        resxml = InsB2bCrm(basic, requestXML, d, subscribesourceid);
                    }
                    #endregion
                }
                #endregion
                #region 搜索带参二维码关注
                else //搜索带参二维码关注
                {
                    //判断是否已经是注册账户
                    B2b_crm b2bcrm = new B2bCrmData().GetB2bCrmByWeiXin(weixinNo, basic.Comid);

                    #region 取消关注后又搜索带参二维码重新关注:(1)会员表中标注状态:关注(2)卡号表中对应的渠道修改为新的渠道
                    if (b2bcrm != null)//取消关注后又搜索带参二维码重新关注:(1)会员表中标注状态:关注(2)卡号表中对应的渠道修改为新的渠道:新添需求，如果是合作单位，则渠道不修改 
                    {
                        WxSubscribeSource model2 = new WxSubscribeSourceData().Getwxqrcode(subscribesourceid);
                        if (model2 != null)
                        {
                            resxml = ChannelChangeByParamQrcode(requestXML, basic, model2);
                        }
                    }
                    #endregion
                    #region 新用户扫描带参二维码关注的
                    else
                    {
                        resxml = InsB2bCrm(basic, requestXML, d, subscribesourceid);
                    }
                    #endregion
                }
                #endregion
                return resxml;
            }
            catch (Exception ex)
            {
                TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\weixinerrlog.txt", "err2:" + ex.Message);
                return "";
            }
        }

        private string ChannelChangeByParamQrcode(RequestXML requestXML, WeiXinBasic basic, WxSubscribeSource model2)
        {
            try
            {
                //第一次关注微信时默认显示内容
                string resxml = basic.Attentionautoreply.Replace("<p>", "").Replace("</p>", "\r\n").Replace("<br />", "\r\n").Replace("&ldquo;", "\"").Replace("&rdquo;", "\"").Replace("&nbsp;", " ").Replace("<span>", "").Replace("</span>", "");

                #region 关注活动  过来的，应该为微信注册渠道，不过此处不用修改，原来取消关注时/默认关注时 对应渠道就已经设置为微信注册渠道
                if (model2.Activityid > 0)
                {
                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + resxml + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                }
                #endregion

                #region 扫描门市单位 或者门市单位渠道人 二维码过来的
                if (model2.Channelcompanyid > 0)//关注渠道单位过来的
                {
                    int channelid = 0;
                    //根据渠道单位id得到渠道单位
                    Member_Channel_company company = new MemberChannelcompanyData().GetChannelCompany((model2.Channelcompanyid).ToString());

                    #region  得到带参二维码的服务顾问 即渠道人信息(指向渠道单位，没有精确到渠道人的 则赋值默认渠道人)
                    string Serviceadvisor = "";//服务专员(顾问)信息 
                    if (company != null)
                    {
                        #region 门店 或者 合作单位，都精确到渠道人

                        //判断带参二维码是否精确到了渠道人
                        if (model2.Channelid > 0)
                        {
                            channelid = model2.Channelid;
                            Member_Channel channel = new MemberChannelData().GetChannelDetail(model2.Channelid);
                            //如果是默认渠道的话，不显示服务专员信息
                            if (channel.Whetherdefaultchannel == 1)
                            {
                                ////如果不存在渠道人，则查询渠道单位的门店经理信息
                                //B2b_company_manageuser manager = new B2bCompanyManagerUserData().GetMenshiManagerByMenshiId(model2.Channelcompanyid);
                                //if (manager != null)
                                //{
                                //    Serviceadvisor += manager.Accounts + manager.Tel + "\r\n";
                                //}
                            }
                            else
                            {
                                if (channel.Name != "")
                                {
                                    Serviceadvisor += channel.Name;
                                }
                                if (channel.Mobile != "")
                                {
                                    Serviceadvisor += channel.Mobile;
                                }

                                if (Serviceadvisor.Trim() != "")
                                {
                                    Serviceadvisor += "\n\n";
                                }

                            }
                        }
                        else //带参二维码没有精确到具体渠道人，则指向默认渠道人
                        {
                            //根据渠道单位id得到 默认渠道人
                            Member_Channel defaultchannel = new MemberChannelData().GetDefaultChannel(model2.Channelcompanyid);

                            if (defaultchannel == null)//没有默认渠道人，添加渠道单位的默认渠道人
                            {
                                Member_Channel channel = new Member_Channel()
                                {
                                    Id = 0,
                                    Com_id = company.Com_id,
                                    Issuetype = company.Issuetype,
                                    Companyid = company.Id,
                                    Name = "默认渠道",
                                    Mobile = "",
                                    Cardcode = 0,
                                    Chaddress = "",
                                    ChObjects = "",
                                    RebateOpen = 0,
                                    RebateConsume = 0,
                                    RebateConsume2 = 0,
                                    RebateLevel = 0,
                                    Opencardnum = 0,
                                    Firstdealnum = 0,
                                    Summoney = 0,
                                    Whetherdefaultchannel = 1,
                                    Runstate = 1
                                };
                                channelid = new MemberChannelData().EditChannel(channel);
                            }
                            else
                            {
                                channelid = defaultchannel.Id;
                            }

                            ////如果不存在渠道人，则查询渠道单位的门店经理信息
                            //B2b_company_manageuser manager = new B2bCompanyManagerUserData().GetMenshiManagerByMenshiId(model2.Channelcompanyid);
                            //if (manager != null)
                            //{
                            //    Serviceadvisor += manager.Accounts + manager.Tel + "\r\n";
                            //}
                        }

                        #endregion

                        #region 以下代码已经注释，由上面代码替代
                        #region 内部门店 归为门店会员 已经注释
                        //if (company.Issuetype == 0)//内部门店 归为门店会员
                        //{
                        //    //判断带参二维码是否精确到了渠道人
                        //    if (model2.Channelid > 0)
                        //    {
                        //        channelid = model2.Channelid;
                        //        Member_Channel channel = new MemberChannelData().GetChannelDetail(model2.Channelid);
                        //        //如果是默认渠道的话，不显示服务专员信息
                        //        if (channel.Whetherdefaultchannel == 1)
                        //        {
                        //            ////如果不存在渠道人，则查询渠道单位的门店经理信息
                        //            //B2b_company_manageuser manager = new B2bCompanyManagerUserData().GetMenshiManagerByMenshiId(model2.Channelcompanyid);
                        //            //if (manager != null)
                        //            //{
                        //            //    Serviceadvisor += manager.Accounts + manager.Tel + "\r\n";
                        //            //}
                        //        }
                        //        else
                        //        {
                        //            if (channel.Name != "")
                        //            {
                        //                Serviceadvisor += channel.Name;
                        //            }
                        //            if (channel.Mobile != "")
                        //            {
                        //                Serviceadvisor += channel.Mobile;
                        //            }

                        //            if (Serviceadvisor.Trim() != "")
                        //            {
                        //                Serviceadvisor += "\n\n";
                        //            }

                        //        }
                        //    }
                        //    else //带参二维码没有精确到具体渠道人，则指向默认渠道人
                        //    {
                        //        //根据渠道单位id得到 默认渠道人
                        //        Member_Channel defaultchannel = new MemberChannelData().GetDefaultChannel(model2.Channelcompanyid);

                        //        if (defaultchannel == null)//没有默认渠道人，添加渠道单位的默认渠道人
                        //        {
                        //            Member_Channel channel = new Member_Channel()
                        //            {
                        //                Id = 0,
                        //                Com_id = company.Com_id,
                        //                Issuetype = company.Issuetype,
                        //                Companyid = company.Id,
                        //                Name = "默认渠道",
                        //                Mobile = "",
                        //                Cardcode = 0,
                        //                Chaddress = "",
                        //                ChObjects = "",
                        //                RebateOpen = 0,
                        //                RebateConsume = 0,
                        //                RebateConsume2 = 0,
                        //                RebateLevel = 0,
                        //                Opencardnum = 0,
                        //                Firstdealnum = 0,
                        //                Summoney = 0,
                        //                Whetherdefaultchannel = 1,
                        //                Runstate = 1
                        //            };
                        //            channelid = new MemberChannelData().EditChannel(channel);
                        //        }
                        //        else
                        //        {
                        //            channelid = defaultchannel.Id;
                        //        }

                        //        ////如果不存在渠道人，则查询渠道单位的门店经理信息
                        //        //B2b_company_manageuser manager = new B2bCompanyManagerUserData().GetMenshiManagerByMenshiId(model2.Channelcompanyid);
                        //        //if (manager != null)
                        //        //{
                        //        //    Serviceadvisor += manager.Accounts + manager.Tel + "\r\n";
                        //        //}
                        //    }
                        //}
                        #endregion
                        #region 外部合作公司 归为公司微信注册会员 已经注释
                        //else//外部合作公司 归为公司微信注册会员 
                        //{
                        //    //获得渠道号(web,weixin)        
                        //    Member_Channel Channelmodel = new MemberChannelData().GetChannelWebWeixin("weixin", basic.Comid);

                        //    if (Channelmodel != null)
                        //    {
                        //        channelid = Channelmodel.Id;
                        //    }
                        //}
                        #endregion
                        #endregion
                    }
                    #endregion

                    #region 内部单位 或 外部单位 关注显示内容
                    //扫描有参二维码，并且二维码对应渠道单位
                    //(1)外部渠道单位：
                    //显示:引言，单位名称，电话，地址，坐标，介绍,微网站链接
                    //(2)内部渠道单位:
                    //显示：引言,单位名称，电话，地址，坐标，介绍，微网站链接
                    if (company.Issuetype == 0)//内部渠道单位
                    {
                        //string innerstr = "感谢您关注 " + company.Companyname + "\n电话 " + company.Companyphone + "\n地址" + Serviceadvisor.Trim() == "" ? "" : "顾问:" + Serviceadvisor + "\n" + company.Companyaddress + "\n" + "<a href='" + httphead + basic.Domain + "/h5/StoreDefault.aspx?menshiid=" + model2.Channelcompanyid + "'> 请点击链接查看更多...</a>\n  ";
                        string innerstr = "感谢您关注 " + company.Companyname + "\n" +
                              Serviceadvisor +
                       "电话 " + company.Companyphone + "\n" +
                       "地址 " + company.Companyaddress + "\n" +

                        "<a href='" + httphead + basic.Domain + "/h5/StoreDefault.aspx?menshiid=" + model2.Channelcompanyid + "'> 请点击链接查看更多...</a>\n";

                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + innerstr + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                    }
                    else //外部渠道单位
                    {
                        B2b_company gongsi = B2bCompanyData.GetCompany(basic.Comid);

                        //string outstr = "感谢您关注 " + gongsi.Com_name + ",特约合作商户" + company.Companyname + "\n<a href='" + httphead + basic.Domain + "/h5/StoreDefault.aspx?menshiid=" + model2.Channelcompanyid + "'> 请点击链接查看更多...</a>\n ";
                        string outstr = "感谢您关注 " + gongsi.Com_name + "特约合作商户:\n\n" +
                           "<a href='" + httphead + basic.Domain + "/h5/StoreDefault.aspx?menshiid=" + model2.Channelcompanyid + "'>" + company.Companyname + "</a>\n";
                        if (company.Companyphone.Trim() != "")
                        {
                            outstr += "电话 " + company.Companyphone + "\n";
                        }
                        if (company.Companyaddress.Trim() != "")
                        {
                            outstr += "地址 " + company.Companyaddress + "\n";
                        }
                        if (company.Bookurl.Trim() != "")
                        {
                            outstr += "<a href='" + company.Bookurl.Trim() + "'>点击立即预定 </a>\n ";
                        }

                        resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + outstr + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                    }
                    #endregion

                    #region 修改会员渠道
                    SqlHelper sqlhelper = new SqlHelper();
                    sqlhelper.BeginTrancation();
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd = sqlhelper.PrepareTextSqlCommand("update b2b_crm set whetherwxfocus=1 where weixin='" + requestXML.FromUserName + "' and com_id=" + basic.Comid);//会员表中标注状态:关注
                        cmd.ExecuteNonQuery();


                        cmd = sqlhelper.PrepareTextSqlCommand("update Member_Card set IssueCard=" + channelid + " where cardcode =(select idcard from b2b_crm where weixin='" + requestXML.FromUserName + "' and com_id=" + basic.Comid + ")");//卡号表中对应的渠道修改为新的渠道
                        cmd.ExecuteNonQuery();

                        sqlhelper.Commit();
                    }
                    catch (Exception e)
                    {
                        sqlhelper.Rollback();
                    }
                    finally
                    {
                        sqlhelper.Dispose();
                    }
                    #endregion
                }
                #endregion

                #region  扫描公司员工二维码过来的
                if (model2.Channelcompanyid == 0 && model2.Channelid > 0)
                {
                    int channelid = model2.Channelid;


                    string Serviceadvisor = "";//服务专员(顾问)信息 

                    #region 获取顾问信息

                    //判断带参二维码是否精确到了渠道人

                    channelid = model2.Channelid;
                    Member_Channel channel = new MemberChannelData().GetChannelDetail(model2.Channelid);
                    //如果是默认渠道的话，不显示服务专员信息
                    if (channel.Whetherdefaultchannel == 1)
                    {
                        ////如果不存在渠道人，则查询渠道单位的门店经理信息
                        //B2b_company_manageuser manager = new B2bCompanyManagerUserData().GetMenshiManagerByMenshiId(model2.Channelcompanyid);
                        //if (manager != null)
                        //{
                        //    Serviceadvisor += manager.Accounts + manager.Tel + "\r\n";
                        //}
                    }
                    else
                    {
                        if (channel.Name != "")
                        {
                            Serviceadvisor += channel.Name;
                        }
                        if (channel.Mobile != "")
                        {
                            Serviceadvisor += channel.Mobile;
                        }

                        if (Serviceadvisor.Trim() != "")
                        {
                            Serviceadvisor += "\n\n";
                        }

                    }

                    #endregion

                    #region 关注显示内容

                    //根据顾问信息中的电话得到 员工信息
                    B2b_company_manageuser u = new B2bCompanyManagerUserData().GetCompanyUserByPhone(channel.Mobile, channel.Com_id);

                    string linkurl = "";
                    if (u != null)
                    {
                        linkurl = "http://shop" + basic.Comid + ".etown.cn/h5/People.aspx?MasterId=" + u.Id;
                    }

                    string str = "点击联系 服务顾问：<a href='" + linkurl + "'>" + Serviceadvisor + "</a>\n";


                    B2b_company gongsi = B2bCompanyData.GetCompany(basic.Comid);
                    string innerstr = "感谢您关注 " + gongsi.Com_name + "\n" +
                              str;

                    resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + innerstr + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                    #endregion

                    #region 修改会员渠道
                    SqlHelper sqlhelper = new SqlHelper();
                    sqlhelper.BeginTrancation();
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd = sqlhelper.PrepareTextSqlCommand("update b2b_crm set whetherwxfocus=1 where weixin='" + requestXML.FromUserName + "' and com_id=" + basic.Comid);//会员表中标注状态:关注
                        cmd.ExecuteNonQuery();


                        cmd = sqlhelper.PrepareTextSqlCommand("update Member_Card set IssueCard=" + channelid + " where cardcode =(select idcard from b2b_crm where weixin='" + requestXML.FromUserName + "' and com_id=" + basic.Comid + ")");//卡号表中对应的渠道修改为新的渠道
                        cmd.ExecuteNonQuery();

                        sqlhelper.Commit();
                    }
                    catch (Exception e)
                    {
                        sqlhelper.Rollback();
                    }
                    finally
                    {
                        sqlhelper.Dispose();
                    }
                    #endregion
                }
                #endregion

                return resxml;
            }
            catch (Exception ex)
            {
                TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\weixinerrlog.txt", "err4:" + ex.Message);
                return "";
            }
        }

        /// <summary>
        /// 会员信息不存在的话录入新会员
        /// </summary>
        /// <param name="basic"></param>
        /// <param name="FromUserName"></param>
        /// <param name="ToUserName"></param>
        /// <param name="defultret">如果是第一次微信关注默认返回内容</param>
        /// <returns></returns>
        private string InsB2bCrm(WeiXinBasic basic, RequestXML requestXML, string defultret, int subscribesourceid = 0)
        {
            try
            {
                if (defultret == "")
                {
                    //第一次关注微信时默认显示内容
                    defultret = basic.Attentionautoreply.Replace("<p>", "").Replace("</p>", "\r\n").Replace("<br />", "\r\n").Replace("&ldquo;", "\"").Replace("&rdquo;", "\"").Replace("&nbsp;", " ").Replace("<span>", "").Replace("</span>", "");
                }

                string FromUserName = requestXML.FromUserName;
                string ToUserName = requestXML.ToUserName;

                //向会员表（b2b_crm）插入会员
                string cardcode = MemberCardData.CreateECard(2, basic.Comid);//创建卡号并插入活动

                string weixinpass = new B2bCrmData().WeixinGetPass(FromUserName, basic.Comid);

                int whetherwxfocus = 1;//微信注册，微信关注状态标注为1(关注)
                int crmlevel = 0;//会员级别:微信一般会员
                int insb2bcrm = new B2bCrmData().InsB2bCrm(basic.Comid, cardcode, FromUserName, weixinpass, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), whetherwxfocus, crmlevel.ToString());
                if (insb2bcrm == 0)
                {
                    defultret = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[创建微信新会员出错]]></Content><FuncFlag>1</FuncFlag></xml>";
                }
                else
                {
                    #region  微信关注积分变动
                    int jifen_range = 0;//积分变动量
                    //插入关注赠送优惠券
                    var InputMoney = MemberCardData.AutoInputMoeny(insb2bcrm, 4, basic.Comid, out jifen_range);
                    //变动等积分
                    B2bcrm_dengjifenlog djflog = new B2bcrm_dengjifenlog
                    {
                        id = 0,
                        crmid = insb2bcrm,
                        dengjifen = jifen_range,
                        ptype = 1,
                        opertor = "系统自动录入",
                        opertime = DateTime.Now,
                        orderid = 0,
                        ordername = "微信关注赠送",
                        remark = "微信关注赠送"
                    };
                    new B2bCrmData().Adjust_dengjifen(djflog, insb2bcrm, basic.Comid, jifen_range);



                    if (InputMoney != 0)
                    {//只要不等于0则有赠送积分活动


                        B2bCrmData prodata = new B2bCrmData();
                        var list = prodata.Readuser(insb2bcrm, basic.Comid);

                        //微信消息模板
                        if (list.Weixin != "")
                        {
                            new Weixin_tmplmsgManage().WxTmplMsg_CrmIntegralReward(basic.Comid, list.Weixin, "您好，积分已经打入您的账户", list.Idcard.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "活动赠送", list.Integral.ToString(), list.Integral.ToString(), "如有疑问，请致电客服。");
                        }
                    }
                    #endregion

                    #region 默认添加的新用户  为微信注册用户
                    int channelid = 0;
                    //获得渠道号(web,weixin)        
                    Member_Channel Channelmodel = new MemberChannelData().GetChannelWebWeixin("weixin", basic.Comid);

                    if (Channelmodel != null)
                    {
                        channelid = Channelmodel.Id;
                    }
                    //卡号和渠道id关联
                    int upCardcodeChannel = new MemberCardData().upCardcodeChannel(cardcode, channelid);
                    if (upCardcodeChannel == 0)
                    {
                        defultret = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[卡号和渠道id关联出错]]></Content><FuncFlag>1</FuncFlag></xml>";

                    }
                    #endregion

                    #region 新用户搜索公众号关注:默认为 微信注册用户
                    if (subscribesourceid == 0)
                    { }
                    #endregion

                    #region 新用户扫描带参二维码关注
                    else
                    {
                        WxSubscribeSource model2 = new WxSubscribeSourceData().Getwxqrcode(subscribesourceid);
                        if (model2 != null)
                        {
                            defultret = ChannelChangeByParamQrcode(requestXML, basic, model2);
                            #region 以下代码已经注释，封装到了上边的类中
                            //#region 扫描门市单位 或者 门市渠道人 二维码过来的
                            //if (model2.Channelcompanyid > 0)
                            //{
                            //    //根据渠道单位id得到渠道单位
                            //    Member_Channel_company company = new MemberChannelcompanyData().GetChannelCompany((model2.Channelcompanyid).ToString());

                            //    //扫描有参二维码，并且二维码对应渠道单位
                            //    //(1)外部渠道单位：
                            //    //显示:引言，单位名称，电话，地址，坐标，介绍,微网站链接
                            //    //(2)内部渠道单位:
                            //    //显示：引言,单位名称，电话，地址，坐标，介绍，微网站链接

                            //    string Serviceadvisor = "";//服务专员(顾问)信息
                            //    //判断带参二维码是否精确到了渠道人
                            //    if (model2.Channelid > 0)
                            //    {
                            //        channelid = model2.Channelid;

                            //        Member_Channel channel = new MemberChannelData().GetChannelDetail(model2.Channelid);
                            //        //如果是默认渠道的话，不显示服务专员信息
                            //        if (channel.Whetherdefaultchannel == 1)
                            //        {
                            //            ////如果不存在渠道人，则查询渠道单位的门店经理信息
                            //            //B2b_company_manageuser manager = new B2bCompanyManagerUserData().GetMenshiManagerByMenshiId(model2.Channelcompanyid);
                            //            //if (manager != null)
                            //            //{
                            //            //    Serviceadvisor += manager.Accounts + manager.Tel + "\r\n";
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            if (channel.Name != "")
                            //            {
                            //                Serviceadvisor += channel.Name;
                            //            }
                            //            if (channel.Mobile != "")
                            //            {
                            //                Serviceadvisor += channel.Mobile;
                            //            }
                            //            if (Serviceadvisor.Trim() != "")
                            //            {
                            //                Serviceadvisor = Serviceadvisor + "\n\n";
                            //            }
                            //        }
                            //    }
                            //    else //带参二维码没有精确到具体渠道人，则指向默认渠道人
                            //    {
                            //        //根据渠道单位id得到 默认渠道人
                            //        Member_Channel defaultchannel = new MemberChannelData().GetDefaultChannel(model2.Channelcompanyid);

                            //        if (defaultchannel == null)//没有默认渠道人，添加渠道单位的默认渠道人
                            //        {

                            //            Member_Channel channel = new Member_Channel()
                            //            {
                            //                Id = 0,
                            //                Com_id = company.Com_id,
                            //                Issuetype = company.Issuetype,
                            //                Companyid = company.Id,
                            //                Name = "默认渠道",
                            //                Mobile = "",
                            //                Cardcode = 0,
                            //                Chaddress = "",
                            //                ChObjects = "",
                            //                RebateOpen = 0,
                            //                RebateConsume = 0,
                            //                RebateConsume2 = 0,
                            //                RebateLevel = 0,
                            //                Opencardnum = 0,
                            //                Firstdealnum = 0,
                            //                Summoney = 0,
                            //                Whetherdefaultchannel = 1,
                            //                Runstate = 1
                            //            };
                            //            channelid = new MemberChannelData().EditChannel(channel);
                            //        }
                            //        else
                            //        {
                            //            channelid = defaultchannel.Id;
                            //        }

                            //        ////如果不存在渠道人，则查询渠道单位的门店经理信息
                            //        //B2b_company_manageuser manager = new B2bCompanyManagerUserData().GetMenshiManagerByMenshiId(model2.Channelcompanyid);
                            //        //if (manager != null)
                            //        //{
                            //        //    Serviceadvisor += manager.Accounts + manager.Tel + "\r\n";
                            //        //}
                            //    }
                            //    if (company.Issuetype == 0)//内部渠道单位
                            //    {

                            //        //string innerstr = "感谢您关注 " + company.Companyname + ",我们的服务电话 " + company.Companyphone + ",地址" + company.Companyaddress + ",\n" + Serviceadvisor + "\n\n<a href='" + httphead + basic.Domain + "/h5/StoreDefault.aspx?menshiid=" + model2.Channelcompanyid + "'> 请点击链接查看更多...</a>\n  ";
                            //        string innerstr = "感谢您关注 " + company.Companyname + "\n" +
                            //            Serviceadvisor +
                            //        "电话 " + company.Companyphone + "\n" +
                            //        "地址 " + company.Companyaddress + "\n" +

                            //         "<a href='" + httphead + basic.Domain + "/h5/StoreDefault.aspx?menshiid=" + model2.Channelcompanyid + "'> 请点击链接查看更多...</a>\n";

                            //        defultret = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + innerstr + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                            //    }
                            //    else //外部渠道单位
                            //    {
                            //        B2b_company gongsi = B2bCompanyData.GetCompany(basic.Comid);

                            //        //string outstr = "感谢您关注 " + gongsi.Com_name + ",特约合作商户" + company.Companyname + "\n<a href='" + httphead + basic.Domain + "/h5/StoreDefault.aspx?menshiid=" + model2.Channelcompanyid + "'> 请点击链接查看更多...</a>\n ";
                            //        string outstr = "感谢您关注 " + gongsi.Com_name + "特约合作商户:\n\n" +
                            //          "<a href='" + httphead + basic.Domain + "/h5/StoreDefault.aspx?menshiid=" + model2.Channelcompanyid + "'>" + company.Companyname + "</a>\n";
                            //        if (company.Companyphone.Trim() != "")
                            //        {
                            //            outstr += "电话 " + company.Companyphone + "\n";
                            //        }
                            //        if (company.Companyaddress.Trim() != "")
                            //        {
                            //            outstr += "地址 " + company.Companyaddress + "\n";
                            //        }
                            //        if (company.Bookurl.Trim() != "")
                            //        {
                            //            outstr += "<a href='" + company.Bookurl.Trim() + "'>点击立即预定 </a>\n ";
                            //        }

                            //        defultret = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + outstr + "]]></Content><FuncFlag>1</FuncFlag></xml>";

                            //    }

                            //    //卡号和渠道id关联
                            //    if (company.Issuetype == 0)//内部门店 归为门店会员
                            //    {
                            //        int upCardcodeChannel = new MemberCardData().upCardcodeChannel(cardcode, channelid);
                            //        if (upCardcodeChannel == 0)
                            //        {
                            //            defultret = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[卡号和渠道id关联出错]]></Content><FuncFlag>1</FuncFlag></xml>";

                            //        }
                            //    }
                            //    else//外部合作公司 归为公司会员 
                            //    {
                            //        //获得渠道号(web,weixin)        
                            //        Member_Channel Channelmodel = new MemberChannelData().GetChannelWebWeixin("weixin", basic.Comid);

                            //        if (Channelmodel != null)
                            //        {
                            //            channelid = Channelmodel.Id;
                            //        }

                            //        int upCardcodeChannel = new MemberCardData().upCardcodeChannel(cardcode, channelid);
                            //        if (upCardcodeChannel == 0)
                            //        {
                            //            defultret = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[卡号和渠道id关联出错]]></Content><FuncFlag>1</FuncFlag></xml>";

                            //        }
                            //    }
                            //}
                            //#endregion

                            //#region 扫描活动 二维码过来的
                            //if (model2.Activityid>0)
                            //{
                            //    //获得渠道号(web,weixin)        
                            //    Member_Channel Channelmodel = new MemberChannelData().GetChannelWebWeixin("weixin", basic.Comid);

                            //    if (Channelmodel != null)
                            //    {
                            //        channelid = Channelmodel.Id;
                            //    }

                            //    defultret = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + defultret + "]]></Content><FuncFlag>1</FuncFlag></xml>";
                            //    //卡号和渠道id关联
                            //    int upCardcodeChannel = new MemberCardData().upCardcodeChannel(cardcode, channelid);
                            //    if (upCardcodeChannel == 0)
                            //    {
                            //        defultret = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[卡号和渠道id关联出错]]></Content><FuncFlag>1</FuncFlag></xml>";
                            //    }
                            //}
                            //#endregion
                            #endregion
                        }
                    }
                    #endregion
                }
                return defultret;
            }
            catch (Exception ex)
            {
                
                TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\weixinerrlog.txt", "err3:" + ex.Message+"<br>"+ex.ToString());
                return "";
            }
        }



        /// <summary>

        /// unix时间转换为adatetime

        /// </summary>

        /// <param name="timeStamp"></param>

        /// <returns></returns>

        public DateTime UnixTimeToTime(string timeStamp)
        {

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            long lTime = long.Parse(timeStamp + "0000000");

            TimeSpan toNow = new TimeSpan(lTime);

            return dtStart.Add(toNow);

        }



        /// <summary>

        /// datetime转换为aunixtime

        /// </summary>

        /// <param name="time"></param>

        /// <returns></returns>

        public int ConvertDateTimeInt(System.DateTime time)
        {

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

            return (int)(time - startTime).TotalSeconds;

        }



        /// <summary>

        /// 调用百度地图，返回坐标信息

        /// </summary>

        /// <param name="y">经度</param>

        /// <param name="x">纬度</param>

        /// <returns></returns>

        public string GetMapInfo(string x, string y)
        {

            try
            {

                string res = string.Empty;

                string parame = string.Empty;

                string url = "http://maps.googleapis.com/maps/api/geocode/xml";

                parame = "latlng=" + x + "," + y + "&language=zh-CN&sensor=false";//此key为个人申请

                res = webRequestPost(url, parame);



                XmlDocument doc = new XmlDocument();



                doc.LoadXml(res);

                XmlElement rootElement = doc.DocumentElement;

                string Status = rootElement.SelectSingleNode("status").InnerText;

                if (Status == "OK")
                {

                    //仅获取城市

                    XmlNodeList xmlResults = rootElement.SelectSingleNode("/GeocodeResponse").ChildNodes;

                    for (int i = 0; i < xmlResults.Count; i++)
                    {

                        XmlNode childNode = xmlResults[i];

                        if (childNode.Name == "status")
                        {

                            continue;

                        }



                        string city = "0";

                        for (int w = 0; w < childNode.ChildNodes.Count; w++)
                        {

                            for (int q = 0; q < childNode.ChildNodes[w].ChildNodes.Count; q++)
                            {

                                XmlNode childeTwo = childNode.ChildNodes[w].ChildNodes[q];



                                if (childeTwo.Name == "long_name")
                                {

                                    city = childeTwo.InnerText;

                                }

                                else if (childeTwo.InnerText == "locality")
                                {

                                    return city;

                                }

                            }

                        }

                        return city;

                    }

                }

            }

            catch (Exception ex)
            {

                //WriteTxt("map异常" + ex.Message.ToString() + "Struck:" + ex.StackTrace.ToString());

                return "0";

            }



            return "0";

        }



        /// <summary>

        /// Post提交调用抓取

        /// </summary>

        /// <param name="url">提交地址</param>

        /// <param name="param">参数</param>

        /// <returns>string</returns>

        public string webRequestPost(string url, string param)
        {

            byte[] bs = System.Text.Encoding.UTF8.GetBytes(param);



            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url + "?" + param);

            req.Method = "Post";

            req.Timeout = 120 * 1000;

            req.ContentType = "application/x-www-form-urlencoded;";

            req.ContentLength = bs.Length;



            using (Stream reqStream = req.GetRequestStream())
            {

                reqStream.Write(bs, 0, bs.Length);

                reqStream.Flush();

            }

            using (WebResponse wr = req.GetResponse())
            {

                //在这里对接收的信息进行处理



                Stream strm = wr.GetResponseStream();



                StreamReader sr = new StreamReader(strm, System.Text.Encoding.UTF8);



                string line;



                System.Text.StringBuilder sb = new System.Text.StringBuilder();



                while ((line = sr.ReadLine()) != null)
                {

                    sb.Append(line + System.Environment.NewLine);

                }

                sr.Close();

                strm.Close();

                return sb.ToString();

            }

        }
        /// <summary>
        /// 默认返回的提示
        /// </summary>
        /// <returns></returns>
        internal string GetDefault()
        {
            return "默认提示";
        }


        /*解析json字符串*/
        public class InternalClass
        {
            public string kf_account;
            public int status;
            public int kf_id;
            public int auto_accept;
            public int accepted_case;
        }
        public class OuterClass
        {
            public List<InternalClass> kf_online_list;
        }

        public class OOuterClass
        {
            public int type;
            public string msg;
        }

        /// <summary>
        /// 微信给顾问发送消息
        /// </summary>
        /// <param name="channelid">渠道ID</param>
        /// <param name="comid">商户id</param>
        /// <param name="openid">发送微信号</param>
        /// <param name="Content">咨询内容</param>
        /// <returns></returns>
        internal int WeixinMessageChannel(int channelid, int comid, string openid, string Content, int type, string mediaid, WeiXinBasic basic)
        {
            #region 获取微信操作账户
            //对渠道发送消息模板
            var crmdata = new B2bCrmData();
            var channledata = new MemberChannelData();//读取渠道信息

            //获取用户信息，并判断用户是 会员还是渠道
            var username = "";//用户姓名
            var userid = 0;
            var userchannleid = 0;//用户类型判断为渠道时，此时是给客户回复
            var lockuserweixin = "";
            decimal idcard = 0;
            DateTime lockusertime;

            var userinfo = crmdata.b2b_crmH5(openid, comid);//获取微信操作账户
            if (userinfo != null)
            {
                //获取用户姓名
                username = userinfo.Name;
                idcard = userinfo.Idcard;
                userid = userinfo.Id;
                if (username == "")
                {
                    var wxcrminfo = crmdata.Readuser(userinfo.Id, comid);
                    if (wxcrminfo != null)
                    {
                        username = wxcrminfo.WxNickname;//微信别名
                    }
                }


                //判断用户类型（会员，还是渠道）
                if (userinfo.Phone != "")
                {
                    var userchannleinfo = channledata.GetPhoneComIdChannelDetail(userinfo.Phone, comid);
                    if (userchannleinfo != null)
                    {
                        userchannleid = userchannleinfo.Id;//用户为渠道，获取渠道ID
                        username = userchannleinfo.Name;//此时姓名发送为渠道姓名
                        lockuserweixin = userchannleinfo.Lockuserweixin;
                        lockusertime = userchannleinfo.Lockusertime;
                    }
                }
            }
            #endregion

            //判断是 渠道给客户回复还是 客户咨询
            if (userchannleid != 0)//顾问 ---------发送给客户
            {
                #region 顾问
                if (lockuserweixin != "")
                {

                    //服务顾问列表 title 文字用 我的服务顾问 子菜单的名称文字。包括客服通道中的 “服务顾问”文字替换。
                    WxMenu menu = new WxMenuData().GetWxMenuByOperType(27, comid);
                    string title = "服务顾问";
                    if (menu != null)
                    {
                        title = menu.Name;
                    }

                    //给顾问发送，发送客服通道
                    Content = title + " " + username + ": (" + DateTime.Now.ToString("hh:mm") + ")  \n" + Content;
                    string data = "";
                    if (type == 1)
                    {
                        data = CustomerMsg_Send.SendWxMsg(comid, lockuserweixin, type, "", Content, "", openid);
                    }
                    else if (type == 2)
                    {
                        data = CustomerMsg_Send.SendWxMsg(comid, lockuserweixin, type, "", Content, mediaid, openid);//再发送语音
                    }
                    else if (type == 3)
                    {
                        data = CustomerMsg_Send.SendWxMsg(comid, lockuserweixin, type, Content, "", mediaid, openid);//再发送语音
                    }

                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    OOuterClass foo = ser.Deserialize<OOuterClass>(data);
                    int type_temp = foo.type;
                    string msg = foo.msg;

                    //客服通道发送失败,发送消息模板，或绑定用户更改
                    if (type_temp == 1)
                    {
                        //给顾问发送消息模板
                        WxMessageLogData messagelogdata = new WxMessageLogData();
                        new Weixin_tmplmsgManage().WxTmplMsg_SubscribeActReward(comid, lockuserweixin, Content + "\\n", "消息回复", DateTime.Now.ToString(), "");
                        WxMessageLog messagelog = new WxMessageLog();
                        messagelog.Comid = comid;
                        messagelog.Weixin = lockuserweixin;
                        var messageedit = messagelogdata.EditWxMessageLog(messagelog);//插入日志

                    }

                    //更新绑定用户的最后通话时间
                    var locachannel = channledata.WxMessageLockUserTime(userchannleid, lockuserweixin);
                }
                else
                {
                    //顾问（当成一个用户）给顾问发送
                    if (basic.Weixintype == 2 || basic.Weixintype == 4)
                    {
                        //查询渠道，如果自动分配渠道并同时发送消息
                        var channelid_temp = CustomerMsg_Send.GetFenpeiChannel(comid, openid, 0);
                        var channleinfo = channledata.GetChannelDetail(channelid_temp);
                        int workday = 0;//工作日，如果是0则非工作日，1为工作日


                        if (channleinfo != null)
                        {
                            //读取渠道关联会员信息，获取微信号
                            var crminfo = crmdata.GetB2bCrmByPhone(channleinfo.Mobile, comid);
                            if (crminfo != null)//必须查询到渠道绑定的账户及时渠道设定为工作日
                            {
                                if (crminfo.Weixin != "")
                                {
                                    var firstsuoding = 1;//当用户第一次锁定时同事摇发送消息模板
                                    if (channleinfo.Lockuserweixin == "")
                                    {
                                        firstsuoding = 2;
                                    }

                                    //对渠锁定用户和发起微信核对如果 不同则绑定新用户，如果相同更新最后通话时间
                                    if (channleinfo.Lockuserweixin != openid)
                                    {
                                        if (channleinfo.Lockuser == 0)//非锁死，当=1是锁死
                                        {
                                            //顾问绑定回复用户，多用户咨询未做处理
                                            var locachannel = channledata.WxMessageLockUser(channelid, openid);

                                            //给渠道发送锁定用户通知
                                            if (channleinfo.Lockuserweixin == "")
                                            {
                                                string data_suodingtongzhi = CustomerMsg_Send.SendWxMsg(comid, crminfo.Weixin, 1, "", "您现在锁定用户为 ：" + username + "(" + userid + ")" + "，\n此时发送的文字或语音都会发送到此会员微信上.\n------------------------", "", basic.Weixinno);
                                            }
                                            else
                                            {
                                                string data_suodingtongzhi = CustomerMsg_Send.SendWxMsg(comid, crminfo.Weixin, 1, "", "您现在锁定用户为 ：" + username + "(" + userid + ")" + "，\n此时发送的文字或语音都会发送到此会员微信上.如需要对其他客户回复请 输入如 R100001 (R用户ID)切换锁定用户 \n------------------------", "", basic.Weixinno);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //更新绑定用户的最后通话时间
                                        var locachannel = channledata.WxMessageLockUserTime(channelid, openid);
                                    }




                                    //给顾问发送
                                    Content = username + DateTime.Now.ToString("hh:mm") + "\n" + Content;
                                    var xingming = username + DateTime.Now.ToString("hh:mm");

                                    string url = "\n\n<a  href='http://shop" + basic.Comid + ".etown.cn/weixin/wxzixunreply.aspx?userweixin=" + openid + "&guwenweixin=" + crminfo.Weixin + "'>请点击回复用户咨询</a>\n";

                                    string data = CustomerMsg_Send.SendWxMsg(comid, crminfo.Weixin, type, "", Content + url, mediaid, openid);
                                    //用户->顾问的是语音信息，则需要把识别后的语音文字在通过客服接口给顾问发送过去
                                    if (type == 2)
                                    {
                                        //string url = "\n\n<a  href='http://shop" + basic.Comid + ".etown.cn/weixin/wxzixunreply.aspx?userweixin=" + openid + "&guwenweixin=" + crminfo.Weixin + "'>请点击回复用户咨询</a>\n";
                                        string data2 = CustomerMsg_Send.SendWxMsg(comid, crminfo.Weixin, 1, "", Content + url, "", openid);
                                    }

                                    JavaScriptSerializer ser = new JavaScriptSerializer();
                                    OOuterClass foo = ser.Deserialize<OOuterClass>(data);
                                    int type_temp = foo.type;
                                    string msg = foo.msg;

                                    //客服通道发送失败,发送消息模板，或绑定用户更改
                                    //认证服务号才可发送模板消息
                                    if (basic.Weixintype == 4)
                                    {
                                        if (type_temp == 1 || firstsuoding == 2)
                                        {
                                            //给顾问发送消息模板
                                            WxMessageLogData messagelogdata = new WxMessageLogData();
                                            new Weixin_tmplmsgManage().WxTmplMsg_SubscribeActReward(comid, crminfo.Weixin, "通知： " + channleinfo.Name + " 你好, \\n有客户 " + username + "(" + userid + ")" + " 在微信上向你留言咨询，请微信上尽快回复。\\n", "客户咨询", DateTime.Now.ToString(), "咨询内容:" + Content);
                                            WxMessageLog messagelog = new WxMessageLog();
                                            messagelog.Comid = comid;
                                            messagelog.Weixin = crminfo.Weixin;
                                            var messageedit = messagelogdata.EditWxMessageLog(messagelog);//插入日志
                                            return messageedit;
                                        }
                                    }
                                }
                                else
                                {
                                    //客户所绑定的渠道未 绑定微信号，此时发送短信提醒。
                                    string msg = "";//返回错误
                                    int sendback = SendSmsHelper.SendSms(channleinfo.Mobile, "您好，您的客户 " + username + " 在微信上向您咨询,请您关注公司微信号并绑定此手机,就可以通过微信给客户回复了。此条客户咨询请登陆后台进行查看并回复。", comid, out msg);

                                }
                            }
                            else
                            {

                                //没找到渠道会员账户或非工作日,给客户发送
                                string backuser = "您选择的服务顾问 " + channleinfo.Name + " 不在线，请直接电话联系或选择其他服务顾问咨询.shop" + comid + ".etown.cn/h5/peoplelist.aspx";
                                var data_temp = CustomerMsg_Send.SendWxMsg(comid, openid, 1, "", backuser, "", basic.Weixinno);

                            }


                        }
                        else
                        {
                            //当没有锁定的客户则返回顾问，错误提示
                            string data = CustomerMsg_Send.SendWxMsg(comid, openid, 1, "", "您的账户检测为顾问账户，尚未锁定咨询客户，如果您要和其他顾问通话，请在全部顾问选择要通话的顾问惊醒绑定！", "", basic.Weixinno);

                        }
                    }
                    else
                    {

                        //当没有锁定的客户则返回顾问，错误提示
                        string data = CustomerMsg_Send.SendWxMsg(comid, openid, 1, "", "您的账户检测为顾问账户，尚未锁定咨询客户，您的微信账户未认证不支持微信咨询！", "", basic.Weixinno);
                    }
                }
                #endregion
            }
            else
            {
                #region 客户--------------发送顾问
                //认证订阅号/认证服务号才可发送客服信息
                if (basic.Weixintype == 2 || basic.Weixintype == 4)
                {
                    //自动分配渠道，如果自动分配渠道并同时发送消息
                    channelid = CustomerMsg_Send.AutoFenpeiChannel(comid, openid, 0);


                    //锁定客户，给顾问发送
                    if (channelid != 0)
                    {
                        var channleinfo = channledata.GetChannelDetail(channelid);
                        int workday = 0;//工作日，如果是0则非工作日，1为工作日


                        if (channleinfo != null)
                        {
                            //读取员工账户
                            B2bCompanyManagerUserData managerdata = new B2bCompanyManagerUserData();
                            B2b_company_manageuser manageruser = managerdata.GetCompanyUserByPhone(channleinfo.Mobile, comid);
                            if (manageruser != null)
                            {
                                workday = crmdata.WorkDay(manageruser.Workdays);//返回工作日
                            }




                            //读取渠道关联会员信息，获取微信号
                            var crminfo = crmdata.GetB2bCrmByPhone(channleinfo.Mobile, comid);
                            if (crminfo != null && workday == 1)//必须查询到渠道绑定的账户及时渠道设定为工作日
                            {
                                if (crminfo.Weixin != "")
                                {
                                    var firstsuoding = 1;//当用户第一次锁定时同事摇发送消息模板
                                    if (channleinfo.Lockuserweixin == "")
                                    {
                                        firstsuoding = 2;
                                    }

                                    //对渠锁定用户和发起微信核对如果 不同则绑定新用户，如果相同更新最后通话时间
                                    if (channleinfo.Lockuserweixin != openid)
                                    {
                                        if (channleinfo.Lockuser == 0)//非锁死，当=1是锁死
                                        {
                                            //顾问绑定回复用户，多用户咨询未做处理
                                            var locachannel = channledata.WxMessageLockUser(channelid, openid);

                                            //给渠道发送锁定用户通知
                                            if (channleinfo.Lockuserweixin == "")
                                            {
                                                string data_suodingtongzhi = CustomerMsg_Send.SendWxMsg(comid, crminfo.Weixin, 1, "", "您现在锁定用户为 ：" + username + "(" + userid + ")" + "，\n此时发送的文字或语音都会发送到此会员微信上.\n------------------------", "", basic.Weixinno);
                                            }
                                            else
                                            {
                                                string data_suodingtongzhi = CustomerMsg_Send.SendWxMsg(comid, crminfo.Weixin, 1, "", "您现在锁定用户为 ：" + username + "(" + userid + ")" + "，\n此时发送的文字或语音都会发送到此会员微信上.如需要对其他客户回复请 输入如 R100001 (R用户ID)切换锁定用户 \n------------------------", "", basic.Weixinno);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //更新绑定用户的最后通话时间
                                        var locachannel = channledata.WxMessageLockUserTime(channelid, openid);
                                    }




                                    //给顾问发送
                                    Content = username + DateTime.Now.ToString("hh:mm") + "\n" + Content;
                                    var xingming = username + DateTime.Now.ToString("hh:mm");

                                    string url = "\n\n<a  href='http://shop" + basic.Comid + ".etown.cn/weixin/wxzixunreply.aspx?userweixin=" + openid + "&guwenweixin=" + crminfo.Weixin + "'>请点击回复用户咨询</a>\n";

                                    string data = CustomerMsg_Send.SendWxMsg(comid, crminfo.Weixin, type, "", Content + url, mediaid, openid);
                                    //用户->顾问的是语音信息，则需要把识别后的语音文字在通过客服接口给顾问发送过去
                                    if (type == 2)
                                    {
                                        //string url = "\n\n<a  href='http://shop" + basic.Comid + ".etown.cn/weixin/wxzixunreply.aspx?userweixin=" + openid + "&guwenweixin=" + crminfo.Weixin + "'>请点击回复用户咨询</a>\n";
                                        string data2 = CustomerMsg_Send.SendWxMsg(comid, crminfo.Weixin, 1, "", Content + url, "", openid);
                                    }

                                    JavaScriptSerializer ser = new JavaScriptSerializer();
                                    OOuterClass foo = ser.Deserialize<OOuterClass>(data);
                                    int type_temp = foo.type;
                                    string msg = foo.msg;

                                    //客服通道发送失败,发送消息模板，或绑定用户更改
                                    //认证服务号才可发送模板消息
                                    if (basic.Weixintype == 4)
                                    {
                                        if (type_temp == 1 || firstsuoding == 2)
                                        {
                                            //给顾问发送消息模板
                                            WxMessageLogData messagelogdata = new WxMessageLogData();
                                            new Weixin_tmplmsgManage().WxTmplMsg_SubscribeActReward(comid, crminfo.Weixin, "通知： " + channleinfo.Name + " 你好, \\n有客户 " + username + "(" + userid + ")" + " 在微信上向你留言咨询，请微信上尽快回复。\\n", "客户咨询", DateTime.Now.ToString(), "咨询内容:" + Content);
                                            WxMessageLog messagelog = new WxMessageLog();
                                            messagelog.Comid = comid;
                                            messagelog.Weixin = crminfo.Weixin;
                                            var messageedit = messagelogdata.EditWxMessageLog(messagelog);//插入日志
                                            return messageedit;
                                        }
                                    }
                                }
                                else
                                {
                                    //客户所绑定的渠道未 绑定微信号，此时发送短信提醒。
                                    string msg = "";//返回错误
                                    int sendback = SendSmsHelper.SendSms(channleinfo.Mobile, "您好，您的客户 " + username + " 在微信上向您咨询,请您关注公司微信号并绑定此手机,就可以通过微信给客户回复了。此条客户咨询请登陆后台进行查看并回复。", comid, out msg);

                                }
                            }
                            else
                            {

                                //没找到渠道会员账户或非工作日,给客户发送
                                string backuser = "您选择的服务顾问 " + channleinfo.Name + " 不在线，请直接电话联系或选择其他服务顾问咨询.shop" + comid + ".etown.cn/h5/peoplelist.aspx";
                                var data = CustomerMsg_Send.SendWxMsg(comid, openid, 1, "", backuser, "", basic.Weixinno);

                            }
                        }
                    }
                    else
                    {

                        //string data = CustomerMsg_Send.SendWxMsg(comid, openid, 1, "", "尊敬的"+username+"，您尚未绑定咨询顾问，请点击我的顾问选择顾问！");
                    }
                }
                #endregion
            }
            return 0;

        }

        /// <summary>
        /// 顾问解锁客户
        /// </summary>
        /// <param name="channelid">渠道ID</param>
        /// <returns></returns>
        internal void WeixinMessageUnLock(string openid, int comid, WeiXinBasic basic)
        {
            var crmdata = new B2bCrmData();
            var channeldata = new MemberChannelData();


            int channelid = 0;
            var userinfo = crmdata.b2b_crmH5(openid, comid);
            if (userinfo != null)
            {
                var channleinfo = channeldata.GetPhoneComIdChannelDetail(userinfo.Phone, comid);
                if (channleinfo != null)
                {
                    channelid = channleinfo.Id;
                }

            }


            var jiesuo = channeldata.WxMessageUnLockUser(channelid);

            //给顾问发送信息，渠道ID不等于0
            if (openid != "")
            {

                var channleinfo = channeldata.GetChannelDetail(channelid);
                if (channleinfo != null)
                {   //读取渠道关联会员信息，获取微信号
                    var crminfo = crmdata.GetB2bCrmByPhone(channleinfo.Mobile, channleinfo.Com_id);
                    if (crminfo != null)
                    {
                        if (crminfo.Weixin != "")
                        {
                            //给渠道发送锁定用户通知
                            string data_suodingtongzhi = CustomerMsg_Send.SendWxMsg(channleinfo.Com_id, crminfo.Weixin, 1, "", "解锁成功,新用户咨询会自动发送到您微信中\n------------------------", "", basic.Weixinno);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 顾问解锁客户
        /// </summary>
        /// <param name="channelid">渠道ID</param>
        /// <returns></returns>
        internal void WeixinMessageDeadLock(string openid, int locktype, int comid, WeiXinBasic basic)
        {
            var crmdata = new B2bCrmData();
            var channeldata = new MemberChannelData();

            int channelid = 0;
            var userinfo = crmdata.b2b_crmH5(openid, comid);
            if (userinfo != null)
            {
                var channleinfo = channeldata.GetPhoneComIdChannelDetail(userinfo.Phone, comid);
                if (channleinfo != null)
                {
                    channelid = channleinfo.Id;
                }

            }

            var jiesuo = channeldata.WxMessageDeadLock(channelid, locktype);

            //渠道ID不等于0
            if (channelid != 0)
            {
                if (locktype == 1)
                {
                    //给渠道发送锁定用户通知
                    string data_suodingtongzhi = CustomerMsg_Send.SendWxMsg(comid, openid, 1, "", "您的账户设定为 “锁定”状态，只能手动切换用户（输入如：R10001）,当其他客户留言时，只显示不会影响你回复之前的客户。您也可以输入 “解除锁定” 恢复自动切换用户，您发送的信息发送给最后留言的客户。\n------------------------", "", basic.Weixinno);
                }
                else
                {
                    //给渠道发送锁定用户通知
                    string data_suodingtongzhi = CustomerMsg_Send.SendWxMsg(comid, openid, 1, "", "锁定状态解除，新用户咨询时，会自动绑定新用户，您发送的信息发送给最后留言的客户。\n------------------------", "", basic.Weixinno);
                }
            }
        }



        /// <summary>
        /// 切换用户
        /// </summary>
        /// <param name="openid">用户OPENID</param>
        /// <param name="channelid">渠道ID</param>
        /// <returns></returns>
        internal void WeixinMessageLock(string openid, string userid, int comid, WeiXinBasic basic)
        {

            var crmdata = new B2bCrmData();
            var channeldata = new MemberChannelData();

            int channelid = 0;
            int userid_temp = int.Parse(userid.Replace("r", "").Replace("R", ""));//去掉字母，获取用户id
            var useropenid = "";

            //获取用户姓名
            var username = "";
            var userinfo = crmdata.Readuser(userid_temp, comid);
            if (userinfo != null)
            {

                username = userinfo.Name;
                useropenid = userinfo.Weixin;
            }

            //获取渠道信息
            var userinfo_channel = crmdata.b2b_crmH5(openid, comid);
            if (userinfo_channel != null)
            {
                //判断用户类型（会员，还是渠道）
                if (userinfo_channel.Phone != "")
                {
                    var userchannleinfo = channeldata.GetPhoneComIdChannelDetail(userinfo_channel.Phone, comid);
                    if (userchannleinfo != null)
                    {
                        channelid = userchannleinfo.Id;//用户为渠道，获取渠道ID
                    }
                }
            }
            if (userinfo == null)
            { //当用户为空
                string data_suodingtongzhi = CustomerMsg_Send.SendWxMsg(comid, openid, 1, "", "没有查询到此用户.\n------------------------", "", basic.Weixinno);

            }
            else
            {
                //锁定会员并发送消息
                if (channelid != 0)
                {
                    //顾问锁定回复用户，多用户咨询未做处理
                    var locachannel = channeldata.WxMessageLockUser(channelid, useropenid);
                    //给渠道发送锁定用户通知
                    string data_suodingtongzhi = CustomerMsg_Send.SendWxMsg(comid, openid, 1, "", "您现在锁定用户为 ：" + username + "(" + userid_temp + ")" + "，\n此时发送的文字或语音都会发送到此会员微信上.\n------------------------", "", basic.Weixinno);

                }
            }


        }



        /// <summary>
        /// 客户解除绑定顾问
        /// </summary>
        /// <param name="channelid">openid</param>
        /// <returns></returns>
        internal void UseUnLockChannel(string openid, int comid, WeiXinBasic basic)
        {
            var crmdata = new B2bCrmData();
            var channeldata = new MemberChannelData();
            var channelname = "";
            int channelid = 0;
            var userinfo = crmdata.b2b_crmH5(openid, comid);
            if (userinfo != null)
            {
                if (userinfo.Phone != "")
                {
                    //判断用户是顾问还是客户
                    var userchannleinfo = channeldata.GetPhoneComIdChannelDetail(userinfo.Phone, comid);
                    if (userchannleinfo != null)
                    {
                        channelid = userchannleinfo.Id;
                    }
                }

                //读取客户的顾问信息
                var channleinfo = channeldata.GetChannelByOpenId(openid);
                if (channleinfo != null)
                {
                    channelname = channleinfo.Name;
                }

                //只针对客户操作，顾问操作无效
                if (channelid == 0)
                {
                    var jiechubangding = channeldata.UserUnlockChannel(userinfo.Idcard, comid);
                    //给渠道发送锁定用户通知
                    string data_suodingtongzhi = CustomerMsg_Send.SendWxMsg(comid, openid, 1, "", "您已经与服务顾问 " + channelname + "解除绑定关系，您在微信公号的留言将不会直接发给该服务顾问。您可以点击<我的服务顾问>来查看和选择其他顾问服务。", "", basic.Weixinno);
                }
            }

        }


        /// <summary>
        /// 客户解除绑定手机
        /// </summary>
        /// <param name="channelid">openid</param>
        /// <returns></returns>
        internal void UserUnLockPhne(string openid, int comid, WeiXinBasic basic)
        {
            var crmdata = new B2bCrmData();
            var userinfo = crmdata.b2b_crmH5(openid, comid);
            if (userinfo != null)
            {
                //只针对客户操作，顾问操作无效
                if (userinfo.Phone != "")
                {
                    var jiechubangding = crmdata.UserUnlockPhone(openid, comid);
                    //给申请用户发送消息
                    string data_suodingtongzhi = CustomerMsg_Send.SendWxMsg(comid, openid, 1, "", "您已经绑定的手机 " + userinfo.Phone + "解除绑定关系，您可以在微信中输入新的手机号,并短信验证。绑定新的手机了。", "", basic.Weixinno);
                }
                else
                {

                    //给申请用户发送消息
                    string data_suodingtongzhi = CustomerMsg_Send.SendWxMsg(comid, openid, 1, "", "您的账户未绑定手机，您可以在微信中输入新的手机号,并短信验证,绑定手机。", "", basic.Weixinno);

                }
            }



        }


    }
}