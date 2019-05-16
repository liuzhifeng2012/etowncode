using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Model;

namespace ETS2.WeiXin.Service.WeiXinService.Data.InternalData
{
    public class InternalWxdelivernotify
    {
        public SqlHelper sqlHelper;
        public InternalWxdelivernotify(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int EditWxdelivernotify(Wxdelivernotify m_d)
        {
            if (m_d.Id == 0)
            {
                string sql = "INSERT INTO  [wxdelivernotify]([out_trade_no] ,[appid],[openid],[transid],[deliver_timestamp],[timeformat],[deliver_status] ,[deliver_msg],[requestxml],[responsexml],[errcode] ,[errmsg] ,[comid]) VALUES "+
                                   "('"+m_d.Out_trade_no+"','"+m_d.Appid+"','"+m_d.Openid+"','"+m_d.Transid+"','"+m_d.Deliver_timestamp+"','"+m_d.Timeformat+"','"+m_d.Deliver_status+"','"+m_d.Deliver_msg+"','"+m_d.Requestxml+"','"+m_d.Responsexml+"','"+m_d.Errcode+"','"+m_d.Errmsg+"','"+m_d.Comid+"');select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
            else 
            {
                string sql = "UPDATE [EtownDB].[dbo].[wxdelivernotify] "+
                               "SET [out_trade_no] = '"+m_d.Out_trade_no+"'"+
                                  ",[appid] = '"+m_d.Appid+"'"+
                                  ",[openid] = '"+m_d.Openid+"'"+
                                  ",[transid] = '"+m_d.Transid+"'"+
                                  ",[deliver_timestamp] ='"+m_d.Deliver_timestamp+"'"+
                                  ",[timeformat] = '"+m_d.Timeformat+"'"+
                                  ",[deliver_status] = '"+m_d.Deliver_status+"'"+
                                  ",[deliver_msg] = '"+m_d.Deliver_msg+"'"+
                                  ",[requestxml] ='"+m_d.Requestxml+"'"+
                                  ",[responsexml] = '"+m_d.Responsexml+"'"+
                                  ",[errcode] = '"+m_d.Errcode+"'"+
                                  ",[errmsg] = '"+m_d.Errmsg+"'"+
                                  ",[comid] = '"+m_d.Comid+"'"+
                             "WHERE id="+m_d.Id;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.ExecuteNonQuery();
                return m_d.Id;
            }
        }
    }
}
