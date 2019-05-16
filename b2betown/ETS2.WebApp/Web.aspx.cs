using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.WeiXin.Service.WeiXinService.Data;
using System.Data.SqlClient;
using System.Data;

namespace ETS2.WebApp
{

    public partial class Web : System.Web.UI.Page
    {
        public string xml1 = "";
        //public string randomid1 = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //public string posid1 = "999999999";
        //public string key1 = "4Mds1hSvWkfTmNrWMv1KTIPj";
        string s = "<?xml version=\"1.0\" encoding=\"utf-8\"?><business_trans version=\"1.0\"><request_type>add_order</request_type><organization>1000001038</organization><req_seq>100010002</req_seq><order><product_num>12810</product_num><num>1</num><mobile>13488761102</mobile><use_date></use_date><real_name_type>0</real_name_type><real_name></real_name><yanzheng_method>0</yanzheng_method></order></business_trans>";

           


        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
        protected void Button1_Click(object sender, EventArgs e)

        {
            var randomidd = Randomid.Text;
            var posidd = Posid.Text;
            var keyd = Deskey.Text;
            ////随机ID + posid + key
            var str = randomidd + posidd + keyd;

            if (Randomid.Text == "" || Posid.Text == "" || Deskey.Text == "")
            {
                Label1.Text = "PosID、DesKey、随机编号 都不可为空";
            }

            //var md5result = EncryptionHelper.ToMD5(str, "UTF-8");

            var md5result =  EncryptionHelper.DESEnCode(s, "3R4FTF1E");

            Result.Text = md5result;
        }
        /// <summary>
        /// 生成查询格式字符串
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button2_Click(object sender, EventArgs e)
        {
            //if (Randomid.Text == "" || Posid.Text == "" || Deskey.Text == "")
            //{
            //    Label1.Text = "PosID、DesKey、随机编号 都不可为空";
            //}
            //string str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            //                 "<business_trans version=\"1.0\">" +
            //                    "<request_type>Searchrcode</request_type>" +
            //                    "<randomid>" + Randomid.Text + "</randomid>" +
            //                    "<pos_id>" + Posid.Text + "</pos_id>" +
            //                    "<qrcode>填写电子码</qrcode>" +
            //                    "<security_md5>" + EncryptionHelper.ToMD5(Randomid.Text + Posid.Text + Deskey.Text, "UTF-8") + "</security_md5>" +
            //                    "<verifycodemode>1</verifycodemode>" +
            //                "</business_trans>";
            //divmsg1.InnerText = str;
           

        }
        /// <summary>
        /// 生成验票格式字符串
        
/// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button3_Click(object sender, EventArgs e)
        {
            if (Randomid.Text == "" || Posid.Text == "" || Deskey.Text == "")
            {
                Label1.Text = "PosID、DesKey、随机编号 都不可为空";
            }

            string str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                            "<business_trans version=\"1.0\"><request_type>Verqrcode</request_type>" +
                            "<randomid>" + Randomid.Text + "</randomid>" +
                            "<pos_id>" + Posid.Text + "</pos_id>" +
                            "<qrcode>填写电子码</qrcode>" +
                            "<security_md5>" + EncryptionHelper.ToMD5(Randomid.Text + Posid.Text + Deskey.Text, "UTF-8") + "</security_md5>" +
                            "<num>验票数量</num>" +
                            "<verifycodemode>1</verifycodemode>" +
                            "</business_trans>";
            divmsg2.InnerText = str;
        }

        /// <summary>
        /// 生成查询服务 格式字符串
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button6_Click(object sender, EventArgs e)
        {
            if (Randomid.Text == "" || Posid.Text == "" || Deskey.Text == "")
            {
                Label1.Text = "PosID、DesKey、随机编号 都不可为空";
            }
            string str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                             "<business_trans version=\"1.0\">" +
                                "<request_type>SearchService</request_type>" +
                                "<randomid>" + Randomid.Text + "</randomid>" +
                                "<pos_id>" + Posid.Text + "</pos_id>" +
                                "<qrcode>填写电子码</qrcode>" +
                                "<security_md5>" + EncryptionHelper.ToMD5(Randomid.Text + Posid.Text + Deskey.Text, "UTF-8") + "</security_md5>" +
                            "</business_trans>";
            div1.InnerText = str;
        }
        /// <summary>
        /// 生成验证服务格式字符串
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button7_Click(object sender, EventArgs e)
        {
            if (Randomid.Text == "" || Posid.Text == "" || Deskey.Text == "")
            {
                Label1.Text = "PosID、DesKey、随机编号 都不可为空";
            }
            string str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                             "<business_trans version=\"1.0\">" +
                                "<request_type>VerqService</request_type>" +
                                "<randomid>" + Randomid.Text + "</randomid>" +
                                "<pos_id>" + Posid.Text + "</pos_id>" +
                                "<qrcode>填写电子码</qrcode>" +
                                "<security_md5>" + EncryptionHelper.ToMD5(Randomid.Text + Posid.Text + Deskey.Text, "UTF-8") + "</security_md5>" +
                                "<verqstate>请求验证服务 1为领取服务， 2为归还服务</verqstate>" +
                            "</business_trans>";
            div2.InnerText = str;
        }

        /// <summary>
        /// 补卡查询接口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button8_Click(object sender, EventArgs e)
        {
            if (Randomid.Text == "" || Posid.Text == "" || Deskey.Text == "")
            {
                Label1.Text = "PosID、DesKey、随机编号 都不可为空";
            }
            string str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                             "<business_trans version=\"1.0\">" +
                                "<request_type>CardFillSearch</request_type>" +
                                "<randomid>" + Randomid.Text + "</randomid>" +
                                "<pos_id>" + Posid.Text + "</pos_id>" +
                                "<qrcode>填写电子码</qrcode>" +
                                "<security_md5>" + EncryptionHelper.ToMD5(Randomid.Text + Posid.Text + Deskey.Text, "UTF-8") + "</security_md5>" +
                            "</business_trans>";
            div3.InnerText = str;
        }

        /// <summary>
        /// 补卡确认接口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button9_Click(object sender, EventArgs e)
        {
            if (Randomid.Text == "" || Posid.Text == "" || Deskey.Text == "")
            {
                Label1.Text = "PosID、DesKey、随机编号 都不可为空";
            }
            string str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                             "<business_trans version=\"1.0\">" +
                                "<request_type>CardFillApply</request_type>" +
                                "<randomid>" + Randomid.Text + "</randomid>" +
                                "<pos_id>" + Posid.Text + "</pos_id>" +
                                "<qrcode>填写电子码</qrcode>" +
                                "<security_md5>" + EncryptionHelper.ToMD5(Randomid.Text + Posid.Text + Deskey.Text, "UTF-8") + "</security_md5>" +
                                "<identity>用户卡表序号</identity>" +////卡用户序号
                                "<cardprintid>丢失的卡号（印刷的）</cardprintid>" + //丢失的印刷id
                            "</business_trans>";
            div4.InnerText = str;
        }

        /// <summary>
        /// 实体卡芯片id上传接口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button10_Click(object sender, EventArgs e)
        {
            if (Randomid.Text == "" || Posid.Text == "" || Deskey.Text == "")
            {
                Label1.Text = "PosID、DesKey、随机编号 都不可为空";
            }
            string str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                             "<business_trans version=\"1.0\">" +
                                "<request_type>CardRelation</request_type>" +
                                "<randomid>" + Randomid.Text + "</randomid>" +
                                "<pos_id>" + Posid.Text + "</pos_id>" +
                                "<qrcode>填写电子码</qrcode>" +
                                "<num>数量</num>" +
                                "<security_md5>" + EncryptionHelper.ToMD5(Randomid.Text + Posid.Text + Deskey.Text, "UTF-8") + "</security_md5>" +
                                "<cardidrelationlist>" +
                                "<cardidrelation>" +
                                "<cardid>32位卡号</cardid>" +
                                "<cardchipid>上传的芯片序号</cardchipid>" +//芯片id
                                "</cardidrelation>" +
                                "</cardidrelationlist>" +
                            "</business_trans>";
            div5.InnerText = str;
        }


        /// <summary>
        /// 闸机刷卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button11_Click(object sender, EventArgs e)
        {
            if (Randomid.Text == "" || Posid.Text == "" || Deskey.Text == "")
            {
                Label1.Text = "PosID、DesKey、随机编号 都不可为空";
            }
            string str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                             "<business_trans version=\"1.0\">" +
                                "<request_type>Paycard</request_type>" +
                                "<randomid>" + Randomid.Text + "</randomid>" +
                                "<pos_id>" + Posid.Text + "</pos_id>" +
                                "<CardID>卡号</CardID>" +
                                "<appID>卡号</appID>" +
                                "<cmd>说明</cmd>" +
                                "<security_md5>" + EncryptionHelper.ToMD5(Randomid.Text + Posid.Text + Deskey.Text, "UTF-8") + "</security_md5>" +
                            "</business_trans>";
            div6.InnerText = str;
        }

        /// <summary>
        /// 生成冲正格式字符串
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button4_Click(object sender, EventArgs e)
        {
            string str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                             "<business_trans version=\"1.0\">" +
                             "<request_type>Reverse</request_type>" +
                             "<randomid>随机码</randomid>" +
                             "<pos_id>posid</pos_id>" +
                             "<qrcode>电子码</qrcode>" +
                             "<security_md5>md5加密后</security_md5>" +
                             "<num>验证数目</num>" +
                             "</business_trans>";
            divmsg3.InnerText = str;
        }
        /// <summary>
        /// 生成淘宝冲正格式字符串
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button5_Click(object sender, EventArgs e)
        {
            string str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                             "<business_trans version=\"1.0\">" +
                             "<request_type>Reverse</request_type>" +
                             "<randomid>随机码</randomid>" +
                             "<pos_id>posid</pos_id>" +
                             "<qrcode>电子码</qrcode>" +
                             "<security_md5>md5加密后</security_md5>" +
                             "<num>验证数目</num>" +
                             "<tb_order_id>淘宝订单号</tb_order_id>" +
                             "<tb_selfdefine_serial_num>对应淘宝核销流水号</tb_selfdefine_serial_num>" +
                             "<tb_token>安全验证token(需要和该订单淘宝发码通知中的token)</tb_token>" +
                             "</business_trans>";
            divmsg4.InnerText = str;
        }
    }
}