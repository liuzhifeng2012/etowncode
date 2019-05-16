using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.Common.Business;
using ETS.Framework;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;

namespace ETS2.WebApp.WeiXin
{
    public partial class ShangJiaSet2 : System.Web.UI.Page
    {
        public string domain = "";//公司域名
        public string url = "";//公司url "http://shop"+comid+".etown.cn/weixin/index.aspx"
        public string token = "";//公司token 16个随机字母+数字
        protected void Page_Load(object sender, EventArgs e)
        {
            int comid = UserHelper.CurrentCompany.ID;
            //微信接口设置信息:没有设置过,直接把商家接收微信请求地址url和token直接录入数据库
            WeiXinBasic b = new WeiXinBasicData().GetWxBasicByComId(comid);
            if (b == null)
            {
                url = "http://shop" + comid + ".etown.cn/weixin/index.aspx";

                domain = "shop" + comid + ".etown.cn";

                token = RandomHelper.RandCode(20);

                int r = new WeiXinBasicData().Editwxbasicstep1(0, 0, comid, domain, url, token);
            }

        }
    }
}