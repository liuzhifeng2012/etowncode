using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ETS2.Common.Business;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;
using System.Web;
using ETS.Data.SqlHelper;
using System.Collections;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS.JsonFactory
{
    /// <summary>
    /// 模板管理
    /// </summary>
    public class ModelJsonData
    {
        #region 获取模板列表
        public static string ModelPageList(int pageindex, int pagesize,int comid=0)
        {
            int totalcount = 0;
            try
            {
                B2bModelData modedata = new B2bModelData();
                var list = modedata.ModelPageList(pageindex, pagesize, out totalcount);
                if (list == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "没有查询到模板", totalCount=0 });
                }

                IEnumerable result = "";
                if (list != null)
                {
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Title = pro.Title,
                                 Bgimage = FileSerivce.GetImgUrl(pro.Bgimage),
                                 Smallimg = FileSerivce.GetImgUrl(pro.Smallimg),
                                 Style_str = pro.Style_str,
                                 Html_str = pro.Html_str,
                                 Daohangnum = pro.Daohangnum,
                                 Daohangimg = pro.Daohangimg,
                                 Bgimage_h = pro.Bgimage_h,
                                 Bgimage_w = pro.Bgimage_w,
                                 Usestate = modedata.UseComidModel(comid, pro.Id)
                             };
                }


                return JsonConvert.SerializeObject(new { type = 100, msg = result, totalCount = totalcount });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion



        #region 获取一个模板
        public static string GetModelById(int modelid)
        {
            try
            {
                B2bModelData modedata = new B2bModelData();
                var list = modedata.GetModelById(modelid);
                if (list == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "没有查询到列表", totalCount = 0 });
                }
                return JsonConvert.SerializeObject(new { type = 100, msg = list});
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion



        #region 获取模板栏目
        public static string ModelMenuPageList(int modelid ,int pageindex, int pagesize)
        {
            int totalcount = 0;
            try
            {
                B2bModelData modedata = new B2bModelData();
                var list = modedata.ModelMenuPageList(modelid,pageindex, pagesize, out totalcount);
                if (list == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "没有查询到模板", totalCount = 0 });
                }

                IEnumerable result = "";
                if (list != null)
                {
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Name = pro.Name,
                                 Imgurl_address = FileSerivce.GetImgUrl(pro.Imgurl),
                                 Imgurl = pro.Imgurl,
                                 Linkurl = pro.Linkurl,
                                 Modelid = pro.Modelid,
                                 Sortid = pro.Sortid,
                                 Fonticon = pro.Fonticon
                             };
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = result, totalCount = totalcount });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 获取指定栏目
        public static string ModelzhidingPageList(int id, int pageindex, int pagesize)
        {
            int totalcount = 0;
            try
            {
                B2bModelData modedata = new B2bModelData();
                var list = modedata.ModelzhidingPageList(id,pageindex, pagesize, out totalcount);
                if (list == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "没有查询到模板", totalCount = 0 });
                }
                return JsonConvert.SerializeObject(new { type = 100, msg = list, totalCount = totalcount });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion



        #region 获取一个模板菜单
        public static string GetModelMenuById(int id)
        {
            try
            {
                B2bModelData modedata = new B2bModelData();
                var list = modedata.GetModelMenuById(id);
                if (list == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "没有查询到菜单", totalCount = 0 });
                }
                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        /// <summary>
        ///  模板修改
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string ModelInsertOrUpdate(B2b_model model)
        {
            using (var helper = new SqlHelper())
            {
                try
                {
                    B2bModelData modeldata = new B2bModelData();

                    int orderid = modeldata.ModelInsertOrUpdate(model);
                    return JsonConvert.SerializeObject(new { type = 100, msg = orderid });
                }
                catch (Exception ex)
                {
                    helper.Rollback();
                    return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                }
            }
        }

        /// <summary>
        ///  选定模板
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string SelectModel(int modelid,int comid)
        {
            using (var helper = new SqlHelper())
            {
                int totalcount = 0;
                int totalcount1=0;
                try
                {
                    B2bModelData modeldata = new B2bModelData();
                    var b2bmodel = modeldata.GetModelById(modelid);
                    var b2bmodelmenu = modeldata.ModelMenuPageList(modelid,1,100,out totalcount);

                    if (b2bmodel != null)
                    {
                        var delemodel = modeldata.DeleteSelectModel(comid);//删除已选择模板

                        H5_html model = new H5_html();
                        model.Comid = comid;
                        model.Modelid = modelid;
                        model.Style_str = b2bmodel.Style_str;
                        model.Html_str = b2bmodel.Html_str;

                        //插入模板
                        int orderid = modeldata.SelectModel(model);

                        //原有图片都下线
                        var saledata = new B2bCompanyImageData();
                        var bannerlist = saledata.UpAllDownState(comid);


                        //插入默认banner（也用于背景图片），重新选择模板后可能重复插入
                        B2bCompanyImageData modedata = new B2bCompanyImageData();
                        var imglist = modedata.GettypemodelidimageLibraryList(1, modelid, 1, 3, out totalcount1);
                        if (imglist != null)
                        {
                            for (int i = 0; i < imglist.Count; i++)
                            {
                                B2b_company_image imagemodel = new B2b_company_image()
                                {
                                    Id = 0,
                                    Com_id = comid,
                                    Typeid = 0,
                                    Imgurl = imglist[i].Imgurl,
                                    Linkurl = "#",
                                    Title = "#",
                                    Channelcompanyid = 0,
                                };
                                var crmid = saledata.InsertOrUpdate(imagemodel);
                            }
                        }


                        if (b2bmodelmenu != null)//首先判断模板里是否有默认栏目
                        {
                            var insertmenu = 0;
                            var imagedata = new B2bCompanyMenuData();
                            var list = imagedata.GetMenuList(comid, 1, 10, out totalcount);//如果已经有菜单的模板将不删除菜单，以后可增加判断是否删除菜单，有利于方便切换模板导航不需要重复修改，但容易出现因栏目图片不符新模板而影响使用。可先删除已有模板再进行切换

                            if (totalcount == 0)
                            {
                                insertmenu = modeldata.InsertSelectModelMenu(modelid, comid);//插入已选择模板菜单
                            }

                            //var deletemenu= modeldata.DeleteSelectModelMenu(comid);//删除已选择模板菜单
                            return JsonConvert.SerializeObject(new { type = 100, msg = insertmenu });
                        }
                    }

                    return JsonConvert.SerializeObject(new { type = 1, msg = "选择模板错误，请重新尝试" });

                }
                catch (Exception ex)
                {
                    helper.Rollback();
                    return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                }
            }
        }

        /// <summary>
        ///  模板菜单修改
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string ModelMenuInsertOrUpdate(B2b_modelmenu model)
        {
            using (var helper = new SqlHelper())
            {
                try
                {
                    B2bModelData modeldata = new B2bModelData();

                    int orderid = modeldata.ModelMenuInsertOrUpdate(model);
                    return JsonConvert.SerializeObject(new { type = 100, msg = orderid });
                }
                catch (Exception ex)
                {
                    helper.Rollback();
                    return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                }
            }
        }



        #region 判断商户是否已经使用模板，同时获得模板信息
        public static string GetComModel(int comid)
        {
            try
            {
                B2bModelData modedata = new B2bModelData();
                var list = modedata.GetComModel(comid);
                if (list == null)
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = "err", totalCount = 0 });
                }
                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        //排序
        public static string ModelMenuSort(string ids)
        {
            if (ids == "")
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "没有排序的元素" });
            }
            else
            {
                string[] str = ids.Split(',');

                string err = "";
                for (int i = 1; i <= str.Length; i++)
                {
                    string materialid = str[i - 1];
                    int sortid = i;
                    int sortmenu = new B2bModelData().ModelMenuSort(materialid, sortid);
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



        #region 获取图片库列表
        public static string GetimageLibraryList(int usetype, int pageindex, int pagesize,int modelid=0)
        {
            int totalcount = 0;
            try
            {
                B2bCompanyImageData modedata = new B2bCompanyImageData();
                var modelmodel=new B2bModelData();
                var list = modedata.GetimageLibraryList(usetype, pageindex, pagesize, out totalcount,modelid);
                if (list == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "没有查询到图片", totalCount = 0 });
                }

                IEnumerable result = "";
                if (list != null)
                {
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Imgurl_address = FileSerivce.GetImgUrl(pro.Imgurl),
                                 Imgurl = pro.Imgurl,
                                 Usetype = pro.Usetype,
                                 Width=pro.Width,
                                 Height=pro.Height,
                                 ModelName = modelmodel.GetModelById(pro.Modelid) == null ? "" : modelmodel.GetModelById(pro.Modelid).Title
                               
                             };
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = result, totalCount = totalcount });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 获取图标库列表
        public static string GetfontLibraryList(int usetype, int pageindex, int pagesize)
        {
            int totalcount = 0;
            try
            {
                B2bCompanyImageData modedata = new B2bCompanyImageData();
                var list = modedata.GetfontLibraryList(usetype, pageindex, pagesize, out totalcount);

                return JsonConvert.SerializeObject(new { type = 100, msg = list, totalCount = totalcount });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 根据模板ID获取图片库列表
        public static string GettypemodelidimageLibraryList(int usetype, int modelid, int pageindex, int pagesize)
        {
            int totalcount = 0;
            try
            {
                B2bCompanyImageData modedata = new B2bCompanyImageData();
                var list = modedata.GettypemodelidimageLibraryList(usetype, modelid, pageindex, pagesize, out totalcount);
                if (list == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "没有查询到图片", totalCount = 0 });
                }

                IEnumerable result = "";
                if (list != null)
                {
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Imgurl_address = FileSerivce.GetImgUrl(pro.Imgurl),
                                 Imgurl = pro.Imgurl,
                                 Usetype = pro.Usetype,
                                 Width = pro.Width,
                                 Height = pro.Height

                             };
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = result, bankCount = list.Count, totalCount = totalcount });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 获取图片库的图片
        public static string GetimageLibraryByid(int id)
        {
            int totalcount = 0;
            try
            {
                var list = B2bCompanyImageData.GetimageLibraryByid(id);
                if (list == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "没有查询到图片", totalCount = 0 });
                }

                IEnumerable result = "";
                if (list != null)
                {
                    list.Imgurl_address = FileSerivce.GetImgUrl(list.Imgurl);//获取图片实际地址
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = list, totalCount = totalcount });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        /// <summary>
        ///  删除图片
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string DeleteLibraryimage(int model)
        {
            using (var helper = new SqlHelper())
            {
                try
                {
                    B2bCompanyImageData modeldata = new B2bCompanyImageData();

                    int orderid = modeldata.DeleteLibraryimage(model);
                    return JsonConvert.SerializeObject(new { type = 100, msg = orderid });
                }
                catch (Exception ex)
                {
                    helper.Rollback();
                    return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                }
            }
        }


        /// <summary>
        ///  插入图片
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string LibraryInsertOrUpdate(b2b_image_library model)
        {
            using (var helper = new SqlHelper())
            {
                try
                {
                    B2bCompanyImageData modeldata = new B2bCompanyImageData();

                    int orderid = modeldata.LibraryInsertOrUpdate(model);
                    return JsonConvert.SerializeObject(new { type = 100, msg = orderid });
                }
                catch (Exception ex)
                {
                    helper.Rollback();
                    return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                }
            }
        }


    }
}
