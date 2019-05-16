using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.Common.Business;
using Newtonsoft.Json;
using FileUpload.FileUpload.Entities;

namespace ETS2.WebApp.UI.CommonUI.Control
{
    public partial class MultiUploadFileControl : System.Web.UI.UserControl
    {
        public UploadFileInfo uploadFileInfo = new UploadFileInfo();
        public string fileUrl = AppSettings.CommonSetting.GetValue("FileUpload/FileUrl").ConvertTo<string>();//文件访问地址
        public string fileType = AppSettings.CommonSetting.GetValue("FileUpload/FileType").ConvertTo<string>();//文件类型
        public string fileDesc = AppSettings.CommonSetting.GetValue("FileUpload/FileDes").ConvertTo<string>();//文件类型描述
        public int fileSize = AppSettings.CommonSetting.GetValue("FileUpload/FileSize").ConvertTo<int>();//文件大小
        public bool IsImage = true;

        public string FileUploadId_ClientId = "";//用户控件-记录 生成的文件在FileUpload数据表的自增Id  的Html控件Id
        public string FilePath_ClientId = "";//用户控件-记录 上传文件的相对路径  的Html控件Id
        public string FileName_CLientId = "";//用户控件-记录 文件的原名  的Html控件Id
        public string File_CLientId = "";//用户控件-记录 文件的实体信息  的Html控件Id
        public int CreatorId = 0;


        public bool isMultiUpload = false;
        /// <summary>
        /// 是否是多图片上传
        /// </summary>
        public bool IsMultiUpload
        {
            get { return isMultiUpload; }
            set { isMultiUpload = value; }
        }

        public string viewImgFlag = "";//查询的图片种类
        /// <summary>
        /// 查询的图片种类
        /// </summary>
        public string ViewImgFlag
        {
            get { return viewImgFlag; }
            set { viewImgFlag = value; }
        }
      

        protected void Page_Load(object sender, EventArgs e)
        {

            //将生成的Html控件Id赋值给变量
            FileUploadId_ClientId = hidFileUploadId.ClientID;
            FilePath_ClientId = hidFilePath.ClientID;
            FileName_CLientId = hidFileName.ClientID;
            File_CLientId = hidFileObject.ClientID;
            CreatorId = ETS2.Common.Business.UserHelper.CurrentUser().Id;

            if (IsImage)
            {
                fileType = AppSettings.CommonSetting.GetValue("FileUpload/ImgType").ConvertTo<string>();
                fileDesc = AppSettings.CommonSetting.GetValue("FileUpload/ImgDes").ConvertTo<string>();
                fileSize = AppSettings.CommonSetting.GetValue("FileUpload/ImgSize").ConvertTo<int>();
            }
        }
    }
}