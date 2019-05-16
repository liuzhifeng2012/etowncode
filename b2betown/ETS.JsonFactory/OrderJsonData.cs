using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using Newtonsoft.Json;
using ETS2.PM.Service.PMService.Data;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using System.Collections;
using ETS.Framework;
using ETS2.PM.Service.PMService.Modle.Enum;
using System.Data;
using ETS2.VAS.Service.VASService.Data;
using ETS2.VAS.Service.VASService.Modle;
using FileUpload.FileUpload.Data;
using ETS2.Common.Business;
using FileUpload.FileUpload.Entities;
using ETS2.Member.Service.MemberService.Data;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.VAS.Service.VASService.Data.Common;
using System.Xml;
using ETS2.Member.Service.MemberService.Model;
using System.Web.Script.Serialization;
using ETS2.WeiXin.Service.WinXinService.BLL;
using ETS2.PM.Service.Taobao_Ms.Data;
using Com.Tenpay.WxpayApi.sysprogram.data;
using Com.Alipiay.app_code2.SysProgram.data;
using ETS2.Permision.Service.PermisionService.Model;
using ETS2.Permision.Service.PermisionService.Data;
using WxPayAPI;
using Com.Tenpay.WxpayApi.sysprogram.model;
using ETS2.VAS.Service.VASService.Modle.Enum;
using ETS2.CRM.Service.CRMService.Modle.Enum;
using ETS2.PM.Service.WL.Data;
using ETS2.PM.Service.WL.Model;



namespace ETS.JsonFactory
{
    public class OrderJsonData
    {
        private static object lockobj = new object();
        /// <summary>
        /// 编辑订单信息
        /// </summary>
        /// <param name="order"></param>
        /// <param name="bindingorderid">返回订单号</param>
        /// <param name="morenfasong">默认发送，当为0则不发送</param>
        /// <returns></returns>
        public static string InsertOrUpdate(B2b_order order, out int bindingorderid, int morenfasong = 1, int isInterfaceSub = 0)
        {


            //using (var helper = new SqlHelper())
            //{
            bindingorderid = 0;//先默认返回订单号为0,回传至
            int bindingorderid_huoqu = 0;//分销订单返回获取值
            try
            {
                //helper.BeginTrancation();
                B2bComProData prodata = new B2bComProData();
                B2bCrmData crmdata = new B2bCrmData();
                B2bOrderData orderdate = new B2bOrderData();
                //读取COMID
                var comid = prodata.GetComidByProid(order.Pro_id);
                B2b_company commanage = B2bCompanyData.GetAllComMsg(comid);

                //读取产品
                B2b_com_pro pro_t = new B2bComProData().GetProById(order.Pro_id.ToString(), order.Speciid, order.channelcoachid);


                //对手机号去除空格,总有部分人 填写空格引起一些列问题，把中间空格删除
                order.U_phone = order.U_phone.Replace(" ", "");


                if (order.Pro_id != 0)
                {
                    if (pro_t != null)
                    {
                        //如果来源为“从外部倒码”则判断库存产品是否充足
                        //库存票检查库存数量，不足则不让提交订单

                        if (pro_t.Source_type == 2)
                        {
                            int kucunpiaoshuliang = new B2bComProData().ProSEPageCount_UNUse(pro_t.Id);
                            if (kucunpiaoshuliang < order.U_num)
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "票已买光，请电话订购或联系商家" });
                            }
                        }
                        if (order.U_num <= 0)
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "提单数量需要大于0" });
                        }
                        //检查 规格产品库存
                        if (order.Speciid != 0)
                        {
                            B2b_com_pro prospeciid = new B2bComProData().GetProspeciidById(order.Pro_id.ToString(), order.Speciid);
                            if (prospeciid != null)
                            {

                                if (pro_t.Source_type != 4)//非导入产品计算规格库存
                                {
                                    if (prospeciid.Limitbuytotalnum < order.U_num)
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = "此规格票已买光，请电话订购或联系商家" });
                                    }
                                }
                            }
                        }



                        //检查订单是否超过产品的每单限购数量
                        if (pro_t.Pro_number > 0)
                        {
                            if (order.U_num > pro_t.Pro_number)
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "当前产品每单限购数量为" + pro_t.Pro_number });
                            }
                        }

                        //检查是否有权购买，针对直销订单
                        if (order.Agentid == 0)
                        {
                            if (pro_t.Viewmethod != 7){
                                //对直销订单判断有权购买,2,6只针对分销购买
                                if (pro_t.Viewmethod == 2 || pro_t.Viewmethod == 6)
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "此产品未授权" });
                                }
                            }

                        }
                        else
                        {
                            //新增对分销商产品判断产品是否授权购买、
                            var ProAgentWarrant = ProAgentWarrantstate(order.Agentid, order.Pro_id, comid);
                            if (ProAgentWarrant == 0)
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "此产品分销商未授权" });
                            }
                        }

                        //如果是 多规格产品则 订购必须有规格值
                        if (pro_t.Manyspeci == 1)
                        {
                            if (order.Speciid == 0)
                            {

                                return JsonConvert.SerializeObject(new { type = 1, msg = "请选择 具体规格" });
                            }

                        }
                        //是否需要身份证提交
                        if (pro_t.issetidcard == 1)
                        {
                            if (order.U_idcard == "")
                            {

                                return JsonConvert.SerializeObject(new { type = 1, msg = "请填写身份证号" });
                            }

                        }
                        //是否需要提交日期
                        if (pro_t.isSetVisitDate == 1)
                        {
                            if (order.U_traveldate.ToString() =="")
                            {

                                return JsonConvert.SerializeObject(new { type = 1, msg = "请选择 使用日期" });
                            }
                        }


                        if (pro_t.Server_type == 10)
                        {
                            //不忽略旅游大巴下单限制
                            if (order.ignoredabatime == 0)
                            {
                                if (pro_t.firststationtime != "")
                                {
                                    //旅游大巴订单 发车前12h 不可以退
                                    DateTime daba_startTime = DateTime.Now;
                                    DateTime daba_endTime = Convert.ToDateTime(order.U_traveldate.ToString("yyyy-MM-dd ") + pro_t.firststationtime);
                                    if ((daba_endTime - daba_startTime).TotalMinutes < 60)
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = " 本产品提前60分钟不可预约" });
                                    }
                                }
                            }



                            //新增 2019.1.31号 对手机号进行判断 如果有手机号错误的直接返回创建订单错误
                            #region  乘车人信息手机号是否正确判断

                            int shoujihaoture = 1;//手机号正确默认为1
                            try
                            {
                                if (order.travelnames != "")
                                {
                                    for (int i = 1; i <= order.U_num; i++)
                                    {
                                        string travelname = order.travelnames.Split(',')[i - 1];
                                        string travelphone = order.travelphones.Split(',')[i - 1]; //主要针对手机号进行判断

                                        //手机号不定于0，如果有一个手机号出错了就直接都是错误
                                        if (shoujihaoture != 0 && new SendEticketData().IsMobile(travelphone) == false)
                                        {
                                            shoujihaoture = 0;
                                        }
                                    }

                                    if (shoujihaoture == 0)
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = " 请正确填写出行人的手机号码，我们会发送乘坐班车的相关信息。" });
                                    }

                                }
                            }
                            catch
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = " 乘客信息填写出错，请刷新重新填写。" });
                            }
                            #endregion


                        }

                        //检查 如果有绑定服务，并且选择了服务，必须选择 已绑定 必须购买的服务(未做，因为涉及到补购买服务)




                        //对教练产品 时间判定,只针对 黑名单里的时间做出 判定
                        if (pro_t.Server_type == 13)
                        {

                            //对教练时间进行控制
                            var channeldata = new MemberChannelData();
                            var workmodel = new B2b_company_manageuser_useworktime();
                            var ManagerUserData = new B2bCompanyManagerUserData();

                            int MasterId_temp = channeldata.GetanageuseridbymChannelid(order.channelcoachid, order.Comid);
                            var Hournum = int.Parse(order.U_traveldate.ToString("HH"));
                            int outnum = 0;
                            var yuyueshijian = SendEticketData.JiaolianTqBaoxinmsg(order.Speciid);//获取 预约 几小时
                            string yuyueshijian_liststr = "";
                            if (yuyueshijian > 0)
                            {

                                B2b_company_manageuser manageruser = B2bCompanyManagerUserData.GetUser(MasterId_temp);

                                if (manageruser != null)
                                {
                                    if (manageruser.Workdays != "")
                                    {//上班日期

                                        var dateweek = 0;
                                        var weekend = 0;//是否为周末
                                        var dt = order.U_traveldate.DayOfWeek.ToString();
                                        switch (dt)
                                        {
                                            case "Monday":
                                                dateweek = 2;
                                                break;
                                            case "Tuesday":
                                                dateweek = 3;
                                                break;
                                            case "Wednesday":
                                                dateweek = 4;
                                                break;
                                            case "Thursday":
                                                dateweek = 5;
                                                break;
                                            case "Friday":
                                                dateweek = 6;
                                                break;
                                            case "Saturday":
                                                dateweek = 7;
                                                weekend = 1;
                                                break;
                                            case "Sunday":
                                                dateweek = 1;
                                                weekend = 1;
                                                break;
                                        }

                                        if (manageruser.Workdays.Contains(dateweek.ToString()))//有当前日期
                                        {

                                        }
                                        else
                                        {
                                            return JsonConvert.SerializeObject(new { type = 1, msg = "你选择的日期教练休息，请选择其他教练" });
                                        }

                                        if (weekend == 1)
                                        {
                                            //获取到多个时间是，每个小时 没个小时一条记录
                                            for (int i = Hournum; i < Hournum + yuyueshijian; i++)
                                            {
                                                if (i > manageruser.workendtimeend)
                                                {
                                                    return JsonConvert.SerializeObject(new { type = 1, msg = "您选择的时间超出了教练的工作时间，请选择其他时间段!" });
                                                }

                                            }
                                        }
                                        else
                                        {
                                            //获取到多个时间是，每个小时 没个小时一条记录
                                            for (int i = Hournum; i < Hournum + yuyueshijian; i++)
                                            {
                                                if (i > manageruser.worktimeend)
                                                {
                                                    return JsonConvert.SerializeObject(new { type = 1, msg = "您选择的时间超出了教练的工作时间，请选择其他时间段!" });
                                                }

                                            }

                                        }


                                    }
                                }


                                //获取到多个时间是，每个小时 没个小时一条记录
                                for (int i = Hournum; i < Hournum + yuyueshijian; i++)
                                {
                                    yuyueshijian_liststr = i + ",";

                                }
                                yuyueshijian_liststr = yuyueshijian_liststr.Substring(0, yuyueshijian_liststr.Length - 1);//去掉最后一个,


                                var Worklist = ManagerUserData.Worktimepagelist(order.Comid, MasterId_temp, DateTime.Parse(order.U_traveldate.ToString("yyyy-MM-dd")), yuyueshijian_liststr, out outnum);
                                if (outnum > 0)
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "您选择的时间，" + Worklist[0].Hournum + "点 已经被预约，请选择其他连续时间，或选择其他教练！" });
                                }

                            }





                        }



                        //系统导入产品，对原分销订单订单号进行绑定 ，默认为0
                        order.Bindingagentorderid = bindingorderid_huoqu;

                    }



                    //计算快递费用
                    string errmsg = "";
                    int deliverytype = order.deliverytype;

                    if (deliverytype == 1 || deliverytype == 2 || deliverytype == 3)
                    {
                        if (order.Shopcartid == 0)
                        {//快递费赋值,如果非购物车产品需要计算快递费，如果是购物车产品之前已经计算并赋值快递费
                            var fee = new B2b_delivery_costData().Getdeliverycost(order.Pro_id, order.City, order.U_num, out errmsg, deliverytype);
                            order.Express = fee;//快递费赋值
                        }
                    }
                    else
                    {
                        order.Express = 0;//快递费赋值
                    }



                }

                var dikou = "";
                int orderid = 0;
                decimal usercard = 0;
                #region 用户订单
                if (order.Agentid == 0)//用户订单
                {

                    #region 读取用户信息
                    if (order.Openid == "")//判断是否有微信号，没有微信号检查手机
                    {//查看手机是否有账户
                        B2b_crm crmm = crmdata.GetSjKeHu(order.U_phone, comid);
                        if (crmm != null)
                        {
                            //通取用户ID
                            order.U_id = crmm.Id;
                            usercard = crmm.Idcard;
                        }
                        else
                        {//没有的话创建新账户
                            B2b_crm crm = new B2b_crm()
                            {
                                Id = 0,
                                Com_id = comid,
                                Name = order.U_name,
                                Sex = "0",
                                Phone = order.U_phone,
                                Email = "",
                                Weixin = "",
                                Laiyuan = "",
                                Regidate = DateTime.Now,
                                Age = 0
                            };

                            string cardcode = MemberCardData.CreateECard(1, comid);//创建卡号并插入活动,1:网站；2:微信

                            crm.Idcard = cardcode.ConvertTo<decimal>(0);
                            int u_id = crmdata.InsertOrUpdate(crm);
                            usercard = cardcode.ConvertTo<decimal>(0);
                            order.U_id = u_id;
                        }
                    }
                    else
                    {
                        //通过微信获取用户ID
                        B2b_crm weixinuserinfo = null;
                        var weixincrm = crmdata.GetB2bCrm(order.Openid, order.Comid, out weixinuserinfo);
                        if (weixincrm == "OK")
                        {
                            if (weixinuserinfo != null)
                            {
                                order.U_id = weixinuserinfo.Id;
                                usercard = weixinuserinfo.Idcard;
                            }
                        }
                        else
                        {
                            //如果微信号查询不到账户的异常，按手机读取账户或创建账户
                            B2b_crm crmm = crmdata.GetSjKeHu(order.U_phone, comid);
                            if (crmm != null)
                            {
                                //通取用户ID
                                order.U_id = crmm.Id;

                            }
                            else
                            {
                                B2b_crm crm = new B2b_crm()
                                {
                                    Id = 0,
                                    Com_id = comid,
                                    Name = order.U_name,
                                    Sex = "0",
                                    Phone = order.U_phone,
                                    Email = "",
                                    Weixin = "",
                                    Laiyuan = "",
                                    Regidate = DateTime.Now,
                                    Age = 0
                                };

                                string cardcode = MemberCardData.CreateECard(1, comid);//创建卡号并插入活动,1:网站；2:微信

                                crm.Idcard = cardcode.ConvertTo<decimal>(0);
                                int u_id = crmdata.InsertOrUpdate(crm);
                                usercard = crm.Idcard;
                                order.U_id = u_id;
                            }
                        }

                    }
                    #endregion

                    order.Backtickettime = DateTime.Now;


                    #region 如果服务类型是 旅游大巴  需要判断产品数量 和 产品订购时间
                    if (pro_t.Server_type == 10)
                    {
                        //控位数量
                        var proid_temp = order.Pro_id;
                        if (pro_t.Source_type == 4)
                        {//如果是绑定的产品查询绑定产品団期
                            proid_temp = pro_t.Bindingid;
                        }

                        int emptynum = new B2b_com_LineGroupDateData().GetEmptyNum(proid_temp, order.U_traveldate);
                        if (emptynum < order.U_num)
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "提交失败，当日已售罄，请选择其他日期" });
                        }

                        //判断订购时间
                        var today = DateTime.Today;
                        TimeSpan riqicha = order.U_traveldate - today;//提单必须提交明天以后的,如果明天的需要再次判断时间是否超出
                        int sub = riqicha.Days;     //sub就是两天相差的天数
                        if (sub < 0)
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "提交失败，此产品需提前预订，请选择其他日期" });
                        }

                        //把产品价格按日期价格赋值
                        string daba_price_temp = new B2b_com_LineGroupDateData().GetNowdayPrice(proid_temp, order.U_traveldate.ToString("yyyy-MM-dd"));
                        if (decimal.Parse(daba_price_temp) > 0)
                        {
                            order.Pay_price = decimal.Parse(daba_price_temp);
                        }


                        ////经和经理确认,截团之前都可以收人提单
                        ////提单最后时间，默认设置为 前一天 16点。
                        //var endtime = DateTime.Parse(DateTime.Today.ToString("yyyy.MM.dd") + " 16:00:00");
                        //if (sub == 1)
                        //{
                        //    //提交时间是和默认结束时间比较
                        //    if (DateTime.Now > endtime)
                        //    {
                        //        return JsonConvert.SerializeObject(new { type = 1, msg = "提交失败，您提交日期已经排车，请选择其他日期的班车！" });
                        //    }
                        //}
                    }
                    #endregion
                    #region 其他服务类型，如果产品参加抢购/限购,判断产品数量是否符合条件
                    else
                    {
                        //根据访问获得公司信息
                        WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(comid);


                        if (pro_t.Ispanicbuy == 1 || pro_t.Ispanicbuy == 2)
                        {
                            if (pro_t.Ispanicbuy == 1)
                            {
                                //提单时间是否在抢购时间之内
                                if (DateTime.Now < pro_t.Panic_begintime || DateTime.Now > pro_t.Panicbuy_endtime)
                                {
                                    if (DateTime.Now < pro_t.Panic_begintime)
                                    {


                                        //短信内容发送微信客服通道
                                        var data = CustomerMsg_Send.SendWxMsg(comid, order.Openid, 1, "", "本次赠送产品 “" + pro_t.Pro_name + "”活动还未开始，请在微信中关注具体活动时间。谢谢！", "", basicc.Weixinno);
                                        return JsonConvert.SerializeObject(new { type = 1, msg = "抢购活动还未开始" });
                                    }
                                    else
                                    {

                                        //短信内容发送微信客服通道
                                        var data = CustomerMsg_Send.SendWxMsg(comid, order.Openid, 1, "", "本次赠送产品 “" + pro_t.Pro_name + "”活动已结束，请在微信中关注具体活动时间。谢谢！", "", basicc.Weixinno);

                                        return JsonConvert.SerializeObject(new { type = 1, msg = "抢购活动已结束" });
                                    }
                                }
                            }
                            //服务类型是跟团游、当地游、旅游大巴，需要判断空位数量
                            if (pro_t.Server_type == 2 || pro_t.Server_type == 8)
                            {
                                int emptynum = new B2b_com_LineGroupDateData().GetEmptyNum(order.Pro_id, order.U_traveldate);
                                if (emptynum < order.U_num)
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "下手晚了，已经卖光了" });
                                }
                            }
                            //服务类型是 酒店客房，需要判断空位数量
                            if (pro_t.Server_type == 9)
                            {
                                if (order.M_b2b_order_hotel != null)
                                {
                                    int minemptynum = new B2b_com_LineGroupDateData().GetMinEmptyNum(pro_t.Id, order.M_b2b_order_hotel.Start_date, order.M_b2b_order_hotel.End_date);

                                    if (minemptynum < order.U_num)//客房有不可预订的情况
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = "下手晚了，已经卖光了" });
                                    }
                                }
                            }
                            //服务类型是 票务 或者 实物产品，需要判断限购总量 
                            if (pro_t.Server_type == 1 || pro_t.Server_type == 11 || pro_t.Server_type == 3 || pro_t.Server_type == 12)
                            {
                                if (pro_t.Limitbuytotalnum < order.U_num)
                                {
                                    if (pro_t.Server_type == 3)
                                    {

                                        //短信内容发送微信客服通道
                                        var data = CustomerMsg_Send.SendWxMsg(comid, order.Openid, 1, "", "本次赠送产品 “" + pro_t.Pro_name + "”活动已经送光了，请在微信中关注下一次活动时间。谢谢！", "", basicc.Weixinno);
                                    }

                                    return JsonConvert.SerializeObject(new { type = 1, msg = "下手晚了，已经卖光了" });
                                }
                            }
                        }
                    }
                    #endregion

                    #region 如果产品参加抢购,判断产品是否有未支付订单
                    if (pro_t.Ispanicbuy == 1)
                    {
                        //同一用户对于同一产品是否有未支付订单
                        int ishasnotpay = new B2bOrderData().Ishasnotpayorder(order.U_id, order.Pro_id);
                        if (ishasnotpay == 1)
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "请完成你的未支付订单" });
                        }
                    }
                    #endregion


                    orderid = orderdate.InsertOrUpdate(order);
                    order.Id = orderid;
                    if (pro_t != null)
                    {
                        #region 把慧择网保险信息录入订单子表
                        if (pro_t.Source_type == 3 && pro_t.Server_type == 14)
                        {

                            //得到规格详情，提取保障期限 和 购买份数,以便于获得 终保时间 和 购买份数
                            string dxenddate = "";
                            int dxbuynum = 0;
                            bool isdxsuc = TqBaoxinmsg(order.speciid, order.U_traveldate, out dxenddate, out dxbuynum);

                            //保存订单信息
                            Api_hzins_OrderApplyReq_Application mhzins1 = new Api_hzins_OrderApplyReq_Application
                            {
                                id = 0,
                                orderid = orderid,
                                applicationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                startDate = order.U_traveldate.ToString("yyyy-MM-dd"),
                                endDate = dxenddate,
                                singlePrice = 0
                            };
                            int mhzins1id = new Api_hzins_OrderApplyReq_ApplicationData().EditOrderApplyReq_Application(mhzins1);
                            //投保人信息
                            Api_hzins_OrderApplyReq_applicantInfo mhzins2 = new Api_hzins_OrderApplyReq_applicantInfo
                            {
                                id = 0,
                                cName = order.baoxiannames.Split(',')[0],
                                eName = order.baoxianpinyinnames == "" ? "" : order.baoxianpinyinnames.Split(',')[0],
                                cardType = (int)Hzins_cardType.Shenfen,
                                cardCode = order.baoxianidcards.Split(',')[0],
                                sex = IdcardHelper.GetSexNoFromIdCard(order.baoxianidcards.Split(',')[0]),
                                birthday = IdcardHelper.GetBrithdayFromIdCard(order.baoxianidcards.Split(',')[0]),
                                mobile = order.U_phone,
                                email = "service@etown.cn",
                                jobInfo = "",
                                orderid = orderid
                            };
                            int mhzins2id = new Api_hzins_OrderApplyReq_applicantInfoData().EditOrderApplyReq_applicantInfo(mhzins2);

                            //被保险人信息
                            string[] dxInsurantInfostr = order.baoxiannames.Split(',');
                            for (int i = 0; i < dxInsurantInfostr.Length; i++)
                            {
                                if (dxInsurantInfostr[i] != "")
                                {
                                    int relationId = (int)Hzins_relationId.Qita;
                                    if (i == 0)
                                    {
                                        relationId = (int)Hzins_relationId.Benren;
                                    }
                                    Api_hzins_OrderApplyReq_insurantInfo mhzins3 = new Api_hzins_OrderApplyReq_insurantInfo
                                    {
                                        id = 0,
                                        insurantId = i.ToString(),
                                        cName = order.baoxiannames.Split(',')[i],
                                        eName = order.baoxianpinyinnames == "" ? "" : order.baoxianpinyinnames.Split(',')[i],
                                        sex = IdcardHelper.GetSexNoFromIdCard(order.baoxianidcards.Split(',')[i]),
                                        cardType = (int)Hzins_cardType.Shenfen,
                                        cardCode = order.baoxianidcards.Split(',')[i],
                                        birthday = IdcardHelper.GetBrithdayFromIdCard(order.baoxianidcards.Split(',')[i]),
                                        relationId = relationId,
                                        count = dxbuynum,
                                        singlePrice = B2b_com_pro_SpeciData.Getgginfobyggid(order.Speciid) == null ? 0 : B2b_com_pro_SpeciData.Getgginfobyggid(order.Speciid).speci_agentsettle_price,
                                        fltNo = "",
                                        fltDate = "",
                                        city = "",
                                        tripPurposeId = (int)Hzins_tripPurposeId.Lvyou,
                                        destination = "",
                                        visaCity = "",
                                        jobInfo = "",
                                        mobile = "",
                                        orderid = orderid
                                    };
                                    int mhzins3id = new Api_hzins_OrderApplyReq_insurantInfoData().EditOrderApplyReq_insurantInfo(mhzins3);
                                }
                            }

                        }
                        #endregion

                        #region 万龙接口订单
                        if (pro_t.Source_type == 3 && pro_t.Serviceid == 4)
                        {
                            try
                            {
                               
                                WlGetProInfoDealRequestData wldata = new WlGetProInfoDealRequestData(commanage.B2bcompanyinfo.wl_PartnerId, commanage.B2bcompanyinfo.wl_userkey);

                                WlDealResponseBody WlDealinfo = wldata.SelectonegetWlProDealData(pro_t.Service_proid, comid);
                                if (WlDealinfo == null)
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "wl绑定产品出错，创建订单失败" });
                                }
                                double toal = WlDealinfo.marketPrice * order.U_num;
                                string tavedate = order.U_traveldate.ToString();
                                var createwlorder = wldata.wlOrderCreateRequest_json(int.Parse(commanage.B2bcompanyinfo.wl_PartnerId), order.U_name, order.U_phone, orderid.ToString(), order.Pro_id.ToString(), WlDealinfo.proID, WlDealinfo.settlementPrice, WlDealinfo.marketPrice, toal, order.U_num, tavedate);//

                                var wlcreate = wldata.wlOrderCreateRequest_data(createwlorder, comid);
                                if (wlcreate.IsSuccess == true)
                                {
                                    //wl订单创建成功
                                }
                                else {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "wl接口创建订单失败1" });
                                }

                            }
                            catch
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "wl接口创建订单失败2" });
                            }
                        }
                        #endregion
                        #region 把美景联动订单信息录入订单子表
                        if (pro_t.Source_type == 3 && pro_t.Serviceid == 3)
                        {
                            ApiService mapiservice = new ApiServiceData().GetApiservice(pro_t.Serviceid);
                            if (mapiservice != null)
                            {
                                Api_Mjld_SubmitOrder_input minput = new Api_Mjld_SubmitOrder_input
                                {
                                    id = 0,
                                    timeStamp = CommonFunc.ConvertDateTimeInt(DateTime.Now).ToString(),
                                    user = mapiservice.Servicername.ToString(),
                                    password = mapiservice.Password,
                                    goodsId = pro_t.Service_proid,
                                    num = order.U_num.ToString(),
                                    phone = order.U_phone,
                                    batch = "0",//<!-值填1时一码一票，值填0或不填该字段是一码多票>
                                    guest_name = order.U_name,
                                    identityno = "",
                                    order_note = "",
                                    forecasttime = pro_t.isneedbespeak == 1 ? order.U_traveldate.ToString("yyyy-MM-dd HH:mm:ss") : "",//预定时间【产品详情里IsReserve=True时，需传递该时间；IsReserve=False时，必须保留该值为空】
                                    consignee = "",
                                    address = "",
                                    zipcode = "",
                                    orderId = order.Id
                                };
                                int mmjldid = new Api_mjld_SubmitOrder_inputData().EditApi_mjld_SubmitOrder_input(minput);
                            }
                            else
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "产品服务商信息查询失败" });

                            }
                        }
                        #endregion
                        #region 把阳光订单信息录入订单子表
                        if (pro_t.Source_type == 3 && pro_t.Serviceid == 1)
                        {
                            ApiService mapiservice = new ApiServiceData().GetApiservice(pro_t.Serviceid);
                            if (mapiservice != null)
                            {
                                Api_yg_addorder_input minput = new Api_yg_addorder_input
                                {
                                    id = 0,
                                    organization = mapiservice.Organization,
                                    password = mapiservice.Password,
                                    req_seq = mapiservice.Organization + DateTime.Now.ToString("yyyyMMddhhssmm") + CommonFunc.CreateNum(6),//请求流水号,
                                    product_num = pro_t.Service_proid.ToString(),
                                    num = order.U_num,
                                    mobile = order.U_phone,
                                    use_date = "",
                                    real_name_type = 0,
                                    real_name = "",
                                    id_card = "",
                                    card_type = 0,
                                    orderId = order.Id
                                };
                                int mmjldid = new Api_yg_addorder_inputData().EditApi_yg_addorder_input(minput);
                            }
                            else
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "产品服务商信息查询失败" });

                            }
                        }
                        #endregion
                    }
                    try
                    {
                        //给直销订单录入推荐渠道人id
                        new B2bOrderData().UporderRecommendMannelid(order.U_id, orderid);
                    }
                    catch { }
                    bindingorderid = orderid;


                    //抵扣积分/预付款
                    B2b_crm b2b_crm = new B2bCrmData().b2b_crmH5(order.Openid, comid);


                    //获得IP
                    string addressIP = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()).GetValue(0).ToString();
                    int pro = 0;

                    //判断金额有效性
                    bool isNum = Domain_def.RegexValidate("^[0123456789.]*$", (order.U_num * order.Pay_price).ToString());
                    if (!isNum)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "操作错误，金额只能包含数字" });
                    }


                    #region 优惠券电子票产品，不需要支付，直接发送电子票（默认已支付）,预约产品，如果是教练订单 支付状态 修改为 等待订单确认 支付
                    if (pro_t.Server_type == 3 || pro_t.Server_type == 12 || pro_t.Server_type == 13)
                    {
                        //优惠券电子票产品，不需要支付，直接发送电子票（默认已支付）
                        //根据订单id得到订单信息
                        B2bOrderData dataorder = new B2bOrderData();
                        B2b_order modelb2border = dataorder.GetOrderById(orderid);
                        if (modelb2border == null)
                        {
                            dikou = "没有查询到此笔订单";
                        }
                        else
                        {
                            if (pro_t.Server_type == 12)
                            {//预约产品
                                if (pro_t.bookpro_ispay == 0)
                                {//不需要支付
                                    //---------------新增1begin--------------//
                                    modelb2border.Pay_state = 2;
                                    modelb2border.Order_state = 2;
                                    modelb2border.Pay_price = 0;//因为 是预约的 把支付金额清0 ，如果需要可以去掉
                                    //修改此订单的支付状态为“支付成功” ,订单状态为“已付款”
                                    dataorder.InsertOrUpdate(modelb2border);
                                    dikou = new SendEticketData().SendEticket(orderid, 1);
                                }

                            }
                            else if (pro_t.Server_type == 13)
                            {//教练产品

                                if (pro_t.unsure == 1)//如果需要商家确认，则赋值未0，需要
                                {
                                    modelb2border.Pay_state = 0;//待商家绑定人确认
                                }
                                else
                                {
                                    modelb2border.Pay_state = 1;//待支付
                                }
                                //修改此订单的支付状态为“支付成功” ,订单状态为“已付款”
                                dataorder.InsertOrUpdate(modelb2border);



                            }
                            else
                            {//赠送产品

                                //---------------新增1begin--------------//
                                modelb2border.Pay_state = 2;
                                modelb2border.Order_state = 2;
                                //修改此订单的支付状态为“支付成功” ,订单状态为“已付款”
                                dataorder.InsertOrUpdate(modelb2border);

                                dikou = new SendEticketData().SendEticket(orderid, 1);
                            }
                        }
                    }
                    #endregion

                    #region 酒店客房产品,向附属表(酒店客房订单表)录入数据 并向客户发送预订成功短信、向酒店负责人发送提醒短信
                    if (pro_t.Server_type == 9)//酒店客房产品
                    {
                        if (order.M_b2b_order_hotel != null)
                        {
                            B2b_order_hotel m_orderhotel = order.M_b2b_order_hotel;
                            m_orderhotel.Orderid = orderid;
                            m_orderhotel.Bookdaynum = (m_orderhotel.End_date - m_orderhotel.Start_date).Days;//入住天数
                            //向附属表(酒店客房订单表)录入数据
                            int m_orderhotelid = new B2b_order_hotelData().InsertOrUpdate(m_orderhotel);
                            order.M_b2b_order_hotel.Id = m_orderhotelid;

                            B2b_com_housetype m_b2b_com_housetype = new B2b_com_housetypeData().GetHouseType(pro_t.Id, comid);
                            if (m_b2b_com_housetype != null)
                            {
                                //获取酒店所在项目的名称
                                string projectname = new B2b_com_projectData().GetProjectNameByid(pro_t.Projectid);

                                if (m_b2b_com_housetype.ReserveType == 1)//不用支付直接发送订房短信
                                {
                                    Smsmodel smodel = new Smsmodel()//微信酒店预订服务商通知短信
                                    {
                                        RecerceSMSPhone = m_b2b_com_housetype.RecerceSMSPhone,
                                        Phone = order.U_phone,
                                        Name = order.U_name,
                                        Title = projectname + pro_t.Pro_name,
                                        //Money = order.Pay_price*order.U_num,
                                        Key = "微信酒店预订服务商通知短信",
                                        Comid = comid,
                                        Num = order.U_num,//间数
                                        Num1 = m_orderhotel.Bookdaynum,//天数
                                        Starttime = m_orderhotel.Start_date,
                                        Endtime = m_orderhotel.End_date,

                                    };
                                    Smsmodel kmodel = new Smsmodel()//客户发送内容
                                    {
                                        Phone = order.U_phone,
                                        Name = order.U_name,
                                        Title = projectname + pro_t.Pro_name,
                                        //Money = order.Pay_price*order.U_num,
                                        Key = "预订酒店短信",
                                        Comid = comid,
                                        Num = order.U_num,//间数
                                        Num1 = m_orderhotel.Bookdaynum,//天数
                                        Starttime = m_orderhotel.Start_date,
                                        Endtime = m_orderhotel.End_date,
                                    };
                                    //向客户发送预定短信
                                    SendSmsHelper.Member_smsBal(kmodel);
                                    //向酒店负责人发送客人的预定信息
                                    SendSmsHelper.Member_smsBal(smodel);
                                }
                            }
                            else
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "获取酒店客房信息失败" });
                            }
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "酒店订单提交失败" });
                        }
                    }
                    #endregion
                    #region 跟团游、当地游,向客户发送预定短信
                    if (pro_t.Server_type == 2 || pro_t.Server_type == 8)//跟团游、当地游
                    {
                        Smsmodel kmodel = new Smsmodel()//客户发送内容
                        {
                            Phone = order.U_phone,
                            Name = order.U_name,
                            Title = pro_t.Pro_name,
                            Key = "线路提单直接发送短信",
                            Comid = comid,
                            Starttime = order.U_traveldate,
                            Customtext = order.Ticketinfo

                        };
                        //向客户发送预定短信
                        SendSmsHelper.Member_smsBal(kmodel);
                    }
                    #endregion
                    #region 票务、旅游大巴,订房,使用积分预付款等于订单金额则直接修改此订单的支付状态为“支付成功” ,订单状态为“已付款”，并且发送电子票；否则对未支付或扣款的票务，提交一个分销订单，但不进行财务处理和发送
                    if (pro_t.Server_type == 1 || pro_t.Server_type == 10 || pro_t.Server_type == 9 || pro_t.Server_type == 11 || pro_t.Server_type == 14)
                    {
                        #region 使用积分预付款等于订单金额
                        if (order.Integral1 + order.Imprest1 == order.U_num * order.Pay_price)
                        {
                            if (order.Integral1 != 0)
                            {
                                MemberIntegralData intdate = new MemberIntegralData();
                                Member_Integral Intinfo = new Member_Integral()
                                {
                                    Id = b2b_crm.Id,
                                    Comid = comid,
                                    Acttype = "reduce_integral",           //操作类型
                                    Money = 0 - order.Integral1,              //交易金额
                                    Admin = "订单使用",
                                    Ip = addressIP,
                                    Ptype = 2,
                                    Oid = 0,
                                    Remark = "",
                                    OrderId = orderid,
                                    OrderName = pro_t.Pro_name
                                };
                                pro = intdate.InsertOrUpdate(Intinfo);
                            }


                            if (order.Imprest1 != 0)
                            {
                                //预付款
                                //BusinessCustomersJsonData.WriteMoney(b2b_crm.Id, comid, "reduce_imprest", order.Integral1.ToString(), orderid, pro_t.Pro_name);
                                MemberImprestData impdate = new MemberImprestData();
                                Member_Imprest Impinfo = new Member_Imprest()
                                {
                                    Id = b2b_crm.Id,
                                    Comid = comid,
                                    Acttype = "reduce_imprest",           //操作类型
                                    Money = 0 - order.Imprest1,              //交易金额
                                    Admin = "订单使用",
                                    Ip = addressIP,
                                    Ptype = 2,
                                    Oid = 0,
                                    Remark = "",
                                    OrderId = orderid,
                                    OrderName = pro_t.Pro_name

                                };
                                pro = impdate.InsertOrUpdate(Impinfo);
                            }
                            //根据订单id得到订单信息
                            B2bOrderData dataorder = new B2bOrderData();
                            B2b_order modelb2border = dataorder.GetOrderById(orderid);

                            if (modelb2border == null)
                            {
                                dikou = "没有查询到此笔订单";
                            }


                            #region 当提交订单时，不管来路，如果订单处理成功进入发码流程前，提交一笔原始分销订单。
                            if (pro_t.Source_type == 4)
                            {
                                int pro_id_old = pro_t.Bindingid;//原产品ID
                                int comid_old = order.Comid;//提交订单商户ID
                                int agentid_old = 0;//提交订单商户绑定的分销id
                                B2bCompanyData comdata = new B2bCompanyData();
                                var cominfo = comdata.GetCompanyBasicById(comid_old);
                                if (cominfo != null)
                                {
                                    agentid_old = cominfo.Bindingagent;
                                }


                                var daoruguigeSpeciid = order.Speciid;
                                //重新读取绑定的规格
                                if (order.Speciid != 0)
                                {
                                    var guiginfo = B2b_com_pro_SpeciData.Getgginfobyggid(order.Speciid);
                                    if (guiginfo != null)
                                    {
                                        daoruguigeSpeciid = guiginfo.binding_id;
                                    }
                                }
                                AgentOrder(agentid_old, pro_id_old.ToString(), "1", order.U_num.ToString(), order.U_name, order.U_phone, order.U_traveldate.ToString(), "", 1, out bindingorderid_huoqu, 0, 1, order.pickuppoint, order.dropoffpoint, "", 0, "", "", "", "", 0, 0, 0, modelb2border.yanzheng_method, daoruguigeSpeciid, order.baoxiannames, order.baoxianpinyinnames, order.baoxianidcards, 0, order.M_b2b_order_hotel, order.Id, order.travelnames, order.travelidcards, order.travelnations, order.travelphones, order.travelremarks,modelb2border.U_idcard);
                                //bindingorderid 是分销返回的订单号，必须成功否则报错
                                //导入产品，如果提交订单错误，则可能是分销产品下线，分销钱不够，或者其他问题没有提交订单
                                if (bindingorderid_huoqu == 0)
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "提交订单错误，请联系商家！" });
                                }
                                B2b_order agentsunorder = dataorder.GetOrderById(bindingorderid_huoqu);
                                if (agentsunorder == null)
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "提交订单错误，请联系商家！" });
                                }
                                if (agentsunorder.Pay_state == 1)
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "提交订单错误，商家额度不足，请联系商家！" });
                                }
                                modelb2border.Bindingagentorderid = bindingorderid_huoqu;
                            }
                            #endregion

                            //---------------新增1begin--------------//
                            modelb2border.Pay_state = 2;
                            modelb2border.Order_state = 2;
                            //修改此订单的支付状态为“支付成功” ,订单状态为“已付款”
                            dataorder.InsertOrUpdate(modelb2border);

                            dikou = new SendEticketData().SendEticket(orderid, 1, "", order.aorderid);
                        }
                        #endregion
                        #region 使用积分预付款不等于订单金额
                        else
                        {//对未支付或扣款的票务，提交一个分销订单，但不进行财务处理和发送

                            B2bOrderData dataorder = new B2bOrderData();
                            B2b_order modelb2border = dataorder.GetOrderById(orderid);

                            #region 当提交订单时，不管来路，如果订单处理成功进入发码流程前，提交一笔原始分销订单。
                            if (pro_t.Source_type == 4)
                            {
                                int pro_id_old = pro_t.Bindingid;//原产品ID
                                int comid_old = order.Comid;//提交订单商户ID
                                int agentid_old = 0;//提交订单商户绑定的分销id
                                B2bCompanyData comdata = new B2bCompanyData();
                                var cominfo = comdata.GetCompanyBasicById(comid_old);
                                if (cominfo != null)
                                {
                                    agentid_old = cominfo.Bindingagent;
                                }
                                var daoruguigeSpeciid = order.Speciid;
                                //重新读取绑定的规格
                                if (order.Speciid != 0)
                                {
                                    var guiginfo = B2b_com_pro_SpeciData.Getgginfobyggid(order.Speciid);
                                    if (guiginfo != null)
                                    {
                                        daoruguigeSpeciid = guiginfo.binding_id;
                                    }
                                }
                                AgentOrder(agentid_old, pro_id_old.ToString(), "1", order.U_num.ToString(), order.U_name, order.U_phone, order.U_traveldate.ToString(), "", 1, out bindingorderid_huoqu, 0, 0, order.pickuppoint, order.dropoffpoint, "", 0, "", "", "", "", 0, 0, 0, modelb2border.yanzheng_method, daoruguigeSpeciid, order.baoxiannames, order.baoxianpinyinnames, order.baoxianidcards, 0, order.M_b2b_order_hotel, order.Id, order.travelnames, order.travelidcards, order.travelnations, order.travelphones, order.travelremarks, modelb2border.U_idcard);
                                //bindingorderid 是分销返回的订单号，必须成功否则报错
                                //导入产品，如果提交订单错误，则可能是分销产品下线，分销钱不够，或者其他问题没有提交订单
                                if (bindingorderid_huoqu == 0)
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "提交订单错误，请联系商家！" });
                                }
                                B2b_order agentsunorder = dataorder.GetOrderById(bindingorderid_huoqu);
                                if (agentsunorder == null)
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "提交订单错误，请联系商家！" });
                                }
                                if (agentsunorder.Pay_state == 1)
                                {
                                    //return JsonConvert.SerializeObject(new { type = 1, msg = "提交订单错误，商家额度不足，请联系商家！" });
                                }
                            }
                            #endregion
                            modelb2border.Bindingagentorderid = bindingorderid_huoqu;
                            dataorder.InsertOrUpdate(modelb2border);
                        }
                        #endregion
                    }
                    #endregion

                    #region 服务类型是旅游大巴 ,不管是不是抢购/限购功能， 空位都直接减少
                    if (pro_t.Server_type == 10)
                    {
                        //需要把出团当天的空位减少
                        int reduce_emptynum = new B2b_com_LineGroupDateData().ReduceEmptyNum(order.Pro_id, order.U_num, order.U_traveldate);
                    }
                    #endregion
                    #region 其他服务类型，如果是抢购/限购产品，订单提交完成以后空位减少
                    else
                    {
                        if (pro_t.Ispanicbuy == 1 || pro_t.Ispanicbuy == 2)
                        {
                            if (pro_t.Server_type == 2 || pro_t.Server_type == 8)
                            {
                                //需要把出团当天的空位减少
                                int reduce_emptynum = new B2b_com_LineGroupDateData().ReduceEmptyNum(order.Pro_id, order.U_num, order.U_traveldate);
                            }

                            if (pro_t.Server_type == 1 || pro_t.Server_type == 11 || pro_t.Server_type == 3)//服务类型：票务;实物产品
                            {
                                //提单时 原始产品可销售数量减少，已销售数量增加，导入产品无需变动；
                                if (pro_t.Bindingid == 0)
                                {
                                    //把票务的可销售数量减少；同时已销售数量增加
                                    int reduce_ticket = new B2bComProData().ReduceLimittotalnum(order.Pro_id, order.U_num);

                                }
                            }
                        }

                        if (pro_t.Server_type == 9)//酒店 
                        {
                            //需要把入住时间内所有天的空位减少
                            int reduce_emptynum = new B2b_com_LineGroupDateData().ReduceEmptyNum(order.Pro_id, order.U_num, order.M_b2b_order_hotel.Start_date, order.M_b2b_order_hotel.End_date);
                        }

                        //对规格库存进行扣减
                        if (order.Speciid != 0)
                        {
                            int reduce_ticket = new B2bComProData().ReduceLimittotalSpeciidnum(order.Pro_id, order.Speciid, order.U_num);
                        }

                    }
                    #endregion


                    #region  //如果是 前台提单自动成功,不需要支付
                    if (order.autosuccess == 1)
                    {

                        B2bOrderData dataorder = new B2bOrderData();
                        B2b_order modelb2border = dataorder.GetOrderById(orderid);

                        modelb2border.Ticketinfo = modelb2border.Order_remark;
                        modelb2border.submanagename = order.submanagename;
                        modelb2border.mangefinset = 1;
                        
                        dataorder.InsertOrUpdate(modelb2border);

                        //处理成功
                        var chenggong = new PayReturnSendEticketData().chulidingdan(orderid, order.U_num * order.Pay_price);

                    }
                    #endregion

                    #region //预约码预约自动处理成功 pno 必须是加密的 直接解密操作一次
                    if (order.yuyuepno != "" && order.yuyuepno != null)
                    {
                        if (pro_t.Server_type != 3)//赠送产品 不能再次处理
                        {
                            //上面订单已提交，现在只要处理成功即可

                            string pno1 = EncryptionHelper.EticketPnoDES(order.yuyuepno, 1);//解密

                            ////查询绑定状态
                            var busbindingdata = new Bus_FeeticketData();
                            var busbindinginfo = busbindingdata.Bus_Feeticket_proById(0, 0, order.Pro_id, pno1);

                            if (busbindinginfo == null)
                            {

                                return JsonConvert.SerializeObject(new { type = 1, msg = "预约码不匹配" });
                            }


                            if (busbindinginfo != null)
                            {
                                //对预约码和预订产品进行匹配必须有相应匹配的绑定才能预订，没有绑定直接 返回错误，
                                var pipei = busbindingdata.BusSearchpno_propipei(pno1, pro_t.Id);

                                if (pipei == 0)
                                {

                                    return JsonConvert.SerializeObject(new { type = 1, msg = "预约码不匹配" });
                                }

                                if (busbindinginfo.Busid != 0)
                                {
                                    var busfeetticketinfo = busbindingdata.GetBus_FeeticketById(busbindinginfo.Busid, pro_t.Com_id);
                                    if (busfeetticketinfo != null)
                                    {
                                        int Iuse = busfeetticketinfo.Iuse;

                                        //对
                                        if (Iuse == 1)
                                        {

                                            var Eticketprodata = new B2bEticketData();
                                            var eticketinfo = Eticketprodata.GetEticketDetail(pno1);
                                            if (eticketinfo != null)
                                            {
                                                if (eticketinfo.bindingname != "")
                                                {
                                                    if (eticketinfo.bindingname.Trim() != order.U_name.Trim())
                                                    {
                                                        return JsonConvert.SerializeObject(new { type = 1, msg = "预约人与绑定人不匹配" });
                                                    }
                                                }
                                            }


                                            //对单日 每个姓名只能预订一人
                                            var dangriyuyue = orderdate.GedangriyuyueBypno(pno1, order.Pro_id, order.U_traveldate);
                                            if (dangriyuyue > 0)
                                            {
                                                return JsonConvert.SerializeObject(new { type = 1, msg = "此预约码限制本人预约，此班车 您已有 " + order.U_traveldate + "的预约。" });
                                            }
                                        }
                                        if (Iuse == 2)
                                        {
                                            //对单日 每个姓名只能预订一人
                                            var dangriyuyue = orderdate.GedangriyuyueBypno(pno1, order.Pro_id, order.U_traveldate);
                                            if (dangriyuyue > 0)
                                            {
                                                return JsonConvert.SerializeObject(new { type = 1, msg = "此预约码限制每班车只能预约1人，您已有 " + order.U_traveldate + "的预约。" });
                                            }
                                        }


                                        //对数量控制
                                        int limitweek = busbindinginfo.limitweek;
                                        int limitweekdaynum = busbindinginfo.limitweekdaynum;
                                        int limitweekendnum = busbindinginfo.limitweekendnum;

                                        int nowdatetype = 0;//0平日；1周末；2节假日

                                        //只有对 对周末 和 平日 单独限定后才进行判定
                                        if (limitweek == 1)
                                        {
                                            //判断出游日期是否为平日或周末
                                            B2b_com_blackoutdates m_blackoutdate = new B2b_com_blackoutdatesData().Getblackoutdate(order.U_traveldate.ToString("yyyy-MM-dd"), pro_t.Com_id);

                                            if (m_blackoutdate == null)
                                            {
                                                //按系统默认规则:周六日为周末，其他为平日;无节假日
                                                if (order.U_traveldate.DayOfWeek == DayOfWeek.Saturday || order.U_traveldate.DayOfWeek == DayOfWeek.Sunday)
                                                {
                                                    nowdatetype = 1;
                                                }
                                                else
                                                {
                                                    nowdatetype = 0;
                                                }
                                            }
                                            else
                                            {
                                                //按系统设定
                                                nowdatetype = m_blackoutdate.datetype;
                                            }

                                            // 您预约 周末或节假日 ，查看 预约绑定限制是否限制使用
                                            if (nowdatetype != 0)
                                            {
                                                if (limitweekendnum < 0)
                                                {
                                                    return JsonConvert.SerializeObject(new { type = 1, msg = "此码限定平日使用" });
                                                }
                                            }

                                            // 您预约 平日,暂时 限定
                                            if (nowdatetype == 0)
                                            {
                                                if (limitweekdaynum < 0)
                                                {
                                                    return JsonConvert.SerializeObject(new { type = 1, msg = "此码限定周末使用" });
                                                }
                                            }

                                            if (nowdatetype == 0)//对平日限制数量
                                            {
                                                if (limitweekdaynum > 0)
                                                {
                                                    int yuyueweek = 2;//2 不限制，0平日，1周末
                                                    if (limitweek == 1)
                                                    {
                                                        yuyueweek = 0;
                                                    }
                                                    var yiyuyueshuliang = orderdate.GetOrderyuyuepnoshulaingBypno(pno1, yuyueweek);
                                                    if (yiyuyueshuliang >= limitweekdaynum)
                                                    {
                                                        return JsonConvert.SerializeObject(new { type = 1, msg = "此预约码已经预约" + yiyuyueshuliang + "次,超出预约限制,请预约其他绑定的产品" });
                                                    }

                                                    if (order.U_num > limitweekdaynum)
                                                    {
                                                        return JsonConvert.SerializeObject(new { type = 1, msg = "此预约码超出了预约限制 " + limitweekdaynum + "次，请预约其他绑定的产品" });
                                                    }

                                                }
                                            }
                                            else
                                            {//对周末限制数量

                                                if (limitweekendnum > 0)
                                                {
                                                    //周末
                                                    var yiyuyueshuliang = orderdate.GetOrderyuyuepnoshulaingBypno(pno1, 1);
                                                    if (yiyuyueshuliang >= limitweekendnum)
                                                    {
                                                        return JsonConvert.SerializeObject(new { type = 1, msg = "此预约码已经预约周末" + yiyuyueshuliang + "次,超出预约限制,请预约其他日期" });
                                                    }
                                                    if (order.U_num > limitweekendnum)
                                                    {
                                                        return JsonConvert.SerializeObject(new { type = 1, msg = "此预约码超出了预约限制 " + limitweekdaynum + "次，请预约其他绑定的产品" });
                                                    }
                                                }
                                            }

                                        }

                                        //修改订单 预约 是周末还是平日，预约成功 才会统计，预约不成功 只对标记不做统计限制，只真毒
                                        var yuyuweek_data = new B2bOrderData().Upyuyueweekid(orderid, nowdatetype);
                                    }

                                }



                            }



                            //验证电子票
                            string returndata = EticketJsonData.EConfirm(pno1, order.U_num.ToString(), pro_t.Com_id.ToString(), 999999999);

                            ETS.JsonFactory.TwoCodeJsonData.JsonCommonEntity entity = (ETS.JsonFactory.TwoCodeJsonData.JsonCommonEntity)JsonConvert.DeserializeObject(returndata, typeof(ETS.JsonFactory.TwoCodeJsonData.JsonCommonEntity));
                            int type = entity.Type;
                            string msg = entity.Msg;

                            if (type == 100)
                            {//验证成功

                                B2bOrderData dataorder = new B2bOrderData();
                                B2b_order modelb2border = dataorder.GetOrderById(orderid);

                                modelb2border.Order_remark = "自助预约:" + pno1;
                                modelb2border.Ticketinfo = "自助预约:" + pno1;
                                modelb2border.Pay_price = 0;
                                modelb2border.yuyuepno = pno1;
                                dataorder.InsertOrUpdate(modelb2border);




                                //处理成功
                                var chenggong = new PayReturnSendEticketData().chulidingdan(orderid, order.U_num * order.Pay_price);


                                #region 赠送保险
                                OrderJsonData.ZengsongBaoxian(orderid);
                                #endregion



                                //对电子码第一次使用进行绑定使用人
                                var eticketdata = new B2bEticketData();
                                var eticketinfo = eticketdata.GetEticketDetail(pno1);
                                if (eticketinfo != null)
                                {
                                    if (eticketinfo.bindingname == "")
                                    {

                                        eticketinfo.bindingname = modelb2border.U_name;
                                        eticketinfo.bindingphone = modelb2border.U_phone;
                                        //eticketinfo.bindingcard = ""; //暂时不记录身份证，为了安全起见

                                        var upbinding = eticketdata.bindingpnoUpdatepeople(eticketinfo);
                                    }

                                }


                            }
                            else
                            {

                                return JsonConvert.SerializeObject(new { type = 1, msg = msg });

                            }
                        }
                    }

                    #endregion

                    #region  //发送消息模板
                    try
                    {
                        if (pro_t.Server_type != 3)//赠送产品（优惠券产品不发送订单消息）
                        {
                            //如果是教练产品，发送消息模板
                            if (pro_t.Server_type == 13)
                            {


                                //修改会员绑定的教练
                                int upchannel = new MemberCardData().upCardcodeChannel(usercard.ToString(), order.channelcoachid);
                                //检测如果会员为渠道，则解绑其锁定客户
                                int userchannelid = new MemberChannelcompanyData().getchannelidbyweixin(order.Comid, "", order.U_id);
                                if (userchannelid != 0)
                                {
                                    var updata = new MemberChannelData().WxMessageUnLockUser(userchannelid);//清楚 顾问绑定用户
                                }


                                if (pro_t.unsure == 1)//如果需要商家确认，则赋值未0，需要
                                {

                                    if (order.autosuccess != 1)//对于前台提单不发送确认。
                                    {
                                        //短信通知
                                        var querenduanxin = pro_t.bookpro_bindname + " 您绑定预约产品：" + pro_t.Pro_name + " 客户:" + order.U_name + " (" + order.U_phone + ") 提交一笔订单，预约时间:" + order.U_traveldate.ToString("yyyy-MM-dd hh:mm") + " 的预订,订单号:" + orderid + ",请立即回复短信 确认订单请回复 qr" + orderid + " ，取消订单请回复 qx" + orderid + " 您确认后客服才能进行支付。";
                                        var msg = "";
                                        var sendback = SendSmsHelper.SendSms(pro_t.bookpro_bindphone, querenduanxin, order.Comid, out msg);

                                        string Returnmd5_temp = EncryptionHelper.ToMD5(orderid.ToString() + "lixh1210", "UTF-8");
                                        var querenduanxin_weixin = "教练产品预约确认：\n\n" + pro_t.bookpro_bindname + " \n\n产品名称：" + pro_t.Pro_name + " \n\n客户:" + order.U_name + " (" + order.U_phone + ") 提交一笔订单，\n\n预约时间:" + order.U_traveldate.ToString("yyyy-MM-dd hh:mm") + " \n\n您确认后客服才能进行支付，点击下面链接进行确认。\n http://shop" + pro_t.Com_id + ".etown.cn/h5/Confirmyuyue.aspx?id=" + orderid + "&md5=" + Returnmd5_temp;
                                        //对绑定顾问发送微信客服通道通知
                                        CustomerMsg_Send.SendWxkefumsg(orderid, 1, querenduanxin_weixin, comid);//给绑定顾问发送
                                    }
                                }
                            }
                            //else if (pro_t.Server_type == 9)
                            //{
                            //    if (order.autosuccess != 1)//对于前台提单不发送确认。
                            //    {
                            //        string projectname = new B2b_com_projectData().GetProjectNameByid(pro_t.Projectid);
                            //        //短信通知
                            //        var querenduanxin = pro_t.bookpro_bindname + " 有客户预订房间：" + projectname + pro_t.Pro_name + " 客户:" + order.U_name + " (" + order.U_phone + ") ，入住时间:" + order.M_b2b_order_hotel.Start_date.ToString("yyyy-MM-dd") + " 离店时间： " + order.M_b2b_order_hotel.End_date.ToString("yyyy-MM-dd") + " ,订单号:" + orderid + ",请立即回复短信 确认订单请回复 qr" + orderid + " ，取消订单请回复 qx" + orderid + " 您确认后客服才能进行支付。";
                            //        var msg = "";
                            //        var sendback = SendSmsHelper.SendSms(pro_t.bookpro_bindphone, querenduanxin, order.Comid, out msg);

                            //        string Returnmd5_temp = EncryptionHelper.ToMD5(orderid.ToString() + "lixh1210", "UTF-8");
                            //        var querenduanxin_weixin = "有客户预订房间：\n\n" + projectname + pro_t.bookpro_bindname + " \n\n产品名称：" + pro_t.Pro_name + " \n\n客户:" + order.U_name + " (" + order.U_phone + ") 入住时间:" + order.U_traveldate.ToString("yyyy-MM-dd hh:mm") + " 离店时间：" + order.M_b2b_order_hotel.End_date.ToString("yyyy-MM-dd") + " \n\n您确认后客服才能进行支付，点击下面链接进行确认。\n http://shop" + pro_t.Com_id + ".etown.cn/h5/Confirmyuyue.aspx?id=" + orderid + "&md5=" + Returnmd5_temp;
                            //        //对绑定顾问发送微信客服通道通知
                            //        CustomerMsg_Send.SendWxkefumsg(orderid, 1, querenduanxin_weixin, comid);//给绑定顾问发送
                            //    }

                            //}
                            else
                            {
                                //微信模板消息-新订单生成
                                new Weixin_tmplmsgManage().WxTmplMsg_OrderNewCreate(orderid);//对客户发送

                                //对顾问发送
                                SendEticketData.SendWeixinKfMsg(orderid);
                            }

                        }
                    }
                    catch
                    {
                    }
                    #endregion

                }
                #endregion

                #region 分销订单
                else
                {//分销订单
                    int MinEmptynum = 0;//订房使用，检测是否有
                    int hotel_yes_dinggou_yuding = 0;



                    //读取分销商信息
                    Agent_company agentinfo = AgentCompanyData.GetAgentWarrant(order.Agentid, order.Comid);
                    if (agentinfo == null)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "账户信息错误" });
                    }


                    //验码扣款，预付款不能为负
                    //if (agentinfo.Imprest <= 0 && order.Warrant_type == 2 && order.Order_type == 1)
                    //{
                    //    return JsonConvert.SerializeObject(new { type = 1, msg = "您的账户余额不足" });
                    //}

                    //价格不能为0
                    if (order.Pay_price <= 0)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "您未授权订购此产品" });
                    }

                    order.Backtickettime = DateTime.Now;

                    if (pro_t != null)
                    {
                        #region  服务类型是旅游大巴，检查空位数量和订购时间
                        if (pro_t.Server_type == 10)
                        {
                            //控位数量
                            var proid_temp = order.Pro_id;
                            if (pro_t.Source_type == 4)
                            {//如果是绑定的产品查询绑定产品団期
                                proid_temp = pro_t.Bindingid;
                            }
                            int emptynum = new B2b_com_LineGroupDateData().GetEmptyNum(proid_temp, order.U_traveldate);
                            if (emptynum < order.U_num)
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "提交失败，当日已售罄，请选择其他日期" });
                            }

                            //判断订购时间
                            var today = DateTime.Today;
                            TimeSpan riqicha = order.U_traveldate - today;//提单必须提交明天以后的,如果明天的需要再次判断时间是否超出
                            int sub = riqicha.Days;     //sub就是两天相差的天数
                            if (sub < 0)
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "提交失败，此产品需提前一天预订，请选择其他日期" });
                            }

                            ////经和经理确认,截团之前都可以收人提单
                            ////提单最后时间，默认设置为 前一天 16点。
                            //var endtime = DateTime.Parse(DateTime.Today.ToString("yyyy.MM.dd") + " 16:00:00");
                            //if (sub == 1)
                            //{
                            //    //提交时间是和默认结束时间比较
                            //    if (DateTime.Now > endtime)
                            //    {
                            //        return JsonConvert.SerializeObject(new { type = 1, msg = "提交失败，您提交日期已经排车，请选择其他日期的班车！" });
                            //    }
                            //}
                        }
                        #endregion

                        if (pro_t.Server_type == 9)
                        {
                            if (order.M_b2b_order_hotel != null)
                            {
                                MinEmptynum = new B2b_com_LineGroupDateData().GetMinEmptyNum(pro_t.Id, order.M_b2b_order_hotel.Start_date, order.M_b2b_order_hotel.End_date);
                                decimal allprice = new B2b_com_LineGroupDateData().Gethotelallprice(pro_t.Id, order.M_b2b_order_hotel.Start_date, order.M_b2b_order_hotel.End_date);
                                if (MinEmptynum < order.U_num)//客房有不可预订的情况
                                {

                                    hotel_yes_dinggou_yuding = 1;//当预订数 小于 控房数 则 订单不成功，需要 绑定人确认，如果为 0 则可以直接成功
                                    if (allprice <= 0)
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = "房满，请选择其他日期或其他房型" });
                                    }
                                }
                            }
                        }


                        #region  服务类型是旅游，检查空位数量和订购时间
                        if (pro_t.Server_type == 2 || pro_t.Server_type == 8)
                        {
                            //控位数量
                            var proid_temp = order.Pro_id;
                            if (pro_t.Source_type == 4)
                            {//如果是绑定的产品查询绑定产品団期
                                proid_temp = pro_t.Bindingid;
                            }
                            int emptynum = new B2b_com_LineGroupDateData().GetEmptyNum(proid_temp, order.U_traveldate);
                            if (emptynum < order.U_num)
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "提交失败，空位数量不足" });
                            }

                            //判断订购时间
                            var today = DateTime.Today;
                            TimeSpan riqicha = order.U_traveldate - today;//提单必须提交明天以后的,如果明天的需要再次判断时间是否超出
                            int sub = riqicha.Days;     //sub就是两天相差的天数
                            if (sub <= 0)
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "提交失败，需提前一天预订" });
                            }

                        }
                        #endregion
                        #region 其他服务类型，如果是抢购/限购产品检查可销售数量/空位数量
                        else
                        {
                            if (pro_t.Ispanicbuy == 1 || pro_t.Ispanicbuy == 2)//如果是抢购/限购产品
                            {
                                if (pro_t.Ispanicbuy == 1)
                                {
                                    //提单时间是否在抢购时间之内
                                    if (DateTime.Now < pro_t.Panic_begintime || DateTime.Now > pro_t.Panicbuy_endtime)
                                    {
                                        if (DateTime.Now < pro_t.Panic_begintime)
                                        {
                                            return JsonConvert.SerializeObject(new { type = 1, msg = "抢购产品，活动还未开始" });
                                        }
                                        else
                                        {
                                            return JsonConvert.SerializeObject(new { type = 1, msg = "抢购产品，活动已结束" });
                                        }
                                    }
                                }
                                //服务类型是跟团游、当地游 ，需要判断空位数量
                                if (pro_t.Server_type == 2 || pro_t.Server_type == 8)
                                {
                                    var proid_temp = order.Pro_id;
                                    if (pro_t.Source_type == 4)
                                    {//如果是绑定的产品查询绑定产品団期
                                        proid_temp = pro_t.Bindingid;
                                    }
                                    int emptynum = new B2b_com_LineGroupDateData().GetEmptyNum(proid_temp, order.U_traveldate);
                                    if (emptynum < order.U_num)
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = "下手晚了，已经卖光了" });
                                    }
                                }
                                //服务类型是 酒店客房，需要判断空位数量
                                if (pro_t.Server_type == 9)
                                {
                                    if (order.M_b2b_order_hotel != null)
                                    {
                                        var proid_temp = order.Pro_id;
                                        if (pro_t.Source_type == 4)
                                        {//如果是绑定的产品查询绑定产品団期
                                            proid_temp = pro_t.Bindingid;
                                        }
                                        int minemptynum = new B2b_com_LineGroupDateData().GetMinEmptyNum(proid_temp, order.M_b2b_order_hotel.Start_date, order.M_b2b_order_hotel.End_date);

                                        if (minemptynum < order.U_num)//客房有不可预订的情况,当抢购或限购情况下 按已有数量购买
                                        {
                                            return JsonConvert.SerializeObject(new { type = 1, msg = "下手晚了，已经卖光了" });
                                        }
                                    }
                                }
                                //服务类型是 票务 或者 实物产品，需要判断限购总量 
                                if (pro_t.Server_type == 1 || pro_t.Server_type == 11)
                                {
                                    if (pro_t.Limitbuytotalnum < order.U_num)
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = "下手晚了，已经卖光了" });
                                    }
                                }
                            }


                        }
                        #endregion
                    }
                    //else
                    //{
                    //    //return JsonConvert.SerializeObject(new { type = 1, msg = "产品信息获取失败" });
                    //}

                    //插入订单
                    orderid = orderdate.InsertOrUpdate(order);
                    order.Id = orderid;
                    bindingorderid = orderid;//返回订单号

                    if (pro_t != null)
                    {
                        #region 把慧择网保险信息录入订单子表
                        if (pro_t.Source_type != 4 && pro_t.Server_type == 14)
                        {

                            //得到规格详情，提取保障期限 和 购买份数,以便于获得 终保时间 和 购买份数
                            string dxenddate = "";
                            int dxbuynum = 0;
                            bool isdxsuc = TqBaoxinmsg(order.speciid, order.U_traveldate, out dxenddate, out dxbuynum);

                            //保存订单信息
                            Api_hzins_OrderApplyReq_Application mhzins1 = new Api_hzins_OrderApplyReq_Application
                            {
                                id = 0,
                                orderid = orderid,
                                applicationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                startDate = order.U_traveldate.ToString("yyyy-MM-dd"),
                                endDate = dxenddate,
                                singlePrice = 0
                            };
                            int mhzins1id = new Api_hzins_OrderApplyReq_ApplicationData().EditOrderApplyReq_Application(mhzins1);
                            //投保人信息
                            Api_hzins_OrderApplyReq_applicantInfo mhzins2 = new Api_hzins_OrderApplyReq_applicantInfo
                            {
                                id = 0,
                                cName = order.baoxiannames.Split(',')[0],
                                eName = order.baoxianpinyinnames == "" ? "" : order.baoxianpinyinnames.Split(',')[0],
                                cardType = (int)Hzins_cardType.Shenfen,
                                cardCode = order.baoxianidcards.Split(',')[0],
                                sex = IdcardHelper.GetSexNoFromIdCard(order.baoxianidcards.Split(',')[0]),
                                birthday = IdcardHelper.GetBrithdayFromIdCard(order.baoxianidcards.Split(',')[0]),
                                mobile = order.U_phone,
                                email = "service@etown.cn",
                                jobInfo = "",
                                orderid = orderid
                            };
                            int mhzins2id = new Api_hzins_OrderApplyReq_applicantInfoData().EditOrderApplyReq_applicantInfo(mhzins2);

                            //被保险人信息
                            string[] dxInsurantInfostr = order.baoxiannames.Split(',');
                            for (int i = 0; i < dxInsurantInfostr.Length; i++)
                            {
                                if (dxInsurantInfostr[i] != "")
                                {
                                    int relationId = (int)Hzins_relationId.Qita;
                                    if (i == 0)
                                    {
                                        relationId = (int)Hzins_relationId.Benren;
                                    }
                                    Api_hzins_OrderApplyReq_insurantInfo mhzins3 = new Api_hzins_OrderApplyReq_insurantInfo
                                    {
                                        id = 0,
                                        insurantId = i.ToString(),
                                        cName = order.baoxiannames.Split(',')[i],
                                        eName = order.baoxianpinyinnames == "" ? "" : order.baoxianpinyinnames.Split(',')[i],
                                        sex = IdcardHelper.GetSexNoFromIdCard(order.baoxianidcards.Split(',')[i]),
                                        cardType = (int)Hzins_cardType.Shenfen,
                                        cardCode = order.baoxianidcards.Split(',')[i],
                                        birthday = IdcardHelper.GetBrithdayFromIdCard(order.baoxianidcards.Split(',')[i]),
                                        relationId = relationId,
                                        count = dxbuynum,
                                        singlePrice = B2b_com_pro_SpeciData.Getgginfobyggid(order.Speciid) == null ? 0 : B2b_com_pro_SpeciData.Getgginfobyggid(order.Speciid).speci_agentsettle_price,
                                        fltNo = "",
                                        fltDate = "",
                                        city = "",
                                        tripPurposeId = (int)Hzins_tripPurposeId.Lvyou,
                                        destination = "",
                                        visaCity = "",
                                        jobInfo = "",
                                        mobile = "",
                                        orderid = orderid
                                    };
                                    int mhzins3id = new Api_hzins_OrderApplyReq_insurantInfoData().EditOrderApplyReq_insurantInfo(mhzins3);
                                }
                            }

                        }
                        #endregion
                        #region 万龙接口订单
                        if (pro_t.Source_type == 3 && pro_t.Serviceid == 4)
                        {
                            try
                            {
                                WlGetProInfoDealRequestData wldata = new WlGetProInfoDealRequestData(commanage.B2bcompanyinfo.wl_PartnerId, commanage.B2bcompanyinfo.wl_userkey);

                                WlDealResponseBody WlDealinfo = wldata.SelectonegetWlProDealData(pro_t.Service_proid, comid);
                                if (WlDealinfo == null)
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "wl绑定产品出错，创建订单失败" });
                                }
                                double toal = WlDealinfo.marketPrice * order.U_num;
                                string tavedate = order.U_traveldate.ToString();
                                var createwlorder = wldata.wlOrderCreateRequest_json(int.Parse(commanage.B2bcompanyinfo.wl_PartnerId), order.U_name, order.U_phone, orderid.ToString(), order.Pro_id.ToString(), WlDealinfo.proID, WlDealinfo.settlementPrice, WlDealinfo.marketPrice, toal, order.U_num, tavedate);//

                                var wlcreate = wldata.wlOrderCreateRequest_data(createwlorder, comid);
                                if (wlcreate.IsSuccess == true)
                                {
                                    //wl订单创建成功
                                }
                                else
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "wl接口创建订单失败1" });
                                }

                            }
                            catch
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "wl接口创建订单失败2" });
                            }
                        }
                        #endregion

                        #region 把美景联动订单信息录入订单子表
                        if (pro_t.Source_type == 3 && pro_t.Serviceid == 3)
                        {
                            ApiService mapiservice = new ApiServiceData().GetApiservice(pro_t.Serviceid);
                            if (mapiservice != null)
                            {
                                Api_Mjld_SubmitOrder_input minput = new Api_Mjld_SubmitOrder_input
                                {
                                    id = 0,
                                    timeStamp = CommonFunc.ConvertDateTimeInt(DateTime.Now).ToString(),
                                    user = mapiservice.Servicername,
                                    password = mapiservice.Password,
                                    goodsId = pro_t.Service_proid.ToString(),
                                    num = order.U_num.ToString(),
                                    phone = order.U_phone,
                                    batch = "0",//<!-值填1时一码一票，值填0或不填该字段是一码多票>
                                    guest_name = order.U_name,
                                    identityno = "",
                                    order_note = "",
                                    forecasttime = pro_t.isneedbespeak == 1 ? order.U_traveldate.ToString("yyyy-MM-dd HH:mm:ss") : "",//预定时间【产品详情里IsReserve=True时，需传递该时间；IsReserve=False时，必须保留该值为空】
                                    consignee = "",
                                    address = "",
                                    zipcode = "",
                                    orderId = order.Id
                                };
                                int mmjldid = new Api_mjld_SubmitOrder_inputData().EditApi_mjld_SubmitOrder_input(minput);
                            }
                            else
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "产品服务商信息查询失败" });
                            }
                        }
                        #endregion
                        #region 把阳光订单信息录入订单子表
                        if (pro_t.Source_type == 3 && pro_t.Serviceid == 1)
                        {
                            ApiService mapiservice = new ApiServiceData().GetApiservice(pro_t.Serviceid);
                            if (mapiservice != null)
                            {
                                Api_yg_addorder_input minput = new Api_yg_addorder_input
                                {
                                    id = 0,
                                    organization = mapiservice.Organization,
                                    password = mapiservice.Password,
                                    req_seq = mapiservice.Organization + DateTime.Now.ToString("yyyyMMddhhssmm") + CommonFunc.CreateNum(6),//请求流水号,
                                    product_num = pro_t.Service_proid.ToString(),
                                    num = order.U_num,
                                    mobile = order.U_phone,
                                    use_date = "",
                                    real_name_type = 0,
                                    real_name = "",
                                    id_card = "",
                                    card_type = 0,
                                    orderId = order.Id
                                };
                                int mmjldid = new Api_yg_addorder_inputData().EditApi_yg_addorder_input(minput);
                            }
                            else
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "产品服务商信息查询失败" });

                            }
                        }
                        #endregion


                        if (pro_t.Server_type == 9)//酒店客房插入
                        {
                            if (order.M_b2b_order_hotel != null)
                            {
                                B2b_order_hotel m_orderhotel = order.M_b2b_order_hotel;
                                m_orderhotel.Id = 0;//新订单肯定需要增加把id换成1
                                m_orderhotel.Orderid = orderid;
                                m_orderhotel.Bookdaynum = (m_orderhotel.End_date - m_orderhotel.Start_date).Days;//入住天数
                                //向附属表(酒店客房订单表)录入数据
                                int m_orderhotelid = new B2b_order_hotelData().InsertOrUpdate(m_orderhotel);
                                order.M_b2b_order_hotel.Id = m_orderhotelid;

                                B2b_com_housetype m_b2b_com_housetype = new B2b_com_housetypeData().GetHouseType(pro_t.Id, comid);
                                if (m_b2b_com_housetype != null)
                                {
                                    //获取酒店所在项目的名称
                                    string projectname = new B2b_com_projectData().GetProjectNameByid(pro_t.Projectid);

                                    if (m_b2b_com_housetype.ReserveType == 1)//不用支付直接发送订房短信
                                    {
                                        Smsmodel smodel = new Smsmodel()//微信酒店预订服务商通知短信
                                        {
                                            RecerceSMSPhone = m_b2b_com_housetype.RecerceSMSPhone,
                                            Phone = order.U_phone,
                                            Name = order.U_name,
                                            Title = projectname + pro_t.Pro_name,
                                            //Money = order.Pay_price*order.U_num,
                                            Key = "微信酒店预订服务商通知短信",
                                            Comid = comid,
                                            Num = order.U_num,//间数
                                            Num1 = m_orderhotel.Bookdaynum,//天数
                                            Starttime = m_orderhotel.Start_date,
                                            Endtime = m_orderhotel.End_date,

                                        };
                                        Smsmodel kmodel = new Smsmodel()//客户发送内容
                                        {
                                            Phone = order.U_phone,
                                            Name = order.U_name,
                                            Title = projectname + pro_t.Pro_name,
                                            //Money = order.Pay_price*order.U_num,
                                            Key = "预订酒店短信",
                                            Comid = comid,
                                            Num = order.U_num,//间数
                                            Num1 = m_orderhotel.Bookdaynum,//天数
                                            Starttime = m_orderhotel.Start_date,
                                            Endtime = m_orderhotel.End_date,
                                        };


                                        if (hotel_yes_dinggou_yuding == 0)
                                        {

                                            //向客户发送预定短信
                                            SendSmsHelper.Member_smsBal(kmodel);
                                            //向酒店负责人发送客人的预定信息
                                            SendSmsHelper.Member_smsBal(smodel);
                                        }
                                        else
                                        {//房不足需要绑定人确认完成

                                            //向酒店负责人发送客人的预定信息
                                            SendSmsHelper.Member_smsBal(smodel);

                                        }
                                    }
                                }
                                else
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "获取酒店客房信息失败" });
                                }
                            }
                        }
                    }


                    //如果订单 是充值订单并且有待处理的订单在订单中做标记
                    if (order.Handlid > 0)
                    {
                        var hand = orderdate.UpHandlid(orderid, order.Handlid);

                    }


                    if (pro_t != null)
                    {
                        #region 旅游大巴 空位减少
                        if (pro_t.Server_type == 10)
                        {
                            //需要把出团当天的空位减少

                            int reduce_emptynum = new B2b_com_LineGroupDateData().ReduceEmptyNum(order.Pro_id, order.U_num, order.U_traveldate);
                        }
                        #endregion
                        #region 旅游产品 空位减少
                        if (pro_t.Server_type == 2 || pro_t.Server_type == 8)
                        {
                            //需要把出团当天的空位减少
                            int reduce_emptynum = new B2b_com_LineGroupDateData().ReduceEmptyNum(order.Pro_id, order.U_num, order.U_traveldate);
                        }
                        #endregion

                        #region 对分销订单，教练订单也发送 绑定确认短信
                        if (pro_t.Server_type == 13)
                        {

                            //短信通知
                            var querenduanxin = pro_t.bookpro_bindname + " 您绑定预约产品：" + pro_t.Pro_name + " 客户:" + order.U_name + " (" + order.U_phone + ") 提交一笔订单，预约时间:" + order.U_traveldate.ToString("yyyy-MM-dd hh:mm") + " 的预订,订单号:" + order.Id + ",请立即回复短信 确认订单请回复 qr" + order.Id + " ，取消订单请回复 qx" + order.Id + " 您确认后客服才能进行支付。";
                            var msg = "";
                            var sendback = SendSmsHelper.SendSms(pro_t.bookpro_bindphone, querenduanxin, order.Comid, out msg);

                        }
                        #endregion


                        #region 其他服务类型，可销售数量/空位数量 减少
                        else
                        {
                            if (pro_t.Ispanicbuy == 1 || pro_t.Ispanicbuy == 2)
                            {
                                if (pro_t.Server_type == 2 || pro_t.Server_type == 8)//跟团游；当地游
                                {
                                    //需要把出团当天的空位减少
                                    int reduce_emptynum = new B2b_com_LineGroupDateData().ReduceEmptyNum(order.Pro_id, order.U_num, order.U_traveldate);
                                }

                                if (pro_t.Server_type == 1 || pro_t.Server_type == 11)//服务类型：票务;实物产品
                                {

                                    //提单时 原始产品可销售数量减少，已销售数量增加，导入产品无需变动；
                                    if (pro_t.Bindingid == 0)
                                    {
                                        //把票务的可销售数量减少；同时已销售数量增加
                                        int reduce_ticket = new B2bComProData().ReduceLimittotalnum(order.Pro_id, order.U_num);

                                    }
                                }
                            }
                            if (pro_t.Server_type == 9)//酒店 
                            {

                                //需要把入住时间内所有天的空位减少
                                int reduce_emptynum = new B2b_com_LineGroupDateData().ReduceEmptyNum(order.Pro_id, order.U_num, order.M_b2b_order_hotel.Start_date, order.M_b2b_order_hotel.End_date);

                            }

                            //对规格库存进行扣减
                            if (order.Speciid != 0)
                            {
                                int reduce_ticket = new B2bComProData().ReduceLimittotalSpeciidnum(order.Pro_id, order.Speciid, order.U_num);
                            }
                        }
                        #endregion
                    }

                    //根据订单id得到订单信息
                    B2bOrderData dataorder = new B2bOrderData();
                    B2b_order modelb2border = dataorder.GetOrderById(orderid);

                    if (morenfasong == 1)
                    {
                        //计算分销余额
                        decimal overmoney = agentinfo.Imprest - modelb2border.Pay_price * modelb2border.U_num - modelb2border.Express + modelb2border.childreduce * modelb2border.Child_u_num;


                        if (modelb2border == null)
                        {
                            dikou = "没有查询到此笔订单";
                            return JsonConvert.SerializeObject(new { type = 1, msg = dikou });
                        }
                        if (order.Order_type == 1)//1为订单处理
                        {
                            //扣减子分销授权金额
                            if (order.Agentsunid != 0)
                            {
                                decimal jine = modelb2border.Pay_price * modelb2border.U_num + modelb2border.Express - modelb2border.Child_u_num * modelb2border.childreduce;//扣减金额
                                var agentsunfin = AgentCompanyData.WriteAgentSunMoney(jine, order.Agentsunid);
                            }

                            if (order.Warrant_type == 1)//1.出票扣款 2.验码扣款
                            {
                                #region 分销商预付款+信用额度 不足1000、500、0 时，给分销商短信通知
                                decimal newestBalance = agentinfo.Imprest + agentinfo.Credit - order.Pay_price * order.U_num;
                                if (newestBalance > agentinfo.maxremindmoney)
                                { }
                                else if (newestBalance > 500 && newestBalance <= agentinfo.maxremindmoney)
                                {
                                    //分销额度不足 提醒通知短信
                                    Agent_RechargeRemindSms remindsms = new Agent_RechargeRemindSmsData().GetAgent_RechargeRemindSms(order.Agentid, order.Comid, agentinfo.maxremindmoney, 0);
                                    //如果额度不足通知短信已经发送过，并且没有充值处理，则不再发送；如果已经充值处理了，提单后额度仍然不足，则继续发送短信
                                    if (remindsms == null)
                                    {
                                        //额度不足提醒短信当天已经发送过则不再发送
                                        int remindnum = new Agent_RechargeRemindSmsData().GetAgent_NowdayRechargeRemindSmsNum(order.Agentid, order.Comid, agentinfo.maxremindmoney);
                                        if (remindnum == 0)
                                        {
                                            string companyname = new B2bCompanyData().GetCompanyNameById(order.Comid);
                                            string remindsmsstr = "额度提醒:你在商户 " + companyname + " 下的信用额度/预付款已经不足" + agentinfo.maxremindmoney + "元，请及时充值；";
                                            string msg = "";
                                            int sendback = SendSmsHelper.SendSms(agentinfo.Mobile, remindsmsstr, order.Comid, out msg);

                                            Agent_RechargeRemindSms mremindsms = new Agent_RechargeRemindSms
                                            {
                                                id = 0,
                                                agentid = order.Agentid,
                                                comid = order.Comid,
                                                smscontent = remindsmsstr,
                                                tishi_edu = agentinfo.maxremindmoney.ToString(),
                                                tishi_phone = agentinfo.Mobile,
                                                tishi_time = DateTime.Now,
                                                isdeal = 0
                                            };
                                            int insremindsms = new Agent_RechargeRemindSmsData().InsRemindsms(mremindsms);
                                        }
                                    }
                                }
                                else if (newestBalance > 0 && newestBalance <= 500)
                                {
                                    //分销额度不足 提醒通知短信
                                    Agent_RechargeRemindSms remindsms = new Agent_RechargeRemindSmsData().GetAgent_RechargeRemindSms(order.Agentid, order.Comid, 500, 0);
                                    //如果额度不足通知短信已经发送过，并且没有充值处理，则不再发送；如果已经充值处理了，提单后额度仍然不足，则继续发送短信
                                    if (remindsms == null)
                                    {
                                        //额度不足提醒短信当天已经发送过则不再发送
                                        int remindnum = new Agent_RechargeRemindSmsData().GetAgent_NowdayRechargeRemindSmsNum(order.Agentid, order.Comid, 500);
                                        if (remindnum == 0)
                                        {
                                            string companyname = new B2bCompanyData().GetCompanyNameById(order.Comid);
                                            string remindsmsstr = "额度提醒:你在商户 " + companyname + " 下的信用额度/预付款已经不足500元，请及时充值；";
                                            string msg = "";
                                            int sendback = SendSmsHelper.SendSms(agentinfo.Mobile, remindsmsstr, order.Comid, out msg);

                                            Agent_RechargeRemindSms mremindsms = new Agent_RechargeRemindSms
                                            {
                                                id = 0,
                                                agentid = order.Agentid,
                                                comid = order.Comid,
                                                smscontent = remindsmsstr,
                                                tishi_edu = "500",
                                                tishi_phone = agentinfo.Mobile,
                                                tishi_time = DateTime.Now,
                                                isdeal = 0
                                            };
                                            int insremindsms = new Agent_RechargeRemindSmsData().InsRemindsms(mremindsms);
                                        }
                                    }
                                }
                                else //newestBalance < 0
                                {
                                    //分销额度不足 提醒通知短信
                                    Agent_RechargeRemindSms remindsms = new Agent_RechargeRemindSmsData().GetAgent_RechargeRemindSms(order.Agentid, order.Comid, 0, 0);
                                    //如果额度不足通知短信已经发送过，并且没有充值处理，则不再发送；如果已经充值处理了，提单后额度仍然不足，则继续发送短信
                                    if (remindsms == null)
                                    {
                                        //额度不足提醒短信当天已经发送过则不再发送
                                        int remindnum = new Agent_RechargeRemindSmsData().GetAgent_NowdayRechargeRemindSmsNum(order.Agentid, order.Comid, 0);
                                        if (remindnum == 0)
                                        {
                                            string companyname = new B2bCompanyData().GetCompanyNameById(order.Comid);
                                            string remindsmsstr = "额度提醒:你在商户 " + companyname + " 下的信用额度/预付款已经不足0元，请及时充值；";
                                            string msg = "";
                                            int sendback = SendSmsHelper.SendSms(agentinfo.Mobile, remindsmsstr, order.Comid, out msg);

                                            Agent_RechargeRemindSms mremindsms = new Agent_RechargeRemindSms
                                            {
                                                id = 0,
                                                agentid = order.Agentid,
                                                comid = order.Comid,
                                                smscontent = remindsmsstr,
                                                tishi_edu = "0",
                                                tishi_phone = agentinfo.Mobile,
                                                tishi_time = DateTime.Now,
                                                isdeal = 0
                                            };
                                            int insremindsms = new Agent_RechargeRemindSmsData().InsRemindsms(mremindsms);
                                        }
                                    }
                                }
                                #endregion

                                //每笔订单金额必须不能超出预付款金额
                                if ((agentinfo.Imprest + agentinfo.Credit) < (order.Pay_price * order.U_num + order.Express - order.childreduce * order.Child_u_num) && order.Warrant_type == 1 && order.Order_type == 1)
                                {
                                    decimal pay_money = 0;
                                    //因上面判断是 预付款+信用额 不足，可实际支付时不考虑信用额。已经欠款的分销支付 不增加欠款为原则。
                                    //判断预付款金额，如果无预付款（或负预付款）则支付订单金额，如果有预付款则扣减预付款的金额
                                    if (agentinfo.Imprest > 0)
                                    {
                                        pay_money = (order.Pay_price * order.U_num + order.Express - order.childreduce * order.Child_u_num) - agentinfo.Imprest;
                                    }
                                    else
                                    {
                                        pay_money = (order.Pay_price * order.U_num + order.Express - order.childreduce * order.Child_u_num);
                                    }


                                    //money 为需要支付的金额。不考虑信用额最高为订单金额 预付款不足=yfkbz
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "yfkbz", money = pay_money, id = orderid });
                                }


                                #region 当提交订单时，不管来路，如果订单处理成功进入发码流程前,扣款前,提交一笔原始分销订单。
                                if (pro_t.Source_type == 4)
                                {
                                    int pro_id_old = pro_t.Bindingid;//原产品ID
                                    int comid_old = order.Comid;//提交订单商户ID
                                    int agentid_old = 0;//提交订单商户绑定的分销id
                                    B2bCompanyData comdata = new B2bCompanyData();
                                    var cominfo = comdata.GetCompanyBasicById(comid_old);
                                    if (cominfo != null)
                                    {
                                        agentid_old = cominfo.Bindingagent;
                                    }

                                    var daoruguigeSpeciid = order.Speciid;
                                    //重新读取绑定的规格
                                    if (order.Speciid != 0)
                                    {
                                        var guiginfo = B2b_com_pro_SpeciData.Getgginfobyggid(order.Speciid);
                                        if (guiginfo != null)
                                        {
                                            daoruguigeSpeciid = guiginfo.binding_id;
                                        }
                                    }


                                    //-----注意:如果旅游产品做成可以导入的话，需要传递参数 儿童数量，暂时没加----
                                    AgentOrder(agentid_old, pro_id_old.ToString(), "1", order.U_num.ToString(), order.U_name, order.U_phone, order.U_traveldate.ToString(), "", 1, out bindingorderid_huoqu, 0, 1, order.pickuppoint, order.dropoffpoint, "", 0, "", "", "", "", 0, 0, 0, modelb2border.yanzheng_method, daoruguigeSpeciid, order.baoxiannames, order.baoxianpinyinnames, order.baoxianidcards, 0, order.M_b2b_order_hotel, order.Id, order.travelnames, order.travelidcards, order.travelnations, order.travelphones, order.travelremarks, modelb2border.U_idcard);
                                    //bindingorderid 是分销返回的订单号，必须成功否则报错
                                    //导入产品，如果提交订单错误，则可能是分销产品下线，分销钱不够，或者其他问题没有提交订单
                                    if (bindingorderid_huoqu == 0)
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = "提交订单错误，请联系商家！" });
                                    }
                                    B2b_order agentsunorder = dataorder.GetOrderById(bindingorderid_huoqu);
                                    if (agentsunorder == null)
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = "提交订单错误，请联系商家！" });
                                    }
                                    if (agentsunorder.Pay_state == 1)
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = "提交订单错误，商家额度不足，请联系商家！" });
                                    }


                                    modelb2border.Bindingagentorderid = bindingorderid_huoqu;
                                    dataorder.InsertOrUpdate(modelb2border);
                                }
                                #endregion


                                //分销商财务扣款
                                Agent_Financial Financialinfo = new Agent_Financial
                                {
                                    Id = 0,
                                    Com_id = modelb2border.Comid,
                                    Agentid = modelb2border.Agentid,
                                    Warrantid = modelb2border.Warrantid,
                                    Order_id = modelb2border.Id,
                                    Servicesname = pro_t.Pro_name + "[" + orderid + "]",
                                    SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                                    Money = 0 - modelb2border.Pay_price * modelb2border.U_num - modelb2border.Express + modelb2border.Child_u_num * modelb2border.childreduce,
                                    Payment = 1,            //收支(0=收款,1=支出)
                                    Payment_type = "分销扣款",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点,分销扣款）
                                    Money_come = "分销账户",         //资金来源（网上支付,银行收款，分销账户等）
                                    Over_money = overmoney
                                };
                                var fin = AgentCompanyData.AgentFinancial(Financialinfo);

                                modelb2border.Pay_state = 2;
                                modelb2border.Order_state = 2;
                                //修改此订单的支付状态为“支付成功” ,订单状态为“已付款”
                                dataorder.InsertOrUpdate(modelb2border);

                                //扣除商户分销订单手续费
                                var KouchuShouxufei_temp = OrderJsonData.KouchuShouxufei(modelb2border);

                                //订单发码
                                if (pro_t != null)
                                {
                                    //dikou = new SendEticketData().SendEticket(orderid, 1, "", order.aorderid);
                                    if (pro_t.Server_type != 9)//酒店客房此时不发送短信，确认后再给客户发送
                                    {
                                        dikou = new SendEticketData().SendEticket(orderid, 1, "", order.aorderid);
                                    }
                                }


                                #region 酒店客房产品 向绑定顾问发送预订短信
                                if (pro_t.Server_type == 9)//酒店客房产品
                                {

                                    if (pro_t.Source_type != 4)
                                    {//如果是导入产品部发送短信，由原始短信发送

                                        if (order.M_b2b_order_hotel != null)
                                        {

                                            string projectname = new B2b_com_projectData().GetProjectNameByid(pro_t.Projectid);
                                            //短信通知
                                            var querenduanxin = pro_t.bookpro_bindname + "你好 客户预订：" + projectname + pro_t.Pro_name + " 姓名:" + order.U_name + " (" + order.U_phone + ") ，" + order.U_num + "间，入住时间:" + order.M_b2b_order_hotel.Start_date.ToString("yyyy-MM-dd") + " 离店时间： " + order.M_b2b_order_hotel.End_date.ToString("yyyy-MM-dd") + " ,订单号:" + orderid + ",请立即回复短信确认：(1)留房请回复 qr" + orderid + " ，(2)满房取消请回复 qx" + orderid + "。";

                                            if (hotel_yes_dinggou_yuding == 1)
                                            {
                                                querenduanxin = pro_t.bookpro_bindname + "你好 客户预订：" + projectname + pro_t.Pro_name + " 姓名:" + order.U_name + " (" + order.U_phone + ") ，" + order.U_num + "间，入住时间:" + order.M_b2b_order_hotel.Start_date.ToString("yyyy-MM-dd") + " 离店时间： " + order.M_b2b_order_hotel.End_date.ToString("yyyy-MM-dd") + " ,订单号:" + orderid + ",请立即回复短信确认：(1)留房请回复 qr" + orderid + " ，(2)满房取消请回复 qx" + orderid + " ,(3)房价调整请回复 tj" + orderid + " ";
                                            }

                                            var msg = "";
                                            var sendback = SendSmsHelper.SendSms(pro_t.bookpro_bindphone, querenduanxin, order.Comid, out msg);

                                            string Returnmd5_temp = EncryptionHelper.ToMD5(orderid.ToString() + "lixh1210", "UTF-8");
                                            var querenduanxin_weixin = "有客户预订房间：\n\n " + projectname + pro_t.Pro_name + " \n\n客户:" + order.U_name + " (" + order.U_phone + ") ，" + order.U_num + "间,入住时间:" + order.M_b2b_order_hotel.Start_date.ToString("yyyy-MM-dd hh:mm") + " 离店时间：" + order.M_b2b_order_hotel.End_date.ToString("yyyy-MM-dd") + " \n\n点击下面链接进行确认。\n http://shop" + pro_t.Com_id + ".etown.cn/h5/Confirmyuyue.aspx?id=" + orderid + "&md5=" + Returnmd5_temp;

                                            //对绑定顾问发送微信客服通道通知
                                            CustomerMsg_Send.SendWxkefumsg(orderid, 1, querenduanxin_weixin, comid);//给绑定顾问发送


                                            //给默认客服发送通知
                                            SendEticketData.SendWeixinKfMsg(orderid);
                                        }
                                        else
                                        {
                                            return JsonConvert.SerializeObject(new { type = 1, msg = "酒店订单提交失败" });
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region 当提交订单时，不管来路，如果订单处理成功进入发码流程前,扣款前,提交一笔原始分销订单。
                                if (pro_t.Source_type == 4)
                                {
                                    int pro_id_old = pro_t.Bindingid;//原产品ID
                                    int comid_old = order.Comid;//提交订单商户ID
                                    int agentid_old = 0;//提交订单商户绑定的分销id
                                    B2bCompanyData comdata = new B2bCompanyData();
                                    var cominfo = comdata.GetCompanyBasicById(comid_old);
                                    if (cominfo != null)
                                    {
                                        agentid_old = cominfo.Bindingagent;
                                    }
                                    var daoruguigeSpeciid = order.Speciid;
                                    //重新读取绑定的规格
                                    if (order.Speciid != 0)
                                    {
                                        var guiginfo = B2b_com_pro_SpeciData.Getgginfobyggid(order.Speciid);
                                        if (guiginfo != null)
                                        {
                                            daoruguigeSpeciid = guiginfo.binding_id;
                                        }
                                    }

                                    //-----注意:如果旅游产品做成可以导入的话，需要传递参数 儿童数量，暂时没加----
                                    AgentOrder(agentid_old, pro_id_old.ToString(), "1", order.U_num.ToString(), order.U_name, order.U_phone, order.U_traveldate.ToString(), "", 1, out bindingorderid_huoqu, 0, 1, order.pickuppoint, order.dropoffpoint, "", 0, "", "", "", "", 0, 0, 0, modelb2border.yanzheng_method, daoruguigeSpeciid, order.baoxiannames, order.baoxianpinyinnames, order.baoxianidcards, 0, null, order.Id, order.travelnames, order.travelidcards, order.travelnations, order.travelphones, order.travelremarks, modelb2border.U_idcard);
                                    //bindingorderid 是分销返回的订单号，必须成功否则报错
                                    //导入产品，如果提交订单错误，则可能是分销产品下线，分销钱不够，或者其他问题没有提交订单
                                    if (bindingorderid_huoqu == 0)
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = "提交订单错误，请联系商家！" });
                                    }
                                    B2b_order agentsunorder = dataorder.GetOrderById(bindingorderid_huoqu);
                                    if (agentsunorder == null)
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = "提交订单错误，请联系商家！" });
                                    }
                                    if (agentsunorder.Pay_state == 1)
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = "提交订单错误，商家额度不足，请联系商家！" });
                                    }
                                    modelb2border.Bindingagentorderid = bindingorderid_huoqu;
                                    dataorder.InsertOrUpdate(modelb2border);
                                }
                                #endregion

                                //---------------新增1begin--------------//

                                //如果是对外接口的请求，则处理订单和发码
                                //if (isInterfaceSub == 1)//此处注释掉了，所有订单都发码，没有商户确认这一步了
                                //{
                                    modelb2border.Pay_state = 2; //对于验码时扣款，此笔订单应该如何支付状态应该如何处理。
                                    modelb2border.Order_state = 2;
                                    //修改此订单的支付状态为“支付成功” ,订单状态为“已付款”
                                    dataorder.InsertOrUpdate(modelb2border);

                                    dikou = new SendEticketData().SendEticket(orderid, 1, "", order.aorderid);
                                //}
                                //else
                                //{
                                //    dikou = "OK";
                                //}

                            }
                        }
                        else
                        {//成功提交充值订单
                            dikou = "OK";
                        }
                    }
                    else
                    {//对分销提交非自动发码的订单，只提交订单，不做财务处理和发码
                        return "OK";
                    }
                }
                #endregion

                //抵扣end
                //helper.Commit();

                SendEticketData sendate = new SendEticketData();
                string pno = "";

                if (pro_t != null)
                {
                    if (pro_t.Source_type == 3)//如果是借口产品，按接口方式读码selservice
                    {
                        if (pro_t.Serviceid == 4)
                        { //如果是接口产品
                            WlGetProInfoDealRequestData wldata = new WlGetProInfoDealRequestData(commanage.B2bcompanyinfo.wl_PartnerId, commanage.B2bcompanyinfo.wl_userkey);
                            var wlorderinfo = wldata.SearchWlOrderData(comid, 0, "", orderid);
                            if (wlorderinfo != null)
                            {
                                pno = wlorderinfo.vouchers;
                            }
                        }
                    }
                    else
                    {//如果不是借口，则按自己规则读码

                        pno = sendate.HuoQuEticketPno(orderid);
                    }
                }





                string Returnmd5 = EncryptionHelper.ToMD5(orderid.ToString() + "lixh1210", "UTF-8");//订单加密验证

                if (dikou == "OK" || dikou == "")
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = orderid, dikou = dikou, pno = pno, md5 = Returnmd5 });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = orderid, dikou = dikou, pno = pno, md5 = Returnmd5 });
                }
            }
            catch (Exception ex)
            {
                //helper.Rollback();
                return JsonConvert.SerializeObject(new { type = 1, msg = "提交订单失败!" });
            }
            //}
        }

        private static bool TqBaoxinmsg(int speciid, DateTime dxbegindate, out string dxenddate, out int dxbuynum)
        {
            dxenddate = "";
            dxbuynum = 0;

            string dxspeciname = new B2b_com_pro_SpeciData().Getspecinamebyid(speciid);
            if (dxspeciname != "")
            {
                string[] dxstr = dxspeciname.Split(';');
                foreach (string dxr in dxstr)
                {
                    if (dxr != "")
                    {
                        if (dxr.IndexOf("保障期限") > -1)
                        {
                            int dxqixian = CommonFunc.GetNumber(dxr);
                            if (dxqixian == 0)
                            {
                                return false;
                            }
                            //得到期限单位:天；月；年
                            if (dxr.IndexOf("天") > -1)
                            {
                                dxenddate = dxbegindate.AddDays(dxqixian).ToString("yyyy-MM-dd");
                            }
                            if (dxr.IndexOf("月") > -1)
                            {
                                dxenddate = dxbegindate.AddMonths(dxqixian).ToString("yyyy-MM-dd");
                            }
                            if (dxr.IndexOf("年") > -1)
                            {
                                dxenddate = dxbegindate.AddYears(dxqixian).ToString("yyyy-MM-dd");
                            }
                        }
                        if (dxr.IndexOf("购买份数") > -1)
                        {
                            dxbuynum = CommonFunc.GetNumber(dxr);
                            if (dxbuynum == 0)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///  插入预付款订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string InsertRecharge(B2b_order order)
        {
            using (var helper = new SqlHelper())
            {

                try
                {
                    B2bOrderData orderdate = new B2bOrderData();

                    int orderid = orderdate.InsertOrUpdate(order);
                    return JsonConvert.SerializeObject(new { type = 100, msg = orderid });
                }
                catch (Exception ex)
                {
                    helper.Rollback();
                    return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                }
            }
        }

        /// <summary>
        ///  退票 修改
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string Upticket(B2b_order order)
        {
            using (var helper = new SqlHelper())
            {
                int finacebackid = 0;
                decimal tuishouxufei = 0;
                int eticket = 0;
                try
                {
                    B2bOrderData orderdate = new B2bOrderData();

                    int orderid = orderdate.InsertOrUpdate(order);

                    B2bFinanceData Financed = new B2bFinanceData();




                    B2b_com_pro pro = new B2b_com_pro();
                    var paydate = new B2bPayData();
                    var prodata = new B2bComProData();
                    if (orderid != 0)
                    {

                        pro = new B2bComProData().GetProById(order.Pro_id.ToString());
                        var company = B2bCompanyData.GetCompany(order.Comid);//根据用户id得到商家基本信息

                        B2b_pay m_pay = new B2bPayData().GetPayByoId(orderid);
                        if (m_pay == null)
                        {

                            //全部退款，当没有查询到支付，而退款预付款和积分--未做



                            return JsonConvert.SerializeObject(new { type = 1, msg = "支付信息不存在" });
                        }
                        else
                        {
                            if (m_pay.Trade_status != "TRADE_SUCCESS")
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "支付失败" });
                            }
                        }

                        //查询手续费，退票是 此手续费按比例一起退（和退款金额一起）

                        var orderid_pay = orderid;//默认此订单为发起的支付的订单
                        if (order.Shopcartid != 0)
                        {
                            orderid_pay = Financed.GetPayidbyorderid(order.Shopcartid);
                        }

                        decimal shouxufei = Financed.GetShouxufeiAmount(orderid_pay);
                        decimal shouru = Financed.GetShouruAmount(orderid_pay);

                        if (shouru != 0 && shouxufei != 0 && order.Ticket != 0)
                        {
                            tuishouxufei = shouxufei * (order.Ticket / shouru);
                        }



                        //去哪 退票不做财务处理，因为 去哪一般都是商家自己开的，钱并未支付到系统里，所以也不需要做退款，只要把票作废即可。
                        if (order.qunar_orderid == "")//非去哪订单 执行退款
                        {
                            if (m_pay.comid == 106)
                            {
                                B2b_Finance Financebackinfo = new B2b_Finance()
                                {
                                    Id = 0,
                                    Com_id = order.Comid,
                                    Agent_id = 0,           //分销编号（默认为0）
                                    Eid = 0,                //易城账户（默认为0，0=没有交易，1=和易城交易）
                                    Order_id = order.Id,           //订单号（默认为0）
                                    Servicesname = pro.Pro_name + "[" + order.Id + "]",       //交易名称/内容
                                    SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                                    Money = decimal.Round(0 - (order.Ticket), 2),              //金额
                                    Payment = 1,            //收支(0=收款,1=支出)
                                    Payment_type = "直销退票",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点）
                                    Money_come = paydate.GetPayByoId(order.Id) == null ? "" : paydate.GetPayByoId(order.Id).Pay_com,         //资金来源（网上支付,银行收款等）
                                    Over_money = decimal.Round(company.Imprest - (order.Ticket), 2)       //余额（根据商家，分销，易城，编号显示相应余额）
                                };
                                finacebackid = Financed.InsertOrUpdate(Financebackinfo);


                                //退手续费，单独两笔这样清楚
                                if (tuishouxufei != 0)
                                {
                                    B2b_Finance Financebackinfo_shouxufei = new B2b_Finance()
                                    {
                                        Id = 0,
                                        Com_id = order.Comid,
                                        Agent_id = 0,           //分销编号（默认为0）
                                        Eid = 0,                //易城账户（默认为0，0=没有交易，1=和易城交易）
                                        Order_id = order.Id,           //订单号（默认为0）
                                        Servicesname = pro.Pro_name + "[" + order.Id + "]",       //交易名称/内容
                                        SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                                        Money = decimal.Round(0 - tuishouxufei, 2),              //金额
                                        Payment = 1,            //收支(0=收款,1=支出)
                                        Payment_type = "直销退票-退手续费",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点）
                                        Money_come = paydate.GetPayByoId(order.Id) == null ? "" : paydate.GetPayByoId(order.Id).Pay_com,         //资金来源（网上支付,银行收款等）
                                        Over_money = decimal.Round(company.Imprest - tuishouxufei, 2)       //余额（根据商家，分销，易城，编号显示相应余额）
                                    };
                                    var finacebackid_shouxufei = Financed.InsertOrUpdate(Financebackinfo_shouxufei);
                                }
                            }


                            //退预付款，退积分--未做


                        }

                        //商户申请的退票，系统出票接口出票已经作废，倒码只要商户提交认为已作废。所以这只对客户退款
                        //var tui = prodata.GetProById(order.Pro_id.ToString());
                        //if (tui != null)
                        //{
                        //    if (tui.Server_type == 1)
                        //    {
                        //        eticket = new B2bEticketData().Backticket_use_num(order.Id);
                        //    }
                        //} 

                    }
                    return JsonConvert.SerializeObject(new { type = 100, msg = orderid, finaceback = finacebackid, eticket = eticket });
                }
                catch (Exception ex)
                {
                    helper.Rollback();
                    return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                }
            }
        }

        /// <summary>
        ///  直销退票
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string DirectSell_Refundticket(B2b_order order, int refundNum)
        {
            using (var helper = new SqlHelper())
            {
                int finacebackid = 0;
                int eticket = 0;
                try
                {
                    B2bOrderData orderdate = new B2bOrderData();

                    B2bFinanceData Financed = new B2bFinanceData();
                    B2b_com_pro pro = new B2b_com_pro();
                    var paydate = new B2bPayData();
                    var prodata = new B2bComProData();

                    //判断退票张数是否等于剩余全部张数
                    int SurplusNum = 0;
                    if (order.Bindingagentorderid > 0)
                    {
                        SurplusNum = new B2bOrderData().GetSurplusNum(order.Bindingagentorderid);
                    }
                    else
                    {
                        SurplusNum = new B2bOrderData().GetSurplusNum(order.Id);
                    }
                    if (refundNum > 0)
                    {
                        if (refundNum > SurplusNum)
                        {
                            refundNum = SurplusNum;//如果退票数量大于 实际数量 ，退票数量更改为实际数量，去哪不应该出现的问题，防止意外
                            //return JsonConvert.SerializeObject(new { type = 1, msg = "退票数量错误" });
                        }
                    }

                    pro = new B2bComProData().GetProById(order.Pro_id.ToString());
                    var company = B2bCompanyData.GetCompany(order.Comid);//根据用户id得到商家基本信息


                    //去哪 退票不做财务处理，因为 去哪一般都是商家自己开的，钱并未支付到系统里，所以也不需要做退款，只要把票作废即可。
                    //B2b_Finance Financebackinfo = new B2b_Finance()
                    //{
                    //    Id = 0,
                    //    Com_id = order.Comid,
                    //    Agent_id = 0,           //分销编号（默认为0）
                    //    Eid = 0,                //易城账户（默认为0，0=没有交易，1=和易城交易）
                    //    Order_id = order.Id,           //订单号（默认为0）
                    //    Servicesname = pro.Pro_name + "[" + order.Id + "]",       //交易名称/内容
                    //    SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                    //    Money = decimal.Round(0 - (order.Ticket), 2),              //金额
                    //    Payment = 1,            //收支(0=收款,1=支出)
                    //    Payment_type = "直销退票",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点）
                    //    Money_come = paydate.GetPayByoId(order.Id) == null ? "" : paydate.GetPayByoId(order.Id).Pay_com,         //资金来源（网上支付,银行收款等）
                    //    Over_money = decimal.Round(company.Imprest - (order.Ticket), 2)       //余额（根据商家，分销，易城，编号显示相应余额）
                    //};
                    //finacebackid = Financed.InsertOrUpdate(Financebackinfo);


                    order.Order_state = (int)OrderStatus.InvalidOrder;//作废
                    if (order.qunar_orderid != "")
                    {
                        order.Ticketinfo += "去哪直销退票(" + refundNum + "张)";
                    }
                    else
                    {
                        order.Ticketinfo += "普通直销退票(" + refundNum + "张)";
                    }
                    order.Backtickettime = DateTime.Now;
                    int orderid = orderdate.InsertOrUpdate(order);

                    //把退票数量记入数据库
                    int insquitnum = new B2bOrderData().Insquitnum(order.Id, refundNum);

                    //把电子码清除退票数量
                    int upPnoNumZero = new B2bEticketData().UpPnoNumZero(order.Id, refundNum, SurplusNum);

                    //如果是导入产品(含有绑定订单号)
                    if (order.Bindingagentorderid > 0)
                    {
                        #region 分销订单 并且 是出票扣款
                        B2b_order agentorder = orderdate.GetOrderById(order.Bindingagentorderid);

                        agentorder.Order_state = (int)OrderStatus.InvalidOrder;//作废
                        if (order.qunar_orderid != "")
                        {
                            agentorder.Ticketinfo += "去哪直销(导入产品)退票(" + refundNum + "张)";
                        }
                        else
                        {
                            agentorder.Ticketinfo += "普通直销(导入产品)退票(" + refundNum + "张)";
                        }
                        agentorder.Backtickettime = DateTime.Now;
                        int orderid2 = orderdate.InsertOrUpdate(agentorder);

                        //把退票数量记入数据库
                        int insquitnum2 = new B2bOrderData().Insquitnum(agentorder.Id, refundNum);

                        //把电子码清除退票数量
                        int upPnoNumZero2 = new B2bEticketData().UpPnoNumZero(agentorder.Id, refundNum, SurplusNum);


                        if (agentorder.Agentid != 0 && agentorder.Warrant_type == 1)
                        {

                            //读取分销商信息
                            Agent_company agentinfo = AgentCompanyData.GetAgentWarrant(agentorder.Agentid, agentorder.Comid);
                            if (agentinfo == null)
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "分销账户信息错误" });
                            }

                            //计算分销余额
                            decimal overmoney = agentinfo.Imprest + refundNum * agentorder.Pay_price + agentorder.Express;

                            //分销商财务扣款
                            Agent_Financial Financialinfo = new Agent_Financial
                            {
                                Id = 0,
                                Com_id = agentorder.Comid,
                                Agentid = agentorder.Agentid,
                                Warrantid = agentorder.Warrantid,
                                Order_id = agentorder.Id,
                                Servicesname = pro.Pro_name + "[" + agentorder.Id + "]",
                                SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                                Money = refundNum * agentorder.Pay_price + agentorder.Express,
                                Payment = 0,            //收支(0=收款,1=支出)
                                Payment_type = "退票退款",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点,分销扣款）
                                Money_come = "分销账户",         //资金来源（网上支付,银行收款，分销账户等）
                                Over_money = overmoney
                            };
                            var fin = AgentCompanyData.AgentFinancial(Financialinfo);

                        }
                        #endregion

                    }

                    return JsonConvert.SerializeObject(new { type = 100, msg = orderid, finaceback = finacebackid, eticket = eticket });

                }
                catch (Exception ex)
                {
                    helper.Rollback();
                    return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                }
            }
        }


        #region 获取订单列表哦
        public static string OrderPageList(string comid, int pageindex, int pagesize, string key, int order_state, int ordertype, int userid = 0, int crmid = 0, int servertype = 0, string begindate = "", string enddate = "", int datetype=0)
        {
            var totalcount = 0;
            try
            {

                var orderdata = new B2bOrderData();
                var financedata = new B2bFinanceData();
                var prodata = new B2bComProData();
                var paydate = new B2bPayData();
                var hoteldata = new B2b_order_hotelData();
                var userdata = new B2bCrmData();
                B2bEticketData eticketdata = new B2bEticketData();
                SendEticketData sendate = new SendEticketData();

                int orderIsAccurateToPerson = 0;
                int channelcompanyid = 0;
                if (userid > 0)
                {
                    Sys_Group group = new Sys_GroupData().GetGroupByUserId(userid);
                    if (group != null)
                    {
                        //判断订单是否要求精确到渠道人 
                        orderIsAccurateToPerson = group.OrderIsAccurateToPerson;
                        Member_Channel_company channelcom = new MemberChannelcompanyData().GetChannelCompanyByUserId(userid);
                        if (channelcom != null)
                        {
                            channelcompanyid = channelcom.Id;
                        }
                    }
                }


                var list = orderdata.OrderPageList(comid, pageindex, pagesize, key, order_state, ordertype, out totalcount, userid, crmid, orderIsAccurateToPerson, channelcompanyid, begindate, enddate, servertype, datetype);


                IEnumerable result = "";
                if (list != null)
                {
                    result = from order in list
                             select new
                             {
                                 Id = order.Id,
                                 Pro_id = order.Pro_id,
                                 Proname = order.Order_type == 2 ? order.payorder ==1 ? "快速支付":"预付款充值" : prodata.GetProById(order.Pro_id.ToString(), order.Speciid, order.channelcoachid).Pro_name.Replace("'", "’"),
                                 Order_type = order.Order_type,
                                 U_name = order.U_name,
                                 U_id = order.U_id,
                                 Channelcompanyname = new MemberChannelcompanyData().GetChannelCompanyName(order.U_id),
                                 U_phone = order.U_phone,
                                 U_num = order.U_num,
                                 U_subdate = order.U_subdate,
                                 U_traveldate = order.U_traveldate,
                                 Payid = order.Payid,
                                 Pay_price = order.Pay_price,
                                 Cost = order.Cost,
                                 Pno = order.Pno,
                                 Server_type = prodata.GetProServer_typeById(order.Pro_id.ToString()),
                                 Profit = (order.Profit) * (order.U_num),
                                 Order_state = order.Order_state,
                                 //Order_eticket_code = order.Order_state == 4 ? sendate.HuoQuEticket(order.Id) : "--",
                                 Order_eticket_code = GetOrder_eticket_code(order),
                                 Order_state_str = EnumUtils.GetName((OrderStatus)order.Order_state),
                                 Pay_state = order.Pay_state,
                                 Send_state = order.Send_state,
                                 Ticketcode = order.Ticketcode,
                                 Rebate = order.Rebate,
                                 Ordercome = order.Ordercome,
                                 Totalcount = (order.Pay_price) * (order.U_num),
                                 Face_price = order.Order_type == 2 ? 0 : prodata.GetProById(order.Pro_id.ToString()).Face_price,
                                 RecerceSMSpeople = prodata.GetProRecerceSMSpeopleById(order.Pro_id.ToString()),
                                 Pay_str = paydate.GetOrderPay(order.Id),
                                 Ticketinfo = order.Ticketinfo == null ? "" : order.Ticketinfo,
                                 Integral1 = order.Integral1,
                                 Imprest1 = order.Imprest1,
                                 Paymoney = (order.Pay_price) * (order.U_num) - order.Integral1 - order.Imprest1,
                                 Openid = order.Openid,
                                 Agentid = order.Agentid,
                                 Agent_company = AgentCompanyData.GetAgentByid(order.Agentid) == null ? "" : AgentCompanyData.GetAgentByid(order.Agentid).Company,
                                 Returnmd5 = EncryptionHelper.ToMD5(order.Id.ToString() + order.Comid.ToString() + order.Agentid.ToString() + "lixh1210", "UTF-8"),
                                 Warrant_type = order.Warrant_type,
                                 Warrantid = order.Warrantid,
                                 Comid = order.Comid,
                                 Agentname = financedata.GetAgentNamebyorderid(order.Id),
                                 Source_type = prodata.GetProSource_typeById(order.Pro_id.ToString()),
                                 Hotelinfo = hoteldata.GetHotelOrderByOrderId(order.Id),
                                 IsCanReplyWx = IsIn48h(order.Openid),
                                 Userinfo = userdata.GetB2bCrmById(order.U_id),
                                 Expresscode = order.Expresscode,
                                 Expresscom = order.Expresscom,
                                 Express = order.Express,
                                 Address = order.Address,
                                 Province = order.Province,
                                 City = order.City,
                                 Code = order.Code,
                                 Order_remark = order.Order_remark,
                                 Deliverytype = order.Deliverytype,
                                 Shopcartid = order.Shopcartid,
                                 BindingOrder = order.Bindingagentorderid == 0 ? null : orderdata.GetOrderById(order.Bindingagentorderid),
                                 //Unuse_Ticket = order.Pay_state != 2 ? 0 : order.Bindingagentorderid == 0 ? eticketdata.SelectEticketUnUsebyOrderid(order.Id) : eticketdata.SelectEticketUnUsebyOrderid(order.Bindingagentorderid),//未做完，如果是已支付，检查电子码是否还有剩余数量，如果有则体现出退票，如果为0则不体现退票
                                 Unuse_Ticket = GetUnuseticketNum(order),
                                 childreduce = order.childreduce,
                                 Child_u_num = order.Child_u_num,
                                 LinePro_BookType = new B2bComProData().GetLinePro_BookType(order.Pro_id),
                                 AgentType = new Agent_companyData().GetAgentType(order.Agentid),
                                 Nuomi_dealid = new B2bOrderData().GetNuomi_dealid(order.Id),
                                 IsHasWxRefund = new B2b_pay_wxrefundlogData().IsHasWxRefund(order.Id),
                                 WxRefundDetail = new B2b_pay_wxrefundlogData().WxRefundDetailStr(order.Id),
                                 Cancelnum = order.Cancelnum,
                                 IsHasAlipayRefund = new B2b_pay_alipayrefundlogData().IsHasAlipayRefund(order.Id),
                                 AlipayRefundDetail = new B2b_pay_alipayrefundlogData().AlipayRefundDetailStr(order.Id),
                                 ////慧择网保险产品是否可以退保(返回 是否可退，出单状态(生效状态))
                                 //Hzins_Iscancancel = Hzins_Iscancancel(order.Id, order.Pro_id),
                                 //相对于慧择网来说订单类型:0 非慧择网订单；1慧择网订单 但没有生成真实保险订单；2慧择网订单 并且生成了真实保险订单
                                 OrderType_Hzins = new B2bComProData().GetOrderType_Hzins(order.Pro_id, order.Id),
                                 //保单号
                                 InsureNum = new Api_hzins_OrderApplyResp_OrderExtData().GetinsureNumbyorderid(order.Id),
                                 //是否可以立即结算:导入产品订单；去哪儿产品订单 不可结算
                                 Iscanjiesuan = Getiscanjiesuan(order),

                                 order.Bindingagentorderid,
                                 //IsShowSms=GetIsShowSms(order),

                                 order.bookpro_bindcompany,
                                 order.bookpro_bindconfirmtime,
                                 order.bookpro_bindname,
                                 order.bookpro_bindphone,
                                 order.payorder,
                                 yiguoqi = prodata.GetProyouxiaoqiById(order.Pro_id),
                                  order.U_idcard,
                             };
                }
                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }



        #region 获取预约订单列表哦
        public static string yuyueOrderPageList(string comid, int pageindex, int pagesize, string key, int order_state, int ordertype, int userid = 0, int crmid = 0, int servertype = 0, string begindate = "", string enddate = "", int datetype = 0)
        {
            var totalcount = 0;
            try
            {

                var orderdata = new B2bOrderData();
                var financedata = new B2bFinanceData();
                var prodata = new B2bComProData();
                var paydate = new B2bPayData();
                var hoteldata = new B2b_order_hotelData();
                var userdata = new B2bCrmData();
                B2bEticketData eticketdata = new B2bEticketData();
                SendEticketData sendate = new SendEticketData();

                int orderIsAccurateToPerson = 0;
                int channelcompanyid = 0;

                var list = orderdata.OrderPageList(comid, pageindex, pagesize, key, order_state, ordertype, out totalcount, userid, crmid, orderIsAccurateToPerson, channelcompanyid, begindate, enddate, servertype, datetype);


                IEnumerable result = "";
                if (list != null)
                {
                    result = from order in list
                             select new
                             {
                                 Id = order.Id,
                                 Pro_id = order.Pro_id,
                                 Proname = order.Order_type == 2 ? order.payorder == 1 ? "快速支付" : "预付款充值" : prodata.GetProById(order.Pro_id.ToString(), order.Speciid, order.channelcoachid).Pro_name.Replace("'", "’"),
                                 Order_type = order.Order_type,
                                 U_name = order.U_name,
                                 U_id = order.U_id,
                                 Channelcompanyname = new MemberChannelcompanyData().GetChannelCompanyName(order.U_id),
                                 U_phone = order.U_phone,
                                 U_num = order.U_num,
                                 use_num = eticketdata.SelectEticketUnUsebyOrderid(order.Id),
                                 U_subdate = order.U_subdate,
                                 U_traveldate = order.U_traveldate,
                                 Payid = order.Payid,
                                 Pay_price = order.Pay_price,
                                 Cost = order.Cost,
                                 Pno = order.Pno,
                                 Server_type = prodata.GetProServer_typeById(order.Pro_id.ToString()),
                                 Profit = (order.Profit) * (order.U_num),
                                 Order_state = order.Order_state,
                                 //Order_eticket_code = order.Order_state == 4 ? sendate.HuoQuEticket(order.Id) : "--",
                                 Order_eticket_code = GetOrder_eticket_code(order),
                                 Order_state_str = EnumUtils.GetName((OrderStatus)order.Order_state),
                                 Pay_state = order.Pay_state,
                                 Send_state = order.Send_state,
                                 Ticketcode = order.Ticketcode,
                                 Rebate = order.Rebate,
                                 Ordercome = order.Ordercome,
                                 Totalcount = (order.Pay_price) * (order.U_num),
                                 Face_price = order.Order_type == 2 ? 0 : prodata.GetProById(order.Pro_id.ToString()).Face_price,
                                 RecerceSMSpeople = prodata.GetProRecerceSMSpeopleById(order.Pro_id.ToString()),
                                 Pay_str = paydate.GetOrderPay(order.Id),
                                 Ticketinfo = order.Ticketinfo == null ? "" : order.Ticketinfo,
                                 Integral1 = order.Integral1,
                                 Imprest1 = order.Imprest1,
                                 Paymoney = (order.Pay_price) * (order.U_num) - order.Integral1 - order.Imprest1,
                                 Openid = order.Openid,
                                 Agentid = order.Agentid,
                                 Agent_company = AgentCompanyData.GetAgentByid(order.Agentid) == null ? "" : AgentCompanyData.GetAgentByid(order.Agentid).Company,
                                 Returnmd5 = EncryptionHelper.ToMD5(order.Id.ToString() + order.Comid.ToString() + order.Agentid.ToString() + "lixh1210", "UTF-8"),
                                 Warrant_type = order.Warrant_type,
                                 Warrantid = order.Warrantid,
                                 Comid = order.Comid,
                                 Agentname = financedata.GetAgentNamebyorderid(order.Id),
                                 Source_type = prodata.GetProSource_typeById(order.Pro_id.ToString()),
                                 Hotelinfo = hoteldata.GetHotelOrderByOrderId(order.Id),
                                 IsCanReplyWx = IsIn48h(order.Openid),
                                 Userinfo = userdata.GetB2bCrmById(order.U_id),
                                 Expresscode = order.Expresscode,
                                 Expresscom = order.Expresscom,
                                 Express = order.Express,
                                 Address = order.Address,
                                 Province = order.Province,
                                 City = order.City,
                                 Code = order.Code,
                                 Order_remark = order.Order_remark,
                                 Deliverytype = order.Deliverytype,
                                 Shopcartid = order.Shopcartid,
                                 BindingOrder = order.Bindingagentorderid == 0 ? null : orderdata.GetOrderById(order.Bindingagentorderid),
                                 //Unuse_Ticket = order.Pay_state != 2 ? 0 : order.Bindingagentorderid == 0 ? eticketdata.SelectEticketUnUsebyOrderid(order.Id) : eticketdata.SelectEticketUnUsebyOrderid(order.Bindingagentorderid),//未做完，如果是已支付，检查电子码是否还有剩余数量，如果有则体现出退票，如果为0则不体现退票
                                 Unuse_Ticket = GetUnuseticketNum(order),
                                 childreduce = order.childreduce,
                                 Child_u_num = order.Child_u_num,
                                 LinePro_BookType = new B2bComProData().GetLinePro_BookType(order.Pro_id),
                                 AgentType = new Agent_companyData().GetAgentType(order.Agentid),
                                 Nuomi_dealid = new B2bOrderData().GetNuomi_dealid(order.Id),
                                 IsHasWxRefund = new B2b_pay_wxrefundlogData().IsHasWxRefund(order.Id),
                                 WxRefundDetail = new B2b_pay_wxrefundlogData().WxRefundDetailStr(order.Id),
                                 Cancelnum = order.Cancelnum,
                                 IsHasAlipayRefund = new B2b_pay_alipayrefundlogData().IsHasAlipayRefund(order.Id),
                                 AlipayRefundDetail = new B2b_pay_alipayrefundlogData().AlipayRefundDetailStr(order.Id),
                                 ////慧择网保险产品是否可以退保(返回 是否可退，出单状态(生效状态))
                                 //Hzins_Iscancancel = Hzins_Iscancancel(order.Id, order.Pro_id),
                                 //相对于慧择网来说订单类型:0 非慧择网订单；1慧择网订单 但没有生成真实保险订单；2慧择网订单 并且生成了真实保险订单
                                 OrderType_Hzins = new B2bComProData().GetOrderType_Hzins(order.Pro_id, order.Id),
                                 //保单号
                                 InsureNum = new Api_hzins_OrderApplyResp_OrderExtData().GetinsureNumbyorderid(order.Id),
                                 //是否可以立即结算:导入产品订单；去哪儿产品订单 不可结算
                                 Iscanjiesuan = Getiscanjiesuan(order),

                                 order.Bindingagentorderid,
                                 //IsShowSms=GetIsShowSms(order),

                                 order.bookpro_bindcompany,
                                 order.bookpro_bindconfirmtime,
                                 order.bookpro_bindname,
                                 order.bookpro_bindphone,
                                 order.payorder,
                                 yiguoqi = prodata.GetProyouxiaoqiById(order.Pro_id),
                                 order.U_idcard,
                             };
                }
                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        private static object GetInsureNum(int p)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region 获取购物车订单列表哦
        public static string OrderCartPageList(string comid, int cartid)
        {
            var totalcount = 0;
            try
            {

                var orderdata = new B2bOrderData();
                var financedata = new B2bFinanceData();
                var prodata = new B2bComProData();
                var paydate = new B2bPayData();
                var hoteldata = new B2b_order_hotelData();
                var userdata = new B2bCrmData();
                B2bEticketData eticketdata = new B2bEticketData();
                SendEticketData sendate = new SendEticketData();

                var list = orderdata.OrderCartPageList(comid, cartid, out totalcount);


                IEnumerable result = "";
                if (list != null)

                    result = from order in list
                             select new
                             {
                                 Id = order.Id,
                                 Pro_id = order.Pro_id,
                                 Proname = order.Order_type == 2 ? "预付款充值" : prodata.GetProById(order.Pro_id.ToString(), order.Speciid).Pro_name,
                                 Order_type = order.Order_type,
                                 U_name = order.U_name,
                                 U_id = order.U_id,
                                 Channelcompanyname = new MemberChannelcompanyData().GetChannelCompanyName(order.U_id),
                                 U_phone = order.U_phone,
                                 U_num = order.U_num,
                                 U_subdate = order.U_subdate,
                                 U_traveldate = order.U_traveldate,
                                 Payid = order.Payid,
                                 Pay_price = order.Pay_price,
                                 Cost = order.Cost,
                                 Pno = order.Pno,
                                 Server_type = prodata.GetProServer_typeById(order.Pro_id.ToString()),
                                 Profit = (order.Profit) * (order.U_num),
                                 Order_state = order.Order_state,
                                 Order_eticket_code = order.Order_state == 4 ? sendate.HuoQuEticket(order.Id) : "--",
                                 Order_state_str = EnumUtils.GetName((OrderStatus)order.Order_state),
                                 Pay_state = order.Pay_state,
                                 Send_state = order.Send_state,
                                 Ticketcode = order.Ticketcode,
                                 Rebate = order.Rebate,
                                 Ordercome = order.Ordercome,
                                 Totalcount = (order.Pay_price) * (order.U_num),
                                 Face_price = order.Order_type == 2 ? 0 : prodata.GetProById(order.Pro_id.ToString()).Face_price,
                                 Pay_str = paydate.GetOrderPay(order.Id),
                                 Ticketinfo = order.Ticketinfo == null ? "" : order.Ticketinfo,
                                 Integral1 = order.Integral1,
                                 Imprest1 = order.Imprest1,
                                 Paymoney = (order.Pay_price) * (order.U_num) - order.Integral1 - order.Imprest1,
                                 Openid = order.Openid,
                                 Agentid = order.Agentid,
                                 Agent_company = AgentCompanyData.GetAgentByid(order.Agentid) == null ? "" : AgentCompanyData.GetAgentByid(order.Agentid).Company,
                                 Returnmd5 = EncryptionHelper.ToMD5(order.Id.ToString() + order.Comid.ToString() + order.Agentid.ToString() + "lixh1210", "UTF-8"),
                                 Warrant_type = order.Warrant_type,
                                 Warrantid = order.Warrantid,
                                 Comid = order.Comid,
                                 Agentname = financedata.GetAgentNamebyorderid(order.Id),
                                 Source_type = prodata.GetProSource_typeById(order.Pro_id.ToString()),
                                 Hotelinfo = hoteldata.GetHotelOrderByOrderId(order.Id),
                                 IsCanReplyWx = IsIn48h(order.Openid),
                                 Userinfo = userdata.GetB2bCrmById(order.U_id),
                                 Expresscode = order.Expresscode,
                                 Expresscom = order.Expresscom,
                                 Express = order.Express,
                                 Address = order.Address,
                                 Province = order.Province,
                                 City = order.City,
                                 Code = order.Code,
                                 Order_remark = order.Order_remark,
                                 Deliverytype = order.Deliverytype,
                                 Shopcartid = order.Shopcartid,
                                 BindingOrder = order.Bindingagentorderid == 0 ? null : orderdata.GetOrderById(order.Bindingagentorderid),
                                 Unuse_Ticket = order.Pay_state != 2 ? 0 : order.Bindingagentorderid == 0 ? eticketdata.SelectEticketUnUsebyOrderid(order.Id) : eticketdata.SelectEticketUnUsebyOrderid(order.Bindingagentorderid)//未做完，如果是已支付，检查电子码是否还有剩余数量，如果有则体现出退票，如果为0则不体现退票

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
        #region 获取订单统计
        public static string OrderCountList(string comid, string startime, string endtime, int searchtype, int userid = 0)
        {
            var totalcount = 0;
            try
            {

                var orderdata = new B2bOrderData();
                var financedata = new B2bFinanceData();
                var prodata = new B2bComProData();

                B2bEticketData eticketdata = new B2bEticketData();
                SendEticketData sendate = new SendEticketData();


                if (searchtype == 1)
                {


                    TimeSpan ts = DateTime.Parse(endtime) - DateTime.Parse(startime);
                    if (ts.Days > 31)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "查询时间过长，只能查询一个月内的每天统计" });
                    }

                    //结束日期增加一天，这样小于结束日期
                    endtime = DateTime.Parse(endtime).AddDays(1).ToString();


                }

                if (searchtype == 2)
                {
                    //初始化为 1号
                    startime = new DateTime(DateTime.Parse(startime).Year, DateTime.Parse(startime).Month, 1).ToString();
                    //初始化 为 下月第一天
                    endtime = new DateTime(DateTime.Parse(endtime).Year, DateTime.Parse(endtime).Month, 1).AddMonths(1).ToString();
                }

                var list = orderdata.OrderCountList(comid, startime, endtime, searchtype, out totalcount, userid);


                IEnumerable result = "";
                if (list != null)

                    result = from order in list
                             select new
                             {
                                 Datestr = order.Datestr, //日期
                                 Countnum = order.Countnum, //笔数
                                 Profit = order.Profit,//毛利
                                 Integral1 = order.Integral1,//优惠
                                 Pay_price = order.Pay_price,//支付金额
                                 U_num = order.U_num,//预订数量
                                 BackPrice = B2bOrderData.BackPrice(comid, order.Datestr, order.Datestr),//退款
                                 WeixinSale = B2bOrderData.WeixinSale(comid, order.Datestr, order.Datestr),//微信
                                 WebSale = B2bOrderData.WebSale(comid, order.Datestr, order.Datestr),//官网
                                 LineSale = "",//电商
                                 UseState = B2bOrderData.UseState(comid, order.Datestr, order.Datestr),//已消费
                                 UnUseState = B2bOrderData.UnUseState(comid, order.Datestr, order.Datestr),//未消费

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


        #region 获取订单财务确认
        public static string Orderfinset(string comid, string startime, string endtime, int mangefinset, string key)
        {
            var totalcount = 0;
            try
            {

                var orderdata = new B2bOrderData();


                var list = orderdata.Orderfinset(comid, startime, endtime, mangefinset, key, out totalcount);


                IEnumerable result = "";
                if (list != null)

                    result = from order in list
                             select new
                             {
                                 submanagename = order.submanagename, //管理员
                                 Countnum = order.Countnum, //笔数
                                 Pay_price = order.Pay_price,//支付金额
                                 U_num = order.U_num,//预订数量
                                 riqi = startime + "/" + endtime,
                                 stardate=startime,
                                 enddate=endtime,
                                 comid = comid,
                                 zhifu = orderdata.Orderfinset_pay_price(comid, startime, endtime, mangefinset, key, order.submanagename)//各个方法支付比例
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


        #region 获取订单财务确认
        public static string Orderfinset_pay_price_list(string comid, string startime, string endtime, int mangefinset, string key, string submanagename)
        {
            var totalcount = 0;
            try
            {

                var orderdata = new B2bOrderData();
                var financedata = new B2bFinanceData();
                var prodata = new B2bComProData();
                var paydate = new B2bPayData();
                var hoteldata = new B2b_order_hotelData();
                var userdata = new B2bCrmData();
                B2bEticketData eticketdata = new B2bEticketData();
                SendEticketData sendate = new SendEticketData();


                var list = orderdata.Orderfinset_pay_price_list(comid, startime, endtime, mangefinset, key, submanagename, out totalcount);
                IEnumerable result = "";
                if (list != null)
                {
                    result = from order in list
                             select new
                             {
                                 Id = order.Id,
                                 Pro_id = order.Pro_id,
                                 Proname = order.Order_type == 2 ? "预付款充值" : prodata.GetProById(order.Pro_id.ToString(), order.Speciid, order.channelcoachid).Pro_name.Replace("'", "’"),
                                 Order_type = order.Order_type,
                                 U_name = order.U_name,
                                 U_id = order.U_id,
                                 Channelcompanyname = new MemberChannelcompanyData().GetChannelCompanyName(order.U_id),
                                 U_phone = order.U_phone,
                                 U_num = order.U_num,
                                 U_subdate = order.U_subdate,
                                 U_traveldate = order.U_traveldate,
                                 Payid = order.Payid,
                                 Pay_price = order.Pay_price,
                                 Cost = order.Cost,
                                 Pno = order.Pno,
                                 Server_type = prodata.GetProServer_typeById(order.Pro_id.ToString()),
                                 Profit = (order.Profit) * (order.U_num),
                                 Order_state = order.Order_state,
                                 //Order_eticket_code = order.Order_state == 4 ? sendate.HuoQuEticket(order.Id) : "--",
                                 Order_eticket_code = GetOrder_eticket_code(order),
                                 Order_state_str = EnumUtils.GetName((OrderStatus)order.Order_state),
                                 Pay_state = order.Pay_state,
                                 Send_state = order.Send_state,
                                 Ticketcode = order.Ticketcode,
                                 Rebate = order.Rebate,
                                 Ordercome = order.Ordercome,
                                 Totalcount = (order.Pay_price) * (order.U_num),
                                 Face_price = order.Order_type == 2 ? 0 : prodata.GetProById(order.Pro_id.ToString()).Face_price,
                                 RecerceSMSpeople = prodata.GetProRecerceSMSpeopleById(order.Pro_id.ToString()),
                                 Pay_str = paydate.GetOrderPay(order.Id),
                                 Ticketinfo = order.Ticketinfo == null ? "" : order.Ticketinfo,
                                 Integral1 = order.Integral1,
                                 Imprest1 = order.Imprest1,
                                 Paymoney = (order.Pay_price) * (order.U_num) - order.Integral1 - order.Imprest1,
                                 Openid = order.Openid,
                                 Agentid = order.Agentid,
                                 Agent_company = AgentCompanyData.GetAgentByid(order.Agentid) == null ? "" : AgentCompanyData.GetAgentByid(order.Agentid).Company,
                                 Returnmd5 = EncryptionHelper.ToMD5(order.Id.ToString() + order.Comid.ToString() + order.Agentid.ToString() + "lixh1210", "UTF-8"),
                                 Warrant_type = order.Warrant_type,
                                 Warrantid = order.Warrantid,
                                 Comid = order.Comid,
                                 Agentname = financedata.GetAgentNamebyorderid(order.Id),
                                 Source_type = prodata.GetProSource_typeById(order.Pro_id.ToString()),
                                 Hotelinfo = hoteldata.GetHotelOrderByOrderId(order.Id),
                                 IsCanReplyWx = IsIn48h(order.Openid),
                                 Userinfo = userdata.GetB2bCrmById(order.U_id),
                                 Expresscode = order.Expresscode,
                                 Expresscom = order.Expresscom,
                                 Express = order.Express,
                                 Address = order.Address,
                                 Province = order.Province,
                                 City = order.City,
                                 Code = order.Code,
                                 Order_remark = order.Order_remark,
                                 Deliverytype = order.Deliverytype,
                                 Shopcartid = order.Shopcartid,
                                 BindingOrder = order.Bindingagentorderid == 0 ? null : orderdata.GetOrderById(order.Bindingagentorderid),
                                 //Unuse_Ticket = order.Pay_state != 2 ? 0 : order.Bindingagentorderid == 0 ? eticketdata.SelectEticketUnUsebyOrderid(order.Id) : eticketdata.SelectEticketUnUsebyOrderid(order.Bindingagentorderid),//未做完，如果是已支付，检查电子码是否还有剩余数量，如果有则体现出退票，如果为0则不体现退票
                                 Unuse_Ticket = GetUnuseticketNum(order),
                                 childreduce = order.childreduce,
                                 Child_u_num = order.Child_u_num,
                                 LinePro_BookType = new B2bComProData().GetLinePro_BookType(order.Pro_id),
                                 AgentType = new Agent_companyData().GetAgentType(order.Agentid),
                                 Nuomi_dealid = new B2bOrderData().GetNuomi_dealid(order.Id),
                                 IsHasWxRefund = new B2b_pay_wxrefundlogData().IsHasWxRefund(order.Id),
                                 WxRefundDetail = new B2b_pay_wxrefundlogData().WxRefundDetailStr(order.Id),
                                 Cancelnum = order.Cancelnum,
                                 IsHasAlipayRefund = new B2b_pay_alipayrefundlogData().IsHasAlipayRefund(order.Id),
                                 AlipayRefundDetail = new B2b_pay_alipayrefundlogData().AlipayRefundDetailStr(order.Id),
                                 ////慧择网保险产品是否可以退保(返回 是否可退，出单状态(生效状态))
                                 //Hzins_Iscancancel = Hzins_Iscancancel(order.Id, order.Pro_id),
                                 //相对于慧择网来说订单类型:0 非慧择网订单；1慧择网订单 但没有生成真实保险订单；2慧择网订单 并且生成了真实保险订单
                                 OrderType_Hzins = new B2bComProData().GetOrderType_Hzins(order.Pro_id, order.Id),
                                 //保单号
                                 InsureNum = new Api_hzins_OrderApplyResp_OrderExtData().GetinsureNumbyorderid(order.Id),
                                 //是否可以立即结算:导入产品订单；去哪儿产品订单 不可结算
                                 Iscanjiesuan = Getiscanjiesuan(order),

                                 order.Bindingagentorderid,
                                 //IsShowSms=GetIsShowSms(order),

                                 order.bookpro_bindcompany,
                                 order.bookpro_bindconfirmtime,
                                 order.bookpro_bindname,
                                 order.bookpro_bindphone,
                                 order.submanagename,
                             };
                }
                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        
            #region 获取订单财务确认-取人已处理
        public static string orderfinset_quren(string comid, string startime, string endtime, int mangefinset, string key, string submanagename)
        {
            var totalcount = 0;
            try
            {

                var orderdata = new B2bOrderData();


                var list = orderdata.orderfinset_quren(comid, startime, endtime, mangefinset, key, submanagename);

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = list });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion
        #region
        private static int IsIn48h(string openid)
        {
            if (openid != "")
            {
                B2b_crm crm = new B2bCrmData().GetB2bCrmByWeiXin(openid);
                if (crm != null)
                {
                    TimeSpan ts = DateTime.Now - crm.Wxlastinteracttime;
                    if (ts.TotalSeconds <= 48 * 60 * 60 && crm.Whetherwxfocus == true)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        #endregion


        #region 合作商获取订单列表哦
        public static string CoopOrderPageList(string comid, int pageindex, int pagesize, string ordercome)
        {
            var totalcount = 0;
            try
            {

                var orderdata = new B2bOrderData();

                var prodata = new B2bComProData();
                var paydate = new B2bPayData();
                SendEticketData sendate = new SendEticketData();

                var list = orderdata.CoopOrderPageList(comid, pageindex, pagesize, ordercome, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from order in list
                             select new
                             {
                                 Id = order.Id,
                                 Pro_id = order.Pro_id,
                                 Proname = order.Order_type == 2 ? "预付款充值" : prodata.GetProById(order.Pro_id.ToString(), order.Speciid).Pro_name,
                                 Order_type = order.Order_type,
                                 U_name = order.U_name,
                                 U_id = order.U_id,
                                 U_phone = order.U_phone,
                                 U_num = order.U_num,
                                 U_subdate = order.U_subdate,
                                 U_traveldate = order.U_traveldate,
                                 Payid = order.Payid,
                                 Pay_price = order.Pay_price,
                                 Cost = order.Cost,
                                 Server_type = prodata.GetProById(order.Pro_id.ToString()) == null ? 0 : prodata.GetProById(order.Pro_id.ToString()).Server_type,
                                 Profit = (order.Profit) * (order.U_num),
                                 Order_state = order.Order_state,
                                 Order_eticket_code = order.Order_state == 4 ? sendate.HuoQuEticket(order.Id) : "--",
                                 Order_state_str = EnumUtils.GetName((OrderStatus)order.Order_state),
                                 Pay_state = order.Pay_state,
                                 Send_state = order.Send_state,
                                 Ticketcode = order.Ticketcode,
                                 Rebate = order.Rebate,
                                 Ordercome = order.Ordercome,
                                 Totalcount = (order.Pay_price) * (order.U_num),
                                 Face_price = order.Order_type == 2 ? 0 : prodata.GetProById(order.Pro_id.ToString()).Face_price,
                                 Pay_str = paydate.GetOrderPay(order.Id),
                                 Ticketinfo = order.Ticketinfo == null ? "" : order.Ticketinfo,
                                 Integral1 = order.Integral1,
                                 Imprest1 = order.Imprest1,
                                 Paymoney = (order.Pay_price) * (order.U_num) - order.Integral1 - order.Imprest1,
                                 Openid = order.Openid,
                                 Agentid = order.Agentid,
                                 Agent_company = AgentCompanyData.GetAgentByid(order.Agentid) == null ? "" : AgentCompanyData.GetAgentByid(order.Agentid).Company,
                                 Returnmd5 = EncryptionHelper.ToMD5(order.Id.ToString() + order.Comid.ToString() + order.Agentid.ToString() + "lixh1210", "UTF-8"),
                                 Warrant_type = order.Warrant_type,
                                 Warrantid = order.Warrantid,
                                 Comid = order.Comid
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

        #region 合作商获取订单列表哦
        public static string CoopVerOrderPageList(string comid, int pageindex, int pagesize, string ordercome)
        {
            var totalcount = 0;
            try
            {

                var orderdata = new B2bOrderData();

                var prodata = new B2bComProData();
                var paydate = new B2bPayData();
                SendEticketData sendate = new SendEticketData();

                var list = orderdata.CoopVerOrderPageList(comid, pageindex, pagesize, ordercome, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from order in list
                             select new
                             {
                                 Id = order.Id,
                                 Pro_id = order.Pro_id,
                                 Proname = order.Order_type == 2 ? "预付款充值" : prodata.GetProById(order.Pro_id.ToString(), order.Speciid).Pro_name,
                                 Order_type = order.Order_type,
                                 U_name = order.U_name,
                                 U_id = order.U_id,
                                 U_phone = order.U_phone,
                                 U_num = order.U_num,
                                 U_subdate = order.U_subdate,
                                 U_traveldate = order.U_traveldate,
                                 Payid = order.Payid,
                                 Pay_price = order.Pay_price,
                                 Cost = order.Cost,
                                 Server_type = prodata.GetProById(order.Pro_id.ToString()) == null ? 0 : prodata.GetProById(order.Pro_id.ToString()).Server_type,
                                 Profit = (order.Profit) * (order.U_num),
                                 Order_state = order.Order_state,
                                 Order_eticket_code = order.Order_state == 4 ? sendate.HuoQuEticket(order.Id) : "--",
                                 Order_state_str = EnumUtils.GetName((OrderStatus)order.Order_state),
                                 Pay_state = order.Pay_state,
                                 Send_state = order.Send_state,
                                 Ticketcode = order.Ticketcode,
                                 Rebate = order.Rebate,
                                 Ordercome = order.Ordercome,
                                 Totalcount = (order.Pay_price) * (order.U_num),
                                 Face_price = order.Order_type == 2 ? 0 : prodata.GetProById(order.Pro_id.ToString()).Face_price,
                                 Pay_str = paydate.GetOrderPay(order.Id),
                                 Ticketinfo = order.Ticketinfo == null ? "" : order.Ticketinfo,
                                 Integral1 = order.Integral1,
                                 Imprest1 = order.Imprest1,
                                 Paymoney = (order.Pay_price) * (order.U_num) - order.Integral1 - order.Imprest1,
                                 Openid = order.Openid,
                                 Agentid = order.Agentid,
                                 Agent_company = AgentCompanyData.GetAgentByid(order.Agentid) == null ? "" : AgentCompanyData.GetAgentByid(order.Agentid).Company,
                                 Returnmd5 = EncryptionHelper.ToMD5(order.Id.ToString() + order.Comid.ToString() + order.Agentid.ToString() + "lixh1210", "UTF-8"),
                                 Warrant_type = order.Warrant_type,
                                 Warrantid = order.Warrantid,
                                 Comid = order.Comid,
                                 Pno = order.Pno
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


        #region 获取订单列表哦
        public static string AgentOrderPageList(string comid, int agentid, string account, int pageindex, int pagesize, string key, int order_state, string begindate = "", string enddate = "", int servertype = 0)
        {
            var totalcount = 0;
            try
            {

                var orderdata = new B2bOrderData();
                var prodata = new B2bComProData();
                var paydate = new B2bPayData();
                int agentlevel = 1;//默认为普通员工级别

                if (account != "")
                {
                    var agentmodel = AgentCompanyData.AgentSearchAccount(account);
                    if (agentmodel != null)
                    {
                        agentlevel = agentmodel.AccountLevel;
                    }
                }
                var hoteldata = new B2b_order_hotelData();
                SendEticketData sendate = new SendEticketData();
                B2bEticketData eticketdata = new B2bEticketData();
                var list = orderdata.AgentOrderPageList(comid, agentid, pageindex, pagesize, key, order_state, out totalcount, begindate, enddate, servertype);
                IEnumerable result = "";
                if (list != null)

                    result = from order in list
                             select new
                             {
                                 Id = order.Id,
                                 Pro_id = order.Pro_id,
                                 Proname = order.Order_type == 2 ? "预付款充值" : prodata.GetProById(order.Pro_id.ToString(), order.Speciid).Pro_name,
                                 Order_type = order.Order_type,
                                 U_name = order.U_name,
                                 U_id = order.U_id,
                                 U_phone = order.U_phone,
                                 U_num = order.U_num,
                                 U_subdate = order.U_subdate,
                                 Payid = order.Payid,
                                 Pay_price = order.Pay_price,
                                 Cost = order.Cost,
                                 Server_type = prodata.GetProById(order.Pro_id.ToString()) == null ? 0 : prodata.GetProById(order.Pro_id.ToString()).Server_type,
                                 Profit = (order.Profit) * (order.U_num),
                                 Order_state = order.Order_state,
                                 //Order_eticket_code = order.Order_state == 4 ? sendate.HuoQuEticket(order.Id) : "--",
                                 Order_eticket_code = GetOrder_eticket_code(order),
                                 Order_state_str = EnumUtils.GetName((OrderStatus)order.Order_state),
                                 Pay_state = order.Pay_state,
                                 Send_state = order.Send_state,
                                 Ticketcode = order.Ticketcode,
                                 Rebate = order.Rebate,
                                 Ordercome = order.Ordercome,
                                 Totalcount = (order.Pay_price) * (order.U_num),
                                 Face_price = order.Order_type == 2 ? 0 : order.Speciid == 0 ? prodata.GetProById(order.Pro_id.ToString()).Face_price : prodata.GetProspeciidById(order.Pro_id.ToString(),order.Speciid).Face_price,
                                 Pay_str = paydate.GetOrderPay(order.Id),
                                 Ticketinfo = order.Ticketinfo == null ? "" : order.Ticketinfo,
                                 Integral1 = order.Integral1,
                                 Imprest1 = order.Imprest1,
                                 Paymoney = (order.Pay_price) * (order.U_num) + order.Express - order.Integral1 - order.Imprest1,
                                 Openid = order.Openid,
                                 Agentid = order.Agentid,
                                 Agent_company = AgentCompanyData.GetAgentByid(order.Agentid) == null ? "" : AgentCompanyData.GetAgentByid(order.Agentid).Company,
                                 Returnmd5 = EncryptionHelper.ToMD5(order.Id.ToString() + order.Comid.ToString() + order.Agentid.ToString() + "lixh1210", "UTF-8"),
                                 Warrant_type = order.Warrant_type,
                                 Warrantid = order.Warrantid,
                                 Comid = order.Comid,
                                 Comname = new B2bCompanyData().GetCompanyNameById(order.Comid),
                                 Agentlevel = agentlevel,

                                 Source_type = new B2bComProData().GetSourcetypebyproid(order.Pro_id),
                                 BindingOrder = order.Bindingagentorderid == 0 ? null : orderdata.GetOrderById(order.Bindingagentorderid),
                                 //Unuse_Ticket =order.Pay_state != 2 ? 0 : order.Bindingagentorderid == 0 ? eticketdata.SelectEticketUnUsebyOrderid(order.Id) : eticketdata.SelectEticketUnUsebyOrderid(order.Bindingagentorderid),//未做完，如果是已支付，检查电子码是否还有剩余数量，如果有则体现出退票，如果为0则不体现退票
                                 Unuse_Ticket = GetUnuseticketNum(order),//未做完，如果是已支付，检查电子码是否还有剩余数量，如果有则体现出退票，如果为0则不体现退票
                                 Expresscode = order.Expresscode,
                                 Expresscom = order.Expresscom,
                                 Express = order.Express,
                                 Address = order.Address,
                                 Code = order.Code,
                                 Order_remark = order.Order_remark,
                                 Deliverytype = order.Deliverytype,
                                 ProImg = GetProImg(order.Pro_id),
                                 childreduce = order.childreduce,
                                 Child_u_num = order.Child_u_num,
                                 Cancelnum = order.Cancelnum,
                                 Hotelinfo = hoteldata.GetHotelOrderByOrderId(order.Id),
                                 //是否可以进行淘宝发码回调(满足条件:24h内 提单成功 但是 发送淘宝发码回调失败的订单)
                                 Iscantaobo_sendret = new Taobao_send_noticeretlogData().GetIscantaobo_sendret(order.Id),
                                 ////慧择网保险产品是否可以退保(返回 是否可退，出单状态(生效状态))
                                 //Hzins_Iscancancel = Hzins_Iscancancel(order.Id, order.Pro_id),
                                 //相对于慧择网来说订单类型:0 非慧择网订单；1慧择网订单 但没有生成真实保险订单；2慧择网订单 并且生成了真实保险订单
                                 OrderType_Hzins = new B2bComProData().GetOrderType_Hzins(order.Pro_id, order.Id),
                                 //保单号
                                 InsureNum = new Api_hzins_OrderApplyResp_OrderExtData().GetinsureNumbyorderid(order.Id),
                                 //是否可以立即结算:导入产品b订单；去哪儿产品订单 不可结算
                                 Iscanjiesuan = Getiscanjiesuan(order),

                                 order.Bindingagentorderid,
                                 //IsShowSms = GetIsShowSms(order),//是否显示短信内容
                                 yiguoqi = prodata.GetProyouxiaoqiById(order.Pro_id),
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        private static string GetOrder_eticket_code(B2b_order order)
        {
            //order.Order_state == 4 ? sendate.HuoQuEticket(order.Id) : "--",
            //非导入产品
            if (order.Bindingagentorderid == 0)
            {
                int sourcetype = new B2bComProData().GetSourcetypebyproid(order.Pro_id);
                if (sourcetype == 3)//外部接口产品
                {
                    return "外部接口产品，电子码:" + order.service_code;
                }
                else
                {
                    string smscontent = new SendEticketData().HuoQuEticket(order.Id);
                    return smscontent;
                }
            }
            //导入产品
            else
            {
                int sourcetype = new B2bComProData().GetSourcetypeByOrderid(order.Bindingagentorderid);
                if (sourcetype == 3)//外部接口产品
                {
                    string service_code = new B2bOrderData().GetService_code(order.Bindingagentorderid);
                    return "外部接口产品，电子码:" + service_code;
                }
                else
                {
                    string smscontent = new SendEticketData().HuoQuEticket(order.Bindingagentorderid);
                    return smscontent;
                }

            }
        }

        private static int GetIsShowSms(B2b_order order)
        {
            //非导入产品
            if (order.Bindingagentorderid == 0)
            {
                int sourcetype = new B2bComProData().GetSourcetypebyproid(order.Pro_id);
                if (sourcetype == 3)//外部接口产品
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            //导入产品
            else
            {
                int sourcetype = new B2bComProData().GetSourcetypeByOrderid(order.Bindingagentorderid);
                if (sourcetype == 3)//外部接口产品
                {
                    return 0;
                }
                else
                {
                    return 1;
                }

            }
        }

        private static int GetUnuseticketNum(B2b_order order)
        {
            //order.Pay_state != 2 ? 0 : order.Bindingagentorderid == 0 ? eticketdata.SelectEticketUnUsebyOrderid(order.Id) : eticketdata.SelectEticketUnUsebyOrderid(order.Bindingagentorderid)
            if (order.Pay_state != 2)
            {
                return 0;
            }
            else
            {
                //非导入产品
                if (order.Bindingagentorderid == 0)
                {
                    int sourcetype = new B2bComProData().GetSourcetypebyproid(order.Pro_id);
                    if (sourcetype == 3)//外部接口产品
                    {
                        return order.service_lastcount;
                    }
                    else
                    {
                        int lastcount = new B2bEticketData().SelectEticketUnUsebyOrderid(order.Id);
                        return lastcount;
                    }
                }
                //导入产品
                else
                {
                    int sourcetype = new B2bComProData().GetSourcetypeByOrderid(order.Bindingagentorderid);
                    if (sourcetype == 3)//外部接口产品
                    {
                        int service_lastcount = new B2bOrderData().GetServiceLastcount(order.Bindingagentorderid);
                        return service_lastcount;
                    }
                    else
                    {
                        int lastcount = new B2bEticketData().SelectEticketUnUsebyOrderid(order.Bindingagentorderid);
                        return lastcount;
                    }

                }
            }
        }
        /// <summary>
        /// 是否可以立即结算:去哪儿订单；导入产品订单 不可立即结算
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private static int Getiscanjiesuan(B2b_order order)
        {
            if (order != null)
            {
                //去哪儿订单
                if (order.qunar_orderid != "")
                {
                    return 0;
                }
                else
                {
                    //根据b订单获得a订单，如果a订单存在则当前b订单不可立即结算
                    int aorderid = new B2bOrderData().GetIdOrderBybindingId(order.Id);

                    if (aorderid > 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            else
            {
                return 0;
            }
        }

        private static string Hzins_Iscancancel(int orderid, int proid)
        {
            //判断是否为外来接口（服务商：慧择网）产品
            int serviceid = new B2bComProData().GetServiceidbyproid(proid);
            if (serviceid == 2)
            {
                string result = new HzinsInter().Hzins_orderDetail(orderid);
                if (result != "")
                {
                    Hzins_SearchInsureResp resp = (Hzins_SearchInsureResp)JsonConvert.DeserializeObject(result, typeof(Hzins_SearchInsureResp));
                    if (resp.respCode == 0)
                    {
                        if (resp.data != null)
                        {
                            if (resp.data.orderDetailInfos != null)
                            {
                                List<Hzins_SearchInsureResp_OrderDetailInfo> infos = resp.data.orderDetailInfos;
                                foreach (Hzins_SearchInsureResp_OrderDetailInfo info in infos)
                                {
                                    if (info != null)
                                    {
                                        int iscancancel = 0;//是否可退保
                                        if (info.payState == 4)//已支付
                                        {
                                            if (info.effectiveState == 1)//未生效
                                            {
                                                iscancancel = 1;
                                            }
                                            else
                                            {
                                                iscancancel = 0;
                                            }


                                        }

                                        string paystatestr = EnumUtils.GetName((Hzins_payState)info.payState);
                                        string issuestatestr = EnumUtils.GetName((Hzins_issueState)info.issueState);
                                        string effectiveStatestr = EnumUtils.GetName((Hzins_effectiveState)info.effectiveState);

                                        return iscancancel + "," + issuestatestr + "(" + effectiveStatestr + ")";
                                    }
                                }
                            }
                        }
                    }


                }
                //查询失败
                return "-1";
            }
            else
            {
                return "-2";
            }




        }

        private static string GetProImg(int proid)
        {
            int imgurl = new B2bComProData().GetImgUrl(proid);

            return FileSerivce.GetImgUrl(imgurl);

        }
        #endregion

        #region 分销订单汇总
        public static string AgentOrderCount(string comid, int agentid, int pageindex, int pagesize, string key = "", string startime = "", string endtime = "")
        {
            var totalcount = 0;
            try
            {

                var orderdata = new B2bOrderData();
                var prodata = new B2bComProData();
                var eticketdate = new B2bEticketData();

                SendEticketData sendate = new SendEticketData();

                var list = orderdata.AgentOrderCount(comid, agentid, pageindex, pagesize, out totalcount, key);
                IEnumerable result = "";
                if (list != null)

                    result = from order in list
                             select new
                             {
                                 Pro_id = order.Pro_id,
                                 Proname = prodata.GetProById(order.Pro_id.ToString()).Pro_name,
                                 TicketNum = order.U_num,
                                 VNum = eticketdate.GetEticketVCount(Int32.Parse(comid), agentid, order.Pro_id, startime, endtime),
                                 Agentid = agentid,
                                 Price = Agent_companyData.WarrantProPrice(order.Pro_id, agentid, Int32.Parse(comid)),
                                 Comid = comid
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


        #region 分销订单汇总
        public static string GetIdAgentOrderCount(string comid, int agentid, int orderid)
        {
            try
            {
                string Proname = "";
                int TicketNum = 0;
                int VNum = 0;
                int UnVNum = 0;
                int voidNum = 0;
                decimal Price = 0;


                var orderdata = new B2bOrderData();
                var prodata = new B2bComProData();
                var eticketdate = new B2bEticketData();
                SendEticketData sendate = new SendEticketData();

                var order = orderdata.GetOrderById(orderid);

                if (order != null)
                {
                    //获取产品名称
                    var promodel = prodata.ClientGetProById(order.Pro_id.ToString(), order.Comid);
                    if (promodel != null)
                    {
                        Proname = promodel.Pro_name;
                    }

                    //产品倒码数量
                    TicketNum = order.U_num;
                    //验证数量
                    VNum = eticketdate.GetEticketOrderVCount(Int32.Parse(comid), agentid, orderid);

                    //未使用数量
                    UnVNum = eticketdate.GetEticketOrderVoidCount(Int32.Parse(comid), agentid, orderid);

                    //作废数量
                    voidNum = TicketNum - VNum - UnVNum;
                    //结算价格
                    Price = order.Pay_price * VNum;

                };

                return JsonConvert.SerializeObject(new { type = 100, msg = Proname, TicketNum = TicketNum, VNum = VNum, voidNum = voidNum, UnVNum = UnVNum, Price = Price });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 分销倒码订单整体作废
        public static string EticketOrderVoid(int comid, int agentid, int orderid)
        {
            try
            {
                var eticketdata = new B2bEticketData();
                var order = eticketdata.EticketOrderVoid(comid, agentid, orderid);

                return JsonConvert.SerializeObject(new { type = 100, msg = order });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 获取处理退票列表
        public static string TicketPageList(int pageindex, int pagesize, string key, int order_state, int endstate)
        {
            var totalcount = 0;
            try
            {

                var orderdata = new B2bOrderData();

                var prodata = new B2bComProData();
                var paydate = new B2bPayData();
                SendEticketData sendate = new SendEticketData();

                var list = orderdata.TicketPageList(pageindex, pagesize, key, order_state, endstate, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from order in list
                             select new
                             {
                                 Id = order.Id,
                                 Pro_id = order.Pro_id,
                                 Proname = order.Order_type == 2 ? "预付款充值" : prodata.GetProById(order.Pro_id.ToString(), order.Speciid).Pro_name,
                                 Order_type = order.Order_type,
                                 U_name = order.U_name,
                                 U_id = order.U_id,
                                 U_phone = order.U_phone,
                                 U_num = order.U_num,
                                 U_subdate = order.U_subdate,
                                 Payid = order.Payid,
                                 Pay_price = order.Pay_price,
                                 Cost = order.Cost,
                                 Profit = (order.Profit) * (order.U_num),
                                 Order_state = order.Order_state,
                                 Order_eticket_code = order.Order_state == 4 ? sendate.HuoQuEticket(order.Id) : "--",
                                 Order_state_str = EnumUtils.GetName((OrderStatus)order.Order_state),
                                 Pay_state = order.Pay_state,
                                 Send_state = order.Send_state,
                                 Ticketcode = order.Ticketcode,
                                 Rebate = order.Rebate,
                                 Ordercome = order.Ordercome,
                                 Totalcount = (order.Pay_price) * (order.U_num) + order.Express - order.Integral1,
                                 Face_price = order.Order_type == 2 ? 0 : prodata.GetProById(order.Pro_id.ToString()).Face_price,
                                 Pay_str = paydate.GetOrderPay(order.Id),
                                 Ticket = order.Ticket,
                                 Ticketinfo = order.Ticketinfo,
                                 ma = prodata.GetProById(order.Pro_id.ToString()).Server_type,
                                 pno = order.Pno,
                                 //come_pay = paydate.GetPayByoId(order.Id) == null ? "" : paydate.GetPayByoId(order.Id).Pay_com,
                                 come_pay = paydate.GetPaycomByorder(order),
                                 trade_no = paydate.GetPayByoId(order.Id) == null ? "" : paydate.GetPayByoId(order.Id).Trade_no,
                                 Total_fee = paydate.GetPayByoId(order.Id) == null ? 0 : paydate.GetPayByoId(order.Id).Total_fee,
                                 Use_pno = new B2bEticketData().SelectOrderid(order.Id) == null ? 0 : new B2bEticketData().SelectOrderid(order.Id).Use_pnum,
                                 Backtickettime = order.Backtickettime,
                                 Comname = B2bCompanyData.GetCompany(order.Comid) == null ? "" : B2bCompanyData.GetCompany(order.Comid).Com_name,
                                 Express = order.Express,
                                 order.askquitfee,
                                 //Pay_desc = paydate.GetOrderPaydesc(order.Id),
                                 order.Agentid,
                                 Agentname = new AgentCompanyData().GetAgentCompanyName(order.Agentid)
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

        #region 销售统计分页列表
        public static string SaleCountPageList(string comid, int pageindex, int pagesize, string startdate, string enddate, string key = "")
        {
            var totalcount = 0;
            try
            {

                var orderdata = new B2bOrderData();
                var prodata = new B2bComProData();

                var list = prodata.ProPageList(comid, pageindex, pagesize, 0, out totalcount, 0, key);
                IEnumerable result = "";
                if (list != null)

                    result = from order in list
                             select new
                             {
                                 Pro_name = order.Pro_name,
                                 Face_price = order.Face_price,
                                 Advise_price = order.Advise_price,
                                 OrderCount = orderdata.CountOrder(order.Id, startdate, enddate),  //获取成功订购数
                                 Daoma_OrderCount = orderdata.Daoma_CountOrder(order.Id, startdate, enddate),  //获取倒码成功订购数
                                 Xiaofei_price = B2bOrderData.Xiaofei_price(order.Id, int.Parse(comid), startdate, enddate),//已消费
                                 Daoma_Xiaofei_price = B2bOrderData.Daoma_Xiaofei_price(order.Id, int.Parse(comid), startdate, enddate),//已消费
                                 Chendian_price = B2bOrderData.Chendian_price(order.Id, int.Parse(comid), startdate, enddate),//沉淀
                                 Tuipiao = B2bOrderData.Tuipiao_price(order.Id, int.Parse(comid), startdate, enddate),//沉淀
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
        #region  得到销售汇总
        public static string GetTotalDate(string comid, string startdate, string enddate)
        {
            using (var helper = new SqlHelper())
            {

                try
                {
                    var orderdata = new B2bOrderData();

                    var totaldate = orderdata.GetTotaldate(comid, startdate, enddate);//得到销售汇总数据
                    return JsonConvert.SerializeObject(new { type = 100, msg = totaldate });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                }
            }
        }
        #endregion


        #region  补发送短信
        public static string SendTicketSms(string comid, int oid)
        {
            using (var helper = new SqlHelper())
            {

                try
                {
                    var sendeticketdate = new SendEticketData();
                    var vasmodel = sendeticketdate.SendEticket(oid, 1);//发送此订单电子码

                    if (vasmodel == "OK")
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = vasmodel });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = vasmodel });
                    }

                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                }
            }
        }
        #endregion

        #region  重发短信
        public static string RestTicketSms(string comid, int oid)
        {
            using (var helper = new SqlHelper())
            {

                try
                {
                    var sendeticketdate = new SendEticketData();
                    var vasmodel = sendeticketdate.SendEticket(oid, 2);//重发电子码

                    if (vasmodel == "OK")
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = vasmodel });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = vasmodel });
                    }
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                }
            }
        }
        #endregion

        
    #region  标注过期
        public static string guoqi_biaozhu(string comid, int oid)
        {
            using (var helper = new SqlHelper())
            {

                try
                {

                    var order1 = new B2bOrderData().guoqi_biaozhu(comid, oid);
                    return JsonConvert.SerializeObject(new { type = 100, msg = order1 });
                   
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                }
            }
        }
        #endregion

        #region  分销倒码确认生成电子码
        public static string CreateDaoma(int comid, int id, int proid, int confirmstate, string beizhu)
        {

            using (var helper = new SqlHelper())
            {
                var data = 0;
                int Order_state = 0;
                try
                {
                    B2bOrderData dataorder = new B2bOrderData();
                    B2bEticketData dataeticket = new B2bEticketData();
                    //读取产品
                    B2b_com_pro pro = new B2bComProData().GetProById(proid.ToString());

                    //读取订单
                    B2b_order oldorder = new B2bOrderData().GetOrderById(id);

                    Order_state = oldorder.Order_state;

                    //处理订单
                    oldorder.Pay_state = 2;
                    oldorder.Order_state = 4;
                    //修改此订单的支付状态为“直接处理成功” ,订单状态为“已完成、已发码”
                    data = dataorder.InsertOrUpdate(oldorder);

                    //订单处理成功，元订单处理状态必须是1，待处理状态
                    if (data > 0 && Order_state == 1)
                    {
                        //生成电子码 
                        SendEticketData eticketdate = new SendEticketData();
                        var eticketinfo = eticketdate.CreateEticket(id, 2);
                        if (eticketinfo != "0" && eticketinfo != "")
                        {
                            return JsonConvert.SerializeObject(new { type = 100, msg = eticketinfo });
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = data });
                        }
                    }

                    if (data > 0)
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = data });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = data });
                    }
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                }
            }
        }
        #endregion

        #region  分销倒码确认生成电子码
        public static string GetExpressfee(int proid, string city, int num)
        {

            using (var helper = new SqlHelper())
            {
                string errmsg = "";
                int deliverytype = 2;
                try
                {
                    var fee = new B2b_delivery_costData().Getdeliverycost(proid, city, num, out errmsg, deliverytype);

                    if (errmsg == "")
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = fee });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = errmsg });
                    }

                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                }
            }
        }
        #endregion

        #region  确认并填写快递单号
        public static string ConfirExpress(int comid, int id, string expresscom, string expresscode, string expresstext)
        {

            using (var helper = new SqlHelper())
            {
                int order_state = 0;
                try
                {
                    var orderinfo = new B2bOrderData().GetOrderById(id);

                    if (orderinfo != null)
                    {
                        if (orderinfo.Order_state == 2)
                        {
                            order_state = 4;
                        }
                    }


                    var order1 = new B2bOrderData().ConfirExpress(comid, id, expresscom, expresscode, order_state, expresstext);



                    return JsonConvert.SerializeObject(new { type = 100, msg = order1 });


                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                }
            }
        }
        #endregion



        #region 根据微信号得到用户的订单列表
        public static string ConsumerOrderPageList(string openid, int pageindex, int pagesize, int accountid, int servertype = 0, int channelid = 0, string startime = "", string endtime = "")
        {
            var totalcount = 0;
            try
            {
                if (openid == "" && accountid == 0 && channelid == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, totalCount = 0, msg = "openid没有值" });
                }
                else
                {

                    var orderdata = new B2bOrderData();

                    var prodata = new B2bComProData();

                    SendEticketData sendate = new SendEticketData();

                    var list = orderdata.ConsumerOrderPageList(openid, pageindex, pagesize, accountid, out totalcount, servertype, channelid, startime, endtime);
                    IEnumerable result = "";
                    if (list != null)

                        result = from order in list
                                 select new
                                 {
                                     Id = order.Id,
                                     ProId = order.Pro_id,
                                     Proname = prodata.GetProById(order.Pro_id.ToString(), order.Speciid, order.channelcoachid).Pro_name,
                                     U_name = order.U_name,
                                     Comid = order.Comid,
                                     //U_phone = order.U_phone,
                                     U_num = order.U_num,
                                     //U_subdate = order.U_subdate,
                                     Imgurl = FileSerivce.GetImgUrl(prodata.GetProById(order.Pro_id.ToString()).Imgurl),
                                     Pay_price = order.Pay_price,
                                     sumprice = order.Pay_price * order.U_num - order.Integral1 + order.Express,
                                     //Profit = (order.Profit) * (order.U_num),
                                     Order_state = order.Order_state,
                                     Order_state_str = order.Order_state == 4 ? sendate.HuoQuEticket(order.Id) : "--",
                                     //Order_state_str = EnumUtils.GetName((OrderStatus)order.Order_state),
                                     Pay_state = order.Pay_state == (int)PayStatus.HasPay ? "已支付" : "未支付",
                                     Send_state = order.Send_state,
                                     ProValid = prodata.GetProById(order.Pro_id.ToString()).Pro_end.ToString("yyyy-MM-dd"),//产品有效期
                                     ConsumerLogo = Getcomlogobyproid(order.Pro_id),//产品商家logo
                                     Server_type = prodata.GetProById(order.Pro_id.ToString()).Server_type,//产品类型
                                     ConsumerCompanyName = Getcompanynamebyproid(order.Pro_id),

                                     Expresscom = order.Expresscom,
                                     Expresscode = order.Expresscode,
                                     pingjiastate = orderdata.Evaluateyesno(order.Id),
                                     U_traveldate = order.U_traveldate.ToString("yyyy-MM-dd HH:mm"),
                                     //Ordercome = order.Ordercome,
                                     //Totalcount = (order.Pay_price) * (order.U_num),
                                     //Face_price = prodata.GetProById(order.Pro_id.ToString()).Face_price
                                     md5 = EncryptionHelper.ToMD5(order.Id.ToString() + "lixh1210", "UTF-8"),
                                 };

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



        #region 根据账户得到用户指定订单
        public static string ConsumerOrderbyorderid(int comid, int orderid, int accountid)
        {
            var totalcount = 0;
            try
            {
                if ( accountid == 0 )
                {
                    return JsonConvert.SerializeObject(new { type = 1, totalCount = 0, msg = "账户信息错误" });
                }
                else
                {

                    var orderdata = new B2bOrderData();

                    var prodata = new B2bComProData();

                    SendEticketData sendate = new SendEticketData();

                    var list = orderdata.GetOrderById(orderid);
                    if (list.Comid != comid || comid ==0)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "商户不匹配" });
                    }
                    if (list.U_id != accountid || accountid == 0)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "登陆账户不匹配" });
                    }

                   // IEnumerable result = "";
                    if (list != null)
                    {
                        B2b_order result = new B2b_order
                        {
                            Id = list.Id,
                            Pro_id = list.Pro_id,
                            Pro_name = prodata.GetProById(list.Pro_id.ToString(), list.Speciid, list.channelcoachid).Pro_name,
                            U_name = list.U_name,
                            Comid = list.Comid,
                            U_phone = list.U_phone,
                            U_num = list.U_num,
                            Pno="",
                            //U_subdate = order.U_subdate,
                            Imgurl = FileSerivce.GetImgUrl(prodata.GetProById(list.Pro_id.ToString()).Imgurl),
                            Pay_price = list.Pay_price,
                            sumprice = list.Pay_price * list.U_num - list.Integral1 + list.Express,
                            //Profit = (order.Profit) * (order.U_num),
                            Order_state = list.Order_state,
                            //Order_state_str = list.Order_state == 4 ? sendate.HuoQuEticket(list.Id) : "--",
                            //Pay_state_str = list.Pay_state == (int)PayStatus.HasPay ? "已支付" : "未支付",
                            Send_state = list.Send_state,
                            //ProValid = prodata.GetProById(list.Pro_id.ToString()).Pro_end.ToString("yyyy-MM-dd"),//产品有效期
                            //ConsumerLogo = Getcomlogobyproid(list.Pro_id),//产品商家logo
                            Server_type = prodata.GetProById(list.Pro_id.ToString()).Server_type,//产品类型

                            Expresscom = list.Expresscom,
                            Expresscode = list.Expresscode,
                            //U_traveldate = list.U_traveldate.ToString("yyyy-MM-dd HH:mm"),
                        };

                        return JsonConvert.SerializeObject(new { type = 100, msg = result });
                    }
                    else {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "未查询到订单" });
                    }
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        private static string Getcompanynamebyproid(int proid)
        {
            B2b_com_pro mpro = new B2bComProData().GetProById(proid.ToString());
            if (mpro == null)
            {
                return "";
            }
            else
            {
                int comid = mpro.Com_id;
                B2b_company mcompany = B2bCompanyData.GetCompany(comid);
                if (mcompany == null)
                {
                    return "";
                }
                else
                {
                    return mcompany.Com_name;
                }

            }
        }
       



        #region 获取预订单的 有电子票的 订单列表,此流程 为客户扫码确认使用流程
        public static string ClientOrderquerenPageList(string openid, int pageindex, int pagesize, int accountid, int servertype = 0)
        {
            var totalcount = 0;
            try
            {
                if (openid == "" && accountid == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, totalCount = 0, msg = "openid没有值" });
                }
                else
                {

                    var orderdata = new B2bOrderData();

                    var prodata = new B2bComProData();

                    SendEticketData sendate = new SendEticketData();

                    var list = orderdata.ClientOrderquerenPageList(openid, pageindex, pagesize, accountid, out totalcount, servertype);
                    IEnumerable result = "";
                    if (list != null)

                        result = from order in list
                                 select new
                                 {
                                     Id = order.Id,
                                     ProId = order.Pro_id,
                                     Proname = prodata.GetProById(order.Pro_id.ToString()).Pro_name,
                                     Comid = order.Comid,
                                     U_num = order.U_num,
                                     Imgurl = FileSerivce.GetImgUrl(prodata.GetProById(order.Pro_id.ToString()).Imgurl),
                                     Pay_price = order.Pay_price,
                                     sumprice = order.Pay_price * order.U_num - order.Integral1 + order.Express,
                                     Order_state = order.Order_state,
                                     Order_state_str = order.Order_state == 4 ? sendate.HuoQuEticket(order.Id) : "--",
                                     Pay_state = order.Pay_state == (int)PayStatus.HasPay ? "已支付" : "未支付",
                                     Send_state = order.Send_state,
                                     ProValid = prodata.GetProById(order.Pro_id.ToString()).Pro_end.ToString("yyyy-MM-dd"),//产品有效期
                                     Pno = order.Pno,
                                     Eticket = new B2bEticketData().GetEticketByOrderid(order.Id),
                                     Expresscom = order.Expresscom,
                                     Expresscode = order.Expresscode,
                                     U_phone = order.U_phone,
                                     U_name = order.U_name,
                                     U_traveldate = order.U_traveldate,
                                 };

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
        #region 根据产品id得到公司微信端logo
        public static string Getcomlogobyproid(int proid)
        {
            if (proid == 0)
            {
                return "";
            }
            else
            {
                B2b_com_pro pro = new B2bComProData().GetProById(proid.ToString());
                if (pro != null)
                {
                    B2b_company_saleset saleset = B2bCompanySaleSetData.GetDirectSellByComid(pro.Com_id.ToString());
                    if (saleset != null)
                    {
                        if (saleset.Smalllogo != null && saleset.Smalllogo != "")
                        {
                            return FileSerivce.GetImgUrl(saleset.Smalllogo.ConvertTo<int>(0));
                        }
                        else
                        {
                            return "";
                        }

                    }
                    else
                    {
                        return "";
                    }

                }
                else
                {
                    return "";

                }

            }
        }
        #endregion
        #region 根据订单id和微信号得到产品信息
        public static string GetProductByOrderId(int orderid, string openid, int Accountid = 0)
        {
            try
            {

                B2b_order order1 = new B2bOrderData().GetProductByOrderId(orderid, openid);

                if (order1 != null)
                {//对账户判断是否为同一个账户
                    B2bCrmData crmdata = new B2bCrmData();
                    var crminfo = crmdata.b2b_crmH5(openid, order1.Comid);
                    if (crminfo != null)
                    {
                        if (Accountid == 0 || Accountid != crminfo.Id)
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "账户不匹配" });
                        }
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "微信账户不匹配" });
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "订单信息错误" });
                }



                List<B2b_order> list = new List<B2b_order>();
                list.Add(order1);

                IEnumerable result = "";
                if (order1 != null)
                {
                    result = from order in list
                             select new
                             {
                                 Id = order.Id,
                                 Proname = new B2bComProData().GetProById(order.Pro_id.ToString()).Pro_name,
                                 U_num = order.U_num,
                                 Pay_state = order.Pay_state == (int)PayStatus.HasPay ? "已支付" : "未支付",
                                 Send_state = order.Send_state,
                                 ProValid = new B2bComProData().GetProById(order.Pro_id.ToString()).Pro_end.ToString("yyyy-MM-dd"),//产品有效期
                                 //ProValid = new B2bComProData().GetProById(order.Pro_id.ToString()).Pro_end.ToString("yyyy-MM-dd"),//产品有效期                              
                                 ConsumerLogo = Getcomlogobyproid(order.Pro_id),//产品商家logo
                                 ConsumerCompanyName = Getcompanynamebyproid(order.Pro_id),
                                 CompanyAddress = Getcompanyaddressbyproid(order.Pro_id),
                                 ServiceContain = new B2bComProData().GetProById(order.Pro_id.ToString()).Service_Contain,
                                 ServiceNotContain = new B2bComProData().GetProById(order.Pro_id.ToString()).Service_NotContain,
                                 Precautions = new B2bComProData().GetProById(order.Pro_id.ToString()).Precautions,//注意事项

                                 Order_state = order.Order_state,
                                 Deliverytype = order.Deliverytype,
                                 Expresscom = order.Expresscom,
                                 Expresscode = order.Expresscode,
                                 Backtickettime = order.Backtickettime,
                                 U_subdate = order.U_subdate,


                             };
                }


                return JsonConvert.SerializeObject(new { type = 100, msg = result });

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }

        private static string Getcompanyaddressbyproid(int proid)
        {
            B2b_com_pro mpro = new B2bComProData().GetProById(proid.ToString());
            if (mpro == null)
            {
                return "";
            }
            else
            {
                int comid = mpro.Com_id;

                B2b_company_info mcompany = new B2bCompanyInfoData().GetCompanyInfo(comid);
                if (mcompany == null)
                {
                    return "";
                }
                else
                {
                    return mcompany.Com_add;
                }

            }
        }
        #endregion

        #region 网站上积分和预付款
        public static string B2bcrmreader(int comid, int uid)
        {
            using (var helper = new SqlHelper())
            {

                try
                {
                    var orderdata = new B2bCrmData();

                    var totaldate = orderdata.Readuser(uid, comid);
                    return JsonConvert.SerializeObject(new { type = 100, msg = totaldate });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                }
            }
        }
        #endregion

        #region 提交酒店订单
        public static string CreateHotelOrder(int reservationid, int comid, string name, string phone, string title, int number, int wxmaterialid, DateTime checkindate, DateTime checkoutdate, int roomtypeid, decimal totalprice, string lastercheckintime, int uid = 0)
        {
            Reservation model = new Reservation()
            {
                Id = reservationid,
                Comid = comid,
                Name = name,
                Phone = decimal.Parse(phone),
                Titile = title,
                Number = number,
                Submit_name = "",
                Rstatic1 = 1,//默认1，未处理
                Rdate1 = DateTime.Now,//默认当前时间
                Ip = CommonFunc.GetRealIP(),
                WxMaterialid = 0,
                Resdate = DateTime.Parse(checkindate.ToString("yyyy-MM-dd")),
                Checkoutdate = DateTime.Parse(checkoutdate.ToString("yyyy-MM-dd")),
                Roomtypeid = roomtypeid,
                Totalprice = totalprice,
                Lastercheckintime = lastercheckintime,
                Subdate = DateTime.Now,
                Uid = uid
            };

            //B2b_com_roomtype roomtype = new B2b_com_roomtypeData().GetRoomType(roomtypeid, comid);
            B2b_com_housetype roomtype = new B2b_com_housetypeData().GetHouseType(roomtypeid, comid);
            try
            {
                int effectnum = new WxMaterialData().insert_Res(model);
                if (effectnum != 0)
                {
                    TimeSpan ts = DateTime.Parse(checkoutdate.ToString("yyyy-MM-dd")).Subtract(DateTime.Parse(checkindate.ToString("yyyy-MM-dd")));
                    Smsmodel smodel = new Smsmodel()
                    {
                        Phone = roomtype.RecerceSMSPhone,
                        Name = name,
                        Title = title,
                        Money = totalprice,
                        Key = "微信酒店预订服务商通知短信",
                        Comid = comid,
                        Num = number,
                        Num1 = ts.Days,
                        Starttime = checkindate,
                        Endtime = checkoutdate,

                    };
                    Smsmodel kmodel = new Smsmodel()
                    {
                        Phone = phone.ToString(),
                        //Name = name,
                        //Title = title,
                        //Money = totalprice,
                        Key = "预订酒店短信",
                        Comid = comid

                    };
                    ////向客户发送预定短信
                    //SendSmsHelper.GetMember_sms(phone.ToString(), name, number.ToString(), title, totalprice, "预订短信", comid);
                    SendSmsHelper.Member_smsBal(kmodel);
                    //向酒店负责人发送客人的预定信息
                    //SendSmsHelper.GetMember_sms(phone.ToString(), name, number.ToString(), title, totalprice, "微信酒店预订服务商通知短信", comid);
                    SendSmsHelper.Member_smsBal(smodel);
                    //roomtype.RecerceSMSPhone
                    return JsonConvert.SerializeObject(new { type = 100, msg = "预定成功" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
                throw;

            }
        }
        #endregion


        #region 网站上积分和预付款
        public static string CoopOrderCount(int comid, string ordercome)
        {
            using (var helper = new SqlHelper())
            {
                int Allnum = 0;
                int Todaynum = 0;
                int Yesterdaynum = 0;
                int Transactionnum = 0;


                try
                {
                    var orderdata = new B2bOrderData();

                    var totaldate = orderdata.CoopOrderCount(comid, ordercome, out Allnum, out Todaynum, out Yesterdaynum, out Transactionnum);//得到销售汇总数据
                    return JsonConvert.SerializeObject(new { type = 100, msg = totaldate, Allnum = Allnum, Todaynum = Todaynum, Yesterdaynum = Yesterdaynum, Transactionnum = Transactionnum });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                }
            }
        }
        #endregion



        public static string QuitOrder(int comid, int orderid, int proid, int num, string testpro)
        {
            //判断是否有订单 绑定传入的订单
            B2b_order d_loldorder = new B2bOrderData().GetOldorderBybindingId(orderid);
            if (d_loldorder != null)
            {
                //try
                //{
                //    string comname = new B2bCompanyData().GetCompanyNameById(d_loldorder.Comid);
                //    return JsonConvert.SerializeObject(new { type = 1, msg = "当前订单为联动订单，请到直接售卖产品的商户(" + comname + ")发起退票" });
                comid = d_loldorder.Comid;
                orderid = d_loldorder.Id;
                proid = d_loldorder.Pro_id;
                //}
                //catch
                //{
                //    return JsonConvert.SerializeObject(new { type = 1, msg = "当前订单为联动订单，请到直接售卖产品的商户(" + d_loldorder.Comid + ")发起退票" });
                //}
            }

            B2b_order oldorder = new B2bOrderData().GetOrderById(orderid);
            if (oldorder == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "订单不存在" });
            }
            #region 如果是一码一验(即一单多个码)的提示 "当前订单为一单多码订单,请到 退订/订单状态 中按电子码退票"
            if (oldorder.yanzheng_method == 1)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "当前订单为一单多码订单,请到 退订/订单状态 中按电子码退票" });
            }
            #endregion
            B2b_com_pro pro = new B2b_com_pro();
            string data = "";
            int backorderstate = 0;//退票处理状态 1=成功 
            if (oldorder != null && proid != 0)
            {
                pro = new B2bComProData().GetProById(proid.ToString());
                if (pro == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "订单中产品不存在" });
                }

                #region 退票条件验证
                //服务类型是旅游大巴  已经截团 或者 不满足退票条件(乘车日期[不包括乘车日期]前退票)，则不再可以退票
                if (pro.Server_type == 10)
                {
                    int isjietuan = new Travelbusorder_operlogData().Ishasplanbus(proid, oldorder.U_traveldate);
                    if (isjietuan > 0)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "旅游大巴产品截团后不可退票" });
                    }
                    if (DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) >= oldorder.U_traveldate)
                    {//爱军2017.11.24号要求，提前2小时，但要重新统计发车时间，暂时放开时间限制
                        return JsonConvert.SerializeObject(new { type = 1, msg = "旅游大巴产品 乘车日期前退票" });
                    }

                    ////旅游大巴订单 发车前12h 不可以退
                    //if (pro.firststationtime != "")
                    //{
                    //    DateTime daba_startTime = DateTime.Now;
                    //    DateTime daba_endTime = Convert.ToDateTime(oldorder.U_traveldate.ToString("yyyy-MM-dd ") + pro.firststationtime);
                    //    if ((daba_endTime - daba_startTime).TotalHours < 12)
                    //    {
                    //        return JsonConvert.SerializeObject(new { type = 1, msg = "发车前12小时不可退" });
                    //    }
                    //}
                }

                //得到产品退票机制:0有效期内退票；1有效期内/外 均可退票；2不可退票
                int QuitTicketMechanism = pro.QuitTicketMechanism;
                if (QuitTicketMechanism == 0)
                {
                    B2b_com_pro modelcompro = pro;
                    int prosourtype = modelcompro.Source_type;
                    if (prosourtype == 4)//分销导入产品; 
                    {
                        int old_proid = new B2bComProData().GetOldproidById(pro.Id.ToString());//绑定产品的原始编号
                        if (old_proid == 0)
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "获取产品原始编号失败" });
                        }
                        else
                        {
                            prosourtype = new B2bComProData().GetProSource_typeById(old_proid.ToString());
                        }
                    }
                    #region 产品有效期
                    //经过以上赋值prosourtype，只可能2个值:1系统自动生成码产品;2倒码产品
                    DateTime pro_start = modelcompro.Pro_start;
                    DateTime pro_end = modelcompro.Pro_end;
                    if (prosourtype == 2 || prosourtype == 3) //倒码产品 或者 外来接口导入产品 
                    { }
                    if (prosourtype == 1) //系统自动生成码产品
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
                    #endregion

                    if (pro_end >= DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
                    {
                    }
                    else
                    {

                        return JsonConvert.SerializeObject(new { type = 1, msg = "产品已过有效期,不可退票" });
                    }
                }
                //如果订单的支付成功 并且订单状态是2（支付成功）,16（超时订单），则需要绕过退款政策，进行退款
                //得到支付信息
                B2b_pay mmmpay = new B2bPayData().GetSUCCESSPayById(orderid);
                if (mmmpay != null)
                {
                    if (oldorder.Order_state == 2 || oldorder.Order_state == 16)
                    { }
                    else
                    {
                        if (QuitTicketMechanism == 2)
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "当前产品退票政策:不可退票" });
                        }
                    }
                }
                else
                {
                    if (QuitTicketMechanism == 2)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "当前产品退票政策:不可退票" });
                    }
                }


                if (proid != oldorder.Pro_id)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "产品不对应" });
                }
                if (num > oldorder.U_num)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "退票数量错误" });
                }
                if (num == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "退票数量不可为0" });
                }
                if (oldorder.Order_state == 8)//订单状态
                {
                    //return JsonConvert.SerializeObject(new { type = 1, msg = "电子票已经消费，不可退票" });
                }
                if (oldorder.Order_state == 1)//订单状态
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "订单未完成，不可退票" });
                }
                if (oldorder.Order_state == 2)//订单状态
                {
                    //return JsonConvert.SerializeObject(new { type = 1, msg = "订单发送电子码未完成，不可退票" });
                }
                if (num * oldorder.Pay_price <= 0) {
                    if (oldorder.yuyuepno == "")
                    {//如果是预约码预约的允许 0元退票
                        return JsonConvert.SerializeObject(new { type = 1, msg = "退票金额错误，退票失败，请重新操作" });
                    }

                }
                //退票，状态必须是已付款或已发码状态才能退票
                if (oldorder.Order_state != 4 && oldorder.Order_state != 2 && oldorder.Order_state != 23 && oldorder.Order_state != 8 && oldorder.Order_state != 22 && oldorder.Order_state != 20 && oldorder.Order_state != 17)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "订单状态出错，请检查，退票失败" });
                }
                #endregion

                #region  外来接口产品(目前只有美景联动、阳光)，退票数量条件验证
                if (pro.Source_type == 4)//分销导入产品
                {
                    var agentorder = new B2bOrderData().GetOrderById(oldorder.Bindingagentorderid);
                    if (agentorder != null)
                    {
                        int serviceid = new B2bComProData().GetServiceidbyproid(agentorder.Pro_id);

                        #region 阳光产品，查询退票数量是否相符
                        if (serviceid == 1)
                        {
                            try
                            {
                                string yg_result = new SunShineInter().query_order(agentorder.Service_order_num);

                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(yg_result);
                                XmlElement ygroot = doc.DocumentElement;

                                string req_seq = ygroot.SelectSingleNode("req_seq").InnerText;//请求流水号
                                string id = ygroot.SelectSingleNode("result/id").InnerText;//结果id 
                                string comment = ygroot.SelectSingleNode("result/comment").InnerText;// 结果描述   
                                //-----------------新增2 begin-------------------------//
                                if (id == "0000")//成功
                                {
                                    string ygbuy_num = ygroot.SelectSingleNode("order/buy_num").InnerText;//购买数量
                                    string ygspare_num = ygroot.SelectSingleNode("order/spare_num").InnerText;//剩余可使用数量   
                                    string yguse_num = ygroot.SelectSingleNode("order/use_num").InnerText;//交易数量
                                    string ygstart_validity_date = ygroot.SelectSingleNode("order/start_validity_date").InnerText;//开始有效期
                                    string ygend_validity_date = ygroot.SelectSingleNode("order/end_validity_date").InnerText;//结束有效期

                                    if (ygspare_num.ConvertTo<int>(0) != num)
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = "退票数量不符，实际可退票数:" + ygspare_num });
                                    }
                                }
                                else
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "查询阳光订单错误:" + id + "(" + comment + ")" });
                                }
                            }
                            catch(Exception ex) 
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "查询阳光订单失败:意外错误"+ex.Message });
                            }
                        }
                        #endregion
                        #region  美景联动产品(退票需要审核)，a.订单状态为17(申请退票中),继续向下执行；b 订单状态不是17(申请退票中)，则修改订单状态为17，停止向下执行，需要等美景联动的退票通知才可继续向下执行
                        if (serviceid == 3)
                        {
                            //美景联动产品 分销外部接口退票，订单状态为17(申请退票中)，直接返回17(申请退票中)不在向下进行 
                            if (serviceid == 3 && agentorder.Order_state == 17)
                            {
                                if (testpro.Trim() == "分销外部接口退票")
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "申请退票中" });
                                }
                            }
                            if (serviceid == 3 && agentorder.Order_state != 17)
                            {
                                ApiService mapiservice = new ApiServiceData().GetApiservice(3);
                                if (mapiservice == null)
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "产品服务商信息查询失败!" });
                                }

                                string MjldinsureNo = new Api_mjld_SubmitOrder_outputData().GetMjldinsureNo(oldorder.Bindingagentorderid);
                                if (MjldinsureNo != "")
                                {
                                    #region 美景联动查询接口，判断退票数量是否在可退票范围内
                                    try
                                    {
                                        //2.6、	订单浏览 
                                        string mjld_GetCodeInforesult = new MjldInter().GetOrderDetail(mapiservice, MjldinsureNo);
                                        if (mjld_GetCodeInforesult != "")
                                        {
                                            XmlDocument doc = new XmlDocument();
                                            doc.LoadXml(mjld_GetCodeInforesult);
                                            XmlElement root = doc.DocumentElement;

                                            string endTime = root.SelectSingleNode("endTime").InnerText;//有效期
                                            string inCount = root.SelectSingleNode("inCount").InnerText;//总人数
                                            string usedCount = root.SelectSingleNode("usedCount").InnerText;//已使用人数
                                            string backCount = root.SelectSingleNode("backCount").InnerText;//退票人数
                                            string status = root.SelectSingleNode("status").InnerText;//状态



                                            int ketuiNum = inCount.ConvertTo<int>(0) - usedCount.ConvertTo<int>(0) - backCount.ConvertTo<int>(0);
                                            if (ketuiNum != num)
                                            {
                                                return JsonConvert.SerializeObject(new { type = 1, msg = "退票数量不符，实际可退票数:" + ketuiNum });
                                            }
                                        }


                                    }
                                    catch { }

                                    #endregion

                                    #region 美景联动提交退票申请接口
                                    try
                                    {
                                        //美景联动接口退票日志
                                        Api_mjld_RefundByOrderID mrefund = new Api_mjld_RefundByOrderID
                                        {
                                            id = 0,
                                            timeStamp = CommonFunc.ConvertDateTimeInt(DateTime.Now).ToString(),
                                            user = mapiservice.Servicername,
                                            password = mapiservice.Password,
                                            RefundPart = "true",
                                            orderid = oldorder.Bindingagentorderid,
                                            mjldorderId = MjldinsureNo,
                                            status = -1,
                                            rtimeStamp = "",
                                            scredenceno = "",
                                            fcredenceno = "",
                                            backCount = 0,
                                        };

                                        string cancelresult = new MjldInter().RefundByOrderID(mapiservice, mrefund);
                                        if (cancelresult != "")
                                        {
                                            XmlDocument doc = new XmlDocument();
                                            doc.LoadXml(cancelresult);
                                            XmlElement root = doc.DocumentElement;

                                            string status = root.SelectSingleNode("status").InnerText;//状态
                                            string scredenceno = root.SelectSingleNode("scredenceno").InnerText;//成功票码
                                            string fcredenceno = root.SelectSingleNode("fcredenceno").InnerText;//失败票码
                                            string backCount = root.SelectSingleNode("backCount").InnerText;//退票成功人数

                                            //美景联动接口退票日志
                                            mrefund.status = status.ConvertTo<int>(0);
                                            mrefund.rtimeStamp = mrefund.timeStamp;
                                            mrefund.scredenceno = scredenceno;
                                            mrefund.fcredenceno = fcredenceno;
                                            mrefund.backCount = backCount.ConvertTo<int>(0);
                                            int mrefundlog = new Api_mjld_RefundByOrderIDData().Editmjldrefundlog(mrefund);

                                            if (status == "0")//提交退票申请成功
                                            {
                                                //把订单原状态 记录在订单备注中 
                                                new B2bOrderData().UpOrderStateAndRemark(oldorder.Id, (int)OrderStatus.WaitQuitOrder, oldorder.Order_state.ToString());

                                                new B2bOrderData().UpOrderStateAndRemark(agentorder.Id, (int)OrderStatus.WaitQuitOrder, agentorder.Order_state.ToString());


                                                return JsonConvert.SerializeObject(new { type = 100, msg = "申请退票中" });
                                            }
                                            else //退票失败
                                            {
                                                return JsonConvert.SerializeObject(new { type = 1, msg = "美景联动接口退票失败" });
                                            }
                                        }
                                        else
                                        {
                                            return JsonConvert.SerializeObject(new { type = 1, msg = "美景联动接口退票失败!" });
                                        }
                                    }
                                    catch
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = "美景联动接口退票失败!" });
                                    }

                                    #endregion
                                }

                            }
                        }
                        #endregion
                    }
                }
                if (pro.Source_type == 3)//外部接口产品
                {
                    #region 阳光产品，查询退票数量是否相符
                    if (pro.Serviceid == 1)
                    {
                        try
                        {
                            string yg_result = new SunShineInter().query_order(oldorder.Service_order_num);

                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(yg_result);
                            XmlElement ygroot = doc.DocumentElement;

                            string req_seq = ygroot.SelectSingleNode("req_seq").InnerText;//请求流水号
                            string id = ygroot.SelectSingleNode("result/id").InnerText;//结果id 
                            string comment = ygroot.SelectSingleNode("result/comment").InnerText;// 结果描述   
                            //-----------------新增2 begin-------------------------//
                            if (id == "0000")//成功
                            {
                                string ygbuy_num = ygroot.SelectSingleNode("order/buy_num").InnerText;//购买数量
                                string ygspare_num = ygroot.SelectSingleNode("order/spare_num").InnerText;//剩余可使用数量   
                                string yguse_num = ygroot.SelectSingleNode("order/use_num").InnerText;//交易数量
                                string ygstart_validity_date = ygroot.SelectSingleNode("order/start_validity_date").InnerText;//开始有效期
                                string ygend_validity_date = ygroot.SelectSingleNode("order/end_validity_date").InnerText;//结束有效期

                                if (ygspare_num.ConvertTo<int>(0) != num)
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "退票数量不符，实际可退票数:" + ygspare_num });
                                }
                            }
                            else
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "查询阳光订单错误:" + id + "(" + comment + ")" });
                            }
                        }catch(Exception ex)
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "查询阳光订单失败:意外错误"+ex.Message });
                        }
                    }
                    #endregion

                    #region WL退票
                    if (pro.Serviceid == 4)
                    {
                        //try
                        //{
                           //查询万龙票是否符合退票情况，数量，状态
                            
                        //}
                        //catch (Exception ex)
                        //{
                        //    return JsonConvert.SerializeObject(new { type = 1, msg = "WL退单失败:意外错误" + ex.Message });
                        //}
                    }
                    #endregion

                    #region 美景联动产品 分销外部接口退票，订单状态为17(申请退票中)，直接返回17(申请退票中)不在向下进行
                    if (pro.Serviceid == 3)
                    {
                        //美景联动产品 分销外部接口退票，订单状态为17(申请退票中)，直接返回17(申请退票中)不在向下进行 
                        if (pro.Serviceid == 3 && oldorder.Order_state == 17)
                        {
                            if (testpro.Trim() == "分销外部接口退票")
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "申请退票中" });
                            }
                        }
                        if (pro.Serviceid == 3 && oldorder.Order_state != 17)
                        {
                            ApiService mapiservice = new ApiServiceData().GetApiservice(3);
                            if (mapiservice == null)
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "产品服务商信息查询失败!" });
                            }

                            string MjldinsureNo = new Api_mjld_SubmitOrder_outputData().GetMjldinsureNo(oldorder.Id);
                            if (MjldinsureNo != "")
                            {
                                #region 美景联动查询接口，判断退票数量是否在可退票范围内
                                try
                                {
                                    //2.6、	订单浏览 
                                    string mjld_GetCodeInforesult = new MjldInter().GetOrderDetail(mapiservice, MjldinsureNo);
                                    if (mjld_GetCodeInforesult != "")
                                    {
                                        XmlDocument doc = new XmlDocument();
                                        doc.LoadXml(mjld_GetCodeInforesult);
                                        XmlElement root = doc.DocumentElement;

                                        string endTime = root.SelectSingleNode("endTime").InnerText;//有效期
                                        string inCount = root.SelectSingleNode("inCount").InnerText;//总人数
                                        string usedCount = root.SelectSingleNode("usedCount").InnerText;//已使用人数
                                        string backCount = root.SelectSingleNode("backCount").InnerText;//退票人数
                                        string status = root.SelectSingleNode("status").InnerText;//状态

                                        int ketuiNum = inCount.ConvertTo<int>(0) - usedCount.ConvertTo<int>(0) - backCount.ConvertTo<int>(0);
                                        if (ketuiNum != num)
                                        {
                                            return JsonConvert.SerializeObject(new { type = 1, msg = "退票数量不符，实际可退票数:" + ketuiNum });
                                        }
                                    }


                                }
                                catch (Exception ex)
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "查询美景联动订单失败:意外错误:" + ex.Message });
                                }

                                #endregion

                                #region 美景联动提交退票申请接口
                                try
                                {
                                    //美景联动接口退票日志
                                    Api_mjld_RefundByOrderID mrefund = new Api_mjld_RefundByOrderID
                                    {
                                        id = 0,
                                        timeStamp = CommonFunc.ConvertDateTimeInt(DateTime.Now).ToString(),
                                        user = mapiservice.Servicername,
                                        password = mapiservice.Password,
                                        RefundPart = "true",
                                        orderid = oldorder.Id,
                                        mjldorderId = MjldinsureNo,
                                        status = -1,
                                        rtimeStamp = "",
                                        scredenceno = "",
                                        fcredenceno = "",
                                        backCount = 0,
                                    };

                                    string cancelresult = new MjldInter().RefundByOrderID(mapiservice, mrefund);
                                    if (cancelresult != "")
                                    {
                                        XmlDocument doc = new XmlDocument();
                                        doc.LoadXml(cancelresult);
                                        XmlElement root = doc.DocumentElement;

                                        string status = root.SelectSingleNode("status").InnerText;//状态
                                        string scredenceno = root.SelectSingleNode("scredenceno").InnerText;//成功票码
                                        string fcredenceno = root.SelectSingleNode("fcredenceno").InnerText;//失败票码
                                        string backCount = root.SelectSingleNode("backCount").InnerText;//退票成功人数

                                        //美景联动接口退票日志
                                        mrefund.status = status.ConvertTo<int>(0);
                                        mrefund.rtimeStamp = mrefund.timeStamp;
                                        mrefund.scredenceno = scredenceno;
                                        mrefund.fcredenceno = fcredenceno;
                                        mrefund.backCount = backCount.ConvertTo<int>(0);
                                        int mrefundlog = new Api_mjld_RefundByOrderIDData().Editmjldrefundlog(mrefund);

                                        if (status == "0")//提交退票申请成功
                                        {
                                            //把订单原状态 记录在订单备注中 
                                            new B2bOrderData().UpOrderStateAndRemark(oldorder.Id, (int)OrderStatus.WaitQuitOrder, oldorder.Order_state.ToString());

                                            return JsonConvert.SerializeObject(new { type = 100, msg = "申请退票中" });
                                        }
                                        else //退票失败
                                        {
                                            return JsonConvert.SerializeObject(new { type = 1, msg = "美景联动接口退票失败" });
                                        }
                                    }
                                    else
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = "美景联动接口退票失败!" });
                                    }
                                }
                                catch
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "美景联动接口退票失败:意外错误!" });
                                }

                                #endregion
                            }

                        }
                    }
                    #endregion
                }
                #endregion

                oldorder.Ticket = num * oldorder.Pay_price;
                oldorder.Ticketinfo = testpro;
                oldorder.Backtickettime = DateTime.Now;




                #region 4已发码；22已处理；8已消费(部分消费的);17(申请退票中)
                if (oldorder.Order_state == 4 || oldorder.Order_state == 22 || oldorder.Order_state == 8 || oldorder.Order_state == 17)//只对已发码或已处理，产品进行退票处理，未发码（只是支付了不处理），部分使用退票
                {
                    #region 产品来源,系统自动生成,直接完成退票
                    if (pro.Source_type == 1)
                    {

                        //判断实际电子票数量
                        var prodata = new B2bEticketData();
                        var eticketinfo = prodata.SelectOrderid(orderid);//通过订单号查询电子票是否存在
                        if (pro.Server_type != 9)
                        {//酒店的可不产生电子票
                            if (eticketinfo == null)
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "没有此电子票" });
                            }
                            //退票数量必须等于未使用数量
                            if (pro.pnonumperticket != 1)
                            {
                                if (num * pro.pnonumperticket != eticketinfo.Use_pnum)//如果一个电子票产生多个码时自动乘以数量，只有自己产生电子票会有此问题
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "退票数量错误" });
                                }
                            }
                            else
                            {
                                if (pro.pro_yanzheng_method == 1)
                                {//如果一个订单产生多个码，要对余下的码进行汇总
                                    int ketuipiaonum = 0;
                                    var SelectOrderid_list = prodata.SelectOrderid_list(orderid);//通过订单号查询电子票是否存在
                                    if (SelectOrderid_list != null) {
                                        for (int i = 0; i < SelectOrderid_list.Count; i++)
                                        {
                                            ketuipiaonum = ketuipiaonum + SelectOrderid_list[i].Use_pnum;
                                        }
                                    }

                                    if (ketuipiaonum != num)
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = "退票数量错误" });
                                    }

                                }
                                else
                                {

                                    if (num != eticketinfo.Use_pnum)
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = "退票数量错误" });
                                    }
                                }
                            }
                        }
                        else
                        {
                            //酒店的也判断下数量
                            //退票数量必须等于未使用数量
                            if (num != oldorder.U_num)
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "退票数量错误" });
                            }
                        }

                        #region 作废票处理
                        //订单状态为:订单退款
                        if (oldorder.Agentid == 0)
                        {//直销订单，标记为处理中做财务处理
                            oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                        }
                        else
                        {//分销订单直接退款
                            oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                        }

                        data = OrderJsonData.InsertRecharge(oldorder);
                        //电子码作废
                        var eticketback = prodata.Backticket_use_num(orderid);


                        //对预约码的预约的产品进行回滚
                        if (oldorder.yuyuepno != "")
                        {


                            if (eticketinfo != null)
                            {
                                if (eticketinfo.Use_pnum > 0)
                                {
                                    //对预约码的预约的产品进行回滚
                                    var yuyuepno_huigun = prodata.Backticket_yuyuepno_num(oldorder.yuyuepno, eticketinfo.Use_pnum);
                                    //把预约码的验票日志作废 
                                    B2b_eticket_log elog = new B2bEticketLogData().GetLasterYueyupnoElog(oldorder.yuyuepno, eticketinfo.Use_pnum); 
                                    elog.A_state = (int)ECodeOperStatus.OperFail;
                                    int result3 = new B2bEticketLogData().InsertOrUpdateLog(elog);
                                }
                            }
                        }

                        //分销财务处理
                        if (oldorder.Agentid != 0)
                        {
                            backorderstate = 1;
                        }

                        #endregion

                    }
                    #endregion
                    #region 产品来源,从外部倒码
                    else if (pro.Source_type == 2)
                    {//倒码产品，本系统不做验证，分销（及客户哪没有退票口），商户退票时，直接退款。（请自行验证并作废）
                        #region  退款处理
                        if (oldorder.Agentid == 0)
                        { //直销订单，标记为处理中做财务处理
                            oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                        }
                        else
                        { //分销订单直接退款
                            oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                        }
                        data = OrderJsonData.InsertRecharge(oldorder);
                        #endregion

                    }
                    #endregion
                    #region 产品来源,外部接口产品(现阶段只有阳光、慧择网、美景联动)
                    else if (pro.Source_type == 3)
                    {
                        if (pro.Serviceid == 1)
                        {
                            #region 阳光接口退票
                            string ygordernum = new Api_yg_addorder_outputData().GetApi_yg_ordernum(oldorder.Id);
                            if (ygordernum.Trim() != "")//生成了阳光订单，如果没有生成则只是退系统的票，不退阳光的票
                            {
                                try
                                {
                                    ApiService mapiservice = new ApiServiceData().GetApiservice(1);
                                    if (mapiservice == null)
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = "产品服务商信息查询失败!" });
                                    }

                                    Api_yg_cancelorder m_ygcancelorder = new Api_yg_cancelorder
                                    {
                                        id = 0,
                                        organization = mapiservice.Organization,
                                        password = mapiservice.Password,
                                        req_seq = mapiservice.Organization + DateTime.Now.ToString("yyyyMMddhhssmm") + CommonFunc.CreateNum(6),
                                        ygorder_num = ygordernum,
                                        num = num,
                                        rResultid = "",
                                        rResultComment = "",
                                        rygorder_num = "",
                                        rnum = 0,
                                        orderId = oldorder.Id,
                                        opertime = DateTime.Now
                                    };
                                    int api_yg_cancelorderId = new Api_yg_cancelorderData().EditApi_yg_cancelorder(m_ygcancelorder);
                                    m_ygcancelorder.id = api_yg_cancelorderId;

                                    string sendresult = new SunShineInter().cancel_order(mapiservice, m_ygcancelorder);

                                    XmlDocument doc = new XmlDocument();
                                    doc.LoadXml(sendresult);
                                    XmlElement root = doc.DocumentElement;

                                    string req_seq = root.SelectSingleNode("req_seq").InnerText;//请求流水号
                                    string id = root.SelectSingleNode("result/id").InnerText;//结果id 
                                    string comment = root.SelectSingleNode("result/comment").InnerText;// 结果描述 

                                    m_ygcancelorder.rResultid = id;
                                    m_ygcancelorder.rResultComment = comment;
                                    new Api_yg_cancelorderData().EditApi_yg_cancelorder(m_ygcancelorder);

                                    if (id == "0000")//退票成功
                                    {
                                        //订单状态为:订单退款
                                        if (oldorder.Agentid == 0)
                                        {//直销订单，标记为处理中做财务处理
                                            oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                                        }
                                        else
                                        {//分销订单直接退款
                                            oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                                        }

                                        oldorder.service_lastcount = oldorder.U_num - oldorder.service_usecount - num;
                                        oldorder.Order_remark += "阳光接口退票:" + id + "返回内容:" + comment;
                                        data = OrderJsonData.InsertRecharge(oldorder);

                                        string rygorder_num = root.SelectSingleNode("order/order_num").InnerText;//订单号
                                        string rnum = root.SelectSingleNode("order/num").InnerText;// 张数 
                                        m_ygcancelorder.rygorder_num = rygorder_num;
                                        m_ygcancelorder.rnum = rnum.ConvertTo<int>(0);
                                        new Api_yg_cancelorderData().EditApi_yg_cancelorder(m_ygcancelorder);
                                    }
                                    else //退票失败
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = comment });
                                    }
                                }
                                catch
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "阳光接口退票失败" });
                                }
                            }
                            else
                            {
                                //订单状态为:订单退款
                                if (oldorder.Agentid == 0)
                                {//直销订单，标记为处理中做财务处理
                                    oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                                }
                                else
                                {//分销订单直接退款
                                    oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                                }
                                oldorder.Order_remark += "阳光提单时失败，所以只需退本系统票就可";
                                data = OrderJsonData.InsertRecharge(oldorder);

                                return JsonConvert.SerializeObject(new { type = 1, msg = "阳光接口退票失败" });
                            }
                            #endregion
                        }
                        else if (pro.Serviceid == 2)
                        {
                            #region 慧择网保险接口退票
                            //根据订单号得到投保单号
                            string insureNo = new Api_hzins_OrderApplyResp_OrderExtData().GetinsureNumbyorderid(orderid);//投保单号
                            if (insureNo != "")
                            {
                                string cancelresult = new HzinsInter().Hzins_orderCancel(orderid);
                                Hzins_OrderCancelResp mresult = (Hzins_OrderCancelResp)JsonConvert.DeserializeObject(cancelresult, typeof(Hzins_OrderCancelResp));
                                if (mresult != null)
                                {
                                    Api_hzins_orderCancel mApi_hzins_orderCancel = new Api_hzins_orderCancel
                                    {
                                        id = 0,
                                        orderid = orderid,
                                        insureNo = insureNo,
                                        respCode = mresult.respCode,
                                        respMsg = mresult.respMsg
                                    };
                                    new Api_hzins_orderCancelData().EditApi_hzins_orderCancel(mApi_hzins_orderCancel);

                                    if (mresult.respCode == 0)
                                    {
                                        //订单状态为:订单退款
                                        if (oldorder.Agentid == 0)
                                        {//直销订单，标记为处理中做财务处理
                                            oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                                        }
                                        else
                                        {//分销订单直接退款
                                            oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                                        }
                                        oldorder.Order_remark += "慧择网接口退票:" + mresult.respCode + "返回内容:" + mresult.respMsg;
                                        data = OrderJsonData.InsertRecharge(oldorder);
                                    }
                                    else
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = mresult.respCode + "(" + mresult.respMsg + ")" });
                                    }
                                }
                            }
                            else
                            {
                                //订单状态为:订单退款
                                if (oldorder.Agentid == 0)
                                {//直销订单，标记为处理中做财务处理
                                    oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                                }
                                else
                                {//分销订单直接退款
                                    oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                                }
                                oldorder.Order_remark += "慧择网接口出票失败，所以只需要退本系统单就可以";
                                data = OrderJsonData.InsertRecharge(oldorder);
                            }

                            #endregion
                        }
                        else if (pro.Serviceid == 3)
                        {
                            #region 美景联动接口退票需要审核：生成了接口订单上面已经处理过；没有生成则只需要退本系统票就可以
                            if (oldorder.Service_order_num != "")
                            {
                                //订单状态为:订单退款
                                if (oldorder.Agentid == 0)
                                {//直销订单，标记为处理中做财务处理
                                    oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                                }
                                else
                                {//分销订单直接退款
                                    oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                                }


                                oldorder.service_lastcount = oldorder.U_num - oldorder.service_usecount - num;
                                oldorder.Order_remark += "美景联动接口退票张数:" + num;
                                data = OrderJsonData.InsertRecharge(oldorder);
                            }
                            else
                            {
                                //订单状态为:订单退款
                                if (oldorder.Agentid == 0)
                                {//直销订单，标记为处理中做财务处理
                                    oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                                }
                                else
                                {//分销订单直接退款
                                    oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                                }
                                oldorder.Order_remark += "美景联动接口出票失败，所以只需要退本系统单就可以";
                                data = OrderJsonData.InsertRecharge(oldorder);
                            }
                            #endregion
                        }
                        else if (pro.Serviceid == 4) {

                                B2b_company commanage = B2bCompanyData.GetAllComMsg(comid);
                                WlGetProInfoDealRequestData wldata = new WlGetProInfoDealRequestData(commanage.B2bcompanyinfo.wl_PartnerId, commanage.B2bcompanyinfo.wl_userkey);

                                var WlOrderinfo = wldata.SearchWlOrderData(comid, 0, "", orderid);
                                if (WlOrderinfo == null)
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "wl退票出错，订单查询失败" });
                                }

                                if (WlOrderinfo.quantity - WlOrderinfo.usedQuantity != num)
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "wl退票数量不符" });
                                }
                                double refundPrice = WlOrderinfo.unitprice * num;

                                var Refundorder = wldata.wlOrderRefundRequest_json(int.Parse(commanage.B2bcompanyinfo.wl_PartnerId), WlOrderinfo.wlorderid, WlOrderinfo.wldealid, WlOrderinfo.orderid.ToString(), WlOrderinfo.partnerdealid.ToString(), WlOrderinfo.vouchers, 1, WlOrderinfo.unitprice, refundPrice, 0);//

                                var wlOrderRefund = wldata.wlOrderRefundRequest_data(Refundorder, comid, 7);

                                if (wlOrderRefund.IsSuccess == true)
                                {
                                    //订单状态为:订单退款
                                    if (oldorder.Agentid == 0)
                                    {//直销订单，标记为处理中做财务处理
                                        oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                                    }
                                    else
                                    {//分销订单直接退款
                                        oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                                    }

                                    oldorder.service_lastcount = 0;
                                    oldorder.Order_remark += "WL接口退票ID:" + WlOrderinfo.wlorderid + ":" + wlOrderRefund.Message;
                                    data = OrderJsonData.InsertRecharge(oldorder);
                                }
                                else {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "wl退票出错" + wlOrderRefund.Message });
                                }

                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "外部接口产品没有此产品" });

                        }

                    }
                    #endregion
                    #region 产品来源,分销系统导入产品
                    else if (pro.Source_type == 4)
                    {
                        //判断绑定产品是否为外部接口产品：是的话不再判断电子票信息
                        var agentorder = new B2bOrderData().GetOrderById(oldorder.Bindingagentorderid);
                        if (agentorder != null)
                        {
                            int sourcetype = new B2bComProData().GetSourcetypebyproid(agentorder.Pro_id);
                            if (sourcetype == 0)
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "原始产品出错，请联系管理员" });
                            }
                            if (sourcetype != 3)
                            {
                                //判断实际电子票数量
                                var prodata = new B2bEticketData();
                                var eticketinfo = prodata.SelectOrderid(oldorder.Bindingagentorderid);//通过订单号查询电子票是否存在
                                if (eticketinfo == null)
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "没有此电子票" });
                                }
                                //退票数量必须等于未使用数量
                                if (pro.Server_type != 9)
                                {
                                    if (num != eticketinfo.Use_pnum)
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = "不支持部分退票" });
                                    }
                                }
                                //电子码作废
                                var eticketback = prodata.Backticket_use_num(oldorder.Bindingagentorderid);
                            }
                        }
                        else {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "未查询到原始订单，请联系管理员" });
                        }


                        #region 作废票处理
                        //订单状态为:订单退款
                        if (oldorder.Agentid == 0)
                        {//直销订单，标记为处理中做财务处理
                            oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                        }
                        else
                        {//分销订单直接退款
                            oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                        }

                        data = OrderJsonData.InsertRecharge(oldorder);



                        #region--------------------对分销导入产品，对绑定分销账户的订单进行处理并退款-----------------------------
                        if (agentorder != null)
                        {
                            #region  外来接口产品
                            int serviceid = new B2bComProData().GetServiceidbyproid(agentorder.Pro_id);
                            if (serviceid == 1)
                            {
                                #region 阳光接口退票
                                string ygordernum = new Api_yg_addorder_outputData().GetApi_yg_ordernum(agentorder.Id);
                                if (ygordernum.Trim() != "")//生成了阳光订单，如果没有生成则只是退系统的票，不退阳光的票
                                {
                                    try
                                    {
                                        ApiService mapiservice = new ApiServiceData().GetApiservice(1);
                                        if (mapiservice == null)
                                        {
                                            return JsonConvert.SerializeObject(new { type = 1, msg = "产品服务商信息查询失败!" });
                                        }

                                        Api_yg_cancelorder m_ygcancelorder = new Api_yg_cancelorder
                                        {
                                            id = 0,
                                            organization = mapiservice.Organization,
                                            password = mapiservice.Password,
                                            req_seq = mapiservice.Organization + DateTime.Now.ToString("yyyyMMddhhssmm") + CommonFunc.CreateNum(6),
                                            ygorder_num = ygordernum,
                                            num = num,
                                            rResultid = "",
                                            rResultComment = "",
                                            rygorder_num = "",
                                            rnum = 0,
                                            orderId = agentorder.Id,
                                            opertime = DateTime.Now
                                        };
                                        int api_yg_cancelorderId = new Api_yg_cancelorderData().EditApi_yg_cancelorder(m_ygcancelorder);
                                        m_ygcancelorder.id = api_yg_cancelorderId;

                                        string sendresult = new SunShineInter().cancel_order(mapiservice, m_ygcancelorder);

                                        XmlDocument doc = new XmlDocument();
                                        doc.LoadXml(sendresult);
                                        XmlElement root = doc.DocumentElement;

                                        string req_seq = root.SelectSingleNode("req_seq").InnerText;//请求流水号
                                        string id = root.SelectSingleNode("result/id").InnerText;//结果id 
                                        string comment = root.SelectSingleNode("result/comment").InnerText;// 结果描述 

                                        m_ygcancelorder.rResultid = id;
                                        m_ygcancelorder.rResultComment = comment;
                                        new Api_yg_cancelorderData().EditApi_yg_cancelorder(m_ygcancelorder);

                                        if (id == "0000")//退票成功
                                        {
                                            agentorder.service_lastcount = agentorder.U_num - agentorder.service_usecount - num;
                                            agentorder.Order_remark += "阳光接口退票:" + id + "返回内容:" + comment;


                                            string rygorder_num = root.SelectSingleNode("order/order_num").InnerText;//订单号
                                            string rnum = root.SelectSingleNode("order/num").InnerText;// 张数 
                                            m_ygcancelorder.rygorder_num = rygorder_num;
                                            m_ygcancelorder.rnum = rnum.ConvertTo<int>(0);
                                            new Api_yg_cancelorderData().EditApi_yg_cancelorder(m_ygcancelorder);
                                        }
                                        else //退票失败
                                        {
                                            return JsonConvert.SerializeObject(new { type = 1, msg = comment });
                                        }
                                    }
                                    catch
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = "阳光接口退票失败" });
                                    }
                                }
                                else
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "阳光接口退票失败.." });
                                }
                                #endregion
                                #region 已注释
                                //#region 阳光接口退票
                                //if (agentorder.Service_order_num != "")
                                //{
                                //    string sendresult = new SunShineInter().cancel_order(agentorder.Service_order_num, num.ToString());

                                //    XmlDocument doc = new XmlDocument();
                                //    doc.LoadXml(sendresult);
                                //    XmlElement root = doc.DocumentElement;

                                //    string req_seq = root.SelectSingleNode("req_seq").InnerText;//请求流水号
                                //    string id = root.SelectSingleNode("result/id").InnerText;//结果id 
                                //    string comment = root.SelectSingleNode("result/comment").InnerText;// 结果描述  
                                //    if (id == "0000")//退票成功
                                //    {
                                //        ////订单状态为:订单退款
                                //        //if (agentorder.Agentid == 0)
                                //        //{//直销订单，标记为处理中做财务处理
                                //        //    agentorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                                //        //}
                                //        //else
                                //        //{//分销订单直接退款
                                //        //    agentorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                                //        //}
                                //        agentorder.Order_remark += "阳光接口退票成功:" + id + "返回内容:" + comment;
                                //        //data = OrderJsonData.InsertRecharge(agentorder);

                                //    }
                                //    else //退票失败
                                //    {
                                //        return JsonConvert.SerializeObject(new { type = 1, msg = comment });
                                //    }
                                //}
                                //#endregion
                                #endregion
                            }
                            else if (serviceid == 2)
                            {
                                #region 根据订单号得到投保单号,判断是否为对外接口产品(慧择网订单)
                                string insureNo = new Api_hzins_OrderApplyResp_OrderExtData().GetinsureNumbyorderid(oldorder.Bindingagentorderid);//投保单号
                                if (insureNo != "")
                                {
                                    #region 慧择网保险产品退票
                                    string cancelresult = new HzinsInter().Hzins_orderCancel(oldorder.Bindingagentorderid);
                                    Hzins_OrderCancelResp mresult = (Hzins_OrderCancelResp)JsonConvert.DeserializeObject(cancelresult, typeof(Hzins_OrderCancelResp));
                                    if (mresult != null)
                                    {
                                        Api_hzins_orderCancel mApi_hzins_orderCancel = new Api_hzins_orderCancel
                                        {
                                            id = 0,
                                            orderid = oldorder.Bindingagentorderid,
                                            insureNo = insureNo,
                                            respCode = mresult.respCode,
                                            respMsg = mresult.respMsg
                                        };
                                        new Api_hzins_orderCancelData().EditApi_hzins_orderCancel(mApi_hzins_orderCancel);

                                        if (mresult.respCode == 0)
                                        {
                                            //订单状态为:订单退款
                                            //agentorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款

                                            agentorder.Order_remark += "慧择网接口退票成功:" + mresult.respCode + "返回内容:" + mresult.respMsg;
                                            //data = OrderJsonData.InsertRecharge(agentorder);
                                        }
                                        else
                                        {
                                            return JsonConvert.SerializeObject(new { type = 1, msg = mresult.respCode + "(" + mresult.respMsg + ")" });
                                        }
                                    }

                                    #endregion
                                }
                                #endregion
                            }
                            else if (serviceid == 3)
                            {
                                #region 根据订单号得到美景联动订单号，判断是否是对外接口产品(美景联动订单)
                                if (agentorder.Service_order_num != "")
                                {
                                    agentorder.Order_remark += "美景联动接口退票张数:" + num;
                                    agentorder.service_lastcount = agentorder.U_num - agentorder.service_usecount - num;
                                }
                                else
                                {
                                    agentorder.Order_remark += "美景联动接口出票失败，所以只需要退本系统单就可以";

                                }
                                #endregion
                            }
                            #endregion

                            agentorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                            var agentorderup = OrderJsonData.InsertRecharge(agentorder);

                            //读取分销商信息
                            Agent_company agentinfo = AgentCompanyData.GetAgentWarrant(agentorder.Agentid, agentorder.Comid);
                            if (agentinfo == null)
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "绑定的分销账户授权信息错误，退票操作部分失败，请手工完成后续操作" });
                            }
                            else
                            {
                                if (agentorder.Warrant_type == 1)
                                {
                                    //计算分销余额
                                    decimal overmoney = agentinfo.Imprest + num * agentorder.Pay_price;

                                    //分销商财务扣款
                                    Agent_Financial Financialinfo = new Agent_Financial
                                    {
                                        Id = 0,
                                        Com_id = agentorder.Comid,
                                        Agentid = agentorder.Agentid,
                                        Warrantid = agentorder.Warrantid,
                                        Order_id = agentorder.Id,
                                        Servicesname = pro.Pro_name + "[" + oldorder.Bindingagentorderid + "]",
                                        SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                                        Money = num * agentorder.Pay_price,
                                        Payment = 0,            //收支(0=收款,1=支出)
                                        Payment_type = "退票退款",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点,分销扣款）
                                        Money_come = "分销账户",         //资金来源（网上支付,银行收款，分销账户等）
                                        Over_money = overmoney
                                    };
                                    var fin = AgentCompanyData.AgentFinancial(Financialinfo);

                                    //退分销订单手续费
                                    var HuigunShouxufei_temp = OrderJsonData.HuigunShouxufei(agentorder, num * agentorder.Pay_price);

                                }

                            }
                        }
                        #endregion


                        //分销财务处理
                        if (oldorder.Agentid != 0)
                        {
                            backorderstate = 1;
                        }

                        #endregion

                    }
                    #endregion
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "产品来源不对" });
                    }


                }
                #endregion
                #region 2已付款 ;23超时订单已取消；20发码失败
                else if (oldorder.Order_state == 2 || oldorder.Order_state == 23 || oldorder.Order_state == 20)//对只支付，未发码的(和超时订单，只有抢购才有超时订单状态23)退款,
                {
                    //对于实物订单，提单支付成功后订单状态为 “已付款”，此时订单产生了电子码(商户处理后订单状态才为"已发码");对于其他产品不保证产生了电子码；

                    //判断订单是否有电子码产生：有，则需要作废电子码；无 ，则不需要作废电子码的操作；
                    #region 产品来源,系统自动生成,直接完成退票
                    if (pro.Source_type == 1)
                    {
                        //判断实际电子票数量
                        var prodata = new B2bEticketData();
                        var eticketinfo = prodata.SelectOrderid(orderid);//通过订单号查询电子票是否存在
                        if (eticketinfo == null)
                        {
                            //return JsonConvert.SerializeObject(new { type = 1, msg = "没有此电子票" });
                        }
                        else
                        {
                            //退票数量必须等于未使用数量
                            if (pro.pro_yanzheng_method == 1)
                            {//如果一个订单产生多个码，要对余下的码进行汇总
                                int ketuipiaonum = 0;
                                var SelectOrderid_list = prodata.SelectOrderid_list(orderid);//通过订单号查询电子票是否存在
                                if (SelectOrderid_list != null)
                                {
                                    for (int i = 0; i < SelectOrderid_list.Count; i++)
                                    {
                                        ketuipiaonum = ketuipiaonum + SelectOrderid_list[i].Use_pnum;
                                    }
                                }

                                if (ketuipiaonum != num)
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "退票数量错误" });
                                }

                            }
                            else
                            {

                                if (num != eticketinfo.Use_pnum)
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "退票数量错误" });
                                }
                            }
                        }

                        #region 作废票处理
                        //订单状态为:订单退款
                        if (oldorder.Agentid == 0)
                        {//直销订单，标记为处理中做财务处理
                            oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                        }
                        else
                        {//分销订单直接退款
                            oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                        }

                        data = OrderJsonData.InsertRecharge(oldorder);
                        if (eticketinfo != null)
                        {
                            //电子码作废
                            var eticketback = prodata.Backticket_use_num(orderid);
                        }

                        //分销财务处理
                        if (oldorder.Agentid != 0)
                        {
                            backorderstate = 1;
                        }

                        #endregion

                    }
                    #endregion
                    #region 产品来源,从外部倒码
                    else if (pro.Source_type == 2)
                    {//倒码产品，本系统不做验证，分销（及客户哪没有退票口），商户退票时，直接退款。（请自行验证并作废）
                        #region  退款处理
                        if (oldorder.Agentid == 0)
                        { //直销订单，标记为处理中做财务处理
                            oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                        }
                        else
                        { //分销订单直接退款
                            oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                        }
                        data = OrderJsonData.InsertRecharge(oldorder);
                        #endregion

                    }
                    #endregion
                    #region 产品来源,外部接口产品(现阶段只有阳光、慧择网;美景联动产品)
                    else if (pro.Source_type == 3)
                    {
                        if (pro.Serviceid == 1)
                        {
                            #region 阳光接口退票
                            string ygordernum = new Api_yg_addorder_outputData().GetApi_yg_ordernum(oldorder.Id);
                            if (ygordernum.Trim() != "")//生成了阳光订单，如果没有生成则只是退系统的票，不退阳光的票
                            {
                                try
                                {
                                    ApiService mapiservice = new ApiServiceData().GetApiservice(1);
                                    if (mapiservice == null)
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = "产品服务商信息查询失败!" });
                                    }

                                    Api_yg_cancelorder m_ygcancelorder = new Api_yg_cancelorder
                                    {
                                        id = 0,
                                        organization = mapiservice.Organization,
                                        password = mapiservice.Password,
                                        req_seq = mapiservice.Organization + DateTime.Now.ToString("yyyyMMddhhssmm") + CommonFunc.CreateNum(6),
                                        ygorder_num = ygordernum,
                                        num = num,
                                        rResultid = "",
                                        rResultComment = "",
                                        rygorder_num = "",
                                        rnum = 0,
                                        orderId = oldorder.Id,
                                        opertime = DateTime.Now
                                    };
                                    int api_yg_cancelorderId = new Api_yg_cancelorderData().EditApi_yg_cancelorder(m_ygcancelorder);
                                    m_ygcancelorder.id = api_yg_cancelorderId;

                                    string sendresult = new SunShineInter().cancel_order(mapiservice, m_ygcancelorder);

                                    XmlDocument doc = new XmlDocument();
                                    doc.LoadXml(sendresult);
                                    XmlElement root = doc.DocumentElement;

                                    string req_seq = root.SelectSingleNode("req_seq").InnerText;//请求流水号
                                    string id = root.SelectSingleNode("result/id").InnerText;//结果id 
                                    string comment = root.SelectSingleNode("result/comment").InnerText;// 结果描述 

                                    m_ygcancelorder.rResultid = id;
                                    m_ygcancelorder.rResultComment = comment;
                                    new Api_yg_cancelorderData().EditApi_yg_cancelorder(m_ygcancelorder);

                                    if (id == "0000")//退票成功
                                    {
                                        //订单状态为:订单退款
                                        if (oldorder.Agentid == 0)
                                        {//直销订单，标记为处理中做财务处理
                                            oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                                        }
                                        else
                                        {//分销订单直接退款
                                            oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                                        }

                                        oldorder.service_lastcount = oldorder.U_num - oldorder.service_usecount - num;
                                        oldorder.Order_remark += "阳光接口退票:" + id + "返回内容:" + comment;
                                        data = OrderJsonData.InsertRecharge(oldorder);

                                        string rygorder_num = root.SelectSingleNode("order/order_num").InnerText;//订单号
                                        string rnum = root.SelectSingleNode("order/num").InnerText;// 张数 
                                        m_ygcancelorder.rygorder_num = rygorder_num;
                                        m_ygcancelorder.rnum = rnum.ConvertTo<int>(0);
                                        new Api_yg_cancelorderData().EditApi_yg_cancelorder(m_ygcancelorder);
                                    }
                                    else //退票失败
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = comment });
                                    }
                                }
                                catch
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "阳光接口退票失败" });
                                }
                            }
                            else
                            {
                                //订单状态为:订单退款
                                if (oldorder.Agentid == 0)
                                {//直销订单，标记为处理中做财务处理
                                    oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                                }
                                else
                                {//分销订单直接退款
                                    oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                                }
                                oldorder.Order_remark += "阳光提单时失败，所以只需退本系统票就可";
                                data = OrderJsonData.InsertRecharge(oldorder);
                            }
                            #endregion
                            #region 已注释
                            //#region 阳光接口退票
                            //if (oldorder.Service_order_num.Trim() != "")//生成了阳光订单，如果没有生成则只是退系统的票，不退阳光的票
                            //{
                            //    string sendresult = new SunShineInter().cancel_order(oldorder.Service_order_num, num.ToString());

                            //    XmlDocument doc = new XmlDocument();
                            //    doc.LoadXml(sendresult);
                            //    XmlElement root = doc.DocumentElement;

                            //    string req_seq = root.SelectSingleNode("req_seq").InnerText;//请求流水号
                            //    string id = root.SelectSingleNode("result/id").InnerText;//结果id 
                            //    string comment = root.SelectSingleNode("result/comment").InnerText;// 结果描述  
                            //    if (id == "0000")//退票成功
                            //    {
                            //        //订单状态为:订单退款
                            //        if (oldorder.Agentid == 0)
                            //        {//直销订单，标记为处理中做财务处理
                            //            oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                            //        }
                            //        else
                            //        {//分销订单直接退款
                            //            oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                            //        }
                            //        oldorder.Order_remark += "阳光接口退票:" + id + "返回内容:" + comment;
                            //        data = OrderJsonData.InsertRecharge(oldorder);

                            //    }
                            //    else //退票失败
                            //    {
                            //        return JsonConvert.SerializeObject(new { type = 1, msg = comment });
                            //    }
                            //}
                            //else
                            //{
                            //    //订单状态为:订单退款
                            //    if (oldorder.Agentid == 0)
                            //    {//直销订单，标记为处理中做财务处理
                            //        oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                            //    }
                            //    else
                            //    {//分销订单直接退款
                            //        oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                            //    }
                            //    oldorder.Order_remark += "阳光提单时失败，所以只需退本系统票就可";
                            //    data = OrderJsonData.InsertRecharge(oldorder);
                            //}
                            //#endregion
                            #endregion
                        }
                        else if (pro.Serviceid == 2)
                        {
                            #region 慧择网保险产品退票
                            //根据订单号得到投保单号
                            string insureNo = new Api_hzins_OrderApplyResp_OrderExtData().GetinsureNumbyorderid(orderid);//投保单号
                            if (insureNo != "")
                            {
                                string cancelresult = new HzinsInter().Hzins_orderCancel(orderid);
                                Hzins_OrderCancelResp mresult = (Hzins_OrderCancelResp)JsonConvert.DeserializeObject(cancelresult, typeof(Hzins_OrderCancelResp));
                                if (mresult != null)
                                {
                                    Api_hzins_orderCancel mApi_hzins_orderCancel = new Api_hzins_orderCancel
                                    {
                                        id = 0,
                                        orderid = orderid,
                                        insureNo = insureNo,
                                        respCode = mresult.respCode,
                                        respMsg = mresult.respMsg
                                    };
                                    new Api_hzins_orderCancelData().EditApi_hzins_orderCancel(mApi_hzins_orderCancel);

                                    if (mresult.respCode == 0)
                                    {
                                        //订单状态为:订单退款
                                        if (oldorder.Agentid == 0)
                                        {//直销订单，标记为处理中做财务处理
                                            oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                                        }
                                        else
                                        {//分销订单直接退款
                                            oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                                        }
                                        oldorder.Order_remark += "慧择网接口退票:" + mresult.respCode + "返回内容:" + mresult.respMsg;
                                        data = OrderJsonData.InsertRecharge(oldorder);
                                    }
                                    else
                                    {
                                        return JsonConvert.SerializeObject(new { type = 1, msg = mresult.respCode + "(" + mresult.respMsg + ")" });
                                    }
                                }
                            }
                            else
                            {
                                //订单状态为:订单退款
                                if (oldorder.Agentid == 0)
                                {//直销订单，标记为处理中做财务处理
                                    oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                                }
                                else
                                {//分销订单直接退款
                                    oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                                }
                                oldorder.Order_remark += "慧择网接口出票失败，所以只需要退本系统单就可以";
                                data = OrderJsonData.InsertRecharge(oldorder);
                            }

                            #endregion
                        }
                        else if (pro.Serviceid == 3)
                        {
                            #region  美景联动接口产品需要审核:如果生成了外部接口产品,上面已经处理；没有生成才会进入此流程
                            if (oldorder.Service_order_num != "")
                            {
                                //订单状态为:订单退款
                                if (oldorder.Agentid == 0)
                                {//直销订单，标记为处理中做财务处理
                                    oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                                }
                                else
                                {//分销订单直接退款
                                    oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                                }


                                oldorder.service_lastcount = oldorder.U_num - oldorder.service_usecount - num;
                                oldorder.Order_remark += "美景联动接口退票张数:" + num;
                                data = OrderJsonData.InsertRecharge(oldorder);
                            }
                            else
                            {
                                //订单状态为:订单退款
                                if (oldorder.Agentid == 0)
                                {//直销订单，标记为处理中做财务处理
                                    oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                                }
                                else
                                {//分销订单直接退款
                                    oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                                }
                                oldorder.Order_remark += "美景联动接口出票失败，所以只需要退本系统单就可以";
                                data = OrderJsonData.InsertRecharge(oldorder);
                            }
                            #endregion
                        }
                        else if (pro.Serviceid == 4) {

                            //对未完成的订单退票处理，和完成订单现在相同，可能没有遇到这样的订单，可以考虑万龙订单失败的情况下此订单还是退票，如果遇到后期再测试吧，这里逻辑乱了不做过多处理
                            B2b_company commanage = B2bCompanyData.GetAllComMsg(comid);
                            WlGetProInfoDealRequestData wldata = new WlGetProInfoDealRequestData(commanage.B2bcompanyinfo.wl_PartnerId, commanage.B2bcompanyinfo.wl_userkey);

                            var WlOrderinfo = wldata.SearchWlOrderData(comid, 0, "", orderid);
                            if (WlOrderinfo == null)
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "wl退票出错，订单查询失败" });
                            }

                            if (WlOrderinfo.quantity - WlOrderinfo.usedQuantity != num)
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "wl退票数量不符" });
                            }
                            double refundPrice = WlOrderinfo.unitprice * num;

                            var Refundorder = wldata.wlOrderRefundRequest_json(int.Parse(commanage.B2bcompanyinfo.wl_PartnerId), WlOrderinfo.wlorderid, WlOrderinfo.wldealid, WlOrderinfo.orderid.ToString(), WlOrderinfo.partnerdealid.ToString(), WlOrderinfo.vouchers, 1, WlOrderinfo.unitprice, refundPrice, 0);//

                            var wlOrderRefund = wldata.wlOrderRefundRequest_data(Refundorder, comid, 7);

                            if (wlOrderRefund.IsSuccess == true)
                            {
                                //订单状态为:订单退款
                                if (oldorder.Agentid == 0)
                                {//直销订单，标记为处理中做财务处理
                                    oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                                }
                                else
                                {//分销订单直接退款
                                    oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                                }

                                oldorder.service_lastcount = 0;
                                oldorder.Order_remark += "WL接口退票ID:" + WlOrderinfo.wlorderid + ":" + wlOrderRefund.Message;
                                data = OrderJsonData.InsertRecharge(oldorder);
                            }
                            else
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "wl退票出错" + wlOrderRefund.Message });
                            }
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "外部接口产品没有此产品" });

                        }

                    }
                    #endregion
                    #region 产品来源,分销系统导入产品
                    else if (pro.Source_type == 4)
                    {
                        //判断实际电子票数量
                        var prodata = new B2bEticketData();
                        var eticketinfo = prodata.SelectOrderid(oldorder.Bindingagentorderid);//通过订单号查询电子票是否存在
                        if (eticketinfo == null)
                        {
                            //return JsonConvert.SerializeObject(new { type = 1, msg = "没有此电子票" });
                        }
                        else
                        {
                            //退票数量必须等于未使用数量
                            if (num != eticketinfo.Use_pnum)
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "不支持部分退票" });
                            }
                        }

                        #region 作废票处理
                        //订单状态为:订单退款
                        if (oldorder.Agentid == 0)
                        {//直销订单，标记为处理中做财务处理
                            oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                        }
                        else
                        {//分销订单直接退款
                            oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                        }

                        data = OrderJsonData.InsertRecharge(oldorder);
                        if (eticketinfo != null)
                        {
                            //电子码作废
                            var eticketback = prodata.Backticket_use_num(oldorder.Bindingagentorderid);
                        }

                        #region--------------------对分销导入产品，对绑定分销账户的订单进行处理并退款-----------------------------

                        var agentorder = new B2bOrderData().GetOrderById(oldorder.Bindingagentorderid);
                        if (agentorder != null)
                        {


                            #region 外来接口产品退票 判断是否为对外接口产品(阳光；慧择网；美景联动)
                            int serviceid = new B2bComProData().GetServiceidbyproid(agentorder.Pro_id);
                            #region 阳光产品
                            if (serviceid == 1)
                            {
                                #region 生成了外部接口产品订单
                                if (agentorder.Service_order_num != "")
                                {
                                    #region 阳光接口退票
                                    string ygordernum = new Api_yg_addorder_outputData().GetApi_yg_ordernum(agentorder.Id);
                                    if (ygordernum.Trim() != "")//生成了阳光订单，如果没有生成则只是退系统的票，不退阳光的票
                                    {
                                        try
                                        {
                                            ApiService mapiservice = new ApiServiceData().GetApiservice(1);
                                            if (mapiservice == null)
                                            {
                                                return JsonConvert.SerializeObject(new { type = 1, msg = "产品服务商信息查询失败!" });
                                            }

                                            Api_yg_cancelorder m_ygcancelorder = new Api_yg_cancelorder
                                            {
                                                id = 0,
                                                organization = mapiservice.Organization,
                                                password = mapiservice.Password,
                                                req_seq = mapiservice.Organization + DateTime.Now.ToString("yyyyMMddhhssmm") + CommonFunc.CreateNum(6),
                                                ygorder_num = ygordernum,
                                                num = num,
                                                rResultid = "",
                                                rResultComment = "",
                                                rygorder_num = "",
                                                rnum = 0,
                                                orderId = agentorder.Id,
                                                opertime = DateTime.Now
                                            };
                                            int api_yg_cancelorderId = new Api_yg_cancelorderData().EditApi_yg_cancelorder(m_ygcancelorder);
                                            m_ygcancelorder.id = api_yg_cancelorderId;

                                            string sendresult = new SunShineInter().cancel_order(mapiservice, m_ygcancelorder);

                                            XmlDocument doc = new XmlDocument();
                                            doc.LoadXml(sendresult);
                                            XmlElement root = doc.DocumentElement;

                                            string req_seq = root.SelectSingleNode("req_seq").InnerText;//请求流水号
                                            string id = root.SelectSingleNode("result/id").InnerText;//结果id 
                                            string comment = root.SelectSingleNode("result/comment").InnerText;// 结果描述 

                                            m_ygcancelorder.rResultid = id;
                                            m_ygcancelorder.rResultComment = comment;
                                            new Api_yg_cancelorderData().EditApi_yg_cancelorder(m_ygcancelorder);

                                            if (id == "0000")//退票成功
                                            {
                                                agentorder.service_lastcount = agentorder.U_num - agentorder.service_usecount - num;
                                                agentorder.Order_remark += "阳光接口退票:" + id + "返回内容:" + comment;


                                                string rygorder_num = root.SelectSingleNode("order/order_num").InnerText;//订单号
                                                string rnum = root.SelectSingleNode("order/num").InnerText;// 张数 
                                                m_ygcancelorder.rygorder_num = rygorder_num;
                                                m_ygcancelorder.rnum = rnum.ConvertTo<int>(0);
                                                new Api_yg_cancelorderData().EditApi_yg_cancelorder(m_ygcancelorder);
                                            }
                                            else //退票失败
                                            {
                                                return JsonConvert.SerializeObject(new { type = 1, msg = comment });
                                            }
                                        }
                                        catch
                                        {
                                            return JsonConvert.SerializeObject(new { type = 1, msg = "阳光接口退票失败" });
                                        }
                                    }
                                    #endregion
                                    #region  已注释
                                    //#region 阳光接口退票

                                    //string sendresult = new SunShineInter().cancel_order(agentorder.Service_order_num, num.ToString());

                                    //XmlDocument doc = new XmlDocument();
                                    //doc.LoadXml(sendresult);
                                    //XmlElement root = doc.DocumentElement;

                                    //string req_seq = root.SelectSingleNode("req_seq").InnerText;//请求流水号
                                    //string id = root.SelectSingleNode("result/id").InnerText;//结果id 
                                    //string comment = root.SelectSingleNode("result/comment").InnerText;// 结果描述  
                                    //if (id == "0000")//退票成功
                                    //{
                                    //    ////订单状态为:订单退款
                                    //    //if (agentorder.Agentid == 0)
                                    //    //{//直销订单，标记为处理中做财务处理
                                    //    //    agentorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                                    //    //}
                                    //    //else
                                    //    //{//分销订单直接退款
                                    //    //agentorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                                    //    //}
                                    //    agentorder.Order_remark += "阳光接口退票:" + id + "返回内容:" + comment;
                                    //    //data = OrderJsonData.InsertRecharge(agentorder);

                                    //}
                                    //else //退票失败
                                    //{
                                    //    return JsonConvert.SerializeObject(new { type = 1, msg = comment });
                                    //}
                                    //#endregion
                                    #endregion
                                }
                                #endregion
                                #region 没有生成外部接口产品，无需处理
                                else
                                { }
                                #endregion
                            }
                            #endregion
                            #region 慧择网产品
                            else if (serviceid == 2)
                            {
                                #region 生成了外部接口订单
                                string insureNo = new Api_hzins_OrderApplyResp_OrderExtData().GetinsureNumbyorderid(oldorder.Bindingagentorderid);//投保单号
                                if (insureNo != "")
                                {
                                    #region 慧择网保险产品退票
                                    string cancelresult = new HzinsInter().Hzins_orderCancel(oldorder.Bindingagentorderid);
                                    Hzins_OrderCancelResp mresult = (Hzins_OrderCancelResp)JsonConvert.DeserializeObject(cancelresult, typeof(Hzins_OrderCancelResp));
                                    if (mresult != null)
                                    {
                                        Api_hzins_orderCancel mApi_hzins_orderCancel = new Api_hzins_orderCancel
                                        {
                                            id = 0,
                                            orderid = oldorder.Bindingagentorderid,
                                            insureNo = insureNo,
                                            respCode = mresult.respCode,
                                            respMsg = mresult.respMsg
                                        };
                                        new Api_hzins_orderCancelData().EditApi_hzins_orderCancel(mApi_hzins_orderCancel);

                                        if (mresult.respCode == 0)
                                        {
                                            ////订单状态为:订单退款
                                            //agentorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款

                                            agentorder.Order_remark += "慧择网接口退票:" + mresult.respCode + "返回内容:" + mresult.respMsg;
                                            //data = OrderJsonData.InsertRecharge(agentorder);
                                        }
                                        else
                                        {
                                            return JsonConvert.SerializeObject(new { type = 1, msg = mresult.respCode + "(" + mresult.respMsg + ")" });
                                        }
                                    }

                                    #endregion
                                }
                                #endregion
                                #region 没有生成外部接口产品，无需处理
                                else
                                { }
                                #endregion
                            }
                            #endregion
                            #region 美景联动产品需要审核：如果生成了外部接口订单上面一开始就已经处理；没有生成此处更不需要处理
                            else if (serviceid == 3)
                            {
                                if (agentorder.Service_order_num != "")
                                {
                                    agentorder.Order_remark += "美景联动接口退票张数:" + num;
                                    agentorder.service_lastcount = agentorder.U_num - agentorder.service_usecount - num;
                                }
                                else
                                {
                                    agentorder.Order_remark += "美景联动接口退票张数:" + num;
                                }
                            }
                            #endregion
                            else { }

                            #endregion


                            agentorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                            var agentorderup = OrderJsonData.InsertRecharge(agentorder);

                            //读取分销商信息
                            Agent_company agentinfo = AgentCompanyData.GetAgentWarrant(agentorder.Agentid, agentorder.Comid);
                            if (agentinfo == null)
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "绑定的分销账户授权信息错误，退票操作部分失败，请手工完成后续操作" });
                            }
                            else
                            {
                                if (agentorder.Warrant_type == 1)
                                {
                                    //计算分销余额
                                    decimal overmoney = agentinfo.Imprest + num * agentorder.Pay_price;

                                    //分销商财务扣款
                                    Agent_Financial Financialinfo = new Agent_Financial
                                    {
                                        Id = 0,
                                        Com_id = agentorder.Comid,
                                        Agentid = agentorder.Agentid,
                                        Warrantid = agentorder.Warrantid,
                                        Order_id = agentorder.Id,
                                        Servicesname = pro.Pro_name + "[" + oldorder.Bindingagentorderid + "]",
                                        SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                                        Money = num * agentorder.Pay_price,
                                        Payment = 0,            //收支(0=收款,1=支出)
                                        Payment_type = "退票退款",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点,分销扣款）
                                        Money_come = "分销账户",         //资金来源（网上支付,银行收款，分销账户等）
                                        Over_money = overmoney
                                    };
                                    var fin = AgentCompanyData.AgentFinancial(Financialinfo);
                                    //退分销订单手续费
                                    var HuigunShouxufei_temp = OrderJsonData.HuigunShouxufei(agentorder, num * agentorder.Pay_price);
                                }

                            }
                        }
                        #endregion


                        //分销财务处理
                        if (oldorder.Agentid != 0)
                        {
                            backorderstate = 1;
                        }

                        #endregion

                    }
                    #endregion
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "产品来源不对" });
                    }

                    #region 注释内容
                    //#region 实物订单
                    //if (pro.Source_type == 1 && pro.Server_type == 11)
                    //{



                    //    #region 作废票处理
                    //    //订单状态为:订单退款
                    //    if (oldorder.Agentid == 0)
                    //    {//直销订单，标记为处理中做财务处理
                    //        oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                    //    }
                    //    else
                    //    {//分销订单直接退款
                    //        oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                    //    }

                    //    data = OrderJsonData.InsertRecharge(oldorder);
                    //    //电子码作废
                    //    var eticketback = prodata.Backticket_use_num(orderid);


                    //    //分销财务处理
                    //    if (oldorder.Agentid != 0)
                    //    {
                    //        backorderstate = 1;
                    //    }
                    //    #endregion
                    //}
                    //#endregion

                    //#region  定点标注退款
                    //if (oldorder.Agentid == 0)
                    //{ //直销订单，标记为处理中做财务处理
                    //    oldorder.Order_state = (int)OrderStatus.HandleQuitOrder;//订单退款
                    //}
                    //else
                    //{ //分销订单直接退款
                    //    oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退款
                    //}
                    //data = OrderJsonData.InsertRecharge(oldorder);
                    //#endregion
                    #endregion
                }
                #endregion

                #region  必须分销订单 Agentid，必须是后台销售订单Warrant_type(倒码订单不需要财务处理)，
                if (oldorder.Agentid != 0 && oldorder.Warrant_type == 1)
                {

                    //读取分销商信息
                    Agent_company agentinfo = AgentCompanyData.GetAgentWarrant(oldorder.Agentid, oldorder.Comid);
                    if (agentinfo == null)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "分销账户信息错误" });
                    }

                    //计算分销余额
                    decimal overmoney = agentinfo.Imprest + num * oldorder.Pay_price + oldorder.Express;

                    //分销商财务扣款
                    Agent_Financial Financialinfo = new Agent_Financial
                    {
                        Id = 0,
                        Com_id = oldorder.Comid,
                        Agentid = oldorder.Agentid,
                        Warrantid = oldorder.Warrantid,
                        Order_id = oldorder.Id,
                        Servicesname = pro.Pro_name + "[" + orderid + "]",
                        SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                        Money = num * oldorder.Pay_price + oldorder.Express,
                        Payment = 0,            //收支(0=收款,1=支出)
                        Payment_type = "退票退款",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点,分销扣款）
                        Money_come = "分销账户",         //资金来源（网上支付,银行收款，分销账户等）
                        Over_money = overmoney
                    };
                    var fin = AgentCompanyData.AgentFinancial(Financialinfo);

                    //退分销订单手续费
                    var HuigunShouxufei_temp = OrderJsonData.HuigunShouxufei(oldorder, num * oldorder.Pay_price + oldorder.Express);

                }
                #endregion

                #region 把退票张数记录到数据库订单表中;并且把销售数量回滚
                if (orderid > 0 && num > 0)
                {
                    #region 原订单 和 绑定订单 退票 退票数量记录入数据库
                    //原订单退票 退票数量记录入数据库
                    int inscanclenum1 = new B2bOrderData().InsertCancleTicketNum(orderid, oldorder.Cancelnum + num);

                    //绑定订单退票 退票数量记录入数据库
                    if (pro.Source_type == 4)
                    {
                        int inscanclenum2 = new B2bOrderData().InsertCancleTicketNum(oldorder.Bindingagentorderid, oldorder.Cancelnum + num);
                    }
                    #endregion

                    #region  服务类型是票务 或者 实物，需要把可销售数量和 已销售数量回滚
                    if (pro.Server_type == 1 || pro.Server_type == 11)
                    {
                        if (pro.Ispanicbuy == 1 || pro.Ispanicbuy == 2)
                        {
                            //回滚原产品 和 导入原产品的产品的库存
                            int rollbackBindingproKucun = new B2bComProData().RollbackProKucun(pro.Id, num, oldorder.U_traveldate.ToString("yyyy-MM-dd"), "");
                        }
                    }
                    #endregion

                    #region  服务类型是旅游大巴 并且没有截团，需要把空位数量 回滚
                    if (pro.Server_type == 10)
                    {
                        int isjietuan = new Travelbusorder_operlogData().Ishasplanbus(proid, oldorder.U_traveldate);
                        if (isjietuan == 0)
                        {
                            int rollbackBindingproKucun = new B2bComProData().RollbackProKucun(pro.Id, num, oldorder.U_traveldate.ToString("yyyy-MM-dd"), "");
                        }
                    }
                    #endregion

                    #region 服务类型是酒店
                    if (pro.Server_type == 9)
                    {
                        if (oldorder.Bindingagentorderid > 0)
                        {
                            //查询酒店订单的入住日期和离店日期
                            B2b_order_hotel morderhotel = new B2b_order_hotelData().GetHotelOrderByOrderId(oldorder.Bindingagentorderid);
                            if (morderhotel != null)
                            {
                                int rollbackBindingproKucun = new B2bComProData().RollbackProKucun(pro.Id, num, morderhotel.Start_date.ToString("yyyy-MM-dd"), morderhotel.End_date.ToString("yyyy-MM-dd"));
                            }
                        }
                        else
                        {
                            //查询酒店订单的入住日期和离店日期
                            B2b_order_hotel morderhotel = new B2b_order_hotelData().GetHotelOrderByOrderId(oldorder.Id);
                            if (morderhotel != null)
                            {
                                int rollbackBindingproKucun = new B2bComProData().RollbackProKucun(pro.Id, num, morderhotel.Start_date.ToString("yyyy-MM-dd"), morderhotel.End_date.ToString("yyyy-MM-dd"));
                            }
                        }
                    }
                    #endregion
                }
                #endregion


                #region 如果是 13 教练预约,对时间控制 已使用状态取消
                if (pro.Server_type == 13)
                {
                    var shanchuyishiyongshijian = B2bCompanyManagerUserData.UseworktimeDelOid(oldorder.Id);
                }
                #endregion


                #region 直销订单， 退票则需要把 推荐人的返佣金额减少
                if (oldorder.Agentid == 0)
                {
                    //推荐人返佣
                    try
                    {
                        B2b_com_pro modelcompro = pro;
                        B2b_order modelb2border = oldorder;
                        if (modelcompro != null)
                        {
                            if (modelcompro.isrebate == 1)
                            {
                                //查询产品的返佣进账记录
                                Member_channel_rebatelog incomelog = new Member_channel_rebatelogData().GetRebateIncomelog(modelcompro.Id);
                                if (incomelog != null)
                                {
                                    //获得渠道人的返佣余额
                                    decimal channelrebatemoney = new Member_channel_rebatelogData().Getrebatemoney(modelb2border.recommendchannelid);
                                    //返佣记录
                                    Member_channel_rebatelog rebatelog = new Member_channel_rebatelog
                                    {
                                        id = 0,
                                        channelid = modelb2border.recommendchannelid,
                                        orderid = modelb2border.Id,
                                        payment = 3,
                                        payment_type = "返佣退货",
                                        proid = modelcompro.Id.ToString(),
                                        proname = modelcompro.Pro_name,
                                        subdatetime = DateTime.Now,
                                        ordermoney = incomelog.ordermoney,
                                        rebatemoney = -incomelog.rebatemoney * (num / modelb2border.U_num),
                                        over_money = decimal.Round(channelrebatemoney - incomelog.rebatemoney * (num / modelb2border.U_num), 2),
                                        comid = modelb2border.Comid
                                    };
                                    //增加返佣记录 同时增加渠道人的返佣金额
                                    new Member_channel_rebatelogData().Editrebatelog(rebatelog);
                                    new Member_channel_rebatelogData().Editchannelrebate(rebatelog.channelid, rebatelog.over_money);
                                }
                            }
                        }
                    }
                    catch { }
                }
                #endregion

                #region 旅游大巴产品则应该把保险也退掉
                if (oldorder.givebaoxianorderid > 0)
                {
                    B2b_order baoxininfo = new B2bOrderData().GetOrderById(oldorder.givebaoxianorderid);
                    if (baoxininfo != null)
                    {
                        try
                        {
                            OrderJsonData.QuitOrder(baoxininfo.Comid, oldorder.givebaoxianorderid, baoxininfo.Pro_id, baoxininfo.U_num, "旅游大巴退票同时退掉保险");
                        }
                        catch { }
                    }
                }
                #endregion

                return JsonConvert.SerializeObject(new { type = 100, msg = "退票成功" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "传参失败" });
            }
        }
        //morenfasong 分销提交订单默认=1 扣款发送电子票，如果=0则只提交订单。  新增可选参数:上车地点,订单备注
        public static string AgentOrder(int agentid, string proid, string ordertype, string u_num, string u_name, string u_phone, string u_traveldate, string agentaccount, int isInterfaceSub, out int orderid, int real_name_type = 0, int morenfasong = 1, string pickuppoint = "", string dropoffpoint = "", string order_remark = "", int deliverytype = 0, string province = "", string city = "", string address = "", string txtcode = "", int shopcartid = 0, decimal expressfee = 0, int u_childnum = 0, int yanzheng_method = 0, int speciid = 0, string baoxiannames = "", string baoxianpinyinnames = "", string baoxianidcards = "", int channelcoachid = 0, B2b_order_hotel b2b_order_hotel = null, int aorderid = 0, string travelnames = "", string travelidcards = "", string travelnations = "", string travelphones = "", string travelremarks = "",string u_idcard="")
        {

            decimal payprice = 0;
            decimal cost = 0;
            decimal profit = 0;

            int warrantid = 0;
            int Warrant_type = 0;
            int Warrant_level = 0;
            int agentsunid = 0;
            int comid = 0;

            string data = "";

            orderid = 0;//先默认返回订单号

            B2b_com_pro pro = new B2bComProData().GetProById(proid.ToString());
            if (pro != null)
            {
                comid = pro.Com_id;
            }
            else
            {
                data = "{\"type\":1,\"msg\":\"产品不存在\"}";
                return data;
            }

            Agent_company agentwarrantinfo = AgentCompanyData.GetAgentWarrant(agentid, comid);

            if (agentwarrantinfo != null)
            {
                warrantid = agentwarrantinfo.Warrantid;
                Warrant_type = agentwarrantinfo.Warrant_type;//支付类型分销 1出票扣款 2验码扣款

                //订房，大巴，实物，保险，只能出票扣款
                if (pro.Server_type == 9 || pro.Server_type == 10 || pro.Server_type == 14 || pro.Server_type == 11)
                {
                    Warrant_type = 1;
                }

                //if (isInterfaceSub == 1)//如果是接口提交的订单，则支付类型都改为: 1出票扣款
                //{
                //    Warrant_type = 1;
                //}
                Warrant_level = agentwarrantinfo.Warrant_level;
                if (agentwarrantinfo.Warrant_state == 0)
                {
                    data = "{\"type\":1,\"msg\":\"产品还没有得到商户授权\"}";

                    return data;
                }
            }
            else
            {
                data = "{\"type\":1,\"msg\":\"产品还没有得到商户授权\"}";

                return data;
            }



            if (pro != null)
            {
                //旅游产品计算单价、成本、毛利
                if (pro.Server_type == 2 || pro.Server_type == 8)
                {
                    cost = pro.Agentsettle_price;
                    comid = pro.Com_id;

                    B2b_com_LineGroupDate lvnowday = new B2b_com_LineGroupDateData().GetLineDayGroupDate(u_traveldate.ConvertTo<DateTime>(), pro.Id);

                    if (lvnowday != null)
                    {
                        //分销差额
                        decimal differprice = 0;
                        if (Warrant_level == 1)
                        {
                            differprice = pro.Agent1_price;
                        }
                        if (Warrant_level == 2)
                        {
                            differprice = pro.Agent2_price;
                        }
                        if (Warrant_level == 3)
                        {
                            differprice = pro.Agent3_price;
                        }
                        payprice = lvnowday.Menprice - differprice;

                        profit = payprice - cost;//根据授权级别，得到分销价格在计算毛利
                    }
                    else
                    {
                        data = "{\"type\":1,\"msg\":\"团期信息获取失败\"}";

                        return data;
                    }
                }
                else if (pro.Server_type == 9)
                { //房
                    cost = pro.Agentsettle_price;
                    comid = pro.Com_id;

                    decimal Menprice = new B2b_com_LineGroupDateData().Gethotelallprice(pro.Id, b2b_order_hotel.Start_date, b2b_order_hotel.End_date, Warrant_level);//单人次价格
                   
                    if (Menprice != 0)
                    { 
                        payprice = Menprice;

                        profit = 0;//订房无法计算毛利，因为每天日历并没有填写成本
                    }
                    else
                    {
                        data = "{\"type\":1,\"msg\":\"房态信息获取失败\"}";

                        return data;
                    }

                }
                else if (pro.Server_type == 10)
                { //大巴
                    cost = pro.Agentsettle_price;
                    comid = pro.Com_id;

                    //得到大巴团期表中分销返还情况，如果团期表中没有设置(分销返还都是0)，则读取基本信息中的分销返还
                    B2b_com_LineGroupDate mgroupdate=  new B2b_com_LineGroupDateData().GetLineDayGroupDate(u_traveldate.ConvertTo<DateTime>(DateTime.Parse("1970-01-01")),pro.Id);
                   
                    //分销价
                    decimal differprice = 0;
                    if (mgroupdate != null)
                    {
                        if (mgroupdate.agent1_back == 0 && mgroupdate.agent2_back == 0 && mgroupdate.agent3_back == 0)
                        {
                            if (Warrant_level == 1)
                            {
                                differprice = pro.Agent1_price;
                            }
                            if (Warrant_level == 2)
                            {
                                differprice = pro.Agent2_price;
                            }
                            if (Warrant_level == 3)
                            {
                                differprice = pro.Agent3_price;
                            }
                            payprice = differprice;
                            profit = Decimal.Parse(payprice.ToString()) - cost;//根据授权级别，得到分销价格在计算毛利
                        }
                        else 
                        {
                            if (Warrant_level == 1)
                            {
                                differprice = mgroupdate.agent1_back;
                            }
                            if (Warrant_level == 2)
                            {
                                differprice = mgroupdate.agent2_back;
                            }
                            if (Warrant_level == 3)
                            {
                                differprice = mgroupdate.agent3_back;
                            }
                            payprice = differprice;
                            profit = Decimal.Parse(payprice.ToString()) - cost;//根据授权级别，得到分销价格在计算毛利
                        }
                    }
                    else 
                    {
                        data = "{\"type\":1,\"msg\":\"没有团期\"}";

                        return data;
                    }

                }
                //其他产品(暂时是 票务、实物)计算单价、成本、毛利
                else
                {
                    cost = pro.Agentsettle_price;
                    comid = pro.Com_id;
                    if (Warrant_level == 1)
                    {
                        payprice = pro.Agent1_price;
                    }
                    if (Warrant_level == 2)
                    {
                        payprice = pro.Agent2_price;
                    }
                    if (Warrant_level == 3)
                    {
                        payprice = pro.Agent3_price;
                    }
                    profit = Decimal.Parse(payprice.ToString()) - cost;//根据授权级别，得到分销价格在计算毛利

                    //如果有规格 则规格读取 价格
                    if (speciid != 0)
                    {
                        B2b_com_pro prospeciid = new B2bComProData().GetProspeciidById(proid.ToString(), speciid);
                        if (prospeciid != null)
                        {

                            cost = prospeciid.Agentsettle_price;

                            if (Warrant_level == 1)
                            {
                                payprice = prospeciid.Agent1_price;
                            }
                            if (Warrant_level == 2)
                            {
                                payprice = prospeciid.Agent2_price;
                            }
                            if (Warrant_level == 3)
                            {
                                payprice = prospeciid.Agent3_price;
                            }

                            profit = payprice - cost;

                        }
                        else
                        {
                            data = "{\"type\":1,\"msg\":\"规格传入错误\"}";

                            return data;
                        }


                    }




                }
                if (u_traveldate == "")
                {
                    u_traveldate = pro.Pro_end.ToString();
                }
                if (pro.Pro_state == 0)
                {
                    data = "{\"type\":1,\"msg\":\"产品已暂停\"}";

                    return data;
                }
                if (pro.Pro_end < DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    data = "{\"type\":1,\"msg\":\"产品已过期\"}";

                    return data;
                }

                //库存票检查库存数量，不足则不让提交订单
                if (pro.Source_type == 2)
                {
                    int kucunpiaoshuliang = new B2bComProData().ProSEPageCount_UNUse(pro.Id);
                    if (kucunpiaoshuliang < Int32.Parse(u_num))
                    {
                        data = "{\"type\":1,\"msg\":\"库存票不足，请电话订购或联系商家\"}";

                        return data;
                    }
                }

                //判断产品实名制类型是否一致
                if (pro.Realnametype != real_name_type)
                {
                    data = "{\"type\":1,\"msg\":\"产品实名制类型不一致\"}";

                    return data;
                }



                //判断u_name最后是否含有逗号，含有的话删除
                if (u_name.Trim() != "")
                {
                    u_name = u_name.Replace("，", ",");
                    if (u_name.Substring(u_name.Length - 1) == ",")
                    {
                        u_name = u_name.Substring(0, u_name.Length - 1);
                    }
                    if (u_name.Length > 25)
                    {
                        data = "{\"type\":1,\"msg\":\"姓名总长度不可超过25字\"}";

                        return data;
                    }
                }
                //判断实名制类型：0无需实名 1一张一人,2一单一人
                if (real_name_type != 0)
                {
                    if (real_name_type == 1)
                    {
                        if (u_name.Trim() != "")
                        {
                            string[] str = u_name.Split(',');
                            if (str.Length > 3)
                            {
                                data = "{\"type\":1,\"msg\":\"实名制类型为一张一人,最多只能购买3张\"}";

                                return data;
                            }
                        }
                    }
                    if (real_name_type == 2)
                    {
                        if (u_name.Trim() != "")
                        {
                            string[] str = u_name.Split(',');
                            if (str.Length > 1)
                            {
                                data = "{\"type\":1,\"msg\":\"实名制类型为一单一人,只需提供一个人名字即可\"}";

                                return data;
                            }
                        }
                    }
                }

            }
            else
            {
                data = "{\"type\":1,\"msg\":\"产品不存在\"}";

                return data;
            }



            if (agentaccount != "")//如果为空的话则是通过接口提交的订单；否则是通过分销后台提交的订单，需要考虑员工账户额度限定；
            {
                Agent_regiinfo agentinfo = AgentCompanyData.GetAgentAccountByUid(agentaccount, agentid);
                if (agentinfo.AccountLevel != 0)
                {
                    //员工账户额度限定，非注册账户则为员工账户
                    //if (agentinfo.Accounttype == 1)//出票扣款
                    //{
                    if (Warrant_type == 1)//出票扣款
                    {
                        if (agentinfo.Amount < payprice * Int32.Parse(u_num))
                        {
                            data = "{\"type\":1,\"msg\":\"员工账户销授权金额不足，请联系您的分销商\"}";

                            return data;
                        }
                    }

                    agentsunid = agentinfo.Id;
                }
            }

            B2b_order order = new B2b_order()
            {
                M_b2b_order_hotel = b2b_order_hotel,
                Id = 0,
                Pro_id = proid.ConvertTo<int>(0),
                Speciid = speciid,
                Order_type = ordertype.ConvertTo<int>(1),
                U_name = u_name,
                U_id = 0,
                U_phone = u_phone,
                U_idcard= u_idcard,
                U_num = u_num.ConvertTo<int>(0),
                U_subdate = DateTime.Now,
                Payid = 0,
                Pay_price = payprice,
                Cost = cost,
                Profit = profit,
                Order_state = (int)OrderStatus.WaitPay,//
                Pay_state = (int)PayStatus.NotPay,//支付模块未做出来前先设置为已支付
                Send_state = (int)SendCodeStatus.NotSend,
                Ticketcode = 0,//电子码未创建，支付后产生码赋值
                Rebate = 0,//  利润返佣金额暂时定为0
                Ordercome = "",//订购来源 暂时定为空
                U_traveldate = DateTime.Parse(u_traveldate),
                Dealer = "自动",
                Comid = comid,
                Pno = "",
                Openid = "",
                Ticketinfo = "",
                Integral1 = 0,   //积分
                Imprest1 = 0,    //预付款
                Agentid = agentid,     //分销ID
                Agentsunid = agentsunid,
                Warrantid = warrantid,   //授权ID
                Warrant_type = Warrant_type, //支付类型分销 1出票扣款 2验码扣款
                pickuppoint = pickuppoint,
                dropoffpoint = dropoffpoint,
                Order_remark = order_remark,
                Deliverytype = deliverytype,
                Province = province,
                City = city,
                Address = address,
                Code = txtcode,
                Shopcartid = shopcartid,
                Express = expressfee,
                Child_u_num = u_childnum,
                childreduce = pro.Childreduce,
                yanzheng_method = yanzheng_method,
                baoxiannames = baoxiannames,
                baoxianpinyinnames = baoxianpinyinnames,
                baoxianidcards = baoxianidcards,
                channelcoachid = channelcoachid,
                aorderid = aorderid,

                travelnames = travelnames,
                travelidcards = travelidcards,
                travelnations = travelnations,
                travelphones = travelphones,
                travelremarks = travelremarks,

                isInterfaceSub = isInterfaceSub
            };

            data = InsertOrUpdate(order, out orderid, morenfasong, isInterfaceSub);
            return data;
        }


        //保存常用地址
        public static int SaveAddress(int agentid, string u_name, string u_phone, string province, string city, string address, string txtcode)
        {

            //先查询是否有此地址了，这个先不做了，同一个地址同一个人也可能

            //直接保存
            B2b_order order = new B2b_order()
            {
                Id = 0,
                U_name = u_name,
                U_phone = u_phone,
                Agentid = agentid,     //分销ID
                Province = province,
                City = city,
                Address = address,
                Code = txtcode
            };
            B2bOrderData orderdate = new B2bOrderData();

            var data = orderdate.SaveAddress(order);
            return data;
        }




        /// <summary>
        /// 特定时间段旅游大巴订单统计数据
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="servertype"></param>
        /// <param name="comid"></param>
        /// <returns></returns>
        public static string travelbusordercountbyday(DateTime startdate, DateTime enddate, int servertype, int comid)
        {
            List<DateTime> list = new List<DateTime>();
            for (int i = 0; i < (enddate - startdate).Days + 1; i++)
            {
                list.Add(startdate.AddDays(i));
            }
            if (list.Count > 0)
            {
                IEnumerable result = "";
                if (list != null)
                {
                    result = from m in list
                             select new
                             {
                                 daydate = m.ToString("yyyy-MM-dd"),
                                 //支付成功人数
                                 paysucnum = new B2bOrderData().GetPaysucNumByServertype(m, servertype, comid, (int)PayStatus.HasPay, 0, "4,8,22"),
                                 //结团人数
                                 closeteamnu = new B2bOrderData().GetCloseTeamNumByServertype(m, servertype, comid, (int)OrderStatus.HasFin),
                             };
                    return JsonConvert.SerializeObject(new { type = 100, msg = result });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }

        public static string Travelbusorderdetail(string traveldate, int proid, int order_state)
        {
            List<B2b_order> list = new B2bOrderData().Travelbusorderdetail(traveldate, proid, order_state);
            if (list == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
            else
            {
                if (list.Count == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = list });
                }
            }
        }

        public static string Edittravelbusorder_operlog(int operlogid, int proid, string proname, string gooutdate, string operremark, int bustotal, string busids, string travelbus_model, string seatnum, string licenceplate, string drivername, string driverphone, int userid, int comid, string issavebus)
        {
            int r = new Travelbusorder_operlogData().Edittravelbusorder_operlog(operlogid, proid, proname, gooutdate, operremark, bustotal, busids, travelbus_model, seatnum, licenceplate, drivername, driverphone, userid, comid, issavebus);
            if (r > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = r });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }

        public static string Gettravelbusorder_sendbusBylogid(int logid)
        {
            int totle = 0;
            IList<Travelbusorder_sendbus> list = new Travelbusorder_sendbusData().Gettravelbusorder_sendbusBylogid(logid, out totle);
            if (totle == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = list, totle = totle });
            }
        }
        /// <summary>
        /// 根据出行日期得到产品列表
        /// </summary>
        /// <param name="daydate"></param>
        /// <param name="servertype"></param>
        /// <param name="comid"></param>
        /// <param name="orderstate"></param>
        /// <returns></returns>
        public static string Getb2bcomprobytraveldate(DateTime daydate, int servertype, int comid)
        {
            IList<B2b_com_pro> list = new B2bComProData().Getb2bcomprobytraveldate(daydate, servertype, comid);
            if (list == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
            else
            {
                if (list.Count > 0)
                {
                    IEnumerable result = "";
                    result = from m in list
                             select new
                             {
                                 Proid = m.Id,
                                 Proname = m.Pro_name,
                                 //支付成功人数
                                 paysucbooknum = new B2bOrderData().GetPaySucNumByProid(m.Id, comid, (int)PayStatus.HasPay, daydate, 0, "4,8,22"),
                                 //结团人数
                                 closeteamnum = new B2bOrderData().GetCloseTeamNumByProid(m.Id, comid, (int)OrderStatus.HasFin, daydate),
                                 //是否已经派车
                                 ishasplanbus = new Travelbusorder_operlogData().Ishasplanbus(m.Id, daydate),
                                 //派车的车辆情况
                                 busdetail = new Travelbusorder_sendbusData().BusDetailstr(m.Id, daydate)
                             };
                    return JsonConvert.SerializeObject(new { type = 100, msg = result });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
            }
        }
        /// <summary>
        /// 结单处理
        /// </summary>
        /// <param name="proid"></param>
        /// <param name="daydate"></param>
        /// <returns></returns>
        public static string CloseOrder(int proid, string daydate, int userid, int comid)
        {
            try
            {


                int r = new B2b_com_LineGroupDateData().CleanEmptyNum(proid, DateTime.Parse(daydate), userid, comid);
                if (r > 0)
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = "结单成功" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "结单失败" });
                }
            }
            catch
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "结单失败" });
            }
        }

        public static string Gettravelbus(int busid)
        {
            Travelbusorder_sendbus r = new Travelbusorder_sendbusData().Gettravelbus(busid);
            if (r != null)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = r });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = r });
            }
        }

        public static string travelbusorderdetailByiscloseteam(string gooutdate, int proid, int iscloseteam)
        {
            IList<B2b_order> list = new B2bOrderData().travelbusorderdetailByiscloseteam(gooutdate, proid, iscloseteam);
            if (list == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
            else
            {
                if (list.Count == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = list });
                }
            }
        }

        public static string travelbusorderdetailBypaystate(string gooutdate, int proid, int paystate)
        {
            IList<B2b_order> list = new B2bOrderData().travelbusorderdetailBypaystate(gooutdate, proid, paystate);
            if (list == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
            else
            {
                if (list.Count == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = list });
                }
            }
        }

        public static string travelbustravelerlistBypaystate(string gooutdate, int proid, int paystate, string orderstate = "")
        {
            IList<b2b_order_busNamelist> list = new B2bOrderData().travelbustravelerlistBypaystate(gooutdate, proid, paystate, 0, orderstate);
            if (list == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
            else
            {
                if (list.Count == 0)
                {
                    //return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                    //var groupbypickuppoint = list.GroupBy(a => a.pickuppoint).Select(g => (new { pickuppoint = g.Key, count = g.Count() }));

                    var proname = "";
                    proname = new B2bComProData().GetProName(proid);

                    B2b_order_busRemindSms remindsms = new B2b_order_busRemindSmsData().GetB2b_order_busRemindSms(proid, gooutdate);
                    string licenceplate = "";//牌照
                    string telphone = "";//司机电话
                    if (remindsms != null)
                    {
                        licenceplate = remindsms.licenceplate;
                        telphone = remindsms.telphone;
                    }
                    //得到发送日志
                    int isreminded = 0;//是否发送过
                    string sendtophones = "";//发送的手机号列表字符串
                    B2b_order_busRemindSmsLog remindsmslog = new B2b_order_busRemindSmsLogData().GetB2b_order_busRemindSmsSucLog(proid, gooutdate);
                    if (remindsmslog != null)
                    {
                        if (remindsmslog.issuc == 1)
                        {
                            isreminded = 1;
                            sendtophones = remindsmslog.sendtophones;
                        }
                    }

                    //得到给送车员的发送日志
                    int issendtodriver = 0;
                    B2b_order_busNamelistSendLog sendtodriverlog = new B2b_order_busNamelistSendLogData().GetB2b_order_busNamelistSendSucLog(proid, gooutdate);
                    if (sendtodriverlog != null)
                    {
                        issendtodriver = 1;
                    }

                    return JsonConvert.SerializeObject(new { type = 100, groupbydata = list, proname = proname, gooutdate = gooutdate, msg = list, totalcount = list.Count, licenceplate = licenceplate, telphone = telphone, isreminded = isreminded, sendtophones = sendtophones, issendtodriver = issendtodriver });

                }
                else
                {
                    var groupbypickuppoint = list.GroupBy(a => a.pickuppoint).Select(g => (new { pickuppoint = g.Key, count = g.Count() }));

                    var proname = "";
                    proname = new B2bComProData().GetProName(proid);

                    B2b_order_busRemindSms remindsms = new B2b_order_busRemindSmsData().GetB2b_order_busRemindSms(proid, gooutdate);
                    string licenceplate = "";//牌照
                    string telphone = "";//司机电话
                    if (remindsms != null)
                    {
                        licenceplate = remindsms.licenceplate;
                        telphone = remindsms.telphone;
                    }
                    //得到发送日志
                    int isreminded = 0;//是否发送过
                    string sendtophones = "";//发送的手机号列表字符串
                    B2b_order_busRemindSmsLog remindsmslog = new B2b_order_busRemindSmsLogData().GetB2b_order_busRemindSmsSucLog(proid, gooutdate);
                    if (remindsmslog != null)
                    {
                        if (remindsmslog.issuc == 1)
                        {
                            isreminded = 1;
                            sendtophones = remindsmslog.sendtophones;
                        }
                    }

                    //得到给送车员的发送日志
                    int issendtodriver = 0;
                    B2b_order_busNamelistSendLog sendtodriverlog = new B2b_order_busNamelistSendLogData().GetB2b_order_busNamelistSendSucLog(proid, gooutdate);
                    if (sendtodriverlog != null)
                    {
                        issendtodriver = 1;
                    }

                    return JsonConvert.SerializeObject(new { type = 100, groupbydata = groupbypickuppoint, proname = proname, gooutdate = gooutdate, msg = list, totalcount = list.Count, licenceplate = licenceplate, telphone = telphone, isreminded = isreminded, sendtophones = sendtophones, issendtodriver = issendtodriver });

                    //IEnumerable result = "";
                    //result = from m in list
                    //         select new
                    //         {
                    //             m.id,
                    //             m.orderid,
                    //             m.agentid,
                    //             m.comid,
                    //             m.IdCard,
                    //             m.name,
                    //             m.Nation,
                    //             m.proid,
                    //             m.travel_time,
                    //             m.yuding_name,
                    //             m.yuding_num,
                    //             m.yuding_phone,
                    //             m.yuding_time,
                    //             m.pickuppoint,
                    //             m.dropoffpoint,
                    //             agentname = GetAgentCompany(m.agentid)
                    //         };

                    //return JsonConvert.SerializeObject(new { type = 100, msg = result });
                }
            }
        }

        public static string travelbusordertravalerdetail(string gooutdate, int proid, int order_state)
        {
            List<b2b_order_busNamelist> list = new B2bOrderData().travelbusordertravalerdetail(gooutdate, proid, order_state);

            if (list == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
            else
            {
                if (list.Count == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
                else
                {
                    var groupbypickuppoint = list.GroupBy(a => a.pickuppoint).Select(g => (new { pickuppoint = g.Key, count = g.Count() }));

                    var proname = "";
                    proname = new B2bComProData().GetProName(proid);

                    B2b_order_busRemindSms remindsms = new B2b_order_busRemindSmsData().GetB2b_order_busRemindSms(proid, gooutdate);
                    string licenceplate = "";//牌照
                    string telphone = "";//司机电话
                    if (remindsms != null)
                    {
                        licenceplate = remindsms.licenceplate;
                        telphone = remindsms.telphone;
                    }
                    //得到发送日志
                    int isreminded = 0;//是否发送过
                    string sendtophones = "";//发送的手机号列表字符串
                    B2b_order_busRemindSmsLog remindsmslog = new B2b_order_busRemindSmsLogData().GetB2b_order_busRemindSmsSucLog(proid, gooutdate);
                    if (remindsmslog != null)
                    {
                        if (remindsmslog.issuc == 1)
                        {
                            isreminded = 1;
                            sendtophones = remindsmslog.sendtophones;
                        }
                    }

                    return JsonConvert.SerializeObject(new { type = 100, groupbydata = groupbypickuppoint, proname = proname, gooutdate = gooutdate, msg = list, totalcount = list.Count, licenceplate = licenceplate, telphone = telphone, isreminded = isreminded, sendtophones = sendtophones });

                    //return JsonConvert.SerializeObject(new { type = 100, msg = list });
                }
            }
        }

        private static string GetAgentCompany(int agentid)
        {
            if (agentid == 0)
            {
                return "直销网站销售";
            }
            else
            {
                Agent_company acompany = AgentCompanyData.GetAgentByid(agentid);
                if (acompany == null)
                {
                    return "..";
                }
                else
                {
                    return acompany.Company;
                }
            }


        }
        /// <summary>
        /// 特定时间段 分销旅游大巴订单统计
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="servertype"></param>
        /// <param name="comid"></param>
        /// <param name="agentid"></param>
        /// <returns></returns>
        public static string agenttravelbusordercountbyday(DateTime startdate, DateTime enddate, int servertype, int comid, int agentid)
        {
            List<DateTime> list = new List<DateTime>();
            for (int i = 0; i < (enddate - startdate).Days + 1; i++)
            {
                list.Add(startdate.AddDays(i));
            }
            if (list.Count > 0)
            {
                IEnumerable result = "";
                if (list != null)
                {
                    result = from m in list
                             select new
                             {
                                 daydate = m.ToString("yyyy-MM-dd"),
                                 //支付成功人数
                                 paysucnum = new B2bOrderData().GetPaysucNumByServertype(m, servertype, comid, (int)PayStatus.HasPay, agentid, "4,22"),
                                 ////结团人数
                                 //closeteamnu = new B2bOrderData().GetCloseTeamNumByServertype(m, servertype, comid, (int)OrderStatus.HasFin,agentid),
                             };
                    return JsonConvert.SerializeObject(new { type = 100, msg = result });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }
        /// <summary>
        /// 根据出行日期得到产品列表
        /// </summary>
        /// <param name="daydate"></param>
        /// <param name="servertype"></param>
        /// <param name="comid"></param>
        /// <param name="agentid"></param>
        /// <returns></returns>
        public static string agentGetb2bcomprobytraveldate(DateTime daydate, int servertype, int comid, int agentid)
        {
            IList<B2b_com_pro> list = new B2bComProData().Getb2bcomprobytraveldate(daydate, servertype, comid);
            if (list == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
            else
            {
                if (list.Count > 0)
                {
                    IEnumerable result = "";
                    result = from m in list
                             select new
                             {
                                 Proid = m.Id,
                                 Proname = m.Pro_name,
                                 //支付成功人数
                                 paysucbooknum = new B2bOrderData().GetPaySucNumByProid(m.Id, comid, (int)PayStatus.HasPay, daydate, agentid, "4,22"),
                                 ////结团人数
                                 //closeteamnum = new B2bOrderData().GetCloseTeamNumByProid(m.Id, comid, (int)OrderStatus.HasFin, daydate),
                                 ////是否已经派车
                                 //ishasplanbus = new Travelbusorder_operlogData().Ishasplanbus(m.Id, daydate),
                                 ////派车的车辆情况
                                 //busdetail = new Travelbusorder_sendbusData().BusDetailstr(m.Id, daydate)
                             };
                    return JsonConvert.SerializeObject(new { type = 100, msg = result });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
            }
        }
        public static string agenttravelbustravelerlistBypaystate(string gooutdate, int proid, int paystate, int agentid, string orderstate = "")
        {
            IList<b2b_order_busNamelist> list = new B2bOrderData().travelbustravelerlistBypaystate(gooutdate, proid, paystate, agentid, orderstate);
            if (list == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
            else
            {
                if (list.Count == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = list });
                    //IEnumerable result = "";
                    //result = from m in list
                    //         select new
                    //         {
                    //             m.id,
                    //             m.orderid,
                    //             m.agentid,
                    //             m.comid,
                    //             m.IdCard,
                    //             m.name,
                    //             m.Nation,
                    //             m.proid,
                    //             m.travel_time,
                    //             m.yuding_name,
                    //             m.yuding_num,
                    //             m.yuding_phone,
                    //             m.yuding_time,
                    //             m.pickuppoint,
                    //             m.dropoffpoint,
                    //             agentname = GetAgentCompany(m.agentid)
                    //         };

                    //return JsonConvert.SerializeObject(new { type = 100, msg = result });
                }
            }
        }

        //返回特定产品是否是否对分销销售权 返回0未授权，
        public static int ProAgentWarrantstate(int agentid, int proid, int comid)
        {
            if (agentid == 0 || proid == 0)
            {
                return 0;
            }
            else
            {
                Agent_company acompany = AgentCompanyData.GetAgentByid(agentid);
                if (acompany == null)
                {
                    return 0;
                }
                else
                {
                    var prodata = new B2bComProData();
                    var pro = prodata.GetProById(proid.ToString());
                    if (pro == null)
                    {
                        return 0;
                    }

                    //分销商已授权
                    if (pro.Viewmethod == 1 || pro.Viewmethod == 2)
                    {
                        return 1;
                    }
                    else
                    {
                        //分销商未授权 查询是否有特定授权
                        AgentCompanyData agentdata = new AgentCompanyData();
                        return agentdata.SearchSetWarrant(agentid, proid, comid);
                    }

                }
            }

        }


        #region 根据微信号得到用户的订单列表
        public static string SaveAddressPageList(int agentid, int pageindex, int pagesize, string key)
        {
            var totalcount = 0;
            try
            {
                if (agentid == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, totalCount = 0, msg = "没有权限" });
                }
                else
                {

                    var orderdata = new B2bOrderData();
                    var list = orderdata.SaveAddressPageList(agentid, pageindex, pagesize, key, out totalcount);
                    return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = list });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        //删除常用地址
        public static string DelSaveAddress(int id, int agentid)
        {
            var data = new B2bOrderData().DelSaveAddress(id, agentid);
            if (data > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = "删除成功" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "错误" });
            }
        }



        public static string GetShopCartExpressfee(string proidstr, string citystr, string numstr)
        {
            using (var helper = new SqlHelper())
            {
                try
                {
                    string feedetail = "";
                    var fee = new B2b_delivery_costData().Getdeliverycost_ShopCart(proidstr, citystr, numstr, out feedetail);

                    if (fee != -1)
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = fee, feedetail = feedetail });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "计算运费错误" });
                    }

                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                }
            }
        }



        //插入购物车，此ORDER都是购物车使用
        public static string InsertCart(B2b_order order)
        {

            var data = 0;
            var redata = 0;
            //首先查询是否购物车有此产品，如果有 则 增加数量，如果没有则插入
            if (order.Agentid != 0)
            {
                data = new B2bOrderData().SearchCart(order.Comid, order.Agentid, order.Pro_id, order.Speciid);
                order.Id = data;

                redata = new B2bOrderData().InsertCart(order);
            }
            else
            {
                data = new B2bOrderData().SearchUserCart(order.Comid, order.Openid, order.Pro_id, order.Speciid);
                order.Id = data;

                redata = new B2bOrderData().InsertUserCart(order);
            }
            if (redata > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = "插入购物车成功" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "插入失败" });
            }
        }


        //插入购物车，此ORDER都是购物车使用
        public static string UpCartNum(B2b_order order)
        {
            var redata = 0;
            if (order.Agentid != 0)
            {
                redata = new B2bOrderData().UpCartNum(order);
            }
            else
            {
                redata = new B2bOrderData().UpUserCartNum(order);
            }
            if (redata > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = "插入购物车成功" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "插入失败" });
            }
        }

        //删除购物车
        public static string DelCart(B2b_order order, string cartid)
        {
            var redata = 0;

            if (cartid != "")
            {
                if (cartid.Substring(cartid.Length - 1, 1) == ",")
                {
                    cartid = cartid.Substring(0, cartid.Length - 1);
                }
            }


            if (order.Agentid != 0)
            {
                redata = new B2bOrderData().DelCart(order, cartid);
            }
            else
            {
                redata = new B2bOrderData().DelUserCart(order, cartid);
            }
            if (redata > 0)
            {
                var total = new B2bOrderData().SearchCartCount(order.Comid, order.Agentid);
                return JsonConvert.SerializeObject(new { type = 100, msg = "删除成功", total = total });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "删除失败" });
            }
        }


        //查询是否有购物车
        public static string SearchCart(B2b_order order)
        {
            var data = 0;
            if (order.Agentid != 0)
            {
                //首先查询是否购物车有产品
                data = new B2bOrderData().SearchCart(order.Comid, order.Agentid, -1, 0);
            }
            else
            {
                //首先查询是否购物车有产品
                data = new B2bOrderData().SearchUserCart(order.Comid, order.Openid, -1, 0);

            }
            if (data > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = data });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "null" });
            }
        }

        //查询购物车产品数量

        public static string SearchCartCount(int comid, int agentid)
        {
            //首先查询是否购物车有此产品，如果有 则 增加数量

            var data = new B2bOrderData().SearchCartCount(comid, agentid);
            if (data > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = data });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "null" });
            }

        }



        //查询是否有购物车
        public static string SearchCartList(B2b_order order, string proid, string cartid)
        {
            //首先查询是否购物车有此产品，如果有 则 增加数量，如果没有则插入
            try
            {

                var prodata = new B2bComProData();
                int Warrant_type = 0;

                int Agentlevel = 3;
                var agentmodel = AgentCompanyData.GetAgentWarrant(order.Agentid, order.Comid);
                if (agentmodel != null)
                {
                    Agentlevel = agentmodel.Warrant_level;
                    Warrant_type = agentmodel.Warrant_type;
                }



                var list = new B2bOrderData().SearchCartList(order.Comid, order.Agentid, proid, cartid);
                decimal totalprice = 0;//购物车中产品的总价格

                IEnumerable result = "";
                if (list != null)
                {
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Pro_name = pro.Speciid == 0 ? pro.Pro_name : pro.Pro_name + new B2bOrderData().SearchUserCartSpeciNameList(order.Comid, pro.Cartid, pro.Speciid, "speci_name"),
                                 Face_price = pro.Speciid == 0 ? pro.Face_price : new B2bOrderData().SearchUserCartSpeciPrcieList(order.Comid, pro.Cartid, pro.Speciid, "speci_face_price"),
                                 Advise_price = pro.Speciid == 0 ? pro.Advise_price : new B2bOrderData().SearchUserCartSpeciPrcieList(order.Comid, pro.Cartid, pro.Speciid, "speci_advise_price"),
                                 Agentsettle_price = pro.Speciid == 0 ? pro.Agentsettle_price : new B2bOrderData().SearchUserCartSpeciPrcieList(order.Comid, pro.Cartid, pro.Speciid, "speci_agentsettle_price"),
                                 Pro_end = pro.Pro_end,
                                 Pro_start = pro.Pro_start,
                                 Source_type = pro.Source_type == 1 ? "自动生成" : "倒码",
                                 Pro_state = pro.Pro_state == 0 ? "下线" : "上线",

                                 Agent_price = Agentlevel == 1 ? pro.Speciid == 0 ? pro.Agent1_price : new B2bOrderData().SearchUserCartSpeciPrcieList(order.Comid, pro.Cartid, pro.Speciid, "speci_agent1_price") : Agentlevel == 2 ? pro.Speciid == 0 ? pro.Agent2_price : new B2bOrderData().SearchUserCartSpeciPrcieList(order.Comid, pro.Cartid, pro.Speciid, "speci_agent2_price") : pro.Speciid == 0 ? pro.Agent3_price : new B2bOrderData().SearchUserCartSpeciPrcieList(order.Comid, pro.Cartid, pro.Speciid, "speci_agent3_price"),
                                 //Projectname = new B2b_com_projectData().GetProjectNameByid(pro.Projectid),
                                 Pro_youxiaoqi = pro.Pro_youxiaoqi,
                                 ProValidateMethod = pro.ProValidateMethod,
                                 Appointdata = pro.Appointdata,
                                 Iscanuseonsameday = pro.Iscanuseonsameday,
                                 Server_type = pro.Server_type,
                                 U_num = pro.U_num,
                                 ProImg = FileSerivce.GetImgUrl(pro.Imgurl),
                                 Cartid = pro.Cartid,
                                 Speciid = pro.Speciid
                             };

                    totalprice = new B2bOrderData().GetAgentCartTotalPrice(order.Comid, order.Agentid, proid, Agentlevel);
                }
                return JsonConvert.SerializeObject(new { type = 100, msg = result, totalprice = totalprice });


            }
            catch
            {
                return null;
            }
        }


        //查询是否有购物车
        public static string SearchUserCartList(B2b_order order, string proid, string speciid)
        {
            //首先查询是否购物车有此产品，如果有 则 增加数量，如果没有则插入
            try
            {

                var list = new B2bOrderData().SearchUserCartList(order.Comid, order.Openid, proid, speciid);

                IEnumerable result = "";
                if (list != null)
                {
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Pro_name = pro.Speciid == 0 ? pro.Pro_name : pro.Pro_name + new B2bOrderData().SearchUserCartSpeciNameList(order.Comid, pro.Cartid, pro.Speciid, "speci_name"),
                                 Imgurl = FileSerivce.GetImgUrl(pro.Imgurl),
                                 Face_price = pro.Speciid == 0 ? pro.Face_price : new B2bOrderData().SearchUserCartSpeciPrcieList(order.Comid, pro.Cartid, pro.Speciid, "speci_face_price"),
                                 Advise_price = pro.Speciid == 0 ? pro.Advise_price : new B2bOrderData().SearchUserCartSpeciPrcieList(order.Comid, pro.Cartid, pro.Speciid, "speci_advise_price"),
                                 Agentsettle_price = pro.Speciid == 0 ? pro.Agentsettle_price : new B2bOrderData().SearchUserCartSpeciPrcieList(order.Comid, pro.Cartid, pro.Speciid, "speci_agentsettle_price"),
                                 Pro_end = pro.Pro_end,
                                 Pro_start = pro.Pro_start,
                                 Source_type = pro.Source_type == 1 ? "自动生成" : "倒码",
                                 Pro_state = pro.Pro_state == 0 ? "下线" : "上线",
                                 //Projectname = new B2b_com_projectData().GetProjectNameByid(pro.Projectid),
                                 Pro_youxiaoqi = pro.Pro_youxiaoqi,
                                 ProValidateMethod = pro.ProValidateMethod,
                                 Appointdata = pro.Appointdata,
                                 Iscanuseonsameday = pro.Iscanuseonsameday,
                                 U_num = pro.U_num,
                                 Cartid = pro.Cartid,
                                 Speciid = pro.Speciid
                             };
                }
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch
            {
                return null;
            }
        }

        //查询是订单产品
        public static string SearchUserOrderList(int Comid, int cartid, string proid)
        {
            //首先查询是否购物车有此产品，如果有 则 增加数量，如果没有则插入
            try
            {

                var list = new B2bOrderData().SearchUserOrderList(Comid, cartid, proid);

                IEnumerable result = "";
                if (list != null)
                {
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Pro_name = pro.Pro_name,
                                 Imgurl = FileSerivce.GetImgUrl(pro.Imgurl),
                                 Face_price = pro.Face_price,
                                 Advise_price = pro.Advise_price,
                                 Agentsettle_price = pro.Agentsettle_price,
                                 Pro_end = pro.Pro_end,
                                 Pro_start = pro.Pro_start,
                                 Source_type = pro.Source_type == 1 ? "自动生成" : "倒码",
                                 Pro_state = pro.Pro_state == 0 ? "下线" : "上线",
                                 //Projectname = new B2b_com_projectData().GetProjectNameByid(pro.Projectid),
                                 Pro_youxiaoqi = pro.Pro_youxiaoqi,
                                 ProValidateMethod = pro.ProValidateMethod,
                                 Appointdata = pro.Appointdata,
                                 Iscanuseonsameday = pro.Iscanuseonsameday,
                                 U_num = pro.U_num
                             };
                }
                return JsonConvert.SerializeObject(new { type = 100, msg = result });


            }
            catch
            {
                return null;
            }
        }


        public static string SumCartPrice(int comid, string userid, string cartid)
        {
            //首先查询是否购物车有此产品，如果有 则 增加数量，如果没有则插入
            var data = new B2bOrderData().SumCartPrice(comid, userid, cartid);//查询非规格产品




            if (data > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = data });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "null" });
            }
        }


        /// <summary>
        /// 编辑分销商常用收货地址
        /// </summary>
        /// <param name="addrid"></param>
        /// <param name="agentid"></param>
        /// <param name="u_name"></param>
        /// <param name="u_phone"></param>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="address"></param>
        /// <param name="txtcode"></param>
        /// <returns></returns>
        public static string SaveAddress(int addrid, int agentid, string u_name, string u_phone, string province, string city, string address, string txtcode)
        {

            B2b_address maddr = new B2b_address()
            {
                Id = addrid,
                U_name = u_name,
                U_phone = u_phone,
                Agentid = agentid,     //分销ID
                Province = province,
                City = city,
                Address = address,
                Code = txtcode
            };
            var data = new B2bOrderData().SaveAddress(maddr);

            return JsonConvert.SerializeObject(new { type = 100, msg = data });
        }

        public static string Getagentaddrbyid(int addrid)
        {
            B2b_address m_addr = new B2bOrderData().Getagentaddrbyid(addrid);

            return JsonConvert.SerializeObject(new { type = 100, msg = m_addr });
        }

        public static string Deladdr(int addrid)
        {
            int m_addr = new B2bOrderData().Deladdr(addrid);

            return JsonConvert.SerializeObject(new { type = 100, msg = m_addr });
        }

        public static string Uporderstate(int orderid, int orderstate)
        {
            int m_addr = new B2bOrderData().Uporderstate(orderid, orderstate);

            return JsonConvert.SerializeObject(new { type = 100, msg = m_addr });
        }


        //对分销充值后，判断是否进行绑定订单二次确认
        public static string agentorderpay_Hand(int orderid)
        {
            //对分销充值订单支付返回，对绑定订单进行处理确认
            B2bOrderData dataorder = new B2bOrderData();
            B2b_order modelb2border = dataorder.GetOrderById(orderid);
            if (modelb2border != null)
            {
                if (modelb2border.Order_type == 2)//1普通订单，2为 充值订单
                {
                    if (modelb2border.Handlid > 0)
                    {
                        var Handdata = agentorder_shoudongchuli(modelb2border.Handlid);
                    }
                }
            }
            return "";
        }

        //对分销订单，重新处理（分销已充值后）,或点击结算
        public static string agentorder_shoudongchuli(int orderid)
        {

            lock (lockobj)
            {
                string dikou = "";
                int bindingorderid_huoqu = 0; //分销导入订单返回值
                //根据订单id得到订单信息
                B2bOrderData dataorder = new B2bOrderData();
                B2b_order modelb2border = dataorder.GetOrderById(orderid);
                if (modelb2border == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "订单信息错误" });
                }

                //读取分销商信息
                Agent_company agentinfo = AgentCompanyData.GetAgentWarrant(modelb2border.Agentid, modelb2border.Comid);
                if (agentinfo == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "分销账户错误" });
                }

                B2b_com_pro pro_t = new B2bComProData().GetProById(modelb2border.Pro_id.ToString());
                if (pro_t == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "产品信息错误" });
                }

                var TabaoOrderdata = new Taobao_send_noticelogData().GetSendNoticeBySelfOrderid(orderid.ToString());
                //if (TabaoOrderdata != null)
                //{
                //    //这个要改，以后手动也可以结算，淘宝2小时限制。
                //    return JsonConvert.SerializeObject(new { type = 1, msg = "淘宝码商订单，不能结算，请充足预付款后让客户重新提单，此笔淘宝交易2小时候自动退款。" });
                //}



                //计算分销余额
                decimal overmoney = agentinfo.Imprest - modelb2border.Pay_price * modelb2border.U_num - modelb2border.Express + modelb2border.childreduce * modelb2border.Child_u_num;


                if (modelb2border == null)
                {
                    dikou = "没有查询到此笔订单";
                    return JsonConvert.SerializeObject(new { type = 1, msg = dikou });
                }
                if (modelb2border.Order_type == 1)//1普通订单，2为 充值订单
                {
                    if (modelb2border.Order_state == 1)//必须是未处理订单
                    {
                        if (modelb2border.Warrant_type == 1)//1.出票扣款 2.验码扣款
                        {

                            //每笔订单金额必须不能超出预付款金额
                            if ((agentinfo.Imprest + agentinfo.Credit) < (modelb2border.Pay_price * modelb2border.U_num + modelb2border.Express - modelb2border.childreduce * modelb2border.Child_u_num) && modelb2border.Warrant_type == 1 && modelb2border.Order_type == 1)
                            {
                                decimal pay_money = 0;
                                //因上面判断是 预付款+信用额 不足，可实际支付时不考虑信用额。已经欠款的分销支付 不增加欠款为原则。
                                //判断预付款金额，如果无预付款（或负预付款）则支付订单金额，如果有预付款则扣减预付款的金额
                                if (agentinfo.Imprest > 0)
                                {
                                    pay_money = (modelb2border.Pay_price * modelb2border.U_num + modelb2border.Express - modelb2border.childreduce * modelb2border.Child_u_num) - agentinfo.Imprest;
                                }
                                else
                                {
                                    pay_money = (modelb2border.Pay_price * modelb2border.U_num + modelb2border.Express - modelb2border.childreduce * modelb2border.Child_u_num);
                                }

                                //money 为需要支付的金额。不考虑信用额最高为订单金额 预付款不足=yfkbz
                                dikou = "预付款不足";
                                return JsonConvert.SerializeObject(new { type = 1, msg = dikou });
                            }


                            #region 当提交订单时，不管来路，如果订单处理成功进入发码流程前,扣款前,提交一笔原始分销订单。
                            if (pro_t.Source_type == 4)
                            {
                                int pro_id_old = pro_t.Bindingid;//原产品ID
                                int comid_old = modelb2border.Comid;//提交订单商户ID
                                int agentid_old = 0;//提交订单商户绑定的分销id
                                B2bCompanyData comdata = new B2bCompanyData();
                                var cominfo = comdata.GetCompanyBasicById(comid_old);
                                if (cominfo != null)
                                {
                                    agentid_old = cominfo.Bindingagent;
                                }

                                var daoruguigeSpeciid = modelb2border.Speciid;
                                //重新读取绑定的规格
                                if (modelb2border.Speciid != 0)
                                {
                                    var guiginfo = B2b_com_pro_SpeciData.Getgginfobyggid(modelb2border.Speciid);
                                    if (guiginfo != null)
                                    {
                                        daoruguigeSpeciid = guiginfo.binding_id;
                                    }
                                }

                                //-----注意:如果旅游产品做成可以导入的话，需要传递参数 儿童数量，暂时没加----
                                AgentOrder(agentid_old, pro_id_old.ToString(), "1", modelb2border.U_num.ToString(), modelb2border.U_name, modelb2border.U_phone, modelb2border.U_traveldate.ToString(), "", 1, out bindingorderid_huoqu, 0, 1, modelb2border.pickuppoint, modelb2border.dropoffpoint, "", 0, "", "", "", "", 0, 0, 0, modelb2border.yanzheng_method, daoruguigeSpeciid, modelb2border.baoxiannames, modelb2border.baoxianpinyinnames, modelb2border.baoxianidcards, 0, modelb2border.M_b2b_order_hotel, modelb2border.Id, modelb2border.travelnames, modelb2border.travelidcards, modelb2border.travelnations, modelb2border.travelphones, modelb2border.travelremarks,modelb2border.U_idcard);
                                //bindingorderid 是分销返回的订单号，必须成功否则报错
                                //导入产品，如果提交订单错误，则可能是分销产品下线，分销钱不够，或者其他问题没有提交订单
                                if (bindingorderid_huoqu == 0)
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "提交订单错误，请联系商家！" });
                                }

                                B2b_order agentsunorder = dataorder.GetOrderById(bindingorderid_huoqu);
                                if (agentsunorder == null)
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "提交订单错误，请联系商家！" });
                                }
                                if (agentsunorder.Pay_state == 1)
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "提交订单错误，商家额度不足，请联系商家！" });
                                }


                                modelb2border.Bindingagentorderid = bindingorderid_huoqu;
                                dataorder.InsertOrUpdate(modelb2border);
                            }
                            #endregion


                            //分销商财务扣款
                            Agent_Financial Financialinfo = new Agent_Financial
                            {
                                Id = 0,
                                Com_id = modelb2border.Comid,
                                Agentid = modelb2border.Agentid,
                                Warrantid = modelb2border.Warrantid,
                                Order_id = modelb2border.Id,
                                Servicesname = pro_t.Pro_name + "[" + orderid + "]",
                                SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                                Money = 0 - modelb2border.Pay_price * modelb2border.U_num - modelb2border.Express + modelb2border.Child_u_num * modelb2border.childreduce,
                                Payment = 1,            //收支(0=收款,1=支出)
                                Payment_type = "分销扣款",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点,分销扣款）
                                Money_come = "分销账户",         //资金来源（网上支付,银行收款，分销账户等）
                                Over_money = overmoney
                            };
                            var fin = AgentCompanyData.AgentFinancial(Financialinfo);

                            modelb2border.Pay_state = 2;
                            modelb2border.Order_state = 2;
                            //修改此订单的支付状态为“支付成功” ,订单状态为“已付款”
                            dataorder.InsertOrUpdate(modelb2border);


                            //扣除商户分销订单手续费
                            var KouchuShouxufei_temp = OrderJsonData.KouchuShouxufei(modelb2border);


                            //订单发码
                            if (pro_t != null)
                            {
                                dikou = new SendEticketData().SendEticket(orderid, 1);
                            }



                        }
                        else
                        {
                            #region 当提交订单时，不管来路，如果订单处理成功进入发码流程前,扣款前,提交一笔原始分销订单。
                            if (pro_t.Source_type == 4)
                            {
                                int pro_id_old = pro_t.Bindingid;//原产品ID
                                int comid_old = modelb2border.Comid;//提交订单商户ID
                                int agentid_old = 0;//提交订单商户绑定的分销id
                                B2bCompanyData comdata = new B2bCompanyData();
                                var cominfo = comdata.GetCompanyBasicById(comid_old);
                                if (cominfo != null)
                                {
                                    agentid_old = cominfo.Bindingagent;
                                }
                                var daoruguigeSpeciid = modelb2border.Speciid;
                                //重新读取绑定的规格
                                if (modelb2border.Speciid != 0)
                                {
                                    var guiginfo = B2b_com_pro_SpeciData.Getgginfobyggid(modelb2border.Speciid);
                                    if (guiginfo != null)
                                    {
                                        daoruguigeSpeciid = guiginfo.binding_id;
                                    }
                                }

                                //-----注意:如果旅游产品做成可以导入的话，需要传递参数 儿童数量，暂时没加----
                                AgentOrder(agentid_old, pro_id_old.ToString(), "1", modelb2border.U_num.ToString(), modelb2border.U_name, modelb2border.U_phone, modelb2border.U_traveldate.ToString(), "", 1, out bindingorderid_huoqu, 0, 1, modelb2border.pickuppoint, modelb2border.dropoffpoint, "", 0, "", "", "", "", 0, 0, 0, modelb2border.yanzheng_method, daoruguigeSpeciid, modelb2border.baoxiannames, modelb2border.baoxianpinyinnames, modelb2border.baoxianidcards, 0, null, modelb2border.Id, modelb2border.travelnames, modelb2border.travelidcards, modelb2border.travelnations, modelb2border.travelphones, modelb2border.travelremarks, modelb2border.U_idcard);
                                //bindingorderid 是分销返回的订单号，必须成功否则报错
                                //导入产品，如果提交订单错误，则可能是分销产品下线，分销钱不够，或者其他问题没有提交订单
                                if (bindingorderid_huoqu == 0)
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "提交订单错误，请联系商家！" });
                                }
                                B2b_order agentsunorder = dataorder.GetOrderById(bindingorderid_huoqu);
                                if (agentsunorder == null)
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "提交订单错误，请联系商家！" });
                                }
                                if (agentsunorder.Pay_state == 1)
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "提交订单错误，商家额度不足，请联系商家！" });
                                }
                                modelb2border.Bindingagentorderid = bindingorderid_huoqu;
                                dataorder.InsertOrUpdate(modelb2border);
                            }
                            #endregion

                            //---------------新增1begin--------------//

                            //如果是对外接口的请求，则处理订单和发码

                            modelb2border.Pay_state = 2; //对于验码时扣款，此笔订单应该如何支付状态应该如何处理。
                            modelb2border.Order_state = 2;
                            //修改此订单的支付状态为“支付成功” ,订单状态为“已付款”
                            dataorder.InsertOrUpdate(modelb2border);

                            dikou = new SendEticketData().SendEticket(orderid, 1);


                        }
                    }
                }
                else
                {//成功提交充值订单
                    dikou = "订单提交成功";
                }

                //得到订单状态
                int dorderstatus = new B2bOrderData().GetOrderState(orderid.ToString());
                if (dorderstatus == 2 || dorderstatus == 4)
                {
                    #region 赠送保险
                    OrderJsonData.ZengsongBaoxian(orderid);
                    #endregion
                }


                SendEticketData sendate = new SendEticketData();
                string pno = "";
                if (pro_t != null)
                {
                    if (pro_t.Source_type == 3)//如果是借口产品，按接口方式读码selservice
                    {
                        if (pro_t.Serviceid == 4)
                        { //如果是接口产品
                            B2b_company commanage = B2bCompanyData.GetAllComMsg(modelb2border.Comid);
                            WlGetProInfoDealRequestData wldata = new WlGetProInfoDealRequestData(commanage.B2bcompanyinfo.wl_PartnerId, commanage.B2bcompanyinfo.wl_userkey);
                            var wlorderinfo = wldata.SearchWlOrderData(modelb2border.Comid, 0, "", orderid);
                            if (wlorderinfo != null)
                            {
                                pno = wlorderinfo.vouchers;
                            }
                        }
                    }
                    else
                    {//如果不是借口，则按自己规则读码

                        pno = sendate.HuoQuEticketPno(orderid);
                    }
                }


                return JsonConvert.SerializeObject(new { type = 100, msg = dikou,pno=pno });
            }
        }




        //对优惠券产品（赠送产品）直接提单
        public static int insertyuohuiquan(int proid, string openid)
        {

            int num = 1;
            decimal price = 0;
            decimal cost = 0;
            int orderid = 0;
            int u_id = 0;
            //helper.BeginTrancation();
            B2bComProData prodata = new B2bComProData();
            B2bCrmData crmdata = new B2bCrmData();

            //读取COMID
            var comid = prodata.GetComidByProid(proid);

            //读取产品
            B2b_com_pro pro_t = new B2bComProData().GetProById(proid.ToString());

            if (pro_t == null)
            {
                return 0;
            }


            if (pro_t.Ispanicbuy == 1 || pro_t.Ispanicbuy == 2)
            {

                //抢购和限购产品，只能订购一次，用此微信号查询是否有订购
                if (openid != "")
                {
                    B2bOrderData dataorder = new B2bOrderData();
                    var order_id_temp = dataorder.GetOrderIdByWeixin(openid, comid, proid);
                    if (order_id_temp != 0)
                    {
                        B2bCrmData userdate = new B2bCrmData();
                        var pass = userdate.WeixinGetPass(openid, comid);

                        //根据访问获得公司信息
                        WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(comid);


                        //短信内容发送微信客服通道
                        var data = CustomerMsg_Send.SendWxMsg(comid, openid, 1, "", "您已经获得本赠送产品，请在 “<a href='http://shop" + comid + ".etown.cn/h5/order/order.aspx?&openid=" + openid + "&weixinpass=" + pass + "'>我的订单</a>” 中查看。", "", basicc.Weixinno);
                        return 0;
                    }
                }
            }


            price = pro_t.Advise_price;
            cost = pro_t.Agentsettle_price;


            B2b_order order = new B2b_order()
            {
                Id = 0,
                Pro_id = proid,
                Speciid = 0,
                Order_type = 1,
                U_name = "",
                U_id = 0,
                U_phone = "",
                U_num = num,
                U_subdate = DateTime.Now,
                Payid = 0,
                Pay_price = price,
                Cost = cost,
                Profit = 0,
                Order_state = (int)OrderStatus.WaitPay,//
                Pay_state = (int)PayStatus.NotPay,//支付模块未做出来前先设置为已支付
                Send_state = (int)SendCodeStatus.NotSend,
                Ticketcode = 0,//电子码未创建，支付后产生码赋值
                Rebate = 0,//  利润返佣金额暂时定为0
                Ordercome = "",//订购来源 暂时定为空
                U_traveldate = DateTime.Now,
                Dealer = "自动",
                Comid = comid,
                Pno = "",
                Openid = openid,
                Ticketinfo = "",

                Integral1 = 0,//积分
                Imprest1 = 0,//预付款
                Agentid = 0,     //分销ID
                Warrantid = 0,   //授权ID
                Warrant_type = 1,  //支付类型分销 1出票扣款 2验码扣款

                pickuppoint = "",
                dropoffpoint = "",

                Order_remark = ""
            };

            try
            {
                var data = OrderJsonData.InsertOrUpdate(order, out orderid);
                return orderid;
            }
            catch
            {
                return 0;
            }

        }


        public static string Insnuomi_dealid(int orderid, int nuomidealid)
        {
            int r = new B2bOrderData().InsNuomiDealid(orderid, nuomidealid);
            if (r > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = "" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }

        }

        public static string Getorderdetail(int orderid)
        {
            B2b_order morder = new B2bOrderData().GetOrderById(orderid);
            if (morder != null)
            {
                List<B2b_order> list = new List<B2b_order>();
                list.Add(morder);
                IEnumerable r = "";
                r = from m in list
                    select new
                    {
                        m.Id,
                        m.Pro_id,
                        Pro_name = new B2bComProData().GetProName(m.Pro_id),
                        Totalnum = m.U_num,//总计
                        Cancelnum = m.Cancelnum,//退款数量
                        CanUsenum = m.U_num - new B2bOrderData().GetConsumeNum(m.Id),  //剩余可用数量
                        ConsumeNum = new B2bOrderData().GetConsumeNum(m.Id),//得到消费数量
                        DeliveryFee = m.Express,//运费
                        SinglePrice = m.Pay_price,//单价
                        jifen = m.Integral1,//积分
                        yufu = m.Imprest1,//预付款
                        Pro_servertype = new B2bComProData().GetProServer_typeById(m.Pro_id.ToString()),
                        order_TotalFee = m.Pay_price * m.U_num - m.Integral1 - m.Imprest1 + m.Express, //单子总金额
                        TotalFee = new B2bPayData().GetPaytotalfee(m),//支付总金额
                        m.Shopcartid,//如果大于0则为购物车订单；否则为普通订单 
                        order_TotalCanQuitFee = m.Pay_price * (m.U_num - new B2bOrderData().GetConsumeNum(m.Id)) - m.Integral1 - m.Imprest1 + m.Express - Gethasquitfeebyoid(m),//单子最多可退款金额
                        //TotalCanQuitFee = GetTotalCanQuitFee(m)//最多可退款金额
                    };
                return JsonConvert.SerializeObject(new { type = 100, msg = r });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }


        /// <summary>
        /// 得到订单已经退款金额
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private static decimal Gethasquitfeebyoid(B2b_order m)
        {
            //得到支付信息
            B2b_pay mpay = new B2bPayData().GetSUCCESSPayById(m.Id);

            if (mpay != null)
            {
                //现在只支持微信退款 、支付宝退款，其他支付方式暂没有退款功能
                #region 微信剩余可退款金额=支付总金额-已通过微信支付退款的金额
                if (mpay.Pay_com == "wx")
                {
                    decimal hasquitfee = new B2b_pay_wxrefundlogData().Gettotalquitfeebyoid(m.Id);
                    return hasquitfee / 100;
                }
                #endregion
                #region 支付宝剩余可退款金额= 支付总金额-已通过支付宝退款的金额
                else if (mpay.Pay_com == "alipay" || mpay.Pay_com == "malipay")
                {
                    decimal hasquitfee = new B2b_pay_alipayrefundlogData().Gettotalquitfeebyoid(m.Id);
                    return hasquitfee;
                }
                else
                {
                    return 0;
                }
                #endregion
            }
            else
            {
                return 0;
            }
        }


        public static string GetPosLogList(int pageindex, int pagesize, string starttime, string key)
        {
            int totalcount = 0;
            List<Pos_log> list = new PoslogData().GetPosLogList(pageindex, pagesize, starttime, key, out totalcount);
            if (totalcount == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "", totalcount = 0 });
            }
            else
            {
                IEnumerable r;
                r = from p in list
                    select new
                    {
                        p.Id,
                        Str = p.Str.Length > 5 ? p.Str.Substring(5) : p.Str,
                        p.Subdate,
                        p.Uip,
                        p.ReturnStr,
                        p.ReturnSubdate,
                        Posid = GetPosid(p.Str),
                        Posopertype = GetPosopertype(p.Str),
                        PosRetstatus = GetPosRetstatus(p.ReturnStr),
                        PosReturnsinfo = GePosReturnsinfo(p.ReturnStr),
                        Pno = GetPosPno(p.Str),
                        Posdetail = GetPosdetail(GetPosid(p.Str))
                    };

                return JsonConvert.SerializeObject(new { type = 100, msg = r, totalcount = totalcount });
            }
        }

        private static string GetPosdetail(string posid)
        {
            if (posid != "")
            {
                B2b_company_info info = new B2bCompanyInfoData().PosInfobyposid(posid.ConvertTo<int>(0));
                if (info != null)
                {
                    string projectname = new B2b_com_projectData().GetProjectname(info.Projectid);
                    if (projectname == "")
                    {
                        projectname = "未绑定，通用项目";
                    }
                    return "公司:" + info.Poscompany + "-项目:" + projectname + "-备注:" + info.Remark;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        private static string GetPosPno(string xmlstr)
        {
            try
            {
                xmlstr = xmlstr.Substring(5);
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(xmlstr.Trim());
                XmlElement xroot = xdoc.DocumentElement;
                string request_type = xroot.SelectSingleNode("qrcode").InnerXml;
                return request_type;
            }
            catch
            {
                return "";
            }
        }

        private static string GePosReturnsinfo(string xmlstr)
        {
            if (xmlstr == "")
            {
                return "";
            }
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(xmlstr.Trim());
                XmlElement xroot = xdoc.DocumentElement;
                string Returnsinfo = xroot.SelectSingleNode("Returnsinfo").InnerXml;
                return Returnsinfo;
            }
            catch
            {
                return "";
            }
        }

        private static string GetPosRetstatus(string xmlstr)
        {
            if (xmlstr == "")
            {
                return "";
            }
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(xmlstr.Trim());
                XmlElement xroot = xdoc.DocumentElement;
                string status = xroot.SelectSingleNode("status").InnerXml;
                return status;
            }
            catch
            {
                return "";
            }
        }

        private static string GetPosopertype(string xmlstr)
        {
            try
            {
                xmlstr = xmlstr.Substring(5);
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(xmlstr.Trim());
                XmlElement xroot = xdoc.DocumentElement;
                string request_type = xroot.SelectSingleNode("request_type").InnerXml;
                return request_type;
            }
            catch
            {
                return "";
            }
        }

        private static string GetPosid(string xmlstr)
        {
            try
            {
                xmlstr = xmlstr.Substring(5);
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(xmlstr.Trim());
                XmlElement xroot = xdoc.DocumentElement;
                string posid = xroot.SelectSingleNode("pos_id").InnerXml;
                return posid;
            }
            catch
            {
                return "";
            }
        }

        public static string GetPosLogById(int logid)
        {
            Pos_log m = new PoslogData().GetPosLogById(logid);
            if (m == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = m });
            }
        }


        /// <summary>
        ///  扣分销分销手续费
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static int KouchuShouxufei(B2b_order modelb2border)
        {
            using (var helper = new SqlHelper())
            {

                try
                {
                    //-----------------------------------------------交易服务费 开始------------
                    //得到商家信息,账户余额
                    B2bFinanceData Financed = new B2bFinanceData();

                    B2b_com_pro pro_t = new B2bComProData().GetProById(modelb2border.Pro_id.ToString());

                    B2b_company modelcom = B2bCompanyData.GetCompany(modelb2border.Comid);

                    //当手续费为0时不操作
                    if (modelcom.ServiceFee == 0)
                    {
                        return 0;
                    }



                    //获得新总金额
                    decimal Over_money = modelcom.Imprest;

                    B2b_Finance Financebackinfo_1 = new B2b_Finance()
                    {
                        Id = 0,
                        Com_id = modelb2border.Comid,
                        Agent_id = modelb2border.Agentid,           //分销编号（默认为0）
                        Eid = 0,                //易城账户（默认为0，0=没有交易，1=和易城交易）
                        Order_id = modelb2border.Id,           //订单号（默认为0）
                        Servicesname = pro_t.Pro_name + "[" + modelb2border.Id + "]",       //交易名称/内容
                        SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                        Money = decimal.Round(0 - ((modelb2border.Pay_price * modelb2border.U_num + modelb2border.Express - modelb2border.Child_u_num * modelb2border.childreduce) * modelcom.ServiceFee), 2),              //金额
                        Payment = 1,            //收支(0=收款,1=支出)
                        Payment_type = "手续费",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点）
                        Money_come = "分销交易",         //资金来源（网上支付,银行收款等）
                        Over_money = decimal.Round(Over_money - ((modelb2border.Pay_price * modelb2border.U_num + modelb2border.Express - modelb2border.Child_u_num * modelb2border.childreduce) * modelcom.ServiceFee), 2)       //余额（根据商家，分销，易城，编号显示相应余额）
                    };
                    int finacebackid_1 = Financed.InsertOrUpdate(Financebackinfo_1);
                    //-----------------------------------------------交易服务费 结束------------
                    return finacebackid_1;
                }
                catch (Exception ex)
                {
                    helper.Rollback();
                    return 0;
                }
            }
        }



        /// <summary>
        ///  回滚分销分销手续费
        /// </summary>
        /// <param name="order"></param>
        ///  <param name="backprice"></param>
        /// <returns></returns>
        public static int HuigunShouxufei(B2b_order modelb2border, decimal backprice)
        {
            using (var helper = new SqlHelper())
            {

                try
                {
                    //-----------------------------------------------交易服务费 开始------------
                    //得到商家信息,账户余额
                    B2bFinanceData Financed = new B2bFinanceData();
                    B2b_com_pro pro_t = new B2bComProData().GetProById(modelb2border.Pro_id.ToString());

                    B2b_company modelcom = B2bCompanyData.GetCompany(modelb2border.Comid);
                    //获得新总金额
                    decimal Over_money = modelcom.Imprest;

                    //订单的支付金额
                    decimal orderpay = modelb2border.Pay_price * modelb2border.U_num + modelb2border.Express - modelb2border.Child_u_num * modelb2border.childreduce;

                    //计算 退回手续费金额
                    decimal shouxufei = Financed.GetShouxufeiAmount(modelb2border.Id);//查元支付的手续费,手续费为负数
                    //当手续费为0时不操作退手续费操作
                    if (shouxufei == 0)
                    {
                        return 0;
                    }

                    decimal backshouxufei = 0;//退回收续费

                    //退回手续费根据订单金额和退回金额比例，乘以之前收取手续费的金额
                    if (backprice > 0 && orderpay > 0)
                    {
                        backshouxufei = (backprice / orderpay) * shouxufei;
                    }
                    else
                    {
                        backshouxufei = 0;
                    }
                    //退回的手续费为，负数,所以下面是  - ，手续费 
                    if (backshouxufei > 0)
                    {
                        backshouxufei = 0;
                    }



                    B2b_Finance Financebackinfo_1 = new B2b_Finance()
                    {
                        Id = 0,
                        Com_id = modelb2border.Comid,
                        Agent_id = modelb2border.Agentid,           //分销编号（默认为0）
                        Eid = 0,                //易城账户（默认为0，0=没有交易，1=和易城交易）
                        Order_id = modelb2border.Id,           //订单号（默认为0）
                        Servicesname = pro_t.Pro_name + "[" + modelb2border.Id + "]",       //交易名称/内容
                        SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                        Money = decimal.Round(0 - backshouxufei, 2),              //金额
                        Payment = 1,            //收支(0=收款,1=支出)
                        Payment_type = "退手续费",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点）
                        Money_come = "分销交易",         //资金来源（网上支付,银行收款等）
                        Over_money = decimal.Round(Over_money - backshouxufei, 2)       //余额（根据商家，分销，易城，编号显示相应余额）
                    };
                    int finacebackid_1 = Financed.InsertOrUpdate(Financebackinfo_1);
                    //-----------------------------------------------交易服务费 结束------------
                    return finacebackid_1;
                }
                catch (Exception ex)
                {
                    helper.Rollback();
                    return 0;
                }
            }
        }





        //评价
        public static string Subevaluate(int comid, int oid, int uid, int star, int evatype, string area)
        {
            var pro = 0;
            try
            {
                B2bOrderData dataorder = new B2bOrderData();

                var evaluateid = dataorder.GetevaluateidByoid(oid, evatype);
                if (evaluateid == 0)
                {
                    int channelid = 0;

                    B2b_order modelb2border = dataorder.GetOrderById(oid);
                    if (modelb2border != null)
                    {
                        channelid = modelb2border.channelcoachid;
                    }

                    pro = dataorder.Insertevaluateid(comid, uid, oid, channelid, 0, star, evatype, area);
                }
                else
                {

                    pro = dataorder.updateevaluateid(evaluateid, 0, star, area);
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


        public static string EvaluatePageList(int comid, int oid, int uid, int channelid, int evatype, int pageindex, int pagesize)
        {
            int totalcount = 0;
            var crmdata = new B2bCrmData();


            var list = new B2bOrderData().EvaluatePageList(comid, oid, uid, channelid, evatype, pageindex, pagesize, out totalcount);
            if (totalcount == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "", totalcount = 0 });
            }
            IEnumerable result = "";
            if (list != null)
            {
                result = from m in list
                         select new
                         {
                             id = m.id,
                             comid = m.comid,
                             anonymous = m.anonymous,
                             channelid = m.channelid,
                             evatype = m.evatype,
                             oid = m.oid,
                             proname = new B2bComProData().Getpronamebyorderid(m.oid),
                             starnum = m.starnum,
                             subtime = m.subtime,
                             text = m.text,
                             uid = m.uid,
                             Imgurl = crmdata.GetNameorImgByid(m.uid, 1),
                             uname = crmdata.GetNameorImgByid(m.uid, 2),
                         };
                return JsonConvert.SerializeObject(new { type = 100, msg = result, totalcount = totalcount });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }

        }

        public static string EvaluatePageinfo(int id, int evatype)
        {
            int totalcount = 0;
            var crmdata = new B2bCrmData();


            var list = new B2bOrderData().EvaluatePageinfo(id, evatype);

            IEnumerable result = "";
            if (list != null)
            {
                result = from m in list
                         select new
                         {
                             id = m.id,
                             comid = m.comid,
                             anonymous = m.anonymous,
                             channelid = m.channelid,
                             evatype = m.evatype,
                             oid = m.oid,
                             proname = new B2bComProData().Getpronamebyorderid(m.oid),
                             starnum = m.starnum,
                             subtime = m.subtime,
                             text = m.text,
                             uid = m.uid,
                             Imgurl = crmdata.GetNameorImgByid(m.uid, 1),
                             uname = crmdata.GetNameorImgByid(m.uid, 2),
                         };
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }

        }

        //支付状态
        public static string GetorderPaystate(int comid, int id)
        {
            var pro = 0;
            try
            {
                B2bOrderData dataorder = new B2bOrderData();
                B2b_order modelb2border = dataorder.GetOrderById(id);
                if (modelb2border != null)
                {
                    if (modelb2border.Order_state == 23)
                    {

                        return JsonConvert.SerializeObject(new { type = 1, msg = -1 });
                    }


                    if (modelb2border.Pay_state == 1)
                    {//待支付订单返回正常
                        return JsonConvert.SerializeObject(new { type = 100, msg = 1 });
                    }
                    if (modelb2border.Pay_state == 2)
                    {//已支付订单返回
                        return JsonConvert.SerializeObject(new { type = 100, msg = 2 });
                    }
                }

                return JsonConvert.SerializeObject(new { type = 1, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


        public static string prosaleordercount(int comid,string begindate,string enddate,int projectid,int productid,string key,string orderstate)
        {
            int totalcount = 0;
            B2bOrderData dataorder = new B2bOrderData();
            var list = dataorder.prosaleordercount(comid, begindate, enddate, projectid, productid, key, orderstate,out totalcount);

            IEnumerable result = "";
            if (list != null)
            {
                result = from m in list
                         select new
                         {
                             u_num = m.U_num,//预定数量
                             Agentid = m.Agentid,
                             Pay_price = m.Pay_price,//平均价格
                             Agentcompany=new AgentCompanyData().GetAgentCompany(m.Agentid),
                             productname = new B2bComProData().GetProName(productid),
                             projectname =new B2b_com_projectData().GetProjectNameByid(projectid),
                             yanzhengnum = dataorder.proyanzhengordercount(comid, begindate, enddate, projectid, productid, m.Agentid),
                          
                         };
                return JsonConvert.SerializeObject(new { type = 100, msg = result, totalcount = totalcount });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }

        }

        //修改支付状态或取消订单
        public static string UporderPaystate(int id, string caozuo, string mobile)
        {
            var pro = 0;
            try
            {
                B2bOrderData dataorder = new B2bOrderData();
                B2b_order modelb2border = dataorder.GetOrderById(id);
                if (modelb2border != null)
                {



                    B2b_com_pro pro_t = new B2bComProData().GetProById(modelb2border.Pro_id.ToString());
                    if (pro_t != null)
                    {
                        if (pro_t.bookpro_bindphone != "")
                        {
                            if (pro_t.bookpro_bindphone != mobile)
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "手机错误" });
                            }
                        }

                        //读取房间
                        if (pro_t.Server_type == 9)
                        {
                            modelb2border.M_b2b_order_hotel = new B2b_order_hotelData().GetHotelOrderByOrderId(id);
                        }

                        //修改绑定人确认日期，确认日期读取 产品设定信息
                        var upbindinfo = dataorder.upprobindinfo(id, pro_t.bookpro_bindname, pro_t.bookpro_bindcompany, pro_t.bookpro_bindphone);

                    }


                    if (caozuo == "qr")
                    {
                        if (modelb2border.Pay_state == 0)
                        {
                            modelb2border.Pay_state = 1;
                            dataorder.InsertOrUpdate(modelb2border);

                        }

                        string querenduanxin_channel = "您提交订订单:" + pro_t.Pro_name + " ，\n\n预约时间：" + modelb2border.U_traveldate.ToString("yyyy-MM-dd hh:mm") + " \n\n教练已确认，\n\n点击链接立即支付\n\n http://shop" + modelb2border.Comid + ".etown.cn/wxpay/micromart_orderpay.aspx?id=" + id + "&comid=" + modelb2border.Comid;

                        int fasongduanxin = 0;
                        if (pro_t.Server_type == 9)
                        {

                            //var data_temp = OrderJsonData.agentorder_shoudongchuli(id);
                            fasongduanxin = 1;

                            string dikou = new SendEticketData().SendEticket(modelb2border.Id, 1);

                            modelb2border.Order_state = 22;
                            dataorder.InsertOrUpdate(modelb2border);

                            var order_a = dataorder.GetOldorderBybindingId(modelb2border.Id);
                            if (order_a == null)
                            { //如果是导入产品 则
                                if (order_a != null)
                                {
                                    order_a.Order_state = 22;
                                    dataorder.InsertOrUpdate(order_a);
                                }

                            }


                        }

                        //给客户发送微信客服通道通知
                        if (fasongduanxin == 0)
                        {
                            CustomerMsg_Send.SendWxkefumsg(id, 2, querenduanxin_channel, modelb2border.Comid);//给客户发送
                        }

                    }
                    if (caozuo == "qx" || caozuo == "tj")
                    {

                        //查询是否为导入产品订单
                        var order_a = dataorder.GetOldorderBybindingId(modelb2border.Id);

                        //先判定订房状态如果是已经取消的放不能再退，因为有多个入口可以退房
                        if (modelb2border.Order_state == 23 || modelb2border.Order_state == 24) {

                            return JsonConvert.SerializeObject(new { type = 1, msg = "订单已取消或退票，请刷新后重新处理！" });
                        }
                        //对导入产品订单 查找原始订单状态
                        if (order_a != null) {
                            if (order_a.Order_state == 23 || order_a.Order_state == 24)
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "订单已取消或退票，请刷新后重新处理！" });
                            }
                        }



                        if (modelb2border.Pay_state == 0 || modelb2border.Pay_state == 1)
                        {
                            if (pro_t.Server_type == 9)
                            {
                                modelb2border.Order_state = (int)OrderStatus.Hotecannel;//订房取消订单
                            }
                            else
                            {
                                modelb2border.Order_state = 23;
                            }

                            dataorder.InsertOrUpdate(modelb2border);

                        }

                        if (modelb2border.Pay_state == 2)
                        {
                            if (pro_t.Server_type == 9)
                            {
                                string beizhu = "";
                                if (caozuo == "qx")
                                {
                                    beizhu = "房满";
                                }
                                if (caozuo == "tj")
                                {
                                    beizhu = "调价";
                                }




                                if (order_a == null)
                                {

                                    string data_temp = OrderJsonData.QuitOrder(modelb2border.Comid, modelb2border.Id, pro_t.Id, modelb2border.U_num, beizhu + "酒店自动退订");

                                    string data1 = "{\"root\":" + data_temp + "}";
                                    XmlDocument xxd = JsonConvert.DeserializeXmlNode(data1);
                                    string type1 = xxd.SelectSingleNode("root/type").InnerText;
                                    string msg1 = xxd.SelectSingleNode("root/msg").InnerText;
                                    if (type1 == "100")
                                    {
                                        #region 用户订单退款(微信支付 并且 已经开通微信自动退款的商户直销订单 退款则自动把款项退给用户，不需要在总账户后台处理；使用其他支付方式 或者 还没有开通微信支付自动退款的商户 则还需要进入总账户后台处理，并且需要手动退款给客户)
                                        B2b_pay msucpay = new B2bPayData().GetSUCCESSPayById(modelb2border.Id);
                                        if (msucpay != null)
                                        {
                                            string refundstr = OrderJsonData.OrderRefundManage(modelb2border.Id, modelb2border.U_num, msucpay.Total_fee, msucpay.Total_fee, "订房微信自动退款", "订房微信自动退款");
                                            //context.Response.Write(refundstr);
                                            //return;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        //context.Response.Write("{\"type\":\"1\",\"msg\":\"" + msg1 + "\"}");
                                    }
                                    B2b_order modelb2border_up = dataorder.GetOrderById(id);
                                    modelb2border_up.Order_state = (int)OrderStatus.Hotecannel;//订房取消订单
                                    modelb2border_up.Order_remark = beizhu; //订房备注
                                    dataorder.InsertOrUpdate(modelb2border_up);

                                }
                                else
                                { //如果是导入产品  

                                    string data_temp = OrderJsonData.QuitOrder(order_a.Comid, order_a.Id, order_a.Pro_id, order_a.U_num, beizhu + "酒店自动退订");

                                    string data1 = "{\"root\":" + data_temp + "}";
                                    XmlDocument xxd = JsonConvert.DeserializeXmlNode(data1);
                                    string type1 = xxd.SelectSingleNode("root/type").InnerText;
                                    string msg1 = xxd.SelectSingleNode("root/msg").InnerText;
                                    if (type1 == "100")
                                    {
                                        #region 用户订单退款(微信支付 并且 已经开通微信自动退款的商户直销订单 退款则自动把款项退给用户，不需要在总账户后台处理；使用其他支付方式 或者 还没有开通微信支付自动退款的商户 则还需要进入总账户后台处理，并且需要手动退款给客户)
                                        B2b_pay msucpay = new B2bPayData().GetSUCCESSPayById(order_a.Id);
                                        if (msucpay != null)
                                        {
                                            string refundstr = OrderJsonData.OrderRefundManage(order_a.Id, order_a.U_num, msucpay.Total_fee, msucpay.Total_fee, "订房微信自动退款", "订房微信自动退款");
                                            //context.Response.Write(refundstr);
                                            //return;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        //context.Response.Write("{\"type\":\"1\",\"msg\":\"" + msg1 + "\"}"); 
                                    }


                                    var order_a_up = dataorder.GetOldorderBybindingId(modelb2border.Id);
                                    order_a_up.Order_state = (int)OrderStatus.Hotecannel;//订房取消订单
                                    order_a_up.Order_remark = beizhu; //订房备注
                                    dataorder.InsertOrUpdate(order_a_up);


                                }


                            }
                        }



                        string querenduanxin_channel = "您提交订订单" + pro_t.Pro_name + " ，预约时间：" + modelb2border.U_traveldate.ToString("yyyy-MM-dd hh:mm") + "此时间教练已被预约，订单取消。请选择其他时间段或其他教练进行预约。 ";

                        int fasongduanxin = 0;
                        if (pro_t.Server_type == 9)
                        {
                            if (modelb2border.Agentid == 0)
                            {
                                querenduanxin_channel = "您提交订订房" + pro_t.Pro_name + " ，入住时间：" + modelb2border.M_b2b_order_hotel.Start_date.ToString("yyyy-MM-dd") + " 离店时间：" + modelb2border.M_b2b_order_hotel.End_date.ToString("yyyy-MM-dd") + "满房订单已取消。请选择其他时间。 ";
                            }
                            else
                            {
                                fasongduanxin = 1;
                            }
                        }


                        //因为分销订单 应该给分销发送而不能给客户发送
                        if (fasongduanxin == 0)
                        {
                            //给客户发送微信客服通道通知
                            CustomerMsg_Send.SendWxkefumsg(id, 2, querenduanxin_channel, modelb2border.Comid);//给客户发送
                        }
                        else
                        {

                            //分销订房，房取消给分销联系人发送短信
                            var agentdata = new AgentCompanyData().GetAgentCompany(modelb2border.Agentid);
                            if (agentdata != null)
                            {
                                string msg = "";
                                string agentquerenduanxin_channel = "您的账户提交订订房" + pro_t.Pro_name + " ，入住时间：" + modelb2border.M_b2b_order_hotel.Start_date.ToString("yyyy-MM-dd") + " 离店时间：" + modelb2border.M_b2b_order_hotel.Start_date.ToString("yyyy-MM-dd") + " 满房订单已取消，订房金额已退回到账户账户中。 ";
                                var sendback = SendSmsHelper.SendSms(agentdata.Mobile, agentquerenduanxin_channel, modelb2border.Comid, out msg);
                            }

                            if (order_a != null)
                            {
                                if (order_a.Agentid != 0)
                                {
                                    //读取房间
                                    if (pro_t.Server_type == 9)
                                    {
                                        order_a.M_b2b_order_hotel = new B2b_order_hotelData().GetHotelOrderByOrderId(order_a.Id);
                                    }

                                    var agentdata_a = new AgentCompanyData().GetAgentCompany(order_a.Agentid);
                                    string msg = "";
                                    string agentquerenduanxin_channel = "您的账户提交订订房" + pro_t.Pro_name + " ，入住时间：" + order_a.M_b2b_order_hotel.Start_date.ToString("yyyy-MM-dd") + " 离店时间：" + order_a.M_b2b_order_hotel.Start_date.ToString("yyyy-MM-dd") + " 满房订单已取消，订房金额已退回到账户账户中。 ";
                                    var sendback = SendSmsHelper.SendSms(agentdata_a.Mobile, agentquerenduanxin_channel, order_a.Comid, out msg);

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



        public static string GetHzins_detail(int orderid)
        {
            if (orderid == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "传递订单号不可为空" });
            }
            string result = new HzinsInter().Hzins_insureDetail(orderid);
            if (result != "")
            {
                Hzins_InsureDetailResp resp = (Hzins_InsureDetailResp)JsonConvert.DeserializeObject(result, typeof(Hzins_InsureDetailResp));
                if (resp.respCode == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = resp });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = resp.respMsg });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "根据订单号没有查询到慧择保险订单" });
            }
        }
        /// <summary>
        /// 慧择网订单批量查询
        /// </summary>
        /// <param name="orderidstr"></param>
        /// <returns></returns>
        public static string GethzinsorderSearch(string orderidstr)
        {
            string result = new HzinsInter().Hzins_orderSearch(orderidstr);
            if (result != "")
            {
                Hzins_SearchInsureResp resp = (Hzins_SearchInsureResp)JsonConvert.DeserializeObject(result, typeof(Hzins_SearchInsureResp));
                if (resp.respCode == 0)
                {
                    if (resp.data != null)
                    {
                        if (resp.data.orderDetailInfos != null)
                        {
                            List<Hzins_SearchInsureResp_OrderDetailInfo> infos = resp.data.orderDetailInfos;

                            if (infos.Count > 0)
                            {
                                foreach (Hzins_SearchInsureResp_OrderDetailInfo info in infos)
                                {
                                    info.effectiveStateStr = EnumUtils.GetName((Hzins_effectiveState)info.effectiveState);
                                }
                                return JsonConvert.SerializeObject(new { type = 100, msg = infos });
                            }
                        }
                    }
                }


            }

            return JsonConvert.SerializeObject(new { type = 1, msg = "" });
        }




        /// <summary>
        /// 赠送保险
        /// </summary>
        /// <param name="orderid"></param>
        public static void ZengsongBaoxian(int orderid)
        {
            try
            {
                #region 旅游大巴产品免费赠送保险
                B2b_order aorder = new B2bOrderData().GetOrderById(orderid);//a 订单

                B2b_com_pro aorder_pro = new B2bComProData().GetProById(aorder.Pro_id.ToString());//a 订单产品


                if (aorder != null)
                {
                    if (aorder_pro != null)
                    {
                        if (aorder_pro.Server_type == 10)
                        {
                            #region  订单产品是导入产品
                            if (aorder.Bindingagentorderid > 0)
                            {

                                B2b_order border = new B2bOrderData().GetOrderById(aorder.Bindingagentorderid);//b 订单

                                B2b_com_pro border_pro = new B2bComProData().GetProById(border.Pro_id.ToString());//b 订单产品

                                if (border != null && border_pro != null)
                                {
                                    //b 订单产品 下赠送的保险产品
                                    int bxproid = new B2bComProData().GetSelbindbx(border.Pro_id);
                                    if (bxproid > 0)
                                    {
                                        //判断b 订单下赠送的保险产品是否是导入的
                                        int bindbxproid = new B2bComProData().GetbindingidbyProid(bxproid);
                                        #region b订单 下赠送的保险产品是导入的：需要b 商户下绑定分销提交一笔分销订单；
                                        if (bindbxproid > 0)
                                        {
                                            //b商户下绑定分销 提交一笔分销订单  
                                            int b_comid = border.Comid;
                                            int b_bindagentid = new B2bCompanyData().GetBindingAgentByComid(b_comid);
                                            //b商户下没有绑定分销
                                            if (b_bindagentid == 0)
                                            {
                                                return;
                                            }
                                            else
                                            {
                                                int bindbxproid_comid = new B2bComProData().GetComidByProid(bindbxproid);

                                                //新添加条件字段:isInterfaceSub(是否是电子票接口提交的订单:0.否;1.是)
                                                int isInterfaceSub = 0;
                                                int bx_orderid = 0;

                                                int Agentlevel = 3;
                                                //int Warrant_type = 0;
                                                var agentmodel = AgentCompanyData.GetAgentWarrant(b_bindagentid, bindbxproid_comid);
                                                if (agentmodel != null)
                                                {
                                                    Agentlevel = agentmodel.Warrant_level;
                                                    //Warrant_type = agentmodel.Warrant_type;
                                                }

                                                //(注：本方法中几个注释情况如果解除注释，则需要加入下面这个逻辑判断)得到绑定保险产品的第一个规格值

                                                List<B2b_com_pro_Speci> listspeci = new B2b_com_pro_SpeciData().AgentGetgglist(bindbxproid, Agentlevel);
                                                if (listspeci.Count == 0)
                                                {
                                                    return;
                                                }
                                                var rlistspeci = listspeci.OrderBy(s => s.speci_agentsettle_price);
                                                B2b_com_pro_Speci firstspeci = rlistspeci.FirstOrDefault();
                                                if (firstspeci == null)
                                                {
                                                    return;
                                                }
                                                int speciid = firstspeci.id;

                                                //提交的保险信息完整才会赠送保险 
                                                string a_baoxiannames = "";
                                                string a_baoxianpinyinnames = "";
                                                string a_baoxianidcards = "";
                                                for (int a = 0; a < aorder.baoxiannames.Split(',').Length; a++)
                                                {
                                                    if (aorder.baoxiannames.Split(',')[a] != "" && aorder.baoxianidcards.Split(',')[a] != "")
                                                    {
                                                        //被保险人 出游当天 上过保险的数量，如果有上过 则不在赠送保险订单
                                                        int samedayBaoxianOrderNum = new B2bOrderData().GetSamedayBaoxianOrderNum(aorder.baoxianidcards.Split(',')[a], aorder.U_traveldate);
                                                        if (samedayBaoxianOrderNum == 0)
                                                        {
                                                            a_baoxiannames += aorder.baoxiannames.Split(',')[a] + ",";
                                                            a_baoxianpinyinnames += aorder.baoxianpinyinnames == "" ? "" : aorder.baoxianpinyinnames.Split(',')[a] + ",";
                                                            a_baoxianidcards += aorder.baoxianidcards.Split(',')[a] + ",";
                                                        }
                                                    }
                                                }
                                                if (a_baoxiannames != "")
                                                {
                                                    a_baoxiannames = a_baoxiannames.Substring(0, a_baoxiannames.Length - 1);
                                                    aorder.baoxiannames = a_baoxiannames;
                                                    aorder.U_num = a_baoxiannames.Split(',').Length;
                                                }
                                                else
                                                {
                                                    aorder.U_num = 0;
                                                }
                                                if (a_baoxianpinyinnames != "")
                                                {
                                                    a_baoxianpinyinnames = a_baoxianpinyinnames.Substring(0, a_baoxianpinyinnames.Length - 1);
                                                    aorder.baoxianpinyinnames = a_baoxianpinyinnames;
                                                }
                                                if (a_baoxianidcards != "")
                                                {
                                                    a_baoxianidcards = a_baoxianidcards.Substring(0, a_baoxianidcards.Length - 1);
                                                    aorder.baoxianidcards = a_baoxianidcards;
                                                }
                                                if (aorder.U_num > 0)
                                                {
                                                    string data = OrderJsonData.AgentOrder(b_bindagentid, bindbxproid.ToString(), "1", aorder.U_num.ToString(), aorder.U_name, aorder.U_phone, aorder.U_traveldate.ToString("yyyy-MM-dd"), "", 1, out bx_orderid, 0, 1, "", "", "赠送保险", 0, "", "", "", "", 0, 0, 0, 0, speciid, aorder.baoxiannames, aorder.baoxianpinyinnames, aorder.baoxianidcards, 0);
                                                    XmlDocument retdoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + data + "}");
                                                    XmlElement retroot = retdoc.DocumentElement;
                                                    string type = retroot.SelectSingleNode("type").InnerText;
                                                    string msg = retroot.SelectSingleNode("msg").InnerText;
                                                    if (type == "100")//创建订单成功
                                                    {
                                                        //赠送保险订单 和  订单 关联
                                                        int givebaoxianorderid = bx_orderid;
                                                        int guanlianbxorder1 = new B2bOrderData().GuanlianBxorder(aorder.Id, givebaoxianorderid);
                                                        int guanlianbxorder2 = new B2bOrderData().GuanlianBxorder(border.Id, givebaoxianorderid);
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                        #region (保险产品都发布在慧择网，所以赠送保险产品都是导入的，不会进入此方法)b订单 下赠送的保险产品是自己系统的:需要b 商户下提交一笔直销订单；然后绕过支付，直接调用处理订单方法
                                        //else
                                        //{
                                        //    List<B2b_com_pro_Speci> listspeci = new B2b_com_pro_SpeciData().GetCanUsegglist(bxproid);
                                        //    if (listspeci.Count == 0)
                                        //    {
                                        //        return;
                                        //    }
                                        //    int speciid = listspeci[0].id;
                                        //    decimal payprice = listspeci[0].speci_advise_price;
                                        //    decimal cost = listspeci[0].speci_agentsettle_price;

                                        //    //b商户id
                                        //    int b_comid = border.Comid;

                                        //    #region  直销保险订单
                                        //    B2b_order order = new B2b_order()
                                        //    {
                                        //        M_b2b_order_hotel = null,
                                        //        Id = 0,
                                        //        Pro_id = bxproid,
                                        //        Speciid = speciid,
                                        //        Order_type = 1,
                                        //        U_name = aorder.U_name,
                                        //        U_id = 0,
                                        //        U_phone = aorder.U_phone,
                                        //        U_num = aorder.U_num,
                                        //        U_subdate = DateTime.Now,
                                        //        Payid = 0,
                                        //        Pay_price = payprice,
                                        //        Cost = cost,
                                        //        Profit = payprice - cost,
                                        //        Order_state = (int)OrderStatus.WaitPay,//
                                        //        Pay_state = (int)PayStatus.NotPay,//支付模块未做出来前先设置为已支付
                                        //        Send_state = (int)SendCodeStatus.NotSend,
                                        //        Ticketcode = 0,//电子码未创建，支付后产生码赋值
                                        //        Rebate = 0,//  利润返佣金额暂时定为0
                                        //        Ordercome = "",//订购来源 暂时定为空
                                        //        U_traveldate = aorder.U_traveldate,
                                        //        Dealer = "自动",
                                        //        Comid = b_comid,
                                        //        Pno = "",
                                        //        Openid = "",
                                        //        Ticketinfo = "",
                                        //        Integral1 = 0,//积分
                                        //        Imprest1 = 0,//预付款
                                        //        Agentid = 0,     //分销ID
                                        //        Warrantid = 0,   //授权ID
                                        //        Warrant_type = 1,  //支付类型分销 1出票扣款 2验码扣款
                                        //        Buyuid = 0,
                                        //        Tocomid = 0,
                                        //        pickuppoint = "",
                                        //        dropoffpoint = "",
                                        //        Order_remark = "",
                                        //        Deliverytype = 0,
                                        //        Province = "",
                                        //        City = "",
                                        //        Address = "",
                                        //        Code = "",
                                        //        channelcoachid = 0,
                                        //        baoxiannames = aorder.baoxiannames,
                                        //        baoxianpinyinnames = aorder.baoxianpinyinnames,
                                        //        baoxianidcards = aorder.baoxianidcards,
                                        //        autosuccess = 0,
                                        //        submanagename = "",
                                        //        yuyuepno = "",
                                        //    };
                                        //    int bxorderid = 0;
                                        //    string data = OrderJsonData.InsertOrUpdate(order, out bxorderid);

                                        //    int insgivebxorderid = new B2bOrderData().UpGivebxorderid(border.Id, bxorderid);
                                        //    #endregion

                                        //    #region 绕过支付 处理订单成功
                                        //    string dikou = new SendEticketData().SendEticket(bxorderid, 1);
                                        //    #endregion
                                        //}
                                        #endregion
                                    }
                                    //不赠送保险产品
                                    else
                                    { }
                                }
                            }
                            #endregion
                            #region 订单产品不是导入产品
                            else
                            {
                                //得到产品所赠保险产品 
                                int bxproid = new B2bComProData().GetSelbindbx(aorder_pro.Id);
                                if (bxproid > 0)
                                {
                                    //判断 订单下赠送的保险产品是否是导入的
                                    int bindbxproid = new B2bComProData().GetbindingidbyProid(bxproid);
                                    #region  订单 下赠送的保险产品是导入的：需要  商户下绑定分销提交一笔分销订单;
                                    if (bindbxproid > 0)
                                    {
                                        //a商户下绑定分销 提交一笔分销订单  
                                        int a_comid = aorder.Comid;
                                        int a_bindagentid = new B2bCompanyData().GetBindingAgentByComid(a_comid);
                                        //a商户下没有绑定分销
                                        if (a_bindagentid == 0)
                                        {
                                            return;
                                        }
                                        else
                                        {
                                            int bindbxproid_comid = new B2bComProData().GetComidByProid(bindbxproid);

                                            //新添加条件字段:isInterfaceSub(是否是电子票接口提交的订单:0.否;1.是)
                                            int isInterfaceSub = 0;
                                            int bx_orderid = 0;

                                            int Agentlevel = 3;
                                            //int Warrant_type = 0;
                                            var agentmodel = AgentCompanyData.GetAgentWarrant(a_bindagentid, bindbxproid_comid);
                                            if (agentmodel != null)
                                            {
                                                Agentlevel = agentmodel.Warrant_level;
                                                //Warrant_type = agentmodel.Warrant_type;
                                            }
                                            //(注：本方法中几个注释情况如果解除注释，则需要加入下面这个逻辑判断)得到绑定保险产品的第一个规格值

                                            List<B2b_com_pro_Speci> listspeci = new B2b_com_pro_SpeciData().AgentGetgglist(bindbxproid, Agentlevel);
                                            if (listspeci.Count == 0)
                                            {
                                                return;
                                            }
                                            var rlistspeci = listspeci.OrderBy(s => s.speci_agentsettle_price);
                                            B2b_com_pro_Speci firstspeci = rlistspeci.FirstOrDefault();
                                            if (firstspeci == null)
                                            {
                                                return;
                                            }
                                            int speciid = firstspeci.id;

                                            //提交的保险信息完整才会赠送保险 
                                            string a_baoxiannames = "";
                                            string a_baoxianpinyinnames = "";
                                            string a_baoxianidcards = "";
                                            for (int a = 0; a < aorder.baoxiannames.Split(',').Length; a++)
                                            {
                                                if (aorder.baoxiannames.Split(',')[a] != "" && aorder.baoxianidcards.Split(',')[a] != "")
                                                {
                                                    //被保险人 出游当天 上过保险的数量，如果有上过 则不在赠送保险订单
                                                    int samedayBaoxianOrderNum = new B2bOrderData().GetSamedayBaoxianOrderNum(aorder.baoxianidcards.Split(',')[a], aorder.U_traveldate);
                                                    if (samedayBaoxianOrderNum == 0)
                                                    {
                                                        a_baoxiannames += aorder.baoxiannames.Split(',')[a] + ",";
                                                        a_baoxianpinyinnames += aorder.baoxianpinyinnames == "" ? "" : aorder.baoxianpinyinnames.Split(',')[a] + ",";
                                                        a_baoxianidcards += aorder.baoxianidcards.Split(',')[a] + ",";
                                                    }
                                                }
                                            }
                                            if (a_baoxiannames != "")
                                            {
                                                a_baoxiannames = a_baoxiannames.Substring(0, a_baoxiannames.Length - 1);
                                                aorder.baoxiannames = a_baoxiannames;
                                                aorder.U_num = a_baoxiannames.Split(',').Length;
                                            }
                                            else
                                            {
                                                aorder.U_num = 0;
                                            }
                                            if (a_baoxianpinyinnames != "")
                                            {
                                                a_baoxianpinyinnames = a_baoxianpinyinnames.Substring(0, a_baoxianpinyinnames.Length - 1);
                                                aorder.baoxianpinyinnames = a_baoxianpinyinnames;
                                            }
                                            if (a_baoxianidcards != "")
                                            {
                                                a_baoxianidcards = a_baoxianidcards.Substring(0, a_baoxianidcards.Length - 1);
                                                aorder.baoxianidcards = a_baoxianidcards;
                                            }

                                            if (aorder.U_num > 0)
                                            {
                                                string data = OrderJsonData.AgentOrder(a_bindagentid, bindbxproid.ToString(), "1", aorder.U_num.ToString(), aorder.U_name, aorder.U_phone, aorder.U_traveldate.ToString("yyyy-MM-dd"), "", 1, out bx_orderid, 0, 1, "", "", "赠送保险", 0, "", "", "", "", 0, 0, 0, 0, speciid, aorder.baoxiannames, aorder.baoxianpinyinnames, aorder.baoxianidcards, 0, null, aorder.Id);
                                                XmlDocument retdoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + data + "}");
                                                XmlElement retroot = retdoc.DocumentElement;
                                                string type = retroot.SelectSingleNode("type").InnerText;
                                                string msg = retroot.SelectSingleNode("msg").InnerText;
                                                if (type == "100")//创建订单成功
                                                {
                                                    //赠送保险订单 和  订单 关联
                                                    int givebaoxianorderid = bx_orderid;
                                                    int guanlianbxorder = new B2bOrderData().GuanlianBxorder(aorder.Id, givebaoxianorderid);
                                                }
                                            }

                                        }
                                    }
                                    #endregion
                                    #region (保险产品都发布在慧择网，所以赠送保险产品都是导入的，不会进入此方法) 订单 下赠送的保险产品是自己系统的:需要a 商户下提交一笔直销订单；然后绕过支付，直接调用处理订单方法
                                    else
                                    {
                                        #region 注释内容
                                        //List<B2b_com_pro_Speci> listspeci = new B2b_com_pro_SpeciData().GetCanUsegglist(bxproid);
                                        //if (listspeci.Count == 0)
                                        //{
                                        //    return;
                                        //}
                                        //int speciid = listspeci[0].id;
                                        //decimal payprice = listspeci[0].speci_advise_price;
                                        //decimal cost = listspeci[0].speci_agentsettle_price;

                                        ////a商户id
                                        //int a_comid = aorder.Comid;

                                        //#region  直销保险订单
                                        //B2b_order order = new B2b_order()
                                        //{
                                        //    M_b2b_order_hotel = null,
                                        //    Id = 0,
                                        //    Pro_id = bxproid,
                                        //    Speciid = speciid,
                                        //    Order_type = 1,
                                        //    U_name = aorder.U_name,
                                        //    U_id = 0,
                                        //    U_phone = aorder.U_phone,
                                        //    U_num = aorder.U_num,
                                        //    U_subdate = DateTime.Now,
                                        //    Payid = 0,
                                        //    Pay_price = payprice,
                                        //    Cost = cost,
                                        //    Profit = payprice - cost,
                                        //    Order_state = (int)OrderStatus.WaitPay,//
                                        //    Pay_state = (int)PayStatus.NotPay,//支付模块未做出来前先设置为已支付
                                        //    Send_state = (int)SendCodeStatus.NotSend,
                                        //    Ticketcode = 0,//电子码未创建，支付后产生码赋值
                                        //    Rebate = 0,//  利润返佣金额暂时定为0
                                        //    Ordercome = "",//订购来源 暂时定为空
                                        //    U_traveldate = aorder.U_traveldate,
                                        //    Dealer = "自动",
                                        //    Comid = a_comid,
                                        //    Pno = "",
                                        //    Openid = "",
                                        //    Ticketinfo = "",
                                        //    Integral1 = 0,//积分
                                        //    Imprest1 = 0,//预付款
                                        //    Agentid = 0,     //分销ID
                                        //    Warrantid = 0,   //授权ID
                                        //    Warrant_type = 1,  //支付类型分销 1出票扣款 2验码扣款
                                        //    Buyuid = 0,
                                        //    Tocomid = 0,
                                        //    pickuppoint = "",
                                        //    dropoffpoint = "",
                                        //    Order_remark = "",
                                        //    Deliverytype = 0,
                                        //    Province = "",
                                        //    City = "",
                                        //    Address = "",
                                        //    Code = "",
                                        //    channelcoachid = 0,
                                        //    baoxiannames = aorder.baoxiannames,
                                        //    baoxianpinyinnames = aorder.baoxianpinyinnames,
                                        //    baoxianidcards = aorder.baoxianidcards,
                                        //    autosuccess = 0,
                                        //    submanagename = "",
                                        //    yuyuepno = "",
                                        //};
                                        //int bxorderid = 0;
                                        //string data = OrderJsonData.InsertOrUpdate(order, out bxorderid);

                                        //int insgivebxorderid = new B2bOrderData().UpGivebxorderid(aorder.Id, bxorderid);
                                        //#endregion

                                        //#region 绕过支付 处理订单成功
                                        //string dikou = new SendEticketData().SendEticket(bxorderid, 1);
                                        //#endregion

                                        #endregion
                                    }
                                    #endregion
                                }
                            }
                            #endregion
                        }
                    }
                }
                #endregion
            }
            catch { }
        }



        public static string Changetraveldate(int orderid, string testpro, string traveldate, string oldtraveldate)
        {
            if (traveldate == "")
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "改期失败：传递日期为空" });
            }
            ////旅游大巴订单 发车前12h 不可以退
            //B2b_order morder = new B2bOrderData().GetOrderById(orderid);
            //if (morder == null)
            //{
            //    return JsonConvert.SerializeObject(new { type = 1, msg = "获取订单失败" });
            //}
            //B2b_com_pro pro = new B2bComProData().GetProById(morder.Pro_id.ToString());
            //if (pro == null)
            //{
            //    return JsonConvert.SerializeObject(new { type = 1, msg = "获取产品失败" });
            //}

            //if (pro.Server_type == 10)
            //{
            //    if (pro.firststationtime != "")
            //    {
            //        DateTime daba_startTime = DateTime.Now;
            //        DateTime daba_endTime = Convert.ToDateTime(morder.U_traveldate.ToString("yyyy-MM-dd ") + pro.firststationtime);
            //        if ((daba_endTime - daba_startTime).TotalHours < 12)
            //        {
            //            return JsonConvert.SerializeObject(new { type = 1, msg = "发车前12小时不可改期" });
            //        }
            //    }
            //}

            string message = "";
            int r = new B2bOrderData().Changetraveldate(orderid, testpro, traveldate, oldtraveldate, out message);
            if (r > 0)
            {
                #region 旅游大巴产品则应该把保险也退掉
                if (orderid > 0)
                {
                    int baoxinorderid = new B2bOrderData().Getgivebaoxinorderid(orderid);
                    if (baoxinorderid > 0)
                    {
                        B2b_order baoxininfo = new B2bOrderData().GetOrderById(baoxinorderid);
                        if (baoxininfo != null)
                        {
                            try
                            {
                                OrderJsonData.QuitOrder(baoxininfo.Comid, baoxininfo.Id, baoxininfo.Pro_id, baoxininfo.U_num, "旅游大巴改期则退掉保险");
                            }
                            catch { }
                        }
                    }
                }
                #endregion

                return JsonConvert.SerializeObject(new { type = 100, msg = "改期成功" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = message });
            }
        }

        public static string Travelbusonboardtag(int orderid, string idcard, string travelname, string traveltime, int comid, int orderbusrideid)
        {
            string pno = new B2bOrderData().GetPnoByOrderId(orderid);
            if (pno != "")
            {
                #region 登录用户
                var username = "";//登录用户
                try
                {
                    //获取PC验证登陆账户
                    B2b_company_manageuser user = UserHelper.CurrentUser();
                    username = user.Accounts;
                }
                catch
                {
                    username = "";
                }
                #endregion

                //验证电子码
                string data = EticketJsonData.EConfirm(pno, "1", comid.ToString(), 999999999, "", username);//返回数据
                XmlDocument retdoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + data + "}");
                XmlElement retroot = retdoc.DocumentElement;
                string type = retroot.SelectSingleNode("type").InnerText;
                string msg1 = retroot.SelectSingleNode("msg").InnerText;
                //if (type == "100")
                //{
                //在旅游大巴订单附属表中 标识上车
                int tagoper = new B2bOrderData().TagBusride(orderbusrideid);
                if (tagoper == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "标识失败" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = "标识成功" });
                }
                //}
                //else 
                //{
                //    return JsonConvert.SerializeObject(new { type = 1, msg = "标识失败:"+msg1 });
                //}  
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "根据订单查询电子票失败" });
            }
        }

        public static string travelbusQunfaRemindSms(string gooutdate, int proid, int paystate, string orderstate, string licenceplate, string telphone, string kerentype, string againphone)
        {
            IList<b2b_order_busNamelist> list = new B2bOrderData().travelbustravelerlistBypaystate(gooutdate, proid, paystate, 0, orderstate);
            if (kerentype == "jietuansuc")
            {
                list = new B2bOrderData().travelbusordertravalerdetail(gooutdate, proid, int.Parse(orderstate));
            }
            if (list == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "客人列表查询失败" });
            }
            else
            {
                string proname = new B2bComProData().GetProName(proid);

                var phones = list.Select(p => p.contactphone).ToArray();

                List<string> listphone = new List<string>();

                string phonsstr = "";
                for (int i = 0; i < phones.Length; i++)
                {
                    if (phones[i] != "")
                    {
                        if (!listphone.Contains(phones[i]))
                        {
                            phonsstr += phones[i] + ",";
                            listphone.Add(phones[i]);
                        }
                    }
                }

                if (phonsstr.Length > 0)
                {
                    phonsstr = phonsstr.Substring(0, phonsstr.Length - 1);
                }

                #region 如果存在补发手机号的话，则为补发短信操作
                if (againphone != "")
                {
                    if (listphone.Contains(againphone))
                    {
                        #region 补发短信
                        //得到群发成功记录
                        B2b_order_busRemindSmsLog bf_remindsmslog = new B2b_order_busRemindSmsLogData().GetB2b_order_busRemindSmsSucLog(proid, gooutdate);
                        if (bf_remindsmslog != null)
                        {
                            //如果已经发送过，则不再发送，直接返回补发成功
                            if (bf_remindsmslog.sendtophones.IndexOf(againphone) > -1)
                            {
                                return JsonConvert.SerializeObject(new { type = 100, msg = "补发短信成功" });
                            }
                        }
                        //得到短信模板内容
                        string bf_smskey = "旅游大巴发车前提醒短信";
                        string bf_smsstr = new SendSmsHelper(new SqlHelper()).GetSmsContent(bf_smskey);
                        bf_smsstr = bf_smsstr.Replace("$starttime$", DateTime.Parse(gooutdate).ToString("yyyy-MM-dd"));
                        bf_smsstr = bf_smsstr.Replace("$title$", proname);
                        bf_smsstr = bf_smsstr.Replace("$card$", licenceplate);
                        bf_smsstr = bf_smsstr.Replace("$phone$", telphone);

                        string bf_msg = "";
                        int bf_sendback = SendSmsHelper.SendSms(againphone, bf_smsstr, 106, out bf_msg);
                        if (bf_sendback > 0)
                        {
                            if (bf_remindsmslog != null)
                            {
                                bf_remindsmslog.sendtophones = bf_remindsmslog.sendtophones + "," + againphone;
                                new B2b_order_busRemindSmsLogData().EditRemindSmsLog(bf_remindsmslog);
                            }

                            return JsonConvert.SerializeObject(new { type = 100, msg = "补发短信成功" });
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "补发短信失败" });
                        }
                        #endregion
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "补发手机号错误" });
                    }
                }
                #endregion

                //得到短信模板内容
                string smskey = "旅游大巴发车前提醒短信";
                string smsstr = new SendSmsHelper(new SqlHelper()).GetSmsContent(smskey);
                smsstr = smsstr.Replace("$starttime$", DateTime.Parse(gooutdate).ToString("yyyy-MM-dd"));
                smsstr = smsstr.Replace("$title$", proname);
                smsstr = smsstr.Replace("$card$", licenceplate);
                smsstr = smsstr.Replace("$phone$", telphone);

                B2b_company_manageuser uuser = UserHelper.CurrentUser();

                B2b_order_busRemindSms remindsms = new B2b_order_busRemindSms
                {
                    id = 0,
                    instime = DateTime.Now,
                    licenceplate = licenceplate,
                    telphone = telphone,
                    traveldate = DateTime.Parse(gooutdate),
                    proid = proid
                };
                int editremindsms = new B2b_order_busRemindSmsData().EditRemindSms(remindsms);

                B2b_order_busRemindSmsLog remindsmslog = new B2b_order_busRemindSmsLog
                {
                    id = 0,
                    proid = proid,
                    sendtime = DateTime.Now,
                    sendtophones = phonsstr,
                    smscontent = smsstr,
                    traveldate = DateTime.Parse(gooutdate),
                    opertorid = uuser.Id,
                    opertorname = uuser.Accounts,
                    issuc = 0
                };
                int editremindlog = new B2b_order_busRemindSmsLogData().EditRemindSmsLog(remindsmslog);
                remindsmslog.id = editremindlog;

                string msg = "";
                int sendback = SendSmsHelper.SendSms(phonsstr, smsstr, 106, out msg);
                if (sendback > 0)
                {
                    remindsmslog.issuc = 1;
                    new B2b_order_busRemindSmsLogData().EditRemindSmsLog(remindsmslog);

                    return JsonConvert.SerializeObject(new { type = 100, msg = "提醒短信发送成功" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "提醒短信发送失败" });
                }
            }
        }

        public static string getPreviewRemindSms(string gooutdate, int proid, string licenceplate, string telphone)
        {
            string proname = new B2bComProData().GetProName(proid);
            //得到短信模板内容
            string smskey = "旅游大巴发车前提醒短信";
            string smsstr = new SendSmsHelper(new SqlHelper()).GetSmsContent(smskey);
            smsstr = smsstr.Replace("$starttime$", DateTime.Parse(gooutdate).ToString("yyyy-MM-dd"));
            smsstr = smsstr.Replace("$title$", proname);
            smsstr = smsstr.Replace("$card$", licenceplate);
            smsstr = smsstr.Replace("$phone$", telphone);

            return JsonConvert.SerializeObject(new { type = 100, msg = smsstr });
        }

        /// <summary>
        /// 微信支付自动退款
        /// </summary>
        /// <param name="mpay">a订单支付信息</param>
        /// <param name="oldorder">a订单</param>
        ///<param name="totalfee">订单总支付金额</param>
        ///<param name="quit_fee">退款金额</param>
        ///<param name="quit_Reason">退款原因</param>
        ///<param name="quit_info">退款说明</param>
        ///<param name="isquityajin">是否是退押金：是 则只需要退款，不需要订单和其他财务处理</param>
        /// <returns></returns>
        public static string WeixinRefundManage(B2b_pay mpay, B2b_order oldorder, decimal totalfee, decimal quit_fee, string quit_Reason, string quit_info, int isquityajin = 0)
        {
            int orderid = oldorder.Id;
            if (mpay.comid == 106 || mpay.comid == 1305 || mpay.comid == 1499 || mpay.comid == 112 || mpay.comid == 2540 || mpay.comid == 2607)
            {
                if (mpay.subtime.AddMonths(6) < DateTime.Now)
                {
                    TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\wxrefund_smallerrLog.txt", orderid + "微信支付只是支持6个月内退款");

                    //context.Response.Write("{\"type\":\"100\",\"msg\":\"退款申请成功\"}");
                    return "{\"type\":\"1\",\"msg\":\"退票申请成功，不过订单已超过6个月,无法自动退款，需要人工处理\"}";
                }
            }
            else
            {
                TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\wxrefund_smallerrLog.txt", orderid + "微信支付只是支持部分商户退款");

                //context.Response.Write("{\"type\":\"100\",\"msg\":\"退票申请成功\"}");
                return "{\"type\":\"1\",\"msg\":\"退票申请成功,不过当前微信公众号没有开通自动退款，需要人工处理\"}";
            }


            #region 微信支付退款(商户106和1305,1499,112 的话，无需进入总账户，下面程序会直接处理商户财务 和 给客户退款 )
            string data2 = "";

            string transaction_id = mpay.Wxtransaction_id;
            string out_trade_no = orderid.ToString();
            //随机生成商户退款单号
            string out_refund_no = string.Format("{0}{1}{2}", mpay.comid, DateTime.Now.ToString("yyyyMMddHHmmss"), new Random().Next(999));
            string total_fee = (totalfee * 100).ToString("F0");
            string refund_fee = (quit_fee * 100).ToString("F0");
            B2b_finance_paytype model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(mpay.comid);

            WxPayConfig config = new WxPayConfig
            {
                APPID = model.Wx_appid,
                APPSECRET = model.Wx_appkey,
                KEY = model.Wx_paysignkey,
                MCHID = model.Wx_partnerid,
                IP = CommonFunc.GetRealIP(),
                SSLCERT_PATH = model.wx_SSLCERT_PATH,
                SSLCERT_PASSWORD = model.wx_SSLCERT_PASSWORD,
                PROXY_URL = "",
                LOG_LEVENL = 3,//日志级别
                REPORT_LEVENL = 0//上报信息配置
            };
            lock (lockobj)
            {

                B2b_pay_wxrefundlog refundlog = new B2b_pay_wxrefundlog
                {
                    id = 0,
                    out_refund_no = out_refund_no,
                    out_trade_no = out_trade_no,
                    transaction_id = transaction_id,
                    total_fee = int.Parse(total_fee),
                    refund_fee = int.Parse(refund_fee),
                    send_xml = "",
                    send_time = DateTime.Parse("1970-01-01 00:00:00"),
                    return_code = "",
                    return_msg = "",
                    err_code = "",
                    err_code_des = "",
                    refund_id = "",
                    return_xml = "",
                    return_time = DateTime.Parse("1970-01-01 00:00:00"),

                };
                try
                {

                    WxPayData data = new WxPayData();

                    data.SetValue("transaction_id", transaction_id);
                    data.SetValue("out_trade_no", out_trade_no);
                    data.SetValue("total_fee", int.Parse(total_fee));//订单总金额
                    data.SetValue("refund_fee", int.Parse(refund_fee));//退款金额
                    data.SetValue("out_refund_no", out_refund_no);//随机生成商户退款单号
                    data.SetValue("op_user_id", config.MCHID);//操作员，默认为商户号
                    data.SetValue("appid", config.APPID);//公众账号ID
                    data.SetValue("mch_id", config.MCHID);//商户号
                    data.SetValue("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));//随机字符串
                    data.SetValue("sign", data.MakeSign(config));//签名

                    string xml = data.ToXml(config);
                    var start = DateTime.Now;

                    refundlog.send_xml = xml;
                    refundlog.send_time = start;

                    int ee = new B2b_pay_wxrefundlogData().Editwxrefundlog(refundlog);
                    refundlog.id = ee;

                    string r = Refund.Run(transaction_id, out_trade_no, total_fee, refund_fee, out_refund_no, config);

                    TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\wxrefund_smallLog.txt", r);

                    string[] str = r.Replace("<br>", ",").Split(',');
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    foreach (string s in str)
                    {
                        if (s != "")
                        {
                            dic.Add(s.Split('=')[0], s.Split('=')[1]);
                        }
                    }
                    string return_code = dic["return_code"];
                    refundlog.return_code = return_code;
                    if (dic.ContainsKey("return_msg"))
                    {
                        string return_msg = dic["return_msg"];
                        refundlog.return_msg = return_msg;
                    }
                    if (dic.ContainsKey("err_code"))
                    {
                        string err_code = dic["err_code"];
                        refundlog.err_code = err_code;
                    }
                    if (dic.ContainsKey("err_code_des"))
                    {
                        string err_code_des = dic["err_code_des"];
                        refundlog.err_code_des = err_code_des;
                    }
                    if (dic.ContainsKey("refund_id"))
                    {
                        string refund_id = dic["refund_id"];
                        refundlog.refund_id = refund_id;
                    }
                    refundlog.return_xml = r;
                    refundlog.return_time = DateTime.Now;
                    new B2b_pay_wxrefundlogData().Editwxrefundlog(refundlog);

                    if (refundlog.err_code != "")
                    {
                        TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\wxrefund_smallLog.txt", orderid + "微信退款失败:" + refundlog.err_code + "(" + refundlog.err_code_des + ")");

                        return "{\"type\":\"1\",\"msg\":\"退款申请成功,不过款项没有自动退，错误信息:" + refundlog.err_code + "(" + refundlog.err_code_des + ")\"}";
                    }
                    else
                    {
                        //如果只是退押金，无需更改商户财务 和 订单状态
                        if (isquityajin == 1)
                        {
                            data2 = "{\"type\":100,\"msg\":\"退款成功，款项已自动退回用户微信余额!\"}";
                            return data2;
                        }
                        else
                        {
                            #region  商户财务处理
                            string remarks = quit_Reason + "-" + quit_info;


                            if (oldorder != null)
                            {
                                if (oldorder.Order_state == 17 || oldorder.Order_state == 18)
                                {

                                    oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退票
                                    oldorder.Ticketinfo = remarks;
                                    oldorder.Backtickettime = DateTime.Now;
                                    oldorder.Ticket = quit_fee;

                                    new B2bOrderData().InsertOrUpdate(oldorder);
                                }

                            }

                            data2 = OrderJsonData.Upticket(oldorder);

                            data2 = "{\"root\":" + data2 + "}";
                            XmlDocument xxd2 = JsonConvert.DeserializeXmlNode(data2);
                            string type2 = xxd2.SelectSingleNode("root/type").InnerText;
                            string msg2 = xxd2.SelectSingleNode("root/msg").InnerText;


                            if (type2 == "100")
                            {
                                data2 = "{\"type\":100,\"msg\":\"退款成功，款项已自动退回用户微信余额!\"}";

                                return data2;
                            }
                            else
                            {
                                TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\wxrefund_smallLog.txt", orderid + "微信退款已经退款，可是出现严重意外错误(" + msg2 + "),抓紧处理，防止重复给客户退款");

                                return "{\"type\":\"1\",\"msg\":\"" + msg2 + "\"}";
                            }
                            #endregion
                        }
                    }
                }
                catch (Exception ef)
                {
                    TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\wxrefund_smallLog.txt", orderid + "微信退款已经退款，可是出现严重意外错误(" + ef.Message + "),抓紧处理，防止重复给客户退款");

                    return "{\"type\":1,\"msg\":\"微信退款意外错误" + ef.Message + "\"}";
                }
            }
            #endregion
        }

        /// <summary>
        /// 订单附加服务押金 退款
        ///  <param name="orderid">a订单id</param>
        ///  <param name="rentserverid">服务id</param>
        ///  <param name="ordertotalfee">订单支付总金额</param>
        ///  <param name="refundfee">退款金额</param>
        ///  <param name="b2b_eticket_Depositid">b2b_eticket_Depositid</param>
        /// </summary>
        /// <returns></returns>
        public static string Rentserver_Refund(int orderid, int rentserverid, decimal ordertotalfee, decimal refundfee, int b2b_eticket_Depositid)
        {
            B2b_order a_orderinfo = new B2bOrderData().GetOrderById(orderid);
            if (a_orderinfo != null)
            {
                #region 判断当前服务是否已经退过押金
                B2b_Rentserver_RefundLog rlog = new B2b_Rentserver_RefundLogData().GetServerRefundlog(orderid, rentserverid, b2b_eticket_Depositid);
                if (rlog != null)
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = "服务押金申请已经提交" });
                }
                #endregion

                #region 根据服务id得到服务信息
                B2b_Rentserver mrentserver = new RentserverData().GetRentServerById(rentserverid);
                if (mrentserver == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "服务信息不存在" });
                }
                #endregion

                #region 得到订单的支付信息
                B2b_pay mpay = new B2bPayData().GetSUCCESSPayById(orderid);
                if (mpay == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "订单支付信息不存在" });
                }
                if (mpay.Trade_status != "TRADE_SUCCESS")
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "支付失败" });
                }
                #endregion

                #region 得到产品信息
                B2b_com_pro mpro = new B2bComProData().GetProById(a_orderinfo.Pro_id.ToString());
                if (mpro == null)
                {
                    //因购买的服务，并不一定有产品所以直接赋值退押金
                     mpro = new B2b_com_pro();
                    mpro.Id = 0;
                    mpro.Pro_name = "退押金";
                    //return JsonConvert.SerializeObject(new { type = 1, msg = "订单产品信息不存在" });
                }
 
                #endregion

                #region 退押金条件验证
                if (ordertotalfee < refundfee)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "退款金额不可大于支付总金额！" });
                }
                if (ordertotalfee != mpay.Total_fee)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "订单总金额和支付总金额不符！！" });
                }
                #endregion

                #region 得到商家基本信息
                var company = B2bCompanyData.GetCompany(a_orderinfo.Comid);
                if (company == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "得到商家基本信息失败" });
                }
                #endregion

                #region 对服务押金 退款进行记录
                B2b_Rentserver_RefundLog server_refundlog = new B2b_Rentserver_RefundLog
                {
                    id = 0,
                    orderid = orderid,

                    proid = mpro.Id,
                    proname = mpro.Pro_name,
                    comid = company.ID,
                    comname = company.Com_name,

                    rentserverid = mrentserver.id,
                    rentservername = mrentserver.servername,
                    ordertotalfee = ordertotalfee,
                    refundfee = refundfee,
                    pay_com = mpay.Pay_com,
                    subtime = DateTime.Now,
                    refundstate = 0,
                    refundremark = "",
                    b2b_eticket_Depositid = b2b_eticket_Depositid
                };
                int refundlogid = new B2b_Rentserver_RefundLogData().EditServer_Refundlog(server_refundlog);
                server_refundlog.id = refundlogid;
                #endregion

                #region 原因1：只有发布产品的商户才有资格收取押金，其他人（例如:导入产品的商户）只是可以卖普通票，不允许收取押金，所以退押金时提交的订单不可能是导入产品订单，此方法不会执行
                if (a_orderinfo.Bindingagentorderid > 0)
                {
                    server_refundlog.refundremark = "传入订单有误，不应该执行当前操作！";
                    new B2b_Rentserver_RefundLogData().EditServer_Refundlog(server_refundlog);

                    return JsonConvert.SerializeObject(new { type = 1, msg = "传入订单有误，不应该执行当前操作！" });
                }
                #endregion
                #region 如果没有b订单，对a订单进行退款
                else
                {
                    #region 同样 原因1，当前方法不会执行
                    if (a_orderinfo.Agentid > 0)
                    {
                        server_refundlog.refundremark = "传入订单有误，不应该执行当前操作！！";
                        new B2b_Rentserver_RefundLogData().EditServer_Refundlog(server_refundlog);

                        return JsonConvert.SerializeObject(new { type = 1, msg = "传入订单有误，不应该执行当前操作！！" });
                    }
                    #endregion
                    #region 直销订单：需要给客户直接退款；并且需要在易城给特定商户(钱支付到易城的)做一笔支出操作；
                    else
                    {
                        #region 微信支付，退押金
                        if (mpay.Pay_com == "wx")
                        {
                            string weixinrefundstr = WeixinRefundManage(mpay, a_orderinfo, ordertotalfee, refundfee, "押金退款", "", 1);
                            XmlDocument retdoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + weixinrefundstr + "}");
                            XmlElement retroot = retdoc.DocumentElement;
                            string type = retroot.SelectSingleNode("type").InnerText;
                            string msg = retroot.SelectSingleNode("msg").InnerText;

                            if (type == "100")
                            {
                                //需要在易城给特定商户(钱支付到易城的)做一笔支出操作；
                                ZhichuFromYicheng(mpay, a_orderinfo, mpro.Pro_name, company, refundfee);


                                server_refundlog.refundremark = "退押金成功，款项已自动退回用户微信余额!";
                                server_refundlog.refundstate = 1;
                                new B2b_Rentserver_RefundLogData().EditServer_Refundlog(server_refundlog);
                                var backyajin = new B2bEticketData().UpbacketicketDepositstate(b2b_eticket_Depositid);//修改退押金状态 
                                return JsonConvert.SerializeObject(new { type = 100, msg = "退押金成功，款项已自动退回用户微信余额!" });
                            }
                            else
                            {
                                server_refundlog.refundremark = msg;
                                new B2b_Rentserver_RefundLogData().EditServer_Refundlog(server_refundlog);
                                
                                return JsonConvert.SerializeObject(new { type = 1, msg = msg });
                            }
                        }
                        #endregion
                        #region 支付宝支付，退押金
                        else if (mpay.Pay_com == "alipay" || mpay.Pay_com == "malipay")
                        {
                            server_refundlog.refundremark = "支付宝支付需要退押金";
                            server_refundlog.refundstate = 2;//退票处理中
                            new B2b_Rentserver_RefundLogData().EditServer_Refundlog(server_refundlog);
                            var backyajin = new B2bEticketData().UpbacketicketDepositstate(b2b_eticket_Depositid);//修改退押金状态 
                            return JsonConvert.SerializeObject(new { type = 100, msg = "退押金申请成功" });
                        }
                        #endregion
                        #region 其他支付，退押金
                        else
                        {
                            server_refundlog.refundremark = "支付方式是" + mpay.Pay_com + ",暂不支持退款";
                            new B2b_Rentserver_RefundLogData().EditServer_Refundlog(server_refundlog); 
                            return JsonConvert.SerializeObject(new { type = 1, msg = "支付方式是" + mpay.Pay_com + ",暂不支持退款" });
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "订单不存在" });
            }
        }

        public static void ZhichuFromYicheng(B2b_pay mpay, B2b_order a_orderinfo, string proname, B2b_company company, decimal refundfee)
        {
            #region 需要在易城给特定商户(钱支付到易城的)做一笔支出操作；
            if (mpay.comid == 106)
            {

                B2b_Finance Financebackinfo = new B2b_Finance()
                {
                    Id = 0,
                    Com_id = a_orderinfo.Comid,
                    Agent_id = 0,           //分销编号（默认为0）
                    Eid = 0,                //易城账户（默认为0，0=没有交易，1=和易城交易）
                    Order_id = a_orderinfo.Id,           //订单号（默认为0）
                    Servicesname = proname + "[" + a_orderinfo.Id + "]",       //交易名称/内容
                    SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                    Money = decimal.Round(0 - (refundfee), 2),              //金额
                    Payment = 1,            //收支(0=收款,1=支出)
                    Payment_type = "直销退押金",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点）
                    Money_come = mpay.Pay_com,        //资金来源（网上支付,银行收款等）
                    Over_money = decimal.Round(company.Imprest - (refundfee), 2)       //余额（根据商家，分销，易城，编号显示相应余额）
                };
                int finacebackid = new B2bFinanceData().InsertOrUpdate(Financebackinfo);

                //查询手续费，退票是 此手续费按比例一起退（和退款金额一起）
                decimal tuishouxufei = 0;
                var orderid_pay = a_orderinfo.Id;//默认此订单为发起的支付的订单
                if (a_orderinfo.Shopcartid != 0)
                {
                    orderid_pay = new B2bFinanceData().GetPayidbyorderid(a_orderinfo.Shopcartid);
                }
                decimal shouxufei = new B2bFinanceData().GetShouxufeiAmount(orderid_pay);
                decimal shouru = new B2bFinanceData().GetShouruAmount(orderid_pay);

                if (shouru != 0 && shouxufei != 0)
                {
                    tuishouxufei = shouxufei * (refundfee / shouru);
                }
                //退手续费，单独两笔这样清楚
                if (tuishouxufei != 0)
                {
                    B2b_Finance Financebackinfo_shouxufei = new B2b_Finance()
                    {
                        Id = 0,
                        Com_id = a_orderinfo.Comid,
                        Agent_id = 0,           //分销编号（默认为0）
                        Eid = 0,                //易城账户（默认为0，0=没有交易，1=和易城交易）
                        Order_id = a_orderinfo.Id,           //订单号（默认为0）
                        Servicesname = proname + "[" + a_orderinfo.Id + "]",       //交易名称/内容
                        SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                        Money = decimal.Round(0 - tuishouxufei, 2),              //金额
                        Payment = 1,            //收支(0=收款,1=支出)
                        Payment_type = "直销退押金-退手续费",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点）
                        Money_come = mpay.Pay_com,         //资金来源（网上支付,银行收款等）
                        Over_money = decimal.Round(company.Imprest - tuishouxufei, 2)       //余额（根据商家，分销，易城，编号显示相应余额）
                    };
                    var finacebackid_shouxufei = new B2bFinanceData().InsertOrUpdate(Financebackinfo_shouxufei);
                }
            }
            #endregion
        }

        public static string GetyajinrefundLoglist(int pageindex, int pagesize, int refundstate, string key)
        {

            try
            {
                var totalcount = 0;
                var list = new B2bOrderData().GetyajinrefundLoglist(pageindex, pagesize, key, refundstate, out totalcount);
                if (list.Count > 0)
                {
                    return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = list });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 100, totalCount = 0, msg = "没有退押金申请" });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string travelbusQunfaRemindSmsReset(string gooutdate, int proid)
        {
            int issuc = 0;
            int r = new B2b_order_busRemindSmsLogData().UpRemindSmsLogState(gooutdate, proid, issuc);
            return JsonConvert.SerializeObject(new { type = 100, msg = "重置成功" });
        }

        public static string SendTrvalNamelist(string gooutdate, int proid, int paystate, string orderstate, string telphone, string firststationtime)
        {
            IList<b2b_order_busNamelist> list = new B2bOrderData().travelbustravelerlistBypaystate(gooutdate, proid, paystate, 0, orderstate);
            if (list == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "客人列表查询失败" });
            }
            else
            {
                string customtext = "";

                foreach (b2b_order_busNamelist mname in list)
                {
                    customtext += mname.name + "(" + mname.contactphone + ")、";
                }
                if (customtext != "")
                {
                    customtext = customtext.Substring(0, customtext.Length - 1);
                }

                string proname = new B2bComProData().GetProName(proid);
                //得到短信模板内容
                string bf_smskey = "给送车员发送乘客名单";
                string bf_smsstr = new SendSmsHelper(new SqlHelper()).GetSmsContent(bf_smskey);
                bf_smsstr = bf_smsstr.Replace("$starttime$", DateTime.Parse(gooutdate).ToString("yyyy-MM-dd"));
                bf_smsstr = bf_smsstr.Replace("$title$", proname + ".总人数:" + list.Count + "人" + ".发车时间:" + firststationtime);
                bf_smsstr = bf_smsstr.Replace("$customtext$", customtext);

                B2b_company_manageuser uuser = UserHelper.CurrentUser();
                B2b_order_busNamelistSendLog remindsmslog = new B2b_order_busNamelistSendLog
                {
                    id = 0,
                    proid = proid,
                    sendtime = DateTime.Now,
                    sendtophones = telphone,
                    smscontent = bf_smsstr,
                    traveldate = DateTime.Parse(gooutdate),
                    opertorid = uuser.Id,
                    opertorname = uuser.Accounts,
                    issuc = 0
                };
                int editremindlog = new B2b_order_busNamelistSendLogData().EditLog(remindsmslog);
                remindsmslog.id = editremindlog;

                string msg = "";
                int sendback = SendSmsHelper.SendSms(telphone, bf_smsstr, 106, out msg);
                if (sendback > 0)
                {
                    remindsmslog.issuc = 1;
                    new B2b_order_busNamelistSendLogData().EditLog(remindsmslog);

                    return JsonConvert.SerializeObject(new { type = 100, msg = "短信发送成功" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "短信发送失败" });
                }
            }
        }

        public static string Yznoticeloglist(int pageindex, int pagesize, string yzdate, string pno, int agentcomid, int comid)
        {
            int totalcount = 0;
            List<Agent_asyncsendlog> yzlogs = new Agent_asyncsendlogData().Getyzlogs(agentcomid, yzdate, pno, comid, pageindex, pagesize, out totalcount, 0);
            if (yzlogs.Count > 0)
            {
                IEnumerable result = from m in yzlogs
                                     select new
                                     {
                                         m.Id,
                                         m.Pno,
                                         m.Num,
                                         m.Confirmtime,
                                         m.Sendtime,
                                         m.Issendsuc,
                                         m.Agentupdatestatus,
                                         m.Remark,
                                         RemarkPithyDesc = m.Remark.Length > 10 ? m.Remark.Substring(0, 10) + ".." : m.Remark,
                                         m.Agentcomid,
                                         AgentComName = new AgentCompanyData().GetAgentCompanyName(m.Agentcomid),
                                         m.Comid,
                                         ComName = new B2bCompanyData().GetCompanyNameById(m.Comid),
                                         m.Issecondsend,
                                         m.Platform_req_seq,
                                         m.Request_content,
                                         m.Response_content,
                                         m.b2b_etcket_logid,
                                         m.isreissue,
                                         isneedreissue = new Agent_asyncsendlogData().IsneedreissueById(m.Id)
                                     };
                return JsonConvert.SerializeObject(new { type = 100, msg = result, totalcount = totalcount });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "查询为空" });
            }
        }

        public static string getNoticesByYzlogid(int b2b_etcket_logid)
        {
            int totalcount = 0;
            List<Agent_asyncsendlog> yzlogs = new Agent_asyncsendlogData().getNoticesByYzlogid(b2b_etcket_logid, out totalcount);
            if (yzlogs.Count > 0)
            {
                IEnumerable result = from m in yzlogs
                                     select new
                                     {
                                         m.Id,
                                         m.Pno,
                                         m.Num,
                                         m.Confirmtime,
                                         m.Sendtime,
                                         m.Issendsuc,
                                         m.Agentupdatestatus,
                                         m.Remark,
                                         RemarkPithyDesc = m.Remark.Length > 10 ? m.Remark.Substring(0, 10) + ".." : m.Remark,
                                         m.Agentcomid,
                                         AgentComName = new AgentCompanyData().GetAgentCompanyName(m.Agentcomid),
                                         m.Comid,
                                         ComName = new B2bCompanyData().GetCompanyNameById(m.Comid),
                                         m.Issecondsend,
                                         m.Platform_req_seq,
                                         m.Request_content,
                                         m.Response_content,
                                         m.b2b_etcket_logid,
                                         m.isreissue,
                                         isneedreissue = new Agent_asyncsendlogData().IsneedreissueById(m.Id)
                                     };
                return JsonConvert.SerializeObject(new { type = 100, msg = result, totalcount = totalcount });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "查询为空" });
            }
        }

        public static string reissueNotice(int noticelogid)
        {
            Agent_asyncsendlog log1 = new Agent_asyncsendlogData().getNoticeLog(noticelogid);
            if (log1 == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "查询通知日志失败" });
            }
            else
            {
                #region 补发验证通知
                lock (lockobj)
                {
                    Agent_asyncsendlog log = new Agent_asyncsendlog
                    {
                        Id = 0,
                        Pno = log1.Pno,
                        Num = log1.Num,
                        Sendtime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        Confirmtime = log1.Confirmtime,
                        Issendsuc = 0,//0失败；1成功
                        Agentupdatestatus = (int)AgentUpdateStatus.Fail,
                        Agentcomid = log1.Agentcomid,
                        Comid = log1.Comid,
                        Remark = "",
                        Issecondsend = 1,
                        Platform_req_seq = log1.Platform_req_seq,//平台流水号，同一次验证的平台流水号相同，分销商根据平台流水号判断是否是同一次验证
                        Request_content = "",
                        Response_content = "",
                        b2b_etcket_logid = log1.b2b_etcket_logid,
                        isreissue = 1
                    };
                    int inslog = new Agent_asyncsendlogData().EditLog(log);
                    log.Id = inslog;
                    try
                    {
                        var eticketinfo = new B2bEticketData().GetEticketDetail(log1.Pno);
                        #region 外部接口产品(电子票表b2b_eticket中没有电子票信息) 直接用日志中的地址
                        if (eticketinfo == null)
                        {
                            //获得分销商信息
                            Agent_company agentinfo = new AgentCompanyData().GetAgentCompany(log1.Agentcomid);
                            if (agentinfo != null)
                            {

                                #region 糯米分销
                                if (agentinfo.Agent_type == (int)AgentCompanyType.NuoMi)//糯米分销
                                {
                                    //查询站外码状态
                                    string username = agentinfo.Agent_auth_username;//百度糯米用户名
                                    string token = agentinfo.Agent_auth_token;//百度糯米token
                                    string code = log1.Pno;//券码
                                    string bindcomname = agentinfo.Agent_bindcomname;//供应商名称

                                    string updateurl = log1.Request_content;


                                    string re = new GetUrlData().HttpPost(updateurl, "");

                                    XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + re + "}");
                                    XmlElement root = doc.DocumentElement;
                                    string info = root.SelectSingleNode("info").InnerText;

                                    //只要返回了数据，则是发送成功
                                    log.Id = inslog;
                                    log.Issendsuc = 1;
                                    log.Request_content = updateurl;
                                    log.Response_content = re;

                                    if (info == "success")
                                    {

                                        string errCode = root.SelectSingleNode("res/errCode").InnerText;//分销商更新结果
                                        if (errCode == "0")
                                        {
                                            log.Agentupdatestatus = (int)AgentUpdateStatus.Suc;
                                            new Agent_asyncsendlogData().EditLog(log);
                                            return JsonConvert.SerializeObject(new { type = 100, msg = "补发成功" });
                                        }
                                        else
                                        {
                                            log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                                            new Agent_asyncsendlogData().EditLog(log);
                                            return JsonConvert.SerializeObject(new { type = 1, msg = "补发失败(" + errCode + ")" });
                                        }
                                    }
                                    else
                                    {
                                        log.Remark = info;
                                        log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                                        new Agent_asyncsendlogData().EditLog(log);
                                        return JsonConvert.SerializeObject(new { type = 1, msg = "补发失败(" + info + ")" });
                                    }
                                }
                                #endregion
                                #region 一般分销推送验证同步请求
                                else
                                {
                                    string re = "";
                                    string updateurl = "";
                                    #region 分销通知发送方式post
                                    if (agentinfo.inter_sendmethod.ToLower() == "post")
                                    {
                                        updateurl = log1.Request_content;//只为记录
                                        if (log1.Agentcomid == 6490)
                                        {
                                            //截取 xxx?xml=xx 
                                            re = new GetUrlData().HttpPost(updateurl.Substring(0, updateurl.IndexOf('?')), updateurl.Substring(updateurl.IndexOf('?') + 5));
                                        }
                                        else
                                        {
                                            updateurl = log1.Request_content;
                                            re = new GetUrlData().HttpPost(updateurl, "");
                                        }


                                        //只要返回了数据，则是发送成功
                                        log.Id = inslog;
                                        log.Issendsuc = 1;
                                        log.Request_content = updateurl;
                                        log.Response_content = re;

                                        log.Remark = re.Replace("'", "''");//单引号替换为'',双引号不用处理;
                                        if (re.Length >= 7)
                                        {
                                            re = re.Substring(0, 7);
                                        }
                                        if (re == "success")
                                        {
                                            log.Agentupdatestatus = (int)AgentUpdateStatus.Suc;
                                            new Agent_asyncsendlogData().EditLog(log);
                                            return JsonConvert.SerializeObject(new { type = 100, msg = "补发成功" });
                                        }
                                        else
                                        {
                                            log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                                            new Agent_asyncsendlogData().EditLog(log);
                                            return JsonConvert.SerializeObject(new { type = 1, msg = "补发失败(" + re + ")" });
                                        }
                                    }
                                    #endregion
                                    #region 分销通知发送方式get
                                    else
                                    {
                                        updateurl = log1.Request_content;

                                        re = new GetUrlData().HttpGet(updateurl);
                                        //只要返回了数据，则是发送成功
                                        log.Id = inslog;
                                        log.Issendsuc = 1;
                                        log.Request_content = updateurl;
                                        log.Response_content = re;

                                        log.Remark = re.Replace("'", "''");//单引号替换为'',双引号不用处理;
                                        if (re.Length >= 7)
                                        {
                                            re = re.Substring(0, 7);
                                        }
                                        if (re == "success")
                                        {
                                            log.Agentupdatestatus = (int)AgentUpdateStatus.Suc;
                                            new Agent_asyncsendlogData().EditLog(log);
                                            return JsonConvert.SerializeObject(new { type = 100, msg = "补发成功" });
                                        }
                                        else
                                        {
                                            log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                                            new Agent_asyncsendlogData().EditLog(log);
                                            return JsonConvert.SerializeObject(new { type = 1, msg = "补发失败(" + re + ")" });
                                        }

                                    }
                                    #endregion

                                }
                                #endregion
                            }
                            else
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "分销信息获取失败" });
                            }
                        }
                        #endregion
                        #region 系统自动生成的电子码
                        else
                        {
                            if (eticketinfo.Agent_id > 0)//分销商电子票,需要发送验证同步请求
                            {
                                //获得分销商信息
                                Agent_company agentinfo = new AgentCompanyData().GetAgentCompany(log1.Agentcomid);
                                if (agentinfo != null)
                                {
                                    string agent_updateurl = agentinfo.Agent_updateurl;
                                    #region 糯米分销
                                    if (agentinfo.Agent_type == (int)AgentCompanyType.NuoMi)//糯米分销
                                    {
                                        //查询站外码状态
                                        string username = agentinfo.Agent_auth_username;//百度糯米用户名
                                        string token = agentinfo.Agent_auth_token;//百度糯米token
                                        string code = eticketinfo.Pno;//券码
                                        string bindcomname = agentinfo.Agent_bindcomname;//供应商名称

                                        string updateurl = agent_updateurl + "?auth={\"userName\":\"" + username + "\",\"token\":\"" + token + "\"}&code=" + code + "&userName=" + bindcomname + "&dealId=&phone="; ;


                                        string re = new GetUrlData().HttpPost(updateurl, "");

                                        XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + re + "}");
                                        XmlElement root = doc.DocumentElement;
                                        string info = root.SelectSingleNode("info").InnerText;

                                        //只要返回了数据，则是发送成功
                                        log.Id = inslog;
                                        log.Issendsuc = 1;
                                        log.Request_content = updateurl;
                                        log.Response_content = re;

                                        if (info == "success")
                                        {

                                            string errCode = root.SelectSingleNode("res/errCode").InnerText;//分销商更新结果
                                            if (errCode == "0")
                                            {
                                                log.Agentupdatestatus = (int)AgentUpdateStatus.Suc;
                                                new Agent_asyncsendlogData().EditLog(log);

                                                return JsonConvert.SerializeObject(new { type = 100, msg = "补发成功" });
                                            }
                                            else
                                            {
                                                log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                                                new Agent_asyncsendlogData().EditLog(log);

                                                return JsonConvert.SerializeObject(new { type = 1, msg = "补发失败(" + errCode + ")" });
                                            }
                                        }
                                        else
                                        {
                                            log.Remark = info;
                                            log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                                            new Agent_asyncsendlogData().EditLog(log);

                                            return JsonConvert.SerializeObject(new { type = 1, msg = "补发失败(" + info + ")" });
                                        }
                                    }
                                    #endregion
                                    #region 一般分销推送验证同步请求
                                    else
                                    {
                                        //一般分销推送验证同步请求 
                                        if (agent_updateurl.Trim() == "" || agentinfo.Inter_deskey == "")
                                        {
                                            log.Remark = "分销商验证同步通知或秘钥没提供";//单引号替换为'',双引号不用处理;
                                            log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                                            new Agent_asyncsendlogData().EditLog(log);

                                            return JsonConvert.SerializeObject(new { type = 1, msg = "分销商验证同步通知或秘钥没提供" });
                                        }

                                        if (eticketinfo.Oid == 0)
                                        {
                                            log.Remark = "电子票对应的订单号为0";//单引号替换为'',双引号不用处理;
                                            log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                                            new Agent_asyncsendlogData().EditLog(log);

                                            return JsonConvert.SerializeObject(new { type = 1, msg = "电子票对应的订单号为0" });
                                        }

                                        //a订单:原分销向指定商户提交的订单;b订单:指定商户下的绑定分销向拥有产品的商户提交的订单
                                        //电子票表中记录的Oid是b订单
                                        //判断b订单 是否 属于某个a订单 
                                        B2b_order loldorder = new B2bOrderData().GetOldorderBybindingId(eticketinfo.Oid);
                                        if (loldorder != null)
                                        {
                                            eticketinfo.Oid = loldorder.Id;
                                        }
                                        /*根据订单号得到分销商订单请求记录(当前需要得到原订单请求流水号)*/
                                        List<Agent_requestlog> listagent_rlog = new Agent_requestlogData().GetAgent_requestlogByOrdernum(eticketinfo.Oid.ToString(), "add_order", "1");
                                        if (listagent_rlog == null)
                                        {
                                            log.Remark = "根据订单号得到分销商订单请求记录失败";//单引号替换为'',双引号不用处理;
                                            log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                                            new Agent_asyncsendlogData().EditLog(log);

                                            return JsonConvert.SerializeObject(new { type = 1, msg = "根据订单号得到分销商订单请求记录失败" });
                                        }

                                        if (listagent_rlog.Count == 0)
                                        {
                                            log.Remark = "根据订单号得到分销商订单请求记录失败.";//单引号替换为'',双引号不用处理;
                                            log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                                            new Agent_asyncsendlogData().EditLog(log);

                                            return JsonConvert.SerializeObject(new { type = 1, msg = "根据订单号得到分销商订单请求记录失败." });
                                        }


                                        /*验证通知发送内容*/
                                        string sbuilder = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                        "<business_trans>" +
                                        "<request_type>sync_order</request_type>" +//<!--验证同步-->
                                        "<req_seq>{0}</req_seq>" +//<!--原订单请求流水号-->
                                        "<platform_req_seq>{1}</platform_req_seq>" +//<!--平台请求流水号-->
                                        "<order>" +//<!--订单信息-->
                                        "<code>{5}</code>" +//<!-- 验证电子码 y-->
                                        "<order_num>{2}</order_num>" +//<!-- 订单号 y-->
                                        "<num>{3}</num>" +//<!-- 使用张数 -->
                                        "<use_time>{4}</use_time>" +//<!--  使用时间  -->
                                        "</order>" +
                                        "</business_trans>", listagent_rlog[0].Req_seq, log.Platform_req_seq, eticketinfo.Oid, log.Num, CommonFunc.ConvertDateTimeInt(log.Confirmtime), log.Pno);

                                        string re = "";
                                        string updateurl = "";
                                        #region 分销通知发送方式post
                                        if (agentinfo.inter_sendmethod.ToLower() == "post")
                                        {
                                            if (eticketinfo.Agent_id == 6490)
                                            {
                                                //当都用完了，才发送通知请求
                                                if (eticketinfo.Use_pnum == 0)
                                                {
                                                    sbuilder = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                    "<business_trans>" +
                                                    "<request_type>sync_order</request_type>" +//<!--验证同步-->
                                                    "<req_seq>{0}</req_seq>" +//<!--原订单请求流水号-->
                                                    "<platform_req_seq>{1}</platform_req_seq>" +//<!--平台请求流水号-->
                                                    "<order>" +//<!--订单信息-->
                                                    "<code>{5}</code>" +//<!-- 验证电子码 y-->
                                                    "<order_num>{2}</order_num>" +//<!-- 订单号 y-->
                                                    "<num>{3}</num>" +//<!-- 使用张数 -->
                                                    "<use_time>{4}</use_time>" +//<!--  使用时间  -->
                                                    "</order>" +
                                                    "</business_trans>", listagent_rlog[0].Req_seq, log.Platform_req_seq, eticketinfo.Oid, eticketinfo.Pnum, CommonFunc.ConvertDateTimeInt(log.Confirmtime), log.Pno);

                                                    re = new GetUrlData().HttpPost(agent_updateurl, sbuilder);
                                                }
                                                updateurl += "?xml=" + sbuilder;//只为记录
                                            }
                                            else
                                            {
                                                updateurl = agent_updateurl + "?xml=" + sbuilder;
                                                re = new GetUrlData().HttpPost(updateurl, "");
                                            }


                                            //只要返回了数据，则是发送成功
                                            log.Id = inslog;
                                            log.Issendsuc = 1;
                                            log.Request_content = updateurl;
                                            log.Response_content = re;

                                            log.Remark = re.Replace("'", "''");//单引号替换为'',双引号不用处理;
                                            if (re.Length >= 7)
                                            {
                                                re = re.Substring(0, 7);
                                            }
                                            if (re == "success")
                                            {
                                                log.Agentupdatestatus = (int)AgentUpdateStatus.Suc;
                                                new Agent_asyncsendlogData().EditLog(log);

                                                return JsonConvert.SerializeObject(new { type = 100, msg = "补发成功" });
                                            }
                                            else
                                            {
                                                log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                                                new Agent_asyncsendlogData().EditLog(log);

                                                return JsonConvert.SerializeObject(new { type = 1, msg = "补发失败(" + re + ")" });
                                            }
                                        }
                                        #endregion
                                        #region 分销通知发送方式get
                                        else
                                        {
                                            if (agent_updateurl.IndexOf('?') > -1)
                                            {
                                                updateurl = agent_updateurl + "&xml=" + sbuilder;
                                            }
                                            else
                                            {
                                                updateurl = agent_updateurl + "?xml=" + sbuilder;
                                            }
                                            re = new GetUrlData().HttpGet(updateurl);
                                            //只要返回了数据，则是发送成功
                                            log.Id = inslog;
                                            log.Issendsuc = 1;
                                            log.Request_content = updateurl;
                                            log.Response_content = re;

                                            log.Remark = re.Replace("'", "''");//单引号替换为'',双引号不用处理;
                                            if (re.Length >= 7)
                                            {
                                                re = re.Substring(0, 7);
                                            }
                                            if (re == "success")
                                            {
                                                log.Agentupdatestatus = (int)AgentUpdateStatus.Suc;
                                                new Agent_asyncsendlogData().EditLog(log);
                                                return JsonConvert.SerializeObject(new { type = 100, msg = "补发成功" });
                                            }
                                            else
                                            {
                                                log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                                                new Agent_asyncsendlogData().EditLog(log);
                                                return JsonConvert.SerializeObject(new { type = 1, msg = "补发失败(" + re + ")" });
                                            }

                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                                else
                                {
                                    return JsonConvert.SerializeObject(new { type = 1, msg = "分销信息获取失败" });
                                }
                            }
                            else
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "非分销无需发送通知" });
                            }
                        }
                        #endregion
                    }
                    catch (Exception ee)
                    {
                        log.Id = inslog;
                        log.Remark = ee.Message.Replace("'", "''");//单引号替换为'',双引号不用处理
                        new Agent_asyncsendlogData().EditLog(log);

                        return JsonConvert.SerializeObject(new { type = 1, msg = "补发失败(" + ee.Message.Replace("'", "''") + ")" });
                    }
                }
                #endregion
            }
        }

        public static string Getagentlist(string isapiagent)
        {
            List<Agent_company> list = new AgentCompanyData().Getagentlist(isapiagent);
            if (list.Count > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "获取分销列表为空" });
            }
        }

        public static string Getcompanylist(string isapicompany)
        {
            List<B2b_company> list = new B2bCompanyData().Getcompanylist(isapicompany);
            if (list.Count > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "获取商家列表为空" });
            }
        }
        /// <summary>
        /// 用户订单退款(微信支付 并且 已经开通微信自动退款的商户直销订单 退款则自动把款项退给用户，不需要在总账户后台处理；使用其他支付方式 或者 还没有开通微信支付自动退款的商户 则还需要进入总账户后台处理，并且需要手动退款给客户)
        /// </summary> 
        /// <param name="oldorder">a订单id</param>
        ///<param name="totalfee">订单总支付金额</param>
        ///<param name="quit_fee">退款金额</param>
        ///<param name="quit_Reason">退款原因</param>
        ///<param name="quit_info">退款说明</param>
        /// <returns></returns>
        public static string OrderRefundManage(int orderid, int quit_num, decimal quit_fee, decimal totalfee, string quit_Reason, string quit_info)
        {
            //得到订单的支付信息
            B2b_pay mpay = new B2bPayData().GetSUCCESSPayById(orderid);

            #region 把退款申请信息 录入b2b_order
            int insaskquitfee = new B2bOrderData().InsAskquitfee(orderid, quit_num, quit_fee, quit_Reason, quit_info);
            if (insaskquitfee == 0)
            {
                TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\wxrefund_smallerrLog.txt", orderid + "把退款申请信息 录入订单表出错");

                //context.Response.Write("{\"type\":\"100\",\"msg\":\"" + orderid + "退款申请信息录入数据库出错\"}");
                return "{\"type\":\"100\",\"msg\":\"" + orderid + "退款申请信息录入数据库出错\"}";
            }
            #endregion

            B2b_order oldorder = new B2bOrderData().GetOrderById(orderid);
            #region 判断是否可以执行 总账户中的 "处理退票" 操作
            if (oldorder != null)
            {
                if (oldorder.Order_state == 17 || oldorder.Order_state == 18)
                { }
                else
                {
                    //data2 = "{\"type\":1,\"msg\":\"订单状态不正确，只有 申请了退票的直销订单才能进一步退款!\"}";
                    TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\wxrefund_smallerrLog.txt", orderid + "订单退票申请成功，不过订单处理退票出错(订单状态不正确)");

                    //context.Response.Write("{\"type\":100,\"msg\":\"" + orderid + "订单退票申请成功\"}");
                    return "{\"type\":100,\"msg\":\"" + orderid + "订单退票申请成功\"}";

                }
            }

            decimal Total_fee = mpay == null ? 0 : mpay.Total_fee;
            //如果有退款金额 ，退款金额不能大于收款的金额
            if (Total_fee != 0)
            {
                if (quit_fee > Total_fee)
                {
                    //data2 = "{\"type\":1,\"msg\":\"金额有误，退款金额不能大于支付金额\"}";
                    TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\wxrefund_smallerrLog.txt", orderid + "金额有误，退款金额不能大于支付金额");

                    //context.Response.Write("{\"type\":100,\"msg\":\"" + orderid + "订单退票申请成功\"}");
                    return "{\"type\":100,\"msg\":\"" + orderid + "订单退票申请成功\"}";
                }
            }
            #endregion

            #region 支付信息不存在(客户用预付款或者积分买的票) 或者 退款金额为0，只需要 修改订单状态即可，无需处理商户财务 和 给客户退款
            if (quit_fee == 0 || mpay == null)
            {
                if (oldorder != null)
                {
                    if (oldorder.Order_state == 17 || oldorder.Order_state == 18)
                    {
                        oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退票
                        oldorder.Ticketinfo = quit_Reason + "-" + quit_info;
                        oldorder.Backtickettime = DateTime.Now;
                        oldorder.Ticket = quit_fee;

                        new B2bOrderData().InsertOrUpdate(oldorder);
                    }

                }
                //context.Response.Write("{\"type\":\"100\",\"msg\":\"退款申请成功\"}");
                return "{\"type\":\"100\",\"msg\":\"退款申请成功\"}";
            }
            #endregion

            #region 支付方式:微信支付
            if (mpay.Pay_com == "wx")
            {
                string weixinrefundstr = OrderJsonData.WeixinRefundManage(mpay, oldorder, totalfee, quit_fee, quit_Reason, quit_info);
                //context.Response.Write(weixinrefundstr);
                return weixinrefundstr;

            }
            #endregion
            #region 支付方式:pc支付宝支付
            else if (mpay.Pay_com == "alipay" || mpay.Pay_com == "malipay")
            {
                string r = "{\"type\":\"100\",\"msg\":\"退款申请提交成功\"}";
                //context.Response.Write(r);
                return r;
            }
            #endregion
            #region 其他支付方式
            else
            {
                string r = "{\"type\":100,\"msg\":\"退款申请提交成功\"}";
                //context.Response.Write(r);
                return r;
            }
            #endregion
        }

        public static string checkPaystate(int orderid)
        {
            int paystate = new B2bOrderData().GetPaystateByOrderid(orderid);
            return JsonConvert.SerializeObject(new { type = 100, msg = paystate });
        }
         
        public static string needvisitdateordercountbyday(DateTime startdate, DateTime enddate, int servertype, int comid)
        {
            List<DateTime> list = new List<DateTime>();
            for (int i = 0; i < (enddate - startdate).Days + 1; i++)
            {
                list.Add(startdate.AddDays(i));
            }   
            if (list.Count > 0)
            {
                IEnumerable result = "";
                if (list != null)
                {
                    result = from m in list
                             select new
                             {
                                 daydate = m.ToString("yyyy-MM-dd"),
                                 //得到需要填写出游日期的产品 的预定订单的支付成功数量
                                 paysucnum = new B2bOrderData().GetNeedVisitDateOrderPaysucNum(m, servertype, comid, (int)PayStatus.HasPay, 0, "4,8,22"),
                             };
                    return JsonConvert.SerializeObject(new { type = 100, msg = result });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }


        public static string GetNeedvisitdateProByTraveldate(DateTime daydate, int servertype, int comid)
        {
            string isSetVisitDate="1";
            IList<B2b_com_pro> list = new B2bComProData().Getb2bcomprobytraveldate(daydate, servertype, comid, isSetVisitDate);
            if (list == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
            else
            {
                if (list.Count > 0)
                {
                    IEnumerable result = "";
                    result = from m in list
                             select new
                             {
                                 Proid = m.Id,
                                 Proname = m.Pro_name,
                                 //支付成功人数
                                 paysucbooknum = new B2bOrderData().GetPaySucNumByProid(m.Id, comid, (int)PayStatus.HasPay, daydate, 0, "4,8,22"),
                             };
                    return JsonConvert.SerializeObject(new { type = 100, msg = result });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
            }
        }

        public static string getNeedvisitdatePaysucorderlist(DateTime gooutdate, int proid, int paystate, string orderstate)
        {
            List<B2b_order> list = new B2bOrderData().getNeedvisitdatePaysucorderlist(gooutdate,proid,paystate,orderstate);
            if (list.Count > 0)
            {
                IEnumerable result = "";
                result = from m in list
                         select new 
                         {
                          m.Id,
                          m.U_name,
                          m.U_phone,
                          m.U_traveldate,
                          m.U_subdate,
                          m.Order_remark,
                          m.Pro_id,
                          Pro_name=new B2bComProData().GetProName(m.Pro_id),
                          m.U_num,
                          m.Order_state
                         };

                return JsonConvert.SerializeObject(new { type = 100, msg =result });
            }
            else 
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }

        public static string HasFinOrder(int orderid)
        {
            B2b_order morder=new B2bOrderData().GetOrderById(orderid);
            if(morder==null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "订单查询失败" });
            } 
            if(morder.Order_state!=(int)OrderStatus.HasSendCode)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "订单状态需要为 已发码 状态才可处理" });
            }
            int chuliorder = new B2bOrderData().HasFinOrder(orderid);
            if (chuliorder > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = "订单处理成功" });
            }
            else 
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "订单处理失败" });
            }
        }




        /// <summary>
        /// 创建一个未支付订单(暂时只是考虑到让美团使用，验证只是加了很基本的验证，如果其他功能调用，请加验证)
        /// </summary>
        /// <param name="agentid"></param>
        /// <param name="product_num"></param>
        /// <param name="ordertype"></param>
        /// <param name="num"></param>
        /// <param name="real_name"></param>
        /// <param name="mobile"></param>
        /// <param name="use_date"></param>
        /// <param name="isInterfaceSub"></param>
        /// <param name="orderid"></param>
        /// <param name="speciid"></param>
        /// <returns></returns>
        public static string CreateNopayOrder(int agentid, string product_num, string ordertype, string num, string real_name, string mobile, string use_date, int isInterfaceSub, out int orderid, int speciid, B2b_order_hotel b2b_order_hotel = null)
        {
            decimal payprice = 0;
            decimal cost = 0;
            decimal profit = 0;

            int warrantid = 0;
            int Warrant_type = 0;
            int Warrant_level = 0;
            int agentsunid = 0;
            int comid = 0;

            string data = "";

            orderid = 0;//先默认返回订单号

            B2b_com_pro pro = new B2bComProData().GetProById(product_num);
            if (pro != null)
            {
                comid = pro.Com_id;
            }
            else
            {
                data = "{\"type\":1,\"msg\":\"产品不存在\"}";
                return data;
            }
            Agent_company agentwarrantinfo = AgentCompanyData.GetAgentWarrant(agentid, comid);

            if (agentwarrantinfo != null)
            {
                warrantid = agentwarrantinfo.Warrantid;
                Warrant_type = agentwarrantinfo.Warrant_type;//支付类型分销 1出票扣款 2验码扣款

                //订房，大巴，实物，保险，只能出票扣款
                if (pro.Server_type == 9 || pro.Server_type == 10 || pro.Server_type == 14 || pro.Server_type == 11)
                {
                    Warrant_type = 1;
                }

                //if (isInterfaceSub == 1)//如果是接口提交的订单，则支付类型都改为: 1出票扣款
                //{
                //    Warrant_type = 1;
                //}
                Warrant_level = agentwarrantinfo.Warrant_level;
                if (agentwarrantinfo.Warrant_state == 0)
                {
                    data = "{\"type\":1,\"msg\":\"产品还没有得到商户授权\"}";

                    return data;
                }
            }
            else
            {
                data = "{\"type\":1,\"msg\":\"产品还没有得到商户授权\"}";

                return data;
            }



            if (pro != null)
            {
                //旅游产品计算单价、成本、毛利
                if (pro.Server_type == 2 || pro.Server_type == 8)
                {
                    cost = pro.Agentsettle_price;
                    comid = pro.Com_id;

                    B2b_com_LineGroupDate lvnowday = new B2b_com_LineGroupDateData().GetLineDayGroupDate(use_date.ConvertTo<DateTime>(), pro.Id);

                    if (lvnowday != null)
                    {
                        //分销差额
                        decimal differprice = 0;
                        if (Warrant_level == 1)
                        {
                            differprice = pro.Agent1_price;
                        }
                        if (Warrant_level == 2)
                        {
                            differprice = pro.Agent2_price;
                        }
                        if (Warrant_level == 3)
                        {
                            differprice = pro.Agent3_price;
                        }
                        payprice = lvnowday.Menprice - differprice;

                        profit = payprice - cost;//根据授权级别，得到分销价格在计算毛利
                    }
                    else
                    {
                        data = "{\"type\":1,\"msg\":\"团期信息获取失败\"}";

                        return data;
                    }
                }
                else if (pro.Server_type == 9)
                { //房
                    cost = pro.Agentsettle_price;
                    comid = pro.Com_id;

                    decimal Menprice = new B2b_com_LineGroupDateData().Gethotelallprice(pro.Id, b2b_order_hotel.Start_date, b2b_order_hotel.End_date, Warrant_level);//单人次价格

                    if (Menprice != 0)
                    {
                        payprice = Menprice;

                        profit = 0;//订房无法计算毛利，因为每天日历并没有填写成本
                    }
                    else
                    {
                        data = "{\"type\":1,\"msg\":\"房态信息获取失败\"}";

                        return data;
                    }

                }
                else if (pro.Server_type == 10)
                { //大巴
                    cost = pro.Agentsettle_price;
                    comid = pro.Com_id;

                    //得到大巴团期表中分销返还情况，如果团期表中没有设置(分销返还都是0)，则读取基本信息中的分销返还
                    B2b_com_LineGroupDate mgroupdate = new B2b_com_LineGroupDateData().GetLineDayGroupDate(use_date.ConvertTo<DateTime>(DateTime.Parse("1970-01-01")), pro.Id);

                    //分销价
                    decimal differprice = 0;
                    if (mgroupdate != null)
                    {
                        if (mgroupdate.agent1_back == 0 && mgroupdate.agent2_back == 0 && mgroupdate.agent3_back == 0)
                        {
                            if (Warrant_level == 1)
                            {
                                differprice = pro.Agent1_price;
                            }
                            if (Warrant_level == 2)
                            {
                                differprice = pro.Agent2_price;
                            }
                            if (Warrant_level == 3)
                            {
                                differprice = pro.Agent3_price;
                            }
                            payprice = differprice;
                            profit = Decimal.Parse(payprice.ToString()) - cost;//根据授权级别，得到分销价格在计算毛利
                        }
                        else
                        {
                            if (Warrant_level == 1)
                            {
                                differprice = mgroupdate.agent1_back;
                            }
                            if (Warrant_level == 2)
                            {
                                differprice = mgroupdate.agent2_back;
                            }
                            if (Warrant_level == 3)
                            {
                                differprice = mgroupdate.agent3_back;
                            }
                            payprice = differprice;
                            profit = Decimal.Parse(payprice.ToString()) - cost;//根据授权级别，得到分销价格在计算毛利
                        }
                    }
                    else
                    {
                        data = "{\"type\":1,\"msg\":\"没有团期\"}";

                        return data;
                    }

                }
                //其他产品(暂时是 票务、实物)计算单价、成本、毛利
                else
                {
                    cost = pro.Agentsettle_price;
                    comid = pro.Com_id;
                    if (Warrant_level == 1)
                    {
                        payprice = pro.Agent1_price;
                    }
                    if (Warrant_level == 2)
                    {
                        payprice = pro.Agent2_price;
                    }
                    if (Warrant_level == 3)
                    {
                        payprice = pro.Agent3_price;
                    }
                    profit = Decimal.Parse(payprice.ToString()) - cost;//根据授权级别，得到分销价格在计算毛利

                    //如果有规格 则规格读取 价格
                    if (speciid != 0)
                    {
                        B2b_com_pro prospeciid = new B2bComProData().GetProspeciidById(product_num, speciid);
                        if (prospeciid != null)
                        {

                            cost = prospeciid.Agentsettle_price;

                            if (Warrant_level == 1)
                            {
                                payprice = prospeciid.Agent1_price;
                            }
                            if (Warrant_level == 2)
                            {
                                payprice = prospeciid.Agent2_price;
                            }
                            if (Warrant_level == 3)
                            {
                                payprice = prospeciid.Agent3_price;
                            }

                            profit = payprice - cost;

                        }
                        else
                        {
                            data = "{\"type\":1,\"msg\":\"规格传入错误\"}";

                            return data;
                        }


                    } 
                }
                if (use_date == ""  )
                {
                    use_date = pro.Pro_end.ToString();
                }
               
                
                if (pro.Pro_state == 0)
                {
                    data = "{\"type\":1,\"msg\":\"产品已暂停\"}";

                    return data;
                }
                if (pro.Pro_end < DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    data = "{\"type\":1,\"msg\":\"产品已过期\"}";

                    return data;
                }

                //库存票检查库存数量，不足则不让提交订单
                if (pro.Source_type == 2)
                {
                    int kucunpiaoshuliang = new B2bComProData().ProSEPageCount_UNUse(pro.Id);
                    if (kucunpiaoshuliang < Int32.Parse(num))
                    {
                        data = "{\"type\":1,\"msg\":\"库存票不足，请电话订购或联系商家\"}";

                        return data;
                    }
                }

                #region 产品实名制验证，暂时先注释
                ////判断产品实名制类型是否一致
                //if (pro.Realnametype != real_name_type)
                //{
                //    data = "{\"type\":1,\"msg\":\"产品实名制类型不一致\"}";

                //    return data;
                //}



                ////判断u_name最后是否含有逗号，含有的话删除
                //if (u_name.Trim() != "")
                //{
                //    u_name = u_name.Replace("，", ",");
                //    if (u_name.Substring(u_name.Length - 1) == ",")
                //    {
                //        u_name = u_name.Substring(0, u_name.Length - 1);
                //    }
                //    if (u_name.Length > 25)
                //    {
                //        data = "{\"type\":1,\"msg\":\"姓名总长度不可超过25字\"}";

                //        return data;
                //    }
                //}
                ////判断实名制类型：0无需实名 1一张一人,2一单一人
                //if (real_name_type != 0)
                //{
                //    if (real_name_type == 1)
                //    {
                //        if (u_name.Trim() != "")
                //        {
                //            string[] str = u_name.Split(',');
                //            if (str.Length > 3)
                //            {
                //                data = "{\"type\":1,\"msg\":\"实名制类型为一张一人,最多只能购买3张\"}";

                //                return data;
                //            }
                //        }
                //    }
                //    if (real_name_type == 2)
                //    {
                //        if (u_name.Trim() != "")
                //        {
                //            string[] str = u_name.Split(',');
                //            if (str.Length > 1)
                //            {
                //                data = "{\"type\":1,\"msg\":\"实名制类型为一单一人,只需提供一个人名字即可\"}";

                //                return data;
                //            }
                //        }
                //    }
                //}
                #endregion
            }
            else
            {
                data = "{\"type\":1,\"msg\":\"产品不存在\"}";

                return data;
            }


            #region 分销员工登录，员工额度限制 验证，暂时注释
            //if (agentaccount != "")//如果为空的话则是通过接口提交的订单；否则是通过分销后台提交的订单，需要考虑员工账户额度限定；
            //{
            //    Agent_regiinfo agentinfo = AgentCompanyData.GetAgentAccountByUid(agentaccount, agentid);
            //    if (agentinfo.AccountLevel != 0)
            //    {
            //        //员工账户额度限定，非注册账户则为员工账户
            //        //if (agentinfo.Accounttype == 1)//出票扣款
            //        //{
            //        if (Warrant_type == 1)//出票扣款
            //        {
            //            if (agentinfo.Amount < payprice * Int32.Parse(u_num))
            //            {
            //                data = "{\"type\":1,\"msg\":\"员工账户销授权金额不足，请联系您的分销商\"}";

            //                return data;
            //            }
            //        }

            //        agentsunid = agentinfo.Id;
            //    }
            //}
            #endregion

           //对使用日期try
            DateTime use_date_temp = pro.Pro_end;
            try
            {
                use_date_temp = DateTime.Parse(use_date);
            }
            catch {
                use_date_temp = pro.Pro_end;
            }

            B2b_order order = new B2b_order()
            {
                M_b2b_order_hotel = null,
                Id = 0,
                Pro_id = product_num.ConvertTo<int>(0),
                Speciid = speciid,
                Order_type = ordertype.ConvertTo<int>(1),
                U_name = real_name,
                U_id = 0,
                U_phone = mobile,
                U_idcard = "",
                U_num =num.ConvertTo<int>(0),
                U_subdate = DateTime.Now,
                Payid = 0,
                Pay_price = payprice,
                Cost = cost,
                Profit = profit,
                Order_state = (int)OrderStatus.WaitPay,//
                Pay_state = (int)PayStatus.NotPay,//支付模块未做出来前先设置为已支付
                Send_state = (int)SendCodeStatus.NotSend,
                Ticketcode = 0,//电子码未创建，支付后产生码赋值
                Rebate = 0,//  利润返佣金额暂时定为0
                Ordercome = "",//订购来源 暂时定为空
                U_traveldate = use_date_temp,
                Dealer = "自动",
                Comid = comid,
                Pno = "",
                Openid = "",
                Ticketinfo = "",
                Integral1 = 0,   //积分
                Imprest1 = 0,    //预付款
                Agentid = agentid,     //分销ID
                Agentsunid = 0,
                Warrantid = warrantid,   //授权ID
                Warrant_type = Warrant_type, //支付类型分销 1出票扣款 2验码扣款
                pickuppoint = "",
                dropoffpoint = "",
                Order_remark = "",
                Deliverytype = 0,
                Province = "",
                City = "",
                Address = "",
                Code = "",
                Shopcartid = 0,
                Express = 0,
                Child_u_num = 0,
                childreduce = pro.Childreduce,
                yanzheng_method = 0,
                baoxiannames = "",
                baoxianpinyinnames = "",
                baoxianidcards = "",
                channelcoachid = 0,
                aorderid = 0,

                travelnames = "",
                travelidcards = "",
                travelnations = "",
                travelphones = "",
                travelremarks = "",

                isInterfaceSub = isInterfaceSub, 
                Backtickettime=DateTime.Now
                
            };

            orderid = new B2bOrderData().InsertOrUpdate(order);
            if (orderid > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = "创建订单成功" });
            }
            else 
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = "创建订单失败" });
            }
           
        }


        public static string BackServerDeposit(int eticketid,int comid)
        {

            //查询押金列表
            string data = "";

            var Rentserverlist = new B2bEticketData().GetB2b_eticket_DepositBypno(eticketid);
            if (Rentserverlist != null)
            {
                //循环对所有押金服务进行退款
                for (int ji = 0; ji < Rentserverlist.Count; ji++)
                {
                    var orderid = Rentserverlist[ji].Depositorder; //相关订单
                    //var saleprice = Rentserverlist[ji].saleprice;  //销售价
                    var Depositprice = Rentserverlist[ji].Depositprice; //押金金额


                    var severdata=new RentserverData().Rentserverbyid(Rentserverlist[ji].sid,comid);
                    if (severdata == null)
                    {
                        data = "{\"type\":1,\"msg\":\"未查询到服务\"}";

                        return data;
                    }

                    if (severdata.Fserver == 0)
                    {//子服务 不涉及退押金
                        if (Rentserverlist[ji].Backdepositstate == 0)
                        {//退押金未标记的

                            decimal orderprice = 0;//此笔订单的支付金额
                            B2bPayData datapay = new B2bPayData();
                            B2b_pay modelb2pay = datapay.GetPayByoId(Rentserverlist[ji].Depositorder);
                            //var orderinfo = new B2bOrderData().GetOrderById(Rentserverlist[ji].Depositorder);

                            if (modelb2pay != null)
                            {
                                orderprice = modelb2pay.Total_fee;
                            }


                            //支付金额和押金必须都大于0否则无法退款
                            if (Depositprice > 0 && orderprice > 0)
                            {
                                /// <summary>
                                /// 订单附加服务押金 退款
                                ///  <param name="orderid">a订单id</param>
                                ///  <param name="rentserverid">服务id</param>
                                ///  <param name="ordertotalfee">订单支付总金额</param>
                                ///  <param name="refundfee">退款金额</param>
                                /// </summary>
                                /// <returns></returns>
                                try
                                {
                                    string retstr = OrderJsonData.Rentserver_Refund(orderid, Rentserverlist[ji].sid, orderprice, Depositprice, Rentserverlist[ji].id);
                                }
                                catch (Exception ex)
                                {


                                    data = "{\"type\":1,\"msg\":\"" + ex.Message + "\"}";

                                    return data;

                                }
                            }
                        }
                    }
                    else {
                        var backyajin = new B2bEticketData().UpbacketicketDepositstate(Rentserverlist[ji].id);//修改退押金状态 ,子服务 不涉及押金 ，直接自动修改状态为已退
                    }
                }

                data = "{\"type\":100,\"msg\":\"\"}";
                return data;
            }

            data = "{\"type\":1,\"msg\":\"没有查询到发卡记录\"}";

            return data;

        }
    
    }

}
