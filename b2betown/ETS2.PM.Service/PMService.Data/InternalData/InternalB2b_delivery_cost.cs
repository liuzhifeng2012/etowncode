using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalB2b_delivery_cost
    {
        public SqlHelper sqlHelper;
        public InternalB2b_delivery_cost(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal List<B2b_delivery_cost> GetB2b_delivery_costlist(int tmpid, out int totalcount)
        {
            string sql = "SELECT [id]  ,[tmpid]  ,[Extype]  ,[Deftype],province  ,[City]  ,[First_num]  ,[First_price]  ,[Con_num]   ,[Con_price]   ,[Comid] FROM  [B2b_delivery_cost] where tmpid=" + tmpid;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            List<B2b_delivery_cost> list = new List<B2b_delivery_cost>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_delivery_cost()
                    {
                        id = reader.GetValue<int>("id"),
                        Extype = reader.GetValue<int>("Extype"),
                        Deftype = reader.GetValue<int>("Deftype"),
                        Province = reader.GetValue<string>("Province"),
                        City = reader.GetValue<string>("city"),
                        tmpid = reader.GetValue<int>("tmpid"),
                        First_num = reader.GetValue<int>("First_num"),
                        First_price = reader.GetValue<decimal>("First_price"),
                        Comid = reader.GetValue<int>("comid"),
                        Con_num = reader.GetValue<int>("con_num"),
                        Con_price = reader.GetValue<decimal>("con_price")
                    });
                }
                totalcount = list.Count;
                return list;
            }


        }
        /// <summary>
        /// 产品运费计算
        /// </summary>
        /// <param name="proid"></param>
        /// <param name="city"></param>
        /// <param name="num"></param>
        /// <param name="errmsg"></param>
        /// <param name="deliverytype"></param>
        /// <returns></returns>
        internal decimal Getdeliverycost(int proid, string city, int num, out string errmsg, int deliverytype = 2)
        {
            sqlHelper.BeginTrancation();
            try
            {
                int ishasdeliveryfee = 0;
                int deliverytmp = 0;
                decimal pro_weight = 0;//产品重量
                int server_type = 0;//产品服务类型，现阶段只有11实物需要计算运费，其他产品运费为0
                int source_type=0;
                int bindingid = 0;
                #region 获得产品用的运费模板
                string sql = "select server_type, pro_weight,ishasdeliveryfee ,deliverytmp,source_type,bindingid from b2b_com_pro where id=" + proid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ishasdeliveryfee = reader.GetValue<int>("ishasdeliveryfee");
                        deliverytmp = reader.GetValue<int>("deliverytmp");
                        pro_weight = reader.GetValue<decimal>("pro_weight");
                        server_type = reader.GetValue<int>("server_type");
                        source_type= reader.GetValue<int>("source_type");
                        bindingid = reader.GetValue<int>("bindingid");
                    }
                }

                if (source_type == 4) {

                    string sql2 = "select server_type, pro_weight,ishasdeliveryfee ,deliverytmp,source_type from b2b_com_pro where id=" + bindingid;
                    var cmd2 = sqlHelper.PrepareTextSqlCommand(sql2);
                    using (var reader = cmd2.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ishasdeliveryfee = reader.GetValue<int>("ishasdeliveryfee");
                            deliverytmp = reader.GetValue<int>("deliverytmp");
                            pro_weight = reader.GetValue<decimal>("pro_weight");
                            server_type = reader.GetValue<int>("server_type");
                        }
                    }
                
                
                }


                if (server_type != 11)//非实物产品没有运费计算
                {
                    sqlHelper.Commit();
                    sqlHelper.Dispose();
                    errmsg = "";
                    return 0;
                }
                #endregion
                #region 包邮
                if (ishasdeliveryfee == 0)//包邮
                {
                    sqlHelper.Commit();
                    sqlHelper.Dispose();
                    errmsg = "";
                    return 0;
                }
                #endregion
                #region 不包邮,默认计算快递价格
                else //不包邮,默认计算快递价格
                {
                    #region 判断是否含有选择的运费模板
                    sql = "select count(1) from B2b_delivery_cost where tmpid=" + deliverytmp + " and Extype=" + deliverytype;
                    cmd = sqlHelper.PrepareTextSqlCommand(sql);
                    object lo = cmd.ExecuteScalar();
                    if (int.Parse(lo.ToString()) == 0)
                    {
                        sqlHelper.Commit();
                        sqlHelper.Dispose();
                        errmsg = "产品不支持当前快递方式，请换其他方式";
                        return 0;
                    }
                    #endregion

                    #region 判断是否在指定城市内:存在于指定地区内，按指定地区运费价格;不存在于指定地区内，按默认运费价格
                    //判断是否在指定城市内
                    sql = "select   count(1)  FROM  [B2b_delivery_cost] where tmpid=" + deliverytmp + "  and city like '%" + city + "%' and [Extype]=" + deliverytype;
                    cmd = sqlHelper.PrepareTextSqlCommand(sql);
                    object o = cmd.ExecuteScalar();


                    int First_num = 0;
                    decimal First_price = 0;
                    int Con_num = 0;
                    decimal Con_price = 0;

                    if (int.Parse(o.ToString()) > 0)//存在于指定地区内，按指定地区运费价格
                    {
                        sql = "select   [First_num]  ,[First_price]   ,[Con_num]   ,[Con_price]     FROM  [B2b_delivery_cost] where tmpid=" + deliverytmp + " and city like '%" + city + "%'  and [Extype]=" + deliverytype;
                        cmd = sqlHelper.PrepareTextSqlCommand(sql);
                        using (var red = cmd.ExecuteReader())
                        {
                            if (red.Read())
                            {
                                First_num = red.GetValue<int>("First_num");
                                First_price = red.GetValue<decimal>("First_price");
                                Con_num = red.GetValue<int>("Con_num");
                                Con_price = red.GetValue<decimal>("Con_price");
                            }
                        }

                    }
                    else //不存在于指定地区内，按默认运费价格
                    {
                        sql = "select   [First_num]  ,[First_price]   ,[Con_num]   ,[Con_price]     FROM  [B2b_delivery_cost] where tmpid=" + deliverytmp + " and  [Extype]=" + deliverytype + "  and [Deftype]=1";
                        cmd = sqlHelper.PrepareTextSqlCommand(sql);
                        using (var red = cmd.ExecuteReader())
                        {
                            if (red.Read())
                            {
                                First_num = red.GetValue<int>("First_num");
                                First_price = red.GetValue<decimal>("First_price");
                                Con_num = red.GetValue<int>("Con_num");
                                Con_price = red.GetValue<decimal>("Con_price");
                            }
                        }
                    }
                    #endregion

                    #region 获取运费的计费方式:1按数量计费2按重量计费
                    //获取运费的计费方式:1按数量计费2按重量计费
                    sql = "select ComputedPriceMethod from B2b_delivery_tmp where id=" + deliverytmp;
                    cmd = sqlHelper.PrepareTextSqlCommand(sql);
                    object computerol = cmd.ExecuteScalar();
                    int computepricemethod = int.Parse(computerol.ToString());
                    #endregion

                    #region 计算运费
                    if (computepricemethod == 1)//1按数量计费
                    {

                        //获取运费
                        if (First_num >= num)//运送数量在首N件内，运费=首费(￥)
                        {
                            sqlHelper.Commit();
                            sqlHelper.Dispose();
                            errmsg = "";
                            return First_price;
                        }
                        else //运送数量在首N件外，运费=首费(￥)+续费(￥)
                        {
                            int add = (int)Math.Ceiling((double)(num - First_num) / (double)Con_num);
                            decimal addfee = add * Con_price;

                            sqlHelper.Commit();
                            sqlHelper.Dispose();
                            errmsg = "";
                            return First_price + addfee;
                        }
                    }
                    else//2按重量计费
                    {
                        decimal totalweight = pro_weight * num;
                        //获取运费
                        if (First_num >= totalweight)//运送重量在首重内，运费=首费(￥)
                        {
                            sqlHelper.Commit();
                            sqlHelper.Dispose();
                            errmsg = "";
                            return First_price;
                        }
                        else //运送重量在首中外，运费=首费(￥)+续费(￥)
                        {
                            int add = (int)Math.Ceiling((double)(totalweight - First_num) / (double)Con_num);
                            decimal addfee = add * Con_price;

                            sqlHelper.Commit();
                            sqlHelper.Dispose();
                            errmsg = "";
                            return First_price + addfee;
                        }

                    }

                    #endregion
                }
                #endregion
            }
            catch (Exception e)
            {
                sqlHelper.Rollback();
                sqlHelper.Dispose();
                errmsg = e.Message;
                return 0;
            }


        }

        /// <summary>
        /// 购物车运费计算:正确返回价格连接字符串，错误返回 “-1，错误原因”
        /// </summary>
        /// <param name="proidstr"></param>
        /// <param name="citystr"></param>
        /// <param name="numstr"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        internal decimal Getdeliverycost_ShopCart(string proidstr, string citystr, string numstr, out string feedetail)
        {
            string errmsg = "";//错误原因
            sqlHelper.BeginTrancation();
            try
            {

                int deliverytmp = 0;//统一运费模板(第一个不包邮的产品的运费模板)
                decimal pro_weight_total = 0;//产品总重量(不包邮产品的总重量)
                int pro_number_total = 0;//产品总数量(不包邮产品的总数量)
                int deliverytype = 2;//模板类型固定设为快递


                #region 得到购物车中 统一运费模板(第一个不包邮的产品的运费模板) 和 产品总重量(不包邮产品的总重量) 和 产品总数量(不包邮产品的总数量)/
                string[] proidarr = proidstr.Split(',');
                //string[] cityarr = citystr.Split(',');
                string[] numarr = numstr.Split(',');
                string[] ishasfeearr = new string[proidarr.Length];
                string[] proweightarr = new string[proidarr.Length];


                for (int i = 0; i < proidarr.Length; i++)
                {
                    if (proidarr[i].Length > 0 && citystr != "" && numarr[i].Length > 0)
                    {
                        //得到第一个产品的模板信息
                        string sqlL = "select server_type, pro_weight,ishasdeliveryfee ,deliverytmp,Bindingid from b2b_com_pro where id=" + proidarr[i];
                        var cmdL = sqlHelper.PrepareTextSqlCommand(sqlL);
                        using (var reader = cmdL.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                //判断如果是导入产品读取原始产品数据
                                if (reader.GetValue<int>("Bindingid") != 0)
                                {

                                    var b2b_proinfo = new B2bComProData().GetProById(reader.GetValue<int>("Bindingid").ToString());
                                    if (b2b_proinfo != null) {

                                        if (b2b_proinfo.Server_type == 11 && b2b_proinfo.ishasdeliveryfee == 1)
                                        {
                                            if (deliverytmp == 0)
                                            {
                                                deliverytmp = b2b_proinfo.deliverytmp;
                                            }
                                            pro_weight_total += int.Parse(numarr[i]) * b2b_proinfo.pro_weight;
                                            pro_number_total += int.Parse(numarr[i]);
                                            ishasfeearr[i] = "1";
                                            proweightarr[i] = (int.Parse(numarr[i]) * b2b_proinfo.pro_weight).ToString();
                                        }
                                        else//不含有运费的话，重量没有用到，直接赋值为0
                                        {
                                            ishasfeearr[i] = "0";
                                            proweightarr[i] = "0";
                                        }
                                    }

                                }
                                else
                                {
                                    
                                    if (reader.GetValue<int>("server_type") == 11 && reader.GetValue<int>("ishasdeliveryfee") == 1)
                                    {
                                        if (deliverytmp == 0)
                                        {
                                            deliverytmp = reader.GetValue<int>("deliverytmp");
                                        }
                                        pro_weight_total += int.Parse(numarr[i]) * reader.GetValue<decimal>("pro_weight");
                                        pro_number_total += int.Parse(numarr[i]);
                                        ishasfeearr[i] = "1";
                                        proweightarr[i] = (int.Parse(numarr[i]) * reader.GetValue<decimal>("pro_weight")).ToString();
                                    }
                                    else//不含有运费的话，重量没有用到，直接赋值为0
                                    {
                                        ishasfeearr[i] = "0";
                                        proweightarr[i] = "0";
                                    }
                                }




                            }
                        }
                    }
                }


                #endregion

                #region  购物车中产品都为包邮产品
                if (deliverytmp == 0)
                {
                    sqlHelper.Commit();
                    sqlHelper.Dispose();
                    errmsg = "产品都为包邮产品";
                    string feedetailstr = "";
                    for (int i = 0; i < proidarr.Length; i++)
                    {
                        feedetailstr += "0" + ",";
                    }
                    if (feedetailstr.Length > 0)
                    {
                        feedetail = feedetailstr.Substring(0, feedetailstr.Length - 1);
                    }
                    else
                    {
                        feedetail = "";
                    }
                    
                    return 0;
                }
                #endregion 
                #region 购物车中产品包含非包邮产品
                else
                {
                    #region  购物车中 不包邮产品都按第一个产品的运费模板计费:计费方式为按数量计费的话得到所有产品的件数；计费方式为按重量计费的话得到所有产品的重量

                    #region 判断是否含有选择的运费模板
                    string sql = "select count(1) from B2b_delivery_cost where tmpid=" + deliverytmp + " and Extype=" + deliverytype;
                    var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                    object lo = cmd.ExecuteScalar();
                    if (int.Parse(lo.ToString()) == 0)
                    {
                        sqlHelper.Commit();
                        sqlHelper.Dispose();
                        errmsg = "产品不支持当前快递方式，请换其他方式";
                        feedetail = "";
                        return -1;
                    }
                    #endregion

                    #region 判断是否在指定城市内:存在于指定地区内，按指定地区运费价格;不存在于指定地区内，按默认运费价格
                    //判断是否在指定城市内
                    sql = "select   count(1)  FROM  [B2b_delivery_cost] where tmpid=" + deliverytmp + "  and city like '%" + citystr + "%' and [Extype]=" + deliverytype;
                    cmd = sqlHelper.PrepareTextSqlCommand(sql);
                    object o = cmd.ExecuteScalar();


                    int First_num = 0;
                    decimal First_price = 0;
                    int Con_num = 0;
                    decimal Con_price = 0;

                    if (int.Parse(o.ToString()) > 0)//存在于指定地区内，按指定地区运费价格
                    {
                        sql = "select   [First_num]  ,[First_price]   ,[Con_num]   ,[Con_price]     FROM  [B2b_delivery_cost] where tmpid=" + deliverytmp + " and city like '%" + citystr + "%'  and [Extype]=" + deliverytype;
                        cmd = sqlHelper.PrepareTextSqlCommand(sql);
                        using (var red = cmd.ExecuteReader())
                        {
                            if (red.Read())
                            {
                                First_num = red.GetValue<int>("First_num");
                                First_price = red.GetValue<decimal>("First_price");
                                Con_num = red.GetValue<int>("Con_num");
                                Con_price = red.GetValue<decimal>("Con_price");
                            }
                        }

                    }
                    else //不存在于指定地区内，按默认运费价格
                    {
                        sql = "select   [First_num]  ,[First_price]   ,[Con_num]   ,[Con_price]     FROM  [B2b_delivery_cost] where tmpid=" + deliverytmp + " and  [Extype]=" + deliverytype + "  and [Deftype]=1";
                        cmd = sqlHelper.PrepareTextSqlCommand(sql);
                        using (var red = cmd.ExecuteReader())
                        {
                            if (red.Read())
                            {
                                First_num = red.GetValue<int>("First_num");
                                First_price = red.GetValue<decimal>("First_price");
                                Con_num = red.GetValue<int>("Con_num");
                                Con_price = red.GetValue<decimal>("Con_price");
                            }
                        }
                    }
                    #endregion

                    #region 获取运费的计费方式:1按数量计费2按重量计费
                    //获取运费的计费方式:1按数量计费2按重量计费
                    sql = "select ComputedPriceMethod from B2b_delivery_tmp where id=" + deliverytmp;
                    cmd = sqlHelper.PrepareTextSqlCommand(sql);
                    object computerol = cmd.ExecuteScalar();
                    int computepricemethod = int.Parse(computerol.ToString());
                    #endregion

                    #region 计算运费
                    if (computepricemethod == 1)//1按数量计费
                    {

                        //获取运费
                        if (First_num >= pro_number_total)//运送数量在首N件内，运费=首费(￥)
                        {
                            sqlHelper.Commit();
                            sqlHelper.Dispose();
                            errmsg = "";
                            feedetail = GetFeeDetail(First_price, proidarr, ishasfeearr, numarr, proweightarr, computepricemethod);
                            return First_price;
                        }
                        else //运送数量在首N件外，运费=首费(￥)+续费(￥)
                        {
                            int add = (int)Math.Ceiling((double)(pro_number_total - First_num) / (double)Con_num);
                            decimal addfee = add * Con_price;

                            sqlHelper.Commit();
                            sqlHelper.Dispose();
                            errmsg = "";
                            feedetail = GetFeeDetail(First_price + addfee, proidarr, ishasfeearr, numarr, proweightarr, computepricemethod);
                            return First_price + addfee;
                        }
                    }
                    else//2按重量计费
                    {
                        decimal totalweight = pro_weight_total;
                        //获取运费
                        if (First_num >= totalweight)//运送重量在首重内，运费=首费(￥)
                        {
                            sqlHelper.Commit();
                            sqlHelper.Dispose();
                            errmsg = "";
                            feedetail = GetFeeDetail(First_price, proidarr, ishasfeearr, numarr, proweightarr, computepricemethod);

                            return First_price;
                        }
                        else //运送重量在首中外，运费=首费(￥)+续费(￥)
                        {
                            int add = (int)Math.Ceiling((double)(totalweight - First_num) / (double)Con_num);
                            decimal addfee = add * Con_price;

                            sqlHelper.Commit();
                            sqlHelper.Dispose();
                            errmsg = "";
                            feedetail = GetFeeDetail(First_price + addfee, proidarr, ishasfeearr, numarr, proweightarr, computepricemethod);

                            return First_price + addfee;
                        }

                    }

                    #endregion


                    #endregion

                }
                #endregion

            }
            catch (Exception e)
            {
                sqlHelper.Rollback();
                sqlHelper.Dispose();
                errmsg = e.Message;
                feedetail = "";
                return 0;
            }

        }



        private string GetFeeDetail(decimal totalprice, string[] proidarr, string[] ishasfeearr, string[] numarr, string[] proweightarr, int computepricemethod)
        {
            string feedetail = "";
            if (computepricemethod == 1)//按数量计费
            {
                decimal totalnum = 0;
                //计算不包邮总数量
                for (int i = 0; i < proidarr.Length; i++)
                {
                    if (ishasfeearr[i] == "1")
                    {
                        totalnum += decimal.Parse(numarr[i]);
                    }
                }
                //计算出运费详情
                for (int i = 0; i < proidarr.Length; i++)
                {
                    if (ishasfeearr[i] == "1")
                    {
                        feedetail += (totalprice * (decimal.Parse(numarr[i]) / totalnum)).ToString("F2") + ",";
                    }
                    else
                    {
                        feedetail += "0" + ",";
                    }
                }

            }
            else if (computepricemethod == 2)//按重量计费
            {
                //计算不包邮总重量
                decimal totalweight = 0;
                for (int i = 0; i < proidarr.Length; i++)
                {
                    if (ishasfeearr[i] == "1")
                    {
                        totalweight += decimal.Parse(proweightarr[i]);
                    }
                }
                //计算出运费详情
                for (int i = 0; i < proidarr.Length; i++)
                {
                    if (ishasfeearr[i] == "1")
                    {
                        if (totalweight == 0)//如果重量为0 ，则 默认快递费为0
                        {
                            feedetail += "0" + ",";
                        }
                        else
                        {
                            feedetail += (totalprice * (decimal.Parse(proweightarr[i]) / totalweight)).ToString("F2") + ",";
                        }

                    }
                    else
                    {
                        feedetail += "0" + ",";
                    }
                }
            }
            if (feedetail.Length > 0)
            {
                return feedetail.Substring(0, feedetail.Length - 1);
            }
            else
            {
                return "";
            }

        }
    }
}
