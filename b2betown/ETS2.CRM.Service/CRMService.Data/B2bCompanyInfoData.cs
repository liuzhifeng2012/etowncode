using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data.InternalData;
using ETS2.Member.Service.MemberService.Model;

namespace ETS2.CRM.Service.CRMService.Data
{
    public class B2bCompanyInfoData
    {

        #region 添加或者编辑 By:Liankai

        /// <summary>
        /// 添加或者编辑 By:Xiaoliu
        /// </summary>
        /// <param name="model">商家扩展 实体</param>
        /// <returns>标识列</returns>
        public static int InsertOrUpdate(B2b_company_info model)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyInfo(sql);
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
        #region 修改商家资质信息
        public static int ModifyZizhi(int comextid, string comcode, string comsitecode, string comlicence, string scenic_intro = "", string domainname = "")
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyInfo(sql);
                    int result = internalData.Modifyzizhi(comextid, comcode, comsitecode, comlicence, scenic_intro, domainname);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 修改商家名称
        public static int UpdateB2bCompanyName(int comid, string comname)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyInfo(sql);
                    int result = internalData.UpdateB2bCompanyName(comid, comname);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion
         #region 修改搜索设定
        public static int UpdateB2bCompanySearchset(int comid, int setsearch)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyInfo(sql);
                    int result = internalData.UpdateB2bCompanySearchset(comid, setsearch);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion
        

        #region 修改商家授权和协议信息
        public static int ModifyComShouquan(int comextid, string sale_Agreement, string agent_Agreement)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyInfo(sql);
                    int result = internalData.ModifyComShouquan(comextid, sale_Agreement, agent_Agreement);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion
        #region 修改打印机绑定信息
        public static int ModifyBangPrint(int comextid, string Defaultprint)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyInfo(sql);
                    int result = internalData.ModifyBangPrint(comextid, Defaultprint);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion
        #region 获取特定公司的pos列表
        public List<B2b_company_info> PosList(string comid, int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bCompanyInfo(helper).PosList(comid, pageindex, pagesize, out totalcount);

                return list;
            }
        }
        #endregion
        public List<B2b_company_info> PosList(int pageindex, int pagesize, out int totalcount,string key="")
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bCompanyInfo(helper).PosList(pageindex, pagesize, out totalcount,key);

                return list;
            }
        }
        #region 获取特定公司的pos详细信息
        public List<B2b_company_info> PosInfo(int pos_id)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyInfo(sql);
                    var list = internalData.PosInfo(pos_id);
                    return list;
                }
                catch
                {
                    throw;
                }

                //var list = new InternalB2bCompanyInfo(helper).PosInfo(pos_id, pageindex, pagesize, out totalcount);

                // return list;
            }
        }
        #endregion

        #region 根据pOSID获取特定的pos详细信息
        public B2b_company_info PosInfobyposid(int pos_id)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyInfo(sql);
                    var list = internalData.PosInfobyposid(pos_id);
                    return list;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 增加修改POS绑定信息
        public static int ModifyBangPos(int posid, string poscompany, int com_id, int userid, string Remark, int pos_id, string md5key, int projectid = 0)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyInfo(sql);
                    int result = internalData.ModifyBangPos(posid, poscompany, com_id, userid, Remark, pos_id, md5key, projectid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 编辑短信
        public static int Insertnote(string key, string content, string title, bool radio, int note_id)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyInfo(sql);
                    int result = internalData.Insertnote(key, content, title, radio, note_id);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        public List<Member_sms> notList(string comid, int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bCompanyInfo(helper).notList(comid, pageindex, pagesize, out totalcount);

                return list;
            }
        }
        public List<Member_sms> noteInfo(int note_id)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bCompanyInfo(helper).noteInfo(note_id);

                return list;
            }
        }
        #endregion

        #region  删除短信
        public static int Delnote(int id, string key)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyInfo(sql);
                    int result = internalData.Delnote(id, key);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        public B2b_company_info GetB2bCompanyInfoByDomainname(string Domainname)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyInfo(sql);
                    var user = internalData.GetB2bCompanyInfoByDomainname(Domainname);
                    return user;
                }
                catch
                {
                    throw;
                }
            }
        }

        public B2b_company_info GetCompanyInfo(int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyInfo(sql);
                    var user = internalData.GetCompanyInfo(comid);
                    return user;
                }
                catch
                {
                    throw;
                }
            }
        }

        public int GetCompanyProCount(int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyInfo(sql);
                    var user = internalData.GetCompanyProCount(comid);
                    return user;
                }
                catch
                {
                    throw;
                }
            }
        }

        public int GetCompanyChengjiaoCount(int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyInfo(sql);
                    var user = internalData.GetCompanyChengjiaoCount(comid);
                    return user;
                }
                catch
                {
                    throw;
                }
            }
        }

        public int GetCompanyWeekChengjiaoCount(int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyInfo(sql);
                    var user = internalData.GetCompanyWeekChengjiaoCount(comid);
                    return user;
                }
                catch
                {
                    throw;
                }
            }
        }

        public List<B2b_company_info> ComList(int pageindex, int pagesize, int comstate, out int totalcount, string key = "")
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyInfo(sql);
                    var result = internalData.ComList(pageindex, pagesize, comstate, out totalcount, key);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public string UpCom(int id, string state)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyInfo(sql);
                    var result = internalData.UpCom(id, state);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        #region 商家详细列表
        public List<B2b_company_info> ComPageList(int pageindex, int pagesize, int prostate, out int totalcount)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyInfo(sql);
                    var result = internalData.ComPageList(pageindex, pagesize, prostate, out totalcount);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 商家详细列表
        public List<B2b_company_info> ComSelectpagelist(int prostate, int pageindex, int pagesize, string key, out int totalcount, int proclass = 0)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyInfo(sql);
                    var result = internalData.ComSelectpagelist(prostate, pageindex, pagesize, key, out totalcount, proclass);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion



        public string UpComstate(int id, string state)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyInfo(sql);
                    var result = internalData.UpComstate(id, state);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }



        public int SortCom(string comid, int sortid)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalB2bCompanyInfo(helper).SortCom(comid, sortid);
                return id;
            }
        }

        public int AdjustHasInnerChannel(int companyid, string hasinnerchannel)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalB2bCompanyInfo(helper).AdjustHasInnerChannel(companyid, hasinnerchannel);
                return result;
            }
        }



        public string Getmd5keybyposid(string pos_id)
        {
            using (var helper = new SqlHelper())
            {
                string result = new InternalB2bCompanyInfo(helper).Getmd5keybyposid(pos_id);
                return result;
            }
        }

        public int Editjoinpolicy(int comid, string joinpolicy)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalB2bCompanyInfo(helper).Editjoinpolicy(comid, joinpolicy);
                return result;
            }
        }
        public string Getjoinpolicy(int comid)
        {
            using (var helper = new SqlHelper())
            {
                string result = new InternalB2bCompanyInfo(helper).Getjoinpolicy(comid);
                return result;
            }
        }
    }
}
