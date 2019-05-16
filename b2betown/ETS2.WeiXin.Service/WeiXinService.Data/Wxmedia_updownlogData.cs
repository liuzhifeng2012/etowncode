using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Data.InternalData;
using ETS2.WeiXin.Service.WeiXinService.Model;

namespace ETS2.WeiXin.Service.WeiXinService.Data
{
    public class Wxmedia_updownlogData
    {
        /// <summary>
        /// 判断是否含有已经标记过"下载用户上传的问候语音" 但是没有完成操作 的记录
        /// </summary>
        /// <param name="weixin"></param>
        /// <param name="clientuptypemark"></param>
        /// <returns></returns>
        public bool Ismarkedandnotdeal(string weixin, int clientuptypemark)
        {
             using(var helper=new SqlHelper())
             {
                 bool r = new InternalWxmedia_updownlog(helper).Ismarkedandnotdeal(weixin, clientuptypemark);
                 return r;
             }
        }

        public int Edituploadlog(Wxmedia_updownlog udlog)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalWxmedia_updownlog(helper).Edituploadlog(udlog);
                return r;
            }
        }
        /// <summary>
        /// 得到含有已经标记过"下载上传的问候语音" 但是没有完成操作 的记录
        /// </summary>
        /// <param name="weixin"></param>
        /// <param name="clientuptypemark"></param>
        /// <returns></returns>
        public Wxmedia_updownlog GetMarkedAndNotdeallog(string weixin, int clientuptypemark)
        {
            using (var helper = new SqlHelper())
            {
                Wxmedia_updownlog r = new InternalWxmedia_updownlog(helper).GetMarkedAndNotdeallog(weixin, clientuptypemark);
                return r;
            }
        }
        /// <summary>
        /// 根据用户微信得到其顾问微信，然后根据微信和标记得到最新的一条保存路径(注：已经上传过语音的即mediaid!="")
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        public  Wxmedia_updownlog GetWxmedia_updownlog(string weixin, int clientuptypemark,int comid)
        {
            using (var helper = new SqlHelper())
            {
                Wxmedia_updownlog r = new InternalWxmedia_updownlog(helper).GetWxmedia_updownlog(weixin, clientuptypemark,comid);
                return r;
            }
        }
        /// <summary>
        /// 删除顾问的没有操作成功的标记
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        public  int DelGuwenNotSucMediaMark(string weixin )
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalWxmedia_updownlog(helper).DelGuwenNotSucMediaMark(weixin );
                return r;
            }
        }
        /// <summary>
        /// 根据微信号得到顾问上传多媒体信息记录
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public List<Wxmedia_updownlog> Getwxmedia_updownlogByopenid(int pageindex,int pagesize,out int totalcount,string openid,string viewmethod="")
        {
            using (var helper = new SqlHelper())
            {
                List<Wxmedia_updownlog> r = new InternalWxmedia_updownlog(helper).Getwxmedia_updownlogByopenid(pageindex,pagesize,out totalcount, openid,viewmethod);
                return r;
            }
        }

        public IList<Wxmedia_updownlog> Getwxdownvoicelist(string openid, int clientuptypemark,int materialid)
        {
            using (var helper = new SqlHelper())
            {
                List<Wxmedia_updownlog> r = new InternalWxmedia_updownlog(helper).Getwxdownvoicelist(openid, clientuptypemark, materialid);
                return r;
            }
        }

        public Wxmedia_updownlog GetWxmedia_updownlogbyid(int uplogid)
        {
            using (var helper = new SqlHelper())
            {
                Wxmedia_updownlog r = new InternalWxmedia_updownlog(helper).GetWxmedia_updownlogbyid(uplogid);
                return r;
            }
        }

        public Wxmedia_updownlog GetNewestuplogbymedia(string savepath)
        {
            using (var helper = new SqlHelper())
            {
                Wxmedia_updownlog r = new InternalWxmedia_updownlog(helper).GetNewestuplogbymedia(savepath);
                return r;
            }
        }

        public Wxmedia_updownlog GetWxmedia_updownlogbymaterialid(int materialid)
        {
            using (var helper = new SqlHelper())
            {
                Wxmedia_updownlog r = new InternalWxmedia_updownlog(helper).GetWxmedia_updownlogbymaterialid(materialid);
                return r;
            }
        }
        /// <summary>
        /// 删除覆盖前的语音的上传记录
        /// </summary>
        /// <param name="materialid"></param>
        /// <returns></returns>
        public int DelBeforeCoverUplog(int materialid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalWxmedia_updownlog(helper).DelBeforeCoverUplog(materialid);
                return r;
            }
        }
    }
}
