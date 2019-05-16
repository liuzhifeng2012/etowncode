using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;
using Newtonsoft.Json;
using System.Xml;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle.Enum;
using System.IO;
using System.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.VAS.Service.VASService.Data;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.PM.Service.Taobao_Ms.Model;
using ETS2.PM.Service.Taobao_Ms.Data;



namespace ETS.JsonFactory
{
    public class TwoCodeJsonData
    {
        //public static string key = "4Mds1hSvWkfTmNrWMv1KTIPj";

        private static object lockobj = new object();


        public static string GetReturnData(string oper, int poslogid)
        {

            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(oper);

                //读取xml 数据
                XmlNode xn = xdoc.SelectSingleNode("business_trans");
                string request_type = xn.SelectSingleNode("request_type").InnerXml;//类型包含（查询=Searchqrcode，验证=Verqrcode，重打=Restqrcode，日结=Dayend），
                string randomid = xn.SelectSingleNode("randomid").InnerXml;//随机id格式：yyyyMMddHHmmssfff
                string pos_id = xn.SelectSingleNode("pos_id").InnerXml;//pos机id         

                string key = "";//MD5加密秘钥
                if (pos_id == "")
                {
                    string backstr = ParamErr("posid参数不可为空");
                    return GetBackStr(backstr, poslogid);
                }
                else
                {
                    key = new B2bCompanyInfoData().Getmd5keybyposid(pos_id);
                    if (key == "")
                    {
                        string backstr = ParamErr("pos还没有绑定秘钥");
                        return GetBackStr(backstr, poslogid);
                    }
                }

                string security_md5 = xn.SelectSingleNode("security_md5").InnerXml;//md5加密字符串
                var returnrandomid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                var returnmd5 = EncryptionHelper.ToMD5(returnrandomid + pos_id + key, "UTF-8");

                string self_md5 = EncryptionHelper.ToMD5(randomid + pos_id + key, "UTF-8");

                if (self_md5.Trim() == security_md5.Trim())//验证md5加密
                {
                    #region 签到功能
                    if (request_type == "SignSystem")//签到功能
                    {
                        string useraccount = xn.SelectSingleNode("useraccount").InnerXml;
                        string userpwd = xn.SelectSingleNode("userpwd").InnerXml;
                        if (useraccount == "01" & userpwd == "0000")
                        {
                            string backstr = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                "<business_trans version=\"1.0\">" +
                                                    "<request_type>SignSystem</request_type>" +//类型
                                                    "<status>Success</status>" +//状态
                                                    "<Returnsinfo>签到成功</Returnsinfo>" +//返回错误，当错误时返回错误原因
                                                    "<randomid>" + returnrandomid + "</randomid>" +//随机编号
                                                    "<security_md5>" + returnmd5 + "</security_md5>" +//MD5加密
                                                "</business_trans>";
                            return GetBackStr(backstr, poslogid);
                        }
                        else
                        {
                            string backstr = ParamErr("此账号不存在或输入有误");
                            return GetBackStr(backstr, poslogid);
                        }
                    }
                    #endregion
                    #region 查询电子票
                    else if (request_type == "Searchrcode")//查询电子票
                    {
                        lock (lockobj)
                        {
                            string qrcode = xn.SelectSingleNode("qrcode").InnerXml;

                            if (qrcode != null && qrcode != "")
                            {
                                qrcode = EncryptionHelper.EticketPnoDES(qrcode, 1);//对码进行解密,对于大于20字符进行揭秘，码13位，身份证18位
                            }

                            //如果为空
                            if(qrcode==""){
                                 string backstr = ParamErr("未查询信息错误，请重新操作！");
                                 return GetBackStr(backstr, poslogid);
                            }

                           
                                //查询电子票信息
                                B2b_eticket eticketinfo = new B2bEticketData().GetEticketDetail(qrcode, pos_id);
                                if (eticketinfo != null)
                                {
                                    //--------------------增加对POS指定项目验证 peter写 2014-11-22----------------------------
                                    //产品如果指定了验证pos，则必须用指定pos验证
                                    string proBindPosid = new B2bComProData().GetProBindPosid(eticketinfo.Pro_id);
                                    if (proBindPosid != "")
                                    {
                                        if (proBindPosid != pos_id)
                                        {
                                            string errbackstr = ParamErr("和产品绑定Pos不符");
                                            return GetBackStr(errbackstr, poslogid);
                                        }
                                    }
                                    else
                                    {
                                        //查询Pos_id是否指定项目，如果指定 则和码的产品项目是否匹配。
                                        var projectid_temp = 0;//是否制定项目
                                        var projectid = 0;//实际项目
                                        var prodata = new B2bCompanyInfoData();
                                        var pos_temp = prodata.PosInfobyposid(int.Parse(pos_id));
                                        if (pos_temp != null)
                                        {
                                            projectid_temp = pos_temp.Projectid;
                                        }

                                        if (projectid_temp != 0)
                                        {//当POS指定了项目ID进行匹配
                                            var projectData = new B2b_com_projectData();
                                            projectid = projectData.GetProjectidByproid(eticketinfo.Pro_id);

                                            if (projectid_temp != projectid)
                                            {
                                                var backstr_temp = ParamErr("非匹配商户");
                                                return GetBackStr(backstr_temp, poslogid);
                                            }
                                        }
                                    }
                                    //--------------------增加对POS指定项目验证 peter写 2014-11-22--------------------------
                                    B2bOrderData orderdata = new B2bOrderData();
                                    B2b_order ordermodel = orderdata.GetOrderById(eticketinfo.Oid);
                                    //判断订单是否为多规格订单
                                    string pro_faceprice = eticketinfo.E_face_price.ToString("f2");
                                    if (ordermodel != null)
                                    {
                                        if (ordermodel.Speciid > 0)
                                        {
                                            B2b_com_pro_Speci mProSpeci = B2b_com_pro_SpeciData.Getgginfobyggid(ordermodel.Speciid);
                                            if (mProSpeci != null)
                                            {
                                                pro_faceprice = mProSpeci.speci_face_price.ToString("f2");
                                            }
                                        }
                                    }

                                    ////得到商户当天的 随机码
                                    string comdayrandomstr = new B2bCompanyData().GetComDayRandomstr(eticketinfo.Com_id, pos_id);

                                    string backstr = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                        "<business_trans version=\"1.0\">" +
                                                             "<request_type>Searchrcode</request_type>" +//类型
                                                              "<status>{0}</status>" +//状态
                                                              "<Returnsinfo>{1}</Returnsinfo>" +//返回错误，当错误时返回错误原因
                                                              "<randomid>{2}</randomid>" +//随机编号
                                                              "<qrcode>{3}</qrcode>" +//电子码
                                                              "<security_md5>{4}</security_md5>" +//MD5加密
                                                              "<pro_name>{5}</pro_name>" +//产品名称
                                                              "<face_price>{6}</face_price>" +//门市价
                                                              "<num>{7}</num>" +//可用数量
                                                               "<use_explain>{8}</use_explain>" +//使用说明
                                                         "</business_trans>", "Success", "电子票查询成功", returnrandomid, qrcode, returnmd5, eticketinfo.E_proname, pro_faceprice, eticketinfo.Use_pnum.ToString(), "验证后当天使用(" + comdayrandomstr + ")");
                                    if (eticketinfo.Use_pnum == 0)
                                    {
                                        backstr = ParamErr("此电子票已验证或不存在");
                                        return GetBackStr(backstr, poslogid);
                                    }
                                    else
                                    {
                                        return GetBackStr(backstr, poslogid);
                                    }


                                }
                                else
                                {
                                    string backstr = ParamErr("此电子票不存在或不是此商家产品");
                                    return GetBackStr(backstr, poslogid);
                                }

                        }
                    }
                    #endregion
                    #region 查询身份证返回电子票列表
                    else if (request_type == "Searchidcard")//查询身份证
                    {
                        lock (lockobj)
                        {
                            string qrcode = xn.SelectSingleNode("qrcode").InnerXml;

                           
                            //如果为空
                            if (qrcode == "")
                            {
                                string backstr = ParamErr("未查询信息错误，请重新操作！");
                                return GetBackStr(backstr, poslogid);
                            }

                            if (qrcode.Trim().Length == 18)
                            {//身份证查询 只有去空格等于18位则认为是身份证查询


                                //通过身份证号查询订单，并查询电子票，查询出有效的电子票

                                var eticketlist = new B2bEticketData().GetEticketListbyidcard(qrcode, pos_id);
                                if (eticketlist != null)
                                {
                                    var comid_temp = eticketlist[0].Com_id;

                                    var eticketlist_info = "";

                                    for (int i = 0; i < eticketlist.Count; i++)
                                    {
                                        eticketlist_info = "<ticketinfo><qrcode>" + eticketlist[i].Pno + "</qrcode>" +//电子码
                                        "<security_md5></security_md5>" +//MD5加密,为空
                                        "<pro_name>" + eticketlist[i].E_proname + "</pro_name>" +//产品名称
                                        "<face_price>" + eticketlist[i].E_face_price + "</face_price>" +//门市价
                                        "<num>" + eticketlist[i].Use_pnum + "</num></ticketinfo>";//可用数量
                                    }

                                    ////得到商户当天的 随机码
                                    string comdayrandomstr = new B2bCompanyData().GetComDayRandomstr(comid_temp, pos_id);

                                    string backstr = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                        "<business_trans version=\"1.0\">" +
                                                             "<request_type>Searchidcard</request_type>" +//类型
                                                              "<status>{0}</status>" +//状态
                                                              "<Returnsinfo>{1}</Returnsinfo>" +//返回错误，当错误时返回错误原因
                                                              "<randomid>{2}</randomid>" +//随机编号
                                                              "<ticketlist>" +
                                                              eticketlist_info +
                                                              "</ticketlist>" +
                                                               "<use_explain>{3}</use_explain>" +//使用说明

                                                         "</business_trans>", "Success", "身份证查询电子票成功", returnrandomid, "请选择使用的订单");

                                    return GetBackStr(backstr, poslogid);

                                }
                                else
                                {
                                    string backstr = ParamErr("未查询到此身份证订单或不是此商家产品！");
                                    return GetBackStr(backstr, poslogid);
                                }
                            }
                            else
                            {
                                    string backstr = ParamErr("身份证信息错误，只支持18位2代身份证");
                                    return GetBackStr(backstr, poslogid);
                                
                            }
                        }
                    }
                    #endregion
                    #region 验证电子票
                    else if (request_type == "Verqrcode")//验证电子票
                    {
                        lock (lockobj)
                        {
                            string qrcode = xn.SelectSingleNode("qrcode").InnerXml;
                            if (qrcode != null && qrcode != "")
                            {
                                qrcode = EncryptionHelper.EticketPnoDES(qrcode, 1);//对码进行解密
                            }


                            string num = xn.SelectSingleNode("num").InnerXml;


                            if (num.ConvertTo<int>(0) == 0)//判断输入是否为数字，以及是否大于0
                            {
                                string backstrr = ParamErr("输入必须为数字且大于0");
                                return GetBackStr(backstrr, poslogid);
                            }
                            else
                            {

                                //判断是否用同一个随机码验证电子票
                                //bool randomwhethersame = new B2bEticketLogData().GetRandomWhetherSame( (int)ECodeOper.ValidateECode, randomid);
                                B2b_eticket_log logg = new B2bEticketLogData().GetElogByRandomid(randomid, (int)ECodeOper.ValidateECode);

                                if (logg != null)
                                {
                                    string backstrr = ParamErr("验证电子票的随机码两次不可相同");
                                    return GetBackStr(backstrr, poslogid);
                                }
                                else
                                {


                                    //验证电子票信息
                                    B2b_eticket eticketinfo = new B2bEticketData().GetEticketDetail(qrcode, pos_id);
                                    if (eticketinfo != null)
                                    {

                                        //--------------------增加对POS指定项目验证 peter写 2014-11-22----------------------------
                                        //查询Pos_id是否指定项目，如果指定 则和码的产品项目是否匹配。
                                        var projectid_temp = 0;//是否制定项目
                                        var projectid = 0;//实际项目
                                        var prodata = new B2bCompanyInfoData();
                                        var pos_temp = prodata.PosInfobyposid(int.Parse(pos_id));
                                        if (pos_temp != null)
                                        {
                                            projectid_temp = pos_temp.Projectid;
                                        }

                                        if (projectid_temp != 0)
                                        {//当POS指定了项目ID进行匹配
                                            var projectData = new B2b_com_projectData();
                                            projectid = projectData.GetProjectidByproid(eticketinfo.Pro_id);

                                            if (projectid_temp != projectid)
                                            {
                                                var backstr_temp = ParamErr("非匹配商户");
                                                return GetBackStr(backstr_temp, poslogid);
                                            }
                                        }
                                        //--------------------增加对POS指定项目验证 peter写 2014-11-22--------------------------



                                        //验证电子票
                                        string returndata = EticketJsonData.EConfirm(qrcode, num, eticketinfo.Com_id.ToString(), int.Parse(pos_id), randomid);

                                        JsonCommonEntity entity = (JsonCommonEntity)JsonConvert.DeserializeObject(returndata, typeof(JsonCommonEntity));
                                        int type = entity.Type;
                                        string msg = entity.Msg;

                                        //根据电子票信息得到商家信息
                                        B2b_company companysummary = B2bCompanyData.GetCompany(eticketinfo.Com_id);

                                        //根据电子票信息得到出票单位（分销商）信息--（现阶段分销商未做，先显示商家信息）
                                        string agentcompany = "";//分销商名称,出票单位

                                        if (eticketinfo.Agent_id == 0)
                                        {
                                            agentcompany = companysummary.Com_name;//如果无分销商则视为 商家自己出票
                                        }
                                        else
                                        {
                                            //查询分销商获取分销商公司名称
                                            var agentinfo = AgentCompanyData.GetAgentByid(eticketinfo.Agent_id);
                                            if (agentinfo != null)
                                            {
                                                agentcompany = agentinfo.Company;
                                            }
                                        }

                                        //判断分销商是否为空
                                        if (agentcompany == "")
                                        {
                                            agentcompany = companysummary.Com_name;//分销商信息为空则归为商家出票
                                        }

                                        #region 产品有效期，验证时间(返回datetime.now)，使用说明(都返回"验证当天有效")，终端id(即posid)
                                        string use_explain = "验证后当天使用";//使用说明
                                        string validtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                        B2b_com_pro modelcompro = new B2bComProData().GetProById(eticketinfo.Pro_id.ToString());
                                        if (modelcompro == null)
                                        {
                                            string backstr = ParamErr("查询产品出错");//理论上不会出现此问题，只是做意外的屏蔽
                                            return GetBackStr(backstr, poslogid);
                                        }

                                        string provalidatemethod = modelcompro.ProValidateMethod;
                                        int appointdate = modelcompro.Appointdata;
                                        int iscanuseonsameday = modelcompro.Iscanuseonsameday;

                                        DateTime pro_end = modelcompro.Pro_end;
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

                                        B2bOrderData orderdata = new B2bOrderData();
                                        B2b_order ordermodel = orderdata.GetOrderById(eticketinfo.Oid);
                                        //如果是旅游大巴的话则 有效期为出行日期当日
                                        if (modelcompro.Server_type == 10)
                                        {
                                            pro_end = ordermodel.U_traveldate;
                                        }
                                        #endregion

                                        if (type == 100)
                                        {
                                            string ishasdeposit = "0";

                                            ////得到商户当天的 随机码
                                            string comdayrandomstr = new B2bCompanyData().GetComDayRandomstr(eticketinfo.Com_id, pos_id);

                                            //身份信息字符串
                                            string cardidstr = "";

                                            //打印索道票
                                            string Printticket_day = DateTime.Now.ToString("yyyy-MM-dd");
                                            string Printticket_startime = DateTime.Now.ToString("HH:mm");
                                            string Printticket_endtime = "17:00";
                                            string Printticket_Security = comdayrandomstr;
                                            string Printticket_pno = qrcode;

                                            if (num.ConvertTo<int>(0) > 0)
                                            {

                                                //打印索道票
                                                //if (modelcompro.worktimehour != 0)
                                                //{//必须有时长设置才继续执行

                                                //    if (modelcompro != null)
                                                //    {
                                                //        if (modelcompro.worktimeid != 0)
                                                //        {
                                                //            var worktimedata = new RentserverData().pro_worktimebyid(modelcompro.worktimeid, modelcompro.Com_id);
                                                //            if (worktimedata != null)
                                                //            {
                                                //                //加入 查询特殊时间设置 日期，工作时间默认表

                                                //                //修改如果有设定特定日期，读取特定日期
                                                //                b2b_com_pro_worktime_calendar r = new RentserverData().GetblackoutdateByWorktimeId(DateTime.Now.ToString("yyyy-MM-dd"), worktimedata.id);
                                                //                if (r != null)
                                                //                {
                                                //                    worktimedata.defaultstartime = r.startime;
                                                //                    worktimedata.defaultendtime = r.endtime;
                                                //                }
                                                //                //上班时间
                                                //                Printticket_startime = new RentserverData().worktimestarbijiao(worktimedata);

                                                //                worktimedata.defaultstartime = Printticket_startime;//重新获取开始时间

                                                //                //下班时间
                                                //                Printticket_endtime = new RentserverData().worktimeendbijiao(modelcompro.worktimehour, worktimedata);
                                                //            }
                                                //        }
                                                //        else
                                                //        {
                                                //            //上班时间大于结束时间则都按一个时间
                                                //            Printticket_startime = new RentserverData().worktimestar_endbijiao(Printticket_startime, Printticket_endtime);
                                                //        }
                                                //    }

                                                //}


                                                //自助取票机终端发卡,只要终端服务都发卡
                                                if (modelcompro.Wrentserver == 1)//必须是使用终端验证服务
                                                {

                                                    //对终端服务发卡
                                                    for (int i = 0; i < num.ConvertTo<int>(0); i++)
                                                    {
                                                        //0补齐+  comid(3-4)+orderid(6-7)+日期12位+第几个(001-999)+4位随机数
                                                        int headnum = 32 - eticketinfo.Com_id.ToString().Length - eticketinfo.Oid.ToString().Length - 12 - 3 - 4;
                                                        string headstr = "";
                                                        for (int j = 0; j < headnum; j++)
                                                        {
                                                            headstr += "0";
                                                        }
                                                        //生成32位用户卡号
                                                        var cardid_temp = headstr + eticketinfo.Com_id + eticketinfo.Oid + DateTime.Now.ToString("yyMMddHHmmss") + (i + 1).ToString().PadLeft(3, '0') + CommonFunc.RandomCode(4);
                                                        cardidstr += "<cardid>" + cardid_temp + "</cardid>";





                                                        //if (eticketinfo.Pnum == 1)//如果电子票一码多人次，未交押金都不行发卡
                                                        //{
                                                            //电子票是否支付押金了，并且未发卡，如果已发卡的则
                                                            //if (eticketinfo.ishasdeposit == 1)
                                                            //{

                                                                if (modelcompro != null)
                                                                {

                                                                    try
                                                                    {
                                                                        var Rentserver_User = new B2b_Rentserver_User();
                                                                        Rentserver_User.oid = eticketinfo.Oid;
                                                                        Rentserver_User.eticketid = eticketinfo.Id;
                                                                        Rentserver_User.cardid = cardid_temp;
                                                                        Rentserver_User.comid = eticketinfo.Com_id;
                                                                        Rentserver_User.serverstate = 1;
                                                                        Rentserver_User.Depositprice = 0;
                                                                        Rentserver_User.Depositorder = 0;
                                                                        Rentserver_User.Depositcome = "";
                                                                        Rentserver_User.Depositstate = 1; //0不需要押金，1//需要押金
                                                                        Rentserver_User.pname = eticketinfo.E_proname;
                                                                        Rentserver_User.sendnum = 0;
                                                                        Rentserver_User.sendstate = 0;
                                                                       
                                                                       
                                                                        DateTime nowtime = DateTime.Now;


                                                                        Rentserver_User.usenum = modelcompro.zhaji_usenum;

                                                                        if (modelcompro.worktimehour == 4)//4小时
                                                                            { 
                                                                                
                                                                                Rentserver_User.endtime = nowtime.AddHours(4).AddMinutes(15);//+4小时，15分钟
                                                                                //当时间大于17点的话按17点计算
                                                                                if (int.Parse(Rentserver_User.endtime.ToString("HH")) > 17)
                                                                                { 
                                                                                    Rentserver_User.endtime=DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")+" 17:00:00");
                                                                                }

                                                                            }
                                                                        if (modelcompro.worktimehour == 8)//1天
                                                                            {
                                                                                Rentserver_User.endtime = nowtime.AddHours(8).AddMinutes(15);//+8小时，15分钟
                                                                                 //当时间大于17点的话按17点计算
                                                                                if (int.Parse(Rentserver_User.endtime.ToString("HH")) > 17)
                                                                                {
                                                                                    Rentserver_User.endtime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 17:00:00");
                                                                                }
                                                                           }
                                                                        if (modelcompro.worktimehour == 12)//1.5天
                                                                            {
                                                                                Rentserver_User.endtime = nowtime.AddDays(1).AddHours(4).AddMinutes(30);//+1天，+4小时，15分钟



                                                                                if (int.Parse(Rentserver_User.endtime.ToString("HH")) > 17)
                                                                                {//如果是 小时大于17点 则 截止17点
                                                                                    Rentserver_User.endtime = DateTime.Parse(Rentserver_User.endtime.ToString("yyyy-MM-dd")+" 17:00:00");//+1天到中午
                                                                                }
                                                                            }
                                                                        if (modelcompro.worktimehour == 16)//2天
                                                                            {
                                                                                Rentserver_User.endtime = DateTime.Parse(nowtime.AddDays(1).ToString("yyyy-MM-dd") + " 17:00:00");//j今天一天，不管几点开始明天到下午
                                                                            }
                                                                       

                                                                        //插入用户表服务，只有产品需要终端验证服务时插入
                                                                        var inb2b_Rentserver_User_id_temp = new RentserverData().inb2b_Rentserver_User(Rentserver_User);

                                                                        //修改电子票记录 标记为已发卡
                                                                        var upetk = new B2bEticketData().Upeticketsencardstate(eticketinfo.Id, 1);

                                                                        ishasdeposit = "1";//当正常发卡后 给回传值发卡

                                                                        //查询 电子表附属押金表，所购买的服务
                                                                        var Rentserverlist = new B2bEticketData().GetB2b_eticket_DepositBypno(eticketinfo.Id);
                                                                        if (Rentserverlist != null)
                                                                        {
                                                                            for (int ji = 0; ji < Rentserverlist.Count; ji++)
                                                                            {
                                                                                var Rentserver_User_info_temp = new B2b_Rentserver_User_info();

                                                                                var rentserverdata = new RentserverData().GetRentServerById(Rentserverlist[ji].sid);
                                                                                Rentserver_User_info_temp.num = rentserverdata.num;
                                                                                Rentserver_User_info_temp.Rentserverid = Rentserverlist[ji].id;
                                                                                Rentserver_User_info_temp.Userid = inb2b_Rentserver_User_id_temp;
                                                                                Rentserver_User_info_temp.Verstate = 0;

                                                                                //对每一种服务进行插入
                                                                                var inb2b_Rentserver_User_info_temp = new RentserverData().inb2b_Rentserver_User_info(Rentserver_User_info_temp);
                                                                            }

                                                                        }
                                                                    }
                                                                    catch { }

                                                                }
                                                            //}
                                                       // }

                                                    }
                                                }
                                            }


                                            //判断订单是否为多规格订单
                                            string pro_faceprice = eticketinfo.E_face_price.ToString("f2");
                                            if (ordermodel != null)
                                            {
                                                if (ordermodel.Speciid > 0)
                                                {
                                                    B2b_com_pro_Speci mProSpeci = B2b_com_pro_SpeciData.Getgginfobyggid(ordermodel.Speciid);
                                                    if (mProSpeci != null)
                                                    {
                                                        pro_faceprice = mProSpeci.speci_face_price.ToString("f2");
                                                    }
                                                }
                                            }

                                            string backstr = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                       "<business_trans version=\"1.0\">" +
                                                           "<request_type>Verqrcode</request_type>" +//类型
                                                           "<status>{0}</status>" +//状态
                                                           "<Returnsinfo>{1}</Returnsinfo>" +//返回错误，当错误时返回错误原因
                                                           "<randomid>{2}</randomid>" +//随机编号+
                                                           "<qrcode>{3}</qrcode>" +//电子码
                                                           "<security_md5>{4}</security_md5>" +//MD5加密
                                                           "<pro_name>{5}</pro_name>" +//产品名称
                                                           "<face_price>{6}</face_price>" +//门市价
                                                           "<num>{7}</num>" +//验证数量
                                                           "<companyname>{8}</companyname>" +//商家名称
                                                           "<agentname>{9}</agentname>" +//出票单位
                                                           "<pos_id>{10}</pos_id>" +//终端id
                                                           "<pro_valid>{11}</pro_valid>" +//产品有效期
                                                           "<checktime>{12}</checktime>" +//验证时间
                                                            "<use_explain>{13}</use_explain>" +//使用说明
                                                            "<cardidlist>{14}</cardidlist>" +//身份信息
                                                            "<ishasdeposit>{15}</ishasdeposit>" +//是否有押金

                                                            "<Printticket_day>{16}</Printticket_day>" +//索道票使用日期
                                                            "<Printticket_startime>{17}</Printticket_startime>" +//索道票开始时间
                                                            "<Printticket_endtime>{18}</Printticket_endtime>" +//索道票结束时间
                                                            "<Printticket_Security>{19}</Printticket_Security>" +//防伪码（3位）
                                                            "<Printticket_pno>e{20}</Printticket_pno>" +//相关电子码
                                                            "<Printticket_operator>zizhu</Printticket_operator>" +//操作员
                                                       "</business_trans>", "Success", "电子票验证成功", returnrandomid, qrcode, returnmd5,
                                                       eticketinfo.E_proname, pro_faceprice, num,
                                                       companysummary.Com_name, agentcompany, pos_id, pro_end.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), use_explain + "(" + comdayrandomstr + ")", cardidstr, ishasdeposit, Printticket_day, Printticket_startime, Printticket_endtime, Printticket_Security, eticketinfo.Id);


                                            ////作为测试 验证延时 用的电子码
                                            //string vqrcodestr = "9119639550605,9119671843056,9119637396204";
                                            //if (vqrcodestr.IndexOf(qrcode) != -1)
                                            //{
                                            //    System.Threading.Thread.Sleep(20000);
                                            //}
                                            return GetBackStr(backstr, poslogid);

                                        }
                                        else
                                        {
                                            string backstr = ParamErr(msg);
                                            return GetBackStr(backstr, poslogid);
                                        }


                                    }
                                    else
                                    {
                                        string backstr = ParamErr("此电子票不存在或不是此商家产品");
                                        return GetBackStr(backstr, poslogid);
                                    }

                                }
                            }
                        }
                    }
                    #endregion
                    #region 电子票冲正
                    else if (request_type == "Reverse")//电子票冲正(把验证过的数据还原)
                    {
                        lock (lockobj)
                        {
                            string qrcode = xn.SelectSingleNode("qrcode").InnerXml;
                            if (qrcode != null && qrcode != "")
                            {
                                qrcode = EncryptionHelper.EticketPnoDES(qrcode, 1);//对码进行解密
                            }
                            string num = xn.SelectSingleNode("num").InnerXml;
                            B2b_eticket eticketinfo = new B2bEticketData().GetEticketDetail(qrcode, pos_id);
                            if (eticketinfo != null)
                            {
                                //根据电子票信息得到商家信息
                                B2b_company companysummary = B2bCompanyData.GetCompany(eticketinfo.Com_id);
                                if (companysummary != null)
                                {
                                    #region 已注释内容
                                    //#region  如果传入了淘宝码商验证信息，则发起 淘宝冲正申请(暂时只是用于手工在web.aspx页面冲正)
                                    ////以下是根据电子码和 验证数量 得到淘宝冲正所需字段的sql语句
                                    ////select top 1 order_id  from taobao_send_noticeretlog where verify_codes like '%电子码%' order by id desc
                                    ////select top 1 serial_num from taobao_consume_retlog where verify_code='电子码' and consume_num=验证数量 order by id desc
                                    ////select top 1 token from taobao_send_noticelog where order_id=(select top 1 order_id  from taobao_send_noticeretlog where verify_codes like '%电子码%' order by id desc) order by id desc

                                    //if (xn.SelectSingleNode("tb_order_id") != null && xn.SelectSingleNode("tb_selfdefine_serial_num") != null && xn.SelectSingleNode("tb_token") != null)
                                    //{
                                    //    string tb_order_id = xn.SelectSingleNode("tb_order_id").InnerXml;
                                    //    string tb_selfdefine_serial_num = xn.SelectSingleNode("tb_selfdefine_serial_num").InnerXml;
                                    //    string tb_token = xn.SelectSingleNode("tb_token").InnerXml;
                                    //    //淘宝订单号，对应淘宝核销流水号,安全验证token(需要和该订单淘宝发码通知中的token)
                                    //    string tb_xml = EticketJsonData.delayTime(1, tb_order_id, qrcode, num, tb_selfdefine_serial_num, tb_token, pos_id);
                                    //    if (string.IsNullOrEmpty(tb_xml))
                                    //    {
                                    //        string backstr = ParamErr("淘宝冲正申请失败");
                                    //        return GetBackStr(backstr, poslogid);
                                    //    }
                                    //}

                                    //#endregion
                                    #endregion

                                    #region 进行过冲正
                                    //获得冲正记录,判断是否进行过冲正
                                    B2b_eticket_log reverseelog = new B2bEticketLogData().GetElogByRandomid(randomid, (int)ECodeOper.Reverse);
                                    if (reverseelog != null)
                                    {

                                        if (reverseelog.A_state == (int)ECodeOperStatus.OperSuc)
                                        {
                                            #region 根据电子票信息得到出票单位（分销商）信息
                                            string agentcompany = "";//分销商名称,出票单位

                                            if (eticketinfo.Agent_id == 0)
                                            {
                                                agentcompany = companysummary.Com_name;//如果无分销商则视为 商家自己出票
                                            }
                                            else
                                            {
                                                //查询分销商获取分销商公司名称
                                                var agentinfo = AgentCompanyData.GetAgentByid(eticketinfo.Agent_id);
                                                if (agentinfo != null)
                                                {
                                                    agentcompany = agentinfo.Company;
                                                }
                                            }

                                            //判断分销商是否为空
                                            if (agentcompany == "")
                                            {
                                                agentcompany = companysummary.Com_name;//分销商信息为空则归为商家出票
                                            }
                                            #endregion

                                            #region 产品有效期，验证时间(返回datetime.now)，使用说明(都返回"验证当天有效")，终端id(即posid)
                                            string use_explain = "验证后请在当天使用";//使用说明
                                            string validtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                            B2b_com_pro modelcompro = new B2bComProData().GetProById(eticketinfo.Pro_id.ToString());
                                            if (modelcompro == null)
                                            {
                                                string backstr = ParamErr("查询产品有效期出错");
                                                return GetBackStr(backstr, poslogid);
                                            }

                                            string provalidatemethod = modelcompro.ProValidateMethod;
                                            int appointdate = modelcompro.Appointdata;
                                            int iscanuseonsameday = modelcompro.Iscanuseonsameday;

                                            DateTime pro_end = modelcompro.Pro_end;
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

                                            B2bOrderData orderdata = new B2bOrderData();
                                            B2b_order ordermodel = orderdata.GetOrderById(eticketinfo.Oid);
                                            //如果是旅游大巴的话则 有效期为出行日期当日
                                            if (modelcompro.Server_type == 10)
                                            {
                                                pro_end = ordermodel.U_traveldate;
                                            }
                                            #endregion


                                            string backstrr = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                            "<business_trans version=\"1.0\">" +
                                                                "<request_type>Reverse</request_type>" +//类型
                                                                "<status>{0}</status>" +//状态
                                                                "<Returnsinfo>{1}</Returnsinfo>" +//返回错误，当错误时返回错误原因
                                                                "<randomid>{2}</randomid>" +//随机编号+
                                                                "<qrcode>{3}</qrcode>" +//电子码
                                                                "<security_md5>{4}</security_md5>" +//MD5加密
                                                                "<pro_name>{5}</pro_name>" +//产品名称
                                                                "<face_price>{6}</face_price>" +//门市价
                                                                "<num>{7}</num>" +//验证数量
                                                                "<companyname>{8}</companyname>" +//商家名称
                                                                "<agentname>{9}</agentname>" +//出票单位
                                                                  "<pos_id>{10}</pos_id>" +//终端id
                                                                 "<pro_valid>{11}</pro_valid>" +//产品有效期
                                                                  "<checktime>{12}</checktime>" +//验证时间
                                                                   "<use_explain>{13}</use_explain>" +//使用说明
                                                            "</business_trans>", "Success", "电子票冲正成功", returnrandomid, qrcode, returnmd5, eticketinfo.E_proname, eticketinfo.E_face_price.ToString("f2"), num, companysummary.Com_name, agentcompany, pos_id, pro_end.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), use_explain);


                                            //删除 发的卡及服务
                                            var Reverse_Rentserver = new RentserverData().Reverse_Rentserver_User(qrcode);

                                            return GetBackStr(backstrr, poslogid);
                                        }
                                        else
                                        {
                                            string backstr = ParamErr("服务器端冲正出现错误");
                                            return GetBackStr(backstr, poslogid);
                                        }
                                    }
                                    #endregion
                                    #region 没有进行过冲正
                                    else
                                    {
                                        #region 如果含有验票记录，则进入冲正操作
                                        //得到需要冲正的 验票记录
                                        B2b_eticket_log elog = new B2bEticketLogData().GetElogByRandomid(randomid, (int)ECodeOper.ValidateECode);

                                        if (elog != null)//如果含有验票记录，则进入冲正操作
                                        {
                                            #region 验票操作失败
                                            if (elog.A_state == (int)ECodeOperStatus.OperFail)//验票操作不成功
                                            {
                                                string backstrrr = ParamErr(elog.A_remark);
                                                return GetBackStr(backstrrr, poslogid);

                                            }
                                            #endregion
                                            #region 验票操作成功,进行冲正
                                            else//验票操作成功
                                            {
                                                try
                                                {
                                                    var pro = new B2bComProData().GetProById(eticketinfo.Pro_id.ToString());//得到产品信息
                                                    B2b_order ordermodel = new B2bOrderData().GetOrderById(eticketinfo.Oid);

                                                    #region   淘宝码商冲正申请
                                                    //以下是根据电子码和 验证数量 得到淘宝冲正所需字段的sql语句
                                                    //select top 1 order_id  from taobao_send_noticeretlog where verify_codes like '%电子码%' order by id desc
                                                    //select top 1 serial_num from taobao_consume_retlog where verify_code='电子码' and consume_num=验证数量 and ycSystemRandomid='易城验证随机码' order by id desc
                                                    //select top 1 token from taobao_send_noticelog where order_id=(select top 1 order_id  from taobao_send_noticeretlog where verify_codes like '%电子码%' order by id desc) order by id desc

                                                    //判断是否是淘宝码商发码（得到淘宝发送通知返回日志）
                                                    Taobao_send_noticeretlog tb_sendnoticeretlog = new Taobao_send_noticeretlogData().GetSendRetLogByQrcode(qrcode);
                                                    if (tb_sendnoticeretlog != null)
                                                    {
                                                        //得到淘宝验证日志
                                                        Taobao_consume_retlog tb_consumelog = new Taobao_consume_retlogData().GetTaobao_consume_retlog(qrcode, num, randomid);
                                                        if (tb_consumelog == null)
                                                        {
                                                            string backstr = ParamErr("淘宝冲正申请失败:淘宝验码日志查询失败");
                                                            return GetBackStr(backstr, poslogid);
                                                        }
                                                        //得到淘宝发送通知日志 
                                                        Taobao_send_noticelog tb_sendnoticelog = new Taobao_send_noticelogData().GetSendNoticeLogByQrcode(qrcode);
                                                        if (tb_sendnoticelog == null)
                                                        {
                                                            string backstr = ParamErr("淘宝冲正申请失败:淘宝发送通知日志查询失败");
                                                            return GetBackStr(backstr, poslogid);
                                                        }
                                                        //淘宝订单号，对应淘宝核销流水号,安全验证token(需要和该订单淘宝发码通知中的token)
                                                        string tb_xml = EticketJsonData.delayTime(1, tb_sendnoticeretlog.order_id, qrcode, num, tb_consumelog.serial_num, tb_sendnoticelog.token, pos_id);
                                                        if (string.IsNullOrEmpty(tb_xml))
                                                        {
                                                            string backstr = ParamErr("淘宝冲正申请失败");
                                                            return GetBackStr(backstr, poslogid);
                                                        }
                                                    }
                                                    #endregion


                                                    //进行一次查询
                                                    var prodata = new B2bEticketData();
                                                    //向验票日志表中添加冲正日志 
                                                    B2bEticketLogData elogdata = new B2bEticketLogData();
                                                    B2b_eticket_log Reverselog = new B2b_eticket_log()
                                                    {
                                                        Id = 0,
                                                        Eticket_id = elog.Eticket_id,
                                                        Pno = elog.Pno,
                                                        Action = (int)ECodeOper.Reverse,
                                                        A_state = (int)ECodeOperStatus.OperFail,//初始状态为失败
                                                        A_remark = "电子票冲正",
                                                        Use_pnum = elog.Use_pnum,
                                                        Actiondate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                                                        Com_id = elog.Com_id,
                                                        PosId = elog.PosId,
                                                        RandomId = randomid
                                                    };

                                                    //电子票信息冲正
                                                    eticketinfo.Use_pnum = eticketinfo.Use_pnum + elog.Use_pnum;//剩余可用数量
                                                    if (eticketinfo.Pnum > eticketinfo.Use_pnum)
                                                    {
                                                        eticketinfo.V_state = (int)EticketCodeStatus.PartValidate;//电子票状态:部分验证
                                                    }
                                                    else
                                                    {
                                                        eticketinfo.V_state = (int)EticketCodeStatus.NotValidate;//电子票状态:未验证
                                                    }
                                                    int result1 = prodata.InsertOrUpdate(eticketinfo);

                                                    //电子票冲正日志表录入日志表
                                                    Reverselog.A_state = (int)ECodeOperStatus.OperSuc;
                                                    Reverselog.A_remark = "电子票冲正成功";
                                                    int result2 = elogdata.InsertOrUpdateLog(Reverselog);

                                                    //电子票已经冲正，随机码id对应的验码日志，验证状态为失败
                                                    elog.A_state = (int)ECodeOperStatus.OperFail;
                                                    int result3 = elogdata.InsertOrUpdateLog(elog);


                                                    if (eticketinfo.Oid != 0)
                                                    {
                                                        //查询订单 
                                                        if (ordermodel != null)
                                                        {
                                                            //判断授权类型为 验证扣款 = 2
                                                            if (ordermodel.Warrant_type == 2)
                                                            {
                                                                decimal overmoney = 0;
                                                                Agent_company agentmodel = AgentCompanyData.GetAgentCompanyByUid(ordermodel.Warrantid);//得到分销商公司信息
                                                                if (agentmodel != null)
                                                                {
                                                                    //分销商财务信息冲正
                                                                    overmoney = agentmodel.Imprest + ordermodel.Pay_price * elog.Use_pnum;

                                                                    Agent_Financial Financialinfo = new Agent_Financial
                                                                    {
                                                                        Id = 0,
                                                                        Com_id = eticketinfo.Com_id,
                                                                        Agentid = ordermodel.Agentid,
                                                                        Warrantid = ordermodel.Warrantid,
                                                                        Order_id = ordermodel.Id,
                                                                        Servicesname = pro.Pro_name + "[" + eticketinfo.Oid + "]",
                                                                        SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                                                                        Money = 0 + ordermodel.Pay_price * elog.Use_pnum,
                                                                        Payment = 0,            //收支(0=收款,1=支出)
                                                                        Payment_type = "验票回滚",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点,分销扣款）
                                                                        Money_come = "分销账户",         //资金来源（网上支付,银行收款，分销账户等）
                                                                        Over_money = overmoney

                                                                    };
                                                                    var fin = AgentCompanyData.EditAgentFinancial(Financialinfo);

                                                                    //分销预付款冲正
                                                                    AgentCompanyData.UpdateImprest(ordermodel.Warrantid, ordermodel.Pay_price * elog.Use_pnum);
                                                                }
                                                            }
                                                        }



                                                        #region 根据电子票信息得到出票单位（分销商）信息
                                                        string agentcompany = "";//分销商名称,出票单位

                                                        if (eticketinfo.Agent_id == 0)
                                                        {
                                                            agentcompany = companysummary.Com_name;//如果无分销商则视为 商家自己出票
                                                        }
                                                        else
                                                        {
                                                            //查询分销商获取分销商公司名称
                                                            var agentinfo = AgentCompanyData.GetAgentByid(eticketinfo.Agent_id);
                                                            if (agentinfo != null)
                                                            {
                                                                agentcompany = agentinfo.Company;
                                                            }
                                                        }

                                                        //判断分销商是否为空
                                                        if (agentcompany == "")
                                                        {
                                                            agentcompany = companysummary.Com_name;//分销商信息为空则归为商家出票
                                                        }
                                                        #endregion

                                                        #region 产品有效期，验证时间(返回datetime.now)，使用说明(都返回"验证当天有效")，终端id(即posid)
                                                        string use_explain = "验证后请在当天使用";//使用说明
                                                        string validtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                                        //B2b_com_pro modelcompro = new B2bComProData().GetProById(eticketinfo.Pro_id.ToString());
                                                        B2b_com_pro modelcompro = pro;
                                                        if (modelcompro == null)
                                                        {
                                                            string backstr = ParamErr("查询产品有效期出错");
                                                            return GetBackStr(backstr, poslogid);
                                                        }

                                                        string provalidatemethod = modelcompro.ProValidateMethod;
                                                        int appointdate = modelcompro.Appointdata;
                                                        int iscanuseonsameday = modelcompro.Iscanuseonsameday;

                                                        DateTime pro_end = modelcompro.Pro_end;
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

                                                        //如果是旅游大巴的话则 有效期为出行日期当日
                                                        if (modelcompro.Server_type == 10)
                                                        {
                                                            pro_end = ordermodel.U_traveldate;
                                                        }
                                                        #endregion



                                                        string backstrr = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                                     "<business_trans version=\"1.0\">" +
                                                                         "<request_type>Reverse</request_type>" +//类型
                                                                         "<status>{0}</status>" +//状态
                                                                         "<Returnsinfo>{1}</Returnsinfo>" +//返回错误，当错误时返回错误原因
                                                                         "<randomid>{2}</randomid>" +//随机编号+
                                                                         "<qrcode>{3}</qrcode>" +//电子码
                                                                         "<security_md5>{4}</security_md5>" +//MD5加密
                                                                         "<pro_name>{5}</pro_name>" +//产品名称
                                                                         "<face_price>{6}</face_price>" +//门市价
                                                                         "<num>{7}</num>" +//验证数量
                                                                         "<companyname>{8}</companyname>" +//商家名称
                                                                         "<agentname>{9}</agentname>" +//出票单位
                                                                            "<pos_id>{10}</pos_id>" +//终端id
                                                                 "<pro_valid>{11}</pro_valid>" +//产品有效期
                                                                  "<checktime>{12}</checktime>" +//验证时间
                                                                   "<use_explain>{13}</use_explain>" +//使用说明
                                                                     "</business_trans>", "Success", "电子票冲正成功", returnrandomid, qrcode, returnmd5, eticketinfo.E_proname, eticketinfo.E_face_price.ToString("f2"), num, companysummary.Com_name, agentcompany, pos_id, pro_end.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), use_explain);
                                                        //System.Threading.Thread.Sleep(9000);

                                                        //删除 发的卡及服务
                                                        var Reverse_Rentserver = new RentserverData().Reverse_Rentserver_User(qrcode);

                                                        return GetBackStr(backstrr, poslogid);
                                                    }
                                                    else
                                                    {
                                                        string backstr = "电子票表中订单号为0";
                                                        return GetBackStr(backstr, poslogid);
                                                    }
                                                }
                                                catch (Exception e)
                                                {
                                                    string backstr = ParamErr(e.Message);
                                                    return GetBackStr(backstr, poslogid);

                                                }
                                            }
                                            #endregion
                                        }
                                        #endregion
                                        #region 不含验票记录(服务器这边没有接收到pos机发送过来的验票请求，未进行过验票操作)，则直接返回"电子票未验证过，无需冲正"
                                        else
                                        {
                                            string backstr = ParamErr("电子票未验证过，无需冲正");
                                            return GetBackStr(backstr, poslogid);
                                        }
                                        #endregion

                                    }
                                    #endregion

                                }
                                else
                                {
                                    string backstr = ParamErr("此电子票商家信息获取有误");
                                    return GetBackStr(backstr, poslogid);
                                }
                            }
                            else
                            {

                                string backstr = ParamErr("此电子票不存在或不是此商家产品");
                                return GetBackStr(backstr, poslogid);
                            }
                        }
                    }
                    #endregion
                    #region 重打电子票
                    else if (request_type == "Restqrcode")//重打电子票
                    {
                        lock (lockobj)
                        {
                            try
                            {
                                var finalecodelog = new B2bEticketLogData().GetFinalPrintEticketLogByPosid(pos_id);//得到pos机最后验证的电子票日志信息

                                if (finalecodelog != null)
                                {
                                    var finaleticket = new B2bEticketData().GetEticketDetail(finalecodelog.Pno, pos_id);

                                    if (finaleticket != null)
                                    {
                                        //根据电子票信息得到商家信息
                                        B2b_company companysummary = B2bCompanyData.GetCompany(finaleticket.Com_id);

                                        //根据电子票信息得到出票单位（分销商）信息--（现阶段分销商未做，先显示商家信息）
                                        string agentcompany = "";//分销商名称,出票单位

                                        B2bEticketData eticketdate = new B2bEticketData();
                                        var eticketinfo = eticketdate.GetEticketByID(finalecodelog.Eticket_id.ToString());
                                        if (eticketinfo != null)
                                        {
                                            if (eticketinfo.Agent_id == 0)
                                            {
                                                agentcompany = companysummary.Com_name;//如果无分销商则视为 商家自己出票
                                            }
                                            else
                                            {
                                                //查询分销商获取分销商公司名称
                                                var agentinfo = AgentCompanyData.GetAgentByid(eticketinfo.Agent_id);
                                                if (agentinfo != null)
                                                {
                                                    agentcompany = agentinfo.Company;
                                                }
                                            }
                                        }

                                        //判断分销商是否为空
                                        if (agentcompany == "")
                                        {
                                            agentcompany = companysummary.Com_name;//分销商信息为空则归为商家出票
                                        }




                                        B2b_eticket_log elog = new B2b_eticket_log()
                                        {
                                            Id = 0,
                                            Eticket_id = finaleticket.Id,
                                            Pno = finaleticket.Pno,
                                            Action = (int)ECodeOper.RePrintEcode,
                                            A_state = (int)ECodeOperStatus.OperSuc,
                                            A_remark = "重打最后一张电子票" + finaleticket.Pno,
                                            Use_pnum = finalecodelog.Use_pnum,
                                            Actiondate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                                            Com_id = finaleticket.Com_id,
                                            PosId = int.Parse(pos_id),
                                            JsId = 0
                                        };
                                        var verifyecodelogid = new B2bEticketLogData().InsertOrUpdateLog(elog);
                                        if (verifyecodelogid == 0)
                                        {
                                            string backstr = ParamErr("录入验码日志出错");
                                            return GetBackStr(backstr, poslogid);
                                        }
                                        else
                                        {
                                            string backstr = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                                    "<business_trans version=\"1.0\">" +
                                                                        "<request_type>Restqrcode</request_type> " +
                                                                        "<status>{0}</status> " +
                                                                        "<Returnsinfo>{1}</Returnsinfo>" +
                                                                        "<randomid>{2}</randomid> " +
                                                                        "<qrcode>{3}</qrcode> " +
                                                                        "<security_md5>{4}</security_md5>" +
                                                                        "<pro_name>{5}</pro_name> " +
                                                                        "<face_price>{6}</face_price> " +
                                                                        "<num>{7}</num> " +
                                                                        "<companyname>{8}</companyname>" +//商家名称
                                                                        "<agentname>{9}</agentname>" +//出票单位
                                                                    "</business_trans>", "Success", "重打电子票成功", returnrandomid, finalecodelog.Pno, returnmd5, finaleticket.E_proname, finaleticket.E_face_price.ToString("f2"), finalecodelog.Use_pnum, companysummary.Com_name, agentcompany);
                                            return GetBackStr(backstr, poslogid);
                                        }
                                    }
                                    else
                                    {
                                        string backstr = ParamErr("未找到此pos机最后打印的电子票信息");
                                        return GetBackStr(backstr, poslogid);
                                    }

                                }
                                else
                                {
                                    string backstr = ParamErr("未找到此pos机的验码信息");
                                    return GetBackStr(backstr, poslogid);
                                }

                            }
                            catch (Exception ex)
                            {
                                string backstr = ParamErr("出现意外错误");
                                return GetBackStr(backstr, poslogid);
                            }
                        }
                    }
                    #endregion
                    #region 日结
                    else if (request_type == "Dayend")//日结
                    {

                        var DayJSdata = new DayJieSuanData();
                        var dsdayjs = DayJSdata.DayJSListByPosId(pos_id);
                        if (dsdayjs == null || dsdayjs.Tables[0].Rows.Count == 0)
                        {
                            string backstr = ParamErr("日结信息为空");
                            return GetBackStr(backstr, poslogid);
                        }
                        else
                        {
                            string backstr = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                    "<business_trans version=\"1.0\">" +
                                                        "<request_type>Dayend</request_type>" +//类型
                                                        "<status>Success</status>" +//状态
                                                        "<Returnsinfo>日结操作成功</Returnsinfo>" +//返回错误，当错误时返回错误原因
                                                        "<randomid>" + returnrandomid + "</randomid>" +//随机编号
                                                        "<pos_id>" + pos_id + "</pos_id>" +//POS编号
                                                        "<security_md5>" + returnmd5 + "</security_md5>" +//MD5加密
                                                        "<pro_list>";
                            foreach (DataRow row in dsdayjs.Tables[0].Rows)
                            {
                                backstr += "<product>" +
                                            "<pro_name>" + row["proname"].ToString() + "</pro_name>" +//产品名称
                                            "<ver_count>" + row["TotalVerifyNum"].ToString() + "</ver_count>" +//验票笔数
                                            "<ver_num>" + row["TotalConsumedNum"].ToString() + "</ver_num>" +//验票数量
                                            "</product>";
                            }
                            backstr += "</pro_list>" +
                                        "</business_trans>";


                            return GetBackStr(backstr, poslogid);
                        }

                    }
                    #endregion
                    #region 查询服务接口/退押金查询接口
                    else if (request_type == "SearchService")
                    {
                        lock (lockobj)
                        {
                            try
                            {
                                string qrcode = xn.SelectSingleNode("qrcode").InnerXml;//芯片id
                                if (qrcode.Trim() == "")
                                {
                                    string backstr1 = ParamErr("卡号不可为空");
                                    return GetBackStr(backstr1, poslogid);
                                }
                                //IDataReader reader = ExcelSqlHelper.ExecuteReader("select usestate from wanlong_yzservice where qrcode='" + qrcode + "'");
                                //int usestate = 0;
                                //if (reader.Read())
                                //{
                                //    usestate = reader.GetInt32(0);
                                //}

                                //首先查询 服务内容 //pos_id
                                var rentserverdata = new RentserverData().RentserverListbyposid(int.Parse(pos_id));
                                if (rentserverdata.Count == 0)
                                {
                                    string backstr1 = ParamErr("POSid未绑定服务");
                                    return GetBackStr(backstr1, poslogid);
                                }

                                string rentservername = "";
                                int comid = 0;
                                int servertype = -1;
                                string rentserveridlist = "";
                                for (int i = 0; i < rentserverdata.Count; i++)
                                {

                                    rentservername += rentserverdata[i].servername;//一般如果是一个在这赋值了，下面还要重新查询所有购买的项目
                                    rentserveridlist += rentserverdata[i].id + ",";


                                    if (comid == 0)
                                    {
                                        comid = rentserverdata[i].comid;
                                    }

                                    if (servertype == -1)
                                    {
                                        servertype = rentserverdata[i].servertype;
                                    }
                                }


                                if (rentserveridlist != "")
                                {
                                    rentserveridlist = rentserveridlist.Substring(0, rentserveridlist.Length - 1);
                                }



                                B2b_company companysummary = B2bCompanyData.GetCompany(comid);
                                if (companysummary == null)
                                {
                                    string backstr3 = ParamErr("验证POSid未找到指定商家");
                                    return GetBackStr(backstr3, poslogid);
                                }


                                if (servertype == 0)
                                {//普通验证服务

                                    //查询用户押金是否正常
                                    var entserver_User = new RentserverData().SearchRentserver_User(0, qrcode);
                                    if (entserver_User == null)
                                    {
                                        string backstr1 = ParamErr("未查询到此卡用户");
                                        return GetBackStr(backstr1, poslogid);
                                    }

                                    if (entserver_User.comid != comid)
                                    {
                                        string backstr1 = ParamErr("绑定POSid与商户不匹配");
                                        return GetBackStr(backstr1, poslogid);
                                    }


                                    if (entserver_User.serverstate == 2)
                                    {
                                        string backstr1 = ParamErr("此卡服务已结束，并已退押金");
                                        return GetBackStr(backstr1, poslogid);
                                    }

                                    if (entserver_User.subtime.ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd"))
                                    {
                                        //string backstr1 = ParamErr("此卡领取后只限制当天使用，领卡日期：" + entserver_User.subtime.ToString("yyyy-MM-dd hh:mm"));
                                        //return GetBackStr(backstr1, poslogid);
                                    }

                                    //再次查询否包含服务名称
                                    var Rentserver_list_temp = new RentserverData().SearchRentserver_User_list(entserver_User.id, rentserveridlist);
                                    if (Rentserver_list_temp.Count == 0)
                                    {
                                        string backstr1 = ParamErr("未查询到此卡购买的小件");
                                        return GetBackStr(backstr1, poslogid);
                                    }
                                    else
                                    {
                                        rentservername = ""; //重新绑定已购买的服务
                                        for (int i = 0; i < Rentserver_list_temp.Count; i++)
                                        {
                                            rentservername += Rentserver_list_temp[i].servername + ",";
                                        }
                                    }

                                    if (rentservername != "")
                                    {
                                        if (rentservername.Substring(rentservername.Length - 1, 1) == ",")
                                        {
                                            rentservername = rentservername.Substring(0, rentservername.Length - 1);
                                        }
                                    }

                                    //查询此卡的所有状态
                                    int num = 0;
                                    int Rentserver_Verstate = 2;//默认为已完成
                                    DateTime guihuantime = DateTime.Now;
                                    var Rentserver_list_state_temp = new RentserverData().SearchRentserver_User_list_state(entserver_User.id, rentserveridlist);
                                    if (Rentserver_list_state_temp.Count == 0)
                                    {
                                        string backstr1 = ParamErr("未查询到购买此终端可提供的小件服务");
                                        return GetBackStr(backstr1, poslogid);
                                    }
                                    else
                                    {
                                        string beizhu = "";
                                        
                                        for (int i = 0; i < Rentserver_list_state_temp.Count; i++)
                                        {
                                            if (Rentserver_list_state_temp[i].Verstate == 2)
                                            {
                                                //string backstr1 = ParamErr("在此验证的服务已办理过了。");
                                                //return GetBackStr(backstr1, poslogid);
                                                num = Rentserver_list_state_temp[i].num;//获取剩余次数
                                                guihuantime = Rentserver_list_state_temp[i].Rettime;
                                            }
                                            else
                                            {//如果有不等于2的 就是未按未结束的服务办理，例如：当一个服务 不需要归还的时候，另一个服务需要归还，第一次领的时候，都可以，而归还的时候，只归还需要归还的产品
                                                Rentserver_Verstate = Rentserver_list_state_temp[i].Verstate;//读取到一个记录即可，因为一起验证所以应该同样的，如果出现一个异常整体验证不了
                                                num=Rentserver_list_state_temp[i].num;//获取剩余次数
                                                beizhu  = Rentserver_list_state_temp[i].Remarks.Trim();
                                                guihuantime = Rentserver_list_state_temp[i].Rettime;
                                            }
                                        }

                                        if (beizhu != "") {
                                            beizhu = "(" + beizhu + ")";
                                        }

                                        if (Rentserver_Verstate == 0)
                                        {
                                            rentservername = "租用:" + rentservername;
                                        }
                                        if (Rentserver_Verstate == 1)
                                        {
                                            rentservername = "归还:" + rentservername + beizhu;
                                        }

                                    }

                                    if (Rentserver_Verstate == 2)//所有的服务验证都已经归还了则弹出错误
                                    {
                                        if (num <= 0)
                                        {
                                            string backstr1 = ParamErr("在此小件已经办理过了。");
                                            return GetBackStr(backstr1, poslogid);
                                        }
                                        else {
                                            //并且 归还日期等于当天
                                            if (guihuantime.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd")) {
                                                string backstr1 = ParamErr("在此小件今天已经办理过了。");
                                                return GetBackStr(backstr1, poslogid);
                                            }
                                        }

                                        Rentserver_Verstate = 0;//对于大于1天的服务，直接归为 未领取

                                    }



                                    string backstr = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                                        "<business_trans version=\"1.0\">" +
                                                                            "<request_type>SearchrService</request_type>" +
                                                                            "<status>{0}</status>" +// //Success ,Error
                                                                            "<Returnsinfo>{1}</Returnsinfo>" +////返回错误，当错误时返回错误原因
                                                                            "<randomid>{2}</randomid>" +
                                                                            "<security_md5>{3}</security_md5>" +
                                                                            "<qrcode>{4}</qrcode>" +////用户卡号ID，用之前输入电子票的参数即可
                                                                            "<Server_name>{5}</Server_name>" +
                                                                            "<usestate>{6}</usestate>" + ////使用状态  0未使用 1已领取未归还 2服务完成（如果不需要归还的直接完成，如果需要归还的在归还后完成）
                                                                            "<Timeoutmoney>0</Timeoutmoney>" +//超时应收金额,如果不等于0则需要弹出提示 收取超时金额
                                                                            "<TimeoutMinute>0</TimeoutMinute>" +//超时时间 分钟
                                                                        "</business_trans>", "Success", "查询服务成功", returnrandomid, returnmd5, qrcode, rentservername, Rentserver_Verstate);
                                    return GetBackStr(backstr, poslogid);
                                }
                                else
                                {//退款服务现在只限定这两种

                                    var entserver_User = new RentserverData().SearchRentserver_User(0, qrcode);
                                    if (entserver_User == null)
                                    {
                                        string backstr1 = ParamErr("未查询到此卡用户");
                                        return GetBackStr(backstr1, poslogid);
                                    }

                                    if (entserver_User.serverstate == 2)
                                    {
                                        string backstr1 = ParamErr(rentservername + "此卡服务已结束，并已退押金");
                                        return GetBackStr(backstr1, poslogid);
                                    }

                                    //判断 结束日期是否为今天，如果小与今天 ，则可以退卡，不然后台才能强制退押金，自助不提供退押金
                                    if (DateTime.Parse(entserver_User.endtime.ToString("yyyy-MM-dd")) > DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
                                    {
                                        string backstr1 = ParamErr("此卡结束日期为：" + entserver_User.endtime.ToString("yyyy-MM-dd") + ",自助系统只提供当天退押金。如还需退押金，请联系管理员后台强制退押，强制退押后此卡将回收不能继续使用！");
                                        return GetBackStr(backstr1, poslogid);
                                    }

                                    //再次查询是否包含此服务
                                    int count_temp = 0;
                                    int overstate = 0;//服务是否都已完成
                                    string overerr = "";//未完成内容
                                    var Rentserver_User_info = new RentserverData().Rentserver_User_infopagelist(1, 50, entserver_User.id, out count_temp);
                                    if (Rentserver_User_info.Count == 0)
                                    {
                                        string backstr1 = ParamErr(rentservername + "此卡服务已结束");
                                        return GetBackStr(backstr1, poslogid);
                                    }
                                    for (int i = 0; i < Rentserver_User_info.Count; i++)
                                    {

                                        if (Rentserver_User_info[i].Verstate == 1)
                                        {
                                            overstate = 1;

                                            var rentserverdata_t = new RentserverData().Rentserverbyuserinfoid(Rentserver_User_info[i].Rentserverid, comid);
                                            if (rentserverdata_t != null)
                                            {
                                                overerr += rentserverdata_t.servername + "未归还! ";
                                            }

                                        }
                                    }

                                    if (overstate == 1)
                                    {
                                        string backstr1 = ParamErr(overerr);
                                        return GetBackStr(backstr1, poslogid);
                                    }

                                    //超时计算

                                    int Timeoutmoney = 0;//超时金额
                                    int TimeoutMinute = 0;//超时时间

                                    var rentserveruserinfo_temp = new RentserverData().SearchRentserver_User_outtime(entserver_User.id);
                                    if (rentserveruserinfo_temp != null)
                                    {
                                        if (rentserveruserinfo_temp.Verstate == 2)
                                        {//如果未领取 就不考虑 超时问题

                                            var order_temp = new B2bOrderData().GetOrderById(entserver_User.oid);
                                            if (order_temp != null)
                                            {
                                                var pro_temp = new B2bComProData().GetProById(order_temp.Pro_id.ToString());
                                                if (pro_temp != null)
                                                {
                                                    if (pro_temp.worktimehour != 0)
                                                    {

                                                        var Vertime = rentserveruserinfo_temp.Vertime;
                                                        var Rettime = rentserveruserinfo_temp.Rettime;

                                                        var ts = (Rettime - Vertime);
                                                        int SUB = Convert.ToInt32(ts.TotalMinutes);

                                                        int timecha = SUB - (pro_temp.worktimehour * 60);//按分钟进行计算

                                                        if (timecha > 0)
                                                        {
                                                            Timeoutmoney = timecha;
                                                            TimeoutMinute = timecha;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }



                                    string backstr = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                                        "<business_trans version=\"1.0\">" +
                                                                            "<request_type>SearchrService</request_type>" +
                                                                            "<status>{0}</status>" +// //Success ,Error
                                                                            "<Returnsinfo>{1}</Returnsinfo>" +////返回错误，当错误时返回错误原因
                                                                            "<randomid>{2}</randomid>" +
                                                                            "<security_md5>{3}</security_md5>" +
                                                                            "<qrcode>{4}</qrcode>" +////用户卡号ID，用之前输入电子票的参数即可
                                                                            "<Server_name>{5}</Server_name>" +
                                                                            "<usestate>{6}</usestate>" + ////使用状态  0未使用 1已领取未归还 2服务完成（如果不需要归还的直接完成，如果需要归还的在归还后完成）
                                                                            "<Timeoutmoney>{7}</Timeoutmoney>" +//超时应收金额,如果不等于0则需要弹出提示 收取超时金额
                                                                            "<TimeoutMinute>{8}</TimeoutMinute>" +//超时时间 分钟
                                                                        "</business_trans>", "Success", "所有服务已归还", returnrandomid, returnmd5, qrcode, rentservername, overstate, Timeoutmoney, TimeoutMinute);
                                    return GetBackStr(backstr, poslogid);

                                }

                            }
                            catch (Exception ex)
                            {
                                string backstr = ParamErr("出现意外错误");
                                return GetBackStr(backstr, poslogid);
                            }
                        }
                    }
                    #endregion
                    #region 验证服务接口/退押金确认接口
                    else if (request_type == "VerqService")
                    {
                        lock (lockobj)
                        {
                            try
                            {
                                string qrcode = xn.SelectSingleNode("qrcode").InnerXml;//用户卡号ID，用之前输入电子票的参数即可
                                string verqstate = xn.SelectSingleNode("verqstate").InnerXml;//请求验证服务 1为领取服务， 2为归还服务 
                                string beizhu = "";
                                if (qrcode.Trim() == "" || verqstate.Trim() == "")
                                {
                                    string backstr1 = ParamErr("卡号和服务状态不可为空");
                                    return GetBackStr(backstr1, poslogid);
                                }

                                if (verqstate.Contains("$"))
                                {
                                    //包含
                                    var verqstatearr = verqstate.Split('$');
                                    verqstate = verqstatearr[0];

                                    if (verqstatearr.Count() > 1) {
                                        beizhu = verqstatearr[1].Trim();
                                    }
                                }






                                //首先查询 服务内容 //pos_id
                                var rentserverdata = new RentserverData().RentserverListbyposid(int.Parse(pos_id));
                                if (rentserverdata.Count == 0)
                                {
                                    string backstr1 = ParamErr("POSid未绑定服务");
                                    return GetBackStr(backstr1, poslogid);
                                }

                                string rentservername = "";
                                int comid = 0;
                                int servertype = -1;
                                string rentserveridlist = "";
                                for (int i = 0; i < rentserverdata.Count; i++)
                                {

                                    rentservername += rentserverdata[i].servername;//一般如果是一个在这赋值了，下面还要重新查询所有购买的项目
                                    rentserveridlist += rentserverdata[i].id + ",";


                                    if (comid == 0)
                                    {
                                        comid = rentserverdata[i].comid;
                                    }

                                    if (servertype == -1)
                                    {
                                        servertype = rentserverdata[i].servertype;
                                    }
                                }

                                if (rentserveridlist != "")
                                {
                                    rentserveridlist = rentserveridlist.Substring(0, rentserveridlist.Length - 1);
                                }



                                B2b_company companysummary = B2bCompanyData.GetCompany(comid);
                                if (companysummary == null)
                                {
                                    string backstr3 = ParamErr("验证POSid未找到指定商家");
                                    return GetBackStr(backstr3, poslogid);
                                }

                                string companyname = companysummary.Com_name;


                                if (servertype == 0)
                                {//普通验证服务

                                    int Printticket_temp = 0;//判断是否为打印索道票

                                    //查询用户押金是否正常
                                    var entserver_User = new RentserverData().SearchRentserver_User(0, qrcode);
                                    if (entserver_User == null)
                                    {
                                        string backstr1 = ParamErr("未查询到此卡用户");
                                        return GetBackStr(backstr1, poslogid);
                                    }

                                    if (entserver_User.comid != comid)
                                    {
                                        string backstr1 = ParamErr("绑定POSid与商户不匹配");
                                        return GetBackStr(backstr1, poslogid);
                                    }


                                    //判断押金是否正常
                                    if (entserver_User.serverstate == 2)
                                    {
                                        string backstr1 = ParamErr("此卡服务已结束，并已退押金");
                                        return GetBackStr(backstr1, poslogid);
                                    }

                                    //再次查询用户所订购包含服务名称
                                    var Rentserver_list_temp = new RentserverData().SearchRentserver_User_list(entserver_User.id, rentserveridlist);
                                    if (Rentserver_list_temp.Count == 0)
                                    {
                                        string backstr1 = ParamErr("未查询到此卡购买的小件服务");
                                        return GetBackStr(backstr1, poslogid);
                                    }
                                    else
                                    {
                                        //对购买的服务重新分配服务id
                                        rentserveridlist = "";
                                        rentservername = "";
                                        for (int i = 0; i < Rentserver_list_temp.Count; i++)
                                        {
                                            rentservername += Rentserver_list_temp[i].servername + ",";
                                            rentserveridlist += Rentserver_list_temp[i].id + ",";
                                        }
                                        if (rentserveridlist != "")
                                        {
                                            rentserveridlist = rentserveridlist.Substring(0, rentserveridlist.Length - 1);
                                        }
                                    }

                                    //去掉最右侧的 ，号
                                    if (rentservername != "")
                                    {
                                        if (rentservername.Substring(rentservername.Length - 1, 1) == ",")
                                        {
                                            rentservername = rentservername.Substring(0, rentservername.Length - 1);
                                        }
                                    }

                                    //查询此卡的所有状态
                                    string use_explain = "";
                                    int Rentserver_Verstate = 0;
                                    var Rentserver_list_state_temp = new RentserverData().SearchRentserver_User_list_state(entserver_User.id, rentserveridlist);
                                    if (Rentserver_list_state_temp.Count == 0)
                                    {
                                        string backstr1 = ParamErr("未查询到此卡购买的小件服务");
                                        return GetBackStr(backstr1, poslogid);
                                    }
                                    else
                                    {
                                        for (int i = 0; i < Rentserver_list_state_temp.Count; i++)
                                        {
                                            if (Rentserver_list_state_temp[i].Verstate == 2)//因为 领取的时候未0 而归还的时候遇到不需要归还的产品就应该跳过，只对归还进行操作
                                            {
                                                //string backstr1 = ParamErr("在此验证的服务已办理过了。");
                                                //return GetBackStr(backstr1, poslogid);
                                            }
                                            #region 判断验证服务传递是否正确
                                            //IDataReader reader = ExcelSqlHelper.ExecuteReader("select top 1 usestate from wanlong_yzservice where qrcode='" + qrcode + "' order by id desc");
                                            int usestate = entserver_User.serverstate;//默认的服务状态 0 未使用 1已领取 2 已归还


                                            if (Rentserver_list_state_temp[i].num > 0)
                                            {
                                                if (verqstate == "1")
                                                {
                                                    //if (Rentserver_list_state_temp[i].Verstate == 1)
                                                    //{
                                                    //    string backstr = ParamErr("此卡已经领取服务");
                                                    //    return GetBackStr(backstr, poslogid);
                                                    //}
                                                    //if (Rentserver_list_state_temp[i].Verstate == 2)
                                                    //{//当领取时，必须都未使用，而当归还时跳过已归还的业务
                                                    //    string backstr = ParamErr("此卡已经归还此服务");
                                                    //    return GetBackStr(backstr, poslogid);
                                                    //}

                                                    //查询服务 是否需要归还
                                                    var Rentserver_temp_data = new RentserverData().Rentserverby_user_Info_id(Rentserver_list_state_temp[i].id);
                                                    if (Rentserver_temp_data != null)
                                                    {
                                                        //服务中 任何一个 含打印索道票就要打印索道票
                                                        if (Rentserver_temp_data.printticket == 1)
                                                        {
                                                            Printticket_temp = 1;
                                                        }

                                                        //如果需要归还 则 处理为 1
                                                        if (Rentserver_temp_data.WR == 1)
                                                        {
                                                            Rentserver_list_state_temp[i].Verstate = 1;
                                                            //num 归还的时候-1
                                                            Rentserver_list_state_temp[i].Vertime = DateTime.Now;
                                                            Rentserver_list_state_temp[i].Rettime = DateTime.Now;
                                                            Rentserver_list_state_temp[i].Remarks = beizhu;
                                                        }
                                                        else
                                                        {
                                                            //不需要归还 直接处理成成功
                                                            Rentserver_list_state_temp[i].Verstate = 2;
                                                            Rentserver_list_state_temp[i].num = Rentserver_list_state_temp[i].num - 1;//数量-1
                                                            Rentserver_list_state_temp[i].Vertime = DateTime.Now;
                                                            Rentserver_list_state_temp[i].Rettime = DateTime.Now;
                                                            Rentserver_list_state_temp[i].Remarks = beizhu;
                                                        }
                                                        //方便测试 不更改值，一直可以
                                                        if (entserver_User.cardchipid.ToString() != "54E96781500104E0")
                                                        {
                                                            var rRentserver = new RentserverData().inb2b_Rentserver_User_info(Rentserver_list_state_temp[i]);
                                                        }

                                                    }
                                                    //ExcelSqlHelper.ExecuteNonQuery("insert wanlong_yzservice (qrcode,usestate) values('" + qrcode + "','" + verqstate + "')");
                                                    use_explain = "领取成功";
                                                }
                                                else if (verqstate == "2")
                                                {

                                                    if (Rentserver_list_state_temp[i].Verstate == 0)
                                                    {//领取时一起领取，所以不能存在未领取的服务
                                                        //string backstr = ParamErr("此卡尚未领取此服务");
                                                        //return GetBackStr(backstr, poslogid);
                                                    }
                                                    if (Rentserver_list_state_temp[i].Verstate == 2)
                                                    {
                                                        //    string backstr = ParamErr("此卡已经归还此服务");
                                                        //    return GetBackStr(backstr, poslogid);
                                                    }
                                                    else
                                                    {//只对未归还的进行归还，而不需要归还的 直接诶跳过

                                                        Rentserver_list_state_temp[i].Verstate = 2;
                                                        Rentserver_list_state_temp[i].num = Rentserver_list_state_temp[i].num - 1;//数量-1
                                                        Rentserver_list_state_temp[i].Rettime = DateTime.Now;
                                                        Rentserver_list_state_temp[i].Remarks = beizhu;

                                                        var rRentserver = new RentserverData().inb2b_Rentserver_User_info(Rentserver_list_state_temp[i]);


                                                    }
                                                    use_explain = "归还成功";
                                                }
                                                else
                                                {
                                                    string backstr = ParamErr("此卡还使用使用错误");
                                                    return GetBackStr(backstr, poslogid);
                                                }
                                            }
                                            else
                                            {
                                                if (usestate == 2)
                                                {
                                                    string backstr = ParamErr("卡已经此服务已完成");
                                                    return GetBackStr(backstr, poslogid);
                                                }
                                                else
                                                {
                                                    string backstr = ParamErr("操作方式错误，请重新操作或联系管理员" + verqstate);
                                                    return GetBackStr(backstr, poslogid);
                                                }
                                            }
                                            #endregion
                                        }

                                        if (verqstate == "1")
                                        {
                                            rentservername = "租用:" + rentservername;
                                        }

                                        if (verqstate == "2")
                                        {

                                            rentservername = "归还:" + rentservername;
                                        }


                                    }

                                    ////得到商户当天的 随机码
                                    string comdayrandomstr = new B2bCompanyData().GetComDayRandomstr(comid, pos_id);
                                    //打印索道票
                                    string Printticket = "0";//默认情况下是不打印索道票
                                    string Printticket_day = DateTime.Now.ToString("yyyy-MM-dd");//索道票日期
                                    string Printticket_startime = DateTime.Now.ToString("HH:mm");//索道票开始时间
                                    string Printticket_endtime = "17:00";//索道票结束时间
                                    string Printticket_Security = comdayrandomstr;//索道票防伪码
                                    string Printticket_pno = entserver_User.eticketid.ToString();//索道票 相关id 电子票




                                    string printno = new RentserverData().GetPrintNoByChipid(entserver_User.cardchipid.ToString());




                                    #region 打印索道票
                                    if (Printticket_temp == 100000)
                                    {

                                        Printticket = "1";
                                        //查询订单
                                        int proid_temp = 0;
                                        var orderdata = new B2bOrderData().GetOrderById(entserver_User.oid);
                                        if (orderdata != null)
                                        {
                                            proid_temp = orderdata.Pro_id;
                                        }
                                        //查询产品
                                        var pro_temp = new B2bComProData().GetProById(proid_temp.ToString());
                                        if (pro_temp != null)
                                        {

                                            if (pro_temp.worktimehour != 0)
                                            {//必须有时长设置才继续执行
                                                if (pro_temp.worktimeid != 0)
                                                {
                                                    var worktimedata = new RentserverData().pro_worktimebyid(pro_temp.worktimeid, pro_temp.Com_id);
                                                    if (worktimedata != null)
                                                    {
                                                        //加入 查询特殊时间设置 日期，工作时间默认表

                                                        //修改如果有设定特定日期，读取特定日期
                                                        b2b_com_pro_worktime_calendar r = new RentserverData().GetblackoutdateByWorktimeId(DateTime.Now.ToString("yyyy-MM-dd"), worktimedata.id);
                                                        if (r != null)
                                                        {
                                                            worktimedata.defaultstartime = r.startime;
                                                            worktimedata.defaultendtime = r.endtime;
                                                        }

                                                        //上班时间
                                                        Printticket_startime = new RentserverData().worktimestarbijiao(worktimedata);
                                                        worktimedata.defaultstartime = Printticket_startime;//重新获取开始时间
                                                        //下班时间
                                                        Printticket_endtime = new RentserverData().worktimeendbijiao(pro_temp.worktimehour, worktimedata);
                                                    }
                                                }
                                                else
                                                {
                                                    //上班时间大于结束时间则都按一个时间
                                                    Printticket_startime = new RentserverData().worktimestar_endbijiao(Printticket_startime, Printticket_endtime);
                                                }
                                            }

                                        }
                                    }
                                    #endregion




                                    string backstr4 = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                                        "<business_trans version=\"1.0\">" +
                                                                            "<request_type>VerqService</request_type>" +
                                                                            "<status>{0}</status>" +// //Success ,Error
                                                                            "<Returnsinfo>{1}</Returnsinfo>" +////返回错误，当错误时返回错误原因
                                                                            "<randomid>{2}</randomid>" +
                                                                            "<security_md5>{3}</security_md5>" +
                                                                            "<Server_name>{4}</Server_name>" +
                                                                            "<companyname>{5}</companyname>" +
                                                                            "<pos_id>{6}</pos_id>" +
                                                                            "<checktime>{7}</checktime>" +
                                                                            "<use_explain>{8}</use_explain>" +
                                                                            "<cardprintid>{9}</cardprintid>" +


                                                                            "<Printticket>{10}</Printticket>" +//是否打印索道票
                                                                            "<Printticket_day>{11}</Printticket_day>" +//索道票使用日期
                                                                            "<Printticket_startime>{12}</Printticket_startime>" +//索道票开始时间
                                                                            "<Printticket_endtime>{13}</Printticket_endtime>" +//索道票结束时间
                                                                            "<Printticket_Security>{14}</Printticket_Security>" +//防伪码（3位）
                                                                            "<Printticket_pno>{15}</Printticket_pno>" +//相关电子码
                                                                            "<Printticket_operator>qiantai</Printticket_operator>" +//操作员
                                                                        "</business_trans>", "Success", "验证服务成功", returnrandomid, returnmd5, rentservername, companyname, pos_id, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), use_explain, printno, Printticket, Printticket_day, Printticket_startime, Printticket_endtime, Printticket_Security, Printticket_pno);
                                    return GetBackStr(backstr4, poslogid);
                                }
                                else
                                {//退押金
                                    string use_explain = "";
                                    var entserver_User = new RentserverData().SearchRentserver_User(0, qrcode);
                                    if (entserver_User == null)
                                    {
                                        string backstr1 = ParamErr("未查询到此卡用户");
                                        return GetBackStr(backstr1, poslogid);
                                    }



                                    if (entserver_User.serverstate == 2)
                                    {
                                        string backstr1 = ParamErr(rentservername + "此卡服务已结束，并已退押金");
                                        return GetBackStr(backstr1, poslogid);
                                    }


                                    string printno = new RentserverData().GetPrintNoByChipid(entserver_User.cardchipid.ToString());

                                    //再次查询是否包含此服务
                                    int count_temp = 0;
                                    int overstate = 0;//服务是否都已完成
                                    string overerr = "";//未完成内容
                                    var Rentserver_User_info = new RentserverData().Rentserver_User_infopagelist(1, 50, entserver_User.id, out count_temp);
                                    if (Rentserver_User_info.Count == 0)
                                    {
                                        string backstr1 = ParamErr(rentservername + "此卡服务已结束");
                                        return GetBackStr(backstr1, poslogid);
                                    }
                                    for (int i = 0; i < Rentserver_User_info.Count; i++)
                                    {

                                        if (Rentserver_User_info[i].Verstate == 1)
                                        {
                                            overstate = 1;

                                            var rentserverdata_t = new RentserverData().Rentserverbyuserinfoid(Rentserver_User_info[i].Rentserverid, comid);
                                            if (rentserverdata_t != null)
                                            {
                                                overerr += rentserverdata_t.servername + "未归还! ";
                                            }

                                        }
                                    }

                                    if (overstate == 1)
                                    {
                                        string backstr1 = ParamErr(overerr);
                                        return GetBackStr(backstr1, poslogid);
                                    }


                                    //修改状态，并清除芯片卡号
                                    entserver_User.serverstate = 2;//修改服务状态已完成
                                    //判断 结束日期是否为今天，如果是今天 ，卡清空，如果不是 则不清空
                                    if (entserver_User.endtime.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                                    {
                                        entserver_User.cardchipid = "0";//清除芯片卡
                                    }

                                    var inb2b_Rentserver_User_id_temp = new RentserverData().inb2b_Rentserver_User(entserver_User);//修改状体

                                    var tuiyajin = OrderJsonData.BackServerDeposit(entserver_User.eticketid, entserver_User.comid);//退服务押金操作


                                    //超时计算

                                    int Timeoutmoney = 0;//超时金额
                                    int TimeoutMinute = 0;//超时时间

                                    var rentserveruserinfo_temp = new RentserverData().SearchRentserver_User_outtime(entserver_User.id);
                                    if (rentserveruserinfo_temp != null)
                                    {
                                        if (rentserveruserinfo_temp.Verstate == 2)
                                        {//如果未领取 就不考虑 超时问题

                                            var order_temp = new B2bOrderData().GetOrderById(entserver_User.oid);
                                            if (order_temp != null)
                                            {
                                                var pro_temp = new B2bComProData().GetProById(order_temp.Pro_id.ToString());
                                                if (pro_temp != null)
                                                {
                                                    if (pro_temp.worktimehour != 0)
                                                    {

                                                        var Vertime = rentserveruserinfo_temp.Vertime;
                                                        var Rettime = rentserveruserinfo_temp.Rettime;

                                                        var ts = (Rettime - Vertime);
                                                        int SUB = Convert.ToInt32(ts.TotalMinutes);

                                                        int timecha = SUB - (pro_temp.worktimehour * 60);//按分钟进行计算

                                                        if (timecha > 0)
                                                        {
                                                            Timeoutmoney = timecha;
                                                            TimeoutMinute = timecha;


                                                            //插入超时金额表
                                                            b2b_Rentserver_user_Timeoutmoney timeout = new b2b_Rentserver_user_Timeoutmoney();
                                                            timeout.comid = order_temp.Comid;
                                                            timeout.oid = order_temp.Id;
                                                            timeout.proid = order_temp.Pro_id;
                                                            timeout.subdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                                                            timeout.subtime = DateTime.Now;
                                                            timeout.TimeoutMinute = TimeoutMinute;
                                                            timeout.Timeoutmoney = Timeoutmoney;
                                                            timeout.userid = entserver_User.id;

                                                            var insttimeout = new RentserverData().insertTimeoutmoney(timeout);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }


                                    string backstr4 = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                                        "<business_trans version=\"1.0\">" +
                                                                            "<request_type>VerqService</request_type>" +
                                                                            "<status>{0}</status>" +// //Success ,Error
                                                                            "<Returnsinfo>{1}</Returnsinfo>" +////返回错误，当错误时返回错误原因
                                                                            "<randomid>{2}</randomid>" +
                                                                            "<security_md5>{3}</security_md5>" +
                                                                            "<Server_name>{4}</Server_name>" +
                                                                            "<companyname>{5}</companyname>" +
                                                                            "<pos_id>{6}</pos_id>" +
                                                                            "<checktime>{7}</checktime>" +
                                                                            "<use_explain>{8}</use_explain>" +
                                                                            "<cardprintid>{9}</cardprintid>" +
                                                                            "<Printticket>0</Printticket>" +//因为是退押金流程所以赋值给默认值是否打印索道票
                                                                            "<Printticket_day></Printticket_day>" +//索道票使用日期
                                                                            "<Printticket_startime></Printticket_startime>" +//索道票开始时间
                                                                            "<Printticket_endtime></Printticket_endtime>" +//索道票结束时间
                                                                            "<Printticket_Security></Printticket_Security>" +//防伪码（3位）
                                                                            "<Printticket_pno></Printticket_pno>" +//相关电子码
                                                                            "<Printticket_operator></Printticket_operator>" +//操作员
                                                                        "</business_trans>", "Success", "退押金操作成功", returnrandomid, returnmd5, rentservername, companyname, pos_id, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), use_explain, printno);
                                    return GetBackStr(backstr4, poslogid);

                                }
                            }
                            catch (Exception ex)
                            {
                                string backstr2 = ParamErr("出现意外错误");
                                return GetBackStr(backstr2, poslogid);
                            }
                        }
                    }
                    #endregion
                    #region 补卡查询接口
                    else if (request_type == "CardFillSearch")
                    {
                        lock (lockobj)
                        {
                            try
                            {
                                string qrcode = xn.SelectSingleNode("qrcode").InnerXml;//用户卡号ID，用之前输入电子票的参数即可
                                if (qrcode.Trim() == "")
                                {
                                    string backstr1 = ParamErr("卡号和服务状态不可为空");
                                    return GetBackStr(backstr1, poslogid);
                                }

                                //首先查询 服务内容 //pos_id
                                string rentservername = "补卡查询";

                                //查询电子码 所发出去的卡
                                int count = 0;
                                var entserver_User = new RentserverData().SearchRentserver_Userbypno(qrcode, out count);
                                if (entserver_User.Count == 0)
                                {
                                    string backstr1 = ParamErr("未查询到此码发送的卡");
                                    return GetBackStr(backstr1, poslogid);
                                }

                                var cardlist = "";
                                for (int i = 0; i < entserver_User.Count; i++)
                                {
                                    //通过芯片卡号获得印刷卡号
                                    string printno = new RentserverData().GetPrintNoByChipid(entserver_User[i].cardchipid.ToString());

                                    cardlist += "<cardinfo><identity>" + entserver_User[i].id + "</identity><cardprintid>" + printno + "</cardprintid></cardinfo>";

                                }

                                string backstr4 = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                                    "<business_trans version=\"1.0\">" +
                                                                        "<request_type>VerqService</request_type>" +
                                                                        "<status>{0}</status>" +// //Success ,Error
                                                                        "<Returnsinfo>{1}</Returnsinfo>" +////返回错误，当错误时返回错误原因
                                                                        "<randomid>{2}</randomid>" +
                                                                        "<security_md5>{3}</security_md5>" +
                                                                        "<pos_id>{4}</pos_id>" +
                                                                        "<qrcode>{5}</qrcode>" +
                                                                        "<cardinfolist>{6}</cardinfolist>" +
                                                                    "</business_trans>", "Success", "补卡查询", returnrandomid, returnmd5, pos_id, qrcode, cardlist);
                                return GetBackStr(backstr4, poslogid);

                            }
                            catch (Exception ex)
                            {
                                string backstr2 = ParamErr("出现意外错误");
                                return GetBackStr(backstr2, poslogid);
                            }
                        }
                    }
                    #endregion
                    #region 补卡确认接口
                    else if (request_type == "CardFillApply")
                    {
                        lock (lockobj)
                        {
                            try
                            {
                                string qrcode = xn.SelectSingleNode("qrcode").InnerXml;//用户卡号ID，用之前输入电子票的参数即可
                                string identity = xn.SelectSingleNode("identity").InnerXml;//id
                                string cardprintid = xn.SelectSingleNode("cardprintid").InnerXml;//卡号
                                if (qrcode.Trim() == "" || identity.Trim() == "")
                                {
                                    string backstr1 = ParamErr("卡号和补卡序号 不可为空");
                                    return GetBackStr(backstr1, poslogid);
                                }

                                //查询用户押金是否正常
                                var entserver_User = new RentserverData().SearchRentserver_User(int.Parse(identity), "");
                                if (entserver_User == null)
                                {
                                    string backstr1 = ParamErr("未查询到此卡用户");
                                    return GetBackStr(backstr1, poslogid);
                                }

                                #region 更新到新卡号

                                var cardid_temp = entserver_User.cardid;
                                var cardid_new = "";
                                var firstnum = 0;//第一个字符
                                if (cardid_temp != "")
                                {
                                    firstnum = int.Parse(cardid_temp.Substring(0, 1));
                                }

                                firstnum = firstnum + 1;//每次补卡 进行加1，默认第一位 为0
                                if (firstnum >= 10)
                                {
                                    firstnum = 9;
                                }
                                cardid_new = firstnum.ToString() + cardid_temp.Substring(1, cardid_temp.Length - 1);//重新组合卡号，自动原卡号作废了,第一位加1，后面的按原来读取
                                entserver_User.cardid = cardid_new;//赋值新的卡号
                                var uprentserver = new RentserverData().inb2b_Rentserver_User(entserver_User);

                                #endregion

                                string backstr4 = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                                    "<business_trans version=\"1.0\">" +
                                                                        "<request_type>VerqService</request_type>" +
                                                                        "<status>{0}</status>" +// //Success ,Error
                                                                        "<Returnsinfo>{1}</Returnsinfo>" +////返回错误，当错误时返回错误原因
                                                                        "<randomid>{2}</randomid>" +
                                                                        "<security_md5>{3}</security_md5>" +
                                                                        "<qrcode>{4}</qrcode>" +//输入的 电子码
                                                                        "<cardidlist>" +
                                                                        "<cardid>{5}</cardid>" +//需要写入扇区的新卡号
                                                                        "</cardidlist>" +
                                                                    "</business_trans>", "Success", "补卡申请成功", returnrandomid, returnmd5, qrcode, cardid_new);
                                return GetBackStr(backstr4, poslogid);

                            }
                            catch (Exception ex)
                            {
                                string backstr2 = ParamErr("出现意外错误");
                                return GetBackStr(backstr2, poslogid);
                            }
                        }
                    }
                    #endregion
                    #region 实体卡芯片id上传接口
                    else if (request_type == "CardRelation")
                    {
                        lock (lockobj)
                        {
                            try
                            {
                                string qrcode = xn.SelectSingleNode("qrcode").InnerXml;//用户卡号ID，用之前输入电子票的参数即可
                                string num = xn.SelectSingleNode("num").InnerXml;



                                XmlNodeList NodeList_item = xdoc.SelectNodes("/business_trans/cardidrelationlist/cardidrelation");
                                if (NodeList_item.Count != 0)
                                {
                                    foreach (XmlNode Node in NodeList_item)
                                    {
                                        XmlNodeList taobaoke_item = Node.ChildNodes;//taobaoke_item集合,第1个taobaoke_item下的所有节点(num_iid、price、title)组成集合
                                        string cardid = taobaoke_item.Item(0).InnerText;//32位生成号
                                        string cardchipid = taobaoke_item.Item(1).InnerText;//卡的芯片id
                                        string cardchipid_temp = cardchipid.Trim();
                                        if(cardchipid_temp=="" || cardchipid_temp==null){
                                            cardchipid_temp="0";
                                        }

                                        if (cardid != "")
                                        {
                                            //查询32位卡号，最后一位如果是，则去掉
                                            if (!string.IsNullOrEmpty(cardid))
                                            {
                                                if (cardid.Substring(cardid.Length - 1, 1) == ",")
                                                {
                                                    cardid = cardid.Substring(0, cardid.Length - 1);
                                                }
                                            }

                                            //芯片id，最后一位如果是，则去掉
                                            if (!string.IsNullOrEmpty(cardchipid))
                                            {
                                                if (cardchipid.Substring(cardchipid.Length - 1, 1) == ",")
                                                {
                                                    cardchipid = cardchipid.Substring(0, cardchipid.Length - 1);
                                                }
                                            }

                                            if (cardid.Contains(","))
                                            {
                                               string[] cardid_arr = cardid.Split(',') ;
                                               string[] cardchipid_arr = cardchipid.Split(',');
                                               if (cardid_arr.Length == cardchipid_arr.Length)
                                               {
                                                   for (int i = 0; i < cardid_arr.Length; i++) {
                                                       //先清除卡id使用的记录
                                                       var clearchipid = new RentserverData().clearRentserver_User_kaid(cardid_arr[i], cardchipid_arr[i]);
                                                       if (clearchipid == 1)
                                                       {
                                                           //循环发卡，统一更新芯片卡号到数据库
                                                           var rentserverdata_t = new RentserverData().upRentserver_User_kaid(cardid_arr[i], cardchipid_arr[i]);
                                                       }
                                                   }
                                               }
                                               else {

                                                   string backstr2 = ParamErr("发卡片数量与上传芯片号与数量不匹配");
                                                   return GetBackStr(backstr2, poslogid);
                                               }
                                            }
                                            else
                                            {
                                                //先清除卡id使用的记录
                                                var clearchipid = new RentserverData().clearRentserver_User_kaid(cardid, cardchipid_temp);
                                                if (clearchipid != -1)//只为判断运行过就成
                                                {
                                                    //发一张卡，更新芯片卡号到数据库
                                                    var rentserverdata_t = new RentserverData().upRentserver_User_kaid(cardid, cardchipid_temp);
                                                }
                                            }

                                        }
                                    }

                                }


                                string backstr4 = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                                    "<business_trans version=\"1.0\">" +
                                                                        "<request_type>VerqService</request_type>" +
                                                                        "<status>{0}</status>" +// //Success ,Error
                                                                        "<Returnsinfo>{1}</Returnsinfo>" +////返回错误，当错误时返回错误原因
                                                                        "<randomid>{2}</randomid>" +
                                                                        "<security_md5>{3}</security_md5>" +
                                                                        "<qrcode>{4}</qrcode>" +
                                                                    "</business_trans>", "Success", "上传成功", returnrandomid, returnmd5, qrcode);
                                return GetBackStr(backstr4, poslogid);

                            }
                            catch (Exception ex)
                            {
                                string backstr2 = ParamErr("出现意外错误");
                                return GetBackStr(backstr2, poslogid);
                            }
                        }
                    }
                    #endregion

                    #region 芯片与印刷id匹配
                    else if (request_type == "Upprintidcardchipid")
                    {
                        lock (lockobj)
                        {
                            try
                            {
                                string qrcode = xn.SelectSingleNode("qrcode").InnerXml;//用户卡号ID，用之前输入电子票的参数即可
                                string num = xn.SelectSingleNode("num").InnerXml;
                                var prodata = new B2bCompanyInfoData();
                                var pos_temp = prodata.PosInfobyposid(int.Parse(pos_id));
                                if (pos_temp == null)
                                {
                                    string backstr2 = ParamErr("POS错误");
                                    return GetBackStr(backstr2, poslogid);
                                }
                                int comid = pos_temp.Com_id;

                                string returndata = ProductJsonData.Relationserver_printid_chipid(comid, qrcode, int.Parse(num));
                                JsonCommonEntity entity = (JsonCommonEntity)JsonConvert.DeserializeObject(returndata, typeof(JsonCommonEntity));
                                int type = entity.Type;
                                string msg = entity.Msg;

                                if (type == 1)
                                {
                                    string backstr2 = ParamErr("上传错误，" + msg);
                                    return GetBackStr(backstr2, poslogid);
                                }


                                string backstr4 = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                                    "<business_trans version=\"1.0\">" +
                                                                        "<request_type>Upprintidcardchipid</request_type>" +
                                                                        "<status>{0}</status>" +// //Success ,Error
                                                                        "<Returnsinfo>{1}</Returnsinfo>" +////返回错误，当错误时返回错误原因
                                                                        "<randomid>{2}</randomid>" +
                                                                        "<security_md5>{3}</security_md5>" +
                                                                        "<qrcode>{4}</qrcode>" +
                                                                    "</business_trans>", "Success", "上传成功", returnrandomid, returnmd5, qrcode);
                                return GetBackStr(backstr4, poslogid);

                            }
                            catch (Exception ex)
                            {
                                string backstr2 = ParamErr("出现意外错误");
                                return GetBackStr(backstr2, poslogid);
                            }
                        }
                    }
                    #endregion

                    #region pos版本更新
                    else if (request_type == "CheckSoftVersion")//pos版本更新
                    {
                        var versioncode = xn.SelectSingleNode("Versioncode").InnerXml;
                        //根据posid得到pos机最新版本
                        Posversionmodifylog poslasterversion = new PosversionmodifylogData().GetLatestVersion(pos_id);
                        //判断pos机版本是否需要更新
                        if (poslasterversion != null)
                        {
                            if (poslasterversion.VersionNo.ToString() == versioncode)
                            {
                                string backstr = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                    "<business_trans version=\"1.0\">" +
                                                        "<request_type>CheckSoftVersion</request_type>" +//类型
                                                        "<status>1</status>" +//状态
                                                        "<Returnsinfo>此版本已经是最新版本</Returnsinfo>" +
                                                        "<randomid>" + returnrandomid + "</randomid>" +
                                                        "<pos_id>" + pos_id + "</pos_id>" +//POS编号
                                                        "<security_md5>" + returnmd5 + "</security_md5>" +//MD5加密
                                                        "<Versioncode>" + versioncode + "</Versioncode>" +
                                                    "</business_trans>";
                                return GetBackStr(backstr, poslogid);
                            }
                            else
                            {
                                //string backstr = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                //                    "<business_transversion=\"1.0\">" +
                                //                        "<request_type>CheckSoftVersion</request_type>" +//类型
                                //                        "<status>" + poslasterversion.Updatetype + "</status>" +//状态(1，不需更新；2，exe更新；3，xml更新，4，exe和xml同时更新)
                                //                        "<Returnsinfo>" + poslasterversion.Updatefileurl + "</Returnsinfo>" +//更新文件地址
                                //                        "<randomid>" + returnrandomid + "</randomid>" +
                                //                        "<pos_id>" + pos_id + "</pos_id>" +//POS编号
                                //                        "<security_md5>" + returnmd5 + "</security_md5>" +//MD5加密
                                //                        "<Versioncode>" + poslasterversion.VersionNo + "</Versioncode>" +
                                //                    "</business_trans>";

                                //版本下载功能更改：status:有更新返回0，否则返回其他值；Returnsinfo: 返回现在地址列表，其格式是"文件名=下载地址;"每个下载地址用“;”号隔开，最后一个下载地址也要加上“;”。
                                //1.3版本升1.4开始
                                string backstr = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                  "<business_trans version=\"1.0\">" +
                                                      "<request_type>CheckSoftVersion</request_type>" +//类型
                                                      "<status>" + poslasterversion.Updatetype + "</status>" +//状态(有更新返回0，否则返回其他值)
                                                      "<Returnsinfo>" + poslasterversion.Updatefileurl + "</Returnsinfo>" +//更新文件地址
                                                      "<randomid>" + returnrandomid + "</randomid>" +
                                                      "<pos_id>" + pos_id + "</pos_id>" +//POS编号
                                                      "<security_md5>" + returnmd5 + "</security_md5>" +//MD5加密
                                                      "<Versioncode>" + poslasterversion.VersionNo + "</Versioncode>" +
                                                  "</business_trans>";

                                Posversionrenewlog renewlog = new Posversionrenewlog
                                {
                                    Id = 0,
                                    Posid = int.Parse(pos_id),
                                    Initversionno = decimal.Parse(versioncode),
                                    Newversionno = poslasterversion.VersionNo,
                                    Updatetype = poslasterversion.Updatetype,
                                    Updatefileurl = poslasterversion.Updatefileurl,
                                    Updatetime = poslasterversion.Updatetime
                                };
                                new PosversionrenewlogData().InsertOrUpdate(renewlog);
                                return GetBackStr(backstr, poslogid);
                            }
                        }
                        else
                        {
                            string backstr = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                    "<business_trans version=\"1.0\">" +
                                                        "<request_type>CheckSoftVersion</request_type>" +//类型
                                                        "<status>1</status>" +//状态
                                                        "<Returnsinfo>当前版本已经是最新版本</Returnsinfo>" +
                                                        "<randomid>" + returnrandomid + "</randomid>" +
                                                        "<pos_id>" + pos_id + "</pos_id>" +//POS编号
                                                        "<security_md5>" + returnmd5 + "</security_md5>" +//MD5加密
                                                        "<Versioncode>" + versioncode + "</Versioncode>" +
                                                    "</business_trans>";
                            return GetBackStr(backstr, poslogid);
                        }
                    }
                    #endregion

                    #region 闸机刷卡
                    else if (request_type == "Paycard")//刷卡
                    {

                        //闸机id
                        //卡类型
                        //开始时间
                        //剩余有效期时间
                        //首次刷卡时间
                        //剩余次数
                        //验证结果
                        //卡片状态

                        lock (lockobj)
                        {
                            string CardID = xn.SelectSingleNode("CardID").InnerXml;//卡号
                            string appID = xn.SelectSingleNode("appID").InnerXml;//商户编号、浩瀚给的
                            string cmd = xn.SelectSingleNode("cmd").InnerXml;//说明
                            pos_id = xn.SelectSingleNode("pos_id").InnerXml;//闸机id
                            DateTime nowdate=DateTime.Now;//现在时间


                            var Rentserver_User_data = new RentserverData().SearchRentserver_UserbyCardID(CardID);//通过卡号获取发卡用户信息

                            if (Rentserver_User_data == null) {
                                string backstr = ParamErr("无效卡，未找到此卡记录");
                                return GetBackStr(backstr, poslogid);
                            }

                            if (Rentserver_User_data.endtime < nowdate) {
                                string backstr = ParamErr("过期卡,已过使用时间");
                                return GetBackStr(backstr, poslogid);
                            }
                            if (Rentserver_User_data.subtime > nowdate)
                            {
                                string backstr = ParamErr("无效卡,卡有效时间还未开始");
                                return GetBackStr(backstr, poslogid);
                            }
                            if (Rentserver_User_data.sendnum ==0)
                            {
                                string backstr = ParamErr("卡过期,卡的使用次数用完");
                                //return GetBackStr(backstr, poslogid);
                            }
                            

                            //查询 闸机和产品是否可以通过

                            var orderdata=new B2bOrderData().GetOrderById(Rentserver_User_data.oid);
                            if (orderdata ==null)
                            {
                                string backstr = ParamErr("未查找到订单");
                                return GetBackStr(backstr, poslogid);
                            }
                            

                            int bangdingzhajicount = 0;
                            var prodata = new B2bComProData().GetProbandingzhajilistByproidposid(Rentserver_User_data.comid, orderdata.Pro_id, pos_id, out bangdingzhajicount);
                            if (prodata.Count == 0)
                            {
                                string backstr = ParamErr("此卡不能在此闸机上消费");
                                return GetBackStr(backstr, poslogid);
                            }
                                                   


                            string backstr1 = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                       "<business_trans version=\"1.0\">" +
                                                           "<request_type>Paycard</request_type>" +//类型
                                                           "<cardName>" + Rentserver_User_data.pname + "</cardName>" +//状态
                                                           "<startTime>"+Rentserver_User_data.subtime+"</startTime>" +
                                                           "<endDate>"+Rentserver_User_data.endtime+"</endDate>" +
                                                           "<useNum>"+Rentserver_User_data.sendnum+"</useNum>" +
                                                           "<cardState>1</cardState>" +
                                                           "<theState>1</theState>" +
                                                           "<randomid>" + returnrandomid + "</randomid>" +
                                                           "<security_md5>" + returnmd5 + "</security_md5>" +//MD5加密
                                                       "</business_trans>";

                            //插入闸机日志


                            Rentserver_User_zhajilog zhajilogmodel = new Rentserver_User_zhajilog();
                            zhajilogmodel.oid=Rentserver_User_data.oid;
                            zhajilogmodel.pos_id = pos_id;
                            zhajilogmodel.clearchipid = CardID;
                            zhajilogmodel.proid=0;
                            zhajilogmodel.Rentserver_Userid=Rentserver_User_data.id;
                            zhajilogmodel.comid=Rentserver_User_data.comid;
                            zhajilogmodel.subtime=DateTime.Now;

                            var zhajilog = new B2bComProData().Rentserver_User_zhajilogInsertOrUpdate(zhajilogmodel);//插入日志

                            var Rentserver_Userup = new RentserverData().jianb2b_Rentserver_User(Rentserver_User_data.id);


                            return GetBackStr(backstr1, poslogid);
                            

                        }


                       
                    }
                    #endregion

                    #region 自助取卡查询电子票
                    else if (request_type == "SelfSearchrcode")//查询电子票
                    {
                        lock (lockobj)
                        {
                            string qrcode = xn.SelectSingleNode("qrcode").InnerXml;

                            if (qrcode != null && qrcode != "")
                            {
                                qrcode = EncryptionHelper.EticketPnoDES(qrcode, 1);//对码进行解密,对于大于20字符进行揭秘，码13位，身份证18位
                            }

                            //如果为空
                            if (qrcode == "")
                            {
                                string backstr = ParamErr("未查询信息错误，请重新操作！");
                                return GetBackStr(backstr, poslogid);
                            }
                             

                            //查询电子票信息
                            B2b_eticket eticketinfo = new B2bEticketData().GetEticketDetail(qrcode, pos_id);
                            if (eticketinfo != null)
                            {



                                //--------------------增加对POS指定项目验证 peter写 2014-11-22----------------------------
                                //产品如果指定了验证pos，则必须用指定pos验证
                                string proBindPosid = new B2bComProData().GetProBindPosid(eticketinfo.Pro_id);
                                if (proBindPosid != "")
                                {
                                    if (proBindPosid != pos_id)
                                    {
                                        string errbackstr = ParamErr("和产品绑定Pos不符");
                                        return GetBackStr(errbackstr, poslogid);
                                    }
                                }
                                else
                                {
                                    //查询Pos_id是否指定项目，如果指定 则和码的产品项目是否匹配。
                                    var projectid_temp = 0;//是否制定项目
                                    var projectid = 0;//实际项目
                                    var prodata = new B2bCompanyInfoData();
                                    var pos_temp = prodata.PosInfobyposid(int.Parse(pos_id));
                                    if (pos_temp != null)
                                    {
                                        projectid_temp = pos_temp.Projectid;
                                    }

                                    if (projectid_temp != 0)
                                    {//当POS指定了项目ID进行匹配
                                        var projectData = new B2b_com_projectData();
                                        projectid = projectData.GetProjectidByproid(eticketinfo.Pro_id);

                                        if (projectid_temp != projectid)
                                        {
                                            var backstr_temp = ParamErr("非匹配商户");
                                            return GetBackStr(backstr_temp, poslogid);
                                        }
                                    }
                                }
                                //查询产品-商品编号（万龙对接，2017-12-19）
                                string merchant_code = "";
                                var promodel = new B2bComProData().GetProById(eticketinfo.Pro_id.ToString());
                                if (promodel != null) {
                                    merchant_code = promodel.merchant_code;
                                }



                                
                                //--------------------增加对POS指定项目验证 peter写 2014-11-22--------------------------
                                B2bOrderData orderdata = new B2bOrderData();
                                B2b_order ordermodel = orderdata.GetOrderById(eticketinfo.Oid);
                                //判断订单是否为多规格订单
                                string pro_faceprice = eticketinfo.E_face_price.ToString("f2");
                                if (ordermodel != null)
                                {
                                    //规格判断
                                    if (ordermodel.Speciid > 0)
                                    {
                                        B2b_com_pro_Speci mProSpeci = B2b_com_pro_SpeciData.Getgginfobyggid(ordermodel.Speciid);
                                        if (mProSpeci != null)
                                        {
                                            pro_faceprice = mProSpeci.speci_face_price.ToString("f2");
                                        }
                                    }

                                    //判断订单是否需要，交押金（万龙对接，2017-12-20）
                                    if (promodel != null)
                                    {
                                        //原来判断是否发卡，现在更改为 判断是否需要支付押金，
                                        if (promodel.Wrentserver != 0) { 
                                            
                                            //如果需要支付押金的，检查是否支付押金。而如果遇到不需要支付押金的，如果需要头盔等窗口办理，自助机不进行办理
                                            if (eticketinfo.ishasdeposit == 0)//未支付押金
                                            {
                                                var backstr_temp = ParamErr("此产品需要支付押金，未支付押金，请关注官方微信号在线交押金。");
                                                return GetBackStr(backstr_temp, poslogid);
                                            }

                                        }
                                    }
                                   

                                }

                                ////得到商户当天的 随机码
                                string comdayrandomstr = new B2bCompanyData().GetComDayRandomstr(eticketinfo.Com_id, pos_id);

                                string backstr = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                    "<business_trans version=\"1.0\">" +
                                                         "<request_type>SelfSearchrcode</request_type>" +//类型
                                                          "<status>{0}</status>" +//状态
                                                          "<Returnsinfo>{1}</Returnsinfo>" +//返回错误，当错误时返回错误原因
                                                          "<randomid>{2}</randomid>" +//随机编号
                                                          "<qrcode>{3}</qrcode>" +//电子码
                                                          "<security_md5>{4}</security_md5>" +//MD5加密
                                                          "<pro_name>{5}</pro_name>" +//产品名称
                                                          "<merchant_code>{9}</merchant_code>" +//产品名称
                                                          "<face_price>{6}</face_price>" +//门市价
                                                          "<num>{7}</num>" +//可用数量
                                                           "<use_explain>{8}</use_explain>" +//使用说明
                                                     "</business_trans>", "Success", "电子票查询成功", returnrandomid, qrcode, returnmd5, eticketinfo.E_proname, pro_faceprice, eticketinfo.Use_pnum.ToString(), "验证后当天使用(" + comdayrandomstr + ")", merchant_code);
                                if (eticketinfo.Use_pnum == 0)
                                {
                                    backstr = ParamErr("此电子票已验证或不存在");
                                    return GetBackStr(backstr, poslogid);
                                }
                                else
                                {
                                    return GetBackStr(backstr, poslogid);
                                }


                            }
                            else
                            {
                                string backstr = ParamErr("此电子票不存在或不是此商家产品");
                                return GetBackStr(backstr, poslogid);
                            }

                        }
                    }
                    #endregion


                    #region 自助取卡验证电子票
                    else if (request_type == "SelfVerqrcode")//验证电子票
                    {
                        lock (lockobj)
                        {
                            string qrcode = xn.SelectSingleNode("qrcode").InnerXml;
                            if (qrcode != null && qrcode != "")
                            {
                                qrcode = EncryptionHelper.EticketPnoDES(qrcode, 1);//对码进行解密
                            }


                            string num = xn.SelectSingleNode("num").InnerXml;


                            if (num.ConvertTo<int>(0) == 0)//判断输入是否为数字，以及是否大于0
                            {
                                string backstrr = ParamErr("输入必须为数字且大于0");
                                return GetBackStr(backstrr, poslogid);
                            }
                            else
                            {

                                //判断是否用同一个随机码验证电子票
                                //bool randomwhethersame = new B2bEticketLogData().GetRandomWhetherSame( (int)ECodeOper.ValidateECode, randomid);
                                B2b_eticket_log logg = new B2bEticketLogData().GetElogByRandomid(randomid, (int)ECodeOper.ValidateECode);

                                if (logg != null)
                                {
                                    string backstrr = ParamErr("验证电子票的随机码两次不可相同");
                                    return GetBackStr(backstrr, poslogid);
                                }
                                else
                                {


                                    //验证电子票信息
                                    B2b_eticket eticketinfo = new B2bEticketData().GetEticketDetail(qrcode, pos_id);
                                    if (eticketinfo != null)
                                    {

                                        //--------------------增加对POS指定项目验证 peter写 2014-11-22----------------------------
                                        //查询Pos_id是否指定项目，如果指定 则和码的产品项目是否匹配。
                                        var projectid_temp = 0;//是否制定项目
                                        var projectid = 0;//实际项目
                                        var prodata = new B2bCompanyInfoData();
                                        var pos_temp = prodata.PosInfobyposid(int.Parse(pos_id));
                                        if (pos_temp != null)
                                        {
                                            projectid_temp = pos_temp.Projectid;
                                        }

                                        if (projectid_temp != 0)
                                        {//当POS指定了项目ID进行匹配
                                            var projectData = new B2b_com_projectData();
                                            projectid = projectData.GetProjectidByproid(eticketinfo.Pro_id);

                                            if (projectid_temp != projectid)
                                            {
                                                var backstr_temp = ParamErr("非匹配商户");
                                                return GetBackStr(backstr_temp, poslogid);
                                            }
                                        }


                                        //--------------------增加对POS指定项目验证 peter写 2014-11-22--------------------------

                                        B2b_com_pro modelcompro = new B2bComProData().GetProById(eticketinfo.Pro_id.ToString());
                                        if (modelcompro == null)
                                        {
                                            string backstr = ParamErr("查询产品出错");//理论上不会出现此问题，只是做意外的屏蔽
                                            return GetBackStr(backstr, poslogid);
                                        }

                                        //--------------------如果需要交押金判断是否交了押金（万龙对接） 2017-12-20--------------------------
                                        if (modelcompro.Wrentserver != 0) {

                                                //如果需要支付押金的，检查是否支付押金。而如果遇到不需要支付押金的，如果需要头盔等窗口办理，自助机不进行办理
                                                if (eticketinfo.ishasdeposit == 0)//未支付押金
                                                {
                                                    var backstr_temp = ParamErr("此产品需要支付押金，尚未支付押金，请关注官方微信号在线交押金。");
                                                    return GetBackStr(backstr_temp, poslogid);
                                                }
                                        }



                                        //验证电子票
                                        string returndata = EticketJsonData.EConfirm(qrcode, num, eticketinfo.Com_id.ToString(), int.Parse(pos_id), randomid);

                                        JsonCommonEntity entity = (JsonCommonEntity)JsonConvert.DeserializeObject(returndata, typeof(JsonCommonEntity));
                                        int type = entity.Type;
                                        string msg = entity.Msg;

                                        //根据电子票信息得到商家信息
                                        B2b_company companysummary = B2bCompanyData.GetCompany(eticketinfo.Com_id);

                                        //根据电子票信息得到出票单位（分销商）信息--（现阶段分销商未做，先显示商家信息）
                                        string agentcompany = "";//分销商名称,出票单位

                                        if (eticketinfo.Agent_id == 0)
                                        {
                                            agentcompany = companysummary.Com_name;//如果无分销商则视为 商家自己出票
                                        }
                                        else
                                        {
                                            //查询分销商获取分销商公司名称
                                            var agentinfo = AgentCompanyData.GetAgentByid(eticketinfo.Agent_id);
                                            if (agentinfo != null)
                                            {
                                                agentcompany = agentinfo.Company;
                                            }
                                        }

                                        //判断分销商是否为空
                                        if (agentcompany == "")
                                        {
                                            agentcompany = companysummary.Com_name;//分销商信息为空则归为商家出票
                                        }

                                        #region 产品有效期，验证时间(返回datetime.now)，使用说明(都返回"验证当天有效")，终端id(即posid)
                                        string use_explain = "验证后当天使用";//使用说明
                                        string validtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");



                                        string merchant_code = modelcompro.merchant_code;
                                        string provalidatemethod = modelcompro.ProValidateMethod;
                                        int appointdate = modelcompro.Appointdata;
                                        int iscanuseonsameday = modelcompro.Iscanuseonsameday;

                                        DateTime pro_end = modelcompro.Pro_end;
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

                                        B2bOrderData orderdata = new B2bOrderData();
                                        B2b_order ordermodel = orderdata.GetOrderById(eticketinfo.Oid);
                                        //如果是旅游大巴的话则 有效期为出行日期当日
                                        if (modelcompro.Server_type == 10)
                                        {
                                            pro_end = ordermodel.U_traveldate;
                                        }
                                        #endregion

                                        if (type == 100)
                                        {
                                            string ishasdeposit = "0";

                                            ////得到商户当天的 随机码
                                            string comdayrandomstr = new B2bCompanyData().GetComDayRandomstr(eticketinfo.Com_id, pos_id);

                                            //身份信息字符串
                                            string cardidstr = "";

                                            //打印索道票
                                            string Printticket_day = DateTime.Now.ToString("yyyy-MM-dd");
                                            string Printticket_startime = DateTime.Now.ToString("HH:mm");
                                            string Printticket_endtime = "17:00";
                                            string Printticket_Security = comdayrandomstr;
                                            string Printticket_pno = qrcode;


                                            //判断订单是否为多规格订单
                                            string pro_faceprice = eticketinfo.E_face_price.ToString("f2");
                                            if (ordermodel != null)
                                            {
                                                if (ordermodel.Speciid > 0)
                                                {
                                                    B2b_com_pro_Speci mProSpeci = B2b_com_pro_SpeciData.Getgginfobyggid(ordermodel.Speciid);
                                                    if (mProSpeci != null)
                                                    {
                                                        pro_faceprice = mProSpeci.speci_face_price.ToString("f2");
                                                    }
                                                }
                                            }

                                            string backstr = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                       "<business_trans version=\"1.0\">" +
                                                           "<request_type>SelfVerqrcode</request_type>" +//类型
                                                           "<status>{0}</status>" +//状态
                                                           "<Returnsinfo>{1}</Returnsinfo>" +//返回错误，当错误时返回错误原因
                                                           "<randomid>{2}</randomid>" +//随机编号+
                                                           "<qrcode>{3}</qrcode>" +//电子码
                                                           "<security_md5>{4}</security_md5>" +//MD5加密
                                                           "<pro_name>{5}</pro_name>" +//产品名称
                                                           "<face_price>{6}</face_price>" +//门市价
                                                           "<num>{7}</num>" +//验证数量
                                                           "<companyname>{8}</companyname>" +//商家名称
                                                           "<agentname>{9}</agentname>" +//出票单位
                                                           "<pos_id>{10}</pos_id>" +//终端id
                                                           "<pro_valid>{11}</pro_valid>" +//产品有效期
                                                           "<checktime>{12}</checktime>" +//验证时间
                                                            "<use_explain>{13}</use_explain>" +//使用说明
                                                            "<cardidlist>{14}</cardidlist>" +//身份信息
                                                            "<ishasdeposit>{15}</ishasdeposit>" +//是否有押金
                                                            "<Printticket_day>{16}</Printticket_day>" +//索道票使用日期
                                                            "<Printticket_startime>{17}</Printticket_startime>" +//索道票开始时间
                                                            "<Printticket_endtime>{18}</Printticket_endtime>" +//索道票结束时间
                                                            "<Printticket_Security>{19}</Printticket_Security>" +//防伪码（3位）
                                                            "<Printticket_pno>e{20}</Printticket_pno>" +//相关电子码
                                                            "<Printticket_operator>zizhu</Printticket_operator>" +//操作员'
                                                            "<merchant_code>{21}</merchant_code>" +//操作员'
                                                       "</business_trans>", "Success", "电子票验证成功", returnrandomid, qrcode, returnmd5,
                                                       eticketinfo.E_proname, pro_faceprice, num,
                                                       companysummary.Com_name, agentcompany, pos_id, pro_end.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), use_explain + "(" + comdayrandomstr + ")", cardidstr, ishasdeposit, Printticket_day, Printticket_startime, Printticket_endtime, Printticket_Security, eticketinfo.Id,merchant_code);


                                            ////作为测试 验证延时 用的电子码
                                            //string vqrcodestr = "9119639550605,9119671843056,9119637396204";
                                            //if (vqrcodestr.IndexOf(qrcode) != -1)
                                            //{
                                            //    System.Threading.Thread.Sleep(20000);
                                            //}
                                            return GetBackStr(backstr, poslogid);

                                        }
                                        else
                                        {
                                            string backstr = ParamErr(msg);
                                            return GetBackStr(backstr, poslogid);
                                        }


                                    }
                                    else
                                    {
                                        string backstr = ParamErr("此电子票不存在或不是此商家产品");
                                        return GetBackStr(backstr, poslogid);
                                    }

                                }
                            }
                        }
                    }
                    #endregion

                    #region 自助退押金确认接口
                    else if (request_type == "SelfVerqService")
                    {
                        lock (lockobj)
                        {
                            try
                            {
                                string qrcode = xn.SelectSingleNode("qrcode").InnerXml;//电子码
                                int eticketid_tuiyajin = 0;
                                int comid_tuiyajin = 0;

                                if (qrcode == "")
                                {
                                    string backstr = ParamErr("电子票查询出错");
                                    return GetBackStr(backstr, poslogid);
                                }

                                B2b_eticket eticketinfo = new B2bEticketData().GetEticketDetail(qrcode, pos_id);
                                if (eticketinfo == null)
                                {
                                    string backstr = ParamErr("查询电子票出错");
                                    return GetBackStr(backstr, poslogid);

                                }
                                var promodel = new B2bComProData().GetProById(eticketinfo.Pro_id.ToString());
                                if (promodel == null)
                                {
                                    string backstr = ParamErr("查询产品出错");
                                    return GetBackStr(backstr, poslogid);
                                }

                                //原来判断是否发卡，现在更改为 判断是否需要支付押金，
                                if (promodel.Wrentserver != 0)
                                {
                                    //如果需要支付押金的，检查是否支付押金。而如果遇到不需要支付押金的，如果需要头盔等窗口办理，自助机不进行办理
                                    if (eticketinfo.ishasdeposit == 0)//未支付押金
                                    {
                                        var backstr_temp = ParamErr("此订单未支付押金！");
                                        return GetBackStr(backstr_temp, poslogid);
                                    }

                                }
                                else {
                                    var backstr_temp = ParamErr("此订单产品不需要支付押金！");
                                    return GetBackStr(backstr_temp, poslogid);
                                }

                                eticketid_tuiyajin = eticketinfo.Id;
                                comid_tuiyajin = eticketinfo.Com_id;


                                var tuiyajin = OrderJsonData.BackServerDeposit(eticketid_tuiyajin, comid_tuiyajin);//退服务押金操作

                                string backstr4 = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                                                        "<business_trans version=\"1.0\">" +
                                                                            "<request_type>SelfVerqService</request_type>" +
                                                                            "<status>{0}</status>" +// //Success ,Error
                                                                            "<Returnsinfo>{1}</Returnsinfo>" +////返回错误，当错误时返回错误原因
                                                                            "<randomid>{2}</randomid>" +
                                                                            "<security_md5>{3}</security_md5>" +
                                                                            "<pos_id>{4}</pos_id>" +
                                                                        "</business_trans>", "Success", "退押金操作成功", returnrandomid, returnmd5, pos_id);
                                return GetBackStr(backstr4, poslogid);
                            }
                            catch (Exception ex)
                            {
                                string backstr2 = ParamErr("出现意外错误");
                                return GetBackStr(backstr2, poslogid);
                            }
                        }
                    }
                    #endregion

                    #region 返回错误"功能正在开发中"
                    else//返回错误"功能正在开发中"
                    {
                        string backstr = ParamErr("功能正在开发中");
                        return GetBackStr(backstr, poslogid);
                    }
                    #endregion

                    



                }
                else
                {
                    string backstr = ParamErr("传递参数被篡改");
                    return GetBackStr(backstr, poslogid);
                }
            }
            catch (Exception ex)
            {
                //string backstr = ParamErr("传递参数有误");
                string backstr = ParamErr(ex.Message);
                return GetBackStr(backstr, poslogid);
            }

        }

        public static string GetBackStr(string backstr, int poslogid)
        {
            //得到pos日志
            Pos_log modelposlog = new PoslogData().GetPosLogById(poslogid);
            if (modelposlog == null)
            {
                backstr = ParamErr("pos日志获取错误，请联系管理员");
            }

            modelposlog.ReturnStr = backstr;
            modelposlog.ReturnSubdate = DateTime.Now;
            new PoslogData().InsertOrUpdate(modelposlog);
            return backstr;
        }


        /// <summary>
        /// 传参错误返回通用错误信息
        /// </summary>
        /// <returns></returns>
        public static string ParamErr(string errmsg)
        {
            var returnrandomid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            return string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                   "<business_trans version=\"1.0\">" +
                                       "<status>{0}</status>" +//状态
                                       "<Returnsinfo>{1}</Returnsinfo>" +//返回错误，当错误时返回错误原因
                                       "<randomid>{2}</randomid>" +//随机编号
                                   "</business_trans>", "Error", errmsg, returnrandomid);
        }




        public class JsonCommonEntity
        {
            private int type;
            private string msg;

            public int Type
            {
                set { this.type = value; }
                get { return this.type; }
            }
            public string Msg
            {
                set { this.msg = value; }
                get { return this.msg; }
            }
        }

    }

}
