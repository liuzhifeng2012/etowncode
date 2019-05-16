using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Framework;
using ETS.JsonFactory;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Common.Business;
using ETS2.Permision.Service.PermisionService.Model;

namespace ETS2.WebApp.JsonFactory
{
    /// <summary>
    /// PermissionHandler 的摘要说明
    /// </summary>
    public class PermissionHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string oper = context.Request["oper"].ConvertTo<string>("");
            if (oper != "")
            {
                if (oper == "confirmcompletedakuan")
                {
                    int id = context.Request["id"].ConvertTo<int>(0);
                    int operstatus = context.Request["operstatus"].ConvertTo<int>(0);
                    int opertor = context.Request["opertor"].ConvertTo<int>(0);
                    string operremark = context.Request["operremark"].ConvertTo<string>("");
                    int zhuanzhangsucimg = context.Request["zhuanzhangsucimg"].ConvertTo<int>(0);


                    string data = PermissionJsonData.Confirmcompletedakuan(id, operstatus, opertor, operremark, zhuanzhangsucimg);

                    context.Response.Write(data);
                }
                if (oper == "channelrebateapplyalllist")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(15);
                    string operstatus = context.Request["operstatus"].ConvertTo<string>("0,1");

                    string data = PermissionJsonData.Channelrebateapplyalllist(pageindex, pagesize, comid, operstatus);

                    context.Response.Write(data);
                }
                if (oper == "channelrebateapplylist")
                {
                    int channelid = context.Request["channelid"].ConvertTo<int>(0);
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(15);
                    string operstatus = context.Request["operstatus"].ConvertTo<string>("0,1");

                    string data = PermissionJsonData.Channelrebateapplylist(pageindex, pagesize, channelid, operstatus);

                    context.Response.Write(data);
                }
                if (oper == "channelapplyrebate")
                {
                    int channelid = context.Request["channelid"].ConvertTo<int>(0);
                    string applytype = context.Request["applytype"].ConvertTo<string>("");
                    string applydetail = context.Request["applydetail"].ConvertTo<string>("");
                    decimal applymoney = context.Request["applymoney"].ConvertTo<decimal>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = PermissionJsonData.Channelapplyrebate(channelid, applytype, applydetail, applymoney, comid);

                    context.Response.Write(data);
                }
                if (oper == "Upchannelrebateaccount")
                {
                    int channelid = context.Request["channelid"].ConvertTo<int>(0);
                    string truename = context.Request["truename"].ConvertTo<string>("");
                    string account = context.Request["account"].ConvertTo<string>("");
                    string newphone = context.Request["newphone"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = PermissionJsonData.Upchannelrebateaccount(channelid, truename, account, newphone, comid);

                    context.Response.Write(data);
                }
                if (oper == "getchanelrebateApplyaccount")
                {
                    int channelid = context.Request["channelid"].ConvertTo<int>(0);
                    string data = PermissionJsonData.GetchanelrebateApplyaccount(channelid);

                    context.Response.Write(data);
                }
                if (oper == "channelrebatelist")
                {
                    int channelid = context.Request["channelid"].ConvertTo<int>(0);
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(15);
                    string payment = context.Request["payment"].ConvertTo<string>("1,2,3");

                    string data = PermissionJsonData.Channelrebatelist(pageindex, pagesize, channelid, payment);

                    context.Response.Write(data);
                }
                if (oper == "getsys_subnav")
                {
                    string vurl = context.Request["vurl"].ConvertTo<string>("");
                    string parastr = context.Request["parastr"].ConvertTo<string>("");
                    string data = PermissionJsonData.Getsys_subnav(vurl, parastr);

                    context.Response.Write(data);
                }
                if (oper == "upsubnavdatabase")
                {

                    int oldviewcode = context.Request["oldviewcode"].ConvertTo<int>(0);
                    int subnavid = context.Request["subnavid"].ConvertTo<int>(0);
                    int oldactionid = context.Request["oldactionid"].ConvertTo<int>(0);
                    int oldcolumnid = context.Request["oldcolumnid"].ConvertTo<int>(0);
                    string oldgroupids = context.Request["oldgroupids"].ConvertTo<string>("");

                    int newviewcode = context.Request["newviewcode"].ConvertTo<int>(0);
                    int newcolumnid = context.Request["newcolumnid"].ConvertTo<int>(0);
                    int newactionid = context.Request["newactionid"].ConvertTo<int>(0);
                    string data = PermissionJsonData.Upsubnavdatabase(subnavid, oldviewcode, oldcolumnid, oldactionid, oldgroupids, newviewcode, newcolumnid, newactionid);

                    context.Response.Write(data);
                }
                if (oper == "allsys_subnavpagelist")
                {
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(0);
                    int seled_actionid = context.Request["actionid"].ConvertTo<int>(0);
                    int seled_columnid = context.Request["columnid"].ConvertTo<int>(0);



                    string data = PermissionJsonData.Getallsys_subnavpagelist(pageindex, pagesize, seled_columnid, seled_actionid);

                    context.Response.Write(data);
                }
                if (oper == "getsys_subnavlistbyvirtualurl")
                {
                    string virtualurl = context.Request["virtualurl"].ConvertTo<string>("");
                    int viewcode = context.Request["viewcode"].ConvertTo<int>(1);
                    int groupid = context.Request["groupid"].ConvertTo<int>(0);
                    string parastr = context.Request["parastr"].ConvertTo<string>("");

                    string data = PermissionJsonData.Getsys_subnavlistbyvirtualurl(virtualurl, viewcode, groupid, parastr);

                    context.Response.Write(data);
                }
                if (oper == "upsubnavviewcode")
                {
                    int viewcode = context.Request["viewcode"].ConvertTo<int>(0);
                    int subnavid = context.Request["subnavid"].ConvertTo<int>(0);
                    int actionid = context.Request["actionid"].ConvertTo<int>(0);
                    string groupids = context.Request["groupids"].ConvertTo<string>("");
                    string data = PermissionJsonData.Upsubnavviewcode(subnavid, viewcode, actionid, groupids);

                    context.Response.Write(data);
                }
                if (oper == "delsys_subnav")
                {
                    int subnavid = context.Request["subnavid"].ConvertTo<int>(0);
                    string data = PermissionJsonData.Delsubnav(subnavid);

                    context.Response.Write(data);
                }
                if (oper == "sys_subnavpagelist")
                {
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(0);
                    int actionid = context.Request["actionid"].ConvertTo<int>(0);
                    int columnid = context.Request["columnid"].ConvertTo<int>(0);



                    string data = PermissionJsonData.Getsys_subnavpagelist(pageindex, pagesize, columnid, actionid);

                    context.Response.Write(data);
                }
                if (oper == "editsys_subnav")
                {
                    int id = context.Request["id"].ConvertTo<int>(0);
                    int actionid = context.Request["actionid"].ConvertTo<int>(0);
                    int columnid = context.Request["columnid"].ConvertTo<int>(0);
                    string subnavurl = context.Request["subnavurl"].ConvertTo<string>("");
                    string subnavname = context.Request["subnavname"].ConvertTo<string>("");
                    string data = PermissionJsonData.Editsys_subnav(id, actionid, columnid, subnavurl.ToLower(), subnavname);

                    context.Response.Write(data);
                }
                if (oper == "permissionlist")
                {
                    int columnid = context.Request["columnid"].ConvertTo<int>(0);
                    string data = PermissionJsonData.Permissionlist(columnid);

                    context.Response.Write(data);
                }
                if (oper == "Getsyssubnav")
                {
                    int subnavid = context.Request["subnavid"].ConvertTo<int>(0);
                    string data = PermissionJsonData.Getsyssubnav(subnavid);

                    context.Response.Write(data);
                }
                if (oper == "permissionpagelist")
                {

                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);


                    string data = PermissionJsonData.PermissionPageList(pageindex, pagesize);

                    context.Response.Write(data);
                }
                if (oper == "grouppagelist")
                {

                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);


                    string data = PermissionJsonData.GroupPageList(pageindex, pagesize);

                    context.Response.Write(data);
                }
                if (oper == "getGroupById")
                {

                    var groupid = context.Request["groupid"].ConvertTo<int>(0);



                    string data = PermissionJsonData.GetGroupById(groupid);

                    context.Response.Write(data);
                }
                if (oper == "getGroupByUserId")
                {
                    var userid = context.Request["userid"].ConvertTo<int>(0);



                    string data = PermissionJsonData.GetGroupByUserId(userid);

                    context.Response.Write(data);
                }


                if (oper == "getActionById")
                {

                    var actionid = context.Request["actionid"].ConvertTo<int>(0);



                    string data = PermissionJsonData.GetActionById(actionid);

                    context.Response.Write(data);
                }


                if (oper == "EditGroup")
                {
                    int groupid = context.Request["groupid"].ConvertTo<int>(0);
                    string groupname = context.Request["groupname"].ConvertTo<string>("");
                    string groupinfo = context.Request["groupinfo"].ConvertTo<string>("");

                    string groupids = context.Request["groupids"].ConvertTo<string>("");
                    bool isviewchannel = context.Request["isviewchannel"].ConvertTo<bool>(true);


                    B2b_company_manageuser user = UserHelper.CurrentUser();
                    int masterid = user.Id;
                    string mastername = user.Employeename;

                    DateTime createdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    Sys_Group sysgroup = new Sys_Group()
                    {

                        Groupid = groupid,
                        Groupname = groupname,
                        Groupinfo = groupinfo,
                        Masterid = masterid,
                        Mastername = mastername,
                        Createdate = createdate,
                        Groupids = groupids,
                        Isviewchannel = isviewchannel,
                        CrmIsAccurateToPerson = context.Request["CrmIsAccurateToPerson"].ConvertTo<bool>(false),
                        OrderIsAccurateToPerson = context.Request["OrderIsAccurateToPerson"].ConvertTo<int>(0),

                        Iscanverify = context.Request["iscanverify"].ConvertTo<int>(0),
                        iscanset_imprest = context.Request["iscanset_imprest"].ConvertTo<int>(0),
                        iscanset_order = context.Request["iscanset_order"].ConvertTo<int>(0),
                        validateservertype = context.Request["validateservertype"].ConvertTo<int>(0),
                        canviewpro = context.Request["canviewpro"].ConvertTo<int>(0)
                    };
                    string data = PermissionJsonData.EditGroup(sysgroup);

                    context.Response.Write(data);
                }

                if (oper == "EditAction")
                {
                    int actionid = context.Request["actionid"].ConvertTo<int>(0);
                    string actionname = context.Request["actionname"].ConvertTo<string>("");
                    string actionurl = context.Request["actionurl"].ConvertTo<string>("");
                    int columnid = context.Request["columnid"].ConvertTo<int>(0);
                    bool isshow = context.Request["columnid"].ConvertTo<bool>(false); ;



                    Sys_Action sysaction = new Sys_Action()
                    {
                        Actionid = actionid,
                        Actionname = actionname,
                        Actionurl = actionurl,
                        Actioncolumnid = columnid,
                        Viewmode = isshow


                    };
                    string data = PermissionJsonData.EditAction(sysaction);

                    context.Response.Write(data);
                }

                if (oper == "masterpagelist")
                {
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);

                    var groupid = context.Request["groupid"].ConvertTo<string>("0");

                    var childcomid = context.Request["childcomid"].ConvertTo<int>(0);

                    string data = PermissionJsonData.Masterpagelist(pageindex, pagesize, groupid, childcomid);

                    context.Response.Write(data);
                }
                if (oper == "masterpagelistbyemploystate")
                {
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);

                    var groupid = context.Request["groupid"].ConvertTo<string>("0");

                    var childcomid = context.Request["childcomid"].ConvertTo<int>(0);

                    string employstate = context.Request["employstate"].ConvertTo<string>("1");//默认显示的在职

                    var key = context.Request["key"].ConvertTo<string>("");

                    string data = PermissionJsonData.Masterpagelist(employstate, pageindex, pagesize, groupid, childcomid, key);

                    context.Response.Write(data);
                }

                if (oper == "masterpagelistByComId")
                {
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);

                    var comid = context.Request["comid"].ConvertTo<string>("0");
                    int userid = context.Request["userid"].ConvertTo<int>(0);

                    string data = PermissionJsonData.MasterpagelistByComId(pageindex, pagesize, comid, userid);

                    context.Response.Write(data);
                }
                if (oper == "isdefaultkf")
                {

                    var comid = context.Request["comid"].ConvertTo<string>("0");
                    int userid = context.Request["userid"].ConvertTo<int>(0);


                    string data = PermissionJsonData.ChangeIsDefaultKf(userid);

                    context.Response.Write(data);
                }
                if (oper == "masterpagelistByComIdAndEmploystate")
                {
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);

                    var comid = context.Request["comid"].ConvertTo<string>("0");
                    int userid = context.Request["userid"].ConvertTo<int>(0);
                    var key = context.Request["key"].ConvertTo<string>("");

                    string employstate = context.Request["employstate"].ConvertTo<string>("1");

                    string data = PermissionJsonData.MasterpagelistByComId(employstate, pageindex, pagesize, comid, userid, key);

                    context.Response.Write(data);
                }
                if (oper == "editmastergroup")
                {
                    var masterid = context.Request["masterid"].ConvertTo<string>("0");
                    var mastername = context.Request["mastername"].ConvertTo<string>("");
                    var grouparr = context.Request["grouparr"].ConvertTo<string>("");

                    B2b_company_manageuser user = UserHelper.CurrentUser();
                    int createmasterid = user.Id;
                    string createmastername = user.Employeename;
                    DateTime createdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    string data = PermissionJsonData.EditMasterGroup(masterid, mastername, grouparr, createmasterid, createmastername, createdate);

                    context.Response.Write(data);
                }
                if (oper == "DistributeAction")
                {
                    var groupid = context.Request["groupid"].ConvertTo<int>(0);
                    var selednodeid = context.Request["selednodeid"].ConvertTo<string>("");
                    var selednodepId = context.Request["selednodepId"].ConvertTo<string>("");

                    B2b_company_manageuser user = UserHelper.CurrentUser();
                    int createmasterid = user.Id;
                    string createmastername = user.Employeename;
                    DateTime createdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));


                    string data = PermissionJsonData.DistributeAction(groupid, selednodeid, createmasterid, createmastername, createdate, selednodepId);



                    context.Response.Write(data);
                }
                if (oper == "GetAllGroups")
                {
                    string data = PermissionJsonData.GetAllGroups();



                    context.Response.Write(data);
                }
                if (oper == "GetGroupByUserId")
                {
                    int userid = context.Request["userid"].ConvertTo<int>(0);
                    string data = PermissionJsonData.GetGroupByUserId(userid);
                    context.Response.Write(data);
                }


                //如果是门市 ，则显示门市经理 权限内的管理组列表
                //如果是合作单位 ，则显示合作单位负责人  权限内的管理组列表
                //如果是公司 ，则 根据登录账户角色判断其可以显示的管理组列表
                if (oper == "GetGroupBychannelsource")
                {
                    string channelsource = context.Request["channelsource"].ConvertTo<string>("0,1");
                    int userid = context.Request["userid"].ConvertTo<int>(0);

                    string data = PermissionJsonData.GetGroupBychannelsource(channelsource, userid);
                    context.Response.Write(data);
                }
                //得到全部的权限分栏
                if (oper == "Getallactioncolumns")
                {
                    string data = PermissionJsonData.Getallactioncolumns();
                    context.Response.Write(data);
                }
                if (oper == "delActionById")
                {
                    int actionid = context.Request["actionid"].ConvertTo<int>(0);
                    string data = PermissionJsonData.DelActionById(actionid);
                    context.Response.Write(data);
                }
                if (oper == "delGroupById")
                {
                    int groupid = context.Request["groupid"].ConvertTo<int>(0);
                    string data = PermissionJsonData.DelGroupById(groupid);
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