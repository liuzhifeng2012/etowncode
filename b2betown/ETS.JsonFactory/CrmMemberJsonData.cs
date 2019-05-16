using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using Newtonsoft.Json;
using ETS2.Common.Business;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Member.Service.MemberService.Data;
using ETS2.Member.Service.MemberService.Model.Enum;
using ETS2.Member.Service.MemberService.Data.InternalData;
using ETS2.WeiXin.Service.WeiXinService.Model;
using System.Web;
using ETS2.WeiXin.Service.WeiXinService.Data;
using System.Collections;
using ETS2.WeiXin.Service.WinXinService.BLL;
using ETS.Data.SqlHelper;
using System.Data.SqlClient;
using ETS.Framework;

namespace ETS.JsonFactory
{
    /// <summary>
    /// 易城系统会员操作
    /// </summary>
    public class CrmMemberJsonData
    {
        internal static string InsertOrUpdate(B2b_crm crm)
        {
            try
            {
                var crmdata = new B2bCrmData();
                var crmid = crmdata.InsertOrUpdate(crm);

                return JsonConvert.SerializeObject(new { type = 100, msg = crmid });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


        #region 判断卡号有效性
        public static string GetCard(string card, int comid)
        {
            var pro = "";
            var cardtype = "";
            try
            {
                bool isNum = Domain_def.RegexValidate("^[0-9]*$", card);
                if (isNum)
                {
                    var crmdata = new B2bCrmData();
                    pro = crmdata.GetCard(card, comid);
                    if (pro == "此卡已经使用")
                    {
                        cardtype = "old";
                    }
                    else if (pro == "OK")
                    {
                        cardtype = "new";
                    }
                    else
                    {
                        cardtype = "err";
                    }
                }
                else
                {
                    pro = ("卡号错误");
                    cardtype = "err";
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = pro, cardtype = cardtype });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 判断渠道卡号有效性
        public static string GetChannelCard(string card, int comid)
        {
            var pro = "";
            var cardtype = "";
            try
            {
                bool isNum = Domain_def.RegexValidate("^[0-9]*$", card);
                if (isNum)
                {
                    var crmdata = new B2bCrmData();
                    pro = crmdata.GetChannelCard(card, comid);
                    if (pro == "此卡已经使用")
                    {
                        cardtype = "old";
                    }
                    else
                    {
                        cardtype = "err";
                    }

                }
                else
                {
                    pro = ("卡号错误");
                    cardtype = "err";
                }


                return JsonConvert.SerializeObject(new { type = 100, msg = pro, cardtype = cardtype });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 判断邮箱
        public static string GetEmail(string email, int comid)
        {
            var pro = "";
            try
            {
                bool isNum = Domain_def.RegexValidate("^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$", email);
                if (isNum)
                {
                    var crmdata = new B2bCrmData();
                    pro = crmdata.GetEmail(email, comid);
                }
                else
                {
                    pro = ("邮箱地址错误");
                }


                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 判断手机
        public static string GetPhone(string phone, int comid)
        {
            var pro = "";
            var phoneregi = "";
            try
            {
                bool isNum = Domain_def.RegexValidate("^1\\d{10}$", phone);
                if (isNum)
                {
                    var crmdata = new B2bCrmData();
                    pro = crmdata.GetPhone(phone, comid);
                    phoneregi = "old";
                }
                else
                {
                    pro = ("手机号码错误");
                    phoneregi = "err";
                }


                return JsonConvert.SerializeObject(new { type = 100, msg = pro, regitype = phoneregi });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 发送短信验证码
        public static string SendSmsLogin(int comid,string phone)
        {
            var pro = "";
            string smsstr = "你的手机动态密码为@randomcode,请妥善保存。有效期30分钟";
            string source = "用户短信登录";
            try
            {
                bool isNum = Domain_def.RegexValidate("^1\\d{10}$", phone);
                if (isNum)
                {
                    //暂时不查询账户，只要是 手机 就可以获取验证码登陆，登录时为其创建一个手机的 会员账户
                    //var crmdata = new B2bCrmData();
                    //pro = crmdata.GetPhone(phone, comid);
                    //if (pro == "OK") {//此处判断是此手机是否用过，如果没用过返回OK 也就是没有此手机账户 
                    //    return JsonConvert.SerializeObject(new { type = 1, msg = "账户信息错误" });
                    //}

                    //创建随机码
                    Random ra = new Random();
                    decimal code = ra.Next(100000, 999999);


                    //30分钟内发送短信的次数，超过3次，提醒“同一个手机号半小时内最多接收3次短信”
                    int sendnumin30 = new ValidCode_SendLogData().GetSendNumIn30ByMobile(phone, source);
                    if (sendnumin30 >= 3)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "同一个手机号半小时内最多接收3次短信!" });
                    }
                    //得到30分钟内最新的发送记录
                    ValidCode_SendLog sendrecord = new ValidCode_SendLogData().GetLasterLogByMobile(phone, source);


                    if (sendrecord != null)//重发验证码，设置验证码为第一次发送的验证码
                    {
                        //30分钟内最新的发送记录的验证码 第一次发送时间如果超过30分钟，则验证码过期需要生成新的验证码
                        DateTime firstsendtime = new ValidCode_SendLogData().GetFirstsendtimeByCode(phone, source, decimal.Parse(sendrecord.randomcode));
                        if (DateTime.Now < firstsendtime.AddMinutes(30))
                        {
                            code = decimal.Parse(sendrecord.randomcode);
                        }
                        else
                        {
                            sendrecord.send_serialnum = 0;
                        }
                    }
                    else //第一次发送验证码，设置验证码发送次数为0次
                    {
                        sendrecord = new ValidCode_SendLog
                        {
                            send_serialnum = 0,
                        };
                    }


                    //发送随机码
                    smsstr = smsstr.Replace("@randomcode", code.ToString());
                    string msg = "";
                    int sendback = SendSmsHelper.SendSms(phone, smsstr, comid, out msg);

                    //把发送记录录入数据库
                    ValidCode_SendLog log = new ValidCode_SendLog
                    {
                        id = 0,
                        mobile = phone,
                        randomcode = code.ToString(),
                        Content = smsstr,
                        send_serialnum = sendrecord.send_serialnum + 1,
                        sendtime = DateTime.Now,
                        returnmsg = msg,
                        source = source,
                        sendip = CommonFunc.GetRealIP()
                    };
                    int data = new ValidCode_SendLogData().InsertLog(log);

                    if (sendback > 0)
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = "发送成功" });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = msg });
                    }
                }
                else
                {
                    pro = ("手机号码错误");
                    return JsonConvert.SerializeObject(new { type = 1, msg = pro });
                }

                
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 判断key
        public static string Getkey(string key)
        {
            var pro = "";
            try
            {
                var crmdata = new B2bCrmData();
                pro = crmdata.Getkey(key);
                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 登陆
        public static string Login(string account, string pwd, int comid, out B2b_crm userinfo)
        {
            var pro = "";
            int accounttype = 0;
            try
            {
                //判断账户为数字，卡号为数字格式
                bool isNum = Domain_def.RegexValidate("^[0-9]*$", account);
                if (isNum)
                {
                    accounttype = 1;
                }
                else
                {
                    accounttype = 0;
                }



                var crmdata = new B2bCrmData();
                pro = crmdata.Login(account, accounttype, pwd, comid, out userinfo);

                return pro;
                //return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {

                userinfo = null;
                return ex.Message;
                //return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        //修改密码
        public static string UpMemberpass(int com_id, int id, string pass)
        {
            var pro = "";
            try
            {
                var crmdata = new B2bCrmData();
                pro = crmdata.UpMemberpass(com_id, id, pass);
                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        //修改手机
        public static string UpMemberphone(int com_id, int id, string phone)
        {
            var pro = "";
            try
            {
                var crmdata = new B2bCrmData();
                pro = crmdata.UpMemberphone(com_id, id, phone);
                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        //修改邮箱
        public static string UpMembermail(int com_id, int id, string mail)
        {
            var pro = "";
            try
            {
                var crmdata = new B2bCrmData();
                pro = crmdata.UpMemberphone(com_id, id, mail);
                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        //修改姓名
        public static string UpMembername(int com_id, int id, string name)
        {
            var pro = "";
            try
            {
                var crmdata = new B2bCrmData();
                pro = crmdata.UpMembername(com_id, id, name);
                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        //绑定或更新会员渠道
        public static string UpCrmChannelid(int com_id, int MasterId, string openid)
        {
            try
            {

                var crmdata = new B2bCrmData();

                var crmmodel = crmdata.GetB2bCrm(openid, com_id);
                if (crmmodel == null)
                {

                    return JsonConvert.SerializeObject(new { type = 1, msg = "会员信息错误" });
                }

                int upchannel = new MemberCardData().upCardcodeChannel(crmmodel.Idcard.ToString(), MasterId);

                //检测如果会员为渠道，则解绑其锁定客户
                int userchannelid=new MemberChannelcompanyData().getchannelidbyweixin(com_id,openid);
                if (userchannelid != 0) {
                    var updata = new MemberChannelData().WxMessageUnLockUser(userchannelid);//清楚 顾问绑定用户
                    
                }



                return JsonConvert.SerializeObject(new { type = 100, msg = upchannel });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string UpMembercard(int com_id, string phone, decimal card)
        {
            var pro = "";
            try
            {
                var crmdata = new B2bCrmData();
                bool isNum = Domain_def.RegexValidate("^[0-9]*$", card.ToString());
                if (isNum)
                {
                    pro = crmdata.GetCard(card.ToString(), com_id);
                    if (pro == "此卡已经使用")
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = "此卡已使用" });
                    }
                    else if (pro == "OK")
                    {
                        pro = crmdata.UpMembercard(com_id, phone, card);
                        return JsonConvert.SerializeObject(new { type = 100, msg = pro });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = "卡号错误" });
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = "卡号错误" });
                }

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        #region 微信通过密码登陆，并同时绑定
        public static string WeixinPassLogin(string phone, string weixin, string pwd, int comid, out B2b_crm userinfo)
        {
            var pro = "";
            try
            {

                var crmdata = new B2bCrmData();

                //判断微信号。是否使用过，如果已经注册的则不绑定微信号
                if (weixin != "")
                {
                    if (crmdata.GetWeixin(weixin, comid))//如果微信号已经使用则返回真
                    {
                        weixin = "";
                    }
                }


                pro = crmdata.WeixinPassLogin(phone, weixin, pwd, comid, out userinfo);

                return pro;
                //return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {

                userinfo = null;
                return ex.Message;
                //return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region kaler
        public static string Weixinopenid(string phone, string weixin, string pwd, int comid, out B2b_crm userinfo)
        {
            var pro = "";
            try
            {

                var crmdata = new B2bCrmData();

                pro = crmdata.WeixinPassLogin(phone, weixin, pwd, comid, out userinfo);

                //return pro;
                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {

                userinfo = null;
                //return ex.Message;
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 微信通过COOCKI登陆
        public static string WeixinCookieLogin(string accountid, string accountmd5, int comid, out B2b_crm userinfo)
        {
            var pro = "";
            try
            {

                var crmdata = new B2bCrmData();
                pro = crmdata.WeixinCookieLogin(accountid, accountmd5, comid, out userinfo);

                return pro;
                //return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {

                userinfo = null;
                return ex.Message;
                //return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 微信登陆
        public static string WeixinLogin(string openid, string weixinpass, int comid, out B2b_crm userinfo)
        {
            var pro = "";
            try
            {
                var crmdata = new B2bCrmData();
                pro = crmdata.WeixinLogin(openid, weixinpass, comid, out userinfo);

                return pro;
                //return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {

                userinfo = null;
                return ex.Message;
                //return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 读取用户信息
        public static string Readuser(int accountid, int comid)
        {

            try
            {
                if (accountid == 0 || comid == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "请您登陆" });
                }
                else
                {
                    var crmdata = new B2bCrmData();
                    var pro = crmdata.Readuser(accountid, comid);
                    return JsonConvert.SerializeObject(new { type = 100, msg = pro });
                }
            }
            catch (Exception ex)
            {
                // return ex.Message;
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 卡号注册绑定
        public static string RegCard(string card, string email, string password1, string name, string phone, int comid, string ChannelCard, string Sex)
        {
            var pro = "";
            int chid = 0;//web或weixin注册渠道ID
            var card_new = 0;//判断是否为新建立卡号
            try
            {

                //如果无卡号
                if (card == "")
                {
                    //新建立卡、并插入活动
                    card = MemberCardData.CreateECard(1, comid);
                    card_new = 1;//新建立卡为1
                }

                var crmcarddata = new B2bCrmData();

                //判断手机号是否使用
                var cardstate = crmcarddata.GetPhone(phone, comid);
                if (cardstate != "OK")
                {
                    return "err";
                }

                if (card_new == 0)
                {
                    //判断卡号是否已使用，只对非电子卡有效
                    cardstate = crmcarddata.GetCard(card, comid);
                    if (cardstate == "此卡已经使用")
                    {
                        return "err1";
                    }
                    if (cardstate == "没有此卡号")
                    {
                        return "err2";
                    }
                }



                //开卡给渠道中(开卡数量，第一次交易数量，金额)赋值，
                Member_Channel channel = new MemberChannelData().GetChannelDetailByCardNo(card);//根据卡号得到所属渠道的详细信息
                if (channel == null)//此卡没有发行渠道，并且有推荐人（门市开卡等）
                {

                    //开卡
                    var crmdata = new B2bCrmData();
                    pro = crmdata.RegCard(card, email, password1, name, phone, comid, Sex);

                    if (pro != "")
                    {
                        //插入关注赠送优惠券
                        int jifen_temp = 0;
                        var InputMoney = MemberCardData.AutoInputMoeny(Int32.Parse(pro), 3, comid, out jifen_temp);
                    }


                    //通过卡号，得到卡号ID
                    MemberCardData memberdate = new MemberCardData();
                    Member_Card cardmodel = memberdate.GetCardByCardNumber(decimal.Parse(card.ToString()));

                    //通过返回直接插入默认活动
                    if (cardmodel != null)
                    {
                        var actback = MemberActivityData.WebWeixinActIns(cardmodel.Id, 3, comid);
                    }


                    if (ChannelCard != "")//有推荐人 才执行以下操作
                    {
                        //直接查询推荐人卡号渠道
                        Member_Channel channelId = new MemberChannelData().GetSelfChannelDetailByCardNo(ChannelCard);//根据卡号得到所属渠道的详细信息
                        if (channelId != null)
                        {

                            //获得渠道号(web,weixin)，
                            MemberChannelData channedate = new MemberChannelData();
                            Member_Channel Channelmodel = channedate.GetChannelWebWeixin("web", comid);
                            if (Channelmodel != null)
                            {
                                chid = Channelmodel.Id;
                            }

                            //分配客服，第一次门市开卡，当没有发行渠道时，分配客服为渠道，但不给返利
                            string channelreg = new MemberChannelData().RegCardChannel_Server_Auto(card, channelId.Id, channelId.Id, comid);//根据卡号得到所属渠道的详细信息

                            //根据卡号得到所属渠道的详细信息
                            Member_Channel channel2 = new MemberChannelData().GetChannelDetailByCardNo(card);

                            //给渠道表中 开卡数量和总金额赋值
                            channel2.Opencardnum = channel2.Opencardnum + 1;
                            channel2.Summoney = channel2.Summoney + channel2.RebateOpen;
                            var channeldata = new MemberChannelData().EditChannel(channel2);

                            //把返佣日志录入渠道返佣日志表
                            ChannelRebateLog channelrebatelog = new ChannelRebateLog()
                            {
                                Id = 0,
                                Channelid = channel2.Id,
                                Execdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                                Rebatemoney = channel2.RebateOpen,
                                Type = (int)ChannelRebateType.OpenCard,
                                Summoney = channel2.Summoney,
                                Remark = "开卡返佣" + channel2.RebateOpen + "元"
                            };
                            var channelrebatelogret = new ChannelRebateLogData().EditChannelRebateLog(channelrebatelog);


                        }
                    }
                    return pro;
                }
                else
                {
                    //开卡
                    var crmdata = new B2bCrmData();
                    pro = crmdata.RegCard(card, email, password1, name, phone, comid, Sex);

                    if (pro != "")
                    {
                        //插入关注赠送优惠券
                        int jifen_temp = 0;
                        var InputMoney = MemberCardData.AutoInputMoeny(Int32.Parse(pro), 3, comid, out jifen_temp);
                    }


                    //分配客服
                    if (ChannelCard != "")
                    {
                        Member_Channel channelId = new MemberChannelData().GetSelfChannelDetailByCardNo(ChannelCard);//根据卡号得到所属渠道的详细信息
                        if (channelId != null)
                        {
                            //当有推荐人则分配客服,已有发行渠道，渠道为空
                            string channelreg = new MemberChannelData().RegCardChannel_Server_Auto(card, 0, channelId.Id, comid);//根据卡号得到所属渠道的详细信息
                        }
                    }


                    //给渠道表中 开卡数量和总金额赋值
                    channel.Opencardnum = channel.Opencardnum + 1;
                    channel.Summoney = channel.Summoney + channel.RebateOpen;
                    var channeldata = new MemberChannelData().EditChannel(channel);

                    //把返佣日志录入渠道返佣日志表
                    ChannelRebateLog channelrebatelog = new ChannelRebateLog()
                    {
                        Id = 0,
                        Channelid = channel.Id,
                        Execdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        Rebatemoney = channel.RebateOpen,
                        Type = (int)ChannelRebateType.OpenCard,
                        Summoney = channel.Summoney,
                        Remark = "开卡返佣" + channel.RebateOpen + "元"
                    };
                    var channelrebatelogret = new ChannelRebateLogData().EditChannelRebateLog(channelrebatelog);


                    return pro;
                }
                //return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                // return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                return "0";
                throw;
            }
        }
        #endregion


        #region 用户注册
        public static string UsereRegCard(string cardid, string email, string password1, string name, string phone, int comid, string sex)
        {
            var pro = 0;
            var channelid = 0;
            try
            {
                var crmcarddata = new B2bCrmData();
                var cardstate = crmcarddata.GetPhone(phone, comid);
                if (cardstate != "OK")
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "手机已使用" });
                }
                var ec = crmcarddata.GetEmail(email, comid);
                if (ec != "OK")
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "邮箱已使用" });
                }
                if (cardid == "")
                {
                    //新建立卡、并插入活动
                    cardid = MemberCardData.CreateECard(1, comid);
                }

                //获得渠道号(webweixin)
                MemberChannelData channedate = new MemberChannelData();
                Member_Channel Channelmodel = channedate.GetChannelWebWeixin("web", comid);
                if (Channelmodel != null)
                {
                    channelid = Channelmodel.Id;
                }

                var crmdata = new B2bCrmData();
                pro = crmdata.UsereRegCard(cardid, email, password1, name, phone, comid, sex, channelid);
                if (pro != 0)
                {
                    //插入关注赠送优惠券
                    int jifen_temp = 0;
                    var InputMoney = MemberCardData.AutoInputMoeny(pro, 3, comid,out jifen_temp);
                    return JsonConvert.SerializeObject(new { type = 100, msg = "OK" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "注册失败" });
                }


            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message, });
                throw;
            }
        }
        #endregion

        #region 微信绑定卡号，可以不填写卡号为注册新卡
        public static string WeixinRegCard(string card, string openid, string name, string phone, int comid)
        {
            var pro = "";
            var cardstate = "";
            int channelid = 0;
            try
            {

                var crmcarddata = new B2bCrmData();
                //判断微信号。是否使用过，如果已经注册的则不绑定微信号
                if (openid != "")
                {
                    if (crmcarddata.GetWeixin(openid, comid))//如果微信号已经使用则返回真
                    {
                        openid = "";
                    }
                }


                //开卡给渠道中(开卡数量，第一次交易数量，金额)赋值
                if (card != "")
                {
                    //判断卡号是否有效
                    cardstate = crmcarddata.GetCard(card, comid);
                    if (cardstate == "此卡已经使用")
                    {
                        return "err1";
                    }
                    if (cardstate == "没有此卡号")
                    {
                        return "err2";
                    }

                    cardstate = crmcarddata.GetPhone(phone, comid);
                    if (cardstate != "OK")
                    {
                        return "err3";
                    }


                    Member_Channel channel = new MemberChannelData().GetChannelDetailByCardNo(card);//根据卡号得到所属渠道的详细信息

                    //渠道ID
                    channelid = channel.Id;


                    //给渠道表中 开卡数量和总金额赋值
                    channel.Opencardnum = channel.Opencardnum + 1;
                    channel.Summoney = channel.Summoney + channel.RebateOpen;
                    var channeldata = new MemberChannelData().EditChannel(channel);

                    //把返佣日志录入渠道返佣日志表
                    ChannelRebateLog channelrebatelog = new ChannelRebateLog()
                    {
                        Id = 0,
                        Channelid = channel.Id,
                        Execdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        Rebatemoney = channel.RebateOpen,
                        Type = (int)ChannelRebateType.OpenCard,
                        Summoney = channel.Summoney,
                        Remark = "开卡返佣" + channel.RebateOpen + "元"
                    };
                    var channelrebatelogret = new ChannelRebateLogData().EditChannelRebateLog(channelrebatelog);
                }
                else
                {
                    //新建立卡、并插入活动
                    card = MemberCardData.CreateECard(2, comid);

                    //判断手机是否用过
                    cardstate = crmcarddata.GetPhone(phone, comid);
                    if (cardstate != "OK")
                    {
                        return "err3";
                    }

                    //获得渠道号(web,weixin)
                    MemberChannelData channedate = new MemberChannelData();
                    Member_Channel Channelmodel = channedate.GetChannelWebWeixin("weixin", comid);
                    if (Channelmodel != null)
                    {
                        channelid = Channelmodel.Id;
                    }
                }

                //设置自动密码,为手机的后6位
                string password1 = phone.Substring(phone.Length - 6, 6);

                //微信开卡
                var crmdata = new B2bCrmData();
                pro = crmdata.WeixinRegCard(card, openid, password1, name, phone, comid, channelid);

                return pro;

                //return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                // return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                return "0";
                throw;
            }
        }
        #endregion


        #region 手机注册账户
        public static string RegAccount(string phone, string name, string password1, int comid)
        {
            var pro = "";
            var cardstate = "";
            var card = "";
            int channelid = 0;
            try
            {

                //开卡给渠道中(开卡数量，第一次交易数量，金额)赋值
                if (phone != "")
                {
                    //判断卡号是否有效
                    var crmcarddata = new B2bCrmData();
                    cardstate = crmcarddata.GetPhone(phone, comid);
                    if (cardstate != "OK")
                    {
                        return "err3";
                    }
                    //新建立卡、并插入活动
                    card = MemberCardData.CreateECard(2, comid);
                }
                else
                {
                    return "err1";
                }

                //获得渠道号(web,weixin)
                MemberChannelData channedate = new MemberChannelData();
                Member_Channel Channelmodel = channedate.GetChannelWebWeixin("weixin", comid);
                if (Channelmodel != null)
                {
                    channelid = Channelmodel.Id;
                }


                //手机版开卡
                var crmdata = new B2bCrmData();
                pro = crmdata.RegAccount(card, phone, name, password1, comid, channelid);


                //通过卡号，得到卡号ID
                MemberCardData memberdate = new MemberCardData();
                Member_Card cardmodel = memberdate.GetCardByCardNumber(decimal.Parse(card.ToString()));

                //通过返回直接插入默认活动
                if (cardmodel != null)
                {
                    var actback = MemberActivityData.WebWeixinActIns(cardmodel.Id, 4, comid);
                }

                return pro;

                //return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                // return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                return "0";
                throw;
            }
        }
        #endregion


        public static string insert_res(int id, int comid, int number, string name, decimal phone, DateTime datetime)
        {
            try
            {
                var wxmater = new WxMaterialData().GetWxMaterial(id);
                Reservation model = new Reservation();
                model.WxMaterialid = id;
                model.Comid = comid;
                model.Name = name;
                if (wxmater != null)
                {
                    model.Titile = wxmater.Title;
                }
                model.Number = number;
                model.Rstatic1 = 1;
                model.Rdate1 = DateTime.Now;
                model.Submit_name = "";
                model.Ip = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                model.Phone = phone;
                model.Resdate = datetime;

                //新添酒店后新添加的字段
                model.Checkoutdate = datetime;
                model.Roomtypeid = 0;
                model.Totalprice = 0;
                model.Lastercheckintime = "";
                model.Subdate = DateTime.Now;
                var list = new WxMaterialData().insert_Res(model);
                if (list != 0)
                {
                    if (wxmater != null)
                    {
                        var price = decimal.Parse((wxmater.Price * number).ToString());
                        SendSmsHelper.GetMember_sms(phone.ToString(), name, number.ToString(), wxmater.Title.ToString(), price, "预订短信", comid);
                    }
                }
                return JsonConvert.SerializeObject(new { type = 10, msg = list });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 0, msg = ex.Message });
            }
        }

        #region 预订加载明细列表
        public static string ResLoadingList(string comid, int pageindex, int pagesize, int userid = 0)
        {
            var totalcount = 0;
            try
            {

                var prodata = new WxMaterialData();

                var list = prodata.Res_LoadingList(comid, pageindex, pagesize, out totalcount, userid);

                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 ID = pro.Id,
                                 Name = pro.Name,
                                 Phone = pro.Phone,
                                 Title = pro.Titile,
                                 Number = pro.Number,
                                 Submit_name = pro.Submit_name,
                                 Rstatic = pro.Rstatic1 == 1 ? "未处理" : "已处理",
                                 Datetime = pro.Rdate1,
                                 Ip = pro.Ip,
                                 Resdate = pro.Resdate,
                                 Money = pro.Number * (prodata.GetWxMaterial(pro.WxMaterialid) == null ? 0 : prodata.GetWxMaterial(pro.WxMaterialid).Price)
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        public static string ResSearchList(string comid, int pageindex, int pagesize, string key)
        {
            var totalcount = 0;
            bool isNum = Domain_def.RegexValidate("^[0-9]*$", key);
            try
            {

                var prodata = new WxMaterialData();
                var list = prodata.ResSearchList(comid, pageindex, pagesize, key, isNum, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 ID = pro.Id,
                                 Name = pro.Name,
                                 Phone = pro.Phone,
                                 Title = pro.Titile,
                                 Number = pro.Number,
                                 Submit_name = pro.Submit_name,
                                 Rstatic = pro.Rstatic1 == 1 ? "未处理" : "已处理",
                                 Datetime = pro.Rdate1,
                                 Ip = pro.Ip,
                                 Resdate = pro.Resdate,
                                 Money = pro.Number * (prodata.GetWxMaterial(pro.WxMaterialid) == null ? 0 : prodata.GetWxMaterial(pro.WxMaterialid).Price)
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        //确认预订
        public static string upRes(int id, int comid, int userid, string beiz)
        {
            try
            {
                var prodata = new WxMaterialData();
                B2b_company_manageuser user = UserHelper.CurrentUser();
                Reservation quren = new Reservation()
                {
                    Id = id,
                    Submit_name = user.Accounts,
                    Rstatic1 = 2,
                    Comid = comid,
                    Ip = System.Web.HttpContext.Current.Request.UserHostAddress,
                    Remarks = beiz

                };
                var pro = prodata.upRes(quren);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string phoneLogin(string phone, int comid, out B2b_crm userinfo)
        {
            string pro = "";
            try
            {
                var crmdata = new B2bCrmData().GetSjKeHu(phone, comid);
                userinfo = crmdata;
                if (crmdata != null)
                {
                    pro = "OK";
                }
                return pro;
            }
            catch (Exception ex)
            {
                userinfo = null;
                return ex.Message;
                throw;
            }
        }



        public static string GetMenshisByComid(int comid)
        {
            if (comid == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "传递参数失败" });
            }
            else
            {
                List<Member_Channel_company> list = new MemberChannelcompanyData().GetMenshiByComid(comid);
                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
        }

        /// <summary>
        /// 得到员工的全部信息 包括所在公司/门市的信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static string GetCompanyUser(int userid)
        {
            if (userid == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "传递参数失败" });
            }
            else
            {
                B2b_company_manageuser model = new B2bCompanyManagerUserData().GetCompanyUser(userid);
                List<B2b_company_manageuser> list = new List<B2b_company_manageuser>();
                list.Add(model);

                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Com_id = pro.Com_id,
                                 Accounts = pro.Accounts,
                                 Passwords = pro.Passwords,
                                 Atype = pro.Atype,
                                 Employeename = pro.Employeename,
                                 Job = pro.Job,
                                 Tel = pro.Tel,
                                 Employeestate = pro.Employeestate,
                                 Createuserid = pro.Createuserid,
                                 Channelcompanyid = pro.Channelcompanyid,
                                 Channelsource = pro.Channelsource,
                                 ChannelCompanyName = new MemberChannelcompanyData().GetChannelCompanyNameById(pro.Channelcompanyid),
                                 CompanyName = new B2bCompanyData().GetCompanyNameById(pro.Com_id)
                             };

                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
        }



        public static string GetWxMemberCountry()
        {
            IList<string> list = new WxMemberBasicData().GetWxMemberCountry();
            if (list != null)
            {
                if (list.Count > 0)
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = list });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "获取列表为空" });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "获取列表为空" });
            }
        }

        public static string GetWxMemberProvince(string country)
        {
            if (country == "")
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "传递参数失败" });
            }
            else
            {
                IList<string> list = new WxMemberBasicData().GetWxMemberProvince(country);

                if (list != null)
                {
                    if (list.Count > 0)
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = list });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "获取列表为空" });
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "获取列表为空" });
                }
            }
        }

        public static string GetWxMemberCity(string province)
        {

            if (province == "")
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "传递参数失败" });
            }
            else
            {
                IList<string> list = new WxMemberBasicData().GetWxMemberCity(province);
                if (list != null)
                {
                    if (list.Count > 0)
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = list });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "获取列表为空" });
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "获取列表为空" });
                }
            }
        }

        public static string GetIndustryById(int id)
        {
            if (id == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "传递参数失败" });
            }
            else
            {
                B2b_companyindustry model = new B2b_companyindustryData().GetIndustryById(id);

                return JsonConvert.SerializeObject(new { type = 100, msg = model });

            }
        }

        public static string EditComIndustry(int id, string name, string remark)
        {
            int result = new B2b_companyindustryData().EditComIndustry(id, name, remark);
            if (result > 0)
            {

                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = result });
            }
        }

        public static string Delindustry(int id)
        {
            if (id == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "传递参数失败" });
            }
            else
            {
                int result = new B2b_companyindustryData().Delindustry(id);

                return JsonConvert.SerializeObject(new { type = 100, msg = result });

            }
        }



        public static string Deltagtype(int id)
        {
            if (id == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "传递参数失败" });
            }
            else
            {
                int result = new B2b_interesttagtypeData().Deltagtype(id);

                return JsonConvert.SerializeObject(new { type = 100, msg = "" });

            }
        }

        public static string GetTagTypeById(int id)
        {
            if (id == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "传递参数失败" });
            }
            else
            {
                B2b_interesttagtype model = new B2b_interesttagtypeData().GetTagTypeById(id);

                return JsonConvert.SerializeObject(new { type = 100, msg = model });

            }
        }

        public static string EditComTagType(int id, string name, string remark, int industryid)
        {
            int result = new B2b_interesttagtypeData().EditComTagType(id, name, remark, industryid);
            if (result > 0)
            {

                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = result });
            }
        }

        public static string GetIndustryList(int industryid)
        {
            int total = 0;
            IList<B2b_companyindustry> result = new B2b_companyindustryData().GetIndustryList(out total, industryid);

            return JsonConvert.SerializeObject(new { type = 100, msg = result });

        }

        public static string GetCompanyDetail(int comid = 0)
        {
            if (comid == 0)
            {
                B2b_company company = UserHelper.CurrentCompany;
                return JsonConvert.SerializeObject(new { type = 100, msg = company });
            }
            else
            {
                B2b_company company = new B2bCompanyData().GetCompanyBasicById(comid);
                return JsonConvert.SerializeObject(new { type = 100, msg = company });
            }
        }

        public static string GetTagListByIndustryid(int industryid, int comid = 0)
        {
            int secondtotal = 0;
            int total = 0;
            var secondlist = new B2b_interesttagtypeData().GetTagTypeList(industryid, out secondtotal);

            IEnumerable result = "";
            if (secondlist != null)
                result = from pro in secondlist
                         select new
                         {
                             Id = pro.Id,
                             Typename = pro.Typename,
                             Remark = pro.Remark,
                             Createtime = pro.Createtime,
                             Industryid = pro.Industryid,
                             Isselfdefined = pro.Isselfdefined,
                             B2binteresttag = new B2b_interesttagData().GetTagListByTypeid(out total, pro.Id, comid, "0,1")
                         };


            return JsonConvert.SerializeObject(new { type = 100, msg = result });
        }

        public static string GetTagListByTypeid(int typeid, int comid, string issystemadd)
        {
            int total = 0;
            IList<B2b_interesttag> result = new B2b_interesttagData().GetTagListByTypeid(out total, typeid, comid, issystemadd);

            return JsonConvert.SerializeObject(new { type = 100, msg = result, totalCount = total });
        }

        public static string GetTagById(int id)
        {
            B2b_interesttag model = new B2b_interesttagData().GetTagById(id);
            return JsonConvert.SerializeObject(new { type = 100, msg = model });
        }

        public static string EditTag(int id, string name, int tagtypeid, int issystemadd = 0, int comid = 0)
        {
            int result = new B2b_interesttagData().EditTag(id, name, tagtypeid, issystemadd, comid);
            return JsonConvert.SerializeObject(new { type = 100, msg = result });
        }

        public static string GetCrmInterest(int crmid)
        {
            int total = 0;
            IList<B2b_crm_interesttag> result = new B2b_crm_interesttagData().GetCrmInterest(out total, crmid);

            return JsonConvert.SerializeObject(new { type = 100, msg = result });
        }

        public static string EditCrmInterest(int crmid, string checkedstr)
        {
            int result = new B2b_crm_interesttagData().EditCrmInterest(crmid, checkedstr);
            return JsonConvert.SerializeObject(new { type = 100, msg = result });
        }

        public static string ChangeComType(int comid, int hangye)
        {
            int result = new B2bCompanyData().ChangeComType(comid, hangye);
            if (result > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = result });
            }
        }

        public static string Deltag(int tagid)
        {
            int result = new B2b_interesttagData().Deltag(tagid);
            return JsonConvert.SerializeObject(new { type = 100, msg = result });
        }

        public static string GetCrmGroupList(int comid, int pageindex, int pagesize)
        {
            int total = 0;
            IList<B2b_group> result = new B2b_groupData().GetCrmGroupList(out total, comid, pageindex, pagesize);

            return JsonConvert.SerializeObject(new { type = 100, msg = result });
        }


        public static string GetB2bGroupById(int groupid)
        {
            B2b_group result = new B2b_groupData().GetB2bgroup(groupid);

            return JsonConvert.SerializeObject(new { type = 100, msg = result });
        }

        public static string EditB2bGroup(int id, string name, int comid, int userid, string remark, DateTime createtime)
        {
            int result = new B2b_groupData().EditB2bGroup(id, name, comid, userid, remark, createtime);

            return JsonConvert.SerializeObject(new { type = 100, msg = result });
        }

        public static string Delb2bgroup(int groupid)
        {
            int result = new B2b_groupData().Delb2bgroup(groupid);

            return JsonConvert.SerializeObject(new { type = 100, msg = result });
        }

        public static string GetCompanyB2bgroup(int comid)
        {
            int total = 0;
            IList<B2b_group> result = new B2b_groupData().GetCompanyB2bgroup(out total, comid);

            return JsonConvert.SerializeObject(new { type = 100, msg = result, total = total });
        }

        public static string Changefenzu(int crmid, int groupid)
        {
            int result = new B2b_groupData().Changefenzu(crmid, groupid);

            return JsonConvert.SerializeObject(new { type = 100, msg = result });
        }

        public static string Getqunfaphone(int comid, string qunfanum = "-1")
        {
            int total = 0;
            List<string> list = new B2bCrmData().Getqunfaphone(comid, out total, qunfanum);

            string result = "";
            if (total > 0)
            {
                foreach (string s in list)
                {
                    if (s != "")
                    {
                        result += s + ",";
                    }
                }
                if (result.IndexOf(",") > 0)
                {
                    result = result.Substring(0, result.Length - 1);
                }
            }

            return JsonConvert.SerializeObject(new { type = 100, msg = result, total = total });
        }

        #region 给客户发送微信模板
        public static string KehuMessage(int comid, string weixin)
        {
            var pro = "";
            try
            {
                if (weixin != "")
                {
                    //页面请求时发送给客户顾问信息
                    CustomerMsg_Send.Sendweixinchient(weixin, comid);
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {

                //return ex.Message;
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }
        #endregion

        public static string getb2bcrmlevels(int comid)
        {
            //判断公司是否含有会员级别设置
            int totalcount = 0;
            List<B2bcrmlevels> result = new B2bcrmlevelsData().Getb2bcrmlevelsbycomid(comid, out totalcount);
            if (totalcount <= 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = result, totalcount = totalcount });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = result, totalcount = totalcount });
            }
        }

        public static string editb2bcrmlevels(int comid, string levelids, string crmlevels, string levelnames, string dengjifens, string tequans, string isavailables)
        {
            try
            {
                string[] arrlevelids = levelids.Split(',');
                string[] arrcrmlevels = crmlevels.Substring(0, crmlevels.Length - 1).Split(',');
                string[] arrlevelnames = levelnames.Substring(0, levelnames.Length - 1).Split(',');
                string[] arrdengjifens = dengjifens.Substring(0, dengjifens.Length - 1).Split(',');
                string[] arrtequans = tequans.Substring(0, tequans.Length - 1).Split(',');
                string[] arrisavailables = isavailables.Substring(0, isavailables.Length - 1).Split(',');

                for (int i = 0; i < 4; i++)
                {
                    string dengjifen_begin = arrdengjifens[i].Substring(0, arrdengjifens[i].IndexOf('-'));
                    string dengjifen_end = arrdengjifens[i].Substring(arrdengjifens[i].IndexOf('-') + 1);

                    if (dengjifen_end == "无上限")
                    {
                        dengjifen_end = "1000000000";
                    }

                    B2bcrmlevels m = new B2bcrmlevels()
                    {
                        id = int.Parse(arrlevelids[i].Trim()),
                        crmlevel = arrcrmlevels[i].Trim(),
                        levelname = arrlevelnames[i].Trim(),
                        dengjifen_begin = decimal.Parse(dengjifen_begin.Trim()),
                        dengjifen_end = decimal.Parse(dengjifen_end.Trim()),
                        tequan = arrtequans[i].Trim(),
                        isavailable = int.Parse(arrisavailables[i].Trim()),
                        com_id = comid
                    };

                    int n = new B2bcrmlevelsData().EditB2bCrmLevel(m);
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = "会员级别编辑成功" });
            }
            catch
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "会员级别编辑失败" });
            }
        }

        public static string GetChannelByMasterId(int masterid)
        {
            if (masterid == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "传参失败" });
            }

            Member_Channel m = new MemberChannelData().GetChannelByMasterId(masterid);
            if (m == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "获取渠道失败" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = m });
            }
        }
        /// <summary>
        /// 根据员工id得到渠道单位信息
        /// </summary>
        /// <param name="yy"></param>
        /// <returns></returns>
        public static string GetMemberChanelCompanyByUserid(int userid)
        {
            Member_Channel_company r = new MemberChannelcompanyData().GetMemberChanelCompanyByUserid(userid);
            if (r == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = r });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = r });
            }
        }




        public static string Editcrmposition(string usern, string usere, int comid, string openid)
        {
            SqlHelper sqlhelper = new SqlHelper();
            sqlhelper.BeginTrancation();
            try
            {
                SqlCommand cmd = new SqlCommand();

                //会员位置表：判断是否保存过位置信息，a.保存过修改b.没有保存过添加
                string sql1 = "select count(1) from b2b_crm_location  where weixin='" + openid + "' and comid=" + comid;
                cmd = sqlhelper.PrepareTextSqlCommand(sql1);
                object o = cmd.ExecuteScalar();
                if (int.Parse(o.ToString()) > 0)
                {
                    string sql2 = "update b2b_crm_location set Latitude='" + usern + "' ,Longitude='" + usere + "' ,Precision='0',createtime='" +new  WeiXinManage().ConvertDateTimeInt(DateTime.Now) + "',createtimeformat='" + DateTime.Now + "'  where weixin='" + openid + "' and comid=" + comid;
                    cmd = sqlhelper.PrepareTextSqlCommand(sql2);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    string sql3 = "insert into b2b_crm_location (weixin,createtime,Latitude,Longitude,Precision,comid,createtimeformat) values('" + openid + "','" + new WeiXinManage().ConvertDateTimeInt(DateTime.Now) + "','" + usern + "','" + usere + "','0','" + comid + "','" + DateTime.Now + "')";
                    cmd = sqlhelper.PrepareTextSqlCommand(sql3);
                    cmd.ExecuteNonQuery();
                }


                sqlhelper.Commit();
                sqlhelper.Dispose();
                return JsonConvert.SerializeObject(new { type = 100, msg = "" });
            }
            catch (Exception e)
            {
                sqlhelper.Rollback();
                sqlhelper.Dispose();
                return JsonConvert.SerializeObject(new { type = 1, msg = ""});
            }
           
        }

    }
}
