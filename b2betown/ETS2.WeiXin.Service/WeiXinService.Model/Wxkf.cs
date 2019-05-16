using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WeiXinService.Model
{
    public class Wxkf
    {
        private int id;
        private int kf_id;
        private string kf_nick = "";
        private string kf_account = "";
        private int yg_id;
        private string yg_name = "";
        private int ms_id;
        private string ms_name = "";
        private int comid;
        private int isonline;
        private int createuserid;
        private DateTime createtime;
        private int isbinded;
        private int iszongkf;
        private int isrun;

        private int kf_status;
        private int auto_accept;
        private int accepted_case;

        public int Accepted_case { get; set; }
        public int Auto_accept { get; set; }
        
        public int Kf_status { get; set; }


        public int Isrun { get; set; }
        public int Isbinded { get; set; }
        public int Iszongkf { get; set; }
        public int Id { get; set; }
        public int Kf_id { get; set; }
        public string Kf_nick { get; set; }
        public string Kf_account { get; set; }
        public int Yg_id { get; set; }
        public string Yg_name { get; set; }
        public int Ms_id { get; set; }
        public string Ms_name { get; set; }
        public int Comid { get; set; }
        public int Isonline { get; set; }
        public int Createuserid { get; set; }
        public DateTime Createtime { get; set; }
    }
}
