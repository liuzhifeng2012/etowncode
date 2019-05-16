using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.WeiXin.qrcode2
{
    public partial class qrcodeuserlist : System.Web.UI.Page
    {
        public int subscribesourceid = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            subscribesourceid = Request["subscribesourceid"].ConvertTo<int>(0);
        }
    }
}