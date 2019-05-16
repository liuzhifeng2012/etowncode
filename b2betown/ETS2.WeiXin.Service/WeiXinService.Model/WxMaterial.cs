using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WeiXinService.Model
{

    [Serializable()]
    public class WxMaterial
    {

        private int materialId;
        private string title = String.Empty;
        private string author = String.Empty;
        private string imgpath = String.Empty;
        private string summary = String.Empty;
        private string article = String.Empty;
        private string articleurl = String.Empty;
        private DateTime operatime;
        private string keyword = String.Empty;
        private int applystate;

        private string phone = String.Empty;
        private decimal price;

        private int salePromoteTypeid;
        private int comid;

        private int fornum;
        private string name;
        private decimal idcard;
        private string authorpayurl;
        private string imgurl;
        


        public WxMaterial() { }



        public string Phone
        {
            get { return this.phone; }
            set { this.phone = value; }
        }
        public decimal Price
        {
            get { return this.price; }
            set { this.price = value; }
        }

        public int SalePromoteTypeid
        {
            get { return this.salePromoteTypeid; }
            set { this.salePromoteTypeid = value; }
        }




        public int Applystate
        {
            get { return this.applystate; }
            set { this.applystate = value; }
        }


        public int MaterialId
        {
            get { return this.materialId; }
            set { this.materialId = value; }
        }


        public string Keyword
        {
            get { return this.keyword; }
            set { this.keyword = value; }
        }


        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }


        public string Author
        {
            get { return this.author; }
            set { this.author = value; }
        }


        public string Imgpath
        {
            get { return this.imgpath; }
            set { this.imgpath = value; }
        }
        
        public string Imgurl
        {
            get { return this.imgurl; }
            set { this.imgurl = value; }
        }
        

        public string Summary
        {
            get { return this.summary; }
            set { this.summary = value; }
        }


        public string Article
        {
            get { return this.article; }
            set { this.article = value; }
        }


        public string Articleurl
        {
            get { return this.articleurl; }
            set { this.articleurl = value; }
        }


        public DateTime Operatime
        {
            get { return this.operatime; }
            set { this.operatime = value; }
        }

        private DateTime staticdate;//有效期开始时间

        public DateTime Staticdate
        {
            get { return staticdate; }
            set { staticdate = value; }
        }
        private DateTime enddate;//有效期结束时间

        public DateTime Enddate
        {
            get { return enddate; }
            set { enddate = value; }
        }


        private int periodicalid;

        public int Periodicalid
        {
            get { return periodicalid; }
            set { periodicalid = value; }
        }

        public int Comid
        {
            get { return this.comid; }
            set { this.comid = value; }
        }

        public int Fornum
        {
            get { return this.fornum; }
            set { this.fornum = value; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public decimal Idcard
        {
            get { return this.idcard; }
            set { this.idcard = value; }
        }


        public string Authorpayurl
        {
            get { return this.authorpayurl; }
            set { this.authorpayurl = value; }
        }
    }
}
