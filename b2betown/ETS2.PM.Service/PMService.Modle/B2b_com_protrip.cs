using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class B2b_com_protrip
    {
        private int id;
        private string activityArea = "";
        private string traffic = "";
        private string scenicActivity = "";
        private string description = "";
        private string hotel = "";
        private string dining = "";
        private int productid;

        private int creator;
        private DateTime createDate;

        public B2b_com_protrip()
        {

        }

        public int Id 
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public int Creator
        {
            get { return this.creator; }
            set { this.creator = value; }
        }
        public DateTime CreateDate
        {
            get { return this.createDate; }
            set { this.createDate = value; }
        }
        public string ActivityArea
        {
            get { return this.activityArea; }
            set { this.activityArea = value; }
        }
        public string Traffic
        {
            get { return this.traffic; }
            set { this.traffic = value; }
        }
        public string ScenicActivity
        {
            get { return this.scenicActivity; }
            set { this.scenicActivity = value; }
        }
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }
        public string Hotel
        {
            get { return this.hotel; }
            set { this.hotel = value; }
        }
        public string Dining
        {
            get { return this.dining; }
            set { this.dining = value; }
        }
        public int Productid
        {
            get { return this.productid; }
            set { this.productid = value; }
        }
    }
}
