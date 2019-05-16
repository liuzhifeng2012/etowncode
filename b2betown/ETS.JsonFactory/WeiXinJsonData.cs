using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.WeiXin.Service.WeiXinService.Model;
using Newtonsoft.Json;
using ETS2.WeiXin.Service.WeiXinService.Data;
using System.Collections;
using FileUpload.FileUpload.Data;
using ETS.Framework;
using ETS2.Common.Business;
using ETS2.WeiXin.Service.WeiXinService.Data.InternalData;
using System.Xml;
using System.Web;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Member.Service.MemberService.Data;
using ETS.Data.SqlHelper;
using ETS2.Permision.Service.PermisionService.Model;
using ETS2.Permision.Service.PermisionService.Data;
using System.Web.Script.Serialization;
using ETS2.WeiXin.Service.WinXinService.BLL;
using ETS2.WeiXin.Service.WinXinService.BLL.WeiXinUploadDown;
using System.Data.SqlClient;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.VAS.Service.VASService.Data;
using System.IO;
using ETS2.WeiXin.Service.WeiXinService.Model.Enum;
using WxPayAPI;

namespace ETS.JsonFactory
{
    public class WeiXinJsonData
    {
        private static object lockobj = new object();
        public static string EditMaterial(WxMaterial material)
        {
            try
            {

                var id = new WxMaterialData().EditMaterial(material);
                material.MaterialId = id;
                if (id > 0)
                {
                    string keystr = material.Keyword.Replace(",", "，");

                    bool editkey = true;

                    string errlog = "";

                    if (keystr.IndexOf("，") == -1)//不存在逗号即一个关键词
                    {
                        string[] keygroup = { keystr };


                        editkey = EditMaterialKey(material, keygroup, out errlog);
                    }
                    else
                    {
                        string[] keygroup = keystr.Split('，');

                        editkey = EditMaterialKey(material, keygroup, out errlog);
                    }

                    if (editkey)
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = id });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = errlog });
                    }

                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "编辑素材返回id为0" });
                }

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }
        /// <summary>
        /// 编辑素材对应关键词
        /// </summary>
        /// <param name="material"></param>
        /// <param name="keystr"></param>
        /// <returns></returns>
        private static bool EditMaterialKey(WxMaterial material, string[] keygroup, out string errlog)
        {
            errlog = "";
            int delkey = new WxMaterialData().DelWxKeyWord(material.MaterialId);
            int delmaterialkey = new WxMaterialData().DelMaterialKeyByMaterialId(material.MaterialId);

            for (int i = 0; i < keygroup.Length; i++)
            {

                int inskey = new WxMaterialData().InsMaterialKey(material.MaterialId, keygroup[i]);
                if (inskey <= 0)
                {
                    errlog += "编辑关键词ins出错";
                }
            }

            if (errlog.Length > 0)
            {
                return false;
            }
            else
            {
                return true;
            }


        }

        public static string GetWxMaterial(int comid, int materialid)
        {
            try
            {
                WxMaterial wxmaterial = new WxMaterialData().GetWxMaterial(comid, materialid);
                if (wxmaterial != null)
                {
                    periodical period = new WxMaterialData().selectperiodical(wxmaterial.Periodicalid);
                    return JsonConvert.SerializeObject(new { type = 100, msg = wxmaterial, per = period });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "数据库不存在记录" });
                }
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }

        public static string selecttype(int typeid, int comid)
        {
            try
            {
                periodical period = new WxMaterialData().selectWxsaletype(typeid, comid);
                if (period == null)
                {
                    //添加一个新操作，第一次发布文章自动到第一期
                    periodical model = new periodical
                    {
                        Id = 0,
                        Comid = comid,
                        Percal = 1,
                        Perinfo = "",
                        Peryear = DateTime.Now.Year,
                        Uptime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        Wxsaletypeid1 = typeid
                    };

                    int id = new WxMaterialData().Editperiod(model);
                    model.Id = id;
                    return JsonConvert.SerializeObject(new { type = 100, msg = model });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = period });
                }

            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }

        public static string Selpercal(int percal, int wxtype)
        {
            try
            {
                periodical period = new WxMaterialData().Selperiod(percal, wxtype);
                return JsonConvert.SerializeObject(new { type = 100, msg = period });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }


        public static string Addperiod(int period, int comid, int Wxsaletypeid, int percal)
        {
            try
            {
                int periodmun = new WxMaterialData().Addperiod(period, comid, Wxsaletypeid, percal);

                periodical periodmsg = new WxMaterialData().selectWxsaletype(Wxsaletypeid, comid);

                return JsonConvert.SerializeObject(new { type = 100, msg = periodmun, per = periodmsg });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }

        public static string WxMaterialPageList(int comid, int pageindex, int pagesize, int applystate, int promotetypeid, string key = "")
        {

            var totalcount = 0;
            try
            {

                var actdata = new WxMaterialData();
                var list = actdata.WxMaterialPageList(comid, pageindex, pagesize, applystate, promotetypeid, out totalcount, key);

                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 MaterialId = pro.MaterialId,
                                 Title = pro.Title,
                                 Applystate = pro.Applystate == 1 ? "使用" : "暂停",
                                 Author = pro.Author,
                                 Imgpath = FileSerivce.GetImgUrl(pro.Imgpath.ConvertTo<int>(0)),
                                 Keyword = actdata.GetWxMaterialKeyWordStrByMaterialId(pro.MaterialId),

                                 Summary = pro.Summary,
                                 Articleurl = pro.Articleurl,

                                 Phone = pro.Phone,
                                 Price = pro.Price,
                                 PromoteTypeId = pro.SalePromoteTypeid,

                                 Forcount = actdata.ForwardingWxMaterialcount(pro.Comid, pro.MaterialId),
                                 Forset = actdata.FrowardingSetSearch(pro.MaterialId)

                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


        public static string ArticleWxMaterialPageList(int comid, int pageindex, int pagesize, int applystate, int promotetypeid, int id)
        {

            var totalcount = 0;
            try
            {
                //如果 独立文章 读取此文章内容
                WxMaterial Articleinfo = null;
                periodical ArtPeriodical = null;

                if (id != 0)
                {
                    var actdata = new WxMaterialData();
                    Articleinfo = actdata.Getidinfo(id);
                    if (Articleinfo != null)
                    {
                        Articleinfo.Imgpath = FileSerivce.GetImgUrl(Articleinfo.Imgpath.ConvertTo<int>(0));
                        promotetypeid = Articleinfo.Periodicalid;
                        ArtPeriodical = new WxMaterialData().selectperiodical(Articleinfo.Periodicalid);
                    };

                }
                if (pageindex != 1)
                {//下拉 加载第二,清空，上面主要加载 分类参数
                    Articleinfo = null;

                }

                //重新赋值，当等于0的时候 1000000
                if (promotetypeid == 0)
                {
                    promotetypeid = 1000000;
                }


                var actdata1 = new WxMaterialData();
                var list1 = actdata1.WxMaterialPageList(comid, pageindex, pagesize, 1, promotetypeid, out totalcount, "");

                IEnumerable result1 = "";
                if (list1 != null)
                    result1 = from pro in list1
                              select new
                              {
                                  MaterialId = pro.MaterialId,
                                  Title = pro.Title,
                                  Applystate = pro.Applystate == 1 ? "使用" : "暂停",
                                  Author = pro.Author,
                                  Imgpath = FileSerivce.GetImgUrl(pro.Imgpath.ConvertTo<int>(0)),
                                  Keyword = actdata1.GetWxMaterialKeyWordStrByMaterialId(pro.MaterialId),

                                  Summary = pro.Summary,
                                  Articleurl = pro.Articleurl,
                                  Operatime = pro.Operatime,
                                  Phone = pro.Phone,
                                  Price = pro.Price,
                                  PromoteTypeId = pro.SalePromoteTypeid,
                                  Article = pro.Article,
                                  Forcount = actdata1.ForwardingWxMaterialcount(pro.Comid, pro.MaterialId),
                                  Forset = actdata1.FrowardingSetSearch(pro.MaterialId),
                                  Periodical = new WxMaterialData().selectperiodical(pro.Periodicalid)
                              };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result1, artmsg = Articleinfo, artper = ArtPeriodical });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


        public static string TopArticleWxMaterialPageList(int comid, int pageindex, int pagesize, int applystate, int promotetypeid, int id)
        {

            var totalcount = 0;
            try
            {

                var actdata1 = new WxMaterialData();
                var list1 = actdata1.WxMaterialPageList(comid, pageindex, pagesize, 1, promotetypeid, out totalcount, "", 1);

                IEnumerable result1 = "";
                if (list1 != null)
                    result1 = from pro in list1
                              select new
                              {
                                  MaterialId = pro.MaterialId,
                                  Title = pro.Title,
                                  Applystate = pro.Applystate == 1 ? "使用" : "暂停",
                                  Author = pro.Author,
                                  Imgpath = FileSerivce.GetImgUrl(pro.Imgpath.ConvertTo<int>(0)),

                                  Summary = pro.Summary,
                                  Articleurl = pro.Articleurl,
                                  Operatime = pro.Operatime,
                                  Phone = pro.Phone,
                                  Price = pro.Price,
                                  PromoteTypeId = pro.SalePromoteTypeid,

                              };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result1 });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


        public static string ShopWxMaterialPageList(int comid, int pageindex, int pagesize, int applystate, int menuid, int promotetypeid, string key = "")
        {

            var totalcount = 0;
            try
            {

                var actdata = new WxMaterialData();
                var list = actdata.ShopWxMaterialPageList(comid, pageindex, pagesize, applystate, menuid, promotetypeid, out totalcount, key);

                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 MaterialId = pro.MaterialId,
                                 Title = pro.Title,
                                 Applystate = pro.Applystate == 1 ? "使用" : "暂停",
                                 Author = pro.Author,
                                 Imgpath = FileSerivce.GetImgUrl(pro.Imgpath.ConvertTo<int>(0)),
                                 Keyword = actdata.GetWxMaterialKeyWordStrByMaterialId(pro.MaterialId),

                                 Summary = pro.Summary,
                                 Articleurl = pro.Articleurl,

                                 Phone = pro.Phone,
                                 Price = pro.Price,
                                 PromoteTypeId = pro.SalePromoteTypeid,

                                 Forcount = actdata.ForwardingWxMaterialcount(pro.Comid, pro.MaterialId),
                                 Forset = actdata.FrowardingSetSearch(pro.MaterialId)

                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


        public static string ForwardingWxMaterialPageList(int comid, int pageindex, int pagesize, int applystate, int promotetypeid, int wxid)
        {

            var totalcount = 0;
            try
            {

                var actdata = new WxMaterialData();
                var list = actdata.ForwardingWxMaterialPageList(comid, pageindex, pagesize, applystate, promotetypeid, wxid, out totalcount);

                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 MaterialId = pro.MaterialId,
                                 Title = pro.Title,
                                 Name = pro.Name,
                                 Idcard = pro.Idcard,
                                 Fornum = pro.Fornum
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string periodicalList(int pageindex, int pagesize, int applystate, int promotetypeid, int comid)
        {

            var totalcount = 0;
            try
            {

                var actdata = new WxMaterialData();
                var list = actdata.periodicalList(pageindex, pagesize, applystate, promotetypeid, comid, out totalcount);

                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Comid = pro.Comid,
                                 Wxsaletypeid = pro.Wxsaletypeid1,
                                 Percal = pro.Percal,
                                 Peryear = pro.Peryear,
                                 Uptime = pro.Uptime,
                                 Perinfo = pro.Perinfo
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        public static string periodicaltypelist(int pageindex, int pagesize, int applystate, int promotetypeid, int type, int comid)
        {

            var totalcount = 0;
            try
            {

                var actdata = new WxMaterialData();
                var list = actdata.periodicaltypelist(pageindex, pagesize, applystate, promotetypeid, type, comid, out totalcount);

                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 MaterialId = pro.MaterialId,
                                 Title = pro.Title,
                                 Applystate = pro.Applystate == 1 ? "使用" : "暂停",
                                 Author = pro.Author,
                                 Imgpath = FileSerivce.GetImgUrl(pro.Imgpath.ConvertTo<int>(0)),
                                 Keyword = actdata.GetWxMaterialKeyWordStrByMaterialId(pro.MaterialId),

                                 Summary = pro.Summary,
                                 Articleurl = pro.Articleurl,

                                 Phone = pro.Phone,
                                 Price = pro.Price,
                                 PromoteTypeId = pro.SalePromoteTypeid,
                                 Periodical = new WxMaterialData().selectperiodical(pro.Periodicalid).ToString() == null ? "--" : new WxMaterialData().selectperiodical(pro.Periodicalid).Percal.ToString()

                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        //加载上一期
        public static string periodicaltypelastlist(int pageindex, int pagesize, int applystate, int promotetypeid, int type, int comid)
        {

            var totalcount = 0;
            var lastid = 0;
            try
            {

                periodical per = new WxMaterialData().selectperiodical(promotetypeid);
                if (per != null)
                {
                    var periodca = per.Percal;

                    if (periodca > 1)
                    {
                        int lastperiodca = periodca - 1;
                        periodical lastper = new WxMaterialData().Selperiod(lastperiodca, type);
                        lastid = lastper.Id;
                    }
                }

                var actdata = new WxMaterialData();
                var list = actdata.periodicaltypelist(pageindex, pagesize, applystate, lastid, type, comid, out totalcount);

                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 MaterialId = pro.MaterialId,
                                 Title = pro.Title,
                                 Applystate = pro.Applystate == 1 ? "使用" : "暂停",
                                 Author = pro.Author,
                                 Imgpath = FileSerivce.GetImgUrl(pro.Imgpath.ConvertTo<int>(0)),
                                 Keyword = actdata.GetWxMaterialKeyWordStrByMaterialId(pro.MaterialId),

                                 Summary = pro.Summary,
                                 Articleurl = pro.Articleurl,

                                 Phone = pro.Phone,
                                 Price = pro.Price,
                                 PromoteTypeId = pro.SalePromoteTypeid,
                                 Periodical = new WxMaterialData().selectperiodical(pro.Periodicalid).ToString() == null ? "--" : new WxMaterialData().selectperiodical(pro.Periodicalid).Percal.ToString()
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result, lastid = lastid });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string DelWxMaterial(int materialid)
        {
            try
            {
                int delmaterial = new WxMaterialData().DelMaterial(materialid);
                int delkey = new WxMaterialData().DelWxKeyWord(materialid);

                return JsonConvert.SerializeObject(new { type = 100, msg = delmaterial });

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }

        }

        public static string FrowardingSet(int materialid, int comid)
        {
            try
            {

                int delmaterial = new WxMaterialData().FrowardingSet(materialid, comid);

                if (delmaterial > 0)
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = delmaterial });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "已经有此条设定了" });
                }


            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }

        }

        public static string FrowardingSetCannel(int materialid, int comid)
        {
            try
            {

                int delmaterial = new WxMaterialData().FrowardingSetCannel(materialid, comid);

                if (delmaterial > 0)
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = delmaterial });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "已经有此条设定了" });
                }


            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }

        }
        public static string GetMenuDetail(int menuid, int comid)
        {
            try
            {
                WxMenu wxmenu = new WxMenuData().GetWxMenu(menuid, comid);
                if (wxmenu != null)
                {
                    IList<WxMenu> list = new List<WxMenu>();
                    list.Add(wxmenu);

                    IEnumerable result = "";
                    if (list != null)

                        result = from pro in list
                                 select new
                                 {
                                     Menuid = pro.Menuid,
                                     Name = pro.Name,
                                     Instruction = pro.Instruction,

                                     Linkurl = pro.Linkurl,
                                     Fathermenuid = pro.Fathermenuid,
                                     Operationtypeid = pro.Operationtypeid,
                                     SalePromoteTypeid = pro.SalePromoteTypeid,
                                     Wxanswertext = pro.Wxanswertext,
                                     FatherMenuName = pro.Fathermenuid == 0 ? "" : new WxMenuData().GetWxMenu(pro.Fathermenuid, pro.Comid).Name,
                                     Product_class = pro.Product_class,
                                     Keyy = pro.Keyy,
                                     pictexttype = pro.pictexttype
                                 };

                    return JsonConvert.SerializeObject(new { type = 100, msg = result });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "数据库不存在记录" });
                }
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }

        public static string EditWxmenu(WxMenu wxmenu)
        {
            try
            {
                string err = "";
                int editmenuid = 0;//编辑的菜单的id
                if (wxmenu.Fathermenuid == 0 && wxmenu.Menuid == 0)//加一级菜单限制（最多3条一级菜单）
                {
                    int totalcount = 0;
                    IList<WxMenu> menulist = new WxMenuData().GetFristMenuList(1, 10, wxmenu.Comid, out totalcount);
                    if (menulist != null)
                    {
                        if (totalcount >= 3)
                        {
                            err = "微信1级菜单限制最多3条";
                            return JsonConvert.SerializeObject(new { type = 1, msg = err });
                        }
                    }
                }

                editmenuid = new WxMenuData().EditWxMenu(wxmenu);

                if (editmenuid <= 0)
                {
                    err = "编辑菜单出错";
                }


                if (err == "")
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = editmenuid });
                }
                else
                {

                    return JsonConvert.SerializeObject(new { type = 1, msg = err });
                }



            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }

        public static string GetFristMenuList(int pageindex, int pagesize, int applystate, int comid)
        {
            var totalcount = 0;
            try
            {

                var actdata = new WxMenuData();
                var list = actdata.GetFristMenuList(pageindex, pagesize, comid, out totalcount);

                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 MenuId = pro.Menuid,
                                 Name = pro.Name,

                                 FatherMenuName = pro.Fathermenuid == 0 ? "" : new WxMenuData().GetWxMenu(pro.Fathermenuid).Name,
                                 Level = pro.Fathermenuid == 0 ? "一级菜单" : "二级菜单",
                                 MenuOperationType = new WxOperationTypeData().GetOprationType(pro.Operationtypeid).Typename,
                                 MenuOperationTypeId = pro.Operationtypeid
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string GetSecondMenuList(int fatherid, int comid)
        {
            var totalcount = 0;
            try
            {

                var actdata = new WxMenuData();
                var list = actdata.GetSecondMenuList(fatherid, comid, out totalcount);

                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 MenuId = pro.Menuid,
                                 Name = pro.Name,

                                 FatherMenuName = pro.Fathermenuid == 0 ? "" : new WxMenuData().GetWxMenu(pro.Fathermenuid).Name,
                                 Level = pro.Fathermenuid == 0 ? "一级菜单" : "二级菜单",
                                 MenuOperationType = new WxOperationTypeData().GetOprationType(pro.Operationtypeid).Typename,

                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string Delwxmenu(int wxmenuid)
        {
            try
            {
                int delmaterial = new WxMenuData().Delwxmenu(wxmenuid);

                return JsonConvert.SerializeObject(new { type = 100, msg = delmaterial });

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        ////-----------------------微信自定义菜单管理----------------------//
        //public static string AppId = "wxa2c85d5ead479d5b";//第三方用户唯一凭证
        //public static string AppSecret = "83860d281b1698f5f88269a2abd8a8df";//第三方用户唯一凭证密钥
        public static string CreateWxMenu(int comid)
        {
            //根据公司id得到开发者凭据
            WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(comid);
            if (basicc != null)
            {
                //获取凭证（首先判断数据库是否存在凭证以及是否过期，如过期，重新获取）
                WXAccessToken token = GetAccessToken(comid, basicc.AppId, basicc.AppSecret);
                //创建自定义菜单
                return CreateMenu(token, comid);
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "公司开发者凭据获取为null" });
            }


        }
        /// <summary>
        /// 获取凭证（首先判断数据库是否存在凭证以及是否过期，如过期，重新获取）
        /// </summary>
        /// <returns></returns>
        private static WXAccessToken GetAccessToken(int comid, string AppId, string AppSecret)
        {
            DateTime fitcreatetime = DateTime.Now.AddHours(-2);
            WXAccessToken token = new WXAccessTokenData().GetLaststWXAccessToken(fitcreatetime, comid);
            if (token == null)
            {
                string geturl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppId + "&secret=" + AppSecret;
                string jsonText = new GetUrlData().HttpGet(geturl);

                XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + jsonText + "}");

                XmlElement rootElement = doc.DocumentElement;
                string access_token = rootElement.SelectSingleNode("access_token").InnerText;

                //把获取到的凭证录入数据库中
                token = new WXAccessToken()
                {
                    Id = 0,
                    ACCESS_TOKEN = access_token,
                    CreateDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    Comid = comid

                };
                int edittoken = new WXAccessTokenData().EditAccessToken(token);
            }
            return token;
        }
        private static string CreateMenu(WXAccessToken token, int comid)
        {
            WeiXinBasic weixinbs = new WeiXinBasicData().GetWxBasicByComId(comid);
            int weixintype = weixinbs.Weixintype;//微信类型:4认证服务号


            string err = "";//返回错误原因

            string createmenuurl = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + token.ACCESS_TOKEN;

            string createmenutext1 = "";//微信菜单内容

            int totalcount = 0;
            IList<WxMenu> firstmenulist = new WxMenuData().GetFristMenuList(1, 10, comid, out totalcount);
            if (firstmenulist != null)
            {
                createmenutext1 = "{\"button\":[";
                foreach (WxMenu firstmenu in firstmenulist)
                {
                    //根据1级菜单获得二级菜单
                    int fatherid = firstmenu.Menuid;

                    IList<WxMenu> secondmenulist = new WxMenuData().GetSecondMenuList(fatherid, comid, out totalcount);

                    if (secondmenulist.Count == 0)//只有1级菜单,二级菜单不存在
                    {

                        if (firstmenu.Operationtypeid == 1)//如果1级菜单类型是“弹出子菜单”，并且二级菜单数量为0
                        {
                            err = firstmenu.Name + "子菜单不可为空";
                        }
                        else if (firstmenu.Operationtypeid == 2) //如果操作类型是“链接新页面”，type="view";其他都是type="click"
                        {
                            createmenutext1 += "{\"type\":\"view\",\"name\":\"" + firstmenu.Name + "\",\"url\":\"" + firstmenu.Linkurl + "\"},";
                        }
                        else if (firstmenu.Operationtypeid == 21) //如果操作类型是“项目分类(链接页面)”，type="view"; 
                        {
                            if (weixintype == 4)//认证服务号
                            {
                                createmenutext1 += "{\"type\":\"view\",\"name\":\"" + firstmenu.Name + "\",\"url\":\"" + firstmenu.Linkurl + "\"},";
                            }
                            else
                            {
                                createmenutext1 += "{\"type\":\"click\",\"name\":\"" + firstmenu.Name + "\",\"key\":\"" + firstmenu.Menuid + "\"},";
                            }
                        }
                        else if (firstmenu.Operationtypeid == 22) //如果操作类型是“关键词(链接页面)”，type="view"; 
                        {
                            if (weixintype == 4)//认证服务号
                            {
                                createmenutext1 += "{\"type\":\"view\",\"name\":\"" + firstmenu.Name + "\",\"url\":\"" + firstmenu.Linkurl + "\"},";
                            }
                            else
                            {
                                createmenutext1 += "{\"type\":\"click\",\"name\":\"" + firstmenu.Name + "\",\"key\":\"" + firstmenu.Menuid + "\"},";
                            }
                        }
                        else if (firstmenu.Operationtypeid == 23) //如果操作类型是“扫码推事件”，type="scancode_push"; 
                        {

                            createmenutext1 += "{\"type\":\"scancode_push\",\"name\":\"" + firstmenu.Name + "\",\"key\":\"" + firstmenu.Menuid + "\",\"sub_button\": [ ]},";

                        }
                        else if (firstmenu.Operationtypeid == 24) //如果操作类型是“拍照或者相册发图”，type="pic_photo_or_album"; 
                        {

                            createmenutext1 += "{\"type\":\"pic_photo_or_album\",\"name\":\"" + firstmenu.Name + "\",\"key\":\"" + firstmenu.Menuid + "\",\"sub_button\": [ ]},";

                        }
                        else if (firstmenu.Operationtypeid == 25) //如果操作类型是“发送位置”，type="location_select"; 
                        {

                            createmenutext1 += "{\"type\":\"location_select\",\"name\":\"" + firstmenu.Name + "\",\"key\":\"" + firstmenu.Menuid + "\",\"sub_button\": [ ]},";

                        }
                        else
                        {
                            createmenutext1 += "{\"type\":\"click\",\"name\":\"" + firstmenu.Name + "\",\"key\":\"" + firstmenu.Menuid + "\"},";
                        }

                    }
                    else//1级，二级菜单都存在（也就是一级弹出二级菜单）
                    {
                        createmenutext1 += "{\"name\":\"" + firstmenu.Name + "\",\"sub_button\":[";
                        foreach (WxMenu secondmenu in secondmenulist)
                        {
                            //如果二级菜单操作类型是“链接新页面”，type="view";其他都是type="click"
                            if (secondmenu.Operationtypeid == 1)//弹出子菜单
                            {
                                err += secondmenu.Name + "操作类型出现错误，不可为弹出子菜单";
                            }
                            else if (secondmenu.Operationtypeid == 2)//链接新页面
                            {
                                createmenutext1 += "{\"type\":\"view\",\"name\":\"" + secondmenu.Name + "\",\"url\":\"" + secondmenu.Linkurl + "\"},";
                            }
                            else if (secondmenu.Operationtypeid == 21)//项目分类(链接页面)
                            {
                                if (weixintype == 4)//认证服务号
                                {
                                    createmenutext1 += "{\"type\":\"view\",\"name\":\"" + secondmenu.Name + "\",\"url\":\"" + secondmenu.Linkurl + "\"},";
                                }
                                else
                                {
                                    createmenutext1 += "{\"type\":\"click\",\"name\":\"" + secondmenu.Name + "\",\"key\":\"" + secondmenu.Menuid + "\"},";
                                }
                            }
                            else if (secondmenu.Operationtypeid == 22)//关键词(链接页面)
                            {
                                if (weixintype == 4)//认证服务号
                                {
                                    createmenutext1 += "{\"type\":\"view\",\"name\":\"" + secondmenu.Name + "\",\"url\":\"" + secondmenu.Linkurl + "\"},";
                                }
                                else
                                {
                                    createmenutext1 += "{\"type\":\"click\",\"name\":\"" + secondmenu.Name + "\",\"key\":\"" + secondmenu.Menuid + "\"},";
                                }
                            }
                            else if (secondmenu.Operationtypeid == 23) //如果操作类型是“扫码推事件”，type="scancode_push"; 
                            {

                                createmenutext1 += "{\"type\":\"scancode_push\",\"name\":\"" + secondmenu.Name + "\",\"key\":\"" + secondmenu.Menuid + "\",\"sub_button\": [ ]},";

                            }
                            else if (secondmenu.Operationtypeid == 24) //如果操作类型是“拍照或者相册发图”，type="pic_photo_or_album"; 
                            {

                                createmenutext1 += "{\"type\":\"pic_photo_or_album\",\"name\":\"" + secondmenu.Name + "\",\"key\":\"" + secondmenu.Menuid + "\",\"sub_button\": [ ]},";

                            }
                            else if (secondmenu.Operationtypeid == 25) //如果操作类型是“发送位置”，type="location_select"; 
                            {

                                createmenutext1 += "{\"type\":\"location_select\",\"name\":\"" + secondmenu.Name + "\",\"key\":\"" + secondmenu.Menuid + "\",\"sub_button\": [ ]},";

                            }
                            else
                            {
                                createmenutext1 += "{\"type\":\"click\",\"name\":\"" + secondmenu.Name + "\",\"key\":\"" + secondmenu.Menuid + "\"},";
                            }
                        }
                        createmenutext1 = createmenutext1.Substring(0, createmenutext1.Length - 1);
                        createmenutext1 += "]},";
                    }
                }
                createmenutext1 = createmenutext1.Substring(0, createmenutext1.Length - 1);
                createmenutext1 += "]}";


            }
            else
            {
                err += "自定义菜单列表为空";

            }
            if (err.Length > 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = err });
            }
            else
            {

                string createmenuutret = new GetUrlData().HttpPost(createmenuurl, createmenutext1);
                XmlDocument createselfmenudoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + createmenuutret + "}");

                XmlElement createselfmenurootElement = createselfmenudoc.DocumentElement;
                string createerrcode = createselfmenurootElement.SelectSingleNode("errcode").InnerText;
                string createerrmsg = createselfmenurootElement.SelectSingleNode("errmsg").InnerText;
                if (createerrcode != "0")
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "创建自定义菜单出错" + createerrcode + " " + createerrmsg });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = "成功发布微信自定义菜单。由于微信客户端缓存，一般发布后1-24小时才会展现出来。建 议尝试取消关注公众账号后，再次关注则可以看到新创建后的效果。" });
                }
            }

        }


        public static string DelChildrenMenu(int fathermenuid)
        {
            try
            {
                if (fathermenuid == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "父菜单id不可为0" });
                }
                else
                {
                    int delmenu = new WxMenuData().DelChildrenMenu(fathermenuid);


                    return JsonConvert.SerializeObject(new { type = 100, msg = delmenu });
                }



            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string GetMenuList(int fathermenuid, int comid)
        {
            var totalcount = 0;
            try
            {

                var actdata = new WxMenuData();
                var list = actdata.GetMenuList(fathermenuid, comid, out totalcount);

                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 MenuId = pro.Menuid,
                                 Name = pro.Name,

                                 FatherMenuName = pro.Fathermenuid == 0 ? "" : new WxMenuData().GetWxMenu(pro.Fathermenuid).Name,
                                 Level = pro.Fathermenuid == 0 ? "一级菜单" : "二级菜单",
                                 MenuOperationType = new WxOperationTypeData().GetOprationType(pro.Operationtypeid).Typename,

                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string MenuSort(string menuids)
        {
            if (menuids == "")
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "没有排序的元素" });
            }
            else
            {
                string[] str = menuids.Split(',');

                string err = "";
                for (int i = 1; i <= str.Length; i++)
                {
                    string menuid = str[i - 1];
                    int sortid = i;
                    int sortmenu = new WxMenuData().SortMenu(menuid, sortid);
                    if (sortmenu == 0)
                    {
                        err += menuid + "err;";

                    }
                }
                if (err == "")
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = "菜单排序成功" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = err });
                }
            }
        }

        public static string GetMaterialByPromoteType(int comid, int promotetypeid)
        {
            var totalcount = 0;
            try
            {
                //根据公司id和促销类型id得到此类型最新期
                periodical pp = new WxMaterialData().GetPeriodicalBySaleType(comid, promotetypeid);
                int maxqiid = pp.Id;


                var actdata = new WxMaterialData();
                var list = actdata.GetMaterialByPromoteType(comid, promotetypeid, maxqiid, out totalcount);

                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 MaterialId = pro.MaterialId,
                                 Title = pro.Title,
                                 Applystate = pro.Applystate == 1 ? "使用" : "暂停",
                                 Author = pro.Author,
                                 Imgpath = FileSerivce.GetImgUrl(pro.Imgpath.ConvertTo<int>(0)),
                                 Keyword = actdata.GetWxMaterialKeyWordStrByMaterialId(pro.MaterialId),

                                 Summary = pro.Summary,
                                 Articleurl = pro.Articleurl,

                                 Phone = pro.Phone,
                                 Price = pro.Price,
                                 PromoteTypeId = pro.SalePromoteTypeid
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string MaterialSort(string materialids)
        {
            if (materialids == "")
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "没有排序的元素" });
            }
            else
            {
                string[] str = materialids.Split(',');

                string err = "";
                for (int i = 1; i <= str.Length; i++)
                {
                    string materialid = str[i - 1];
                    int sortid = i;
                    int sortmenu = new WxMaterialData().SortMaterial(materialid, sortid);
                    if (sortmenu == 0)
                    {
                        err += materialid + "err;";

                    }
                }
                if (err == "")
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = "菜单排序成功" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = err });
                }
            }
        }



        public static string Wxmaterialtypepagelist(int pageindex, int pagesize, int comid)
        {
            var totalcount = 0;
            try
            {

                var actdata = new WxSalePromoteTypeData();
                var list = actdata.Wxmaterialtypepagelist(pageindex, pagesize, comid, out totalcount);

                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 TypeName = pro.Typename,
                                 TypeClass = pro.Typeclass,
                                 Isshowpast = pro.Isshowpast

                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string GetMaterialType(int id, int comid)
        {
            try
            {
                WxSalePromoteType wxmenu = new WxSalePromoteTypeData().GetMaterialType(id, comid);
                if (wxmenu != null)
                {

                    return JsonConvert.SerializeObject(new { type = 100, msg = wxmenu });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "数据库不存在记录" });
                }
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }

        public static string EditMaterialType(int id, string typename, string typeclass, int comid, bool isshowpast)
        {
            try
            {

                int delmenu = new WxMenuData().EditMaterialType(id, typename, typeclass, comid, isshowpast);


                return JsonConvert.SerializeObject(new { type = 100, msg = delmenu });

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string GetAllWxMaterialType(int comid)
        {
            var totalcount = 0;
            try
            {

                var actdata = new WxSalePromoteTypeData();
                var list = actdata.GetAllWxMaterialType(comid, out totalcount);

                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 TypeName = pro.Typename,
                                 TypeClass = pro.Typeclass

                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string GetWxBasicByComId(int comid)
        {
            try
            {
                WeiXinBasic wxbasic = new WeiXinBasicData().GetWxBasicByComId(comid);
                return JsonConvert.SerializeObject(new { type = 100, msg = wxbasic });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string Editwxbasic(WeiXinBasic basic)
        {
            try
            {

                int edit = new WeiXinBasicData().Editwxbasic(basic);


                return JsonConvert.SerializeObject(new { type = 100, msg = edit });

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string Editperiod(periodical model)
        {
            try
            {



                int id = new WxMaterialData().Editperiod(model);

                model.Id = id;
                return JsonConvert.SerializeObject(new { type = 100, msg = model });

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }



        public static string GetWxQi(int comid, int salepromotetypeid)
        {
            try
            {
                periodical model = new WxMaterialData().GetPeriodicalBySaleType(comid, salepromotetypeid);


                return JsonConvert.SerializeObject(new { type = 100, msg = model });

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


        public static string GetMemberShipCardMaterial(int comid, int materialid)
        {
            try
            {
                MemberShipCardMaterial wxmaterial = new MemberShipCardMaterialData().GetMembershipcardMaterial(comid, materialid);
                if (wxmaterial != null)
                {

                    return JsonConvert.SerializeObject(new { type = 100, msg = wxmaterial });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }

        public static string EditMemberShipCardMaterial(MemberShipCardMaterial model)
        {
            try
            {

                int edit = new MemberShipCardMaterialData().EditMaterial(model);


                return JsonConvert.SerializeObject(new { type = 100, msg = edit });

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }



        public static string Membershipcardpagelist(int comid, int pageindex, int pagesize, bool applystate)
        {

            var totalcount = 0;
            try
            {

                var actdata = new MemberShipCardMaterialData();
                var list = actdata.Membershipcardpagelist(comid, pageindex, pagesize, applystate, out totalcount);

                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 MaterialId = pro.MaterialId,
                                 Title = pro.Title,
                                 Applystate = pro.Applystate == true ? "使用" : "暂停",

                                 Imgpath = FileSerivce.GetImgUrl(pro.Imgpath.ConvertTo<int>(0)),

                                 Summary = pro.Summary,


                                 Phone = pro.Phone,
                                 Price = pro.Price,

                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        public static string AllMembershipcardpagelist(int comid, int pageindex, int pagesize)
        {

            var totalcount = 0;
            try
            {

                var actdata = new MemberShipCardMaterialData();
                var list = actdata.AllMembershipcardpagelist(comid, pageindex, pagesize, out totalcount);

                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 MaterialId = pro.MaterialId,
                                 Title = pro.Title,
                                 Applystate = pro.Applystate == true ? "使用" : "暂停",

                                 Imgpath = FileSerivce.GetImgUrl(pro.Imgpath.ConvertTo<int>(0)),

                                 Summary = pro.Summary,

                                 Phone = pro.Phone,
                                 Price = pro.Price,
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        public static string DelMemberShipCardMaterial(int materialid)
        {
            try
            {
                int delmaterial = new MemberShipCardMaterialData().DelMemberShipCardMaterial(materialid);

                return JsonConvert.SerializeObject(new { type = 100, msg = delmaterial });

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }

        }
        public static string MemberShipCardMaterialSort(string materialids)
        {
            if (materialids == "")
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "没有排序的元素" });
            }
            else
            {
                string[] str = materialids.Split(',');

                string err = "";
                for (int i = 1; i <= str.Length; i++)
                {
                    string materialid = str[i - 1];
                    int sortid = i;
                    int sortmenu = new MemberShipCardMaterialData().SortMaterial(materialid, sortid);
                    if (sortmenu == 0)
                    {
                        err += materialid + "err;";

                    }
                }
                if (err == "")
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = "菜单排序成功" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = err });
                }
            }
        }

        #region 网页授权获取用户信息，引导用户打开的链接地址
        public static string GetFollowOpenLink(int comid, string redirect_uri)
        {

            string followlink = "https://open.weixin.qq.com/connect/oauth2/authorize?";

            //根据comid得到多微信商户基本信息
            WeiXinBasic basic = new WeiXinBasicData().GetWxBasicByComId(comid);

            if (basic == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "商家尚未设置微信接口" });
            }

            string appid = basic.AppId;
            //string redirect_uri = "http://shop" + comid + ".etown.cn/m/indexcard.aspx";
            string response_type = "code";
            string scope = "snsapi_base";//（不弹出授权页面，直接跳转，只能获取用户openid）
            string state = "123";
            followlink += "appid=" + appid + "&redirect_uri=" + redirect_uri + "&response_type=" + response_type + "&scope=" + scope + "&state=" + state + "#wechat_redirect";

            return JsonConvert.SerializeObject(new { type = 100, msg = followlink });
        }
        public static string GetFollowOpenLinkUrl(int comid, string redirect_uri)
        {

            string followlink = "https://open.weixin.qq.com/connect/oauth2/authorize?";

            //根据comid得到多微信商户基本信息
            WeiXinBasic basic = new WeiXinBasicData().GetWxBasicByComId(comid);

            if (basic == null)
            {
                return "";
            }


            string appid = basic.AppId;
            //string redirect_uri = "http://shop" + comid + ".etown.cn/m/indexcard.aspx";
            string response_type = "code";
            string scope = "snsapi_base";//（不弹出授权页面，直接跳转，只能获取用户openid）
            string state = "123";
            followlink += "appid=" + appid + "&redirect_uri=" + redirect_uri + "&response_type=" + response_type + "&scope=" + scope + "&state=" + state + "#wechat_redirect";

            return followlink;
        }
        #endregion

        #region  网页授权获取用户信息，引导用户打开的链接地址(和支付有关，如果商户没有微信支付，则用易城商户的微信支付)
        public static string GetFollowOpenLinkAboutPay(int comid, string redirect_uri)
        {



            string followlink = "https://open.weixin.qq.com/connect/oauth2/authorize?";

            //根据comid得到多微信商户基本信息
            WeiXinBasic basic = new WeiXinBasicData().GetWxBasicByComId(comid);

            //if (basic == null)
            //{
            //    return JsonConvert.SerializeObject(new { type = 1, msg = "商家尚未开通微信支付,请选择其他支付方式" });
            //}

            bool issetfinancepaytype = false;
            //根据产品判断商家是否含有自己的微信支付:a.含有的话支付到商家；b.没有的话支付到平台的微信公众号账户中
            B2b_finance_paytype model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(comid);
            if (model != null)
            {
                //商家微信支付的所有参数都存在
                if (model.Wx_appid != "" && model.Wx_appkey != "" && model.Wx_partnerid != "" && model.Wx_paysignkey != "")
                {
                    //appId = model.Wx_appid;
                    //appsecret = model.Wx_appkey;
                    //appkey = model.Wx_paysignkey;
                    //mchid = model.Wx_partnerid;
                    //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "d");
                    issetfinancepaytype = true;
                }
            }

            //如果商户没有开通微信支付的话，统一用易城微信支付
            if (issetfinancepaytype == false)
            {
                basic = new WeiXinBasicData().GetWxBasicByComId(106);
                redirect_uri = redirect_uri.Replace(comid.ToString(), "106");
            }




            string appid = basic.AppId;
            //string redirect_uri = "http://shop" + comid + ".etown.cn/m/indexcard.aspx";
            string response_type = "code";
            string scope = "snsapi_base";//（不弹出授权页面，直接跳转，只能获取用户openid）
            string state = "123";
            followlink += "appid=" + appid + "&redirect_uri=" + redirect_uri + "&response_type=" + response_type + "&scope=" + scope + "&state=" + state + "#wechat_redirect";

            return JsonConvert.SerializeObject(new { type = 100, msg = followlink });
        }
        public static string GetFollowOpenLinkUrlAboutPay(int comid, string redirect_uri)
        {

            string followlink = "https://open.weixin.qq.com/connect/oauth2/authorize?";

            //根据comid得到多微信商户基本信息
            WeiXinBasic basic = new WeiXinBasicData().GetWxBasicByComId(comid);

            //if (basic == null)
            //{
            //    return JsonConvert.SerializeObject(new { type = 1, msg = "商家尚未开通微信支付,请选择其他支付方式" });
            //}

            bool issetfinancepaytype = false;
            //根据产品判断商家是否含有自己的微信支付:a.含有的话支付到商家；b.没有的话支付到平台的微信公众号账户中
            B2b_finance_paytype model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(comid);
            if (model != null)
            {
                //商家微信支付的所有参数都存在
                if (model.Wx_appid != "" && model.Wx_appkey != "" && model.Wx_partnerid != "" && model.Wx_paysignkey != "")
                {
                    //appId = model.Wx_appid;
                    //appsecret = model.Wx_appkey;
                    //appkey = model.Wx_paysignkey;
                    //mchid = model.Wx_partnerid;
                    //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "d");
                    issetfinancepaytype = true;
                }
            }

            //如果商户没有开通微信支付的话，统一用易城微信支付
            if (issetfinancepaytype == false)
            {
                basic = new WeiXinBasicData().GetWxBasicByComId(106);
                redirect_uri = redirect_uri.Replace(comid.ToString(), "106");
            }




            string appid = basic.AppId;
            //string redirect_uri = "http://shop" + comid + ".etown.cn/m/indexcard.aspx";
            string response_type = "code";
            string scope = "snsapi_base";//（不弹出授权页面，直接跳转，只能获取用户openid）
            string state = "123";
            followlink += "appid=" + appid + "&redirect_uri=" + redirect_uri + "&response_type=" + response_type + "&scope=" + scope + "&state=" + state + "#wechat_redirect";

            return followlink;
        }
        #endregion



        public static string GetFollowOpenLinkTest(int comid)
        {

            return JsonConvert.SerializeObject(new { type = 100, msg = "" });
        }


        public static string GetWxSendMsgListByComid(int userid, int comid, int pageindex, int pagesize)
        {
            var totalcount = 0;
            try
            {
                var list = new WxRequestXmlData().GetWxSendMsgListByComid(userid, comid, pageindex, pagesize, out totalcount);

                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 FromUserName = pro.FromUserName,
                                 ImageUrl = "",
                                 MsgType = pro.MsgType,
                                 Content = pro.Content,
                                 Recognition = pro.Recognition,
                                 //CreateTime = UnixTimeToTime(pro.CreateTime).ToString("yyyy-MM-dd HH:mm"),
                                 CreateTime = pro.CreateTimeFormat.ToString("yyyy-MM-dd HH:mm"),
                                 ComId = pro.Comid,
                                 WhetherReply = new WxRequestXmlData().JudgeWhetherReply(pro.CreateTimeFormat, pro.FromUserName, pro.Comid),
                                 LasterReplyTime = new WxRequestXmlData().GetLasterReplyTime(pro.CreateTimeFormat, pro.FromUserName, pro.Comid),
                                 KeRenId = pro.Crmid,
                                 KeRenName = pro.Crmname,
                                 KeRenPhone = pro.Crmphone,
                                 WhetherRenZheng = new WxRequestXmlData().JudgeWhetherRenZheng(pro.Comid),
                                 Headimgurl = pro.Headimgurl,
                                 Nickname = pro.Nickname.Replace("?", ""),
                                 City = pro.City,
                                 Province = pro.Province,
                                 Sex = pro.Sex == 0 ? "未知" : pro.Sex == 1 ? "男" : "女",
                                 Crmid = pro.Crmid,
                                 ChannelCompanyName = GetChannelCompany(pro.Crmid, pro.FromUserName),
                                 PicUrl = pro.PicUrl,
                                 Label = pro.Label
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        private static string GetChannelCompany(int crmid, string openid)
        {
            Member_Channel channel = new MemberChannelData().GetChannelByOpenId(openid);
            if (channel == null)
            {
                return "";
            }
            else
            {
                if (channel.Companyid == 0)//搜索公众号关注的
                {
                    return "";
                }
                else
                {
                    Member_Channel_company channelcompany = new MemberChannelcompanyData().GetChannelCompanyByCrmId(crmid);
                    if (channelcompany != null)
                    {
                        return channelcompany.Companyname;
                    }
                    else
                    {
                        return "";
                    }

                }
            }

        }
        /// <summary>

        /// unix时间转换为adatetime

        /// </summary>

        /// <param name="timeStamp"></param>

        /// <returns></returns>

        private static DateTime UnixTimeToTime(string timeStamp)
        {

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            long lTime = long.Parse(timeStamp + "0000000");

            TimeSpan toNow = new TimeSpan(lTime);

            return dtStart.Add(toNow);

        }



        /// <summary>

        /// datetime转换为aunixtime

        /// </summary>

        /// <param name="time"></param>

        /// <returns></returns>

        private static int ConvertDateTimeInt(System.DateTime time)
        {

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

            return (int)(time - startTime).TotalSeconds;

        }




        public static string GetWxSendMsgListByFromUser(int comid, string fromusername, int pageindex, int pagesize)
        {
            var totalcount = 0;
            try
            {
                var list = new WxRequestXmlData().GetWxSendMsgListByFromUser(comid, fromusername, pageindex, pagesize, out totalcount);

                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 FromUserName = pro.FromUserName,
                                 ImageUrl = "",
                                 MsgType = pro.MsgType,
                                 Content = pro.Content,
                                 Recognition = pro.Recognition,
                                 //CreateTime = UnixTimeToTime(pro.CreateTime).ToString("yyyy-MM-dd HH:mm"),
                                 CreateTime = pro.CreateTimeFormat.ToString("yyyy-MM-dd HH:mm"),
                                 ComId = pro.Comid,
                                 WhetherReply = new WxRequestXmlData().JudgeWhetherReply(pro.CreateTimeFormat, pro.FromUserName, pro.Comid),
                                 ContentType = pro.ContentType == true ? 1 : 0,//1:客户发来的信息；0：客服发回的信息
                                 KeRenId = pro.Crmid,
                                 KeRenName = pro.Crmname,
                                 KeRenPhone = pro.Crmphone,
                                 KeFuId = pro.Manageuserid,
                                 KeFuName = pro.Manageusername,
                                 Headimgurl = pro.Headimgurl,
                                 Nickname = pro.Nickname,
                                 City = pro.City,
                                 Province = pro.Province,
                                 Sex = pro.Sex == 0 ? "未知" : pro.Sex == 1 ? "男" : "女",
                                 PicUrl = pro.PicUrl,
                                 Label = pro.Label
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


        public static string Getzixunlog(int comid, int pageindex, int pagesize, string userweixin, string guwenweixin, string key)
        {
            var totalcount = 0;
            try
            {
                var channeldata = new MemberChannelData();
                var list = new WxRequestXmlData().Getzixunlog(comid, pageindex, pagesize, userweixin, guwenweixin, key, out totalcount);

                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             orderby pro.Id
                             select new
                             {
                                 Id = pro.Id,
                                 MsgType = pro.MsgType,
                                 Content = FilterHref(pro.Content),
                                 ChannelImg = FileSerivce.GetImgUrl(channeldata.GetChannelImgbyopenid(pro.FromUserName)),
                                 Recognition = pro.Recognition,
                                 CreateTime = pro.CreateTimeFormat.ToString("yyyy-MM-dd HH:mm"),
                                 Nickname = pro.Nickname == "" ? "匿名" : pro.Nickname.Substring(0, 1) + "**",
                                 City = pro.City,
                                 Province = pro.Province,
                                 Headimgurl = pro.Headimgurl,
                                 guwen = guwenweixin == pro.FromUserName ? 1 : 0,
                                 Mediaid = pro.MediaId,
                                 IsOver2Days = GetIsOver2Days(pro.CreateTimeFormat)
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        private static string FilterHref(string content)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"(?is)</?a\b[^>]*>(?:(?!</?a).)*</a>");
            content = reg.Replace(content, "");
            return content;
        }


        public static string GetWxSendMsgListByTop5(int comid, int pageindex, int pagesize)
        {
            var totalcount = 0;
            try
            {
                var channeldata = new MemberChannelData();
                var list = new WxRequestXmlData().GetWxSendMsgListByTop5(comid, pageindex, pagesize, out totalcount);

                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 MsgType = pro.MsgType,
                                 Content = pro.Content,
                                 ChannelImg = FileSerivce.GetImgUrl(channeldata.GetChannelImgbyopenid(pro.FromUserName)),
                                 Recognition = pro.Recognition,
                                 CreateTime = pro.CreateTimeFormat.ToString("yyyy-MM-dd HH:mm"),
                                 Nickname = pro.Nickname == "" ? "匿名" : pro.Nickname.Substring(0, 1) + "**",
                                 City = pro.City,
                                 Province = pro.Province,
                                 Headimgurl = pro.Headimgurl,
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string SendWxMsg(int comid, string fromusername, int type, string img, string txt)
        {
            B2b_company_manageuser manageuser = UserHelper.CurrentUser();//客服信息（账户表B2b_company_manageuser）

            B2b_crm crm = new B2bCrmData().GetB2bCrmByWeiXin(fromusername);
            if (crm == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "发送客服消息失败" });
            }
            else
            {
                if (crm.Whetherwxfocus == false)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "微信用户已经取消了关注" });
                }
            }


            WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(comid);
            if (basicc != null)
            {
                //获取凭证（首先判断数据库是否存在凭证以及是否过期，如过期，重新获取）
                WXAccessToken token = GetAccessToken(comid, basicc.AppId, basicc.AppSecret);
                //发送文本信息
                string err = "";//返回错误原因

                string createmenuurl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + token.ACCESS_TOKEN;
                string createmenutext1 = "";//微信菜单内容
                if (type == 1)//文本
                {
                    createmenutext1 = "{\"touser\":\"" + fromusername + "\", \"msgtype\":\"text\",\"text\":{\"content\":\"" + txt + "\"}}";
                }


                if (err.Length > 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = err });
                }
                else
                {
                    string createmenuutret = new GetUrlData().HttpPost(createmenuurl, createmenutext1);

                    XmlDocument createselfmenudoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + createmenuutret + "}");

                    XmlElement createselfmenurootElement = createselfmenudoc.DocumentElement;
                    string createerrcode = createselfmenurootElement.SelectSingleNode("errcode").InnerText;
                    if (createerrcode != "0")
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "回复客服信息失败" + createerrcode });
                    }
                    else
                    {
                        //发送客服信息，信息内容录入数据库
                        if (type == 1)//文本
                        {
                            RequestXML retRequestXML = new RequestXML();
                            retRequestXML.PostStr = createmenutext1;
                            retRequestXML.ToUserName = fromusername;
                            retRequestXML.FromUserName = new WeiXinBasicData().GetWxBasicByComId(comid).Weixinno.ConvertTo<string>("");
                            retRequestXML.CreateTime = ConvertDateTimeInt(DateTime.Now).ToString();
                            retRequestXML.MsgType = "text";
                            retRequestXML.Content = txt;
                            retRequestXML.contentType = false;
                            retRequestXML.Comid = comid;

                            retRequestXML.Manageuserid = manageuser.Id;
                            retRequestXML.Manageusername = manageuser.Accounts;


                            int inswxexchangemsg = new WxRequestXmlData().EditWxRequestXmlLog(retRequestXML);
                        }


                        return JsonConvert.SerializeObject(new { type = 100, msg = "回复客服信息成功" });
                    }
                }

            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }


        public static string GwToUserSendWxMsg(int comid, string guwenweixin, string fromusername, int type, string img, string txt, string mediaid)
        {
            B2b_crm crm = new B2bCrmData().GetB2bCrmByWeiXin(fromusername);
            if (crm == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "发送客服消息失败" });
            }
            else
            {
                if (crm.Whetherwxfocus == false)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "微信用户已经取消了关注" });
                }
            }


            WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(comid);
            if (basicc != null)
            {
                //获取凭证（首先判断数据库是否存在凭证以及是否过期，如过期，重新获取）
                WXAccessToken token = GetAccessToken(comid, basicc.AppId, basicc.AppSecret);
                //发送文本信息
                string err = "";//返回错误原因

                string createmenuurl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + token.ACCESS_TOKEN;
                string createmenutext1 = "";//微信菜单内容
                if (type == 1)//文本
                {
                    createmenutext1 = "{\"touser\":\"" + fromusername + "\", \"msgtype\":\"text\",\"text\":{\"content\":\"" + txt + "\"}}";
                }
                if (type == 3)//语音
                {
                    createmenutext1 = "{\"touser\":\"" + fromusername + "\", \"msgtype\":\"voice\",\"voice\":{\"media_id\":\"" + mediaid + "\"}}";
                }


                if (err.Length > 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = err });
                }
                else
                {
                    if (createmenutext1 == "")
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "回复客服信息不支持当前类型" });
                    }
                    string createmenuutret = new GetUrlData().HttpPost(createmenuurl, createmenutext1);

                    XmlDocument createselfmenudoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + createmenuutret + "}");

                    XmlElement createselfmenurootElement = createselfmenudoc.DocumentElement;
                    string createerrcode = createselfmenurootElement.SelectSingleNode("errcode").InnerText;
                    if (createerrcode != "0")
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "回复客服信息失败" + createerrcode });
                    }
                    else
                    {
                        //发送客服信息，信息内容录入数据库
                        if (type == 1)//文本
                        {
                            RequestXML retRequestXML = new RequestXML();
                            retRequestXML.PostStr = createmenutext1;
                            retRequestXML.ToUserName = fromusername;
                            retRequestXML.FromUserName = guwenweixin;
                            retRequestXML.CreateTime = ConvertDateTimeInt(DateTime.Now).ToString();
                            retRequestXML.MsgType = "text";
                            retRequestXML.Content = txt;
                            retRequestXML.contentType = false;
                            retRequestXML.Comid = comid;

                            retRequestXML.Manageuserid = 0;
                            retRequestXML.Manageusername = "";


                            int inswxexchangemsg = new WxRequestXmlData().EditWxRequestXmlLog(retRequestXML);
                        }
                        if (type == 3)//语音
                        {
                            RequestXML retRequestXML = new RequestXML();
                            retRequestXML.PostStr = createmenutext1;
                            retRequestXML.ToUserName = fromusername;
                            retRequestXML.FromUserName = guwenweixin;
                            retRequestXML.CreateTime = ConvertDateTimeInt(DateTime.Now).ToString();
                            retRequestXML.MsgType = "voice";
                            retRequestXML.Content = "";
                            retRequestXML.contentType = false;
                            retRequestXML.Comid = comid;
                            retRequestXML.MediaId = mediaid;

                            retRequestXML.Manageuserid = 0;
                            retRequestXML.Manageusername = "";


                            int inswxexchangemsg = new WxRequestXmlData().EditWxRequestXmlLog(retRequestXML);
                        }


                        return JsonConvert.SerializeObject(new { type = 100, msg = "回复客服信息成功" });
                    }
                }

            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }



        public static string HandleQrCodeCreateStatus(int channelcompanyid, int activityid, string checkstatus)
        {
            try
            {
                int result = 0;
                if (channelcompanyid != 0)
                {
                    result = new MemberChannelData().HandleQrCodeCreateStatus(channelcompanyid, checkstatus);

                }
                else
                {
                    result = new MemberActivityData().HandleQrCodeCreateStatus(activityid, checkstatus);
                }
                if (result > 0)
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = "" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }

            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }



        public static string GetWxSubscribeList(int comid, int subscribesourceid, int pageindex, int pagesize)
        {
            var totalcount = 0;
            try
            {
                List<WxSubscribeDetail> list = new WxSubscribeDetailData().GetWxSubscribeList(comid, subscribesourceid, pageindex, pagesize, out totalcount);

                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 OpenId = pro.Openid,
                                 SubscribeTime = pro.Subscribetime,
                                 Event = pro.Eevent,
                                 EventKey = pro.Eventkey,
                                 ChannelCompanyName = pro.Channelcompanyid == 0 ? "" : new MemberChannelData().GetChannelCompanyById(pro.Channelcompanyid).Companyname,
                                 ActivityName = pro.Activityid == 0 ? "" : new MemberActivityData().GetMemberActivityById(pro.Activityid).Title,
                                 BindPhone = GetBindPhone(pro.Openid, pro.Comid),
                                 Sex = pro.Sex == 0 ? "未知" : pro.Sex == 1 ? "男" : "女",
                                 City = pro.City,
                                 Nickname = pro.Nickname,
                                 ChannelName = GetChannelName(pro.Subscribesourceid),
                                 ProductName = GetProductName(pro.Subscribesourceid)

                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        private static string GetProductName(int subscribesourceid)
        {
            WxSubscribeSource source = new WxSubscribeSourceData().GetWXSourceById(subscribesourceid);
            if (source != null)
            {
                if (source.Productid == 0)
                {
                    return "";
                }
                else
                {
                    B2b_com_pro model = new B2bComProData().GetProById(source.Productid.ToString());
                    if (model == null)
                    {
                        return "";
                    }
                    else
                    {
                        return model.Pro_name;
                    }
                }
            }
            else
            {
                return "";
            }
        }

        private static string GetChannelName(int subscribesourceid)
        {
            WxSubscribeSource source = new WxSubscribeSourceData().GetWXSourceById(subscribesourceid);
            if (source != null)
            {
                if (source.Channelid == 0)
                {
                    return "";
                }
                else
                {
                    Member_Channel channel = new MemberChannelData().GetChannelDetail(source.Channelid);
                    if (channel == null)
                    {
                        return "";
                    }
                    else
                    {
                        return channel.Name;
                    }
                }
            }
            else
            {
                return "";
            }
        }

        private static string GetBindPhone(string openid, int comid)
        {
            B2b_crm model = new B2bCrmData().GetB2bCrm(openid, comid);
            if (model == null)
            {
                return "未绑定";
            }
            else
            {
                return model.Phone;
            }
        }
        public static string GetWXSourcelist(int comid, int wxsourcetype, int pageindex, int pagesize)
        {
            var totalcount = 0;
            try
            {
                List<WxSubscribeSource> list = new WxSubscribeSourceData().GetWXSourcelist(comid, wxsourcetype, pageindex, pagesize, out totalcount);

                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 SourceType = pro.Sourcetype,
                                 ChannelCompanyId = pro.Channelcompanyid,
                                 ChannelCompanyName = pro.Channelcompanyid == 0 ? "" : new MemberChannelData().GetChannelCompanyById(pro.Channelcompanyid).Companyname,
                                 ActivityId = pro.Activityid,
                                 ActivityName = pro.Activityid == 0 ? "" : new MemberActivityData().GetMemberActivityById(pro.Activityid).Title,
                                 ComId = pro.Comid,
                                 Whethercreateqrcode = JudgeWhethercreateqrcode(pro.Sourcetype, pro.Channelcompanyid, pro.Activityid)
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        public static string SeledWXSourcelist(int comid, int wxsourcetype, int pageindex, int pagesize, string onlinestatus = "1")
        {
            var totalcount = 0;
            try
            {
                List<WxSubscribeSource> list = new WxSubscribeSourceData().SeledWXSourcelist(comid, wxsourcetype, pageindex, pagesize, out totalcount, onlinestatus);

                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 SourceType = pro.Sourcetype,
                                 ChannelCompanyId = pro.Channelcompanyid,
                                 ChannelCompanyName = pro.Channelcompanyid == 0 ? "" : new MemberChannelData().GetChannelCompanyById(pro.Channelcompanyid).Companyname,
                                 ActivityId = pro.Activityid,
                                 ActivityName = pro.Activityid == 0 ? "" : new MemberActivityData().GetMemberActivityById(pro.Activityid).Title,
                                 ComId = pro.Comid,
                                 //Whethercreateqrcode = JudgeWhethercreateqrcode(pro.Sourcetype, pro.Channelcompanyid, pro.Activityid),
                                 SubscribeTotalCount = new WxSubscribeDetailData().GetSubScribeTotalCountBySubScribeId(pro.Id),
                                 Title = pro.Title,
                                 QrcodeUrl = pro.Qrcodeurl,
                                 CreateTime = pro.Createtime.ToString("yyyy-MM-dd").ConvertTo<DateTime>(),
                                 ScanTotal = new WxSubscribeDetailData().GetScanTotalCount(pro.Id),
                                 LastScanTime = new WxSubscribeDetailData().GetLasterScanTime(pro.Id).ConvertTo<DateTime>(DateTime.Parse("1970-01-01")).ToString("yyyy-MM-dd"),
                                 Onlinestatus = pro.Onlinestatus
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        private static bool JudgeWhethercreateqrcode(int sourcetype, int Channelcompanyid, int Activityid)
        {

            if (sourcetype == 1)//活动
            {

                return new MemberActivityData().GetMemberActivityById(Activityid).Whethercreateqrcode;
            }
            else //渠道公司
            {
                return new MemberChannelData().GetChannelCompanyById(Channelcompanyid).Whethercreateqrcode;

            }

        }


        public static string GetPromotionActList(int comid)
        {
            try
            {
                List<Member_Activity> list = new MemberActivityData().GetPromotionActList(comid);
                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }

        public static string GetPromoteChannelCompany(int comid)
        {
            try
            {
                List<Member_Channel_company> list = new MemberChannelcompanyData().GetPromoteChannelCompany(comid);
                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }
        /// <summary>
        /// 编辑微信二维码
        /// </summary>
        /// <param name="qrcodeid"></param>
        /// <param name="comid"></param>
        /// <param name="qrcodename"></param>
        /// <param name="promoteact"></param>
        /// <param name="promotechannelcompany"></param>
        /// <param name="channelid"></param>
        /// <param name="onlinestatus"></param>
        /// <returns></returns>
        public static string EditWxQrcode(int qrcodeid, int comid, string qrcodename, int promoteact, int promotechannelcompany, int channelid = 0, int onlinestatus = 1, int productid = 0, int MaterialId = 0)
        {
            try
            {
                WeiXinBasic basic = new WeiXinBasicData().GetWxBasicByComId(comid);
                if (basic == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
                if (basic.Weixintype != 4)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "微信公众号必须是认证公众号" });
                }

                #region 判断二维码来源
                int sourcetype = 0;
                if (promoteact > 0 && promotechannelcompany > 0)
                {
                    sourcetype = 3;//综合推广
                }
                else
                {
                    if (promoteact > 0)
                    {
                        sourcetype = 1;//活动推广
                    }
                    if (promotechannelcompany > 0)
                    {
                        sourcetype = 2;//渠道公司推广
                    }
                }
                #endregion

                #region 判断素材是否生成过非渠道二维码
                if (MaterialId > 0)//素材id>0,则为系统自动生成素材带参二维码
                {
                    //判断素材是否生成过非渠道二维码
                    string sql1 = "select id from  WxSubscribeSource where Wxmaterialid=" + MaterialId + " and channelcompanyid=0 and activityid=0 and channelid=0";
                    using (var cmd1 = new SqlHelper().PrepareTextSqlCommand(sql1))
                    {
                        object o = cmd1.ExecuteScalar();
                        if (o != null)
                        {
                            qrcodeid = int.Parse(o.ToString());
                        }
                    }
                }
                #endregion

                #region  判断产品是否生成过非渠道二维码
                if (productid > 0)//产品id>0,则为系统自动生成产品带参二维码
                {
                    //判断产品是否生成过非渠道二维码
                    string sql1 = "select id from  WxSubscribeSource where productid=" + productid + " and channelcompanyid=0 and activityid=0 and channelid=0";
                    using (var cmd1 = new SqlHelper().PrepareTextSqlCommand(sql1))
                    {
                        object o = cmd1.ExecuteScalar();
                        if (o != null)
                        {
                            qrcodeid = int.Parse(o.ToString());
                        }
                    }
                }
                #endregion

                #region 判断是否生成过非渠道活动二维码
                if (promoteact > 0)
                {
                    string sql1 = "select id from  WxSubscribeSource where channelcompanyid=0 and activityid=" + promoteact + " and channelid=0";
                    using (var cmd1 = new SqlHelper().PrepareTextSqlCommand(sql1))
                    {
                        object o = cmd1.ExecuteScalar();
                        if (o != null)
                        {
                            qrcodeid = int.Parse(o.ToString());
                        }
                    }
                }
                #endregion

                #region 已经注释掉，感觉没用 判断二维码说明是否相同
                //int countt = new WxSubscribeSourceData().WhetherSameExplain(qrcodename, comid);
                //if (countt > 0 && qrcodeid == 0)
                //{
                //    return JsonConvert.SerializeObject(new { type = 1, msg = "二维码说明不能相同", qrcodeurl = "" });
                //}
                #endregion


                #region 添加二维码
                string qrcodeurl = "";//带参二维码地址 
                if (qrcodeid == 0)
                {
                    string sql1 = "insert into WxSubscribeSource(sourcetype,channelcompanyid,activityid,comid,ticket,title,qrcodeurl,createtime,channelid,onlinestatus,productid,Wxmaterialid) values(" + sourcetype + "," + promotechannelcompany + "," + promoteact + "," + comid + ",'','" + qrcodename + "','','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + channelid + "," + onlinestatus + "," + productid + "," + MaterialId + ");select @@IDENTITY";
                    var cmd1 = new SqlHelper().PrepareTextSqlCommand(sql1);

                    object o = cmd1.ExecuteScalar();
                    qrcodeid = int.Parse(o.ToString());


                    if (qrcodeid > 0)
                    {
                        //生成新的二维码图片
                        string ticket = "";//二维码ticket
                        qrcodeurl = GetWxQrCodeUrl(comid, qrcodeid, out ticket);
                        if (qrcodeurl == "")
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "二维码图片生成有误", qrcodeurl = "" });
                        }

                        string sql2 = "update WxSubscribeSource set ticket='" + ticket + "', qrcodeurl='" + qrcodeurl + "' where id=" + qrcodeid;
                        var cmd2 = new SqlHelper().PrepareTextSqlCommand(sql2);

                        int nn = cmd2.ExecuteNonQuery();
                        if (nn == 0)
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "二维码ticket和url修改有误", qrcodeurl = "" });
                        }
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "录入二维码信息出错", qrcodeurl = "" });
                    }

                }
                #endregion
                #region 修改二维码
                else
                {
                    //得到带参二维码地址
                    string sql1 = "select qrcodeurl from  WxSubscribeSource where id=" + qrcodeid;
                    var cmd1 = new SqlHelper().PrepareTextSqlCommand(sql1);

                    object o = cmd1.ExecuteScalar();
                    if (o != null)
                    {
                        qrcodeurl = o.ToString();
                    }


                    //修改二维码基本信息 
                    string sql2 = "update WxSubscribeSource set   title='" + qrcodename + "',sourcetype='" + sourcetype + "',channelcompanyid=" + promotechannelcompany + ",activityid=" + promoteact + ",channelid=" + channelid + ",onlinestatus=" + onlinestatus + ",productid=" + productid + ",Wxmaterialid=" + MaterialId + "   where id=" + qrcodeid;
                    var cmd2 = new SqlHelper().PrepareTextSqlCommand(sql2);

                    cmd2.ExecuteNonQuery();

                }
                #endregion
                return JsonConvert.SerializeObject(new { type = 100, msg = "", qrcodeurl = qrcodeurl });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }
        public static string GetWxTempQrCodeUrl(int comid, int tempqrcodeid)
        {
            //根据公司id得到开发者凭据
            WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(comid);

            //获取凭证（首先判断数据库是否存在凭证以及是否过期，如过期，重新获取）
            WXAccessToken token = GetAccessToken(comid, basicc.AppId, basicc.AppSecret);

            string createmenuurl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + token.ACCESS_TOKEN;
            string createmenutext1 = "";//微信菜单内容
            createmenutext1 = "{\"expire_seconds\": 604800,\"action_name\": \"QR_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + tempqrcodeid + "}}}";

            string createmenuutret = new GetUrlData().HttpPost(createmenuurl, createmenutext1);
            XmlDocument createselfmenudoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + createmenuutret + "}");

            XmlElement createselfmenurootElement = createselfmenudoc.DocumentElement;
            XmlNode createerrcode = createselfmenurootElement.SelectSingleNode("errcode");

            if (createerrcode == null)
            {
                string ticket = createselfmenurootElement.SelectSingleNode("ticket").InnerText;

                string geturl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + System.Web.HttpContext.Current.Server.UrlEncode(ticket);

                return geturl;
            }
            else
            {

                return "";
            }
        }
        public static string GetWxQrCodeUrl(int comid, int wxsubscribesourceid, out string ticket)
        {
            //根据公司id得到开发者凭据
            WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(comid);

            //获取凭证（首先判断数据库是否存在凭证以及是否过期，如过期，重新获取）
            WXAccessToken token = GetAccessToken(comid, basicc.AppId, basicc.AppSecret);

            string createmenuurl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + token.ACCESS_TOKEN;
            string createmenutext1 = "";//微信菜单内容
            createmenutext1 = "{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + wxsubscribesourceid + "}}}";

            string createmenuutret = new GetUrlData().HttpPost(createmenuurl, createmenutext1);
            XmlDocument createselfmenudoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + createmenuutret + "}");

            XmlElement createselfmenurootElement = createselfmenudoc.DocumentElement;
            XmlNode createerrcode = createselfmenurootElement.SelectSingleNode("errcode");

            if (createerrcode == null)
            {
                ticket = createselfmenurootElement.SelectSingleNode("ticket").InnerText;

                string geturl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + System.Web.HttpContext.Current.Server.UrlEncode(ticket);

                return geturl;
            }
            else
            {
                ticket = "";
                return "";
            }
        }
        public static string GetWxQrCodeUrl(int comid, int wxsubscribesourceid)
        {
            //根据公司id得到开发者凭据
            WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(comid);

            //获取凭证（首先判断数据库是否存在凭证以及是否过期，如过期，重新获取）
            WXAccessToken token = GetAccessToken(comid, basicc.AppId, basicc.AppSecret);

            string createmenuurl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + token.ACCESS_TOKEN;
            string createmenutext1 = "";//微信菜单内容
            createmenutext1 = "{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + wxsubscribesourceid + "}}}";

            string createmenuutret = new GetUrlData().HttpPost(createmenuurl, createmenutext1);
            XmlDocument createselfmenudoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + createmenuutret + "}");

            XmlElement createselfmenurootElement = createselfmenudoc.DocumentElement;
            XmlNode createerrcode = createselfmenurootElement.SelectSingleNode("errcode");

            if (createerrcode == null)
            {
                string ticket = createselfmenurootElement.SelectSingleNode("ticket").InnerText;

                string geturl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + System.Web.HttpContext.Current.Server.UrlEncode(ticket);

                return JsonConvert.SerializeObject(new { type = 100, msg = geturl });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }

        public static string Getwxqrcode(int qrcodeid)
        {
            try
            {
                WxSubscribeSource model = new WxSubscribeSourceData().Getwxqrcode(qrcodeid);
                return JsonConvert.SerializeObject(new { type = 100, msg = model });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }

        public static string WxWhetherVertify(int comid)
        {
            try
            {
                bool b = new WxRequestXmlData().JudgeWhetherRenZheng(comid);
                return JsonConvert.SerializeObject(new { type = 100, msg = b });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }

        public static string AddNextPeriod(int comid, int Wxsaletypeid, int percal)
        {
            try
            {
                //首先判断当前期是否含有文章:没有文章，不可添加下一期
                int count = new WxMaterialData().GetWxMaterialCountByPercal(comid, Wxsaletypeid, percal);
                if (count > 0)
                {
                    periodical model = new periodical
                    {
                        Id = 0,
                        Comid = comid,
                        Percal = percal + 1,
                        Perinfo = "",
                        Peryear = DateTime.Now.Year,
                        Uptime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        Wxsaletypeid1 = Wxsaletypeid
                    };

                    int id = new WxMaterialData().Editperiod(model);

                    model.Id = id;
                    return JsonConvert.SerializeObject(new { type = 100, msg = model });

                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "当前期还没有任何文章，不可添加下一期!" });
                }
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }
        public static string IsCanAddperiod(int comid, int Wxsaletypeid, int percal)
        {
            try
            {
                //首先判断当前期是否含有文章:没有文章，不可添加下一期
                int count = new WxMaterialData().GetWxMaterialCountByPercal(comid, Wxsaletypeid, percal);
                if (count > 0)
                {

                    int Percal = percal + 1;

                    int Peryear = DateTime.Now.Year;


                    return JsonConvert.SerializeObject(new { type = 100, Percal = Percal, Peryear = Peryear });

                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "当前期还没有任何文章，不可添加下一期!" });
                }
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }
        public static string AdjustQrcodeStatus(int sourceid, int onlinestatus)
        {
            try
            {
                int b = new WxRequestXmlData().AdjustQrcodeStatus(sourceid, onlinestatus);
                return JsonConvert.SerializeObject(new { type = 100, msg = b });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }

        /// <summary>
        /// 得到渠道单位列表
        /// </summary>
        /// <param name="comid"></param>
        /// <param name="channeltype"></param>
        /// <returns></returns>
        public static string GetChannelList(int comid, int channeltype = 100)
        {
            try
            {
                List<Member_Channel_company> list = new MemberChannelcompanyData().GetChannelList(comid, channeltype);
                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }

        public static string GetChannelCompanyList(int comid, string issuetype, string companystate, string whetherdepartment, int channelcompanyid = 0)
        {
            try
            {
                List<Member_Channel_company> list = new MemberChannelcompanyData().GetChannelCompanyList(comid, issuetype, companystate, whetherdepartment, channelcompanyid);
                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }

        public static string GetActivityList(int comid, string runstate, string whetherexpired)
        {
            try
            {
                List<Member_Activity> list = new MemberActivityData().GetActivityList(comid, runstate, whetherexpired);
                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }

        public static string GetWXSourcelist2(int comid, string wxsourcetype, int pageindex, int pagesize, string onlinestatus)
        {
            var totalcount = 0;
            try
            {
                List<WxSubscribeSource> list = new WxSubscribeSourceData().GetWXSourcelist2(comid, wxsourcetype, pageindex, pagesize, onlinestatus, out totalcount);

                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 SourceType = pro.Sourcetype,
                                 ChannelCompanyId = pro.Channelcompanyid,
                                 ChannelCompanyName = pro.Channelcompanyid == 0 ? "" : new MemberChannelData().GetChannelCompanyById(pro.Channelcompanyid).Companyname,
                                 ActivityId = pro.Activityid,
                                 ActivityName = pro.Activityid == 0 ? "" : new MemberActivityData().GetMemberActivityById(pro.Activityid).Title,
                                 ComId = pro.Comid,
                                 SubscribeTotalCount = new WxSubscribeDetailData().GetSubScribeTotalCountBySubScribeId(pro.Id),
                                 Title = pro.Title,
                                 QrcodeUrl = pro.Qrcodeurl,
                                 CreateTime = pro.Createtime.ToString("yyyy-MM-dd").ConvertTo<DateTime>(),
                                 ScanTotal = new WxSubscribeDetailData().GetScanTotalCount(pro.Id),
                                 LastScanTime = new WxSubscribeDetailData().GetLasterScanTime(pro.Id),
                                 Onlinestatus = pro.Onlinestatus
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        /// <summary>
        /// 得到渠道列表
        /// </summary>
        /// <param name="channelcomid"></param>
        /// <param name="runstate"></param>
        /// <param name="whetherdefaultchannel"></param>
        /// <returns></returns>
        public static string GetChannelList(int channelcomid, string runstate, string whetherdefaultchannel)
        {
            try
            {
                int totalcount = 0;
                List<Member_Channel> list = new MemberChannelData().GetChannelList(channelcomid, runstate, whetherdefaultchannel, out totalcount);
                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }
        /// <summary>
        /// 微信群发
        /// </summary>
        /// <param name="weixinnos"></param>
        /// <param name="content"></param>
        /// <param name="msgtype"></param>
        /// <param name="userid"></param>
        /// <param name="comid"></param>
        /// <returns></returns>
        public static string Wxqunfa(string weixins, string content, string msgtype, string media_id, int userid, int comid, int channelcompanyid1 = 0)
        {

            Wxqunfa_log log = null;

            if (msgtype == "text")
            {
                if (content == "")
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "接收参数失败" });
                }
            }

            if (weixins == "" || userid == 0 || comid == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "接收参数失败" });
            }
            else
            {
                #region 录入群发日志
                //根据userid得到员工信息：a.公司员工 b.门店员工
                B2b_company_manageuser userinfo = new B2bCompanyManagerUserData().GetCompanyUser(userid);
                if (userinfo != null)
                {
                    //录入群发日志表
                    log = new Wxqunfa_log()
                    {
                        Id = 0,
                        Msgtype = msgtype,
                        Media_id = media_id,
                        Content = content,
                        Sendtime = DateTime.Now,
                        Errcode = 99999,//默认群发失败
                        Errmsg = "",
                        Msg_id = "0",
                        Userid = userid,
                        Channelcompanyid = channelcompanyid1,//发送给的门市id
                        Comid = comid,
                        Yearmonth = DateTime.Now.ToString("yyyyMM"),
                        Yearmonthday = DateTime.Now.ToString("yyyyMMdd"),
                        Weixins = weixins
                    };
                    int qunfalogid = new Wxqunfa_logData().EditLog(log);
                    log.Id = qunfalogid;
                    //向微信群发日志子表中录入数据
                    string[] arr = weixins.Split(',');
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (arr[i] != "")
                        {
                            new Wxqunfa_wxnoData().InsWxno(qunfalogid, arr[i]);
                        }
                    }


                    if (userinfo.Channelcompanyid > 0)//门店经理，每月发送限制4次
                    {
                        string yearmonth = DateTime.Now.ToString("yyyyMM");
                        //得到当月门店发送的次数
                        int yearmonthnum = new Wxqunfa_logData().GetSendNum(comid, int.Parse(userinfo.Channelcompanyid.ToString()), yearmonth);
                        if (yearmonthnum < 5)
                        { }
                        else
                        {
                            log.Errmsg = "门店发送当月发送次数已达最大次数4次";
                            new Wxqunfa_logData().EditLog(log);

                            return JsonConvert.SerializeObject(new { type = 1, msg = "门店发送当月发送次数已达最大次数4次" });
                        }
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "获取员工信息失败" });
                }
                #endregion
            }

            string token = "";
            #region 获取开发者凭据
            //根据公司id得到开发者凭据
            WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(comid);
            if (basicc != null)
            {
                //获取凭证（首先判断数据库是否存在凭证以及是否过期，如过期，重新获取）
                WXAccessToken model = GetAccessToken(comid, basicc.AppId, basicc.AppSecret);
                token = model.ACCESS_TOKEN;
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "获取开发者凭据失败" });
            }
            #endregion

            #region openid列表字符串
            string[] arrr = weixins.Split(',');

            string weixinstr = "";
            for (int i = 0; i < arrr.Length; i++)
            {
                if (arrr[i] != "")
                {
                    weixinstr += "\"" + arrr[i] + "\",";
                }
            }
            weixinstr = weixinstr.Substring(0, weixinstr.Length - 1);
            #endregion
            string url = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token=" + token;
            string param = "";
            if (msgtype == "text")//群发文本信息
            {
                param = "{" +
                                 "\"touser\": [" + weixinstr + "], \"msgtype\": \"text\", \"text\": { \"content\": \"" + content + "\"}" +
                              "}";
            }
            else if (msgtype == "news")//群发图文信息
            {
                if (media_id == "")
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "请选择群发图文消息" });
                }
                param = "{\"touser\":[" + weixinstr + "],\"mpnews\":{\"media_id\":\"" + media_id + "\"},\"msgtype\":\"mpnews\"}";
            }
            else//其他消息类型 
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "暂时只支持群发纯文本信息和图文消息" });
            }

            #region 发送post请求
            string ret = new GetUrlData().HttpPost(url, param);

            XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + ret + "}");
            XmlElement root = doc.DocumentElement;
            string errcode = root.SelectSingleNode("errcode").InnerText;
            if (errcode != "0")
            {
                log.Errcode = int.Parse(errcode);
                log.Errmsg = root.SelectSingleNode("errmsg").InnerText;
                new Wxqunfa_logData().EditLog(log);

                return JsonConvert.SerializeObject(new { type = 1, msg = "群发失败" + errcode });
            }
            else
            {
                log.Errcode = int.Parse(errcode);
                log.Errmsg = root.SelectSingleNode("errmsg").InnerText;
                log.Msg_id = root.SelectSingleNode("msg_id").InnerText;
                new Wxqunfa_logData().EditLog(log);

                return JsonConvert.SerializeObject(new { type = 100, msg = "群发成功" });
            }
            #endregion
        }

        public static string Getqunfalist(int comid, int userid, int pageindex, int pagesize)
        {
            if (comid == 0 || userid == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "参数传递失败" });
            }
            else
            {
                int totalcount = 0;
                List<Wxqunfa_log> list = new Wxqunfa_logData().GetQunfalist(comid, userid, pageindex, pagesize, out totalcount);
                IEnumerable result = "";
                if (list != null)
                {
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Content = pro.Content,
                                 SendResult = pro.Errcode == 0 ? "发送成功" : "发送失败",
                                 SendTime = pro.Sendtime.ToString("yyyyMMdd HH:mm:ss"),
                                 Sender = pro.Channelcompanyid == 0 ? new B2bCompanyData().GetCompanyNameById(comid) + new B2bCompanyManagerUserData().GetCompanyUser(userid).Employeename : new MemberChannelcompanyData().GetChannelCompanyNameById(pro.Channelcompanyid) + new B2bCompanyManagerUserData().GetCompanyUser(userid).Employeename,
                                 Sendobj = pro.Channelcompanyid == 0 ? new B2bCompanyData().GetCompanyNameById(comid) : new MemberChannelcompanyData().GetChannelCompanyNameById(pro.Channelcompanyid)
                             };
                }
                return JsonConvert.SerializeObject(new { type = 100, msg = result, totalcount = totalcount });
            }

        }

        public static string Getinvitecodesendlog(int comid, int userid, int pageindex, int pagesize)
        {
            if (comid == 0 || userid == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "参数传递失败" });
            }
            else
            {
                int totalcount = 0;
                List<B2b_invitecodesendlog> list = new B2b_invitecodesendlogData().Getinvitecodesendlog(comid, userid, pageindex, pagesize, out totalcount);
                IEnumerable result = "";
                if (list != null)
                {
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Phone = pro.Phone,
                                 Smscontent = pro.Smscontent,
                                 Invitecode = pro.Invitecode,
                                 Senduserid = pro.Senduserid,
                                 Sendtime = pro.Sendtime.ToString("yyyyMMdd HH:mm:ss"),
                                 Issendsuc = pro.Issendsuc == 1 ? "成功" : "失败",
                                 Isqunfa = pro.Isqunfa == 1 ? "群发" : "单发",
                                 Remark = pro.Remark,
                                 Comid = pro.Comid

                             };
                }
                return JsonConvert.SerializeObject(new { type = 100, msg = result, totalcount = totalcount });
            }
        }

        public static string Editwxbasicstep1(int wxbasicid, int weixintype, int comid, string domain, string url, string token)
        {
            int result = new WeiXinBasicData().Editwxbasicstep1(wxbasicid, weixintype, comid, domain, url, token);

            if (result > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = result });
            }
        }

        public static string Editwxbasicstep2(int wxbasicid, string appid, string appsecret)
        {
            int result = new WeiXinBasicData().Editwxbasicstep2(wxbasicid, appid, appsecret);

            if (result > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = result });
            }
        }

        public static string Editwxbasicreply(int wxbasicid, string leavemsgreply, string attentionreply)
        {
            int result = new WeiXinBasicData().Editwxbasicreply(wxbasicid, leavemsgreply, attentionreply);

            if (result > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = result });
            }
        }

        public static string Editwxbasicstep(int wxbasicid, string appid, string appsecret, int weixintype)
        {
            int result = new WeiXinBasicData().Editwxbasicstep(wxbasicid, appid, appsecret, weixintype);

            if (result > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = result });
            }
        }

        public static string Delmaterialtype(int typeid, int comid)
        {



            int result = new WxMaterialData().Delmaterialtype(typeid, comid);

            if (result > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = result });
            }
        }

        public static string Getwxkfpagelist(int pageindex, int pagesize, int comid, int userid, string isrun = "0,1", string key = "")
        {
            //根据公司id得到开发者凭据
            WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(comid);

            //获取凭证（首先判断数据库是否存在凭证以及是否过期，如过期，重新获取）
            WXAccessToken token = GetAccessToken(comid, basicc.AppId, basicc.AppSecret);

            string url = "https://api.weixin.qq.com/cgi-bin/customservice/getkflist?access_token=" + token.ACCESS_TOKEN;

            string ret = new GetUrlData().HttpGet(url);

            JavaScriptSerializer ser = new JavaScriptSerializer();
            OuterClass foo = ser.Deserialize<OuterClass>(ret);

            List<InternalClass> data = foo.kf_list;

            List<int> realkfidlist = new List<int>();//实际现在公众号中的微信客服id列表

            int userrid = UserHelper.CurrentUserId();
            if (data.Count > 0)
            {
                foreach (InternalClass ic in data)
                {
                    int r = new WxkfData().Editwxkf(ic.kf_id, ic.kf_account, ic.kf_nick, basicc.Comid, userrid);
                    realkfidlist.Add(ic.kf_id);
                }
            }

            //获取微信客服列表(1.总公司登录显示全部2.门市登录显示本门市绑定客服)
            int totalcount = 0;
            IList<Wxkf> wxkflist = new WxkfData().Getwxkfpagelist(pageindex, pagesize, basicc.Comid, out   totalcount, userrid, isrun, key);
            if (wxkflist.Count > 0)
            {
                //删除公众号中已经删除的客服
                foreach (Wxkf kf in wxkflist)
                {
                    if (!realkfidlist.Contains(kf.Kf_id))
                    {
                        int d = new WxkfData().DelWxkf(kf.Kf_id, comid);
                    }
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = wxkflist, total = totalcount });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "客服列表查询结果为空" });
            }

        }

        public static string Getwxkf(int kfid, int comid)
        {
            Wxkf m = new WxkfData().Getwxkf(kfid, comid);
            if (m == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "获取微信客服失败" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = m });
            }
        }

        public static string Getygdetail(string phone, int comid)
        {
            B2b_company_manageuser user = new B2bCompanyManagerUserData().GetCompanyUserByPhone(phone, comid);
            if (user == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "没有员工绑定当前手机号" });
            }

            if (user.Channelcompanyid == 0)//公司员工
            {
                return JsonConvert.SerializeObject(new { type = 100, ms_id = 0, ms_name = UserHelper.CurrentCompany.Com_name, yg_id = user.Id, yg_name = user.Employeename });
            }
            else
            {
                Member_Channel_company ms = new MemberChannelcompanyData().GetMenshiByPhone(phone, comid);
                if (ms == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "没有门市绑定当前手机号" });
                }
                return JsonConvert.SerializeObject(new { type = 100, ms_id = ms.Id, ms_name = ms.Companyname, yg_id = user.Id, yg_name = user.Employeename });
            }

        }

        public static string Bindwxdkf(Wxkf m)
        {

            //一个员工只能绑定一个工号
            Wxkf m1 = new WxkfData().Getwxkfbyygid(m.Yg_id);
            if (m1 != null)
            {
                if (m.Kf_id != m1.Kf_id)
                {//员工现在已经绑定了其他客服工号
                    return JsonConvert.SerializeObject(new { type = 1, msg = "一个员工只能绑定一个客服工号" });
                }
            }
            //主客服在每个门市只可设置一个
            if (m.Iszongkf == 1)
            {
                //查询总客服(ms_id=0,则查询公司总客服；ms_id!=0,则查询门市总客服)
                Wxkf m2 = new WxkfData().Getwxzkf(m.Comid, m.Ms_id);
                if (m2 != null)
                {
                    if (m.Kf_id != m2.Kf_id)
                    {//公司/门市已经有了主客服
                        return JsonConvert.SerializeObject(new { type = 1, msg = "总客服在每个公司/门市只可设置一个" });
                    }
                }
            }

            int r = new WxkfData().BindWxdkf(m);
            return JsonConvert.SerializeObject(new { type = 100, msg = r });
        }


        //微信模板列表
        public static string Templatemodelpagelist(int pageindex, int pagesize)
        {
            int totalcount = 0;
            var list = new Weixin_templateData().Templatemodelpagelist(pageindex, pagesize, out totalcount);
            IEnumerable result = "";
            if (list != null)
            {
                result = from pro in list
                         select new
                         {
                             Id = pro.Id,
                             Infotype = pro.Infotype,
                             Template_name = pro.Template_name,
                             First_DATA = pro.First_DATA,
                             Remark_DATA = pro.Remark_DATA,
                             Template_id = pro.Template_id
                         };
            }
            return JsonConvert.SerializeObject(new { type = 100, msg = result, totalcount = totalcount });
        }

        //微信模板列表
        public static string Templatecompagelist(int comid)
        {
            //先插入不含的微信回复模板
            var insertmodel = new Weixin_templateData().Templatecominsert(comid);

            int totalcount = 0;
            var list = new Weixin_templateData().Templatecompagelist(comid, out totalcount);
            IEnumerable result = "";
            if (list != null)
            {
                result = from pro in list
                         select new
                         {
                             Id = pro.Id,
                             Infotype = pro.Infotype,
                             Template_name = pro.Template_name,
                             First_DATA = pro.First_DATA,
                             Remark_DATA = pro.Remark_DATA,
                             Template_id = pro.Template_id
                         };
            }
            return JsonConvert.SerializeObject(new { type = 100, msg = result, totalcount = totalcount });
        }


        //微信模板信息
        public static string Templatemodelinfo(int id)
        {
            var tempmodel = new Weixin_templateData().Templatemodelinfo(id);
            return JsonConvert.SerializeObject(new { type = 100, msg = tempmodel });
        }

        //编辑微信模板
        public static string Templatemodeledit(int id, int infotype, string template_name, string first_DATA, string remark_DATA)
        {
            var tempmodel = new Weixin_templateData().Templatemodeledit(id, infotype, template_name, first_DATA, remark_DATA);
            if (tempmodel > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = tempmodel });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = tempmodel });

            }

        }


        //编辑微信模板
        public static string Templateedit(int comid, string id, string template_id)
        {

            if (comid == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "参数传递错误" });
            }

            if (id == "" || id == ",")
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "参数传递错误" });
            }

            if (template_id == "" || template_id == ",")
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "参数传递错误" });
            }

            var id_arr = id.Split(',');
            var template_id_arr = template_id.Split(',');
            var tempmodel = 0;
            if (id_arr.Count() != template_id_arr.Count())
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "参数传递错误" });
            }

            for (int i = 0; i < id_arr.Count(); i++)
            {
                if (id_arr[i] != "")
                {
                    tempmodel = new Weixin_templateData().Templateedit(comid, id_arr[i], template_id_arr[i]);
                }
            }


            if (tempmodel > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = tempmodel });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = tempmodel });

            }

        }




        /// <summary>
        /// 上传微信图文信息到微信服务器
        /// </summary>
        /// <param name="createuserid"></param>
        /// <param name="comid"></param>
        /// <param name="txttitle"></param>
        /// <param name="txtauthor"></param>
        /// <param name="txtdigest"></param>
        /// <param name="thumb_media_id"></param>
        /// <param name="sel_show_cover_pic"></param>
        /// <param name="txtcontent"></param>
        /// <param name="txtcontent_source_url"></param>
        /// <returns></returns>
        public static string Uploadwxqunfa_news(int createuserid, int comid, string txttitle, string txtauthor, string txtdigest, string thumb_media_id, string sel_show_cover_pic, string txtcontent, string txtcontent_source_url, string thumb_url, int materialid)
        {
            WeiXinBasic basic = new WeiXinBasicData().GetWxBasicByComId(comid);

            WXAccessToken m_accesstoken = new Weixin_tmplmsgManage().GetAccessToken(basic.Comid, basic.AppId, basic.AppSecret);

            List<Wxarticle> list = new List<Wxarticle>();
            list.Add(new Wxarticle() { title = txttitle, author = txtauthor, content = txtcontent, content_source_url = txtcontent_source_url, digest = txtdigest, show_cover_pic = sel_show_cover_pic, thumb_media_id = thumb_media_id });

            string created_at = "";//图文消息生成时间
            string mediaid = new WxUploadDownManage().Uploadnews(m_accesstoken.ACCESS_TOKEN, list, out created_at);

            if (mediaid == "")
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "图文消息上传失败" });
            }
            if (mediaid == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "图文消息上传失败" });
            }



            SqlHelper sqlhelper = new SqlHelper();
            sqlhelper.BeginTrancation();
            try
            {
                SqlCommand cmd = new SqlCommand();

                DateTime createtime = new WeiXinManage().UnixTimeToTime(created_at);
                //向 图文添加记录表 中添加记录
                string sql1 = "INSERT INTO [EtownDB].[dbo].[wxqunfa_news_addrecord]([is_singlenews] ,[type],[media_id] ,[created_at],[createtime] ,[createuserid] ,[comid])" +
     "VALUES(1,'news','" + mediaid + "','" + created_at + "','" + createtime + "'," + createuserid + " ," + comid + ");select @@IDENTITY;";
                cmd = sqlhelper.PrepareTextSqlCommand(sql1);
                object o = cmd.ExecuteScalar();
                int newrecordid = int.Parse(o.ToString());

                string sql2 = "INSERT INTO [EtownDB].[dbo].[wxqunfa_news]([thumb_media_id],[author],[title] ,[content_source_url],[content],[digest],[show_cover_pic],[newsrecordid],thumb_url,wxmaterialid) " +
     " VALUES('" + thumb_media_id + "','" + txtauthor + "','" + txttitle + "','" + txtcontent_source_url + "','" + txtcontent + "','" + txtdigest + "','" + sel_show_cover_pic + "'," + newrecordid + ",'" + thumb_url + "'," + materialid + ");select @@IDENTITY;";
                cmd = sqlhelper.PrepareTextSqlCommand(sql2);
                cmd.ExecuteNonQuery();


                sqlhelper.Commit();
                sqlhelper.Dispose();
                return JsonConvert.SerializeObject(new { type = 100, msg = newrecordid });
            }
            catch (Exception ex)
            {
                sqlhelper.Rollback();
                sqlhelper.Dispose();
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }

        }

        public static string Wxqunfa_news_addrecordpagelist(int userid, int comid, int pageindex, int pagesize, string key)
        {
            int totalcount = 0;
            List<Wxqunfa_news_addrecord> recordlist = new Wxqunfa_news_addrecordData().Wxqunfa_news_addrecordpagelist(userid, comid, pageindex, pagesize, key, out totalcount);
            if (recordlist == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "没有在有效期内的图文消息" });
            }
            if (recordlist.Count == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "没有在有效期内的图文消息" });
            }

            IEnumerable result = "";

            result = from pro in recordlist
                     select new
                     {
                         Id = pro.id,
                         CreateTime = pro.createtime,
                         NewsList = new Wxqunfa_newsData().GetTop1NewsListByRecordid(pro.id),
                         MediaId = pro.media_id,
                         pro.is_singlenews
                     };
            return JsonConvert.SerializeObject(new { type = 100, msg = result, totalcount = totalcount });

        }

        public static string GetNewsListByRecordid(int recordid)
        {

            List<Wxqunfa_news> recordlist = new Wxqunfa_newsData().GetNewsListByRecordid(recordid);
            if (recordlist == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "没有在有效期内的图文消息" });
            }
            if (recordlist.Count == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "没有在有效期内的图文消息" });
            }
            return JsonConvert.SerializeObject(new { type = 100, msg = recordlist });
        }

        public static string Wxqunfa_news_addrecord(int userid, int comid, int tuwen_recordid)
        {
            if (userid == 0 || comid == 0 || tuwen_recordid == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "传参失败" });
            }

            Wxqunfa_news_addrecord r = new Wxqunfa_news_addrecordData().Wxqunfa_news_addrecord(userid, comid, tuwen_recordid);
            if (r == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "没有在有效期内的图文消息" });
            }

            return JsonConvert.SerializeObject(new { type = 100, msg = r });

        }
        /// <summary>
        /// 获得微信群发消息
        /// </summary>
        /// <param name="newsid"></param>
        /// <returns></returns>
        public static string Getwxqunfa_newslist(int news_recordid, int opertype)
        {
            if (opertype == 0)//新建
            {
                List<Wxqunfa_news> list = new Wxqunfa_newsData().GetNewsListByRecordid(news_recordid);
                list.Add(new Wxqunfa_news { id = 0 });
                return JsonConvert.SerializeObject(new { type = 100, msg = list, num = list.Count + 1 });
            }
            else if (opertype == 2)//编辑
            {
                List<Wxqunfa_news> list = new Wxqunfa_newsData().GetNewsListByRecordid(news_recordid);

                return JsonConvert.SerializeObject(new { type = 100, msg = list, num = list.Count + 1 });
            }
            else//展示
            {
                List<Wxqunfa_news> list = new Wxqunfa_newsData().GetNewsListByRecordid(news_recordid);
                if (list.Count == 0)
                {
                    list.Add(new Wxqunfa_news { id = 0 });
                    return JsonConvert.SerializeObject(new { type = 100, msg = list, num = 1 });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = list, num = list.Count });
                }
            }

        }
        /// <summary>
        /// 保存微信群发的多图文素材
        /// </summary>
        /// <param name="newsid"></param>
        /// <param name="news_recordid"></param>
        /// <param name="createuserid"></param>
        /// <param name="comid"></param>
        /// <param name="txttitle"></param>
        /// <param name="txtauthor"></param>
        /// <param name="txtdigest"></param>
        /// <param name="thumb_media_id"></param>
        /// <param name="sel_show_cover_pic"></param>
        /// <param name="txtcontent"></param>
        /// <param name="txtcontent_source_url"></param>
        /// <param name="thumb_url"></param>
        /// <param name="materialid"></param>
        /// <returns></returns>
        public static string Savewxqunfa_multinews(int newsid, int news_recordid, int createuserid, int comid, string txttitle, string txtauthor, string txtdigest, string thumb_media_id, string sel_show_cover_pic, string txtcontent, string txtcontent_source_url, string thumb_url, int materialid)
        {
            if (news_recordid == 0)//添加多图文消息
            {
                SqlHelper sqlhelper = new SqlHelper();
                sqlhelper.BeginTrancation();
                try
                {
                    SqlCommand cmd = new SqlCommand();

                    DateTime createtime = DateTime.Now;
                    //向 图文添加记录表 中添加记录
                    string sql1 = "INSERT INTO  [wxqunfa_news_addrecord]([is_singlenews] ,[type],[media_id] ,[created_at],[createtime] ,[createuserid] ,[comid])" +
         "VALUES(0,'news','" + "" + "','" + "" + "','" + createtime + "'," + createuserid + " ," + comid + ");select @@IDENTITY;";
                    cmd = sqlhelper.PrepareTextSqlCommand(sql1);
                    object o = cmd.ExecuteScalar();
                    int newrecordid = int.Parse(o.ToString());

                    string sql2 = "INSERT INTO  [wxqunfa_news]([thumb_media_id],[author],[title] ,[content_source_url],[content],[digest],[show_cover_pic],[newsrecordid],thumb_url,wxmaterialid) " +
         " VALUES('" + thumb_media_id + "','" + txtauthor + "','" + txttitle + "','" + txtcontent_source_url + "','" + txtcontent + "','" + txtdigest + "','" + sel_show_cover_pic + "'," + newrecordid + ",'" + thumb_url + "'," + materialid + ");select @@IDENTITY;";
                    cmd = sqlhelper.PrepareTextSqlCommand(sql2);
                    cmd.ExecuteNonQuery();


                    sqlhelper.Commit();
                    sqlhelper.Dispose();
                    return JsonConvert.SerializeObject(new { type = 100, msg = newrecordid });
                }
                catch (Exception ex)
                {
                    sqlhelper.Rollback();
                    sqlhelper.Dispose();
                    return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                }
            }
            else
            {
                if (newsid == 0)//添加多图文消息
                {
                    SqlHelper sqlhelper = new SqlHelper();
                    string sql2 = "INSERT INTO  [wxqunfa_news]([thumb_media_id],[author],[title] ,[content_source_url],[content],[digest],[show_cover_pic],[newsrecordid],thumb_url,wxmaterialid) " +
       " VALUES('" + thumb_media_id + "','" + txtauthor + "','" + txttitle + "','" + txtcontent_source_url + "','" + txtcontent + "','" + txtdigest + "','" + sel_show_cover_pic + "'," + news_recordid + ",'" + thumb_url + "'," + materialid + ");select @@IDENTITY;";
                    var cmd = sqlhelper.PrepareTextSqlCommand(sql2);
                    cmd.ExecuteNonQuery();

                    string sql3 = "update wxqunfa_news_addrecord set media_id='' where id=" + news_recordid;
                    var cmd3 = sqlhelper.PrepareTextSqlCommand(sql3);
                    cmd3.ExecuteNonQuery();
                    return JsonConvert.SerializeObject(new { type = 100, msg = news_recordid });
                }
                else//编辑多图文消息 
                {
                    SqlHelper sqlhelper = new SqlHelper();
                    string sql2 = "UPDATE  [wxqunfa_news]   SET [thumb_media_id] = '" + thumb_media_id + "',[author] = '" + txtauthor + "' ,[title] = '" + txttitle + "' ,[content_source_url] = '" + txtcontent_source_url + "' ,[content] = '" + txtcontent + "',[digest] = '" + txtdigest + "'  ,[show_cover_pic] = '" + sel_show_cover_pic + "'  ,[newsrecordid] = " + news_recordid + " ,[thumb_url] = '" + thumb_url + "'  ,[wxmaterialid] = " + materialid + " WHERE id=" + newsid;
                    var cmd = sqlhelper.PrepareTextSqlCommand(sql2);
                    cmd.ExecuteNonQuery();

                    string sql3 = "update wxqunfa_news_addrecord set media_id='' where id=" + news_recordid;
                    var cmd3 = sqlhelper.PrepareTextSqlCommand(sql3);
                    cmd3.ExecuteNonQuery();
                    return JsonConvert.SerializeObject(new { type = 100, msg = news_recordid });
                }
            }

        }

        public static string Delwxqunfa_news(int newsid)
        {
            if (newsid > 0)
            {
                SqlHelper sqlHelper = new SqlHelper();
                string sql = "delete wxqunfa_news where id=" + newsid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.ExecuteNonQuery();

                string sql3 = "update wxqunfa_news_addrecord set media_id='' where id=(select newsrecordid from wxqunfa_news where id=" + newsid + ")";
                var cmd3 = sqlHelper.PrepareTextSqlCommand(sql3);
                cmd3.ExecuteNonQuery();


                return JsonConvert.SerializeObject(new { type = 100, msg = "" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = "" });
            }

        }

        public static string Uploadwxqunfa_news_multi(int news_recordid, int comid)
        {
            WeiXinBasic basic = new WeiXinBasicData().GetWxBasicByComId(comid);

            WXAccessToken m_accesstoken = new Weixin_tmplmsgManage().GetAccessToken(basic.Comid, basic.AppId, basic.AppSecret);


            List<Wxarticle> list = new List<Wxarticle>();
            SqlHelper sqlHelper = new SqlHelper();
            string sql = "SELECT   [id]  ,[thumb_media_id] ,[author] ,[title] ,[content_source_url],[content] ,[digest],[show_cover_pic] ,[newsrecordid],[thumb_url] ,[wxmaterialid] FROM [wxqunfa_news] where newsrecordid=" + news_recordid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Wxarticle
                    {
                        title = reader.GetValue<string>("title"),
                        author = reader.GetValue<string>("author"),
                        content = reader.GetValue<string>("content"),
                        content_source_url = reader.GetValue<string>("content_source_url"),
                        digest = reader.GetValue<string>("digest"),
                        show_cover_pic = reader.GetValue<int>("show_cover_pic").ToString(),
                        thumb_media_id = reader.GetValue<string>("thumb_media_id")
                    });
                }
            }
            if (list.Count == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "当前多图文消息不存在" });
            }

            string created_at = "";//图文消息生成时间
            string mediaid = new WxUploadDownManage().Uploadnews(m_accesstoken.ACCESS_TOKEN, list, out created_at);

            if (mediaid == "")
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "图文消息上传失败" });
            }
            if (mediaid == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "图文消息上传失败" });
            }

            string sql2 = "update wxqunfa_news_addrecord set  [media_id]='" + mediaid + "' where id=" + news_recordid;
            var cmdd = sqlHelper.PrepareTextSqlCommand(sql2);
            int re = cmdd.ExecuteNonQuery();
            if (re > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = "图文消息上传成功" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "图文消息上传录入失败" });
            }
        }

        public static string EditChannelWxQrcode(WxSubscribeSource m)
        {
            try
            {
                WeiXinBasic basic = new WeiXinBasicData().GetWxBasicByComId(m.Comid);
                if (basic == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
                if (basic.Weixintype != 4)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "微信公众号必须是认证公众号" });
                }

                //如果渠道二维码没有生成过，则新生成二维码；生成过，则不再生成二维码，只是需要修改二维码属性信息
                string qrcodeurl = "";//带参二维码地址

                if (m.Id == 0)
                {
                    string sql1 = @"INSERT INTO  [WxSubscribeSource]
                                           ([sourcetype]
                                           ,[channelcompanyid]
                                           ,[activityid]
                                           ,[comid]
                                           ,[ticket]
                                           ,[title]
                                           ,[qrcodeurl]
                                           ,[createtime]
                                           ,[onlinestatus]
                                           ,[channelid]
                                           ,[productid]
                                           ,[Wxmaterialid]
                                           ,[qrcodeviewtype]
                                           ,[projectid]
                                           ,[Wxmaterialtypeid]
                                           ,viewchannelcompanyid
                                           ,choujiangactid)
                                     VALUES
                                           (@sourcetype
                                           ,@channelcompanyid
                                           ,@activityid
                                           ,@comid
                                           ,@ticket
                                           ,@title
                                           ,@qrcodeurl
                                           ,@createtime
                                           ,@onlinestatus
                                           ,@channelid
                                           ,@productid
                                           ,@Wxmaterialid
                                           ,@qrcodeviewtype
                                           ,@projectid
                                           ,@Wxmaterialtypeid
                                          ,@viewchannelcompanyid
                                          ,@choujiangactid);select @@identity;";
                    var cmd1 = new SqlHelper().PrepareTextSqlCommand(sql1);
                    cmd1.AddParam("@sourcetype", m.Sourcetype);
                    cmd1.AddParam("@channelcompanyid", m.Channelcompanyid);
                    cmd1.AddParam("@activityid", m.Activityid);
                    cmd1.AddParam("@comid", m.Comid);
                    cmd1.AddParam("@ticket", m.Ticket);
                    cmd1.AddParam("@title", m.Title);
                    cmd1.AddParam("@qrcodeurl", m.Qrcodeurl);
                    cmd1.AddParam("@createtime", m.Createtime);
                    cmd1.AddParam("@onlinestatus", m.Onlinestatus);
                    cmd1.AddParam("@channelid", m.Channelid);
                    cmd1.AddParam("@productid", m.Productid);
                    cmd1.AddParam("@Wxmaterialid", m.Wxmaterialid);
                    cmd1.AddParam("@qrcodeviewtype", m.qrcodeviewtype);
                    cmd1.AddParam("@projectid", m.projectid);
                    cmd1.AddParam("@Wxmaterialtypeid", m.wxmaterialtypeid);
                    cmd1.AddParam("@viewchannelcompanyid", m.viewchannelcompanyid);
                    cmd1.AddParam("@choujiangactid", m.choujiangactid);

                    object o = cmd1.ExecuteScalar();
                    m.Id = int.Parse(o.ToString());



                    if (m.Id > 0)
                    {
                        //生成新的二维码图片
                        string ticket = "";//二维码ticket
                        qrcodeurl = GetWxQrCodeUrl(m.Comid, m.Id, out ticket);
                        ////------------正式环境时需要去掉下面的静态内容----------------
                        //ticket = "gQEh8DoAAAAAAAAAASxodHRwOi8vd2VpeGluLnFxLmNvbS9xL2EzVVNYd2ZsVUVNcC1TbXVtRmwxAAIEu/54VQMEAAAAAA==";
                        //qrcodeurl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=gQEh8DoAAAAAAAAAASxodHRwOi8vd2VpeGluLnFxLmNvbS9xL2EzVVNYd2ZsVUVNcC1TbXVtRmwxAAIEu%2f54VQMEAAAAAA%3d%3d";
                        ////----------------------------------

                        if (qrcodeurl == "")
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "二维码图片生成有误", qrcodeurl = "" });
                        }

                        string sql2 = "update WxSubscribeSource set ticket='" + ticket + "' , qrcodeurl='" + qrcodeurl + "' where id=" + m.Id;
                        var cmd2 = new SqlHelper().PrepareTextSqlCommand(sql2);

                        int nn = cmd2.ExecuteNonQuery();
                        if (nn == 0)
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "二维码ticket和url录入有误", qrcodeurl = "" });
                        }
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "录入二维码信息出错", qrcodeurl = "" });
                    }
                }
                else
                {

                    //修改二维码基本信息 
                    string sql2 = @"UPDATE  [WxSubscribeSource]
                                   SET [sourcetype] = @sourcetype
                                      ,[channelcompanyid] = @channelcompanyid
                                      ,[activityid] = @activityid
                                      ,[comid] = @comid
                                      ,[ticket] = @ticket
                                      ,[title] = @title
                                      ,[qrcodeurl] = @qrcodeurl
                                      ,[createtime] = @createtime
                                      ,[onlinestatus] = @onlinestatus
                                      ,[channelid] = @channelid
                                      ,[productid] = @productid
                                      ,[Wxmaterialid] = @Wxmaterialid
                                      ,[qrcodeviewtype] = @qrcodeviewtype
                                      ,[projectid] = @projectid
                                      ,[Wxmaterialtypeid] = @Wxmaterialtypeid
                                      ,viewchannelcompanyid=@viewchannelcompanyid
                                      ,choujiangactid=@choujiangactid 
                                 WHERE id=@id";
                    var cmd2 = new SqlHelper().PrepareTextSqlCommand(sql2);
                    cmd2.AddParam("@id", m.Id);
                    cmd2.AddParam("@sourcetype", m.Sourcetype);
                    cmd2.AddParam("@channelcompanyid", m.Channelcompanyid);
                    cmd2.AddParam("@activityid", m.Activityid);
                    cmd2.AddParam("@comid", m.Comid);
                    cmd2.AddParam("@ticket", m.Ticket);
                    cmd2.AddParam("@title", m.Title);
                    cmd2.AddParam("@qrcodeurl", m.Qrcodeurl);
                    cmd2.AddParam("@createtime", m.Createtime);
                    cmd2.AddParam("@onlinestatus", m.Onlinestatus);
                    cmd2.AddParam("@channelid", m.Channelid);
                    cmd2.AddParam("@productid", m.Productid);
                    cmd2.AddParam("@Wxmaterialid", m.Wxmaterialid);
                    cmd2.AddParam("@qrcodeviewtype", m.qrcodeviewtype);
                    cmd2.AddParam("@projectid", m.projectid);
                    cmd2.AddParam("@Wxmaterialtypeid", m.wxmaterialtypeid);
                    cmd2.AddParam("@viewchannelcompanyid", m.viewchannelcompanyid);
                    cmd2.AddParam("@choujiangactid", m.choujiangactid);
                    cmd2.ExecuteNonQuery();

                    //得到带参二维码地址
                    string sql1 = "select qrcodeurl from  WxSubscribeSource where id=" + m.Id;
                    var cmd1 = new SqlHelper().PrepareTextSqlCommand(sql1);
                    using (var reader = cmd1.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            qrcodeurl = reader.GetValue<string>("qrcodeurl");
                        }
                    }

                }
                return JsonConvert.SerializeObject(new { type = 100, msg = "", qrcodeurl = qrcodeurl, qrcodeid = m.Id });

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }

        public static string Getchannelwxqrcodebychannelid(int channelid)
        {
            WxSubscribeSource source = new WxSubscribeSourceData().Getchannelwxqrcodebychannelid(channelid);
            if (source == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = source });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = source });
            }
        }

        public static string Getmicromallimgbycomid(int comid)
        {
            int micromallimgid = new B2bCompanyData().Getmicromallimgbycomid(comid);
            if (micromallimgid == 0)
            {
                micromallimgid = new B2bCompanyData().Getmicromallimgbycomid(106);
            }
            string imgurl = FileSerivce.GetImgUrl(micromallimgid);

            return JsonConvert.SerializeObject(new { type = 100, imgid = micromallimgid, imgurl = imgurl });
        }

        public static string GetReserveproVerifywxqrcode(int comid, int qrcodeviewtype)
        {
            WxSubscribeSource source = new WxSubscribeSourceData().GetReserveproVerifywxqrcode(comid, qrcodeviewtype);
            if (source == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = source });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = source });
            }
        }
        /// <summary>
        /// 下载微信语音
        /// </summary>
        /// <param name="mediaid"></param>
        /// <param name="openid"></param>
        /// <param name="comid"></param>
        /// <param name="clientuptypemark"></param>
        /// <returns></returns>
        public static string Wxdownvoice(string mediaid, string openid, int comid, int clientuptypemark, string remarks, int materialid)
        {
            lock (lockobj)
            {
                //删除当前顾问 的没有完成操作的标记 
                int r_delmark = new Wxmedia_updownlogData().DelGuwenNotSucMediaMark(openid);

                #region 下载文章录音
                if (materialid > 0)
                {
                    #region 判断是否有文章语音,有的话覆盖之前该文章语音
                    Wxmedia_updownlog log = new Wxmedia_updownlogData().GetWxmedia_updownlogbymaterialid(materialid);
                    if (log != null)
                    {
                        //首先需要把覆盖前的语音上传过的记录去掉
                        int delbeforelogs = new Wxmedia_updownlogData().DelBeforeCoverUplog(materialid);


                        WeiXinBasic basic = new WeiXinBasicData().GetWxBasicByComId(comid);
                        if (basic == null)
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "获取公司微信配置信息出错" });
                        }

                        //获取凭证（首先判断数据库是否存在凭证以及是否过期，如过期，重新获取）
                        WXAccessToken token = GetAccessToken(comid, basic.AppId, basic.AppSecret);

                        string Filepath = System.Web.HttpContext.Current.Server.MapPath("/WxClientVoiceUploadFile/");
                        if (!Directory.Exists(Filepath))//判断路径是否存在
                        {
                            Directory.CreateDirectory(Filepath);//如果不存在创建文件夹           
                        }

                        string relativepath = log.relativepath;
                        string savepath = log.savepath;

                        // 下载用户上传的语音到本地
                        string udresult = new WxUploadDownManage().GetMultimedia(token.ACCESS_TOKEN, mediaid, savepath);
                        if (udresult == "1")//下载用户上传的语音 到本地成功
                        {
                            log.mediaid = mediaid;
                            log.mediatype = "voice";
                            log.savepath = savepath;
                            log.created_at = ConvertDateTimeInt(DateTime.Now).ToString();
                            log.createtime = DateTime.Now;
                            log.opertype = "down";//下载用户上传的语音
                            log.isfinish = 1;
                            log.relativepath = relativepath;
                            log.materialid = materialid;
                            int udlogresult2 = new Wxmedia_updownlogData().Edituploadlog(log);
                            if (udlogresult2 > 0)
                            {
                                return JsonConvert.SerializeObject(new { type = 100, msg = "上传语音成功" });
                            }
                            else
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "上传语音失败.." });
                            }

                        }
                        else //下载用户上传的语音 到本地失败
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = ".上传语音失败" });
                        }
                    }
                    #endregion
                    #region 判断是否有文章语音,没有的话 新建该文章语音
                    else
                    {
                        Wxmedia_updownlog udlog = new Wxmedia_updownlog
                        {
                            id = 0,
                            mediaid = mediaid,
                            mediatype = "voice",
                            savepath = "",
                            created_at = "",
                            createtime = DateTime.Now,
                            opertype = "down",
                            operweixin = openid,
                            clientuptypemark = clientuptypemark,
                            comid = comid,
                            relativepath = "",
                            txtcontent = "",
                            isfinish = 0,
                            remarks = remarks,
                            materialid = materialid
                        };
                        int udlogresult = new Wxmedia_updownlogData().Edituploadlog(udlog);
                        udlog.id = udlogresult;

                        #region 下载用户上传的语音到本地
                        WeiXinBasic basic = new WeiXinBasicData().GetWxBasicByComId(comid);
                        if (basic == null)
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "获取公司微信配置信息出错" });
                        }

                        //获取凭证（首先判断数据库是否存在凭证以及是否过期，如过期，重新获取）
                        WXAccessToken token = GetAccessToken(comid, basic.AppId, basic.AppSecret);

                        string Filepath = System.Web.HttpContext.Current.Server.MapPath("/WxClientVoiceUploadFile/");
                        if (!Directory.Exists(Filepath))//判断路径是否存在
                        {
                            Directory.CreateDirectory(Filepath);//如果不存在创建文件夹           
                        }
                        string medianame = DateTime.Now.ToString("yyyyMMddHHmmssfff") + (new Random()).Next().ToString().Substring(0, 4) + ".amr";
                        string relativepath = "/WxClientVoiceUploadFile/" + medianame;
                        string savepath = System.Web.HttpContext.Current.Server.MapPath("/WxClientVoiceUploadFile/") + medianame;

                        // 下载用户上传的语音到本地
                        string udresult = new WxUploadDownManage().GetMultimedia(token.ACCESS_TOKEN, mediaid, savepath);

                        #endregion

                        if (udresult == "1")//下载用户上传的语音 到本地成功
                        {
                            udlog.savepath = savepath;
                            udlog.created_at = ConvertDateTimeInt(DateTime.Now).ToString();
                            udlog.createtime = DateTime.Now;
                            udlog.isfinish = 1;
                            udlog.relativepath = relativepath;

                            int udlogresult2 = new Wxmedia_updownlogData().Edituploadlog(udlog);
                            if (udlogresult2 > 0)
                            {
                                return JsonConvert.SerializeObject(new { type = 100, msg = "上传语音成功" });
                            }
                            else
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "上传语音失败.." });
                            }
                        }
                        else //下载用户上传的语音 到本地失败
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = ".上传语音失败" });
                        }
                    }
                    #endregion
                }
                #endregion
                #region 下载顾问录音
                else
                {
                    Wxmedia_updownlog udlog = new Wxmedia_updownlog
                    {
                        id = 0,
                        mediaid = mediaid,
                        mediatype = "voice",
                        savepath = "",
                        created_at = "",
                        createtime = DateTime.Now,
                        opertype = "down",
                        operweixin = openid,
                        clientuptypemark = clientuptypemark,
                        comid = comid,
                        relativepath = "",
                        txtcontent = "",
                        isfinish = 0,
                        remarks = remarks,
                        materialid = materialid
                    };
                    int udlogresult = new Wxmedia_updownlogData().Edituploadlog(udlog);
                    udlog.id = udlogresult;

                    #region 下载用户上传的语音到本地
                    WeiXinBasic basic = new WeiXinBasicData().GetWxBasicByComId(comid);
                    if (basic == null)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "获取公司微信配置信息出错" });
                    }

                    //获取凭证（首先判断数据库是否存在凭证以及是否过期，如过期，重新获取）
                    WXAccessToken token = GetAccessToken(comid, basic.AppId, basic.AppSecret);

                    string Filepath = System.Web.HttpContext.Current.Server.MapPath("/WxClientVoiceUploadFile/");
                    if (!Directory.Exists(Filepath))//判断路径是否存在
                    {
                        Directory.CreateDirectory(Filepath);//如果不存在创建文件夹           
                    }
                    string medianame = DateTime.Now.ToString("yyyyMMddHHmmssfff") + (new Random()).Next().ToString().Substring(0, 4) + ".amr";
                    string relativepath = "/WxClientVoiceUploadFile/" + medianame;
                    string savepath = System.Web.HttpContext.Current.Server.MapPath("/WxClientVoiceUploadFile/") + medianame;

                    // 下载用户上传的语音到本地
                    string udresult = new WxUploadDownManage().GetMultimedia(token.ACCESS_TOKEN, mediaid, savepath);

                    #endregion

                    if (udresult == "1")//下载用户上传的语音 到本地成功
                    {
                        udlog.savepath = savepath;
                        udlog.created_at = ConvertDateTimeInt(DateTime.Now).ToString();
                        udlog.createtime = DateTime.Now;
                        udlog.isfinish = 1;
                        udlog.relativepath = relativepath;

                        int udlogresult2 = new Wxmedia_updownlogData().Edituploadlog(udlog);
                        if (udlogresult2 > 0)
                        {
                            return JsonConvert.SerializeObject(new { type = 100, msg = "上传语音成功" });
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "上传语音失败.." });
                        }
                    }
                    else //下载用户上传的语音 到本地失败
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = ".上传语音失败" });
                    }
                }
                #endregion
            }
        }


        public static string Getwxdownvoicelist(string openid, int clientuptypemark, int materialid)
        {
            IList<Wxmedia_updownlog> list = new Wxmedia_updownlogData().Getwxdownvoicelist(openid, clientuptypemark, materialid);
            if (list.Count == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "查询结果为空" });
            }
            else
            {
                IEnumerable result = "";
                result = from m in list
                         select new
                         {
                             m.id,

                             m.mediatype,
                             m.savepath,
                             m.created_at,
                             m.createtime,
                             m.opertype,
                             m.operweixin,
                             m.clientuptypemark,
                             m.comid,
                             m.relativepath,
                             m.txtcontent,
                             m.isfinish,
                             IsOver3days = GetIsOver3Days(m.createtime, m.savepath),
                             mediaid = GetNewestupmediaid(m.createtime, m.savepath, m.mediaid)
                         };
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
        }
        /// <summary>
        /// 得到最新上传的多媒体id
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private static string GetNewestupmediaid(DateTime downtime, string savepath, string mediaid)
        {
            //判断下载时间是否超过2天(微信服务器端保存多媒体信息3天)
            if (downtime.AddDays(2) > DateTime.Now)
            {
                return mediaid;
            }
            else
            {
                //如果超过，在判断多媒体最新上传到微信服务器获取的媒体id，是否超过3天
                //获得多媒体最新上传的记录
                Wxmedia_updownlog log = new Wxmedia_updownlogData().GetNewestuplogbymedia(savepath);
                if (log != null)
                {
                    if (log.createtime.AddDays(2) > DateTime.Now)
                    {
                        return log.mediaid;
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
        }
        private static int GetIsOver2Days(DateTime downtime)
        {
            //判断下载时间是否超过2天(微信服务器端保存多媒体信息3天)
            if (downtime.AddDays(2) > DateTime.Now)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        /// <summary>
        /// 判断下载时间是否超过3天；如果超过，在判断多媒体最新上传到微信服务器获取的媒体id，是否超过3天：超过3天需要重新上传获取媒体id
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private static int GetIsOver3Days(DateTime downtime, string savepath)
        {
            //判断下载时间是否超过2天(微信服务器端保存多媒体信息3天)
            if (downtime.AddDays(2) > DateTime.Now)
            {
                return 0;
            }
            else
            {
                //如果超过，在判断多媒体最新上传到微信服务器获取的媒体id，是否超过3天
                //获得多媒体最新上传的记录
                Wxmedia_updownlog log = new Wxmedia_updownlogData().GetNewestuplogbymedia(savepath);
                if (log != null)
                {
                    if (log.createtime.AddDays(2) > DateTime.Now)
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    return 1;
                }
            }
        }

        public static string Getwxvoicemediaid(string openid, int uplogid, int comid)
        {

            WeiXinBasic basic = new WeiXinBasicData().GetWxBasicByComId(comid);
            if (basic == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "获取商户微信信息出错" });
            }

            WXAccessToken m_accesstoken = WeiXinManage.GetAccessToken(basic.Comid, basic.AppId, basic.AppSecret);

            Wxmedia_updownlog udlog = new Wxmedia_updownlogData().GetWxmedia_updownlogbyid(uplogid);
            if (udlog == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "获取多媒体上传下载日志信息出错" });
            }
            else
            {
                //根据udlog.savepath 得到最近一次上传记录
                Wxmedia_updownlog newestuplog = new Wxmedia_updownlogData().GetNewestuplogbymedia(udlog.savepath);
                if (newestuplog != null)
                {
                    //在2天内上传过，则不再重复上传获取mediaid(微信服务器保存多媒体期限是3天)
                    if (newestuplog.createtime.AddDays(2) > DateTime.Now)
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = newestuplog.mediaid });
                    }
                    else
                    {
                        #region 上传多媒体信息返回mediaid
                        string media_id = new WxUploadDownManage().UploadMultimedia(m_accesstoken.ACCESS_TOKEN, "voice", udlog.savepath);
                        if (media_id != "")
                        {
                            Wxmedia_updownlog uplog = new Wxmedia_updownlog
                            {
                                id = 0,
                                mediaid = media_id,
                                mediatype = "voice",
                                savepath = udlog.savepath,
                                created_at = ConvertDateTimeInt(DateTime.Now).ToString(),
                                createtime = DateTime.Now,
                                opertype = "up",
                                operweixin = openid,
                                clientuptypemark = (int)Clientuptypemark.UpMedia,//上传多媒体信息
                                comid = basic.Comid,
                                relativepath = udlog.relativepath,
                                txtcontent = "",
                                isfinish = 1
                            };
                            int uplogresult = new Wxmedia_updownlogData().Edituploadlog(uplog);
                            if (uplogresult == 0)
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "录入上传信息出错" });
                            }
                            else
                            {
                                return JsonConvert.SerializeObject(new { type = 100, msg = media_id });
                            }
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "上传多媒体信息返回mediaid出错" });
                        }
                        #endregion
                    }
                }
                else
                {
                    #region 上传多媒体信息返回mediaid
                    string media_id = new WxUploadDownManage().UploadMultimedia(m_accesstoken.ACCESS_TOKEN, "voice", udlog.savepath);
                    if (media_id != "")
                    {
                        Wxmedia_updownlog uplog = new Wxmedia_updownlog
                        {
                            id = 0,
                            mediaid = media_id,
                            mediatype = "voice",
                            savepath = udlog.savepath,
                            created_at = ConvertDateTimeInt(DateTime.Now).ToString(),
                            createtime = DateTime.Now,
                            opertype = "up",
                            operweixin = openid,
                            clientuptypemark = (int)Clientuptypemark.UpMedia,//上传多媒体信息
                            comid = basic.Comid,
                            relativepath = udlog.relativepath,
                            txtcontent = "",
                            isfinish = 1
                        };
                        int uplogresult = new Wxmedia_updownlogData().Edituploadlog(uplog);
                        if (uplogresult == 0)
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "录入上传信息出错" });
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new { type = 100, msg = media_id });
                        }
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "上传多媒体信息返回mediaid出错" });
                    }
                    #endregion
                }


            }
        }

        public static string Createtempwxqrcode(int tempqrcodeid, int comid)
        {
            try
            {
                WeiXinBasic basic = new WeiXinBasicData().GetWxBasicByComId(comid);
                if (basic == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
                if (basic.Weixintype != 4)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "微信公众号必须是认证公众号" });
                }

                string tempqrcodeurl = "";

                #region 生成临时二维码，并保存入缓存，缓存1天
                if (HttpRuntime.Cache["tempqrcodeurl"] == null)
                {
                    tempqrcodeurl = GetWxTempQrCodeUrl(comid, tempqrcodeid);

                    //有效期7200秒，开发者必须在自己的服务全局缓存jsapi_ticket
                    HttpRuntime.Cache.Insert("tempqrcodeurl", tempqrcodeurl, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration); //HttpRuntime.Cache与HttpContext.Current.Cache是同一对象,建议使用HttpRuntime.Cache

                    return tempqrcodeurl;
                }
                else
                {
                    tempqrcodeurl = System.Web.HttpRuntime.Cache.Get("tempqrcodeurl").ToString();
                }
                #endregion
                return JsonConvert.SerializeObject(new { type = 100, msg = "", qrcodeurl = tempqrcodeurl });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }

        public static string getNativePayQrcode(int proid, int comid, string paramtype)
        {
            if (paramtype == "pid" || paramtype == "oid")
            { }
            else 
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "链接中带固定参数productid标识错误" });   
            }
            B2b_finance_paytype model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(comid);

            if (model != null)
            {
                //商家微信支付的所有参数都存在
                if (model.Wx_appid != "" && model.Wx_appkey != "" && model.Wx_partnerid != "" && model.Wx_paysignkey != "")
                { }
                else
                {
                    model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(106);
                } 
            }
            else
            {
                model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(106);
                //return JsonConvert.SerializeObject(new { type = 1, msg ="商户没有开通微信支付" });
            }

            WxPayConfig config = new WxPayConfig
            {
                APPID = model.Wx_appid,
                APPSECRET = model.Wx_appkey,
                KEY = model.Wx_paysignkey,
                MCHID = model.Wx_partnerid,
                IP = CommonFunc.GetRealIP(),
                SSLCERT_PATH = model.wx_SSLCERT_PATH,
                SSLCERT_PASSWORD = model.wx_SSLCERT_PASSWORD,
                PROXY_URL = "",
                LOG_LEVENL = 3,//日志级别
                REPORT_LEVENL = 0//上报信息配置
            };



            NativePay nativePay = new NativePay();
            string url = nativePay.GetPrePayUrl(paramtype + proid.ToString(), config);

            WxPayData data = new WxPayData();
            data.SetValue("appid", model.Wx_appid);
            data.SetValue("mch_id", model.Wx_partnerid);
            data.SetValue("long_url", url);
            data.SetValue("nonce_str", config.MCHID);
            data.SetValue("sign", new WxPayData().MakeSign(config));

            WxPayData shorturl = WxPayApi.ShortUrl(data, config);
            if (shorturl.GetValue("return_code").ToString() == "SUCCESS")
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = shorturl.GetValue("short_url").ToString(), qrcodeurl = shorturl.GetValue("short_url").ToString() });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "短连接转换失败" });
            }  
        }


        public static string getadlist(int pageindex, int pagesize, int applystate, int comid,string key)
        {

            var totalcount = 0;
            try
            {

                var actdata = new WxAdData();
                var list = actdata.Getwxadpagelist(pageindex, pagesize, comid, out totalcount, key, applystate);


                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = list });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


        public static string Editwxad(Wxad adinfo)
        {

            try
            {

                var actdata = new WxAdData();
                var pro = actdata.Editwxad(adinfo);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string Getwxad(int id,int comid)
        {

            try
            {

                var actdata = new WxAdData();
                var pro = actdata.Getwxad(id, comid);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }



        public static string DelWxad(int id, int comid)
        {

            try
            {

                var actdata = new WxAdData();
                var pro = actdata.DelWxad(id, comid);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string Wxadaddcount(int id, int comid, int vadd, int ladd)
        {

            try
            {

                var actdata = new WxAdData();
                var pro = actdata.Wxadaddcount(id, comid, vadd, ladd);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }




        public static string Getwxadimagespagelist(int pageindex, int pagesize, int comid, int aid,string key="")
        {

            var totalcount = 0;
            try
            {

                var actdata = new WxAdImagesData();
                var list = actdata.Getwxadimagespagelist(pageindex, pagesize, comid,aid, out totalcount, key);


                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = list });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


        public static string Editwxadimage(WxAdImages adinfo)
        {

            try
            {

                var actdata = new WxAdImagesData();
                var pro = actdata.Editwxadimage(adinfo);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string Getwxadimages(int id, int aid)
        {

            try
            {

                var actdata = new WxAdImagesData();
                var pro = actdata.Getwxadimages(id, aid);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }



        public static string DelWxadimages(int id, int adid)
        {

            try
            {

                var actdata = new WxAdImagesData();
                var pro = actdata.DelWxadimages(id, adid);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string upWxadimages_sort(string menuids)
        {

            if (menuids == "")
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "没有排序的元素" });
            }
            else
            {
                string[] str = menuids.Split(',');

                string err = "";
                var actdata = new WxAdImagesData();
                for (int i = 1; i <= str.Length; i++)
                {
                    string menuid = str[i - 1];
                    int sortid = i;
                    var pro = actdata.upWxadimages_sort(int.Parse(menuid), sortid);
                    if (pro == 0)
                    {
                        err += menuid + "err;";

                    }
                }
                if (err == "")
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = "排序成功" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = err });
                }
            }

        }

    }

    public class InternalClass
    {
        public string kf_account;
        public string kf_nick;
        public int kf_id;
    }
    public class OuterClass
    {
        public List<InternalClass> kf_list;
    }
}
