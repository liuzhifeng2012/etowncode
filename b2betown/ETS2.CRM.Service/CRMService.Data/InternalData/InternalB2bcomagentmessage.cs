using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;
using System.Data;

namespace ETS2.CRM.Service.CRMService.Data.InternalData
{
    public class InternalB2bcomagentmessage
    {
        
        private SqlHelper sqlHelper;
        public InternalB2bcomagentmessage(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }


        #region 分销商通知列表
        internal List<B2b_com_agent_message> AgentMessageList(int Pageindex, int Pagesize, int comid, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "b2b_com_agent_message";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";
            var condition = "comid=" + comid ;


            cmd.PagingCommand(tblName, strGetFields, Pageindex, Pagesize, sortKey, sortMode, condition);


            List<B2b_com_agent_message> list = new List<B2b_com_agent_message>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_com_agent_message
                    {
                        Id = reader.GetValue<int>("id"),
                        Comid = reader.GetValue<int>("Comid"),
                        Title = reader.GetValue<string>("Title"),
                        Message = reader.GetValue<string>("Message"),
                        State = reader.GetValue<int>("State"),
                        Subtime = reader.GetValue<DateTime>("Subtime"),
                      
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());
            return list;

        }

        #endregion


        #region 分销商通知列表
        internal List<B2b_com_agent_message> AgentViewMessageList(int Pageindex, int Pagesize, int comid,int agentid, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "b2b_com_agent_message";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";
            var condition = "State =1 and comid in (select comid from [agent_warrant] where warrant_state=1 and comid=" + comid + " and agentid=" + agentid + ")";


            cmd.PagingCommand(tblName, strGetFields, Pageindex, Pagesize, sortKey, sortMode, condition);


            List<B2b_com_agent_message> list = new List<B2b_com_agent_message>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_com_agent_message
                    {
                        Id = reader.GetValue<int>("id"),
                        Comid = reader.GetValue<int>("Comid"),
                        Title = reader.GetValue<string>("Title"),
                        Message = reader.GetValue<string>("Message"),
                        State = reader.GetValue<int>("State"),
                        Subtime = reader.GetValue<DateTime>("Subtime"),

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());
            return list;

        }

        #endregion


        #region 分销通知详情
        /// <summary>
        /// 获得分销公司信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal B2b_com_agent_message AgentMessageInfo(int id,int comid)
        {
            string sql = @"SELECT * FROM b2b_com_agent_message where id=@id and comid=@comid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);
            cmd.AddParam("@comid", comid);

            B2b_com_agent_message u = null;
            using (var reader = cmd.ExecuteReader())
            {

                if (reader.Read())
                {
                    u = new B2b_com_agent_message
                    {
                        Id = reader.GetValue<int>("id"),
                        Comid = reader.GetValue<int>("Comid"),
                        Title = reader.GetValue<string>("Title"),
                        Message = reader.GetValue<string>("Message"),
                        State = reader.GetValue<int>("State"),
                        Subtime = reader.GetValue<DateTime>("Subtime"),
                    };
                }
            }
            return u;
        }
        #endregion


        #region 插入最后查看时间
        /// <summary>
        /// 插入最后查看时间
        /// </summary>
        /// <param name="comid"></param>
        /// <param name="agentid"></param>
        /// <param name="ViewEndtime">默认查一个月内的通知，以后有最后查看时间，列出所有最后一次查看后的数量</param>
        /// <returns></returns>
        internal int AgentMessageNew(int agentid, int comid,DateTime ViewEndtime)
        {
            string sql = @"SELECT count(id) as id FROM b2b_com_agent_message where comid=@comid and state=1 and subtime>@ViewEndtime";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@comid", comid);
                cmd.AddParam("@ViewEndtime", ViewEndtime);

                using (var reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        return reader.GetValue<int>("id");
                    }
                }
                return 0;
            }
            catch {
                return 0;
            }
        }
        #endregion

        #region 分销商通知编辑
        internal int AgentMessageUp(B2b_com_agent_message model)
        {
            string sqlTxt = @"update b2b_com_agent_message set Title =@Title,Message =@Message,State =@State Where id=@Id and comid=@comid;select @@identity;";

            if (model.Id == 0) {//id为0 做成插入
                sqlTxt = "insert b2b_com_agent_message (comid,title,message,subtime) values (@comid,@Title,@Message,@Subtime) ";
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            //分销商通知编辑
            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@comid", model.Comid);
            cmd.AddParam("@Title", model.Title);
            cmd.AddParam("@Message", model.Message);
            cmd.AddParam("@State", model.State);
            cmd.AddParam("@Subtime", DateTime.Now);

            return cmd.ExecuteNonQuery();

        }
        #endregion




    }
}
