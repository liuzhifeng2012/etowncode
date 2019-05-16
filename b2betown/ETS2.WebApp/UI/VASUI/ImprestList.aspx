<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ImprestList.aspx.cs" Inherits="ETS2.WebApp.UI.CrmUI.ImprestList" %>

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
                    url: "/JsonFactory/FinanceHandler.ashx?oper=imprestlist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("预付款列表错误");
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
                    url: "/JsonFactory/FinanceHandler.ashx?oper=imprestcount",
                    data: { comid: comid },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("积分列表错误");
                            return;
                        }
                        if (data.type == 100) {

                            $("#imprestinfo").html(data.msg+"元");


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
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                 <li><a href="/ui/crmui/BusinessCustomersList.aspx" onfocus="this.blur()">会员列表</a></li>
                <li><a href="IntegralList.aspx" target="" title="">积分详情</a></li>
                <li  class="on"><a href="ImprestList.aspx" target="" title="">会员预付款详情</a></li>
               
            </ul>
        </div>--%>

        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <table width="780" border="0">
                    <tr>
                      <td height="32">会员预付款总额：<span id="imprestinfo"></span> </td>
                    </tr>
        </table>
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
                         <td width="100">
                            <p align="left">
                                会员卡号
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
                                录入
                            </p>
                      </td>
                        <td width="44">
                            <p align="left">
                                支出
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
                                ${ChangeDateFormat(Subdate)}
                            </p>
                        </td>
                        <td >
                            <p align="left">
                                {{if Crm !=null}}  ${Crm.Idcard}{{/if}}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${Admin}[${OrderId}]</p>
                        </td>
                        <td >
                            <p align="left">
                                {{if Ptype==1}}录入{{else}}支出{{/if}}</p>
                        </td>
                        <td >
                            <p align="left">
                                {{if Money>= 0}}${Money} 元{{/if}}</p>
                        </td>
                        <td>
                            <p align="left">
                              {{if Money< 0}}${Money} 元{{/if}}</p>
                        </td>
                        
                    </tr>
    </script>
</asp:Content>
