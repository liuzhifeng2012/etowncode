using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.VAS.Service.VASService.Modle;
using ETS.Data.SqlHelper;
using ETS2.VAS.Service.VASService.Data.InternalData;

namespace ETS2.VAS.Service.VASService.Data
{
    public class Member_channel_rebatelogData
    {
        public int Editrebatelog(Member_channel_rebatelog rebatelog)
        {
             using(var helper=new SqlHelper())
             {
                 int r = new InternalMember_channel_rebatelog(helper).Editrebatelog(rebatelog);
                 return r;
             }
        }

        public decimal Getrebatemoney(int channelid)
        {
            using (var helper = new SqlHelper())
            {
                decimal r = new InternalMember_channel_rebatelog(helper).Getrebatemoney(channelid);
                return r;
            }
        }

        public int Editchannelrebate(int channelid, decimal overmoney)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalMember_channel_rebatelog(helper).Editchannelrebate(channelid,overmoney);
                return r;
            }
        }

        public IList<Member_channel_rebatelog> Channelrebatelist(int pageindex, int pagesize, int channelid, string payment,out int totalcount)
        {
             using(var helper=new SqlHelper())
             {
                 IList<Member_channel_rebatelog> r = new InternalMember_channel_rebatelog(helper).Channelrebatelist(pageindex,pagesize,channelid,payment,out totalcount);
                 return r;
             }
        }
        /// <summary>
        /// 根据产品id查询返佣进账记录
        /// </summary>
        /// <param name="proid"></param>
        /// <returns></returns>
        public Member_channel_rebatelog GetRebateIncomelog(int proid)
        {
            using (var helper = new SqlHelper())
            {
                Member_channel_rebatelog r = new InternalMember_channel_rebatelog(helper).GetRebateIncomelog(proid);
                return r;
            }
        }

        public int Getrebatenum(int comid, string staffphone)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalMember_channel_rebatelog(helper).Getrebatenum(comid,staffphone);
                return r;
            }
        } 
    }
}
