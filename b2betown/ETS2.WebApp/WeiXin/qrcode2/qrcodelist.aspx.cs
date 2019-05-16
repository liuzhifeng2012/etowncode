using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using System.Data;
using ETS2.Common.Business;
using ETS2.Member.Service.MemberService.Data;
using ETS2.Member.Service.MemberService.Model;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.Common.Business;
namespace ETS2.WebApp.WeiXin.qrcode2
{
    public partial class qrcodelist : System.Web.UI.Page
    {
        GridView gw;
        string termids;

        public string isqudao = "1";//1.渠道单位；2.活动
        public string isrun = "0,1";//二维码运行状态:1.运行;0.暂停

        public int comid = UserHelper.CurrentCompany.ID;//公司id

        public int isrenzheng = 1;//1已认证；0未认证
        protected void Page_Load(object sender, EventArgs e)
        {
            isqudao = Request["isqudao"].ConvertTo<string>("2");
            isrun = Request["isrun"].ConvertTo<string>("0,1");
            if (!IsPostBack)
            {
                hid_key.Value = "";

                GridView2.DataSource = GetAllTerms(isqudao);//获得 渠道单位/活动 列表
                GridView2.DataBind();

                if (UserHelper.ValidateLogin())
                {
                    int comid = UserHelper.CurrentCompany.ID;
                    WeiXinBasic wxbasic = new WeiXinBasicData().GetWxBasicByComId(comid);
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
        }


        private DataSet GetAllTerms(string isqudao, string key = "")
        {
            try
            {
                if (isqudao == "1")//渠道单位列表
                {
                    if (key == "")
                    {
                        return ExcelSqlHelper.ExecuteDataset("select issuetype, id,companyname as title,companystate from Member_Channel_company where com_id=" + comid + " and companystate in (" + isrun + ") order by issuetype,id desc");

                    }
                    else
                    {
                        return ExcelSqlHelper.ExecuteDataset("select  issuetype,id,companyname as title,companystate from Member_Channel_company where com_id=" + comid + " and companystate in (" + isrun + ") and (companyname like '%" + key + "%' or id in (select companyid from member_channel where name='" + key + "' or mobile='" + key + "' )) order by issuetype,id desc");
                    }
                }
                else //活动列表
                {
                    if (key == "")
                    {
                        return ExcelSqlHelper.ExecuteDataset("select  0 as issuetype, id,title,runstate as companystate  from Member_Activity where runstate in (" + isrun + ") and com_id=" + comid + " order by id desc");
                    }
                    else
                    {
                        return ExcelSqlHelper.ExecuteDataset("select 0 as issuetype, id,title,runstate as companystate   from Member_Activity where runstate in (" + isrun + ") and com_id=" + comid + " and  title like '%" + key + "%' order by id desc");

                    }
                }

            }
            catch
            {
                return null;
            }
        }
        private DataSet getammebytermid(string termid)
        {
            try
            {
                if (isqudao == "1")
                {
                    return ExcelSqlHelper.ExecuteDataset("select id,channelcompanyid,activityid,comid,title,qrcodeurl,createtime,onlinestatus,channelid from  WxSubscribeSource where comid=" + comid + " and  channelcompanyid=" + termid + "   order by id desc");
                }
                {
                    return ExcelSqlHelper.ExecuteDataset("select id,channelcompanyid,activityid,comid,title,qrcodeurl,createtime,onlinestatus,channelid from  WxSubscribeSource where comid=" + comid + " and  activityid=" + termid + "  order by id desc");

                }
                //return null;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获得活动名称
        /// </summary>
        /// <param name="channelunitid"></param>
        /// <returns></returns>
        public string GetActivityNameById(string activityid)
        {
            if (activityid == "0" || activityid == "")
            {
                return "";
            }
            else
            {
                Member_Activity model = new MemberActivityData().GetMemberActivityById(int.Parse(activityid));
                if (model == null)
                {
                    return "";
                }
                else
                {
                    return model.Title;
                }
            }
        }
        public string Runstatus(string status)
        {
            if (status == "1"||status.ToLower()=="true")
            {
                return "运行";
            }
            else
            {
                return "暂停";
            }
        }
        /// <summary>
        /// 获得渠道名称
        /// </summary>
        /// <param name="channelunitid"></param>
        /// <returns></returns>
        public string GetChannelNameById(string channelid)
        {
            if (channelid == "0" || channelid == "")
            {
                return "";
            }
            else
            {
                Member_Channel channel = new MemberChannelData().GetChannelDetail(int.Parse(channelid));
                if (channel == null)
                {
                    return "";
                }
                else
                {
                    return channel.Name;
                }
            }
        }
        /// <summary>
        /// 获得渠道单位名称
        /// </summary>
        /// <param name="channelunitid"></param>
        /// <returns></returns>
        public string GetChannelUnitNameById(string channelunitid)
        {
            if (channelunitid == "0" || channelunitid == "")
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
        /// 获得当前 公司/活动 下所有二维码取消关注微信总数
        /// </summary>
        /// <param name="wxsourceid"></param>
        /// <returns></returns>
        public int GetQxWxTotal2(string id, string isqudao)
        {
            return new WxSubscribeDetailData().GetQxWxTotal2(id, isqudao);
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
        /// 获得当前二维码关注微信总数
        /// </summary>
        /// <param name="wxsourceid"></param>
        /// <returns></returns>
        public int GetWxTotal(string wxsourceid)
        {
            return new WxSubscribeDetailData().GetSubScribeTotalCountBySubScribeId(int.Parse(wxsourceid));
        }
        /// <summary>
        /// 获得当前二维码取消关注微信总数
        /// </summary>
        /// <param name="wxsourceid"></param>
        /// <returns></returns>
        public int GetQxWxTotal(string wxsourceid)
        {
            return new WxSubscribeDetailData().GetQxWxTotal(int.Parse(wxsourceid));
        }

        /// <summary>
        /// 获得当前二维码扫码总数
        /// </summary>
        /// <param name="wxsourceid"></param>
        /// <returns></returns>
        public int GetScanTotal(string wxsourceid)
        {
            return new WxSubscribeDetailData().GetScanTotalCount(int.Parse(wxsourceid));
        }
        public string GetLastScanTime(string wxsourceid)
        {
            return new WxSubscribeDetailData().GetLasterScanTime(int.Parse(wxsourceid));
        }
        public string Onlinestatus(string onlinestatus)
        {
            if (onlinestatus == "True")
            {
                return "运行";
            }
            else
            {
                return "暂停";
            }
        }




        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //*******关键代码*******//
                gw = (GridView)e.Row.FindControl("GridView3");
                termids = ((HiddenField)e.Row.FindControl("HiddenField1")).Value;

                ((GridView)e.Row.FindControl("GridView3")).DataSource = getammebytermid(((HiddenField)e.Row.FindControl("HiddenField1")).Value);
                ((GridView)e.Row.FindControl("GridView3")).DataBind();

                e.Row.Attributes.Add("onmouseover", "e=this.style.backgroundColor; this.style.backgroundColor='#FCEEF1';");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=e");
            }
        }
        public string zjls()
        {
            return GetAllTerms(isqudao, hid_key.Value).Tables[0].Rows.Count.ToString();
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
            GridView2.DataSource = GetAllTerms(isqudao, hid_key.Value);
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

                GridView2.DataSource = GetAllTerms(isqudao, key);//获得 渠道单位/活动 列表
                GridView2.DataBind();
            }


        }

    }
}