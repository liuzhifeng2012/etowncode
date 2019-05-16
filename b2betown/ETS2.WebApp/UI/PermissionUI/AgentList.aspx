<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="AgentList.aspx.cs"
    Inherits="ETS2.WebApp.UI.PermissionUI.AgentList" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 100; //每页显示条数

        $(function () {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();

            //获得分销类型
            $.post("/JsonFactory/AgentHandler.ashx?oper=GetAgentSourceSortlist", {}, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {
                    if (data.msg.length > 0) {
                        var optionstr = "";
                        for (var i = 0; i < data.msg.length; i++) {

                            optionstr += '<option value="' + data.msg[i].key + '" >' + data.msg[i].value + '</option>';

                        }
                        $("#agentsourcesort").html(optionstr);
                        $("#sel_sourcesort").html(optionstr);
                        
                    }
                }
            })
            //获取地区列表
            $.post("/JsonFactory/AgentHandler.ashx?oper=managegetagentprovincelist", {}, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {
                    if (data.msg == "") {
                        $("#com_province").html('<option value="省份" selected="selected">省份</option>');
                    } else {
                        var provincestr = '<option value="省份" selected="selected">省份</option>';
                        for (var i = 0; i < data.msg.length; i++) {
                            provincestr += '<option value="' + data.msg[i] + '" >' + data.msg[i] + '</option>';
                        }
                        $("#com_province").html(provincestr);
                    }

                }
            })

            SearchList(1, '', '', '',0);

            //装载产品列表
            function SearchList(pageindex, com_province, com_city, key, agentsourcesort) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=manageagentpagelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, com_province: com_province, com_city: com_city, Key: key, agentsourcesort: agentsourcesort },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询产品列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                //                                $("#tblist").html("查询数据为空");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pageSize, pageindex, com_province, com_city, key, agentsourcesort);
                            }


                        }
                    }
                })


            }

            $("#Search").click(function () {
                var com_province = $("#com_province").trimVal();
                var com_city = $("#com_city").trimVal();

                if (com_province == "" || com_province == "省份") {
                    //                    $.prompt("请选择所在省份");
                    //                    return;
                    com_province = ""
                }
                if (com_city == "" || com_city == "城市") {
                    //                    $.prompt("请选择所属城市");
                    //                    return;
                    com_city = ""
                }
                var key = $("#key").trimVal();
                var agentsourcesort = $("#agentsourcesort").trimVal();

                SearchList(1, com_province, com_city, key, agentsourcesort);
            })

            //分页
            function setpage(newcount, newpagesize, curpage, com_province, com_city, key, agentsourcesort) {
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

                        SearchList(page, com_province, com_city, key, agentsourcesort);

                        return false;
                    }
                });
            }


        })

        //----此部分是行业部分BEGIN---//
        function changehangye() {
            var agentid = $("#hid_edit_agentid").val();
            var Agentsort = $("#sel_hangye").val();

            var checktext = $("#sel_hangye").find("option:selected").text();
            $.post("/JsonFactory/AgentHandler.ashx?oper=ChangeAgentsort", { Agentsort: Agentsort, agentid: agentid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert("调整失败");
                } else {
                    alert("调整成功");
                    $("#Span_company").text("");
                    //退票
                    $("#div_showhangye").hide();
                    if (checktext.length > 4) {
                        checktext = checktext.substr(0,4)+"..";
                    }
                    $("#td_agentsortvalue_" + agentid).text(checktext);
                }
            })
        }
        function hangyecancel() {
            $("#Span_company").text("");
            $("#div_showhangye").hide();


            $("#span_agentcompany").text("");
            $("#div_agentmsgset").hide();

            $("#Span4").text("");
            $("#div_sourcesort").hide();
        }
        function hangye_set(agentid, agentcompany, Agentsort) {
            $("#Span_company").html(agentcompany);
            $("#hid_edit_agentid").val(agentid);
            $("#sel_hangye").find("option[val='" + Agentsort + "']").attr("selected", true);
            $("#div_showhangye").show();
        }
        function sourcesort_set(agentid, agentcompany, Agentsourcesort) {
            $("#Span4").html(agentcompany);
            $("#hid_up_agentid").val(agentid);
            $("#sel_sourcesort").find("option[val='" + Agentsourcesort + "']").attr("selected", true);
            $("#div_sourcesort").show();
        }

        function changesourcesort() {
            var agentid = $("#hid_up_agentid").val();
            var Agentsourcesort = $("#sel_sourcesort").val();

            var checktext = $("#sel_sourcesort").find("option:selected").text();
            $.post("/JsonFactory/AgentHandler.ashx?oper=ChangeAgentsourcesort", { Agentsourcesort: Agentsourcesort, agentid: agentid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert("调整失败");
                } else {
                    alert("调整成功");
                    $("#Span4").text("");
                    $("#div_sourcesort").hide();
                    if (checktext.length > 4) {
                        checktext = checktext.substr(0, 4) + "..";
                    }
                    $("#td_agentsourcesortvalue_" + agentid).text(checktext);
                }
            })
         }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <%--  <li><a href="FuncModuleList.aspx" onfocus="this.blur()" target=""><span>权限管理</span></a></li>
                <li><a href="RoleList.aspx" onfocus="this.blur()" target=""><span>管理组管理</span></a></li>--%>
                <li><a href="MasterList.aspx" onfocus="this.blur()" target=""><span>人员管理</span></a></li>
                <%--  <li><a href="notelist.aspx" onfocus="this.blur()" target=""><span>短信管理</span></a></li>--%>
                <li><a href="SSort.aspx" onfocus="this.blur()" target=""><span>商户管理</span></a></li>
                <li class="on"><a href="AgentList.aspx" onfocus="this.blur()" target=""><span>分销商管理</span></a></li>
                <li><a href="ComFinance.aspx" onfocus="this.blur()" target=""><span>财务对账表</span></a></li>
                <li><a href="Withdraw_handle.aspx" onfocus="this.blur()" target=""><span>提现财务管理</span></a></li>
                <li><a href="ticketlist.aspx" onfocus="this.blur()"><span>退票管理</span></a></li>
                <!--<li><a href="bankmanager.aspx" onfocus="this.blur()"><span>绑定银行管理</span></a></li>-->
                <li><a href="Modellist.aspx" onfocus="this.blur()" target=""><span>模板管理</span></a></li>
                <li><a href="ProClass.aspx" onfocus="this.blur()" target=""><span>产品类目</span></a></li>
            </ul>
        </div>
        <div class="mi-form-item">
            <select name="com_province" id="com_province" class="mi-input" style="width: 120px;">
                <option value="省份" selected="selected">省份</option>
            </select>
            <select name="com_city" id="com_city" class="mi-input" style="width: 120px;">
                <option value="城市" selected="selected">市县</option>
            </select>
            <span>分销类型 </span>
            <select id="agentsourcesort" class="mi-input" style="width: 120px;">
                <option value="0" selected="selected">类 别</option>
            </select>
            <span>关键词</span><input name="key" type="text" id="key" class="mi-input" style="width: 120px;" />
            <input name="Search" type="button" id="Search" value="查询" style="width: 120px; height: 26px;" />
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    分销商列表</h3>
                <table width="780" border="0">
                    <tr>
                        <td width="20">
                            <p align="left">
                                ID</p>
                        </td>
                        <td width="120">
                            <p align="left">
                                分销商公司名称
                            </p>
                        </td>
                        <td width="80">
                            <p align="left">
                                开户账户
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                手机号
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                联系人姓名
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                账户类型
                            </p>
                        </td>
                        <td width="20">
                            <p align="left">
                                状态
                            </p>
                        </td>
                         <td width="50">
                            <p align="left">
                                分销类型
                            </p>
                        </td>
                        <td width="70">
                            <p align="left">
                                分销短信设置
                            </p>
                        </td>
                        <td width="20">
                            <p align="left">
                                &nbsp;</p>
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
                                ${Company}
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                ${Account}
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                ${Mobile}</p>
                        </td>
                                                <td>
                            <p align="left">
                                ${Name}</p>
                        </td>
                         <td>
                         <p align="left">
                             <span id="td_agentsortvalue_${Id}"> ${Agentsortvalue.length>4?Agentsortvalue.substr(0,4)+"..":Agentsortvalue}</span>
                               <input type="button"  onclick="hangye_set('${Id}','${Company}','${Agentsort}')"  value="管理"/>
                               </p>
                        </td>
                                                 <td>
                            <p align="left">
                                {{if Run_state==0}}运行{{else}}暂停{{/if}}</p>
                        </td>
                         <td>
                         <p align="left">
                              <span id="td_agentsourcesortvalue_${Id}">  ${Agentsourcesortvalue.length>4?Agentsourcesortvalue.substr(0,4)+"..":Agentsourcesortvalue}</span>
                               <input type="button"  onclick="sourcesort_set('${Id}','${Company}','${agentsourcesort}')"  value="管理"/>
                               </p>
                        </td>
                        <td>
                         <span  id="td_msgset_${Id}">
                           {{if Agent_messagesetting==0}}
                            易城商户发送 
                            {{else}}
                            分销自己发送 
                            {{/if}}
                            </span>
                       <input type="button" id="btn_${Id}" value="调整" onclick="agent_msgset(${Id},'${Company}',${Agent_messagesetting})"/>
                        </td>
                        <td>
                            <p align="left">
                             <a href="AgentManage.aspx?agentid=${Id}">管理 </a>  &nbsp;
                            </p>
 
                        </td>
                    </tr>
    </script>
    <script type="text/javascript">
        function agent_msgset(agentcompanyid, agentcompany, agent_messagesetting) {
            $("#span_agentcompany").html(agentcompany);
            $("#hid_agentcompanyid").val(agentcompanyid);
            $("#agent_messagesetting").find("option[val='" + agent_messagesetting + "']").attr("selected", true);
            $("#div_agentmsgset").show();
        }
        function changemsgset() {
            var agentid = $("#hid_agentcompanyid").val();
            var agent_messagesetting = $("#agent_messagesetting").val();
            var checktext = $("#agent_messagesetting").find("option:selected").text();
            $.post("/JsonFactory/AgentHandler.ashx?oper=ChangeAgentMsgset", { agent_messagesetting: agent_messagesetting, agentid: agentid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert("调整短信设置失败");
                } else {
                    alert("调整成功");
                    $("#span_agentcompany").text("");
                    //退票
                    $("#div_agentmsgset").hide();

                    $("#td_msgset_" + agentid).html(checktext);
                }
            })
        }
    </script>
    <div id="div_showhangye" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 350px; height: 200px; display: none; left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#E7F0FA"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 12px; font-weight: bold" id="span1">分销商账户类型管理
                    </span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    分销商名称：<span id="Span_company"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    账户类型：
                    <select id="sel_hangye">
                        <option value="0" selected="selected">票务分销</option>
                        <option value="1">微站渠道</option>
                        <option value="2">商家项目账户(查看验证数据)</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#E7F0FA" class="tdHead">
                    <input type="hidden" id="hid_edit_agentid" value="0" />
                    <input id="Button1" type="button" class="formButton" onclick="changehangye()" value="  提  交  " />
                    <input name="cancel_rh" type="button" class="formButton" onclick="hangyecancel();"
                        value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>

    <div id="div_sourcesort" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 350px; height: 200px; display: none; left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#E7F0FA"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 12px; font-weight: bold" id="span3">分销商类型管理
                    </span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    分销商名称：<span id="Span4"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    账户类型：
                    <select id="sel_sourcesort">
                     
                    </select>
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#E7F0FA" class="tdHead">
                    <input type="hidden" id="hid_up_agentid" value="0" />
                    <input id="Button3" type="button" class="formButton" onclick="changesourcesort()" value="  提  交  " />
                    <input name="cancel_rh" type="button" class="formButton" onclick="hangyecancel();"
                        value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>

    <div id="div_agentmsgset" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 350px; height: 200px; display: none; left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#E7F0FA"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 12px; font-weight: bold" id="span2">分销商
                    </span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    分销商名称：<span id="span_agentcompany"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    短信设置：
                    <select id="agent_messagesetting">
                        <option value="0">易城商户发送</option>
                        <option value="1">分销自己发送</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#E7F0FA" class="tdHead">
                    <input type="hidden" id="hid_agentcompanyid" value="0" />
                    <input id="Button2" type="button" class="formButton" onclick="changemsgset()" value="  提  交  " />
                    <input name="cancel_rh" type="button" class="formButton" onclick="hangyecancel();"
                        value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        var province = document.getElementById('com_province');
        var city = document.getElementById('com_city');
    </script>
    <script src="/Scripts/City.js" type="text/javascript"></script>
</asp:Content>
