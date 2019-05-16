using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI.MemberUI
{
    public partial class CardEdit : System.Web.UI.Page
    {
        public int cardid = 0;//活动ID
        public int CardRule_Second = 0;
        public int CardRule_First = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            cardid = Request["id"].ConvertTo<int>(0);
            CardRule_Second = Int16.Parse(DateTime.Now.ToString("yyMM"));
            CardRule_First = 5001;
        }
    }
}