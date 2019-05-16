<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master"  CodeBehind="ServerManage.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.ServerManage" %>

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

            $.post("/JsonFactory/ProductHandler.ashx?oper=Rentserverpagelist", { comid: comid, pageindex: 1, pagesize: 20 }, function (data) {
                data = eval("(" + data + ")");

                if (data.type == 1) {
                    //                        $.prompt(data.msg);
                    return;
                }
                if (data.type == 100) {
                    var hid_Fserver = $('#hid_Fserver').val();
                    if (data.totalcount == 0) {
                        //                                $("#tblist").html("查询数据为空");
                    } else {
                        for (var i = 0; i < data.msg.length; i++) {
                            if (hid_Fserver != 0 && hid_Fserver == data.msg[i].id) {
                                $("#Fserver").append('<option value="' + data.msg[i].id + '" selected>' + data.msg[i].servername + '</option>');
                            } else {
                                $("#Fserver").append('<option value="' + data.msg[i].id + '">' + data.msg[i].servername + '</option>');
                            }
                        }

                    }
                }
            })



            function SearchList() {

                $.post("/JsonFactory/ProductHandler.ashx?oper=Rentserverbyid", { comid: comid, id: id }, function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        //$.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $("#servername").val(data.msg.servername);
                        $('#WR').val(data.msg.WR);
                        $('#posid').val(data.msg.posid);
                        $('#saleprice').val(data.msg.saleprice);
                        $('#serverDepositprice').val(data.msg.serverDepositprice);
                        $('#renttype').val(data.msg.renttype);
                        $('#mustselect').val(data.msg.mustselect);
                        $('#servertype').val(data.msg.servertype);
                        $('#printticket').val(data.msg.printticket);
                        $('#Fserver').val(data.msg.Fserver);
                        $('#hid_Fserver').val(data.msg.Fserver);
                        $('#num').val(data.msg.num);
                        if (data.msg.Fserver == 0) {
                            $("#sonview").hide();
                            $("input:radio[name='Sonserver'][value='0']").attr("checked", true);
                        } else {
                            $("#sonview").show();
                            $("input:radio[name='Sonserver'][value='1']").attr("checked", true);
                        }

                    }
                })
            }

            $("#Sonserver0").click(function () {
                $("#sonview").hide();
            });
            $("#Sonserver1").click(function () {
                $("#sonview").show();
            });

            $("#button").click(function () {
                var servername = $('#servername').val();
                var servertype = $('#servertype').val();
                var WR = $('#WR').val();
                var posid = $('#posid').val();
                var saleprice = $('#saleprice').val();
                var serverDepositprice = $('#serverDepositprice').val();
                var renttype = $('#renttype').val();
                var mustselect = $('#mustselect').val();
                var printticket = $('#printticket').val();
                var num = $('#num').val();
                var Fserver = $('#Fserver').val();
                var Sonserver = $('input:radio[name="Sonserver"]:checked').trimVal();



                if (servername == "") {
                    $.prompt("请填写服务名称");
                    return;
                }
//                if (posid == "") {
//                    $.prompt("请填写PosID,此ID必须与客户端匹配");
//                    return;
//                }

                //                if (Startime == "") {
                //                    $.prompt("请填写起始日期");
                //                    return;
                //                }
                //                if (Endtime == "") {
                //                    $.prompt("请填写结束日期");
                //                    return;
                //                }


                $.post("/JsonFactory/ProductHandler.ashx?oper=upRentserver", { id: id, servername: servername, comid: comid, posid: posid, WR: WR, saleprice: saleprice, serverDepositprice: serverDepositprice, renttype: renttype, mustselect: mustselect, servertype: servertype, Fserver: Fserver, printticket: printticket, Sonserver: Sonserver, num: num }, function (data) {
                    data = eval('(' + data + ')');
                    if (data.type == '100') {
                        $.prompt("操作成功");
                        location.href = "ServerList.aspx";
                        return;
                    } else {
                        $.prompt(data.msg + " 操作失败！");
                        return;
                    }
                })
            })

            $("#js_btn_cancel").click(function () {
                location.href = "ServerList.aspx";
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
                        内部服务</h2>

                    <div class="mi-form-item">
                        <label class="mi-label">
                            服务类型</label>
                        <select id="servertype" class="mi-input" style="margin-left: 0; width: 160px;">
                                        <option value="0" selected>押金或小件租赁服务</option>
                                        <!--<option value="1" >退押金POS绑定</option>-->
                            </select>
                    </div>

                    <div class="mi-form-item">
                        <label class="mi-label">
                            服务名称</label>
                        <input id="servername" value="" name="servername" style="width: 160px; height: 20px;" type="text" placeholder="请输入服务名称" class="frm_input" autocomplete="off">
                    </div>


                    <div class="mi-form-item" style=" display:none;">
                        <label class="mi-label">
                            同时打印索道票（租用时）</label>
                            <select id="printticket" class="mi-input" style="margin-left: 0; width: 120px;">
                                        <option value="0" selected>不打印</option>
                                        <option value="1" >打印</option>
                            </select>
                    </div>
                    
                    
                    <div class="mi-form-item" style=" display:none;">
                        <label class="mi-label">
                            是否需要归还</label>
                            <select id="WR" class="mi-input" style="margin-left: 0; width: 120px;">
                                        <option value="0">不用归还</option>
                                        <option value="1" selected>归还</option>
                            </select>（租用的需要进行归还，一次性的不需要归还）
                    </div>
                    <div class="mi-form-item" style=" display:none;">
                        <label class="mi-label">
                            使用次数</label>
                            <select id="num" class="mi-input" style="margin-left: 0; width: 120px;">
                                        <option value="1" selected>1次</option>
                                        <option value="2" >2次</option>
                            </select>
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            必须支付的服务</label>
                            <select id="mustselect" class="mi-input" style="margin-left: 0; width: 120px;">
                                        <option value="0" selected>自由选择服务</option>
                                        <option value="1" >必须支付服务</option>
                            </select>
                    </div>
                    <div class="mi-form-item" >
                        <label class="mi-label">
                            商品编号</label>
                        <input id="renttype" value="" name="renttype" style="width: 120px; height: 20px;" type="text" placeholder="商品编号，和商户对接使用" class="frm_input" autocomplete="off">
                    </div>
                    <div class="mi-form-item" style="display:none;">
                        <label class="mi-label">
                            是否为子服务（客户订购主终端服务后，自动可使用子服务）</label>
                            <input name="Sonserver" id="Sonserver0" value="0" checked="checked" type="radio">不是子服务 <input name="Sonserver"  id="Sonserver1"  value="1" type="radio">是子服务 
                       <div id="sonview" style="  display:none;">
                        <label class="mi-label">
                            请选择主服务</label>
                            <select id="Fserver" class="mi-input" style="margin-left: 0; width: 120px;">
                                        <option value="0" selected>请选择主终端服务</option>
                            </select>
                        </div>
                    </div>

                    <div class="mi-form-item">
                        <label class="mi-label">
                            售价</label>
                        <input id="saleprice" value="" name="saleprice" style="width: 60px; height: 20px;" type="text" placeholder="请输入售价" class="frm_input" autocomplete="off">元
                        （当客户选择此服务则会增加到支付金额中，做为订单的中的收入，如果只需要交押金此设定为0）
                    </div>
                    <div class="mi-form-item">
                        <label class="mi-label">
                            押金金额</label>
                        <input id="serverDepositprice" value="" name="serverDepositprice" style="width: 60px; height: 20px;" type="text" placeholder="请输入押金" class="frm_input" autocomplete="off">元
                        （当客户选择此服务押金也会增加到支付金额中，退还押金时按此设定金额退还）
                    </div>
                    

                    <div class="mi-form-item" style=" display:none;">
                        <label class="mi-label">
                           PosID</label>
                        <input type="text" id="posid" class="frm_input" value="" style="width: 160px; height: 20px;" >
                        （此pos一定是通过系统分配id，否则不能通过验证）
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
                    <input type="hidden" id="hid_Fserver" value="0" />
</asp:Content>