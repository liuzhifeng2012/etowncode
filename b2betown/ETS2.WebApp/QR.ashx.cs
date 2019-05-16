using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Framework;

namespace ETS2.WebApp
{
    /// <summary>
    /// QR 的摘要说明
    /// </summary>
    public class QR : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
             string p = context.Request["p"].ConvertTo<string>("");
             if (p != "")
             {
                 if (p.IndexOf("【")>0)
                 {
                     p = p.Substring(0, p.IndexOf("【"));
                 }
                 if (p.IndexOf("[") > 0)
                 {
                     p = p.Substring(0, p.IndexOf("["));
                 }
                
                 context.Response.Redirect("/ui/pmui/eticket/twocodedetail.aspx?pno="+p);
             }
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}