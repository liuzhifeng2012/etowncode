using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS.Framework;
namespace ETS2.WebApp.UI.PMUI
{
    public partial class LineGroupDate : System.Web.UI.Page
    {
        public int lineid = 0;//线路id

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lineid = Request["lineid"].ConvertTo<int>(0);
                if (lineid == 0)
                {
                    Response.Redirect("/ui/pmui/productlist.aspx");
                }

                List<B2b_com_LineGroupDate> list = new B2b_com_LineGroupDateData().GetLineGroupDateByLineid(lineid);
                if (list != null && list.Count > 0)
                {
                    var date = from r in list
                               select r.Daydate.ToString("yyyy-MM-dd");
                    hidLeavingDate.Value = string.Join(",", date.ToList());
                    hidinitLeavingDate.Value = string.Join(",", date.ToList());


                }
            }



        }
    }
}