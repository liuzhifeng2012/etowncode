<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ETicketIndex.aspx.cs"
    Inherits="ETS2.WebApp.UI.PMUI.ETicket.CheckCode" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="/Scripts/print.js" type="text/javascript"></script>
    <script language="javascript" src="/Scripts/LodopFuncs.js"></script>
    <object id="LODOP_OB" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0"
        height="0">
        <param name="License" value="394101459411010811811255105117">
        <param name="LicenseA" value="741444553515055585560596856128">
        <embed id="LODOP_EM" type="application/x-print-lodop" width="0" height="0" pluginspage="install_lodop32.exe"></embed>
    </object>
    <script type="text/javascript">
        $(function () {
            $("#errmsg").hide();

            var comid = $("#hid_comid").trimVal();
            var hidpno = $("#hid_pno").trimVal();


            if (hidpno == "") {
                $("#tr1").show().siblings().hide();
                $("#alast").hide().siblings("#viewpro").hide();
            }
            else {

            }

            $("#selbtn").click(function () {
                $("#errmsg").hide();
                $("#alast").hide().siblings("#viewpro").hide();

                //清空电子票使用张数
                $("#usenum").find("option").remove();

                var pno = $("#pno").trimVal();
                if (pno == "") {
                    //                    $.prompt("电子码不可为空");
                    $("#errmsg").show().find("strong").text("电子码不可为空");
                    return;
                }
                else {
                    //判断电子码是否存在
                    $.post("/JsonFactory/EticketHandler.ashx?oper=ValidateEticket", { pno: pno, comid: comid }, function (data) {
                        data = eval("(" + data + ")");
                        if (data.type == 1) {
                            //                            $.prompt(data.msg);
                            $("#errmsg").show().find("strong").text(data.msg);
                            return;
                        }
                        if (data.type == 100) {
                            //给电子码使用数量赋值
                            var use_pnum = data.msg.Use_pnum;
                            for (var i = use_pnum; i >= 1; i--) {

                                $("#usenum").append("<option value='" + i + "'>" + i + "</option>")
                            }
                            $("#pno2").val(pno);
                            $("#ticketname").text(data.msg.E_proname);

                            //隐藏行
                            $("#tr2").show().siblings().hide();
                            //电子票号清空
                            $("#pno").val("");

                        }
                    })
                }
            })
            //确认使用
            $("#confirmbtn").click(function () {
                $("#errmsg").hide();
                $("#tblist").html("");

                var usenum = $("#usenum").val();


                $.post("/JsonFactory/EticketHandler.ashx?oper=econfirm", { pno: $("#pno2").trimVal(), usenum: usenum, comid: comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        //                        $.prompt(data.msg);
                        //                        $("#errmsg").show().find("strong").text(data.msg);
                        $.prompt(data.msg, { buttons: [{ title: "确定", value: true}], submit: function (e, v, m, f) { if (v == true) { window.location.reload() } } })
                        return;
                    }
                    if (data.type == 100) {


                        //确认使用以后显示电子票使用情况
                        $("#tr1").show().siblings().hide();
                        $("#alast").show().siblings("#viewpro").show();


                        //电子票使用详细信息
                        $.post("/JsonFactory/EticketHandler.ashx?oper=getedetail", { validateticketlogid: data.msg, pno: $("#pno2").trimVal(), comid: comid }, function (data) {
                            data = eval("(" + data + ")");
                            if (data.type == 1) {
                                //                        $.prompt(data.msg);
                                $("#errmsg").show().find("strong").text(data.msg);
                                //电子票号清空
                                $("#pno2").val("");
                                return;
                            }
                            if (data.type == 100) {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                //电子票号清空
                                $("#pno2").val("");
                                //打印电子凭证验证单，
                                var LODOP; //声明为全局变量   
                                prn_Print('电子凭证验证单', '<%=comname%>', data.msg[0].ProductName, data.msg[0].OnePrice, data.msg[0].Company, '', data.msg[0].Pno, '<%=DateTime.Now %>', data.msg[0].ThisUseNum, '<%=username %>', '<%=printname %>');

                            }
                        })
                    }
                })
            })



        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="/ui/pmui/eticket/eticketindex.aspx" onfocus="this.blur()"
                    target=""><span>电子凭证验证</span></a></li>
                <li><a href="/ui/pmui/eticket/eticketlist.aspx" onfocus="this.blur()" target=""><span>
                    验证明细</span></a></li>
                 <li><a href="/ui/pmui/eticket/InterfaceUseLog.aspx" onfocus="this.blur()"
                    target=""><span>Wl接口验证明细</span></a></li>
                   <li class=""><a href="/ui/pmui/eticket/Eticket_safety.aspx" onfocus="this.blur()"
                    target=""><span>安全码查询</span></a></li>
                <li><a href="/ui/pmui/eticket/ReserveproVerify.aspx" onfocus="this.blur()" target="">
                    <span>预订产品验证</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <h3>
                    电脑验码</h3>
                <table class="grid">
                    <tr id="tr1">
                        <td>
                            <label>
                                请输入电子码<font class="inputstyle">：</font>
                                <input name="pno" type="text" id="pno" maxlength="120" class="inputstyle" style="font-size: 20px;" />
                                &nbsp;
                            </label>
                            <label>
                                &nbsp;
                                <input type="button" id="selbtn" value="确认查询" style="width: 160px; height: 30px;" />
                            </label>
                            <a id="alast" href="RePrintEticket.aspx" target="">重打最后一张核销小票</a>
                            <table id="viewpro" width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tbody id="tblist">
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr id="tr2">
                        <td>
                            <div>
                                <span style="padding-top: 20px;">
                                    <label>
                                        <font>电子码： </font>
                                        <input name="pno" type="text" id="pno2" value="" maxlength="120" style="background-color: #EFEFEF;
                                            font-size: 20px;" class="inputstyle" readonly />
                                    </label>
                                    <label>
                                        &nbsp; 使用数量：
                                        <select name="usenum" id="usenum" class="inputstyle">
                                        </select>
                                        张
                                    </label>
                                    &nbsp;&nbsp;&nbsp;
                                    <label>
                                        <input type="button" id="confirmbtn" value="  确认使用  " style="width: 160px; height: 25px;" />
                                    </label>
                                </span>
                            </div>
                            <div id="ticketname" style="font-size: 16px; color: Green">
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <table width="100%" height="100px" border="0" cellspacing="0" cellpadding="0">
        <tr bgcolor="#F30" id="errmsg" style="display: none;">
            <td height="31" colspan="4" bgcolor="#F30">
                <strong></strong>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hid_pno" value="<%=pno %>" />
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                      <tr>
                                    <td height="48" colspan="4" align="center">
                                        <h2>
                                            <strong>${ProductName}</strong>电子票确认成功！</h2>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="22%" height="31" align="right" bgcolor="#00CC33">
                                        服务内容：
                                    </td>
                                    <td height="31" colspan="3" bgcolor="#00CC33" class="STYLE4">
                                        <strong>${ProductName}</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="27" align="right" bgcolor="#00CC33">
                                        使用数量：
                                    </td>
                                    <td width="24%" height="27" bgcolor="#00CC33">
                                        <span class="STYLE9">${ThisUseNum}</span> 张
                                    </td>
                                    <td width="46%" colspan="2" rowspan="3" bgcolor="#00CC33" class="STYLE8">
                                        电子票号：${Pno}
                                    </td>
                                </tr>
                                <tr>
                                    <td height="30" align="right" bgcolor="#00CC33">
                                        服务单价：
                                    </td>
                                    <td height="30" bgcolor="#00CC33">
                                        ${fmoney(OnePrice,2)} 元
                                    </td>
                                </tr>
                                <tr>
                                    <td height="30" align="right" bgcolor="#00CC33">
                                        有 效 期：
                                    </td>
                                    <td height="30" bgcolor="#00CC33">
                                        ${ChangeDateFormat(Pro_Start)}--${ChangeDateFormat(Pro_end)}
                                    </td>
                                </tr>
    </script>
</asp:Content>
