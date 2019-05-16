using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;
using ETS2.VAS.Service.WebReference2;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;


namespace ETS2.VAS.Service.VASService.Data.Common
{
    public class SunShineInter
    {
        ////----------正式参数---------------
        //public string organization = "1000000438";//机构号
        //public string password = "mvjHC6yllZ";//密码
        //public string skey = "oiszFLRc";//DES密钥

        ////-----------测试参数----------------
        //public string organization = "1000000286";//机构号
        //public string password = "7Udu8O5m6r";//密码
        //public string skey = "vSK5eIRC";//des密钥

        #region  发送电子票
        internal string Add_Order(ApiService mapiservice, Api_yg_addorder_input minput)
        {
            StringBuilder builderOrder = new StringBuilder();
            builderOrder.Append(@"<?xml version=""1.0"" encoding=""utf-8"" ?>");
            builderOrder.Append(@"<business_trans version=""1.0"">");
            builderOrder.AppendFormat("<request_type>{0}</request_type>", "add_order");
            builderOrder.AppendFormat("<organization>{0}</organization>", mapiservice.Organization);//<!-机构号-->
            builderOrder.AppendFormat("<password>{0}</password>", mapiservice.Password);//<!-- 接口使用密码  y-->
            builderOrder.AppendFormat("<req_seq>{0}</req_seq>", minput.req_seq);//<!--请求流水号 y-->
            builderOrder.AppendFormat("<order>");//<!--订单信息-->
            builderOrder.AppendFormat("<product_num>{0}</product_num>", minput.product_num);//<!--产品编码 y-->
            builderOrder.AppendFormat("<num>{0}</num>", minput.num);//<!--购买数量 y-->
            builderOrder.AppendFormat("<mobile>{0}</mobile>", minput.mobile);//<!-- 手机号码 y-->
            builderOrder.AppendFormat("<use_date>{0}</use_date>", minput.use_date);//<!-- 使用时间 -->
            builderOrder.AppendFormat("<real_name_type>{0}</real_name_type>", minput.real_name_type);//<!-- 实名制类型：0无需实名 1一张一人,2一单一人,3一单一人+身份证-->
            builderOrder.AppendFormat("<real_name>{0}</real_name>", minput.real_name);//<!--真是姓名  ,隔开 最多3个名字 <=3 -->
            builderOrder.AppendFormat("<id_card>{0}</id_card>", minput.id_card);//<!--证件号码 -->
            builderOrder.AppendFormat("<card_type>{0}</card_type>", minput.card_type);//<!--证件类型0身份证；1其他证件 -->
            builderOrder.AppendFormat("</order>");
            builderOrder.AppendFormat("</business_trans>");

            string bstr = "";
            try
            {
                ITicketService its10 = new ITicketService();
                string en = EncryptionHelper.DESEnCode(builderOrder.ToString(), mapiservice.Deskey);
                string retxmls = its10.getEleInterface(mapiservice.Organization, en);

                bstr = EncryptionHelper.DESDeCode(retxmls, mapiservice.Deskey);
            }
            catch (Exception e)
            {
                bstr = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                            "<business_trans>" +
                                 "<response_type>add_order</response_type>" +
                                 "<req_seq>" + minput.req_seq + "</req_seq>" +
                                 "<result>" +
                                     "<id></id>" +
                                     "<comment>" + e.Message + "</comment>" +
                                " </result>" +
                           "</business_trans>";
            }
            //录入交互日志
            ApiLog mapilog = new ApiLog
            {
                Id = 0,
                request_type = "add_order",
                Serviceid = 1,
                Str = builderOrder.ToString().Trim(),
                Subdate = DateTime.Now,
                ReturnStr = bstr,
                ReturnSubdate = DateTime.Now,
                Errmsg = "",
            };
            int ins = new ApiLogData().EditLog(mapilog);

            return bstr;

        }
        #endregion

        #region  重发电子票
        public string repeat_order(ApiService mapiservice, Api_yg_addorder_output moutput)
        {
            string req_seq = mapiservice.Organization + DateTime.Now.ToString("yyyyMMddhhssmm") + CommonFunc.CreateNum(6);//请求流水号

            StringBuilder buildOrder = new StringBuilder();
            buildOrder.Append(@"<?xml version=""1.0"" encoding=""utf-8""?>");
            buildOrder.Append(@"<business_trans version=""1.0"">");
            buildOrder.Append("<request_type>repeat_order</request_type>");//<!--重发-->
            buildOrder.AppendFormat("<organization>{0}</organization>", mapiservice.Organization);//<!--机构号-->
            buildOrder.AppendFormat("<password>{0}</password>", mapiservice.Password);//<!-- 接口使用密码  -->
            buildOrder.AppendFormat("<req_seq>{0}</req_seq>", req_seq);//<!--请求流水号-->
            buildOrder.Append("<order>");
            buildOrder.AppendFormat("<order_num>{0}</order_num>", moutput.yg_ordernum);//<!-- 订单号 y-->    
            buildOrder.Append("</order>");
            buildOrder.Append("</business_trans>");

            string bstr = "";
            try
            {
                ITicketService its12 = new ITicketService();
                string en = EncryptionHelper.DESEnCode(buildOrder.ToString(), mapiservice.Deskey);
                string retxmls = its12.getEleInterface(mapiservice.Organization, en);
                bstr = EncryptionHelper.DESDeCode(retxmls, mapiservice.Deskey);


            }
            catch (Exception e)
            {
                bstr = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                           "<business_trans>" +
                                "<response_type>repeat_order</response_type>" +
                                "<req_seq>" + req_seq + "</req_seq>" +
                                "<result>" +
                                    "<id></id>" +
                                    "<comment>" + e.Message + "</comment>" +
                               " </result>" +
                          "</business_trans>";

            }

            //录入交互日志
            ApiLog mapilog = new ApiLog
            {
                Id = 0,
                request_type = "repeat_order",
                Serviceid = 1,
                Str = buildOrder.ToString().Trim(),
                Subdate = DateTime.Now,
                ReturnStr = bstr,
                ReturnSubdate = DateTime.Now,
                Errmsg = "",
            };
            int ins = new ApiLogData().EditLog(mapilog);

            return bstr;
        }
        #endregion

      
        #region 转发电子票
        public string sendto_order(string order_num, string old_mobile, string new_mobile)
        {
            ApiService mapiservice = new ApiServiceData().GetApiservice(1);

            string req_seq = mapiservice.Organization + DateTime.Now.ToString("yyyyMMddhhssmm") + CommonFunc.CreateNum(6);//请求流水号

            StringBuilder builderOrder = new StringBuilder();
            builderOrder.Append(@"<?xml version=""1.0"" encoding=""utf-8"" ?>");
            builderOrder.Append(@"<business_trans version=""1.0"">");
            builderOrder.AppendFormat("<request_type>{0}</request_type>", "sendto_order");
            builderOrder.AppendFormat("<organization>{0}</organization>", mapiservice.Organization);//<!-机构号-->
            builderOrder.AppendFormat("<password>{0}</password>", mapiservice.Password);//<!-- 接口使用密码  y-->
            builderOrder.AppendFormat("<req_seq>{0}</req_seq>", req_seq);//<!--请求流水号 y-->
            builderOrder.AppendFormat("<order>");//<!--订单信息-->
            builderOrder.AppendFormat("<order_num>{0}</order_num>", order_num);//<!--订单号 y-->
            builderOrder.AppendFormat("<old_mobile>{0}</old_mobile>", old_mobile);//<!-- 原手机号 y-->
            builderOrder.AppendFormat("<new_mobile>{0}</new_mobile>", new_mobile);//<!-- 新手机号 y -->
            builderOrder.AppendFormat("</order>");
            builderOrder.AppendFormat("</business_trans>");

            string bstr = "";
            try
            {
                ITicketService its10 = new ITicketService();
                string en = EncryptionHelper.DESEnCode(builderOrder.ToString(), mapiservice.Deskey);
                string retxmls = its10.getEleInterface(mapiservice.Organization, en);
                bstr = EncryptionHelper.DESDeCode(retxmls, mapiservice.Deskey);

            }
            catch (Exception e)
            {
                bstr = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<business_trans>" +
                               "<response_type>sendto_order</response_type>" +
                               "<req_seq>" + req_seq + "</req_seq>" +
                               "<result>" +
                                   "<id></id>" +
                                   "<comment>" + e.Message + "</comment>" +
                              " </result>" +
                         "</business_trans>";

            }
            //录入交互日志
            ApiLog mapilog = new ApiLog
            {
                Id = 0,
                request_type = "sendto_order",
                Serviceid = 1,
                Str = builderOrder.ToString().Trim(),
                Subdate = DateTime.Now,
                ReturnStr = bstr,
                ReturnSubdate = DateTime.Now,
                Errmsg = "",
            };
            int ins = new ApiLogData().EditLog(mapilog);

            return bstr;

        }
        #endregion  
        #region 查询电子票
        public string query_order(string req_seq_sel)
        {
            ApiService mapiservice = new ApiServiceData().GetApiservice(1);


            string req_seq = mapiservice.Organization + DateTime.Now.ToString("yyyyMMddhhssmm") + CommonFunc.CreateNum(6);//请求流水号

            StringBuilder builderOrder = new StringBuilder();
            builderOrder.Append(@"<?xml version=""1.0"" encoding=""utf-8""?>");
            builderOrder.Append(@"<business_trans version=""1.0"">");
            builderOrder.Append("<request_type>query_order</request_type>");//<!--查询-->
            builderOrder.AppendFormat("<organization>{0}</organization>", mapiservice.Organization);//<!--机构号-->
            builderOrder.AppendFormat("<password>{0}</password>", mapiservice.Password);//<!-- 接口使用密码  -->
            builderOrder.AppendFormat("<req_seq>{0}</req_seq>", req_seq);//<!--请求流水号-->
            builderOrder.Append("<order>");
            builderOrder.AppendFormat("<order_num>{0}</order_num>", req_seq_sel);//<!-- 订单号 y-->
            builderOrder.Append("</order>");
            builderOrder.Append("</business_trans>");

            string bstr = "";
            try
            {
                ITicketService its11 = new ITicketService();
                string en = EncryptionHelper.DESEnCode(builderOrder.ToString(), mapiservice.Deskey);
                string retxmls = its11.getEleInterface(mapiservice.Organization, en);

                bstr = EncryptionHelper.DESDeCode(retxmls, mapiservice.Deskey);

            }
            catch (Exception e)
            {
                bstr = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                       "<business_trans>" +
                            "<response_type>query_order</response_type>" +
                            "<req_seq>" + req_seq + "</req_seq>" +
                            "<result>" +
                                "<id></id>" +
                                "<comment>" + e.Message + "</comment>" +
                           " </result>" +
                      "</business_trans>";

            }
            //录入交互日志
            ApiLog mapilog = new ApiLog
            {
                Id = 0,
                request_type = "query_order",
                Serviceid = 1,
                Str = builderOrder.ToString().Trim(),
                Subdate = DateTime.Now,
                ReturnStr = bstr,
                ReturnSubdate = DateTime.Now,
                Errmsg = "",
            };
            int ins = new ApiLogData().EditLog(mapilog);

            return bstr;

        }
        #endregion

        #region  撤销电子票
        public string cancel_order(ApiService mapiservice, Api_yg_cancelorder m_ygcancelorder)
        {
            StringBuilder buildOrder = new StringBuilder();
            buildOrder.Append(@"<?xml version=""1.0"" encoding=""utf-8""?>");
            buildOrder.Append(@"<business_trans version=""1.0"">");
            buildOrder.Append("<request_type>cancel_order</request_type>");//<!--撤销-->
            buildOrder.AppendFormat("<organization>{0}</organization>", mapiservice.Organization);//<!--机构号-->
            buildOrder.AppendFormat("<password>{0}</password>", mapiservice.Password);//<!-- 接口使用密码  -->
            buildOrder.AppendFormat("<req_seq>{0}</req_seq>", m_ygcancelorder.req_seq);//<!--请求流水号-->
            buildOrder.Append("<order>");
            buildOrder.AppendFormat("<order_num>{0}</order_num>", m_ygcancelorder.ygorder_num);//<!-- 订单号 y-->
            buildOrder.AppendFormat("<num>{0}</num>", m_ygcancelorder.num);//<!-- 张数 y-->
            buildOrder.Append("</order>");
            buildOrder.Append("</business_trans>");

            string bstr = "";
            try
            {
                ITicketService its12 = new ITicketService();
                string en = EncryptionHelper.DESEnCode(buildOrder.ToString(), mapiservice.Deskey);
                string retxmls = its12.getEleInterface(mapiservice.Organization, en);
                bstr = EncryptionHelper.DESDeCode(retxmls, mapiservice.Deskey);

            }
            catch (Exception e)
            {
                bstr = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                     "<business_trans>" +
                          "<response_type>cancel_order</response_type>" +
                          "<req_seq>" + m_ygcancelorder.req_seq + "</req_seq>" +
                          "<result>" +
                              "<id></id>" +
                              "<comment>" + e.Message + "</comment>" +
                         " </result>" +
                    "</business_trans>";

            }

            //录入交互日志
            ApiLog mapilog = new ApiLog
            {
                Id = 0,
                request_type = "cancel_order",
                Serviceid = 1,
                Str = buildOrder.ToString().Trim(),
                Subdate = DateTime.Now,
                ReturnStr = bstr,
                ReturnSubdate = DateTime.Now,
                Errmsg = "",
            };
            int ins = new ApiLogData().EditLog(mapilog);

            return bstr;
        }
        #endregion

    }
}
