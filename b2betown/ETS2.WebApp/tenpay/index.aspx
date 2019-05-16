<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="ETS2.WebApp.tenpay.index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
    <title>财付通付款通道</title>
    <meta http-equiv="Content-Type" content="text/html; charset=GBK">
    <meta content="网上购物/网上支付/安全支付/安全购物/购物，安全/支付,安全/财付通/安全,支付/安全，购物/支付,在线/付款,收款/网上,贸易/网上贸易."
        name="description">
    <meta content="网上购物/网上支付/安全支付/安全购物/购物，安全/支付,安全/财付通/安全,支付/安全，购物/支付,在线/付款,收款/网上,贸易/网上贸易."
        name="keywords">
    <meta content="MSHTML 6.00.3790.2577" name="GENERATOR">
    <style type="text/css">
        a:link
        {
            color: #003399;
        }
        .px12
        {
            font-size: 12px;
        }
        a:visited
        {
            color: #003399;
        }
        a:hover
        {
            color: #FF6600;
        }
        .px14
        {
            font-size: 14px;
        }
        .px12hui
        {
            font-size: 12px;
            color: #999999;
        }
        .STYLE2
        {
            font-size: 14px;
            font-weight: bold;
        }
        .style1
        {
            width: 263px;
        }
    </style>
    <script language="javascript">
        function payFrm() {
            if (directFrm.order_no.value == "") {
                alert("提醒：请填写订单编号；如果无特定的订单编号，请采用默认编号！（刷新一下页面就可以了）");
                directFrm.order_no.focus();
                return false;
            }
            if (directFrm.product_name.value == "") {
                alert("提醒：请填写商品名称(付款项目)！");
                directFrm.product_name.focus();
                return false;
            }
            if (directFrm.order_price.value == "") {
                alert("提醒：请填写订单的交易金额！");
                directFrm.order_price.focus();
                return false;
            } else if (directFrm.order_price.value.split(".")[1].length > 2) {
                alert("交易金额的小数点只能有两位！");
                return false;
            }

            if (directFrm.remarkexplain.value == "") {
                alert("提醒：请填写您的简要说明！");
                directFrm.remarkexplain.focus();
                return false;
            }
            if (directFrm.remarkexplain.value.length > 31) {
                alert("提醒：超出规定的字数,请重新输入");
                event.returnValue = false;
                return false;
            }
            return true;
        }
    </script>
</head>
<body topmargin="0">
    <form id="directFrm" runat="server" onsubmit="return payFrm();">
    <div align="center">
        <table width="760" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td width="381" align="left" valign="middle">
                    <a href="https://www.tenpay.com/" target="_blank">
                        <img src="image/logo.jpg" width="537" height="145" border="0"></a>
                </td>
                <td width="379" align="right" valign="middle">
                    <font style="color: #000000; font-size: 12px;">您好，请 <a href="https://www.tenpay.com/"
                        target="_blank">注册</a> 或 <a href="https://www.tenpay.com/" target="_blank">登录</a>
                        | <a href="https://www.tenpay.com/" target="_blank">财付通首页</a></font>
                </td>
            </tr>
        </table>
        <table width="760" height="25" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                </td>
            </tr>
        </table>
        <table width="760" height="406" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td align="center" valign="top">
                    <table width="760" border="0" cellspacing="0" cellpadding="3" align="center">
                        <tr>
                            <td height="30" width="5" bgcolor="#666666">
                            </td>
                            <td width="743" height="30" bgcolor="#FF6600">
                                <font style="color: #FFFFFF; font-size: 14px;"><b>财付通快速付款通道 方便 快捷 安全</b></font>
                            </td>
                        </tr>
                    </table>
                    <table width="760" height="42" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="30">
                                <span class="STYLE2">
                                    <img src="image/arrow_02_01.gif">
                                    填写订单信息</span>
                            </td>
                        </tr>
                    </table>
                    <table width="760" border="0" cellspacing="0" cellpadding="0" align="center" height="1">
                        <tr>
                            <td width="740" bgcolor="#CCCCCC">
                            </td>
                            <td width="20">
                            </td>
                        </tr>
                    </table>
                    <table width="760" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="left">
                                <table width="760" height="30" border="0" align="left" cellpadding="0" cellspacing="1"
                                    bgcolor="#FFCC00">
                                    <tr>
                                        <td align="center" valign="top" bgcolor="#FFFFEE">
                                            <table width="760" height="351" border="0" cellpadding="6" cellspacing="0" class="px14"
                                                style="display: ">
                                                <tr>
                                                    <td height="26" align="right" valign="top" class="style1">
                                                        &nbsp;
                                                    </td>
                                                    <td valign="top">
                                                    </td>
                                                    <td width="269" rowspan="8" valign="top">
                                                        <table width="100%" border="0" align="left" cellpadding="0" cellspacing="5">
                                                            <tr>
                                                                <td height="10" align="center" valign="middle">
                                                                    <img src="image/cft.gif" width="180" height="81">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td height="24">
                                                                    <font style="color: #000000; font-size: 12px;"><b>说明1：</b></font>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td height="24">
                                                                    <font style="color: #000000; font-size: 12px;">本通道为采用财付通付款。请在支付前与本网站达成一致。</font>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td height="24">
                                                                    <font style="color: #000000; font-size: 12px;"><b>说明2：</b></font>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td height="24">
                                                                    <font style="color: #000000; font-size: 12px;">请务必确认好订单和货款后，再付款。可以在快速付款通道里的“付款概要”和“订单金额”中填入相应的订单信息。</font>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="26" align="right" valign="top" class="style1">
                                                        收款方：
                                                    </td>
                                                    <td width="353" valign="top">
                                                        <asp:Label ID="lbl_Name" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" valign="top" class="style1">
                                                        订单编号：
                                                    </td>
                                                    <td valign="top">
                                                        <asp:TextBox ID="order_no" runat="server" ReadOnly="True" Width="300px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" valign="top" class="style1">
                                                        付款项目：
                                                    </td>
                                                    <td valign="top">
                                                        <asp:TextBox ID="product_name" runat="server" Width="300px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" valign="top" class="style1">
                                                        付款金额：
                                                    </td>
                                                    <td valign="top">
                                                        <asp:TextBox ID="order_price" runat="server" onKeyUp="if(isNaN(value))execCommand('undo')"
                                                            onafterpaste="if(isNaN(value))execCommand('undo')"></asp:TextBox>
                                                        元（格式：500.01）
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="99" align="right" valign="top" class="style1">
                                                        简要说明：
                                                    </td>
                                                    <td valign="top">
                                                        <asp:TextBox ID="remarkexplain" runat="server" Columns="48" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                                        <br>
                                                        请填写您订单的简要说明（30字以内）
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" valign="top" class="style1">
                                                        &nbsp;
                                                    </td>
                                                    <td valign="top">
                                                        <b>&nbsp;<asp:ImageButton ID="ibtn_Next" runat="server" ImageUrl="image/next.gif"
                                                            OnClick="ibtn_Next_Click" style="height: 27px" />
                                                        </b>
                                                    </td>
                                                    <td valign="top">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table width="760" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table width="760" border="0" cellpadding="0" cellspacing="4" class="px12">
            <tbody>
                <tr>
                    <td width="71" rowspan="2" align="center" nowrap bgcolor="#CCCCCC" class="box-note">
                        <font class="note-help">支持<font class="note-help">银行 </font></font>
                    </td>
                    <td width="14" rowspan="2">
                    </td>
                    <td width="120">
                        <img alt="中国工商银行" src="image/icon_zggsyh_s.gif" border="0">
                    </td>
                    <td width="142">
                        <img alt="中国建设银行" src="image/icon_ccb_s.gif" border="0">
                    </td>
                    <td width="108">
                        <img alt="上海浦东发展银行" src="image/icon_spdb_s.gif" border="0">
                    </td>
                    <td width="142">
                        <img alt="招商银行" src="image/icon_zsyh_s.gif" border="0">
                    </td>
                    <td width="141">
                        <img alt="中国民生银行" src="image/icon_cmbc_s.gif" border="0">
                    </td>
                </tr>
                <tr>
                    <td>
                        <div align="left">
                            <img alt="中国农业银行" src="image/icon_abc_s.gif" border="0">
                        </div>
                    </td>
                    <td>
                        <img alt="广东发展银行" src="image/icon_gdb_s.gif" border="0">
                    </td>
                    <td>
                        <img alt="兴业银行" src="image/index_38.gif" border="0">
                    </td>
                    <td>
                        <div align="left">
                            <img alt="深圳发展银行" src="image/icon_sdb_s.gif" border="0">
                        </div>
                    </td>
                    <td>
                        <img alt="VISA" src="image/icon_visa_s.gif" border="0">
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <hr width="760" size="1">
    <table width="760" border="0" align="center" cellspacing="1" class="px12hui">
        <tbody>
            <tr>
                <td>
                    如果您点击“确认”按钮，即表示您已经接受“财付通服务协议”，同意向卖家购买 此物品。
                    <br>
                    您有责任查阅完整的物品登录资料，包括卖家的说明和接受的付款方式。卖家必须承担物品信息正确登录的 责任！
                </td>
            </tr>
        </tbody>
    </table>
    <table width="760" border="0" align="center" cellpadding="0" cellspacing="0" class="px12">
        <tbody>
            <tr align="center">
                <td class="txt12 lh15">
                    <div align="center">
                        <a href="http://www.tencent.com/" target="_blank">腾讯旗下公司</a> | 财付通 版权所有 2008
                    </div>
                </td>
            </tr>
            <tr align="center">
                <td class="txt12 lh15">
                    <div align="center">
                        <img alt="财付通通过“国际权威安全认证” " src="image/logo_vbvv.gif" border="0"><br>
                        财付通通过“国际权威安全 认证”</div>
                </td>
            </tr>
        </tbody>
    </table>
    </form>
</body>
</html>

