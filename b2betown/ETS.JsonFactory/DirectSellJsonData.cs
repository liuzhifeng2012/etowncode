using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using Newtonsoft.Json;
using FileUpload.FileUpload.Entities;
using FileUpload.FileUpload.Data;
using ETS2.Common.Business;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS.Framework;
using System.Collections;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.WeiXin.Service.WeiXinService.Model;

namespace ETS.JsonFactory
{
    public class DirectSellJsonData
    {
        public static string InsertOrUpdate(B2b_company_saleset saleset)
        {
            try
            {
                var saledata = new B2bCompanySaleSetData();
                var crmid = saledata.InsertOrUpdate(saleset);

                return JsonConvert.SerializeObject(new { type = 100, msg = crmid });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string Updatesms(B2b_company_saleset saleset)
        {
            try
            {
                var saledata = new B2bCompanySaleSetData();
                var crmid = saledata.Updatesms(saleset);

                return JsonConvert.SerializeObject(new { type = 100, msg = crmid });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string GetDirectSellByComid(string comid)
        {
            try
            {
                B2b_company_saleset com = B2bCompanySaleSetData.GetDirectSellByComid(comid);
                return JsonConvert.SerializeObject(new { type = 100, msg = com });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }

        public static string GetFileupload(string id)
        {
            try
            {
                FileUploadModel com = new FileUploadData().GetFileById(int.Parse(id));
                return JsonConvert.SerializeObject(new { type = 100, msg = com });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }

        public static string Getcomlogobyproid(int proid)
        {
            if (proid == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
            else
            {
                B2b_com_pro pro = new B2bComProData().GetProById(proid.ToString());
                if (pro != null)
                {
                    B2b_company_saleset saleset = B2bCompanySaleSetData.GetDirectSellByComid(pro.Com_id.ToString());
                    if (saleset != null)
                    {
                        if (saleset.Smalllogo != null && saleset.Smalllogo != "")
                        { 
                            return JsonConvert.SerializeObject(new { type = 100, msg = FileSerivce.GetImgUrl(saleset.Smalllogo.ConvertTo<int>(0)) });
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
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });

                }

            }
        }




        public static string Getimagelist(int comid, int typeid, int pageindex, int pagesize)
        {
            //typeid 0 =微网站Banner， 1=门市Banner
            int totalcount = 0;
            try
            {
                var imagedata = new B2bCompanyImageData();

                List<B2b_company_image> list = imagedata.GetimageList(comid, typeid, pageindex, pagesize, out totalcount);
                IEnumerable result = "";
                if (list != null)
                {
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Com_id = pro.Com_id,
                                 Imgurl_address = FileSerivce.GetImgUrl(pro.Imgurl),
                                 Imgurl = pro.Imgurl,
                                 Linkurl = pro.Linkurl,
                                 Title = pro.Title,
                                 Typeid = pro.Typeid,
                                 State = pro.State == 1 ? "上线" : "下线"
                             };
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = result, totalCount = totalcount });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }

        }


        public static string Getchannelimagelist(int comid, int typeid,int channelcompanyid, int pageindex, int pagesize)
        {
            //typeid 0 =微网站Banner， 1=门市Banner

            if (channelcompanyid == 0) {
                return JsonConvert.SerializeObject(new { type = 1, msg = "操作错误，请重新登录" });
            }

            if (typeid == 0) {
                typeid = 1;//渠道只能添加门市显示的图片
            }

            int totalcount = 0;
            try
            {


                var imagedata = new B2bCompanyImageData();

                List<B2b_company_image> list = imagedata.GetimageList(comid, typeid, pageindex, pagesize, out totalcount,channelcompanyid);
                IEnumerable result = "";
                if (list != null)
                {
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Com_id = pro.Com_id,
                                 Imgurl_address = FileSerivce.GetImgUrl(pro.Imgurl),
                                 Imgurl = pro.Imgurl,
                                 Linkurl = pro.Linkurl,
                                 Title = pro.Title,
                                 Typeid = pro.Typeid,
                                 State = pro.State == 1 ? "上线" : "下线"
                             };
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = result, totalCount = totalcount });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }



        public static string PageGetimagelist(int comid, int typeid)
        {
            //typeid 0 =微网站Banner， 1=门市Banner
            int totalcount = 0;
            try
            {
                var imagedata = new B2bCompanyImageData();

                List<B2b_company_image> list = imagedata.PageGetimageList(comid, typeid, out totalcount);
                IEnumerable result = "";
                if (list != null)
                {
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Com_id = pro.Com_id,
                                 Imgurl_address = FileSerivce.GetImgUrl(pro.Imgurl),
                                 Imgurl = pro.Imgurl,
                                 Linkurl = pro.Linkurl,
                                 Title = pro.Title,
                                 Typeid = pro.Typeid,
                                 State = pro.State==1 ?"上线":"下线"
                             };
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = result, totalCount = totalcount });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }



        public static string GetimageByComid(int comid, int id)
        {
            try
            {
                B2b_company_image com = B2bCompanyImageData.GetimageByComid(comid, id);
                return JsonConvert.SerializeObject(new { type = 100, msg = com });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }

         public static string GetchannelimageByComid(int comid, int id,int channelcompanyid)
        {
            if (channelcompanyid == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "操作错误，请重新登录" });
            }

            try
            {
                B2b_company_image com = B2bCompanyImageData.GetimageByComid(comid, id,channelcompanyid);
                return JsonConvert.SerializeObject(new { type = 100, msg = com });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }
        

        public static string Deleteimage(int comid,int id)
        {
            try
            {
                var saledata = new B2bCompanyImageData();
                var crmid = saledata.Deleteimage(comid, id);

                return JsonConvert.SerializeObject(new { type = 100, msg = crmid });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string Deletechannelimage(int comid, int id, int channelcompanyid)
        {
            if (channelcompanyid == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "操作错误，请重新登录" });
            }
            try
            {
                var saledata = new B2bCompanyImageData();
                var crmid = saledata.Deleteimage(comid, id,channelcompanyid);

                return JsonConvert.SerializeObject(new { type = 100, msg = crmid });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        //图片添加修改
        public static string InsertOrUpdate(B2b_company_image saleset)
        {
            try
            {
                var saledata = new B2bCompanyImageData();
                var crmid = saledata.InsertOrUpdate(saleset);

                return JsonConvert.SerializeObject(new { type = 100, msg = crmid });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        //门市图片添加修改
        public static string ChannelInsertOrUpdate(B2b_company_image saleset)
        {
            if (saleset.Channelcompanyid == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "操作错误，请重新登录" });
            }

            try
            {
                var saledata = new B2bCompanyImageData();
                var crmid = saledata.InsertOrUpdate(saleset);

                return JsonConvert.SerializeObject(new { type = 100, msg = crmid });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }



        public static string Getmenulist(int comid, int pageindex, int pagesize, int usetype = 0, int menuindex = 0)
        {
            int totalcount = 0;
            int totalcount1=0;
            int totalcount2 = 0;
            int totalcount3 = 0;
            try
            {
                var imagedata = new B2bCompanyMenuData();
                var prodata = new B2bComProData();
                var actdata = new WxMaterialData();
                var projectdata = new B2b_com_projectData();
                List<B2b_company_menu> list = imagedata.GetMenuList(comid, pageindex, pagesize, out totalcount, usetype, menuindex);
                IEnumerable result = "";
                if (list != null)
                {
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Com_id = pro.Com_id,
                                 Imgurl_address = FileSerivce.GetImgUrl(pro.Imgurl),
                                 Imgurl = pro.Imgurl,
                                 Linkurl = pro.Linkurl,
                                 Name = pro.Name,
                                 Fonticon = pro.Fonticon,
                                 Usestyle = pro.Usestyle,
                                 Menutype = pro.Menutype,
                                 Usetype = pro.Usetype,
                                 Projectlist = pro.Projectlist,
                                 menuindex= pro.menuindex,
                                 menuviewtype = pro.menuviewtype,
                                 hotellist = pro.menuviewtype == 1 ? projectdata.Projectpagelist(comid.ToString(), 1, 12, "1", out totalcount3, "", 1, pro.Projectlist, 9) : null,
                                 prolist = pro.Menutype==0 ? prodata.Selectpagelist_diaoyong(comid.ToString(), 1, 12, "", out totalcount1, pro.Projectlist, 0, pro.Id) : null,//读出每个栏目的产品，每页12个
                                 Materiallist = pro.Menutype == 0 ? null : actdata.ShopWxMaterialPageList(comid, 1, 12, 10, pro.Id, pro.Projectlist, out totalcount2, "")
                             };
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = result, totalCount = totalcount });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }


        public static string Getconsultantlist(int comid, int pageindex, int pagesize, int channelid=0)
        {
            int totalcount = 0;
            try
            {
                var imagedata = new B2bCompanyMenuData();
                var prodata = new B2bComProData();
                List<B2b_company_menu> list = imagedata.GetconsultantList(comid, pageindex, pagesize, out totalcount, channelid);
                IEnumerable result = "";
                if (list != null)
                {
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Com_id = pro.Com_id,
                                 Imgurl_address = FileSerivce.GetImgUrl(pro.Imgurl),
                                 Imgurl = pro.Imgurl,
                                 Linkurl = pro.Linkurl,
                                 Linktype = pro.Linktype,
                                 Projectlist = new B2b_com_projectData().GetProject(pro.Linktype, comid),
                                 Name = pro.Name,
                                 Fonticon = pro.Fonticon,
                                 Outdata= pro.Outdata
                             };
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = result, totalCount = totalcount });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }

        public static string GetMenuByComid(int comid, int id)
        {
            try
            {
                int totalcount1=0;
                int selectpro = 0;
                int totalcount2 = 0;
                List<WxMaterial> Material=null;
                var actdata = new WxMaterialData();
                B2b_company_menu list = B2bCompanyMenuData.GetMenuByComid(comid, id);
                var prodata = new B2bComProData();
                List<B2b_com_pro> Prolist =null;
                IEnumerable result = "";
                if (list != null)
                {
                    Prolist = prodata.Selectpagelist_diaoyong(comid.ToString(), 1, 12, "", out totalcount1, list.Projectlist, 0, id);//读出每个栏目的产品，每页12个
                    selectpro = prodata.Selectpagelist_ct(comid.ToString(), id);
                    list.Imgurl_address = FileSerivce.GetImgUrl(list.Imgurl);
                    Material = list.Menutype == 0 ? null : actdata.ShopWxMaterialPageList(comid, 1, 12, 10, id, list.Projectlist, out totalcount2, "");
                }


                return JsonConvert.SerializeObject(new { type = 100, msg = list, prolist = Prolist, selectpro = selectpro, Materiallist = Material });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }

        public static string GetConsultantByComid(int comid, int id)
        {
            try
            {
                B2b_company_menu com = B2bCompanyMenuData.GetConsultantByComid(comid, id);
                var prodata = new B2bComProData();
                var actdata = new WxMaterialData();
                List<B2b_com_pro> Prolist = null;
                List<WxMaterial> WxMaterial = null;
                int totalcount1 = 0;


                if (com.Outdata == 0)
                {
                    Prolist = prodata.Selectpagelist_diaoyong(comid.ToString(), 1, 50, "", out totalcount1, 0, 0, 0, id);//读出每个栏目的产品，每页12个
                }
                if (com.Outdata == 2)
                {
                    WxMaterial = actdata.WxMaterialPageList(comid, 1, 100, 10, 1000000, out totalcount1, "", 0, id);
                }
                return JsonConvert.SerializeObject(new { type = 100, msg = com, Prolist = Prolist, WxMaterial = WxMaterial });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }

        public static string GetChannelProList(int comid, int id)
        {
            try
            {
                    var prodata = new B2bComProData();
                    int totalcount1 = 0;
                    var Prolist = prodata.Selectpagelist_diaoyong(comid.ToString(), 1, 50, "", out totalcount1, 0, 0, 0, id);//读出每个栏目的产品，每页12个
                    return JsonConvert.SerializeObject(new { type = 100, msg = Prolist});
                
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }
        public static string DeleteConsultant(int comid, int id)
        {
            try
            {
                var saledata = new B2bCompanyMenuData();
                var crmid = saledata.DeleteConsultant(comid, id);
                return JsonConvert.SerializeObject(new { type = 100, msg = crmid });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string Deletemenu(int comid, int id)
        {
            try
            {
                var saledata = new B2bCompanyMenuData();
                var crmid = saledata.Deletemenu(comid, id);

                return JsonConvert.SerializeObject(new { type = 100, msg = crmid });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string GetButtonByComid(int comid,int id)
        {
            if (id == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "参数错误" });
            }
            else
            {
                var saledata = new B2bCompanyMenuData();
                var crmid = saledata.GetButtonByComid(comid, id);

                return JsonConvert.SerializeObject(new { type = 100, msg = crmid });

            }
        }

        public static string GetButtonlist(int comid, int pageindex, int pagesize, int linktype = 0)
        {
            int totalcount = 0;
            try
            {
                var imagedata = new B2bCompanyMenuData();
                var prodata = new B2bComProData();

                List<B2b_company_Button> list = imagedata.GetButtonlist(comid, pageindex, pagesize, linktype, out totalcount);
                IEnumerable result = "";
                if (list != null)
                {
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Com_id = pro.Comid,
                                 Linkurl = pro.Linkurl,
                                 Name = pro.Name,
                                 Linkurlname = pro.Linkurlname,
                                 Linktype = pro.Linktype,
                             };
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = result, totalCount = totalcount });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }


        public static string UpDownState(int comid, int id, int channelcompanyid=0)
        {
            try
            {
                int state=0;
                
                //获取原来的状态，无法获得都暂停
                var imgdata = B2bCompanyImageData.GetimageByComid(comid, id, channelcompanyid);
                if (imgdata != null) {
                    if (imgdata.State == 0) {
                        state = 1;
                    }
                }

                var saledata = new B2bCompanyImageData();
                var crmid = saledata.UpDownState(comid, id,state);

                return JsonConvert.SerializeObject(new { type = 100, msg = crmid });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


        public static string MenuSort(string ids)
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
                    int sortmenu = new B2bCompanyMenuData().SortMenu(materialid, sortid);
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



        public static string SortConsultant(string ids)
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
                    int sortmenu = new B2bCompanyMenuData().SortConsultant(materialid, sortid);
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



        //图片添加修改
        public static string InsertOrUpdate(B2b_company_menu saleset)
        {
            try
            {
                var saledata = new B2bCompanyMenuData();
                var crmid = saledata.InsertOrUpdate(saleset);

                //录入成功后操作子表 显示产品
                if (crmid != 0) {

                    //先删除原先显示的数据
                    var delmenu_pro = saledata.deletemenu_pro(saleset.Com_id, crmid);

                    //重新判断再逐一插入
                    if (saleset.Prolist != "") {
                        var proid_arr = saleset.Prolist.Split(',');
                        for (int i = 0; i < proid_arr.Count(); i++) {
                            if (proid_arr[i] != "") {
                                if (int.Parse(proid_arr[i]) != 0)
                                {
                                    var insertmenu_pro = saledata.Insertmenu_pro(saleset.Com_id, int.Parse(proid_arr[i]), crmid);
                                }
                            }
                        }
                    
                    }
                
                }



                return JsonConvert.SerializeObject(new { type = 100, msg = crmid });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


        //底部菜单管理
        public static string ButtonInsertOrUpdate(B2b_company_Button saleset)
        {
            try
            {
                var saledata = new B2bCompanyMenuData();
                var crmid = saledata.ButtonInsertOrUpdate(saleset);

                return JsonConvert.SerializeObject(new { type = 100, msg = crmid });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


        public static string DeleteButton(int comid, int id)
        {
            try
            {
                var saledata = new B2bCompanyMenuData();
                var crmid = saledata.DeleteButton(comid, id);

                return JsonConvert.SerializeObject(new { type = 100, msg = crmid });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


        //图片添加修改
        public static string ConsultantInsertOrUpdate(B2b_company_menu saleset)
        {
            try
            {
                var saledata = new B2bCompanyMenuData();
                var crmid = saledata.ConsultantInsertOrUpdate(saleset);


                //录入成功后操作子表 显示产品
                if (crmid != 0)
                {

                    //先删除原先显示的数据
                    var delmenu_pro = saledata.deleteConsultant_pro(saleset.Com_id, crmid);

                    //重新判断再逐一插入
                    if (saleset.Prolist != "")
                    {
                        var proid_arr = saleset.Prolist.Split(',');
                        for (int i = 0; i < proid_arr.Count(); i++)
                        {
                            if (proid_arr[i] != "")
                            {
                                if (int.Parse(proid_arr[i]) != 0)
                                {
                                    var insertmenu_pro = saledata.InsertConsultant_pro(saleset.Com_id, int.Parse(proid_arr[i]), crmid);
                                }
                            }
                        }

                    }

                }
                return JsonConvert.SerializeObject(new { type = 100, msg = crmid });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


    }
}
