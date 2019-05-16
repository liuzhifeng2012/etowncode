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

    public class B2bCompanyData
    {
        #region 添加或者编辑 By:Liankai

        /// <summary>
        /// 添加或者编辑公司基本信息 By:Xiaoliu
        /// </summary>
        /// <param name="model">商家 实体</param>
        /// <returns>标识列</returns>
        public static int InsertOrUpdate(B2b_company model)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompany(sql);
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

        /// <summary>
        /// 根据商家id得到商家基本信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static B2b_company GetCompany(int companyid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompany(sql);
                    B2b_company result = internalData.GetCompany(companyid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #region 得到公司基本信息和扩展信息
        public static B2b_company GetAllComMsg(int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompany(sql);
                    B2b_company com = internalData.GetCompany(comid);
                    if (com != null)
                    {
                        com.B2bcompanyinfo = new InternalB2bCompanyInfo(sql).GetCompanyInfo(comid);
                    }
                    return com;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 得到公司基本信息和扩展信息
        public static B2b_company_info GetB2bcompanyinfo(int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {

                   var com = new InternalB2bCompanyInfo(sql).GetCompanyInfo(comid);

                    return com;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 通过公司绑定的手机查询是否有此手机的商户
        public static int GetAllComMsgbyphone(string phone)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompany(sql);
                    var com = internalData.GetAllComMsgbyphone(phone);
                    return com;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 得到公司基本信息和扩展信息
        public static B2b_company GetAllComMsgbyAgentid(int agentid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompany(sql);
                    B2b_company com = internalData.GetCompanybyAgentid(agentid);
                    return com;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 通过关键词查询未开通的公司信息
        public static B2b_company SearchUnopenCom(string key)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompany(sql);
                    B2b_company com = internalData.SearchUnopenCom(key);
                    if (com != null)
                    {
                        com.B2bcompanyinfo = new InternalB2bCompanyInfo(sql).GetCompanyInfo(com.ID);
                    }
                    return com;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 根据域名得到COMID
        public static B2b_company_info GetComId(string domain)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompany(sql);
                    B2b_company_info com = internalData.DomainGetComId(domain);

                    return com;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 根据管理域名得到COMID
        public static B2b_company_info GetComIdByAdmindomain(string domain)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompany(sql);
                    B2b_company_info com = internalData.GetComIdByAdmindomain(domain);

                    return com;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 管理后台域名
        public static B2b_company_info AdminDomainGetComId(string domain)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompany(sql);
                    B2b_company_info com = internalData.AdminDomainGetComId(domain);

                    return com;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 根据用户id得到商家基本信息
        public static B2b_company GetCompanyByUid(int userid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompany(sql);
                    B2b_company result = internalData.GetCompanyByUid(userid);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        public List<B2b_company> ComList(int pageindex, int pagesize, out int totalcount)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompany(sql);
                    var result = internalData.ComList(pageindex, pagesize, out totalcount);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public string UpCom(int comid, int id, string state)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompany(sql);
                    var result = internalData.UpCom(comid, id, state);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        public string Agent_Open_Comid(int comid, int agentid,DateTime opendate,DateTime enddate)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompany(sql);
                    var result = internalData.Agent_Open_Comid(comid, agentid, opendate, enddate);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        



        public int AdjustFee(string id, decimal fee)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompany(sql);
                    var result = internalData.AdjustFee(id, fee);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public int AdjustServiceFee(string id, decimal ServiceFee)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompany(sql);
                    var result = internalData.AdjustServiceFee(id, ServiceFee);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public List<B2b_company> GetAllCompanys(out int firsttotalcount)
        {
            using (var sql = new SqlHelper())
            {
                    var internalData = new InternalB2bCompany(sql);
                    var result = internalData.GetAllCompanys(out firsttotalcount);

                    return result;
            }
        }
        public List<B2b_company> GetAllCompanys(string comstate,out int firsttotalcount)
        {
            using (var sql = new SqlHelper())
            {
                var internalData = new InternalB2bCompany(sql);
                var result = internalData.GetAllCompanys(comstate,out firsttotalcount);

                return result;
            }
        }


        public  string GetCompanyNameById(int comid)
        {
            if (comid == 0)
            {
                return "";
            }
            else
            {
                object o = ExcelSqlHelper.ExecuteScalar("select com_name from b2b_company where id=" + comid);
                return o == null ? "" : o.ToString();
            }
        }

        public B2b_company GetCompanyBasicById(int comid)
        {
            using (var sql = new SqlHelper())
            {
                var internalData = new InternalB2bCompany(sql);
                var result = internalData.GetCompanyBasicById(comid);

                return result;
            }
        }

        public int ChangeComType(int comid, int hangye)
        {
            using (var sql = new SqlHelper())
            {
                var internalData = new InternalB2bCompany(sql);
                var result = internalData.ChangeComType(comid,hangye);

                return result;
            }
        }


        //得到商户当天的 随机码
        public string GetComDayRandomstr(int comid, string posid,int randomnum=3)
        {
            using (var sql = new SqlHelper())
            {
                var internalData = new InternalB2bCompany(sql);
                var result = internalData.GetComDayRandomstr(comid, posid, randomnum);

                return result;
            }
        }

        public List<B2b_company_nowdayrandom> GetComDayRandomlist(int comid,DateTime searchdate, int randomnum = 3)
        {
            using (var sql = new SqlHelper())
            {
                var internalData = new InternalB2bCompany(sql);
                var result = internalData.GetComDayRandomlist(comid, searchdate, randomnum);

                return result;
            }
        }

        //生成当天的随机码（循环24次，每次都循环查一次，没有才插入，手工生成码）
        public int CreateComDayRandom(int comid, DateTime searchdate, int randomnum = 3)
        {
            using (var sql = new SqlHelper())
            {
                var internalData = new InternalB2bCompany(sql);
                var result = internalData.CreateComDayRandom(comid, searchdate, randomnum);

                return result;
            }
        }


        public static string GetComPhoneLogo(int comid)
        {
            using (var sql = new SqlHelper())
            {
                var internalData = new InternalB2bCompany(sql);
                var result = internalData.GetComPhoneLogo(comid);

                return result;
            }
        }

        public static string GetComWebLogo(int comid)
        {
            using (var sql = new SqlHelper())
            {
                var internalData = new InternalB2bCompany(sql);
                var result = internalData.GetComWebLogo(comid);

                return result;
            }
        }

        public int Editqunarbycomid(int comid, int isqunar, string qunar_username, string qunar_pass)
        {
            using(var helper=new SqlHelper())
            {
                var r = new InternalB2bCompany(helper).Editqunarbycomid(comid,isqunar,qunar_username,qunar_pass);
                return r;
            }
        }

        public B2b_company Getqunarbycomid(int comid)
        {
             using(var helper=new SqlHelper())
             {
                 B2b_company m = new InternalB2bCompany(helper).Getqunarbycomid(comid);
                 return m;
             }
        }

        public B2b_company GetqunarbyQunarname(string supplierIdentity)
        {
            using (var helper = new SqlHelper())
            {
                B2b_company m = new InternalB2bCompany(helper).GetqunarbyQunarname(supplierIdentity);
                return m;
            }
        }

        public int Getmicromallimgbycomid(int comid)
        {
            using (var helper = new SqlHelper())
            {
                int m = new InternalB2bCompany(helper).Getmicromallimgbycomid(comid);
                return m;
            }
        }

        public int Insmicroimg(int comid, int micromallimgid)
        {
            using (var helper = new SqlHelper())
            {
                int m = new InternalB2bCompany(helper).Insmicroimg(comid,micromallimgid);
                return m;
            }
        }

        public B2b_company GetMicromallByComid(int comid)
        {
             using(var helper=new SqlHelper())
             {
                 B2b_company m = new InternalB2bCompany(helper).GetMicromallByComid(comid);
                 return m;
             }
        }

        public int GetBindingAgentByComid(int  comid)
        {
            using (var helper = new SqlHelper())
            {
                int m = new InternalB2bCompany(helper).GetBindingAgentByComid(comid);
                return m;
            }
        }

        public List<B2b_company> Getcompanylist(string isapicompany="0,1")
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_company> m = new InternalB2bCompany(helper).Getcompanylist(isapicompany);
                return m;
            }
        }
    }
}
