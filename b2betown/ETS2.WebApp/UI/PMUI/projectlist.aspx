<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="projectlist.aspx.cs" Inherits="ETS2.WebApp.UI.PMUI.projectlist"
    MasterPageFile="/UI/Etown.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var PageSize = 16;
        $(function () {

            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();

            if ($.cookie("project_online") != null && $.cookie("project_key") != null && $.cookie("project_curpage") != null) {

                $("#key").val($.cookie("project_key"));
                SearchList($.cookie("project_curpage"), PageSize, $.cookie("project_key"), $.cookie("project_online"));
            } else {

                //把上下线状态，key，当前页 保存到cookie
                $.cookie("project_online", "1", { path: '/' });
                $.cookie("project_key", "", { path: '/' });
                $.cookie("project_curpage", "1", { path: '/' });

                SearchList(1, PageSize, "", '1');
            }

            $("#Search1").click(function () {
                var key = $("#key").trimVal();
                if (key == "") {
                    $.prompt("请输入关键词");
                    return;
                }


                //把上下线状态，key，当前页 保存到cookie
                $.cookie("project_online", "0,1", { path: '/' });
                $.cookie("project_key", key, { path: '/' });
                $.cookie("project_curpage", "1", { path: '/' });

                SearchList(1, PageSize, key, '0,1');
            })
        })
        //装载项目列表
        function SearchList(pageindex, pagesize, key, onlinestate) {
            if (pageindex == '') {
                $.prompt("请选择跳到的页数");
                return;
            }
            $.ajax({
                type: "post",
                url: "/JsonFactory/ProductHandler.ashx?oper=projectpagelist",
                data: { comid: $("#hid_comid").val(), pageindex: pageindex, pagesize: pagesize, key: key, projectstate: onlinestate },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt("查询失败" + data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $("#tblist").empty();
                        $("#divPage").empty();
                        if (data.totalCount == 0) {

                        } else {


                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                            setpage(data.totalCount, pagesize, pageindex, key, onlinestate);
                        }


                    }
                }
            })


        }

        //分页
        function setpage(count, pagesize, curpage, key, onlinestate) {
            $("#divPage").paginate({
                count: Math.ceil(count / pagesize),
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


                    $.cookie("project_curpage", page, { path: '/' });

                    SearchList(page, pagesize, key, onlinestate);

                    return false;
                }
            });
        }
        function viewcc(onlinestate) {

            $.cookie("project_online", onlinestate, { path: '/' });

            SearchList(1, PageSize, '', onlinestate);
        }

        function reset1() {
            $.cookie("project_online", null, { path: '/' });
            $.cookie("project_key", null, { path: '/' });
            $.cookie("project_curpage", null, { path: '/' });

            $("#key").val("");
            $.cookie("project_online", "1", { path: '/' });
            $.cookie("project_key", "", { path: '/' });
            $.cookie("project_curpage", "1", { path: '/' });

            SearchList(1, PageSize, "", '1');
        }
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <%--<div id="secondary-tabs" class="navsetting ">
            <ul>
                <li class="on"><a href="/ui/pmui/ProjectList.aspx" onfocus="this.blur()" target=""><span>
                    项目列表</span></a></li>
                <li><a href="/ui/pmui/Projectedit.aspx" onfocus="this.blur()" target=""><span>添加项目</span></a></li>
                <li><a href="/ui/pmui/ProductList.aspx" onfocus="this.blur()" target=""><span>产品列表</span></a></li>
                <li><a href="/ui/pmui/ProductServerTypeList.aspx" onfocus="this.blur()" target=""><span>
                    添加产品</span></a></li>
                     <li><a href="/ui/pmui/order/Salecount.aspx" onfocus="this.blur()" target="">产品统计</a></li>
                      <li><a href="/ui/pmui/BindingAgent.aspx" onfocus="this.blur()" target="">导入分销系统产品</a></li>
                    <li  ><a href="/ui/pmui/eticket_useset.aspx" onfocus="this.blur()" target="">
                        <span>商户特定日期设定</span></a></li>
                   <li><a href="/ui/pmui/delivery/deliverylist.aspx" onfocus="this.blur()" target="">
                    <span>运费模版管理</span></a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    项目管理</h3>
                <h4>
                    <a style="float: right" href="projectsort.aspx">点击对项目排序</a></h4>
                <div style="text-align: center;">
                    <label>
                        请输入关键词
                        <input name="key" type="text" id="key" style="width: 160px; height: 20px;">
                    </label>
                    <label>
                        <input name="Search1" type="button" id="Search1" value="  查  询  " style="width: 120px;
                            height: 26px;">
                    </label>
                    <a href="javascript:void(0)" onclick="reset1()">重置条件</a> <a href="javascript:void(0)"
                        onclick="viewcc('0,1')">全部</a> <a href="javascript:void(0)" onclick="viewcc('1')">上线</a>
                    <a href="javascript:void(0)" onclick="viewcc('0')">下线</a>
                </div>
                <table width="780" border="0">
                    <tbody>
                        <tr>
                            <td width="10">
                                <p align="left">
                                    ID</p>
                            </td>
                            <td width="60">
                                <p align="left">
                                    项目名称
                                </p>
                            </td>
                            <td width="22">
                                <p align="left">
                                    所在城市
                                </p>
                            </td>
                            <td width="30">
                                <p align="left">
                                    上线时间
                                </p>
                            </td>
                            <td width="20">
                                <p align="left">
                                    运行状态
                                </p>
                            </td>
                            <td width="20">
                            <p align="left">
                                所有验证数</p>
                        </td>
                        <td width="12">
                            <p align="left">
                                上月</p>
                        </td>
                        <td width="12">
                            <p align="left">
                                本月</p>
                        </td>
                        <td width="12">
                            <p align="left">
                                昨天</p>
                        </td>
                        <td width="12">
                            <p align="left">
                                今天</p>
                        </td>
                            <td width="120">
                                <p align="left">
                                    修改管理
                                </p>
                            </td>
                        </tr>
                    </tbody>
                    <tbody id="tblist">
                    </tbody>
                </table>
                <div id="divPage">
                </div>
            </div>
        </div>
        <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr>
                            <td>
                                <p align="left">
                                    ${Id}</p>
                            </td>
                            <td>
                                <p align="left">
                                    ${Projectname}
                                </p>
                            </td>
                            <td>
                                <p align="left">
                                    ${Province}</p>
                            </td>
                            <td>
                                <p align="left">
                                   ${ChangeDateFormat(CreateTime)}</p>
                            </td>
                            <td>
                                <p align="left">
                                    ${OnlineState}
                                </p>
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
                            <td>
                                <p align="left">
                                    <a href="projectedit.aspx?projectid=${Id}" class="a_anniu">编 辑</a> &nbsp; <a href="productlist.aspx?projectid=${Id}" class="a_anniu">产品管理</a>  <a href="ProjectAgentlist.aspx?projectid=${Id}" class="a_anniu">设置项目账户</a> <a href="Project_finance.aspx?projectid=${Id}" class="a_anniu">项目对账</a>
                                </p>
                            </td>
                        </tr>
        </script>
</asp:Content>
