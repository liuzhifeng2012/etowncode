using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.PM.Service.PMService.Data;

namespace ETS2.WebApp.UI.PMUI
{
    public partial class PackageBindingpro : System.Web.UI.Page
    {
        public int id = 0;
        public string fproname = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            id = Request["id"].ConvertTo<int>(0);

            var ProData = new B2bComProData();
            var proinfo = ProData.GetProById(id.ToString());

            if (proinfo != null) {
                fproname = proinfo.Pro_name;
            }

        }
    }
}