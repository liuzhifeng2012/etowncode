using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Tenpay.WxpayApi.sysprogram.model;
using ETS.Data.SqlHelper;
using Com.Tenpay.WxpayApi.sysprogram.data.internaldata;

namespace Com.Tenpay.WxpayApi.sysprogram.data
{
    public class B2b_pay_wxrefundlogData
    {
        public int Editwxrefundlog(B2b_pay_wxrefundlog refundlog)
        {
            using (var helper = new SqlHelper())
            {
                int r = new Internalb2b_pay_wxrefundlog(helper).Editwxrefundlog(refundlog);
                return r;
            }
        }

        public int Upreturnxml(string returnxml, int id)
        {
            using (var helper = new SqlHelper())
            {
                int r = new Internalb2b_pay_wxrefundlog(helper).Upreturnxml(returnxml,id);
                return r;
            }
        }

        public int  IsHasWxRefund(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new Internalb2b_pay_wxrefundlog(helper).IsHasWxRefund(orderid);
                return r;
            }
        }

        public string WxRefundDetailStr(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                string r = new Internalb2b_pay_wxrefundlog(helper).WxRefundDetailStr(orderid);
                return r;
            }
        }

        public decimal Gettotalquitfeebyoid(int orderid)
        {
            using(var helper=new SqlHelper())
            {
                decimal r = new Internalb2b_pay_wxrefundlog(helper).Gettotalquitfeebyoid(orderid);
                return r;
            }
        }
    }
}
