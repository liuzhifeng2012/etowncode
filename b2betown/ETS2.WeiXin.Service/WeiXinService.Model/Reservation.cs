using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WeiXinService.Model
{
    public class Reservation
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private int uid;
        public int Uid 
        {
            get { return uid; }
            set { uid = value; }
        }

        private int comid;

        public int Comid
        {
            get { return comid; }
            set { comid = value; }
        }
        private string name;//预订人姓名

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private decimal phone;//预订人电话

        public decimal Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        private string titile;//预订标题

        public string Titile
        {
            get { return titile; }
            set { titile = value; }
        }
        private int number;//预订数量

        public int Number
        {
            get { return number; }
            set { number = value; }
        }
        private string submit_name;//操作人

        public string Submit_name
        {
            get { return submit_name; }
            set { submit_name = value; }
        }
        private int Rstatic;//预订单状态

        public int Rstatic1
        {
            get { return Rstatic; }
            set { Rstatic = value; }
        }
        private DateTime Rdate;//时间

        public DateTime Rdate1
        {
            get { return Rdate; }
            set { Rdate = value; }
        }
        private string ip;

        public string Ip
        {
            get { return ip; }
            set { ip = value; }
        }
        private int wxMaterialid;//活动表WxMaterial的id

        public int WxMaterialid
        {
            get { return wxMaterialid; }
            set { wxMaterialid = value; }
        }

        private DateTime resdate;//预订时间

        public DateTime Resdate
        {
            get { return resdate; }
            set { resdate = value; }
        }

        private DateTime checkoutdate;//离店日期

        public DateTime Checkoutdate
        {
            get { return checkoutdate; }
            set { checkoutdate = value; }
        }

        private int roomtypeid;//入住房型id
        public int Roomtypeid
        {
            get { return roomtypeid; }
            set { roomtypeid = value; }
        }

        private decimal totalprice;//总金额
        public decimal Totalprice
        {
            get { return totalprice; }
            set { totalprice = value; }
        }
        private string lastercheckintime;// 最晚入住时间
        public string Lastercheckintime
        {
            get { return lastercheckintime; }
            set { lastercheckintime = value; }
        }
        private DateTime subdate;//提交订单日期
        public DateTime Subdate
        {
            get { return subdate; }
            set { subdate = value; }
        }

        private string remarks;//备注

        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }




    }
}
