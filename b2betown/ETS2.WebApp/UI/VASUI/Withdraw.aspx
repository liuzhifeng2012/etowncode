<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/Etown.Master" CodeBehind="Withdraw.aspx.cs" Inherits="ETS2.WebApp.UI.VASUI.Withdraw" %>


<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
 <script type="text/javascript">
     $(function () {
         //获取商家绑定打印机信息
         $.post("/JsonFactory/FinanceHandler.ashx?oper=getpaytype", { comid: $("#hid_comid").trimVal() }, function (data) {
             data = eval("(" + data + ")");
             if (data.type == 1) {
                 $("#error-box").html("获取信息出错，" + data.msg);
             }
             if (data.type == 100) {
                 if (data.msg == null) {
                     $("#error-box").html("您尚未设定收款方，请设置并完善收款银行信息");
                     return;
                 } else {

                     $("#hid_comextid").val(data.msg.Id);
                     $("#bank_account").val(data.msg.Bank_account);
                     $("#bank_card").val(data.msg.Bank_card);
                     $("#bank_name").val(data.msg.Bank_name);
                     return;
                 }
             }
         })

         $("#confirm").click(function () {
             var bank_account = $("#bank_account").val();
             var bank_card = $("#bank_card").val();
             var bank_name = $("#bank_name").val();
             var money = changeTwoDecimal(ismoney($("#Money").val()));
             var imprest = changeTwoDecimal($("#Imprest").val());
             if (money == "") {
                 $("#error-box").html("请填写提现金额");
                 $("#error-box").show();
                 return;
             }
             if (!$("#Money").Amount()) {
                 $("#error-box").html('提现金额格式不对');
                 return;
             }
             if (money <= 0) {
                 $("#error-box").html("提现金额错误，请重新填写");
                 $("#error-box").show();
                 return;
             }
             if (money > imprest) {
                 $("#error-box").html("您提现的金额大于可提现金额，请重新填写");
                 $("#error-box").show();
                 return;
             }

             $.post("/JsonFactory/FinanceHandler.ashx?oper=Withdraw", { money: money, bank_account: bank_account, bank_card: bank_card, bank_name: bank_name, comid: $("#hid_comid").trimVal(), id: $("#hid_comextid").trimVal() }, function (data) {
                 data = eval("(" + data + ")");
                 if (data.type == 1) {
                     $("#error-box").val("提现申请出错！");
                     return;
                 }
                 if (data.type == 100) {
                     location.href = "/ui/vasui/FinanceAll.aspx";
                 }
             })

         })
     })
     function ismoney(value) {
         var mny = /^[1-9]d*.d*|0.d*[1-9]d*$/;
         if (mny.test(value)) {
             return value;
         } else {
             return 0;
         }


     }

     function changeTwoDecimal(floatvar) {
         var f_x = parseFloat(floatvar);
         if (isNaN(f_x)) {
             alert('参数传递错误');
             return false;
         }
         var f_x = Math.round(floatvar * 100) / 100;
         return f_x;
     }


    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <%--<div id="secondary-tabs" class="navsetting ">
            <ul>
                <li ><a href="FinanceAll.aspx" target="" title="">收支明细</a></li>
                <li class="on"><a href="Withdraw.aspx"  onfocus="this.blur()" target="">账户提现</a></li>
                <li ><a href="Withdraw_oldlist.aspx"  onfocus="this.blur()" target="">历史提现记录</a></li>
                <li><a href="Serverpay.aspx"  onfocus="this.blur()" target="">收款设置</a></li>
                <li><a href="PaySet.aspx"  onfocus="this.blur()" target="">网上支付收款设置</a></li>
            </ul>
        </div>--%>

        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                     </h3>
                    <div id="error-box" class="error-box"></div>
<h3></h3>
               <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
                   <h2 class="p-title-area">提现申请</h2>
                   <div class="mi-form-item">
                        <label class="mi-label"> 账户余额(可提现金额)</label>
                       <input name="Imprest" type="text" id="Imprest"  size="25" value="<%=imprest%>" class="mi-input" style="width:100px;background-color:#efefef;"  disabled="disabled"/>
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 提现金额</label>
                       <input name="Money" type="text" id="Money"  size="25"  class="mi-input"  style="width:100px;" /> 元
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 开户银行</label>
                       <input name="bank_account" type="text" id="bank_account"  size="25"  class="mi-input"  style="width:500px;background-color:#efefef;"  disabled="disabled"  />
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 卡号/银行账号</label>
                       <input name="bank_card" type="text" id="bank_card"  size="25"  class="mi-input"  style="width:300px;background-color:#efefef;"  disabled="disabled"  />
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 开卡人姓名/开户单位：</label>
                       <input name="bank_name" type="text" id="bank_name"  size="25"  class="mi-input"  style="width:500px;background-color:#efefef;"  disabled="disabled"  />
                   </div>
                   <div class="mi-form-explain"></div>
               </div>

                <table border="0">
                  <tr>
                    <td width="600" height="80" align="center"><input type="button" name="confirm" id="confirm"  class="mi-input" value="  确认提现  " />
                        
                    </td>
                  </tr>
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
