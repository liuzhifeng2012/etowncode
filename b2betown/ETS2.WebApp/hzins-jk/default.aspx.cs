using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using System.Text;
using ETS2.VAS.Service.VASService.Data.Common;
using Newtonsoft.Json;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Modle.Enum;

namespace ETS2.WebApp.hzins_jk
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var inputStream = Request.InputStream;

            var strLen = Convert.ToInt32(inputStream.Length);

            var strArr = new byte[strLen];

            inputStream.Read(strArr, 0, strLen);

            var requestMes = Encoding.UTF8.GetString(strArr);

            //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\BaoxianLog.txt", requestMes);
            //录入和慧择网交互日志
            ApiLog mapilog = new ApiLog
            {
                Id = 0,
                request_type = "Hzins_AsyncNotice",
                Serviceid = 2,
                Str = requestMes.Trim(),
                Subdate = DateTime.Now,
                ReturnStr = "success",
                ReturnSubdate = DateTime.Now,
                Errmsg = "",
            };
            int ins = new ApiLogData().EditLog(mapilog);



            Hzins_AsyncOrderInfo mresp = (Hzins_AsyncOrderInfo)JsonConvert.DeserializeObject(requestMes, typeof(Hzins_AsyncOrderInfo));
            if (mresp != null)
            {
                string insureNum = mresp.insureNum;
                int orderid = new Api_hzins_OrderApplyResp_OrderExtData().GetorderidbyinsureNum(mresp.insureNum);
                if (orderid == 0)
                {
                    TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\BaoxianLog.txt", "orderid=0");
                    return;
                }
                B2b_order modelb2border = new B2bOrderData().GetOrderById(orderid);
                if (modelb2border == null)
                {
                    TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\BaoxianLog.txt", "modelb2border==null");
                    return;
                }

                if (mresp.resultCode == 0)
                {
                    if (mresp.policyList != null)
                    {
                        List<AsyncOrderInfo_policyList> policyList = mresp.policyList;
                        foreach (AsyncOrderInfo_policyList policy in policyList)
                        {
                            int issueState = policy.issueState;
                            string cName = policy.insurant;

                            Api_hzins_OrderApplyResp_OrderInfo m1 = new Api_hzins_OrderApplyResp_OrderInfo
                            {
                                id = 0,
                                orderid = modelb2border.Id,
                                insureNum = insureNum,
                                policyNum = "",
                                cName = cName,
                                cardCode = "",
                                issueState = issueState
                            };
                            int ins1 = new Api_hzins_OrderApplyResp_OrderInfoData().EditOrderApplyResp_OrderInfo(m1);

                        }

                        modelb2border.Order_state = (int)OrderStatus.HasSendCode;
                        modelb2border.Order_remark = "异步出单成功";
                        new B2bOrderData().InsertOrUpdate(modelb2border);

                        try
                        {
                            //如果保险订单(b单)是原始订单(a单)的绑定订单，则修改原始订单(a单)的状态
                            int aorderid = new B2bOrderData().Getinitorderid(orderid);
                            if (aorderid > 0)
                            {
                                new B2bOrderData().Uporderstate(aorderid, (int)OrderStatus.HasSendCode);
                            }
                        }
                        catch (Exception ex)
                        {
                            TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\BaoxianLog.txt", "aorderid error");
                            return;
                        }
                    }
                }
                else
                {
                    modelb2border.Order_remark = "异步出单通知错误(" + requestMes + ")";
                    new B2bOrderData().InsertOrUpdate(modelb2border);

                    TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\BaoxianLog.txt", "mresp.resultCode != 0");
                    return;
                }
            }




            Response.Write("success");

            Response.End();
        }
    }
}