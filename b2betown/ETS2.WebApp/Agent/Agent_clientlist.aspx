<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Manage.Master" CodeBehind="Agent_clientlist.aspx.cs"
    Inherits="ETS2.WebApp.Agent.Agent_clientlist" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <title>分销客户信息</title>
<script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            var agentid = $("#hid_agentid").trimVal();

            SearchList(1);

            function SearchList(pageindex) {
                var key = $("#key").trimVal();
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }

                $.post("/JsonFactory/AgentHandler.ashx?oper=getclientlistbyagentid", { pageindex: pageindex, pagesize: pageSize, agentid: agentid, key: key }, function (data) {

                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        //                        $.prompt("查询渠道列表错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#tblist").empty();
                        $("#divPage").empty();
                        if (data.totalCount == 0) {
                            $("#tblist").html("<tr><td colspan='3'>查询数据为空</td></tr>");
                        } else {
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                            setpage(data.totalcount, pageSize, pageindex);
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
            $("#Proinfocancel").click(function () {
                $("#ProInfo").hide();
            })
            $("#closeProInfo").click(function () {
                $("#ProInfo").hide();
            })


        })

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
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
                        <div id="settings" class="view main">
                            <div id="secondary-tabs" class="navsetting ">
                             <ul class="composetab">
                                            <li class="left" style="width:110px;padding-right:2px;">
                                                <div class="composetab_img"></div>
                                                <div class="composetab_sel"><div><a href="Agent_clientlist.aspx">客户信息</a></div></div>
                                            </li>
                                            <li class="left" style="width:110px;padding-right:2px;">
                                                <div class="composetab_img"></div>
                                                <div class="composetab_unsel"><div><a href="Agent_commonaddresslist.aspx">常用客户管理</a></div></div>
                                            </li>
                                         </ul>
                                          <div class="toolbg toolbgline toolheight nowrap" style="">
                                 <div class="right">
                                             <label>
                                            关键词查询
                                            <input name="key" type="text" id="key" style="width: 160px; height: 20px;" placeholder="客户姓名、客户手机" />
                                        </label>
                                        <label>
                                            <input name="Search" type="button" id="Search" value="查询" style="width: 120px; height: 26px;" />
                                        </label>
                                 </div>
         <div class="nowrap left" unselectable="on" onselectstart="return false;">
         <!--<a class="btn_gray btn_space" hidefocus="" id="quick_del" href="javascript:;" name="del">删除</a>-->
         
         
         </div></div>

                            </div>
                            <div id="setting-home" class="vis-zone mail-list">
                                <div class="inner">
                                    <table width="780" border="0" class="O2">
                                        <tr class="O2title">
                                            <td width="50">
                                                <p align="left">
                                                    客户姓名</p>
                                            </td>
                                            <td width="40">
                                                <p align="left">
                                                    客户电话</p>
                                            </td>
                                            <td width="330">
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
                        <script type="text/x-jquery-tmpl" id="ProductItemEdit">
   
                <tr class="d_out" onmouseover="this.className='d_over'" onmouseout="this.className='d_out'">
                        <td >  
                                    <p>${U_name} </p> 
                        </td>
                        <td  >
                             <p>${U_phone} </p> 
                               
                        </td>
                         
                       
                        <td >
                            <p align="left">
                               </p>
                        </td>
                    </tr>
               
                    
                        </script>
                        <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
                    
    <input type="hidden" id="hid_accountid" value="<%=accountid %>" />
    <input type="hidden" id="hid_accountlevel" value="<%=AccountLevel %>" />
</asp:Content>

