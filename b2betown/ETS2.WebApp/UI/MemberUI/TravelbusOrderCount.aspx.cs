using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS.Framework;

namespace ETS2.WebApp.UI.MemberUI
{
    public partial class TravelbusOrderCount : System.Web.UI.Page
    {
        public string nowdate = "";
        public string monthdate = "";

        public int servertype = 0;//服务类型
        public int orderstate_paysuc = (int)OrderStatus.HasFin;//订单类型：处理成功
        public int paystate_haspay = (int)PayStatus.HasPay;//支付类型:已经支付
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime gooutdate = Request["gooutdate"].ConvertTo<DateTime>(DateTime.Now);
            nowdate = gooutdate.ToString("yyyy-MM-dd");
            monthdate = DateTime.Parse(nowdate).AddDays(6).ToString("yyyy-MM-dd");

            servertype = (int)ProductServer_Type.LvyouBus;
             
        }
    }
}