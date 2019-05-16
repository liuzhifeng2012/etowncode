using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Meituan.Model;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.PM.Service.Meituan.Data.Internal
{
    public class InternalPoiResponse
    {
        public SqlHelper sqlHelper;
        public InternalPoiResponse(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal List<PoiResponseBody> GetPoiResponseBody(out int totalcount, string method, List<string> poiIdList, Agent_company agentinfo, int pageindex, int pagesize)
        {
            #region 多点拉取
            if (method.Trim() == "multi")
            {
                string poiidstr = "";
                foreach (string proid in poiIdList)
                {
                    poiidstr = proid + ",";
                }
                poiidstr = poiidstr.Substring(0, poiidstr.Length - 1);

                string sql = @"SELECT *
                          FROM  [b2b_com_project] where  id in (" + poiidstr + ") and  address!='' and coordinate!='' and   onlinestate=1 ";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                List<PoiResponseBody> list = new List<PoiResponseBody>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new PoiResponseBody
                        {
                            partnerId = int.Parse(agentinfo.mt_partnerId),
                            partnerPoiId = reader.GetValue<int>("id").ToString(),
                            name = reader.GetValue<string>("projectname"),
                            city = reader.GetValue<string>("city"),
                            address = reader.GetValue<string>("address"),
                            phone = reader.GetValue<string>("mobile"), 
                            locType = 2,//景点坐标系类型	0 火星坐标 1 地球坐标 2 百度坐标 3 图吧坐标 4 搜狗坐标 5 其他
                            longitude = GetMtlongitude(reader.GetValue<string>("coordinate")), //景区地理经度	必须真实有效,值为真实值的6次方取整
                            latitude = GetMtlatitude(reader.GetValue<string>("coordinate"))//景区地理纬度	必须真实有效,值为真实值的6次方取整 
                        });
                    }
                }
                totalcount = list.Count;
                return list;
            }
            #endregion
            #region  批量拉取 page
            else if (method.Trim() == "page")
            {
                var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

                var condition = " id in (select id from b2b_com_project where address!='' and coordinate!='' and   onlinestate=1 and comid in (select comid  from agent_warrant where  agentid="+agentinfo.Id+"  and warrant_state=1))";

                cmd.PagingCommand1("b2b_com_project", "*", "id", "", pagesize, pageindex, "", condition);

                List<PoiResponseBody> list = new List<PoiResponseBody>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new PoiResponseBody
                        {
                            partnerId = int.Parse(agentinfo.mt_partnerId),
                            partnerPoiId = reader.GetValue<int>("id").ToString(),
                            name = reader.GetValue<string>("projectname"),
                            city = reader.GetValue<string>("city"),
                            address = reader.GetValue<string>("address"),
                            phone = reader.GetValue<string>("mobile"),
                            locType = 2,//景点坐标系类型	0 火星坐标 1 地球坐标 2 百度坐标 3 图吧坐标 4 搜狗坐标 5 其他
                            longitude = GetMtlongitude(reader.GetValue<string>("coordinate")), //景区地理经度	必须真实有效,值为真实值的6次方取整
                            latitude = GetMtlatitude(reader.GetValue<string>("coordinate"))//景区地理纬度	必须真实有效,值为真实值的6次方取整 
                        });
                    }

                }
                totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
                return list;
            }
            #endregion
            else
            {
                totalcount = 0;
                return new List<PoiResponseBody>();
            }
        }

        private int GetMtlatitude(string coordinate)
        {
            try
            {
                if (coordinate != "")
                {
                    if (coordinate.IndexOf(',') > -1)
                    {
                        string r = coordinate.Substring(coordinate.IndexOf(',') + 1);
                        double r1 = Math.Ceiling(double.Parse(r)) * 10 * 10 * 10 * 10 * 10 * 10;
                        string r2 = r1.ToString("f0");
                        return int.Parse(r2);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        private int GetMtlongitude(string coordinate)
        {
            try
            {
                if (coordinate != "")
                {
                    if (coordinate.IndexOf(',') > -1)
                    {
                        string r = coordinate.Substring(0, coordinate.IndexOf(','));
                        double r1 = Math.Ceiling(double.Parse(r)) * 10 * 10 * 10 * 10 * 10 * 10;
                        string r2 = r1.ToString("f0");
                        return int.Parse(r2);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }

        }
    }
}
