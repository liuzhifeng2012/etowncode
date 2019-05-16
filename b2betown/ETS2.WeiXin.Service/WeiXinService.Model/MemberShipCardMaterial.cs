using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WeiXinService.Model
{
    public class MemberShipCardMaterial
    {
        private int materialId;
        private string title = String.Empty;

        private string imgpath = String.Empty;
        private string summary = String.Empty;
        private string article = String.Empty;

        private bool applystate;

        private string phone = String.Empty;
        private decimal price;

        private int comid;





        public MemberShipCardMaterial() { }

        public decimal Price
        {
            get { return this.price; }
            set { this.price = value; }
        }

        public string Phone
        {
            get { return this.phone; }
            set { this.phone = value; }
        }




        public bool Applystate
        {
            get { return this.applystate; }
            set { this.applystate = value; }
        }


        public int MaterialId
        {
            get { return this.materialId; }
            set { this.materialId = value; }
        }



        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }



        public string Imgpath
        {
            get { return this.imgpath; }
            set { this.imgpath = value; }
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



        public int Comid
        {
            get { return this.comid; }
            set { this.comid = value; }
        }

    }
}
