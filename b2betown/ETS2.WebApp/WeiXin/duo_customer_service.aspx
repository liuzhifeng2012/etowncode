<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="duo_customer_service.aspx.cs"
    MasterPageFile="/UI/Etown.Master" Inherits="ETS2.WebApp.WeiXin.duo_customer_service" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            //得到公司是否把消息转发到多客服
            $.post("/JsonFactory/AccountInfo.ashx?oper=getcurcompany", { comid: $("#hid_comid").val() }, function (data) {
                dat = eval("(" + data + ")");
                if (dat.type == 1) {

                }
                if (dat.type == 100) {
                    if (dat.msg.B2bcompanyinfo.Istransfer_customer_service == 1) {
                        $("#labelmsg").html("将消息发送到多客服(已启用)");
                        $("#closeBt").html("停用");
                        $("#hid1").val(1);
                    }
                    else {
                        $("#labelmsg").html("将消息发送到多客服(已停用)");
                        $("#closeBt").html("启用");
                        $("#hid1").val(0);
                    }
                }
            });




            //判断是否将消息转发到多客服
            $("#closeBt").click(function () {
                //判断公司是否是出于开发模式，即判断weixinbasic表是否含有记录
                $.post("/JsonFactory/WeiXinHandler.ashx?oper=getwxbasic", { comid: $("#hid_comid").trimVal() }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $("#labelmsg").html("将消息发送到多客服(已停用)");
                        $("#closeBt").html("启用");
                        $("#hid1").val(0);
                        alert("只有微信公众号处于开发模式才可将消息转发到多客服");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg == null) {
                            $("#labelmsg").html("将消息发送到多客服(已停用)");
                            $("#closeBt").html("启用");
                            $("#hid1").val(0);
                            alert("只有微信公众号处于开发模式才可将消息转发到多客服");
                            return;
                        } else {
                            if (data.msg.Weixintype != 4) {
                                $("#labelmsg").html("将消息发送到多客服(已停用)");
                                $("#closeBt").html("启用");
                                $("#hid1").val(0);
                                alert("只有微信公众号是认证服务号才可将消息转发到多客服");
                                return;
                            } else {
                                var isqiyong = 0;
                                if ($("#hid1").val() == 1) {//如果原本是启用状态，则需要执行关闭操作
                                    isqiyong = 0;
                                }
                                else {
                                    isqiyong = 1;
                                }
                                $.post("/JsonFactory/AccountInfo.ashx?oper=editcompanyistran_customer_service", { comid: $("#hid_comid").val(), istransfer_customer_service: isqiyong }, function (data1) {
                                    data1 = eval("(" + data1 + ")");
                                    if (data.type == 1) {
                                        $("#labelmsg").html("将消息发送到多客服(已停用)");
                                        $("#closeBt").html("启用");
                                        $("#hid1").val(0);
                                        alert("调整失败");
                                        return;
                                    }
                                    if (data1.type == 100) {

                                        if ($("#hid1").val() == 0) {
                                            $("#labelmsg").html("将消息发送到多客服(已启用)");
                                            $("#closeBt").html("停用");
                                            $("#hid1").val(1);
                                        } else {
                                            $("#labelmsg").html("将消息发送到多客服(已停用)");
                                            $("#closeBt").html("启用");
                                            $("#hid1").val(0);
                                        }
                                        alert("调整成功");
                                        return;
                                    }
                                })
                            }
                        }
                    }
                })




            })
        })
    </script>
    <style type="text/css">
        .developer_info_opr .btn
        {
            margin-left: 8px;
        }
        .btn_warn:hover
        {
            background-color: #d43d3d;
            background-image: -moz-linear-gradient(top,#d43d3d 0,#d43d3d 100%);
            background-image: -webkit-gradient(linear,0 0,0 100%,from(#d43d3d),to(#d43d3d));
            background-image: -webkit-linear-gradient(top,#d43d3d 0,#d43d3d 100%);
            background-image: -o-linear-gradient(top,#d43d3d 0,#d43d3d 100%);
            background-image: linear-gradient(to bottom,#d43d3d 0,#d43d3d 100%);
            border-color: #d43d3d;
            box-shadow: none;
            -moz-box-shadow: none;
            -webkit-box-shadow: none;
            color: #fff;
        }
        .btn:hover
        {
            text-decoration: none;
        }
        a:hover
        {
            text-decoration: underline;
        }
        .btn
        {
            min-width: 60px;
        }
        .btn_warn
        {
            background-color: #f24d4d;
            background-image: -moz-linear-gradient(top,#f24d4d 0,#f24d4d 100%);
            background-image: -webkit-gradient(linear,0 0,0 100%,from(#f24d4d),to(#f24d4d));
            background-image: -webkit-linear-gradient(top,#f24d4d 0,#f24d4d 100%);
            background-image: -o-linear-gradient(top,#f24d4d 0,#f24d4d 100%);
            background-image: linear-gradient(to bottom,#f24d4d 0,#f24d4d 100%);
            border-color: #f24d4d;
            color: #fff;
        }
        .btn
        {
            display: inline-block;
            overflow: visible;
            padding: 0 22px;
            height: 30px;
            line-height: 30px;
            vertical-align: middle;
            text-align: center;
            text-decoration: none;
            border-radius: 3px;
            -moz-border-radius: 3px;
            -webkit-border-radius: 3px;
            font-size: 14px;
            border-width: 1px;
            border-style: solid;
            cursor: pointer;
        }
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="WxAllSendMsgPage.aspx" onfocus="this.blur()"><span>微信留言</span></a></li>
                <%if (IsParentCompanyUser)
                  { %>   
                <li class="on"><a href="duo_customer_service.aspx" onfocus="this.blur()"><span>微信多客服</span></a></li>
                <%} %>
                <li><a href="wx_kflist.aspx" onfocus="this.blur()"><span>微信多客服列表</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        微信多客服</h2>
                    <div class="mi-form-item">
                        <label class="mi-label" style="color: Black;" id="labelmsg">
                            将消息发送到多客服(已启用)</label>
                        <a href="javascript:;" class="btn btn_warn" id="closeBt">停用</a>
                        <input type="hidden" id="hid1" value="" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            如果公众号处于开发模式,微信服务器在收到这条消息时，会把当次发送的消息转发至多客服系统。消息被转发到多客服以后，会被自动分配给一个在线的客服帐号。</label>
                    </div>
                </div>
                <br />
            </div>
        </div>
    </div>
    <div class="data">
    </div>
</asp:Content>
