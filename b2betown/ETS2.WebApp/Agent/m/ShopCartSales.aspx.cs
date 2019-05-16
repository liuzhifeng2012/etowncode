using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.PM.Service.PMService.Data;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.WebApp.Agent.m
{
    public partial class ShopCartSales : System.Web.UI.Page
    {
        public int comid_temp = 0;
        public int Agentid = 0;
        public string proid = "";
        public string num = "";

        public int addrid = 0;//常用地址id,只有实物产品用到 
        protected void Page_Load(object sender, EventArgs e)
        {
            proid = Request["proid"].ConvertTo<string>("");
            Agentid = Request["agentid"].ConvertTo<int>(0);
            comid_temp = Request["comid"].ConvertTo<int>(0);
            addrid = Request["addrid"].ConvertTo<int>(0);


            //去除最右侧，号
            if (proid != "")
            {
                if (proid.Substring(proid.Length - 1, 1) == ",")
                {
                    proid = proid.Substring(0, proid.Length - 1);
                }
            }

            var proid_arr = proid.Split(',');
            int id = int.Parse(proid_arr[0]);//得到第一个ID，读取基本的COMID,及公司信息

             
            B2bComProData prodate = new B2bComProData();
            var proinfo = prodate.GetProById(id.ToString());
            if (proinfo != null)
            {
                comid_temp = proinfo.Com_id;
                 
                //作废超时未支付订单，完成回滚操作
                int rs = new B2bComProData().CancelOvertimeOrder(proinfo);
            }


            //获取每个产品数量
            var list = new B2bOrderData().SearchCartList(comid_temp, Agentid, proid);
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    num += list[i].U_num + ",";
                }
            }
            //去除最右侧，号
            if (num != "")
            {
                if (num.Substring(num.Length - 1, 1) == ",")
                {
                    num = num.Substring(0, num.Length - 1);
                }
            }

        }
    }
}