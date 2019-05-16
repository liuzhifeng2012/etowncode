<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Manage.Master"  CodeBehind="Project_Hotellist.aspx.cs" Inherits="ETS2.WebApp.Agent.Project_Hotellist" %>

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
                    url: "/JsonFactory/AgentHandler.ashx?oper=AgentHotelOrderlist",
                    data: { pageindex: pageindex, pagesize: pageSize, productid: Id, begindate: startime, enddate: endtime },
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
                window.open("/excel/DownExcel.aspx?oper=downagentpro_yplist&proid=" + '<%=Id %>' + "&startime=" + $("#startime").trimVal() + "&endtime=" + $("#endtime").trimVal(), target = "_blank");
            })
        })

        function showbinder(bookpro_bindcompany, bookpro_bindconfirmtime, bookpro_bindname, bookpro_bindphone) {
            alert(bookpro_bindcompany + "--" + bookpro_bindname + "--" + bookpro_bindphone + "--" + bookpro_bindconfirmtime);
        }
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
                     <label style="display:none;">
                        <input  type="button" id="downtoexcel" value="下载到excel" style="width: 120px;
                            height: 26px;" >
                    </label>
         </div>
         <div class="nowrap left" unselectable="on" onselectstart="return false;">
        
         
         </div></div>



        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                  <table width="780" border="0" style="margin-left: 15px;">
                    <tr>
                        <th width="10%">
                            项目
                        </th>
                        <th width="20%">
                            产品信息
                        </th>
                        <th width="5%">
                            订单id
                        </th>
                        <th width="10%">
                            提单时间
                        </th>
                        <th width="10%">
                            入住人信息
                        </th>
                        <th width="10%">
                            入住间数
                        </th>
                        <th width="10%">
                            入住日期
                        </th>
                        <th width="10%">
                            离店日期
                        </th>
                        <th width="10%">
                            入住天数
                        </th>
                        <th>
                            绑定人
                        </th>
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
          <td title="${projectname}">${projectname}</td>
          <td title="(${pro_id})${pro_name}">(${pro_id})${pro_name}</td>
          <td>${ordernum}</td>
          <td title="${u_subdate}">${u_subdate}</td>
          <td title="${u_name}(${u_phone})">${u_name}(${u_phone})</td>
          <td>${u_num}</td>
          <td>${start_date}</td>
          <td>${end_date}</td>
          <td>${bookdaynum}</td>
          <td><a href="javascript:void(0)" onclick="showbinder('${bookpro_bindcompany}','${bookpro_bindconfirmtime}','${bookpro_bindname}','${bookpro_bindphone}')" style="text-decoration:underline;">绑定人信息</a></td>
        </tr>
    </script>
    <input id="hid_id" type="hidden" value="<%=Id %>" />
</asp:Content>
