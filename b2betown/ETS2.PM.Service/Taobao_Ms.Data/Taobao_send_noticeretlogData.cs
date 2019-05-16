using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.Taobao_Ms.Model;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Taobao_Ms.Data.Internal;

namespace ETS2.PM.Service.Taobao_Ms.Data
{
    public class Taobao_send_noticeretlogData
    {
        public int Editsendnoticeretlog(Taobao_send_noticeretlog mretlog)
        {
            using (var helper=new SqlHelper())
            {
                int r = new InternalTaobao_send_noticeretlog(helper).Editsendnoticeretlog(mretlog);
                return r;
            }
        }

        public string GetVerifyCodesByTborderid(string tboid)
        {
            using (var helper = new SqlHelper())
            {
                string r = new InternalTaobao_send_noticeretlog(helper).GetVerifyCodesByTborderid(tboid);
                return r;
            }
        }

        public Taobao_send_noticeretlog GetSendRetLogByQrcode(string qrcode)
        {
            using (var helper = new SqlHelper())
            {
                Taobao_send_noticeretlog r = new InternalTaobao_send_noticeretlog(helper).GetSendRetLogByQrcode(qrcode);
                return r;
            }
        }
        /*回调发码通知请求次数*/
        public int GetInvokeNum(string tborderid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalTaobao_send_noticeretlog(helper).GetInvokeNum(tborderid);
                return r;
            }
        }
        /// <summary>
        /// 24h内 提单成功 但是 发送淘宝发码回调失败的订单;如果 成功了，则不再(无需)可以回调
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public int GetIscantaobo_sendret(int orderid)
        {
             using(var helper=new SqlHelper())
             {
                 int r = new InternalTaobao_send_noticeretlog(helper).GetIscantaobo_sendret(orderid);
                 return r;
             }
        }
    }
}
