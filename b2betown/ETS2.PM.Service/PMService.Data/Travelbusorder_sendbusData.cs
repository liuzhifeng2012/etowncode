using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data.InternalData;
using ETS.Data.SqlHelper;

namespace ETS2.PM.Service.PMService.Data
{
    public class Travelbusorder_sendbusData
    {
        public Travelbusorder_sendbus Gettravelbus(int busid)
        {
            using (var helper = new SqlHelper())
            {

                Travelbusorder_sendbus r = new Internaltravelbusorder_sendbus(helper).Gettravelbus(busid);

                return r;
            }

        }

        public IList<Travelbusorder_sendbus> Gettravelbusorder_sendbusBylogid(int logid, out int totle)
        {
            using (var helper=new  SqlHelper())
            {
                IList<Travelbusorder_sendbus> list = new Internaltravelbusorder_sendbus(helper).Gettravelbusorder_sendbusBylogid(logid,out totle);
                return list;
           
            }
        }
        /// <summary>
        /// 派车的详细情况
        /// </summary>
        /// <param name="proid"></param>
        /// <param name="daydate"></param>
        /// <returns></returns>
        public string BusDetailstr(int proid, DateTime daydate)
        {
            using (var helper = new SqlHelper())
            {
                string r = new Internaltravelbusorder_sendbus(helper).BusDetailstr(proid,daydate);
                return r;

            }
        }
    }
}
