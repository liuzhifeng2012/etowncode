<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/Etown.Master" CodeBehind="Serverpay.aspx.cs" Inherits="ETS2.WebApp.UI.VASUI.Serverpay" %>

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
                     $.prompt("您尚未设定收款方，请设置并完善收款银行信息");
                     return;
                 } else {

                     if (data.msg.Paytype == 1) {
                         $("#paytype1").attr("checked", 'checked');
                     }
                     if (data.msg.Paytype == 2) {
                         $("#paytype2").attr("checked", 'checked');
                     }
                     $("#hid_comextid").val(data.msg.Id);
                     $("#alipay_account").val(data.msg.Alipay_account);
                     $("#alipay_id").val(data.msg.Alipay_id);
                     $("#alipay_key").val(data.msg.Alipay_key);
                     $("#bank_account").val(data.msg.Bank_account);
                     $("#bank_card").val(data.msg.Bank_card);
                     $("#bank_name").val(data.msg.Bank_name);
                     
                     if (data.msg.Uptype==1) {
                         $("#bank_account").attr("disabled", "disabled");
                         $("#bank_card").attr("disabled", "disabled");
                         $("#confirmpub").hide();
                         $("#bank_name").attr("disabled", "disabled");
                         document.getElementById("type").checked = true;
                     }
                     return;
                 }
             }
         })
         $("#confirmpub").click(function () {
             var alipay_account = $("#alipay_account").val();
             var alipay_id = $("#alipay_id").val();
             var alipay_key = $("#alipay_key").val();
             var bank_account = $("#bank_account").val();
             var bank_card = $("#bank_card").val();
             var bank_name = $("#bank_name").val();
             var paytype = $(':input:radio:checked').trimVal();
             var type = document.getElementById("type").checked;
             $.post("/JsonFactory/FinanceHandler.ashx?oper=editpaybank", { type: type, paytype: paytype, alipay_account: alipay_account, alipay_id: alipay_id, alipay_key: alipay_key, bank_account: bank_account, bank_card: bank_card, bank_name: bank_name, comid: $("#hid_comid").trimVal(), id: $("#hid_comextid").trimVal() }, function (data) {
                 data = eval("(" + data + ")");
                 if (data.type == 1) {
                     $.prompt("修改收款方式出错");
                     return;
                 }
                 if (data.type == 100) {
                     $.prompt("修改收款方式成功");
                     location.reload();
                     return;
                 }
             })

         })
     })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="FinanceAll.aspx" target="" title="">收支明细</a></li>
                <li ><a href="Withdraw.aspx"  onfocus="this.blur()" target="">账户提现</a></li>
                 <li><a href="Withdraw_oldlist.aspx"  onfocus="this.blur()" target="">历史提现记录</a></li>
                <li class="on"><a href="Serverpay.aspx"  onfocus="this.blur()" target="">收款设置</a></li>
                <li><a href="PaySet.aspx"  onfocus="this.blur()" target="">网上支付收款设置</a></li>
            </ul>

        </div>--%>

        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    请选择直销收款方式</h3>

                <table  class="grid">
                    <tr>
                        <td height="80">
                        <p>
                         <label><input name="paytype" type="radio" id="paytype1" value="1" checked />
                         款项支付到平台（客户支付到平台账户，您可以随时进行提现，提现后自动转账到您指定的账户）  
                                             </label>   
                         </p><br/>
                         <table width="550">
                           <tr>
                             <td><label></label>
                               <table align="left" class="grid">
                               <tr>
                                 <td width="160" height="30"><p align="right">对公开户银行/支付宝姓名或公司名称： </p></td>
                                 <td><p align="left">
                                     <input name="bank_account" type="text" id="bank_account" size="50" maxlength="100" />
                                 </p></td>
                               </tr>
                               <tr>
                                 <td height="30" align="right"><p>对公银行账号/支付宝账户： </p></td>
                                 <td><p align="left">
                                     <input name="bank_card" type="text" id="bank_card" size="25" maxlength="50" />
                                 </p></td>
                               </tr>
                               <tr>
                                 <td height="30" align="right">开户单位 ：</td>
                                 <td><input name="bank_name" type="text" id="bank_name" size="50" maxlength="200" /></td>
                               </tr>
                               <tr>
                                 <td height="30" align="right">锁定收款账户：</td>
                                 <td><label><input type="checkbox" id="type" />  锁  定 </label>（当锁定收款账户，则不能修改编辑，如需更改请联系平台客服 李先生 13511097178 ）</td>
                               </tr>
                               <tr>
                                 <td height="30" align="right"></td>
                                 <td>注：您使用银行收款时请填写 <b>对公账户</b>，或填写您收款的支付宝账户。</td>
                               </tr>
                             </table>
                             
                             
                             </td>
                           </tr>
                         </table>
                         <div style=" display:none;"><label><input name="paytype" id="paytype2" type="radio" value="2" />
款项我的支付宝账户
                         (客户支付直接支付到您的支付宝账户,支付手续费按您与支付宝签订的手续费由支付宝收取）</label>
                         <table width="550" bgcolor="#efefef">
                           <tr>
                             <td><table width="400" class="grid">
                               <tr>
                                 <td width="160" height="30" align="right">支付宝账号：</td>
                                 <td><p align="left">
                                     <input name="alipay_account" type="text" id="alipay_account" />
                                 </p></td>
                               </tr>
                               <tr>
                                 <td height="30" align="right"><p align="right">合作身份ID： </p></td>
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
                             </table>                              </td>
                           </tr>
                         </table></div></td>
                    </tr>
                    
                    <tbody id="tblist">
                    </tbody>
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
