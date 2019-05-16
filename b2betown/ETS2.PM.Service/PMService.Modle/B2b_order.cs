using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{

    [Serializable()]
    public class B2b_order
    {
        //是否是分销接口出票
        public int isInterfaceSub { get; set; }

        //乘车人信息
        public string travelnames { get; set; }
        public string travelphones { get; set; }
        public string travelidcards { get; set; }
        public string travelnations { get; set; }
        public string travelremarks { get; set; }

        public int ignoredabatime { get; set; }//忽略发车前30分钟不可提单的限制
        public int givebaoxianorderid { get; set; }//绑定保险订单id

        public int aorderid { get; set; }//a订单id
        public int recommendchannelid { get; set; }//推荐人渠道id
        public string qunar_orderid { get; set; }
        private int id;
        private int pro_id;
        private int agentid;//分销ID
        private int warrantid;//授权ID
        private int agentsunid;
        private int warrant_type;//支付类型分销 1出票扣款 2验码扣款
        private int order_type;
        private string u_name = String.Empty;
        private int u_id;
        private string u_phone = String.Empty;
        private string u_idcard = String.Empty;
        private int u_num;
        private int countnum;
        private string datestr;
        public  decimal childreduce{get;set;}
        public int yanzheng_method { get; set; }
        public string payserverpno { get; set; }
        public string serverid { get; set; }
        public int payorder { get; set; }

        public string service_code;
        public int service_usecount;
        public int service_lastcount;
       

        public string service_order_num = "";
        public string service_req_seq = "";
        public string order_remark = "";
        public string servicepro_v_state = "";
      

        private DateTime u_subdate;
        private int payid;
        private decimal pay_price;
        private decimal cost;
        private decimal profit;
        private int order_state;
        private int pay_state;
        private int send_state;
        private int ticketcode;
        private decimal rebate;
        private string ordercome = String.Empty;
        private string dealer = String.Empty;
        private DateTime u_traveldate;
        private string pno = String.Empty;

        public string openid = String.Empty;
        private int tuipiao;
        private DateTime tuipiao_endtime;


        

        public int buyuid;
        public int tocomid;
        public int bindingagentorderid;
        private B2b_order_hotel m_b2b_order_hotel = null;//酒店订单附属表

        private int child_U_num;

        public int Cancelnum;//退票数量


        public string province ;//省市
        public string city ;//城市
        public string address ;//配送地址
        public string code ;//邮编
        public decimal express;//快递费

        public string expresscom;
        public string expresscode;
        public int deliverytype;
        public int shopcartid;
        public int handlid;
        public int speciid;
        public int cartid;
        public int channelcoachid { get; set; }

        //新增可选参数:上车地点,下车地点
        public string pickuppoint { get; set; }
        public string dropoffpoint { get; set; }

        //申请退票信息
        public decimal askquitfee { get; set; }
        public string askquitfeereason { get; set; }
        public string askquitfeeexplain { get; set; }
        public int askquitnum { get; set; }


        //慧择网保险设计的字段
        public string baoxiannames { get; set; }
        public string baoxianpinyinnames { get; set; }
        public string baoxianidcards { get; set; }
        public int autosuccess { get; set; }
        public string submanagename { get; set; }
        public int mangefinset { get; set; }//财务结算 前台收款，与服务商结算都可以使用，默认为0 不需要结算,1=需要结算，2=已结算。
        public string yuyuepno { get; set; }
        public int yuyueweek { get; set; } 
        public string sid { get; set; }

        //产品关联人姓名 ;关联人手机（给预订发通知）;预订前是否支付
        public string bookpro_bindphone { get; set; }
        public string bookpro_bindname { get; set; }
        public string bookpro_bindcompany { get; set; }
        public DateTime bookpro_bindconfirmtime { get; set; }



        public string Pro_name { get; set; }
        public string Imgurl { get; set; }
        public decimal sumprice { get; set; }

        public int Server_type { get; set; }



        public int Child_u_num 
        {
            get { return this.child_U_num; }
            set { this.child_U_num = value; }
        }

        public B2b_order() { }

        public B2b_order_hotel M_b2b_order_hotel 
        {
            get { return m_b2b_order_hotel; }
            set { this.m_b2b_order_hotel = value; }
        }


        private string ticketinfo;//退款备注

        public string Ticketinfo
        {
            get { return ticketinfo; }
            set { ticketinfo = value; }
        }
        private DateTime backtickettime;//退款提交时间

        public DateTime Backtickettime
        {
            get { return backtickettime; }
            set { backtickettime = value; }
        }
        private decimal ticket;//退款状态

        public decimal Ticket
        {
            get { return ticket; }
            set { ticket = value; }
        }

        private decimal Integral;

        public decimal Integral1
        {
            get { return Integral; }
            set { Integral = value; }
        }

        private decimal Imprest;

        public decimal Imprest1
        {
            get { return Imprest; }
            set { Imprest = value; }
        }

        public string Openid
        {
            get { return this.openid; }
            set { this.openid = value; }
        }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }


        public int Pro_id
        {
            get { return this.pro_id; }
            set { this.pro_id = value; }
        }


        public int Order_type
        {
            get { return this.order_type; }
            set { this.order_type = value; }
        }


        public string U_name
        {
            get { return this.u_name; }
            set { this.u_name = value; }
        }

        public string Pno
        {
            get { return this.pno; }
            set { this.pno = value; }
        }
        public int U_id
        {
            get { return this.u_id; }
            set { this.u_id = value; }
        }


        public string U_phone
        {
            get { return this.u_phone; }
            set { this.u_phone = value; }
        }
        public string U_idcard
        {
            get { return this.u_idcard; }
            set { this.u_idcard = value; }
        }

        public int U_num
        {
            get { return this.u_num; }
            set { this.u_num = value; }
        }

        public int Countnum
        {
            get { return this.countnum; }
            set { this.countnum = value; }
        }

        public string Datestr
        {
            get { return this.datestr; }
            set { this.datestr = value; }
        }

        public DateTime U_subdate
        {
            get { return this.u_subdate; }
            set { this.u_subdate = value; }
        }


        public int Payid
        {
            get { return this.payid; }
            set { this.payid = value; }
        }


        public decimal Pay_price
        {
            get { return this.pay_price; }
            set { this.pay_price = value; }
        }


        public decimal Cost
        {
            get { return this.cost; }
            set { this.cost = value; }
        }


        public decimal Profit
        {
            get { return this.profit; }
            set { this.profit = value; }
        }


        public int Order_state
        {
            get { return this.order_state; }
            set { this.order_state = value; }
        }


        public int Pay_state
        {
            get { return this.pay_state; }
            set { this.pay_state = value; }
        }


        public int Send_state
        {
            get { return this.send_state; }
            set { this.send_state = value; }
        }


        public int Ticketcode
        {
            get { return this.ticketcode; }
            set { this.ticketcode = value; }
        }


        public decimal Rebate
        {
            get { return this.rebate; }
            set { this.rebate = value; }
        }


        public string Ordercome
        {
            get { return this.ordercome; }
            set { this.ordercome = value; }
        }
        public string Dealer
        {
            get { return this.dealer; }
            set { this.dealer = value; }
        }
        public DateTime U_traveldate
        {
            get { return this.u_traveldate; }
            set { this.u_traveldate = value; }
        }

        private int comid;

        public int Comid
        {
            get { return comid; }
            set { comid = value; }
        }
        public int Agentid
        {
            get { return agentid; }
            set { agentid = value; }
        }

        public int Agentsunid
        {
            get { return agentsunid; }
            set { agentsunid = value; }
        }

        public int Warrantid
        {
            get { return warrantid; }
            set { warrantid = value; }
        }
        public int Warrant_type
        {
            get { return warrant_type; }
            set { warrant_type = value; }
        }
        public int Tuipiao
        {
            get { return tuipiao; }
            set { tuipiao = value; }
        }
        public DateTime Tuipiao_endtime
        {
            get { return tuipiao_endtime; }
            set { tuipiao_endtime = value; }
        }

        public string Service_order_num
        {
            get { return this.service_order_num; }
            set { this.service_order_num = value; }
        }
        public string Service_req_seq
        {
            get { return this.service_req_seq; }
            set { this.service_req_seq = value; }
        }
        public string Order_remark
        {
            get { return this.order_remark; }
            set { this.order_remark = value; }
        }

        public string Servicepro_v_state
        {
            get { return this.servicepro_v_state; }
            set { this.servicepro_v_state = value; }
        }

        public int Buyuid
        {
            get { return this.buyuid; }
            set { this.buyuid = value; }
        }
        public int Tocomid
        {
            get { return this.tocomid; }
            set { this.tocomid = value; }
        }
        public int Bindingagentorderid
        {
            get { return this.bindingagentorderid; }
            set { this.bindingagentorderid = value; }
        }


        public string Province
        {
            get { return this.province; }
            set { this.province = value; }
        }
        public string City
        {
            get { return this.city; }
            set { this.city = value; }
        }

        public string Address
        {
            get { return this.address; }
            set { this.address = value; }
        }

        public string Code
        {
            get { return this.code; }
            set { this.code = value; }
        }

        public decimal Express
        {
            get { return this.express; }
            set { this.express = value; }
        }


        public string Expresscom
        {
            get { return this.expresscom; }
            set { this.expresscom = value; }
        }
        public string Expresscode
        {
            get { return this.expresscode; }
            set { this.expresscode = value; }
        }
        public int Deliverytype
        {
            get { return this.deliverytype; }
            set { this.deliverytype = value; }
        }

        public int Shopcartid
        {
            get { return this.shopcartid; }
            set { this.shopcartid = value; }
        }

        public int Handlid
        {
            get { return this.handlid; }
            set { this.handlid = value; }
        }

        public int Speciid
        {
            get { return this.speciid; }
            set { this.speciid = value; }
        }
        public int Cartid
        {
            get { return this.cartid; }
            set { this.cartid = value; }
        }
        
    }
}
