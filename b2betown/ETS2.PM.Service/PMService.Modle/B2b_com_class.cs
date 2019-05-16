using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    /// <summary>
    /// 商家产品分类管理，未正常使用，未做完
    /// </summary>
    [Serializable()]
    public class B2b_com_class
    {
        private int id;
        private int comid;
        private string classname = String.Empty;
        private int classid;
        private int proid;
        private int industryid;

        public B2b_com_class() { }


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
        public int Classid
        {
            get { return classid; }
            set { classid = value; }
        }
        public int Proid
        {
            get { return proid; }
            set { proid = value; }
        }
        public string Classname
        {
            get { return classname; }
            set { classname = value; }
        }
        public int Industryid
        {
            get { return industryid; }
            set { industryid = value; }
        }

    }
}
