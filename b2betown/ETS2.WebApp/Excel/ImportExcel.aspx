<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ImportExcel.aspx.cs"
    Inherits="ETS2.WebApp.Excel.ImportExcel" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #settings h3
        {
            padding: 10px 0 5px 0;
            font-size: 20px;
            font-weight: bold;
            clear: both;
            color: #2D65AA;
        }
        #settings h4
        {
            padding: 10px 0 5px 0;
            font-size: 15px;
            clear: both;
        }
        .gray:hover
        {
            background: -webkit-linear-gradient(top,#fefefe,#ebeced);
            background: -moz-linear-gradient(top,#f2f3f7,#ebeced);
            background: linear-gradient(top,#f2f3f7,#ebeced);
        }
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
      <%--  <div id="secondary-tabs" class="navsetting ">
            <ul> 
                <li><a href="/ui/crmui/BusinessCustomersList.aspx?isactivate=0" onfocus="this.blur()">
                    未激活用户列表</a></li>
                <%
                    if (IsParentCompanyUser == true)
                    {
                %>
                <li class="on"><a href="/excel/ImportExcel.aspx" title="">导入历史会员信息</a></li>
                <li><a href="/excel/ObtainGZList.aspx" onfocus="this.blur()">导入已有微信粉丝</a></li>
                <%} %>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <%-- <h3 style="float: right">
                    <a href="ObtainGZList.aspx" style="text-decoration: underline; color: Blue">导入微信关注用户
                    </a>
                </h3>--%>
                <table>
                    <%-- <tr>
                        <td height="24" colspan="2">
                            <h3>
                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Excel/DownExcel.aspx"
                                    Style="font-size: larger; text-decoration: underline;">导入Excel格式详情可点击下载样例查看</asp:HyperLink></h3>
                        </td>
                    </tr>--%>
                    <tr>
                        <td height="24" colspan="2">
                            <h3>
                                导入公司名称:<%=comname %></h3>
                        </td>
                    </tr>
                    <tr>
                        <td height="24">
                            <h4>
                                请选择会员文件(excel)：</h4>
                        </td>
                        <td>
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                            <a style="color: Blue; text-decoration: underline;" href="/Excel/Excel导入样例.xls">Excel样例文件</a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <%--  <tr>
                        <td height="24">
                            请选择导入会员的激活状态:
                        </td>
                        <td>
                            <asp:RadioButtonList ID="radactivatestate" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="未激活" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="激活" Value="1"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>--%>
                    <tr>
                        <td height="24" colspan="2">
                            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="导入Excel到数据库"
                                class="button blue" />
                            <%--  <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="导入项目列表" class="button blue" />--%>
                            <asp:Literal ID="Literal1" runat="server" />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td height="24" colspan="2">
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False">
                                <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" CssClass="gvRow" />
                                <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" CssClass="gvHeader" />
                                <AlternatingRowStyle BackColor="#F7F7F7" CssClass="gvAlternatingRow" />
                                <Columns>
                                    <asp:BoundField DataField="id" HeaderText="编号" />
                                    <asp:BoundField DataField="name" HeaderText="姓名" />
                                    <asp:BoundField DataField="phone" HeaderText="手机" />
                                    <asp:BoundField DataField="email" HeaderText="邮箱" />
                                    <asp:BoundField DataField="weixin" HeaderText="微信" />
                                    <asp:BoundField DataField="importtime" HeaderText="录入时间" Visible="false" />
                                    <asp:BoundField DataField="whetheractivate" HeaderText="是否激活" Visible="false" />
                                    <asp:BoundField DataField="errreason" HeaderText="出错原因" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
   
   
</asp:Content>
