using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Data.InternalData;

namespace ETS2.WeiXin.Service.WeiXinService.Data
{
   public class WxSalePromoteTypeData
    {
       public WxSalePromoteType GetWxMenu(int id)
       {
           using (var helper = new SqlHelper())
           {
               WxSalePromoteType WsptMenu = new InternalWxSalePromoteType(helper).GetWsptMenu(id);
               return WsptMenu;
           }
       }

       public List<WxSalePromoteType> Wxmaterialtypepagelist(int pageindex, int pagesize, out int totalcount)
       {
           using (var helper = new SqlHelper())
           {
               List<WxSalePromoteType> WsptMenu = new InternalWxSalePromoteType(helper).Wxmaterialtypepagelist(pageindex,pagesize,out totalcount);
               return WsptMenu;
           }
       }
       public List<WxSalePromoteType> Wxmaterialtypepagelist(int pageindex, int pagesize,int comid, out int totalcount)
       {
           using (var helper = new SqlHelper())
           {
               List<WxSalePromoteType> WsptMenu = new InternalWxSalePromoteType(helper).Wxmaterialtypepagelist(pageindex, pagesize,comid, out totalcount);
               return WsptMenu;
           }
       }

       public WxSalePromoteType GetMaterialType(int id)
       {
           using (var helper = new SqlHelper())
           {
               WxSalePromoteType WsptMenu = new InternalWxSalePromoteType(helper).GetMaterialType(id);
               return WsptMenu;
           }
       }
       public WxSalePromoteType GetMaterialType(int id,int comid)
       {
           using (var helper = new SqlHelper())
           {
               WxSalePromoteType WsptMenu = new InternalWxSalePromoteType(helper).GetMaterialType(id,comid);
               return WsptMenu;
           }
       }
       public List<WxSalePromoteType> GetAllWxMaterialType(int comid,out int totalcount)
       {
           using (var helper = new SqlHelper())
           {
               List<WxSalePromoteType> WsptMenu = new InternalWxSalePromoteType(helper).GetAllWxMaterialType(comid,out totalcount);
               return WsptMenu;
           }
       }
       public List<WxSalePromoteType> GetAllWxMaterialType(out int totalcount)
       {
           using (var helper = new SqlHelper())
           {
               List<WxSalePromoteType> WsptMenu = new InternalWxSalePromoteType(helper).GetAllWxMaterialType(out totalcount);
               return WsptMenu;
           }
       }

       public List<WxSalePromoteType> GetRecommendWxMaterialType(int comid, out int totalcount)
       {
           using (var helper = new SqlHelper())
           {
               List<WxSalePromoteType> WsptMenu = new InternalWxSalePromoteType(helper).GetRecommendWxMaterialType( comid,out totalcount);
               return WsptMenu;
           }
       }
    }
}
