<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountManager.aspx.cs"
    MasterPageFile="/UI/Etown.Master" Inherits="ETS2.WebApp.Account.AccountManager" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {

            var comid = $("#hid_comid").trimVal();
            var userid = $("#hid_userid").trimVal();
            var staffid = $("#hid_staffid").trimVal();


            if (staffid != "0") {
                $.post("/JsonFactory/UserHandle.ashx?oper=GetAccountInfo", { id: staffid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取用户信息出错", {
                            buttons: [{ title: "确定", value: true}],
                            submit: function (e, v, m, f) {
                                if (v == true) {
                                    location.href = "usermanage.aspx";
                                }
                            }
                        });
                    }
                    if (data.type == 100) {
                        $("#accountt").val(data.msg[0].Accounts);


                        $("#password").val(data.msg[0].PassWord);
                        $("#password2").val(data.msg[0].PassWord);

                        $("#employeename").val(data.msg[0].MasterName);

                        $("#companyname").val(data.msg[0].CompanyName);
                        $("#tel").val(data.msg[0].Tel);

                        $("#txtjob").val(data.msg[0].Job);
                        $("#Selfbrief").val(data.msg[0].Selfbrief);
                        $("#Workdaystime").val(data.msg[0].Workdaystime);
                        $("#Workendtime").val(data.msg[0].Workendtime);
                        //                        $("#Workaddress").val(data.msg[0].WorkAddress);
                        $("#Fixphone").val(data.msg[0].Fixphone);
                        $("#Email").val(data.msg[0].Email);
                        $("#Homepage").val(data.msg[0].Homepage);
                        $("#Weibopage").val(data.msg[0].Weibopage);
                        $("#QQ").val(data.msg[0].QQ);
                        $("#Weixin").val(data.msg[0].Weixin);

                        $("#hid_groupid").val(data.msg[0].GroupIds);

                        //$("#selchannels").val(data.msg[0].ChannelCompanyId);
                        $("#channelsname").html(data.msg[0].ChannelCompanyName);
                        $("#hid_channelcompanyid").val(data.msg[0].ChannelCompanyId);
                        $("#hid_createuserid").val(data.msg[0].CreateUserId);


                        if (data.Prolist != null) {
                            for (var i = 0; i < data.Prolist.length; i++) {
                                $("#proid").append('<option value="' + data.Prolist[i].Id + '" selected="selected">' + data.Prolist[i].Pro_name + '</option>');
                            }
                        }
                        if (data.projcetid != 0) {

                            $("#hid_projcetid").val(data.projcetid);
                            $("#projectid").find("option[value='" + data.projcetid + "']").attr("selected", true);
                        }
                    }
                })
            }


            var seled = $("#hid_projcetid").val(); ;
            $("#projectid").append('<option value="0"  >请选择项目</option>');
            //加载项目类目
            $.post("/JsonFactory/ProductHandler.ashx?oper=projectlist", { comid: $("#hid_comid").trimVal(), prosort: 1, projectstate: 1 }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                }
                if (data.type == 100) {

                    if (data.totalCount > 0) {
                        for (var i = 0; i < data.totalCount; i++) {
                            if (data.msg[i].Id == seled) {
                                $("#projectid").append('<option value="' + data.msg[i].Id + '" selected="selected">' + data.msg[i].Projectname + '</option>');
                            } else {
                                $("#projectid").append('<option value="' + data.msg[i].Id + '"  >' + data.msg[i].Projectname + '</option>');
                            }
                        }
                    }
                }
            })


            $("#projectid").change(function () {
                var projectid_temp = $("#projectid").val();
                //加载产品
                $.post("/JsonFactory/ProductHandler.ashx?oper=pagelistname", { comid: $("#hid_comid").trimVal(), projectid: projectid_temp, pro_state: 1, pageindex: 1, pagesize: 100 }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                    }
                    if (data.type == 100) {
                        $("#proid").empty();
                        if (data.totalCount > 0) {
                            for (var i = 0; i < data.msg.length; i++) {
                                if (data.msg[i].Id == seled) {
                                    $("#proid").append('<option value="' + data.msg[i].Id + '" selected="selected">' + data.msg[i].Pro_name + '</option>');
                                } else {
                                    $("#proid").append('<option value="' + data.msg[i].Id + '"  >' + data.msg[i].Pro_name + '</option>');
                                }
                            }


                        }
                    }
                })

            })



            $("#button").click(function () {
                var account = $("#accountt").trimVal();
                var password = $("#password").trimVal();
                var password2 = $("#password2").trimVal();
                var employeename = $("#employeename").trimVal();
                var tel = $("#tel").trimVal();


                if (account == "") {
                    $.prompt("账号信息不可为空");
                    return;
                }
                if (password == "" || password2 == "") {
                    $.prompt("两次密码输入都不可为空");
                    return;
                }
                if (employeename == "") {
                    $.prompt("姓名不可为空");
                    return;
                }

                var job = $("#txtjob").trimVal();
                if (job == "") {
                    alert("请输入职位描述");
                    return;
                }
                var Selfbrief = $("#Selfbrief").trimVal();
                if (Selfbrief == "") {
                    alert("请输入个人简介");
                    return;
                }
                var Workdaystime = $("#Workdaystime").trimVal();
                var Workendtime = $("#Workendtime").trimVal();


                var Fixphone = $("#Fixphone").trimVal();
                var Email = $("#Email").trimVal();

                var Homepage = $("#Homepage").trimVal();
                var Weibopage = $("#Weibopage").trimVal();
                var QQ = $("#QQ").trimVal();
                var Weixin = $("#Weixin").trimVal();
                var proid = $("#proid").trimVal();
                var projectid = $("#projectid").trimVal();




                //var groupids = $('input:radio[name="radgrouptype"]:checked').trimVal();
                var groupids = $("#hid_groupid").val();
                var channelcompanyid = $("#hid_channelcompanyid").val();

                $.post("/JsonFactory/AccountInfo.ashx?oper=changestaffpwd", { comid: comid, staffid: staffid, password: password, job: job, Selfbrief: Selfbrief, Fixphone: Fixphone, Email: Email, Homepage: Homepage, Weibopage: Weibopage, QQ: QQ, Weixin: Weixin, Workdaystime: Workdaystime, Workendtime: Workendtime, projectid: projectid, proid: proid }, function (data) {
                    data = eval('(' + data + ')');
                    if (data.type == 1) {
                        $.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("编辑账号信息成功",
                        {
                            buttons: [{ title: "确定", value: true}],
                            submit: callbackfuc
                        });

                    }
                })
                function callbackfuc(e, v, m, f) {
                    if (v == true) {
                        //                        location.href = "/";
                        location.reload();
                    }
                }

            })
        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="/Account/AccountManager.aspx" onfocus="this.blur()" target="">
                    <span>账户管理</span></a></li>
                <%--<li><a href="/ui/userui/bangdingprint.aspx" onfocus="this.blur()" target=""><span>绑定打印机</span></a></li>--%>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    </h3>



                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
               <h2 class="p-title-area">账户管理</h2>
               <div class="mi-form-item">
                    <label class="mi-label">用户登录账户</label>
                    <input type="text" id="accountt" value="" class="mi-input" style="background:#efefef;" readonly />*
               </div>
               <div class="mi-form-item">
                    <label class="mi-label">密码</label>
                    <input type="text" id="password" value="" class="mi-input"  />
               </div>
               <div class="mi-form-item">
                    <label class="mi-label">重复密码</label>
                    <input type="text" id="password2" value="" class="mi-input"  />
               </div>
               <div class="mi-form-item">
                    <label class="mi-label">姓名</label>
                    <input type="text" id="employeename" value="" class="mi-input" style="background:#efefef;" readonly />
               </div>
               <div class="mi-form-item">
                    <label class="mi-label">单位</label>
                    <input type="text" id="companyname" value="" class="mi-input" style="background:#efefef;" readonly />
               </div>
               <div class="mi-form-item">
                    <label class="mi-label">联系电话</label>
                    <input type="text" id="tel" value="" class="mi-input" style="background:#efefef;" readonly />
               </div>

                <div class="mi-form-explain"></div>
</div>

<div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <div class="mi-form-item">
                        <label class="mi-label">
                            职位描述</label>
                        <input type="text" id="txtjob" value="" class="mi-input" maxlength="5" style="width: 80px;" />*
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            个人介绍(请不要超过60个汉字)
                        </label>
                        <input type="text" id="Selfbrief" value="" style="width: 500px;" class="mi-input" />*
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            平日工作时间
                        </label>
                        <span>
                            <input type="text" id="Workdaystime" value="上午9点到下午18点" class="mi-input" /></span>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            周末工作时间
                        </label>
                        <span>
                            <input type="text" id="Workendtime" value="上午9点到下午18点" class="mi-input" /></span>
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        其他联系信息</h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            电子邮箱</label>
                        <input type="text" id="Email" value="" class="mi-input" />
                    </div>
                    <div class="mi-form-item" style="display: none;">
                        <label class="mi-label">
                            主页网址</label>
                        <input type="text" id="Homepage" value="" class="mi-input" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            微博网址(选填)</label>
                        <input type="text" id="Weibopage" value="" class="mi-input" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            QQ号</label>
                        <input type="text" id="QQ" value="" class="mi-input" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            微信号</label>
                        <input type="text" id="Weixin" value="" class="mi-input" />
                    </div>
                    <div class="mi-form-explain">
                    </div>
                    
                </div>
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        顾问页自定义产品</h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            项目</label>
                       <select id="projectid" >
                            </select>
                    </div>
                    <div class="mi-form-item" >
                        <label class="mi-label">
                            请选择产品</label>
                         <select id="proid" size="6" multiple="multiple">
                            </select>
                    </div>
                     <div class="mi-form-item" >
                    <label class="mi-label">
                            请选择具体显示的产品，请按 Ctrl 进行多选。</label></div>
                    <div class="mi-form-explain">
                    </div>
                    
                </div>


                <table width="780" class="grid">

                    <tr>
                        <td height="80" colspan="2" align="center">
                            <input type="button" name="button" id="button" value="  确认提交  "  class="mi-input"/>
                        </td>
                    </tr>
                </table>
                <br />
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <input type="hidden" id="hid_staffid" value="<%=staffid %>" />
    <input type="hidden" id="hid_viewgroupids" value="" />
    <input type="hidden" id="hid_createuserid" value="" />
    <input type="hidden" id="hid_projcetid" value="0" />
    
</asp:Content>
