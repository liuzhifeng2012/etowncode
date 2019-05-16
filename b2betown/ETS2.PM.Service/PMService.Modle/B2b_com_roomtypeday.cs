using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    [Serializable()]
    public class B2b_com_roomtypeday
    {

        private int id;
        private decimal dayprice;
        private int dayavailablenum;
        private int reserveType;
        private DateTime daydate;
        private int roomtypeid;



        public B2b_com_roomtypeday() { }




        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }


        public int ReserveType
        {
            get { return this.reserveType; }
            set { this.reserveType = value; }
        }


        public decimal Dayprice
        {
            get { return this.dayprice; }
            set { this.dayprice = value; }
        }


        public int Dayavailablenum
        {
            get { return this.dayavailablenum; }
            set { this.dayavailablenum = value; }
        }


        public DateTime Daydate
        {
            get { return this.daydate; }
            set { this.daydate = value; }
        }


        public int Roomtypeid
        {
            get { return this.roomtypeid; }
            set { this.roomtypeid = value; }
        }

    }
}
