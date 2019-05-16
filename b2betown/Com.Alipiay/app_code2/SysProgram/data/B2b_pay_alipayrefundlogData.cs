using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Alipiay.app_code2.SysProgram.model;
using ETS.Data.SqlHelper;
using Com.Alipiay.app_code2.SysProgram.data.internaldata;

namespace Com.Alipiay.app_code2.SysProgram.data
{
    public class B2b_pay_alipayrefundlogData
    {

        //public B2b_pay_alipayrefundlog GetLastestrefundsuclog(int orderid)
        //{
        //      using(var helper=new SqlHelper())
        //      {
        //           B2b_pay_alipayrefundlog r=new InternalB2b_pay_alipayrefundlog(helper).GetLastestrefundsuclog(orderid);
        //           return r;
        //      }
        //}

        public int Editalipayrefundlog(B2b_pay_alipayrefundlog nowlog)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2b_pay_alipayrefundlog(helper).Editalipayrefundlog(nowlog);
                return r;
            }
        }

        public int Uprefundlog(string batch_no, string success_num, string result_details, string error_code, string error_desc,string notify_id,string notify_type,string notify_time)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2b_pay_alipayrefundlog(helper).Uprefundlog(batch_no, success_num, result_details, error_code, error_desc, notify_id, notify_type, notify_time);
                return r;
            }
        }

        public decimal Gettotalquitfeebyoid(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                decimal r = new InternalB2b_pay_alipayrefundlog(helper).Gettotalquitfeebyoid(orderid);
                return r;
            }
        }

        public int IsHasAlipayRefund(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2b_pay_alipayrefundlog(helper).IsHasAlipayRefund(orderid);
                return r;
            }
        }

        public string AlipayRefundDetailStr(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                string r = new InternalB2b_pay_alipayrefundlog(helper).AlipayRefundDetailStr(orderid);
                return r;
            }
        }

        public B2b_pay_alipayrefundlog Getrefundlogbybatch_no(string batch_no)
        {
            using (var helper = new SqlHelper())
            {
                B2b_pay_alipayrefundlog r = new InternalB2b_pay_alipayrefundlog(helper).Getrefundlogbybatch_no(batch_no);
                return r;
            }
        }
    }
}
