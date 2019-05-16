<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master"  CodeBehind="AgentManageFinance.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.AgentManageFinance" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 100; //每页显示条数

        $(function () {
            //判断是否可以设置预付款
            $.post("/JsonFactory/PermissionHandler.ashx?oper=getGroupByUserId", { userid: $("#hid_userid").trimVal() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {
                    if (data.msg.iscanset_imprest == 0) {
                        $("#add_imprest").attr("disabled", "disabled");
                        $("#reduce_imprest").attr("disabled", "disabled");

                    }
                    if (data.msg.iscanset_imprest == 1) {
                        $("#add_imprest").removeAttr("disabled");
                        $("#reduce_imprest").removeAttr("disabled");

                    }
                }
            })



            var agentid = $("#hid_agentid").trimVal();
            var comid = $("#hid_comid").trimVal();



            $.post("/JsonFactory/AgentHandler.ashx?oper=getAgentId", { agentid: agentid, comid: comid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("获取数据出现错误");
                    return;
                }
                if (data.type == 100) {
                    $("#company").html(data.msg.Company);
                    $("#imprest").html("￥ " + data.msg.Imprest);
                    $("#credit").html(data.msg.Credit);
                    $("#warrant_level").html(data.msg.Warrant_level);
                    if (data.msg.Warrant_type == 1) {
                        $("#warrant_type").html("出票扣款");
                    } else if (data.msg.Warrant_type == 2) {
                        $("#warrant_type").html("验证扣款");
                    } else {
                        $("#warrant_type").html("尚未设定或设定错误");
                    }
                }

            })



            SearchList(1);

            //装载产品列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=agentFinacelist",
                    data: { comid: comid, agentid: agentid, pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询财务列表错误");
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

            //充预付款
            $("#add_imprest").click(function () {
                $("#Operating").show();
                $("#OperatingTitle").html("充预付款");
                $("#OperatingName").html($("#company").html());
                $("#OperatingType").html("本次充预付款金额");
                $("#acttype").val("add_imprest");
                $("#money").val("0");
            })

            //减预付款
            $("#reduce_imprest").click(function () {
                $("#Operating").show();
                $("#OperatingTitle").html("减预付款");
                $("#OperatingName").html($("#company").html());
                $("#OperatingType").html("本次减预付款金额");
                $("#acttype").val("reduce_imprest");
                $("#money").val("0");
            })

            //设置信用额度
            $("#setcredit").click(function () {
                $("#Operating").show();
                $("#Rebatetype").hide();
                $("#OperatingTitle").html("设置信用额度");
                $("#OperatingName").html($("#company").html());
                $("#OperatingType").html("信用额度");
                $("#acttype").val("setcredit");
                $("#money").val($("#credit").html());
            })


            $("#cancel").click(function () {
                $("#acttype").val("");
                $("#Operating").hide();
            })
            $("#cancelmember").click(function () {
                $("#MemberInfo").hide();
            })


            $("#submit").click(function () {
                var acttype = $("#acttype").val();
                var money = $("#money").val();
                var ordername = $("#ordername").val();
                var Rebatetype = $('input:radio[name="Rebatetype"]:checked').trimVal();



                if ($("#acttype").val() != "setcredit") {
                    if (Rebatetype == "" || Rebatetype == null) {
                        $.prompt("请选择 操作的是 预付款还是返点！");
                        return;
                    }
                }


                if (money == 0 || money == "") {
                    $.prompt("请填写金额");
                    return;
                }
                if (ordername == "") {
                    $.prompt("请填写备注，已被后续核对");
                    return;
                }

                if (acttype == "") {
                    $.prompt("出错，请重新操作");
                    return;
                }

                $("#loading").show();
                $("#submit").val("处理中").attr("disabled","disabled");
                if (acttype == "setcredit") {
                    $.post("/JsonFactory/AgentHandler.ashx?oper=setagentcredit", { agentid: agentid, comid: comid, money: money, ordername: ordername }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $.prompt("操作数据出现错误");
                            $("#loading").hide();
                            $("#submit").val(" 确 定 ").removeAttr("disabled");
                            return;
                        }
                        if (data.type == 100) {
                            if (data.msg == 0) {
                                $.prompt("参数传递出错，请重新操作");
                                $("#loading").hide();
                                $("#submit").val(" 确 定 ").removeAttr("disabled");
                                return;
                            } else if (data.msg != 0) {
                                $.prompt("设定成功");
                                $("#acttype").val("");
                                $("#money").val(0);
                                $("#Operating").hide();
                                $("#loading").hide();
                                $("#submit").val(" 确 定 ").removeAttr("disabled");
                                location.reload();
                                return;
                            }
                        }
                    })

                } else {

                    $.post("/JsonFactory/AgentHandler.ashx?oper=writeagentmoney", { agentid: agentid, comid: comid, acttype: acttype, money: money, ordername: ordername, Rebatetype: Rebatetype, userid: $("#hid_userid").trimVal() }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $.prompt("操作数据出现错误");
                            $("#loading").hide();
                            $("#submit").val(" 确 定 ").removeAttr("disabled");
                            return;
                        }
                        if (data.type == 100) {
                            if (data.msg == 0) {
                                $.prompt("参数传递出错，请重新操作");
                                $("#loading").hide();
                                $("#submit").val(" 确 定 ").removeAttr("disabled");
                                return;
                            } else if (data.msg != 0) {
                                $.prompt("录入成功");
                                $("#acttype").val("");
                                $("#money").val(0);
                                $("#Operating").hide();
                                $("#loading").hide();
                                $("#submit").val(" 确 定 ").removeAttr("disabled");
                                location.reload();
                                return;
                            }
                        }
                    })
                }


            })


        })

    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
      <%--  <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="AgentList.aspx" onfocus="this.blur()" target=""><span>分销商管理</span></a></li>
                
                 
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone  mail-list">
            <div class="inner">
             <h3>
                    授权分销商</h3>
                    <table >
                    <tr>
                        <td class="tdHead">
                            公司名称：<span id="company"></span> </td>

                    </tr>
                    <tr>
                        <td class="tdHead">
                           预付款： <span id="imprest"></span> <input type="button" name="add_imprest" id="add_imprest" value=" 人工充值  " /> <input type="button" name="reduce_imprest" id="reduce_imprest" value=" 扣减预付款  " />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                           信用额度： <span id="credit"></span> <input type="button" name="setcredit" id="setcredit" value=" 设置信用额度  " /> 
                        </td>

                    </tr>
                     <tr>
                        <td class="tdHead">
                           授权类型： <span id="warrant_type"></span>
                        </td>

                    </tr>
                    <tr>
                        <td class="tdHead">
                           授权级别： <span id="warrant_level"></span>
                        </td>

                    </tr>
                </table>

                <h3>
                    <a href="AgentManageOrder.aspx?agentid=<%=agentid %>" class="a_anniu">订单列表</a>  &nbsp; <a href="AgentVCount.aspx?agentid=<%=agentid %>" class="a_anniu">分销验票统计</a> &nbsp; <a href="AgentVerification.aspx?agentid=<%=agentid %>" class="a_anniu" >分销验票记录</a>  &nbsp;  <a href="AgentManageFinance.aspx?agentid=<%=agentid %>" class="a_anniu" >财务管理</a> &nbsp;  <a href="AgentRechargeFinance.aspx?agentid=<%=agentid %>" class="a_anniu" >充值记录</a> &nbsp;  <a href="AgentManageSet.aspx?agentid=<%=agentid %>"  class="a_anniu">管  理</a></h3>
                    
                    <table width="780" border="0">
                    <tr>
                        <td width="41">
                            <p align="left">
                                流水号</p>
                        </td>
                        <td width="80">
                            <p align="left">
                                操作时间
                            </p>
                        </td>
                        <td width="204">
                            <p align="left">
                                内容
                            </p>
                      </td>
                        <td width="90">
                            <p align="left">
                                收支类型
                            </p>
                      </td>
                        <td width="50">
                            <p align="left">
                                收入
                            </p>
                      </td>
                        <td width="44">
                            <p align="left">
                                支出
                            </p>
                      </td>
                        <td width="66">
                            <p align="left">
                                余额
                            </p>
                      </td>
                        <td width="79">
                            <p align="left">
                                资金渠道
                            </p>
                      </td>
                        <td width="72">
                            <p align="left">&nbsp;
                          </p>
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
                        <td >
                            <p align="left">
                                ${Id}</p>
                        </td>
                        <td>
                            <p align="left" title="${jsonDateFormatKaler(Subdate)}">
                                ${jsonDateFormatKaler(Subdate)}
                            </p>
                        </td>
                        <td >
                            <p align="left" title="${Servicesname}">
                                 ${Servicesname}
                                {{if PayNo != null }}
                                    ${PayNo.Trade_no}
                                {{/if}} 
                                </p>
                        </td>
                        <td >
                            <p align="left">
                                ${Payment_type}
                                {{if Rebatetype==1}} -返点{{/if}}
                                
                                </p>
                        </td>
                        <td >
                            <p align="left">
                                {{if Money>= 0}}${Money}{{/if}}</p>
                        </td>
                        <td >
                            <p align="left">
                              {{if Money< 0}}${Money}{{/if}}</p>
                        </td>
                        <td>
                            <p align="left">
                                ${Over_money}    
                            </p>
                        </td>
                        <td >
                            <p align="left">
                                ${Money_come}</p>
                        </td>
                        <td >
                            <p align="left">
                            ${Payment_type}${Remarks}
                            </p>
                        </td>
                    </tr>
    </script>

    <div id="Operating" style="background-color:#ffffff;border:2px solid #5984bb;  margin:0px auto;width:400px; height:230px;display:none;left:20%; position:absolute; top:20%;">
                <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999" style="padding:5px;">
                  <tr>
                    <td height="42" colspan="2" bgcolor="#C1D9F3" ><span style=" padding-left:10px; font-size:18px;" id="OperatingTitle"></span></td>
                  </tr>
                  <tr>
                    <td width="40%" height="35" align="right" bgcolor="#E7F0FA" >分销商: </td>
                    <td width="60%"  bgcolor="#E7F0FA"><span id="OperatingName"></span></td>
                  </tr>
                  <tr>
                    <td height="38" align="right" bgcolor="#E7F0FA" > <span id="OperatingType"></span> : </td>
                    <td bgcolor="#E7F0FA">
                        <input name="money" id="money" type="text" size="10" value="0" />
                    </td>
                  </tr>
                  <tr id="Rebatetype">
                    <td height="38" align="right" bgcolor="#E7F0FA" >金额类型: </td>
                    <td bgcolor="#E7F0FA">
                        <label><input name="Rebatetype" type="radio" value="0" /> 预付款</label>
                         <label><input name="Rebatetype" type="radio" value="1" /> 返点 </label>
                    </td>
                  </tr>
                  <tr>
                            <td height="38" align="right" bgcolor="#E7F0FA">
                                <span id="">备注说明</span>:
                            </td>
                            <td bgcolor="#E7F0FA">
                                <input name="ordername" id="ordername" type="text" maxlength="50" size="20"  />
                            </td>
                            
                        </tr>
                  <tr>
                    <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input name="acttype" id="acttype" type="hidden" value="0" />
					<input name="submit" id="submit" type="button" class="formButton" value="  确  定  " /> 
					<input name="cancel" id="cancel" type="button" class="formButton" value="  取  消  " /></td>
                  </tr>
                </table>
			</div>	
    <input type="hidden" id="hid_agentid" value='<%=agentid %>' />
     <div id="loading" class="loading" style="display: none;">
            正在加载...
   </div>
</asp:Content>

