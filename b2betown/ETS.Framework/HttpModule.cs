using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ETS.Framework
{     /// <summary>
    ///TestHttpModule 的摘要说明
    /// </summary>
    public class HttpModule : IHttpModule
    {
        public HttpModule()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication httpapp = sender as HttpApplication; //得到当前应用程序对象
            HttpContext httpcontext = httpapp.Context;  //得到当前请求的上下文

            String url = httpcontext.Request.Url.ToString();
          

            int lastindex = url.LastIndexOf('/');
            String filename = url.Substring(lastindex);

            //判断网址是否符合伪静态网址形式a_wx_Mid.aspx，符合的话，则根据mid得到原始参数initparam，重写网址 a.aspx? code=d&initparam 进入a.aspx页面;
            if (new Regex(@"_wx_(\d+)\.aspx").IsMatch(filename))
            {
                //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "initUrl:" + url); 

                Match match = new Regex(@"_wx_(\d+)\.aspx").Match(filename);//创建Match对象
                string mid = match.Groups[1].Value;//通过正则结果对象，取到组（）中数字的值（ID）

                //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "mid:" + mid); 

                ReqUrl_Log model = new ReqUrl_LogHelper().GetReqUrlLogById(mid);
                if (model != null)
                {
                    string trueurl = url.Replace("http://" + model.hoststr, "").Replace("_wx_" + mid, "") + model.paramstr.Replace("?", "&");

                    //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "trueurl:" + trueurl); 
                    httpcontext.RewritePath(trueurl);//通过指定的ID，重写真正存在的URL地址。
                    return;
                }
            }

            //列表分类 可以用html或者其他字符串做后缀名，但需要服务器支持。
            Regex reg = new Regex(@"list_(\d+)\.aspx"); //创建正则对象，用来验证文件名是否满足条件
            if (reg.IsMatch(filename))
            {
                Match match = reg.Match(filename);//创建Match对象
                string id = match.Groups[1].Value;//通过正则结果对象，取到组（）中数字的值（ID）
                httpcontext.RewritePath("/h5/list.aspx?class=" + id);//通过指定的ID，重写真正存在的URL地址。
                return;
            }

            //微信支付页面
            if (new Regex(@"payment_(\d+)_(\d+)\.aspx").IsMatch(filename))
            {
                Match match = new Regex(@"payment_(\d+)_(\d+)\.aspx").Match(filename);//创建Match对象
                string orderid = match.Groups[1].Value;//通过正则结果对象，取到组（）中数字的值 
                string showwxpaytitle = match.Groups[2].Value;//通过正则结果对象，取到组（）中数字的值 

                string trueurl = "/wxpay/payment.aspx?orderid=" + orderid + "&showwxpaytitle=" + showwxpaytitle + "&" + url.Substring(url.LastIndexOf('?') + 1);
                httpcontext.RewritePath(trueurl);//通过指定的ID，重写真正存在的URL地址。
                return;
            }

            //实物产品 微信共享收货地址/h5/order/pay.aspx?num=1&id=3958
            if (new Regex(@"micromart_pay_(\d+)_(\w+)\.aspx").IsMatch(filename))
            {
                Match match = new Regex(@"micromart_pay_(\d+)_(\w+)\.aspx").Match(filename);//创建Match对象
                string num = match.Groups[1].Value;//通过正则结果对象，取到组（）中数字的值 
                string proid = match.Groups[2].Value;//通过正则结果对象，取到组（）中数字的值 

                string nextpara = "";
                if (url.IndexOf("?") != -1)
                {
                    nextpara = "&" + url.Substring(url.LastIndexOf('?') + 1);
                }
                string trueurl = "/wxpay/micromart_pay.aspx?num=" + num + "&id=" + proid + nextpara;
                ////TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "newUrl:" + trueurl);
                httpcontext.RewritePath(trueurl);//通过指定的ID，重写真正存在的URL地址。
                return;
            }



            //列表关键词查询
            if (new Regex(@"list_(\D+)\.aspx").IsMatch(filename))
            {
                // "重写面";
                Match matches = new Regex(@"list_(\D+)\.aspx").Match(filename);
                httpcontext.RewritePath("/h5/list.aspx?KEY=" + System.Web.HttpUtility.UrlEncode(matches.Groups[1].Value, System.Text.Encoding.UTF8));
                return;
            }


            ////订购页面短网址
            //if (new Regex(@"orderenter_(\d+)\.aspx").IsMatch(filename))
            //{
            //    // "重写面";
            //    Match matches = new Regex(@"orderenter_(\d+)\.aspx").Match(filename);
            //    httpcontext.RewritePath("/h5/orderenter.aspx?id=" + matches.Groups[1].Value);
            //    return;
            //}
            //个人地址
            //if (new Regex(@"list_(\D+)\.aspx").IsMatch(filename))
            //{
            //    // "重写面";
            //    Match matches = new Regex(@"list_(\D+)\.aspx").Match(filename);
            //    httpcontext.RewritePath("/h5/list.aspx?KEY=" + System.Web.HttpUtility.UrlEncode(matches.Groups[1].Value, System.Text.Encoding.UTF8));
            //    return;
            //}


        }

    }
}
