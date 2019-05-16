using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI.CrmUI
{
    public partial class Member_group_edit : System.Web.UI.Page
    {
        public int groupid = 0;//分组id
        protected void Page_Load(object sender, EventArgs e)
        {
            groupid = Request["groupid"].ConvertTo<int>(0);
        }
    }
}