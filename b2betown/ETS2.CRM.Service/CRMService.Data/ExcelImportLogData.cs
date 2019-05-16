using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Data.InternalData;

namespace ETS2.CRM.Service.CRMService.Data
{
    public  class ExcelImportLogData
    {
        public int GetMaxImportNum(int comid)
        {
            using (var helper = new SqlHelper())
            {

                var crmid = new InternalExcelImportLog(helper).GetMaxImportNum(comid);

                return crmid;
            }
        }

        public int InsExcelImportLog(int importstate, int crmid, int comid, string cardcode, string name, string phone, string weixin, int whetherwxfocus, int whetheractivate, string importtime, int importnum,string email="",string errreason="")
        {
            using (var helper = new SqlHelper())
            {

                int logid = new InternalExcelImportLog(helper).InsExcelImportLog(importstate, crmid, comid, cardcode, name, phone, weixin, whetherwxfocus, whetheractivate, importtime, importnum,email,errreason);

                return logid;
            }
        }
    }
}
