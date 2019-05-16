using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.Member.Service.MemberService.Data;
using System.Collections;
using Newtonsoft.Json;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.Framework;
using FileUpload.FileUpload.Entities;
using FileUpload.FileUpload.Data;

namespace ETS.JsonFactory
{
    public class ChannelJsonData
    {

        public static string GetUnitList(int unittype)
        {
            try
            {

                var list = new MemberChannelcompanyData().GetUnitList(unittype);

                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        public static string GetUnitList(int comid, int unittype,int userid=0,int actid=0)
        {
            try
            {
                if (userid != 0)
                {
                      MemberChannelcompanyData channeldate= new  MemberChannelcompanyData();
                       B2b_company_manageuser user = B2bCompanyManagerUserData.GetUser(userid);
                       if (user != null)
                       {
                           if (user.Channelcompanyid == 0)
                           {
                               var list = new MemberChannelcompanyData().GetUnitList(comid, unittype);

                                 IEnumerable result = "";
                                 if (list != null)
                                     result = from pro in list
                                              select new
                                              {
                                                  Id = pro.Id,
                                                  Com_id = pro.Com_id,
                                                  Companyname = pro.Companyname,
                                                  Issuetype = pro.Issuetype,
                                                  Selectstate = channeldate.GetchannelUnitListselected(actid,pro.Id)

                                              };
                               return JsonConvert.SerializeObject(new { type = 100, msg = result });
                           }
                           else
                           {
                               var list = new MemberChannelcompanyData().GetUnitList(comid, unittype, int.Parse(user.Channelcompanyid.ToString()));
                               IEnumerable result = "";
                               if (list != null)
                                   result = from pro in list
                                            select new
                                            {
                                                Id = pro.Id,
                                                Com_id = pro.Com_id,
                                                Companyname = pro.Companyname,
                                                Issuetype = pro.Issuetype,
                                                Selectstate = channeldate.GetchannelUnitListselected(pro.Id, actid)

                                            };
                               return JsonConvert.SerializeObject(new { type = 100, msg = result });
                           }
                       }
                       else 
                       {
                           return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                       }
                }
                else 
                {

                    var list = new MemberChannelcompanyData().GetUnitList(comid, unittype);

                    return JsonConvert.SerializeObject(new { type = 100, msg = list });
                }

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string GetUnitList(int comid, string unittype, string ischosen, int userid)
        {
            try
            {
                List<Member_Channel_company> list = new List<Member_Channel_company>();

                //判断操作账户类型(总公司账户;门市账户)
                bool IsParentCompanyUser = new B2bCompanyManagerUserData().IsParentCompanyUser(userid);

                int channelcompanyid = 0;//门店id，用于门市账户添加员工

                if (IsParentCompanyUser)//总公司账户
                {
                    if (ischosen == "0")//总公司账户，并且没有选择渠道来源：人员所在单位只是显示 总公司名称
                    {
                        list = null;
                    }
                    else //总公司账户，并且选择了渠道来源：人员所在单位显示 总公司名称+各门店列表/各合作单位列表
                    {
                        list = new MemberChannelcompanyData().GetUnitList(comid, unittype);
                    }
                }
                else //门店账户
                {
                    //人员所在单位显示 当前门店
                    Member_Channel_company model = new MemberChannelcompanyData().GetChannelCompanyByUserId(userid);
                    channelcompanyid = model.Id;
                    list.Add(model);
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = list, channelcompanyid = channelcompanyid });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message, channelcompanyid = 0 });
                throw;
            }
        }

        public static string GetUnitListselected(int actid)
        {
            try
            {

                var list = new MemberChannelcompanyData().GetUnitListselected(actid);
                //IEnumerable result = "";
                //if (list != null)
                //{
                //    result = from order in list
                //             select new
                //             {
                //                 Id = order.Id,

                //             };
                //}

                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string GetChannelDetail(int channelid, int comid)
        {
            try
            {
                var channelobj = new MemberChannelData().GetChannelDetail(channelid, comid);
                return JsonConvert.SerializeObject(new { type = 100, msg = channelobj });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }
        public static string GetChannelDetail(int channelid)
        {
            try
            {
                var channelobj = new MemberChannelData().GetChannelDetail(channelid);
                return JsonConvert.SerializeObject(new { type = 100, msg = channelobj });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }


        public static string Upchannelcompanproject(int channelcompanyid, string companyproject)
        {
            try
            {
                var channelobj = new MemberChannelcompanyData().Upchannelcompanproject(channelcompanyid, companyproject);
                return JsonConvert.SerializeObject(new { type = 100, msg = channelobj });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }

        public static string EditChannel(Member_Channel channel)
        {
            try
            {
                var id = new MemberChannelData().EditChannel(channel);
                return JsonConvert.SerializeObject(new { type = 100, msg = id });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new
                {
                    type = 1,
                    msg = ex.Message
                });
            }
        }

        public static string PageList(string comid, int pageindex, int pagesize, string issuetype, int userid)
        {
            var totalcount = 0;
            try
            {
                var list = new List<Member_Channel>();
                B2b_company_manageuser userr = B2bCompanyManagerUserData.GetUser(userid);
                if (userr != null)
                {
                    if (userr.Channelcompanyid == 0)//总公司账户 
                    {
                        list = new MemberChannelData().PageList(comid, pageindex, pagesize, issuetype, out totalcount);
                    }
                    else //总公司下面渠道 
                    {
                        list = new MemberChannelData().PageList(comid, pageindex, pagesize, issuetype, int.Parse(userr.Channelcompanyid.ToString()), out totalcount);
                    }

                    IEnumerable result = "";
                    if (list != null)

                        result = from pro in list
                                 select new
                                 {
                                     Id = pro.Id,
                                     Com_id = pro.Com_id,
                                     Issuetype = pro.Issuetype,
                                     //CompanyName = new MemberChannelcompanyData().GetCompanyById(pro.Companyid).Companyname,
                                     CompanyName = pro.Issuetype == 3 ? "网站渠道" : (pro.Issuetype == 4 ? "微信渠道" : new MemberChannelcompanyData().GetCompanyById(pro.Companyid).Companyname),
                                     Name = pro.Name,
                                     Mobile = pro.Mobile,
                                     ChObjects = pro.ChObjects,
                                     Cardcode = pro.Cardcode,
                                     Chaddress = pro.Chaddress,
                                     RebateOpen = pro.RebateOpen,
                                     RebateConsume = pro.RebateConsume,
                                     RebateConsume2 = pro.RebateConsume2,
                                     RebateLevel = pro.RebateLevel == 0 ? "普通" : "暂时未设",
                                     //添加录入量,开卡量，第一次交易量，金额
                                     OpenCardNum = pro.Opencardnum,
                                     Firstdealnum = pro.Firstdealnum,
                                     Summoney = pro.Summoney,
                                     companyid = pro.Companyid,

                                     EnterCardNum = new MemberCardData().GetEnteredNumberByChannelId(pro.Id)
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

        //渠道查询
        public static string SeachList(string comid, int pageindex, int pagesize, string key, int select, int userid)
        {



            bool isNum = Domain_def.RegexValidate("^[0-9]*$", key);
            var totalcount = 0;
            try
            {
                var list = new List<Member_Channel>();
                B2b_company_manageuser userr = B2bCompanyManagerUserData.GetUser(userid);
                if (userr != null)
                {
                    if (userr.Channelcompanyid == 0)//总公司账户 
                    {
                        list = new MemberChannelData().SeachList(comid, pageindex, pagesize, key, select, isNum, out totalcount);
                    }
                    else //总公司下面渠道 
                    {
                        list = new MemberChannelData().SeachList(comid, pageindex, pagesize, key, select, isNum, int.Parse(userr.Channelcompanyid.ToString()), out totalcount);
                    }
                    IEnumerable result = "";
                    if (list != null)

                        result = from pro in list
                                 select new
                                 {
                                     Id = pro.Id,
                                     Com_id = pro.Com_id,
                                     Issuetype = pro.Issuetype,
                                     ////CompanyName = new MemberChannelcompanyData().GetCompanyById(pro.Companyid).Companyname,
                                     CompanyName = pro.Issuetype == 3 ? "网站渠道" : (pro.Issuetype == 4 ? "微信渠道" : new MemberChannelcompanyData().GetCompanyById(pro.Companyid) == null ? "--" : new MemberChannelcompanyData().GetCompanyById(pro.Companyid).Companyname),
                                     Name = pro.Name,
                                     Mobile = pro.Mobile,
                                     ChObjects = pro.ChObjects,
                                     Cardcode = pro.Cardcode,
                                     Chaddress = pro.Chaddress,
                                     RebateOpen = pro.RebateOpen,
                                     RebateConsume = pro.RebateConsume,
                                     RebateConsume2 = pro.RebateConsume2,
                                     RebateLevel = pro.RebateLevel == 0 ? "普通" : "暂时未设",
                                     //添加录入量,开卡量，第一次交易量，金额
                                     OpenCardNum = pro.Opencardnum,
                                     Firstdealnum = pro.Firstdealnum,
                                     Summoney = pro.Summoney,
                                     companyid = pro.Companyid,

                                     EnterCardNum = new MemberCardData().GetEnteredNumberByChannelId(pro.Id)
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
        public static string EditChannelCompany(Member_Channel_company company)
        {
            try
            {
                int id = new MemberChannelcompanyData().EditChannelCompany(company);
                if (company.Id == 0)//如果是添加渠道单位操作，则默认添加一个此单位下的渠道人
                {
                    Member_Channel channel = new Member_Channel()
                    {
                        Id = 0,
                        Com_id = company.Com_id,
                        Issuetype = company.Issuetype,
                        Companyid = id,
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
                    int idd = new MemberChannelData().EditChannel(channel);
                    if (id > 0 && idd > 0)
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = id });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "编辑渠道时出现错误" });
                    }
                }
                else
                {
                    //判断渠道公司中是否含有默认渠道，没有的话添加进去
                    int numm = new MemberChannelData().GetDefaultChannelNum(company.Id);
                    if (numm == 0)
                    {
                        Member_Channel channel = new Member_Channel()
                        {
                            Id = 0,
                            Com_id = company.Com_id,
                            Issuetype = company.Issuetype,
                            Companyid = id,
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
                            Whetherdefaultchannel = 1
                        };
                        int idd = new MemberChannelData().EditChannel(channel);
                        if (id > 0 && idd > 0)
                        {
                            return JsonConvert.SerializeObject(new { type = 100, msg = id });
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "编辑渠道时出现错误" });
                        }
                    }
                    else
                    {
                        if (id > 0)
                        {
                            return JsonConvert.SerializeObject(new { type = 100, msg = id });
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "编辑渠道时出现错误" });
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new
                {
                    type = 1,
                    msg = ex.Message
                });
            }
        }

        public static string GetChannelByCompanyid(string companyid)
        {
            try
            {
                var list = new MemberChannelData().GetChannelByCompanyid(companyid);
                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }

        public static string Upchannl(int id, int comid, decimal channlcard, decimal oldcard, int uptype)
        {
            try
            {
                var pro = "";

                //获得渠道ID
                var Channeinfo = new MemberChannelData().GetSelfChannelDetailByCardNo(channlcard.ToString());

                B2bCrmData crmdate = new B2bCrmData();
                var Userinfo = crmdate.Readuser(id, comid);


                if (Channeinfo != null && Userinfo != null)
                {
                    pro = new MemberChannelData().UpchannlT(Userinfo.Cardid, Channeinfo.Id, uptype);
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }

        //渠道统计
        public static string Channelstatistics(string comid, int pageindex, int pagesize, string issuetype)
        {
            var totalcount = 0;
            try
            {


                var prodata = new MemberChannelData();
                var list = prodata.Channelstatistics(comid, pageindex, pagesize, issuetype, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 //添加录入量,开卡量，第一次交易量，金额
                                 OpenCardNum = pro.Opencardnum,//开卡
                                 Firstdealnum = pro.Firstdealnum,//第一次录入量
                                 Summoney = pro.Summoney,//金额
                                 Channelname = pro.Channeltypename,
                                 Companyid = pro.Companyid,
                                 Companynum = pro.Companynum
                                 // companyid = pro.Companyid,//
                                 // companyname = pro.Companyid == 0 ? "微信网站注册" : new MemberChannelcompanyData().GetCompanyById(pro.Companyid).Companyname,
                                 //ID = pro.Id
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        public static string Channelstatistics2(string comid, int pageindex, int pagesize, string issuetype, string companystate, int channelcompanyid = 0)
        {
            var totalcount = 0;
            try
            {


                var prodata = new MemberChannelData();
                var list = prodata.Channelstatistics2(comid, pageindex, pagesize, issuetype, companystate, out totalcount, channelcompanyid);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 //添加录入量,开卡量，第一次交易量，金额

                                 OpenCardNum = pro.Opencardnum,//开卡
                                 Firstdealnum = pro.Firstdealnum,//第一次录入量
                                 Summoney = pro.Summoney,//金额
                                 Channelname = pro.Channeltypename,
                                 Companyid = pro.Companyid,
                                 Companynum = pro.Companynum,
                                 Companystate = pro.Companystate
                                 // companyid = pro.Companyid,//
                                 // companyname = pro.Companyid == 0 ? "微信网站注册" : new MemberChannelcompanyData().GetCompanyById(pro.Companyid).Companyname,
                                 //ID = pro.Id
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        //渠道统计ChannelYk
        public static string ChannelYk(string comid, int pageindex, int pagesize, string issuetype)
        {
            var totalcount = 0;
            try
            {


                var prodata = new MemberChannelData();
                var list = prodata.ChannelYk(comid, pageindex, pagesize, issuetype, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 Yknum = pro.Com_id,//验卡数量
                                 Companyname = pro.Channeltypename //门市名称
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string GetAllInnerChannels()
        {
            var totalcount = 0;
            try
            {

                var prodata = new MemberChannelData();
                var list = prodata.GetAllInnerChannels(out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Com_id = pro.Com_id,
                                 Issuetype = pro.Issuetype,
                                 //CompanyName = new MemberChannelcompanyData().GetCompanyById(pro.Companyid).Companyname,
                                 CompanyName = pro.Issuetype == 3 ? "网站渠道" : (pro.Issuetype == 4 ? "微信渠道" : new MemberChannelcompanyData().GetCompanyById(pro.Companyid).Companyname),
                                 Name = pro.Name,
                                 Mobile = pro.Mobile,
                                 ChObjects = pro.ChObjects,
                                 Cardcode = pro.Cardcode,
                                 Chaddress = pro.Chaddress,
                                 RebateOpen = pro.RebateOpen,
                                 RebateConsume = pro.RebateConsume,
                                 RebateConsume2 = pro.RebateConsume2,
                                 RebateLevel = pro.RebateLevel == 0 ? "普通" : "暂时未设",
                                 //添加录入量,开卡量，第一次交易量，金额
                                 OpenCardNum = pro.Opencardnum,
                                 Firstdealnum = pro.Firstdealnum,
                                 Summoney = pro.Summoney,
                                 companyid = pro.Companyid,

                                 EnterCardNum = new MemberCardData().GetEnteredNumberByChannelId(pro.Id)
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }



        public static string GetChannelCompanyList(int comid, string channeltype, int pageindex, int pagesize)
        {
            var totalcount = 0;
            try
            {
                var prodata = new MemberChannelcompanyData();
                var list = prodata.GetChannelCompanyList(comid, channeltype, pageindex, pagesize, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Companyname = pro.Companyname,
                                 Issuetype = pro.Issuetype == 0 ? "内部门市" : pro.Issuetype == 1 ? "合作公司" : pro.Issuetype.ToString()

                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string GetChannelCompany(string channelcompanyid)
        {
            try
            {
                Member_Channel_company company = new MemberChannelcompanyData().GetChannelCompany(channelcompanyid);
                List<Member_Channel_company> list = new List<Member_Channel_company>();
                list.Add(company);
                IEnumerable result = "";
                if (list != null)
                {
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Com_id = pro.Com_id,
                                 Companyname = pro.Companyname,
                                 Issuetype = pro.Issuetype,
                                 Whethercreateqrcode = pro.Whethercreateqrcode,
                                 Companyaddress = pro.Companyaddress,
                                 Companyphone = pro.Companyphone,
                                 CompanyCoordinate = pro.CompanyCoordinate,
                                 CompanyimgID = pro.Companyimg,
                                 CompanyImgUrl = FileSerivce.GetImgUrl(pro.Companyimg),
                                 Companyintro = pro.Companyintro,
                                 Companyproject = pro.Companyproject,
                                 CompanyState = pro.Companystate,
                                 WhetherDepartment = pro.Whetherdepartment,
                                 Bookurl = pro.Bookurl,
                                 CompanyLocate = pro.CompanyLocate,
                                 Province=pro.Province,
                                 City=pro.City,
                                 Outshop=pro.Outshop
                             };

                    return JsonConvert.SerializeObject(new { type = 100, msg = result });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = "" });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }



        public static string SearchChannelByChannelUnit(string comid, int pageindex, int pagesize, int channelcompanyid)
        {
            var totalcount = 0;
            try
            {
                var list = new List<Member_Channel>();


                list = new MemberChannelData().SearchChannelByChannelUnit(comid, pageindex, pagesize, channelcompanyid, out totalcount);

                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Com_id = pro.Com_id,
                                 Issuetype = pro.Issuetype,
                                 ////CompanyName = new MemberChannelcompanyData().GetCompanyById(pro.Companyid).Companyname,
                                 CompanyName = pro.Issuetype == 3 ? "网站渠道" : (pro.Issuetype == 4 ? "微信渠道" : new MemberChannelcompanyData().GetCompanyById(pro.Companyid) == null ? "--" : new MemberChannelcompanyData().GetCompanyById(pro.Companyid).Companyname),
                                 Name = pro.Name,
                                 Mobile = pro.Mobile,
                                 ChObjects = pro.ChObjects,
                                 Cardcode = pro.Cardcode,
                                 Chaddress = pro.Chaddress,
                                 RebateOpen = pro.RebateOpen,
                                 RebateConsume = pro.RebateConsume,
                                 RebateConsume2 = pro.RebateConsume2,
                                 RebateLevel = pro.RebateLevel == 0 ? "普通" : "暂时未设",
                                 //添加录入量,开卡量，第一次交易量，金额
                                 OpenCardNum = pro.Opencardnum,
                                 Firstdealnum = pro.Firstdealnum,
                                 Summoney = pro.Summoney,
                                 companyid = pro.Companyid,
                                 Runstate = pro.Runstate == 1 ? "运行" : "暂停",
                                 EnterCardNum = new MemberCardData().GetEnteredNumberByChannelId(pro.Id)
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });



            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string Channellistbycomid(string comid, int pageindex, int pagesize)
        {
            var totalcount = 0;
            try
            {
                var list = new List<Member_Channel>();


                list = new MemberChannelData().Channellistbycomid(comid, pageindex, pagesize, out totalcount);

                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Com_id = pro.Com_id,
                                 Issuetype = pro.Issuetype,
                                 ////CompanyName = new MemberChannelcompanyData().GetCompanyById(pro.Companyid).Companyname,
                                 CompanyName = pro.Issuetype == 3 ? "网站渠道" : (pro.Issuetype == 4 ? "微信渠道" : new MemberChannelcompanyData().GetCompanyById(pro.Companyid) == null ? "--" : new MemberChannelcompanyData().GetCompanyById(pro.Companyid).Companyname),
                                 Name = pro.Name,
                                 Mobile = pro.Mobile,
                                 ChObjects = pro.ChObjects,
                                 Cardcode = pro.Cardcode,
                                 Chaddress = pro.Chaddress,
                                 RebateOpen = pro.RebateOpen,
                                 RebateConsume = pro.RebateConsume,
                                 RebateConsume2 = pro.RebateConsume2,
                                 RebateLevel = pro.RebateLevel == 0 ? "普通" : "暂时未设",
                                 //添加录入量,开卡量，第一次交易量，金额
                                 OpenCardNum = pro.Opencardnum,
                                 Firstdealnum = pro.Firstdealnum,
                                 Summoney = pro.Summoney,
                                 companyid = pro.Companyid,

                                 EnterCardNum = new MemberCardData().GetEnteredNumberByChannelId(pro.Id)
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });



            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


        public static string Channelcompanypagelist(string comid, int pageindex, int pagesize, string key, int channelcompanyid = 0, string channelcompanytype = "0,1,3,4",string openid="")
        {
            try
            {
                int totalcount = 0;
                var crmdata=new B2bCrmData();

                List<Member_Channel_company> list = new MemberChannelcompanyData().Channelcompanypagelist(comid, pageindex, pagesize, key, out totalcount, channelcompanyid, channelcompanytype);

                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             //orderby pro.Distance
                             select new
                             {
                                 Id = pro.Id,
                                 Com_id = pro.Com_id,
                                 Companyname = pro.Companyname,
                                 Issuetype = pro.Issuetype,
                                 Whethercreateqrcode = pro.Whethercreateqrcode,
                                 Companyaddress = pro.Companyaddress,
                                 Companyphone = pro.Companyphone,
                                 CompanyCoordinate = pro.CompanyCoordinate,
                                 CompanyLocate  =pro.CompanyLocate,
                                 Companyimgurl = FileSerivce.GetImgUrl(pro.Companyimg),
                                 Companyintro = pro.Companyintro,
                                 Companyproject = pro.Companyproject,
                                 Distance = crmdata.CalculateTheCoordinates(openid,pro.Id)
                             }


                             ;
                return JsonConvert.SerializeObject(new { type = 100, totalcount = totalcount, msg = result });


            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }



        public static string ChannelcompanyOrderlocation(string comid, int pageindex, int pagesize, string key, int channelcompanyid = 0, string channelcompanytype = "0,1,3,4", string openid = "",string n1="",string e1="")
        {
            try
            {
                int totalcount = 0;
                var crmdata = new B2bCrmData();

                List<Member_Channel_company> list = new MemberChannelcompanyData().ChannelcompanyOrderlocation(comid, pageindex, pagesize, key, out totalcount, channelcompanyid, channelcompanytype, n1, e1);

                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             //orderby pro.Distance
                             select new
                             {
                                 Id = pro.Id,
                                 Com_id = pro.Com_id,
                                 Companyname = pro.Companyname,
                                 Issuetype = pro.Issuetype,
                                 Whethercreateqrcode = pro.Whethercreateqrcode,
                                 Companyaddress = pro.Companyaddress,
                                 Companyphone = pro.Companyphone,
                                 CompanyCoordinate = pro.CompanyCoordinate,
                                 CompanyLocate = pro.CompanyLocate,
                                 Companyimgurl = FileSerivce.GetImgUrl(pro.Companyimg),
                                 Companyintro = pro.Companyintro,
                                 Companyproject = pro.Companyproject,
                                 Distance = crmdata.CalculateTheCoordinates(openid, pro.Id),
                                 m=new WxSubscribeSourceData().GetWXSourceByChannelcompanyid(pro.Id)

                             }


                             ;
                return JsonConvert.SerializeObject(new { type = 100, totalcount = totalcount, msg = result });


            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string Adjustchannelcompanystatus(int companyid, int status)
        {
            if (companyid == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "渠道公司编号传递为0" });
            }
            else
            {
                try
                {
                    int result = new MemberChannelcompanyData().Adjustchannelcompanystatus(companyid, status);

                    //调整门市下员工状态(在职，离职)
                    int employstate = 0;
                    if (status == 1)//调整渠道公司“开通”
                    {
                        employstate = 1;
                    }
                    else //调整渠道公司"停业"
                    {
                        employstate = 0;
                    }
                    int result2 = new B2bCompanyManagerUserData().AdjustChannelCompanyEmploerstate(companyid, employstate);
                    if (result > 0)
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = "操作成功" });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "操作有误" });
                    }
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                    throw;
                }
            }
        }

        public static string Getchannelcompanyname(int channelcompanyid, int comid = 0)
        {
            if (comid == 0)
            {
                if (channelcompanyid == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
                else
                {
                    Member_Channel_company company = new MemberChannelcompanyData().GetChannelCompany(channelcompanyid.ToString());
                    if (company == null)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = company.Companyname });
                    }

                }
            }
            else
            {
                if (channelcompanyid == 0)
                {
                    B2b_company company = B2bCompanyData.GetCompany(comid);
                    if (company == null)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                    }
                    else 
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = company.Com_name  });
                    }
                   
                }
                else
                {
                    Member_Channel_company company = new MemberChannelcompanyData().GetChannelCompany(channelcompanyid.ToString());
                    if (company == null)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = company.Companyname });
                    }
                   
                }
            }
        }
    }
}
