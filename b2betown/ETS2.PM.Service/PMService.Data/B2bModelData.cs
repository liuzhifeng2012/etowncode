using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class B2bModelData
    {

        //模板列表
        public List<B2b_model> ModelPageList(int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bModel(helper).ModelPageList(pageindex, pagesize, out totalcount);
                return list;
            }
        }

        //菜单列表
        public List<B2b_modelmenu> ModelMenuPageList(int modelid,int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bModel(helper).ModelMenuPageList(modelid, pageindex, pagesize, out totalcount);
                return list;
            }
        }


        //指定菜单列表
        public List<B2b_modelmenu> ModelzhidingPageList(int id,int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bModel(helper).ModelzhidingPageList(id,pageindex, pagesize, out totalcount);
                return list;
            }
        }


        #region 编辑模板信息
        public int ModelInsertOrUpdate(B2b_model model)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bModel(sql);
                    int result = internalData.ModelInsertOrUpdate(model);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 删除已选择模板
        public int DeleteSelectModel(int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bModel(sql);
                    int result = internalData.DeleteSelectModel(comid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 删除已选择模板菜单
        public int DeleteSelectModelMenu(int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bModel(sql);
                    int result = internalData.DeleteSelectModelMenu(comid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 插入模板菜单
        public int InsertSelectModelMenu(int modelid,int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bModel(sql);
                    int result = internalData.InsertSelectModelMenu(modelid, comid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 选择模板
        public int SelectModel(H5_html model)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bModel(sql);
                    int result = internalData.SelectModel(model);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 选择模板
        public H5_html SelectModelSearchComid(int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bModel(sql);
                    var result = internalData.SelectModelSearchComid(comid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 选择模板
        public int UseComidModel(int comid,int modelid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bModel(sql);
                    var result = internalData.UseComidModel(comid, modelid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 编辑模板菜单信息
        public int ModelMenuInsertOrUpdate(B2b_modelmenu modelmenu)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bModel(sql);
                    int result = internalData.ModelMenuInsertOrUpdate(modelmenu);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        //获得一个模板
        public B2b_model GetModelById(int modelid)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalB2bModel(helper).GetModelById(modelid);

                return pro;
            }
        }


        //获得一个菜单
        public B2b_modelmenu GetModelMenuById(int id)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalB2bModel(helper).GetModelMenuById(id);

                return pro;
            }
        }


        //获得一个菜单
        public H5_html GetComModel(int comid)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalB2bModel(helper).GetComModel(comid);

                return pro;
            }
        }

        //排序
        public int ModelMenuSort(string id, int sortid)
        {
            using (var helper = new SqlHelper())
            {
                var ret = new InternalB2bModel(helper).ModelMenuSort(id, sortid);
                return ret;
            }
        }

    }
}
