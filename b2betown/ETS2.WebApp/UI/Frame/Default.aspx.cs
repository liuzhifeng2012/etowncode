using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle.Enum;


namespace ETS2.WebApp.UI.Frame
{
    public partial class Default : System.Web.UI.Page
    {
        public string proname = "";
        public int comid = 0;
        public int lineid = 0;
        public DateTime today = DateTime.Today;
        public string pro_Remark = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            lineid = Request["lineid"].ConvertTo<int>(0);
            B2b_com_pro pro = new B2bComProData().GetProById(lineid.ToString());
            if (pro != null)
            {
                proname = pro.Pro_name;
                pro_Remark = pro.Pro_Remark;
            }

            List<B2b_com_LineGroupDate> list = new B2b_com_LineGroupDateData().GetLineGroupDateByLineid(lineid);
            if (list != null && list.Count > 0)
            {
                var date = from r in list
                           select r.Daydate.ToString("yyyy-MM-dd");
                hidLeavingDate.Value = string.Join(",", date.ToList());

                hidMinLeavingDate.Value = string.Join(",", date.ToList()).Split(',')[0];


                var price = from p in list
                            select p.Menprice;
                hidLinePrice.Value = string.Join(",", price.ToList());

            }


        }
    }
}