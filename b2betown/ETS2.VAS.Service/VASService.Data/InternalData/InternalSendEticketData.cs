using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.CRM.Service.CRMService.Modle;


namespace ETS2.VAS.Service.VASService.Data.InternalData
{
    public class InternalSendEticketData
    {
         private SqlHelper sqlHelper;
         public InternalSendEticketData(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        #region 设定的默认 客服绑定微信数量
         /// <summary>
         ///  设定的默认 客服绑定微信数量
         /// </summary>
         /// <param name="comid"></param>
         /// <returns></returns>
         internal List<B2b_crm> SearchIsDefaultKfList(int comid)
         {
            const string sqlTxt = @"SELECT   *
  FROM [b2b_crm] where com_id=@comid and phone in (select tel  from b2b_company_manageuser where com_id=@comid and Isdefaultkf=1)";

             var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
             cmd.AddParam("@Comid", comid);

             List<B2b_crm> list = new List<B2b_crm>();
             using (var reader = cmd.ExecuteReader())
             {
                 while (reader.Read())
                 {
                     list.Add(new B2b_crm
                     {
                         Id = reader.GetValue<int>("id"),
                         Weixin = reader.GetValue<string>("weixin"),
                         Phone = reader.GetValue<string>("Phone"),
                     });
                 }
             }
             return list;

         }
         #endregion


         #region 查询指定手机的会员绑定的微信
         /// <summary>
         ///  设定的默认 客服绑定微信数量
         /// </summary>
         /// <param name="phone"></param>
         /// <returns></returns>
         internal List<B2b_crm> SearchBindingGw(string phone,int comid)
         {
             const string sqlTxt = @"SELECT   *
  FROM [b2b_crm] where com_id=@comid and weixin !='' and phone =@phone";

             var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
             cmd.AddParam("@Comid", comid);
             cmd.AddParam("@phone", phone);

             List<B2b_crm> list = new List<B2b_crm>();
             using (var reader = cmd.ExecuteReader())
             {
                 while (reader.Read())
                 {
                     list.Add(new B2b_crm
                     {
                         Id = reader.GetValue<int>("id"),
                         Weixin = reader.GetValue<string>("weixin"),
                     });
                 }
             }
             return list;

         }
         #endregion

    }
}
