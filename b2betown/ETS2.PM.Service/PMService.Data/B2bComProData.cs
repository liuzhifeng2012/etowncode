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
    public class B2bComProData
    {
        #region 编辑产品信息
        public int InsertOrUpdate(B2b_com_pro product)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bComPro(sql);
                    int result = internalData.InsertOrUpdate(product);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 更新产品时同时更新帮 导入 的产品信息
        public int UpBindingProInsertOrUpdate(B2b_com_pro product)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bComPro(sql);
                    int result = internalData.UpBindingProInsertOrUpdate(product);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion
        #region 更新产品时同时更新帮 导入 的产品信息
        public int UpBindingProState(int proid, int state)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bComPro(sql);
                    int result = internalData.UpBindingProState(proid, state);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 更新产品时同时更新帮 导入 的产品 分销价(成本价)
        public int UpBindingProUpdatePrice(B2b_com_pro product)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bComPro(sql);
                    int result = internalData.UpBindingProInsertOrUpdate(product);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 修改产品服务信息
        public int ModifyProExt(string proid, string service_Contain, string service_NotContain, string Precautions, string Sms)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bComPro(sql);
                    int result = internalData.ModifyProExt(proid, service_Contain, service_NotContain, Precautions, Sms);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion
        #region 获取特定公司的产品列表
        //public List<B2b_com_pro> ProPageList(string comid, int pageindex, int pagesize, out int totalcount)
        //{
        //    using (var helper = new SqlHelper())
        //    {

        //        var list = new InternalB2bComPro(helper).ProPageList(comid, pageindex, pagesize, out totalcount);

        //        return list;
        //    }
        //}
        #endregion
        #region 产品排序列表
        public List<B2b_com_pro> sortlist(int comid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bComPro(helper).sortlist(comid, out totalcount);

                return list;
            }
        }
        #endregion
        public int SortMenu(string menuid, int sortid)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalB2bComPro(helper).SortMenu(menuid, sortid);
                return id;
            }
        }
        #region 获取特定公司的产品列表
        public List<B2b_eticket> SearchPnoPageList(string comid, int pageindex, int pagesize, int pro_id, int statetype, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bComPro(helper).SearchPnoPageList(comid, pageindex, pagesize, pro_id, statetype, out totalcount);

                return list;
            }
        }
        #endregion

        #region 获取库存电子票数量
        public int ProSEPageCount(int proid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bComPro(sql);
                    int result = internalData.ProSEPageCount(proid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 获取库存电子票数量，未发送
        public int ProSEPageCount_UNUse(int proid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bComPro(sql);
                    int result = internalData.ProSEPageCount_UNUse(proid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 获取库存电子票数量，已发送
        public int ProSEPageCount_Use(int proid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bComPro(sql);
                    int result = internalData.ProSEPageCount_Use(proid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 获取库存电子票数量，作废
        public int ProSEPageCount_Con(int proid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bComPro(sql);
                    int result = internalData.ProSEPageCount_Con(proid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 查询
        public List<B2b_com_pro> Selectpagelist(string comid, int pageindex, int pagesize, string key, out int totalcount, int projectid = 0, int proclass = 0, int menuid = 0, int consultantid = 0, int channelid = 0, string channelphone = "", int allview = 0, string pno = "", int Servertype = 0)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bComPro(helper).Selectpagelist(comid, pageindex, pagesize, key, out totalcount, projectid, proclass, menuid, consultantid, channelid, channelphone, allview, pno, Servertype);

                return list;
            }
        }
        #endregion


        #region 查询
        public List<B2b_com_pro> TopPropagelist(string comid, int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bComPro(helper).TopPropagelist(comid, pageindex, pagesize, out totalcount);

                return list;
            }
        }
        #endregion



        #region 查询
        public List<B2b_com_pro> Selectpagelist_diaoyong(string comid, int pageindex, int pagesize, string key, out int totalcount, int projectid = 0, int proclass = 0, int menuid = 0, int consultantid = 0, int channelid = 0, string channelphone = "")
        {
            using (var helper = new SqlHelper())
            {

                //判断栏目中是否选择具体子项目否则 按项目查询产品
                var menudata = new B2bCompanyMenuData();
                var menuinfo = menudata.selectoucountmenu_pro(int.Parse(comid), menuid);
                if (menuinfo > 0)
                {
                    projectid = 0;
                }
                else
                {
                    menuid = 0;
                }

                var list = Selectpagelist(comid, pageindex, pagesize, key, out totalcount, projectid, proclass, menuid, consultantid, channelid, channelphone);

                return list;
            }
        }
        #endregion

        #region 查询是否选择了产品
        public int Selectpagelist_ct(string comid, int menuid)
        {
            using (var helper = new SqlHelper())
            {

                //判断栏目中是否选择具体子项目否则 按项目查询产品
                var menudata = new B2bCompanyMenuData();
                var menuinfo = menudata.selectoucountmenu_pro(int.Parse(comid), menuid);
                if (menuinfo > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }


            }
        }
        #endregion

        #region 获取电子票总验证数量
        public int ProYanzhengCount(int proid, DateTime startime, DateTime endtime, int all = 0)
        {
            int bindingagentid = 0;
            using (var sql = new SqlHelper())
            {

                try
                {

                    var internalData = new InternalB2bComPro(sql);
                    //如果是导入产品则查询原始产品ID
                    var proinfo = new B2bComProData().GetProById(proid.ToString());
                    if (proinfo != null)
                    {

                        if (proinfo.Bindingid != 0)
                        {
                            proid = proinfo.Bindingid;

                            var cominfo = B2bCompanyData.GetCompany(proinfo.Com_id);
                            if (cominfo != null)
                            {
                                bindingagentid = cominfo.Bindingagent;
                            }
                        }

                        //如果是酒店业务，查询订单数量
                        if (proinfo.Server_type == 9)
                        {
                            int result1 = internalData.ProHotelYanzhengCount(proid, startime, endtime, 0, bindingagentid);
                            return result1;
                        }
                    }

                    //默认查询验票数据
                    int result = internalData.ProYanzhengCount(proid, startime, endtime, all, bindingagentid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 获取电子票总验证数量
        public int ProYanzhengCountbyProjectid(int comid, int projectid, DateTime startime, DateTime endtime, int all = 0)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bComPro(sql);
                    int result = internalData.ProYanzhengCountbyProjectid(comid, projectid, startime, endtime, all);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 获取电子票总验证数量
        public int ProHotelYanzhengCountbyProjectid(int comid, int projectid, DateTime startime, DateTime endtime, int all = 0)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bComPro(sql);
                    int result = internalData.ProHotelYanzhengCountbyProjectid(comid, projectid, startime, endtime, all);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 查询判断项目下所有产品是否为订房，如果是前台做跳转
        public int Selectpro_hotel(int comid, int projectid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bComPro(helper).Selectpro_hotel(comid, projectid);

                return list;
            }
        }
        #endregion
        public B2b_com_pro GetProById(string proid, int Speciid = 0, int channelcoachid = 0)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalB2bComPro(helper).GetProById(proid, Speciid, channelcoachid);

                return pro;
            }
        }
        public int GetProyouxiaoqiById(int proid)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalB2bComPro(helper).GetProyouxiaoqiById(proid);

                return pro;
            }
        }



        public B2b_com_pro GetProspeciidById(string proid, int Speciid)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalB2bComPro(helper).GetProspeciidById(proid, Speciid);

                return pro;
            }
        }

        public int GetTopProImageById(int comid)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalB2bComPro(helper).GetTopProImageById(comid);

                return pro;
            }
        }


        public int GetProSource_typeById(string proid)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalB2bComPro(helper).GetProSource_typeById(proid);

                return pro;
            }
        }


        public int GetProServer_typeById(string proid)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalB2bComPro(helper).GetProServer_typeById(proid);

                return pro;
            }
        }

        public string GetProRecerceSMSpeopleById(string proid)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalB2bComPro(helper).GetProRecerceSMSpeopleById(proid);

                return pro;
            }
        }


        public string GetTravelEndingByLineid(string proid)
        {
            using (var helper = new SqlHelper())
            {

                var result = new InternalB2bComPro(helper).GetTravelEndingByLineid(int.Parse(proid));

                return result;
            }
        }
        public B2b_com_pro ClientGetProById(string proid, int comid)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalB2bComPro(helper).ClientGetProById(proid, comid);

                return pro;
            }
        }

        #region 根据产品id得到商家 id
        public int GetComidByProid(int proid)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalB2bComPro(helper).GetProById(proid.ToString());
                if (pro != null)
                {
                    return pro.Com_id;
                }
                else
                {
                    return 0;

                }
            }
        }
        #endregion

        public int ModifyProState(int proid, int prostate)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bComPro(sql);
                    int result = internalData.ModifyProState(proid, prostate);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public int LuruEticket(int proid, string key_temp, int comid, int pnum)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bComPro(sql);
                    int result = internalData.LuruEticket(proid, key_temp, comid, pnum);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public int ZuofeiEticket(int proid, string key_temp, int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bComPro(sql);
                    int result = internalData.ZuofeiEticket(proid, key_temp, comid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public string ReaderTop1Eticket(int proid, int comid, int num, int order_no, out string pnostr)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bComPro(sql);
                    string result = internalData.ReaderTop1Eticket(proid, comid, num, order_no, out  pnostr);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        //先判断是否有此订单的电子码已经读取了，防止重复读取，如果已读取直接读取此电子码
        public string SearchTop1Eticket(int proid, int comid, int num, int order_no, out string pnostr)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bComPro(sql);
                    string result = internalData.SearchTop1Eticket(proid, comid, num, order_no, out  pnostr);
                    return result;
                }
                catch
                {
                    throw;

                }
            }
        }


        public string SearchEticket(int comid, string key_temp)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bComPro(sql);
                    string result = internalData.SearchEticket(comid, key_temp);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public string SearchWeishiyongEticket(int comid, string key_temp, out int countnum)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bComPro(sql);
                    string result = internalData.SearchWeishiyongEticket(comid, key_temp, out countnum);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public List<B2b_com_pro> ProPageList(string comid, int pageindex, int pagesize, int prostate, out int totalcount, int projectid = 0, string key = "", string viewmethod = "", int canviewpro = 0, int userid = 0, int servertype = 0)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bComPro(helper).ProPageList(comid, pageindex, pagesize, prostate, out totalcount, projectid, key, viewmethod, canviewpro, userid, servertype);

                return list;
            }
        }

        #region 最大 最小价格
        public static string Pro_max(int comid)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalB2bComPro(helper).Pro_max(comid);

                return pro;
            }
        }
        #endregion

        public List<B2b_com_pro> statepagelist(int comid, int pageindex, int pagesize, int state, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bComPro(helper).statepagelist(comid, pageindex, pagesize, state, out totalcount);

                return list;
            }
        }


        public B2b_com_pro GetProByOrderID(int orderid)
        {
            using (var helper = new SqlHelper())
            {

                var model = new InternalB2bComPro(helper).GetProByOrderID(orderid);

                return model;
            }
        }
        //产品分类列表
        public List<B2b_com_class> Proclasslist(int pageindex, int pagesize, out int totalcount, int industryid = 0)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bComPro(helper).Proclasslist(pageindex, pagesize, out totalcount, industryid);
                return list;

            }
        }
        //产品分类列表
        public B2b_com_class Proclassbyid(int classid)
        {
            using (var helper = new SqlHelper())
            {

                var model = new InternalB2bComPro(helper).Proclassbyid(classid);
                return model;

            }
        }


        //产品分类列表
        public int Proclassmanage(int classid, string classname, int industryid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bComPro(sql);
                    int result = internalData.Proclassmanage(classid, classname, industryid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        //产品分类列表
        public int Proclassdel(int classid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bComPro(sql);
                    int result = internalData.Proclassdel(classid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }


        //根据产品名称查询所属类目
        public int Searchproclassbyid(int proid)
        {
            using (var helper = new SqlHelper())
            {

                var model = new InternalB2bComPro(helper).Searchproclassbyid(proid);
                return model;

            }
        }

        //根据产品ID得到所属分类名称
        public string Searchproclassnamebyid(int proid)
        {
            using (var helper = new SqlHelper())
            {

                var model = new InternalB2bComPro(helper).Searchproclassnamebyid(proid);
                return model;

            }
        }

        //根据产品名称查询所属类目
        public int upproclass(int proid, int proclass, int comid)
        {
            using (var helper = new SqlHelper())
            {

                var model = new InternalB2bComPro(helper).upproclass(proid, proclass, comid);
                return model;

            }
        }


        public string GetLowerPriceByProjectId(int projectid)
        {
            using (var helper = new SqlHelper())
            {
                string result = new InternalB2bComPro(helper).GetLowerPriceByProjectId(projectid);
                return result;
            }
        }
        /// <summary>
        /// 修改公司下产品的项目id
        /// </summary>
        /// <param name="comid"></param>
        /// <param name="result1"></param>
        /// <returns></returns>
        public int UpProjectId(string comid, int projectid)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalB2bComPro(helper).UpProjectId(comid, projectid);
                return result;
            }
        }


        public List<B2b_com_pro> GetHouseTypePageList(int pageindex, int pagesize, int comid, out int totalcount, int proid = 0, int projectid = 0, int paichuproid = 0)
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_com_pro> r = new InternalB2bComPro(helper).GetHouseTypePageList(pageindex, pagesize, comid, out totalcount, proid, projectid, paichuproid);
                return r;
            }
        }
        /*减少可销售数量，同时增加已销售数量*/
        public int ReduceLimittotalnum(int proid, int ordernum)
        {
            using (var helper = new SqlHelper())
            {
                int rr = new InternalB2bComPro(helper).ReduceLimittotalnum(proid, ordernum);
                return rr;
            }
        }

        /*减少可规格库存数量*/
        public int ReduceLimittotalSpeciidnum(int proid, int Speciid, int ordernum)
        {
            using (var helper = new SqlHelper())
            {
                int rr = new InternalB2bComPro(helper).ReduceLimittotalSpeciidnum(proid, Speciid, ordernum);
                return rr;
            }
        }
        /*判断是否是抢购产品*/
        public bool Ispanicbuypro(int proid)
        {
            using (var helper = new SqlHelper())
            {
                bool rr = new InternalB2bComPro(helper).Ispanicbuypro(proid);
                return rr;
            }
        }
        /// <summary>
        /// 票务产品，判断 是否抢购/限购，是的话 作废超时未支付订单，完成回滚操作
        /// </summary>
        /// <param name="proid"></param>
        /// <returns></returns>
        public int CancelOvertimeOrder(B2b_com_pro pro)
        {
            using (var helper = new SqlHelper())
            {
                if (pro != null)
                {
                    int rr = new InternalB2bComPro(helper).CancelOvertimeOrder(pro);
                    return rr;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 对单独订单进行库存回滚
        /// </summary>
        /// <param name="proid"></param>
        /// <returns></returns>
        public int BackOrdernum(int proid, int num)
        {
            using (var helper = new SqlHelper())
            {
                int rr = new InternalB2bComPro(helper).BackOrdernum(proid, num);
                return rr;
            }
        }


        /// <summary>
        /// 对规格库存回滚
        /// </summary>
        /// <param name="proid"></param>
        /// <returns></returns>
        public int BackSpeciOrdernum(int proid, int Speciid, int num)
        {
            using (var helper = new SqlHelper())
            {
                int rr = new InternalB2bComPro(helper).BackSpeciOrdernum(proid, Speciid, num);
                return rr;
            }
        }
        /// <summary>
        /// 判断产品是否有效：1.票务、实物、保险，直接判断有效期 和产品上线状态2.酒店，跟团游，当地游，旅游大巴  则判断是否含有有效的房态/团期 以及产品上下线状态
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        /// <param name="dateTime"></param>
        /// <param name="dateTime_2"></param>
        /// <returns></returns>
        public int IsYouxiao(int proid, int Server_type, DateTime Pro_start, DateTime Pro_end, int pro_state, string outdate = "", int ordernum = 0)
        {
            using (var helper = new SqlHelper())
            {
                int rr = new InternalB2bComPro(helper).IsYouxiao(proid, Server_type, Pro_start, Pro_end, pro_state, outdate, ordernum);
                return rr;
            }
        }
        /// <summary>
        /// 获得产品分类详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public B2b_com_class GetB2bcomclass(int id)
        {
            using (var helper = new SqlHelper())
            {
                B2b_com_class rr = new InternalB2bComPro(helper).GetB2bcomclass(id);
                return rr;
            }
        }

        /// <summary>
        /// 前台显示产品的实际有效期，当设定独立验票时
        /// </summary>
        /// <param name="pro_start"></param>
        /// <param name="pro_end"></param>
        /// <param name="provalidatemethod"></param>
        /// <param name="appointdate"></param>
        /// <param name="iscanuseonsameday"></param>
        /// <returns></returns>
        /// 

        public string GetPro_Youxiaoqi(DateTime pro_start, DateTime pro_end, string provalidatemethod, int appointdate, int iscanuseonsameday)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalB2bComPro(helper).GetPro_Youxiaoqi(pro_start, pro_end, provalidatemethod, appointdate, iscanuseonsameday);

                return pro;
            }
        }
        /// <summary>
        /// 根据出行日期得到产品列表(服务类型相同，并且当天有团期的)
        /// </summary>
        /// <param name="daydate"></param>
        /// <param name="servertype"></param>
        /// <param name="comid"></param>
        /// <param name="orderstate"></param>
        /// <returns></returns>
        public IList<B2b_com_pro> Getb2bcomprobytraveldate(DateTime daydate, int servertype, int comid, string isSetVisitDate = "0,1")
        {
            using (var helper = new SqlHelper())
            {

                var result = new InternalB2bComPro(helper).Getb2bcomprobytraveldate(daydate, servertype, comid, isSetVisitDate);

                return result;
            }
        }
        /// <summary>
        /// 判断公司是否加了旅游大巴产品
        /// </summary>
        /// <param name="comid"></param>
        /// <returns></returns>
        public int IsHasLvyoubusPro(int comid, int servertype)
        {
            using (var helper = new SqlHelper())
            {

                var result = new InternalB2bComPro(helper).IsHasLvyoubusPro(comid, servertype);

                return result;
            }
        }



        /// <summary>
        /// 根据产品编号 得到绑定的产品的原始编号
        /// </summary>
        /// <param name="comid"></param>
        /// <returns></returns>
        public int GetOldproidById(string product_num)
        {
            using (var helper = new SqlHelper())
            {

                var result = new InternalB2bComPro(helper).GetOldproidById(product_num);

                return result;
            }
        }
        /// <summary>
        /// 产品过期自动下线
        /// </summary>
        /// <returns></returns>
        public int ProAutoDownLine()
        {
            using (var helper = new SqlHelper())
            {

                var result = new InternalB2bComPro(helper).ProAutoDownLine();

                return result;
            }
        }
        /// <summary>
        /// 根据电子码得到产品信息
        /// </summary>
        /// <param name="pno"></param>
        /// <returns></returns>
        public B2b_com_pro Getprobypno(string pno)
        {
            using (var helper = new SqlHelper())
            {

                B2b_com_pro result = new InternalB2bComPro(helper).Getprobypno(pno);

                return result;
            }
        }
        /// <summary>
        /// 根据电子码得到订单 可以预约人数
        /// </summary>
        /// <param name="pno"></param>
        /// <returns></returns>
        public int Getordercanbooknumbypno(string pno)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalB2bComPro(helper).Getordercanbooknumbypno(pno);

                return result;
            }
        }
        /// <summary>
        /// 根据电子码得到订单号
        /// </summary>
        /// <param name="pno"></param>
        /// <returns></returns>
        public int GetOrderIdByPno(string pno)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalB2bComPro(helper).GetOrderIdByPno(pno);

                return result;
            }
        }





        #region 获取电子票总验证数量
        public int ProYanzhengCountbyProjectid(int comid, int projectid, DateTime startime, DateTime endtime, int all = 0, int agentid = 0)
        {
            using (var sql = new SqlHelper())
            {
                try
                {

                    var prodata = new B2b_com_projectData();
                    int agentpro = 0;
                    var proinfo = prodata.GetProjectBindingid(projectid);
                    if (proinfo != 0)
                    {
                        agentpro = 1;

                    }

                    var internalData = new InternalB2bComPro(sql);
                    int result = internalData.ProYanzhengCountbyProjectid(comid, projectid, startime, endtime, all, agentpro, agentid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        //增加可销售数量，减少已销售数量
        public int AddLimittotalnum(int proid, int num)
        {
            using (var helper = new SqlHelper())
            {
                int rr = new InternalB2bComPro(helper).AddLimittotalnum(proid, num);
                return rr;
            }
        }
        /// <summary>
        /// 得到产品的原始产品id
        /// </summary>
        /// <param name="proid"></param>
        /// <returns></returns>
        public int GetbindingidbyProid(int proid)
        {
            using (var helper = new SqlHelper())
            {
                int rr = new InternalB2bComPro(helper).GetbindingidbyProid(proid);
                return rr;
            }
        }
        /// <summary>
        /// 得到产品的 可销售数量和已销售数量
        /// </summary>
        /// <param name="proid"></param>
        /// <param name="limitbuytotalnum"></param>
        /// <param name="buynum"></param>
        public void Getsalenum(int proid, out int limitbuytotalnum, out int buynum)
        {
            using (var helper = new SqlHelper())
            {
                new InternalB2bComPro(helper).GetbindingidbyProid(proid, out limitbuytotalnum, out buynum);

            }
        }

        public int Uplimitbuytotalnum(int proid, int upnum)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bComPro(helper).Uplimitbuytotalnum(proid, upnum);
                return r;
            }
        }

        public int EditProChildImg(int proid, string MultiImgUpIds)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bComPro(helper).EditProChildImg(proid, MultiImgUpIds);
                return r;
            }
        }

        public int DelProChildImg(int fileUploadId)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bComPro(helper).DelProChildImg(fileUploadId);
                return r;
            }
        }


        public List<B2b_com_pro> WebProPageList(string comid, int pageindex, int pagesize, int prostate, out int totalcount, int projectid = 0, string key = "", string viewmethod = "")
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bComPro(helper).WebProPageList(comid, pageindex, pagesize, prostate, out totalcount, projectid, key, viewmethod);

                return list;
            }
        }

        public int GetImgUrl(int proid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bComPro(helper).GetImgUrl(proid);

                return list;
            }
        }
        /// <summary>
        /// 获取产品是否需要预付
        /// </summary>
        /// <param name="proid"></param>
        /// <returns></returns>
        public string GetLinePro_BookType(int proid)
        {
            using (var helper = new SqlHelper())
            {

                string r = new InternalB2bComPro(helper).GetLinePro_BookType(proid);

                return r;
            }
        }
        /// <summary>
        /// //得到项目下第一个在线产品图片做为项目图片
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public int GetFirstProImgInProjectId(int projectid)
        {
            using (var helper = new SqlHelper())
            {

                int r = new InternalB2bComPro(helper).GetFirstProImgInProjectId(projectid);

                return r;
            }
        }

        public string GetProName(int proid)
        {
            using (var helper = new SqlHelper())
            {

                string r = new InternalB2bComPro(helper).GetProName(proid);

                return r;
            }
        }

        public string GetComNamebyproid(int proid)
        {
            using (var helper = new SqlHelper())
            {

                string r = new InternalB2bComPro(helper).GetComNamebyproid(proid);

                return r;
            }
        }

        public int GetSourcetypebyproid(int proid)
        {
            using (var helper = new SqlHelper())
            {

                var r = new InternalB2bComPro(helper).GetSourcetypebyproid(proid);

                return r;
            }
        }

        public IList<B2b_com_pro> GetProlistbyprojectid(int projectid, string servertypes, int topnums)
        {
            using (var helper = new SqlHelper())
            {
                var r = new InternalB2bComPro(helper).GetProlistbyprojectid(projectid, servertypes, topnums);

                return r;
            }
        }
        public int GetServiceidbyproid(int proid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bComPro(helper).GetServiceidbyproid(proid);

                return r;
            }
        }
        public string Getcoachnamebyid(int channelid)
        {
            using (var helper = new SqlHelper())
            {
                var r = new InternalB2bComPro(helper).Getcoachnamebyid(channelid);
                return r;
            }
        }
        //通过订单ID 获取产品名称
        public string Getpronamebyorderid(int orderid)
        {
            string proname = "";
            B2b_com_pro prodata;
            using (var helper = new SqlHelper())
            {
                var order = new B2bOrderData().GetOrderById(orderid);
                if (order != null)
                {
                    prodata = new B2bComProData().GetProById(order.Pro_id.ToString(), order.Speciid, order.channelcoachid);

                    if (prodata != null)
                    {
                        proname = prodata.Pro_name;
                    }
                }
            }
            return proname;
        }

        public int GetIsHzinsPro(int proid)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalB2bComPro(helper).GetIsHzinsPro(proid);
                if (result == 2)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

            }
        }
        /// <summary>
        /// 相对于慧择网来说订单类型:0 非慧择网订单；1慧择网订单 但没有生成真实保险订单；2慧择网订单 并且生成了真实保险订单
        /// </summary>
        /// <param name="proid"></param>
        /// <returns></returns>
        public int GetOrderType_Hzins(int proid, int orderid)
        {
            using (var helper = new SqlHelper())
            {
                int bindorderid = new B2bOrderData().GetBindOrderIdByOrderid(orderid);
                if (bindorderid > 0)
                {
                    int bindproid = new B2bOrderData().GetProIdbyorderid(bindorderid);
                    if (bindproid > 0)
                    {
                        orderid = bindorderid;
                        proid = bindproid;
                    }
                }


                int result = new InternalB2bComPro(helper).GetIsHzinsPro(proid);
                if (result == 2)//慧择网订单
                {
                    //根据订单号得到投保单号
                    string insureNo = new Api_hzins_OrderApplyResp_OrderExtData().GetinsureNumbyorderid(orderid);//投保单号
                    if (insureNo == "")
                    {
                        return 1;
                    }
                    else
                    {
                        return 2;
                    }
                }
                else//非慧择网订单
                {
                    return 0;
                }

            }
        }

        public int GetSourcetypeByOrderid(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                int sourtype = new InternalB2bComPro(helper).GetSourcetypeByOrderid(orderid);
                return sourtype;
            }

        }

        public IList<B2b_com_pro> Getbaoxianlist(int comid)
        {
            using (var helper = new SqlHelper())
            {
                IList<B2b_com_pro> r = new InternalB2bComPro(helper).Getbaoxianlist(comid);
                return r;
            }
        }
        /// <summary>
        /// 得到产品下赠送保险id
        /// </summary>
        /// <param name="border_proid"></param>
        /// <returns></returns>
        public int GetSelbindbx(int proid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bComPro(helper).GetSelbindbx(proid);
                return r;
            }
        }

        /// <summary>
        /// 得到美团 poi（project） 最大的增量id
        /// </summary>
        /// <returns></returns>
        public int GetMeituanMaxPoiid()
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bComPro(helper).GetMeituanMaxPoiid();
                return r;
            }
        }
        /// <summary>
        /// 得到美团 poi（project） 下次增量id
        /// </summary>
        /// <returns></returns>
        public int GetMeituanPoiNextIncrementId(int projectid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bComPro(helper).GetMeituanPoiNextIncrementId(projectid);
                return r;
            }
        }

        public int GetMeituanMaxProid()
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bComPro(helper).GetMeituanMaxProid();
                return r;
            }
        }

        public int GetMeituanProNextIncrementId(int proid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bComPro(helper).GetMeituanProNextIncrementId(proid);
                return r;
            }
        }

        public List<B2b_com_pro> GetAgentProList(out int totalcount, int agentid, string method, string productids, int pageindex, int pagesize)
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_com_pro> r = new InternalB2bComPro(helper).GetAgentProList(out totalcount, agentid, method, productids, pageindex, pagesize);
                return r;
            }
        }

        public IList<B2b_com_pro> GetWxProlistbyprojectid(int projectid, string servertypes, int topnums)
        {
            using (var helper = new SqlHelper())
            {
                var r = new InternalB2bComPro(helper).GetWxProlistbyprojectid(projectid, servertypes, topnums);

                return r;
            }
        }

        public int GetServertypeByPno(string pno)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bComPro(helper).GetServertypeByPno(pno);

                return r;
            }
        }

        public string GetProBindPosid(int proid)
        {
            using (var helper = new SqlHelper())
            {
                string r = new InternalB2bComPro(helper).GetProBindPosid(proid);

                return r;
            }
        }

        public string GetProjectNameByProid(string proid)
        {
            using (var helper = new SqlHelper())
            {
                string r = new InternalB2bComPro(helper).GetProjectNameByProid(proid);

                return r;
            }
        }

        /// <summary>
        /// 获得酒店产品的精简信息(proid,proname)
        /// </summary>
        /// <param name="comid"></param>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public List<B2b_com_pro> Selhotelproductlist(int comid, int projectid)
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_com_pro> r = new InternalB2bComPro(helper).Selhotelproductlist(comid, projectid);

                return r;
            }
        }

        /// <summary>
        /// 回滚原产品 和 导入原产品的产品的库存
        /// </summary>
        /// <param name="proid"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public int RollbackProKucun(int proid, int num, string traveldate = "", string enddate = "")
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bComPro(helper).RollbackProKucun(proid, num, traveldate, enddate);

                return r;
            }
        }

        public int GetServertypeByProid(int proid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bComPro(helper).GetServertypeByProid(proid);

                return r;
            }
        }

        public int GetLimitbuytotalnum(int proid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bComPro(helper).GetLimitbuytotalnum(proid);

                return r;
            }
        }

        public List<B2b_com_pro_group> GetProgrouplistByComid(int comid, string runstate = "0,1")
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_com_pro_group> r = new InternalB2bComPro(helper).GetProgrouplistByComid(comid, runstate);

                return r;
            }
        }

        public List<B2b_com_pro_group> GetProgroupPagelistByComid(int comid, int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_com_pro_group> r = new InternalB2bComPro(helper).GetProgroupPagelistByComid(comid, pageindex, pagesize, out totalcount);

                return r;
            }
        }

        public int Editprogroup(B2b_com_pro_group m)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2bComPro(helper).Editprogroup(m);
                return r;
            }
        }



        public List<B2b_com_pro_Package> GetProPackagPagelistByid(int pid, int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_com_pro_Package> r = new InternalB2bComPro(helper).GetProPackagPagelistByid(pid, pageindex, pagesize, out totalcount);

                return r;
            }
        }


        /// <summary>
        /// 根据id等到绑定产品编号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public B2b_com_pro_Package GetProPackagbyid(int id)
        {
            using (var helper = new SqlHelper())
            {

                var result = new InternalB2bComPro(helper).GetProPackagbyid(id);

                return result;
            }
        }


        #region 编辑套票绑定产品
        public int ProPackageInsertOrUpdate(B2b_com_pro_Package product)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bComPro(sql);
                    int result = internalData.ProPackageInsertOrUpdate(product);
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
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DelProPackagbyid(int id)
        {
            using (var helper = new SqlHelper())
            {

                var result = new InternalB2bComPro(helper).DelProPackagbyid(id);

                return result;
            }
        }


        /// <summary>
        /// 计算此酒店房间入住的结算价
        /// </summary>
        /// <param name="star"></param>
        /// <param name="end"></param>
        /// <param name="proid"></param>
        /// <returns></returns>
        public decimal jisuan_hotel_jiesuanjia(int comid, int proid, string stardate, string enddate)
        {
            using (var helper = new SqlHelper())
            {
                var r = new InternalB2bComPro(helper).jisuan_hotel_jiesuanjia(comid, proid, stardate, enddate);//查询此产品的 此时间的

                return r;
            }
        }




        /// <summary>
        /// 得到分销下没有上架的产品列表
        /// </summary>
        /// <param name="comid"></param>
        /// <param name="agentid"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public List<B2b_com_pro> GetNotStockProPagelist(int comid, int agentid, int agentlevel, int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bComPro(helper).GetNotStockProPagelist(comid, agentid, agentlevel, pageindex, pagesize, out totalcount);

                return list;
            }
        }
        /// <summary>
        /// 根据原始产品id得到绑定产品id列表
        /// </summary>
        /// <param name="proid"></param>
        /// <returns></returns>
        public List<int> Getbindingproidlist(int proid)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2bComPro(helper).Getbindingproidlist(proid);

                return list;
            }
        }

        public List<int> GetAutoDownlineProlist()
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2bComPro(helper).GetAutoDownlineProlist();
                return list;
            }
        }
        /// <summary>
        /// 根据产品id 和 分销级别 获得分销价格，暂时只是求得票务产品的；
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        public decimal GetAgentPrice(int proid, int agentlevel)
        {
            using (var helper = new SqlHelper())
            {
                decimal r = new InternalB2bComPro(helper).GetAgentPrice(proid, agentlevel);
                return r;
            }
        }
        /// <summary>
        /// 得到产品的前topnum个子图片id
        /// </summary>
        /// <param name="proid"></param>
        /// <param name="topnum"></param>
        /// <returns></returns>
        public List<int> GetProChildImgArr(int proid, int topnum)
        {
            using (var helper = new SqlHelper())
            {
                List<int> arr = new InternalB2bComPro(helper).GetProChildImgArr(proid, topnum);
                return arr;
            }
        }


        public List<B2b_com_pro_bandingzhajipos> GetProbandingzhajilistByproid(int comid, int proid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                var rt = new InternalB2bComPro(helper).GetProbandingzhajilistByproid(comid, proid, out totalcount);

                return rt;
            }
        }

        public List<B2b_com_pro_bandingzhajipos> GetProbandingzhajilistByproidposid(int comid, int proid, string pos_id, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                var rt = new InternalB2bComPro(helper).GetProbandingzhajilistByproidposid(comid, proid, pos_id, out totalcount);

                return rt;
            }
        }



        /// <summary>
        /// 添加修改
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int ProbandingzhajiposInsertOrUpdate(B2b_com_pro_bandingzhajipos prozhaji)
        {
            using (var helper = new SqlHelper())
            {

                var result = new InternalB2bComPro(helper).ProbandingzhajiposInsertOrUpdate(prozhaji);

                return result;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Probandingzhajiposdel(int comid, int proid)
        {
            using (var helper = new SqlHelper())
            {

                var result = new InternalB2bComPro(helper).Probandingzhajiposdel(comid, proid);

                return result;
            }
        }

        //闸机日志
        public List<Rentserver_User_zhajilog> GetRentserver_User_zhajilogByuid(int comid, int Rentserver_Userid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                var rt = new InternalB2bComPro(helper).GetRentserver_User_zhajilogByuid(comid, Rentserver_Userid, out totalcount);

                return rt;
            }
        }

        /// <summary>
        /// 闸机日志添加修改
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Rentserver_User_zhajilogInsertOrUpdate(Rentserver_User_zhajilog prozhaji)
        {
            using (var helper = new SqlHelper())
            {

                var result = new InternalB2bComPro(helper).Rentserver_User_zhajilogInsertOrUpdate(prozhaji);

                return result;
            }
        }


    }
}
