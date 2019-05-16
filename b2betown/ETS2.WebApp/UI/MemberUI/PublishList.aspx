<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="PublishList.aspx.cs"
    Inherits="ETS2.WebApp.UI.MemberUI.PublishList" %>

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
                    url: "/JsonFactory/IssueHandler.ashx?oper=pagelist",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询发行列表错误");
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
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="CardList.aspx" onfocus="this.blur()"><span>卡片管理</span></a></li>
                <li><a href="CardEdit.aspx" onfocus="this.blur()"><span>添加卡片</span></a></li>
                <li><a href="membercardlist.aspx" onfocus="this.blur()"><span>已录入卡号列表</span></a></li>
                <li class="on"><a href="PublishList.aspx" onfocus="this.blur()"><span>发行管理</span></a></li>
                <li><a href="PublishEdit.aspx" onfocus="this.blur()">添加发行</a></li>

            </ul>
        </div>
        <form id="commonForm" action="Checkcode2.html" method="get">
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    发行列表</h3>
                <h3>
                    &nbsp;</h3>
                <table width="780" border="0">
                    <tr>
                        <td width="194">
                            <p align="left">
                                发行标题</p>
                        </td>
                        <td width="69">
                            发行编号
                        </td>
                        <td width="67">
                            卡种类
                        </td>
                        <td width="103">
                            <p align="left">
                                发行量</p>
                        </td>
                        <td width="34">
                            开卡量
                        </td>
                        <td width="70">
                            发行日期
                        </td>
                        <td width="80">
                            <p align="left">
                                截至日期
                            </p>
                        </td>
                        <td width="103">
                            <p align="left">
                                发行渠道单位</p>
                        </td>
                        <td width="43">
                            <p align="left">
                                发行人</p>
                        </td>
                        <td width="47">
                            状态
                        </td>
                        <td width="124">
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
        </form>
    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">
      <tr>
                        
                        <td width="194">
                            <p align="left">
                                <a href="Publishdetail.aspx?issueid=${Id}" style="color: #0000FF">${Title}</a></p>
                        </td>
                        <td width="69">
                           ${Id}
                        </td>
                        <td width="67">
                            实体卡
                        </td>
                        <td width="103">
                            <p align="left">
                                ${Num}（已录${EnteredNum}）</p>
                        </td>
                        <td width="34">
                            ${OpenCardNum}
                        </td>
                        <td width="70">
                           ---
                        </td>
                        <td width="80">
                            <p align="left">
                                ---</p>
                        </td>
                        <td width="103">
                            <p align="left">
                                ${IsSueType}${UnitName}</p>
                        </td>
                        <td width="43">
                            <p align="left">
                                ${ISueName}</p>
                        </td>
                        <td width="47">
                            ---
                        </td>
                        <td width="124">
                            <p align="left">
                                <a href="PublishEdit.aspx?issueid=${Id}">管理</a></p>
                        </td>
                    </tr>
                    
    </script>
</asp:Content>
