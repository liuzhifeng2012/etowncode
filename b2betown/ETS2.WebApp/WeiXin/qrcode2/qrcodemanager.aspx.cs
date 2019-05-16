using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.WeiXin.qrcode2
{
    public partial class qrcodemanager : System.Web.UI.Page
    {

        public int qrcodeid = 0;//二维码id
        protected void Page_Load(object sender, EventArgs e)
        {
            qrcodeid = Request["qrcodeid"].ConvertTo<int>(0);

        }

    }
}