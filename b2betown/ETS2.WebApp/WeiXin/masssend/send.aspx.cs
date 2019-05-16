using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.Common.Business;
using ETS.Framework;

namespace ETS2.WebApp.WeiXin.masssend
{
    public partial class send : System.Web.UI.Page
    {
        public int isrenzheng = 1;//1已认证；0未认证

        public int tuwen_recordid = 0;//图文消息id
        protected void Page_Load(object sender, EventArgs e)
        {
            tuwen_recordid = Request["tuwen_recordid"].ConvertTo<int>(0);
            if (UserHelper.ValidateLogin())
            {
                int comid = UserHelper.CurrentCompany.ID;
                WeiXinBasic wxbasic = new WeiXinBasicData().GetWxBasicByComId(comid);
                if (wxbasic != null)
                {

                    if (wxbasic.Weixintype==4)
                    {
                        isrenzheng = 1;
                    }
                    else
                    {
                        isrenzheng = 0;
                    }
                }
                else
                {
                    isrenzheng = 0;
                }
            }
            else 
            {
                Response.Redirect("/Manage/index1.html");
            }
            
        }
    }
}