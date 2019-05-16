using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class B2b_com_protripData
    {
        public List<B2b_com_protrip> Gettriplistbylineid(int productid)
        {
             using (var helper=new SqlHelper())
             {
                 List<B2b_com_protrip> r = new InternalB2b_com_protrip(helper).Gettriplistbylineid(productid);
                 return r;
             }
        }

        public B2b_com_protrip GetLineTripById(int tripid, int productid)
        {
            using (var helper = new SqlHelper())
            {

                B2b_com_protrip r = new InternalB2b_com_protrip(helper).GetLineTripById(tripid,productid);
                return r;
            }
        }

        public int Edittrip(B2b_com_protrip tourJourney)
        {
            using (var helper = new SqlHelper())
            {

                int r = new InternalB2b_com_protrip(helper).Edittrip(tourJourney);
                return r;
            }
        }

        public int DeleteLineTrip(int tripid, int ProductId)
        {
            using (var helper = new SqlHelper())
            {

                int r = new InternalB2b_com_protrip(helper).DeleteLineTrip(tripid,ProductId);
                return r;
            }
        }
    }
}
