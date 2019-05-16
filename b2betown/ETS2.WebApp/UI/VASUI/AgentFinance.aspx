<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/Etown.Master" CodeBehind="AgentFinance.aspx.cs" Inherits="ETS2.WebApp.UI.VASUI.AgentFinance" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="FinanceAll.aspx" target="" title="">收支明细</a></li>
                <li class="on"><a href="AgentFinance.aspx"  onfocus="this.blur()" target="">分销财务汇总</a></li>
                <li ><a href="Withdraw.aspx"  onfocus="this.blur()" target="">账户提现</a></li>
                 <li><a href="Withdraw_oldlist.aspx"  onfocus="this.blur()" target="">历史提现记录</a></li>
                <li ><a href="Serverpay.aspx"  onfocus="this.blur()" target="">收款设置</a></li>
            </ul>
        </div>

        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    分销财务</h3>

                <table  class="grid">

                               <tr>
                                 <td width="160" height="30"><p align="right">分销总预付款金额： </p></td>
                                 <td><p align="left">
                                     
                                 </p></td>
                               </tr>
                               <tr>
                                 <td height="30" align="right"><p>分销今日销售扣减/验票扣减金额： </p></td>
                                 <td><p align="left">
                                    
                                 </p></td>
                               </tr>
                               <tr>
                                 <td height="30" align="right">分销今日充值金额：</td>
                                 <td>
                                 
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
    </div>
    <input type="hidden" id="hid_comextid" value="" />
    <div class="data">
    </div>
</asp:Content>
