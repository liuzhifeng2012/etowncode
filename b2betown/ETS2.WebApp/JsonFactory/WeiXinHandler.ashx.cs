using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Framework;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS.JsonFactory;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.Common.Business;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Member.Service.MemberService.Data;

namespace ETS2.WebApp.JsonFactory
{
    /// <summary>
    /// WeiXinHandler 的摘要说明
    /// </summary>
    public class WeiXinHandler : IHttpHandler
    {
        private static object lockobj = new object();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");

            string oper = context.Request["oper"].ConvertTo<string>("");
            if (oper != "")
            {
                //按微信支付规则生成原生支付二维码
                if (oper == "getNativePayQrcode")
                {
                    int proid = context.Request["proid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    //链接中带固定参数productid（可定义为产品标识pid 或订单号oid ）
                    string paramtype = context.Request["paramtype"].ConvertTo<string>("");

                    string data = WeiXinJsonData.getNativePayQrcode(proid,comid,paramtype);
                    context.Response.Write(data);
                }
                if (oper == "Getzixunlog")
                {
                    string userweixin = context.Request["userweixin"].ConvertTo<string>("");
                    string guwenweixin = context.Request["guwenweixin"].ConvertTo<string>("");
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    string key = context.Request["key"].ConvertTo<string>("");

                    if (key == "0") {
                        context.Response.Write("{\"type\":\"100\",\"msg\":\"\",\"totalCount\":\"0\"}");
                        return;
                    }

                    string data = WeiXinJsonData.Getzixunlog(comid,pageindex,pagesize,userweixin, guwenweixin,key);
                    context.Response.Write(data);
                }
                if (oper == "createtempwxqrcode")
                { 
                    int MaterialId = context.Request["MaterialId"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    int tempqrcodeid = MaterialId + 1000000;

                    string data = WeiXinJsonData.Createtempwxqrcode(tempqrcodeid, comid);
                    context.Response.Write(data);
                }
                if (oper == "getwxvoicemediaid")
                {
                    string openid = context.Request["openid"].ConvertTo<string>("");
                    int uplogid = context.Request["uplogid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = WeiXinJsonData.Getwxvoicemediaid(openid, uplogid,comid);
                    context.Response.Write(data);
                }
                if (oper == "getwxdownvoicelist")
                { 
                    string openid = context.Request["openid"].ConvertTo<string>(""); 
                    int clientuptypemark = context.Request["clientuptypemark"].ConvertTo<int>(0);
                    int materialid = context.Request["materialid"].ConvertTo<int>(0);

                    string data = WeiXinJsonData.Getwxdownvoicelist(openid,clientuptypemark,materialid);
                    context.Response.Write(data);
                }
                if (oper == "wxdownvoice")
                {
                    string mediaid = context.Request["mediaid"].ConvertTo<string>("");
                    string openid = context.Request["openid"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int clientuptypemark = context.Request["clientuptypemark"].ConvertTo<int>(0);
                    string remarks = context.Request["remarks"].ConvertTo<string>("");
                    int materialid = context.Request["materialid"].ConvertTo<int>(0);

                    string data = WeiXinJsonData.Wxdownvoice(mediaid, openid, comid, clientuptypemark, remarks, materialid);
                    context.Response.Write(data);
                } 
                if (oper == "Getmicromallimgbycomid")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = WeiXinJsonData.Getmicromallimgbycomid(comid);
                    context.Response.Write(data);
                }
                if (oper == "Getchannelwxqrcodebychannelid")
                {
                    int channelid = context.Request["channelid"].ConvertTo<int>(0);

                    string data = WeiXinJsonData.Getchannelwxqrcodebychannelid(channelid);
                    context.Response.Write(data);
                }
                //得到公司预订产品验证二维码
                if (oper == "GetReserveproVerifywxqrcode")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int qrcodeviewtype = context.Request["qrcodeviewtype"].ConvertTo<int>(0);

                    string data = WeiXinJsonData.GetReserveproVerifywxqrcode(comid,qrcodeviewtype);
                    context.Response.Write(data);
                }
                if (oper == "editchannelwxqrcode")
                {

                    int id = context.Request["id"].ConvertTo<int>(0);
                    string qrcodename = context.Request["qrcodename"].ConvertTo<string>();
                    int qrcodeviewtype = context.Request["viewtype"].ConvertTo<int>(0);
                    int projectid = context.Request["projectid"].ConvertTo<int>(0);
                    int productid = context.Request["productid"].ConvertTo<int>(0);
                    int materialtype = context.Request["materialtype"].ConvertTo<int>(0);
                    int materialid = context.Request["materialid"].ConvertTo<int>(0);
                    int channelcompanyid = context.Request["channelcompanyid"].ConvertTo<int>(0);
                    int viewchannelcompanyid = context.Request["viewchannelcompanyid"].ConvertTo<int>(0);
                    int channelid = context.Request["channelid"].ConvertTo<int>(0);
                    int promoteact = context.Request["promoteact"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int onlinestatus = context.Request["onlinestatus"].ConvertTo<int>(0);
                   

                    int micromallimgid = context.Request["micromallimgid"].ConvertTo<int>(0);
                    //向商户中加入微商城图片
                    int insmicroimg = new B2bCompanyData().Insmicroimg(comid, micromallimgid);

                    int sel_choujiangact = context.Request["sel_choujiangact"].ConvertTo<int>(0);
                    if (id > 0)
                    {
                        WxSubscribeSource m = new WxSubscribeSourceData().GetWXSourceById(id);

                        m.Title = qrcodename;
                        m.qrcodeviewtype = qrcodeviewtype;
                        m.projectid = projectid;
                        m.Productid = productid;
                        m.wxmaterialtypeid = materialtype;
                        m.Wxmaterialid = materialid;
                        m.Channelcompanyid = channelcompanyid;
                        m.viewchannelcompanyid = viewchannelcompanyid;
                        m.Channelid = channelid;
                        m.Activityid = promoteact;
                        m.Comid = comid;
                        m.Onlinestatus = onlinestatus == 1 ? true : false;
                        m.Createtime = DateTime.Now;
                        m.Qrcodeurl = m.Qrcodeurl;
                        m.Sourcetype = m.Sourcetype;
                        m.Ticket = m.Ticket;
                        m.choujiangactid = sel_choujiangact;


                        string data = WeiXinJsonData.EditChannelWxQrcode(m);
                        context.Response.Write(data);
                    }
                    else
                    {
                        WxSubscribeSource m = new WxSubscribeSource
                        {
                            Id = id,
                            Title = qrcodename,
                            qrcodeviewtype = qrcodeviewtype,
                            projectid = projectid,
                            Productid = productid,
                            wxmaterialtypeid = materialtype,
                            Wxmaterialid = materialid,
                            Channelcompanyid = channelcompanyid,
                            viewchannelcompanyid = viewchannelcompanyid,
                            Channelid = channelid,
                            Activityid = promoteact,
                            Comid = comid,
                            Onlinestatus = onlinestatus == 1 ? true : false,
                            Createtime = DateTime.Now,
                            Qrcodeurl = "",
                            Sourcetype = 0,
                            Ticket = "",
                            choujiangactid=sel_choujiangact
                        };

                        string data = WeiXinJsonData.EditChannelWxQrcode(m);
                        context.Response.Write(data);
                    }


                }
                if (oper == "EditWxMaterial")
                {
                    // content: txtcontent, articleurl: txturl, keywords: txtkeyword
                    int materialid = context.Request["materialid"].ConvertTo<int>(0);
                    string title = context.Request["title"].ConvertTo<string>("");
                    string author = context.Request["author"].ConvertTo<string>("");
                    string imgurl = context.Request["imgurl"].ConvertTo<string>("");
                    string summary = context.Request["summary"].ConvertTo<string>("");
                    string content = context.Request["content"].ConvertTo<string>("");
                    string articleurl = context.Request["articleurl"].ConvertTo<string>("");
                    string keywords = context.Request["keywords"].ConvertTo<string>("");
                    int applystate = context.Request["applystate"].ConvertTo<int>(1);

                    string txtphone = context.Request["phone"].ConvertTo<string>("");
                    decimal price = context.Request["price"].ConvertTo<decimal>(0);
                    int promotetype = context.Request["promotetype"].ConvertTo<int>(1);

                    var Actstar = context.Request["Actstar"].ConvertTo<DateTime>();
                    var Actend = context.Request["Actend"].ConvertTo<DateTime>();

                    var periodicalid = context.Request["periodicalid"].ConvertTo<int>(0);

                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string authorpayurl = context.Request["authorpayurl"].ConvertTo<string>("");

                    WxMaterial material = new WxMaterial
                    {
                        MaterialId = materialid,
                        Title = title,
                        Author = author,
                        Article = content,
                        Articleurl = articleurl,
                        Imgpath = imgurl,
                        Summary = summary,
                        Operatime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        Keyword = keywords,
                        Applystate = applystate,
                        Phone = txtphone,
                        Price = price,
                        SalePromoteTypeid = promotetype,
                        Staticdate = Actstar,
                        Enddate = Actend,
                        Periodicalid = periodicalid,
                        Comid = comid,
                        Authorpayurl = authorpayurl
                    };

                    string data = WeiXinJsonData.EditMaterial(material);
                    context.Response.Write(data);
                }
                if (oper == "Addperiod")//添加下一期
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int Wxsaletypeid = context.Request["promotetypeid"].ConvertTo<int>(0);
                    int percal = context.Request["periodical"].ConvertTo<int>(0);

                    //periodical qi = new periodical
                    //{
                    //    Id = 0,
                    //    Comid = comid,
                    //    Percal = percal + 1,
                    //    Perinfo = "",
                    //    Peryear = DateTime.Now.Year,
                    //    Uptime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    //    Wxsaletypeid1 = Wxsaletypeid
                    //};


                    string data = WeiXinJsonData.AddNextPeriod(comid, Wxsaletypeid, percal);
                    context.Response.Write(data);
                    //string data = WeiXinJsonData.Editperiod(qi);
                    //context.Response.Write(data);
                }
                if (oper == "IsCanAddperiod")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int Wxsaletypeid = context.Request["promotetypeid"].ConvertTo<int>(0);
                    int percal = context.Request["periodical"].ConvertTo<int>(0);
                    string data = WeiXinJsonData.IsCanAddperiod(comid, Wxsaletypeid, percal);
                    context.Response.Write(data);
                }
                if (oper == "Selpercal")
                {
                    int wxtype = context.Request["wxtype"].ConvertTo<int>(0);
                    int percal = context.Request["percal"].ConvertTo<int>(0);
                    string data = WeiXinJsonData.Selpercal(percal, wxtype);
                    context.Response.Write(data);
                }
                if (oper == "selecttypeid")//菜单类型查期
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int Wxsaletypeid = context.Request["promotetypeid"].ConvertTo<int>(0);
                    string data = WeiXinJsonData.selecttype(Wxsaletypeid, comid);
                    context.Response.Write(data);
                }
                if (oper == "GetWxMaterial")
                {
                    int materialid = context.Request["materialid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = WeiXinJsonData.GetWxMaterial(comid, materialid);
                    context.Response.Write(data);
                }
                //期列表
                if (oper == "periodicallist")
                {
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    int applystate = context.Request["applystate"].ConvertTo<int>(1);
                    int promotetypeid = context.Request["promotetypeid"].ConvertTo<int>(1);

                    int comid = context.Request["comid"].ConvertTo<int>(0);


                    string data = WeiXinJsonData.periodicalList(pageindex, pagesize, applystate, promotetypeid, comid);
                    context.Response.Write(data);
                }
                

                //每期查询列表
                if (oper == "periodicaltypelist")
                {
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    int applystate = context.Request["applystate"].ConvertTo<int>(1);
                    int promotetypeid = context.Request["promotetypeid"].ConvertTo<int>(1);
                    int type = context.Request["type"].ConvertTo<int>(1);

                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = WeiXinJsonData.periodicaltypelist(pageindex, pagesize, applystate, promotetypeid, type, comid);
                    context.Response.Write(data);
                }

                //每期查询列表
                if (oper == "periodicaltypelastlist")
                {
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    int applystate = context.Request["applystate"].ConvertTo<int>(1);
                    int promotetypeid = context.Request["promotetypeid"].ConvertTo<int>(1);
                    int type = context.Request["type"].ConvertTo<int>(1);

                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = WeiXinJsonData.periodicaltypelastlist(pageindex, pagesize, applystate, promotetypeid, type, comid);
                    context.Response.Write(data);
                }


                if (oper == "pagelist")
                {
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    int applystate = context.Request["applystate"].ConvertTo<int>(1);
                    string key = context.Request["key"].ConvertTo<string>("");
                    int promotetypeid = context.Request["promotetypeid"].ConvertTo<int>(1000000);

                    int comid = context.Request["comid"].ConvertTo<int>(0);


                    string data = WeiXinJsonData.WxMaterialPageList(comid, pageindex, pagesize, applystate, promotetypeid, key);
                    context.Response.Write(data);
                }

                if (oper == "articlepagelist")
                {
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    int applystate = context.Request["applystate"].ConvertTo<int>(1);
                    int id = context.Request["id"].ConvertTo<int>(0);
                    int promotetypeid = context.Request["promotetypeid"].ConvertTo<int>(1000000);
                    int comid = context.Request["comid"].ConvertTo<int>(0);



                    string data = WeiXinJsonData.ArticleWxMaterialPageList(comid, pageindex, pagesize, applystate, promotetypeid, id);
                    context.Response.Write(data);
                }
                if (oper == "TopArticleWxMaterialPageList")
                {
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    int applystate = context.Request["applystate"].ConvertTo<int>(1);
                    int id = context.Request["id"].ConvertTo<int>(0);
                    int promotetypeid = context.Request["promotetypeid"].ConvertTo<int>(1000000);
                    int comid = context.Request["comid"].ConvertTo<int>(0);



                    string data = WeiXinJsonData.TopArticleWxMaterialPageList(comid, pageindex, pagesize, applystate, promotetypeid, id);
                    context.Response.Write(data);
                }




                if (oper == "shoppagelist")
                {
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    int applystate = context.Request["applystate"].ConvertTo<int>(10);
                    string key = context.Request["key"].ConvertTo<string>("");
                    int promotetypeid = context.Request["promotetypeid"].ConvertTo<int>(1000000);
                    int menuid = context.Request["menuid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);


                    string data = WeiXinJsonData.ShopWxMaterialPageList(comid, pageindex, pagesize, applystate, menuid, promotetypeid, key);
                    context.Response.Write(data);
                }




                if (oper == "forwardingpagelist")
                {
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    int applystate = context.Request["applystate"].ConvertTo<int>(1);
                    int promotetypeid = context.Request["promotetypeid"].ConvertTo<int>(1000000);
                    int wxid = context.Request["wxid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);


                    string data = WeiXinJsonData.ForwardingWxMaterialPageList(comid, pageindex, pagesize, applystate, promotetypeid, wxid);
                    context.Response.Write(data);

                }

                if (oper == "frowardingset")
                {
                    int materialid = context.Request["materialid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string data = WeiXinJsonData.FrowardingSet(materialid, comid);
                    context.Response.Write(data);
                }
                if (oper == "frowardingsetcannel")
                {
                    int materialid = context.Request["materialid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string data = WeiXinJsonData.FrowardingSetCannel(materialid, comid);
                    context.Response.Write(data);
                }
                if (oper == "delmaterial")
                {
                    int materialid = context.Request["materialid"].ConvertTo<int>(0);
                    string data = WeiXinJsonData.DelWxMaterial(materialid);
                    context.Response.Write(data);
                }
                if (oper == "GetMenuDetail")
                {
                    int menuid = context.Request["menuid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = WeiXinJsonData.GetMenuDetail(menuid, comid);
                    context.Response.Write(data);
                }
                if (oper == "GetFristMenuList")
                {
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    int applystate = context.Request["applystate"].ConvertTo<int>(1);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = WeiXinJsonData.GetFristMenuList(pageindex, pagesize, applystate, comid);
                    context.Response.Write(data);
                }

                if (oper == "GetSecondMenuList")
                {
                    var fatherid = context.Request["fatherid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = WeiXinJsonData.GetSecondMenuList(fatherid, comid);
                    context.Response.Write(data);
                }


                if (oper == "EditMenuDetail")
                {

                    int menuid = context.Request["menuid"].ConvertTo<int>(0);
                    string name = context.Request["name"].ConvertTo<string>("");
                    string instruction = context.Request["instruction"].ConvertTo<string>("");
                    int oprtype = context.Request["oprtype"].ConvertTo<int>(0);
                    string linkurl = context.Request["linkurl"].ConvertTo<string>("");
                    string txtwxtext = context.Request["txtwxtext"].ConvertTo<string>("");
                    int textsalepromotetypeid = context.Request["textsalepromotetypeid"].ConvertTo<int>(0);
                    int fatherid = context.Request["fatherid"].ConvertTo<int>(0);

                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string keyy = context.Request["keyy"].ConvertTo<string>("");

                    int product_class = context.Request["product_class"].ConvertTo<int>(0);
                    int pictexttype = context.Request["pictexttype"].ConvertTo<int>(1);
                    WxMenu wxmenu = new WxMenu()
                    {
                        Menuid = menuid,
                        Name = name,
                        Instruction = instruction,
                        Operationtypeid = oprtype,
                        Linkurl = linkurl,
                        Wxanswertext = txtwxtext,
                        SalePromoteTypeid = textsalepromotetypeid,
                        Fathermenuid = fatherid,
                        Comid = comid,
                        Product_class = product_class,
                        Keyy = keyy,
                        pictexttype = pictexttype
                    };



                    string data = WeiXinJsonData.EditWxmenu(wxmenu);
                    context.Response.Write(data);
                }
                if (oper == "delwxmenu")
                {
                    int wxmenuid = context.Request["wxmenuid"].ConvertTo<int>(0);
                    string data = WeiXinJsonData.Delwxmenu(wxmenuid);
                    context.Response.Write(data);
                }
                if (oper == "createwxmenu")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string data = WeiXinJsonData.CreateWxMenu(comid);
                    context.Response.Write(data);
                }
                if (oper == "DelChildrenMenu")
                {
                    int fathermenuid = context.Request["fathermenuid"].ConvertTo<int>(0);

                    string data = WeiXinJsonData.DelChildrenMenu(fathermenuid);
                    context.Response.Write(data);
                }

                if (oper == "GetMenuList")
                {
                    int fathermenuid = context.Request["fathermenuid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = WeiXinJsonData.GetMenuList(fathermenuid, comid);
                    context.Response.Write(data);
                }
                if (oper == "Menusort")
                {
                    string menuids = context.Request["menuids"].ConvertTo<string>("");

                    string data = WeiXinJsonData.MenuSort(menuids);
                    context.Response.Write(data);
                }
                if (oper == "GetMaterialByPromoteType")
                {
                    int promotetypeid = context.Request["promotetypeid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);


                    string data = WeiXinJsonData.GetMaterialByPromoteType(comid, promotetypeid);
                    context.Response.Write(data);
                }
                if (oper == "MaterialSort")
                {
                    string materialids = context.Request["materialids"].ConvertTo<string>("");


                    string data = WeiXinJsonData.MaterialSort(materialids);
                    context.Response.Write(data);
                }
                if (oper == "wxmaterialtypepagelist")
                {
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    int comid = context.Request["comid"].ConvertTo<int>(0);


                    string data = WeiXinJsonData.Wxmaterialtypepagelist(pageindex, pagesize, comid);
                    context.Response.Write(data);
                }
                if (oper == "getmaterialtype")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string data = WeiXinJsonData.GetMaterialType(id, comid);
                    context.Response.Write(data);
                }
                if (oper == "editmaterialtype")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var typename = context.Request["typename"].ConvertTo<string>("");
                    var typeclass = context.Request["typeclass"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    bool isshowpast = context.Request["isshowpast"].ConvertTo<bool>(true);

                    string data = WeiXinJsonData.EditMaterialType(id, typename, typeclass, comid, isshowpast);
                    context.Response.Write(data);
                }
                if (oper == "GetAllWxMaterialType")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string data = WeiXinJsonData.GetAllWxMaterialType(comid);
                    context.Response.Write(data);
                }
                if (oper == "getwxbasic")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = WeiXinJsonData.GetWxBasicByComId(comid);
                    context.Response.Write(data);
                }
                if (oper == "GetWxQi")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int salepromotetypeid = context.Request["salepromotetype"].ConvertTo<int>(0);

                    string data = WeiXinJsonData.GetWxQi(comid, salepromotetypeid);
                    context.Response.Write(data);
                }
                if (oper == "editwxbasic")
                {

                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var wxbasicid = context.Request["wxbasicid"].ConvertTo<int>(0);
                    var domain = context.Request["domain"].ConvertTo<string>("");
                    var url = context.Request["url"].ConvertTo<string>("");
                    var token = context.Request["token"].ConvertTo<string>("");
                    var appid = context.Request["appid"].ConvertTo<string>("");
                    var appsecret = context.Request["appsecret"].ConvertTo<string>("");
                    var attentionautoreply = context.Request["attentionautoreply"].ConvertTo<string>("");
                    var leavemsgautoreply = context.Request["leavemsgautoreply"].ConvertTo<string>("");
                    var whethervertify = context.Request["whethervertify"].ConvertTo<bool>(false);

                    WeiXinBasic basic = new WeiXinBasic
                    {
                        Id = wxbasicid,
                        Comid = comid,
                        Domain = domain,
                        Url = url,
                        Token = token,
                        AppId = appid,
                        AppSecret = appsecret,
                        Attentionautoreply = attentionautoreply,
                        Leavemsgautoreply = leavemsgautoreply,
                        Whethervertify = whethervertify
                    };

                    string data = WeiXinJsonData.Editwxbasic(basic);
                    context.Response.Write(data);
                }
                if (oper == "GetMemberShipCardMaterial")
                {
                    int materialid = context.Request["materialid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = WeiXinJsonData.GetMemberShipCardMaterial(comid, materialid);
                    context.Response.Write(data);
                }
                if (oper == "EditMemberShipCardMaterial")
                {
                    int materialid = context.Request["materialid"].ConvertTo<int>(0);
                    string title = context.Request["title"].ConvertTo<string>("");

                    string imgurl = context.Request["imgurl"].ConvertTo<string>("");
                    string summary = context.Request["summary"].ConvertTo<string>("");
                    string content = context.Request["content"].ConvertTo<string>("");


                    bool applystate = context.Request["applystate"].ConvertTo<bool>(true);

                    string txtphone = context.Request["phone"].ConvertTo<string>("");
                    decimal price = context.Request["price"].ConvertTo<decimal>(0);

                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    MemberShipCardMaterial material = new MemberShipCardMaterial
                    {
                        MaterialId = materialid,
                        Title = title,

                        Article = content,

                        Imgpath = imgurl,
                        Summary = summary,


                        Applystate = applystate,
                        Phone = txtphone,
                        Price = price,
                        Comid = comid

                    };

                    string data = WeiXinJsonData.EditMemberShipCardMaterial(material);
                    context.Response.Write(data);
                }

                if (oper == "Membershipcardpagelist")
                {
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    bool applystate = context.Request["applystate"].ConvertTo<bool>(true);


                    int comid = context.Request["comid"].ConvertTo<int>(0);


                    string data = WeiXinJsonData.Membershipcardpagelist(comid, pageindex, pagesize, applystate);
                    context.Response.Write(data);
                }
                if (oper == "AllMembershipcardpagelist")
                {
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);

                    int comid = context.Request["comid"].ConvertTo<int>(0);


                    string data = WeiXinJsonData.AllMembershipcardpagelist(comid, pageindex, pagesize);
                    context.Response.Write(data);
                }
                if (oper == "delMemberShipCardmaterial")
                {
                    int materialid = context.Request["materialid"].ConvertTo<int>(0);
                    string data = WeiXinJsonData.DelWxMaterial(materialid);
                    context.Response.Write(data);
                }
                if (oper == "MemberShipCardMaterialSort")
                {
                    string materialids = context.Request["materialids"].ConvertTo<string>("");


                    string data = WeiXinJsonData.MemberShipCardMaterialSort(materialids);
                    context.Response.Write(data);
                }
                if (oper == "GetMenshiLink")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string redirect_uri = context.Request["redirect_uri"].ConvertTo<string>("");

                    string data = WeiXinJsonData.GetFollowOpenLink(comid, redirect_uri);
                    context.Response.Write(data);
                }
                if (oper == "GetMenshiLinkAboutPay")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string redirect_uri = context.Request["redirect_uri"].ConvertTo<string>("");

                    string data = WeiXinJsonData.GetFollowOpenLinkAboutPay(comid, redirect_uri);
                    context.Response.Write(data);
                }
                if (oper == "GetFollowOpenLinkTest")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);


                    string data = WeiXinJsonData.GetFollowOpenLinkTest(comid);
                    context.Response.Write(data);
                }
                if (oper == "wxsendmsglistbycomid")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);

                    int userid = context.Request["userid"].ConvertTo<int>(0);

                    string data = WeiXinJsonData.GetWxSendMsgListByComid(userid, comid, pageindex, pagesize);

                    context.Response.Write(data);
                }
                if (oper == "wxsendmsglistbyfromuser")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string fromusername = context.Request["fromusername"].ConvertTo<string>("");
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);

                    string data = WeiXinJsonData.GetWxSendMsgListByFromUser(comid, fromusername, pageindex, pagesize);

                    context.Response.Write(data);
                }
                if (oper == "wxsendmsglistbytop5")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    var pageindex = 1;
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(5);

                    string data = WeiXinJsonData.GetWxSendMsgListByTop5(comid, pageindex, pagesize);
                    context.Response.Write(data);
                }
                if (oper == "wxguwentop5")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(101);
                    var pageindex = 1;
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);

                    string data = WeiXinJsonData.GetWxSendMsgListByTop5(comid, pageindex, pagesize);
                    context.Response.Write("callback(" + data + ")");
                }
                if (oper == "sendwxmsg")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string fromusername = context.Request["fromusername"].ConvertTo<string>("");
                    int type = context.Request["type"].ConvertTo<int>(0);
                    string img = context.Request["img"].ConvertTo<string>("");
                    string txt = context.Request["txt"].ConvertTo<string>("");



                    string data = WeiXinJsonData.SendWxMsg(comid, fromusername, type, img, txt);

                    context.Response.Write(data);
                }

                if (oper == "guwenwxsendmsg")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string fromusername = context.Request["userweixin"].ConvertTo<string>("");
                    string guwenweixin = context.Request["guwenweixin"].ConvertTo<string>("");
                    int type = context.Request["type"].ConvertTo<int>(0);
                    string img = context.Request["img"].ConvertTo<string>("");
                    string txt = context.Request["txt"].ConvertTo<string>("");
                    string mediaid = context.Request["mediaid"].ConvertTo<string>("");
                    
                    //查询顾问是否由此权限
                    if (guwenweixin =="")
                    {
                        context.Response.Write("{\"type\":\"1\",\"msg\":\"顾问信息错误\"}");
                        return;
                    }

                    var backchannel = new MemberChannelcompanyData().jianchaguwenbyweixin(comid, guwenweixin);

                    if (backchannel == 0)
                    {
                        context.Response.Write("{\"type\":\"1\",\"msg\":\"顾问权限错误\"}");
                        return;
                    }

                    string data = WeiXinJsonData.GwToUserSendWxMsg(comid, guwenweixin, fromusername, type, img, txt, mediaid);

                    context.Response.Write(data);
                }


                if (oper == "WXSourcelist")//得到本公司全部的 活动推广/门市推广 列表
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var wxsourcetype = context.Request["wxsourcetype"].ConvertTo<int>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);


                    string data = WeiXinJsonData.GetWXSourcelist(comid, wxsourcetype, pageindex, pagesize);
                    context.Response.Write(data);
                }
                if (oper == "GetWXSourcelist2")//得到本公司全部的 活动推广/门市推广 列表
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var wxsourcetype = context.Request["wxsourcetype"].ConvertTo<string>("1,2,3");
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);

                    string onlinestatus = context.Request["onlinestatus"].ConvertTo<string>("1");//默认显示使用的
                    string data = WeiXinJsonData.GetWXSourcelist2(comid, wxsourcetype, pageindex, pagesize, onlinestatus);
                    context.Response.Write(data);
                }
                if (oper == "SeledWXSourcelist")//得到本公司选中的 活动推广/门市推广 列表
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var wxsourcetype = context.Request["wxsourcetype"].ConvertTo<int>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);

                    string onlinestatus = context.Request["onlinestatus"].ConvertTo<string>("1");//默认显示使用的

                    string data = WeiXinJsonData.SeledWXSourcelist(comid, wxsourcetype, pageindex, pagesize, onlinestatus);
                    context.Response.Write(data);
                }
                if (oper == "HandleQrCodeCreateStatus")//对 活动推广/门市推广 的成员是否生成自己的带参二维码
                {
                    int channelcompanyid = context.Request["channelcompanyid"].ConvertTo<int>(0);
                    int activityid = context.Request["activityid"].ConvertTo<int>(0);
                    string checkstatus = context.Request["checkstatus"].ConvertTo<string>("true");

                    string data = WeiXinJsonData.HandleQrCodeCreateStatus(channelcompanyid, activityid, checkstatus);
                    context.Response.Write(data);
                }
                if (oper == "GetWxQrCodeUrl")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int wxsubscribesourceid = context.Request["wxsubscribesourceid"].ConvertTo<int>();
                    string data = WeiXinJsonData.GetWxQrCodeUrl(comid, wxsubscribesourceid);
                    context.Response.Write(data);
                }
                if (oper == "GetWxSubscribeList")//得到微信关注用户列表
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var subscribesourceid = context.Request["subscribesourceid"].ConvertTo<int>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);


                    string data = WeiXinJsonData.GetWxSubscribeList(comid, subscribesourceid, pageindex, pagesize);
                    context.Response.Write(data);
                }
                if (oper == "GetPromoteActivity")//获得推广活动列表
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    string data = WeiXinJsonData.GetPromotionActList(comid);
                    context.Response.Write(data);
                }
                if (oper == "GetPromoteChannelCompany")//获得推广渠道公司列表
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    string data = WeiXinJsonData.GetPromoteChannelCompany(comid);
                    context.Response.Write(data);
                }
                if (oper == "editwxqrcode")//编辑非渠道二维码
                {
                    int qrcodeid = context.Request["qrcodeid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string qrcodename = context.Request["qrcodename"].ConvertTo<string>(); 
                    int promotechannelcompany = context.Request["promotechannelcompany"].ConvertTo<int>(0);
                    int channelid = context.Request["channelid"].ConvertTo<int>(0);
                     
                    int promoteact = context.Request["promoteact"].ConvertTo<int>(0);
                    int productid = context.Request["productid"].ConvertTo<int>(0);
                    int MaterialId = context.Request["MaterialId"].ConvertTo<int>(0);

                    int onlinestatus = context.Request["onlinestatus"].ConvertTo<int>(1);

                    string data = WeiXinJsonData.EditWxQrcode(qrcodeid, comid, qrcodename, promoteact, promotechannelcompany, channelid, onlinestatus, productid, MaterialId);
                    context.Response.Write(data);

                }
                if (oper == "getwxqrcode")
                {
                    int qrcodeid = context.Request["qrcodeid"].ConvertTo<int>(0);
                    string data = WeiXinJsonData.Getwxqrcode(qrcodeid);
                    context.Response.Write(data);
                }
                if (oper == "WxWhetherVertify")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = WeiXinJsonData.WxWhetherVertify(comid);
                    context.Response.Write(data);
                }
                if (oper == "adjustqrcodestatus")
                {
                    int sourceid = context.Request["sourceid"].ConvertTo<int>(0);
                    int onlinestatus = context.Request["onlinestatus"].ConvertTo<string>("false") == "true" ? 1 : 0;

                    string data = WeiXinJsonData.AdjustQrcodeStatus(sourceid, onlinestatus);
                    context.Response.Write(data);

                }
                if (oper == "GetChannelList")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int channeltype = context.Request["channeltype"].ConvertTo<int>(0);

                    string data = WeiXinJsonData.GetChannelList(comid, channeltype);
                    context.Response.Write(data);
                }

                if (oper == "GetChannels")
                {

                    int channelcomid = context.Request["channelcomid"].ConvertTo<int>(0);
                    string runstate = context.Request["runstate"].ConvertTo<string>("1");
                    string whetherdefaultchannel = context.Request["whetherdefaultchannel"].ConvertTo<string>("0");

                    string data = WeiXinJsonData.GetChannelList(channelcomid, runstate, whetherdefaultchannel);
                    context.Response.Write(data);
                }
                if (oper == "GetChannelCompanyList")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string issuetype = context.Request["issuetype"].ConvertTo<string>("0");
                    string companystate = context.Request["companystate"].ConvertTo<string>("1");
                    string whetherdepartment = context.Request["whetherdepartment"].ConvertTo<string>("0");

                    string data = WeiXinJsonData.GetChannelCompanyList(comid, issuetype, companystate, whetherdepartment);
                    context.Response.Write(data);
                }
                if (oper == "GetChannelCompanyList2")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string issuetype = context.Request["issuetype"].ConvertTo<string>("0");
                    string companystate = context.Request["companystate"].ConvertTo<string>("1");
                    string whetherdepartment = context.Request["whetherdepartment"].ConvertTo<string>("0");
                    int channelcompanyid = context.Request["channelcompanyid"].ConvertTo<int>(0);


                    string data = WeiXinJsonData.GetChannelCompanyList(comid, issuetype, companystate, whetherdepartment, channelcompanyid);
                    context.Response.Write(data);
                }
                if (oper == "GetActivityList")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string runstate = context.Request["runstate"].ConvertTo<string>("1");
                    string whetherexpired = context.Request["whetherexpired"].ConvertTo<string>("0");

                    string data = WeiXinJsonData.GetActivityList(comid, runstate, whetherexpired);
                    context.Response.Write(data);
                }
                if (oper == "wxqunfa")
                {
                    int userid = context.Request["userid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    int sendobj = context.Request["sendobj"].ConvertTo<int>(0);
                    int sendchildobj = context.Request["sendchildobj"].ConvertTo<int>(0);//门市id


                    string country = context.Request["country"].ConvertTo<string>("");//发送地区: 国家
                    string province = context.Request["province"].ConvertTo<string>("");//发送地区:省市
                    string city = context.Request["city"].ConvertTo<string>("");//发送地区: 城市
                    string sex = context.Request["sex"].ConvertTo<string>("");//性别

                    int tagtype = context.Request["tagtype"].ConvertTo<int>(0);//兴趣标签类型
                    int tag = context.Request["tag"].ConvertTo<int>(0);//兴趣标签

                    int menshiid = 0;
                    string weixins = GetQunfaWeixin(userid, comid, sendobj, sendchildobj, country, province, city, sex, tagtype, tag, out menshiid);


                    string content = context.Request["content"].ConvertTo<string>("");
                    string msgtype = context.Request["msgtype"].ConvertTo<string>("");
                    string media_id = context.Request["media_id"].ConvertTo<string>("");



                    string data = WeiXinJsonData.Wxqunfa(weixins, content, msgtype, media_id, userid, comid, menshiid);
                    context.Response.Write(data);
                }
                if (oper == "wxqunfa_dddtest")
                {
                    int userid = context.Request["userid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    int menshiid = 0;
                    string weixins = context.Request["weixins"].ConvertTo<string>("");


                    string content = context.Request["content"].ConvertTo<string>("");
                    string msgtype = context.Request["msgtype"].ConvertTo<string>("");
                    string media_id = context.Request["media_id"].ConvertTo<string>("");


                    string data = WeiXinJsonData.Wxqunfa(weixins, content, msgtype, media_id, userid, comid, menshiid);
                    context.Response.Write(data);
                }
                if (oper == "wxqunfanum")
                {
                    int userid = context.Request["userid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    int sendobj = context.Request["sendobj"].ConvertTo<int>(0);
                    int sendchildobj = context.Request["sendchildobj"].ConvertTo<int>(0);//门市id


                    string country = context.Request["country"].ConvertTo<string>("");//发送地区: 国家
                    string province = context.Request["province"].ConvertTo<string>("");//发送地区:省市
                    string city = context.Request["city"].ConvertTo<string>("");//发送地区: 城市
                    string sex = context.Request["sex"].ConvertTo<string>("");//性别

                    int tagtype = context.Request["tagtype"].ConvertTo<int>(0);//兴趣标签类型
                    int tag = context.Request["tag"].ConvertTo<int>(0);//兴趣标签

                    int menshiid = 0;
                    string weixins = GetQunfaWeixin(userid, comid, sendobj, sendchildobj, country, province, city, sex, tagtype, tag, out menshiid);

                    if (weixins != "")
                    {
                        string[] arr = weixins.Split(',');

                        context.Response.Write("{\"type\":\"100\",\"msg\":\"" + arr.Length + "\"}");
                    }
                    else
                    {
                        context.Response.Write("{\"type\":\"100\",\"msg\":\"0\"}");
                    }

                }

                if (oper == "Getqunfalist")
                {
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int userid = context.Request["userid"].ConvertTo<int>(0);


                    string data = WeiXinJsonData.Getqunfalist(comid, userid, pageindex, pagesize);
                    context.Response.Write(data);
                }
                if (oper == "Getinvitecodesendlog")
                {
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int userid = context.Request["userid"].ConvertTo<int>(0);


                    string data = WeiXinJsonData.Getinvitecodesendlog(comid, userid, pageindex, pagesize);
                    context.Response.Write(data);
                }
                if (oper == "editwxbasicstep1")
                {
                    int weixintype = context.Request["weixintype"].ConvertTo<int>();
                    int comid = context.Request["comid"].ConvertTo<int>(10);
                    int wxbasicid = context.Request["wxbasicid"].ConvertTo<int>(0);
                    string domain = context.Request["domain"].ConvertTo<string>();
                    string url = context.Request["url"].ConvertTo<string>();
                    string token = context.Request["token"].ConvertTo<string>();

                    string data = WeiXinJsonData.Editwxbasicstep1(wxbasicid, weixintype, comid, domain, url, token);

                    context.Response.Write(data);
                }
                if (oper == "editwxbasicstep2")
                {
                    int wxbasicid = context.Request["wxbasicid"].ConvertTo<int>(0);
                    string appid = context.Request["appid"].ConvertTo<string>();
                    string appsecret = context.Request["appsecret"].ConvertTo<string>();


                    string data = WeiXinJsonData.Editwxbasicstep2(wxbasicid, appid, appsecret);

                    context.Response.Write(data);
                }
                if (oper == "editwxbasicstep")
                {
                    int wxbasicid = context.Request["wxbasicid"].ConvertTo<int>(0);
                    string appid = context.Request["appid"].ConvertTo<string>();
                    string appsecret = context.Request["appsecret"].ConvertTo<string>();
                    int weixintype = context.Request["weixintype"].ConvertTo<int>(0);


                    string data = WeiXinJsonData.Editwxbasicstep(wxbasicid, appid, appsecret, weixintype);

                    context.Response.Write(data);
                }
                if (oper == "editwxbasicreply")
                {
                    int wxbasicid = context.Request["wxbasicid"].ConvertTo<int>(0);
                    string leavemsgreply = context.Request["leavemsgreply"].ConvertTo<string>();
                    string attentionreply = context.Request["attentionreply"].ConvertTo<string>();


                    string data = WeiXinJsonData.Editwxbasicreply(wxbasicid, leavemsgreply, attentionreply);

                    context.Response.Write(data);
                }
                if (oper == "delmaterialtype")
                {
                    int typeid = context.Request["typeid"].ConvertTo<int>(0);

                    int comid = context.Request["comid"].ConvertTo<int>(0);


                    string data = WeiXinJsonData.Delmaterialtype(typeid, comid);

                    context.Response.Write(data);
                }
                if (oper == "Getwxkfpagelist")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int userid = context.Request["userid"].ConvertTo<int>(0);
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    string isrun = context.Request["isrun"].ConvertTo<string>("0,1");

                    string key = context.Request["key"].ConvertTo<string>("");

                    string data = WeiXinJsonData.Getwxkfpagelist(pageindex, pagesize, comid, userid, isrun, key);
                    context.Response.Write(data);
                }
                if (oper == "Getwxkf")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int kfid = context.Request["kfid"].ConvertTo<int>(0);
                    string data = WeiXinJsonData.Getwxkf(kfid, comid);
                    context.Response.Write(data);
                }
                if (oper == "Getygdetail")
                {
                    string phone = context.Request["phone"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string data = WeiXinJsonData.Getygdetail(phone, comid);
                    context.Response.Write(data);
                }
                if (oper == "bindwxdkf")
                {
                    int kfid = context.Request["kfid"].ConvertTo<int>(0);
                    int ygid = context.Request["ygid"].ConvertTo<int>(0);
                    string ygname = context.Request["ygname"].ConvertTo<string>("");
                    int msid = context.Request["msid"].ConvertTo<int>(0);
                    string msname = context.Request["msname"].ConvertTo<string>("");
                    int isrun = context.Request["isrun"].ConvertTo<int>(0);

                    int createuserid = context.Request["createuserid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    int iszongkf = context.Request["iszongkf"].ConvertTo<int>(0);
                    int isbinded = context.Request["isbinded"].ConvertTo<int>(0);

                    DateTime createtime = DateTime.Now;

                    Wxkf m = new Wxkf
                    {
                        Kf_id = kfid,
                        Yg_id = ygid,
                        Yg_name = ygname,
                        Ms_id = msid,
                        Ms_name = msname,
                        Isrun = isrun,
                        Comid = comid,
                        Createtime = createtime,
                        Createuserid = createuserid,
                        Iszongkf = iszongkf,
                        Isbinded = isbinded

                    };

                    string data = WeiXinJsonData.Bindwxdkf(m);
                    context.Response.Write(data);
                }

                //获取微信模板MODEL信息
                if (oper == "templatemodelinfo")
                {
                    int id = context.Request["id"].ConvertTo<int>(0);
                    string data = WeiXinJsonData.Templatemodelinfo(id);
                    context.Response.Write(data);
                }

                //获取微信模板MODEL列表信息
                if (oper == "templatemodelpagelist")
                {
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    string data = WeiXinJsonData.Templatemodelpagelist(pageindex, pagesize);
                    context.Response.Write(data);
                }

                //获取微信模板MODEL列表信息
                if (oper == "templatecompagelist")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);


                    string data = WeiXinJsonData.Templatecompagelist(comid);
                    context.Response.Write(data);
                }

                //编辑微信模板MODEL信息
                if (oper == "templatemodeledit")
                {
                    int id = context.Request["id"].ConvertTo<int>(0);
                    int infotype = context.Request["infotype"].ConvertTo<int>(0);
                    string template_name = context.Request["template_name"].ConvertTo<string>("");
                    string first_DATA = context.Request["first_DATA"].ConvertTo<string>("");
                    string remark_DATA = context.Request["remark_DATA"].ConvertTo<string>("");


                    string data = WeiXinJsonData.Templatemodeledit(id, infotype, template_name, first_DATA, remark_DATA);

                    context.Response.Write(data);
                }

                //编辑微信模板MODEL信息
                if (oper == "templateedit")
                {
                    string id = context.Request["id"].ConvertTo<string>("");
                    string template_id = context.Request["template_id"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);


                    string data = WeiXinJsonData.Templateedit(comid, id, template_id);

                    context.Response.Write(data);
                }
                if (oper == "Getwxqunfa_newslist")
                {

                    int news_recordid = context.Request["news_recordid"].ConvertTo<int>(0);
                    int opertype = context.Request["opertype"].ConvertTo<int>(1);

                    string data = WeiXinJsonData.Getwxqunfa_newslist(news_recordid, opertype);

                    context.Response.Write(data);
                }
                if (oper == "delwxqunfa_news")
                {
                    int newsid = context.Request["newsid"].ConvertTo<int>(0);

                    string data = WeiXinJsonData.Delwxqunfa_news(newsid);

                    context.Response.Write(data);
                }
                //保存微信群发的多图文素材
                if (oper == "savewxqunfa_multinews")
                {
                    lock (lockobj)
                    {
                        int newsid = context.Request["newsid"].ConvertTo<int>(0);
                        int news_recordid = context.Request["news_recordid"].ConvertTo<int>(0);

                        int createuserid = context.Request["createuserid"].ConvertTo<int>(0);
                        int comid = context.Request["comid"].ConvertTo<int>(0);
                        string txttitle = context.Request["txttitle"].ConvertTo<string>("");
                        string txtauthor = context.Request["txtauthor"].ConvertTo<string>("");
                        string txtdigest = context.Request["txtdigest"].ConvertTo<string>("");
                        string thumb_media_id = context.Request["thumb_media_id"].ConvertTo<string>("");
                        string sel_show_cover_pic = context.Request["sel_show_cover_pic"].ConvertTo<string>("");
                        string txtcontent = context.Request["txtcontent"].ConvertTo<string>("");
                        string txtcontent_source_url = context.Request["txtcontent_source_url"].ConvertTo<string>("");

                        string thumb_url = context.Request["thumb_url"].ConvertTo<string>("");
                        int materialid = context.Request["materialid"].ConvertTo<int>(0);


                        string data = WeiXinJsonData.Savewxqunfa_multinews(newsid, news_recordid, createuserid, comid, txttitle, txtauthor, txtdigest, thumb_media_id, sel_show_cover_pic, txtcontent, txtcontent_source_url, thumb_url, materialid);

                        context.Response.Write(data);
                    }
                }
                //把多图文素材上传到微信服务器
                if (oper == "uploadwxqunfa_news_multi")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int news_recordid = context.Request["news_recordid"].ConvertTo<int>(0);
                    string data = WeiXinJsonData.Uploadwxqunfa_news_multi(news_recordid, comid);

                    context.Response.Write(data);
                }
                //保存微信群发的单图文素材 并且上传到微信服务器
                if (oper == "uploadwxqunfa_news")
                {
                    lock (lockobj)
                    {
                        int createuserid = context.Request["createuserid"].ConvertTo<int>(0);
                        int comid = context.Request["comid"].ConvertTo<int>(0);
                        string txttitle = context.Request["txttitle"].ConvertTo<string>("");
                        string txtauthor = context.Request["txtauthor"].ConvertTo<string>("");
                        string txtdigest = context.Request["txtdigest"].ConvertTo<string>("");
                        string thumb_media_id = context.Request["thumb_media_id"].ConvertTo<string>("");
                        string sel_show_cover_pic = context.Request["sel_show_cover_pic"].ConvertTo<string>("");
                        string txtcontent = context.Request["txtcontent"].ConvertTo<string>("");
                        string txtcontent_source_url = context.Request["txtcontent_source_url"].ConvertTo<string>("");

                        string thumb_url = context.Request["thumb_url"].ConvertTo<string>("");
                        int materialid = context.Request["materialid"].ConvertTo<int>(0);


                        string data = WeiXinJsonData.Uploadwxqunfa_news(createuserid, comid, txttitle, txtauthor, txtdigest, thumb_media_id, sel_show_cover_pic, txtcontent, txtcontent_source_url, thumb_url, materialid);

                        context.Response.Write(data);
                    }
                }
                if (oper == "wxqunfa_news_addrecordpagelist")
                {
                    int userid = context.Request["userid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(20);
                    string key = context.Request["key"].ConvertTo<string>("");

                    string data = WeiXinJsonData.Wxqunfa_news_addrecordpagelist(userid, comid, pageindex, pagesize, key);

                    context.Response.Write(data);
                }
                if (oper == "wxqunfa_news_addrecord")
                {
                    int userid = context.Request["userid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int tuwen_recordid = context.Request["tuwen_recordid"].ConvertTo<int>(0);

                    string data = WeiXinJsonData.Wxqunfa_news_addrecord(userid, comid, tuwen_recordid);

                    context.Response.Write(data);
                }

                if (oper == "GetNewsListByRecordid")
                {
                    int recordid = context.Request["recordid"].ConvertTo<int>(0);

                    string data = WeiXinJsonData.GetNewsListByRecordid(recordid);

                    context.Response.Write(data);
                }


                //广告列表
                if (oper == "getadlist")
                {
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    int applystate = context.Request["applystate"].ConvertTo<int>(1);
                    
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string key = context.Request["key"].ConvertTo<string>("");

                    string data = WeiXinJsonData.getadlist(pageindex, pagesize, applystate, comid,key);
                    context.Response.Write(data);
                }


                //一个
                if (oper == "Getwxad")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = WeiXinJsonData.Getwxad(id, comid);
                    context.Response.Write(data);
                }

                //访问量
                if (oper == "Wxadaddcount")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int vadd = context.Request["vadd"].ConvertTo<int>(0);
                    int ladd = context.Request["ladd"].ConvertTo<int>(0);
                    string data = WeiXinJsonData.Wxadaddcount(id, comid, vadd, ladd);
                    context.Response.Write(data);
                }

                //删除
                if (oper == "DelWxad")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = WeiXinJsonData.DelWxad(id, comid);
                    context.Response.Write(data);
                }

                //编辑
                if (oper == "editad")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    string Link = context.Request["Link"].ConvertTo<string>("");
                    string Title = context.Request["Title"].ConvertTo<string>("");
                    string Author = context.Request["Author"].ConvertTo<string>("");

                    string Keyword = context.Request["Keyword"].ConvertTo<string>("");

                    int applystate = context.Request["applystate"].ConvertTo<int>(1);

                    int comid = context.Request["comid"].ConvertTo<int>(0);
                     int Musicid = context.Request["Musicid"].ConvertTo<int>(0);
                      int Adtype = context.Request["Adtype"].ConvertTo<int>(0);

                    Wxad adinfo=new Wxad();
                    adinfo.Id = id;
                    adinfo.Applystate = applystate;
                    adinfo.Comid = comid;
                    adinfo.Link = Link;
                    adinfo.Musicid = Musicid;
                    adinfo.Title = Title;
                    adinfo.Adtype = Adtype;
                    adinfo.Author = Author;
                    adinfo.Keyword = Keyword;
                    
                    string data = WeiXinJsonData.Editwxad( adinfo);
                    context.Response.Write(data);
                }



                //广告列表
                if (oper == "Getwxadimagespagelist")
                {
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    int adid = context.Request["adid"].ConvertTo<int>(0);

                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string key = context.Request["key"].ConvertTo<string>("");

                    string data = WeiXinJsonData.Getwxadimagespagelist(pageindex, pagesize, comid, adid, key);
                    context.Response.Write(data);
                }


                //一个
                if (oper == "Getwxadimages")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    int adid = context.Request["adid"].ConvertTo<int>(0);

                    string data = WeiXinJsonData.Getwxadimages(id, adid);
                    context.Response.Write(data);
                }

                //删除
                if (oper == "DelWxadimages")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    int adid = context.Request["adid"].ConvertTo<int>(0);
                   
                    string data = WeiXinJsonData.DelWxadimages(id, adid);
                    context.Response.Write(data);
                }

                //改变顺序
                if (oper == "upWxadimages_sort")
                {
                    string menuids = context.Request["menuids"].ConvertTo<string>("");

                    string data = WeiXinJsonData.upWxadimages_sort(menuids);
                    context.Response.Write(data);
                }

                //编辑
                if (oper == "Editwxadimage")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    string Link = context.Request["Link"].ConvertTo<string>("");
                    int Adid = context.Request["Adid"].ConvertTo<int>(1);

                    int Imageid = context.Request["Imageid"].ConvertTo<int>(0);
                    int Sort = context.Request["Sort"].ConvertTo<int>(0);


                    WxAdImages adinfo = new WxAdImages();
                    adinfo.Id = id;
                    adinfo.Adid = Adid;
                    adinfo.Imageid = Imageid;
                    adinfo.Link = Link;
                    adinfo.Sort = Sort;

                    string data = WeiXinJsonData.Editwxadimage(adinfo);
                    context.Response.Write(data);
                }


            }
        }

        private string GetQunfaWeixin(int userid, int comid, int sendobj, int sendchildobj, string country, string province, string city, string sex, int tagtype, int tag, out int menshiid)
        {

            string weixins = "";//微信号列表
            if (sendobj == 0)//群发对象:全部
            {
                //得到公司全部会员
                List<B2b_crm> crms = new B2bCrmData().GetB2bCrmWeixinByComid(comid, country, province, city, sex, tagtype, tag);
                if (crms != null)
                {
                    if (crms.Count > 0)
                    {
                        foreach (B2b_crm m in crms)
                        {
                            if (m.Weixin != "")
                            {
                                weixins += m.Weixin + ",";
                            }
                        }
                        weixins = weixins.Substring(0, weixins.Length - 1);
                    }
                }
                menshiid = 0;
            }
            else if (sendobj == 1) //群发对象:门店
            {
                if (sendchildobj != 0)
                {
                    //得到门店 得到门店全部会员
                    List<B2b_crm> crms = new B2bCrmData().GetB2bCrmWeixinByMenshi(sendchildobj, country, province, city, sex, tagtype, tag);
                    if (crms != null)
                    {
                        if (crms.Count > 0)
                        {
                            foreach (B2b_crm m in crms)
                            {
                                if (m.Weixin != "")
                                {
                                    weixins += m.Weixin + ",";
                                }
                            }
                            weixins = weixins.Substring(0, weixins.Length - 1);
                        }
                    }

                }
                else
                {
                    //得到公司全部会员
                    List<B2b_crm> crms = new B2bCrmData().GetB2bCrmWeixinByComid(comid, country, province, city, sex, tagtype, tag);
                    if (crms != null)
                    {
                        if (crms.Count > 0)
                        {
                            foreach (B2b_crm m in crms)
                            {
                                if (m.Weixin != "")
                                {
                                    weixins += m.Weixin + ",";
                                }
                            }
                            weixins = weixins.Substring(0, weixins.Length - 1);
                        }
                    }
                }
                menshiid = sendchildobj;
            }
            else //群发对象:分组
            {

                //得到公司默组会员
                List<B2b_crm> crms = new B2bCrmData().GetB2bCrmWeixinByComid(comid, country, province, city, sex, tagtype, tag, sendchildobj.ToString());
                if (crms != null)
                {
                    if (crms.Count > 0)
                    {
                        foreach (B2b_crm m in crms)
                        {
                            if (m.Weixin != "")
                            {
                                weixins += m.Weixin + ",";
                            }
                        }
                        weixins = weixins.Substring(0, weixins.Length - 1);
                    }
                }

                //选择按兴趣分组，则门市id必须为0
                menshiid = 0;
            }
            return weixins;
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