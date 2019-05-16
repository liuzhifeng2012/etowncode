using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using ETS2.CRM.Service.CRMService.Data;
using ETS.Framework;

namespace ETS2.WebApp.UI.PermissionUI
{
    public partial class interesttagtypelist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetIndustryList();//获得公司行业列表

        }
        private void GetIndustryList()
        {
        
            rptTopMenuList.DataSource = null;//清空一级菜单
            rptTopMenuList.DataBind();

            int firsttotal = 0;
            var firstlist = new B2b_companyindustryData().GetIndustryList(out firsttotal);

            IEnumerable fristresult = "";
            if (firstlist != null)
            {
                fristresult = from pro in firstlist
                              select new
                              {
                                  MenuId = pro.Id,
                                  Name = pro.Industryname,
                                  Remark = pro.Remark,
                                  Class="公司行业",
                                  FirstTotleCount = firsttotal
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

                int secondtotal = 0;
                var secondlist = new B2b_interesttagtypeData().GetTagTypeList(fatherid, out secondtotal);
                IEnumerable secondresult = "";
                if (secondlist != null)
                {
                    secondresult = from pro in secondlist
                                   select new
                                   {
                                       MenuId = pro.Id,
                                       Name = pro.Typename,
                                       Remark = pro.Remark,
                                       Createtime = pro.Createtime,
                                       Industryid = pro.Industryid,
                                       Isselfdefined = pro.Isselfdefined,
                                       SecondTotleCount = secondtotal,
                                       Class="兴趣类型",
                                       FatherMenuId = fatherid
                                   };
                }

                rptMenuList.DataSource = secondresult;
                rptMenuList.DataBind();

            }

        }
    }
}