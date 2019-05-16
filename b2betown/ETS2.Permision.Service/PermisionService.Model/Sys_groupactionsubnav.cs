using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.Permision.Service.PermisionService.Model
{
    public class Sys_groupactionsubnav
    {
        public Sys_groupactionsubnav() { }
        public int id { get; set; }
        public int groupid { get; set; }
        public int actionid { get; set; }
        public int subnavid { get; set; }
    }
}
