using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.Member.Service.MemberService.Data.InternalData;
using ETS2.Member.Service.MemberService.Model;

namespace ETS2.Member.Service.MemberService.Data
{
    public class MemberIssueActivityData
    {
        public int DelIssueIdByIssueId(int issueid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {

                    int result = new InternalMemberIssueActivity(sql).DelIssueIdByIssueId(issueid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public int InsertOrUpdate(Model.Member_Issue_Activity act)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalMemberIssueActivity(helper).InsertOrUpdate(act);
                return id;
            }
        }



        public List<Member_Issue_Activity> GetIssuePromot(int issueid)
        {
    
            using (var helper = new SqlHelper())
            {
                var id = new InternalMemberIssueActivity(helper).GetIssuePromot(issueid);
                return id;
            }
        }

        public string GetIssueActStr(int isid)
        {
            
            using (var helper=new SqlHelper())
            {
                var actstr = new InternalMemberIssueActivity(helper).GetIssueActStr(isid);
                return actstr;
            }
        }
    }
}
