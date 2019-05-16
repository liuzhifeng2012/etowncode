<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Manage.Master" CodeBehind="Project_Yanpiaolist.aspx.cs" Inherits="ETS2.WebApp.Agent.Project_Yanpiaolist" %>

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
            var nowdate = '<%=startime %>';
            var enddate = '<%=endtime %>';

            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {
                $($(this)).datepicker();

                $("#startime").val(nowdate);
                $("#endtime").val(enddate);

            });

            SearchList(1);

            //装载列表
            function SearchList(pageindex) {
                var startime = $("#startime").val();
                var endtime = $("#endtime").val();
                if (startime != "" || endtime != "") {
                    if (startime == "" || endtime == "") {
                        alert("起始日期和结束日期需要都选中！");
                        return;
                    }
                }
                var Id = $("#hid_id").val();
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=pnolistbyproid",
                    data: { pageindex: pageindex, pagesize: pageSize, Id: Id, startime: startime, endtime: endtime },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");
                        if (data == "err") {
                            $.prompt("查询错误");
                            return;
                        }

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

            $("#downtoexcel").click(function () {
                window.open("/excel/DownExcel.aspx?oper=downagentpro_yplist&proid=" + '<%=Id %>' + "&startime=" + $("#startime").trimVal() + "&endtime=" + $("#endtime").trimVal(),target="_blank");
            })
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
                <div class="composetab_sel"><div><a href="javascript:;">验票统计</a></div></div>
            </li>

         </ul>
          <div class="toolbg toolbgline toolheight nowrap" style="">
         <div class="right searchtool">
                 日期查询  起始<input name="startime" type="text" id="startime"  isdate="yes"/>
                      截止<input name="endtime" type="text" id="endtime" isdate="yes"/>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px;
                            height: 26px;" >
                    </label>
                     <label>
                        <input  type="button" id="downtoexcel" value="下载到excel" style="width: 120px;
                            height: 26px;" >
                    </label>
         </div>
         <div class="nowrap left" unselectable="on" onselectstart="return false;">
        
         
         </div></div>



        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                 <table border="0" width="780" class="mail-list-title">
                 
                    <tr>
                        <td width="6%" align="center" bgcolor="#CCCCCC">
                            编号
                        </td>
                        <td width="26%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                产品名称
                            </p>
                        </td>
                        <td width="6%" height="26"  bgcolor="#CCCCCC">
                            <p align="center">
                                订单ID</p>
                        </td>
                        <td width="6%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                面值
                            </p>
                        </td>
                        <td width="4%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                使用
                            </p>
                        </td>
                        <td width="10%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                有效期
                            </p>
                        </td>
                        <td width="10%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                票号
                            </p>
                        </td>
                        <td width="10%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                验票账户
                            </p>
                        </td>
                        <td width="22%" height="26" bgcolor="#CCCCCC">
                            <p align="center">
                                确认日期
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
                        <td >
                            <p align="center">
                                ${id}</p>
                        </td>
                        <td >
                            <p align="center">
                                ${ProName}
                            </p>
                        </td>
                        <td class="sender item">
                            <p align="center">
                                ${Orderid}
                            </p>
                        </td>
                        <td class="sender item">
                            <p align="center">
                                ${FacePrice}</p>
                        </td>
                        <td class="sender item">
                            <p align="center">
                                ${UseNum}</p>
                        </td>
                        <td class="sender item">
                            <p align="center">
                                ${ChangeDateFormat(ProEnd)}
                            </p>
                        </td>
                        <td class="sender item">
                            <p align="center">
                                ${Pno}</p>
                        </td>
                        <td class="sender item">
                            <p align="center">
                                {{if PosId !="0"}}
                                ${PosId} 
                                {{/if}}
                                {{if Pcaccount !=""}}
                                ${Pcaccount}
                                {{/if}}
                                </p>
                        </td>
                        <td class="sender item">
                            <p align="center">
                                ${ConfirmDate}</p>
                        </td>
                    </tr>
    </script>
    <input id="hid_id" type="hidden" value="<%=Id %>" />
</asp:Content>
