using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.Permision.Service.PermisionService.Model;
using ETS.Data.SqlHelper;
using ETS2.Permision.Service.PermisionService.Data.InternalData;

namespace ETS2.Permision.Service.PermisionService.Data
{
    public class Sys_subnavData
    {
        public Sys_subnav Getsyssubnav(int subnavid)
        {
             using(var helper=new SqlHelper())
             {
                 Sys_subnav m = new InternalSys_subnav(helper).Getsyssubnav(subnavid);
                 return m;
             }
        }

        public int Editsys_subnav(int id, int actionid, int columnid, string subnavurl, string subnavname)
        {
            using (var helper = new SqlHelper())
            {
                int m = new InternalSys_subnav(helper).Editsys_subnav( id,   actionid,   columnid,   subnavurl,   subnavname);
                return m;
            }
        }
        public IList<Sys_subnav> Getsys_subnavpagelist(int pageindex, int pagesize, int columnid, int actionid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                List<Sys_subnav> r = new InternalSys_subnav(helper).Getsys_subnavpagelist(pageindex, pagesize, columnid, actionid, out totalcount);
                return r;
            }
        }

        public int Upsubnavviewcode(int subnavid, int viewcode,int actionid,string groupids)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalSys_subnav(helper).Upsubnavviewcode(subnavid,viewcode,actionid,groupids);
                return r;
            }
        }

        public IList<Sys_subnav> GetSys_subnavbyactionid(int actionid, int viewcode)
        {
            using (var helper = new SqlHelper())
            {
                IList<Sys_subnav> r = new InternalSys_subnav(helper).GetSys_subnavbyactionid(actionid, viewcode);
                return r;
            }
        }

        public IList<Sys_subnav> Getsys_subnavlistbyvirtualurl(string virtualurl, int viewcode, int groupid, string parastr)
        {
            using (var helper = new SqlHelper())
            {
                IList<Sys_subnav> r = new InternalSys_subnav(helper).Getsys_subnavlistbyvirtualurl(virtualurl, viewcode, groupid, parastr);
                return r;
            }
        }

        public int GetsubnavNum(string subnavname, string subnavurl)
        {
            using (var helper = new SqlHelper())
            {
                int  r = new InternalSys_subnav(helper).GetsubnavNum(subnavname,subnavurl);
                return r;
            }
        }

        public IList<Sys_subnav> Getallsys_subnavpagelist(int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                IList<Sys_subnav> r = new InternalSys_subnav(helper).Getallsys_subnavpagelist(  pageindex,   pagesize, out   totalcount);
                return r;
            }
        }

        public int Upsubnavdatabase(int subnavid,int oldviewcode,int oldcolumnid,int oldactionid,string oldgroupids,int newviewcode,int newcolumnid,int newactionid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalSys_subnav(helper).Upsubnavdatabase(subnavid,oldviewcode,oldcolumnid,oldactionid,oldgroupids,newviewcode,newcolumnid,newactionid);
                return r;
            }
        }

        public Sys_subnav Getsys_subnav(string vurl, string parastr)
        {
            using (var helper = new SqlHelper())
            {
                Sys_subnav r = new InternalSys_subnav(helper).Getsys_subnav(vurl,parastr);
                return r;
            }
        }
    }
}
