using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Model;

namespace ETS2.WeiXin.Service.WeiXinService.Data.InternalData
{
    public class InternalWxMenu
    {
        private SqlHelper sqlHelper;
        public InternalWxMenu(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal WxMenu GetWxMenu(int menuid)
        {
            string sql = @"SELECT  [menuid]
      ,[name]
      ,[instruction]
      ,[linkurl]
      ,[fathermenuid]
      ,[operationtypeid]
      ,[SalePromoteTypeid]
      ,[wxanswertext]
 ,[comid]
  FROM [WxMenu] where menuid=@menuid ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@menuid", menuid);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    WxMenu wxMenu = new WxMenu();
                    wxMenu.Menuid = reader.GetValue<int>("Menuid");
                    wxMenu.Name = reader.GetValue<string>("Name");
                    wxMenu.Instruction = reader.GetValue<string>("Instruction");
                    wxMenu.Linkurl = reader.GetValue<string>("Linkurl");
                    wxMenu.Fathermenuid = reader.GetValue<int>("Fathermenuid");
                    wxMenu.Operationtypeid = reader.GetValue<int>("Operationtypeid");
                    wxMenu.SalePromoteTypeid = reader.GetValue<int>("SalePromoteTypeid");
                    wxMenu.Wxanswertext = reader.GetValue<string>("Wxanswertext");
                    wxMenu.Comid = reader.GetValue<int>("comid");

                    reader.Close();

                    return wxMenu;
                }
                else
                {
                    return null;
                }
            }
        }
        internal WxMenu GetWxMenu(int menuid, int comid)
        {
            string sql = @"SELECT  [menuid]
      ,[name]
      ,[instruction]
      ,[linkurl]
      ,[fathermenuid]
      ,[operationtypeid]
      ,[SalePromoteTypeid]
      ,[wxanswertext]
 ,[comid]
,product_class
,keyy
,pictexttype 
  FROM [EtownDB].[dbo].[WxMenu] where menuid=@menuid and comid=@comid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@menuid", menuid);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    WxMenu wxMenu = new WxMenu();
                    wxMenu.Menuid = reader.GetValue<int>("Menuid");
                    wxMenu.Name = reader.GetValue<string>("Name");
                    wxMenu.Instruction = reader.GetValue<string>("Instruction");
                    wxMenu.Linkurl = reader.GetValue<string>("Linkurl");
                    wxMenu.Fathermenuid = reader.GetValue<int>("Fathermenuid");
                    wxMenu.Operationtypeid = reader.GetValue<int>("Operationtypeid");
                    wxMenu.SalePromoteTypeid = reader.GetValue<int>("SalePromoteTypeid");
                    wxMenu.Wxanswertext = reader.GetValue<string>("Wxanswertext");
                    wxMenu.Comid = reader.GetValue<int>("comid");
                    wxMenu.Product_class = reader.GetValue<int>("product_class");
                    wxMenu.Keyy = reader.GetValue<string>("keyy");
                    wxMenu.pictexttype = reader.GetValue<int>("pictexttype");

                    reader.Close();

                    return wxMenu;
                }
                else
                {
                    return null;
                }
            }
        }

        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateWxMenu";

        internal int EditWxMenu(WxMenu model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);

            cmd.AddParam("@Menuid", model.Menuid);
            cmd.AddParam("@Name", model.Name);
            cmd.AddParam("@Instruction", model.Instruction);
            cmd.AddParam("@Operationtypeid", model.Operationtypeid);
            cmd.AddParam("@Linkurl", model.Linkurl);
            cmd.AddParam("@Wxanswertext", model.Wxanswertext);
            cmd.AddParam("@Fathermenuid", model.Fathermenuid);
            cmd.AddParam("@SalePromoteTypeid", model.SalePromoteTypeid);
            cmd.AddParam("@Comid", model.Comid);
            cmd.AddParam("@ProductClass", model.Product_class);
            cmd.AddParam("@Keyy", model.Keyy);
            cmd.AddParam("@pictexttype", model.pictexttype);


            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        internal List<WxMenu> GetFristMenuList(int pageindex, int pagesize, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "WxMenu";
            var strGetFields = "*";
            var sortKey = "sortid";
            var sortMode = "0";

            var condition = "";
            condition += "fathermenuid=0  ";


            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<WxMenu> list = new List<WxMenu>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    WxMenu wxmenu = new WxMenu();

                    wxmenu.Menuid = reader.GetValue<int>("Menuid");
                    wxmenu.Name = reader.GetValue<string>("Name");
                    wxmenu.Instruction = reader.GetValue<string>("instruction");
                    wxmenu.Operationtypeid = reader.GetValue<int>("operationtypeid");
                    wxmenu.Linkurl = reader.GetValue<string>("linkurl");
                    wxmenu.Wxanswertext = reader.GetValue<string>("wxanswertext");
                    wxmenu.SalePromoteTypeid = reader.GetValue<int>("SalePromoteTypeid");
                    wxmenu.Fathermenuid = reader.GetValue<int>("fathermenuid");



                    list.Add(wxmenu);

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }

        internal List<WxMenu> GetFristMenuList(int pageindex, int pagesize, int comid, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "WxMenu";
            var strGetFields = "*";
            var sortKey = "sortid";
            var sortMode = "0";

            var condition = "";
            condition += "fathermenuid=0 and comid=" + comid;


            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<WxMenu> list = new List<WxMenu>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    WxMenu wxmenu = new WxMenu();

                    wxmenu.Menuid = reader.GetValue<int>("Menuid");
                    wxmenu.Name = reader.GetValue<string>("Name");
                    wxmenu.Instruction = reader.GetValue<string>("instruction");
                    wxmenu.Operationtypeid = reader.GetValue<int>("operationtypeid");
                    wxmenu.Linkurl = reader.GetValue<string>("linkurl");
                    wxmenu.Wxanswertext = reader.GetValue<string>("wxanswertext");
                    wxmenu.SalePromoteTypeid = reader.GetValue<int>("SalePromoteTypeid");
                    wxmenu.Fathermenuid = reader.GetValue<int>("fathermenuid");
                    wxmenu.Product_class = reader.GetValue<int>("product_class");


                    list.Add(wxmenu);

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }
        internal List<WxMenu> GetSecondMenuList(int fatherid, out int totalcount)
        {
            string sqlsecondlist = @"SELECT   [menuid]
      ,[name]
      ,[instruction]
      ,[linkurl]
      ,[fathermenuid]
      ,[operationtypeid]
      ,[SalePromoteTypeid]
      ,[wxanswertext]
 ,[comid]
  FROM [EtownDB].[dbo].[WxMenu] where fathermenuid=@fathermenuid   order by sortid ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlsecondlist);
            cmd.AddParam("@fathermenuid", fatherid);



            List<WxMenu> list = new List<WxMenu>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    WxMenu wxmenu = new WxMenu();

                    wxmenu.Menuid = reader.GetValue<int>("Menuid");
                    wxmenu.Name = reader.GetValue<string>("Name");
                    wxmenu.Instruction = reader.GetValue<string>("instruction");
                    wxmenu.Operationtypeid = reader.GetValue<int>("operationtypeid");
                    wxmenu.Linkurl = reader.GetValue<string>("linkurl");
                    wxmenu.Wxanswertext = reader.GetValue<string>("wxanswertext");
                    wxmenu.SalePromoteTypeid = reader.GetValue<int>("SalePromoteTypeid");
                    wxmenu.Fathermenuid = reader.GetValue<int>("fathermenuid");


                    list.Add(wxmenu);

                }
            }
            totalcount = list.Count;

            return list;
        }
        internal List<WxMenu> GetSecondMenuList(int fatherid, int comid, out int totalcount)
        {
            string sqlsecondlist = @"SELECT   [menuid]
      ,[name]
      ,[instruction]
      ,[linkurl]
      ,[fathermenuid]
      ,[operationtypeid]
      ,[SalePromoteTypeid]
      ,[wxanswertext]
      ,[comid]
     , product_class
FROM [EtownDB].[dbo].[WxMenu] where fathermenuid=@fathermenuid and comid=@comid order by sortid ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlsecondlist);
            cmd.AddParam("@fathermenuid", fatherid);
            cmd.AddParam("@comid", comid);


            List<WxMenu> list = new List<WxMenu>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    WxMenu wxmenu = new WxMenu();

                    wxmenu.Menuid = reader.GetValue<int>("Menuid");
                    wxmenu.Name = reader.GetValue<string>("Name");
                    wxmenu.Instruction = reader.GetValue<string>("instruction");
                    wxmenu.Operationtypeid = reader.GetValue<int>("operationtypeid");
                    wxmenu.Linkurl = reader.GetValue<string>("linkurl");
                    wxmenu.Wxanswertext = reader.GetValue<string>("wxanswertext");
                    wxmenu.SalePromoteTypeid = reader.GetValue<int>("SalePromoteTypeid");
                    wxmenu.Fathermenuid = reader.GetValue<int>("fathermenuid");
                    wxmenu.Product_class = reader.GetValue<int>("product_class");

                    list.Add(wxmenu);

                }
            }
            totalcount = list.Count;

            return list;
        }

        internal List<WxMenu> GetMenuList(out int totalcount)
        {
            string sqlsecondlist = @"SELECT   [menuid]
      ,[name]
      ,[instruction]
      ,[linkurl]
      ,[fathermenuid]
      ,[operationtypeid]
      ,[SalePromoteTypeid]
      ,[wxanswertext]
 ,[comid]
  FROM [EtownDB].[dbo].[WxMenu] ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlsecondlist);


            List<WxMenu> list = new List<WxMenu>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    WxMenu wxmenu = new WxMenu();

                    wxmenu.Menuid = reader.GetValue<int>("Menuid");
                    wxmenu.Name = reader.GetValue<string>("Name");
                    wxmenu.Instruction = reader.GetValue<string>("instruction");
                    wxmenu.Operationtypeid = reader.GetValue<int>("operationtypeid");
                    wxmenu.Linkurl = reader.GetValue<string>("linkurl");
                    wxmenu.Wxanswertext = reader.GetValue<string>("wxanswertext");
                    wxmenu.SalePromoteTypeid = reader.GetValue<int>("SalePromoteTypeid");
                    wxmenu.Fathermenuid = reader.GetValue<int>("fathermenuid");


                    list.Add(wxmenu);

                }
            }
            totalcount = list.Count;

            return list;
        }

        internal int DelChildrenMenu(int fathermenuid)
        {
            string sql = "delete WxMenu where fathermenuid =@fathermenuid ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@fathermenuid", fathermenuid);

            return cmd.ExecuteNonQuery();
        }
        internal List<WxMenu> GetMenuList(int fathermenuid, out int totalcount)
        {
            string sqlsecondlist = @"SELECT   [menuid]
      ,[name]
      ,[instruction]
      ,[linkurl]
      ,[fathermenuid]
      ,[operationtypeid]
      ,[SalePromoteTypeid]
      ,[wxanswertext]
 ,[comid]
  FROM [EtownDB].[dbo].[WxMenu]  where fathermenuid=" + fathermenuid + " order by sortid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlsecondlist);


            List<WxMenu> list = new List<WxMenu>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    WxMenu wxmenu = new WxMenu();

                    wxmenu.Menuid = reader.GetValue<int>("Menuid");
                    wxmenu.Name = reader.GetValue<string>("Name");
                    wxmenu.Instruction = reader.GetValue<string>("instruction");
                    wxmenu.Operationtypeid = reader.GetValue<int>("operationtypeid");
                    wxmenu.Linkurl = reader.GetValue<string>("linkurl");
                    wxmenu.Wxanswertext = reader.GetValue<string>("wxanswertext");
                    wxmenu.SalePromoteTypeid = reader.GetValue<int>("SalePromoteTypeid");
                    wxmenu.Fathermenuid = reader.GetValue<int>("fathermenuid");


                    list.Add(wxmenu);

                }
            }
            totalcount = list.Count;

            return list;
        }
        internal List<WxMenu> GetMenuList(int fathermenuid, int comid, out int totalcount)
        {
            string sqlsecondlist = @"SELECT   [menuid]
      ,[name]
      ,[instruction]
      ,[linkurl]
      ,[fathermenuid]
      ,[operationtypeid]
      ,[SalePromoteTypeid]
      ,[wxanswertext]
 ,[comid]
  FROM [EtownDB].[dbo].[WxMenu]  where fathermenuid=" + fathermenuid + " and comid=" + comid + " order by sortid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlsecondlist);


            List<WxMenu> list = new List<WxMenu>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    WxMenu wxmenu = new WxMenu();

                    wxmenu.Menuid = reader.GetValue<int>("Menuid");
                    wxmenu.Name = reader.GetValue<string>("Name");
                    wxmenu.Instruction = reader.GetValue<string>("instruction");
                    wxmenu.Operationtypeid = reader.GetValue<int>("operationtypeid");
                    wxmenu.Linkurl = reader.GetValue<string>("linkurl");
                    wxmenu.Wxanswertext = reader.GetValue<string>("wxanswertext");
                    wxmenu.SalePromoteTypeid = reader.GetValue<int>("SalePromoteTypeid");
                    wxmenu.Fathermenuid = reader.GetValue<int>("fathermenuid");


                    list.Add(wxmenu);

                }
            }
            totalcount = list.Count;

            return list;
        }

        internal int SortMenu(string menuid, int sortid)
        {
            string sql = "update WxMenu set sortid=@sortid where menuid =@menuid ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@menuid", menuid);
            cmd.AddParam("@sortid", sortid);

            return cmd.ExecuteNonQuery();
        }

        internal int EditMaterialType(int id, string typename, string typeclass, int comid, bool isshowpast)
        {
            string sql = "";
            if (id == 0)
            {
                sql = "insert into WxSalePromoteType(typename,typeclass,comid,isshowpast) values('" + typename + "','" + typeclass + "'," + comid + ",'" + isshowpast + "') ";
            }
            else
            {
                sql = "update WxSalePromoteType set typename='" + typename + "' ,typeclass='" + typeclass + "',comid=" + comid + ",isshowpast='" + isshowpast + "'  where id=" + id;
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);


            return cmd.ExecuteNonQuery();
        }


        internal WxMenu GetWxmenuByOperType(int opertype, int comid)
        {
            string sql = "select top 1 * from wxmenu  where operationtypeid=" + opertype + " and comid=" + comid;
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);


                using (var reader = cmd.ExecuteReader())
                {
                    WxMenu m = null;
                    if (reader.Read())
                    {
                        m = new WxMenu
                        {
                            Menuid = reader.GetValue<int>("Menuid"),
                            Name = reader.GetValue<string>("Name"),
                            Instruction = reader.GetValue<string>("instruction"),
                            Operationtypeid = reader.GetValue<int>("operationtypeid"),
                            Linkurl = reader.GetValue<string>("linkurl"),
                            Wxanswertext = reader.GetValue<string>("wxanswertext"),
                            SalePromoteTypeid = reader.GetValue<int>("SalePromoteTypeid"),
                            Fathermenuid = reader.GetValue<int>("fathermenuid") 
                        };
                    }
                    return m;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
