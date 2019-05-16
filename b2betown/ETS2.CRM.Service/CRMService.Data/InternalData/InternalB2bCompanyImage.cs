using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;
using System.Data;

namespace ETS2.CRM.Service.CRMService.Data.InternalData
{
    public class InternalB2bCompanyImage
    {
        private SqlHelper sqlHelper;
        public InternalB2bCompanyImage(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        #region 添加图片

        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateB2bImage";

        public int InsertOrUpdate(B2b_company_image model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);
            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Comid", model.Com_id);
            cmd.AddParam("@Title", model.Title);
            cmd.AddParam("@Typeid", model.Typeid);
            cmd.AddParam("@Linkurl", model.Linkurl);
            cmd.AddParam("@Imgurl", model.Imgurl);
            cmd.AddParam("@Channelcompanyid", model.Channelcompanyid);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion



        #region  删除图片
        internal int Deleteimage(int comid, int id, int channelcompanyid=0)
        {
            const string sqlTxt = @"delete [dbo].[b2b_company_image] where com_id=@comid and id=@id and channelcompanyid=@channelcompanyid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@id", id);
            cmd.AddParam("@channelcompanyid", channelcompanyid);
            return cmd.ExecuteNonQuery();
        }
        #endregion

        #region  修改图片状态图片
        internal int UpDownState(int comid, int id,int state)
        {
            const string sqlTxt = @"update [dbo].[b2b_company_image] set state=@state where com_id=@comid and id=@id";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@id", id);
            cmd.AddParam("@state", state);
            
            return cmd.ExecuteNonQuery();
        }
        #endregion

        #region  修改图片状态图片
        internal int UpAllDownState(int comid)
        {
            const string sqlTxt = @"update [dbo].[b2b_company_image] set state=0 where com_id=@comid and typeid=0 and channelcompanyid=0";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@comid", comid);

            return cmd.ExecuteNonQuery();
        }
        #endregion


        #region  得到图片
        internal B2b_company_image GetimageByComid(int comid, int id, int channelcompanyid=0)
        {
            const string sqlTxt = @"SELECT *
  FROM [dbo].[b2b_company_image] where com_id=@comid and id=@id and channelcompanyid=@channelcompanyid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@id", id);
            cmd.AddParam("@channelcompanyid", channelcompanyid);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_company_image u = null;

                while (reader.Read())
                {
                    u = new B2b_company_image
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Imgurl = reader.GetValue<int>("Imgurl"),
                        Imgurl_address = reader.GetValue<string>("Imgurl_address"),
                        Linkurl = reader.GetValue<string>("Linkurl"),
                        Title = reader.GetValue<string>("title"),
                        Typeid = reader.GetValue<int>("Typeid"),
                        State = reader.GetValue<int>("State")
                    };

                }
                return u;
            }
        }
        #endregion


        #region  客户得到图片列表 typeid 0 为站点=banner图片 1=门市banner ，
        internal List<B2b_company_image> PageGetimageList(int comid, int typeid, out int totalcount)
        {
            totalcount = 0;
            const string sqlTxt = @"SELECT *
  FROM [dbo].[b2b_company_image] where com_id=@comid and typeid=@typeid and state=1 and channelcompanyid=0";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@typeid", typeid);

            List<B2b_company_image> list = new List<B2b_company_image>();
            using (var reader = cmd.ExecuteReader())
            {
                 
                while (reader.Read())
                {
                    list.Add(new B2b_company_image
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Imgurl = reader.GetValue<int>("Imgurl"),
                        Imgurl_address = reader.GetValue<string>("Imgurl_address"),
                        Linkurl = reader.GetValue<string>("Linkurl"),
                        Title = reader.GetValue<string>("title"),
                        Typeid = reader.GetValue<int>("Typeid"),
                        State = reader.GetValue<int>("State")
                    });
                    totalcount = totalcount + 1;
                }
            }
            return list;
        }
        #endregion


        #region  客户得到（渠道）图片列表 
        internal List<B2b_company_image> PageChannelGetimageList(int comid, int channelcompanyid, out int totalcount)
        {
            totalcount = 0;
            const string sqlTxt = @"SELECT *
  FROM [dbo].[b2b_company_image] where com_id=@comid and channelcompanyid=@channelcompanyid and typeid=1 and state=1";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@channelcompanyid", channelcompanyid);

            List<B2b_company_image> list = new List<B2b_company_image>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_company_image
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Imgurl = reader.GetValue<int>("Imgurl"),
                        Imgurl_address = reader.GetValue<string>("Imgurl_address"),
                        Linkurl = reader.GetValue<string>("Linkurl"),
                        Title = reader.GetValue<string>("title"),
                        Typeid = reader.GetValue<int>("Typeid"),
                        State = reader.GetValue<int>("State")
                    });
                    totalcount = totalcount + 1;
                }
            }
            return list;
        }
        #endregion


        #region  得到图片列表 typeid 0 为站点=banner图片 1=门市banner，2=导航图片
        internal List<B2b_company_image> GetimageList(int comid, int typeid, int pageindex, int pagesize, out int totalcount, int channelcompanyid = 0)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = "com_id=" + comid + " and  typeid=" + typeid +" and channelcompanyid="+channelcompanyid;

            cmd.PagingCommand1("b2b_company_image", "*", "id desc", "", pagesize, pageindex, "", condition);

            List<B2b_company_image> list = new List<B2b_company_image>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_company_image
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Imgurl = reader.GetValue<int>("Imgurl"),
                        Imgurl_address = reader.GetValue<string>("Imgurl_address"),
                        Linkurl = reader.GetValue<string>("Linkurl"),
                        Title = reader.GetValue<string>("title"),
                        Typeid = reader.GetValue<int>("Typeid"),
                        State = reader.GetValue<int>("State")
                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }
        #endregion


        #region  从图库中得到图片列表 usetype 根据类型得到相应的图片列表
        internal List<b2b_image_library> GetimageLibraryList(int usetype, int pageindex, int pagesize, out int totalcount,int modelid=0)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = "usetype=" + usetype;

            if (modelid != 0) {
                condition += " and  modelid=" + modelid;
            }

            cmd.PagingCommand1("b2b_image_library", "*", "id", "", pagesize, pageindex, "", condition);

            List<b2b_image_library> list = new List<b2b_image_library>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new b2b_image_library
                    {
                        Id = reader.GetValue<int>("Id"),
                        Imgurl = reader.GetValue<int>("Imgurl"),
                        Usetype = reader.GetValue<int>("usetype"),
                        Width = reader.GetValue<int>("Width"),
                        Height = reader.GetValue<int>("Height"),
                        Modelid = reader.GetValue<int>("Modelid"),
                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }
        #endregion

        #region  图标库列表
        internal List<b2b_image_library> GetfontLibraryList(int usetype, int pageindex, int pagesize, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = "";
            cmd.PagingCommand1("fonticon", "*", "id", "", pagesize, pageindex, "", condition);

            List<b2b_image_library> list = new List<b2b_image_library>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new b2b_image_library
                    {
                        Id = reader.GetValue<int>("Id"),
                        Fonticon = reader.GetValue<string>("fontname"),
                       
                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }
        #endregion

        #region  从图库中得到图片列表 usetype,modelid 根据类型得到相应的图片列表
        internal List<b2b_image_library> GettypemodelidimageLibraryList(int usetype,int modelid, int pageindex, int pagesize, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = "usetype=" + usetype + " and modelid =" + modelid;

            cmd.PagingCommand1("b2b_image_library", "*", "id", "", pagesize, pageindex, "", condition);

            List<b2b_image_library> list = new List<b2b_image_library>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new b2b_image_library
                    {
                        Id = reader.GetValue<int>("Id"),
                        Imgurl = reader.GetValue<int>("Imgurl"),
                        Usetype = reader.GetValue<int>("usetype"),
                        Width = reader.GetValue<int>("Width"),
                        Height = reader.GetValue<int>("Height"),
                        Modelid = reader.GetValue<int>("Modelid"),
                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }
        #endregion


        #region  从图库得到图片
        internal b2b_image_library GetimageLibraryByid(int id)
        {
            const string sqlTxt = @"SELECT *
  FROM [dbo].[b2b_image_library] where id=@id";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@id", id);

            using (var reader = cmd.ExecuteReader())
            {
                b2b_image_library u = null;

                while (reader.Read())
                {
                    u = new b2b_image_library
                    {
                        Id = reader.GetValue<int>("Id"),
                        Imgurl = reader.GetValue<int>("Imgurl"),
                        Usetype = reader.GetValue<int>("usetype"),
                        Width = reader.GetValue<int>("Width"),
                        Height = reader.GetValue<int>("Height"),
                        Modelid = reader.GetValue<int>("Modelid"),
                    };

                }
                return u;
            }
        }
        #endregion

        #region  删除图库图片
        internal int DeleteLibraryimage(int id)
        {
            const string sqlTxt = @"delete [dbo].[b2b_image_library] where id=@id ";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@id", id);
            return cmd.ExecuteNonQuery();
        }
        #endregion


        #region 添加图库图片

        private static readonly string SQLLibraryInsertOrUpdate = "usp_InsertOrUpdateB2bLibraryImage";

        public int LibraryInsertOrUpdate(b2b_image_library model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLLibraryInsertOrUpdate);
            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Usetype", model.Usetype);
            cmd.AddParam("@Width", model.Width);
            cmd.AddParam("@Imgurl", model.Imgurl);
            cmd.AddParam("@Height", model.Height);
            cmd.AddParam("@Modelid", model.Modelid);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion

    }
}
