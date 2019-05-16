using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.Permision.Service.PermisionService.Model;

namespace ETS2.Permision.Service.PermisionService.Data.InternalData
{
    public class InternalSys_subnav
    {
        public SqlHelper sqlHelper;
        public InternalSys_subnav(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal Sys_subnav Getsyssubnav(int subnavid)
        {
            string sql = @"SELECT [id]
                              ,[subnav_name]
                              ,[subnav_url]
                              ,[viewcode]
                              ,[sortid]
                              ,[actioncolumnid]
                              ,[actionid]
                          FROM  [sys_subnav] where id=@id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", subnavid);
            using (var reader = cmd.ExecuteReader())
            {
                Sys_subnav m = null;
                if (reader.Read())
                {
                    m = new Sys_subnav
                    {
                        id = reader.GetValue<int>("id"),
                        subnav_name = reader.GetValue<string>("subnav_name"),
                        subnav_url = reader.GetValue<string>("subnav_url"),
                        viewcode = reader.GetValue<int>("viewcode"),
                        sortid = reader.GetValue<int>("sortid"),
                        actioncolumnid = reader.GetValue<int>("actioncolumnid"),
                        actionid = reader.GetValue<int>("actionid"),

                    };
                }
                return m;
            }
        }

        internal int Editsys_subnav(int id, int actionid, int columnid, string subnavurl, string subnavname)
        {
            if (id == 0)
            {
                string sql = @"INSERT INTO  [sys_subnav]
                                       ([subnav_name]
                                       ,[subnav_url] 
                                       ,[actioncolumnid]
                                       ,[actionid])
                                 VALUES
                                       (@subnav_name 
                                       ,@subnav_url  
                                       ,@actioncolumnid 
                                       ,@actionid);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@subnav_name", subnavname);
                cmd.AddParam("@subnav_url", subnavurl);
                cmd.AddParam("@actioncolumnid", columnid);
                cmd.AddParam("@actionid", actionid);
                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
            else
            {
                string sql = @"UPDATE  [sys_subnav]
                               SET [subnav_name] = @subnav_name 
                                  ,[subnav_url] = @subnav_url 
                                  
                                  ,[actioncolumnid] = @actioncolumnid 
                                  ,[actionid] = @actionid 
                             WHERE id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@subnav_name", subnavname);
                cmd.AddParam("@subnav_url", subnavurl);
                cmd.AddParam("@actioncolumnid", columnid);
                cmd.AddParam("@actionid", actionid);
                cmd.AddParam("@id", id);
                cmd.ExecuteNonQuery();
                return id;
            }
        }

        internal List<Sys_subnav> Getsys_subnavpagelist(int pageindex, int pagesize, int columnid, int actionid, out int totalcount)
        {
            var Table = "sys_subnav";
            var Column = "*";
            var OrderColumn = "id desc";
            var GroupColumn = "";
            var PageSize = pagesize;
            var CurrentPage = pageindex;
            var Group = "";
            var Condition = "";
            if (columnid > 0)
            {
                Condition = " actioncolumnid =" + columnid;
            }
            if (actionid > 0)
            {
                Condition = " actionid =" + actionid;
            }

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");



            cmd.PagingCommand1(Table, Column, OrderColumn, GroupColumn, PageSize, CurrentPage, Group, Condition);

            List<Sys_subnav> list = new List<Sys_subnav>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Sys_subnav
                    {
                        id = reader.GetValue<int>("id"),
                        subnav_name = reader.GetValue<string>("subnav_name"),
                        subnav_url = reader.GetValue<string>("subnav_url"),
                        viewcode = reader.GetValue<int>("viewcode"),
                        sortid = reader.GetValue<int>("sortid"),
                        actioncolumnid = reader.GetValue<int>("actioncolumnid"),
                        actionid = reader.GetValue<int>("actionid"),

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;

        }

        internal int Upsubnavviewcode(int subnavid, int viewcode, int actionid, string groupids)
        {
            sqlHelper.BeginTrancation();
            try
            {
                string sql = "update sys_subnav set viewcode=" + viewcode + " where id=" + subnavid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.ExecuteNonQuery();

                if (viewcode == 1)//显示
                {
                    #region 注释部分
                    //IList<int> list=new List<int>();
                    //string sql2 = "select groupid from Sys_ActionGroup where actionid="+actionid;
                    //cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                    //using(var reader=cmd.ExecuteReader())
                    //{
                    //   while(reader.Read())
                    //   {
                    //       list.Add(reader.GetValue<int>("groupid"));
                    //   }
                    //}
                    // if(list.Count>0)
                    // {
                    //    foreach(int gid in list)
                    //    {
                    //        string sql3 = "insert into sys_groupactionsubnav(groupid,actionid,subnavid) values("+gid+","+actionid+","+subnavid+")";
                    //        cmd = sqlHelper.PrepareTextSqlCommand(sql3);
                    //        cmd.ExecuteNonQuery();
                    //    }
                    // }
                    #endregion

                    //在关系表中把原来含有此导航的管理组重新录入
                    if (groupids != "")
                    {
                        string[] groupidarr = groupids.Split(',');
                        foreach (string gid in groupidarr)
                        {
                            if (gid != "")
                            {
                                string sql3 = "insert into sys_groupactionsubnav(groupid,actionid,subnavid) values('" + gid + "'," + actionid + "," + subnavid + ")";
                                cmd = sqlHelper.PrepareTextSqlCommand(sql3);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                else
                {
                    //删除关系表中原来含有此导航的管理组
                    string sql4 = "delete sys_groupactionsubnav where subnavid=" + subnavid;
                    cmd = sqlHelper.PrepareTextSqlCommand(sql4);
                    cmd.ExecuteNonQuery();
                }

                sqlHelper.Commit();
                sqlHelper.Dispose();
                return 1;
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }

        internal IList<Sys_subnav> GetSys_subnavbyactionid(int actionid, int viewcode)
        {
            string sql = @"SELECT [id]
                              ,[subnav_name]
                              ,[subnav_url]
                              ,[viewcode]
                              ,[sortid]
                              ,[actioncolumnid]
                              ,[actionid]
                          FROM  [sys_subnav] where actionid=@actionid and viewcode=@viewcode";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@actionid", actionid);
            cmd.AddParam("@viewcode", viewcode);
            using (var reader = cmd.ExecuteReader())
            {
                IList<Sys_subnav> list = new List<Sys_subnav>();
                while (reader.Read())
                {
                    list.Add(new Sys_subnav
                    {
                        id = reader.GetValue<int>("id"),
                        subnav_name = reader.GetValue<string>("subnav_name"),
                        subnav_url = reader.GetValue<string>("subnav_url"),
                        viewcode = reader.GetValue<int>("viewcode"),
                        sortid = reader.GetValue<int>("sortid"),
                        actioncolumnid = reader.GetValue<int>("actioncolumnid"),
                        actionid = reader.GetValue<int>("actionid"),
                    });
                }
                return list;
            }
        }

        internal IList<Sys_subnav> Getsys_subnavlistbyvirtualurl(string virtualurl, int viewcode, int groupid, string parastr)
        {
            try
            {
                #region 注释部分
                //                string sql = @"SELECT [id]
                //                                                  ,[subnav_name]
                //                                                  ,[subnav_url]
                //                                                  ,[viewcode]
                //                                                  ,[sortid]
                //                                                  ,[actioncolumnid]
                //                                                  ,[actionid]
                //                                              FROM  [sys_subnav] where
                //                     viewcode=@viewcode 
                //                     and  id in (select subnavid from sys_groupactionsubnav where  
                //                     actionid in (SELECT  [actionid]  FROM  [sys_subnav] where     groupid=@groupid and  LOWER(subnav_url)=@virtualurl and CHARINDEX('?', subnav_url)=0)
                //                     or   actionid in (SELECT  [actionid]  FROM  [sys_subnav] where     groupid=@groupid and  LOWER(subnav_url)=@virtualurl+@parastr and CHARINDEX('?', subnav_url)>0))
                //                     order by sortid,id";
                #endregion

                if (parastr != "" && parastr.IndexOf('?') == -1)
                {
                    parastr = "?" + parastr;
                }

                sqlHelper.BeginTrancation();
                //首先判断子导航列表 subnav_url 是否等于 虚拟路径+参数：含有正常查询；不含有则查询虚拟路径
                string sql = @"SELECT count(1)
                                              FROM  [sys_subnav] where
                     viewcode=@viewcode 
                     and  id in (select subnavid from sys_groupactionsubnav where  
                      actionid in (SELECT  [actionid]  FROM  [sys_subnav] where     LOWER(subnav_url)=@virtualurl+@parastr)
                      and groupid=@groupid ) ";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@viewcode", viewcode);
                cmd.AddParam("@virtualurl", virtualurl.ToLower());
                cmd.AddParam("@parastr", parastr.ToLower());
                cmd.AddParam("@groupid", groupid);
                object o = cmd.ExecuteScalar();
                string sql2 = "";
                if (int.Parse(o.ToString()) == 0)
                {
                    sql2 = @"SELECT [id]
                                                  ,[subnav_name]
                                                  ,[subnav_url]
                                                  ,[viewcode]
                                                  ,[sortid]
                                                  ,[actioncolumnid]
                                                  ,[actionid]
                                              FROM  [sys_subnav] where
                     viewcode=@viewcode 
                     and  id in (select subnavid from sys_groupactionsubnav where   
                     actionid in (SELECT  [actionid]  FROM  [sys_subnav] where   LOWER(subnav_url)=@virtualurl)
                     and groupid=@groupid ) 
                     order by sortid,id";
                    cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                    cmd.AddParam("@viewcode", viewcode);
                    cmd.AddParam("@virtualurl", virtualurl.ToLower());
                    cmd.AddParam("@groupid", groupid);
                }
                else
                {
                    sql2 = @"SELECT [id]
                                                  ,[subnav_name]
                                                  ,[subnav_url]
                                                  ,[viewcode]
                                                  ,[sortid]
                                                  ,[actioncolumnid]
                                                  ,[actionid]
                                              FROM  [sys_subnav] where
                     viewcode=@viewcode 
                     and  id in (select subnavid from sys_groupactionsubnav where  
                      actionid in (SELECT  [actionid]  FROM  [sys_subnav] where     groupid=@groupid and  LOWER(subnav_url)=@virtualurl+@parastr)
                     and groupid=@groupid ) 
                     order by sortid,id";
                    cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                    cmd.AddParam("@viewcode", viewcode);
                    cmd.AddParam("@virtualurl", virtualurl.ToLower());
                    cmd.AddParam("@parastr", parastr.ToLower());
                    cmd.AddParam("@groupid", groupid);
                }

                IList<Sys_subnav> list = new List<Sys_subnav>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Sys_subnav
                        {
                            id = reader.GetValue<int>("id"),
                            subnav_name = reader.GetValue<string>("subnav_name"),
                            subnav_url = reader.GetValue<string>("subnav_url").ToLower(),
                            viewcode = reader.GetValue<int>("viewcode"),
                            sortid = reader.GetValue<int>("sortid"),
                            actioncolumnid = reader.GetValue<int>("actioncolumnid"),
                            actionid = reader.GetValue<int>("actionid"),
                        });
                    }

                }

                sqlHelper.Commit();
                sqlHelper.Dispose();
                return list;
            }
            catch
            {
                sqlHelper.Dispose();
                return new List<Sys_subnav>();
            }
        }

        internal int GetsubnavNum(string subnavname, string subnavurl)
        {
            string sql = @"SELECT count(1)
                          FROM  [sys_subnav] where subnav_name=@subnav_name or subnav_url=@subnav_url";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@subnav_name", subnavname);
            cmd.AddParam("@subnav_url", subnavurl);
            object o = cmd.ExecuteScalar();
            return int.Parse(o.ToString());
        }

        internal IList<Sys_subnav> Getallsys_subnavpagelist(int pageindex, int pagesize, out int totalcount)
        {
            var Table = "sys_subnav";
            var Column = "*";
            var OrderColumn = "id desc";
            var GroupColumn = "";
            var PageSize = pagesize;
            var CurrentPage = pageindex;
            var Group = "";
            var Condition = "";


            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");



            cmd.PagingCommand1(Table, Column, OrderColumn, GroupColumn, PageSize, CurrentPage, Group, Condition);

            List<Sys_subnav> list = new List<Sys_subnav>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Sys_subnav
                    {
                        id = reader.GetValue<int>("id"),
                        subnav_name = reader.GetValue<string>("subnav_name"),
                        subnav_url = reader.GetValue<string>("subnav_url"),
                        viewcode = reader.GetValue<int>("viewcode"),
                        sortid = reader.GetValue<int>("sortid"),
                        actioncolumnid = reader.GetValue<int>("actioncolumnid"),
                        actionid = reader.GetValue<int>("actionid"),

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }

        internal int Upsubnavdatabase(int subnavid, int oldviewcode, int oldcolumnid, int oldactionid, string oldgroupids, int newviewcode, int newcolumnid, int newactionid)
        {
            sqlHelper.BeginTrancation();
            try
            {
                //判断是选中 还是 取消选中：选中则进行处理操作；取消则还原处理操作
                if (newviewcode == 1)
                {
                    ////把原来对子导航的关联 和 操作 去除
                    //string sql1 = "update  [sys_subnav] set viewcode=0, actioncolumnid=0,actionid=0 where id=" + subnavid;
                    //var cmd = sqlHelper.PrepareTextSqlCommand(sql1);
                    //cmd.ExecuteNonQuery();

                    string sql2 = "delete sys_groupactionsubnav where subnavid=" + subnavid;
                    var cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                    cmd.ExecuteNonQuery();

                    //对子导航重新操作
                    string sql3 = "update  [sys_subnav] set viewcode=1, actioncolumnid=" + newcolumnid + ",actionid=" + newactionid + " where id=" + subnavid;
                    cmd = sqlHelper.PrepareTextSqlCommand(sql3);
                    cmd.ExecuteNonQuery();

                }
                else
                {
                    string sql1 = "update  [sys_subnav] set viewcode=" + oldviewcode + ", actioncolumnid=" + oldcolumnid + ",actionid=" + oldactionid + " where id=" + subnavid;
                    var cmd = sqlHelper.PrepareTextSqlCommand(sql1);
                    cmd.ExecuteNonQuery();

                    //在关系表中把原来含有此导航的管理组重新录入
                    if (oldgroupids != "")
                    {
                        string[] groupidarr = oldgroupids.Split(',');
                        foreach (string gid in groupidarr)
                        {
                            if (gid != "")
                            {
                                string sql3 = "insert into sys_groupactionsubnav(groupid,actionid,subnavid) values('" + gid + "'," + oldactionid + "," + subnavid + ")";
                                cmd = sqlHelper.PrepareTextSqlCommand(sql3);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                #region 注释部分
                //#region 修改导航的所属权限(actionid 和 actioncolumnid)
                ////导航 原来在权限下面：取消的话，设置actionid 和 actioncolumnid 等于0；选中的话更改所在权限
                ////导航 原来不在权限下面:取消的话，还原为以前的权限；选中的话 更改所在权限；
                //int upactioncolumnid = 0;
                //int upactionid = 0;
                //if (underaction == 1)
                //{
                //    if (checkd == 1)
                //    {
                //        upactioncolumnid = seledcolumnid;
                //        upactionid = seledactionid;
                //    }
                //    else
                //    {
                //        upactioncolumnid = 0;
                //        upactionid = 0;
                //    }
                //}
                //else
                //{
                //    if (checkd == 1)
                //    {
                //        upactioncolumnid = seledcolumnid;
                //        upactionid = seledactionid;

                //    }
                //    else
                //    {

                //        upactioncolumnid = columnid;
                //        upactionid = actionid;

                //    }
                //}
                //string sql = "update sys_subnav set  actionid=" + actionid + ",actioncolumnid=" + columnid + " where id=" + subnavid;
                //var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                //cmd.ExecuteNonQuery();

                //#endregion

                //#region  修改导航的显示:  a.在权限下是否显示  b.在角色下是否显示

                ////导航 原来在权限下面
                //int upviewcode = 0; //权限下显示方式
                //if (underaction == 1)
                //{
                //    if (checkd == 1)
                //    {
                //        upviewcode = viewcode;

                //    }
                //    else
                //    {
                //        upviewcode = 0;

                //    }
                //}
                //else
                //{
                //    if (checkd == 1)
                //    {
                //        upviewcode = 0;
                //    }
                //    else
                //    {
                //        upviewcode = viewcode;
                //    }
                //}
                //#endregion


                ////导航 原显示0,选中->更改所在权限；取消选中->更改所在权限；(不用另外考虑，按正常流程走就可以)
                ////导航 原显示1,a.选中->更改所在权限，显示变为0，sys_groupactionsubnav 删除记录；
                //               //b.取消选中->更改所在权限，显示变为1，sys_groupactionsubnav 添加记录   
                //if (viewcode == 1)//显示
                //{
                //    if (checkd == 1)
                //    {
                //        string sql = "update sys_subnav set viewcode=0,actionid=" + actionid + ",actioncolumnid=" + columnid + " where id=" + subnavid;
                //        var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                //        cmd.ExecuteNonQuery();

                //        string sql2 = "delete sys_groupactionsubnav where subnavid="+subnavid;
                //        cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                //        cmd.ExecuteNonQuery();
                //    }
                //    else 
                //    {
                //        string sql0 = "update sys_subnav set viewcode=1,actionid=" + actionid + ",actioncolumnid=" + columnid + " where id=" + subnavid;
                //        var cmd = sqlHelper.PrepareTextSqlCommand(sql0);
                //        cmd.ExecuteNonQuery();

                //        string sql = "update sys_subnav set viewcode=1,actionid=" + actionid + ",actioncolumnid=" + columnid + " where id=" + subnavid;
                //        cmd = sqlHelper.PrepareTextSqlCommand(sql);
                //        cmd.ExecuteNonQuery();

                //        IList<int> list = new List<int>();
                //        string sql2 = "select groupid from Sys_ActionGroup where actionid=" + actionid;
                //        cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                //        using (var reader = cmd.ExecuteReader())
                //        {
                //            while (reader.Read())
                //            {
                //                list.Add(reader.GetValue<int>("groupid"));
                //            }
                //        }
                //        if (list.Count > 0)
                //        {
                //            foreach (int gid in list)
                //            {
                //                string sql3 = "insert into sys_groupactionsubnav(groupid,actionid,subnavid) values(" + gid + "," + actionid + "," + subnavid + ")";
                //                cmd = sqlHelper.PrepareTextSqlCommand(sql3);
                //                cmd.ExecuteNonQuery();
                //            }
                //        }
                //    } 
                //}
                #endregion


                sqlHelper.Commit();
                return 1;
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }

        }



        internal Sys_subnav Getsys_subnav(string vurl, string parastr)
        {
            if (parastr != "" && parastr.IndexOf('?') == -1)
            {
                parastr = "?" + parastr;
            }
            string sql = "select * from sys_subnav where subnav_url='" + vurl.ToLower() + parastr.ToLower() + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                Sys_subnav r = null;
                if (reader.Read())
                {
                    r = new Sys_subnav
                    {
                        id = reader.GetValue<int>("id"),
                        subnav_name = reader.GetValue<string>("subnav_name"),
                        subnav_url = reader.GetValue<string>("subnav_url"),
                        viewcode = reader.GetValue<int>("viewcode"),
                        sortid = reader.GetValue<int>("sortid"),
                        actioncolumnid = reader.GetValue<int>("actioncolumnid"),
                        actionid = reader.GetValue<int>("actionid"),
                    };
                }
                return r;
            }
        }
    }
}
