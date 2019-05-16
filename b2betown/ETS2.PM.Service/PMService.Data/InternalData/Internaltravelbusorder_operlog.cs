using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internaltravelbusorder_operlog
    {
        public SqlHelper sqlHelper;
        public Internaltravelbusorder_operlog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int Edittravelbusorder_operlog(int operlogid, int proid, string proname, string gooutdate, string operremark,
            int bustotal, string busids, string travelbus_model, string seatnum, string licenceplate, string drivername, string driverphone, int userid, int comid, string issavebus)
        {
            sqlHelper.BeginTrancation();
            try
            {
                #region 派车记录表
                if (operlogid == 0)//添加派车处理日志
                {
                    string str1 = "INSERT INTO [travelbusorder_operlog]([proid],[proname],[traveldate],[opertime],[operremark],[opertor],[comid]) VALUES " +
                                 " (" + proid + ",'" + proname + "','" + gooutdate + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + operremark + "'," + userid + "," + comid + ");select @@identity;";
                    var cmd = sqlHelper.PrepareTextSqlCommand(str1);
                    object o = cmd.ExecuteScalar();
                    operlogid = o == null ? 0 : int.Parse(o.ToString());
                    if (operlogid == 0)
                    {
                        return 0;
                    }
                }
                else //编辑派车处理日志
                {
                    string str7 = "UPDATE  [travelbusorder_operlog] SET [proid] = " + proid + ",[proname] = '" + proname + "',[traveldate] = '" + gooutdate + "',[opertime] = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',[operremark] = '" + operremark + "',[opertor] = '" + userid + "',[comid] = '" + comid + "' WHERE id=" + operlogid;
                    var cmd = sqlHelper.PrepareTextSqlCommand(str7);
                    cmd.ExecuteNonQuery();
                }
                #endregion
                #region 车量详情

                string[] issavebusstr = issavebus.Split(',');//操作类型:0编辑车辆操作；1保存车辆操作；2删除车辆操作
                string[] busidsstr = busids.Split(',');
                string[] travelbus_modelstr = travelbus_model.Split(',');
                string[] seatnumstr = seatnum.Split(',');
                string[] licenceplatestr = licenceplate.Split(',');
                string[] drivernamestr = drivername.Split(',');
                string[] driverphonestr = driverphone.Split(',');
                for (int i = 0; i < bustotal; i++)
                {
                    if (issavebusstr[i] == "2")//删除车辆
                    {
                        string str3 = "delete  [travelbusorder_sendbus]   WHERE id=" + busidsstr[i];
                        var cmd = sqlHelper.PrepareTextSqlCommand(str3);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        if (busidsstr[i] == "0") //增加车辆
                        {

                            string str2 = "INSERT INTO  [travelbusorder_sendbus]([travelbusorder_operlogid],[drivername] ,[driverphone],[licenceplate],[travelbus_model] ,[seatnum]) VALUES " +
                                   " (" + operlogid + ",'" + drivernamestr[i] + "','" + driverphonestr[i] + "','" + licenceplatestr[i] + "','" + travelbus_modelstr[i] + "' ,'" + seatnumstr[i] + "')";
                            var cmd = sqlHelper.PrepareTextSqlCommand(str2);
                            cmd.ExecuteNonQuery();
                        }
                        else//编辑车辆
                        {

                            string str3 = "UPDATE  [travelbusorder_sendbus] SET [travelbusorder_operlogid] = '" + operlogid + "',[drivername] = '" + drivernamestr[i] + "',[driverphone] = '" + driverphonestr[i] + "',[licenceplate] = '" + licenceplatestr[i] + "',[travelbus_model] = '" + travelbus_modelstr[i] + "',[seatnum] = '" + seatnumstr[i] + "' WHERE id=" + busidsstr[i];
                            var cmd = sqlHelper.PrepareTextSqlCommand(str3);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                #endregion
                #region 记录表和订单的关联表
                //删除关联表数据，然后重新录入
                string str4 = "DELETE FROM  [travelbusorder_operlog_order] WHERE  travelbusorder_operlogid=" + operlogid;
                var cmd2 = sqlHelper.PrepareTextSqlCommand(str4);
                cmd2.ExecuteNonQuery();

                //查询出符合条件的订单
                string str5 = "select id  FROM b2b_order where  u_traveldate='" + DateTime.Parse(gooutdate).ToString("yyyy-MM-dd HH:mm:ss") + "' and pro_id=" + proid + " and order_state=4";
                cmd2 = sqlHelper.PrepareTextSqlCommand(str5);
                List<int> orderstr = new List<int>();
                using (var reader = cmd2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orderstr.Add(reader.GetValue<int>("id"));
                    }
                }

                //录入关联表
                foreach (int oid in orderstr)
                {
                    string str6 = "INSERT INTO  [travelbusorder_operlog_order]([travelbusorder_operlogid],[orderid]) VALUES(" + operlogid + "," + oid + ")";
                    cmd2 = sqlHelper.PrepareTextSqlCommand(str6);
                    cmd2.ExecuteNonQuery();
                }
                #endregion
                sqlHelper.Commit();
                sqlHelper.Dispose();
                return operlogid;
            }
            catch (Exception e)
            {
                sqlHelper.Rollback();
                sqlHelper.Dispose();
                return 0;
            }
          
        }

        internal int Ishasplanbus(int proid, DateTime daydate)
        {
            string sql = "select count(1) from travelbusorder_operlog where proid=" + proid + "  and traveldate='" + daydate + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            int r = o == null ? 0 : int.Parse(o.ToString());
            if (r > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
