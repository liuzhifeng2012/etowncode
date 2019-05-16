using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ETS.Data.SqlHelper;
using System.Collections;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Member.Service.MemberService.Data;
using System.IO;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.VAS.Service.VASService.Data;
using ETS2.VAS.Service.VASService.Modle;
using System.Data.SqlClient;
using ETS2.Member.Service.MemberService.Data.InternalData;
using ETS2.WeiXin.Service.WinXinService.BLL;

namespace ETS.JsonFactory
{
    public class PromotionJsonDate
    {
        #region 根据产品id得到活动
        public static string GetActById(int actid)
        {

            try
            {

                var pro = MemberActivityData.GetActById(actid);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 添加或修改促销活动信息
        public static string EditActInfo(Member_Activity actinfo)
        {

            try
            {

                var pro = MemberActivityData.EditActInfo(actinfo);
                //if (actinfo.Id == 0)
                //{
                //    WxSubscribeSource source = new WxSubscribeSource
                //    {
                //        Id = 0,
                //        Activityid = pro,
                //        Channelcompanyid = 0,
                //        Sourcetype = 1,
                //        Comid = actinfo.Com_id,
                //        Whethercreateqrcode = false
                //    };

                //    int dd = new WxSubscribeSourceData().EditSubscribeSource(source);
                //}

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 促销活动列表
        public static string ActPageList(string comid, int pageindex, int pagesize,int userid=0,string state="0,1")
        {
            //活动过期
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, 0);
            //DateTime dt2 = DateTime.Now.Subtract(ts);
            //string msg= DateTime.Now.ToString() + "-" + ts.Days.ToString() + "天/r/n";
            //msg += dt2.ToString();

            var totalcount = 0;
            try
            {
                var manageuserdata = new B2bCompanyManagerUserData();
                var actdata = new MemberActivityData();
                var list = actdata.ActPageList(comid, pageindex, pagesize, out totalcount,state);
                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Title = pro.Title,
                                 Actstar = pro.Actstar,
                                 Actend = pro.Actend,
                                 Acttype = pro.Acttype,
                                 Cashback = pro.Cashback,
                                 CashFull = pro.CashFull,
                                 Com_id = pro.Com_id,
                                 Discount = pro.Discount,
                                 Money = pro.Money,
                                 RepeatIssue = pro.RepeatIssue,
                                 ReturnAct = pro.ReturnAct,
                                 UseOnceUseOnce = pro.UseOnce,
                                 FaceObjects = pro.FaceObjects,
                                 Runstate = pro.Runstate,
                                 ExpiryDate = DateTime.Now < pro.Actend ? "未过期" : "已过期",
                                 Whetheredit = WhetherEditByIdStr(pro.Id,userid)=="1"?"yes":"no",
                                 CreateChannel = manageuserdata.GetCompanynamebyUserid(pro.CreateUserId)

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


        #region 根据卡号查询所享受的促销活动列表
        public static string AccountActPageList(int accountid, int comid, int channelcompanyid, int pageindex, int pagesize)
        {

            var totalcount = 0;
            try
            {

                var actdata = new MemberActivityData();
                var list = actdata.AccountActPageList(accountid, comid, channelcompanyid, pageindex, pagesize, out totalcount);
                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Title = pro.Title.Length > 10 ? pro.Title.Substring(0, 10) + "." : pro.Title,
                                 Actstar = pro.Actstar,
                                 Actend = pro.Actend,
                                 Acttype = pro.Acttype,
                                 Cashback = pro.Cashback,
                                 CashFull = pro.CashFull,
                                 Com_id = pro.Com_id,
                                 Discount = pro.Discount,
                                 Money = pro.Money,
                                 RepeatIssue = pro.RepeatIssue,
                                 ReturnAct = pro.ReturnAct,
                                 UseOnce = pro.UseOnce,
                                 FaceObjects = pro.FaceObjects,
                                 Runstate = pro.Runstate,
                                 Usestate = pro.Usestate == 1 ? "未使用" : "已使用",
                                 Actnum = pro.Actnum,
                                 Cardid = pro.Cardid,
                                 Aid = pro.Aid,
                                 Remark = pro.Remark,
                                 Usetitle = pro.Usetitle,
                                 Useremark = pro.Useremark


                             };

                if (totalcount == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 100, totalCount = 0, msg = "" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
                }


            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 验证查询时根据卡号查询所享受的促销活动列表
        public static string VerAccountActPageList(int accountid, int comid, int channelcompanyid, int pageindex, int pagesize, int channelcomid)
        {

            var totalcount = 0;
            try
            {

                var actdata = new MemberActivityData();
                var list = actdata.AccountActPageList(accountid, comid, channelcompanyid, pageindex, pagesize, out totalcount, channelcomid);
                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Title = pro.Title.Length > 10 ? pro.Title.Substring(0, 10) + "." : pro.Title,
                                 Actstar = pro.Actstar,
                                 Actend = pro.Actend,
                                 Acttype = pro.Acttype,
                                 Cashback = pro.Cashback,
                                 CashFull = pro.CashFull,
                                 Com_id = pro.Com_id,
                                 Discount = pro.Discount,
                                 Money = pro.Money,
                                 RepeatIssue = pro.RepeatIssue,
                                 ReturnAct = pro.ReturnAct,
                                 UseOnce = pro.UseOnce,
                                 FaceObjects = pro.FaceObjects,
                                 Runstate = pro.Runstate,
                                 Usestate = pro.Usestate == 1 ? "未使用" : "已使用",
                                 Actnum = pro.Actnum,
                                 Cardid = pro.Cardid,
                                 Aid = pro.Aid,
                                 Remark = pro.Remark,
                                 Usetitle = pro.Usetitle,
                                 Useremark = pro.Useremark


                             };

                if (totalcount == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 100, totalCount = 0, msg = "" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
                }


            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 根据卡号查询所享受的促销活动列表
        public static string AccountUnActPageList(int accountid, int comid, int channelcompanyid)
        {

            var totalcount = 0;
            try
            {

                var actdata = new MemberActivityData();
                var list = actdata.AccountUnActPageList(accountid, comid, channelcompanyid, out totalcount);
                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Title = pro.Title.Length > 10 ? pro.Title.Substring(0, 10) + "." : pro.Title,
                                 Actstar = pro.Actstar,
                                 Actend = pro.Actend,
                                 Acttype = pro.Acttype,
                                 Cashback = pro.Cashback,
                                 CashFull = pro.CashFull,
                                 Com_id = pro.Com_id,
                                 Discount = pro.Discount,
                                 Money = pro.Money,
                                 RepeatIssue = pro.RepeatIssue,
                                 ReturnAct = pro.ReturnAct,
                                 UseOnce = pro.UseOnce,
                                 FaceObjects = pro.FaceObjects,
                                 Runstate = pro.Runstate,
                                 Remark = pro.Remark,
                                 Usetitle = pro.Usetitle,
                                 Useremark = pro.Useremark

                                 //Usestate = pro.Usestate == 1 ? "未使用" : "已使用",
                                 //Actnum = pro.Actnum,
                                 //Cardid = pro.Cardid,
                                 //Aid = pro.Aid
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

        #region 领取促销活动列表 aid=活动id aaid=领取后的活动与卡号管理表的id（也就是用户活动ID）
        public static string AccountClaimActPageList(int aid, int cardid, int AccountId, int comid)
        {
            int totalcount = 0;
            var aaid = 0;
            var ret = "";
            try
            {
                if (cardid != 0)
                {
                    //查询优惠券
                    MemberActivityData Activitydata = new MemberActivityData();
                    Member_Activity Activitymodel = Activitydata.GetMemberActivityById(aid);

                    if (Activitymodel != null)
                    {
                        if (Activitymodel.Runstate == false) { 
                            return JsonConvert.SerializeObject(new { type = 1, msg = "活动已结束" });
                        }
                        var today = DateTime.Now;

                        if (Activitymodel.Actstar < today && Activitymodel.Actend.AddDays(1) > today) 
                        { } else
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "活动已结束" });
                        }
                    }


                    //每个活动只能领取一次，防止重复领取，
                    var listact = Activitydata.AccountActInfo(aid, AccountId, comid, out totalcount);
                    if (listact == null)
                    {
                        //领取活动
                        aaid = Activitydata.AccountClaimActPageList(aid, cardid, comid);
                        if (aaid != 0)
                        {
                            if (Activitymodel != null)
                            {
                                if (Activitymodel.Acttype==4)//只有积分优惠券才会领取时充入积分
                                {
                                    MemberIntegralData intdate = new MemberIntegralData();
                                    Member_Integral Intinfo = new Member_Integral()
                                    {
                                        Id = AccountId,
                                        Comid = comid,
                                        Acttype = "add_integral",         //操作类型,增加积分
                                        Money = Activitymodel.Money,     //交易金额
                                        Admin = "领取积分优惠券",
                                        Ip = "",
                                        Ptype = 1,
                                        Oid = 0,
                                        Remark = "",
                                        OrderId = 0,
                                        OrderName = Activitymodel.Atitle
                                    };
                                    var InsertIntegral = intdate.InsertOrUpdate(Intinfo);

                                    //优惠券确认使用
                                    Member_Activity actinfo=null;
                                    string phone="";
                                    string name="";
                                    decimal idcard=0;
                                    decimal aggcardcode=0;

                                    MemberCardData carddata = new MemberCardData();
                                    var confirm = carddata.EconfirmCard(aaid, aid, cardid, comid, out actinfo, out phone, out name, out idcard, out aggcardcode);



                                    B2bCrmData prodata = new B2bCrmData();
                                    var list = prodata.Readuser(AccountId, comid);
                                    //微信消息模板
                                    if (list.Weixin != "")
                                    {
                                        new Weixin_tmplmsgManage().WxTmplMsg_CrmIntegralReward(list.Com_id, list.Weixin, "您好，" + AccountId + " 积分已经打入您的账户", list.Idcard.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "活动赠送", Intinfo.Money.ToString(), list.Integral.ToString(), "如有疑问，请致电客服。");
                                    }


                                 }
                            }
                        
                        }
                        ret = "OK";
                    }
                    else
                    {
                        ret = "已领取活动";
                    }

                    return JsonConvert.SerializeObject(new { type = 100, msg = ret });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "Unlogin" });
                }


            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 添加或修改摇奖活动信息
        public static string ERNIEEditActInfo(Member_ERNIE ERNIEinfo, List<Member_ERNIE_Award> Awardinfo)
        {

            try
            {
                //修改活动信息
                var pro = MemberERNIEData.ERNIEEditActInfo(ERNIEinfo);

                if (pro != 0)
                {

                    //获得活动信息
                    var erniedata = MemberERNIEData.ERNIEGetActById(pro);
                    if (erniedata != null)
                    {
                        //对修改加已限制，如果已完成上线，不能修改奖项，只能修改文字
                        if (erniedata.Online == 0)//未上线
                        {
                            if (ERNIEinfo.Id != 0)
                            {//删除所有奖项
                                MemberERNIEData.ERNIEDelAwardInfo(ERNIEinfo.Id);
                            }

                            int j = 1;
                            for (int i = 0; i < Awardinfo.Count; i++)
                            {
                                if (Awardinfo[i].Award_title != "")
                                {
                                    Awardinfo[i].ERNIE_id = pro;
                                    Awardinfo[i].Award_class = j;
                                    MemberERNIEData.ERNIEEditAwardInfo(Awardinfo[i]);
                                    j = j + 1;
                                }
                            }
                        }
                    }
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


        #region 促销抽奖活动列表
        public static string ERNIEActPageList(string comid, int pageindex, int pagesize,string runstate="0,1")
        {
            //活动过期
            //TimeSpan ts = new TimeSpan(0, 0, 0, 0, 0);
            //DateTime dt2 = DateTime.Now.Subtract(ts);
            //string msg= DateTime.Now.ToString() + "-" + ts.Days.ToString() + "天/r/n";
            //msg += dt2.ToString();

            var totalcount = 0;
            try
            {

                var actdata = new MemberERNIEData();
                var list = actdata.ERNIEActPageList(Int32.Parse(comid), pageindex, pagesize, out totalcount,runstate);
                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Title = pro.Title,
                                 ERNIE_type = pro.ERNIE_type == 1 ? "大转盘摇奖" : "其他方式",
                                 ERNIE_RateNum = pro.ERNIE_RateNum,
                                 ERNIE_Limit = pro.ERNIE_Limit == 0 ? "一次性抽奖活动" : "每天抽奖活动",
                                 Limit_Num = pro.Limit_Num,
                                 Runstate = pro.Runstate == 0 ? "停止" : "运行中",
                                 Com_id = pro.Com_id,
                                 ERNIE_star = pro.ERNIE_star,
                                 ERNIE_end = pro.ERNIE_end,
                                 Remark = pro.Remark,
                                 Online = pro.Online,
                                 Online_str = pro.Online == 1 ? "已完成" : ""

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

        #region 中奖列表
        public static string ERNIERecordpagelist(string comid, int pageindex, int pagesize, int actid, int ERNIE_type, string key)
        {

            var totalcount = 0;
            try
            {

                var actdata = new MemberERNIEData();
                var list = actdata.ERNIERecordpagelist(comid, pageindex, pagesize, actid, ERNIE_type, key, out totalcount);
                IEnumerable result = "";
                if (list != null)
                    result = from pro in list

                             select new
                             {
                                 Id = pro.Id,
                                 Title = MemberERNIEData.ERNIEGetActById(pro.ERNIE_id) != null ? MemberERNIEData.ERNIEGetActById(pro.ERNIE_id).Title : "",
                                 Award = MemberERNIEData.ERNIEGetAwardById(pro.Awardid) != null ? MemberERNIEData.ERNIEGetAwardById(pro.Awardid).Award_title : "",
                                 Name = pro.Name,
                                 ERNIE_id = pro.ERNIE_id,
                                 Phone = pro.Phone,
                                 ERNIE_code = pro.ERNIE_code,
                                 ERNIE_openid = pro.ERNIE_openid,
                                 ERNIE_uid = pro.ERNIE_uid,
                                 Address = pro.Address,
                                 Winning_state = pro.Winning_state,
                                 Ip = pro.Ip,
                                 ERNIE_time = pro.ERNIE_time,
                                 Process_state = pro.Process_state,
                                 Awardid = pro.Awardid

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



        #region 根据产品id得到活动
        public static string ERNIEGetActById(int actid)
        {

            try
            {

                var pro = MemberERNIEData.ERNIEGetActById(actid);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion





        #region 添加或修改摇奖活动奖品信息
        public static string ERNIEEditAwardInfo(Member_ERNIE_Award ERNIEinfo)
        {

            try
            {

                var pro = MemberERNIEData.ERNIEEditAwardInfo(ERNIEinfo);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 促销抽奖活动奖品列表
        public static string ERNIEAwardPageList(int actid, int pageindex, int pagesize)
        {

            var totalcount = 0;
            try
            {

                var actdata = new MemberERNIEData();
                var list = actdata.ERNIEAwardPageList(actid, pageindex, pagesize, out totalcount);
                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Award_title = pro.Award_title,
                                 ERNIE_id = pro.ERNIE_id,
                                 Award_class = pro.Award_class,
                                 Award_num = pro.Award_num,
                                 Award_type = pro.Award_type,
                                 Award_Get_Num = pro.Award_Get_Num

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


        #region 根据产品id得到摇奖活动奖励
        public static string ERNIEGetAwardById(int actid)
        {

            try
            {

                var pro = MemberERNIEData.ERNIEGetAwardById(actid);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 根据产品id得到摇奖活动奖励
        public static string ERNIEAwardget(int actid, int topclass)
        {

            try
            {

                var pro = MemberERNIEData.ERNIEAwardget(actid, topclass);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion
        #region 抽奖摇奖
        public static string ERNIEChoujiang(ERNIE_Record recordinfo)
        {
            //先产生个随机码超出范围的随机号，后面根据活动设定重新获得随机号，防止没有查询到活动时无返回随机码
            Random ra = new Random();
            var code = ra.Next(98888888, 99888888);

            try
            {
                //先判断活动
                var erniedate = MemberERNIEData.ERNIEGetActById(recordinfo.ERNIE_id);
                if (erniedate != null)
                {
                    if (erniedate.Runstate == 1 && erniedate.Online == 1 && erniedate.ERNIE_star < DateTime.Now)
                    {//运行中,在起始时间内

                        if (erniedate.ERNIE_end.AddDays(1) < DateTime.Now)//超出日期则显示活动已经结束，时间按结束日期的23:59:59秒
                        {
                            return JsonConvert.SerializeObject(new { error = "此次活动已经结束", sn = code, success = false });
                        }



                        code = ra.Next((98888888 - erniedate.ERNIE_RateNum), 98888888);
                        recordinfo.ERNIE_code = code;//赋值随机码
                        recordinfo.Ip = CommonFunc.GetRealIP();//记录IP
                        recordinfo.ERNIE_time = DateTime.Now;//记录时间

                        //读取用户信息
                        B2bCrmData crmmodel = new B2bCrmData();
                        B2b_crm memberinfo = crmmodel.b2b_crmH5(recordinfo.ERNIE_openid, erniedate.Com_id);
                        if (memberinfo != null)
                        {
                            recordinfo.ERNIE_uid = memberinfo.Id;//读取用户ID,必须是已关注用户才能抽奖

                            var ERNIE_Limit = erniedate.ERNIE_Limit;//抽奖频率
                            var Limit_Num = erniedate.Limit_Num;//抽奖次数

                            var searchdate = MemberERNIEData.SearchChoujiang(recordinfo, ERNIE_Limit);//查询是否抽过奖
                            if (searchdate < Limit_Num)
                            {
                                var insertdate = MemberERNIEData.InsertChoujiang(recordinfo);//插入抽奖

                                //判断是否中奖。可以重复，按指定
                                var panduanchoujiang = MemberERNIEData.ChoujiangSearchAwardcode(insertdate, recordinfo.ERNIE_id);
                                if (panduanchoujiang > 0)
                                { //如果中奖，
                                    //处理中奖，返回中奖级别
                                    var zhongjiang = MemberERNIEData.ZhongjiangAwardcode(insertdate, panduanchoujiang, memberinfo.Id);
                                    if (zhongjiang > 0)
                                    {
                                        return JsonConvert.SerializeObject(new { error = "", insert_id = insertdate, success = true, prizetype = zhongjiang, sn = code });
                                    }
                                }

                            }
                            else
                            {
                                return JsonConvert.SerializeObject(new { error = "您已经参加过抽奖活动", sn = code, success = false });
                            }
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new { error = "只有关注微信用户才能参与抽奖", sn = code, success = false });
                        }
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { error = "活动尚未开始，请稍后再关注", sn = code, success = false });

                    }
                }

                return JsonConvert.SerializeObject(new { error = "", sn = code, success = false });


            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { error = "", sn = code, success = false });
                throw;
            }
        }
        #endregion

        #region 抽奖活动确认上线,并生成奖项的电子码
        public static string ERNIEeditActOnline(int actid)
        {
            MemberERNIEData erniedate = new MemberERNIEData();
            int code = 0;//随机码
            int ERNIE_RateNum = 0; //摇奖基数

            try
            {
                var searchpro = MemberERNIEData.ERNIEGetActById(actid);
                if (searchpro == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "失败，没有查询到此抽奖活动" });
                }

                if (searchpro.Online == 1)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "失败，此活动已经上线" });
                }
                searchpro.Runstate = 1;
                MemberERNIEData.ERNIEEditActInfo(searchpro);//把活动设定为运行中



                //基数也就是范围 基数的数量值，摇奖就在此范围内，最小比例 9千万分之1
                ERNIE_RateNum = searchpro.ERNIE_RateNum;

                //查询此活动的奖项信息
                int totalcount = 0;
                var searchAward = erniedate.ERNIEAwardPageList(actid, 1, 100, out totalcount);
                if (searchAward == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "失败，没有查询到此抽奖活动的奖项信息" });
                }

                //生成实体类
                ERNIE_Awardcode awardcode = new ERNIE_Awardcode();
                Random ra = new Random();
                for (int i = 0; i < totalcount; i++)
                {//循环奖品信息
                    for (int j = 0; j < searchAward[i].Award_num; j++)//循环奖品数量，插入
                    {
                        code = ra.Next((98888888 - ERNIE_RateNum), 98888888);
                        awardcode.ERNIE_id = searchAward[i].ERNIE_id;
                        awardcode.Award_id = searchAward[i].Id;
                        awardcode.Award_code = code;
                        var insertAward = MemberERNIEData.InsertAward(awardcode);
                    }

                }

                var pro = MemberERNIEData.ERNIEeditActOnline(actid);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion



        #region 抽奖活动确认上线,并生成奖项的电子码
        public static string ERNIERecordedit(int actid)
        {
            try
            {
                var pro = MemberERNIEData.ERNIERecordedit(actid);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 提交中奖信息
        public static string ERNIEZhongjiang(ERNIE_Record Recordinfo)
        {
            MemberERNIEData ernieddate = new MemberERNIEData();
            string rtstr = "";//返回文字
            try
            {

                var pror = ernieddate.ERNIERecordInfo(Recordinfo.Id);//查询中奖纪录
                if (pror != null)
                {
                    if (pror.Process_state == 0)
                    {
                        var pro = MemberERNIEData.ERNIEZhongjiang(Recordinfo);//中奖提交

                        if (pro > 0)
                        {
                            var proernie = MemberERNIEData.ERNIEGetActById(Recordinfo.ERNIE_id);//查询活动
                            if (proernie != null)
                            {

                                var prow = MemberERNIEData.ERNIEGetAwardById(pror.Awardid);//查询奖品纪录，成功状态修改为中奖奖品级别

                                if (prow != null)
                                {
                                    if (prow.Award_type == 2)//必须是赠送积分的才会自动赠送
                                    {
                                        //中奖状态修改，为已处理
                                        var prochuli = MemberERNIEData.ERNIEZhongjiangChuli(Recordinfo.Id);
                                        if (prochuli > 0)
                                        {
                                            //插入积分
                                            MemberIntegralData intdate = new MemberIntegralData();
                                            Member_Integral Intinfo = new Member_Integral()
                                            {
                                                Id = pror.ERNIE_uid,
                                                Comid = proernie.Com_id,
                                                Acttype = "add_integral",         //操作类型
                                                Money = prow.Award_Get_Num,       //交易金额
                                                Admin = proernie.Title,
                                                Ip = CommonFunc.GetRealIP(),
                                                Ptype = 1,
                                                Oid = 0,
                                                Remark = "",
                                                OrderId = 0,
                                                OrderName = ""
                                            };
                                            pro = intdate.InsertOrUpdate(Intinfo);
                                            //抽奖赠送等积分
                                            B2bcrm_dengjifenlog djflog = new B2bcrm_dengjifenlog
                                            {
                                                id = 0,
                                                crmid = pror.ERNIE_uid,
                                                dengjifen = prow.Award_Get_Num,
                                                ptype = 1,
                                                opertor = "抽奖赠送等积分",
                                                opertime = DateTime.Now,
                                                orderid = 0,
                                                ordername = "抽奖赠送等积分",
                                                remark = "抽奖赠送等积分"
                                            };
                                            new B2bCrmData().Adjust_dengjifen(djflog, pror.ERNIE_uid, proernie.Com_id, prow.Award_Get_Num);

                                            if (pro > 0)
                                            {
                                                rtstr = "您中奖的" + prow.Award_Get_Num + "元，已经打入您的积分中，请查收！";
                                            }
                                            else
                                            {
                                                rtstr = "中奖信息已经提交成功！";
                                            }
                                        }
                                        else
                                        {

                                            rtstr = "中奖信息已经提交成功！";
                                        }
                                    }
                                    else
                                    {

                                        rtstr = "中奖信息已经提交成功！";
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { rt = -1, msg = "中奖信息提交失败" });
                    }
                    return JsonConvert.SerializeObject(new { rt = 1, msg = rtstr });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { rt = -1, msg = "中奖信息提交失败！" });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { rt = -1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 获奖名单
        public static string Huojiangmingdan(int actid, string openid)
        {

            var totalcount = 0;
            try
            {

                var actdata = new MemberERNIEData();
                var list = actdata.ERNIEHuojiangmingdan(actid);
                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 ERNIE_id = pro.ERNIE_id,
                                 phone = pro.Phone == "" ? "****" : pro.Phone.Substring(0, 3) + "****" + pro.Phone.Substring(7),
                                 Name = pro.Name == "" ? "**" : pro.Name.Substring(0, 1) + "**",
                                 Award_class = MemberERNIEData.ERNIEAwardgetID(pro.Awardid) + "等奖"
                             };

                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        public static string WhetherEditByIdJson(int actid, int operuserid)
        {


            Member_Activity model = new MemberActivityData().GetMemberActivityById(actid);

            if (model == null)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = "获取优惠活动失败", data = "0" });
            }
            else
            {
                //获得操作人信息
                B2b_company_manageuser user = B2bCompanyManagerUserData.GetUser(operuserid);
                if (user != null)
                {
                    if (model.Com_id == user.Com_id)
                    {
                        if (model.CreateUserId == 0)//由于以前活动表中没有createuserid,所以createuserid=0认定为公司创建
                        {
                            if (user.Channelcompanyid == 0)//操作人属于公司,可以编辑活动
                            {
                                return JsonConvert.SerializeObject(new { type = 100, msg = "", data = "1" });
                            }
                            else
                            {
                                return JsonConvert.SerializeObject(new { type = 100, msg = "", data = "0" });
                            }
                        }
                        else
                        {
                            //判断创建人和现在的操作人是否属于同一单位(同一门市或者同一公司下)
                            bool whethersameunit = new MemberActivityData().WhetherSameunit(model.CreateUserId, user.Channelcompanyid);
                            if (whethersameunit)
                            {
                                return JsonConvert.SerializeObject(new { type = 100, msg = "", data = "1" });
                            }
                            else
                            {
                                return JsonConvert.SerializeObject(new { type = 100, msg = "", data = "0" });
                            }
                        }
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = "活动非本公司活动", data = "0" });
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = "获取操作人信息失败", data = "0" });
                }

            }
        }
        public static string WhetherEditByIdStr(int actid, int operuserid)
        {


            Member_Activity model = new MemberActivityData().GetMemberActivityById(actid);

            if (model == null)
            {
                return "0";
            }
            else
            {
                //获得操作人信息
                B2b_company_manageuser user = B2bCompanyManagerUserData.GetUser(operuserid);
                if (user != null)
                {
                    if (model.Com_id == user.Com_id)
                    {
                        if (model.CreateUserId == 0)//由于以前活动表中没有createuserid,所以createuserid=0认定为公司创建
                        {
                            if (user.Channelcompanyid == 0)//操作人属于公司,可以编辑活动
                            {
                                return "1";
                            }
                            else
                            {
                                return "0";
                            }
                        }
                        else
                        {
                            //判断创建人和现在的操作人是否属于同一单位(同一门市或者同一公司下)
                            bool whethersameunit = new MemberActivityData().WhetherSameunit(model.CreateUserId,user.Channelcompanyid);
                            if (whethersameunit)
                            {
                                return "1";
                            }
                            else 
                            {
                                return "0";
                            }
                        }
                    }
                    else
                    {
                        return "0";
                    }
                }
                else
                {
                    return "0";
                }

            }
        }
    }
}
