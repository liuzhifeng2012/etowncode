using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Data.InternalData;
using ETS2.WeiXin.Service.WeiXinService.Model;

namespace ETS2.WeiXin.Service.WeiXinService.Data
{
    public class WxRequestXmlData
    {
        /// <summary>
        /// 编辑微信访问记录日志
        /// </summary>
        /// <param name="requestXML"></param>
        /// <returns></returns>
        public int EditWxRequestXmlLog(RequestXML requestXML)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalWxRequestXml(sql);
                    int result = internalData.InsertOrUpdate(requestXML);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public List<RequestXML> GetWxErrSendMsgList(int comid, string ToUserName, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxRequestXml(helper).GetWxErrSendMsgList(comid, ToUserName, out totalcount);

                return list;
            }
        }

        public int UpWxErrSendMsgList(int id)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxRequestXml(helper).UpWxErrSendMsgList(id);

                return list;
            }
        }


        public List<RequestXML> GetWxSendMsgListByComid(int userid, int comid, int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxRequestXml(helper).GetWxSendMsgListByComid(userid, comid, pageindex, pagesize, out totalcount);

                return list;
            }
        }

        public bool JudgeWhetherReply(DateTime CreateTimeFormat, string FromUserName ,int comid)
        {
             using (var helper=new SqlHelper())
             {
                 var result = new InternalWxRequestXml(helper).JudgeWhetherReply(CreateTimeFormat, FromUserName, comid);
                 return result;
             }
        }


        public List<RequestXML> GetWxSendMsgListByFromUser(int comid, string fromusername, int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxRequestXml(helper).GetWxSendMsgListByFromUser(comid,fromusername, pageindex, pagesize, out totalcount);

                return list;
            }
        }

        public List<RequestXML> Getzixunlog(int comid, int pageindex, int pagesize, string userweixin, string guwenweixin,string key, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxRequestXml(helper).Getzixunlog(comid, pageindex, pagesize, userweixin, guwenweixin,key, out totalcount);

                return list;
            }
        }


        public List<RequestXML> GetWxSendMsgListByTop5(int comid, int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxRequestXml(helper).GetWxSendMsgListByTop5(comid, pageindex, pagesize, out totalcount);

                return list;
            }
        }

        public string GetLasterReplyTime(DateTime CreateTimeFormat, string FromUserName, int comid)
        {
            using (var helper = new SqlHelper())
            {
                var result = new InternalWxRequestXml(helper).GetLasterReplyTime(CreateTimeFormat, FromUserName, comid);
                return result;
            }
        }

        public bool JudgeWhetherRenZheng(int comid)
        {
            using (var helper = new SqlHelper())
            {
                var result = new InternalWxRequestXml(helper).JudgeWhetherRenZheng(comid);
                return result;
            }
        }

        internal bool GetWhetherSendAutoReply(string FromUserName, string defaultret)
        {
            using (var helper = new SqlHelper())
            {
                var result = new InternalWxRequestXml(helper).GetWhetherSendAutoReply(FromUserName, defaultret);
                return result;
            }
        }

        public int AdjustQrcodeStatus(int sourceid, int onlinestatus)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalWxRequestXml(helper).AdjustQrcodeStatus(sourceid, onlinestatus);
                return result;
            }
        }
        /// <summary>
        /// 根据用户(FromUserName) 和 消息创建时间(CreateTime) 判断消息/事件是否接收过，接收过不在接收处理
        /// </summary>
        /// <param name="fromusername"></param>
        /// <param name="createtime"></param>
        /// <returns></returns>
        internal int JadgeMsg_isReceived(string fromusername, string createtime)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalWxRequestXml(helper).JadgeMsg_isReceived(fromusername, createtime);
                return result;
            }
        }
        /// <summary>
        /// 获取顾问咨询记录(除了系统记录的)
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public List<RequestXML> GetWxrequestxmlByopenid(int pageindex,int pagesize,out int totalcount, string openid, string sysweixin)
        {
            using (var helper = new SqlHelper())
            {
                List<RequestXML> result = new InternalWxRequestXml(helper).GetWxrequestxmlByopenid(pageindex,pagesize,out totalcount,  openid,sysweixin);
                return result;
            }
        }


        public int GetWxErr_sms_SendMsgList(int comid, string mobile)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxRequestXml(helper).GetWxErr_sms_SendMsgList(comid, mobile);

                return list;
            }
        }

        public int InsertWxErr_sms_SendMsgList(int comid,string mobile)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxRequestXml(helper).InsertWxErr_sms_SendMsgList(comid,mobile);

                return list;
            }
        }


    }
}
