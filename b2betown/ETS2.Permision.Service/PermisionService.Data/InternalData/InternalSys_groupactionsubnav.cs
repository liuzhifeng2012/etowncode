using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.Permision.Service.PermisionService.Model;

namespace ETS2.Permision.Service.PermisionService.Data.InternalData
{
    public class InternalSys_groupactionsubnav
    {
        public SqlHelper sqlHelper;
        public InternalSys_groupactionsubnav(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal IList<Sys_groupactionsubnav> GetSys_groupactionsubnav(int actionid, string groupid)
        {
            string sql = @"SELECT [id]
                              ,[groupid]
                              ,[actionid]
                              ,[subnavid]
                          FROM  [sys_groupactionsubnav] where actionid=@actionid and groupid=@groupid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@actionid", actionid);
            cmd.AddParam("@groupid", groupid);
            using (var reader = cmd.ExecuteReader())
            {
                IList<Sys_groupactionsubnav> list = new List<Sys_groupactionsubnav>();
                while (reader.Read())
                {
                    list.Add(new Sys_groupactionsubnav
                    {
                        id = reader.GetValue<int>("id"),
                        actionid = reader.GetValue<int>("actionid"),
                        groupid = reader.GetValue<int>("groupid"),
                        subnavid = reader.GetValue<int>("subnavid"),
                    });
                }
                return list;
            }

        }

        internal string GetGroupsByactionsubnavid(int actionid, int subnavid)
        {
            string sql = "select groupid from sys_groupactionsubnav where actionid=" + actionid + " and subnavid=" + subnavid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                string groupids = "";
                while (reader.Read())
                {
                    groupids += reader.GetValue<int>("groupid");
                }
                if(groupids.Length>0)
                {
                    groupids = groupids.Substring(0,groupids.Length-1);
                }
                return groupids;
            }
        }
    }
}
