using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using ETS.Framework;

namespace ETS.JsonFactory
{
    public class JsonHandler : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Cache.SetExpires(DateTime.Now);
            context.Response.ContentType = jsonType;
            this.SetResponse(context);
            context.Response.End();
        }

        private const string jsonType = "application/json";
        private const string javascriptType = "application/javascript";


        private void SetResponse(HttpContext context)
        {
            string[] segments = context.Request.Url.Segments;
            string action = "";
            if (segments.Length < 3)
            {
                return;
            }
            action = segments[2].Trim('/').ToLower();
            JsonBase json = null;
            switch (action)
            {
                case "user":
                    json = new UserJosnData(context.Request);
                    break;

                default:
                    break;
            }
            if (json == null)
            {
                return;
            }

            string responseString = json.OutputData();

            var compress = new PageCompression(context);
            compress.GZipEncodePage();

            context.Response.Write(responseString);
        }
    }
}
