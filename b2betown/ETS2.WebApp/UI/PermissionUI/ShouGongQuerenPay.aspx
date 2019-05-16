<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ShouGongQuerenPay.aspx.cs" Inherits="ETS2.WebApp.UI.PermissionUI.ShouGongQuerenPay" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {

            $("#Enter").click(function () {

                var trade_no = $("#trade_no").trimVal();
                var order_no = $("#order_no").trimVal();
                var total_fee = $("#total_fee").trimVal();
                var sub = $("#sub").trimVal();
                //防止重复提交
                if (sub == 0) {
                    $("#sub").val(1);
                    if (trade_no == "" || order_no == "" || total_fee == "") {
                        $.prompt("参数传递错误");
                        $("#sub").val(0);
                        return;
                    }


                    $.post("/JsonFactory/FinanceHandler.ashx?oper=shougongqueren", { trade_no: trade_no, order_no: order_no, total_fee: total_fee }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $.prompt(data.msg);
                            $("#sub").val(0);
                            return;
                        }
                        if (data.type == 100) {
                            $.prompt("处理成功");
                            return;
                        }
                    })
                }
            })



        })

        //----此部分是行业部分END---//
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <%--<li><a href="FuncModuleList.aspx" onfocus="this.blur()" target=""><span>权限管理</span></a></li>
                <li><a href="RoleList.aspx" onfocus="this.blur()" target=""><span>管理组管理</span></a></li>--%>
                <li><a href="MasterList.aspx" onfocus="this.blur()" target=""><span>人员管理</span></a></li>
                <%-- <li><a href="notelist.aspx" onfocus="this.blur()" target=""><span>短信管理</span></a></li>--%>
                <li><a href="SSort.aspx" onfocus="this.blur()" target=""><span>商户管理</span></a></li>
                <li><a href="AgentList.aspx" onfocus="this.blur()" target=""><span>分销商管理</span></a></li>
                <li><a href="ComFinance.aspx" onfocus="this.blur()" target=""><span>财务对账表</span></a></li>
                <li><a href="Withdraw_handle.aspx" onfocus="this.blur()" target=""><span>提现财务管理</span></a></li>
                <li><a href="ticketlist.aspx" onfocus="this.blur()"><span>退票管理</span></a></li>
                <!--<li><a href="bankmanager.aspx" onfocus="this.blur()"><span>绑定银行管理</span></a></li>-->
                <li><a href="Modellist.aspx" onfocus="this.blur()" target=""><span>模板管理</span></a></li>
                <li><a href="ProClass.aspx" onfocus="this.blur()" target=""><span>产品类目</span></a></li>
                <li><a href="/weixin/WxTemplateManage.aspx" onfocus="this.blur()" target="">微信模板管理</a></li>
                <li class="on"><a href="ShouGongQuerenPay.aspx" onfocus="this.blur()" target="">手工支付确认</a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3 id="h3groups">
                    手工确认支付：
                </h3>
            </div>
            <table class="grid">
                    <tr>
                        <td height="24" align="right">
                            支付接口ID：
                        </td>
                        <td>
                            <input name="trade_no" type="text" id="trade_no" size="24" />
                        </td>
                    </tr>
                    <tr>
                        <td height="24" align="right">
                            支付本系统订单ID：
                        </td>
                        <td>
                            <input name="order_no" type="text" id="order_no" size="24" />
                        </td>
                    </tr>
                    <tr>
                        <td height="24" align="right">
                            支付金额：
                        </td>
                        <td>
                            <input name="total_fee" type="text" id="total_fee" size="24" />
                        </td>
                    </tr>
                    <tr>
                        <td height="80" colspan="2" align="center">
                            <input type="button" name="button" id="Enter" value="  确认支付成功  " /><br />
                        此功能只对已经支付成功而系统未接收到返回处理的进行操作<br />确认后，客户订单会确认成功，并发送电子码，商户会增加此笔支付的财务记录。
                        </td>
                    </tr>
                </table>

        </div>
    </div>

    <input type="hidden" id="sub" value="0" />
</asp:Content>

