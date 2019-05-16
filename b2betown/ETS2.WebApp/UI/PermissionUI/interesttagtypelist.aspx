<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="interesttagtypelist.aspx.cs"
    MasterPageFile="/UI/Etown.Master" Inherits="ETS2.WebApp.UI.PermissionUI.interesttagtypelist" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function delindustry(id) {
            if (confirm("确认删除此行业吗？")) {
                $.post("/JsonFactory/CrmMemberHandler.ashx?oper=delindustry", { id: id }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("删除出错" + data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("删除成功", {
                            buttons: [{ title: '确定', value: true}],
                            opacity: 0.1,
                            focus: 0,
                            show: 'slideDown',
                            submit: function (e, v, m, f) {
                                if (v == true)
                                    location.href = "interesttagtypelist.aspx";
                            }
                        });
                    }
                });
            } else {
                alert("你取消了删除操作");
            }
        }
        function deltagtype(id, Isselfdefined) {
            if (Isselfdefined == "1") {
                $.prompt("自定义兴趣类型不可删除");
                return;
            } else {
                if (confirm("确认删除此行业吗？")) {
                    $.post("/JsonFactory/CrmMemberHandler.ashx?oper=deltagtype", { id: id }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $.prompt("删除出错" + data.msg);
                            return;
                        }
                        if (data.type == 100) {
                            $.prompt("删除成功", {
                                buttons: [{ title: '确定', value: true}],
                                opacity: 0.1,
                                focus: 0,
                                show: 'slideDown',
                                submit: function (e, v, m, f) {
                                    if (v == true)
                                        location.href = "interesttagtypelist.aspx";
                                }
                            });
                        }
                    });
                } else {
                    alert("你取消了删除操作");
                }
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
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/PermissionUI/interesttagtypelist.aspx" onfocus="this.blur()"><span>
                    会员兴趣类型设置</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list mail-list1">
            <div class="inner">
                <h3>
                    <label>
                        会员兴趣类型列表</label>
                </h3>
                <h3>
                    <label style="float: right">
                        <a href="CompanyIndustryEdit.aspx" style="color: #2D65AA;">公司行业添加</a>
                    </label>
                </h3>
                <asp:Repeater ID="rptTopMenuList" runat="server">
                    <HeaderTemplate>
                        <table border="0" width="780" class="mail-list-title">
                            <tr>
                                <td width="6%" align="center" bgcolor="#CCCCCC" style="display: none;">
                                    编号
                                </td>
                                <td width="54%" height="26" align="left" bgcolor="#CCCCCC">
                                    <p align="left">
                                        名称
                                    </p>
                                </td>
                                <td width="20%" height="26" align="left" bgcolor="#CCCCCC">
                                    <p align="center">
                                        分类
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
                            <td align="center" style="display: none;">
                                <%#Eval("MenuId")%>
                            </td>
                            <td height="26" align="left">
                                <p align="left">
                                    <%#Eval("Name")%>
                                    <span id="tab_sun_<%#Eval("MenuId") %> %>_biao" onclick="tab_select(<%#Eval("MenuId") %>)">
                                        ▼</span>
                                </p>
                            </td>
                            <td height="26" align="left">
                                <p align="center">
                                    <%#Eval("Class")%>
                                </p>
                            </td>
                            <td height="26">
                                <p align="center">
                                    <a href="interesttagtypeedit.aspx?industryid=<%#Eval("MenuId") %>">兴趣类型添加</a> <a
                                        href="CompanyIndustryEdit.aspx?industryid=<%#Eval("MenuId") %>">编辑</a> <a onclick="delindustry(<%#Eval("MenuId") %>)"
                                            href="javascript:void(0)">删除</a>
                                </p>
                            </td>
                        </tr>
                        <asp:HiddenField ID="HideFuncId" runat="server" Value='<%#Eval("MenuId") %>' />
                        <asp:Repeater ID="rptMenuList" runat="server">
                            <ItemTemplate>
                                <tr class="tab_sun_<%#Eval("FatherMenuId") %>" style="background-color: #EFEFEF;">
                                    <td align="center" style="display: none;">
                                        <%#Eval("MenuId")%>
                                    </td>
                                    <td height="26" align="left" style="padding-left: 20px;">
                                        <p align="left">
                                            <%#Eval("Name") %>
                                        </p>
                                    </td>
                                    <td height="26" align="left">
                                        <p align="center">
                                            <%#Eval("Class")%>
                                        </p>
                                    </td>
                                    <td height="26">
                                        <p align="center">
                                            <a href="InterestTagList.aspx?tagtypeid=<%#Eval("MenuId") %>">标签管理</a> <a href="interesttagtypeedit.aspx?industryid=<%#Eval("FatherMenuId") %>&tagtypeid=<%#Eval("MenuId") %> ">
                                                编辑</a> <a onclick="deltagtype(<%#Eval("MenuId") %>,<%#Eval("Isselfdefined") %>)"
                                                    href="javascript:void(0)">删除</a>
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
                        <tr id="Tr1" runat="server" visible='<%#bool.Parse((rptTopMenuList.Items.Count==0).ToString())%>'>
                            <td style="height: 26px" colspan="6">
                                查询数据为空
                            </td>
                        </tr>
                        </tbody> </table>
                    </FooterTemplate>
                </asp:Repeater>
                <div id="divPage">
                </div>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
</asp:Content>
