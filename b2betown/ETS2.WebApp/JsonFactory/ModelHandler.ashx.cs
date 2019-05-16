using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Framework;
using ETS.JsonFactory;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Common.Business;
using System.Web.SessionState;
using Newtonsoft.Json;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS2.CRM.Service.CRMService.Modle;
namespace ETS2.WebApp.JsonFactory
{
    /// <summary>
    /// ModelHandler 的摘要说明
    /// </summary>
    public class ModelHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string oper = context.Request["oper"].ConvertTo<string>("");

            if (oper != "")
            {
                //模板列表
                if (oper == "modelpagelist")
                {

                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    var data = ModelJsonData.ModelPageList(Pageindex, Pagesize, comid);
                    context.Response.Write(data);
                }

                //模板菜单列表
                if (oper == "modelmenupagelist")
                {

                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    int modelid = context.Request["modelid"].ConvertTo<int>(0);

                    var data = ModelJsonData.ModelMenuPageList(modelid,Pageindex, Pagesize);
                    context.Response.Write(data);
                }

                //模板指定菜单列表
                if (oper == "modelzhidingpagelist")
                {

                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(100);
                    int id = context.Request["id"].ConvertTo<int>(0);

                    var data = ModelJsonData.ModelzhidingPageList(id, Pageindex, Pagesize);
                    context.Response.Write(data);
                }

                //单个模板列表
                if (oper == "getModelById")
                {

                    int modelid = context.Request["modelid"].ConvertTo<int>(0);
                    var data = ModelJsonData.GetModelById(modelid);
                    context.Response.Write(data);
                }


                //单个模板菜单列表
                if (oper == "getmodelmenubyId")
                {

                    int id = context.Request["id"].ConvertTo<int>(0);
                    var data = ModelJsonData.GetModelMenuById(id);
                    context.Response.Write(data);
                }


                //单个模板列表
                if (oper == "getComModel")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    var data = ModelJsonData.GetComModel(comid);
                    context.Response.Write(data);
                }

                //模板设定
                if (oper == "selectmodel") {

                    int modelid = context.Request["modelid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    var data = ModelJsonData.SelectModel(modelid, comid);
                    context.Response.Write(data);

                }


                //模板管理
                if (oper == "ModelInsertOrUpdate")
                {
                    int modelid = context.Request["modelid"].ConvertTo<int>(0);
                    string title = context.Request["title"].ConvertTo<string>("");
                    int daohangnum = context.Request["daohangnum"].ConvertTo<int>(0);
                    string style_str = context.Request["style_str"].ConvertTo<string>("");
                    string html_str = context.Request["html_str"].ConvertTo<string>("");
                    int daohangimg = context.Request["daohangimg"].ConvertTo<int>(0);
                    int bgimage_w = context.Request["bgimage_w"].ConvertTo<int>(0);
                    int bgimage_h = context.Request["bgimage_h"].ConvertTo<int>(0);
                    int bgimage = context.Request["bgimage"].ConvertTo<int>(0);
                    int smallimg = context.Request["smallimg"].ConvertTo<int>(0);


                    B2b_model modelinfo = new B2b_model();

                    modelinfo.Id = modelid;
                    modelinfo.Title = title;
                    modelinfo.Daohangimg = daohangimg;
                    modelinfo.Bgimage_w = bgimage_w;
                    modelinfo.Bgimage_h = bgimage_h;
                    modelinfo.Daohangnum = daohangnum;
                    modelinfo.Style_str = style_str;
                    modelinfo.Html_str = html_str;
                    modelinfo.Bgimage = bgimage;
                    modelinfo.Smallimg = smallimg;

                    var data = ModelJsonData.ModelInsertOrUpdate(modelinfo);
                    context.Response.Write(data);
                }

                //栏目管理
                if (oper == "ModelMenuInsertOrUpdate")
                {
                    int modelid = context.Request["modelid"].ConvertTo<int>(0);
                    string name = context.Request["name"].ConvertTo<string>("");
                    string linkurl = context.Request["linkurl"].ConvertTo<string>("");
                    int linktype = context.Request["linktype"].ConvertTo<int>(0);
                    int imgurl = context.Request["imgurl"].ConvertTo<int>(0);
                    int id = context.Request["id"].ConvertTo<int>(0);
                    string fonticon = context.Request["fonticon"].ConvertTo<string>("");

                    B2b_modelmenu modelinfo = new B2b_modelmenu();

                    modelinfo.Id = id;
                    modelinfo.Modelid = modelid;
                    modelinfo.Imgurl = imgurl;
                    modelinfo.Linkurl = linkurl;
                    modelinfo.Linktype = linktype;
                    modelinfo.Name = name;
                    modelinfo.Sortid = 0;
                    modelinfo.Fonticon = fonticon;

                    var data = ModelJsonData.ModelMenuInsertOrUpdate(modelinfo);
                    context.Response.Write(data);
                }



                if (oper == "modelmenuSort")
                {
                    string ids = context.Request["ids"].ConvertTo<string>("");

                    string data = ModelJsonData.ModelMenuSort(ids);
                    context.Response.Write(data);
                }


                //图片库列表
                if (oper == "imageLibraryList")
                {

                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    int usetype = context.Request["usetype"].ConvertTo<int>(0);
                    int modelid = context.Request["modelid"].ConvertTo<int>(0);


                    var data = ModelJsonData.GetimageLibraryList(usetype, Pageindex, Pagesize,modelid);
                    context.Response.Write(data);
                }

                //读取图标库
                if (oper == "fontLibraryList")
                {

                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    int usetype = context.Request["usetype"].ConvertTo<int>(0);
                    int modelid = context.Request["modelid"].ConvertTo<int>(0);


                    var data = ModelJsonData.GetfontLibraryList(usetype, Pageindex, Pagesize);
                    context.Response.Write(data);
                }
                



                //图片库列表
                if (oper == "imagemodelLibraryList")
                {

                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    int usetype = context.Request["usetype"].ConvertTo<int>(0);
                    int modelid = context.Request["modelid"].ConvertTo<int>(0);


                    var data = ModelJsonData.GettypemodelidimageLibraryList(usetype, modelid, Pageindex, Pagesize);
                    context.Response.Write(data);
                }

                //获取图片库图片
                if (oper == "imageLibraryByid")
                {
                    int id = context.Request["id"].ConvertTo<int>(0);

                    var data = ModelJsonData.GetimageLibraryByid(id);
                    context.Response.Write(data);
                }

                //删除图片
                if (oper == "deleteLibraryimage")
                {

                    int id = context.Request["id"].ConvertTo<int>(0);

                    var data = ModelJsonData.DeleteLibraryimage(id);
                    context.Response.Write(data);

                }


                //添加修改图片
                if (oper == "libraryInsertOrUpdate")
                {
                    int id = context.Request["id"].ConvertTo<int>(0);
                    int Usetype = context.Request["Usetype"].ConvertTo<int>(0);
                    int imgurl = context.Request["imgurl"].ConvertTo<int>(0);
                    int Width = context.Request["Width"].ConvertTo<int>(0);
                    int Height = context.Request["Height"].ConvertTo<int>(0);
                    int Modelid = context.Request["Modelid"].ConvertTo<int>(0);

                    b2b_image_library modelinfo = new b2b_image_library();

                    modelinfo.Id = id;
                    modelinfo.Usetype = Usetype;
                    modelinfo.Imgurl = imgurl;
                    modelinfo.Width = Width;
                    modelinfo.Height = Height;
                    modelinfo.Modelid = Modelid;

                    var data = ModelJsonData.LibraryInsertOrUpdate(modelinfo);
                    context.Response.Write(data);
                }


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