<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="/Agent/Manage.Master" CodeBehind="Project.aspx.cs" Inherits="ETS2.WebApp.Agent.Project" %>


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
                var key = $("#key").val();
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=projectlist",
                    data: { pageindex: pageindex, pagesize: pageSize, agentid: agentid, key: key },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询错误");
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

            //查询
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
        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
           <ul class="composetab">
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_sel"><div><a href="ProjectList.aspx">项目列表</a></div></div>
            </li>
         </ul>
          <div class="toolbg toolbgline toolheight nowrap" style="">
         <div class="right">
                     
         </div>
         <div class="nowrap left" unselectable="on" onselectstart="return false;">
         <!--<a class="btn_gray btn_space" hidefocus="" id="quick_del" href="javascript:;" name="del">删除</a>-->
         
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
                        <td width="260">
                            <p align="left">
                                项目名称</p>
                        </td>
                        <td width="60">
                            <p align="left">
                                所有验证数</p>
                        </td>
                        <td width="60">
                            <p align="left">
                                上月验证数</p>
                        </td>
                        <td width="60">
                            <p align="left">
                                本月验证数</p>
                        </td>
                        <td width="60">
                            <p align="left">
                                昨天验证数</p>
                        </td>
                        <td width="60">
                            <p align="left">
                                今天验证数</p>
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
                               ${Projectname}</p>
                        </td>
                        <td>
                            <p align="left">
                              ${All_Use_pnum} </p>
                        </td>
                        <td>
                            <p align="left">
                              ${YoM_Use_pnum} </p>
                        </td>
                        <td>
                            <p align="left">
                              ${ToM_Use_pnum} </p>
                        </td>
                        <td>
                            <p align="left">
                              ${Yday_Use_pnum } </p>
                        </td>
                        <td>
                            <p align="left">
                              ${Today_Use_pnum} </p>
                        </td>
                        <td >
                             <a href="Project_Count.aspx?id=${Id}">查看此项目</a>
                        </td>
                    </tr>
                    
    </script>
</asp:Content>