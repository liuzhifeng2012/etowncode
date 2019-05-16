using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{
    [Serializable()]
    public class B2b_crm_location
    {
        private int id;
        private int comid;
        private string weixin = String.Empty;
        private string createtime = String.Empty;
        private string latitude = String.Empty;
        private string longitude = String.Empty;
        private string priecision = String.Empty;
        private DateTime createtimeformat ;

        public B2b_crm_location() { }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int Comid
        {
            get { return comid; }
            set { comid = value; }
        }

        public string Weixin
        {
            get { return weixin; }
            set { weixin = value; }
        }

        public string Createtime
        {
            get { return createtime; }
            set { createtime = value; }
        }
        public string Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        public string Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }
        
        public DateTime Createtimeformat
        {
            get { return createtimeformat; }
            set { createtimeformat = value; }
        }


    }
}
