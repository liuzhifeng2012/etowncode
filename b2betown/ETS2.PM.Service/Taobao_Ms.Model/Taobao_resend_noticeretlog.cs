using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Taobao_Ms.Model
{
    public class Taobao_resend_noticeretlog
    {
         public Taobao_resend_noticeretlog() { }
         public int id { get; set; }
         public string order_id { get; set; }//订单编号
         public string verify_codes { get; set; }//发送成功的验证码及可验证次数的列表，码和可验证次数用英文冒号分隔，多个码之间用英文逗号分隔，所有字符都为英文半角
         public string token { get; set; }//安全验证token，需要和发码通知中的token一致
         public string codemerchant_id { get; set; }//码商ID,是码商的话必须传递,如果是信任卖家,不需要传
         public string qr_images { get; set; }
         public string ret_code { get; set; }
         public DateTime ret_time { get; set; }
    }
}
