<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="AgentManageSet.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.AgentManageSet" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            var agentid = $("#hid_agentid").trimVal();
            var comid = $("#hid_comid").trimVal();
            var userid = $("#hid_userid").trimVal();


            $.post("/JsonFactory/AgentHandler.ashx?oper=getAgentId", { agentid: agentid, comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("获取数据出现错误");
                    return;
                }
                if (data.type == 100) {

                    if (agentid == 9 || data.msg.Warrant_type != 0 || comid == 106) {
                        $("#company").html(data.msg.Company);
                        $("#Account").html(data.msg.Account);

                        $("#name").html(data.msg.Contentname);
                        $("#tel").html(data.msg.Tel);
                        $("#mobile").html(data.msg.Mobile);
                        $("#address").html(data.msg.Address);
                        $("#imprest").html("￥ " + data.msg.Imprest);
                        $("#credit").html(data.msg.Credit);
                    }

                    if (data.msg.Warrant_type == 0) {

                        if (agentid != 9) {
                            $("#confirmpub").html("此分销非您创建的分销商，请给 易城分销商 授权！")
                            //$("input:radio[name='warrant_type'][value=" + data.msg.Warrant_type + "]").attr("checked", true);
                        }

                    } else {
                        $("input:radio[name='warrant_type'][value=" + data.msg.Warrant_type + "]").attr("checked", true);
                        $("input:radio[name='warrant_type']").attr("readonly", "readonly"); //已授权用户进行修改
                        $("input:radio[name='warrant_state'][value=" + data.msg.Warrant_state + "]").attr("checked", true);
                        if (data.msg.Warrant_state == -1) {
                            $("#state").html("新注册未授权用户，请选择开通状态！");
                        }



                        $("input:radio[name='warrant_level'][value=" + data.msg.Warrant_level + "]").attr("checked", true);
                        $("#confirmpub").html("确认 修改此授权信息")
                    }
                }

            })



            $("#confirmpub").click(function () {


                var warrant_type = $('input[name=warrant_type]:checked').val();
                var warrant_state = $('input[name=warrant_state]:checked').val();
                var warrant_level = $('input[name=warrant_level]:checked').val();

                if (warrant_state == "") {
                    $.prompt("请选择开通状态");
                    return;
                }


                $.post("/JsonFactory/AgentHandler.ashx?oper=modifyAgentExt", { agentid: agentid, comid: comid, warrant_type: warrant_type, warrant_state: warrant_state, warrant_level: warrant_level }, function (data) {
                    data = eval('(' + data + ')');
                    if (data.type == '100') {
                        $.prompt("操作成功");
                        //location.href = "Agentlist.aspx";
                        return;
                    } else {
                        //                        $.prompt("失败，操作出错！");
                        $.prompt(data.msg);
                        return;
                    }
                })
            })

        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
      <%--  <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="AgentList.aspx" onfocus="this.blur()" target=""><span>分销商管理</span></a></li>
                
                 
            </ul>
        </div>--%>

        <input type="hidden" id="hid_agentid" value='<%=agentid %>' />
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    </h3>
                <h3>
                    <a href="AgentManageOrder.aspx?agentid=<%=agentid %>" class="a_anniu">订单列表</a>  &nbsp; <a href="AgentVCount.aspx?agentid=<%=agentid %>" class="a_anniu">分销验票统计</a> &nbsp; <a href="AgentVerification.aspx?agentid=<%=agentid %>" class="a_anniu" >分销验票记录</a>  &nbsp;  <a href="AgentManageFinance.aspx?agentid=<%=agentid %>" class="a_anniu" >财务管理</a> &nbsp;  <a href="AgentRechargeFinance.aspx?agentid=<%=agentid %>" class="a_anniu" >充值记录</a> &nbsp;  <a href="AgentManageSet.aspx?agentid=<%=agentid %>"  class="a_anniu">管  理</a></h3>
                
                <h3>
                    </h3>
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
                   <h2 class="p-title-area">授权分销商</h2>
                   <div class="mi-form-item">
                        <label class="mi-label"> 公司名称</label>
                        <span id="company" style=" padding:40px;"></span>
                   </div>
                                      <div class="mi-form-item">
                        <label class="mi-label"> 登陆账户</label>
                        <span id="Account" style=" padding:40px;"></span>
                   </div>



                   <div class="mi-form-item">
                        <label class="mi-label"> 公司联系人姓名</label>
                         <span id="name" style=" padding:40px;"></span>
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 联系电话</label>
                         <span id="tel" style=" padding:40px;"></span>
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 联系手机</label>
                         <span id="mobile" style=" padding:40px;"></span>
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 预付款</label>
                         <span id="imprest" style=" padding:40px;"></span>
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 信用额度</label>
                         <span id="credit" style=" padding:40px;"></span>
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 授权类型</label>
                        <input type="radio" name="warrant_type" value="1" checked />出票扣款(适合接口或后台出票) <input type="radio" name="warrant_type"  value="2" /> 验码扣款(适合接口或者倒码)
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 分销级别</label>
                        <input type="radio" name="warrant_level" value="1" checked />一级
                        <input type="radio" name="warrant_level" value="2" /> 二级
                        <input type="radio" name="warrant_level" value="3" /> 三级 
                        （享受相当级别价格，如果产品此级别价格为0则为限制订购）
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 授权状态</label>
                         <input type="radio" name="warrant_state" value="0" />关闭  <input type="radio" name="warrant_state"  value="1" /> 开通 <div id="state" style=" color:#ff0000"></div>
                   </div>
                   <div class="mi-form-explain"></div>
               </div>

                <table border="0">
                    <tr>
                      <td width="600" height="80" align="center">
							 <input type="button" name="confirmpub" id="confirmpub" value="  确    认  "  class="mi-input" />
							 <input type="button" name="Submit" value="返回上一步" onClick=" history.back()" class="mi-input">
                    </td>
                    </tr>
                </table>
            </div>
        </div>

    </div>
    <div class="data">
    </div>
</asp:Content>
