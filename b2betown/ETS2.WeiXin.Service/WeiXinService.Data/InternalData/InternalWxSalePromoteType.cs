using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Model;

namespace ETS2.WeiXin.Service.WeiXinService.Data.InternalData
{
    public class InternalWxSalePromoteType
    {
        private SqlHelper sqlHelper;
        public InternalWxSalePromoteType(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        //查询类型名
        internal WxSalePromoteType GetWsptMenu(int id)
        {
            string sql = @"SELECT [id]
      ,[typename]
  FROM [EtownDB].[dbo].[WxSalePromoteType] where id=@id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    WxSalePromoteType wxMenu = new WxSalePromoteType();
                    wxMenu.Id = reader.GetValue<int>("Id");
                    wxMenu.Typename = reader.GetValue<string>("Typename");

                    return wxMenu;
                }
                else
                {
                    return null;
                }
            }
        }

        internal List<WxSalePromoteType> Wxmaterialtypepagelist(int pageindex, int pagesize, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "WxSalePromoteType";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "0";

            var condition = "";


            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<WxSalePromoteType> list = new List<WxSalePromoteType>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    WxSalePromoteType wxmaterial = new WxSalePromoteType();

                    wxmaterial.Id = reader.GetValue<int>("id");
                    wxmaterial.Typename = reader.GetValue<string>("typename");
                    wxmaterial.Typeclass = reader.GetValue<string>("typeclass") == null ? "detail" : reader.GetValue<string>("typeclass");




                    list.Add(wxmaterial);

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }

        internal List<WxSalePromoteType> Wxmaterialtypepagelist(int pageindex, int pagesize, int comid, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "WxSalePromoteType";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "0";

            var condition = " comid =" + comid;


            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<WxSalePromoteType> list = new List<WxSalePromoteType>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    WxSalePromoteType wxmaterial = new WxSalePromoteType();

                    wxmaterial.Id = reader.GetValue<int>("id");
                    wxmaterial.Typename = reader.GetValue<string>("typename");
                    wxmaterial.Isshowpast = reader.GetValue<bool>("isshowpast");
                    wxmaterial.Typeclass = reader.GetValue<string>("typeclass") == null ? "detail" : reader.GetValue<string>("typeclass");




                    list.Add(wxmaterial);

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }
        internal WxSalePromoteType GetMaterialType(int id)
        {
            string sql = @"SELECT [id]
      ,[typename]
     ,[typeclass]
  FROM [EtownDB].[dbo].[WxSalePromoteType] where id=@id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    WxSalePromoteType wxMenu = new WxSalePromoteType();
                    wxMenu.Id = reader.GetValue<int>("Id");
                    wxMenu.Typename = reader.GetValue<string>("Typename");
                    wxMenu.Typeclass = reader.GetValue<string>("typeclass") == null ? "detail" : reader.GetValue<string>("typeclass");

                    return wxMenu;
                }
                else
                {
                    return null;
                }
            }
        }
        internal WxSalePromoteType GetMaterialType(int id, int comid)
        {
            string sql = @"SELECT [id]
      ,[typename]
     ,[typeclass]
     ,isshowpast
  FROM [EtownDB].[dbo].[WxSalePromoteType] where id=@id and comid=@comid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);
            cmd.AddParam("@comid", comid);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    WxSalePromoteType wxMenu = new WxSalePromoteType();
                    wxMenu.Id = reader.GetValue<int>("Id");
                    wxMenu.Typename = reader.GetValue<string>("Typename");
                    wxMenu.Isshowpast = reader.GetValue<bool>("isshowpast");
                    wxMenu.Typeclass = reader.GetValue<string>("typeclass") == null ? "detail" : reader.GetValue<string>("typeclass");

                    return wxMenu;
                }
                else
                {
                    return null;
                }
            }
        }
        internal List<WxSalePromoteType> GetAllWxMaterialType(int comid, out int totalcount)
        {
            string sql = "select id,typename,typeclass from WxSalePromoteType where comid=@comid ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);

            List<WxSalePromoteType> list = new List<WxSalePromoteType>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    WxSalePromoteType wxmaterial = new WxSalePromoteType();

                    wxmaterial.Id = reader.GetValue<int>("id");
                    wxmaterial.Typename = reader.GetValue<string>("typename");
                    wxmaterial.Typeclass = reader.GetValue<string>("typeclass") == null ? "detail" : reader.GetValue<string>("typeclass");




                    list.Add(wxmaterial);

                }
            }
            totalcount = list.Count;

            return list;
        }
        internal List<WxSalePromoteType> GetAllWxMaterialType(out int totalcount)
        {
            string sql = "select id,typename,typeclass from WxSalePromoteType ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            List<WxSalePromoteType> list = new List<WxSalePromoteType>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    WxSalePromoteType wxmaterial = new WxSalePromoteType();

                    wxmaterial.Id = reader.GetValue<int>("id");
                    wxmaterial.Typename = reader.GetValue<string>("typename");
                    wxmaterial.Typeclass = reader.GetValue<string>("typeclass") == null ? "detail" : reader.GetValue<string>("typeclass");




                    list.Add(wxmaterial);

                }
            }
            totalcount = list.Count;

            return list;
        }

        internal List<WxSalePromoteType> GetRecommendWxMaterialType(int comid, out int totalcount)
        {
            string sql = "select id,typename,typeclass from WxSalePromoteType where comid=" + comid + " and  id in (select salepromotetypeid from WxMenu where comid=" + comid + " and salepromotetypeid>0)";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            List<WxSalePromoteType> list = new List<WxSalePromoteType>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    WxSalePromoteType wxmaterial = new WxSalePromoteType();

                    wxmaterial.Id = reader.GetValue<int>("id");
                    wxmaterial.Typename = reader.GetValue<string>("typename");
                    wxmaterial.Typeclass = reader.GetValue<string>("typeclass") == null ? "detail" : reader.GetValue<string>("typeclass");




                    list.Add(wxmaterial);

                }
            }
            totalcount = list.Count;

            return list;
        }
    }
}
