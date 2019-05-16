using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WeiXinService.Model
{
    public class WxOperationType
    {
        private int id;
        private string typename = String.Empty;

        public WxOperationType() { }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }




        public string Typename
        {
            get { return this.typename; }
            set { this.typename = value; }
        }

    }
}
