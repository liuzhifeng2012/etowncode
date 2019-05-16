using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle.Enum;

namespace ETS2.WebApp.UI.UserUI
{
    public partial class bangdingpos_add : System.Web.UI.Page
    {
        public int pos_id = 0;//绑定POS自动编号ID

        public string md5key = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            pos_id = Request["pos_id"].ConvertTo<int>(0);

            if (pos_id == 0)
            {
                md5key = RandomHelper.RandCode(24);
            }

        }
    }
}