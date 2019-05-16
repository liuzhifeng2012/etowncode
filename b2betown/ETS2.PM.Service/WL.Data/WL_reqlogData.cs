using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.WL.Model;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.WL.Data.Internal;

namespace ETS2.PM.Service.WL.Data
{
    public class WL_reqlogData
    {

        public int EditReqlog(WL_reqlog m)
        {
            using (var helper = new SqlHelper())
            {
                int id = new InternalWL_reqlog(helper).EditReqlog(m);
                return id;
            }
        }
    }
}
