<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgentFinanceCount.aspx.cs"
    MasterPageFile="/UI/Etown.Master" Inherits="ETS2.WebApp.UI.PMUI.AgentCount" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var comid = $("#hid_comid").trimVal();

            SearchList(comid, CurentMonthFirstDate(), CurentDate());


            $("#Search").click(function () {
                SearchList(comid, $("#startime").val(), $("#endtime").val());
            })

            //日历
            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {
                $($(this)).datepicker();
            });
            $("#startime").val(CurentMonthFirstDate());
            $("#endtime").val(CurentDate());


            //增加月份
            $("#btn-add").click(function () {
                var i = $("#u_num").val();
                var patrn = /^([1-9]\d*|1)(\.\d*[1-9])?$/;
                if (!patrn.exec(i)) {
                    $.prompt("请填写正确的月份");
                    return;
                }
                else {
                    i++;
                    $("#u_num").val(i);
                }
            })
            //减少月份
            $("#btn-reduce").click(function () {
                var i = $("#u_num").val();
                var patrn = /^([1-9]\d*|1)(\.\d*[1-9])?$/;
                if (!patrn.exec(i)) {
                    $.prompt("请填写正确的月份");
                    return;
                }
                else {
                    i--;
                    if (i > 0) {
                        $("#u_num").val(i);
                    } else {
                        return;
                    }
                }
            })
        })
        //查询分销商统计信息
        function SearchList(comid, StartDate, EndDate) {
            var day = DateDiff(EndDate, StartDate);
            if (parseInt(day) < 0) {
                alert("起始时间不可大于结束时间");
                return;
            }

            if (parseInt(day) > 30) {
                alert("查询区间不可大于一个月");
                return;
            }
            showLoading();
            $.post("/JsonFactory/AgentHandler.ashx?oper=GetAgentFinanceCount", { comid: comid, StartDate: StartDate, EndDate: EndDate }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                    hideLoading();
                }
                if (data.type == 100) {
                    $("#tblist").empty(); 
                    $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                    hideLoading();
                }
            })
        }

        //当前月第一天
        function CurentMonthFirstDate() {
            var now = new Date();
            var year = now.getFullYear();       //年
            var month = now.getMonth() + 1;     //月

            var clock = year + "-";

            if (month < 10)
                clock += "0";

            clock += month + "-";

            return (clock + "01");
        }
        //当前日期
        function CurentDate() {
            var now = new Date();

            var year = now.getFullYear();       //年
            var month = now.getMonth() + 1;     //月
            var day = now.getDate();            //日

            //var hh = now.getHours();            //时
            //var mm = now.getMinutes();          //分

            var clock = year + "-";

            if (month < 10)
                clock += "0";

            clock += month + "-";

            if (day < 10)
                clock += "0";

            clock += day;

            //if(hh < 10)
            //    clock += "0";

            //clock += hh + ":";
            //if (mm < 10) clock += '0'; 
            // clock += mm; 
            return (clock);
        }

        function showLoading() {
            $("html").css({
                "overflow-y": "hidden"
            });
            if ($("#bgDiv").html() == null) {
                $('<div id="bgDiv"></div>').appendTo("body")
            }
            if ($("#loading").html() != null) {
                $("#loading").remove()
            }
            $('<div id="loading" style="top: 352px;"><img src="/Images/loading.gif" alt="loading..."></div>').appendTo("body");
            var b = $(window).height();
            var d = $(window).scrollTop();
            var c = $("#loading").height();
            $("#bgDiv").css({
                height: $(document).innerHeight()
            }).show();
            $("#loading").css({
                top: (b - c) / 2
            }).show()
        }
        function hideLoading() {
            $("html").css({
                "overflow-y": "auto"
            });
            $("#bgDiv, #loading").hide();

        }
    </script>
    <style type="text/css">
        /*---loading --*/
        #loading
        {
            position: fixed;
            z-index: 99999;
            width: 50%;
            margin: 0 25%;
            display: none;
        }
        #loading img
        {
            width: 85px;
            height: 85px;
            margin: 30px auto;
            display: block;
        }
        #bgDiv
        {
            width: 100%;
            height: 100%;
            background: #000;
            filter: alpha(opacity=50);
            -moz-opacity: .5;
            -khtml-opacity: .5;
            opacity: .5;
            position: absolute;
            top: 0;
            left: 0;
            z-index: 999;
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <%--<div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="AgentList.aspx" onfocus="this.blur()" target=""><span>分销商管理</span></a></li>
                
                 
                <li><a href="AgentSalesCode.aspx" onfocus="this.blur()" target=""><span>后台销售订单</span></a></li>
                <li><a href="AgentBackCode.aspx" onfocus="this.blur()" target=""><span>导码订单</span></a></li>
                <li><a href="AgentRecharge.aspx" onfocus="this.blur()" target=""><span>充值订单</span></a></li>
                   <li ><a href="AgentRecharge_Person.aspx" onfocus="this.blur()" target=""><span>人工充值记录</span></a></li>
                <li><a href="AgentMessage.aspx" onfocus="this.blur()" target="">管理分销通知</a></li>
                <li class="on"><a href="AgentFinanceCount.aspx" onfocus="this.blur()" target="">分销商统计</a>
                </li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone  mail-list">
            <div class="inner">
                <div style="margin-top: 20px;">
                    <label>
                        日汇总:</label>
                    起始日期<input type="text" id="startime" isdate="yes" style="width: 120px; height: 20px;" />
                    截止日期<input type="text" id="endtime" isdate="yes" style="width: 120px; height: 20px;" />
                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px; height: 26px;">
                    </label>
                </div>
                <div style="margin-top: 20px; display: none;">
                    <label>
                        月汇总:</label>
                    起始年月
                    <select id="sel_startyear">
                        <option value="2014">2014</option>
                        <option value="2015">2015</option>
                        <option value="2016">2016</option>
                    </select>
                    <select id="sel_startmonth">
                        <option value="1">1</option>
                        <option value="2">2</option>
                        <option value="3">3</option>
                        <option value="4">4</option>
                        <option value="5">5</option>
                        <option value="6">6</option>
                        <option value="7">7</option>
                        <option value="8">8</option>
                        <option value="9">9</option>
                        <option value="10">10</option>
                        <option value="11">11</option>
                        <option value="12">12</option>
                    </select>
                    &nbsp;&nbsp; <span>查几月：</span> <a href="javascript:void(0);" id="btn-reduce">-</a>
                    <input type="text" maxlength="4" autocomplete="off" value="1" id="u_num" size="5"
                        readonly="readonly">
                    <a href="javascript:void(0);" id="btn-add">+</a>
                    <input type="button" id="Button1" value="查询" style="width: 120px; height: 26px;">
                </div>
                <table width="780" border="0" style="margin-top: 20px;">
                    <tr>
                        <td width="100px">
                            <p align="left">
                                日期
                            </p>
                        </td>
                        <td width="80px">
                            <p align="left">
                                在线充值
                            </p>
                        </td>
                        <td width="80px">
                            <p align="left">
                                人工充值
                            </p>
                        </td>
                        <td width="80px">
                            <p align="left">
                                人工返点
                            </p>
                        </td>
                        <td width="80px">
                            <p align="left">
                                销售额
                            </p>
                        </td>
                        <td width="80px">
                            <p align="left">
                                毛利
                            </p>
                        </td>
                        <td width="80px">
                            <p align="left">
                                倒码销售额
                            </p>
                        </td>
                        <td width="80px">
                            <p align="left">
                                倒码毛利
                            </p>
                        </td>
                        <td width="80px">
                            <p align="left">
                                消费
                            </p>
                        </td>
                        <td width="80px">
                            <p align="left">
                                未消费
                            </p>
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                    <tr style="display: ;">
                        <td colspan="11">
                            <span>注1：每次查询中的 未消费金额 统计当天销售的票未使用的票， 可能会随着订单验证的增加而变化。</span><br />
                            <span>注2：分销毛利是按实际订单成本汇总，不会因产品成本调整而产生金额变化。</span><br />
                            <span>注3: 每日消费金额 是统计当日的消费 金额。</span>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                   <tr class="fontcolor">
                        <td valign="top">
                            <p align="left">
                             ${daydate}
                            </p>
                        </td>
                              <td valign="top">
                            <p align="left">
                             ${Line_Imprest}
                            </p>
                        </td>
                        <td valign="top">
                            <p align="left">
                            ${Hand_Imprest}
                             </p>
                        </td>
                        <td valign="top">
                            <p align="left">
                                ${Hand_Rebate}
                            </p>
                        </td>

                        <td valign="top">
                            <p align="left">
                                  ${Sale_price}
                            </p>
                        </td>
                         <td valign="top">
                            <p align="left">
                                   ${Maoli_price}
                            </p>
                        </td>
                         <td valign="top">
                            <p align="left">
                                   ${Daoma_Sale_price} 
                            </p>
                        </td>
                         <td valign="top">
                            <p align="left">
                                   ${Daoma_Maoli_price}
                            </p>
                        </td>
                         <td valign="top">
                            <p align="left">
                                   ${Xiaofei_price}
                            </p>
                        </td>
                         <td valign="top">
                            <p align="left">
                                  ${Chendian_price}
                            </p>
                        </td>
                    </tr>
    </script>
    <!--loading层--->
    <div style="height: 565px; display: none;" id="bgDiv">
    </div>
    <div id="loading" style="top: 352px; display: none;">
        <img src="/Images/loading.gif" alt="loading...">
    </div>
</asp:Content>
