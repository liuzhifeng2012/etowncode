using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Modle.Enum;
using ETS.JsonFactory;

namespace ETS2.WebApp.JsonFactory
{
    /// <summary>
    /// DirectSellHandler 的摘要说明
    /// </summary>
    public class DirectSellHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string oper = context.Request["oper"].ConvertTo<string>("");
            if (oper != "")
            {

                if (oper == "editcdirectset")
                {
                    var directsellsetid = context.Request["directsellsetid"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var userid = context.Request["userid"].ConvertTo<int>(0);

                    var title = context.Request["title"].ConvertTo<string>("");
                    var kefu = context.Request["kefu"].ConvertTo<string>("");
                    var buttom = context.Request["bottom"].ConvertTo<string>("");

                    var smsaccount = context.Request["smsaccount"].ConvertTo<string>("");
                    var smspass = context.Request["smspass"].ConvertTo<string>("");
                    var smssign = context.Request["smssign"].ConvertTo<string>("");

                    var logo = context.Request["logo"].ConvertTo<string>("");
                    var smalllogo = context.Request["smalllogo"].ConvertTo<string>("");

                    B2b_company_saleset saleset = new B2b_company_saleset()
                    {
                        Id = directsellsetid,
                        Com_id = comid,
                        Dealuserid = userid,
                        Title = title,
                        Service_Phone = kefu,
                        Copyright = buttom,
                        Smsaccount = smsaccount,
                        Smspass = smspass,
                        Smssign = smssign,
                        Model_style = (int)ModelStyle.DefaultModel,
                        Payto = 0,
                        WorkingHours = "",
                        Tophtml = "",
                        BottomHtml = "",

                        Logo = logo,
                        Smalllogo = smalllogo,
                        Compressionlogo = smalllogo
                    };
                    var data = DirectSellJsonData.InsertOrUpdate(saleset);
                    context.Response.Write(data);

                }
                //修改短信设置
                if (oper == "editsmsset")
                {
                    var directsellsetid = context.Request["directsellsetid"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                   
                    var smsaccount = context.Request["smsaccount"].ConvertTo<string>("");
                    var smspass = context.Request["smspass"].ConvertTo<string>("");
                    var smssign = context.Request["smssign"].ConvertTo<string>("");
                    var smstype = context.Request["smstype"].ConvertTo<int>(0);
                    var subid = context.Request["subid"].ConvertTo<string>("");


                    if (smstype == 1) {
                        smssign = smssign.Trim();
                        if (smssign.Substring(0, 1)!="【"){

                            var data1 = "{\"type\":1,\"msg\":\"签名必须以左中文左括号 【 \"}";
                            context.Response.Write(data1);
                            return;
                        }

                        if (smssign.Substring(smssign.Length-1, 1)!="】"){

                            var data2 = "{\"type\":1,\"msg\":\"签名必须以右中文左括号 】\"}";
                            context.Response.Write(data2);
                            return;
                        }
                    }




                    B2b_company_saleset saleset = new B2b_company_saleset()
                    {
                        Id = directsellsetid,
                        Com_id = comid,
                        Smsaccount = smsaccount,
                        Smspass = smspass,
                        Smssign = smssign,
                        Smstype = smstype,
                        Subid = subid
                    };
                    var data = DirectSellJsonData.Updatesms(saleset);
                    context.Response.Write(data);

                }

                if (oper == "getdirectsellset")
                {
                    var comid = context.Request["comid"];
                    var data = DirectSellJsonData.GetDirectSellByComid(comid);
                    context.Response.Write(data);
                }
                if (oper == "getfileupload")
                {
                    var id = context.Request["id"];
                    var data = DirectSellJsonData.GetFileupload(id);
                    context.Response.Write(data);
                }
                if (oper == "getcomlogobyproid")
                {
                    var proid = context.Request["proid"].ConvertTo<int>(0);
                    var data = DirectSellJsonData.Getcomlogobyproid(proid);
                    context.Response.Write(data);
                }

                if (oper == "getimagelist")
                {
                    var typeid = context.Request["typeid"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    //var usetype = context.Request["usetype"].ConvertTo<int>(0);
                    var data = DirectSellJsonData.Getimagelist(comid, typeid, pageindex, pagesize);
                    context.Response.Write(data);
                }

                if (oper == "getchannelimagelist")
                {
                    var typeid = context.Request["typeid"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var channelcompanyid = context.Request["channelcompanyid"].ConvertTo<int>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var data = DirectSellJsonData.Getchannelimagelist(comid, typeid, channelcompanyid, pageindex, pagesize);
                    context.Response.Write(data);
                }

                if (oper == "pagegetimagelist")
                {
                    var typeid = context.Request["typeid"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var data = DirectSellJsonData.PageGetimagelist(comid, typeid);
                    context.Response.Write(data);
                }

                if (oper == "getimagebyid")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var data = DirectSellJsonData.GetimageByComid(comid, id);
                    context.Response.Write(data);
                }


                if (oper == "getchannelimagebyid")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var channelcompanyid = context.Request["channelcompanyid"].ConvertTo<int>(0);
                    var data = DirectSellJsonData.GetchannelimageByComid(comid, id, channelcompanyid);
                    context.Response.Write(data);
                }
                //修改图片
                if (oper == "editimage")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var typeid = context.Request["typeid"].ConvertTo<int>(0);
                    var Imgurl = context.Request["imgurl"].ConvertTo<int>(0);
                    var Linkurl= context.Request["Linkurl"].ConvertTo<string>("");
                    var title = context.Request["title"].ConvertTo<string>("");


                    B2b_company_image imagemodel = new B2b_company_image()
                    {
                        Id = id,
                        Com_id = comid,
                        Typeid = typeid,
                        Imgurl = Imgurl,
                        Linkurl = Linkurl,
                        Title = title,
                        Channelcompanyid = 0,
                    };
                    var data = DirectSellJsonData.InsertOrUpdate(imagemodel);
                    context.Response.Write(data);

                }

                //修改图片
                if (oper == "editchannelimage")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var typeid = context.Request["typeid"].ConvertTo<int>(0);
                    var Imgurl = context.Request["imgurl"].ConvertTo<int>(0);
                    var Linkurl = context.Request["Linkurl"].ConvertTo<string>("");
                    var title = context.Request["title"].ConvertTo<string>("");
                    var channelcompanyid = context.Request["channelcompanyid"].ConvertTo<int>(0);


                    B2b_company_image imagemodel = new B2b_company_image()
                    {
                        Id = id,
                        Com_id = comid,
                        Typeid = typeid,
                        Imgurl = Imgurl,
                        Linkurl = Linkurl,
                        Title = title,
                        Channelcompanyid = channelcompanyid,
                    };
                    var data = DirectSellJsonData.ChannelInsertOrUpdate(imagemodel);
                    context.Response.Write(data);

                }

                //删除图片
                if (oper == "deleteimage")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);

                    var data = DirectSellJsonData.Deleteimage(comid, id);
                    context.Response.Write(data);

                }

                //删除图片
                if (oper == "deletechannelimage")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var channelcompanyid = context.Request["channelcompanyid"].ConvertTo<int>(0);

                    var data = DirectSellJsonData.Deletechannelimage(comid, id, channelcompanyid);
                    context.Response.Write(data);

                }

                //删除图片
                if (oper == "updwonstate")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var channelcompanyid = context.Request["channelcompanyid"].ConvertTo<int>(0);

                    var data = DirectSellJsonData.UpDownState(comid, id, channelcompanyid);
                    context.Response.Write(data);

                }


                if (oper == "getmenulist")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var usetype = context.Request["usetype"].ConvertTo<int>(0);
                    var menuindex = context.Request["menuindex"].ConvertTo<int>(0);

                    var data = DirectSellJsonData.Getmenulist(comid, pageindex, pagesize, usetype, menuindex);
                    context.Response.Write(data);
                }

                if (oper == "getconsultantlist")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var channelid = context.Request["channelid"].ConvertTo<int>(0);


                    var data = DirectSellJsonData.Getconsultantlist(comid, pageindex, pagesize);
                    context.Response.Write(data);
                }

                if (oper == "getmenubyid")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var data = DirectSellJsonData.GetMenuByComid(comid, id);
                    context.Response.Write(data);
                }

                if (oper == "getconsultantbyid")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var data = DirectSellJsonData.GetConsultantByComid(comid, id);
                    context.Response.Write(data);
                }

                if (oper == "getchannelprolist")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);//渠道ID
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var data = DirectSellJsonData.GetChannelProList(comid, id);
                    context.Response.Write(data);
                }

                //修改栏目
                if (oper == "editmenu")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var typeid = context.Request["typeid"].ConvertTo<int>(0);
                    var Imgurl = context.Request["imgurl"].ConvertTo<int>(0);
                    var Linkurl = context.Request["Linkurl"].ConvertTo<string>("");
                    var Linktype = context.Request["Linktype"].ConvertTo<int>(0);
                    var name = context.Request["name"].ConvertTo<string>("");
                    var fonticon = context.Request["fonticon"].ConvertTo<string>("");
                    var usetype = context.Request["usetype"].ConvertTo<int>(0);
                    var usestyle = context.Request["usestyle"].ConvertTo<int>(0);
                    var projectlist = context.Request["projectlist"].ConvertTo<int>(0);
                    var prolist = context.Request["prolist"].ConvertTo<string>("");
                    var Menutype = context.Request["Menutype"].ConvertTo<int>(0);
                    var menuindex = context.Request["menuindex"].ConvertTo<int>(0);
                    var menuviewtype = context.Request["menuviewtype"].ConvertTo<int>(0);
                    
                    B2b_company_menu menumodel = new B2b_company_menu()
                    {
                        Id = id,
                        Com_id = comid,
                        Imgurl = Imgurl,
                        Linkurl = Linkurl,
                        Linktype = Linktype,
                        Name = name,
                        Fonticon = fonticon,
                        Usestyle = usestyle,
                        Usetype = usetype,
                        Projectlist = projectlist,
                        Prolist = prolist,
                        Menutype = Menutype,
                        menuindex = menuindex,
                        menuviewtype = menuviewtype
                    };
                    var data = DirectSellJsonData.InsertOrUpdate(menumodel);
                    context.Response.Write(data);

                }

                if (oper == "MenuSort")
                {
                    string ids = context.Request["ids"].ConvertTo<string>("");

                    string data = DirectSellJsonData.MenuSort(ids);
                    context.Response.Write(data);
                }

                //修改底部按钮
                if (oper == "editbutton")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var linkurl = context.Request["linkurl"].ConvertTo<string>("");
                    var name = context.Request["name"].ConvertTo<string>("");
                    var linkurlname = context.Request["linkurlname"].ConvertTo<string>("");
                    var sort = context.Request["sort"].ConvertTo<int>(0);
                    var linktype = context.Request["linktype"].ConvertTo<int>(0);

                    B2b_company_Button buttonmodel = new B2b_company_Button()
                    {
                        Id = id,
                        Comid = comid,
                        Linkurl = linkurl,
                        Name = name,
                        Linkurlname = linkurlname,
                        Sort = sort,
                        Linktype = linktype
                    };
                    var data = DirectSellJsonData.ButtonInsertOrUpdate(buttonmodel);
                    context.Response.Write(data);

                }

                //删除底部按钮
                if (oper == "deletebutton")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);

                    var data = DirectSellJsonData.DeleteButton(comid, id);
                    context.Response.Write(data);

                }

                //获取指定底部按钮
                if (oper == "getbuttonbyid")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);

                    var data = DirectSellJsonData.GetButtonByComid(comid, id);
                    context.Response.Write(data);

                }

                //获取指定底部按钮列表
                if (oper == "getbuttonlist")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var linktype = context.Request["linktype"].ConvertTo<int>(10);
                    var data = DirectSellJsonData.GetButtonlist(comid, pageindex, pagesize, linktype);
                    context.Response.Write(data);
                }


                //修改导航
                if (oper == "editconsultant")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var typeid = context.Request["typeid"].ConvertTo<int>(0);
                    var Imgurl = context.Request["imgurl"].ConvertTo<int>(0);
                    var Linkurl = context.Request["Linkurl"].ConvertTo<string>("");
                    var Linktype = context.Request["Linktype"].ConvertTo<int>(0);//产品分类
                    var name = context.Request["name"].ConvertTo<string>("");
                    var fonticon = context.Request["fonticon"].ConvertTo<string>("");
                    var outdata = context.Request["outdata"].ConvertTo<int>(0);
                    var proid = context.Request["proid"].ConvertTo<string>("");
                    var MaterialId = context.Request["MaterialId"].ConvertTo<string>("");
                    var channelid = context.Request["channelid"].ConvertTo<int>(0);

                    //如果选择微信文章则 只记录文章选择的数据
                    if (outdata == 2) {
                        proid = MaterialId;
                    }



                    B2b_company_menu menumodel = new B2b_company_menu()
                    {
                        Id = id,
                        Com_id = comid,
                        Imgurl = Imgurl,
                        Linkurl = Linkurl,
                        Linktype = Linktype,
                        Name = name,
                        Fonticon = fonticon,
                        Outdata = outdata,
                        Prolist = proid,
                        Channelid = channelid,
                    };
                    var data = DirectSellJsonData.ConsultantInsertOrUpdate(menumodel);
                    context.Response.Write(data);

                }
                if (oper == "menuSort")
                {
                    string ids = context.Request["ids"].ConvertTo<string>("");

                    string data = DirectSellJsonData.MenuSort(ids);
                    context.Response.Write(data);
                }

                if (oper == "SortConsultant")
                {
                    string ids = context.Request["ids"].ConvertTo<string>("");

                    string data = DirectSellJsonData.SortConsultant(ids);
                    context.Response.Write(data);
                }
                //删除菜单
                if (oper == "deletemenu")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);

                    var data = DirectSellJsonData.Deletemenu(comid, id);
                    context.Response.Write(data);

                }

                //删除菜单
                if (oper == "deleteConsultant")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);

                    var data = DirectSellJsonData.DeleteConsultant(comid, id);
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