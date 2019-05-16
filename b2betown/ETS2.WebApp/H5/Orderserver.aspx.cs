using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.WebApp.H5
{
    public partial class Orderserver : System.Web.UI.Page
    {

        public string title = "";
        public int comid = 0;
        public string pno="";
        public int proid=0;
        public int err = 0;
        public string errtext = "";
        public string phone = "";
        public string name = "";
        public string u_phone = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            pno = Request["pno"].ConvertTo<string>("");

            if (pno != "")
            {

                string pno1 = EncryptionHelper.EticketPnoDES(pno, 1);//解密

                var prodata = new B2bEticketData();
                var eticketinfo = prodata.GetEticketDetail(pno1);
                if (eticketinfo != null)
                {
                    comid = eticketinfo.Com_id;//重新设定最大数 不能大于可以预定数量
                    title = eticketinfo.E_proname;

                    //if (eticketinfo.V_state == 2) {
                    //    if (eticketinfo.sendcard == 0) {
                    //        err = 1;
                    //        errtext = "此电子票已验证，不可购买服务，请到窗口办理！";
                    //    }
                    //}


                    //if (eticketinfo.Pnum > 1) {
                    //    err = 1;
                    //    errtext = "此电子票不可购买服务，请到窗口办理！";
                    //}

                    var orderinfo = new B2bOrderData().GetOrderById(eticketinfo.Oid);
                    if (orderinfo != null) {
                        u_phone = orderinfo.U_phone;
                        name = orderinfo.U_name;
                    }



                    proid = eticketinfo.Pro_id;
                    B2b_com_pro pro = new B2bComProData().GetProById(proid.ToString());
                    if (pro != null)
                    {
                        if (pro.Rentserverid == "") {
                            err = 1;
                            errtext = "此产品不可购买服务！";
                        }
                    }
                    else {
                        err = 1;
                        errtext = "此产品不可购买服务！";
                    
                    }




                }
                else {
                    err = 1;
                    errtext = "此电子票不可用！";
                
                }




            }
            else {
                err = 1;
                errtext = "此电子票不可用";
            }

        }
    }
}