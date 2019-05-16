using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internalagent_requestlog
    {
        private SqlHelper sqlHelper;
        public Internalagent_requestlog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int Is_secondreq(string organization, string req_seq, string request_type)
        {
            string sql = "select count(1) from agent_requestlog where organization=" + organization + " and req_seq='" + req_seq + "' and request_type='"+request_type+"'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            object o = cmd.ExecuteScalar();
            int r = o == null ? 0 : int.Parse(o.ToString());
            if (r > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        internal int Is_secondordernum(string organization, string ordernum, string request_type)
        {
            string sql = "select count(1) from agent_requestlog where organization=" + organization + " and ordernum='" + ordernum + "' and request_type='" + request_type + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            object o = cmd.ExecuteScalar();
            int r = o == null ? 0 : int.Parse(o.ToString());
            if (r > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        internal int Editagent_reqlog(Agent_requestlog m)
        {
            if (m.Id == 0)
            {
                string sql = "INSERT INTO [EtownDB].[dbo].[agent_requestlog]" +
                               "([organization]" +
                              " ,[encode_requeststr]" +
                              " ,[decode_requeststr]" +
                              " ,[request_time]" +
                              " ,[encode_returnstr]" +
                              " ,[decode_returnstr]" +
                              " ,[return_time]" +
                              " ,[errmsg]" +
                              " ,[request_type]" +
                              " ,[req_seq]" +
                             "  ,[ordernum]" +
                            "   ,[is_dealsuc]" +
                           "    ,[is_second_receivereq]" +
                          "    ,[request_ip])" +
                         "VALUES" +
                              " (" + m.Organization +
                              " ,'" + m.Encode_requeststr + "'" +
                               ",'" + m.Decode_requeststr + "'" +
                               ",'" + m.Request_time + "'" +
                               ",'" + m.Encode_returnstr + "'" +
                               ",'" + m.Decode_returnstr + "'" +
                               ",'" + m.Return_time + "'" +
                               ",'" + m.Errmsg + "'" +
                               ",'" + m.Request_type + "'" +
                               ",'" + m.Req_seq + "'" +
                               ",'" + m.Ordernum + "'" +
                               "," + m.Is_dealsuc +
                               "," + m.Is_second_receivereq +
                               ",'" + m.Request_ip + "');select @@identity;";

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                return o == null ? 0 : int.Parse(o.ToString());
            }
            else
            {
                string sql = "UPDATE [EtownDB].[dbo].[agent_requestlog]" +
                               "SET [organization] = " + m.Organization +
                                "  ,[encode_requeststr] = '" + m.Encode_requeststr + "'" +
                                 " ,[decode_requeststr] = '" + m.Decode_requeststr + "'" +
                                 " ,[request_time] = '" + m.Request_time + "'" +
                                 " ,[encode_returnstr] = '" + m.Encode_returnstr + "'" +
                                 " ,[decode_returnstr] = '" + m.Decode_returnstr + "'" +
                                 " ,[return_time] ='" + m.Return_time + "'" +
                                 " ,[errmsg] = '" + m.Errmsg + "'" +
                                "  ,[request_type] = '" + m.Request_type + "'" +
                                 " ,[req_seq] = '" + m.Req_seq + "'" +
                                 " ,[ordernum] = '" + m.Ordernum + "'" +
                                 " ,[is_dealsuc] = '" + m.Is_dealsuc + "'" +
                                 " ,[is_second_receivereq] = '" + m.Is_second_receivereq + "'" +
                                 " ,[request_ip] = '" + m.Request_ip + "'" +
                             " WHERE id=" + m.Id;

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.ExecuteNonQuery();
                return m.Id;
            }
        }

        internal bool Ismatch_ip(string organization, string Requestip)
        {
            string sql = "select count(1) from agent_ip where agentid='"+organization+"' and bindip='"+Requestip+"'";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                int r = o == null ? 0 : int.Parse(o.ToString());
                if (r > 0)
                {
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            catch 
            {
                return false;
            }
        }

        internal List<Agent_requestlog> GetAgent_requestlogByOrdernum(string ordernum,string request_type,string isdealsuc)
        {
            string sql = @"SELECT [id]
      ,[organization]
      ,[encode_requeststr]
      ,[decode_requeststr]
      ,[request_time]
      ,[encode_returnstr]
      ,[decode_returnstr]
      ,[return_time]
      ,[errmsg]
      ,[request_type]
      ,[req_seq]
      ,[ordernum]
      ,[is_dealsuc]
      ,[is_second_receivereq]
      ,[request_ip]
  FROM  [agent_requestlog] where ordernum=@ordernum and request_type=@requesttype and  is_dealsuc in (@isdealsuc)";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@ordernum",ordernum);
            cmd.AddParam("@requesttype", request_type);
            cmd.AddParam("@isdealsuc", isdealsuc);

            List<Agent_requestlog> list = new List<Agent_requestlog>();
            using(var reader=cmd.ExecuteReader())
            {
                while(reader.Read())
                {
                    list.Add(new Agent_requestlog() {
                     Id=reader.GetValue<int>("id"),
                     Organization = reader.GetValue<int>("organization"),
                     Encode_requeststr = reader.GetValue<string>("encode_requeststr"),
                     Decode_requeststr = reader.GetValue<string>("decode_requeststr"),
                     Request_time = reader.GetValue<DateTime>("request_time"),
                     Encode_returnstr = reader.GetValue<string>("encode_returnstr"),
                     Decode_returnstr = reader.GetValue<string>("decode_returnstr"),
                     Return_time = reader.GetValue<DateTime>("return_time"),
                     Errmsg = reader.GetValue<string>("errmsg"),
                     Request_type = reader.GetValue<string>("request_type"),
                     Req_seq = reader.GetValue<string>("req_seq"),
                     Ordernum = reader.GetValue<string>("ordernum"),
                     Is_dealsuc = reader.GetValue<int>("is_dealsuc"),
                     Is_second_receivereq = reader.GetValue<int>("is_second_receivereq"),
                     Request_ip = reader.GetValue<string>("request_ip")
                    });
                }
            }
            return list;
        }



        internal Agent_requestlog GetAgent_addorderlogByOrderIddaitype(string ordernum, string request_type, int isdealsuc)
        {
            string sql = @"SELECT [id]
      ,[organization]
      ,[encode_requeststr]
      ,[decode_requeststr]
      ,[request_time]
      ,[encode_returnstr]
      ,[decode_returnstr]
      ,[return_time]
      ,[errmsg]
      ,[request_type]
      ,[req_seq]
      ,[ordernum]
      ,[is_dealsuc]
      ,[is_second_receivereq]
      ,[request_ip]
  FROM  [agent_requestlog] where ordernum=@ordernum and request_type=@requesttype and  is_dealsuc in (@isdealsuc)";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@ordernum", ordernum);
            cmd.AddParam("@requesttype", request_type);
            cmd.AddParam("@isdealsuc", isdealsuc);


            using (var reader = cmd.ExecuteReader())
            {
                Agent_requestlog r = null;
                if (reader.Read())
                {
                    r = new Agent_requestlog()
                    {
                        Id = reader.GetValue<int>("id"),
                        Organization = reader.GetValue<int>("organization"),
                        Encode_requeststr = reader.GetValue<string>("encode_requeststr"),
                        Decode_requeststr = reader.GetValue<string>("decode_requeststr"),
                        Request_time = reader.GetValue<DateTime>("request_time"),
                        Encode_returnstr = reader.GetValue<string>("encode_returnstr"),
                        Decode_returnstr = reader.GetValue<string>("decode_returnstr"),
                        Return_time = reader.GetValue<DateTime>("return_time"),
                        Errmsg = reader.GetValue<string>("errmsg"),
                        Request_type = reader.GetValue<string>("request_type"),
                        Req_seq = reader.GetValue<string>("req_seq"),
                        Ordernum = reader.GetValue<string>("ordernum"),
                        Is_dealsuc = reader.GetValue<int>("is_dealsuc"),
                        Is_second_receivereq = reader.GetValue<int>("is_second_receivereq"),
                        Request_ip = reader.GetValue<string>("request_ip")
                    };
                }
                return r;
            }

        }

        internal Agent_requestlog GetAgent_addorderlogByOrderId(string ordernum, int isdealsuc)
        {
            string sql = @"SELECT [id]
      ,[organization]
      ,[encode_requeststr]
      ,[decode_requeststr]
      ,[request_time]
      ,[encode_returnstr]
      ,[decode_returnstr]
      ,[return_time]
      ,[errmsg]
      ,[request_type]
      ,[req_seq]
      ,[ordernum]
      ,[is_dealsuc]
      ,[is_second_receivereq]
      ,[request_ip]
  FROM  [agent_requestlog] where ordernum=@ordernum and request_type=@requesttype and  is_dealsuc in (@isdealsuc)";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@ordernum", ordernum);
            cmd.AddParam("@requesttype", "add_order");
            cmd.AddParam("@isdealsuc", isdealsuc);

           
            using (var reader = cmd.ExecuteReader())
            {
                Agent_requestlog  r = null;
                if (reader.Read())
                {
                    r=new Agent_requestlog()
                    {
                        Id = reader.GetValue<int>("id"),
                        Organization = reader.GetValue<int>("organization"),
                        Encode_requeststr = reader.GetValue<string>("encode_requeststr"),
                        Decode_requeststr = reader.GetValue<string>("decode_requeststr"),
                        Request_time = reader.GetValue<DateTime>("request_time"),
                        Encode_returnstr = reader.GetValue<string>("encode_returnstr"),
                        Decode_returnstr = reader.GetValue<string>("decode_returnstr"),
                        Return_time = reader.GetValue<DateTime>("return_time"),
                        Errmsg = reader.GetValue<string>("errmsg"),
                        Request_type = reader.GetValue<string>("request_type"),
                        Req_seq = reader.GetValue<string>("req_seq"),
                        Ordernum = reader.GetValue<string>("ordernum"),
                        Is_dealsuc = reader.GetValue<int>("is_dealsuc"),
                        Is_second_receivereq = reader.GetValue<int>("is_second_receivereq"),
                        Request_ip = reader.GetValue<string>("request_ip")
                    } ;
                }
                return r;
            }
         
        }

        internal Agent_requestlog GetAgent_addorderlogByReq_seq(string organization, string req_seq)
        {
            string sql = @"SELECT [id]
      ,[organization]
      ,[encode_requeststr]
      ,[decode_requeststr]
      ,[request_time]
      ,[encode_returnstr]
      ,[decode_returnstr]
      ,[return_time]
      ,[errmsg]
      ,[request_type]
      ,[req_seq]
      ,[ordernum]
      ,[is_dealsuc]
      ,[is_second_receivereq]
      ,[request_ip]
  FROM  [agent_requestlog] where organization=@organization and request_type=@requesttype and  req_seq =@req_seq and ordernum!='' and is_dealsuc='1'";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@organization", organization);
            cmd.AddParam("@requesttype", "add_order");
            cmd.AddParam("@req_seq", req_seq);


            using (var reader = cmd.ExecuteReader())
            {
                Agent_requestlog r = null;
                if (reader.Read())
                {
                    r = new Agent_requestlog()
                    {
                        Id = reader.GetValue<int>("id"),
                        Organization = reader.GetValue<int>("organization"),
                        Encode_requeststr = reader.GetValue<string>("encode_requeststr"),
                        Decode_requeststr = reader.GetValue<string>("decode_requeststr"),
                        Request_time = reader.GetValue<DateTime>("request_time"),
                        Encode_returnstr = reader.GetValue<string>("encode_returnstr"),
                        Decode_returnstr = reader.GetValue<string>("decode_returnstr"),
                        Return_time = reader.GetValue<DateTime>("return_time"),
                        Errmsg = reader.GetValue<string>("errmsg"),
                        Request_type = reader.GetValue<string>("request_type"),
                        Req_seq = reader.GetValue<string>("req_seq"),
                        Ordernum = reader.GetValue<string>("ordernum"),
                        Is_dealsuc = reader.GetValue<int>("is_dealsuc"),
                        Is_second_receivereq = reader.GetValue<int>("is_second_receivereq"),
                        Request_ip = reader.GetValue<string>("request_ip")
                    };
                }
                return r;
            }
        }
      
        internal Agent_requestlog GetAgent_addorderlogByReq_seq(string organization, string req_seq,int isdealsuc)
        {
            string sql = @"SELECT [id]
      ,[organization]
      ,[encode_requeststr]
      ,[decode_requeststr]
      ,[request_time]
      ,[encode_returnstr]
      ,[decode_returnstr]
      ,[return_time]
      ,[errmsg]
      ,[request_type]
      ,[req_seq]
      ,[ordernum]
      ,[is_dealsuc]
      ,[is_second_receivereq]
      ,[request_ip]
  FROM  [agent_requestlog] where organization=@organization and request_type=@requesttype and  req_seq =@req_seq and ordernum!='' and is_dealsuc=@is_dealsuc";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@organization", organization);
            cmd.AddParam("@requesttype", "add_order");
            cmd.AddParam("@req_seq", req_seq);
            cmd.AddParam("@is_dealsuc", isdealsuc);


            using (var reader = cmd.ExecuteReader())
            {
                Agent_requestlog r = null;
                if (reader.Read())
                {
                    r = new Agent_requestlog()
                    {
                        Id = reader.GetValue<int>("id"),
                        Organization = reader.GetValue<int>("organization"),
                        Encode_requeststr = reader.GetValue<string>("encode_requeststr"),
                        Decode_requeststr = reader.GetValue<string>("decode_requeststr"),
                        Request_time = reader.GetValue<DateTime>("request_time"),
                        Encode_returnstr = reader.GetValue<string>("encode_returnstr"),
                        Decode_returnstr = reader.GetValue<string>("decode_returnstr"),
                        Return_time = reader.GetValue<DateTime>("return_time"),
                        Errmsg = reader.GetValue<string>("errmsg"),
                        Request_type = reader.GetValue<string>("request_type"),
                        Req_seq = reader.GetValue<string>("req_seq"),
                        Ordernum = reader.GetValue<string>("ordernum"),
                        Is_dealsuc = reader.GetValue<int>("is_dealsuc"),
                        Is_second_receivereq = reader.GetValue<int>("is_second_receivereq"),
                        Request_ip = reader.GetValue<string>("request_ip")
                    };
                }
                return r;
            }
        }

        internal bool Getisselforder(string organization, string order_num)
        {
            string sql = "select count(1) from  agent_requestlog where organization='" + organization + "' and ordernum='" + order_num + "' and request_type='add_order'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            if (int.Parse(o.ToString()) > 0)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        internal bool lvmamaGetisselforder(string organization, string order_num)
        {
            string sql = "select count(1) from  agent_requestlog where organization='" + organization + "' and ordernum='" + order_num + "' and request_type='apply_code'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            if (int.Parse(o.ToString()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal Agent_requestlog GetAgent_cancelorderlogByReq_seq(string organization, string req_seq, int is_dealsuc)
        {
            string sql = @"SELECT  top 1 * 
  FROM  [agent_requestlog] where organization=@organization and request_type=@requesttype and  req_seq =@req_seq and ordernum!='' and is_dealsuc=@is_dealsuc";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@organization", organization);
            cmd.AddParam("@requesttype", "cancel_order");
            cmd.AddParam("@req_seq", req_seq);
            cmd.AddParam("@is_dealsuc", is_dealsuc);


            using (var reader = cmd.ExecuteReader())
            {
                Agent_requestlog r = null;
                if (reader.Read())
                {
                    r = new Agent_requestlog()
                    {
                        Id = reader.GetValue<int>("id"),
                        Organization = reader.GetValue<int>("organization"),
                        Encode_requeststr = reader.GetValue<string>("encode_requeststr"),
                        Decode_requeststr = reader.GetValue<string>("decode_requeststr"),
                        Request_time = reader.GetValue<DateTime>("request_time"),
                        Encode_returnstr = reader.GetValue<string>("encode_returnstr"),
                        Decode_returnstr = reader.GetValue<string>("decode_returnstr"),
                        Return_time = reader.GetValue<DateTime>("return_time"),
                        Errmsg = reader.GetValue<string>("errmsg"),
                        Request_type = reader.GetValue<string>("request_type"),
                        Req_seq = reader.GetValue<string>("req_seq"),
                        Ordernum = reader.GetValue<string>("ordernum"),
                        Is_dealsuc = reader.GetValue<int>("is_dealsuc"),
                        Is_second_receivereq = reader.GetValue<int>("is_second_receivereq"),
                        Request_ip = reader.GetValue<string>("request_ip")
                    };
                }
                return r;
            }
        }


        internal Agent_requestlog GetAgent_cdiscard_codelogByReq_seq(string organization, string req_seq, int is_dealsuc)
        {
            string sql = @"SELECT  top 1 * 
  FROM  [agent_requestlog] where organization=@organization and request_type=@requesttype and  req_seq =@req_seq and ordernum!='' and is_dealsuc=@is_dealsuc";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@organization", organization);
            cmd.AddParam("@requesttype", "discard_code");
            cmd.AddParam("@req_seq", req_seq);
            cmd.AddParam("@is_dealsuc", is_dealsuc);


            using (var reader = cmd.ExecuteReader())
            {
                Agent_requestlog r = null;
                if (reader.Read())
                {
                    r = new Agent_requestlog()
                    {
                        Id = reader.GetValue<int>("id"),
                        Organization = reader.GetValue<int>("organization"),
                        Encode_requeststr = reader.GetValue<string>("encode_requeststr"),
                        Decode_requeststr = reader.GetValue<string>("decode_requeststr"),
                        Request_time = reader.GetValue<DateTime>("request_time"),
                        Encode_returnstr = reader.GetValue<string>("encode_returnstr"),
                        Decode_returnstr = reader.GetValue<string>("decode_returnstr"),
                        Return_time = reader.GetValue<DateTime>("return_time"),
                        Errmsg = reader.GetValue<string>("errmsg"),
                        Request_type = reader.GetValue<string>("request_type"),
                        Req_seq = reader.GetValue<string>("req_seq"),
                        Ordernum = reader.GetValue<string>("ordernum"),
                        Is_dealsuc = reader.GetValue<int>("is_dealsuc"),
                        Is_second_receivereq = reader.GetValue<int>("is_second_receivereq"),
                        Request_ip = reader.GetValue<string>("request_ip")
                    };
                }
                return r;
            }
        }

        internal bool Getisselforderbyreq_sql(string organization, string req_seq)
        {
            string sql = "select count(1) from  agent_requestlog where organization='" + organization + "' and req_seq='" + req_seq + "' and request_type='add_order'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            if (int.Parse(o.ToString()) > 0)
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
