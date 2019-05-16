using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Framework;
using System.Web.SessionState;

namespace ETS2.WebApp.Account
{
    /// <summary>
    /// 获取验证码（图片）
    /// </summary>
    public class GetValidateCode : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 获取验证码（一般处理程序入口函数）
        /// </summary>
        /// <param name="context">当前上下文</param>
        public void ProcessRequest(HttpContext context)
        {
            // 创建验证码
            ValidateCode validateCode = new ValidateCode();

            // 获取验证码（字符串），写入session
            context.Session["SomeValidateCode"] = validateCode.GetString();

            
            // 输出验证码（图片）
            context.Response.BinaryWrite(validateCode.GetByteArray());

        }

        /// <summary>
        /// 输出验证码图片
        /// </summary>
        /// <param name="context"></param>
        private void OutputValidateCodeImg(HttpContext context)
        {
            // 创建验证码
            //ValidateCode validateCode = new ValidateCode(new ValidateCode.ValidateCodeConfiger()
            //{
            //    CharKind = ValidateCode.En_CharKind.Lower | ValidateCode.En_CharKind.Number | ValidateCode.En_CharKind.Upper,
            //    CharNum = 8,
            //    CharSpacing = 8,
            //    FontSize = 24,
            //    Width = 480,
            //    Height = 48,
            //    Noise = new ValidateCode.NoiseLine()
            //    {
            //        BorderColor = Color.Silver,
            //        BorderWidth = 1,
            //        Count = 20
            //    },
            //    BorderColor = Color.Blue,
            //    BackgroundColor = Color.White
            //});
            ValidateCode validateCode = new ValidateCode();

            //validateCode.Reload();

            // 获取验证码（字符串）
            string sessionName = "SomeValidateCode"; // 和Demo.aspx中对应
            context.Session[sessionName] = validateCode.GetString();

            // 输出验证码（图片）
            context.Response.BinaryWrite(validateCode.GetByteArray());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}