using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI.PMUI.ETicket
{
    public partial class TwoCodeList : System.Web.UI.Page
    {
        public int uid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            uid = Request["uid"].ConvertTo<int>(0);
        }
    }
}