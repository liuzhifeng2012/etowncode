using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework.UploadWeixin;
using System.Web.Script.Serialization;

namespace ETS2.WebApp.WeiXin.masssend.appmsg
{
    public partial class list : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            //新建一个form表单项，就是需要提交哪些字段和数据的列表

            List<FormItem> list = new List<FormItem>();

            //添加微信接口上的access_token参数，注意，access_token是有过期时间的，代码中的access_token肯定过期了，获取access_token的地址请点击我，需要有自己的服务号才会有的

            list.Add(new FormItem()
            {
                Name = "access_token",
                ParamType =
                    ParamType.Text,
                Value = "MlfTORyg_dRTuiQThmKUxVVkK7q_SMEd0y9GwBmj6NJw3E0J2jVnC3RxgdO1Yjog2QD4DDxhdqEkZaklR7czq8sSbW4mnhM7n9-5lIIymVGkrBAv2nnnktUyYcuYTMs2SYtp-pn6IWEtTpsFVlUFZQ"
            });
            //添加FORM表单中这条数据的类型，目前只做了两种，一种是文本，一种是文件

            list.Add(new FormItem() { Name = "type", Value = "image", ParamType = ParamType.Text });

            //添加Form表单中文件的路径，路径必须是基于硬盘的绝对路径

            list.Add(new FormItem() { Name = "media", Value = @"d:\1.jpg", ParamType = ParamType.File });

            //通过Funcs静态类中的PostFormData方法，将表单数据发送至http://file.api.weixin.qq.com/cgi-bin/media/upload腾讯上传下载文件接口
            string result = Funcs.PostFormData(list, "http://file.api.weixin.qq.com/cgi-bin/media/upload");

            //获取返回值，并取出的结果中的media_id，注意，有可能返回的是腾讯的错误代码，请自行判断

            JavaScriptSerializer jss = new JavaScriptSerializer();

            var mydata = jss.Deserialize(result,typeof(Xml));

            ////通过Funcs静态类中的SaveFileFromUrl方法，将指定微信media_id的文件下载到本机

            //var saveResult = Funcs.SaveFileFromUrl(@"d:\lee.jpg", "http://file.api.weixin.qq.com/cgi-bin/media/get?access_token=MlfTORyg_dRTuiQThmKUxVVkK7q_SMEd0y9GwBmj6NJw3E0J2jVnC3RxgdO1Yjog2QD4DDxhdqEkZaklR7czq8sSbW4mnhM7n9-5lIIymVGkrBAv2nnnktUyYcuYTMs2SYtp-pn6IWEtTpsFVlUFZQ&media_id=" + mydata["media_id"].ToString());


        }
    }
}