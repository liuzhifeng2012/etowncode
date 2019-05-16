using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Data.InternalData;
using ETS2.WeiXin.Service.WeiXinService.Model;

namespace ETS2.WeiXin.Service.WeiXinService.Data
{
    public class WxMemberBasicData
    {
        public bool JudgeExists(string openid,out int wxmemberbasicid)
        {
            using (var helper = new SqlHelper())
            {
                var result = new InternalWxMemberBasic(helper).JudgeExists(openid,out wxmemberbasicid);
                return result;
            }
        }

        public int EditWxMemeberBasic(Model.WxMemberBasic memberbasic)
        {
             using (var helper=new SqlHelper())
             {
                 var result = new InternalWxMemberBasic(helper).EditWxMemberBasic(memberbasic);
                 return result;
             }
        }

        public IList<string> GetWxMemberCountry()
        { 
            using(var helper=new SqlHelper())
            {
                IList<string> result = new InternalWxMemberBasic(helper).GetWxmemberCountry();
                return result;
            }
        }

        public IList<string> GetWxMemberProvince(string country)
        {
            using (var helper = new SqlHelper())
            {
                IList<string> result = new InternalWxMemberBasic(helper).GetWxMemberProvince(country);
                return result;
            }
        }

        public IList<string> GetWxMemberCity(string province)
        {
            using (var helper = new SqlHelper())
            {
                IList<string> result = new InternalWxMemberBasic(helper).GetWxMemberCity(province);
                return result;
            }
        }

        internal WxMemberBasic Getwxmemberbasic(string fromusername)
        {
            using (var helper = new SqlHelper())
            {
                WxMemberBasic result = new InternalWxMemberBasic(helper).Getwxmemberbasic(fromusername);
                return result;
            }
        }
    }
}
