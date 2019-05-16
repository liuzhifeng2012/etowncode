using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{    /// <summary>
    /// 商家产品管理
    /// </summary>
    [Serializable()]
    public class B2b_Rentserver
    {
        public int id { get; set; }
        public int comid { get; set; }
        public string servername { get; set; }
        public int servertype { get; set; }
        public string renttype { get; set; }
        public int WR { get; set; }
        public int num { get; set; }
        public int posid { get; set; }
        public decimal saleprice { get; set; }
        public decimal serverDepositprice { get; set; }
        public int mustselect { get; set; }
        public int printticket { get; set; }//打印索道票
        public int Fserver { get; set; }//子服务，没有自己的押金，不需要选择，当购买主服务后子服务只做单独验证
        
    }
}
