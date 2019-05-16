using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS2.Common.Business;
using ETS.Framework;
using ETS.JsonFactory;
using ETS2.PM.Service.PMService.Data;

namespace ETS2.WebApp.JsonFactory
{
    /// <summary>
    /// SmsHandler 的摘要说明
    /// </summary>
    public class SmsHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
             context.Response.ContentType = "text/plain";
             string oper = context.Request["oper"].ConvertTo<string>("");
             string phone = context.Request["phone"].ConvertTo<string>("");
             string smscontent = context.Request["smscontent"].ConvertTo<string>("");

             string dxstr = context.Request["dxstr"].ConvertTo<string>("");
             //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\BaoxianLog.txt", dxstr);
             string msg = "";
             if (oper == "234908uasdlkfjasdfou234kldfuasfk234u809dsfjasdfu90")
             {
                 var snsback = SendSmsHelper.SendSms(phone, smscontent, 106, out msg);

                 context.Response.Write(snsback);
             }

             //接收短信上行数据 http://ip:port/MO?dxstr=2,13800138000,0,F165245140224152846,20140428172543;2,15193792747,0,F165246140224152846,20140428172543; 
             if (dxstr != "") {

                 //var OrderData1 = new B2bOrderData();
                 //var insertstepm = OrderData1.InsertSmsback(0, "", "", dxstr, "");


                 var dxstr_arr = dxstr.Split(';');
                 for (int i = 0; i < dxstr_arr.Length; i++) {//分解每条消息 
                     if(dxstr_arr[i] !=""){//判断消息是否为空
                         var duanxin_arr = dxstr_arr[i].Split(',');
                         if (duanxin_arr.Length == 5) { //必须含有5个参数，下面读取对应的值，如果不匹配，暂时 不操作
                             var duanxin_type = int.Parse(duanxin_arr[0]);
                             var duanxin_mobile = duanxin_arr[1];
                             var duanxin_state = duanxin_arr[2];
                             var duanxin_con = duanxin_arr[3];
                             var duanxin_time = duanxin_arr[4];


                             if (duanxin_type == 0) { //上行消息
                                 
                                 //上行消息写入记录
                                 var OrderData=new B2bOrderData();
                                 var insertsms = OrderData.InsertSmsback(duanxin_type,duanxin_mobile, duanxin_state, duanxin_con, duanxin_time);


                                 //对上行消息判定
                                 if (duanxin_con != "") {
                                     if (duanxin_con.ToLower().Substring(0, 2) == "qr" || duanxin_con.ToLower().Substring(0, 2) == "qx" || duanxin_con.ToLower().Substring(0, 2) == "tj")
                                     { 
                                     //截取前两个字qr，进入确认流程
                                         int orderid = int.Parse(duanxin_con.ToLower().Substring(2, duanxin_con.Length-2));

                                         var snsback = OrderJsonData.UporderPaystate(orderid, duanxin_con.ToLower().Substring(0, 2), duanxin_mobile);

                                         context.Response.Write(snsback);
                                    
                                     }

                                     
                                 }


                                
                             }

                             if (duanxin_type == 2)//状态报告，未做
                             {


                             }
                         
                         }
                     
                     }
                 
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