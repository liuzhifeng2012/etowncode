<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Etown.Master" AutoEventWireup="true"
    CodeBehind="Member_Activity_LogList.aspx.cs" Inherits="ETS2.WebApp.UI.WebForm1" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        var pageSize = 10; //每页显示条数
        $(function () {

            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();

            SearchList(1);

            //活动加载明细列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/BusinessCustomersHandler.ashx?oper=LoadingList",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, userid: userid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询会员数据出现错误！");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
//                                $("#tblist").html("<tr><td colspan='11'>没有查到会员信息。</td></tr>");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pageSize, pageindex);
                            }


                        }
                    }
                })


            }

            //活动搜索明细列表
            $("#Search").click(function () {
                var pageindex = 1;
                var key = $("#key").val();
                var ServerName = $("#ServerName").val();

                if (key == "") {
                    $.prompt("查询关键词不能为空");
                    return;
                }

                $.ajax({
                    type: "post",
                    url: "/JsonFactory/BusinessCustomersHandler.ashx?oper=SearchActivityList",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, key: key, ServerName: ServerName, userid: userid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询会员数据出现错误！");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
//                                $("#tblist").html("<tr><td colspan='11'>没有查到会员信息。</td></tr>");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pageSize, pageindex);
                            }


                        }
                    }
                })


            })


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
                <%--<li><a href="BusinessCustomersList.aspx" onfocus="this.blur()">客户列表</a></li>
                 <li><a href="#" onfocus="this.blur()" target="right-main">导出EXCEL </a></li> 
                <li class="on"><a href="Member_Activity_LogList.aspx" onfocus="this.blur()">验证列表</a></li>
               <li><a href="/v/card.aspx" onfocus="this.blur()" target="_blank">添加会员</a></li>--%>
                <li><a href="/V/VerCard.aspx" onfocus="this.blur()" target=""><span>
                    会员卡验证</span></a></li>
                <li><a href="/v/Member_yufukuan_LogList.aspx" onfocus="this.blur()" target="">
                    <span>会员预付款验证明细</span></a></li>
                <li><a href="/v/Member_jifen_LogList.aspx" onfocus="this.blur()" target="">
                    <span>会员积分验证明细</span></a></li>
                <li class="on"><a href="/ui/crmui/Member_Activity_LogList.aspx" onfocus="this.blur()"
                    target=""><span>验证明细</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                </h3>
                <div style="text-align: center;">
                    <label>
                        请输入(卡号，订单编号，服务专员)
                        <input name="key" type="text" id="key" style="width: 160px; height: 20px;">
                    </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询会员" style="width: 120px;
                            height: 26px;">
                    </label>
                </div>
                <table width="780" border="0">
                    <tr>
                        <td width="4%" height="42">
                            使用ID
                        </td>
                        <td width="12%">
                            卡号
                        </td>
                        <td width="8%">
                            会员姓名
                        </td>
                        <td width="14%">
                            活动
                        </td>
                        <td width="8%">
                            订单编号
                        </td>
                        <td width="10%">
                            服务
                        </td>
                        <td width="8%">
                            渠道单位
                        </td>
                        <td width="6%">
                            服务专员
                        </td>
                        <td width="6%">
                            消费人数
                        </td>
                        <td width="8%">
                            订单总金额
                        </td>
                        <td width="6%">
                            会员返利
                        </td>
                        <td width="12%">
                            使用时间&nbsp;
                        </td>
                        <%--<td width="12%">
                            管理
                        </td>--%>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr>
                        <td >
                            <p align="left">
                                ${ID}</p>
                        </td>
                        <td>
                            ${CardID}
                        </td>
                        <td>${username}</td>
                        <td>
                                ${ACTID}
                        </td>
                        <td>
                            ${OrderId} 
                        </td>
                        <td>
                            ${ServerName} 
                        </td>
                        <td>
                            ${channel}
                        </td>
                        <td >
                                ${Sales_admin}
                        </td>
                        <td >
                               ${Num_people}
                        </td>
                        <td>
                              ${Per_capita_money}
                        </td>
                        <td>
                              ${Member_return_money}
                        </td>
                        <td >
                                ${jsonDateFormatKaler(Usesubdate)}
                        </td>

                    </tr>
                    </script>
                </table>
                <div id="divPage">
                </div>
            </div>
        </div>
    </div>
    <%--<td>
                               <a href="Member_Activity_Manage.aspx?id=${ID}">查看</a>
                       </td>--%>
    <div class="data">
    </div>
</asp:Content>
