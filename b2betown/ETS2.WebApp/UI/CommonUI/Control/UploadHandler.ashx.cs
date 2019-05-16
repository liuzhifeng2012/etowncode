using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI.WebControls;
using FileUpload.FileUpload.Entities;
using FileUpload.FileUpload.Data;
using ETS.Framework;
using System.Collections;
using ETS2.WebApp.cn.etown.image;
using ETS.JsonFactory;
using ETS2.PM.Service.PMService.Data;


namespace ETS2.WebApp.UI.CommonUI.Control
{
    /// <summary>
    /// UploadHandler 的摘要说明
    /// </summary>
    public class UploadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string Flag = HttpContext.Current.Request.QueryString["OptionFlag"];

            switch (Flag)
            {
                case "UploadFile":
                    UploadFile(context);
                    break;
                case "DeleteFile":
                    DeleteFile(context);
                    break;
                case "GetProChildImg":
                    GetProChildImg(context);
                    break;
            }
        }
        /// <summary>
        /// 查询产品子级图片
        /// </summary>
        private void GetProChildImg(HttpContext context)
        {
            int objId = Convert.ToInt32(context.Request["ObjId"]);
            string data = FileUploadJsonData.GetProChildImg(objId);
            context.Response.Write(data);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="context"></param>
        private static void UploadFile(HttpContext context)
        {
            try
            {
                HttpPostedFile file = context.Request.Files["FileData"];

                int objId = Convert.ToInt32(context.Request["ObjId"]);
                int ObjType = Convert.ToInt32(context.Request["ObjType"]);
                string FileType = Convert.ToString(context.Request["FileType"]);
                int FileSize = Convert.ToInt32(context.Request["FileSize"]);
                string CreatorId = context.Request["CreatorId"].ToString();
                //string ObjTypeName = Convert.ToString((context.Request["ObjTypeName"]));
                //string uploadpath = HttpContext.Current.Server.MapPath(context.Request["folder"]) + "\\";
                //string uploadpath = @"C:\Users\Administrator\AppData\Roaming\Microsoft\Windows\Network Shortcuts\UploadFileTest (10.0.4.18)";
                //string path = @"Z:\";

                //检查文件
                if (FileType != "*" && !FileType.Contains(Path.GetExtension(file.FileName).ToLower()))//验证类型
                {
                    context.Response.Write("typeError");
                    return;
                }
                else if (file.ContentLength > FileSize)//检查文件大小
                {
                    context.Response.Write("sizeError");
                    return;
                }

                if (file != null)
                {
                    /*----已修改(不能用对象初始化器，报代码已经优化，不能计算值)------*/
                    var fileUpload = new FileUploadModel();
                    fileUpload.Objid = objId;
                    fileUpload.Objtype = ObjType;
                    fileUpload.Creationip = HttpContext.Current.Request.UserHostAddress;
                    fileUpload.Creator = CreatorId;
                    fileUpload.OrigenalName = file.FileName;
                    #region
                    ////走本地
                    //fileUpload = FileUploadBLL.GeneratePathFile(file.FileName, fileUpload);
                    //file.SaveAs(fileUpload.SavePath);//创建文件

                    ////走wcf服务
                    //fileUpload = FileUploadBLL.GeneratePathFile(file.FileName, fileUpload, file.InputStream);

                    ////新方法
                    //string uploadPath =
                    //HttpContext.Current.Server.MapPath(@context.Request["folder"]) + "\\";

                    //if (file != null)
                    //{
                    //    if (!Directory.Exists(uploadPath))
                    //    {
                    //        Directory.CreateDirectory(uploadPath);
                    //    }
                    //    file.SaveAs(uploadPath + file.FileName);
                    //    //下面这句代码缺少的话，上传成功后上传队列的显示不会自动消失
                    //    context.Response.Write("1");
                    //}
                    //else
                    //{
                    //    context.Response.Write("0");
                    //}  
                    #endregion
                    //UpLoadAndSaveImage upload = new UpLoadAndSaveImage();
                    ImgUploadService upload = new ImgUploadService();
                    try
                    {
                        string virPath = "";
                        //string physicPath = HttpContext.Current.Server.MapPath(@context.Request.ApplicationPath + "/" + "UploadFile/");

                        byte[] data = new byte[file.ContentLength];

                        file.InputStream.Read(data, 0, file.ContentLength);

                        string physicPath = upload.UpLoadAndSave(data, ref virPath, Path.GetExtension(file.FileName).ToLower());

                        //文件路径等信息，插入数据库
                        fileUpload.Relativepath = virPath;
                        fileUpload.Savepath = physicPath;
                        fileUpload.Type = 0;
                        fileUpload = new FileUploadData().InsertFileUpload(fileUpload);




                        string strFileObject = "[{\"Id\":\"" + fileUpload.Id + "\",\"ObjType\":\"" + fileUpload.Objtype
                            + "\",\"ObjId\":\"" + fileUpload.Objid + "\",\"FileName\":\"" + fileUpload.OrigenalName
                            + "\",\"FilePath\":\"" + fileUpload.Relativepath + "\",\"ExtName\":\"" + fileUpload.Extentionname
                            + "\",\"Type\":\"" + fileUpload.Type
                             + "\",\"physicPath\":\"" + fileUpload.Savepath
                            + "\",\"Creator\":\"" + fileUpload.Creator + "\",\"CreationIp\":\"" + fileUpload.Creationip + "\"}]";

                        //context.Response.Write(fileUpload.Id.ToString() + "|" + fileUpload.RelativePath + "|" + fileUpload.OrigenalName);
                        context.Response.Write(strFileObject);


                    }
                    catch (Exception ex)
                    {
                        context.Response.Write(ex.Message);
                        return;
                    }



                }
                else
                {
                    context.Response.Write("noFile");
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("Exception:" + ex.ToString());
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteFile(HttpContext context)
        {
            try
            {
                FileUploadModel model = new FileUploadModel();
                if (context.Request["fileUploadId"] != null)
                {
                    int fileUploadId = Convert.ToInt32(context.Request["fileUploadId"]);
                    model = new FileUploadData().GetFileById(fileUploadId);
                     if(model!=null){
                    //删除产品子级图片表
                    new B2bComProData().DelProChildImg(fileUploadId);

                    //删除数据库里的附件信息
                    new FileUploadData().DeleteById(fileUploadId);

                    ImgUploadService upload = new ImgUploadService();
                    //删除文件
                    upload.DelFile(model.Savepath);
                    //File.Delete(model.Savepath);
                     }
                    context.Response.Write("success");

                }
                else
                {
                    //int objId = Convert.ToInt32(context.Request["objId"]);
                    //int objType = Convert.ToInt32(context.Request["objType"]);
                    //model = new FileUploadData().GetFileByObjIdType(objId, objType);
                    context.Response.Write("fail");
                }
 
                
            }
            catch (Exception ex)
            {
                context.Response.Write("Exception:" + ex.ToString());
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}