using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;
using FileUpload.FileUpload.Data;
using FileUpload.FileUpload.Entities;

namespace ETS2.Common.Business
{
    public class FileSerivce
    {
        public static IList<FileUploadModel> GetFiles(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return null;
            }
            var data = new FileUploadData(null);
            return data.GetById(id);
        }
        public static string defaultimgurl = "/Images/defaultThumb.png";//默认图片
        public static string GetImgUrl(int imgid)
        {
            string fileUrl = AppSettings.CommonSetting.GetValue("FileUpload/FileUrl").ConvertTo<string>();//文件访问地址

            if (imgid == 0)
            {
                return defaultimgurl;
            }
            else
            {
                FileUploadModel identityFileUpload = new FileUploadData().GetFileById(imgid);
                if (identityFileUpload != null)
                {
                    return fileUrl + identityFileUpload.Relativepath;
                }
                else
                {
                    return defaultimgurl;
                }
            }
        }
        //public static IList<FileUpload> GetFiles(int objId, int typeId)
        //{
        //    var model = ContainerUtility.Container.Resolve<FileUploadData>();
        //    return model.Get(objId, typeId);
        //}

        //public static string GetLogoFilesPath(string id)
        //{
        //    string result = "/Images/NewImage/logo.png";
        //    FileUploadData model = ETS.Framework.ContainerUtility.Container.Resolve<FileUploadService.Data.FileUploadData>();
        //    IList<FileUpload> fileList = model.GetById(id);
        //    if (fileList.Count > 0)
        //    {
        //        FileUpload currrentInfor = fileList.First();
        //        if (!string.IsNullOrEmpty(currrentInfor.RelativePath))
        //        {
        //            var configHelper = new ConfigureHelper("CommonScetion");
        //            string fileHost = configHelper.GetValue("/FileUpload/Host");
        //            result = fileHost + currrentInfor.RelativePath;
        //        }
        //    }
        //    return result;
        //}

    }
}
