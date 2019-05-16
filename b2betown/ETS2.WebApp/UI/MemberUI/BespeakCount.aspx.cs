using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS.Framework;

namespace ETS2.WebApp.UI.MemberUI
{
    public partial class BespeakCount : System.Web.UI.Page
    {
        public string nowdate = "";
        public string monthdate = "";

        //预约类型：0提单同时预约；1自助预约
         //预约状态:0未处理；1预约成功；2预约失败
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime gooutdate = Request["gooutdate"].ConvertTo<DateTime>(DateTime.Now);
            nowdate = gooutdate.ToString("yyyy-MM-dd");
            monthdate = DateTime.Parse(nowdate).AddDays(6).ToString("yyyy-MM-dd");
        }
    }
}