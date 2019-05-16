<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/Etown.Master" CodeBehind="Financebank.aspx.cs" Inherits="ETS2.WebApp.UI.VASUI.Financebank" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
 <script type="text/javascript">
     $(function () {
         //获取商家绑定打印机信息
         $.post("/JsonFactory/FinanceHandler.ashx?oper=getpaytype", { comid: $("#hid_comid").trimVal() }, function (data) {
             data = eval("(" + data + ")");
             if (data.type == 1) {
                 $.prompt("获取信息出错，" + data.msg);
             }
             if (data.type == 100) {
                 if (data.msg == null) {
                     $.prompt("您尚未设定收款银行信息");
                     return;
                 } else {
                     $("#hid_comextid").val(data.msg.Id);
                     $("#alipay_account").val(data.msg.Alipay_account);
                     $("#alipay_id").val(data.msg.Alipay_id);
                     $("#alipay_key").val(data.msg.Alipay_key);
                     $("#bank_account").val(data.msg.Bank_account);
                     $("#bank_card").val(data.msg.Bank_card);
                     $("#bank_name").val(data.msg.Bank_name);
                     $("#userbank_account").val(data.msg.Userbank_account);
                     $("#userbank_card").val(data.msg.Userbank_card);
                     $("#userbank_name").val(data.msg.Userbank_name);
                     return;
                 }
             }
         })
         $("#confirmpub").click(function () {

             var paytype = $(':input:radio:checked').trimVal();
             var alipay_account = $("#alipay_account").val();
             var alipay_id = $("#alipay_id").val();
             var alipay_key = $("#alipay_key").val();
             var bank_account = $("#bank_account").val();
             var bank_card = $("#bank_card").val();
             var bank_name = $("#bank_name").val();
             var userbank_account = $("#userbank_account").val();
             var userbank_card = $("#userbank_card").val();
             var userbank_name = $("#userbank_name").val();

             $.post("/JsonFactory/FinanceHandler.ashx?oper=editpaybank", { alipay_account: alipay_account, alipay_id: alipay_id, alipay_key: alipay_key, bank_account: bank_account, bank_card: bank_card, bank_name: bank_name, userbank_account: userbank_account, userbank_card: userbank_card, userbank_name: userbank_name, comid: $("#hid_comid").trimVal(), id: $("#hid_comextid").trimVal() }, function (data) {
                 data = eval("(" + data + ")");
                 if (data.type == 1) {
                     $.prompt("修改收款银行出错");
                     return;
                 }
                 if (data.type == 100) {
                     $.prompt("修改收款银行成功");
                     return;
                 }
             })

         })
     })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="FinanceAll.aspx" target="" title="">收支明细</a></li>
                <li ><a href="Serverpay.aspx"  onfocus="this.blur()" target="">收款方式</a></li>
                <li class="on"><a href="Financebank.aspx"  onfocus="this.blur()" target="">收款银行</a></li>
            </ul>
        </div>

        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>收款银行</h3>


                  <table class="grid">
                    <tr>
                      <td height="31"><p align="right"><strong>支付宝收款信息</strong>： </p>                        </td>
                    </tr>
                  </table>
				   <table border="0" class="grid">
                    <tr>
                      <td height="30"><p align="right">支付宝账号： </p></td>
                      <td><p align="left">
                        <input name="alipay_account" type="text" id="alipay_account" />
                      </p></td>
                    </tr>
                    <tr>
                      <td height="30"><p align="right">合作身份ID： </p></td>
                      <td><p align="left">
                        <input name="alipay_id" type="text" id="alipay_id" />
                      </p></td>
                    </tr>
                    <tr>
                      <td height="30" align="right"><p>KEY： </p></td>
                      <td><p align="left">
                        <input name="alipay_key" type="text" id="alipay_key" />
                      </p></td>
                    </tr>
              </table>
				  
                  <table border="0" class="grid">
                    <tr>
                      <td height="31"><p align="right"><strong>银行收款信息</strong>： </p>                        </td>
                    </tr>
              </table>
                  <table border="0" class="grid">
                    <tr>
                      <td height="30"><p align="right">开户银行： </p></td>
                      <td><p align="left">
                        <input name="bank_account" type="text" id="bank_account" />
                      </p></td>
                    </tr>
                    <tr>
                      <td height="30" align="right"><p>卡号/银行账号： </p></td>
                      <td><p align="left">
                        <input name="bank_card" type="text" id="bank_card" />
                      </p></td>
                    </tr>
                    <tr>
                      <td height="30" align="right">开卡人姓名/开户单位：</td>
                      <td><input name="bank_name" type="text" id="bank_name" /></td>
                    </tr>
              </table>

                  <table border="0" class="grid">
                    <tr>
                      <td height="31" align="right"><p><strong>银行卡收款信息</strong>： </p>                        </td>
                    </tr>
                  </table>
                  <table border="0" class="grid">
                    <tr>
                      <td height="30"><p align="right">开卡银行： </p></td>
                      <td><p align="left">
                        <input name="userbank_account" type="text" id="userbank_account" />
                      </p></td>
                    </tr>
                    <tr>
                      <td height="30"><p align="right">开卡人姓名： </p></td>
                      <td><p align="left">
                        <input name="userbank_card" type="text" id="userbank_card" />
                      </p></td>
                    </tr>
                    <tr>
                      <td height="30" align="right"><p>卡号： </p></td>
                      <td><p align="left">
                        <input name="userbank_name" type="text" id="userbank_name" />
                      </p></td>
                    </tr>
              </table>
 
                <table border="0">
                  <tr>
                    <td width="600" height="80" align="center"><input type="button" name="confirmpub" id="confirmpub" value="  确认修改  " />
                        
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
