<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Manage.Master"
    CodeBehind="AgentStaff_Manage.aspx.cs" Inherits="ETS2.WebApp.Agent.AgentStaff_Manage" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            //图形验证码
            $("#validateCodetext").click(function () {
                $("#validateCode").attr("src", "/Account/GetValidateCode.ashx?tick=" + (new Date()).getTime());
            })
            $("#validateCode").click(function () {
                this.src = '/Account/GetValidateCode.ashx?tick=' + (new Date()).getTime();
            })


            var accountlevel = $("#hid_accountlevel").trimVal();
            if (accountlevel == 0) {
                $("#secondary-tabs").find("li").eq(1).show();
            }

            var agentid = $("#hid_agentid").trimVal();
            var account = $("#hid_account").trimVal();
            var id = $("#hid_id").trimVal();
            var comid = $("#hid_comid_temp").trimVal();

            if (id == 0) {
                $("#AccountMobile").removeAttr("readonly");
            } else {
                $("#AccountMobile").attr("readonly", "readonly");
            }
            //获得分销信息
            $.post("/JsonFactory/AgentHandler.ashx?oper=getagentinfo", { agentid: agentid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { alert("获得分销信息出错"); return; }
                if (data.type == 100) {
                    $("#Company").text(data.msg.Company);
                }
            })

            SearchList(1);

            //装载产品列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }

                if (id != 0) {
                    $.ajax({
                        type: "post",
                        url: "/JsonFactory/AgentHandler.ashx?oper=getmanageaccountinfo",
                        data: { id: id },
                        async: false,
                        success: function (data) {
                            data = eval("(" + data + ")");
                            if (data.type == 1) {
                                $.prompt("查询错误");
                                return;
                            }
                            if (data.type == 100) {
                                $("#Account").val(data.msg.Account);
                                $("#Accountname").val(data.msg.Contentname);
                                $("#AccountMobile").val(data.msg.Mobile);
                                $("#Pwd").val(data.msg.Pwd);
                                $("#QPwd").val(data.msg.Pwd);
                                

                                $("#Amount").val(data.msg.Amount);
                                $("#Account").attr("readonly", "readonly");

                                $("#Span_company").html(data.msg.Contentname);
                                $("#hid_edit_agentuserid").val($("#hid_id").trimVal());
                            }


                        }
                    })
                }

            }

            $("#upaccount").click(function () {
               
                var Account = $("#Account").trimVal();
                var Accountname = $("#Accountname").trimVal();
                var AccountMobile = $("#AccountMobile").trimVal();
                var Pwd = $("#Pwd").trimVal();
                var QPwd = $("#QPwd").trimVal();
                
                var Amount = $("#Amount").val();
                var Accounttype = 1; //新增
                if (id != 0) {
                    Accounttype = 2;
                }


                if (Account == "") {
                    $.prompt("请填写账户");
                    return;
                }
                if (Accountname == "") {
                    $.prompt("请填写姓名");
                    return;
                }

                if (AccountMobile == "") {
                    $.prompt("请填写手机号");
                    return;
                } else {
                    if (!isMobel(AccountMobile)) {
                        $.prompt("请正确填写手机号");
                        return;
                    }
                }

                if (id == 0) {
                    var phonecode = $("#phonecode").val();
                    if (phonecode == "") {
                        alert("请填写短信验证码");
                        return;
                    }
                    //判断验证码输入是否正确
                    $.post("/JsonFactory/AgentHandler.ashx?oper=judgevalidsms", { mobile: $("#AccountMobile").val(), smscode: $("#phonecode").val(), source: "员工管理验证码" }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            alert("验证码不相符");
                            return;
                        }
                        if (data.type == 100) {
                           
                        }
                    }) 
                }
                if (Pwd == "") {
                    $.prompt("请填写密码");
                    return;
                }

                if (Pwd != QPwd) {
                    $.prompt("两次密码填写不同");
                    return;
                }

                //修改
                $.post("/JsonFactory/AgentHandler.ashx?oper=ManageAccountUp", { id: id, agentid: agentid, Account: Account, Accountname: Accountname, AccountMobile: AccountMobile, Pwd: Pwd, Amount: Amount, Accounttype: Accounttype }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        alert("失败，请刷新后重新操作");
                        return;
                    }
                    if (data.type == 100) {
                        alert("修改成功");
                        location.href = "AgentStaff.aspx";
                        return;
                    }
                })
            })

            //获取手机验证码
            $("#getphonecode").click(function () {
                var imgcode = $("#getcode").trimVal();
                if (imgcode == "") {
                    alert("请输入图形验证码!");
                    return;
                }
                //判断图形验证码是否正确
                $.post("/JsonFactory/RegisterUser.ashx?oper=verifyimgcode", { imgcode: imgcode }, function (dd) {
                    dd = eval("(" + dd + ")");
                    if (dd.type == 1) {
                        alert("图形验证码输入不正确");
                        $("#validateCode").attr("src", "/Account/GetValidateCode.ashx?tick=" + (new Date()).getTime());
                        return;
                    } else {
                        var id = $("#hid_id").trimVal();
                        var phone = "";
                        if (id != 0) {
                            phone = $.trim($("#newphone").val());
                        } else {
                            phone = $.trim($("#AccountMobile").val());
                        }

                        if (phone == "") {
                            alert("请输入手机号码!");
                            return;
                        }
                        if (!checkMobile(phone)) {
                            alert("请正确输入手机号!");
                            return;
                        }

                        if ($.trim($("#getphonecode").html()) == "获取短信验证码") {
                            $("#getphonecode").attr("disabled", "disabled").css("background-color", "#f4f4f4");
                            _callSmsApi();

                        }
                    }
                })

            })

            //提交手机更改
            $("#sub_rh").click(function () {
                var phonecode = $("#phonecode").val();
                if (phonecode == "") {
                    alert("请填写短信验证码");
                    return;
                }
                //判断验证码输入是否正确
                $.post("/JsonFactory/AgentHandler.ashx?oper=judgevalidsms", { mobile:
$("#newphone").val(), smscode: $("#phonecode").val(), source: "员工管理验证码"
                }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        alert("验证码不相符");
                        return;
                    }
                    if (data.type == 100) {
                        //更改分销员工联系手机号
                        $.post("/JsonFactory/AgentHandler.ashx?oper=AgentUserUpPhone", { agentuserid: $("#hid_edit_agentuserid").trimVal(), newphone: $("#newphone").val() }, function (data1) {
                            data1 = eval("(" + data1 + ")");
                            if (data1.type == 1) {
                                //$.prompt("失败，请刷新后重新操作");
                                return;
                            }
                            if (data1.type == 100) {
                                //$.prompt("修改成功");
                                location.reload();
                                //return;
                            }
                        })
                    }
                })
            })
            //取消手机更改
            $("#cancel_rh").click(function () {
                $("#phonecode").val("");
                $("#div_showhangye").hide();

            })
        })
        function newphone_set() {
            $("#div_showhangye").show();
        }
        function checkMobile(tel) {
            tel = $.trim(tel);
            if (tel.length != 11 || tel.substr(0, 1) != 1) {
                return false;
            }
            return true;
        }
        function _sendSmsCD() {
            var sec = parseInt($("#getphonecode").html());
            if (sec > 1) {
                $("#getphonecode").html((sec - 1) + "秒后可再次发送短信");
                window.setTimeout(_sendSmsCD, 1000);
            } else {
                $("#getphonecode").html("获取短信验证码");
                $("#getphonecode").removeAttr("disabled").css("background-color", "#FFFFFF");
            }
        }

        function _callSmsApi() {
            var imgcode = $("#getcode").trimVal();
            if (imgcode == "") {
                alert("请输入图形验证码!");
                return;
            }
            var id = $("#hid_id").trimVal();
            var phone = "";
            if (id != 0) {
                phone = $.trim($("#newphone").val());
            } else {
                phone = $.trim($("#AccountMobile").val());
            }
            $.post("/JsonFactory/AgentHandler.ashx?oper=callvalidsms", { imgcode: imgcode, mobile: phone, comid: 0, source: "员工管理验证码" }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert(data.msg);
                    location.reload();
                }
                if (data.type == 100) {
                    $("#getphonecode").html("30秒后可再次发送短信");
                    window.setTimeout(_sendSmsCD, 1000);
                }
            })

        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
        <ul class="composetab">
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_sel"><div><a href="AgentStaff.aspx">员工管理</a></div></div>
            </li>

            
         </ul>
          <div class="toolbg toolbgline toolheight nowrap" style="">
         <div class="right searchtool">
                 <span>&nbsp;</span>   
         </div>
         <div class="nowrap left" unselectable="on" onselectstart="return false;">
         <!--<a class="btn_gray btn_space" hidefocus="" id="quick_del" href="javascript:;" name="del">删除</a>-->
         
         
         </div></div>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    员工管理</h3>
                <table class="grid">
                    <tr>
                        <td class="tdHead">
                            公司名称:
                        </td>
                        <td>
                            <h3 class="Company" id="Company">
                            </h3>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            账户名 :
                        </td>
                        <td>
                            <input name="Input" class="dataNum dataIcon" id="Account" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            联系人姓名 :
                        </td>
                        <td>
                            <input name="Input" class="dataNum dataIcon" id="Accountname" value="" />
                        </td>
                    </tr>
                    <%if (Id == 0)
                      {
                    %>
                    <tr>
                        <td class="tdHead">
                            图形验证码 :
                        </td>
                        <td>
                            <input name="getcode" type="text" placeholder="图形验证码" id="getcode" size="10" class="dataNum dataIcon" />
                            <img id="validateCode" src="/Account/GetValidateCode.ashx" alt="ValidateCode" title="点击图片刷新验证码" />
                            <a href="javascript:;" id="validateCodetext">更换</a>
                        </td>
                    </tr>
                    <%} %>
                    <tr>
                        <td class="tdHead">
                            联系手机:
                        </td>
                        <td>
                            <input name="Input" class="dataNum dataIcon" id="AccountMobile" value="" />
                            <%if (Id == 0)
                              {
                            %>
                            <a id="getphonecode" style="text-decoration: underline; cursor: pointer;">获取短信验证码</a>
                            <%
                                }
                              else
                              {  %>
                            <a style="text-decoration: underline; cursor: pointer;" onclick="newphone_set()">更换新手机</a>
                            <%
                                }  %>
                        </td>
                    </tr>
                    <%if (Id == 0)
                      {
                    %>
                    <tr>
                        <td class="tdHead">
                            短信验证码:
                        </td>
                        <td>
                            <input name="Input" class="dataNum dataIcon" id="phonecode" value="" />
                        </td>
                    </tr>
                    <%
                        }  %>
                    <tr>
                        <td class="tdHead">
                            登陆密码 :
                        </td>
                        <td>
                            <input name="Input" type="password" class="dataNum dataIcon" id="Pwd" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            再次确认密码 :
                        </td>
                        <td>
                            <input name="Input" type="password" class="dataNum dataIcon" id="QPwd" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            授权额度 :
                        </td>
                        <td>
                            <input name="Input" class="dataNum dataIcon" id="Amount" value="" />元
                        </td>
                    </tr>
                </table>
                <table width="300px" class="grid">
                    <tr>
                        <td class="tdHead">
                            <input id="upaccount" type="button" value="    修改账户信息   " name="upaccount"></input>
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
    <%if (Id > 0)
      {
    %>
    <div id="div_showhangye" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 350px; height: 200px; display: none; left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#E7F0FA"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 12px; font-weight: bold" id="span1">更换新手机
                    </span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    员工姓名：<span id="Span_company"></span>
                </td>
            </tr>
            <tr>
                <td class="tdHead">
                    图形验证码 :
                    <input name="getcode" type="text" placeholder="图形验证码" id="getcode" size="10" />
                    <img id="validateCode" src="/Account/GetValidateCode.ashx" alt="ValidateCode" title="点击图片刷新验证码" />
                    <a href="javascript:;" id="validateCodetext">更换</a>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    新手机号码 :
                    <input name="Input" class="dataNum dataIcon" id="newphone" value="" />
                    <a id="getphonecode" style="text-decoration: underline; cursor: pointer;">获取短信验证码</a>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    短信验证码 :
                    <input name="Input" class="dataNum dataIcon" id="phonecode" value="" />
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#E7F0FA" class="tdHead">
                    <input type="hidden" id="hid_edit_agentuserid" value="0" />
                    <input id="sub_rh" type="button" class="formButton" value="  提  交  " />
                    <input id="cancel_rh" type="button" class="formButton" value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>
    <%
        }  %>
    <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
    <input id="hid_id" type="hidden" value="<%=Id %>" />
    <input id="hid_comid_temp" type="hidden" value="<%=comid_temp %>" />
</asp:Content>
