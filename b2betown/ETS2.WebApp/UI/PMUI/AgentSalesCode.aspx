<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="AgentSalesCode.aspx.cs"
    Inherits="ETS2.WebApp.UI.PMUI.AgentSalesCode" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script> 

    <script type="text/javascript">   
        $(function () {
            //日历
            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {
                $($(this)).datepicker();
            });

            $("html").die().live("keydown", function (event) {
                if (event.keyCode == 13) {
                    $("#Search").click();    //这里添加要处理的逻辑  
                    return false;
                }
            });
            $.post("/JsonFactory/ProductHandler.ashx?oper=getservertypelist", {}, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $("#sel_servertype").html('<option value="0">全部</option>');
                }
                if (data.type == 100) {
                    var optionstr = '<option value="0">全部</option>';
                    for (var i = 0; i < data.msg.length; i++) {
                        optionstr += '<option value="' + data.msg[i].ID + '">' + data.msg[i].Value + '</option>'
                    }
                    $("#sel_servertype").html(optionstr);
                }
            })
            var pageSize = 10; //每页显示条数
            var comid = $("#hid_comid").trimVal();
            var userid = $("#hid_userid").trimVal();

            //获取订单列表
            SearchList(1);
            //装载产品列表
            function SearchList(pageindex) {
                var servertype = $("#sel_servertype").trimVal();
                var key = $("#key").trimVal();
                var order_state = $("#order_state").trimVal();
                var datetype = $("#datetype").trimVal();
                if ($("#startime").trimVal() != "" || $("#endtime").trimVal() != "") {
                    if ($("#startime").trimVal() == "" || $("#endtime").trimVal() == "") {
                        alert("开始时间和结束时间需要同时选择");
                        return;
                    }
                }

                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                if ($("#startime").trimVal() != "" || $("#endtime").trimVal() != "") {
                    if ($("#startime").trimVal() == "" || $("#endtime").trimVal() == "") {
                        alert("开始时间和结束时间需要同时选择");
                        return;
                    }
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=getorderlist",
                    data: { servertype: servertype, comid: comid, pageindex: pageindex, pagesize: pageSize, key: key, order_state: order_state, ordertype: 2, beginDate: $("#startime").trimVal(), endDate: $("#endtime").trimVal(), datetype: datetype },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询订单列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                //                                $("#tblist").html("查询数据为空");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pageSize, pageindex);

                                var oidstr = ""; //订单号列表
                                $.each($("input[name='hid_hzinsorderid']"), function (i, item) {
                                    oidstr += $(item).val() + ",";
                                })
                                if (oidstr != "") {
                                    oidstr = oidstr.substr(0, oidstr.length - 1);
                                    //查询慧择网保单列表
                                    $.post("/JsonFactory/OrderHandler.ashx?oper=GethzinsorderSearch", { oidstr: oidstr }, function (datat) {
                                        datat = eval("(" + datat + ")");
                                        if (datat.type == 1) {
                                            alert("查询慧择网保单状态出错");
                                            return;
                                        }
                                        if (datat.type == 100) {
                                            for (var ii = 0; ii < datat.msg.length; ii++) {
                                                //                                                alert(datat.msg[ii].insureNum);
                                                $("#span_" + datat.msg[ii].insureNum).text(datat.msg[ii].effectiveStateStr);
                                                if (datat.msg[ii].effectiveState == 1)//未生效之前可以退保
                                                {
                                                    $("#back_" + datat.msg[ii].insureNum).show();
                                                }

                                            }
                                        }
                                    })
                                }
                            }


                        }
                    }
                })


            }

            $("#Search").click(function () {
                SearchList(1);
            })
            //退票
            $("#Enter").click(function () {
                var id = $("#hid_id").val();
                var proid = $("#hid_proid").val();
                var num = $("#tnum").val();
                var testpro = $("#testpro").val();

                if (id == 0) {
                    $.prompt("错误id！");
                    return;
                }
                if (num == "") {
                    num = 1;
                }
                if (testpro == "") {
                    $.prompt("请填写退款说明备注！");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=com-getticket",
                    data: { comid: comid, userid: userid, id: id, proid: proid, num: num, testpro: testpro },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("提交退款申请错误" + data.msg);
                            return;
                        }
                        if (data.type == 100) {
                            $("#hid_id").val(0);
                            $.prompt("提交退款申请成功" + data.msg, { submit: function () { location.href = "/ui/pmui/AgentSalesCode.aspx" } });
                            //                            window.location.reload();
                            return;
                        }
                    }
                })


            })
            //改期
            $("#ChangeTraveldata").click(function () {
                var id = $("#hid_id").val();
                var proid = $("#hid_proid").val();
                var num = $("#tnum").val();
                var testpro = $("#testpro").val();

                if (id == 0) {
                    $.prompt("错误id！");
                    return;
                }
                if (num == "") {
                    num = 1;
                }
                if (testpro == "") {
                    $.prompt("请填写改期说明备注！");
                    return;
                }
                if ($("#ktraveldate").trimVal() == $("#hid_oldtraveldate").trimVal())
                {
                    $.prompt("乘车日期没有变化！");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=changetraveldate",
                    data: { comid: comid, userid: userid, id: id, proid: proid, num: num, testpro: testpro, traveldate: $("#ktraveldate").trimVal(), oldtraveldate: $("#hid_oldtraveldate").trimVal() },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt(data.msg);
                            return;
                        }
                        if (data.type == 100) {
                            $("#hid_id").val(0);
                            $.prompt("提交改期申请成功");
                            window.location.reload();
                            return;
                        }
                    }
                })


            })


            $("#w_Enter").click(function () {
                var id = $("#hid_id").val();
                var proid = $("#hid_proid").val();
                var testpro = $("#w_testpro").val();
                var confirmstate = $('input:radio[name="confirmstate"]:checked').val();
                if (id == 0) {
                    $.prompt("订单ID错误！");
                    return;
                }
                if (proid == "") {
                    $.prompt("产品ID错误！");
                    return;
                }
                if (confirmstate == "") {
                    $.prompt("确认状态错误！");
                    return;
                }



                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=createdaoma",
                    data: { comid: comid, id: id, proid: proid, confirmstate: confirmstate, testpro: testpro },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("返回错误，请刷新后重新操作");
                            return;
                        }
                        if (data.type == 100) {
                            $("#hid_id").val(0);
                            $.prompt("已经确认");
                            window.location.reload();
                            return;
                        }
                    }
                })



            })


            $("#cancel_express").click(function () {
                //关闭
                $("#express").hide();
            });

            $("#cancel_cart_express").click(function () {
                //关闭
                $("#expresscart").hide();
            });

            $("#express_sub").click(function () {
                var id = $("#hid_id").val();
                var expresscom = $("#expresscom").val();
                var expresscode = $("#expresscode").val();
                var expresstext = $("#expresstext").val();

                if (id == 0) {
                    $.prompt("订单ID错误！");
                    return;
                }
                if (expresscom == "") {
                    $.prompt("请填写快递公司！");
                    return;
                }
                if (expresscode == "") {
                    $.prompt("请填写快递号！");
                    return;
                }


                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=confirexpress",
                    data: { comid: comid, id: id, expresscom: expresscom, expresscode: expresscode, expresstext: expresstext },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("返回错误，请刷新后重新操作");
                            return;
                        }
                        if (data.type == 100) {
                            $("#hid_id").val(0);
                            $.prompt("已经确认");
                            window.location.reload();
                            return;
                        }
                    }
                })


            })


            $("#express_cart_sub").click(function () {
                var id = $("#hid_id").val();
                var expresscom = $("#expresscom_cart").val();
                var expresscode = $("#expresscode_cart").val();
                var expresstext = $("#expresstex_cartt").val();

                if (id == 0) {
                    $.prompt("订单ID错误！");
                    return;
                }
                if (expresscom == "") {
                    $.prompt("请填写快递公司！");
                    return;
                }
                if (expresscode == "") {
                    $.prompt("请填写快递号！");
                    return;
                }


                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=confircartexpress",
                    data: { comid: comid, id: id, expresscom: expresscom, expresscode: expresscode, expresstext: expresstext },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("返回错误，请刷新后重新操作");
                            return;
                        }
                        if (data.type == 100) {
                            $("#hid_id").val(0);
                            $.prompt("已经确认");
                            window.location.reload();
                            return;
                        }
                    }
                })


            })



            //分页
            function setpage(newcount, newpagesize, curpage) {
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

                        SearchList(page);

                        return false;
                    }
                });
            }




            $("#Enterhotelcon").click(function () {
                var id = $("#hid_id").val();
                var paystate = $("#hid_paystate").val();
                

                var confirmstate = $('input:radio[name="roomstate"]:checked').val();
                if (confirmstate == undefined) {
                    confirmstate = "";
                }

                if (id == 0) {
                    $.prompt("订单ID错误！");
                    return;
                }
                if (confirmstate == "") {
                    $.prompt("请确认房间确认状态！");
                    return;
                }



                orderfin(id, confirmstate, paystate);



            })

            $("#AgentSalesCodeToExcel").click(function () { 
                window.open("/excel/DownExcel.aspx?oper=agentsalescodetoexcel&comid=" + comid + "&order_state=" + $("#order_state").trimVal() + "&servertype=" + $("#sel_servertype").trimVal() + "&key=" + $("#key").trimVal() + "&beginDate=" + $("#startime").trimVal() + "&endDate=" + $("#endtime").trimVal() + "&userid=" + userid + "&ordertype=2", target = "_blank");
            })

        })





        //房间标注已处理
        function hotelfin(str1, str2, str3, str4, str5, str6, str7) {

            $("#hid_id").val(str1);
            $("#Proname").html(str2);
            $("#Start_date").html(str3);
            $("#End_date").html(str4);
            $("#U_name").html(str5);
            $("#U_phone").html(str6);

            $("#hid_paystate").val(str7);//如果支付状态为0 则是确认是否有房
            if (str7 == 0) {

                $("#entyes").text("有房");
                $("#entno").text("无房");

            } else {
                $("#entyes").text("确认房已预订成功");
                $("#entno").text("无房作废");
            }


            $("#hotelcon").show();
        }
        function closehotelcon() {
            $("#hotelcon").hide();
        }






        //标注已处理
        function orderfin(oid, state,paystate) {
            if (oid == '') {
                $.prompt("参数传递错误");
                return;
            }
            var comid = $("#hid_comid").trimVal();
            var str = "确认处理";

            if (paystate == 0) {
                if (state == 1) {
                    str = "确认有房！";
                }
                if (state == 0) {
                    str = "房满作废，订单自动取消！";
                }

            } else {
                if (state == 1) {
                    str = "确认预留房间，并发送确认通知消息！";
                }
                if (state == 0) {
                    str = "房满作废，发送作废通知消息！";
                }

            
            }


            if (confirm(str)) {
                if (paystate == 0) {
                    $.ajax({
                        type: "post",
                        url: "/JsonFactory/OrderHandler.ashx?oper=orderyoufang",
                        data: { comid: comid, id: oid, state: state },
                        async: false,
                        success: function (data) {
                            data = eval("(" + data + ")");

                            if (data.type == 1) {
                                $.prompt("操作出错");
                                return;
                            }
                            if (data.type == 100) {
                                $.prompt("操作成功");
                                window.location.reload();
                            }
                        }
                    })
                } else {
                    $.ajax({
                        type: "post",
                        url: "/JsonFactory/OrderHandler.ashx?oper=orderfin",
                        data: { comid: comid, id: oid, state: state, paystate: paystate },
                        async: false,
                        success: function (data) {
                            data = eval("(" + data + ")");

                            if (data.type == 1) {
                                $.prompt("操作出错");
                                return;
                            }
                            if (data.type == 100) {
                                $.prompt("操作成功");
                                window.location.reload();
                            }
                        }
                    })
                }
            }
        }




        function sendticketsms(oid) {
            if (oid == '') {
                $.prompt("参数传递错误");
                return;
            }
            var comid = $("#hid_comid").trimVal();
            $.ajax({
                type: "post",
                url: "/JsonFactory/OrderHandler.ashx?oper=sendticketsms",
                data: { comid: comid, oid: oid },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt("发送短信错误，请查看码库存是否充足！");
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("发送成功");
                        window.location.reload();
                    }
                }
            })


        }
        function restticketsms(oid) {
            if (oid == '') {
                $.prompt("参数传递错误");
                return;
            }
            var comid = $("#hid_comid").trimVal();



            $.ajax({
                type: "post",
                url: "/JsonFactory/OrderHandler.ashx?oper=restticketsms",
                data: { comid: comid, oid: oid },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        //                        $.prompt("重发短信失败");
                        alert(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("重发成功");
                        window.location.reload();
                    }
                }
            })


        }
        function guoqi_biaozhu(oid) {
            if (oid == '') {
                $.prompt("参数传递错误");
                return;
            }
            var comid = $("#hid_comid").trimVal();

            if (confirm("订单："+oid+"确认，标注为已过期订单吗？")) {
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=guoqi_biaozhu",
                    data: { comid: comid, oid: oid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            //                        $.prompt("重发短信失败");
                            alert(data.msg);
                            return;
                        }
                        if (data.type == 100) {
                            alert("标注成功");
                        }
                    }
                })

            }

        }
        


        function referrer_ch(str1, str2, str3) {
            $("#span_rh").text("用户：" + str1 + " 订购:" + str2 + " 短信:");
            $("#smstext").text(str3);
            $("#rhshow").show();
        }

        function cancel() {
            $("#rhshow").hide();
            $("#span_rh").text("");
            $("#smstext").text("");

            //退票
            $("#showticket").hide();
        }

        //退票
        function backticket_ch(type, id, proid, proname, pronum, pirce, Source_type, backnum) {
            if (Source_type == 2) {
                alert("此订单产品为“倒码产品”,本系统无法验证使用情况，请自行验证，退票则按未使用进行退款。");
            }
            $("#span_ticket").text(type);
            $("#pro_name").html(proname);
            $("#pro_num").html(pronum);

            $("#tr_ktraveldate").hide();
            $("#tr_tnum").show();
            //提交按钮 根据 操作类型(1退票;2改期) 显隐
            $("#Enter").show();
            $("#ChangeTraveldata").hide();

            $("#tnum").val(backnum);
            //$("#tnum").attr("readonly", "readonly");
            $("#hid_id").val(id);
            $("#hid_proid").val(proid);
            $("#span_price").html(pirce);
            $("#showticket").show();
        }
       
        //改变出游日期
        function ChangeTraveldate_ch(type, id, proid, proname, pronum, pirce, Source_type, backnum, servertype, traveldate) {
            $("#span_ticket").text(type);
            $("#pro_name").html(proname);
            $("#pro_num").html(pronum);
            $("#tnum").val(backnum);
            $("#tnum").attr("readonly", "readonly");
            $("#hid_id").val(id);
            $("#hid_proid").val(proid);
            $("#span_price").html(pirce);

            $("#tr_ktraveldate").show();
            $("#tr_tnum").hide();
            //提交按钮 根据 操作类型(1退票;2改期) 显隐
            $("#Enter").hide();
            $("#ChangeTraveldata").show();

            $("#ktraveldate").val(ChangeDateFormat(traveldate));
            $("#hid_oldtraveldate").val(ChangeDateFormat(traveldate));

            var tody = new Date();
            nian = tody.getFullYear();
            month = tody.getMonth() + 1;
            day = tody.getDate();
            month = month < 10 ? "0" + month : month
            day = day < 10 ? "0" + day : day
            var jt = nian + "-" + month + "-" + day;

            $("#ktraveldate").datepicker({ minDate: new Date(jt) });

            $("#showticket").show();

        }

        //倒码确认
        function agentwarrant_ch(type, id, proid, proname, pronum, pirce) {
            $("#w_span_ticket").text(type);
            $("#w_pro_name").html(proname);
            $("#w_num").html(pronum);
            $("#w_num").val(pronum);
            $("#w_num").attr("disabled", "disabled");
            $("#hid_id").val(id);
            $("#hid_proid").val(proid);
            $("#w_price").html(pirce);
            $("#showwarrant").show();
        }

        //关闭窗口
        function w_cancel() {
            //退票
            $("#showwarrant").hide();
        }


        //弹出实物订单
        function orderfin_express(str0, str1, str2, str3, str4, str5, str6, str7, str8, str9, str10) {
            $("#hid_id").val(str0);

            $("#express_proname").html(str1);
            $("#express_name").html(str2);
            $("#express_phone").html(str3);
            $("#express_num").html(str4);
            $("#express_address").html(str5);
            $("#expresstext").html(str6);
            $("#expresscom").val(str7);
            $("#expresscode").val(str8);
            $("#expresspno").html(str9);
            if (str10 != "2") {
                $("#express_sub").val("  修  改  ");
            } else {
                $("#express_sub").val(" 确认发货 ");
            }
            if (str0 == '') {
                $.prompt("参数传递错误");
                return;
            }
            $("#express").show();

        }


        //弹出订单
        function orderinfo(str0, str1, str2, str3, str4, str5, str6, str7, str8, str9, str10) {
            $("#orderinfo_id").val(str0);

            $("#orderinfo_proname").html(str1);
            $("#orderinfo_name").html(str2 + "(" + str3 + ")");

            $("#orderinfo_date").html(str4);
            $("#orderinfo_text").html(str5);
            $("#RecerceSMSpeople").html(str6);
            if (str7 != "" && str7 != null) {
                $("#RecerceSMSpeople").html(str9 + ":" + str8 + ":" + str7 + "(" + str10 + ")");
            }

            $("#orderinfo").show();

        }

        //弹出实物订单
        function orderfin_cart_express(str, str0, str1, str2, str3, str4, str5, str6, str7, str8, str9, str10) {
            var idarr = "";
            $.ajax({
                type: "post",
                url: "/JsonFactory/OrderHandler.ashx?oper=getcartorderlist",
                data: { comid: $("#hid_comid").trimVal(), cartid: str },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt("查询订单列表错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#prolist").html("");
                        if (data.totalCount == 0) {
                            //$("#tblist").html("查询数据为空");
                        } else {
                            var prolisthtml = '<tr style="background: none repeat scroll 0% 0% #999999;">                               <td width="30" style="border-bottom: 1px solid #CCC;">                               <p align="left">                                    ID</p>                            </td>                            <td width="300" style="border-bottom: 1px solid #CCC;">                                <p align="left">                                    商品</p>                            </td>                            <td width="70" style="border-bottom: 1px solid #CCC;">                                <p align="right">数量</p>                            </td>                        </tr>';
                            var prosum = 0;

                            for (var i = 0; i < data.msg.length; i++) {
                                idarr += data.msg[i].Id + ","
                                prolisthtml += '<tr><td  style="border-bottom: 1px solid #CCC;"><p align="left">' + data.msg[i].Id + '</p></td><td  style="border-bottom: 1px solid #CCC;"><p align="left">' + data.msg[i].Proname + '</p></td><td style="border-bottom: 1px solid #CCC;"><p align="right">' + data.msg[i].U_num + '</p></td></tr>';
                                prosum += data.msg[i].Agent_price * data.msg[i].U_num;
                            }
                            $("#prolist").append(prolisthtml);
                            $("#hid_id").val(idarr);
                        }
                    }
                }
            })


            $("#express_cart_name").html(str2);
            $("#express_cart_phone").html(str3);
            $("#express_cart_address").html(str5);
            $("#expresstext_cart").html(str6);
            $("#expresscom_cart").val(str7);
            $("#expresscode_cart").val(str8);
            $("#expresspno_cart").html(str9);

            $("#express_cart_sub").val(" 确认发货 ");

            if (str0 == '') {
                $.prompt("参数传递错误");
                return;
            }
            $("#expresscart").show();

        }

        //旅游订单处理 
        function orderfin_lvyou(Id, Proname, U_name, U_phone, U_num, Order_remark, Order_state,oper) {
            $("#hid_id").val(Id);

            $("#Span1").html(Proname);
            $("#Span2").html(U_num);
            $("#Span3").html(U_name);
            $("#Span4").html(U_phone);
            $("#Textarea1").html(Order_remark);

            if (Id == '') {
                $.prompt("参数传递错误");
                return;
            }
            if(oper=='detail'){
              $("#Button1").hide();
            }else{
             $("#Button1").show();
            }

            $("#divlvyou").show(); 
        }
        $(function(){
          //旅游订单弹出层隐藏
          $("#Button2").click(function(){
            $("#divlvyou").hide();
          })
          //旅游订单处理
           $("#Button1").click(function(){
               var orderid=$("#hid_id").trimVal();
               $.post("/JsonFactory/OrderHandler.ashx?oper=uporderstate",{orderid:orderid,orderstate:"22"},function(data){
                  data=eval("("+data+")");
                  if(data.type==1){
                  }
                  if(data.type==100){
                        $.prompt("处理成功");
                        window.location.reload();
                  } 
               })
          })
        })
        //显示备注
        function showremark(str) {
            if (str != '') {
                alert(str);
                return;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="setting-home" class="vis-zone  mail-list">
            <div class="inner">
                <h3>
                    订单管理</h3>
                <div  style="float:right;padding-right:110px;">
                    <label>
                        请输入
                        <input name="key" type="text" id="key" placeholder="手机，姓名,订单号，电子码，产品名称" style="width: 160px; height: 20px;"  value="<%=kkey %>">
                    </label>
                    <select id="sel_servertype" class=" ">
                      
                    </select>
                    

                    <select id="order_state" class="">
                        <option value="0" selected>全部</option>
                        <option value="1">待支付款</option>
                        <option value="2">支付成功(未处理/未发码)</option>
                        <option value="4">已发码/已处理完成</option>
                        <option value="8">已使用/已发货</option>
                        <option value="16">退款</option>
                        <option value="25">已过期</option>
                    </select>
                    <label>
                     <select id="datetype" class="">
                        <option value="0" selected>按订单日期</option>
                        <option value="1">按出游/入住</option>
                    </select>
                        <input class="mi-input " name="startime" id="startime" placeholder="开始时间" value="" isdate="yes" style="width: 120px;" type="text">-
                        <input class="mi-input " name="endtime" id="endtime" placeholder="结束时间" value="" isdate="yes" style="width: 120px;" type="text">
                    </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="订单查询" style="width: 120px;
                            height: 26px;">
                    </label> 
                </div>
               <div style="float:right; padding-right:110px;">
                   <label>
                       <input   type="button"   value="导出到Excel" style="width: 120px;
                            height: 26px;" id="AgentSalesCodeToExcel"  >
                  </label>
                </div>
                <table width="780px" border="0">
                    <tr>
                        <td width="45px" height="30px">
                            <p align="left">
                                ID
                            </p>
                        </td>
                        <td width="80px">
                            <p align="left">
                                提交时间
                            </p>
                        </td>
                        <td width="200px">
                            <p align="left">
                                产品名称
                            </p>
                        </td>
                         <td width="60px">
                            <p align="left">
                            出行/入住日期
                            </p>
                        </td>
                        <td width="70px">
                            <p align="left">
                                分销商
                            </p>
                        </td>
                        <td width="100px">
                            <p align="left">
                                购买人
                            </p>
                        </td>
                        <td width="35px">
                            <p align="center">
                                门市价
                            </p>
                        </td>
                        <td width="35px">
                            <p align="center">
                                单价
                            </p>
                        </td>
                        <td width="25px">
                            <p align="center">
                                数量
                            </p>
                        </td>
                        <td width="25px">
                            <p align="center">
                                优惠
                            </p>
                        </td>
                        <td width="25px">
                            <p align="center">
                                运费
                            </p>
                        </td>
                        <td width="45px">
                            <p align="center">
                                实收
                            </p>
                        </td>
                        <td width="180px">
                            <p align="center">
                                状态
                            </p>
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                </table>
                <div id="divPage">
                </div>
                <p>
                    &nbsp;
                </p>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr class="fontcolor">
                        <td valign="top">
                            <p align="left">
                                ${Id}</p>
                        </td>
                        <td valign="top">
                            <p align="left" title="${jsonDateFormatKaler(U_subdate)}">
                                ${jsonDateFormatKaler(U_subdate)}</p>
                        </td>
                        <td valign="top">
                            <p align="left" title="${Proname}">
                               ${Proname}
                            </p>
                        </td>
                        <td valign="top" title="${jsonDateFormatKaler(U_traveldate)}">
                            <p align="left">
                                 {{if Server_type==9}}
                                    {{if Hotelinfo !="" && Hotelinfo != null }}
                                      ${ChangeDateFormat(Hotelinfo.Start_date)}/<br>${ChangeDateFormat(Hotelinfo.End_date)}
                                    {{/if}}
                                 {{else}}
                                    ${ChangeDateFormat(U_traveldate)}
                                {{/if}}
                                </p>
                        </td>
                        <td valign="top">
                            <p align="left">
                                {{if Server_type==3}}
                                ${ChangeDateFormat(U_traveldate)}
                                (${Agent_company})
                                {{else}}
                                    ${Agent_company}
                                {{/if}}
                                </p>
                        </td>
                        <td valign="top">
                            <p align="left">
                            {{if U_idcard !=""}}
                                 <input type="button" onclick="alert('姓名：${U_name} 身份证：${U_idcard} 手机：${U_phone}');" value="身份证"/>
                                {{/if}}
                                ${U_name}(${U_phone})
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                               ${Face_price}</p>
                        </td>
                        <td valign="top">
                            <p align="center">
                               ${Pay_price}</p>
                        </td>
                        <td valign="top">
                            <p align="center">
                              ${U_num}</p>
                        </td>
                        


                        <td valign="top">
                            <p align="center">
                                 {{if Order_state>1}} ${Integral1}{{/if}}</p>
                        </td>
                        <td valign="top">
                            <p align="center">
                               ${Express}</p>
                        </td>
                        <td valign="top">
                            <p align="center">
                               {{if Server_type==3}}${Pay_price+Express}{{else}}{{if Order_state>1}} ${Paymoney+Express} {{if Imprest1>=1}}+ ${Imprest1}{{/if}}{{if childreduce*Child_u_num>0}}-${childreduce*Child_u_num}{{/if}}{{/if}}  {{/if}}
                            </p>
                        </td>

                        <td valign="top">
                            <p align="center">

                         {{if OrderType_Hzins==0}}<!--非慧择网订单-->
                           {{if Source_type!=4 }}
                           {{if Server_type==11 || Server_type==9 }}
                                {{if Server_type==11 }}
                                    {{if Order_state==1}}
                                         等待客户付款
                                    {{/if}}
                                    {{if Order_state==2}}
                                         {{if Shopcartid==0}}
                                         订单已付款 <input type="button" onclick="orderfin_express('${Id}','${Proname}','${U_name}','${U_phone}','${U_num}','${Province}:${City}:${Address}:${Code}','${Order_remark}','','','${Pno}','${Order_state}')" style="color:#ff0000"  value="立即处理"/>
                                         {{else}}
                                         订单已付款(购物车) <input type="button" onclick="orderfin_cart_express('${Shopcartid}','${Id}','${Proname}','${U_name}','${U_phone}','${U_num}','${Province}:${City}:${Address}:${Code}','${Order_remark}','','','${Pno}','${Order_state}')" style="color:#ff0000"  value="立即处理"/>
                                         {{/if}}
                                         <input type="button" onclick="backticket_ch('退订申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${Source_type}','${Unuse_Ticket}','${Unuse_Ticket}')"  value="退订"/>

                                    {{/if}}
                                    {{if Order_state==4}}
                                        {{if Deliverytype==4}}
                                         自提，已发码(${Pno})  <input type="button" onclick="backticket_ch('退订申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${Source_type}','${Unuse_Ticket}','${Unuse_Ticket}')"  value="退订"/>
                                        {{else}}
                                         订单已发货<input type="button" onclick="orderfin_express('${Id}','${Proname}','${U_name}','${U_phone}','${U_num}','${Province}:${City}:${Address}:${Code}','${Order_remark}','${Expresscom}','${Expresscode}','${Pno}','${Order_state}')" value="详情"/><input type="button" onclick="backticket_ch('退订申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${Source_type}','${Unuse_Ticket}','${Unuse_Ticket}')"  value="退订"/>
                                        {{/if}}
                                    {{/if}}
                                    {{if Order_state==8}}
                                         订单已提货,<input type="button" onclick="orderfin_express('${Id}','${Proname}','${U_name}','${U_phone}','${U_num}','${Province}:${City}:${Address}:${Code}','${Order_remark}','${Expresscom}','${Expresscode}','${Pno}','${Order_state}')" value="详情"/>
                                    {{/if}}
                                    {{if Order_state==16}}
                                         订单已退款,<input type="button" onclick="orderfin_express('${Id}','${Proname}','${U_name}','${U_phone}','${U_num}','${Province}:${City}:${Address}:${Code}','${Order_remark}','${Expresscom}','${Expresscode}','${Pno}','${Order_state}')" value="详情"/>
                                    {{/if}}
                                    {{if Order_state==23}}
                                        超时订单，已取消
                                    {{/if}}
                                    {{if Order_state==25}}
                                     订单已过期
                                {{/if}}
                                {{/if}}
                                {{if Server_type==9 }}
                                    {{if Hotelinfo != null}}
                                    {{if Order_state==1}}
                                         等待客户付款
                                    {{/if}}
                                    {{if Order_state==2 || Order_state==4}}
                                         <input type="button" onclick="hotelfin('${Id}','${Proname}','${ChangeDateFormat(Hotelinfo.Start_date)}','${ChangeDateFormat(Hotelinfo.End_date)}','${U_name}','${U_phone}','${Pay_state}')" style="color:#ff0000"  value="订房处理"/>
                                         <input type="button" onclick="backticket_ch('退订申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${Source_type}','${Unuse_Ticket}','${Unuse_Ticket}')"  value="退订"/>
                                         <input type="button" onclick="orderinfo('${Id}','${Proname}','${U_name}','${U_phone}','入住:${ChangeDateFormat(Hotelinfo.Start_date)} 离店：${ChangeDateFormat(Hotelinfo.End_date)}（${U_num}间，${Hotelinfo.Bookdaynum}天）','${Order_remark}','${RecerceSMSpeople}','${bookpro_bindphone}','${bookpro_bindname}','${bookpro_bindcompany}','${jsonDateFormatKaler(bookpro_bindconfirmtime)}')" value="详情"/>
                                    {{/if}}
                                    {{if Order_state==8}}
                                         订单已处理<input type="button" onclick="orderinfo('${Id}','${Proname}','${U_name}','${U_phone}','入住:${ChangeDateFormat(Hotelinfo.Start_date)}离店：${ChangeDateFormat(Hotelinfo.End_date)}（${U_num}间，${Hotelinfo.Bookdaynum}天）','${Order_remark}','${RecerceSMSpeople}','${bookpro_bindphone}','${bookpro_bindname}','${bookpro_bindcompany}','${jsonDateFormatKaler(bookpro_bindconfirmtime)}')" value="详情"/>
                                    {{/if}}
                                    {{if Order_state==16}}
                                         订单已退款<input type="button" onclick="orderinfo('${Id}','${Proname}','${U_name}','${U_phone}','入住:${ChangeDateFormat(Hotelinfo.Start_date)}离店：${ChangeDateFormat(Hotelinfo.End_date)}（${U_num}间，${Hotelinfo.Bookdaynum}天）','${Order_remark}','${RecerceSMSpeople}','${bookpro_bindphone}','${bookpro_bindname}','${bookpro_bindcompany}','${jsonDateFormatKaler(bookpro_bindconfirmtime)}')" value="详情"/>
                                    {{/if}}
                                    {{if Order_state==22}}
                                        房间已确认<input type="button" onclick="orderinfo('${Id}','${Proname}','${U_name}','${U_phone}','入住:${ChangeDateFormat(Hotelinfo.Start_date)}离店：${ChangeDateFormat(Hotelinfo.End_date)}（${U_num}间，${Hotelinfo.Bookdaynum}天）','${Order_remark}','${RecerceSMSpeople}','${bookpro_bindphone}','${bookpro_bindname}','${bookpro_bindcompany}','${jsonDateFormatKaler(bookpro_bindconfirmtime)}')" value="详情"/>
                                        <input type="button" onclick="backticket_ch('退订申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${Source_type}','${Unuse_Ticket}','${Unuse_Ticket}')"  value="退订"/>
                                    {{/if}}
                                    {{if Order_state==23}}
                                        超时订单，已取消
                                    {{/if}}
                                    {{if Order_state==24}}
                                        房满已取消
                                    {{/if}}
                                    {{if Order_state==25}}
                                     订单已过期
                                {{/if}}
                                    {{else}}
                                        订房信息错误
                                    {{/if}}
                                {{/if}}

                           {{else}}
                         {{if Server_type==2||Server_type==8}}
                                {{if Order_state==1}}
                                     等待客户付款
                                {{/if}}
                                {{if Order_state==2}} 
                                     订单已付款(短信发送失败) <input type="button" onclick="orderfin_lvyou('${Id}','${Proname}','${U_name}','${U_phone}','${U_num}','${Order_remark}','${Order_state}')" style="color:#ff0000"  value="立即处理"/> 
                                     <input type="button" onclick="backticket_ch('退订申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${Source_type}','${Unuse_Ticket}','${Unuse_Ticket}')"  value="退订"/> 
                                     <input type="button" onclick="restticketsms('${Id}')"  value="重发"/>
                                {{/if}}
                                {{if Order_state==4}} 
                                      订单已付款 <input type="button" onclick="orderfin_lvyou('${Id}','${Proname}','${U_name}','${U_phone}','${U_num}','${Order_remark}','${Order_state}')" style="color:#ff0000"  value="立即处理"/>  <input type="button" onclick="backticket_ch('退订申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${Source_type}','${Unuse_Ticket}','${Unuse_Ticket}')"  value="退订"/>  
                                {{/if}}
                                 {{if Order_state==22}} 
                                 订单已处理
                                 {{/if}}
                                 {{if Order_state==23}} 
                                 超时订单
                                 {{/if}}
                                {{if Order_state==16}}
                                     订单已退款,<input type="button" onclick="orderfin_lvyou('${Id}','${Proname}','${U_name}','${U_phone}','${U_num}','${Order_remark}','${Order_state}','detail')" value="详情"/>
                                {{/if}}
                                {{if Order_state==25}}
                                     订单已过期
                                {{/if}}
                         {{else}}


                            {{if Order_state==1}}
                                {{if Server_type==3}}
                                    旅游优惠券
                                {{else}}
                                    {{if Server_type!=1 }}
                                        <input type="button" onclick="orderfin('${Id}')" style="color:#ff0000"  value="标注已处理"/>
                                    {{else}}
                                        {{if Order_type==1}}
                                             {{if Warrant_type==1}}
                                                 {{if Agentid=="0"}}
                                                     等待对方付款
                                                 {{else}}
                                                     分销购票失败
                                                 {{/if}}
                                             {{else}}
                                                 <input type="button" onclick="agentwarrant_ch('${Agent_company} 分销倒码确认','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}')"  value="分销倒码, 待确认"/>
                                             {{/if}}
                                        {{else}}
                                            充值等待支付
                                        {{/if}}
                                    {{/if}}
                                {{/if}}
                            {{/if}}

                            {{if Order_state==2}}
                                ${Order_state_str}
                                {{if Order_type==1}}
                                     <input type="button" onclick="sendticketsms('${Id}')" style="color:#ff0000"  value="立即发码"/>(可能码原不足)
                                {{else}}
                                     （充值失败，请手工为此会员充值）
                                {{/if}}
                            {{/if}}
                            {{if Order_state==4}}
                                
                                 {{if Warrant_type==1}}
                                     <input type="button" onclick="restticketsms('${Id}')"  value="重发"/> 
                                       
                                     <input type="button" onclick="referrer_ch('${U_name}(${U_phone})',' ${Proname}','${Order_eticket_code}')"  value="短信"/>
                                     
                                     <input type="button" onclick="referrer_ch('查看本订单支付详情','','${Pay_str}')"  value="支付"/>
                                     <input type="button" onclick="backticket_ch('退票申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${Source_type}','${U_num}')"  value="退票"/>
                                     
                                           {{if Server_type!=1 }}
                                             <input type="button" onclick="ChangeTraveldate_ch('改期','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${Source_type}','${U_num}','${Server_type}','${U_traveldate}')"  value="改期"/>
                                           {{/if}}


                                           {{if yiguoqi ==1}}
                                           {{if Comid==106}}
                                            <input type="button" onclick="guoqi_biaozhu('${Id}')"  value="过期"/>
                                            {{/if}}
                                           {{/if}}
                                 {{else}}
                                     <a href="/Agent/DownExcel.aspx?comid=${Comid}&agentid=${Agentid}&orderid=${Id}&md5info=${Returnmd5}" style="font-size: 14px; font-weight: bold;color:#333933">下载电子码</a>
                                 {{/if}}
                            {{/if}}
                            {{if Order_state==6}}
                            已充值
                            {{/if}}
                            {{if Order_state==8}}
                            已消费
                             {{if Unuse_Ticket>0}}
                            <input type="button" onclick="backticket_ch('退票申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${Source_type}','${Unuse_Ticket}','${Unuse_Ticket}')"  value="退票"/>
                            {{/if}}
                                {{if yiguoqi ==1}}
                                           {{if Comid==106}}
                                            <input type="button" onclick="guoqi_biaozhu('${Id}')"  value="过期"/>
                                            {{/if}}
                                {{/if}}
                            {{/if}}
                            {{if Order_state==16}}
                                {{if U_num>Cancelnum}}
                                    部分退票 ${Cancelnum}张
                                {{else}}
                                    订单退票
                                {{/if}}
                            {{/if}}
                            {{if Order_state==17}}
                            申请退票中
                            {{/if}}
                            {{if Order_state==18}}
                            退票处理中
                            {{/if}}
                            {{if Order_state==19}}
                            作废
                            {{/if}}
                            {{if Order_state==20}}
                            发码出错
                            {{/if}}
                            {{if Order_state==21}}
                            重新发码出错
                            {{/if}}
                            {{if Order_state==22}}
                            已处理  <input type="button" onclick="backticket_ch('退票申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${Source_type}','${U_num}')"  value="退票"/>
                            {{/if}}
                            {{if Order_state==23}}
                            超时订单，已取消
                            {{/if}}
                            {{if Order_state==24}}
                                        取消
                                    {{/if}}
                            {{if Order_state==25}}
                                     订单已过期
                            {{/if}}

                            {{/if}}
                           {{/if}}
                       {{else}}

                          
                            {{if BindingOrder != null}}
                            {{if Server_type==11}}
                                {{if BindingOrder.Order_state==1}}
                                     等待客户付款
                                {{/if}}
                                {{if BindingOrder.Order_state==2}}
                                     订单已付款，等待确认发货
                                {{/if}}
                                {{if BindingOrder.Order_state==4}}
                                    {{if BindingOrder.Deliverytype==4}}
                                     自提，已发码(${BindingOrder.Pno})  
                                    {{else}}
                                     订单已发货 ${BindingOrder.Expresscom}:${BindingOrder.Expresscode}
                                    {{/if}}
                                {{/if}}
                                {{if BindingOrder.Order_state==8}}
                                     订单已提货  ${BindingOrder.Expresscom}:${BindingOrder.Expresscode}
                                {{/if}}
                                {{if BindingOrder.Order_state==16}}
                                     订单已退款 
                                {{/if}}

                                {{if BindingOrder.Order_state==25}}
                                     订单已过期
                                {{/if}}
                           {{else}}



                                {{if BindingOrder.Order_state==1}}
                                    {{if BindingOrder.Server_type==3}}
                                        旅游优惠券
                                    {{else}}
                                        {{if BindingOrder.Server_type!=1 }}
                                            <input type="button" onclick="orderfin('${Id}')" style="color:#ff0000"  value="标注已处理"/>
                                        {{else}}
                                            {{if BindingOrder.Order_type==1}}
                                                 {{if BindingOrder.Warrant_type==1}}
                                                     {{if BindingOrder.Agentid=="0"}}
                                                         等待对方付款
                                                     {{else}}
                                                         分销购票失败
                                                     {{/if}}
                                                 {{else}}
                                                     <input type="button" onclick="agentwarrant_ch('${Agent_company} 分销倒码确认','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}')"  value="分销倒码, 待确认"/>
                                                 {{/if}}
                                            {{else}}
                                                充值等待支付
                                            {{/if}}
                                        {{/if}}
                                    {{/if}}
                                {{/if}}

                                {{if BindingOrder.Order_state==2}}
                                    ${BindingOrder.Order_state_str}
                                    {{if BindingOrder.Order_type==1}}
                                     {{if Server_type==9}}

                                        订单提交成功，等待酒店确认
                                    {{else}}
                                     <input type="button" onclick="sendticketsms('${Id}')" style="color:#ff0000"  value="立即发码"/>(可能码原不足.)
                                    
                                    {{/if}}
                                    {{else}}
                                         （充值失败，请手工为此会员充值）
                                    {{/if}}
                                     <input type="button" onclick="backticket_ch('退票申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${Source_type}','${Unuse_Ticket}','${Unuse_Ticket}')"  value="退票"/>
                                {{/if}}
                                {{if BindingOrder.Order_state==4}}
                                
                                     {{if Warrant_type==1}}
                                         <input type="button" onclick="restticketsms('${Id}')"  value="重发"/> 
                                         
                                         <input type="button" onclick="referrer_ch('${U_name}(${U_phone})',' ${Proname}','${Order_eticket_code}')"  value="短信"/>
                                          
                                         <input type="button" onclick="referrer_ch('查看本订单支付详情','','${Pay_str}')"  value="支付"/>
                                         <input type="button" onclick="backticket_ch('退票申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${Source_type}','${U_num}')"  value="退票"/>
                                     {{else}}
                                         <a href="/Agent/DownExcel.aspx?comid=${Comid}&agentid=${Agentid}&orderid=${Id}&md5info=${Returnmd5}" style="font-size: 14px; font-weight: bold;color:#333933">下载电子码</a>
                                     {{/if}}
                                {{/if}}
                                {{if BindingOrder.Order_state==6}}
                                  已充值
                                {{/if}}
                                {{if BindingOrder.Order_state==8}}
                                  已消费
                                {{if Unuse_Ticket>0}}
                                    <input type="button" onclick="backticket_ch('退票申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${Source_type}','${Unuse_Ticket}')"  value="退票"/>
                                {{/if}}
                               {{/if}}
                                {{if BindingOrder.Order_state==16}}
                                    {{if BindingOrder.U_num>Cancelnum}}
                                        部分退票 ${Cancelnum}张
                                    {{else}}
                                        订单退票
                                    {{/if}}
                                {{/if}}
                                {{if BindingOrder.Order_state==17}}
                                申请退票中
                                {{/if}}
                                {{if BindingOrder.Order_state==18}}
                                退票处理中
                                {{/if}}
                                {{if BindingOrder.Order_state==19}}
                                作废
                                {{/if}}
                                {{if BindingOrder.Order_state==20}}
                                发码出错
                                {{/if}}
                                {{if BindingOrder.Order_state==21}}
                                重新发码出错
                                {{/if}}
                                {{if BindingOrder.Order_state==22}}
                                已处理<input type="button" onclick="backticket_ch('退票申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${Source_type}','${U_num}')"  value="退票"/>
                                {{/if}}
                                {{if BindingOrder.Order_state==23}}
                                超时订单，已取消
                                {{/if}}
                                {{if BindingOrder.Order_state==25}}
                                     订单已过期
                                {{/if}}
                            {{/if}}

                            {{else}}

                            失败订单，商家预付款不足
                            {{/if}}
                            {{/if}}

                              {{/if}}
                            {{if  OrderType_Hzins==1}}<!--慧择网订单 但没有生成真实保险订单-->
                                 <span id="span_hzinsorderstaus">${Order_state_str}</span>

                                 {{if Pay_state==2}}
                                   {{if Order_state==2||Order_state==20}}<!--订单状态是已付款或者发码出错的可以退保-->
                                 <input type="button" id="back_hzinsorderstaus${Id}" onclick="backticket_ch('退票申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${Source_type}','${U_num}')"  value="退保"/> 
                                   {{/if}}
                                 {{/if}} 

                                  {{if Order_remark!=''}}
                                       {{if isNaN(Order_remark)}}
                                      <input type="button" onclick="showremark('${Order_remark}')"  value="备注"/>                         {{/if}}
                                  {{/if}}
                            {{/if}}

                            {{if  OrderType_Hzins==2}}<!--慧择网订单 并且已经生成真实保险订单-->
                                 <span id="span_${InsureNum}"></span>
                                  
                                  {{if Order_state==4}}
                                 <input type="button" id="back_${InsureNum}" onclick="backticket_ch('退票申请','${Id}','${Pro_id}','${Proname}','${U_num}','${Totalcount}','${Source_type}','${U_num}')"  value="退保" style="display:none;"/>            
                                 {{/if}}
                                 
                                 {{if Bindingagentorderid>0}}
                                  <a href="/agent/Hzins_detail.aspx?orderid=${Bindingagentorderid}" target="_blank" style="text-decoration: underline">保单详情</a>
                                   <!--保存生成真实保险订单的 本系统订单号-->
                                  <input type="hidden" name="hid_hzinsorderid" value="${Bindingagentorderid}">   
                                 {{else}}
                                  <a href="/agent/Hzins_detail.aspx?orderid=${Id}" target="_blank" style="text-decoration: underline">保单详情</a>   
                                   <!--保存生成真实保险订单的 本系统订单号-->
                                  <input type="hidden" name="hid_hzinsorderid" value="${Id}">
                                 {{/if}}  
                                 
                            {{/if}}
                                </p>
                        </td>
                    </tr>
    </script>
        <div id="hotelcon" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 450px; height: 200px;; display: none; left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 10px;">
            <tr>
                <td height="30" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    房间类型：<span id="Proname"></span>
                </td>
            </tr>
            <tr>
                <td height="30" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    入住时间：<span id="Start_date"></span> 离店时间：<span id="End_date"></span>
                </td>
            </tr>
            <tr>
                <td height="30" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    入住姓名：<span id="U_name"></span> 手机：<span id="U_phone"></span> 
                </td>
            </tr>
            <tr>
                <td height="30" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    <label>
                        <input type="radio" name="roomstate" value="1" />
                        <span  id="entyes">确认房已预订成功</span></label>
                    <label>
                        <input type="radio"   name="roomstate" value="0" />
                        <span id="entno">无房作废</span></label>
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input id="Enterhotelcon" name="Enterhotelcon" type="button" class="formButton" value="  提  交  " />
                    <input name="cancel_rh" type="button" class="formButton" onclick="closehotelcon();" value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>

    <div id="rhshow" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px auto;
        width: 400px; height: 130px; display: none; left: 20%; position: absolute; top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 12px;" id="span_rh"></span>
                </td>
            </tr>
            <tr>
                <td height="60" align="right" bgcolor="#E7F0FA" class="tdHead">
                </td>
                <td height="60" bgcolor="#E7F0FA" class="tdHead" id="tdgroups">
                    <span id="smstext" style="font-size: 14px;"></span>
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input name="cancel_rh" id="cancel_rh" type="button" class="formButton" onclick="cancel();"
                        value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>
    <div id="showticket" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: auto; height: auto; display: none; left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 12px; font-weight: bold" id="span_ticket">
                    </span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    产 品：<span id="pro_name"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    数 量：<span id="pro_num"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    价 格：<span id="span_price"></span>
                </td>
            </tr>
            <tr  id="tr_tnum">
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    退 量：<input type="text" id="tnum" value="" style="width: 30px;" />*按实际未使用的数量退票
                </td>
            </tr>
            <tr style="display:none;" id="tr_ktraveldate">
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    乘坐日期： <input name="pro_end" type="text" id="ktraveldate"  value="" size="12" >
                    <input type="hidden" id="hid_oldtraveldate"  value="" />
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    备 注：<textarea id="testpro" cols="35" rows="2" style="margin-right: 10px;"></textarea>
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input id="Enter" name="Enter" type="button" class="formButton" value="  提  交  " />
                    <input id="ChangeTraveldata"   type="button" class="formButton" value="  提  交  " style="display:none;" />
                    <input name="cancel_rh" type="button" class="formButton" onclick="cancel();" value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>
    <div id="showwarrant" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: auto; height: auto; display: none; left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 12px; font-weight: bold" id="w_span_ticket">
                    </span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    产 品：<span id="w_pro_name"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    倒码数量量：<span id="w_num"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    价 格：<span id="w_price"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    备 注：<textarea id="w_testpro" cols="35" rows="2" style="margin-right: 10px;"></textarea>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    <label>
                        <input type="radio" name="confirmstate" value="1" checked="checked" />
                        生成电子码</label>
                    <label>
                        <input type="radio" name="confirmstate" value="0" />
                        作废此笔订单</label>
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input id="w_Enter" name="w_Enter" type="button" class="formButton" value="  确   认  " />
                    <input name="cancel_rh" type="button" class="formButton" onclick="w_cancel();" value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>
    <div id="express" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px auto;
        width: 400px; height: auto; display: none; left: 20%; position: absolute; top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    产 品：<span id="express_proname"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    订购数量：<span id="express_num"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    姓名：<span id="express_name"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    电话：<span id="express_phone"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    地址：<span id="express_address"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    电子凭证：<span id="expresspno"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    快递单位：<input type="text" id="expresscom" value="" style="width: 120px;" />
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    快递编号：<input type="text" id="expresscode" value="" style="width: 120px;" />
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    备 注：<textarea id="expresstext" cols="35" rows="2" style="margin-right: 10px;"></textarea>
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input id="express_sub" name="express_sub" type="button" class="formButton" value="  确认发货  " />
                    <input name="cancel_express" type="button" class="formButton" id="cancel_express"
                        value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>
    <div id="expresscart" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 500px; height: auto; display: none; left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="100%" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    <table width="400px" id="prolist" class="grid" style="padding: 5px;">
                    </table>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    姓名：<span id="express_cart_name"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    电话：<span id="express_cart_phone"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    地址：<span id="express_cart_address"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    电子凭证：<span id="expresspno_cart"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    快递单位：<input type="text" id="expresscom_cart" value="" style="width: 120px;" />
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    快递编号：<input type="text" id="expresscode_cart" value="" style="width: 120px;" />
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    备 注：<textarea id="expresstext_cart" cols="35" rows="2" style="margin-right: 10px;"></textarea>
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input id="express_cart_sub" name="express_cart_sub" type="button" class="formButton"
                        value="  确认发货  " />
                    <input name="cancel_cart_express" type="button" class="formButton" id="cancel_cart_express"
                        value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>
    <div id="divlvyou" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px auto;
        width: 400px; height: auto; display: none; left: 20%; position: absolute; top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    产 品：<span id="Span1"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    订购数量：<span id="Span2"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    姓名：<span id="Span3"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    电话：<span id="Span4"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#E7F0FA" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    备 注：<textarea id="Textarea1" cols="35" rows="2" style="margin-right: 10px;"></textarea>
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input id="Button1" type="button" class="formButton" value="  立即处理  " />
                    <input id="Button2" type="button" class="formButton" value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>

    <div id="orderinfo" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px auto;
        width: 400px; height: auto; display: none; left: 20%; position: absolute; top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                   订单号：<span id="orderinfo_id"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    产品名称：<span id="orderinfo_proname"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    出游、入住人：<span id="orderinfo_name"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    出游/入住日期：<span id="orderinfo_date"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                   预约绑定人：<span id="RecerceSMSpeople"></span>
                </td>
            </tr>
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    备注：<span id="orderinfo_text"></span>
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                   
                    <input name="cancel_express" type="button" onclick="$('#orderinfo').hide();"
                        value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>

    <input type="hidden" id="hid_id" value="0" />
    <input type="hidden" id="hid_proid" value="0" />
    <input type="hidden" id="hid_paystate" value="" />
</asp:Content>
