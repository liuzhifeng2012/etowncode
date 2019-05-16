using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.JsonFactory;
using ETS.Framework;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using System.Text.RegularExpressions;
using ETS2.PM.Service.PMService.Modle;
using ETS2.Member.Service.MemberService.Data;

namespace ETS2.WebApp.WeiXin.invitecode
{
    public partial class duanxin_send : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string phone = Request["etmobile"].ConvertTo<string>("");

            string duanxintext = Request["duanxintext1"].ConvertTo<string>("");



            int comid = Request["comid"].ConvertTo<int>(0);

            int userid = Request["userid"].ConvertTo<int>(0);
            string isqunfa = Request["isqunfa"].ConvertTo<string>("no");
            if (isqunfa == "yes")
            {
                isqunfa = "1";
            }
            else
            {
                isqunfa = "0";
            }
            string msg = "";
            int sendstate = 0;//发送结果默认为0

            string Invitecode = "";//验证码

            //获取公司微信号
            B2b_company com = B2bCompanyData.GetAllComMsg(comid);
            if (com.B2bcompanyinfo.Weixinname == "")
            {
                msg = phone + "公司微信号为空";
            }
            else 
            {
                duanxintext = duanxintext.Replace("$comweixin$", com.B2bcompanyinfo.Weixinname);

                //判断手机格式是否正确
                bool isphone = Regex.IsMatch(phone, @"^1[358]\d{9}$", RegexOptions.IgnoreCase);
                if (isphone)
                {
                    //获得随机码
                    Invitecode = MemberCardData.GetRandomCode().ToString();
                    //使用随机码时，标记为已使用 防止重复码
                    ExcelSqlHelper.ExecuteNonQuery("update RandomCode set state = 1 where code = " + Invitecode);

                    duanxintext = duanxintext.Replace("$invitecode$", Invitecode);
                    sendstate = SendSmsHelper.SendSms(phone, duanxintext, comid, out msg);

                }
                else
                {
                    msg = phone + "手机格式不正确";
                }
            }

           



            //录入发送邀请码日志
            B2b_invitecodesendlog log = new B2b_invitecodesendlog
            {
                Id = 0,
                Phone = phone,
                Smscontent = duanxintext,
                Invitecode = Invitecode,
                Senduserid = userid,
                Sendtime = DateTime.Now,
                Issendsuc = 0,
                Isqunfa = int.Parse(isqunfa),
                Remark = msg,
                Comid = comid
            };
            if (sendstate > 0)
            {
                log.Issendsuc = 1;
            }
            else
            {
                log.Issendsuc = 0;
            }

            int result = new B2b_invitecodesendlogData().Inslog(log);


        }
    }
}