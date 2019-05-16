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

namespace ETS2.WebApp.UI
{
    public partial class Etown : System.Web.UI.MasterPage
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


            //SortedDictionary<string, string> para = CommonFunc.GetRequestPost();  //post传递过来的参数
            //var parastr = "";//传递过来的参数字符串             
            //if (para.Count == 0)
            //{
            //    para = CommonFunc.GetRequestGet();  //get传递过来的参数

            //} 
            //if (para.Count > 0)
            //{
            //    parastr = CommonFunc.CreateLinkString(para);
            //}
            //if (parastr != "")
            //{
            //    parastr = "?" + parastr.ToLower();
            //}
            parastr = HttpContext.Current.Request.Url.Query.ToLower();
            VirtualUrl = Request.CurrentExecutionFilePath.ToLower() ;

            //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "1");

            //如果是微旅行，则直接跳转 当域名为微旅行，访问默认页面则直接跳转会员专区
            if ((RequestDomin == "v.vctrip.com" || RequestDomin == "www.vctrip.com" || RequestDomin == "v.etown.cn") && Requestfile == "/default.aspx")
            {
                //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "2");

                //非手机的跳转到V目录下
                string u = Request.ServerVariables["HTTP_USER_AGENT"];
                bool phonebool = detectmobilebrowser.HttpUserAgent(u);
                if (phonebool == false)
                {
                    Response.Redirect("/V/");//非手机的跳转到V目录
                }
                else
                {
                    Response.Redirect("/M/");//非手机的跳转到V目录
                }
            }

            if (RequestDomin == "y.etown.cn" || RequestDomin == "www.y.etown.cn")
            {
                Response.Redirect("/YY/");
            }




            //如果是绑定域名跳转到产品页，否则
            B2b_company_info companyinfo = B2bCompanyData.GetComId(RequestDomin);
            if (companyinfo != null)
            {
                //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "3");
                //comid = companyinfo.Com_id;
                if (companyinfo.Com_id == 1305)//麦客行
                {
                    if (UserHelper.ValidateLogin())
                    {
                        // TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "4");
                        GetUser();
                    }
                    else
                    {
                        //Response.Redirect("/h5/order/");
                        //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "5");
                        Response.Redirect("/Agent/page.html");
                    }
                }
                else if (companyinfo.Com_id == 1194)//大好河山张家口
                {
                    if (UserHelper.ValidateLogin())
                    {
                        // TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "4");
                        GetUser();
                    }
                    else
                    {
                        //Response.Redirect("/h5/order/");
                        //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "5");
                        Response.Redirect("/Channel/index.aspx");
                    }
                }
                else
                {
                    //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "6");
                    Response.Redirect("/ui/shangjiaui/ProductList.aspx");
                }

            }
            else
            {
                //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "7");
                //判定是否为自助域名规则安 shop1.etown.cn
                if (Domain_def.Domain_yanzheng(RequestDomin))
                {
                    //comid = Int32.Parse(Domain_def.Domain_Huoqu(RequestDomin));
                    if (RequestDomin == "shop1143.etown.cn")
                    {
                        Response.Redirect("http://shop.etown.cn/Manage/default.html");
                    }
                    else
                    {
                        //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "9");
                        Response.Redirect("/ui/shangjiaui/ProductList.aspx");

                    }

                }
                else
                {

                   
                    if (UserHelper.ValidateLogin())
                    {
                        //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "10");
                        GetUser();
                    }
                    else
                    {
                        //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "11");
                        if (RequestDomin == "admin.vctrip.com")
                        {
                            //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "12");
                            Response.Redirect("/Manage/index1.html");
                        }
                        else
                        {
                            if (RequestDomin == "shop.etown.cn" || RequestDomin == "admin.etown.cn" || RequestDomin == "test.etown.cn")
                            {
                                //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "13");
                                Response.Redirect("/Manage/default.html");

                            }

                            if (RequestDomin == "duole.etown.cn" || RequestDomin == "duolemeidi.etown.cn")
                            {
                                Response.Redirect("/Channel/duole.html");
                            }
                            if (RequestDomin == "mm.easytour.cn" || RequestDomin == "weixin.easytour.cn")//跳转渠道
                            {
                                Response.Redirect("/Channel/easytour.html");
                            }
                            else
                            {
                                //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "15");
                                Response.Redirect("/admin/");
                            }
                        }
                    }
                }
            }


            //如果是商家指向的域名则跳转到商户预订页。必须访问默认页时，如果是后台页则认为是管理后台
            //if (RequestDomin != "shop.etown.cn" && RequestDomin != "test.etown.cn" && Requestfile == "/default.aspx")
            //{
            //    如果非指定域名直接跳转到商家预订页,再由此页面订如果是绑定的访问此站，非绑定的访问登陆管理后台
            //    Response.Redirect("/ui/shangjiaui/ProductList.aspx");
            //}
            //else
            //{//如果不是是shop、test.etown.cn 则为易城商户管理

            //    if (UserHelper.ValidateLogin())
            //    {
            //        GetUser();
            //    }
            //    else
            //    {

            //        if (RequestDomin == "shop.etown.cn" || RequestDomin == "test.etown.cn"  || RequestDomin == "admin.etown.cn" || RequestDomin == "admin.vctrip.com")
            //        {
            //            Response.Redirect("/Manage/index1.html");
            //        }
            //        else {

            //            Response.Redirect("/admin/");
            //        }



            //    }
            //}
        }

        private void GetUser()
        {
            B2b_company_manageuser user = UserHelper.CurrentUser();
            B2b_company company = UserHelper.CurrentCompany;

            atypee = user.Atype;
            userid = user.Id;
            comid = company.ID;
            comname = company.Com_name;

            ////判断页面是否在权限页面链接中：没有，不做处理；有，判断角色是否可以访问此页面
            //bool isactionurl = new Sys_ActionData().Isactionurl(Requestfile);
            //if (isactionurl)
            //{
            //    bool iscanvisit = new Sys_ActionData().Iscanvisit(Requestfile,userid);
            //    if (iscanvisit == false)
            //    {
            //        //Response.Redirect("/manage.aspx");
            //        Response.Write("<script>window.location.href='/manage.aspx'</script>"); 
            //    }
            //}



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
                ////控制只有平台总账户才可以进入/ui/permissionui目录
                //if (Requestfile.Contains("/ui/permissionui"))
                //{
                //  if(muser.Id!=1035)
                //  {
                //      Response.Redirect("http://shop.etown.cn");
                //  }
                //}

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



            //根据不同用户显示不同的左侧栏
            int totalcount = 0;
            List<Sys_ActionColumn> topList = new Sys_ActionColumnData().GetActionColumnByUser(UserHelper.CurrentUserId(), out totalcount);


            rptTopMenuList.DataSource = topList;
            rptTopMenuList.DataBind();

            foreach (RepeaterItem item in rptTopMenuList.Items)
            {
                int funcId = (item.FindControl("HideFuncId") as HiddenField).Value.ConvertTo<int>();

                int totalaction = 0;
                var menuList = new Sys_ActionData().GetActionsByColumnId(UserHelper.CurrentUserId(), funcId, out totalaction);

                Repeater rptMenuList = item.FindControl("rptMenuList") as Repeater;
                rptMenuList.DataSource = menuList;
                rptMenuList.DataBind();

            }

        }
    }
}