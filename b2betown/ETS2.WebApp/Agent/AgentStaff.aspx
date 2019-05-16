<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Manage.Master"
    CodeBehind="AgentStaff.aspx.cs" Inherits="ETS2.WebApp.Agent.AgentStaff" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            var agentid = $("#hid_agentid").trimVal();
            var accountlevel = $("#hid_accountlevel").trimVal();
            if (accountlevel == 0) {
                $("#secondary-tabs").find("li").eq(1).show();
            }

            SearchList(1);

            //装载产品列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=Accountlist",
                    data: { pageindex: pageindex, pagesize: pageSize, agentid: agentid, accountid: $("#hid_accountid").trimVal() },
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
                <div class="composetab_sel"><div><a href="AgentStaff.aspx">员工管理</a></div></div>
            </li>
             <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_unsel"><div><a href="AgentCompany.aspx">分销商信息</a></div></div>
            </li>
             <li class="left" style="width:110px;padding-right:2px;" id="taobaoset">
                <div class="composetab_img"></div>
                <div class="composetab_unsel"><div><a href="Agent_OpenTaobao.aspx">淘宝店铺</a></div></div>
            </li>

         </ul>
          <div class="toolbg toolbgline toolheight nowrap" style="">
         <div class="right searchtool">
                 <span>&nbsp;</span>   
         </div>
         <div class="nowrap left" unselectable="on" onselectstart="return false;">
         <a class="btn_gray btn_space" hidefocus="" id="quick_add" href="AgentStaff_Manage.aspx" name="add">添加新员工</a>
         
         </div></div>

        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <table width="780" border="0">
                    <tr>
                        <td width="26">
                            <p align="left">
                                序号</p>
                        </td>
                        <td width="100">
                            <p align="left">
                                账户</p>
                        </td>
                        <td width="100">
                            <p align="left">
                                姓名
                            </p>
                        </td>
                        <td width="100">
                            <p align="left">
                                手机
                            </p>
                        </td>
                        <td width="100">
                            <p align="left">
                                授权额度
                            </p>
                        </td>
                        <td width="100">
                            管理
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
                               ${Account}</p>
                        </td>
                        <td >
                            <p align="left">
                               ${Contentname}</p>
                        </td>
                        <td >
                            <p align="left">
                               ${Mobile}</p>
                        </td>
                        <td >
                            <p align="left">
                               {{if AccountLevel==0}} 开户账户 {{else}} 员工账户{{/if}}</p>
                        </td>
                        <td >
                             <a href="AgentStaff_Manage.aspx?id=${Id}" class="a_anniu">管理账户</a>
                        </td>
                    </tr>
                    
    </script>
    <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
     
</asp:Content>
