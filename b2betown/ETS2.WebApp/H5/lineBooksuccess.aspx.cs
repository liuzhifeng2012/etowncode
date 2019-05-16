using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.PM.Service.PMService.Data;
using ETS2.Common.Business;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.WebApp.H5
{
    public partial class lineBooksuccess : System.Web.UI.Page
    {

        public int lineid = 0;
        public string pro_name = "";
        public int id;
        public int com_id;
        public int pro_state;
        public string pro_Remark = String.Empty;
        public decimal face_price;
        public decimal advise_price;
        public string sms = String.Empty;

        public int adultnum = 0;
        public int childnum = 0;
        public string outdate = "";
        public int travelproductid;//产品编号

        public string travelstarting;//出发地
        public string travelending;//目的地


        protected void Page_Load(object sender, EventArgs e)
        {
            lineid = Request["lineid"].ConvertTo<int>(0);
            adultnum = Request["adultnum"].ConvertTo<int>(0);
            childnum = Request["childnum"].ConvertTo<int>(0);
            outdate = Request["outdate"].ConvertTo<string>("");

            if (lineid != 0)
            {
                var prodata = new B2bComProData();
                var pro = prodata.GetProById(lineid.ToString());

                if (pro != null)
                {
                    pro_name = pro.Pro_name;
                    com_id = pro.Com_id;
                    pro_state = pro.Pro_state;
                    pro_Remark = pro.Pro_Remark;
                    face_price = pro.Face_price;
                    advise_price = pro.Advise_price;
                    sms = pro.Sms;
                    travelproductid = pro.Travelproductid;

                    //读取団期价格//根据实际选择的団期报价
                    B2b_com_LineGroupDate linemode = new B2b_com_LineGroupDateData().GetLineDayGroupDate(DateTime.Parse(outdate), lineid);
                    if (linemode != null)
                    {
                        advise_price = linemode.Menprice;
                    }


                }


            }

        }
    }
}