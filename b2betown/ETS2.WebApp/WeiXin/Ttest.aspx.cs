using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using Newtonsoft.Json;
using ETS2.WebApp.wxpay;
using Com.Tenpay.TenpayAppV3;
using System.Text.RegularExpressions;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS.JsonFactory;
using ETS2.Common.Business;

namespace ETS2.WebApp.WeiXin
{
    public partial class Ttest : System.Web.UI.Page
    {

        // 获取微信用户openid总体思路:   a.aspx?id=33&name=y ，
        //判断网址是否符合伪静态网址形式a_wx_Mid.aspx，符合的话，则根据mid得到原始参数initparam，重写网址 a.aspx? code=d&initparam 进入a.aspx页面;

        //进入a.aspx页面，首先判断openid是否存在于cookie中，存在直接读取，
        //不存在，则判断code值是否存在，存在获取openid，并且保存到cookie中； 
        //不存在，把域名、网址、参数部分 保存到数据库 请求网址记录表ReqUrl_Log，返回标识列值Mid；
        //判断浏览器,如果为微信浏览器,则带着参数Mid 跳转到 中间页面,然后组建网页授权链接地址（注：授权后重定向的回调链接地址为 a_wx_Mid.aspx），自动跳转

        public int comid = 0;
        public string Openid = "";
        public string scopeurl = "";//网页授权地址


        public string d1 = "";//测试参数1
        public string d2 = "";//测试参数2
        public string currentTime = DateTime.Now.ToString("r");
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //GetOpenId(ref comid);
            //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "comid:" + comid);

            //d1 = Request["d1"].ConvertTo<string>("");
            //d2 = Request["d2"].ConvertTo<string>("");
            string partnerId = "2312";
            string userkey = "E60C5FA69F937CF81F1A67F738BF5508";
            string url = "http://test.wlski.net/wl.trip.order.get";
            var indexOf = url.IndexOf("/", 11, StringComparison.Ordinal);
            d1=url.Substring(indexOf).Replace("rhone-doc", "rhone").Replace("/","");

            String stringToSign = "POST" + " " + "wl.trip.order.get" + "\n" + currentTime;

        //    string qianming = SendSmsHelper.wanlongqianming(stringToSign, userkey);

        //    string responseData = SendSmsHelper.GetHttpPost(url, stringToSign, qianming);
        //    d1 = qianming;
        //    d2 = responseData;
        }

        


        private void GetOpenId(ref int comid)
        {
            //完整url、域名、参数
            string urlstr = Request.Url.ToString();
            string hoststr = HttpContext.Current.Request.Url.Host;
            string paramstr = HttpContext.Current.Request.Url.Query;

            //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "urlstr:" + urlstr + "\r\n----------------------------------------\r\n" + "hoststr:" + hoststr + "\r\n----------------------------------------\r\n" + "paramstr:" + paramstr);

            //根据域名获取comid
            comid = int.Parse(Regex.Match(hoststr, "\\d{1,}").ToString());
            if (comid == 0) { return; }



            //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "Cookiehelp.GetCookie(openid):" + Cookiehelp.GetCookie("openid"));

            //判断openid是否存在于cookie中
            if (Cookiehelp.GetCookie("openid") != "")
            {
                Openid = Cookiehelp.GetCookie("openid");
                //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "openid:" + Openid);
            }
            else
            {
                //判断code值是否存在，存在获取openid，并且保存到cookie中； 
                if (Request["code"].ConvertTo<string>("") != "")
                {
                    string code = Request["code"];
                    //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "code:" + code);

                    WeiXinBasic basic = new WeiXinBasicData().GetWxBasicByComId(comid);
                    if (basic == null)
                    {
                        return;
                    }

                    //必须为认证服务号才含有网页授权获取用户openid的权限
                    if (basic.Weixintype == 4)
                    {
                        string requrl =
                                       string.Format(
                                           "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code",
                                           basic.AppId, basic.AppSecret, code);
                        string returnStr = HttpUtil.Send("", requrl);
                        //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "returnStr:" + returnStr);

                        var obj = JsonConvert.DeserializeObject<ModelOpenID>(returnStr);
                        Openid = obj.openid;

                        //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "获取的openid:" + Openid);

                        //向cookie添加值
                        Cookiehelp.WriteCookie("openid", Openid, DateTime.Now.AddDays(30));
                    }

                }
                else //code值不存在，把域名、完整网址、参数部分 保存到数据库 请求网址记录表ReqUrl_Log，返回标识列值Mid；
                {
                    ReqUrl_Log requrllog = new ReqUrl_Log
                    {
                        mid = 0,
                        urlstr = urlstr,
                        hoststr = hoststr,
                        paramstr = paramstr,
                        subtime = DateTime.Now
                    };

                    int mid = new ReqUrl_LogHelper().InsReqUrlLog(requrllog);

                    WeiXinBasic basic = new WeiXinBasicData().GetWxBasicByComId(comid);
                    if (basic == null)
                    {
                        return;
                    }

                    //必须为认证服务号才含有网页授权获取用户openid的权限
                    if (basic.Weixintype == 4)
                    {
                        string redirect_url = urlstr.Replace(".aspx" + paramstr, "_wx_" + mid + ".aspx");
                        //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "redirect_url:" + redirect_url);

                        scopeurl = WeiXinJsonData.GetFollowOpenLinkUrl(comid, redirect_url);
                        //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "scopeurl:" + scopeurl);
                    }
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //测试阶段清除cookie,测试完成后删除
            Cookiehelp.DeleteCookie("openid");
        }
    }
}