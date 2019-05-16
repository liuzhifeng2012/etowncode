using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.Member.Service.MemberService.Model;
using ETS.Data.SqlHelper;
using ETS2.Member.Service.MemberService.Data.InternalData;

namespace ETS2.Member.Service.MemberService.Data
{
    public class WxSubscribeSourceData
    {
        public List<WxSubscribeSource> GetWXSourcelist(int comid, int wxsourcetype, int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalWxSubscribeSource(helper).GetWXSourcelist(comid, wxsourcetype, pageindex, pagesize, out totalcount);

                return list;
            }
        }

        public List<WxSubscribeSource> SeledWXSourcelist(int comid, int wxsourcetype, int pageindex, int pagesize, out int totalcount, string onlinestatus = "1")
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalWxSubscribeSource(helper).SeledWXSourcelist(comid, wxsourcetype, pageindex, pagesize, out totalcount, onlinestatus);

                return list;
            }
        }

        public int EditSubscribeSource(WxSubscribeSource source)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalWxSubscribeSource(helper).EditSubscribeSource(source);

                return list;
            }
        }
        /// <summary>
        /// 判断二维码是否已经生成过
        /// </summary>
        /// <param name="comid"></param>
        /// <param name="sourcetype"></param>
        /// <param name="promoteact"></param>
        /// <param name="promotechannelcompany"></param>
        /// <returns></returns>
        public int WhetherCreated(int comid, int sourcetype, int promoteact, int promotechannelcompany)
        {
            using (var helper = new SqlHelper())
            {
                var count = new InternalWxSubscribeSource(helper).WheherCreated(comid, sourcetype, promoteact, promotechannelcompany);
                return count;
            }
        }

        public int WhetherSameExplain(string qrcodename, int comid)
        {
            using (var helper = new SqlHelper())
            {
                var count = new InternalWxSubscribeSource(helper).WhetherSameExplain(qrcodename, comid);
                return count;
            }
        }

        public WxSubscribeSource Getwxqrcode(int qrcodeid)
        {
            using (var helper = new SqlHelper())
            {
                var count = new InternalWxSubscribeSource(helper).Getwxqrcode(qrcodeid);
                return count;
            }
        }

        public List<WxSubscribeSource> GetWXSourcelist2(int comid, string wxsourcetype, int pageindex, int pagesize, string onlinestatus, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalWxSubscribeSource(helper).GetWXSourcelist2(comid, wxsourcetype, pageindex, pagesize, onlinestatus, out totalcount);

                return list;
            }
        }

        public WxSubscribeDetail GetWXSourceByOpenId(string openid)
        {
            using (var helper = new SqlHelper())
            {
                var count = new InternalWxSubscribeSource(helper).GetWXSourceByOpenId(openid);
                return count;
            }
        }
        /// <summary>
        /// 判断是否已经创建过渠道人的带参二维码
        /// </summary>
        /// <param name="channelid"></param>
        /// <returns></returns>
        public bool Ishascreate_paramqrcode(int channelid)
        {
            using (var helper = new SqlHelper())
            {
                var count = new InternalWxSubscribeSource(helper).Ishascreate_paramqrcode(channelid);
                return count;
            }

        }

        public WxSubscribeSource GetWXSourceById(int subscribesourceid)
        {
            using (var helper = new SqlHelper())
            {
                var m = new InternalWxSubscribeSource(helper).GetWXSourceById(subscribesourceid);

                return m;
            }
        }
        public WxSubscribeSource GetWXSourceByChannelcompanyid(int channelcompanyid)
        {
            using (var helper = new SqlHelper())
            {
                var m = new InternalWxSubscribeSource(helper).GetWXSourceByChannelcompanyid(channelcompanyid);

                return m;
            }
        }

        

        public WxSubscribeSource Getchannelwxqrcodebychannelid(int channelid)
        {
            using (var helper = new SqlHelper())
            {
                WxSubscribeSource m = new InternalWxSubscribeSource(helper).Getchannelwxqrcodebychannelid(channelid);

                return m;
            }
        }

        public WxSubscribeSource GetReserveproVerifywxqrcode(int comid, int qrcodeviewtype)
        {
            using (var helper = new SqlHelper())
            {
                WxSubscribeSource m = new InternalWxSubscribeSource(helper).GetReserveproVerifywxqrcode(comid,qrcodeviewtype);

                return m;
            }
        }
    }
}
