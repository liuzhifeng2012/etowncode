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
   public class Bus_FeeticketData
    {
       #region 编辑大巴车免费券
       public int Bus_FeeticketInsertOrUpdate(Bus_Feeticket businfo)
       {
           using (var sql = new SqlHelper())
           {
               try
               {
                   var internalData = new InternalBus_Feeticket(sql);
                   int result = internalData.Bus_FeeticketInsertOrUpdate(businfo);
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
       public List<Bus_Feeticket> Bus_Feeticketpagelist(int comid, int pageindex, int pagesize, out int totalcount)
       {
           using (var helper = new SqlHelper())
           {

               var list = new InternalBus_Feeticket(helper).Bus_Feeticketpagelist(comid, pageindex, pagesize, out totalcount);

               return list;
           }
       }
       #endregion

       #region 查询
       public Bus_Feeticket GetBus_FeeticketById(int id, int comid)
       {
           using (var helper = new SqlHelper())
           {

               var pro = new InternalBus_Feeticket(helper).GetBus_FeeticketById(id, comid);

               return pro;
           }
       }
       #endregion


       #region 删除
       public int DeleteBus_FeeticketById(int id, int comid)
       {
           using (var helper = new SqlHelper())
           {

               var pro = new InternalBus_Feeticket(helper).DeleteBus_FeeticketById(id, comid);

               return pro;
           }
       }
       #endregion


       #region 导入大巴车免费券
       public int Bus_Feeticket_pnoInsertOrUpdate(Bus_Feeticket_pno businfo)
       {
           using (var sql = new SqlHelper())
           {
               try
               {
                   var internalData = new InternalBus_Feeticket(sql);
                   int result = internalData.Bus_Feeticket_pnoInsertOrUpdate(businfo);
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
       public List<Bus_Feeticket_pno> Bus_Feeticket_pnopagelist(int busid, int pageindex, int pagesize, out int totalcount)
       {
           using (var helper = new SqlHelper())
           {

               var list = new InternalBus_Feeticket(helper).Bus_Feeticket_pnopagelist(busid, pageindex, pagesize, out totalcount);

               return list;
           }
       }
       #endregion

       #region 查询
       public Bus_Feeticket_pno Bus_Feeticket_pnoById(int id,int busid,int proid=0)
       {
           using (var helper = new SqlHelper())
           {

               var pro = new InternalBus_Feeticket(helper).GetBus_Feeticket_pnoById(id, busid,proid);

               return pro;
           }
       }
       #endregion


       #region 删除
       public int DeleteBus_Feeticket_pnoById(int id, int busid)
       {
           using (var helper = new SqlHelper())
           {

               var pro = new InternalBus_Feeticket(helper).DeleteBus_Feeticket_pnoById(id, busid);

               return pro;
           }
       }
       #endregion



       #region 设定可使用大巴车免费券的产品
       public int Bus_feeticket_ProInsert(Bus_feeticket_Pro businfo)
       {
           using (var sql = new SqlHelper())
           {
               try
               {
                   var internalData = new InternalBus_Feeticket(sql);
                   int result = internalData.Bus_feeticket_ProInsert(businfo);
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
       public List<Bus_feeticket_Pro> Bus_Feeticket_propagelist(int busid, int pageindex, int pagesize, out int totalcount)
       {
           using (var helper = new SqlHelper())
           {

               var list = new InternalBus_Feeticket(helper).Bus_feeticket_Propagelist(busid, pageindex, pagesize, out totalcount);

               return list;
           }
       }
       #endregion

       #region 查询
       public Bus_feeticket_Pro Bus_Feeticket_proById(int id, int busid,int proid=0,string pno="")
       {
           using (var helper = new SqlHelper())
           {

               var pro = new InternalBus_Feeticket(helper).GetBus_feeticket_ProById(id, busid, proid, pno);

               return pro;
           }
       }
       #endregion

       #region 根据产品查询是否属于验证码级
       public int BusSearchpnobyproid(int comid, int proid)
       {
           using (var helper = new SqlHelper())
           {

               var pro = new InternalBus_Feeticket(helper).BusSearchpnobyproid(comid, proid);

               return pro;
           }
       }
       #endregion



       #region 删除
       public int DeleteBus_feeticket_ProById(int id, int busid)
       {
           using (var helper = new SqlHelper())
           {

               var pro = new InternalBus_Feeticket(helper).DeleteBus_feeticket_ProById(id, busid);

               return pro;
           }
       }
       #endregion


       #region 查询
       public List<B2b_com_pro> busfeeticketbindingpropagelist(int pageindex, int pagesize, int busid, int bindingprotype, int comid, out int totalcount)
       {
           using (var helper = new SqlHelper())
           {

               var list = new InternalBus_Feeticket(helper).busfeeticketbindingpropagelist(pageindex, pagesize, busid, bindingprotype, comid, out totalcount);

               return list;
           }
       }
       #endregion


       #region 绑定
       public int Busbindingpro(int busid, int comid, int proid, int type, int subtype, int limitweek, int limitweekdaynum, int limitweekendnum)
       {
           using (var helper = new SqlHelper())
           {

               var pro = new InternalBus_Feeticket(helper).Busbindingpro(busid, comid, proid, type, subtype,limitweek,limitweekdaynum,limitweekendnum);

               return pro;
           }
       }
       #endregion

       #region 查询
       public List<Bus_feeticket_Pro> BusFeeticketpropagelist(int busid, int pageindex, int pagesize, out int totalcount)
       {
           using (var helper = new SqlHelper())
           {

               var list = new InternalBus_Feeticket(helper).BusFeeticketpropagelist(busid, pageindex, pagesize, out totalcount);

               return list;
           }
       }
       #endregion

       #region 查询
       public List<Bus_Feeticket_pno> BusFeeticketpnopagelist(int busid, int pageindex, int pagesize, out int totalcount)
       {
           using (var helper = new SqlHelper())
           {

               var list = new InternalBus_Feeticket(helper).BusFeeticketpnopagelist(busid, pageindex, pagesize, out totalcount);

               return list;
           }
       }
       #endregion


       #region 根据产品查询是否属于验证码级
       public int BusSearchpno_propipei(string pno, int proid)
       {
           using (var helper = new SqlHelper())
           {

               var pro = new InternalBus_Feeticket(helper).BusSearchpno_propipei(pno, proid);

               return pro;
           }
       }
       #endregion

    }
}
