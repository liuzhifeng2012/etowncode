using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        public int note_id=0;
        protected void Page_Load(object sender, EventArgs e)
        {
            note_id = Request["note_id"].ConvertTo<int>(0);
        }
    }
}