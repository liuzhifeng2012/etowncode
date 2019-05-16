using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.Common.Business;

namespace ETS2.WebApp.yanzheng
{
    public partial class loginout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UserHelper.Logout("/yanzheng");//如果关闭不跳转 则 按执行下面关闭
        }
    }
}