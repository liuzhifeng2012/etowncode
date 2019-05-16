using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.Common.Business;

namespace ETS2.WeiXin.Service.WeiXinService.Data.InternalData
{
    public class InternalWxAdImages
    {
         private SqlHelper sqlHelper;
         public InternalWxAdImages(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }


         internal int Editwxadimage(WxAdImages adinfo)
        {
            try
            {
                string sql = "";
                sqlHelper.BeginTrancation();
               if (adinfo.Id > 0)
                {
                    sql = "update weixin_ad_images set adid=" + adinfo.Adid + ",imageid=" + adinfo.Imageid + ",link='" + adinfo.Link + "',sort=" + adinfo.Sort + " where id=" + adinfo.Id ;

                }
               else
               {
                   sql = "INSERT INTO weixin_ad_images(adid,imageid,link)" +
        " VALUES(" + adinfo.Adid + "," + adinfo.Imageid + ",'" + adinfo.Link + "')";

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
         internal List<WxAdImages> Getwxadimagespagelist(int pageindex, int pagesize, int comid, int adid, out int totalcount, string key = "")
        {
            string condition = " adid=" + adid;


            var cmdd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            cmdd.PagingCommand1("weixin_ad_images", "*", "sort,id desc", "", pagesize, pageindex, "", condition);

            List<WxAdImages> list = new List<WxAdImages>();
            using (var reader = cmdd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new WxAdImages()
                    {
                        Id = reader.GetValue<int>("id"),
                        Adid = reader.GetValue<int>("Adid"),
                        Imageid = reader.GetValue<int>("imageid"),
                        Imageurl =  FileSerivce.GetImgUrl(reader.GetValue<int>("imageid")),
                        Link = reader.GetValue<string>("link"),
                        Sort = reader.GetValue<int>("sort"),

                    });
                }
            }
            totalcount = int.Parse(cmdd.Parameters[0].Value.ToString());
            return list;

        }

         internal int DelWxadimages(int id, int adid)
        {
            string sql = "delete weixin_ad_images where id=" + id +" and adid="+adid;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }


        internal int upWxadimages_sort(int id, int sort)
        {
            string sql = "update weixin_ad_images set sort="+sort+" where id=" + id ;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }


         internal WxAdImages Getwxadimages(int id, int aid)
        {
            string sql = "SELECT    *  FROM weixin_ad_images where id=" + id;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                WxAdImages m = null;
                if (reader.Read())
                {
                    m = new WxAdImages()
                    {
                        
                        Id = reader.GetValue<int>("id"),
                        Adid = reader.GetValue<int>("Adid"),
                        Imageid = reader.GetValue<int>("imageid"),
                        Link = reader.GetValue<string>("link"),
                        Sort = reader.GetValue<int>("sort"),

                    };
                }
                return m;
            }
        }


    }
}
