using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.PM.Service.PMService.Data;
using ETS2.Common.Business;

namespace ETS2.WebApp.H5
{
    public partial class lineTrip : System.Web.UI.Page
    {
        public int lineid = 0;
        public string pro_name = "";
        public int id;
        public int com_id;
        public int pro_state;
        public int server_type;
        public int pro_type;
        public int source_type;
        public string pro_Remark = String.Empty;
        public DateTime pro_start;
        public DateTime pro_end;
        public decimal face_price;
        public decimal advise_price;
        private decimal agent1_price;
        public decimal agent2_price;
        public decimal agent3_price;
        public decimal agentsettle_price;
        public int thatDay_can;
        public int thatday_can_day;
        public string service_Contain = String.Empty;
        public string service_NotContain = String.Empty;
        public string precautions = String.Empty;
        public int tuan_pro;
        public int zhixiao;
        public int agentsale;
        public DateTime createtime;
        public int createuserid;
        public string sms = String.Empty;
        public int imgurl;
        public int pro_number;
        public string pro_explain;
        public int totalpay;
        public int u_num;
        public decimal totalpay_price;
        public decimal totalprofit;
        public int tuipiao;
        public int tuipiao_guoqi;
        public int tuipiao_endday;
        public int proclass;
        public int projectid;
        public string imgaddress;
        public int travelproductid;//产品编号

        public string travelstarting;//出发地
        public string travelending;//目的地


        protected void Page_Load(object sender, EventArgs e)
        {
            lineid = Request["lineid"].ConvertTo<int>(0);

            if (lineid != 0)
            {
                var prodata = new B2bComProData();
                var pro = prodata.GetProById(lineid.ToString());

                if (pro != null)
                {
                    pro_name = pro.Pro_name;
                    com_id = pro.Com_id;
                    pro_state = pro.Pro_state;
                    server_type = pro.Server_type;
                    pro_type = pro.Pro_type;
                    source_type = pro.Source_type;
                    pro_Remark = pro.Pro_Remark;
                    pro_start = pro.Pro_start;
                    pro_end = pro.Pro_end;
                    face_price = pro.Face_price;
                    advise_price = pro.Advise_price;
                    agent1_price = pro.Agent1_price;
                    agent2_price = pro.Agent2_price;
                    agent3_price = pro.Agent3_price;
                    agentsettle_price = pro.Agentsettle_price;
                    thatDay_can = pro.ThatDay_can;
                    thatday_can_day = pro.Thatday_can_day;
                    service_Contain = pro.Service_Contain;
                    service_NotContain = pro.Service_NotContain;
                    precautions = pro.Precautions;
                    tuan_pro = pro.Tuan_pro;
                    zhixiao = pro.Zhixiao;
                    agentsale = pro.Agentsale;
                    createtime = pro.Createtime;
                    sms = pro.Sms;
                    createuserid = pro.Createuserid;
                    imgurl = pro.Imgurl;
                    pro_number = pro.Pro_number;
                    pro_explain = pro.Pro_explain;
                    tuipiao = pro.Tuipiao;
                    tuipiao_guoqi = pro.Tuipiao_guoqi;
                    tuipiao_endday = pro.Tuipiao_endday;
                    imgaddress = FileSerivce.GetImgUrl(pro.Imgurl);
                    projectid = pro.Projectid;
                    travelproductid = pro.Travelproductid;
                    travelstarting = pro.Travelstarting;
                    travelending = pro.Travelending;

                }


            }

        }
    }
}