using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Meituan.Model;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.PM.Service.Meituan.Data.Internal
{
    public class InternalDealResponse
    {
        public SqlHelper sqlHelper;
        public InternalDealResponse(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }


        internal List<DealResponseBody> GetDealResponseBody(out int totalcount, Agent_company Agentinfo, string method, List<string> productIdList, int currentPage = 0, int pageSize = 0)
        {
            #region 多点拉取
            if (method.Trim() == "multi")
            {
                string proidstr = "";
                foreach (string proid in productIdList)
                {
                    proidstr = proid + ",";
                }
                proidstr = proidstr.Substring(0, proidstr.Length - 1);

                string sql = "select * from b2b_com_pro  where  id in (select proid from  b2b_com_pro_groupbuystocklog where proid in (" + proidstr + ") and isstock=1 and stockagentcompanyid="+Agentinfo.Id+") and  pro_end>='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";


                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                List<DealResponseBody> list = new List<DealResponseBody>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new DealResponseBody
                        {
                            useDateMode = reader.GetValue<int>("isneedbespeak"),//不需要预约则为0；需要预约则预约当天可用，为1
                            visitorInfoType = reader.GetValue<int>("issetidcard") == 0 ? 1 : 3,//不需要身份证号则为1；需要身份证号则为3
                            visitorInfoGroupSize = 0,//每多少个人共用一个游客信息(不会出现这种情况，设为0)
                            visitorInfoRule = new UserInfoRule
                            {
                                name = reader.GetValue<int>("issetidcard") == 0 ? false : true,
                                pinyin = reader.GetValue<int>("issetidcard") == 0 ? false : true,
                                mobile =  true,
                                address = false,
                                email = false,
                                hkmlp = false,
                                tlp = false,
                                mtp = false,
                                passport = false,
                                credentials = reader.GetValue<int>("issetidcard") == 0 ? false : true
                            },
                            autoRefundTime = 0,
                            refundType = reader.GetValue<int>("QuitTicketMechanism") == 0 ? 4 : reader.GetValue<int>("QuitTicketMechanism") == 1 ? 4 : 3,
                            partlyRefund = false,
                            refundNote = "",
                            refundStairsRules = GetRefundStairsRules(reader.GetValue<int>("isneedbespeak"), reader.GetValue<int>("QuitTicketMechanism")),
                            scheduleOnlineTime = reader.GetValue<DateTime>("pro_start").ToString("yyyy-MM-dd"),
                            scheduleOfflineTime = reader.GetValue<DateTime>("pro_end").AddDays(1).ToString("yyyy-MM-dd"),
                            servicePhones =new List<ServicePhone>(),//?? 服务电话
                            getInAddresses = new string[]{""},//?? 入园地址
                            needTicket = false,
                            getTicketRule = new GetTicketRule(),
                            ticketGetAddress = new string[] {""},
                            orderCancelTime = 30,
                            otherNote = reader.GetValue<string>("Precautions"),
                            returnVoucher = true,
                            voucherText = "",
                            getVoucherRule = new GetVoucherRule
                            {
                                effectiveCertificate = false,
                                voucherLoaders = new int[] { 2, 4 },
                                needCertificateSupplement = false,
                                certificateSupplement = ""
                            },
                            stock = reader.GetValue<int>("ispanicbuy") == 0 ? -1 : reader.GetValue<int>("limitbuytotalnum"),
                            marketPrice = reader.GetValue<decimal>("face_price"),
                            mtPrice = reader.GetValue<decimal>("advise_price"),
                            settlementPrice = 0,//结算价??
                            voucherTimeBegin = 0,
                            voucherTimeEnd = 0,
                            voucherTimes = GetVoucherTimes(),
                            aheadHoursType = reader.GetValue<int>("iscanuseonsameday"),
                            aheadMinutes = reader.GetValue<int>("iscanuseonsameday") == 1 ? 0 : reader.GetValue<int>("iscanuseonsameday") == 2 ? 2 * 60 : 24 * 60 + (24 * 60 - 23 * 60 - 59),
                            aheadTimeRules = new List<AheadTimeRule>(),
                            idLimitDays = 0,
                            idLimitCount = 0,
                            phoneLimitDays = 0,
                            phoneLimitCount = 0,
                            maxBuyCount = reader.GetValue<int>("pro_number"),
                            minBuyCount = 1,
                            title = reader.GetValue<string>("pro_name"),
                            subTitle = "",
                            include = reader.GetValue<string>("service_Contain"),
                            exclude = reader.GetValue<string>("service_NotContain"),
                            partnerId = int.Parse(Agentinfo.mt_partnerId),
                            partnerDealId = reader.GetValue<int>("id").ToString(),
                            partnerPoiId = reader.GetValue<int>("projectid").ToString(),
                            dealImageInfos = GetDealImageInfos(reader.GetValue<int>("imgurl")),//??产品图片
                            voucherDateBegin = reader.GetValue<DateTime>("pro_start").ToString("yyyy-MM-dd"),
                            voucherDateEnd = reader.GetValue<DateTime>("pro_end").AddDays(1).ToString("yyyy-MM-dd"),
                            validWeekRule = GetValidWeekRule(reader.GetValue<int>("isblackoutdate"), reader.GetValue<int>("etickettype")),
                            unavailableDates = new string[] {""}
                        });
                    }

                }
                totalcount = list.Count;
                return list;
            }
            #endregion
            #region  批量拉取 page
            else if (method.Trim() == "page")
            {
                var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
                var condition = "id in (select proid from  b2b_com_pro_groupbuystocklog where  isstock=1  and stockagentcompanyid=" + Agentinfo.Id + ") and  pro_end>='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                cmd.PagingCommand1("b2b_com_pro", "*", "id", "", pageSize, currentPage, "", condition);

                List<DealResponseBody> list = new List<DealResponseBody>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new DealResponseBody
                        {
                            useDateMode = reader.GetValue<int>("isneedbespeak"),//不需要预约则为0；需要预约则预约当天可用，为1
                            visitorInfoType = reader.GetValue<int>("issetidcard") == 0 ? 1 : 3,//不需要身份证号则为1；需要身份证号则为3
                            visitorInfoGroupSize = 0,//每多少个人共用一个游客信息(不会出现这种情况，设为0)
                            visitorInfoRule = new UserInfoRule
                            {
                                name = reader.GetValue<int>("issetidcard") == 0 ? false : true,
                                pinyin = reader.GetValue<int>("issetidcard") == 0 ? false : true,
                                mobile = reader.GetValue<int>("issetidcard") == 0 ? false : true,
                                address = false,
                                email = false,
                                hkmlp = false,
                                tlp = false,
                                mtp = false,
                                passport = false,
                                credentials = reader.GetValue<int>("issetidcard") == 0 ? false : true
                            },
                            autoRefundTime = 0,
                            refundType = reader.GetValue<int>("QuitTicketMechanism") == 0 ? 4 : reader.GetValue<int>("QuitTicketMechanism") == 1 ? 4 : 3,
                            partlyRefund = false,
                            refundNote = "",
                            refundStairsRules = GetRefundStairsRules(reader.GetValue<int>("isneedbespeak"), reader.GetValue<int>("QuitTicketMechanism")),
                            scheduleOnlineTime = reader.GetValue<DateTime>("pro_start").ToString("yyyy-MM-dd"),
                            scheduleOfflineTime = reader.GetValue<DateTime>("pro_end").AddDays(1).ToString("yyyy-MM-dd"),
                            servicePhones =new List<ServicePhone>(),//?? 服务电话
                            getInAddresses = new string[]{""},//?? 入园地址
                            needTicket = false,
                            getTicketRule = new GetTicketRule(),
                            ticketGetAddress = new string[] { "" },
                            orderCancelTime = 30,
                            otherNote = reader.GetValue<string>("Precautions"),
                            returnVoucher = true,
                            voucherText = "",
                            getVoucherRule = new GetVoucherRule
                            {
                                effectiveCertificate = false,
                                voucherLoaders = new int[] { 2, 4 },
                                needCertificateSupplement = false,
                                certificateSupplement = ""
                            },
                            stock = reader.GetValue<int>("ispanicbuy") == 0 ? -1 : reader.GetValue<int>("limitbuytotalnum"),
                            marketPrice = reader.GetValue<decimal>("face_price"),
                            mtPrice = reader.GetValue<decimal>("advise_price"),
                            settlementPrice = 0,//结算价??
                            voucherTimeBegin = 0,
                            voucherTimeEnd = 0,
                            voucherTimes = GetVoucherTimes(),
                            aheadHoursType = reader.GetValue<int>("iscanuseonsameday"),
                            aheadMinutes = reader.GetValue<int>("iscanuseonsameday") == 1 ? 0 : reader.GetValue<int>("iscanuseonsameday") == 2 ? 2 * 60 : 24 * 60 + (24 * 60 - 23 * 60 - 59),
                            aheadTimeRules = new List<AheadTimeRule>(),
                            idLimitDays = 0,
                            idLimitCount = 0,
                            phoneLimitDays = 0,
                            phoneLimitCount = 0,
                            maxBuyCount = reader.GetValue<int>("pro_number"),
                            minBuyCount = 1,
                            title = reader.GetValue<string>("pro_name"),
                            subTitle = "",
                            include = reader.GetValue<string>("service_Contain"),
                            exclude = reader.GetValue<string>("service_NotContain"),
                            partnerId = int.Parse(Agentinfo.mt_partnerId),
                            partnerDealId = reader.GetValue<int>("id").ToString(),
                            partnerPoiId = reader.GetValue<int>("projectid").ToString(),
                            dealImageInfos = GetDealImageInfos(reader.GetValue<int>("imgurl")),//??产品图片
                            voucherDateBegin = reader.GetValue<DateTime>("pro_start").ToString("yyyy-MM-dd"),
                            voucherDateEnd = reader.GetValue<DateTime>("pro_end").AddDays(1).ToString("yyyy-MM-dd"),
                            validWeekRule = GetValidWeekRule(reader.GetValue<int>("isblackoutdate"), reader.GetValue<int>("etickettype")),
                            unavailableDates = new string[] { "" }
                        });
                    }

                }
                totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
                return list;

            }
            #endregion
            else
            {
                totalcount = 0;
                return new List<DealResponseBody>();
            }
        }

        private ValidWeekRule GetValidWeekRule(int isblackoutdate, int etickettype)
        {
            if (isblackoutdate == 0)
            {
                return new ValidWeekRule
                {
                    monday = true,
                    tuesday = true,
                    wednesday = true,
                    thursday = true,
                    friday = true,
                    saturday = true,
                    sunday = true
                };
            }
            //限定使用日期：只是处理了平日票和周末票，假日票限制没有办法处理
            else
            {
                //平日票
                if (etickettype == 0)
                {
                    return new ValidWeekRule
                    {
                        monday = true,
                        tuesday = true,
                        wednesday = true,
                        thursday = true,
                        friday = true,
                        saturday = false,
                        sunday = false
                    };
                }
                //周末票
                else
                {
                    return new ValidWeekRule
                    {
                        monday = true,
                        tuesday = true,
                        wednesday = true,
                        thursday = true,
                        friday = true,
                        saturday = true,
                        sunday = true
                    };
                }
            }
        }

        private List<DealImageInfo> GetDealImageInfos(int imgid)
        {
            List<DealImageInfo> list = new List<DealImageInfo>();
            list.Add(new DealImageInfo
            {
                imageName = "",
                imageUrl = imgid.ToString(),
                frontImage = true
            });
            return list;
        }

        /// <summary>
        /// 入园时间:统一设定8点到17点
        /// </summary>
        /// <returns></returns>
        private List<VoucherTime> GetVoucherTimes()
        {
            List<VoucherTime> list = new List<VoucherTime>();
            list.Add(new VoucherTime
            {
                voucherStartTime = 8 * 60,
                voucherEndTime = 17 * 60,
                needVoucherOrGetInTimeSupplement = false,
                voucherOrGetInTimeSupplement = ""
            });
            return list;
        }

        /// <summary>
        /// 具体的退款规则
        /// </summary>
        /// <param name="isneedbespeak">是否需要预约</param>
        /// <param name="QuitTicketMechanism">退票规则:0有效期内可退票；1有效期内/外均可退票；2不可退票</param>
        /// <returns></returns>
        private List<RefundStairsRule> GetRefundStairsRules(int isneedbespeak, int QuitTicketMechanism)
        {
            List<RefundStairsRule> list = new List<RefundStairsRule>();
            //无需预约
            if (isneedbespeak == 0)
            {
                //有效期内可退票
                if (QuitTicketMechanism == 0)
                {
                    RefundStairsRule rule1 = new RefundStairsRule
                    {
                        refundTimeType = 2,
                        refundLimitMinutes = 24 * 60,
                        refundFeeMode = 1,
                        refundFee = 0.00,
                        refundNote = "退款时间截止到有效期当天"
                    };
                    list.Add(rule1);
                    return list;
                }
                //有效期内/外均可退票,截止到有效期后7天的0点0分
                if (QuitTicketMechanism == 1)
                {
                    RefundStairsRule rule2 = new RefundStairsRule
                    {
                        refundTimeType = 3,
                        refundLimitMinutes = 6 * 24 * 60,
                        refundFeeMode = 1,
                        refundFee = 0.00,
                        refundNote = "退款时间截止到有效期结束后7天内"
                    };
                    list.Add(rule2);
                }
                return new List<RefundStairsRule>();
            }
            //需要预约
            else
            {
                if (QuitTicketMechanism == 0 || QuitTicketMechanism == 1)
                {
                    //退款时间在使用开始日期前1天的18点前
                    RefundStairsRule rule1 = new RefundStairsRule
                    {
                        refundTimeType = 1,
                        refundLimitMinutes = 24 * 60 - (18 * 60 + 0),
                        refundFeeMode = 1,
                        refundFee = 0.00,
                        refundNote = "退款时间截止到使用日期前一天18点前"
                    };
                    list.Add(rule1);
                    return list;
                }
                return new List<RefundStairsRule>();
            }
        }
         
    }
}
