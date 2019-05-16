using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI
{
    public partial class periodical : System.Web.UI.Page
    {
        public int promotetypeid = 1;//默认是显示国内推荐
      
        protected void Page_Load(object sender, EventArgs e)
        {
            promotetypeid = Request["promotetypeid"].ConvertTo<int>(1);
        }
    }
}