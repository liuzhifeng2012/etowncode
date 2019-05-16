using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{
    /// <summary>
    /// 商家信息拓展表
    /// </summary>
    [Serializable()]
    public class B2b_company_info
    {

        private int id;
        private int com_id;
        private string com_city = String.Empty;
        private string com_add = String.Empty;
        private int com_class;
        private string com_code = String.Empty;
        private string com_sitecode = String.Empty;
        private string com_license = String.Empty;
        private string sale_Agreement = String.Empty;
        private string agent_Agreement = String.Empty;
        private string scenic_address = String.Empty;
        private string scenic_intro = String.Empty;
        private string scenic_Takebus = String.Empty;
        private string scenic_Drivingcar = String.Empty;
        private string contact = String.Empty;
        private string tel = String.Empty;
        private string phone = String.Empty;
        private string qq = String.Empty;
        private string email = String.Empty;
        private string defaultprint = String.Empty;
        private string pos = String.Empty;
        private int posid;
        private int projectid;
        private string poscompany = String.Empty;
        private DateTime bindingTime;
        private string admin = String.Empty;
        private string remark = String.Empty;


        private string serviceinfo = String.Empty;
        private string coordinate = String.Empty;
        private int coordinatesize;
        private string domainname = String.Empty;
        private string admindomain = String.Empty;
        private string merchant_intro = String.Empty;


        private string province = String.Empty;
        private string city = String.Empty;

        private string wxfocus_author = String.Empty;
        private string wxfocus_url = String.Empty;

        private int istransfer_customer_service;

        public string md5key { get; set; }

        public B2b_company_info() { }

        public int Istransfer_customer_service
        {
            get { return this.istransfer_customer_service; }
            set { this.istransfer_customer_service = value; }
        }


        public string Wxfocus_author
        {
            get { return this.wxfocus_author; }
            set { this.wxfocus_author = value; }
        }

        public string Wxfocus_url
        {
            get { return this.wxfocus_url; }
            set { this.wxfocus_url = value; }
        }


        public string Merchant_intro
        {
            get { return this.merchant_intro; }
            set { this.merchant_intro = value; }
        }


        private bool hasInnerChannel;//是否含有所属门市

        public bool HasInnerChannel
        {
            get { return hasInnerChannel; }
            set { hasInnerChannel = value; }
        }


        private int info_state;//屏蔽0,显示1

        public int Info_state
        {
            get { return info_state; }
            set { info_state = value; }
        }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public int Coordinatesize
        {
            get { return this.coordinatesize; }
            set { this.coordinatesize = value; }
        }


        public string wl_PartnerId { get; set; }
        public string wl_userkey { get; set; }
       


        public int Com_id
        {
            get { return this.com_id; }
            set { this.com_id = value; }
        }


        public string Com_city
        {
            get { return this.com_city; }
            set { this.com_city = value; }
        }




        public string Serviceinfo
        {
            get { return this.serviceinfo; }
            set { this.serviceinfo = value; }
        }
        public string Coordinate
        {
            get { return this.coordinate; }
            set { this.coordinate = value; }
        }
        public string Domainname
        {
            get { return this.domainname; }
            set { this.domainname = value; }
        }

        public string Admindomain
        {
            get { return this.admindomain; }
            set { this.admindomain = value; }
        }

        



        public string Com_add
        {
            get { return this.com_add; }
            set { this.com_add = value; }
        }


        public int Com_class
        {
            get { return this.com_class; }
            set { this.com_class = value; }
        }


        public string Com_code
        {
            get { return this.com_code; }
            set { this.com_code = value; }
        }


        public string Com_sitecode
        {
            get { return this.com_sitecode; }
            set { this.com_sitecode = value; }
        }


        public string Com_license
        {
            get { return this.com_license; }
            set { this.com_license = value; }
        }


        public string Sale_Agreement
        {
            get { return this.sale_Agreement; }
            set { this.sale_Agreement = value; }
        }


        public string Agent_Agreement
        {
            get { return this.agent_Agreement; }
            set { this.agent_Agreement = value; }
        }


        public string Scenic_address
        {
            get { return this.scenic_address; }
            set { this.scenic_address = value; }
        }


        public string Scenic_intro
        {
            get { return this.scenic_intro; }
            set { this.scenic_intro = value; }
        }


        public string Scenic_Takebus
        {
            get { return this.scenic_Takebus; }
            set { this.scenic_Takebus = value; }
        }


        public string Scenic_Drivingcar
        {
            get { return this.scenic_Drivingcar; }
            set { this.scenic_Drivingcar = value; }
        }


        public string Contact
        {
            get { return this.contact; }
            set { this.contact = value; }
        }


        public string Tel
        {
            get { return this.tel; }
            set { this.tel = value; }
        }


        public string Phone
        {
            get { return this.phone; }
            set { this.phone = value; }
        }


        public string Qq
        {
            get { return this.qq; }
            set { this.qq = value; }
        }


        public string Email
        {
            get { return this.email; }
            set { this.email = value; }
        }


        public string Defaultprint
        {
            get { return this.defaultprint; }
            set { this.defaultprint = value; }
        }

        public string Pos
        {
            get { return this.pos; }
            set { this.pos = value; }
        }

        public int Posid
        {
            get { return this.posid; }
            set { this.posid = value; }
        }
        public int Projectid
        {
            get { return this.projectid; }
            set { this.projectid = value; }
        }

        
        public string Poscompany
        {
            get { return this.poscompany; }
            set { this.poscompany = value; }
        }

        public DateTime BindingTime
        {
            get { return this.bindingTime; }
            set { this.bindingTime = value; }
        }

        public string Admin
        {
            get { return this.admin; }
            set { this.admin = value; }
        }

        public string Remark
        {
            get { return this.remark; }
            set { this.remark = value; }
        }

        private string weixinimg;

        public string Weixinimg
        {
            get { return weixinimg; }
            set { weixinimg = value; }
        }
        private string weixinname;

        public string Weixinname
        {
            get { return weixinname; }
            set { weixinname = value; }
        }


        private decimal sjZxsk = 0;
        /// <summary>
        /// 直销收款
        /// </summary>
        public decimal SjZxsk
        {
            get { return this.sjZxsk; }
            set { this.sjZxsk = value; }
        }
        private decimal sjZxtp = 0;
        /// <summary>
        /// 直销退票
        /// </summary>
        public decimal SjZxtp
        {
            get { return this.sjZxtp; }
            set { this.sjZxtp = value; }
        }
        private decimal sjTx = 0;
        /// <summary>
        /// 商家提现
        /// </summary>
        public decimal SjTx
        {
            get { return this.sjTx; }
            set { this.sjTx = value; }
        }
        private decimal sjZsjalipay = 0;
        /// <summary>
        /// 转商家支付宝
        /// </summary>
        public decimal SjZsjalipay
        {
            get { return this.sjZsjalipay; }
            set { this.sjZsjalipay = value; }
        }
        private decimal sjSxf = 0;
        /// <summary>
        /// 手续费
        /// </summary>
        public decimal SjSxf
        {
            get { return this.sjSxf; }
            set { this.sjSxf = value; }
        }

        public string Province
        {
            get { return province; }
            set { province = value; }
        }
         public string City
        {
            get { return city; }
            set { city = value; }
        }
        


    }
}