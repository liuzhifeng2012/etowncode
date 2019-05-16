<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Manage.Master" CodeBehind="Agent_Com_Open.aspx.cs" Inherits="ETS2.WebApp.Agent.Agent_Com_Open" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var agentid = $("#hid_agentid").trimVal();

            <%if (comaccount !=""){ %>
                var key2 = "<%=comaccount %>";
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=searchunopencom",
                    data: { agentid: agentid, key: key2 },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt(data.msg);
                            return;
                        }
                        if (data.type == 100) {
                            $("#Company").text(data.msg.Company);
                            $("#Account").text(data.msg.Account);
                            $("#name").text(data.msg.Contentname);
                            $("#tel").text(data.msg.Tel);
                            $("#hid_opencomid").val(data.msg.id);
                            $("#kaitong").show();
                        }
                    }
                })

            <%} %>

            //查询未开通的商户
            $("#Search").click(function () {
                var key = $("#key").trimVal();
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=searchunopencom",
                    data: { agentid: agentid, key: key },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt(data.msg);
                            return;
                        }
                        if (data.type == 100) {
                            $("#Company").text(data.msg.Company);
                            $("#Account").text(data.msg.Account);
                            $("#name").text(data.msg.Contentname);
                            $("#tel").text(data.msg.Tel);
                            $("#hid_opencomid").val(data.msg.id);
                            $("#kaitong").show();
                        }
                    }
                })
            })

            $("#confirmButton").click(function () {
                var comid = $("#hid_opencomid").trimVal();
                var Account = $("#Account").html();
                var youxiaoqi = $("#youxiaoqi").trimVal();
                var AdjustHasInnerChannel = $("#AdjustHasInnerChannel").trimVal();
                if (comid == "0") {
                    $.prompt("参数错误，请刷新重新操作");
                    return;
                }

                //开通
                $.post("/JsonFactory/AgentHandler.ashx?oper=AgentComOpen", { agentid: agentid, comid: comid, Account: Account, youxiaoqi: youxiaoqi,AdjustHasInnerChannel:AdjustHasInnerChannel }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("失败，请刷新后重新操作");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("开通成功");
                        location.href = "Agent_Com_list.aspx";
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
         <ul class="composetab">
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_unsel"><div><a href="Agent_Com_list.aspx">商户列表</a></div></div>
            </li>
             <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_sel"><div><a href="Agent_Com_Open.aspx">开通新商户</a></div></div>
            </li>
             <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_unsel"><div><a href="AgentFinane.aspx">财务列表</a></div></div>
            </li>

         </ul>
          <div class="toolbg toolbgline toolheight nowrap" style="">
         <div class="right searchtool">
                 <span>&nbsp;</span>   
         </div>
         <div class="nowrap left" unselectable="on" onselectstart="return false;">
        
         
         </div></div>
        </div>
         <div id="setting-home" class="vis-zone">
            <div class="inner">
 
              <h3>开通新商户</h3>
                    <div style="text-align: center;">
                        <label>
                           公司全称\注册账户\联系电话
                            <input name="key" type="text" id="key" style="width: 160px; height: 20px;" />
                        </label>
                        <label>
                            <input name="Search" type="button" id="Search" value="查询未开通商户" style="width: 120px; height: 26px;" />
                        </label>
                    </div>

<div id="kaitong" style=" display:none">
                <table width="700px" class="grid">
                    <tr>
                        <td class="tdHead" style=" width:80px;">
                            公司名称 : 
                        </td>
                         <td>
                           <h3 class="Company" id="Company">
                        </h3>
                        </td>
                    </tr>
                     <tr>
                        <td class="tdHead" style=" width:80px;">
                            登陆账户 : 
                        </td>
                         <td>
                             <span id="Account"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            联系人姓名 : 
                        </td>
                         <td>
                           <span id="name"></span>
                        </td>
                    </tr>
                        <tr>
                        <td class="tdHead">
                            联系电话 : 
                        </td>
                         <td>
                              <span id="tel"></span>
                        </td>
                    </tr> 
                     <tr>
                        <td class="tdHead">
                            开通计时时间 : 
                        </td>
                         <td>
                              <span id="datetime"><%=today %></span>
                        </td>
                    </tr> 
                                         <tr>
                        <td class="tdHead">
                            开通年限 : 
                        </td>
                         <td>
                              <select name="youxiaoqi" id="youxiaoqi">
	                             <option value="1" selected="selected">1年</option>
                                 <option value="2">2年</option>
                                 <option value="3">3年</option>
                                 <option value="4">4年</option>
                                 <option value="5">5年</option>
	                        </select>
                        </td>
                    </tr> 
                                       <tr>
                        <td class="tdHead">
                            账户类型 : 
                        </td>
                         <td>
                              <select name="AdjustHasInnerChannel" id="AdjustHasInnerChannel">
	                             <option value="false" selected="selected">普通账户</option>
                                 <option value="true">连锁账户</option>
	                        </select>
                        </td>
                    </tr> 
                </table>
                 <table width="300px" class="grid">
                 <tr>
                        <td class="tdHead">
                        <input id="hid_opencomid" type="hidden" value="0" />
                        <input id="confirmButton" type="button" value="    立即开通   " name="confirmButton" />
                </td>
                    </tr> 
                </table>
</div>
                <div id="divPage">
                </div>
            </div>
        </div>

    </div>
    <div class="data">
    </div>

    <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
</asp:Content>
