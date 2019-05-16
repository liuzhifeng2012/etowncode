<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Manage.Master"  CodeBehind="ProjectList.aspx.cs" Inherits="ETS2.WebApp.Agent.ProjectList" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            var agentid = $("#hid_agentid").trimVal();
            var comid = $("#hid_comid_temp").trimVal();
            SearchList(1);

            //装载列表
            function SearchList(pageindex) {
                var key = $("#key").val();
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/JsonFactory/AgentHandler.ashx?oper=warrantprojectlist",
                    data: { pageindex: pageindex, pagesize: pageSize, agentid: agentid, comid: comid, key: key },
                    async: false,
                    success: function (data) {
                        data = eval("(" + data + ")");

                        if (data.type == 1) {
                            $.prompt("查询错误");
                            return;
                        }
                        if (data.type == 100) {
                            $("#tblist").empty();
                            $("#divPage").empty();
                            if (data.totalCount == 0) {
                                $("#tblist").html("<tr><td colspan='15'>查询数据为空</td></tr>");
                            } else {
                                $("#ProductItemEdit").tmpl(data.msg).appendTo("#tblist");
                                setpage(data.totalCount, pageSize, pageindex);
                            }


                        }
                    }
                })

            }

            //查询
            $("#Search").click(function () {
                SearchList(1);
            })

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



            //加载qq
            $("#loading").show();
            $.ajax({
                type: "post",
                url: "/JsonFactory/CrmMemberHandler.ashx?oper=channelqqList",
                data: { comid: comid, pageindex: 1, pagesize: 12 },
                async: false,
                success: function (data) {
                    data = eval("(" + data + ")");
                    $("#loading").hide();
                    if (data.type == 1) {
                        return;
                    }
                    if (data.type == 100) {
                        $("#loading").hide();
                        var qqstr = "";
                        for (var i = 0; i < data.msg.length; i++) {
                            qqstr += '<a target="_blank" href="http://wpa.qq.com/msgrd?v=3&amp;uin=' + data.msg[i].QQ + '&amp;site=qq&amp;menu=yes"><img style="vertical-align:bottom;padding-left:5px;" src="/images/qq.png" alt="' + data.msg[i].QQ + '" title="' + data.msg[i].QQ + '" border="0"></a>'
                        }
                        $("#contentqq").append(qqstr);

                    }
                }
            })
        })
    </script>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
                        <table>
                    <tr>
                        <td class="tdHead" id="contentqq" style="font-size:14px; height:26px;">
                            <div class="left">
                    <img id="comlogo" src="" class="" height="42"></div>
                    
                    <div class="left comleft">
                    <div ><span> 商户名称：
                    <%=company %> </span>
                    <span>授权类型：
                    <%=Warrant_type_str%>；</span> <span><%if (contact_phone != "")
                     {%>客服电话：<%=contact_phone %><%} %></span>
                     </div>
                      <div>
                      <%=yufukuan%>
                    <a class="a_anniu" href="Recharge.aspx?comid=<%=comid_temp %>" target="_blank">在线充值</a> <span id="messagenew"
                        style="padding-left: 30px;"></span><span id="shopcart" style="padding-left: 30px;">
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
                <div class="composetab_sel"><div><a href="ProjectList.aspx?comid=<%=comid_temp %>">项目列表</a></div></div>
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
                <div class="composetab_unsel" style="position:relative;"><div>
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
         <div class="right searchtool">
                 <span>&nbsp;</span>   
         </div>
         <div class="nowrap left" unselectable="on" onselectstart="return false;">
         <!--<a class="btn_gray btn_space" hidefocus="" id="quick_del" href="javascript:;" name="del">删除</a>-->
         
         
         </div></div>
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">
                <table width="780" border="0" class="O2">
                    <tbody>
                        <tr class="O2title">
                            <td width="10">
                                <p align="left">
                                    ID</p>
                            </td>
                            <td width="200">
                                <p align="left">
                                    项目名称
                                </p>
                            </td>
                            <td width="50">
                                <p align="left">
                                    
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

    </div>
    <div class="data">
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
                                   <a class="a_anniu" href="Manage_sales.aspx?projectid=${Id}&comid=<%=comid_temp %>">进入产品购买</a> 
                                </p>
                            </td>
                        </tr>
        </script>
            <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
    <input id="hid_comid_temp" type="hidden" value="<%=comid_temp %>" />
</asp:Content>
