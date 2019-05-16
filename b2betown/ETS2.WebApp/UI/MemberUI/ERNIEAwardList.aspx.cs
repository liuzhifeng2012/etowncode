using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI.MemberUI
{
    public partial class ERNIEAwardList : System.Web.UI.Page
    {
        public int actid=0;
        protected void Page_Load(object sender, EventArgs e)
        {
            actid = Request["actid"].ConvertTo<int>(0);
        }
    }
}