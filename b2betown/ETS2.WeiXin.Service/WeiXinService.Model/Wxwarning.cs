using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WeiXinService.Model
{
    public class Wxwarning
    {
        public int Id { get; set; }
        public string AppId { get; set; }
        public int ErrorType { get; set; }
        public string Description { get; set; }
        public string AlarmContent { get; set; }
        public string TimeStamp { get; set; }
        public string Responsexml { get; set; }
        public DateTime Timeformat { get; set; }
        public int Comid { get; set; }
    }
}
