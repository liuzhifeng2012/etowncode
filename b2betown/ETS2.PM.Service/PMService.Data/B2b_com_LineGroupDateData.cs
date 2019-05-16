using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class B2b_com_LineGroupDateData
    {

        public List<B2b_com_LineGroupDate> GetLineGroupDateByLineid(int lineid, string isvalid = "0,1",int servertype=0)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2b_com_LineGroupDate(helper).GetLineGroupDateByLineid(lineid, isvalid,servertype);

                return list;
            }
        }

        public B2b_com_LineGroupDate GetLineDayGroupDate(DateTime daydate, int lineid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2b_com_LineGroupDate(helper).GetLineDayGroupDate(daydate, lineid);

                return list;
            }
        }

        public int EditLineGroupDate(B2b_com_LineGroupDate model)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2b_com_LineGroupDate(helper).EditLineGroupDate(model);

                return list;
            }
        }

        public int DelLineGroupDate(int lineid, string daydate)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2b_com_LineGroupDate(helper).DelLineGroupDate(lineid, daydate);

                return list;
            }
        }

        /**得到一段时间内房型的最低价格**/
        public string GetMinPrice(int fangxingid, string startdata = "", string enddata = "")
        {
            using (var helper = new SqlHelper())
            {

                string m = new InternalB2b_com_LineGroupDate(helper).GetMinPrice(fangxingid, startdata, enddata);

                return m;
            }
        }
        /**判断一段时间内房型是否可以预订**/
        public string IsCanBook(int fangxingid, string startdate = "", string enddate = "")
        {
            using (var helper = new SqlHelper())
            {

                string m = new InternalB2b_com_LineGroupDate(helper).IsCanBook(fangxingid,startdate, enddate);

                return m;
            }
        }

        /**得到房态信息**/
        public List<B2b_com_LineGroupDate> GetHouseTypeDayList(int proid, string startdate, string enddate)
        {
            using (var helper = new SqlHelper())
            {

                var  m = new InternalB2b_com_LineGroupDate(helper).GetHouseTypeDayList(proid, startdate, enddate);
               
                
                    return m;
               
            }
        }
        /**得到房态信息**/
        public decimal Gethotelallprice(int proid, DateTime start_date, DateTime enddate, int Agentlevel = 0)
        {
            using (var helper = new SqlHelper())
            {

                decimal m = new InternalB2b_com_LineGroupDate(helper).Gethotelallprice(proid, start_date, enddate, Agentlevel);

                return m;
               
            }
        }
        


        public string GetNowdayPrice(int fangxingid, string startdate)
        {
            using (var helper = new SqlHelper())
            {

                string m = new InternalB2b_com_LineGroupDate(helper).GetNowdayPrice(fangxingid, startdate);

                return m;
            }
        }

        public string  GetNowdayavailablenum(int fangxingid, string startdate)
        {
            using (var helper = new SqlHelper())
            {

                string m = new InternalB2b_com_LineGroupDate(helper).GetNowdayavailablenum(fangxingid, startdate);

                return m;
            }
        }

        public List<B2b_com_LineGroupDate> GetLineDayGroupDate(string checkindate, string checkoutdate, int proid)
        {
            using (var helper = new SqlHelper())
            {

                List<B2b_com_LineGroupDate> m = new InternalB2b_com_LineGroupDate(helper).GetLineDayGroupDate(checkindate,checkoutdate,proid);

                return m;
            }
        }

        public int ReduceEmptyNum(int proid, int booknum, DateTime start_date )
        {
            using (var helper = new SqlHelper())
            {

                int m = new InternalB2b_com_LineGroupDate(helper).ReduceEmptyNum(proid, booknum, start_date);

                return m;
            }
        }
        public int ReduceEmptyNum(int proid, int booknum, DateTime start_date, DateTime enddate)
        {
            using (var helper = new SqlHelper())
            {

                int m = new InternalB2b_com_LineGroupDate(helper).ReduceEmptyNum(proid, booknum, start_date, enddate);

                return m;
            }
        }
        /*得到产品当天的空位数量*/
        public int GetEmptyNum(int proid, DateTime daydate)
        {
            using (var helper = new SqlHelper())
            {

                int m = new InternalB2b_com_LineGroupDate(helper).GetEmptyNum(proid,daydate);

                return m;
            }
        }

        /*处理大巴时，清空控位，防止再提交*/
        public int CleanEmptyNum(int proid, DateTime daydate,int userid=0,int comid=0)
        {
            using (var helper = new SqlHelper())
            {

                int m = new InternalB2b_com_LineGroupDate(helper).CleanEmptyNum(proid, daydate,userid,comid);

                return m;
            }
        }

        /*得到产品一段时间内的最小空位数量*/
        public int GetMinEmptyNum(int proid, DateTime start_date, DateTime end_date)
        {
            using (var helper = new SqlHelper())
            {

                int m = new InternalB2b_com_LineGroupDate(helper).GetMinEmptyNum(proid, start_date, end_date);

                return m;
            }
        }
        /// <summary>
        /// 得到产品项目的最小团期
        /// </summary>
        /// <param name="proid"></param>
        /// <returns></returns>
        public B2b_com_LineGroupDate GetMinValidByProjectid(int projectid, int comid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2b_com_LineGroupDate(helper).GetMinValidByProjectid(projectid,comid);

                return list;
            }
        }

        /// <summary>
        /// 得到产品产品的最小团期
        /// </summary>
        /// <param name="proid"></param>
        /// <returns></returns>
        public B2b_com_LineGroupDate GetMinValidByProid(int proid, int comid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2b_com_LineGroupDate(helper).GetMinValidByProid(proid, comid);

                return list;
            }
        }

        public int UpGroupdateprice(int proid, decimal price)
        {
            using (var helper = new SqlHelper())
            {

                var r = new InternalB2b_com_LineGroupDate(helper).UpGroupdateprice(proid, price);

                return r;
            }
        }

        public int Rollbackemptynum(int proid, DateTime dateTime,int unum)
        {
            using (var helper = new SqlHelper())
            {

                var r = new InternalB2b_com_LineGroupDate(helper).Rollbackemptynum(proid, dateTime,unum);

                return r;
            }
        }

        public decimal GetMinValidePrice(int proid)
        {
            using (var helper = new SqlHelper())
            {

                var r = new InternalB2b_com_LineGroupDate(helper).GetMinValidePrice(proid  );

                return r;
            }
        }

        public List<B2b_com_LineGroupDate> GetLineGroupDate(int proid, DateTime startTime, DateTime endTime)
        {
            using (var helper = new SqlHelper())
            {

                List<B2b_com_LineGroupDate> r = new InternalB2b_com_LineGroupDate(helper).GetLineGroupDate(proid,startTime,endTime);

                return r;
            }
        }
    }
}
