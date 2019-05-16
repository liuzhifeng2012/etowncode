<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="LPManage.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.LPManage" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").trimVal();
            //根据当前用户获得商家信息---需要处理
            $.ajax({
                type: "post",
                url: "/JsonFactory/AccountInfo.ashx?oper=getcurcompany",
                data: { comid: $("#hid_comid").trimVal() },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取信息出错，" + data.msg);
                    }
                    if (data.type == 100) {

                        $("input:radio[value='" + data.msg.Lp + "']").attr('checked', 'true');

                        $("#LP_AgentLevel option[value='" + data.msg.Lp_agentlevel + "']").attr("selected", true);

                    }
                }
            })

            //提交按钮
            $("#btn-submit").click(function () {
                var LP = $('input[name="LP"]:checked').val(); ;
                var LP_AgentLevel = $("#LP_AgentLevel").val();

                if (LP == 1) {
                    if (LP_AgentLevel == "") {
                        $.prompt("请选择授权级别！");
                        return;
                    }
                }

                $("#loading").html("正在提交，请稍后...")

                //创建订单
                $.post("/JsonFactory/AccountInfo.ashx?oper=editlp", { comid: comid, lp: LP, lp_agentlevel: LP_AgentLevel }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("修改成功！");
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
      <%--  <div id="secondary-tabs" class="navsetting ">
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
                    <a href="AgentList.aspx" class="a_anniu">分销商列表</a> <a href="AgentRegi.aspx" class="a_anniu">新增分销商</a> <a href="LPManage.aspx" class="b_anniu">平台分销商设定</a> </h3>
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        平台分销商授权设定</h2>
                    <div class="mi-form-item" style=" line-height:180%">
                        <label class="mi-label">
                           授权设置 </label>
                         <label> <input name="LP" type="radio" value="1" style="background-color: rgb(204, 204, 204);" /> 授权平台更多分销商  </label>(您的产品将有机会让平台更多分销商看到)         
                         <br>  <label> <input name="LP" type="radio" value="0" style="background-color: rgb(204, 204, 204);" /> 暂停  </label>(选择暂停后平台分销商不能看到您的产品)
                         <br>
                    </div>
                    <div class="mi-form-item" style=" line-height:180%">
                        <label class="mi-label">
                            授权级别</label>
                        <select id="LP_AgentLevel" class="mi-input" style="width: 210px;">
                            <option value="1">一级分销</option>
                            <option value="2">二级分销</option>
                            <option value="3" selected>三级分销</option>
                        </select>
                         <br> (平台分销将自动按此级别扣款，其中如果某产品此级别设定价格为0，则表示此级别分销不可销售。)
                         <br>
                    </div>
                    <div class="mi-form-item" style=" line-height:180%">
                        <label class="mi-label">
                            授权类型</label>
                           <input type="text" value="销售扣款" class="mi-input" disabled />
                            
                        
                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <table width="600px;">
                    <tr>
                        <td align="center">
                            <input type="button" name="Search" id="btn-submit" class="mi-input" value="  确认 平台分销商授权设置  " />
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
</asp:Content>
