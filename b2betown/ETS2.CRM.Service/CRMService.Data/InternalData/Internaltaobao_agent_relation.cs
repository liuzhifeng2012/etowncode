using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.CRM.Service.CRMService.Data.InternalData
{
    public class Internaltaobao_agent_relation
    {
        public SqlHelper sqlHelper;
        public Internaltaobao_agent_relation(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal Taobao_agent_relation GetTb_agent_relation(int agentid, int serialnum)
        {
            string sql = @"SELECT [serialnum]
                              ,[tb_id]
                              ,[tb_seller_wangwangid]
                              ,[tb_seller_wangwang]
                              ,[tb_shop_name]
                              ,[tb_shop_url]
                              ,[tb_shop_state]
                              ,agentid 
                          FROM  [taobao_agent_relation] where agentid=@agentid and serialnum=@serialnum";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@agentid", agentid);
            cmd.AddParam("@serialnum", serialnum);

            using (var reader = cmd.ExecuteReader())
            {
                Taobao_agent_relation r = null;
                if (reader.Read())
                {
                    r = new Taobao_agent_relation
                    {
                        serialnum = reader.GetValue<int>("serialnum"),
                        tb_id = reader.GetValue<string>("tb_id"),
                        tb_seller_wangwangid = reader.GetValue<string>("tb_seller_wangwangid"),
                        tb_seller_wangwang = reader.GetValue<string>("tb_seller_wangwang"),
                        tb_shop_name = reader.GetValue<string>("tb_shop_name"),
                        tb_shop_url = reader.GetValue<string>("tb_shop_url"),
                        tb_shop_state = reader.GetValue<int>("tb_shop_state"),
                        agentid = reader.GetValue<int>("agentid")
                    };

                }
                return r;
            }
        }

        internal int EditpartTb_agent_relation(Taobao_agent_relation r)
        {
            if (r.serialnum == 0)
            {
                string sql = @"INSERT INTO [EtownDB].[dbo].[taobao_agent_relation]
                               ([tb_id]
                               
                               ,[tb_seller_wangwang]
                               ,[tb_shop_name]
                               ,[tb_shop_url]
                               ,[tb_shop_state]
                               ,[agentid])
                         VALUES
                               (@tb_id
                                
                               ,@tb_seller_wangwang
                               ,@tb_shop_name
                               ,@tb_shop_url
                               ,@tb_shop_state
                               ,@agentid);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@tb_id", r.tb_id);
                
                cmd.AddParam("@tb_seller_wangwang", r.tb_seller_wangwang);
                cmd.AddParam("@tb_shop_name", r.tb_shop_name);
                cmd.AddParam("@tb_shop_url", r.tb_shop_url);
                cmd.AddParam("@tb_shop_state", r.tb_shop_state);
                cmd.AddParam("@agentid", r.agentid);

                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
            else
            {
                string sql = @"UPDATE [EtownDB].[dbo].[taobao_agent_relation]
                               SET [tb_id] = @tb_id
                                  
                                  ,[tb_seller_wangwang] = @tb_seller_wangwang
                                  ,[tb_shop_name] = @tb_shop_name
                                  ,[tb_shop_url] = @tb_shop_url
                                  ,[tb_shop_state] = @tb_shop_state
                                  ,[agentid] = @agentid
                             WHERE  serialnum=@serialnum";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@serialnum", r.serialnum);
                cmd.AddParam("@tb_id", r.tb_id);
                 
                cmd.AddParam("@tb_seller_wangwang", r.tb_seller_wangwang);
                cmd.AddParam("@tb_shop_name", r.tb_shop_name);
                cmd.AddParam("@tb_shop_url", r.tb_shop_url);
                cmd.AddParam("@tb_shop_state", r.tb_shop_state);
                cmd.AddParam("@agentid", r.agentid);
                cmd.ExecuteNonQuery();
                return r.serialnum;
            }
        }

        internal IList<Taobao_agent_relation> GetTb_agent_relationList(int agentid)
        {
            string sql = @"SELECT [serialnum]
                              ,[tb_id]
                              ,[tb_seller_wangwangid]
                              ,[tb_seller_wangwang]
                              ,[tb_shop_name]
                              ,[tb_shop_url]
                              ,[tb_shop_state]
                              ,agentid 
                          FROM  [taobao_agent_relation] where agentid=@agentid order by serialnum desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@agentid", agentid);
            using (var reader = cmd.ExecuteReader())
            {
                List<Taobao_agent_relation> r = new List<Taobao_agent_relation>();
                while (reader.Read())
                {
                    r.Add(new Taobao_agent_relation
                    {
                        serialnum = reader.GetValue<int>("serialnum"),
                        tb_id = reader.GetValue<string>("tb_id"),
                        tb_seller_wangwangid = reader.GetValue<string>("tb_seller_wangwangid"),
                        tb_seller_wangwang = reader.GetValue<string>("tb_seller_wangwang"),
                        tb_shop_name = reader.GetValue<string>("tb_shop_name"),
                        tb_shop_url = reader.GetValue<string>("tb_shop_url"),
                        tb_shop_state = reader.GetValue<int>("tb_shop_state"),
                        agentid = reader.GetValue<int>("agentid")
                    });
                }
                return r;
            }
        }

        internal int EditTb_agent_relation(Taobao_agent_relation r)
        {
            if (r.serialnum == 0)
            {
                string sql = @"INSERT INTO [EtownDB].[dbo].[taobao_agent_relation]
                               ([tb_id]
                               ,tb_seller_wangwangid
                               ,[tb_seller_wangwang]
                               ,[tb_shop_name]
                               ,[tb_shop_url]
                               ,[tb_shop_state]
                               ,[agentid])
                         VALUES
                               (@tb_id
                                ,@tb_seller_wangwangid
                               ,@tb_seller_wangwang
                               ,@tb_shop_name
                               ,@tb_shop_url
                               ,@tb_shop_state
                               ,@agentid);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@tb_id", r.tb_id);
                cmd.AddParam("@tb_seller_wangwangid", r.tb_seller_wangwangid);
                cmd.AddParam("@tb_seller_wangwang", r.tb_seller_wangwang);
                cmd.AddParam("@tb_shop_name", r.tb_shop_name);
                cmd.AddParam("@tb_shop_url", r.tb_shop_url);
                cmd.AddParam("@tb_shop_state", r.tb_shop_state);
                cmd.AddParam("@agentid", r.agentid);

                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
            else
            {
                string sql = @"UPDATE [EtownDB].[dbo].[taobao_agent_relation]
                               SET [tb_id] = @tb_id
                                  ,tb_seller_wangwangid=@tb_seller_wangwangid
                                  ,[tb_seller_wangwang] = @tb_seller_wangwang
                                  ,[tb_shop_name] = @tb_shop_name
                                  ,[tb_shop_url] = @tb_shop_url
                                  ,[tb_shop_state] = @tb_shop_state
                                  ,[agentid] = @agentid
                             WHERE  serialnum=@serialnum";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@serialnum", r.serialnum);
                cmd.AddParam("@tb_id", r.tb_id);
                cmd.AddParam("@tb_seller_wangwangid", r.tb_seller_wangwangid);
                cmd.AddParam("@tb_seller_wangwang", r.tb_seller_wangwang);
                cmd.AddParam("@tb_shop_name", r.tb_shop_name);
                cmd.AddParam("@tb_shop_url", r.tb_shop_url);
                cmd.AddParam("@tb_shop_state", r.tb_shop_state);
                cmd.AddParam("@agentid", r.agentid);
                cmd.ExecuteNonQuery();
                return r.serialnum;
            }
        }

        //internal bool IsAskByTbid(string tbid)
        //{
        //    string sql = "select count(1) from taobao_agent_relation where tb_id='"+tbid+"'";
        //    var cmd = sqlHelper.PrepareTextSqlCommand(sql);
        //    object o = cmd.ExecuteScalar();
        //    int c = int.Parse(o.ToString());
        //    if (c > 0)
        //    {
        //        return true;
        //    }
        //    else 
        //    {
        //        return false;
        //    }
        //}

        internal bool IsbindAgentBytbwangwangid(string wangwangid, int agentid)
        {
            string sql = "select count(1) from taobao_agent_relation where tb_seller_wangwangid='" + wangwangid + "' and agentid!="+agentid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            int c = int.Parse(o.ToString());
            if (c > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal bool IsbAgentBytbwangwangid(string wangwangid)
        {
            string sql = "select count(1) from taobao_agent_relation where tb_seller_wangwangid='" + wangwangid + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            int c = int.Parse(o.ToString());
            if (c > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal bool IsAskByTbWangwang(string wangwang)
        {
            string sql = "select count(1) from taobao_agent_relation where tb_seller_wangwang='" +  wangwang + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            int c = int.Parse(o.ToString());
            if (c > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
