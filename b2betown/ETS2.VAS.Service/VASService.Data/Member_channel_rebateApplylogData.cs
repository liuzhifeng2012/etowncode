using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.VAS.Service.VASService.Modle;
using ETS.Data.SqlHelper;
using ETS2.VAS.Service.VASService.Data.InternalData;

namespace ETS2.VAS.Service.VASService.Data
{
    public class Member_channel_rebateApplylogData
    {
        public int Insrebateapplylog(Member_channel_rebateApplylog applylog)
        {
           using(var helper=new SqlHelper())
           {
               int r = new InternalMember_channel_rebateApplylog(helper).Insrebateapplylog(applylog);
               return r;
           }
        }

        public IList<Member_channel_rebateApplylog> Channelrebateapplylist(int pageindex, int pagesize, int channelid, string operstatus, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                IList<Member_channel_rebateApplylog> r = new InternalMember_channel_rebateApplylog(helper).Channelrebateapplylist(pageindex,pagesize,channelid,operstatus,out totalcount);
                return r;
            }
        }

        public IList<Member_channel_rebateApplylog> Channelrebateapplyalllist(int pageindex, int pagesize, int comid, string operstatus, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                IList<Member_channel_rebateApplylog> r = new InternalMember_channel_rebateApplylog(helper).Channelrebateapplyalllist(pageindex, pagesize, comid, operstatus, out totalcount);
                return r;
            }
        }

        public int Confirmcompletedakuan(int id, int operstatus, int opertor, string operremark, int zhuanzhangsucimg)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalMember_channel_rebateApplylog(helper).Confirmcompletedakuan(id,operstatus,opertor,operremark,zhuanzhangsucimg);
                return r;
            }
        }

        public decimal Getrebateapplytotal(int comid, string staffphone)
        {
            using (var helper = new SqlHelper())
            {
                decimal r = new InternalMember_channel_rebateApplylog(helper).Getrebateapplytotal(comid,staffphone);
                return r;
            }
        }

        public decimal Getrebatehastixian(int comid, string staffphone)
        {
            using (var helper = new SqlHelper())
            {
                decimal r = new InternalMember_channel_rebateApplylog(helper).Getrebatehastixian(comid, staffphone);
                return r;
            }
        }

        public decimal Getrebatenottixian(int comid, string staffphone)
        {
            using (var helper = new SqlHelper())
            {
                decimal r = new InternalMember_channel_rebateApplylog(helper).Getrebatenottixian(comid, staffphone);
                return r;
            }
        }
    }
}
