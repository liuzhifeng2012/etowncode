<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ObtainGZList.aspx.cs" MasterPageFile="/UI/Etown.Master"
    Inherits="ETS2.WebApp.Excel.ObtainGZList" EnableEventValidation="false" %>

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
      <%--  <div id="secondary-tabs" class="navsetting ">
            <ul>
  
                <li><a href="/ui/crmui/BusinessCustomersList.aspx?isactivate=0" onfocus="this.blur()">
                    未激活用户列表</a></li>
                <%
                    if (IsParentCompanyUser == true)
                    {
                %>
                <li><a href="/excel/ImportExcel.aspx" onfocus="this.blur()">导入历史会员信息</a></li>
                <li class="on"><a href="/excel/ObtainGZList.aspx" onfocus="this.blur()">导入已有微信粉丝</a></li>

                <%} %>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">

            <div style=" width:600px; height:40px; padding-left:10px; border: 1px solid #FF4444; display:none;" id="viewrenzheng" >

                            <div  style="padding:12px 0 14px 0;">
                                <h5 class="text-overflow">
                                   <a smartracker="on" seed="contentList-mainLinkbox" href="http://shop1143.etown.cn/v/about.aspx?id=2661" target="_blank" style=" font-size:16px"> 该功能需将微信服务号进行认证后才能使用 ,点击查看如何进行认证... </a></h5>
                            </div>
                            <div class="content-list-des text-overflow">
                            </div>
                </div>

                <table>
                    <tr>
                        <td height="24" colspan="2">
                            <h3>
                                导入公司名称:<%=comname %></h3>
                        </td>
                    </tr>
                    <tr>
                        <td height="24" colspan="2">
                            <h3>
                                第一步：获取微信关注用户列表</h3>
                        </td>
                    </tr>
                    <tr>
                        <td height="24" colspan="2">
                            <asp:Button ID="Button1" runat="server" Text="获取微信关注用户列表" OnClick="Button1_Click"
                                class="button blue" />
                            <asp:Literal ID="Literal1" runat="server" />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td height="24" colspan="2">
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="id" HeaderText="编号" />
                                    <asp:BoundField DataField="total" HeaderText="总数量" />
                                    <asp:BoundField DataField="count" HeaderText="读取数量" />
                                    <asp:BoundField DataField="nextopenid" HeaderText="下一次调用OpenId" />
                                    <asp:BoundField DataField="obtaintime" HeaderText="读取时间" />
                                    <asp:BoundField DataField="errcode" HeaderText="错误编码" />
                                    <asp:BoundField DataField="errmsg" HeaderText="错误描述" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td height="24" colspan="2">
                            <h3>
                                第二步：导入列表并生成会员</h3>
                        </td>
                    </tr>
                    <tr>
                        <td height="24" colspan="2">
                            <asp:Button ID="Button2" runat="server" Text="导入用户列表" OnClick="Button2_Click" class="button blue" />
                            <asp:Literal ID="Literal2" runat="server" />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td height="24" colspan="2">
                            <h4>
                                最近一次导入记录</h4>
                        </td>
                    </tr>
                    <tr>
                        <td height="24" colspan="2">
                            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False">
                                <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" CssClass="gvRow" />
                                <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" CssClass="gvHeader" />
                                <AlternatingRowStyle BackColor="#F7F7F7" CssClass="gvAlternatingRow" />
                                <Columns>
                                    <asp:BoundField DataField="id" HeaderText="编   号" />
                                    <asp:BoundField DataField="total" HeaderText="总 数 量" />
                                    <asp:BoundField DataField="splitcount" HeaderText="导 入 数 量" />
                                    <asp:BoundField DataField="splittime" HeaderText="拆分时间" Visible="False" />
                                    <asp:BoundField DataField="splitno" HeaderText="拆分次数" Visible="False" />
                                    <asp:TemplateField HeaderText="操 作1" Visible="False">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" Text='下载Excel' runat="server" NavigateUrl='<%# "~/Excel/DownExcel.aspx?splitid="+Eval("id") %>'></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <br />
                                            <asp:Label ID="Label1" runat="server"></asp:Label>
                                            <asp:Button ID="Button3" runat="server" CommandArgument='<%# Eval("Id") %>' OnClick="Button3_Click"
                                                Text="生成会员" class="button blue" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    
</asp:Content>
