using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Model;

namespace ETS2.WeiXin.Service.WeiXinService.Data.InternalData
{

    public class InternalWxAd
    {
        private SqlHelper sqlHelper;
        public InternalWxAd(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }


        internal int Editwxad(Wxad adinfo)
        {
            try
            {
                string sql = "";
                sqlHelper.BeginTrancation();
                if (adinfo.Id > 0)
                {
                    sql = "update weixin_ad set Title='" + adinfo.Title + "',Adtype=" + adinfo.Adtype + ",Link='" + adinfo.Link + "',musicid=" + adinfo.Musicid + ",Author='" + adinfo.Author + "',Keyword='" + adinfo.Keyword + "',Applystate=" + adinfo.Applystate + ",Votecount=" + adinfo.Votecount + ",Lookcount=" + adinfo.Lookcount + " where id=" + adinfo.Id + " and comid=" + adinfo.Comid;
                    
                }
                else
                {
                    sql = "INSERT INTO weixin_ad(Comid,Title,Adtype,Link,musicid,Author,Keyword,Applystate,Votecount,Lookcount)" +
         " VALUES(" + adinfo.Comid + ",'" + adinfo.Title + "'," + adinfo.Adtype + ",'" + adinfo.Link + "'," + adinfo.Musicid + ",'" + adinfo.Author + "','" + adinfo.Keyword + "'," + adinfo.Applystate + "," + adinfo.Votecount + "," + adinfo.Lookcount + ")";

                }

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.ExecuteNonQuery();

                sqlHelper.Commit();
                sqlHelper.Dispose();
                return 1;
            }
            catch
            {

                sqlHelper.Rollback();
                sqlHelper.Dispose();
                return 0;
            }

        }
        /*获取微信客服列表(1.总公司登录显示全部2.门市登录显示本门市绑定客服)*/
        internal List<Wxad> Getwxadpagelist(int pageindex, int pagesize, int comid, out int totalcount, string key = "", int applystate=0)
        {
            string condition = " comid=" + comid;

            if (key != "")
            {
                condition += "  and  title like '%" + key + "%'";
            }
            if (applystate != 0) {
                condition += " and applystate= " + applystate;
            }


            var cmdd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            cmdd.PagingCommand1("weixin_ad", "*", "id desc", "", pagesize, pageindex, "", condition);

            List<Wxad> list = new List<Wxad>();
            using (var reader = cmdd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Wxad()
                    {
                        Id = reader.GetValue<int>("id"),
                        Comid = reader.GetValue<int>("Comid"),
                        Title = reader.GetValue<string>("Title"),
                        Adtype = reader.GetValue<int>("Adtype"),
                        Link = reader.GetValue<string>("Link"),
                        Author = reader.GetValue<string>("Author"),
                        Keyword = reader.GetValue<string>("Keyword"),
                        Applystate = reader.GetValue<int>("Applystate"),
                        Votecount = reader.GetValue<int>("Votecount"),
                        Lookcount = reader.GetValue<int>("Lookcount"),
                        Musicid = reader.GetValue<int>("Musicid"),
                    });
                }
            }
            totalcount = int.Parse(cmdd.Parameters[0].Value.ToString());
            return list;

        }

        internal int DelWxad(int id, int comid)
        {
            string sql = "delete weixin_ad where id=" + id + " and comid=" + comid;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        //对投票和浏览都进行加
        internal int Wxadaddcount(int id, int comid,int vadd,int ladd)
        {
            string sql = "update weixin_ad set Votecount=Votecount+ " + vadd + ",Lookcount=Lookcount+ " + ladd +"  where id=" + id + " and comid=" + comid;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal Wxad Getwxad(int id, int comid)
        {
            string sql = "SELECT    *  FROM weixin_ad where id=" + id + " and comid=" + comid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                Wxad m = null;
                if (reader.Read())
                {
                    m = new Wxad()
                    {
                        Id = reader.GetValue<int>("id"),
                        Comid = reader.GetValue<int>("Comid"),
                        Title = reader.GetValue<string>("Title"),
                        Adtype = reader.GetValue<int>("Adtype"),
                        Link = reader.GetValue<string>("Link"),
                        Author = reader.GetValue<string>("Author"),
                        Keyword = reader.GetValue<string>("Keyword"),
                        Applystate = reader.GetValue<int>("Applystate"),
                        Votecount = reader.GetValue<int>("Votecount"),
                        Lookcount = reader.GetValue<int>("Lookcount"),
                        Musicid = reader.GetValue<int>("Musicid"),
                    };
                }
                return m;
            }
        }



    }
}
