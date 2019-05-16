<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ComFinance.aspx.cs"
    Inherits="ETS2.WebApp.UI.PermissionUI.ComFinance" %>

<%@ Register Src="/UI/CommonUI/Control/UploadFileControl.ascx" TagName="uploadFile"
    TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 20; //每页显示条数

        $(function () {
            //日历
            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {
                $($(this)).datepicker();
            });

            //            //获得交易类型列表
            //            Selpayment_type();
            //            //获得支付方式列表
            //            Selmoney_come();

            var userid = $("#hid_userid").val();
            var comid = $("#hid_comid").val();
            SearchList(1);

            $("#Search").click(function () {
                var payment_type = $("#payment_type").val();
                var money_come = $("#money_come").val();
                var starttime = $("#startime").val();
                var endtime = $("#endtime").val();
                var orderid = $("#oid").val();
                var key = $("#key").val();

                SearchList(1, payment_type, money_come, starttime, endtime, orderid, key);
            })


            //装载财务列表
            function SearchList(pageindex, payment_type, money_come, starttime, endtime, orderid, key) {
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/FinanceHandler.ashx?oper=ComFinancelist",
                    data: { pageindex: pageindex, pagesize: pageSize, key: key, oid: orderid, payment_type: payment_type, money_come: money_come, starttime: starttime, endtime: endtime },
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
                                $("#tblist").html("查询数据为空");
                            } else {
                                $("#FinanceItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pageSize, pageindex, payment_type, money_come, starttime, endtime, orderid, key);
                            }


                        }
                    }
                })


            }

            //分页
            function setpage(newcount, newpagesize, curpage, payment_type, money_come, starttime, endtime, orderid, key) {
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

                        SearchList(page, payment_type, money_come, starttime, endtime, orderid, key);

                        return false;
                    }
                });
            }




            //电子码提交
            $("#submit_conf").click(function () {

                var id_temp = $("#F1").text();
                if (id_temp == "") {
                    $.prompt("操作出现错误");
                    return;
                }
                var remarks = $("#remarks").val(); //  电子码
                if (remarks == "") {
                    $.prompt("请填写完成打款备注");
                    return;
                }

                var printscreen = $("#<%=headPortrait.FileUploadId_ClientId %>").val();


                $.post("/JsonFactory/FinanceHandler.ashx?oper=WithdrawConf", { printscreen: printscreen, remarks: remarks, id: id_temp, comid: comid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $("#F1").val("");
                        $("#F2").val("");
                        $("#F3").val("");
                        $("#F4").val("");
                        $("#remarks").val("")
                        $("#manageWithdraw").hide();

                        $.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $("#F1").val("");
                        $("#F2").val("");
                        $("#F3").val("");
                        $("#F4").val("");
                        $("#remarks").val("")
                        $("#manageWithdraw").hide();

                        $.prompt("操作成功" + data.msg, {
                            buttons: [{ title: '确定', value: true}],
                            opacity: 0.1,
                            focus: 0,
                            show: 'slideDown',
                            submit: callbackfunc
                        });
                    }
                })
                function callbackfunc(e, v, m, f) {
                    if (v == true)
                        window.location.reload();
                }

            })

            //关闭窗口
            $("#cancel_conf").click(function () {
                $("#manageWithdraw").hide();
            })


        })

        //弹出窗口
        function referrer_Withdraw(id, comid, money, type1, remarks, printscreenurl) {
            $("#F1").text(id);
            $("#F2").text(type1);
            $("#F3").text("￥" + money);
            $("#F4").text(remarks);


            bindViewImg(printscreenurl);

            $("#manageWithdraw").show();
        };
        function bindViewImg(printscreenurl) {
            var defaultPath = "";
            var imgSrc = printscreenurl;
            if (imgSrc == "") {
                $("#headPortraitImg").attr("src", defaultPath);
            } else {
                var filePath = '<%=headPortrait.fileUrl %>';
                var headlogoImgSrc = filePath + imgSrc;
                $("#headPortraitImg").attr("src", headlogoImgSrc);
            }
        }


        function Selpayment_type() {
            $.post("/JsonFactory/FinanceHandler.ashx?oper=Selpayment_type", {}, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {
                    var tstr = '<option value="" selected="selected">请选择..</option>';
                    for (var i = 0; i < data.msg.length; i++) {
                        if (data.msg[i] != '') {
                            tstr += '<option value="' + data.msg[i] + '">' + data.msg[i] + '</option>';
                        }
                    }
                    $("#payment_type").html(tstr);
                }
            })
        }
        function Selmoney_come() {
            $.post("/JsonFactory/FinanceHandler.ashx?oper=Selmoney_come", {}, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {
                    var tstr = '<option value="" selected="selected">请选择..</option>';
                    for (var i = 0; i < data.msg.length; i++) {
                        if (data.msg[i] != '') {
                            tstr += '<option value="' + data.msg[i] + '">' + data.msg[i] + '</option>';
                        }
                    }
                    $("#money_come").html(tstr);
                }
            })
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="MasterList.aspx" onfocus="this.blur()" target=""><span>人员管理</span></a></li>
                <li><a href="SSort.aspx" onfocus="this.blur()" target=""><span>商户管理</span></a></li>
                <li class="on"><a href="ComFinance.aspx" onfocus="this.blur()" target=""><span>财务对账表</span></a></li>
                <li><a href="Withdraw_handle.aspx" onfocus="this.blur()" target=""><span>提现财务管理</span></a></li>
                <li><a href="ticketlist.aspx" onfocus="this.blur()"><span>退票管理</span></a></li>
                <li><a href="bankmanager.aspx" onfocus="this.blur()"><span>绑定银行管理</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <div style="text-align: left;">
                    <div style="display: none;">
                        <label style="color: #4D4D4D; font-size: 14px; position: relative;">
                            交易类型:</label>
                        <select id="payment_type" class="mi-input" style="width: 140px;">
                            <option value="0" selected="selected">请选择..</option>
                        </select>
                        <label style="color: #4D4D4D; font-size: 14px; position: relative;">
                            支付方式:</label>
                        <select id="money_come" class="mi-input" style="width: 140px;">
                            <option value="0" selected="selected">请选择..</option>
                        </select>
                        <br />
                        <label style="color: #4D4D4D; font-size: 14px; position: relative;">
                            起止时间:</label>
                        <input class="mi-input" name="startime" id="startime" placeholder="起始时间" value=""
                            isdate="yes" type="text">-
                        <input class="mi-input" name="endtime" id="endtime" placeholder="结束时间" value="" isdate="yes"
                            type="text"><br />
                    </div>
                    <label style="color: #4D4D4D; font-size: 14px; position: relative;">
                        订单号:
                    </label>
                    <input name="oid" type="text" id="oid" class="mi-input" placeholder="订单号">
                    <label style="color: #4D4D4D; font-size: 14px; position: relative;">
                        关键词:
                    </label>
                    <input name="key" type="text" id="key" class="mi-input" placeholder="支付姓名，手机，对接流水号">
                    <label>
                        <input name="Search" type="button" id="Search" value="  查  询  " style="width: 120px;
                            height: 26px;">
                    </label>
                </div>
                <table width="780" border="0">
                    <tr>
                        <td width="35">
                            <p align="left">
                                流水号</p>
                        </td>
                        <td width="65">
                            <p align="left">
                                提交时间
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                收支类型
                            </p>
                        </td>
                        <td width="35">
                            <p align="left">
                                公司ID
                            </p>
                        </td>
                        <td width="120">
                            <p align="left">
                                公司名称
                            </p>
                        </td>
                        <td width="40">
                            <p align="left">
                                订单号
                            </p>
                        </td>
                        <td width="40">
                            <p align="left">
                                支付人
                            </p>
                        </td>
                        <td width="80">
                            <p align="left">
                                支付手机
                            </p>
                        </td>
                        <td width="170">
                            <p align="left">
                                核对号
                            </p>
                        </td>
                        <td width="40">
                            <p align="left">
                                收入
                            </p>
                        </td>
                        <td width="40">
                            <p align="left">
                                支出
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                余额
                            </p>
                        </td>
                        <td width="140">
                            <p align="left">
                                资金渠道
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
    <script type="text/x-jquery-tmpl" id="FinanceItemEdit">   
                    <tr>
                        <td >
                            <p align="left">
                                ${Id}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${ChangeDateFormat(Subdate)}
                            </p>
                        </td>
                         
                        <td >
                            <p align="left">
                                ${Payment_type}</p>
                        </td>
                        <td >
                            <p align="left">
                               ${Com_id}</p>
                        </td>
                         <td >
                            <p align="left">
                               ${ComName}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${Order_id}</p>
                        </td>
                        <td >
                            <p align="left">
                                {{if Money>= 0}} {{if Payinfo!= null}}${Payinfo.Pay_name}{{/if}}{{/if}}</p>
                        </td>
                        <td >
                            <p align="left">
                                {{if Money>= 0}}{{if Payinfo!= null}} ${Payinfo.Pay_phone}{{/if}}{{/if}}</p>
                        </td>
                        <td >
                            <p align="left" title="{{if Money>= 0}}{{if Payinfo!= null}}${Payinfo.Trade_no}{{/if}}{{/if}}">
                                 {{if Money>= 0}}{{if Payinfo!= null}}${Payinfo.Trade_no}{{/if}}{{/if}}</p>
                        </td>
                        <td >
                            <p align="left">
                              {{if Money>= 0}}${Money}{{/if}}</p>
                        </td>
                        <td >
                            <p align="left">
                              {{if Money< 0}}${Money}{{/if}}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${Over_money}    
                            </p>
                        </td>

                        <td >
                            <p align="left">
                            ${Money_come}
                            </p> 
                        </td>
                    </tr>
    </script>
    <div id="manageWithdraw" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 600px; height: 500px; display: none; left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 18px;" id="title"></span>
                </td>
            </tr>
            <tr>
                <td width="21%" height="22" align="right" bgcolor="#E7F0FA" class="tdHead">
                    财务订单:
                </td>
                <td bgcolor="#E7F0FA" class="tdHead" id="">
                    <span style="padding-left: 5px;" id="F1"></span>
                </td>
            </tr>
            <tr>
                <td height="22" align="right" bgcolor="#E7F0FA" class="tdHead">
                    操作类型:
                </td>
                <td bgcolor="#E7F0FA" class="tdHead" id="td1">
                    <span style="padding-left: 5px;" id="F2"></span>
                </td>
            </tr>
            <tr>
                <td height="22" align="right" bgcolor="#E7F0FA" class="tdHead">
                    金额:
                </td>
                <td bgcolor="#E7F0FA" class="tdHead" id="td2">
                    <span style="padding-left: 5px;" id="F3"></span>
                </td>
            </tr>
            <tr>
                <td height="35" align="right" bgcolor="#E7F0FA" class="tdHead">
                    备注:
                </td>
                <td bgcolor="#E7F0FA" class="tdHead" id="td3">
                    <span style="padding-left: 5px; word-break: break-all; width: 200px; overflow: auto;
                        }" id="F4"></span>
                </td>
            </tr>
            <tr>
                <td height="35" align="right" bgcolor="#E7F0FA" class="tdHead">
                    打款成功:
                </td>
                <td bgcolor="#E7F0FA" class="tdHead" id="td4">
                    <textarea name="remarks" cols="35" rows="3" id="remarks"></textarea>
                    <br>
                    打款成功后详细说明，此内容商户可看到
                </td>
            </tr>
            <tr bgcolor="#E7F0FA" class="tdHead">
                <td class="tdHead" align="right">
                    提现截图：
                </td>
                <td>
                    <div class="C_head">
                        <dl>
                            <dt>
                                <input type="hidden" id="hid_logo" value="" />
                                <img alt="" class="headPortraitImgSrc" id="headPortraitImg" src="" width="100" height="60" /></dt>
                        </dl>
                        <div class="cl">
                        </div>
                    </div>
                    <div class="C_head_no">
                        <div class="C_head_1">
                            <ul>
                                <li style="height: 20px; margin-left: 40px">
                                    <div class="C_verify">
                                        <%--  <label>
                                            上传截图：</label>--%>
                                        <span>
                                            <uc1:uploadFile ID="headPortrait" runat="server" />
                                        </span>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td height="20" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input type="hidden" id="hid_luru_proid" value="" />
                    <input name="submit_luru" id="submit_conf" type="button" class="formButton" value="  确定完成打款  " />
                    <input name="cancel_luru" id="cancel_conf" type="button" class="formButton" value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
