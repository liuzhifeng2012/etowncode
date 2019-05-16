using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.VAS.Service.VASService.Modle
{
    public class Agent_asyncsendlog
    {
        public int isreissue{get;set;} //是否补发过
        private int id;
        private string pno;
        private int num;
        private DateTime confirmtime;//电子票验证时间
        private DateTime sendtime;
        private int issendsuc;
        private int agentcomid;
        private int agentupdatestatus;
        private int comid;
        private string remark;
        private int issecondsend;//是否是第一次验证请求发送失败后第二次发送

        private string platform_req_seq;//平台请求流水号

        private string request_content;
        private string response_content;

        public int b2b_etcket_logid { get; set; }

        public string Request_content
        {
            get;
            set;
        }
        public string Response_content
        {
            get;
            set;
        }

        public Agent_asyncsendlog() { }
        public string Platform_req_seq
        {
            get { return this.platform_req_seq; }
            set { this.platform_req_seq = value; }
        }
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public int Issecondsend
        {
            get { return this.issecondsend; }
            set { this.issecondsend = value; }
        }

        public int Comid
        {
            get { return this.comid; }
            set { this.comid = value; }
        }
        public int Agentupdatestatus
        {
            get { return this.agentupdatestatus; }
            set { this.agentupdatestatus = value; }
        }
        public string Pno
        {
            get { return this.pno; }
            set { this.pno = value; }
        }
        public string Remark
        {
            get { return this.remark; }
            set { this.remark = value; }
        }
        public int Num
        {
            get { return this.num; }
            set { this.num = value; }
        }

        public DateTime Sendtime
        {
            get { return this.sendtime; }
            set { this.sendtime = value; }
        }
        public DateTime Confirmtime
        {
            get { return this.confirmtime; }
            set { this.confirmtime = value; }
        }
        public int Issendsuc
        {
            get { return this.issendsuc; }
            set { this.issendsuc = value; }
        }
        public int Agentcomid
        {
            get { return this.agentcomid; }
            set { this.agentcomid = value; }
        }
    }



}
