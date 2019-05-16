using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class B2b_com_housetype
    {
        private int id;
        private string bedtype = String.Empty;
        private string wifi = String.Empty;
        private int reserveType;
        private string builtuparea = String.Empty;
        private string floor = String.Empty;
        private string bedwidth = String.Empty;
        private bool whetherextrabed;
        private decimal extrabedprice;
        private int largestguestnum;
        private bool whethernonsmoking;
        private string amenities = String.Empty;
        private string mediatechnology = String.Empty;
        private string foodanddrink = String.Empty;
        private string showerRoom = String.Empty;
        private int breakfast;
        private string sms = String.Empty;
 
        private string roomtyperemark = String.Empty;
        private int comid;
        private int proid;
   
        private string recerceSMSName;
        private string recerceSMSPhone;

        private string pro_name;

        public B2b_com_housetype() { }

        public string Proname
        {
            get { return this.pro_name; }
            set { this.pro_name = value; }
        }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        
        public int Proid
        {
            get { return this.proid; }
            set { this.proid = value; }
        }
        public string RecerceSMSName
        {
            get { return this.recerceSMSName; }
            set { this.recerceSMSName = value; }
        }
        public string RecerceSMSPhone
        {
            get { return this.recerceSMSPhone; }
            set { this.recerceSMSPhone = value; }
        }


 

        public string Bedtype
        {
            get { return this.bedtype; }
            set { this.bedtype = value; }
        }


        public string Wifi
        {
            get { return this.wifi; }
            set { this.wifi = value; }
        }


        public int ReserveType
        {
            get { return this.reserveType; }
            set { this.reserveType = value; }
        }


        public string Builtuparea
        {
            get { return this.builtuparea; }
            set { this.builtuparea = value; }
        }


        public string Floor
        {
            get { return this.floor; }
            set { this.floor = value; }
        }


        public string Bedwidth
        {
            get { return this.bedwidth; }
            set { this.bedwidth = value; }
        }


        public bool Whetherextrabed
        {
            get { return this.whetherextrabed; }
            set { this.whetherextrabed = value; }
        }


        public decimal Extrabedprice
        {
            get { return this.extrabedprice; }
            set { this.extrabedprice = value; }
        }


        public int Largestguestnum
        {
            get { return this.largestguestnum; }
            set { this.largestguestnum = value; }
        }


        public bool Whethernonsmoking
        {
            get { return this.whethernonsmoking; }
            set { this.whethernonsmoking = value; }
        }


        public string Amenities
        {
            get { return this.amenities; }
            set { this.amenities = value; }
        }


        public string Mediatechnology
        {
            get { return this.mediatechnology; }
            set { this.mediatechnology = value; }
        }


        public string Foodanddrink
        {
            get { return this.foodanddrink; }
            set { this.foodanddrink = value; }
        }


        public string ShowerRoom
        {
            get { return this.showerRoom; }
            set { this.showerRoom = value; }
        }


        public int Breakfast
        {
            get { return this.breakfast; }
            set { this.breakfast = value; }
        }

 

        public string Roomtyperemark
        {
            get { return this.roomtyperemark; }
            set { this.roomtyperemark = value; }
        }


        public int Comid
        {
            get { return this.comid; }
            set { this.comid = value; }
        }


 

    }
}
