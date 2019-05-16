<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/UI/Etown.Master" CodeBehind="PromotionList.aspx.cs"
    Inherits="ETS2.WebApp.UI.MemberUI.PromotionList" %>

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
                    url: "/JsonFactory/PromotionHandler.ashx?oper=pagelist",
                    data: {userid:userid, comid: comid, pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询活动列表错误");
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
                <li class="on"><a href="PromotionList.aspx" onfocus="this.blur()"><span>优惠券管理</span></a></li>
                <li><a href="PromotionManage.aspx" onfocus="this.blur()"><span>添加优惠券</span></a></li>
            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <h3>
                    优惠券列表</h3>
                <h3>
                    &nbsp;</h3>
                <table width="780" border="0">
                    <tr>
                        <td width="51">
                            <p align="left">
                                序号</p>
                        </td>
                        <td width="157">
                            <p align="left">
                                优惠券名称
                            </p>
                        </td>
                        <td width="72">
                            促销类型
                        </td>
                        <td width="55">
                            <p align="left">
                                优惠</p>
                        </td>
                        <td width="5">
                            
                        </td>
                        <td width="145">
                            <p align="left">
                                活动有效期
                            </p>
                        </td>
                        <td width="45">
                            是否过期
                        </td>
                        <td width="69">
                            运行状态
                        </td>
                        <td width="137">
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
                        <td width="51">
                            <p align="left">
                                ${Id}</p>
                        </td>
                        <td width="157">
                            <p align="left">
                                ${Title}
                            </p>
                        </td>
                        <td width="72">
                            <p align="left">
                                {{if Acttype==1}}
                                消费抵扣券
                                {{/if}}
                                {{if Acttype==2}}
                                消费折扣券
                                {{/if}}
                                {{if Acttype==3}}
                                消费满就送
                                {{/if}}
                                {{if Acttype==4}}
                                积分券
                                {{/if}}
                                </p>
                        </td>
                        <td width="55">
                            <p align="left">
                                 {{if Acttype==1}}
                                ${Money} 元
                                {{/if}}
                                {{if Acttype==2}}
                                ${Discount} %
                                {{/if}}
                                {{if Acttype==3}}
                                满 ${CashFull} 返 ${Cashback}
                                {{/if}}
                                {{if Acttype==4}}
                                ${Money}
                                {{/if}}
                                </p>
                        </td>
                        <td width="40">
                            <p align="left">
                                ${UseOnce}</p>
                        </td>
                        <td width="145">
                            <p align="left">
                                ${ChangeDateFormat(Actstar)} 至 ${ChangeDateFormat(Actend)}</p>
                        </td>
                        <td  width="45">${ExpiryDate}</td>
                        <td width="69">
                            <p align="left">
                                  ${Runstate}
                            </p>
                        </td>
                        <td width="137">
                           {{if Whetheredit=="yes"}}
                            <p align="left"><a href="PromotionEdit.aspx?actid=${Id}">修改</a>
                            </p>
                            {{else}}
                            ${CreateChannel}
                           {{/if}}
                        </td>
                    </tr>
    </script>
</asp:Content>
