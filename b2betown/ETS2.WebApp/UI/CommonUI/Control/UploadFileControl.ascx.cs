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
    public partial class UploadFileControl : System.Web.UI.UserControl
    {
        public UploadFileInfo uploadFileInfo = new UploadFileInfo();
        public string fileUrl = AppSettings.CommonSetting.GetValue("FileUpload/FileUrl").ConvertTo<string>();//文件访问地址
        public string fileType = AppSettings.CommonSetting.GetValue("FileUpload/FileType").ConvertTo<string>();//文件类型
        public string fileDesc = AppSettings.CommonSetting.GetValue("FileUpload/FileDes").ConvertTo<string>();//文件类型描述
        public int fileSize = AppSettings.CommonSetting.GetValue("FileUpload/FileSize").ConvertTo<int>();//文件大小
        public string displayImgId = "";//页面上的显示图片控件Id
        public string displayImgNameId = "";//页面上的显示图片名控件Id
        public string DeleteBtnId = "";//页面上的删除按钮Id
        public bool IsImage = true;

        public string FileUploadId_ClientId = "";//用户控件-记录 生成的文件在FileUpload数据表的自增Id  的Html控件Id
        public string FilePath_ClientId = "";//用户控件-记录 上传文件的相对路径  的Html控件Id
        public string FileName_CLientId = "";//用户控件-记录 文件的原名  的Html控件Id
        public string File_CLientId = "";//用户控件-记录 文件的实体信息  的Html控件Id
        public int CreatorId = 0;


        bool displayCheckEdit = false;
        /// <summary>
        /// 是否显示编辑框(多个上传时设置为True)
        /// </summary>
        public bool DisplayCheckEdit
        {
            get { return displayCheckEdit; }
            set { displayCheckEdit = value; }
        }

        /// <summary>
        /// 上传文件返回的Id
        /// </summary>
        public string AryId
        {
            get;
            set;
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
            this.LoadData();
        }

        private void LoadData()
        {
            if (string.IsNullOrEmpty(this.AryId))
            {
                return;
            }

            var files = FileSerivce.GetFiles(this.AryId);
            if (files != null)
            {
                var data = from f in files
                           select new
                           {
                               Id = f.Id,
                               RelativePath = f.Relativepath,
                               OrigenalName = f.OrigenalName
                           };
                this.hideResult.Value = JsonConvert.SerializeObject(data);
            }
            ////throw new NotImplementedException();
        }
    }
   
}