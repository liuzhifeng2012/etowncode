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
    public class B2b_model
    {
        private int id;
        private int daohangnum;
        private string title = String.Empty;
        private int daohangimg ;
        private string style_str = String.Empty;
        private string html_str = String.Empty;
        private int bgimage;
        private int bgimage_w;
        private int bgimage_h;
        private int smallimg;
        private int usestate= 0;


        public B2b_model() { }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int Usestate
        {
            get { return usestate; }
            set { usestate = value; }
        }

        public int Daohangnum
        {
            get { return daohangnum; }
            set { daohangnum = value; }
        }
        public int Bgimage
        {
            get { return bgimage; }
            set { bgimage = value; }
        }

        public int Smallimg
        {
            get { return smallimg; }
            set { smallimg = value; }
        }
        
        public int Bgimage_w
        {
            get { return bgimage_w; }
            set { bgimage_w = value; }
        }
        public int Bgimage_h
        {
            get { return bgimage_h; }
            set { bgimage_h = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public int Daohangimg
        {
            get { return daohangimg; }
            set { daohangimg = value; }
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
