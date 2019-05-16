using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS2.Common.Business;
using FileUpload.FileUpload.Entities;
using FileUpload.FileUpload.Data;

namespace ETS2.WebApp.V
{
    public partial class Member : System.Web.UI.MasterPage
    {

        public string fileUrl = AppSettings.CommonSetting.GetValue("FileUpload/FileUrl").ConvertTo<string>();//文件访问地址

        public int comid = 0;//公司id
        public string proid = "";//产品id
        public string ordertype = "1";//订单类型 默认为1（直销订单）
        public int orderid = 0;//
        public string proname = "";



        //页面显示信息
        public string Logourl = "";//LOGO
        public string Com_name = "";//公司名称
        public string Scenic_name = "";//景点名称
        public string Scenic_intro = "";//景点简介
        public string Service_Phone = "";//服务电话
        public string Copyright = "";//服务电话
        public string RequestUrl = "";//访问网址通过访问网址判断商家，如果访问网址为空则跳转到登陆页

        //用户信息
        public string AccountId = "";
        public string AccountName = "";
        public string AccountCard = "";
        public string Today = "";
        public int viewtop = 1; //头部及左侧相关显示控制
        protected void Page_Load(object sender, EventArgs e)
        {
            Request.ValidateInput();


            if (Session["AccountId"] != null)
            {
                AccountId = Session["AccountId"].ToString();
                AccountName = Session["AccountName"].ToString();
                AccountCard = Session["AccountCard"].ToString();
                Today = DateTime.Now.ToString("yyyy-MM-dd");

            }

            comid = Request["comid"].ConvertTo<int>(0);
            proid = Request["proid"].ConvertTo<string>("");
            orderid = Request["orderid"].ConvertTo<int>(0);
            ordertype = Request["ordertype"].ConvertTo<string>("1");
            RequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();

            if (orderid != 0)
            {
                B2bOrderData dataorder = new B2bOrderData();
                B2b_order modelb2border = dataorder.GetOrderById(orderid);
                if (modelb2border != null)
                {
                    proid = modelb2border.Pro_id.ToString();
                }
            }

            if (proid != "")
            {
                B2b_com_pro pro = new B2bComProData().GetProById(proid);
                if (pro != null)
                {
                    //如果是 云顶旅游大巴的 不显示头部
                    if (pro.Projectid == 2179)
                    {
                        viewtop = 0;
                        Scenic_name = pro.Pro_name;
                        Com_name = pro.Pro_name;
                    }

                    //绿野 不显示头部
                    if (pro.Com_id == 2553)
                    {
                        viewtop = 0;
                    }

                }
            }


            //根据域名读取商户ID,如果没有绑定域名直接跳转后台
            B2b_company_info companyinfo = B2bCompanyData.GetComId(RequestUrl);
            if (companyinfo != null)
            {
                comid = companyinfo.Com_id;
            }
            else
            {
                //判定是否为自助域名规则安 shop1.etown.cn
                if (Domain_def.Domain_yanzheng(RequestUrl))
                {
                    comid = Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl));
                }
            }

            //判断商户ID（域名未绑定）
            if (comid != 0)
            {
                B2b_company com = B2bCompanyData.GetAllComMsg(comid);
                B2b_company_saleset comsetinfo = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());
                if (com != null)
                {
                    if (Com_name == "")
                    {
                        Com_name = com.Com_name;
                    }
                    if (Scenic_name == "")
                    {
                        Scenic_name = com.Scenic_name;
                    }
                    Scenic_intro = com.B2bcompanyinfo.Scenic_intro;

                    if (comsetinfo != null)
                    {
                        Logourl = comsetinfo.Logo;
                        if (Logourl != "")
                        {
                            FileUploadModel url = new FileUploadData().GetFileById(int.Parse(Logourl));
                            if (url != null)
                            {
                                Logourl = "<img src=\"" + fileUrl + url.Relativepath + "\" alt=\"" + Scenic_name + "\" height=\"24\" />";
                            }
                        }

                        Service_Phone = comsetinfo.Service_Phone;
                        if (Service_Phone != "")
                        {
                            Service_Phone = "客服电话:" + Service_Phone;
                        }

                        Copyright = comsetinfo.Copyright;
                    }
                }
            }
            else
            {
                //如果没有商户ID（域名未绑定） 则跳转管理登陆页面。
                // Response.Redirect("/Manage/index1.html");
            }



        }
    }
}