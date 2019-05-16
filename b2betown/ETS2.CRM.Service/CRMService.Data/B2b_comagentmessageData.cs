using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Data.InternalData;
using System.Web;
namespace ETS2.CRM.Service.CRMService.Data
{
    public class B2b_comagentmessageData
    {
        #region 分销商通知列表
        public List<B2b_com_agent_message> AgentMessageList(int Pageindex, int Pagesize, int comid, out int totalcount)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bcomagentmessage(sql);
                    var result = internalData.AgentMessageList(Pageindex, Pagesize,comid, out totalcount);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 分销商查看通知列表
        public List<B2b_com_agent_message> AgentViewMessageList(int Pageindex, int Pagesize, int comid,int agentid, out int totalcount)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bcomagentmessage(sql);
                    var result = internalData.AgentViewMessageList(Pageindex, Pagesize, comid, agentid, out totalcount);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 分销通知详情
        public B2b_com_agent_message AgentMessageInfo(int id, int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bcomagentmessage(sql);
                    var result = internalData.AgentMessageInfo(id,comid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 分销通知是否有最新的 //ViewEndtime 插入最后查看时间
        public int AgentMessageNew(int agentid, int comid,DateTime ViewEndtime)
        {
           

            using (var sql = new SqlHelper())
            {
                try
                {
                    //读取此商户的COOCKI
                    if (System.Web.HttpContext.Current.Request.Cookies["agentmessage" + comid] != null)
                    {
                        ViewEndtime = DateTime.Parse(System.Web.HttpContext.Current.Request.Cookies["agentmessage" + comid].Value);
                    }
                    var internalData = new InternalB2bcomagentmessage(sql);
                    var result = internalData.AgentMessageNew(agentid, comid, ViewEndtime);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 分销商通知编辑
        public static int AgentMessageUp(B2b_com_agent_message regiinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bcomagentmessage(sql);
                    int result = internalData.AgentMessageUp(regiinfo);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


    }
}
