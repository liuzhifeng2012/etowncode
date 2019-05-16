using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.VAS.Service.VASService.Modle;

namespace ETS2.VAS.Service.VASService.Data.InternalData
{
    public class InternalB2b_finance_paytype
    {
        public SqlHelper sqlHelper;
        public InternalB2b_finance_paytype(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal B2b_finance_paytype GetFinancePayTypeByComid(int comid)
        {
            string sql = @"SELECT [id]
      ,[com_id]
      ,[paytype]
      ,[bank_account]
      ,[bank_card]
      ,[bank_name]
      ,[alipay_account]
      ,[alipay_id]
      ,[alipay_key]
      ,[userbank_account]
      ,[userbank_card]
      ,[userbank_name]
      ,[uptype]
      ,[wx_appid]
      ,[wx_appkey]
      ,[wx_paysignkey]
      ,[wx_partnerid]
      ,[wx_partnerkey]
      ,tenpay_id
      ,tenpay_key
     ,wx_SSLCERT_PATH
    ,wx_SSLCERT_PASSWORD
  FROM [EtownDB].[dbo].[b2b_finance_paytype] where com_id=@comid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_finance_paytype u = null;
                if (reader.Read())
                {
                    u = new B2b_finance_paytype()
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Paytype = reader.GetValue<int>("paytype"),
                        Bank_account = reader.GetValue<string>("bank_account"),
                        Bank_card = reader.GetValue<string>("bank_card"),
                        Bank_name = reader.GetValue<string>("bank_name"),
                        Alipay_account = reader.GetValue<string>("alipay_account"),
                        Alipay_id = reader.GetValue<string>("alipay_id"),
                        Alipay_key = reader.GetValue<string>("alipay_key"),
                        Userbank_account = reader.GetValue<string>("userbank_account"),
                        Userbank_card = reader.GetValue<string>("userbank_card"),
                        Userbank_name = reader.GetValue<string>("userbank_name"),
                        Uptype = reader.GetValue<int>("uptype"),
                        Wx_appid = reader.GetValue<string>("wx_appid"),
                        Wx_appkey = reader.GetValue<string>("wx_appkey"),
                        Wx_paysignkey = reader.GetValue<string>("wx_paysignkey"),
                        Wx_partnerid = reader.GetValue<string>("wx_partnerid"),
                        Wx_partnerkey = reader.GetValue<string>("wx_partnerkey"),
                        Tenpay_id = reader.GetValue<string>("tenpay_id"),
                        Tenpay_key = reader.GetValue<string>("tenpay_key"),
                        wx_SSLCERT_PATH = reader.GetValue<string>("wx_SSLCERT_PATH"),
                        wx_SSLCERT_PASSWORD = reader.GetValue<string>("wx_SSLCERT_PASSWORD"),
                    };
                }
                return u;
            }

        }
    }
}
