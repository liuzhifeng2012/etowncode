using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.Common.Business;
using FileUpload.FileUpload.Entities.Enum;

namespace ETS2.WebApp.UI.UserUI
{
    public partial class usermanage : System.Web.UI.Page
    {
        public int staffid = 0;//编辑员工的id
        public string companyname = "";//公司名称

        public string weidian_url = "";//个人微店地址

        public int isaccountmanage = 0;//是否是账户管理
        public int istiaozhuan = 1;//是否跳转，如果是没有传入员工id，则是个人的账户管理，编辑后还是在本页不跳转到列表页面
        protected void Page_Load(object sender, EventArgs e)
        {
            staffid = Request["staffid"].ConvertTo<int>(0);
            isaccountmanage = Request["isaccountmanage"].ConvertTo<int>(0);
            if (UserHelper.ValidateLogin())
            {
                int comid = UserHelper.CurrentUserId();

                companyname = UserHelper.CurrentCompany.Com_name;

                if (staffid > 0)
                {
                    weidian_url = "http://shop" + comid + ".etown.cn/weidian_" + staffid + ".aspx";
                }
                else
                {
                    if (isaccountmanage == 1)
                    {
                        staffid = UserHelper.CurrentUserId();
                        istiaozhuan = 0;
                    }
                }
            }



            BindHeadPortrait();
        }
        private void BindHeadPortrait()
        {
            headPortrait.uploadFileInfo.ObjType = (int)FileObjType.Photo;
            headPortrait.displayImgId = "headPortraitImg";

        }
    }
}