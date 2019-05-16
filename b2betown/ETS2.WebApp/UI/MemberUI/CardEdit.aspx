<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="CardEdit.aspx.cs"
    Inherits="ETS2.WebApp.UI.MemberUI.CardEdit" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
<link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {

            //首先加载数据
            var comid = $("#hid_comid").trimVal();
            var hid_cardid = $("#hid_cardid").trimVal();
            if (hid_cardid != 0) {
                $.post("/JsonFactory/CardHandler.ashx?oper=getCardByIdNew", {comid:comid, cardid: hid_cardid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取数据出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#Cname").val(data.msg.Cname);
                        $("#Ctype").val(data.msg.Ctype);
                        $("#Printnum").val(data.msg.Printnum);
                        $("#Exchange").val(data.msg.Exchange);
                        $("#Remark").val(data.msg.Remark);
                        $("#CardRule_starnum").val(data.msg.CardRule_starnum);
                        $("#CardRule_First").val(data.msg.CardRule_First);
                        $("#CardRule_First").attr("disabled", true);

                        $("input:radio[name='CardRule'][value=" + data.msg.CardRule + "]").attr("checked", true);
                        $("input:radio[name='Zhuanzeng'][value=" + data.msg.Zhuanzeng + "]").attr("checked", true);
                        $("input:radio[name='Qrcode'][value=" + data.msg.Qrcode + "]").attr("checked", true);

                    }
                })
            }
            //加载前4位号
            if (hid_cardid == 0) {
                var comid = $("#hid_comid").trimVal();
                $.post("/JsonFactory/CardHandler.ashx?oper=getCardFirst", { comid: comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("获取数据出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#CardRule_First").val(data.msg.CardRule_First);
                        //$("#CardRule_First").attr("disabled", true);
                    }
                })
            }


            //第二步按钮
            $("#GoSub").click(function () {
                var cardid = $("#hid_cardid").trimVal();
                var comid = $("#hid_comid").trimVal();
                var Cname = $("#Cname").trimVal();
                var Ctype = $("#Ctype").val();
                var Printnum = $("#Printnum").val();
                var Exchange = $("#Exchange").trimVal();
                var Remark = $("#Remark").trimVal();
                var CardRule_starnum = $("#CardRule_starnum").trimVal();
                var CardRule_First = $("#CardRule_First").trimVal();
                var CardRule_Second = $("#CardRule_Second").trimVal();


                var CardRule = $('input:radio[name="CardRule"]:checked').val();
                var Zhuanzeng = $('#Zhuanzeng').val();
                var Qrcode = $('#Qrcode').val();



                if (Cname == '') {
                    $.prompt('卡名称不可为空');
                    return;
                }

                if (Ctype == '') {
                    $.prompt('卡类型不可为空');
                    return;
                }
                if (Printnum == '') {
                    $.prompt('发卡量不可为空');
                    return;
                }
                if (!$("#Printnum").Amount()) {
                    $.prompt('发卡量格式不对');
                    return;
                }

                if (CardRule == null) {
                    $.prompt('卡号设置不可为空');
                    return;
                }
                if (CardRule_First == '') {
                    $.prompt('卡首4位不可为空');
                    return;
                }
                if (CardRule_Second == '') {
                    $.prompt('卡首5-8位不可为空');
                    return;
                }

                if (CardRule == '2') {
                    if (CardRule_starnum == '') {
                        $.prompt('顺序增长请设定起始卡号');
                        return;
                    }
                    if (!$("#CardRule_starnum").Amount()) {
                        $.prompt('起始卡号格式不对');
                        return;
                    }

                    if ($("#CardRule_starnum").val().length < 8) {
                        $.prompt('请填写 8 位起始卡号');
                        return;
                    }


                }


                if (Zhuanzeng == null) {
                    //$.prompt('请选择是否可转送');
                    //return;
                }

                $("#GoSub").val("正在提交中，请稍后不要重复点击！")
                $("#GoSub").attr("disabled", true);

                $.post("/JsonFactory/CardHandler.ashx?oper=editCardinfo", { Cname: Cname, Ctype: Ctype, Printnum: Printnum, Exchange: Exchange
                , CardRule: CardRule, Zhuanzeng: 0, Qrcode: 1, Remark: Remark, CardRule_starnum: CardRule_starnum, CardRule_First: CardRule_First, CardRule_Second: CardRule_Second, comid: comid, cardid: cardid
                }, function (data) {
                    data = eval('(' + data + ')');
                    if (data.type == '100') {
                        location.href = "CardList.aspx";
                        return;
                    } else {
                        $.prompt("添加新卡出错出错");
                        return;
                    }
                });
            })
        });
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="CardList.aspx" onFocus="this.blur()"><span>卡片管理</span></a></li>
                <li class="on"><a href="CardEdit.aspx" onFocus="this.blur()" ><span>
                    添加卡片</span></a></li>
                    <li><a href="MemberCardList.aspx" onfocus="this.blur()"><span>已录入卡号管理</span></a></li>
                    <li><a href="PublishList.aspx" onfocus="this.blur()"><span>发行管理</span></a></li>
            </ul>
        </div>

        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    参考卡样： 现金卡 电子会员卡 储值卡</h3>
               <!-- <ul class="card_list">
                    <li><a href="#"><strong>电子会员卡</strong><span></span>
                        <p style="background-position: -260px 0;">
                        </p>
                    </a></li>
                    <li><a href="#"><strong>储值礼品卡</strong><span></span>
                        <p style="background-position: -260px -150px;">
                        </p>
                    </a></li>
                    <li><a href="#"><strong>实体会员卡</strong><span></span>
                        <p style="background-position: -260px -300px;">
                        </p>
                    </a></li>
                </ul>
                <p>&nbsp;
                    </p>-->
               <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
                   <h2 class="p-title-area">填写卡基本信息</h2>
                   <div class="mi-form-item">
                        <label class="mi-label"> 卡名称</label>
                       <input name="Cname" type="text" id="Cname"  size="25" class="mi-input"  style="width:200px;"/>
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 类型</label>
                       <select name="Ctype" id="Ctype" class="mi-input" onclick="return select2_onclick()">
                                <option value="1" selected>实体卡</option>
                          </select>
                   </div>
                   <div class="mi-form-explain"></div>

               </div>

               <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px; position: relative; z-index: 10;">
                   <h2 class="p-title-area">发卡设置</h2>
                   <div class="mi-form-item">
                        <label class="mi-label"> 总发卡量</label>
                       <input name="Printnum" type="text" id="Printnum"  size="25" class="mi-input"  style="width:100px;"/>
                   </div>
                   <div class="mi-form-item">
                        <label class="mi-label"> 卡号设置</label>
                        <table border="0">
                                <tr>
                                  <td>卡号规则:
                                    <input name="CardRule_First"  class="mi-input"  style="width:50px;" type="text" id="CardRule_First" size="6" value="<%= CardRule_First%>" />
                                    <input name="CardRule_Second"  class="mi-input"  style="width:50px;" type="text" id="CardRule_Second" size="6" value="<%=CardRule_Second%>" /> **** ****     </td>
                                  <td><label>
                                    <input type="radio" name="CardRule" value="1">
随机编码 </label>
                                    <label>(最后8位为随机吗)<br>
                                    <input type="radio" name="CardRule" value="2">
顺序增长</label>
最后8位起始号
<input name="CardRule_starnum" type="text" id="CardRule_starnum" size="10" maxlength="8"  class="mi-input"  style="width:70px;" /></td>
                                </tr>
                              </table>
                   </div>

                    <div class="mi-form-item">
                        <label class="mi-label"> 具体使用说明</label>
                       <textarea name="Remark" cols="50" id="Remark"  class="mi-input"  style="width:400px;"></textarea>
                   </div>

                   <div class="mi-form-explain"></div>

               </div>

                <table width="780" border="0">
                    <tr>
                        <td width="699" height="44" align="center">
                        <input type="hidden" name="Zhuanzeng" value="0">
                                    <input type="hidden" name="Qrcode" value="1">
                                    <input name="Exchange" type="hidden" class="icon" id="textfield14" value="ALL" size="25" />
                             <input type="button" name="GoSub" id="GoSub" class="mi-input"  value=" 完成基本信息填写，确认  " />
                        </td>
                    </tr>
                </table>
            </div>
        </div>

    </div>
    <div class="data">
    </div>
    <input type="hidden" id="hid_cardid" value="<%=cardid %>" />
</asp:Content>
