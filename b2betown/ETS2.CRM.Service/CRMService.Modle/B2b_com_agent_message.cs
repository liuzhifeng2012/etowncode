using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{
    /// <summary>
    /// 商家基本信息表
    /// </summary>
    [Serializable()]
    public class B2b_com_agent_message
    {
        private int id;
        private string title = String.Empty;
        private string message = String.Empty;
        private int state ;
        private int comid;
        private DateTime subtime;
        private int sendsms;
        private string smstext;


        public B2b_com_agent_message() { }


        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public int Sendsms
        {
            get { return this.sendsms; }
            set { this.sendsms = value; }
        }
        public string Smstext
        {
            get { return this.smstext; }
            set { this.smstext = value; }
        }


        public int State
        {
            get { return this.state; }
            set { this.state = value; }
        }
        public int Comid
        {
            get { return this.comid; }
            set { this.comid = value; }
        }
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }
        public string Message
        {
            get { return this.message; }
            set { this.message = value; }
        }
        public DateTime Subtime
        {
            get { return this.subtime; }
            set { this.subtime = value; }
        }

    }
}
