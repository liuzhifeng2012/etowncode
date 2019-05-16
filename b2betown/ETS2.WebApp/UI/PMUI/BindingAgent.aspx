<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master"  CodeBehind="BindingAgent.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.BindingAgent" %>

<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <%--  <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>--%>
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script src="/Scripts/convertToPinyinLower.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").trimVal();

            //判断密码
            $("#Account").blur(function () {
                $("#AccountVer").html(""); //离开后先清空备注
                var Account = $("#Account").val();
                if (Account == "") {
                    $("#AccountVer").html("请填写账户");
                    $("#AccountVer").css("color", "red");
                    return;
                } else {
                    $("#AccountVer").html("√");
                    $("#AccountVer").css("color", "green");

                }
            })

            //判断密码
            $("#Pwd").blur(function () {
                $("#PwdVer").html(""); //离开后先清空备注
                var Pwd = $("#Pwd").val();
                if (Pwd == "") {
                    $("#PwdVer").html("填写密码");
                    $("#PwdVer").css("color", "red");
                    return;
                } else {
                    $("#PwdVer").html("√");
                    $("#PwdVer").css("color", "green");

                }
            })

            $("html").die().live("keydown", function (event) {
                if (event.keyCode == 13) {
                    $("#btn-login").click();    //这里添加要处理的逻辑  
                }
            });


            //提交按钮
            $("#btn-login").click("click", function () {

                var Account = $("#Account").val();
                var Pwd = $("#Pwd").val();

                if (Account == "") {
                    $("#AccountVer").html("请填写账户");
                    $("#AccountVer").css("color", "red");
                    return;
                }
                else {
                    $("#AccountVer").html("√");
                    $("#AccountVer").css("color", "green");
                }

                if (Pwd == "") {
                    $("#PwdVer").html("填写密码");
                    $("#PwdVer").css("color", "red");
                    return;
                } else {
                    $("#PwdVer").html("√");
                    $("#PwdVer").css("color", "green");

                }

                $(".loading-text").html("正在提交绑定信息，请稍后...")


                $.post("/JsonFactory/AgentHandler.ashx?oper=bindingagent", { comid: comid, Account: Account, Pwd: Pwd }, function (data) {
                    if (data == "OK") {
                        location.href = "BindingAgentList.aspx";
                    } else {
                        $("#login_err").html(data);
                        $("#login_err").css("color", "red");
                        $(".loading-text").html("")
                        return;
                    }


                })
            })
        })



    </script>
    <link type="text/css" href="/Scripts/timepicker/css/jquery-ui-1.8.17.custom.css"
        rel="stylesheet" />
    <link type="text/css" href="/Scripts/timepicker/css/jquery-ui-timepicker-addon.css"
        rel="stylesheet" />
    <script type="text/javascript" src="/Scripts/timepicker/js/jquery-ui-1.8.17.custom.min.js"></script>
    <script type="text/javascript" src="/Scripts/timepicker/js/jquery-ui-timepicker-addon.js"></script>
    <script type="text/javascript" src="/Scripts/timepicker/js/jquery-ui-timepicker-zh-CN.js"></script>
    <style type="text/css">
        .none
        {
            display: none;
        }
        #loading
        {
            position: absolute;
            left: 50%;
            top: 60px;
            z-index: 99;
        }
        #loading, #loading .lbk, #loading .lcont
        {
            width: 146px;
            height: 146px;
        }
        #loading .lbk, #loading .lcont
        {
            position: relative;
        }
        #loading .lbk
        {
            background-color: #000;
            opacity: .5;
            border-radius: 10px;
            margin: -73px 0 0 -73px;
            z-index: 0;
        }
        #loading .lcont
        {
            margin: -146px 0 0 -73px;
            text-align: center;
            color: #f5f5f5;
            font-size: 14px;
            line-height: 35px;
            z-index: 1;
        }
        #loading img
        {
            width: 35px;
            height: 35px;
            margin: 30px auto;
            display: block;
        }
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/pmui/projectlist.aspx" onfocus="this.blur()" target=""><span>项目管理</span></a></li>
                <li><a href="/ui/pmui/Projectedit.aspx" onfocus="this.blur()" target=""><span>添加项目</span></a></li>
                <li><a href="/ui/pmui/ProductList.aspx" onfocus="this.blur()"
                    target=""><span>产品列表</span></a></li>
                <li><a href="/ui/pmui/ProductServerTypeList.aspx" onfocus="this.blur()"
                    target=""><span>添加产品</span></a></li>
                <li><a href="/ui/pmui/order/Salecount.aspx" onfocus="this.blur()" target="">产品统计</a></li>
                <li class="on"><a href="/ui/pmui/BindingAgent.aspx" onfocus="this.blur()" target="">导入分销系统产品</a></li>
                  
                    <li  ><a href="/ui/pmui/eticket_useset.aspx" onfocus="this.blur()" target="">
                        <span>商户特定日期设定</span></a></li>
                          <li><a href="/ui/pmui/delivery/deliverylist.aspx" onfocus="this.blur()" target="">
                    <span>运费模版管理</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                </h3>
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        绑定分销</h2>

                   <div class="mi-form-item">
                        <div id="login_err" style="padding-left:30px;"></div>
	                        <label for="payPwd" class="ui-label">账户名</label>
	                        <input autocomplete="off" class="mi-input" style="width: 200px;" type="text" id="Account" name="Account" data-error="    " value="" seed="JCompleteForm-payPwd" smartracker="on" data-widget-cid="widget-2" data-validator-set="widget-3" placeholder="账户" ><span id="AccountVer"></span>


                        <label for="payPwdConfirm" class="ui-label">登录密码</label>
	                    <input autocomplete="off"  class="mi-input" style="width: 200px;" type="password" id="Pwd" name="Pwd" data-error="    "  seed="JCompleteForm-payPwdConfirm" smartracker="on" data-widget-cid="widget-4" data-validator-set="widget-5" placeholder="密码" ><span id="PwdVer"></span>
                    </div>
                    


                <table border="0">
                    <tr>
                        <td width="600" height="80" align="center">

                            <input type="button" id="btn-login" value="  确认绑定  " class="mi-input" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <input type="hidden" id="hid_bindingagent" value="<%=bindingagent %>" />
   
    <div id="loading" style="top: 150px; display: none;">
        <div class="lbk">
        </div>
        <div class="lcont">
            <img src="/Images/loading.gif" alt="loading..." />正在加载...</div>
    </div>
</asp:Content>
