using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using FileUpload.FileUpload.Data.InternalData;
using FileUpload.FileUpload.Entities;
using System.IO;

namespace FileUpload.FileUpload.Data
{
    public class FileUploadData
    {
        private string connString;
        public FileUploadData()
        {
        }
        public FileUploadData(string connString)
        {
            this.connString = connString;
        }

        //#region 批量添加上传文件信息
        ///// <summary>
        ///// 批量添加上传文件信息
        ///// </summary>
        ///// <param name="files">文件实体类集合</param>
        ///// <returns>上传文件Id集合</returns>
        //public IList<int> UploadFile(int objId, FileObjType objType, params FileUploadService.Entities.FileUpload[] files)
        //{
        //    using (var sqlHelper = new SqlHelper(connString))
        //    {
        //        if (files == null && files.Length == 0)
        //        {
        //            return null;
        //        }

        //        var internalData = new InternalFileUpload(sqlHelper);
        //        IList<int> result = new List<int>(files.Length);
        //        foreach (var item in files)
        //        {
        //            item.ObjId = objId;
        //            item.ObjType = (int)objType;
        //            int retUpload = internalData.CreateUploadInfo(item);
        //            result.Add(retUpload);
        //        }
        //        return result;
        //    }
        //}
        //#endregion

        //#region 获取对象的文件上传信息
        ///// <summary>
        ///// 获取对象的文件上传信息
        ///// </summary>
        ///// <param name="objId">对象Id</param>
        ///// <param name="objType">对象类型</param>
        ///// <returns>上传文件集合</returns>
        //public IList<FileUploadService.Entities.FileUpload> Get(int objId, int objType)
        //{
        //    using (var sqlHelper = new SqlHelper(this.connString))
        //    {
        //        return sqlHelper.ExecSqlHelper<IList<FileUploadService.Entities.FileUpload>>(h => new InternalFileUpload(h).GetFileUpload(objId, objType));
        //    }
        //}
        //#endregion


        //#region 按对象删除上传的文件信息
        ///// <summary>
        ///// 按对象删除上传的文件信息
        ///// </summary>
        ///// <param name="objId">对象Id</param>
        ///// <param name="objType">对象类型</param>
        ///// <returns>影响行数</returns>
        //public int Delete(int objId, int objType)
        //{
        //    using (var sqlHelper = new SqlHelper(this.connString))
        //    {
        //        return sqlHelper.ExecSqlHelper<int>(h => new InternalFileUpload(h).Delete(objId, objType));
        //    }
        //}
        //#endregion



        public IList<FileUploadModel> GetById(string id)
        {
            using (var sqlHelper = new SqlHelper(this.connString))
            {
                var internalData = new InternalFileUpload(sqlHelper);
                return internalData.GetById(id);
            }
        }


  
        /// <summary>
        /// 按自增Id删除上传的文件信息
        /// </summary>
        /// <param name="Id">自增Id</param>
        /// <returns>影响行数</returns>
        public int DeleteById(int Id)
        {
            using (var sqlHelper = new SqlHelper(this.connString))
            {
                return sqlHelper.ExecSqlHelper<int>(h => new InternalFileUpload(h).Delete(Id));
            }
        }


        /// <summary>
        /// 通过自增id获取文件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FileUploadModel GetFileById(int Id)
        {
            try
            {
                using (var sqlHelper = new SqlHelper(this.connString))
                {
                    return sqlHelper.ExecSqlHelper<FileUploadModel>(h => new InternalFileUpload(h).GetFileById(Id));
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        ///// <summary>
        ///// 通过自增id获取文件信息
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public Entities.FileUpload GetFileByObjIdType(int objId, int objType)
        //{
        //    using (var sqlHelper = new SqlHelper(this.connString))
        //    {
        //        return sqlHelper.ExecSqlHelper<Entities.FileUpload>(h => new InternalFileUpload(h).GetFileByObjIdType(objId, objType));
        //    }
        //}


        public FileUploadModel InsertFileUpload(FileUploadModel fileUpload)
        {
            using (var sqlHelper = new SqlHelper(this.connString))
            {
                int id = sqlHelper.ExecSqlHelper<int>(h => new InternalFileUpload(h).CreateUploadInfo(fileUpload));
                return GetFileById(id);
            }
        }


        public IList<FileUploadModel> GetProChildImg(int objId)
        {
            try
            {
                using (var sqlHelper = new SqlHelper(this.connString))
                {
                    return sqlHelper.ExecSqlHelper<IList<FileUploadModel>>(h => new InternalFileUpload(h).GetProChildImg(objId));
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public IList<FileUploadModel> Getprochildimglist(int comid, int proid)
        {
            try
            {
                using (var sqlHelper = new SqlHelper(this.connString))
                {
                    return sqlHelper.ExecSqlHelper<IList<FileUploadModel>>(h => new InternalFileUpload(h).GetProChildImg(comid,proid));
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
