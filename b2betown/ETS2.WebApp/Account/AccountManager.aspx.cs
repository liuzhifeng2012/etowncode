using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.Common.Business;

namespace ETS2.WebApp.Account
{
    public partial class AccountManager : System.Web.UI.Page
    {
        public int staffid = 0;//编辑员工的id
        protected void Page_Load(object sender, EventArgs e)
        {
            staffid = Request["staffid"].ConvertTo<int>(UserHelper.CurrentUserId());
        }
    }
}