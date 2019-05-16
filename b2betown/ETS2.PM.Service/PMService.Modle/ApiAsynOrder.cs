using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class ApiAsynOrder
    {
        private int id;
        private string req_seq;
        private string platform_req_seq;
        private string order_num;
        private int num;
        private DateTime use_time;
        private int serviceid;
        private int issecondsend;

        private int issuc;
        private int logid;


        public ApiAsynOrder() { }

        public int Issuc
        {
            get { return this.issuc; }
            set { this.issuc = value; }
        }

        public int Logid
        {
            get { return this.logid; }
            set { this.logid = value; }
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


        public string Req_seq
        {
            get { return this.req_seq; }
            set { this.req_seq = value; }
        }

        public string Platform_req_seq
        {
            get { return this.platform_req_seq; }
            set { this.platform_req_seq = value; }
        }
        public string Order_num
        {
            get { return this.order_num; }
            set { this.order_num = value; }
        }
        public int Num
        {
            get { return this.num; }
            set { this.num = value; }
        }
        public DateTime Use_time
        {
            get { return this.use_time; }
            set { this.use_time = value; }
        }
        public int Serviceid
        {
            get { return this.serviceid; }
            set { this.serviceid = value; }
        }
    }
}
