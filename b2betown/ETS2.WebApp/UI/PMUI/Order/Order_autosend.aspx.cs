using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.VAS.Service.VASService.Data;

namespace ETS2.WebApp.UI.PMUI.Order
{
    public partial class Order_autosend : System.Web.UI.Page
    {
        public int order_no = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            order_no = Int32.Parse(Request["id"]);
            string retunstr = new PayReturnSendEticketData().ShougongReturnSendEticket(order_no);
        }
    }
}