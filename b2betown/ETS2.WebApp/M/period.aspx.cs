using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.WebApp.M
{
    public partial class period : System.Web.UI.Page
    {
        public int id = 0;
        public int comid = 0;
        public int type = 0;
        public string time = "";
        public string wxtype = "";
        public int over = 0;
        public int periodca = 0;
        public int peryear = 0;
        public string comname = "";//公司名称
        public string comlogo = "";//公司logo

        public string lastperiodcaurl = "#";//上一期地址
        public string nextperiodcaurl = "#";//下一期地址

        protected void Page_Load(object sender, EventArgs e)
        {
            var totalcount = 0;
            id = Request["id"].ConvertTo<int>(0);
            type = Request["type"].ConvertTo<int>(0);

           
            //获取访问的域名   
            string RequestDomin = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToLower();
            //根据访问的域名获得公司信息
            WeiXinBasic basicc = new WeiXinBasicData().GetWeiXinBasicByDomain(RequestDomin);


            if (basicc != null) {
                comid = basicc.Comid;
                //根据公司id得到公司logo地址和公司名称
                comname = B2bCompanyData.GetCompany(comid).Com_name;

                B2b_company_saleset pro = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());
                if (pro != null)
                {
                    if (pro.Smalllogo != null && pro.Smalllogo != "")
                    {
                        comlogo = FileSerivce.GetImgUrl(pro.Smalllogo.ConvertTo<int>(0));
                    }
                }
                if (id == 0)//如果没有期号传过来，默认显示最新期
                {
                    id = new WxMaterialData().GetNewestPerical(type,comid);
                }
            }


            

            int periodid = id;
            int salepromotetypeid = type;



            if (id != 0 && type != 0)
            {
                List<WxMaterial> list = new WxMaterialData().periodicaltypelist(1, 20, 10, periodid, salepromotetypeid, out totalcount);
                //Repeater1.DataSource = list;
                //Repeater1.DataBind();

                periodical per = new WxMaterialData().selectperiodical(id);

                if (per != null)
                {
                    periodca = per.Percal;

                    if (periodca > 1)
                    {
                        int lastperiodca = periodca - 1;


                        periodical lastper = new WxMaterialData().Selperiod(lastperiodca, type);
                        lastperiodcaurl = "/M/period.aspx?id=" + lastper.Id + "&type=" + type;
                    }



                    time = per.Uptime.ToString("yyyy-MM-dd");

                    peryear = per.Peryear;
                }

                wxtype = new WxSalePromoteTypeData().GetWxMenu(salepromotetypeid).Typename;

                over = new WxMaterialData().selectWxsaletype(salepromotetypeid, comid).Percal;

                if (periodca < over)
                {
                    int nextperiodca = periodca + 1;
                    periodical nextper = new WxMaterialData().Selperiod(nextperiodca, type);
                    nextperiodcaurl = "/M/period.aspx?id=" + nextper.Id + "&type=" + type;
                }

            }

        }

        public string substr(string str)
        {
            string strnum = str;
            if (strnum == "0.00")
            {
                strnum = " ";
            }
            else
            {
                strnum = "￥" + strnum + "起";
            }
            return strnum;
        }

    }
}