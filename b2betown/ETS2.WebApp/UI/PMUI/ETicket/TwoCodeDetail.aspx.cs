using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.PM.Service.PMService.Data;

using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;
using ThoughtWorks.QRCode.Codec.Util;
using System.IO;
using System.Drawing;
namespace ETS2.WebApp.UI.PMUI.ETicket
{
    public partial class TwoCodeDetail : System.Web.UI.Page
    {
        public string pno = "";//电子码
        protected void Page_Load(object sender, EventArgs e)
        {
            pno = Request["pno"].ConvertTo<string>("");
        }

    }
}