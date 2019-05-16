using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.JsonFactory;
using System.Xml;
using Newtonsoft.Json;

namespace ETS2.WebApp.wxpay
{
    public partial class feedback : System.Web.UI.Page
    {
        public string nativePayImgurl = "/Images/defaultThumb.png";//微信原生支付二维码 
        protected void Page_Load(object sender, EventArgs e)
        {
            int comid = 106;
            string urljson = WeiXinJsonData.getNativePayQrcode(0, comid, "oid");
            try
            {
                XmlDocument retdoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + urljson + "}");
                XmlElement retroot = retdoc.DocumentElement;
                string type = retroot.SelectSingleNode("type").InnerText;
                string msg = retroot.SelectSingleNode("msg").InnerText;

                nativePayImgurl = "http://shop"+comid+".etown.cn/ui/pmui/eticket/showtcode.aspx?pno=" + msg;
            }
            catch
            { }
        }
    }
}