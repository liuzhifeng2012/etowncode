using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;

namespace ETS2.WeiXin.Service.WeiXinService.Data.InternalData
{
    public class InternalWxwarning
    {
        public SqlHelper sqlHelper;
        public InternalWxwarning(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
    }
}
