using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.Member.Service.MemberService.Data;
namespace ETS2.WebApp.UI.MemberUI
{
    public partial class ERNIEActEdit : System.Web.UI.Page
    {
        public string nowdate = "";//现在日期
        public int actid = 0;//活动ID
        public int Online = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            nowdate = DateTime.Now.ToString("yyyy-MM-dd");
            actid = Request["actid"].ConvertTo<int>(0);

            var erniedata = MemberERNIEData.ERNIEGetActById(actid);
            if (erniedata != null) {
                Online = erniedata.Online;
                
            }
        }
    }
}