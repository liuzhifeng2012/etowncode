<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ProjectAgentlist.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.ProjectAgentlist" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 100; //每页显示条数

        $(function () {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();
            var projectid = $("#hid_projectid").trimVal();


            SearchList(1);

            //装载产品列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=projectagentpagelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, projectid: projectid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                //                                $("#tblist").html("查询数据为空");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pageSize, pageindex);
                            }
                        }
                    }
                })


            }

            //分页
            function setpage(newcount, newpagesize, curpage) {
                $("#divPage").paginate({
                    count: Math.ceil(newcount / newpagesize),
                    start: curpage,
                    display: 5,
                    border: false,
                    text_color: '#888',
                    background_color: '#EEE',
                    text_hover_color: 'black',
                    images: false,
                    rotate: false,
                    mouse: 'press',
                    onChange: function (page) {

                        SearchList(page);

                        return false;
                    }
                });
            }

            $("#add").click(function () {
                $("#addaccount").show();
            })
            $("#binding").click(function () {
                $("#bindingaccount").show();
            })


            //提交按钮
            $("#bindingsub").click(function () {
                var bindingemail = $("#bindingemail").val();

                if (bindingemail == "") {
                    $("#bindingEmailVer").html("请填写手机");
                    $("#bindingEmailVer").css("color", "red");
                    return;
                }

                $("#loading").html("正在提交注册信息，请稍后...")

                //bangding
                $.post("/JsonFactory/AgentHandler.ashx?oper=Agentbindingproject", { comid: comid, bindingemail: bindingemail, projectid: projectid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("提交出现错误，请刷新重新提交！");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg == "OK") {

                            location.reload();
                        }
                        else {
                            $.prompt("参数传递出错，请重新操作");
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



                $("#loading").html("正在提交注册信息，请稍后...")

                //创建
                $.post("/JsonFactory/AgentHandler.ashx?oper=Agentregi", { comid: comid, Email: Email, Password1: 123456, Name: Name, Tel: "", Phone: Phone, Company: Company, Address: "", projectid: projectid, agentsort: 2 }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("注册出现错误，请刷新重新提交！");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg == "OK") {

                            location.reload();
                        }
                        else {
                            $.prompt("参数传递出错，请重新操作");
                            return;
                        }
                    }
                })

            })

        })
        function closeaddaccount() {
            $("#addaccount").hide();
        }
        function closebindingaccount() {
            $("#bindingaccount").hide();
        }

    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
              <li><a href="projectlist.aspx" onfocus="this.blur()" target=""><span>返回项目列表</span></a></li>
                <li class="on"><a href="ProjectAgentList.aspx" onfocus="this.blur()" target=""><span>授权账户列表查看验票数</span></a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    <%=projectname%> 授权账户</h3>
                    <input name="add" id="add" value="  添加新账户  " style="width: 120px;
                            height: 26px;" type="button">

                    <input name="add" id="binding" value="  对已有账户授权  " style="width: 120px;
                            height: 26px;" type="button">

                <table width="780" border="0">
                    <tr>
                        <td width="20">
                            <p align="left">
                                ID</p>
                        </td>
                        <td width="120">
                            <p align="left">
                                公司名称
                            </p>
                        </td>
                        <td width="120">
                            <p align="left">
                                联系人手机
                            </p>
                        </td>
                        <td width="80">
                            <p align="left">
                                姓名
                            </p>
                        </td>
                        <td width="200">
                            <p align="left">
                               开户账户</p>
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                </table>
                <div id="divPage">
                </div>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr>
                        <td>
                            <p align="left">
                                ${Id}</p>
                        </td>
                        <td>
                            <p align="left">
                                ${Company}</p>
                        </td>
                        <td>
                            <p align="left">
                                ${Mobile}
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                ${Contentname}
                            </p>
                        </td>
                        <td>
                            <p align="left">
                            ${Account}
                            </p>
 
                        </td>
                    </tr>
    </script>

    <div id="addaccount" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 450px; height: 400px; display: none; left: 20%; position: absolute;
        top: 20%;">

            <div class="mi-form-item">
                        <label class="mi-label"> 公司名称</label>
                       <input name="Company" type="text" id="Company"  size="25" class="mi-input"  style="width:200px;"/><span id="CompanyVer"></span>
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 登陆账户</label>
                       <input name="Email" type="text" id="Email"  size="25" class="mi-input"  style="width:200px;"/><span id="EmailVer"></span>（初始密码默认为 123456 ）	
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 联系人姓名</label>
                       <input name="Name" type="text" id="Name"  size="25" class="mi-input"  style="width:200px;"/><span id="NameVer"></span>
                   </div>
                                      <div class="mi-form-item">
                        <label class="mi-label"> 联系人手机</label>
                       <input name="Phone" type="text" id="Phone"  size="25" class="mi-input"  style="width:200px;"/><span id="PhoneVer"></span>
                   </div>

                   <div class="mi-form-item">
                       <input type="button" name="Search" id="btn-submit"  class="mi-input" value="  确认添加新账户  " />
                        <input name="cancel_rh" type="button" class="formButton" onclick="closeaddaccount ();" value="  关  闭  " />
                   </div>
    </div>
     <div id="bindingaccount" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 450px; height: 200px; display: none; left: 20%; position: absolute;
        top: 20%;">

                   <div class="mi-form-item">
                       <label class="mi-label"> 授权的登陆账户</label>
                       <input name="bindingemail" type="text" id="bindingemail"  size="25" class="mi-input"  style="width:200px;"/><span id="bindingEmailVer"></span>	
                   </div>

                   <div class="mi-form-item">
                       <input type="button" name="Search" id="bindingsub"  class="mi-input" value="  确认授权登陆账户  " />
                        <input name="cancel_rh" type="button" class="formButton" onclick="closebindingaccount ();" value="  关  闭  " />
                   </div>
    </div>


    <input type="hidden" id="hid_projectid" value="<%=projectid %>" />
</asp:Content>
