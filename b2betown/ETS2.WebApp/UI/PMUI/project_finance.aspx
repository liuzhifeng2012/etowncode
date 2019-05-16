<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="project_finance.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.project_finance" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script> 
    <script type="text/javascript">


        $(function () {
            var projectid = $("#hid_projectid").trimVal();
            var comid = $("#hid_comid").trimVal();

            //日历
            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {
                $($(this)).datepicker();
            });

            SearchList(1, 10);

            function SearchList(pageindex, pagesize) {
                var startime = $("#startime").trimVal();
                var endtime = $("#endtime").trimVal();
                $.post("/JsonFactory/ProductHandler.ashx?oper=projectfinancepagelist", { comid: comid, projectid: projectid, pageindex: pageindex, pagesize: pagesize, startime: startime, endtime: endtime }, function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        //$.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $("#tblist").empty();
                        $("#divPage").empty();
                        if (data.totalcount == 0) {
                            //$("#tblist").html("查询数据为空");
                        } else {
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                            setpage(data.totalcount, pagesize, pageindex);
                        }
                    }
                })

                $.post("/JsonFactory/ProductHandler.ashx?oper=projectfinancesum", { comid: comid, projectid: projectid, startime: startime, endtime: endtime }, function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        //$.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $("#fsum").html(data.msg);
                        $("#yanpiao").html(data.yanpiaoprice);
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

                        SearchList(page, newpagesize);
                        return false;
                    }
                });
            }


            $("#Search").click(function () {
                SearchList(1, 10);
             })

            $("#sub_endtime").click(function () {
                var id = $("#hid_id").val();
                var Money = $("#Money").val();
                var Remarks = $("#Remarks").val();

                $.post("/JsonFactory/ProductHandler.ashx?oper=upprojectfinance", { id: id, comid: $("#hid_comid").val(), Money: Money, Remarks: Remarks, projectid: projectid }, function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        alert(" 操作成功！");
                        window.location.reload();
                    }
                })
            })

        })

        function addtmp(tmpid) {
            $("#orderinfo").show();
            $("#hid_id").val(tmpid);
            $("#costprice").val("");
            $("#stardate").val("");
            $("#enddate").val("");
        }


    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                  <%=projectname%> - 结算金额列表</h3>
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

                <h4 style="float: left">
                    <a style="" href="javascript:;" onclick="addtmp(0);" class="a_anniu">录入新结算金额</a>
                </h4>
                <div class="left">已结算金额：<span id="fsum">0</span>元  已验证金额 <span id="yanpiao">0</span>元 </div>
                <table width="780" border="0">
                    <tr>
                        <td width="50px">
                            <p align="left">
                                ID</p>
                        </td>
                        <td width="20%">
                            <p align="left">
                                结算金额
                            </p>
                        </td>
                        <td width="20%">
                            <p align="left">
                                操作日期
                            </p>
                        </td>
                        <td width="20%">
                            <p align="left">
                                备注
                            </p>
                        </td>
                        <td width="20%">
                            <p align="left">
                                操作人
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                 &nbsp;</p>
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
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr>
                        <td>
                            <p>${id}</p>
                        </td>
                        <td>
                            <p>${Money}</p>
                        </td>
                        <td>
                            <p>${ChangeDateFormat(Subdate)}</p>
                        </td>
                        <td>
                            <p>${Remarks}</p>
                        </td>
                      <td>
                            <p>${admin}</p>
                        </td>
                        <td>
                            <p>  </p>
                        </td>
                    </tr>
    </script>
     <div id="orderinfo" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px 300px;
        width: 400px; height: auto; display: none; left: 20%; position: absolute; top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    打款金额：<input type="text" id="Money" value="" style="width: 120px;" />
                </td>
            </tr>
             <tr>
                <td height="20" bgcolor="#C1D9F3" class="tdHead" style="padding-left: 10px; font-size: 12px;">
                    打款备注：<input type="text" id="Remarks"  value="" style="width: 120px;" />
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input id="sub_endtime" name="sub_endtime" type="button" class="formButton" value="  确认  " />
                    <input name="cancel_endtime" type="button" onclick="$('#orderinfo').hide();"
                        value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>

    <input type="hidden" id="hid_projectid" value="<%=projectid %>" />
</asp:Content>