using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Meituan.Model
{
    public class PoiResponse
    {
        public PoiResponse() { }
        public int code { get; set; }
        public string describe { get; set; }
        public int partnerId { get; set; }
        public int totalSize { get; set; } 
        public List<PoiResponseBody> body { get; set; }
       
    }
    public class PoiResponseBody 
    {
        public PoiResponseBody() { }
        public int partnerId { get; set; }
        public string partnerPoiId { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string phone { get; set; } 
        public int locType { get; set; }//景点坐标系类型	0 火星坐标 1 地球坐标 2 百度坐标 3 图吧坐标 4 搜狗坐标 5 其他
        public int longitude { get; set; }//景区地理经度	必须真实有效,值为真实值的6次方取整
        public int latitude { get; set; }//景区地理纬度	必须真实有效,值为真实值的6次方取整 
    }
   
}
