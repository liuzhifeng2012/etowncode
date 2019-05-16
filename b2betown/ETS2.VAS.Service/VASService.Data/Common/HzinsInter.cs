using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Framework;
using ETS2.PM.Service.PMService.Data;

namespace ETS2.VAS.Service.VASService.Data.Common
{
    public class HzinsInter
    {
        private string partnerId = "146491";//渠道商身份标识，由慧择指定
        private string signkey = "146491#%##^11";//签名秘钥，由慧择指定
        //承保 
        public string Hzins_OrderApply(B2b_com_pro modelcompro, B2b_order modelb2border)
        {
            string transNo = modelb2border.Id.ToString();//(以后需要修改为渠道商订单号)交易流水号，每次请求不能相同
            string caseCode = modelcompro.Service_proid;//(以后需要更改为正式产品)方案代码，每一款保险公司产品的方案代码都不相同，由慧择分配

            //签名，预签名数据：密钥+渠道商身份标识+交易流水号+方案代码
            string sign = EncryptionHelper.ToMD5(signkey + partnerId + transNo + caseCode, "UTF-8");
            sign = sign.ToLower();

            //根据订单号 得到慧择网 承保信息
            Api_hzins_OrderApplyReq_Application mapplication = new Api_hzins_OrderApplyReq_ApplicationData().GetOrderApplyReq_Application(modelb2border.Id);
            if (mapplication == null)
            {
                TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\BaoxianLog.txt", "fail1");
                return "fail";
            }
            Api_hzins_OrderApplyReq_applicantInfo mapplicationinfo = new Api_hzins_OrderApplyReq_applicantInfoData().GetOrderApplyReq_applicantInfo(modelb2border.Id);
            if (mapplicationinfo == null)
            {
                TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\BaoxianLog.txt", "fail2");
                return "fail";
            }
            List<Api_hzins_OrderApplyReq_insurantInfo> listinsurantInfo = new Api_hzins_OrderApplyReq_insurantInfoData().GetOrderApplyReq_insurantInfo(modelb2border.Id);
            if (listinsurantInfo == null)
            {
                TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\BaoxianLog.txt", "fail3");
                return "fail";
            }
            else
            {
                if (listinsurantInfo.Count == 0)
                {
                    TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\BaoxianLog.txt", "fail4");
                    return "fail";
                }
            }

            string s = "{" +
                        "\"partnerId\":" + partnerId + "," +
                        "\"transNo\":\"" + transNo + "\"," +
                        "\"caseCode\":\"" + caseCode + "\"," +
                        "\"sign\":\"" + sign + "\"," +
                        "\"applicationData\":{" +
                                "\"applicationDate\":\"" + mapplication.applicationDate + "\"," +
                                "\"startDate\":\"" + mapplication.startDate + "\"," +
                                "\"endDate\":\"" + mapplication.endDate + "\"" +
                        "}," +
                        "\"applicantInfo\":{" +
                                 "\"cName\":\"" + mapplicationinfo.cName + "\"," +
                                  "\"eName\":\"" + mapplicationinfo.eName + "\"," +
                                  "\"cardType\":\"" + mapplicationinfo.cardType + "\"," +
                                  "\"cardCode\":\"" + mapplicationinfo.cardCode + "\"," +
                                  "\"sex\":" + mapplicationinfo.sex + "," +
                                   "\"birthday\":\"" + mapplicationinfo.birthday + "\"," +
                                   "\"mobile\":\"" + mapplicationinfo.mobile + "\"," +
                                   "\"email\":\"" + mapplicationinfo.email + "\"," +
                                   "\"jobInfo\":null" +
                         "}," +
                        "\"insurantInfos\":[";

            string insurantInfosstr = "";
            foreach (Api_hzins_OrderApplyReq_insurantInfo minsinfo in listinsurantInfo)
            {
                if (minsinfo != null)
                {
                    insurantInfosstr += "{" +
                      "\"insurantId\":" + minsinfo.insurantId + "," +
                      "\"cName\":\"" + minsinfo.cName + "\"," +
                      "\"eName\":\"" + minsinfo.eName + "\"," +
                      "\"sex\":" + minsinfo.sex + "," +
                      "\"cardType\":" + minsinfo.cardType + "," +
                      "\"cardCode\":\"" + minsinfo.cardCode + "\"," +
                      "\"birthday\":\"" + minsinfo.birthday + "\"," +
                      "\"relationId\":" + minsinfo.relationId + "," +
                      "\"count\":" + minsinfo.count + "," +
                      "\"singlePrice\":" + minsinfo.singlePrice + "," +
                      "\"fltNo\":null," +
                      "\"fltDate\":null," +
                      "\"city\":\"null\"," +
                      "\"tripPurposeId\":" + minsinfo.tripPurposeId + "," +
                       "\"destination\":null," +
                       "\"visaCity\":null," +
                       "\"jobInfo\":null," +
                       "\"mobile\":null" +
                 "},";
                }
            }
            insurantInfosstr = insurantInfosstr.Substring(0, insurantInfosstr.Length - 1);

            s += insurantInfosstr + "]," +
             "\"extendInfo\":{" +
                  "\"userId\":null," +
                  "\"email\":null," +
                  "\"userName\":null," +
                  "\"phone\":null" +
              "}" +
 "}";

            string str = new GetUrlData().HttpPostJson("http://channel.hzins.com/api/orderApply", s.Trim());

            //录入和慧择网交互日志
            ApiLog mapilog = new ApiLog
            {
                Id = 0,
                request_type = "orderApply",
                Serviceid = 2,
                Str = s.Trim(),
                Subdate = DateTime.Now,
                ReturnStr = str,
                ReturnSubdate = DateTime.Now,
                Errmsg = "",
            };
            int ins = new ApiLogData().EditLog(mapilog);

            //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\BaoxianLog.txt", "发送信息:" + s.Trim() + "<br>" + "返回信息:" + str);
            return str;
        }

        //投保单查询
        public string Hzins_orderDetail(int orderid)
        {

            string transNo = orderid.ToString();//(以后需要修改为渠道商订单号)易流水号，每次请求不能相同

            //根据订单号得到投保单号
            string insureNo = new Api_hzins_OrderApplyResp_OrderExtData().GetinsureNumbyorderid(orderid);//投保单号
            if (insureNo == "")
            {
                return "";
            }

            //签名，预签名数据：密钥+渠道商身份标识+交易流水号+方案代码
            string sign = EncryptionHelper.ToMD5(signkey + partnerId + transNo + insureNo, "UTF-8");
            sign = sign.ToLower();

            string s = "{" +
                            "\"transNo\":\"" + transNo + "\"," +
                            "\"partnerId\":" + partnerId + "," +
                            "\"insureNum\":\"" + insureNo + "\"," +
                            "\"sign\":\"" + sign + "\"," +
                            "\"idCard\":null," +
                            "\"email\":null," +
                            "\"userId\":null," +
                            "\"pageNum\":1," +
                            "\"pageSize\":5" +
                        "}";

            string str = new GetUrlData().HttpPostJson("http://channel.hzins.com/api/orderDetail", s.Trim());

            //录入和慧择网交互日志
            ApiLog mapilog = new ApiLog
            {
                Id = 0,
                request_type = "orderDetail",
                Serviceid = 2,
                Str = s.Trim(),
                Subdate = DateTime.Now,
                ReturnStr = str,
                ReturnSubdate = DateTime.Now,
                Errmsg = "",
            };
            int ins = new ApiLogData().EditLog(mapilog);

            //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\BaoxianLog.txt", "发送信息:" + s.Trim() + "<br>" + "返回信息:" + str);
            return str;
        }

        //退保
        public string Hzins_orderCancel(int orderid)
        {

            string transNo = orderid.ToString(); //(以后需要修改为渠道商订单号)易流水号，每次请求不能相同

            //根据订单号得到投保单号
            string insureNo = new Api_hzins_OrderApplyResp_OrderExtData().GetinsureNumbyorderid(orderid);//投保单号
            if (insureNo == "")
            {
                return "";
            }

            //签名，预签名数据：密钥+渠道商身份标识+交易流水号+方案代码
            string sign = EncryptionHelper.ToMD5(signkey + partnerId + transNo + insureNo, "UTF-8");
            sign = sign.ToLower();

            string s = "{" +
                    "\"transNo\":\"" + transNo + "\"," +
                    "\"partnerId\":" + partnerId + "," +
                    "\"insureNo\":\"" + insureNo + "\"," +
                    "\"sign\":\"" + sign + "\"," +
                    "\"extendInfo\":{" +
                        "\"userId\":null," +
                        "\"email\":null," +
                        "\"userName\":null," +
                        "\"phone\":null" +
                    "}" +
                "}";

            string str = new GetUrlData().HttpPostJson("http://channel.hzins.com/api/orderCancel", s.Trim());

            //录入和慧择网交互日志
            ApiLog mapilog = new ApiLog
            {
                Id = 0,
                request_type = "orderCancel",
                Serviceid = 2,
                Str = s.Trim(),
                Subdate = DateTime.Now,
                ReturnStr = str,
                ReturnSubdate = DateTime.Now,
                Errmsg = "",
            };
            int ins = new ApiLogData().EditLog(mapilog);

            //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\BaoxianLog.txt", "发送信息:" + s.Trim() + "<br>" + "返回信息:" + str);
            return str;
        }

        //投保单详情
        public string Hzins_insureDetail(int orderid)
        {
            string transNo = orderid.ToString();//(以后需要修改为渠道商订单号)易流水号，每次请求不能相同

            //根据订单号得到投保单号
            string insureNo = new Api_hzins_OrderApplyResp_OrderExtData().GetinsureNumbyorderid(orderid);//投保单号
            if (insureNo == "")
            {
                return "";
            }


            //签名，预签名数据：密钥+渠道商身份标识+交易流水号+方案代码
            string sign = EncryptionHelper.ToMD5(signkey + partnerId + transNo + insureNo, "UTF-8");
            sign = sign.ToLower();

            string s = "{" +
                            "\"transNo\":\"" + transNo + "\"," +
                            "\"partnerId\":" + partnerId + "," +
                            "\"insureNum\":\"" + insureNo + "\"," +
                            "\"sign\":\"" + sign + "\"" +
                        "}";

            string str = new GetUrlData().HttpPostJson("http://channel.hzins.com/api/insureDetail", s.Trim());

            //录入和慧择网交互日志
            ApiLog mapilog = new ApiLog
            {
                Id = 0,
                request_type = "insureDetail",
                Serviceid = 2,
                Str = s.Trim(),
                Subdate = DateTime.Now,
                ReturnStr = str,
                ReturnSubdate = DateTime.Now,
                Errmsg = "",
            };
            int ins = new ApiLogData().EditLog(mapilog);

            //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\BaoxianLog.txt", "发送信息:" + s.Trim() + "<br>" + "返回信息:" + str);
            return str;
        }

        //投保单批量查询
        public string Hzins_orderSearch(string orderidstr)
        {

            string transNo = orderidstr;//(以后需要修改为渠道商订单号)易流水号，每次请求不能相同

            //根据订单号列表得到投保单号列表
            List<Api_hzins_OrderApplyResp_OrderExt> list = new Api_hzins_OrderApplyResp_OrderExtData().GetinsureNumsbyorderids(orderidstr);
            if (list == null)
            {
                return "";
            }
            else
            {
                if (list.Count == 0)
                {
                    return "";
                }
            }
            //需要对insureNos处理一下，形如:["15080546794635","15080544341441"]
            string insureNos = "";
            foreach (Api_hzins_OrderApplyResp_OrderExt ext in list)
            {
                insureNos += "\"" + ext.insureNum + "\",";
            }
            if (insureNos != "")
            {
                insureNos = insureNos.Substring(0, insureNos.Length - 1);
            }

            //签名，预签名数据：密钥+渠道商身份标识+交易流水号 
            string sign = EncryptionHelper.ToMD5(signkey + partnerId + transNo, "UTF-8");
            sign = sign.ToLower();

            string s = "{" +
                            "\"transNo\":\"" + transNo + "\"," +
                            "\"partnerId\":" + partnerId + "," +
                            "\"sign\":\"" + sign + "\"," +
                            "\"insureNums\":[" + insureNos + "]," +
                            "\"applicant\":null," +
                            "\"insurant\":null," +
                            "\"idCard\":null," +
                            "\"userId\":null," +
                            "\"startTime\":null," +
                            "\"endTime\":null," +
                            "\"pageNum\":1," +
                            "\"pageSize\":" + list.Count +
                        "}";

            string str = new GetUrlData().HttpPostJson("http://channel.hzins.com/api/orderSearch", s.Trim());

            //录入和慧择网交互日志
            ApiLog mapilog = new ApiLog
            {
                Id = 0,
                request_type = "orderSearch",
                Serviceid = 2,
                Str = s.Trim(),
                Subdate = DateTime.Now,
                ReturnStr = str,
                ReturnSubdate = DateTime.Now,
                Errmsg = "",
            };
            int ins = new ApiLogData().EditLog(mapilog);

            //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\BaoxianLog.txt", "发送信息:" + s.Trim() + "<br>" + "返回信息:" + str);
            return str;
        }
    }
}
