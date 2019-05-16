using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    [Serializable()]
    public class B2b_com_roomtypeimg
    {

        private int id;
        private int imgid;
        private string imgremark = String.Empty;
        private int roomtypeid;



        public B2b_com_roomtypeimg() { }




        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }





        public int Imgid
        {
            get { return this.imgid; }
            set { this.imgid = value; }
        }


        public string Imgremark
        {
            get { return this.imgremark; }
            set { this.imgremark = value; }
        }


        public int Roomtypeid
        {
            get { return this.roomtypeid; }
            set { this.roomtypeid = value; }
        }

    }
}
