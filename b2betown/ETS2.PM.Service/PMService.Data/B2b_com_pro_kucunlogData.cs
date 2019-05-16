using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class B2b_com_pro_kucunlogData 
    {

        public int Editkucunlog(B2b_com_pro_kucunlog m)
        {
             using(var helper=new SqlHelper())
             {
                 int r = new Internalb2b_com_pro_kucunlog(helper).Editkucunlog(m);
                 return r;
             }
        }
    }
}
