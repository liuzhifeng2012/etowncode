<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Manage.Master" CodeBehind="PrintTicket.aspx.cs" Inherits="ETS2.WebApp.Agent.PrintTicket" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/Print.js" type="text/javascript"></script>

<script language="javascript" src="/Scripts/LodopFuncs.js"></script>
<object id="LODOP_OB" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width=0 height=0> 
  
	<embed id="LODOP_EM" type="application/x-print-lodop" width=0 height=0 pluginspage="install_lodop32.exe"></embed>
</object> 

    <script type="text/javascript">

        $(function () {
            var agentid = $("#hid_agentid").trimVal();
            var id = $("#hid_id").trimVal();
            var comid = $("#hid_comid_temp").trimVal();

            $("#confirmButton").click(function () {
                var id = $("#hid_id").trimVal();

                var print_num = $("#u_num").trimVal();
                var printn = $("#printn").trimVal();
                if (printn == "") {
                    $.prompt("请填写打印的数量");
                    return;
                }
                var patrn = /^([1-9]\d*|1)(\.\d*[1-9])?$/;
                if (!patrn.exec(printn)) {
                    $.prompt("请填写打印的数量");
                    return;
                }
                if (parseInt(printn) > parseInt(print_num) || parseInt(printn) == 0) {
                    $.prompt("打印数量不能大于可打印数量");
                    return;
                }

                if (confirm("请确认打票机已连接,票纸已放入，确认后立即开始打印！")) {
                    $('#confirmButton').hide().after('<span id="spLoginLoading" style="margin-left:10px;color:#2D65AA;font-size:16px;">打印中……<span id="pnum"></span></span>');

                    $.ajax({
                        type: "post",
                        url: "/JsonFactory/AgentHandler.ashx?oper=printeticket",
                        data: { pagesize: printn, id: id, comid: comid, agentid: agentid },
                        async: false,
                        success: function (data) {
                            data = eval("(" + data + ")");

                            if (data.type == 1) {
                                $.prompt(data.msg);
                                return;
                            }
                            if (data.type == 100) {
                                $("#old_print_num").val(parseInt($("#old_print_num").val()) + parseInt(printn));
                                $("#print_num").val(parseInt($("#print_num").val()) - parseInt(printn));


                                for (var i = 0; i < printn; i++) {
                                    $("#pnum").html(" 第" + (i + 1) + " 已提交。");
                                    var LODOP; //声明为全局变量   <%=pro_start%> - <%=pro_end %>
                                    //CreatePrintPageTicket("<%=proname%>", data.msg[i].Pagecode, "<%= Face_price %>元", "2015-2016雪季", "<%=mobile %>", "<%=address %>", "<%=Pro_Remark %>", data.msg[i].PnoMd5, "http://y.etown.cn", "<%=agentcompany %>", data.msg[i].Pno, i);

                                    //打印预约码
                                    //CreatePrintPageTicket_yuyuema("<%=proname%>", data.msg[i].Pagecode, "<%= Face_price %>元", "2015-2016雪季", "010-59059150", "<%=address %>", "<%=Pro_Remark %>", data.msg[i].PnoMd5, data.msg[i].Pno, "<%=agentcompany %>", data.msg[i].Pno, i);

                                    //打印电子票

                                    if (i == 0) {
                                        //CreatePrintPageTicket_dianzipiao("测试打印电子票", "1234567891230", "99元", "2015-01-01", "400123456789", "地址：测试地址", "服务包含门票一张，不含其他的自费项目等", "12345678912345678912345678912456789", "getDate()", "测试单位", "923456789123", 0);
                                    }

                                    CreatePrintPageTicket_dianzipiao("<%=proname%>", data.msg[i].Pagecode, "<%= Face_price %>元", "2015-2016雪季", "<%=mobile %>", "<%=address %>", "<%=Pro_Remark %>", data.msg[i].PnoMd5, data.msg[i].Pno, "<%=agentcompany %>", data.msg[i].Pno, i);
                                }
                            }
                        }
                    })
                }

            })



            $("#confirmButtonyuyuma").click(function () {
                var id = $("#hid_id").trimVal();

                var print_num = $("#u_num").trimVal();
                var printn = $("#printn").trimVal();
                if (printn == "") {
                    $.prompt("请填写打印的数量");
                    return;
                }
                var patrn = /^([1-9]\d*|1)(\.\d*[1-9])?$/;
                if (!patrn.exec(printn)) {
                    $.prompt("请填写打印的数量");
                    return;
                }
                if (parseInt(printn) > parseInt(print_num) || parseInt(printn) == 0) {
                    $.prompt("打印数量不能大于可打印数量");
                    return;
                }

                if (confirm("请确认打票机已连接,票纸已放入，确认后立即开始打印！")) {
                    $('#confirmButton').hide().after('<span id="spLoginLoading" style="margin-left:10px;color:#2D65AA;font-size:16px;">打印中……<span id="pnum"></span></span>');

                    $.ajax({
                        type: "post",
                        url: "/JsonFactory/AgentHandler.ashx?oper=printeticket",
                        data: { pagesize: printn, id: id, comid: comid, agentid: agentid },
                        async: false,
                        success: function (data) {
                            data = eval("(" + data + ")");

                            if (data.type == 1) {
                                $.prompt(data.msg);
                                return;
                            }
                            if (data.type == 100) {
                                $("#old_print_num").val(parseInt($("#old_print_num").val()) + parseInt(printn));
                                $("#print_num").val(parseInt($("#print_num").val()) - parseInt(printn));


                                for (var i = 0; i < printn; i++) {
                                    $("#pnum").html(" 第" + (i + 1) + " 已提交。");
                                    var LODOP; //声明为全局变量   <%=pro_start%> - <%=pro_end %>
                                    //CreatePrintPageTicket("<%=proname%>", data.msg[i].Pagecode, "<%= Face_price %>元", "2015-2016雪季", "<%=mobile %>", "<%=address %>", "<%=Pro_Remark %>", data.msg[i].PnoMd5, "http://y.etown.cn", "<%=agentcompany %>", data.msg[i].Pno, i);
                                    if (i == 0) {
                                        CreatePrintPageTicket_dianzipiao("测试打印电子票", "1234567891230", "99元", "2015-01-01", "400123456789", "地址：测试地址", "服务包含门票一张，不含其他的自费项目等", "12345678912345678912345678912456789", "getDate()", "测试单位", "923456789123", 0);
                                    }
                                    //打印预约码
                                    CreatePrintPageTicket_yuyuema("<%=proname%>", data.msg[i].Pagecode, "<%= Face_price %>元", "2015-2016雪季", "0313-5698865", "<%=address %>", "<%=Pro_Remark %>", data.msg[i].PnoMd5, data.msg[i].Pno, "绿野滑雪小站", data.msg[i].Pno, i);

                                 }
                            }
                        }
                    })
                }

            })

            $("#printtest").click(function () {
                alert("测试打印");
                CreatePrintPageTicket("测试打印电子票", "1234567891230", "99元", "2015-01-01", "400123456789", "地址：测试地址", "服务包含门票一张，不含其他的自费项目等", "12345678912345678912345678912456789", "getDate()", "测试单位", "923456789123", 0);
            })


        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
                    <table>
                    <tr>
                        <td class="tdHead" style="font-size:14px; height:26px;">
                            商户名称： <%=company %>   (授权类型： <%=Warrant_type_str%>)</td>
                    </tr>
                    <tr>
                        <td class="tdHead" style="font-size:14px;height:26px;">
                           <%=yufukuan%>  (<a href="Recharge.aspx?comid=<%=comid_temp %>">在线充值</a>)   <span id="shopcart" style=" padding-left:30px;"></span>
                        </td>

                    </tr>
                </table>
        <div id="secondary-tabs" class="navsetting ">
         
                     <ul class="composetab">
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_unsel"><div><a href="Order.aspx?comid=<%=comid_temp %>">返回订单记录</a></div></div>
            </li>
             <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_sel"><div><a href="javasript:;">打印纸质票</a></div></div>
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
                    打印纸质票</h3>
                    <div style="">
                     <input id="printtest" type="button" value="    测试打印纸质票    " name="printtest"></input>
                    </div>
                <table width="700px" class="grid">
                    <tr>
                        <td class="tdHead" style=" width:160px;">
                            名     称 : 
                        </td>
                         <td>
                           <h3 class="titlepng"><%=proname%>
                        </h3>
                        </td>
                    </tr>
                     <tr>
                        <td class="tdHead">
                            预订数量 : 
                        </td>
                         <td >
                         <div class="wrap-input">
                     
                    <input id="u_num" name="u_num" type="text" class="input-ticket" autocomplete="off"
                        maxlength="10"  size="10" value="<%=ordernum %>" disabled="disabled">
                    </div>
                        </td>
                    </tr> 
                    <tr>
                        <td class="tdHead">
                            已打印数量 : 
                        </td>
                         <td >
                         <div class="wrap-input">
                         <input id="old_print_num" name="old_print_num" type="text" class="input-ticket" autocomplete="off"
                        maxlength="10"  size="10" value="<%=printnum %>" disabled="disabled" />
                    </div>
                        </td>
                    </tr> 
                    <tr>
                        <td class="tdHead">
                            剩余可以打印数量 : 
                        </td>
                         <td >
                         <div class="wrap-input">
                         <input id="print_num" name="print_num" type="text" class="input-ticket" autocomplete="off"
                        maxlength="10"  size="10" value="<%=unprintnum %>" disabled="disabled">
                    </div>
                        </td>
                    </tr> 
                     </table>
                     <table width="300px" class="grid">
                        <tr>
                        <td class="tdHead">
                         本次打印数量：<input id="printn" name="printn" type="text" class="input-ticket" autocomplete="off" maxlength="25"  size="25" value=""> 张 
                         
                        </td>
	                    </tr>
                    </table>
                <table width="300px" class="grid">
                 <tr>
                        <td class="tdHead">
                       <input id="confirmButton" type="button" value="    打印电子票    " name="confirmButton"></input>
                       <input id="confirmButtonyuyuma" type="button" value="    打印班车预约吗    " name="confirmButton"></input>
                       
                     <br>(注：票纸打印中 地址按项目地址读取，价格、有效期读取产品信息，出票单位、电话读取分销商信息，出票时间为订单时间)
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

    <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
    <input id="hid_comid_temp" type="hidden" value="<%=comid_temp %>" />
    <input id="hid_id" type="hidden" value="<%=id %>" />
    <input id="hid_payprice" type="hidden" value="0" />
    
</asp:Content>