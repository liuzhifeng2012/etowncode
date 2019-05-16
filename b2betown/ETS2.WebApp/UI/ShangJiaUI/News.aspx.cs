using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Member.Service.MemberService.Data;
using ETS2.Member.Service.MemberService.Model;
using ETS.Framework;
using ETS2.Common.Business;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.WeiXin.Service.WeiXinService.Model;
using System.Data;

namespace ETS2.WebApp.UI.ShangJiaUI
{
    public partial class News : System.Web.UI.Page
    {

        public int AccountId = 0;
        public string AccountName = "";
        public string AccountCard = "";
        public string AccountEmail = "";
        public string AccountWeixin = "";
        public string Accountphone = "";
        public decimal Imprest = 0;
        public decimal Integral = 0;
        public string AccountPass = "";

        public string RequestUrl = "";//访问网址通过访问网址判断商家，如果访问网址为空则跳转到登陆页
        public int comid = 0;

        public int channeltype = 0;//0为用户 1为渠道商
        public int channelid = 0;//0为用户 1为渠道商


        public decimal RebateConsume = 0;
        public decimal RebateOpen = 0;
        public int Opencardnum = 0;
        public int Firstdealnum = 0;
        public decimal Summoney = 0;

        public decimal Servercard = 0;
        public string Servername = "";//服务专员姓名
        public string Servermobile = "";//服务专员手机

        public string Listtime;

        public string weixin = "";

        public int typeid = 0;

        public int periodnum = 0;
        public string Com_name ="";
        public string Scenic_name = "";
        public string Scenic_intro ="";



        protected void Page_Load(object sender, EventArgs e)
        {
            WxMaterialData Wx = new WxMaterialData();
            WxMaterial wmater = Wx.logGetidinfo(" SalePromoteTypeid !=4 order by operatime  desc ");

            int totalcount = 0;


            if (wmater != null)
            {
                Listtime = wmater.Operatime.ToString("yyyy-MM-dd");
            }
            RequestUrl = Request.ServerVariables["SERVER_NAME"].ToString();
            comid = Request["comid"].ConvertTo<int>(0);
            B2b_company_info companyinfo = B2bCompanyData.GetComId(RequestUrl);
            if (companyinfo != null)
            {
                comid = companyinfo.Com_id;
            }
            else
            { //判定是否为自助域名规则安 shop1.etown.cn
                if (Domain_def.Domain_yanzheng(RequestUrl))
                {
                    comid = Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl));
                }
            }

            if (comid != 0) { 
                  B2b_company com = B2bCompanyData.GetAllComMsg(comid);

                  if (com != null)
                  {
                      Com_name = com.Com_name;
                      Scenic_name = com.Scenic_name;
                      Scenic_intro = com.B2bcompanyinfo.Scenic_intro;
                  }
            
            }



             //菜单项new WxSalePromoteTypeData()
             List<WxSalePromoteType> menulist = new WxSalePromoteTypeData().GetAllWxMaterialType(comid, out totalcount);

                if (comid == 101)
                {
                    List<WxSalePromoteType> list = new List<WxSalePromoteType>();
                    int[] i = { 0, 1, 2, 5, 12 };
                    foreach (int s in i)
                    {
                        WxSalePromoteType wxmaterial = new WxSalePromoteType();

                        wxmaterial.Id = menulist[s].Id;
                        wxmaterial.Typename = menulist[s].Typename;
                        list.Add(wxmaterial);
                    }

                    menu.DataSource = list;
                    menu.DataBind();
                }
                else
                {
                    List<WxSalePromoteType> list = new List<WxSalePromoteType>();
                    //int[] i = { 0, 1, 2, 5, 12 };
                    for (int s = 0; s < menulist.Count; s++)
                    {
                        WxSalePromoteType wxmaterial = new WxSalePromoteType();

                        wxmaterial.Id = menulist[s].Id;
                        wxmaterial.Typename = menulist[s].Typename;
                        
                        var period = new WxMaterialData().GetPeriodicalBySaleType(comid, menulist[s].Id);
                        if (period != null)
                        {

                            list.Add(wxmaterial);
                        }
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
                    var period = new WxMaterialData().GetPeriodicalBySaleType(comid, typeid);
                    if (period != null)
                    {
                        rep.DataSource = new WxMaterialData().periodicaltypelist(1, 20, 10, period.Id, typeid, out totalcount);
                        rep.DataBind();

                        periodnum = period.Percal;
                    }
            }
        }

        public string pernum(int typeid, int comid)
        {
            string num = "";
 
            if (typeid == 1 || typeid == 2 || typeid == 3 || typeid == 9 || typeid == 23)
            {
              var  period = new WxMaterialData().GetPeriodicalBySaleType(comid, typeid);
                num = "第 " + period.Percal.ToString() + "  期";
            }
            return num;
        }
        public int Int_pernum(int typeid, int comid)
        {
            int num = 0;
            if (typeid == 1 || typeid == 2 || typeid == 3 || typeid == 9 || typeid == 23)
            {
                var period = new WxMaterialData().GetPeriodicalBySaleType(comid, typeid);
                num = period.Percal;
            }
            return num;
        }
    }
}