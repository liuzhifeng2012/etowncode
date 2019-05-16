<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="MemberCardList.aspx.cs"
    Inherits="ETS2.WebApp.UI.MemberUI.MemberCardList" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {

            var comid = $("#hid_comid").trimVal();
            var issueid = $("#hid_issueid").trimVal();
            var channelid = $("#hid_channelid").trimVal();
            var actid = $("#hid_activityid").trimVal();
            var cardcode = $("#hid_cardcode").trimVal();
            var isopencard = $("#hid_isopencard").trimVal();
            var phone = $("#hid_phone").trimVal();
            $("#select2").val(isopencard);


            SearchList(1);

            //装载卡列表
            function SearchList(pageindex) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/CardHandler.ashx?oper=MemberCardPageList",
                    data: { comid: comid, pageindex: pageindex, pagesize: pageSize, issueid: issueid, channelid: channelid, actid: actid, cardcode: cardcode, isopencard: isopencard },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询卡列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
//                                $("#tblist").html("<tr><td colspan='9'>查询数据不存在或者卡号未录入</td></tr>");
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

            $("#btns").click(function () {
                var cardcoded = $("#cardcode").trimVal();
                if (cardcoded == "") {
                    alert("卡号不可为空");
                }
                else {
                    window.open("membercardlist.aspx?pno=" + cardcoded, target = "_self");
                }
            })

            $("#select2").change(function () {
                $("#select2 option").each(function (i, o) {
                    if ($(this).attr("selected")) {
                        isopencard = $("#select2").val();
                        SearchList(1);
                    }
                });

            })
        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="CardList.aspx" onfocus="this.blur()"><span>卡片管理</span></a></li>
                <li><a href="CardEdit.aspx" onfocus="this.blur()"><span>添加卡片</span></a></li>
                <li class="on"><a href="membercardlist.aspx" onfocus="this.blur()"><span>已录入卡号列表</span></a></li>
                <li><a href="PublishList.aspx" onfocus="this.blur()"><span>发行管理</span></a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    已录入卡号列表</h3>
                <h3>
                    &nbsp;</h3>
                <table border="0">
                    <tr>
                        <td colspan="1">
                            <select name="select2" id="select2">
                                <option value="0">全部</option>
                                <option value="1">已经开卡</option>
                                <option value="2">未开卡</option>
                            </select>
                        </td>
                        <td colspan="10">
                            卡号:<input type="text" id="cardcode" value="" /><input type="button" id="btns" value="确认查询" />
                        </td>
                    </tr>
                </table>
                <table border="0">
                    <tr>
                        <td width="120">
                            <p align="left">
                                卡号</p>
                        </td>
                        <td width="69">
                            持卡人姓名
                        </td>
                        <td width="80">
                            持卡人手机
                        </td>
                        <td width="120">
                            <p align="left">
                                开卡时间</p>
                        </td>
                        <td width="78">
                            发送渠道
                        </td>
                        <td width="210">
                            参与的活动
                        </td>
                        <td width="200">
                            <p align="left">
                                发行标题
                            </p>
                        </td>
                        <td width="60">
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
    </div>
    <div class="data">
    </div>
    <input type="hidden" id="hid_issueid" value="<%=issueid %>" />
    <input type="hidden" id="hid_channelid" value="<%=channelid %>" />
    <input type="hidden" id="hid_actid" value="<%=actid %>" />
    <input type="hidden" id="hid_cardcode" value="<%=cardcode %>" />
    <input type="hidden" id="hid_isopencard" value="<%=isopencard %>" />
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">
      <tr>
                        
                        <td >
                            <p align="left">
                                ${CardCode}</p>
                        </td>
                        <td >
                           ${Name}
                        </td>
                        <td >
                           ${Phone}
                        </td>
                        <td >
                            <p align="left">
                                ${OpenSubDate} </p>
                        </td>
                        <td >
                            ${ChannelName}
                        </td>
                        <td >
                           ${ActStr}
                        </td>
                        <td>
                            <p align="left">
                            ${IssueTitle}</p>
                        </td>
                        
                        <td >
                            ${EnteredState}${OpenState}
                        </td>
                        <td >
                            <p align="left">
                                 管理 </p>
                        </td>
                    </tr>
                    
    </script>
</asp:Content>
