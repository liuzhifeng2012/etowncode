using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.Common.Business;

namespace ETS2.WebApp.UI.CrmUI
{
    public partial class B2bCrm_LevelManage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (UserHelper.ValidateLogin() == true)
                {
                    int comid = UserHelper.CurrentCompany.ID;
                    //判断公司是否含有会员级别设置
                    int totalcount = 0;
                    List<B2bcrmlevels> list = new B2bcrmlevelsData().Getb2bcrmlevelsbycomid(comid, out totalcount);

                    if (totalcount == 0)
                    {
                        InsDefaultLevel(comid);//录入会员默认级别
                    }

                }

            }
        }

        /// <summary>
        /// 公司不含有会员级别设置，则录入默认级别
        /// </summary>
        /// <param name="comid"></param>
        private void InsDefaultLevel(int comid)
        {
            

            for (int i = 1; i <= 4; i++)
            {
                if (i == 1)
                {
                    B2bcrmlevels m = new B2bcrmlevels
                    {
                        id = 0,
                        crmlevel = "A",
                        levelname = "普通会员",
                        dengjifen_begin = 0,
                        dengjifen_end = 499,
                        tequan = "享受普通会员权利",
                        com_id = comid,
                        isavailable = 1
                    };
                    int n = new B2bcrmlevelsData().EditB2bCrmLevel(m);
                }
                if (i == 2)
                {
                    B2bcrmlevels m = new B2bcrmlevels
                    {
                        id = 0,
                        crmlevel = "B",
                        levelname = "铜牌会员",
                        dengjifen_begin = 500,
                        dengjifen_end = 1999,
                        tequan = "享受铜牌会员权利",
                        com_id = comid,
                        isavailable = 1
                    };
                    int n = new B2bcrmlevelsData().EditB2bCrmLevel(m);
                }
                if (i == 3)
                {
                    B2bcrmlevels m = new B2bcrmlevels
                    {
                        id = 0,
                        crmlevel = "C",
                        levelname = "银牌会员",
                        dengjifen_begin = 2000,
                        dengjifen_end = 4999,
                        tequan = "享受银牌会员权利",
                        com_id = comid,
                        isavailable = 1
                    };
                    int n = new B2bcrmlevelsData().EditB2bCrmLevel(m);
                }
                if (i == 4)
                {
                    B2bcrmlevels m = new B2bcrmlevels
                    {
                        id = 0,
                        crmlevel = "D",
                        levelname = "金牌会员",
                        dengjifen_begin = 5000,
                        dengjifen_end = 1000000000,
                        tequan = "享受金牌会员权利",
                        com_id = comid,
                        isavailable = 1
                    };
                    int n = new B2bcrmlevelsData().EditB2bCrmLevel(m);
                }
            }
        }
    }
}