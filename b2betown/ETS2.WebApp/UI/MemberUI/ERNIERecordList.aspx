<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="ERNIERecordList.aspx.cs"
    Inherits="ETS2.WebApp.UI.MemberUI.ERNIERecordList" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数

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
                    url: "/JsonFactory/PromotionHandler.ashx?oper=ERNIERecordpagelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询摇奖活动列表错误");
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

        function Fajiang(actid) {
            if (actid == '') {
                $.prompt("参数传递错误");
                return;
            }



            $.ajax({
                type: "post",
                url: "/JsonFactory/PromotionHandler.ashx?oper=ERNIERecordedit",
                data: { actid: actid },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");
                    if (data.type == 1) {
                        $.prompt(data.msg);
                        return;
                    }
                    if (data.type == 100) {
                        $.prompt("确认成功");
                        return;
                    }
                }
            })
        }

    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
       <%-- <div id="secondary-tabs" class="navsetting ">
            <ul>
                <%if (IsParentCompanyUser)
                  { %>
                <li><a href="ERNIEActList.aspx" onfocus="this.blur()"><span>摇奖活动管理</span></a></li>
                <li><a href="ERNIEActEdit.aspx" onfocus="this.blur()"><span>添加摇奖活动</span></a></li>
                <li class="on"><a href="ERNIERecordList.aspx" onfocus="this.blur()"><span>中奖管理</span></a></li>
                <%}
                  else
                  {
                %>
                <li><a href="ERNIERecordList.aspx" onfocus="this.blur()"><span>中奖管理</span></a></li>
                <%
                    } %>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    中奖管理</h3>
                <h3>
                    &nbsp;</h3>
                <table width="780" border="0">
                    <tr>
                        <td width="40">
                            <p align="left">
                                序号</p>
                        </td>
                        <td width="100">
                            <p align="left">
                                活动名称
                            </p>
                        </td>
                        <td width="90">
                            奖品名称
                        </td>
                        <td width="80">
                            <p align="left">
                                抽奖时间</p>
                        </td>
                        <td width="40">
                            中奖号码
                        </td>
                        <td width="100">
                            <p align="left">
                                中奖用户
                            </p>
                        </td>
                        <td width="45">
                            用户ID
                        </td>
                        <td width="40">
                            处理状态
                        </td>
                        <td width="70">
                            <p align="left">
                                操作</p>
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
                            <p align="left">
                                ${Title}
                            </p>
                        </td>
                        <td >
                            <p align="left">
                                ${Award}</p>
                        </td>
                        <td >
                            <p align="left">
                                 ${ChangeDateFormat(ERNIE_time)}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${ERNIE_code}</p>
                        </td>
                        <td >
                            <p align="left">
                                 ${Name} (${Phone})  </p>
                        </td>
                        <td>${ERNIE_uid}</td>
                        <td>
                            <p align="left">
                                  ${Process_state}
                            </p>
                        </td>
                        <td>
                            <p align="left"> 
                            {{if Process_state==0}}
                            <input type="button" Onclick="javascript:if(confirm('确认,标记将为已发奖！'))Fajiang('${Id}')" value="确认发奖"> 
                            {{/if}}
                              </p>
                        </td>
                    </tr>
    </script>
</asp:Content>
