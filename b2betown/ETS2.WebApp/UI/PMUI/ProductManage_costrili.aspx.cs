using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.WebApp.UI.PMUI
{
    public partial class ProductManage_costrili : System.Web.UI.Page
    {
        public int proid = 0;
        public int manyspeci = 0;
        public string proname = "";
        public List<B2b_com_pro_Speci> gglist = null;//规格列表


        protected void Page_Load(object sender, EventArgs e)
        {
            proid = Request["proid"].ConvertTo<int>(0);
            var prodata = new B2bComProData();
            if (proid != 0) {
                var proinfo = prodata.GetProById(proid.ToString());
                if (proinfo != null) {
                    proname = proinfo.Pro_name;
                    manyspeci = proinfo.Manyspeci;
                    gglist = new B2b_com_pro_SpeciData().Getgglist(proinfo.Id);

                }
            
            }

        }
    }
}