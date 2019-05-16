using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data
{
    public class Agent_requestlogData
    {
        public int Is_secondreq(string organization, string req_seq, string request_type)
        {
             using(var helper=new SqlHelper())
             {
                 int r = new Internalagent_requestlog(helper).Is_secondreq(organization, req_seq, request_type);
                 return r;
             }
        }

        public int Is_secondordernum(string organization, string ordernum, string request_type)
        {
            using (var helper = new SqlHelper())
            {
                int r = new Internalagent_requestlog(helper).Is_secondordernum(organization, ordernum, request_type);
                return r;
            }
        }

        public int Editagent_reqlog(Agent_requestlog reqlog)
        {
            using (var helper = new SqlHelper())
            {
                int r = new Internalagent_requestlog(helper).Editagent_reqlog(reqlog);
                return r;
            }
        }

        public bool Ismatch_ip(string organization, string Requestip)
        {
            using (var helper = new SqlHelper())
            {
                bool r = new Internalagent_requestlog(helper).Ismatch_ip(organization,Requestip);
                return r;
            }
        }
        /*根据订单号得到分销商订单请求记录*/
        public List<Agent_requestlog> GetAgent_requestlogByOrdernum(string ordernum, string request_type, string isdealsuc)
        {
            using (var helper = new SqlHelper())
            {
                List<Agent_requestlog> r = new Internalagent_requestlog(helper).GetAgent_requestlogByOrdernum(ordernum,request_type,isdealsuc);
                return r;
            }
        }

        /*根据订单号得到分销商订单请求记录 带 type*/
        public Agent_requestlog GetAgent_addorderlogByOrderIddaitype(string ordernum, string request_type, int isdealsuc)
        {
            using (var helper = new SqlHelper())
            {
                Agent_requestlog r = new Internalagent_requestlog(helper).GetAgent_addorderlogByOrderIddaitype(ordernum, request_type, isdealsuc);
                return r;
            }
        }

        /*根据订单号得到分销商订单请求记录*/
        public Agent_requestlog GetAgent_addorderlogByOrderId(string ordernum,  int isdealsuc)
        {
            using (var helper = new SqlHelper())
            {
                Agent_requestlog r = new Internalagent_requestlog(helper).GetAgent_addorderlogByOrderId(ordernum, isdealsuc);
                return r;
            }
        }

        public Agent_requestlog GetAgent_addorderlogByReq_seq(string organization, string req_seq)
        {
            using (var helper = new SqlHelper())
            {
                Agent_requestlog r = new Internalagent_requestlog(helper).GetAgent_addorderlogByReq_seq(organization, req_seq);
                return r;
            }
        }
        public Agent_requestlog GetAgent_addorderlogByReq_seq(string organization, string req_seq,int is_dealsuc)
        {
            using (var helper = new SqlHelper())
            {
                Agent_requestlog r = new Internalagent_requestlog(helper).GetAgent_addorderlogByReq_seq(organization, req_seq, is_dealsuc);
                return r;
            }
        }

        public bool Getisselforder(string organization, string order_num)
        {
            using (var helper = new SqlHelper())
            {
                bool r = new Internalagent_requestlog(helper).Getisselforder(organization,order_num);
                return r;
            }
        }
        public bool lvmamaGetisselforder(string organization, string order_num)
        {
            using (var helper = new SqlHelper())
            {
                bool r = new Internalagent_requestlog(helper).lvmamaGetisselforder(organization, order_num);
                return r;
            }
        }

        public Agent_requestlog GetAgent_cancelorderlogByReq_seq(string organization, string req_seq, int is_dealsuc)
        {
            using (var helper = new SqlHelper())
            {
                Agent_requestlog r = new Internalagent_requestlog(helper).GetAgent_cancelorderlogByReq_seq(  organization,   req_seq,  is_dealsuc);
                return r;
            }
        }

        public Agent_requestlog GetAgent_cdiscard_codelogByReq_seq(string organization, string req_seq, int is_dealsuc)
        {
            using (var helper = new SqlHelper())
            {
                Agent_requestlog r = new Internalagent_requestlog(helper).GetAgent_cdiscard_codelogByReq_seq(organization, req_seq, is_dealsuc);
                return r;
            }
        }

        public bool Getisselforderbyreq_sql(string organization, string req_seq)
        {
            using (var helper = new SqlHelper())
            {
                bool r = new Internalagent_requestlog(helper).Getisselforderbyreq_sql(organization, req_seq);
                return r;
            }
        }
    }
}
