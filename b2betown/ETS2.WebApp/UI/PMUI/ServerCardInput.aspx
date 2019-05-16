<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ServerCardInput.aspx.cs"
    Inherits="ETS2.WebApp.UI.PMUI.ServerCardInput" MasterPageFile="/UI/Etown.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .mail-list tbody td
        {
            height: 40px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $("#cardchipid").focus();

            $("html").die().live("keydown", function (event) {
                if (event.keyCode == 13) {
                    //                    $("#btnluru").click();    //这里添加要处理的逻辑  
                    if ($("#cardchipid").trimVal().length > 10) {
                        alert("请确认卡片芯片ID是否重复录入了!!");
                        $("#cardchipid").focus();
                        return;
                    }
                    $("#btnluru").click();
                    $("#cardchipid").focus();
                    return false;
                }
            });

            $("#btnluru").click(function () {
                var cardchipid = $("#cardchipid").trimVal();
                if (cardchipid == "") {
                    alert("卡芯片ID还没有读取");
                    return;
                }
                var cardprintid = $("#hid_cardprintid").trimVal();
                if (cardprintid == "") {
                    alert("卡面印刷编号最小值还没有设置!");
                    return;
                }
                if (isNaN(cardprintid)) {
                    alert("卡面印刷编号格式不正确!");
                    return;
                }

                $.post("/JsonFactory/ProductHandler.ashx?oper=Relationserver_printid_chipid", { comid: $("#hid_comid").trimVal(), cardchipid: cardchipid, cardprintid: cardprintid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        alert(data.msg);
                        $("#cardchipid").focus();
                        return;
                    }
                    if (data.type == 100) {
                        var nextprintid = parseInt($("#hid_cardprintid").val()) + 1;

                        var prefillzero = "";
                        if (nextprintid.toString().length < 5) {
                            //如果输入的最小编号小于5位，用0补齐
                            for (var i = 0; i < 5 - nextprintid.toString().length; i++) {
                                prefillzero += "0";
                            }
                        }
                        $("#lblprintid").text("NO." + prefillzero + nextprintid.toString());
                        $("#hid_cardprintid").val(nextprintid);
                        //                        alert(data.msg);
                        $("#cardchipid").val("").focus();
                        return;
                    }

                })
            })

            $("[name='editcardprintid']").click(function () {
                $("#span_rh").text("卡面印刷编号设置");
                $("#mincardprintid").val("");
                $("#rhshow").show();
            })
            $("#cancel_rh").click(function () {
                $("#rhshow").hide();
            })
            $("#submit_rh").click(function () {
                var mincardprintid = $("#mincardprintid").trimVal();
                if (mincardprintid == "") {
                    alert("卡面印刷编号最小值还没有设置!");
                    return;
                }
                if (isNaN(mincardprintid)) {
                    alert("请录入数字");
                    return;
                }

                var prefillzero = "";
                if (mincardprintid.length < 5) {
                    //如果输入的最小编号小于5位，用0补齐
                    for (var i = 0; i < 5 - mincardprintid.length; i++) {
                        prefillzero += "0";
                    }
                }
                $("#lblprintid").text("NO." + prefillzero + mincardprintid);
                $("#hid_cardprintid").val(parseInt(mincardprintid));
                $("#rhshow").hide();
            })
        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
       <%-- <div id="secondary-tabs" class="navsetting">
            <ul>
                <li><a href="javascript:void(0)" onfocus="this.blur()" target="" onclick="dbsavevurl('/ui/pmui/productservertypelist.aspx')">
                    <span>添加产品</span></a></li>
                <li><a href="javascript:void(0)" onfocus="this.blur()" target="" onclick="dbsavevurl('/ui/pmui/projectlist.aspx')">
                    <span>项目管理</span></a></li>
                <li><a href="javascript:void(0)" onfocus="this.blur()" target="" onclick="dbsavevurl('/ui/pmui/projectedit.aspx')">
                    <span>添加项目</span></a></li>
                <li><a href="javascript:void(0)" onfocus="this.blur()" target="" onclick="dbsavevurl('/ui/pmui/productlist.aspx')">
                    <span>产品列表</span></a></li>
                <li><a href="javascript:void(0)" onfocus="this.blur()" target="" onclick="dbsavevurl('/ui/pmui/order/salecount.aspx')">
                    <span>产品统计</span></a></li>
                <li><a href="javascript:void(0)" onfocus="this.blur()" target="" onclick="dbsavevurl('/ui/pmui/bindingagentlist.aspx')">
                    <span>导入分销系统产品</span></a></li>
                <li><a href="javascript:void(0)" onfocus="this.blur()" target="" onclick="dbsavevurl('/ui/pmui/eticket_useset.aspx')">
                    <span>商户特定日期设定</span></a></li>
                <li><a href="javascript:void(0)" onfocus="this.blur()" target="" onclick="dbsavevurl('/ui/pmui/delivery/deliverylist.aspx')">
                    <span>运费模板管理</span></a></li>
                <li class="on"><a href="javascript:void(0)" onfocus="this.blur()" target="" onclick="dbsavevurl('/ui/pmui/serverlist.aspx')">
                    <span>终端服务管理</span></a></li>
                <li><a href="javascript:void(0)" onfocus="this.blur()" target="" onclick="dbsavevurl('/ui/pmui/cablewayeticket_useset.aspx')">
                    <span>下班时间管理</span></a></li>
            </ul>
        </div>--%>
            <div class="navsetting ">
            <ul class="composetab">
                <%--<li style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/serverlist.aspx">终端服务管理</a></div>
                    </div>
                </li>
                <li style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/ServerPrintSuodao.aspx">索道票打印统计</a></div>
                    </div>
                </li>
                <li  style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/ServerFakaStat.aspx">发卡统计</a></div>
                    </div>
                </li>
                <li  style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/ServerTimeoutStat.aspx">归还超时统计</a></div>
                    </div>
                </li>--%>
                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_sel">
                        <div>
                            <a href="/ui/pmui/servercardinputlist.aspx">服务卡管理</a></div>
                    </div>
                </li>
                <li class="on" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/servercardinput.aspx">录入服务卡</a></div>
                    </div>
                </li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3 style="font-size: 20px;">
                    服务卡片录入</h3>
                <table style="border: none;">
                    <tr>
                        <td style="font-size: 18px;">
                            卡片芯片ID
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type="text" id="cardchipid" value="" style="line-height: 25px; width: 320px;"
                                autocomplete="off" />
                        </td>
                    </tr>
                    <tr>
                        <td style="font-size: 18px;">
                            卡面印刷编号(请按编号<lable style="color: rgb(60, 175, 220);">升序</lable>录入，并且认真核对和卡片芯片是否相符，如不符，请点击
                            <a href="javascript:void(0)" style="color: rgb(60, 175, 220); text-decoration: underline;"
                                name="editcardprintid">这里</a> 设置编号最小值 )
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label style="font-size: 16px; font-weight: bold; color: Red;" id="lblprintid">
                                <a href="javascript:void(0)" style="color: rgb(60, 175, 220); text-decoration: underline;"
                                    name="editcardprintid">编号最小值未设置</a></label>
                            <input type="hidden" id="hid_cardprintid" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a href="javascript:void(0)" style="color: #FFF; border-color: rgb(28, 189, 241);
                                background-color: rgb(60, 175, 220); font-size: 14px; margin-top: 10px; padding: 27px 80px;
                                height: 18px; width: 120px;" id="btnluru">录 入</a>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div id="rhshow" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px auto;
        width: 400px; height: 130px; display: none; left: 20%; position: absolute; top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 18px;" id="span_rh"></span>
                </td>
            </tr>
            <tr>
                <td height="30" align="right" bgcolor="#E7F0FA" class="tdHead">
                    编号最小值:
                </td>
                <td height="30" bgcolor="#E7F0FA" class="tdHead" id="tdgroups">
                    <input type="text" id="mincardprintid" value="" placeholder="形如:00001" />
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input name="submit_rh" id="submit_rh" type="button" class="formButton" value="  确  定  " />
                    <input name="cancel_rh" id="cancel_rh" type="button" class="formButton" value="  取  消  " />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
