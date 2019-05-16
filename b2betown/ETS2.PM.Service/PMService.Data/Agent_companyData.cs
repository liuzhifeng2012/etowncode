using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;
using System.Data;
using ETS2.PM.Service.PMService.Data.InternalData;
using ETS2.CRM.Service.CRMService.Modle;


namespace ETS2.PM.Service.PMService.Data
{
    public class Agent_companyData
    {
        #region 授权产品项目列表
        public List<B2b_com_project> WarrantProjectlist(int Pageindex, int Pagesize, string Key, int Agentid, int Warrant_type, int comid, out int totalcount, int Agentlevel = 0)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.WarrantProjectlist(Pageindex, Pagesize, Key, Agentid, Warrant_type, comid, out totalcount, Agentlevel);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 授权产品列表
        public List<B2b_com_pro> WarrantProlist(int Pageindex, int Pagesize, string Key, int Agentid, int Warrant_type, int comid, out int totalcount, int Agentlevel = 0, int projectid=0,string viewmethod="")
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.WarrantProlist(Pageindex, Pagesize, Key, Agentid, Warrant_type, comid, out totalcount, Agentlevel, projectid,viewmethod);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion
        #region 授权产品列表
        public List<B2b_com_pro> AllWarrantProlist(int Pageindex, int Pagesize, string Key, int Agentid, out int totalcount, string viewmethod = "", int servertype=0)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.AllWarrantProlist(Pageindex, Pagesize, Key, Agentid,  out totalcount, viewmethod,servertype);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 未授权产品列表
        public List<B2b_com_pro> UnWarrantProlist(int Pageindex, int Pagesize, string Key, int Agentid, int Warrant_type, int comid, out int totalcount, int Agentlevel = 0, int projectid = 0, string viewmethod = "")
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.UnWarrantProlist(Pageindex, Pagesize, Key, Agentid, Warrant_type, comid, out totalcount, Agentlevel, projectid, viewmethod);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 授权产品列表
        public List<B2b_com_pro> BindingWarrantProlist(int Pageindex, int Pagesize, string Key, int Agentid, int Warrant_type, int bindingcomid,int comid, out int totalcount, int Agentlevel = 0)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.BindingWarrantProlist(Pageindex, Pagesize, Key, Agentid, Warrant_type, bindingcomid, comid, out totalcount, Agentlevel);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 单个产品授权价格
        public static decimal WarrantProPrice(int proid, int Agentid, int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.WarrantProPrice(proid, Agentid,comid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 导入产品
        public static decimal ImprestPro(int comid, int proid, int Agentlevel)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.ImprestPro(comid, proid, Agentlevel);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 导入产品规格
        public static decimal ImprestProGuige(int comid, int newproid,int oldproid, int Agentlevel)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.ImprestProGuige(comid, newproid, oldproid, Agentlevel);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 关联导入产品
        public static decimal UpImprestPro(int comid, int proid, int projectid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.UpImprestPro(comid, proid, projectid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 关联导入产品
        public static int SearchImprestNewProid(int comid, int proid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.SearchImprestNewProid(comid, proid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion
         #region 关联导入产品
        public static int SearchImprestNewProjectid(int comid, int proid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalAgentCompany(sql);
                    var result = internalData.SearchImprestNewProjectid(comid, proid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        /// <summary>
        /// 调整分销短信设置
        /// </summary>
        /// <param name="agentid"></param>
        /// <param name="agentmsgset"></param>
        /// <returns></returns>
        public int ChangeAgentMsgset(int agentid, int agentmsgset)
        {
             using(var helper=new SqlHelper())
             {
                 int r = new InternalAgentCompany(helper).ChangeAgentMsgSet(agentid,agentmsgset);
                 return r;
             }
        }

        public List<string> Getagentprovincelist(int comid=0)
        {
            using (var helper = new SqlHelper())
            {
                List<string> r = new InternalAgentCompany(helper).Getagentprovincelist(comid);
                return r;
            }
        }

        public int GetAgentIdByAccount(string Email)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalAgentCompany(helper).GetAgentIdByAccount(Email);
                return r;
            }
        }

        public int AgentUpPhone(int agentid, string newphone)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalAgentCompany(helper).AgentUpPhone(agentid,newphone);
                return r;
            }
        }

        public int AgentUserUpPhone(int agentuserid, string newphone)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalAgentCompany(helper).AgentUserUpPhone(agentuserid, newphone);
                return r;
            }
        }

        public int UpAgentTaobaoState(int istaobao,int agentid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalAgentCompany(helper).UpAgentTaobaoState(istaobao, agentid);
                return r;
            }
        }

        public string GetAgentType(int agentid)
        {
            using (var helper = new SqlHelper())
            {
                string r = new InternalAgentCompany(helper).GetAgentType(agentid);
                return r;
            }
        }

        public  Agent_warrant GetAgentWarrant(int Agentid, int comid)
        {
            using (var helper = new SqlHelper())
            {
                Agent_warrant r = new InternalAgentCompany(helper).GetAgentWarrant(Agentid,comid);
                return r;
            }
        }

        public string GetAgentName(int agentid)
        {
            using (var helper = new SqlHelper())
            {
                string r = new InternalAgentCompany(helper).GetAgentName(agentid);
                return r;
            }
        }
        /// <summary>
        /// 美团配置信息其他分销是否配置过
        /// </summary>
        /// <param name="regiinfo"></param>
        /// <returns></returns>
        public bool IsExistMeituanConfig(Agent_company regiinfo)
        {
            using (var helper = new SqlHelper())
            {
                bool r = new InternalAgentCompany(helper).IsExistMeituanConfig(regiinfo);
                return r;
            }
        }
        /// <summary>
        /// 获得产品的分销价格
        /// </summary>
        /// <param name="agentid"></param>
        /// <param name="productid"></param>
        /// <returns></returns>
        public Agent_warrant GetAgentWarrantInfo(int agentid, int productid)
        {
            using (var helper = new SqlHelper())
            {
                Agent_warrant r = new InternalAgentCompany(helper).GetAgentWarrantInfo(agentid,productid);
                return r;
            }
        }
    }
}
