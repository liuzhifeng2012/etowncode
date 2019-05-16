using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Data.InternalData;

namespace ETS2.WeiXin.Service.WeiXinService.Data
{
    public class Wx_onlinekf_huoqurecordData
    {
        
        /// <summary>
        ///判断商户10分钟内是否获取过在线客服列表
        /// </summary>
        /// <param name="comid"></param>
        /// <returns></returns>
        internal bool Ishuoqued(int comid)
        {
            using (var helper=new SqlHelper())
            {
                bool r = new InternalWx_onlinekf_huoqurecord(helper).Ishuoqued(comid);
                return r;
            }
        }
        /// <summary>
        /// 录入在线客服获取列表记录
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        internal int InsertRecord(Model.Wx_onlinekf_huoqurecord record)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalWx_onlinekf_huoqurecord(helper).InsertRecord(record);
                return r;
            }
        }
    }
}
