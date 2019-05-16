<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="AgentRegi.aspx.cs"
    Inherits="ETS2.WebApp.UI.PMUI.AgentRegi" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var agentid = $("#hid_agentid").trimVal();
            var comid = $("#hid_comid").trimVal();
               //获得分销类型
            $.post("/JsonFactory/AgentHandler.ashx?oper=GetAgentSourceSortlist", {}, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {
                    if (data.msg.length > 0) {
                        var optionstr = "";
                        for (var i = 0; i < data.msg.length; i++) {
                            if (data.msg[i].key!=2)
                            {
                              optionstr += '<option value="' + data.msg[i].key + '" >' + data.msg[i].value + '</option>';
                            }
                          
                        }
                        $("#agentsourcesort").html(optionstr);
                    }
                }
            })
            //账号名
            $("#Email").blur(function () {
                $("#EmailVer").html(""); //离开后先清空备注
                var Email = $("#Email").val();
                //判断邮箱不为空
                if (Email != "") {
                    $.post("/JsonFactory/AgentHandler.ashx?oper=getEmail", { Email: Email, comid: comid,}, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            if (data.msg == "OK") {
                                $("#EmailVer").html("√");
                                $("#EmailVer").css("color", "green");
                                $("#VEmail").val(1);
                            } else {
                                $("#EmailVer").html(data.msg);
                                $("#EmailVer").css("color", "red");
                                $("#VEmail").val(0);
                                return;
                            }

                        }
                    })
                }
            })


            //账号名
            $("#Phone").blur(function () {
                $("#PhoneVer").html(""); //离开后先清空备注
                var Phone = $("#Phone").val();
                //判断手机不为空
                if (Phone != "") {
                    $.post("/JsonFactory/AgentHandler.ashx?oper=getPhone", { Phone: Phone, comid: comid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 100) {
                            if (data.msg == "OK") {
                                $("#PhoneVer").html("√");
                                $("#PhoneVer").css("color", "green");
                                $("#VPhone").val(1);
                            } else {
                                $("#PhoneVer").html(data.msg );
                                $("#PhoneVer").css("color", "red");
                                $("#VPhone").val(0);
                                return;
                            }
                        }
                    })
                }
            })

            
            //提交按钮
            $("#Search").click(function () {
                var Phone = $("#wphone").val();
                if (Phone == "") {
                    $("#wphoneVer").html("请填写手机");
                    $("#wphoneVer").css("color", "red");
                    return;
                }

                //创建订单
                $.post("/JsonFactory/AgentHandler.ashx?oper=Agentsearch", {comid: comid,Phone: Phone}, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $("#wphoneVer").html(data.msg);
                        $("#wphoneVer").css("color", "red");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg == "OK") {
                            location.href = "AgentManageSet.aspx?agentid=" + data.id;
                            return;
                        }
                        else {
                            $("#wphoneVer").html("查询出错，请重新查询");
                            $("#wphoneVer").css("color", "red");
                            return;
                        }
                    }
                })
            })

            //提交按钮
            $("#btn-submit").click(function () {
                var Email = $("#Email").val();
                var Name = $("#Name").val();
                var Phone = $("#Phone").val();
                var Company = $("#Company").val();
                var agentsourcesort = $("#agentsourcesort").val();
                var sms=0;

                $("input:checkbox[name=sms]:checked'").each(function (i) {
                    sms = $(this).val();
                });

                if (Company == "") {
                    $("#CompanyVer").html("请填写公司名称");
                    $("#CompanyVer").css("color", "red");
                    return;
                }
                if (Email == "") {
                    $("#EmailVer").html("请填账户");
                    $("#EmailVer").css("color", "red");
                    return;
                }
                if ($("#VEmail").val() == 0) {
                    $("#EmailVer").html("电子邮箱有误");
                    $("#EmailVer").css("color", "red");
                    return;
                }

               if (Name == "") {
                    $("#NameVer").html("请填写姓名");
                    $("#NameVer").css("color", "red");
                    return;
                }
                if (Phone == "") {
                    $("#PhoneVer").html("请填写手机");
                    $("#PhoneVer").css("color", "red");
                    return;
                }
                if ($("#VPhone").val() == 0) {
                    $("#PhoneVer").html("手机有误");
                    $("#PhoneVer").css("color", "red");
                    return;
                };

                
                var com_province = $("#com_province").trimVal();
                var com_city = $("#com_city").trimVal();


                if (com_province == "" || com_province == "省份") {
                    $.prompt("请选择所在省份");
                    return;
                }
                if (com_city == "" || com_city == "城市") {
                    $.prompt("请选择所属城市");
                    return;
                }
                if (agentsourcesort==0)
                {
                    $.prompt("请选择分销类型");
                    return;
                }

                $("#loading").html("正在提交注册信息，请稍后...")

                //创建订单
                $.post("/JsonFactory/AgentHandler.ashx?oper=Agentregi", {agentsourcesort:agentsourcesort, comid: comid, Email: Email, Password1: 123456, Name: Name, Tel: "", Phone: Phone, Company: Company, Address: "", com_province: com_province, com_city: com_city,sms:sms ,lp:0 }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("注册出现错误，请刷新重新提交！");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg == "OK") {
                            location.href = "AgentManageSet.aspx?agentid=" + data.id;
                            return;
                        }
                        else {
                            $.prompt("参数传递出错，请重新操作");
                            return;
                        }
                    }
                })

            })

        })

    </script>
    <style type="text/css">
        .ui-input
        {
            border: 1px solid #C1C1C1;
            color: #343434;
            font-size: 14px;
            height: 25px;
            line-height: 25px;
            padding: 2px;
            vertical-align: middle;
            width: 200px;
        }
    </style>
    </head>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="AgentList.aspx" onfocus="this.blur()" target=""><span>分销商管理</span></a></li>
                <li><a href="AgentSalesCode.aspx" onfocus="this.blur()" target=""><span>后台销售订单</span></a></li>
                <li><a href="AgentBackCode.aspx" onfocus="this.blur()" target=""><span>导码订单</span></a></li>
                <li><a href="AgentRecharge.aspx" onfocus="this.blur()" target=""><span>充值订单</span></a></li>
                <li><a href="AgentMessage.aspx" onfocus="this.blur()" target="">管理分销通知</a></li>
                <li><a href="AgentFinanceCount.aspx" onfocus="this.blur()" target="">分销商统计</a>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
  <h3>
                    <a href="AgentList.aspx" class="a_anniu">分销商列表</a> <a href="AgentRegi.aspx" class="b_anniu">新增分销商</a> <a href="LPManage.aspx" class="a_anniu">平台分销商设定</a> </h3>
                  <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        对指定已注册分销商授权</h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                           分销商手机</label>
                        <input name="wphone" type="text" id="wphone" size="25" class="mi-input" style="width: 200px;" /><span
                            id="wphoneVer"></span>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                           </label>
                          <input type="button" name="Search" id="Search" class="mi-input" value="  查询分销商  " />
                    </div>
                     <div class="mi-form-explain">
                    </div>
                </div>

                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        新增分销商</h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            分销商公司名称</label>
                        <input name="Company" type="text" id="Company" size="25" class="mi-input" style="width: 200px;" /><span
                            id="CompanyVer"></span>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            登陆账户</label>
                        <input name="Email" type="text" id="Email" size="25" class="mi-input" style="width: 200px;" /><span
                            id="EmailVer"></span>（初始密码默认为 123456 ）
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            联系人姓名</label>
                        <input name="Name" type="text" id="Name" size="25" class="mi-input" style="width: 200px;" /><span
                            id="NameVer"></span>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            联系人手机</label>
                        <input name="Phone" type="text" id="Phone" size="25" class="mi-input" style="width: 200px;" /><span
                            id="PhoneVer"></span>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            所在省市</label>
                        <select name="com_province" id="com_province" class="mi-input">
                            <option value="省份" selected="selected">省份</option>
                        </select>
                        <select name="com_city" id="com_city" class="mi-input">
                            <option value="城市" selected="selected">市县</option>
                        </select>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            分销类型</label>
                        <select id="agentsourcesort" class="mi-input" style="width: 210px;">
                            <option value="0" selected>类 别</option>
                        </select>
                    </div>
                    <div class="mi-form-item" id="sendsms">
                        <label class="mi-label">
                            发送短信</label>
                        <input name="sms" type="checkbox" value="1" />发送通知短信
                    </div>

                    <div class="mi-form-explain">
                    </div>
                </div>
                <table width="600px;">
                    <tr>
                        <td align="center">
                            <input type="button" name="btn-submit" id="btn-submit" class="mi-input" value="  确认添加新分销商  " />
                        </td>
                    </tr>
                </table>
                <div id="divPage">
                </div>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <input type="hidden" name="VEmail" id="VEmail" value="0" />
    <input type="hidden" name="VPhone" id="VPhone" value="0" />
    <input type="hidden" id="hid_agentid" value='' />
    <script type="text/javascript">
        var province = document.getElementById('com_province');
        var city = document.getElementById('com_city');
    </script>
    <script src="/Scripts/City.js" type="text/javascript"></script>
</asp:Content>
