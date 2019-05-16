<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Etown.Master" AutoEventWireup="true"
    CodeBehind="Channelstatistics.aspx.cs" Inherits="ETS2.WebApp.UI.Channelstatistics"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").trimVal();

            //判断公司是否含有添加内部渠道单位(所属门市)的权限:和平台总账户商户管理中是否含有所属门市挂钩
            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=GetCompanyInfo", { comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert("判断商家是否含有所属门市出错");
                    return;
                }
                if (data.type == 100) {
                    if (data.msg.HasInnerChannel == true) {
                        $("#addinnerchannelcompany").show();

                        $("#addinnerchanneltongji").show();
                    }
                    else {
                        $("#addinnerchannelcompany").hide();

                        $("#addinnerchanneltongji").hide();
                    }
                }
            })


            var channelcompanytype = $("#hid_channelcompanytype").val();

            if (channelcompanytype == "inner") {
                $("#addinnerchanneltongji").addClass("on");
                //                $("#divaddoutchannel").hide();
            } else {
                $("#addoutchanneltongji").addClass("on");
                //                $("#divaddinnerchannel").hide();
            }
        })
    </script>
    <script type="text/javascript" src="/Scripts/jquery.cookie.2.2.0.min.js"></script>
    <script type="text/javascript">
        $(function () {
            if ($.cookie($("#hid_userid").val() + "_IsViewMd1")) {
                //展开门店的Id
                var seledid = $.cookie($("#hid_userid").val() + "_IsViewMd1");

                var array = seledid.split(',');
                for (var i = 0; i < array.length; i++) {
                    $("#div" + array[i]).css("display", "block");
                    $("#btnShowHidden" + array[i]).html("隐藏渠道人列表");
                }

            }
        })
        /*隐藏嵌套的Gridview*/
        function ShowHidden(sid) {

            var str = $.cookie($("#hid_userid").val() + "_IsViewMd1");


            if ($("#div" + sid).css("display") == "none") {//展开门店操作
                $("#div" + sid).css("display", "block");
                $("#btnShowHidden" + sid).html("隐藏渠道人列表");
                if (str == null) {
                    $.cookie($("#hid_userid").val() + "_IsViewMd1", sid, { path: '/' }); //保存展开门店的id

                } else {
                    $.cookie($("#hid_userid").val() + "_IsViewMd1", str + "," + sid, { path: '/' }); //保存展开门店的id

                }
            } else {//关闭门店操作

                $("#div" + sid).css("display", "none");
                $("#btnShowHidden" + sid).html("展开渠道人列表");

                if (str == null) {
                    $.cookie($("#hid_userid").val() + "_IsViewMd1", null, { path: '/' }); //保存展开门店的id

                } else {
                    var array = $.cookie($("#hid_userid").val() + "_IsViewMd1").split(',');
                    if (array.length == 1) {
                        str = str.replace(sid, "");
                    } else {
                        str = str.replace("," + sid, "");
                    }
                    if (str == "") {
                        $.cookie($("#hid_userid").val() + "_IsViewMd1", null, { path: '/' }); //保存展开门店的id

                    } else {
                        $.cookie($("#hid_userid").val() + "_IsViewMd1", str, { path: '/' }); //保存展开门店的id

                    }
                }
            }
        }
        function EditChannelUnit(id) {
            window.open("ChannelCompanyEdit.aspx?channelcompanyid=" + id, target = "_self");
        }
        function ViewOpenCardList(id) {
            window.open("MemberCardList.aspx?channelid=" + id + "&isopencard=1", target = "_self");
        }
        function EditChannel(id) {
            window.open("ChannelEdit.aspx?channelid=" + id, target = "_self");
        }
        function EditProChannel(id) {
            window.open("ChannelCompanyProEdit.aspx?channelcompanyid=" + id, target = "_self");
        }
        function ImgChannel(id) {
            window.open("ChannelImgList.aspx?channelcompanyid=" + id, target = "_self");
        }
        function ViewChannel(id) {
            var h = 680;
            var w = 430;
            var t = screen.availHeight / 2 - h / 2;
            var l = screen.availWidth / 2 - w / 2;
            var prop = "dialogHeight:" + h + "px; dialogWidth:" + w + "px; dialogLeft:" + l + "px; dialogTop:" + t + "px;toolbar:no; menubar:no; scrollbars:yes; resizable:no;location:no;status:no;help:no";
            var path = "/h5/StoreDefault.aspx?menshiid=" + id;
            var ret = window.showModalDialog(path, "", prop);
        }


      
    </script>
    <style type="text/css">
        /*-gridview样式-*/
        .gv
        {
            border: 0.1px solid #D7D7D7;
            font-size: 12px;
            text-align: center;
        }
        .gvHeader
        {
            color: #3F6293;
            background-color: #F7F7F7;
            height: 40px;
            line-height: 24px;
            text-align: center;
            font-weight: normal;
            font-variant: normal;
        }
        .gvHeader th
        {
            font-weight: normal;
            font-variant: normal;
            text-align: center;
        }
        .gvRow, .gvAlternatingRow, .gvEditRow
        {
            line-height: 20px;
            text-align: center;
            padding: 2px;
            height: 30px;
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
        
        .gvHeader2
        {
            color: #3F6293;
            background-color: #F7F7F7;
            height: 30px;
            text-align: center;
            font-weight: normal;
            font-variant: normal;
        }
        .gvHeader2 th
        {
            font-weight: normal;
            font-variant: normal;
            text-align: center;
        }
        .gvHeader2 th, .gvHeader2 td
        {
            border: 0;
        }
        
        .gvRow2, .gvEditRow2
        {
            line-height: 20px;
            text-align: center;
            padding: 2px;
            height: 30px;
        }
        .gvRow2 td, .gvEditRow2 td
        {
            border: 0;
        }
        
        
        a:link, a:visited
        {
            color: #1e5494;
            text-decoration: none;
        }
        .aspNetDisabled
        {
            color: #666;
            text-decoration: none;
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
            width: 100px;
            line-height: 28px;
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <%--  <div id="secondary-tabs" class="navsetting ">
            <ul>
                <%if (IsParentCompanyUser)
                  { %>
                <li id="addoutchanneltongji"><a href="Channelstatistics.aspx?channelcompanytype=out"
                    onfocus="this.blur()">合作单位</a></li>
                <li id="addoutchannelcompany"><a href="ChannelCompanyEdit.aspx?channeltype=out" onfocus="this.blur()">
                    添加合作单位</a></li>
                <li id="addinnerchanneltongji"><a href="Channelstatistics.aspx?channelcompanytype=inner"
                    onfocus="this.blur()">所属门店 </a></li>
                <li id="addinnerchannelcompany"><a href="ChannelCompanyEdit.aspx?channeltype=inner"
                    onfocus="this.blur()"><span>添加门店</span></a></li>
                <%}
                  else
                  { %>
                <li id="addinnerchanneltongji"><a href="Channelstatistics.aspx?channelcompanytype=inner"
                    onfocus="this.blur()">所属门店 </a></li>
                <% } %>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    <%-- <div id="divaddinnerchannel">
                        <a style="text-align: left; color: Blue;" href="ChannelEdit.aspx?channeltype=inner">
                            添加所属门店渠道人</a>
                    </div>
                    <div id="divaddoutchannel">
                        <a style="text-align: left; color: Blue;" href="ChannelEdit.aspx?channeltype=out">添加合作公司渠道人</a>
                    </div>--%>
                </h3>
                <h3>
                    <%--<a href="javascript:void(0)" onclick="viewchannelcompany('0,1')">显示全部</a>&nbsp;
                    <a href="javascript:void(0)" onclick="viewchannelcompany(1)">显示开通</a> &nbsp; <a href="javascript:void(0)"
                        onclick="viewchannelcompany(0)">显示停业</a>&nbsp;--%>
                </h3>
                <table>
                    <tr>
                        <td style="padding: 0 10px 20px 10px;">
                            请按门店，姓名，手机查询
                            <asp:TextBox ID="txtkey" runat="server" Style="height: 20px;"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="Btnsearch" runat="server" Text="查 询" OnClick="Btnsearch_Click" class="button blue" />
                        </td>
                        <%
                            if (IsParentCompanyUser)
                            {
                        %>
                        <td>
                            所属门店关注总数:<%=md_subscribenum %>
                        </td>
                        <%
                            }
                        %>
                    </tr>
                </table>
                <asp:GridView ID="GridView2" runat="server" Width="1000px" AllowPaging="True" AutoGenerateColumns="False"
                    OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging"
                    BorderStyle="Solid" PageSize="15">
                    <RowStyle ForeColor="#4A3C8C" CssClass="gvRow" />
                    <HeaderStyle BackColor="#C1D9F3" Font-Bold="True" ForeColor="#000000" CssClass="gvHeader" />
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="编号">
                            <HeaderStyle Width="3%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="companyname" HeaderText="渠道单位名称">
                            <HeaderStyle Width="12%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="opencardnum" HeaderText="开卡量">
                            <HeaderStyle Width="5%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="firstdealnum" HeaderText="第一次交易量">
                            <HeaderStyle Width="10%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="summoney" HeaderText="金额">
                            <HeaderStyle Width="5%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="companynum" HeaderText="验卡量">
                            <HeaderStyle Width="5%" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="扫码总数">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <%# GetScanTotal2(Eval("ID").ToString(),"1")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="关注总数">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <%# GetWxTotal2(Eval("id").ToString(), "1", Eval("issuetype").ToString())%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="取消关注总数">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <%# GetQxWxTotal2(Eval("id").ToString(),"1") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="状 态">
                            <HeaderStyle Width="4%" />
                            <ItemTemplate>
                                <%#Runstatus(Eval("companystate").ToString())%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle Width="45%" />
                            <ItemTemplate>
                                <span id="span4" style="color: Red; cursor: pointer; margin: 0 6px 0 0" onclick="ViewChannel('<%#Eval("id") %>',224)">
                                    预览</span>
                                <%if (channelcompanytype == "out")
                                  { %><span id="span2" style="color: Red; cursor: pointer; margin: 0 6px 0 0" onclick="EditProChannel('<%#Eval("id") %>',224)">
                                      合作推荐 </span>
                                <%}
                                  else
                                  { %><span id="span5" style="color: Red; cursor: pointer; margin: 0 6px 0 0" onclick="EditProChannel('<%#Eval("id") %>',224)">
                                      店长推荐 </span>
                                <%} %>
                                <span id="span1" style="color: Red; cursor: pointer; margin: 0 6px 0 0" onclick="EditChannelUnit('<%#Eval("id") %>',224)">
                                    编辑</span> <span id="span3" style="color: Red; cursor: pointer; margin: 0 6px 0 0"
                                        onclick="ImgChannel('<%#Eval("id") %>',224)">门市图片</span>
                                <!---点击用于列表展开，执行JS函数--->
                                <span id="btnShowHidden<%#Eval("ID") %>" style="color: Red; cursor: pointer; margin: 0 0 0 0"
                                    onclick="ShowHidden('<%#Eval("ID") %>')">显示渠道人</span>
                                <tr>
                                    <td colspan="100%">
                                        <div id="div<%#Eval("ID") %>" style="display: none; padding-bottom: 10px;">
                                            <%-- <div style="float: left; font-size: small">
                                                └</div>--%>
                                            <div style="border: 1 solid RGB(40,80,150); position: relative; left: 0px; overflow: auto;
                                                width: 100%;">
                                                <!---绑定内层Gridview--->
                                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("id") %>' />
                                                <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" Width="100%"
                                                    Height="100%" BorderStyle="Solid" AllowPaging="True" OnPageIndexChanging="GridView3_PageIndexChanging"
                                                    OnRowDataBound="GridView3_RowDataBound" PageSize="10">
                                                    <RowStyle ForeColor="#4A3C8C" CssClass="gvRow2" />
                                                    <HeaderStyle BackColor="#C1D9F3" Font-Bold="True" ForeColor="#000000" CssClass="gvHeader2" />
                                                    <EmptyDataTemplate>
                                                        <table width="100%" style="height: 30px;">
                                                            <tr>
                                                                <td align="center">
                                                                    当前渠道单位还未添加渠道人!
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:BoundField DataField="id" HeaderText="编号">
                                                            <HeaderStyle Width="3%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="渠道单位名称">
                                                            <HeaderStyle Width="10%" />
                                                            <ItemTemplate>
                                                                <%#GetChannelUnitNameById(Eval("companyid").ToString())%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="name" HeaderText="姓名">
                                                            <HeaderStyle Width="5%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="mobile" HeaderText="手机">
                                                            <HeaderStyle Width="9%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="cardcode" HeaderText="卡号">
                                                            <HeaderStyle Width="11%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Chaddress" HeaderText="地 点">
                                                            <HeaderStyle Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ChObjects" HeaderText="渠道人群">
                                                            <HeaderStyle Width="5%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="返佣办法">
                                                            <HeaderStyle Width="10%" />
                                                            <ItemTemplate>
                                                                卡开<%# Eval("RebateOpen")%>元/首次交易<%# Eval("RebateConsume")%>元
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="rebatelevel" HeaderText="渠道级别">
                                                            <HeaderStyle Width="5%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="录入量">
                                                            <HeaderStyle Width="5%" />
                                                            <ItemTemplate>
                                                                <%# GetEnteredNumberByChannelId(Eval("id").ToString())%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="开卡量">
                                                            <HeaderStyle Width="5%" />
                                                            <ItemTemplate>
                                                                <span id="span1" style="color: Red; cursor: pointer; margin: 0 20px 0 0" onclick="ViewOpenCardList('<%#Eval("id") %>',224)">
                                                                    <%#Eval("OpenCardNum")%></span>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="firstdealnum" HeaderText="第一次交易量">
                                                            <HeaderStyle Width="5%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="summoney" HeaderText="金额">
                                                            <HeaderStyle Width="3%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="类型">
                                                            <HeaderStyle Width="5%" />
                                                            <ItemTemplate>
                                                                <%# GetTypeName(Eval("Issuetype").ToString())%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="状 态">
                                                            <HeaderStyle Width="4%" />
                                                            <ItemTemplate>
                                                                <%#Runstatus(Eval("runstate").ToString())%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="管理">
                                                            <HeaderStyle Width="5%" />
                                                            <ItemTemplate>
                                                                <span id="span1" style="color: Red; cursor: pointer; margin: 0 10px 0 0" onclick="EditChannel('<%#Eval("id") %>',224)">
                                                                    编辑</span> <span id="span6" style="color: blue; cursor: pointer;" onclick="referrer_ch1('<%#Eval("id") %>','<%#Eval("name") %>','<%#Eval("companyid") %>',150)">
                                                                        二维码</span>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerTemplate>
                                                        <table width="100%" style="height: 30px;">
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:HiddenField ID="bb" runat="server" />
                                                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="First" CommandName="Page"
                                                                        Enabled='<%#((GridView)Container.NamingContainer).PageIndex != 0 %>'>首页</asp:LinkButton>
                                                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Prev" CommandName="Page"
                                                                        Enabled='<%#((GridView)Container.NamingContainer).PageIndex != 0 %>'>上一页</asp:LinkButton>
                                                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Next" CommandName="Page"
                                                                        Enabled='<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>'>下一页</asp:LinkButton>
                                                                    <asp:LinkButton ID="LinkButton5" runat="server" CommandArgument="Last" CommandName="Page"
                                                                        Enabled='<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>'>尾页</asp:LinkButton>
                                                                    <asp:Label ID="Label3" runat="server" Text=""></asp:Label>/共<asp:Label ID="Label5"
                                                                        runat="server" Text="" ForeColor="Red"></asp:Label>条 第<asp:Label ID="Label6" runat="server"
                                                                            Text="<%# ((GridView)Container.NamingContainer).PageIndex + 1 %>" ForeColor="Red"></asp:Label>/<asp:Label
                                                                                ID="Label7" runat="server" Text="<%# ((GridView)Container.NamingContainer).PageCount %>"
                                                                                ForeColor="Red"></asp:Label>页 转到第
                                                                    <asp:TextBox ID="txtNewPageIndex" runat="server" Width="20px" Text='<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>' />页
                                                                    <asp:LinkButton ID="btnGo" runat="server" CausesValidation="False" CommandArgument="-2"
                                                                        CommandName="Page" Text="GO" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </PagerTemplate>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerTemplate>
                        <table width="100%" style="height: 30px;">
                            <tr>
                                <td align="right">
                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="First" CommandName="Page"
                                        Enabled='<%#((GridView)Container.NamingContainer).PageIndex != 0 %>'>首页</asp:LinkButton>
                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Prev" CommandName="Page"
                                        Enabled='<%#((GridView)Container.NamingContainer).PageIndex != 0 %>'>上一页</asp:LinkButton>
                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Next" CommandName="Page"
                                        Enabled='<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>'>下一页</asp:LinkButton>
                                    <asp:LinkButton ID="LinkButton5" runat="server" CommandArgument="Last" CommandName="Page"
                                        Enabled='<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>'>尾页</asp:LinkButton>
                                    <asp:Label ID="Label3" runat="server" Text="<%#ts() %>"></asp:Label>/共<asp:Label
                                        ID="Label5" runat="server" Text="<%#zjls() %>" ForeColor="Red"></asp:Label>条
                                    第<asp:Label ID="Label6" runat="server" Text="<%# ((GridView)Container.NamingContainer).PageIndex + 1 %>"
                                        ForeColor="Red"></asp:Label>/<asp:Label ID="Label7" runat="server" Text="<%# ((GridView)Container.NamingContainer).PageCount %>"
                                            ForeColor="Red"></asp:Label>页 转到第
                                    <asp:TextBox ID="txtNewPageIndex" runat="server" Width="20px" Text='<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>' />页
                                    <asp:LinkButton ID="btnGo" runat="server" CausesValidation="False" CommandArgument="-2"
                                        CommandName="Page" Text="GO" />
                                </td>
                            </tr>
                        </table>
                    </PagerTemplate>
                </asp:GridView>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <asp:HiddenField ID="hid_key" runat="server" Value="" />
    <input type="hidden" id="hid_channelcompanytype" value="<%=channelcompanytype %>" />
    <div id="proqrcode_rhshow" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; display: none; left: 10%; position: absolute; top: 20%;">
        <input type="hidden" id="hid_channelcompanyid" value="" />
        <input type="hidden" id="hid_channelid" value="" />
         <input type="hidden" id="hid_channelname" value="" />
        <input type="hidden" id="hid_qrcodeid" value="" />
        <table width="600px" border="0" cellpadding="10" cellspacing="1" style="margin: 10px 5px;">
            <tr style="text-align: center;">
                <td colspan="3">
                    <span style="font-size: 16px;">渠道二维码信息</span>
                </td>
            </tr>
            <tr>
                <td style="width: 200px; text-align: center;">
                    <img src="/Images/defaultThumb.png" id="img1" height="150" width="150" />
                    <br />
                    <label>
                        二 维 码 图 片</label>
                    <label style="display: none;">
                        *二维码尺寸请按照43像素的<br />
                        整数倍缩放，以保持最佳效果</label>
                </td>
                <td colspan="2" style="vertical-align: top; padding: 10px 0 5px 0;">
                    <table id="table_qr" style="min-height: 200px;">
                        <tr>
                            <td>
                                二维码显示类型
                            </td>
                            <td>
                                <select id="sel_viewtype" class="mi-input">
                                    <option value="0">请选择..</option>
                                    <option value="1">营销活动</option>
                                    <option value="2">产品图文</option>
                                   <%-- <option value="3">分销商</option>--%>
                                    <option value="4">文章图文</option>
                                    <option value="5">微商城图文</option>
                                    <option value="6">门店图文</option>
                                    <option value="7">合作单位图文</option>
                                    <option value="8">顾问信息</option>   
                                    <option value="10">抽奖活动</option>   
                                </select>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td>
                                项目列表
                            </td>
                            <td>
                                <select id="sel_projectlist" class="mi-input">
                                    <option value="0">请选择..</option>
                                </select>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td>
                                产品列表
                            </td>
                            <td>
                                <select id="sel_prolist" class="mi-input">
                                    <option value="0">请选择..</option>
                                </select>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td>
                                素材类型列表
                            </td>
                            <td>
                                <select id="sel_materialtype" class="mi-input">
                                    <option value="0">请选择..</option>
                                </select>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td>
                                素材列表
                            </td>
                            <td>
                                <select id="sel_materiallist" class="mi-input">
                                    <option value="0">请选择..</option>
                                </select>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td>
                                微商城图片
                            </td>
                            <td>
                                <input type="hidden" id="hid_micromallimg" value="" />
                                <img alt="" class="headPortraitImgSrc" id="headPortraitImg" src="/images/defaultThumb.png" />
                                <uc1:uploadFile ID="headPortrait" runat="server" />
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td>
                                门店列表
                            </td>
                            <td>
                                <select id="sel_menshi" class="mi-input">
                                    <option value="0">请选择..</option>
                                </select>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td>
                                合作单位列表
                            </td>
                            <td>
                                <select id="sel_hezuo" class="mi-input">
                                    <option value="0">请选择..</option>
                                </select>
                            </td>
                        </tr>
                         <tr style="display: none;">
                            <td>
                                活动列表
                            </td>
                            <td>
                                <select id="sel_act" class="mi-input">
                                    <option value="0">请选择..</option>
                                </select>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td>
                                抽奖活动列表
                            </td>
                            <td>
                                <select id="sel_choujiangact" class="mi-input">
                                    <option value="0">请选择..</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center; font-weight: bold; font-size: 16px;">
                                <a href="javascript:void(0)" onclick="submitbtn()" class="a_anniu">确 定</a> <a href="javascript:void(0)"
                                    onclick="closebtn()" class="a_anniu">关 闭</a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        $(function () {
            $("#sel_viewtype").change(function () {
                var sel_viewtype = $("#sel_viewtype").val();
                Viewbyviewtype(sel_viewtype); 
            })

            $("#sel_projectlist").change(function () {
                var seled = $("#sel_projectlist").val();
                showproductlist(seled, 0);
            })

            $("#sel_materialtype").change(function () {
                var seled = $("#sel_materialtype").val();
                showmateriallist(seled, 0);
            })
        })
        function Viewbyviewtype(sel_viewtype) {
            $.each($("#table_qr tr"), function (i, item) {
                $(item).hide();
            })

            if (sel_viewtype == 1) {
                $("#sel_act").parent().parent().show();
                showactlist();
            }
            else if (sel_viewtype == 2) {

                $("#sel_projectlist").parent().parent().show();
                $("#sel_prolist").parent().parent().show();
                showprojectlist();

            }
            else if (sel_viewtype == 3) {
              
            }
            else if (sel_viewtype == 4) {

                $("#sel_materialtype").parent().parent().show();
                $("#sel_materiallist").parent().parent().show();
                showmaterialtypelist();
            }
            else if (sel_viewtype == 5) {
//                //微商城图文  图片添加操作
//                $("#hid_micromallimg").parent().parent().show();
            }
            else if (sel_viewtype == 6) {
                $("#sel_menshi").parent().parent().show();
                showmenshilist();
            }
            else if (sel_viewtype == 7) {
                $("#sel_hezuo").parent().parent().show();
                showhezuolist();
            }
            else if (sel_viewtype == 8) {

            }
            else if (sel_viewtype == 10) {
                $("#sel_choujiangact").parent().parent().show();
                showchoujianglist();
            }
            $("#table_qr tr").first().show();
            $("#table_qr tr").last().show();
            return true;
        }

        function showchoujianglist(seled) {
            $.post("/JsonFactory/PromotionHandler.ashx?oper=ERNIEActpagelist", { comid: $("#hid_comid").val(), pageindex: 1, pagesize: 200, key: "", runstate: "0,1" }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {
                    if (data.totalCount == 0) {
                        alert("商户没有抽奖活动!");
                        var str = '<option value="0">请选择..</option>';
                        $("#sel_choujiangact").html(str);
                        return;
                    } else {
                        var str = '<option value="0">请选择..</option>';
                        for (var i = 0; i < data.totalCount; i++) {
                            if (seled == data.msg[i].Id) {
                                str += '<option value="' + data.msg[i].Id + '" selected="selected">' + data.msg[i].Title + '</option>';
                            } else {
                                str += '<option value="' + data.msg[i].Id + '" >' + data.msg[i].Title + '</option>';
                            }
                        }
                        $("#sel_choujiangact").html(str);
                    }
                }
            })
        }
        function showprojectlist(seled) {
            $.post("/JsonFactory/ProductHandler.ashx?oper=projectpagelist", { comid: $("#hid_comid").val(), pageindex: 1, pagesize: 200, key: "", projectstate: "1" }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {
                    if (data.totalCount == 0) {
                        alert("商户没有产品项目!");
                        var str = '<option value="0">请选择..</option>';
                        $("#sel_projectlist").html(str);
                        return;
                    } else {
                        var str = '<option value="0">请选择..</option>';
                        for (var i = 0; i < data.totalCount; i++) {
                            if (seled == data.msg[i].Id) {
                                str += '<option value="' + data.msg[i].Id + '" selected="selected">' + data.msg[i].Projectname + '</option>';
                            } else {
                                str += '<option value="' + data.msg[i].Id + '" >' + data.msg[i].Projectname + '</option>';
                            }
                        }
                        $("#sel_projectlist").html(str);
                    }
                }
            })
        }
        function showproductlist(projectid, seled) {
            $.post("/JsonFactory/ProductHandler.ashx?oper=pagelist", { projectid: projectid, comid: $("#hid_comid").val(), pageindex: 1, pagesize: 300, key: "", pro_state: "1" }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {

                    if (data.totalCount == 0) {
                        alert("商户当前项目下没有产品");
                        var str = '<option value="0">请选择..</option>';
                        $("#sel_prolist").html(str);
                        return;
                    }
                    if (data.type == 100) {
                        var str = '<option value="0">请选择..</option>';
                        for (var i = 0; i < data.totalCount; i++) {
                            if (seled == data.msg[i].Id) {
                                str += '<option value="' + data.msg[i].Id + '" selected="selected">' + data.msg[i].Pro_name + '</option>';
                            } else {
                                str += '<option value="' + data.msg[i].Id + '">' + data.msg[i].Pro_name + '</option>';
                            }
                        }
                        $("#sel_prolist").html(str);
                    }
                }
            })
        }
        function showmaterialtypelist(seled) {
            $.post("/jsonfactory/WeiXinHandler.ashx?oper=GetAllWxMaterialType", { comid: $("#hid_comid").trimVal() }, function (data) {
                data = eval("(" + data + ")");

                if (data.type == 1) { }
                if (data.type == 100) {
                    if (data.totalCount == 0) {
                        alert("商户下面没有素材类型");
                        var str = '<option value="0">请选择..</option>';
                        $("#sel_materialtype").html(str);
                        return;
                    }
                    if (data.totalCount > 0) {

                        var str = '<option value="0">请选择..</option>';
                        for (var i = 0; i < data.totalCount; i++) {
                            if (data.msg[i].Id == seled) {
                                str += '<option value="' + data.msg[i].Id + '" selected="selected">' + data.msg[i].TypeName + '</option>';
                            } else {
                                str += '<option value="' + data.msg[i].Id + '"  >' + data.msg[i].TypeName + '</option>';
                            }
                        }
                        $("#sel_materialtype").html(str);
                    }
                }
            });

        }
        function showmateriallist(promotetypeid, seled) {
            $.post("/JsonFactory/WeiXinHandler.ashx?oper=pagelist", { comid: $("#hid_comid").val(), pageindex: 1, pagesize: 200, applystate: 1, promotetypeid: promotetypeid, key: "" }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {

                    if (data.totalCount == 0) {
                        alert("商户当前类型下没有素材");
                        var str = '<option value="0">请选择..</option>';
                        $("#sel_materiallist").html(str);
                        return;
                    }
                    if (data.type == 100) {
                        var str = '<option value="0">请选择..</option>';
                        for (var i = 0; i < data.totalCount; i++) {
                            if (seled == data.msg[i].MaterialId) {
                                str += '<option value="' + data.msg[i].MaterialId + '" selected="selected">' + data.msg[i].Title + '</option>';
                            } else {
                                str += '<option value="' + data.msg[i].MaterialId + '">' + data.msg[i].Title + '</option>';
                            }
                        }
                        $("#sel_materiallist").html(str);
                    }
                }
            })
        }
        function showactlist(seled) {
            $.post("/JsonFactory/PromotionHandler.ashx?oper=pagelist", { comid: $("#hid_comid").val(), pageindex: 1, pagesize: 200, userid:0,state:"1" }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {
                    if (data.totalCount == 0) {
                        alert("商户没有有效活动!");
                        var str = '<option value="0">请选择..</option>';
                        $("#sel_act").html(str);
                        return;
                    } else {
                        var str = '<option value="0">请选择..</option>';
                        for (var i = 0; i < data.totalCount; i++) {
                            if (seled == data.msg[i].Id) {
                                str += '<option value="' + data.msg[i].Id + '" selected="selected">' + data.msg[i].Title + '</option>';
                            } else {
                                str += '<option value="' + data.msg[i].Id + '" >' + data.msg[i].Title + '</option>';
                            }
                        }
                        $("#sel_act").html(str);
                    }
                }
            })
        }
         
        function showmenshilist(seled) {
            $.post("/JsonFactory/ProductHandler.ashx?oper=channelcompanylist", { comid: $("#hid_comid").val(), key: "", companystate: "1", Issuetype: "0" }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {
                    if (data.totalCount == 0) {
                        alert("商户没有门市!");
                        var str = '<option value="0">请选择..</option>';
                        $("#sel_menshi").html(str);
                        return;
                    } else {
                        var str = '<option value="0">请选择..</option>';
                        for (var i = 0; i < data.totalCount; i++) {
                            if ($("#hid_channelcompanyid").val() == data.msg[i].Id) {
                                str += '<option value="' + data.msg[i].Id + '" selected="selected">' + data.msg[i].Companyname + '</option>';
                            } else {
//                                str += '<option value="' + data.msg[i].Id + '" >' + data.msg[i].Companyname + '</option>';
                            }
                        }
                        $("#sel_menshi").html(str);
                    }
                }
            })
        }
        function showhezuolist(seled) {
            $.post("/JsonFactory/ProductHandler.ashx?oper=channelcompanylist", { comid: $("#hid_comid").val(), key: "", companystate: "1", Issuetype: "1" }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {
                    if (data.totalCount == 0) {
                        alert("商户没有合作单位!");
                        var str = '<option value="0">请选择..</option>';
                        $("#sel_hezuo").html(str);
                        return;
                    } else {
                        var str = '<option value="0">请选择..</option>';
                        for (var i = 0; i < data.totalCount; i++) {
                            if ($("#hid_channelcompanyid").val() == data.msg[i].Id) {
                                str += '<option value="' + data.msg[i].Id + '" selected="selected">' + data.msg[i].Companyname + '</option>';
                            } else {
//                               str += '<option value="' + data.msg[i].Id + '" >' + data.msg[i].Companyname + '</option>';
                            }
                        }
                        $("#sel_hezuo").html(str);
                    }
                }
            })
        }

        function submitbtn() {
            var sel_viewtype = $("#sel_viewtype").val();
            var sel_projectlist = $("#sel_projectlist").val();
            var sel_prolist = $("#sel_prolist").val();
            var sel_materialtype = $("#sel_materialtype").val();
            var sel_materiallist = $("#sel_materiallist").val();
            var micromallimgid = $("#<%=headPortrait.FileUploadId_ClientId %>").val();
            if (micromallimgid == "") {
                micromallimgid = $("#hid_micromallimg").trimVal();
            }
            var sel_menshi = $("#sel_menshi").val();
            var sel_hezuo = $("#sel_hezuo").val();
            var sel_act = $("#sel_act").val();
            var sel_choujiangact = $("#sel_choujiangact").val();

            if (sel_viewtype == 0) {
                alert("请选择显示类型");
                return;
            }
            if (sel_viewtype == 1) {
                if (sel_act == 0) {
                    alert("请选择活动");
                    return;
                }
            }
            if (sel_viewtype == 2) {
                if (sel_projectlist == 0 && sel_prolist == 0) {
                    alert("项目 和 产品 至少选择一项 ");
                    return;
                }
            }
            if (sel_viewtype == 3) {
                 
            }
            if (sel_viewtype == 4) {
                if (sel_materialtype == 0&&sel_materiallist == 0) {
                    alert("素材类型 和 素材 至少选择一项");
                    return;
                }
            }
            if (sel_viewtype == 5) {
//                if (micromallimgid == 0) {
//                    alert("请选择微商城图片");
//                    return;
//                }
            }
            var channelcompanyid = $("#hid_channelcompanyid").val();//渠道人属于的门市
            var viewchannelcompanyid = 0;//渠道人展示的门市
            if (sel_viewtype == 6) {
                if (channelcompanyid!="0") {
                    if (sel_menshi == 0) {
                        alert("请选择门市");
                        return;
                    }
                }
                viewchannelcompanyid = sel_menshi;
            }
            if (sel_viewtype == 7) {
                if (channelcompanyid != "0") {
                    if (sel_hezuo == 0) {
                        alert("请选择合作单位");
                        return;
                    }
                }
                viewchannelcompanyid = sel_hezuo;
            }
            if (sel_viewtype == 10) {
                if (sel_choujiangact == 0) {
                    alert("请选择抽奖活动");
                    return;
                }
            }

            var qrcodename = $("#hid_channelname").val() + "二维码(系统生成)";
            if ($("#hid_channelname").val()=="默认渠道")
            {
                qrcodename ="门市二维码(系统生成)";
            }

            $.post("/JsonFactory/WeiXinHandler.ashx?oper=editchannelwxqrcode", { viewchannelcompanyid: viewchannelcompanyid, id: $("#hid_qrcodeid").val(), qrcodename: qrcodename, viewtype: sel_viewtype, projectid: sel_projectlist, productid: sel_prolist, materialtype: sel_materialtype, materialid: sel_materiallist, channelcompanyid: channelcompanyid, channelid: $("#hid_channelid").val(), promoteact: sel_act, comid: $("#hid_comid").trimVal(), onlinestatus: 1, micromallimgid: micromallimgid, sel_choujiangact: sel_choujiangact }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert(data.msg);
                    return;
                }
                if (data.type == 100) {
                    alert("二维码操作成功");
                    $("#hid_qrcodeid").val(data.qrcodeid);
                    $("#img1").attr("src", data.qrcodeurl);

                }
            })

        }
        //弹出层
        function referrer_ch1(channelid,channelname,channelcompanyid, pixsize) {
//            //获取微商城图片：不存在的话默认读取 易城 设置的微商城图片
//            $.post("/JsonFactory/WeiXinHandler.ashx?oper=Getmicromallimgbycomid", {comid:$("#hid_comid").val()}, function (data2) {
//                data2 = eval("(" + data2 + ")");
//                if (data2.type == 1) { }
//                if (data2.type == 100) {
//                    $("#hid_micromallimg").val(data2.imgid);
//                    $("#headPortraitImg").attr("src",data2.imgurl);
//                }
//            })

            $("#hid_channelid").val(channelid);
            $("#hid_channelname").val(channelname);
            $("#hid_channelcompanyid").val(channelcompanyid);
            //获取渠道的二维码信息
            $.post("/JsonFactory/WeiXinHandler.ashx?oper=Getchannelwxqrcodebychannelid", { channelid: channelid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $("#img1").attr("src", "/Images/defaultThumb.png");
                }
                if (data.type == 100) {
                    $("#hid_qrcodeid").val(data.msg.Id);

                    $("#img1").attr("src", data.msg.Qrcodeurl);
                    $("#sel_viewtype").val(data.msg.qrcodeviewtype)

                    var result = Viewbyviewtype(data.msg.qrcodeviewtype);
                    if(result==true){
                        if (data.msg.qrcodeviewtype == 1) {
//                            alert(data.msg.Activityid);
                            showactlist(data.msg.Activityid);
                        }
                        else if (data.msg.qrcodeviewtype == 2) {
                            showprojectlist(data.msg.projectid);
                            showproductlist(data.msg.projectid, data.msg.Productid);
                        }
                        else if (data.msg.qrcodeviewtype == 3) {
                         
                        }
                        else if (data.msg.qrcodeviewtype == 4) {
                            showmaterialtypelist(data.msg.wxmaterialtypeid);
                            showmateriallist(data.msg.wxmaterialtypeid, data.msg.Wxmaterialid);
                        }
                        else if (data.msg.qrcodeviewtype == 5) {

                        }
                        else if (data.msg.qrcodeviewtype == 6) {
                            showmenshilist(data.msg.viewchannelcompanyid);
                        }
                        else if (data.msg.qrcodeviewtype == 7) {
                            showhezuolist(data.msg.viewchannelcompanyid);
                        }
                        else if (data.msg.qrcodeviewtype == 8) {
                            //顾问信息 暂时不处理
                        }
                        else if (data.msg.qrcodeviewtype == 10) {
                            showchoujianglist(data.msg.choujiangactid);
                        }
                        else { } 
                    }
                   
                }
            })
             
          
            $("#proqrcode_rhshow").show();
        };
       
        function closebtn() {
            $("#img1").attr("src", "/Images/defaultThumb.png")

            $("#proqrcode_rhshow").hide();
            $("#hid_channelid").val("0");
            $.each($("#table_qr tr"), function (i, item) {
                $(item).hide();
            })
            $("#table_qr tr").first().show();
            $("#table_qr tr").last().show();
            $("#sel_viewtype").val(0);
        }
         
    </script>
</asp:Content>
