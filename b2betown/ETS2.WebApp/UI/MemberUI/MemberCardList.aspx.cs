using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI.MemberUI
{
    public partial class MemberCardList : System.Web.UI.Page
    {
        public int issueid = 0;//发行id
        public int channelid = 0;//渠道id
        public int actid = 0;//活动id
        public decimal cardcode = 0;//卡号
        public int isopencard = 0;//开卡状态，默认全部0；已开卡1；未开卡2
     
        protected void Page_Load(object sender, EventArgs e)
        {
            issueid = Request["issueid"].ConvertTo<int>(0);
            channelid = Request["channelid"].ConvertTo<int>(0);
            actid = Request["actid"].ConvertTo<int>(0);
            cardcode = Request["pno"].ConvertTo<decimal>(0);
            isopencard = Request["isopencard"].ConvertTo<int>(0);
            

        }
    }
}