<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="PublishEdit.aspx.cs"
    Inherits="ETS2.WebApp.UI.MemberUI.PublishEdit" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">


        //编辑发行管理
        $(function () {
            var comid = $("#hid_comid").trimVal();
            var issueid = $("#hid_issueid").trimVal();

            //给渠道类型赋初始值
            ShowUnitList("0"); //默认显示内部渠道单位
            //给卡片类型赋初始值
            $.post("/JsonFactory/CardHandler.ashx?oper=pagelist", { comid: $("#hid_comid").trimVal(), pageindex: "1", pagesize: "1000" }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("获得卡片类型初始值出现错误");
                    return;
                }
                if (data.type == 100) {
                    $("#select").empty();
                    for (var i = 0; i < data.msg.length; i++) {
                        $("#select").append("<option value=" + data.msg[i].Id + ">" + data.msg[i].Cname + "</option>");

                    }

                    ShowCardDetail(data.msg[0].Id);
                }
            })
            //给促销活动赋初始值
            $.post("/JsonFactory/PromotionHandler.ashx?oper=pagelist", { comid: $("#hid_comid").trimVal(), pageindex: "1", pagesize: "1000" }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("给促销活动附初始值出错");
                    return;
                }
                if (data.type == 100) {
                    $("#spancuxiao").empty();
                    for (var i = 0; i < data.msg.length; i++) {
                        $("#spancuxiao").append('<label><input type="checkbox" name="checkbox" value="' + data.msg[i].Id + '">' + data.msg[i].Title + '</label><br>');
                    }
                }
            })
            if (issueid != "0") {//编辑发行操作
                //根据发行id得到发行信息
                $.post("/JsonFactory/IssueHandler.ashx?oper=GetIssueDetailById", { issueid: issueid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("根据发行id得到发行信息出错");
                        return;
                    }
                    if (data.type == 100) {
                        var cardid = data.msg.Crid;
                        var channelid = data.msg.Chid;
                        $("#hid_issueid").val(data.msg.Id);
                        $("#hid_channelid").val(channelid);
                        $("#hid_cardid").val(cardid);

                        $("#textfield4").val(data.msg.Title);
                        $("#textfield2").val(data.msg.Num);
                        $("input[name='radiobutton1'][value='" + data.msg.Openyes + "']").attr("checked", true);
                        $("input[name='checkbox5'][value='" + data.msg.Openaddress + "']").attr("checked", true);
                        ShowCardDetail(cardid);

                        //根据发行id给促销活动列表赋值
                        $.post("/JsonFactory/IssueHandler.ashx?oper=GetIssuePromot", { "issueid": issueid }, function (data) {
                            data = eval("(" + data + ")");
                            if (data.type == 1) {
                                $.prompt("根据发行id给促销活动列表赋值出错");
                                return;
                            }
                            if (data.type == 100) {
                                for (var i = 0; i < data.msg.length; i++) {
                                    var actidd = data.msg[i].Acid;
                                    $("input[name='checkbox'][value='" + actidd + "']").attr("checked", true);
                                }
                            }
                        })

                        //判断渠道是否存在，存在显示详细；否则为空
                        $.post("/JsonFactory/ChannelHandler.ashx?oper=getchanneldetail", { channelid: channelid }, function (data) {
                            data = eval("(" + data + ")");
                            if (data.type == 1) {
                                $.prompt("获取渠道信息出错");
                                return;
                            }
                            if (data.type == 100) {
                                //渠道类型赋值
                                //                                $("#select2").val(data.msg.Issuetype);
                                $("#select2").empty();
                                if (data.msg.Issuetype == "0") {
                                    $("#select2").append("<option value='" + data.msg.Issuetype + "'>内部渠道</option>");
                                }
                                if (data.msg.Issuetype == "1") {
                                    $("#select2").append("<option value='" + data.msg.Issuetype + "'>外部渠道</option>");
                                }
                                //渠道单位赋值
                                $("#select3").empty();

                                var companyidd = data.msg.Companyid;
                                //添加外部或者内部渠道单位列表
                                $.post("/JsonFactory/ChannelHandler.ashx?oper=getunitlistneww", { unittype: data.msg.Issuetype,comid:comid }, function (data) {
                                    data = eval("(" + data + ")");
                                    if (data.type == 1) {
                                        $.prompt("获取渠道单位列表出现问题");
                                        return;
                                    }
                                    if (data.type == 100) {
                                        for (var i = 0; i < data.msg.length; i++) {
                                            if (companyidd == data.msg[i].Id) {
                                                $("#select3").append("<option value='" + data.msg[i].Id + "'>" + data.msg[i].Companyname + "</option>");

                                            }
                                        }
                                    }
                                })

                                //                                //给发行人赋值
                                //                                ShowChannelDetail(data.msg.Id);
                                //发行人
                                $("#select5").empty();
                                $("#select5").append("<option value=" + data.msg.Id + ">" + data.msg.Name + "</option>");

                                $("#textfield322").val(data.msg.Cardcode);

                                $("#tdfanyong").html("有效开卡奖励 " + data.msg.RebateOpen + "元 第一次成交奖励 " + data.msg.RebateConsume + "元");


                            }
                        })

                    }
                })
            }
            //卡片类型变动
            $("#select").change(function () {
                var seled = $(this).val();

                ShowCardDetail(seled);
            })
            //渠道单位
            $("#select2").change(function () {
                var seled = $(this).find("option:selected").val();
                ShowUnitList(seled);
            })
            //点击渠道单位得到发行人基本信息
            $("#select3").change(function () {
                var seled = $(this).find("option:selected").val();
                ShowChannelList(seled);
            })
            //点击发行人姓名得到不同的渠道信息
            $("#select5").change(function () {
                var seled = $(this).find("option:selected").val();

                //判断渠道是否存在，存在显示详细；否则为空
                $.post("/JsonFactory/ChannelHandler.ashx?oper=getchanneldetail", { channelid: seled }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取渠道信息出错");
                        return;
                    }
                    if (data.type == 100) {
                        $("#textfield322").val(data.msg.Cardcode);
                        $("#tdfanyong").html("有效开卡奖励 " + data.msg.RebateOpen + "元 第一次成交奖励 " + data.msg.RebateConsume + "元");

                        //给隐藏列渠道id赋值
                        $("#hid_channelid").val(data.msg.Id);
                    }
                })
            })


            function ShowUnitList(issuetype) {
                $("#select3").empty();
                //添加外部或者内部渠道单位列表
                $.post("/JsonFactory/ChannelHandler.ashx?oper=getunitlistneww", { unittype: issuetype,comid:comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取渠道单位列表出现问题");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg == null || data.msg == "") {
                            $.prompt("渠道单位列表为空，请先添加？",
                                {
                                    buttons: [{ title: "确定", value: true}],
                                    show: "slideDown",
                                    submit: function (e, v, m, f) {
                                        if (v == true) {
                                            window.open("ChannelList.aspx", target = "_self");
                                        }
                                    }
                                });
                            return;
                        }
                        else {
                            for (var i = 0; i < data.msg.length; i++) {
                                $("#select3").append("<option value='" + data.msg[i].Id + "'>" + data.msg[i].Companyname + "</option>");
                            }
                            //根据companyid获得选中项渠道单位的渠道信息
                            if (data.msg.length == 0) {
                                $.prompt("渠道单位列表为空，请先添加？",
                                {
                                    buttons: [{ title: "确定", value: true}],
                                    show: "slideDown",
                                    submit: function (e, v, m, f) {
                                        if (v == true) {
                                            window.open("ChannelList.aspx", target = "_self");
                                        }
                                    }
                                });
                                return;
                            }
                            else {

                                ShowChannelList(data.msg[0].Id);
                            }


                        }
                    }
                })
            }
            //根据companyid获得选中项渠道单位的渠道信息
            function ShowChannelList(companyid) {
                $.post("/JsonFactory/ChannelHandler.ashx?oper=GetChannelByCompanyid", { companyid: companyid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("根据公司id得到渠道信息列表出错");
                        return;
                    }
                    if (data.type == 100) {
                        //给发行人姓名赋值
                        $("#select5").empty();
                        for (var i = 0; i < data.msg.length; i++) {
                            $("#select5").append("<option value=" + data.msg[i].Id + ">" + data.msg[i].Name + "</option>");
                        }
                        //默认显示第一条渠道记录的信息
                        if (data.msg.length == 0) {
                            //                            //如果渠道单位没有绑定发行人，提示，然后跳到渠道信息编辑页面
                            //                            $.prompt("渠道单位没有绑定发行人，请先添加？",
                            //                                {
                            //                                    buttons: [{ title: "确定", value: true}],
                            //                                    show: "slideDown",
                            //                                    submit: function (e, v, m, f) {
                            //                                        if (v == true) {
                            //                                            window.open("ChannelList.aspx", target = "_self");
                            //                                        }
                            //                                    }
                            //                                });
                            //                            return;
                        } else {
                            //默认显示第一个发行人的信息
                            $("#select5").val(data.msg[0].Id);
                            $("#textfield322").val(data.msg[0].Cardcode);
                            $("#tdfanyong").html("有效开卡奖励 " + data.msg[0].RebateOpen + "元 第一次成交奖励 " + data.msg[0].RebateConsume + "元");

                            //给隐藏列渠道id赋值
                            $("#hid_channelid").val(data.msg[0].Id);
                        }
                    }
                })
            }

            //获得卡片详情（暂时写成静态的）
            function ShowCardDetail(cardid) {
                $("#select").val(cardid);
                $("#tdcardtype").empty().html("实体会员卡");
                $("#hid_cardid").val(cardid);
            }




            //完成设置，开始发行
            $("#aedit").click(function () {
                var pubtitle = $("#textfield4").trimVal();
                var channelid = $("#hid_channelid").trimVal();
                var cardid = $("#hid_cardid").trimVal();

                var num = $("#textfield2").trimVal();
                var actchecked = ""; //选中的活动
                $("input[name='checkbox']:checkbox:checked").each(function () {

                    actchecked += $(this).val() + ",";

                })
                actchecked = actchecked.substring(0, actchecked.length - 1);

                var isopen = $('input[name="radiobutton1"]:checked').val();

                var openaddress = ""; //开卡地点
                $("input[name='checkbox5']:checkbox:checked").each(function () {
                    openaddress += $(this).val() + ",";
                })
                openaddress = openaddress.substring(0, openaddress.length - 1);

                if (pubtitle == "") {
                    $.prompt("标题没有填写");
                    return;
                }
                if (channelid == "") {
                    $.prompt("channelid 没有选择");
                    return;
                }
                if (cardid == "") {
                    $.prompt("cardid没有选择");
                    return;
                }
                if (num == "") {
                    $.prompt("请选择发行量");
                    return;
                }
                if (actchecked == "") {
                    $.prompt("请选择促销活动");
                    return;
                }
                if (isopen == "") {
                    $.prompt("请选择是否需要开卡");
                    return;
                }
                if (openaddress == "") {
                    $.prompt("请选择开卡地点");
                    return;
                }
                else {
                    $.post("/JsonFactory/IssueHandler.ashx", { oper: "editissue", comid: $("#hid_comid").trimVal(), issueid: issueid, pubtitle: pubtitle, channelid: channelid, cardid: cardid, num: num, actchecked: actchecked, isopen: isopen, openaddress: openaddress }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            $.prompt("编辑发布信息出错");
                            return;
                        } else {
                            $.prompt("编辑发行成功", {
                                buttons: [
                                { title: '确定', value: true }

                                     ],
                                opacity: 0.1,
                                focus: 0,
                                show: 'slideDown',
                                submit: callbackfunc
                            });
                        }
                        function callbackfunc(e, v, m, f) {
                            if (v == true)
                                location.href = "PublishList.aspx";
                        }
                    })
                }

            })
        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="CardList.aspx" onfocus="this.blur()"><span>卡片管理</span></a></li>
                <li><a href="CardEdit.aspx" onfocus="this.blur()"><span>添加卡片</span></a></li>
                <li><a href="membercardlist.aspx" onfocus="this.blur()"><span>已录入卡号列表</span></a></li>
                <li><a href="PublishList.aspx" onfocus="this.blur()"  ><span>发行管理</span></a></li>
                <li class="on"><a href="PublishEdit.aspx" onfocus="this.blur()"  >
                    <span>添加发行</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    </h3>

                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
                               <h2 class="p-title-area">发行设置</h2>
                               <div class="mi-form-item">
                                    <label class="mi-label">填写发行标题</label>
                                    <input name="textfield4" type="text" id="textfield4" value="" size="65"  class="mi-input" />
                               </div>
                               <div class="mi-form-item">
                                    <label class="mi-label">选择渠道类型</label>
                                    <select name="select2" id="select2">
                                        <option value="0">内部渠道</option>
                                        <option value="1">外部渠道</option> 
                                    </select>
                                    <select name="select3" id="select3">
                                    <%--    <option value="1">广发银行</option>
                                        <option value="2">招商银行</option>
                                        <option value="3">市场部</option>--%>
                                    </select>
                               </div>
                               <div class="mi-form-item">
                                    <label class="mi-label">选择已有卡片</label>
                                    
                                    <label>
                                        <input type="radio" name="radiobutton" value="radiobutton" checked="checked">
                                        手工录入已有卡号
                                    </label>
                                    <%-- <label>
                                    <input type="radio" name="radiobutton" value="radiobutton">
                                    整批导入
                                    </label>--%>
                                    <select name="select" id="select">
                                       <%-- <option value="1" selected>出境游500元实体会员卡</option>
                                        <option value="2">电子会员卡</option>
                                        <option value="3">储值礼品卡</option>--%>
                                    </select>
                               </div>


                               <div class="mi-form-item">
                                    <label class="mi-label">发行量</label>
                                    
                                     <input name="textfield2" type="text" id="textfield2" value="" size="10" class="mi-input" >                          张
                               </div>
                               <div class="mi-form-item">
                                    <label class="mi-label">选择促销活动</label>
                                     <span id="spancuxiao">
                               <%-- <label>
                                    <input type="checkbox" name="checkbox" value="1" checked="checked">
                                    500元出境旅游现金优惠卡道
                                </label>
                                <label>
                                    <input type="checkbox" name="checkbox" value="2">
                                    门票促销道
                                </label>
                                <label>
                                    <input type="checkbox" name="checkbox" value="3">
                                    在线开卡送10元
                                </label>--%>
                                 </span>
                               </div>
                               <div class="mi-form-explain"></div>
                </div>

                 <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
                               <h2 class="p-title-area">发行人设置</h2>
                               <div class="mi-form-item">
                                    <label class="mi-label">发行人姓名</label>
                                    <select name="select5" id="select5" >
                              
                                    </select>
                                    （根据上面渠道单位进行人员选择）
                               </div>
                               <div class="mi-form-item">
                                    <label class="mi-label">发行人卡号</label>
                                     <input name="textfield322" type="text" id="textfield322" value=""  readonly/>
                            （根据选择自动调出）
                               </div>
                               <div class="mi-form-item">
                                    <label class="mi-label">奖励佣金</label>
                                   <span id="tdfanyong"></span>
                               </div>
                               <div class="mi-form-explain"></div>
                </div>
                 <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10; display:none;">
                               <h2 class="p-title-area">开卡设置</h2>
                               <div class="mi-form-item">
                                    <label class="mi-label">是否需要开卡</label>
                                    <label>
                                <input type="radio" name="radiobutton1" value="true" checked="checked">
                                需要开卡
                               </div>
                               <input type="checkbox" name="checkbox5" value="1" checked>
                                    全部
                            
                               <div class="mi-form-explain"></div>
                </div>

                <table width="780" border="0">
                    <tr>
                        <td width="699" height="44" align="center">
                            <a href="javascript:void(0)" class="font_14" id="aedit"><strong>完成设置，开始发行</strong></a>
                        </td>
                        <td width="59">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <input type="hidden" id="hid_issueid" value="<%=issueid %>" />
    <input type="hidden" id="hid_channelid" value="" />
    <input type="hidden" id="hid_cardid" value="" />
</asp:Content>
