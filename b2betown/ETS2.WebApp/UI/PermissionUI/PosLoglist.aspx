<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PosLoglist.aspx.cs" Inherits="ETS2.WebApp.UI.PermissionUI.PosLoglist"
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

            SearchList(1, 12, "", "");
            $("#Search").click(function () {
                var startime = $("#startime").val();
                var key = $("#key").val();

                SearchList(1, 12, startime, key);
            })
          


        })

        function SearchList(pageindex, pagesize, startime, key) {
            if (pageindex == '') {
                $.prompt("请选择跳到的页数");
                return;
            }
            $.post("/JsonFactory/OrderHandler.ashx?oper=posloglist", { pageindex: pageindex, pagesize: pagesize, startime: startime, key: key }, function (data) {
                data = eval("(" + data + ")");

                if (data.type == 1) {
                    //                    $.prompt("查询列表错误");
                    return;
                }
                if (data.type == 100) {
                    $("#tblist").empty();
                    $("#divPage").empty();
                    if (data.totalCount == 0) {

                    } else {
                        $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                        setpage(data.totalcount, pagesize, pageindex, startime, key);
                    }


                }
            })
        }
        //分页
        function setpage(newcount, newpagesize, curpage, startime, key) {
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

                    SearchList(page, newpagesize, startime, key);

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


        function showdetail(logid) {
            $.post("/JsonFactory/OrderHandler.ashx?oper=getposlogbyid", {logid:logid}, function (data) {
                data = eval("(" + data + ")");
                if(data.type==1){}
                if(data.type==100){
                    $("#proqrcode_rhshow").show();
                    $("#lbl_rqxml").text(data.msg.Str);
                    $("#lbl_rpxml").text(data.msg.ReturnStr);
                }
            })

            
        }
        function closedetail() 
        { 
            $("#proqrcode_rhshow").hide();
            $("#lbl_rqxml").html("");
            $("#lbl_rpxml").html("");
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
                <li class="on"><a href="ProClass.aspx" onfocus="this.blur()" target=""><span>Pos验证日志</span></a></li>
               <li><a href="yznoticelist.aspx" onfocus="this.blur()" target=""><span>验证通知日志</span></a></li>
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
                    <input type="text" id="key" style="width: 160px;" placeholder="电子码、POSID" class="mi-input">
                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px; height: 26px;">
                    </label>
                    <table width="780" border="0">
                        <tr>
                            <td width="60">
                                <p align="left">
                                    POSID
                                </p>
                            </td>
                              <td width="100">
                                <p align="left">
                                    电子码
                                </p>
                            </td>
                            <td width="60">
                                <p align="left">
                                    操作类型
                                </p>
                            </td>
                            <td width="157">
                                <p align="left">
                                    发送时间
                                </p>
                            </td>
                            <td width="40">
                                <p align="left">
                                    返回状态
                                </p>
                            </td>
                            <td width="257">
                                <p align="left">
                                    描述
                                </p>
                            </td>
                            <td width="157">
                                <p align="left">
                                    客户端IP
                                </p>
                            </td>
                            <td width="60">
                                <p align="center">
                                    详 情
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
                            <p align="left" title="${Posdetail}">
                              <a href="javascript:void(0)" style="text-decoration:underline;"> ${Posid} </a></p>
                        </td>
                         <td  >
                            <p align="left">
                               ${Pno} </p>
                        </td>
                        <td>
                            <p align="left">
                             ${Posopertype}  
                            </p>
                        </td>
                        <td  >
                            <p align="left">
                               ${jsonDateFormatKaler(Subdate)}
                            </p>
                        </td>
                       <td  >
                            <p align="left">
                             ${PosRetstatus}
                            </p>
                        </td>
                           <td  >
                            <p align="left" title="${PosReturnsinfo}">
                               ${PosReturnsinfo}
                            </p>
                        </td>
                       <td  >
                            <p align="left">
                             ${Uip}
                            </p>
                        </td>
                       
                        <td  >
                        <p align="center">
                           <a href="javascript:void(0)" onclick="showdetail('${Id}')" style="text-decoration:underline;">点击查看详情</a>
                        </p>
                        </td>
                      
                    </tr>
        </script>
        <div id="proqrcode_rhshow" style="background-color: #ffffff; border: 2px solid #5984bb;
            margin: 0px auto; display: none; left: 20%; position: absolute; top: 20%;">
            <table width="500px" border="0" cellpadding="10" cellspacing="1" style="margin: 10px 5px;">
                <tr>
                    <td   bgcolor="#FFFFFF" class="tdHead">
                        <label>
                            传递xml:
                        </label>
                    </td>
                </tr>
                <tr>
                    <td  bgcolor="#FFFFFF" class="tdHead">
                        <label id="lbl_rqxml">
                        </label>
                    </td>
                </tr>
                <tr>
                    <td   bgcolor="#FFFFFF" class="tdHead">
                        <label>
                            返回xml:
                        </label>
                    </td>
                </tr>
                <tr>
                    <td   bgcolor="#FFFFFF" class="tdHead">
                        <label id="lbl_rpxml">
                        </label>
                    </td>
                </tr>
                <tr>
                    <td  align="center" bgcolor="#FFFFFF" class="tdHead">
                        <input id="closebtn" type="button" class="formButton" value="  关 闭  "  onclick="closedetail()"/>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
