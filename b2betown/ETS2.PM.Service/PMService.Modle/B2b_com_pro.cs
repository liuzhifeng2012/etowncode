using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    /// <summary>
    /// 商家产品管理
    /// </summary>
    [Serializable()]
    public class B2b_com_pro
    {
        public int issetidcard { get; set; }//提单时是否设置身份证号

        public int progroupid { get; set; }//产品分组

        public int isSetVisitDate { get; set; }//提单时是否设置游玩日期

        public string SpecifyPosid { get; set; }
        public int pnonumperticket { get; set; }//产品预订1张产生电子码个数
        public string firststationtime { get; set; }//首发站发车时间
        public int pro_yanzheng_method { get; set; }//电子码核销类型
        public int selbindbx { get; set; }
        public int isrebate { get; set; }
        public string nuomi_dealid { get; set; }
        private int id;
        private int com_id;
        private string pro_name = String.Empty;
        private int pro_state;
        private int server_type;
        private int pro_type;
        private int source_type;
        private string pro_Remark = String.Empty;
        private DateTime pro_start;
        private DateTime pro_end;
        private decimal face_price;
        private decimal advise_price;
        private decimal agent1_price;
        private decimal agent2_price;
        private decimal agent3_price;
        private decimal agentsettle_price;
        private int thatDay_can;
        private int thatday_can_day;
        private string service_Contain = String.Empty;
        private string service_NotContain = String.Empty;
        private string precautions = String.Empty;
        private int tuan_pro;
        private int zhixiao;
        private int agentsale;
        private DateTime createtime;
        private int createuserid;
        private string sms = String.Empty;

        private int imgurl;
        private string imgaddress;
        private int pro_number;
        private string pro_explain;


        private int totalpay;
        private int u_num;
        private decimal totalpay_price;
        private decimal totalprofit;


        private int tuipiao;
        private int tuipiao_guoqi;
        private int tuipiao_endday;

        private int proclass;

        private int projectid;

        private int travelproductid;
        private int traveltype;
        private string travelstarting;
        private string travelending;

        private int ispanicbuy = 0;
        private DateTime panic_begintime = DateTime.Now;
        private DateTime panicbuy_endtime = DateTime.Now;
        private int limitbuytotalnum = 10000;


        private int linepro_booktype;//线路预订类型1:无需预付；2需要预付

        private string proValidateMethod;//产品验证有效期方式：1按产品有效期；2按指定有效期
        private int appointdata;//指定产品有效期
        private int iscanuseonsameday;//当天是否可用
        private int viewmethod;//显示范围

        private string pro_youxiaoqi;//前台显示实际的有效期，当选择按指定有效期使用时

        private decimal childreduce;//儿童减免

        private int bindingid;
        private int manyspeci;
        private int cartid;
        private int speciid;

        public int QuitTicketMechanism { get; set; }

        public int isneedbespeak { get; set; }
        public int daybespeaknum { get; set; }
        public string bespeaksucmsg { get; set; }
        public string bespeakfailmsg { get; set; }
        public string customservicephone { get; set; }
        public int isblackoutdate { get; set; }
        public int etickettype { get; set; }

        public int buynum { get; set; }
        public decimal pro_weight { get; set; }

        public int xuhao { get; set; }

        //产品关联人姓名 ;关联人手机（给预订发通知）;预订前是否支付
        public string bookpro_bindphone { get; set; }
        public int bookpro_ispay { get; set; }
        public string bookpro_bindname { get; set; }
        public string bookpro_bindcompany { get; set; }
        public DateTime bookpro_bindconfirmtime { get; set; }

        public int worktimehour { get; set; }//工作时间已小时计算，主要用于打印索道票
        public int worktimeid { get; set; }//工作结束时间 调取产品工作时间表
        public int zhaji_usenum { get; set; }


        public decimal Childreduce
        {
            get { return this.childreduce; }
            set { this.childreduce = value; }
        }

        public string ProValidateMethod
        {
            get { return this.proValidateMethod; }
            set { this.proValidateMethod = value; }
        }
        public int Appointdata
        {
            get { return this.appointdata; }
            set { this.appointdata = value; }
        }
        public int Iscanuseonsameday
        {
            get { return this.iscanuseonsameday; }
            set { this.iscanuseonsameday = value; }
        }
        public int Viewmethod
        {
            get { return this.viewmethod; }
            set { this.viewmethod = value; }
        }

        public int Linepro_booktype
        {
            get { return linepro_booktype; }
            set { linepro_booktype = value; }
        }

        public int Ispanicbuy
        {
            get { return ispanicbuy; }
            set { ispanicbuy = value; }
        }
        public DateTime Panic_begintime
        {
            get { return panic_begintime; }
            set { panic_begintime = value; }
        }
        public DateTime Panicbuy_endtime
        {
            get { return panicbuy_endtime; }
            set { panicbuy_endtime = value; }
        }
        public int Limitbuytotalnum
        {
            get { return limitbuytotalnum; }
            set { limitbuytotalnum = value; }
        }

        public int ishasdeliveryfee { get; set; }
        public int deliverytmp { get; set; }
        public int unsure { get; set; }
        public int unyuyueyanzheng { get; set; }

        public int Wrentserver { get; set; }
        public int WDeposit { get; set; }
        public decimal Depositprice { get; set; }
        public string Rentserverid { get; set; }
        public string bandingzhajiid { get; set; }
        public string merchant_code { get; set; }

        
        private B2b_com_housetype housetype;

        public B2b_com_pro() { }

        public B2b_com_housetype Housetype
        {
            get { return housetype; }
            set { housetype = value; }
        }

        public string MultiImgUpIds
        {
            get;
            set;
        }


        public int Travelproductid
        {
            get { return travelproductid; }
            set { travelproductid = value; }
        }
        public int Traveltype
        {
            get { return traveltype; }
            set { traveltype = value; }
        }
        public string Travelstarting
        {
            get { return travelstarting; }
            set { travelstarting = value; }
        }
        public string Travelending
        {
            get { return travelending; }
            set { travelending = value; }
        }




        private decimal pro_Integral;//产品反积分

        public decimal Pro_Integral
        {
            get { return pro_Integral; }
            set { pro_Integral = value; }
        }

        private int sortid;//产品排序

        public int Sortid
        {
            get { return sortid; }
            set { sortid = value; }
        }
        public int Projectid
        {
            get { return projectid; }
            set { projectid = value; }
        }
        public int Proclass
        {
            get { return proclass; }
            set { proclass = value; }
        }

        public string Pro_explain
        {
            get { return this.pro_explain; }
            set { this.pro_explain = value; }
        }

        public int Pro_number
        {
            get { return this.pro_number; }
            set { this.pro_number = value; }
        }

        public int Imgurl
        {
            get { return this.imgurl; }
            set { this.imgurl = value; }
        }
        public string Imgaddress
        {
            get { return this.imgaddress; }
            set { this.imgaddress = value; }
        }


        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }



        public int Com_id
        {
            get { return this.com_id; }
            set { this.com_id = value; }
        }


        public string Pro_name
        {
            get { return this.pro_name; }
            set { this.pro_name = value; }
        }


        public int Pro_state
        {
            get { return this.pro_state; }
            set { this.pro_state = value; }
        }


        public int Server_type
        {
            get { return this.server_type; }
            set { this.server_type = value; }
        }


        public int Pro_type
        {
            get { return this.pro_type; }
            set { this.pro_type = value; }
        }
        public int Source_type
        {
            get { return this.source_type; }
            set { this.source_type = value; }
        }
        public string Pro_Remark
        {
            get { return this.pro_Remark; }
            set { this.pro_Remark = value; }
        }


        public DateTime Pro_start
        {
            get { return this.pro_start; }
            set { this.pro_start = value; }
        }


        public DateTime Pro_end
        {
            get { return this.pro_end; }
            set { this.pro_end = value; }
        }


        public decimal Face_price
        {
            get { return this.face_price; }
            set { this.face_price = value; }
        }


        public decimal Advise_price
        {
            get { return this.advise_price; }
            set { this.advise_price = value; }
        }
        public decimal Agent1_price
        {
            get { return this.agent1_price; }
            set { this.agent1_price = value; }
        }
        public decimal Agent2_price
        {
            get { return this.agent2_price; }
            set { this.agent2_price = value; }
        }
        public decimal Agent3_price
        {
            get { return this.agent3_price; }
            set { this.agent3_price = value; }
        }


        public decimal Agentsettle_price
        {
            get { return this.agentsettle_price; }
            set { this.agentsettle_price = value; }
        }


        public int ThatDay_can
        {
            get { return this.thatDay_can; }
            set { this.thatDay_can = value; }
        }


        public int Thatday_can_day
        {
            get { return this.thatday_can_day; }
            set { this.thatday_can_day = value; }
        }


        public string Service_Contain
        {
            get { return this.service_Contain; }
            set { this.service_Contain = value; }
        }


        public string Service_NotContain
        {
            get { return this.service_NotContain; }
            set { this.service_NotContain = value; }
        }


        public string Precautions
        {
            get { return this.precautions; }
            set { this.precautions = value; }
        }


        public int Tuan_pro
        {
            get { return this.tuan_pro; }
            set { this.tuan_pro = value; }
        }


        public int Zhixiao
        {
            get { return this.zhixiao; }
            set { this.zhixiao = value; }
        }


        public int Agentsale
        {
            get { return this.agentsale; }
            set { this.agentsale = value; }
        }
        public int Createuserid
        {
            get { return this.createuserid; }
            set { this.createuserid = value; }
        }
        public DateTime Createtime
        {
            get { return this.createtime; }
            set { this.createtime = value; }
        }

        public string Sms
        {
            get { return this.sms; }
            set { this.sms = value; }
        }

        public int Totalpay
        {
            get { return this.totalpay; }
            set { this.totalpay = value; }
        }
        public int U_num
        {
            get { return this.u_num; }
            set { this.u_num = value; }
        }

        public decimal Totalprofit
        {
            get { return this.totalprofit; }
            set { this.totalprofit = value; }
        }

        public decimal Totalpay_price
        {
            get { return this.totalpay_price; }
            set { this.totalpay_price = value; }
        }
        public int Tuipiao
        {
            get { return this.tuipiao; }
            set { this.tuipiao = value; }
        }
        public int Tuipiao_guoqi
        {
            get { return this.tuipiao_guoqi; }
            set { this.tuipiao_guoqi = value; }
        }
        public int Tuipiao_endday
        {
            get { return this.tuipiao_endday; }
            set { this.tuipiao_endday = value; }
        }



        private int serviceid;//服务商id

        public int Serviceid
        {
            get { return serviceid; }
            set { serviceid = value; }
        }
        private string service_proid;//服务商产品编号

        public string Service_proid
        {
            get { return service_proid; }
            set { service_proid = value; }
        }
        private int realnametype;//实名制类型

        public int Realnametype
        {
            get { return realnametype; }
            set { realnametype = value; }
        }

        public string Pro_youxiaoqi
        {
            get { return this.pro_youxiaoqi; }
            set { this.pro_youxiaoqi = value; }
        }

        public int Bindingid
        {
            get { return this.bindingid; }
            set { this.bindingid = value; }
        }
        public int Manyspeci
        {
            get { return this.manyspeci; }
            set { this.manyspeci = value; }
        }

        public int Cartid
        {
            get { return this.cartid; }
            set { this.cartid = value; }
        }

        public int Speciid
        {
            get { return this.speciid; }
            set { this.speciid = value; }
        }


        public string dropoffpoint { get; set; }
        public string pickuppoint { get; set; }
        public string pro_note { get; set; }

        ////多规格 
        //public List<B2b_com_pro_Speci> listSpeci { get; set; }
        //public List<B2b_com_pro_Specitype> listSpecitype { get; set; }
        //public List<B2b_com_pro_Specitypevalue> listSpecitypevalue { get; set; }
        public string guigestr { get; set; }

    }

    /// <summary>
    /// 产品分组实体类
    /// </summary>
    public class B2b_com_pro_group
    {
        public int id { get; set; }
        public string groupname { get; set; }
        public int runstate { get; set; }
        public int comid { get; set; }
    }
}
