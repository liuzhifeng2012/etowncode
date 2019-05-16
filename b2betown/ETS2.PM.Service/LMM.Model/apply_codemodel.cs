using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.LMM.Model
{
    public class apply_codemodel
    {
        public int id { get; set; }
        public int comid { get; set; }
        public int orderid { get; set; }

        public string uid { get; set; }
        public string password { get; set; }
        public string timestamp { get; set; }
        public string visitTime { get; set; }

        public string supplierGoodsId { get; set; }
        public string settlePrice { get; set; }

        public string num { get; set; }
        public string serialNo { get; set; }

        public string sign { get; set; }

        public string idNum { get; set; }
        public string idType { get; set; }
        public string mobile { get; set; }
        public string name { get; set; }

        public contacts contacts  { get; set; }
        public travellerList[] travellerList { get; set; }
    }

    public class contacts
    {
        public contacts() {}
        public string idNum { get; set; }
        public string idType { get; set; }
        public string mobile { get; set; }
        public string name { get; set; }
    }

    public class travellerList
    {
        public travellerList() { }
        public string idNum { get; set; }
        public string idType { get; set; }
        public string mobile { get; set; }
        public string name { get; set; }
    }


}
