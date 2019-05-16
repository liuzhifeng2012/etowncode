using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Data.InternalData;
using ETS2.WeiXin.Service.WeiXinService.Model;

namespace ETS2.WeiXin.Service.WeiXinService.Data
{
    public class WxkfData
    {
        public int Editwxkf(int kf_id, string kf_account, string kf_nick, int comid, int createuserid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalWxkf(helper).Editwxkf(kf_id, kf_account, kf_nick, comid, createuserid);
                return r;
            }
        }

        public IList<Wxkf> Getwxkfpagelist(int pageindex, int pagesize, int comid, out int totalcount, int userid = 0, string isrun = "0,1", string key = "")
        {
            using (var helper = new SqlHelper())
            {
                List<Wxkf> list = new InternalWxkf(helper).Getwxkfpagelist(pageindex, pagesize, comid, out totalcount, userid, isrun, key);
                return list;
            }
        }

        public int DelWxkf(int kfid, int comid)
        {
            using (var helper = new SqlHelper())
            {
                int d = new InternalWxkf(helper).DelWxkf(kfid, comid);
                return d;
            }
        }

        public Wxkf Getwxkf(int kfid, int comid)
        {
            using (var helper = new SqlHelper())
            {
                Wxkf d = new InternalWxkf(helper).Getwxkf(kfid, comid);
                return d;
            }
        }

        public int BindWxdkf(Wxkf m)
        {
            using (var helper = new SqlHelper())
            {
                int d = new InternalWxkf(helper).BindWxdkf(m);
                return d;
            }
        }

        internal int UpWxkf_downline(int comid)
        {
            using (var helper = new SqlHelper())
            {
                int d = new InternalWxkf(helper).UpWxkf_downline(comid);
                return d;
            }
        }

        internal int Upwxkf_online(int kf_id, int Comid, int status, int auto_accept, int accepted_case)
        {
            using (var helper = new SqlHelper())
            {
                int d = new InternalWxkf(helper).Upwxkf_online(kf_id, Comid, status, auto_accept, accepted_case);
                return d;
            }
        }
        /// <summary>
        /// 获取公司客服(全部)列表
        /// </summary>
        /// <param name="comid"></param>
        /// <param name="isonline"></param>
        /// <param name="isrun"></param>
        /// <returns></returns>
        internal List<Wxkf> Getwxkflist(int comid, string isonline, string isrun)
        {
            using (var helper = new SqlHelper())
            {
                List<Wxkf> list = new InternalWxkf(helper).Getwxkflist(comid, isonline, isrun);
                return list;
            }
        }


        /// <summary>
        /// 获取公司客服(不包括门市客服)列表 
        /// </summary>
        /// <param name="comid"></param>
        /// <param name="isonline"></param>
        /// <param name="isrun"></param>
        /// <returns></returns>
        internal List<Wxkf> GetGs_wxkflist(int comid, string isonline, string isrun)
        {
            using (var helper = new SqlHelper())
            {
                List<Wxkf> list = new InternalWxkf(helper).GetGs_wxkflist(comid, isonline, isrun);
                return list;
            }

        }
        /// <summary>
        /// 获取渠道人对应的在线客服
        /// </summary>
        /// <param name="comid"></param>
        /// <param name="openid"></param>
        /// <param name="isonline"></param>
        /// <param name="isrun"></param>
        /// <returns></returns>
        internal Wxkf GetChannel_Wxkf(int channelid, int comid,  string isonline, string isrun)
        {
            using (var helper = new SqlHelper())
            {
                Wxkf list = new InternalWxkf(helper).GetChannel_Wxkf(channelid, comid, isonline, isrun);
                return list;
            }
        }
        /// <summary>
        /// 获得门市的在线客服列表
        /// </summary>
        /// <param name="msid"></param>
        /// <param name="isonline"></param>
        /// <param name="isrun"></param>
        /// <returns></returns>
        internal List<Wxkf> GetMs_wxkflist(int msid, string isonline,string isrun)
        {
            using (var helper = new SqlHelper())
            {
                List<Wxkf> list = new InternalWxkf(helper).GetMs_wxkflist(msid, isonline, isrun);
                return list;
            }
        }
        /// <summary>
        /// 查询 公司/门市 的总客服
        /// </summary>
        /// <param name="comid"></param>
        /// <param name="msid"></param>
        /// <returns></returns>
        public Wxkf Getwxzkf(int comid, int msid)
        {
            using (var helper = new SqlHelper())
            {
                Wxkf li = new InternalWxkf(helper).Getwxzkf(comid, msid);
                return li;
            }
        }
        /// <summary>
        /// 根据员工id获取客服信息
        /// </summary>
        /// <param name="ygid"></param>
        /// <returns></returns>
        public Wxkf Getwxkfbyygid(int ygid)
        {
            using (var helper = new SqlHelper())
            {
                Wxkf li = new InternalWxkf(helper).Getwxkfbyygid(ygid);
                return li;
            }
        }
    }
}
