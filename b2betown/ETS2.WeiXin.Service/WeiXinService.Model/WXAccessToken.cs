using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WeiXinService.Model
{
    /// <summary>
    /// 微信凭证
    /// </summary>
    public class WXAccessToken
    {
        private int id;

        /// <summary>

        /// 标识列

        /// </summary>

        public int Id
        {

            get { return id; }

            set { id = value; }

        }
        private int comid;

       
        public int Comid
        {

            get { return comid; }

            set { comid = value; }

        }

        private string aCCESS_TOKEN = "";



        public string ACCESS_TOKEN
        {

            get { return aCCESS_TOKEN; }

            set { aCCESS_TOKEN = value; }

        }

        private DateTime createDate;



        public DateTime CreateDate
        {

            get { return createDate; }

            set { createDate = value; }

        }
    }
}
