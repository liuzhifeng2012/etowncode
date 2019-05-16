<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Manage.Master" CodeBehind="Agent_commonaddressedit.aspx.cs"
    Inherits="ETS2.WebApp.Agent.Agent_commonaddressedit" %>


<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
   <title>常用客户管理</title>
<script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
      <link rel="stylesheet" href="/h5/order/css/css4.css" />
    <link rel="stylesheet" href="/h5/order/css/css1.css" />
    <link href="/Styles/H5/yuding.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {

            var addrid = $("#hid_addrid").trimVal();
            if (addrid != "0") {
                $.post("/JsonFactory/OrderHandler.ashx?oper=getagentaddrbyid", { addrid: addrid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) { }
                    if (data.type == 100) {
                        $("#in_name").val(data.msg.U_name);
                        $("#in_phone").val(data.msg.U_phone);
                        $("#in_province").val(data.msg.Province);
                        $("#in_city").append('<option value="' + data.msg.City + '" selected="selected">' + data.msg.City + '</option>');
                        $("#in_address").val(data.msg.Address);
                        $("#in_code").val(data.msg.Code);

                    }
                })
            }
            $("#submitsave").click(function () {
                var agentid = $("#hid_agentid").trimVal();
                var u_name = $("#in_name").trimVal();
                var u_phone = $("#in_phone").trimVal();
                var province = $("#in_province").trimVal();
                var city = $("#in_city").trimVal();
                var address = $("#in_address").trimVal();
                var txtcode = $("#in_code").trimVal();
                if (agentid == 0) {
                    alert("分销id不可为空，请重新登录");
                    return;
                }
                if (u_name == "") {
                    alert("收货人姓名不可为空");
                    return;
                }
                if (u_phone == "") {
                    alert("联系电话不可为空");
                    return;
                } else {
                    if (!isMobel(u_phone)) {
                        alert("请正确填写接收人手机号");
                        return;
                    }
                }
                if (province == "省份") {
                    alert("请选择省份");
                    return;
                }
                if (city == "城市") {
                    alert("请选择城市");
                    return;
                }
                if (address == "") {
                    alert("详细地址不可为空");
                    return;
                }


                $.post("/JsonFactory/OrderHandler.ashx?oper=editagentaddr", { addrid: $("#hid_addrid").trimVal(), agentid: agentid, u_name: u_name, u_phone: u_phone, province: province, city: city, address: address, txtcode: txtcode }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) { }
                    if (data.type == 100) {
                        alert("编辑成功");
                        location.href = "/agent/Agent_commonaddresslist.aspx";
                    }
                })
            })

            $("#submitcannel").click(function () {
                history.go(-1);

            })
        }) 
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
                        <div id="settings" class="view main">
                            <div id="secondary-tabs" class="navsetting ">
                                <ul class="composetab">
                                            <li class="left" style="width:110px;padding-right:2px;">
                                                <div class="composetab_img"></div>
                                                <div class="composetab_unsel"><div><a href="Agent_clientlist.aspx">客户信息</a></div></div>
                                            </li>
                                            <li class="left" style="width:110px;padding-right:2px;">
                                                <div class="composetab_img"></div>
                                                <div class="composetab_sel"><div><a href="Agent_commonaddresslist.aspx">常用客户管理</a></div></div>
                                            </li>
                                         </ul>
                                         <div class="toolbg toolbgline toolheight nowrap" style="">
                                 <div class="right">
                                        
                                 </div>
                                 <div class="nowrap left" unselectable="on" onselectstart="return false;">
                                 <!--<a class="btn_gray btn_space" hidefocus="" id="quick_add" href="/Agent/Agent_commonaddressedit.aspx" name="add">新增常用客户</a>-->
         
         
                                 </div></div>
                            </div>
                            <div id="setting-home" class="vis-zone mail-list">
                                <div class="inner">
                                    <div class="app-inner inner-order peerpay-gift address-fm" style=" 
                                        height: 200%;" id="sku-message-poppage">
                                      
                                        <div class="block" style="margin-bottom: 10px;">
                                            <div class="block-item">
                                                <label class="form-row form-text-row">
                                                    <em class="form-text-label">收货人</em> <span class="input-wrapper">
                                                        <input id="in_name" name="in_name" class="form-text-input" value="" placeholder="名字"
                                                            type="text"></span>
                                                </label>
                                            </div>
                                            <div class="block-item">
                                                <label class="form-row form-text-row">
                                                    <em class="form-text-label">联系电话</em> <span class="input-wrapper">
                                                        <input id="in_phone" name="in_phone" class="form-text-input" value="" placeholder="手机或固话"
                                                            type="tel"></span>
                                                </label>
                                            </div>
                                            <div class="block-item No-Eticket">
                                                <div class="form-row form-text-row">
                                                    <em class="form-text-label">选择地区</em>
                                                    <div class="input-wrapper input-region js-area-select">
                                                        <span>
                                                            <select id="in_province" name="in_province" class="address-province" data-next-type="城市"
                                                                data-next="city">
                                                                <option data-code="" value="省份">省份</option>
                                                            </select>
                                                        </span><span>
                                                            <select id="in_city" name="in_city" class="address-city" data-next-type="区县" data-next="county">
                                                                <option data-code="0" value="城市">城市</option>
                                                            </select>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="block-item No-Eticket">
                                                <label class="form-row form-text-row">
                                                    <em class="form-text-label">详细地址</em> <span class="input-wrapper">
                                                        <input id="in_address" name="in_address" class="form-text-input" value="" placeholder="街道门牌信息"
                                                            type="text"></span>
                                                </label>
                                            </div>
                                            <div class="block-item No-Eticket">
                                                <label class="form-row form-text-row">
                                                    <em class="form-text-label">邮政编码</em> <span class="input-wrapper">
                                                        <input id="in_code" maxlength="6" name="in_code" class="form-text-input" value=""
                                                            placeholder="邮政编码" type="tel"></span>
                                                </label>
                                            </div>
                                        </div>
                                        <div>
                                            <div class="action-container">
                                                <a id="submitsave" class="js-address-save btn btn-block btn-blue">保存</a> <a id="submitcannel"
                                                    class="js-address-cancel btn btn-block">取消</a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="data">
                        </div>
                        <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
                         <input type="hidden" id="hid_addrid" value="<%=addrid %>" />
                        <script type="text/javascript">
                            var province = document.getElementById('in_province');
                            var city = document.getElementById('in_city');
                        </script>
                        <script src="/Scripts/City.js" type="text/javascript"></script>
                   
    <input type="hidden" id="hid_accountid" value="<%=accountid %>" />
    <input type="hidden" id="hid_accountlevel" value="<%=AccountLevel %>" />
</asp:Content>