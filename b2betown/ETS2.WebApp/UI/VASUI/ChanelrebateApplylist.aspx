<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChanelrebateApplylist.aspx.cs"
    Inherits="ETS2.WebApp.UI.VASUI.ChanelrebateApplylist" MasterPageFile="/UI/Etown.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 15; //每页显示条数

        $(function () {

            SearchList(1, "0,1");

            //装载返佣申请提现列表
            function SearchList(pageindex, operstatus) {
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/PermissionHandler.ashx?oper=channelrebateapplylist",
                    data: { channelid: '<%=channelid %>', pageindex: pageindex, pagesize: pageSize, operstatus: operstatus },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {

                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalcount == 0) {
                                //                                $("#tblist").html("查询数据为空");
                            } else {
                                $("#FinanceItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalcount, pageSize, pageindex, operstatus);
                            }


                        }
                    }
                })


            }

            //分页
            function setpage(newcount, newpagesize, curpage, operstatus) {
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

                        SearchList(page, operstatus);

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
                <li><a href="ChanelrebateApplyaccount.aspx" onfocus="this.blur()">提现账户管理</a></li>
                <li class="on"><a href="ChanelrebateApplylist.aspx" target="" title="">提现申请列表</a></li>
                <li><a href="ChanelrebateApply.aspx" target="" title="">提现申请</a></li>
                <li><a href="Chanelrebatelist.aspx" target="" title="">返佣记录</a></li>
            </ul>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <table width="780" border="0">
                    <tr>
                        <td height="32">
                            顾问返佣提现记录
                        </td>
                    </tr>
                </table>
                <table width="780" border="0">
                    <tr>
                        <td width="60">
                            <p align="left">
                                操作时间
                            </p>
                        </td>
                        <td width="100">
                            <p align="left">
                                申请类型
                            </p>
                        </td>
                        <td width="150">
                            <p align="left">
                                申请详情
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                提现金额
                            </p>
                        </td>
                        <td width="76">
                            <p align="left">
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
                        
                        <td>
                            <p>
                                ${jsonDateFormat(applytime)}
                            </p>
                        </td>
                        <td >
                            <p  >
                                ${applytype}</p>
                        </td>
                        <td >
                            <p  >
                                 ${applydetail}</p>
                        </td>
                         <td >
                            <p  >
                                ${applymoney}</p>
                        </td>
                         
                         <td>
                            <p  >
                                 {{if operstatus==0}}
                                    申请中
                                 {{/if}}
                                 {{if operstatus==1}}
                                   已提现
                                 {{/if}}

                                 </p>
                        </td>
                    </tr>
    </script>
</asp:Content>
