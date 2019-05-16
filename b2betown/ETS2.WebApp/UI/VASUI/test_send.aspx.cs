using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS2.Common.Business;
using Com.Alipay;
using System.Collections.Specialized;
using ETS2.VAS.Service.VASService.Data;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;


namespace ETS2.WebApp.UI.VASUI
{
	public partial class test_send : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            int order_no = Int32.Parse(Request.QueryString["out_trade_no"]);	        //获取订单号
            //返回订单号
            int orderid = order_no;
            if (orderid != 0)
            {
                //根据订单id得到订单信息
                B2bOrderData dataorder = new B2bOrderData();
                B2b_order modelb2border = dataorder.GetOrderById(orderid);

                //根据产品id得到产品信息
                B2bComProData datapro = new B2bComProData();
                B2b_com_pro modelcompro = datapro.GetProById(modelb2border.Pro_id.ToString());


                    //生成电子码
                    int u_num = modelb2border.U_num;
                    int comid = modelcompro.Com_id;
                    RandomCodeData datarandomcode = new RandomCodeData();
                    RandomCode modelrandomcode = datarandomcode.GetRandomCode();//得到未用随机码对象

                    //设置取出的电子码状态为1（已使用）
                    modelrandomcode.State = 1;
                    int randomcodeid = datarandomcode.InsertOrUpdate(modelrandomcode);
                    string eticketcode = "9" + comid.ToString() + modelrandomcode.Code.ToString();
                    string sendstr = "";

                    //录入电子票列表
                    B2bEticketData eticketdata = new B2bEticketData();
                    B2b_eticket eticket = new B2b_eticket()
                    {

                        Id = 0,
                        Com_id = comid,
                        Pro_id = modelcompro.Id,
                        Agent_id = 0,//直销
                        Pno = eticketcode,
                        E_type = (int)EticketCodeType.ShuZiMa,
                        Pnum = modelb2border.U_num,
                        Use_pnum = modelb2border.U_num,
                        E_proname = modelcompro.Pro_name,
                        E_face_price = modelcompro.Face_price,
                        E_sale_price = modelcompro.Advise_price,
                        E_cost_price = modelcompro.Agentsettle_price,
                        V_state = (int)EticketCodeStatus.NotValidate,
                        Send_state = (int)EticketCodeSendStatus.NotSend,
                        Subdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    };
                    int eticketid = eticketdata.InsertOrUpdate(eticket);
                    if (eticketid > 0)
                    {

                        //生成电子码短信，稍后可单独写类或写到数据库中
                        diveticketcode.InnerText = "电子码生成成功:" + eticketcode;
                        sendstr = "感谢您订购" + modelcompro.Pro_name + modelb2border.U_num + "张" + ",电子码:" + eticketcode + " 有效期至:" + modelcompro.Pro_end.ToString("yyyy-MM-dd");

                    }
                    else
                    {
                        diveticketcode.InnerText = "电子码生成ERROR";

                    }


            }

		}
	}
}