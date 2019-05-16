using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.VAS.Service.VASService.Modle;
using ETS.Data.SqlHelper;
using ETS2.VAS.Service.VASService.Data.InternalData;

namespace ETS2.VAS.Service.VASService.Data
{
    public class Member_channel_rebateApplyaccountData
    {
        public Member_channel_rebateApplyaccount GetchanelrebateApplyaccount(int channelid)
        {
            using(var helper=new SqlHelper())
            {
                Member_channel_rebateApplyaccount m = new InternalMember_channel_rebateApplyaccount(helper).GetchanelrebateApplyaccount(channelid);
                return m;
            }
        }

        public int Upchannelrebateaccount(int channelid, string truename, string account, string newphone,int comid)
        {
            using (var helper = new SqlHelper())
            {
                int  m = new InternalMember_channel_rebateApplyaccount(helper).Upchannelrebateaccount(channelid,truename,account,newphone,comid);
                return m;
            }
        }

       
    }
}
