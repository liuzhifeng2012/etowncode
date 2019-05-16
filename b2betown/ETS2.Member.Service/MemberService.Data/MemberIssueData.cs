using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.Member.Service.MemberService.Model;
using ETS.Data.SqlHelper;
using ETS2.Member.Service.MemberService.Data.InternalData;

namespace ETS2.Member.Service.MemberService.Data
{
    public class MemberIssueData
    {
        public int InsertOrUpdate(Member_Issue issuemodel)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalMemberIssue(helper).InsertOrUpdate(issuemodel);
                return id;
            }
        }

        public Member_Issue GetIssueDetailById(int issueid)
        {
             using(var helper=new SqlHelper())
             {
                 var model = new InternalMemberIssue(helper).GetIssueDetailById(issueid);
                 return model;
             }
        }
        public List<Member_Issue> PageList(string comid, int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalMemberIssue(helper).PageList(comid, pageindex, pagesize,  out totalcount);

                return list;
            }
        }
         
    }
}
