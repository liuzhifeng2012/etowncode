using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using ETS2.WebApp.cn.etown.image;
using ETS2.Common.Business;
using ETS.Framework;

namespace ETS2.WebApp.JsonFactory
{
    /// <summary>
    /// ImgUploadHandler 的摘要说明
    /// </summary>
    public class ImgUploadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //在此写入您的处理程序实现。
            context.Response.ContentType = "text/plain";
            //验证上传的权限TODO
            string _fileNamePath = "";
            try
            {
                _fileNamePath = context.Request.Files[0].FileName;

                //开始上传
                string _savedFileResult = UpLoadImage(_fileNamePath, context);
                string ret = "{\"type\":\"100\",\"msg\":\"" + _savedFileResult + "\"}";
                context.Response.Write(ret);
            }
            catch
            {
                //context.Response.Write("上传提交出错");
                string ret = "{\"type\":\"1\",\"msg\":\"上传提交出错\"}";
                context.Response.Write(ret);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        public string UpLoadImage(string fileNamePath, HttpContext context)
        {
            try
            {
                HttpPostedFile file = context.Request.Files[0];

                //获得文件扩展名
                string fileNameExt = Path.GetExtension(file.FileName).ToLower();
                //验证合法的文件
                if (CheckImageExt(fileNameExt))
                {
                    ImgUploadService upload = new ImgUploadService();

                    string virPath = "";
                    byte[] data = new byte[file.ContentLength];

                    file.InputStream.Read(data, 0, file.ContentLength);

                    string physicPath = upload.UpLoadAndSave(data, ref virPath, Path.GetExtension(file.FileName).ToLower());

                    string FullPath = "http://image.etown.cn/UploadFile/" + virPath;

                    return FullPath;
                }
                else
                {
                    throw new Exception("文件格式非法，请上传.gif, .jpg, .jpeg, .bmp, .png格式的文件。");
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            #region
            //try
            //{
            //    string serverPath = System.Web.HttpContext.Current.Server.MapPath("~");

            //    string toFilePath = Path.Combine(serverPath, @"UploadFile\");

            //    //获取要保存的文件信息
            //    FileInfo file = new FileInfo(fileNamePath);
            //    //获得文件扩展名
            //    string fileNameExt = file.Extension.ToLower();

            //    //验证合法的文件
            //    if (CheckImageExt(fileNameExt))
            //    {
            //        //生成将要保存的随机文件名
            //        string fileName = GetImageName() + fileNameExt;

            //        //获得要保存的文件路径
            //        string serverFileName = toFilePath + fileName;
            //        //物理完整路径                    
            //        string toFileFullPath = serverFileName; //HttpContext.Current.Server.MapPath(toFilePath);

            //        //将要保存的完整文件名                
            //        string toFile = toFileFullPath;//+ fileName;

            //        ///创建WebClient实例       
            //        WebClient myWebClient = new WebClient();
            //        //设定windows网络安全认证   方法1
            //        myWebClient.Credentials = CredentialCache.DefaultCredentials;
            //        ////设定windows网络安全认证   方法2

            //        context.Request.Files[0].SaveAs(toFile);

            //        string relativePath = System.Web.HttpContext.Current.Request.ApplicationPath + string.Format(@"UploadFile/{0}", fileName);

            //        return relativePath;
            //    }
            //    else
            //    {
            //        throw new Exception("文件格式非法，请上传.gif, .jpg, .jpeg, .bmp, .png格式的文件。");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return ex.Message;
            //}
            #endregion
        }



        #region Private Methods
        /// <summary>
        /// 检查是否为合法的上传图片
        /// </summary>
        /// <param name="_fileExt"></param>
        /// <returns></returns>
        private bool CheckImageExt(string _ImageExt)
        {
            string[] allowExt = new string[] { ".gif", ".jpg", ".jpeg", ".bmp", ".png" };
            for (int i = 0; i < allowExt.Length; i++)
            {
                if (allowExt[i] == _ImageExt) { return true; }
            }
            return false;

        }

        private string GetImageName()
        {
            Random rd = new Random();
            StringBuilder serial = new StringBuilder();
            serial.Append(DateTime.Now.ToString("yyyyMMddHHmmssff"));
            serial.Append(rd.Next(0, 999999).ToString());
            return serial.ToString();

        }

        #endregion
    }
}