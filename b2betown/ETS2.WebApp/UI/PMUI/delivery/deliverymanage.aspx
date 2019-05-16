<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="deliverymanage.aspx.cs"
    MasterPageFile="/UI/Etown.Master" Inherits="ETS2.WebApp.UI.PMUI.delivery.deliverymanage" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        //去除数组中重复值
        function getNoRepeat(s) {
            return s.sort().join(",,").replace(/(,|^)([^,]+)(,,\2)+(,|$)/g, "$1$2$4").replace(/,,+/g, ",").replace(/,$/, "").split(",");
        }

        //判断是为否为正整数
        function CheckInt(obj) {
            var pattern = /^[1-9]\d*|0$/; //匹配非负整数
            obj = obj.replace(/[^\d]/g, "");
            if (!pattern.test(obj)) {
                obj = "";
                return false;
            }
        }

        //带小数点
        function CheckFloat(obj) {

            var pattern = /^[1-9]\d*|$/; //匹配非负整数
            //先把非数字的都替换掉，除了数字和.
            obj = obj.replace(/[^\d.]/g, "");
            //必须保证第一个为数字而不是.
            obj = obj.replace(/^\./g, "");
            //保证只有出现一个.而没有多个.
            obj = obj.replace(/\.{2,}/g, ".");
            //保证.只出现一次，而不能出现两次以上
            obj = obj.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
            if (!pattern.test(obj)) {
                obj = "";
                return false;
            }
        }

        $(function () {
            var windowWidth = document.documentElement.clientWidth;
            var windowHeight = document.documentElement.clientHeight;
            var popupHeight = $("#popupContact").height();
            var popupWidth = $("#popupContact").width();

            $("input:checkbox['.js_delivery_type']").removeAttr("checked");

            var tmpid = $("#hid_deliverytmpid").trimVal();
            if (tmpid > 0)//编辑操作
            {
                $.post("/JsonFactory/ProductHandler.ashx?oper=getdeliverytmp", { tmpid: tmpid }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt(data.msg)
                    }
                    if (data.type == 100) {
                        $("#ComputedPriceMethod").val(data.msg.ComputedPriceMethod);
                        $("#name").val(data.msg.tmpname);
                        $("#hid_js_edit_area_index").val(data.totalcount - 1);
                        var extypes = data.msg.extypes;

                        var extypes_arr = extypes.split(',');
                        for (var i = 0; i < extypes_arr.length; i++) {
                            if (extypes_arr[i] != "") {

                                $("input:checkbox['.js_delivery_type'][value=" + extypes_arr[i] + "]").attr("checked", "checked");
                                $("#js_delivery_item_" + extypes_arr[i]).show();
                            }
                        }

                        //保存到隐藏域中的城市名
                        var delivery1_js_areas = "";
                        var delivery2_js_areas = "";
                        var delivery3_js_areas = "";

                        for (var i = 0; i < data.tmpcostlist.length; i++) {
                            if (data.tmpcostlist[i].Deftype == 1) {

                                $("#js_delivery_item_" + data.tmpcostlist[i].Extype).find(".js_normal_area").find("[name='startstandards']").val(data.tmpcostlist[i].First_num);
                                $("#js_delivery_item_" + data.tmpcostlist[i].Extype).find(".js_normal_area").find("[name='startfees']").val(data.tmpcostlist[i].First_price);
                                $("#js_delivery_item_" + data.tmpcostlist[i].Extype).find(".js_normal_area").find("[name='addstandards']").val(data.tmpcostlist[i].Con_num);
                                $("#js_delivery_item_" + data.tmpcostlist[i].Extype).find(".js_normal_area").find("[name='addfees']").val(data.tmpcostlist[i].Con_price);
                            }
                            else {


                                var js_custom_areastr = '<li class="tbody group js_custom_area">' +
                                        '<div class="table_cell edit_range l">' +
                                            '<div class="td_panel group">' +
                                                '<div class="group">' +
                                                    '<div class="text">' +
                                                        '<p class="">' +
                                                            '指定地区<a class="js_edit_area" href="javascript:;" data-index="' + i + '">编辑</a><a class="js_delete_area" data-x="-137"  href="javascript:;">删除</a><span span-index="' + i + '"></span>' +
                                                        '</p>' +
                                                        '<p class="area">';


                                var citystr_arr = data.tmpcostlist[i].City.split(',');
                                var provincestr_arr = data.tmpcostlist[i].Province.split(',');
                                for (var j = 0; j < citystr_arr.length; j++) {
                                    if (citystr_arr[j] != "") {
                                        var city_arr = citystr_arr[j].split(';');
                                        var province_arr = provincestr_arr[j].split(';');
                                        for (var q = 0; q < city_arr.length; q++) {
                                            if (city_arr[q] != '') {
                                                js_custom_areastr += '<span data-destcountry="中国" data-destprovince="' + province_arr[q] + '" data-destcity="' + city_arr[q] + '" class="js_area_item">' + city_arr[q] + '</span>';
                                                if (data.tmpcostlist[i].Extype == 1) {
                                                    delivery1_js_areas += city_arr[q] + ",";
                                                }

                                                if (data.tmpcostlist[i].Extype == 2) {
                                                    delivery2_js_areas += city_arr[q] + ",";
                                                }
                                                if (data.tmpcostlist[i].Extype == 3) {
                                                    delivery3_js_areas += city_arr[q] + ",";
                                                }
                                            }
                                        }

                                    }
                                }

                                js_custom_areastr += '</p>' +
                                                    '</div>' +
                                                '</div>' +
                                            '</div>' +
                                        '</div>' +
                                        '<div class="group input_group">' +
                                            '<div class="table_cell edit_f_num l tl">' +
                                                '<div class="td_panel">' +
                                                   ' <span class="frm_input_box">' +
                                                       ' <input value="' + data.tmpcostlist[i].First_num + '" name="startstandards" class="startstandards frm_input" type="text"  placeholder="">  </span>' +
                                                '</div>' +
                                           ' </div>' +
                                           ' <div class="table_cell edit_f_price l tr">' +
                                                '<div class="td_panel">' +
                                                   ' <span class="frm_input_box">' +
                                                       ' <input value="' + data.tmpcostlist[i].First_price + '" name="startfees" class="startfees frm_input" type="text" placeholder="">' +
                                                   ' </span>' +
                                               ' </div>' +
                                            '</div>' +
                                           ' <div class="table_cell edit_l_num l tl">' +
                                               ' <div class="td_panel">' +
                                                    '<span class="frm_input_box">' +
                                                       ' <input value="' + data.tmpcostlist[i].Con_num + '"  name="addstandards" class="addstandards frm_input" type="text" placeholder="">' +
                                                   ' </span>' +
                                               ' </div>' +
                                            '</div>' +
                                           ' <div class="table_cell edit_l_price l tr">' +
                                               ' <div class="td_panel">' +
                                                    '<span class="frm_input_box">' +
                                                        '<input value="' + data.tmpcostlist[i].Con_price + '"  name="addfees" class="addfees frm_input" type="text" placeholder="">' +
                                                   ' </span>' +
                                                '</div>' +
                                           ' </div>' +
                                           ' <p class="frm_tips fail dn">' +
                                               ' 件数需为0至99的整数，费用应为0.00至999.99的数字</p>' +
                                        '</div>' +
                                    '</li>';


                                var initprovincearr = getNoRepeat(provincestr_arr[0].split(';'));

                                js_custom_areastr = js_custom_areastr.replace('<span span-index="' + i + '"></span>', '<span span-index="' + i + '">' + initprovincearr.toString() + '</span>');

                                $("#js_delivery_item_" + data.tmpcostlist[i].Extype).find(".js_add_area").parent().parent().parent().before(js_custom_areastr);


                            }

                        }
                        delivery1_js_areas = delivery1_js_areas.substr(0, delivery1_js_areas.length - 1);
                        delivery2_js_areas = delivery2_js_areas.substr(0, delivery2_js_areas.length - 1);
                        delivery3_js_areas = delivery3_js_areas.substr(0, delivery3_js_areas.length - 1);

                        $("#hid_delivery1_js_area").val(delivery1_js_areas);
                        $("#hid_delivery2_js_area").val(delivery2_js_areas);
                        $("#hid_delivery3_js_area").val(delivery3_js_areas);
                    }
                });
            }


            $(".js_edit_area").live("click", function () {
                $("#sub_rh").attr("data-index", $(this).attr("data-index"));

                //显示指定地区弹窗窗
                letDivCenter("#div_agentmsgset");

                //                $(".mask").show();

                //判断属于的配送方式：1平邮    2快递   3EMS
                if ($(this).parents("#js_delivery_item_1").length > 0) {
                    $("#sub_rh").attr("data-delivery-index", 1);
                }
                if ($(this).parents("#js_delivery_item_2").length > 0) {
                    $("#sub_rh").attr("data-delivery-index", 2);
                }
                if ($(this).parents("#js_delivery_item_3").length > 0) {
                    $("#sub_rh").attr("data-delivery-index", 3);
                }
            })

            $("#com_city").click(function () {
                if ($(this).val() == "城市") {
                    $("#com_city option").each(function () {
                        $(this).attr("selected", "selected");
                    })
                }
            })
            //选择指定地区
            $("#sub_rh").live("click", function () {
                var datadeliveryindex = $(this).attr("data-delivery-index");
                var dataindex = $(this).attr("data-index");

                //原始指定地区
                var initarea = $("#hid_delivery" + datadeliveryindex + "_js_area").val();
                //新增指定地区
                var strvalue = "";

                //原始指定地区的html
                var initjs_areahtml = $("a[data-index='" + dataindex + "']").parent().next().html();
                //新增指定地区的html
                var newjs_areahtml = "";

                //判断循环是否出错
                var eacherr = 0;
                $("#com_city option:selected").each(function () {
                    var initarea_str = initarea.split(',');

                    //判断选择的地区是否已经指定过
                    if (initarea_str.indexOf($(this).val()) > -1) {
                        alert($(this).val() + "已经指定过，不可重复指定");
                        eacherr = 1;
                        return false;
                    }
                    //过滤掉默认项"城市"
                    if ($(this).val() != "城市") {
                        var com_province = $("#com_province").val();
                        //新增指定地区html
                        newjs_areahtml += '<span data-destcountry="中国"   data-destprovince="' + com_province + '"  data-destcity="' + $(this).val() + '" class="js_area_item">' + $(this).val() + '</span>';

                        strvalue += $(this).val() + ",";
                    }
                })
                //选择指定地区出现了错误，不在执行以下程序
                if (eacherr == 1) {
                    return;
                }

                if (strvalue.length > 0) {
                    strvalue = strvalue.substr(0, strvalue.length - 1);
                    if (initarea == "") {
                        $("#hid_delivery" + datadeliveryindex + "_js_area").val(strvalue);
                    }
                    else {
                        $("#hid_delivery" + datadeliveryindex + "_js_area").val(initarea + "," + strvalue);
                    }

                    if (initjs_areahtml == '<span class="js_no_area">未选择任何区域</span>' || initjs_areahtml == '<span class="js_no_area" style="color: rgb(225, 95, 99);">未选择任何区域</span>' || initjs_areahtml == '<span style="color: rgb(225, 95, 99);" class="js_no_area">未选择任何区域</span>') {

                        $("a[data-index='" + dataindex + "']").parent().next().html(newjs_areahtml);
                    } else {

                        $("a[data-index='" + dataindex + "']").parent().next().html(initjs_areahtml + newjs_areahtml);
                    }

                    var initprovince = $("span[span-index='" + dataindex + "']").text();
                    if ($("span[span-index='" + dataindex + "']").text() == "") {
                        initprovince = $("#com_province").val();
                    } else {
                        initprovince += "," + $("#com_province").val();
                    }
                    var initprovincearr = getNoRepeat(initprovince.split(','));

                    $("span[span-index='" + dataindex + "']").text(initprovincearr.toString());

                    //隐藏指定地区弹窗窗
                    $("#div_agentmsgset").hide();
                    $(".mask").hide();

                } else {
                    alert("请至少选择一项.");
                }
            })
            $("#cancel_rh").click(function () {
                $("#div_agentmsgset").hide();
                $(".mask").hide();
            })

            $("input:checkbox['.js_delivery_type']").click(function () {

                //获取配送方式
                var deliverytypes = "";
                $("input:checkbox['.js_delivery_type']:checked").each(function () {
                    deliverytypes += $(this).val() + ",";
                })
                if (deliverytypes == "") {
                    $(".js_delivery_type_fail").show();
                } else {
                    $(".js_delivery_type_fail").hide();
                }

                $("input:checkbox['.js_delivery_type']").each(function () {

                    if ($(this).attr("checked") == "checked") {
                        $("#js_delivery_item_" + $(this).val()).show();
                    } else {
                        $("#js_delivery_item_" + $(this).val()).hide();
                    }

                })
            })

            $(".js_add_area").click(function () {
                var editindex = parseInt($("#hid_js_edit_area_index").trimVal()) + 1;
                $("#hid_js_edit_area_index").val(editindex);
                var js_custom_areastr = '<li class="tbody group js_custom_area">' +
                                        '<div class="table_cell edit_range l">' +
                                            '<div class="td_panel group">' +
                                                '<div class="group">' +
                                                    '<div class="text">' +
                                                        '<p class="">' +
                                                            '指定地区<a class="js_edit_area" href="javascript:;" data-index="' + editindex + '">编辑</a><a class="js_delete_area" data-x="-137"  href="javascript:;">删除</a><span span-index="' + editindex + '"></span>' +
                                                        '</p>' +
                                                        '<p class="area">' +
                                                            '<span class="js_no_area">未选择任何区域</span>' +
                                                        '</p>' +
                                                    '</div>' +
                                                '</div>' +
                                            '</div>' +
                                        '</div>' +
                                        '<div class="group input_group">' +
                                            '<div class="table_cell edit_f_num l tl">' +
                                                '<div class="td_panel">' +
                                                   ' <span class="frm_input_box">' +
                                                       ' <input value="" name="startstandards" class="startstandards frm_input" type="text"  placeholder="">  </span>' +
                                                '</div>' +
                                           ' </div>' +
                                           ' <div class="table_cell edit_f_price l tr">' +
                                                '<div class="td_panel">' +
                                                   ' <span class="frm_input_box">' +
                                                       ' <input value="" name="startfees" class="startfees frm_input" type="text" placeholder="">' +
                                                   ' </span>' +
                                               ' </div>' +
                                            '</div>' +
                                           ' <div class="table_cell edit_l_num l tl">' +
                                               ' <div class="td_panel">' +
                                                    '<span class="frm_input_box">' +
                                                       ' <input value="" name="addstandards" class="addstandards frm_input" type="text" placeholder="">' +
                                                   ' </span>' +
                                               ' </div>' +
                                            '</div>' +
                                           ' <div class="table_cell edit_l_price l tr">' +
                                               ' <div class="td_panel">' +
                                                    '<span class="frm_input_box">' +
                                                        '<input value="" name="addfees" class="addfees frm_input" type="text" placeholder="">' +
                                                   ' </span>' +
                                                '</div>' +
                                           ' </div>' +
                                           ' <p class="frm_tips fail dn">' +
                                               ' 件数需为0至99的整数，费用应为0.00至999.99的数字</p>' +
                                        '</div>' +
                                    '</li>';
                $(this).parent().parent().parent().before(js_custom_areastr);
            })

            $(".js_delete_area").live("click", function () {
                //判断是否添加了指定地区
                if ($(this).parent().next().find(".js_no_area").length > 0) {


                    //把隐藏域中保存的指定地区也去掉 判断属于的配送方式：1平邮    2快递   3EMS
                    var deliverytype = 0;
                    if ($(this).parents("#js_delivery_item_1").length > 0) {
                        deliverytype = 1;
                    }
                    if ($(this).parents("#js_delivery_item_2").length > 0) {
                        deliverytype = 2;
                    }
                    if ($(this).parents("#js_delivery_item_3").length > 0) {
                        deliverytype = 3;
                    }
                    var allarea = $("#hid_delivery" + deliverytype + "_js_area").trimVal();
                    $(this).parent().next().find(".js_area_item").each(function () {
                        var join_area = $(this).text();
                        if (allarea.indexOf(join_area) > -1) {
                            allarea = allarea.replace(join_area + ",", "").replace(join_area, "");
                        }
                    })
                    $("#hid_delivery" + deliverytype + "_js_area").val(allarea);

                    $(this).parent().parent().parent().parent().parent().parent().empty();
                } else {
                    if (confirm("确定要删除该指定地区的运费设置吗？")) {

                        //把隐藏域中保存的指定地区也去掉 判断属于的配送方式：1平邮    2快递   3EMS
                        var deliverytype = 0;
                        if ($(this).parents("#js_delivery_item_1").length > 0) {
                            deliverytype = 1;
                        }
                        if ($(this).parents("#js_delivery_item_2").length > 0) {
                            deliverytype = 2;
                        }
                        if ($(this).parents("#js_delivery_item_3").length > 0) {
                            deliverytype = 3;
                        }
                        var allarea = $("#hid_delivery" + deliverytype + "_js_area").trimVal();
                        $(this).parent().next().find(".js_area_item").each(function () {
                            var join_area = $(this).text();
                            if (allarea.indexOf(join_area) > -1) {
                                allarea = allarea.replace(join_area + ",", "").replace(join_area, "");
                            }
                        })
                        $("#hid_delivery" + deliverytype + "_js_area").val(allarea);

                        $(this).parent().parent().parent().parent().parent().parent().empty();
                    }
                }

            })

            //保存按钮
            $("#js_btn_save").click(function () {

                var tmpname = $("#name").trimVal();
                if (tmpname == "") {
                    $(".js_name_fail").next().removeClass("dn");
                    return false;
                } else {
                    $(".js_name_fail").next().addClass("dn");
                }

                //获取配送方式
                var deliverytypes = "";
                $("input:checkbox['.js_delivery_type']:checked").each(function () {
                    deliverytypes += $(this).val() + ",";
                })
                if (deliverytypes != "") {
                    deliverytypes = deliverytypes.substr(0, deliverytypes.length - 1);
                }
                if (deliverytypes == "") {
                    $(".js_delivery_type_fail").show();
                    return false;
                } else {
                    $(".js_delivery_type_fail").hide();
                }

                //配送方式限定必须有快递方式
                if (deliverytypes.indexOf("2") == -1) {
                    $.prompt("配送方式必须含有快递方式")
                    return false;
                }

                var join_deliverytype = ""; //拼接起来的配送方式
                var join_provinces = ""; //拼接起来的省份字符串

                var join_areas = ""; //拼接起来的地区字符串
                var join_startstandards = ""; //拼接起来的首N件
                var join_startfees = ""; //拼接起来的首费
                var join_addstandards = ""; //拼接起来的续M件
                var join_addfees = ""; //拼接起来的续费

                var err1 = 0; //判断循环是否有错误：0无；1有
                $("input:checkbox['.js_delivery_type']:checked").each(function () {
                    var deliverytype = $(this).val(); //配送模式


                    //全国默认地区  
                    $("#js_delivery_item_" + deliverytype).find(".js_normal_area").each(function () {

                        //判断件数，费用输入是否正确
                        var startstandards = $(this).find("[name='startstandards']").val();
                        var startfees = $(this).find("[name='startfees']").val();
                        var addstandards = $(this).find("[name='addstandards']").val();
                        var addfees = $(this).find("[name='addfees']").val();
                        if (CheckInt(startstandards) == false || CheckInt(addstandards) == false || CheckFloat(startfees) == false || CheckFloat(addfees) == false) {

                            $(this).find(".frm_tips").removeClass("dn");
                            err1 = 1;
                            return false;
                        }

                        join_deliverytype += deliverytype + ",";
                        join_provinces += "默认" + ",";
                        join_areas += "默认" + ",";
                        join_startstandards += startstandards + ",";
                        join_startfees += startfees + ",";
                        join_addstandards += addstandards + ",";
                        join_addfees += addfees + ",";

                    })

                    //指定地区 
                    $("#js_delivery_item_" + deliverytype).find(".js_custom_area").each(function () {

                        if ($(this).html() != "") {
                            //判断是否指定了地区
                            if ($(this).find(".js_no_area").length > 0) {
                                $(".js_no_area").css("color", "#e15f63");
                                err1 = 1;
                                return false;
                            }
                            //判断件数，费用输入是否正确
                            var startstandards = $(this).find("[name='startstandards']").val();
                            var startfees = $(this).find("[name='startfees']").val();
                            var addstandards = $(this).find("[name='addstandards']").val();
                            var addfees = $(this).find("[name='addfees']").val();

                           
                                 if (CheckInt(startstandards) == false || CheckInt(addstandards) == false || CheckFloat(startfees) == false || CheckFloat(addfees) == false) {
                                    $(".frm_tips .fail").show();
                                    $(this).find(".frm_tips").removeClass("dn");
                                    err1 = 1;
                                    return false;
                                }
                            

                            $(this).find(".js_area_item").each(function () {
                                join_provinces += $(this).attr("data-destprovince") + ";";
                                join_areas += $(this).text() + ";";
                            })
                            join_areas = join_areas.substr(0, join_areas.length - 1);
                            join_provinces = join_provinces.substr(0, join_provinces.length - 1);

                            join_deliverytype += deliverytype + ",";
                            join_provinces += ",";
                            join_areas += ",";
                            join_startstandards += startstandards + ",";
                            join_startfees += startfees + ",";
                            join_addstandards += addstandards + ",";
                            join_addfees += addfees + ",";
                        }
                    })
                })

                if (err1 == 1) {
                    return false;
                }
                join_deliverytype = join_deliverytype.substr(0, join_deliverytype.length - 1);
                join_provinces = join_provinces.substr(0, join_provinces.length - 1);
                join_areas = join_areas.substr(0, join_areas.length - 1);
                join_startstandards = join_startstandards.substr(0, join_startstandards.length - 1);
                join_startfees = join_startfees.substr(0, join_startfees.length - 1);
                join_addstandards = join_addstandards.substr(0, join_addstandards.length - 1);
                join_addfees = join_addfees.substr(0, join_addfees.length - 1);


                $.post("/JsonFactory/ProductHandler.ashx?oper=editdeliverytmp", { ComputedPriceMethod: $("#ComputedPriceMethod").trimVal(), join_provinces: join_provinces, tmpid: $("#hid_deliverytmpid").trimVal(), tmpname: tmpname, deliverytypes: deliverytypes, join_deliverytype: join_deliverytype, join_areas: join_areas, join_startstandards: join_startstandards, join_startfees: join_startfees, join_addstandards: join_addstandards, join_addfees: join_addfees, comid: $("#hid_comid").trimVal(), operor: $("#hid_userid").trimVal() }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt(data.msg)
                    }
                    if (data.type == 100) {
                        $.prompt("运费模板管理处理成功",{
                                buttons: [
                                 { title: '确定', value: true }
                                ],
                                opacity: 0.1,
                                focus: 0,
                                show: 'slideDown',
                                submit: function (e, v, m, f) {
                                    if (v == true)
                                        location.href = "/ui/pmui/delivery/deliverylist.aspx";
                                }
                            });
                        //                        window.location.reload();
                       // window.open("/ui/pmui/delivery/deliverylist.aspx" , target = "_self");

                    }
                })
            })

            $("#js_btn_cancel").click(function () {
                window.open("/ui/pmui/ProductList.aspx", target = "_self");
            })
           

        })
      
        function letDivCenter(divName) {
            var top = ($(window).height() - $(divName).height()) / 2;
            var left = ($(window).width() - $(divName).width()) / 2;
            var scrollTop = $(document).scrollTop();
            var scrollLeft = $(document).scrollLeft();
            $(divName).css({ position: 'absolute', 'top': top + scrollTop, left: left + scrollLeft }).show();
        }  
    </script>
    <style type="text/css">
        .js_area_item
        {
            padding: 0 2px;
        }
        .js_edit_area
        {
            padding: 0 5px;
        }
        .js_delete_area
        {
            padding: 0 5px;
        }
        .popover
        {
            width: 290px;
        }
        .popover
        {
            width: 277px;
            position: absolute;
            margin-top: 12px;
            z-index: 999;
        }
        .popover .popover_inner
        {
            border: 1px solid #d9dadc;
            word-wrap: break-word;
            word-break: break-all;
            padding: 30px;
            background-color: #fff;
            box-shadow: none;
            -moz-box-shadow: none;
            -webkit-box-shadow: none;
        }
        .popover .popover_bar
        {
            text-align: justify;
            font-size: 0;
        }
        .popover .popover_bar
        {
            text-align: center;
            margin-top: 20px;
        }
        .popover .popover_bar .btn
        {
            margin: 0;
        }
        .popover .popover_arrow_out
        {
            top: 0;
        }
        .popover .popover_arrow
        {
            position: absolute;
            left: 50%;
            margin-left: -8px;
            margin-top: -8px;
            display: inline-block;
            width: 0;
            height: 0;
            border-width: 8px;
            border-style: dashed;
            border-color: transparent;
            border-top-width: 0;
            border-bottom-color: #d9dadc;
            border-bottom-style: solid;
        }
        .popover .popover_arrow_in
        {
            border-bottom-color: #fff;
            top: 1px;
        }
        
        .delivery_edit .main_bd
        {
            padding: 20px 30px;
        }
        
        .delivery_edit .frm_control_group
        {
            padding-bottom: 18px;
        }
        
        .frm_label
        {
            float: left;
            width: 5em;
            margin-top: .3em;
            margin-right: 1em;
            font-size: 14px;
        }
        
        .frm_controls
        {
            display: table-cell;
            vertical-align: top;
            float: none;
            width: auto;
        }
        .frm_input_box
        {
            display: inline-block;
            position: relative;
            height: 30px;
            line-height: 30px;
            vertical-align: middle;
            width: 78px;
            font-size: 14px;
            padding: 0 10px;
            border: 1px solid #e7e7eb;
            box-shadow: none;
            -moz-box-shadow: none;
            -webkit-box-shadow: none;
            border-radius: 0;
            -moz-border-radius: 0;
            -webkit-border-radius: 0;
            background-color: #fff;
        }
        
        .main_bd .frm_control_group .frm_tips.fail
        {
            color: #e15f63;
        }
        
        .delivery_edit .frm_control_checkbox .frm_label
        {
            margin-top: 0;
        }
        
        
        .icon_checkbox
        {
            width: 16px;
            height: 16px;
            vertical-align: middle;
            display: inline-block;
            margin-top: -0.2em;
        }
        
        .delivery_edit .table_list
        {
            border: 1px solid #e7e7eb;
            border-bottom: 0;
            margin: 9px 0 0 84px;
        }
        
        
        .thead
        {
            background-color: #f4f5f9;
        }
        
        
        .thead .table_cell
        {
            line-height: 32px;
            border-left: 1px solid #e7e7eb;
            border-bottom: 1px solid #e7e7eb;
        }
        .table_cell
        {
            padding-left: 20px;
            padding-right: 20px;
        }
        .table_cell
        {
            padding: 0;
            font-weight: 400;
            font-style: normal;
        }
        
        .group:after
        {
            content: "\200B";
            display: block;
            height: 0;
            clear: both;
        }
        
        .tbody .table_cell
        {
            padding-top: 9px;
            padding-bottom: 9px;
        }
        .tbody .table_cell
        {
            padding-top: 6px;
            padding-bottom: 6px;
            border-top: 1px solid #e7e7eb;
        }
        .l
        {
            float: left;
        }
        .frm_input
        {
            height: 22px;
            margin: 4px 0;
        }
        .frm_input, .frm_textarea
        {
            width: 100%;
            background-color: transparent;
            border: 0;
            outline: 0;
        }
        
        .frm_controls
        {
            display: table-cell;
            vertical-align: top;
            float: none;
            width: auto;
        }
        .main_bd .frm_control_group .table_list .edit_range
        {
            width: 40%;
        }
        .main_bd .frm_control_group .table_list .edit_f_num
        {
            width: 15%;
        }
        .main_bd .frm_control_group .table_list .edit_f_price
        {
            width: 15%;
        }
        .main_bd .frm_control_group .table_list .edit_l_num
        {
            width: 15%;
        }
        .main_bd .frm_control_group .table_list .edit_l_price
        {
            width: 15%;
        }
        .btn
        {
            min-width: 60px;
        }
        .btn_primary
        {
            background-color: #44b549;
            background-image: -moz-linear-gradient(top,#44b549 0,#44b549 100%);
            background-image: -webkit-gradient(linear,0 0,0 100%,from(#44b549),to(#44b549));
            background-image: -webkit-linear-gradient(top,#44b549 0,#44b549 100%);
            background-image: -o-linear-gradient(top,#44b549 0,#44b549 100%);
            background-image: linear-gradient(to bottom,#44b549 0,#44b549 100%);
            border-color: #44b549;
            color: #fff;
        }
        .btn
        {
            display: inline-block;
            overflow: visible;
            padding: 0 22px;
            height: 30px;
            line-height: 30px;
            vertical-align: middle;
            text-align: center;
            text-decoration: none;
            border-radius: 3px;
            -moz-border-radius: 3px;
            -webkit-border-radius: 3px;
            font-size: 14px;
            border-width: 1px;
            border-style: solid;
            cursor: pointer;
        }
        .btn_default
        {
            background-color: #fff;
            background-image: -moz-linear-gradient(top,#fff 0,#fff 100%);
            background-image: -webkit-gradient(linear,0 0,0 100%,from(#fff),to(#fff));
            background-image: -webkit-linear-gradient(top,#fff 0,#fff 100%);
            background-image: -o-linear-gradient(top,#fff 0,#fff 100%);
            background-image: linear-gradient(to bottom,#fff 0,#fff 100%);
            border-color: #e7e7eb;
            color: #222;
        }
        
        .main_bd .table_list .tbody .input_group .frm_tips.fail
        {
            padding-left: 363px;
            padding-top: 0;
        }
        
        
        
        .dialog_wrp
        {
            z-index: 9999;
        }
        .dialog_wrp
        {
            position: fixed;
            top: 50%;
            left: 50%;
            width: 726px;
            z-index: 3;
        }
        .dialog_wrp
        {
            z-index: 9999;
        }
        .dialog_wrp
        {
            position: fixed;
            top: 50%;
            left: 50%;
            width: 726px;
            z-index: 3;
        }
        .dialog
        {
            border-width: 0;
            overflow: visible;
        }
        .dialog
        {
            overflow: hidden;
            border: 1px solid transparent;
            background-color: #fff;
            border-radius: 0;
            -moz-border-radius: 0;
            -webkit-border-radius: 0;
        }
        .dialog_hd
        {
            line-height: 52px;
            height: 52px;
            border-bottom-width: 0;
        }
        .dialog_hd
        {
            position: relative;
            padding: 0 20px;
            line-height: 38px;
            height: 38px;
            background-color: #f4f5f9;
            background-image: -moz-linear-gradient(top,#f4f5f9 0,#f4f5f9 100%);
            background-image: -webkit-gradient(linear,0 0,0 100%,from(#f4f5f9),to(#f4f5f9));
            background-image: -webkit-linear-gradient(top,#f4f5f9 0,#f4f5f9 100%);
            background-image: -o-linear-gradient(top,#f4f5f9 0,#f4f5f9 100%);
            background-image: linear-gradient(to bottom,#f4f5f9 0,#f4f5f9 100%);
            border-bottom: 1px solid #e7e7eb;
        }
        .dialog_hd h3
        {
            font-weight: 400;
            font-style: normal;
        }
        .dialog_hd h3
        {
            color: #222;
        }
        .pop_closed
        {
            background: url("/images/deliveryimg/base_z231ecc.png") 0 -2757px no-repeat;
        }
        .pop_closed
        {
            position: absolute;
            top: 50%;
            margin-top: -8px;
            right: 20px;
            width: 16px;
            height: 16px;
            line-height: 999em;
            overflow: hidden;
        }
        .dialog_bd
        {
            padding: 66px 45px 108px;
        }
        .area_select_dialog .scope_area
        {
            padding-left: 45px;
        }
        .scope_area
        {
            margin-top: 10px;
            height: 302px;
            overflow: hidden;
        }
        .unchoose_scope, .choosed_scope
        {
            border: 1px solid #e7e7eb;
            float: left;
            zoom: 1;
            height: 300px;
            width: 200px;
            overflow: visible;
            background-color: #fff;
        }
        .scope_hd
        {
            height: 27px;
            line-height: 27px;
            position: relative;
            background-color: #f4f5f9;
            text-align: left;
        }
        .scope_hd label
        {
            display: block;
            cursor: pointer;
            margin-left: 10px;
            cursor: default;
            width: 175px;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
            word-wrap: normal;
            font-size: 14px;
        }
        .scope_list
        {
            height: 275px;
            overflow-y: scroll;
            zoom: 1;
            position: relative;
        }
        .scope_list dd
        {
            margin-top: 1px;
            box-sizing: border-box;
            overflow: hidden;
        }
        .scope_area .frm_msg.fail
        {
            padding-left: 10px;
        }
        
        .frm_msg.fail
        {
            color: #e15f63;
        }
        .frm_msg
        {
            display: none;
            overflow: hidden;
        }
        .frm_tips, .frm_msg
        {
            padding-top: 4px;
            width: 300px;
        }
        .scope_list
        {
            zoom: 1;
        }
        .empty_tips
        {
            padding: 100px 0;
            text-align: center;
            font-size: 14px;
            color: #8d8d8d;
        }
        .dn
        {
            display: none;
        }
        .scope_list dd.first_dd_list
        {
            margin-top: 10px;
        }
        
        .scope_list dd
        {
            margin-top: 1px;
            box-sizing: border-box;
            overflow: hidden;
        }
        .scope_list dd a
        {
            display: block;
            height: 24px;
            line-height: 24px;
            color: #222;
            overflow: hidden;
        }
        .scope_list dd a .sub_icon.show_sub_icon
        {
            background: url("/Images/deliveryimg/areaselector_z218878.png") 0 0 no-repeat;
        }
        .scope_list dd a .sub_icon
        {
            width: 13px;
            height: 13px;
            vertical-align: middle;
            display: inline-block;
            margin-right: 5px;
            text-indent: -999em;
            margin-top: 6px;
            float: left;
            margin-left: 10px;
        }
        .scope_area .item_name
        {
            width: 140px;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
            word-wrap: normal;
            display: block;
        }
        .scope_list dd a
        {
            line-height: 24px;
            color: #222;
        }
        .scope_list .sub_scope_list
        {
            zoom: 1;
            width: 100%;
        }
        .scope_list .sub_scope_list li
        {
            margin-top: 1px;
        }
        .scope_list .sub_scope_list li a
        {
            width: 100%;
            padding-left: 30px;
        }
        .scope_list dd a.disabled
        {
            color: #a3a3a3;
        }
        .opr_btn
        {
            float: left;
            margin: 140px 20px;
        }
        .btn
        {
            min-width: 60px;
        }
        .btn_default
        {
            background-color: #fff;
            background-image: -moz-linear-gradient(top,#fff 0,#fff 100%);
            background-image: -webkit-gradient(linear,0 0,0 100%,from(#fff),to(#fff));
            background-image: -webkit-linear-gradient(top,#fff 0,#fff 100%);
            background-image: -o-linear-gradient(top,#fff 0,#fff 100%);
            background-image: linear-gradient(to bottom,#fff 0,#fff 100%);
            border-color: #e7e7eb;
            color: #222;
        }
        .btn
        {
            display: inline-block;
            overflow: visible;
            padding: 0 22px;
            height: 30px;
            line-height: 30px;
            vertical-align: middle;
            text-align: center;
            text-decoration: none;
            border-radius: 3px;
            -moz-border-radius: 3px;
            -webkit-border-radius: 3px;
            font-size: 14px;
            border-width: 1px;
            border-style: solid;
            cursor: pointer;
        }
        .selected_scope_list
        {
            float: left;
            width: 100%;
            box-sizing: border-box;
            margin-top: 10px;
        }
        .selected_scope_list .selected_scope_item
        {
            padding-left: 10px;
            position: relative;
            height: 24px;
            line-height: 24px;
        }
        
        .selected_scope_list .selected_scope_item .as_scope_del
        {
            text-indent: -999em;
            position: absolute;
            font-size: 16px;
            display: block;
            color: #000;
            top: 5px;
            right: 10px;
            height: 14px;
            width: 15px;
            background: transparent url(/Images/deliveryimg/spr_icon_selector218877.png) no-repeat 0 0;
            background-position: 0 -56px;
        }
        .scope_list dd a
        {
            display: block;
            height: 24px;
            line-height: 24px;
            color: #222;
            overflow: hidden;
        }
        .selected_scope_list .selected_scope_item
        {
            line-height: 24px;
            display: block;
        }
        li
        {
            text-align: -webkit-match-parent;
        }
        .dialog_ft
        {
            padding: 16px 0;
            background-color: #f4f5f9;
        }
        .dialog_ft
        {
            margin: 0;
            padding: 25px 0;
            text-align: center;
            border-top: 1px solid transparent;
            box-shadow: none;
            -moz-box-shadow: none;
            -webkit-box-shadow: none;
        }
        .dialog_ft .btn
        {
            margin-left: .3em;
            margin-right: .3em;
        }
        .btn button
        {
            display: block;
            height: 100%;
            background-color: transparent;
            border: 0;
            outline: 0;
            overflow: visible;
            padding: 0 22px;
        }
        .btn_default button
        {
            color: #222;
        }
        .mask
        {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            filter: alpha(opacity = 75);
            -moz-opacity: .75;
            -khtml-opacity: .75;
            opacity: .75;
            background-color: #000;
            z-index: 0;
        }
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/pmui/projectlist.aspx" onfocus="this.blur()" target=""><span>项目管理</span></a></li>
                <li><a href="/ui/pmui/Projectedit.aspx" onfocus="this.blur()" target=""><span>添加项目</span></a></li>
                <li><a href="/ui/pmui/ProductList.aspx" onfocus="this.blur()" target=""><span>产品列表</span></a></li>
                <li><a href="/ui/pmui/ProductServerTypeList.aspx" onfocus="this.blur()" target=""><span>
                    添加产品</span></a></li>
                <li><a href="/ui/pmui/order/Salecount.aspx" onfocus="this.blur()" target="">产品统计</a></li>
                <li><a href="/ui/pmui/BindingAgent.aspx" onfocus="this.blur()" target="">导入分销系统产品</a></li>
                <li><a href="/ui/pmui/eticket_useset.aspx" onfocus="this.blur()" target=""><span>商户特定日期设定</span></a></li>
                <li class="on"><a href="/ui/pmui/delivery/deliverylist.aspx" onfocus="this.blur()"
                    target=""><span>运费模版管理</span></a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone">
            <div class="inner">
                <div class="col_main">
                    <div class="main_bd">
                        <div class="delivery_edit" id="js_delivery_container">
                            <div class="frm_control_group">
                                <label for="" class="frm_label">
                                    模板名称</label>
                                <div class="frm_controls">
                                    <span class="frm_input_box">
                                        <input id="name" value="" name="" type="text" placeholder="" class="frm_input" autocomplete="off">
                                    </span>
                                    <p class="frm_fail js_name_fail">
                                    </p>
                                    <p class="frm_tips fail dn">
                                        请填写模板名称，不超过40个字节</p>
                                    <p class="frm_tips">
                                        不超过40个字节（1个汉字为2个字节）</p>
                                </div>
                            </div>
                            <div class="frm_control_group frm_control_checkbox">
                                <span class="frm_label">计费方式</span>
                          
                                    <select id="ComputedPriceMethod" class="mi-input" style="margin-left: 0; width: 120px;">
                                        <option value="1">按数量计费</option>
                                        <option value="2">按重量计费</option>
                                    </select>
                              
                                
                            </div>
                            <div class="frm_control_group frm_control_checkbox">
                                <span class="frm_label">配送方式</span>
                                <label for="js_agree_1" class="frm_checkbox_label"  >
                                    <input type="checkbox" id="js_agree_1" class="js_delivery_type frm_checkbox" value="1">
                                    <span class="lbl_content">平邮</span>
                                </label>
                                <label for="js_agree_2" class="frm_checkbox_label">
                                    <i class="icon_checkbox"></i>
                                    <input type="checkbox" id="js_agree_2" class="js_delivery_type frm_checkbox" value="2">
                                    <span class="lbl_content">快递</span>
                                </label>
                                <label for="js_agree_3" class="frm_checkbox_label"  >
                                    <i class="icon_checkbox"></i>
                                    <input type="checkbox" id="js_agree_3" class="js_delivery_type frm_checkbox" value="3">
                                    <span class="lbl_content">EMS</span>
                                </label>
                                <label style="margin-left:20px;">暂时只支持快递配送</label>
                                <div class="frm_controls">
                                    <p class="frm_tips fail js_delivery_type_fail dn" style="display: none;">
                                        请选择配送方式</p>
                                </div>
                            </div>
                            <div class="frm_control_group" id="js_delivery_setting">
                                <span class="frm_label">运费设置</span>
                                <div class="frm_controls">
                                    <p class="frm_tips js_delivery_type_fail " style="display: ;">
                                        请先选择配送方式</p>
                                </div>
                                <ul class="table_list none" id="js_delivery_item_1" style="display: none;">
                                    <li class="thead group">
                                        <div class="table_cell">
                                            <div class="td_panel br_none" style="background-color: #f4f5f9;">
                                                平邮运费设置</div>
                                        </div>
                                    </li>
                                    <li class="tbody head group">
                                        <div class="table_cell edit_range l">
                                            <div class="td_panel">
                                                配送区域</div>
                                        </div>
                                        <div class="table_cell edit_f_num l tl">
                                            <div class="td_panel" >
                                                首N件/首重(Kg)</div>
                                        </div>
                                        <div class="table_cell edit_f_price l tl">
                                            <div class="td_panel">
                                                首费(￥)</div>
                                        </div>
                                        <div class="table_cell edit_l_num l tl"  >
                                            <div class="td_panel">
                                                续M件/续重(Kg)</div>
                                        </div>
                                        <div class="table_cell edit_l_price l tl">
                                            <div class="td_panel" >
                                                续费(￥)</div>
                                        </div>
                                    </li>
                                    <li class="tbody group js_normal_area">
                                        <div class="table_cell edit_range l">
                                            <div class="td_panel">
                                                全国默认地区</div>
                                        </div>
                                        <div class="group input_group">
                                            <div class="table_cell edit_f_num l tl">
                                                <div class="td_panel">
                                                    <span class="frm_input_box">
                                                        <input value="" name="startstandards" class="startstandards frm_input" type="text"
                                                            placeholder="">
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="table_cell edit_f_price l tr">
                                                <div class="td_panel">
                                                    <span class="frm_input_box">
                                                        <input value="" name="startfees" class="startfees frm_input" type="text" placeholder="">
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="table_cell edit_l_num l tl">
                                                <div class="td_panel">
                                                    <span class="frm_input_box">
                                                        <input value="" name="addstandards" class="addstandards frm_input" type="text" placeholder="">
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="table_cell edit_l_price l tr">
                                                <div class="td_panel">
                                                    <span class="frm_input_box">
                                                        <input value="" name="addfees" class="addfees frm_input" type="text" placeholder="">
                                                    </span>
                                                </div>
                                            </div>
                                            <p class="frm_tips fail dn">
                                                件数需为0至99的整数，费用应为0.00至999.99的数字</p>
                                        </div>
                                    </li>
                                    <li class="tbody group">
                                        <div class="table_cell">
                                            <div class="td_panel">
                                                <a class="js_add_area" href="javascript:;" style="color: #1e5494;">为指定地区设置运费</a></div>
                                        </div>
                                    </li>
                                </ul>
                                <ul class="table_list none" id="js_delivery_item_2" style="display: none;">
                                    <li class="thead group">
                                        <div class="table_cell">
                                            <div class="td_panel br_none" style="background-color: #f4f5f9;">
                                                快递运费设置</div>
                                        </div>
                                    </li>
                                    <li class="tbody head group">
                                        <div class="table_cell edit_range l">
                                            <div class="td_panel">
                                                配送区域</div>
                                        </div>
                                        <div class="table_cell edit_f_num l tl"  >
                                            <div class="td_panel">
                                                首N件/首重(Kg)</div>
                                        </div>
                                        <div class="table_cell edit_f_price l tl">
                                            <div class="td_panel">
                                                首费(￥)</div>
                                        </div>
                                        <div class="table_cell edit_l_num l tl"  >
                                            <div class="td_panel">
                                                续M件/续重(Kg)</div>
                                        </div>
                                        <div class="table_cell edit_l_price l tl">
                                            <div class="td_panel">
                                                续费(￥)</div>
                                        </div>
                                    </li>
                                    <li class="tbody group js_normal_area">
                                        <div class="table_cell edit_range l">
                                            <div class="td_panel">
                                                全国默认地区</div>
                                        </div>
                                        <div class="group input_group">
                                            <div class="table_cell edit_f_num l tl">
                                                <div class="td_panel">
                                                    <span class="frm_input_box">
                                                        <input value="" name="startstandards" class="startstandards frm_input" type="text"
                                                            placeholder="">
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="table_cell edit_f_price l tr">
                                                <div class="td_panel">
                                                    <span class="frm_input_box">
                                                        <input value="" name="startfees" class="startfees frm_input" type="text" placeholder="">
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="table_cell edit_l_num l tl">
                                                <div class="td_panel">
                                                    <span class="frm_input_box">
                                                        <input value="" name="addstandards" class="addstandards frm_input" type="text" placeholder="">
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="table_cell edit_l_price l tr">
                                                <div class="td_panel">
                                                    <span class="frm_input_box">
                                                        <input value="" name="addfees" class="addfees frm_input" type="text" placeholder="">
                                                    </span>
                                                </div>
                                            </div>
                                            <p class="frm_tips fail dn">
                                                件数需为0至99的整数，费用应为0.00至999.99的数字</p>
                                        </div>
                                    </li>
                                    <li class="tbody group">
                                        <div class="table_cell">
                                            <div class="td_panel">
                                                <a class="js_add_area" href="javascript:;" style="color: #1e5494;">为指定地区设置运费</a></div>
                                        </div>
                                    </li>
                                </ul>
                                <ul class="table_list none" id="js_delivery_item_3" style="display: none;">
                                    <li class="thead group">
                                        <div class="table_cell">
                                            <div class="td_panel br_none" style="background-color: #f4f5f9;">
                                                EMS运费设置</div>
                                        </div>
                                    </li>
                                    <li class="tbody head group">
                                        <div class="table_cell edit_range l">
                                            <div class="td_panel">
                                                配送区域</div>
                                        </div>
                                        <div class="table_cell edit_f_num l tl"  >
                                            <div class="td_panel">
                                                首N件/首重(Kg)</div>
                                        </div>
                                        <div class="table_cell edit_f_price l tl">
                                            <div class="td_panel">
                                                首费(￥)</div>
                                        </div>
                                        <div class="table_cell edit_l_num l tl"  >
                                            <div class="td_panel">
                                                续M件/续重(Kg)</div>
                                        </div>
                                        <div class="table_cell edit_l_price l tl">
                                            <div class="td_panel">
                                                续费(￥)</div>
                                        </div>
                                    </li>
                                    <li class="tbody group js_normal_area">
                                        <div class="table_cell edit_range l">
                                            <div class="td_panel">
                                                全国默认地区</div>
                                        </div>
                                        <div class="group input_group">
                                            <div class="table_cell edit_f_num l tl">
                                                <div class="td_panel">
                                                    <span class="frm_input_box">
                                                        <input value="" name="startstandards" class="startstandards frm_input" type="text"
                                                            placeholder="">
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="table_cell edit_f_price l tr">
                                                <div class="td_panel">
                                                    <span class="frm_input_box">
                                                        <input value="" name="startfees" class="startfees frm_input" type="text" placeholder="">
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="table_cell edit_l_num l tl">
                                                <div class="td_panel">
                                                    <span class="frm_input_box">
                                                        <input value="" name="addstandards" class="addstandards frm_input" type="text" placeholder="">
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="table_cell edit_l_price l tr">
                                                <div class="td_panel">
                                                    <span class="frm_input_box">
                                                        <input value="" name="addfees" class="addfees frm_input" type="text" placeholder="">
                                                    </span>
                                                </div>
                                            </div>
                                            <p class="frm_tips fail dn">
                                                件数需为0至99的整数，费用应为0.00至999.99的数字</p>
                                        </div>
                                    </li>
                                    <li class="tbody group">
                                        <div class="table_cell">
                                            <div class="td_panel">
                                                <a class="js_add_area" href="javascript:;" style="color: #1e5494;">为指定地区设置运费</a></div>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                            <div class="frm_control_group">
                                <span class="frm_label"></span>
                                <div class="frm_controls">
                                    <a class="btn btn_primary" id="js_btn_save" href="javascript:;">保存</a>&nbsp;&nbsp;&nbsp;
                                    <a class="btn btn_default" id="js_btn_cancel" href="javascript:;">取消</a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!--编辑指定地区弹窗--->
                    <div class="mask" style="display: none;">
                    </div>
                    <div id="div_agentmsgset" style="background-color: #ffffff; border: 2px solid #5984bb;
                        width: 600px; height: 450px; display: none;">
                        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#E7F0FA"
                            style="padding: 5px;">
                            <tr>
                                <td height="42" colspan="2" bgcolor="#C1D9F3">
                                    <span style="padding-left: 10px; font-size: 12px; font-weight: bold" id="span2">选择地区
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td height="42" colspan="2" bgcolor="#ffffff">
                                    省、直辖市、市(<span style="font-size: large; font-weight: bold;">按住ctrl键实现城市多选</span>)
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" bgcolor="#ffffff" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                                    <div class="ui-form-item">
                                        <label class="ui-label">
                                            所在省市</label>
                                        <select name="com_province" id="com_province" class="ui-input" style="width: 120px;">
                                            <option value="省份">省份</option>
                                        </select>
                                        <select name="com_city" id="com_city" class="ui-input" style="width: 343px;" multiple="multiple"
                                            size="20">
                                            <option value="城市">全部</option>
                                        </select>
                                    </div>
                                    <input id="sub_rh" type="button" class="formButton" value="  提  交  " />
                                    <input id="cancel_rh" type="button" class="formButton" value="  关  闭  " />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <script type="text/javascript">
                        var province = document.getElementById('com_province');
                        var city = document.getElementById('com_city');
                    </script>
                    <script src="/Scripts/City.js" type="text/javascript"></script>
                    <input type="hidden" id="hid_js_edit_area_index" value="0" />
                    <input type="hidden" id="hid_delivery1_js_area" value="" />
                    <input type="hidden" id="hid_delivery2_js_area" value="" />
                    <input type="hidden" id="hid_delivery3_js_area" value="" />
                    <input type="hidden" id="hid_deliverytmpid" value="<%=tmpid %>" />
</asp:Content>
