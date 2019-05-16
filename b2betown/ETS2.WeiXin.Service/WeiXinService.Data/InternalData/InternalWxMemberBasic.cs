using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS.Framework;
using System.Data;
using ETS2.WeiXin.Service.WeiXinService.Model;

namespace ETS2.WeiXin.Service.WeiXinService.Data.InternalData
{
    public class InternalWxMemberBasic
    {
        private SqlHelper sqlHelper;
        public InternalWxMemberBasic(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal bool JudgeExists(string openid, out int wxmemberbasicid)
        {

            var cmd = sqlHelper.PrepareStoredSqlCommand("Proc_JudgeExistsWxMemberBasic");
            cmd.AddParam("@openid", openid);
            cmd.AddOutParam("@id", SqlDbType.Int, 32);



            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            wxmemberbasicid = int.Parse(cmd.Parameters[1].Value.ToString());
            if ((int)parm.Value == 0)//操作出错
            {
                return false;
            }//操作成功
            else
            {
                if (wxmemberbasicid == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
                //return true;

            }

        }

        internal int EditWxMemberBasic(Model.WxMemberBasic model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand("usp_InsertOrUpdateWxMemberBasic");
            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@subscribe", model.Subscribe);
            cmd.AddParam("@openid", model.Openid);
            cmd.AddParam("@nickname", model.Nickname);
            cmd.AddParam("@sex", model.Sex);
            cmd.AddParam("@language", model.Language);
            cmd.AddParam("@city", model.City);
            cmd.AddParam("@province", model.Province);
            cmd.AddParam("@country", model.Country);
            cmd.AddParam("@headimgurl", model.Headimgurl);
            cmd.AddParam("@subscribe_time", model.Subscribe_time);
            cmd.AddParam("@comid", model.Comid);
            cmd.AddParam("@renewtime", model.renewtime);


            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }

        internal IList<string> GetWxmemberCountry()
        {
            string sql = "select distinct(wxcountry) as wxcountry from b2b_crm where wxcountry !='' order by wxcountry desc";

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    List<string> list = new List<string>();
                    while (reader.Read())
                    {
                        list.Add(reader.GetValue<string>("wxcountry"));
                    }
                    return list;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        internal IList<string> GetWxMemberProvince(string country)
        {
            string sql = "select distinct(wxprovince) as wxprovince from b2b_crm where wxprovince!='' and  wxcountry ='" + country + "' order by wxprovince desc";

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    List<string> list = new List<string>();
                    while (reader.Read())
                    {
                        list.Add(reader.GetValue<string>("wxprovince"));
                    }
                    return list;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        internal IList<string> GetWxMemberCity(string province)
        {
            string sql = "select distinct(wxcity) as wxcity from b2b_crm where wxcity!='' and  wxprovince ='" + province + "'  order by wxcity desc";

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    List<string> list = new List<string>();
                    while (reader.Read())
                    {
                        list.Add(reader.GetValue<string>("wxcity"));
                    }
                    return list;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        internal WxMemberBasic Getwxmemberbasic(string fromusername)
        {
            string sql = @"SELECT  [id]
                          ,[subscribe]
                          ,[openid]
                          ,[nickname]
                          ,[sex]
                          ,[language]
                          ,[city]
                          ,[province]
                          ,[country]
                          ,[headimgurl]
                          ,[subscribe_time]
                          ,[comid]
                          ,renewtime
                      FROM  [WxMemberBasic] where openid=@openid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@openid",fromusername);
            using(var reader=cmd.ExecuteReader())
            {
                WxMemberBasic m = null;
                if(reader.Read())
                {
                    m = new WxMemberBasic 
                    {
                     Id=reader.GetValue<int>("id"),
                     Subscribe = reader.GetValue<int>("subscribe"),
                     Openid = reader.GetValue<string>("openid"),
                     Nickname = reader.GetValue<string>("nickname"),
                     Sex = reader.GetValue<int>("sex"),
                     Language = reader.GetValue<string>("language"),
                     City = reader.GetValue<string>("city"),
                     Province = reader.GetValue<string>("province"),
                     Country = reader.GetValue<string>("country"),
                     Headimgurl = reader.GetValue<string>("headimgurl"),
                     Subscribe_time = reader.GetValue<DateTime>("subscribe_time"),
                     Comid = reader.GetValue<int>("comid"),
                     renewtime = reader.GetValue<DateTime>("renewtime"), 
                    };
                }
                return m;
            }

        }
    }
}
