using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WeiXinService.Model
{
    public class Weixin_templatemsg_sendlog
    {
        private int id;
        public int Id { get { return this.id; } set { this.id = value; } }

        private string msg_send_content;
        public string Msg_send_content { get { return this.msg_send_content; } set { this.msg_send_content = value; } }

        private string touser;
        public string Touser { get { return this.touser; } set { this.touser = value; } }

        private string template_id;
        public string Template_id { get { return this.template_id; } set { this.template_id = value; } }

        private string url;
        public string Url { get { return this.url; } set { this.url = value; } }

        private DateTime msg_send_createtime;
        public DateTime Msg_send_createtime { get { return this.msg_send_createtime; } set { this.msg_send_createtime = value; } }

        private string msg_call_content;
        public string Msg_call_content { get { return this.msg_call_content; } set { this.msg_call_content = value; } }

        private string msg_call_errcode;
        public string Msg_call_errcode { get { return this.msg_call_errcode; } set { this.msg_call_errcode = value; } }

        private string msg_call_errmsg;
        public string Msg_call_errmsg { get { return this.msg_call_errmsg; } set { this.msg_call_errmsg = value; } }

        private string msgid;
        public string Msgid { get { return this.msgid; } set { this.msgid = value; } }

        private string msg_push_content;
        public string Msg_push_content { get { return this.msg_push_content; } set { this.msg_push_content = value; } }
       
        private string msg_push_CreateTime;
        public string Msg_push_CreateTime { get { return this.msg_push_CreateTime; } set { this.msg_push_CreateTime = value; } }
       
        private DateTime msg_push_CreateTime_format;
        public DateTime Msg_push_CreateTime_format { get { return this.msg_push_CreateTime_format; } set { this.msg_push_CreateTime_format = value; } }
       
        private string msg_push_status;
        public string Msg_push_status { get { return this.msg_push_status; } set { this.msg_push_status = value; } }

        private int orderid;
        public int Orderid { get { return this.orderid; } set { this.orderid = value; } }

        private string public_account;
        public string Public_account { get { return this.public_account; } set { this.public_account = value; } }
       
        private int comid;
        public int Comid { get { return this.comid; } set { this.comid = value; } }

        private string remark;
        public string Remark { get { return this.remark; } set { this.remark = value; } }

        private string infotype;
        public string Infotype { get { return this.infotype; } set { this.infotype = value; } }
    }
}
