<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="/UI/Etown.Master" CodeBehind="OrderFinSet_prolist.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.Order.OrderFinSet_prolist" %>

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

            var pageSize = 20; //每页显示条数
            var comid = $("#hid_comid").trimVal();
            var userid = $("#hid_userid").trimVal();

            //获取订单列表
            SearchList(1);
            //装载产品列表
            function SearchList(pageindex) {

                var key = $("#key").trimVal();
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }

                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=orderfinset_pro_list",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, key: key, startdate: "<%=stardate %>", enddate: "<%=enddate %>", submanagename: "<%=submanagename %>" },
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
                     <a href="javascript:history.go(-1);"><<<返回</a></h3> 
                <div style="float:right;padding-right:110px; display:none;"> 


                     <label>
                        提单时间
                        <input class="mi-input" name="startime" id="startime" placeholder="开始时间" value=""
                            isdate="yes" type="text" style="width: 120px;">-
                        <input class="mi-input" name="endtime" id="endtime" placeholder="结束时间" value="" isdate="yes"
                            type="text" style="width: 120px;">
                    </label>
                      <label> 
                        关键字<input name="key" type="text" id="key"  class="mi-input" value="" placeholder="手机，姓名，订单号,产品名称">
                    </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px;
                            height: 26px;">
                    </label>
                 
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
                        <td width="60px">
                            <p align="left">
                            出行/入住日期
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
                        <td width="25px">
                            <p align="center">
                                数量
                            </p>
                        </td>
                        <td width="30px">
                            <p align="center">
                                优惠
                            </p>
                        </td>
                        <td width="30px">
                            <p align="center">
                                运费
                            </p>
                        </td>
                        <td width="45px">
                            <p align="center">
                                实收
                            </p>
                        </td>
                        <td width="65px">
                            <p align="center">
                                收款人
                            </p>
                        </td>
                        <td width="65px">
                            <p align="center">
                                支付方式
                            </p>
                        </td>
                        <td width="100px">
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
                            <p align="left" title="${U_name}(${U_phone})">
                                ${U_name}(${U_phone})
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                               {{if Server_type==2 || Server_type==8}}${Totalcount}{{else}} ${Pay_price}{{/if}}</p>
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
                         ${Express}
                        </p>
                        </td>
                        <td valign="top">
                            <p align="center">
                               {{if Server_type !=1}}  {{if Pay_state==2}} ${Pay_price*U_num+Express}{{else}}0{{/if}}{{else}}{{if Order_state>1}} ${Paymoney+Express} {{/if}}{{/if}}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="center" title="${submanagename}">
                            ${submanagename}
                            </p>
                        </td>
                        <td valign="top" title="${Ticketinfo}">
                            <p align="center">
                             ${Ticketinfo}
                            </p>
                        </td>

                        <td valign="top">
                            <p align="center">

                             {{if Source_type!=4 }} 
                                {{if Order_state==1}}
                                     等待客户付款
                                {{/if}}
                                {{if Order_state==2 ||  Order_state==4}} 
                                     已付款
                                {{/if}}
                                {{if Order_state==8}}
                                     订房已验证
                                {{/if}}
                                {{if Order_state==16}}
                                     订单已退款
                                {{/if}}
                                {{if Order_state==22}}
                                    订房成功
                                {{/if}}
                                {{if Order_state==23}}
                                     超时订单
                                {{/if}}
                                {{if Order_state==18}}
                                     已退订
                                {{/if}}
                                {{else}}
                                 {{if BindingOrder != null}}
                                 {{if BindingOrder.Order_state==1}}
                                     等待客户付款
                                {{/if}}
                                {{if BindingOrder.Order_state==2 ||  BindingOrder.Order_state==4}} 
                                    已付款
                                {{/if}}
                                
                                {{if BindingOrder.Order_state==8}}
                                     已验证
                                {{/if}}
                                {{if BindingOrder.Order_state==16}}
                                     订单已退款
                                {{/if}}
                                {{if BindingOrder.Order_state==22}}
                                    确认成功
                                {{/if}}
                                {{if BindingOrder.Order_state==23}}
                                     超时订单
                                {{/if}}
                                {{if BindingOrder.Order_state==18}}
                                     已退订
                                {{/if}}



                                 {{/if}}
                                {{/if}}
                            </p>
                        </td>
                    </tr>
    </script>
    <input type="hidden" id="hid_id" value="0" />
    <input type="hidden" id="hid_proid" value="0" />

    <input type="hidden" id="hid_stardate" value="0" />
    <input type="hidden" id="hid_enddate" value="0" />
    <input type="hidden" id="hid_submanagename" value="0" />


</asp:Content>