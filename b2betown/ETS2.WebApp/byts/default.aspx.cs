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
namespace ETS2.WebApp.byts
{
    public partial class _default : System.Web.UI.Page
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

        public List<MemberShipCardMaterial> mcMaterilList = new List<MemberShipCardMaterial>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AccountId"] != null)
            {
                WxMaterialData Wx = new WxMaterialData();
                WxMaterial wmater = Wx.logGetidinfo(" SalePromoteTypeid !=4 order by operatime  desc ");

                int totalcount = 0;


                if (wmater != null)
                {
                    Listtime = wmater.Operatime.ToString("yyyy-MM-dd");
                }

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

                //会员信息
                B2bCrmData dateuser = new B2bCrmData();
                B2b_crm modeluser = dateuser.Readuser(AccountId, comid);

                if (modeluser != null)
                {
                    AccountWeixin = modeluser.Weixin;
                    AccountEmail = modeluser.Email;
                    Accountphone = modeluser.Phone;
                    Servercard = modeluser.Servercard;
                    Imprest = modeluser.Imprest;
                    Integral = modeluser.Integral;
                    //密码
                    AccountPass = modeluser.Password1;
                    //微信关注
                    weixin = modeluser.Weixin == "" ? "未关注" : "已关注";
                }

                //渠道
                MemberChannelData channeldate = new MemberChannelData();

                //渠道信息
                Member_Channel channelmodel = channeldate.GetSelfChannelDetailByCardNo(AccountCard);
                if (channelmodel != null)
                {
                    channeltype = 1;
                    channelid = channelmodel.Id;

                    RebateConsume = channelmodel.RebateConsume;
                    RebateOpen = channelmodel.RebateOpen;
                    Opencardnum = channelmodel.Opencardnum;
                    Firstdealnum = channelmodel.Firstdealnum;
                    Summoney = channelmodel.Summoney;
                }


                //服务专员信息,服务专员ID
                if (Servercard != 0)
                {
                    Member_Channel channelmode2 = channeldate.GetChannelDetail(Int32.Parse(Servercard.ToString()));
                    if (channelmode2 != null)
                    {
                        Servername = channelmode2.Name;
                        Servermobile = channelmode2.Mobile;
                    }
                }

                ////菜单项new WxSalePromoteTypeData()
                //List<WxSalePromoteType> menulist = new WxSalePromoteTypeData().GetAllWxMaterialType(comid, out totalcount);

                //菜单项new WxSalePromoteTypeData()
                List<WxSalePromoteType> menulist = new WxSalePromoteTypeData().GetRecommendWxMaterialType(comid, out totalcount);

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
                    menu.DataSource = menulist;
                    menu.DataBind();
                }

                //得到微信会员卡说明和使用门店说明
                int ttcount = 0;
                mcMaterilList = new MemberShipCardMaterialData().GetMCMateralListByComId(comid,out ttcount);

            }
            else
            {

                Response.Redirect("/byts/login.aspx");
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

                //if (typeid == 1 || typeid == 2 || typeid == 3 || typeid == 9 || typeid == 23)
                //{
                periodical period = new WxMaterialData().GetPeriodicalBySaleType(comid, typeid);

                List<WxMaterial> list = new WxMaterialData().periodicaltypelist(1, 20, 10, period.Id, typeid, out totalcount);
                //if (totalcount > 0)
                //{
                rep.DataSource = new WxMaterialData().periodicaltypelist(1, 20, 10, period.Id, typeid, out totalcount);
                rep.DataBind();
                //}
                //else
                //{
                //    Label lb = e.Item.FindControl("lblEmpty") as Label;
                //    lb.Text = "无推荐线路";
                //}


                periodnum = period.Percal;
                //}

            }
        }
        public string strsub(string name)
        {
            //string str = "";
            //if (name == "国内推荐" || name == "出境推荐" || name == "温泉滑雪" || name == "每日特惠" || name == "会员活动")
            //{
            //    str = name;
            //}

            //return str;
            return name;
        }

        public string pernum(int typeid, int comid)
        {
            string num = "";
            periodical period = new periodical();
            //if (typeid == 1 || typeid == 2 || typeid == 3 || typeid == 9 || typeid == 23)
            //{
            period = new WxMaterialData().GetPeriodicalBySaleType(comid, typeid);
            num = "第 " + period.Percal.ToString() + "  期";
            //}
            return num;
        }
        public int Int_pernum(int typeid, int comid)
        {
            int num = 0;
            //if (typeid == 1 || typeid == 2 || typeid == 3 || typeid == 9 || typeid == 23)
            //{
            periodical period = new WxMaterialData().GetPeriodicalBySaleType(comid, typeid);
            num = period.Percal;
            //}
            return num;
        }
    }
}