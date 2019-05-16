using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WeiXinService.Model
{
    public class Weixin_template
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { this.id = value; }
        }

        private int comid;
        public int Comid
        {
            get { return comid; }
            set { this.comid = value; }
        }

        private string infotype;
        public string Infotype
        {
            get { return this.infotype; }
            set { this.infotype = value; }
        }

        private string template_id;
        public string Template_id
        {
            get { return this.template_id; }
            set { this.template_id = value; }
        }

        private string template_name;
        public string Template_name
        {
            get { return this.template_name; }
            set { this.template_name = value; }
        }

        private string first_DATA;
        public string First_DATA
        {
            get { return this.first_DATA; }
            set { this.first_DATA = value; }
        }

        private string remark_DATA;
        public string Remark_DATA
        {
            get { return this.remark_DATA; }
            set { this.remark_DATA = value; }
        }
    }
}
