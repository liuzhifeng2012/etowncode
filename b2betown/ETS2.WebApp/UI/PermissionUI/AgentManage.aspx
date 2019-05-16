<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="AgentManage.aspx.cs"
    Inherits="ETS2.WebApp.UI.PermissionUI.AgentManage" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
   <link href="../../Styles/permissionUI/agentmanage.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            var agentid = $("#hid_agentid").trimVal();
            SearchList();


            function SearchList() {
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=companygetagentinfo",
                    data: { agentid: agentid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#maxremindmoney").val(data.msg.maxremindmoney);
                            $("#Company").val(data.msg.Company);
                            $("#name").val(data.msg.Contentname);
                            $("#tel").val(data.msg.Tel);
                            $("#phone").val(data.msg.Mobile);
                            $("#address").val(data.msg.Address);
                            $("#agentdomain").val(data.msg.Agent_domain);
                            $("input:radio[name='Run_state'][value=" + data.msg.Run_state + "]").attr("checked", true);
                            //                            $("#com_province").val(data.msg.com_province);
                            //                            $("#com_city").val(data.msg.com_city);
                            if (data.msg.com_city != "") {
                                $("#com_city").append("<option value='" + data.msg.com_city + "' selected>" + data.msg.com_city + "</option>");
                            }
                            if (data.msg.com_province != "") {
                                $("#com_province option[value='" + data.msg.com_province + "']").attr("selected", true);
                            }
                            //开通淘宝码商
                            $("input:radio[name='istaobao'][value='" + data.msg.istaobao + "']").attr("checked", "checked");
                            //                            $("#tb_syncurl").val(data.msg.tb_syncurl);
                            //                            $("input:radio[name='tb_isret_consumeresult'][value='" + data.msg.tb_isret_consumeresult + "']").attr("checked", "checked");

                            //是否特属美团团购
                            $("input[name='ismeituan'][value='" + data.msg.ismeituan + "']").attr("checked", "checked");
                            if (data.msg.ismeituan == 1) {
                                $(".meituan_tr").css("display", "");
                                $(".lvmama").css("display", "none");
                            }
                            if (data.msg.ismeituan == 3) {
                                $(".lvmama").css("display", "");
                                $(".meituan_tr").css("display", "none");
                            }

                            $("#Lvmama_uid").val(data.msg.Lvmama_uid);
                            $("#Lvmama_password").val(data.msg.Lvmama_password);
                            $("#Lvmama_Apikey").val(data.msg.Lvmama_Apikey);


                            $("#mt_partnerId").val(data.msg.mt_partnerId);
                            $("#mt_client").val(data.msg.mt_client);
                            $("#mt_secret").val(data.msg.mt_secret);
                            $("#mt_mark").val(data.msg.mt_mark);

                            //分销商美团配置信息一旦添加，不可再次修改,防止出现混乱
                            if (data.msg.ismeituan==1) {
                                $("input[name='ismeituan'][value='0']").attr("disabled", "true");
                                $("#mt_partnerId").attr("readonly", "readonly");
                                $("#mt_client").attr("readonly", "readonly");
                                $("#mt_secret").attr("readonly", "readonly");
                                $("#mt_mark").attr("readonly", "readonly");
                            }

                        }
                    }
                })


            }

            $("#confirmButton").click(function () {
                var Company = $("#Company").trimVal();
                var name = $("#name").trimVal();
                var tel = $("#tel").trimVal();
                var phone = $("#phone").trimVal();
                var address = $("#address").trimVal();
                var agentdomain = $("#agentdomain").trimVal();
                var Run_state = $('input:radio[name="Run_state"]:checked').val();
                if (Company == "") {
                    $.prompt("公司名称不能为空");
                    return;
                }
                if (name == "") {
                    $.prompt("请填写姓名");
                    return;
                }
                //                if (phone == "") {
                //                    $.prompt("请填写手机号");
                //                    return;
                //                } else {
                //                    if (!isMobel(phone)) {
                //                        $.prompt("请正确填写手机号");
                //                        return;
                //                    }
                //                }

                var com_province = $("#com_province").trimVal();
                var com_city = $("#com_city").trimVal();


                if (com_province == "" || com_province == "省份") {
                    $.prompt("请选择所在省份");
                    return;
                }
                if (com_city == "" || com_city == "城市") {
                    //                    $.prompt("请选择所属城市");
                    //                    return;
                    com_city == "";
                }

                //开通淘宝码商
                var istaobao = $("input:radio[name='istaobao']:checked").trimVal();
                var tb_syncurl = $("#tb_syncurl").trimVal();
                var tb_isret_consumeresult = $("input:radio[name='tb_isret_consumeresult']:checked").trimVal();

                var maxremindmoney = $("#maxremindmoney").trimVal();
                if (maxremindmoney == "") {
                    alert("最大额度不可为空");
                    return;
                }
                else {
                    if (parseInt(maxremindmoney) < 1000) {
                        alert("最大额度不可小于1000");
                        return;
                    }
                }

                //判断美团团购配置信息
                var ismeituan = $("input[name='ismeituan']:checked").val();
                var mt_partnerId = $("#mt_partnerId").trimVal();
                var mt_client = $("#mt_client").trimVal();
                var mt_secret = $("#mt_secret").trimVal();
                var mt_mark = $("#mt_mark").trimVal();


                if (ismeituan == 1) {
                    if (mt_partnerId == "") {
                        alert("请填写合作商ID！"); 
                        $("#mt_partnerId").focus();
                        return;
                    }
                    if (mt_client == "") {
                        alert("请填写合作商账户！");
                        $("#mt_client").focus();
                        return;
                    }
                    if (mt_secret == "") {
                        alert("请填写合作商秘钥！");
                        $("#mt_secret").focus();
                        return;
                    }
                    if (mt_mark == "") {
                        alert("请填写美团备注！");
                        $("#mt_mark").focus();
                        return;
                    }
                }


                var Lvmama_uid = $("#Lvmama_uid").trimVal();
                var Lvmama_password = $("#Lvmama_password").trimVal();
                var Lvmama_Apikey = $("#Lvmama_Apikey").trimVal();

                if (ismeituan == 3) {
                    if (Lvmama_uid == "") {
                        alert("请填写合作商UID！");
                        $("#Lvmama_uid").focus();
                        return;
                    }
                    if (Lvmama_password == "") {
                        alert("请填写合作商账户密码！");
                        $("#Lvmama_password").focus();
                        return;
                    }
                    if (Lvmama_Apikey == "") {
                        alert("请填写合作商秘钥！");
                        $("#Lvmama_Apikey").focus();
                        return;
                    }
                }



                //提交修改
                $.post("/JsonFactory/AgentHandler.ashx?oper=AdminAgentUp", { ismeituan: ismeituan, mt_partnerId: mt_partnerId, mt_client: mt_client, mt_secret: mt_secret, mt_mark: mt_mark, maxremindmoney: maxremindmoney, agentid: agentid, Company: Company, name: name, tel: tel, phone: phone, address: address, Run_state: Run_state, com_province: com_province, com_city: com_city, agentdomain: agentdomain, istaobao: istaobao, tb_syncurl: tb_syncurl, tb_isret_consumeresult: tb_isret_consumeresult, Lvmama_uid: Lvmama_uid, Lvmama_password: Lvmama_password, Lvmama_Apikey: Lvmama_Apikey }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        //                        $.prompt("修改成功"); 
                        //                        window.open("AgentList.aspx", target = "_self");
                        if (confirm("修改成功。是否返回列表页面？")) {
                            window.open("AgentList.aspx", target = "_self");
                            return;
                        }
                        else {
                            return;
                        }

                    }
                })

            })

            $("#cancel_rh").click(function () {
                $("#div_taobao").hide();
            })
            $("#newbtn").click(function () {

                //判断是否都保存了 合作卖家信息
                var issaved = 1;
                $('[name="issaved"]').each(function (i, value) {
                    if ($.trim($(this).val()) == "0") {
                        issaved = 0;
                    }
                })

                if (issaved == 0) {
                    alert("合作卖家信息需要全部保存！");
                    return;
                }
                $("#tblist").append('<tr>' +

                     '<td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;" >' +
                   '淘宝旺旺ID:' +
                    '<input   class="dataNum dataIcon" name="tb_seller_wangwangid" id="tb_seller_wangwangid0" value="" />' +
                    '</td>' +
                     '<td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">' +
                   '淘宝旺旺号:' +
                    '<input   class="dataNum dataIcon"  name="tb_seller_wangwang"  id="tb_seller_wangwang0" value="" /> ' +
                '</td>' +
                  '<td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;display:none;">' +
                    '淘宝ID(选填):' +
                    '<input   class="dataNum dataIcon" name="tb_id" id="tb_id0" value="" placeholder="淘宝店域名后的ID数字" />' +
                    '</td>' +
            '</tr>' +

                 '<tr>' +
                '<td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">' +
                 '淘宝店铺名称:' +
                    '<input   class="dataNum dataIcon"  name="tb_shop_name"  id="tb_shop_name0" value="" /> ' +
                    '</td>' +
                     '<td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">' +
                    '淘宝店铺地址:' +
                    '<input   class="dataNum dataIcon"  name="tb_shop_url"  id="tb_shop_url0" value="" /> ' +
                '</td>' +
                 '<td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px; display:;">店铺状态:' +
                  '开通' +
                 '<input type="hidden" id="tb_shop_state0" value="1" >' +
                 '</td>' +
                '</tr> ' +
         '<tr>' +
                '<td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;" colspan="3"> ' +
                    '<input type="button" id="nbtn0" value="  添加  " onclick="addtaobao(0)" /> ' +
                    '<input type="hidden" id="nhid0" name="issaved" value="0">' +
                   ' <hr>' +
                '</td>' +
           ' </tr>');
            })

            //是否特属美团团购
            $("input[name='ismeituan']").change(function () {
                var ismeituan = $("input[name='ismeituan']:checked").val();
                //                alert(ismeituan);
                if (ismeituan == 1) {
                    $(".meituan_tr").css("display", "");
                    $(".lvmama").css("display", "none");
                }
                else if (ismeituan == 3) {
                    $(".lvmama").css("display", "");
                    $(".meituan_tr").css("display", "none");
                }
                else {
                    $(".meituan_tr").css("display", "none");
                    $(".lvmama").css("display", "none");
                }
            })
        })
        function taobaoshow() {
            //查询已添加的合作卖家
            $.post("/JsonFactory/AgentHandler.ashx?oper=GetTb_agent_relationList", { agentid: $("#hid_agentid").trimVal() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {
                    $("#tblist").empty();
                    if (data.msg == '') {
                        $("#tblist").html("<tr><td colspan='2'>还暂未添加合作卖家信息</td></tr>");
                    } else {
                        $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                    }
                }
            })
            $("#div_taobao").show();
        }
        function addtaobao(serialnum) {
            var agentid = $("#hid_agentid").trimVal();
            var tb_id = $.trim($("#tb_id" + serialnum).trimVal());
            var tb_seller_wangwangid = $.trim($("#tb_seller_wangwangid" + serialnum).trimVal());
            var tb_seller_wangwang = $.trim($("#tb_seller_wangwang" + serialnum).trimVal());
            var tb_shop_name = $.trim($("#tb_shop_name" + serialnum).trimVal());
            var tb_shop_url = $.trim($("#tb_shop_url" + serialnum).trimVal());
            var tb_shop_state = $.trim($("#tb_shop_state" + serialnum).trimVal());
            //                if (tb_id == "") {
            //                    alert("淘宝ID不可为空");
            //                    return;
            //                }
            if (tb_seller_wangwangid == "") {
                alert("淘宝旺旺ID不可为空");
                return;
            } else {
                if (isNaN(tb_seller_wangwangid)) {
                    alert("请到码商后台合作卖家列表中查询正确的淘宝旺旺ID!");
                    return;
                }
            }
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

            $.post("/JsonFactory/AgentHandler.ashx?oper=EditTb_agent_relation", { serialnum: serialnum, agentid: agentid, tb_id: tb_id, tb_seller_wangwangid: tb_seller_wangwangid, tb_seller_wangwang: tb_seller_wangwang, tb_shop_name: tb_shop_name, tb_shop_url: tb_shop_url, tb_shop_state: tb_shop_state }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    alert(data.msg);
                    return;
                }
                if (data.type == 100) {

                    taobaoshow();


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
                <li><a href="/ui/PermissionUI/MasterList.aspx" onfocus="this.blur()" target=""><span>
                    人员管理</span></a></li>
                <%--  <li><a href="notelist.aspx" onfocus="this.blur()" target=""><span>短信管理</span></a></li>--%>
                <li><a href="/ui/PermissionUI/SSort.aspx" onfocus="this.blur()" target=""><span>商户管理</span></a></li>
                <li class="on"><a href="/ui/PermissionUI/AgentList.aspx" onfocus="this.blur()" target="">
                    <span>分销商管理</span></a></li>
                <li><a href="/ui/PermissionUI/ticketlist.aspx" onfocus="this.blur()"><span>退票管理</span></a></li>
                <li><a href="/agent/outinterface.aspx?agentid=<%=Agentid %>" onfocus="this.blur()"><span>
                    对外产品接口参数</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <table width="700px" class="grid">
                    <tr>
                        <td colspan="2">
                            <h3>
                                分销商信息管理</h3>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead" style="width: 150px;">
                            公司名称 :
                        </td>
                        <td>
                            <input name="Input" class="dataNum dataIcon" id="Company" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead" style="width: 80px;">
                            绑定域名 :
                        </td>
                        <td>
                            <input name="Input" class="dataNum dataIcon" id="agentdomain" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            联系人姓名 :
                        </td>
                        <td>
                            <input name="Input" class="dataNum dataIcon" id="name" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            联系电话 :
                        </td>
                        <td>
                            <input name="Input" class="dataNum dataIcon" id="tel" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            联系手机:
                        </td>
                        <td>
                            <input name="Input" class="dataNum dataIcon" id="phone" value="" readonly="readonly"
                                style="background-color: Gray;" /><label>需要在分销商后台修改</label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            地区
                        </td>
                        <td>
                            <select name="com_province" id="com_province">
                                <option value="省份" selected="selected">省份</option>
                            </select>
                            <select name="com_city" id="com_city">
                                <option value="城市" selected="selected">市县</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            地址 :
                        </td>
                        <td>
                            <input name="Input" class="dataNum dataIcon" id="address" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            账户状态 :
                        </td>
                        <td>
                            <input name="Run_state" id="Run_state1" type="radio" value="0" checked="checked" />
                            开通
                            <input name="Run_state" id="Run_state2" type="radio" value="-1" />
                            暂停
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            是否开通淘宝 :
                        </td>
                        <td>
                            <label>
                                <input type="radio" value="1" name="istaobao">是</label>
                            <label>
                                <input type="radio" value="0" name="istaobao" checked="checked">否</label>
                            <label>
                                <a href="javascript:void(0);" onclick="taobaoshow()" style="text-decoration: underline;
                                    color: Blue; font-size: 15px;">淘宝合作卖家编辑</a>
                            </label>
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td class="tdHead">
                            同步url地址 :
                        </td>
                        <td>
                            
                            <input name="Input" class="dataNum dataIcon" id="tb_syncurl" value="http://shop.etown.cn/taobao_ms/notice.ashx"
                                style="width: 400px; background-color: Gray;" readonly="readonly" />
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdHead">
                            是否返回使用报告 :
                        </td>
                        <td>
                            <label>
                                <input type="radio" value="1" name="tb_isret_consumeresult" checked="checked">是</label>
                            <%--  <label>
                                <input type="radio" value="0" name="tb_isret_consumeresult">否</label>--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdHead">
                            是否特特殊渠道（易城开发接口渠道）：
                        </td>
                        <td>
                            <label>
                                <input type="radio" value="0" name="ismeituan">普通分销商
                            </label>
                            <label>
                                <input type="radio" value="1" name="ismeituan">美团
                            </label>
                            <label>
                                <input type="radio" value="3" name="ismeituan" >驴妈妈
                            </label>
                        </td>
                    </tr>
                    <tr class="meituan_tr" style="display:none;">
                        <td colspan="2">
                            <ul>
                                <li><h3>请配置美团相关数据(信息一旦设定则不可更改)</h3> </li>
                                <li>
                                   <label>合作商ID(partnerId)* </label>
                                   <div>
                                    <input class="form-control" id="mt_partnerId" value="" />
                                  </div>
                                </li>
                                <li><label>合作商账户(clientId)* </label>
                                    <div>
                                       <input class="form-control" id="mt_client" value="" />
                                   </div>
                                </li>
                                <li>
                                     <label>合作商秘钥(clientSecret)* </label>
                                     <div>
                                        <input class="form-control" id="mt_secret" value="" />
                                     </div>
                                </li>
                                <li>
                                     <label>备注* </label>
                                     <div>
                                        <input class="form-control" id="mt_mark" value="" placeholder="例如：万达商户 使用；对接人：小明" />
                                     </div>
                                </li>
                            </ul>
                        </td>
                    </tr>
                    <tr class="lvmama" style="display:none;">
                        <td colspan="2">
                            <ul>
                                <li><h3>驴妈妈渠道配置</h3> </li>
                                <li>
                                   <label>合作商ID(uid)* </label>
                                   <div>
                                    <input class="form-control" id="Lvmama_uid" value="" />
                                  </div>
                                </li>
                                <li><label>合作商账户密码(password)* </label>
                                    <div>
                                       <input class="form-control" id="Lvmama_password" value="" />
                                   </div>
                                </li>
                                <li>
                                     <label>合作商秘钥(Apikey)* </label>
                                     <div>
                                        <input class="form-control" id="Lvmama_Apikey" value="" />
                                     </div>
                                </li>
                            </ul>
                        </td>
                    </tr>

                    <tr>
                        <td class="tdHead">
                            分销商余额不足提醒最高额度:
                        </td>
                        <td>
                            <label>
                                <input name="Input" class="dataNum dataIcon" id="maxremindmoney" value="1000" placeholder="不少于1000的整数" />
                            </label>
                        </td>
                    </tr>
                </table>
                <table width="300px" class="grid">
                    <tr>
                        <td class="tdHead">
                            <input id="confirmButton" type="button" value="    保存公司信息   " name="confirmButton"></input>
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
    <div id="div_taobao" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 800px; height: auto; display: none; left: 10%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#E7F0FA"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="3" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 12px; font-weight: bold" id="span1">淘宝合作卖家信息
                    </span>
                    <input type="button" id="newbtn" value="  新增  " />
                    <span style="font-size: 20px;">旺旺ID!=旺旺号。请到码商后台合作卖家列表中查询正确的淘宝旺旺ID!<br>
                    </span><a href="javascript:void(0)" id="cancel_rh" style="float: right; text-decoration: underline;
                        color: Blue; font-size: 15px;">关 闭</a>
                </td>
            </tr>
            <tbody id="tblist">
            </tbody>
        </table>
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">
     <tr>
              
                     <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                   淘宝旺旺ID:
                   {{if $.trim(tb_seller_wangwangid)==""}}
                    <input   class="dataNum dataIcon" name="tb_seller_wangwangid" id="tb_seller_wangwangid${serialnum}" value="${tb_seller_wangwangid}" />
                    {{else}}
                     ${tb_seller_wangwangid}
                     <input type="hidden"   name="tb_seller_wangwangid" id="tb_seller_wangwangid${serialnum}" value="${tb_seller_wangwangid}" /> 
                    {{/if}}
                    </td>
                     <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                   淘宝旺旺号:${tb_seller_wangwang} 
                    <input type="hidden"    name="tb_seller_wangwang"  id="tb_seller_wangwang${serialnum}" value="${tb_seller_wangwang}"  /> 
                </td>
                  <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;display:none;">
                    淘宝ID(选填):${tb_id} 
                    <input type="hidden"    name="tb_id" id="tb_id${serialnum}" value="${tb_id}" />
                    </td>
            </tr>
            
                 <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                 淘宝店铺名称:${tb_shop_name}
                    <input type="hidden"    name="tb_shop_name"  id="tb_shop_name${serialnum}" value="${tb_shop_name}"  /> 
                    </td>
                     <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    淘宝店铺地址:${tb_shop_url}
                    <input  type="hidden"  name="tb_shop_url"  id="tb_shop_url${serialnum}" value="${tb_shop_url}"  /> 
                </td>
                 <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px; display:;">店铺状态:{{if tb_shop_state=="1"}}
                  开通
                  {{else}}
                  暂停
                 {{/if}} 
                 <input type="hidden" id="tb_shop_state${serialnum}" value="${tb_shop_state}" >
                 </td>
                </tr> 
         <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;" colspan="3">  
                {{if $.trim(tb_seller_wangwangid)==""}}
                    <input type="button" id="nbtn${serialnum}" value="  添加  " onclick="addtaobao('${serialnum}')" /> 
                    <input type="hidden" id="nhid${serialnum}" name="issaved" value="0">
                    {{else}}
                     <label style="font-size:15px;color:blue;">已审核通过</label>
                      <input type="hidden" name="issaved" value="1">
                    {{/if}} 
                    <hr>
                </td>
            </tr>
    </script>
    <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
    <script type="text/javascript">
        var province = document.getElementById('com_province');
        var city = document.getElementById('com_city');
    </script>
    <script src="/Scripts/City.js" type="text/javascript"></script>
</asp:Content>
