using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI.PMUI
{
    public partial class BindingAgentPorList : System.Web.UI.Page
    {       
        
        
        public int wacomid = 0;//产品授权商户编号

        protected void Page_Load(object sender, EventArgs e)
        {
            wacomid = Request["comid"].ConvertTo<int>(0);
        }
    }
}