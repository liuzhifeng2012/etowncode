using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Model;

namespace ETS2.WeiXin.Service.WeiXinService.Data.InternalData
{
    public class InternalWx_onlinekf_huoqurecord
    {
        public SqlHelper sqlHelper;
        public InternalWx_onlinekf_huoqurecord(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal bool Ishuoqued(int comid)
        {
            string sql = "select count(1) from wx_onlinekf_huoqurecord where comid=" + comid + "  and  huoqu_time>='" + DateTime.Now.AddSeconds(-5) + "'";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            if (int.Parse(o.ToString()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal int InsertRecord(Wx_onlinekf_huoqurecord record)
        {
            int issuc = record.Huoqu_issuc == true ? 1 : 0;

            string sql = "INSERT INTO [EtownDB].[dbo].[wx_onlinekf_huoqurecord]([comid],[huoqu_time],[huoqu_issuc],[huoqu_content])" +
                " VALUES(" + record.Comid + ",'" + record.Huoqu_time + "'," +issuc +",'" + record.Huoqu_content + "')";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }
    }
}
