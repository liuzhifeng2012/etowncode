



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WeiXinService.Model
{
    //微信请求类

    public class RequestXML
    {
        private int id;
        private string toUserName = "";
        private string fromUserName = "";
        private string createTime = "";
        private string msgType = "";
        private string content = "";
        private string location_X = "";
        private string location_Y = "";
        private string scale = "";
        private string label = "";
        private string picUrl = "";
        public string eevent = "";
        public string eventKey = "";
        //以下为新增字段
        public string mediaId = "";
        public string format = "";
        public string recognition = "";
        public string thumbMediaId = "";
        public string title = "";
        public string description = "";
        public string url = "";
        public string msgId = "";
        public bool contentType = true;//微信交互消息类型(用户消息；系统回复消息)
        private string postStr = "";//传递过来的数据
        private DateTime createTimeFormat = DateTime.Now;//返回的数据
        private int comid;


        private string latitude;//地理位置纬度
        private string longitude;//地理位置经度
        private string precision;//地理位置精度


        private string tmplmsg_push_status;//模板消息推送状态

        private string scanResult;//扫码内容
        private int sendstate=1;

        public string ScanResult 
        {
            get { return this.scanResult; }
            set { this.scanResult = value; }
        }

        public string Tmplmsg_push_status
        {

            get { return tmplmsg_push_status; }

            set { tmplmsg_push_status = value; }

        }
        public string Latitude
        {

            get { return latitude; }

            set { latitude = value; }

        }
        public string Longitude
        {

            get { return longitude; }

            set { longitude = value; }

        }
        public string Precision
        {

            get { return precision; }

            set { precision = value; }

        }

        private string headimgurl;
        public string Headimgurl
        {

            get { return headimgurl; }

            set { headimgurl = value; }

        }
        private string city;
        public string City
        {

            get { return city; }

            set { city = value; }

        }
        private string province;
        public string Province
        {

            get { return province; }

            set { province = value; }

        }
        private string nickname;
        public string Nickname
        {

            get { return nickname; }

            set { nickname = value; }

        }

        private int sex = 0;
        public int Sex
        {

            get { return sex; }

            set { sex = value; }

        }

        private int crmid;//(客人信息对应会员表crm)
        public string crmname = "";
        public string crmphone = "";
        public int Crmid
        {

            get { return crmid; }

            set { crmid = value; }

        }
        public string Crmname
        {

            get { return crmname; }

            set { crmname = value; }

        }
        public string Crmphone
        {

            get { return crmphone; }

            set { crmphone = value; }

        }
        private int manageuserid;//(客服信息对应账户表b2b_company_manageuser)
        public string manageusername = "";

        public int Manageuserid
        {

            get { return manageuserid; }

            set { manageuserid = value; }

        }
        public string Manageusername
        {

            get { return manageusername; }

            set { manageusername = value; }

        }





        public string MediaId
        {

            get { return mediaId; }

            set { mediaId = value; }

        }
        public string Format
        {

            get { return format; }

            set { format = value; }

        }
        public string Recognition
        {

            get { return recognition; }

            set { recognition = value; }

        }
        public string ThumbMediaId
        {

            get { return thumbMediaId; }

            set { thumbMediaId = value; }

        }
        public string Title
        {

            get { return title; }

            set { title = value; }

        }
        public string Description
        {

            get { return description; }

            set { description = value; }

        }
        public string Url
        {

            get { return url; }

            set { url = value; }

        }
        public string MsgId
        {

            get { return msgId; }

            set { msgId = value; }

        }
        public bool ContentType
        {

            get { return contentType; }

            set { contentType = value; }

        }
        public int Comid
        {

            get { return comid; }

            set { comid = value; }

        }

        /// <summary>

        /// 标识列

        /// </summary>

        public int Id
        {

            get { return id; }

            set { id = value; }

        }



        /// <summary>

        /// 消息接收方微信号，一般为公众平台账号微信号

        /// </summary>

        public string ToUserName
        {

            get { return toUserName; }

            set { toUserName = value; }

        }





        /// <summary>

        /// 消息发送方微信号

        /// </summary>

        public string FromUserName
        {

            get { return fromUserName; }

            set { fromUserName = value; }

        }





        /// <summary>

        /// 创建时间

        /// </summary>

        public string CreateTime
        {

            get { return createTime; }

            set { createTime = value; }

        }




        /// <summary>

        /// 信息类型 地理位置:location,文本消息:text,消息类型:image

        /// </summary>

        public string MsgType
        {

            get { return msgType; }

            set { msgType = value; }

        }




        /// <summary>

        /// 信息内容

        /// </summary>

        public string Content
        {

            get { return content; }

            set { content = value; }

        }




        /// <summary>

        /// 地理位置纬度

        /// </summary>

        public string Location_X
        {

            get { return location_X; }

            set { location_X = value; }

        }




        /// <summary>

        /// 地理位置经度

        /// </summary>

        public string Location_Y
        {

            get { return location_Y; }

            set { location_Y = value; }

        }




        /// <summary>

        /// 地图缩放大小

        /// </summary>

        public string Scale
        {

            get { return scale; }

            set { scale = value; }

        }




        /// <summary>

        /// 地理位置信息

        /// </summary>

        public string Label
        {

            get { return label; }

            set { label = value; }

        }




        /// <summary>

        /// 图片链接，开发者可以用HTTP GET获取

        /// </summary>

        public string PicUrl
        {

            get { return picUrl; }

            set { picUrl = value; }

        }




        /// <summary>
        /// 事件类型，subscribe(订阅)、unsubscribe(取消订阅)、CLICK(自定义菜单点击事件)
        /// </summary>
        public string Eevent
        {
            get { return eevent; }

            set { eevent = value; }
        }



        /// <summary>
        /// 事件KEY值，与自定义菜单接口中KEY值对应
        /// </summary>
        public string EventKey
        {
            get { return eventKey; }

            set { eventKey = value; }
        }





        /// <summary>

        ///  传递过来的xml

        /// </summary>

        public string PostStr
        {

            get { return postStr; }

            set { postStr = value; }

        }




        public DateTime CreateTimeFormat
        {

            get { return createTimeFormat; }

            set { createTimeFormat = value; }

        }

        public int Sendstate
        {

            get { return sendstate; }

            set { sendstate = value; }

        }

        

    }
}
