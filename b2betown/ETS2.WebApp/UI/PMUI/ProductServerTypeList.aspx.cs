using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.Common.Business;

namespace ETS2.WebApp.UI.PMUI
{
    public partial class ProductAdd2 : System.Web.UI.Page
    {
        public int projectid = 0;//项目id


        public int com_type = 0;//商户的行业
        public int comid = 0;//公司编号
        protected void Page_Load(object sender, EventArgs e)
        {
            projectid = Request["projectid"].ConvertTo<int>(0);

            if(UserHelper.ValidateLogin())
            {
                com_type = UserHelper.CurrentCompany.Com_type;
                comid = UserHelper.CurrentCompany.ID;
            }
        }
    }
}