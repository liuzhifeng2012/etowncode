using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS.Data.SqlHelper;
using ETS.Framework;

namespace ETS2.WeiXin.Service.WeiXinService.Data.InternalData
{
    public class InternalWeixin_template
    {
        private SqlHelper sqlHelper;
        public InternalWeixin_template(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal Weixin_template GetWeixinTmpl(int comid, string infotype)
        {
            string sql = @"SELECT [id]
      ,[comid]
      ,[infotype]
      ,[template_id]
      ,[template_name]
      ,[first_DATA]
      ,[remark_DATA]
  FROM [EtownDB].[dbo].[weixin_template] where comid=@comid and infotype=@infotype";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@infotype", infotype);

            using (var reader = cmd.ExecuteReader())
            {
                Weixin_template m = null;
                if (reader.Read())
                {
                    m = new Weixin_template
                    {
                        Id = reader.GetValue<int>("id"),
                        Comid = reader.GetValue<int>("comid"),
                        Infotype = reader.GetValue<string>("infotype"),
                        Template_id = reader.GetValue<string>("template_id"),
                        Template_name = reader.GetValue<string>("template_name"),
                        First_DATA = reader.GetValue<string>("first_data"),
                        Remark_DATA = reader.GetValue<string>("remark_data")
                    };
                }
                return m;
            }

        }
        internal List<Weixin_template> Templatemodelpagelist(int pageindex, int pagesize, out int totalcount)
        {

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = "";

            cmd.PagingCommand1("weixin_template_model", "*", "id desc", "", pagesize, pageindex, "", condition);

            List<Weixin_template> list = new List<Weixin_template>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Weixin_template
                    {
                        Id = reader.GetValue<int>("id"),
                        Template_id = reader.GetValue<string>("Template_id"),
                        First_DATA = reader.GetValue<string>("First_DATA"),
                        Remark_DATA = reader.GetValue<string>("Remark_DATA"),
                        Infotype = reader.GetValue<string>("Infotype"),
                        Template_name = reader.GetValue<string>("Template_name")
                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }



        internal List<Weixin_template> Templatecompagelist(int comid, out int totalcount)
        {

            string sql = @"SELECT * FROM [weixin_template] where comid=@comid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);
            int i = 0;
            List<Weixin_template> list = new List<Weixin_template>();
            using (var reader = cmd.ExecuteReader())
            {
                
                while (reader.Read())
                {
                    list.Add(new Weixin_template
                    {
                        Id = reader.GetValue<int>("id"),
                        Template_id = reader.GetValue<string>("Template_id"),
                        First_DATA = reader.GetValue<string>("First_DATA"),
                        Remark_DATA = reader.GetValue<string>("Remark_DATA"),
                        Infotype = reader.GetValue<string>("Infotype"),
                        Template_name = reader.GetValue<string>("Template_name")
                    });
                    i++;
                }
            }
            totalcount = i;
            return list;
        }


        


        internal Weixin_template Templatemodelinfo(int id)
        {
            string sql = @"SELECT * FROM [weixin_template_model] where id=@id";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);

            using (var reader = cmd.ExecuteReader())
            {
                Weixin_template m = null;
                if (reader.Read())
                {
                    m = new Weixin_template
                    {
                        Id = reader.GetValue<int>("id"),
                        Template_id = reader.GetValue<string>("Template_id"),
                        First_DATA = reader.GetValue<string>("First_DATA"),
                        Remark_DATA = reader.GetValue<string>("Remark_DATA"),
                        Infotype = reader.GetValue<string>("Infotype"),
                        Template_name = reader.GetValue<string>("Template_name")
                    };
                }
                return m;
            }
        }

        internal int Templatemodeledit(int id, int infotype, string template_name, string first_DATA, string remark_DATA)
        {
            string sql = "update weixin_template_model set template_name=@template_name,first_DATA=@first_DATA,remark_DATA=@remark_DATA where id=@id ";
            if (id == 0) {
                sql = "insert weixin_template_model (infotype,template_name,first_DATA,remark_DATA) values(@infotype,@template_name,@first_DATA,@remark_DATA) ";
            }


            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);
            cmd.AddParam("@infotype", infotype);
            cmd.AddParam("@template_name", template_name);
            cmd.AddParam("@first_DATA", first_DATA);
            cmd.AddParam("@remark_DATA", remark_DATA);

            return cmd.ExecuteNonQuery();
        }

        internal int Templateedit(int comid,string id, string template_id)
        {
            string sql = "update weixin_template set template_id=@template_id where id=@id and comid=@comid ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@template_id", template_id);
            return cmd.ExecuteNonQuery();
        }


        internal int Templatecominsert(int comid)
        {

            string sql = "insert into  weixin_template (comid,template_id,infotype,template_name,first_DATA,remark_DATA) select " + comid + ",'',infotype,template_name,first_DATA,remark_DATA from weixin_template_model  where not infotype in (select infotype from weixin_template where comid=" + comid + ") ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }
        

    }
}
