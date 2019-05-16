<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Manage.Master" CodeBehind="Project_Count.aspx.cs" Inherits="ETS2.WebApp.Agent.Project_Count" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {

            //日历
            var nowdate = '<%=Yesterday %>';
            var enddate = '<%=today %>';

            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {
                $($(this)).datepicker();

                $("#startime").val(nowdate);
                $("#endtime").val(enddate);

            });

            var agentid = $("#hid_agentid").trimVal();

            SearchList(1);

            //装载列表
            function SearchList(pageindex) {
                var startime = $("#startime").val();
                var endtime = $("#endtime").val();
                var Id = $("#hid_id").val();
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=prolistbyprojectid",
                    data: { pageindex: pageindex, pagesize: pageSize, Id: Id, startime: startime, endtime: endtime },
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
                <div class="composetab_unsel"><div><a href="Project.aspx">返回项目列表</a></div></div>
            </li>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_sel"><div><a href="Project_Count.aspx?id=<%=Id %>">验票统计</a></div></div>
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
                <div style="text-align: center;">

                      日期查询  起始<input name="startime" type="text" id="startime"  isdate="yes"/>
                      截止<input name="endtime" type="text" id="endtime" isdate="yes"/>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px;
                            height: 26px;" >
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
                                产品</p>
                        </td>
                        <td width="60">
                            <p align="left">验证数量</p>
                        </td>
                        <td width="60">
                            <p align="left">单价</p>
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
                               ${Pro_name}</p>
                        </td>
                        <td >
                            <p align="left" >
                               ${Use_pnum}</p>
                        </td>
                        <td>
                            <p align="left">
                                ${Agentsettle_price}</p>
                        </td>
                        <td>
                        {{if Servertype==9}}
                             <a href="Project_Hotellist.aspx?id=${Id}&startime=${ChangeDateFormat(startime)}&endtime=${ChangeDateFormat(endtime)}">查看验票数据</a>
                        {{else}}
                             <a href="Project_Yanpiaolist.aspx?id=${Id}&startime=${ChangeDateFormat(startime)}&endtime=${ChangeDateFormat(endtime)}">查看验票数据</a>
                        {{/if}}
                        
                        </td>
                    </tr>
                    
    </script>
    <input id="hid_id" type="hidden" value="<%=Id %>" />
</asp:Content>
