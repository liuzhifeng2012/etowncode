using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.Common.Business;

namespace ETS.JsonFactory
{
    public static class GeneralFunc
    {
        public static int GetComid(string RequestUrl)
        {
            int comid = 0;

            if (RequestUrl == "agent.maikexing.com")
            {
                comid = 1305; 
            }

            if(comid==0){
            if (Domain_def.Domain_yanzheng(RequestUrl))//如果符合shop101.etown.cn的格式，则从多微信商户基本信息表中获取comid
            {
                comid = Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl).ToString());
               
            }
            }
            if (comid == 0)
            {
                //根据域名读取商户ID
                B2b_company_info companyinfo = B2bCompanyData.GetComId(RequestUrl);
                if (companyinfo != null)
                {
                    comid = companyinfo.Com_id;

                }
            }
            if (comid == 0)
            {
                //根据绑定的管理后台域名读取商户ID 
                B2b_company_info companyinfo1 = B2bCompanyData.GetComIdByAdmindomain(RequestUrl);
                if (companyinfo1 != null)
                {
                    comid = companyinfo1.Com_id;

                }
            }
            return comid;
        }
    }
}
