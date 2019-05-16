using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WeiXinService.Model
{
    public  class periodical //‘期’
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private int comid;

        public int Comid
        {
            get { return comid; }
            set { comid = value; }
        }
        private int Wxsaletypeid;//素材类型id

        public int Wxsaletypeid1
        {
            get { return Wxsaletypeid; }
            set { Wxsaletypeid = value; }
        }
        private int percal;//期号

        public int Percal
        {
            get { return percal; }
            set { percal = value; }
        }
        private int peryear;//年号

        public int Peryear
        {
            get { return peryear; }
            set { peryear = value; }
        }
        private DateTime uptime;//上传时间

        public DateTime Uptime
        {
            get { return uptime; }
            set { uptime = value; }
        }

        private string perinfo;

        public string Perinfo
        {
            get { return perinfo; }
            set { perinfo = value; }
        }

    }
}
