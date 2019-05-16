<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/ui/etown.master"  CodeBehind="WxTemplateManage.aspx.cs" Inherits="ETS2.WebApp.WeiXin.WxTemplateManage" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="head">
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
                    url: "/JsonFactory/WeiXinHandler.ashx?oper=templatemodelpagelist",
                    data: { pageindex: pageindex, pagesize: pageSize },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询产品列表错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                $("#tblist").html("查询数据为空");
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
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="body">
    <div id="settings" class="view main">
        <%--<div id="secondary-tabs" class="navsetting ">
            <ul>
                <li><a href="/ui/permissionui/MasterList.aspx" onfocus="this.blur()" target=""><span>人员管理</span></a></li>
                <li><a href="/ui/permissionui/SSort.aspx" onfocus="this.blur()" target=""><span>商户管理</span></a></li>
                <li><a href="/ui/permissionui/AgentList.aspx" onfocus="this.blur()" target=""><span>分销商管理</span></a></li>
                <li><a href="/ui/permissionui/ticketlist.aspx" onfocus="this.blur()"><span>退票管理</span></a></li>
                <!--<li><a href="bankmanager.aspx" onfocus="this.blur()"><span>绑定银行管理</span></a></li>-->
                <li><a href="/ui/permissionui/ModelList.aspx" onfocus="this.blur()" target=""><span>模板管理</span></a></li>
                <li class="on"><a href="WxTemplateManage.aspx" onfocus="this.blur()" target="">微信模板管理</a></li>

            </ul>
        </div>--%>
        <div id="setting-home" class="vis-zone  mail-list">
            <div class="inner">
                <h3>
                   微信模板管理</h3>
                <div><a href="WxTemplateDetail.aspx">添加新模板</a>
                </div>
                <p>
                    &nbsp;</p>
                <table width="780" border="0">
                    <tr>
                        <td width="33" bgcolor="#CCCCCC">
                            ID
                        </td>
                        <td width="50" bgcolor="#CCCCCC">
                           操作类型
                        </td>
                        <td width="80" bgcolor="#CCCCCC">
                           微信指定名称
                        </td>
                        <td width="120" bgcolor="#CCCCCC">
                           前置说明
                        </td>
                        <td width="120" bgcolor="#CCCCCC">
                           后置备注
                        </td>
                        <td width="40" bgcolor="#CCCCCC">
                           
                        </td>
                    </tr>
                    <tbody id="tblist">
                    </tbody>
                </table>
                <div id="divPage">
                </div>
                <p>
                </p>
                <div>
                </div>
            </div>
        </div>
    </div>
    <div class="data">
    </div>
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">   
        <tr>
                        <td>
                           ${Id}
                        </td>
                        <td>
                             {{if Infotype==1}}
                                新订单模板
                             {{/if}}
                             {{if Infotype==2}}
                             订单状态变更通知
                             {{/if}}
                             {{if Infotype==3}}
                             门票订单预订成功通知
                             {{/if}}
                             {{if Infotype==4}}
                             酒店预订确认通知
                             {{/if}}
                             {{if Infotype==5}}
                              	会员充值通知 
                             {{/if}}
                             {{if Infotype==6}}
                              	会员消费通知 
                             {{/if}}
                             {{if Infotype==7}}
                              	积分奖励提醒 
                             {{/if}}
                              {{if Infotype==8}}
                              订阅活动开始提醒 
                             {{/if}}
                        </td>
                        <td>
                             ${Template_name}
                        </td>
                        <td>
                             ${First_DATA}
                        </td>
                        <td>
                             ${Remark_DATA}
                        </td>
                        <td>
                            <a href="WxTemplateDetail.aspx?id=${Id}">管理</a>
                        </td>
                    </tr>
    </script>
</asp:Content>
