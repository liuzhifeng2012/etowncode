<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/UI/Etown.Master"  CodeBehind="PaySet.aspx.cs" Inherits="ETS2.WebApp.UI.VASUI.PaySet" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
 <script type="text/javascript">
     $(function () {
         //获取商家微信设置信息
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
                     $("#appid").val(data.msg.Wx_appid);
                     $("#appsecret").val(data.msg.Wx_appkey);
                     $("#paysignkey").val(data.msg.Wx_paysignkey);
                     $("#partnerid").val(data.msg.Wx_partnerid);
                     $("#partnerkey").val(data.msg.Wx_partnerkey);
                     $("#wx_SSLCERT_PATH").val(data.msg.wx_SSLCERT_PATH);
                     $("#wx_SSLCERT_PASSWORD").val(data.msg.wx_SSLCERT_PASSWORD);

                     return;
                 }
             }
         })
         $("#confirmpub").click(function () {
             var appid = $("#appid").val();
             var appkey = $("#appsecret").val();
             var paysignkey = $("#paysignkey").val();
             var partnerid = $("#partnerid").val();
             var partnerkey = $("#partnerkey").val();
             var wx_SSLCERT_PATH = $("#wx_SSLCERT_PATH").val();
             var wx_SSLCERT_PASSWORD = $("#wx_SSLCERT_PASSWORD").val();

             $.post("/JsonFactory/FinanceHandler.ashx?oper=editpaywx", { appid: appid, appkey: appkey, paysignkey: paysignkey, partnerid: partnerid, partnerkey: partnerkey, comid: $("#hid_comid").trimVal(), id: $("#hid_comextid").trimVal(), wx_SSLCERT_PATH: wx_SSLCERT_PATH, wx_SSLCERT_PASSWORD: wx_SSLCERT_PASSWORD }, function (data) {
                 data = eval("(" + data + ")");
                 if (data.type == 1) {
                     $.prompt("修改微信支付出错:" + data.msg);
                    
                     return;
                 }
                 if (data.type == 100) {
                     $.prompt("修改微信支付成功");
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
                <li><a href="Serverpay.aspx"  onfocus="this.blur()" target="">收款设置</a></li>
                <li class="on"><a href="PaySet.aspx"  onfocus="this.blur()" target="">网上支付收款设置</a></li>
            </ul>
        </div>--%>

        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    微信支付设定</h3>
                <table  class="grid">
                    <tr>
                        <td height="80">
                         <table width="550">
                           <tr>
                             <td><label></label>
                               <table align="left" class="grid">
                               <tr>
                                 <td width="160" height="30"><p align="right">AppID： </p></td>
                                 <td><p align="left">
                                     <input name="appid" type="text" id="appid" size="50" maxlength="100" />
                                 </p></td>
                               </tr>
                               <tr>
                                 <td height="30" align="right"><p>AppSecret： </p></td>
                                 <td><p align="left">
                                     <input name="appsecret" type="text" id="appsecret" size="25" maxlength="50" />
                                 </p></td>
                               </tr>
                               <tr>
                                 <td height="30" align="right">PaySignKey：</td>
                                 <td><input name="paysignkey" type="text" id="paysignkey" size="50" maxlength="200" /><br /><label>2014.09.10后申请成功的版本是v3版本，需要在微信支付商户后台 自己设置；v2版本在微信支付成功后发送的邮件中提供；</label></td>
                               </tr>
                               <tr>
                                 <td height="30" align="right">partnerId/mch_id：</td>
                                 <td><input name="partnerid" type="text" id="partnerid" size="50" maxlength="200" /></td>
                               </tr>
                               <tr>
                                 <td height="30" align="right">partnerKey：</td>
                                 <td><input name="partnerkey" type="text" id="partnerkey" size="50" maxlength="200" /><br /><label>v3版本无需填写</label></td>
                               </tr>
                                 <tr>
                                 <td height="30" align="right">API证书存放路径：</td>
                                 <td><input   type="text" id="wx_SSLCERT_PATH" size="50" maxlength="200" value="wxcert/****/apiclient_cert.p12" />  </td>
                               </tr>
                                 <tr>
                                 <td height="30" align="right">API证书密码(初始为商户号)：</td>
                                 <td><input   type="text" id="wx_SSLCERT_PASSWORD" size="50" maxlength="200" />  </td>
                               </tr>
                             </table></td>
                           </tr>
                         </table>
                        </td>
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
 
    <input type="hidden" id="hid_comextid" value="" />
    <div class="data">
    </div>
</asp:Content>
