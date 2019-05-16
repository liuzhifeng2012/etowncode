using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
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

namespace ETS2.WebApp.UI.PMUI.travel
{
    public partial class linegroupdate : System.Web.UI.Page
    {
        public int userid = 0;//当前登录用户id
        public string username = "";//当前登录用户名

        public int comid = 0;//当前登录商家id
        public string comname = "";//公司名称
        public string groupname = "";//所在分组


        public string RequestDomin = "";//访问域名
        public string Requestfile = "";//访问文件
        public string companydo = "";//商户网址

        public int atypee = 2;//判断登录身份（管理员,验票员)，默认是验票元

        public string comlogo = "/images/defaultThumb.png";//公司logo
        public string fileUrl = AppSettings.CommonSetting.GetValue("FileUpload/FileUrl").ConvertTo<string>();//文件访问地址

        public int iscanverify = 1;//是否可以验证电子票和验证会员卡

        public int lineid = 0;//线路id
        public string linename = "";//产品名称

        public int ServerType = 0;//产品服务类型（1.电子凭证2.跟团游8.本地游9.酒店客房）


        public decimal adviseprice = 0;//直销价格

        public decimal basic_agent1_price = 0;//基本信息中一级分销返还
        public decimal basic_agent2_price = 0;//基本信息中二级分销返还
        public decimal basic_agent3_price = 0;//基本信息中三级分销返还
        protected void Page_Load(object sender, EventArgs e)
        {

            //获取访问的域名   
            RequestDomin = Request.ServerVariables["SERVER_NAME"].ToLower();
            Requestfile = Request.ServerVariables["Url"].ToLower();




            //如果是微旅行，则直接跳转 当域名为微旅行，访问默认页面则直接跳转会员专区
            if ((RequestDomin == "v.vctrip.com" || RequestDomin == "www.vctrip.com" || RequestDomin == "v.etown.cn") && Requestfile == "/default.aspx")
            {
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




            //如果是绑定域名跳转到产品页，否则
            B2b_company_info companyinfo = B2bCompanyData.GetComId(RequestDomin);
            if (companyinfo != null)
            {
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
                        Response.Redirect("/ui/shangjiaui/ProductList.aspx");
                    }

                }
                else
                {
                    if (UserHelper.ValidateLogin())
                    {
                        GetUser();
                    }
                    else
                    {
                        if (RequestDomin == "admin.vctrip.com")
                        {
                            Response.Redirect("/Manage/index1.html");
                        }
                        else
                        {
                            if (RequestDomin == "shop.etown.cn" || RequestDomin == "admin.etown.cn" || RequestDomin == "test.etown.cn")
                            {

                                Response.Redirect("/Manage/default.html");

                            }

                            if (RequestDomin == "mm.easytour.cn" || RequestDomin == "weixin.easytour.cn")//跳转渠道
                            {

                                Response.Redirect("/Channel/easytour.html");
                            }
                            else if (RequestDomin == "zhangjiakou.etown.cn" || RequestDomin == "zhangjiakoushop.etown.cn")//跳转渠道
                            {
                                Response.Redirect("/Channel/zhangjiakou.html");
                            }
                            else
                            {

                                Response.Redirect("/admin/");
                            }
                        }
                    }
                }
            }
            if (!IsPostBack)
            {
                lineid = Request["lineid"].ConvertTo<int>(0);
                if (lineid == 0)
                {
                    Response.Redirect("/ui/pmui/productlist.aspx");
                }
                //获得产品基本信息
                var pro = new B2bComProData().GetProById(lineid.ToString());
                if (pro != null)
                {
                    ServerType = pro.Server_type;
                    adviseprice = pro.Advise_price;
                    linename = pro.Pro_name;

                    basic_agent1_price = pro.Agent1_price;
                    basic_agent2_price = pro.Agent2_price;
                    basic_agent3_price = pro.Agent3_price;
                }



                //获得团期/房态
                List<B2b_com_LineGroupDate> list = new B2b_com_LineGroupDateData().GetLineGroupDateByLineid(lineid, "1");
                if (list != null && list.Count > 0)
                {
                    var date = from r in list 
                               orderby r.Daydate
                               select r.Daydate.ToString("yyyy-MM-dd");
                    hidLeavingDate.Value = string.Join(",", date.ToList());
                    hidinitLeavingDate.Value = string.Join(",", date.ToList());


                }
            }

        }

        private void GetUser()
        {
            B2b_company_manageuser user = UserHelper.CurrentUser();
            B2b_company company = UserHelper.CurrentCompany;

            atypee = user.Atype;
            userid = user.Id;
            comid = company.ID;
            comname = company.Com_name;

            //判断页面是否在权限页面链接中：没有，不做处理；有，判断角色是否可以访问此页面
            bool isactionurl = new Sys_ActionData().Isactionurl(Requestfile);
            if (isactionurl)
            {
                bool iscanvisit = new Sys_ActionData().Iscanvisit(Requestfile, userid);
                if (iscanvisit == false)
                {
                    //Response.Redirect("/manage.aspx");
                    Response.Write("<script>window.location.href='/manage.aspx'</script>");
                }
            }



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
                //控制只有平台总账户才可以进入/ui/permissionui目录
                if (Requestfile.Contains("/ui/permissionui"))
                {
                    if (muser.Id != 1035)
                    {
                        Response.Redirect("http://shop.etown.cn");
                    }
                }

                Sys_Group gg = new Sys_GroupData().GetGroupByUserId(muser.Id);
                if (gg == null)
                {
                    //Response.Write("<script>alert('用户尚未分配角色，请联系管理员！');location.href='/Manage/index1.html'</script>");
                }
                else
                {
                    iscanverify = gg.Iscanverify;
                    groupname = gg.Groupname;
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