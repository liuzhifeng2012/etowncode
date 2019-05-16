using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO.Compression;

namespace ETS.Framework
{
    public class PageCompression
    {
        private HttpContext context;


        public bool IsGZip { private set; get; }
        public bool IsDeflate { private set; get; }

        public PageCompression(HttpContext context)
        {
            this.context = context;
            this.Init();

        }

        public PageCompression()
            : this(HttpContext.Current)
        {
        }

        public void Init()
        {
            string acceptEncoding = context.Request.Headers["Accept-Encoding"];
            if (String.IsNullOrEmpty(acceptEncoding))
            {
                return;
            }
            acceptEncoding = acceptEncoding.ToLower();
            if (acceptEncoding.Contains("gzip"))
            {
                this.IsGZip = true;
            }
            if (acceptEncoding.Contains("deflate"))
            {
                this.IsDeflate = true;
            }
            if (acceptEncoding == "*")
            {
                this.IsDeflate = true;
            }
        }

        public void GZipEncodePage()
        {
            var stream = this.context.Response.Filter;
            if (IsGZip)
            {
                this.context.Response.Filter = new GZipStream(stream, CompressionMode.Compress);
                this.context.Response.AddHeader("Content-Encoding", "gzip");
                return;
            }
            if (IsDeflate)
            {
                this.context.Response.Filter = new DeflateStream(stream, CompressionMode.Compress);
                this.context.Response.AddHeader("Content-Encoding", "deflate");
            }

        }
    }
}
