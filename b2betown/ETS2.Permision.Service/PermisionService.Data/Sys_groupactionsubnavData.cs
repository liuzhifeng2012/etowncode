using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.Permision.Service.PermisionService.Model;
using ETS.Data.SqlHelper;
using ETS2.Permision.Service.PermisionService.Data.InternalData;

namespace ETS2.Permision.Service.PermisionService.Data
{
    public class Sys_groupactionsubnavData
    {
        public IList<Sys_groupactionsubnav> GetSys_groupactionsubnav(int actionid, string groupid)
        {
             using(var helper=new SqlHelper())
             {
                 IList<Sys_groupactionsubnav> list = new InternalSys_groupactionsubnav(helper).GetSys_groupactionsubnav(actionid,groupid);
                 return list;
             }
        }

        public string GetGroupsByactionsubnavid(int actionid, int subnavid)
        {
            using (var helper = new SqlHelper())
            {
                string r = new InternalSys_groupactionsubnav(helper).GetGroupsByactionsubnavid(actionid, subnavid);
                return r;
            }
        }
    }
}
