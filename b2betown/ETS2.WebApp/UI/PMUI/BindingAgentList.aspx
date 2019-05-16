<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="/UI/Etown.Master" CodeBehind="BindingAgentList.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.BindingAgentList" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数

        $(function () {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();
            SearchList(1, '');

            //装载产品列表
            function SearchList(pageindex, key) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=bindingwarrantlist",
                    data: { pageindex: pageindex, pagesize: pageSize, key: key },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("列表错误");
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
            $("#loginagent").click(function () {
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=loginagent",
                    data: {},
                    async: false,
                    success: function (data) {
                        if (data == "OK") {
                            window.open("/Agent/Default.aspx", target = "_blank");
                        } else {

                            alert("快速登录错误！");
                        }
                    }
                })
            })

            //查询
            $("#Search").click(function () {
                var key = $("#key").val();

                SearchList(1, key);
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
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/pmui/projectlist.aspx" onfocus="this.blur()" target=""><span>项目管理</span></a></li>
                <li><a href="/ui/pmui/Projectedit.aspx" onfocus="this.blur()" target=""><span>添加项目</span></a></li>
                <li><a href="/ui/pmui/ProductList.aspx" onfocus="this.blur()"
                    target=""><span>产品列表</span></a></li>
                <li><a href="/ui/pmui/ProductServerTypeList.aspx" onfocus="this.blur()"
                    target=""><span>添加产品</span></a></li>
                <li><a href="/ui/pmui/order/Salecount.aspx" onfocus="this.blur()" target="">产品统计</a></li>
                <li class="on"><a href="/ui/pmui/BindingAgent.aspx" onfocus="this.blur()" target="">导入分销系统产品</a></li>
            </ul>
        </div>--%>
       <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                    <div style="text-align: ;">
                    <input name="loginagent" type="button" id="loginagent" value="快速登录分销系统" style="width: 120px; height: 26px;" />
                        <%--<label>
                           商户查询
                            <input name="key" type="text" id="key" style="width: 160px; height: 20px;" />
                        </label>
                        <label>
                            <input name="Search" type="button" id="Search" value="查询" style="width: 120px; height: 26px;" />
                        </label>--%>
                    </div>
                </h3>
                <table width="780" border="0">
                    <tr>
                        <td width="26">
                            <p align="left">
                                序号</p>
                        </td>
                        <td width="260">
                            <p align="left">
                                名称</p>
                        </td>
                        <td width="60">
                            <p align="left">
                                </p>
                        </td>
                        <td width="60">
                            <p align="left">
                                 出票类型
                            </p>
                        </td>
                        <td width="100">
                            <p align="left">
                                预付款金额</p>
                        </td>
                         <td width="100">
                            <p align="left">
                                信用额</p>
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
                               ${Comname}</p>
                        </td>
                        <td >
                            <p align="left">
                               </p>
                        </td>
                        <td >
                            <p align="left">
                                ${Warrant_type}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${Imprest}</p>
                        </td>
                                                <td >
                            <p align="left">
                                ${Credit}</p>
                        </td>
                        <td >
                             <a href="BindingAgentPorList.aspx?comid=${Comid}">进入此商户</a>
                        </td>
                    </tr>
                    
    </script>
</asp:Content>

