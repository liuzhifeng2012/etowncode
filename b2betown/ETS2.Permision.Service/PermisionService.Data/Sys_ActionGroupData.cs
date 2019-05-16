using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.Permision.Service.PermisionService.Data.InternalData;
using ETS.Data.SqlHelper;

namespace ETS2.Permision.Service.PermisionService.Data
{
    public class Sys_ActionGroupData
    {
        public int DistributeAction(int groupid, string selednodeid,int createuserid,string createusername,DateTime createdate)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalSys_ActionGroup(helper).DistributeAction(groupid,selednodeid,createuserid,createusername,createdate);

                return list;
            }
        }
    }
}
