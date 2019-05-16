using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    [Serializable()]
    public class RandomCode
    {

        private int id;
        private int code;
        private int state;



        public RandomCode() { }




        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }





        public int Code
        {
            get { return this.code; }
            set { this.code = value; }
        }


        public int State
        {
            get { return this.state; }
            set { this.state = value; }
        }

    }
}
