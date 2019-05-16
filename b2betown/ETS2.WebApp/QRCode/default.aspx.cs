using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.QRCode
{
    public partial class _default : System.Web.UI.Page
    {
        public string pno = "";//电子码
        protected void Page_Load(object sender, EventArgs e)
        {
            pno = Request["pno"].ConvertTo<string>(pno);

            if (pno != "")
            {
                pno = EncryptionHelper.Decrypt(pno, "lixh1210");
            }
        }
    }
}