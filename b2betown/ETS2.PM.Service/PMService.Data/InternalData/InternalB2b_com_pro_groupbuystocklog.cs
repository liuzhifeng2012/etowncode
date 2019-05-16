using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalB2b_com_pro_groupbuystocklog
    {
        private SqlHelper sqlHelper;
        public InternalB2b_com_pro_groupbuystocklog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal List<B2b_com_pro_groupbuystocklog> GroupbuyStockLogPagelist(int pageindex, int pagesize, string key, int groupbuytype, string stockstate, int comid, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var tblName = "b2b_com_pro_groupbuystocklog";
            var strGetFields = "*";
            var sortKey = "id desc";

            var condition = "comid=" + comid;
            if (groupbuytype > 0)
            {
                condition += " and groupbuytype=" + groupbuytype;
            }

            if (stockstate != "0,1")
            {
                condition += " and isstock = " + stockstate.ConvertTo<int>();
            }


            if (key != "")
            {
                if (key.ConvertTo<int>(0) == 0)
                {
                    condition += " and proname like '%" + key + "%'";
                }
                else
                {
                    condition += " and  proid=" + key;
                }
            }



            //cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);
            cmd.PagingCommand1(tblName, strGetFields, sortKey, "", pagesize, pageindex, "0", condition);

            List<B2b_com_pro_groupbuystocklog> list = new List<B2b_com_pro_groupbuystocklog>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_com_pro_groupbuystocklog
                    {
                        id = reader.GetValue<int>("id"),
                        proid = reader.GetValue<int>("proid"),
                        proname = reader.GetValue<string>("proname"),
                        stocktime = reader.GetValue<DateTime>("stocktime"),
                        operuserid = reader.GetValue<int>("operuserid"),
                        isstock = reader.GetValue<int>("isstock"),
                        groupbuytype = reader.GetValue<int>("groupbuytype"),
                        comid = reader.GetValue<int>("comid"),
                        stockagentcompanyid = reader.GetValue<int>("stockagentcompanyid"),
                        stockagentcompanyname = reader.GetValue<string>("stockagentcompanyname")

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }



        internal int GetStocklogCount(int productid, int stockagentcompanyid)
        {
            string sql = "select count(1) from b2b_com_pro_groupbuystocklog where proid=" + productid + " and stockagentcompanyid=" + stockagentcompanyid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            return o == null ? 0 : int.Parse(o.ToString());
        }

        internal int EditStocklog(B2b_com_pro_groupbuystocklog m)
        {
            if (m.id > 0)
            {
                string sql = "UPDATE  [b2b_com_pro_groupbuystocklog] "
                                 + " SET [proid] =@proid"
                                      + ",[proname] =@proname "
                                      +" ,[isstock] = @isstock"
                                      + ",[stocktime] = @stocktime"
                                      + ",[operuserid] =  @operuserid"
                                      + ",[comid] = @comid "
                                      + ",[stockagentcompanyid] = @stockagentcompanyid "
                                      + ",[stockagentcompanyname] =@stockagentcompanyname"
                                      + ",[groupbuytype] = @groupbuytype"
                                      + ",[groupbuystatus] =  @groupbuystatus"
                                      + ",[groupbuystatusdesc] =@groupbuystatusdesc "
                                 + " WHERE id=" + m.id;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@proid", m.proid);
                cmd.AddParam("@proname", m.proname);
                cmd.AddParam("@isstock", m.isstock);
                cmd.AddParam("@stocktime", m.stocktime);
                cmd.AddParam("@operuserid", m.operuserid);
                cmd.AddParam("@comid", m.comid);
                cmd.AddParam("@stockagentcompanyid", m.stockagentcompanyid);
                cmd.AddParam("@stockagentcompanyname", m.stockagentcompanyname);
                cmd.AddParam("@groupbuytype", m.groupbuytype);
                cmd.AddParam("@groupbuystatus", m.groupbuystatus);
                cmd.AddParam("@groupbuystatusdesc", m.groupbuystatusdesc);

                cmd.ExecuteNonQuery();
                return m.id;
            }
            else
            {
                string sql = "INSERT INTO [b2b_com_pro_groupbuystocklog] "+
           "([proid] "+
           ",[proname] "+
           ",[isstock] "+
           ",[stocktime] "+
           ",[operuserid] "+
           ",[comid] "+
           ",[stockagentcompanyid] "+
           ",[stockagentcompanyname] "+
           ",[groupbuytype] "+
           ",[groupbuystatus] "+
           ",[groupbuystatusdesc]) "+
     "VALUES "+
           "(@proid"+
           ",@proname"+
           ",@isstock"+
           ",@stocktime"+
           ",@operuserid "+
           ",@comid "+
           ",@stockagentcompanyid "+
           ",@stockagentcompanyname "+
           ",@groupbuytype "+
           ",@groupbuystatus"+
           ",@groupbuystatusdesc);select @@identity;";

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@proid", m.proid);
                cmd.AddParam("@proname", m.proname);
                cmd.AddParam("@isstock", m.isstock);
                cmd.AddParam("@stocktime", m.stocktime);
                cmd.AddParam("@operuserid", m.operuserid);
                cmd.AddParam("@comid", m.comid);
                cmd.AddParam("@stockagentcompanyid", m.stockagentcompanyid);
                cmd.AddParam("@stockagentcompanyname", m.stockagentcompanyname);
                cmd.AddParam("@groupbuytype", m.groupbuytype);
                cmd.AddParam("@groupbuystatus", m.groupbuystatus);
                cmd.AddParam("@groupbuystatusdesc", m.groupbuystatusdesc);


                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
        }

        internal int DownStockPro(int proid, int agentcompanyid)
        {
            string sql = "update b2b_com_pro_groupbuystocklog set isstock=0 where proid=" + proid + " and stockagentcompanyid="+agentcompanyid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 获得当前美团分销上架的包含在以上产品列表中的产品
        /// </summary>
        /// <param name="proidlist"></param>
        /// <param name="agentcompanyid"></param>
        /// <returns></returns>
        internal List<int> GetChildStockProidList(List<int> proidlist, int agentcompanyid)
        {
            string arr="";
            foreach(int item in proidlist){
              arr+=item.ToString()+",";
            }
            arr=arr.Substring(0,arr.Length-1);
            string sql = "select proid from b2b_com_pro_groupbuystocklog where stockagentcompanyid=" + agentcompanyid + " and proid in (" + arr + ") and isstock=1";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            List<int> list = new List<int>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(
                         reader.GetValue<int>("proid") 
                    ); 
                }
            }
            return list;
        }

        internal List<int> GetStockProidListByProproject(int proprojectid)
        {
            string sql = "select proid from b2b_com_pro_groupbuystocklog where isstock=1 and  (proid in (select id from b2b_com_pro where projectid=" + proprojectid + "  and pro_state=1) or proid in (select id from b2b_com_pro where bindingid in (select id from b2b_com_pro where projectid=" + proprojectid + ")  and pro_state=1))";
         
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            List<int> list = new List<int>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(
                         reader.GetValue<int>("proid")
                    );
                }
            }
            return list;
        }

        internal bool IsStockPro(int proid)
        {
            string sql = "select count(1) from b2b_com_pro_groupbuystocklog where isstock=1 and proid='"+proid+"'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            if (int.Parse(o.ToString()) > 0)
            {
                return true;
            }
            else {
                return false;
            }
        }

        internal List<int> GetStockbindingproidlistByProid(int proid)
        {
            string sql = "select proid from b2b_com_pro_groupbuystocklog where isstock=1 and proid in (select id from b2b_com_pro where bindingid="+proid+" and pro_state=1)";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            List<int> list = new List<int>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(
                         reader.GetValue<int>("proid")
                    );
                }
            }
            return list;
        }

        internal int UpGroupbuystatus(int proid,int agentid, int groupbuystatus, string groupbuystatusdesc)
        {
            string sql = "update b2b_com_pro_groupbuystocklog set groupbuystatus=@groupbuystatus,groupbuystatusdesc=@groupbuystatusdesc where proid=@proid and stockagentcompanyid=@stockagentcompanyid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@groupbuystatus", groupbuystatus);
            cmd.AddParam("@groupbuystatusdesc", groupbuystatusdesc);
            cmd.AddParam("@proid", proid);
            cmd.AddParam("@stockagentcompanyid", agentid);

            return cmd.ExecuteNonQuery();

        }
    }
}
