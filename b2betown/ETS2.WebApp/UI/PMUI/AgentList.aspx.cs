using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.Common.Business;


namespace ETS2.WebApp.UI.PMUI
{
    public partial class AgentList : System.Web.UI.Page
    {
        public string today = DateTime.Now.ToString("yyyy-MM-dd");
        public string yesterday = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
        public string tomonth_star = DateTime.Now.ToString("yyyy") + "-" + DateTime.Now.ToString("MM") + "-1";
        public string yestermonth_star = DateTime.Now.AddMonths(-1).ToString("yyyy") +"-"+ DateTime.Now.AddMonths(-1).ToString("MM") +"-1";
        public string yestermonth_end = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-" + DateTime.Now.ToString("MM") + "-1").AddDays(-1).ToString("yyyy-MM-dd");

        public string month_3= DateTime.Now.AddMonths(-3).ToString("yyyy") + "-" + DateTime.Now.AddMonths(-3).ToString("MM") + "-1";


        public string startime = "";
        public string endtime = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            startime = Request["startime"].ConvertTo<string>("");
            endtime = Request["endtime"].ConvertTo<string>("");


            if (startime == "") {
                startime = month_3;
            }
            if (endtime == "") {

                endtime = today;
            }



        }
    }
}