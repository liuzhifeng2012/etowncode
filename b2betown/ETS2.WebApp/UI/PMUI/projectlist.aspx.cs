using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.Permision.Service.PermisionService.Model;
using ETS2.Permision.Service.PermisionService.Data;

namespace ETS2.WebApp.UI.PMUI
{
    public partial class projectlist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //如果是角色 "财务人员"，则跳转到产品统计
            int loginuserid=UserHelper.CurrentUserId();
            List<Sys_Group> grouplist = new Sys_MasterGroupData().GetGroupByMasterId(loginuserid);
            if (grouplist != null)
            {
                List<int> listgroupid = new List<int>();
                 foreach(Sys_Group mgroup in grouplist)
                 {
                    if(mgroup!=null)
                    {
                        listgroupid.Add(mgroup.Groupid);
                    }
                 }
                //财务人员 在管理组中是9
                if(listgroupid.Contains(9))
                {
                    Response.Redirect("/ui/pmui/order/salecount.aspx");
                }
            }
        }
    }
}