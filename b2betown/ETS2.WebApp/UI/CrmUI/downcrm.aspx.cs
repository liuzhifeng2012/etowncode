using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using System.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using System.Collections;
using ETS2.Member.Service.MemberService.Data;

namespace ETS2.WebApp.UI.CrmUI
{
    public partial class downcrm : System.Web.UI.Page
    {
        public int comid = 0;
        public string md5info = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comid = Request["comid"].ConvertTo<int>(0);
            md5info = Request["md5info"].ConvertTo<string>("");

            string companyName = new B2bCompanyData().GetCompanyNameById(comid);


            string Returnmd5 = EncryptionHelper.ToMD5(comid.ToString() + "lixh1210", "UTF-8");
            if (Returnmd5 == md5info)//验证MD5
            {
                //ExcelRender.RenderToExcel(
                //               ExcelSqlHelper.ExecuteDataTable(CommandType.Text, "select idcard [会员卡号], name [姓名],phone [手机],email [邮件],imprest [预付款],Integral [积分],regidate [注册时间] from b2b_crm where com_id=" + comid),
                //               Context, companyName + "会员信息" + ".xls");

                DataTable crmdt = ExcelSqlHelper.ExecuteDataTable(CommandType.Text, "select idcard [会员卡号], name [姓名],phone [手机],email [邮件],imprest [预付款],Integral [积分],regidate [注册时间] from b2b_crm where com_id=" + comid);

                DataTable dt = new DataTable();
                dt.Columns.Add("会员卡号");
                dt.Columns.Add("姓名");
                dt.Columns.Add("手机");
                dt.Columns.Add("邮件");
                dt.Columns.Add("预付款");
                dt.Columns.Add("积分");
                dt.Columns.Add("注册时间");

                dt.Columns.Add("渠道单位");
                dt.Columns.Add("推荐人");
                DataRow dr = dt.NewRow();
                foreach (DataRow rr in crmdt.Rows)
                {
                    dr = dt.NewRow();
                    dr["会员卡号"] = rr["会员卡号"].ToString();
                    dr["姓名"] = rr["姓名"].ToString();
                    dr["手机"] = rr["手机"].ToString();
                    dr["邮件"] = rr["邮件"].ToString();
                    dr["预付款"] = rr["预付款"].ToString();
                    dr["积分"] = rr["积分"].ToString();
                    dr["注册时间"] = rr["注册时间"].ToString();
                    dr["渠道单位"] = new MemberChannelcompanyData().UpCompanyById(rr["会员卡号"].ToString());
                    dr["推荐人"] = MemberChannelData.SearchNamestring(rr["会员卡号"].ToString());

                    dt.Rows.Add(dr);
                }

                ExcelRender.RenderToExcel(dt, Context, companyName + "会员信息" + ".xls");
            }
        }
    }
}