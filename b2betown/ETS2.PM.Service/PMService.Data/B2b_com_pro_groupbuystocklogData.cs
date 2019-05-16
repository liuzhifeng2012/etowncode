using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data.InternalData;
using ETS.Data.SqlHelper;

namespace ETS2.PM.Service.PMService.Data
{
    public class B2b_com_pro_groupbuystocklogData
    {
        public List<B2b_com_pro_groupbuystocklog> GroupbuyStockLogPagelist(int pageindex, int pagesize, string key, int groupbuytype, string stockstate, int comid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            { 
               var list = new InternalB2b_com_pro_groupbuystocklog(helper).GroupbuyStockLogPagelist(  pageindex,   pagesize,   key,   groupbuytype,   stockstate,   comid,    out   totalcount);

                return list;

            }
        }

        public int GetStocklogCount(int productid, int stockagentcompanyid)
        {
           using(var helper=new SqlHelper())
           {
               int count = new InternalB2b_com_pro_groupbuystocklog(helper).GetStocklogCount(productid, stockagentcompanyid);
               return count;
           }    
        }

        public int EditStocklog(B2b_com_pro_groupbuystocklog m)
        {
            using(var helper=new SqlHelper())
            {
                int result = new InternalB2b_com_pro_groupbuystocklog(helper).EditStocklog(m);
                return result;
            }
        }

        public int DownStockPro(int proid, int agentcompanyid)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalB2b_com_pro_groupbuystocklog(helper).DownStockPro(proid,agentcompanyid);
                return result;
            }
        }
        /// <summary>
        /// 获得当前美团分销上架的包含在以上产品列表中的产品
        /// </summary>
        /// <param name="proidlist"></param>
        /// <param name="agentcompanyid"></param>
        /// <returns></returns>
        public List<int> GetChildStockProidList(List<int> proidlist, int agentcompanyid)
        {
            using (var helper = new SqlHelper())
            {
                List<int> result = new InternalB2b_com_pro_groupbuystocklog(helper).GetChildStockProidList(proidlist, agentcompanyid);
                return result;
            }
        }
        /// <summary>
        /// 得到产品项目下已经在美团上架的产品(也包括导入这个项目下产品 的导入产品) 
        /// </summary>
        /// <param name="proprojectid"></param>
        /// <returns></returns>
        public List<int> GetStockProidListByProproject(int proprojectid)
        {
            using (var helper = new SqlHelper())
            {
                List<int> result = new InternalB2b_com_pro_groupbuystocklog(helper).GetStockProidListByProproject(proprojectid);
                return result;
            }
        }
        /// <summary>
        /// 判断是否是美团上架产品
        /// </summary>
        /// <param name="proid"></param>
        /// <returns></returns>
        public bool IsStockPro(int proid)
        {
             using(var helper=new SqlHelper())
             {
                 bool r = new InternalB2b_com_pro_groupbuystocklog(helper).IsStockPro(proid);
                 return r;
             }
        }
        /// <summary>
        /// 得到上架美团绑定此产品的绑定产品列表
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public List<int> GetStockbindingproidlistByProid(int proid)
        {
            using (var helper = new SqlHelper())
            {
                List<int> r = new InternalB2b_com_pro_groupbuystocklog(helper).GetStockbindingproidlistByProid(proid);
                return r;
            }
        }

        /// <summary>
        /// 修改产品的上单状态
        /// </summary>
        /// <param name="proid"></param>
        /// <param name="groupbuystatus"></param>
        /// <param name="groupbuystatusdesc"></param>
        /// <returns></returns>
        public int UpGroupbuystatus(int proid,int agentid, int groupbuystatus, string groupbuystatusdesc)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2b_com_pro_groupbuystocklog(helper).UpGroupbuystatus(proid,agentid,groupbuystatus,groupbuystatusdesc);
                return r;
            }
        }
    }
}
