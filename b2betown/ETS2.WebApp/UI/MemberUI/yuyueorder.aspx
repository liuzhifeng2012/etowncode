<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="yuyueorder.aspx.cs" Inherits="ETS2.WebApp.UI.MemberUI.yuyueorder" %>


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

            var pageSize = 10; //每页显示条数
            var comid = $("#hid_comid").trimVal();
            var userid = $("#hid_userid").trimVal();


            $("html").die().live("keydown", function (event) {
                if (event.keyCode == 13) {
                    $("#Search").click();    //这里添加要处理的逻辑  
                    return false;
                }
            });



            //获取订单列表
            SearchList(1);
            //装载产品列表
            function SearchList(pageindex) {
                var servertype = 0;
                var key = $("#key").trimVal();
                var order_state = 0;
                var datetype = $("#datetype").trimVal();
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
                    url: "/JsonFactory/OrderHandler.ashx?oper=getyuyueorderlist",
                    data: { servertype: servertype, userid: $("#hid_userid").trimVal(), comid: comid, pageindex: pageindex, pagesize: pageSize, key: key, order_state: order_state, ordertype: 5, beginDate: $("#startime").trimVal(), endDate: $("#endtime").trimVal(), datetype: datetype },
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
                            }


                        }
                    }
                })


            }

            $("#Search").click(function () {
                SearchList(1);
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



        })

    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">

        <div id="setting-home" class="vis-zone  mail-list">
            <div class="inner">
                <h3>
                    绑定预约产品使用情况统计</h3>
                <div style="float:right;padding-right:110px;"> 

                     <label>
                        <select id="datetype" class="">
                        <option value="0" selected>按订单日期</option>
                    </select>
                        <input class="mi-input" name="startime" id="startime" placeholder="开始时间" value=""  isdate="yes" type="text" style="width: 120px;">-
                        <input class="mi-input" name="endtime" id="endtime" placeholder="结束时间" value="" isdate="yes"  type="text" style="width: 120px;">
                    </label>
                      <label> 
                        关键字<input name="key" type="text" id="key"  class="mi-input" value="" placeholder="手机，姓名，订单号,产品名称">
                    </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px;
                            height: 26px;">
                    </label>
                 
                </div>
                <div style="float:right; padding-right:110px;">
                </div>
                <table width="780px" border="0">
                    <tr>
                        <td width="45px" height="30px">
                            <p align="left">
                                订单号
                            </p>
                        </td>
                        <td width="100px">
                            <p align="left">
                                提交时间
                            </p>
                        </td>
                        <td width="207px">
                            <p align="left">
                                产品名称
                            </p>
                        </td>

                        <td width="100px">
                            <p align="left">
                                购买人
                            </p>
                        </td>
                        <td width="45px">
                            <p align="center">
                                单价
                            </p>
                        </td>
                        <td width="45px">
                            <p align="center">
                                实收
                            </p>
                        </td>
                        <td width="25px">
                            <p align="center">
                                数量
                            </p>
                        </td>
                        <td width="60px">
                            <p align="center">
                                已使用数量
                            </p>
                        </td>
                        <td width="60px">
                            <p align="center">
                                剩余数量
                            </p>
                        </td>
                        <td width="50px">
                            <p align="center">
                                备注
                            </p>
                        </td>
                        <td width="60px">
                            <p align="center">
                                来源
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
                        <td valign="top">
                            <p align="left" title="${U_name}(${U_phone})">
                                 {{if U_idcard !=""}}
                                 <input type="button" onclick="alert('姓名：${U_name} 身份证：${U_idcard} 手机：${U_phone}');" value="身份证"/>
                                {{/if}}
                                ${U_name}(${U_phone})
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                               {{if Server_type==2 || Server_type==8}}${Totalcount}{{else}} ${Pay_price}{{/if}}</p>
                        </td>
                        <td valign="top">
                            <p align="center">
                               {{if Server_type !=1}}  {{if Pay_state==2}} ${Pay_price*U_num+Express}{{else}}0{{/if}}{{else}}{{if Order_state>1}} ${Paymoney+Express} {{/if}}{{/if}}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                               ${U_num}</p>
                        </td>
                        <td valign="top">
                            <p align="center">
                              
                               {{if Order_state==16}}
                                     0
                                 {{else}}
                                    ${U_num-use_num}
                                {{/if}}
                               
                               </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                               ${use_num}</p>
                        </td>
                        <td valign="top">
                            <p align="center" title="${Ticketinfo}">
                           ${Ticketinfo}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                               {{if Agentid!="0"}} 分销 {{if Warrant_type=="1"}}(出){{else}}(验){{/if}} {{else}}{{if Openid==""}}网站{{else}}微信{{/if}}{{/if}}
                            </p>
                        </td>

                        <td valign="top">
                            <p align="center">
                                {{if Order_state==1}} 
                                     等待付款 
                                {{/if}}
                                {{if Order_state==2}} 
                                     订单已付款 
                                {{/if}}
                                {{if Order_state==4}}
                                     已发码
                                {{/if}}
                                {{if Order_state==8}}
                                     已使用
                                {{/if}}
                                {{if Order_state==16}}
                                     订单已退款
                                {{/if}}
                                {{if Order_state==23}}
                                     超时订单
                                {{/if}}
                                {{if Order_state==18}}
                                     已退订
                                {{/if}}
                               
                            </p>
                        </td>
                    </tr>
    </script>
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
            <tr id="tr_tnum">
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
                        确认预留房</label>
                    <label>
                        <input type="radio" name="roomstate" value="0" />
                        无房作废</label>
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

    <div id="express" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 400px; height: auto; display: none; left: 20%; position: absolute;
        top: 20%;">
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
                    <input name="cancel_express" type="button" class="formButton" id="cancel_express" value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>
     <div id="expresscart" style="background-color: #ffffff; border: 2px solid #5984bb;
        margin: 0px auto; width: 500px; height: auto; display: none;  left: 20%; position: absolute;
        top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="100%" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;" >
                      <table width="400px" id="prolist" class="grid" style=" padding:5px;">
                        
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
                    <input id="express_cart_sub" name="express_cart_sub" type="button" class="formButton" value="  确认发货  " />
                    <input name="cancel_cart_express" type="button" class="formButton" id="cancel_cart_express" value="  关  闭  " />
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
</asp:Content>