using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.Common.Business;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS.Framework;

namespace ETS2.WebApp.H5.Hotel
{
    public partial class HotelSubmit : System.Web.UI.Page
    {
        public string checkindate = "";//入住日期
        public string checkoutdate = "";//离店日期
        public int bookdaynum = 0;//入住天数

        public int roomtypeid = 0;//房型id
        public int comid = 0;//公司id

        //public string comName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime checkindate1 = Request["indate"].ConvertTo<DateTime>();
            checkindate = checkindate1.ToString("yyyy-MM-dd");
            DateTime checkoutdate1 = Request["outdate"].ConvertTo<DateTime>();
            checkoutdate = checkoutdate1.ToString("yyyy-MM-dd");
            bookdaynum = (checkoutdate1 - checkindate1).Days;
            roomtypeid = Request["id"].ConvertTo<int>(0);
            comid = Request["comid"].ConvertTo<int>(0);

            bookdaynum = (checkoutdate1 - checkindate1).Days;
            //if (comid != 0)
            //{
            //    B2b_company companyinfo = B2bCompanyData.GetCompany(comid);
            //    if (companyinfo != null)
            //    {
            //        comName = companyinfo.Com_name;
            //        if (comName.Length >= 9)
            //        {
            //            comName = comName.Substring(0, 9) + "..";
            //        }
            //    }
            //}

        }
    }
}