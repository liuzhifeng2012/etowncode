using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Model;

namespace ETS2.WeiXin.Service.WeiXinService.Data.InternalData
{
    public class InternalWxmedia_updownlog
    {
        public SqlHelper sqlHelper;
        public InternalWxmedia_updownlog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal bool Ismarkedandnotdeal(string weixin, int clientuptypemark)
        {
            string sql = "select count(1) from wxmedia_updownlog where isfinish=0 and  operweixin='" + weixin + "' and clientuptypemark=" + clientuptypemark;
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

        internal int Edituploadlog(Wxmedia_updownlog m)
        {
            try
            {
                if (m.remarks == null)
                {
                    m.remarks = "";
                }
                if (m.materialid == null)
                {
                    m.materialid = 0;
                }

                if (m.id > 0)//编辑操作
                {
                    string sql = @"UPDATE [wxmedia_updownlog]
   SET [mediaid] = @mediaid
      ,[mediatype] = @mediatype
      ,[savepath] = @savepath
      ,[created_at] = @created_at
      ,[createtime] = @createtime
      ,[opertype] = @opertype
      ,[operweixin] = @operweixin
      ,[clientuptypemark] = @clientuptypemark
      ,[comid] = @comid
      ,relativepath=@relativepath
      ,txtcontent=@txtcontent
      ,isfinish=@isfinish
      ,remarks=@remarks
     ,materialid=@materialid
 WHERE id=@id";

                    var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                    cmd.AddParam("@id", m.id);
                    cmd.AddParam("@mediaid", m.mediaid);
                    cmd.AddParam("@mediatype", m.mediatype);
                    cmd.AddParam("@savepath", m.savepath);
                    cmd.AddParam("@created_at", m.created_at);
                    cmd.AddParam("@createtime", m.createtime);
                    cmd.AddParam("@opertype", m.opertype);
                    cmd.AddParam("@operweixin", m.operweixin);
                    cmd.AddParam("@clientuptypemark", m.clientuptypemark);
                    cmd.AddParam("@comid", m.comid);
                    cmd.AddParam("@relativepath", m.relativepath);
                    cmd.AddParam("@txtcontent", m.txtcontent);
                    cmd.AddParam("@isfinish", m.isfinish);
                    cmd.AddParam("@remarks", m.remarks);
                    cmd.AddParam("@materialid", m.materialid);

                    cmd.ExecuteNonQuery();
                    return m.id;
                }
                else //添加操作
                {
                    string sql = @"INSERT INTO [wxmedia_updownlog]
           ([mediaid]
           ,[mediatype]
           ,[savepath]
           ,[created_at]
           ,[createtime]
           ,[opertype]
           ,[operweixin]
           ,[clientuptypemark]
           ,[comid]
           ,relativepath
          ,txtcontent
          ,isfinish
          ,remarks
          ,materialid 
)
     VALUES
           (@mediaid
           ,@mediatype
           ,@savepath
           ,@created_at
           ,@createtime
           ,@opertype
           ,@operweixin
           ,@clientuptypemark
           ,@comid
           ,@relativepath
           ,@txtcontent
           ,@isfinish
           ,@remarks
           ,@materialid);select @@identity;";

                    var cmd = sqlHelper.PrepareTextSqlCommand(sql);

                    cmd.AddParam("@mediaid", m.mediaid);
                    cmd.AddParam("@mediatype", m.mediatype);
                    cmd.AddParam("@savepath", m.savepath);
                    cmd.AddParam("@created_at", m.created_at);
                    cmd.AddParam("@createtime", m.createtime);
                    cmd.AddParam("@opertype", m.opertype);
                    cmd.AddParam("@operweixin", m.operweixin);
                    cmd.AddParam("@clientuptypemark", m.clientuptypemark);
                    cmd.AddParam("@comid", m.comid);
                    cmd.AddParam("@relativepath", m.relativepath);
                    cmd.AddParam("@txtcontent", m.txtcontent);
                    cmd.AddParam("@isfinish", m.isfinish);
                    cmd.AddParam("@remarks", m.remarks);
                    cmd.AddParam("@materialid", m.materialid);

                    object o = cmd.ExecuteScalar();
                    return o == null ? 0 : int.Parse(o.ToString());
                }
            }
            catch
            {
                return 0;
            }
        }

        internal Wxmedia_updownlog GetMarkedAndNotdeallog(string weixin, int clientuptypemark)
        {
            string sql = "select top 1 * from wxmedia_updownlog where isfinish=0 and operweixin='" + weixin + "' and clientuptypemark=" + clientuptypemark + " order by id desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                Wxmedia_updownlog m = null;
                if (reader.Read())
                {
                    m = new Wxmedia_updownlog
                    {
                        id = reader.GetValue<int>("id"),
                        mediaid = reader.GetValue<string>("mediaid"),
                        mediatype = reader.GetValue<string>("mediatype"),
                        savepath = reader.GetValue<string>("savepath"),
                        created_at = reader.GetValue<string>("created_at"),
                        createtime = reader.GetValue<DateTime>("createtime"),
                        opertype = reader.GetValue<string>("opertype"),
                        operweixin = reader.GetValue<string>("operweixin"),
                        clientuptypemark = reader.GetValue<int>("clientuptypemark"),
                        comid = reader.GetValue<int>("comid"),
                        relativepath = reader.GetValue<string>("relativepath"),
                        txtcontent = reader.GetValue<string>("txtcontent"),
                        isfinish = reader.GetValue<int>("isfinish"),
                        materialid = reader.GetValue<int>("materialid"),
                        remarks = reader.GetValue<string>("remarks")
                    };
                }
                return m;
            }
        }

        internal Wxmedia_updownlog GetWxmedia_updownlog(string weixin, int clientuptypemark, int comid)
        {
            try
            {
                string sql = @"select top 1 * from wxmedia_updownlog 
                                where comid=@comid and  
                                operweixin=
                                (select weixin from b2b_crm where com_id=@comid 
                                 and phone=(select mobile from member_channel where com_id=@comid  and id=(select issuecard from member_card where cardcode=(select idcard from b2b_crm where weixin=@weixin)))) 
                                 and mediaid!='' and clientuptypemark=@clientuptypemark and opertype='down' 
                                   order by id desc";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@comid", comid);
                cmd.AddParam("@weixin", weixin);
                cmd.AddParam("@clientuptypemark", clientuptypemark);
                using (var reader = cmd.ExecuteReader())
                {
                    Wxmedia_updownlog m = null;
                    if (reader.Read())
                    {
                        m = new Wxmedia_updownlog
                        {
                            id = reader.GetValue<int>("id"),
                            mediaid = reader.GetValue<string>("mediaid"),
                            mediatype = reader.GetValue<string>("mediatype"),
                            savepath = reader.GetValue<string>("savepath"),
                            created_at = reader.GetValue<string>("created_at"),
                            createtime = reader.GetValue<DateTime>("createtime"),
                            opertype = reader.GetValue<string>("opertype"),
                            operweixin = reader.GetValue<string>("operweixin"),
                            clientuptypemark = reader.GetValue<int>("clientuptypemark"),
                            comid = reader.GetValue<int>("comid"),
                            relativepath = reader.GetValue<string>("relativepath"),
                            txtcontent = reader.GetValue<string>("txtcontent"),
                            isfinish = reader.GetValue<int>("isfinish"),
                            materialid = reader.GetValue<int>("materialid"),
                            remarks = reader.GetValue<string>("remarks")
                        };
                    }
                    return m;
                }
            }
            catch
            {
                return null;
            }
        }

        internal int DelGuwenNotSucMediaMark(string weixin)
        {
            string sql = "delete  wxmedia_updownlog where operweixin='" + weixin + "' and isfinish=0";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal List<Wxmedia_updownlog> Getwxmedia_updownlogByopenid(int pageindex, int pagesize, out int totalcount, string openid, string viewmethod = "")
        {
            try
            {
                var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
                string condition = "operweixin='" + openid + "' and isfinish=1 and opertype='down'";
                if (viewmethod != "")
                {
                    condition += " and viewmethod in (" + viewmethod + ")";
                }
                cmd.PagingCommand1("wxmedia_updownlog", "*", "clientuptypemark", "", pagesize, pageindex, "", condition);

                List<Wxmedia_updownlog> m = new List<Wxmedia_updownlog>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        m.Add(new Wxmedia_updownlog
                        {
                            id = reader.GetValue<int>("id"),
                            mediaid = reader.GetValue<string>("mediaid"),
                            mediatype = reader.GetValue<string>("mediatype"),
                            savepath = reader.GetValue<string>("savepath"),
                            created_at = reader.GetValue<string>("created_at"),
                            createtime = reader.GetValue<DateTime>("createtime"),
                            opertype = reader.GetValue<string>("opertype"),
                            operweixin = reader.GetValue<string>("operweixin"),
                            clientuptypemark = reader.GetValue<int>("clientuptypemark"),
                            comid = reader.GetValue<int>("comid"),
                            relativepath = reader.GetValue<string>("relativepath"),
                            txtcontent = reader.GetValue<string>("txtcontent"),
                            isfinish = reader.GetValue<int>("isfinish"),
                            materialid = reader.GetValue<int>("materialid"),
                            remarks = reader.GetValue<string>("remarks")
                        });

                    }
                }
                totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
                return m;
            }
            catch
            {

                totalcount = 0;
                return null;
            }
        }

        internal List<Wxmedia_updownlog> Getwxdownvoicelist(string openid, int clientuptypemark, int materialid)
        {
            string sql = @"SELECT   [id]
                              ,[mediaid]
                              ,[mediatype]
                              ,[savepath]
                              ,[created_at]
                              ,[createtime]
                              ,[opertype]
                              ,[operweixin]
                              ,[clientuptypemark]
                              ,[comid]
                              ,[relativepath]
                              ,[txtcontent]
                              ,[isfinish]
                              ,[remarks]
                              ,materialid
                          FROM  [wxmedia_updownlog] where operweixin=@openid and clientuptypemark=@clientuptypemark and mediatype='voice' and opertype='down' and isfinish=1 and materialid=@materialid order by id desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@openid", openid);
            cmd.AddParam("@clientuptypemark", clientuptypemark);
            cmd.AddParam("@materialid", materialid);
            using (var reader = cmd.ExecuteReader())
            {
                List<Wxmedia_updownlog> list = new List<Wxmedia_updownlog>();
                while (reader.Read())
                {
                    list.Add(new Wxmedia_updownlog
                    {
                        id = reader.GetValue<int>("id"),
                        mediaid = reader.GetValue<string>("mediaid"),
                        mediatype = reader.GetValue<string>("mediatype"),
                        savepath = reader.GetValue<string>("savepath"),
                        created_at = reader.GetValue<string>("created_at"),
                        createtime = reader.GetValue<DateTime>("createtime"),
                        opertype = reader.GetValue<string>("opertype"),
                        operweixin = reader.GetValue<string>("operweixin"),
                        clientuptypemark = reader.GetValue<int>("clientuptypemark"),
                        comid = reader.GetValue<int>("comid"),
                        relativepath = reader.GetValue<string>("relativepath"),
                        txtcontent = reader.GetValue<string>("txtcontent"),
                        isfinish = reader.GetValue<int>("isfinish"),
                        materialid = reader.GetValue<int>("materialid"),
                        remarks = reader.GetValue<string>("remarks")
                    });
                }
                return list;
            }
        }

        internal Wxmedia_updownlog GetWxmedia_updownlogbyid(int uplogid)
        {
            string sql = @"SELECT   [id]
                              ,[mediaid]
                              ,[mediatype]
                              ,[savepath]
                              ,[created_at]
                              ,[createtime]
                              ,[opertype]
                              ,[operweixin]
                              ,[clientuptypemark]
                              ,[comid]
                              ,[relativepath]
                              ,[txtcontent]
                              ,[isfinish]
                              ,[remarks]
                              ,materialid 
                          FROM  [wxmedia_updownlog] where  id=@id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", uplogid);

            using (var reader = cmd.ExecuteReader())
            {
                Wxmedia_updownlog m = null;
                if (reader.Read())
                {
                    m = new Wxmedia_updownlog
                    {
                        id = reader.GetValue<int>("id"),
                        mediaid = reader.GetValue<string>("mediaid"),
                        mediatype = reader.GetValue<string>("mediatype"),
                        savepath = reader.GetValue<string>("savepath"),
                        created_at = reader.GetValue<string>("created_at"),
                        createtime = reader.GetValue<DateTime>("createtime"),
                        opertype = reader.GetValue<string>("opertype"),
                        operweixin = reader.GetValue<string>("operweixin"),
                        clientuptypemark = reader.GetValue<int>("clientuptypemark"),
                        comid = reader.GetValue<int>("comid"),
                        relativepath = reader.GetValue<string>("relativepath"),
                        txtcontent = reader.GetValue<string>("txtcontent"),
                        isfinish = reader.GetValue<int>("isfinish"),
                        materialid = reader.GetValue<int>("materialid"),
                        remarks = reader.GetValue<string>("remarks")
                    };
                }
                return m;
            }
        }

        internal Wxmedia_updownlog GetNewestuplogbymedia(string savepath)
        {
            string sql = @"SELECT  top 1   [id]
                              ,[mediaid]
                              ,[mediatype]
                              ,[savepath]
                              ,[created_at]
                              ,[createtime]
                              ,[opertype]
                              ,[operweixin]
                              ,[clientuptypemark]
                              ,[comid]
                              ,[relativepath]
                              ,[txtcontent]
                              ,[isfinish]
                              ,[remarks]
                              ,materialid 
                          FROM  [wxmedia_updownlog] where  savepath=@savepath and opertype='up' order by id desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@savepath", savepath);

            using (var reader = cmd.ExecuteReader())
            {
                Wxmedia_updownlog m = null;
                if (reader.Read())
                {
                    m = new Wxmedia_updownlog
                    {
                        id = reader.GetValue<int>("id"),
                        mediaid = reader.GetValue<string>("mediaid"),
                        mediatype = reader.GetValue<string>("mediatype"),
                        savepath = reader.GetValue<string>("savepath"),
                        created_at = reader.GetValue<string>("created_at"),
                        createtime = reader.GetValue<DateTime>("createtime"),
                        opertype = reader.GetValue<string>("opertype"),
                        operweixin = reader.GetValue<string>("operweixin"),
                        clientuptypemark = reader.GetValue<int>("clientuptypemark"),
                        comid = reader.GetValue<int>("comid"),
                        relativepath = reader.GetValue<string>("relativepath"),
                        txtcontent = reader.GetValue<string>("txtcontent"),
                        isfinish = reader.GetValue<int>("isfinish"),
                        materialid = reader.GetValue<int>("materialid"),
                        remarks = reader.GetValue<string>("remarks")
                    };
                }
                return m;
            }
        }

        internal Wxmedia_updownlog GetWxmedia_updownlogbymaterialid(int materialid)
        {
            string sql = @"SELECT  top 1   [id]
                              ,[mediaid]
                              ,[mediatype]
                              ,[savepath]
                              ,[created_at]
                              ,[createtime]
                              ,[opertype]
                              ,[operweixin]
                              ,[clientuptypemark]
                              ,[comid]
                              ,[relativepath]
                              ,[txtcontent]
                              ,[isfinish]
                              ,[remarks]
                              ,materialid
                          FROM  [wxmedia_updownlog] where  materialid=@materialid and opertype='down' order by id desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@materialid", materialid);

            using (var reader = cmd.ExecuteReader())
            {
                Wxmedia_updownlog m = null;
                if (reader.Read())
                {
                    m = new Wxmedia_updownlog
                    {
                        id = reader.GetValue<int>("id"),
                        mediaid = reader.GetValue<string>("mediaid"),
                        mediatype = reader.GetValue<string>("mediatype"),
                        savepath = reader.GetValue<string>("savepath"),
                        created_at = reader.GetValue<string>("created_at"),
                        createtime = reader.GetValue<DateTime>("createtime"),
                        opertype = reader.GetValue<string>("opertype"),
                        operweixin = reader.GetValue<string>("operweixin"),
                        clientuptypemark = reader.GetValue<int>("clientuptypemark"),
                        comid = reader.GetValue<int>("comid"),
                        relativepath = reader.GetValue<string>("relativepath"),
                        txtcontent = reader.GetValue<string>("txtcontent"),
                        isfinish = reader.GetValue<int>("isfinish"),
                        materialid = reader.GetValue<int>("materialid"),
                        remarks = reader.GetValue<string>("remarks")
                    };
                }
                return m;
            }
        }

        internal int DelBeforeCoverUplog(int materialid)
        {
            string sql = " delete wxmedia_updownlog  where opertype='up' and materialid>0 and savepath in (select savepath from wxmedia_updownlog where opertype='down' and materialid=" + materialid + " and materialid>0 and mediatype='voice') ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }
    }
}
