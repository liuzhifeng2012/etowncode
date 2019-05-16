using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using Newtonsoft.Json;
using ETS.Framework;
using System.Xml;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Member.Service.MemberService.Data;
using System.Threading;
using ETS2.CRM.Service.CRMService.Modle.Enum;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.Common.Business;

namespace ETS2.WebApp.Excel
{
    public partial class ObtainGZList : System.Web.UI.Page
    {
        public int comid = UserHelper.CurrentCompany.ID;//公司id
        public string comname = UserHelper.CurrentCompany.Com_name;//公司名称
        public bool IsParentCompanyUser = true; //判断操作账户类型(总公司账户;门市账户)

        public int dealuserid = UserHelper.CurrentUserId();

        public int isrenzheng = 1;//1已认证；0未认证
        protected void Page_Load(object sender, EventArgs e)
        {

            int userid = UserHelper.CurrentUserId();
            IsParentCompanyUser = new B2bCompanyManagerUserData().IsParentCompanyUser(userid);

            ObtainGzListSplit lastSplit = new ObtainGzListSplitData().LastSplitNoByComid(comid);//最后一次拆分记录
            if (lastSplit != null)
            {
                BindGrid2(comid, lastSplit.Splitno, lastSplit.Obtainno);
            }

            if (UserHelper.ValidateLogin())
            {
                int comidd = UserHelper.CurrentCompany.ID;
                WeiXinBasic wxbasic = new WeiXinBasicData().GetWxBasicByComId(comidd);
                if (wxbasic != null)
                {

                    if (wxbasic.Weixintype == 4)
                    {
                        isrenzheng = 1;
                    }
                    else
                    {
                        isrenzheng = 0;
                    }
                }
                else
                {
                    isrenzheng = 0;
                }
            }
            else
            {
                Response.Redirect("/Manage/index1.html");
            }

        }
        #region "页面加载中效果"
        /// <summary>
        /// 页面加载中效果
        /// </summary>
        public static void initJavascript()
        {
            HttpContext.Current.Response.Write(" <script language=JavaScript type=text/javascript>");
            HttpContext.Current.Response.Write("var t_id = setInterval(animate,20);");
            HttpContext.Current.Response.Write("var pos=0;var dir=2;var len=0;");
            HttpContext.Current.Response.Write("function animate(){");
            HttpContext.Current.Response.Write("var elem = document.getElementById('progress');");
            HttpContext.Current.Response.Write("if(elem != null) {");
            HttpContext.Current.Response.Write("if (pos==0) len += dir;");
            HttpContext.Current.Response.Write("if (len>32 || pos>79) pos += dir;");
            HttpContext.Current.Response.Write("if (pos>79) len -= dir;");
            HttpContext.Current.Response.Write(" if (pos>79 && len==0) pos=0;");
            HttpContext.Current.Response.Write("elem.style.left = pos;");
            HttpContext.Current.Response.Write("elem.style.width = len;");
            HttpContext.Current.Response.Write("}}");
            HttpContext.Current.Response.Write("function remove_loading() {");
            HttpContext.Current.Response.Write(" this.clearInterval(t_id);");
            HttpContext.Current.Response.Write("var targelem = document.getElementById('loader_container');");
            HttpContext.Current.Response.Write("targelem.style.display='none';");
            HttpContext.Current.Response.Write("targelem.style.visibility='hidden';");
            HttpContext.Current.Response.Write("}");
            HttpContext.Current.Response.Write("</script>");
            HttpContext.Current.Response.Write("<style>");
            HttpContext.Current.Response.Write("#loader_container {text-align:center; position:absolute; top:40%; width:100%; left: 0;}");
            HttpContext.Current.Response.Write("#loader {font-family:Tahoma, Helvetica, sans; font-size:11.5px; color:#000000; background-color:#FFFFFF; padding:10px 0 16px 0; margin:0 auto; display:block; width:130px; border:1px solid #5a667b; text-align:left; z-index:2;}");
            HttpContext.Current.Response.Write("#progress {height:5px; font-size:1px; width:1px; position:relative; top:1px; left:0px; background-color:#8894a8;}");
            HttpContext.Current.Response.Write("#loader_bg {background-color:#e4e7eb; position:relative; top:8px; left:8px; height:7px; width:113px; font-size:1px;}");
            HttpContext.Current.Response.Write("</style>");
            HttpContext.Current.Response.Write("<div id=loader_container>");
            HttpContext.Current.Response.Write("<div id=loader>");
            HttpContext.Current.Response.Write("<div align=center>数据正在导入,请勿关闭页面... </div>");
            HttpContext.Current.Response.Write("<div id=loader_bg><div id=progress> </div></div>");
            HttpContext.Current.Response.Write("</div></div>");
            HttpContext.Current.Response.Flush();
        }
        public static void UnloadJavascript()
        {
            HttpContext.Current.Response.Write(" <script language=JavaScript type=text/javascript>");
            HttpContext.Current.Response.Write("remove_loading();");
            HttpContext.Current.Response.Write(" </script>");
        }
        #endregion


        void BindGrid(int comid, int obtainno)
        {
            GridView1.DataSource = ExcelSqlHelper.ExecuteReader("select * from ObtainGzListLog where comid=" + comid + " and obtainno=" + obtainno);
            GridView1.DataBind();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            initJavascript();
            //获得以前总共抓取关注者列表的次数
            int MaxOtainNo = new ObtainGzListLogData().GetMaxObtainNo(comid);
            if (MaxOtainNo == 0)
            {
                Literal2.Text = "还未获取过微信关注者列表";
            }
            else
            {
                ////验证一下是否拆分过,拆分过不在拆分
                //bool issplited = false;
                //if (issplited == true)
                //{
                //    Literal2.Text = "关注者列表已经拆分过无需再分";
                //    return;
                //}
                //else
                //{
                //获得以前总共拆分过关注者列表的次数
                int MaxSplitNo = new ObtainGzListSplitData().MaxSplitNo(comid, MaxOtainNo);

                IList<ObtainGzListLog> loglist = new ObtainGzListLogData().GetObtaingzlistlog(comid, MaxOtainNo);
                if (loglist != null)
                {
                    string openidstr = "";
                    foreach (ObtainGzListLog log in loglist)
                    {
                        openidstr += log.Openid + ",";
                    }
                    openidstr = openidstr.Substring(0, openidstr.Length - 1);

                    string[] str = openidstr.Split(',');

                    //每20000个一组，判断可以拆分为几组
                    int splitnum = int.Parse(Math.Ceiling((double)(str.Length) / (double)20000).ToString());

                    if (splitnum == 1)
                    {
                        ObtainGzListSplit splitmodel = new ObtainGzListSplit
                        {
                            Id = 0,
                            Comid = comid,
                            Dealerid = dealuserid,
                            Total = str.Length,
                            Splitcount = str.Length,
                            Splitno = MaxSplitNo + 1,
                            Obtainno = MaxOtainNo,
                            Splittime = DateTime.Now,
                            Splitopenid = openidstr,
                            Issuc = 1
                        };

                        new ObtainGzListSplitData().InsertsObtainGzListSplit(splitmodel);

                    }
                    else
                    {
                        //获取拆分的openid字符串
                        for (int i = 1; i <= splitnum; i++)
                        {
                            if (i == splitnum)
                            {
                                //录入最后不满20000的openid字符串
                                string splitopenid = "";
                                for (int j = 20000 * (splitnum - 1); j < str.Length; j++)
                                {
                                    splitopenid += str[j] + ",";
                                }
                                splitopenid = splitopenid.Substring(0, splitopenid.Length - 1);
                                ObtainGzListSplit splitmodel = new ObtainGzListSplit
                                {
                                    Id = 0,
                                    Comid = comid,
                                    Dealerid = dealuserid,
                                    Total = str.Length,
                                    Splitcount = (str.Length) % 20000,
                                    Splitno = MaxSplitNo + 1,
                                    Obtainno = MaxOtainNo,
                                    Splittime = DateTime.Now,
                                    Splitopenid = splitopenid,
                                    Issuc = 1
                                };

                                new ObtainGzListSplitData().InsertsObtainGzListSplit(splitmodel);
                            }
                            else
                            {
                                string splitopenid = "";
                                for (int j = 20000 * (i - 1); j < 20000 * i; j++)
                                {
                                    splitopenid += str[j] + ",";
                                }
                                splitopenid = splitopenid.Substring(0, splitopenid.Length - 1);


                                ObtainGzListSplit splitmodel = new ObtainGzListSplit
                                {
                                    Id = 0,
                                    Comid = comid,
                                    Dealerid = dealuserid,
                                    Total = str.Length,
                                    Splitcount = 20000,
                                    Splitno = MaxSplitNo + 1,
                                    Obtainno = MaxOtainNo,
                                    Splittime = DateTime.Now,
                                    Splitopenid = splitopenid,
                                    Issuc = 1
                                };

                                new ObtainGzListSplitData().InsertsObtainGzListSplit(splitmodel);
                            }
                        }



                    }

                    UnloadJavascript();
                    Literal2.Text = "微信关注者总数目:" + str.Length;


                    BindGrid2(comid, MaxSplitNo + 1, MaxOtainNo);
                }
                else
                {
                    UnloadJavascript();
                    Literal2.Text = "获取微信关注者列表为空";
                }
                //}
            }

        }

        private void BindGrid2(int comid, int SplitNo, int obtainno)
        {
            GridView2.DataSource = ExcelSqlHelper.ExecuteReader("select * from [ObtainGzList_Split] where comid=" + comid + " and [splitno]=" + SplitNo + " and obtainno=" + obtainno);
            GridView2.DataBind();
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            initJavascript();
            //根据公司id得到开发者凭据
            WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(comid);
            if (basicc != null)
            {
                //获取凭证（首先判断数据库是否存在凭证以及是否过期，如过期，重新获取）
                WXAccessToken token = GetAccessToken(comid, basicc.AppId, basicc.AppSecret);

                //获得以前总共抓取关注者列表的次数
                int MaxOtainNo = new ObtainGzListLogData().GetMaxObtainNo(comid);

                //计算需要向微信端发送请求次数
                int weixintotal = AskWeixinTotal(token.ACCESS_TOKEN);//获得关注微信用户总数目
                if (weixintotal == 0)
                {
                    Literal1.Text = "获得关注微信用户总数目出错";
                    return;
                }
                int askno = int.Parse(Math.Ceiling((double)weixintotal / (double)10000).ToString());

                for (int i = 0; i < askno; i++)
                {
                    if (i == 0)//第一次请求，发送无参数next_openid
                    {
                        AskWeixin(token.ACCESS_TOKEN, "", MaxOtainNo + 1);
                    }
                    else //第一次以后请求，发送带参数next_openid
                    {
                        string next_openid = new ObtainGzListLogData().GetNextOpenId(comid, MaxOtainNo + 1);
                        if (next_openid != "")
                        {
                            AskWeixin(token.ACCESS_TOKEN, next_openid, MaxOtainNo + 1);
                        }
                    }
                }

                //BindGrid(comid, MaxOtainNo + 1);
                UnloadJavascript();
                Literal1.Text = "共获取微信关注用户数量:" + weixintotal;
            }
            else
            {
                UnloadJavascript();
                Literal1.Text = "公司开发者凭据获取有误";

            }

        }

        private int AskWeixinTotal(string token)
        {
            //获取关注者列表

            string geturl = "https://api.weixin.qq.com/cgi-bin/user/get?access_token=" + token;

            string jsonText = new GetUrlData().HttpGet(geturl);
            XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + jsonText + "}");

            XmlElement rootElement = doc.DocumentElement;

            if (rootElement.SelectSingleNode("errcode") == null)
            {

                string total = rootElement.SelectSingleNode("total").InnerText;
                return int.Parse(total);
            }
            else
            {
                return 0;
            }
        }

        private void AskWeixin(string token, string nextopenid, int nowotainno)
        {
            //获取关注者列表

            string geturl = "https://api.weixin.qq.com/cgi-bin/user/get?access_token=" + token;
            if (nextopenid != "")
            {
                geturl = "https://api.weixin.qq.com/cgi-bin/user/get?access_token=" + token + "&next_openid=" + nextopenid;
            }
            string jsonText = new GetUrlData().HttpGet(geturl);
            XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + jsonText + "}");

            XmlElement rootElement = doc.DocumentElement;

            if (rootElement.SelectSingleNode("errcode") == null)
            {
                int id = 0;
                string total = rootElement.SelectSingleNode("total").InnerText;
                string count = rootElement.SelectSingleNode("count").InnerText;
                string next_openid = rootElement.SelectSingleNode("next_openid").InnerText;
                XmlNode data = rootElement.SelectSingleNode("data");
                string openid = "";
                foreach (XmlNode node in data.ChildNodes)
                {
                    openid += node.InnerText + ",";
                }
                openid = openid.Substring(0, openid.Length - 1);

                DateTime obtaintime = DateTime.Now;
                string errcode = "";
                string errmsg = "";

                int insertlog = new ObtainGzListLogData().InsObtainLog(id, total, count, openid, next_openid, obtaintime, errcode, errmsg, comid, dealuserid, nowotainno);

            }
            else
            {
                int id = 0;
                string total = "0";
                string count = "0";
                string next_openid = "0";
                string openid = "";
                DateTime obtaintime = DateTime.Now;
                XmlNode data = rootElement.SelectSingleNode("data");
                string errcode = rootElement.SelectSingleNode("errcode").InnerText;
                string errmsg = rootElement.SelectSingleNode("errmsg").InnerText;

                int insertlog = new ObtainGzListLogData().InsObtainLog(id, total, count, openid, next_openid, obtaintime, errcode, errmsg, comid, dealuserid, nowotainno);


            }
        }
        /// <summary>
        /// 获取凭证（首先判断数据库是否存在凭证以及是否过期，如过期，重新获取）
        /// </summary>
        /// <returns></returns>
        private static WXAccessToken GetAccessToken(int comid, string AppId, string AppSecret)
        {
            DateTime fitcreatetime = DateTime.Now.AddHours(-2);
            WXAccessToken token = new WXAccessTokenData().GetLaststWXAccessToken(fitcreatetime, comid);
            if (token == null)
            {
                string geturl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppId + "&secret=" + AppSecret;
                string jsonText = new GetUrlData().HttpGet(geturl);

                XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + jsonText + "}");

                XmlElement rootElement = doc.DocumentElement;
                string access_token = rootElement.SelectSingleNode("access_token").InnerText;

                //把获取到的凭证录入数据库中
                token = new WXAccessToken()
                {
                    Id = 0,
                    ACCESS_TOKEN = access_token,
                    CreateDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    Comid = comid

                };
                int edittoken = new WXAccessTokenData().EditAccessToken(token);
            }
            return token;
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            initJavascript();
            string id = ((Button)sender).CommandArgument.ToString();
            //Response.Write("<script>alert('" + id + "')</script>");
            //根据id得到拆分的的记录
            ObtainGzListSplit model = new ObtainGzListSplitData().GetObtainGzListSplit(int.Parse(id));
            if (model != null)
            {
                //根据公司得到会员录入的次数
                int MaxImportNum = new ExcelImportLogData().GetMaxImportNum(comid);
                int rowAffected = 0;//录入正确的条数
                int rowerr = 0;//录入错误的条数

                string dd = "";//页面提示内容

                string[] str = model.Splitopenid.Split(',');
                for (int i = 0; i < str.Length; i++)
                {

                    if (str[i] != "")
                    {

                        int whetherwxfocus = 0;//微信关注状态标注为1(未关注)
                        int whetheractivate = 0;//用户激活状态标注为0(未激活)

                        string name = "";//姓名
                        string phone = "";//手机号     
                        string email = "";//email
                        string weixin = str[i];//微信号
                        if (weixin != "")
                        {
                            whetherwxfocus = 1;
                            whetheractivate = 1;
                        }


                        int importstate = 1;//录入状态默认1：成功;0:出错
                        string ErrReason = "";//录入错误原因

                        bool ishascrmphone = false;
                        bool ishascrmweixin = false;
                        //if (phone != "")
                        //{
                        //    ishascrmphone = new B2bCrmData().IsHasCrmPhone(comid, phone);//判断当前公司会员是否已经绑定当前手机 
                        //    if (ishascrmphone == true)
                        //    {
                        //        ErrReason = "当前公司已有会员绑定过手机" + phone;

                        //    }
                        //}
                        if (weixin != "")
                        {
                            ishascrmweixin = new B2bCrmData().IsHasCrmWeiXin(comid, weixin);//判断当前公司会员是否已经绑定当前微信
                            if (ishascrmweixin == true)
                            {
                                ErrReason = "当前公司已有会员绑定过微信" + weixin;

                            }
                        }
                        if (ishascrmphone == false && ishascrmweixin == false)
                        {
                            if (phone == "" && weixin == "")
                            {
                                ErrReason = "导入会员的手机，微信必须至少有其中一项";
                                //会员通过excel录入日志
                                importstate = 0;
                                int importnum = MaxImportNum + 1;
                                string importtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                int insertlog = new ExcelImportLogData().InsExcelImportLog(importstate, 0, comid, "0", name, phone, weixin, whetherwxfocus, whetheractivate, importtime, importnum, email, ErrReason);
                                rowerr++;
                            }
                            else
                            {
                                //创建卡号并插入活动
                                string cardcode = MemberCardData.CreateECard(2, comid);
                                //插入会员表
                                decimal imprest = 0;//预付款
                                decimal integral = 0;//积分                     
                                string agegroup = "";//年龄段
                                string crmlevel = "A";//会员级别,默认网站注册

                                string country = "";
                                string province = "";
                                string city = "";
                                string address = "";

                                //插入会员表
                                int insb2bcrm = new B2bCrmData().ExcelInsB2bCrm(comid, cardcode, name, phone, weixin, whetherwxfocus, whetheractivate, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), email, imprest, integral, country, province, city, address, agegroup, crmlevel);
                                //插入关注赠送优惠券
                                int jifen_temp = 0;
                                var InputMoney = MemberCardData.AutoInputMoeny(insb2bcrm, 4, comid, out jifen_temp);

                                //会员通过excel录入日志
                                int importnum = MaxImportNum + 1;
                                string importtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                int insertlog = new ExcelImportLogData().InsExcelImportLog(importstate, insb2bcrm, comid, cardcode, name, phone, weixin, whetherwxfocus, whetheractivate, importtime, importnum, email, ErrReason);

                                rowAffected++;
                            }
                        }
                        else
                        {
                            //会员通过excel录入日志
                            importstate = 0;
                            int importnum = MaxImportNum + 1;
                            string importtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            int insertlog = new ExcelImportLogData().InsExcelImportLog(importstate, 0, comid, "0", name, phone, weixin, whetherwxfocus, whetheractivate, importtime, importnum, email, ErrReason);
                            rowerr++;
                        }

                    }

                }
                UnloadJavascript();
                dd += "成功导入数据共：" + rowAffected.ToString() + "条;";
                if (rowerr > 0)
                {
                    dd += "重复数据共：" + rowerr.ToString() + "条;";
                }

                Response.Write("<script>alert('" + dd + "');</script>");
            }
            else
            {
                UnloadJavascript();
                Response.Write("<script>alert('拆分记录为空');</script>");
            }


        }
    }
}