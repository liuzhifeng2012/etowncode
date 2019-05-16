using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.Member.Service.MemberService.Model;
using ETS.Data.SqlHelper;
using ETS2.Member.Service.MemberService.Data.InternalData;

namespace ETS2.Member.Service.MemberService.Data
{
    public class ChannelRebateLogData
    {
        public int EditChannelRebateLog(ChannelRebateLog channelrebatelog)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    int result = new InternalChannelRebateLog(sql).EditChannelRebateLog(channelrebatelog);
                    return result;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

        }
    }
}
