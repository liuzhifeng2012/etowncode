using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Data.InternalData;

namespace ETS2.WeiXin.Service.WeiXinService.Data
{
    public class Weixin_templatemsg_sendlogData
    {
        internal int EditTmplLog(Weixin_templatemsg_sendlog m)
        {
             using(var helper=new  SqlHelper())
             {
                 int r = new InternalWeixin_templatemsg_sendlog(helper).EditTmplLog(m);
                 return r;
             }
        }
        /// <summary>
        /// 对同一订单 模板消息一旦发送成功，不在重复发送
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="Template_id"></param>
        /// <returns></returns>
        internal bool Issendsuc(int orderid, string Template_id)
        {
            using (var helper = new SqlHelper())
            {
                bool r = new InternalWeixin_templatemsg_sendlog(helper).Issendsuc(orderid,Template_id);
                return r;
            }
        }
        /// <summary>
        /// 根据msgid得到模板消息记录
        /// </summary>
        /// <param name="msgid"></param>
        /// <returns></returns>
        internal Weixin_templatemsg_sendlog GetTmplLogByMsgId(string msgid)
        {
            using (var helper = new SqlHelper())
            {
                Weixin_templatemsg_sendlog r = new InternalWeixin_templatemsg_sendlog(helper).GetTmplLogByMsgId(msgid);
                return r;
            }
        }
    }
}
