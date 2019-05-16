using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Data.InternalData;

namespace ETS2.WeiXin.Service.WeiXinService.Data
{
    public class WxMaterialData
    {
        public int EditMaterial(WxMaterial material)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalWxMaterial(helper).EditMaterial(material);
                return id;
            }
        }

        public int DelMaterialKeyByMaterialId(int materialid)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalWxMaterial(helper).DelMaterialKeyByMaterialId(materialid);
                return id;
            }
        }

        public int InsMaterialKey(int MaterialId, string keyword)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalWxMaterial(helper).InsMaterialKey(MaterialId, keyword);
                return id;
            }
        }
        public WxMaterial GetWxMaterial(int materialid)
        {
            using (var helper = new SqlHelper())
            {
                WxMaterial wxmaterial = new InternalWxMaterial(helper).GetWxMaterial(materialid);
                return wxmaterial;
            }
        }
        public WxMaterial GetWxMaterial(int comid, int materialid)
        {
            using (var helper = new SqlHelper())
            {
                WxMaterial wxmaterial = new InternalWxMaterial(helper).GetWxMaterial(comid, materialid);
                return wxmaterial;
            }
        }
        /// <summary>
        ///  期
        /// </summary>
        /// <param name="periodicalid"></param>
        /// <returns></returns>
        public periodical selectperiodical(int periodicalid)
        {
            using (var helper = new SqlHelper())
            {
                periodical wxmaterial = new InternalWxMaterial(helper).selectperiodical(periodicalid);
                return wxmaterial;
            }
        }
        public periodical Selperiod(int percal, int wxtype)
        {
            using (var helper = new SqlHelper())
            {
                periodical wxmaterial = new InternalWxMaterial(helper).Selperiod(percal, wxtype);
                return wxmaterial;
            }
        }

        public periodical selectWxsaletype(int Wxsaletype, int comid)
        {
            using (var helper = new SqlHelper())
            {
                periodical wxmaterial = new InternalWxMaterial(helper).selectWxsaletype(Wxsaletype, comid);
                return wxmaterial;
            }
        }
        /// <summary>
        ///  添加 期
        /// </summary>
        /// <param name="periodicalid"></param>
        /// <returns></returns>
        public int Addperiod(int period, int comid, int Wxsaletypeid, int percal)
        {
            using (var helper = new SqlHelper())
            {
                int periodical = new InternalWxMaterial(helper).Addperiod(period, comid, Wxsaletypeid, percal);
                return periodical;
            }
        }

        public WxMaterial Getidinfo(int id)
        {
            using (var helper = new SqlHelper())
            {
                WxMaterial wxmaterial = new InternalWxMaterial(helper).Getidinfo(id);
                return wxmaterial;
            }
        }

  
        public List<WxMaterial> WxMaterialPageList(int pageindex, int pagesize, int applystate, int promotetypeid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMaterial(helper).WxMaterialPageList(pageindex, pagesize, applystate, promotetypeid, out totalcount);

                return list;
            }
        }
        public List<WxMaterial> WxMaterialPageList(int comid, int pageindex, int pagesize, int applystate, int promotetypeid, out int totalcount, string key = "", int top1 = 0, int consultantid = 0, int consultantpro=0)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMaterial(helper).WxMaterialPageList(comid, pageindex, pagesize, applystate, promotetypeid, out totalcount, key, top1, consultantid, consultantpro);

                return list;
            }
        }
        public List<WxMaterial> ShopWxMaterialPageList(int comid, int pageindex, int pagesize, int applystate,int menuid, int promotetypeid, out int totalcount, string key = "")
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMaterial(helper).ShopWxMaterialPageList(comid, pageindex, pagesize, applystate, menuid, promotetypeid, out totalcount, key);

                return list;
            }
        }
        public List<WxMaterial> ForwardingWxMaterialPageList(int comid, int pageindex, int pagesize, int applystate, int promotetypeid, int wxid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMaterial(helper).ForwardingWxMaterialPageList(comid, pageindex, pagesize, applystate, promotetypeid, wxid, out totalcount);

                return list;
            }
        }


        public int ForwardingWxMaterialcount(int comid, int wxid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMaterial(helper).ForwardingWxMaterialcount(comid, wxid);

                return list;
            }
        }


        //期
        public List<periodical> periodicalList(int pageindex, int pagesize, int applystate, int promotetypeid, int comid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMaterial(helper).periodicalList(pageindex, pagesize, applystate, promotetypeid, comid, out totalcount);

                return list;
            }
        }
        public List<periodical> periodicalList(int pageindex, int pagesize, int applystate, int promotetypeid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMaterial(helper).periodicalList(pageindex, pagesize, applystate, promotetypeid, out totalcount);

                return list;
            }
        }
        public List<WxMaterial> periodicaltypelist(int pageindex, int pagesize, int applystate, int periodid, int salepromotetypeid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMaterial(helper).periodicaltypelist(pageindex, pagesize, applystate, periodid, salepromotetypeid, out totalcount);

                return list;
            }
        }
        public List<WxMaterial> periodicaltypelist(int pageindex, int pagesize, int applystate, int promotetypeid, int type, int comid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMaterial(helper).periodicaltypelist(pageindex, pagesize, applystate, promotetypeid, type, comid, out totalcount);

                return list;
            }
        }
        //会员登录特惠
        public List<WxMaterial> LogWxMaterialPageList(int pageindex, int pagesize, int applystate, string promotetype, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMaterial(helper).logPageList(pageindex, pagesize, applystate, promotetype, out totalcount);

                return list;
            }
        }
        public WxMaterial logGetidinfo(string promotetype)
        {
            using (var helper = new SqlHelper())
            {
                WxMaterial wxmaterial = new InternalWxMaterial(helper).logGetidinfo(promotetype);
                return wxmaterial;
            }
        }

        public string GetWxMaterialKeyWordStrByMaterialId(int materialid)
        {
            using (var helper = new SqlHelper())
            {

                var keys = new InternalWxMaterial(helper).GetWxMaterialKeyWordStrByMaterialId(materialid);

                return keys;
            }
        }

        public int DelMaterial(int materialid)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalWxMaterial(helper).DelMaterial(materialid);
                return id;
            }
        }

        public int DelWxKeyWord(int materialid)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalWxMaterial(helper).DelWxKeyWord(materialid);
                return id;
            }
        }



        internal IList<WxMaterial> GetWxMaterialByKeyword(string keyword)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMaterial(helper).GetWxMaterialByKeyword(keyword);

                return list;
            }
        }
        internal IList<WxMaterial> GetWxMaterialByKeyword(string keyword, int comid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMaterial(helper).GetWxMaterialByKeyword(keyword, comid);

                return list;
            }
        }
        internal IList<WxMaterial> GetLatestWxMaterial()
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMaterial(helper).GetLatestWxMaterial();

                return list;
            }
        }



        public IList<WxMaterial> GetMaterialByPromoteType(int comid, int promotetypeid, int periodicalid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMaterial(helper).GetMaterialByPromoteType(comid, promotetypeid, periodicalid, out totalcount);

                return list;
            }
        }

        public IList<WxMaterial> GetMaterialByPromoteType(int promotetypeid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMaterial(helper).GetMaterialByPromoteType(promotetypeid, out totalcount);

                return list;
            }
        }

        public int SortMaterial(string materialid, int sortid)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalWxMaterial(helper).SortMaterial(materialid, sortid);
                return id;
            }
        }

        /// <summary>
        ///  插入Reservation数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int insert_Res(Reservation model)
        {
            using (var helper = new SqlHelper())
            {
                var num = new InternalWxMaterial(helper).insert_Res(model);
                return num;
            }
        }
        #region 活动加载明细列表
        public List<Reservation> Res_LoadingList(string comid, int pageindex, int pagesize, out int totalcount,int userid=0)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMaterial(helper).Res_LoadingList(comid, pageindex, pagesize, out totalcount,userid);

                return list;
            }
        }
        #endregion

        public List<Reservation> ResSearchList(string comid, int pageindex, int pagesize, string key, bool isNum, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMaterial(helper).ResSearchList(comid, pageindex, pagesize, key, isNum, out totalcount);

                return list;
            }
        }
        public Reservation Res_id(int id, int comid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMaterial(helper).Res_id(id, comid);

                return list;
            }
        }

        #region 修改预订信息
        public string upRes(Reservation quren)
        {
            using (var helper = new SqlHelper())
            {

                var res = new InternalWxMaterial(helper).upRes(quren);

                return res.ToString();
            }
        }
        #endregion

        public periodical GetPeriodicalBySaleType(int comid, int saletype)
        {
            //获得促销类型的最新期
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMaterial(helper).GetPeriodicalBySaleType(comid, saletype);

                return list;
            }
        }

        internal IList<WxMaterial> GetWxMaterialByNewestPeriodical(int pageindex, int pagesize, int applystate, int promotetypeid, int perical, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMaterial(helper).PageList(pageindex, pagesize, applystate, promotetypeid, perical, out totalcount);

                return list;
            }
        }
        internal IList<WxMaterial> GetWxMaterialByNewestPeriodical(int pageindex, int pagesize, int applystate, int promotetypeid, int perical, int comid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMaterial(helper).PageList(pageindex, pagesize, applystate, promotetypeid, perical, comid, out totalcount);

                return list;
            }
        }
        public int Editperiod(periodical model)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalWxMaterial(helper).Editperiod(model);
                return id;
            }
        }

        public int FrowardingSet(int materialid, int comid)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalWxMaterial(helper).FrowardingSet(materialid, comid);
                return id;
            }
        }
        public int FrowardingSetCannel(int materialid, int comid)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalWxMaterial(helper).FrowardingSetCannel(materialid, comid);
                return id;
            }
        }
        public int FrowardingSetSearch(int materialid)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalWxMaterial(helper).FrowardingSetSearch(materialid);
                return id;
            }
        }

        public int FrowardingSetList(int comid)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalWxMaterial(helper).FrowardingSetList(comid);
                return id;
            }
        }


        public int GetWxMaterialCountByPercal(int comid, int Wxsaletypeid, int percal)
        {
            using (var helper = new SqlHelper())
            {
                int count = new InternalWxMaterial(helper).GetWxMaterialCountByPercal(comid, Wxsaletypeid, percal);
                return count;
            }
        }

        public int GetNewestPerical(int Wxsaletypeid,int comid)
        {
            using (var helper = new SqlHelper())
            {
                int count = new InternalWxMaterial(helper).GetNewestPerical(Wxsaletypeid,comid);
                return count;
            }
        }

        public int Delmaterialtype(int typeid,   int comid)
        {
            using (var helper = new SqlHelper())
            {
                int count = new InternalWxMaterial(helper).Delmaterialtype(typeid,  comid);
                return count;
            }
        }

        internal IList<WxMaterial> GetWxMateriallistbytypeid(int wxmaterialtypeid, int topnums)
        {
            using (var helper = new SqlHelper())
            {
                IList<WxMaterial> r = new InternalWxMaterial(helper).GetWxMateriallistbytypeid(wxmaterialtypeid, topnums);
                return r;
            }
        }
    }
}
