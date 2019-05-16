using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WinXinService.BLL
{
    public class WeiXinBack
    {


        private string content = "";

        /// <summary>

        /// 文本返回的消息

        /// </summary>

        public string Content
        {

            get { return content; }

            set { content = value; }

        }
        private string fromusername = "";

        /// <summary>

        /// 发送方账号

        /// </summary>

        public string Fromusername
        {

            get { return fromusername; }

            set { fromusername = value; }

        }

        private int msgType = 0;

        /// <summary>

        /// MsgType判断返回的消息

        /// </summary>

        public int MsgType
        {

            get { return msgType; }

            set { msgType = value; }

        }

        public WeiXinBack(string content, string fromusername)
        {
            this.content = content;
            this.fromusername = fromusername;
        }

        /// <summary>
        /// 得到返回的文本消息
        /// </summary>
        /// <returns></returns>
        internal string GetReMsg()
        {

            string defaultret = "亲，感谢您的留言。微旅行微信人工客户服务正在建设中，您可选择输入下面对应数字继续操作：\n\n" +

                                "1 查看微旅行会员优惠活动列表；\n" +
                                "2 微信开卡；\n" +
                                "3 微旅行会员服务介绍。\n\n" +

                                "更多问题您可以电询微旅行客服部 4006888210";

          
            //if (content == "0")
            //{
            //    return "如果您已经有微旅行会员卡，请点击  <a href='http://v.etown.cn/m/Weixin.aspx?openid=" + this.fromusername + "'>设置我的会员账户</a> 专享更多优惠服务；\n\n " +
            //             "或点击查看  <a href='http://v.etown.cn/m/Default.aspx?openid=" + this.fromusername + "'>微旅行优惠活动及券卡列表</a>。";
            //}

            //else if (content == "1")
            //{
            //    MsgType = 1;//显示图文信息
            //    return "";
            //}
            //else if (content == "2")
            //{
            //    if (this.fromusername == "o8sTMjmqvD29JaJQ7RvbpovfwHqA")
            //    {
            //        return "vincent,欢迎你的加入";
            //    }
            //    else if (this.fromusername == "o8sTMjp20YEx4i_MhUG95_jepkyo")
            //    {
            //        return "星海，欢迎你的加入";
            //    }
            //    else
            //    {
            //        return defaultret;
            //    }
            //}
       
            if (content == "1")
            {
                return "<a href='http://v.etown.cn/m/Default.aspx?openid=" + this.fromusername + "'>微旅行优惠活动及券卡列表</a>";
            }
            else if (content == "2")
            {
                return "<a href='http://v.etown.cn/m/Weixin.aspx?openid=" + this.fromusername + "'>微信开卡</a>";
            }
            else if (content == "3")
            {
                return "1、	成为微旅行会员后可享受会员专享服务及特惠线路。\n" +
                        "2、	个人会员及企业会员持本卡需先开卡激活后方可使用。\n" +
                        "3、	持本卡在合作旅行社门市咨询或报名时需提前出示本卡,且每次消费时只能参与一种有效优惠活动。\n" +
                        "4、	本卡是实名制会员卡，每卡只能绑定一位专属会员，开卡后不能转让使用。如有丢失请联系微旅行客服补办。\n" +
                        "5、	请您妥善保管账户密码信息，如因自身原因导致会员账户信息泄漏而造成损失由会员自行承担。\n" +
                        "6、	本卡不能透支，不能提现，不退余额，不计利息。\n" +
                        "7、	使用本卡消费时应按所参与优惠活动标明的限定内容享受优惠，不能与商户/门店其他优惠活动同时使用。\n\n" +

                        "北京微旅程科技有限公司 微旅行会员服务部 \n" +
                        "2013-9-11";
            }
            else
            {
                return defaultret;
            }
        }
        /// <summary>
        /// 返回的图文信息
        /// </summary>
        /// <param name="FromUserName"></param>
        /// <returns></returns>
        internal string GetRePic(string FromUserName)
        {
            return "<item><Title><![CDATA[title1]]></Title><Description><![CDATA[description1]]></Description><PicUrl><![CDATA[http://shop.etown.cn/images/tempimg/01.jpg]]></PicUrl><Url><![CDATA[http://www.etown.cn/web-test/weilvxingh5/index.html]]></Url></item>";
        }
        /// <summary>
        /// 默认返回的提示
        /// </summary>
        /// <returns></returns>
        internal string GetDefault()
        {
            return "默认提示";
        }
    }
}
