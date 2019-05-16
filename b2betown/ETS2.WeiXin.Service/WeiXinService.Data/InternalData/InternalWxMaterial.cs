using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Model;
using System.Data;
using System.Data.SqlClient;
using ETS2.Common.Business;

namespace ETS2.WeiXin.Service.WeiXinService.Data.InternalData
{
    public class InternalWxMaterial
    {
        private SqlHelper sqlHelper;
        public InternalWxMaterial(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        //插入或修改渠道信息
        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateWxMaterial";

        internal int EditMaterial(WxMaterial model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);

            cmd.AddParam("@MaterialId", model.MaterialId);
            cmd.AddParam("@Title", model.Title);
            cmd.AddParam("@Author", model.Author);
            cmd.AddParam("@Imgpath", model.Imgpath);
            cmd.AddParam("@Summary", model.Summary);
            cmd.AddParam("@Article", model.Article);
            cmd.AddParam("@Articleurl", model.Articleurl);
            cmd.AddParam("@Operatime", model.Operatime);
            cmd.AddParam("@ApplyState", model.Applystate);

            cmd.AddParam("@Phone", model.Phone);
            cmd.AddParam("@Price", model.Price);
            cmd.AddParam("@PromoteTypeId", model.SalePromoteTypeid);

            cmd.AddParam("@staticdate", model.Staticdate);
            cmd.AddParam("@enddate", model.Enddate);

            cmd.AddParam("@periodicalid", model.Periodicalid);
            cmd.AddParam("@comid", model.Comid);
            cmd.AddParam("@authorpayurl", model.Authorpayurl);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }






        internal int InsMaterialKey(int MaterialId, string keyword)
        {
            string sql1 = "insert WxKeyWord(keyword) values('" + keyword + "');SELECT @@IDENTITY; ";
            var cmd1 = sqlHelper.PrepareTextSqlCommand(sql1);
            object o = cmd1.ExecuteScalar();
            int keyid = 0;
            if (o != null)
            {
                keyid = int.Parse(o.ToString());
            }



            string sql2 = "insert WxMaterialKeyWord(MaterialId,KeyId) values(" + MaterialId + "," + keyid + ")";
            var cmd2 = sqlHelper.PrepareTextSqlCommand(sql2);
            return cmd2.ExecuteNonQuery();
        }


        internal WxMaterial GetWxMaterial(int materialid)
        {
            string sql = "SELECT TOP 1 *  FROM [EtownDB].[dbo].[WxMaterial] where MaterialId=" + materialid;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    WxMaterial wxmaterial = new WxMaterial();
                    wxmaterial.MaterialId = reader.GetValue<int>("MaterialId");
                    wxmaterial.Title = reader.GetValue<string>("title");
                    wxmaterial.Author = reader.GetValue<string>("author");
                    wxmaterial.Imgpath = reader.GetValue<string>("imgpath");
                    wxmaterial.Summary = reader.GetValue<string>("summary");
                    wxmaterial.Article = reader.GetValue<string>("article");
                    wxmaterial.Articleurl = reader.GetValue<string>("articleurl");
                    wxmaterial.Operatime = reader.GetValue<DateTime>("operatime");
                    wxmaterial.Applystate = reader.GetValue<bool>("applystate") == true ? 1 : 0;

                    wxmaterial.Phone = reader.GetValue<string>("phone") == null ? "" : reader.GetValue<string>("phone");
                    wxmaterial.Price = reader.GetValue<decimal?>("price") == null ? 0 : reader.GetValue<decimal>("price");
                    wxmaterial.SalePromoteTypeid = reader.GetValue<int?>("SalePromoteTypeid") == null ? 1 : reader.GetValue<int>("SalePromoteTypeid");
                    wxmaterial.Staticdate = reader.GetValue<DateTime>("staticdate");
                    wxmaterial.Enddate = reader.GetValue<DateTime>("enddate");
                    wxmaterial.Periodicalid = reader.GetValue<int>("periodicalid");
                    wxmaterial.Comid = reader.GetValue<int>("comid");
                    wxmaterial.Authorpayurl = reader.GetValue<string>("authorpayurl");

                    reader.Close();
                    wxmaterial.Keyword = GetWxMaterialKeyWordStrByMaterialId(materialid);

                    return wxmaterial;
                }
                else
                {
                    return null;
                }
            }


        }
        internal WxMaterial GetWxMaterial(int comid, int materialid)
        {
            string sql = "SELECT TOP 1 *  FROM [EtownDB].[dbo].[WxMaterial] where MaterialId=" + materialid + " and comid=" + comid;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    WxMaterial wxmaterial = new WxMaterial();
                    wxmaterial.MaterialId = reader.GetValue<int>("MaterialId");
                    wxmaterial.Title = reader.GetValue<string>("title");
                    wxmaterial.Author = reader.GetValue<string>("author");
                    wxmaterial.Imgpath = reader.GetValue<string>("imgpath");
                    wxmaterial.Summary = reader.GetValue<string>("summary");
                    wxmaterial.Article = reader.GetValue<string>("article");
                    wxmaterial.Articleurl = reader.GetValue<string>("articleurl");
                    wxmaterial.Operatime = reader.GetValue<DateTime>("operatime");
                    wxmaterial.Applystate = reader.GetValue<bool>("applystate") == true ? 1 : 0;

                    wxmaterial.Phone = reader.GetValue<string>("phone") == null ? "" : reader.GetValue<string>("phone");
                    wxmaterial.Price = reader.GetValue<decimal?>("price") == null ? 0 : reader.GetValue<decimal>("price");
                    wxmaterial.SalePromoteTypeid = reader.GetValue<int?>("SalePromoteTypeid") == null ? 1 : reader.GetValue<int>("SalePromoteTypeid");
                    wxmaterial.Staticdate = reader.GetValue<DateTime>("staticdate");
                    wxmaterial.Enddate = reader.GetValue<DateTime>("enddate");
                    wxmaterial.Periodicalid = reader.GetValue<int>("periodicalid");
                    wxmaterial.Comid = reader.GetValue<int>("comid");
                    wxmaterial.Authorpayurl = reader.GetValue<string>("authorpayurl");

                    reader.Close();
                    wxmaterial.Keyword = GetWxMaterialKeyWordStrByMaterialId(materialid);

                    return wxmaterial;
                }
                else
                {
                    return null;
                }
            }


        }
        /// <summary>
        ///  查询 期详细
        /// </summary>
        /// <param name="periodicalid"></param>
        /// <returns></returns>
        internal periodical selectperiodical(int periodicalid)
        {
            string sql = "SELECT TOP 1 *  FROM periodical where id=" + periodicalid;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    periodical period = new periodical();

                    period.Id = reader.GetValue<int>("id");
                    period.Comid = reader.GetValue<int>("comid");
                    period.Wxsaletypeid1 = reader.GetValue<int>("Wxsaletypeid");
                    period.Percal = reader.GetValue<int>("percal");
                    period.Peryear = reader.GetValue<int>("peryear");
                    period.Uptime = reader.GetValue<DateTime>("uptime");

                    reader.Close();

                    return period;
                }
                else
                {
                    return null;
                }
            }


        }

        internal periodical Selperiod(int percal, int wxtype)
        {
            string sql = "SELECT TOP 1 *  FROM periodical where percal=" + percal + " and  Wxsaletypeid=" + wxtype;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    periodical period = new periodical();

                    period.Id = reader.GetValue<int>("id");
                    period.Comid = reader.GetValue<int>("comid");
                    period.Wxsaletypeid1 = reader.GetValue<int>("Wxsaletypeid");
                    period.Percal = reader.GetValue<int>("percal");
                    period.Peryear = reader.GetValue<int>("peryear");
                    period.Uptime = reader.GetValue<DateTime>("uptime");

                    reader.Close();

                    return period;
                }
                else
                {
                    return null;
                }
            }


        }


        /// <summary>
        ///  查询 期详细（条件是菜单）
        /// </summary>
        /// <param name="periodicalid"></param>
        /// <returns></returns>
        internal periodical selectWxsaletype(int Wxsaletype, int comid)
        {
            string sql = "SELECT TOP 1 *  FROM periodical where comid=@comid and  Wxsaletypeid=@Wxsaletypeid order by uptime desc ";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@Wxsaletypeid", Wxsaletype);

            try
            {
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        periodical period = new periodical();

                        period.Id = reader.GetValue<int>("id");
                        period.Comid = reader.GetValue<int>("comid");
                        period.Wxsaletypeid1 = reader.GetValue<int>("Wxsaletypeid");
                        period.Percal = reader.GetValue<int>("percal");
                        period.Peryear = reader.GetValue<int>("peryear");
                        period.Uptime = reader.GetValue<DateTime>("uptime");

                        reader.Close();

                        return period;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }


        }


        internal int Addperiod(int period, int comid, int Wxsaletypeid, int percal)
        {
            string sql = "";
            if (period == 0)
            {
                sql = @"INSERT INTO [EtownDB].[dbo].[periodical]([comid],[Wxsaletypeid],[percal],[uptime])
                VALUES(@comid,@Wxsaletypeid,@percal+1,@uptime)";
            }
            else
            {
                sql = @"UPDATE [EtownDB].[dbo].[periodical]
                   SET [comid] = @comid
                      ,[Wxsaletypeid] = @Wxsaletypeid
                      ,[percal] = @percal
                      ,[uptime] = @uptime
                 WHERE  id=" + period;
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@Wxsaletypeid", Wxsaletypeid);
            cmd.AddParam("@percal", percal);
            cmd.AddParam("@uptime", DateTime.Now);
            return cmd.ExecuteNonQuery();
        }

        internal WxMaterial Getidinfo(int materialid)
        {
            string sql = "SELECT TOP 1 [MaterialId],[title],[author],[imgpath],[summary],[article],[articleurl],[operatime],[applystate],[phone] ,[price],[SalePromoteTypeid]  FROM [EtownDB].[dbo].[WxMaterial] where MaterialId=" + materialid;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    WxMaterial wxmaterial = new WxMaterial();
                    wxmaterial.MaterialId = reader.GetValue<int>("MaterialId");
                    wxmaterial.Title = reader.GetValue<string>("title");
                    wxmaterial.Author = reader.GetValue<string>("author");
                    wxmaterial.Imgpath = reader.GetValue<string>("imgpath");
                    wxmaterial.Summary = reader.GetValue<string>("summary");
                    wxmaterial.Article = reader.GetValue<string>("article");
                    wxmaterial.Articleurl = reader.GetValue<string>("articleurl");
                    wxmaterial.Operatime = reader.GetValue<DateTime>("operatime");
                    wxmaterial.Applystate = reader.GetValue<bool>("applystate") == true ? 1 : 0;

                    wxmaterial.Phone = reader.GetValue<string>("phone") == null ? "" : reader.GetValue<string>("phone");
                    wxmaterial.Price = reader.GetValue<decimal?>("price") == null ? 0 : reader.GetValue<decimal>("price");
                    wxmaterial.SalePromoteTypeid = reader.GetValue<int?>("SalePromoteTypeid") == null ? 1 : reader.GetValue<int>("SalePromoteTypeid");

                    return wxmaterial;
                }
                else
                {
                    return null;
                }
            }


        }

        public string GetWxMaterialKeyWordStrByMaterialId(int materialid)
        {
            string keywords = "";//关键词字符串

            string sql = "select  a.KeyId,a.keyword from WxKeyWord a where a.KeyId in (select b.KeyId from WxMaterialKeyWord b where b.MaterialId=" + materialid + ")";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    keywords += reader.GetValue<string>("keyword") + "，";
                }

                return keywords.Length > 0 ? keywords.Substring(0, keywords.Length - 1) : "";
            }
        }

        internal List<WxMaterial> WxMaterialPageList(int pageindex, int pagesize, int applystate, int promotetypeid, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "WxMaterial";
            var strGetFields = "*";
            var sortKey = "sortid";
            var sortMode = "0";

            var condition = "SalePromoteTypeid=" + promotetypeid;
            if (applystate != 10)
            {
                condition += " and applystate=" + applystate;
            }


            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<WxMaterial> list = new List<WxMaterial>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    WxMaterial wxmaterial = new WxMaterial();

                    wxmaterial.MaterialId = reader.GetValue<int>("MaterialId");
                    wxmaterial.Title = reader.GetValue<string>("title");
                    wxmaterial.Applystate = reader.GetValue<bool>("applystate") == true ? 1 : 0;
                    wxmaterial.Article = reader.GetValue<string>("article");
                    wxmaterial.Articleurl = reader.GetValue<string>("articleurl");
                    wxmaterial.Author = reader.GetValue<string>("author");
                    wxmaterial.Imgpath = reader.GetValue<string>("imgpath");
                    wxmaterial.Operatime = reader.GetValue<DateTime>("Operatime");
                    wxmaterial.Summary = reader.GetValue<string>("Summary");

                    wxmaterial.Phone = reader.GetValue<string>("phone");
                    wxmaterial.Price = reader.GetValue<decimal>("price");
                    wxmaterial.SalePromoteTypeid = reader.GetValue<int>("SalePromoteTypeid");



                    list.Add(wxmaterial);

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }

        internal List<WxMaterial> WxMaterialPageList(int comid, int pageindex, int pagesize, int applystate, int promotetypeid, out int totalcount, string key = "", int top1 = 0, int consultantid = 0,int consultantpro=0)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "WxMaterial";
            var strGetFields = "*";
            var sortKey = "MaterialId";
            var sortMode = "1";

            var condition = " comid=" + comid;

            if (promotetypeid != 1000000)
            {
                condition += " and SalePromoteTypeid=" + promotetypeid;
            }

            if (applystate != 10)
            {
                condition += " and applystate=" + applystate;
            }

            if (key != "")
            {
                condition += " and (MaterialId in (select MaterialId from WxMaterialKeyWord where keyid in (select keyid from WxKeyWord where keyword like '%" + key + "%')) or title like  '%" + key + "%')";
            }

            if (top1 == 1) {
                condition += " and  MaterialId in (select max(MaterialId) from WxMaterial group by SalePromoteTypeid )";

            }

            if (consultantid != 0)
            {
                condition += " and  MaterialId in  (select linktype from [consultant_pro] where id=" + consultantid + ")";
            
            }
            if (consultantpro != 0) {

                condition += " and  MaterialId in  (select proid from [consultant_pro_prolist] where menuid=" + consultantpro + ")";
            }


            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<WxMaterial> list = new List<WxMaterial>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    var temp_imgid = reader.GetValue<string>("imgpath");
                    var temp_imgurl = "";
                    if(temp_imgid !=""){
                        try
                        {
                            temp_imgurl = FileSerivce.GetImgUrl(int.Parse(reader.GetValue<string>("imgpath")));
                        }
                        catch {
                            temp_imgurl = "";
                        }
                    }


                    WxMaterial wxmaterial = new WxMaterial();

                    wxmaterial.MaterialId = reader.GetValue<int>("MaterialId");
                    wxmaterial.Title = reader.GetValue<string>("title");
                    wxmaterial.Applystate = reader.GetValue<bool>("applystate") == true ? 1 : 0;
                    wxmaterial.Article = reader.GetValue<string>("article");
                    wxmaterial.Articleurl = reader.GetValue<string>("articleurl");
                    wxmaterial.Author = reader.GetValue<string>("author");
                    wxmaterial.Imgpath = reader.GetValue<string>("imgpath");
                    wxmaterial.Imgurl = temp_imgurl;
                    wxmaterial.Operatime = reader.GetValue<DateTime>("Operatime");
                    wxmaterial.Summary = reader.GetValue<string>("Summary");

                    wxmaterial.Phone = reader.GetValue<string>("phone");
                    wxmaterial.Price = reader.GetValue<decimal>("price");
                    wxmaterial.SalePromoteTypeid = reader.GetValue<int>("SalePromoteTypeid");
                    wxmaterial.Periodicalid = reader.GetValue<int>("Periodicalid");


                    list.Add(wxmaterial);

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }
        //通过商城显示文章调用，及选择文章
        internal List<WxMaterial> ShopWxMaterialPageList(int comid, int pageindex, int pagesize, int applystate, int menuid, int promotetypeid, out int totalcount, string key = "")
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "WxMaterial";
            var strGetFields = "*";
            var sortKey = "MaterialId";
            var sortMode = "1";

            var condition = " comid=" + comid;

            if (menuid != 0)
            {
                condition += "and MaterialId in (select proid from H5Menu_pro where menuid=" + menuid + ")";
            }
            if (applystate != 10)
            {
                condition += "and applystate=" + applystate;
            }

            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<WxMaterial> list = new List<WxMaterial>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    var temp_imgid = reader.GetValue<string>("imgpath");
                    var temp_imgurl = "";
                    if (temp_imgid != "")
                    {
                        try
                        {
                            temp_imgurl = FileSerivce.GetImgUrl(int.Parse(reader.GetValue<string>("imgpath")));
                        }
                        catch
                        {
                            temp_imgurl = "";
                        }
                    }

                    WxMaterial wxmaterial = new WxMaterial();

                    wxmaterial.MaterialId = reader.GetValue<int>("MaterialId");
                    wxmaterial.Title = reader.GetValue<string>("title");
                    wxmaterial.Applystate = reader.GetValue<bool>("applystate") == true ? 1 : 0;
                    wxmaterial.Article = reader.GetValue<string>("article");
                    wxmaterial.Articleurl = reader.GetValue<string>("articleurl");
                    wxmaterial.Author = reader.GetValue<string>("author");
                    wxmaterial.Imgpath = reader.GetValue<string>("imgpath");
                    wxmaterial.Imgurl = temp_imgurl;
                    wxmaterial.Operatime = reader.GetValue<DateTime>("Operatime");
                    wxmaterial.Summary = reader.GetValue<string>("Summary");

                    wxmaterial.Phone = reader.GetValue<string>("phone");
                    wxmaterial.Price = reader.GetValue<decimal>("price");
                    wxmaterial.SalePromoteTypeid = reader.GetValue<int>("SalePromoteTypeid");



                    list.Add(wxmaterial);

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }

        internal List<WxMaterial> ForwardingWxMaterialPageList(int comid, int pageindex, int pagesize, int applystate, int promotetypeid, int wxid, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "member_forwardingcount left join  WxMaterial on member_forwardingcount.wxmaid=WxMaterial.materialid left join b2b_crm on member_forwardingcount.uid=b2b_crm.id";
            var strGetFields = "WxMaterial.title,WxMaterial.materialid,b2b_crm.name,b2b_crm.idcard,member_forwardingcount.fornum";
            var sortKey = "fornum";
            var sortMode = "1";

            var condition = " WxMaterial.comid=" + comid + " and member_forwardingcount.wxmaid=" + wxid;


            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<WxMaterial> list = new List<WxMaterial>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    WxMaterial wxmaterial = new WxMaterial();

                    wxmaterial.MaterialId = reader.GetValue<int>("MaterialId");
                    wxmaterial.Title = reader.GetValue<string>("title");
                    wxmaterial.Name = reader.GetValue<string>("name");//临时用到姓名
                    wxmaterial.Idcard = reader.GetValue<decimal>("idcard");//临时用到卡号
                    wxmaterial.Fornum = reader.GetValue<int>("fornum");//临时用到数量
                    list.Add(wxmaterial);

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }

        //统计本素材转发访问数
        internal int ForwardingWxMaterialcount(int comid, int wxid)
        {
            string sql = "select sum(fornum) as fornum from Member_forwardingcount where wxmaid=" + wxid + " group by wxmaid ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("fornum");
                }
                else
                {
                    return 0;
                }
            }

        }


        internal List<periodical> periodicalList(int pageindex, int pagesize, int applystate, int promotetypeid, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "periodical";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "0";

            var condition = "Wxsaletypeid=" + promotetypeid;
            if (applystate != 10)
            {
                condition += "&applystate=" + applystate;
            }


            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<periodical> list = new List<periodical>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    periodical wxmaterial = new periodical();

                    wxmaterial.Id = reader.GetValue<int>("id");
                    wxmaterial.Comid = reader.GetValue<int>("comid");
                    wxmaterial.Wxsaletypeid1 = reader.GetValue<int>("Wxsaletypeid");
                    wxmaterial.Percal = reader.GetValue<int>("percal");
                    wxmaterial.Peryear = reader.GetValue<int>("peryear");
                    wxmaterial.Uptime = reader.GetValue<DateTime>("uptime");
                    wxmaterial.Perinfo = reader.GetValue<string>("perinfo");

                    list.Add(wxmaterial);

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }
        internal List<periodical> periodicalList(int pageindex, int pagesize, int applystate, int promotetypeid, int comid, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "periodical";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "0";

            var condition = "Wxsaletypeid=" + promotetypeid + " and comid=" + comid;
            if (applystate != 10)
            {
                condition += " and applystate=" + applystate;
            }


            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<periodical> list = new List<periodical>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    periodical wxmaterial = new periodical();

                    wxmaterial.Id = reader.GetValue<int>("id");
                    wxmaterial.Comid = reader.GetValue<int>("comid");
                    wxmaterial.Wxsaletypeid1 = reader.GetValue<int>("Wxsaletypeid");
                    wxmaterial.Percal = reader.GetValue<int>("percal");
                    wxmaterial.Peryear = reader.GetValue<int>("peryear");
                    wxmaterial.Uptime = reader.GetValue<DateTime>("uptime");
                    wxmaterial.Perinfo = reader.GetValue<string>("perinfo");

                    list.Add(wxmaterial);

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }
        internal List<WxMaterial> periodicaltypelist(int pageindex, int pagesize, int applystate, int periodid, int salepromotetypeid, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "WxMaterial";
            var strGetFields = "*";
            var sortKey = "sortid";
            var sortMode = "0";

            var condition = "SalePromoteTypeid=" + salepromotetypeid + "and periodicalid=" + periodid;
            if (applystate != 10)
            {
                condition += "&applystate=" + applystate;
            }


            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<WxMaterial> list = new List<WxMaterial>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    WxMaterial wxmaterial = new WxMaterial();

                    wxmaterial.MaterialId = reader.GetValue<int>("MaterialId");
                    wxmaterial.Title = reader.GetValue<string>("title");
                    wxmaterial.Applystate = reader.GetValue<bool>("applystate") == true ? 1 : 0;
                    wxmaterial.Article = reader.GetValue<string>("article");
                    wxmaterial.Articleurl = reader.GetValue<string>("articleurl");
                    wxmaterial.Author = reader.GetValue<string>("author");
                    wxmaterial.Imgpath = reader.GetValue<string>("imgpath");

                    wxmaterial.Operatime = reader.GetValue<DateTime>("Operatime");
                    wxmaterial.Summary = reader.GetValue<string>("Summary");

                    wxmaterial.Phone = reader.GetValue<string>("phone");
                    wxmaterial.Price = reader.GetValue<decimal>("price");
                    wxmaterial.SalePromoteTypeid = reader.GetValue<int>("SalePromoteTypeid");

                    wxmaterial.Periodicalid = reader.GetValue<int>("periodicalid");

                    list.Add(wxmaterial);

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }
        internal List<WxMaterial> periodicaltypelist(int pageindex, int pagesize, int applystate, int promotetypeid, int type, int comid, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "WxMaterial";
            var strGetFields = "*";
            var sortKey = "sortid";
            var sortMode = "0";

            var condition = "SalePromoteTypeid=" + type + " and periodicalid=" + promotetypeid + " and comid=" + comid;
            if (applystate != 10)
            {
                condition += " and applystate=" + applystate;
            }


            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<WxMaterial> list = new List<WxMaterial>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    WxMaterial wxmaterial = new WxMaterial();

                    wxmaterial.MaterialId = reader.GetValue<int>("MaterialId");
                    wxmaterial.Title = reader.GetValue<string>("title");
                    wxmaterial.Applystate = reader.GetValue<bool>("applystate") == true ? 1 : 0;
                    wxmaterial.Article = reader.GetValue<string>("article");
                    wxmaterial.Articleurl = reader.GetValue<string>("articleurl");
                    wxmaterial.Author = reader.GetValue<string>("author");
                    wxmaterial.Imgpath = reader.GetValue<string>("imgpath");

                    wxmaterial.Operatime = reader.GetValue<DateTime>("Operatime");
                    wxmaterial.Summary = reader.GetValue<string>("Summary");

                    wxmaterial.Phone = reader.GetValue<string>("phone");
                    wxmaterial.Price = reader.GetValue<decimal>("price");
                    wxmaterial.SalePromoteTypeid = reader.GetValue<int>("SalePromoteTypeid");

                    wxmaterial.Periodicalid = reader.GetValue<int>("periodicalid");

                    list.Add(wxmaterial);

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }

        //会员登录特惠
        internal List<WxMaterial> logPageList(int pageindex, int pagesize, int applystate, string promotetype, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "WxMaterial";
            var strGetFields = "*";
            var sortKey = "operatime";
            var sortMode = "0";

            var condition = promotetype;
            if (applystate != 10)
            {
                condition += "&applystate=" + applystate;
            }


            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<WxMaterial> list = new List<WxMaterial>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    WxMaterial wxmaterial = new WxMaterial();

                    wxmaterial.MaterialId = reader.GetValue<int>("MaterialId");
                    wxmaterial.Title = reader.GetValue<string>("title");
                    wxmaterial.Applystate = reader.GetValue<bool>("applystate") == true ? 1 : 0;
                    wxmaterial.Article = reader.GetValue<string>("article");
                    wxmaterial.Articleurl = reader.GetValue<string>("articleurl");
                    wxmaterial.Author = reader.GetValue<string>("author");
                    wxmaterial.Imgpath = reader.GetValue<string>("imgpath");

                    wxmaterial.Operatime = reader.GetValue<DateTime>("Operatime");
                    wxmaterial.Summary = reader.GetValue<string>("Summary");

                    wxmaterial.Phone = reader.GetValue<string>("phone");
                    wxmaterial.Price = reader.GetValue<decimal>("price");
                    wxmaterial.SalePromoteTypeid = reader.GetValue<int>("SalePromoteTypeid");



                    list.Add(wxmaterial);

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }
        internal WxMaterial logGetidinfo(string promotetype)
        {
            string sql = "select top(1)* from WxMaterial where " + promotetype + "";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    WxMaterial wxmaterial = new WxMaterial();
                    wxmaterial.MaterialId = reader.GetValue<int>("MaterialId");
                    wxmaterial.Title = reader.GetValue<string>("title");
                    wxmaterial.Author = reader.GetValue<string>("author");
                    wxmaterial.Imgpath = reader.GetValue<string>("imgpath");
                    wxmaterial.Summary = reader.GetValue<string>("summary");
                    wxmaterial.Article = reader.GetValue<string>("article");
                    wxmaterial.Articleurl = reader.GetValue<string>("articleurl");
                    wxmaterial.Operatime = reader.GetValue<DateTime>("operatime");
                    wxmaterial.Applystate = reader.GetValue<bool>("applystate") == true ? 1 : 0;

                    wxmaterial.Phone = reader.GetValue<string>("phone") == null ? "" : reader.GetValue<string>("phone");
                    wxmaterial.Price = reader.GetValue<decimal?>("price") == null ? 0 : reader.GetValue<decimal>("price");
                    wxmaterial.SalePromoteTypeid = reader.GetValue<int?>("SalePromoteTypeid") == null ? 1 : reader.GetValue<int>("SalePromoteTypeid");

                    return wxmaterial;
                }
                else
                {
                    return null;
                }
            }


        }
        //特惠end
        internal int DelMaterial(int materialid)
        {
            string sql = "delete WxMaterial where MaterialId=@MaterialId ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@MaterialId", materialid);

            return cmd.ExecuteNonQuery();
        }

        internal int DelWxKeyWord(int materialid)
        {
            string sql = "delete WxKeyWord where keyid in (select keyid from WxMaterialKeyWord where materialid=@MaterialId) ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@MaterialId", materialid);

            return cmd.ExecuteNonQuery();
        }

        internal int DelMaterialKeyByMaterialId(int materialid)
        {
            string sql = "delete WxMaterialKeyWord where MaterialId=@MaterialId ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@MaterialId", materialid);

            return cmd.ExecuteNonQuery();

        }

        internal List<WxMaterial> GetWxMaterialByKeyword(string keyword)
        {
            string sql = "SELECT top 10  [MaterialId],[title],[author],[imgpath],[summary],[article],[articleurl],[operatime],[applystate],[phone]      ,[price]      ,[SalePromoteTypeid]  FROM [EtownDB].[dbo].[WxMaterial] where MaterialId in (select materialid from WxMaterialKeyWord where keyid in (select keyid from wxkeyword where keyword='" + keyword + "')) and applystate=1 order by sortid,materialid desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            List<WxMaterial> list = new List<WxMaterial>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    WxMaterial wxmaterial = new WxMaterial();

                    wxmaterial.MaterialId = reader.GetValue<int>("MaterialId");
                    wxmaterial.Title = reader.GetValue<string>("title");
                    wxmaterial.Applystate = reader.GetValue<bool>("applystate") == true ? 1 : 0;
                    wxmaterial.Article = reader.GetValue<string>("article");
                    wxmaterial.Articleurl = reader.GetValue<string>("articleurl");
                    wxmaterial.Author = reader.GetValue<string>("author");
                    wxmaterial.Imgpath = reader.GetValue<string>("imgpath");

                    wxmaterial.Operatime = reader.GetValue<DateTime>("Operatime");
                    wxmaterial.Summary = reader.GetValue<string>("Summary");


                    list.Add(wxmaterial);

                }
            }


            return list;
        }
        internal List<WxMaterial> GetWxMaterialByKeyword(string keyword, int comid)
        {
            string sql = "SELECT top 10  [MaterialId],[title],[author],[imgpath],[summary],[article],[articleurl],[operatime],[applystate],[phone]      ,[price]      ,[SalePromoteTypeid]  FROM [EtownDB].[dbo].[WxMaterial] where MaterialId in (select materialid from WxMaterialKeyWord where keyid in (select keyid from wxkeyword where keyword='" + keyword + "')) and applystate=1 and comid=" + comid + " order by sortid,materialid desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            List<WxMaterial> list = new List<WxMaterial>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    WxMaterial wxmaterial = new WxMaterial();

                    wxmaterial.MaterialId = reader.GetValue<int>("MaterialId");
                    wxmaterial.Title = reader.GetValue<string>("title");
                    wxmaterial.Applystate = reader.GetValue<bool>("applystate") == true ? 1 : 0;
                    wxmaterial.Article = reader.GetValue<string>("article");
                    wxmaterial.Articleurl = reader.GetValue<string>("articleurl");
                    wxmaterial.Author = reader.GetValue<string>("author");
                    wxmaterial.Imgpath = reader.GetValue<string>("imgpath");

                    wxmaterial.Operatime = reader.GetValue<DateTime>("Operatime");
                    wxmaterial.Summary = reader.GetValue<string>("Summary");


                    list.Add(wxmaterial);

                }
            }


            return list;
        }

        internal List<WxMaterial> GetLatestWxMaterial()
        {
            string sql = "SELECT top 10 [MaterialId],[title],[author],[imgpath],[summary],[article],[articleurl],[operatime],[applystate],[phone],[price],[SalePromoteTypeid]  FROM [EtownDB].[dbo].[WxMaterial] where  applystate=1 order by sortid,MaterialId desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            List<WxMaterial> list = new List<WxMaterial>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    WxMaterial wxmaterial = new WxMaterial();

                    wxmaterial.MaterialId = reader.GetValue<int>("MaterialId");
                    wxmaterial.Title = reader.GetValue<string>("title");
                    wxmaterial.Applystate = reader.GetValue<bool>("applystate") == true ? 1 : 0;
                    wxmaterial.Article = reader.GetValue<string>("article");
                    wxmaterial.Articleurl = reader.GetValue<string>("articleurl");
                    wxmaterial.Author = reader.GetValue<string>("author");
                    wxmaterial.Imgpath = reader.GetValue<string>("imgpath");

                    wxmaterial.Operatime = reader.GetValue<DateTime>("Operatime");
                    wxmaterial.Summary = reader.GetValue<string>("Summary");


                    list.Add(wxmaterial);

                }
            }


            return list;
        }

        internal int Delwxmenu(int wxmenuid)
        {
            string sql = "delete WxMenu where menuid=@menuid ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@menuid", wxmenuid);

            return cmd.ExecuteNonQuery();
        }
        internal IList<WxMaterial> GetMaterialByPromoteType(int comid, int promotetypeid, int periodicalid, out int totalcount)
        {
            string sql = "SELECT   [MaterialId],[title],[author],[imgpath],[summary],[article],[articleurl],[operatime],[applystate],[phone],[price],[SalePromoteTypeid]  FROM [EtownDB].[dbo].[WxMaterial] where SalePromoteTypeid=@promotetypeid and comid=@comid and periodicalid=@periodicalid order by sortid ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@promotetypeid", promotetypeid);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@periodicalid", periodicalid);

            List<WxMaterial> list = new List<WxMaterial>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    WxMaterial wxmaterial = new WxMaterial();

                    wxmaterial.MaterialId = reader.GetValue<int>("MaterialId");
                    wxmaterial.Title = reader.GetValue<string>("title");
                    wxmaterial.Applystate = reader.GetValue<bool>("applystate") == true ? 1 : 0;
                    wxmaterial.Article = reader.GetValue<string>("article");
                    wxmaterial.Articleurl = reader.GetValue<string>("articleurl");
                    wxmaterial.Author = reader.GetValue<string>("author");
                    wxmaterial.Imgpath = reader.GetValue<string>("imgpath");

                    wxmaterial.Operatime = reader.GetValue<DateTime>("Operatime");
                    wxmaterial.Summary = reader.GetValue<string>("Summary");

                    wxmaterial.Phone = reader.GetValue<string>("phone");
                    wxmaterial.Price = reader.GetValue<decimal>("price");

                    list.Add(wxmaterial);

                }
            }
            totalcount = list.Count;

            return list;
        }
        internal IList<WxMaterial> GetMaterialByPromoteType(int promotetypeid, out int totalcount)
        {
            string sql = "SELECT   [MaterialId],[title],[author],[imgpath],[summary],[article],[articleurl],[operatime],[applystate],[phone],[price],[SalePromoteTypeid]  FROM [EtownDB].[dbo].[WxMaterial] where SalePromoteTypeid=@promotetypeid order by sortid ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@promotetypeid", promotetypeid);

            List<WxMaterial> list = new List<WxMaterial>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    WxMaterial wxmaterial = new WxMaterial();

                    wxmaterial.MaterialId = reader.GetValue<int>("MaterialId");
                    wxmaterial.Title = reader.GetValue<string>("title");
                    wxmaterial.Applystate = reader.GetValue<bool>("applystate") == true ? 1 : 0;
                    wxmaterial.Article = reader.GetValue<string>("article");
                    wxmaterial.Articleurl = reader.GetValue<string>("articleurl");
                    wxmaterial.Author = reader.GetValue<string>("author");
                    wxmaterial.Imgpath = reader.GetValue<string>("imgpath");

                    wxmaterial.Operatime = reader.GetValue<DateTime>("Operatime");
                    wxmaterial.Summary = reader.GetValue<string>("Summary");

                    wxmaterial.Phone = reader.GetValue<string>("phone");
                    wxmaterial.Price = reader.GetValue<decimal>("price");

                    list.Add(wxmaterial);

                }
            }
            totalcount = list.Count;

            return list;
        }

        internal int SortMaterial(string materialid, int sortid)
        {
            string sql = "update WxMaterial set sortid=@sortid where materialid =@materialid ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@materialid", materialid);
            cmd.AddParam("@sortid", sortid);

            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        ///  插入Reservation数据
        /// </summary>
        /// <returns></returns>
        internal int insert_Res(Reservation model)
        {
            string sqltxt = "";
            sqltxt = @"INSERT INTO Reservation(comid,name,phone,titile,number,submit_name,Rstatic,Rdate,ip,WxMaterialid,resdate
           ,[checkoutdate]
           ,[roomtypeid]
           ,[totalprice]
           ,[lastercheckintime]
           ,[subdate],uid)VALUES
            (@comid,@name,@phone,@titile,@number,@submit_name,@Rstatic,@Rdate,@ip,@WxMaterialid,@resdate
           ,@checkoutdate
           ,@roomtypeid
           ,@totalprice
           ,@lastercheckintime
           ,@subdate
           ,@uid);SELECT @@IDENTITY;";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@comid", model.Comid);
            cmd.AddParam("@name", model.Name);
            cmd.AddParam("@phone", model.Phone);
            cmd.AddParam("@titile", model.Titile);
            cmd.AddParam("@number", model.Number);
            cmd.AddParam("@submit_name", model.Submit_name);
            cmd.AddParam("@Rstatic", model.Rstatic1);
            cmd.AddParam("@Rdate", model.Rdate1);
            cmd.AddParam("@ip", model.Ip);
            cmd.AddParam("@WxMaterialid", model.WxMaterialid);
            cmd.AddParam("@resdate", model.Resdate);
            cmd.AddParam("@checkoutdate", model.Checkoutdate);
            cmd.AddParam("@roomtypeid", model.Roomtypeid);
            cmd.AddParam("@totalprice", model.Totalprice);
            cmd.AddParam("@lastercheckintime", model.Lastercheckintime);
            cmd.AddParam("@subdate", model.Subdate);
            cmd.AddParam("@uid", model.Uid);
            //cmd.ExecuteNonQuery();

            object o = cmd.ExecuteScalar();
            int id = 0;
            if (o != null)
            {
                id = int.Parse(o.ToString());
            }

            return id;
        }

        internal List<Reservation> Res_LoadingList(string comid, int pageindex, int pagesize, out int totalcount, int userid = 0)
        {
            int channelcompanyid = 0;
            if (userid > 0)
            {
                try
                {
                    string sqlq = "select channelcompanyid from b2b_company_manageuser where id=" + userid;
                    var cmdd = sqlHelper.PrepareTextSqlCommand(sqlq);
                    object o = cmdd.ExecuteScalar();
                    sqlHelper.Dispose();
                    if (o == null || o.ToString() == "")
                    {
                        channelcompanyid = 0;
                    }
                    else
                    {
                        channelcompanyid = int.Parse(o.ToString());
                    }
                }
                catch 
                {
                    sqlHelper.Dispose();
                    channelcompanyid = 0;
                }
            }


            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "Reservation";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";
            var condition = "comid=" + comid;
            if (channelcompanyid > 0)
            {
                condition += " and uid in (select  id from b2b_crm where idcard in (select cardcode from member_card where issuecard in (select id from member_channel where companyid=" + channelcompanyid + ")) )";
            }

            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);



            List<Reservation> list = new List<Reservation>();

            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Reservation
                    {
                        Id = reader.GetValue<int>("Id"),
                        Comid = reader.GetValue<int>("Comid"),
                        Name = reader.GetValue<string>("Name"),
                        Phone = reader.GetValue<decimal>("Phone"),
                        Titile = reader.GetValue<string>("Titile"),
                        Number = reader.GetValue<int>("Number"),
                        Submit_name = reader.GetValue<string>("Submit_name"),
                        Rstatic1 = reader.GetValue<int>("Rstatic"),
                        Rdate1 = reader.GetValue<DateTime>("Rdate"),
                        Ip = reader.GetValue<string>("Ip"),
                        WxMaterialid = reader.GetValue<int>("WxMaterialid"),
                        Resdate = reader.GetValue<DateTime>("resdate")
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }

        internal Reservation Res_id(int id, int comid)
        {
            string sqlTxt = "";
            sqlTxt = @"SELECT [id]
      ,[comid]
      ,[name]
      ,[phone]
      ,[titile]
      ,[number]
      ,[submit_name]
      ,[Rstatic]
      ,[Rdate]
      ,[ip]
      ,[WxMaterialid]
      ,[resdate]
  FROM [EtownDB].[dbo].[Reservation] where  comid=@comid and id=@id";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@id", id);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    Reservation Reserva = new Reservation();
                    Reserva.Id = reader.GetValue<int>("Id");
                    Reserva.Comid = reader.GetValue<int>("Comid");
                    Reserva.Name = reader.GetValue<string>("Name");
                    Reserva.Phone = reader.GetValue<decimal>("Phone");
                    Reserva.Titile = reader.GetValue<string>("Titile");
                    Reserva.Number = reader.GetValue<int>("Number");
                    Reserva.Submit_name = reader.GetValue<string>("Submit_name");
                    Reserva.Rstatic1 = reader.GetValue<int>("Rstatic");
                    Reserva.Rdate1 = reader.GetValue<DateTime>("Rdate");
                    Reserva.Ip = reader.GetValue<string>("Ip");
                    Reserva.WxMaterialid = reader.GetValue<int>("WxMaterialid");
                    Reserva.Resdate = reader.GetValue<DateTime>("resdate");
                    reader.Close();

                    return Reserva;
                }
                else
                {
                    return null;
                }
            }


        }

        internal List<Reservation> ResSearchList(string comid, int pageindex, int pagesize, string key, bool isNum, out int totalcount)
        {
            string sqlTxt = "";
            int countnum = 0;
            if (isNum)
            {
                sqlTxt = @"SELECT [id]
      ,[comid]
      ,[name]
      ,[phone]
      ,[titile]
      ,[number]
      ,[submit_name]
      ,[Rstatic]
      ,[Rdate]
      ,[ip]
      ,[WxMaterialid]
      ,[resdate]
  FROM [EtownDB].[dbo].[Reservation] where  comid=@comid and phone=@key";
            }
            else
            {
                sqlTxt = @"SELECT [id]
      ,[comid]
      ,[name]
      ,[phone]
      ,[titile]
      ,[number]
      ,[submit_name]
      ,[Rstatic]
      ,[Rdate]
      ,[ip]
      ,[WxMaterialid]
      ,[resdate]
  FROM [EtownDB].[dbo].[Reservation] where comid=@comid and (name=@key or titile like '%@key%' )";
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@key", key);
            cmd.AddParam("@comid", comid);


            List<Reservation> list = new List<Reservation>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Reservation
                    {
                        Id = reader.GetValue<int>("Id"),
                        Comid = reader.GetValue<int>("Comid"),
                        Name = reader.GetValue<string>("Name"),
                        Phone = reader.GetValue<decimal>("Phone"),
                        Titile = reader.GetValue<string>("Titile"),
                        Number = reader.GetValue<int>("Number"),
                        Submit_name = reader.GetValue<string>("Submit_name"),
                        Rstatic1 = reader.GetValue<int>("Rstatic"),
                        Rdate1 = reader.GetValue<DateTime>("Rdate"),
                        Ip = reader.GetValue<string>("Ip"),
                        WxMaterialid = reader.GetValue<int>("WxMaterialid"),
                        Resdate = reader.GetValue<DateTime>("resdate")
                    });
                    countnum = countnum + 1;
                }
            }
            totalcount = countnum;

            return list;
        }

        #region 修改预订信息

        public int upRes(Reservation queren)
        {
            const string sqlTxt = @"Update [Reservation] set  submit_name=@Submit_name,Rstatic=@Rstatic,ip=@Ip,Remarks=@Remarks where id=@id and comid=@Comid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@Id", queren.Id);
            cmd.AddParam("@Submit_name", queren.Submit_name);
            cmd.AddParam("@Rstatic", queren.Rstatic1);
            cmd.AddParam("@Comid", queren.Comid);
            cmd.AddParam("@Ip", queren.Ip);
            cmd.AddParam("@Remarks", queren.Remarks);
            cmd.ExecuteNonQuery();
            return queren.Id;
        }
        #endregion

        internal periodical GetPeriodicalBySaleType(int comid, int saletype)
        {
            string sql = "SELECT  top 1  *  FROM periodical where comid=@comid and  Wxsaletypeid=@Wxsaletypeid  and id in (select periodicalid from WxMaterial where comid=@comid and SalePromoteTypeid=@Wxsaletypeid) order by percal desc ";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@Wxsaletypeid", saletype);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    periodical period = new periodical();

                    period.Id = reader.GetValue<int>("id");
                    period.Comid = reader.GetValue<int>("comid");
                    period.Wxsaletypeid1 = reader.GetValue<int>("Wxsaletypeid");
                    period.Percal = reader.GetValue<int>("percal");
                    period.Peryear = reader.GetValue<int>("peryear");
                    period.Uptime = reader.GetValue<DateTime>("uptime");

                    reader.Close();

                    return period;
                }
                else
                {
                    return null;
                }
            }
        }

        internal List<WxMaterial> PageList(int pageindex, int pagesize, int applystate, int promotetypeid, int perical, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "WxMaterial";
            var strGetFields = "*";
            var sortKey = "sortid";
            var sortMode = "0";

            var condition = "SalePromoteTypeid=" + promotetypeid + " and periodicalid=" + perical;
            if (applystate != 10)
            {
                condition += " and applystate=" + applystate;
            }


            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<WxMaterial> list = new List<WxMaterial>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    WxMaterial wxmaterial = new WxMaterial();

                    wxmaterial.MaterialId = reader.GetValue<int>("MaterialId");
                    wxmaterial.Title = reader.GetValue<string>("title");
                    wxmaterial.Applystate = reader.GetValue<bool>("applystate") == true ? 1 : 0;
                    wxmaterial.Article = reader.GetValue<string>("article");
                    wxmaterial.Articleurl = reader.GetValue<string>("articleurl");
                    wxmaterial.Author = reader.GetValue<string>("author");
                    wxmaterial.Imgpath = reader.GetValue<string>("imgpath");

                    wxmaterial.Operatime = reader.GetValue<DateTime>("Operatime");
                    wxmaterial.Summary = reader.GetValue<string>("Summary");

                    wxmaterial.Phone = reader.GetValue<string>("phone");
                    wxmaterial.Price = reader.GetValue<decimal>("price");
                    wxmaterial.SalePromoteTypeid = reader.GetValue<int>("SalePromoteTypeid");



                    list.Add(wxmaterial);

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }
        internal List<WxMaterial> PageList(int pageindex, int pagesize, int applystate, int promotetypeid, int perical, int comid, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "WxMaterial";
            var strGetFields = "*";
            var sortKey = "sortid";
            var sortMode = "0";

            var condition = "SalePromoteTypeid=" + promotetypeid + " and periodicalid=" + perical + " and comid=" + comid;
            if (applystate != 10)
            {
                condition += " and applystate=" + applystate;
            }


            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<WxMaterial> list = new List<WxMaterial>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    WxMaterial wxmaterial = new WxMaterial();

                    wxmaterial.MaterialId = reader.GetValue<int>("MaterialId");
                    wxmaterial.Title = reader.GetValue<string>("title");
                    wxmaterial.Applystate = reader.GetValue<bool>("applystate") == true ? 1 : 0;
                    wxmaterial.Article = reader.GetValue<string>("article");
                    wxmaterial.Articleurl = reader.GetValue<string>("articleurl");
                    wxmaterial.Author = reader.GetValue<string>("author");
                    wxmaterial.Imgpath = reader.GetValue<string>("imgpath");

                    wxmaterial.Operatime = reader.GetValue<DateTime>("Operatime");
                    wxmaterial.Summary = reader.GetValue<string>("Summary");

                    wxmaterial.Phone = reader.GetValue<string>("phone");
                    wxmaterial.Price = reader.GetValue<decimal>("price");
                    wxmaterial.SalePromoteTypeid = reader.GetValue<int>("SalePromoteTypeid");



                    list.Add(wxmaterial);

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }
        internal int Editperiod(periodical model)
        {
            string procname = "usp_InsertOrUpdateWxPeriod";
            var cmd = sqlHelper.PrepareStoredSqlCommand(procname);

            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Comid", model.Comid);
            cmd.AddParam("@Wxsaletypeid", model.Wxsaletypeid1);
            cmd.AddParam("@Percal", model.Percal);
            cmd.AddParam("@Peryear", model.Peryear);
            cmd.AddParam("@Uptime", model.Uptime);
            cmd.AddParam("@Perinfo", model.Perinfo);



            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        //插入设定，wxmaid唯一键，如果有重复插入失败
        internal int FrowardingSet(int materialid, int comid)
        {
            try
            {
                string sql = "insert member_forwarding_set(comid,wxmaid) values(@comid,@materialid)";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@materialid", materialid);
                cmd.AddParam("@comid", comid);
                return cmd.ExecuteNonQuery();
            }
            catch
            {
                return 0;
            }
        }
        //取消设定
        internal int FrowardingSetCannel(int materialid, int comid)
        {
            try
            {
                string sql = "delete member_forwarding_set where comid=@comid and wxmaid=@materialid";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@materialid", materialid);
                cmd.AddParam("@comid", comid);
                return cmd.ExecuteNonQuery();
            }
            catch
            {
                return 0;
            }
        }

        //查询是否有此设定，如果有返回1没有返回0
        internal int FrowardingSetSearch(int materialid)
        {
            string sql = "SELECT  *  FROM member_forwarding_set where wxmaid=@materialid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@materialid", materialid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        //查询是否有此设定，如果有返回1没有返回0
        internal int FrowardingSetList(int comid)
        {
            string sql = "SELECT top 1 *  FROM member_forwarding_set where comid=@comid order by id desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("wxmaid");
                }
            }
            return 0;
        }

        internal int GetWxMaterialCountByPercal(int comid, int Wxsaletypeid, int percal)
        {
            //string sql = "select count(1) from WxMaterial where salepromotetypeid=" + Wxsaletypeid + "  and  comid=" + comid + " and   periodicalid=" + percal;
            string sql = "select count(1) from WxMaterial where salepromotetypeid=" + Wxsaletypeid + "  and  comid=" + comid + " and   periodicalid in" +
                          "( select id from periodical where comid=" + comid + " and Wxsaletypeid=" + Wxsaletypeid + " and percal=" + percal + ")";


            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            return int.Parse(o.ToString());

        }

        internal int GetNewestPerical(int Wxsaletypeid, int comid)
        {
            try
            {
                string sql = "select  max(id) from periodical where comid=" + comid + " and wxsaletypeid=" + Wxsaletypeid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return int.Parse(o.ToString());
            }
            catch (Exception ex)
            {
                sqlHelper.Dispose();
                return 1;
            }
        }


        internal int Delmaterialtype(int typeid, int comid)
        {
           
            try
            {
                sqlHelper.BeginTrancation();
                SqlCommand cmd = new SqlCommand();
                string sql1 = "delete WxSalePromoteType where id=" + typeid + " and comid=" + comid;
                cmd= sqlHelper.PrepareTextSqlCommand(sql1);
                cmd.ExecuteNonQuery();

                string sql2 = "delete WxMaterial where SalePromoteTypeid=" + typeid + " and comid=" + comid;
                cmd= sqlHelper.PrepareTextSqlCommand(sql2);
                cmd.ExecuteNonQuery();


                sqlHelper.Commit();
                return 1;
            }
            catch
            {
                sqlHelper.Rollback();
                return 0;
            }
            finally
            {
                sqlHelper.Dispose();
            }

        }

        internal IList<WxMaterial> GetWxMateriallistbytypeid(int wxmaterialtypeid, int topnums)
        {
            string sql = "select top " + topnums + " * from WxMaterial where SalePromoteTypeid=" + wxmaterialtypeid + " and applystate=1";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using(var reader=cmd.ExecuteReader())
            {
                IList<WxMaterial> list = new List<WxMaterial>();
                while(reader.Read())
                {
                    WxMaterial wxmaterial = new WxMaterial();

                    wxmaterial.MaterialId = reader.GetValue<int>("MaterialId");
                    wxmaterial.Title = reader.GetValue<string>("title");
                    wxmaterial.Applystate = reader.GetValue<bool>("applystate") == true ? 1 : 0;
                    wxmaterial.Article = reader.GetValue<string>("article");
                    wxmaterial.Articleurl = reader.GetValue<string>("articleurl");
                    wxmaterial.Author = reader.GetValue<string>("author");
                    wxmaterial.Imgpath = reader.GetValue<string>("imgpath");
                    wxmaterial.Operatime = reader.GetValue<DateTime>("Operatime");
                    wxmaterial.Summary = reader.GetValue<string>("Summary");

                    wxmaterial.Phone = reader.GetValue<string>("phone");
                    wxmaterial.Price = reader.GetValue<decimal>("price");
                    wxmaterial.SalePromoteTypeid = reader.GetValue<int>("SalePromoteTypeid");

                     
                    list.Add(wxmaterial);
                }
                return list;
            }
        }
    }
}
