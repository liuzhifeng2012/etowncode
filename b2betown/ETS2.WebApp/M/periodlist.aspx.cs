using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.WeiXin.Service.WeiXinService.Model;
using System.Data;

namespace ETS2.WebApp.M
{
    public partial class periodlist : System.Web.UI.Page
    {
        public int promotetypeid = 1;//默认是显示国内推荐
        public string promotetype = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            var totalcount = 0;
            promotetypeid = Request["promotetypeid"].ConvertTo<int>(1);
            List<periodical> list = new WxMaterialData().periodicalList(1, 20, 10, promotetypeid, out totalcount);

            Repeater1.DataSource = list;
            Repeater1.DataBind();

            promotetype = new WxSalePromoteTypeData().GetWxMenu(promotetypeid).Typename;
        }
        
    }
}