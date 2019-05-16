using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;
using System.Data;

namespace ETS2.CRM.Service.CRMService.Data.InternalData
{
    public class InternalB2bCompanySaleSet
    {
        private SqlHelper sqlHelper;
        public InternalB2bCompanySaleSet(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        #region 添加或者编辑公司基本信息

        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateB2bSaleSet";

        public int InsertOrUpdate(B2b_company_saleset model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);
            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Comid", model.Com_id);
            cmd.AddParam("@Payto", model.Payto);
            cmd.AddParam("@Model_style", model.Model_style);

            cmd.AddParam("@Title", model.Title);
            cmd.AddParam("@ServicePhone", model.Service_Phone);
            cmd.AddParam("@WorkingHours", model.WorkingHours);
            cmd.AddParam("@Copyright", model.Copyright);
            cmd.AddParam("@Tophtml", model.Tophtml);
            cmd.AddParam("@BottomHtml", model.BottomHtml);
            cmd.AddParam("@Dealuserid", model.Dealuserid);
            cmd.AddParam("@Smsaccount", model.Smsaccount);
            cmd.AddParam("@Smspass", model.Smspass);
            cmd.AddParam("@Smssign", model.Smssign);
            cmd.AddParam("@Logo", model.Logo);
            cmd.AddParam("@SmallLogo", model.Smalllogo);
            cmd.AddParam("@CompressionLogo", model.Compressionlogo);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion

        #region 修改短信账户
        public int Updatesms(B2b_company_saleset model)
        {
            var sqlTxt = @"UPDATE b2b_company_saleset SET 			   			   
			    Smsaccount=@Smsaccount,Smspass=@Smspass,Smssign=@Smssign,Smstype=@Smstype,SmsSubid=@Subid
		   WHERE Id = @Id;";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@comid", model.Com_id);
            cmd.AddParam("@Smsaccount", model.Smsaccount);
            cmd.AddParam("@Smspass", model.Smspass);
            cmd.AddParam("@Smssign", model.Smssign);
            cmd.AddParam("@Smstype", model.Smstype);
            cmd.AddParam("@Subid", model.Subid);

            return cmd.ExecuteNonQuery();
        }
        #endregion


        #region 得到直销设置实体
        internal B2b_company_saleset GetDirectSellByComid(string comid)
        {
            const string sqlTxt = @"SELECT *
  FROM [EtownDB].[dbo].[b2b_company_saleset] where com_id=@comid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_company_saleset u = null;

                while (reader.Read())
                {
                    u = new B2b_company_saleset
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Payto = reader.GetValue<int>("payto"),
                        Model_style = reader.GetValue<int>("model_style"),
                        Logo = reader.GetValue<string>("logo"),
                        Title = reader.GetValue<string>("title"),
                        Service_Phone = reader.GetValue<string>("Service_Phone"),
                        WorkingHours = reader.GetValue<string>("WorkingHours"),
                        Copyright = reader.GetValue<string>("Copyright"),
                        Tophtml = reader.GetValue<string>("tophtml"),
                        BottomHtml = reader.GetValue<string>("bottomHtml"),
                        Dealuserid = reader.GetValue<int>("dealuserid"),
                        Smsaccount = reader.GetValue<string>("Smsaccount"),
                        Smspass = reader.GetValue<string>("Smspass"),
                        Smssign = reader.GetValue<string>("Smssign"),
                        Smalllogo = reader.GetValue<string>("smalllogo"),
                        Compressionlogo = reader.GetValue<string>("compressionlogo"),
                        Smstype = reader.GetValue<int>("Smstype"),
                        Subid = reader.GetValue<string>("SmsSubid"),


                    };

                }
                return u;
            }
        }
        #endregion
    }
}
