using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.Member.Service.MemberService.Data.InternalData;
using ETS.Data.SqlHelper;
using ETS2.Member.Service.MemberService.Model;

namespace ETS2.Member.Service.MemberService.Data
{
    /// <summary>
    ///  卡号活动关联表 by：xiaoliu
    /// </summary>
    public class MemberCardActivityData
    {
        public int EditMemberCardActivity(Model.Member_Card_Activity cardact)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalMemberCardActivity(helper).EditMemberCardActivity(cardact);
                return id;
            }
        }

        public Member_Card_Activity GetCardActInfo(int cardid, int actid)
        {
            using (var helper = new SqlHelper())
            {
                Member_Card_Activity cardact = new InternalMemberCardActivity(helper).GetCardActInfo(cardid, actid);
                return cardact;
            }
        }
    }
}
