<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="yznoticelist.aspx.cs" Inherits="ETS2.WebApp.UI.PermissionUI.yznoticelist"
    MasterPageFile="/UI/Etown.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {
                $($(this)).datepicker();
            });

            SearchList(1, 12);
            $("#Search").click(function () {
                SearchList(1, 12);
            })

            //加载通过接口出票的分销列表
            $.post("/JsonFactory/OrderHandler.ashx?oper=getagentlist", { isapiagent: "1" }, function (data) {
                data = eval("(" + data + ")");
                var str = '<option value="0">全 部</option>';
                if (data.type == 1) { }
                if (data.type == 100) {

                    for (var i = 0; i < data.msg.length; i++) {
                        str += '<option value="' + data.msg[i].Id + '">' + data.msg[i].Company + '</option>';
                    }
                    $("#sel_agentcomid").html(str);
                }
            })
            //加载通过接口售票的商家列表
            $.post("/JsonFactory/OrderHandler.ashx?oper=getcompanylist", { isapicompany: "1" }, function (data) {
                data = eval("(" + data + ")");
                var str = '<option value="0">全 部</option>';
                if (data.type == 1) { }
                if (data.type == 100) {

                    for (var i = 0; i < data.msg.length; i++) {
                        str += '<option value="' + data.msg[i].ID + '">' + data.msg[i].Com_name + '</option>';
                    }
                    $("#sel_comid").html(str);
                }
            })
        })

        function SearchList(pageindex, pagesize) {
            var startime = $("#startime").val();
            var key = $("#key").val();
            var agentcomid = $("#sel_agentcomid").val();
            var comid = $("#sel_comid").val();

            if (pageindex == '') {
                $.prompt("请选择跳到的页数");
                return;
            }
            $.post("/JsonFactory/OrderHandler.ashx?oper=yznoticeloglist", { pageindex: pageindex, pagesize: pagesize, startime: startime, key: key, agentcomid: agentcomid, comid: comid }, function (data) {
                data = eval("(" + data + ")");

                if (data.type == 1) {
                    $("#tblist").empty();
                    //                    $.prompt("查询列表错误");
                    return;
                }
                if (data.type == 100) {
                    $("#tblist").empty();
                    $("#divPage").empty();
                    if (data.totalCount == 0) {

                    } else {
                        $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                        setpage(data.totalcount, pagesize, pageindex);
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

                    SearchList(page, newpagesize);

                    return false;
                }
            });
        }

        document.onkeydown = keyDownSearch;
        function keyDownSearch(e) {
            // 兼容FF和IE和Opera  
            var theEvent = e || window.event;
            var code = theEvent.keyCode || theEvent.which || theEvent.charCode;
            if (code == 13) {
                $("#Search").click(); //具体处理函数  
                return false;
            }
            return true;
        }


        function showDetail(b2b_etcket_logid) {
            $.post("/JsonFactory/OrderHandler.ashx?oper=getNoticesByYzlogid", { b2b_etcket_logid: b2b_etcket_logid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {
                    $("#proqrcode_rhshow").show();
                    $("#Tbody1").empty();
                    $("#Script1").tmpl(data.msg).appendTo("#Tbody1");
                }
            })


        }
        function closedetail() {
            $("#proqrcode_rhshow").hide();
            $("#Tbody1").empty();
        }
        function reissueNotice(noticelogid) {
            if (confirm("确认补发吗?")) {
                $.post("/JsonFactory/OrderHandler.ashx?oper=reissueNotice", { noticelogid: noticelogid }, function (data) {
                    data = eval("(" + data + ")");
                    alert(data.msg);
                    return;
                })
            }
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="FuncModuleList.aspx" onfocus="this.blur()" target=""><span>权限管理</span></a></li>
                <li><a href="RoleList.aspx" onfocus="this.blur()" target=""><span>角色管理</span></a></li>
                <li><a href="SSort.aspx" onfocus="this.blur()" target=""><span>商户管理</span></a></li>
                <li><a href="AgentList.aspx" onfocus="this.blur()" target=""><span>分销商管理</span></a></li>
                <li><a href="ticketlist.aspx" onfocus="this.blur()"><span>退票管理</span></a></li>
                <li><a href="ComFinance.aspx" onfocus="this.blur()" target=""><span>财务对账表</span></a></li>
                <li><a href="Withdraw_handle.aspx" onfocus="this.blur()"><span>提现财务管理</span></a></li>
                <!--<li><a href="bankmanager.aspx" onfocus="this.blur()"><span>绑定银行管理</span></a></li>-->
                <li><a href="Modellist.aspx" onfocus="this.blur()" target=""><span>模板管理</span></a></li>
                <li><a href="ProClass.aspx" onfocus="this.blur()" target=""><span>产品类目</span></a></li>
                <li><a href="posloglist.aspx" onfocus="this.blur()" target=""><span>Pos验证日志</span></a></li>
                <li class="on"><a href="yznoticelist.aspx" onfocus="this.blur()" target=""><span>验证通知日志</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    Pos验证日志</h3>
                <br />
                <div style="text-align: center;">
                    <input class="mi-input" name="startime" id="startime" placeholder="验证时间" value=""
                        isdate="yes" type="text" style="width: 120px;">
                    <input type="text" id="key" style="width: 160px;" placeholder="电子码" class="mi-input">
                    <select id="sel_agentcomid" class="mi-input">
                    </select>
                    <select id="sel_comid" class="mi-input">
                    </select>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px; height: 26px;">
                    </label>
                    <table width="780" border="0">
                        <tr>
                            <td width="100">
                                <p align="left">
                                    电子码
                                </p>
                            </td>
                            <td width="30">
                                <p align="left">
                                    数量
                                </p>
                            </td>
                            <td width="100">
                                <p align="left">
                                    验证时间
                                </p>
                            </td>
                            <td width="100">
                                <p align="left">
                                    通知时间
                                </p>
                            </td>
                            <td width="100">
                                <p align="left">
                                    分销返回描述
                                </p>
                            </td>
                            <td width="100">
                                <p align="left">
                                    分销商
                                </p>
                            </td>
                            <td width="100">
                                <p align="left">
                                    出票商家
                                </p>
                            </td>
                            <td width="50">
                                <p align="left">
                                    是否需补发
                                </p>
                            </td>
                            <td width="100">
                                <p align="center">
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
                        
                         <td  >
                            <p align="left">
                               ${Pno} </p>
                        </td>
                        <td>
                            <p align="left">
                             ${Num}  
                            </p>
                        </td>
                        <td  >
                            <p align="left">
                               ${jsonDateFormatKaler(Confirmtime)}
                            </p>
                        </td>
                       <td  >
                            <p align="left">
                             ${jsonDateFormatKaler(Sendtime)} 
                            </p>
                        </td>
                           <td  >
                            <p align="left" title="${Remark}" >
                               ${RemarkPithyDesc}
                            </p>
                        </td>
                       <td  >
                            <p align="left">
                             ${AgentComName}
                            </p>
                        </td>
                        <td  >
                            <p align="left">
                             ${ComName}
                            </p>
                        </td>
                        <td  >
                            <p align="left">
                             {{if isneedreissue==1}}
                             <lable style="color:blue;">是</lable>
                             {{else}}
                             否
                             {{/if}}
                            </p>
                        </td>
                        <td  >
                        <p align="center"> 
                           <input name="btndetail" type="button"   value="详情" style="width: 60px;
                            height: 26px;" onclick="showDetail('${b2b_etcket_logid}')">
                            
                               <input name="btnreissue" type="button"   value="补发" style="width: 60px;
                            height: 26px;" onclick="reissueNotice('${Id}')"> 
                          
                        </p>
                        </td>
                      
                    </tr>
        </script>
        <script type="text/x-jquery-tmpl" id="Script1">   
                    <tr>
                    <td>
                            <p align="left">
                               ${Id} </p>
                        </td>
                        <td>
                            <p align="left">
                               ${jsonDateFormatKaler(Sendtime)} </p>
                        </td>
                         
                           <td  >
                            <p align="left" title="${Remark}" >
                               ${RemarkPithyDesc}
                            </p>
                        </td>  
                    </tr>
        </script>
        <div id="proqrcode_rhshow" style="background-color: #ffffff; border: 2px solid #5984bb;
            margin: 0px auto; display: none; left: 20%; position: absolute; top: 20%;">
            <table width="500px" border="0" cellpadding="10" cellspacing="1" style="margin: 10px 5px;">
                <tbody id="Tbody1">
                </tbody>
                <tr>
                    <td align="center" bgcolor="#FFFFFF" class="tdHead" colspan="3">
                        <input id="closebtn" type="button" class="formButton" value="  关 闭  " onclick="closedetail()" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
