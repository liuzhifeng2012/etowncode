using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI.MemberUI
{
    public partial class PromotionEdit : System.Web.UI.Page
    {
        public string nowdate = "";//现在日期
        public string monthdate = "";//现在日期+一个月
        public int actid = 0;//活动ID
        public int acttype = 0;
        public int faceObjects = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            nowdate = DateTime.Now.ToString("yyyy-MM-dd");
            monthdate = DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd");
            actid = Request["actid"].ConvertTo<int>(0);
            acttype = Request["acttype"].ConvertTo<int>(0);
            faceObjects = Request["faceObjects"].ConvertTo<int>(0);
        }
    }
}