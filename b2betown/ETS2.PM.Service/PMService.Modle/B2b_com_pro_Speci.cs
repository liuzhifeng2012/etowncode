using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class B2b_com_pro_Speci
    {
        public B2b_com_pro_Speci() { }
        public int id { get; set; }
        public string speci_name { get; set; }
        public decimal speci_face_price { get; set; }
        public decimal speci_advise_price { get; set; }
        public decimal speci_agent1_price { get; set; }
        public decimal speci_agent2_price { get; set; }
        public decimal speci_agent3_price { get; set; }
        public decimal speci_agentsettle_price { get; set; }
        public decimal speci_pro_weight { get; set; }
        public int speci_totalnum { get; set; }
        public int comid { get; set; }
        public int proid { get; set; }
        public string speci_type_nameid_Array { get; set; }
        public int binding_id { get; set; }
      
    }
}
