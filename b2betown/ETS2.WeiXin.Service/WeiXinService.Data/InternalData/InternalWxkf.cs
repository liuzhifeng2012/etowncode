using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Model;

namespace ETS2.WeiXin.Service.WeiXinService.Data.InternalData
{
    public class InternalWxkf
    {
        private SqlHelper sqlHelper;
        public InternalWxkf(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }


        internal int Editwxkf(int kf_id, string kf_account, string kf_nick, int comid, int createuserid)
        {
            try
            {
                sqlHelper.BeginTrancation();
                string sql1 = "select count(1) from Wxkf where kf_id=" + kf_id + " and comid=" + comid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql1);
                object o = cmd.ExecuteScalar();
                if (int.Parse(o.ToString()) > 0)
                {
                    string sql2 = "update wxkf set kf_nick='" + kf_nick + "',kf_account='" + kf_account + "' where kf_id=" + kf_id + " and comid=" + comid;
                    cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    string sql3 = "INSERT INTO [EtownDB].[dbo].[Wxkf]([kf_id],[kf_nick],[kf_account],[yg_id],[yg_name],[ms_id] ,[ms_name],[comid],[isrun],[createtime],[createuserid])" +
         " VALUES(" + kf_id + ",'" + kf_nick + "','" + kf_account + "',-1,'',-1,''," + comid + ",0,'" + DateTime.Now + "'," + createuserid + ")";
                    cmd = sqlHelper.PrepareTextSqlCommand(sql3);
                    cmd.ExecuteNonQuery();
                }


                sqlHelper.Commit();
                sqlHelper.Dispose();
                return 1;
            }
            catch
            {

                sqlHelper.Rollback();
                sqlHelper.Dispose();
                return 0;
            }
         
        }
        /*获取微信客服列表(1.总公司登录显示全部2.门市登录显示本门市绑定客服)*/
        internal List<Wxkf> Getwxkfpagelist(int pageindex, int pagesize, int comid, out int totalcount, int userid = 0, string isrun = "0,1", string key = "")
        {
            string condition = " comid=" + comid;
            if (isrun != "0,1")
            {
                condition += " and isrun in (" + isrun + ")";
            }

            if (userid > 0)
            {
                try
                {
                    string sql2 = "select channelcompanyid from b2b_company_manageuser where id=" + userid;
                    var cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                    object o = cmd.ExecuteScalar();
                    sqlHelper.Dispose();
                    if (int.Parse(o.ToString()) > 0)
                    {
                        condition += " and ms_id in (" + int.Parse(o.ToString()) + ")";
                    }
                }
                catch
                { sqlHelper.Dispose(); }
            }
            if (key != "")
            {
                condition += "  and ( kf_id like '%" + key + "%' or kf_nick like '%" + key + "%'  or kf_account like '%" + key + "%' or yg_name like '%" + key + "%' or ms_name like '%" + key + "%')";
            }

            var cmdd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            cmdd.PagingCommand1("wxkf", "*", "isbinded,kf_id desc", "", pagesize, pageindex, "", condition);

            List<Wxkf> list = new List<Wxkf>();
            using (var reader = cmdd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Wxkf()
                    {
                        Id = reader.GetValue<int>("id"),
                        Kf_id = reader.GetValue<int>("kf_id"),
                        Kf_nick = reader.GetValue<string>("kf_nick"),
                        Kf_account = reader.GetValue<string>("kf_account"),
                        Yg_id = reader.GetValue<int>("yg_id"),
                        Yg_name = reader.GetValue<string>("yg_name"),
                        Ms_id = reader.GetValue<int>("ms_id"),
                        Ms_name = reader.GetValue<string>("ms_name"),
                        Comid = reader.GetValue<int>("comid"),
                        Isonline = reader.GetValue<int>("isonline"),
                        Isbinded = reader.GetValue<int>("isbinded"),
                        Iszongkf = reader.GetValue<int>("iszongkf"),
                        Isrun = reader.GetValue<int>("isrun"),
                        Kf_status = reader.GetValue<int>("kf_status"),
                        Auto_accept = reader.GetValue<int>("auto_accept"),
                        Accepted_case = reader.GetValue<int>("accepted_case")
                    });
                }
            }
            totalcount = int.Parse(cmdd.Parameters[0].Value.ToString());
            return list;

        }

        internal int DelWxkf(int kfid, int comid)
        {
            string sql = "delete wxkf where kf_id=" + kfid + " and comid=" + comid;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal Wxkf Getwxkf(int kfid, int comid)
        {
            string sql = "SELECT    *  FROM [EtownDB].[dbo].[Wxkf] where kf_id=" + kfid + " and comid=" + comid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                Wxkf m = null;
                if (reader.Read())
                {
                    m = new Wxkf()
                    {
                        Id = reader.GetValue<int>("id"),
                        Kf_id = reader.GetValue<int>("kf_id"),
                        Kf_nick = reader.GetValue<string>("kf_nick"),
                        Kf_account = reader.GetValue<string>("kf_account"),
                        Yg_id = reader.GetValue<int>("yg_id"),
                        Yg_name = reader.GetValue<string>("yg_name"),
                        Ms_id = reader.GetValue<int>("ms_id"),
                        Ms_name = reader.GetValue<string>("ms_name"),
                        Comid = reader.GetValue<int>("comid"),
                        Isonline = reader.GetValue<int>("isonline"),
                        Isbinded = reader.GetValue<int>("isbinded"),
                        Iszongkf = reader.GetValue<int>("iszongkf"),
                        Isrun = reader.GetValue<int>("isrun"),
                        Kf_status = reader.GetValue<int>("kf_status"),
                        Auto_accept = reader.GetValue<int>("auto_accept"),
                        Accepted_case = reader.GetValue<int>("accepted_case")
                    };
                }
                return m;
            }
        }

        internal int BindWxdkf(Wxkf m)
        {
            string sql = "update wxkf set iszongkf=" + m.Iszongkf + ",isbinded=" + m.Isbinded + ", yg_id=" + m.Yg_id + ",yg_name='" + m.Yg_name + "',ms_id=" + m.Ms_id + ",ms_name='" + m.Ms_name + "',isrun=" + m.Isrun + ",createtime='" + m.Createtime + "',createuserid=" + m.Createuserid + "  where comid=" + m.Comid + "  and kf_id=" + m.Kf_id;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int UpWxkf_downline(int comid)
        {
            string sql = "update wxkf set isonline=0,kf_status=0,auto_accept=0,accepted_case=0 where comid=" + comid;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int Upwxkf_online(int kf_id, int Comid, int status, int auto_accept, int accepted_case)
        {
            string sql = "update wxkf set isonline=1,kf_status=" + status + ",auto_accept=" + auto_accept + ",accepted_case=" + accepted_case + " where comid=" + Comid + " and kf_id=" + kf_id;


            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal List<Wxkf> Getwxkflist(int comid, string isonline, string isrun)
        {
            string sql = "SELECT  *  FROM [EtownDB].[dbo].[Wxkf] where comid="+comid+" and isonline in ("+isonline+") and isrun in ("+isrun+")";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
          
            List<Wxkf> list = new List<Wxkf>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Wxkf
                    {
                        Id = reader.GetValue<int>("id"),
                        Kf_id = reader.GetValue<int>("kf_id"),
                        Kf_nick = reader.GetValue<string>("kf_nick"),
                        Kf_account = reader.GetValue<string>("kf_account"),
                        Yg_id = reader.GetValue<int>("yg_id"),
                        Yg_name = reader.GetValue<string>("yg_name"),
                        Ms_id = reader.GetValue<int>("ms_id"),
                        Ms_name = reader.GetValue<string>("ms_name"),
                        Comid = reader.GetValue<int>("comid"),
                        Isonline = reader.GetValue<int>("isonline"),
                        Isbinded = reader.GetValue<int>("isbinded"),
                        Iszongkf = reader.GetValue<int>("iszongkf"),
                        Isrun = reader.GetValue<int>("isrun"),
                        Kf_status = reader.GetValue<int>("kf_status"),
                        Auto_accept = reader.GetValue<int>("auto_accept"),
                        Accepted_case = reader.GetValue<int>("accepted_case")
                    });
                }
            }
            return list;

        }

        internal List<Wxkf> GetGs_wxkflist(int comid, string isonline, string isrun)
        {
            string sql = "SELECT *  FROM [EtownDB].[dbo].[Wxkf] where comid="+comid+" and isonline in ("+isonline+") and isrun in ("+isrun+") and ms_id=0";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
        
            List<Wxkf> list = new List<Wxkf>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Wxkf
                    {
                        Id = reader.GetValue<int>("id"),
                        Kf_id = reader.GetValue<int>("kf_id"),
                        Kf_nick = reader.GetValue<string>("kf_nick"),
                        Kf_account = reader.GetValue<string>("kf_account"),
                        Yg_id = reader.GetValue<int>("yg_id"),
                        Yg_name = reader.GetValue<string>("yg_name"),
                        Ms_id = reader.GetValue<int>("ms_id"),
                        Ms_name = reader.GetValue<string>("ms_name"),
                        Comid = reader.GetValue<int>("comid"),
                        Isonline = reader.GetValue<int>("isonline"),
                        Isbinded = reader.GetValue<int>("isbinded"),
                        Iszongkf = reader.GetValue<int>("iszongkf"),
                        Isrun = reader.GetValue<int>("isrun"),
                        Kf_status = reader.GetValue<int>("kf_status"),
                        Auto_accept = reader.GetValue<int>("auto_accept"),
                        Accepted_case = reader.GetValue<int>("accepted_case")
                    });
                }
            }
            return list;
        }

        internal Wxkf GetChannel_Wxkf(int channelid, int comid, string isonline, string isrun)
        {
            string sql = "select * from Wxkf where comid =" + comid + " and  isonline in (" + isonline + ") and isrun in (" + isrun + ") and   yg_id in (select id from b2b_company_manageuser where com_id=" + comid + " and tel in " +
" (select mobile from Member_Channel where id=" + channelid + "  and com_id=" + comid + "))";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                Wxkf m = null;
                if (reader.Read())
                {
                    m = new Wxkf
                    {
                        Id = reader.GetValue<int>("id"),
                        Kf_id = reader.GetValue<int>("kf_id"),
                        Kf_nick = reader.GetValue<string>("kf_nick"),
                        Kf_account = reader.GetValue<string>("kf_account"),
                        Yg_id = reader.GetValue<int>("yg_id"),
                        Yg_name = reader.GetValue<string>("yg_name"),
                        Ms_id = reader.GetValue<int>("ms_id"),
                        Ms_name = reader.GetValue<string>("ms_name"),
                        Comid = reader.GetValue<int>("comid"),
                        Isonline = reader.GetValue<int>("isonline"),
                        Isbinded = reader.GetValue<int>("isbinded"),
                        Iszongkf = reader.GetValue<int>("iszongkf"),
                        Isrun = reader.GetValue<int>("isrun"),
                        Kf_status = reader.GetValue<int>("kf_status"),
                        Auto_accept = reader.GetValue<int>("auto_accept"),
                        Accepted_case = reader.GetValue<int>("accepted_case")
                    };
                }
                return m;
            }
        }

        internal List<Wxkf> GetMs_wxkflist(int msid, string isonline, string isrun)
        {
            string sql = "select * from wxkf where ms_id=" + msid + " and isonline in (" + isonline + ") and isrun in (" + isrun + ")";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                List<Wxkf> list = new List<Wxkf>();
                while (reader.Read())
                {
                    list.Add(new Wxkf
                    {
                        Id = reader.GetValue<int>("id"),
                        Kf_id = reader.GetValue<int>("kf_id"),
                        Kf_nick = reader.GetValue<string>("kf_nick"),
                        Kf_account = reader.GetValue<string>("kf_account"),
                        Yg_id = reader.GetValue<int>("yg_id"),
                        Yg_name = reader.GetValue<string>("yg_name"),
                        Ms_id = reader.GetValue<int>("ms_id"),
                        Ms_name = reader.GetValue<string>("ms_name"),
                        Comid = reader.GetValue<int>("comid"),
                        Isonline = reader.GetValue<int>("isonline"),
                        Isbinded = reader.GetValue<int>("isbinded"),
                        Iszongkf = reader.GetValue<int>("iszongkf"),
                        Isrun = reader.GetValue<int>("isrun"),
                        Kf_status = reader.GetValue<int>("kf_status"),
                        Auto_accept = reader.GetValue<int>("auto_accept"),
                        Accepted_case = reader.GetValue<int>("accepted_case")
                    });
                }
                return list;
            }
        }

        internal Wxkf Getwxzkf(int comid, int msid)
        {
            string sql = "select * from wxkf where comid="+comid+" and iszongkf=1";
            if (msid > 0)
            {
                  sql = "select * from wxkf where comid="+comid+" and  ms_id="+msid+" and iszongkf=1";
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            using (var reader = cmd.ExecuteReader())
            {
                Wxkf m = null;
                if (reader.Read())
                {
                    m = new Wxkf
                    {
                        Id = reader.GetValue<int>("id"),
                        Kf_id = reader.GetValue<int>("kf_id"),
                        Kf_nick = reader.GetValue<string>("kf_nick"),
                        Kf_account = reader.GetValue<string>("kf_account"),
                        Yg_id = reader.GetValue<int>("yg_id"),
                        Yg_name = reader.GetValue<string>("yg_name"),
                        Ms_id = reader.GetValue<int>("ms_id"),
                        Ms_name = reader.GetValue<string>("ms_name"),
                        Comid = reader.GetValue<int>("comid"),
                        Isonline = reader.GetValue<int>("isonline"),
                        Isbinded = reader.GetValue<int>("isbinded"),
                        Iszongkf = reader.GetValue<int>("iszongkf"),
                        Isrun = reader.GetValue<int>("isrun"),
                        Kf_status = reader.GetValue<int>("kf_status"),
                        Auto_accept = reader.GetValue<int>("auto_accept"),
                        Accepted_case = reader.GetValue<int>("accepted_case")
                    };
                }
                return m;
            }
              
            
        }

        internal Wxkf Getwxkfbyygid(int ygid)
        {
            string sql = "select * from wxkf where yg_id="+ygid;
           

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            using (var reader = cmd.ExecuteReader())
            {
                Wxkf m = null;
                if (reader.Read())
                {
                    m = new Wxkf
                    {
                        Id = reader.GetValue<int>("id"),
                        Kf_id = reader.GetValue<int>("kf_id"),
                        Kf_nick = reader.GetValue<string>("kf_nick"),
                        Kf_account = reader.GetValue<string>("kf_account"),
                        Yg_id = reader.GetValue<int>("yg_id"),
                        Yg_name = reader.GetValue<string>("yg_name"),
                        Ms_id = reader.GetValue<int>("ms_id"),
                        Ms_name = reader.GetValue<string>("ms_name"),
                        Comid = reader.GetValue<int>("comid"),
                        Isonline = reader.GetValue<int>("isonline"),
                        Isbinded = reader.GetValue<int>("isbinded"),
                        Iszongkf = reader.GetValue<int>("iszongkf"),
                        Isrun = reader.GetValue<int>("isrun"),
                        Kf_status = reader.GetValue<int>("kf_status"),
                        Auto_accept = reader.GetValue<int>("auto_accept"),
                        Accepted_case = reader.GetValue<int>("accepted_case")
                    };
                }
                return m;
            }
              
        }
    }
}
