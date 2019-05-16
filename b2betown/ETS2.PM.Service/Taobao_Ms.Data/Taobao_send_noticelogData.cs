using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.Taobao_Ms.Model;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Taobao_Ms.Data.Internal;

namespace ETS2.PM.Service.Taobao_Ms.Data
{
    public class Taobao_send_noticelogData
    {
        public int Editsendnoticelog(Taobao_send_noticelog sendnoticelog)
        {
            using(var helper=new  SqlHelper())
            {
                var r = new InternalTaobao_send_noticelog(helper).Editsendnoticelog(sendnoticelog);
                return r;
            }
        }

        //public int GetSendNoticeNum(string taobao_orderid, string method)
        //{
        //    using (var helper = new SqlHelper())
        //    {
        //        var r = new InternalTaobao_send_noticelog(helper).GetSendNoticeNum(taobao_orderid,method);
        //        return r;
        //    }
        //}
        public int GetSendNoticeNum(string token)
        {
            using (var helper = new SqlHelper())
            {
                var r = new InternalTaobao_send_noticelog(helper).GetSendNoticeNum(token);
                return r;
            }
        }

        public string GetSysOidByTaobaoOid(string tboid)
        {
            using (var helper = new SqlHelper())
            {
                var r = new InternalTaobao_send_noticelog(helper).GetSysOidByTaobaoOid(tboid);
                return r;
            }
        }
       
        public Taobao_send_noticelog GetSendNoticeLogByTbOid(string orderid)
        {
            using(var helper=new SqlHelper())
            {
                Taobao_send_noticelog r = new InternalTaobao_send_noticelog(helper).GetSendNoticeLogByTbOid(orderid);
                return r;
            }
        }
        public Taobao_send_noticelog GetSendNoticeBySelfOrderid(string selforderid)
        {
            using (var helper = new SqlHelper())
            {
                var r = new InternalTaobao_send_noticelog(helper).GetSendNoticeBySelfOrderid(selforderid);
                return r;
            }
        }


        public Taobao_send_noticelog GetSendNoticeLogByQrcode(string qrcode)
        {
            using (var helper = new SqlHelper())
            {
                Taobao_send_noticelog r = new InternalTaobao_send_noticelog(helper).GetSendNoticeLogByQrcode(qrcode);
                return r;
            }
        }
    }
}
