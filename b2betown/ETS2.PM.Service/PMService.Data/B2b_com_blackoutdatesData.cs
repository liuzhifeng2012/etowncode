using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class B2b_com_blackoutdatesData
    {
        public List<B2b_com_blackoutdates> Getblackoutdatebycomid(int comid, string state)
        {
            using (var helper=new  SqlHelper())
            {
                List<B2b_com_blackoutdates> r = new Internalb2b_com_blackoutdates(helper).Getblackoutdatebycomid(comid,state);
                return r;
            }
        }

        public B2b_com_blackoutdates Getblackoutdate(string daydate, int comid)
        {
            using (var helper = new SqlHelper())
            {
                B2b_com_blackoutdates r = new Internalb2b_com_blackoutdates(helper).Getblackoutdate(daydate, comid);
                return r;
            }
        }

        public int Delblackoutdate(string daydate, int comid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new Internalb2b_com_blackoutdates(helper).Delblackoutdate(daydate, comid);
                return r;
            }
        }

        public int InsB2b_com_blackoutdates(B2b_com_blackoutdates model)
        {
            using (var helper = new SqlHelper())
            {
                int r = new Internalb2b_com_blackoutdates(helper).InsB2b_com_blackoutdates( model  );
                return r;
            }
        } 
    }
}
