using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class B2b_eticket_usesetData
    {
        public List<B2b_eticket_useset> GetB2b_eticket_useset(string daydate, int comid)
        {
            return null;
             //using(var helper=new  SqlHelper())
             //{
             //    List<B2b_eticket_useset> r = new Internalb2b_eticket_useset(helper).GetB2b_eticket_useset(daydate,comid);
             //    return r;
             //}
        }

        public int Delcomusesetbycomid(int comid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new Internalb2b_eticket_useset(helper).Delcomusesetbycomid( comid);
                return r;
            }
            
        }

        public int Inscomusesetbycomid(B2b_eticket_useset m)
        {
            using (var helper = new SqlHelper())
            {
                int r = new Internalb2b_eticket_useset(helper).Inscomusesetbycomid(m);
                return r;
            }
        }



        public List<B2b_eticket_useset> Geteticket_usesetlist(int comid,string nowdatetype="0,1,2")
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_eticket_useset> r = new Internalb2b_eticket_useset(helper).Geteticket_usesetlist(comid,nowdatetype);
                return r;
            }
        }
    }
}
