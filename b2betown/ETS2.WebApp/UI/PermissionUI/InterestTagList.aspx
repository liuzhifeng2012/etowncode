<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InterestTagList.aspx.cs"
    MasterPageFile="/UI/Etown.Master" Inherits="ETS2.WebApp.UI.PermissionUI.InterestTagList" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();

            var tagtypeid = $("#hid_tagtypeid").val();

            SearchList(1, 10, tagtypeid);

        })


        function SearchList(pageindex, pagesize, tagtypeid) {
            $.ajax({
                type: "post",
                url: "/JsonFactory/CrmMemberHandler.ashx?oper=GetTagListByTypeid",
                data: { typeid: tagtypeid, pageindex: pageindex, pagesize: pagesize },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt("查询标签列表失败");
                        return;
                    }
                    if (data.type == 100) {
                        $("#tblist").empty();
                        $("#divPage").empty();
                        if (data.totalCount == 0) {

                        } else {
                            $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                            setpage(data.totalCount, pageindex, pagesize, tagtypeid);
                        }
                    }
                }
            })
        }


        //分页
        function setpage(total, pageindex, pagesize, tagtypeid) {
            $("#divPage").paginate({
                count: Math.ceil(total / pagesize),
                start: pageindex,
                display: 5,
                border: false,
                text_color: '#888',
                background_color: '#EEE',
                text_hover_color: 'black',
                images: false,
                rotate: false,
                mouse: 'press',
                onChange: function (page) {

                    SearchList(page, pagesize, tagtypeid);

                    return false;
                }
            });
        }
        function deltag(tagid) {
            $.post("/JsonFactory/CrmMemberHandler.ashx?oper=Deltag", { tagid: tagid }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                }
                if (data.type == 100) {
                    $.prompt("删除成功", {
                        buttons: [
                                 { title: '确定', value: true }
                                ],
                        opacity: 0.1,
                        focus: 0,
                        show: 'slideDown',
                        submit: function (e, v, m, f) {
                            if (v == true)
                                location.href = "InterestTagList.aspx?tagtypeid=" + $("#hid_tagtypeid").trimVal();
                        }
                    });

                }
            })
        }
 
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <%--<li class="on"><a href="/ui/crmui/BusinessCustomersList.aspx" onfocus="this.blur()">
                    会员列表</a></li>
                <li><a href="/ui/crmui/BusinessCustomersList.aspx" onfocus="this.blur()">会员列表</a></li>--%>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone  mail-list">
            <div class="inner">
                <h3>
                    兴趣标签列表</h3>
                <a href="InterestTagEdit.aspx?tagtypeid=<%=tagtypeid %>" style="float: right;">标签添加</a>
                <table width="780" border="0">
                    <tr>
                        <td width="6%">
                            标签id
                        </td>
                        <td width="6%">
                            标签名称
                        </td>
                        <td width="3%">
                            标签类型
                        </td>
                        <td width="4%">
                            管理
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                </table>
                <div id="divPage">
                </div>
                <p>
                    &nbsp;
                </p>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
                    <tr style="height: 60px;">
                        <td >
                             ${Id}
                        </td>
                        <td>${TagName}</td>
                        <td>${Tagtypeid}</td>
                        <td>
                        <a href="InterestTagEdit.aspx?tagid=${Id}&tagtypeid=${Tagtypeid}">编辑</a>
                        <a href="javascript:void(0)" onclick="deltag(${Id})">删除</a>
                        </td>
                    </tr>
    </script>
    <input type="hidden" id="hid_tagtypeid" value="<%=tagtypeid %>" />
</asp:Content>
