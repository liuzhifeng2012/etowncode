<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Manage.Master" CodeBehind="Verification.aspx.cs" Inherits="ETS2.WebApp.Agent.Verification" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <link href="/Scripts/JUI/jquery-ui-1.10.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JUI/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/JUI/datepicker-regional.js" type="text/javascript"></script>
    <script src="/Scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>

    <script type="text/javascript">
        var pageSize = 200; //每页显示条数
        $(function () {
            var agentid = $("#hid_agentid").trimVal();
            var comid = $("#hid_comid_temp").trimVal();
            SearchList(1);

            //日历
            var dateinput = $("input[isdate=yes]");
            $.each(dateinput, function (i) {
                $($(this)).datepicker();
            });

            $("#Search").click(function () {
                SearchList(1);
            })
            //装载产品列表
            //装载产品列表
            function SearchList(pageindex) {
                var key = $("#key").trimVal();
                var startime = $("#startime").trimVal();
                var endtime = $("#endtime").trimVal();

                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/EticketHandler.ashx?oper=agentEticketlog",
                    data: { comid: comid, agentid: agentid, pageindex: pageindex, pagesize: pageSize, key: key, startime: startime, endtime: endtime },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询列表错误");
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
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
     <table>
            <tr>
                <td class="tdHead" id="contentqq" style="font-size: 14px; height: 26px;">
                   <div class="left"><img id="comlogo" src="" class="" height="42"></div>
                    
                    <div class="left comleft">
                    <div ><span> 商户名称：
                    <%=company %> </span>
                    <span>授权类型：
                    <%=Warrant_type_str%>；</span> <span><%if (contact_phone != "")
                     {%>客服电话：<%=contact_phone %><%} %></span>
                     </div>
                      <div>
                      <%=yufukuan%>
                    <a class="a_anniu" href="Recharge.aspx?comid=<%=comid_temp %>" target="_blank">在线充值</a> <span id="Span1"
                        style="padding-left: 30px;"></span><span id="Span2" style="padding-left: 30px;">
                    </span>
                    </div>
                     </div>
                </td>
            </tr>
        </table>
        <div id="secondary-tabs" class="navsetting ">
            <ul class="composetab">
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_unsel"><div><a href="ProjectList.aspx?comid=<%=comid_temp %>">项目列表</a></div></div>
            </li>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_unsel"><div>
                <a href="Manage_sales.aspx?comid=<%=comid_temp%>">产品列表</a></div></div>
            </li>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img" style="z-index:2;"></div>
                <div class="composetab_unsel" style="position:relative;"><div>
                <a href="Order.aspx?comid=<%=comid_temp%>">订单记录</a>
                </div></div>
            </li>
            <%if (Warrant_type == 2)
                  { %>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img" style="z-index:2;"></div>
                <div class="composetab_sel" style="position:relative;"><div>
                <a href="EticketCount.aspx?comid=<%=comid_temp %>">验码统计</a>
                </div></div>
            </li>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img" style="z-index:2;"></div>
                <div class="composetab_unsel" style="position:relative;"><div>
                <a href="Verification.aspx?comid=<%=comid_temp %>">验码记录</a>
                </div></div>
            </li>
            <% } %>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img" style="z-index:2;"></div>
                <div class="composetab_unsel" style="position:relative;"><div>
                <a href="Finane.aspx?comid=<%=comid_temp %>">财务列表</a>
                </div></div>
            </li>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img" style="z-index:2;"></div>
                <div class="composetab_unsel" style="position:relative;"><div>
                <a href="EticketBack.aspx?comid=<%=comid_temp %>">退订/订单状态</a>
                </div></div>
            </li>
            <% if (ishaslvyoubusproorder == 1)
                   {%>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img" style="z-index:2;"></div>
                <div class="composetab_unsel" style="position:relative;"><div>
                <a href="/Agent/travelbusordercount.aspx?comid=<%=comid_temp %>">旅游大巴统计</a>
                </div></div>
            </li>
            <% } %>
         </ul>
         <div class="toolbg toolbgline toolheight nowrap" style="">
         <div class="right">
                   <label>
                        关键词查询
                        <input name="key" type="text" id="key" class="mi-input" style="width: 160px;" />
                    </label>
                      <label>日期查询  起始<input name="startime" type="text" id="startime"  isdate="yes"/>
                      截止<input name="endtime" type="text" id="endtime" isdate="yes"/>
                      </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 120px; height: 26px;" />
                    </label>
         </div>
        </div>

        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <table width="780" border="0" class="O2">
                    <tr  class="O2title">
                        <td width="51">
                            <p align="left">
                                流水号</p>
                        </td>
                        <td width="100">
                            <p align="left">
                                验证时间
                            </p>
                        </td>
                        <td width="150">
                            <p align="left">
                                服务内容
                            </p>
                      </td>
                        <td width="60">
                            <p align="left">
                                订单号
                            </p>
                      </td>
                        <td width="66">
                            <p align="left">
                                电子票
                            </p>
                      </td>
                        <td width="66">
                            <p align="left">
                                订购数量/使用数量
                            </p>
                      </td>
                        <td width="72">
                            <p align="left">
                               结算价格
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
    <script type="text/x-jquery-tmpl" id="ProductItemEdit">
                <tr  class="fontcolor d_out" onmouseover="this.className='d_over'" onmouseout="this.className='d_out'">
                        <td >
                            <p align="left">
                                ${Id}</p>
                        </td>
                        <td>
                            <p align="left">
                                ${jsonDateFormatKaler(Actiondate)}
                            </p>
                        </td>
                        <td>
                            <p align="left">
                                ${E_proname}</p>
                        </td>
                                                <td>
                            <p align="left">
                                ${Oid}</p>
                        </td>
                        <td >
                            <p align="left">
                                ${Pno}</p>
                        </td>
                        <td>
                            <p align="left">
                                ${Pnum}/${Use_pnum}</p>
                        </td>
                        <td>
                            <p align="left">
                            ${E_sale_price}
                            </p>
                        </td>
                    </tr>
                    
    </script>
    <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
    <input id="hid_comid_temp" type="hidden" value="<%=comid_temp %>" />
</asp:Content>