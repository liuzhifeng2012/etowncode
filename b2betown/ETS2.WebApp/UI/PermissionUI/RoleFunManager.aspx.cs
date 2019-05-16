using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.Permision.Service.PermisionService.Model;
using ETS2.Permision.Service.PermisionService.Data;

namespace ETS2.WebApp.UI.PermissionUI
{
    public partial class RoleFunManager : System.Web.UI.Page
    {
        public string treestr = "";

        public string groupid = "0";//管理组id
        public string groupname = "";//管理组名称
        protected void Page_Load(object sender, EventArgs e)
        {
            groupid = Request["groupid"].ConvertTo<string>("0");

            Sys_Group group = new Sys_GroupData().GetGroupById(groupid.ConvertTo<int>(0));
            groupname = group.Groupname;

            //根据groupid得到管理组所拥有的权限
            int listcount = 0;
            IList<Sys_Action> actionlist = new Sys_ActionData().GetActionsByGroupdId(groupid, out listcount);
            var ListSelectedActionId = new List<int>();
            foreach (Sys_Action a in actionlist)
            {
                ListSelectedActionId.Add(a.Actionid);
            }



            treestr += "[";
            //得到权限分栏表
            int columncount = 0;
            IList<Sys_ActionColumn> columns = new Sys_ActionColumnData().GetColumns(out columncount);

            foreach (Sys_ActionColumn column in columns)
            {
                //判断权限分栏下如果没有权限，则不显示
                int actionnum = new Sys_ActionData().GetActionNumByColumn(column.Actioncolumnid);
                if (actionnum > 0)
                {
                    int columnid = column.Actioncolumnid;
                    string columnname = column.Actioncolumnname;

                    treestr += "{id:" + columnid + ",pId:0,name:\"" + columnname + "\",open:true,nocheck:true},";
                    //得到权限分栏表下的权限列表
                    int actioncount = 0;
                    IList<Sys_Action> actions = new Sys_ActionData().GetActionsByColumnId(columnid, out actioncount);


                    foreach (Sys_Action action in actions)
                    {
                        int actionid = action.Actionid;
                        string actionname = action.Actionname;
                        string actionurl = action.Actionurl;

                        if (ListSelectedActionId.Contains(actionid))
                        {
                            treestr += "{id:" + actionid + ",pId:" + columnid + ",name:\"" + actionname + "\", checked: true},";
                        }
                        else
                        {
                            treestr += "{id:" + actionid + ",pId:" + columnid + ",name:\"" + actionname + "\"},";
                        }
                        //根据管理组、权限 得到右侧子导航(显示)--选中项
                        var ListSelectedsubnavId = new List<int>();
                        IList<Sys_groupactionsubnav> checked_slist = new Sys_groupactionsubnavData().GetSys_groupactionsubnav(action.Actionid, groupid);
                        if (checked_slist.Count > 0)
                        { 
                            foreach (Sys_groupactionsubnav a in checked_slist)
                            {
                                ListSelectedsubnavId.Add(a.subnavid);
                            }
                        }

                        //根据权限 得到右侧子导航(显示)
                        int viewcode = 1;
                        IList<Sys_subnav> slist = new Sys_subnavData().GetSys_subnavbyactionid(action.Actionid, viewcode);
                        if (slist.Count > 0)
                        {
                            foreach (Sys_subnav m in slist)
                            {
                                int subnavid = m.id;
                                string subnavname = m.subnav_name;

                                if (ListSelectedsubnavId.Count > 0)
                                {
                                    if (ListSelectedsubnavId.Contains(m.id))
                                    {
                                        treestr += "{id:" + subnavid + ",pId:" + action.Actionid + ",name:\"" + subnavname + "\", checked: true},";
                                    }
                                    else
                                    {
                                        treestr += "{id:" + subnavid + ",pId:" + action.Actionid + ",name:\"" + subnavname + "\"},";
                                    }
                                }
                                else
                                {
                                    treestr += "{id:" + subnavid + ",pId:" + action.Actionid + ",name:\"" + subnavname + "\"},";

                                }
                            }
                        }

                    }
                }
            }
            treestr = treestr.Substring(0, treestr.Length - 1);
            treestr += "]";

            //            treestr = @"[
            //			{ id: 1, pId: 0, name: '随意勾选 1', open: true, nocheck:true },
            //			{ id: 11, pId: 1, name: '随意勾选 1-1', open: true },
            //			{ id: 111, pId: 11, name: '随意勾选 1-1-1' },
            //			{ id: 112, pId: 11, name: '随意勾选 1-1-2' },
            //			{ id: 12, pId: 1, name: '随意勾选 1-2', open: true },
            //			{ id: 121, pId: 12, name: '随意勾选 1-2-1' },
            //			{ id: 122, pId: 12, name: '随意勾选 1-2-2' },
            //			{ id: 2, pId: 0, name: '随意勾选 2', checked: true, open: true },
            //			{ id: 21, pId: 2, name: '随意勾选 2-1' },
            //			{ id: 22, pId: 2, name: '随意勾选 2-2', open: true },
            //			{ id: 221, pId: 22, name: '随意勾选 2-2-1', checked: true },
            //			{ id: 222, pId: 22, name: '随意勾选 2-2-2' },
            //			{ id: 23, pId: 2, name: '随意勾选 2-3' }
            //		]";


        }
    }
}