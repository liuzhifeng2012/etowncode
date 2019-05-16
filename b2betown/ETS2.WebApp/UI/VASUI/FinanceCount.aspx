<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="FinanceCount.aspx.cs" Inherits="ETS2.WebApp.UI.VASUI.FinanceCount" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
        <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script> 
    <script type="text/javascript">
        var pageSize = 15; //每页显示条数

        $(function () {
            //日历
            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {
                $($(this)).datepicker();
            });


            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();
            SearchList(1);


            $("#Search").click(function () {
                SearchList(1);
            })


            //装载财务列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                var stardate = $("#startime").val();
                var enddate = $("#endtime").val();

                if (stardate == "" || enddate == "") {
                    $("#tblist").html("请选择查询日期；");
                    return;
                }


                $.ajax({
                    type: "post",
                    url: "/JsonFactory/FinanceHandler.ashx?oper=Financecount",
                    data: { comid: comid, stardate: stardate, enddate: enddate },
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
        })
    </script>
  
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">

        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">

                <%--<h3>
                    收支明细</h3>--%>
                    <div style="float:right;padding-right:110px;"> 
                   <label>
                        <input class="mi-input" name="startime" id="startime" placeholder="开始时间" value=""  isdate="yes" type="text" style="width: 120px;">-
                        <input class="mi-input" name="endtime" id="endtime" placeholder="结束时间" value="" isdate="yes"  type="text" style="width: 120px;">
                    </label>
                     
                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px;
                            height: 26px;">
                    </label>
                 
                </div>


                <table width="780" border="0">
                    <tr>
                        <td width="51">
                            <p align="left">
                               收支渠道 </p>
                        </td>
                        <td width="60">
                            <p align="left">
                                收支类型
                            </p>
                        </td>
                        <td width="244">
                            <p align="left">
                                金额
                            </p>
                      </td>
                       
                      <td width="72">
                            <p align="left">&nbsp;
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
                                ${Money_come}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${Payment_type}
                            </p>
                        </td>
                        <td >
                            <p align="left">
                                ${Money}</p>
                        </td>
                       <td >
                            <p align="left">
                               </p>
                        </td>
                    </tr>
    </script>
</asp:Content>
