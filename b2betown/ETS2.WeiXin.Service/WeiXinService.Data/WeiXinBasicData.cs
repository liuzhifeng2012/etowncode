using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data.InternalData;
using ETS.Data.SqlHelper;

namespace ETS2.WeiXin.Service.WeiXinService.Data
{
    public class WeiXinBasicData
    {
        public WeiXinBasic GetWxBasicByComId(int comid)
        {
            
              

                using (var helper = new SqlHelper())
                {
                    WeiXinBasic basic = new InternalWeiXinBasic(helper).GetWxBasicByComId(comid);

                    return basic;
                }
        }

        public int Editwxbasic(WeiXinBasic basic)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalWeiXinBasic(helper).Editwxbasic(basic);
                return id;
            }
        }

        public WeiXinBasic GetWeiXinBasicByDomain(string RequestDomin)
        {
            using (var helper = new SqlHelper())
            {
                WeiXinBasic basic = new InternalWeiXinBasic(helper).GetWeiXinBasicByDomain(RequestDomin);

                return basic;
            }
        }




        internal int ModifyWeiXinID(int id, string weixinoriginalid)
        {

            using (var helper = new SqlHelper())
            {
                int result = new InternalWeiXinBasic(helper).ModifyWeiXinID(id,weixinoriginalid);

                return result;
            }
        }

        public int Editwxbasicstep1(int wxbasicid, int weixintype, int comid, string domain, string url, string token)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalWeiXinBasic(helper).Editwxbasicstep1(wxbasicid, weixintype, comid, domain, url, token);

                return result;
            }
        }

        public int Editwxbasicstep2(int wxbasicid, string appid, string appsecret)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalWeiXinBasic(helper).Editwxbasicstep2(wxbasicid, appid,appsecret);

                return result;
            }
        }

        public int Editwxbasicreply(int wxbasicid, string leavemsgreply, string attentionreply)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalWeiXinBasic(helper).Editwxbasicreply(wxbasicid, leavemsgreply,attentionreply);

                return result;
            }
        }

        public int Editwxbasicstep(int wxbasicid, string appid, string appsecret, int weixintype)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalWeiXinBasic(helper).Editwxbasicstep(wxbasicid, appid, appsecret,weixintype);

                return result;
            }
        }
    }
}
