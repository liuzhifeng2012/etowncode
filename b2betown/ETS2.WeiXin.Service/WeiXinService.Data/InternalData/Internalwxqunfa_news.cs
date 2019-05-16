using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Model;

namespace ETS2.WeiXin.Service.WeiXinService.Data.InternalData
{
    public class Internalwxqunfa_news
    {
        private SqlHelper sqlHelper;
        public Internalwxqunfa_news(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal List<Wxqunfa_news> GetNewsListByRecordid(int recordid)
        {
            string sql = "select * from Wxqunfa_news where newsrecordid=" + recordid;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            List<Wxqunfa_news> list = new List<Wxqunfa_news>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Wxqunfa_news()
                    {
                        id = reader.GetValue<int>("id"),
                        thumb_media_id = reader.GetValue<string>("thumb_media_id"),
                        author = reader.GetValue<string>("author"),
                        title = reader.GetValue<string>("title"),
                        content_source_url = reader.GetValue<string>("content_source_url"),
                        content = reader.GetValue<string>("content"),
                        digest = reader.GetValue<string>("digest"),
                        show_cover_pic = reader.GetValue<int>("show_cover_pic"),
                        newsrecordid = reader.GetValue<int>("newsrecordid"),
                        thumb_url = reader.GetValue<string>("thumb_url"),
                        wxmaterialid = reader.GetValue<int>("wxmaterialid"),

                    });
                }
            }
            return list;

        }

         

        internal List<Wxqunfa_news> GetTop1NewsListByRecordid(int recordid)
        {
            string sql = "select top 1 * from Wxqunfa_news where newsrecordid=" + recordid;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            List<Wxqunfa_news> list = new List<Wxqunfa_news>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Wxqunfa_news()
                    {
                        id = reader.GetValue<int>("id"),
                        thumb_media_id = reader.GetValue<string>("thumb_media_id"),
                        author = reader.GetValue<string>("author"),
                        title = reader.GetValue<string>("title"),
                        content_source_url = reader.GetValue<string>("content_source_url"),
                        content = reader.GetValue<string>("content"),
                        digest = reader.GetValue<string>("digest"),
                        show_cover_pic = reader.GetValue<int>("show_cover_pic"),
                        newsrecordid = reader.GetValue<int>("newsrecordid"),
                        thumb_url = reader.GetValue<string>("thumb_url"),
                        wxmaterialid = reader.GetValue<int>("wxmaterialid"),

                    });
                }
            }
            return list;
        }
    }
}
