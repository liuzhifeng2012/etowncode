using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;

namespace ETS2.WebApp.UI.MemberUI
{
    public partial class mTravelbusOrderVisitorlist : System.Web.UI.Page
    {
        public int proid = 0;
        public string daydate = "";
        public string oper = "";

        public int servertype = (int)ProductServer_Type.LvyouBus;//服务类型
        public int orderstate_paysuc = (int)OrderStatus.HasFin;//订单类型：处理成功
        public int paystate_haspay = (int)PayStatus.HasPay;//支付类型:已经支付

        public string pickuppoint = "";//上车地点
        public string dropoffpoint = "";//下车地点
        public decimal advise_price = 0;//建议价格
        public int emptynum = 0;//当天空位数量
        public int comid = 0;
        public string firststationtime = "";//发车时间

        protected void Page_Load(object sender, EventArgs e)
        {
            proid = Context.Request["proid"].ConvertTo<int>(0);
            daydate = Context.Request["daydate"].ConvertTo<string>("");
            oper = Context.Request["oper"].ConvertTo<string>("");

            B2b_com_pro pro = new B2bComProData().GetProById(proid.ToString());
            if (pro != null)
            {
                // 作废超时未支付订单，完成回滚操作
                int rs = new B2bComProData().CancelOvertimeOrder(pro);

                pickuppoint = pro.pickuppoint;
                dropoffpoint = pro.dropoffpoint;
                advise_price = pro.Advise_price;
                emptynum = new B2b_com_LineGroupDateData().GetEmptyNum(proid,DateTime.Parse(daydate));
                comid = pro.Com_id;
                firststationtime = pro.firststationtime;
            }
        }
    }
}