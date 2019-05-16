using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.Member.Service.MemberService.Model
{
    [Serializable()]

    /// <summary>
    /// 摇奖活动,奖品随机码
    /// </summary>
    public class ERNIE_Record
    {
        private int id;
        private int com_id;
        private int eRNIE_id;
        private int eRNIE_uid;
        private string eRNIE_openid;
        private decimal eRNIE_code;
        private int winning_state;
        private DateTime eRNIE_time;
        private string phone;
        private string name;
        private string ip;
        private string address;
        private int awardid;
        private int process_state;


        public ERNIE_Record() { }


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
        public int ERNIE_id
        {
            get { return this.eRNIE_id; }
            set { this.eRNIE_id = value; }
        }
        public int ERNIE_uid
        {
            get { return this.eRNIE_uid; }
            set { this.eRNIE_uid = value; }
        }
        public string ERNIE_openid
        {
            get { return this.eRNIE_openid; }
            set { this.eRNIE_openid = value; }
        }
        public decimal ERNIE_code
        {
            get { return this.eRNIE_code; }
            set { this.eRNIE_code = value; }
        }
        public int Winning_state
        {
            get { return this.winning_state; }
            set { this.winning_state = value; }
        }
        public DateTime ERNIE_time
        {
            get { return this.eRNIE_time; }
            set { this.eRNIE_time = value; }
        }

        public string Phone
        {
            get { return this.phone; }
            set { this.phone = value; }
        }
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        public string Ip
        {
            get { return this.ip; }
            set { this.ip = value; }
        }
        public string Address
        {
            get { return this.address; }
            set { this.address = value; }
        }
        public int Awardid
        {
            get { return this.awardid; }
            set { this.awardid = value; }
        }
        
        public int Process_state
        {
            get { return this.process_state; }
            set { this.process_state = value; }
        }


    }
}
