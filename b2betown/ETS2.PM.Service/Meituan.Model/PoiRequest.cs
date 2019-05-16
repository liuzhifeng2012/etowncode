using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Meituan.Model
{
    public class PoiRequest
    {
        public PoiRequest() { }
        public string partnerId { get; set; }
        public PoiRequestBody body { get; set; }
    }
    public class PoiRequestBody
    {
        public PoiRequestBody() { }
        public string method { get; set; }//拉取方法	page:分页查询 multi:单个查询
        public int currentPage { get; set; }//当前页数	当method=page 时必传
        public int pageSize { get; set; }//每页记录数	当method=page 时必传
        public List<string> partnerPoiId { get; set; }//POI ID数组	当method=multi 时必传 
    }
}
