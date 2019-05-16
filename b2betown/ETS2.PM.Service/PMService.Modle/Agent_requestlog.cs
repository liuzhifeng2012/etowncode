using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class Agent_requestlog
    {
        private int id;
        private int organization;
        private string encode_requeststr;
        private string decode_requeststr;
        private DateTime request_time;
        private string encode_returnstr;
        private string decode_returnstr;
        private DateTime return_time;
        private string errmsg;
        private string request_type;
        private string req_seq;
        private string ordernum;
        private int is_dealsuc;
        private int is_second_receivereq;
        private string request_ip;


        public Agent_requestlog() { }




        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }



        public int Organization
        {
            get { return this.organization; }
            set { this.organization = value; }
        }


        public string Encode_requeststr
        {
            get { return this.encode_requeststr; }
            set { this.encode_requeststr = value; }
        }


        public string Decode_requeststr
        {
            get { return this.decode_requeststr; }
            set { this.decode_requeststr = value; }
        }


        public DateTime Request_time
        {
            get { return this.request_time; }
            set { this.request_time = value; }
        }



        public string Encode_returnstr
        {
            get { return this.encode_returnstr; }
            set { this.encode_returnstr = value; }
        }

        public string Decode_returnstr
        {
            get { return this.decode_returnstr; }
            set { this.decode_returnstr = value; }
        }
        public DateTime Return_time
        {
            get { return this.return_time; }
            set { this.return_time = value; }
        }
        public string Errmsg
        {
            get { return this.errmsg; }
            set { this.errmsg = value; }
        }
        public string Request_type
        {
            get { return this.request_type; }
            set { this.request_type = value; }
        }
        public string Req_seq
        {
            get { return this.req_seq; }
            set { this.req_seq = value; }
        }
        public string Ordernum
        {
            get { return this.ordernum; }
            set { this.ordernum = value; }
        }
        public int Is_dealsuc
        {
            get { return this.is_dealsuc; }
            set { this.is_dealsuc = value; }
        }
        public int Is_second_receivereq
        {
            get { return this.is_second_receivereq; }
            set { this.is_second_receivereq = value; }
        }
        public string Request_ip
        {
            get { return this.request_ip; }
            set { this.request_ip = value; }
        }

    }
}
