<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="/UI/Etown.Master" CodeBehind="OrderFinSet.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.Order.OrderFinSet" %>

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


            //获取订单列表
            SearchList(1);
            //装载产品列表
            function SearchList(pageindex) {
                var date1 = $("#startdate").trimVal();
                var date2 = $("#enddate").trimVal();
                var key = $("#key").trimVal();

                if (date1 == "" || date2 == "") {
                    $.prompt("请选择查询的日期范围");
                    return;
                }

                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=orderfinset",
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
                                                                $("#tblist").html("未查询到数据");
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
                SearchList(1);
            })


        })


        //修改状态
        function caiwuqueren(comid, startdate, enddate, submanagename) {
            if (comid == '') {
                $.prompt("参数传递错误");
                return;
            }

            if (confirm("确认" + submanagename + ": 从" + startdate + " 到" + enddate + " 前台提单财务已核实！")) {

                $.ajax({
                    type: "post",
                    url: "/JsonFactory/OrderHandler.ashx?oper=orderfinset_quren",
                    data: { comid: comid, startdate: startdate, enddate: enddate, submanagename: submanagename },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("操作出错");
                            return;
                        }
                        if (data.type == 100) {
                            $.prompt("操作成功");
                            //window.location.reload();
                        }
                    }
                })
            }
        }

    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">

        <div id="setting-home" class="vis-zone  mail-list">
            <div class="inner">
                <h3>
                    前台提单财务结算</h3>
                <div>
                </div>
                <p>
                    查询：<input type="text" maxlength="100" size=20" name="key" id="key" style=" display:none;"> 从
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
                               姓名
                            </p>
                        </td>
                        <td width="200">
                            <p align="left">
                                日期
                            </p>
                        </td>
                        <td width="46">
                            <p align="left">
                                订单数量
                            </p>
                        </td>
                        <td width="60">
                            <p align="left">
                                收取金额
                            </p>
                        </td>
                        <td width="150">
                            <p align="left">
                                支付方式
                            </p>
                        </td>
                        <td width="120">
                            <p align="left">
                                
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
                                ${submanagename}
                            </p>
                        </td>
                        <td >
                            <p align="left">
                                ${riqi}</p>
                        </td>
                        <td >
                            <p align="left">
                                <u>${Countnum}</u></p>
                        </td>
                        <td >
                            <p align="left">
                                <u>${Pay_price}元</u></p>
                        </td>
                        <td >
                            <p align="left">
                                {{if zhifu !=null}}
                                    {{each(i,data) zhifu}} 
                                        {{= data.Ticketinfo}}：{{= data.Countnum}}笔，共{{= data.Pay_price}}元  <br>
                                    {{/each}} 
                                {{/if}}
                            </p>
                        </td> <td>
                            <p align="left">
                                <u> <a href="OrderFinSet_prolist.aspx?comid=${comid}&stardate=${stardate}&enddate=${enddate}&submanagename=${submanagename}">查看订单</a>    <a href="javascript:;" class="a_anniu" onclick="caiwuqueren('${comid}','${stardate}','${enddate}','${submanagename}')">财务确认</a></u></p>
                        </td>
                       
                    </tr>
                  
    </script>
</asp:Content>
