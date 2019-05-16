<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Manage.Master" CodeBehind="Agent_Com_list.aspx.cs" Inherits="ETS2.WebApp.Agent.Agent_Com_list" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            var agentid = $("#hid_agentid").trimVal();

            SearchList(1);

            //装载列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=agentcomlist",
                    data: { pageindex: pageindex, pagesize: pageSize, agentid: agentid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询渠道列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                $("#tblist").html("<tr><td colspan='15'>查询数据为空</td></tr>");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pageSize, pageindex);
                            }
                        }
                    }
                })


                //查询
                $("#Search").click(function () {
                    var pageindex = 1;
                    var key = $("#key").val();
                    var select = '';

                    if (key == "" || key == null) {
                        $.prompt("查询条件为空");
                        return;
                    }
                    $.ajax({
                        type: "post",
                        url: "/JsonFactory/AgentHandler.ashx?oper=agentcomlist",
                        data: { pageindex: pageindex, pagesize: pageSize, key: key, agentid: agentid },
                        async: false,
                        success: function (data) {
                            data = eval("(" + data + ")");

                            if (data.type == 1) {
                                $.prompt("查询列表错误");
                                return;
                            }
                            if (data.type == 100) {
                                $("#tblist").empty();
                                $("#divPage").empty();
                                if (data.totalCount == 0) {
                                    $("#tblist").html("<tr><td colspan='15'>查询数据为空</td></tr>");
                                } else {
                                    $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                    setpage(data.totalCount, pageSize, pageindex);
                                }
                            }
                        }
                    })
                })

                $("#0").click(function () {
                    select(0, 1);
                    $("#0").css("color", "red");
                })
                $("#1").click(function () {
                    select(1, 1);
                    $("#1").css("color", "red");
                })
                $("#3").click(function () {
                    select(3, 1);
                    $("#3").css("color", "red");
                })
                $("#4").click(function () {
                    select(4, 1);
                    $("#4").css("color", "red");
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
        <div id="secondary-tabs" class="navsetting ">

         <ul class="composetab">
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_sel"><div><a href="Agent_Com_list.aspx">商户列表</a></div></div>
            </li>
             <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_unsel"><div><a href="Agent_Com_Open.aspx">开通新商户</a></div></div>
            </li>
             <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_unsel"><div><a href="AgentFinane.aspx">财务列表</a></div></div>
            </li>

         </ul>
          <div class="toolbg toolbgline toolheight nowrap" style="">
         <div class="right searchtool">
                 <span>&nbsp;</span>   
         </div>
         <div class="nowrap left" unselectable="on" onselectstart="return false;">
        
         
         </div></div>

        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                    <div style="text-align: center;">
                        <label>
                           关键词查询
                            <input name="key" type="text" id="key" style="width: 160px; height: 20px;" />
                        </label>
                        <label>
                            <input name="Search" type="button" id="Search" value="查询" style="width: 120px; height: 26px;" />
                        </label>
                    </div>
                <table width="780" border="0">
                    <tr>
                        <td width="26">
                            <p align="left">
                                序号</p>
                        </td>
                        <td width="260">
                            <p align="left">
                                商户名称</p>
                        </td>
                        <td width="160">
                            <p align="left">
                                开户登陆账户 </p>
                        </td>
                        <td width="60">
                            <p align="left">
                                到期日期
                            </p>
                        </td>
                        <td width="100">
                            <p align="left">
                                商户类型</p>
                        </td>
                         <td width="100">
                            <p align="left">
                              状态  </p>
                        </td>
                        <td width="100">
                            
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
                        <td >
                            <p align="left">
                               ${Id}</p>
                        </td>
                        <td >
                            <p align="left" >
                               ${Comname}</p>
                        </td>
                        <td >
                            <p align="left">
                            ${Accountinfo}
                            </p>
                        </td>
                        <td >
                            <p align="left">
                             {{if Agentopenstate==1}}
                             ${ChangeDateFormat(EndData)}
                              {{else}}
                              --
                            {{/if}}
                            </p>
                        </td>
                        <td >
                            <p align="left">
                            ${HasInnerChannel}
                            </p>
                        </td>
                                                <td >
                            <p align="left"> 
                            {{if Agentopenstate==1}}
                                ${Com_state}
                            {{else}}
                                <a href="Agent_com_open.aspx?comaccount=${Accountinfo}">未开通</a>
                            {{/if}}
                             </p>
                        </td>
                        <td >
                            
                        </td>
                    </tr>
                    
    </script>
    <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
</asp:Content>
