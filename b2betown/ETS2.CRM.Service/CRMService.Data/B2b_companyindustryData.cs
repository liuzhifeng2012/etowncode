using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Data.InternalData;

namespace ETS2.CRM.Service.CRMService.Data
{
    public class B2b_companyindustryData
    {
        public IList<B2b_companyindustry> GetIndustryList(out int total, int industryid = 0)
        {
            using (var helper = new SqlHelper())
            {

                var crmid = new InternalB2b_companyindustry(helper).GetIndustryList(out total, industryid);

                return crmid;
            }
        }

        public B2b_companyindustry GetIndustryById(int id)
        {
            using (var helper = new SqlHelper())
            {

                var crmid = new InternalB2b_companyindustry(helper).GetIndustryById(id);

                return crmid;
            }
        }

        public int EditComIndustry(int id, string name, string remark)
        {
            using (var helper = new SqlHelper())
            {

                var crmid = new InternalB2b_companyindustry(helper).EditComIndustry(id, name, remark);

                return crmid;
            }
        }

        public int Delindustry(int id)
        {
            using (var helper = new SqlHelper())
            {

                var crmid = new InternalB2b_companyindustry(helper).Delindustry(id);

                return crmid;
            }

        }



        public B2b_companyindustry GetIndustryByComid(int comid)
        {
            using (var helper = new SqlHelper())
            {

                B2b_companyindustry c = new InternalB2b_companyindustry(helper).GetIndustryByComid(comid);

                return c;
            }

        }

        public string GetIndustryNameById(int id)
        {
            using (var helper = new SqlHelper())
            {

                var crmid = new InternalB2b_companyindustry(helper).GetIndustryById(id);
                if (crmid == null)
                {
                    return "";
                }
                else
                {
                    return crmid.Industryname;
                }

            }
        }
    }
}
