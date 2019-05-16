using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    [Serializable()]
    public class B2b_com_LineGroupDate
    {
        private int id;
        private decimal menprice;
        private decimal childprice;
        private decimal oldmenprice;
        private int emptynum = 0;
        private DateTime daydate = DateTime.Parse(DateTime.Now.ToShortDateString());
        private int lineid;

        public decimal agent1_back { get; set; }
        public decimal agent2_back { get; set; }
        public decimal agent3_back { get; set; }

        public B2b_com_LineGroupDate() { }

        //新增字段，支付成功数量
        public int paysucnum { get; set; }
        //新增字段，待支付数量
        public int waitpaynum { get; set; }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public decimal Menprice
        {
            get { return this.menprice; }
            set { this.menprice = value; }
        }
        public decimal Childprice
        {
            get { return this.childprice; }
            set { this.childprice = value; }
        }
        public decimal Oldmenprice
        {
            get { return this.oldmenprice; }
            set { this.oldmenprice = value; }
        }
        public int Emptynum
        {
            get { return this.emptynum; }
            set { this.emptynum = value; }
        }
        public DateTime Daydate
        {
            get { return this.daydate; }
            set { this.daydate = value; }
        }
        public int Lineid
        {
            get { return this.lineid; }
            set { this.lineid = value; }
        }
    }
}
