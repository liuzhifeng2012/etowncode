using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class B2b_order_bespeakData
    {
        public int Subbespeak( B2b_order_bespeak m)
        {
            using(var helper=new SqlHelper())
            {
                return new Internalb2b_order_bespeak(helper).Subbespeak(m);
            } 
        }

        public int GetBespeaknum(string pno, string bespeakdate)
        {
            using (var helper = new SqlHelper())
            {
                return new Internalb2b_order_bespeak(helper).GetBespeaknum(pno,bespeakdate);
            } 
        }

        public List<B2b_order_bespeak> Getbespeaklist(string comid, int pageindex, int pagesize, string bespeakdate, string key, string bespeaktype, string bespeakstate, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                return new Internalb2b_order_bespeak(helper).Getbespeaklist(comid,pageindex,pagesize,bespeakdate,key,bespeaktype,bespeakstate,out totalcount);
            } 
        }

        public int Operbespeakstate(int id, int bespeakstate)
        {
            using (var helper = new SqlHelper())
            {
                return new Internalb2b_order_bespeak(helper).Operbespeakstate( id, bespeakstate);
            } 
        }

        public int Gettotalbespeaknum(int proid, DateTime bespeakdate)
        {
            using (var helper = new SqlHelper())
            {
                return new Internalb2b_order_bespeak(helper).Gettotalbespeaknum(proid, bespeakdate);
            } 
        }

        public B2b_order_bespeak GetbespeakByid(int id)
        {
            using (var helper = new SqlHelper())
            {
                return new Internalb2b_order_bespeak(helper).GetbespeakByid(id);
            } 
        }

        public B2b_order_bespeak Geteffectivebespeak(string pno)
        {
            using (var helper = new SqlHelper())
            {
                return new Internalb2b_order_bespeak(helper).Geteffectivebespeak(pno);
            } 
        }
    }
}
