using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.Permision.Service.PermisionService.Model;
using ETS2.Permision.Service.PermisionService.Data.InternalData;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.Permision.Service.PermisionService.Data
{
    public class Sys_MasterGroupData
    {
        public List<Sys_Group> GetGroupByMasterId(int masterid)
        {
            using (var helper = new SqlHelper())
            {


                var result = new InternalSys_MasterGroup(helper).GetGroupByMasterId(masterid);

                return result;

            }
        }

        public string GetGroupNameStrByMasterId(int masterid)
        {
            string groupnamestr = "";

            List<Sys_Group> list = GetGroupByMasterId(masterid);
            if (list == null)
            {
                return "";
            }
            else
            {
                if (list.Count > 0)
                {
                    foreach (Sys_Group sysgroup in list)
                    {
                        groupnamestr += sysgroup.Groupname + ",";
                    }
                    return groupnamestr.Substring(0, groupnamestr.Length - 1);
                }
                else
                {
                    return "";
                }
            }
        }

        public int EditMasterGroup(string masterid, string mastername, string grouparr, int createmasterid, string createmastername, DateTime createdate)
        {
            using (var helper = new SqlHelper())
            {

                var count = new InternalSys_MasterGroup(helper).EditMasterGroup(masterid, mastername, grouparr, createmasterid, createmastername, createdate);
                return count;
            }
        }

        public string GetGroupIdStrByMasterId(int masterid)
        {
            string groupidstr = "";

            List<Sys_Group> list = GetGroupByMasterId(masterid);
            if (list == null)
            {
                return "";
            }
            else
            {
                if (list.Count > 0)
                {
                    foreach (Sys_Group sysgroup in list)
                    {
                        groupidstr += sysgroup.Groupid + ",";
                    }
                    return groupidstr.Substring(0, groupidstr.Length - 1);
                }
                else
                {
                    return "";
                }
            }
        }

 
    }
}
