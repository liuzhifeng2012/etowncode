<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="MenuList.aspx.cs"
    Inherits="ETS2.WebApp.WeiXin.MenuList" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function delwxmenu(menuid) {
            if (confirm("确认删除此信息吗？")) {
                $.post("/JsonFactory/WeiXinHandler.ashx?oper=delwxmenu", { wxmenuid: menuid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("删除信息出错" + data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("删除信息成功", {
                            buttons: [{ title: '确定', value: true}],
                            opacity: 0.1,
                            focus: 0,
                            show: 'slideDown',
                            submit: function (e, v, m, f) {
                                if (v == true)
                                    location.href = "menulist.aspx";
                            }
                        });
                    }
                });
            } else {
                alert("你取消了删除操作");
            }
        }
        //产品详情展示
        function tab_select(li) {

            if ($(".tab_sun_" + li + "").css("display") == "none") {

                $(".tab_sun_" + li + "").css('display', '');
                $("#tab_sun_" + li + "_biao").text(' ▲');
            } else {

                $(".tab_sun_" + li + "").css('display', 'none');
                $("#tab_sun_" + li + "_biao").text(' ▼');
            }

        }
        //创建自定义菜单
        function createmenu() {
            if (confirm("确认创建自定义菜单吗？")) {
                $.post("/JsonFactory/WeiXinHandler.ashx?oper=createwxmenu", { comid: $("#hid_comid").trimVal() }, function (data2) {
                    data2 = eval("(" + data2 + ")");
                    if (data2.type == 1) {
                        $.prompt("创建微信菜单出现问题:" + data2.msg);
                        return;
                    }
                    if (data2.type == 100) {
                        $.prompt(data2.msg);
                        return;
                    }
                });
            }
            else {
                $.prompt("操作已取消");
                return;
            }
        }
    </script>
    <style type="text/css">
        .button.gray
        {
            color: #8c96a0;
            text-shadow: 1px 1px 1px #fff;
            border: 1px solid #dce1e6;
            box-shadow: 0 1px 2px #fff inset,0 -1px 0 #a8abae inset;
            background: -webkit-linear-gradient(top,#f2f3f7,#e4e8ec);
            background: -moz-linear-gradient(top,#f2f3f7,#e4e8ec);
            background: linear-gradient(top,#f2f3f7,#e4e8ec);
        }
        .button
        {
            width: 140px;
            height: 40px;
            line-height: 38px;
            text-align: center;
            font-weight: bold;
            color: #fff;
            text-shadow: 1px 1px 1px #333;
            border-radius: 5px;
            margin: 0 20px 20px 0;
            position: relative;
            overflow: hidden;
        }
        .button.black
        {
            border: 1px solid #333;
            box-shadow: 0 1px 2px #8b8b8b inset,0 -1px 0 #3d3d3d inset,0 -2px 3px #8b8b8b inset;
            background: -webkit-linear-gradient(top,#656565,#4c4c4c);
            background: -moz-linear-gradient(top,#656565,#4a4a4a);
            background: linear-gradient(top,#656565,#4a4a4a);
        }
        .button.blue
        {
            border: 1px solid #1e7db9;
            box-shadow: 0 1px 2px #8fcaee inset,0 -1px 0 #497897 inset,0 -2px 3px #8fcaee inset;
            background: -webkit-linear-gradient(top,#42a4e0,#2e88c0);
            background: -moz-linear-gradient(top,#42a4e0,#2e88c0);
            background: linear-gradient(top,#42a4e0,#2e88c0);
        }
        .blue:hover
        {
            background: -webkit-linear-gradient(top,#70bfef,#4097ce);
            background: -moz-linear-gradient(top,#70bfef,#4097ce);
            background: linear-gradient(top,#70bfef,#4097ce);
        }
        
        .gv
        {
            border: 1px solid #D7D7D7;
            font-size: 12px;
            text-align: center;
        }
        .gvHeader
        {
            color: #3F6293;
            background-color: #F7F7F7;
            height: 24px;
            line-height: 24px;
            text-align: center;
            font-weight: normal;
            font-variant: normal;
        }
        .gvHeader th
        {
            font-weight: normal;
            font-variant: normal;
            min-width: 200px;
            text-align: center;
        }
        .gvRow, .gvAlternatingRow, .gvEditRow
        {
            line-height: 20px;
            text-align: center;
            padding: 2px;
            height: 20px;
        }
        .gvAlternatingRow
        {
            background-color: #F5FBFF;
        }
        .gvEditRow
        {
            background-color: #FAF9DD;
        }
        .gvEditRow input
        {
            background-color: #FFFFFF;
            width: 80px;
        }
        .gvEditRow .gvOrderId input, .gvEditRow .gvOrderId
        {
            width: 30px;
        }
        .gvEditRow .checkBox input, .gvEditRow .checkBox
        {
            width: auto;
        }
        .gvCommandField
        {
            text-align: center;
            width: 130px;
        }
        .gvLeftField
        {
            text-align: left;
            padding-left: 10px;
        }
        .gvBtAField
        {
            text-align: center;
            width: 130px;
        }
        .gvCommandField input
        {
            background-image: url(../Images/gvCommandFieldABg.jpg);
            background-repeat: no-repeat;
            line-height: 23px;
            border-top-style: none;
            border-right-style: none;
            border-bottom-style: none;
            border-left-style: none;
            width: 50px;
            height: 23px;
            margin-right: 3px;
            margin-left: 3px;
            text-indent: 10px;
        }
        .gvPage
        {
            padding-left: 15px;
            font-size: 18px;
            color: #333333;
            font-family: Arial, Helvetica, sans-serif;
        }
        .gvPage a
        {
            display: block;
            text-decoration: none;
            padding-top: 2px;
            padding-right: 5px;
            padding-bottom: 2px;
            padding-left: 5px;
            border: 1px solid #FFFFFF;
            float: left;
            font-size: 12px;
            font-weight: normal;
        }
        .gvPage a:hover
        {
            display: block;
            text-decoration: none;
            border: 1px solid #CCCCCC;
        }
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <%--<div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/weixin/ShangJiaSet2.aspx" onfocus="this.blur()"><span>微信接口设置</span></a></li>
                <li><a href="/weixin/WxDefaultReply.aspx" onfocus="this.blur()"><span>微信默认设置</span></a></li>
                <li class="on"><a href="/weixin/menulist.aspx" onfocus="this.blur()"><span>自定义菜单管理</span></a></li>
                <li><a href="/MemberShipCard/MemberShipCardList.aspx" onfocus="this.blur()"><span>会员卡专区管理</span></a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone mail-list mail-list1">
            <div class="inner">
                <h3>
                    微信菜单设置
                </h3>
                <h3>
                    <label style="float: right">
                        <a href="MenuDetail.aspx" style="color: #2D65AA;">微信主菜单添加</a> <a href="Menusort.aspx?fathermenuid=0"
                            style="color: #2D65AA;">微信主菜单排序</a>
                    </label>
                </h3>
                <asp:Repeater ID="rptTopMenuList" runat="server">
                    <HeaderTemplate>
                        <table border="0" width="780" class="mail-list-title">
                            <tr>
                                <td width="6%" align="center" bgcolor="#CCCCCC">
                                    菜单id
                                </td>
                                <td width="10%" height="26" align="left" bgcolor="#CCCCCC">
                                    <p align="left">
                                        菜单标题
                                    </p>
                                </td>
                                <td width="25%" height="26" bgcolor="#CCCCCC">
                                    <p align="center">
                                        主菜单标题
                                    </p>
                                </td>
                                <td width="5%" height="26" align="left" bgcolor="#CCCCCC">
                                    <p align="center">
                                        菜单级别
                                    </p>
                                </td>
                                <td width="20%" height="26" bgcolor="#CCCCCC">
                                    <p align="center">
                                        菜单类型
                                    </p>
                                </td>
                                <td width="20%" height="26" bgcolor="#CCCCCC">
                                    <p align="center">
                                        管 理
                                    </p>
                                </td>
                            </tr>
                            <tbody id="tblist">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td width="6%" align="center">
                                <%#Eval("MenuId")%>
                            </td>
                            <td width="10%" height="26" align="left">
                                <p align="left">
                                    <%#Eval("Name")%>
                                    <span id="tab_sun_<%#Eval("MenuId") %> %>_biao" onclick="tab_select(<%#Eval("MenuId") %>)">
                                        ▼</span>
                                </p>
                            </td>
                            <td width="25%" height="26">
                                <p align="center">
                                    <%#Eval("FatherMenuName")%>
                                </p>
                            </td>
                            <td width="5%" height="26" align="left">
                                <p align="center">
                                    <%#Eval("Level")%>
                                </p>
                            </td>
                            <td width="20%" height="26">
                                <p align="center">
                                    <%#Eval("MenuOperationType")%>
                                </p>
                            </td>
                            <td width="20%" height="26">
                                <p align="center">
                                    <asp:Label ID="Label1" Text="" runat="server" Visible='<%#bool.Parse(((int)Eval("MenuOperationTypeId")==1).ToString())%>'>
                        <a href="MenuDetail.aspx?fathermenuid=<%#Eval("MenuId") %>&fathermenuname=<%#Eval("Name") %>">
                                        子菜单添加</a>
                                    </asp:Label>
                                    <asp:Label ID="Label2" Text="" runat="server" Visible='<%#bool.Parse(((int)Eval("MenuOperationTypeId")==1).ToString())%>'>
                                    <a href="Menusort.aspx?fathermenuid=<%#Eval("MenuId") %>">子菜单排序</a>
                                    </asp:Label>
                                    <a href="MenuDetail.aspx?fathermenuid=0&menuid=<%#Eval("MenuId") %>">编辑</a> <a onclick="delwxmenu(<%#Eval("MenuId") %>)"
                                        href="javascript:void(0)">删除</a>
                                </p>
                            </td>
                        </tr>
                        <asp:HiddenField ID="HideFuncId" runat="server" Value='<%#Eval("MenuId") %>' />
                        <asp:Repeater ID="rptMenuList" runat="server">
                            <ItemTemplate>
                                <tr class="tab_sun_<%#Eval("FatherMenuId") %>" style="background-color: #EFEFEF;
                                    display: none;">
                                    <td width="6%" align="center">
                                        <%#Eval("MenuId")%>
                                    </td>
                                    <td width="10%" height="26" align="left">
                                        <p align="left">
                                            <a href="MenuDetail.aspx">
                                                <%#Eval("Name") %>
                                            </a>
                                        </p>
                                    </td>
                                    <td width="25%" height="26">
                                        <p align="center">
                                            <%#Eval("FatherMenuName")%>
                                        </p>
                                    </td>
                                    <td width="5%" height="26" align="left">
                                        <p align="center">
                                            <%#Eval("Level")%>
                                        </p>
                                    </td>
                                    <td width="20%" height="26">
                                        <p align="center">
                                            <%#Eval("MenuOperationType")%>
                                        </p>
                                    </td>
                                    <td width="20%" height="26">
                                        <p align="center">
                                            <a href="MenuDetail.aspx?fathermenuid=<%#Eval("FatherMenuId") %>&menuid=<%#Eval("MenuId") %> ">
                                                编辑</a> <a onclick="delwxmenu(<%#Eval("MenuId") %>)" href="javascript:void(0)">删除</a>
                                        </p>
                                    </td>
                                </tr>
                                <tr id="Tr1" runat="server" visible="false">
                                    <td style="height: 26px" colspan="6">
                                        查询数据为空
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ItemTemplate>
                    <FooterTemplate>
                        <tr runat="server" visible='<%#bool.Parse((rptTopMenuList.Items.Count==0).ToString())%>'>
                            <td style="height: 26px" colspan="6">
                                查询数据为空
                            </td>
                        </tr>
                        </tbody> </table>
                    </FooterTemplate>
                </asp:Repeater>
                <div id="divPage">
                </div>
                <input type="button" value="生成微信自定义菜单" onclick="createmenu()" class="button blue" />
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <input type="hidden" id="hid_tb" value="" />
</asp:Content>
