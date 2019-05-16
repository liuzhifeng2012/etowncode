using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.Agent.m
{
    public partial class CommonAddrEdit : System.Web.UI.Page
    {
        public int addrid = 0;//地址id
        public string proid = "";//产品id
        public int unum = 1;////订购产品数量,只有实物产品用到

        public int isshopcart = 0;//是否是购物车常用地址
        protected void Page_Load(object sender, EventArgs e)
        {
            addrid = Request["addrid"].ConvertTo<int>(0);
            proid = Request["proid"].ConvertTo<string>("");
            unum = Request["unum"].ConvertTo<int>(1);
            isshopcart = Request["isshopcart"].ConvertTo<int>(0);
        }
    }
}