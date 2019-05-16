<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ServerFakaStat.aspx.cs"
    Inherits="ETS2.WebApp.UI.PMUI.ServerFakaStat" MasterPageFile="/UI/Etown.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
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

            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();

            SearchList(1, 10);

            $("#Search").click(function () {
                SearchList(1, 10);
            })
            $("#cancel_rh").click(function () {
                $("#zhajlist").html("");
                $("#rhshow").hide();
            })
        })
        function zhajilog(id) {
            $("#rhshow").show();
            $("#zhajilist").empty();
            $.post("/JsonFactory/ProductHandler.ashx?oper=zhajilogPagelist", { comid: $("#hid_comid").trimVal(), id: id }, function (data) {
                data = eval("(" + data + ")");

                if (data.type == 1) {
                }
                if (data.type == 100) {
                   
                    $("#zhajiItemEdit").tmpl(data.msg).appendTo("#zhajilist");
                }
            })
        }

        function tuiyajin(id) {
            $.post("/JsonFactory/ProductHandler.ashx?oper=tuiyajin", { comid: $("#hid_comid").trimVal(), id: id }, function (data) {
                data = eval("(" + data + ")");

                if (data.type == 1) {
                    alert("退款失败，"+data.msg);
                    return;
                }
                if (data.type == 100) {

                    alert("退款成功");
                    return;
                }
            })
        }
     


        function SearchList(pageindex, pagesize) {
            var comid = $("#hid_comid").trimVal();
            var startime = $("#startime").trimVal();
            var endtime = $("#endtime").trimVal();
            var key = $("#key").trimVal();
            var serverstate = $("#sel_serverstate").trimVal();
            var serverid = $("#sel_serverid").trimVal();

            if (startime != "" || endtime != "") {
                if (startime == "" || endtime == "") {
                    alert("开始时间和结束时间需要同时选择");
                    return;
                }
            }




            $.post("/JsonFactory/ProductHandler.ashx?oper=serverfakaPagelist", { comid: comid, pageindex: pageindex, pagesize: pagesize, startime: startime, endtime: endtime, key: key, serverstate: serverstate, serverid: serverid }, function (data) {
                data = eval("(" + data + ")");

                if (data.type == 1) {
                    //                        $.prompt(data.msg);
                    $("#tblist").empty();
                    $("#divPage").empty();
                    return;
                }
                if (data.type == 100) {
                    $("#tblist").empty();
                    $("#divPage").empty();
                    if (data.totalcount == 0) {
                    } else {
                        $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                        setpage(data.totalcount, pagesize, pageindex);
                    }
                    $("#totalfaka").text(data.fakatotal);
                    $("#totalnotlingqu").text(data.notlingqu);
                    $("#totalguihuan").text(data.guihuan);
                    $("#totalnotguihuan").text(data.notguihuan);

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


        //分页
        function setpage_sml(newcount, newpagesize, curpage) {
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

                    zhajilog(page, newpagesize);

                    return false;
                }
            });
        }




    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div class="navsetting ">
            <ul class="composetab">
                <li style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/serverlist.aspx">终端服务管理</a></div>
                    </div>
                </li>
              <%--  <li style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/ServerPrintSuodao.aspx">索道票打印统计</a></div>
                    </div>
                </li>--%>
                <li class="on" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/ServerFakaStat.aspx">发卡记录</a></div>
                    </div>
                </li>
<%--                <li style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/ServerFakaCount.aspx">发卡统计</a></div>
                    </div>
                </li>--%>
                <li style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/ServerxiaojianCount.aspx">小件统计</a></div>
                    </div>
                </li>

<%--                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/ServerTimeoutStat.aspx">归还超时统计</a></div>
                    </div>
                </li>--%>
               <%-- <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_sel">
                        <div>
                            <a href="/ui/pmui/servercardinputlist.aspx">服务卡管理</a></div>
                    </div>
                </li>
                <li class="left" style="width: 110px; padding-right: 2px;">
                    <div class="composetab_img">
                    </div>
                    <div class="composetab_unsel">
                        <div>
                            <a href="/ui/pmui/servercardinput.aspx">录入服务卡</a></div>
                    </div>
                </li>--%>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h4>
                    <label>
                        <select id="sel_serverid">
                            <option value="0">全部服务</option>
                        </select>
                    </label>
                    <label>
                        <select id="sel_serverstate">
                            <option value="-1">全部状态</option>
                            <option value="1" selected>未完成</option>
                            <option value="2">已完成/有效期截止</option>
                        </select>
                    </label>
                    <label>
                        <input class="mi-input" name="startime" id="startime" placeholder="发卡开始时间" value=""
                            isdate="yes" type="text" style="width: 120px;">-
                        <input class="mi-input" name="endtime" id="endtime" placeholder="结束时间" value="" isdate="yes"
                            type="text" style="width: 120px;">
                    </label>
                    <label>
                        关键字
                        <input name="key" type="text" id="key" class="mi-input" style="width: 280px;" placeholder="用户手机、订单号、产品名称" />
                    </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px; height: 26px;" />
                    </label>
                </h4>
                <br />
                <h4 style="float: right;">
                    <label>
                        发卡数量:<span id="totalfaka"></span> &nbsp;归还数量:<span id="totalguihuan"></span>&nbsp;未归还数量:<span
                            id="totalnotguihuan"></span>&nbsp;未领取数量:<span id="totalnotlingqu"></span></label>
                </h4>
                <table width="780" border="0">
                    <tr>
                        <td width="6%">
                            <p align="left">
                                订单号
                            </p>
                        </td>
                        <td width="20%">
                            <p align="left">
                                产品名称
                            </p>
                        </td>
                        <td width="10%">
                            <p align="left">
                                发卡时间</p>
                        </td>
                        <td width="10%">
                            <p align="left">
                                结束时间
                            </p>
                        </td>
                        <td width="10%">
                            <p align="left">
                                用户电话
                            </p>
                        </td>
                        <td width="20%">
                            <p align="left">
                                状态
                            </p>
                        </td>
                        <td width="10%">
                            <p align="left">
                                印刷id
                            </p>
                        </td>
                        <td width="10%">
                            <p align="left">
                                管理
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
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr>
                        <td>
                            <p><a href="#">${oid}</a></p>
                        </td>
                        <td>
                            <p>${pname}</p>
                        </td>
                        <td>
                            <p>${jsonDateFormat(subtime)}</p>
                        </td>
                       <td>
                            <p>${jsonDateFormat(endtime)}</p>
                        </td>
                        <td>
                            <p>${userphone}</p>
                        </td>
                        <td>
                            <p>${serverstatedesc}

                                {{each(i,user) xueju}}
                                    <br>${user.servername}.
                                    {{if user.Verstate==0}}
                                      未领取
                                    {{/if}}
                                    {{if user.Verstate==1}}
                                      已领取
                                    {{/if}}
                                    {{if user.Verstate==2}}
                                      已归还
                                    {{/if}}
                                {{/each}}

                              {{if backstate !=null}}
                              {{each(i,user) backstate}}
                                    {{if user.Backdepositstate==0}}

                                    {{if serverstate==2}}
                                       <a class="a_anniu" href="javascript:;" onclick="tuiyajin('${eticketid}')">押金未退</a>
                                    {{else}}
                                       <a class="a_anniu" href="javascript:;" onclick="tuiyajin('${eticketid}')">服务未完成，强制退押金</a>
                                    {{/if}}

                                    {{/if}}
                                {{/each}}


                              {{/if}}
                             </p>
                        </td>
                           <td title="${cardchipid}">
                            <p> ${printid} 
                            </p>
                        </td> 
                        <td>
                            <p>
                            <a class="a_anniu" href="javascript:;" onclick="zhajilog('${id}')"> 查看刷卡记录</a>
                             </p>
                        </td>
                    </tr>
    </script>

    <script type="text/x-jquery-tmpl" id="zhajiItemEdit">   
                    <tr>
                        <td>
                            <p>${id}</p>
                        </td>
                        <td>
                            <p>${jsonDateFormat(subtime)}</p>
                        </td>
                        <td>
                            <p>${pos_id}</p>
                        </td>
                           <td>
                            <p> ${clearchipid} 
                            </p>
                        </td> 
                    </tr>
    </script>
    <div id="rhshow" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px auto;
        width: 600px; height: 320px; position: absolute; z-index: 1003;  left: 20%; top: 20%; background-color:#E7F0FA; display: none;overflow:auto;  ">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
                        <tr>
                <td height="38" align="center" bgcolor="#FFFFFF" class="tdHead">
                    <input name="cancel_rh" id="cancel_rh" type="button" class="formButton" value="  关闭  " />
                </td>
            </tr>
            <tr>
                <td  bgcolor="#E7F0FA" class="tdHead">
                     <table width="560" border="1">
                    <tr>
                        <td width="6%">
                            <p align="left">
                                ID
                            </p>
                        </td>
                        <td width="20%">
                            <p align="left">
                                刷卡时间
                            </p>
                        </td>
                        <td width="20%">
                            <p align="left">
                                POSID
                            </p>
                        </td>
                        <td width="20%">
                            <p align="left">
                                芯片ID</p>
                        </td>
                    </tr>
                    <tbody id="zhajilist">
                    </tbody>
                </table>
                </td>
            </tr>

        </table>
    </div>


</asp:Content>
