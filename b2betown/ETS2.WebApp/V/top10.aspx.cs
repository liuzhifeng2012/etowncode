using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.JsonFactory;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.WeiXin.Service.WeiXinService.Model;

namespace ETS2.WebApp.V
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public int AccountId = 0;
        public string AccountName = "";
        public string AccountCard = "";
        public string RequestUrl = "";//访问网址通过访问网址判断商家，如果访问网址为空则跳转到登陆页
        public int comid = 0;
        public int typeid = 0;

        public int periodnum = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            int totalcount = 0;
            if (Session["AccountId"] != null)
            {
                //账户信息
                AccountId = Int32.Parse(Session["AccountId"].ToString());
                AccountName = Session["AccountName"].ToString();
                AccountCard = Session["AccountCard"].ToString();
                RequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();

                B2b_company_info companyinfo = B2bCompanyData.GetComId(RequestUrl);
                if (companyinfo != null)
                {
                    comid = companyinfo.Com_id;
                }

                //菜单项new WxSalePromoteTypeData()
                List<WxSalePromoteType> menulist = new WxSalePromoteTypeData().GetAllWxMaterialType(comid, out totalcount);
                menu.DataSource = menulist;
                menu.DataBind();

            }
            else {
                comid = 101;
                //菜单项new WxSalePromoteTypeData()
                List<WxSalePromoteType> menulist = new WxSalePromoteTypeData().GetAllWxMaterialType(comid, out totalcount);

                List<WxSalePromoteType> list = new List<WxSalePromoteType>();
                int[] i={0,1,2,5};
                foreach(int s in i){
                    WxSalePromoteType wxmaterial = new WxSalePromoteType();

                    wxmaterial.Id = menulist[s].Id;
                    wxmaterial.Typename = menulist[s].Typename;
                    list.Add(wxmaterial);
                }

                menu.DataSource = list;
                menu.DataBind();

            }
        }

        protected void menu_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int totalcount = 0;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rep = e.Item.FindControl("Rplist") as Repeater;//找到里层的repeater对象
                WxSalePromoteType rowv = (WxSalePromoteType)e.Item.DataItem;//找到分类Repeater关联的数据项 
                typeid = Convert.ToInt32(rowv.Id); //获取填充子类的id 

                if (typeid == 1 || typeid == 2 || typeid == 3|| typeid == 9)
                {
                    periodical period = new WxMaterialData().GetPeriodicalBySaleType(comid, typeid);

                    rep.DataSource = new WxMaterialData().periodicaltypelist(1, 20, 10, period.Id, typeid, out totalcount);
                    rep.DataBind();

                    periodnum = period.Percal;
                }

            }
        }
        public string strsub(string name)
        {
            string str = "";
            if (name == "国内推荐" || name == "出境推荐" || name == "温泉滑雪" || name == "每日特惠") 
            {
                str = name;
            }

            return str;
        }

        public string pernum(int typeid, int comid)
        {
            string num = "";
            periodical period = new periodical();
            if (typeid == 1 || typeid == 2 || typeid == 3|| typeid == 9)
            {
                period = new WxMaterialData().GetPeriodicalBySaleType(comid, typeid);
                num = "第 " + period.Percal.ToString() + "  期";
            }
            return num;
        }
        public int Int_pernum(int typeid, int comid)
        {
            int num = 0;
            if (typeid == 1 || typeid == 2 || typeid == 3 || typeid == 9)
            {
                periodical period = new WxMaterialData().GetPeriodicalBySaleType(comid, typeid);
                num = period.Percal;
            }
            return num;
        }

    }
}