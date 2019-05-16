using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Data.InternalData;

namespace ETS2.CRM.Service.CRMService.Data
{
    public class B2bCompanyManagerUserData
    {
        #region 添加或者编辑员工信息

        /// <summary>
        /// 添加或者编辑 By:Xiaoliu
        /// </summary>
        /// <param name="model">商家员工 实体</param>
        /// <returns>标识列</returns>
        public static int InsertOrUpdate(B2b_company_manageuser model)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyManageUser(sql);
                    int result = internalData.InsertOrUpdate(model);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion
        #region 根据用户id获取用户信息

        public static B2b_company_manageuser GetUser(int userId)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyManageUser(sql);
                    var user = internalData.GetUser(userId);
                    return user;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 判断手机是否存在
        public bool Ishasphone(string mobile, int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyManageUser(sql);
                    var user = internalData.Ishasphone(mobile, comid);
                    return user;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion



        #region 验证用户是否存在

        /// <summary>
        /// 验证用户是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static B2b_company_manageuser VerifyUser(string username, string password, out string msg)
        {
            var message = "";
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyManageUser(sql);
                    var user = internalData.VerfyUser(username, password, out message);
                    msg = message;
                    return user;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 根据员工账户获得ID
        public int FromAccountGetId(string account)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyManageUser(sql);
                    var result = internalData.FromAccountGetId(account);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion



        #region 获取商户第一个账户

        /// <summary>
        /// 获取商户第一个账户
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string GetFirstAccountUser(int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyManageUser(sql);
                    var user = internalData.GetFirstAccountUser(comid);
                    return user;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion
        #region 获取商户第一个账户ID

        /// <summary>
        /// 获取商户第一个账户ID
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int GetFirstIDUser(int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyManageUser(sql);
                    var user = internalData.GetFirstIDUser(comid);
                    return user;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 修改用户密码
        public static int ChangePwd(int userid, string oldpwd, string pwd1, out string message)
        {
            message = "";
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyManageUser(sql);
                    //判断老密码是否输入正确
                    int num1 = internalData.CheckOldPwd(userid, oldpwd);
                    if (num1 == 0)
                    {
                        message = "原密码输入错误";
                        return 0;
                    }
                    //判断密码修改是否成功
                    int result = internalData.ChangePwd(userid, pwd1);
                    return result;

                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    return 0;

                }
            }
        }
        #endregion

        #region 修改默认客服账户
        public static int ChangeIsDefaultKf(int userid, int IsDefaultKf)
        {
         
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyManageUser(sql);
                    //判断密码修改是否成功
                    int result = internalData.ChangeIsDefaultKf(userid, IsDefaultKf);
                    return result;

                }
                catch (Exception ex)
                {
                    return 0;

                }
            }
        }
        #endregion

        #region 获得特定公司员工列表
        public List<B2b_company_manageuser> Manageuserpagelist(string comid, int pageindex, int pagesize, out int totalcount, int userid = 0)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bCompanyManageUser(helper).Manageuserpagelist(comid, pageindex, pagesize, out totalcount, userid);

                return list;
            }
        }
        #endregion
        public List<B2b_company_manageuser> Manageuserpagelist(string employstate, string comid, int pageindex, int pagesize, out int totalcount, int userid = 0, string key = "")
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bCompanyManageUser(helper).Manageuserpagelist(employstate, comid, pageindex, pagesize, out totalcount, userid, key);

                return list;
            }
        }

        public List<B2b_company_manageuser> ViewChanneluserpagelist(int comid, int channelcompanyid, int pageindex, int pagesize, out int totalcount, string key = "", string openid = "", string usern = "", string usere = "", int isheadofficekf = 0, int isonlycoachlist=0,string isviewjiaolian="0,1")
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bCompanyManageUser(helper).ViewChanneluserpagelist(comid, channelcompanyid, pageindex, pagesize, out totalcount, key, openid, usern, usere, isheadofficekf, isonlycoachlist,isviewjiaolian);

                return list;
            }
        }

        public List<B2b_company_manageuser> ViewQQpagelist(int comid, int channelcompanyid, int pageindex, int pagesize, out int totalcount, string key = "", string openid = "", string usern = "", string usere = "")
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bCompanyManageUser(helper).ViewQQpagelist(comid, channelcompanyid, pageindex, pagesize, out totalcount, key, openid, usern, usere);

                return list;
            }
        }

        public List<B2b_company_manageuser> Masterpagelist(int pageindex, int pagesize, string groupid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bCompanyManageUser(helper).Manageuserpagelist(pageindex, pagesize, groupid, out totalcount);

                return list;
            }
        }
        public List<B2b_company_manageuser> Masterpagelist(int pageindex, int pagesize, string groupid, int childcomid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bCompanyManageUser(helper).Manageuserpagelist(pageindex, pagesize, groupid, childcomid, out totalcount);

                return list;
            }
        }

        public List<B2b_company_manageuser> Masterpagelist(string employstate, int pageindex, int pagesize, string groupid, int childcomid, out int totalcount,string key="")
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bCompanyManageUser(helper).Manageuserpagelist(employstate, pageindex, pagesize, groupid, childcomid, out totalcount,key);

                return list;
            }
        }


        public static B2b_company_manageuser GetManageUserByAccount(string account)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyManageUser(sql);
                    var user = internalData.GetManageUserByAccount(account);
                    return user;
                }
                catch
                {
                    throw;
                }
            }
        }

        public static B2b_company GetB2bCompanyByCompanyName(string companyname)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyManageUser(sql);
                    var user = internalData.GetB2bCompanyByCompanyName(companyname);
                    return user;
                }
                catch
                {
                    //throw;
                    return null;
                }
            }
        }
        #region 得到子商户的分销网站经理的账户信息
        public B2b_company_manageuser Getchildcompanyuser(int childcomid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyManageUser(sql);
                    var user = internalData.Getchildcompanyuser(childcomid);
                    return user;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        public int Adjustemploerstate(int masterid, int employerstate)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyManageUser(sql);
                    int result = internalData.Adjustemploerstate(masterid, employerstate);
                    return result;
                }
                catch
                {
                    throw;
                }
            }

        }
        /// <summary>
        /// 调整渠道公司下员工状态
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="employstate"></param>
        /// <returns></returns>
        public int AdjustChannelCompanyEmploerstate(int companyid, int employstate)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyManageUser(sql);
                    int result = internalData.AdjustChannelCompanyEmploerstate(companyid, employstate);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public bool IsParentCompanyUser(int userid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyManageUser(sql);
                    bool result = internalData.IsParentCompanyUser(userid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public bool Ishasaccount(string account)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyManageUser(sql);
                    bool result = internalData.Ishasaccount(account);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public B2b_company_manageuser GetMenshiManagerByMenshiId(int menshiid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {

                    B2b_company_manageuser result = new InternalB2bCompanyManageUser(sql).GetMenshiManagerByMenshiId(menshiid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public B2b_company_manageuser GetCompanyUser(int userid)
        {
            using (var sql = new SqlHelper())
            {

                B2b_company_manageuser result = new InternalB2bCompanyManageUser(sql).GetCompanyUser(userid);
                return result;

            }
        }
        public string GetCompanynamebyUserid(int userid)
        {
            using (var sql = new SqlHelper())
            {

                var result = new InternalB2bCompanyManageUser(sql).GetCompanynamebyUserid(userid);
                return result;

            }
        }


        public B2b_company_manageuser GetCompanyUserByPhone(string phone,int comid)
        {
            using (var sql = new SqlHelper())
            {

                B2b_company_manageuser result = new InternalB2bCompanyManageUser(sql).GetCompanyUserByPhone(phone,comid);
                return result;

            }
        }

        internal B2b_company_manageuser GetGuwenByVipweixin(string openid, int comid)
        {
            using (var sql = new SqlHelper())
            {

                B2b_company_manageuser result = new InternalB2bCompanyManageUser(sql).GetGuwenByVipweixin(openid,comid);
                return result;

            }
        }


        public string  GetCompanyUserName(int userid)
        {
            using(var helper=new SqlHelper())
            {
                string r = new InternalB2bCompanyManageUser(helper).GetCompanyUserName(userid);
                return r;
            }
        }
        /// <summary>
        /// 得到开户时的账户
        /// </summary>
        /// <param name="comid"></param>
        /// <returns></returns>
        public B2b_company_manageuser GetOpenAccount(int comid)
        {
            using (var helper = new SqlHelper())
            {
                B2b_company_manageuser r = new InternalB2bCompanyManageUser(helper).GetOpenAccount(comid);
                return r;
            }
        }

        public B2b_company_manageuser GetCompanyUserByOpenid(string openid)
        {
            using (var helper = new SqlHelper())
            {
                B2b_company_manageuser r = new InternalB2bCompanyManageUser(helper).GetCompanyUserByOpenid(openid);
                return r;
            }
        }

        public List<B2b_company_manageuser_useworktime> Worktimepagelist(int comid, int MasterId,DateTime date,string hourstr, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bCompanyManageUser(helper).Worktimepagelist(comid, MasterId, date,hourstr, out totalcount);

                return list;
            }
        }


        #region 添加已使用 工时间
        /// <summary>
        /// 添加已使用 工时间
        /// </summary>
        /// <param name="model">商家员工已用工时 实体</param>
        /// <returns>标识列</returns>
        public static int UseworktimeInsertOrUpdate(B2b_company_manageuser_useworktime model)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyManageUser(sql);
                    int result = internalData.UseworktimeInsertOrUpdate(model);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 删除已使用 工时间
        /// <summary>
        /// 删除已使用 工时间
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>标识列</returns>
        public static int UseworktimeDel(int id)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyManageUser(sql);
                    int result = internalData.UseworktimeDel(id);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion
        #region 删除已使用 工时间
        /// <summary>
        /// 删除已使用 工时间
        /// </summary>
        /// <param name="oid">oid</param>
        /// <returns>标识列</returns>
        public static int UseworktimeDelOid(int oid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyManageUser(sql);
                    int result = internalData.UseworktimeDelOid(oid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        public int UpChannelcompanyid(string masterid,int channelcompanyid)
        {
            using (var helper = new SqlHelper())
            {

                int r = new InternalB2bCompanyManageUser(helper).UpChannelcompanyid(masterid,channelcompanyid);

                return r;
            }
        }
    }
}
