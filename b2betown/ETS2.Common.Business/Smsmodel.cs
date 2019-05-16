using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.Common.Business
{
    public class Smsmodel
    {
        private int comid;//商户ID

        public int Comid
        {
            get { return comid; }
            set { comid = value; }
        }
        private string comName;//商户名
        
        public string ComName
        {
            get { return comName; }
            set { comName = value; }
        }
        private string recerceSMSPhone;//服务商电话
        public string RecerceSMSPhone 
        {
            get { return this.recerceSMSPhone; }
            set { recerceSMSPhone = value; }
        }

        private string phone;//预订人手机

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        private string name;//预订人姓名

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string card;//卡号

        public string Card
        {
            get { return card; }
            set { card = value; }
        }
        private string password;//密码

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        private decimal money;//钱 取绝对值

        public decimal Money
        {
            get { return money; }
            set { money = value; }
        }
        private decimal imprest;//预付款 取绝对值

        public decimal Imprest
        {
            get { return imprest; }
            set { imprest = value; }
        }
        private decimal integral;//积分 取绝对值

        public decimal Integral
        {
            get { return integral; }
            set { integral = value; }
        }
        private int num;//数量1

        public int Num
        {
            get { return num; }
            set { num = value; }
        }
        private int num1;//数量2

        public int Num1
        {
            get { return num1; }
            set { num1 = value; }
        }
        private string old;//原来值 列如：预付款10元 减预付款5元 old=10

        public string Old
        {
            get { return old; }
            set { old = value; }
        }
        private DateTime starttime;//开始时间

        public DateTime Starttime
        {
            get { return starttime; }
            set { starttime = value; }
        }
        private DateTime endtime;//结束时间

        public DateTime Endtime
        {
            get { return endtime; }
            set { endtime = value; }
        }
        private string code;//随机码

        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        private string customtext;//自定义文本

        public string Customtext
        {
            get { return customtext; }
            set { customtext = value; }
        }
        private string key;//短信类型

        public string Key
        {
            get { return key; }
            set { key = value; }
        }
        private string title;//标题

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

    }
}
