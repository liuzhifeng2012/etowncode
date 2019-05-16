using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
      /// <summary>
    /// 商家产品绑定闸机
    /// </summary>
    [Serializable()]
    public class B2b_com_pro_bandingzhajipos
    {
        public int id { get; set; }//绑定id
        public int comid { get; set; }//商户
        public int proid { get; set; }//产品
        public string pos_id { get; set; }//闸机


    }

}
