using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    /// <summary>
    /// 套票绑定产品
    /// </summary>
    [Serializable()]
    public class B2b_com_pro_Package
    {
        public int Id { get; set; }//提单时是否设置
        public int Fid { get; set; }//提单时是否设置
        public int Sid { get; set; }//提单时是否设置
        public int Snum { get; set; }//提单时是否设置

    }
}
