using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class B2b_com_pro_SpecitypeData
    {
        public int Editguigetype(B2b_com_pro_Specitype mguigetype)
        {
             using(var helper=new SqlHelper())
             {
                 int typeid = new Internalb2b_com_pro_Specitype(helper).Editguigetype(mguigetype);
                 return typeid;
             }
        }



        public List<B2b_com_pro_Specitype> Getggtypelist(int proid)
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_com_pro_Specitype> r = new Internalb2b_com_pro_Specitype(helper).Getggtypelist(proid);
                return r;
            }
        }

        public int Getggtypemaxid(int proid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new Internalb2b_com_pro_Specitype(helper).Getggtypemaxid(proid);
                return r;
            }
        }
    }
}
