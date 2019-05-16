using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using FileUpload.FileUpload.Entities;
using ETS2.PM.Service.PMService.Data;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.WebApp.H5.ad
{
    public partial class Default : System.Web.UI.Page
    {
        public int id = 0;
        public int comid = 0;
        public string Title = "";
        public string Link = "";
        public string Author = "";
        public string Keyword = "";

        public int Applystate = 0;
        public int Votecount = 0;
        public int Lookcount = 0;
        public int Musicid = 0;
        public string Musicscr = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            id = Request["id"].ConvertTo<int>(0);

            ShowImgBind();

        }

        private void ShowImgBind()
        {
            //var comid = Context.Request["comid"].ConvertTo<int>(0);
            
            string RequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();
            if (Domain_def.Domain_yanzheng(RequestUrl))//如果符合shop101.etown.cn的格式，则从多微信商户基本信息表中获取comid
            {
                comid = Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl).ToString());

            }
            else
            {
                B2b_company_info companyinfo = B2bCompanyData.GetComId(RequestUrl);
                if (companyinfo != null)
                {
                    comid = companyinfo.Com_id;
                }
            }

            if (id != 0 && comid !=0)
            {
                var actdata = new WxAdData();
                var pro = actdata.Getwxad(id, comid);

                if (pro != null)
                {
                    id = pro.Id;
                    Title = pro.Title;
                    Link = pro.Link;
                    Author = pro.Author;
                    Keyword = pro.Keyword;
                    Musicid=pro.Musicid;
                    Applystate = pro.Applystate;
                    
                    if(Musicid !=0){
                        Musicscr =FileSerivce.GetImgUrl(Musicid);
                    }
                }
            }
        }
    }
}