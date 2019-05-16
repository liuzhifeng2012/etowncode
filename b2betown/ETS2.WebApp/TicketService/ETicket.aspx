<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ETicket.aspx.cs" Inherits="ETS2.WebApp.TicketService.ETicket" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>电子票</title>
    <%-- <style type="text/css">
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
    </style>--%>
    <style>
        *
        {
            margin: 0;
            padding: 0;
        }
        ul, ol
        {
            list-style: none;
        }
        .title
        {
            color: #ADADAD;
            font-size: 14px;
            font-weight: bold;
            padding: 8px 16px 5px 10px;
        }
        .hidden
        {
            display: none;
        }
        
        .new-btn-login-sp
        {
            border: 1px solid #D74C00;
            padding: 1px;
            display: inline-block;
        }
        
        .new-btn-login
        {
            background-color: transparent;
            background-image: url("images/new-btn-fixed.png");
            border: medium none;
        }
        .new-btn-login
        {
            background-position: 0 -198px;
            width: 82px;
            color: #FFFFFF;
            font-weight: bold;
            height: 28px;
            line-height: 28px;
            padding: 0 10px 3px;
        }
        .new-btn-login:hover
        {
            background-position: 0 -167px;
            width: 82px;
            color: #FFFFFF;
            font-weight: bold;
            height: 28px;
            line-height: 28px;
            padding: 0 10px 3px;
        }
        .bank-list
        {
            overflow: hidden;
            margin-top: 5px;
        }
        .bank-list li
        {
            float: left;
            width: 153px;
            margin-bottom: 5px;
        }
        
        #main
        {
            width: 750px;
            margin: 0 auto;
            font-size: 14px;
            font-family: '宋体';
        }
        #logo
        {
            background-color: transparent;
            background-image: url("images/new-btn-fixed.png");
            border: medium none;
            background-position: 0 0;
            width: 166px;
            height: 35px;
            float: left;
        }
        .red-star
        {
            color: #f00;
            width: 10px;
            display: inline-block;
        }
        .null-star
        {
            color: #fff;
        }
        .content
        {
            margin-top: 5px;
        }
        
        .content dt
        {
            width: 160px;
            display: inline-block;
            text-align: right;
            float: left;
        }
        .content dd
        {
            margin-left: 100px;
            margin-bottom: 5px;
        }
        #foot
        {
            margin-top: 10px;
        }
        .foot-ul li
        {
            text-align: center;
        }
        .note-help
        {
            color: #999999;
            font-size: 12px;
            line-height: 130%;
            padding-left: 3px;
        }
        
        .cashier-nav
        {
            font-size: 14px;
            margin: 15px 0 10px;
            text-align: left;
            height: 30px;
            border-bottom: solid 2px #CFD2D7;
        }
        .cashier-nav ol li
        {
            float: left;
        }
        .cashier-nav li.current
        {
            color: #AB4400;
            font-weight: bold;
        }
        .cashier-nav li.last
        {
            clear: right;
        }
        .alipay_link
        {
            text-align: right;
        }
        .alipay_link a:link
        {
            text-decoration: none;
            color: #8D8D8D;
        }
        .alipay_link a:visited
        {
            text-decoration: none;
            color: #8D8D8D;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <%--<div>
        <div id="settings" class="view main">
            <div id="secondary-tabs" class="navsetting ">
                <ul>
                    <li><a href="product.aspx" onfocus="this.blur()">产品列表</a></li>
                    <li><a href="producttype.aspx" onfocus="this.blur()">产品类型列表</a></li>
                    <li><a href="ETicket.aspx" onfocus="this.blur()">电子票测试</a></li>
                </ul>
            </div>
            <div id="setting-home" class="vis-zone">
                <div class="inner">
                    <table>
                        <tr>
                            <td height="24" colspan="2">
                                <h3>
                                    对接接口名称:阳光</h3>
                            </td>
                        </tr>
                        <tr>
                            <td height="24" colspan="2">
                                <asp:Button ID="Button1" runat="server" Text="发送电子票" class="button blue" OnClick="Button1_Click" />
                                <asp:Literal ID="Literal1" runat="server" />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td height="24" colspan="2">
                                <asp:Button ID="Button2" runat="server" Text="查询电子票" class="button blue" OnClick="Button2_Click" />
                                <asp:Literal ID="Literal2" runat="server" />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td height="24" colspan="2">
                                <asp:Button ID="Button3" runat="server" Text="撤销电子票" class="button blue" OnClick="Button3_Click" />
                                <asp:Literal ID="Literal3" runat="server" />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td height="24" colspan="2">
                                <asp:Button ID="Button4" runat="server" Text="重新发送电子票" class="button blue" OnClick="Button4_Click" />
                                <asp:Literal ID="Literal4" runat="server" />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td height="24" colspan="2">
                                <asp:Button ID="Button5" runat="server" Text="转发电子票" class="button blue" OnClick="Button5_Click" />
                                <asp:Literal ID="Literal5" runat="server" />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td height="24" colspan="2">
                                <asp:Button ID="Button6" runat="server" Text="电子票验证同步" class="button blue" />
                                <asp:Literal ID="Literal6" runat="server" />
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
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>--%>
    <div id="main"  style="display:none;">
        <div id="head">
            <dl class="alipay_link">
                <a target="_blank" href="http://www.alipay.com/"><span>支付宝首页</span></a>| <a target="_blank"
                    href="https://b.alipay.com/home.htm"><span>商家服务</span></a>| <a target="_blank" href="http://help.alipay.com/support/index_sh.htm">
                        <span>帮助中心</span></a>
            </dl>
            <span class="title">支付宝即时到账批量退款有密接口快速通道</span>
        </div>
        <div class="cashier-nav">
            <ol>
                <li class="current">1、确认信息 →</li>
                <li>2、点击确认 →</li>
                <li class="last">3、确认完成</li>
            </ol>
        </div>
        <div id="body" style="clear: left; display:none;">
            <dl class="content">
                <dt>订单号：</dt>
                <dd>
                    <span class="null-star">*</span>
                    <asp:TextBox ID="Orderid" name="Orderid" runat="server"></asp:TextBox>
                    <span>必填 </span>
                </dd>
                <dt>退款金额：</dt>
                <dd>
                    <span class="null-star">*</span>
                    <asp:TextBox ID="Quitfee" name="Quitfee" runat="server"></asp:TextBox>
                    <span>必填 </span>
                </dd>
                <dt></dt>
                <dd>
                    <span class="new-btn-login-sp">
                        <asp:Button ID="BtnAlipay" name="BtnAlipay" Text="确 认" Style="text-align: center;"
                            runat="server" OnClick="BtnAlipay_Click" /></span>
                    <label runat="server" id="lblresult">
                    </label>
                </dd>
            </dl>
            <hr />
            <dl class="content">
                <dt>订单号：</dt>
                <dd>
                    <span class="null-star">*</span>
                    <asp:TextBox ID="qunar_order"   runat="server"></asp:TextBox>
                    <span>必填 </span>
                </dd>
                <dd>
                    <span class="new-btn-login-sp">
                        <asp:Button ID="Button1"   Text="去哪儿消费通知" Style="text-align: center;"
                            runat="server" onclick="Button1_Click"   /></span>
                    <label runat="server" id="Label222">
                    </label>
                </dd>
            </dl>
        </div>
        <div id="foot">
            <ul class="foot-ul">
                <li><font class="note-help">如果您点击“确认”按钮，即表示您同意该次的执行操作。 </font></li>
                <li>支付宝版权所有 2011-2015 ALIPAY.COM </li>
            </ul>
            <ul>
        </div>
    </div>
    </form>
</body>
</html>
