using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    [Serializable()]
    public class B2b_Rentserver_User
    {
        public int id { get; set; }
        public int comid { get; set; }

        public string servername { get; set; }
        public string cardid { get; set; }
        
        public int oid { get; set; }
        public int Depositstate { get; set; }
        public int serverstate { get; set; }

        public decimal Depositprice { get; set; }
        public int Depositorder { get; set; }
        public string Depositcome { get; set; }
        public DateTime subtime { get; set; }
        public string cardchipid { get; set; }
        public int eticketid { get; set; }
        
        public string pname {  get; set;}
        public int cardyouxiaoqi{get;set;}
        public DateTime endtime {get; set; }
        public int sendnum { get; set; }
        public int usenum { get; set; }
        
        public int sendstate { get; set; }
    }
}
