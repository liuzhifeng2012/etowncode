using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Data;
using Newtonsoft.Json;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS.Framework;
using System.Xml;
using ETS2.Member.Service.MemberService.Data;
using ETS2.WeiXin.Service.WeiXinService.Model.Enum;
using ETS2.WeiXin.Service.WinXinService.BLL.WeiXinUploadDown;
using ETS2.PM.Service.PMService.Data;

namespace ETS2.WeiXin.Service.WinXinService.BLL
{
    public class CustomerMsg_Send
    {


        public static string SendWxMsg(int comid, string tousername, int type, string img, string txt, string mediaid  ,string fromusername )
        {
            //B2b_company_manageuser manageuser = UserHelper.CurrentUser();//客服信息（账户表B2b_company_manageuser）

            B2b_crm crm = new B2bCrmData().GetB2bCrmByWeiXin(tousername);
            if (crm == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "发送客服消息失败" });
            }
            //else
            //{
            //    if (crm.Whetherwxfocus == false)
            //    {
            //        return JsonConvert.SerializeObject(new { type = 1, msg = "微信用户已经取消了关注" });
            //    }
            //}


            WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(comid);
            if (basicc != null)
            {
                //获取凭证（首先判断数据库是否存在凭证以及是否过期，如过期，重新获取）
                WXAccessToken token = WeiXinManage.GetAccessToken(comid, basicc.AppId, basicc.AppSecret);
                //发送文本信息
                string err = "";//返回错误原因

                string createmenuurl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + token.ACCESS_TOKEN;
                string createmenutext1 = "";//微信菜单内容
                if (type == 1)//文本
                {
                    createmenutext1 = "{\"touser\":\"" + tousername + "\", \"msgtype\":\"text\",\"text\":{\"content\":\"" + txt + "\"}}";
                }
                if (type == 2)//语音
                {
                    if (mediaid != "")
                    {
                        createmenutext1 = "{\"touser\":\"" + tousername + "\", \"msgtype\":\"voice\",\"voice\":{\"media_id\":\"" + mediaid + "\"}}";
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "语音mediaid不可为空" });
                    }
                }
                if (type == 3)//图片
                {
                    if (mediaid != "")
                    {
                        createmenutext1 = "{\"touser\":\"" + tousername + "\", \"msgtype\":\"image\",\"image\":{\"media_id\":\"" + mediaid + "\"}}";
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "图片mediaid不可为空" });
                    }
                }

                if (err.Length > 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = err });
                }
                else
                {
                    string createmenuutret = new GetUrlData().HttpPost(createmenuurl, createmenutext1);

                    XmlDocument createselfmenudoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + createmenuutret + "}");

                    XmlElement createselfmenurootElement = createselfmenudoc.DocumentElement;
                    string createerrcode = createselfmenurootElement.SelectSingleNode("errcode").InnerText;
                    if (createerrcode != "0")
                    {

                        //短信提示 ，查询会员账户，并检测是否今天发送过短信，如果发送过就不发送了，每天发送一次
                        var crmdata = new B2bCrmData();
                        var crminfo = crmdata.b2b_crmH5(tousername, comid);
                        if (crminfo != null) {
                            if (crminfo.Phone != "")
                            {
                                var smstixing = new WxRequestXmlData().GetWxErr_sms_SendMsgList(comid, crminfo.Phone);
                                if (smstixing == 0)
                                {
                                    var smstixinginsert = new WxRequestXmlData().InsertWxErr_sms_SendMsgList(comid, crminfo.Phone);
                                    if (smstixinginsert != 0)
                                    {
                                        var cominfo = B2bCompanyData.GetCompany(comid);
                                        string comname = "";
                                        string weixincom = "";
                                        if (cominfo != null) {
                                            comname = cominfo.Com_name;
                                            weixincom = cominfo.B2bcompanyinfo.Weixinname;
                                        }

                                        ////发送短信
                                        //var smsmsg = "";
                                        //var smssendback = SendSmsHelper.SendSms(crminfo.Phone, "有一条给您发送微信消息接收失败 请关注" + comname + "微信账户： " + weixincom + " ，输入“我的消息”获取消息内容！", comid, out smsmsg);
                                    }
                                }
                            }
                        }

                        //发送客服信息，信息内容录入数据库
                        if (type == 1)//文本
                        {
                            RequestXML retRequestXML = new RequestXML();
                            retRequestXML.PostStr = createmenutext1;
                            retRequestXML.ToUserName = tousername;
                            //retRequestXML.FromUserName = new WeiXinBasicData().GetWxBasicByComId(comid).Weixinno.ConvertTo<string>("");
                            retRequestXML.FromUserName = fromusername;
                            retRequestXML.CreateTime = ConvertDateTimeInt(DateTime.Now).ToString();
                            retRequestXML.MsgType = "text";
                            retRequestXML.Content = txt;
                            retRequestXML.contentType = false;
                            retRequestXML.Comid = comid;
                            retRequestXML.Sendstate = 0;//发送状态为 未发送


                            int inswxexchangemsg = new WxRequestXmlData().EditWxRequestXmlLog(retRequestXML);
                        }
                        //发送客服信息，信息内容录入数据库
                        if (type == 2)//语音
                        {
                            RequestXML retRequestXML = new RequestXML();
                            retRequestXML.PostStr = createmenutext1;
                            retRequestXML.ToUserName = tousername;
                            //retRequestXML.FromUserName = new WeiXinBasicData().GetWxBasicByComId(comid).Weixinno.ConvertTo<string>("");
                            retRequestXML.FromUserName = fromusername;
                            retRequestXML.CreateTime = ConvertDateTimeInt(DateTime.Now).ToString();
                            retRequestXML.MsgType = "voice";
                            retRequestXML.Recognition = txt;
                            retRequestXML.contentType = false;
                            retRequestXML.Comid = comid;
                            retRequestXML.MediaId = mediaid;
                            retRequestXML.Sendstate = 0;//发送状态为 未发送


                            int inswxexchangemsg = new WxRequestXmlData().EditWxRequestXmlLog(retRequestXML);
                        }

                        if (type == 3)//图片
                        {
                            RequestXML retRequestXML = new RequestXML();
                            retRequestXML.PostStr = createmenutext1;
                            retRequestXML.ToUserName = tousername;
                            //retRequestXML.FromUserName = new WeiXinBasicData().GetWxBasicByComId(comid).Weixinno.ConvertTo<string>("");
                            retRequestXML.FromUserName = fromusername;
                            retRequestXML.CreateTime = ConvertDateTimeInt(DateTime.Now).ToString();
                            retRequestXML.MsgType = "image";
                            retRequestXML.PicUrl = img;
                            retRequestXML.contentType = false;
                            retRequestXML.Comid = comid;
                            retRequestXML.Sendstate = 0;//发送状态为 未发送

                            int inswxexchangemsg = new WxRequestXmlData().EditWxRequestXmlLog(retRequestXML);
                        }


                        return JsonConvert.SerializeObject(new { type = 1, msg = "回复客服信息失败" + createerrcode });

                    }
                    else
                    {
                        //发送客服信息，信息内容录入数据库
                        if (type == 1)//文本
                        {
                            RequestXML retRequestXML = new RequestXML();
                            retRequestXML.PostStr = createmenutext1;
                            retRequestXML.ToUserName = tousername;
                            //retRequestXML.FromUserName = new WeiXinBasicData().GetWxBasicByComId(comid).Weixinno.ConvertTo<string>("");
                            retRequestXML.FromUserName = fromusername;
                            retRequestXML.CreateTime = ConvertDateTimeInt(DateTime.Now).ToString();
                            retRequestXML.MsgType = "text";
                            retRequestXML.Content = txt;
                            retRequestXML.contentType = false;
                            retRequestXML.Comid = comid;
                            retRequestXML.Sendstate = 1;//发送状态为 未发送
                            //retRequestXML.Manageuserid = manageuser.Id;
                            //retRequestXML.Manageusername = manageuser.Accounts;


                            int inswxexchangemsg = new WxRequestXmlData().EditWxRequestXmlLog(retRequestXML);
                        }
                        //发送客服信息，信息内容录入数据库
                        if (type == 2)//语音
                        {
                            RequestXML retRequestXML = new RequestXML();
                            retRequestXML.PostStr = createmenutext1;
                            retRequestXML.ToUserName = tousername;
                            //retRequestXML.FromUserName = new WeiXinBasicData().GetWxBasicByComId(comid).Weixinno.ConvertTo<string>("");
                            retRequestXML.FromUserName = fromusername;
                            retRequestXML.CreateTime = ConvertDateTimeInt(DateTime.Now).ToString();
                            retRequestXML.MsgType = "voice";
                            retRequestXML.Recognition = txt;
                            retRequestXML.contentType = false;
                            retRequestXML.Comid = comid;
                            retRequestXML.MediaId = mediaid;
                            retRequestXML.Sendstate = 1;//发送状态为 未发送
                            //retRequestXML.Manageuserid = manageuser.Id;
                            //retRequestXML.Manageusername = manageuser.Accounts;


                            int inswxexchangemsg = new WxRequestXmlData().EditWxRequestXmlLog(retRequestXML);
                        }

                        if (type == 3)//图片
                        {
                            RequestXML retRequestXML = new RequestXML();
                            retRequestXML.PostStr = createmenutext1;
                            retRequestXML.ToUserName = tousername;
                            //retRequestXML.FromUserName = new WeiXinBasicData().GetWxBasicByComId(comid).Weixinno.ConvertTo<string>("");
                            retRequestXML.FromUserName = fromusername;
                            retRequestXML.CreateTime = ConvertDateTimeInt(DateTime.Now).ToString();
                            retRequestXML.MsgType = "image";
                            retRequestXML.PicUrl = img;
                            retRequestXML.contentType = false;
                            retRequestXML.Comid = comid;
                            retRequestXML.Sendstate = 1;//发送状态为 未发送
                            //retRequestXML.Manageuserid = manageuser.Id;
                            //retRequestXML.Manageusername = manageuser.Accounts;


                            int inswxexchangemsg = new WxRequestXmlData().EditWxRequestXmlLog(retRequestXML);
                        }
                        return JsonConvert.SerializeObject(new { type = 100, msg = "回复客服信息成功" });
                    }
                }

            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }

        /// <summary>

        /// datetime转换为aunixtime

        /// </summary>

        /// <param name="time"></param>

        /// <returns></returns>

        private static int ConvertDateTimeInt(System.DateTime time)
        {

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

            return (int)(time - startTime).TotalSeconds;

        }

        //当客户的渠道为0时，随机自动分配一个渠道编号（优先未锁定用户的）,sendtype= 当为1的时候 有渠道也发送，一般是点击我的顾问时候发送，0=则是留言的时候变更渠道发送
        public static int AutoFenpeiChannel(int comid, string openid, int sendtype = 1, int companyid = 0, int channleid = 0)
        {
            var crmdata = new B2bCrmData();
            MemberCardData carddata = new MemberCardData();
            var channeldata = new MemberChannelData();//读取渠道信息
            decimal idcard = 0;
            decimal channelid = 0;
            var userinfo_auto = carddata.GetMemberCardByOpenId(openid); ;//获取微信操作账户
            if (userinfo_auto != null)
            {
                idcard = userinfo_auto.Cardcode; ;
                channelid = userinfo_auto.IssueCard;//获取渠道ID
            }

            //判断对 微信注册，网站注册渠道进行归0
            var channeltype = channeldata.GetChannelDetail(int.Parse(channelid.ToString()));
            if (channeltype != null)
            {
                if (channeltype.Issuetype == 3 || channeltype.Issuetype == 4 || channeltype.Name == "默认渠道")
                {//如果渠道时微信注册或网站注册，渠道ID归0，下面自动重新分配有效渠道
                    channelid = 0;
                }
            }

            if (channelid == 0)
            {
                //查询在线渠道列表,
                int totalcount = 0;//在线数量
                var channellist = channeldata.GetChannelListByComid(comid, companyid, 2, out totalcount, channleid);//先查询渠道列表 锁定客户为空的

                if (channellist != null)
                {
                    if (totalcount > 0)
                    {
                        //随机选择一个渠道
                        Random rand = new Random();
                        var channel_temp = channellist[rand.Next(0, totalcount - 1)];
                        var channelid_temp = channel_temp.Id;

                        //绑定渠道
                        int upchannel = new MemberCardData().upCardcodeChannel(idcard.ToString(), channelid_temp);
                        channelid = channelid_temp;
                    }
                }
                //只有变更时才发送顾问信息
                Sendweixinchient(openid, comid);
            }
            else
            {
                //点击顾问也发送
                if (sendtype == 1)
                {
                    Sendweixinchient(openid, comid);
                }
            }
            return int.Parse(channelid.ToString());
        }


        //当客户的渠道为0时，随机自动分配一个渠道编号（优先未锁定用户的）,sendtype= 当为1的时候 有渠道也发送，一般是点击我的顾问时候发送，0=则是留言的时候变更渠道发送
        public static int GetFenpeiChannel(int comid, string openid, int sendtype = 1, int companyid = 0, int channleid = 0)
        {
            var crmdata = new B2bCrmData();
            MemberCardData carddata = new MemberCardData();
            var channeldata = new MemberChannelData();//读取渠道信息
            decimal idcard = 0;
            decimal channelid = 0;
            var userinfo_auto = carddata.GetMemberCardByOpenId(openid); ;//获取微信操作账户
            if (userinfo_auto != null)
            {
                idcard = userinfo_auto.Cardcode; ;
                channelid = userinfo_auto.IssueCard;//获取渠道ID
            }

            //判断对 微信注册，网站注册渠道进行归0
            var channeltype = channeldata.GetChannelDetail(int.Parse(channelid.ToString()));
            if (channeltype != null)
            {
                if (channeltype.Issuetype == 3 || channeltype.Issuetype == 4 || channeltype.Name == "默认渠道")
                {//如果渠道时微信注册或网站注册，渠道ID归0，下面自动重新分配有效渠道
                    channelid = 0;
                }
            }

            
            return int.Parse(channelid.ToString());
        }


        //给客户发送顾问的信息
        public static void Sendweixinchient(string openid, int comid)
        {
            WeiXinBasic basic = new WeiXinBasicData().GetWxBasicByComId(comid);


            string company = "";
            string channelname = "";
            string name = "";

            WxMessageLogData messagelogdata = new WxMessageLogData();
            //var messageinfo = messagelogdata.GetWxMessageLogSendTime(comid, openid);//查询2小时内是否给渠道发送过消息

            //if (messageinfo == 0)
            //{

            //会员
            MemberCardData carddata = new MemberCardData();
            var userdata = new B2bCrmData();

            var userinfo = userdata.GetB2bCrm(openid, comid);
            if (userinfo != null)
            {
                name = userinfo.Name;
            }

            var cardinfo = carddata.GetMemberCardByOpenId(openid);
            if (cardinfo != null)
            {
                //获取渠道
                var channeldata = new MemberChannelData();
                var channelinfo = channeldata.GetChannelDetail(int.Parse(cardinfo.IssueCard.ToString()));
                if (channelinfo != null)
                {
                    channelname = channelinfo.Name;//渠道名称

                    if (channelinfo.Companyid == 0)//内部渠道
                    {
                        B2bCompanyData comdata = new B2bCompanyData();
                        var cominfo = comdata.GetCompanyBasicById(comid);
                        if (cominfo != null)
                        {
                            company = cominfo.Com_name;
                        }
                    }
                    else
                    { //外部合作单位,调取合作单位名称
                        var channelcompanydata = new MemberChannelcompanyData();
                        var channelcominfo = channelcompanydata.GetChannelCompany(channelinfo.Companyid.ToString());
                        if (channelcominfo != null)
                        {
                            company = channelcominfo.Companyname;
                        }
                    }


                }
            }


            if (channelname != "" && channelname != "默认渠道" && channelname != "微信注册" && channelname != "网站注册")
            {
                //微信客服 文本消息
                string data = SendWxMsg(comid, openid, 1, "", name + "你好，我是您的服务顾问" + company + "的 " + channelname + "  ，\n请直接在微信上给我语音或文字留言，我会在手机微信上看到留言并很快回复。您不信？ 现在就试试…","",basic.Weixinno);

                //微信模板消息
                new Weixin_tmplmsgManage().WxTmplMsg_SubscribeActReward(comid, openid, name + "你好，我是您的服务顾问" + company + "的 " + channelname + "  ， \\n请直接在微信上给我语音或文字留言，我会在手机微信上看到留言并很快回复。您不信？ 现在就试试…\\n", "向《我的服务顾问：" + channelname + "》微信咨询", DateTime.Now.ToString(), "");
                WxMessageLog messagelog = new WxMessageLog();
                messagelog.Comid = comid;
                messagelog.Weixin = openid;
                var messageedit = messagelogdata.EditWxMessageLog(messagelog);//插入日志

                //顾问录入的 问候语音
                int isreplymediasuc = 0;//发送语音消息状态：0失败；1成功
              
                WXAccessToken m_accesstoken = WeiXinManage.GetAccessToken(basic.Comid, basic.AppId, basic.AppSecret);
                //根据用户微信得到其顾问微信，然后根据微信和标记得到最新的一条保存路径(注：已经上传过语音的即mediaid!="")
                Wxmedia_updownlog udlog = new Wxmedia_updownlogData().GetWxmedia_updownlog(openid, (int)Clientuptypemark.DownGreetVoice, basic.Comid);
                if (udlog == null)
                {
                    isreplymediasuc = 0;
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
                            operweixin = openid,
                            clientuptypemark = (int)Clientuptypemark.DownGreetVoice,//上传多媒体信息
                            comid = basic.Comid,
                            relativepath = udlog.relativepath,
                            txtcontent = "",
                            isfinish = 1
                        };
                        int uplogresult = new Wxmedia_updownlogData().Edituploadlog(uplog);
                        if (uplogresult == 0)
                        {
                            isreplymediasuc = 0;
                        }
                        else
                        {
                            //resxml = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[voice]]></MsgType><Voice><MediaId><![CDATA[" + media_id + "]]></MediaId></Voice></xml>";
                            isreplymediasuc = 1;
                            SendWxMsg(comid, openid, 2, "", "", media_id,basic.Weixinno);
                        }
                    }
                    else
                    {
                        isreplymediasuc = 0;
                    }
                }

                ////如果发送语音失败，则发送客服消息
                //if (isreplymediasuc == 0)
                //{
                //}

            }
            //}
        }



        //给顾问，客户，绑定人发送通知, sentype=1 给绑定顾问，=2给客户 =3给教练
        public static void SendWxkefumsg(int orderid, int sentype, string msg, int comid)
        {
            WeiXinBasic basic = new WeiXinBasicData().GetWxBasicByComId(comid);
            string openid = "";


            WxMessageLogData messagelogdata = new WxMessageLogData();

            B2bOrderData orderdate = new B2bOrderData();
            var orderinfo = orderdate.GetOrderById(orderid);

            if (orderinfo == null) {
                return ;
            }


            //会员

            MemberCardData carddata = new MemberCardData();
            var userdata = new B2bCrmData();

            if (sentype == 2)
            {
                var userinfo = userdata.Readuser(orderinfo.U_id, orderinfo.Comid);
                if (userinfo == null)
                {
                    return;
                }

                openid = userinfo.Weixin;

                if (openid != "")
                {
                    //微信客服 文本消息
                    string data = SendWxMsg(comid, openid, 1, "", msg, "", basic.Weixinno);
                }
            }

            if (sentype ==1 )
            {
                var b2bprodata = new B2bComProData();
                var proinfo = b2bprodata.GetProById(orderinfo.Pro_id.ToString(), orderinfo.Speciid, orderinfo.channelcoachid);
                if (proinfo == null) {
                    return;
                }

                if (proinfo.bookpro_bindphone != "") {
                    //var channeldata = new MemberChannelData();
                    //var channelinfo = channeldata.GetPhoneComIdChannelDetail(proinfo.bookpro_bindphone,comid);


                    //直接通过 手机查询用户
                     var userinfo_binding = userdata.GetSjKeHu(proinfo.bookpro_bindphone , orderinfo.Comid);
                     if (userinfo_binding == null)
                     {
                         return;
                     }
                     openid = userinfo_binding.Weixin;
                     if (openid != "")
                    {
                         //微信 文本消息
                         string data_binding = SendWxMsg(comid, openid, 1, "", msg, "", basic.Weixinno);
                    }
                }

            }

            //给教练
            if (sentype ==3)
            {
                var b2bprodata = new B2bComProData();
                var proinfo = b2bprodata.GetProById(orderinfo.Pro_id.ToString(), orderinfo.Speciid, orderinfo.channelcoachid);
                if (proinfo == null)
                {
                    return;
                }

                if (orderinfo.channelcoachid !=0)
                {
                    var channeldata = new MemberChannelData();
                    var channelinfo = MemberChannelData.GetChannelinfo(orderinfo.channelcoachid);

                    if (channelinfo != null)
                    {

                        //直接通过 手机查询用户
                        var userinfo_binding = userdata.GetSjKeHu(channelinfo.Mobile, orderinfo.Comid);
                        if (userinfo_binding == null)
                        {
                            return;
                        }
                        openid = userinfo_binding.Weixin;
                        if (openid != "")
                        {
                            //微信 文本消息
                            string data_binding = SendWxMsg(comid, openid, 1, "", msg, "", basic.Weixinno);
                        }
                    }

                }

            }

        }





    }
}
