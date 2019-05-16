using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.Permision.Service.PermisionService.Model
{
    public class Sys_subnav
    {
        public Sys_subnav() { }

        public int id { get; set; }
        public string subnav_name { get; set; }
        public string subnav_url { get; set; }
        public int viewcode { get; set; }
        public int sortid { get; set; }
        public int actioncolumnid { get; set; }
        public int actionid { get; set; }


    }
}
