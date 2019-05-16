using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data.InternalData;
using Newtonsoft.Json;
using System.Data;


namespace ETS2.CRM.Service.CRMService.Data
{
    public class AgentCompanyData
    {

        #region 判断分销商电子邮箱是否使用过
        public static int GetEmail(string email)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    int result = internalData.GetEmail(email);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 通过域名查询是否为分销商
        public static int DomainGetAgentid(string domain)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    int result = internalData.DomainGetAgentid(domain);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 判断分销商电子手机是否使用过
        public static int GetPhone(string phone)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    int result = internalData.GetPhone(phone);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 查询分销商手机
        public static int Agentsearch(string phone)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    int result = internalData.Agentsearch(phone);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 分销商注册账户
        public static int RegiAccount(Agent_company regiinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    int result = internalData.RegiAccount(regiinfo);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 分销商修改商家信息
        public static int RegiUpCompany(Agent_company regiinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    int result = internalData.RegiUpCompany(regiinfo);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 分销商修改商家信息
        public static int AdminRegiUpCompany(Agent_company regiinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    int result = internalData.AdminRegiUpCompany(regiinfo);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 分销商修改登陆信息
        public static int RegiUpAccount(Agent_regiinfo regiinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    int result = internalData.RegiUpAccount(regiinfo);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 管理员修改分销商修改登陆信息
        public static int ManageUpAccount(Agent_regiinfo regiinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    int result = internalData.ManageUpAccount(regiinfo);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 分销商登陆信息查询
        public static Agent_regiinfo AgentSearchAccount(string account)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.AgentSearchAccount(account);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 查询分销商开户账户
        public static string GetAgentAccount(int agentid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.GetAgentAccount(agentid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 查询分销商返点录入金额
        public static decimal GetAgentRebatemoney(int agentid, int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.GetAgentRebatemoney(agentid, comid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 授权列表
        public List<Agent_warrant> Warrantpagelist(int Pageindex, int Pagesize, string Key, int Agentid, out int totalcount, string comstate = "1,2", string offcomids = "")
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.Warrantpagelist(Pageindex, Pagesize, Key, Agentid, out totalcount, comstate, offcomids);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 授权列表
        public List<B2b_company> UnWarrantpagelist(int Pageindex, int Pagesize, string Key, int Agentid, out int totalcount, string offcomids="")
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.UnWarrantpagelist(Pageindex, Pagesize, Key, Agentid, out totalcount,offcomids);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 微站分销列表，分销商看到的
        public List<B2b_company> AgentComlist(int Pageindex, int Pagesize, string Key, int Agentid, out int totalcount)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.AgentComlist(Pageindex, Pagesize, Key, Agentid, out totalcount);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion



        #region 分销账户列表
        public List<Agent_regiinfo> Accountlist(int Pageindex, int Pagesize, int Agentid, out int totalcount,int accountid=0)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.Accountlist(Pageindex, Pagesize, Agentid, out totalcount,accountid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 分销商列表
        public List<Agent_warrant> Agentpagelist(int Pageindex, int Pagesize, string Key, int comid, out int totalcount, string com_province = "", string com_city = "", string warrantstate = "0,1", int agentsourcesort = 0, string orderby = "")
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.Agentpagelist(Pageindex, Pagesize, Key, comid, out totalcount, com_province, com_city, warrantstate, agentsourcesort,orderby);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion




        #region 特别授权
        public int SetWarrant(int agentid, int type, int proid, int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.SetWarrant(agentid, type, proid, comid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 查询特别授权信息
        public int SearchSetWarrant(int agentid, int proid, int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.SearchSetWarrant(agentid, proid, comid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 所有分销商列表
        public List<Agent_company> ManageAgentpagelist(int Pageindex, int Pagesize, string Key, out int totalcount, string com_province, string com_city, int agentsourcesort = 0)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.ManageAgentpagelist(Pageindex, Pagesize, Key, out totalcount, com_province, com_city, agentsourcesort);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion




        #region 查询未授权分销商列表
        public List<Agent_regiinfo> Unagentlist(int Pageindex, int Pagesize, string Key, int comid, out int totalcount)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.Unagentlist(Pageindex, Pagesize, Key, comid, out totalcount);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 查询未授权分销商列表
        public List<Agent_Financial> AgentFinacelist(int Pageindex, int Pagesize, int agentid, int comid, out int totalcount, int recharge=0)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.AgentFinacelist(Pageindex, Pagesize, agentid, comid, out totalcount, recharge);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 分销商登陆
        public static string Login(string email, string pwd, out Agent_company userinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.Login(email, pwd, out userinfo);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 商户绑定分销商
        public static int BindingAgent(int comid, int agentid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.BindingAgent(comid, agentid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 解除商户绑定分销商
        public static int UnBindingAgent(int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.UnBindingAgent(comid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 分销商
        public static int ModifyAgentInfo(Agent_company regiinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    int result = internalData.ModifyAgentInfo(regiinfo);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 分销商授权
        public static int AddAgentInfo(Agent_company regiinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    int result = internalData.AddAgentInfo(regiinfo);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 项目授权
        public static int AddAgentProject(Agent_company regiinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    int result = internalData.AddAgentProject(regiinfo);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion



        #region 分销商授权
        public static int SearchAgentwarrant(Agent_company regiinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    int result = internalData.SearchAgentwarrant(regiinfo);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 根据分销id得到分销商基本信息
        public static Agent_company GetAgentByid(int agentid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    Agent_company result = internalData.GetAgentByid(agentid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 根据授权id得到分销商基本信息
        public static Agent_company GetAgentCompanyByUid(int warrantid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    Agent_company result = internalData.GetAgentCompanyByUid(warrantid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 根据授权id得到分销商登陆账户
        public static Agent_regiinfo GetAgentAccountByUid(string account, int agentid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    Agent_regiinfo result = internalData.GetAgentAccountByUid(account, agentid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 根据id得到分销信息
        public static Agent_regiinfo GetManageAgentAccountByUid(int id)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    Agent_regiinfo result = internalData.GetManageAgentAccountByUid(id);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 根据用户id,comid得到分销授权于预付款
        public static Agent_company GetAgentWarrant(int agentid, int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    Agent_company result = internalData.GetAgentWarrant(agentid, comid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 根据用户id,comid得到分销授权于预付款
        public static int GetAgentWarrantAgentlevel(int agentid, int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.GetAgentWarrantAgentlevel(agentid, comid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 根据产品id获取原绑定原产品的授权类型
        public static int GetAgentWarranttypebyproid(int proid, int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    int result = internalData.GetAgentWarranttypebyproid(proid, comid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 后台充入预付款
        public static decimal Hand_Imprest(int agentid, int comid, string startime = "", string endtime = "")
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.Hand_Imprest(agentid, comid, startime, endtime);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 分销后台扣款预付款
        public static decimal HandOut_Imprest(int agentid, int comid, string startime = "", string endtime = "")
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.HandOut_Imprest(agentid, comid, startime, endtime);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion
        #region 后台充入返点
        public static decimal Hand_Rebate(int agentid, int comid, string startime = "", string endtime = "")
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.Hand_Rebate(agentid, comid, startime, endtime);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion



        #region 网上支付预付款
        public static decimal Line_Imprest(int agentid, int comid, string startime = "", string endtime = "")
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.Line_Imprest(agentid, comid, startime, endtime);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 销售额
        public static decimal Sale_price(int agentid, int comid, int projectid = 0, string startime = "", string endtime = "")
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.Sale_price(agentid, comid, projectid, startime, endtime);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 出票情况
        public static int Outticketnum(int agentid, int comid, int projectid = 0, string startime = "", string endtime = "")
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.Outticketnum(agentid, comid, projectid, startime, endtime);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 验票情况
        public static int Yanzhengticketnum(int agentid, int comid, int projectid = 0, string startime = "", string endtime = "")
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.Yanzhengticketnum(agentid, comid, projectid, startime, endtime);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 毛利
        public static decimal Maoli_price(int agentid, int comid, int projectid = 0, string startime = "", string endtime = "")
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.Maoli_price(agentid, comid, projectid, startime, endtime);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 倒码销售额
        public static decimal Daoma_Sale_price(int agentid, int comid, int projectid = 0, string startime = "", string endtime = "")
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.Daoma_Sale_price(agentid, comid, projectid, startime, endtime);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 倒码毛利
        public static decimal Daoma_Maoli_price(int agentid, int comid, int projectid = 0, string startime = "", string endtime = "")
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.Daoma_Maoli_price(agentid, comid, projectid, startime, endtime);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 已消费
        public static decimal Xiaofei_price(int agentid, int comid, int projectid = 0, string startime = "", string endtime = "")
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.Xiaofei_price(agentid, comid, projectid, startime, endtime);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 分销导入产品已消费
        public static decimal AgentDaoru_Xiaofei_price(int agentid, int comid, int projectid = 0, string startime = "", string endtime = "")
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.AgentDaoru_Xiaofei_price(agentid, comid, projectid, startime, endtime);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 倒码已消费
        public static decimal Daoma_Xiaofei_price(int agentid, int comid, int projectid = 0)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.Daoma_Xiaofei_price(agentid, comid, projectid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 沉淀
        public static decimal Chendian_price(int agentid, int comid, int projectid = 0, string startime = "", string endtime = "")
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.Chendian_price(agentid, comid, projectid, startime, endtime);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 分销导入产品沉淀
        public static decimal AgentDaoru_Chendian_price(int agentid, int comid, int projectid = 0, string startime = "", string endtime = "")
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.AgentDaoru_Chendian_price(agentid, comid, projectid, startime, endtime);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 写出分销财务记录
        public static int WriteAgentMoney(string acttype, decimal money, int agentid, int comid, int warrantid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    int result = internalData.WriteAgentMoney(acttype, money, agentid, comid, warrantid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 子分销扣减金额
        public static int WriteAgentSunMoney(decimal money, int agentsunid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    int result = internalData.WriteAgentSunMoney(money, agentsunid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion



        #region 分销财务扣款
        public static int AgentFinancial(Agent_Financial Financialinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    int result = internalData.AgentFinancial(Financialinfo);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 分销财务编辑
        public static int EditAgentFinancial(Agent_Financial Financialinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    int result = internalData.EditAgentFinancial(Financialinfo);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        //修改分销商预付款
        public static int UpdateImprest(int wid, decimal money)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    int result = internalData.UpdateImprest(wid, money);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        //修改信用额度
        public static int UpdateCredit(int wid, decimal money)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    int result = internalData.UpdateCredit(wid, money);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }


        //修改分销账户类型
        public static int ChangeAgentsort(int agentid, int Agentsort)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    int result = internalData.ChangeAgentsort(agentid, Agentsort);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }


        public Agent_company GetAgentCompany(int id)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    Agent_company result = internalData.GetAgentCompany(id);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public string GetAgentBind_ip(int Agentid)
        {
            string r = new InternalAgentCompany(new SqlHelper()).GetAgentBindIp(Agentid);
            return r;
        }

        /// <summary>
        /// 获得分销商 授权信息
        /// </summary>
        /// <param name="agentid"></param>
        /// <param name="comid"></param>
        /// <returns></returns>
        public Agent_warrant GetAgent_Warrant(int agentid, int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    Agent_warrant result = internalData.GetAgent_Warrant(agentid, comid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public static string GetAgentComProvince(int agentid)
        {
            string r = new InternalAgentCompany(new SqlHelper()).GetAgentComProvince(agentid);
            return r;
        }

        public static string GetAgentComCity(int agentid)
        {
            string r = new InternalAgentCompany(new SqlHelper()).GetAgentComCity(agentid);
            return r;
        }
        /// <summary>
        /// //网上支付预付款
        /// </summary>
        /// <param name="comid"></param>
        /// <returns></returns>
        public static decimal Line_Imprest(int comid, DateTime subdate)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.Line_Imprest(comid, subdate);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// 后台充入预付款
        /// </summary>
        /// <param name="comid"></param>
        /// <returns></returns>
        public static decimal Hand_Imprest(int comid, DateTime subdate)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.Hand_Imprest(comid, subdate);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// 后台充入返点
        /// </summary>
        /// <param name="comid"></param>
        /// <param name="subdate"></param>
        /// <returns></returns>
        public static decimal Hand_Rebate(int comid, DateTime subdate)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.Hand_Rebate(comid, subdate);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// 商户下 分销总余额
        /// </summary>
        /// <param name="comid"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static decimal TotalImprest(int comid, DateTime subdate)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.TotalImprest(comid, subdate);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// 销售额
        /// </summary>
        /// <param name="comid"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static decimal Daoma_Sale_price(int comid, DateTime subdate)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.Daoma_Sale_price(comid, subdate);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// 分销商总毛利
        /// </summary>
        /// <param name="comid"></param>
        /// <param name="subdate"></param>
        /// <returns></returns>
        public static decimal Daoma_Maoli_price(int comid, DateTime subdate)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.Daoma_Maoli_price(comid, subdate);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// 已消费
        /// </summary>
        /// <param name="comid"></param>
        /// <param name="subdate"></param>
        /// <returns></returns>
        public static decimal Xiaofei_price(int comid, DateTime subdate)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.Xiaofei_price(comid, subdate);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }



        public static decimal Sale_price(int comid, DateTime subdate)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    decimal result = internalData.Sale_price(comid, subdate);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public static decimal Maoli_price(int comid, DateTime subdate)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.Maoli_price(comid, subdate);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public static decimal Chendian_price(int comid, DateTime subdate)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.Chendian_price(comid, subdate);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        //指定分销商本系统总销售量
        public static int Agent_AllSale(int agentid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.Agent_AllSale(agentid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }


        public static decimal OverMoney(int comid, DateTime subtime)
        {

            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    decimal result = internalData.OverMoney(comid, subtime);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public static int ChangeAgentsourcesort(int agentid, int Agentsourcesort)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    int result = internalData.ChangeAgentsourcesort(agentid, Agentsourcesort);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public List<Agent_Financial> Agent_Financialpagelist(int pageindex, int pagesize, string key, int agentid, int comid, string payment_type, out int totalcount)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    List<Agent_Financial> result = new InternalAgentCompany(sql).Agent_Financialpagelist(pageindex, pagesize, key, agentid, comid, payment_type, out totalcount);
                    return result;
                }
                catch
                {
                    totalcount = 0;
                    return null;
                }
            }
        }

        public string GetAgentCompanyName(int agentcompanyid)
        {
            using (var helper = new SqlHelper())
            {
                string r = new InternalAgentCompany(helper).GetAgentCompanyName(agentcompanyid);
                return r;
            }
        }

        public int GetAgentUserCountByMobile(string mobile, string account = "")
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalAgentCompany(helper).GetAgentUserCountByMobile(mobile, account);
                return r;
            }
        }

        public int UpAgentUserPass(string mobile, decimal code, string account)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalAgentCompany(helper).UpAgentUserPass(mobile, code, account);
                return r;
            }
        }

        public Agent_company GetFreeLandingAgentUserByOpenId(string openid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.GetFreeLandingAgentUserByOpenId(openid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public int GetAgentCountByMobile(string mobile)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalAgentCompany(helper).GetAgentCountByMobile(mobile);
                return r;
            }
        }

        public static int GetAgentidByTb_sellerid(string taobao_sellerid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalAgentCompany(helper).GetAgentidByTb_sellerid(taobao_sellerid);
                return r;
            }
        }
        ////判断除了当前分销外 ，该淘宝卖家是否绑定了其他分销
        //public bool IsbindAgentBytbsellerid(string tbsellerid, int agentid)
        //{
        //    using (var helper = new SqlHelper())
        //    {
        //        bool r = new InternalAgentCompany(helper).IsbindAgentBytbsellerid(tbsellerid,agentid);
        //        return r;
        //    }
        //}

        public static Agent_regiinfo GetAgentAccountByid(int accountid)
        {
            using (var sql = new SqlHelper())
            {

                var internalData = new InternalAgentCompany(sql);
                Agent_regiinfo result = internalData.GetAgentAccountByid(accountid );

                return result;

            }
        }

        public int  GetWarrant_type(int Agentid, int comid)
        {
            using (var sql = new SqlHelper())
            {
                int result = new InternalAgentCompany(sql).GetWarrant_type(Agentid,comid);

                return result;

            }
        }

        public Agent_company GetAgentCompanyByName(string companyname)
        {
            using (var sql = new SqlHelper())
            {
                Agent_company result = new InternalAgentCompany(sql).GetAgentCompanyByName(companyname);

                return result;

            }
        }

        public int UpAgentip(int agentid, string deskey)
        {
            using (var sql = new SqlHelper())
            {
                int result = new InternalAgentCompany(sql).UpAgentip(agentid,deskey);

                return result;

            }
        }

        public int Editagentoutinterface(int agentid, string agent_updateurl, string txtagentip, string inter_sendmethod="post")
        {
            using (var sql = new SqlHelper())
            {
                int result = new InternalAgentCompany(sql).Editagentoutinterface(agentid, agent_updateurl, txtagentip, inter_sendmethod);

                return result;

            }
        }

        public List<Agent_company> Getagentlist(string isapiagent="0,1")
        {
            using (var sql = new SqlHelper())
            {
                List<Agent_company> result = new InternalAgentCompany(sql).Getagentlist(isapiagent);

                return result;

            }
        }

        public List<Agent_warrant> GetWarrantlist(string Key, int Agentid, string comstate = "1,2", string containcomids = "")
        {
            using (var sql = new SqlHelper())
            {
               
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.GetWarrantlist(Key, Agentid, comstate, containcomids);
                    return result; 
            }
        }

        public List<B2b_company> UnWarrantlist(string Key, int Agentid, string containcomids)
        {
            using (var sql = new SqlHelper())
            {
               
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.UnWarrantlist(Key, Agentid, containcomids);
                    return result;
               
            }
        }

        public IList<Agent_company> GetMeituanAgentCompanyList(int comid)
        {
            using(var helper=new SqlHelper()){
                IList<Agent_company> result = new InternalAgentCompany(helper).GetMeituanAgentCompanyList(comid);
                return result;
            }
        }
        /// <summary>
        /// 根据美团PartnerId得到分销信息
        /// </summary>
        /// <param name="PartnerId"></param>
        /// <returns></returns>
        public Agent_company GetAgentCompanyByMeituanPartnerId(string PartnerId)
        {
             using(var helper=new SqlHelper())
             {
                 Agent_company info = new InternalAgentCompany(helper).GetAgentCompanyByMeituanPartnerId(PartnerId);
                 return info;
             }
        }

        /// <summary>
        /// 根据美团PartnerId得到分销信息
        /// </summary>
        /// <param name="PartnerId"></param>
        /// <returns></returns>
        public Agent_company GetAgentCompanyByLvmamaPartnerId(string lvmamauid)
        {
            using (var helper = new SqlHelper())
            {
                Agent_company info = new InternalAgentCompany(helper).GetAgentCompanyByLvmamaPartnerId(lvmamauid);
                return info;
            }
        }

        /// <summary>
        /// 得到所有的美团分销
        /// </summary>
        /// <returns></returns>
        public List<Agent_company> GetAllMeituanAgentCompany()
        {
            using (var helper = new SqlHelper())
            {
                List<Agent_company> list = new InternalAgentCompany(helper).GetAllMeituanAgentCompany();
                return list;
            }
        }
        /// <summary>
        /// 获得分销授权列表
        /// </summary>
        /// <param name="agentid"></param>
        /// <returns></returns>
        public List<Agent_warrant> GetAgentWarrantList(int agentid, string warrantstate)
        {
            using (var helper = new SqlHelper())
            {
                List<Agent_warrant> list = new InternalAgentCompany(helper).GetAgentWarrantList(agentid, warrantstate);
                return list;
            }
        }
        /// <summary>
        /// 判断是否是美团分销
        /// </summary>
        /// <param name="agentid"></param>
        /// <returns></returns>
        public bool IsMeituanAgent(int agentid)
        {
            using (var helper = new SqlHelper())
            {
                bool r = new InternalAgentCompany(helper).IsMeituanAgent(agentid);
                return r;
            }
        }
    }
}
