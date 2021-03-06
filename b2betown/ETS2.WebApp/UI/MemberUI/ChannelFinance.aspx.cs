﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS2.PM.Service.PMService.Data;

namespace ETS2.WebApp.UI.MemberUI
{
    public partial class ChannelFinance : System.Web.UI.Page
    {
        public int comid = UserHelper.CurrentCompany.ID;//公司id
        public bool IsParentCompanyUser = true; //判断操作账户类型(总公司账户;门市账户)
        public string channelcompanyid = "0";//员工所在门市id,默认为0
        public int ishaslvyoubusproorder = 0;//是否含有 旅游大巴产品订单
        protected void Page_Load(object sender, EventArgs e)
        {
            if (UserHelper.ValidateLogin())
            {
                int userid = UserHelper.CurrentUserId();
                IsParentCompanyUser = new B2bCompanyManagerUserData().IsParentCompanyUser(userid);
                if (IsParentCompanyUser == false)//如果是门市账户
                {
                    B2b_company_manageuser user = B2bCompanyManagerUserData.GetUser(userid);
                    channelcompanyid = user.Channelcompanyid.ToString();
                }

                //ishaslvyoubusproorder = new B2bComProData().IsHasLvyoubusPro(UserHelper.CurrentCompany.ID, (int)ProductServer_Type.LvyouBus);

                ishaslvyoubusproorder = new B2bOrderData().IsHasLvyoubusProOrder(UserHelper.CurrentCompany.ID, (int)ProductServer_Type.LvyouBus);   
      
            }
        }
    }
}