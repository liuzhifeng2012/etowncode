<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Etown.Master" AutoEventWireup="true" CodeBehind="Member_Activity_Manage.aspx.cs" Inherits="ETS2.WebApp.UI.WebForm2" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
<style>
.pro1{ width:80px;float:left; padding-bottom:2px; padding-left:5px;border-bottom:inset 1px;}
.pro2{ width:80px;float:left;padding-bottom:2px;border-bottom:inset 1px;}
.pro3{ width:200px;float:left;padding-bottom:2px;border-bottom:inset 1px;}


</style>
    <script type="text/javascript">
        $(function () {

            //首先加载数据
            var hid_id = $("#hid_id").trimVal();
            var comid = $("#hid_comid").trimVal();

            if (hid_id != 0) {
                $.post("/JsonFactory/BusinessCustomersHandler.ashx?oper=logdetails", { id: hid_id, comid: comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取数据出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#CardID").html(data.msg.CardID);
                        $("#ACTID").html(data.msg.ACTID);
                        $("#OrderId").html(data.msg.OrderId);
                        $("#ServerName").html(data.msg.ServerName);
                        $("#sales_admin").html(data.msg.Sales_admin);
                        $("#Num_people").html(data.msg.Num_people);
                        $("#Usesubdate").html(ChangeDateFormat(data.msg.Usesubdate));

                    }
                })
            }

        });
            
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
     <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
               <li class="on"><a href="BusinessCustomersList.aspx" onFocus="this.blur()" target="right-main">
                    活动日志列表</a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
          <div class="inner">
              <h3>日志信息</h3>
                <table width="71%" class="grid">
                    <tr>
                        <td width="15%" class="tdHead">
                            卡号：                        </td>
                        <td width="85%"><span id="CardID"></span></td>
                    </tr>
                    <tr>
                        <td class="tdHead">活动ID：                        </td>
                        <td><span id="ACTID"></span></td>
                    </tr>
                    <tr>
                        <td class="tdHead">订单编号：                        </td>
                      <td><span id="OrderId"></span></td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            服务项目：</td>
                        <td><span id="ServerName"></span></td>
                    </tr>
                    <tr>
                        <td class="tdHead"> 服务专员：                        </td>
                        <td><span id="sales_admin"></span></td>
                    </tr>
                    <tr>
                      <td class="tdHead">消费人数：</td>
                      <td><span id="Num_people"></span></td>
                    </tr>
                    <tr>
                      <td class="tdHead">使用时间：</td>
                      <td><span id="Usesubdate"></span></td>
                    </tr>
                </table>
              <p>&nbsp;
              </p>
          </div>
        </div>
    </div>
    <div class="data">
    </div>
    <input type="hidden" id="hid_id" value="<%=id %>" />
</asp:Content>
