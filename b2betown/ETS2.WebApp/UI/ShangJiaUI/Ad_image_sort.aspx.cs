using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.Common.Business;

namespace ETS2.WebApp.UI.ShangJiaUI
{
    public partial class Ad_image_sort : System.Web.UI.Page
    {
        public int adid = 0;
        public int comid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            adid=Request["adid"].ConvertTo<int>(0);

             comid = UserHelper.CurrentCompany.ID;

        }
    }
}