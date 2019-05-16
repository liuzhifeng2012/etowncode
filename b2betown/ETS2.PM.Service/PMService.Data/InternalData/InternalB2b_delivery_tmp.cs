using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalB2b_delivery_tmp
    {
        public SqlHelper sqlHelper;
        public InternalB2b_delivery_tmp(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int Uplimitbuytotalnum(int ComputedPriceMethod, string join_provinces, string deliverytypes, int tmpid, string tmpname, string join_deliverytype, string join_areas, string join_startstandards, string join_startfees, string join_addstandards, string join_addfees, int comid, int operor, out string errmsg)
        {
            try
            {
                sqlHelper.BeginTrancation();

                if (tmpid == 0)//录入操作
                {
                    //录入快递模板
                    string sql = "INSERT INTO  [B2b_delivery_tmp]  (ComputedPriceMethod,[tmpname],extypes ,[Operor] ,[opertime]  ,[comid])  VALUES (" + ComputedPriceMethod + ",'" + tmpname + "','" + deliverytypes + "' ," + operor + " ,'" + DateTime.Now + "'  ," + comid + ");select @@identity;";
                    var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                    object o = cmd.ExecuteScalar();

                    int newid = int.Parse(o.ToString());
                    tmpid = newid;

                    //录入快递价格
                    string[] join_deliverytype_arr = join_deliverytype.Split(',');
                    string[] join_areas_arr = join_areas.Split(',');
                    string[] join_provinces_arr = join_provinces.Split(',');
                    string[] join_startstandards_arr = join_startstandards.Split(',');
                    string[] join_startfees_arr = join_startfees.Split(',');
                    string[] join_addstandards_arr = join_addstandards.Split(',');
                    string[] join_addfees_arr = join_addfees.Split(',');

                    for (int i = 0; i < join_deliverytype_arr.Length; i++)
                    {
                        int Deftype = 0;//默认地区:0否；1是
                        if (join_areas_arr[i] == "默认")
                        {
                            Deftype = 1;
                        }



                        sql = "INSERT INTO  [B2b_delivery_cost] ([tmpid] ,[Extype]  ,[Deftype] ,province ,[City] ,[First_num] ,[First_price]  ,[Con_num] ,[Con_price]  ,[Comid])" +
  "  VALUES (" + newid + "  , " + join_deliverytype_arr[i] + "  , " + Deftype + " , '" + join_provinces_arr[i] + "' , '" + join_areas_arr[i] + "'  , " + join_startstandards_arr[i] + "  , '" + decimal.Parse(join_startfees_arr[i]) + "'    , " + join_addstandards_arr[i] + "  , '" + decimal.Parse(join_addfees_arr[i]) + "'   , " + comid + " )";

                        cmd = sqlHelper.PrepareTextSqlCommand(sql);
                        cmd.ExecuteNonQuery();
                    }
                }
                else //编辑操作
                {
                    string sql = "UPDATE  [B2b_delivery_tmp]  SET ComputedPriceMethod=" + ComputedPriceMethod + " , [tmpname] ='" + tmpname + "',extypes='" + deliverytypes + "'   ,[Operor] =" + operor + "   ,[opertime] ='" + DateTime.Now + "'    ,[comid] =" + comid + "   WHERE id=" + tmpid;
                    var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                    cmd.ExecuteNonQuery();

                    sql = "delete B2b_delivery_cost where tmpid=" + tmpid;
                    cmd = sqlHelper.PrepareTextSqlCommand(sql);
                    cmd.ExecuteNonQuery();

                    //录入快递价格
                    string[] join_deliverytype_arr = join_deliverytype.Split(',');
                    string[] join_areas_arr = join_areas.Split(',');
                    string[] join_provinces_arr = join_provinces.Split(',');
                    string[] join_startstandards_arr = join_startstandards.Split(',');
                    string[] join_startfees_arr = join_startfees.Split(',');
                    string[] join_addstandards_arr = join_addstandards.Split(',');
                    string[] join_addfees_arr = join_addfees.Split(',');


                    int newid = tmpid;
                    for (int i = 0; i < join_deliverytype_arr.Length; i++)
                    {
                        int Deftype = 0;//默认地区:0否；1是
                        if (join_areas_arr[i] == "默认")
                        {
                            Deftype = 1;
                        }
                        sql = "INSERT INTO  [B2b_delivery_cost] ([tmpid] ,[Extype]  ,[Deftype] ,province ,[City] ,[First_num] ,[First_price]  ,[Con_num] ,[Con_price]  ,[Comid])" +
  "  VALUES (" + newid + "  , " + join_deliverytype_arr[i] + "  , " + Deftype + " ,'" + join_provinces_arr[i] + "'  , '" + join_areas_arr[i] + "'  , " + join_startstandards_arr[i] + "  , '" + decimal.Parse(join_startfees_arr[i]) + "'    , " + join_addstandards_arr[i] + "  , '" + decimal.Parse(join_addfees_arr[i]) + "'   , " + comid + " )";

                        cmd = sqlHelper.PrepareTextSqlCommand(sql);
                        cmd.ExecuteNonQuery();
                    }

                }

                sqlHelper.Commit();
                sqlHelper.Dispose();
                errmsg = "";
                return tmpid;
            }
            catch (Exception e)
            {
                sqlHelper.Rollback();
                sqlHelper.Dispose();
                errmsg = e.Message;
                return 0;
            }
        }

        //internal B2b_delivery_tmp Gettmpbycomid(int comid)
        //{
        //    string sql = "select * from B2b_delivery_tmp where comid=" + comid;
        //    var cmd = sqlHelper.PrepareTextSqlCommand(sql);

        //    using (var reader = cmd.ExecuteReader())
        //    {
        //        B2b_delivery_tmp m = null;
        //        if (reader.Read())
        //        {
        //            m = new B2b_delivery_tmp()
        //            {
        //                id = reader.GetValue<int>("id"),
        //                tmpname = reader.GetValue<string>("tmpname"),
        //                extypes = reader.GetValue<string>("extypes")
        //            };
        //        }
        //        return m;
        //    }
        //}

        internal B2b_delivery_tmp Getdeliverytmp(int tmpid)
        {
            string sql = "select * from B2b_delivery_tmp where id=" + tmpid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_delivery_tmp m = null;
                if (reader.Read())
                {
                    m = new B2b_delivery_tmp()
                    {
                        id = reader.GetValue<int>("id"),
                        tmpname = reader.GetValue<string>("tmpname"),
                        extypes = reader.GetValue<string>("extypes"),
                        ComputedPriceMethod = reader.GetValue<int>("ComputedPriceMethod")
                    };
                }
                return m;
            }
        }

        internal IList<B2b_delivery_tmp> Getdeliverytmplist(int comid, out int totalcount)
        {
            string sql = "select * from B2b_delivery_tmp where comid=" + comid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            List<B2b_delivery_tmp> list = new List<B2b_delivery_tmp>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_delivery_tmp()
                    {
                        id = reader.GetValue<int>("id"),
                        tmpname = reader.GetValue<string>("tmpname"),
                        extypes = reader.GetValue<string>("extypes"),
                        comid = reader.GetValue<int>("comid"),

                    });
                }

                totalcount = list.Count;
                return list;
            }
        }

        internal IList<B2b_delivery_tmp> Getdeliverytmppagelist(int comid, int pageindex, int pagesize, out int totalcount)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            cmd.PagingCommand1("B2b_delivery_tmp", "*", "opertime desc", "", pagesize, pageindex, "", "comid=" + comid);

            List<B2b_delivery_tmp> list = new List<B2b_delivery_tmp>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_delivery_tmp()
                    {
                        id = reader.GetValue<int>("id"),
                        comid = reader.GetValue<int>("comid"),
                        extypes = reader.GetValue<string>("extypes"),
                        Operor = reader.GetValue<int>("Operor"),
                        opertime = reader.GetValue<DateTime>("opertime"),
                        tmpname = reader.GetValue<string>("tmpname"),
                        ComputedPriceMethod = reader.GetValue<int>("ComputedPriceMethod"),
                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;

        }

        internal int deltmp(int comid, int tmpid, out string errmsg)
        {

            //查询模板下是否含有在线产品:含有在线产品的话禁止删除，提醒去修改产品；不含的话删除运费模板，同时更改模板下产品运费模板为0(未设置模板)
            try
            {
                if (comid == 0 || tmpid == 0)
                {
                    errmsg = "传递参数失败";
                    return 0;
                }

                sqlHelper.BeginTrancation();
                string sql = "select id,pro_name from b2b_com_pro where pro_state=1 and  deliverytmp=" + tmpid + " and com_id=" + comid + " and ishasdeliveryfee=1";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);

                List<int> listid = new List<int>();
                List<string> listproname = new List<string>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listid.Add(reader.GetValue<int>("id"));
                        listproname.Add(reader.GetValue<string>("pro_name"));
                    }
                }
                if (listid.Count > 0)
                {
                    string str = "";
                    for (int i = 0; i < listid.Count; i++)
                    {
                        str += listproname[i] + "(" + listid[i] + "),";
                    }
                    if (str.Length > 0)
                    {
                        str = str.Substring(0, str.Length - 1);
                    }

                    sqlHelper.Commit();
                    sqlHelper.Dispose();
                    errmsg = "运费模板不可删除，产品" + str + "正在使用";
                    return 0;
                }
                else
                {
                    string sql1 = "update b2b_com_pro set deliverytmp=0 where  deliverytmp=" + tmpid + " and com_id=" + comid;
                    var cmd1 = sqlHelper.PrepareTextSqlCommand(sql1);
                    cmd1.ExecuteNonQuery();

                    string sql2 = "delete B2b_delivery_cost where tmpid=" + tmpid + " and comid=" + comid;
                    var cmd2 = sqlHelper.PrepareTextSqlCommand(sql2);
                    cmd2.ExecuteNonQuery();

                    string sql3 = "delete B2b_delivery_tmp where id=" + tmpid + " and comid=" + comid;
                    var cmd3 = sqlHelper.PrepareTextSqlCommand(sql3);
                    cmd3.ExecuteNonQuery();

                    sqlHelper.Commit();
                    sqlHelper.Dispose();
                    errmsg = "";
                    return 1;
                }

            }
            catch (Exception e)
            {
                sqlHelper.Rollback();
                sqlHelper.Dispose();
                errmsg = e.Message;
                return 0;
            }
        }

        internal bool WhetherSaled(int lineid, string daydate)
        {
            int servertype = new B2bComProData().GetProServer_typeById(lineid.ToString());
            if (servertype == 10 || servertype == 2 || servertype == 8)//旅游大巴、跟团游、当地游
            {
                try
                {
                    string sql = "select count(1) from b2b_order where order_state in (4,22) and  pro_id =" + lineid + " and convert(varchar(10),u_traveldate,120)='" + daydate + "'";
                    var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                    object o = cmd.ExecuteScalar();
                    int r = int.Parse(o.ToString());
                    if (r > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
            //客房
            else if (servertype == 2 || servertype == 8)
            {
                try
                {
                    string sql = "select count(1) from b2b_order where order_state in (4,22)  and  pro_id =" + lineid + "  and id in (select orderid from b2b_order_hotel where '" + daydate + "' >= convert(varchar(10),start_date,120) and  convert(varchar(10),end_date,120)>='" + daydate + "' )  ";
                    var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                    object o = cmd.ExecuteScalar();
                    int r = int.Parse(o.ToString());
                    if (r > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                } 

            }
            //其他服务类型以后需要家判断
            else
            {
                return false;
            }

        }
    }
}
