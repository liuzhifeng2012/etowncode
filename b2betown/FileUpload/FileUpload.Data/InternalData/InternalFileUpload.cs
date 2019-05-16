using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using FileUpload.FileUpload.Entities;
using System.Data.SqlClient;

namespace FileUpload.FileUpload.Data.InternalData
{
    public class InternalFileUpload
    {
        private SqlHelper sqlHelper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlHelper"></param>
        public InternalFileUpload(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        #region 添加上传文件信息

        private static string SQL_UploadFile = @"INSERT INTO [EtownDB].[dbo].[fileupload]
           ([OrigenalName]
           ,[relativepath]
           ,[savepath]
           ,[extentionname]
           ,[type]
           ,[objid]
           ,[objtype]
           ,[creator]
           ,[creationip])
     VALUES
           (@OrigenalName
           ,@relativepath
           ,@savepath
           ,@extentionname
           ,@type
           ,@objid
           ,@objtype
           ,@creator
           ,@creationip);SELECT @@IDENTITY";
        /// <summary>
        /// 添加上传文件信息
        /// </summary>
        /// <param name="file">上传文件实体类</param>
        /// <returns>自增Id</returns>
        public int CreateUploadInfo(FileUploadModel file)
        {
            var cmd = sqlHelper.PrepareTextSqlCommand(SQL_UploadFile);
            cmd.AddParam("@OrigenalName", file.OrigenalName);
            cmd.AddParam("@relativepath", file.Relativepath == null ? "" : file.Relativepath);
            cmd.AddParam("@savepath", file.Savepath);
            cmd.AddParam("@extentionname", file.Extentionname);
            cmd.AddParam("@type", file.Type);
            cmd.AddParam("@objid", file.Objid);
            cmd.AddParam("@objtype", file.Objtype);
            cmd.AddParam("@creator", file.Creator);
            cmd.AddParam("@creationip", file.Creationip == null ? "" : file.Creationip);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        #endregion


        //        #region 按对象批量删除上传文件信息
        //        private static string SQL_Delete = @"DELETE dbo.FileUpload WHERE [ObjId] = @objId AND [ObjType] = @type";

        //        /// <summary>
        //        /// 按对象批量删除上传文件信息
        //        /// </summary>
        //        /// <param name="objId">对象Id</param>
        //        /// <param name="objType">对象类型</param>
        //        /// <returns>影响行数</returns>
        //        public int Delete(int objId, int objType)
        //        {
        //            var cmd = this.sqlHelper.PrepareTextSqlCommand(SQL_Delete);
        //            cmd.AddParam("@ObjId", objId);
        //            cmd.AddParam("@type", objType);
        //            return cmd.ExecuteNonQuery();
        //        }
        //        #endregion


        //        #region 根据对象及类型获取上传的文件信息
        //        private const string SqlGetAllFileUpload = @"SELECT [Id],[Type],[ObjId],[FilePath],[ExtName],[FileName],[UploadTime],[CreationIp],[Creator] FROM [dbo].[FileUpload] WHERE [objId] = @objId AND [ObjType] = @objType";
        //        /// <summary>
        //        /// 根据对象及类型获取上传的文件信息
        //        /// </summary>
        //        /// <returns>文件信息集合</returns>
        //        internal IList<FileUploadService.Entities.FileUpload> GetFileUpload(int objId, int objType)
        //        {
        //            var cmd = sqlHelper.PrepareTextSqlCommand(SqlGetAllFileUpload);
        //            cmd.AddParam("@ObjId", objId);
        //            cmd.AddParam("@objType", objType);

        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                return this.FromReader(reader);
        //            }
        //        }
        //        #endregion


        private IList<FileUploadModel> FromReader(SqlDataReader reader)
        {
            if (!reader.HasRows)
            {
                return null;
            }
            IList<FileUploadModel> files = new List<FileUploadModel>();
            while (reader.Read())
            {
                FileUploadModel file = new FileUploadModel();
                file.Id = reader.GetValue<int>("id");
                file.OrigenalName = reader.GetValue<string>("OrigenalName");
                file.Relativepath = reader.GetValue<string>("relativepath");
                file.Savepath = reader.GetValue<string>("savepath");
                file.Extentionname = reader.GetValue<string>("extentionname");
                file.Type = reader.GetValue<int>("type");
                file.Objid = reader.GetValue<int>("objid");
                file.Objtype = reader.GetValue<int>("objtype");
                file.Creator = reader.GetValue<string>("creator");
                file.Creationip = reader.GetValue<string>("creationip");
                files.Add(file);
            }
            return files;
        }


        //        private const string SQLGetByAry = @"SELECT [id]
        //              ,[OrigenalName]
        //              ,[relativepath]
        //              ,[savepath]
        //              ,[extentionname]
        //              ,[type]
        //              ,[objid]
        //              ,[objtype]
        //              ,[creator]
        //              ,[creationip]
        //          FROM [EtownDB].[dbo].[fileupload] a WHERE EXISTS(
        //        	SELECT 1 FROM dbo.UDF_SplitStringToInt(@id,',') b
        //        		WHERE a.Id = b.Id)";
        private const string SQLGetByAry = @"SELECT [id]
      ,[OrigenalName]
      ,[relativepath]
      ,[savepath]
      ,[extentionname]
      ,[type]
      ,[objid]
      ,[objtype]
      ,[creator]
      ,[creationip]
  FROM [EtownDB].[dbo].[fileupload]  ";

        internal IList<FileUploadModel> GetById(string id)
        {
            var cmd = sqlHelper.PrepareTextSqlCommand(SQLGetByAry);
            cmd.AddParam("@id", id);
            using (var reader = cmd.ExecuteReader())
            {
                return FromReader(reader);
            }
        }

        //#region 1U新增

        /// <summary>
        /// 按自增Id删除上传的文件信息
        /// </summary>
        /// <param name="Id">自增Id</param>
        /// <returns>影响行数</returns>
        internal int Delete(int Id)
        {
            string sql = @"DELETE dbo.FileUpload WHERE [Id] = @Id";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@Id", Id);
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 通过自增id获取文件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal FileUploadModel GetFileById(int id)
        {
            string sql = @"SELECT [id]
      ,[OrigenalName]
      ,[relativepath]
      ,[savepath]
      ,[extentionname]
      ,[type]
      ,[objid]
      ,[objtype]
      ,[creator]
      ,[creationip]
  FROM [EtownDB].[dbo].[fileupload] f WHERE f.Id=@id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);

            FileUploadModel file = null;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    file = new FileUploadModel()
                    {
                        Id = reader.GetValue<int>("id"),
                        OrigenalName = reader.GetValue<string>("OrigenalName"),
                        Relativepath = reader.GetValue<string>("relativepath") == "" ? "Default.jpg" : reader.GetValue<string>("relativepath"),
                        Savepath = reader.GetValue<string>("savepath"),
                        Extentionname = reader.GetValue<string>("extentionname"),
                        Type = reader.GetValue<int>("type"),
                        Objid = reader.GetValue<int>("objid"),
                        Objtype = reader.GetValue<int>("objtype"),
                        Creator = reader.GetValue<string>("creator"),
                        Creationip = reader.GetValue<string>("creationip")
                    };
                }
            }
            //sqlHelper.Commit();
            //sqlHelper.Dispose();
            return file;
        }

        ///// <summary>
        ///// 通过自增id获取文件信息
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //internal Entities.FileUpload GetFileByObjIdType(int objId, int objType)
        //{
        //    string sql = "SELECT * FROM [ETS2Authentication].[dbo].[FileUpload] f WHERE f.ObjId=" + objId + " and f.ObjType=" + objType;
        //    var cmd = sqlHelper.PrepareTextSqlCommand(SQLGetByAry);
        //    cmd.AddParam("@ObjId", objId);
        //    cmd.AddParam("@ObjType", objType);

        //    FileUploadService.Entities.FileUpload file = new FileUploadService.Entities.FileUpload();
        //    using (var reader = cmd.ExecuteReader())
        //    {
        //        if (reader.Read())
        //        {

        //            file.Id = reader.GetValue<int>(FileUploadService.Entities.FileUpload.FILED_Id);
        //            file.ExtentionName = reader.GetValue<string>(FileUploadService.Entities.FileUpload.FILED_ExtName);
        //            file.CreationIp = reader.GetValue<string>(FileUploadService.Entities.FileUpload.FILED_CreationIp);
        //            file.Creator = reader.GetValue<string>(FileUploadService.Entities.FileUpload.FILED_Creator);
        //            file.RelativePath = reader.GetValue<string>(FileUploadService.Entities.FileUpload.FILED_FilePath);
        //            file.OrigenalName = reader.GetValue<string>(FileUploadService.Entities.FileUpload.FILED_FileName);
        //            reader.Close();
        //        }
        //    }
        //    return file;
        //}

        //#endregion



        private const string SQLGetByAry2 = @"SELECT [id]
      ,[OrigenalName]
      ,[relativepath]
      ,[savepath]
      ,[extentionname]
      ,[type]
      ,[objid]
      ,[objtype]
      ,[creator]
      ,[creationip]
  FROM [EtownDB].[dbo].[fileupload] where  id in (select fileuploadid from b2b_com_pro_childimg where proid=@proid)";

        internal IList<FileUploadModel> GetProChildImg(int objId)
        {
            var cmd = sqlHelper.PrepareTextSqlCommand(SQLGetByAry2);
            cmd.AddParam("@proid", objId);
            using (var reader = cmd.ExecuteReader())
            {
                return FromReader(reader);
            }
        }


        private const string SQLGetByAry3 = @"SELECT [id]
      ,[OrigenalName]
      ,[relativepath]
      ,[savepath]
      ,[extentionname]
      ,[type]
      ,[objid]
      ,[objtype]
      ,[creator]
      ,[creationip]
  FROM [EtownDB].[dbo].[fileupload] where  id in (select fileuploadid from b2b_com_pro_childimg where proid=@proid and proid in (select id from b2b_com_pro where com_id=@comid))";

        internal IList<FileUploadModel> GetProChildImg(int comid, int proid)
        {
            var cmd = sqlHelper.PrepareTextSqlCommand(SQLGetByAry3);
            cmd.AddParam("@proid", proid);
            cmd.AddParam("@comid", comid);
            using (var reader = cmd.ExecuteReader())
            {
                return FromReader(reader);
            }
        }
    }
}
