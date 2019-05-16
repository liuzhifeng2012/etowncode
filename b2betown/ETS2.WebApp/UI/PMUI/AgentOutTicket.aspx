<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="/UI/Etown.Master" CodeBehind="AgentOutTicket.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.AgentOutTicket" %>


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
            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {
                $($(this)).datepicker();
            });


            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();


            //获取项目列表
            $.post("/JsonFactory/ProductHandler.ashx?oper=projectlist", { comid: comid, projectstate: 1 }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) { }
                if (data.type == 100) {
                    if (data.msg == "") {

                    } else {
                        var provincestr = ' <option value=""  selected="selected">全部</option>';
                        for (var i = 0; i < data.msg.length; i++) {
                            provincestr += '<option value="' + data.msg[i].Id + '" >' + data.msg[i].Projectname + '</option>';
                        }
                        $("#projectid").html(provincestr);
                    }

                }
            })

            $("#Search").click(function () {
               
                var key = $("#key").trimVal();

                var agentsourcesort = $("#agentsourcesort").val();
                var warrantstate = $("#sel_warrantstate").val();
                var projectid = $("#projectid").val();


                SearchList(1, "", "", key, agentsourcesort, warrantstate, projectid);
            })








            SearchList(1, "", "", "", 0, 1, "");


        })

        function timeSearchList(startime, endtime) {
            $("#startime").val(startime);
            $("#endtime").val(endtime);
           
            var key = $("#key").trimVal();

            var agentsourcesort = $("#agentsourcesort").val();
            var warrantstate = $("#sel_warrantstate").val();
            var projectid = $("#projectid").val();
            SearchList(1, "", "", key, agentsourcesort, warrantstate, projectid);
        }


        //装载产品列表
        function SearchList(pageindex, com_province, com_city, key, agentsourcesort, warrantstate, projectid) {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();
            $("#loading").show();
            var startime = $("#startime").val();
            var endtime = $("#endtime").val();

            if (pageindex == '') {
                $.prompt("请选择跳到的页数");
                return;
            }
            $.ajax({
                type: "post",
                url: "/JsonFactory/AgentHandler.ashx?oper=agentoutticketlist",
                data: { comid: comid, pageindex: pageindex, pagesize: pageSize, com_province: "", com_city: "", Key: key, agentsourcesort: agentsourcesort, warrantstate: warrantstate, projectid: projectid, startime: startime, endtime: endtime },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt("查询产品列表错误");
                        return;
                    }
                    if (data.type == 100) {
                        $("#loading").hide();
                        $("#tblist").empty();
                        $("#divPage").empty();
                        if (data.totalCount == 0) {
                            // $("#tblist").html("查询数据为空");
                            if (data.msg1 != "") {
                                $("#ProductItemEdit").tmpl(data.msg1).appendTo("#tblist");
                            }
                        } else {

                            if (data.msg1 != "") {
                                $("#ProductItemEdit").tmpl(data.msg1).appendTo("#tblist");
                            }

                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                            setpage(data.totalCount, pageSize, pageindex, com_province, com_city, key, agentsourcesort, warrantstate, projectid);
                        }


                    }
                }
            })


        }

        //分页
        function setpage(newcount, newpagesize, curpage, com_province, com_city, key, agentsourcesort, warrantstate, projectid) {
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

                    SearchList(page, com_province, com_city, key, agentsourcesort, warrantstate, projectid);

                    return false;
                }
            });
        }


    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
      <%--  <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="AgentList.aspx" onfocus="this.blur()" target=""><span>分销商管理</span></a></li>
                <li><a href="AgentSalesCode.aspx" onfocus="this.blur()" target=""><span>后台销售订单</span></a></li>
                <li><a href="AgentBackCode.aspx" onfocus="this.blur()" target=""><span>导码订单</span></a></li>
                <li><a href="AgentRecharge.aspx" onfocus="this.blur()" target=""><span>充值订单</span></a></li>
                 <li ><a href="AgentRecharge_Person.aspx" onfocus="this.blur()" target=""><span>人工充值记录</span></a></li>
                <li><a href="AgentMessage.aspx" onfocus="this.blur()" target="">管理分销通知</a></li>
                <li> <a href="AgentFinanceCount.aspx"  onfocus="this.blur()" target="">分销商统计</a> </li>
               
            </ul>
        </div>--%>
        <div class="mi-form-item">
            <div style=" padding-bottom:10px; font-size:14px; line-height:30px;">
            <input class="mi-input" name="startime" id="startime" placeholder="起始时间" value="<%=startime %>" isdate="yes" type="text">-
            <input class="mi-input" name="endtime" id="endtime" placeholder="结束时间" value="<%=endtime %>" isdate="yes" type="text">
            
            <a href="javascript:;" onclick="timeSearchList('<%=today %>','<%=today %>')">今天</a>
            <a href="javascript:;"  onclick="timeSearchList('<%=yesterday %>','<%=yesterday %>')">昨天</a>
            <a href="javascript:;"  onclick="timeSearchList('<%=tomonth_star %>','<%=today %>')">本月</a>
            <a href="javascript:;"  onclick="timeSearchList('<%=yestermonth_star %>','<%=yestermonth_end %>')">上月</a>
            <a href="javascript:;"  onclick="timeSearchList('<%=month_3 %>','<%=today %>')">近3个月</a>
            <a href="javascript:;"  onclick="timeSearchList('','')">全部</a>
            </div>

            <label>
               授权状态
            </label>
            <select id="sel_warrantstate" class="mi-input" style="width: 120px;">
            <option value="" >全部</option>
                <option value="1"  selected="selected">授权运行</option>
                <option value="0">暂停关闭</option>
            </select> 
             <label>
               指定项目
            </label>
            <select id="projectid" class="mi-input" style="width: 120px;">

            </select> 
            <label>
                关键词</label>
            <input name="key" type="text" id="key" class="mi-input" placeholder="分销公司名称,手机" style="width: 120px;"/>
            <input name="Search" type="button" id="Search" value="查询" style="width: 120px; height: 26px;" />
            <br>
             </label>

                    
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    <a href="AgentList.aspx" class="b_anniu">分销商列表</a> <a href="AgentRegi.aspx" class="a_anniu">新增分销商</a> <a href="LPManage.aspx" class="a_anniu">平台分销商设定</a> </h3>
                <table width="780" border="0">
                    <tr>
                        <td width="20">
                            <p align="left">
                                ID</p>
                        </td>
                        <td width="160">
                            <p align="left">
                                分销商公司名称
                            </p>
                        </td>
                        <td width="3">
                            <p align="left">
                               
                            </p>
                        </td>
                        <td width="120">
                            <p align="left">
                               日期
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                出票数量
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                验票数量
                            </p>
                        </td>
                        
                        <td width="40">
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

    注1：默认查询近3月的统计，余额 不按时间统计。
    <br>注2：在线充值，人工充值，返点，人工扣款，销售额，毛利，未消费，倒码销售，倒码毛利，已消费按 指定时间查询。
    <br>注3：未消费，倒码毛利，是统计指定时间内产生的订单 未消费或倒码毛利 。
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr>
                        <td>
                            <p align="left">
                                ${Agentid}</p>
                        </td>
                                                <td>
                            <p align="left">
                                {{if AgentWarrant !=null}}
                                ${AgentWarrant.Company}
                                {{/if}}</p>
                        </td>
                        <td>
                            <p align="left">
                               
                            </p>
                        </td>
                       <td>
                            <p align="left">
                               ${startime}  到  ${endtime}
                            </p>
                        </td>
                        <td>
                            <p align="left">
                               ${Outticketnum}</p>
                        </td>
                        <td>
                            <p align="left">
                                ${Yanzhengticketnum}</p>
                        </td>
                        

                        <td>
                            <p align="left">
                             <a href="/ui/pmui/eticket/eticketlist.aspx?agentid=${Agentid}" class="a_anniu">查看验票详情</a> 
                            </p>
 
                        </td>
                    </tr>
    </script>
    <div id="loading" class="loading" style="display: none;">
            正在加载...
        </div>

</asp:Content>