using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.Member.Service.MemberService.Model;
using ETS.Data.SqlHelper;
using ETS2.Member.Service.MemberService.Data.InternalData;

namespace ETS2.Member.Service.MemberService.Data
{
    public class MemberForwardingData
    {

        public int Forwardingcount(int uid, int wxmaid,string uip, int comid)
        {
            using (var helper = new SqlHelper())
            {
                //查询此账户IP 是否访问过
                var logstate = new InternalMemberForwarding(helper).Forwardingcount_search(uid, wxmaid, uip, comid);

                if (logstate == 0)//没有访问
                {
                    //访问写入日志表
                    var forlog = new InternalMemberForwarding(helper).Forwardingcountlog_add(uid, wxmaid, uip, comid);
                    if (forlog != 0) {
                        var forwarding = new InternalMemberForwarding(helper).Forwardingcount_add(uid, wxmaid,comid);
                        var forwardinginsert=new InternalMemberForwarding(helper).ForwardingcountInsert(uid, wxmaid,comid,forwarding);
                        return forwardinginsert;
                    }

                    return -2;//写入日志错误
                }
                else { //有访问
                   
                    return -1;//不重复统计
                }

            }
        }

        //插入数据库或+1
        public int ForwardingcountInsert(int uid, int wxmaid, int comid, int weixinjilu)
        {
            using (var helper = new SqlHelper())
            {
                var forwarding = new InternalMemberForwarding(helper).ForwardingcountInsert(uid, wxmaid, comid, weixinjilu);
                 return forwarding;
            }
        }


    }
}
