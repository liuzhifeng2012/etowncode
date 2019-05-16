using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;
using ThoughtWorks.QRCode.Codec.Util;
using System.IO;
using System.Drawing;
namespace ETS2.WebApp.UI.PMUI.ETicket
{
    public partial class ShowTcode : System.Web.UI.Page
    {
        public string pno = "";//电子码
        protected void Page_Load(object sender, EventArgs e)
        {
           int QRCodeScale = 4;
           pno = Request["pno"].ConvertTo<string>("");
           QRCodeScale = Request["big"].ConvertTo<int>(4);

            //创建二维码
            creatQRCodeImage(QRCodeScale);

        }
        protected void creatQRCodeImage(int QRCodeS)
        {
            if (pno == "")
            {
                msg.InnerText = "电子码未传递过来";
                return;
            }

            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            //设置编码类型
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;

            //设置尺寸
            qrCodeEncoder.QRCodeScale = QRCodeS;

            //设置版本
            qrCodeEncoder.QRCodeVersion = 0;

            //设置纠错
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;

            try
            {
                Bitmap bmp;
                bmp = qrCodeEncoder.Encode(pno);//你获得的图象
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                Response.ContentType = "image/bmp";
                Response.BinaryWrite(ms.ToArray());
            }
            catch (Exception ex)
            {
                msg.InnerText = "Invalid version !" + ex.Message;
            }
        }

    }
}