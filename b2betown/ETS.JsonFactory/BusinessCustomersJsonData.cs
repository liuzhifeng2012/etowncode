using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Data;
using System.Collections;
using Newtonsoft.Json;
using ETS2.VAS.Service.VASService.Data;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Common.Business;
using ETS2.Member.Service.MemberService.Data;
using ETS2.Member.Service.MemberService.Data.InternalData;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Permision.Service.PermisionService.Model;
using ETS2.Permision.Service.PermisionService.Data;
using System.Data;
using System.Text.RegularExpressions;
using ETS2.WeiXin.Service.WinXinService.BLL;
using ETS.Framework;

namespace ETS.JsonFactory
{
    public class BusinessCustomersJsonData
    {
        public static string SJKeHuPageList(string comid, int pageindex, int pagesize, int userid)
        {
            var totalcount = 0;
            try
            {

                var list = new List<B2b_crm>();

                B2b_company_manageuser userr = B2bCompanyManagerUserData.GetUser(userid);
                if (userr != null)
                {
                    if (userr.Channelcompanyid == 0)//总公司账户，根据comid得到crm
                    {
                        list = new B2bCrmData().SJKeHuPageList(comid, pageindex, pagesize, out totalcount);
                    }
                    else //总公司下面渠道，渠道表+卡号表+会员表连接查询得到渠道下的crm
                    {
                        list = new B2bCrmData().SJKeHuPageList(comid, pageindex, pagesize, userr, out totalcount);
                    }



                    IEnumerable result = "";

                    if (list != null)

                        result = from pro in list
                                 select new
                                 {
                                     id = pro.Id,
                                     comid = pro.Com_id,
                                     phone = pro.Phone,
                                     registerdate = pro.Regidate,
                                     customername = pro.Name,
                                     imprest = pro.Imprest,
                                     integral = pro.Integral,
                                     idcard = pro.Idcard,
                                     email = pro.Email == null || pro.Email == "" ? "" : pro.Email,
                                     serverid = pro.Servercard,
                                     winxin = pro.Weixin == "" ? "" : "yes",
                                     channel = new MemberChannelcompanyData().UpCompanyById(pro.Idcard.ToString()),
                                     referrer = MemberChannelData.Upstring(pro.Idcard.ToString()) == null ? "" : MemberChannelData.Upstring(pro.Idcard.ToString()).Name.ToString(),

                                     WxHeadimgurl = pro.WxHeadimgurl,
                                     WxNickname = pro.WxNickname,
                                     WxProvince = pro.WxProvince,
                                     WxCity = pro.WxCity,
                                     WxSex = pro.WxSex == 0 ? "未知" : pro.WxSex == 1 ? "男" : "女"
                                 };

                    return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }


            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


        public static string fuwuPageList(string comid, int pageindex, int pagesize, int user)
        {
            var totalcount = 0;
            try
            {

                var prodata = new B2bCrmData();
                var list = prodata.fuwuPageList(comid, pageindex, pagesize, user, out totalcount);
                IEnumerable result = "";
                var memcompany = new MemberChannelcompanyData();
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 id = pro.Id,
                                 comid = pro.Com_id,
                                 phone = pro.Phone,
                                 registerdate = pro.Regidate,
                                 customername = pro.Name,
                                 imprest = pro.Imprest,
                                 integral = pro.Integral,
                                 idcard = pro.Idcard,
                                 email = pro.Email,
                                 serverid = pro.Servercard,
                                 winxin = pro.Weixin == null || pro.Weixin == "" ? "" : "yes",
                                 channel = memcompany.UpCompanyById(pro.Idcard.ToString()),
                                 referrer = MemberChannelData.Upstring(pro.Idcard.ToString()).Name.ToString()
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string SearchPageList(int userid, string comid, int pageindex, int pagesize, string key, string isactivate = "1", string iswxfocus = "0,1", string ishasweixin = "0,1", string ishasphone = "0,1")
        {
            var totalcount = 0;

            try
            {
                var list = new List<B2b_crm>();

                #region
                Sys_Group group = new Sys_GroupData().GetGroupByUserId(userid);
                //判断会员是否要求精确到渠道人
                bool crmIsAccurateToPerson = group.CrmIsAccurateToPerson;

                Member_Channel_company channelcom = new MemberChannelcompanyData().GetChannelCompanyByUserId(userid);
                if (channelcom == null)//总公司账户
                {
                    list = new B2bCrmData().SearchPageList(comid, pageindex, pagesize, key, out totalcount, isactivate, iswxfocus, ishasweixin, ishasphone);

                }
                else
                {
                    string channelcompanytype = "0";//渠道类型：0内部门店；1合作公司

                    if (channelcom.Issuetype == 0)//内部门市
                    {
                        list = new B2bCrmData().SearchPageList1(comid, pageindex, pagesize, key, channelcom.Id, out totalcount, isactivate, iswxfocus, ishasweixin, channelcompanytype, crmIsAccurateToPerson, userid, ishasphone);
                    }
                    else //合作公司
                    {
                        channelcompanytype = "1";
                        list = new B2bCrmData().SearchPageList1(comid, pageindex, pagesize, key, channelcom.Id, out totalcount, isactivate, iswxfocus, ishasweixin, channelcompanytype, crmIsAccurateToPerson, userid, ishasphone);

                    }
                }

                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 id = pro.Id,
                                 comid = pro.Com_id,
                                 phone = pro.Phone,
                                 registerdate = pro.Regidate,
                                 customername = pro.Name,
                                 imprest = pro.Imprest,
                                 integral = pro.Integral,
                                 idcard = pro.Idcard,
                                 email = pro.Email,
                                 serverid = pro.Servercard,
                                 isfocuswinxin = pro.Weixin == "" ? "" : pro.Whetherwxfocus == false ? "已取消" : "已关注",
                                 weixin = pro.Weixin,
                                 channel = new MemberChannelcompanyData().UpCompanyById(pro.Idcard.ToString()),
                                 referrer = MemberChannelData.SearchNamestring(pro.Idcard.ToString()),



                                 WxHeadimgurl = pro.WxHeadimgurl,
                                 WxNickname = pro.WxNickname.Replace("?", ""),
                                 WxProvince = pro.WxProvince,
                                 WxCity = pro.WxCity,
                                 WxSex = pro.WxSex == 0 ? "未知" : pro.WxSex == 1 ? "男" : "女",
                                 GroupId = pro.Groupid,
                                 GroupName = new B2b_groupData().GetB2bgroupName(pro.Groupid),

                                 IsCanReplyWx = IsIn48h(pro.Wxlastinteracttime),
                                 whetherwxfocus = pro.Whetherwxfocus
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });




                #endregion




            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        /// <summary>
        /// 判断时间是否在48h内
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private static int IsIn48h(DateTime dateTime)
        {
            TimeSpan ts = DateTime.Now - dateTime;
            if (ts.TotalSeconds <= 48 * 60 * 60)
            {
                return 1;
            }
            else
            {

                return 0;
            }
        }

        public static string Readuser(int id, int comid)
        {
            try
            {
                var prodata = new B2bCrmData();
                var list = prodata.Readuser(id, comid);
                var rename = MemberChannelData.GetChannelinfo(int.Parse(MemberCardData.GetCardNumber(list.Idcard).IssueCard.ToString()));
                var memcard = MemberChannelData.GetChannelinfo(int.Parse(MemberCardData.GetCardNumber(list.Idcard).ServerCard.ToString()));

                return JsonConvert.SerializeObject(new { type = 100, msg = list, Namechannl = rename, Mem = memcard });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        //初始会员密码
        public static string initialuser(int id, int comid)
        {
            var b2b = new B2bCrmData();
            var phone = b2b.Readuser(id, comid).Phone.ToString();
            var pass = phone.Substring(5, 6).ToString();
            var name = b2b.Readuser(id, comid).Name.ToString();
            var cardcode = b2b.Readuser(id, comid).Idcard.ToString();
            try
            {
                var prodata = new B2bCrmData();

                B2b_crm b2bcrm = new B2b_crm()
                {
                    Id = id,
                    Com_id = comid,
                    Password1 = pass
                };
                var pro = prodata.initialuser(b2bcrm);
                if (int.Parse(pro) == id)
                {
                    SendSmsHelper.GetMember_sms(phone, name, cardcode, pass, 0, "初始化密码", comid);
                }
                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


        public static string UpMember(int id, int comid, string Name, string Phone, string Sex, string Email, int Age)
        {
            try
            {
                //根据手机得到公司会员信息
                B2b_crm crm = new B2bCrmData().GetB2bCrmByPhone(comid, Phone);
                if (crm != null)
                {
                    if (crm.Id != id)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "当前手机已经绑定，请更换手机" });
                    }
                }


                var prodata = new B2bCrmData();

                //把返佣日志录入渠道返佣日志表
                B2b_crm b2bcrm = new B2b_crm()
                {
                    Id = id,
                    Name = Name,
                    Phone = Phone,
                    Sex = Sex,
                    Email = Email,
                    Age = Age,
                    Com_id = comid
                };
                var pro = prodata.UpMember(b2bcrm);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        //微信编辑个人信息
        public static string weiUpMember(int comid, decimal cardcode, string Name, string Phone, string Sex, DateTime Birthday, decimal code)
        {
            try
            {
                var prodata = new B2bCrmData();
                var pro = "";
                var list = Phone_code.code_info(decimal.Parse(Phone), comid);
                if (code == list.Code)
                {
                    //把返佣日志录入渠道返佣日志表
                    B2b_crm b2bcrm = new B2b_crm()
                    {
                        Idcard = cardcode,
                        Name = Name,
                        Phone = Phone,
                        Sex = Sex,
                        Birthday = Birthday,
                        Com_id = comid
                    };
                    pro = prodata.weiUpMember(b2bcrm);
                }
                else
                {
                    pro = "验证码错误";
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


        public static string ReadImprest(int id, int comid)
        {
            var totalcount = 0;
            try
            {

                MemberImprestData intdate = new MemberImprestData();
                var list = intdate.ReadImprest(id, comid, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 id = pro.Id,
                                 comid = pro.Comid,
                                 admin = pro.Admin,
                                 money = pro.Money,
                                 subdate = pro.Subdate,
                                 orderid = pro.OrderId,
                                 ordername = pro.OrderName
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        public static string SmsSend(int comid, string Smsphone, string Smstext)
        {
            string[] pohone_arr;
            string[] phone_name_arr;
            string phonestr = "";
            string namestr = "";
            string smsstr = "";
            int sendtype = 0;//默认是没有姓名群发，1为有姓名一个一个发
            string msg = "";
            int sendstate = 0;
            try
            {
                //当包含姓名通配符则一条一条发送
                if (Smstext.IndexOf("%name%") > 0)
                {
                    sendtype = 1;
                }

                //分割
                //Regex reg = new Regex(@"\r\n");
                //pohone_arr = reg.Split(Smsphone.Trim());

                pohone_arr = Smsphone.Split('\n');

                if (sendtype == 1)//安一条一条发送
                {
                    for (int i = 0; i < pohone_arr.Length; i++)
                    {
                        if (pohone_arr[i] != "")
                        {
                            phone_name_arr = pohone_arr[i].Split(' ');//空格区分
                            phonestr = phone_name_arr[0].Substring(0, 11);
                            if (phone_name_arr.Length >= 2)
                            {
                                namestr = phone_name_arr[1];
                            }
                            smsstr = Smstext.Replace("%name%", namestr);
                            sendstate = SendSmsHelper.SendSms(phonestr, smsstr, comid, out msg);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < pohone_arr.Length; i++)
                    {
                        if (pohone_arr[i] != "")
                        {
                            if (phonestr == "")
                            {
                                phonestr = pohone_arr[i].Substring(0, 11);
                            }
                            else
                            {
                                phonestr = phonestr + "," + pohone_arr[i].Substring(0, 11);
                            }

                            smsstr = Smstext;
                            sendstate = SendSmsHelper.SendSms(phonestr, smsstr, comid, out msg);
                        }
                    }
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = sendstate });

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }



        public static string Contentsend(int comid, string mobile, string name, string company, string address)
        {

            string Smstext = "";
            string msg = "";
            int sendstate = 0;
            try
            {

                Smstext = "商户咨询:" + name + "(" + mobile + ")" + "公司名称:" + company + " 地址:" + address;

                if (mobile != "" || name != "")
                {
                    sendstate = SendSmsHelper.SendSms("13511097178", Smstext, 101, out msg);
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = sendstate });

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string ReadIntegral(int id, int comid)
        {
            var totalcount = 0;
            try
            {
                MemberIntegralData intdate = new MemberIntegralData();
                var list = intdate.ReadIntegral(id, comid, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 id = pro.Id,
                                 comid = pro.Comid,
                                 admin = pro.Admin,
                                 money = pro.Money,
                                 subdate = pro.Subdate,
                                 orderid = pro.OrderId,
                                 ordername = pro.OrderName
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string WriteMoney(int id, int comid, string acttype, string money, decimal orderid, string ordername, string remark = "")
        {
            try
            {
                //获得操作用户
                B2b_company_manageuser user = UserHelper.CurrentUser();
                B2b_company company = UserHelper.CurrentCompany;
                string username = user.Accounts;
                //获得IP
                string addressIP = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()).GetValue(0).ToString();
                int pro = 0;

                //判断金额有效性
                bool isNum = Domain_def.RegexValidate("^[0-9]*$", money);
                if (!isNum)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "操作错误，金额只能包含数字" });
                }

                //充等积分
                if (acttype == "add_dengjifen")
                {
                    B2bcrm_dengjifenlog djflog = new B2bcrm_dengjifenlog
                    {
                        id = 0,
                        crmid = id,
                        dengjifen = decimal.Parse(money),
                        ptype = 1,
                        opertor = user.Id.ToString(),
                        opertime = DateTime.Now,
                        orderid = int.Parse(orderid.ToString()),
                        ordername = ordername,
                        remark = remark
                    };
                    

                    pro = new B2bCrmData().Adjust_dengjifen(djflog,id, comid, decimal.Parse(money));
                    if (pro== 0)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "调整用户等积分失败" });
                    }

                }
                //减等积分
                else if (acttype == "reduce_dengjifen")
                {
                    B2bcrm_dengjifenlog djflog = new B2bcrm_dengjifenlog
                    {
                        id = 0,
                        crmid = id,
                        dengjifen = 0-decimal.Parse(money),
                        ptype = 2,
                        opertor = user.Id.ToString(),
                        opertime = DateTime.Now,
                        orderid = int.Parse(orderid.ToString()),
                        ordername = ordername,
                        remark = remark
                    };


                    pro = new B2bCrmData().Adjust_dengjifen(djflog, id, comid, 0 - decimal.Parse(money));
                    if (pro == 0)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "调整用户等积分失败" });
                    }

                }
                //充积分
                else  if (acttype == "add_integral")
                {
                    MemberIntegralData intdate = new MemberIntegralData();
                    Member_Integral Intinfo = new Member_Integral()
                            {
                                Id = id,
                                Comid = comid,
                                Acttype = acttype,         //操作类型
                                Money = decimal.Parse(money),     //交易金额
                                Admin = username,
                                Ip = addressIP,
                                Ptype = 1,
                                Oid = 0,
                                Remark = "",
                                OrderId = orderid,
                                OrderName = ordername
                            };
                    pro = intdate.InsertOrUpdate(Intinfo);
                    if (pro != 0)
                    {
                        //积分变动 触发等积分变动
                        B2bcrm_dengjifenlog djflog = new B2bcrm_dengjifenlog
                        {
                            id = 0,
                            crmid = id,
                            dengjifen = decimal.Parse(money),
                            ptype = 1,
                            opertor = user.Id.ToString(),
                            opertime = DateTime.Now,
                            orderid = int.Parse(orderid.ToString()),
                            ordername = ordername,
                            remark ="后台调整积分引起等积分变动:"+remark
                        };
                        new B2bCrmData().Adjust_dengjifen(djflog, id, comid, decimal.Parse(money));





                        B2bCrmData prodata = new B2bCrmData();
                        var list = prodata.Readuser(Intinfo.Id, Intinfo.Comid);

                        //微信消息模板
                        if (list.Weixin != "")
                        {
                            new Weixin_tmplmsgManage().WxTmplMsg_CrmIntegralReward(list.Com_id, list.Weixin, "您好，积分已经打入您的账户", list.Idcard.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "活动赠送", Intinfo.Money.ToString(), list.Integral.ToString(), "如有疑问，请致电客服。");
                        }

                        SendSmsHelper.GetMember_sms(list.Phone, list.Name, list.Idcard.ToString(), list.Password1, Intinfo.Money, "充积分", comid);//发送短信
                    }
                }//减积分
                else if (acttype == "reduce_integral")
                {
                    MemberIntegralData intdate = new MemberIntegralData();
                    Member_Integral Intinfo = new Member_Integral()
                    {
                        Id = id,
                        Comid = comid,
                        Acttype = acttype,           //操作类型
                        Money = 0 - decimal.Parse(money),              //交易金额
                        Admin = username,
                        Ip = addressIP,
                        Ptype = 2,
                        Oid = 0,
                        Remark = "",
                        OrderId = orderid,
                        OrderName = ordername
                    };
                    pro = intdate.InsertOrUpdate(Intinfo);
                    if (pro != 0)
                    {
                        ////积分变动 触发等积分变动
                        //B2bcrm_dengjifenlog djflog = new B2bcrm_dengjifenlog
                        //{
                        //    id = 0,
                        //    crmid = id,
                        //    dengjifen = 0-decimal.Parse(money),
                        //    ptype = 2,
                        //    opertor = user.Id.ToString(),
                        //    opertime = DateTime.Now,
                        //    orderid = int.Parse(orderid.ToString()),
                        //    ordername = ordername,
                        //    remark = "后台调整积分引起等积分变动:" + remark
                        //};
                        //new B2bCrmData().Adjust_dengjifen(djflog, id, comid, 0-decimal.Parse(money));



                        B2bCrmData prodata = new B2bCrmData();
                        var list = prodata.Readuser(Intinfo.Id, Intinfo.Comid);
                        //SendSmsHelper.GetMember_sms(list.Phone, list.Name, list.Idcard.ToString(), list.Password1, Intinfo.Money, "减积分", comid);//发送短信

                        if (list.Weixin != "")
                        {
                            new Weixin_tmplmsgManage().WxTmplMsg_CrmConsume(list.Com_id, list.Weixin, "名称", "使用积分", "会员卡号", list.Idcard.ToString(), DateTime.Now.ToString(), " 使用" + Intinfo.Money + " 积分,如有疑问，请致电客服。");
                        }

                    }
                }//充预付款
                else if (acttype == "add_imprest")
                {
                    MemberImprestData impdate = new MemberImprestData();
                    Member_Imprest Impinfo = new Member_Imprest()
                    {
                        Id = id,
                        Comid = comid,
                        Acttype = acttype,           //操作类型
                        Money = decimal.Parse(money),               //交易金额
                        Admin = username,
                        Ip = addressIP,
                        Ptype = 1,
                        Oid = 0,
                        Remark = "",
                        OrderId = orderid,
                        OrderName = ordername
                    };
                    pro = impdate.InsertOrUpdate(Impinfo);
                    if (pro != 0)
                    {
                        B2bCrmData prodata = new B2bCrmData();
                        var list = prodata.Readuser(Impinfo.Id, Impinfo.Comid);
                        //微信消息模板
                        if (list.Weixin != "")
                        {
                            new Weixin_tmplmsgManage().WxTmplMsg_CrmRecharge(list.Com_id, list.Weixin, "您好，已成功进行会员卡充值", "会员卡号", list.Idcard.ToString(), Impinfo.Money.ToString() + "元", "充值成功", "如有疑问，请致电客服。");
                        }
                        SendSmsHelper.GetMember_sms(list.Phone, list.Name, list.Idcard.ToString(), list.Password1, Impinfo.Money, "充预付款", comid);//发送短信
                    }
                }//减预付款
                else if (acttype == "reduce_imprest")
                {
                    MemberImprestData impdate = new MemberImprestData();
                    Member_Imprest Impinfo = new Member_Imprest()
                    {
                        Id = id,
                        Comid = comid,
                        Acttype = acttype,           //操作类型
                        Money = 0 - decimal.Parse(money),              //交易金额
                        Admin = username,
                        Ip = addressIP,
                        Ptype = 2,
                        Oid = 0,
                        Remark = "",
                        OrderId = orderid,
                        OrderName = ordername

                    };
                    pro = impdate.InsertOrUpdate(Impinfo);
                    if (pro != 0)
                    {
                        B2bCrmData prodata = new B2bCrmData();
                        var list = prodata.Readuser(Impinfo.Id, Impinfo.Comid);
                        //SendSmsHelper.GetMember_sms(list.Phone, list.Name, list.Idcard.ToString(), list.Password1, Impinfo.Money, "减预付款", comid);//发送短信
                        if (list.Weixin != "")
                        {
                            new Weixin_tmplmsgManage().WxTmplMsg_CrmConsume(list.Com_id, list.Weixin, "名称", "使用预付款", "会员卡号", list.Idcard.ToString(), DateTime.Now.ToString(), " 使用" + Impinfo.Money + " 元预付款,如有疑问，请致电客服。");
                        }
                    }
                }
                else
                {
                    pro = 0;//正常操作返回0错误
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        #region 活动加载明细列表
        public static string LoadingList(string comid, int pageindex, int pagesize, int userid)
        {
            var totalcount = 0;
            try
            {

                var list = new List<Member_Activity_Log>();
                B2b_company_manageuser userr = B2bCompanyManagerUserData.GetUser(userid);
                if (userr != null)
                {
                    if (userr.Channelcompanyid == 0)//总公司账户 
                    {
                        list = new B2bCrmData().LoadingList(comid, pageindex, pagesize, out totalcount);
                    }
                    else //总公司下面渠道 
                    {
                        list = new B2bCrmData().LoadingList(comid, pageindex, pagesize, int.Parse(userr.Channelcompanyid.ToString()), out totalcount);
                    }


                    IEnumerable result = "";
                    var memcompany = new MemberChannelcompanyData();
                    if (list != null)
                        result = from pro in list
                                 select new
                                 {
                                     ID = pro.ID,
                                     CardID = MemberCardData.GetCardId(pro.CardID).Cardcode.ToString(),
                                     //CardID =pro.CardID,
                                     ACTID = MemberActivityData.GetActById(pro.ACTID).Title.ToString(),
                                     OrderId = pro.OrderId,
                                     ServerName = pro.ServerName,
                                     Sales_admin = pro.Sales_admin,
                                     Num_people = pro.Num_people,
                                     Usesubdate = pro.Usesubdate,
                                     Per_capita_money = pro.Per_capita_money * pro.Num_people,
                                     Member_return_money = pro.Member_return_money,
                                     username = B2bCrmData.GetCrmCardcode(MemberCardData.GetCardId(pro.CardID).Cardcode) == null ? "--" : B2bCrmData.GetCrmCardcode(MemberCardData.GetCardId(pro.CardID).Cardcode).Name.ToString(),
                                     channel = memcompany.UpCompanyById(MemberCardData.GetCardId(pro.CardID).Cardcode.ToString())
                                 };

                    return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }




            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion




        #region 活动使用日志
        public static string SearchActivityList(string comid, int pageindex, int pagesize, string key, string ServerName, int userid)
        {
            var totalcount = 0;
            bool isNum = Domain_def.RegexValidate("^[0-9]*$", key);

            try
            {
                var list = new List<Member_Activity_Log>();
                B2b_company_manageuser userr = B2bCompanyManagerUserData.GetUser(userid);
                if (userr != null)
                {
                    if (userr.Channelcompanyid == 0)//总公司账户 
                    {
                        list = new B2bCrmData().SearchActivityList(comid, pageindex, pagesize, key, ServerName, isNum, out totalcount);
                    }
                    else //总公司下面渠道 
                    {
                        list = new B2bCrmData().SearchActivityList(comid, pageindex, pagesize, key, ServerName, isNum, int.Parse(userr.Channelcompanyid.ToString()), out totalcount);
                    }
                    IEnumerable result = "";
                    var memcompany = new MemberChannelcompanyData();
                    if (list != null)

                        result = from pro in list
                                 select new
                                 {
                                     ID = pro.ID,
                                     CardID = MemberCardData.GetCardId(pro.CardID).Cardcode,
                                     //CardID =pro.CardID,
                                     ACTID = MemberActivityData.GetActById(pro.ACTID).Title,
                                     OrderId = pro.OrderId,
                                     ServerName = pro.ServerName,
                                     Sales_admin = pro.Sales_admin,
                                     Num_people = pro.Num_people,
                                     Usesubdate = pro.Usesubdate,
                                     Per_capita_money = pro.Per_capita_money * pro.Num_people,
                                     Member_return_money = pro.Member_return_money,
                                     username = B2bCrmData.GetCrmCardcode(MemberCardData.GetCardId(pro.CardID).Cardcode) == null ? "--" : B2bCrmData.GetCrmCardcode(MemberCardData.GetCardId(pro.CardID).Cardcode).Name.ToString(),
                                     channel = MemberChannelData.GetChannelinfo(int.Parse(MemberCardData.GetCardNumber(MemberCardData.GetCardId(pro.CardID).Cardcode).IssueCard.ToString())) == null ? "--" : memcompany.GetCompanyById(MemberChannelData.GetChannelinfo(int.Parse(MemberCardData.GetCardNumber(MemberCardData.GetCardId(pro.CardID).Cardcode).IssueCard.ToString())).Companyid).Companyname.ToString()
                                 };

                    return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }


            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion




        #region 根据用户卡号，及活动号查询活动使用日志
        public static string Logcardact(int actid, int cardid, int comid)
        {
            var totalcount = 0;

            try
            {

                var prodata = new B2bCrmData();
                var list = prodata.Logcardact(actid, cardid, comid, out totalcount);
                IEnumerable result = "";
                var memcompany = new MemberChannelcompanyData();
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 ID = pro.ID,
                                 CardID = MemberCardData.GetCardId(pro.CardID).Cardcode,
                                 ACTID = MemberActivityData.GetActById(pro.ACTID).Title,
                                 //OrderId = pro.OrderId,
                                 ServerName = pro.ServerName,
                                 //Sales_admin = pro.Sales_admin,
                                 //Num_people = pro.Num_people,
                                 Usesubdate = pro.Usesubdate,
                                 Per_capita_money = pro.Per_capita_money * pro.Num_people,
                                 //Member_return_money = pro.Member_return_money,
                             };

                return JsonConvert.SerializeObject(new { type = 100, msg = result, totalcount = totalcount });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region  crm活动日志详细页面
        public static string Logdetails(int id, int comid)
        {
            try
            {
                var prodata = new B2bCrmData();
                var list = prodata.Logdetails(id, comid);

                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 会员统计
        public static string GetCrmStatistics(int comid, int userid)
        {
            try
            {

                B2b_company_manageuser userr = B2bCompanyManagerUserData.GetUser(userid);
                if (userr != null)
                {

                    DataTable dt = new B2bCrmData().GetCrmStatistics(comid, int.Parse(userr.Channelcompanyid.ToString()));
                    return JsonConvert.SerializeObject(new { type = 100, msg = dt });


                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }


            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion
        #region 得到公司附加信息
        public static string GetCompanyInfo(int comid)
        {
            try
            {

                B2b_company_info model = new B2bCompanyInfoData().GetCompanyInfo(comid);
                if (model != null)
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = model });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = model });
                }
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }
        #endregion

        public static string Readdengjifen(int id, int comid)
        {
            var totalcount = 0;
            try
            {

                var list = new B2bcrm_dengjifenlogData().Getdengjifenlist(id, comid, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 id = pro.id,
                                 comid = comid,
                                 admin =GetOpertor(pro.opertor),
                                 money = pro.dengjifen,
                                 subdate = pro.opertime,
                                 orderid = pro.orderid,
                                 ordername = pro.ordername
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        private static string GetOpertor(string userid)
        {
            if (userid.ConvertTo<int>(0) == 0)
            {
                return userid;
            }
            else 
            {
                B2b_company_manageuser u = new B2bCompanyManagerUserData().GetCompanyUser(userid.ConvertTo<int>());
                if (u != null)
                {
                    return u.Employeename;
                }
                else 
                {
                    return userid;
                }
            }
           
        }
    }
}
