using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI.PermissionUI
{
    public partial class MasterList : System.Web.UI.Page
    {
        public string groupid = "0";
        public string groupname = "";

        public int childcomid = 0;//子公司id，得到其公司的员工列表
        //public string oper1 = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            groupid = Request["groupid"].ConvertTo<string>("0");
            groupname = Request["groupname"].ConvertTo<string>("");
            //oper1 = Request["oper1"].ConvertTo<string>("");

            childcomid = Request["childcomid"].ConvertTo<int>(0);
        }
    }
}