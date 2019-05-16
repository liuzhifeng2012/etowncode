using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Framework;
using ETS.JsonFactory;
using System.Collections.Specialized;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.WebApp.JsonFactory
{
    /// <summary>
    /// TwoCode 的摘要说明
    /// </summary>
    public class TwoCode : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string oper = context.Request["oper"].ConvertTo<string>("");

            SortedDictionary<string, string> para = CommonFunc.GetRequestPost();  //post传递过来的参数
            var postparastr = "";//传递过来的参数字符串             
            if (para.Count == 0)
            {
                para = CommonFunc.GetRequestGet();  //get传递过来的参数

            }

            if (para.Count > 0)
            {
                postparastr = CommonFunc.CreateLinkString(para);
            }
            //把对方传入的参数插入数据库
            PoslogData poslogdata = new PoslogData();
            Pos_log poslog = new Pos_log
            {
                Id = 0,
                Str = postparastr,
                Subdate = DateTime.Now,
                Uip = CommonFunc.GetRealIP(),
                ReturnStr = "",
                ReturnSubdate = DateTime.Now
            };
            int poslogid = poslogdata.InsertOrUpdate(poslog);


            if (oper != "")
            {
                if (para.Count > 0)
                {
                    string data = TwoCodeJsonData.GetReturnData(oper.Trim(), poslogid);

                    context.Response.Write(data);

                }
                else
                {
                    string backstr = TwoCodeJsonData.ParamErr("接收参数出错");
                    context.Response.Write(TwoCodeJsonData.GetBackStr(backstr, poslogid));
                }
            }
            else
            {
                string backstr = TwoCodeJsonData.ParamErr("未传递参数oper");
                context.Response.Write(TwoCodeJsonData.GetBackStr(backstr, poslogid));
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