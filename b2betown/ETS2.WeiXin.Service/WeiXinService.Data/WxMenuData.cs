using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Data.InternalData;

namespace ETS2.WeiXin.Service.WeiXinService.Data
{
    public class WxMenuData
    {
        public WxMenu GetWxMenu(int menuid)
        {
            using (var helper = new SqlHelper())
            {
                WxMenu wxMenu = new InternalWxMenu(helper).GetWxMenu(menuid);
                return wxMenu;
            }
        }
        public WxMenu GetWxMenu(int menuid, int comid)
        {
            using (var helper = new SqlHelper())
            {
                WxMenu wxMenu = new InternalWxMenu(helper).GetWxMenu(menuid, comid);
                return wxMenu;
            }
        }
        public int EditWxMenu(WxMenu wxmenu)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalWxMenu(helper).EditWxMenu(wxmenu);
                return id;
            }
        }
        public List<WxMenu> GetFristMenuList(int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMenu(helper).GetFristMenuList(pageindex, pagesize, out totalcount);

                return list;
            }
        }
        public List<WxMenu> GetFristMenuList(int pageindex, int pagesize, int comid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMenu(helper).GetFristMenuList(pageindex, pagesize, comid, out totalcount);

                return list;
            }
        }
        public List<WxMenu> GetSecondMenuList(int fatherid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMenu(helper).GetSecondMenuList(fatherid, out totalcount);

                return list;
            }
        }
        public List<WxMenu> GetSecondMenuList(int fatherid, int comid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMenu(helper).GetSecondMenuList(fatherid, comid, out totalcount);

                return list;
            }
        }

        public int Delwxmenu(int wxmenuid)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalWxMaterial(helper).Delwxmenu(wxmenuid);
                return id;
            }
        }

        public IList<WxMenu> GetMenuList(out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMenu(helper).GetMenuList(out totalcount);

                return list;
            }
        }

        public int DelChildrenMenu(int fathermenuid)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalWxMenu(helper).DelChildrenMenu(fathermenuid);
                return id;
            }
        }

        public IList<WxMenu> GetMenuList(int fathermenuid, out int totalcount)
        {

            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMenu(helper).GetMenuList(fathermenuid, out totalcount);

                return list;
            }
        }
        public IList<WxMenu> GetMenuList(int fathermenuid, int comid, out int totalcount)
        {

            using (var helper = new SqlHelper())
            {

                var list = new InternalWxMenu(helper).GetMenuList(fathermenuid, comid, out totalcount);

                return list;
            }
        }

        public int SortMenu(string menuid, int sortid)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalWxMenu(helper).SortMenu(menuid, sortid);
                return id;
            }
        }




        public int EditMaterialType(int id, string typename, string typeclass, int comid,bool isshowpast)
        {
            using (var helper = new SqlHelper())
            {
                var did = new InternalWxMenu(helper).EditMaterialType(id, typename, typeclass, comid,isshowpast);
                return id;
            }
        }
        /// <summary>
        /// 根据操作类型得到微信菜单
        /// </summary>
        /// <param name="opertype"></param>
        /// <param name="comid"></param>
        /// <returns></returns>
        public WxMenu GetWxMenuByOperType(int opertype, int comid)
        {
           using(var helper=new SqlHelper())
           {
               WxMenu m = new InternalWxMenu(helper).GetWxmenuByOperType(opertype,comid);
               return m;
           }
        }
    }
}
