using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class B2b_com_project
    {

        private int id;
        private string projectname = "";
        private int projectimg;
        private string province = "";
        private string city = "";
        private int industryid;
        private string briefintroduce = "";
        private string address = "";
        private string mobile = "";
        private string coordinate = "";
        private string serviceintroduce = "";
        private string onlinestate = "0";
        private int comid;
        private DateTime createtime;
        private int createuserid;
        public int bindingprojectid {get;set;}
        public int hotelset{get;set;}
        public decimal grade { get; set; }
        public string star{get;set;}
        public int parking{get;set;}
        public string cu { get; set; }
        public string minprice { get; set; }
        public string Imgaddress { get; set; }


        public B2b_com_project() { }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public int Createuserid
        {
            get { return createuserid; }
            set { createuserid = value; }
        }
        public DateTime Createtime
        {
            get { return createtime; }
            set { createtime = value; }
        }


        public int Comid
        {
            get { return comid; }
            set { comid = value; }
        }
        public string Projectname
        {
            get { return projectname; }
            set { projectname = value; }
        }
        public int Projectimg
        {
            get { return projectimg; }
            set { projectimg = value; }
        }
        public string Province
        {
            get { return province; }
            set { province = value; }
        }
        public string City
        {
            get { return city; }
            set { city = value; }
        }
        public int Industryid
        {
            get { return industryid; }
            set { industryid = value; }
        }
        public string Briefintroduce
        {
            get { return briefintroduce; }
            set { briefintroduce = value; }
        }
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }
        public string Coordinate
        {
            get { return coordinate; }
            set { coordinate = value; }
        }
        public string Serviceintroduce
        {
            get { return serviceintroduce; }
            set { serviceintroduce = value; }
        }
        public string Onlinestate
        {
            get { return onlinestate; }
            set { onlinestate = value; }
        }

    }
}
