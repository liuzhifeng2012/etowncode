using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.WL.Model;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.Framework;

namespace ETS2.PM.Service.WL.Data.Internal
{
	public class InternalWLDealResponse
	{
         public SqlHelper sqlHelper;
        public InternalWLDealResponse(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

       
	}
}
