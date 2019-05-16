using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.Permision.Service.PermisionService.Model;
using ETS.Data.SqlHelper;
using ETS2.Permision.Service.PermisionService.Data.InternalData;
 

namespace ETS2.Permision.Service.PermisionService.Data
{
    public class Sys_GroupData
    {
        public List<Sys_Group> GroupPageList(int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalSys_Group(helper).GroupPageList(pageindex, pagesize, out totalcount);
                return list;
            }
        }

        public Sys_Group GetGroupById(int groupid)
        {
            using (var helper = new SqlHelper())
            {


                Sys_Group result = new InternalSys_Group(helper).GetGroupById(groupid);

                return result;

            }

        }

        public int EditGroup(Sys_Group sysgroup)
        {
            using (var helper = new SqlHelper())
            {


                int result = new InternalSys_Group(helper).EditGroup(sysgroup);

                return result;

            }
        }



        public List<Sys_Group> GetAllGroups(out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalSys_Group(helper).GetAllGroups( out totalcount);
                return list;
            }
        }

        public Sys_Group GetGroupByUserId(int userid)
        {
            using (var helper = new SqlHelper())
            {


                Sys_Group result = new InternalSys_Group(helper).GetGroupByUserId(userid);

                return result;

            }
        }

        public int DelGroupById(int groupid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalSys_Group(helper).DelGroupById(groupid);

                return list;
            }
        }

        public Sys_Group GetGroupByName(string groupname)
        {
            using (var helper = new SqlHelper())
            {


                Sys_Group result = new InternalSys_Group(helper).GetGroupByName(groupname);

                return result;

            }
        }
        /// <summary>
        ///   如果是门市 ，则显示门市经理 权限内的管理组列表
        ///如果是合作单位 ，则显示合作单位负责人  权限内的管理组列表
        ///如果是公司 ，根据登录账户角色判断其可以显示的管理组列表
        /// </summary>
        /// <param name="channelsource"></param>
        /// <returns></returns>
        public Sys_Group GetGroupBychannelsource( string channelsource,int userid=0)
        {
            using (var helper = new SqlHelper())
            {
                Sys_Group result = new InternalSys_Group(helper).GetGroupBychannelsource(  channelsource,userid);

                return result;

            }
        }
    }
}
