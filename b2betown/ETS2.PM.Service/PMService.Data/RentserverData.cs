using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.PM.Service.PMService.Data
{
    public class RentserverData
    {
        #region 根据服务卡芯片id得到 卡面印刷编号
        public string GetPrintNoByChipid(string Chipid)
        {
            using (var helper = new SqlHelper())
            {
                string PrintNo = new InternalRentserver(helper).GetPrintNoByChipid(Chipid);
                //如果输入的印刷编号小于5位，用0补齐
                var prefillzero = "";
                if (PrintNo.Length < 5)
                {
                    for (int i = 0; i < 5 - PrintNo.Length; i++)
                    {
                        prefillzero += "0";
                    }
                }
                return prefillzero + PrintNo;
            }
        }
        #endregion
        #region  根据卡面印刷编号得到 卡芯片id
        public string GetChipidByPrintNo(string printno)
        {
            using (var helper = new SqlHelper())
            {
                string chipid = new InternalRentserver(helper).GetChipidByPrintNo(printno);
                return chipid;
            }
        }
        #endregion

        #region 增加或更改服务内容
        public int upRentserver(int id, int comid, string servername, int WR, int num, int posid, decimal saleprice, decimal serverDepositprice, string renttype, int mustselect, int servertype, int printticket, int Fserver)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalRentserver(helper).upRentserver(id, comid, servername, WR, num, posid, saleprice, serverDepositprice, renttype, mustselect, servertype, printticket, Fserver);

                return pro;
            }
        }
        #endregion

        #region 删除
        public int delRentserver(int id, int comid)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalRentserver(helper).delRentserver(id, comid);

                return pro;
            }
        }
        #endregion


        #region 查询
        public List<B2b_Rentserver> Rentserverpagelist(int pageindex, int pagesize, int comid, out int totalcount, int proid = 0, string pno = "")
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).Rentserverpagelist(pageindex, pagesize, comid, out totalcount, proid, pno);

                return list;
            }
        }
        #endregion


        #region 查询
        public B2b_Rentserver Rentserverbyid(int id, int comid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).Rentserverbyid(id, comid);

                return list;
            }
        }
        #endregion

        #region 查询是否包含子服务
        public B2b_Rentserver RentserverbyFid(int Fid, int comid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).RentserverbyFid(Fid, comid);

                return list;
            }
        }
        #endregion


        #region 通过未使用的服务查询服务名称
        public B2b_Rentserver Rentserverbyuserinfoid(int id, int comid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).Rentserverbyuserinfoid(id, comid);

                return list;
            }
        }
        #endregion


        #region 查询
        public B2b_Rentserver Rentserverby_user_Info_id(int id)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).Rentserverby_user_Info_id(id);

                return list;
            }
        }
        #endregion

        #region 跟去产品id查询
        public B2b_Rentserver Rentserverbyidandproid(int id, int proid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).Rentserverbyidandproid(id, proid);

                return list;
            }
        }
        #endregion




        #region 跟产品id查询所有绑定的服务
        public B2b_Rentserver Rentserverproid(int proid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).Rentserverproid(proid);

                return list;
            }
        }
        #endregion


        #region 通过pos查询服务
        public B2b_Rentserver Rentserverbyposid(int posid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).Rentserverbyposid(posid);

                return list;
            }
        }
        #endregion

        #region 通过pos查询服务
        public List<B2b_Rentserver> RentserverListbyposid(int posid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).RentserverListbyposid(posid);

                return list;
            }
        }
        #endregion

        #region 查询
        public int Rentserverbinding(int id, int proid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).Rentserverbinding(id, proid);

                return list;
            }
        }
        #endregion

        #region 插入产品与服务绑定
        public int inpro_rentserver(int proid, int sid, int comid)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalRentserver(helper).inpro_rentserver(proid, sid, comid);

                return pro;
            }
        }
        #endregion

        #region 删除产品绑定的服务
        public int deletepro_rentserver(int proid, int comid)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalRentserver(helper).deletepro_rentserver(proid, comid);

                return pro;
            }
        }
        #endregion

        #region 插入用户 终端验证服务表
        public int inb2b_Rentserver_User(B2b_Rentserver_User Rentserver_User)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalRentserver(helper).inb2b_Rentserver_User(Rentserver_User);

                return pro;
            }
        }
        #endregion
        #region 使用数量减一
        public int jianb2b_Rentserver_User(int id)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalRentserver(helper).jianb2b_Rentserver_User(id);

                return pro;
            }
        }
        #endregion
        

        #region 补卡 修改卡id
        public int upRentserver_User_kaid(string cardid, string cardchipid)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalRentserver(helper).upRentserver_User_kaid(cardid, cardchipid);

                return pro;
            }
        }
        #endregion

        #region 清除id
        public int clearRentserver_User_kaid(string cardid, string cardchipid)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalRentserver(helper).clearRentserver_User_kaid(cardid, cardchipid);

                return pro;
            }
        }
        #endregion

        #region 修改发送状态
        public int upRentserver_User_sendstate_str(int sendstate,string cardchipid,string reuntstr)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalRentserver(helper).upRentserver_User_sendstate_str(sendstate, cardchipid, reuntstr);

                return pro;
            }
        }
        #endregion

        #region 查询终端信息
        public B2b_Rentserver_User SearchRentserver_User(int id, string cardchipid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).SearchRentserver_User(id, cardchipid);

                return list;
            }
        }
        #endregion

        #region 通过电子码查询发出的卡
        public List<B2b_Rentserver_User> SearchRentserver_Userbypno(string pno, out int count)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).SearchRentserver_Userbypno(pno, out count);

                return list;
            }
        }
        #endregion

        #region 通过芯片卡号查询用户
        public B2b_Rentserver_User SearchRentserver_UserbyCardID(string CardID)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).SearchRentserver_UserbyCardID(CardID);

                return list;
            }
        }
        #endregion

        #region 通过电子码查询发出的卡
        public B2b_Rentserver_User SearchRentserver_bypno(string pno)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).SearchRentserver_bypno(pno);

                return list;
            }
        }
        #endregion



        #region 查询
        public List<B2b_Rentserver_User> Rentserver_Userpagelist(int pageindex, int pagesize, int oid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).Rentserver_Userpagelist(pageindex, pagesize, oid, out totalcount);

                return list;
            }
        }
        #endregion


        #region 插入用户 终端验证服务表
        public int inb2b_Rentserver_User_info(B2b_Rentserver_User_info Rentserver_User_info)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalRentserver(helper).inb2b_Rentserver_User_info(Rentserver_User_info);

                return pro;
            }
        }
        #endregion

        #region 查询终端信息
        public B2b_Rentserver_User_info SearchRentserver_User_info(int Userid, int Rentserverid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).SearchRentserver_User_info(Userid, Rentserverid);

                return list;
            }
        }
        #endregion


        #region 查询终超时
        public B2b_Rentserver_User_info SearchRentserver_User_outtime(int Userid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).SearchRentserver_User_outtime(Userid);

                return list;
            }
        }
        #endregion


        #region 查询终端状态
        public List<B2b_Rentserver_User_info> SearchRentserver_User_list_state(int Userid, string Rentserverlistid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).SearchRentserver_User_list_state(Userid, Rentserverlistid);

                return list;
            }
        }
        #endregion

        #region 查询终端信息
        public List<B2b_Rentserver> SearchRentserver_User_list(int Userid, string Rentserverlistid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).SearchRentserver_User_list(Userid, Rentserverlistid);

                return list;
            }
        }
        #endregion
        #region 查询终端信息
        public List<B2b_Rentserver_User_info> SearchRentserver_User_alllist_state(int Userid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).SearchRentserver_User_alllist_state(Userid);

                return list;
            }
        }
        #endregion



        #region 查询终端信息list
        public List<B2b_Rentserver> SearchRentserverList_User_info(int Userid, string rentserverlist)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).SearchRentserverList_User_info(Userid, rentserverlist);

                return list;
            }
        }
        #endregion



        #region 查询
        public List<B2b_Rentserver_User_info> Rentserver_User_infopagelist(int pageindex, int pagesize, int Userid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).Rentserver_User_infopagelist(pageindex, pagesize, Userid, out totalcount);

                return list;
            }
        }
        #endregion



        #region 冲正，删除发出去的卡及服务
        public int Reverse_Rentserver_User(string pno)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalRentserver(helper).Reverse_Rentserver_User(pno);

                return pro;
            }
        }
        #endregion


        /// <summary>
        /// 根据服务id得到服务信息
        /// </summary>
        /// <param name="rentserverid"></param>
        /// <returns></returns>
        public B2b_Rentserver GetRentServerById(int rentserverid)
        {
            using (var helper = new SqlHelper())
            {

                B2b_Rentserver r = new InternalRentserver(helper).GetRentServerById(rentserverid);

                return r;
            }
        }


        //-------------------------------结束时间，索道票管理-------------------------------------------




        #region 增加或更改服务内容
        public int uppro_worktime(int id, int comid, string title, string defaultendtime, string defaultstartime)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalRentserver(helper).uppro_worktime(id, comid, title, defaultendtime, defaultstartime);

                return pro;
            }
        }
        #endregion


        #region 超时插入
        public int insertTimeoutmoney(b2b_Rentserver_user_Timeoutmoney timeout)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalRentserver(helper).insertTimeoutmoney(timeout);

                return pro;
            }
        }
        #endregion

        #region 删除
        public int delpro_worktime(int id, int comid)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalRentserver(helper).delpro_worktime(id, comid);

                return pro;
            }
        }
        #endregion


        #region 查询
        public List<b2b_com_pro_worktime> pro_worktimepagelist(int pageindex, int pagesize, int comid, out int totalcount, int proid = 0)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).pro_worktimepagelist(pageindex, pagesize, comid, out totalcount, proid);

                return list;
            }
        }
        #endregion


        #region 查询
        public b2b_com_pro_worktime pro_worktimebyid(int id, int comid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).pro_worktimebyid(id, comid);

                return list;
            }
        }
        #endregion


        #region 比较开始时间
        public string worktimestarbijiao(b2b_com_pro_worktime worktime)
        {
            DateTime dt1 = DateTime.Now;//现在时间
            string startime = dt1.ToString("HH:mm");
            try
            {
                if (worktime != null)
                {
                    if (worktime.defaultstartime != "")
                    {
                        var s1_temp = DateTime.Now.ToString("yyyy-MM-dd") + " " + worktime.defaultstartime + ":00";
                        DateTime Dt = DateTime.Parse(s1_temp);

                        //先比较 现在时间是否大于设定的开始时间，如果小于 代表未开始之前验证，输出设定开始时间
                        if (DateTime.Compare(dt1, Dt) > 0)
                        {
                            return startime;
                        }
                        else
                        {
                            return Dt.ToString("HH:mm");
                        }
                    }
                }
                return startime;

            }
            catch
            {
                //出现异常直接输出 传入值
                return startime;
            }

        }
        #endregion

        #region 结束时间比较
        public string worktimeendbijiao(int hour, b2b_com_pro_worktime worktime)
        {
            string endtime = "17:00";//先对结束时间赋值一个默认值
            try
            {

                if (worktime != null)
                {
                    if (worktime.defaultendtime != "")
                    {
                        var s1_temp = DateTime.Now.ToString("yyyy-MM-dd") + " " + worktime.defaultendtime + ":00";
                        DateTime Dt = DateTime.Parse(s1_temp);//传入的结束时间

                        var star_temp = DateTime.Now.ToString("yyyy-MM-dd") + " " + worktime.defaultstartime + ":00";
                        DateTime Dstar = DateTime.Parse(star_temp);//传入的开始时间

                        DateTime dt1 = Dstar.AddHours(hour);//现在时间 加上使用的时间

                        //先比较 现在时间加上使时间用是否大于传入的结束时间，如果大于 代表未开始之前验证，输出设定开始时间
                        if (DateTime.Compare(dt1, Dt) > 0)
                        {
                            return Dt.ToString("HH:mm");
                        }
                        else
                        {

                            return dt1.ToString("HH:mm"); ;
                        }
                    }
                }
                return endtime;

            }
            catch
            {
                //出现异常直接输出 传入值
                return endtime;
            }

        }
        #endregion

        #region 比较开始时间
        public string worktimestar_endbijiao(string startime, string endtime)
        {
            DateTime dt1 = DateTime.Now;//现在时间

            try
            {
                var s1_temp = DateTime.Now.ToString("yyyy-MM-dd") + " " + startime + ":00";
                DateTime Dt = DateTime.Parse(s1_temp);

                var s2_temp = DateTime.Now.ToString("yyyy-MM-dd") + " " + endtime + ":00";
                DateTime Dt2 = DateTime.Parse(s2_temp);

                //先比较 现在时间是否大于设定的开始时间，如果小于 代表未开始之前验证，输出设定开始时间
                if (DateTime.Compare(Dt, Dt2) > 0)
                {
                    return Dt2.ToString("HH:mm"); ;
                }
                else
                {
                    return Dt.ToString("HH:mm");
                }

            }
            catch
            {
                //出现异常直接输出 传入值
                return startime;
            }

        }
        #endregion

      
        /// <summary>
        /// 根据产品工作时间类型id获得特定日期
        /// </summary>
        /// <param name="proworktimetypeid">产品工作时间类型</param>
        /// <param name="datatype">日期类型(0全部；1有效日期及今天以后的日期包括今天；2.失效日期)</param>
        /// <returns></returns>
        public List<b2b_com_pro_worktime_calendar> GetblackoutdatebyProWorktimeId(int proworktimeid, string datatype = "0")
        {
            using (var helper = new SqlHelper())
            {
                List<b2b_com_pro_worktime_calendar> list = new InternalRentserver(helper).GetblackoutdatebyProWorktimeId(proworktimeid, datatype);

                return list;
            }
        }

        public b2b_com_pro_worktime_calendar GetblackoutdateByWorktimeId(string daydate, int proworktimeid)
        {
            using (var helper = new SqlHelper())
            {
                b2b_com_pro_worktime_calendar r = new InternalRentserver(helper).GetblackoutdateByWorktimeId(daydate, proworktimeid);
                return r;
            }
        }

        public int DelblackoutdateByWorktimeId(string daydate, int proworktimeid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalRentserver(helper).DelblackoutdateByWorktimeId(daydate, proworktimeid);
                return r;
            }
        }

        public int Insworktimeblackoutdates(b2b_com_pro_worktime_calendar model)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalRentserver(helper).Insworktimeblackoutdates(model);
                return r;
            }
        }

        public int Delworktimeusesetbyworktimeid(int proworktimeid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalRentserver(helper).Delworktimeusesetbyworktimeid(proworktimeid);
                return r;
            }
        } 
        public List<b2b_Rentserver_user_Timeoutmoney> GetserverTimeoutPagelist(int comid, int pageindex, int pagesize, string startime, string endtime, string key, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                List<b2b_Rentserver_user_Timeoutmoney> list = new InternalRentserver(helper).GetserverTimeoutPagelist(comid, pageindex, pagesize, startime, endtime, key,out  totalcount);
                return list;
            }
        }

        public List<B2b_com_pro> serverSuodaoPagelist(int comid, string startime, string endtime, string key)
        {
            using (var helper = new SqlHelper())
            {
               var list = new InternalRentserver(helper).serverSuodaoPagelist(comid, startime, endtime, key);
                return list;
            }
        }

        public decimal GetServerTimeoutMoney(int comid, string startime, string endtime, string key)
        {
            using (var helper = new SqlHelper())
            {
                decimal list = new InternalRentserver(helper).GetServerTimeoutMoney(comid, startime, endtime, key);
                return list;
            }
        }
   
        public List<B2b_Rentserver_User> GetserverfakaPagelist(int comid, int pageindex, int pagesize, string startime, string endtime, string key, int serverstate, int serverid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_Rentserver_User> list = new InternalRentserver(helper).GetserverfakaPagelist(comid, pageindex, pagesize, startime, endtime, key, serverstate, serverid, out   totalcount);
                return list;
            }
        }

        public int SearchRentserver_count_state(int sid, string startime, string endtime)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalRentserver(helper).SearchRentserver_count_state(sid,startime,endtime);
                return list;
            }
        }
        public int SearchRentserver_weiguihuancount_state(int sid, string startime, string endtime)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalRentserver(helper).SearchRentserver_weiguihuancount_state(sid, startime, endtime);
                return list;
            }
        }


        public int GetServerUsageCount(int comid, string startime, string endtime, string key, int serverstate, int serverid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalRentserver(helper).GetServerUsageCount(comid, startime, endtime, key, serverstate, serverid);
                return r;
            }
        }

        #region 查询
        public List<B2b_pro_cost_rili> procostrilipagelist(int pageindex, int pagesize, int comid, int proid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).procostrilipagelist(pageindex, pagesize, comid, proid, out totalcount);

                return list;
            }
        }
        #endregion

        #region 查询
        public B2b_pro_cost_rili procostrilibyid( int comid, int id)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).procostrilibyid(comid, id);

                return list;
            }
        }
        #endregion

        #region 修改
        public int upprocostrili(B2b_pro_cost_rili costrili)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).upprocostrili(costrili);

                return list;
            }
        }
        #endregion

        #region 查询最后一天日历的日期
        public string prolastcostrilibyid(int comid,int proid,int id)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).prolastcostrilibyid(comid,proid,id);

                return list;
            }
        }
        #endregion
        #region 查询最后一天日历的日期
        public string produibicostrili(int comid, int proid,string stardate,string enddate)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).produibicostrili(comid, proid, stardate, enddate);

                return list;
            }
        }
        #endregion
        
        #region 修改
        public int delcostrili(int comid,int id)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).delcostrili(comid,id);

                return list;
            }
        }
        #endregion




        #region 查询
        public List<B2b_project_finance> projectfinancepagelist(int pageindex, int pagesize, int comid, int projectid, string startime, string endtime, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).projectfinancepagelist(pageindex, pagesize, comid, projectid, startime, endtime, out totalcount);

                return list;
            }
        }
        #endregion

        #region 查询
        public decimal projectfinancesum(int comid, int projectid, string startime, string endtime)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).projectfinancesum(comid, projectid, startime, endtime);

                return list;
            }
        }
        #endregion

        #region 修改
        public int upprojectfinance(B2b_project_finance project_finance)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalRentserver(helper).upprojectfinance(project_finance);

                return list;
            }
        }
        #endregion

    }
}
