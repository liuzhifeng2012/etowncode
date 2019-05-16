<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="/Agent/Manage.Master" CodeBehind="Agentinfo.aspx.cs" Inherits="ETS2.WebApp.Agent.Agentinfo" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            var agentid = $("#hid_agentid").trimVal();
            var proid = $("#hid_proid").trimVal();
            var comid = $("#hid_comid_temp").trimVal();
            SearchList(1);

            //装载产品列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=getagentaccountinfo",
                    data: { agentid: agentid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#Account").text(data.msg.Account);
                            $("#Accountname").val(data.msg.Contentname);
                            $("#AccountMobile").val(data.msg.Mobile);
                            $("#Pwd").val(data.msg.Pwd);

                            if (data.msg.AccountLevel == 1) {
                                $("#Amount").text(data.msg.Amount+ "元");
                            } else {
                                $("#edu").hide();
                            }

                        }
                    }
                })


            }


            $("#upaccount").click(function () {
                var Accountname = $("#Accountname").trimVal();
                var AccountMobile = $("#AccountMobile").trimVal();
                var Pwd = $("#Pwd").trimVal();

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
                if (Pwd == "") {
                    $.prompt("请填写密码");
                    return;
                }


                //修改
                $.post("/JsonFactory/AgentHandler.ashx?oper=AgentAccountUp", { agentid: agentid, Accountname: Accountname, AccountMobile: AccountMobile, Pwd: Pwd }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("失败，请刷新后重新操作");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("修改成功");
                        return;
                    }
                })

            })


        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="Agentinfo.aspx" onfocus="this.blur()"><span>账户管理</span></a></li>
            </ul>
        </div>
         <div id="setting-home" class="vis-zone">
            <div class="inner">
             <h3>账户管理</h3>
              <table class="grid">
                    <tr>
                        <td class="tdHead">
                            账户 : 
                        </td>
                         <td>
                           <h3 class="Company" id="Account">
                        </td>
                    </tr>
                    <tr id="edu">
                        <td class="tdHead">
                            授权额度 : 
                        </td>
                         <td>
                           <h3 class="Company" id="Amount">
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            联系手机: 
                        </td>
                         <td>
                           <input name="Input" class="dataNum dataIcon" id="AccountMobile" value="" />
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
                    <tr>
                        <td class="tdHead">
                           登陆密码 : 
                        </td>
                         <td>
                            <input name="Input" type="password" class="dataNum dataIcon" id="Pwd" value="" />
                        </td>
                    </tr> 
                </table>
                 <table width="300px" class="grid">
                 <tr>
                        <td class="tdHead">
                       <input id="upaccount" type="button" value="    修改登陆账户信息   " name="upaccount"></input>
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

    <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
    <input id="hid_comid_temp" type="hidden" value="<%=comid_temp %>" />
</asp:Content>
