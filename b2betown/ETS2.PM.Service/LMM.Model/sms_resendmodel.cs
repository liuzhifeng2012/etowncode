using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.LMM.Model
{
   public class sms_resendmodel
    {
        public string uid { get; set; }
        public string password { get; set; }
        public string sign { get; set; }

        public string timestamp { get; set; }
        public string extId { get; set; }
    }
}
