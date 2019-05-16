using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;
using System.Data;
using System.Data.SqlClient;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalB2bModel
    {
        private SqlHelper sqlHelper;
        public InternalB2bModel(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }


        #region 模板列表
        internal List<B2b_model> ModelPageList(int pageindex, int pagesize, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "b2b_Model";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";
            var condition = "";
            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<B2b_model> list = new List<B2b_model>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_model
                    {
                        Id = reader.GetValue<int>("id"),
                        Title = reader.GetValue<string>("title"),
                        Daohangnum = reader.GetValue<int>("daohangnum"),
                        Daohangimg = reader.GetValue<int>("daohangimg"),
                        Style_str = reader.GetValue<string>("style_str"),
                        Html_str = reader.GetValue<string>("html_str"),
                        Bgimage = reader.GetValue<int>("bgimage"),
                        Smallimg = reader.GetValue<int>("Smallimg"),
                        Bgimage_w = reader.GetValue<int>("bgimage_w"),
                        Bgimage_h = reader.GetValue<int>("bgimage_h")
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;

        }
        #endregion


        internal B2b_model GetModelById(int modelid)
        {
            const string sqltxt = @"SELECT  *

  FROM b2b_Model where [Id]=@modelid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@modelid", modelid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_model
                        {
                            Id = reader.GetValue<int>("id"),
                            Title = reader.GetValue<string>("title"),
                            Daohangnum = reader.GetValue<int>("daohangnum"),
                            Daohangimg = reader.GetValue<int>("daohangimg"),
                            Style_str = reader.GetValue<string>("style_str"),
                            Html_str = reader.GetValue<string>("html_str"),
                            Bgimage = reader.GetValue<int>("bgimage"),
                            Smallimg = reader.GetValue<int>("Smallimg"),
                            Bgimage_w = reader.GetValue<int>("bgimage_w"),
                            Bgimage_h = reader.GetValue<int>("bgimage_h")
                        };
                }
                return null;
            }
        }



        #region 添加或者编辑模板

        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateB2bModel";

        public int ModelInsertOrUpdate(B2b_model model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);

            cmd.AddParam("@id", model.Id);
            cmd.AddParam("@Daohangnum", model.Daohangnum);
            cmd.AddParam("@Daohangimg", model.Daohangimg);
            cmd.AddParam("@Title", model.Title);
            cmd.AddParam("@Style_str", model.Style_str);
            cmd.AddParam("@Html_str", model.Html_str);
            cmd.AddParam("@Bgimage", model.Bgimage);
            cmd.AddParam("@Smallimg", model.Smallimg);
            cmd.AddParam("@Bgimage_h", model.Bgimage_h);
            cmd.AddParam("@Bgimage_w", model.Bgimage_w);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;

        }
        #endregion


        #region 选择模板
        public int SelectModel(H5_html model)
        {
            const string sqlTxt = "insert h5html (html_str,style_str,js_str,modelid,comid)values(@html_str,@style_str,'',@modelid,@comid)";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@modelid",model.Modelid);
            cmd.AddParam("@comid", model.Comid);
            cmd.AddParam("@html_str", model.Html_str);
            cmd.AddParam("@style_str", model.Style_str);
            return cmd.ExecuteNonQuery();
        }
        #endregion

        #region 删除已选择模板
        public int DeleteSelectModel(int comid)
        {
            const string sqlTxt = "delete h5html where comid=@comid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@comid", comid);
            return cmd.ExecuteNonQuery();
        }
        #endregion



        #region 插入选择模板菜单
        public int InsertSelectModelMenu(int modelid,int comid)
        {
            const string sqlTxt = "insert h5menu (name,imgurl,linkurl,fonticon,sortid,linktype,comid)  select name,imgurl,linkurl,fonticon,sortid,linktype,@comid from b2b_Modelmenu where modelid=@modelid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@modelid", modelid);
            return cmd.ExecuteNonQuery();
        }
        #endregion



        #region 删除已选择模板
        public int DeleteSelectModelMenu(int comid)
        {
            const string sqlTxt = "delete h5menu where comid=@comid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@comid", comid);
            return cmd.ExecuteNonQuery();
        }
        #endregion


        #region 选择模板
        public H5_html SelectModelSearchComid(int Comid)
        {
            const string sqltxt = @"SELECT  *  FROM h5html where [COMId]=@Comid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@Comid", Comid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new H5_html
                    {
                        Id = reader.GetValue<int>("id"),
                        Style_str = reader.GetValue<string>("style_str"),
                        Html_str = reader.GetValue<string>("html_str"),
                        Modelid = reader.GetValue<int>("Modelid"),
                        Comid = reader.GetValue<int>("Comid")
                    };
                }
                return null;
            }
        }
        #endregion

        #region 判断此模板是否已选用
        public int UseComidModel(int Comid,int modelid)
        {
            const string sqltxt = @"SELECT  *  FROM h5html where [COMId]=@Comid and modelid=@modelid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@Comid", Comid);
            cmd.AddParam("@modelid", modelid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return 1;
                }
                return 0;
            }
        }
        #endregion

        


        #region 添加或者编辑模板菜单

        private static readonly string SQLMenuInsertOrUpdate = "usp_InsertOrUpdateB2bModelMenu";

        public int ModelMenuInsertOrUpdate(B2b_modelmenu model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLMenuInsertOrUpdate);

            cmd.AddParam("@id", model.Id);
            cmd.AddParam("@Modelid", model.Modelid);
            cmd.AddParam("@Name", model.Name);
            cmd.AddParam("@Linkurl", model.Linkurl);
            cmd.AddParam("@Linktype", model.Linktype);
            cmd.AddParam("@Imgurl", model.Imgurl);
            cmd.AddParam("@Sortid", model.Sortid);
            cmd.AddParam("@Fonticon", model.Fonticon);
            

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;

        }
        #endregion


        #region 模板菜单列表
        internal List<B2b_modelmenu> ModelMenuPageList(int modelid,int pageindex, int pagesize, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "b2b_ModelMenu";
            var strGetFields = "*";
            var sortKey = "sortid";
            var sortMode = "0";
            var condition = " modelid=" + modelid;
            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<B2b_modelmenu> list = new List<B2b_modelmenu>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_modelmenu
                    {
                        Id = reader.GetValue<int>("id"),
                        Name = reader.GetValue<string>("Name"),
                        Linkurl = reader.GetValue<string>("Linkurl"),
                        Imgurl = reader.GetValue<int>("Imgurl"),
                        Modelid = reader.GetValue<int>("Modelid"),
                        Sortid = reader.GetValue<int>("Sortid"),
                        Fonticon = reader.GetValue<string>("Fonticon"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;

        }
        #endregion

        #region 模板菜单列表
        internal List<B2b_modelmenu> ModelzhidingPageList(int id,int pageindex, int pagesize, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "b2b_model_zhiding";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";
            var condition = " ";
            if (id != 0) {
                condition = " id="+id;
            }

            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<B2b_modelmenu> list = new List<B2b_modelmenu>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_modelmenu
                    {
                        Id = reader.GetValue<int>("id"),
                        Name = reader.GetValue<string>("Name"),
                        Linkurl = reader.GetValue<string>("Linkurl")
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;

        }
        #endregion


        internal B2b_modelmenu GetModelMenuById(int id)
        {
            const string sqltxt = @"SELECT  *

  FROM b2b_Modelmenu where [Id]=@id ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@id", id);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_modelmenu
                    {
                        Id = reader.GetValue<int>("id"),
                        Name = reader.GetValue<string>("Name"),
                        Linkurl = reader.GetValue<string>("Linkurl"),
                        Imgurl = reader.GetValue<int>("Imgurl"),
                        Modelid = reader.GetValue<int>("Modelid"),
                        Sortid = reader.GetValue<int>("Sortid"),
                        Fonticon = reader.GetValue<string>("Fonticon"),
                    };
                }
                return null;
            }
        }





        internal H5_html GetComModel(int comid)
        {
            const string sqltxt = @"SELECT  *

  FROM H5Html where [comId]=@comid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new H5_html
                    {
                        Id = reader.GetValue<int>("id"),
                        Comid = reader.GetValue<int>("Comid"),
                        Html_str = reader.GetValue<string>("Html_str"),
                        Style_str = reader.GetValue<string>("Style_str"),
                        Modelid = reader.GetValue<int>("Modelid"),
                    };
                }
                return null;
            }
        }


        //排序
        internal int ModelMenuSort(string id, int sortid)
        {
            string sql = "update b2b_Modelmenu set sortid=@sortid where id =@id ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);
            cmd.AddParam("@sortid", sortid);

            return cmd.ExecuteNonQuery();
        }

    }
}
