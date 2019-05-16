<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="AgentMessage.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.AgentMessage" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 15; //每页显示条数

        $(function () {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();
            SearchList(1);

            //装载产品列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=agentmessagepagelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize },
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
                                //                                $("#tblist").html("查询数据为空");
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
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="AgentList.aspx" onfocus="this.blur()" target=""><span>分销商管理</span></a></li>
                
                 
                <li><a href="AgentSalesCode.aspx" onfocus="this.blur()" target=""><span>后台销售订单</span></a></li>
                <li><a href="AgentBackCode.aspx" onfocus="this.blur()" target=""><span>导码订单</span></a></li>
                <li><a href="AgentRecharge.aspx" onfocus="this.blur()" target=""><span>充值订单</span></a></li>
                   <li ><a href="AgentRecharge_Person.aspx" onfocus="this.blur()" target=""><span>人工充值记录</span></a></li>
                <li class="on"><a href="AgentMessage.aspx" onfocus="this.blur()" target="">管理分销通知</a></li>
                <li><a href="AgentFinanceCount.aspx" onfocus="this.blur()" target="">分销商统计</a>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    通知列表</h3><a href="AgentMessageUp.aspx" class="a_anniu">  添加新通知 </a>
                <table width="780" border="0">
                    <tr>
                        <td width="20">
                            <p align="left">
                                ID</p>
                        </td>
                        <td width="200">
                            <p align="left">
                                标题
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                提交日期
                            </p>
                        </td>
                        <td width="25">
                            <p align="left">
                                状态
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
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr>
                        <td>
                            <p align="left">
                                ${Id}</p>
                        </td>
                        <td>
                            <p align="left">
                                ${Title}
                            </p>
                        </td>

                        <td>
                            <p align="left">
                                ${ChangeDateFormat(Subtime)}</p>
                        </td>
                        <td>
                            <p align="left">
                                {{if State==0}}
                                    下线
                                {{else}}
                                    上线
                                {{/if}}
                                </p>
                        </td>
                        <td>
                            <p align="left">
                             <a href="AgentMessageUp.aspx?id=${Id}" class="a_anniu">管理</a>  &nbsp;
                            </p>
 
                        </td>
                    </tr>
    </script>

</asp:Content>
