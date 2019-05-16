<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/Agent/Manage.Master"
    CodeBehind="Manage_sales.aspx.cs" Inherits="ETS2.WebApp.Agent.Manage_sales" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="/Scripts/shopcart.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageSize = 10; //每页显示条数
        $(function () {
            var agentid = $("#hid_agentid").trimVal();
            var comid = $("#hid_comid_temp").trimVal();
            var projectid = $("#hid_projectid").trimVal();

            SearchList(1);

            //装载产品列表
            function SearchList(pageindex) {
                var key = $("#key").trimVal();
                if (pageindex == '') {
                    $.prompt("请选择跳到的页数");
                    return;
                }

                $.post("/JsonFactory/AgentHandler.ashx?oper=warrantprolist", { pageindex: pageindex, pagesize: pageSize, agentid: agentid, comid: comid, key: key, projectid: projectid, viewmethod: "1,2" }, function (data) {

                    data = eval("(" + data + ")");

                    if (data.type == 1) {
                        $.prompt("查询列表错误");
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
                })
            }

            $("#Search").click(function () {
                SearchList(1);
            })

            $("html").die().live("keydown", function (event) {
                if (event.keyCode == 13) {
                    // $("#Search").click();    //这里添加要处理的逻辑  
                }
            });

            $("#0").click(function () {
                select(0, 1);
                $("#0").css("color", "red");
            })
            $("#1").click(function () {
                select(1, 1);
                $("#1").css("color", "red");
            })
            $("#3").click(function () {
                select(3, 1);
                $("#3").css("color", "red");
            })
            $("#4").click(function () {
                select(4, 1);
                $("#4").css("color", "red");
            })

            //分页 
            function setpage1(newcount, newpagesize, curpage, opval) {
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

                        select(opval, page);

                        return false;
                    }
                });
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
            $("#Proinfocancel").click(function () {
                $("#ProInfo").hide();
            })
            $("#closeProInfo").click(function () {
                $("#ProInfo").hide();
            })


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



        function Proadd(Pro_name, Pro_Remark, Service_Contain, Service_NotContain, Precautions) {
            $("#ProInfo").show();
            $("#Pro_name").html(Pro_name);
            $("#Pro_Remark").html(Pro_Remark);
            $("#Service_Contain").html(Service_Contain);
            $("#Service_NotContain").html(Service_NotContain);
            $("#Precautions").html(Precautions);

        }


        function addcart(proid) {
            alert("添加到购物车");
            $.post("/JsonFactory/OrderHandler.ashx?oper=agentaddcart", { agentid: $("#hid_agentid").trimVal(), comid: $("#hid_comid_temp").trimVal(), proid: proid, u_num: 1 }, function (data) {
                data = eval("(" + data + ")");
                if (data.type == 1) {
                }
                if (data.type == 100) {
                    $("#cart").show();
                    searchcart();
                }
            })
        }

        function viewsun(strid) {

               $("." + strid).show();
        }
    </script>
    <style type="text/css">
        #Service_Contain img
        {
            max-width: 400px;
        }
        

    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="body" runat="server">
    <div id="settings" class="view main">
        <table>
            <tr>
                <td class="tdHead" id="contentqq" style="font-size: 14px; height: 26px;">
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
                <div class="composetab_unsel"><div><a href="ProjectList.aspx?comid=<%=comid_temp %>">项目列表</a></div></div>
            </li>
            <li class="left" style="width:110px;padding-right:2px;">
                <div class="composetab_img"></div>
                <div class="composetab_sel"><div>
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
         <div class="right">
                    <%=projectname %>
                    <label>
                        关键词查询
                        <input name="key" type="text" id="key" class="mi-input" style="width: 160px;" />
                    </label>
                    <label>
                        <input name="Search" type="button" id="Search" value="查询" style="width: 60px; height: 26px;" />
                    </label>
         </div>
         <div class="nowrap left" unselectable="on" onselectstart="return false;">
         <!--<a class="btn_gray btn_space" hidefocus="" id="quick_del" href="javascript:;" name="del">删除</a>-->
         
         
         </div></div>

      
        </div>
        <div id="setting-home" class="vis-zone mail-list">
            <div class="inner">

                <table width="780" border="0" class="O2">
                    <tr class="O2title">
                        <td width="40" height="35">
                            <p align="left">
                               产品编号</p>
                        </td>
                        <td width="40">
                            <p align="left">
                                图片</p>
                        </td>

                        <td width="330">
                            <p align="left">
                                产品名称</p>
                        </td>
                        <td width="25">
                            淘
                        </td>
                        <td width="35">
                            <p align="left">
                                库存</p>
                        </td>
                        <td width="35">
                            门市价
                        </td>
                        <td width="35">
                            销售价
                        </td>
                        <td width="50">
                            结算价
                        </td>
                        <td width="120">
                        </td>
                        <td width="50">
                            <p align="left">
                                备注</p>
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
   
                <tr class="d_out" onclick="viewsun('guigelist${Id}')" onmouseover="this.className='d_over'" onmouseout="this.className='d_out'">
                        <td > 
                             
                                    <p> {{if Manyspeci==1}}--{{else}}${Id}{{/if}} </p>
                            
                        </td>
                        <td title="${Projectname}">
                            <p align="left">
                               {{if ProImg !=""}}
                                <img src="${ProImg}" height="30" width="30">
                              {{/if}}</p>
                        </td>
                        <td title="${Projectname} ${Pro_name}">
                            <p align="left" >
                              <a href="javascript:;" onclick="Proadd('${Pro_name}','${Pro_Remark}','${Service_Contain}','${Service_NotContain}','${Precautions}')"> 


                              ${Pro_name}   
                              {{if IsViewStockNum==1}}
                              
                            {{/if}}</a></p>
                        </td>
                        <td class="home-10">
                        {{if Server_type==1}}
                            {{if Bindingid ==0 }}
                                {{if Source_type=="自动生成"}}
                                    淘
                                {{/if}}
                            {{else}}
                                {{if BindingSource_type=="1"}}
                                    淘
                                {{/if}}
                            {{/if}}
                        {{/if}}
                        </td>
                          <td >
                            <p align="left">
                            {{if Ispanicbuy==0}}
                            --
                            {{else}}
                            ${StockNum}
                            {{/if}}
                              </p>
                        </td>

                        <td >
                            {{if Manyspeci==1}}
                                <span>--</span>                   
                            {{else}} 
                            ${Face_price}
                            {{/if}}
                        </td>
                        <td >
                            <p align="left">
                              {{if Manyspeci==1}}--{{else}}  ${Advise_price}{{/if}}</p>
                        </td>
                        <td >
                            <p align="left">
                              {{if Manyspeci==1}}--{{else}}  ${Agent_price}{{/if}}</p>
                        </td>

                        <td > 
                        {{if Agent_price>0}}
                            <% 
                                if (Warrant_type==1) {%>

                                {{if Ispanicbuy==0}}
                                 <a href="pro_sales.aspx?id=${Id}" class="qianggou_anniu">抢购</a>
                                {{else}}
                                 <a  href="pro_sales.aspx?id=${Id}" class="qianggou_anniu">订购</a>
                                {{/if}}
                                {{if Server_type==11}}
                                    {{if Manyspeci==0}}
                                 <a href="javascript:;" onClick="addcart('${Id}')" class="qianggou_anniu">加入购物车</a>
                                    {{/if}}
                                 {{/if}}

                                <%}else if(Warrant_type==3){ %>
                                 <a href="pro_sales.aspx?id=${Id}" class="qianggou_anniu">订购</a>
                                <%}else{ %>
                                {{if binding_Warrant_type==0}}
                                 <a  href="pro_sales.aspx?id=${Id}" class="qianggou_anniu">倒码</a>
                                {{else}}
                                只接受接口出票
                                {{/if}}
                                <%} 
                             %>
                             {{else}}

                                {{if Manyspeci==1}}
                                        <% 
                                        if (Warrant_type==1) {%>

                                        {{if Ispanicbuy==0}}
                                         <a href="pro_sales.aspx?id=${Id}" class="qianggou_anniu">抢购</a>
                                        {{else}}
                                         <a  href="pro_sales.aspx?id=${Id}" class="qianggou_anniu">订购</a>
                                        {{/if}}
                                        <%}else{ %>
                                        {{if binding_Warrant_type==0}}
                                         <a  href="pro_sales.aspx?id=${Id}" class="qianggou_anniu">订购</a>
                                        {{else}}
                                        只接受接口出票
                                        {{/if}}
                                        <%} 
                                     %>
                                {{else}}

                                  已售完
                                {{/if}}
                         {{/if}}
                        </td>
                        <td  title="${Pro_explain}">
                            <p align="left">
                                ${Pro_explain.length>8?Pro_explain.substring(0,8)+"..":Pro_explain}</p>
                        </td>
                    </tr>
                   {{if Manyspeci==1}}
                        {{each(i,user) GuigeList}}
                         <tr class="d_out guigelist${Id}" style="display: none;background-color: #cccccc;" onmouseover="this.className='d_over'" onmouseout="this.className='d_out'">
                            <td > 
                            ${Id}-${user.id}
                            </td>
                            <td>
                            
                            </td>
                            <td title="${Projectname} ${Pro_name}${user.speci_name}">
                             ${Pro_name}${user.speci_name}
                            </td>
                            <td class="home-10">
                            </td>
                            <td >
                            </td>
                            <td >
                            ${user.speci_face_price}
                            </td>
                            <td >
                            ${user.speci_advise_price}
                            </td>
                            <td >
                           ${user.speci_agentsettle_price}
                            </td>
                            <td > 
                            </td>
                            <td>
                            </td>
                         </tr>
                        {{/each}}
                     {{/if}}
                      
                    
    </script>
    <div id="ProInfo" style="background-color: #ffffff; border: 2px solid #5984bb; margin: 0px auto;
        width: 550px; height: 350px; display: none; left: 20%; position: absolute; top: 20%;
        overflow: auto;">
        <table width="100%" border="0" cellpadding="10" cellspacing="1" bgcolor="#999999"
            style="padding: 5px;">
            <tr>
                <td height="42" colspan="2" bgcolor="#C1D9F3">
                    <span class="STYLE1">&nbsp;&nbsp;&nbsp;&nbsp;产品介绍</span><span style="float: right;
                        font-size: 18px; padding-right: 10px; cursor: pointer;" id="closeProInfo">X</span>
                </td>
            </tr>
            <tr>
                <td width="80" height="30" bgcolor="#E7F0FA">
                    &nbsp;&nbsp;&nbsp;&nbsp;产品名称:
                </td>
                <td height="30" bgcolor="#E7F0FA">
                    <span id="Pro_name"></span>
                </td>
            </tr>
            <tr>
                <td height="30" bgcolor="#E7F0FA" class="tdHead">
                    &nbsp;&nbsp;&nbsp;&nbsp;备注:
                </td>
                <td height="30" bgcolor="#E7F0FA" class="tdHead">
                    <span id="Pro_Remark"></span>
                </td>
            </tr>
            <tr>
                <td height="30" bgcolor="#E7F0FA">
                    <span id="Span1"></span>
                </td>
                <td height="30" bgcolor="#E7F0FA">
                    <span id="Service_Contain"></span>
                </td>
            </tr>
            <tr>
                <td height="30" bgcolor="#E7F0FA" class="tdHead">
                </td>
                <td height="30" bgcolor="#E7F0FA" class="tdHead">
                    <span id="Service_NotContain"></span>
                </td>
            </tr>
            <tr>
                <td height="30" bgcolor="#E7F0FA" class="tdHead">
                </td>
                <td height="30" bgcolor="#E7F0FA" class="tdHead">
                    <span id="Precautions"></span>
                </td>
            </tr>
            <tr>
                <td height="30" bgcolor="#E7F0FA" class="tdHead">
                </td>
                <td height="30" bgcolor="#E7F0FA" class="tdHead">
                    <input name="Proinfocancel" id="Proinfocancel" type="button" class="formButton" value="  关  闭  " />
                </td>
            </tr>
        </table>
    </div>
    <div id='cart' style="display: none; position: absolute; bottom: 6em; right: 2em;
        width: 55px; height: 55px; background-color: #FFFAFA; margin: 10px; border-radius: 60px;
        border: solid rgb(55,55,55)  #FF0000   1px; cursor: pointer;">
        <a href="ShopCart.aspx?comid=<%=comid_temp %>">
            <img src="/images/cart.gif" width="39" style="padding: 8px 0 0 9px;" /></a>
    </div>
    <input id="hid_agentid" type="hidden" value="<%=Agentid %>" />
    <input id="hid_comid_temp" type="hidden" value="<%=comid_temp %>" />
    <input id="hid_projectid" type="hidden" value="<%=projectid %>" />
</asp:Content>
