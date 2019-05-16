<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Chanelrebatelist.aspx.cs"
    Inherits="ETS2.WebApp.UI.VASUI.Chanelrebatelist" MasterPageFile="/UI/Etown.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 15; //每页显示条数

        $(function () {
            var userid = $("#hid_userid").trimVal();
            var comid = $("#hid_comid").trimVal();
            SearchList(1, "1,2,3");

            //装载返佣列表
            function SearchList(pageindex, payment) {
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/PermissionHandler.ashx?oper=channelrebatelist",
                    data: { channelid: '<%=channelid %>', pageindex: pageindex, pagesize: pageSize, payment: payment },
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
                                setpage(data.totalcount, pageSize, pageindex, payment);
                            }


                        }
                    }
                })


            }

            //分页
            function setpage(newcount, newpagesize, curpage, payment) {
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

                        SearchList(page, payment);

                        return false;
                    }
                });
            }
        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <table width="780" border="0">
                    <tr>
                        <td height="32">
                            可提现佣金总额：<span id="imprestinfo"><%=rebatemoney%></span> &nbsp;&nbsp;<a href="ChanelrebateApply.aspx" style=" text-decoration:underline;">佣金提现</a>
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
                                订单号
                            </p>
                        </td>
                        <td width="150">
                            <p align="left">
                                产品
                            </p>
                        </td>
                        <td width="50">
                            <p align="left">
                                订单金额
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
                        <td width="44">
                            <p align="left">
                                余额
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
                                ${jsonDateFormat(subdatetime)}
                            </p>
                        </td>
                        <td >
                            <p  >
                               {{if orderid>0}}
                               ${orderid}
                               {{else}}
                               ---
                               {{/if}}

                                </p>
                        </td>
                        <td >
                            <p  title="(${proid})${proname}">
                                {{if proid>0}}(${proid})
                                {{/if}}
                                ${proname}</p>
                        </td>
                         <td >
                            <p  >
                                {{if ordermoney>0}}
                                ${ordermoney}
                                {{else}}
                                ---
                                {{/if}}
                                </p>
                        </td>
                        <td >
                            <p  >
                                ${payment_type}({{if payment==1}}返佣进账{{/if}}{{if payment==2}}返佣提现{{/if}}{{if payment==3}}返佣退货{{/if}})</p>
                        </td>
                        <td >
                            <p  >
                                {{if rebatemoney>= 0}}${rebatemoney} 元{{/if}}</p>
                        </td>
                        <td>
                            <p  >
                              {{if rebatemoney< 0}}${rebatemoney} 元{{/if}}</p>
                        </td>
                        
                         <td>
                            <p  >
                                ${over_money}元</p>
                        </td>
                    </tr>
    </script>
</asp:Content>
