using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Framework;
using ETS.JsonFactory;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Member.Service.MemberService.Data;

namespace ETS2.WebApp.JsonFactory
{
    /// <summary>
    /// Choujiang 的摘要说明
    /// </summary>
    public class Choujiang : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string oper = context.Request["oper"].ConvertTo<string>("");

            if (oper != "")
            {

                if (oper == "dzp_choujiang")
                {
                    
                    var Id = context.Request["Id"].ConvertTo<int>(0);
                    var openid = context.Request["openid"].ConvertTo<string>("");
                    var ygj = context.Request["ygj"].ConvertTo<string>("");

                    ERNIE_Record Recorduser = new ERNIE_Record()
                    {
                        ERNIE_id = Id,
                        ERNIE_openid = openid,
                    };
                    var data = PromotionJsonDate.ERNIEChoujiang(Recorduser);
                    context.Response.Write(data);

                }

                if (oper == "huodong_tel")
                {

                    var Id = context.Request["Id"].ConvertTo<int>(0);
                    var openid = context.Request["openid"].ConvertTo<string>("");
                    var ygj = context.Request["ygj"].ConvertTo<string>("");
                    var udid = context.Request["udid"].ConvertTo<int>(0);
                    var hdtel = context.Request["hdtel"].ConvertTo<string>("");
                    var hduname = context.Request["hduname"].ConvertTo<string>("");



                    ERNIE_Record Recorduser = new ERNIE_Record()
                    {
                        ERNIE_id = Id,
                        ERNIE_openid = openid,
                        Name = hduname,
                        Phone = hdtel,
                        Id = udid,

                    };
                    var data = PromotionJsonDate.ERNIEZhongjiang(Recorduser);
                    context.Response.Write(data);

                }

                if (oper == "huojiang")
                {

                    var Id = context.Request["Id"].ConvertTo<int>(0);
                    var openid = context.Request["openid"].ConvertTo<string>("");

                    var data = PromotionJsonDate.Huojiangmingdan(Id,openid);
                    context.Response.Write(data);

                }


            }



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