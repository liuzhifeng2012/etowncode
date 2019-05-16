using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
namespace ETS2.WebApp.UI.PermissionUI
{
    public partial class ModelMenuSort : System.Web.UI.Page
    {
        public int modelid = 0;//模板id
        protected void Page_Load(object sender, EventArgs e)
        {
            modelid = Request["modelid"].ConvertTo<int>(0);
        }
    }
}