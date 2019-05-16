using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalB2b_com_roomtype
    {
        private SqlHelper sqlHelper;
        public InternalB2b_com_roomtype(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        #region 添加或者编辑产品信息

        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateB2bComRoomType";

        public int InsertOrUpdate(B2b_com_roomtype model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);

            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Name", model.Name);
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
            cmd.AddParam("@Sms", model.Sms);
            cmd.AddParam("@Sortid", model.Sortid);
            cmd.AddParam("@Server_type", model.Server_type);
            cmd.AddParam("@Pro_type", model.Pro_type);
            cmd.AddParam("@Source_type", model.Source_type);
            cmd.AddParam("@Createuserid", model.Createuserid);
            cmd.AddParam("@Createtime", model.Createtime);
            cmd.AddParam("@Whetheravailabel", model.Whetheravailabel);
            cmd.AddParam("@Roomtyperemark", model.Roomtyperemark);
            cmd.AddParam("@Comid", model.Comid);
            cmd.AddParam("@Roomtypeimg", model.Roomtypeimg);
            cmd.AddParam("@RecerceSMSName", model.RecerceSMSName);
            cmd.AddParam("@RecerceSMSPhone", model.RecerceSMSPhone);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;

        }
        #endregion

        #region 得到房型信息详情
        internal B2b_com_roomtype GetRoomType(int roomtypeid, int comid)
        {
            string sql = @"SELECT [id]
      ,[name]
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
      ,[sms]
      ,[sortid]
      ,[server_type]
      ,[pro_type]
      ,[source_type]
      ,[createuserid]
      ,[createtime]
      ,[whetheravailabel]
      ,[roomtyperemark]
      ,[comid]
      ,[roomtypeimg]
     ,[RecerceSMSPhone]
      ,[RecerceSMSName]
  FROM [dbo].[b2b_com_roomtype] where id=@roomtypeid and comid=@comid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@roomtypeid", roomtypeid);

            var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new B2b_com_roomtype
                {
                    Id = reader.GetValue<int>("id"),
                    Name = reader.GetValue<string>("name"),
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
                    Sms = reader.GetValue<string>("sms"),
                    Sortid = reader.GetValue<int>("sortid"),
                    Server_type = reader.GetValue<int>("server_type"),
                    Pro_type = reader.GetValue<int>("pro_type"),
                    Source_type = reader.GetValue<int>("source_type"),
                    Createuserid = reader.GetValue<int>("createuserid"),
                    Createtime = reader.GetValue<DateTime>("createtime"),
                    Whetheravailabel = reader.GetValue<bool>("whetheravailabel"),
                    Roomtyperemark = reader.GetValue<string>("roomtyperemark"),
                    Comid = reader.GetValue<int>("comid"),

                    Roomtypeimg = reader.GetValue<int>("roomtypeimg"),
                    RecerceSMSName = reader.GetValue<string>("RecerceSMSName"),
                    RecerceSMSPhone = reader.GetValue<string>("RecerceSMSPhone")
                };
            }
            return null;

        }
        #endregion
        #region 获得房型类型列表
        internal List<B2b_com_roomtype> GetRoomTypePageList(int pageindex, int pagesize, int comid, out int totalcount)
        {


            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "b2b_com_roomtype";
            var strGetFields = "*";
            var sortKey = "sortid";
            var sortMode = "1";
            var condition = "comid=" + comid;
            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<B2b_com_roomtype> list = new List<B2b_com_roomtype>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_com_roomtype
                    {
                        Id = reader.GetValue<int>("id"),
                        Name = reader.GetValue<string>("name"),
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
                        Sms = reader.GetValue<string>("sms"),
                        Sortid = reader.GetValue<int>("sortid"),
                        Server_type = reader.GetValue<int>("server_type"),
                        Pro_type = reader.GetValue<int>("pro_type"),
                        Source_type = reader.GetValue<int>("source_type"),
                        Createuserid = reader.GetValue<int>("createuserid"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Whetheravailabel = reader.GetValue<bool>("whetheravailabel"),
                        Roomtyperemark = reader.GetValue<string>("roomtyperemark"),
                        Comid = reader.GetValue<int>("comid"),

                        Roomtypeimg = reader.GetValue<int>("roomtypeimg")
                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());
            return list;



        }
        #endregion


    }
}
