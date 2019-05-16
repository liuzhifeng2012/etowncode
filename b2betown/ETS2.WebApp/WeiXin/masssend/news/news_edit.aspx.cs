using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.WeiXin.Service.WinXinService.BLL.WeiXinUploadDown;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.WeiXin.Service.WinXinService.BLL;
using System.IO;
using ETS.Framework;
using ETS2.Common.Business;

namespace ETS2.WebApp.WeiXin.masssend.news
{
    public partial class news_edit : System.Web.UI.Page
    {
        public int materialid = 0;//素材id
        public int comid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            materialid = Request["materialid"].ConvertTo<int>(0);

            if (UserHelper.ValidateLogin() == false)
            {
                Response.Redirect("/default.aspx");
            }
            else 
            {
                comid = UserHelper.CurrentCompany.ID;
            }
        }
        protected void bt_upload_Click(object sender, EventArgs e)
        {
            try
            {

                if (FileUpload1.PostedFile.FileName == "")
                {
                    this.lb_info.Text = "请选择文件！";
                    return;
                }
                else
                {
                    string filepath = FileUpload1.PostedFile.FileName;
                    if (!IsAllowedExtension(FileUpload1))
                    {
                        this.lb_info.Text = "上传文件格式不正确,只支持jpg!";
                    }
                    else
                    {
                        string filename = filepath.Substring(filepath.LastIndexOf("\\") + 1);
                        string suffix = filename.Split('.')[1];//获取上传文件的后缀
                        string newfilename = DateTime.Now.ToString("yyyyMMddhhmmss") + "." + suffix;//重命名上传文件
                        string Filepath = Server.MapPath("/WxGroupSendUploadFile/");//设置上传文件的保存路径
                        if (!Directory.Exists(Filepath))//判断路径是否存在
                        {
                            Directory.CreateDirectory(Filepath);//如果不存在创建文件夹           
                        }
                        FileUpload1.PostedFile.SaveAs(Filepath +newfilename);

                        WeiXinBasic basic = new WeiXinBasicData().GetWxBasicByComId(comid);
                        WXAccessToken m_accesstoken = new Weixin_tmplmsgManage().GetAccessToken(basic.Comid, basic.AppId, basic.AppSecret);
                        if (m_accesstoken == null)
                        {
                            Image1.ImageUrl = "/WxGroupSendUploadFile/" + newfilename;
                            this.lb_info.Text = "上传图片成功,获取微信调用接口凭证失败！";
                            return;
                        }
                        string thumb_media_id = new WxUploadDownManage().UploadMultimedia(m_accesstoken.ACCESS_TOKEN, "image", Filepath + newfilename);
                        if (thumb_media_id != null)
                        {
                            if (thumb_media_id == "")
                            {
                                this.lb_info.Text = "上传图片失败！";
                            }
                            else
                            {
                                Image1.ImageUrl = "/WxGroupSendUploadFile/" + newfilename;
                                hid_thumb_media_id.Value = thumb_media_id;

                                this.lb_info.Text = "上传图片成功！";
                            }
                        }
                        else
                        {
                            this.lb_info.Text = "上传图片失败！";
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                this.lb_info.Text = "上传发生错误！原因：" + ex.ToString();
            }
        }
        private static bool IsAllowedExtension(System.Web.UI.WebControls.FileUpload upfile)
        {
            string strOldFilePath = "";
            string strExtension = "";
            string[] arrExtension = { ".jpg" };
            if (upfile.PostedFile.FileName != string.Empty)
            {
                strOldFilePath = upfile.PostedFile.FileName;//获得文件的完整路径名 
                strExtension = strOldFilePath.Substring(strOldFilePath.LastIndexOf("."));//获得文件的扩展名，如：.jpg 
                for (int i = 0; i < arrExtension.Length; i++)
                {
                    if (strExtension.Equals(arrExtension[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}