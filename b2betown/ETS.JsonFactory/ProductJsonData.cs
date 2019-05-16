using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using Newtonsoft.Json;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data;
using System.Collections;
using ETS2.Common.Business;
using FileUpload.FileUpload.Data;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.VAS.Service.VASService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using FileUpload.FileUpload.Entities;
using System.Xml;
using ETS2.WeiXin.Service.WeiXinService.Model;
using System.Data;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS2.Member.Service.MemberService.Data;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Permision.Service.PermisionService.Data;
using ETS2.Permision.Service.PermisionService.Model;
using ETS2.PM.Service.Meituan.Data;
using ETS2.PM.Service.Meituan.Model;
using ETS2.PM.Service.WL.Data;

namespace ETS.JsonFactory
{
    public class ProductJsonData
    {
        private static object lockobj = new object();
        #region 编辑产品信息
        public static string InsertOrUpdatePro(B2b_com_pro product)
        {

            try
            {
                var prodata = new B2bComProData();
                var proid = prodata.InsertOrUpdate(product);

                #region 新建产品 抢购/限购产品库存变化日志;产品变动 同时 美团下架产品变动
                try
                {
                    if (product.Id == 0)
                    {
                        if (product.Ispanicbuy == 1 || product.Ispanicbuy == 2)
                        {
                            #region 库存变化日志
                            B2b_com_pro_kucunlog kucunlog = new B2b_com_pro_kucunlog
                            {
                                id = 0,
                                orderid = 0,
                                proid = proid,
                                servertype = product.Server_type,
                                daydate = DateTime.Parse("1970-01-01"),
                                proSpeciId = 0,
                                surplusnum = product.Limitbuytotalnum,
                                operor = "",
                                opertime = DateTime.Now,
                                opertype = "新建抢购/限购产品",
                                oper = "抢购/限购"
                            };
                            new B2b_com_pro_kucunlogData().Editkucunlog(kucunlog);
                            #endregion
                        }
                    }
                    #region 产品变动 同时 美团下架产品变动
                    else {
                         //产品下线 美团产品下架 及向美团发送下架通知
                         if(product.Pro_state==0)
                         {
                             /*******************************
                             *  如果是美团上架产品，对原始产品和导入产品进行美团下架处理 和 下架通知
                             * *****************************/
                              //判断是否是美团上架产品
                             bool isstockpro = new B2b_com_pro_groupbuystocklogData().IsStockPro(product.Id);
                             if(isstockpro)
                             {
                                 //得到上架美团绑定此产品的绑定产品列表
                                 List<int> proidlist = new B2b_com_pro_groupbuystocklogData().GetStockbindingproidlistByProid(product.Id);
                                 //加入原始产品
                                 proidlist.Add(proid);
                                 //下架操作
                                 DownStockPro(proidlist, 0);
                             }
                             
                         }
                         //产品信息变动 向美团发送产品信息变动通知 和 日历价格变动通知
                         else
                         {
                             /*******************************
                           *  如果是美团上架产品，对原始产品和导入产品 发送产品信息变动通知 和 日历价格变动通知
                           * *****************************/
                             //判断是否是美团上架产品
                             bool isstockpro = new B2b_com_pro_groupbuystocklogData().IsStockPro(product.Id);
                             if (isstockpro)
                             {
                                 //得到上架美团绑定此产品的绑定产品列表
                                 List<int> proidlist = new B2b_com_pro_groupbuystocklogData().GetStockbindingproidlistByProid(product.Id);
                                 //加入原始产品
                                 proidlist.Add(proid);
                                 //发送 产品信息变动通知 和 日历价格变动通知
                                 StockProSendChangeNotice(proidlist, 0);
                             }
                              
                         }
                    }
                    #endregion
                }
                catch { }
                #endregion

                if (proid != 0)
                {
                    //产品子图片编辑
                    if (product.MultiImgUpIds != "")
                    {
                        int editprochildimg = new B2bComProData().EditProChildImg(proid, product.MultiImgUpIds);
                    }

                    //绑定验证服务
                    if (product.Wrentserver == 1)
                    {
                        var delserver = new RentserverData().deletepro_rentserver(proid, product.Com_id);
                        if (product.Rentserverid != "")
                        {
                            var Rentserveridarr = product.Rentserverid.Split(',');
                            for (int i = 0; i < Rentserveridarr.Length; i++)
                            {
                                //先删除所有绑定

                                int sid = 0;
                                try
                                {
                                    sid = int.Parse(Rentserveridarr[i].Trim());
                                }
                                catch
                                {
                                    sid = 0;
                                }

                                //判断都不为0时 插入绑定
                                if (sid != 0 && proid != 0 && product.Com_id != 0)
                                {
                                    var insserver = new RentserverData().inpro_rentserver(proid, int.Parse(Rentserveridarr[i].Trim()), product.Com_id);
                                }
                            }
                        }

                        //绑定闸机
                        var delzhaji = new B2bComProData().Probandingzhajiposdel( product.Com_id,proid);//删除已绑定的，
                        if (product.bandingzhajiid != "")
                        {
                            var bandingzhajiidarr = product.bandingzhajiid.Split(',');
                            for (int i = 0; i < bandingzhajiidarr.Length; i++)
                            {
                                //先删除所有绑定

                                //判断都不为0时 插入绑定
                                if ( proid != 0 && product.Com_id != 0)
                                {

                                    B2b_com_pro_bandingzhajipos bandingzhajipos = new B2b_com_pro_bandingzhajipos();
                                    bandingzhajipos.comid=product.Com_id;
                                    bandingzhajipos.pos_id = bandingzhajiidarr[i].Trim();
                                    bandingzhajipos.proid = proid;

                                    var insserver = new B2bComProData().ProbandingzhajiposInsertOrUpdate(bandingzhajipos);
                                }
                            }
                        }


                    }


                    //判断产品服务类型：服务类型是旅游大巴(10),则更改团期中价格等于 直销价格(adviseprice)
                    if (product.Server_type == 10)
                    {
                        //产品价格变动记录
                        int pricelog = new B2b_com_pro_pricelogData().EditPriceLog(proid, product);

                        int upgroupdateprice = new B2b_com_LineGroupDateData().UpGroupdateprice(proid, product.Advise_price);
                    }

                    //多规格
                    if (product.Manyspeci == 1)
                    {
                        #region 非导入产品
                        if (product.Source_type != 4)
                        {
                            //把产品下的规格值都设为下线
                            int uplinestatus = new B2b_com_pro_SpecitypevalueData().UpLinestatus(proid, 0);

                            List<string> listSpcitype = new List<string>();//规格类型列表
                            List<string> listSpcivalue = new List<string>();//规格值列表
                            List<string> listSpci = new List<string>();//规格列表
                            #region 把规格类型，规格值，规格 分别提取到集合中
                            var guigearr = product.guigestr.Split('|');
                            for (int i = 0; i < guigearr.Length; i++)
                            {
                                var guigedetailarr = guigearr[i].Split('-');
                                for (int j = 0; j < guigedetailarr.Length; j++)
                                {
                                    if (j == 0)
                                    {
                                        var gg = guigedetailarr[j].Split(';');
                                        for (int y = 0; y < gg.Length; y++)
                                        {
                                            var spcitype = gg[y].Substring(0, gg[y].IndexOf(":")); //形如:尺寸
                                            if (!listSpcitype.Contains(spcitype))
                                            {
                                                listSpcitype.Add(spcitype);
                                            }
                                            var specivalL = gg[y];//形如:尺寸:1寸
                                            if (!listSpcivalue.Contains(specivalL))
                                            {
                                                listSpcivalue.Add(specivalL);
                                            }
                                        }
                                    }
                                }
                                listSpci.Add(guigearr[i]);
                            }
                            #endregion

                            #region 录入规格类型
                            for (int i = 0; i < listSpcitype.Count; i++)
                            {
                                string guigetype = listSpcitype[i];
                                B2b_com_pro_Specitype mguigetype = new B2b_com_pro_Specitype
                                {
                                    comid = product.Com_id,
                                    proid = proid,
                                    type_name = guigetype
                                };
                                //根据proid,规格类型 判断规格类型是否存在：存在则编辑；否则录入
                                int guigetypeid = new B2b_com_pro_SpecitypeData().Editguigetype(mguigetype);
                                #region 录入规格类型下的规格值(如果规格值 在当前规格类型中依然存在，则修改规格值的状态就可以)
                                for (int j = 0; j < listSpcivalue.Count; j++)
                                {
                                    if (listSpcivalue[j].Substring(0, listSpcivalue[j].IndexOf(":")) == guigetype)
                                    {
                                        B2b_com_pro_Specitypevalue mguigevalue = new B2b_com_pro_Specitypevalue
                                        {
                                            comid = product.Com_id,
                                            isonline = 1,
                                            proid = proid,
                                            typeid = guigetypeid,
                                            val_name = listSpcivalue[j].Substring(listSpcivalue[j].IndexOf(":") + 1)
                                        };
                                        int guigevalueid = new B2b_com_pro_SpecitypevalueData().Editguigetypevalue(mguigevalue);
                                    }
                                }
                                #endregion
                            }
                            #endregion

                            #region 录入规格
                            for (int i = 0; i < listSpci.Count; i++)
                            {

                                var arr = listSpci[i].Split('-');
                                #region 先提取出规格值id 字符串
                                var ggvalueidstr = "";
                                var ggvaluestr = "";//形如（尺寸:1寸）

                                var ggarr = arr[0].Split(';');
                                for (int j = 0; j < ggarr.Length; j++)
                                {
                                    var ggtype = ggarr[j].Substring(0, ggarr[j].IndexOf(":"));
                                    var ggvalue = ggarr[j].Substring(ggarr[j].IndexOf(":") + 1);
                                    //根据proid，规格类型，规格值 查询规格值id
                                    int ggvalueid = new B2b_com_pro_SpecitypevalueData().Getguigevalueid(proid, ggtype, ggvalue);
                                    ggvalueidstr += ggvalueid.ToString() + "-";
                                    ggvaluestr += ggtype + ":" + ggvalue + ";";
                                }
                                if (ggvalueidstr.Length > 0)
                                {
                                    ggvalueidstr = ggvalueidstr.Substring(0, ggvalueidstr.Length - 1);
                                    ggvaluestr = ggvaluestr.Substring(0, ggvaluestr.Length - 1);
                                }
                                #endregion

                                B2b_com_pro_Speci mspeci = new B2b_com_pro_Speci
                                {
                                    speci_name = ggvaluestr,
                                    speci_face_price = arr[1].ConvertTo<decimal>(0),
                                    speci_advise_price = arr[2].ConvertTo<decimal>(0),
                                    speci_agent1_price = arr[3].ConvertTo<decimal>(0),
                                    speci_agent2_price = arr[4].ConvertTo<decimal>(0),
                                    speci_agent3_price = arr[5].ConvertTo<decimal>(0),
                                    speci_agentsettle_price = arr[6].ConvertTo<decimal>(0),
                                    speci_pro_weight = arr[7].ConvertTo<decimal>(0),
                                    speci_totalnum = arr[8].ConvertTo<int>(0),
                                    speci_type_nameid_Array = ggvalueidstr,
                                    comid = product.Com_id,
                                    proid = proid,
                                };
                                //根据proid,规格值id字符串 判断是否含有规格，含有编辑；不含增加
                                int guigeid = new B2b_com_pro_SpeciData().EditB2b_com_pro_Speci(mspeci);
                            }
                            #endregion

                        }
                        #endregion
                        #region 导入产品
                        else
                        {
                            //导入产品修改，只修改 录入规格 销售价，3分销价
                            List<string> listSpcitype = new List<string>();//规格类型列表
                            List<string> listSpcivalue = new List<string>();//规格值列表
                            List<string> listSpci = new List<string>();//规格列表
                            #region 把规格类型，规格值，规格 分别提取到集合中
                            var guigearr = product.guigestr.Split('|');
                            for (int i = 0; i < guigearr.Length; i++)
                            {
                                var guigedetailarr = guigearr[i].Split('-');
                                for (int j = 0; j < guigedetailarr.Length; j++)
                                {
                                    if (j == 0)
                                    {
                                        var gg = guigedetailarr[j].Split(';');
                                        for (int y = 0; y < gg.Length; y++)
                                        {
                                            var spcitype = gg[y].Substring(0, gg[y].IndexOf(":")); //形如:尺寸
                                            if (!listSpcitype.Contains(spcitype))
                                            {
                                                listSpcitype.Add(spcitype);
                                            }
                                            var specivalL = gg[y];//形如:尺寸:1寸
                                            if (!listSpcivalue.Contains(specivalL))
                                            {
                                                listSpcivalue.Add(specivalL);
                                            }
                                        }
                                    }
                                }
                                listSpci.Add(guigearr[i]);
                            }
                            #endregion



                            #region 录入规格
                            for (int i = 0; i < listSpci.Count; i++)
                            {

                                var arr = listSpci[i].Split('-');
                                #region 先提取出规格值id 字符串
                                var ggvalueidstr = "";
                                var ggvaluestr = "";//形如（尺寸:1寸）

                                var ggarr = arr[0].Split(';');
                                for (int j = 0; j < ggarr.Length; j++)
                                {
                                    var ggtype = ggarr[j].Substring(0, ggarr[j].IndexOf(":"));
                                    var ggvalue = ggarr[j].Substring(ggarr[j].IndexOf(":") + 1);
                                    //根据proid(原始产品id)，规格类型，规格值 查询规格值id
                                    int initproid = new B2bComProData().GetOldproidById(proid.ToString());

                                    int ggvalueid = new B2b_com_pro_SpecitypevalueData().Getguigevalueid(initproid, ggtype, ggvalue);
                                    ggvalueidstr += ggvalueid.ToString() + "-";
                                    ggvaluestr += ggtype + ":" + ggvalue + ";";
                                }
                                if (ggvalueidstr.Length > 0)
                                {
                                    ggvalueidstr = ggvalueidstr.Substring(0, ggvalueidstr.Length - 1);
                                    ggvaluestr = ggvaluestr.Substring(0, ggvaluestr.Length - 1);
                                }
                                #endregion

                                B2b_com_pro_Speci mspeci = new B2b_com_pro_Speci
                                {
                                    speci_name = ggvaluestr,
                                    speci_face_price = arr[1].ConvertTo<decimal>(0),
                                    speci_advise_price = arr[2].ConvertTo<decimal>(0),
                                    speci_agent1_price = arr[3].ConvertTo<decimal>(0),
                                    speci_agent2_price = arr[4].ConvertTo<decimal>(0),
                                    speci_agent3_price = arr[5].ConvertTo<decimal>(0),
                                    speci_agentsettle_price = arr[6].ConvertTo<decimal>(0),
                                    speci_pro_weight = arr[7].ConvertTo<decimal>(0),
                                    speci_totalnum = arr[8].ConvertTo<int>(0),
                                    speci_type_nameid_Array = ggvalueidstr,
                                    comid = product.Com_id,
                                    proid = proid,
                                };
                                //根据proid,规格值id字符串 判断是否含有规格，含有编辑；不含增加
                                int guigeid = new B2b_com_pro_SpeciData().EditB2b_com_pro_Speci(mspeci);
                            }
                            #endregion
                        }
                        #endregion
                    }
                }

                var daorupro = prodata.UpBindingProInsertOrUpdate(product);//修改导入产品，但不修改产品导入产品成本价，成本价单独根据分销商核算

                //如果产品下线 导入产品同时下线，但上线的时候不同时上线
                if (product.Pro_state == 0)
                {
                    var daoruprostate = prodata.UpBindingProState(proid, product.Pro_state);
                }

                //UpBindingProUpdatePrice //修改成本价，暂时不做


                //编辑产品所属类目
                if (proid != 0)
                {
                    if (product.Proclass != 0)
                    {
                        prodata.upproclass(proid, product.Proclass, product.Com_id);
                    }
                }
                //编辑(旅游)产品的目的地
                if (proid != 0)
                {
                    if (product.Travelending != "")
                    {
                        ExcelSqlHelper.ExecuteNonQuery("delete b2b_com_trvalending where lineid=" + proid);


                        string[] arr = product.Travelending.Replace("，", ",").Split(',');
                        for (int i = 0; i < arr.Length; i++)
                        {
                            var ending = arr[i].Trim();
                            if (ending != "")
                            {
                                ExcelSqlHelper.ExecuteNonQuery("insert into b2b_com_trvalending(ending,lineid) values('" + ending + "'," + proid + ")");
                            }
                        }


                        //prodata.upproclass(product.Id, product.Proclass, product.Com_id);
                    }
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = proid });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }

        }

       
        #endregion
        #region 修改产品的服务信息
        public static string ModifyProExt(string proid, string service_Contain, string service_NotContain, string Precautions, string Sms)
        {
            try
            {
                var prodata = new B2bComProData();
                var count = prodata.ModifyProExt(proid, service_Contain, service_NotContain, Precautions, Sms);

                return JsonConvert.SerializeObject(new { type = 100, msg = count });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion
        #region 根据公司id获取产品列表
        //public static string ProPageList(string comid, int pageindex, int pagesize)
        //{
        //    var totalcount = 0;
        //    try
        //    {

        //        var prodata = new B2bComProData();
        //        var list = prodata.ProPageList(comid, pageindex, pagesize, out totalcount);
        //        IEnumerable result = "";
        //        if (list != null)

        //            result = from pro in list
        //                     select new
        //                     {
        //                         Id = pro.Id,
        //                         Pro_name = pro.Pro_name.Length > 18 ? pro.Pro_name.Substring(0, 18) : pro.Pro_name,
        //                         Face_price = pro.Face_price,
        //                         Advise_price = pro.Advise_price,
        //                         Agentsettle_price = pro.Agentsettle_price,
        //                         Pro_end = pro.Pro_end,
        //                         Pro_start = pro.Pro_start,
        //                         Source_type = pro.Source_type == 1 ? "自动生成" : "倒码",
        //                         Pro_state = pro.Pro_state == 0 ? "下线" : "上线",
        //                         Service_Contain = pro.Service_Contain,
        //                         Service_NotContain = pro.Service_NotContain,
        //                         Precautions = pro.Precautions,
        //                         Pro_Remark = pro.Pro_Remark,

        //                         Count_Num = prodata.ProSEPageCount(pro.Id),
        //                         Use_Num = prodata.ProSEPageCount_Use(pro.Id),
        //                         UnUse_Num = prodata.ProSEPageCount_UNUse(pro.Id),
        //                         Invalid_Num = prodata.ProSEPageCount_Con(pro.Id),

        //                         Imgurl = AppSettings.CommonSetting.GetValue("FileUpload/FileUrl").ConvertTo<string>() + new FileUploadData().GetFileById(pro.Imgurl).Relativepath

        //                     };

        //        return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
        //    }
        //    catch (Exception ex)
        //    {
        //        return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
        //        throw;
        //    }
        //}
        #endregion

        #region 产品排序列表
        public static string sortlist(int comid)
        {
            var totalcount = 0;
            try
            {

                var prodata = new B2bComProData();
                var list = prodata.sortlist(comid, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Pro_name = pro.Pro_name.Length > 18 ? pro.Pro_name.Substring(0, 18) : pro.Pro_name,
                                 Face_price = pro.Face_price,
                                 Advise_price = pro.Advise_price,
                                 Agentsettle_price = pro.Agentsettle_price,
                                 Pro_end = pro.Pro_end,
                                 Pro_start = pro.Pro_start,
                                 Source_type = pro.Source_type == 1 ? "自动生成" : "倒码",
                                 Pro_state = pro.Pro_state == 0 ? "下线" : "上线",
                                 Service_Contain = pro.Service_Contain,
                                 Service_NotContain = pro.Service_NotContain,
                                 Precautions = pro.Precautions,
                                 Pro_Remark = pro.Pro_Remark,

                                 Count_Num = prodata.ProSEPageCount(pro.Id),
                                 Use_Num = prodata.ProSEPageCount_Use(pro.Id),
                                 UnUse_Num = prodata.ProSEPageCount_UNUse(pro.Id),
                                 Invalid_Num = prodata.ProSEPageCount_Con(pro.Id),

                                 Imgurl = FileSerivce.GetImgUrl(pro.Imgurl),
                                 Sortid = pro.Sortid
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion
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
                    int sortmenu = new B2bComProData().SortMenu(menuid, sortid);
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


        #region 根据
        public static string SearchPnoPageList(string comid, int pageindex, int pagesize, int pro_id, int statetype)
        {
            var totalcount = 0;
            try
            {

                var prodata = new B2bComProData();
                var list = prodata.SearchPnoPageList(comid, pageindex, pagesize, pro_id, statetype, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Pno = pro.Pno,
                                 Pnum = pro.Pnum,
                                 E_proname = pro.E_proname,
                                 Runstate = pro.Runstate == 1 ? "未发送" : pro.Runstate == 2 ? "已发送" : "作废",
                                 Oid = pro.Oid == 0 ? "--" : pro.Oid.ToString(),
                                 Send_state = pro.Send_state == 1 ? "--" : "已发送",
                                 Subdate = pro.Subdate.ToString(),
                                 Sendtime = pro.Sendtime == null ? "--" : pro.Sendtime.ToString()
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion
        #region 根据产品id得到产品信息
        public static string GetProById(string proid)
        {

            try
            {

                var prodata = new B2bComProData();
                var pro = prodata.GetProById(proid);
                if (pro != null)
                {
                    pro.Proclass = prodata.Searchproclassbyid(pro.Id);
                    pro.Travelending = prodata.GetTravelEndingByLineid(proid);

                    int comid_temp = pro.Com_id;
                    var proid_temp = pro.Id;
                    if (pro.Source_type == 4)
                    {
                        comid_temp = new B2bComProData().GetComidByProid(pro.Bindingid);
                        proid_temp = pro.Bindingid;
                    }
                    //获取产品的规格信息
                    if (pro.Manyspeci == 1)
                    {

                        List<B2b_com_pro_Speci> gglist = new B2b_com_pro_SpeciData().Getgglist(pro.Id);
                        List<B2b_com_pro_Specitype> ggtypelist = new B2b_com_pro_SpecitypeData().Getggtypelist(proid_temp);
                        if (ggtypelist.Count > 0 && gglist.Count > 0)
                        {
                            //当前产品最大的规格类型id
                            int ggtypemaxid = new B2b_com_pro_SpecitypeData().Getggtypemaxid(proid_temp);

                            IEnumerable result = null;
                            result = from m in ggtypelist
                                     select new
                                     {
                                         GuigeNum = m.id,
                                         GuigeTitle = m.type_name,
                                         GuigeValues = Getggvallist(m.id)
                                     };

                            return JsonConvert.SerializeObject(new { type = 100, msg = pro, proclass = pro.Proclass, ggtypelist = result, gglist = gglist, ggtypemaxid = ggtypemaxid, initprocomid = comid_temp });
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = pro, proclass = pro.Proclass, ggtypelist = "", gglist = "", ggtypemaxid = "-1", initprocomid = comid_temp });
                        }
                    }

                    return JsonConvert.SerializeObject(new { type = 100, msg = pro, proclass = pro.Proclass, initprocomid = comid_temp });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = pro, proclass = pro.Proclass });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        /// <summary>
        /// 根据规格类型得到规格值列表
        /// </summary>
        /// <param name="ggid"></param>
        /// <param name="proid"></param>
        /// <returns></returns>
        private static List<B2b_com_pro_Specitypevalue> Getggvallist(int ggtypeid)
        {
            List<B2b_com_pro_Specitypevalue> list = new B2b_com_pro_SpecitypevalueData().Getggvallist(ggtypeid);
            return list;
        }
        #endregion

        #region 根据产品id,agentid得到分销产品信息
        public static string GetAgentProById(string proid, int agentlevel)
        {

            try
            {
                var prodata = new B2bComProData();
                var pro = prodata.GetProById(proid);


                if (agentlevel == 1)
                {
                    pro.Advise_price = pro.Agent1_price;
                }
                if (agentlevel == 2)
                {
                    pro.Advise_price = pro.Agent2_price;
                }
                if (agentlevel == 3)
                {
                    pro.Advise_price = pro.Agent3_price;
                }



                if (pro.Bindingid != 0)
                {
                    proid = pro.Bindingid.ToString();
                }

                var linedate = new B2b_com_LineGroupDateData().GetLineGroupDateByLineid(proid.ConvertTo<int>(0), "1", pro.Server_type);//产品团期列表,1代表与有效期内

                decimal ad_price = 0;
                //现在增加了不同级别分销返还,如果返还价格都为0(则应该是原来添加的团旗，为了不和原来的冲突)，则用基本信息中设置的返还价格
                if (pro.Server_type == 9)
                {//订房
                    for (int i = 0; i < linedate.Count; i++)
                    {
                        if (linedate[i].agent1_back == 0 && linedate[i].agent2_back == 0 && linedate[i].agent3_back == 0)
                        {
                            linedate[i].Menprice = linedate[i].Menprice - pro.Advise_price;
                        }
                        else
                        {
                            decimal agent_back = 0;//分销返还金额
                            if (agentlevel == 1)
                            {
                                agent_back = linedate[i].agent1_back;
                            }
                            if (agentlevel == 2)
                            {
                                agent_back = linedate[i].agent2_back;
                            }
                            if (agentlevel == 3)
                            {
                                agent_back = linedate[i].agent3_back;
                            }
                            linedate[i].Menprice = linedate[i].Menprice - agent_back;
                        }

                        if (ad_price == 0)
                        {
                            ad_price = linedate[i].Menprice;
                        }
                    }
                    pro.Advise_price = ad_price;//读取某一天的价格
                }
                //现在增加了不同级别分销价格,如果分销价格都为0(则应该是原来添加的团旗，为了不和原来的冲突)，则用基本信息中设置的分销价格
                if (pro.Server_type == 10)
                {//大巴
                    for (int i = 0; i < linedate.Count; i++)
                    {
                        if (linedate[i].agent1_back == 0 && linedate[i].agent2_back == 0 && linedate[i].agent3_back == 0)
                        {
                            linedate[i].Menprice = pro.Advise_price;
                        }
                        else
                        {
                            decimal agent_price = 0;//分销价
                            if (agentlevel == 1)
                            {
                                agent_price = linedate[i].agent1_back;
                            }
                            if (agentlevel == 2)
                            {
                                agent_price = linedate[i].agent2_back;
                            }
                            if (agentlevel == 3)
                            {
                                agent_price = linedate[i].agent3_back;
                            }
                            linedate[i].Menprice = agent_price;
                        }

                        if (ad_price == 0)
                        {
                            ad_price = linedate[i].Menprice;
                        }
                    }
                    pro.Advise_price = ad_price;//读取某一天的价格
                }

                pro.Agentsettle_price = 0;
                pro.Agent1_price = 0;
                pro.Agent2_price = 0;
                pro.Agent3_price = 0;


                return JsonConvert.SerializeObject(new { type = 100, msg = pro, linedate = linedate });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 客户端根据产品id得到产品信息
        public static string ClientGetProById(string proid, int comid)
        {

            try
            {

                var prodata = new B2bComProData();
                var pro = prodata.ClientGetProById(proid, comid);

                var proid_temp = proid;

                if (pro.Source_type == 4)
                {
                    proid_temp = pro.Bindingid.ToString();
                }


                var linedate = new B2b_com_LineGroupDateData().GetLineGroupDateByLineid(proid_temp.ConvertTo<int>(0), "1", pro.Server_type);//产品团期列表,1代表与有效期内


                if (pro != null)
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = pro, linedate = linedate });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "没有查询到此产品信息" });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 查询产品信息
        public static string Selectpagelist(string comid, int pageindex, int pagesize, string key, int projectid = 0, int proclass = 0, int isoutpro = 0, string openid = "", string viewmethod = "", int MasterId = 0, int menuid = 0, int channelid = 0, string pno = "", int Servertype=0)
        {
            var totalcount = 0;
            try
            {
                var b2bdata = new B2bCrmData();

                //顾问录入多媒体信息列表
                if (isoutpro == 3)
                {
                    List<Wxmedia_updownlog> list = new Wxmedia_updownlogData().Getwxmedia_updownlogByopenid(pageindex, pagesize, out totalcount, openid, viewmethod);
                    if (list == null)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                    }
                    else
                    {
                        if (list.Count > 0)
                        {
                            IEnumerable r = "";
                            if (r != null)
                                r = from pro in list
                                    select new
                                    {
                                        pro.id,
                                        pro.mediaid,
                                        clientuptypemark = pro.clientuptypemark,
                                        relativepath = pro.relativepath != "" ? pro.relativepath : pro.savepath != "" ? pro.savepath.Substring(28) : "",
                                        txtcontent = pro.txtcontent,
                                        WxHeadimgurl = pro.operweixin == "" ? "" : b2bdata.Searchb2bcrmbyopenid(pro.operweixin)
                                    };

                            return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = r });
                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new { type = 1, totalCount = 0, msg = "" });
                        }
                    }
                }
                //顾问咨询记录
                if (isoutpro == 4)
                {
                    WeiXinBasic basic = new WeiXinBasicData().GetWxBasicByComId(int.Parse(comid));
                    if (basic == null)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                    }
                    List<RequestXML> list = new WxRequestXmlData().GetWxrequestxmlByopenid(pageindex, pagesize, out totalcount, openid, basic.Weixinno);
                    if (list == null)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                    }
                    else
                    {
                        if (list.Count > 0)
                        {
                            IEnumerable r = "";
                            if (r != null)
                                r = from pro in list
                                    select new
                                    {
                                        pro.MsgType,
                                        pro.Content,
                                        pro.CreateTimeFormat,
                                        pro.Recognition,
                                        WxHeadimgurl = pro.FromUserName == "" ? "" : b2bdata.Searchb2bcrmbyopenid(pro.FromUserName)
                                    };

                            return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = r });

                        }
                        else
                        {
                            return JsonConvert.SerializeObject(new { type = 1, totalCount = 0, msg = "" });
                        }
                    }
                }

                //易游外部接口产品
                if (isoutpro == 1)
                {
                    if (pagesize > 40)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "外部接口产品每页显示数量最多不能大于40条" });
                    }
                    //string url = "http://preview.etschina.com:8010/TourLineList.ashx?pageindex=" + pageindex + "&pagesize=" + pagesize + "&operatorid=185&tourType=" + proclass;
                    string url = "http://www.etschina.com:8010/TourLineList.ashx?pageindex=" + pageindex + "&pagesize=" + pagesize + "&operatorid=185&tourType=" + proclass;
                    string encry_s = new GetUrlData().HttpGet(url);

                    string encry_key = "64623FB229B4463C99922C9C";
                    string decry_s = EncryptionHelper.Decrypt(encry_s, encry_key);

                    XmlDocument xdoc = new XmlDocument();
                    xdoc.LoadXml(decry_s);
                    XmlElement xroot = xdoc.DocumentElement;
                    XmlNode xnode = xroot.SelectSingleNode("TourProductList");

                    string json = JsonConvert.SerializeXmlNode(xnode);
                    return json;
                }

                //微信文章
                if (isoutpro == 2)
                {
                    var actdata = new WxMaterialData();

                    List<WxMaterial> listwenzhang = null;

                    listwenzhang = actdata.WxMaterialPageList(int.Parse(comid), pageindex, pagesize, 1, 1000000, out totalcount, key, 0, 0, menuid);

                    if (totalcount == 0)
                    {
                        listwenzhang = actdata.WxMaterialPageList(int.Parse(comid), pageindex, pagesize, 1, 1000000, out totalcount, key, 0, menuid);
                    }
                    IEnumerable resultwenzhang = "";
                    if (listwenzhang != null)
                        resultwenzhang = from pro in listwenzhang
                                         select new
                                         {
                                             MaterialId = pro.MaterialId,
                                             Title = pro.Title,
                                             Applystate = pro.Applystate == 1 ? "使用" : "暂停",
                                             Author = pro.Author,
                                             Imgpath = FileSerivce.GetImgUrl(pro.Imgpath.ConvertTo<int>(0)),
                                             //Keyword = actdata.GetWxMaterialKeyWordStrByMaterialId(pro.MaterialId),

                                             Summary = pro.Summary,
                                             Articleurl = pro.Articleurl,

                                             Phone = pro.Phone,
                                             Price = pro.Price,
                                             //PromoteTypeId = pro.SalePromoteTypeid,

                                             //Forcount = actdata.ForwardingWxMaterialcount(pro.Comid, pro.MaterialId),
                                             //Forset = actdata.FrowardingSetSearch(pro.MaterialId)

                                         };

                    return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = resultwenzhang });

                }

                //微信评价
                if (isoutpro == 5)
                {
                    var crmdata = new B2bCrmData();
                    //只获取 此渠道 评价
                    var list = new B2bOrderData().EvaluatePageList(int.Parse(comid), 0, 0, channelid, 0, pageindex, pagesize, out totalcount);
                    if (totalcount == 0)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "", totalcount = 0 });
                    }
                    IEnumerable result = "";
                    if (list != null)
                    {
                        result = from m in list
                                 select new
                                 {
                                     id = m.id,
                                     comid = m.comid,
                                     anonymous = m.anonymous,
                                     channelid = m.channelid,
                                     evatype = m.evatype,
                                     oid = m.oid,
                                     starnum = m.starnum,
                                     subtime = m.subtime,
                                     text = m.text,
                                     uid = m.uid,
                                     Imgurl = crmdata.GetNameorImgByid(m.uid, 1),
                                     uname = crmdata.GetNameorImgByid(m.uid, 2),
                                 };
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                    }

                    return JsonConvert.SerializeObject(new { type = 100, msg = result, totalcount = totalcount });

                }
                //内部产品
                if (isoutpro == 0)
                {
                    string channelphone = "";
                    var prodata = new B2bComProData();
                    List<B2b_com_pro> list = null;
                    //分2步判断 渠道是否设置自己的产品，商户是否制定产品
                    if (MasterId != 0)
                    {

                        channelid = new MemberChannelData().GetChannelidbymanageuserid(MasterId, int.Parse(comid));
                        var channelinfo = new MemberChannelData().GetChannelDetail(channelid, int.Parse(comid));
                        if (channelinfo != null)
                        {
                            channelphone = channelinfo.Mobile;
                        }

                        if (channelid != 0)
                        {
                            //渠道是否设定制定产品
                            list = prodata.Selectpagelist_diaoyong(comid, pageindex, pagesize, "", out totalcount, 0, 0, 0, 0, channelid, channelphone);

                        }
                        if (totalcount == 0)
                        {

                            var menudata = new B2bCompanyMenuData();
                            var menu_temp_id = menudata.selectConsultant_projectid(int.Parse(comid), channelid);

                            //渠道制定项目
                            if (menu_temp_id != 0)
                            {
                                list = prodata.Selectpagelist_diaoyong(comid, pageindex, pagesize, "", out totalcount, 0, 0, 0, menu_temp_id, 0, channelphone);
                            }


                            if (totalcount == 0)
                            {
                                //商户是否制定产品
                                list = prodata.Selectpagelist_diaoyong(comid, pageindex, pagesize, "", out totalcount, 0, 0, 0, menuid, 0, channelphone);
                            }
                        }
                    }

                    if (totalcount == 0)
                    {
                        //默认查询产品
                        list = prodata.Selectpagelist(comid, pageindex, pagesize, key, out totalcount, projectid, proclass, 0, 0, 0, "", 0, pno,Servertype);
                    }

                    IEnumerable result = "";
                    if (list != null)

                        result = from pro in list
                                 select new
                                 {
                                     Id = pro.Id,
                                     Pro_name = pro.Pro_name,
                                     Manyspeci = pro.Manyspeci,
                                     Face_price = pro.Manyspeci == 0 ? pro.Face_price : new B2b_com_pro_SpeciData().Getspeciminfacepricebyid(pro.Id),
                                     Advise_price = pro.Manyspeci == 0 ? pro.Advise_price : new B2b_com_pro_SpeciData().Getspeciminpricebyid(pro.Id),
                                     Agentsettle_price = pro.Agentsettle_price,
                                     Pro_end = pro.Pro_end,
                                     Pro_start = pro.Pro_start,
                                     Source_type = pro.Source_type == 1 ? "自动生成" : "倒码",
                                     Pro_state = pro.Pro_state == 0 ? "下线" : "上线",
                                     Service_Contain = pro.Service_Contain,
                                     Service_NotContain = pro.Service_NotContain,
                                     Precautions = pro.Precautions,
                                     Pro_Remark = pro.Pro_Remark,
                                     Pro_explain = pro.Pro_explain,
                                     Imgurl = FileSerivce.GetImgUrl(pro.Imgurl),
                                     Pro_Integral = pro.Pro_Integral,
                                     Server_type = pro.Server_type,
                                     Ispanicbuy = pro.Ispanicbuy,
                                     Projectid = pro.Projectid,
                                     Limitbuytotalnum = pro.Limitbuytotalnum,
                                     ishasdeliveryfee = pro.ishasdeliveryfee,
                                     Comid = pro.Com_id,
                                     firststationtime= pro.firststationtime,
                                     Projectname = new B2b_com_projectData().GetProjectNameByid(pro.Projectid),
                                     HousetypeNowdayprice = CommonFunc.OperTwoDecimal(new B2b_com_housetypeData().GetHousetypeNowdayprice(pro.Id, pro.Bindingid).ToString()),//获取房型以后日期的房态最低价格
                                     IsYouXiao = new B2bComProData().IsYouxiao(pro.Id, pro.Server_type, pro.Pro_start, pro.Pro_end, pro.Pro_state)//判断产品是否有效：1.票务，直接判断有效期 和产品上线状态2.酒店，跟团游，当地游 则判断是否含有有效的房态/团期 以及产品上下线状态
                                 };

                    return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
                }

                return JsonConvert.SerializeObject(new { type = 1, totalCount = 0, msg = "" });

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion






        #region 查询产品信息
        public static string SelectMenupagelist(string comid, int pageindex, int pagesize, int menuid, int projectid, int allview = 0, string key = "")
        {
            var totalcount = 0;
            try
            {
                int menuviewtype = 0;

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

                var menudatainfo = B2bCompanyMenuData.GetMenuByComid(int.Parse(comid), menuid);
                if (menudatainfo != null) {
                    menuviewtype = menudatainfo.menuviewtype;
                }


                var prodata = new B2bComProData();
                var list = prodata.Selectpagelist(comid, pageindex, pagesize, key, out totalcount, projectid, 0, menuid, 0, 0, "", allview);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Pro_name = pro.Pro_name,
                                 Face_price = pro.Manyspeci == 0 ? pro.Face_price : new B2b_com_pro_SpeciData().Getspeciminfacepricebyid(pro.Id),
                                 Advise_price = pro.Manyspeci == 0 ? pro.Advise_price : new B2b_com_pro_SpeciData().Getspeciminpricebyid(pro.Id),
                                 Agentsettle_price = pro.Agentsettle_price,
                                 Pro_end = pro.Pro_end,
                                 Pro_start = pro.Pro_start,
                                 Source_type = pro.Source_type == 1 ? "自动生成" : "倒码",
                                 Pro_state = pro.Pro_state == 0 ? "下线" : "上线",
                                 Service_Contain = pro.Service_Contain,
                                 Service_NotContain = pro.Service_NotContain,
                                 Precautions = pro.Precautions,
                                 Pro_Remark = pro.Pro_Remark,
                                 Pro_explain = pro.Pro_explain,
                                 Imgurl = FileSerivce.GetImgUrl(pro.Imgurl),
                                 Pro_Integral = pro.Pro_Integral,
                                 Server_type = pro.Server_type,
                                 Ispanicbuy = pro.Ispanicbuy,
                                 Limitbuytotalnum = pro.Limitbuytotalnum,
                                 Projectid = pro.Projectid,
                                 Manyspeci = pro.Manyspeci,
                                 isbig = menuviewtype == 3 ? 1: RandomHelper.YesOdd(pro.xuhao, totalcount),//返回如果是 需要大图返回1 否则返回0, 如果类型等于3则为大图
                                 Projectname = new B2b_com_projectData().GetProjectNameByid(pro.Projectid),
                                 HousetypeNowdayprice = CommonFunc.OperTwoDecimal(new B2b_com_housetypeData().GetHousetypeNowdayprice(pro.Id, pro.Bindingid).ToString()),//获取房型以后日期的房态最低价格
                                 IsYouXiao = new B2bComProData().IsYouxiao(pro.Id, pro.Server_type, pro.Pro_start, pro.Pro_end, pro.Pro_state)//判断产品是否有效：1.票务，直接判断有效期 和产品上线状态2.酒店，跟团游，当地游 则判断是否含有有效的房态/团期 以及产品上下线状态
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });


            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion



        public static string SelectMenuHotelpagelist(int comid, int pageindex, int pagesize, int projectid)
        {
            int totalcount = 0;
            try
            {
                var imagedata = new B2bCompanyMenuData();
                var projectdata = new B2b_com_projectData();
                var list = projectdata.Projectpagelist(comid.ToString(), 1, 12, "1", out totalcount, "", 1, projectid, 9);
                return JsonConvert.SerializeObject(new { type = 100, msg = list, totalCount = totalcount });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }


        #region 查询相关产品信息
        public static string XiangguanPropagelist(string comid, int pageindex, int pagesize, int proid)
        {
            var totalcount = 0;
            int projectid = 0;
            try
            {
                //判断栏目中是否选择具体子项目否则 按项目查询产品
                var prodata = new B2bComProData();
                var proinfo = prodata.ClientGetProById(proid.ToString(), int.Parse(comid));
                if (proinfo != null)
                {
                    projectid = proinfo.Projectid;
                }



                var list = prodata.Selectpagelist(comid, pageindex, pagesize, "", out totalcount, projectid, 0, 0);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Pro_name = pro.Pro_name.Length > 18 ? pro.Pro_name.Substring(0, 18) : pro.Pro_name,
                                 Face_price = pro.Face_price,
                                 Advise_price = CommonFunc.OperTwoDecimal(pro.Advise_price.ToString()),
                                 Agentsettle_price = pro.Agentsettle_price,
                                 Pro_end = pro.Pro_end,
                                 Pro_start = pro.Pro_start,
                                 Source_type = pro.Source_type == 1 ? "自动生成" : "倒码",
                                 Pro_state = pro.Pro_state == 0 ? "下线" : "上线",
                                 Service_Contain = pro.Service_Contain,
                                 Service_NotContain = pro.Service_NotContain,
                                 Precautions = pro.Precautions,
                                 Pro_Remark = pro.Pro_Remark,
                                 Pro_explain = pro.Pro_explain,
                                 Imgurl = FileSerivce.GetImgUrl(pro.Imgurl),
                                 Pro_Integral = pro.Pro_Integral,
                                 Server_type = pro.Server_type,
                                 Ispanicbuy = pro.Ispanicbuy,
                                 Limitbuytotalnum = pro.Limitbuytotalnum,
                                 Projectid = pro.Projectid,
                                 Projectname = new B2b_com_projectData().GetProjectNameByid(pro.Projectid),
                                 HousetypeNowdayprice = CommonFunc.OperTwoDecimal(new B2b_com_housetypeData().GetHousetypeNowdayprice(pro.Id, pro.Bindingid).ToString()),//获取房型以后日期的房态最低价格
                                 IsYouXiao = new B2bComProData().IsYouxiao(pro.Id, pro.Server_type, pro.Pro_start, pro.Pro_end, pro.Pro_state)//判断产品是否有效：1.票务，直接判断有效期 和产品上线状态2.酒店，跟团游，当地游 则判断是否含有有效的房态/团期 以及产品上下线状态
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });


            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 查询产品信息
        public static string TopPropagelist(string comid, int pageindex, int pagesize)
        {
            var totalcount = 0;
            try
            {
                //判断栏目中是否选择具体子项目否则 按项目查询产品

                var prodata = new B2bComProData();
                var list = prodata.TopPropagelist(comid, pageindex, pagesize, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Pro_name = pro.Pro_name,
                                 Face_price = pro.Manyspeci == 0 ? pro.Face_price : new B2b_com_pro_SpeciData().Getspeciminfacepricebyid(pro.Id),
                                 Advise_price = pro.Manyspeci == 0 ? pro.Advise_price : new B2b_com_pro_SpeciData().Getspeciminpricebyid(pro.Id),
                                 Agentsettle_price = pro.Agentsettle_price,
                                 Pro_end = pro.Pro_end,
                                 Pro_start = pro.Pro_start,
                                 Source_type = pro.Source_type == 1 ? "自动生成" : "倒码",
                                 Pro_state = pro.Pro_state == 0 ? "下线" : "上线",
                                 Service_Contain = pro.Service_Contain,
                                 Service_NotContain = pro.Service_NotContain,
                                 Precautions = pro.Precautions,
                                 Pro_Remark = pro.Pro_Remark,
                                 Pro_explain = pro.Pro_explain,
                                 Imgurl = FileSerivce.GetImgUrl(pro.Imgurl),
                                 Pro_Integral = pro.Pro_Integral,
                                 Server_type = pro.Server_type,
                                 Ispanicbuy = pro.Ispanicbuy,
                                 Limitbuytotalnum = pro.Limitbuytotalnum,
                                 Projectid = pro.Projectid,
                                 Projectname = new B2b_com_projectData().GetProjectNameByid(pro.Projectid),
                                 HousetypeNowdayprice = CommonFunc.OperTwoDecimal(new B2b_com_housetypeData().GetHousetypeNowdayprice(pro.Id, pro.Bindingid).ToString()),//获取房型以后日期的房态最低价格
                                 IsYouXiao = new B2bComProData().IsYouxiao(pro.Id, pro.Server_type, pro.Pro_start, pro.Pro_end, pro.Pro_state)//判断产品是否有效：1.票务，直接判断有效期 和产品上线状态2.酒店，跟团游，当地游 则判断是否含有有效的房态/团期 以及产品上下线状态
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });


            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion





        #region 根据产品id得到商家id
        internal static string GetComidByProid(int proid)
        {
            try
            {

                var prodata = new B2bComProData();
                B2b_com_pro pro = prodata.GetProById(proid.ToString());

                return JsonConvert.SerializeObject(new { type = 100, msg = pro.Com_id });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 修改产品状态
        public static string ModifyProState(int proid, int prostate)
        {
            try
            {
                int result = new B2bComProData().ModifyProState(proid, prostate);

                if (result > 0)
                {//如果是 进行下线，则对 导入产品同时下线，但不对上线操作。容易引起随意 上线下线，把所有导入产品给下线了。
                    if (prostate == 0)
                    {
                        /*******************************
                         *  如果是美团上架产品，对原始产品和导入产品进行美团下架处理 和 下架通知
                         * *****************************/
                        //判断是否是美团上架产品
                        bool isstockpro = new B2b_com_pro_groupbuystocklogData().IsStockPro(proid);
                        if (isstockpro)
                        {
                            //得到上架美团绑定此产品的绑定产品列表
                            List<int> proidlist = new B2b_com_pro_groupbuystocklogData().GetStockbindingproidlistByProid(proid);
                            //加入原始产品
                            proidlist.Add(proid);
                            //下架操作
                            DownStockPro(proidlist, 0);
                        }


                        var daoruprostate = new B2bComProData().UpBindingProState(proid, prostate);
                    }
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 修改产品状态
        public static string LuruEticket(int proid, string key, int comid, string leixing)
        {
            string key_temp = "";
            int pnum = 1;//每张票默认1张
            int result = 0;
            int zuofeicountnum = 0;//退票时，判断数量
            string sresult = "";
            try
            {
                if (key == "")
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "电子码错误" });
                }
                string[] key_arr = key.Split('\n');
                string key_str = "";

                //查询重复
                foreach (string k in key_arr)
                {
                    if ((k.Trim().Length) > 0)
                    {
                        key_str = key_str + "'" + k.ToString() + "',";
                        zuofeicountnum = zuofeicountnum + 1;
                    }
                }

                //去掉末尾,
                key_str = key_str.Substring(0, key_str.Length - 1);



                if (leixing == "1")//录入
                {
                    //同一账户不能有重复电子码，
                    sresult = new B2bComProData().SearchEticket(comid, key_str);

                    //如果返回不是""，则证明有重复值
                    if (sresult != "")
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "电子码重复:" + sresult });
                    }
                    //录入
                    foreach (string k in key_arr)
                    {
                        if ((k.Trim().Length) > 0)
                        {
                            result = new B2bComProData().LuruEticket(proid, k.Trim(), comid, pnum);
                        }
                    }
                }
                else
                { //作废
                    //同一账户不能有重复电子码，
                    int countnum = 0;
                    sresult = new B2bComProData().SearchWeishiyongEticket(comid, key_str, out countnum);

                    //如果返回不是必须返回
                    if (sresult != "")
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "错误:" + sresult });
                    }
                    else
                    {
                        if (countnum != zuofeicountnum)
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "退票数量与查询到数量不符" });
                        }
                    }

                    //循环作废
                    foreach (string k in key_arr)
                    {
                        if ((k.Trim().Length) > 0)
                        {
                            result = new B2bComProData().ZuofeiEticket(proid, k.Trim(), comid);
                        }
                    }

                }

                // result = new B2bComProData().LuruEticket(proid, key_temp,comid,pnum);
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion



        public static string WLProPageList(string comid, int pageindex, int pagesize, int prostate, string key = "")
        {
            var totalcount = 0;
            try
            {
                int canviewpro = 0;//可以看到产品范围:0全部，默认；1自己发布的产品

                B2b_company commanage = B2bCompanyData.GetAllComMsg(int.Parse(comid));
                WlGetProInfoDealRequestData wldata = new WlGetProInfoDealRequestData(commanage.B2bcompanyinfo.wl_PartnerId, commanage.B2bcompanyinfo.wl_userkey);

                var list = wldata.WLProPageList(comid, pageindex, pagesize, prostate, out totalcount, key);
   

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = list });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string WLProup(string comid)
        {

            try
            {
 
                B2b_company commanage = B2bCompanyData.GetAllComMsg(int.Parse(comid));
                WlGetProInfoDealRequestData wldata = new WlGetProInfoDealRequestData(commanage.B2bcompanyinfo.wl_PartnerId, commanage.B2bcompanyinfo.wl_userkey);
                var wlprolaqu_json = wldata.productchangedNotify_json(int.Parse(commanage.B2bcompanyinfo.wl_PartnerId));
                var list = wldata.ProductChangedNotify(wlprolaqu_json, int.Parse(comid));

                if (list.IsSuccess == true)
                {
                    return JsonConvert.SerializeObject(new { type = 100,  msg = list.Message });
                }
                else {
                    return JsonConvert.SerializeObject(new { type = 1,  msg = list.Message });
                }


            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string ProPageList(string comid, int pageindex, int pagesize, int prostate, int projectid = 0, string key = "", string viewmethod = "", int userid = 0, int servertype = 0)
        {
            var totalcount = 0;
            try
            {
                int canviewpro = 0;//可以看到产品范围:0全部，默认；1自己发布的产品
                if (userid > 0)
                {
                    Sys_Group sysgroup = new Sys_GroupData().GetGroupByUserId(userid);
                    if (sysgroup != null)
                    {
                        canviewpro = sysgroup.canviewpro;
                    }
                }

                var prodata = new B2bComProData();
                var list = prodata.ProPageList(comid, pageindex, pagesize, prostate, out totalcount, projectid, key, viewmethod, canviewpro, userid, servertype);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {

                                 Id = pro.Id,
                                 Pro_name = pro.Pro_name,
                                 Face_price = pro.Manyspeci == 0 ? CommonFunc.GetRound(pro.Face_price.ToString(), 1) : CommonFunc.GetRound(new B2b_com_pro_SpeciData().Getspeciminfacepricebyid(pro.Id).ToString(), 1),
                                 Advise_price = pro.Manyspeci == 0 ? CommonFunc.GetRound(pro.Advise_price.ToString(), 1) : CommonFunc.GetRound(new B2b_com_pro_SpeciData().Getspeciminpricebyid(pro.Id), 1),
                                 Agentsettle_price = CommonFunc.GetRound(pro.Agentsettle_price.ToString(), 1),
                                 Hotel_price = CommonFunc.GetRound(new B2b_com_housetypeData().GetHousetypeNowdayprice(pro.Id, pro.Bindingid).ToString(), 1),//获取房型以后日期的房态最低价格,
                                 Pro_end = pro.Pro_end,
                                 Pro_start = pro.Pro_start,
                                 Source_type = pro.Source_type == 1 ? "自动生成" : pro.Source_type == 2 ? "倒码" : pro.Source_type == 4 ? "分销导入" : "外部接口",


                                 Pro_state = pro.Pro_state == 0 ? "下线" : "上线",
                                 Service_Contain = pro.Service_Contain,
                                 Service_NotContain = pro.Service_NotContain,
                                 Precautions = pro.Precautions,
                                 Pro_Remark = pro.Pro_Remark,
                                 Viewmethod = pro.Viewmethod,

                                 Count_Num = prodata.ProSEPageCount(pro.Id),
                                 Use_Num = prodata.ProSEPageCount_Use(pro.Id),
                                 UnUse_Num = prodata.ProSEPageCount_UNUse(pro.Id),
                                 Invalid_Num = prodata.ProSEPageCount_Con(pro.Id),
                                 Imgurl = FileSerivce.GetImgUrl(pro.Imgurl),
                                 Proclass = prodata.Searchproclassbyid(pro.Id),

                                 Projectid = pro.Projectid,
                                 Projectname = new B2b_com_projectData().GetProjectNameByid(pro.Projectid),

                                 Servertype = pro.Server_type,
                                 Bindingid = pro.Bindingid,
                                 ProClassName = prodata.Searchproclassnamebyid(pro.Id),
                                 Use_pnum = prodata.ProYanzhengCount(pro.Id, DateTime.Now, DateTime.Now, 1),
                                 ProComeCom = pro.Source_type == 4 ? prodata.GetComNamebyproid(pro.Bindingid) + "(原产品编号:" + pro.Bindingid + ")" : "",//导入产品查询商家

                                 StockNum = AgentJosnData.GetStockNum(pro.Id, pro.Ispanicbuy, pro.Server_type),
                                 Ispanicbuy = pro.Ispanicbuy,
                                 Manyspeci = pro.Manyspeci,

                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


        public static string ProPageListName(string comid, int pageindex, int pagesize, int prostate, int projectid = 0, string key = "", string viewmethod = "")
        {
            var totalcount = 0;
            try
            {

                var prodata = new B2bComProData();
                var list = prodata.ProPageList(comid, pageindex, pagesize, prostate, out totalcount, projectid, key, viewmethod);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {

                                 Id = pro.Id,
                                 Pro_name = pro.Pro_name.Length > 30 ? pro.Pro_name.Substring(0, 30) : pro.Pro_name,
                                 Face_price = CommonFunc.GetRound(pro.Face_price.ToString(), 1),
                                 Advise_price = CommonFunc.GetRound(pro.Advise_price.ToString(), 1),
                                 Agentsettle_price = CommonFunc.GetRound(pro.Agentsettle_price.ToString(), 1),
                                 Hotel_price = CommonFunc.GetRound(new B2b_com_housetypeData().GetHousetypeNowdayprice(pro.Id, pro.Bindingid).ToString(), 1),//获取房型以后日期的房态最低价格,
                                 Pro_end = pro.Pro_end,
                                 Pro_start = pro.Pro_start,
                                 Source_type = pro.Source_type == 1 ? "自动生成" : pro.Source_type == 2 ? "倒码" : pro.Source_type == 4 ? "分销导入" : "外部接口",
                                 Pro_state = pro.Pro_state == 0 ? "下线" : "上线",
                                 Imgurl = FileSerivce.GetImgUrl(pro.Imgurl),
                                 Projectid = pro.Projectid,
                                 Projectname = new B2b_com_projectData().GetProjectNameByid(pro.Projectid),
                                 Servertype = pro.Server_type,
                                 Bindingid = pro.Bindingid,

                                 Ispanicbuy = pro.Ispanicbuy,

                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string ComPageList(int pageindex, int pagesize, int prostate)
        {
            int totalcount = 0;
            try
            {
                B2bCompanyInfoData info = new B2bCompanyInfoData();
                var list = info.ComPageList(pageindex, pagesize, prostate, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 ID = pro.Com_id,
                                 Com_name = B2bCompanyData.GetCompany(pro.Com_id).Com_name,
                                 //Com_state = pro.Info_state == 0 ? "屏蔽" : "显示",
                                 Imprest = GetLogo(pro.Com_id),
                                 Cominfo = pro.Scenic_intro,
                                 end_price = B2bComProData.Pro_max(pro.Com_id).Substring(0, B2bComProData.Pro_max(pro.Com_id).IndexOf(',', 0)),
                                 stata_price = B2bComProData.Pro_max(pro.Com_id).Substring(B2bComProData.Pro_max(pro.Com_id).IndexOf(',', 0) + 1, B2bComProData.Pro_max(pro.Com_id).Length - B2bComProData.Pro_max(pro.Com_id).IndexOf(',', 0) - 1)
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        public static string ComList(int pageindex, int pagesize, int comstate, string key = "")
        {
            int totalcount = 0;
            try
            {
                var info = new B2bCompanyInfoData();
                var list = info.ComList(pageindex, pagesize, comstate, out totalcount, key);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 ID = pro.Com_id,
                                 Com_name = B2bCompanyData.GetCompany(pro.Com_id).Com_name,
                                 Platform_state = pro.Info_state == 0 ? "已屏蔽" : "已显示",
                                 Com_state = B2bCompanyData.GetCompany(pro.Com_id).Com_state == 1 ? "已开通" : "已暂停",
                                 //Com_type = pro.Com_type,
                                 //Scenic_name = pro.Scenic_name,
                                 BriefIntroduction = pro.Scenic_intro.Length > 16 ? pro.Scenic_intro.Substring(0, 15) + "." : pro.Scenic_intro,
                                 Introduction = pro.Scenic_intro,
                                 MembersNum = new B2bCrmData().GetMemberNums(pro.Com_id),
                                 WeiXinState = new WeiXinBasicData().GetWxBasicByComId(pro.Com_id) == null ? "" : "已开通",
                                 WeiXinAttentionNum = new B2bCrmData().GetWeiXinAttentionNum(pro.Com_id),

                                 Fee = B2bCompanyData.GetCompany(pro.Com_id).Fee,
                                 ServiceFee = B2bCompanyData.GetCompany(pro.Com_id).ServiceFee,

                                 SjZxsk = pro.SjZxsk,//商家直销收款
                                 SjZxtp = pro.SjZxtp,//商家直销退票
                                 SjTx = pro.SjTx,//得到商家提现金额
                                 SjZsjalipay = pro.SjZsjalipay,//转商家支付宝
                                 SjSxf = pro.SjSxf,//手续费
                                 SjNotTx = pro.SjZxsk - pro.SjTx - pro.SjZsjalipay - pro.SjSxf - pro.SjZxtp,
                                 //end_price = B2bComProData.Pro_max(pro.Com_id).Substring(0, B2bComProData.Pro_max(pro.Com_id).IndexOf(',', 0)),
                                 //stata_price = B2bComProData.Pro_max(pro.Com_id).Substring(B2bComProData.Pro_max(pro.Com_id).IndexOf(',', 0) + 1, B2bComProData.Pro_max(pro.Com_id).Length - B2bComProData.Pro_max(pro.Com_id).IndexOf(',', 0) - 1)
                                 HasInnerChannel = pro.HasInnerChannel,
                                 Hangye = GetIndustryByComid(pro.Com_id)
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        private static string GetIndustryByComid(int comid)
        {
            B2b_companyindustry m = new B2b_companyindustryData().GetIndustryByComid(comid);
            if (m != null)
            {
                return m.Industryname;
            }
            else
            {
                return "未归类";
            }
        }
        #region 查询商户信息
        public static string ComSelectpagelist(int prostate, int pageindex, int pagesize, string key, int proclass = 0)
        {
            int totalcount = 0;
            try
            {
                B2bCompanyInfoData info = new B2bCompanyInfoData();
                var list = info.ComSelectpagelist(prostate, pageindex, pagesize, key, out totalcount, proclass);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 ID = pro.Com_id,
                                 Com_name = B2bCompanyData.GetCompany(pro.Com_id).Com_name,
                                 //Com_state = pro.Info_state == 0 ? "屏蔽" : "显示",
                                 Imprest = GetLogo(pro.Com_id),
                                 Cominfo = pro.Scenic_intro,
                                 end_price = B2bComProData.Pro_max(pro.Com_id).Substring(0, B2bComProData.Pro_max(pro.Com_id).IndexOf(',', 0)),
                                 stata_price = B2bComProData.Pro_max(pro.Com_id).Substring(B2bComProData.Pro_max(pro.Com_id).IndexOf(',', 0) + 1, B2bComProData.Pro_max(pro.Com_id).Length - B2bComProData.Pro_max(pro.Com_id).IndexOf(',', 0) - 1)
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        private static string GetLogo(int comid)
        {
            string fileurl = AppSettings.CommonSetting.GetValue("FileUpload/FileUrl").ConvertTo<string>("");
            B2b_company_saleset model = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());
            if (model == null)
            {
                return "";
            }
            else
            {
                string logo = model.Logo;
                if (logo == "")
                {
                    return "";
                }
                else
                {
                    FileUploadModel mm = new FileUploadData().GetFileById(logo.ConvertTo<int>(0));
                    if (mm == null)
                    {
                        return "";
                    }
                    else
                    {
                        return fileurl + mm.Relativepath;
                    }


                }

            }
        }
        #endregion

        public static string UpCom(int id, string state)
        {
            try
            {
                var prodata = new B2bCompanyInfoData();
                var result = prodata.UpCom(id, state);

                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string statepagelist(int comid, int pageindex, int pagesize, int state)
        {
            var totalcount = 0;
            try
            {

                var prodata = new B2bComProData();
                var list = prodata.statepagelist(comid, pageindex, pagesize, state, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Pro_name = pro.Pro_name.Length > 18 ? pro.Pro_name.Substring(0, 18) : pro.Pro_name,
                                 Face_price = pro.Face_price,
                                 Advise_price = pro.Advise_price,
                                 Agentsettle_price = pro.Agentsettle_price,
                                 Pro_end = pro.Pro_end,
                                 Pro_start = pro.Pro_start,
                                 Source_type = pro.Source_type == 1 ? "自动生成" : "倒码",
                                 Pro_state = pro.Pro_state == 0 ? "下线" : "上线",
                                 Service_Contain = pro.Service_Contain,
                                 Service_NotContain = pro.Service_NotContain,
                                 Precautions = pro.Precautions,
                                 Pro_Remark = pro.Pro_Remark,
                                 Pro_explain = pro.Pro_explain,
                                 Count_Num = prodata.ProSEPageCount(pro.Id),
                                 Use_Num = prodata.ProSEPageCount_Use(pro.Id),
                                 UnUse_Num = prodata.ProSEPageCount_UNUse(pro.Id),
                                 Invalid_Num = prodata.ProSEPageCount_Con(pro.Id),

                                 Imgurl = FileSerivce.GetImgUrl(pro.Imgurl)

                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


        public static string UpComstate(int id, string state)
        {
            try
            {
                var prodata = new B2bCompanyInfoData();
                var result = prodata.UpComstate(id, state);

                // 商户注册并在总后台开通后，发送给商户一个开通短信
                B2b_company_manageuser m = new B2bCompanyManagerUserData().GetOpenAccount(id);
                if (m != null)
                {
                    string smsstr = "恭喜您，贵单位在易城商户系统注册账户已经开通。账户名" + m.Accounts + " 系统登录网址shop.etown.cn 感谢您的支持！易城商户服务电话010-59059052";
                    string msg = "";
                    int sendback = SendSmsHelper.SendSms(m.Tel, smsstr, m.Com_id, out msg);

                }



                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string AdjustFee(string id, decimal fee)
        {
            try
            {

                if (fee < 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "手续费比例不能小于0" });
                }
                var prodata = new B2bCompanyData();
                var result = prodata.AdjustFee(id, fee);

                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string AdjustServiceFee(string id, decimal ServiceFee)
        {
            try
            {
                if (ServiceFee < 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "服务费费比例不能小于0" });
                }


                var prodata = new B2bCompanyData();
                var result = prodata.AdjustServiceFee(id, ServiceFee);


                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string ComSort(string comids)
        {
            if (comids == "")
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "没有排序的元素" });
            }
            else
            {
                string[] str = comids.Split(',');

                string err = "";
                for (int i = 1; i <= str.Length; i++)
                {
                    string comid = str[i - 1];
                    int sortid = i;
                    int sortcom = new B2bCompanyInfoData().SortCom(comid, sortid);
                    if (sortcom == 0)
                    {
                        err += comid + "err;";

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

        public static string Companysortlist(int pageindex, int pagesize, int comstate)
        {
            int totalcount = 0;
            try
            {
                var info = new B2bCompanyInfoData();
                var list = info.ComList(pageindex, pagesize, comstate, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 ID = pro.Com_id,
                                 Com_name = B2bCompanyData.GetCompany(pro.Com_id).Com_name,

                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string InsertOrUpdatePro(B2b_com_roomtype model, DateTime prostart, DateTime proend, string roomtypepara)
        {
            try
            {

                var roomtypeid = new B2b_com_roomtypeData().InsertOrUpdate(model);

                //删除当前房型下的所有当日记录
                int deldaydetail = new B2b_com_roomtypedayData().DelRoomTypeDayByRoomTypeId(roomtypeid);

                string[] arr1 = roomtypepara.Split('&');
                Dictionary<string, int> myDictionary = new Dictionary<string, int>();
                for (int i = 0; i < arr1.Length; i++)
                {
                    if (arr1[i] != "")
                    {
                        var para = arr1[i].Substring(0, arr1[i].IndexOf("="));
                        var value = arr1[i].Substring(arr1[i].IndexOf("=") + 1);
                        myDictionary.Add(para, int.Parse(value));
                    }
                }



                TimeSpan ts = proend - prostart;
                int iDay = ts.Days;


                for (int i = 0; i <= iDay; i++)
                {
                    var ndate = prostart.AddDays(i).ToString("yyyyMMdd");


                    B2b_com_roomtypeday dayy = new B2b_com_roomtypeday
                    {
                        Id = 0,
                        Dayprice = myDictionary["dayprice" + ndate],
                        Dayavailablenum = myDictionary["availablenum" + ndate],
                        ReserveType = myDictionary["reservetype" + ndate],
                        Daydate = prostart.AddDays(i),
                        Roomtypeid = roomtypeid
                    };

                    int editroomtypeday = new B2b_com_roomtypedayData().InsertOrUpdate(dayy);

                }

                return JsonConvert.SerializeObject(new { type = 100, msg = roomtypeid });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }

        }

        public static string GetRoomType(int roomtypeid, int comid)
        {
            try
            {
                if (roomtypeid == 0 || comid == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
                else
                {
                    B2b_com_roomtype roomtype = new B2b_com_roomtypeData().GetRoomType(roomtypeid, comid);
                    if (roomtype != null)
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = roomtype });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                    }
                }
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
                throw;
            }
        }

        public static string GetRoomTypePagelist(int comid, int pageindex, int pagesize, string startdate = "", string enddate = "", int proid = 0, int projectid = 0, int Agentlevel = 0)
        {
            try
            {
                int ototalcount = 0;
                int stotalcount = 0;
                int totalcount = 0;
                DateTime nowdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));//当前日期
                DateTime nextdaydate = DateTime.Parse(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));//当前日期后一天
                List<B2b_com_pro> list = new List<B2b_com_pro>();
                //两次查询，先查询指定产品
                if (proid != 0)
                {
                    var olist = new B2bComProData().GetHouseTypePageList(pageindex, 1, comid, out ototalcount, proid, projectid);
                    if (olist != null)
                    {
                        foreach (B2b_com_pro mw in olist)
                        {
                            list.Add(mw);
                        }
                        totalcount = totalcount + ototalcount;
                    }

                }

                var slist = new B2bComProData().GetHouseTypePageList(pageindex, 0, comid, out stotalcount, proid, projectid, proid);

                if (slist != null)
                {
                    foreach (B2b_com_pro mw in slist)
                    {
                        list.Add(mw);
                    }
                    totalcount = totalcount + stotalcount;
                }

                IEnumerable result = "";
                if (list != null)

                    result = from model in list
                             select new
                             {
                                 ID = model.Id,
                                 RoomtypeName = model.Pro_name,
                                 BedType = model.Housetype.Bedtype,
                                 Wifi = model.Housetype.Wifi,
                                 WhetherAvailabel = model.Pro_state == 1 ? "上线" : "下线",
                                 CretateTime = model.Createtime,

                                 //前台页面加载(手机酒店预订)
                                 Name = model.Pro_name,
                                 //NowdayPrice = new B2b_com_roomtypedayData().GetRoomTypeDay(model.Id) == null ? 0 : new B2b_com_roomtypedayData().GetRoomTypeDay(model.Id).Dayprice,
                                 Roomtypeimge = FileSerivce.GetImgUrl(model.Imgurl),
                                 Breakfast = model.Housetype.Breakfast, //早餐
                                 Whetherextrabed = model.Housetype.Whetherextrabed,//是否能加床
                                 largestguestnum = model.Housetype.Largestguestnum,//最大入住人数
                                 whethernonsmoking = model.Housetype.Whethernonsmoking,//可否安排无烟楼层
                                 amenities = model.Housetype.Amenities,//便利设施
                                 Mediatechnology = model.Housetype.Mediatechnology,//媒体/科技
                                 Foodanddrink = model.Housetype.Foodanddrink,//食品和饮品
                                 ShowerRoom = model.Housetype.ShowerRoom,//浴室
                                 Builtuparea = model.Housetype.Builtuparea,//建筑面积
                                 floor = model.Housetype.Floor,//楼层
                                 bedwidth = model.Housetype.Bedwidth,//床宽
                                 //dayavailablenum = new B2b_com_roomtypedayData().GetRoomTypeDay(model.Id) == null ? 0 : new B2b_com_roomtypedayData().GetRoomTypeDay(model.Id).Dayavailablenum,  //当日空房数量
                                 NowdayPrice = new B2b_com_LineGroupDateData().GetNowdayPrice(model.Id, startdate),
                                 //Nowdayavailablenum = new B2b_com_LineGroupDateData().GetNowdayavailablenum(model.Id, startdate),
                                 MinEmptynum = new B2b_com_LineGroupDateData().GetMinEmptyNum(model.Id, startdate.ConvertTo<DateTime>(nowdate), enddate.ConvertTo<DateTime>(nextdaydate)),
                                 allprice = new B2b_com_LineGroupDateData().Gethotelallprice(model.Id, startdate.ConvertTo<DateTime>(nowdate), enddate.ConvertTo<DateTime>(nextdaydate), Agentlevel),

                                 daynum = (enddate.ConvertTo<DateTime>(nextdaydate) - startdate.ConvertTo<DateTime>(nowdate)).TotalDays,
                                 extrabedprice = model.Housetype.Extrabedprice,
                                 wifi = model.Housetype.Wifi,
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
                throw;
            }
        }

        public static string GetRoomTypeDayList(int roomtypeid)
        {
            try
            {
                int totalcount = 0;

                string pro_start = "";
                string pro_end = "";
                var list = new B2b_com_roomtypedayData().GetRoomTypeDayList(roomtypeid, out pro_start, out pro_end, out totalcount);

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = list, prostart = pro_start, proend = pro_end });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
                throw;
            }
        }

        public static string GetLineDayGroupDate(DateTime daydate, int lineid)
        {
            try
            {
                B2b_com_LineGroupDate model = new B2b_com_LineGroupDateData().GetLineDayGroupDate(daydate, lineid);

                return JsonConvert.SerializeObject(new { type = 100, msg = model });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
                throw;
            }
        }

        public static string Uplinegroupdate(int lineid, string initdatestr, string datestr, string dayprice, string emptynum, string agent1_back = "", string agent2_back = "", string agent3_back = "")
        {
            lock (lockobj)
            {
                try
                {

                    string[] date1 = datestr.Split(',');
                    string[] dayprice1 = dayprice.Split(',');
                    string[] emptynum1 = emptynum.Split(',');

                    string[] agent1_back1 = agent1_back.Split(',');
                    string[] agent2_back1 = agent2_back.Split(',');
                    string[] agent3_back1 = agent3_back.Split(',');

                    for (int i = 0; i < date1.Length; i++)
                    {
                        if (date1[i] != "" && dayprice1[i] != "" && emptynum1[i] != "")
                        {
                            new B2b_com_LineGroupDateData().DelLineGroupDate(lineid, date1[i]);

                            #region 库存变化日志
                            try
                            {
                                B2b_com_pro_kucunlog kucunlog = new B2b_com_pro_kucunlog
                                {
                                    id = 0,
                                    orderid = 0,
                                    proid = lineid,
                                    servertype = new B2bComProData().GetServertypeByProid(lineid),
                                    daydate = DateTime.Parse(date1[i]),
                                    proSpeciId = 0,
                                    surplusnum = 0,
                                    operor = "",
                                    opertime = DateTime.Now,
                                    opertype = "编辑先删除团期",
                                    oper = "团期"
                                };
                                new B2b_com_pro_kucunlogData().Editkucunlog(kucunlog);
                            }
                            catch { }
                            #endregion


                            B2b_com_LineGroupDate model = new B2b_com_LineGroupDate
                            {
                                //Id = modelid,
                                Id = 0,
                                Menprice = dayprice1[i].ConvertTo<decimal>(0),
                                Childprice = dayprice1[i].ConvertTo<decimal>(0),
                                Oldmenprice = dayprice1[i].ConvertTo<decimal>(0),
                                Emptynum = emptynum1[i].ConvertTo<int>(0),
                                Lineid = lineid,
                                Daydate = date1[i].ConvertTo<DateTime>(),
                                agent1_back = 0,
                                agent2_back = 0,
                                agent3_back = 0
                            };
                            //分销返还金额设置了
                            if (agent1_back != "" && agent2_back != "" && agent3_back != "")
                            {
                                model.agent1_back = agent1_back1[i].ConvertTo<decimal>(0);
                                model.agent2_back = agent2_back1[i].ConvertTo<decimal>(0);
                                model.agent3_back = agent3_back1[i].ConvertTo<decimal>(0);
                            }

                            int edit = new B2b_com_LineGroupDateData().EditLineGroupDate(model);


                            #region 库存变化日志
                            try
                            {
                                B2b_com_pro_kucunlog kucunlog2 = new B2b_com_pro_kucunlog
                                {
                                    id = 0,
                                    orderid = 0,
                                    proid = lineid,
                                    servertype = new B2bComProData().GetServertypeByProid(lineid),
                                    daydate = DateTime.Parse(date1[i]),
                                    proSpeciId = 0,
                                    surplusnum = emptynum1[i].ConvertTo<int>(0),
                                    operor = "",
                                    opertime = DateTime.Now,
                                    opertype = "编辑后添加团期",
                                    oper = "团期"
                                };
                                new B2b_com_pro_kucunlogData().Editkucunlog(kucunlog2);
                            }
                            catch { }
                            #endregion
                        }
                    }

                    return JsonConvert.SerializeObject(new { type = 100, msg = "" });
                }
                catch (Exception e)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
                    throw;
                }
            }
        }

        public static string DelLineGroupDate(int lineid, string daydate)
        {
            try
            {
                int model = new B2b_com_LineGroupDateData().DelLineGroupDate(lineid, daydate);
                #region  库存日志
                try
                {
                    B2b_com_pro_kucunlog kucunlog = new B2b_com_pro_kucunlog
                    {
                        id = 0,
                        orderid = 0,
                        proid = lineid,
                        servertype = new B2bComProData().GetServertypeByProid(lineid),
                        daydate = DateTime.Parse(daydate),
                        proSpeciId = 0,
                        surplusnum = 0,
                        operor = "",
                        opertime = DateTime.Now,
                        opertype = "删除团期",
                        oper = "团期"
                    };
                    int r = new B2b_com_pro_kucunlogData().Editkucunlog(kucunlog);
                }
                catch { }
                #endregion

                return JsonConvert.SerializeObject(new { type = 100, msg = model });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
                throw;
            }
        }


        public static string AdjustHasInnerChannel(int companyid, string hasinnerchannel)
        {
            try
            {
                int result = new B2bCompanyInfoData().AdjustHasInnerChannel(companyid, hasinnerchannel);
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }
        //产品分类列表
        public static string Proclasslist(int pageindex, int pagesize, int prostate, int industryid = 0)
        {
            int totalcount = 0;
            try
            {
                var inddata = new B2b_companyindustryData();
                var prodata = new B2bComProData();

                var list = prodata.Proclasslist(pageindex, pagesize, out totalcount, industryid);

                IEnumerable result = "";
                if (list != null)

                    result = from model in list
                             select new
                             {
                                 Id = model.Id,
                                 Classid = model.Classid,
                                 Classname = model.Classname,
                                 Industryid = model.Industryid,
                                 Industryname = inddata.GetIndustryById(model.Industryid) != null ? inddata.GetIndustryById(model.Industryid).Industryname : "",
                             };



                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        //产品分类管理
        public static string Proclassmanage(int classid, string classname, int industryid)
        {
            try
            {
                var prodata = new B2bComProData();
                var result = prodata.Proclassmanage(classid, classname, industryid);
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }
        //产品分类获取
        public static string Proclassbyid(int classid)
        {
            try
            {
                var prodata = new B2bComProData();
                var result = prodata.Proclassbyid(classid);
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }
        //产品分类删除
        public static string Proclassdel(int classid)
        {
            try
            {
                var prodata = new B2bComProData();
                var result = prodata.Proclassdel(classid);
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }


        //根据产品读取所属分类
        public static string Searchproclassbyid(int proid)
        {
            try
            {
                var prodata = new B2bComProData();
                var result = prodata.Searchproclassbyid(proid);
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }


        public static string EditProject(B2b_com_project model)
        {
            try
            {
                var result = new B2b_com_projectData().EditProject(model);
                if(model.Id>0)
                {
                    if (model.Onlinestate == "0")
                    {
                        /**************************************
                       * 把项目下已经上架产品下架并且向美团发送下架通知
                       * **************************************/

                        //获取项目下已经上架的产品的id
                        List<int> proidlist = new B2b_com_pro_groupbuystocklogData().GetStockProidListByProproject(model.Id);
                        if (proidlist.Count > 0)
                        {
                            string uresult = DownStockPro(proidlist, 0);
                        }
                    }
                }
              
                        
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }

     

        public static string GetProject(int projectid, int comid)
        {
            try
            {
                if (projectid == 0 || comid == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "参数不可为空" });
                }

                B2b_com_project u = new B2b_com_projectData().GetProject(projectid, comid);
                if (u == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "查询结果为空" });
                }
                else
                {
                    List<B2b_com_project> list = new List<B2b_com_project>();
                    list.Add(u);

                    IEnumerable result = "";
                    if (list != null)

                        result = from pro in list
                                 select new
                                 {
                                     Id = pro.Id,
                                     Projectname = pro.Projectname,
                                     Province = pro.Province,
                                     City = pro.City,
                                     Industryid = pro.Industryid,
                                     Industryname = new B2b_companyindustryData().GetIndustryNameById(pro.Industryid),
                                     Projectimg = pro.Projectimg,
                                     Projectimgurl = FileSerivce.GetImgUrl(pro.Projectimg),
                                     CreateTime = pro.Createtime,
                                     OnlineState = pro.Onlinestate,

                                     Briefintroduce = pro.Briefintroduce,
                                     Address = pro.Address,
                                     Mobile = pro.Mobile,
                                     Coordinate = pro.Coordinate,
                                     Serviceintroduce = pro.Serviceintroduce,
                                     hotelset = pro.hotelset,
                                    grade =pro.grade,
                                    star = pro.star,
                                    parking =pro.parking,
                                     cu = pro.cu



                                 };

                    return JsonConvert.SerializeObject(new { type = 100, msg = result });
                }


            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
            }
        }

        public static string Projectpagelist(string comid, int pageindex, int pagesize, string projectstate, string key = "", int runpro=0, int projectid = 0, int Servertype = 0)
        {
            var totalcount = 0;
            try
            {
                DateTime today_star = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                DateTime today = DateTime.Now;


                DateTime ymonth_star = DateTime.Parse(today.AddMonths(-1).ToString("yyyy-MM") + "-1");
                DateTime ymonth_ned = DateTime.Parse(ymonth_star.AddMonths(1).ToString("yyyy-MM-dd"));

                DateTime today_month_star = DateTime.Parse(today.ToString("yyyy-MM") + "-1");
                DateTime today_month_end = DateTime.Now;

                DateTime yday_star = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
                DateTime yday_end = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));


                B2b_company_manageuser user = UserHelper.CurrentUser();
                B2b_company company = UserHelper.CurrentCompany;

                int agentid = company.Bindingagent;



                var prod = new B2bComProData();


                var prodata = new B2b_com_projectData();
                var list = prodata.Projectpagelist(comid, pageindex, pagesize, projectstate, out totalcount, key, runpro, projectid, Servertype);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Projectname = pro.Projectname,
                                 Province = pro.Province,
                                 City = pro.City,
                                 Industryid = pro.Industryid,
                                 Industryname = new B2b_companyindustryData().GetIndustryNameById(pro.Industryid),
                                 Projectimg = pro.Projectimg,
                                 Projectimgurl = FileSerivce.GetImgUrl(pro.Projectimg),
                                 CreateTime = pro.Createtime,
                                 OnlineState = pro.Onlinestate == "1" ? "上线" : "下线",
                                 hotelset = pro.hotelset,
                                 grade = pro.grade,
                                 star = pro.star,
                                 parking = pro.parking,
                                 cu = pro.cu,
                                 All_Use_pnum = prod.ProYanzhengCountbyProjectid(pro.Comid, pro.Id, today, today, 1, agentid),
                                 Today_Use_pnum = prod.ProYanzhengCountbyProjectid(pro.Comid, pro.Id, today_star, today, 0, agentid),
                                 Yday_Use_pnum = prod.ProYanzhengCountbyProjectid(pro.Comid, pro.Id, yday_star, yday_end, 0, agentid),
                                 ToM_Use_pnum = prod.ProYanzhengCountbyProjectid(pro.Comid, pro.Id, today_month_star, today_month_end, 0, agentid),
                                 YoM_Use_pnum = prod.ProYanzhengCountbyProjectid(pro.Comid, pro.Id, ymonth_star, ymonth_ned, 0, agentid),

                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


        public static string WebProjectpagelist(string comid, int pageindex, int pagesize, string projectstate, string key = "", int runpro = 0, int projectid = 0, int Servertype = 0)
        {
            var totalcount = 0;
            try
            {
  

                var prod = new B2bComProData();


                var prodata = new B2b_com_projectData();
                var list = prodata.Projectpagelist(comid, pageindex, pagesize, projectstate, out totalcount, key, 1, projectid, Servertype);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Projectname = pro.Projectname,
                                 Province = pro.Province,
                                 City = pro.City,
                                 Industryid = pro.Industryid,
                                 Industryname = new B2b_companyindustryData().GetIndustryNameById(pro.Industryid),
                                 Projectimg = pro.Projectimg,
                                 Projectimgurl = FileSerivce.GetImgUrl(pro.Projectimg),
                                 CreateTime = pro.Createtime,
                                 OnlineState = pro.Onlinestate == "1" ? "上线" : "下线",
                                 hotelset = pro.hotelset,
                                 grade = pro.grade,
                                 star = pro.star,
                                 parking = pro.parking,
                                 cu = pro.cu,
                                 minprice=  CommonFunc.OperTwoDecimal(pro.minprice)//今天以后的最低价格
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string Projectpageuserlist(string comid, int pageindex, int pagesize, string projectstate, string key = "")
        {
            var totalcount = 0;
            try
            {

                var prodata = new B2b_com_projectData();
                var list = prodata.Projectpagelist(comid, pageindex, pagesize, projectstate, out totalcount, key);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Projectname = pro.Projectname,
                                 Province = pro.Province,
                                 City = pro.City,
                                 Industryid = pro.Industryid,
                                 Industryname = new B2b_companyindustryData().GetIndustryNameById(pro.Industryid),
                                 Projectimg = pro.Projectimg,
                                 Projectimgurl = FileSerivce.GetImgUrl(pro.Projectimg),
                                 CreateTime = pro.Createtime,
                                 OnlineState = pro.Onlinestate == "1" ? "上线" : "下线",

                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string Projectlist(string comid, string projectstate, int prosort = 0)
        {
            var totalcount = 0;
            try
            {

                var prodata = new B2b_com_projectData();
                var list = prodata.Projectlist(comid, projectstate, out totalcount, prosort);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Projectname = pro.Projectname,
                                 Province = pro.Province,
                                 City = pro.City,
                                 Industryid = pro.Industryid,
                                 Industryname = new B2b_companyindustryData().GetIndustryNameById(pro.Industryid),
                                 Projectimg = pro.Projectimg,
                                 Projectimgurl = FileSerivce.GetImgUrl(pro.Projectimg),
                                 CreateTime = pro.Createtime,
                                 OnlineState = pro.Onlinestate == "1" ? "上线" : "下线"

                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string ProjectSelectpagelist(int projectstate, int pageindex, int pagesize, string key, int proclass, string comid, int projectid = 0, string openid = "", int price = 0)
        {
            int totalcount = 0;
            var crmdata = new B2bCrmData();
            try
            {

                var list = new B2b_com_projectData().ProjectSelectpagelist(projectstate, pageindex, pagesize, key, out totalcount, proclass, comid, projectid, price);
                if (totalcount == 0)
                {
                    //判断公司是否含有项目，不含有的话，添加默认项目
                    int count = new B2b_com_projectData().GetProjectCountByComId(int.Parse(comid));
                    if (count == 0)
                    {
                        //添加公司默认项目
                        B2b_company company = new B2bCompanyData().GetCompanyBasicById(int.Parse(comid));
                        B2b_company_info companyinfo = new B2bCompanyInfoData().GetCompanyInfo(int.Parse(comid));
                        B2b_company_saleset saleset = B2bCompanySaleSetData.GetDirectSellByComid(company.ID.ToString());


                        B2b_com_project model = new B2b_com_project()
                        {
                            Id = 0,
                            Projectname = company.Com_name,
                            Projectimg = saleset.Logo.ConvertTo<int>(0),
                            Province = companyinfo.Province,
                            City = companyinfo.City,
                            Industryid = company.Com_type,
                            Briefintroduce = companyinfo.Scenic_intro,
                            Address = companyinfo.Scenic_address,
                            Mobile = companyinfo.Tel,
                            Coordinate = "",
                            Serviceintroduce = companyinfo.Serviceinfo,
                            Onlinestate = "1",
                            Comid = company.ID,
                            Createtime = DateTime.Now,
                            Createuserid = 0
                        };
                        int result1 = new B2b_com_projectData().EditProject(model);
                        model.Id = result1;
                        list.Add(model);
                        //设置公司下产品的项目id都改为默认项目id
                        int result2 = new B2bComProData().UpProjectId(comid, result1);
                    }
                }
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 ID = pro.Id,
                                 Project_name = pro.Projectname,
                                 Project_img = FileSerivce.GetImgUrl(pro.Projectimg),
                                 BriefIntroduce = pro.Briefintroduce,
                                 Lowerprice = new B2bComProData().GetLowerPriceByProjectId(pro.Id),
                                 Distance = crmdata.ProjCoordinates(openid, pro.Id)
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }



        #region 根据线路id得到旅游线路信息
        public static string GetLineById(int lineid)
        {

            try
            {

                var proinfo = new B2bComProData().GetProById(lineid.ToString());
                if (proinfo.Bindingid != 0)
                {
                    lineid = proinfo.Bindingid;

                }

                var linedatedata = new B2b_com_LineGroupDateData();
                var tripdata = new B2b_com_protripData();

                //得到产品服务类型
                int servertype = new B2bComProData().GetProServer_typeById(lineid.ToString());
                var linedate = linedatedata.GetLineGroupDateByLineid(lineid, "1", servertype);//产品团期列表,1代表与有效期内
                var trip = tripdata.Gettriplistbylineid(lineid);//产品行程列表

                return JsonConvert.SerializeObject(new { type = 100, msg = trip, linedate = linedate });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 根据线路id得到行程
        public static string GetTripById(int lineid)
        {

            try
            {

                var tripdata = new B2b_com_protripData();
                var trip = tripdata.Gettriplistbylineid(lineid);//产品行程列表

                return JsonConvert.SerializeObject(new { type = 100, msg = trip });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        public static string GetLineTripById(int tripid, int productid)
        {

            B2b_com_protrip trip = new B2b_com_protripData().GetLineTripById(tripid, productid);
            if (trip == null)
            {
                trip = new B2b_com_protrip();
                trip.Id = 0;
            }
            return JsonConvert.SerializeObject(trip);


        }

        public static string Edittrip(string saveInfor, int productId, int creator)
        {
            var providerId = creator;

            B2b_com_protrip tourJourney = JsonConvert.DeserializeObject<B2b_com_protrip>(saveInfor);

            if (tourJourney != null)
            {
                tourJourney.Productid = productId;
                tourJourney.Dining = tourJourney.Dining == null ? "" : tourJourney.Dining;
            }
            tourJourney.Description = FilterXSS(tourJourney.Description);
            tourJourney.Creator = creator;
            tourJourney.CreateDate = DateTime.Now;

            int result = new B2b_com_protripData().Edittrip(tourJourney);


            return JsonConvert.SerializeObject(new { msg = result });
        }
        public static string Edittrip(B2b_com_protrip trip)
        {
            int result = new B2b_com_protripData().Edittrip(trip);


            return JsonConvert.SerializeObject(new { msg = result });
        }
        ///<summary>     
        ///过滤xss攻击脚本(不是很安全的)  
        ///</summary>     
        ///<param name="input">传入字符串</param>     
        ///<returns>过滤后的字符串</returns>     
        private static string FilterXSS(string html)
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;

            // CR(0a) ，LF(0b) ，TAB(9) 除外，过滤掉所有的不打印出来字符.     
            // 目的防止这样形式的入侵 ＜java\0script＞     
            // 注意：\n, \r,  \t 可能需要单独处理，因为可能会要用到     
            string ret = System.Text.RegularExpressions.Regex.Replace(
                html, "([\x00-\x08][\x0b-\x0c][\x0e-\x20])", string.Empty);

            //替换所有可能的16进制构建的恶意代码     
            //<IMG SRC=&#X40&#X61&#X76&#X61&#X73&#X63&#X72&#X69&#X70&#X74
            //&#X3A&#X61&_#X6C&#X65&#X72&#X74&#X28&#X27&#X58&#X53&#X53&#X27&#X29>     
            string chars = "abcdefghijklmnopqrstuvwxyz" +
                        "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890" +
                        "!@#$%^&*()~`;:?+/={}[]-_|'\"\\";
            for (int i = 0; i < chars.Length; i++)
            {
                ret =
                    System.Text.RegularExpressions.Regex.Replace(ret,
                        string.Concat("(&#[x|X]0{0,}",
                            Convert.ToString((int)chars[i], 16).ToLower(),
                            ";?)"),
                        chars[i].ToString(),
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            }

            //过滤\t, \n, \r构建的恶意代码   
            string[] keywords = {"javascript", "vbscript", "expression", 
                "applet", "meta", "xml", "blink", "link",
                "script", "embed", "object", "iframe", "frame", 
                "frameset", "ilayer", "layer", "bgsound", "title",
                "base" ,"onabort", "onactivate", "onafterprint",
                "onafterupdate", "onbeforeactivate", "onbeforecopy", 
                "onbeforecut", "onbeforedeactivate", "onbeforeeditfocus",
                "onbeforepaste", "onbeforeprint", "onbeforeunload",
                "onbeforeupdate", "onblur", "onbounce", "oncellchange",
                "onchange", "onclick", "oncontextmenu", "oncontrolselect",
                "oncopy", "oncut", "ondataavailable", "ondatasetchanged", 
                "ondatasetcomplete", "ondblclick", "ondeactivate",
                "ondrag", "ondragend", "ondragenter", "ondragleave",
                "ondragover", "ondragstart", "ondrop", "onerror", 
                "onerrorupdate", "onfilterchange", "onfinish", 
                "onfocus", "onfocusin", "onfocusout", "onhelp", 
                "onkeydown", "onkeypress", "onkeyup", "onlayoutcomplete",
                "onload", "onlosecapture", "onmousedown", "onmouseenter", 
                "onmouseleave", "onmousemove", "onmouseout", "onmouseover",
                "onmouseup", "onmousewheel", "onmove", "onmoveend", 
                "onmovestart", "onpaste", "onpropertychange", 
                "onreadystatechange", "onreset", "onresize",
                "onresizeend", "onresizestart", "onrowenter", 
                "onrowexit", "onrowsdelete", "onrowsinserted",
                "onscroll", "onselect", "onselectionchange",
                "onselectstart", "onstart", "onstop", "onsubmit",
                "onunload"};

            foreach (string key in keywords)
            {
                ret = System.Text.RegularExpressions.Regex.Replace(ret, key, "@G" + key + "G@");
            }

            return ret;
        }

        public static string DeleteLineTrip(int tripid, int ProductId)
        {
            int r = new B2b_com_protripData().DeleteLineTrip(tripid, ProductId);
            return JsonConvert.SerializeObject(new { type = 100, msg = r });
        }

        public static string InsertOrUpdateHouseType(B2b_com_housetype model)
        {
            try
            {

                var roomtypeid = new B2b_com_housetypeData().InsertOrUpdate(model);

                return JsonConvert.SerializeObject(new { type = 100, msg = roomtypeid });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string GetHouseType(int proid, int comid)
        {
            var pro_temp_id = 0;//判断是否为导入的产品如果不为0则是导入产品，产品返回后，产品id重新赋值
            if (proid != 0)
            {
                var proinfo = new B2bComProData().GetProById(proid.ToString());
                if (proinfo != null)
                {
                    if (proinfo.Bindingid != 0)
                    {
                        pro_temp_id = proinfo.Bindingid;
                    }
                }
            }

            if (proid == 0 || comid == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "传递参数失败" });
            }
            else
            {
                B2b_com_housetype roomtype = new B2b_com_housetypeData().GetHouseType(proid, comid);
                if (roomtype != null)
                {
                    if (pro_temp_id != 0)
                    {
                        roomtype.Proid = pro_temp_id;//如果产品为导入产品则，返回产品ID，重新赋值
                    }

                    return JsonConvert.SerializeObject(new { type = 100, msg = roomtype });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
            }

        }

        public static string ProjectMenusort(string menuids)
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
                    int sortmenu = new B2b_com_projectData().SortMenu(menuid, sortid);
                    if (sortmenu == 0)
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

        public static string GetHouseTypeDayList(int proid, string startdate, string enddate)
        {
            var r = new B2b_com_LineGroupDateData().GetHouseTypeDayList(proid, startdate, enddate);

            if (r.Count == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = startdate + "--" + enddate + "已满房" });
            }
            else
            {
                //判断是否有房型已满房
                string errmsg = "";
                foreach (B2b_com_LineGroupDate m in r)
                {
                    if (m.Emptynum == 0)
                    {
                        errmsg = m.Daydate.ToString("yyyy-MM-dd") + "已满房";
                        break;
                    }
                }


                if (errmsg == "")
                {
                    //判断是否查询超出了房态范围
                    System.TimeSpan ND = DateTime.Parse(enddate) - DateTime.Parse(startdate);
                    int n = ND.Days;   //天数差 
                    if (r.Count < n)
                    {
                        errmsg = DateTime.Parse(enddate).AddDays(r.Count - n).ToString("yyyy-MM-dd") + "房已满";
                        return JsonConvert.SerializeObject(new { type = 1, msg = errmsg });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = r });
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = errmsg });
                }


            }
        }

        public static string GetMinValidByProjectid(int projectid, int comid)
        {
            B2b_com_LineGroupDate m = new B2b_com_LineGroupDateData().GetMinValidByProjectid(projectid, comid);
            if (m != null)
            {
                return JsonConvert.SerializeObject(new { type = 100, mindate = m.Daydate.ToString("yyyy-MM-dd"), mindate_next = m.Daydate.AddDays(1).ToString("yyyy-MM-dd") });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, mindate = DateTime.Now.ToString("yyyy-MM-dd"), mindate_next = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") });
            }
        }

        public static string GetMinValidByProid(int proid, int comid)
        {
            B2b_com_LineGroupDate m = new B2b_com_LineGroupDateData().GetMinValidByProid(proid, comid);
            if (m != null)
            {
                return JsonConvert.SerializeObject(new { type = 100, mindate = m.Daydate.ToString("yyyy-MM-dd"), mindate_next = m.Daydate.AddDays(1).ToString("yyyy-MM-dd") });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, mindate = DateTime.Now.ToString("yyyy-MM-dd"), mindate_next = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") });
            }
        }

        public static string Getprobypno(string pno)
        {
            if (pno == "")
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "传入电子码为空" });
            }
            B2b_com_pro m = new B2bComProData().Getprobypno(pno);

            if (m != null)
            {
                if (m.isneedbespeak == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "当前产品无需预约" });
                }

                //当日以及当日以后的日期已经进行了预约，自助预约状态为 成功/处理中 不再可以预约，需要通知商户取消预约
                string effectivebespeakdate = "";//有效预约日期
                B2b_order_bespeak effectivebespeak = new B2b_order_bespeakData().Geteffectivebespeak(pno);
                if (effectivebespeak != null)
                {
                    if (effectivebespeak.beaspeakstate == 0)//商户未处理
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "你已经预约过" + effectivebespeakdate + ",商户还未确认，请耐心等待预约成功通知短信或者拨打客服电话(" + m.customservicephone + ")询问具体情况" });
                    }
                    if (effectivebespeak.beaspeakstate == 1)//商户处理预约成功
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "你已经预约成功" + effectivebespeakdate + "，无需再预约，如改变预约请拨打客服电话(" + m.customservicephone + "处理)" });
                    }
                }

                List<B2b_com_pro> list = new List<B2b_com_pro>();
                list.Add(m);

                IEnumerable result = "";
                result = from model in list
                         select new
                         {
                             model.Pro_name,
                             Canbooknum = new B2bComProData().Getordercanbooknumbypno(pno),
                             OrderId = new B2bComProData().GetOrderIdByPno(pno),
                             model.Com_id,
                             model.Id
                         };

                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            else
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = "请确认电子码输入是否正确!" });
            }
        }
        /// <summary>
        /// 提交预约请求
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static string Subautobespeak(B2b_order_bespeak m)
        {
            B2b_com_pro mpro = new B2bComProData().GetProById(m.Proid.ToString());

            //判断产品是否需要预约
            if (mpro.isneedbespeak == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "产品无需预约" });

            }

            //判断产品预约人数是否超过限制
            int bespeaksum = new B2b_order_bespeakData().Gettotalbespeaknum(m.Proid, m.Bespeakdate);
            if (bespeaksum + m.Bespeaknum > mpro.daybespeaknum)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "产品预约人数过多，请预约其他日期" });
            }

            //客户提交预约加限制:当天18点前提交第二天预约，18点后提交第三天预约；
            string min_bespeakdatestr = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            DateTime min_bespeakdate = min_bespeakdatestr.ConvertTo<DateTime>();//可以预约的最早日期

            DateTime subtime = m.subtime;
            int inthour = m.subtime.Hour;
            if (inthour > 18)
            {
                min_bespeakdate = min_bespeakdate.AddDays(1);
            }

            if (m.Bespeakdate < min_bespeakdate)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "当前可以预约的最早日期是" + min_bespeakdate.ToString("yyyy-MM-dd") });
            }


            //同一个电子码每天只可预约一次
            int count = new B2b_order_bespeakData().GetBespeaknum(m.Pno, m.Bespeakdate.ToString("yyyy-MM-dd"));
            if (count > 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "当前电子码已经对" + m.Bespeakdate.ToString("yyyy-MM-dd") + "提交了预约,请等待商家确认！" });

            }

            //把预约信息记录数据库表b2b_order_bespeak
            int result = new B2b_order_bespeakData().Subbespeak(m);
            if (result > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = "" });

            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "提交预约信息失败" });
            }


        }
        /// <summary>
        /// 得到预约日期列表(暂时设为一个月)
        /// </summary>
        /// <returns></returns>
        public static string Getbespeakdatelist()
        {
            try
            {
                //得到可与预约的最早日期(客户提交预约加限制:当天18点前提交第二天预约，18点后提交第三天预约；)
                string min_bespeakdatestr = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                DateTime min_bespeakdate = min_bespeakdatestr.ConvertTo<DateTime>();//可以预约的最早日期
                int inthour = DateTime.Now.Hour;
                if (inthour > 18)
                {
                    min_bespeakdate = min_bespeakdate.AddDays(1);
                }

                DataTable dt = new DataTable("datelist");
                DataColumn coll = dt.Columns.Add("Daydate", typeof(string));
                DataColumn col2 = dt.Columns.Add("Menprice", typeof(string));

                DataRow dr;
                for (int i = 0; i < 31; i++)
                {
                    dr = dt.NewRow();//新行
                    string ddate = min_bespeakdate.AddDays(i).ToString("yyyy-MM-dd");
                    dr["Daydate"] = ddate;
                    dr["Menprice"] = "";
                    dt.Rows.Add(dr);
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = "", linedate = dt });
            }
            catch
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "获取预约日期列表失败", linedate = "" });
            }

        }
        private static int ConvertDateTimeInt(System.DateTime time)
        {

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

            return (int)(time - startTime).TotalSeconds;

        }

        public static string Getbespeaklist(int pageindex, int pagesize, string bespeakdate, string comid, string key, string bespeaktype, string bespeakstate)
        {
            var totalcount = 0;
            try
            {
                var list = new B2b_order_bespeakData().Getbespeaklist(comid, pageindex, pagesize, bespeakdate, key, bespeaktype, bespeakstate, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from m in list
                             select new
                             {
                                 m.Id,
                                 m.Bespeakname,
                                 m.beaspeakstate,
                                 beaspeakstatedesc = m.beaspeakstate == 0 ? "未处理预约" : m.beaspeakstate == 1 ? "预约成功处理" : "预约失败处理",//-1全部；0未处理预约；1预约成功处理；2预约失败处理
                                 beaspeaktype = m.beaspeaktype == 0 ? "提单直接预约" : "自助预约",
                                 m.Bespeakdate,
                                 m.Bespeaknum,
                                 m.Comid,
                                 m.Idcard,
                                 m.Orderid,
                                 m.Phone,
                                 m.Pno,
                                 m.Proid,
                                 m.Proname,
                                 m.remark,
                                 m.subtime,

                             };

                return JsonConvert.SerializeObject(new { type = 100, totalcount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string Operbespeakstate(int id, int bespeakstate)
        {

            B2b_order_bespeak m = new B2b_order_bespeakData().GetbespeakByid(id);
            if (m == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "获取预约信息失败" });
            }

            B2b_com_pro mpro = new B2bComProData().GetProById(m.Proid.ToString());
            if (mpro == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "获取预约产品信息失败" });
            }

            int r = new B2b_order_bespeakData().Operbespeakstate(id, bespeakstate);
            if (r > 0)
            {
                string smscontent = "";//发送内容
                if (bespeakstate == 1)
                {
                    //发送预约成功短信
                    smscontent = mpro.bespeaksucmsg;

                }
                if (bespeakstate == 2)
                {
                    //发送预约失败短信
                    smscontent = mpro.bespeakfailmsg;
                }

                smscontent = smscontent.Replace("$客服电话$", mpro.customservicephone);
                smscontent = smscontent.Replace("$产品名称$", m.Proname);
                smscontent = smscontent.Replace("$数量$", m.Bespeaknum.ToString());
                smscontent = smscontent.Replace("$票号$", m.Pno);
                smscontent = smscontent.Replace("$预约日期$", m.Bespeakdate.ToString("yyyy/MM/dd"));

                string msg = "";
                int sendback = SendSmsHelper.SendSms(m.Phone, smscontent, m.Comid, out msg);
                //记录短信日志表
                B2bSmsMobileSendDate smsmobilelog = new B2bSmsMobileSendDate();
                B2b_smsmobilesend smslog = new B2b_smsmobilesend()
                {
                    Mobile = m.Phone,
                    Content = smscontent,
                    Flag = (int)SendCodeStatus.HasSend,
                    Text = m.Id + "|自助预约短信" + msg,
                    Delaysendtime = "",
                    Oid = 0,
                    Pno = m.Pno,
                    Realsendtime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    Smsid = sendback,
                    Sendeticketid = 0,
                };
                int insertsendmobileid = smsmobilelog.InsertOrUpdate(smslog);

                return JsonConvert.SerializeObject(new { type = 100, msg = "" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "处理失败" });
            }
        }

        public static string Getblackoutdate(string daydate, int comid)
        {
            B2b_com_blackoutdates r = new B2b_com_blackoutdatesData().Getblackoutdate(daydate, comid);

            return JsonConvert.SerializeObject(new { type = 100, msg = r });
        }

        public static string Delblackoutdate(string daydate, int comid)
        {
            int r = new B2b_com_blackoutdatesData().Delblackoutdate(daydate, comid);
            return JsonConvert.SerializeObject(new { type = 100, msg = r });
        }

        public static string Upcomblackoutdate(string ordinaryday_etickettypes, string weekday_etickettypes, string holiday_etickettypes, int comid, string initdatestr, string datestr, string datetype, int userid)
        {
            lock (lockobj)
            {
                try
                {
                    int delcomuseset = new B2b_eticket_usesetData().Delcomusesetbycomid(comid);

                    string[] ordinaryday_etickettypes1 = ordinaryday_etickettypes.Split(',');
                    for (int i = 0; i < ordinaryday_etickettypes1.Length; i++)
                    {
                        B2b_eticket_useset m = new B2b_eticket_useset
                        {
                            id = 0,
                            comid = comid,
                            datetype = 0,
                            etickettype = ordinaryday_etickettypes1[i].ConvertTo<int>(0),
                            operid = userid,
                            opertime = DateTime.Now
                        };

                        new B2b_eticket_usesetData().Inscomusesetbycomid(m);
                    }
                    string[] weekday_etickettypes1 = weekday_etickettypes.Split(',');
                    for (int i = 0; i < weekday_etickettypes1.Length; i++)
                    {
                        B2b_eticket_useset m = new B2b_eticket_useset
                        {
                            id = 0,
                            comid = comid,
                            datetype = 1,
                            etickettype = weekday_etickettypes1[i].ConvertTo<int>(0),
                            operid = userid,
                            opertime = DateTime.Now
                        };

                        new B2b_eticket_usesetData().Inscomusesetbycomid(m);
                    }
                    string[] holiday_etickettypes1 = holiday_etickettypes.Split(',');
                    for (int i = 0; i < holiday_etickettypes1.Length; i++)
                    {
                        B2b_eticket_useset m = new B2b_eticket_useset
                        {
                            id = 0,
                            comid = comid,
                            datetype = 2,
                            etickettype = holiday_etickettypes1[i].ConvertTo<int>(0),
                            operid = userid,
                            opertime = DateTime.Now
                        };
                        new B2b_eticket_usesetData().Inscomusesetbycomid(m);
                    }


                    string[] date1 = datestr.Split(',');
                    string[] datetype1 = datetype.Split(',');

                    for (int i = 0; i < date1.Length; i++)
                    {
                        if (date1[i] != "" && datetype1[i] != "")
                        {

                            new B2b_com_blackoutdatesData().Delblackoutdate(date1[i], comid);


                            B2b_com_blackoutdates model = new B2b_com_blackoutdates
                            {

                                id = 0,
                                blackoutdate = date1[i].ConvertTo<DateTime>(),
                                comid = comid,
                                datetype = datetype1[i].ConvertTo<int>(0),
                                operid = userid,
                                opertime = DateTime.Now,

                            };

                            int edit = new B2b_com_blackoutdatesData().InsB2b_com_blackoutdates(model);
                        }
                    }

                    return JsonConvert.SerializeObject(new { type = 100, msg = "" });
                }
                catch (Exception e)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
                    throw;
                }
            }
        }

        public static string Geteticket_usesetlist(int comid)
        {
            List<B2b_eticket_useset> list = new B2b_eticket_usesetData().Geteticket_usesetlist(comid);
            if (list == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
            else
            {
                if (list.Count == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = list });
                }
            }
        }

        public static string Uplimitbuytotalnum(int proid, int upnum)
        {
            int r = new B2bComProData().Uplimitbuytotalnum(proid, upnum);
            if (proid == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "传递参数有误" });
            }
            if (r > 0)
            {
                #region 库存变化日志
                try
                {
                    B2b_com_pro_kucunlog kucunlog = new B2b_com_pro_kucunlog
                    {
                        id = 0,
                        orderid = 0,
                        proid = proid,
                        servertype = new B2bComProData().GetServertypeByProid(proid),
                        daydate = DateTime.Parse("1970-01-01"),
                        proSpeciId = 0,
                        surplusnum = new B2bComProData().GetLimitbuytotalnum(proid),
                        operor = "",
                        opertime = DateTime.Now,
                        opertype = "编辑抢购/限购产品",
                        oper = "抢购/限购"

                    };
                    new B2b_com_pro_kucunlogData().Editkucunlog(kucunlog);
                }
                catch { }
                #endregion

                return JsonConvert.SerializeObject(new { type = 100, msg = "调整成功" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "调整失败" });
            }

        }

        public static string Editdeliverytmp(int ComputedPriceMethod, string join_provinces, string deliverytypes, int tmpid, string tmpname, string join_deliverytype, string join_areas, string join_startstandards, string join_startfees, string join_addstandards, string join_addfees, int comid, int operor)
        {
            string errmsg = "";
            int r = new B2b_delivery_tmpData().Uplimitbuytotalnum(ComputedPriceMethod, join_provinces, deliverytypes, tmpid, tmpname, join_deliverytype, join_areas, join_startstandards, join_startfees, join_addstandards, join_addfees, comid, operor, out errmsg);

            if (r > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = r });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "调整失败" + errmsg });
            }
        }

        public static string Getdeliverytmp(int tmpid)
        {
            B2b_delivery_tmp tmp = new B2b_delivery_tmpData().Getdeliverytmp(tmpid);
            if (tmp == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "获取运费信息失败" });
            }
            else
            {
                int totalcount = 0;
                IList<B2b_delivery_cost> tmpcostlist = new B2b_delivery_costData().GetB2b_delivery_costlist(tmpid, out totalcount);
                if (tmpcostlist == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "获取运费信息失败." });
                }
                else
                {
                    if (tmpcostlist.Count > 0)
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = tmp, totalcount = totalcount, tmpcostlist = tmpcostlist });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "获取运费信息失败.." });
                    }
                }
            }

        }

        public static string Getdeliverytmplist(int comid)
        {
            int totalcount = 0;
            IList<B2b_delivery_tmp> tmplist = new B2b_delivery_tmpData().Getdeliverytmplist(comid, out  totalcount);
            if (tmplist != null)
            {
                if (tmplist.Count > 0)
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = tmplist, totalcount = totalcount });

                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "获取运费模板失败." });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "获取运费模板失败.." });
            }
        }

        public static string Getprochildimglist(int comid, int proid)
        {
            IList<FileUploadModel> list = new FileUploadData().Getprochildimglist(comid, proid);
            if (list != null)
            {
                if (list.Count > 0)
                {
                    IEnumerable result = null;

                    result = from img in list
                             select new
                             {
                                 fileuploadid = img.Id,
                                 imgurl = FileSerivce.GetImgUrl(img.Id)
                             };
                    return JsonConvert.SerializeObject(new { type = 100, msg = result, totalcount = list.Count });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }
        /// <summary>
        /// 获得模板列表
        /// </summary>
        /// <param name="comid"></param>
        /// <returns></returns>
        public static string Deliverytmppagelist(int comid, int pageindex, int pagesize)
        {
            int totalcount = 0;
            IList<B2b_delivery_tmp> tmplist = new B2b_delivery_tmpData().Getdeliverytmppagelist(comid, pageindex, pagesize, out totalcount);
            if (tmplist.Count > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = tmplist, totalcount = totalcount });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }

        public static string deltmp(int comid, int tmpid)
        {

            string errmsg = "";
            int r = new B2b_delivery_tmpData().deltmp(comid, tmpid, out errmsg);
            if (r > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = "" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = errmsg });
            }
        }

        public static string WebProPageList(string comid, int pageindex, int pagesize, int prostate, int projectid, string key, string viewmethod)
        {

            var totalcount = 0;
            try
            {

                var prodata = new B2bComProData();
                var list = prodata.WebProPageList(comid, pageindex, pagesize, prostate, out totalcount, projectid, key, viewmethod);

                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {

                                 Id = pro.Id,
                                 Pro_name = pro.Pro_name.Length > 18 ? pro.Pro_name.Substring(0, 18) : pro.Pro_name,
                                 Face_price = CommonFunc.OperTwoDecimal(pro.Face_price.ToString()),
                                 Advise_price = CommonFunc.OperTwoDecimal(pro.Advise_price.ToString()),
                                 Agentsettle_price = CommonFunc.OperTwoDecimal(pro.Agentsettle_price.ToString()),
                                 Hotel_price = CommonFunc.OperTwoDecimal(new B2b_com_housetypeData().GetHousetypeNowdayprice(pro.Id, pro.Bindingid).ToString()),//获取房型以后日期的房态最低价格,
                                 Pro_end = pro.Pro_end,
                                 Pro_start = pro.Pro_start,
                                 Source_type = pro.Source_type == 1 ? "自动生成" : pro.Source_type == 2 ? "倒码" : pro.Source_type == 4 ? "分销导入" : "外部接口",
                                 Pro_state = pro.Pro_state == 0 ? "下线" : "上线",
                                 Service_Contain = pro.Service_Contain,
                                 Service_NotContain = pro.Service_NotContain,
                                 Precautions = pro.Precautions,
                                 Pro_Remark = pro.Pro_Remark,
                                 Viewmethod = pro.Viewmethod,

                                 Count_Num = prodata.ProSEPageCount(pro.Id),
                                 Use_Num = prodata.ProSEPageCount_Use(pro.Id),
                                 UnUse_Num = prodata.ProSEPageCount_UNUse(pro.Id),
                                 Invalid_Num = prodata.ProSEPageCount_Con(pro.Id),
                                 Imgurl = FileSerivce.GetImgUrl(pro.Imgurl),
                                 Proclass = prodata.Searchproclassbyid(pro.Id),

                                 Projectid = pro.Projectid,
                                 Projectname = new B2b_com_projectData().GetProjectNameByid(pro.Projectid),

                                 Servertype = pro.Server_type,
                                 Bindingid = pro.Bindingid,
                                 ProClassName = prodata.Searchproclassnamebyid(pro.Id),
                                 Use_pnum = prodata.ProYanzhengCount(pro.Id, DateTime.Now, DateTime.Now, 1),



                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }

        }

        public static string WhetherSaled(int lineid, string daydate)
        {

            bool r = new B2b_delivery_tmpData().WhetherSaled(lineid, daydate);

            return JsonConvert.SerializeObject(new { type = 100, msg = r });

        }

        public static string Editjoinpolicy(int comid, string joinpolicy)
        {
            int r = new B2bCompanyInfoData().Editjoinpolicy(comid, joinpolicy);

            return JsonConvert.SerializeObject(new { type = 100, msg = r });
        }
        public static string Getjoinpolicy(int comid)
        {
            string r = new B2bCompanyInfoData().Getjoinpolicy(comid);

            return JsonConvert.SerializeObject(new { type = 100, msg = r });
        }

        public static string Getchannelcompanylist(int comid, string Issuetype, string companystate, string key)
        {
            List<Member_Channel_company> r = new MemberChannelcompanyData().Getchannelcompanylist(comid, Issuetype, companystate, key);
            if (r.Count > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = r, totalCount = r.Count });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = r, totalCount = 0 });
            }
        }

        public static string Selectguigelist(int comid, int proid, int agentid = 0)
        {
            try
            {
                //如果是导入的 产品，并且是多规格，规格名称及规格值读取原产品的
                B2b_com_pro mpro = new B2bComProData().GetProById(proid.ToString());
                if (mpro != null)
                {
                    if (mpro.Manyspeci == 1)
                    {
                        if (mpro.Source_type == 4)
                        {
                            proid = mpro.Bindingid;

                        }
                    }
                }



                List<B2b_com_pro_Specitype> ggtypelist = new B2b_com_pro_SpecitypeData().Getggtypelist(proid);
                if (ggtypelist.Count > 0)
                {

                    IEnumerable result = null;
                    result = from m in ggtypelist
                             select new
                             {
                                 GuigeNum = m.id,
                                 GuigeTitle = m.type_name,
                                 GuigeValues = Getggvallist(m.id)
                             };
                    return JsonConvert.SerializeObject(new { type = 100, msg = result });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }

        }


        public static string SelectServerlist(int comid, int proid, int agentid = 0, string pno = "")
        {
            try
            {
                int totalcount = 0;
                //只有 商户自己才能收押金所以非商户直销看不到押金
                List<B2b_Rentserver> ggtypelist = new RentserverData().Rentserverpagelist(1, 50, comid, out totalcount, proid, pno);
                if (ggtypelist.Count > 0)
                {

                    return JsonConvert.SerializeObject(new { type = 100, msg = ggtypelist });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }

        }

        public static string Getguigepricearr(int proid)
        {
            var proinfo = new B2bComProData().GetProById(proid.ToString());
            if (proinfo != null)
            {
                List<B2b_com_pro_Speci> gglist = null;//规格列表
                //如果多规格读取规格
                if (proinfo.Manyspeci == 1)
                {
                    gglist = new B2b_com_pro_SpeciData().Getgglist(proinfo.Id);


                    if (gglist != null)
                    {
                        string guigestr = "{";//规格返回字符串
                        for (int i = 0; i < gglist.Count(); i++)
                        {
                            guigestr += "\"" + gglist[i].speci_type_nameid_Array.Replace("-", "_") + "\": {" +
                                   "\"price\": \"" + gglist[i].speci_advise_price.ToString("0.00") + "\", " +
                                   "\"mktprice\": \"" + gglist[i].speci_face_price.ToString("0.00") + "\"," +
                                   "\"speciid\": \"" + gglist[i].id + "\"" +
                                  "},";
                            //guigestr += gglist[i].speci_type_nameid_Array.Replace("-", "_") + ":{" +
                            //       "price:" + gglist[i].speci_advise_price.ToString("0.00") + ", " +
                            //       "mktprice:" + gglist[i].speci_face_price.ToString("0.00") + "," +
                            //       "speciid:" + gglist[i].id + 
                            //      "},";
                        }
                        guigestr += "}";

                        return JsonConvert.SerializeObject(new { type = 100, msg = guigestr, face_price = gglist[0].speci_face_price.ToString("0.00"), advise_price = gglist[0].speci_advise_price.ToString("0.00") });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = "多规格的规格不存在" });
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "非多规格产品" });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "产品信息获取失败" });
            }



        }


        public static string busfeeticketpagelist(int comid, int pageindex, int pagesize)
        {
            var totalcount = 0;
            var totalcountpro = 0;
            var totalcountpno = 0;
            try
            {
                var list = new Bus_FeeticketData().Bus_Feeticketpagelist(comid, pageindex, pagesize, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {

                                 Id = pro.Id,
                                 Comid = pro.Comid,
                                 Feeday = pro.Feeday,
                                 Title = pro.Title,
                                 Endtime = pro.Endtime,
                                 Startime = pro.Startime,
                                 Iuse = pro.Iuse,
                                 BusProinfo = new Bus_FeeticketData().BusFeeticketpropagelist(pro.Id, 1, 10, out totalcountpro),
                                 BusPnoinfo = new Bus_FeeticketData().BusFeeticketpnopagelist(pro.Id, 1, 10, out totalcountpno),
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalcount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }



        public static string delbusfeeticket(int id, int comid)
        {
            try
            {
                var result = new Bus_FeeticketData().DeleteBus_FeeticketById(id, comid);
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }

        }

        public static string upbusfeeticket(Bus_Feeticket businfo)
        {
            try
            {
                var result = new Bus_FeeticketData().Bus_FeeticketInsertOrUpdate(businfo);
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }

        }

        public static string GetBus_FeeticketById(int id, int comid)
        {
            try
            {
                var result = new Bus_FeeticketData().GetBus_FeeticketById(id, comid);
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }

        }


        public static string Busbindingpro(int busid, int comid, int proid, int type, int subtype, int limitweek, int limitweekdaynum, int limitweekendnum)
        {
            try
            {
                var result = new Bus_FeeticketData().Busbindingpro(busid, comid, proid, type, subtype, limitweek, limitweekdaynum, limitweekendnum);
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }

        }



        public static string busfeeticketbindingpropagelist(int pageindex, int pagesize, int busid, int bindingprotype, int comid)
        {
            try
            {
                int totalcount = 0;
                var list = new Bus_FeeticketData().busfeeticketbindingpropagelist(pageindex, pagesize, busid, bindingprotype, comid, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 busid = busid,
                                 Pro_name = pro.Pro_name,
                                 Com_id = pro.Com_id,
                                 BusProinfo = new Bus_FeeticketData().Bus_Feeticket_proById(0, busid, pro.Id),
                                 BusPnoinfo = new Bus_FeeticketData().Bus_Feeticket_pnoById(0, busid, pro.Id),
                                 bindingprotype = bindingprotype,
                             };




                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message, totalCount = 0 });
                throw;
            }

        }



        public static string upRentserver(int id, int comid, string servername, int WR, int num, int posid, decimal saleprice, decimal serverDepositprice, string renttype, int mustselect, int servertype, int printticket, int Fserver)
        {
            try
            {
                var result = new RentserverData().upRentserver(id, comid, servername, WR, num, posid, saleprice, serverDepositprice, renttype, mustselect, servertype, printticket, Fserver);
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }

        }

        public static string delRentserver(int id, int comid)
        {
            try
            {
                var result = new RentserverData().delRentserver(id, comid);
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }

        }


        public static string Rentserverpagelist(int pageindex, int pagesize, int comid, int proid = 0)
        {
            try
            {
                int totalcount = 0;
                var list = new RentserverData().Rentserverpagelist(pageindex, pagesize, comid, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 id = pro.id,
                                 servername = pro.servername,
                                 num = pro.num,
                                 posid = pro.posid,
                                 WR = pro.WR,
                                 comid = pro.comid,
                                 renttype = pro.renttype,
                                 saleprice = pro.saleprice,
                                 serverDepositprice = pro.serverDepositprice,
                                 bindingstate = new RentserverData().Rentserverbinding(pro.id, proid),

                             };


                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message, totalCount = 0 });
                throw;
            }

        }




        public static string Rentserverbyid(int id, int comid)
        {
            try
            {
                int totalcount = 0;
                var list = new RentserverData().Rentserverbyid(id, comid);

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = list });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message, totalCount = 0 });
                throw;
            }

        }
        public static string uppro_worktime(int id, int comid, string title, string defaultendtime, string defaultstartime)
        {
            try
            {
                var list = new RentserverData().uppro_worktime(id, comid, title, defaultendtime, defaultstartime);

                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message, totalCount = 0 });
                throw;
            }

        }


        public static string pro_worktimepagelist(int pageindex, int pagesize, int comid, int proid = 0)
        {
            try
            {
                int totalcount = 0;
                var list = new RentserverData().pro_worktimepagelist(pageindex, pagesize, comid, out totalcount);

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = list });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message, totalCount = 0 });
                throw;
            }

        }
        public static string pro_worktimebyid(int id, int comid)
        {
            try
            {
                var list = new RentserverData().pro_worktimebyid(id, comid);

                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message, totalCount = 0 });
                throw;
            }

        }

        public static string delpro_worktime(int id, int comid)
        {
            try
            {
                var result = new RentserverData().delpro_worktime(id, comid);
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }

        }

        /// <summary>
        /// 得到公司下保险产品列表(产品id;产品名称)
        /// </summary>
        /// <param name="comid"></param>
        /// <returns></returns>
        public static string Getbaoxianlist(int comid)
        {
            IList<B2b_com_pro> list = new B2bComProData().Getbaoxianlist(comid);
            if (list.Count > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });

            }
        }

        public static string Relationserver_printid_chipid(int comid, string cardchipid, int cardprintid)
        {
            IDataReader reader = ExcelSqlHelper.ExecuteReader("select count(1) as  countnum from b2b_Rentserver_printid_chipid where   cardchipid='" + cardchipid + "' or cardprintid=" + cardprintid);
            if (reader.Read())
            {
                int countnum = reader.GetValue<int>("countnum");
                if (countnum > 0)
                {
                    reader.Close();
                    return JsonConvert.SerializeObject(new { type = 1, msg = "服务卡芯片ID或者印刷编号已经录入过，请检查" });
                }
                else
                {
                    int r = ExcelSqlHelper.ExecuteNonQuery("insert into b2b_Rentserver_printid_chipid(comid,cardchipid,cardprintid) values(" + comid + ",'" + cardchipid + "'," + cardprintid + ")");
                    if (r > 0)
                    {
                        reader.Close();
                        return JsonConvert.SerializeObject(new { type = 100, msg = "录入服务卡成功" });
                    }
                    else
                    {
                        reader.Close();
                        return JsonConvert.SerializeObject(new { type = 1, msg = "录入服务卡失败" });
                    }
                }
            }

            reader.Close();
            return JsonConvert.SerializeObject(new { type = 1, msg = "录入服务卡失败" });
        }

        public static string Relationserver_printid_chipidList(int comid, decimal cardchipid, int beginprintid, int endprintid)
        {
            string condition = "comid=" + comid;
            if (cardchipid != 0)
            {
                condition += " and cardchipid='" + cardchipid + "'";
            }

            if (beginprintid != 0 && endprintid == 0)
            {
                condition += " and cardprintid=" + beginprintid;
            }
            else if (beginprintid == 0 && endprintid != 0)
            {
                condition += " and cardprintid=" + endprintid;
            }
            else
            {
                if (beginprintid != 0)
                {
                    condition += " and cardprintid>=" + beginprintid;
                }
                if (endprintid != 0)
                {
                    condition += " and cardprintid<=" + endprintid;
                }
            }
            DataTable t = ExcelSqlHelper.ExecuteDataTable("select * from b2b_Rentserver_printid_chipid where " + condition + " order by id desc");
            if (t != null)
            {
                if (t.Rows.Count > 0)
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = t, totalCount = t.Rows.Count });
                }
                else
                {

                    return JsonConvert.SerializeObject(new { type = 1, msg = "查询服务卡为空", totalCount = 0 });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "查询服务卡为空", totalCount = 0 });
            }

        }

        public static string delRelationserver_printid_chipid(int relationid)
        {
            int r = ExcelSqlHelper.ExecuteNonQuery("delete b2b_Rentserver_printid_chipid where id=" + relationid);
            if (r > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = "删除成功" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "删除失败" });
            }
        }

        public static string Getservertypelist()
        {
            Dictionary<string, string> dics = EnumUtils.GetValueName(typeof(ProductServer_Type));
            if (dics.Count > 0)
            {
                DataTable tblDatas = new DataTable("Datas");
                DataColumn dc = null;
                //dc = tblDatas.Columns.Add("ID", Type.GetType("System.Int32"));
                //dc.AutoIncrement = true;//自动增加
                //dc.AutoIncrementSeed = 1;//起始为1
                //dc.AutoIncrementStep = 1;//步长为1
                //dc.AllowDBNull = false; 

                dc = tblDatas.Columns.Add("ID", Type.GetType("System.String"));
                dc = tblDatas.Columns.Add("Value", Type.GetType("System.String"));

                foreach (KeyValuePair<string, string> item in dics)
                {
                    DataRow newRow;
                    newRow = tblDatas.NewRow();
                    newRow["ID"] = item.Key;
                    newRow["Value"] = item.Value;
                    tblDatas.Rows.Add(newRow);
                }
                return JsonConvert.SerializeObject(new { type = 100, msg = tblDatas });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = dics });
            }
        }

        public static string GetblackoutdateByWorktimeId(string daydate, int proworktimeid)
        {
            b2b_com_pro_worktime_calendar r = new RentserverData().GetblackoutdateByWorktimeId(daydate, proworktimeid);

            return JsonConvert.SerializeObject(new { type = 100, msg = r });
        }

        public static string DelblackoutdateByWorktimeId(string daydate, int proworktimeid)
        {
            int r = new RentserverData().DelblackoutdateByWorktimeId(daydate, proworktimeid);
            return JsonConvert.SerializeObject(new { type = 100, msg = r });
        }

        public static string Upworktimeblackoutdate(int proworktimeid, string initdatestr, string datestr, string starttime, string endtime, int userid, int comid)
        {
            lock (lockobj)
            {
                try
                {
                    int delcomuseset = new RentserverData().Delworktimeusesetbyworktimeid(proworktimeid);


                    string[] date1 = datestr.Split(',');
                    string[] starttime1 = starttime.Split(',');
                    string[] endtime1 = endtime.Split(',');

                    for (int i = 0; i < date1.Length; i++)
                    {
                        if (date1[i] != "" && starttime1[i] != "" && endtime1[i] != "")
                        {

                            new RentserverData().DelblackoutdateByWorktimeId(date1[i], proworktimeid);


                            b2b_com_pro_worktime_calendar model = new b2b_com_pro_worktime_calendar
                            {

                                id = 0,
                                setdate = date1[i].ConvertTo<DateTime>(),
                                worktimeid = proworktimeid,
                                startime = starttime1[i],
                                endtime = endtime1[i],
                                comid = comid,
                            };

                            int edit = new RentserverData().Insworktimeblackoutdates(model);
                        }
                    }

                    return JsonConvert.SerializeObject(new { type = 100, msg = "" });
                }
                catch (Exception e)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = e.Message });
                    throw;
                }
            }
        }

        public static string Getorderstatelist()
        {
            Dictionary<string, string> dics = EnumUtils.GetValueName(typeof(OrderStatus));
            if (dics.Count > 0)
            {
                DataTable tblDatas = new DataTable("Datas");
                DataColumn dc = null;
                //dc = tblDatas.Columns.Add("ID", Type.GetType("System.Int32"));
                //dc.AutoIncrement = true;//自动增加
                //dc.AutoIncrementSeed = 1;//起始为1
                //dc.AutoIncrementStep = 1;//步长为1
                //dc.AllowDBNull = false; 

                dc = tblDatas.Columns.Add("ID", Type.GetType("System.String"));
                dc = tblDatas.Columns.Add("Value", Type.GetType("System.String"));

                foreach (KeyValuePair<string, string> item in dics)
                {
                    DataRow newRow;
                    newRow = tblDatas.NewRow();
                    newRow["ID"] = item.Key;
                    newRow["Value"] = item.Value;
                    tblDatas.Rows.Add(newRow);
                }
                return JsonConvert.SerializeObject(new { type = 100, msg = tblDatas });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = dics });
            }
        }

        public static string GetserverTimeoutPagelist(int comid, int pageindex, int pagesize, string startime, string endtime, string key)
        {
            int totalcount = 0;
            List<b2b_Rentserver_user_Timeoutmoney> list = new RentserverData().GetserverTimeoutPagelist(comid, pageindex, pagesize, startime, endtime, key, out totalcount);
            if (list.Count > 0)
            {
                IEnumerable result = from m in list
                                     select new
                                     {
                                         m.comid,
                                         m.subdate,
                                         m.subtime,
                                         m.oid,
                                         m.userid,
                                         m.proid,
                                         m.Timeoutmoney,
                                         m.TimeoutMinute,
                                         proname = new B2bComProData().GetProName(m.proid),
                                         userphone = new B2bOrderData().GetPhoneByOrderid(m.oid)
                                     };
                decimal timeoutTotalMoney = new RentserverData().GetServerTimeoutMoney(comid, startime, endtime, key);

                return JsonConvert.SerializeObject(new { type = 100, msg = result, timeoutTotalMoney = timeoutTotalMoney, totalcount = totalcount });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }
        public static string serverSuodaoPagelist(int comid, string startime, string endtime, string key)
        {
            string endtime_shiji = DateTime.Parse(endtime).AddDays(1).ToString("yyyy-MM-dd");//结束日期增加一天，这样
            var list = new RentserverData().serverSuodaoPagelist(comid, startime, endtime_shiji, key);
            if (list.Count > 0)
            {
                IEnumerable result = from m in list
                                     select new
                                     {
                                         num = m.U_num,
                                         proname = m.Pro_name,
                                         startime = startime,
                                         endtime = endtime
                                     };

                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "未查询到指定日期的验证数据" });
            }
        }


        public static string GetserverfakaPagelist(int comid, int pageindex, int pagesize, string startime, string endtime, string key, int serverstate, int serverid)
        {
            int totalcount = 0;
            List<B2b_Rentserver_User> list = new RentserverData().GetserverfakaPagelist(comid, pageindex, pagesize, startime, endtime, key, serverstate, serverid, out totalcount);
            if (list.Count > 0)
            {
                IEnumerable result = from m in list
                                     select new
                                     {
                                         m.id,
                                         m.comid,
                                         m.oid,
                                         m.cardid,
                                         m.Depositstate,
                                         m.serverstate,
                                         serverstatedesc = m.serverstate == 2 ? "已结束" : m.endtime>DateTime.Now?"服务进行中":"已过期",
                                         m.subtime,
                                         m.eticketid,
                                         m.cardchipid,
                                         printid = new RentserverData().GetPrintNoByChipid(m.cardchipid.ToString()),
                                         m.endtime,
                                         m.sendstate,
                                         m.sendnum,
                                         m.pname,
                                         xueju = new RentserverData().SearchRentserver_User_alllist_state(m.id),
                                         userphone = new B2bOrderData().GetPhoneByOrderid(m.oid),
                                         backstate = new B2bEticketData().GetB2b_eticket_DepositBypno(m.eticketid)
                                     };
                //未领取
                int notlingqu = new RentserverData().GetServerUsageCount(comid, startime, endtime, key, 0, serverid);
                //领取未归还
                int notguihuan = new RentserverData().GetServerUsageCount(comid, startime, endtime, key, 1, serverid);
                //归还
                int guihuan = new RentserverData().GetServerUsageCount(comid, startime, endtime, key, 2, serverid);

                int fakatotal = notlingqu + notguihuan + guihuan;

                return JsonConvert.SerializeObject(new { type = 100, msg = result, totalcount = totalcount, notlingqu = notlingqu, notguihuan = notguihuan, guihuan = guihuan, fakatotal = fakatotal });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }

        public static string xiaojiancountPagelist(int comid, int pageindex, int pagesize, string startime, string endtime)
        {
            int totalcount = 0;
            List<B2b_Rentserver> list = new RentserverData().Rentserverpagelist(pageindex, pagesize, comid, out totalcount, 0, "");
            if (list.Count > 0)
            {
                IEnumerable result = from m in list
                                     select new
                                     {
                                         m.id,
                                         m.comid,
                                         m.servername,
                                         m.renttype,
                                         m.WR,
                                         m.num,
                                         m.posid,
                                         m.saleprice,
                                         m.serverDepositprice,
                                         m.mustselect,
                                         m.servertype,
                                         m.printticket,
                                         m.Fserver,
                                         guihuancount=new RentserverData().SearchRentserver_count_state(m.id,startime,endtime),
                                         weiguihuancount = new RentserverData().SearchRentserver_weiguihuancount_state(m.id, startime, endtime),
                                         
                                     };

                return JsonConvert.SerializeObject(new { type = 100, msg = result, totalcount = totalcount });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }

        public static string CompanyPoslist(int comid)
        {
            int total = 0;
            List<B2b_company_pos> list = new B2bCompanyPosData().GetPosByComId(comid, out total);
            if (total == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
        }



        public static string procostrilipagelist(int pageindex, int pagesize, int comid, int proid)
        {
            try
            {
                int totalcount = 0;
                var list = new RentserverData().procostrilipagelist(pageindex, pagesize, comid, proid, out totalcount);

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = list });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message, totalCount = 0 });
                throw;
            }

        }

        public static string procostrilibyid(int comid, int id)
        {
            try
            {
                var list = new RentserverData().procostrilibyid(comid, id);

                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message, totalCount = 0 });
                throw;
            }

        }

        public static string upprocostrili(int comid, int id, int proid, decimal costprice, string stardate, string enddate, string admin)
        {
            try
            {
                var costrili = new B2b_pro_cost_rili();
                costrili.comid = comid;
                costrili.id = id;
                costrili.proid = proid;
                costrili.costprice = costprice;
                costrili.stardate = stardate;
                costrili.enddate = enddate;
                costrili.admin = admin;


                //var lashrili = new RentserverData().prolastcostrilibyid(comid, proid, id);
                //if (lashrili != "") {
                //    var lastdate = DateTime.Parse(lashrili).AddDays(1);
                //    var stardate_temp = DateTime.Parse(stardate);

                //    if (lastdate != stardate_temp) {
                //        return JsonConvert.SerializeObject(new { type = 1, msg = "添加和修改产品成本价必须和上一条延续不能隔开日期！" });
                //    }
                //}

                var duibirili = new RentserverData().produibicostrili(comid, proid, stardate, enddate);
                if (duibirili != "")
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "日期" + duibirili + "已经设定过了，如需重设成本价请删除包含此日期的设定！" });
                }


                var list = new RentserverData().upprocostrili(costrili);



                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message, totalCount = 0 });
                throw;
            }

        }

        public static string delcostrili(int comid, int id)
        {
            try
            {
                var list = new RentserverData().delcostrili(comid, id);

                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message, totalCount = 0 });
                throw;
            }

        }


        public static string projectfinancepagelist(int pageindex, int pagesize, int comid, int projectid, string startime, string endtime)
        {
            try
            {
                int totalcount = 0;
                var list = new RentserverData().projectfinancepagelist(pageindex, pagesize, comid, projectid, startime, endtime, out totalcount);

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = list });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message, totalCount = 0 });
                throw;
            }

        }

        public static string projectfinancesum(int comid, int projectid, string startime, string endtime)
        {
            try
            {
                var list = new RentserverData().projectfinancesum(comid, projectid, startime, endtime);//结算数据
                decimal yanpiaoprice = 0;//项目验票金额

                List<B2b_com_pro> prolist = new B2bComProData().Selhotelproductlist(comid, projectid);
                for (int i = 0; i < prolist.Count; i++)
                {
                    yanpiaoprice += B2bOrderData.Xiaofei_price(prolist[i].Id, comid, startime, endtime);//已消费
                }



                return JsonConvert.SerializeObject(new { type = 100, msg = list, yanpiaoprice = yanpiaoprice });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message, totalCount = 0 });
                throw;
            }

        }

        public static string upprojectfinance(int comid, int id, int projectid, decimal Money, string Remarks, string admin)
        {
            try
            {
                var project_finance = new B2b_project_finance();
                project_finance.comid = comid;
                project_finance.id = id;
                project_finance.Projectid = projectid;
                project_finance.Money = Money;
                project_finance.Remarks = Remarks;
                project_finance.admin = admin;
                var list = new RentserverData().upprojectfinance(project_finance);

                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message, totalCount = 0 });
                throw;
            }

        }

        public static string GetProgrouplistByComid(int comid,string runstate="0,1")
        {
            List<B2b_com_pro_group> list = new B2bComProData().GetProgrouplistByComid(comid,runstate);
            if (list.Count > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = list });
            }
        }

        public static string GetProgroupPagelistByComid(int comid, int pageindex, int pagesize)
        {
            int totalcount = 0;
            List<B2b_com_pro_group> list = new B2bComProData().GetProgroupPagelistByComid(comid, pageindex, pagesize, out totalcount);
            if (list.Count > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = list, totalCount = totalcount });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = list, totalCount = 0 });
            }
        }

        public static string Editprogroup(int progroupid, int comid, string groupname, int runstate)
        {
            if (comid == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "传递参数错误:comid=0" });
            }
            B2b_com_pro_group mprogroup = new B2b_com_pro_group
            {
                id = progroupid,
                groupname = groupname,
                comid = comid,
                runstate = runstate
            };

            int r = new B2bComProData().Editprogroup(mprogroup);
            if (r > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = "编辑成功" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "请查看是否有重复的组合" });
            }
        }



        public static string DelProPackagbyid(int id, int comid)
        {
            try
            {
                var list = new B2bComProData().DelProPackagbyid(id);
                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message, totalCount = 0 });
                throw;
            }

        }

        public static string GetProPackagPagelistByid(int pid, int pageindex, int pagesize)
        {
            int totalcount = 0;
            var list = new B2bComProData().GetProPackagPagelistByid(pid, pageindex, pagesize, out totalcount);
            if (list.Count > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = list, totalCount = totalcount });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = list, totalCount = 0 });
            }
        }


        public static string GetProPackagbyid(int id )
        {
            var list = new B2bComProData().GetProPackagbyid(id);
            if (list!=null)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = list});
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = list, totalCount = 0 });
            }
        }


        public static string ProPackageInsertOrUpdate(B2b_com_pro_Package product)
        {
            var list = new B2bComProData().ProPackageInsertOrUpdate(product);
            if (list >0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = list, totalCount = 0 });
            }
        }
        public static string CompanyProbandignzhaji(int comid,int proid)
        {
            int totalCount = 0;
            var list = new B2bComProData().GetProbandingzhajilistByproid(comid, proid,out totalCount);
            if (list.Count >0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = list, totalCount = totalCount });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = list, totalCount = 0 });
            }
        }
        

       
        public static string GroupbuyStockLogPagelist(int pageindex, int pagesize, string key, int groupbuytype, string stockstate, int comid)
        {
            var totalcount = 0;
            try
            {
                var list = new B2b_com_pro_groupbuystocklogData().GroupbuyStockLogPagelist(pageindex, pagesize, key, groupbuytype, stockstate, comid, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from m in list
                             select new
                             { 
                                 m.id,
                                 m.proid,
                                 m.proname,
                                 m.isstock,
                                 m.groupbuytype,
                                 m.stocktime,
                                 m.operuserid, 
                                 m.stockagentcompanyid,
                                 stockagentcompanyname=m.stockagentcompanyname, 
                                 m.groupbuystatus,
                                 m.groupbuystatusdesc, 
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalcount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }

        }

        public static string GetNotStockProPagelist(int comid, int agentid, int pageindex, int pagesize)
        {
            var totalcount = 0;
            try
            {
                //获取分销授权信息
                Agent_warrant agentWarrant = new AgentCompanyData().GetAgent_Warrant(agentid, comid);
                if (agentWarrant==null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, totalcount = totalcount, msg = "分销授权信息获取失败" });
                }

                var list = new B2bComProData().GetNotStockProPagelist(comid,agentid,agentWarrant.Warrant_level, pageindex, pagesize,out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from m in list
                             select new
                             { 
                                 Id=m.Id,
                                 Proname=m.Pro_name
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalcount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        /// <summary>
        /// 上架美团产品
        /// </summary>
        /// <param name="proidstr"></param>
        /// <param name="pronamestr"></param>
        /// <param name="isstock"></param>
        /// <param name="groupbuytype"></param>
        /// <param name="operuserid"></param>
        /// <param name="comid"></param>
        /// <param name="stockagentcompanyid"></param>
        /// <param name="stockagentcompanyname"></param>
        /// <returns></returns>
        public static string StockPro(string proidstr, string pronamestr, int isstock, int groupbuytype, int operuserid, int comid, int stockagentcompanyid, string stockagentcompanyname)
        {
           

            string[] proidarr = proidstr.Split(',');
            string[] pronamearr = pronamestr.Split(',');

            /**********************************
            * 向美团发送产品变化（上架）通知(产品状态 0 产品下架 1 产品上架 2 产品信息变化  5 产品的价格日历变化)
            * ********************************/
            Agent_company agentinfo = new AgentCompanyData().GetAgentCompany(stockagentcompanyid);
            if (agentinfo == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "获取美团分销信息失败" });
            }


            //* 请求的json字符串 
            string bodystr = "";
            foreach (var item in proidarr)
            {
                bodystr += "{" +
                              "\"partnerDealId\": \"" + item + "\"," +
                              "\"status\": " + (int)Meituan_ProStatus.Shangjia + "" +
                           "},";
            }
            bodystr = bodystr.Substring(0, bodystr.Length - 1);

            string reqstr = "{" +
                //"\"code\": 200," +
                //"\"describe\": \"产品变化通知\"," +
                         "\"partnerId\":" + agentinfo.mt_partnerId + "," +
                         "\"body\": [" + bodystr + "]" +
                       "}";

            DealChangeNotice mrequest = (DealChangeNotice)JsonConvert.DeserializeObject(reqstr, typeof(DealChangeNotice));

            ReturnResult r = new MeiTuanInter(agentinfo.mt_partnerId, agentinfo.mt_secret, agentinfo.mt_client).ProductChangedNotify(mrequest, agentinfo.Id);

            if (r.IsSuccess == false)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "上架失败:向美团发送上架通知失败" });
            }


            int addstocklogstatus = 1;//添加团购上架日志状态（0 失败；1成功）

            
            for(int i=0;i<proidarr.Length;i++){
                
                /********************************
                 * 保存上架日志
                 * *****************************/
                B2b_com_pro_groupbuystocklog newlog = new B2b_com_pro_groupbuystocklog
                { 
                  id=0,
                  proid = int.Parse(proidarr[i]),
                  proname=pronamearr[i],
                  isstock = 1,
                  stocktime=DateTime.Now,
                  operuserid=operuserid,
                  comid=comid,
                  stockagentcompanyid=stockagentcompanyid,
                  stockagentcompanyname=stockagentcompanyname,
                  groupbuytype=groupbuytype,
                  groupbuystatus=(int)GroupbuyStatus.NotOnline,
                  groupbuystatusdesc="未上线"
                };
                int addresult = new B2b_com_pro_groupbuystocklogData().EditStocklog(newlog);
                if(addresult<=0)
                {
                    addstocklogstatus = 0;
                    break;
                }
            }
            if (addstocklogstatus == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "上架失败：添加团购上架日志失败" });
            }
             

            return JsonConvert.SerializeObject(new { type = 100, msg = "上架成功" });
            

        }

        /// <summary>
        /// 下架美团产品
        /// </summary>
        /// <param name="proidlist"></param>
        /// <param name="agentcompanyid"></param>
        /// <returns></returns>
        public static string DownStockPro(List<int> proidlist, int agentcompanyid)
        {
            #region 下架单个美团分销的产品
            if (agentcompanyid > 0)
            {
                Agent_company agentinfo = new AgentCompanyData().GetAgentCompany(agentcompanyid);
                if (agentinfo == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "获取美团分销信息失败" });
                }
                if(agentinfo.ismeituan==0)
                {
                    //美团日志中 下架美团产品
                    foreach (var proid in proidlist)
                    {
                        int rresult = new B2b_com_pro_groupbuystocklogData().DownStockPro(proid, agentinfo.Id);
                    }

                    return JsonConvert.SerializeObject(new { type = 100, msg = "下架成功" });
                }

                //发送美团下架通知
                //* 请求的json字符串 
                string bodystr = "";
                foreach (var proid in proidlist)
                {
                    bodystr += "{" +
                                  "\"partnerDealId\": \"" + proid + "\"," +
                                  "\"status\": " + (int)Meituan_ProStatus.Xiajia + "" +
                               "},";
                }
                bodystr = bodystr.Substring(0,bodystr.Length-1);

                string reqstr = "{" +
                             //"\"code\": 200," +
                             //"\"describe\": \"产品变化通知\"," +
                             "\"partnerId\":" + agentinfo.mt_partnerId + "," +
                             "\"body\": [" + bodystr + "]" +
                           "}";

                DealChangeNotice mrequest = (DealChangeNotice)JsonConvert.DeserializeObject(reqstr, typeof(DealChangeNotice));

                ReturnResult r = new MeiTuanInter(agentinfo.mt_partnerId, agentinfo.mt_secret, agentinfo.mt_client).ProductChangedNotify(mrequest, agentinfo.Id);

                if (r.IsSuccess == false)
                {
                    //return JsonConvert.SerializeObject(new { type = 1, msg = "下架失败：产品变化通知发送失败" });
                }
                //美团日志中 下架美团产品
                foreach (var proid in proidlist)
                {
                    int rresult = new B2b_com_pro_groupbuystocklogData().DownStockPro(proid, agentinfo.Id);
                } 

                return JsonConvert.SerializeObject(new { type = 100, msg = "下架成功" });

            }
            #endregion
            #region 下架多个美团分销下面的产品(下架单个产品 和 产品项目时会出现)
            else 
            {
                bool issuc = true;
                string errmsg = "";

                //获得所有的美团分销；然后获得各个美团分销上架的包含在以上产品列表中的产品，然后分别下架和发送下架通知
                List<Agent_company> agentlist = new AgentCompanyData().GetAllMeituanAgentCompany();
                foreach(Agent_company agentinfo in agentlist)
                {
                    List<int> childproidlist = new B2b_com_pro_groupbuystocklogData().GetChildStockProidList(proidlist,agentinfo.Id);
                    if(childproidlist.Count>0)
                    {
                       string rrresult=  DownStockPro(childproidlist, agentinfo.Id);
                       //解析json
                       XmlDocument retdoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + rrresult + "}");
                       XmlElement retroot = retdoc.DocumentElement;
                       string type = retroot.SelectSingleNode("type").InnerText;
                       string msg = retroot.SelectSingleNode("msg").InnerText;
                       if(type=="1")
                       {
                           issuc = false;
                           errmsg = msg;
                           break;
                       }
                    }  
                }

                if (issuc == false)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = errmsg });
                }
                else {
                    return JsonConvert.SerializeObject(new { type = 100, msg = "下架成功" });
                }
            }
            #endregion

        }

        /// <summary>
        /// 对美团产品发送 信息变动通知 和 价格日历变动通知
        /// </summary>
        /// <param name="proidlist"></param>
        /// <param name="agentcompanyid"></param>
        private static string StockProSendChangeNotice(List<int> proidlist, int agentcompanyid)
        {
            #region 变动单个美团分销的产品
            if (agentcompanyid > 0)
            {
                Agent_company agentinfo = new AgentCompanyData().GetAgentCompany(agentcompanyid);
                if (agentinfo == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "获取美团分销信息失败" });
                }
                //发送美团信息变动通知 和 价格日历变动

                #region 信息变动通知
                //* 请求的json字符串 
                string bodystr = "";
                foreach (var proid in proidlist)
                {
                    bodystr += "{" +
                                  "\"partnerDealId\": \"" + proid + "\"," +
                                  "\"status\": " + (int)Meituan_ProStatus.Change + "" +
                               "},";
                }
                bodystr = bodystr.Substring(0, bodystr.Length - 1);

                string reqstr = "{" +
                             //"\"code\": 200," +
                             //"\"describe\": \"产品变化通知\"," +
                             "\"partnerId\":" + agentinfo.mt_partnerId + "," +
                             "\"body\": [" + bodystr + "]" +
                           "}";

                DealChangeNotice mrequest = (DealChangeNotice)JsonConvert.DeserializeObject(reqstr, typeof(DealChangeNotice));

                ReturnResult r = new MeiTuanInter(agentinfo.mt_partnerId, agentinfo.mt_secret, agentinfo.mt_client).ProductChangedNotify(mrequest, agentinfo.Id);
                #endregion

                #region 价格日历变动通知
                string bodystr2 = "";
                foreach (var proid in proidlist)
                {
                    bodystr2 += "{" +
                                  "\"partnerDealId\": \"" + proid + "\"," +
                                  "\"status\": " + (int)Meituan_ProStatus.Change + "" +
                               "},";
                }
                bodystr2 = bodystr2.Substring(0, bodystr2.Length - 1);

                string reqstr2 = "{" +
                             //"\"code\": 200," +
                             //"\"describe\": \"产品变化通知\"," +
                             "\"partnerId\":" + agentinfo.mt_partnerId + "," +
                             "\"body\": [" + bodystr2 + "]" +
                           "}";

                DealChangeNotice mrequest2 = (DealChangeNotice)JsonConvert.DeserializeObject(reqstr2, typeof(DealChangeNotice));

                ReturnResult r2 = new MeiTuanInter(agentinfo.mt_partnerId, agentinfo.mt_secret, agentinfo.mt_client).ProductChangedNotify(mrequest2, agentinfo.Id);
               #endregion


                if (r.IsSuccess&&r2.IsSuccess)
                {
                    
                    return JsonConvert.SerializeObject(new { type = 100, msg = "发送通知成功" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "发送通知失败：产品变化通知发送失败" });
                }


            }
            #endregion
            #region 变动多个美团分销下面的产品(变动单个产品 时会出现)
            else
            {
                bool issuc = true;
                string errmsg = "";

                //获得所有的美团分销；然后获得各个美团分销上架的包含在以上产品列表中的产品，然后分别发送通知
                List<Agent_company> agentlist = new AgentCompanyData().GetAllMeituanAgentCompany();
                foreach (Agent_company agentinfo in agentlist)
                {
                    List<int> childproidlist = new B2b_com_pro_groupbuystocklogData().GetChildStockProidList(proidlist, agentinfo.Id);
                    if (childproidlist.Count > 0)
                    {
                        string rrresult = StockProSendChangeNotice(childproidlist, agentinfo.Id);
                        //解析json
                        XmlDocument retdoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + rrresult + "}");
                        XmlElement retroot = retdoc.DocumentElement;
                        string type = retroot.SelectSingleNode("type").InnerText;
                        string msg = retroot.SelectSingleNode("msg").InnerText;
                        if (type == "1")
                        {
                            issuc = false;
                            errmsg = msg;
                            break;
                        }
                    }
                }

                if (issuc == false)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg =errmsg });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = "发送通知成功" });
                }
            }
            #endregion
        }
        /// <summary>
        /// 自动下线过期产品
        /// </summary>
        /// <returns></returns>
        public static void ProAutoDownLine()
        {
            /******************************************
             * 如果是美团团购产品，需要把产品下架；同时向美团发送下架通知
             * 1.查询出过期的团购上架产品列表
             * 2.下架产品并且发送下架通知
             * ****************************************/
            List<int> list = new B2bComProData().GetAutoDownlineProlist();
            if(list.Count>0)
            {
                DownStockPro(list,0);

                //自动下线过期产品
                int downresult = new B2bComProData().ProAutoDownLine();
            } 
        }


        public static string zhajilogPagelist(int comid, int id)
        {
            int totalCount = 0;
            var list = new B2bComProData().GetRentserver_User_zhajilogByuid(comid, id, out totalCount);
            if (list.Count > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = list, totalCount = totalCount });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = list, totalCount = 0 });
            }
        }

    }
}
