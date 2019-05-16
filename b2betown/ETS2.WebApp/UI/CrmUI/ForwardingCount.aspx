<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ForwardingCount.aspx.cs"
    Inherits="ETS2.WebApp.UI.CrmUI.ForwardingCount" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        var pageSize = 20; //每页显示条数
        $(function () {

            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();

            SearchList(1);

            //验票明细列表
            function SearchList(pageindex, promotetypeid) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/WeiXinHandler.ashx?oper=forwardingpagelist",
                    data: { comid: $("#hid_comid").trimVal(), pageindex: pageindex, pagesize: pageSize, applystate: 10, promotetypeid: promotetypeid ,wxid:<%=wxid %>},
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询微信素材列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                $("#tblist").html("<tr><td height=\"26\" colspan=\"7\">查询数据为空</td></tr>");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pageSize, pageindex, promotetypeid);
                            }

                        }
                    }
                })


            }



            //分页
            function setpage(newcount, newpagesize, curpage) {
                $("#divPage").paginate({
                    count: Math.ceil(newcount / newpagesize),
                    start: curpage,
                    display: 5,
                    border: false,
                    text_color: '#888',
                    background_color: '#EEE',
                    text_hover_color: 'black',
                    images: false,
                    rotate: false,
                    mouse: 'press',
                    onChange: function (page) {

                        SearchList(page);

                        return false;
                    }
                });
            }
        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="BusinessCustomersList.aspx" onfocus="this.blur()" target="right-main">
                    客户列表</a></li>
                <%-- <li><a href="#" onfocus="this.blur()" target="right-main">导出EXCEL </a></li>--%>
                <li><a href="Member_Activity_LogList.aspx" onfocus="this.blur()">验证列表</a></li>
               <%-- <li><a href="/v/card.aspx" onfocus="this.blur()" target="_blank">添加会员</a></li>--%>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    活动转发统计</h3>  <a href="javascript:history.go(-1);">返回>></a>

                <table width="780" border="0">
                    <tr>
                        <td width="10%" height="42">
                        活动ID
                        </td>
                        <td width="30%">
                        活动名称
                        </td>
                        <td width="10%">
                            转发账户
                        </td>
                        <td width="10%">
                            卡号
                        </td>
                        <td width="10%">
                            访问数量
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
<tr>
                        <td class="sender item">
                            <p align="left">
                                ${MaterialId}</p>
                        </td>
                        <td height="26" class="sender item">
                            <p align="left">
                                ${Title}
                            </p>
                        </td>
                        <td height="26" class="sender item">
                            <p align="left">
                               ${Name}
                            </p>
                        </td>
                        <td height="26" class="sender item">
                            <p align="left">
                             ${Idcard}  </p>
                        </td>

                        <td height="26" class="sender item">
                            <p align="left">
                               ${Fornum} </p>
                        </td>
                    </tr>
                    </script>
                </table>
                <div id="divPage">
                </div>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
</asp:Content>
