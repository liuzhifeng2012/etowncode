using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.Member.Service.MemberService.Model;
using ETS.Data.SqlHelper;
using ETS2.Member.Service.MemberService.Data.InternalData;

namespace ETS2.Member.Service.MemberService.Data
{
    public class WxSubscribeDetailData
    {
        public int EditSubscribeDetail(WxSubscribeDetail model)
        {
            using (var helper = new SqlHelper())
            {
                int data = new InternalWxSubscribeDetail(helper).EditSubscribeDetail(model);
                return data;
            }
        }

        public List<WxSubscribeDetail> GetWxSubscribeList(int comid, int subscribesourceid, int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                List<WxSubscribeDetail> data = new InternalWxSubscribeDetail(helper).GetWxSubscribeList(comid, subscribesourceid, pageindex, pagesize, out totalcount);
                return data;
            }
        }

        public WxSubscribeDetail GetWxSubscribeByOpenId(string openid, int comid)
        {
            using (var helper = new SqlHelper())
            {
                WxSubscribeDetail data = new InternalWxSubscribeDetail(helper).GetWxSubscribeByOpenId(openid, comid);
                return data;
            }
        }



        public int GetSubScribeTotalCountBySubScribeId(int subscribesourceid)
        {
            using (var helper = new SqlHelper())
            {
                int totalcount = new InternalWxSubscribeDetail(helper).GetSubScribeTotalCountBySourceidd(subscribesourceid);
                return totalcount;
            }
        }

        public int GetScanTotalCount(int subscribesourceid)
        {
            using (var helper = new SqlHelper())
            {
                int totalcount = new InternalWxSubscribeDetail(helper).GetScanTotalCount(subscribesourceid);
                return totalcount;
            }
        }

        public string GetLasterScanTime(int subscribesourceid)
        {
            using (var helper = new SqlHelper())
            {
                string scantime = new InternalWxSubscribeDetail(helper).GetLasterScanTime(subscribesourceid);
                return scantime;
            }
        }
        /// <summary>
        /// 获得当前 公司/活动 下所有二维码关注微信总数
        /// </summary>
        /// <param name="wxsourceid"></param>
        /// <returns></returns>
        public int GetWxTotal2(string id, string isqudao,string issuetype="0,1")
        {
            using (var helper = new SqlHelper())
            {
                int totalcount = new InternalWxSubscribeDetail(helper).GetWxTotal2(id,isqudao,issuetype);
                return totalcount;
            }
        }
        /// <summary>
        /// 获得当前 公司/活动 下所有二维码扫码总数
        /// </summary>
        /// <param name="wxsourceid"></param>
        /// <returns></returns>
        public int GetScanTotal2(string id, string isqudao)
        {
            using (var helper = new SqlHelper())
            {
                int totalcount = new InternalWxSubscribeDetail(helper).GetScanTotal2(id, isqudao);
                return totalcount;
            }
        }
        /// <summary>
        /// 获得当前 公司/活动 下所有二维码取消关注微信总数
        /// </summary>
        /// <param name="wxsourceid"></param>
        /// <returns></returns>
        public int GetQxWxTotal2(string id, string isqudao)
        {
            using (var helper = new SqlHelper())
            {
                int totalcount = new InternalWxSubscribeDetail(helper).GetQxWxTotal2(id, isqudao);
                return totalcount;
            }
        }


        public int GetQxWxTotal(int wxsourceid)
        {
            using (var helper = new SqlHelper())
            {
                int totalcount = new InternalWxSubscribeDetail(helper).GetQxWxTotal(wxsourceid);
                return totalcount;
            }
        }

        public int GetMd_Subscribenum(int comid)
        {
            using (var helper = new SqlHelper())
            {
                int totalcount = new InternalWxSubscribeDetail(helper).GetMd_Subscribenum(comid);
                return totalcount;
            }
        }
        /// <summary>
        /// 获得请求发送的次数
        /// </summary>
        /// <param name="p"></param>
        /// <param name="cretime"></param>
        /// <param name="p_2"></param>
        public int GetReqnum(string weixin, string cretime, string eventname)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalWxSubscribeDetail(helper).GetReqnum(weixin,cretime,eventname);
                return r;
            }
        }
    }
}
