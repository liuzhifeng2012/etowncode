using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.Permision.Service.PermisionService.Model;
using ETS.Data.SqlHelper;
using ETS2.Permision.Service.PermisionService.Data.InternalData;

namespace ETS2.Permision.Service.PermisionService.Data
{
    public class Sys_ActionColumnData
    {
        public IList<Sys_ActionColumn> GetColumns(out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalSys_ActionColumn(helper).GetColumns(out totalcount);

                return list;
            }
        }

        public List<Sys_ActionColumn> GetActionColumnByUser(int userid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalSys_ActionColumn(helper).GetActionColumnByUser(userid,out totalcount);

                return list;
            }
        }

        public List<Sys_ActionColumn> Getallactioncolumns()
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalSys_ActionColumn(helper).Getallactioncolumns();

                return list;
            }
        }

        public Sys_ActionColumn GetActionColumn(int  columnid)
        {
            using (var helper = new SqlHelper())
            {

                Sys_ActionColumn r = new InternalSys_ActionColumn(helper).GetActionColumn(columnid);

                return r;
            }
        }
    }
}
