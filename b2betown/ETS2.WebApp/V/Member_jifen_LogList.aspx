<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Etown.Master" AutoEventWireup="true"
    CodeBehind="Member_jifen_LogList.aspx.cs" Inherits="ETS2.WebApp.UI.WebForm1" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 15; //每页显示条数

        $(function () {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();
            SearchList(1);

            //装载财务列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/FinanceHandler.ashx?oper=integrallist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("积分列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                //                                $("#tblist").html("查询数据为空");
                            } else {
                                $("#FinanceItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pageSize, pageindex);
                            }


                        }
                    }
                })

                $.ajax({
                    type: "post",
                    url: "/JsonFactory/FinanceHandler.ashx?oper=integralcount",
                    data: { comid: comid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("积分列表错误");
                            return;
                        }
                        if (data.type == 100) {

                            $("#integralinfo").html(data.msg);


                        }
                    }
                })

            }


            //搜索明细列表
            $("#Search").click(function () {
                var pageindex = 1;
                var key = $("#key").val();
                var ServerName = $("#ServerName").val();

                if (key == "") {
                    $.prompt("查询关键词不能为空");
                    return;
                }

                $.ajax({
                    type: "post",
                    url: "/JsonFactory/FinanceHandler.ashx?oper=integrallist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, key: key },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("积分列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                //                                $("#tblist").html("查询数据为空");
                            } else {
                                $("#FinanceItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pageSize, pageindex);
                            }


                        }
                    }
                })


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
            <ul>
                <li><a href="/ui/pmui/eticket/eticketindex.aspx" onfocus="this.blur()"
                    target=""><span>会员卡验证</span></a></li>
                <li><a href="/v/Member_yufukuan_LogList.aspx" onfocus="this.blur()" target="">
                    <span>会员预付款验证明细</span></a></li>
                <li class="on"><a href="/v/Member_jifen_LogList.aspx" onfocus="this.blur()" target="">
                    <span>会员积分验证明细</span></a></li>
                <li><a href="/ui/crmui/Member_Activity_LogList.aspx" onfocus="this.blur()" target="">
                    <span>活动验证明细</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <table width="780" border="0">
                    <tr>
                      <td height="32">会员积分总额：<span id="integralinfo"></span> </td>
                    </tr>
        </table>
        <div style="text-align: center;">
                    <label>
                        请输入(手机号)
                        <input name="key" type="text" id="key" style="width: 160px; height: 20px;">
                    </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询会员" style="width: 120px;
                            height: 26px;">
                    </label>
                </div>
                <table width="780" border="0">
                    <tr>
                        <td width="51">
                            <p align="left">
                                流水号</p>
                        </td>
                        <td width="60">
                            <p align="left">
                                操作时间
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                               姓名
                            </p>
                      </td>
                      <td width="60">
                            <p align="left">
                               手机
                            </p>
                      </td>

                        <td width="150">
                            <p align="left">
                                内容
                            </p>
                      </td>
                        <td width="76">
                            <p align="left">
                                收支类型
                            </p>
                      </td>
                        <td width="50">
                            <p align="left">
                                赠送
                            </p>
                      </td>
                        <td width="44">
                            <p align="left">
                                使用
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
    <script type="text/x-jquery-tmpl" id="FinanceItemEdit">   
                    <tr>
                        <td >
                            <p align="left">
                                ${Id}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${jsonDateFormat(Subdate)}
                            </p>
                        </td>
                        <td >
                            <p align="left">
                                {{if Crm !=null}}  ${Crm.Name}( ${Crm.Id}){{/if}}</p>
                        </td>
                                                <td >
                            <p align="left">
                                {{if Crm !=null}}  ${Crm.Phone}{{/if}}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${Admin}[${OrderId}]</p>
                        </td>
                        <td >
                            <p align="left">
                                {{if Ptype==1}}赠送{{else}}支出{{/if}}</p>
                        </td>
                        <td >
                            <p align="left">
                                {{if Money>= 0}}${Money}{{/if}}</p>
                        </td>
                        <td>
                            <p align="left">
                              {{if Money< 0}}${Money}{{/if}}</p>
                        </td>
                        
                    </tr>
    </script>
</asp:Content>
