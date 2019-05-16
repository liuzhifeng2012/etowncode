using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Meituan.Model
{
    public class ApiRequest 
    {
        public ApiRequest() { }
        public string partnerId { get; set; }
        public object body { get; set; }
    }


    public class MTRequestData<T> where T : ApiRequest
    {
        public MTRequestData() { }
        public T body { get; set; }
    }
}
