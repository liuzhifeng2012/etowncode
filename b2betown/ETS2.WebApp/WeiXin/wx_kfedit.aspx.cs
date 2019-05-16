using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.WeiXin
{
    public partial class wx_kfedit : System.Web.UI.Page
    {
        public int kfid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            kfid = Request["kfid"].ConvertTo<int>(0);
            if(kfid==0)
            {
                Response.Redirect("wx_kflist.aspx");
            }
        }
    }
}