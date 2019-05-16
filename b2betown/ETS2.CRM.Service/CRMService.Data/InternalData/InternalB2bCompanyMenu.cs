using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;
using System.Data;

namespace ETS2.CRM.Service.CRMService.Data.InternalData
{
    class InternalB2bCompanyMenu
    {
        private SqlHelper sqlHelper;
        public InternalB2bCompanyMenu(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        #region 添加菜单或修改菜单

        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateB2bMenu";

        public int InsertOrUpdate(B2b_company_menu model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);
            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Comid", model.Com_id);
            cmd.AddParam("@Name", model.Name);
            cmd.AddParam("@Linkurl", model.Linkurl);
            cmd.AddParam("@Linktype", model.Linktype);
            cmd.AddParam("@Imgurl", model.Imgurl);
            cmd.AddParam("@Fonticon", model.Fonticon);
            cmd.AddParam("@Usestyle", model.Usestyle);
            cmd.AddParam("@Usetype", model.Usetype);
            cmd.AddParam("@Projectlist", model.Projectlist);
            cmd.AddParam("@Menutype", model.Menutype);
            cmd.AddParam("@menuindex", model.menuindex);
              cmd.AddParam("@menuviewtype", model.menuviewtype);
            
            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion


        #region 添加或修改底部按钮
        public int ButtonInsertOrUpdate(B2b_company_Button model)
        {
            string sqlTxt = "";
            if (model.Id != 0)
            {
                sqlTxt = @"update [dbo].[H5Button] set comid=@comid,name=@Name,linkurl=@Linkurl,Linkurlname=@Linkurlname,Sort=@Sort,Linktype=@Linktype where id=@Id ";
            }
            else {
                sqlTxt = @"insert [dbo].[H5Button] (comid,name,linkurl,Linkurlname,Linktype)values(@comid,@Name,@Linkurl,@Linkurlname,@Linktype) ";
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Comid", model.Comid);
            cmd.AddParam("@Name", model.Name);
            cmd.AddParam("@Linkurl", model.Linkurl);
            cmd.AddParam("@Linkurlname", model.Linkurlname);
            cmd.AddParam("@Sort", model.Sort);
            cmd.AddParam("@Linktype", model.Linktype);
           
            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion



        #region  删除子栏目显示内容
        internal int DeleteButton(int comid, int id)
        {
            const string sqlTxt = @"delete [dbo].[H5Button] where comid=@comid and id=@id";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@id", id);
            return cmd.ExecuteNonQuery();
        }
        #endregion






        #region  得到底部按钮
        internal B2b_company_Button GetButtonByComid(int comid, int id)
        {
            const string sqlTxt = @"SELECT *
  FROM [dbo].[H5Button] where comid=@comid and id=@id";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@id", id);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_company_Button u = null;

                while (reader.Read())
                {
                    u = new B2b_company_Button
                    {
                        Id = reader.GetValue<int>("Id"),
                        Comid = reader.GetValue<int>("comid"),
                        Linkurl = reader.GetValue<string>("Linkurl"),
                        Linkurlname = reader.GetValue<string>("Linkurlname"),
                        Name = reader.GetValue<string>("Name"),
                        Sort = reader.GetValue<int>("Sort"),
                        Linktype = reader.GetValue<int>("Linktype"),
                    };

                }
                return u;
            }
        }
        #endregion


        #region  插入子栏目显示内容 proid=产品ID,ID=栏目id
        internal int Insertmenu_pro(int comid, int proid,int id)
        {
            const string sqlTxt = @"insert [dbo].[H5Menu_pro] (comid,menuid,proid)values(@comid,@id,@proid) ";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@id", id);
            cmd.AddParam("@proid", proid);
            return cmd.ExecuteNonQuery();
        }
        #endregion

        #region  插入子栏目显示内容 proid=产品ID,ID=栏目id
        internal int InsertConsultant_pro(int comid, int proid, int id)
        {
            const string sqlTxt = @"insert [dbo].[consultant_pro_prolist] (comid,menuid,proid)values(@comid,@id,@proid) ";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@id", id);
            cmd.AddParam("@proid", proid);
            return cmd.ExecuteNonQuery();
        }
        #endregion


        #region  删除子栏目显示内容
        internal int deletemenu_pro(int comid, int id)
        {
            const string sqlTxt = @"delete [dbo].[H5Menu_pro] where comid=@comid and Menuid=@id";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@id", id);
            return cmd.ExecuteNonQuery();
        }
        #endregion

        #region  删除渠道子栏目显示内容
        internal int deleteConsultant_pro(int comid, int id)
        {
            const string sqlTxt = @"delete [dbo].[consultant_pro_prolist] where comid=@comid and Menuid=@id";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@id", id);
            return cmd.ExecuteNonQuery();
        }
        #endregion

        #region  查询渠道是否设定自己项目
        internal int selectConsultant_projectid(int comid, int channelid)
        {
            const string sqlTxt = @"select id  from [dbo].[consultant_pro] where comid=@comid and channelid=@channelid";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@comid", comid);
                cmd.AddParam("@channelid", channelid);

                using (var reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        return reader.GetValue<int>("id");
                    }
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region  查询渠道是否设定自己项目
        internal int selectprojceidbychannelid(int comid, int channelid)
        {
            const string sqlTxt = @"select linktype  from [dbo].[consultant_pro] where comid=@comid and channelid=@channelid";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@comid", comid);
                cmd.AddParam("@channelid", channelid);

                using (var reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        return reader.GetValue<int>("linktype");
                    }
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
        #endregion


        #region  查询子栏目显示内容
        internal int selectoucountmenu_pro(int comid, int id)
        {
            const string sqlTxt = @"select count(id) as num from [dbo].[H5Menu_pro] where comid=@comid and Menuid=@id";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@comid", comid);
                cmd.AddParam("@id", id);

                using (var reader = cmd.ExecuteReader())
                {
                   

                    if (reader.Read())
                    {
                        return reader.GetValue<int>("num");
                    }
                    return 0;
                }
            }
            catch {
                return 0;
            }
        }
        #endregion


        #region  删除子栏目显示内容
        internal int Deletemenu(int comid, int id)
        {
            const string sqlTxt = @"delete [dbo].[H5Menu] where comid=@comid and id=@id";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@id", id);
            return cmd.ExecuteNonQuery();
        }
        #endregion




        #region  得到菜单
        internal B2b_company_menu GetMenuByComid(int comid, int id)
        {
            const string sqlTxt = @"SELECT *
  FROM [dbo].[H5Menu] where comid=@comid and id=@id";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@id", id);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_company_menu u = null;

                while (reader.Read())
                {
                    u = new B2b_company_menu
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("comid"),
                        Imgurl = reader.GetValue<int>("Imgurl"),
                        Linkurl = reader.GetValue<string>("Linkurl"),
                        Linktype = reader.GetValue<int>("Linktype"),
                        Name = reader.GetValue<string>("Name"),
                        Fonticon = reader.GetValue<string>("Fonticon"),
                        Usestyle = reader.GetValue<int>("Usestyle"),
                        Usetype = reader.GetValue<int>("Usetype"),
                        Projectlist = reader.GetValue<int>("Projectlist"),
                        Menutype = reader.GetValue<int>("Menutype"),
                        menuviewtype = reader.GetValue<int>("menuviewtype"),
                    };

                }
                return u;
            }
        }
        #endregion


        #region  得到图片列表 Usetype =0 微站， =1微商城
        internal List<B2b_company_menu> GetMenuList(int comid, int pageindex, int pagesize, out int totalcount, int usetype = 0, int menuindex=0)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

            var condition = "comid=" + comid + " and Usetype=" + usetype + " and ((Projectlist in (select projectid from b2b_com_pro where pro_state=1) and not id in (select menuid from H5Menu_pro) ) or (id in (select menuid from H5Menu_pro where proid in (select id from b2b_com_pro where pro_state=1))) OR menutype=1 ) ";
            

            if (usetype == 0) {
                condition = "comid=" + comid + " and Usetype=" + usetype ;
            }


            condition += " and menuindex=" + menuindex;


            cmd.PagingCommand1("H5Menu", "*", "sortid,id", "", pagesize, pageindex, "", condition);

            List<B2b_company_menu> list = new List<B2b_company_menu>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_company_menu
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("comid"),
                        Imgurl = reader.GetValue<int>("Imgurl"),
                        Linkurl = reader.GetValue<string>("Linkurl"),
                        Linktype = reader.GetValue<int>("Linktype"),
                        Name = reader.GetValue<string>("Name"),
                        Fonticon = reader.GetValue<string>("Fonticon"),
                        Usestyle = reader.GetValue<int>("Usestyle"),
                        Usetype = reader.GetValue<int>("Usetype"),
                        Projectlist = reader.GetValue<int>("Projectlist"),
                        Menutype = reader.GetValue<int>("Menutype"),
                        menuindex = reader.GetValue<int>("menuindex"),
                        menuviewtype = reader.GetValue<int>("menuviewtype"),
                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }
        #endregion


        #region  得到图片列表 Usetype =0 微站， =1微商城
        internal List<B2b_company_Button> GetButtonlist(int comid, int pageindex, int pagesize, int linktype, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

            var condition = "comid=" + comid + " and linktype=" + linktype;


            cmd.PagingCommand1("H5Button", "*", "sort,id", "", pagesize, pageindex, "", condition);

            List<B2b_company_Button> list = new List<B2b_company_Button>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_company_Button
                    {
                        Id = reader.GetValue<int>("Id"),
                        Comid = reader.GetValue<int>("comid"),
                        Linkurl = reader.GetValue<string>("Linkurl"),
                        Linkurlname = reader.GetValue<string>("Linkurlname"),
                        Name = reader.GetValue<string>("Name"),
                        Sort = reader.GetValue<int>("Sort"),
                        Linktype = reader.GetValue<int>("Linktype"),
                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }
        #endregion


        internal int SortMenu(string id, int sortid)
        {
            string sql = "update H5Menu set sortid=@sortid where id =@id ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);
            cmd.AddParam("@sortid", sortid);

            return cmd.ExecuteNonQuery();
        }
        #region 添加菜单或修改菜单



        private static readonly string SQLInsertOrUpdate1 = "usp_InsertOrUpdateB2bConsultant";

        public int ConsultantInsertOrUpdate(B2b_company_menu model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate1);
            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Comid", model.Com_id);
            cmd.AddParam("@Name", model.Name);
            cmd.AddParam("@Linkurl", model.Linkurl);
            cmd.AddParam("@Linktype", model.Linktype);//产品分类
            cmd.AddParam("@Imgurl", model.Imgurl);
            cmd.AddParam("@Fonticon", model.Fonticon);
            cmd.AddParam("@Outdata", model.Outdata);
            cmd.AddParam("@Channelid", model.Channelid);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion

        
        #region  通过渠道ID获取自定义菜单（产品）id
        internal int getConsultantidbychannelid(int channelid)
        {
            const string sqlTxt = @"SELECT *
  FROM [dbo].[consultant_pro] where channelid=@channelid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@channelid", channelid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("Id");   
                }
                return 0;
            }
        }
        #endregion

        #region  删除菜单
        internal int DeleteConsultant(int comid, int id)
        {
            const string sqlTxt = @"delete [dbo].[consultant_pro] where comid=@comid and id=@id";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@id", id);
            return cmd.ExecuteNonQuery();
        }
        #endregion



        #region  得到菜单
        internal B2b_company_menu GetConsultantByComid(int comid, int id)
        {
            const string sqlTxt = @"SELECT *
  FROM [dbo].[consultant_pro] where comid=@comid and id=@id ";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@id", id);


            using (var reader = cmd.ExecuteReader())
            {
                B2b_company_menu u = null;

                while (reader.Read())
                {
                    u = new B2b_company_menu
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("comid"),
                        Imgurl = reader.GetValue<int>("Imgurl"),
                        Linkurl = reader.GetValue<string>("Linkurl"),
                        Linktype = reader.GetValue<int>("Linktype"),
                        Name = reader.GetValue<string>("Name"),
                        Fonticon = reader.GetValue<string>("Fonticon"),
                        Outdata = reader.GetValue<int>("Outdata"),
                        Channelid = reader.GetValue<int>("channelid"),
                    };

                }
                return u;
            }
        }
        #endregion


        #region  得到图片列表 typeid 1=banner图片 ，2=导航图片
        internal List<B2b_company_menu> GetconsultantList(int comid, int pageindex, int pagesize, out int totalcount,int channelid=0)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = "comid=" + comid + " and  channelid="+channelid;
            cmd.PagingCommand1("consultant_pro", "*", "sortid,id", "", pagesize, pageindex, "", condition);

            List<B2b_company_menu> list = new List<B2b_company_menu>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_company_menu
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("comid"),
                        Imgurl = reader.GetValue<int>("Imgurl"),
                        Linkurl = reader.GetValue<string>("Linkurl"),
                        Linktype = reader.GetValue<int>("Linktype"),
                        Name = reader.GetValue<string>("Name"),
                        Fonticon = reader.GetValue<string>("Fonticon"),
                        Outdata = reader.GetValue<int>("Outdata"),
                        Channelid = reader.GetValue<int>("channelid"),
                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }
        #endregion

        internal int SortConsultant(string id, int sortid)
        {
            string sql = "update consultant_pro set sortid=@sortid where id =@id ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);
            cmd.AddParam("@sortid", sortid);

            return cmd.ExecuteNonQuery();
        }


    }
}
