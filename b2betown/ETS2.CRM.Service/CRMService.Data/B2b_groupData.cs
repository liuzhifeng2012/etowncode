using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Data.InternalData;

namespace ETS2.CRM.Service.CRMService.Data
{
    public class B2b_groupData
    {
        public IList<B2b_group> GetCrmGroupList(out int total, int comid, int pageindex, int pagesize)
        {
            using (var helper = new SqlHelper())
            {
                IList<B2b_group> result = new InternalB2b_group(helper).GetCrmGroupList(out total, comid, pageindex, pagesize);
                return result;
            }
        }

        public B2b_group GetB2bgroup(int groupid)
        {
            using (var helper = new SqlHelper())
            {
                B2b_group result = new InternalB2b_group(helper).GetB2bgroup(groupid);
                return result;
            }
        }

        public int EditB2bGroup(int id, string name, int comid, int userid, string remark, DateTime createtime)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalB2b_group(helper).EditB2bGroup(id, name, comid, userid, remark, createtime);
                return result;
            }
        }

        public int Delb2bgroup(int groupid)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalB2b_group(helper).Delb2bgroup(groupid);
                return result;
            }
        }

        public IList<B2b_group> GetCompanyB2bgroup(out int total, int comid)
        {
            using (var helper = new SqlHelper())
            {
                IList<B2b_group> result = new InternalB2b_group(helper).GetCompanyB2bgroup(out total, comid);
                return result;
            }
        }

        public int Changefenzu(int crmid, int groupid)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalB2b_group(helper).Changefenzu(crmid, groupid);
                return result;
            }
        }

        public string GetB2bgroupName(int groupid)
        {
            using (var helper = new SqlHelper())
            {
                B2b_group result = new InternalB2b_group(helper).GetB2bgroup(groupid);
                if (result == null)
                {
                    return "默认组";
                }
                else
                {
                    return result.Groupname;
                }
            }


        }
    }
}
