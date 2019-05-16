using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data
{
    public class B2b_delivery_tmpData
    {
        public int Uplimitbuytotalnum(int ComputedPriceMethod, string join_provinces, string deliverytypes, int tmpid, string tmpname, string join_deliverytype, string join_areas, string join_startstandards, string join_startfees, string join_addstandards, string join_addfees, int comid, int operor, out string errmsg)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2b_delivery_tmp(helper).Uplimitbuytotalnum(ComputedPriceMethod,join_provinces, deliverytypes, tmpid, tmpname, join_deliverytype, join_areas, join_startstandards, join_startfees, join_addstandards, join_addfees, comid, operor, out errmsg);
                return r;
            }
        }

        //public  B2b_delivery_tmp Gettmpbycomid(int comid)
        //{
        //    using (var helper = new SqlHelper())
        //    {
        //        B2b_delivery_tmp r = new InternalB2b_delivery_tmp(helper).Gettmpbycomid(comid);
        //        return r;
        //    }
        //}

        public B2b_delivery_tmp Getdeliverytmp(int tmpid)
        {
            using (var helper = new SqlHelper())
            {
                B2b_delivery_tmp r = new InternalB2b_delivery_tmp(helper).Getdeliverytmp(tmpid);
                return r;
            }
        }

        public IList<B2b_delivery_tmp> Getdeliverytmplist(int comid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                IList<B2b_delivery_tmp> r = new InternalB2b_delivery_tmp(helper).Getdeliverytmplist(comid, out totalcount);
                return r;
            }
        }
        public IList<B2b_delivery_tmp> Getdeliverytmppagelist(int comid,int pageindex,int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                IList<B2b_delivery_tmp> r = new InternalB2b_delivery_tmp(helper).Getdeliverytmppagelist(comid,pageindex,pagesize, out totalcount);
                return r;
            }
        }


        public int deltmp(int comid, int tmpid, out string errmsg)
        {
               using (var helper = new SqlHelper())
            {
                int r = new InternalB2b_delivery_tmp(helper).deltmp(comid,  tmpid, out errmsg );
                return r;
            }
        }

        public bool WhetherSaled(int lineid, string daydate)
        {
            using (var helper = new SqlHelper())
            {
                bool r = new InternalB2b_delivery_tmp(helper).WhetherSaled(lineid, daydate);
                return r;
            }
        }
    }
}
