using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{    /// <summary>
    /// 分销商基本信息表
    /// </summary>
    [Serializable()]
    public class Agent_company
    {
        //分销通知发送方式（get,post）,默认post
        public string inter_sendmethod { get; set; }

        public int istaobao { get; set; }
        public string tb_syncurl { get; set; }
        public int tb_isret_consumeresult { get; set; }
        public int maxremindmoney { get; set; }
    
        //增加美团合作商配置信息
        public int ismeituan { get; set; }
        public string mt_partnerId { get; set; }
        public string mt_client { get; set; }
        public string mt_secret { get; set; }
        public string mt_mark { get; set; }


        //增加驴妈妈
        public string Lvmama_uid { get; set; }
        public string Lvmama_password { get; set; }
        public string Lvmama_Apikey { get; set; }


        private int id;
        private int comid;
        private string company;
        private string name;
        private string tel;
        private string mobile;
        private string contentname;
        private string address;
        private int run_state;
        private decimal imprest;
        private decimal credit;
        private int warrant_type;
        private int warrant_state;
        private int warrant_level;
        private string account;
        private string pwd;
        private int accountLevel;

        private decimal amount;
        private int warrantid;
        private int agentsort;
        private string agent_domain;
        private int weixin_img;
        private int account_type;//账户类型，是分销账户还是，项目的商户账户
        private int projectid;
        private decimal rebatemoney;

        private int warrant_lp;


        public int Agent_messagesetting { get; set; }//分销短信发送设置
        public string com_province { get; set; }
        public string com_city { get; set; }

        public int agentsourcesort { get; set; }
        private int sms;
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public int Sms
        {
            get { return this.sms; }
            set { this.sms = value; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        public int Projectid
        {
            get { return this.projectid; }
            set { this.projectid = value; }
        }
        public int Account_type
        {
            get { return this.account_type; }
            set { this.account_type = value; }
        }

        public int Warrantid
        {
            get { return this.warrantid; }
            set { this.warrantid = value; }
        }

        public int Comid
        {
            get { return this.comid; }
            set { this.comid = value; }
        }

        public int Run_state
        {
            get { return this.run_state; }
            set { this.run_state = value; }
        }

        public decimal Imprest
        {
            get { return this.imprest; }
            set { this.imprest = value; }
        }
        public decimal Credit
        {
            get { return this.credit; }
            set { this.credit = value; }
        }
        public decimal Rebatemoney
        {
            get { return this.rebatemoney; }
            set { this.rebatemoney = value; }
        }
        
        public string Company
        {
            get { return this.company; }
            set { this.company = value; }
        }

        public string Tel
        {
            get { return this.tel; }
            set { this.tel = value; }
        }

        public string Mobile
        {
            get { return this.mobile; }
            set { this.mobile = value; }
        }

        public string Contentname
        {
            get { return this.contentname; }
            set { this.contentname = value; }
        }

        public string Address
        {
            get { return this.address; }
            set { this.address = value; }
        }

        public string Account
        {
            get { return this.account; }
            set { this.account = value; }
        }
        public string Pwd
        {
            get { return this.pwd; }
            set { this.pwd = value; }
        }
        public int AccountLevel
        {
            get { return this.accountLevel; }
            set { this.accountLevel = value; }
        }
        public decimal Amount
        {
            get { return this.amount; }
            set { this.amount = value; }
        }

        public int Warrant_type
        {
            get { return this.warrant_type; }
            set { this.warrant_type = value; }
        }
        public int Warrant_state
        {
            get { return this.warrant_state; }
            set { this.warrant_state = value; }
        }
        public int Warrant_level
        {
            get { return this.warrant_level; }
            set { this.warrant_level = value; }
        }


        private string inter_password;
        private string inter_deskey;
        private string agent_queryurl;
        private string agent_updateurl;
        private string agent_auth_username;
        private string agent_auth_token;
        private int agent_type;
        private string agent_bindcomname;




        public string Inter_password
        {
            get { return this.inter_password; }
            set { this.inter_password = value; }
        }
        public string Inter_deskey
        {
            get { return this.inter_deskey; }
            set { this.inter_deskey = value; }
        }
        public string Agent_queryurl
        {
            get { return this.agent_queryurl; }
            set { this.agent_queryurl = value; }
        }
        public string Agent_updateurl
        {
            get { return this.agent_updateurl; }
            set { this.agent_updateurl = value; }
        }
        public string Agent_auth_username
        {
            get { return this.agent_auth_username; }
            set { this.agent_auth_username = value; }
        }
        public string Agent_auth_token
        {
            get { return this.agent_auth_token; }
            set { this.agent_auth_token = value; }
        }
        public int Agent_type
        {
            get { return this.agent_type; }
            set { this.agent_type = value; }
        }
        public string Agent_bindcomname
        {
            get { return this.agent_bindcomname; }
            set { this.agent_bindcomname = value; }
        }
        public int Agentsort
        {
            get { return this.agentsort; }
            set { this.agentsort = value; }
        }

        public int Weixin_img
        {
            get { return this.weixin_img; }
            set { this.weixin_img = value; }
        }
        public string Agent_domain
        {
            get { return this.agent_domain; }
            set { this.agent_domain = value; }
        }
        public int Warrant_lp
        {
            get { return this.warrant_lp; }
            set { this.warrant_lp = value; }
        }

        

    }
}
