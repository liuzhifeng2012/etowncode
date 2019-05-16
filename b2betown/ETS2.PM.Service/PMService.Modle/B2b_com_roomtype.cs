using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    [Serializable()]
    public class B2b_com_roomtype
    {

        private int id;
        private string name = String.Empty;
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
        private int sortid;
        private int server_type;
        private int pro_type;
        private int source_type;
        private int createuserid;
        private DateTime createtime;
        private bool whetheravailabel;
        private string roomtyperemark = String.Empty;
        private int comid;
        private int roomtypeimg;

        private string recerceSMSName;
        private string recerceSMSPhone;


        public B2b_com_roomtype() { }




        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
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




        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
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


        public string Sms
        {
            get { return this.sms; }
            set { this.sms = value; }
        }


        public int Sortid
        {
            get { return this.sortid; }
            set { this.sortid = value; }
        }


        public int Server_type
        {
            get { return this.server_type; }
            set { this.server_type = value; }
        }


        public int Pro_type
        {
            get { return this.pro_type; }
            set { this.pro_type = value; }
        }


        public int Source_type
        {
            get { return this.source_type; }
            set { this.source_type = value; }
        }


        public int Createuserid
        {
            get { return this.createuserid; }
            set { this.createuserid = value; }
        }


        public DateTime Createtime
        {
            get { return this.createtime; }
            set { this.createtime = value; }
        }


        public bool Whetheravailabel
        {
            get { return this.whetheravailabel; }
            set { this.whetheravailabel = value; }
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


        public int Roomtypeimg
        {
            get { return this.roomtypeimg; }
            set { this.roomtypeimg = value; }
        }

    }
}
