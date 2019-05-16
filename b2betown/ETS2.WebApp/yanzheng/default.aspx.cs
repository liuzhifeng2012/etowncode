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
using ETS.Framework;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Member.Service.MemberService.Data;
using FileUpload.FileUpload.Entities;
using FileUpload.FileUpload.Data;

namespace ETS2.WebApp.yanzheng
{
    public partial class _default : System.Web.UI.Page
    {
        public int userid = 0;//当前登录用户id
        public string username = "";//当前登录用户名

        public int comid = 0;//当前登录商家id
        public string comname = "";//公司名称
        public int groupid = 0;//管理组id
        public string groupname = "";//所在分组


        public string RequestDomin = "";//访问域名
        public string Requestfile = "";//访问文件
        public string companydo = "";//商户网址

        public int atypee = 2;//判断登录身份（管理员,验票员)，默认是验票元

        public string comlogo = "/images/defaultThumb.png";//公司logo
        public string fileUrl = AppSettings.CommonSetting.GetValue("FileUpload/FileUrl").ConvertTo<string>();//文件访问地址

        public int iscanverify = 1;//是否可以验证电子票和验证会员卡

        public string VirtualUrl = "";//虚拟路径
        public string parastr = "";//参数

        protected void Page_Load(object sender, EventArgs e)
        {
            //获取访问的域名   
            RequestDomin = Request.ServerVariables["SERVER_NAME"].ToLower();
            Requestfile = Request.ServerVariables["Url"].ToLower();

            if (UserHelper.ValidateLogin())
            {
                GetUser();
                //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "002");
                Response.Redirect("/ui/pmui/ETicket/mETicketIndex.aspx");
            }
            else
            {
                //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "003");
                //Response.Redirect("/yanzheng/Login.aspx");
            }
           
        }


        private void GetUser()
        {
            //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "004");

            B2b_company_manageuser user = UserHelper.CurrentUser();
            B2b_company company = UserHelper.CurrentCompany;

            atypee = user.Atype;
            userid = user.Id;
            comid = company.ID;
            comname = company.Com_name;



            //根据comid得到公司logo信息
            B2b_company_saleset logoset = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());
            if (logoset != null)
            {
                int logo_temp = 0;

                if (logoset.Logo != "")
                {
                    logo_temp = int.Parse(logoset.Logo);
                }

                FileUploadModel identityFileUpload = new FileUploadData().GetFileById(logo_temp);
                if (identityFileUpload != null)
                {
                    comlogo = fileUrl + identityFileUpload.Relativepath;
                }
            }
            //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "005");

            username = user.Accounts;
            B2b_company companyinfo = B2bCompanyData.GetAllComMsg(comid);

            if (companyinfo.B2bcompanyinfo.Domainname != "")
            {
                companydo = "http://" + companyinfo.B2bcompanyinfo.Domainname;
            }
            else
            {
                companydo = "http://shop" + company.ID + ".etown.cn";
            }

            //根据userid得到用户信息，如果用户没有渠道公司的分配，则显示全部门市
            B2b_company_manageuser muser = B2bCompanyManagerUserData.GetUser(UserHelper.CurrentUserId());
            if (muser != null)
            {

                //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "006");
                Sys_Group gg = new Sys_GroupData().GetGroupByUserId(muser.Id);
                if (gg == null)
                {
                    //Response.Write("<script>alert('用户尚未分配角色，请联系管理员！');location.href='/Manage/index1.html'</script>");
                }
                else
                {
                    iscanverify = gg.Iscanverify;
                    groupname = gg.Groupname;
                    groupid = gg.Groupid;
                }

            }
        }
    }
}