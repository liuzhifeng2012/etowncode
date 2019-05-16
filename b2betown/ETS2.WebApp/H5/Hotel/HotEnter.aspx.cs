using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.Common.Business;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS.Framework;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;

namespace ETS2.WebApp.H5
{
    public partial class HotEnter : System.Web.UI.Page
    {
        public string checkindate = "";//入住日期
        public string checkoutdate = "";//离店日期
        public int bookdaynum = 0;//入住天数

        public int proid = 0;//产品(房型)id
        public int comid = 0;//公司id

        public int uid = 0;//会员id

        public Decimal Imprest = 0;//预付款
        public Decimal Integral = 0;//积分

        public string openid = "";//微信号
        public string ordertype = "1";//订单类型 默认为1订单；2充值
        public decimal singleroom_totalprice = 0;//商品建议价格(注:针对酒店则是 单个房间总的价格)

        public int buyuid = 0; //购买用户ID
        public int tocomid = 0;//来访商户COMID

        public string fangtai = "";//房态

        public string projectname = "";//项目名称
        public string projectimgurl = "";//项目图片

        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime checkindate1 = Request["indate"].ConvertTo<DateTime>();
            checkindate = checkindate1.ToString("yyyy-MM-dd");
            DateTime checkoutdate1 = Request["outdate"].ConvertTo<DateTime>();
            checkoutdate = checkoutdate1.ToString("yyyy-MM-dd");
            bookdaynum = (checkoutdate1 - checkindate1).Days;

            buyuid = Request["buyuid"].ConvertTo<int>(0);
            tocomid = Request["tocomid"].ConvertTo<int>(0);
          

            proid = Request["id"].ConvertTo<int>(0);
            comid = Request["comid"].ConvertTo<int>(0);

            bookdaynum = (checkoutdate1 - checkindate1).Days;

            uid = Request["uid"].ConvertTo<int>(0);

            //获得房态信息
            List<B2b_com_LineGroupDate> list = new B2b_com_LineGroupDateData().GetLineDayGroupDate(checkindate, checkoutdate, proid);
            if (list.Count > 0)
            {
                foreach (B2b_com_LineGroupDate m in list)
                {
                    singleroom_totalprice += m.Menprice;
                    fangtai += m.Menprice + ",";
                }
                if (fangtai.Length > 0)
                {
                    fangtai = fangtai.Substring(0, fangtai.Length - 1);
                }
            }

            if (proid != 0)
            {
                var prodata = new B2bComProData().GetProById(proid.ToString());
                if (prodata != null)
                {
                    comid = prodata.Com_id;
                    B2b_com_project mod = new B2b_com_projectData().GetProject( prodata.Projectid, comid);
                    if (mod != null)
                    {
                        projectimgurl = FileSerivce.GetImgUrl(mod.Projectimg);
                        projectname = mod.Projectname;
                    }

                }
            }



            //从cookie中得到微信号
            if (Request.Cookies["openid"] != null)
            {
                openid = Request.Cookies["openid"].Value;
            }
            B2bCrmData b2b_crm = new B2bCrmData();
            if (openid != "")
            {
                B2b_crm b2bmodle = b2b_crm.b2b_crmH5(openid, comid);
                if (b2bmodle != null)
                {
                    Imprest = b2bmodle.Imprest;
                    Integral = b2bmodle.Integral;
                }
            }
      
           
        }
    }
}