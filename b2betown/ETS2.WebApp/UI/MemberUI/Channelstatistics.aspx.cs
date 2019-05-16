using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Modle;
using System.Data;
using System.Data.SqlClient;
using ETS2.Member.Service.MemberService.Data;
using ETS2.Member.Service.MemberService.Model;
using FileUpload.FileUpload.Entities.Enum;

namespace ETS2.WebApp.UI
{
    public partial class Channelstatistics : System.Web.UI.Page
    {

        GridView gw;
        string termids;

        public string channelcompanytype = "inner";//默认为内部渠道
        public string isrun = "1,0";//渠道单位运行状态:1.运行;0.暂停
        public int comid = UserHelper.CurrentCompany.ID;//公司id
        public string comname = UserHelper.CurrentCompany.Com_name;//公司名称


        public bool IsParentCompanyUser = true; //判断操作账户类型(总公司账户;门市账户)
        public string channelcompanyid = "0";//员工所在门市id,默认为0

        public int md_subscribenum = 0;//门店关注总数
        protected void Page_Load(object sender, EventArgs e)
        {
            //图片上传 绑定控件
            headPortrait.uploadFileInfo.ObjType = (int)FileObjType.Photo;
            headPortrait.displayImgId = "headPortraitImg";


            channelcompanytype = Request["channelcompanytype"].ConvertTo<string>("inner");

            int userid = UserHelper.CurrentUserId();
            IsParentCompanyUser = new B2bCompanyManagerUserData().IsParentCompanyUser(userid);
            if (IsParentCompanyUser == false)//如果是门市账户
            {
                B2b_company_manageuser user = B2bCompanyManagerUserData.GetUser(userid);
                if (user != null)
                {
                    channelcompanyid = user.Channelcompanyid.ToString();
                }

                Member_Channel_company ccompay = new MemberChannelcompanyData().GetChannelCompany(channelcompanyid);
                if (ccompay != null)
                {
                    if (ccompay.Issuetype == 0)
                    {
                        channelcompanytype = "inner";
                    }
                    if (ccompay.Issuetype == 1)
                    {
                        channelcompanytype = "out";
                    }
                }
            }

            //根据登录公司显示门店关注总数  
            md_subscribenum = new WxSubscribeDetailData().GetMd_Subscribenum(UserHelper.CurrentCompany.ID);

            isrun = Request["isrun"].ConvertTo<string>("1,0");
            if (!IsPostBack)
            {
                hid_key.Value = "";

                if (channelcompanytype == "inner")//如果是 所属门店/合作单位，则需要把总公司信息加上
                {
                    DataSet ds1 = GetHeadOffice();//得到总公司信息
                    DataSet ds2 = GetAllTerms(channelcompanytype, channelcompanyid);
                    ds1.Merge(ds2, false, MissingSchemaAction.Ignore);
                    GridView2.DataSource = ds1;//获得 渠道单位 列表
                    GridView2.DataBind();
                }
                else
                {
                    GridView2.DataSource = GetAllTerms(channelcompanytype, channelcompanyid);//获得 渠道单位 列表
                    GridView2.DataBind();
                }
            }


        }
        #region  得到总公司信息
        private DataSet GetHeadOffice()
        {
            string sql = "select 0 as  issuetype, '" + comname + "' as companyname, 0 as id,1 as  companystate, COUNT(c.id) AS Expr1,SUM(isnull(c.opencardnum,0))  AS opencardnum, SUM(isnull(c.firstdealnum,0)) AS firstdealnum, SUM(isnull(c.summoney,0)) " +
                        "  AS summoney,c.Com_id,0 AS Expr2,COUNT(d.ID) AS companynum  " +
                        "  from Member_Channel as  c  left join   " +
                        "  dbo.Member_Activity_Log AS d ON d.sales_admin = c.name where c.id in (" +
                        "  select id from Member_Channel where Com_id=" + comid + " and   mobile in (select tel from b2b_company_manageuser where com_id=" + comid + " and channelcompanyid=0))" +
                        "   group by c.Com_id";
            DataSet ds = ExcelSqlHelper.ExecuteDataset(sql);
            return ds;
        }
        #endregion
        #region 绑定GridView2
        private DataSet GetAllTerms(string channelcompanytype, string channelcompanyid, string key = "")
        {
            try
            {
                if (channelcompanyid != "0")//显示特定渠道单位信息
                {
                    string sql = @"SELECT    a.issuetype, a.companyname, a.id,a.companystate, COUNT(c.id) AS Expr1, SUM(isnull(c.opencardnum,0))  AS opencardnum, SUM(isnull(c.firstdealnum,0)) AS firstdealnum, SUM(isnull(c.summoney,0))  " +
                                    " AS summoney, a.Com_id, a.id AS Expr2,COUNT(d.ID) AS companynum " +
                                    " FROM         dbo.Member_Channel_company AS a LEFT OUTER JOIN " +
                                    " dbo.Member_Channel AS c ON a.id = c.companyid left join  " +
                                    " dbo.Member_Activity_Log AS d ON d.sales_admin = c.name " +
                                    " where a.id=" + channelcompanyid +
                                    " GROUP BY a.issuetype,a.companyname, a.Com_id, a.id,a.companystate";
                    if (key != "")
                    {
                        sql = "SELECT    a.issuetype, a.companyname, a.id,a.companystate, COUNT(c.id) AS Expr1, SUM(isnull(c.opencardnum,0))  AS opencardnum, SUM(isnull(c.firstdealnum,0)) AS firstdealnum, SUM(isnull(c.summoney,0)) " +
                                  " AS summoney, a.Com_id, a.id AS Expr2,COUNT(d.ID) AS companynum " +
                                  " FROM         dbo.Member_Channel_company AS a LEFT OUTER JOIN " +
                                  " dbo.Member_Channel AS c ON a.id = c.companyid left join  " +
                                  " dbo.Member_Activity_Log AS d ON d.sales_admin = c.name " +
                                  " where a.id=" + channelcompanyid + "  and (a.companyname like '%" + key + "%'" + " or a.id in (select companyid from Member_Channel where name ='" + key + "' or mobile='" + key + "'))" +
                                  " GROUP BY a.issuetype,a.companyname, a.Com_id, a.id,a.companystate";
                    }


                    //SqlParameter[] para = { new SqlParameter("@channelcompanyid", SqlDbType.VarChar, 255) };
                    //para[0].Value = channelcompanyid;

                    DataSet ds = ExcelSqlHelper.ExecuteDataset(sql);
                    return ds;
                }
                else //显示渠道列表
                {
                    if (channelcompanytype == "inner")//所属门店
                    {

                        string sql = "SELECT    a.issuetype, a.companyname, a.id,a.companystate, COUNT(c.id) AS Expr1, SUM(isnull(c.opencardnum,0))  AS opencardnum, SUM(isnull(c.firstdealnum,0)) AS firstdealnum, SUM(isnull(c.summoney,0))  " +
                        " AS summoney, a.Com_id, a.id AS Expr2,COUNT(d.ID) AS companynum " +
                        " FROM         dbo.Member_Channel_company AS a LEFT OUTER JOIN " +
                        " dbo.Member_Channel AS c ON a.id = c.companyid left join  " +
                        " dbo.Member_Activity_Log AS d ON d.sales_admin = c.name " +
                        "  where a.Issuetype=0 and a.com_id=" + comid + " and a.companystate in (" + isrun + ")" +//最后 and a.id=0默认不显示
                        " GROUP BY a.issuetype,a.companyname, a.Com_id, a.id,a.companystate";
                        if (key != "")
                        {
                            sql = "SELECT     a.issuetype,a.companyname, a.id,a.companystate, COUNT(c.id) AS Expr1, SUM(isnull(c.opencardnum,0))  AS opencardnum, SUM(isnull(c.firstdealnum,0)) AS firstdealnum, SUM(isnull(c.summoney,0))  " +
                                    " AS summoney, a.Com_id, a.id AS Expr2,COUNT(d.ID) AS companynum " +
                                    " FROM         dbo.Member_Channel_company AS a LEFT OUTER JOIN " +
                                    " dbo.Member_Channel AS c ON a.id = c.companyid left join  " +
                                    " dbo.Member_Activity_Log AS d ON d.sales_admin = c.name " +
                                    "  where a.Issuetype=0 and a.com_id=" + comid + " and a.companystate in (" + isrun + ") and (a.companyname like '%" + key + "%'" + " or a.id in (select companyid from Member_Channel where name ='" + key + "' or mobile='" + key + "'))" +
                                    " GROUP BY a.issuetype,a.companyname, a.Com_id, a.id,a.companystate";
                        }

                        //SqlParameter[] para = { new SqlParameter("@comid", SqlDbType.Int, 32), new SqlParameter("@companystate", SqlDbType.VarChar, 255) };
                        //para[0].Value = comid;
                        //para[1].Value = isrun;
                        DataSet ds = ExcelSqlHelper.ExecuteDataset(sql);
                        return ds;
                    }
                    else
                    {
                        string sql = @"SELECT     a.issuetype,a.companyname, a.id,a.companystate, COUNT(c.id) AS Expr1, SUM(isnull(c.opencardnum,0))  AS opencardnum, SUM(isnull(c.firstdealnum,0)) AS firstdealnum, SUM(isnull(c.summoney,0)) " +
                          " AS summoney, a.Com_id, a.id AS Expr2,COUNT(d.ID) AS companynum " +
                          " FROM         dbo.Member_Channel_company AS a LEFT OUTER JOIN " +
                          " dbo.Member_Channel AS c ON a.id = c.companyid left join  " +
                          " dbo.Member_Activity_Log AS d ON d.sales_admin = c.name " +
                         "  where a.Issuetype=1   and a.com_id=" + comid + " and a.companystate in (" + isrun + ") " +
                         "  GROUP BY a.issuetype,a.companyname, a.Com_id, a.id,a.companystate";
                        if (key != "")
                        {
                            sql = @"SELECT    a.issuetype, a.companyname, a.id,a.companystate, COUNT(c.id) AS Expr1, SUM(isnull(c.opencardnum,0))  AS opencardnum, SUM(isnull(c.firstdealnum,0)) AS firstdealnum, SUM(isnull(c.summoney,0)) " +
                            " AS summoney, a.Com_id, a.id AS Expr2,COUNT(d.ID) AS companynum " +
                            " FROM         dbo.Member_Channel_company AS a LEFT OUTER JOIN " +
                            " dbo.Member_Channel AS c ON a.id = c.companyid left join  " +
                            " dbo.Member_Activity_Log AS d ON d.sales_admin = c.name " +
                           "  where a.Issuetype=1   and a.com_id=" + comid + " and a.companystate in (" + isrun + ") and (a.companyname like '%" + key + "%'" + " or a.id in (select companyid from Member_Channel where name ='" + key + "' or mobile='" + key + "'))" +
                           "  GROUP BY a.issuetype,a.companyname, a.Com_id, a.id,a.companystate";
                        }
                        //SqlParameter[] para = { new SqlParameter("@comid", SqlDbType.Int, 32), new SqlParameter("@companystate", SqlDbType.VarChar, 255) };
                        //para[0].Value = comid;
                        //para[1].Value = isrun;
                        DataSet ds = ExcelSqlHelper.ExecuteDataset(sql);
                        return ds;
                    }

                }

            }
            catch
            {
                return null;
            }
        }
        #endregion
        #region 绑定GridView3
        private DataSet getammebytermid(string termid, string key = "")//渠道单位下边的渠道人列表
        {
            try
            {
                string sql = "SELECT [id] ,[Com_id] ,[Issuetype],[companyid] ,[name],[mobile],[cardcode] ,[Chaddress] ,[ChObjects] ,[RebateOpen],[RebateConsume] ,[RebateConsume2] ,[rebatelevel],[opencardnum],[firstdealnum],[summoney],[whetherdefaultchannel] ,[runstate] FROM [EtownDB].[dbo].[Member_Channel]  where companyid in (" + termid + ") and com_id=" + comid;
                if (key != "")
                {
                    //sql += " and ( name ='"+ key +"' or mobile ='"+key+"')";
                }
                if (termid == "0")//如果渠道公司id为0，则为总部渠道
                {
                    sql = "SELECT [id] ,[Com_id] ,[Issuetype],[companyid] ,[name],[mobile],[cardcode] ,[Chaddress] ,[ChObjects] ,[RebateOpen],[RebateConsume] ,[RebateConsume2] ,[rebatelevel],[opencardnum],[firstdealnum],[summoney],[whetherdefaultchannel] ,[runstate] FROM [EtownDB].[dbo].[Member_Channel]  where companyid =0 and Issuetype=0 and com_id=" + comid;
                }

                DataSet ds = ExcelSqlHelper.ExecuteDataset(sql);
                return ds;
            }
            catch
            {
                return null;
            }
        }
        #endregion
        #region 自定义函数 改变绑定字段

        public string GetEnteredNumberByChannelId(string id)
        {
            int EnterCardNum = new MemberCardData().GetEnteredNumberByChannelId(int.Parse(id));
            return EnterCardNum.ToString();
        }


        public string GetTypeName(string Issuetype)
        {
            if (Issuetype == "0")
            {
                return "内部渠道";
            }
            else if (Issuetype == "1")
            {
                return "外部渠道";
            }
            else if (Issuetype == "3")
            {
                return "网站注册";
            }
            else
            {
                return "微信注册";
            }

        }
        /// <summary>
        /// 获得渠道单位名称
        /// </summary>
        /// <param name="channelunitid"></param>
        /// <returns></returns>
        public string GetChannelUnitNameById(string channelunitid)
        {
            if (channelunitid == "0")
            {
                return "";
            }
            else
            {
                Member_Channel_company channelunit = new MemberChannelData().GetChannelCompanyById(int.Parse(channelunitid));
                if (channelunitid == null)
                {
                    return "";
                }
                else
                {
                    return channelunit.Companyname;
                }
            }
        }

        public string Runstatus(string status)
        {
            if (status == "1")
            {
                return "运行";
            }
            else
            {
                return "暂停";
            }
        }
        /// <summary>
        /// 获得当前 公司/活动 下所有二维码关注微信总数
        /// </summary>
        /// <param name="wxsourceid"></param>
        /// <returns></returns>
        public int GetWxTotal2(string id, string isqudao, string issuetype = "0,1")
        {
            return new WxSubscribeDetailData().GetWxTotal2(id, isqudao, issuetype);
        }
        /// <summary>
        /// 获得当前 公司/活动 下所有二维码扫码总数
        /// </summary>
        /// <param name="wxsourceid"></param>
        /// <returns></returns>
        public int GetScanTotal2(string id, string isqudao)
        {
            return new WxSubscribeDetailData().GetScanTotal2(id, isqudao);
        }

        /// <summary>
        /// 获得当前 公司/活动 下所有二维码取消关注微信总数
        /// </summary>
        /// <param name="wxsourceid"></param>
        /// <returns></returns>
        public int GetQxWxTotal2(string id, string isqudao)
        {
            return new WxSubscribeDetailData().GetQxWxTotal2(id, isqudao);
        }
        #endregion



        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //*******关键代码*******//
                gw = (GridView)e.Row.FindControl("GridView3");
                termids = ((HiddenField)e.Row.FindControl("HiddenField1")).Value;

                ((GridView)e.Row.FindControl("GridView3")).DataSource = getammebytermid(((HiddenField)e.Row.FindControl("HiddenField1")).Value, hid_key.Value);
                ((GridView)e.Row.FindControl("GridView3")).DataBind();

                e.Row.Attributes.Add("onmouseover", "e=this.style.backgroundColor; this.style.backgroundColor='#FCEEF1';");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=e");
            }
        }
        public string zjls()
        {
            return GetAllTerms(channelcompanytype, channelcompanyid, hid_key.Value).Tables[0].Rows.Count.ToString();
        }
        //GRIDVIEW的自定义分页
        public string ts()
        {
            string fre = "";
            if (GridView2.PageCount == 1)
            {
                fre += "第<font color=\"red\">1</font>-<font color=\"red\">" + GridView2.Rows.Count.ToString() + "</font>条";
                return fre;
            }
            if ((GridView2.PageIndex + 1) == GridView2.PageCount)//最后页的情况
            {
                fre = "第<font color=\"red\">" + (GridView2.PageIndex * GridView2.PageSize + 1).ToString() + "</font>-<font color=\"red\">" + (GridView2.PageIndex * GridView2.PageSize + GridView2.Rows.Count).ToString() + "</font>条";
                return fre;
            }
            fre = "第<font color=\"red\">" + (GridView2.PageIndex * GridView2.PageSize + 1).ToString() + "</font>-<font color=\"red\">" + (GridView2.PageIndex * GridView2.PageSize + GridView2.PageSize).ToString() + "</font>条";


            return fre;
        }
        /// <summary>
        /// gridview3 分页相关
        /// </summary>
        /// <returns></returns>
        public string zjls1(string termid)
        {
            return getammebytermid(termid).Tables[0].Rows.Count.ToString();
        }
        public string ts1(GridView GridView2)
        {
            string fre = "";
            if (GridView2.PageCount == 1)
            {
                fre += "第<font color=\"red\">1</font>-<font color=\"red\">" + GridView2.Rows.Count.ToString() + "</font>条";
                return fre;
            }
            if ((GridView2.PageIndex + 1) == GridView2.PageCount)//最后页的情况
            {
                fre = "第<font color=\"red\">" + (GridView2.PageIndex * GridView2.PageSize + 1).ToString() + "</font>-<font color=\"red\">" + (GridView2.PageIndex * GridView2.PageSize + GridView2.Rows.Count).ToString() + "</font>条";
                return fre;
            }
            fre = "第<font color=\"red\">" + (GridView2.PageIndex * GridView2.PageSize + 1).ToString() + "</font>-<font color=\"red\">" + (GridView2.PageIndex * GridView2.PageSize + GridView2.PageSize).ToString() + "</font>条";

            return fre;
        }
        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // 得到该控件
            GridView theGrid = sender as GridView;
            int newPageIndex = 0;
            if (e.NewPageIndex == -3)
            {
                //点击了Go按钮
                TextBox txtNewPageIndex = null;

                //GridView较DataGrid提供了更多的API，获取分页块可以使用BottomPagerRow 或者TopPagerRow，当然还增加了HeaderRow和FooterRow
                GridViewRow pagerRow = theGrid.BottomPagerRow;

                if (pagerRow != null)
                {
                    //得到text控件
                    txtNewPageIndex = pagerRow.FindControl("txtNewPageIndex") as TextBox;
                }
                if (txtNewPageIndex != null)
                {
                    //得到索引
                    newPageIndex = int.Parse(txtNewPageIndex.Text) - 1;
                }
            }
            else
            {
                //点击了其他的按钮
                newPageIndex = e.NewPageIndex;
            }
            //防止新索引溢出
            newPageIndex = newPageIndex < 0 ? 0 : newPageIndex;
            newPageIndex = newPageIndex >= theGrid.PageCount ? theGrid.PageCount - 1 : newPageIndex;

            //得到新的值
            theGrid.PageIndex = newPageIndex;

            //重新绑定
            GridView2.DataSource = GetAllTerms(channelcompanytype, channelcompanyid, hid_key.Value);
            GridView2.DataBind();
        }
        protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //gridview3的自定义分页绑定
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                ((Label)e.Row.FindControl("Label3")).Text = ts1(gw);
                ((Label)e.Row.FindControl("Label5")).Text = zjls1(termids);
                ((HiddenField)e.Row.FindControl("bb")).Value = termids;
            }
        }
        protected void GridView3_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //*******关键代码*******//
            GridView theGrid = (GridView)sender;
            gw = theGrid;
            termids = ((HiddenField)theGrid.BottomPagerRow.FindControl("bb")).Value;


            // 得到该控件
            //GridView theGrid = sender as GridView;
            int newPageIndex = 0;
            if (e.NewPageIndex == -3)
            {
                //点击了Go按钮
                TextBox txtNewPageIndex = null;

                //GridView较DataGrid提供了更多的API，获取分页块可以使用BottomPagerRow 或者TopPagerRow，当然还增加了HeaderRow和FooterRow
                GridViewRow pagerRow = theGrid.BottomPagerRow;

                if (pagerRow != null)
                {
                    //得到text控件
                    txtNewPageIndex = pagerRow.FindControl("txtNewPageIndex") as TextBox;
                }
                if (txtNewPageIndex != null)
                {
                    //得到索引
                    newPageIndex = int.Parse(txtNewPageIndex.Text) - 1;
                }
            }
            else
            {
                //点击了其他的按钮
                newPageIndex = e.NewPageIndex;
            }
            //防止新索引溢出
            newPageIndex = newPageIndex < 0 ? 0 : newPageIndex;
            newPageIndex = newPageIndex >= theGrid.PageCount ? theGrid.PageCount - 1 : newPageIndex;

            //得到新的值
            theGrid.PageIndex = newPageIndex;


            theGrid.DataSource = getammebytermid(termids);
            theGrid.DataBind();
        }
        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btnsearch_Click(object sender, EventArgs e)
        {
            string key = txtkey.Text.Trim();
            if (key != "")
            {
                hid_key.Value = key;

                GridView2.DataSource = GetAllTerms(channelcompanytype, channelcompanyid, key);//获得 渠道单位/活动 列表
                GridView2.DataBind();
            }


        }
    }
}