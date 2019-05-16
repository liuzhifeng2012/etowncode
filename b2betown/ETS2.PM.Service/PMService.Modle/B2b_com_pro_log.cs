using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    /// <summary>
    /// 产品发布状态没修改状态日志
    /// </summary>
    [Serializable()]
    public class B2b_com_pro_log
    {

        private int id;
        private int pro_id;
        private int oldstate;
        private int newstate;
        private string remark = String.Empty;
        private DateTime modifydate;
        private int confirm;
        private int modifyuserid;



        public B2b_com_pro_log() { }




        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }





        public int Pro_id
        {
            get { return this.pro_id; }
            set { this.pro_id = value; }
        }


        public int Oldstate
        {
            get { return this.oldstate; }
            set { this.oldstate = value; }
        }


        public int Newstate
        {
            get { return this.newstate; }
            set { this.newstate = value; }
        }


        public string Remark
        {
            get { return this.remark; }
            set { this.remark = value; }
        }


        public DateTime Modifydate
        {
            get { return this.modifydate; }
            set { this.modifydate = value; }
        }


        public int Confirm
        {
            get { return this.confirm; }
            set { this.confirm = value; }
        }


        public int Modifyuserid
        {
            get { return this.modifyuserid; }
            set { this.modifyuserid = value; }
        }

    }
}
