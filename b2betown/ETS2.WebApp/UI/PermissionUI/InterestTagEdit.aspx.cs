using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI.PermissionUI
{
    public partial class InterestTagEdit : System.Web.UI.Page
    {
        public int tagid = 0;
        public int tagtypeid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            tagid = Request["tagid"].ConvertTo<int>(0);
            tagtypeid = Request["tagtypeid"].ConvertTo<int>(0);
        }
    }
}