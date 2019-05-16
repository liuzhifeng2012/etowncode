using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.Permision.Service.PermisionService.Model;
using ETS.Data.SqlHelper;
using ETS2.Permision.Service.PermisionService.Data.InternalData;

namespace ETS2.Permision.Service.PermisionService.Data
{
    public class Sys_ActionData
    {
        public List<Sys_Action> GetAllAction(out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalSys_Action(helper).GetAllAction(out totalcount);

                return list;
            }
        }

        public int ActionInit()
        {
            using (var helper = new SqlHelper())
            {

                var count = new InternalSys_Action(helper).ActionInit();
                return count;
            } 
        }

        public List<Sys_Action> PermissionPageList(int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalSys_Action(helper).PermissionPageList(pageindex,pagesize,out totalcount);

                return list;
            }
        }

        public Sys_Action GetActionById(int actionid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalSys_Action(helper).GetActionById(actionid);

                return list;
            }
        }

        public int EditAction(Sys_Action sysaction)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalSys_Action(helper).EditAction(sysaction);

                return list;
            }
        }

        public IList<Sys_Action> GetActionsByColumnId(int columnid,out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalSys_Action(helper).GetActionsByColumnId(columnid,out totalcount);

                return list;
            }
        }

        public IList<Sys_Action> GetActionsByGroupdId(string groupid,out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalSys_Action(helper).GetActionsByGroupdId(groupid, out totalcount);

                return list;
            }
        }

        public List<Sys_Action> GetAllRoleFuncByUser(int userid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalSys_Action(helper).GetAllRoleFuncByUser(userid, out totalcount);

                return list;
            }
        }



        public List<Sys_Action> GetActionsByColumnId(int userid, int funcId, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalSys_Action(helper).GetActionsByColumnId(userid,funcId, out totalcount);

                return list;
            }
        }

        public int DelActionById(int actionid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalSys_Action(helper).DelActionById(actionid);

                return list;
            }
        }
        /*判断链接地址是不是包含在权限对应的地址中*/
        public bool Isactionurl(string Requestfile)
        {
            using(var helper=new SqlHelper())
            {
                bool r = new InternalSys_Action(helper).Isactionurl(Requestfile);
                return r;
            }
        }
        /*判断用户是否可以访问权限对应的地址*/
        public bool Iscanvisit(string Requestfile,int userid)
        {
            using (var helper = new SqlHelper())
            {
                bool r = new InternalSys_Action(helper).Iscanvisit(Requestfile,userid);
                return r;
            }
        }

        public List<Sys_Action> Permissionlist(int columnid=0)
        {
             using(var helper=new SqlHelper())
             {
                 List<Sys_Action> r = new InternalSys_Action(helper).Permissionlist(columnid);
                 return r;
             }
        }



        public int GetActionNumByColumn(int columnid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalSys_Action(helper).GetActionNumByColumn(columnid);
                return r;
            }
        }
    }
}
