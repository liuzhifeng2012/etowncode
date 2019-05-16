using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.Common.Business;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.Member.Service.MemberService.Data;
using ETS2.Member.Service.MemberService.Model;


namespace ETS2.WebApp.H5
{
    public partial class StoreDefault : System.Web.UI.Page
    {
        #region 辨别商家参数
        public string RequestUrl = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Trim().ToLower();
        public int comid = 0;
        #endregion

        public string title_arr = "";
        public string img_arr = "";
        public string url_arr = "";
        public string menshiimgurl = "";
        public string companyname = "";
        public int menshiid = 0;
        public Member_Channel_company menshi = null;//门店信息

        protected void Page_Load(object sender, EventArgs e)
        {
            //string u = Request.ServerVariables["HTTP_USER_AGENT"];
            //bool bo = detectmobilebrowser.HttpUserAgent(u);

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


            #region 传递过来门市id,通过门市id获得门市信息
             menshiid = Request["menshiid"].ConvertTo<int>(0);
            if (menshiid > 0)
            {
                menshi = new MemberChannelcompanyData().GetChannelCompany(menshiid.ToString());
                if (menshi != null)
                {
                    menshiimgurl = FileSerivce.GetImgUrl(menshi.Companyimg);
                    companyname = menshi.Companyname;
                    comid = menshi.Com_id;
                }
            }
            #endregion



            ////默认图片
            //string defaultimg = "/Images/StoreDefault.jpg";
            //title_arr = "\\\"" + companyname + "\\\"";
            //img_arr = "\\\"" + defaultimg.Replace("/", "\\\\\\/") + "\\\"";
            //url_arr = "\\\"" + "#" + "\\\""; ;

            int totalcount = 0;
            B2bCompanyImageData imgdata = new B2bCompanyImageData();

            //Banner,调取门市总社图片
            List<B2b_company_image> channelimglist = imgdata.PageChannelGetimageList(comid, menshiid, out totalcount);
            if (channelimglist != null)
            {
                for (int i = 0; i < totalcount; i++)
                {
                    if (title_arr == "")
                    {
                        title_arr = "\\\"" + channelimglist[i].Title + "\\\"";
                        img_arr = "\\\"" + FileSerivce.GetImgUrl(channelimglist[i].Imgurl).Replace("/", "\\\\\\/") + "\\\"";
                        url_arr = "\\\"" + channelimglist[i].Linkurl.Replace("/", "\\\\\\/") + "\\\""; ;
                    }
                    else
                    {
                        title_arr += "," + "\\\"" + channelimglist[i].Title + "\\\"";
                        img_arr += "," + "\\\"" + FileSerivce.GetImgUrl(channelimglist[i].Imgurl).Replace("/", "\\\\\\/") + "\\\"";
                        url_arr += "," + "\\\"" + channelimglist[i].Linkurl.Replace("/", "\\\\\\/") + "\\\""; ;
                    }
                }
            }


            //Banner,调取总社图片
            List<B2b_company_image> imglist = imgdata.PageGetimageList(comid, 1, out totalcount);
            if (imglist != null)
            {
                for (int i = 0; i < totalcount; i++)
                {
                    if (title_arr == "")
                    {
                        title_arr = "\\\"" + imglist[i].Title + "\\\"";
                        img_arr = "\\\"" + FileSerivce.GetImgUrl(imglist[i].Imgurl).Replace("/", "\\\\\\/") + "\\\"";
                        url_arr = "\\\"" + imglist[i].Linkurl.Replace("/", "\\\\\\/") + "\\\""; ;
                    }
                    else
                    {
                        title_arr += "," + "\\\"" + imglist[i].Title + "\\\"";
                        img_arr += "," + "\\\"" + FileSerivce.GetImgUrl(imglist[i].Imgurl).Replace("/", "\\\\\\/") + "\\\"";
                        url_arr += "," + "\\\"" + imglist[i].Linkurl.Replace("/", "\\\\\\/") + "\\\""; ;
                    }
                }
            }

        }
    }
}