using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ETS.Framework
{
    public class ReqUrl_Log
    {
        public int mid { get; set; }
        public string urlstr { get; set; }//完整网址
        public string hoststr { get; set; }//域名
        public string paramstr { get; set; }//参数
        public DateTime subtime { get; set; }//保存时间
    }
    public class ReqUrl_LogHelper
    {

        public int InsReqUrlLog(ReqUrl_Log requrllog)
        {
            try
            {
                object o = ExcelSqlHelper.ExecuteScalar("insert into ReqUrl_Log(urlstr,hoststr,paramstr,subtime) values('" + requrllog.urlstr + "','" + requrllog.hoststr + "','" + requrllog.paramstr + "','" + requrllog.subtime + "');select @@identity;");
                return int.Parse(o.ToString());
            }
            catch
            {
                return 0;
            }
        }

        internal ReqUrl_Log GetReqUrlLogById(string mid)
        {
            DataTable dt = ExcelSqlHelper.ExecuteDataTable("select * from ReqUrl_Log where mid=" + mid);
            if (dt != null)
            {
                ReqUrl_Log m = new ReqUrl_Log
                {
                    mid = int.Parse(dt.Rows[0]["mid"].ToString()),
                    urlstr = dt.Rows[0]["urlstr"].ToString(),
                    hoststr = dt.Rows[0]["hoststr"].ToString(),
                    paramstr = dt.Rows[0]["paramstr"].ToString(),
                    subtime = DateTime.Parse(dt.Rows[0]["subtime"].ToString())
                };
                return m;
            }
            else
            {
                return null;
            }
        }
    }
}
