using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.Common.Business;
using ETS2.Permision.Service.PermisionService.Model;
using ETS2.Permision.Service.PermisionService.Data;

namespace ETS2.WebApp.WeiXin.ad
{
    public partial class AdManage : System.Web.UI.Page
    {
        public int comid = 0;
        public int userid = 0;
        public int iscandelmaterialtype = 0;//是否可以删除素材类型:0不可以；1可以

        protected void Page_Load(object sender, EventArgs e)
        {

            if (UserHelper.ValidateLogin())
            {
                comid = UserHelper.CurrentCompany.ID;
                userid = UserHelper.CurrentUserId();
                //根据用户获得所属分组
                Sys_Group group = new Sys_GroupData().GetGroupByUserId(userid);
                if (group != null)
                {
                    if (group.Groupname == "管理员" || group.Groupname == "商户负责人（含实体卡）" || group.Groupname == "商户负责人" || group.Groupname == "景区负责人（含分销系统）")
                    {
                        iscandelmaterialtype = 1;
                    }
                    else
                    {
                        iscandelmaterialtype = 0;
                    }
                }
            }
        }
    }
}