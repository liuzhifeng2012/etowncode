using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.Permision.Service.PermisionService.Model;

namespace ETS2.Permision.Service.PermisionService.Data.InternalData
{
    public class InternalSys_Group
    {
        private SqlHelper sqlHelper;
        public InternalSys_Group(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal List<Sys_Group> GroupPageList(int pageindex, int pagesize, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "Sys_Group";
            var strGetFields = "*";
            var sortKey = "groupid";
            var sortMode = "0";

            var condition = "";


            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);

            List<Sys_Group> list = new List<Sys_Group>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Sys_Group
                    {
                        Groupid = reader.GetValue<int>("Groupid"),
                        Groupname = reader.GetValue<string>("Groupname"),
                        Groupinfo = reader.GetValue<string>("Groupinfo"),
                        Masterid = reader.GetValue<int>("Masterid"),
                        Mastername = reader.GetValue<string>("Mastername"),
                        Createdate = reader.GetValue<DateTime>("Createdate"),


                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }

        internal Sys_Group GetGroupById(int groupid)
        {
            const string sqltxt = @"SELECT [groupid]
      ,[groupname]
      ,[groupinfo]
      ,[masterid]
      ,[mastername]
      ,[createdate]
      ,[isviewchannel]
      ,[viewgroupids]
      ,CrmIsAccurateToPerson
      ,iscanverify
      , iscanset_imprest
      , iscanset_order
      ,OrderIsAccurateToPerson
      ,validateservertype
      ,canviewpro 
  FROM  [Sys_Group] where groupid=@groupid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@groupid", groupid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Sys_Group
                    {
                        Groupid = reader.GetValue<int>("groupid"),
                        Groupinfo = reader.GetValue<string>("groupinfo"),
                        Groupname = reader.GetValue<string>("groupname"),
                        Isviewchannel = reader.GetValue<bool>("isviewchannel"),
                        Groupids = reader.GetValue<string>("viewgroupids"),
                        CrmIsAccurateToPerson = reader.GetValue<bool>("CrmIsAccurateToPerson"),
                        OrderIsAccurateToPerson = reader.GetValue<int>("OrderIsAccurateToPerson"),
                        Iscanverify=reader.GetValue<int>("iscanverify"),
                        iscanset_order = reader.GetValue<int>("iscanset_order"),
                        iscanset_imprest = reader.GetValue<int>("iscanset_imprest"),
                        validateservertype = reader.GetValue<int>("validateservertype"),
                        canviewpro = reader.GetValue<int>("canviewpro")
                    }; 
                }
                return null;
            }
        }
        #region 编辑管理组
        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateSysGroup";
        internal int EditGroup(Sys_Group model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);

            cmd.AddParam("@Groupid", model.Groupid);
            cmd.AddParam("@Groupname", model.Groupname);
            cmd.AddParam("@Groupinfo", model.Groupinfo);
            cmd.AddParam("@Masterid", model.Masterid);
            cmd.AddParam("@Mastername", model.Mastername);
            cmd.AddParam("@Createdate", model.Createdate);

            cmd.AddParam("@Groupids", model.Groupids);
            cmd.AddParam("@Isviewchannel", model.Isviewchannel);
            cmd.AddParam("@CrmIsAccurateToPerson", model.CrmIsAccurateToPerson);
            cmd.AddParam("@OrderIsAccurateToPerson", model.OrderIsAccurateToPerson);

            cmd.AddParam("@iscanverify",model.Iscanverify);
            cmd.AddParam("@iscanset_imprest", model.iscanset_imprest);
            cmd.AddParam("@iscanset_order", model.iscanset_order);
            cmd.AddParam("@validateservertype", model.validateservertype);
            cmd.AddParam("@canviewpro", model.canviewpro);
            

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion

        internal List<Sys_Group> GetAllGroups(out int totalcount)
        {



            var cmd = sqlHelper.PrepareTextSqlCommand("SELECT [groupid]      ,[groupname]      ,[groupinfo]      ,[masterid]      ,[mastername]      ,[createdate]  FROM [EtownDB].[dbo].[Sys_Group]");
            List<Sys_Group> list = new List<Sys_Group>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Sys_Group
                    {
                        Groupid = reader.GetValue<int>("Groupid"),
                        Groupname = reader.GetValue<string>("Groupname"),
                        Groupinfo = reader.GetValue<string>("Groupinfo"),
                        Masterid = reader.GetValue<int>("Masterid"),
                        Mastername = reader.GetValue<string>("Mastername"),
                        Createdate = reader.GetValue<DateTime>("Createdate"),


                    });

                }
            }
            totalcount = list.Count;

            return list;
        }

        internal Sys_Group GetGroupByUserId(int userid)
        {
            const string sqltxt = @"SELECT [groupid]
      ,[groupname]
      ,[groupinfo]
      ,[masterid]
      ,[mastername]
      ,[createdate]
      ,[isviewchannel]
      ,[viewgroupids]
      ,CrmIsAccurateToPerson
     ,iscanverify
     ,iscanset_imprest
     ,iscanset_order
     ,OrderIsAccurateToPerson
     ,validateservertype
     ,canviewpro
  FROM  [Sys_Group] where groupid in (select groupid from Sys_MasterGroup where masterid=@masterid)";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@masterid", userid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Sys_Group
                    {
                        Groupid = reader.GetValue<int>("groupid"),
                        Groupinfo = reader.GetValue<string>("groupinfo"),
                        Groupname = reader.GetValue<string>("groupname"),
                        Isviewchannel = reader.GetValue<bool>("isviewchannel"),
                        Groupids = reader.GetValue<string>("viewgroupids"),
                        CrmIsAccurateToPerson = reader.GetValue<bool>("crmIsAccurateToPerson"),
                        OrderIsAccurateToPerson = reader.GetValue<int>("OrderIsAccurateToPerson"),
                        Iscanverify=reader.GetValue<int>("iscanverify"),
                        iscanset_imprest = reader.GetValue<int>("iscanset_imprest"),
                        iscanset_order = reader.GetValue<int>("iscanset_order"),
                        validateservertype = reader.GetValue<int>("validateservertype"),
                        canviewpro = reader.GetValue<int>("canviewpro"),
                    };
                }
                return null;
            }
        }

        internal int DelGroupById(int groupid)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand("usp_DelGroupById");
            cmd.AddParam("@GroupId", groupid);


            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }

        internal Sys_Group GetGroupByName(string groupname)
        {
            const string sqltxt = @"SELECT [groupid]
      ,[groupname]
      ,[groupinfo]
      ,[masterid]
      ,[mastername]
      ,[createdate]
      ,[isviewchannel]
      ,[viewgroupids]
      ,CrmIsAccurateToPerson
  FROM [EtownDB].[dbo].[Sys_Group] where groupname=@groupname ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@groupname", groupname);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Sys_Group
                    {
                        Groupid = reader.GetValue<int>("groupid"),
                        Groupinfo = reader.GetValue<string>("groupinfo"),
                        Groupname = reader.GetValue<string>("groupname"),
                        Isviewchannel = reader.GetValue<bool>("isviewchannel"),
                        Groupids = reader.GetValue<string>("viewgroupids"),
                        CrmIsAccurateToPerson = reader.GetValue<bool>("CrmIsAccurateToPerson")
                    };
                }
                return null;
            }
        }

        internal Sys_Group GetGroupBychannelsource(  string channelsource,int userid=0)
        {
              string sqltxt = "SELECT * FROM [EtownDB].[dbo].[Sys_Group] where  1=1 ";
          
            if (channelsource == "0")
            {
                sqltxt+=" and groupname = '门店经理' ";
            }
            else if (channelsource == "1")
            { 
                sqltxt += " and groupname = '合作单位负责人' ";
            }
            else 
            {
                if (userid == 0)
                { 
                    sqltxt += " and groupname = '管理员' ";
                }
                else 
                {
                    sqltxt += " and groupid in (select groupid from Sys_MasterGroup where masterid="+userid+")";
                }
            }
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Sys_Group
                    {
                        Groupid = reader.GetValue<int>("groupid"),
                        Groupinfo = reader.GetValue<string>("groupinfo"),
                        Groupname = reader.GetValue<string>("groupname"),
                        Isviewchannel = reader.GetValue<bool>("isviewchannel"),
                        Groupids = reader.GetValue<string>("viewgroupids"),
                        CrmIsAccurateToPerson = reader.GetValue<bool>("crmIsAccurateToPerson"),
                        Iscanverify = reader.GetValue<int>("iscanverify")
                    };
                }
                return null;
            }
        }
    }
}
