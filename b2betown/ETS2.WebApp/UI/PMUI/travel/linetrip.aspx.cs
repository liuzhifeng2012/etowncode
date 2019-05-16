using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;

namespace ETS2.WebApp.UI.PMUI.travel
{
    public partial class linetrip : System.Web.UI.Page
    {
        public int CurrentTourProductId = 0;//线路id
        public string ControlLoadInfor = string.Empty;//前台脚本调用
        public string EditOrView { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
                CurrentTourProductId = Request["lineid"].ConvertTo<int>(0);

                if (CurrentTourProductId == 0)
                {

                    ControlLoadInfor = "[{Id:0}]";
                }
                else
                {

                    List<B2b_com_protrip> tourList = new B2b_com_protripData().Gettriplistbylineid(CurrentTourProductId);
                    if (tourList != null && tourList.Count() > 0)
                    {
                        var query = from d in tourList
                                    select "{Id:'" + d.Id + "'}";

                        ControlLoadInfor = "[" + String.Join(",", query.ToArray()) + "]";
                    }
                    else
                    {
                        ControlLoadInfor = "[{Id:0}]";
                    }
                }
            }

        }
    }
}