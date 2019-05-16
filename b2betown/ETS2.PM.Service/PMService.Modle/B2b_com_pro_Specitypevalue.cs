using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class B2b_com_pro_Specitypevalue
    {
        public B2b_com_pro_Specitypevalue() { }
        public int id { get; set; }
        public int comid { get; set; }
        public int typeid { get; set; }
        public string val_name { get; set; }
        public int proid { get; set; }
        public int isonline { get; set; }

        //展示用到
        public string Name { get; set; }
    }
}
