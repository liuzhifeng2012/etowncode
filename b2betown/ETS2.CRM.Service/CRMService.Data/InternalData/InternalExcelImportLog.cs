using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;

namespace ETS2.CRM.Service.CRMService.Data.InternalData
{
    public class InternalExcelImportLog
    {
        private SqlHelper sqlHelper;
        public InternalExcelImportLog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int GetMaxImportNum(int comid)
        {
            string sql = "select max(importnum) from excelimportlog where comid=" + comid;
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null ? 0 : int.Parse(o.ToString());
            }
            catch (Exception e)
            {
                sqlHelper.Dispose();
                return 0;
            }
        }

        internal int InsExcelImportLog(int importstate, int crmid, int comid, string cardcode, string name, string phone, string weixin, int whetherwxfocus, int whetheractivate, string importtime, int importnum, string email = "", string errreason = "")
        {
            string sql = "INSERT INTO [EtownDB].[dbo].[ExcelImportLog]" +
           "(importstate,[crmid],[weixin] ,[phone],[name],[idcard],[sex],[email],[importtime] ,[age],[birthday] ,[whetherwxfocus] ,[whetheractivate],[comid],[importnum],errreason)" +
           "VALUES(" + importstate + "," + crmid + ",'" + weixin + "','" + phone + "','" + name + "','" + cardcode + "','','" + email + "','" + importtime + "',0,'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," + whetherwxfocus + "," + whetheractivate + "," + comid + "," + importnum + ",'" + errreason + "');SELECT @@IDENTITY;";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                return o == null ? 0 : int.Parse(o.ToString());
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        internal int InternalObtainGzListLog()
        {
            throw new NotImplementedException();
        }
    }
}
