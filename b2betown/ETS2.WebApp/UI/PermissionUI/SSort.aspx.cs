using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI
{
    public partial class SSort : System.Web.UI.Page
    {
        public int comstate = 1;//公司运营状态：1，开通；2，暂停
        protected void Page_Load(object sender, EventArgs e)
        {
            comstate = Request["comstate"].ConvertTo<int>(1);
        }
    }
}