using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Framework;

namespace ETS2.WebApp.TicketService
{
    /// <summary>
    /// httpservice 的摘要说明
    /// </summary>
    public class httpservice : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string organization = context.Request["organization"].ConvertTo<string>("");
            string xml = context.Request["xml"].ConvertTo<string>("");

            context.Response.ContentType = "text/plain";
            HttpService hservice = new HttpService();
            if (organization == "" || xml == "")
            {
                //context.Response.Write("请传入请求参数");
                string ret = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                    "<business_trans  version=\"1.0\">" +
                        "<response_type>ERROR</response_type>" +
                        "<req_seq></req_seq>" +
                            "<result>" +
                                "<id>1111</id>" +
                                "<comment><![CDATA[请传入请求参数]]></comment>" +
                            "</result>" +
                    "</business_trans>";
                context.Response.Write(ret);
                return;
            }
            string rxml = hservice.getEleInterface(organization, xml);

            context.Response.Write(rxml);
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