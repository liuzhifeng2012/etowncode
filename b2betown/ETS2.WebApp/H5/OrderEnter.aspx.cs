using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.JsonFactory;
using System.Xml;
using Newtonsoft.Json;
using ETS2.PM.Service.PMService.Modle.Enum;

namespace ETS2.WebApp.H5
{
    public partial class OrderEnter : System.Web.UI.Page
    {
        public bool bo = true;
        public int comid = 0;
        public string title = "";
        public string comlogo = "";
        public string logoimg = "";
        public string pro_name = "";
        public string imgurl = "";
        public string weixinname = "";
        public string Scenic_intro = "";
        public int id = 0;
        public string price = "";
        public string summary = "";
        public string sumaryend = "";
        public string nowdate = "";//现在日期
        public DateTime nowtoday;

        public DateTime panic_begintime;
        public DateTime panicbuy_endtime;
        public int ispanicbuy = 0;//抢购判断


        public string Wxfocus_url = "";
        public string Wxfocus_author = "";

        public string ordertype = "1";//订单类型 默认为1订单；2充值
        public string costprice = "";//成本价
        public string lirun_price = "";//利润

        public string remark = "";
        public int num = 1;
        public int pro_num = 0;
        public int pro_max = 0;
        public int pro_min = 0;
        public string pro_explain = "";
        public string phone = "";

        public Decimal Imprest = 0;
        public Decimal Integral = 0;


        public string openid = "";//微信号
        public string weixinpass = "";//微信一次性密码


        public int buyuid = 0; //购买用户ID
        public int tocomid = 0;//来访商户COMID

        public int shijiacha = 0;


        public string provalidatemethod;//判断 1按产品有效期，2指定有效期
        public int appointdate;//1=一星期，，2=1个月，3=3个月，4=6个月，5=一年
        public int iscanuseonsameday;//1当天可用，0当天不可用


        public DateTime pro_end;
        public DateTime pro_start;
        public string pro_youxiaoqi = "";


        public int projectid = 0;//项目id


        public int iscanbook = 1;//产品是否可以预订

        public decimal childreduce = 0;//儿童减免费用
        public int pro_servertype = 1;//产品服务类型1.票务10.旅游大巴

        public int proclass = 0;//产品分类


        public string pickuppoint = "";//上车地点
        public string dropoffpoint = "";//下车地点

        public int manyspeci = 0;
        public List<B2b_com_pro_Speci> gglist = null;//规格列表
        public decimal face_price = 0;
        //public decimal price = 0;
        public string pno = "";//预约电子码 加密
        public int Use_pnum = 0;
        public string bindingname = "";
        public string bindingphone = "";
        public string bindingcard = "";
        public int limitweek = 0;//是否周末，平日独立 限制数量
        public int limitweekdaynum = 0;//平日限制数量
        public int limitweekendnum = 0;//周末限制数量
        public int Iuse = 0;//自己使用

        public int isSetVisitDate = 0;//是否需要显示预约日期

        public bool issetfinancepaytype = false;//是否设置了微信支付参数

        public bool iswxsubscribenum = false;//是否是微信认证订阅号/订阅号

        protected void Page_Load(object sender, EventArgs e)
        {
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            bo = detectmobilebrowser.HttpUserAgent(u);


            proclass = Request["proclass"].ConvertTo<int>(0);
            projectid = Request["projectid"].ConvertTo<int>(0);

            //string u = Request.ServerVariables["HTTP_USER_AGENT"];
            //bool bo = detectmobilebrowser.HttpUserAgent(u);

            id = Request["id"].ConvertTo<int>(0);
            num = Request["num"].ConvertTo<int>(1);
            nowdate = DateTime.Now.ToString("yyyy-MM-dd");
            nowtoday = DateTime.Now;
            buyuid = Request["buyuid"].ConvertTo<int>(0);
            tocomid = Request["tocomid"].ConvertTo<int>(0);
            pno = Request["pno"].ConvertTo<string>("");
            if (id != 0)
            {

                B2b_com_pro pro = new B2bComProData().GetProById(id.ToString());
                if (pro != null)
                {
                    isSetVisitDate = pro.isSetVisitDate;
                    // 作废超时未支付订单，完成回滚操作
                    int rs = new B2bComProData().CancelOvertimeOrder(pro);

                    if (pro.Source_type == 4)
                    {
                        iscanbook = 1;
                    }
                    else
                    {
                        iscanbook = new B2bComProData().IsYouxiao(pro.Id, pro.Server_type, pro.Pro_start, pro.Pro_end, pro.Pro_state);//判断产品是否有效：1.票务，直接判断有效期 和产品上线状态2.酒店，跟团游，当地游 则判断是否含有有效的房态/团期 以及产品上下线状态
                    }

                    //判断微信 是否是认证服务号
                    WeiXinBasic mbasic = new WeiXinBasicData().GetWxBasicByComId(pro.Com_id);
                    if (mbasic == null)
                    {
                        iswxsubscribenum = false;
                    }
                    else
                    {
                        if (mbasic.Weixintype == 1 || mbasic.Weixintype == 2)
                        {
                            iswxsubscribenum = true;
                        }
                        else
                        {
                            iswxsubscribenum = false;
                        }
                    }

                    
                    pro_servertype = pro.Server_type;
                    pickuppoint = pro.pickuppoint;
                    dropoffpoint = pro.dropoffpoint;


                    childreduce = pro.Childreduce;
                    imgurl = FileSerivce.GetImgUrl(pro.Imgurl);

                    //实物产品必须进入 新商城页面订购
                    if (pro.Server_type == 11) {
                        Response.Redirect("/h5/order/pro.aspx?id=" + id);
                    }

                    face_price = pro.Face_price;
      
                    manyspeci = pro.Manyspeci;
                    //如果多规格读取规格
                    if (manyspeci == 1)
                    {
                        gglist = new B2b_com_pro_SpeciData().Getgglist(pro.Id);
                    }

                }
                if (pro.Ispanicbuy == 1)
                {
                    panic_begintime = pro.Panic_begintime;
                    panicbuy_endtime = pro.Panicbuy_endtime;


                    TimeSpan tss = pro.Panic_begintime - nowtoday;
                    var day = tss.Days * 24 * 3600; ;      //这是相差的天数
                    var h = tss.Hours * 3600;              //这是相差的小时数，
                    var m = tss.Minutes * 60;
                    var s = tss.Seconds;
                    shijiacha = day + h + m + s;
                }
                projectid = pro.Projectid;
                comid = pro.Com_id;
                pro_name = pro.Pro_name;
                price = pro.Advise_price.ToString();
                if (price == "0.00" || price == "0")
                {
                    price = "0";
                }
                else
                {
                    CommonFunc.OperTwoDecimal(price);
                    //price = price.IndexOf(".") != -1 ? price.Substring(0, price.IndexOf(".")) : price;                    
                }

                nowdate = DateTime.Now.ToString("yyyy-MM-dd");
                summary = "包含服务：" + pro.Service_Contain;
                sumaryend = summary;
                if (pro.Service_NotContain != "")
                {
                    sumaryend += "</br> 不包含服务：" + pro.Service_NotContain + "</br>";
                }
                if (pro.Precautions != "")
                {
                    sumaryend += "注意事项：" + pro.Precautions;
                }

                //如果服务类型是 票务；  则备注信息中 显示 电子码使用限制
                if (pro.Server_type == 1)
                {
                    if (pro.Iscanuseonsameday == 0)//电子码当天不可用
                    {
                        sumaryend = "此产品当天预订不可用<br>" + sumaryend;
                    }
                    if (pro.Iscanuseonsameday == 1)//电子码当天可用
                    {
                        sumaryend = "此产品当天预订可用<br>" + sumaryend;
                    }
                    if (pro.Iscanuseonsameday == 2)//电子码出票2小时内不可用
                    {
                        sumaryend = "此产品出票2小时内不可用<br>" + sumaryend;
                    }
                }
                if (summary.Length > 30)
                {
                    summary = summary.Substring(0, 30);
                }
                if (summary.Length > 150)
                {
                    summary = summary.Substring(0, 150) + "...";
                }
                remark = pro.Pro_Remark;
                pro_num = pro.Pro_number;
                if (pro_num == 0)
                {
                    pro_max = 100;
                    pro_min = 1;
                }
                else
                {
                    pro_min = 1;
                    pro_max = pro_num;
                }


                if (pno != "") {

                    pro_max = 1;

                    string pno1 = EncryptionHelper.EticketPnoDES(pno, 1);//解密

                    var prodata = new B2bEticketData();
                    var eticketinfo = prodata.GetEticketDetail(pno1);
                    if (eticketinfo != null) {
                        Use_pnum = eticketinfo.Use_pnum;//重新设定最大数 不能大于可以预定数量

                        bindingname =eticketinfo.bindingname;
                        bindingphone = eticketinfo.bindingphone;
                        bindingcard = eticketinfo.bindingcard;
                    }


                    //查询绑定状态
                    var busbindingdata = new Bus_FeeticketData();
                    var busbindinginfo = busbindingdata.Bus_Feeticket_proById(0, 0, id, pno1);
                    if (busbindinginfo != null) { 
                        
                        limitweek = busbindinginfo.limitweek;
                        limitweekdaynum = busbindinginfo.limitweekdaynum;
                        limitweekendnum = busbindinginfo.limitweekendnum;

                        if (busbindinginfo.Busid != 0) {
                            var busfeetticketinfo = busbindingdata.GetBus_FeeticketById(busbindinginfo.Busid, eticketinfo.Com_id);
                            if (busfeetticketinfo != null) {
                                Iuse =busfeetticketinfo.Iuse;
                            }
                            
                        }
                    }

                    //如果不是限制个人使用 则 最大限购数不为 电子票剩余使用量
                    if (Iuse == 0) {
                        pro_max = Use_pnum;
                    }


                
                }



                if (pro.Server_type == 1)
                {
                    pro_explain = pro.Pro_explain;

                }

                #region 产品有效期判定(微信模板--门票订单预订成功通知 中也有用到)
                provalidatemethod = pro.ProValidateMethod;//判断 1按产品有效期，2指定有效期
                appointdate = pro.Appointdata;//1=一星期，，2=1个月，3=3个月，4=6个月，5=一年
                iscanuseonsameday = pro.Iscanuseonsameday;//1当天可用，0当天不可用

                DateTime pro_end = pro.Pro_end;
                //返回有效期
                pro_youxiaoqi = new B2bComProData().GetPro_Youxiaoqi(pro.Pro_start, pro.Pro_end, provalidatemethod, appointdate, iscanuseonsameday);

                #endregion



                var commodel = B2bCompanyData.GetCompany(comid);

                if (commodel != null)
                {
                    if (commodel.B2bcompanyinfo != null)
                    {
                        Wxfocus_url = commodel.B2bcompanyinfo.Wxfocus_url;
                        Wxfocus_author = commodel.B2bcompanyinfo.Wxfocus_author;
                        weixinname = commodel.B2bcompanyinfo.Weixinname;
                        Scenic_intro = commodel.B2bcompanyinfo.Scenic_intro;

                    }

                    title = commodel.Com_name;
                }


                B2b_company_saleset procom = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());
                if (procom != null)
                {

                    comlogo = FileSerivce.GetImgUrl(procom.Smalllogo.ConvertTo<int>(0));
                    logoimg = comlogo;
                }


                var saleset = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());
                if (saleset != null)
                {
                    phone = saleset.Service_Phone;
                }

                //查询项目电话,如果有项目电话调取项目电话
                var projectdata = new B2b_com_projectData();
                var projectmodel = projectdata.GetProject(projectid, comid);
                if (projectmodel != null)
                {
                    if (projectmodel.Mobile != "")
                    {
                        phone = projectmodel.Mobile;
                    }
                }





                //从cookie中得到微信号
                if (Request.Cookies["openid"] != null)
                {
                    openid = Request.Cookies["openid"].Value;
                }
                B2bCrmData b2b_crm = new B2bCrmData();
                if (openid != "")
                {
                    B2b_crm b2bmodle = b2b_crm.b2b_crmH5(openid, comid);
                    if (b2bmodle != null)
                    {
                        Imprest = b2bmodle.Imprest;
                        Integral = b2bmodle.Integral;
                    }

                }

            }
        }
    }
}