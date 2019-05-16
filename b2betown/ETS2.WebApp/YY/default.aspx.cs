using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.Common.Business;

namespace ETS2.WebApp.YY
{
    public partial class _default : System.Web.UI.Page
    {
        #region 辨别商家参数
        public string RequestUrl = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Trim().ToLower();
        public int comid = 0;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (comid == 0)
            {
                if (Domain_def.Domain_yanzheng(RequestUrl))//如果符合shop101.etown.cn的格式，则从多微信商户基本信息表中获取comid
                {
                    comid = Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl));
                }
            }
        }
    }
}