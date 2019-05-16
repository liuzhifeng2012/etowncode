using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    [Serializable()]
    public class Rentserver_User_zhajilog
    {
        public int id { get; set; }
        public int comid { get; set; }
        public int proid{ get; set; }
        public int oid{ get; set; }
        public int Rentserver_Userid{ get; set; }
        public DateTime subtime { get; set; }
        public string clearchipid { get; set; }
        public string pos_id { get; set; }
    }
}
