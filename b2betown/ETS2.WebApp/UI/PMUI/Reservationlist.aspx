<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Etown.Master" AutoEventWireup="true"
    CodeBehind="Reservationlist.aspx.cs" Inherits="ETS2.WebApp.UI.Reservationlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        var pageSize = 10; //每页显示条数
        $(function () {

            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();

            SearchList(1);

            //活动加载明细列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/CrmMemberHandler.ashx?oper=ResLoadingList",
                    data: { userid: userid, comid: comid, pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
//                            $.prompt("查询数据出现错误！");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                //                                $("# ").html("没有查到预订信息。");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pageSize, pageindex);
                            }
                        }
                    }
                })
            }

            $("#cancel_luru").click(function () {
                $("#queren").hide();
            })
            $("#submit_luru").click(function () {
                var id = $("#hid_id").val();
                var userid = $("#hid_userid").trimVal();
                var comid = $("#hid_comid").trimVal();
                var beiz = $("#eticketkey").val();
                $.post("/JsonFactory/CrmMemberHandler.ashx?oper=upRes", { id: id, userid: userid, comid: comid, beiz: beiz }, function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt("操作数据出现错误");
                        return;
                    }
                    if (data.type == 100) {
                        if (data.msg == 0) {
                            $.prompt("参数传递出错，请重新操作");
                            return;
                        } else if (data.msg != 0) {
                            $.prompt("预订单确认成功");
                            location.reload();
                            return;
                        }
                    }
                })
            })

            //搜索列表
            $("#Search").click(function () {
                var pageindex = 1;
                var key = $("#key").val();
                var ServerName = $("#ServerName").val();

                if (key == "") {
                    $.prompt("查询关键词不能为空");
                    return;
                }

                $.ajax({
                    type: "post",
                    url: "/JsonFactory/CrmMemberHandler.ashx?oper=ResSearchList",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, key: key, ServerName: ServerName },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
//                            $.prompt("查询预约数据出现错误！");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                //                                $("#tblist").html("没有查到预约信息。");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pageSize, pageindex);
                            }


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
        })

        function UpRes(id) {
            $("#queren").show();
            $("#hid_id").val(id);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
     <%--   <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/pmui/order/orderlist.aspx" onfocus="this.blur()" target="">订单列表</a></li>
                <li class="on"><a href="/ui/pmui/Reservationlist.aspx" onfocus="this.blur()" target="">
                    订房订单列表</a></li>
                <li><a href="/ui/MemberUI/ChannelFinance.aspx" onfocus="this.blur()">门票返佣 </a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                </h3>
                <div style="text-align: center;">
                    <label>
                        请输入(手机，标题，姓名)
                        <input name="key" type="text" id="key" style="width: 160px; height: 20px;">
                    </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询预订" style="width: 120px;
                            height: 26px;">
                    </label>
                </div>
                <table width="780" border="0">
                    <tr>
                        <td width="4%" height="42">
                            使用ID
                        </td>
                        <td width="6%">
                            姓名
                        </td>
                        <td width="8%">
                            电话
                        </td>
                        <td width="16%">
                            标题
                        </td>
                        <td width="6%">
                            数量
                        </td>
                        <td width="6%">
                            操作人
                        </td>
                        <td width="6%">
                            状态
                        </td>
                        <td width="10%">
                            预订时间
                        </td>
                        <td width="10%">
                            提交时间
                        </td>
                        <td width="8%">
                            ip
                        </td>
                        <td width="8%">
                            总金额
                        </td>
                        <td width="6%">
                            管理
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr>
                        <td >
                            <p align="left">
                                ${ID}</p>
                        </td>
                        <td>
                            ${Name}
                        </td>
                        <td>${Phone}</td>
                        <td>
                                ${Title}
                        </td>
                        <td>
                            ${Number} 
                        </td>
                        <td>
                            ${Submit_name} 
                        </td>
                        <td>
                            ${Rstatic}
                        </td>
                        <td >
                                ${jsonDateFormatKaler(Resdate)}
                        </td>
                        <td >
                                ${jsonDateFormatKaler(Datetime)}
                        </td>
                        <td >
                               ${Ip}
                        </td>
                        <td>
                              ${Money}
                        </td>
                        <td width="6%">
                         {{if Rstatic=="已处理"}}已确认 {{/if}} {{if Rstatic=="未处理"}}<input type="button" onclick="javascript:UpRes('${ID}')"  value="确认预订"/> {{/if}}
                        </td>
                    </tr>
                    </script>
                </table>
                <div id="divPage">
                </div>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <div id="queren" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px auto;
        width: 400px; height: 300px; display: none; left: 20%; position: absolute; top: 20%;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span style="padding-left: 10px; font-size: 18px;" id="lurutitle">酒店预订处理</span>
                </td>
            </tr>
            <tr>
                <td height="30" align="right" bgcolor="#E7F0FA" class="tdHead">
                    备注:
                </td>
                <td height="30" bgcolor="#E7F0FA" class="tdHead" id="td1">
                    <textarea name="eticketkey" id="eticketkey" cols="40" rows="10" style="width: 355px;
                        height: 200px;"></textarea>
                </td>
            </tr>
            <tr>
                <td height="38" colspan="2" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input type="hidden" id="hid_luru_proid" value="" />
                    <input name="submit_luru" id="submit_luru" type="button" class="formButton" value="  确  定  " />
                    <input name="cancel_luru" id="cancel_luru" type="button" class="formButton" value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>
    <input id="hid_id" type="hidden" value="" />
</asp:Content>
