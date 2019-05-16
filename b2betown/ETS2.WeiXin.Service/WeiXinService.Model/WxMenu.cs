using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WeiXinService.Model
{
    [Serializable()]
    public class WxMenu
    {
        private int comid;
        private int menuid;
        private string name = String.Empty;
        private string instruction = String.Empty;

        private string linkurl = String.Empty;
        private int fathermenuid;

        private int operationtypeid;
        private int salePromoteTypeid;
        private string wxanswertext = String.Empty;

        private int product_class;

        private string keyy;

        public int pictexttype { get; set; }

        public WxMenu() { }

        public string Keyy
        {
            get { return this.keyy; }
            set { this.keyy = value; }
        }


        public string Wxanswertext
        {
            get { return this.wxanswertext; }
            set { this.wxanswertext = value; }
        }


        public int Operationtypeid
        {
            get { return this.operationtypeid; }
            set { this.operationtypeid = value; }
        }
        public int Product_class
        {
            get { return this.product_class; }
            set { this.product_class = value; }
        }
        public int SalePromoteTypeid
        {
            get { return this.salePromoteTypeid; }
            set { this.salePromoteTypeid = value; }
        }

        public int Menuid
        {
            get { return this.menuid; }
            set { this.menuid = value; }
        }

        public int Comid
        {
            get { return this.comid; }
            set { this.comid = value; }
        }



        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }


        public string Instruction
        {
            get { return this.instruction; }
            set { this.instruction = value; }
        }




        public string Linkurl
        {
            get { return this.linkurl; }
            set { this.linkurl = value; }
        }


        public int Fathermenuid
        {
            get { return this.fathermenuid; }
            set { this.fathermenuid = value; }
        }


    }
}
