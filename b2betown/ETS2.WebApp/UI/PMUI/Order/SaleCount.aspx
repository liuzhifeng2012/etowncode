<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="SaleCount.aspx.cs"
    Inherits="ETS2.WebApp.UI.PMUI.Order.SaleCount" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //日历
            var startdate = '<%=this.startdate %>';
            var enddate = '<%=this.enddate %>';
            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {

                $("#startdate").val(startdate);
                $("#enddate").val(enddate);
                $($(this)).datepicker();
            });


            var pageSize = 20; //每页显示条数
            var comid = $("#hid_comid").trimVal();
            var userid = $("#hid_userid").trimVal();


            //获取汇总信息
            $.post("/JsonFactory/OrderHandler.ashx?oper=gettotaldate", { comid: comid, startdate: startdate, enddate: enddate }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    $.prompt("获取汇总信息Error");
                    return;
                }
                if (data.type == 100) {
                    $("#u_num").text(data.msg.salecount[0].u_num);
                    $("#totalpay").text(fmoney(data.msg.salecount[0].totalpay, 2));
                    $("#totalprofit").text(fmoney(data.msg.salecount[0].totalprofit, 2));
                }
            })

            //获取订单列表
            SearchList(1);
            //装载产品列表
            function SearchList(pageindex) {
                var date1 = $("#startdate").trimVal();
                var date2 = $("#enddate").trimVal();
                var key = $("#key").trimVal();

                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=getsalecount",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, startdate: date1, enddate: date2, key: key },
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

            //查询按钮
            $("#chaxun").click(function () {
                //var date1 = $("#startdate").trimVal();
                //var date2 = $("#enddate").trimVal();
                //location.href = "salecount.aspx?startdate=" + date1 + "&enddate=" + date2;
                SearchList(1);
            })


        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <%--<div id="secondary-tabs" class="navsetting ">
            <ul>
                   <li><a href="/ui/pmui/ProjectList.aspx" onfocus="this.blur()" target=""><span>
                    项目列表</span></a></li>
                <li><a href="/ui/pmui/Projectedit.aspx" onfocus="this.blur()" target=""><span>添加项目</span></a></li>
                <li><a href="/ui/pmui/ProductList.aspx" onfocus="this.blur()" target=""><span>产品列表</span></a></li>
                <li><a href="/ui/pmui/ProductServerTypeList.aspx" onfocus="this.blur()" target=""><span>
                    添加产品</span></a></li>
                     <li  class="on"><a href="/ui/pmui/order/Salecount.aspx" onfocus="this.blur()" target="">产品统计</a></li>
                       <li><a href="/ui/pmui/BindingAgent.aspx" onfocus="this.blur()" target="">导入分销系统产品</a></li>
                    <li  ><a href="/ui/pmui/eticket_useset.aspx" onfocus="this.blur()" target="">
                        <span>商户特定日期设定</span></a></li>
                          <li><a href="/ui/pmui/delivery/deliverylist.aspx" onfocus="this.blur()" target="">
                    <span>运费模版管理</span></a></li>
            </ul>
        </div>--%>
        <div class="navsetting ">
            <ul class="composetab">
                 <li class="on" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/order/SaleCount.aspx">产品统计</a></div>
                    </div>
                </li> 
                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/order/HotelOrderStat.aspx">订房统计</a></div>
                    </div>
                </li> 
                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/order/ProAgentSaleCount.aspx">产品销售统计</a></div>
                    </div>
                </li> 
                
            </ul>
        </div>
        <div id="setting-home" class="vis-zone  mail-list">
            <div class="inner">
                <h3>
                    销售统计</h3>
                <div>
                </div>
                <p>
                    查询： 产品名称 <input type="text" maxlength="100" size=20" name="key" id="key"> 从
                    <input type="text" maxlength="100" size="15" name="startdate" id="startdate" isdate="yes">
                    到
                    <input type="text" maxlength="100" size="15" name="enddate" id="enddate" isdate="yes">
                    <input type="button"   value="查询" method="post" id="chaxun" >
                    <%--<a href="salecount.aspx?startdate=<%=enddate %>&enddate=<%=enddate %>" iscx="yes">今天</a> <a href="salecount.aspx?startdate=<%=yesterdayDate %>&enddate=<%=yesterdayDate %>" iscx="yes">昨天</a><a href="salecount.aspx?startdate=<%=weekfirstdate %>&enddate=<%=weekenddate %>" iscx="yes"> 本周</a><a href="salecount.aspx?startdate=<%=enddate %>&enddate=<%=enddate %>" iscx="yes"> 上周 </a><a href="javascript:void(0)" iscx="yes"> 本月</a><a href="salecount.aspx?startdate=<%=enddate %>&enddate=<%=enddate %>" iscx="yes"> 上月</a><a href="salecount.aspx?startdate=<%=enddate %>&enddate=<%=enddate %>" iscx="yes"> 所有时间</a>--%>
                </p>
                <table border="0">
                    <tr>
                        
                        <td width="150">
                            <p align="left">
                                产品
                            </p>
                        </td>
                        <td width="46">
                            <p align="left">
                                门市价
                            </p>
                        </td>
                        <td width="46">
                            <p align="left">
                                销售价
                            </p>
                        </td>
                        <td width="34">
                            <p align="left">
                                订单数量
                            </p>
                        </td>
                        <td width="34">
                            <p align="left">
                                数量
                            </p>
                        </td>
                        <td width="85">
                            <p align="left">
                                出票金额
                            </p>
                        </td>
                        <td width="45">
                            <p align="left">
                                已消费（出票）
                            </p>
                        </td>
                        <td width="45">
                            <p align="left">
                                退票
                            </p>
                        </td>
                        <td width="45">
                            <p align="left">
                                已消费（倒码）
                            </p>
                        </td>
                        <td width="35">
                            <p align="left">
                                沉淀
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
    <input type="hidden" id="hid_startdate" value="" />
    <input type="hidden" id="hid_enddate" value="" />
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                  <tr>
                        <td >
                            <p align="left">
                                ${Pro_name}
                            </p>
                        </td>
                        <td >
                            <p align="left">
                                ${Face_price}</p>
                        </td>
                        <td >
                            <p align="left">
                                <u>${Advise_price}</u></p>
                        </td>
                        <td >
                            <p align="left">
                                <u>${OrderCount.Totalpay}</u></p>
                        </td>
                        <td >
                            <p align="left">
                                ${OrderCount.U_num}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${OrderCount.Totalpay_price}
                                {{if Daoma_OrderCount.Totalpay_price !=0}}倒码：${Daoma_OrderCount.Totalpay_price}{{/if}}
                                </p>
                        </td>
                        <td >
                            <p align="left">
                                ${Xiaofei_price}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${Tuipiao}</p>
                        </td>
                        <td >
                            <p align="left">
                               ${Daoma_Xiaofei_price} </p>
                        </td>
                        <td >
                            <p align="left">
                               ${Chendian_price} </p>
                        </td>
                    </tr>
                  
    </script>
</asp:Content>
