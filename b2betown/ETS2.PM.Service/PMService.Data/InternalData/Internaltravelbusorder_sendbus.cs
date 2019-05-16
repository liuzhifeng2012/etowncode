using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internaltravelbusorder_sendbus
    {
        public SqlHelper sqlHelper;
        public Internaltravelbusorder_sendbus(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }


        internal IList<Travelbusorder_sendbus> Gettravelbusorder_sendbusBylogid(int logid, out int totle)
        {
            string sql = @"SELECT [id]
      ,[travelbusorder_operlogid]
      ,[drivername]
      ,[driverphone]
      ,[licenceplate]
      ,[travelbus_model]
      ,[seatnum]
  FROM [EtownDB].[dbo].[travelbusorder_sendbus] where travelbusorder_operlogid=@logid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@logid", logid);
            using (var reader = cmd.ExecuteReader())
            {
                IList<Travelbusorder_sendbus> list = new List<Travelbusorder_sendbus>();
                while (reader.Read())
                {
                    list.Add(new Travelbusorder_sendbus
                    {
                        id = reader.GetValue<int>("id"),
                        travelbusorder_operlogid = reader.GetValue<int>("travelbusorder_operlogid"),
                        drivername = reader.GetValue<string>("drivername"),
                        driverphone = reader.GetValue<string>("driverphone"),
                        licenceplate = reader.GetValue<string>("licenceplate"),
                        travelbus_model = reader.GetValue<string>("travelbus_model"),
                        seatnum = reader.GetValue<int>("seatnum")

                    });
                }
                totle = list.Count;
                return list;
            }
        }

        internal string BusDetailstr(int proid, DateTime daydate)
        {
            try
            {
                string returnstr = "";

                sqlHelper.BeginTrancation();
                string sql = "select id from travelbusorder_operlog where proid=" + proid + " and traveldate='" + daydate + "'";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                int logid = o == null ? 0 : int.Parse(o.ToString());
                if (logid > 0)
                {
                    sql = "SELECT [id],[travelbusorder_operlogid],[drivername],[driverphone],[licenceplate],[travelbus_model],[seatnum] FROM [travelbusorder_sendbus] where travelbusorder_operlogid=" + logid;
                    cmd = sqlHelper.PrepareTextSqlCommand(sql);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            returnstr += reader.GetValue<int>("id") + "-" + reader.GetValue<string>("travelbus_model") + reader.GetValue<int>("seatnum") + ",";
                        }
                        returnstr = returnstr.Substring(0, returnstr.Length - 1);
                    }
                }

                sqlHelper.Commit();
                sqlHelper.Dispose();
                return returnstr;
            }
            catch
            {
                sqlHelper.Rollback();
                sqlHelper.Dispose();
                return "";
            }
           

        }

        internal Travelbusorder_sendbus Gettravelbus(int busid)
        {
            string sql = @"SELECT [id]
      ,[travelbusorder_operlogid]
      ,[drivername]
      ,[driverphone]
      ,[licenceplate]
      ,[travelbus_model]
      ,[seatnum]
  FROM [EtownDB].[dbo].[travelbusorder_sendbus] where id=@id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", busid);
            using (var reader = cmd.ExecuteReader())
            {
                Travelbusorder_sendbus r = null;
                if (reader.Read())
                {
                    r = new Travelbusorder_sendbus
                    {
                        id = reader.GetValue<int>("id"),
                        travelbusorder_operlogid = reader.GetValue<int>("travelbusorder_operlogid"),
                        drivername = reader.GetValue<string>("drivername"),
                        driverphone = reader.GetValue<string>("driverphone"),
                        licenceplate = reader.GetValue<string>("licenceplate"),
                        travelbus_model = reader.GetValue<string>("travelbus_model"),
                        seatnum = reader.GetValue<int>("seatnum")

                    };
                }
                return r;
            }
        }
    }
}
