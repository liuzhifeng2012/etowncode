using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI.PMUI.Order
{
    public partial class OrderFinSet : System.Web.UI.Page
    {

        public string startdate = "";//当前前一个月日期
        public string enddate = "";//当前日期


        public string yesterdayDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");//昨天日期

        public string weekfirstdate = "";//本星期第一天
        public string weekenddate = "";//本星期最后一天

        public string lastweekfirstdate = "";//上周第一天
        public string lastweekenddate = "";//上周最后一天

        public string monthfirstdate = "";//本月第一天
        public string monthenddate = "";//本月最后一天

        public string lastmonthfirstdate = "";//上个月第一天
        public string lastmonthenddate = "";//上个月最后一天

        protected void Page_Load(object sender, EventArgs e)
        {
            startdate = Request["startdate"].ConvertTo<string>(DateTime.Now.ToString("yyyy-MM-dd"));
            enddate = Request["enddate"].ConvertTo<string>(DateTime.Now.ToString("yyyy-MM-dd"));


            DateTime dt = DateTime.Now;
            int weeknow = Convert.ToInt32(DateTime.Now.DayOfWeek);
            int dayspan = (-1) * weeknow + 1;
            DateTime dt2 = dt.AddMonths(1);
            //获取本周第一天
            weekfirstdate = DateTime.Now.AddDays(dayspan).ToString("yyyy-MM-dd");
            weekenddate = DateTime.Now.AddDays(dayspan).AddDays(6).ToString("yyyy-MM-dd");


            //本月第一天
            monthfirstdate = dt.AddDays(-(dt.Day) + 1).ToString("yyyy-MM-dd");
            //本月最后一天
            monthenddate = dt2.AddDays(-dt.Day).ToString("yyyy-MM-dd");
            //上个月第一天
            lastmonthfirstdate = dt.AddMonths(-1).AddDays(-dt.Day + 1).ToString("yyyy-MM-dd");
            //上个月最后一天
            lastmonthenddate = dt.AddDays(-dt.Day).ToString("yyyy-MM-dd");
            //上周第一天
            lastweekfirstdate = DateTime.Parse(weekfirstdate).AddDays(-7).ToString("yyyy-MM-dd");
            //上周最后一天
            lastweekenddate = DateTime.Parse(weekfirstdate).AddDays(-1).ToString("yyyy-MM-dd");


        }
    }
}