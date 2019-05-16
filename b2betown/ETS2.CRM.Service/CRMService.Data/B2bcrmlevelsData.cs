using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Data.InternalData;

namespace ETS2.CRM.Service.CRMService.Data
{
    public class B2bcrmlevelsData
    {
        public List<B2bcrmlevels> Getb2bcrmlevelsbycomid(int comid,out int totalcount)
        {
             using(var helper=new SqlHelper())
             {
                 List<B2bcrmlevels> list = new InternalB2bcrmlevels(helper).Getb2bcrmlevelsbycomid(comid,out totalcount);
                 return list;
             }
        }

        public int EditB2bCrmLevel(B2bcrmlevels m)
        {
            using (var helper = new SqlHelper())
            {
                int d = new InternalB2bcrmlevels(helper).EditB2bCrmLevel(m);
                return d;
            }
        }

        public B2bcrmlevels Getb2bcrmlevel(int comid, string crmlevel)
        {
            using (var helper = new SqlHelper())
            {
                B2bcrmlevels d = new InternalB2bcrmlevels(helper).Getb2bcrmlevel(comid, crmlevel);
                return d;
            }
        }

        public int Getcrmlevelscount(int comid)
        {
            using (var helper = new SqlHelper())
            {
                int d = new InternalB2bcrmlevels(helper).Getcrmlevelscount(comid);
                return d;
            }
             
        }

        public B2bcrmlevels Getb2bcrmlevelbyweixin(string weixin, int comid)
        {
            using (var helper = new SqlHelper())
            {
                B2bcrmlevels d = new InternalB2bcrmlevels(helper).Getb2bcrmlevelbyweixin(weixin, comid);
                return d;
            }
        }
        /// <summary>
        /// 根据公司id 和 最小等积分 获得会员级别
        /// </summary>
        /// <param name="comid"></param>
        /// <param name="djf_begin"></param>
        /// <returns></returns>
        public B2bcrmlevels Getb2bcrmlevel(int comid, decimal djf_begin)
        {
            using (var helper = new SqlHelper())
            {
                B2bcrmlevels d = new InternalB2bcrmlevels(helper).Getb2bcrmlevelbyweixin(comid, djf_begin);
                return d;
            }
        }
    }
}
