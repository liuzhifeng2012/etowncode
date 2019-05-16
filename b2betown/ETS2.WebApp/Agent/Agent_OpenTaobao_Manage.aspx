<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Agent_OpenTaobao_Manage.aspx.cs"
    Inherits="ETS2.WebApp.Agent.Agent_OpenTaobao_Manage" MasterPageFile="/Agent/Manage.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {


            var serialnum = $("#hid_serialnum").trimVal();
            if (serialnum != 0) {
                var agentid = $("#hid_agentid").trimVal();
                $.post("/JsonFactory/AgentHandler.ashx?oper=GetTb_agent_relation", { serialnum: serialnum, agentid: agentid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) { }
                    if (data.type == 100) {
                        if (data.msg.toLowerCase() == "null") {
                            alert("查询出错"); return;
                        }
                        else {
                            $("#tb_id").val(data.msg.tb_id);
                            $("#tb_seller_wangwang").val(data.msg.tb_seller_id);

                            $("#tb_shop_name").val(data.msg.tb_shop_name);
                            $("#tb_shop_url").val(data.msg.tb_shop_url);

                        }
                    }
                })
            }

            $("#confirmButton").click(function () {
                var agentid = $("#hid_agentid").trimVal();
                var serialnum = $.trim($("#hid_serialnum").trimVal());
                var tb_id = $.trim($("#tb_id").trimVal());
                var tb_seller_wangwang = $.trim($("#tb_seller_wangwang").trimVal());

                var tb_shop_name = $.trim($("#tb_shop_name").trimVal());
                var tb_shop_url = $.trim($("#tb_shop_url").trimVal());
                var tb_shop_state = 1;
//                if (tb_id == "") {
//                    alert("淘宝ID不可为空");
//                    return;
//                }
                if (tb_seller_wangwang == "") {
                    alert("淘宝旺旺号不可为空");
                    return;
                }
                if (tb_shop_name == "") {
                    alert("淘宝店铺名称不可为空");
                    return;
                }
                if (tb_shop_url == "") {
                    alert("淘宝店铺地址不可为空");
                    return;
                }
                $.post("/JsonFactory/AgentHandler.ashx?oper=EditpartTb_agent_relation", { serialnum: serialnum, agentid: agentid, tb_id: tb_id, tb_seller_wangwang: tb_seller_wangwang, tb_shop_name: tb_shop_name, tb_shop_url: tb_shop_url, tb_shop_state: tb_shop_state }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        alert(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        alert("提交审核成功，请等待通知");
                        window.open("Agent_OpenTaobao.aspx", target = "_self");
                    }
                })

            })
        })
    </script>
    <style type="text/css">
        .dataIcon
        {
            width: 400px;
        }
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
       <ul class="composetab">
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_unsel"><div><a href="AgentStaff.aspx">员工管理</a></div></div>
            </li>
             <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_unsel"><div><a href="AgentCompany.aspx">分销商信息</a></div></div>
            </li>
             <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_sel"><div><a href="Agent_OpenTaobao.aspx">淘宝店铺</a></div></div>
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
                <h3>
                    开通淘宝</h3>
                <table width="700px" class="grid">
                    <tr style="display:none;">
                        <td class="tdHead" style="width: 200px;">
                            淘宝ID(选填) :
                        </td>
                        <td>
                            <input name="Input" class="dataNum dataIcon" id="tb_id" value="" placeholder="请填写淘宝店域名后的ID数字" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            淘宝旺旺号 :
                        </td>
                        <td>
                            <input name="Input" class="dataNum dataIcon" id="tb_seller_wangwang" value="" placeholder="请填写淘宝旺旺号" />
                        </td>
                    </tr> 
                    <tr>
                        <td class="tdHead">
                            淘宝店铺名称 :
                        </td>
                        <td>
                            <input name="Input" class="dataNum dataIcon" id="tb_shop_name" value="" placeholder="请填写淘宝店铺名称" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            淘宝店铺地址 :
                        </td>
                        <td>
                            <input name="Input" class="dataNum dataIcon" id="tb_shop_url" value="" placeholder="请填写淘宝店铺地址" />
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td class="tdHead">
                            淘宝店铺状态 :
                        </td>
                        <td>
                            <label>
                                <input type="radio" value="1" name="tb_shop_state" checked="checked" />开通</label>
                            <label>
                                <input type="radio" value="0" name="tb_shop_state" />暂停</label>
                        </td>
                    </tr>
                </table>
                <table width="300px" class="grid">
                    <tr>
                        <td class="tdHead">
                            <input id="confirmButton" type="button" value="    保存并提交审核   " />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
    <input id="hid_comid_temp" type="hidden" value="<%=comid_temp %>" />
    <!--淘宝分销关系表 标识列-->
    <input id="hid_serialnum" type="hidden" value="<%=serialnum %>" />
</asp:Content>
