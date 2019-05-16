using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.Common.Business;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Member.Service.MemberService.Data;

namespace ETS2.WebApp.UI.VASUI
{
    public partial class Chanelrebatelist : System.Web.UI.Page
    {
        public int channelid = 0;//登录用户对应的渠道id
        public decimal rebatemoney = 0;//渠道返佣余额
        protected void Page_Load(object sender, EventArgs e)
        {
            if(UserHelper.ValidateLogin())
            {
                Member_Channel m = new MemberChannelData().GetChannelByMasterId(UserHelper.CurrentUserId());
                if(m!=null)
                {
                    channelid = m.Id;
                    rebatemoney = m.Rebatemoney;
                }
            }
        }
    }
}