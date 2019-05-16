<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="PromotionEdit.aspx.cs"
    Inherits="ETS2.WebApp.UI.MemberUI.PromotionEdit" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {


            //日历
            var nowdate = '<%=this.nowdate %>';
            var monthdate = '<%=this.monthdate %>';
            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {

                $("#Actstar").val(nowdate);
                $("#Actend").val(monthdate);

                $($(this)).datepicker({
                    numberOfMonths: 2,
                    minDate: 0,
                    defaultDate: +4,
                    maxDate: '+8m +1w'
                });
            });
            var acttype = $("#hid_acttype").trimVal();
            var faceObjects = $("#hid_faceObjects").trimVal();

            $("#UseChannelAll").click(function () {
                var UseChannelAll = "";
                UseChannelAll = $('input[name="UseChannelAll"]:checked').val();

                if (UseChannelAll == 1) {
                    $("input[name='UseChannel']").attr("checked", true);
                } else {
                    $("input[name='UseChannel']").attr("checked", false);
                }
            })



            //首先加载数据
            var hid_actid = $("#hid_actid").trimVal();
            if (hid_actid != '0') {
                //判断是否可以对活动进行编辑
                $.post("/JsonFactory/PromotionHandler.ashx?oper=WhetherEditById", { actid: hid_actid, operuserid: $("#hid_userid").trimVal() }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 100) {
                        if (data.data == 1) {
                            $("#GoActAddNext").removeAttr("disabled");
                        } else {
                            $("#GoActAddNext").attr("disabled", "disabled");
                        }
                    }
                    if (data.type == 1) {
                        $("#GoActAddNext").attr("disabled", "disabled");
                    }
                })

                //获取活动信息
                $.post("/JsonFactory/PromotionHandler.ashx?oper=getActById", { actid: hid_actid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取数据出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#title").val(data.msg.Title);
                        $("#hid_acttype").val(data.msg.Acttype);
                        $("#Acttype").find("option[value='" + data.msg.Acttype + "']").attr("selected", true);


                        $("#Money").val(data.msg.Money);
                        $("#Discount").val(data.msg.Discount);
                        $("#CashFull").val(data.msg.CashFull);
                        $("#Cashback").val(data.msg.Cashback);
                        $("#Actstar").val(ChangeDateFormat(data.msg.Actstar));
                        $("#Actend").val(ChangeDateFormat(data.msg.Actend));
                        $("#ReturnAct").val(data.msg.ReturnAct);
                        $("#atitle").val(data.msg.Atitle);
                        $("#usetitle").val(data.msg.Usetitle);
                        $("#remark").val(data.msg.Remark);
                        $("#useremark").val(data.msg.Useremark);


                        $("input:radio[name='FaceObjects'][value=" + data.msg.FaceObjects + "]").attr("checked", true);
                        $("input:radio[name='UseOnce'][value=" + data.msg.UseOnce + "]").attr("checked", true);
                        $("input:radio[name='RepeatIssue'][value=" + data.msg.RepeatIssue + "]").attr("checked", true);
                        $("input:radio[name='Runstate'][value=" + data.msg.Runstate + "]").attr("checked", true);


                        var Acttype_hid = data.msg.Acttype;
                        if (Acttype_hid == "1" || Acttype_hid == "4") {
                            $("#Acttype1").show();
                            $("#Acttype2").hide();
                            $("#Acttype3").hide();
                        }
                        if (Acttype_hid == "2") {
                            $("#Acttype1").hide();
                            $("#Acttype2").show();
                            $("#Acttype3").hide();
                        }
                        if (Acttype_hid == "3") {
                            $("#Acttype1").hide();
                            $("#Acttype2").hide();
                            $("#Acttype3").show();
                        }


                    }
                })
            } else {//新增活动

                //对活动类型判断显示不同内容
                $("#Acttype2").hide();
                $("#Acttype3").hide();

                var Acttype_hid = $("#hid_acttype").trimVal();
                if (Acttype_hid == "1" || Acttype_hid == "4") {
                    $("#Acttype1").show();
                    $("#Acttype2").hide();
                    $("#Acttype3").hide();
                }
                if (Acttype_hid == "2") {
                    $("#Acttype1").hide();
                    $("#Acttype2").show();
                    $("#Acttype3").hide();
                }
                if (Acttype_hid == "3") {
                    $("#Acttype1").hide();
                    $("#Acttype2").hide();
                    $("#Acttype3").show();
                }

                $("input:radio[name='FaceObjects'][value=" + faceObjects + "]").attr("checked", true);
                $("#Acttype").val(acttype);

            }
            //加载可用门市
            $.post("/JsonFactory/ChannelHandler.ashx?oper=getunitlistneww", { unittype: 2, comid: $("#hid_comid").trimVal(), userid: $("#hid_userid").trimVal(), actid: $("#hid_actid").trimVal() }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("获取数据出现错误");
                    return;
                }
                if (data.type == 100) {
                    $("#UseChannel_model").html("");
                    var groupstr = "";
                    for (var i = 0; i < data.msg.length; i++) {
                        if (data.msg[i].Companyname != "") {
                            if (data.msg[i].Selectstate==0){
                                groupstr += ' <label > <input  name="UseChannel" type="checkbox" value="' + data.msg[i].Id + '" > ' + data.msg[i].Companyname + '</label><br> ';
                            }else{
                               groupstr += ' <label > <input  name="UseChannel" type="checkbox" value="' + data.msg[i].Id + '" checked="checked"> ' + data.msg[i].Companyname + '</label><br> ';
                            }
                        }
                    }
                    $("#UseChannel_model").html(groupstr);
                }
            })




            //第二步按钮
            $("#GoActAddNext").click(function () {
                var hid_actid = $("#hid_actid").trimVal();
                var comid = $("#hid_comid").trimVal();
                var title = $("#title").trimVal();

                var Acttype = $("#Acttype").val();
                //                alert(Acttype);
                if (Acttype == "") {
                    Acttype = $("#hid_acttype").val();
                    alert(Acttype);
                }


                var Money = $("#Money").val();
                var Discount = $("#Discount").trimVal();
                var CashFull = $("#CashFull").trimVal();
                var Cashback = $("#Cashback").trimVal();
                var Actstar = $("#Actstar").trimVal();
                var Actend = $("#Actend").trimVal();
                var ReturnAct = $("#ReturnAct").trimVal();

                var Atitle = $("#atitle").trimVal();
                var Usetitle = $("#usetitle").trimVal();
                var Remark = $("#remark").trimVal();
                var Useremark = $("#useremark").trimVal();

                var UseOnce = $('input:radio[name="UseOnce"]:checked').val();
                var RepeatIssue = $('input:radio[name="RepeatIssue"]:checked').val();
                var FaceObjects = $('input:radio[name="FaceObjects"]:checked').val();
                var Runstate = $('input:radio[name="Runstate"]:checked').val();
                var UseChannel = "";
                $('input[name="UseChannel"]:checked').each(function () {
                    UseChannel = UseChannel + $(this).val() + ',';
                });



                if (title == '') {
                    $.prompt('促销活动名称不可为空');
                    return;
                }
                if (Acttype == '1') {
                    if (Money == '') {
                        $.prompt('现金抵扣金额不可为空');
                        return;
                    }
                    if (!$("#Money").Amount()) {
                        $.prompt('现金抵扣金额格式不对');
                        return;
                    }
                }
                if (Acttype == '2') {
                    if (Discount == '') {
                        $.prompt('折扣不可为空');
                        return;
                    }
                    if (!$("#Discount").Amount()) {
                        $.prompt('折扣格式不对');
                        return;
                    }
                }
                if (Acttype == '3') {
                    if (CashFull == '' || Cashback == '') {
                        $.prompt('消费满金额返金额不可为空');
                        return;
                    }
                    if (!$("#CashFull").Amount()) {
                        $.prompt('费满金额返金额格式不对');
                        return;
                    }
                    if (!$("#Cashback").Amount()) {
                        $.prompt('费满金额返金额格式不对');
                        return;
                    }
                }

                if (Actstar == '') {
                    $.prompt('活动开始日期不可为空');
                    return;
                }
                if (Actend == '') {
                    $.prompt('活动截止日期不可为空');
                    return;
                }

                if (Remark == '') {
                    $.prompt('活动备注不可为空');
                    return;
                }
                if (Usetitle == '') {
                    $.prompt('活动简要说明不可为空');
                    return;
                }
                if (Useremark == '') {
                    $.prompt('活动使用备注不可为空');
                    return;
                }

                if (UseChannel == '') {
                    // $.prompt('使用门店不可为空');
                    // return;
                }

                $.post("/JsonFactory/PromotionHandler.ashx?oper=editActinfo", { createuserid: $("#hid_userid").val(), title: title, Acttype: Acttype, Money: Money, Discount: Discount
                , CashFull: CashFull, Cashback: Cashback, Actstar: Actstar, Actend: Actend, ReturnAct: ReturnAct, UseOnce: UseOnce, RepeatIssue: RepeatIssue, FaceObjects: FaceObjects, comid: comid, actid: hid_actid, Runstate: Runstate, Atitle: Atitle, Usetitle: Usetitle, Remark: Remark, Useremark: Useremark, UseChannel: UseChannel
                }, function (data) {
                    data = eval('(' + data + ')');
                    if (data.type == '100') {
                        location.href = "PromotionList.aspx";
                        return;
                    } else {
                        $.prompt("促销活动出错");
                        return;
                    }
                });
            })
        });
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <%--<div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="PromotionList.aspx" onfocus="this.blur()"><span>优惠券管理</span></a></li>
                <li class="on"><a href="PromotionManage.aspx" onfocus="this.blur()"><span>添加优惠券</span></a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                </h3>
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        优惠券内容<span>
                            <%if (acttype == 1)
                              { %>
                            (消费抵扣券)
                            <%}
                              else if (acttype == 2)
                              { %>
                            (消费折扣券)
                            <%}
                              else if (acttype == 3)
                              { %>
                            (消费满就送)
                            <%}
                              else if (acttype == 4)
                              { %>
                            (积分券)
                            <%} %>
                        </span>
                    </h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            优惠券名称</label>
                        <input data-explain="" data-widget-cid="widget-13" id="title" value="" data-default="活动名称"
                            class="mi-input" placeholder="最多9个汉字" maxlength="18" type="text">
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            优惠券副标题说明</label>
                        <input data-explain="" data-widget-cid="widget-13" id="atitle" value="" data-default="副标题说明"
                            class="mi-input" placeholder="最多50个汉字" maxlength="18" type="text">
                    </div>
                    <div class="mi-form-item" style="display: none">
                        <label class="mi-label">
                            选择优惠券类型</label>
                        <select name="Acttype" id="Acttype" class="mi-input">
                            <option value="1">消费抵扣券</option>
                            <option value="2">消费折扣券</option>
                            <option value="3">消费满就送</option>
                            <option value="4">积分券(金额打入积分中，不限使用)</option>
                        </select>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            优惠：</label>
                        <div id="Acttype1">
                            金额
                            <input name="Money" type="text" id="Money" value="" size="10" class="mi-input" style="width: 60px;">
                        </div>
                        <div id="Acttype2">
                            折扣
                            <input name="Discount" type="text" id="Discount" value="" size="10" class="mi-input"
                                style="width: 60px;">
                            %；
                        </div>
                        <div id="Acttype3">
                            消费满
                            <input name="CashFull" type="text" id="CashFull" value="" size="10" class="mi-input"
                                style="width: 60px;">
                            元返
                            <input name="Cashback" type="text" id="Cashback" value="" size="10" class="mi-input"
                                style="width: 60px;">
                            元。
                        </div>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            优惠券有效期</label>
                        <div class="mi-form-text">
                            <input class="mi-input" value="" id="Actstar" type="text" isdate="yes" />
                            <span class="mi-form-spliter">-</span>
                            <input class="mi-input" value="" id="Actend" type="text" isdate="yes" />
                            <div class="mi-form-explain">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        优惠券介绍</h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            优惠券介绍说明</label>
                        <textarea name="remark" cols="50" rows="6" id="remark" class="mi-input" style="width: 500px;"></textarea>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            使用地点/</label>
                        <input name="usetitle" type="text" id="usetitle" size="25" maxlength="50" class="mi-input"
                            style="width: 500px;" />
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            优惠券备注</label>
                        <textarea name="useremark" cols="50" rows="6" id="useremark" class="mi-input" style="width: 500px;"></textarea>
                        <div class="mi-form-explain">
                        </div>
                    </div>
                </div>
                <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        优惠券使用对象</h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            对象范围</label>
                        <label>
                            <input name="FaceObjects" id="FaceObjects1" type="radio" value="1" checked="checked" />
                            所有会员（需会员点击领取）</label><br />
                        <label>
                            <input name="FaceObjects" id="FaceObjects2" type="radio" value="2" />
                            特定会员（通过发行设定特定会员享受此优惠）</label><br />
                        <label>
                            <input name="FaceObjects" id="FaceObjects3" type="radio" value="3" />
                            网站注册（注册自动赠送）</label><br />
                        <label>
                            <input name="FaceObjects" id="FaceObjects4" type="radio" value="4" />
                        微信注册（微信关注自动赠送）
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            优惠券使用门店</label>
                        <div>
                            <label>
                                <input name="UseChannelAll" id="UseChannelAll" type="checkbox" value="1">
                                全选</label><br>
                            <br>
                        </div>
                        <div id="UseChannel_model">
                        </div>
                        <div class="mi-form-explain">
                        </div>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            运行状态</label>

                            <label>
                                <input name="Runstate" id="Runstate" type="radio" value="true" checked />
                                运行中</label>
                            <label>
                                <input type="radio" id="Runstate" name="Runstate" value="false">
                                停止运行</label>
                            <div class="mi-form-explain">
                            </div>
                    </div>
                </div>
                <table width="780" border="0">
                    <tr>
                        <td width="699" height="44" align="center">
                            <input name="UseOnce" type="hidden" value="true" checked />
                            <input name="RepeatIssue" type="hidden" value="1" checked>
                            <input name="ReturnAct" id="ReturnAct" type="hidden" value="0" />
                            <input type="button" name="GoActAddNext" id="GoActAddNext" class="mi-input" value="      确      认     " />
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
    <input type="hidden" id="hid_actid" value="<%=actid %>" />
    <input type="hidden" id="hid_acttype" value="<%=acttype %>" />
    <input type="hidden" id="hid_faceObjects" value="<%=faceObjects %>" />
    <script type="text/x-jquery-tmpl" id="UseChannelList">
        <option value='${Id}'>${Companyname}</option>          
    </script>
    <script>



        function abc() {

            alert($("#UseChannel").get(0).selectedIndex = 2)

            $("#UseChannel option[text='0']").attr("selected", "selected");
            $("#UseChannel").find("option[text='0']").attr("selected", true);

            $("#UseChannel").find("option[text='0']").each(function () {
                $(this).attr("selected", true);
            })

        }

//        $(function () {

//        //对已选择的进行标注
//        var hid_actid = $("#hid_actid").trimVal();
//        if (hid_actid != '0') {
//            $.post("/JsonFactory/ChannelHandler.ashx?oper=GetUnitListselected", { actid: hid_actid }, function (data) {
//                data = eval("(" + data + ")");
//                if (data.type == 100) {
//                    for (var i = 0; i < data.msg.length; i++) {
//                        $('input[name="UseChannel"]').each(function () {
//                            alert("测试：" + data.msg[i].Id);
//                            if ($(this).val() == data.msg[i].Id) {
//                                alert(data.msg[i].Id);
//                                $(this).attr("checked", 'true');
//                            }
//                        });
//                    }
//                }
//            })

//        }
//        })


    </script>
</asp:Content>
