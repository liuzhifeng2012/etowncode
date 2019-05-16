<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Etown.Master" AutoEventWireup="true" CodeBehind="ChannelYK.aspx.cs" Inherits="ETS2.WebApp.UI.ChannelYK" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();
            SearchList1(1);

            //装载产品列表
            function SearchList1(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/ChannelHandler.ashx?oper=ChannelYk",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("渠道统计列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist1").empty();
                            $("#divPage1").empty();
                            if (data.totalCount == 0) {
                                $("#tblist1").html("<tr><td colspan='15'>查询数据为空</td></tr>");
                            } else {
                                $("#ProductItemEdit1").tmpl(data.msg).appendTo("#tblist1");
                                setpage1(data.totalCount, pageSize, pageindex);
                            }


                        }
                    }
                })
            }
            //分页
            function setpage1(newcount, newpagesize, curpage) {
                $("#divPage1").paginate({
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

                        SearchList1(page);

                        return false;
                    }
                });
            }
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
  <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="ChannelList.aspx" onfocus="this.blur()"><span>渠道发行人管理</span></a></li>
                <li><a href="ChannelEdit.aspx" onfocus="this.blur()">添加实体卡发行渠道</a></li>
                <li><a href="Channelstatistics.aspx" onfocus="this.blur()">渠道统计</a></li>
                <li  class="on"><a href="ChannelYK.aspx" onfocus="this.blur()">验卡统计</a></li>
            </ul>
        </div>
        <form id="commonForm" action="Checkcode2.html" method="get">
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    <div style="text-align: center;">
                     
                </div>
                </h3>
                <table width="780" border="0">
                    <tr>
                        <td width="141">
                            <p align="left">
                                渠道单位名称</p>
                        </td>
                        <%--<td width="48">验卡数量</td>--%>
                        <td width="48">
                            验卡量
                        </td>
                    </tr>
                    <tbody id="tblist1">
                    </tbody>
                </table>
                <div id="divPage1">
                </div>
            </div>
        </div>
        </form>
    </div>
    <div class="data1">
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit1">
      <tr>
                        <td width="141">
                            <p align="left" >
                            ${Companyname}
                             </p>
                        </td>
                         <td width="48">
                           ${Yknum}
                        </td>
                    </tr>
                    
    </script>
</asp:Content>
