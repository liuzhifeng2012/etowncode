using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;
using System.Data.SqlClient;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalB2b_com_housetype
    {
        private SqlHelper sqlHelper;
        public InternalB2b_com_housetype(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        #region 添加或者编辑产品信息

        public int InsertOrUpdate(B2b_com_housetype model)
        {
            int result = 0;
            try
            {
                sqlHelper.BeginTrancation();
                SqlCommand cmd = new SqlCommand();
                string sql1 = "delete b2b_com_housetype where proid=" + model.Proid;
                cmd = sqlHelper.PrepareTextSqlCommand(sql1);
                cmd.ExecuteNonQuery();
                string sql2 = @"INSERT INTO b2b_com_housetype
			   (
			     proid,
			     bedtype,
			     wifi,
			     ReserveType,
			     Builtuparea,
			     floor,
			     bedwidth,
			     whetherextrabed,
			     extrabedprice,
			     largestguestnum,
			     [whethernon-smoking],
			     amenities,
			     Mediatechnology,
			     Foodanddrink,
			     ShowerRoom,
			     Breakfast,
			 
			     roomtyperemark,
			     comid,
			   
			     RecerceSMSPhone,
			     RecerceSMSName
			   )
		 VALUES
			   (
			   
			   @Proid,
			     @Bedtype,
			     @Wifi,
			     @ReserveType,
			     @Builtuparea,
			     @Floor,
			     @Bedwidth,
			     @Whetherextrabed,
			     @Extrabedprice,
			     @Largestguestnum,
			     @Whethernonsmoking,
			     @Amenities,
			     @Mediatechnology,
			     @Foodanddrink,
			     @ShowerRoom,
			     @Breakfast,
			    
			     @Roomtyperemark  ,
			     @Comid,
			  
			     @RecerceSMSPhone,
			     @RecerceSMSName
			   )";
                cmd = sqlHelper.PrepareTextSqlCommand(sql2);

                cmd.AddParam("@Proid", model.Proid);
                cmd.AddParam("@Bedtype", model.Bedtype);
                cmd.AddParam("@Wifi", model.Wifi);
                cmd.AddParam("@ReserveType", model.ReserveType);
                cmd.AddParam("@Builtuparea", model.Builtuparea);
                cmd.AddParam("@Floor", model.Floor);
                cmd.AddParam("@Bedwidth", model.Bedwidth);
                cmd.AddParam("@Whetherextrabed", model.Whetherextrabed);
                cmd.AddParam("@Extrabedprice", model.Extrabedprice);
                cmd.AddParam("@Largestguestnum", model.Largestguestnum);
                cmd.AddParam("@Whethernonsmoking", model.Whethernonsmoking);
                cmd.AddParam("@Amenities", model.Amenities);
                cmd.AddParam("@Mediatechnology", model.Mediatechnology);
                cmd.AddParam("@Foodanddrink", model.Foodanddrink);
                cmd.AddParam("@ShowerRoom", model.ShowerRoom);
                cmd.AddParam("@Breakfast", model.Breakfast);

                cmd.AddParam("@Roomtyperemark", model.Roomtyperemark);
                cmd.AddParam("@Comid", model.Comid);
                cmd.AddParam("@RecerceSMSName", model.RecerceSMSName);
                cmd.AddParam("@RecerceSMSPhone", model.RecerceSMSPhone);

                cmd.ExecuteNonQuery();


                sqlHelper.Commit();
                sqlHelper.Dispose();
                result = 1;
            }
            catch (Exception e)
            {

                sqlHelper.Rollback();
                sqlHelper.Dispose();
                result = -1;
            }
            
            return result;


        }
        #endregion

        #region 得到房型信息详情
        internal B2b_com_housetype GetB2b_com_housetype(int proid, int comid)
        {


            //查询是否为导入产品，如果是导入产品读取主产品的id，读取主产品日历
            string sql_f = "select * from B2b_com_pro  where id=" + proid;
            var cmd_f = sqlHelper.PrepareTextSqlCommand(sql_f);
            using (var reader = cmd_f.ExecuteReader())
            {
                if (reader.Read())
                {
                    if (reader.GetValue<int>("bindingid") != 0)
                    {
                        proid = reader.GetValue<int>("bindingid");
                    }
                }
            }


            string sql = @"SELECT [id]
      ,[proid]
      ,[bedtype]
      ,[wifi]
      ,[ReserveType]
      ,[Builtuparea]
      ,[floor]
      ,[bedwidth]
      ,[whetherextrabed]
      ,[extrabedprice]
      ,[largestguestnum]
      ,[whethernon-smoking]
      ,[amenities]
      ,[Mediatechnology]
      ,[Foodanddrink]
      ,[ShowerRoom]
      ,[Breakfast]
      ,[roomtyperemark]
      ,[comid]
     ,[RecerceSMSPhone]
      ,[RecerceSMSName]
  FROM b2b_com_housetype where proid=@proid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@proid", proid);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_com_housetype m = null;
                if (reader.Read())
                {
                    m = new B2b_com_housetype
                    {
                        Id = reader.GetValue<int>("id"),
                        Proid = reader.GetValue<int>("proid"),
                        Bedtype = reader.GetValue<string>("bedtype"),
                        Wifi = reader.GetValue<string>("wifi"),
                        ReserveType = reader.GetValue<int>("ReserveType"),
                        Builtuparea = reader.GetValue<string>("Builtuparea"),
                        Floor = reader.GetValue<string>("floor"),
                        Bedwidth = reader.GetValue<string>("bedwidth"),
                        Whetherextrabed = reader.GetValue<bool>("whetherextrabed"),
                        Extrabedprice = reader.GetValue<decimal>("extrabedprice"),
                        Largestguestnum = reader.GetValue<int>("largestguestnum"),
                        Whethernonsmoking = reader.GetValue<bool>("whethernon-smoking"),
                        Amenities = reader.GetValue<string>("amenities"),
                        Mediatechnology = reader.GetValue<string>("Mediatechnology"),
                        Foodanddrink = reader.GetValue<string>("Foodanddrink"),
                        ShowerRoom = reader.GetValue<string>("ShowerRoom"),
                        Breakfast = reader.GetValue<int>("Breakfast"),

                        Roomtyperemark = reader.GetValue<string>("roomtyperemark"),
                        Comid = reader.GetValue<int>("comid"),
                        RecerceSMSName = reader.GetValue<string>("RecerceSMSName"),
                        RecerceSMSPhone = reader.GetValue<string>("RecerceSMSPhone"),

                        Proname = new B2bComProData().GetProById(reader.GetValue<int>("proid").ToString()).Pro_name
                    };
                }
                return m;
            }


        }
        #endregion

        /**得到房型当前价格**/
        internal decimal GetHousetypeNowdayprice(int proid)
        {
            string sql = "select min(menprice) from B2b_com_LineGroupDate where lineid=" + proid + " and daydate>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and menprice >0";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                if (o == null)
                {
                    return 0;
                }
                else
                {
                    if (o.ToString() != "")
                    {
                        return decimal.Parse(o.ToString());
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception e)
            {
                sqlHelper.Dispose();
                return 0;
            }

        }


        /**得到房型当前价格**/
        internal decimal GetHousetypeNowdaypricebyprojectid(int projectid)
        {
            string sql = "select min(menprice) from B2b_com_LineGroupDate where (lineid in (select id from b2b_com_pro where projectid=" + projectid + ") or lineid in (select bindingid from b2b_com_pro where projectid=" + projectid + ") ) and daydate>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and menprice >0";

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                if (o == null)
                {
                    return 0;
                }
                else
                {
                    if (o.ToString() != "")
                    {
                        return decimal.Parse(o.ToString());
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception e)
            {
                sqlHelper.Dispose();
                return 0;
            }

        }


    }
}
