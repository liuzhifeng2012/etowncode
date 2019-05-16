using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.Common.Business;
using ETS2.PM.Service.PMService.Data;


namespace ETS2.WebApp.UI.ShangJiaUI
{
    public partial class consultant_pro : System.Web.UI.Page
    {
        public int Modelid = 0;
        public int Daohangimg = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            var comid = UserHelper.CurrentCompany.ID;
            B2bModelData mdate = new B2bModelData();
            //判断模板，进行跳转
            var h5model = mdate.SelectModelSearchComid(comid);
            if (h5model != null)
            {
                Modelid = h5model.Modelid;
            }


            if (Modelid != 0)
            {
                var modelinfo = mdate.GetModelById(Modelid);
                if (modelinfo != null)
                {
                    Daohangimg = modelinfo.Daohangimg;
                }
            }
        }
    }
}