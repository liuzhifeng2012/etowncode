using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    /// <summary>
    /// 模板管理
    /// </summary>
    [Serializable()]
    public class H5_html
    {
        private int id;
        private string style_str = String.Empty;
        private string html_str = String.Empty;
        private int comid;
        private int modelid;


        public H5_html() { }

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
        public int Modelid
        {
            get { return modelid; }
            set { modelid = value; }
        }
        public string Style_str
        {
            get { return style_str; }
            set { style_str = value; }
        }
        public string Html_str
        {
            get { return html_str; }
            set { html_str = value; }
        }


    }
}
