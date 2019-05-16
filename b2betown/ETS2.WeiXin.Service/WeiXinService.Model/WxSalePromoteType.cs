using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WeiXinService.Model
{
    public class WxSalePromoteType
    {
        private int id;
        private string typename = String.Empty;
        private string typeclass = String.Empty;

        private bool isshowpast = true;

        public WxSalePromoteType() { }

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

        public string Typeclass
        {
            get { return this.typeclass; }
            set { this.typeclass = value; }
        }

        public bool Isshowpast
        {
            get { return this.isshowpast; }
            set { this.isshowpast = value; }
        }
    }
}
