using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Common.Business;
using System.Collections;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS.Framework;
using ETS2.WeiXin.Service.WeiXinService.Model;

namespace ETS2.WebApp.WeiXin
{
    public partial class MenuList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (UserHelper.ValidateLogin())
            {
                B2b_company company = UserHelper.CurrentCompany;

                //判断是否进行过微信接口设置,没有设置过进入微信接口设置页面
                WeiXinBasic b = new WeiXinBasicData().GetWxBasicByComId(company.ID);
                if (b == null)
                {
                    Response.Redirect("/weixin/ShangJiaSet2.aspx");
                }


                //显示菜单列表
                int pageindex = 1;
                int pagesize = 10;
                int comid = company.ID;
                GetMenuList(pageindex, pagesize, comid);//获得微信菜单
            }
            else
            {
                Response.Redirect("/Manage/index1.html");
            }

        }

        private void GetMenuList(int pageindex, int pagesize, int comid)
        {
            rptTopMenuList.DataSource = null;//清空一级菜单
            rptTopMenuList.DataBind();

            int firsttotalcount = 0;
            var firstlist = new WxMenuData().GetFristMenuList(pageindex, pagesize, comid, out firsttotalcount);

            IEnumerable fristresult = "";
            if (firstlist != null)
            {
                fristresult = from pro in firstlist
                              select new
                              {
                                  MenuId = pro.Menuid,
                                  Name = pro.Name,

                                  FatherMenuName = pro.Fathermenuid == 0 ? "" : new WxMenuData().GetWxMenu(pro.Fathermenuid).Name,
                                  Level = pro.Fathermenuid == 0 ? "一级菜单" : "二级菜单",
                                  MenuOperationType = new WxOperationTypeData().GetOprationType(pro.Operationtypeid).Typename,
                                  MenuOperationTypeId = pro.Operationtypeid,
                                  FirstTotleCount = firsttotalcount
                              };
            }
            rptTopMenuList.DataSource = fristresult;
            rptTopMenuList.DataBind();



            foreach (RepeaterItem item in rptTopMenuList.Items)
            {
                Repeater rptMenuList = item.FindControl("rptMenuList") as Repeater;
                rptMenuList.DataSource = null;
                rptMenuList.DataBind();

                int fatherid = (item.FindControl("HideFuncId") as HiddenField).Value.ConvertTo<int>();

                int secondtotalcount = 0;
                var secondlist = new WxMenuData().GetSecondMenuList(fatherid, comid, out secondtotalcount);
                IEnumerable secondresult = "";
                if (secondlist != null)
                {
                    secondresult = from pro in secondlist
                                   select new
                                   {
                                       MenuId = pro.Menuid,
                                       Name = pro.Name,
                                       FatherMenuId = fatherid,
                                       FatherMenuName = pro.Fathermenuid == 0 ? "" : new WxMenuData().GetWxMenu(pro.Fathermenuid).Name,
                                       Level = pro.Fathermenuid == 0 ? "一级菜单" : "二级菜单",
                                       MenuOperationType = new WxOperationTypeData().GetOprationType(pro.Operationtypeid).Typename,
                                       SecondTotleCount = secondtotalcount
                                   };
                }

                rptMenuList.DataSource = secondresult;
                rptMenuList.DataBind();

            }

        }
    }
}