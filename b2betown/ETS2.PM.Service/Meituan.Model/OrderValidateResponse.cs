using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Meituan.Model
{
    #region 下单前预约 Response 已经弃用
    public class OrderValidateResponse
    {
        public int code { get; set; }
        public string describe { get; set; }
        public int partnerId { get; set; }
        public OrderValidateResponseBody body { get; set; }

    }
    public class OrderValidateResponseBody
    {
        public int orderStatus { get; set; }
    }
    #endregion
}
