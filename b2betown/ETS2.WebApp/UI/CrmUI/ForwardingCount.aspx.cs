using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI.CrmUI
{
	public partial class ForwardingCount : System.Web.UI.Page
	{
        public int wxid = 0;
		protected void Page_Load(object sender, EventArgs e)
		{
            wxid = Request["wxid"].ConvertTo<int>(0);
		}
	}
}