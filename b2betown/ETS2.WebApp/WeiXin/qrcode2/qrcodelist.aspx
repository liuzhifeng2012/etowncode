<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="qrcodelist.aspx.cs" MasterPageFile="/UI/Etown.Master"
    Inherits="ETS2.WebApp.WeiXin.qrcode2.qrcodelist" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#cancel_rh").click(function () {
                $("#imgg").attr("src", "")
                $("#rhshow").hide();
            })
        })
        //弹出二维码大图
        function referrer_ch(qrcodeurl, pixsize) {
            //            $("#span_rh").text("二维码图片");
            $("#imgg").attr("src", qrcodeurl).attr("height", pixsize).attr("width", pixsize);
            $("#rhshow").show();
        };

        function editqrcode(qrcodeid) {
            window.open("qrcodemanager.aspx?qrcodeid=" + qrcodeid, target = "_self");
        }
        function viewscanrecord(qrcodeid) {
            window.open("qrcodeuserlist.aspx?subscribesourceid=" + qrcodeid, target = "_self");
        }
        
    </script>
    <script type="text/javascript" src="/Scripts/jquery.cookie.2.2.0.min.js"></script>
    <script type="text/javascript">
        $(function () {
            if ($.cookie($("#hid_userid").val() + "_IsViewMd2")) {
                //展开门店的Id
                var seledid = $.cookie($("#hid_userid").val() + "_IsViewMd2");

                var array = seledid.split(',');
                for (var i = 0; i < array.length; i++) {
                    $("#div" + array[i]).css("display", "block");
                    $("#btnShowHidden" + array[i]).html("隐藏二维码列表");
                }

            }
        })
        /*隐藏嵌套的Gridview*/
        function ShowHidden(sid) {

            var str = $.cookie($("#hid_userid").val() + "_IsViewMd2");


            if ($("#div" + sid).css("display") == "none") {//展开门店操作
                $("#div" + sid).css("display", "block");
                $("#btnShowHidden" + sid).html("隐藏二维码列表");
                if (str == null) {
                    $.cookie($("#hid_userid").val() + "_IsViewMd2", sid, { path: '/' }); //保存展开门店的id

                } else {
                    $.cookie($("#hid_userid").val() + "_IsViewMd2", str + "," + sid, { path: '/' }); //保存展开门店的id

                }
            } else {//关闭门店操作

                $("#div" + sid).css("display", "none");
                $("#btnShowHidden" + sid).html("展开二维码列表");

                if (str == null) {
                    $.cookie($("#hid_userid").val() + "_IsViewMd2", null, { path: '/' }); //保存展开门店的id

                } else {
                    var array = $.cookie($("#hid_userid").val() + "_IsViewMd2").split(',');
                    if (array.length == 1) {
                        str = str.replace(sid, "");
                    } else {
                        str = str.replace("," + sid, "");
                    }
                    if (str == "") {
                        $.cookie($("#hid_userid").val() + "_IsViewMd2", null, { path: '/' }); //保存展开门店的id

                    } else {
                        $.cookie($("#hid_userid").val() + "_IsViewMd2", str, { path: '/' }); //保存展开门店的id

                    }
                }
            }
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

      <script type="text/javascript">
          jQuery.extend({ alertWindow: function (e, t, n) {
              var e = e, t = t, r; n === undefined ? r = "#FF7C00" : r = n; if ($("body").find(".alertWindow1").length === 0) {
                  var i = '<div  class="alertWindow1" style="width: 100%;height: 100%; background:rgba(0,0,0,0.5);position: fixed; left:0px; top: 0px; z-index: 9999;"><div  style="width: 400px; height: 200px;background: #FFF;margin: 180px auto;border: 2px solid #CFCFCF; border-bottom: 10px solid ' + r + ';">' + '<div  style="width: inherit;height: 20px;">' + '<div class="alertWindowCloseButton1" style="float: right; width: 10px; height: 20px;margin-right:5px;font-family:\'microsoft yahei\';color:' + r + ';cursor: pointer;">X</div>' + "</div>" + '<h1 class="alertWindowTitle" style="margin-top:20px;text-align:center;font-family:\'\';font-size: 18px;font-weight: normal;color: ' + r + ';">' + e + "</h1>" + '<div class="alertWindowContent" style="width:360px;px;height: 60px;padding-left:20px;padding-right:20px;text-align:center;font-size: 15px;color: #7F7F7F;">' + t + "</div>" + '<p><input class="alertWindowCloseSure1" type="button" value="确 定" style="width: 100px;height: 50px;background:' + r + ';border:none;position: relative;bottom: 18px;font-size:18px;color:#FFFFFF;-webkit-border-radius: 10px;-moz-border-radius: 10px;border-radius: 10px;cursor: pointer;"></p>' + "</div>" + "</div>"; $("body").append(i); var s = $(".alertWindow1");
                  $(".alertWindowCloseButton1").click(function () {
                      //                   s.hide() 
                      window.location.href = "/manage.aspx";
                  }),
                $(".alertWindowCloseSure1").click(function () {
                    //                    s.hide()
                    window.location.href = "/manage.aspx";
                })
              } else $(".alertWindowTitle").text(e), $(".alertWindowContent").text(t), $(".alertWindow1").show()
          }
          })
   </script>
    <style type="text/css">
        /*弹出层的样式*/
        .alertWindowContent h1, p
        {
            text-align: center;
            font-size: 18px;
            font-weight: bolder;
        }
        .alertWindowContent input
        {
            width: 100px;
            height: 50px;
            cursor: pointer;
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
            border-radius: 5px;
        }
    </style>
    <script>
        $(function () {
          if(<%=isrenzheng %>==0){
            $("#viewrenzheng").show();
            //            jQuery.alertWindow("标题设置", "内容设置");
            //jQuery.alertWindow("该功能需将微信服务号进行认证后才能使用", "");
                
          }
        });
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <%--<div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="qrcodeuserlist.aspx" onfocus="this.blur()">扫码实时记录</a></li>
                <%
                    if (isqudao == "1")
                    {
                %>
                <li class="on"><a href="qrcodelist.aspx?isqudao=1" onfocus="this.blur()">渠道二维码列表</a></li>
                <li><a href="qrcodelist.aspx?isqudao=0" onfocus="this.blur()">活动二维码列表</a></li>
                <%
                    }
                    else
                    {
                %>
                <li><a href="qrcodelist.aspx?isqudao=1" onfocus="this.blur()">渠道二维码列表</a></li>
                <li class="on"><a href="qrcodelist.aspx?isqudao=0" onfocus="this.blur()">活动二维码列表</a></li>
                <%
                    } %>
                <li><a href="qrcodemanager.aspx" onfocus="this.blur()">二维码添加</a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <%-- <a href="#" onclick="viewqrcodes('0,1')">显示全部</a>&nbsp;&nbsp;<a href="#" onclick="viewqrcodes('1')">显示使用</a>&nbsp;&nbsp;<a
                    href="#" onclick="viewqrcodes('0')">显示暂停</a>--%>
                <table>
                    <tr>
                        <td style="padding: 0 10px 20px 10px;">
                            <%
                                if (isqudao == "1")
                                {
                            %>
                            请按门市,姓名,手机查询
                            <%
                                }
                                else
                                {
                            %>
                            活动名称查询
                            <%
                                } %>
                            <asp:TextBox ID="txtkey" runat="server" Style="height: 20px;"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="Btnsearch" runat="server" Text="查 询" OnClick="Btnsearch_Click" class="button blue" />
                        </td>
                    </tr>
                </table>
                <div style=" width:600px; height:40px; padding-left:10px; border: 1px solid #FF4444; display:none;" id="viewrenzheng" >

                            <div  style="padding:12px 0 14px 0;">
                                <h5 class="text-overflow">
                                   <a smartracker="on" seed="contentList-mainLinkbox" href="http://shop1143.etown.cn/v/about.aspx?id=2661" target="_blank" style=" font-size:16px"> 该功能需将微信服务号进行认证后才能使用 ,点击查看如何进行认证... </a></h5>
                            </div>
                            <div class="content-list-des text-overflow">
                            </div>
                </div>

                <asp:GridView ID="GridView2" runat="server" Width="1000px" AllowPaging="True" AutoGenerateColumns="False"
                    OnRowDataBound="GridView2_RowDataBound" OnPageIndexChanging="GridView2_PageIndexChanging"
                    PageSize="15">
                    <RowStyle ForeColor="#4A3C8C" CssClass="gvRow" />
                    <HeaderStyle BackColor="#C1D9F3" Font-Bold="True" ForeColor="#000000" CssClass="gvHeader" />
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="编号">
                            <HeaderStyle Width="5%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="title" HeaderText="公司名称">
                            <HeaderStyle Width="25%" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="扫码总数">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <%# GetScanTotal2(Eval("ID").ToString(),isqudao)%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="关注总数">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <%# GetWxTotal2(Eval("id").ToString(), isqudao, Eval("issuetype").ToString())%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="取消关注总数">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <%# GetQxWxTotal2(Eval("id").ToString(),isqudao) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="状态">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <%#Runstatus(Eval("companystate").ToString())%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle Width="40%" />
                            <ItemTemplate>
                                <!---点击用于列表展开，执行JS函数--->
                                <span id="btnShowHidden<%#Eval("ID") %>" style="color: Red; cursor: pointer; margin: 0 0 0 0"
                                    onclick="ShowHidden('<%#Eval("ID") %>')">展开二维码列表</span>
                                <tr>
                                    <td colspan="100%">
                                        <div id="div<%#Eval("ID") %>" style="display: none;">
                                            <%-- <div style="float: left; font-size: small">
                                                └</div>--%>
                                            <div style="border: 1 solid RGB(40,80,150); position: relative; left: 0px; overflow: auto;
                                                width: 100%;">
                                                <!---绑定内层Gridview--->
                                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("id") %>' />
                                                <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" Width="100%"
                                                    Height="100%" AllowPaging="True" OnPageIndexChanging="GridView3_PageIndexChanging"
                                                    OnRowDataBound="GridView3_RowDataBound" PageSize="10">
                                                    <RowStyle ForeColor="#4A3C8C" CssClass="gvRow2" />
                                                    <HeaderStyle BackColor="#C1D9F3" Font-Bold="True" ForeColor="#000000" CssClass="gvHeader2" />
                                                    <EmptyDataTemplate>
                                                        <table width="100%" style="height: 30px;">
                                                            <tr>
                                                                <td align="center">
                                                                    当前渠道单位还未创建带参二维码!
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:BoundField DataField="id" HeaderText="编号">
                                                            <HeaderStyle Width="5%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="title" HeaderText="二维码名称">
                                                            <HeaderStyle Width="18%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="渠道单位">
                                                            <HeaderStyle Width="16%" />
                                                            <ItemTemplate>
                                                                <%# GetChannelUnitNameById(Eval("channelcompanyid").ToString()) %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="渠道人">
                                                            <HeaderStyle Width="5%" />
                                                            <ItemTemplate>
                                                                <%# GetChannelNameById(Eval("channelid").ToString()) %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="活 动">
                                                            <HeaderStyle Width="10%" />
                                                            <ItemTemplate>
                                                                <%# GetActivityNameById(Eval("activityid").ToString()) %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="createtime" HeaderText="生成时间" DataFormatString="{0:yyyy/MM/dd}">
                                                            <HeaderStyle Width="7%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="扫码总数">
                                                            <HeaderStyle Width="5%" />
                                                            <ItemTemplate>
                                                                <%# GetScanTotal(Eval("id").ToString()) %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="关注总数">
                                                            <HeaderStyle Width="5%" />
                                                            <ItemTemplate>
                                                                <%# GetWxTotal(Eval("id").ToString()) %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="取消关注总数">
                                                            <HeaderStyle Width="5%" />
                                                            <ItemTemplate>
                                                                <%# GetQxWxTotal(Eval("id").ToString()) %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="二维码图片">
                                                            <HeaderStyle Width="8%" />
                                                            <ItemTemplate>
                                                                <span id="span1" style="float: right; color: Red; cursor: pointer; margin: 0 0 0 0"
                                                                    onclick="referrer_ch('<%#Eval("qrcodeurl") %>',224)">小图片</span> <span id="span2"
                                                                        style="float: right; color: Red; cursor: pointer; margin: 0 0 0 0" onclick="referrer_ch('<%#Eval("qrcodeurl") %>',420)">
                                                                        大图片</span>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="使用状态">
                                                            <HeaderStyle Width="5%" />
                                                            <ItemTemplate>
                                                                <%# Onlinestatus(Eval("onlinestatus").ToString())%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle Width="11%" />
                                                            <ItemTemplate>
                                                                <span id="span1" style="color: Red; cursor: pointer; margin: 0 0 0 0" onclick="viewscanrecord('<%#Eval("id") %>')">
                                                                    扫码记录</span> &nbsp;&nbsp;<%--<span id="span2" style="color: Red; cursor: pointer; margin: 0 0 0 0"
                                                                        onclick="editqrcode('<%#Eval("id") %>')"> 编辑</span>--%>
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
                <asp:HiddenField ID="hid_key" runat="server" Value="" />
                <div id="rhshow" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px auto;
                    display: none; left: 20%; position: absolute; top: 20%;">
                    <table width="500px" border="0" cellpadding="10" cellspacing="1" style="padding: 5px;">
                        <tr>
                            <td colspan="2" align="center">
                                <img src="" id="imgg" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                                <label>
                                    *二维码尺寸请按照43像素的整数倍缩放，以保持最佳效果</label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                                <input name="cancel_rh" id="cancel_rh" type="button" class="formButton" value="  关 闭  " />
                            </td>
                        </tr>
                    </table>
                </div>
</asp:Content>
