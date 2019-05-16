<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/ui/etown.master" CodeBehind="PosVersionList.aspx.cs"
    Inherits="ETS2.WebApp.UI.PosUI.PosVersionList" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="head">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数

        $(function () {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();
            SearchList(1);

            //装载产品列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/PosVersionHandler.ashx?oper=posversionpagelist",
                    data: {pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询产品列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                $("#tblist").html("查询数据为空");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pageSize, pageindex);
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
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="body">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="usermanage.html" onfocus="this.blur()" target="">Pos版本记录列表</a></li>
                <li><a href="useradd.html" target="" title="">添加Pos版本记录</a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone  mail-list">
            <div class="inner">
                <h3>
                    Pos版本管理</h3>
                <div>
                </div>
                <p>
                    &nbsp;</p>
                <table width="780" border="0">
                    <tr>
                        <td width="33" bgcolor="#CCCCCC">
                            PosID
                        </td>
                        <td width="232" bgcolor="#CCCCCC">
                            Pos最新版本
                        </td>
                        <td width="59" bgcolor="#CCCCCC">
                            最近执行的操作
                        </td>
                        <td width="60" bgcolor="#CCCCCC">
                            &nbsp;
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                </table>
                <div id="divPage">
                </div>
                <p>
                </p>
                <div>
                </div>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
        <tr>
                        <td>
                           ${posid}
                        </td>
                        <td>
                             ${versionno}
                        </td>
                        <td>
                             ${updatetype}
                        </td>
                        <td>
                            <a href="PosVersionHandle.aspx?posid=${posid}">管理</a>
                        </td>
                    </tr>
    </script>
</asp:Content>
