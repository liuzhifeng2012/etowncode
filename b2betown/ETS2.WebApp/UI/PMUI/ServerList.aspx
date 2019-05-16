<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ServerList.aspx.cs"
    Inherits="ETS2.WebApp.UI.PMUI.ServerList" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">


        $(function () {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();

            SearchList(1, 50);
            var pagesize = 50;
            function SearchList(pageindex, pagesize) {

                $.post("/JsonFactory/ProductHandler.ashx?oper=Rentserverpagelist", { comid: comid, pageindex: pageindex, pagesize: pagesize }, function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        //                        $.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $("#tblist").empty();
                        $("#divPage").empty();
                        if (data.totalcount == 0) {
                            //                                $("#tblist").html("查询数据为空");
                        } else {
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                            setpage(data.totalcount, pagesize, pageindex);
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

                        SearchList(page, newpagesize);

                        return false;
                    }
                });
            }



        })


        function deltmp(tmpid, tmpname) {
            if (confirm("确认删除" + tmpname + "吗?")) {
                if (tmpid == 0) {
                    alert("删除失败");
                    return;
                } else {
                    $.post("/JsonFactory/ProductHandler.ashx?oper=delRentserver", { id: tmpid, comid: $("#hid_comid").trimVal() }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            alert(data.msg);
                            return;
                        }
                        if (data.type == 100) {
                            alert("删除成功");
                            window.location.reload();
                            return;
                        }
                    });
                }
            }
        }

    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div class="navsetting ">
            <ul class="composetab">
                 <li class="on" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/serverlist.aspx">终端服务管理</a></div>
                    </div>
                </li>
               <%-- <li style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/ServerPrintSuodao.aspx">索道票打印统计</a></div>
                    </div>
                </li>--%>
                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/ServerFakaStat.aspx">发卡记录</a></div>
                    </div>
                </li>
                <li   style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/ServerxiaojianCount.aspx">小件统计</a></div>
                    </div>
                </li>
<%--                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/ServerTimeoutStat.aspx">归还超时统计</a></div>
                    </div>
                </li>--%>
              <%--  <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_sel">
                        <div>
                            <a href="/ui/pmui/servercardinputlist.aspx">服务卡管理</a></div>
                    </div>
                </li>
                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/servercardinput.aspx">录入服务卡</a></div>
                    </div>
                </li>--%>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    内部服务列表</h3>
                <h4 style="float: right">
                    <a style="" href="ServerCardInput.aspx" class="a_anniu">服务卡片录入</a> <a style="" href="ServerManage.aspx"
                        class="a_anniu">新建服务</a>
                </h4>
                <table width="780" border="0">
                    <tr>
                        <td width="20px">
                            <p align="left">
                                ID</p>
                        </td>
                        <td width="10%">
                            <p align="left">
                                类别
                            </p>
                        </td>
                        <td width="20%">
                            <p align="left">
                                服务名称
                            </p>
                        </td>
                        <td width="10%">
                            <p align="left">
                                售价
                            </p>
                        </td>
                        <td width="10%">
                            <p align="left">
                                押金
                            </p>
                        </td>
                        <td width="15%">
                            <p align="left">
                                是否归还
                            </p>
                        </td>
                        <td width="10%">
                            <p align="left">
                                PosID
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                管理 &nbsp;</p>
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                </table>
                <div id="divPage">
                </div>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr>
                        <td>
                            <p>${id}</p>
                        </td>
                        <td>
                            <p>${renttype}</p>
                        </td>
                        <td>
                            <p>${servername}</p>
                        </td>
                        <td>
                            <p>${saleprice}元</p>
                        </td>
                        <td>
                            <p>${serverDepositprice}元</p>
                        </td>
                           <td>
                            <p>{{if WR==0}}
                                无需归还
                                {{/if}}
                                {{if WR==1}}
                                归还
                                {{/if}}
                            </p>
                        </td>
                         <td>
                            <p> ${posid}
                            </p>
                        </td>
                        <td>
                            <p><a href="ServerManage.aspx?id=${id}" class="a_anniu">管理</a> <br> <a href="javascript:void(0)" onclick="deltmp('${id}','${servername}')" class="a_anniu">删除</a>   </p>
                        </td>
                    </tr>
    </script>
</asp:Content>
