using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.PM.Service.PMService.Modle;
using ETS2.Common.Business;
using ETS2.PM.Service.PMService.Data;
using ETS.Framework;

namespace ETS2.WebApp.UI.PMUI.delivery
{
    public partial class deliverymanage : System.Web.UI.Page
    {
        public int tmpid = 0;//模板id
        protected void Page_Load(object sender, EventArgs e)
        {
            tmpid = Request["tmpid"].ConvertTo<int>(0);
            //if(tmpid==0)
            //{
            //    Response.Redirect("/ui/pmui/delivery/deliverylist.aspx");
            //}
        }
    }
}