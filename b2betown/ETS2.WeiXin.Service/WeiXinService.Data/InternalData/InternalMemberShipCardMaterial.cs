using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Model;

namespace ETS2.WeiXin.Service.WeiXinService.Data.InternalData
{
    public class InternalMemberShipCardMaterial
    {
        private SqlHelper sqlHelper;
        public InternalMemberShipCardMaterial(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        //插入或修改渠道信息
        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateMemberShipMaterial";

        internal int EditMaterial(MemberShipCardMaterial model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);

            cmd.AddParam("@MaterialId", model.MaterialId);
            cmd.AddParam("@Title", model.Title);

            cmd.AddParam("@Imgpath", model.Imgpath);
            cmd.AddParam("@Summary", model.Summary);
            cmd.AddParam("@Article", model.Article);
            cmd.AddParam("@ApplyState", model.Applystate);
            cmd.AddParam("@Phone", model.Phone);
            cmd.AddParam("@Price", model.Price);
            cmd.AddParam("@comid", model.Comid);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        internal MemberShipCardMaterial GetMembershipcardMaterial( int materialid)
        {
            string sql = @"SELECT  [MaterialId]
      ,[title]
      
      ,[imgpath]
      ,[summary]
      ,[article]
      ,[applystate]
      ,[phone]
      ,[price]
      ,[sortid]
      ,[comid]
  FROM [EtownDB].[dbo].[MemberShipCardMaterial] where MaterialId=" + materialid ;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    MemberShipCardMaterial material = new MemberShipCardMaterial();
                    material.MaterialId = reader.GetValue<int>("MaterialId");
                    material.Title = reader.GetValue<string>("title");

                    material.Imgpath = reader.GetValue<string>("imgpath");
                    material.Summary = reader.GetValue<string>("summary");
                    material.Article = reader.GetValue<string>("article");

                    material.Applystate = reader.GetValue<bool>("applystate");

                    material.Phone = reader.GetValue<string>("phone") == null ? "" : reader.GetValue<string>("phone");
                    material.Price = reader.GetValue<decimal>("price");
                    material.Comid = reader.GetValue<int>("comid");

                    reader.Close();


                    return material;
                }
                else
                {
                    return null;
                }
            }


        }
        internal MemberShipCardMaterial GetMembershipcardMaterial(int comid, int materialid)
        {
            string sql = @"SELECT  [MaterialId]
      ,[title]
      
      ,[imgpath]
      ,[summary]
      ,[article]
      ,[applystate]
      ,[phone]
  ,[price]
      ,[sortid]
      ,[comid]
  FROM [EtownDB].[dbo].[MemberShipCardMaterial] where MaterialId=" + materialid + " and comid=" + comid;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    MemberShipCardMaterial material = new MemberShipCardMaterial();
                    material.MaterialId = reader.GetValue<int>("MaterialId");
                    material.Title = reader.GetValue<string>("title");

                    material.Imgpath = reader.GetValue<string>("imgpath");
                    material.Summary = reader.GetValue<string>("summary");
                    material.Article = reader.GetValue<string>("article");

                    material.Applystate = reader.GetValue<bool>("applystate");

                    material.Phone = reader.GetValue<string>("phone") == null ? "" : reader.GetValue<string>("phone");
                    material.Price = reader.GetValue<decimal>("price");
                    material.Comid = reader.GetValue<int>("comid");

                    reader.Close();


                    return material;
                }
                else
                {
                    return null;
                }
            }


        }

        internal List<MemberShipCardMaterial> Membershipcardpagelist(int comid, int pageindex, int pagesize, bool applystate, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "MemberShipCardMaterial";
            var strGetFields = "*";
            var sortKey = "sortid";
            var sortMode = "0";

            var condition = " comid=" + comid;


            condition += " and applystate='" + applystate+"'";



            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<MemberShipCardMaterial> list = new List<MemberShipCardMaterial>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    MemberShipCardMaterial model = new MemberShipCardMaterial();

                    model.MaterialId = reader.GetValue<int>("MaterialId");
                    model.Title = reader.GetValue<string>("title");
                    model.Applystate = reader.GetValue<bool>("applystate") ;
                    model.Article = reader.GetValue<string>("article");
                  
                    model.Imgpath = reader.GetValue<string>("imgpath");

                
                    model.Summary = reader.GetValue<string>("Summary");

                    model.Phone = reader.GetValue<string>("phone");
                    model.Price = reader.GetValue<decimal>("price");

                    list.Add(model);

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }
        internal List<MemberShipCardMaterial> AllMembershipcardpagelist(int comid, int pageindex, int pagesize, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "MemberShipCardMaterial";
            var strGetFields = "*";
            var sortKey = "sortid";
            var sortMode = "0";

            var condition = " comid=" + comid;
 


            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<MemberShipCardMaterial> list = new List<MemberShipCardMaterial>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    MemberShipCardMaterial model = new MemberShipCardMaterial();

                    model.MaterialId = reader.GetValue<int>("MaterialId");
                    model.Title = reader.GetValue<string>("title");
                    model.Applystate = reader.GetValue<bool>("applystate");
                    model.Article = reader.GetValue<string>("article");

                    model.Imgpath = reader.GetValue<string>("imgpath");


                    model.Summary = reader.GetValue<string>("Summary");

                    model.Phone = reader.GetValue<string>("phone");
                    model.Price = reader.GetValue<decimal>("price");

                    list.Add(model);

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }
        internal int DelMemberShipCardMaterial(int materialid)
        {
            string sql = "delete MemberShipCardMaterial where MaterialId=@MaterialId ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@MaterialId", materialid);

            return cmd.ExecuteNonQuery();
        }
        internal int SortMaterial(string materialid, int sortid)
        {
            string sql = "update MemberShipCardMaterial set sortid=@sortid where materialid =@materialid ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@materialid", materialid);
            cmd.AddParam("@sortid", sortid);

            return cmd.ExecuteNonQuery();
        }

        internal List<MemberShipCardMaterial> GetMCMateralListByComId(int comid,out int totalcount)
        {
            string sql = @"SELECT [MaterialId]
      ,[title]
      ,[imgpath]
      ,[summary]
      ,[article]
      ,[applystate]
      ,[phone]
      ,[sortid]
      ,[comid]
      ,[price]
  FROM [EtownDB].[dbo].[MemberShipCardMaterial] where comid=@comid and applystate=1";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid",comid);

            List<MemberShipCardMaterial> list = new List<MemberShipCardMaterial>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    MemberShipCardMaterial model = new MemberShipCardMaterial();

                    model.MaterialId = reader.GetValue<int>("MaterialId");
                    model.Title = reader.GetValue<string>("title");
                    model.Applystate = reader.GetValue<bool>("applystate");
                    model.Article = reader.GetValue<string>("article");

                    model.Imgpath = reader.GetValue<string>("imgpath");


                    model.Summary = reader.GetValue<string>("Summary");

                    model.Phone = reader.GetValue<string>("phone");
                    model.Price = reader.GetValue<decimal>("price");

                    list.Add(model);

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }
    }
}
