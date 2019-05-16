<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="Bus_Feeticket_add.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.Bus_Feeticket_add" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
<link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-impromptu.4.0.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {

            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {
                $($(this)).datepicker();
            });



            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();
            var id = $("#hid_id").trimVal();
            if (id != 0) {
                SearchList();
            }
            function SearchList() {

                $.post("/JsonFactory/ProductHandler.ashx?oper=getbusfeeticketbyid", { comid: comid, id: id }, function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        //$.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $("#Title").val(data.msg.Title);
                        $('#Feeday').val(data.msg.Feeday);
                        $('#iuse').val(data.msg.Iuse);
                        

                        $('#Startime').val(ChangeDateFormat(data.msg.Startime));
                        $('#Endtime').val(ChangeDateFormat(data.msg.Endtime));


                    }
                })
            }

            $("#button").click(function () {
                var title = $('#Title').val();
                var Feeday = $('#Feeday').val();
                var Startime = $('#Startime').val();
                var Endtime = $('#Endtime').val();
                var iuse = $('#iuse').val();
                if (title == "") {
                    $.prompt("请填写免费券名称");
                    return;
                }

//                if (Startime == "") {
//                    $.prompt("请填写起始日期");
//                    return;
//                }
//                if (Endtime == "") {
//                    $.prompt("请填写结束日期");
//                    return;
//                }


                $.post("/JsonFactory/ProductHandler.ashx?oper=upbusfeeticket", { id: id, title: title, comid: comid, Feeday: Feeday, Startime: Startime, Endtime: Endtime, iuse: iuse }, function (data) {
                    data = eval('(' + data + ')');
                    if (data.type == '100') {
                        $.prompt("操作成功");
                        location.href = "Bus_Feeticket_manage.aspx";
                        return;
                    } else {
                        $.prompt(data.msg +" 操作失败！" );
                        return;
                    }
                })
            })

            $("#js_btn_cancel").click(function () {
                 location.href = "Bus_Feeticket_manage.aspx";
            })


        })

      
    </script>
   
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">

        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <div class="col_main">
                    <div class="main_bd">

                        <div class="edit-box J-commonSettingsModule" style="opacity: 1; margin-top: 0px;
                    position: relative; z-index: 10;">
                    <h2 class="p-title-area">
                        免费券管理</h2>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            免费券名称</label>
                        <input id="Title" value="" name="Title" type="text" placeholder="" class="frm_input" autocomplete="off">
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            限制本人使用</label>
                            <select id="iuse" class="mi-input" style="margin-left: 0; width: 120px;">
                                        <option value="0">不限制使用</option>
                                        <option value="1">限制本人使用</option>
                                        <option value="2">限当天每班次只能预订一次</option>
                                    </select>
                    </div>
                    <div class="mi-form-item" style=" display:none;">
                        <label class="mi-label">
                            使用限制</label>
                        <select id="Feeday" class="mi-input" style="margin-left: 0; width: 120px;">
                                        <option value="0">不限制使用</option>
                                        <option value="1">限制每个产品指定次数</option>
                                    </select>
                    </div>
                    <div class="mi-form-item" style="  display:none;">
                        <label class="mi-label">
                           开始日期</label>
                        <input type="text" id="Startime" class="js_delivery_type frm_checkbox" value="" isdate="yes"  >
                    </div>
                    <div class="mi-form-item" style="  display:none;">
                        <label class="mi-label">
                            结束日期</label>
                         <input type="text" id="Endtime" class="js_delivery_type frm_checkbox" value=""  isdate="yes">

                    </div>
                    <div class="mi-form-explain">
                    </div>
                </div>
                <table class="grid" width="780px;">
                    <tbody><tr>
                        <td align="center" height="80">
                            <input name="button" id="button" class="mi-input" value="      确 认 提 交     " type="button">
                            
                           
                        </td>
                    </tr>
                </tbody></table>

                    </div>

                    <input type="hidden" id="hid_id" value="<%=id %>" />
</asp:Content>