using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.PM.Service.PMService.Data;

namespace ETS2.WebApp.UI.ShangJiaUI
{
	public partial class H5SetStep : System.Web.UI.Page
	{
        #region 辨别商家参数
        public string RequestUrl = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Trim().ToLower();
        public int comid = 0;
        #endregion

        public int Modelid = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            comid = Request["comid"].ConvertTo<int>(0);

            B2bModelData mdate = new B2bModelData();
            var h5model = mdate.SelectModelSearchComid(comid);
            if (h5model != null)
            {
                Modelid = h5model.Modelid;
            }

        }
	}
}