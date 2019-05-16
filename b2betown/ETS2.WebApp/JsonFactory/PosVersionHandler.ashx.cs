using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Framework;
using ETS.JsonFactory;

namespace ETS2.WebApp.JsonFactory
{
    /// <summary>
    /// PosVersionHandler 的摘要说明
    /// </summary>
    public class PosVersionHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.ContentType = "text/plain";

            string oper = context.Request["oper"].ConvertTo<string>("");
            if (oper != "")
            {

                if (oper == "posversionpagelist")
                {
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var data = PosVersionJsonData.GetPosVersionPageList(pageindex,pagesize);
                    context.Response.Write(data);
                }
                if (oper == "updateposversion")
                {
                
                }
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