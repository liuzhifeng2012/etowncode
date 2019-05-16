<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Manage.Master" CodeBehind="Agent_commonaddresslist.aspx.cs"
    Inherits="ETS2.WebApp.Agent.Agent_commonaddresslist" %>


<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
   <title>常用客户管理</title>
<script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(function () {
            var agentid = $("#hid_agentid").trimVal();

            SearchList(1);

            function SearchList(pageindex) {

                $.post("/JsonFactory/OrderHandler.ashx?oper=addresspagelist", { pageindex: pageindex, pagesize: 10, agentid: agentid }, function (data) {

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
                            setpage(data.totalCount, 10, pageindex);
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
                            <div id="secondary-tabs" class="navsetting ">
                                 <ul class="composetab">
                                            <li class="left" style="width:110px;padding-right:2px;">
                                                <div class="composetab_img"></div>
                                                <div class="composetab_unsel"><div><a href="Agent_clientlist.aspx">客户信息</a></div></div>
                                            </li>
                                            <li class="left" style="width:110px;padding-right:2px;">
                                                <div class="composetab_img"></div>
                                                <div class="composetab_sel"><div><a href="Agent_commonaddresslist.aspx">常用客户管理</a></div></div>
                                            </li>
                                         </ul>
                                          <div class="toolbg toolbgline toolheight nowrap" style="">
                                 <div class="right">
                                        
                                 </div>
         <div class="nowrap left" unselectable="on" onselectstart="return false;">
         <a class="btn_gray btn_space" hidefocus="" id="quick_add" href="/Agent/Agent_commonaddressedit.aspx" name="add">新增常用客户</a>
         
         
         </div></div>
                            </div>
                            <div id="setting-home" class="vis-zone mail-list">
                                <div class="inner"> 
                                    <table width="780" border="0" class="O2">
                                        <tr class="O2title">
                                            <td width="50" height="30">
                                                <p align="left" style="padding-left: 5px;">
                                                    姓名
                                                </p>
                                            </td>
                                            <td width="60">
                                                <p align="left">
                                                    手机
                                                </p>
                                            </td>
                                            <td width="120">
                                                <p align="left">
                                                    城市
                                                </p>
                                            </td>
                                            <td width="350">
                                                <p align="left">
                                                    地址
                                                </p>
                                            </td>
                                            <td width="100">
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
                    <tr   class="d_out" onmouseover="this.className='d_over'" onmouseout="this.className='d_out'">
                        <td>
                            <p align="left" style=" padding-left:5px;">
                                ${U_name}
                            </p>
                        </td>
                         <td>
                            <p align="left">
                                ${U_phone}
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                ${Province}/${City}</p>
                        </td>
                        <td>
                            <p align="left">
                                ${Address}(${Code})</p>
                        </td>
                        <td>
                            <p align="left" id="agent${Agentid}">
                             <a href="/Agent/Agent_commonaddressedit.aspx?addrid=${Id}" class="a_anniu">编辑</a>
  <a href="javascript:void(0)" class="a_anniu" onclick="deladdress('${Id}')">删除</a>
                             
                            </p>
 
                        </td>
                    </tr>
                        </script>
                        <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
                    
    <input type="hidden" id="hid_accountid" value="<%=accountid %>" />
    <input type="hidden" id="hid_accountlevel" value="<%=AccountLevel %>" />
</asp:Content>
