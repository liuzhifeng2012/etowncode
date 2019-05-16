using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    /// <summary>
    /// 模板菜单管理
    /// </summary>
    [Serializable()]
    public class B2b_modelmenu
    {

        private int id;
        private string name = String.Empty;
        private string linkurl = String.Empty;
        private int linktype;
        private int imgurl;
        private int sortid;
        private int modelid;
        private string fonticon;



        public B2b_modelmenu() { }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int Imgurl
        {
            get { return imgurl; }
            set { imgurl = value; }
        }
        public int Sortid
        {
            get { return sortid; }
            set { sortid = value; }
        }
        public int Modelid
        {
            get { return modelid; }
            set { modelid = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Linkurl
        {
            get { return linkurl; }
            set { linkurl = value; }
        }
        public int Linktype
        {
            get { return linktype; }
            set { linktype = value; }
        }
        public string Fonticon
        {
            get { return fonticon; }
            set { fonticon = value; }
        }
        

    }
}
